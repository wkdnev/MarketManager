using MarketManager.Components;
using MarketManager.Models;
using MarketManager.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure eBay settings from environment variables with fallback to appsettings.json
builder.Services.Configure<EbayConfig>(options =>
{
    var config = builder.Configuration.GetSection("EbayConfig");
    
    // Read from environment variables first, then fall back to config
    options.ClientId = Environment.GetEnvironmentVariable("EBAY_CLIENT_ID") 
        ?? config["ClientId"] 
        ?? string.Empty;
    
    options.ClientSecret = Environment.GetEnvironmentVariable("EBAY_CLIENT_SECRET") 
        ?? config["ClientSecret"] 
        ?? string.Empty;
    
    options.RedirectUri = Environment.GetEnvironmentVariable("EBAY_REDIRECT_URI") 
        ?? config["RedirectUri"] 
        ?? "https://localhost:5001/ebay/callback";
    
    options.Environment = Environment.GetEnvironmentVariable("EBAY_ENVIRONMENT") 
        ?? config["Environment"] 
        ?? "SANDBOX";
    
    options.RefreshToken = Environment.GetEnvironmentVariable("EBAY_REFRESH_TOKEN") 
        ?? config["RefreshToken"];
    
    // Log configuration source (without exposing sensitive data)
    var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("Startup");
    logger.LogInformation("eBay Configuration:");
    logger.LogInformation("  Environment: {Environment}", options.Environment);
    logger.LogInformation("  ClientId configured: {HasClientId}", !string.IsNullOrEmpty(options.ClientId));
    logger.LogInformation("  ClientSecret configured: {HasClientSecret}", !string.IsNullOrEmpty(options.ClientSecret));
    logger.LogInformation("  RefreshToken configured: {HasRefreshToken}", !string.IsNullOrEmpty(options.RefreshToken));
    logger.LogInformation("  RedirectUri: {RedirectUri}", options.RedirectUri);
});

// Add HttpClient for API calls
builder.Services.AddHttpClient<EbayAuthService>();
builder.Services.AddHttpClient<EbayApiService>();

// Register eBay services
builder.Services.AddScoped<EbayAuthService>();
builder.Services.AddScoped<EbayApiService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
