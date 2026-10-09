using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

/// <summary>Company editor layout. Persistence and action workflows are not implemented yet.</summary>
public partial class UcCompanyManagement : UserControl, TransportERP.Desktop.CoreUI.IFoundationScreen, TransportERP.Desktop.CoreUI.IExplicitScreenLayout
{
    public TransportERP.Desktop.CoreUI.FoundationUiSession Foundation { get; }
    public bool HasUnsavedChanges => Foundation.HasUnsavedChanges;
    public bool IsBusy => Foundation.IsBusy;
    public bool ConfirmLeave() => Foundation.ConfirmLeave();

    public UcCompanyManagement()
    {
        InitializeComponent();
        OnyxPhaseOneProperties.Apply(this);
        ConfigureUnavailableActions();
        Foundation = new TransportERP.Desktop.CoreUI.FoundationUiSession(this);
        Foundation.RequireFields(new[] { "btnSave", "btnPublish", "btnValidate" }, txtCompanyNameAr.Name, cmbActivityType.Name, cmbBaseCurrency.Name, cmbCountry.Name);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}



    private void ConfigureUnavailableActions()
    {
        const string reason = "غير متاح حاليًا: لم يُنفَّذ هذا الإجراء ولم يُربط بخدمة تشغيلية.";
        components ??= new System.ComponentModel.Container();
        var availabilityTip = new ToolTip(components) { ShowAlways = true };
        tabCompaniesSection.Text += " — الحفظ غير مرتبط";
        availabilityTip.SetToolTip(btnClose, "إغلاق الشاشة");
        foreach (var button in new[] { button1, btnSave, btnEdit, btnDelete, button2, btnFirst, btnPrevious, btnNext, btnLast, btnUndo, btnSearch, btnPrint, button4, btnAddLogo, btnRemoveLogo })
        {
            button.Enabled = false;
            button.AccessibleDescription = reason;
            availabilityTip.SetToolTip(button, reason);
            // Disabled buttons do not reliably receive hover messages. Their host
            // also explains the disabled state without changing the layout.
            if (button.Parent is Control host)
                availabilityTip.SetToolTip(host, reason);
        }
    }

    [Category("Action")]
    public event EventHandler? CloseRequested;

    internal Button CloseButton => btnClose;

    private void BtnClose_Click(object? sender, EventArgs e)
        => CloseRequested?.Invoke(this, EventArgs.Empty);

    private void lblTitle_Click(object sender, EventArgs e)
    {

    }
}
