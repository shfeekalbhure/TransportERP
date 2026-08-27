using TransportERP.Offline;

namespace TransportERP.Desktop.Offline;

/// <summary>
/// Production application boundary for a deliberately narrow first Offline business action.
/// The durable outbox identity is injected into the typed payload atomically by QueueAsync.
/// </summary>
public sealed class DesktopOfflineBusinessProducer(
    DesktopOfflineRuntime runtime,
    OfflineOperationScope scope)
{
    private readonly OperationalPartyOfflineProducer _producer =
        new OperationalPartyOfflineProducer(runtime.QueueAsync, scope);

    public Task<OfflineEnqueueResult> QueueOperationalPartyAsync(
        string name,
        string mobile,
        string addressText,
        CancellationToken cancellationToken = default)
        => _producer.QueueAsync(name, mobile, addressText, cancellationToken);
}
