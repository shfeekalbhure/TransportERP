using TransportERP.Domain.Waybills;

namespace TransportERP.Tests;

public sealed class P2C01BFinanceTests
{
    [Fact]
    public void Payment_plan_accepts_mixed_amount_and_percent_when_total_matches()
    {
        var currencyId = Guid.NewGuid();
        var lines = new[]
        {
            new PaymentPlanValue(1, "SENDER", null, "CASH", currencyId, 40m, null, "ON_APPROVAL", null),
            new PaymentPlanValue(2, "RECEIVER", null, "BANK", null, null, 60m, "ON_DELIVERY", null)
        };

        WaybillFinancialRules.ValidatePaymentPlan(100m, currencyId, lines);
    }

    [Fact]
    public void Payment_plan_rejects_total_mismatch_and_invalid_line_mode()
    {
        var currencyId = Guid.NewGuid();
        Assert.Equal("PLAN_TOTAL_INVALID", Assert.Throws<WaybillFinancialRuleException>(() =>
            WaybillFinancialRules.ValidatePaymentPlan(100m, currencyId,
            [new PaymentPlanValue(1, "SENDER", null, "CASH", currencyId, 90m, null, "ON_APPROVAL", null)])).Code);

        Assert.Equal("PLAN_LINE_MODE_INVALID", Assert.Throws<WaybillFinancialRuleException>(() =>
            WaybillFinancialRules.ValidatePaymentPlan(100m, currencyId,
            [new PaymentPlanValue(1, "SENDER", null, "CASH", currencyId, 100m, 100m, "ON_APPROVAL", null)])).Code);
    }

    [Fact]
    public void Financial_status_is_derived_from_accepted_net_collections_and_reversals()
    {
        var currencyId = Guid.NewGuid();
        var first = Guid.NewGuid();
        var second = Guid.NewGuid();

        var partial = WaybillFinancialRules.CalculateFinancialStatus(100m, 2m,
        [
            new CollectionLedgerValue(first, currencyId, 2m, 25m, "ACCEPTED", null)
        ]);
        Assert.Equal(WaybillFinancialStatuses.Partial, partial.Status);
        Assert.Equal(25m, partial.PaidEquivalent);
        Assert.Equal(75m, partial.RemainingEquivalent);

        var paid = WaybillFinancialRules.CalculateFinancialStatus(100m, 2m,
        [
            new CollectionLedgerValue(first, currencyId, 2m, 25m, "ACCEPTED", null),
            new CollectionLedgerValue(second, currencyId, 2m, 75m, "ACCEPTED", null)
        ]);
        Assert.Equal(WaybillFinancialStatuses.Paid, paid.Status);
        Assert.Equal(100m, paid.PaidEquivalent);
        Assert.Equal(0m, paid.RemainingEquivalent);

        var reversed = WaybillFinancialRules.CalculateFinancialStatus(100m, 2m,
        [
            new CollectionLedgerValue(first, currencyId, 2m, 25m, "ACCEPTED", null),
            new CollectionLedgerValue(second, currencyId, 2m, 75m, "ACCEPTED", null),
            new CollectionLedgerValue(Guid.NewGuid(), currencyId, 2m, 75m, "REVERSED", second)
        ]);
        Assert.Equal(WaybillFinancialStatuses.Partial, reversed.Status);
        Assert.Equal(25m, reversed.PaidEquivalent);
        Assert.Equal(75m, reversed.RemainingEquivalent);
    }

    [Fact]
    public void Financial_status_detects_overpayment_without_mutating_operational_state()
    {
        var currencyId = Guid.NewGuid();
        var result = WaybillFinancialRules.CalculateFinancialStatus(100m, 1m,
        [new CollectionLedgerValue(Guid.NewGuid(), currencyId, 1m, 110m, "ACCEPTED", null)]);

        Assert.Equal(WaybillFinancialStatuses.Overpaid, result.Status);
        Assert.Equal(110m, result.PaidEquivalent);
        Assert.Equal(-10m, result.RemainingEquivalent);
    }
}
