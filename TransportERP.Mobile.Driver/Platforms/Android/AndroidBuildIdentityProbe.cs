using Android.Content.PM;
using TransportERP.Application.Sync;

namespace TransportERP.Mobile.Driver.Platforms.Android;

internal static class AndroidBuildIdentityProbe
{
    internal static async Task<BuildIdentityV1> MeasureAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var context = global::Android.App.Application.Context;
        var sourcePath = context.ApplicationInfo?.SourceDir;
        if (string.IsNullOrWhiteSpace(sourcePath) || !File.Exists(sourcePath))
            throw new InvalidOperationException("BUILD_IDENTITY_UNAVAILABLE");
        string artifactDigest;
        await using (var stream = new global::System.IO.FileStream(
            sourcePath,
            global::System.IO.FileMode.Open,
            global::System.IO.FileAccess.Read,
            global::System.IO.FileShare.Read))
            artifactDigest = BuildIdentityV1.Sha256LowerHex(stream);

        IEnumerable<Signature>? signers;
        if (OperatingSystem.IsAndroidVersionAtLeast(28))
        {
            var packageInfo = context.PackageManager?.GetPackageInfo(
                context.PackageName!, PackageInfoFlags.SigningCertificates);
            signers = packageInfo?.SigningInfo?.GetApkContentsSigners();
        }
        else
        {
#pragma warning disable CS0618 // Required fail-closed signer measurement on supported API 23-27.
            signers = context.PackageManager?.GetPackageInfo(
                context.PackageName!, PackageInfoFlags.Signatures)?.Signatures;
#pragma warning restore CS0618
        }
        var signerArray = signers?.ToArray();
        if (signerArray is not { Length: 1 })
            throw new InvalidOperationException("BUILD_SIGNER_IDENTITY_UNAVAILABLE");
        var signerBytes = signerArray[0].ToByteArray();
        if (signerBytes is not { Length: > 0 })
            throw new InvalidOperationException("BUILD_SIGNER_IDENTITY_UNAVAILABLE");
        try
        {
            return new BuildIdentityV1(
                BuildIdentityV1.AndroidPlatform,
                artifactDigest,
                BuildIdentityV1.Sha256LowerHex(signerBytes));
        }
        finally
        {
            System.Security.Cryptography.CryptographicOperations.ZeroMemory(signerBytes);
        }
    }
}
