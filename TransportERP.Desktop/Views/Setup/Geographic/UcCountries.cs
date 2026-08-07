namespace TransportERP.Desktop.Views.Setup.Geographic;

/// <summary>
/// GEN-003 — الدول.
/// هذه الشاشة تحتوي فقط على حقول الدول الخاصة بها،
/// أما الأدوات العامة مثل الأزرار والبحث والجدول والتصفح والتدقيق فتأتي من القالب المشترك.
/// </summary>
public partial class UcCountries : UserControl
{
    /// <summary>
    /// إنشاء الشاشة ثم تجهيز الأحداث والقيم الافتراضية.
    /// </summary>
    public UcCountries()
    {
        InitializeComponent();
        ConfigureSharedControls();
        ConfigureRuntimeDefaults();
    }

    /// <summary>
    /// ربط الشاشة بالمكونات العامة الموجودة داخل TransportReferenceScreenShell.
    /// بهذه الطريقة لا نكرر أزرار جديد وحفظ وبحث وتصفح في كل شاشة.
    /// </summary>
    private void ConfigureSharedControls()
    {
        // ربط زر "جديد" الموحد بعملية تفريغ حقول شاشة الدول.
        screenShell.Toolbar.NewRequested += (_, _) => ClearEditor();

        // زر الإغلاق موحد، وهذه الشاشة تحدد فقط كيف تغلق تبويبها الحالي.
        screenShell.Toolbar.CloseRequested += (_, _) => CloseHostTab();

        // البحث والتصفية موحدان، والشاشة تستقبل الحدث فقط لإرسال القيمة لاحقًا إلى API.
        screenShell.SearchPanel.SearchTextChanged += (_, _) => HandleSearchChanged();
        screenShell.SearchPanel.StatusChanged += (_, _) => HandleSearchChanged();
    }

    /// <summary>
    /// القيم الافتراضية الخاصة بشاشة الدول فقط.
    /// </summary>
    private void ConfigureRuntimeDefaults()
    {
        RightToLeft = RightToLeft.Yes;
        cmbStatus.SelectedIndex = 0;
        screenShell.SearchPanel.SetStatusItems("نشط", "موقوف");
        screenShell.SearchPanel.SearchPlaceholder = "ابحث بكود الدولة أو الاسم...";
        screenShell.Pagination.SetPageInfo(1, 1, 0, 0, 0);
        screenShell.AuditPanel.ClearAuditInfo();
    }

    /// <summary>
    /// تفريغ حقول السجل عند الضغط على زر جديد.
    /// الحقول الخاصة بالدول فقط موجودة هنا، بينما الأزرار نفسها موجودة في القالب المشترك.
    /// </summary>
    private void ClearEditor()
    {
        txtCountryCode.Clear();
        txtNameAr.Clear();
        txtNameEn.Clear();
        txtIso2.Clear();
        txtIso3.Clear();
        txtDialCode.Clear();
        txtCurrencyCode.Clear();
        txtNotes.Clear();
        cmbStatus.SelectedIndex = 0;
        screenShell.AlertBar.HideMessage();
        screenShell.AuditPanel.ClearAuditInfo();
        txtCountryCode.Focus();
    }

    /// <summary>
    /// نقطة واحدة تستقبل تغير البحث أو الحالة.
    /// الربط الحقيقي بالـAPI سيضاف هنا لاحقًا بدون تعديل المكون المشترك.
    /// </summary>
    private void HandleSearchChanged()
    {
        // لاحقًا نرسل screenShell.SearchPanel.SearchText وSelectedStatus إلى API.
    }

    /// <summary>
    /// إغلاق تبويب الشاشة الحالي داخل الـMain Shell.
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
