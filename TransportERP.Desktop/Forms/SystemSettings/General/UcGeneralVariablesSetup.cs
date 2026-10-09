namespace TransportERP.Desktop.Forms.SystemSettings.General;

// Sole source: Scribd document 973153595, printed pp.3–10. Draft settings never affect other screens.
public sealed partial class UcGeneralVariablesSetup : UserControl, TransportERP.Desktop.CoreUI.IFoundationScreen, TransportERP.Desktop.CoreUI.IExplicitScreenLayout
{
    private static readonly Field[] MainFields =
    {
        new("txtMobilePath", "مسار ملفات وأيقونات تطبيقات الموبايل", FieldKind.ForeignText),
        new("chkSingleSession", "عدم السماح للمستخدم من الاتصال بأكثر من جهاز في نفس الوقت", FieldKind.Check),
        new("chkSingleDevice", "ربط المستخدم بجهاز واحد", FieldKind.Check),
        new("chkUserHeader", "ترويسة التقارير حسب المستخدم", FieldKind.Check),
        new("chkReview", "استخدام نظام مراجعة الوثائق", FieldKind.Check),
        new("chkSeparateStock", "استخدام نظام المخزون المنفصل", FieldKind.Check),
        new("chkLedgerOnly", "استخدام نظام الأستاذ العام فقط", FieldKind.Check),
        new("chkBranchClearing", "استخدام توسيط آلي لحساب جاري الفروع", FieldKind.Check),
        new("cboBranchPosting", "نوع توسيط قيد جاري الفروع للمخزون", FieldKind.Choice, new[] { "توسيط قيد الإيراد", "توسيط قيد التكاليف" }),
        new("chkLiteBranch", "عدم السماح لمنح صلاحية للمستخدم لأكثر من فرع في نظام اللايت", FieldKind.Check),
        new("chkReportSearch", "عرض محرك بحث التقارير", FieldKind.Check),
        new("chkVat", "استخدام ضريبة القيمة المضافة", FieldKind.Check),
        new("chkWithholding", "استخدام خصم الضريبة من المصدر", FieldKind.Check),
        new("chkInvoice", "استخدام الفاتورة الإلكترونية", FieldKind.Check),
        new("chkTaxIncluded", "السعر شامل ضريبة المبيعات", FieldKind.Check),
        new("txtTaxLength", "طول الرقم الضريبي"),
        new("cboTaxPeriod", "نوع الفترة الضريبية", FieldKind.Choice, new[] { "غير مستخدم", "شهري", "ربعي" }),
        new("cboVatPosting", "طريقة ترحيل القيمة المضافة", FieldKind.Choice, new[] { "بعملة المستند", "بالعملة المحلية" }),
        new("txtAccountsYear", "أرصدة الحسابات للسنوات المتعددة تبدأ من سنة"),
        new("txtStockYear", "الأرصدة المخزنية للسنوات المتعددة تبدأ من سنة"),
        new("txtFailedLogins", "توقيف المستخدم بعد فشله في دخول النظام بعد"),
        new("txtPasswordLength", "الحد الأدنى لطول كلمة المرور للمستخدمين")
    };

    private static readonly Field[] VariableFields =
    {
        new("cboAccountNumber", "نوع رقم الحساب", FieldKind.Choice, new[] { "رقمي", "رقمي وحرفي" }),
        new("txtAccountLength", "طول رقم الحساب"),
        new("txtAccountRank", "رتبة الحساب الفرعي"),
        new("chkNoParentAccount", "عدم تضمين رقم الحساب الأعلى في رقم الحساب", FieldKind.Check),
        new("cboFxClearing", "الحساب الوسيط لفروق العملة", FieldKind.Choice, new[] { "وحيد", "متعدد" }),
        new("chkForeignCurrencies", "استخدام العملات الأجنبية", FieldKind.Check),
        new("chkAnalyticalOnly", "ترميز الحسابات التحليلية فقط", FieldKind.Check),
        new("cboOtherSequence", "تسلسل الحسابات المدينة والدائنة الأخرى", FieldKind.Choice, new[] { "حسب نوع الحساب", "حسب المجموعة" }),
        new("cboCenters", "مراكز التكلفة", FieldKind.Choice, new[] { "غير مستخدم", "اختياري", "إجباري", "أرباح وخسائر" }),
        new("cboProjects", "بيانات المشاريع", FieldKind.Choice, new[] { "غير مستخدم", "اختياري", "إجباري", "أرباح وخسائر" }),
        new("cboActivities", "بيانات الأنشطة", FieldKind.Choice, new[] { "غير مستخدم", "اختياري", "إجباري", "أرباح وخسائر" }),
        new("cboDimensionPosting", "طريقة الترحيل للمراكز والمشاريع والأنشطة", FieldKind.Choice,
            new[] { "ترحيل إلى جميع أطراف القيد", "ترحيل إلى حساب الأرباح والخسائر" }),
        new("cboCenterNumber", "نوع رقم المركز", FieldKind.Choice, new[] { "رقمي", "رقمي وحرفي" }),
        new("txtCenterLength", "طول رقم المركز"),
        new("chkNoParentCenter", "عدم تضمين رقم المركز الأعلى في رقم المركز", FieldKind.Check),
        new("chkAccountCenters", "ربط الحسابات بالمراكز", FieldKind.Check),
        new("chkAccountProjects", "ربط الحسابات بالمشاريع", FieldKind.Check),
        new("chkCenterProjects", "ربط المراكز بالمشاريع", FieldKind.Check),
        new("chkProjectActivities", "ربط المشاريع بالأنشطة", FieldKind.Check),
        new("cboEmployeeSequence", "تسلسل رقم الموظف", FieldKind.Choice, new[] { "آلي يمكن تعديله", "آلي لا يمكن تعديله", "يدوي" }),
        new("cboEmployeeSequenceType", "نوع تسلسل الموظف", FieldKind.Choice, new[] { "عام", "حسب حقول الترميز العام" }),
        new("cboEmployeeSequenceField", "حقل التسلسل للموظف", FieldKind.Lookup),
        new("txtEmployeeMinimum", "الحد الأدنى لرقم الموظف"),
        new("txtEmployeeMaximum", "الحد الأعلى لرقم الموظف"),
        new("chkTitleFirst", "اللقب في بداية اسم الموظف", FieldKind.Check),
        new("cboEmployeePermissions", "صلاحيات الموظفين", FieldKind.Choice,
            new[] { "غير مستخدم", "على مستوى الموظف", "على مستوى الهيكل الإداري" })
    };

    public UcGeneralVariablesSetup()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
        Load += (_, _) => ApplyReferenceProperties();
        // PDF1 presentation field has no current save contract; keep it outside the Foundation snapshot.
        txtReferenceWebsite.Name = string.Empty;
        foundation = new TransportERP.Desktop.CoreUI.FoundationUiSession(layout, bindDisabledActions: false);
        txtReferenceWebsite.Name = "txtReferenceWebsite";

        void SetVisible(string name, bool visible)
        {
            Controls.Find(name, true)[0].Visible = visible;
            Controls.Find("lbl" + name, true)[0].Visible = visible;
        }
        void UpdateDependencies()
        {
            bool vat = ((CheckBox)Controls.Find("chkVat", true)[0]).Checked;
            foreach (var name in new[] { "chkInvoice", "chkTaxIncluded", "cboTaxPeriod", "cboVatPosting" }) SetVisible(name, vat);
            SetVisible("cboBranchPosting", ((CheckBox)Controls.Find("chkBranchClearing", true)[0]).Checked);
            SetVisible("cboEmployeeSequenceField", ((ComboBox)Controls.Find("cboEmployeeSequenceType", true)[0]).SelectedIndex == 1);
        }
        foreach (var name in new[] { "chkVat", "chkBranchClearing" })
            ((CheckBox)Controls.Find(name, true)[0]).CheckedChanged += (_, _) => UpdateDependencies();
        ((ComboBox)Controls.Find("cboEmployeeSequenceType", true)[0]).SelectedIndexChanged += (_, _) => UpdateDependencies();
        UpdateDependencies();
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
    private void draftAction_Click(object? sender, EventArgs e)
    {
        fields.Enabled = true;

        sectionFields0.Enabled = true;         sectionFields1.Enabled = true;
        standardCommandEdit.Enabled = false;
        var firstEditor = fields.Controls.Cast<Control>().FirstOrDefault(c => c.Enabled && (c is TextBox { ReadOnly: false } || c is CheckBox));
        if (firstEditor != null) firstEditor.Focus();

        tabsFields.SelectNextControl(null, true, true, true, false);
    }
    private void grid_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (sender is DataGridView { IsCurrentCellDirty: true, CurrentCell: DataGridViewCheckBoxCell } grid)
            grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }
    private enum FieldKind { Text, ForeignText, Lookup, Derived, Check, SingleCheck, Choice, DerivedCheck, MultiLookup }
    private sealed record Field(string Name, string Label, FieldKind Kind = FieldKind.Text, string[]? Choices = null);
    private sealed record Table(string Name, string? Label, bool ReadOnly, params Field[] Columns);
    private void ApplyReferenceProperties()
    {
        var containers = new List<Control>();
        void Suspend(Control parent)
        {
            parent.SuspendLayout();
            containers.Add(parent);
            foreach (Control child in parent.Controls) Suspend(child);
        }
        Suspend(this);
        try { OnyxPhaseOneProperties.Apply(this); }
        finally
        {
            for (int i = containers.Count - 1; i >= 0; i--) containers[i].ResumeLayout(false);
            PerformLayout();
        }
    }
}
