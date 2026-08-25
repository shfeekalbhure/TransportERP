using TransportERP.Api.Identity;

namespace TransportERP.Api.Security;

public interface IDeviceTrustResolver
{
    Task<TrustedDeviceBinding?> ResolveForSyncAsync(CurrentSecurityContext current, string deviceId,
        string? credential, Guid correlationId, CancellationToken cancellationToken = default);
}

public sealed class RegisteredDeviceTrustResolver(TransportERP.Api.Identity.RegisteredDeviceService devices)
    : IDeviceTrustResolver
{
    public async Task<TrustedDeviceBinding?> ResolveForSyncAsync(CurrentSecurityContext current, string deviceId,
        string? credential, Guid correlationId, CancellationToken cancellationToken = default)
    {
        if (!current.IsLocalSession || !current.SessionId.HasValue || !current.RegisteredDeviceId.HasValue ||
            !current.DeviceCredentialVersion.HasValue) return null;
        var binding = await devices.ValidateBindingAsync(current.UserId, current.CompanyId, current.BranchId,
            deviceId, credential, updateLastSeen: true, correlationId, cancellationToken);
        return binding is not null && binding.RegisteredDeviceId == current.RegisteredDeviceId &&
               binding.CredentialVersion == current.DeviceCredentialVersion ? binding : null;
    }
}
