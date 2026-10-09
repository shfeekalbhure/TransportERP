namespace TransportERP.Desktop.Forms.SystemSettings.General;

// Sole source: Scribd document 973153595, ONYX ERP v8 تهيئة النظام, printed pp.55–59.
// Text-driven draft only. Parent inheritance, account level and system prerequisites are unconnected.
public sealed partial class UcAccountingChartData : UserControl, TransportERP.Desktop.CoreUI.IFoundationScreen, TransportERP.Desktop.CoreUI.IExplicitScreenLayout
{
    private readonly Button openingRequestButton;

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F5 && openingRequestButton.Enabled)
        {
            openingRequestButton.PerformClick();
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    public UcAccountingChartData()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
        Load += (_, _) => ApplyReferenceProperties();
         // These preview lists were created after the original Foundation snapshot; keep them outside its business-field contract.
        lstbtnSelectCenters.Name = string.Empty;
        lstbtnSelectProjects.Name = string.Empty;
        lstbtnSelectActivities.Name = string.Empty;
        // The empty PDF24–26 currency grid is presentation only; preserve the existing currency field contract.
        currencyPreview.Name = string.Empty;
        foundation = new TransportERP.Desktop.CoreUI.FoundationUiSession(layout, bindDisabledActions: false);
        currencyPreview.Name = "currencyPreview";
        lstCurrencies.ItemCheck += (_, _) => UpdateCurrencyPresentation();
        lstCurrencies.SelectedIndexChanged += (_, _) => UpdateCurrencyPresentation();
        lstCurrencies.DataSourceChanged += (_, _) => UpdateCurrencyPresentation();
        lstCurrencies.EnabledChanged += (_, _) => UpdateCurrencyPresentation();
        lstCurrencies.Invalidated += (_, _) => UpdateCurrencyPresentation();
        Layout += (_, _) => UpdateCurrencyPresentation();
        Load += (_, _) => UpdateCurrencyPresentation();
        lstbtnSelectCenters.Name = "lstbtnSelectCenters";
        lstbtnSelectProjects.Name = "lstbtnSelectProjects";
        lstbtnSelectActivities.Name = "lstbtnSelectActivities";
        // Source permits choosing nature/report for a root account (parent zero).
        // Descendant inheritance and leaf-account eligibility require connected account data.
        var parent = (TextBox)Controls.Find("txtParentAccount", true)[0];
        void UpdateRootFields()
        {
            bool root = parent.Text.Trim() is "0" or "٠";
            Controls.Find("cboNature", true)[0].Enabled = root;
            Controls.Find("cboReport", true)[0].Enabled = root;
        }
        parent.TextChanged += (_, _) => UpdateRootFields();
        UpdateRootFields();
        foreach (var name in new[] { "cboCashFlow", "cboCenters", "cboProjects", "cboActivities" })
            Controls.Find(name, true)[0].Enabled = false;
        Controls.Find("chkWithholding", true)[0].Enabled = false;
        Controls.Find("cboFxClearingAccount", true)[0].AccessibleDescription =
            "يستخدم عند تعدد حسابات فروق العملة لحساب له عملة أجنبية (ص.8)؛ الإعداد وقائمة الحسابات غير مرتبطين.";
        // Source p.56: choose the account from an opening request through F5.
        openingRequestButton = btnOpeningRequest;
        openingRequestButton.Text = "F5";

        // pp.58: project/activity usage follows the same selection mechanism as cost centers.


        var toolbar = (FlowLayoutPanel)Controls.Find("pnlToolbar", true)[0];

        SourceSetupLayout.AddToolbarAction(this, "btnOpeningRequest", "طلب فتح حساب (F5)");
        SourceSetupLayout.AddToolbarAction(this, "btnSelectCenters", "مراكز التكلفة");
        SourceSetupLayout.AddToolbarAction(this, "btnSelectProjects", "المشاريع");
        SourceSetupLayout.AddToolbarAction(this, "btnSelectActivities", "الأنشطة");
        SourceSetupLayout.ApplyReviewedProperties(this);

        // Reference layout and all editors are declared in Designer; the host retains scrolling.
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

        standardCommandAdd.Enabled = false;
        Control? firstEditor = txtParentAccount.Enabled && !txtParentAccount.ReadOnly ? txtParentAccount : null;
        if (firstEditor != null) firstEditor.Focus();

    }
    private void grid_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (sender is DataGridView { IsCurrentCellDirty: true, CurrentCell: DataGridViewCheckBoxCell } grid)
            grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }
    private void btnOpeningRequest_Click(object? sender, EventArgs e) => grpbtnOpeningRequest.Visible = !grpbtnOpeningRequest.Visible;

    private void btnSelectCenters_Click(object? sender, EventArgs e) => grpbtnSelectCenters.Visible = !grpbtnSelectCenters.Visible;

    private void btnSelectProjects_Click(object? sender, EventArgs e) => grpbtnSelectProjects.Visible = !grpbtnSelectProjects.Visible;

    private void btnSelectActivities_Click(object? sender, EventArgs e) => grpbtnSelectActivities.Visible = !grpbtnSelectActivities.Visible;
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

    private bool updatingCurrencyPresentation;
    private bool? showingCurrencyOptions;

    private void UpdateCurrencyPresentation()
    {
        if (updatingCurrencyPresentation) return;
        updatingCurrencyPresentation = true;
        try
        {
            // Existing options continue to use the original editor and its bindings.
            // The picture-only grid is shown solely for the current unconnected empty state.
            bool hasOptions = lstCurrencies.Items.Count > 0;
            if (showingCurrencyOptions == hasOptions) return;
            showingCurrencyOptions = hasOptions;
            lstCurrencies.Visible = hasOptions;
            currencyPreview.Visible = !hasOptions;
        }
        finally { updatingCurrencyPresentation = false; }
    }

    private void standardCommandImport_Click(object? sender, EventArgs e)
    {
        using var shortcuts = new FrmAccountingImportShortcuts();
        shortcuts.ShowDialog(FindForm());
    }
}
