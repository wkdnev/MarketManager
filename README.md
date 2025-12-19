# MarketManager - eBay Marketplace Management

A comprehensive ASP.NET Blazor application for managing your eBay marketplace activities including buying, selling, inventory management, and analytics.

## Features

- 🔐 **OAuth 2.0 Authentication** - Secure connection to eBay Developer Sandbox
- 🛒 **Buying Tools** - Search deals, manage watchlist, track purchases
- 📦 **Selling Tools** - Create listings, manage inventory, fulfill orders
- 📈 **Analytics** - Track sales performance and market trends
- 🎯 **Real-time Updates** - Interactive Blazor Server components

## Prerequisites

- .NET 10.0 SDK or later
- eBay Developer Account
- Visual Studio 2024 or VS Code

## Getting Started

### 1. Create an eBay Developer Account

1. Go to [eBay Developer Program](https://developer.ebay.com)
2. Sign up for a free developer account
3. Create a sandbox user account for testing

### 2. Create an eBay Application

1. Log in to your eBay Developer account
2. Navigate to **My Account** → **Applications**
3. Click **Create an Application**
4. Choose **Production/Sandbox** keys
5. Fill in the application details:
   - **Application Title**: MarketManager (or your choice)
   - **Application Type**: Select appropriate type

### 3. Configure OAuth Settings

1. In your eBay application settings, add the OAuth redirect URI:
   ```
   https://localhost:5001/ebay/callback
   ```
   
2. Note down your credentials:
   - **App ID (Client ID)**: Your application ID
   - **Cert ID (Client Secret)**: Your certificate ID

### 4. Configure the Application

1. Open `appsettings.json` in the project root
2. Update the eBay configuration:

```json
{
  "EbayConfig": {
    "ClientId": "YOUR_CLIENT_ID_HERE",
    "ClientSecret": "YOUR_CLIENT_SECRET_HERE",
    "RedirectUri": "https://localhost:5001/ebay/callback",
    "Environment": "SANDBOX",
    "RefreshToken": ""
  }
}
```

**Important:** Never commit your credentials to source control. Use user secrets for development:

```bash
dotnet user-secrets init
dotnet user-secrets set "EbayConfig:ClientId" "YOUR_CLIENT_ID"
dotnet user-secrets set "EbayConfig:ClientSecret" "YOUR_CLIENT_SECRET"
```

### 5. Run the Application

```bash
dotnet run
```

The application will start at `https://localhost:5001`

### 6. Authenticate with eBay

1. Navigate to the home page
2. Click **"Connect Now"** button
3. You'll be redirected to eBay's authorization page
4. Sign in with your **sandbox user account** (not your developer account)
5. Grant the requested permissions
6. You'll be redirected back to the application

After successful authentication, the refresh token will be displayed in the console. For persistent access, add this token to your `appsettings.json`:

```json
{
  "EbayConfig": {
    "RefreshToken": "YOUR_REFRESH_TOKEN_HERE"
  }
}
```

## OAuth 2.0 Flow

The application implements the OAuth 2.0 Authorization Code Grant flow:

1. **User Initiates**: User clicks "Connect to eBay"
2. **Authorization Request**: App redirects to eBay with client ID and scopes
3. **User Consents**: User logs in and grants permissions
4. **Authorization Code**: eBay redirects back with authorization code
5. **Token Exchange**: App exchanges code for access token and refresh token
6. **API Access**: App uses access token to make API calls
7. **Token Refresh**: App uses refresh token to get new access tokens when expired

## API Scopes

The application requests the following OAuth scopes:

- `https://api.ebay.com/oauth/api_scope` - General API access
- `https://api.ebay.com/oauth/api_scope/sell.inventory` - Manage inventory
- `https://api.ebay.com/oauth/api_scope/sell.inventory.readonly` - Read inventory
- `https://api.ebay.com/oauth/api_scope/sell.fulfillment` - Manage orders
- `https://api.ebay.com/oauth/api_scope/sell.fulfillment.readonly` - Read orders
- `https://api.ebay.com/oauth/api_scope/sell.marketing` - Manage marketing
- `https://api.ebay.com/oauth/api_scope/sell.marketing.readonly` - Read marketing
- `https://api.ebay.com/oauth/api_scope/sell.account` - Manage account
- `https://api.ebay.com/oauth/api_scope/sell.account.readonly` - Read account
- `https://api.ebay.com/oauth/api_scope/commerce.catalog.readonly` - Read catalog

## Project Structure

```
MarketManager/
├── Components/
│   ├── Layout/           # Layout components
│   ├── Pages/            # Razor pages
│   │   ├── Home.razor           # Dashboard
│   │   ├── EbaySettings.razor   # OAuth settings
│   │   └── EbayCallback.razor   # OAuth callback handler
├── Models/
│   ├── EbayConfig.cs     # Configuration model
│   └── EbayModels.cs     # API response models
├── Services/
│   ├── EbayAuthService.cs   # OAuth 2.0 implementation
│   └── EbayApiService.cs    # eBay API wrapper
├── wwwroot/              # Static files
├── appsettings.json      # Configuration
└── Program.cs            # Application entry point
```

## Development vs Production

### Sandbox (Development)
- Use sandbox credentials
- Base URL: `https://api.sandbox.ebay.com`
- Auth URL: `https://auth.sandbox.ebay.com`
- Test with sandbox user accounts

### Production
- Update `Environment` to `"PRODUCTION"` in config
- Use production credentials
- Base URL: `https://api.ebay.com`
- Auth URL: `https://auth.ebay.com`
- Real transactions and data

## Troubleshooting

### "Not Connected" Error
- Verify Client ID and Client Secret are correct
- Ensure redirect URI matches exactly (including https://)
- Check that the redirect URI is configured in eBay developer portal

### "Invalid Credentials" Error
- Make sure you're using sandbox credentials for sandbox environment
- Verify the credentials are not expired

### "Authorization Failed" Error
- Ensure you're logging in with a sandbox **user account**, not developer account
- Check that all required scopes are granted

### Token Refresh Issues
- Refresh tokens are valid for 18 months
- Store refresh tokens securely
- Implement token storage in a database for production use

## Security Notes

1. **Never commit credentials** to source control
2. **Use user secrets** for development
3. **Use environment variables** or Azure Key Vault for production
4. **Store refresh tokens securely** in an encrypted database
5. **Validate state parameter** to prevent CSRF attacks
6. **Use HTTPS** for all OAuth redirects

## Next Steps

1. Implement persistent token storage (database)
2. Add state parameter validation for OAuth security
3. Create UI pages for buying/selling features
4. Implement error handling and logging
5. Add unit tests for services
6. Deploy to production with proper secrets management

## Resources

- [eBay Developer Documentation](https://developer.ebay.com/docs)
- [eBay OAuth 2.0 Guide](https://developer.ebay.com/api-docs/static/oauth-tokens.html)
- [eBay API Explorer](https://developer.ebay.com/my/api_test_tool)
- [Blazor Documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/)

## License

This project is licensed under the MIT License.
