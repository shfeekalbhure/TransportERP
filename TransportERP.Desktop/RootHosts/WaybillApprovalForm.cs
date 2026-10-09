using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

/// <summary>SHP-014 control/approval surface. Approval is online/server-authoritative.</summary>
 
/// <summary>Compatibility window hosting the reusable business surface.</summary>
public sealed partial class WaybillApprovalForm : Form
{
    public UcWaybillApproval Content { get; } = new()
    {
        Dock = DockStyle.Fill
    };

    public WaybillApprovalForm()
    {
        Text = "اعتماد البوليصة — SHP-014";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(760, 520);
        Controls.Add(Content);
        Content.ApproveRequested += (_, e) => ApproveRequested?.Invoke(this, e);
        Content.ReturnRequested += (_, e) => ReturnRequested?.Invoke(this, e);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }

    public event EventHandler? ApproveRequested;
    public event EventHandler? ReturnRequested;
    public void Bind(WaybillResponse waybill, WaybillValidationResponse validation) => Content.Bind(waybill, validation);
}
