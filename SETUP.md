# eBay OAuth 2.0 Setup Guide

## Quick Start Checklist

- [ ] Create eBay Developer account
- [ ] Create application in eBay Developer Portal
- [ ] Configure OAuth redirect URI
- [ ] Add credentials to appsettings.json
- [ ] Run the application
- [ ] Complete OAuth authentication flow

## Step-by-Step Instructions

### Step 1: Create eBay Developer Account

1. Visit https://developer.ebay.com
2. Click "Register" in the top right corner
3. Complete the registration form
4. Verify your email address
5. Accept the eBay Developers Program License Agreement

### Step 2: Create Sandbox User Account

1. Log in to eBay Developer Portal
2. Navigate to **Dashboard** → **Sandbox**
3. Click **Create a Sandbox User**
4. Fill in the required information
5. Note down the username and password (you'll need this for testing)

### Step 3: Create an Application

1. In eBay Developer Portal, go to **My Account** → **Applications**
2. Click **Create an Application**
3. Fill in the application details:
   - **Application Title**: MarketManager
   - **Description**: eBay marketplace management application
   - **Category**: Choose appropriate category
4. Click **Create**

### Step 4: Get Your Credentials

After creating the application, you'll see:

**Sandbox Environment:**
- **App ID (Client ID)**: Copy this value
- **Cert ID (Client Secret)**: Copy this value

### Step 5: Configure OAuth Redirect URI

1. In your application settings, find the **OAuth Redirect URIs** section
2. Add the following URI:
   ```
   https://localhost:5001/ebay/callback
   ```
3. Click **Save** or **Add**

**Important:** The redirect URI must match exactly, including:
- Protocol (https://)
- Domain (localhost)
- Port (:5001)
- Path (/ebay/callback)

### Step 6: Configure the Application

#### Option A: Using appsettings.json (Quick Setup)

1. Open `/appsettings.json` in your project
2. Replace the placeholder values:

```json
{
  "EbayConfig": {
    "ClientId": "YOUR_APP_ID_HERE",
    "ClientSecret": "YOUR_CERT_ID_HERE",
    "RedirectUri": "https://localhost:5001/ebay/callback",
    "Environment": "SANDBOX",
    "RefreshToken": ""
  }
}
```

**⚠️ Warning:** Don't commit this file with your credentials to git!

#### Option B: Using User Secrets (Recommended for Development)

1. Open a terminal in the project directory
2. Initialize user secrets:
   ```bash
   dotnet user-secrets init
   ```

3. Set your credentials:
   ```bash
   dotnet user-secrets set "EbayConfig:ClientId" "YOUR_APP_ID"
   dotnet user-secrets set "EbayConfig:ClientSecret" "YOUR_CERT_ID"
   ```

### Step 7: Run the Application

1. Open a terminal in the project directory
2. Run the application:
   ```bash
   dotnet run
   ```

3. Open your browser and navigate to:
   ```
   https://localhost:5001
   ```

### Step 8: Authenticate with eBay

1. On the home page, click **"Connect Now"** button
2. You'll be redirected to eBay's authorization page
3. **Important:** Log in with your **sandbox user account**, NOT your developer account
4. Review the requested permissions
5. Click **"Agree"** to grant permissions
6. You'll be redirected back to the application

### Step 9: Save Refresh Token (Optional)

After successful authentication:

1. Check the console output for the refresh token
2. Copy the refresh token value
3. Add it to your configuration:

```json
{
  "EbayConfig": {
    "RefreshToken": "YOUR_REFRESH_TOKEN_HERE"
  }
}
```

**Note:** Refresh tokens are valid for 18 months and allow persistent access without re-authentication.

## Testing the Connection

Once authenticated, you should see:
- ✅ Green "Connected to eBay" status on the dashboard
- Access to all marketplace management features
- Ability to make API calls to eBay

## Common Issues and Solutions

### Issue: "Invalid Client Credentials"
**Solution:** 
- Verify you copied the App ID and Cert ID correctly
- Make sure you're using Sandbox credentials for Sandbox environment
- Check there are no extra spaces or characters

### Issue: "Redirect URI Mismatch"
**Solution:**
- Ensure the redirect URI in your code matches exactly what's in eBay Developer Portal
- Check for trailing slashes, protocol (https), and port number
- URI must be: `https://localhost:5001/ebay/callback`

### Issue: "Authentication Failed - Wrong User"
**Solution:**
- You must use a **sandbox user account** to test, not your developer account
- Create a sandbox user in the eBay Developer Portal if you haven't already
- Log out and try again with the correct account

### Issue: "SSL Certificate Error"
**Solution:**
- For development, trust the ASP.NET Core development certificate:
  ```bash
  dotnet dev-certs https --trust
  ```

### Issue: "Access Token Expired"
**Solution:**
- This is normal - access tokens expire after 2 hours
- The application will automatically refresh using the refresh token
- Make sure you have a valid refresh token configured

## Moving to Production

When ready to deploy to production:

1. **Get Production Credentials:**
   - In eBay Developer Portal, switch to Production keys
   - Copy your production App ID and Cert ID

2. **Update Configuration:**
   ```json
   {
     "EbayConfig": {
       "Environment": "PRODUCTION",
       "ClientId": "PRODUCTION_APP_ID",
       "ClientSecret": "PRODUCTION_CERT_ID"
     }
   }
   ```

3. **Update Redirect URI:**
   - Use your production domain instead of localhost
   - Example: `https://yourapp.com/ebay/callback`
   - Update the URI in eBay Developer Portal

4. **Secure Credentials:**
   - Use Azure Key Vault, AWS Secrets Manager, or similar
   - Never hardcode credentials in production
   - Use environment variables or secure configuration providers

5. **Test Thoroughly:**
   - Test the OAuth flow in production environment
   - Use real eBay accounts (not sandbox users)
   - Monitor for any authentication issues

## API Scopes Explained

The application requests these OAuth scopes:

| Scope | Purpose |
|-------|---------|
| `api_scope` | General API access |
| `sell.inventory` | Create and manage inventory items |
| `sell.inventory.readonly` | View inventory items |
| `sell.fulfillment` | Create and manage orders |
| `sell.fulfillment.readonly` | View orders |
| `sell.marketing` | Create and manage promotions |
| `sell.marketing.readonly` | View marketing campaigns |
| `sell.account` | Manage account settings |
| `sell.account.readonly` | View account information |
| `commerce.catalog.readonly` | Read product catalog data |

## Support and Resources

- **eBay Developer Documentation:** https://developer.ebay.com/docs
- **OAuth 2.0 Guide:** https://developer.ebay.com/api-docs/static/oauth-tokens.html
- **API Test Tool:** https://developer.ebay.com/my/api_test_tool
- **Developer Forums:** https://community.ebay.com/t5/Developer-Program/ct-p/developer-program

## Security Best Practices

1. ✅ Never commit credentials to version control
2. ✅ Use environment-specific configuration
3. ✅ Rotate credentials regularly
4. ✅ Monitor API usage and errors
5. ✅ Implement rate limiting
6. ✅ Log authentication events
7. ✅ Use HTTPS for all OAuth redirects
8. ✅ Validate state parameter to prevent CSRF
9. ✅ Store refresh tokens encrypted in database
10. ✅ Implement proper error handling

---

**Need Help?** Check the main README.md for more detailed information and troubleshooting tips.
