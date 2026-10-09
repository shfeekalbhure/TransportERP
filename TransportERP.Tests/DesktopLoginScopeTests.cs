using Microsoft.EntityFrameworkCore;
using TransportERP.Api.Authentication;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class DesktopLoginScopeTests
{
    [Fact]
    public async Task Company_role_never_exposes_other_company_or_closed_period()
    {
        await using var db = new TransportErpDbContext(new DbContextOptionsBuilder<TransportErpDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        var a = new Company { Id = Guid.NewGuid(), LegalNameAr = "A" };
        var b = new Company { Id = Guid.NewGuid(), LegalNameAr = "B" };
        var ba = new Branch { Id = Guid.NewGuid(), CompanyId = a.Id, NameAr = "A1" };
        var bb = new Branch { Id = Guid.NewGuid(), CompanyId = b.Id, NameAr = "B1" };
        var user = new User { Id = Guid.NewGuid(), CompanyId = a.Id };
        var role = new Role { Id = Guid.NewGuid(), CompanyId = a.Id };
        db.AddRange(a, b, ba, bb, user, role,
            new UserRole { UserId = user.Id, RoleId = role.Id, CompanyId = a.Id },
            new FiscalPeriod { Id = Guid.NewGuid(), CompanyId = a.Id, Code = "OPEN-A" },
            new FiscalPeriod { Id = Guid.NewGuid(), CompanyId = a.Id, Code = "CLOSED-A", Status = "CLOSED" },
            new FiscalPeriod { Id = Guid.NewGuid(), CompanyId = b.Id, Code = "OPEN-B" });
        await db.SaveChangesAsync();
        var scopes = await DesktopLoginModule.AllowedScopes(db, user, default);
        Assert.Single(scopes);
        Assert.Equal(a.Id, scopes[0].CompanyId);
        Assert.Equal(ba.Id, scopes[0].BranchId);
        role.Status = "INACTIVE";
        await db.SaveChangesAsync();
        Assert.Empty(await DesktopLoginModule.AllowedScopes(db, user, default));
    }

    [Fact]
    public async Task Unscoped_role_does_not_implicitly_grant_all_companies()
    {
        await using var db = new TransportErpDbContext(new DbContextOptionsBuilder<TransportErpDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        var company = new Company { Id = Guid.NewGuid() };
        var user = new User { Id = Guid.NewGuid() };
        var role = new Role { Id = Guid.NewGuid() };
        db.AddRange(company, user, role,
            new Branch { Id = Guid.NewGuid(), CompanyId = company.Id },
            new FiscalPeriod { Id = Guid.NewGuid(), CompanyId = company.Id },
            new UserRole { UserId = user.Id, RoleId = role.Id });
        await db.SaveChangesAsync();
        Assert.Empty(await DesktopLoginModule.AllowedScopes(db, user, default));
    }
}
