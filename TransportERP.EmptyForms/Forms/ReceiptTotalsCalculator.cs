using System.Globalization;

namespace TransportERP.EmptyForms;

// Receipt-only presentation calculation. Exchange-rate convention matches
// UcExchangeRates: one source-currency unit equals Rate target-currency units.
internal static class ReceiptTotalsCalculator
{
    internal sealed record Line(string? CurrencyId, object? Amount, object? Rate);
    internal sealed record Result(decimal? Total, decimal? Difference, string? Error);

    internal static bool TryDecimal(object? value, out decimal number) => decimal.TryParse(
        Convert.ToString(value, CultureInfo.CurrentCulture),
        NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint |
        NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
        CultureInfo.CurrentCulture, out number);

    internal static Result Calculate(string? currencyId, object? headerAmount, IEnumerable<Line> lines)
    {
        if (string.IsNullOrWhiteSpace(currencyId)) return new(null, null, "اختر عملة السند لحساب الإجمالي.");
        decimal total = 0m;
        int index = 0;
        try
        {
            foreach (var line in lines)
            {
                index++;
                if (string.IsNullOrWhiteSpace(line.CurrencyId)) return new(null, null, $"السطر {index}: عملة التفاصيل غير محددة.");
                if (!TryDecimal(line.Amount, out var amount)) return new(null, null, $"السطر {index}: أدخل مبلغًا صالحًا.");
                if (line.CurrencyId == currencyId) total = checked(total + amount);
                else
                {
                    if (!TryDecimal(line.Rate, out var rate) || rate <= 0m)
                        return new(null, null, $"السطر {index}: يلزم سعر صرف موجب من عملة السطر إلى عملة السند.");
                    total = checked(total + checked(amount * rate));
                }
            }
            // No rounding policy is defined for receipts. Preserve decimal precision;
            // never round a nonzero difference to a displayed zero.
            return TryDecimal(headerAmount, out var head)
                ? new(total, checked(head - total), null)
                : new(total, null, "أدخل مبلغ السند لحساب الفارق.");
        }
        catch (OverflowException) { return new(null, null, "القيمة تتجاوز الدقة العددية المتاحة؛ راجع المبالغ وأسعار الصرف."); }
    }
}
