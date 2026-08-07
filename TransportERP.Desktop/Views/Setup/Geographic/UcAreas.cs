namespace TransportERP.Desktop.Views.Setup.Geographic;

/// <summary>
/// GEN-007 — المناطق.
/// تحتوي الشاشة فقط على الحقول الخاصة بالمنطقة، بينما الأدوات المشتركة تأتي من القالب الموحد.
/// </summary>
public partial class UcAreas : UserControl
{
    public UcAreas()
    {
        InitializeComponent();
        ConfigureSharedControls();
        ConfigureRuntimeDefaults();
    }

    /// <summary>
    /// يربط أزرار القالب المشترك وسلوك البحث بالشاشة بدون تكرار الأدوات نفسها.
    /// </summary>
    private void ConfigureSharedControls()
    {
        screenShell.Toolbar.NewRequested += (_, _) => ClearEditor();
        screenShell.Toolbar.CloseRequested += (_, _) => CloseHostTab();
        screenShell.SearchPanel.SearchTextChanged += (_, _) => HandleSearchChanged();
        screenShell.SearchPanel.StatusChanged += (_, _) => HandleSearchChanged();
    }

    /// <summary>
    /// يجهز القيم الافتراضية واتجاه الشاشة من اليمين إلى اليسار.
    /// </summary>
    private void ConfigureRuntimeDefaults()
    {
        RightToLeft = RightToLeft.Yes;
        cmbStatus.SelectedIndex = 0;
        screenShell.SearchPanel.SetStatusItems("نشط", "موقوف");
        screenShell.SearchPanel.SearchPlaceholder = "ابحث بكود المنطقة أو الاسم...";
        screenShell.Pagination.SetPageInfo(1, 1, 0, 0, 0);
        screenShell.AuditPanel.ClearAuditInfo();
    }

    /// <summary>
    /// يفرغ فقط حقول المنطقة عند إنشاء سجل جديد.
    /// </summary>
    private void ClearEditor()
    {
        cmbCountry.SelectedIndex = -1;
        cmbGovernorate.SelectedIndex = -1;
        cmbDirectorate.SelectedIndex = -1;
        cmbCity.SelectedIndex = -1;
        txtAreaCode.Clear();
        txtNameAr.Clear();
        txtNameEn.Clear();
        txtNotes.Clear();
        cmbStatus.SelectedIndex = 0;
        screenShell.AlertBar.HideMessage();
        screenShell.AuditPanel.ClearAuditInfo();
        cmbCountry.Focus();
    }

    /// <summary>
    /// نقطة ربط البحث والتصفية مع الـ API لاحقًا.
    /// </summary>
    private void HandleSearchChanged()
    {
        // لاحقًا تُرسل قيم SearchText وSelectedStatus إلى API.
    }

    /// <summary>
    /// يغلق تبويب المناطق من مساحة العمل الرئيسية.
    /// </summary>
    private void CloseHostTab()
    {
        if (Parent is not TabPage page || page.Parent is not TabControl tabs)
        {
            return;
        }

        tabs.TabPages.Remove(page);
        Dispose();
        page.Dispose();
    }
}
