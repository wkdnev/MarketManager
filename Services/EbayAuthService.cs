using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MarketManager.Models;

namespace MarketManager.Services;

/// <summary>
/// Service for handling eBay OAuth 2.0 authentication
/// </summary>
public class EbayAuthService
{
    private readonly HttpClient _httpClient;
    private readonly EbayConfig _config;
    private string? _accessToken;
    private DateTime _tokenExpiryTime;

    public EbayAuthService(HttpClient httpClient, IOptions<EbayConfig> config)
    {
        _httpClient = httpClient;
        _config = config.Value;
    }

    /// <summary>
    /// Get the authorization URL for user consent flow
    /// </summary>
    public string GetAuthorizationUrl(string state, string[] scopes)
    {
        var scopeString = string.Join(" ", scopes);
        return $"{_config.AuthUrl}?client_id={_config.ClientId}" +
               $"&response_type=code" +
               $"&redirect_uri={Uri.EscapeDataString(_config.RedirectUri)}" +
               $"&scope={Uri.EscapeDataString(scopeString)}" +
               $"&state={state}";
    }

    /// <summary>
    /// Exchange authorization code for access and refresh tokens
    /// </summary>
    public async Task<EbayTokenResponse?> ExchangeCodeForTokenAsync(string authorizationCode)
    {
        try
        {
            var credentials = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{_config.ClientId}:{_config.ClientSecret}"));

            var requestBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", authorizationCode),
                new KeyValuePair<string, string>("redirect_uri", _config.RedirectUri)
            });

            var request = new HttpRequestMessage(HttpMethod.Post, _config.TokenUrl)
            {
                Content = requestBody
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<EbayTokenResponse>(content);

            if (tokenResponse != null)
            {
                _accessToken = tokenResponse.access_token;
                _tokenExpiryTime = DateTime.UtcNow.AddSeconds(tokenResponse.expires_in - 60); // 60 second buffer
            }

            return tokenResponse;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error exchanging code for token: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Get access token using refresh token
    /// </summary>
    public async Task<string?> GetAccessTokenAsync()
    {
        // Return cached token if still valid
        if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _tokenExpiryTime)
        {
            return _accessToken;
        }

        // Refresh token if we have one
        if (!string.IsNullOrEmpty(_config.RefreshToken))
        {
            var tokenResponse = await RefreshAccessTokenAsync(_config.RefreshToken);
            if (tokenResponse != null)
            {
                _accessToken = tokenResponse.access_token;
                _tokenExpiryTime = DateTime.UtcNow.AddSeconds(tokenResponse.expires_in - 60);
                return _accessToken;
            }
        }

        // If no valid token available, return null (user needs to authenticate)
        return null;
    }

    /// <summary>
    /// Refresh the access token using a refresh token
    /// </summary>
    public async Task<EbayTokenResponse?> RefreshAccessTokenAsync(string refreshToken)
    {
        try
        {
            var credentials = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{_config.ClientId}:{_config.ClientSecret}"));

            var requestBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("scope", GetDefaultScopes())
            });

            var request = new HttpRequestMessage(HttpMethod.Post, _config.TokenUrl)
            {
                Content = requestBody
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<EbayTokenResponse>(content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error refreshing token: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Get default OAuth scopes for eBay API
    /// </summary>
    private string GetDefaultScopes()
    {
        return string.Join(" ", new[]
        {
            "https://api.ebay.com/oauth/api_scope",
            "https://api.ebay.com/oauth/api_scope/sell.marketing.readonly",
            "https://api.ebay.com/oauth/api_scope/sell.marketing",
            "https://api.ebay.com/oauth/api_scope/sell.inventory.readonly",
            "https://api.ebay.com/oauth/api_scope/sell.inventory",
            "https://api.ebay.com/oauth/api_scope/sell.account.readonly",
            "https://api.ebay.com/oauth/api_scope/sell.account",
            "https://api.ebay.com/oauth/api_scope/sell.fulfillment.readonly",
            "https://api.ebay.com/oauth/api_scope/sell.fulfillment",
            "https://api.ebay.com/oauth/api_scope/commerce.catalog.readonly",
        });
    }

    /// <summary>
    /// Check if user is authenticated
    /// </summary>
    public bool IsAuthenticated()
    {
        return !string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _tokenExpiryTime;
    }
}
