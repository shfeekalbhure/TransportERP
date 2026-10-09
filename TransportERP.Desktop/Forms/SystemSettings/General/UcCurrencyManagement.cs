using System;
using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

/// <summary>إدارة العملات. Designer-editable preview; database actions are intentionally disabled.</summary>
public partial class UcCurrencyManagement : UserControl, TransportERP.Desktop.CoreUI.IFoundationScreen, TransportERP.Desktop.CoreUI.IExplicitScreenLayout
{
    public TransportERP.Desktop.CoreUI.FoundationUiSession Foundation { get; }
    public bool HasUnsavedChanges => Foundation.HasUnsavedChanges;
    public bool IsBusy => Foundation.IsBusy;
    public bool ConfirmLeave() => Foundation.ConfirmLeave();

    public UcCurrencyManagement()
    {
        InitializeComponent();
        OnyxPhaseOneProperties.Apply(this);
        Foundation = new TransportERP.Desktop.CoreUI.FoundationUiSession(this);
        Foundation.ValidateAction = action =>
        {
            if (action != "btnSave") return null;
            if (tabMain.SelectedTab == tabCurrencyRoles)
                return cmbCompany.SelectedItem == null || cmbAccountingCurrency.SelectedItem == null ? "اختر الشركة والعملة المحاسبية." : null;
            return string.IsNullOrWhiteSpace(txtCurrencyCode.Text) || string.IsNullOrWhiteSpace(txtCurrencyNameAr.Text)
                ? "أكمل رمز العملة والاسم العربي." : null;
        };
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public event EventHandler? CloseRequested;
    internal Button CloseButton => btnClose;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);

}
