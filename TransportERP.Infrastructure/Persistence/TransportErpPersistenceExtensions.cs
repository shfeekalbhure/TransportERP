using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace TransportERP.Infrastructure.Persistence;

public static class TransportErpPersistenceExtensions
{
    public static DbContextOptionsBuilder ConfigureTransportErpPostgreSql(
        this DbContextOptionsBuilder options,
        string connectionString)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        options.UseNpgsql(connectionString, npgsql =>
            npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "transport_erp"));
        options.ReplaceService<IModelCustomizer, TransportErpP2CombinedModelCustomizer>();
        options.AddInterceptors(
            new P2FinanceAppendOnlyInterceptor(),
            new P2ShippingAppendOnlyInterceptor());
        return options;
    }

    public static IServiceCollection AddTransportErpPostgreSql(
        this IServiceCollection services,
        string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        services.AddDbContext<TransportErpDbContext>(options =>
            options.ConfigureTransportErpPostgreSql(connectionString));
        return services;
    }
}
