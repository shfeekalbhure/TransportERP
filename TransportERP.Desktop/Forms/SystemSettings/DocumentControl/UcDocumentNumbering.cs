using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.DocumentControl;

public partial class UcDocumentNumbering : UserControl, IFoundationScreen
{
    public FoundationUiSession Foundation { get; }
    public bool HasUnsavedChanges => Foundation.HasUnsavedChanges;
    public bool IsBusy => Foundation.IsBusy;
    public bool ConfirmLeave() => Foundation.ConfirmLeave();

    public event EventHandler? CloseRequested;
    internal Button CloseButton => btnClose;

    public UcDocumentNumbering()
    {
        InitializeComponent();
        SharedScreenProperties.Apply(this);
        ApplyReferenceDesign();

        btnClose.Click += BtnClose_Click;

        cboCompany.SelectedIndexChanged += (_, _) => UpdateScopeDisplay();
        cboBranch.SelectedIndexChanged += (_, _) => UpdateScopeDisplay();
        cboFiscalYear.SelectedIndexChanged += (_, _) => UpdateScopeDisplay();

        cboCompany.TextChanged += (_, _) => UpdateScopeDisplay();
        cboBranch.TextChanged += (_, _) => UpdateScopeDisplay();
        cboFiscalYear.TextChanged += (_, _) => UpdateScopeDisplay();

        UpdateScopeDisplay();
        SharedScreenProperties.ApplyContent(this);

        Foundation = new FoundationUiSession(this);
        Foundation.RequireFields(
            new[] { "btnSave", "btnPublish", "btnValidate" },
            cboCompany.Name,
            cboBranch.Name,
            cboFiscalYear.Name,
            cboDocumentType.Name);
        ApplyNumberingRequirementColors();
        Load += (_, _) => ApplyNumberingRequirementColors();
        Foundation.DocumentLoaded += (_, _) => ApplyNumberingRequirementColors();
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    private void UpdateScopeDisplay()
    {
        txtScopeCompany.Text = cboCompany.Text;
        txtScopeBranch.Text = cboBranch.Text;
        txtScopeYear.Text = cboFiscalYear.Text;
    }

    private void ApplyNumberingRequirementColors()
    {
        // These four editors are the existing Save/Publish/Validate contract.
        // Scope textboxes are derived displays, not independent required inputs.
        foreach (Control editor in new Control[] { cboCompany, cboBranch, cboFiscalYear, cboDocumentType })
            global::TransportERP.RequiredFieldAppearance.Apply(editor, true);
        foreach (Control editor in new Control[] { txtScopeCompany, txtScopeBranch, txtScopeYear,
            txtStatus, txtOverridePolicy, txtExpectedVersion, txtPermissionStatus,
            txtApprovalStatus, txtReservationNumber, txtReservationState, txtReservationId })
            global::TransportERP.RequiredFieldAppearance.Apply(editor, false);
    }

    private void BtnClose_Click(object? sender, EventArgs e)
        => CloseRequested?.Invoke(this, EventArgs.Empty);

    private void ApplyReferenceDesign()
    {
        cboResetType.Items.Clear();
        cboResetType.DisplayMember = nameof(FoundationChoice.Label);
        cboResetType.ValueMember = nameof(FoundationChoice.Id);

        cboResetType.DataSource = new[]
        {
            new FoundationChoice("NONE", "بدون إعادة"),
            new FoundationChoice("COMPANY", "حسب الشركة"),
            new FoundationChoice("BRANCH", "حسب الفرع"),
            new FoundationChoice("YEAR", "حسب السنة"),
            new FoundationChoice("COMPANY_YEAR", "حسب الشركة والسنة"),
            new FoundationChoice("BRANCH_YEAR", "حسب الفرع والسنة")
        };

        cboResetType.SelectedValue = "NONE";
        cboResetType.Tag = "FLD-SET-NUM-001";
        lblResetType.Text = "سياسة إعادة الترقيم";

        cboResetType.DropDownWidth =
            ((FoundationChoice[])cboResetType.DataSource)
            .Max(x => TextRenderer.MeasureText(
                x.Label, cboResetType.Font).Width)
            + SystemInformation.VerticalScrollBarWidth + 16;
    }
}
