namespace TransportERP.Desktop;

/// <summary>
/// الشاشة الرئيسية لنظام TransportERP.
/// تعرض مؤشرات الأداء والاختصارات وآخر العمليات والتنبيهات ضمن واجهة عربية RTL.
/// </summary>
public partial class FrmDashboard : Form
{
    /// <summary>
    /// إنشاء الشاشة الرئيسية وتهيئة مكوناتها المصممة بصريًا.
    /// </summary>
    public FrmDashboard()
    {
        InitializeComponent();
        LoadDevelopmentPreviewData();
    }

    /// <summary>
    /// تحميل بيانات معاينة مؤقتة أثناء مرحلة تطوير الواجهات فقط.
    /// تُستبدل لاحقًا ببيانات الخدمات وواجهة API.
    /// </summary>
    private void LoadDevelopmentPreviewData()
    {
        dgvRecentOperations.Rows.Clear();
        dgvRecentOperations.Rows.Add("سند قبض", "RV-000125", "شركة النقل الرئيسية", "125,000", "معلق");
        dgvRecentOperations.Rows.Add("قيد يومي", "JV-000084", "الفرع الرئيسي", "75,500", "معتمد");
        dgvRecentOperations.Rows.Add("سند صرف", "PV-000041", "الصندوق الرئيسي", "32,000", "مراجع");
        dgvRecentOperations.Rows.Add("تحديث مستخدم", "USR-000012", "إدارة النظام", "—", "مكتمل");

        statusBar.CompanyName = "شركة النقل الرئيسية";
        statusBar.BranchName = "الفرع الرئيسي";
        statusBar.FiscalYear = DateTime.Today.Year.ToString();
        statusBar.FinancialPeriod = "الفترة الحالية";
        statusBar.CurrentUser = "مستخدم تجريبي";
        statusBar.CurrentRole = "مدير النظام";
        statusBar.EnvironmentName = "بيئة التطوير";
        statusBar.SystemVersion = "1.0.0";
        statusBar.SetConnectionStatus(false, "لم يتم ربط API بعد");
    }

    /// <summary>
    /// إغلاق الشاشة الرئيسية والعودة إلى شاشة الدخول في وضع التطوير.
    /// </summary>
    private void btnLogout_Click(object? sender, EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// عرض رسالة مؤقتة عند الضغط على اختصار لم تُربط شاشته بعد.
    /// </summary>
    private void QuickAction_Click(object? sender, EventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        MessageBox.Show(
            $"سيتم فتح شاشة: {button.Text} بعد تنفيذها وربطها.",
            "TransportERP",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
}
