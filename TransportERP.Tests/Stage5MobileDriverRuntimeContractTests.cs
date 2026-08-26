namespace TransportERP.Tests;

public sealed class Stage5MobileDriverRuntimeContractTests
{
    [Fact]
    public void Driver_is_a_real_android_maui_executable_with_no_startup_enable_path()
    {
        var project = Read("TransportERP.Mobile.Driver", "TransportERP.Mobile.Driver.csproj");
        var program = Read("TransportERP.Mobile.Driver", "MauiProgram.cs");
        var activation = Read("TransportERP.Mobile.Driver", "Offline", "DriverOfflineActivationService.cs");

        Assert.Contains("<TargetFramework>net10.0-android</TargetFramework>", project, StringComparison.Ordinal);
        Assert.Contains("<OutputType>Exe</OutputType>", project, StringComparison.Ordinal);
        Assert.Contains("<UseMaui>true</UseMaui>", project, StringComparison.Ordinal);
        Assert.Contains("IDriverOfflineFeatureGate, DriverClosedOfflineFeatureGate", program, StringComparison.Ordinal);
        Assert.DoesNotContain("ActivateAsync(", program, StringComparison.Ordinal);
        Assert.Contains("!request.OfflineRuntimeAuthorized || !featureGate.IsOfflineRuntimeAuthorized",
            activation, StringComparison.Ordinal);
        Assert.Contains("RunSyncSupervisorAsync", activation, StringComparison.Ordinal);
        Assert.Contains("_supervisorCancellation?.Cancel()", activation, StringComparison.Ordinal);
        Assert.Contains("volatileSession.Clear()", activation, StringComparison.Ordinal);
        Assert.Contains("public sealed class DriverOfflineActivationRequest", activation, StringComparison.Ordinal);
        Assert.DoesNotContain("record DriverOfflineActivationRequest", activation, StringComparison.Ordinal);
    }

    [Fact]
    public void Android_native_security_is_purpose_separated_non_exportable_and_fail_closed()
    {
        var storage = Read("TransportERP.Mobile.Driver", "Platforms", "Android",
            "AndroidSecureStorageEncryptionKeyProvider.cs");
        var signer = Read("TransportERP.Mobile.Driver", "Platforms", "Android",
            "AndroidKeystoreDeviceSigningKey.cs");
        var manifest = Read("TransportERP.Mobile.Driver", "Platforms", "Android", "AndroidManifest.xml");

        Assert.Contains("SecureStorage.Default", storage, StringComparison.Ordinal);
        Assert.Contains("sqlcipher.outbox.v1", storage, StringComparison.Ordinal);
        Assert.Contains("sqlcipher.readcache.v1", storage, StringComparison.Ordinal);
        Assert.Contains("!CryptographicOperations.FixedTimeEquals(outbox, readCache)", storage,
            StringComparison.Ordinal);
        Assert.DoesNotContain("Preferences.", storage, StringComparison.Ordinal);
        Assert.DoesNotContain("File.Write", storage, StringComparison.Ordinal);

        Assert.Contains("AndroidKeyStore", signer, StringComparison.Ordinal);
        Assert.Contains("KeyStorePurpose.Sign | KeyStorePurpose.Verify", signer, StringComparison.Ordinal);
        Assert.Contains("ECGenParameterSpec(\"secp256r1\")", signer, StringComparison.Ordinal);
        Assert.Contains("DerToP1363", signer, StringComparison.Ordinal);
        Assert.Contains("new byte[64]", signer, StringComparison.Ordinal);
        Assert.DoesNotContain("ExportParameters(includePrivateParameters: true)", signer,
            StringComparison.Ordinal);
        Assert.DoesNotContain("ExportPkcs8PrivateKey", signer, StringComparison.Ordinal);

        Assert.Contains("android:allowBackup=\"false\"", manifest, StringComparison.Ordinal);
        Assert.Contains("android:usesCleartextTraffic=\"false\"", manifest, StringComparison.Ordinal);
    }

    private static string Read(params string[] path) =>
        File.ReadAllText(Path.Combine([RepositoryRoot(), .. path]));

    private static string RepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "TransportERP.slnx")))
            directory = directory.Parent;

        return directory?.FullName
            ?? throw new DirectoryNotFoundException("TransportERP repository root was not found.");
    }
}
