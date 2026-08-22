using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

public static class ArrivalExecutionScreenCatalog
{
    public static readonly WaybillScreenDefinition ItemMovement = new("SHP-017", "حركة الصنف", TransportScreenProfile.ReportInquiry);
    public static readonly WaybillScreenDefinition WaybillMovement = new("SHP-018", "حركة البوليصة", TransportScreenProfile.ReportInquiry);
    public static readonly WaybillScreenDefinition TripTracking = new("SHP-031", "تتبع وإغلاق الرحلة", TransportScreenProfile.ControlApproval);
    public static readonly WaybillScreenDefinition TransitStop = new("SHP-032", "محطة وسيطة وترانزيت", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition PartialUnload = new("SHP-033", "تفريغ جزئي", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition ArrivalReceiving = new("SHP-034", "استلام واعتماد فرع الوصول", TransportScreenProfile.ControlApproval);
}

public sealed class ItemMovementForm : ShippingRtlForm
{
    private readonly TextBox _summary = InfoBox(820);
    private readonly DataGridView _timeline = ReadOnlyGrid();
    public ItemMovementForm() : base("حركة الصنف — SHP-017", 1100, 620)
    {
        _summary.Dock = DockStyle.Top;
        Controls.Add(_timeline); Controls.Add(_summary);
    }
    public void Bind(ItemMovementResponse value)
    {
        _summary.Text = $"الأصل {value.OriginalQuantity:N3} | المطلق {value.ReleasedQuantity:N3} | المخصص {value.AllocatedQuantity:N3} | قيد النقل {value.LoadedQuantity:N3} | الواصل {value.ArrivedQuantity:N3} | المسلم {value.DeliveredQuantity:N3} | المتبقي {value.RemainingQuantity:N3}";
        _timeline.DataSource = value.Timeline.ToList();
    }
}

public sealed record WaybillMovementScreenState(string Waybill, string OperationalStatus, string FinancialStatus, WaybillMovementResponse Movement);

public sealed class WaybillMovementForm : ShippingRtlForm
{
    private readonly TextBox _summary = InfoBox(820);
    private readonly DataGridView _timeline = ReadOnlyGrid();
    public WaybillMovementForm() : base("حركة البوليصة — SHP-018", 1100, 620)
    {
        _summary.Dock = DockStyle.Top;
        Controls.Add(_timeline); Controls.Add(_summary);
    }
    public void Bind(WaybillMovementScreenState state)
    {
        _summary.Text = $"البوليصة: {state.Waybill} | التشغيلي: {state.OperationalStatus} | المالي: {state.FinancialStatus}";
        _timeline.DataSource = state.Movement.Timeline.ToList();
    }
}

public sealed record TripTrackingScreenState(
    TripResponse Trip,
    string LastOperationalEvent,
    int WaybillCount,
    string Manifest,
    decimal CustodyBalance,
    int OpenExceptions,
    string ETAPlaceholder);

public sealed class TripTrackingCloseForm : ShippingRtlForm
{
    private TripTrackingScreenState? _state;
    private readonly TextBox _trip = InfoBox();
    private readonly TextBox _status = InfoBox();
    private readonly TextBox _last = InfoBox(420);
    private readonly TextBox _manifest = InfoBox();
    private readonly TextBox _custody = InfoBox();
    private readonly TextBox _exceptions = InfoBox();
    private readonly TextBox _eta = InfoBox();
    private readonly DataGridView _stops = ReadOnlyGrid();
    private readonly Label _message = new() { AutoSize = true };

    public TripTrackingCloseForm() : base("تتبع وإغلاق الرحلة — SHP-031", 1050, 680)
    {
        var top = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(12) };
        AddRow(top, 0, "الرحلة", _trip); AddRow(top, 1, "الحالة", _status); AddRow(top, 2, "آخر حدث", _last);
        AddRow(top, 3, "كشف الحمولة", _manifest); AddRow(top, 4, "رصيد العهدة", _custody);
        AddRow(top, 5, "الاستثناءات المفتوحة", _exceptions); AddRow(top, 6, "ETA", _eta);
        var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 55, FlowDirection = FlowDirection.RightToLeft };
        bottom.Controls.Add(ActionButton("إغلاق الرحلة", (_, _) => RequestClose())); bottom.Controls.Add(_message);
        Controls.Add(_stops); Controls.Add(top); Controls.Add(bottom);
    }
    public event Action<Guid, CloseTripRequest>? CloseTripRequested;
    public void Bind(TripTrackingScreenState state)
    {
        _state = state; _trip.Text = state.Trip.TripNo; _status.Text = state.Trip.Status; _last.Text = state.LastOperationalEvent;
        _manifest.Text = state.Manifest; _custody.Text = $"{state.CustodyBalance:N3}";
        _exceptions.Text = state.OpenExceptions == 0 ? "لا توجد استثناءات مفتوحة" : $"{state.OpenExceptions} استثناء مفتوح";
        _eta.Text = string.IsNullOrWhiteSpace(state.ETAPlaceholder) ? "GPS غير مفعّل في هذه المرحلة" : state.ETAPlaceholder;
        _stops.DataSource = state.Trip.Stops.ToList();
    }
    private void RequestClose()
    {
        if (_state is null) { _message.Text = "حمّل الرحلة أولاً."; return; }
        if (_state.CustodyBalance > 0m || _state.OpenExceptions > 0) { _message.Text = "الإغلاق محجوب: توجد عهدة أو استثناءات مفتوحة."; return; }
        CloseTripRequested?.Invoke(_state.Trip.Id, new CloseTripRequest(DateTimeOffset.UtcNow, _state.Trip.Version, OperationId("desktop-trip-close")));
    }
}

public sealed record TransitStopScreenState(
    Guid TripId, Guid ManifestId, Guid LocationId, string Trip, string Stop, string Waybill, string Item,
    decimal Expected, decimal Actual, Guid? HoldingId, decimal Holding, Guid? NextTripId, string Status);

public sealed class TransitStopForm : ShippingRtlForm
{
    private TransitStopScreenState? _state;
    private readonly DataGridView _rows = ReadOnlyGrid();
    private readonly NumericUpDown _reallocateQty = QuantityInput();
    private readonly Label _message = new() { AutoSize = true };
    public TransitStopForm() : base("محطة وسيطة وترانزيت — SHP-032", 1100, 620)
    {
        var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 62, FlowDirection = FlowDirection.RightToLeft };
        bottom.Controls.Add(ActionButton("تسجيل الوصول", (_, _) => RequestArrival()));
        bottom.Controls.Add(ActionButton("إعادة ترحيل", (_, _) => RequestReallocate()));
        bottom.Controls.Add(new Label { Text = "الكمية", AutoSize = true }); bottom.Controls.Add(_reallocateQty); bottom.Controls.Add(_message);
        Controls.Add(_rows); Controls.Add(bottom);
    }
    public event Action<Guid, RecordArrivalRequest>? ArrivalRequested;
    public event Action<Guid, ReallocateTransitRequest>? ReallocateRequested;
    public void Bind(IReadOnlyList<TransitStopScreenState> states)
    {
        _rows.DataSource = states.ToList();
        _state = _rows.CurrentRow?.DataBoundItem as TransitStopScreenState;
        _rows.SelectionChanged += (_, _) => _state = _rows.CurrentRow?.DataBoundItem as TransitStopScreenState;
    }
    private void RequestArrival()
    {
        if (_state is null) { _message.Text = "اختر سطراً."; return; }
        ArrivalRequested?.Invoke(_state.TripId, new RecordArrivalRequest(_state.ManifestId, _state.LocationId, DateTimeOffset.UtcNow, OperationId("desktop-arrival")));
    }
    private void RequestReallocate()
    {
        if (_state?.HoldingId is not Guid holdingId || _state.NextTripId is not Guid nextTrip || _reallocateQty.Value <= 0m)
        { _message.Text = "حدد رصيد ترانزيت ورحلة تالية وكمية موجبة."; return; }
        ReallocateRequested?.Invoke(holdingId, new ReallocateTransitRequest(nextTrip, _reallocateQty.Value, OperationId("desktop-reallocate")));
    }
}

public sealed record PartialUnloadScreenState(
    Guid ArrivalId, Guid ManifestLineId, string ManifestLine, decimal ExpectedQty, decimal ActualQty,
    decimal RemainingInTransit, string DifferenceType, decimal DamageQty, string? Notes);

public sealed class PartialUnloadForm : ShippingRtlForm
{
    private PartialUnloadScreenState? _state;
    private readonly TextBox _line = InfoBox();
    private readonly TextBox _expected = InfoBox();
    private readonly TextBox _actual = InfoBox();
    private readonly TextBox _remaining = InfoBox();
    private readonly NumericUpDown _unload = QuantityInput();
    private readonly NumericUpDown _damage = QuantityInput();
    private readonly ComboBox _difference = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 180 };
    private readonly TextBox _evidence = InputBox();
    private readonly TextBox _notes = InputBox(360);
    private readonly Label _message = new() { AutoSize = true };
    public PartialUnloadForm() : base("تفريغ جزئي — SHP-033", 800, 600)
    {
        _difference.Items.AddRange(["", "SHORT", "DAMAGE", "SHORT_AND_DAMAGE"]); _difference.SelectedIndex = 0;
        var table = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(12) };
        AddRow(table, 0, "سطر الكشف", _line); AddRow(table, 1, "المتوقع", _expected); AddRow(table, 2, "المستلم", _actual);
        AddRow(table, 3, "المتبقي في النقل", _remaining); AddRow(table, 4, "كمية التفريغ", _unload); AddRow(table, 5, "تالف", _damage);
        AddRow(table, 6, "نوع الفرق", _difference); AddRow(table, 7, "مرجع الدليل UUID", _evidence); AddRow(table, 8, "ملاحظات", _notes);
        var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 55, FlowDirection = FlowDirection.RightToLeft };
        bottom.Controls.Add(ActionButton("تسجيل التفريغ", (_, _) => RequestUnload())); bottom.Controls.Add(_message);
        Controls.Add(table); Controls.Add(bottom);
    }
    public event Action<Guid, RecordUnloadRequest>? UnloadRequested;
    public void Bind(PartialUnloadScreenState state)
    {
        _state = state; _line.Text = state.ManifestLine; _expected.Text = state.ExpectedQty.ToString("N3");
        _actual.Text = state.ActualQty.ToString("N3"); _remaining.Text = state.RemainingInTransit.ToString("N3");
        _unload.Maximum = Math.Max(0m, state.RemainingInTransit); _damage.Maximum = _unload.Maximum; _notes.Text = state.Notes ?? "";
    }
    private void RequestUnload()
    {
        if (_state is null) { _message.Text = "حمّل السطر أولاً."; return; }
        if (_damage.Value > _unload.Value) { _message.Text = "كمية التالف لا تتجاوز كمية التفريغ."; return; }
        Guid? evidence = null;
        if (!string.IsNullOrWhiteSpace(_evidence.Text) && !Guid.TryParse(_evidence.Text.Trim(), out var parsed)) { _message.Text = "مرجع الدليل غير صحيح."; return; }
        else if (Guid.TryParse(_evidence.Text.Trim(), out var parsedEvidence)) evidence = parsedEvidence;
        var input = new ArrivalUnloadLineInput(_state.ManifestLineId, _unload.Value, _damage.Value,
            string.IsNullOrWhiteSpace(_difference.Text) ? null : _difference.Text, evidence, _notes.Text);
        UnloadRequested?.Invoke(_state.ArrivalId, new RecordUnloadRequest([input], DateTimeOffset.UtcNow, OperationId("desktop-unload")));
    }
}

public sealed class ArrivalReceivingForm : ShippingRtlForm
{
    private ArrivalReceiptResponse? _receipt;
    private readonly TextBox _summary = InfoBox(800);
    private readonly DataGridView _lines = ReadOnlyGrid();
    private readonly Label _message = new() { AutoSize = true };
    public ArrivalReceivingForm() : base("استلام واعتماد فرع الوصول — SHP-034", 1100, 650)
    {
        _summary.Dock = DockStyle.Top;
        var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 55, FlowDirection = FlowDirection.RightToLeft };
        bottom.Controls.Add(ActionButton("اعتماد الاستلام", (_, _) => RequestFinalize())); bottom.Controls.Add(_message);
        Controls.Add(_lines); Controls.Add(_summary); Controls.Add(bottom);
    }
    public event Action<Guid, FinalizeArrivalRequest>? FinalizeRequested;
    public void Bind(ArrivalReceiptResponse receipt)
    {
        _receipt = receipt;
        var expected = receipt.Lines.Sum(x => x.ExpectedQuantity); var actual = receipt.Lines.Sum(x => x.ActualQuantity);
        _summary.Text = $"الرحلة {receipt.TripId} | الكشف {receipt.ManifestId} | الموقع {receipt.LocationId} | الاستلام {receipt.ReceivedAt:g} | المتوقع {expected:N3} | الفعلي {actual:N3} | الحالة {receipt.Status}";
        _lines.DataSource = receipt.Lines.ToList();
    }
    private void RequestFinalize()
    {
        if (_receipt is null) { _message.Text = "حمّل الاستلام أولاً."; return; }
        if (_receipt.Lines.Any(x => x.DifferenceType == "UNVALIDATED")) { _message.Text = "لا يمكن الاعتماد قبل التحقق من كل الفروقات."; return; }
        FinalizeRequested?.Invoke(_receipt.Id, new FinalizeArrivalRequest(_receipt.Version, OperationId("desktop-arrival-finalize")));
    }
}
