using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TransportERP.Application.Sync;

namespace TransportERP.Desktop.Application;

internal static class DesktopBuildIdentityProbe
{
    internal static BuildIdentityV1 Measure(string? deploymentRoot = null)
    {
        var root = Path.GetFullPath(deploymentRoot ?? AppContext.BaseDirectory);
        if (!Directory.Exists(root)) throw new InvalidOperationException("BUILD_IDENTITY_UNAVAILABLE");
        var files = Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories)
            .Select(path => new
            {
                FullPath = path,
                RelativePath = Path.GetRelativePath(root, path).Replace('\\', '/')
            })
            .OrderBy(file => file.RelativePath, StringComparer.Ordinal)
            .ToArray();
        if (files.Length == 0 || files.Any(file => file.RelativePath.StartsWith("../", StringComparison.Ordinal)))
            throw new InvalidOperationException("BUILD_IDENTITY_UNAVAILABLE");

        using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        Span<byte> number = stackalloc byte[8];
        foreach (var file in files)
        {
            var pathBytes = Encoding.UTF8.GetBytes(file.RelativePath);
            BinaryPrimitives.WriteInt32BigEndian(number[..4], pathBytes.Length);
            hash.AppendData(number[..4]);
            hash.AppendData(pathBytes);
            using var stream = new FileStream(file.FullPath, FileMode.Open, FileAccess.Read,
                FileShare.ReadWrite | FileShare.Delete);
            BinaryPrimitives.WriteInt64BigEndian(number, stream.Length);
            hash.AppendData(number);
            var buffer = new byte[64 * 1024];
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                hash.AppendData(buffer, 0, read);
            CryptographicOperations.ZeroMemory(buffer);
        }

        var artifactDigest = Convert.ToHexString(hash.GetHashAndReset()).ToLowerInvariant();
        return new BuildIdentityV1(
            BuildIdentityV1.DesktopWindowsPlatform,
            artifactDigest,
            TrySignerCertificateDigest(deploymentRoot is null
                ? Environment.ProcessPath
                : Path.Combine(root, "TransportERP.Desktop.exe")));
    }

    private static string? TrySignerCertificateDigest(string? executablePath)
    {
        if (string.IsNullOrWhiteSpace(executablePath) || !File.Exists(executablePath)) return null;
        try
        {
            using var source = X509Certificate.CreateFromSignedFile(executablePath);
            using var certificate = new X509Certificate2(source);
            return BuildIdentityV1.Sha256LowerHex(certificate.RawData);
        }
        catch (CryptographicException)
        {
            return null;
        }
    }
}
