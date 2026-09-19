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
        services.AddSingleton<Infrastructure.MediaStorage>();
        services.AddScoped<DigiCard.Contracts.Invitations.IDraftReader, Features.GetDraft.DraftReader>();
        return services;
    }
    public static IEndpointRouteBuilder MapInvitations(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/invitations").RequireAuthorization();
        Features.ListDrafts.Endpoint.Map(group);
        Features.CreateDraft.Endpoint.Map(group);
        Features.GetDraft.Endpoint.Map(group);
        Features.UpdateDraft.Endpoint.Map(group);
        Features.DeleteDraft.Endpoint.Map(group);

        // The form-post twin of the line above, for the JavaScript-free "my cards" page.
        Features.DeleteDraft.FormEndpoint.Map(group);

        // Uploaded images are their own resource, not a sub-resource of one card: the same photo
        // can end up on several cards, and a background is not part of a card at all.
        var media = endpoints.MapGroup("/api/media").RequireAuthorization();
        Features.UploadMedia.Endpoint.Map(media);
        Features.GetMedia.Endpoint.Map(media);

        return endpoints;
    }
}
