using System.Diagnostics;
using System.IO;
using System.Windows.Automation;
using TransportERP.Desktop.Application;
using TransportERP.Desktop.Offline;

namespace TransportERP.Desktop.E2ETests;

/// <summary>
/// Drives the normal release WinExe exclusively through Windows UI Automation. Authentication
/// material is entered into the visible controls and is never passed through arguments,
/// environment variables, files, or a test-only application hook.
/// </summary>
internal sealed class DesktopReleaseUiAutomation : IAsyncDisposable
{
    private static readonly TimeSpan NormalCloseTimeout = TimeSpan.FromSeconds(15);
    private static readonly TimeSpan CleanupExitTimeout = TimeSpan.FromSeconds(5);
    private static readonly TimeSpan OperationsWindowDiagnosticDelay = TimeSpan.FromSeconds(15);
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
        RequireRunningBeforeClose(_process);
        if (cancellationToken.IsCancellationRequested)
            throw new InvalidOperationException("DESKTOP_E2E_PARENT_BUDGET_EXHAUSTED");
        if (!_process.CloseMainWindow())
        {
            if (_process.HasExited)
                throw new InvalidOperationException("DESKTOP_E2E_PROCESS_EXITED_BEFORE_CLOSE");
            throw new InvalidOperationException("DESKTOP_E2E_NORMAL_CLOSE_UNAVAILABLE");
        }
        await WaitForNormalExitAsync(_process, NormalCloseTimeout, cancellationToken);
    }

    internal static void RequireRunningBeforeClose(Process process)
    {
        ArgumentNullException.ThrowIfNull(process);
        if (process.HasExited)
            throw new InvalidOperationException("DESKTOP_E2E_PROCESS_EXITED_BEFORE_CLOSE");
    }

    internal static async Task WaitForNormalExitAsync(
        Process process,
        TimeSpan closeTimeout,
        CancellationToken parentCancellation)
    {
        ArgumentNullException.ThrowIfNull(process);
        if (closeTimeout <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(closeTimeout));
        if (parentCancellation.IsCancellationRequested)
            throw new InvalidOperationException("DESKTOP_E2E_PARENT_BUDGET_EXHAUSTED");
        if (process.HasExited)
        {
            EnsureNormalExit(process);
            return;
        }

        using var phase = CancellationTokenSource.CreateLinkedTokenSource(parentCancellation);
        phase.CancelAfter(closeTimeout);
        try
        {
            await process.WaitForExitAsync(phase.Token);
        }
        catch (OperationCanceledException)
        {
            if (parentCancellation.IsCancellationRequested)
                throw new InvalidOperationException("DESKTOP_E2E_PARENT_BUDGET_EXHAUSTED");
            process.Refresh();
            if (process.HasExited)
            {
                EnsureNormalExit(process);
                return;
            }
            throw new InvalidOperationException("DESKTOP_E2E_NORMAL_CLOSE_TIMEOUT");
        }
        EnsureNormalExit(process);
    }

    private static void EnsureNormalExit(Process process)
    {
        if (process.ExitCode != 0)
            throw new InvalidOperationException("DESKTOP_E2E_NORMAL_CLOSE_NONZERO");
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
        var operationsWindowCondition = new AndCondition(
            processCondition,
            new PropertyCondition(
                AutomationElement.AutomationIdProperty, SyncOperationsAutomationIds.Form),
            new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window));
        var diagnosticAt = DateTimeOffset.UtcNow + OperationsWindowDiagnosticDelay;
        var diagnosticEmitted = false;
        var consecutiveUnexpectedWindowSamples = 0;
        while (!_process.HasExited)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var diagnosticState = ReadOperationsDiagnosticState();
            if (string.Equals(diagnosticState, "DESKTOP_OPERATIONS_SUPERVISOR_STOPPED",
                    StringComparison.Ordinal))
                throw new InvalidOperationException("DESKTOP_E2E_SUPERVISOR_STOPPED");
            if (string.Equals(diagnosticState, "DESKTOP_OPERATIONS_RUNTIME_MISSING",
                    StringComparison.Ordinal))
                throw new InvalidOperationException("DESKTOP_E2E_OPERATIONS_RUNTIME_MISSING");
            if (string.Equals(diagnosticState, "DESKTOP_OPERATIONS_CREATE_FAILED",
                    StringComparison.Ordinal))
                throw new InvalidOperationException("DESKTOP_E2E_OPERATIONS_CREATE_FAILURE");
            if (string.Equals(diagnosticState, "DESKTOP_OPERATIONS_SHOW_FAILED",
                    StringComparison.Ordinal))
                throw new InvalidOperationException("DESKTOP_E2E_OPERATIONS_SHOW_FAILURE");
            if (string.Equals(diagnosticState, "DESKTOP_OPERATIONS_WINDOW_NOT_VISIBLE",
                    StringComparison.Ordinal))
                throw new InvalidOperationException("DESKTOP_E2E_OPERATIONS_WINDOW_NOT_VISIBLE");

            // WinForms exposes an owned modeless Form below its owner in some UIA providers,
            // rather than as a direct child of the desktop root. Keep both process and exact
            // AutomationId in the query; a title, class name or arbitrary descendant is never
            // accepted as the governed operations surface.
            var operationsWindow = AutomationElement.RootElement.FindFirst(
                TreeScope.Descendants, operationsWindowCondition);
            if (operationsWindow is not null)
                return operationsWindow;

            var windows = AutomationElement.RootElement.FindAll(TreeScope.Children, processCondition);
            var unexpectedWindowObserved = false;
            foreach (AutomationElement window in windows)
            {
                var automationId = window.Current.AutomationId;
                if (string.Equals(automationId, SyncOperationsAutomationIds.Form,
                        StringComparison.Ordinal))
                    return window;
                if (!string.Equals(automationId, DesktopAutomationIds.Shell,
                        StringComparison.Ordinal))
                    unexpectedWindowObserved = true;
            }
            consecutiveUnexpectedWindowSamples = unexpectedWindowObserved
                ? consecutiveUnexpectedWindowSamples + 1
                : 0;

            if (!diagnosticEmitted && DateTimeOffset.UtcNow >= diagnosticAt)
            {
                var code = consecutiveUnexpectedWindowSamples >= 2
                    ? "DESKTOP_E2E_UNEXPECTED_WINDOW"
                    : ClassifyOperationsWindowTimeout(diagnosticState);
                Console.WriteLine($"DESKTOP_OPERATIONS_DIAGNOSTIC:{code}");
                diagnosticEmitted = true;
            }
            await Task.Delay(100, cancellationToken);
        }
        throw new InvalidOperationException($"DESKTOP_E2E_PROCESS_EXITED_{_process.ExitCode}");
    }

    private string ReadOperationsDiagnosticState()
    {
        const string statusPrefix = "عمليات المزامنة (";
        var status = ReadStatus();
        if (status.StartsWith(statusPrefix, StringComparison.Ordinal) &&
            status.EndsWith(')'))
            return status[statusPrefix.Length..^1];

        var helpText = Element(DesktopAutomationIds.Operations).GetCurrentPropertyValue(
            AutomationElement.HelpTextProperty, ignoreDefaultValue: true);
        return helpText is string value && value.StartsWith("DESKTOP_OPERATIONS_",
                   StringComparison.Ordinal)
            ? value
            : "";
    }

    internal static string ClassifyOperationsWindowTimeout(string diagnosticState) =>
        diagnosticState switch
        {
            "DESKTOP_OPERATIONS_CREATE_STARTED" =>
                "DESKTOP_E2E_OPERATIONS_TIMEOUT_CREATE_STARTED",
            "DESKTOP_OPERATIONS_CREATED" => "DESKTOP_E2E_OPERATIONS_TIMEOUT_CREATED",
            "DESKTOP_OPERATIONS_SHOW_STARTED" =>
                "DESKTOP_E2E_OPERATIONS_TIMEOUT_SHOW_STARTED",
            "DESKTOP_OPERATIONS_WINDOW_SHOWN" =>
                "DESKTOP_E2E_OPERATIONS_TIMEOUT_WINDOW_SHOWN",
            "DESKTOP_OPERATIONS_SHOW_RETURNED" =>
                "DESKTOP_E2E_OPERATIONS_WINDOW_UNDISCOVERABLE_AFTER_SHOW",
            "DESKTOP_OPERATIONS_READY" or "DESKTOP_OPERATIONS_INVOKED" =>
                "DESKTOP_E2E_OPERATIONS_WINDOW_ABSENT",
            _ => "DESKTOP_E2E_OPERATIONS_DIAGNOSTIC_INVALID"
        };

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
        try
        {
            if (_process.HasExited)
                return;
            _process.CloseMainWindow();
            using var normalTimeout = new CancellationTokenSource(CleanupExitTimeout);
            try
            {
                await _process.WaitForExitAsync(normalTimeout.Token);
                return;
            }
            catch (OperationCanceledException)
            {
                // Cleanup may kill a failed test process, but a kill is never acceptance evidence.
            }

            _process.Refresh();
            if (_process.HasExited)
                return;
            try { _process.Kill(entireProcessTree: true); }
            catch
            {
                _process.Refresh();
                if (_process.HasExited)
                    return;
                throw new InvalidOperationException("DESKTOP_E2E_PROCESS_KILL_FAILED");
            }
            using var killTimeout = new CancellationTokenSource(CleanupExitTimeout);
            try { await _process.WaitForExitAsync(killTimeout.Token); }
            catch (OperationCanceledException)
            {
                _process.Refresh();
                if (_process.HasExited)
                    return;
                throw new InvalidOperationException("DESKTOP_E2E_PROCESS_KILL_TIMEOUT");
            }
        }
        finally
        {
            _process.Dispose();
        }
    }
}
