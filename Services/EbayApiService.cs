using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MarketManager.Models;

namespace MarketManager.Services;

/// <summary>
/// Service for interacting with eBay REST APIs
/// </summary>
public class EbayApiService
{
    private readonly HttpClient _httpClient;
    private readonly EbayConfig _config;
    private readonly EbayAuthService _authService;

    public EbayApiService(
        HttpClient httpClient, 
        IOptions<EbayConfig> config, 
        EbayAuthService authService)
    {
        _httpClient = httpClient;
        _config = config.Value;
        _authService = authService;
    }

    /// <summary>
    /// Search for items on eBay
    /// </summary>
    public async Task<string> SearchItemsAsync(string query, int limit = 10)
    {
        var accessToken = await _authService.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(accessToken))
        {
            return "Not authenticated. Please authenticate with eBay first.";
        }

        try
        {
            var url = $"{_config.BaseUrl}/buy/browse/v1/item_summary/search?q={Uri.EscapeDataString(query)}&limit={limit}";
            
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return content;
            }
            else
            {
                return $"Error: {response.StatusCode} - {content}";
            }
        }
        catch (Exception ex)
        {
            return $"Exception: {ex.Message}";
        }
    }

    /// <summary>
    /// Get user's active listings
    /// </summary>
    public async Task<string> GetActiveListingsAsync()
    {
        var accessToken = await _authService.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(accessToken))
        {
            return "Not authenticated. Please authenticate with eBay first.";
        }

        try
        {
            var url = $"{_config.BaseUrl}/sell/inventory/v1/inventory_item";
            
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return content;
            }
            else
            {
                return $"Error: {response.StatusCode} - {content}";
            }
        }
        catch (Exception ex)
        {
            return $"Exception: {ex.Message}";
        }
    }

    /// <summary>
    /// Get user's orders
    /// </summary>
    public async Task<string> GetOrdersAsync()
    {
        var accessToken = await _authService.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(accessToken))
        {
            return "Not authenticated. Please authenticate with eBay first.";
        }

        try
        {
            var url = $"{_config.BaseUrl}/sell/fulfillment/v1/order";
            
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return content;
            }
            else
            {
                return $"Error: {response.StatusCode} - {content}";
            }
        }
        catch (Exception ex)
        {
            return $"Exception: {ex.Message}";
        }
    }

    /// <summary>
    /// Get trending searches and market insights
    /// </summary>
    public async Task<string> GetTrendingSearchesAsync()
    {
        var accessToken = await _authService.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(accessToken))
        {
            return "Not authenticated. Please authenticate with eBay first.";
        }

        try
        {
            // Note: This would use eBay's Marketing API for trending data
            var url = $"{_config.BaseUrl}/sell/marketing/v1/item_promotion";
            
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return content;
            }
            else
            {
                return $"Error: {response.StatusCode} - {content}";
            }
        }
        catch (Exception ex)
        {
            return $"Exception: {ex.Message}";
        }
    }

    /// <summary>
    /// Get item details by item ID
    /// </summary>
    public async Task<string> GetItemDetailsAsync(string itemId)
    {
        var accessToken = await _authService.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(accessToken))
        {
            return "Not authenticated. Please authenticate with eBay first.";
        }

        try
        {
            var url = $"{_config.BaseUrl}/buy/browse/v1/item/{itemId}";
            
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return content;
            }
            else
            {
                return $"Error: {response.StatusCode} - {content}";
            }
        }
        catch (Exception ex)
        {
            return $"Exception: {ex.Message}";
        }
    }
}
