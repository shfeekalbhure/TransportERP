using System;
using System.Windows.Forms;
namespace TransportERP.EmptyForms;

/// <summary>Navigation host for the independently designed UcAccountGroupsAndTypes.</summary>
public partial class FrmScreen_04_01_02 : Form
{
    public FrmScreen_04_01_02()
    {
        InitializeComponent();
        content.CloseRequested += Content_CloseRequested;
        CancelButton = content.CloseButton;
    }
    private void Content_CloseRequested(object? sender, EventArgs e) => Close();
}
