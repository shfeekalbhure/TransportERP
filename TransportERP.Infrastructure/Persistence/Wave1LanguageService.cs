using System.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Wave1;

namespace TransportERP.Infrastructure.Persistence;

/// <summary>
/// Runtime service for the current-approved GEN-014 language contract only.
/// Held ACC-036/074/075/050 capabilities are intentionally not part of this service.
/// </summary>
public sealed class Wave1LanguageService(Wave1ReferenceDbContext db)
{
    public async Task<PagedResponse<LanguageListItemDto>> ListLanguagesAsync(LanguageQueryRequest request, CancellationToken ct = default)
    {
        var page = ValidatePage(request.Page, request.PageSize, out var pageSize);
        var q = db.Languages.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            var term = request.SearchText.Trim();
            q = q.Where(x => x.Code.Contains(term) || x.CultureCode.Contains(term));
        }
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            var active = ParseStatus(request.Status);
            q = q.Where(x => x.IsActive == active);
        }
        if (!string.IsNullOrWhiteSpace(request.Direction))
        {
            var direction = NormalizeDirection(request.Direction);
            q = q.Where(x => x.Direction == direction);
        }

        q = ApplyLanguageSort(q, request.SortBy, request.SortDirection);
        var total = await q.CountAsync(ct);
        var rows = await q.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(x => new LanguageListItemDto(
                x.Id, x.Code, x.CultureCode, x.Direction,
                x.IsActive ? "Active" : "Stopped", x.Version))
            .ToListAsync(ct);
        return new(rows, page, pageSize, total);
    }

    public async Task<LanguageDto?> GetLanguageAsync(Guid id, CancellationToken ct = default)
        => await db.Languages.AsNoTracking().Where(x => x.Id == id)
            .Select(x => new LanguageDto(
                x.Id, x.Code, x.CultureCode, x.Direction,
                x.IsActive ? "Active" : "Stopped", x.Version))
            .SingleOrDefaultAsync(ct);

    public Task<LanguageDto> CreateLanguageAsync(
        OperationContext context,
        CreateLanguageRequest request,
        CancellationToken ct = default)
        => ExecuteRequiredAsync(async () =>
        {
            context.EnsureComplete();
            var code = Required(request.Code);
            var cultureCode = Required(request.CultureCode);
            var direction = NormalizeDirection(request.Direction);
            if (await db.Languages.AnyAsync(x => x.Code == code || x.CultureCode == cultureCode, ct))
                throw new Wave1ReferenceRuleException("CONFLICT");

            var now = DateTimeOffset.UtcNow;
            var entity = new Wave1LanguageEntity
            {
                Id = Guid.NewGuid(),
                Code = code,
                CultureCode = cultureCode,
                Direction = direction,
                IsActive = true,
                Version = 1,
                CreatedAt = now,
                UpdatedAt = now
            };
            db.Languages.Add(entity);
            await AppendAuditAsync(context, "Language.Create", "Language", entity.Id,
                null, JsonSerializer.Serialize(ToLanguageDto(entity)), null, ct);
            await db.SaveChangesAsync(ct);
            return ToLanguageDto(entity);
        }, ct);

    public Task<LanguageDto?> UpdateLanguageAsync(
        OperationContext context,
        Guid id,
        UpdateLanguageRequest request,
        CancellationToken ct = default)
        => ExecuteAsync(async () =>
        {
            context.EnsureComplete();
            var entity = await db.Languages.SingleOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return null;
            if (entity.Version != request.ExpectedVersion)
                throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");

            var code = Required(request.Code);
            var cultureCode = Required(request.CultureCode);
            var direction = NormalizeDirection(request.Direction);
            if (await db.Languages.AnyAsync(x => x.Id != id && (x.Code == code || x.CultureCode == cultureCode), ct))
                throw new Wave1ReferenceRuleException("CONFLICT");

            var before = JsonSerializer.Serialize(ToLanguageDto(entity));
            entity.Code = code;
            entity.CultureCode = cultureCode;
            entity.Direction = direction;
            entity.Version++;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            await AppendAuditAsync(context, "Language.Update", "Language", entity.Id,
                before, JsonSerializer.Serialize(ToLanguageDto(entity)), null, ct);
            await db.SaveChangesAsync(ct);
            return ToLanguageDto(entity);
        }, ct);

    public Task<LanguageDto?> DisableLanguageAsync(
        OperationContext context,
        Guid id,
        DisableRequest request,
        CancellationToken ct = default)
        => ExecuteAsync(async () =>
        {
            context.EnsureComplete();
            if (string.IsNullOrWhiteSpace(request.Reason))
                throw new Wave1ReferenceRuleException("VALIDATION_FAILED");

            var entity = await db.Languages.SingleOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return null;
            if (entity.Version != request.ExpectedVersion)
                throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");

            var before = JsonSerializer.Serialize(ToLanguageDto(entity));
            entity.IsActive = false;
            entity.Version++;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            await AppendAuditAsync(context, "Language.Disable", "Language", entity.Id,
                before, JsonSerializer.Serialize(ToLanguageDto(entity)), request.Reason.Trim(), ct);
            await db.SaveChangesAsync(ct);
            return ToLanguageDto(entity);
        }, ct);

    private async Task<T?> ExecuteAsync<T>(Func<Task<T?>> action, CancellationToken ct)
    {
        await using var tx = await BeginTransactionIfSupportedAsync(ct);
        try
        {
            var value = await action();
            if (tx is not null) await tx.CommitAsync(ct);
            return value;
        }
        catch
        {
            if (tx is not null) await tx.RollbackAsync(ct);
            throw;
        }
    }

    private async Task<T> ExecuteRequiredAsync<T>(Func<Task<T>> action, CancellationToken ct)
    {
        await using var tx = await BeginTransactionIfSupportedAsync(ct);
        try
        {
            var value = await action();
            if (tx is not null) await tx.CommitAsync(ct);
            return value;
        }
        catch
        {
            if (tx is not null) await tx.RollbackAsync(ct);
            throw;
        }
    }

    private async Task<IDbContextTransaction?> BeginTransactionIfSupportedAsync(CancellationToken ct)
        => db.Database.IsRelational()
            ? await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct)
            : null;

    private async Task AppendAuditAsync(
        OperationContext context,
        string action,
        string entityType,
        Guid entityId,
        string? before,
        string? after,
        string? reason,
        CancellationToken ct)
    {
        var previousHash = await db.AuditEvents.AsNoTracking()
            .Where(x => x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.DeviceId == null)
            .OrderByDescending(x => x.OccurredAt)
            .ThenByDescending(x => x.Id)
            .Select(x => x.Hash)
            .FirstOrDefaultAsync(ct);

        var evt = new AuditEvent
        {
            Id = Guid.NewGuid(),
            OccurredAt = DateTimeOffset.UtcNow,
            ActorUserId = context.UserId,
            CompanyId = context.CompanyId,
            BranchId = context.BranchId,
            Action = action,
            Outcome = "SUCCESS",
            EntityType = entityType,
            EntityId = entityId,
            CorrelationId = context.CorrelationId,
            BeforeJson = before,
            AfterJson = after,
            Reason = string.IsNullOrWhiteSpace(reason) ? null : reason.Trim(),
            PreviousHash = previousHash
        };
        evt.Hash = AuditEventService.ComputeHash(evt);
        db.AuditEvents.Add(evt);
    }

    private static int ValidatePage(int page, int requestedPageSize, out int effectivePageSize)
    {
        if (page < 1 || requestedPageSize <= 0)
            throw new Wave1ReferenceRuleException("VALIDATION_FAILED");
        effectivePageSize = Math.Min(requestedPageSize, 200);
        return page;
    }

    private static IQueryable<Wave1LanguageEntity> ApplyLanguageSort(
        IQueryable<Wave1LanguageEntity> q,
        string? sortBy,
        string? sortDirection)
    {
        var key = string.IsNullOrWhiteSpace(sortBy) ? "code" : sortBy.Trim().ToLowerInvariant();
        if (key is not ("code" or "culturecode" or "direction" or "status"))
            throw new Wave1ReferenceRuleException("VALIDATION_FAILED");

        var desc = string.Equals(sortDirection?.Trim(), "desc", StringComparison.OrdinalIgnoreCase);
        if (!string.IsNullOrWhiteSpace(sortDirection) && !desc &&
            !string.Equals(sortDirection.Trim(), "asc", StringComparison.OrdinalIgnoreCase))
            throw new Wave1ReferenceRuleException("VALIDATION_FAILED");

        return (key, desc) switch
        {
            ("code", false) => q.OrderBy(x => x.Code),
            ("code", true) => q.OrderByDescending(x => x.Code),
            ("culturecode", false) => q.OrderBy(x => x.CultureCode),
            ("culturecode", true) => q.OrderByDescending(x => x.CultureCode),
            ("direction", false) => q.OrderBy(x => x.Direction).ThenBy(x => x.Code),
            ("direction", true) => q.OrderByDescending(x => x.Direction).ThenBy(x => x.Code),
            ("status", false) => q.OrderByDescending(x => x.IsActive).ThenBy(x => x.Code),
            _ => q.OrderBy(x => x.IsActive).ThenBy(x => x.Code)
        };
    }

    private static bool ParseStatus(string status)
        => status.Trim().ToLowerInvariant() switch
        {
            "active" => true,
            "stopped" => false,
            _ => throw new Wave1ReferenceRuleException("VALIDATION_FAILED")
        };

    private static string NormalizeDirection(string value)
    {
        var x = Required(value).ToUpperInvariant();
        return x is "RTL" or "LTR"
            ? x
            : throw new Wave1ReferenceRuleException("VALIDATION_FAILED");
    }

    private static string Required(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new Wave1ReferenceRuleException("VALIDATION_FAILED");
        return value.Trim();
    }

    private static LanguageDto ToLanguageDto(Wave1LanguageEntity x)
        => new(x.Id, x.Code, x.CultureCode, x.Direction,
            x.IsActive ? "Active" : "Stopped", x.Version);
}
