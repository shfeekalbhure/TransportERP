using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Numbering;

namespace TransportERP.Tests;

public sealed class P2FoundationContractTests
{
    [Fact]
    public void OperationContext_RequiresCompleteScope()
    {
        var context = new OperationContext(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        context.EnsureComplete();

        var invalid = new OperationContext(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        Assert.Throws<ArgumentException>(() => invalid.EnsureComplete());
    }

    [Fact]
    public void NumberReservation_RequiresSequenceAndIdempotencyKey()
    {
        var request = new NumberReservationRequest(Guid.NewGuid(), "op-waybill-001");
        request.EnsureValid();

        Assert.Throws<ArgumentException>(() => new NumberReservationRequest(Guid.Empty, "op").EnsureValid());
        Assert.Throws<ArgumentException>(() => new NumberReservationRequest(Guid.NewGuid(), " ").EnsureValid());
    }

    [Fact]
    public void GeoDtos_PreserveHierarchyReferences()
    {
        var countryId = Guid.NewGuid();
        var governorateId = Guid.NewGuid();
        var directorateId = Guid.NewGuid();
        var cityId = Guid.NewGuid();

        var governorate = new GovernorateDto(governorateId, countryId, "01", "محافظة", null, true, 1);
        var directorate = new DirectorateDto(directorateId, governorate.Id, "0101", "مديرية", null, true, 1);
        var city = new CityDto(cityId, directorate.Id, "010101", "مدينة", null, true, 1);
        var area = new AreaDto(Guid.NewGuid(), city.Id, "01010101", "منطقة", null, true, 1);

        Assert.Equal(countryId, governorate.CountryId);
        Assert.Equal(governorateId, directorate.GovernorateId);
        Assert.Equal(directorateId, city.DirectorateId);
        Assert.Equal(cityId, area.CityId);
    }

    [Fact]
    public void TransportError_RequiresSafeStructuredMetadata()
    {
        var error = new TransportError(TransportErrorCode.ValidationFailed, Guid.NewGuid(), "validation.failed");
        error.EnsureComplete();

        Assert.Throws<ArgumentException>(() =>
            new TransportError(TransportErrorCode.ValidationFailed, Guid.Empty, "validation.failed").EnsureComplete());
    }

    [Fact]
    public void CapabilityState_DisabledRequiresReason()
    {
        var state = CapabilityState.Disabled("MISSING_PERMISSION");
        Assert.True(state.IsVisible);
        Assert.False(state.IsEnabled);
        Assert.Equal("MISSING_PERMISSION", state.ReasonCode);
        Assert.Throws<ArgumentException>(() => CapabilityState.Disabled(" "));
    }
}
