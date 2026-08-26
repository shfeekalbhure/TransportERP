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
            // AndroidKeyStore providers are only required to implement the DER ECDSA form.
            // Do not infer that a 64-byte result from an optional provider alias is P1363:
            // a DER signature can also be 64 bytes.  Normalize the required DER form with the
            // strict parser below so the wire representation is always unambiguous JOSE P1363.
            using var derSigner = Java.Security.Signature.GetInstance("SHA256withECDSA")
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
            derSigner.InitSign(privateKey);
            derSigner.Update(input);
            derSignature = derSigner.Sign();
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

    /// <summary>
    /// Production enrollment/recovery seam. The opaque one-use authority can only be issued by
    /// the authenticated activation coordinator after an exact-scope server decision. A signing
    /// or activation path can never create or replace the alias implicitly.
    /// </summary>
    internal async ValueTask ProvisionForAuthorizedEnrollmentAsync(
        DriverDeviceKeyEnrollmentAuthorization authorization,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(authorization);
        authorization.Consume();
        await GenerateFreshKeyAsync("DEVICE_KEY_ALREADY_PROVISIONED", cancellationToken);
    }

    internal async ValueTask ReplaceForAuthorizedRecoveryAsync(
        DriverDeviceKeyEnrollmentAuthorization authorization,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(authorization);
        if (authorization.ChangeType != "RECOVER")
            throw new DriverOfflineUnavailableException("DEVICE_KEY_ENROLLMENT_AUTHORITY_INVALID");
        authorization.Consume();
        await _gate.WaitAsync(cancellationToken);
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            using var keyStore = LoadKeyStore();
            if (keyStore.ContainsAlias(KeyAlias)) keyStore.DeleteEntry(KeyAlias);
            GenerateFreshKey(keyStore);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) { throw; }
        catch (DriverOfflineUnavailableException) { throw; }
        catch (Exception exception)
        {
            throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE", exception);
        }
        finally { _gate.Release(); }
    }

    private async ValueTask GenerateFreshKeyAsync(
        string existingKeyCode,
        CancellationToken cancellationToken)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            using var keyStore = LoadKeyStore();
            if (keyStore.ContainsAlias(KeyAlias))
                throw new DriverOfflineUnavailableException(existingKeyCode);

            GenerateFreshKey(keyStore);
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

    private static void GenerateFreshKey(KeyStore keyStore)
    {
        if (keyStore.ContainsAlias(KeyAlias))
            throw new DriverOfflineUnavailableException("DEVICE_KEY_ALREADY_PROVISIONED");
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

#if TRANSPORTERP_DEVICE_TESTS
    internal sealed record AndroidSignatureDiagnostics(
        bool JavaDerSignatureVerified,
        bool DerP1363RoundTripVerified,
        bool PublicJwkMatchesCertificate,
        bool ProductionP1363Verified);

    /// <summary>
    /// Test-APK-only diagnosis. Only booleans leave this method; signature, SPKI and coordinates
    /// are zeroed and are never persisted by the device-test activity.
    /// </summary>
    internal async ValueTask<AndroidSignatureDiagnostics> DiagnoseSignatureForDeviceTestAsync(
        ReadOnlyMemory<byte> signingInput,
        ReadOnlyMemory<byte> productionP1363,
        DevicePublicP256Jwk expectedPublicJwk,
        CancellationToken cancellationToken = default)
    {
        if (signingInput.IsEmpty || productionP1363.Length != 64)
            return new(false, false, false, false);
        await RequireExistingKeyAsync(cancellationToken);
        var input = signingInput.ToArray();
        var p1363 = productionP1363.ToArray();
        byte[]? rawDer = null;
        byte[]? roundTripDer = null;
        byte[]? spki = null;
        var javaDerVerified = false;
        var roundTripVerified = false;
        var jwkMatches = false;
        var productionVerified = false;
        try
        {
            using var keyStore = LoadKeyStore();
            using var privateKey = keyStore.GetKey(KeyAlias, null) as IPrivateKey
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
            using var certificate = keyStore.GetCertificate(KeyAlias)
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
            using var certificatePublicKey = certificate.PublicKey
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
            spki = certificatePublicKey.GetEncoded()
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
            using var keyFactory = KeyFactory.GetInstance(KeyProperties.KeyAlgorithmEc)
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
            using var publicKeySpec = new X509EncodedKeySpec(spki);
            using var publicKey = keyFactory.GeneratePublic(publicKeySpec) as IPublicKey
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");

            using (var rawSigner = Java.Security.Signature.GetInstance("SHA256withECDSA")
                       ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE"))
            {
                rawSigner.InitSign(privateKey);
                rawSigner.Update(input);
                rawDer = rawSigner.Sign();
            }

            javaDerVerified = VerifyDer(publicKey, input, rawDer);
            var converted = DerToP1363(rawDer);
            try
            {
                roundTripDer = P1363ToDer(converted);
            }
            finally
            {
                CryptographicOperations.ZeroMemory(converted);
            }
            roundTripVerified = VerifyDer(publicKey, input, roundTripDer);
            var productionDer = P1363ToDer(p1363);
            try { productionVerified = VerifyDer(publicKey, input, productionDer); }
            finally { CryptographicOperations.ZeroMemory(productionDer); }

            using var publicEcdsa = ECDsa.Create();
            publicEcdsa.ImportSubjectPublicKeyInfo(spki, out var consumed);
            var parameters = publicEcdsa.ExportParameters(includePrivateParameters: false);
            jwkMatches = consumed == spki.Length && parameters.Q.X is { Length: 32 } x &&
                parameters.Q.Y is { Length: 32 } y &&
                string.Equals(Base64Url(x), expectedPublicJwk.X, StringComparison.Ordinal) &&
                string.Equals(Base64Url(y), expectedPublicJwk.Y, StringComparison.Ordinal);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) { throw; }
        catch
        {
            // The fixed boolean matrix is the only diagnostic emitted by the device test.
        }
        finally
        {
            CryptographicOperations.ZeroMemory(input);
            CryptographicOperations.ZeroMemory(p1363);
            if (rawDer is not null) CryptographicOperations.ZeroMemory(rawDer);
            if (roundTripDer is not null) CryptographicOperations.ZeroMemory(roundTripDer);
            if (spki is not null) CryptographicOperations.ZeroMemory(spki);
        }
        return new(javaDerVerified, roundTripVerified, jwkMatches, productionVerified);
    }

    private static bool VerifyDer(IPublicKey publicKey, byte[] input, byte[] der)
    {
        using var verifier = Java.Security.Signature.GetInstance("SHA256withECDSA")
            ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
        verifier.InitVerify(publicKey);
        verifier.Update(input);
        return verifier.Verify(der);
    }

    /// <summary>
    /// Test-APK-only enrollment seam. Production binaries have no implicit or callable key
    /// creation path; registration/rebind must be implemented by a separately governed flow.
    /// </summary>
    internal async ValueTask ProvisionFreshKeyForDeviceTestAsync(
        CancellationToken cancellationToken = default)
    {
        await GenerateFreshKeyAsync("DEVICE_TEST_KEY_ALREADY_EXISTS", cancellationToken);
    }

    /// <summary>
    /// Test-APK-only native verification of the JOSE P1363 bytes returned by the production
    /// signer. Re-encoding to canonical X9.62 DER and verifying through the same Android
    /// provider proves both coordinates without relying on platform-specific .NET ECDsa support.
    /// Linux server tests independently verify the same 64-byte format with .NET.
    /// </summary>
    internal async ValueTask<bool> VerifyP1363ForDeviceTestAsync(
        ReadOnlyMemory<byte> signingInput,
        ReadOnlyMemory<byte> p1363Signature,
        CancellationToken cancellationToken = default)
    {
        if (signingInput.IsEmpty || p1363Signature.Length != 64)
            return false;
        await RequireExistingKeyAsync(cancellationToken);
        var input = signingInput.ToArray();
        var p1363 = p1363Signature.ToArray();
        byte[]? der = null;
        byte[]? spki = null;
        try
        {
            using var keyStore = LoadKeyStore();
            using var certificate = keyStore.GetCertificate(KeyAlias)
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
            using var certificatePublicKey = certificate.PublicKey
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
            spki = certificatePublicKey.GetEncoded()
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
            using var keyFactory = KeyFactory.GetInstance(KeyProperties.KeyAlgorithmEc)
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
            using var publicKeySpec = new X509EncodedKeySpec(spki);
            using var publicKey = keyFactory.GeneratePublic(publicKeySpec) as IPublicKey
                ?? throw new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE");
            der = P1363ToDer(p1363);
            cancellationToken.ThrowIfCancellationRequested();
            return VerifyDer(publicKey, input, der);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(input);
            CryptographicOperations.ZeroMemory(p1363);
            if (der is not null)
                CryptographicOperations.ZeroMemory(der);
            if (spki is not null)
                CryptographicOperations.ZeroMemory(spki);
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

#if TRANSPORTERP_DEVICE_TESTS
    private static byte[] P1363ToDer(ReadOnlySpan<byte> signature)
    {
        if (signature.Length != 64)
            throw new CryptographicException("An ES256 P1363 signature must be 64 bytes.");
        var r = CanonicalDerInteger(signature[..32]);
        var s = CanonicalDerInteger(signature[32..]);
        var result = new byte[2 + 2 + r.Length + 2 + s.Length];
        var offset = 0;
        result[offset++] = 0x30;
        result[offset++] = checked((byte)(result.Length - 2));
        result[offset++] = 0x02;
        result[offset++] = checked((byte)r.Length);
        r.CopyTo(result.AsSpan(offset));
        offset += r.Length;
        result[offset++] = 0x02;
        result[offset++] = checked((byte)s.Length);
        s.CopyTo(result.AsSpan(offset));
        return result;
    }

    private static byte[] CanonicalDerInteger(ReadOnlySpan<byte> coordinate)
    {
        var first = 0;
        while (first < coordinate.Length - 1 && coordinate[first] == 0)
            first++;
        var value = coordinate[first..];
        var prefix = (value[0] & 0x80) == 0 ? 0 : 1;
        var result = new byte[value.Length + prefix];
        value.CopyTo(result.AsSpan(prefix));
        return result;
    }
#endif

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
