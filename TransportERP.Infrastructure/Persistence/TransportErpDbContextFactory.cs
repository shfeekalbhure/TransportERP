using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace TransportERP.Infrastructure.Persistence;

public sealed class TransportErpDbContextFactory : IDesignTimeDbContextFactory<TransportErpDbContext>
{
    public TransportErpDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("TRANSPORTERP_DESIGN_CONNSTR");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "TRANSPORTERP_DESIGN_CONNSTR must be set for EF Core design-time operations.");
        }
        var options = new DbContextOptionsBuilder<TransportErpDbContext>()
            .UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "transport_erp"))
            .ReplaceService<IModelCustomizer, TransportErpP2ModelCustomizer>()
            .Options;
        return new TransportErpDbContext(options);
    }
}
