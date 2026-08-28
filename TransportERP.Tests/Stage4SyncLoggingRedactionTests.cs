namespace TransportERP.Tests;

public sealed class Stage4SyncLoggingRedactionTests
{
    [Fact]
    public void Sync_workers_log_only_failure_types_and_never_exception_content_or_secret_fields()
    {
        var syncDirectory = Path.Combine(RepositoryRoot(), "TransportERP.Api", "Sync");
        var workerFiles = Directory.GetFiles(syncDirectory, "*Worker.cs", SearchOption.TopDirectoryOnly);
        Assert.NotEmpty(workerFiles);

        foreach (var path in workerFiles)
        {
            var source = File.ReadAllText(path);
            Assert.DoesNotContain("LogError(exception", source, StringComparison.Ordinal);
            Assert.DoesNotContain("exception.Message", source, StringComparison.Ordinal);
            Assert.DoesNotContain("exception.ToString", source, StringComparison.Ordinal);

            foreach (var secret in new[]
                     {
                         "PayloadJson", "Authorization", "Bearer", "DPoP", "{Nonce}", "{Jti}",
                         "CredentialHash", "RefreshToken", "ProofPublicJwkCanonicalJson"
                     })
            {
                Assert.DoesNotContain(secret, source, StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    private static string RepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "TransportERP.slnx")))
            directory = directory.Parent;

        return directory?.FullName
            ?? throw new DirectoryNotFoundException("TransportERP repository root was not found.");
    }
}
