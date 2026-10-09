namespace TransportERP.Desktop;

using CoreUI;
using Authentication;

public sealed partial class FrmLogin : FrmBase
{
    public FrmLogin()
    {
        InitializeComponent();
#if DEBUG
        btnPreview.Visible = true;
#endif
        txtApiAddress.Text = Environment.GetEnvironmentVariable("TRANSPORTERP_API_URL") ?? "https://localhost:7011/";
        // Login_Click requires credentials; DesktopSession requires a valid HTTPS address.
        global::TransportERP.RequiredFieldAppearance.Apply(txtUsername, true);
        global::TransportERP.RequiredFieldAppearance.Apply(txtPassword, true);
        global::TransportERP.RequiredFieldAppearance.Apply(txtApiAddress, true);
        global::TransportERP.RequiredFieldAppearance.Apply(cmbLanguage, false);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }
    private void Preview_Click(object? sender, EventArgs e)
    {
#if DEBUG
        using var scope = FrmWorkScope.CreatePreview();
        Hide();
        try
        {
            if (scope.ShowDialog() != DialogResult.OK) return;
            using var main = new FrmMain();
            main.ApplyDesignPreview();
            main.ShowDialog();
        }
        finally { Show(); Activate(); }
#endif
    }
    private void ShowPassword_CheckedChanged(object? sender, EventArgs e)
        => txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
    private void Exit_Click(object? sender, EventArgs e) => Close();
    private void Connection_Click(object? sender, EventArgs e) => txtApiAddress.Focus();
    private async void Login_Click(object? sender, EventArgs e)
    {
        if (!btnLogin.Enabled) return;
        if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
        {
            lblStatus.Text = "أدخل اسم المستخدم وكلمة المرور.";
            (string.IsNullOrWhiteSpace(txtUsername.Text) ? txtUsername : txtPassword).Focus();
            return;
        }
        btnLogin.Enabled = btnExit.Enabled = btnConnection.Enabled = false;
        txtApiAddress.Enabled = false;
        ControlBox = false;
        lblStatus.Text = "جارٍ التحقق من بيانات الدخول...";
        try
        {
            using var session = new DesktopSession(txtApiAddress.Text);
            var login = await session.LoginAsync(txtUsername.Text.Trim(), txtPassword.Text);
            txtPassword.Clear();
            lblConnection.Text = "الاتصال بالخادم ناجح";
            using var scope = new FrmWorkScope(session, login);
            Hide();
            try
            {
                if (scope.ShowDialog() != DialogResult.OK || session.Current is null) return;
                using var main = new FrmMain();
                main.ApplyAuthenticatedSession(session);
                main.ShowDialog();
            }
            finally { Show(); Activate(); }
        }
        catch (Exception ex) when (ex is System.Net.Http.HttpRequestException or TaskCanceledException)
        {
            lblConnection.Text = "تعذر الاتصال بالخادم";
            lblStatus.Text = "تحقق من عنوان الخادم وتشغيل الخدمة وشهادة HTTPS.";
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or System.Text.Json.JsonException)
        { lblStatus.Text = ex is System.Text.Json.JsonException ? "استجابة الخادم غير صالحة." : ex.Message; }
        finally
        {
            txtPassword.Clear(); chkShowPassword.Checked = false;
            btnLogin.Enabled = btnExit.Enabled = btnConnection.Enabled = true;
            txtApiAddress.Enabled = true; ControlBox = true;
            if (lblStatus.Text == "جارٍ التحقق من بيانات الدخول...") lblStatus.Text = "أدخل بياناتك لتسجيل الدخول.";
        }
    }
}
