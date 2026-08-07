namespace TransportERP.Desktop.Views.Setup.Geographic;

/// <summary>
/// GEN-004 — المحافظات.
/// الحقول هنا خاصة بالمحافظة فقط، أما الأدوات العامة فتأتي من TransportReferenceScreenShell.
/// </summary>
public partial class UcGovernorates : UserControl
{
    public UcGovernorates()
    {
        InitializeComponent();
        ConfigureSharedControls();
        ConfigureRuntimeDefaults();
    }

    /// <summary>
    /// ربط أحداث الأدوات المشتركة بوظائف شاشة المحافظات.
    /// </summary>
    private void ConfigureSharedControls()
    {
        screenShell.Toolbar.NewRequested += (_, _) => ClearEditor();
        screenShell.Toolbar.CloseRequested += (_, _) => CloseHostTab();
        screenShell.SearchPanel.SearchTextChanged += (_, _) => HandleSearchChanged();
        screenShell.SearchPanel.StatusChanged += (_, _) => HandleSearchChanged();
    }

    /// <summary>
    /// ضبط القيم الافتراضية للشاشة دون تكرار إعدادات البحث والتصفح والتدقيق.
    /// </summary>
    private void ConfigureRuntimeDefaults()
    {
        RightToLeft = RightToLeft.Yes;
        cmbStatus.SelectedIndex = 0;
        screenShell.SearchPanel.SetStatusItems("نشط", "موقوف");
        screenShell.SearchPanel.SearchPlaceholder = "ابحث بكود المحافظة أو الاسم...";
        screenShell.Pagination.SetPageInfo(1, 1, 0, 0, 0);
        screenShell.AuditPanel.ClearAuditInfo();
    }

    /// <summary>
    /// تفريغ الحقول الخاصة بالمحافظة عند إنشاء سجل جديد.
    /// </summary>
    private void ClearEditor()
    {
        cmbCountry.SelectedIndex = -1;
        txtGovernorateCode.Clear();
        txtNameAr.Clear();
        txtNameEn.Clear();
        txtNotes.Clear();
        cmbStatus.SelectedIndex = 0;
        screenShell.AlertBar.HideMessage();
        screenShell.AuditPanel.ClearAuditInfo();
        cmbCountry.Focus();
    }

    /// <summary>
    /// نقطة الربط المستقبلية مع API عند تغير البحث أو التصفية.
    /// </summary>
    private void HandleSearchChanged()
    {
        // لاحقًا نرسل SearchText وSelectedStatus إلى API.
    }

    /// <summary>
    /// إغلاق تبويب الشاشة الحالي داخل الـMain Shell.
    /// </summary>
    private void CloseHostTab()
    {
        if (Parent is not TabPage page || page.Parent is not TabControl tabs) return;
        tabs.TabPages.Remove(page);
        Dispose();
        page.Dispose();
    }
}
