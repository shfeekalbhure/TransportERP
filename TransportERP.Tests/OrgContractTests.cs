using TransportERP.Contracts.Core;
using TransportERP.Contracts.Org;
using Xunit;

namespace TransportERP.Tests;

public sealed class OrgContractTests
{
    [Fact] public void OrgPermissionIds_are_stable_and_screen_scoped()
    {
        Assert.Equal("GEN008.Manage", OrgPermissions.CurrencyManage); Assert.Equal("GEN013.Reserve", OrgPermissions.NumberingReserve); Assert.Equal("GEN015.Manage", OrgPermissions.SettingsManage);
    }
    [Fact] public void Amendment_error_codes_remain_available_for_org_mapping()
    {
        Assert.True(Enum.IsDefined(TransportErrorCode.DuplicateNumber)); Assert.True(Enum.IsDefined(TransportErrorCode.NumberSequenceInactive)); Assert.True(Enum.IsDefined(TransportErrorCode.StateTransitionInvalid)); Assert.False(Enum.IsDefined((TransportErrorCode)999));
    }
    [Fact] public void Number_reservation_requires_an_idempotency_key()
    {
        var request = new NumberReservationRequest(Guid.CreateVersion7(), "idempotency-key"); Assert.False(string.IsNullOrWhiteSpace(request.IdempotencyKey));
    }
}
