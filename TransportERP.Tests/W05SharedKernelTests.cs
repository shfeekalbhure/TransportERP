using TransportERP.Contracts.Attachments;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Numbering;
using TransportERP.Contracts.Party;
using TransportERP.Contracts.Tracking;

namespace TransportERP.Tests;

public sealed class W05SharedKernelTests
{
    [Fact]
    public void MoneyAndFx_PreserveCurrencyAndHistoricalRate()
    {
        var transactionCurrency = Guid.NewGuid();
        var accountingCurrency = Guid.NewGuid();
        var amount = new MoneyAmount(transactionCurrency, 125.50m);
        var fx = new FxSnapshot(transactionCurrency, accountingCurrency, 2m, DateTimeOffset.UtcNow, "TEST");

        amount.EnsureNonNegative();
        fx.EnsureValid();
        var converted = fx.ConvertToAccounting(amount);

        Assert.Equal(accountingCurrency, converted.CurrencyId);
        Assert.Equal(251.00m, converted.Amount);
    }

    [Fact]
    public void Fx_RejectsInvalidOrContradictoryRate()
    {
        var currency = Guid.NewGuid();
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new FxSnapshot(currency, Guid.NewGuid(), 0m, DateTimeOffset.UtcNow, "TEST").EnsureValid());
        Assert.Throws<ArgumentException>(() =>
            new FxSnapshot(currency, currency, 1.1m, DateTimeOffset.UtcNow, "TEST").EnsureValid());
    }

    [Fact]
    public void OperationalPartySnapshot_DoesNotRequireAccountingAccountButRequiresIdentityConsistency()
    {
        var party = new OperationalPartySnapshot(
            null,
            "P-001",
            "عميل تشغيلي",
            "777000000",
            null,
            null,
            new AddressSnapshot(null, null, null, null, null, "تعز"));
        party.EnsureValid();

        var invalid = party with { IdentityNo = "12345", IdentityType = null };
        Assert.Throws<ArgumentException>(() => invalid.EnsureValid());
    }

    [Fact]
    public void AttachmentDescriptor_RequiresHashStorageAndActorMetadata()
    {
        var descriptor = new AttachmentDescriptor(
            Guid.NewGuid(), AttachmentOwnerKind.Waybill, Guid.NewGuid(), "GOODS_PHOTO",
            "blob://ref", "sha256:abc", 100, "image/jpeg", Guid.NewGuid(), DateTimeOffset.UtcNow);
        descriptor.EnsureComplete();

        Assert.Throws<ArgumentException>(() => (descriptor with { ContentHash = " " }).EnsureComplete());
    }

    [Fact]
    public void MovementEnvelope_IsAppendOnlyMetadataAndCannotReverseItself()
    {
        var eventId = Guid.NewGuid();
        var movement = new MovementEnvelope(
            eventId, Guid.NewGuid(), Guid.NewGuid(), "Waybill", Guid.NewGuid(), "LOAD",
            DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, Guid.NewGuid(), Guid.NewGuid(), "op-1", null);
        movement.EnsureComplete();

        Assert.Throws<ArgumentException>(() => (movement with { ReversesEventId = eventId }).EnsureComplete());
    }

    [Fact]
    public void GeoAddressSnapshot_EnforcesDeclaredHierarchy()
    {
        var country = Guid.NewGuid();
        var governorate = Guid.NewGuid();
        var directorate = Guid.NewGuid();
        var city = Guid.NewGuid();
        var area = Guid.NewGuid();
        new GeoAddressSnapshot(country, governorate, directorate, city, area, "شارع 1").EnsureUsable();

        Assert.Throws<ArgumentException>(() =>
            new GeoAddressSnapshot(null, null, null, null, area, null).EnsureUsable());
    }

    [Fact]
    public void NumberReservationContract_UsesKnownStateAndAuthoritativeIdentity()
    {
        var dto = new NumberReservationDto(Guid.NewGuid(), Guid.NewGuid(), 1001, "WB-1001", NumberReservationStates.Committed);
        dto.EnsureValid();
        Assert.True(NumberReservationStates.IsKnown(NumberReservationStates.Void));

        Assert.Throws<ArgumentException>(() => (dto with { State = "REUSED" }).EnsureValid());
    }
}
