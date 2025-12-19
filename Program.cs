using MarketManager.Components;
using MarketManager.Models;
using MarketManager.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure eBay settings
builder.Services.Configure<EbayConfig>(builder.Configuration.GetSection("EbayConfig"));

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
