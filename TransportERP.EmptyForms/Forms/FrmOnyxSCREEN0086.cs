using System;
using System.Windows.Forms;
namespace TransportERP.EmptyForms;

/// <summary>Navigation host for the independently designed UcAccountOpeningRequest.</summary>
public partial class FrmOnyxSCREEN0086 : Form
{
    public FrmOnyxSCREEN0086()
    {
        InitializeComponent();
        content.CloseRequested += Content_CloseRequested;
        CancelButton = content.CloseButton;
    }
    private void Content_CloseRequested(object? sender, EventArgs e) => Close();
}
