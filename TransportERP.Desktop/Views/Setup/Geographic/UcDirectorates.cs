namespace TransportERP.Desktop.Views.Setup.Geographic;

/// <summary>
/// GEN-005 — المديريات.
/// الشاشة تحتفظ فقط بحقول المديرية، بينما الأدوات العامة تأتي من القالب المشترك.
/// </summary>
public partial class UcDirectorates : UserControl
{
    public UcDirectorates()
    {
        InitializeComponent();
        ConfigureSharedControls();
        ConfigureRuntimeDefaults();
    }

    /// <summary>
    /// ربط أحداث الأدوات المشتركة بوظائف شاشة المديريات.
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
        screenShell.SearchPanel.SearchPlaceholder = "ابحث بكود المديرية أو الاسم...";
        screenShell.Pagination.SetPageInfo(1, 1, 0, 0, 0);
        screenShell.AuditPanel.ClearAuditInfo();
    }

    /// <summary>
    /// تفريغ حقول المديرية عند الضغط على زر جديد الموجود في الشريط المشترك.
    /// </summary>
    private void ClearEditor()
    {
        cmbCountry.SelectedIndex = -1;
        cmbGovernorate.SelectedIndex = -1;
        txtDirectorateCode.Clear();
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
    /// إغلاق تبويب الشاشة الحالي.
    /// </summary>
    private void CloseHostTab()
    {
        if (Parent is not TabPage page || page.Parent is not TabControl tabs) return;
        tabs.TabPages.Remove(page);
        Dispose();
        page.Dispose();
    }
}
