using System;
using System.Windows.Forms;
namespace TransportERP.EmptyForms;

/// <summary>Navigation host for the independently designed UcChartOfAccounts.</summary>
public partial class FrmScreen_04_01_01 : Form
{
    public FrmScreen_04_01_01()
    {
        InitializeComponent();
        content.CloseRequested += Content_CloseRequested;
        CancelButton = content.CloseButton;
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }
    private void Content_CloseRequested(object? sender, EventArgs e) => Close();
}
