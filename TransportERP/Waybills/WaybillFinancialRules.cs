namespace TransportERP.Domain.Waybills;

public static class WaybillFinancialStatuses
{
    public const string Unpaid = "UNPAID";
    public const string Partial = "PARTIAL";
    public const string Paid = "PAID";
    public const string Overpaid = "OVERPAID";
}

public sealed record PaymentPlanValue(
    int LineNo,
    string PayerRole,
    Guid? PartyId,
    string PaymentMethodCode,
    Guid? AmountCurrencyId,
    decimal? Amount,
    decimal? Percent,
    string DueTrigger,
    DateTimeOffset? DueAt);

public sealed record CollectionLedgerValue(
    Guid Id,
    Guid CurrencyId,
    decimal ExchangeRate,
    decimal Amount,
    string Status,
    Guid? ReversalOfId);

public static class WaybillFinancialRules
{
    private const decimal Tolerance = 0.0001m;

    public static void ValidatePaymentPlan(decimal waybillNetAmount, Guid waybillCurrencyId, IReadOnlyList<PaymentPlanValue> lines)
    {
        if (waybillNetAmount < 0m)
            throw new WaybillFinancialRuleException("PLAN_TOTAL_INVALID");
        if (lines.Count == 0)
            throw new WaybillFinancialRuleException("PLAN_REQUIRED");
        if (lines.Select(x => x.LineNo).Distinct().Count() != lines.Count || lines.Any(x => x.LineNo < 1))
            throw new WaybillFinancialRuleException("PLAN_LINE_INVALID");

        decimal normalized = 0m;
        foreach (var line in lines)
        {
            if (line.PayerRole is not ("SENDER" or "RECEIVER" or "PAYER"))
                throw new WaybillFinancialRuleException("PAYER_ROLE_INVALID");
            if (string.IsNullOrWhiteSpace(line.PaymentMethodCode))
                throw new WaybillFinancialRuleException("PAYMENT_METHOD_INVALID");
            if (string.IsNullOrWhiteSpace(line.DueTrigger))
                throw new WaybillFinancialRuleException("DUE_TRIGGER_INVALID");
            if (string.Equals(line.DueTrigger, "DATE", StringComparison.OrdinalIgnoreCase) && !line.DueAt.HasValue)
                throw new WaybillFinancialRuleException("DUE_DATE_REQUIRED");

            var hasAmount = line.Amount.HasValue;
            var hasPercent = line.Percent.HasValue;
            if (hasAmount == hasPercent)
                throw new WaybillFinancialRuleException("PLAN_LINE_MODE_INVALID");

            if (line.Amount is decimal amount)
            {
                if (line.AmountCurrencyId != waybillCurrencyId || amount <= 0m)
                    throw new WaybillFinancialRuleException("PLAN_AMOUNT_INVALID");
                normalized += amount;
            }
            else if (line.Percent is decimal percent)
            {
                if (percent <= 0m || percent > 100m)
                    throw new WaybillFinancialRuleException("PLAN_PERCENT_INVALID");
                normalized += waybillNetAmount * percent / 100m;
            }
            else
            {
                throw new WaybillFinancialRuleException("PLAN_LINE_MODE_INVALID");
            }
        }

        if (Math.Abs(normalized - waybillNetAmount) > Tolerance)
            throw new WaybillFinancialRuleException("PLAN_TOTAL_INVALID");
    }

    public static (string Status, decimal PaidEquivalent, decimal RemainingEquivalent) CalculateFinancialStatus(
        decimal waybillNetAmount,
        decimal waybillExchangeRate,
        IReadOnlyList<CollectionLedgerValue> ledger)
    {
        if (waybillNetAmount < 0m || waybillExchangeRate <= 0m)
            throw new WaybillFinancialRuleException("FINANCIAL_BASIS_INVALID");

        var dueAccounting = waybillNetAmount * waybillExchangeRate;
        decimal paidAccounting = 0m;
        foreach (var entry in ledger)
        {
            if (entry.Amount <= 0m || entry.ExchangeRate <= 0m)
                throw new WaybillFinancialRuleException("AMOUNT_INVALID");
            if (entry.Status == "ACCEPTED" && entry.ReversalOfId is null)
                paidAccounting += entry.Amount * entry.ExchangeRate;
            else if (entry.Status == "REVERSED" && entry.ReversalOfId.HasValue)
                paidAccounting -= entry.Amount * entry.ExchangeRate;
        }

        if (paidAccounting < 0m && Math.Abs(paidAccounting) <= Tolerance)
            paidAccounting = 0m;

        var paidEquivalent = paidAccounting / waybillExchangeRate;
        var remaining = waybillNetAmount - paidEquivalent;
        string status;
        if (paidAccounting <= Tolerance)
            status = WaybillFinancialStatuses.Unpaid;
        else if (paidAccounting + Tolerance < dueAccounting)
            status = WaybillFinancialStatuses.Partial;
        else if (Math.Abs(paidAccounting - dueAccounting) <= Tolerance)
            status = WaybillFinancialStatuses.Paid;
        else
            status = WaybillFinancialStatuses.Overpaid;

        return (status, paidEquivalent, remaining);
    }
}

public sealed class WaybillFinancialRuleException(string code) : InvalidOperationException(code)
{
    public string Code { get; } = code;
}
