namespace TransportERP.Desktop;

public partial class FrmMain
{
#if DEBUG
    internal void ApplyDesignPreview()
    {
        AuthenticatedSession = null;
        Text = "معاينة الشاشات فقط — بدون خادم أو قاعدة بيانات";
        lblCompanyName.Text = "الشركة: توضيحية — معاينة";
        lblBranch.Text = "الفرع: توضيحي — معاينة";
        lblFinancialYear.Text = "الفترة: 2026 — توضيحية";
        lblCurrentUser.Text = "المستخدم: معاينة فقط";
        lblCompanyValue.Text = "شركة توضيحية";
        lblBranchValue.Text = "فرع توضيحي";
        lblYearValue.Text = "2026 — توضيحية";
        lblUserValue.Text = "معاينة — غير مصادق";
    }
#endif
    internal Authentication.DesktopSession? AuthenticatedSession { get; private set; }
    internal void ApplyAuthenticatedSession(Authentication.DesktopSession session)
    {
        var current = session.Current ?? throw new InvalidOperationException("جلسة الدخول مطلوبة.");
        AuthenticatedSession = session;
        lblCompanyName.Text = "الشركة: " + current.Scope.CompanyName;
        lblBranch.Text = "الفرع: " + current.Scope.BranchName;
        lblFinancialYear.Text = "الفترة: " + current.Scope.FiscalPeriodName;
        lblCurrentUser.Text = "المستخدم: " + current.DisplayName;
        lblCompanyValue.Text = current.Scope.CompanyName;
        lblBranchValue.Text = current.Scope.BranchName;
        lblYearValue.Text = current.Scope.FiscalPeriodName;
        lblUserValue.Text = current.DisplayName;
        var expiryTimer = new System.Windows.Forms.Timer { Interval = 1000 };
        expiryTimer.Tick += (_, _) =>
        {
            if (DateTimeOffset.UtcNow < current.ExpiresAt) return;
            expiryTimer.Stop();
            MessageBox.Show(this, "انتهت جلسة العمل. سجل الدخول من جديد.", "انتهاء الجلسة");
            Close();
        };
        FormClosed += (_, _) => { expiryTimer.Dispose(); AuthenticatedSession = null; };
        expiryTimer.Start();
    }
}
