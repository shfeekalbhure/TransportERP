using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace TransportERP.Desktop.Offline;

public sealed record DeviceProofPublicKey(string Curve, string X, string Y);

/// <summary>
/// A signing handle that never exposes device private-key material.
/// Signatures use the JOSE-required fixed-width R || S representation.
/// </summary>
public interface IDeviceProofSigningKey : IDisposable
{
    DeviceProofPublicKey PublicKey { get; }
    ValueTask<byte[]> SignHashAsync(ReadOnlyMemory<byte> hash, CancellationToken cancellationToken = default);
}

public interface IDeviceProofSigningKeyStore
{
    ValueTask<IDeviceProofSigningKey> OpenAsync(string certificateThumbprint, CancellationToken cancellationToken = default);
}

/// <summary>
/// Opens a non-exportable P-256 device key from the current user's Windows
/// certificate store. Provisioning/rotation remains an authenticated device
/// registration concern; this class cannot create or export credentials.
/// </summary>
public sealed class WindowsCertificateDeviceProofSigningKeyStore : IDeviceProofSigningKeyStore
{
    public ValueTask<IDeviceProofSigningKey> OpenAsync(
        string certificateThumbprint,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!OperatingSystem.IsWindows())
            throw new DeviceProofKeyStoreException("DEVICE_KEY_STORE_UNAVAILABLE", "The Windows certificate store is unavailable.");

        var normalized = NormalizeThumbprint(certificateThumbprint);
        using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
        var matches = store.Certificates.Find(X509FindType.FindByThumbprint, normalized, validOnly: false);
        if (matches.Count != 1)
            throw new DeviceProofKeyStoreException("DEVICE_KEY_NOT_FOUND", "The registered device signing key is unavailable or ambiguous.");

        var certificate = new X509Certificate2(matches[0]);
        try
        {
            var now = DateTime.UtcNow;
            if (now < certificate.NotBefore.ToUniversalTime() || now > certificate.NotAfter.ToUniversalTime())
                throw new DeviceProofKeyStoreException("DEVICE_KEY_EXPIRED", "The registered device signing key is outside its validity period.");

            ECDsa? signer = certificate.GetECDsaPrivateKey();
            try
            {
                if (signer is not ECDsaCng cng || cng.Key.KeySize != 256)
                    throw new DeviceProofKeyStoreException("DEVICE_KEY_INVALID", "The device key must be a Windows CNG P-256 private key.");

                var exportPolicy = cng.Key.ExportPolicy;
                if (exportPolicy != CngExportPolicies.None)
                    throw new DeviceProofKeyStoreException("DEVICE_KEY_EXPORTABLE", "Exportable device signing keys are prohibited.");

                var handle = new WindowsCertificateSigningKey(certificate, cng);
                signer = null; // Ownership moved into the returned handle.
                return ValueTask.FromResult<IDeviceProofSigningKey>(handle);
            }
            finally
            {
                signer?.Dispose();
            }
        }
        catch
        {
            certificate.Dispose();
            throw;
        }
    }

    private static string NormalizeThumbprint(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("A registered certificate thumbprint is required.", nameof(value));

        var normalized = new string(value.Where(character => !char.IsWhiteSpace(character)).ToArray()).ToUpperInvariant();
        if ((normalized.Length != 40 && normalized.Length != 64) || normalized.Any(character => !Uri.IsHexDigit(character)))
            throw new DeviceProofKeyStoreException("DEVICE_KEY_REFERENCE_INVALID", "The device key reference is invalid.");
        return normalized;
    }

    private sealed class WindowsCertificateSigningKey : IDeviceProofSigningKey
    {
        private readonly X509Certificate2 _certificate;
        private readonly ECDsaCng _signer;

        public WindowsCertificateSigningKey(X509Certificate2 certificate, ECDsaCng signer)
        {
            _certificate = certificate;
            _signer = signer;
            var parameters = signer.ExportParameters(includePrivateParameters: false);
            PublicKey = new DeviceProofPublicKey(
                "P-256",
                Base64Url(parameters.Q.X ?? throw new DeviceProofKeyStoreException("DEVICE_KEY_INVALID", "The public key is incomplete.")),
                Base64Url(parameters.Q.Y ?? throw new DeviceProofKeyStoreException("DEVICE_KEY_INVALID", "The public key is incomplete.")));
        }

        public DeviceProofPublicKey PublicKey { get; }

        public ValueTask<byte[]> SignHashAsync(ReadOnlyMemory<byte> hash, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (hash.Length != 32)
                throw new ArgumentException("A SHA-256 hash is required.", nameof(hash));

            return ValueTask.FromResult(_signer.SignHash(
                hash.Span,
                DSASignatureFormat.IeeeP1363FixedFieldConcatenation));
        }

        public void Dispose()
        {
            _signer.Dispose();
            _certificate.Dispose();
        }

        private static string Base64Url(byte[] bytes) =>
            Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }
}

public sealed class DeviceProofKeyStoreException : Exception
{
    public DeviceProofKeyStoreException(string code, string message, Exception? innerException = null)
        : base(message, innerException) => Code = code;

    public string Code { get; }
}
