using System;
using System.Windows.Forms;

namespace TransportERP.EmptyForms;

/// <summary>مجموعات وأنواع الحسابات. Designer-editable preview; database actions are intentionally disabled.</summary>
public partial class UcAccountGroupsAndTypes : UserControl
{
    public UcAccountGroupsAndTypes()
    {
        InitializeComponent();
    }

    public event EventHandler? CloseRequested;
    internal Button CloseButton => btnClose;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);

}
