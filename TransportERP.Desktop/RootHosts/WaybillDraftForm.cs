using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

/// <summary>
/// P2-C01-A transaction shell. It has no database reference; callers bind it to an HTTP/API client.
/// SHP-005/006/007/008 are represented as governed transaction tabs.
/// </summary>
 
/// <summary>Compatibility window hosting the reusable business surface.</summary>
public sealed partial class WaybillDraftForm : Form
{
    public UcWaybillDraft Content { get; } = new()
    {
        Dock = DockStyle.Fill
    };

    public WaybillDraftForm()
    {
        Text = "البوليصة — SHP-005";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1000, 680);
        Controls.Add(Content);
        Content.NewRequested += (_, e) => NewRequested?.Invoke(this, e);
        Content.SaveRequested += (_, e) => SaveRequested?.Invoke(this, e);
        Content.SubmitRequested += (_, e) => SubmitRequested?.Invoke(this, e);
        Content.CancelRequested += (_, e) => CancelRequested?.Invoke(this, e);
        Content.CloseRequested += (_, _) => Close();
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }

    public event EventHandler? NewRequested;
    public event EventHandler? SaveRequested;
    public event EventHandler? SubmitRequested;
    public event EventHandler? CancelRequested;
    public void Bind(WaybillResponse value) => Content.Bind(value);
    public void SetValidation(IReadOnlyList<string> errors) => Content.SetValidation(errors);
}
