using System.Text.Json;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Waybills;

namespace TransportERP.Offline;

public delegate Task<OfflineEnqueueResult> OfflineBusinessQueue(
    OfflineOperationEnqueueTemplate template,
    Func<OfflineGeneratedOperationIdentity, string> payloadFactory,
    CancellationToken cancellationToken);

/// <summary>
/// Shared production application producer used by Desktop and Android. It owns the exact typed
/// CreateOperationalParty payload contract while platform runtimes retain scope/permission/store
/// enforcement in their QueueAsync adapters.
/// </summary>
public sealed class OperationalPartyOfflineProducer(
    OfflineBusinessQueue queue,
    OfflineOperationScope scope)
{
    public Task<OfflineEnqueueResult> QueueAsync(
        string name,
        string mobile,
        string addressText,
        CancellationToken cancellationToken = default)
    {
        Validate(name, mobile, addressText);
        var template = new OfflineOperationEnqueueTemplate(
            Guid.NewGuid(), scope.CompanyId, scope.BranchId, scope.UserId, scope.RegisteredDeviceId,
            "CreateOperationalParty", "CREATE", "OperationalParty", null, null, DateTimeOffset.UtcNow);
        return queue(template, identity => JsonSerializer.Serialize(
            new OperationalPartyCreateRequest(
                name.Trim(), mobile.Trim(), null, null,
                new GeoAddressSnapshot(null, null, null, null, addressText.Trim()),
                identity.ClientOperationId)), cancellationToken);
    }

    private static void Validate(string name, string mobile, string addressText)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > 200 ||
            string.IsNullOrWhiteSpace(mobile) || mobile.Trim().Length > 40 ||
            string.IsNullOrWhiteSpace(addressText) || addressText.Trim().Length > 500)
            throw new OfflineStoreException(
                "BUSINESS_INPUT_INVALID", "Operational-party input is incomplete or exceeds its contract limit.");
    }
}
