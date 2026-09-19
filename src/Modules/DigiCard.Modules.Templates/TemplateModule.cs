using DigiCard.Contracts.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace DigiCard.Modules.Templates;
public static class TemplateModule
{
    public static IServiceCollection AddTemplates(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TemplatesDbContext>(o => o.UseSqlServer(connectionString,
            sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", "templates")));
        services.AddScoped<ITemplateCatalog, Features.ListTemplates.TemplateCatalog>();
        services.AddScoped<IOccasionCatalog, Features.ListOccasions.OccasionCatalog>();
        services.AddScoped<IPoemCatalog, Features.ListPoems.PoemCatalog>();
        return services;
    }
}
