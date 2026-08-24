using System.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Wave1;

namespace TransportERP.Infrastructure.Persistence;

public sealed class Wave1AccountClassificationAuthorityService(Wave1AccountingAuthorityDbContext db)
{
    private DbSet<AuditEvent> Audit => db.Set<AuditEvent>();

    public async Task<PagedResponse<ACC036Dto>> ListAsync(OperationContext context, PagedQueryRequest request, CancellationToken ct = default)
    {
        context.EnsureComplete();
        if (request.Page < 1 || request.PageSize is < 1 or > 200) throw new ArgumentOutOfRangeException(nameof(request.PageSize));

        var groups = await db.AccountGroups.AsNoTracking().Where(x => x.CompanyId == context.CompanyId).ToListAsync(ct);
        var types = await db.AccountTypes.AsNoTracking().Where(x => x.CompanyId == context.CompanyId).ToListAsync(ct);
        IEnumerable<ACC036Dto> rows = groups.Select(GroupDto).Concat(types.Select(TypeDto));
        if (request.IsActive.HasValue) rows = rows.Where(x => x.IsActive == request.IsActive.Value);
        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            var term = request.SearchText.Trim();
            rows = rows.Where(x => x.Code.Contains(term, StringComparison.OrdinalIgnoreCase)
                || x.ArabicName.Contains(term, StringComparison.OrdinalIgnoreCase)
                || (x.EnglishName?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false));
        }
        var ordered = rows.OrderBy(x => x.Kind, StringComparer.Ordinal).ThenBy(x => x.Code, StringComparer.Ordinal).ToArray();
        return new(ordered.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToArray(), request.Page, request.PageSize, ordered.Length);
    }

    public async Task<ACC036Dto?> GetAsync(OperationContext context, Guid id, CancellationToken ct = default)
    {
        context.EnsureComplete();
        var group = await db.AccountGroups.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && x.CompanyId == context.CompanyId, ct);
        if (group is not null) return GroupDto(group);
        var type = await db.AccountTypes.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && x.CompanyId == context.CompanyId, ct);
        return type is null ? null : TypeDto(type);
    }

    public Task<ACC036Dto> CreateAsync(OperationContext context, CreateACC036Request request, CancellationToken ct = default)
        => ExecuteRequiredAsync(async () =>
        {
            context.EnsureComplete();
            var kind = NormalizeKind(request.Kind); var code = NormalizeCode(request.Code);
            var arabicName = Required(request.ArabicName, 200, "INVALID_ARABIC_NAME");
            var englishName = Optional(request.EnglishName, 200, "INVALID_ENGLISH_NAME");
            var now = DateTimeOffset.UtcNow;

            if (kind == ACC036Kinds.Group)
            {
                if (await db.AccountGroups.AnyAsync(x => x.CompanyId == context.CompanyId && x.Code == code, ct)) throw new ArgumentException("DUPLICATE_CODE");
                var row = new Wave1AccountGroupRecord
                {
                    Id = Guid.NewGuid(), CompanyId = context.CompanyId, Code = code, ArabicName = arabicName, EnglishName = englishName,
                    AllowsPostingAccounts = request.AllowsPostingAccounts ?? false,
                    ShowInFinancialStatements = request.ShowInFinancialStatements ?? true,
                    DisplayOrder = request.DisplayOrder ?? 0,
                    IsActive = true, Version = 1, CreatedAt = now, UpdatedAt = now
                };
                if (row.DisplayOrder < 0) throw new ArgumentException("INVALID_DISPLAY_ORDER");
                db.AccountGroups.Add(row);
                await AppendAuditAsync(context, "AccountGroup.Create", row.Id, null, JsonSerializer.Serialize(GroupDto(row)), null, ct);
                await db.SaveChangesAsync(ct);
                return GroupDto(row);
            }

            if (await db.AccountTypes.AnyAsync(x => x.CompanyId == context.CompanyId && x.Code == code, ct)) throw new ArgumentException("DUPLICATE_CODE");
            var type = new Wave1AccountTypeRecord
            {
                Id = Guid.NewGuid(), CompanyId = context.CompanyId, Code = code, ArabicName = arabicName, EnglishName = englishName,
                FinancialClassification = NormalizeFinancial(request.FinancialClassification), NormalBalance = NormalizeBalance(request.NormalBalance),
                IsActive = true, Version = 1, CreatedAt = now, UpdatedAt = now
            };
            db.AccountTypes.Add(type);
            await AppendAuditAsync(context, "AccountType.Create", type.Id, null, JsonSerializer.Serialize(TypeDto(type)), null, ct);
            await db.SaveChangesAsync(ct);
            return TypeDto(type);
        }, ct);

    public Task<ACC036Dto?> UpdateAsync(OperationContext context, Guid id, UpdateACC036Request request, CancellationToken ct = default)
        => ExecuteAsync(async () =>
        {
            context.EnsureComplete();
            var kind = NormalizeKind(request.Kind); var code = NormalizeCode(request.Code);
            var arabicName = Required(request.ArabicName, 200, "INVALID_ARABIC_NAME");
            var englishName = Optional(request.EnglishName, 200, "INVALID_ENGLISH_NAME");

            if (kind == ACC036Kinds.Group)
            {
                var row = await db.AccountGroups.SingleOrDefaultAsync(x => x.Id == id && x.CompanyId == context.CompanyId, ct);
                if (row is null) return null;
                if (row.Version != request.ExpectedVersion) throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");
                if (await db.AccountGroups.AnyAsync(x => x.Id != id && x.CompanyId == context.CompanyId && x.Code == code, ct)) throw new ArgumentException("DUPLICATE_CODE");
                var before = JsonSerializer.Serialize(GroupDto(row));
                row.Code = code; row.ArabicName = arabicName; row.EnglishName = englishName;
                row.AllowsPostingAccounts = request.AllowsPostingAccounts ?? row.AllowsPostingAccounts;
                row.ShowInFinancialStatements = request.ShowInFinancialStatements ?? row.ShowInFinancialStatements;
                row.DisplayOrder = request.DisplayOrder ?? row.DisplayOrder;
                if (row.DisplayOrder < 0) throw new ArgumentException("INVALID_DISPLAY_ORDER");
                row.Version++; row.UpdatedAt = DateTimeOffset.UtcNow;
                await AppendAuditAsync(context, "AccountGroup.Update", row.Id, before, JsonSerializer.Serialize(GroupDto(row)), null, ct);
                await db.SaveChangesAsync(ct);
                return GroupDto(row);
            }

            var type = await db.AccountTypes.SingleOrDefaultAsync(x => x.Id == id && x.CompanyId == context.CompanyId, ct);
            if (type is null) return null;
            if (type.Version != request.ExpectedVersion) throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");
            if (await db.AccountTypes.AnyAsync(x => x.Id != id && x.CompanyId == context.CompanyId && x.Code == code, ct)) throw new ArgumentException("DUPLICATE_CODE");
            var beforeType = JsonSerializer.Serialize(TypeDto(type));
            type.Code = code; type.ArabicName = arabicName; type.EnglishName = englishName;
            type.FinancialClassification = NormalizeFinancial(request.FinancialClassification);
            type.NormalBalance = NormalizeBalance(request.NormalBalance);
            type.Version++; type.UpdatedAt = DateTimeOffset.UtcNow;
            await AppendAuditAsync(context, "AccountType.Update", type.Id, beforeType, JsonSerializer.Serialize(TypeDto(type)), null, ct);
            await db.SaveChangesAsync(ct);
            return TypeDto(type);
        }, ct);

    public Task<ACC036Dto?> DisableAsync(OperationContext context, Guid id, DisableReferenceRequest request, CancellationToken ct = default)
        => ExecuteAsync(async () =>
        {
            context.EnsureComplete();
            if (string.IsNullOrWhiteSpace(request.Reason)) throw new ArgumentException("REASON_REQUIRED");
            var group = await db.AccountGroups.SingleOrDefaultAsync(x => x.Id == id && x.CompanyId == context.CompanyId, ct);
            if (group is not null)
            {
                if (group.Version != request.ExpectedVersion) throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");
                var before = JsonSerializer.Serialize(GroupDto(group)); group.IsActive = false; group.Version++; group.UpdatedAt = DateTimeOffset.UtcNow;
                await AppendAuditAsync(context, "AccountGroup.Disable", id, before, JsonSerializer.Serialize(GroupDto(group)), request.Reason.Trim(), ct);
                await db.SaveChangesAsync(ct); return GroupDto(group);
            }
            var type = await db.AccountTypes.SingleOrDefaultAsync(x => x.Id == id && x.CompanyId == context.CompanyId, ct);
            if (type is null) return null;
            if (type.Version != request.ExpectedVersion) throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");
            var beforeType = JsonSerializer.Serialize(TypeDto(type)); type.IsActive = false; type.Version++; type.UpdatedAt = DateTimeOffset.UtcNow;
            await AppendAuditAsync(context, "AccountType.Disable", id, beforeType, JsonSerializer.Serialize(TypeDto(type)), request.Reason.Trim(), ct);
            await db.SaveChangesAsync(ct); return TypeDto(type);
        }, ct);

    private async Task AppendAuditAsync(OperationContext context, string action, Guid id, string? before, string? after, string? reason, CancellationToken ct)
    {
        var previous = await Audit.AsNoTracking().Where(x => x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.DeviceId == null)
            .OrderByDescending(x => x.OccurredAt).ThenByDescending(x => x.Id).Select(x => x.Hash).FirstOrDefaultAsync(ct);
        var e = new AuditEvent
        {
            Id = Guid.NewGuid(), OccurredAt = DateTimeOffset.UtcNow, ActorUserId = context.UserId, CompanyId = context.CompanyId,
            BranchId = context.BranchId, Action = action, Outcome = "SUCCESS", EntityType = action.StartsWith("AccountGroup", StringComparison.Ordinal) ? "AccountGroup" : "AccountType",
            EntityId = id, CorrelationId = context.CorrelationId, BeforeJson = before, AfterJson = after, Reason = reason, PreviousHash = previous, Hash = string.Empty
        };
        e.Hash = AuditEventService.ComputeHash(e); Audit.Add(e);
    }

    private async Task<IDbContextTransaction?> BeginAsync(CancellationToken ct)
        => db.Database.IsRelational() ? await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct) : null;
    private async Task<T?> ExecuteAsync<T>(Func<Task<T?>> action, CancellationToken ct)
    { await using var tx = await BeginAsync(ct); try { var v = await action(); if (tx is not null) await tx.CommitAsync(ct); return v; } catch { if (tx is not null) await tx.RollbackAsync(ct); throw; } }
    private async Task<T> ExecuteRequiredAsync<T>(Func<Task<T>> action, CancellationToken ct)
    { await using var tx = await BeginAsync(ct); try { var v = await action(); if (tx is not null) await tx.CommitAsync(ct); return v; } catch { if (tx is not null) await tx.RollbackAsync(ct); throw; } }

    private static string NormalizeKind(string? value) { var x = (value ?? "").Trim().ToUpperInvariant(); return ACC036Kinds.IsKnown(x) ? x : throw new ArgumentException("INVALID_KIND"); }
    private static string NormalizeCode(string value) => Required(value, 60, "INVALID_CODE").ToUpperInvariant();
    private static string NormalizeFinancial(string? value) { var x = (value ?? "").Trim().ToUpperInvariant(); return x is "ASSET" or "LIABILITY" or "EQUITY" or "REVENUE" or "EXPENSE" ? x : throw new ArgumentException("INVALID_FINANCIAL_CLASSIFICATION"); }
    private static string NormalizeBalance(string? value) { var x = (value ?? "").Trim().ToUpperInvariant(); return x is "DEBIT" or "CREDIT" ? x : throw new ArgumentException("INVALID_NORMAL_BALANCE"); }
    private static string Required(string? value, int max, string code) { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException(code); var x = value.Trim(); if (x.Length > max) throw new ArgumentException(code); return x; }
    private static string? Optional(string? value, int max, string code) { if (string.IsNullOrWhiteSpace(value)) return null; var x = value.Trim(); if (x.Length > max) throw new ArgumentException(code); return x; }
    private static ACC036Dto GroupDto(Wave1AccountGroupRecord x) => new(x.Id, x.CompanyId, ACC036Kinds.Group, x.Code, x.ArabicName, x.EnglishName, null, null, x.AllowsPostingAccounts, x.ShowInFinancialStatements, x.DisplayOrder, x.IsActive, x.Version);
    private static ACC036Dto TypeDto(Wave1AccountTypeRecord x) => new(x.Id, x.CompanyId, ACC036Kinds.Type, x.Code, x.ArabicName, x.EnglishName, x.FinancialClassification, x.NormalBalance, null, null, null, x.IsActive, x.Version);
}
