using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace TransportERP.Infrastructure.Persistence;

public static class TransportErpPersistenceExtensions
{
    public static IServiceCollection AddTransportErpPostgreSql(this IServiceCollection services, string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        services.AddDbContext<TransportErpDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "transport_erp"));
            options.ReplaceService<IModelCustomizer, TransportErpP2CombinedModelCustomizer>();
            options.AddInterceptors(new P2FinanceAppendOnlyInterceptor(), new P2ShippingAppendOnlyInterceptor());
        });

        services.AddDbContext<Wave1GeoDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsAssembly(typeof(Wave1GeoDbContext).Assembly.FullName);
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory_Wave1Geo", "transport_erp");
            }));

        services.AddDbContext<Wave1ReferenceDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsAssembly(typeof(Wave1ReferenceDbContext).Assembly.FullName);
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory_Wave1Reference", "transport_erp");
            });
            options.ReplaceService<IModelCustomizer, Wave1ReferenceRuntimeModelCustomizer>();
        });

        services.AddDbContext<Wave1CountryAuthorityDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsAssembly(typeof(Wave1CountryAuthorityDbContext).Assembly.FullName);
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory_Wave1CountryAuthority", "transport_erp");
            }));

        services.AddDbContext<Wave1NumberingAuthorityDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsAssembly(typeof(Wave1NumberingAuthorityDbContext).Assembly.FullName);
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory_Wave1NumberingAuthority", "transport_erp");
            }));

        services.AddDbContext<Wave1AccountingAuthorityDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsAssembly(typeof(Wave1AccountingAuthorityDbContext).Assembly.FullName);
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory_Wave1AccountingAuthority", "transport_erp");
            });
            options.ReplaceService<IModelCustomizer, Wave1AccountingAuthorityModelCustomizer>();
        });

        services.AddScoped<Wave1GeoService>();
        services.AddScoped<Wave1LanguageService>();
        services.AddScoped<Wave1CountryAuthorityService>();
        services.AddScoped<Wave1NumberingAuthorityService>();
        services.AddScoped<Wave1AccountClassificationAuthorityService>();
        services.AddScoped<Wave1AgingAuthorityService>();
        services.AddScoped<Wave1CashFlowAuthorityService>();
        services.AddScoped<Wave1BalanceSheetService>();
        services.AddScoped<Wave1DetailedTrialBalanceService>();
        services.AddScoped<Wave1DeliveryAuditWriter>();

        // Historical mixed services remain deliberately unregistered. Their code is retained for lineage only.
        return services;
    }
}
