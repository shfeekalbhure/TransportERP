using System;
using System.Windows.Forms;
namespace TransportERP.EmptyForms;

/// <summary>Navigation host for the independently designed UcGeneralLedgerSettings.</summary>
public partial class FrmOnyxSCREEN0078 : Form
{
    public FrmOnyxSCREEN0078()
    {
        InitializeComponent();
        content.CloseRequested += Content_CloseRequested;
        CancelButton = content.CloseButton;
        FormClosing += (_, e) => { if (!content.ConfirmLeave()) e.Cancel = true; };
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }
    private void Content_CloseRequested(object? sender, EventArgs e) => Close();
}
