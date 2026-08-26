using TransportERP.Desktop.Offline;

namespace TransportERP.Desktop.Application;

internal sealed class DesktopShellForm : Form, IDesktopOnlineSignInSurface
{
    private readonly Label _offlineStatus;
    private readonly TextBox _publicOrigin;
    private readonly TextBox _userName;
    private readonly TextBox _password;
    private readonly TextBox _companyId;
    private readonly TextBox _branchId;
    private readonly TextBox _deviceId;
    private readonly TextBox _deviceCredential;
    private readonly TextBox _certificateThumbprint;
    private readonly Button _signIn;
    private readonly Button _logout;
    private readonly Button _operations;
    private DesktopOfflineRuntime? _runtime;

    public event EventHandler<DesktopOnlineSignInRequest>? SignInRequested;
    public event EventHandler? LogoutRequested;

    internal DesktopShellForm()
    {
        Text = "TransportERP";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(760, 700);

        _offlineStatus = new Label
        {
            AutoSize = true,
            Text = "العمل دون اتصال مغلق — يلزم تسجيل الدخول والتفويض",
            Dock = DockStyle.Top,
            Padding = new Padding(16)
        };
        _publicOrigin = Input("https://transport.example/");
        _userName = Input();
        _password = Input(password: true);
        _companyId = Input();
        _branchId = Input();
        _deviceId = Input();
        _deviceCredential = Input(password: true);
        _certificateThumbprint = Input();
        _signIn = new Button
        {
            Text = "تسجيل الدخول والتحقق من العمل دون اتصال",
            AutoSize = true,
            Margin = new Padding(8)
        };
        _signIn.Click += (_, _) => RequestSignIn();
        _logout = new Button
        {
            Text = "تسجيل الخروج",
            Enabled = false,
            AutoSize = true,
            Margin = new Padding(8)
        };
        _logout.Click += (_, _) => LogoutRequested?.Invoke(this, EventArgs.Empty);
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
            AutoScroll = true,
            Padding = new Padding(16)
        };
        body.Controls.Add(_offlineStatus);
        body.Controls.Add(Field("عنوان خادم HTTPS العام", _publicOrigin));
        body.Controls.Add(Field("اسم المستخدم أو البريد", _userName));
        body.Controls.Add(Field("كلمة المرور", _password));
        body.Controls.Add(Field("معرّف الشركة", _companyId));
        body.Controls.Add(Field("معرّف الفرع", _branchId));
        body.Controls.Add(Field("معرّف الجهاز", _deviceId));
        body.Controls.Add(Field("اعتماد الجهاز", _deviceCredential));
        body.Controls.Add(Field("بصمة شهادة مفتاح إثبات الجهاز", _certificateThumbprint));
        body.Controls.Add(_signIn);
        body.Controls.Add(_logout);
        body.Controls.Add(_operations);
        Controls.Add(body);
    }

    internal void AttachAuthenticatedRuntime(DesktopOfflineRuntime runtime)
    {
        if (_runtime is not null)
            throw new InvalidOperationException("DESKTOP_OFFLINE_RUNTIME_ALREADY_ACTIVE");
        _runtime = runtime;
        _offlineStatus.Text = "العمل دون اتصال جاهز ضمن نطاق الجلسة الحالية";
        _signIn.Enabled = false;
        _logout.Enabled = true;
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
        _logout.Enabled = true;
        _operations.Enabled = false;
    }

    internal void CloseForSessionEnd(string reasonCode)
    {
        if (IsDisposed)
            return;
        _runtime = null;
        ClearSecrets();
        _signIn.Enabled = false;
        _logout.Enabled = false;
        _operations.Enabled = false;
        _offlineStatus.Text = $"انتهت الجلسة وأُغلق العمل دون اتصال ({reasonCode})";
        Close();
    }

    private void ShowOperations()
    {
        if (_runtime is null)
            return;
        var operations = _runtime.CreateOperationsForm();
        operations.Show(this);
    }

    public void ReportSignInFailed(string reasonCode)
    {
        if (IsDisposed)
            return;
        ClearSecrets();
        _offlineStatus.Text = $"تعذر تسجيل الدخول أو تفويض العمل دون اتصال ({reasonCode})";
        _signIn.Enabled = true;
    }

    public void ReportSignInSucceeded()
    {
        if (IsDisposed)
            return;
        ClearSecrets();
        _offlineStatus.Text = "تم التحقق من الجلسة والجهاز وسياسة العمل دون اتصال";
        _signIn.Enabled = false;
    }

    private void RequestSignIn()
    {
        if (!Guid.TryParse(_companyId.Text, out var companyId) || companyId == Guid.Empty ||
            !Guid.TryParse(_branchId.Text, out var branchId) || branchId == Guid.Empty)
        {
            ReportSignInFailed("SIGN_IN_SCOPE_INVALID");
            return;
        }

        _signIn.Enabled = false;
        _offlineStatus.Text = "جارٍ التحقق من الجلسة والجهاز والسياسة...";
        SignInRequested?.Invoke(this, new DesktopOnlineSignInRequest(
            _publicOrigin.Text,
            _userName.Text,
            _password.Text,
            companyId,
            branchId,
            _deviceId.Text,
            _deviceCredential.Text,
            _certificateThumbprint.Text));
        // UI controls must not retain password or device credential after the request is emitted.
        ClearSecrets();
    }

    private void ClearSecrets()
    {
        _password.Clear();
        _deviceCredential.Clear();
    }

    private static TextBox Input(string? value = null, bool password = false) => new()
    {
        Width = 520,
        Text = value ?? string.Empty,
        UseSystemPasswordChar = password
    };

    private static Control Field(string label, TextBox input)
    {
        var panel = new FlowLayoutPanel
        {
            AutoSize = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Margin = new Padding(8)
        };
        panel.Controls.Add(new Label { AutoSize = true, Text = label });
        panel.Controls.Add(input);
        return panel;
    }
}
