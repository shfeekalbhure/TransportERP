using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TransportERP.Infrastructure.Persistence;

public sealed class TransportErpDbContextFactory : IDesignTimeDbContextFactory<TransportErpDbContext>
{
    public TransportErpDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<TransportErpDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=transport_erp_design;Username=transport_erp;Password=design-time-only")
            .Options;
        return new TransportErpDbContext(options);
    }
}
