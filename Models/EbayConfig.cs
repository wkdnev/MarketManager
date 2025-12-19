namespace MarketManager.Models;

/// <summary>
/// Configuration model for eBay API settings
/// </summary>
public class EbayConfig
{
    /// <summary>
    /// eBay Application ID (Client ID) for OAuth 2.0
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// eBay Application Secret (Client Secret) for OAuth 2.0
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// OAuth 2.0 Redirect URI for authentication callback
    /// </summary>
    public string RedirectUri { get; set; } = "https://localhost:5001/ebay/callback";

    /// <summary>
    /// eBay API Environment: PRODUCTION or SANDBOX
    /// </summary>
    public string Environment { get; set; } = "SANDBOX";

    /// <summary>
    /// Refresh Token for maintaining persistent API access
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Base URL for eBay API endpoints based on environment
    /// </summary>
    public string BaseUrl => Environment.ToUpper() == "PRODUCTION" 
        ? "https://api.ebay.com" 
        : "https://api.sandbox.ebay.com";

    /// <summary>
    /// OAuth Authorization URL
    /// </summary>
    public string AuthUrl => Environment.ToUpper() == "PRODUCTION"
        ? "https://auth.ebay.com/oauth2/authorize"
        : "https://auth.sandbox.ebay.com/oauth2/authorize";

    /// <summary>
    /// OAuth Token URL
    /// </summary>
    public string TokenUrl => Environment.ToUpper() == "PRODUCTION"
        ? "https://api.ebay.com/identity/v1/oauth2/token"
        : "https://api.sandbox.ebay.com/identity/v1/oauth2/token";
}
