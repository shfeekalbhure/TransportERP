using System.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TransportERP.Application.Waybills;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;
using TransportERP.Domain.Waybills;

namespace TransportERP.Infrastructure.Persistence;

public sealed class EfWaybillFinanceStore(TransportErpDbContext db, IWaybillAuditSink audit) : IWaybillFinanceStore
{
    private DbSet<WaybillEntity> Waybills => db.Set<WaybillEntity>();
    private DbSet<PaymentPlanLineEntity> Plans => db.Set<PaymentPlanLineEntity>();
    private DbSet<CollectionTransactionEntity> Collections => db.Set<CollectionTransactionEntity>();
    private DbSet<FinancialLinkEntity> FinancialLinks => db.Set<FinancialLinkEntity>();

    public async Task<WaybillFinanceBasis?> GetBasisAsync(
        Guid companyId, Guid branchId, Guid waybillId, CancellationToken cancellationToken)
    {
        var x = await Waybills.AsNoTracking().SingleOrDefaultAsync(
            w => w.Id == waybillId && w.CompanyId == companyId && w.BranchId == branchId,
            cancellationToken);
        return x is null ? null : Basis(x);
    }

    public async Task<PaymentPlanResponse> SetPaymentPlanAsync(
        OperationContext context, Guid waybillId, SetPaymentPlanRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            var waybill = await RequireWaybill(context, waybillId, cancellationToken);
            var operationId = request.ClientOperationId.Trim();

            if (waybill.LastClientOperationId == operationId)
            {
                var replay = await ActivePlan(waybillId, cancellationToken);
                await tx.CommitAsync(cancellationToken);
                return PlanResponse(context, waybill, replay);
            }
            if (waybill.Version != request.ExpectedVersion)
                throw new WaybillPersistenceException("CONCURRENCY_CONFLICT");
            if (waybill.Status is not ("DRAFT" or "APPROVED"))
                throw new WaybillPersistenceException("INVALID_STATE");
            if (waybill.Status == "APPROVED" && await Collections.AnyAsync(x => x.WaybillId == waybillId, cancellationToken))
                throw new WaybillPersistenceException("INVALID_STATE");

            var now = DateTimeOffset.UtcNow;
            var active = await Plans.Where(x => x.WaybillId == waybillId && x.Status == "ACTIVE")
                .OrderBy(x => x.LineNo).ToListAsync(cancellationToken);
            var beforeJson = PaymentPlanAuditJson(active);

            foreach (var line in active)
            {
                line.Status = "CANCELLED";
                line.Version++;
                line.UpdatedAt = now;
            }

            foreach (var input in request.Lines)
            {
                Plans.Add(new PaymentPlanLineEntity
                {
                    Id = Guid.NewGuid(),
                    WaybillId = waybillId,
                    LineNo = input.LineNo,
                    PayerRole = input.PayerRole.Trim().ToUpperInvariant(),
                    PartyId = input.PartyId,
                    PaymentMethodCode = input.PaymentMethodCode.Trim().ToUpperInvariant(),
                    AmountCurrencyId = input.Amount?.CurrencyId,
                    Amount = input.Amount?.Amount,
                    Percent = input.Percent,
                    DueTrigger = input.DueTrigger.Trim().ToUpperInvariant(),
                    DueAt = input.DueAt,
                    Status = "ACTIVE",
                    CreatedAt = now,
                    UpdatedAt = now,
                    Version = 1
                });
            }

            waybill.LastClientOperationId = operationId;
            waybill.Version++;
            waybill.UpdatedAt = now;
            await Save(cancellationToken);
            await audit.WriteAsync(context, "WaybillPaymentPlanSet", "SUCCESS", "Waybill", waybill.Id,
                beforeJson, PaymentPlanInputAuditJson(request.Lines), null, cancellationToken);
            await tx.CommitAsync(cancellationToken);

            var lines = await ActivePlan(waybillId, cancellationToken);
            return PlanResponse(context, waybill, lines);
        }
        catch (Exception ex) when (IsSerializationFailure(ex))
        {
            db.ChangeTracker.Clear();
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    public async Task<CollectionResponse> RecordCollectionAsync(
        OperationContext context, Guid waybillId, RecordCollectionRequest request, CancellationToken cancellationToken)
    {
        var operationId = request.ClientOperationId.Trim();
        var replay = await FindScopedOperation(context, operationId, cancellationToken);
        if (replay is not null)
            return ReplayOrConflict(context, replay, waybillId, request.Amount.CurrencyId, request.Amount.Amount, request.ExchangeRate);

        try
        {
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            var waybill = await RequireWaybill(context, waybillId, cancellationToken);
            if (waybill.Status != "APPROVED")
                throw new WaybillPersistenceException("INVALID_STATE");

            if (request.AccountingReferenceId.HasValue)
                await EnsureAccountingReferenceAsync(
                    context, request.AccountingDocumentType!, request.AccountingReferenceId.Value, cancellationToken);

            var entity = new CollectionTransactionEntity
            {
                Id = Guid.NewGuid(), WaybillId = waybillId, CompanyId = context.CompanyId, BranchId = context.BranchId,
                PayerRole = request.PayerRole.Trim().ToUpperInvariant(), PartyId = request.PartyId,
                PaymentMethodCode = request.PaymentMethodCode.Trim().ToUpperInvariant(),
                CurrencyId = request.Amount.CurrencyId, ExchangeRate = request.ExchangeRate, Amount = request.Amount.Amount,
                CollectedByType = request.CollectedByType.Trim().ToUpperInvariant(), CollectedById = request.CollectedById,
                CollectedAt = request.CollectedAt, ClientOperationId = operationId, Status = "ACCEPTED",
                AccountingReferenceId = request.AccountingReferenceId
            };
            Collections.Add(entity);
            if (request.AccountingReferenceId.HasValue)
                AddFinancialLink(waybill, request.AccountingDocumentType!, request.AccountingReferenceId.Value,
                    request.Amount.Amount, request.Amount.CurrencyId, "COLLECTION");

            await Save(cancellationToken);
            await RefreshFinancialStatus(waybill, cancellationToken);
            waybill.LastClientOperationId = operationId;
            waybill.Version++;
            waybill.UpdatedAt = DateTimeOffset.UtcNow;
            await Save(cancellationToken);
            await audit.WriteAsync(context, "WaybillCollectionRecord", "SUCCESS", "CollectionTransaction", entity.Id,
                null, CollectionAuditJson(entity), null, cancellationToken);
            await tx.CommitAsync(cancellationToken);
            return CollectionResponseOf(context, entity);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            db.ChangeTracker.Clear();
            replay = await FindScopedOperation(context, operationId, cancellationToken);
            if (replay is not null)
                return ReplayOrConflict(context, replay, waybillId, request.Amount.CurrencyId, request.Amount.Amount, request.ExchangeRate);
            throw new WaybillPersistenceException("DUPLICATE_OPERATION", ex);
        }
        catch (Exception ex) when (IsSerializationFailure(ex))
        {
            db.ChangeTracker.Clear();
            replay = await FindScopedOperation(context, operationId, cancellationToken);
            if (replay is not null)
                return ReplayOrConflict(context, replay, waybillId, request.Amount.CurrencyId, request.Amount.Amount, request.ExchangeRate);
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    public async Task<CollectionResponse> ReverseCollectionAsync(
        OperationContext context, Guid collectionId, ReverseCollectionRequest request, CancellationToken cancellationToken)
    {
        var operationId = request.ClientOperationId.Trim();
        var replay = await FindScopedOperation(context, operationId, cancellationToken);
        if (replay is not null)
        {
            if (replay.ReversalOfId != collectionId)
                throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT");
            return CollectionResponseOf(context, replay);
        }

        try
        {
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            var original = await Collections.AsNoTracking().SingleOrDefaultAsync(x =>
                x.Id == collectionId && x.CompanyId == context.CompanyId && x.BranchId == context.BranchId,
                cancellationToken) ?? throw new WaybillPersistenceException("NOT_FOUND");
            if (original.Status != "ACCEPTED" || original.ReversalOfId.HasValue)
                throw new WaybillPersistenceException("ALREADY_REVERSED");
            if (await Collections.AsNoTracking().AnyAsync(x => x.ReversalOfId == original.Id, cancellationToken))
                throw new WaybillPersistenceException("ALREADY_REVERSED");

            var localDate = original.CollectedAt.UtcDateTime.Date;
            var closed = await db.FiscalPeriods.AsNoTracking().AnyAsync(x =>
                x.CompanyId == context.CompanyId && x.Status == "CLOSED" &&
                x.StartDate.Date <= localDate && x.EndDate.Date >= localDate,
                cancellationToken);
            if (closed)
                throw new WaybillPersistenceException("PERIOD_CLOSED");

            var waybill = await RequireWaybill(context, original.WaybillId, cancellationToken);
            if (request.AccountingReferenceId.HasValue)
                await EnsureAccountingReferenceAsync(
                    context, request.AccountingDocumentType!, request.AccountingReferenceId.Value, cancellationToken);

            var reversal = new CollectionTransactionEntity
            {
                Id = Guid.NewGuid(), WaybillId = original.WaybillId, CompanyId = original.CompanyId, BranchId = original.BranchId,
                PayerRole = original.PayerRole, PartyId = original.PartyId, PaymentMethodCode = original.PaymentMethodCode,
                CurrencyId = original.CurrencyId, ExchangeRate = original.ExchangeRate, Amount = original.Amount,
                CollectedByType = "USER", CollectedById = context.UserId, CollectedAt = DateTimeOffset.UtcNow,
                ClientOperationId = operationId, Status = "REVERSED", ReversalOfId = original.Id,
                ReversalReason = request.Reason.Trim(), AccountingReferenceId = request.AccountingReferenceId
            };
            Collections.Add(reversal);
            if (request.AccountingReferenceId.HasValue)
                AddFinancialLink(waybill, request.AccountingDocumentType!, request.AccountingReferenceId.Value,
                    original.Amount, original.CurrencyId, "COLLECTION_REVERSAL");

            await Save(cancellationToken);
            await RefreshFinancialStatus(waybill, cancellationToken);
            waybill.LastClientOperationId = operationId;
            waybill.Version++;
            waybill.UpdatedAt = DateTimeOffset.UtcNow;
            await Save(cancellationToken);
            await audit.WriteAsync(context, "WaybillCollectionReverse", "SUCCESS", "CollectionTransaction", reversal.Id,
                CollectionAuditJson(original), CollectionAuditJson(reversal), request.Reason.Trim(), cancellationToken);
            await tx.CommitAsync(cancellationToken);
            return CollectionResponseOf(context, reversal);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            db.ChangeTracker.Clear();
            replay = await FindScopedOperation(context, operationId, cancellationToken);
            if (replay is not null)
            {
                if (replay.ReversalOfId != collectionId)
                    throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
                return CollectionResponseOf(context, replay);
            }
            if (await Collections.AsNoTracking().AnyAsync(x =>
                    x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.ReversalOfId == collectionId,
                    cancellationToken))
                throw new WaybillPersistenceException("ALREADY_REVERSED", ex);
            throw new WaybillPersistenceException("DUPLICATE_OPERATION", ex);
        }
        catch (Exception ex) when (IsSerializationFailure(ex))
        {
            db.ChangeTracker.Clear();
            replay = await FindScopedOperation(context, operationId, cancellationToken);
            if (replay is not null && replay.ReversalOfId == collectionId)
                return CollectionResponseOf(context, replay);
            if (await Collections.AsNoTracking().AnyAsync(x =>
                    x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.ReversalOfId == collectionId,
                    cancellationToken))
                throw new WaybillPersistenceException("ALREADY_REVERSED", ex);
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    public async Task<WaybillFinancialStatusResponse> GetFinancialStatusAsync(
        OperationContext context, Guid waybillId, CancellationToken cancellationToken)
    {
        var waybill = await Waybills.AsNoTracking().SingleOrDefaultAsync(x =>
            x.Id == waybillId && x.CompanyId == context.CompanyId && x.BranchId == context.BranchId,
            cancellationToken) ?? throw new WaybillPersistenceException("NOT_FOUND");
        var ledger = await Ledger(waybillId, cancellationToken);
        var calc = WaybillFinancialRules.CalculateFinancialStatus(
            waybill.FreightTotal - waybill.DiscountTotal, waybill.ExchangeRate, ledger);
        return StatusResponse(context, waybill, calc);
    }

    private async Task<WaybillEntity> RequireWaybill(OperationContext context, Guid waybillId, CancellationToken ct)
        => await Waybills.SingleOrDefaultAsync(x =>
            x.Id == waybillId && x.CompanyId == context.CompanyId && x.BranchId == context.BranchId, ct)
            ?? throw new WaybillPersistenceException("NOT_FOUND");

    private Task<CollectionTransactionEntity?> FindScopedOperation(
        OperationContext context, string operationId, CancellationToken ct)
        => Collections.AsNoTracking().SingleOrDefaultAsync(x =>
            x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.ClientOperationId == operationId, ct);

    private async Task EnsureAccountingReferenceAsync(
        OperationContext context, string documentType, Guid documentId, CancellationToken ct)
    {
        if (documentId == Guid.Empty || string.IsNullOrWhiteSpace(documentType))
            throw new WaybillPersistenceException("ACCOUNTING_REFERENCE_INVALID");

        var type = documentType.Trim().ToUpperInvariant();
        var exists = type switch
        {
            "RECEIPT_VOUCHER" => await db.ReceiptVouchers.AsNoTracking().AnyAsync(x =>
                x.Id == documentId && x.CompanyId == context.CompanyId && x.BranchId == context.BranchId &&
                (x.Status == "APPROVED" || x.Status == "POSTED"), ct),
            "PAYMENT_VOUCHER" => await db.PaymentVouchers.AsNoTracking().AnyAsync(x =>
                x.Id == documentId && x.CompanyId == context.CompanyId && x.BranchId == context.BranchId &&
                (x.Status == "APPROVED" || x.Status == "POSTED"), ct),
            "JOURNAL_ENTRY" => await db.JournalEntries.AsNoTracking().AnyAsync(x =>
                x.Id == documentId && x.CompanyId == context.CompanyId && x.BranchId == context.BranchId &&
                (x.Status == "APPROVED" || x.Status == "POSTED" || x.Status == "REVERSED"), ct),
            _ => false
        };

        if (!exists)
            throw new WaybillPersistenceException("ACCOUNTING_REFERENCE_INVALID");
    }

    private async Task RefreshFinancialStatus(WaybillEntity waybill, CancellationToken ct)
    {
        var ledger = await Ledger(waybill.Id, ct);
        var calc = WaybillFinancialRules.CalculateFinancialStatus(
            waybill.FreightTotal - waybill.DiscountTotal, waybill.ExchangeRate, ledger);
        waybill.FinancialStatus = calc.Status;
    }

    private Task<List<CollectionLedgerValue>> Ledger(Guid waybillId, CancellationToken ct)
        => Collections.AsNoTracking().Where(x => x.WaybillId == waybillId)
            .Select(x => new CollectionLedgerValue(x.Id, x.CurrencyId, x.ExchangeRate, x.Amount, x.Status, x.ReversalOfId))
            .ToListAsync(ct);

    private Task<List<PaymentPlanLineEntity>> ActivePlan(Guid waybillId, CancellationToken ct)
        => Plans.AsNoTracking().Where(x => x.WaybillId == waybillId && x.Status == "ACTIVE")
            .OrderBy(x => x.LineNo).ToListAsync(ct);

    private static WaybillFinanceBasis Basis(WaybillEntity x)
        => new(x.Id, x.CompanyId, x.BranchId, x.Status, x.FinancialStatus, x.CurrencyId, x.ExchangeRate,
            x.FreightTotal - x.DiscountTotal, x.Version, x.LastClientOperationId);

    private static PaymentPlanResponse PlanResponse(OperationContext context, WaybillEntity waybill, IReadOnlyList<PaymentPlanLineEntity> lines)
        => new(waybill.Id, waybill.CurrencyId, waybill.FreightTotal - waybill.DiscountTotal, waybill.Version,
            lines.Select(x => new PaymentPlanLineResponse(x.Id, x.LineNo, x.PayerRole, x.PartyId, x.PaymentMethodCode,
                x.Amount.HasValue && x.AmountCurrencyId.HasValue ? new MoneyAmount(x.AmountCurrencyId.Value, x.Amount.Value) : null,
                x.Percent, x.DueTrigger, x.DueAt, x.Status)).ToList(), context.CorrelationId);

    private static CollectionResponse CollectionResponseOf(OperationContext context, CollectionTransactionEntity x)
        => new(x.Id, x.WaybillId, x.PayerRole, x.PartyId, x.PaymentMethodCode,
            new MoneyAmount(x.CurrencyId, x.Amount), x.ExchangeRate, x.CollectedByType, x.CollectedById,
            x.BranchId, x.CollectedAt, x.Status, x.ReversalOfId, x.AccountingReferenceId, context.CorrelationId);

    private static WaybillFinancialStatusResponse StatusResponse(
        OperationContext context, WaybillEntity waybill, (string Status, decimal PaidEquivalent, decimal RemainingEquivalent) calc)
        => new(waybill.Id,
            new MoneyAmount(waybill.CurrencyId, waybill.FreightTotal - waybill.DiscountTotal),
            new MoneyAmount(waybill.CurrencyId, calc.PaidEquivalent),
            new MoneyAmount(waybill.CurrencyId, calc.RemainingEquivalent),
            calc.Status, waybill.Version, context.CorrelationId);

    private static CollectionResponse ReplayOrConflict(
        OperationContext context, CollectionTransactionEntity replay, Guid waybillId, Guid currencyId, decimal amount, decimal rate)
    {
        if (replay.CompanyId != context.CompanyId || replay.BranchId != context.BranchId ||
            replay.WaybillId != waybillId || replay.CurrencyId != currencyId ||
            replay.Amount != amount || replay.ExchangeRate != rate)
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT");
        return CollectionResponseOf(context, replay);
    }

    private void AddFinancialLink(WaybillEntity waybill, string documentType, Guid documentId, decimal amount, Guid currencyId, string linkType)
        => FinancialLinks.Add(new FinancialLinkEntity
        {
            Id = Guid.NewGuid(), WaybillId = waybill.Id, DocumentType = documentType.Trim().ToUpperInvariant(),
            DocumentId = documentId, Amount = amount, CurrencyId = currencyId, LinkType = linkType,
            Status = "ACTIVE", CreatedAt = DateTimeOffset.UtcNow
        });

    private static string PaymentPlanAuditJson(IEnumerable<PaymentPlanLineEntity> lines)
        => JsonSerializer.Serialize(lines.Select(x => new
        {
            x.LineNo, x.PayerRole, x.PartyId, x.PaymentMethodCode,
            x.AmountCurrencyId, x.Amount, x.Percent, x.DueTrigger, x.DueAt, x.Status
        }));

    private static string PaymentPlanInputAuditJson(IEnumerable<PaymentPlanLineInput> lines)
        => JsonSerializer.Serialize(lines.Select(x => new
        {
            x.LineNo, PayerRole = x.PayerRole.Trim().ToUpperInvariant(), x.PartyId,
            PaymentMethodCode = x.PaymentMethodCode.Trim().ToUpperInvariant(),
            AmountCurrencyId = x.Amount?.CurrencyId, Amount = x.Amount?.Amount, x.Percent,
            DueTrigger = x.DueTrigger.Trim().ToUpperInvariant(), x.DueAt, Status = "ACTIVE"
        }));

    private static string CollectionAuditJson(CollectionTransactionEntity x)
        => JsonSerializer.Serialize(new
        {
            x.Id, x.WaybillId, x.CompanyId, x.BranchId, x.PayerRole, x.PartyId,
            x.PaymentMethodCode, x.CurrencyId, x.ExchangeRate, x.Amount,
            x.CollectedByType, x.CollectedById, x.CollectedAt, x.ClientOperationId,
            x.Status, x.AccountingReferenceId, x.ReversalOfId, x.ReversalReason
        });

    private async Task Save(CancellationToken ct)
    {
        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    private static bool IsUniqueViolation(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
            if (current is PostgresException { SqlState: "23505" }) return true;
        return false;
    }

    private static bool IsSerializationFailure(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
            if (current is PostgresException { SqlState: "40001" or "40P01" }) return true;
        return false;
    }
}
