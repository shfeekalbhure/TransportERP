using TransportERP.Desktop.Offline;

namespace TransportERP.Desktop.Application;

internal sealed class DesktopShellForm : Form, IDesktopOnlineSignInSurface
{
    private readonly Label _offlineStatus;
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
    private readonly TextBox _partyName;
    private readonly TextBox _partyMobile;
    private readonly TextBox _partyAddress;
    private readonly Button _queueParty;
    private DesktopOfflineRuntime? _runtime;
    private DesktopOfflineBusinessProducer? _businessProducer;
    private bool _runtimeAuthorized;

    public event EventHandler<DesktopOnlineSignInRequest>? SignInRequested;
    public event EventHandler? LogoutRequested;

    internal DesktopShellForm()
    {
        Name = DesktopAutomationIds.Shell;
        Text = "TransportERP";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(760, 700);

        _offlineStatus = new Label
        {
            Name = DesktopAutomationIds.OfflineStatus,
            AutoSize = true,
            Text = "العمل دون اتصال مغلق — يلزم تسجيل الدخول والتفويض",
            Dock = DockStyle.Top,
            Padding = new Padding(16)
        };
        _userName = Input(DesktopAutomationIds.UserName);
        _password = Input(DesktopAutomationIds.Password, password: true);
        _companyId = Input(DesktopAutomationIds.CompanyId);
        _branchId = Input(DesktopAutomationIds.BranchId);
        _deviceId = Input(DesktopAutomationIds.DeviceId);
        _deviceCredential = Input(DesktopAutomationIds.DeviceCredential, password: true);
        _certificateThumbprint = Input(DesktopAutomationIds.CertificateThumbprint);
        _signIn = new Button
        {
            Name = DesktopAutomationIds.SignIn,
            Text = "تسجيل الدخول والتحقق من العمل دون اتصال",
            AutoSize = true,
            Margin = new Padding(8)
        };
        _signIn.Click += (_, _) => RequestSignIn();
        _logout = new Button
        {
            Name = DesktopAutomationIds.Logout,
            Text = "تسجيل الخروج",
            Enabled = false,
            AutoSize = true,
            Margin = new Padding(8)
        };
        _logout.Click += (_, _) => LogoutRequested?.Invoke(this, EventArgs.Empty);
        _operations = new Button
        {
            Name = DesktopAutomationIds.Operations,
            Text = "عمليات المزامنة",
            Enabled = false,
            AccessibleDescription = "DESKTOP_OPERATIONS_CLOSED",
            AutoSize = true,
            Margin = new Padding(16)
        };
        _operations.Click += (_, _) => ShowOperations();
        _partyName = Input(DesktopAutomationIds.PartyName);
        _partyMobile = Input(DesktopAutomationIds.PartyMobile);
        _partyAddress = Input(DesktopAutomationIds.PartyAddress);
        _queueParty = new Button
        {
            Name = DesktopAutomationIds.QueueParty,
            Text = "حفظ طرف تشغيلي في قائمة المزامنة المشفرة",
            Enabled = false,
            AutoSize = true,
            Margin = new Padding(16)
        };
        _queueParty.Click += async (_, _) => await QueueOperationalPartyAsync();

        var body = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            Padding = new Padding(16)
        };
        body.Controls.Add(_offlineStatus);
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
        body.Controls.Add(new Label
        {
            AutoSize = true,
            Text = "إضافة طرف تشغيلي — إجراء فعلي يعمل دون اتصال",
            Padding = new Padding(8)
        });
        body.Controls.Add(Field("اسم الطرف", _partyName));
        body.Controls.Add(Field("رقم الجوال", _partyMobile));
        body.Controls.Add(Field("العنوان", _partyAddress));
        body.Controls.Add(_queueParty);
        Controls.Add(body);
    }

    internal void AttachAuthenticatedRuntime(DesktopOfflineRuntime runtime)
    {
        if (_runtime is not null)
            throw new InvalidOperationException("DESKTOP_OFFLINE_RUNTIME_ALREADY_ACTIVE");
        _runtime = runtime;
        _businessProducer = runtime.CanQueueOperationalParties ? runtime.CreateBusinessProducer() : null;
        _runtimeAuthorized = true;
        _offlineStatus.Text = "العمل دون اتصال جاهز ضمن نطاق الجلسة الحالية";
        _signIn.Enabled = false;
        _logout.Enabled = true;
        _operations.Enabled = true;
        _operations.AccessibleDescription = "DESKTOP_OPERATIONS_READY";
        _queueParty.Enabled = _businessProducer is not null;
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
        // Supervisor termination revokes this UI's write capability immediately. The application
        // context still owns and disposes the runtime during authenticated session teardown.
        _runtimeAuthorized = false;
        _runtime = null;
        _businessProducer = null;
        _operations.AccessibleDescription = "DESKTOP_OPERATIONS_SUPERVISOR_STOPPED";
        _logout.Enabled = true;
        _operations.Enabled = false;
        _queueParty.Enabled = false;
    }

    internal void CloseForSessionEnd(string reasonCode)
    {
        if (IsDisposed)
            return;
        _runtime = null;
        _businessProducer = null;
        _runtimeAuthorized = false;
        _operations.AccessibleDescription = "DESKTOP_OPERATIONS_SESSION_ENDED";
        ClearSecrets();
        _signIn.Enabled = false;
        _logout.Enabled = false;
        _operations.Enabled = false;
        _queueParty.Enabled = false;
        _offlineStatus.Text = $"انتهت الجلسة وأُغلق العمل دون اتصال ({reasonCode})";
        Close();
    }

    private void ShowOperations()
    {
        _operations.AccessibleDescription = "DESKTOP_OPERATIONS_INVOKED";
        var runtime = _runtime;
        if (runtime is null)
        {
            _operations.AccessibleDescription = "DESKTOP_OPERATIONS_RUNTIME_MISSING";
            return;
        }

        _operations.AccessibleDescription = "DESKTOP_OPERATIONS_CREATE_STARTED";
        SyncOperationsForm operations;
        try
        {
            operations = runtime.CreateOperationsForm();
        }
        catch
        {
            _operations.AccessibleDescription = "DESKTOP_OPERATIONS_CREATE_FAILED";
            return;
        }

        _operations.AccessibleDescription = "DESKTOP_OPERATIONS_CREATED";
        operations.Shown += (_, _) =>
            _operations.AccessibleDescription = "DESKTOP_OPERATIONS_WINDOW_SHOWN";
        _operations.AccessibleDescription = "DESKTOP_OPERATIONS_SHOW_STARTED";
        try
        {
            operations.Show(this);
            if (!operations.IsHandleCreated || !operations.Visible)
            {
                DisposeOperationsForm(operations);
                _operations.AccessibleDescription = "DESKTOP_OPERATIONS_WINDOW_NOT_VISIBLE";
                return;
            }
            _operations.AccessibleDescription = "DESKTOP_OPERATIONS_SHOW_RETURNED";
        }
        catch
        {
            DisposeOperationsForm(operations);
            _operations.AccessibleDescription = "DESKTOP_OPERATIONS_SHOW_FAILED";
        }
    }

    private static void DisposeOperationsForm(SyncOperationsForm operations)
    {
        try { operations.Dispose(); }
        catch { }
    }

    private async Task QueueOperationalPartyAsync()
    {
        var producer = _businessProducer;
        if (producer is null) return;
        _queueParty.Enabled = false;
        try
        {
            var result = await producer.QueueOperationalPartyAsync(
                _partyName.Text, _partyMobile.Text, _partyAddress.Text);
            _partyName.Clear();
            _partyMobile.Clear();
            _partyAddress.Clear();
            _offlineStatus.Text = result.Created
                ? $"تمت إضافة العملية المشفرة ({result.Operation.LocalOperationId:D})"
                : $"العملية موجودة مسبقًا ({result.Operation.LocalOperationId:D})";
        }
        catch (Exception exception)
        {
            _offlineStatus.Text = $"تعذر إضافة العملية ({SafeCode(exception)})";
        }
        finally
        {
            _queueParty.Enabled = _runtimeAuthorized && _businessProducer is not null;
        }
    }

    private static string SafeCode(Exception exception) => exception switch
    {
        TransportERP.Offline.OfflineStoreException store when IsSafeCode(store.Code) => store.Code,
        _ => "BUSINESS_OPERATION_FAILED"
    };

    private static bool IsSafeCode(string value) => value.Length is > 0 and <= 64 &&
        value.All(character => character is >= 'A' and <= 'Z' or >= '0' and <= '9' or '_');

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

    private static TextBox Input(string automationId, string? value = null, bool password = false) => new()
    {
        Name = automationId,
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

internal static class DesktopAutomationIds
{
    internal const string Shell = "TransportERP.Desktop.Shell";
    internal const string OfflineStatus = "TransportERP.Desktop.OfflineStatus";
    internal const string UserName = "TransportERP.Desktop.UserName";
    internal const string Password = "TransportERP.Desktop.Password";
    internal const string CompanyId = "TransportERP.Desktop.CompanyId";
    internal const string BranchId = "TransportERP.Desktop.BranchId";
    internal const string DeviceId = "TransportERP.Desktop.DeviceId";
    internal const string DeviceCredential = "TransportERP.Desktop.DeviceCredential";
    internal const string CertificateThumbprint = "TransportERP.Desktop.CertificateThumbprint";
    internal const string SignIn = "TransportERP.Desktop.SignIn";
    internal const string Logout = "TransportERP.Desktop.Logout";
    internal const string Operations = "TransportERP.Desktop.Operations";
    internal const string PartyName = "TransportERP.Desktop.PartyName";
    internal const string PartyMobile = "TransportERP.Desktop.PartyMobile";
    internal const string PartyAddress = "TransportERP.Desktop.PartyAddress";
    internal const string QueueParty = "TransportERP.Desktop.QueueParty";
}
