using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

/// <summary>SHP-011 — accepted collection rows are immutable; reversal is a separate governed action.</summary>
 
/// <summary>Compatibility window hosting the reusable business surface.</summary>
public sealed partial class WaybillCollectionsForm : Form
{
    public UcWaybillCollections Content { get; } = new()
    {
        Dock = DockStyle.Fill
    };

    public WaybillCollectionsForm()
    {
        Text = "تحصيلات البوليصة — SHP-011";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        MinimumSize = new Size(980, 560);
        Controls.Add(Content);
        Content.RecordRequested += (_, e) => RecordRequested?.Invoke(this, e);
        Content.ReverseRequested += (_, e) => ReverseRequested?.Invoke(this, e);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }

    public event EventHandler? RecordRequested;
    public event EventHandler? ReverseRequested;
    public void Bind(IReadOnlyList<CollectionResponse> items) => Content.Bind(items);
    public CollectionResponse? SelectedCollection => Content.SelectedCollection;
}
