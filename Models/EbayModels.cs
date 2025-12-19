namespace MarketManager.Models;

/// <summary>
/// Model for OAuth 2.0 token response from eBay
/// </summary>
public class EbayTokenResponse
{
    public string access_token { get; set; } = string.Empty;
    public string token_type { get; set; } = string.Empty;
    public int expires_in { get; set; }
    public string? refresh_token { get; set; }
    public string? refresh_token_expires_in { get; set; }
}

/// <summary>
/// Model for eBay API error responses
/// </summary>
public class EbayError
{
    public string? ErrorId { get; set; }
    public string? Domain { get; set; }
    public string? Category { get; set; }
    public string? Message { get; set; }
    public string? LongMessage { get; set; }
}
