using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TransportERP.Infrastructure.Persistence;

public sealed class TransportErpDbContextFactory : IDesignTimeDbContextFactory<TransportErpDbContext>
{
    public TransportErpDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("TRANSPORTERP_DESIGN_CONNSTR")
            ?? "Host=127.0.0.1;Port=15432;Database=poc14_pg_test;Username=poc14user;Password=poc14pass";
        var options = new DbContextOptionsBuilder<TransportErpDbContext>()
            .UseNpgsql(connectionString)
            .Options;
        return new TransportErpDbContext(options);
    }
}
