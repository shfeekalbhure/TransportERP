namespace TransportERP.Api.Security;

public interface IDeviceTrustResolver
{
    Task<bool> IsTrustedAsync(Guid userId, string deviceId, CancellationToken cancellationToken = default);
}

public sealed class DenyAllDeviceTrustResolver : IDeviceTrustResolver
{
    public Task<bool> IsTrustedAsync(Guid userId, string deviceId, CancellationToken cancellationToken = default)
        => Task.FromResult(false);
}
