using System;
using System.Windows.Forms;

namespace TransportERP.EmptyForms;

/// <summary>طلب فتح حساب. Designer-editable preview; database actions are intentionally disabled.</summary>
public partial class UcAccountOpeningRequest : UserControl
{
    public UcAccountOpeningRequest()
    {
        InitializeComponent();
    }

    public event EventHandler? CloseRequested;
    internal Button CloseButton => btnClose;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);

}
