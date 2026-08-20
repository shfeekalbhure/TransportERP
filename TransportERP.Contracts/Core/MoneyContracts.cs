namespace TransportERP.Contracts.Core;

/// <summary>
/// Currency-qualified amount. Sign policy belongs to the calling domain;
/// this value only guarantees that the currency identity is explicit.
/// </summary>
public sealed record MoneyAmount(Guid CurrencyId, decimal Amount)
{
    public void EnsureValid()
    {
        if (CurrencyId == Guid.Empty)
        {
            throw new ArgumentException("A currency identity is required.", nameof(CurrencyId));
        }
    }

    public void EnsureNonNegative()
    {
        EnsureValid();
        if (Amount < 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(Amount), "A non-negative amount is required.");
        }
    }
}

/// <summary>
/// Immutable exchange-rate snapshot used to explain historical accounting conversions.
/// </summary>
public sealed record FxSnapshot(
    Guid TransactionCurrencyId,
    Guid AccountingCurrencyId,
    decimal Rate,
    DateTimeOffset CapturedAt,
    string Source)
{
    public void EnsureValid()
    {
        if (TransactionCurrencyId == Guid.Empty || AccountingCurrencyId == Guid.Empty)
        {
            throw new ArgumentException("Transaction and accounting currencies are required.");
        }

        if (Rate <= 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(Rate), "Exchange rate must be greater than zero.");
        }

        if (CapturedAt == default)
        {
            throw new ArgumentException("A capture timestamp is required.", nameof(CapturedAt));
        }

        if (string.IsNullOrWhiteSpace(Source))
        {
            throw new ArgumentException("An exchange-rate source is required.", nameof(Source));
        }

        if (TransactionCurrencyId == AccountingCurrencyId && Rate != 1m)
        {
            throw new ArgumentException("Same-currency conversion must use a rate of 1.", nameof(Rate));
        }
    }

    public MoneyAmount ConvertToAccounting(MoneyAmount source)
    {
        EnsureValid();
        source.EnsureValid();
        if (source.CurrencyId != TransactionCurrencyId)
        {
            throw new ArgumentException("The source amount currency does not match the FX snapshot.", nameof(source));
        }

        return new MoneyAmount(AccountingCurrencyId, source.Amount * Rate);
    }
}
