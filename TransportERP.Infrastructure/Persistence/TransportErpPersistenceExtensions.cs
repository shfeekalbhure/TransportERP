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

        services.AddDbContext<Wave1ReferenceDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsAssembly(typeof(Wave1ReferenceDbContext).Assembly.FullName);
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory_Wave1Reference", "transport_erp");
            }));

        services.AddScoped<Wave1GeoService>();
        services.AddScoped<Wave1NumberingService>();
        services.AddScoped<Wave1ReferenceService>();
        services.AddScoped<Wave1FinancialReportService>();

        return services;
    }
}
