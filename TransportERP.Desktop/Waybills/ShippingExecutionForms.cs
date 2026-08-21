using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

public static class ShippingExecutionScreenCatalog
{
    public static readonly WaybillScreenDefinition Release = new("SHP-015", "إطلاق كميات الأصناف", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Allocation = new("SHP-016", "توزيع الأصناف على الرحلات", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Remaining = new("SHP-019", "المتبقي غير المرحل", TransportScreenProfile.ReportInquiry);
    public static readonly WaybillScreenDefinition ReadyToLoad = new("SHP-023", "البوالص الجاهزة للتحميل", TransportScreenProfile.ReportInquiry);
    public static readonly WaybillScreenDefinition LoadPlanning = new("SHP-024", "تخطيط الحمولة", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Trip = new("SHP-025", "إنشاء الرحلة", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Loading = new("SHP-027", "تحميل الرحلة", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Manifest = new("SHP-028", "كشف الحمولة", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Handover = new("SHP-029", "تسليم عهدة الحمولة للسائق", TransportScreenProfile.ControlApproval);
    public static readonly WaybillScreenDefinition Departure = new("SHP-030", "انطلاق الرحلة", TransportScreenProfile.ControlApproval);
}

public abstract class ShippingRtlForm : Form
{
    protected ShippingRtlForm(string title, int width = 900, int height = 520)
    {
        Text = title;
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(width, height);
    }

    protected static DataGridView ReadOnlyGrid() => new()
    {
        Dock = DockStyle.Fill,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        ReadOnly = true,
        AutoGenerateColumns = true,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false,
        RightToLeft = RightToLeft.Yes
    };

    protected static Button ActionButton(string text, EventHandler handler)
    {
        var button = new Button { Text = text, AutoSize = true };
        button.Click += handler;
        return button;
    }
}

/// <summary>SHP-015 — release cannot exceed server-derived RemainingToRelease.</summary>
public sealed class ItemReleaseForm : ShippingRtlForm
{
    private readonly TextBox _original = Box();
    private readonly TextBox _released = Box();
    private readonly TextBox _remaining = Box();

    public ItemReleaseForm() : base("إطلاق كميات الأصناف — SHP-015", 720, 420)
    {
        var table = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(16) };
        Add(table, 0, "الكمية الأصلية", _original);
        Add(table, 1, "المطلق", _released);
        Add(table, 2, "المتبقي للإطلاق", _remaining);
        var action = ActionButton("إطلاق كمية", (_, _) => ReleaseRequested?.Invoke(this, EventArgs.Empty));
        Controls.Add(table);
        Controls.Add(new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 52, FlowDirection = FlowDirection.RightToLeft, Controls = { action } });
    }

    public event EventHandler? ReleaseRequested;

    public void Bind(ItemQuantityStateResponse value)
    {
        _original.Text = value.OriginalQuantity.ToString("N3");
        _released.Text = value.ReleasedNet.ToString("N3");
        _remaining.Text = value.RemainingToRelease.ToString("N3");
    }

    private static TextBox Box() => new() { ReadOnly = true, Width = 260 };
    private static void Add(TableLayoutPanel table, int row, string label, Control control)
    {
        table.RowCount = Math.Max(table.RowCount, row + 1);
        table.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Right }, 0, row);
        table.Controls.Add(control, 1, row);
    }
}

/// <summary>SHP-016 — allocation is governed by released remaining quantity.</summary>
public sealed class TripAllocationForm : ShippingRtlForm
{
    private readonly DataGridView _rows = ReadOnlyGrid();
    public TripAllocationForm() : base("توزيع الأصناف على الرحلات — SHP-016")
    {
        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 52, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
        top.Controls.Add(ActionButton("تخصيص كمية", (_, _) => AllocateRequested?.Invoke(this, EventArgs.Empty)));
        top.Controls.Add(ActionButton("فك التخصيص المحدد", (_, _) => UnallocateRequested?.Invoke(this, EventArgs.Empty)));
        Controls.Add(_rows); Controls.Add(top);
    }
    public event EventHandler? AllocateRequested;
    public event EventHandler? UnallocateRequested;
    public void Bind(IReadOnlyList<AllocationResponse> rows) => _rows.DataSource = rows.ToList();
    public AllocationResponse? Selected => _rows.CurrentRow?.DataBoundItem as AllocationResponse;
}

/// <summary>SHP-019 — read-only operational balance projection.</summary>
public sealed class RemainingShippingQuantityForm : ShippingRtlForm
{
    private readonly DataGridView _rows = ReadOnlyGrid();
    public RemainingShippingQuantityForm() : base("المتبقي غير المرحل — SHP-019") => Controls.Add(_rows);
    public void Bind(IEnumerable<object> rows) => _rows.DataSource = rows.ToList();
}

/// <summary>SHP-023 — selection surface only; no direct database access.</summary>
public sealed class ReadyToLoadWaybillsForm : ShippingRtlForm
{
    private readonly DataGridView _rows = ReadOnlyGrid();
    public ReadyToLoadWaybillsForm() : base("البوالص الجاهزة للتحميل — SHP-023")
    {
        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 52, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
        top.Controls.Add(ActionButton("إنشاء كشف حمولة", (_, _) => GenerateManifestRequested?.Invoke(this, EventArgs.Empty)));
        Controls.Add(_rows); Controls.Add(top);
    }
    public event EventHandler? GenerateManifestRequested;
    public void Bind(IEnumerable<object> rows) => _rows.DataSource = rows.ToList();
}

/// <summary>SHP-024 — planning shell with explicit allocation/capacity information.</summary>
public sealed class LoadPlanningForm : ShippingRtlForm
{
    private readonly DataGridView _rows = ReadOnlyGrid();
    public LoadPlanningForm() : base("تخطيط الحمولة — SHP-024") => Controls.Add(_rows);
    public void Bind(IEnumerable<object> rows) => _rows.DataSource = rows.ToList();
}

/// <summary>SHP-025 — Trip response binding; vehicle/driver remain external references until Fleet implementation.</summary>
public sealed class TripForm : ShippingRtlForm
{
    private readonly TextBox _tripNo = Box();
    private readonly TextBox _driver = Box();
    private readonly TextBox _vehicle = Box();
    private readonly TextBox _status = Box();
    private readonly DataGridView _stops = ReadOnlyGrid();

    public TripForm() : base("إنشاء الرحلة — SHP-025", 880, 560)
    {
        var table = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(12) };
        Add(table, 0, "رقم الرحلة", _tripNo); Add(table, 1, "السائق", _driver); Add(table, 2, "المركبة", _vehicle); Add(table, 3, "الحالة", _status);
        _stops.Dock = DockStyle.Fill;
        Controls.Add(_stops); Controls.Add(table);
    }

    public void Bind(TripResponse trip)
    {
        _tripNo.Text = trip.TripNo; _driver.Text = trip.DriverId.ToString(); _vehicle.Text = trip.VehicleId.ToString(); _status.Text = trip.Status;
        _stops.DataSource = trip.Stops.ToList();
    }

    private static TextBox Box() => new() { ReadOnly = true, Width = 320 };
    private static void Add(TableLayoutPanel table, int row, string label, Control control)
    {
        table.RowCount = Math.Max(table.RowCount, row + 1);
        table.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Right }, 0, row);
        table.Controls.Add(control, 1, row);
    }
}

/// <summary>SHP-027 — server-accepted load command; loaded quantity never exceeds manifest allocation.</summary>
public sealed class ManifestLoadingForm : ShippingRtlForm
{
    private readonly DataGridView _lines = ReadOnlyGrid();
    public ManifestLoadingForm() : base("تحميل الرحلة — SHP-027")
    {
        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 52, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
        top.Controls.Add(ActionButton("تسجيل تحميل", (_, _) => LoadRequested?.Invoke(this, EventArgs.Empty)));
        Controls.Add(_lines); Controls.Add(top);
    }
    public event EventHandler? LoadRequested;
    public void Bind(ManifestResponse manifest) => _lines.DataSource = manifest.Lines.ToList();
    public ManifestLineResponse? SelectedLine => _lines.CurrentRow?.DataBoundItem as ManifestLineResponse;
}

/// <summary>SHP-028 — manifest totals and state; finalize is a governed server action.</summary>
public sealed class ManifestForm : ShippingRtlForm
{
    private readonly Label _header = new() { AutoSize = true };
    private readonly DataGridView _lines = ReadOnlyGrid();
    public ManifestForm() : base("كشف الحمولة Manifest — SHP-028", 980, 600)
    {
        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 56, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
        top.Controls.Add(ActionButton("اعتماد كشف الحمولة", (_, _) => FinalizeRequested?.Invoke(this, EventArgs.Empty)));
        top.Controls.Add(_header);
        Controls.Add(_lines); Controls.Add(top);
    }
    public event EventHandler? FinalizeRequested;
    public void Bind(ManifestResponse manifest)
    {
        _header.Text = $"{manifest.ManifestNo} — {manifest.Status} — إصدار {manifest.Version}";
        _lines.DataSource = manifest.Lines.ToList();
    }
}

/// <summary>SHP-029 — custody begins only after assigned driver accepts the finalized manifest.</summary>
public sealed class ManifestHandoverForm : ShippingRtlForm
{
    private readonly Label _summary = new() { AutoSize = true };
    public ManifestHandoverForm() : base("تسليم عهدة الحمولة للسائق — SHP-029", 760, 380)
    {
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(20), WrapContents = false };
        panel.Controls.Add(_summary);
        panel.Controls.Add(ActionButton("تأكيد استلام السائق", (_, _) => AcceptRequested?.Invoke(this, EventArgs.Empty)));
        Controls.Add(panel);
    }
    public event EventHandler? AcceptRequested;
    public void Bind(ManifestResponse manifest) => _summary.Text = $"الكشف: {manifest.ManifestNo} — الحالة: {manifest.Status} — عدد السطور: {manifest.Lines.Count}";
}

/// <summary>SHP-030 — departure command is operational only and does not create revenue.</summary>
public sealed class TripDepartureForm : ShippingRtlForm
{
    private readonly Label _summary = new() { AutoSize = true };
    public TripDepartureForm() : base("انطلاق الرحلة — SHP-030", 760, 380)
    {
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(20), WrapContents = false };
        panel.Controls.Add(_summary);
        panel.Controls.Add(ActionButton("تأكيد انطلاق الرحلة", (_, _) => StartRequested?.Invoke(this, EventArgs.Empty)));
        Controls.Add(panel);
    }
    public event EventHandler? StartRequested;
    public void Bind(TripResponse trip) => _summary.Text = $"الرحلة: {trip.TripNo} — الحالة: {trip.Status} — السائق: {trip.DriverId}";
}
