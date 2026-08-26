using System.Security.Cryptography;
using Android.Security.Keystore;
using Java.Security;
using Java.Security.Spec;
using TransportERP.Mobile.Driver.Offline;
using TransportERP.Offline.Transport;

namespace TransportERP.Mobile.Driver.Platforms.Android;

/// <summary>
/// Uses a pre-enrolled, non-exportable P-256 private key in Android Keystore. Signing paths never
/// provision or replace a missing alias. Only the public SPKI and signing operation leave
/// Keystore; the Android DER ECDSA result is normalized to JOSE P1363 format.
/// </summary>
public sealed class AndroidKeystoreDeviceSigningKey : IDriverNativeDeviceSigningKey
{
    private const string KeyAlias = "transporterp.driver.device-pop.p256.v1";
    private const string AndroidKeyStore = "AndroidKeyStore";
    private readonly SemaphoreSlim _gate = new(1, 1);

    public async ValueTask<bool> IsNativeSigningKeyAvailableAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            await RequireExistingKeyAsync(cancellationToken);
            return true;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch
        {
            return false;
        }
    }

    public async ValueTask<DevicePublicP256Jwk> GetPublicJwkAsync(
        CancellationToken cancellationToken = default)
    {
        await RequireExistingKeyAsync(cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            using var keyStore = LoadKeyStore();
            using var certificate = keyStore.GetCertificate(KeyAlias)
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
            using var publicKey = certificate.PublicKey
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
            var subjectPublicKeyInfo = publicKey.GetEncoded()
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
            try
            {
                using var publicEcdsa = ECDsa.Create();
                publicEcdsa.ImportSubjectPublicKeyInfo(subjectPublicKeyInfo, out var consumed);
                if (consumed != subjectPublicKeyInfo.Length)
                    throw new CryptographicException("Unexpected trailing public-key data.");
                var parameters = publicEcdsa.ExportParameters(includePrivateParameters: false);
                if (parameters.Q.X is not { Length: 32 } x || parameters.Q.Y is not { Length: 32 } y)
                    throw new CryptographicException("The Android key is not P-256.");
                return new(Base64Url(x), Base64Url(y));
            }
            finally
            {
                CryptographicOperations.ZeroMemory(subjectPublicKeyInfo);
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (DriverOfflineUnavailableException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE", exception);
        }
    }

    public async ValueTask<byte[]> SignEs256Async(
        ReadOnlyMemory<byte> signingInput,
        CancellationToken cancellationToken = default)
    {
        if (signingInput.IsEmpty)
            throw new ArgumentException("A signing input is required.", nameof(signingInput));
        await RequireExistingKeyAsync(cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();

        var input = signingInput.ToArray();
        byte[]? derSignature = null;
        try
        {
            using var keyStore = LoadKeyStore();
            using var privateKey = keyStore.GetKey(KeyAlias, null) as IPrivateKey
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
            using var signer = Java.Security.Signature.GetInstance("SHA256withECDSA")
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
            signer.InitSign(privateKey);
            signer.Update(input);
            derSignature = signer.Sign();
            cancellationToken.ThrowIfCancellationRequested();
            return DerToP1363(derSignature);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (DriverOfflineUnavailableException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE", exception);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(input);
            if (derSignature is not null)
                CryptographicOperations.ZeroMemory(derSignature);
        }
    }

    private async Task RequireExistingKeyAsync(CancellationToken cancellationToken)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            using var keyStore = LoadKeyStore();
            if (!keyStore.ContainsAlias(KeyAlias))
                throw new DriverOfflineUnavailableException("DEVICE_KEY_REBIND_REQUIRED");
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (DriverOfflineUnavailableException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE", exception);
        }
        finally
        {
            _gate.Release();
        }
    }

#if TRANSPORTERP_DEVICE_TESTS
    /// <summary>
    /// Test-APK-only enrollment seam. Production binaries have no implicit or callable key
    /// creation path; registration/rebind must be implemented by a separately governed flow.
    /// </summary>
    internal async ValueTask ProvisionFreshKeyForDeviceTestAsync(
        CancellationToken cancellationToken = default)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            using var keyStore = LoadKeyStore();
            if (keyStore.ContainsAlias(KeyAlias))
                throw new DriverOfflineUnavailableException("DEVICE_TEST_KEY_ALREADY_EXISTS");

            using var generator = KeyPairGenerator.GetInstance(KeyProperties.KeyAlgorithmEc, AndroidKeyStore)
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
            using var curve = new ECGenParameterSpec("secp256r1");
            using var specification = new KeyGenParameterSpec.Builder(
                    KeyAlias,
                    KeyStorePurpose.Sign | KeyStorePurpose.Verify)
                .SetAlgorithmParameterSpec(curve)
                .SetDigests(KeyProperties.DigestSha256)
                .SetUserAuthenticationRequired(false)
                .Build();
            generator.Initialize(specification);
            using var generated = generator.GenerateKeyPair()
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (DriverOfflineUnavailableException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE", exception);
        }
        finally
        {
            _gate.Release();
        }
    }
#endif

    private static KeyStore LoadKeyStore()
    {
        var keyStore = KeyStore.GetInstance(AndroidKeyStore)
            ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
        keyStore.Load(null);
        return keyStore;
    }

    private static byte[] DerToP1363(ReadOnlySpan<byte> der)
    {
        var offset = 0;
        ReadTag(der, ref offset, 0x30);
        var sequenceLength = ReadLength(der, ref offset);
        if (sequenceLength != der.Length - offset)
            throw new CryptographicException("Invalid ECDSA DER sequence length.");

        var r = ReadInteger(der, ref offset);
        var s = ReadInteger(der, ref offset);
        if (offset != der.Length)
            throw new CryptographicException("Unexpected ECDSA DER trailing data.");

        var signature = new byte[64];
        CopyUnsignedCoordinate(r, signature.AsSpan(0, 32));
        CopyUnsignedCoordinate(s, signature.AsSpan(32, 32));
        return signature;
    }

    private static ReadOnlySpan<byte> ReadInteger(ReadOnlySpan<byte> source, ref int offset)
    {
        ReadTag(source, ref offset, 0x02);
        var length = ReadLength(source, ref offset);
        if (length is < 1 or > 33 || offset + length > source.Length)
            throw new CryptographicException("Invalid ECDSA DER integer.");
        var integer = source.Slice(offset, length);
        offset += length;
        if ((integer[0] & 0x80) != 0 ||
            (integer.Length > 1 && integer[0] == 0 && (integer[1] & 0x80) == 0))
            throw new CryptographicException("Non-canonical ECDSA DER integer.");
        return integer[0] == 0 ? integer[1..] : integer;
    }

    private static int ReadLength(ReadOnlySpan<byte> source, ref int offset)
    {
        if (offset >= source.Length)
            throw new CryptographicException("Truncated ECDSA DER length.");
        var first = source[offset++];
        if (first < 0x80)
            return first;
        var count = first & 0x7f;
        if (count is < 1 or > 2 || offset + count > source.Length)
            throw new CryptographicException("Invalid ECDSA DER length.");
        var length = 0;
        for (var index = 0; index < count; index++)
            length = checked((length << 8) | source[offset++]);
        if (length < 0x80)
            throw new CryptographicException("Non-canonical ECDSA DER length.");
        return length;
    }

    private static void ReadTag(ReadOnlySpan<byte> source, ref int offset, byte expected)
    {
        if (offset >= source.Length || source[offset++] != expected)
            throw new CryptographicException("Invalid ECDSA DER tag.");
    }

    private static void CopyUnsignedCoordinate(ReadOnlySpan<byte> source, Span<byte> destination)
    {
        if (source.IsEmpty || source.Length > destination.Length)
            throw new CryptographicException("ECDSA coordinate is outside P-256.");
        source.CopyTo(destination[^source.Length..]);
    }

    private static string Base64Url(ReadOnlySpan<byte> value) =>
        Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');
}
