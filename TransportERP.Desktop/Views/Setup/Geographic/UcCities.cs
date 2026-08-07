namespace TransportERP.Desktop.Views.Setup.Geographic;

/// <summary>
/// GEN-006 — المدن.
/// الشاشة تحتفظ فقط بحقول المدينة، بينما الأدوات العامة تأتي من القالب المشترك.
/// </summary>
public partial class UcCities : UserControl
{
    public UcCities()
    {
        InitializeComponent();
        ConfigureSharedControls();
        ConfigureRuntimeDefaults();
    }

    /// <summary>
    /// ربط أحداث الأدوات المشتركة بوظائف شاشة المدن.
    /// </summary>
    private void ConfigureSharedControls()
    {
        screenShell.Toolbar.NewRequested += (_, _) => ClearEditor();
        screenShell.Toolbar.CloseRequested += (_, _) => CloseHostTab();
        screenShell.SearchPanel.SearchTextChanged += (_, _) => HandleSearchChanged();
        screenShell.SearchPanel.StatusChanged += (_, _) => HandleSearchChanged();
    }

    /// <summary>
    /// ضبط القيم الافتراضية للشاشة.
    /// </summary>
    private void ConfigureRuntimeDefaults()
    {
        RightToLeft = RightToLeft.Yes;
        cmbStatus.SelectedIndex = 0;
        screenShell.SearchPanel.SetStatusItems("نشط", "موقوف");
        screenShell.SearchPanel.SearchPlaceholder = "ابحث بكود المدينة أو الاسم...";
        screenShell.Pagination.SetPageInfo(1, 1, 0, 0, 0);
        screenShell.AuditPanel.ClearAuditInfo();
    }

    /// <summary>
    /// تفريغ حقول المدينة عند الضغط على زر جديد الموحد.
    /// </summary>
    private void ClearEditor()
    {
        cmbCountry.SelectedIndex = -1;
        cmbGovernorate.SelectedIndex = -1;
        cmbDirectorate.SelectedIndex = -1;
        txtCityCode.Clear();
        txtNameAr.Clear();
        txtNameEn.Clear();
        txtNotes.Clear();
        cmbStatus.SelectedIndex = 0;
        screenShell.AlertBar.HideMessage();
        screenShell.AuditPanel.ClearAuditInfo();
        cmbCountry.Focus();
    }

    /// <summary>
    /// نقطة الربط المستقبلية مع API للبحث والتصفية.
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
