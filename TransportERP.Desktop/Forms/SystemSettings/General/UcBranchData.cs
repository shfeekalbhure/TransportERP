using System.ComponentModel;
using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

// Text authority: Scribd 973153595, printed pp.24–27.
// DBT layout policy is explicit in Designer.cs; branch reference-image fidelity remains unverified.
public partial class UcBranchData : UserControl, IFoundationScreen, IExplicitScreenLayout
{
    private readonly FoundationUiSession foundation;

    public UcBranchData()
    {
        InitializeComponent();
        foundation = new FoundationUiSession(this, bindDisabledActions: false);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public FoundationUiSession Foundation => foundation;
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool HasUnsavedChanges => foundation.HasUnsavedChanges;
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsBusy => foundation.IsBusy;
    public bool ConfirmLeave() => foundation.ConfirmLeave();

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        TransportERP.Desktop.CoreUI.AuditMetadataInstaller.Apply(this, TransportERP.Desktop.CoreUI.AuditMetadataProfile.Standard);
        if (!DesignMode && LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            UpdateLiteVisibility();
    }

    private void btnAdd_Click(object? sender, EventArgs e)
    {
        fieldsMain.Enabled = true;
        fieldsDetails.Enabled = true;
        fieldsArchive.Enabled = true;
        fieldsDocuments.Enabled = true;
        fieldsHeaders.Enabled = true;
        fieldsAddresses.Enabled = true;
        btnToolbarAddresses.Enabled = true;
        btnAdd.Enabled = false;
        tabsFields.SelectedTab = tabMain;
        txtYear.Focus();
        // Save, logo loading and data-backed lists remain unconnected.
    }

    private void chkLite_CheckedChanged(object? sender, EventArgs e)
    {
        if (!DesignMode && LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            UpdateLiteVisibility();
    }

    private void UpdateLiteVisibility()
    {
        bool linked = chkLite.Checked;
        txtConnection.Visible = linked;
        lbltxtConnection.Visible = linked;
        cboConnection.Visible = linked;
        lblcboConnection.Visible = linked;
        if (linked && !tabsFields.TabPages.Contains(tabDocuments))
            tabsFields.TabPages.Insert(3, tabDocuments);
        else if (!linked && tabsFields.TabPages.Contains(tabDocuments))
            tabsFields.TabPages.Remove(tabDocuments);
    }

    private void toolbarAddresses_Click(object? sender, EventArgs e)
    {
        tabsFields.SelectedTab = tabAddresses;
        addresses_Click(sender, e);
    }

    private void addresses_Click(object? sender, EventArgs e)
    {
        grpbtnSelectAddressesIdentifiers.Visible = !grpbtnSelectAddressesIdentifiers.Visible;
    }
}

