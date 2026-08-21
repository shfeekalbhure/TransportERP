using System.Drawing.Printing;
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

    protected static TextBox InfoBox(int width = 280) => new() { ReadOnly = true, Width = width };
    protected static TextBox InputBox(int width = 280) => new() { Width = width };
    protected static NumericUpDown QuantityInput() => new()
    {
        Width = 180,
        DecimalPlaces = 3,
        Minimum = 0m,
        Maximum = 1_000_000_000m,
        ThousandsSeparator = true
    };

    protected static void AddRow(TableLayoutPanel table, int row, string label, Control control)
    {
        table.RowCount = Math.Max(table.RowCount, row + 1);
        table.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Right }, 0, row);
        table.Controls.Add(control, 1, row);
    }

    protected static string OperationId(string action) => $"{action}-{Guid.NewGuid():N}";

    protected static string RiskText(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Trim() is "[]" or "{}")
            return "لا توجد مخاطر مسجلة";
        return value.Trim();
    }
}

/// <summary>SHP-015 — W3 release identities, quantities and hold state.</summary>
public sealed class ItemReleaseForm : ShippingRtlForm
{
    private ItemReleaseScreenState? _state;
    private readonly TextBox _waybill = InfoBox();
    private readonly TextBox _item = InfoBox();
    private readonly TextBox _original = InfoBox();
    private readonly TextBox _released = InfoBox();
    private readonly TextBox _remaining = InfoBox();
    private readonly TextBox _holdStatus = InfoBox();
    private readonly NumericUpDown _releaseQty = QuantityInput();
    private readonly Label _message = new() { AutoSize = true };

    public ItemReleaseForm() : base("إطلاق كميات الأصناف — SHP-015", 760, 520)
    {
        var table = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(16) };
        AddRow(table, 0, "البوليصة", _waybill);
        AddRow(table, 1, "الصنف", _item);
        AddRow(table, 2, "الكمية الأصلية", _original);
        AddRow(table, 3, "المطلق", _released);
        AddRow(table, 4, "المتبقي للإطلاق", _remaining);
        AddRow(table, 5, "حالة الحجز", _holdStatus);
        AddRow(table, 6, "كمية الإطلاق", _releaseQty);

        var action = ActionButton("إطلاق كمية", (_, _) => RequestRelease());
        var bottom = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 58,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(8),
            Controls = { action, _message }
        };
        Controls.Add(table);
        Controls.Add(bottom);
    }

    public event Action<Guid, Guid, ReleaseItemRequest>? ReleaseRequested;

    public void Bind(ItemReleaseScreenState state)
    {
        _state = state;
        _waybill.Text = state.Waybill;
        _item.Text = state.Item;
        _original.Text = state.Quantity.OriginalQuantity.ToString("N3");
        _released.Text = state.Quantity.ReleasedNet.ToString("N3");
        _remaining.Text = state.Quantity.RemainingToRelease.ToString("N3");
        _holdStatus.Text = string.IsNullOrWhiteSpace(state.HoldStatus) ? "لا يوجد حجز نشط" : state.HoldStatus;
        _releaseQty.Maximum = Math.Max(0m, state.Quantity.RemainingToRelease);
        _releaseQty.Value = 0m;
        _message.Text = "";
    }

    public void Bind(ItemQuantityStateResponse value)
        => Bind(new ItemReleaseScreenState(
            value.WaybillId.ToString(),
            value.ItemId.ToString(),
            value,
            "حالة الحجز غير محملة"));

    private void RequestRelease()
    {
        if (_state is null)
        {
            _message.Text = "حمّل بيانات البوليصة والصنف أولاً.";
            return;
        }

        var qty = _releaseQty.Value;
        if (qty <= 0m || qty > _state.Quantity.RemainingToRelease)
        {
            _message.Text = "كمية الإطلاق يجب أن تكون موجبة ولا تتجاوز المتبقي.";
            return;
        }

        _message.Text = "سيتم التحقق من الحجز والمتبقي نهائياً في الخادم.";
        ReleaseRequested?.Invoke(
            _state.Quantity.WaybillId,
            _state.Quantity.ItemId,
            new ReleaseItemRequest(qty, DateTimeOffset.UtcNow, OperationId("desktop-release")));
    }
}

/// <summary>SHP-016 — W3 allocation planning with released remaining, route and quantity input.</summary>
public sealed class TripAllocationForm : ShippingRtlForm
{
    private readonly DataGridView _rows = ReadOnlyGrid();
    private readonly NumericUpDown _allocateQty = QuantityInput();
    private readonly TextBox _reason = InputBox(240);
    private readonly Label _message = new() { AutoSize = true };

    public TripAllocationForm() : base("توزيع الأصناف على الرحلات — SHP-016", 980, 600)
    {
        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 72, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
        top.Controls.Add(ActionButton("تخصيص كمية", (_, _) => RequestAllocate()));
        top.Controls.Add(new Label { Text = "كمية التخصيص", AutoSize = true, Padding = new Padding(4, 8, 4, 0) });
        top.Controls.Add(_allocateQty);
        top.Controls.Add(ActionButton("فك التخصيص المحدد", (_, _) => RequestUnallocate()));
        top.Controls.Add(new Label { Text = "سبب الفك", AutoSize = true, Padding = new Padding(4, 8, 4, 0) });
        top.Controls.Add(_reason);

        var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 42, FlowDirection = FlowDirection.RightToLeft, Controls = { _message } };
        _rows.SelectionChanged += (_, _) => SyncSelected();
        Controls.Add(_rows);
        Controls.Add(bottom);
        Controls.Add(top);
    }

    public event Action<Guid, AllocateItemRequest>? AllocateRequested;
    public event Action<Guid, UnallocateRequest>? UnallocateRequested;

    public void Bind(IReadOnlyList<TripAllocationPlanningRow> rows)
    {
        _rows.DataSource = rows.ToList();
        SyncSelected();
    }

    public TripAllocationPlanningRow? Selected => _rows.CurrentRow?.DataBoundItem as TripAllocationPlanningRow;

    private void SyncSelected()
    {
        var selected = Selected;
        if (selected is null)
        {
            _allocateQty.Maximum = 0m;
            _allocateQty.Value = 0m;
            return;
        }

        _allocateQty.Maximum = Math.Max(0m, selected.ReleasedRemaining);
        if (_allocateQty.Value > _allocateQty.Maximum)
            _allocateQty.Value = _allocateQty.Maximum;
        _message.Text = $"المسار: {selected.Route} — الحالة: {selected.AllocationStatus}";
    }

    private void RequestAllocate()
    {
        var selected = Selected;
        if (selected is null || _allocateQty.Value <= 0m)
        {
            _message.Text = "اختر سطراً وأدخل كمية تخصيص موجبة.";
            return;
        }

        AllocateRequested?.Invoke(
            selected.TripId,
            new AllocateItemRequest(
                selected.WaybillItemId,
                selected.ReleaseId,
                _allocateQty.Value,
                OperationId("desktop-allocate")));
    }

    private void RequestUnallocate()
    {
        var selected = Selected;
        if (selected?.AllocationId is not Guid allocationId)
        {
            _message.Text = "السطر المحدد لا يمثل تخصيصاً قابلاً للفك.";
            return;
        }

        if (string.IsNullOrWhiteSpace(_reason.Text))
        {
            _message.Text = "سبب فك التخصيص مطلوب.";
            return;
        }

        UnallocateRequested?.Invoke(
            allocationId,
            new UnallocateRequest(_reason.Text.Trim(), OperationId("desktop-unallocate")));
    }
}

/// <summary>SHP-019 — typed read-only operational balance projection.</summary>
public sealed class RemainingShippingQuantityForm : ShippingRtlForm
{
    private readonly DataGridView _rows = ReadOnlyGrid();

    public RemainingShippingQuantityForm() : base("المتبقي غير المرحل — SHP-019", 1050, 560)
        => Controls.Add(_rows);

    public void Bind(IReadOnlyList<RemainingShippingRow> rows) => _rows.DataSource = rows.ToList();
}

/// <summary>SHP-023 — typed ready-to-load selection with text-first priority/risk information.</summary>
public sealed class ReadyToLoadWaybillsForm : ShippingRtlForm
{
    private readonly DataGridView _rows = ReadOnlyGrid();
    private readonly Label _message = new() { AutoSize = true };

    public ReadyToLoadWaybillsForm() : base("البوالص الجاهزة للتحميل — SHP-023", 980, 560)
    {
        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 52, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
        top.Controls.Add(ActionButton("إنشاء كشف حمولة", (_, _) => RequestManifest()));
        top.Controls.Add(_message);
        _rows.SelectionChanged += (_, _) => SyncSelected();
        Controls.Add(_rows);
        Controls.Add(top);
    }

    public event Action<Guid, GenerateManifestRequest>? GenerateManifestRequested;

    public void Bind(IReadOnlyList<ReadyToLoadRow> rows)
    {
        _rows.DataSource = rows.ToList();
        SyncSelected();
    }

    private ReadyToLoadRow? Selected => _rows.CurrentRow?.DataBoundItem as ReadyToLoadRow;

    private void SyncSelected()
    {
        var selected = Selected;
        _message.Text = selected is null
            ? "لا يوجد سطر محدد."
            : $"الأولوية: {selected.Priority} — المخاطر: {RiskText(selected.RiskFlags)}";
    }

    private void RequestManifest()
    {
        var selected = Selected;
        if (selected is null) return;
        GenerateManifestRequested?.Invoke(
            selected.TripId,
            new GenerateManifestRequest(null, OperationId("desktop-manifest")));
    }
}

/// <summary>SHP-024 — typed capacity/allocation plan. Weight and volume are server-proportional allocation measures.</summary>
public sealed class LoadPlanningForm : ShippingRtlForm
{
    private readonly DataGridView _rows = ReadOnlyGrid();
    private readonly NumericUpDown _allocateQty = QuantityInput();
    private readonly Label _capacity = new() { AutoSize = true };

    public LoadPlanningForm() : base("تخطيط الحمولة — SHP-024", 1120, 620)
    {
        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 72, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
        top.Controls.Add(ActionButton("إنشاء كشف حمولة", (_, _) => RequestManifest()));
        top.Controls.Add(ActionButton("تخصيص كمية إضافية", (_, _) => RequestAllocate()));
        top.Controls.Add(new Label { Text = "كمية التخصيص", AutoSize = true, Padding = new Padding(4, 8, 4, 0) });
        top.Controls.Add(_allocateQty);

        var bottom = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 52,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(8),
            Controls = { _capacity }
        };

        _rows.SelectionChanged += (_, _) => SyncSelected();
        Controls.Add(_rows);
        Controls.Add(bottom);
        Controls.Add(top);
    }

    public event Action<Guid, AllocateItemRequest>? AllocateRequested;
    public event Action<Guid, GenerateManifestRequest>? GenerateManifestRequested;

    public void Bind(IReadOnlyList<LoadPlanningRow> rows)
    {
        _rows.DataSource = rows.ToList();
        SyncSelected();
    }

    public LoadPlanningRow? Selected => _rows.CurrentRow?.DataBoundItem as LoadPlanningRow;

    private void SyncSelected()
    {
        var selected = Selected;
        if (selected is null)
        {
            _capacity.Text = "لا توجد معلومات سعة/مخاطر محددة.";
            _allocateQty.Maximum = 0m;
            _allocateQty.Value = 0m;
            return;
        }

        _allocateQty.Maximum = Math.Max(0m, selected.ReleasedRemaining);
        if (_allocateQty.Value > _allocateQty.Maximum)
            _allocateQty.Value = _allocateQty.Maximum;

        _capacity.Text =
            $"السعة: {selected.Capacity} — الوزن المخصص: {selected.AllocatedWeight:N3}/{CapacityValue(selected.CapacityWeight)}" +
            $" — الحجم المخصص: {selected.AllocatedVolume:N3}/{CapacityValue(selected.CapacityVolume)}" +
            $" — {selected.CapacityStatus} — الأولوية: {selected.Priority} — المخاطر: {RiskText(selected.RiskFlags)}";
    }

    private static string CapacityValue(decimal value) => value > 0m ? value.ToString("N3") : "غير محددة";

    private void RequestAllocate()
    {
        var selected = Selected;
        if (selected is null || _allocateQty.Value <= 0m) return;
        AllocateRequested?.Invoke(
            selected.TripId,
            new AllocateItemRequest(
                selected.WaybillItemId,
                selected.ReleaseId,
                _allocateQty.Value,
                OperationId("desktop-plan-allocate")));
    }

    private void RequestManifest()
    {
        var selected = Selected;
        if (selected is null) return;
        GenerateManifestRequested?.Invoke(
            selected.TripId,
            new GenerateManifestRequest(null, OperationId("desktop-plan-manifest")));
    }
}

/// <summary>SHP-025 — create-trip input surface using the existing C CreateTripRequest contract.</summary>
public sealed class TripForm : ShippingRtlForm
{
    private readonly TextBox _tripNo = InputBox(320);
    private readonly TextBox _driver = InputBox(320);
    private readonly TextBox _vehicle = InputBox(320);
    private readonly TextBox _origin = InputBox(320);
    private readonly TextBox _destination = InputBox(320);
    private readonly DateTimePicker _plannedDepartAt = new() { Width = 320, Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd HH:mm" };
    private readonly TextBox _status = InfoBox(320);
    private readonly DataGridView _stops = ReadOnlyGrid();
    private readonly Label _message = new() { AutoSize = true };
    private IReadOnlyList<TripStopInput> _plannedStops = [];

    public TripForm() : base("إنشاء الرحلة — SHP-025", 920, 680)
    {
        var table = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(12) };
        AddRow(table, 0, "رقم الرحلة", _tripNo);
        AddRow(table, 1, "السائق", _driver);
        AddRow(table, 2, "المركبة", _vehicle);
        AddRow(table, 3, "المنشأ", _origin);
        AddRow(table, 4, "الوجهة", _destination);
        AddRow(table, 5, "الانطلاق المخطط", _plannedDepartAt);
        AddRow(table, 6, "الحالة", _status);

        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 52, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
        top.Controls.Add(ActionButton("إنشاء الرحلة", (_, _) => RequestCreate()));
        top.Controls.Add(_message);

        Controls.Add(_stops);
        Controls.Add(table);
        Controls.Add(top);
        _status.Text = "مسودة جديدة";
    }

    public event Action<CreateTripRequest>? CreateTripRequested;

    public void SetPlannedStops(IReadOnlyList<TripStopInput> stops)
    {
        _plannedStops = stops;
        _stops.DataSource = stops.ToList();
    }

    public void SetReferences(Guid vehicleId, Guid driverId, Guid originId, Guid destinationId)
    {
        _vehicle.Text = vehicleId.ToString();
        _driver.Text = driverId.ToString();
        _origin.Text = originId.ToString();
        _destination.Text = destinationId.ToString();
    }

    public void Bind(TripResponse trip)
    {
        _tripNo.Text = trip.TripNo;
        _driver.Text = trip.DriverId.ToString();
        _vehicle.Text = trip.VehicleId.ToString();
        _origin.Text = trip.OriginId.ToString();
        _destination.Text = trip.DestinationId.ToString();
        _plannedDepartAt.Value = trip.PlannedDepartAt.LocalDateTime;
        _status.Text = trip.Status;
        _stops.DataSource = trip.Stops.ToList();
    }

    private void RequestCreate()
    {
        if (string.IsNullOrWhiteSpace(_tripNo.Text) ||
            !Guid.TryParse(_vehicle.Text, out var vehicleId) ||
            !Guid.TryParse(_driver.Text, out var driverId) ||
            !Guid.TryParse(_origin.Text, out var originId) ||
            !Guid.TryParse(_destination.Text, out var destinationId))
        {
            _message.Text = "رقم الرحلة ومراجع السائق والمركبة والمنشأ والوجهة مطلوبة.";
            return;
        }

        _message.Text = "سيتم التحقق من المسار والمراجع في الخادم.";
        CreateTripRequested?.Invoke(new CreateTripRequest(
            _tripNo.Text.Trim(),
            vehicleId,
            driverId,
            originId,
            destinationId,
            new DateTimeOffset(_plannedDepartAt.Value),
            _plannedStops,
            OperationId("desktop-trip-create")));
    }
}

/// <summary>SHP-027 — load quantity input with explicit resource-risk confirmation.</summary>
public sealed class ManifestLoadingForm : ShippingRtlForm
{
    private ManifestResponse? _manifest;
    private readonly DataGridView _lines = ReadOnlyGrid();
    private readonly NumericUpDown _loadQty = QuantityInput();
    private readonly CheckBox _resourceConfirmed = new() { Text = "تمت مراجعة قيد السعة/المخاطر", AutoSize = true };
    private readonly Label _risk = new() { AutoSize = true };

    public ManifestLoadingForm() : base("تحميل الرحلة — SHP-027", 1000, 600)
    {
        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 72, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
        top.Controls.Add(ActionButton("تسجيل تحميل", (_, _) => RequestLoad()));
        top.Controls.Add(new Label { Text = "كمية التحميل", AutoSize = true, Padding = new Padding(4, 8, 4, 0) });
        top.Controls.Add(_loadQty);
        top.Controls.Add(_resourceConfirmed);

        var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 48, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8), Controls = { _risk } };
        _lines.SelectionChanged += (_, _) => SyncSelected();

        Controls.Add(_lines);
        Controls.Add(bottom);
        Controls.Add(top);
    }

    public event Action<Guid, Guid, LoadManifestLineRequest>? LoadRequested;

    public void Bind(ManifestResponse manifest, IReadOnlyList<ManifestLoadingRow> rows)
    {
        _manifest = manifest;
        _lines.DataSource = rows.ToList();
        SyncSelected();
    }

    public void Bind(ManifestResponse manifest)
        => Bind(
            manifest,
            manifest.Lines.Select(x => ManifestLoadingRow.FromManifestLine(
                x,
                x.WaybillItemId.ToString(),
                "بيانات المخاطر غير محملة")).ToList());

    public ManifestLoadingRow? SelectedLine => _lines.CurrentRow?.DataBoundItem as ManifestLoadingRow;

    private void SyncSelected()
    {
        var line = SelectedLine;
        if (line is null)
        {
            _loadQty.Maximum = 0m;
            _loadQty.Value = 0m;
            _risk.Text = "لا يوجد سطر محدد.";
            return;
        }

        var remaining = Math.Max(0m, line.AllocatedQty - line.LoadedQty);
        _loadQty.Maximum = remaining;
        if (_loadQty.Value > remaining)
            _loadQty.Value = remaining;
        _risk.Text = $"المخاطر: {RiskText(line.RiskFlags)} — حالة الخادم: {line.Status}";
    }

    private void RequestLoad()
    {
        var line = SelectedLine;
        if (_manifest is null || line is null || _loadQty.Value <= 0m) return;

        LoadRequested?.Invoke(
            _manifest.Id,
            line.ManifestLineId,
            new LoadManifestLineRequest(
                _loadQty.Value,
                DateTimeOffset.UtcNow,
                _resourceConfirmed.Checked,
                OperationId("desktop-load")));
    }
}

/// <summary>SHP-028 — manifest, driver/vehicle, proportional physical totals and printable RTL representation.</summary>
public sealed class ManifestForm : ShippingRtlForm
{
    private Guid _manifestId;
    private long _manifestVersion;
    private readonly Label _header = new() { AutoSize = true };
    private readonly TextBox _driver = InfoBox();
    private readonly TextBox _vehicle = InfoBox();
    private readonly TextBox _totalQty = InfoBox();
    private readonly TextBox _totalWeight = InfoBox();
    private readonly TextBox _totalVolume = InfoBox();
    private readonly TextBox _status = InfoBox();
    private readonly DataGridView _lines = ReadOnlyGrid();
    private readonly PrintDocument _printDocument = new();

    public ManifestForm() : base("كشف الحمولة Manifest — SHP-028", 1040, 700)
    {
        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 56, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
        top.Controls.Add(ActionButton("اعتماد كشف الحمولة", (_, _) => RequestFinalize()));
        top.Controls.Add(ActionButton("معاينة الطباعة RTL", (_, _) => ShowPrintPreview()));
        top.Controls.Add(_header);

        var summary = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(10) };
        AddRow(summary, 0, "السائق", _driver);
        AddRow(summary, 1, "المركبة", _vehicle);
        AddRow(summary, 2, "إجمالي الكمية", _totalQty);
        AddRow(summary, 3, "إجمالي الوزن", _totalWeight);
        AddRow(summary, 4, "إجمالي الحجم", _totalVolume);
        AddRow(summary, 5, "الحالة", _status);

        _printDocument.PrintPage += PrintPage;
        Controls.Add(_lines);
        Controls.Add(summary);
        Controls.Add(top);
    }

    public event Action<Guid, FinalizeManifestRequest>? FinalizeRequested;

    public void Bind(ManifestScreenState state)
    {
        _manifestId = state.Manifest.Id;
        _manifestVersion = state.Manifest.Version;
        _header.Text = $"{state.Manifest.ManifestNo} — إصدار {state.Manifest.Version}";
        _driver.Text = state.Trip.DriverId.ToString();
        _vehicle.Text = state.Trip.VehicleId.ToString();
        _totalQty.Text = state.TotalQuantity.ToString("N3");
        _totalWeight.Text = state.TotalWeight.ToString("N3");
        _totalVolume.Text = state.TotalVolume.ToString("N3");
        _status.Text = state.Manifest.Status;
        _lines.DataSource = state.Manifest.Lines.ToList();
    }

    public void Bind(ManifestResponse manifest)
    {
        _manifestId = manifest.Id;
        _manifestVersion = manifest.Version;
        _header.Text = $"{manifest.ManifestNo} — إصدار {manifest.Version}";
        _driver.Text = "بيانات السائق غير محملة";
        _vehicle.Text = "بيانات المركبة غير محملة";
        _totalQty.Text = manifest.Lines.Sum(x => x.Quantity).ToString("N3");
        _totalWeight.Text = manifest.Lines.Sum(x => x.Weight).ToString("N3");
        _totalVolume.Text = manifest.Lines.Sum(x => x.Volume).ToString("N3");
        _status.Text = manifest.Status;
        _lines.DataSource = manifest.Lines.ToList();
    }

    private void RequestFinalize()
    {
        if (_manifestId == Guid.Empty || _manifestVersion < 1) return;
        FinalizeRequested?.Invoke(
            _manifestId,
            new FinalizeManifestRequest(_manifestVersion, OperationId("desktop-manifest-finalize")));
    }

    private void ShowPrintPreview()
    {
        using var preview = new PrintPreviewDialog
        {
            Document = _printDocument,
            Width = 1000,
            Height = 720,
            RightToLeft = RightToLeft.Yes
        };
        preview.ShowDialog(this);
    }

    private void PrintPage(object? sender, PrintPageEventArgs e)
    {
        using var titleFont = new Font("Segoe UI", 14, FontStyle.Bold);
        using var bodyFont = new Font("Segoe UI", 10);
        using var format = new StringFormat { Alignment = StringAlignment.Far, FormatFlags = StringFormatFlags.DirectionRightToLeft };

        var right = e.MarginBounds.Right;
        var y = e.MarginBounds.Top;
        e.Graphics.DrawString($"كشف الحمولة — {_header.Text}", titleFont, Brushes.Black, right, y, format);
        y += 40;
        foreach (var line in new[]
                 {
                     $"السائق: {_driver.Text}",
                     $"المركبة: {_vehicle.Text}",
                     $"إجمالي الكمية: {_totalQty.Text}",
                     $"إجمالي الوزن: {_totalWeight.Text}",
                     $"إجمالي الحجم: {_totalVolume.Text}",
                     $"الحالة: {_status.Text}"
                 })
        {
            e.Graphics.DrawString(line, bodyFont, Brushes.Black, right, y, format);
            y += 28;
        }
    }
}

/// <summary>SHP-029 — custody summary with driver/vehicle/totals/acceptance time and governed handover request.</summary>
public sealed class ManifestHandoverForm : ShippingRtlForm
{
    private ManifestScreenState? _state;
    private readonly Label _summary = new() { AutoSize = true, MaximumSize = new Size(680, 0) };

    public ManifestHandoverForm() : base("تسليم عهدة الحمولة للسائق — SHP-029", 800, 460)
    {
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(20), WrapContents = false };
        panel.Controls.Add(_summary);
        panel.Controls.Add(ActionButton("تأكيد استلام السائق", (_, _) => RequestAccept()));
        Controls.Add(panel);
    }

    public event Action<Guid, HandoverManifestRequest>? AcceptRequested;

    public void Bind(ManifestScreenState state)
    {
        _state = state;
        var accepted = state.Manifest.DriverAcceptedAt?.ToLocalTime().ToString("yyyy-MM-dd HH:mm") ?? "لم يتم القبول بعد";
        _summary.Text =
            $"الكشف: {state.Manifest.ManifestNo}\r\n" +
            $"السائق: {state.Trip.DriverId}\r\n" +
            $"المركبة: {state.Trip.VehicleId}\r\n" +
            $"إجمالي الكمية: {state.TotalQuantity:N3}\r\n" +
            $"إجمالي الوزن: {state.TotalWeight:N3}\r\n" +
            $"إجمالي الحجم: {state.TotalVolume:N3}\r\n" +
            $"الحالة: {state.Manifest.Status}\r\n" +
            $"وقت القبول: {accepted}";
    }

    private void RequestAccept()
    {
        if (_state is null) return;
        AcceptRequested?.Invoke(
            _state.Manifest.Id,
            new HandoverManifestRequest(
                _state.Trip.DriverId,
                DateTimeOffset.UtcNow,
                _state.Manifest.Version,
                OperationId("desktop-handover")));
    }
}

/// <summary>SHP-030 — departure confirmation with trip/manifest/driver/vehicle/planned/actual/status.</summary>
public sealed class TripDepartureForm : ShippingRtlForm
{
    private DepartureScreenState? _state;
    private readonly Label _summary = new() { AutoSize = true, MaximumSize = new Size(680, 0) };

    public TripDepartureForm() : base("انطلاق الرحلة — SHP-030", 800, 460)
    {
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(20), WrapContents = false };
        panel.Controls.Add(_summary);
        panel.Controls.Add(ActionButton("تأكيد انطلاق الرحلة", (_, _) => RequestStart()));
        Controls.Add(panel);
    }

    public event Action<Guid, StartTripRequest>? StartRequested;

    public void Bind(DepartureScreenState state)
    {
        _state = state;
        var actual = state.Trip.ActualDepartAt?.ToLocalTime().ToString("yyyy-MM-dd HH:mm") ?? "لم تنطلق بعد";
        _summary.Text =
            $"الرحلة: {state.Trip.TripNo}\r\n" +
            $"الكشف: {state.Manifest.ManifestNo}\r\n" +
            $"السائق: {state.Trip.DriverId}\r\n" +
            $"المركبة: {state.Trip.VehicleId}\r\n" +
            $"الانطلاق المخطط: {state.Trip.PlannedDepartAt.ToLocalTime():yyyy-MM-dd HH:mm}\r\n" +
            $"الانطلاق الفعلي: {actual}\r\n" +
            $"الحالة: {state.Trip.Status}";
    }

    public void Bind(TripResponse trip)
    {
        _state = null;
        var actual = trip.ActualDepartAt?.ToLocalTime().ToString("yyyy-MM-dd HH:mm") ?? "لم تنطلق بعد";
        _summary.Text =
            $"الرحلة: {trip.TripNo}\r\n" +
            "الكشف: بيانات الكشف غير محملة\r\n" +
            $"السائق: {trip.DriverId}\r\n" +
            $"المركبة: {trip.VehicleId}\r\n" +
            $"الانطلاق المخطط: {trip.PlannedDepartAt.ToLocalTime():yyyy-MM-dd HH:mm}\r\n" +
            $"الانطلاق الفعلي: {actual}\r\n" +
            $"الحالة: {trip.Status}";
    }

    private void RequestStart()
    {
        if (_state is null) return;
        StartRequested?.Invoke(
            _state.Trip.Id,
            new StartTripRequest(
                DateTimeOffset.UtcNow,
                _state.Trip.Version,
                OperationId("desktop-trip-start")));
    }
}
