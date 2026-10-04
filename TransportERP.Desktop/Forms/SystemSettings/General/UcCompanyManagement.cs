using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

/// <summary>Company editor layout. Persistence and action workflows are not implemented yet.</summary>
public partial class UcCompanyManagement : UserControl
{
    public UcCompanyManagement()
    {
        InitializeComponent();
        btnClose.Click += BtnClose_Click;
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
