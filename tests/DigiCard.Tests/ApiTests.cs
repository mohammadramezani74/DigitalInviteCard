using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using DigiCard.Contracts.Invitations;
using DigiCard.Modules.Accounts;
using DigiCard.Modules.Templates;
using DigiCard.Modules.Invitations.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
namespace DigiCard.Tests;
public sealed class AppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        // The HTTP-only tests must never reach a real server. The previous placeholder used
        // Server=localhost with the sa account and a wrong password, so every run fired a burst
        // of failed sa logins at the developer's own SQL Server - enough to trip the password
        // policy and lock the account out. Port 1 refuses immediately instead: nothing listens
        // there, no login is ever attempted, and the tests finish in seconds rather than a minute.
        builder.UseSetting("ConnectionStrings:DefaultConnection", Environment.GetEnvironmentVariable("DIGICARD_TEST_SQL")
            ?? "Server=127.0.0.1,1;Database=DigiCardTests;User Id=digicard_tests_placeholder;Password=unused;TrustServerCertificate=True;Connect Timeout=1");
        builder.ConfigureServices(services => services.AddAuthentication(o =>
        { o.DefaultAuthenticateScheme = "Test"; o.DefaultChallengeScheme = "Test"; })
            .AddScheme<AuthenticationSchemeOptions, TestAuthentication>("Test", _ => { }));
    }
    public HttpClient Browser(string? user = null)
    {
        var client = CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = new Uri("https://localhost"), AllowAutoRedirect = false });
        if (user is not null) client.DefaultRequestHeaders.Add("X-Test-User", user);
        return client;
    }
}
public sealed class TestAuthentication(IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger, UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("X-Test-User", out var user)) return Task.FromResult(AuthenticateResult.NoResult());
        var identity = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, user.ToString()), new Claim(ClaimTypes.Name, "Tester")], Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name)));
    }
}
public sealed class SqlFactAttribute : FactAttribute
{
    public SqlFactAttribute()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DIGICARD_TEST_SQL")))
            Skip = "Set DIGICARD_TEST_SQL to a dedicated SQL Server test database.";
    }
}
public sealed class ApiTests : IClassFixture<AppFactory>
{
    private readonly AppFactory factory;
    public ApiTests(AppFactory factory) => this.factory = factory;
    [Theory]
    [InlineData("/")]
    [InlineData("/Account/Login")]
    [InlineData("/Account/Register")]
    public async Task Public_pages_render_without_database_access(string path)
    {
        using var client = factory.Browser();
        var response = await client.GetAsync(path);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("<html lang=\"fa\" dir=\"rtl\">", await response.Content.ReadAsStringAsync());
    }
    [Fact]
    public async Task Anonymous_users_cannot_read_drafts()
    {
        using var client = factory.Browser();
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync($"/api/invitations/{Guid.NewGuid()}")).StatusCode);
    }
    [Fact]
    public async Task Writes_without_antiforgery_are_rejected()
    {
        using var client = factory.Browser("owner");
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsJsonAsync("/api/invitations/", new CreateDraftRequest())).StatusCode);
    }
    [Fact]
    public async Task Invalid_names_are_rejected_before_accessing_database()
    {
        using var client = factory.Browser("owner");
        await Csrf(client);
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsJsonAsync("/api/invitations/", new CreateDraftRequest())).StatusCode);
    }
    [SqlFact]
    public async Task SqlServer_roundtrip_is_owner_scoped()
    {
        await using var scope = factory.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
        await scope.ServiceProvider.GetRequiredService<TemplatesDbContext>().Database.MigrateAsync();
        var db = scope.ServiceProvider.GetRequiredService<InvitationsDbContext>();
        await db.Database.MigrateAsync();
        using var alice = factory.Browser("alice");
        await Csrf(alice);
        var response = await alice.PostAsJsonAsync("/api/invitations/", new CreateDraftRequest
        { TemplateId = Guid.Parse("a41a6319-3438-4ef9-89da-fae6b30b0101"), BrideName = "سارا", GroomName = "علی" });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var draft = (await response.Content.ReadFromJsonAsync<DraftResponse>())!;
        try
        {
            Assert.Equal(HttpStatusCode.OK, (await alice.GetAsync($"/api/invitations/{draft.Id}")).StatusCode);
            using var bob = factory.Browser("bob");
            Assert.Equal(HttpStatusCode.NotFound, (await bob.GetAsync($"/api/invitations/{draft.Id}")).StatusCode);
            Assert.Equal("سارا", (await alice.GetFromJsonAsync<DraftResponse>($"/api/invitations/{draft.Id}"))!.BrideName);
        }
        finally { await db.Drafts.Where(x => x.Id == draft.Id).ExecuteDeleteAsync(); }
    }
    private static async Task Csrf(HttpClient client)
    {
        var token = (await client.GetFromJsonAsync<Token>("/api/antiforgery"))!;
        client.DefaultRequestHeaders.Add("X-CSRF-TOKEN", token.TokenValue);
    }
    private sealed record Token([property: System.Text.Json.Serialization.JsonPropertyName("token")] string TokenValue);
}
