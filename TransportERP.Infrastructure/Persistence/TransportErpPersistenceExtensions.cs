using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace TransportERP.Infrastructure.Persistence;

public static class TransportErpPersistenceExtensions
{
    public static IServiceCollection AddTransportErpPostgreSql(
        this IServiceCollection services,
        string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        services.AddDbContext<TransportErpDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "transport_erp");
            });
            options.ReplaceService<IModelCustomizer, TransportErpP2CombinedModelCustomizer>();
        });
        return services;
    }
}
