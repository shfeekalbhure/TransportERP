using TransportERP.Offline.Transport;

namespace TransportERP.Mobile.Driver.Offline;

/// <summary>
/// Non-secret identity context supplied to a trusted device-registration binding verifier.
/// Implementations must resolve the binding within this exact company, branch and registered
/// device scope; a DeviceId string alone is never sufficient.
/// </summary>
public sealed record DriverDeviceKeyBindingContext(
    Guid CompanyId,
    Guid BranchId,
    Guid UserId,
    Guid RegisteredDeviceId,
    Guid SessionId,
    string DeviceId);

public enum DriverDeviceKeyBindingDecision
{
    Match,
    RegisteredBindingMissing,
    Mismatch,
    VerificationUnavailable
}

/// <summary>
/// Authenticated hosts inject an implementation backed by their governed registration state.
/// This contract deliberately does not assume or call a network endpoint.
/// </summary>
public interface IDriverDeviceKeyBindingVerifier
{
    ValueTask<DriverDeviceKeyBindingDecision> VerifyAsync(
        DriverDeviceKeyBindingContext context,
        DevicePublicP256Jwk currentPublicKey,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Production default: an absent trusted binding source can never authorize Offline activation.
/// </summary>
public sealed class DriverClosedDeviceKeyBindingVerifier : IDriverDeviceKeyBindingVerifier
{
    public ValueTask<DriverDeviceKeyBindingDecision> VerifyAsync(
        DriverDeviceKeyBindingContext context,
        DevicePublicP256Jwk currentPublicKey,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(currentPublicKey);
        cancellationToken.ThrowIfCancellationRequested();
        return ValueTask.FromResult(DriverDeviceKeyBindingDecision.VerificationUnavailable);
    }
}

/// <summary>
/// Ephemeral verifier populated only by the authenticated activation authority. The server's
/// public JWK and thumbprint are non-secret; they remain memory-only and exact-session scoped.
/// </summary>
public sealed class DriverServerDeviceKeyBindingVerifier : IDriverDeviceKeyBindingVerifier
{
    private readonly object _gate = new();
    private AuthorizedBinding? _binding;

    public ValueTask<DriverDeviceKeyBindingDecision> VerifyAsync(
        DriverDeviceKeyBindingContext context,
        DevicePublicP256Jwk currentPublicKey,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(currentPublicKey);
        cancellationToken.ThrowIfCancellationRequested();
        lock (_gate)
        {
            var binding = _binding;
            if (binding is null || binding.ExpiresAt <= DateTimeOffset.UtcNow || !binding.Matches(context))
                return ValueTask.FromResult(DriverDeviceKeyBindingDecision.VerificationUnavailable);
            return ValueTask.FromResult(
                string.Equals(binding.X, currentPublicKey.X, StringComparison.Ordinal) &&
                string.Equals(binding.Y, currentPublicKey.Y, StringComparison.Ordinal)
                    ? DriverDeviceKeyBindingDecision.Match
                    : DriverDeviceKeyBindingDecision.Mismatch);
        }
    }

    internal void Authorize(DriverServerActivationDecision decision, DateTimeOffset expiresAt)
    {
        ArgumentNullException.ThrowIfNull(decision);
        var publicKey = decision.ProofPublicJwk;
        lock (_gate)
        {
            _binding = decision.Enabled && decision.ProofKeyVersion is >= 1 && publicKey is not null &&
                publicKey.Kty == "EC" && publicKey.Crv == "P-256" &&
                !string.IsNullOrWhiteSpace(publicKey.X) && !string.IsNullOrWhiteSpace(publicKey.Y) &&
                expiresAt > DateTimeOffset.UtcNow
                    ? new(decision.CompanyId, decision.BranchId, decision.UserId,
                        decision.RegisteredDeviceId, decision.SessionId, decision.DeviceId,
                        publicKey.X, publicKey.Y, expiresAt)
                    : null;
        }
    }

    internal void Clear()
    {
        lock (_gate) _binding = null;
    }

    private sealed record AuthorizedBinding(
        Guid CompanyId,
        Guid BranchId,
        Guid UserId,
        Guid RegisteredDeviceId,
        Guid SessionId,
        string DeviceId,
        string X,
        string Y,
        DateTimeOffset ExpiresAt)
    {
        internal bool Matches(DriverDeviceKeyBindingContext context) =>
            CompanyId == context.CompanyId && BranchId == context.BranchId &&
            UserId == context.UserId && RegisteredDeviceId == context.RegisteredDeviceId &&
            SessionId == context.SessionId && string.Equals(DeviceId, context.DeviceId, StringComparison.Ordinal);
    }
}

/// <summary>
/// Opaque capability issued only after the governed verifier accepts the exact native public key.
/// External composition callers cannot construct one and therefore cannot bypass activation.
/// </summary>
public sealed class DriverVerifiedDeviceKeyBinding
{
    internal DriverVerifiedDeviceKeyBinding(
        DriverDeviceKeyBindingContext context,
        DevicePublicP256Jwk publicKey)
    {
        Context = context;
        PublicKey = publicKey;
    }

    internal DriverDeviceKeyBindingContext Context { get; }
    internal DevicePublicP256Jwk PublicKey { get; }
}

internal static class DriverDeviceKeyBindingGuard
{
    internal static async ValueTask<DriverVerifiedDeviceKeyBinding> RequireMatchAsync(
        DriverDeviceKeyBindingContext context,
        IDriverNativeDeviceSigningKey signingKey,
        IDriverDeviceKeyBindingVerifier verifier,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(signingKey);
        ArgumentNullException.ThrowIfNull(verifier);
        ValidateContext(context);

        // The signer is use-only. If the registered alias was lost, this call must fail with
        // DEVICE_KEY_REBIND_REQUIRED and must never provision a replacement implicitly.
        var currentPublicKey = await signingKey.GetPublicJwkAsync(cancellationToken);
        DriverDeviceKeyBindingDecision decision;
        try
        {
            decision = await verifier.VerifyAsync(context, currentPublicKey, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch
        {
            throw new DriverOfflineUnavailableException("DEVICE_KEY_BINDING_VERIFICATION_REQUIRED");
        }

        var failureCode = decision switch
        {
            DriverDeviceKeyBindingDecision.Match => null,
            DriverDeviceKeyBindingDecision.RegisteredBindingMissing => "DEVICE_KEY_REBIND_REQUIRED",
            DriverDeviceKeyBindingDecision.Mismatch => "DEVICE_KEY_ROTATION_REQUIRED",
            DriverDeviceKeyBindingDecision.VerificationUnavailable => "DEVICE_KEY_BINDING_VERIFICATION_REQUIRED",
            _ => "DEVICE_KEY_BINDING_VERIFICATION_REQUIRED"
        };
        if (failureCode is not null)
            throw new DriverOfflineUnavailableException(failureCode);
        return new DriverVerifiedDeviceKeyBinding(context, currentPublicKey);
    }

    internal static async ValueTask RequireStillCurrentAsync(
        DriverVerifiedDeviceKeyBinding verifiedBinding,
        DriverOfflineCompositionOptions options,
        IDriverNativeDeviceSigningKey signingKey,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(verifiedBinding);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(signingKey);
        var context = verifiedBinding.Context;
        if (context.CompanyId != options.CompanyId || context.BranchId != options.BranchId ||
            context.UserId != options.UserId ||
            context.RegisteredDeviceId != options.RegisteredDeviceId)
        {
            throw new DriverOfflineUnavailableException("DEVICE_KEY_BINDING_SCOPE_INVALID");
        }

        var currentPublicKey = await signingKey.GetPublicJwkAsync(cancellationToken);
        if (!string.Equals(currentPublicKey.X, verifiedBinding.PublicKey.X, StringComparison.Ordinal) ||
            !string.Equals(currentPublicKey.Y, verifiedBinding.PublicKey.Y, StringComparison.Ordinal))
        {
            throw new DriverOfflineUnavailableException("DEVICE_KEY_ROTATION_REQUIRED");
        }
    }

    private static void ValidateContext(DriverDeviceKeyBindingContext context)
    {
        if (context.CompanyId == Guid.Empty || context.BranchId == Guid.Empty ||
            context.UserId == Guid.Empty || context.RegisteredDeviceId == Guid.Empty ||
            context.SessionId == Guid.Empty || string.IsNullOrWhiteSpace(context.DeviceId) ||
            context.DeviceId.Any(char.IsWhiteSpace))
        {
            throw new DriverOfflineUnavailableException("DEVICE_KEY_BINDING_SCOPE_INVALID");
        }
    }
}
