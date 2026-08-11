using System.Text.Json;
using TransportERP.Contracts.Core;

namespace TransportERP.Tests;

public sealed class W1CoreContractTests
{
    [Fact]
    public void OperationContext_RequiresIdentityScopeAndCorrelation()
    {
        var valid = NewContext();
        valid.EnsureComplete();

        var missingScope = valid with { BranchId = Guid.Empty };
        Assert.Throws<ArgumentException>(missingScope.EnsureComplete);
    }

    [Fact]
    public void CapabilityState_RepresentsPresentationOnlyStates()
    {
        Assert.False(CapabilityState.Hidden.IsVisible);
        Assert.True(CapabilityState.Enabled.IsVisible);
        Assert.True(CapabilityState.Enabled.IsEnabled);

        var disabled = CapabilityState.Disabled("PERIOD_CLOSED");
        Assert.True(disabled.IsVisible);
        Assert.False(disabled.IsEnabled);
        Assert.Equal("PERIOD_CLOSED", disabled.ReasonCode);
        Assert.Throws<ArgumentException>(() => CapabilityState.Disabled(" "));
    }

    [Fact]
    public void TransportError_UsesOnlyTheApprovedStandardCodesAndCorrelation()
    {
        var error = new TransportError(
            TransportErrorCode.ConcurrencyConflict,
            Guid.CreateVersion7(),
            "errors.concurrencyConflict");

        error.EnsureComplete();
        var approvedCodes = new[]
        {
            TransportErrorCode.ValidationFailed,
            TransportErrorCode.PermissionDenied,
            TransportErrorCode.ScopeDenied,
            TransportErrorCode.ConcurrencyConflict,
            TransportErrorCode.NotFound,
            TransportErrorCode.Conflict,
            TransportErrorCode.StateTransitionInvalid,
            TransportErrorCode.ApprovalStateInvalid,
            TransportErrorCode.PeriodClosed,
            TransportErrorCode.SelfApprovalDenied,
            TransportErrorCode.DuplicateNumber,
            TransportErrorCode.NumberSequenceInactive,
            TransportErrorCode.NumberingStateInvalid,
            TransportErrorCode.IdempotencyConflict
        };

        Assert.Equal(approvedCodes, Enum.GetValues<TransportErrorCode>());
        foreach (var code in approvedCodes)
        {
            new TransportError(code, Guid.CreateVersion7(), "error.contract").EnsureComplete();
        }

        // MessageKey is presentation-safe metadata rather than raw exception detail.
        Assert.Equal("error.contract", new TransportError(
            TransportErrorCode.NotFound,
            Guid.CreateVersion7(),
            "error.contract").MessageKey);
        Assert.Throws<ArgumentException>(() => new TransportError(
            TransportErrorCode.ScopeDenied,
            Guid.Empty,
            "errors.scopeDenied").EnsureComplete());
        Assert.Throws<ArgumentException>(() => new TransportError(
            TransportErrorCode.ScopeDenied,
            Guid.CreateVersion7(),
            " ").EnsureComplete());
        Assert.Throws<ArgumentException>(() => new TransportError(
            (TransportErrorCode)999,
            Guid.CreateVersion7(),
            "errors.unknown").EnsureComplete());
    }

    [Fact]
    public void BusinessAuditEvent_CarriesRequiredAppendOnlyAuditMetadata()
    {
        using var before = JsonDocument.Parse("{\"active\":true}");
        using var after = JsonDocument.Parse("{\"active\":false}");
        var auditEvent = new BusinessAuditEvent(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            DateTimeOffset.UtcNow,
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            "FiscalPeriod",
            Guid.CreateVersion7(),
            "Close",
            Guid.CreateVersion7(),
            "Year-end close",
            before.RootElement.Clone(),
            after.RootElement.Clone());

        auditEvent.EnsureComplete();
        Assert.Equal("FiscalPeriod", auditEvent.EntityType);
        Assert.Equal("Close", auditEvent.Action);
        Assert.True(auditEvent.BeforeState.HasValue);
        Assert.True(auditEvent.AfterState.HasValue);
        Assert.Throws<ArgumentException>(() => (auditEvent with
        {
            OccurredAt = default
        }).EnsureComplete());
    }

    private static OperationContext NewContext() => new(
        Guid.CreateVersion7(),
        Guid.CreateVersion7(),
        Guid.CreateVersion7(),
        Guid.CreateVersion7());
}
