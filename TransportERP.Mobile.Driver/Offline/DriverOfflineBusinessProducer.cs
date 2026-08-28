using TransportERP.Offline;

namespace TransportERP.Mobile.Driver.Offline;

/// <summary>
/// Driver application's first production Offline business producer. It binds the generated,
/// durable ClientOperationId into the typed business payload inside QueueAsync.
/// </summary>
public sealed class DriverOfflineBusinessProducer(
    DriverOfflineRuntime runtime,
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
