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
            options.AddInterceptors(
                new P2FinanceAppendOnlyInterceptor(),
                new P2ShippingAppendOnlyInterceptor());
        });

        services.AddDbContext<Wave1GeoDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsAssembly(typeof(Wave1GeoDbContext).Assembly.FullName);
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory_Wave1Geo", "transport_erp");
            }));
        services.AddScoped<Wave1GeoService>();

        return services;
    }
}
