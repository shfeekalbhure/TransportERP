using System;
using System.Windows.Forms;
namespace TransportERP.EmptyForms;

/// <summary>Navigation host for the independently designed UcAccountCostCenterLinking.</summary>
public partial class FrmOnyxSCREEN0093 : Form
{
    public FrmOnyxSCREEN0093()
    {
        InitializeComponent();
        content.CloseRequested += Content_CloseRequested;
        CancelButton = content.CloseButton;
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }
    private void Content_CloseRequested(object? sender, EventArgs e) => Close();
}
