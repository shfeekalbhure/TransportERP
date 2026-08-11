using Microsoft.EntityFrameworkCore;
using TransportERP.Application.Org;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Org;
using TransportERP.Domain.Org;
using TransportERP.Infrastructure.Geo;

namespace TransportERP.Infrastructure.Org;

/// <summary>EF/MySQL repository for the additive GEN-008..015 model. All writes share the DbContext unit of work with audit events.</summary>
public sealed class EfOrgRepository(TransportErpDbContext db) : IOrgRepository
{
    public Task<OrgEntity?> FindAsync(OrgResource r, Guid id, CancellationToken ct) => Query(r).FirstOrDefaultAsync(x => x.Id == id, ct)!;
    public Task AddAsync(OrgEntity entity, CancellationToken ct) { db.Add(entity); return Task.CompletedTask; }
    public Task SaveAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
    public Task AddReservationAsync(NumberReservation reservation, CancellationToken ct) { db.NumberReservations.Add(reservation); return Task.CompletedTask; }
    public Task<NumberReservation?> FindReservationAsync(Guid sequenceId, string key, CancellationToken ct) => db.NumberReservations.FirstOrDefaultAsync(x => x.SequenceId == sequenceId && x.IdempotencyKey == key, ct);
    public async Task<PagedResponse<OrgDto>> ListAsync(OrgResource r, PagedQueryRequest q, OperationContext context, CancellationToken ct)
    {
        var rows = await Query(r).Where(x => !q.IsActive.HasValue || x.IsActive == q.IsActive).ToListAsync(ct);
        if (!string.IsNullOrWhiteSpace(q.SearchText)) rows = rows.Where(x => Search(x).Contains(q.SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
        // Company-scoped resources never leak across the operation company. Global resources remain global by contract.
        rows = rows.Where(x => x switch { Branch b => b.CompanyId == context.CompanyId, ExchangeRate e => e.CompanyId == context.CompanyId, FiscalYear y => y.CompanyId == context.CompanyId, _ => true }).OrderBy(Code).ToList();
        return new PagedResponse<OrgDto>(rows.Skip((q.Page - 1) * q.PageSize).Take(q.PageSize).Select(ToDto).ToArray(), q.Page, q.PageSize, rows.Count);
    }
    public Task<bool> CodeExistsAsync(OrgResource r, string code, Guid? exceptId, Guid? companyId, CancellationToken ct) => r switch
    {
        OrgResource.Currencies => db.Currencies.AnyAsync(x => x.Code == code && x.Id != exceptId, ct),
        OrgResource.Companies => db.Companies.AnyAsync(x => x.Code == code && x.Id != exceptId, ct),
        OrgResource.Branches => db.Branches.AnyAsync(x => x.CompanyId == companyId && x.Code == code && x.Id != exceptId, ct),
        OrgResource.FiscalYears => db.FiscalYears.AnyAsync(x => x.CompanyId == companyId && x.Code == code && x.Id != exceptId, ct),
        OrgResource.NumberSequences => db.NumberSequences.AnyAsync(x => x.Code == code && x.Id != exceptId, ct),
        OrgResource.Languages => db.Languages.AnyAsync(x => x.LanguageCode == code && x.Id != exceptId, ct),
        OrgResource.ExchangeRates => Task.FromResult(false), // DB index protects the effective-date scope.
        OrgResource.SettingOverrides => Task.FromResult(false), // DB index protects definition/scope.
        _ => Task.FromResult(false)
    };
    private IQueryable<OrgEntity> Query(OrgResource r) => r switch { OrgResource.Currencies => db.Currencies, OrgResource.ExchangeRates => db.ExchangeRates, OrgResource.Companies => db.Companies, OrgResource.Branches => db.Branches, OrgResource.FiscalYears => db.FiscalYears, OrgResource.NumberSequences => db.NumberSequences, OrgResource.Languages => db.Languages, OrgResource.SettingOverrides => db.SettingOverrides, _ => throw new ArgumentOutOfRangeException(nameof(r)) };
    private static string Code(OrgEntity x) => x switch { Currency a => a.Code, Company a => a.Code, Branch a => a.Code, FiscalYear a => a.Code, NumberSequence a => a.Code, Language a => a.LanguageCode, ExchangeRate a => $"{a.BaseCurrencyId:N}:{a.QuoteCurrencyId:N}:{a.EffectiveFrom:yyyyMMdd}", SettingOverride a => $"{a.DefinitionId:N}:{a.ScopeType}:{a.ScopeId:N}", _ => string.Empty };
    private static string Search(OrgEntity x) => x switch { Currency a => $"{a.Code} {a.ArabicName} {a.EnglishName}", Company a => $"{a.Code} {a.ArabicName} {a.EnglishName}", Branch a => $"{a.Code} {a.ArabicName} {a.EnglishName}", FiscalYear a => $"{a.Code} {a.ArabicName} {a.EnglishName}", NumberSequence a => $"{a.Code} {a.ArabicName} {a.EnglishName}", Language a => $"{a.LanguageCode} {a.ArabicName} {a.EnglishName}", _ => Code(x) };
    private static OrgDto ToDto(OrgEntity x) { var values=x.GetType().GetProperties().Where(p=>p.Name is not ("Id" or "IsActive" or "Version" or "CreatedAtUtc" or "UpdatedAtUtc" or "Code" or "ArabicName" or "EnglishName" or "LanguageCode")).ToDictionary(p=>p.Name,p=>p.GetValue(x)?.ToString()); return x switch { Currency a => new(a.Id,a.Code,a.ArabicName,a.EnglishName,a.IsActive,a.Version,values), Company a => new(a.Id,a.Code,a.ArabicName,a.EnglishName,a.IsActive,a.Version,values), Branch a => new(a.Id,a.Code,a.ArabicName,a.EnglishName,a.IsActive,a.Version,values), ExchangeRate a => new(a.Id,Code(a),"سعر صرف",a.Source,a.IsActive,a.Version,values), FiscalYear a => new(a.Id,a.Code,a.ArabicName,a.EnglishName,a.IsActive,a.Version,values), NumberSequence a => new(a.Id,a.Code,a.ArabicName,a.EnglishName,a.IsActive,a.Version,values), Language a => new(a.Id,a.LanguageCode,a.ArabicName,a.EnglishName,a.IsActive,a.Version,values), SettingOverride a => new(a.Id,Code(a),"","",a.IsActive,a.Version,values), _ => throw new ArgumentOutOfRangeException() }; }
}
