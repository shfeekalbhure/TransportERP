using System.Diagnostics;
using System.Windows.Automation;
using TransportERP.Desktop.Application;

namespace TransportERP.Desktop.E2ETests;

/// <summary>
/// Drives the normal release WinExe exclusively through Windows UI Automation. Authentication
/// material is entered into the visible controls and is never passed through arguments,
/// environment variables, files, or a test-only application hook.
/// </summary>
internal sealed class DesktopReleaseUiAutomation : IAsyncDisposable
{
    private readonly Process _process;
    private AutomationElement? _window;

    private DesktopReleaseUiAutomation(Process process) => _process = process;

    internal string ExecutablePath => _process.MainModule?.FileName
        ?? throw new InvalidOperationException("DESKTOP_E2E_PROCESS_PATH_UNAVAILABLE");

    internal static async Task<DesktopReleaseUiAutomation> LaunchAsync(
        string executablePath,
        CancellationToken cancellationToken)
    {
        if (!OperatingSystem.IsWindows() || !Path.IsPathFullyQualified(executablePath) ||
            !File.Exists(executablePath))
            throw new InvalidOperationException("DESKTOP_E2E_EXECUTABLE_INVALID");
        var process = Process.Start(new ProcessStartInfo
        {
            FileName = Path.GetFullPath(executablePath),
            UseShellExecute = false,
            WorkingDirectory = Path.GetDirectoryName(Path.GetFullPath(executablePath))!
        }) ?? throw new InvalidOperationException("DESKTOP_E2E_PROCESS_START_FAILED");
        var driver = new DesktopReleaseUiAutomation(process);
        try
        {
            await driver.WaitForWindowAsync(cancellationToken);
            return driver;
        }
        catch
        {
            await driver.DisposeAsync();
            throw;
        }
    }

    internal async Task SignInAsync(
        string userName,
        string password,
        Guid companyId,
        Guid branchId,
        string deviceId,
        string deviceCredential,
        string certificateThumbprint,
        CancellationToken cancellationToken)
    {
        SetValue(DesktopAutomationIds.UserName, userName);
        SetValue(DesktopAutomationIds.Password, password);
        SetValue(DesktopAutomationIds.CompanyId, companyId.ToString("D"));
        SetValue(DesktopAutomationIds.BranchId, branchId.ToString("D"));
        SetValue(DesktopAutomationIds.DeviceId, deviceId);
        SetValue(DesktopAutomationIds.DeviceCredential, deviceCredential);
        SetValue(DesktopAutomationIds.CertificateThumbprint, certificateThumbprint);
        Invoke(DesktopAutomationIds.SignIn);
        await WaitForEnabledAsync(DesktopAutomationIds.QueueParty, cancellationToken);
    }

    internal async Task QueueOperationalPartyAsync(
        string name,
        string mobile,
        string address,
        CancellationToken cancellationToken)
    {
        SetValue(DesktopAutomationIds.PartyName, name);
        SetValue(DesktopAutomationIds.PartyMobile, mobile);
        SetValue(DesktopAutomationIds.PartyAddress, address);
        Invoke(DesktopAutomationIds.QueueParty);
        await WaitForStatusAsync("تمت إضافة العملية المشفرة", cancellationToken);
    }

    internal async Task WaitForPersistedSucceededOperationAsync(CancellationToken cancellationToken)
    {
        Invoke(DesktopAutomationIds.Operations);
        var operationsWindow = await WaitForOperationsWindowAsync(cancellationToken);
        while (!_process.HasExited)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var refresh = Element(operationsWindow, SyncOperationsAutomationIds.Refresh);
            if (refresh.Current.IsEnabled &&
                refresh.TryGetCurrentPattern(InvokePattern.Pattern, out var refreshPattern) &&
                refreshPattern is InvokePattern invoke)
                invoke.Invoke();
            await Task.Delay(150, cancellationToken);
            var summary = Element(operationsWindow, SyncOperationsAutomationIds.Summary).Current.Name;
            if (summary.Contains("آخر حالة: نجحت", StringComparison.Ordinal) &&
                summary.Contains("النتيجة: SUCCEEDED", StringComparison.Ordinal))
                return;
            if (summary.Contains("مرفوضة", StringComparison.Ordinal) ||
                summary.Contains("فشلت", StringComparison.Ordinal))
                throw new InvalidOperationException("DESKTOP_E2E_PERSISTED_OPERATION_FAILED");
            await Task.Delay(350, cancellationToken);
        }
        throw new InvalidOperationException($"DESKTOP_E2E_PROCESS_EXITED_{_process.ExitCode}");
    }

    internal async Task CloseNormallyAsync(CancellationToken cancellationToken)
    {
        if (_process.HasExited) return;
        if (!_process.CloseMainWindow())
            throw new InvalidOperationException("DESKTOP_E2E_NORMAL_CLOSE_UNAVAILABLE");
        await _process.WaitForExitAsync(cancellationToken);
    }

    internal string ReadStatus() => Element(DesktopAutomationIds.OfflineStatus)
        .Current.Name;

    internal void AssertClosedDefault()
    {
        if (!string.Equals(
                ReadStatus(),
                "العمل دون اتصال مغلق — يلزم تسجيل الدخول والتفويض",
                StringComparison.Ordinal) ||
            Element(DesktopAutomationIds.QueueParty).Current.IsEnabled ||
            Element(DesktopAutomationIds.Operations).Current.IsEnabled)
            throw new InvalidOperationException("DESKTOP_E2E_CLOSED_DEFAULT_VIOLATED");
    }

    private async Task WaitForWindowAsync(CancellationToken cancellationToken)
    {
        while (!_process.HasExited)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _process.Refresh();
            if (_process.MainWindowHandle != IntPtr.Zero)
            {
                var candidate = AutomationElement.FromHandle(_process.MainWindowHandle);
                if (candidate is not null && string.Equals(
                        candidate.Current.AutomationId, DesktopAutomationIds.Shell,
                        StringComparison.Ordinal))
                {
                    _window = candidate;
                    return;
                }
            }
            await Task.Delay(100, cancellationToken);
        }
        throw new InvalidOperationException($"DESKTOP_E2E_PROCESS_EXITED_{_process.ExitCode}");
    }

    private async Task WaitForEnabledAsync(string automationId, CancellationToken cancellationToken)
    {
        while (!_process.HasExited)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (Element(automationId).Current.IsEnabled) return;
            await Task.Delay(100, cancellationToken);
        }
        throw new InvalidOperationException($"DESKTOP_E2E_PROCESS_EXITED_{_process.ExitCode}");
    }

    private async Task WaitForStatusAsync(string prefix, CancellationToken cancellationToken)
    {
        while (!_process.HasExited)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var status = ReadStatus();
            if (status.StartsWith(prefix, StringComparison.Ordinal)) return;
            if (status.StartsWith("تعذر", StringComparison.Ordinal) ||
                status.StartsWith("توقفت", StringComparison.Ordinal) ||
                status.StartsWith("انتهت", StringComparison.Ordinal))
                throw new InvalidOperationException("DESKTOP_E2E_UI_REPORTED_FAILURE");
            await Task.Delay(100, cancellationToken);
        }
        throw new InvalidOperationException($"DESKTOP_E2E_PROCESS_EXITED_{_process.ExitCode}");
    }

    private AutomationElement Element(string automationId)
    {
        var window = _window ?? throw new InvalidOperationException("DESKTOP_E2E_WINDOW_UNAVAILABLE");
        return Element(window, automationId);
    }

    private static AutomationElement Element(AutomationElement root, string automationId) =>
        root.FindFirst(
                TreeScope.Descendants,
                new PropertyCondition(AutomationElement.AutomationIdProperty, automationId))
            ?? throw new InvalidOperationException($"DESKTOP_E2E_CONTROL_MISSING_{automationId}");

    private async Task<AutomationElement> WaitForOperationsWindowAsync(CancellationToken cancellationToken)
    {
        var processCondition = new PropertyCondition(
            AutomationElement.ProcessIdProperty, _process.Id);
        while (!_process.HasExited)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var windows = AutomationElement.RootElement.FindAll(TreeScope.Children, processCondition);
            foreach (AutomationElement window in windows)
                if (string.Equals(window.Current.AutomationId, SyncOperationsAutomationIds.Form,
                        StringComparison.Ordinal))
                    return window;
            await Task.Delay(100, cancellationToken);
        }
        throw new InvalidOperationException($"DESKTOP_E2E_PROCESS_EXITED_{_process.ExitCode}");
    }

    private void SetValue(string automationId, string value)
    {
        var element = Element(automationId);
        if (!element.TryGetCurrentPattern(ValuePattern.Pattern, out var pattern) ||
            pattern is not ValuePattern valuePattern || valuePattern.Current.IsReadOnly)
            throw new InvalidOperationException($"DESKTOP_E2E_CONTROL_NOT_WRITABLE_{automationId}");
        valuePattern.SetValue(value);
    }

    private void Invoke(string automationId)
    {
        var element = Element(automationId);
        if (!element.TryGetCurrentPattern(InvokePattern.Pattern, out var pattern) ||
            pattern is not InvokePattern invokePattern || !element.Current.IsEnabled)
            throw new InvalidOperationException($"DESKTOP_E2E_CONTROL_NOT_INVOKABLE_{automationId}");
        invokePattern.Invoke();
    }

    public async ValueTask DisposeAsync()
    {
        if (_process.HasExited)
        {
            _process.Dispose();
            return;
        }
        _process.CloseMainWindow();
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        try { await _process.WaitForExitAsync(timeout.Token); }
        catch (OperationCanceledException)
        {
            _process.Kill(entireProcessTree: true);
            await _process.WaitForExitAsync();
        }
        _process.Dispose();
    }
}
