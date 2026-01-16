using AspNetCore.GitHub.Issue65070;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;

// Create the web application builder.
var webApplicationBuilder = WebApplication.CreateBuilder(args);
webApplicationBuilder.Configuration.AddEnvironmentVariables();
webApplicationBuilder.Configuration.AddJsonFile("appsettings.user.json", optional: true);

// Configure Services
webApplicationBuilder.Services.AddHttpContextAccessor();
webApplicationBuilder.Services.AddLogging(options => options.AddConsole());
webApplicationBuilder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always;
    options.MinimumSameSitePolicy = SameSiteMode.Lax; // Required for correlation in OAuth.
    options.ConsentCookie.IsEssential = false; // By default, cookies will require consent from the user.
});

var mvcBuilder = webApplicationBuilder.Services.AddRazorPages();
var blazorBuilder = webApplicationBuilder.Services.AddServerSideBlazor(options =>
{
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(5);
    options.DisconnectedCircuitMaxRetained = 100;
});

var authenticationBuilder = webApplicationBuilder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = EndpointPaths.SignIn;
        options.AccessDeniedPath = EndpointPaths.SignIn;
        options.LogoutPath = EndpointPaths.SignOut;

        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.IsEssential = true;
        webApplicationBuilder.Configuration.GetSection("Authentication:Scheme").Bind(options);
    })
    .AddMicrosoftAccount(options =>
    {
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.CallbackPath = EndpointPaths.Authenticated;
        webApplicationBuilder.Configuration.GetSection("Authentication:IdentityProvider").Bind(options);
    });

webApplicationBuilder.Services.AddCascadingAuthenticationState();
webApplicationBuilder.Services.AddAuthorization();
webApplicationBuilder.Services.AddAntiforgery(options =>
{
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.IsEssential = true;
});

// Configure Request Pipeline
var webApplication = webApplicationBuilder.Build();
webApplication.UseHsts();
webApplication.UseHttpsRedirection();
webApplication.UseStaticFiles();
webApplication.UseRouting();
webApplication.UseCookiePolicy();
webApplication.UseAuthentication();
webApplication.UseAuthorization();
webApplication.UseAntiforgery();

// Map Endpoints
webApplication.MapControllers();
webApplication.MapBlazorHub(options => options.CloseOnAuthenticationExpiration = true);
webApplication.MapFallbackToPage("/_Host");

// Run the application
await webApplication.RunAsync();