using DigiCard.Modules.Invitations.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace DigiCard.Modules.Invitations;
public static class InvitationModule
{
    public static IServiceCollection AddInvitations(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<InvitationsDbContext>(o => o.UseSqlServer(connectionString,
            sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", "invitations")));
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<DigiCard.Contracts.Invitations.IDraftReader, Features.GetDraft.DraftReader>();
        return services;
    }
    public static IEndpointRouteBuilder MapInvitations(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/invitations").RequireAuthorization();
        Features.CreateDraft.Endpoint.Map(group);
        Features.GetDraft.Endpoint.Map(group);
        return endpoints;
    }
}
