using TransportERP.Contracts.Accounting;

namespace TransportERP.Infrastructure.Persistence;

/// <summary>Builds receipt-only posting amounts from an explicit configuration. Does not mutate balances.</summary>
public static class ReceiptPostingPlan
{
    public sealed record PostingLine(Guid AccountId, decimal Debit, decimal Credit, Guid CurrencyId, decimal ForeignAmount, Guid? FinancialDimensionId = null);
    public sealed record Plan(IReadOnlyList<PostingLine> Lines, string PolicySnapshot);

    public static Plan Build(ReceiptDraft draft, ReceiptConfiguration settings, Guid baseCurrencyId, int baseDigits)
    {
        if (baseDigits is < 0 or > 4) throw new InvalidOperationException("دقة العملة الأساسية غير متوافقة مع تخزين القيود (حتى أربع خانات).");
        if (settings.PostingDimension is not (null or "costCenter" or "project" or "activity"))
            throw new InvalidOperationException("القيد يدعم بُعدًا واحدًا فقط؛ راجع إعداد بُعد الترحيل.");
        Guid? Dimension(Dictionary<string, string?>? values)
        {
            if (settings.PostingDimension == null) return null;
            string? value = values?.GetValueOrDefault(settings.PostingDimension);
            if (string.IsNullOrWhiteSpace(value)) value = draft.Additional.GetValueOrDefault(settings.PostingDimension);
            return Guid.TryParse(value, out var id) ? id : null;
        }
        var rounding = settings.Rounding switch
        {
            "TO_EVEN" => MidpointRounding.ToEven,
            "AWAY_FROM_ZERO" => MidpointRounding.AwayFromZero,
            _ => throw new InvalidOperationException("حدد قاعدة التقريب في إعدادات سند القبض قبل الترحيل.")
        };
        if (draft.Method is not ("CASH" or "CHEQUE")) throw new InvalidOperationException("طريقة قبض غير صالحة.");
        var destination = settings.Destinations.SingleOrDefault(d => d.Id == draft.DestinationId)
            ?? throw new InvalidOperationException("وجهة القبض غير مهيأة.");
        if (destination.AccountId == Guid.Empty || (draft.Method == "CHEQUE" && destination.Kind != "BANK") ||
            (draft.Method == "CASH" && destination.Kind != "CASH")) throw new InvalidOperationException("وجهة القبض لا تطابق الطريقة.");
        var debitAccount = draft.Method == "CASH" ? destination.AccountId : settings.ChequeTreatment switch
        {
            "DIRECT_BANK" => destination.AccountId,
            "CHEQUES_RECEIVABLE" when settings.ChequesReceivableAccountId is { } id && id != Guid.Empty => id,
            _ => throw new InvalidOperationException("حدد سياسة الشيك وحساب أوراق القبض عند الحاجة قبل الترحيل.")
        };
        decimal baseRate = draft.CurrencyId == baseCurrencyId ? 1m : draft.ExchangeRate;
        if (baseRate <= 0 || draft.Amount <= 0 || draft.Lines.Count == 0)
            throw new InvalidOperationException("المبلغ وسعر التحويل والتفاصيل مطلوبة.");
        var result = new List<PostingLine>();
        decimal total = 0;
        foreach (var line in draft.Lines)
        {
            if (line.Amount <= 0 || line.AccountId == Guid.Empty || line.CurrencyId == Guid.Empty)
                throw new InvalidOperationException("راجع حساب وعملة ومبلغ كل سطر.");
            if (draft.Method == "CHEQUE" && (string.IsNullOrWhiteSpace(line.ChequeNumber) || line.ChequeDueDate == null))
                throw new InvalidOperationException("رقم الشيك وتاريخ استحقاقه مطلوبان لكل سطر.");
            decimal rate = line.CurrencyId == draft.CurrencyId ? 1m : line.Rate ?? 0m;
            if (rate <= 0) throw new InvalidOperationException("سعر التحويل إلى عملة السند مطلوب لكل عملة مختلفة.");
            decimal amount = checked(line.Amount * rate);
            total = checked(total + amount);
            decimal credit = decimal.Round(checked(amount * baseRate), baseDigits, rounding);
            if (credit <= 0 || credit > 999999999999999.9999m) throw new InvalidOperationException("مبلغ السطر بعد التحويل خارج دقة التخزين؛ راجع المبلغ.");
            result.Add(new(line.AccountId, 0, credit, line.CurrencyId, line.Amount, Dimension(line.Additional)));
        }
        if (total != draft.Amount) throw new InvalidOperationException("يجب أن يكون فرق السند صفرًا قبل الترحيل.");
        decimal debit = decimal.Round(checked(draft.Amount * baseRate), baseDigits, rounding);
        if (debit > 999999999999999.9999m) throw new InvalidOperationException("المجموع بعد التحويل يتجاوز دقة التخزين.");
        if (result.Sum(l => l.Credit) != debit)
            throw new InvalidOperationException("فارق تقريب بين الرأس والسطور؛ لم يُرحّل السند. يلزم ضبط المبالغ أو اعتماد سياسة فرق تقريب.");
        result.Insert(0, new(debitAccount, debit, 0, draft.CurrencyId, draft.Amount, Dimension(draft.Additional)));
        return new(result, System.Text.Json.JsonSerializer.Serialize(new
        {
            settings.ChequeTreatment, settings.ChequesReceivableAccountId, settings.Rounding,
            settings.PostingDimension, settings.CostCenterDimensionCode, settings.ProjectDimensionCode, settings.ActivityDimensionCode,
            Destination = destination, BaseCurrencyId = baseCurrencyId, BaseDigits = baseDigits, BaseRate = baseRate
        }));
    }
}
