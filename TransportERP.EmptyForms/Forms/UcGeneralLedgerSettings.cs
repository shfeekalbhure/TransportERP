using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransportERP.Contracts.Accounting;

namespace TransportERP.EmptyForms;

/// <summary>Company ledger workflow settings, using the shared accounting settings service.</summary>
public partial class UcGeneralLedgerSettings : UserControl, TransportERP.Desktop.CoreUI.IWorkspaceCloseGuard
{
    private LedgerSettingsDocument? document;
    private Func<LedgerSettingsUpdate, Task<LedgerSettingsDocument>>? save;
    private Func<Task<LedgerSettingsDocument>>? reload;
    private Func<Task>? openReceiptSettings;
    private LedgerWorkflowPolicy baseline = LedgerWorkflowPolicy.Legacy;
    private bool loading;
    private bool busy;
    public bool IsBusy => busy;
    public bool HasUnsavedChanges => !loading && CurrentPolicy != baseline;
    public LedgerWorkflowPolicy CurrentPolicy => new(chkRequireReview.Checked, chkRequireApproval.Checked,
        cmbPostingMode.SelectedIndex == 1 ? "AUTOMATIC" : "MANUAL")
    {
        RequireJournalDescription = chkRequireJournalDescription.CheckState == CheckState.Indeterminate
            ? null : chkRequireJournalDescription.Checked
    };
    public event EventHandler? CloseRequested;
    internal Button CloseButton => btnClose;

    public UcGeneralLedgerSettings()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.ScreenProperties.Apply(this);
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
        TransportERP.Desktop.CoreUI.SharedScreenProperties.ApplyContent(this);
        AutoScrollMinSize = new System.Drawing.Size(820, 520);
        pnlActions.WrapContents = false;
        chkRequireReview.CheckedChanged += (_, _) => RefreshState();
        chkRequireApproval.CheckedChanged += (_, _) => RefreshState();
        chkRequireJournalDescription.CheckStateChanged += (_, _) => RefreshState();
        cmbPostingMode.SelectedIndexChanged += (_, _) => RefreshState();
        btnSave.Click += async (_, _) => await SaveSettingsAsync();
        btnRefresh.Click += async (_, _) => await ReloadSettingsAsync();
        btnUndo.Click += (_, _) => UndoChanges();
        // The workspace invokes IWorkspaceCloseGuard once before closing the tab.
        btnClose.Click += (_, _) => { if (!busy) CloseRequested?.Invoke(this, EventArgs.Empty); };
        btnReceiptSettings.Click += async (_, _) => await OpenReceiptSettingsAsync();
        ApplyPolicy(baseline);
        RefreshState();
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public void ConnectLedgerSettings(LedgerSettingsDocument value,
        Func<LedgerSettingsUpdate, Task<LedgerSettingsDocument>> saveHandler,
        Func<Task<LedgerSettingsDocument>> reloadHandler)
    {
        if (busy || HasUnsavedChanges) throw new InvalidOperationException("احفظ التعديلات أو تراجع عنها قبل إعادة الربط.");
        save = saveHandler; reload = reloadHandler; ApplyDocument(value);
    }

    public void ConnectReceiptSettings(Func<Task> handler)
    { openReceiptSettings = handler; RefreshState(); }

    public void ShowConnectionError(string message)
    { lblPreview.Text = message; }

    private void ApplyPolicy(LedgerWorkflowPolicy policy)
    {
        loading = true;
        try
        {
            chkRequireReview.Checked = policy.RequireReview;
            chkRequireApproval.Checked = policy.RequireApproval;
            chkRequireJournalDescription.CheckState = policy.RequireJournalDescription switch
            { true => CheckState.Checked, false => CheckState.Unchecked, null => CheckState.Indeterminate };
            cmbPostingMode.SelectedIndex = policy.PostingMode == "AUTOMATIC" ? 1 : 0;
        }
        finally { loading = false; }
    }

    private void ApplyDocument(LedgerSettingsDocument value)
    {
        document = value;
        baseline = value.Policy ?? LedgerWorkflowPolicy.Legacy;
        ApplyPolicy(baseline);
        lblScope.Text = "الشركة: " + value.CompanyName + "\r\nفرع الجلسة: " + value.BranchName +
            "\r\nنطاق السياسة: الشركة بجميع فروعها؛ صلاحيات المستندات تبقى مقيدة بنطاق المستخدم.";
        lblCoverage.Text = "التغطية الفعلية الحالية:\r\n" +
            (value.SupportedDocumentTypes.Count == 0 ? "لم تُعلن الخدمة عن مستندات مدعومة." : string.Join("\r\n", value.SupportedDocumentTypes.Select(x => "• " + (x == "RECEIPT_VOUCHER" ? "سند القبض: النقد والشيكات — إنشاء القيد الفعلي" : x)))) +
            "\r\nالأنواع الأخرى تحتفظ بسلوكها القائم حتى ربط خدماتها بهذه السياسة.";
        lblPolicyStatus.Text = value.Policy == null
            ? "لم تُحفظ سياسة عامة بعد. يستمر السلوك القائم: حفظ، اعتماد، ثم ترحيل يدوي. القيم المعروضة لا تُفعّل سياسة جديدة قبل الحفظ الصريح."
            : "السياسة العامة محفوظة. تُثبت نسخة السياسة على المستند عند إنشائه؛ تغييرها هنا لا يعيد كتابة المستندات القائمة أو القيود المرحلة.";
        auditGrid.Rows.Clear();
        foreach (var item in value.Audit)
            auditGrid.Rows.Add(item.At.UtcDateTime.ToString("yyyy-MM-dd HH:mm:ss 'UTC'"), item.Actor,
                item.Action == "ConfigureGeneralLedger" ? "تعديل سياسة الأستاذ العام" : item.Action, item.Reason ?? "—");
        lblPreview.Text = value.CanConfigure ? "تم تحميل إعدادات الشركة. لم تُجرَ تغييرات." : "عرض فقط: لا تتوفر صلاحية إعداد الأستاذ العام على مستوى الشركة.";
        RefreshState();
    }

    private void RefreshState()
    {
        if (loading || IsDisposed) return;
        var canEdit = document?.CanConfigure == true && save != null && !busy;
        chkRequireReview.Enabled = chkRequireApproval.Enabled = cmbPostingMode.Enabled = canEdit;
        chkRequireJournalDescription.Enabled = canEdit;
        chkRequireJournalDescription.Text = "إدخال بيان قيد اليومية إجباري — " +
            (CurrentPolicy.RequireJournalDescription switch { true => "نعم", false => "لا", null => "غير محدد" });
        btnSave.Enabled = canEdit && (document!.Policy == null || HasUnsavedChanges);
        btnSave.Text = document?.Policy == null ? "حفظ وتفعيل" : "حفظ";
        btnUndo.Enabled = !busy && HasUnsavedChanges;
        btnRefresh.Enabled = !busy && reload != null;
        btnClose.Enabled = !busy;
        btnReceiptSettings.Enabled = !busy && !HasUnsavedChanges && openReceiptSettings != null;
        lblDirty.Text = busy ? "جارٍ تنفيذ العملية…" : HasUnsavedChanges ? "تعديلات غير محفوظة" : "لا توجد تعديلات غير محفوظة";
        lblPath.Text = CurrentPolicy.Description + "\r\n" +
            (CurrentPolicy.PostingMode == "AUTOMATIC"
                ? "يبدأ الترحيل بعد نجاح آخر مرحلة مطلوبة فقط. يلزم أن يملك منفذ تلك المرحلة صلاحية الترحيل الحالية؛ وإلا تُحفظ المرحلة دون ترحيل مع بيان السبب. فشل الترحيل لا يعني فشل حفظ المرحلة، ويمكن إعادة المحاولة دون تكرار القيد."
                : "بعد اكتمال المراحل المطلوبة يستخدم المخوّل أمر الترحيل يدويًا.");
    }

    public async Task SaveSettingsAsync()
    {
        if (busy || save == null || document?.CanConfigure != true) return;
        if (cmbPostingMode.SelectedIndex < 0) { lblPreview.Text = "اختر طريقة الترحيل."; return; }
        var update = new LedgerSettingsUpdate(CurrentPolicy, document.Version);
        busy = true; RefreshState();
        try
        {
            var result = await save(update);
            if (IsDisposed) return;
            ApplyDocument(result);
            lblPreview.Text = "حُفظت سياسة الأستاذ العام. تطبق على المستندات الجديدة المدعومة؛ المستندات القائمة تحتفظ بسياستها.";
        }
        catch (Exception ex) { if (!IsDisposed) lblPreview.Text = "تعذر تأكيد حفظ الإعدادات: " + ex.Message + " أعد التحميل للتحقق؛ التعديلات المحلية محفوظة بالشاشة."; }
        finally { busy = false; RefreshState(); }
    }

    public async Task ReloadSettingsAsync()
    {
        if (busy || reload == null || !ConfirmDiscard()) return;
        busy = true; RefreshState();
        try { var value = await reload(); if (!IsDisposed) ApplyDocument(value); }
        catch (Exception ex) { if (!IsDisposed) lblPreview.Text = "تعذر تحميل الإعدادات: " + ex.Message; }
        finally { busy = false; RefreshState(); }
    }

    public bool UndoChanges(Func<bool>? confirm = null)
    {
        if (busy || !HasUnsavedChanges || !(confirm?.Invoke() ?? ConfirmDiscard())) return false;
        ApplyPolicy(baseline); RefreshState(); lblPreview.Text = "تم التراجع عن التعديلات غير المحفوظة فقط."; return true;
    }

    private async Task OpenReceiptSettingsAsync()
    {
        if (busy || HasUnsavedChanges || openReceiptSettings == null) return;
        busy = true; RefreshState();
        try { await openReceiptSettings(); }
        catch (Exception ex) { if (!IsDisposed) lblPreview.Text = "تعذر فتح أو حفظ إعدادات القبض: " + ex.Message; }
        finally { busy = false; RefreshState(); }
    }

    private bool ConfirmDiscard() => !HasUnsavedChanges || MessageBox.Show(this,
        "توجد تعديلات غير محفوظة. هل تريد تجاهلها؟", "إعدادات الأستاذ العام",
        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
    public bool ConfirmLeave() => !busy && ConfirmDiscard();
}
