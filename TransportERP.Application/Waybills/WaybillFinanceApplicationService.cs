using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;
using TransportERP.Domain.Waybills;

namespace TransportERP.Application.Waybills;

public sealed record WaybillFinanceBasis(
    Guid Id,
    Guid CompanyId,
    Guid BranchId,
    string OperationalStatus,
    string FinancialStatus,
    Guid CurrencyId,
    decimal ExchangeRate,
    decimal NetAmount,
    long Version,
    string LastClientOperationId);

public interface IWaybillFinanceStore
{
    Task<WaybillFinanceBasis?> GetBasisAsync(
        Guid companyId, Guid branchId, Guid waybillId, CancellationToken cancellationToken);

    Task<PaymentPlanResponse> SetPaymentPlanAsync(
        OperationContext context, Guid waybillId, SetPaymentPlanRequest request, CancellationToken cancellationToken);

    Task<CollectionResponse> RecordCollectionAsync(
        OperationContext context, Guid waybillId, RecordCollectionRequest request, CancellationToken cancellationToken);

    Task<CollectionResponse> ReverseCollectionAsync(
        OperationContext context, Guid collectionId, ReverseCollectionRequest request, CancellationToken cancellationToken);

    Task<WaybillFinancialStatusResponse> GetFinancialStatusAsync(
        OperationContext context, Guid waybillId, CancellationToken cancellationToken);
}

public sealed class WaybillFinanceApplicationService(
    IWaybillFinanceStore store,
    IOperationalPartyRepository parties)
{
    public async Task<PaymentPlanResponse> SetPaymentPlanAsync(
        OperationContext context,
        Guid waybillId,
        SetPaymentPlanRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        EnsureOperation(request.ClientOperationId);
        var basis = await RequireBasis(context, waybillId, cancellationToken);
        var operationId = request.ClientOperationId.Trim();
        if (!string.Equals(basis.LastClientOperationId, operationId, StringComparison.Ordinal) &&
            basis.Version != request.ExpectedVersion)
            throw new WaybillFinanceApplicationException("CONCURRENCY_CONFLICT");
        if (basis.OperationalStatus is not ("DRAFT" or "APPROVED"))
            throw new WaybillFinanceApplicationException("INVALID_STATE");

        var values = request.Lines.Select(x => new PaymentPlanValue(
            x.LineNo,
            NormalizePayerRole(x.PayerRole),
            x.PartyId,
            Required(x.PaymentMethodCode, "PAYMENT_METHOD_INVALID"),
            x.Amount?.CurrencyId,
            x.Amount?.Amount,
            x.Percent,
            Required(x.DueTrigger, "DUE_TRIGGER_INVALID").ToUpperInvariant(),
            x.DueAt)).ToList();

        await parties.EnsureUsableAsync(context.CompanyId, context.BranchId,
            values.Where(x => x.PartyId.HasValue).Select(x => x.PartyId!.Value).Distinct().ToArray(),
            cancellationToken);
        WaybillFinancialRules.ValidatePaymentPlan(basis.NetAmount, basis.CurrencyId, values);
        return await store.SetPaymentPlanAsync(context, waybillId, request, cancellationToken);
    }

    public async Task<CollectionResponse> RecordCollectionAsync(
        OperationContext context,
        Guid waybillId,
        RecordCollectionRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        EnsureOperation(request.ClientOperationId);
        request.Amount.EnsureValid();
        if (request.Amount.Amount <= 0m)
            throw new WaybillFinanceApplicationException("AMOUNT_INVALID");
        if (request.ExchangeRate <= 0m)
            throw new WaybillFinanceApplicationException("AMOUNT_INVALID");
        _ = NormalizePayerRole(request.PayerRole);
        _ = Required(request.PaymentMethodCode, "PAYMENT_METHOD_INVALID");
        var collectedByType = Required(request.CollectedByType, "COLLECTOR_REQUIRED").ToUpperInvariant();
        if (request.CollectedById == Guid.Empty || request.CollectedAt == default)
            throw new WaybillFinanceApplicationException("COLLECTOR_REQUIRED");
        // No scoped driver/agent resolver exists in the current contract. Fail closed instead of
        // accepting a caller-supplied identity that cannot be proven in the operation scope.
        if (collectedByType != "USER" || request.CollectedById != context.UserId)
            throw new WaybillFinanceApplicationException("SCOPE_DENIED");
        if (request.AccountingReferenceId.HasValue && string.IsNullOrWhiteSpace(request.AccountingDocumentType))
            throw new WaybillFinanceApplicationException("ACCOUNTING_REFERENCE_INVALID");
        if (!request.AccountingReferenceId.HasValue && !string.IsNullOrWhiteSpace(request.AccountingDocumentType))
            throw new WaybillFinanceApplicationException("ACCOUNTING_REFERENCE_INVALID");

        var basis = await RequireBasis(context, waybillId, cancellationToken);
        if (basis.OperationalStatus != "APPROVED")
            throw new WaybillFinanceApplicationException("INVALID_STATE");

        await parties.EnsureUsableAsync(context.CompanyId, context.BranchId,
            request.PartyId.HasValue ? new[] { request.PartyId.Value } : Array.Empty<Guid>(), cancellationToken);

        return await store.RecordCollectionAsync(context, waybillId, request, cancellationToken);
    }

    public async Task<CollectionResponse> ReverseCollectionAsync(
        OperationContext context,
        Guid collectionId,
        ReverseCollectionRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        EnsureOperation(request.ClientOperationId);
        if (string.IsNullOrWhiteSpace(request.Reason))
            throw new WaybillFinanceApplicationException("REASON_REQUIRED");
        if (request.AccountingReferenceId.HasValue && string.IsNullOrWhiteSpace(request.AccountingDocumentType))
            throw new WaybillFinanceApplicationException("ACCOUNTING_REFERENCE_INVALID");
        if (!request.AccountingReferenceId.HasValue && !string.IsNullOrWhiteSpace(request.AccountingDocumentType))
            throw new WaybillFinanceApplicationException("ACCOUNTING_REFERENCE_INVALID");
        return await store.ReverseCollectionAsync(context, collectionId, request, cancellationToken);
    }

    public Task<WaybillFinancialStatusResponse> GetFinancialStatusAsync(
        OperationContext context,
        Guid waybillId,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        return store.GetFinancialStatusAsync(context, waybillId, cancellationToken);
    }

    private async Task<WaybillFinanceBasis> RequireBasis(OperationContext context, Guid id, CancellationToken ct)
        => await store.GetBasisAsync(context.CompanyId, context.BranchId, id, ct)
            ?? throw new WaybillFinanceApplicationException("NOT_FOUND");

    private static string NormalizePayerRole(string value)
    {
        var role = Required(value, "PAYER_ROLE_INVALID").ToUpperInvariant();
        if (role is not ("SENDER" or "RECEIVER" or "PAYER"))
            throw new WaybillFinanceApplicationException("PAYER_ROLE_INVALID");
        return role;
    }

    private static string Required(string value, string code)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new WaybillFinanceApplicationException(code);
        return value.Trim();
    }

    private static void EnsureOperation(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Trim().Length > 160)
            throw new WaybillFinanceApplicationException("CLIENT_OPERATION_REQUIRED");
    }
}

public sealed class WaybillFinanceApplicationException(string code) : InvalidOperationException(code)
{
    public string Code { get; } = code;
}
