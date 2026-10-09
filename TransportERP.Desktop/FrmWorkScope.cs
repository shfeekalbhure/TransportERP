namespace TransportERP.Desktop;

using CoreUI;
using Authentication;
using TransportERP.Contracts.Authentication;

public sealed partial class FrmWorkScope : FrmBase
{
    private readonly DesktopSession? session;
    private bool designPreview;
    private readonly LoginResponse? login;
    private sealed record Choice(Guid Id, string Caption) { public override string ToString() => Caption; }
    public FrmWorkScope() { InitializeComponent(); 
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }
#if DEBUG
    internal static FrmWorkScope CreatePreview()
    {
        var form = new FrmWorkScope { designPreview = true };
        form.Text = "معاينة التصميم | اختيار نطاق العمل";
        form.lblDemo.Text = "معاينة فقط — بيانات توضيحية، بدون اتصال أو جلسة دخول حقيقية.";
        form.cmbCompany.Items.Add("شركة توضيحية — معاينة");
        form.cmbBranch.Items.Add("فرع توضيحي — معاينة");
        form.cmbYear.Items.Add("2026 — فترة توضيحية");
        form.cmbCompany.SelectedIndex = form.cmbBranch.SelectedIndex = form.cmbYear.SelectedIndex = 0;
        form.btnOpen.Text = "معاينة الشاشة الرئيسية";
        return form;
    }
#endif
    public FrmWorkScope(DesktopSession session, LoginResponse login) : this()
    {
        this.session = session;
        this.login = login;
        lblDemo.Text = $"مرحباً {login.DisplayName} — اختر من نطاقات العمل المسموحة لك.";
        cmbCompany.SelectedIndexChanged += CompanyChanged;
        cmbBranch.SelectedIndexChanged += BranchChanged;
        cmbCompany.Items.AddRange(login.Scopes.DistinctBy(s => s.CompanyId)
            .Select(s => (object)new Choice(s.CompanyId, s.CompanyName)).ToArray());
        if (cmbCompany.Items.Count > 0) cmbCompany.SelectedIndex = 0;
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }
    private void CompanyChanged(object? sender, EventArgs e)
    {
        cmbBranch.Items.Clear(); cmbYear.Items.Clear();
        if (login is null || cmbCompany.SelectedItem is not Choice company) return;
        cmbBranch.Items.AddRange(login.Scopes.Where(s => s.CompanyId == company.Id).DistinctBy(s => s.BranchId)
            .Select(s => (object)new Choice(s.BranchId, s.BranchName)).ToArray());
        if (cmbBranch.Items.Count > 0) cmbBranch.SelectedIndex = 0;
    }
    private void BranchChanged(object? sender, EventArgs e)
    {
        cmbYear.Items.Clear();
        if (login is null || cmbCompany.SelectedItem is not Choice company || cmbBranch.SelectedItem is not Choice branch) return;
        cmbYear.Items.AddRange(login.Scopes.Where(s => s.CompanyId == company.Id && s.BranchId == branch.Id)
            .Select(s => (object)new Choice(s.FiscalPeriodId, s.FiscalPeriodName)).ToArray());
        if (cmbYear.Items.Count > 0) cmbYear.SelectedIndex = 0;
    }
    private async void Open_Click(object? sender, EventArgs e)
    {
#if DEBUG
        if (designPreview)
        {
            DialogResult = DialogResult.OK;
            Close();
            return;
        }
#endif
        if (!btnOpen.Enabled || session is null || login is null) return;
        if (cmbCompany.SelectedItem is not Choice company || cmbBranch.SelectedItem is not Choice branch ||
            cmbYear.SelectedItem is not Choice period) return;
        btnOpen.Enabled = btnBack.Enabled = false; ControlBox = false;
        try
        {
            await session.SelectAsync(new(login.LoginTicket, company.Id, branch.Id, period.Id));
            DialogResult = DialogResult.OK; Close();
        }
        catch (Exception ex) when (ex is System.Net.Http.HttpRequestException or TaskCanceledException or InvalidOperationException or System.Text.Json.JsonException)
        {
            lblDemo.Text = ex is InvalidOperationException ? ex.Message : "تعذر الاتصال بالخادم. حاول مجددًا أو ارجع لتسجيل الدخول.";
        }
        finally
        {
            if (!IsDisposed) { btnOpen.Enabled = btnBack.Enabled = true; ControlBox = true; }
        }
    }
    private void Back_Click(object? sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }
}
