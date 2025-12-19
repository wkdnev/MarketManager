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
   http://localhost:5085/ebay/callback
   ```
   
2. Note down your credentials:
   - **App ID (Client ID)**: Your application ID
   - **Cert ID (Client Secret)**: Your certificate ID

### 4. Configure the Application

**The application reads all sensitive configuration from environment variables for security.**

#### Option A: Using .env file (Recommended)

1. Copy the example environment file:
   ```bash
   cp .env.example .env
   ```

2. Edit `.env` and add your eBay credentials:
   ```bash
   EBAY_CLIENT_ID=your_ebay_app_id_here
   EBAY_CLIENT_SECRET=your_ebay_cert_id_here
   EBAY_REDIRECT_URI=http://localhost:5085/ebay/callback
   EBAY_ENVIRONMENT=SANDBOX
   EBAY_REFRESH_TOKEN=
   ```

3. Load environment variables before running:
   ```bash
   source load-env.sh
   dotnet run
   ```

#### Option B: Set environment variables directly

**macOS/Linux:**
```bash
export EBAY_CLIENT_ID="your_app_id"
export EBAY_CLIENT_SECRET="your_cert_id"
export EBAY_REDIRECT_URI="http://localhost:5085/ebay/callback"
export EBAY_ENVIRONMENT="SANDBOX"
dotnet run
```

**Windows (PowerShell):**
```powershell
$env:EBAY_CLIENT_ID="your_app_id"
$env:EBAY_CLIENT_SECRET="your_cert_id"
$env:EBAY_REDIRECT_URI="http://localhost:5085/ebay/callback"
$env:EBAY_ENVIRONMENT="SANDBOX"
dotnet run
```

**Windows (CMD):**
```cmd
set EBAY_CLIENT_ID=your_app_id
set EBAY_CLIENT_SECRET=your_cert_id
set EBAY_REDIRECT_URI=http://localhost:5085/ebay/callback
set EBAY_ENVIRONMENT=SANDBOX
dotnet run
```

#### Option C: Fallback to appsettings.json (Not Recommended)

If no environment variables are set, the app will fall back to `appsettings.json`. However, **never commit credentials to appsettings.json**.
```

### 5. Run the Application

```bash
# Load environment variables (if using .env file)
source load-env.sh

# Run the application
dotnet run
```

The application will start at `http://localhost:5085`

### 6. Authenticate with eBay

1. Navigate to the home page at http://localhost:5085
2. Click **"Connect Now"** button
3. You'll be redirected to eBay's authorization page
4. Sign in with your **sandbox user account** (not your developer account)
5. Grant the requested permissions
6. You'll be redirected back to the application

After successful authentication, the refresh token will be displayed in the console. For persistent access, add this token to your environment:

**Using .env file:**
```bash
# Edit .env file
EBAY_REFRESH_TOKEN=your_refresh_token_here
```

**Using environment variable:**
```bash
export EBAY_REFRESH_TOKEN="your_refresh_token_here"
```
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

## Environment Variables

All sensitive configuration is managed through environment variables for security:

| Variable | Description | Required | Default |
|----------|-------------|----------|---------|
| `EBAY_CLIENT_ID` | Your eBay App ID (Client ID) | Yes | - |
| `EBAY_CLIENT_SECRET` | Your eBay Cert ID (Client Secret) | Yes | - |
| `EBAY_REDIRECT_URI` | OAuth callback URL | No | `http://localhost:5085/ebay/callback` |
| `EBAY_ENVIRONMENT` | `SANDBOX` or `PRODUCTION` | No | `SANDBOX` |
| `EBAY_REFRESH_TOKEN` | Long-lived refresh token | No | - |

**Configuration Priority:**
1. Environment variables (highest priority)
2. appsettings.json (fallback only)

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
├── .env.example          # Environment variables template
├── load-env.sh           # Script to load environment variables
├── appsettings.json      # Base configuration (no secrets)
└── Program.cs            # Application entry point with env var support
```

## Development vs Production

### Sandbox (Development)
- Set `EBAY_ENVIRONMENT=SANDBOX`
- Use sandbox credentials
- Base URL: `https://api.sandbox.ebay.com`
- Auth URL: `https://auth.sandbox.ebay.com`
- Test with sandbox user accounts

### Production
- Set `EBAY_ENVIRONMENT=PRODUCTION`
- Use production credentials
- Base URL: `https://api.ebay.com`
- Auth URL: `https://auth.ebay.com`
- Real transactions and data

## Troubleshooting

### "Not Connected" Error
- Verify `EBAY_CLIENT_ID` and `EBAY_CLIENT_SECRET` environment variables are set
- Run `source load-env.sh` if using .env file
- Check that the redirect URI matches exactly (including protocol and port)
- Verify the redirect URI is configured in eBay developer portal

### "Invalid Credentials" Error
- Make sure environment variables are loaded correctly
- Verify you're using sandbox credentials for sandbox environment
- Check the credentials are not expired or revoked

### "Authorization Failed" Error
- Ensure you're logging in with a sandbox **user account**, not developer account
- Check that all required scopes are granted
- Verify redirect URI matches between app and eBay portal

### Environment Variables Not Loading
- If using .env file, run `source load-env.sh` before `dotnet run`
- Check .env file exists and is in the project root
- Verify environment variables are exported: `echo $EBAY_CLIENT_ID`

### Token Refresh Issues
- Refresh tokens are valid for 18 months
- Store refresh token in `EBAY_REFRESH_TOKEN` environment variable
- Store refresh tokens securely
- Implement token storage in a database for production use

## Security Notes

1. ✅ **All credentials use environment variables** - Never commit credentials to source control
2. ✅ **`.env` file ignored** - The .gitignore prevents accidental commits of .env files
3. ✅ **Configuration priority** - Environment variables override appsettings.json
4. 🔒 **Use encrypted storage** - Store refresh tokens in encrypted database for production
5. 🔒 **Validate state parameter** - Implement CSRF protection in OAuth flow
6. 🔒 **Use HTTPS** - Always use HTTPS for OAuth redirects in production
7. 🔒 **Rotate credentials** - Regularly rotate API keys and refresh tokens
8. 🔒 **Monitor access** - Log and monitor API usage and authentication events

**For Production Deployment:**
- Use Azure Key Vault, AWS Secrets Manager, or similar for secrets
- Set environment variables through your hosting platform
- Never use .env files in production
- Implement proper logging and monitoring

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
