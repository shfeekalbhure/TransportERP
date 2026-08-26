using TransportERP.Desktop.Offline;

namespace TransportERP.Desktop.Application;

internal sealed class DesktopShellForm : Form
{
    private readonly Label _offlineStatus;
    private readonly Button _operations;
    private DesktopOfflineRuntime? _runtime;

    internal DesktopShellForm()
    {
        Text = "TransportERP";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(640, 320);

        _offlineStatus = new Label
        {
            AutoSize = true,
            Text = "العمل دون اتصال مغلق — يلزم تسجيل الدخول والتفويض",
            Dock = DockStyle.Top,
            Padding = new Padding(16)
        };
        _operations = new Button
        {
            Text = "عمليات المزامنة",
            Enabled = false,
            AutoSize = true,
            Margin = new Padding(16)
        };
        _operations.Click += (_, _) => ShowOperations();

        var body = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(16)
        };
        body.Controls.Add(_offlineStatus);
        body.Controls.Add(_operations);
        Controls.Add(body);
    }

    internal void AttachAuthenticatedRuntime(DesktopOfflineRuntime runtime)
    {
        if (_runtime is not null)
            throw new InvalidOperationException("DESKTOP_OFFLINE_RUNTIME_ALREADY_ACTIVE");
        _runtime = runtime;
        _offlineStatus.Text = "العمل دون اتصال جاهز ضمن نطاق الجلسة الحالية";
        _operations.Enabled = true;
    }

    internal void ReportSupervisorStopped()
    {
        if (IsDisposed)
            return;
        if (InvokeRequired)
        {
            BeginInvoke((Action)ReportSupervisorStopped);
            return;
        }
        _offlineStatus.Text = "توقفت خدمة المزامنة — أعد تسجيل الدخول";
        _operations.Enabled = false;
    }

    private void ShowOperations()
    {
        if (_runtime is null)
            return;
        var operations = _runtime.CreateOperationsForm();
        operations.Show(this);
    }
}
