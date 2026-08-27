#if TRANSPORTERP_DEVICE_TESTS
using System.Text.Json;
using Android.App;
using Android.OS;
using TransportERP.Mobile.Driver.DeviceTesting;

namespace TransportERP.Mobile.Driver;

[Activity(
    Name = "com.transporterp.mobile.driver.DriverDeviceTestActivity",
    Exported = true,
    Enabled = true,
    ExcludeFromRecents = true,
    NoHistory = true)]
public sealed class DriverDeviceTestActivity : Activity
{
    private const string PhaseExtra = "phase";
    private const string ResultFileName = "driver-device-test-result.json";
    private const string StateFileName = "driver-device-test-state.json";
    private const string E2eStateFileName = "driver-device-e2e-state.json";
    private const string E2eInputFileName = "driver-device-e2e-input.json";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    protected override async void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        var phase = Intent?.GetStringExtra(PhaseExtra) ?? string.Empty;
        var filesDirectory = FilesDir?.AbsolutePath;
        if (string.IsNullOrWhiteSpace(filesDirectory))
        {
            Finish();
            return;
        }

        var resultPath = Path.Combine(filesDirectory, ResultFileName);
        var statePath = Path.Combine(filesDirectory, StateFileName);
        try
        {
            File.Delete(resultPath);
            var e2eInputPath = Path.Combine(filesDirectory, E2eInputFileName);
            var e2eConfiguration = await ReadE2eConfigurationAsync(e2eInputPath);
            var result = await AndroidDriverRuntimeSelfTest.RunAsync(
                phase,
                statePath,
                Path.Combine(filesDirectory, E2eStateFileName),
                e2eConfiguration,
                CancellationToken.None);
            if (phase == "e2e-verify" && result.Passed)
                File.Delete(e2eInputPath);
            await WriteResultAtomicallyAsync(resultPath, result);
        }
        catch
        {
            // Evidence is deliberately limited to a fixed code. Exception messages, stack traces,
            // key material and request artifacts are never written by this test-only activity.
            var failure = DriverDeviceTestResult.Failure(phase, "RUNTIME_EXCEPTION");
            await WriteResultAtomicallyAsync(resultPath, failure);
        }
        finally
        {
            FinishAndRemoveTask();
        }
    }

    private static async Task<DriverDeviceE2eConfiguration> ReadE2eConfigurationAsync(string path)
    {
        try
        {
            if (!File.Exists(path)) return DriverDeviceE2eConfiguration.Invalid;
            return JsonSerializer.Deserialize<DriverDeviceE2eConfiguration>(
                       await File.ReadAllTextAsync(path), JsonOptions)
                   ?? DriverDeviceE2eConfiguration.Invalid;
        }
        catch (JsonException)
        {
            return DriverDeviceE2eConfiguration.Invalid;
        }
    }

    private static async Task WriteResultAtomicallyAsync(
        string resultPath,
        DriverDeviceTestResult result)
    {
        var temporaryPath = resultPath + ".tmp";
        var json = JsonSerializer.Serialize(result, JsonOptions);
        await File.WriteAllTextAsync(temporaryPath, json);
        File.Move(temporaryPath, resultPath, overwrite: true);
    }
}
#endif
