namespace TransportERP.Desktop.Forms.SystemSettings.General;

// Sole source: https://www.scribd.com/document/973153595/ONYX-ERP-v8-تهيئة-النظام
// Printed pp.86–96. Source headings define the nine sections; journal examples are not UI fields.
public sealed partial class UcIntermediateAccounts : UserControl, TransportERP.Desktop.CoreUI.IFoundationScreen, TransportERP.Desktop.CoreUI.IExplicitScreenLayout
{
    private static readonly (string Title, string[] Labels)[] Sections =
    {
        ("الحسابات الوسيطة (1)", new[]
        {
            "حساب فروق العملة", "الأصناف المفقودة/الزائدة", "أوراق الدفع", "أوراق القبض",
            "حساب فوارق الكسور", "مركز الحسابات الوسيطة", "حساب فوارق التكلفة",
            "فوارق الكسور بعد ضريبة المبيعات", "عمولات المندوبين", "عمولات الموظفين",
            "رقم حساب العمولة", "الحساب الوسيط لخصم المشتريات", "حساب جاري الفروع",
            "حساب وسيط إيداعات نقدية لدى البنوك", "حساب وسيط الاعتمادات المستندية"
        }),
        ("الحسابات الوسيطة (2)", new[]
        {
            "حساب فوارق التحويل", "حساب فوارق الصرف", "أوراق دفع كمبيالة", "حساب فوارق أصناف مركبة",
            "حساب عجز مبيعات نقاط البيع", "حساب فائض مبيعات نقاط البيع", "حساب وسيط عملاء آجل",
            "حساب وسيط مبيعات الكوبونات", "حساب استبدال النقاط", "حساب وسيط الضريبة المستحقة",
            "حساب وسيط توريد المشتريات", "الحساب الوسيط للاستبدال والدفع النقدي للمرتجعات", "الدفعة المقدمة"
        }),
        ("الحسابات الوسيطة (المرتبات)", new[]
        {
            "حساب وسيط مستحقات الموظفين", "حساب ضريبة الرعاية الصحية"
        }),
        ("الحسابات الوسيطة (الأصول)", new[]
        {
            "وسيط استبعاد الأصول", "وسيط الإضافات الأصول", "أرباح وخسائر الأصول", "وسيط نقل الأصول",
            "حساب وسيط الصيانة", "مركز التكلفة", "المشروع", "النشاط"
        }),
        ("الحسابات الوسيطة (شحن الطرود)", new[]
        {
            "حساب المبيعات", "حساب الخصم المسموح به", "حساب الأعباء"
        }),
        ("الحسابات الوسيطة (نظام إدارة المطاعم)", new[]
        {
            "حساب التسويات المخزنية الآلية", "حساب الأطعمة الجاهزة", "حساب التأمين", "حساب إيجار الأدوات",
            "حساب المصروفات العامة", "حساب المقبوضات", "حساب الأصناف التالفة", "حساب الضيافة", "حساب التغذية"
        }),
        ("الحسابات الوسيطة (نظام إدارة المستشفيات)", new[]
        {
            "وسيط مديونية المرضى", "حساب نسب الأطباء", "خصم لمريض داخلي", "حساب إيرادات الرقود"
        }),
        ("الحسابات الوسيطة (نظام إدارة الأسطول)", new[]
        {
            "حساب الخصم المسموح به للنقل", "حساب عمولة إيرادات النقل", "حساب إيرادات المبيعات",
            "حساب وسيط النقل الداخلي", "حساب تكاليف نقل الموردين", "حساب عمولات السائقين",
            "حساب عهد السائقين", "وسيط مديونية عملاء النقل", "وسيط إيرادات النقل", "حساب مقدم تكلفة النقل",
            "حساب تكلفة تسوية فروقات النقل", "حساب الغرامات", "حساب دائن عمولات السائقين"
        }),
        ("الحسابات الوسيطة (نظام صيانة الأصول)", new[]
        {
            "حساب مصاريف الصيانة", "حساب المرتبات دائن", "مركز تكلفة المرتبات",
            "حساب مصاريف قطع الغيار", "حساب عهد الصيانة للموظفين"
        })
    };

    private static string FieldName(int section, int index) => $"cboIntermediate_{section}_{index}";

    public UcIntermediateAccounts()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
        Load += (_, _) => OnyxPhaseOneProperties.Apply(this);
        // These reference displays have no additional persistence contract.
        Control[] referenceDisplays = { txtName_cboIntermediate_0_0, txtName_cboIntermediate_0_1, txtName_cboIntermediate_0_2, txtName_cboIntermediate_0_3, txtName_cboIntermediate_0_4, txtName_cboIntermediate_0_5, txtName_cboIntermediate_0_6, txtName_cboIntermediate_0_7, txtName_cboIntermediate_0_8, txtName_cboIntermediate_0_9, txtName_cboIntermediate_0_10, txtName_cboIntermediate_0_11, txtName_cboIntermediate_0_12, txtName_cboIntermediate_0_13, txtName_cboIntermediate_0_14, txtName_cboIntermediate_1_0, txtName_cboIntermediate_1_1, txtName_cboIntermediate_1_2, txtName_cboIntermediate_1_3, txtName_cboIntermediate_1_4, txtName_cboIntermediate_1_5, txtName_cboIntermediate_1_6, txtName_cboIntermediate_1_7, txtName_cboIntermediate_1_8, txtName_cboIntermediate_1_9, txtName_cboIntermediate_1_10, txtName_cboIntermediate_1_11, txtName_cboIntermediate_1_12, cboReferenceBranch };
        var referenceNames = referenceDisplays.Select(c => c.Name).ToArray();
        foreach (var display in referenceDisplays) display.Name = string.Empty;
        foundation = new TransportERP.Desktop.CoreUI.FoundationUiSession(layout, bindDisabledActions: false);
        for (int i = 0; i < referenceDisplays.Length; i++) referenceDisplays[i].Name = referenceNames[i];

        var tabs = (TabControl)Controls.Find("tabsFields", true)[0];
        tabs.Multiline = false;
        var toolbar = (FlowLayoutPanel)Controls.Find("pnlToolbar", true)[0];
        Controls.Find("lblStatus", true)[0].Text =
            "معاينة الحسابات الوسيطة — قوائم الدليل المحاسبي وإعدادات الأنظمة غير مرتبطة؛ الإضافة والتعديل والحفظ لا تُطبّق على النظام.";
        // Conditional fields stay visible but disabled: no assumption that module prerequisites are active.
        foreach (var (section, index, reason) in new[]
        {
            (0, 10, "يتطلب ترحيل عمولة المندوبين في فاتورة المبيعات."),
            (0, 12, "يتطلب إعداد التعامل مع حساب جاري الفروع."),
            (0, 13, "يتطلب توسيط حساب إيداعات نقدية لدى البنوك."),
            (3, 5, "يتطلب ربط مراكز التكلفة على مستوى الفرع في نظام الأصول."),
            (3, 6, "يتطلب ربط المشاريع على مستوى الفرع في نظام الأصول."),
            (3, 7, "يتطلب ربط الأنشطة على مستوى الفرع في نظام الأصول.")
        })
            Controls.Find(FieldName(section, index), true)[0].AccessibleDescription = reason + " الإعداد والقائمة غير مرتبطين.";
        TransportERP.Desktop.CoreUI.SharedScreenProperties.ApplyContent(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
    private readonly TransportERP.Desktop.CoreUI.FoundationUiSession foundation;
    [System.ComponentModel.Browsable(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public TransportERP.Desktop.CoreUI.FoundationUiSession Foundation => foundation;
    [System.ComponentModel.Browsable(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public bool HasUnsavedChanges => foundation.HasUnsavedChanges;
    [System.ComponentModel.Browsable(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public bool IsBusy => foundation.IsBusy;
    public bool ConfirmLeave() => foundation.ConfirmLeave();
    private void accountSelection_Changed(object? sender, EventArgs e)
    {
        if (sender is not ComboBox account) return;
        var display = Controls.Find("txtName_" + account.Name, true).OfType<TextBox>().SingleOrDefault();
        if (display != null)
            display.Text = account.SelectedItem is TransportERP.Desktop.CoreUI.FoundationChoice choice ? choice.Label : string.Empty;
    }

    private void draftAction_Click(object? sender, EventArgs e)
    {
        fields.Enabled = true;

        sectionFields0.Enabled = true;         sectionFields1.Enabled = true;         sectionFields2.Enabled = true;         sectionFields3.Enabled = true;         sectionFields4.Enabled = true;         sectionFields5.Enabled = true;         sectionFields6.Enabled = true;         sectionFields7.Enabled = true;         sectionFields8.Enabled = true;
        standardCommandAdd.Enabled = false;
        var firstEditor = fields.Controls.Cast<Control>().FirstOrDefault(c => c.Enabled && (c is TextBox { ReadOnly: false } || c is CheckBox));
        if (firstEditor != null) firstEditor.Focus();

        tabsFields.SelectNextControl(null, true, true, true, false);
    }
    private void grid_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (sender is DataGridView { IsCurrentCellDirty: true, CurrentCell: DataGridViewCheckBoxCell } grid)
            grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }}
