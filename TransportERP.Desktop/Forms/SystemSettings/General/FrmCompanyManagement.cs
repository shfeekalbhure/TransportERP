using System;
using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

/// <summary>SCR-SET-004. Form host retained for the existing navigation registry.</summary>
public sealed partial class FrmCompanyManagement : FrmBase
{
    public FrmCompanyManagement()
    {
        InitializeComponent();
        CancelButton = companyManagement.CloseButton;
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }

    private void CompanyManagement_CloseRequested(object? sender, EventArgs e) => Close();

    private void companyManagement_Load(object sender, EventArgs e)
    {

    }
}
