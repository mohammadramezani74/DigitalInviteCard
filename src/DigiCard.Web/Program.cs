using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DigiCard.Contracts.Templates;
using DigiCard.Modules.Templates;
using DigiCard.Modules.Invitations;
using Microsoft.AspNetCore.Antiforgery;
using DigiCard.Web.Components;
using DigiCard.Web.Components.Account;
using DigiCard.Modules.Accounts;
using System.Text;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingServerAuthenticationStateProvider>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

// Must come AFTER AddIdentityCookies, and that is not a style preference. AddApplicationCookie
// assigns a whole new CookieAuthenticationEvents object, so anything configured before it is
// thrown away - which is exactly what happened the first time this was written.
//
// An API that answers "not signed in" with a redirect to an HTML login page is a trap: the
// caller asked for JSON, follows the redirect, and gets a page instead - which surfaces as a
// deserialization error somewhere far away from the real cause. Under /api the answer is a
// status code. Everything else keeps the ordinary redirect, because a person following a link
// does want the login page.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context => Answer(context, StatusCodes.Status401Unauthorized);
    options.Events.OnRedirectToAccessDenied = context => Answer(context, StatusCodes.Status403Forbidden);

    static Task Answer(Microsoft.AspNetCore.Authentication.RedirectContext<Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions> context, int status)
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = status;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    }
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found. Set it with user secrets: " +
        "dotnet user-secrets set \"ConnectionStrings:DefaultConnection\" \"...\" --project src/DigiCard.Web");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", "accounts")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Confirmation is required in production and skipped while developing. The scaffolded email
// sender does nothing, so with confirmation on there is no way to reach a usable account without
// hunting for the confirmation link on a page - which makes anything behind a login untestable.
builder.Services.AddIdentityCore<ApplicationUser>(options =>
        options.SignIn.RequireConfirmedAccount = !builder.Environment.IsDevelopment())
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddTemplates(connectionString);
builder.Services.AddInvitations(connectionString);
builder.Services.AddAntiforgery(o => o.HeaderName = "X-CSRF-TOKEN");
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

// The scaffold email sender is development-only. Fail closed until a real sender is installed.
if (!builder.Environment.IsDevelopment())
    throw new InvalidOperationException("Configure a production IEmailSender<ApplicationUser> and remove this guard before deployment.");

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// Serves the static web assets that live in packages rather than in wwwroot - which is where
// _framework/blazor.web.js comes from. Without this the script tag in App.razor 404s, the
// WebAssembly runtime never starts, and every interactive component renders as nothing at all
// while the server-rendered pages carry on looking perfectly healthy.
app.MapStaticAssets();

app.MapHealthChecks("/health/live");

app.MapGet("/api/templates", async (ITemplateCatalog catalog, CancellationToken ct) =>
    Results.Ok(await catalog.ListAsync(ct)));

// The editor runs in the browser, so the catalogs it needs have to be reachable over HTTP.
// All three are public read-only data - the same rows the gallery already renders server-side.
app.MapGet("/api/occasions", async (IOccasionCatalog occasions, CancellationToken ct) =>
    Results.Ok(await occasions.ListAsync(ct)));

app.MapGet("/api/poems", async (IPoemCatalog poems, string? occasion, CancellationToken ct) =>
    Results.Ok(await poems.ListAsync(occasion, ct)));

app.MapGet("/api/antiforgery", (IAntiforgery antiforgery, HttpContext context) =>
{
    context.Response.Headers.CacheControl = "no-store";
    return Results.Ok(new { token = antiforgery.GetAndStoreTokens(context).RequestToken });
}).RequireAuthorization();

// ---------------------------------------------------------------------------- crawler files
// robots.txt is a crawling hint, never a privacy control: /studio and /drafts are protected by
// the authorization checks on those endpoints, and are listed here only to keep them out of
// search results.
app.MapGet("/robots.txt", (HttpContext http, IConfiguration config) =>
{
    var origin = PublicOrigin(http, config);
    var body = new StringBuilder()
        .AppendLine("User-agent: *")
        .AppendLine("Allow: /")
        .AppendLine("Disallow: /studio")
        .AppendLine("Disallow: /drafts/")
        .AppendLine("Disallow: /Account/")
        .AppendLine($"Sitemap: {origin}/sitemap.xml")
        .ToString();

    return Results.Text(body, "text/plain", Encoding.UTF8);
});

app.MapGet("/sitemap.xml", async (HttpContext http, IConfiguration config,
    ITemplateCatalog catalog, IOccasionCatalog occasions, CancellationToken ct) =>
{
    var origin = PublicOrigin(http, config);
    var templates = await catalog.ListAsync(ct);
    var allOccasions = await occasions.ListAsync(ct);

    // Written through a stream rather than a StringWriter: a StringWriter would declare
    // utf-16 in the XML prolog while the response is served as utf-8.
    using var stream = new MemoryStream();
    var settings = new XmlWriterSettings { Indent = true, Encoding = new UTF8Encoding(false) };
    using (var xml = XmlWriter.Create(stream, settings))
    {
        xml.WriteStartDocument();
        xml.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

        WriteUrl(xml, origin, "1.0");
        WriteUrl(xml, $"{origin}/templates", "0.9");

        // Only active occasions: a hidden one has no templates behind it, so listing it in
        // the sitemap would point crawlers at an empty page.
        foreach (var occasion in allOccasions)
            WriteUrl(xml, $"{origin}/templates/category/{occasion.Slug}", "0.8");

        foreach (var template in templates)
            WriteUrl(xml, $"{origin}/templates/{template.Slug}", "0.7");

        xml.WriteEndElement();
        xml.WriteEndDocument();
    }

    return Results.Bytes(stream.ToArray(), "application/xml");

    static void WriteUrl(XmlWriter xml, string location, string priority)
    {
        xml.WriteStartElement("url");
        xml.WriteElementString("loc", location);
        xml.WriteElementString("priority", priority);
        xml.WriteEndElement();
    }
});

app.MapInvitations();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(DigiCard.Web.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();

// Sitemap URLs have to match the host the crawler actually fetched, so falling back to the
// request origin is correct here. A canonical tag is a stronger claim and is only emitted when
// Seo:BaseUrl is configured explicitly - see SeoHead.razor.
static string PublicOrigin(HttpContext http, IConfiguration config)
{
    var configured = config["Seo:BaseUrl"];
    return string.IsNullOrWhiteSpace(configured)
        ? $"{http.Request.Scheme}://{http.Request.Host}"
        : configured.TrimEnd('/');
}

public partial class Program { }
