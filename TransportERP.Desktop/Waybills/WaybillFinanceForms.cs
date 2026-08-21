using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

public static class WaybillFinanceScreenCatalog
{
    public static readonly WaybillScreenDefinition Pricing = new("SHP-009", "الخدمات والرسوم", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition PaymentPlan = new("SHP-010", "خطة الدفع", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Collections = new("SHP-011", "تحصيلات البوليصة", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition FinancialStatus = new("SHP-012", "حالة السداد والمتبقي", TransportScreenProfile.ReportInquiry);
}

/// <summary>
/// SHP-009 — B only exposes the pricing totals inherited from the Waybill and the action that opens the governed payment plan.
/// No accounting account or posting rule is embedded in Desktop.
/// </summary>
public sealed class WaybillPricingForm : Form
{
    private readonly TextBox _freight = ReadOnlyBox();
    private readonly TextBox _discount = ReadOnlyBox();
    private readonly TextBox _net = ReadOnlyBox();
    private readonly TextBox _currency = ReadOnlyBox();

    public WaybillPricingForm()
    {
        Text = "الخدمات والرسوم — SHP-009";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(720, 360);

        var panel = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(16) };
        AddRow(panel, 0, "إجمالي الأجرة", _freight);
        AddRow(panel, 1, "الخصم", _discount);
        AddRow(panel, 2, "الصافي", _net);
        AddRow(panel, 3, "العملة", _currency);

        var openPlan = new Button { Text = "خطة الدفع — SHP-010", AutoSize = true, Dock = DockStyle.Bottom };
        openPlan.Click += (_, _) => PaymentPlanRequested?.Invoke(this, EventArgs.Empty);
        Controls.Add(panel);
        Controls.Add(openPlan);
    }

    public event EventHandler? PaymentPlanRequested;

    public void Bind(WaybillResponse waybill)
    {
        _freight.Text = waybill.FreightTotal.ToString("N2");
        _discount.Text = waybill.DiscountTotal.ToString("N2");
        _net.Text = waybill.NetAmount.ToString("N2");
        _currency.Text = waybill.CurrencyId.ToString();
    }

    private static TextBox ReadOnlyBox() => new() { ReadOnly = true, Width = 320 };

    private static void AddRow(TableLayoutPanel table, int row, string label, Control control)
    {
        table.RowCount = Math.Max(table.RowCount, row + 1);
        table.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Right }, 0, row);
        table.Controls.Add(control, 1, row);
    }
}

/// <summary>SHP-010 — payment plan is distinct from actual collections.</summary>
public sealed class WaybillPaymentPlanForm : Form
{
    private readonly DataGridView _lines = Grid();
    private readonly Label _total = new() { AutoSize = true };
    private readonly Label _version = new() { AutoSize = true };

    public WaybillPaymentPlanForm()
    {
        Text = "خطة الدفع للبوليصة — SHP-010";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        MinimumSize = new Size(900, 520);

        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 48, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
        var save = new Button { Text = "حفظ خطة الدفع", AutoSize = true };
        save.Click += (_, _) => SaveRequested?.Invoke(this, EventArgs.Empty);
        top.Controls.AddRange([save, new Label { Text = "الصافي:" }, _total, new Label { Text = "الإصدار:" }, _version]);

        _lines.Dock = DockStyle.Fill;
        Controls.Add(_lines);
        Controls.Add(top);
    }

    public event EventHandler? SaveRequested;

    public void Bind(PaymentPlanResponse response)
    {
        _total.Text = response.NetAmount.ToString("N2");
        _version.Text = response.WaybillVersion.ToString();
        _lines.DataSource = response.Lines.ToList();
    }

    private static DataGridView Grid() => new()
    {
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        AutoGenerateColumns = true,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
        ReadOnly = true,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        RightToLeft = RightToLeft.Yes
    };
}

/// <summary>SHP-011 — accepted collection rows are immutable; reversal is a separate governed action.</summary>
public sealed class WaybillCollectionsForm : Form
{
    private readonly DataGridView _collections = Grid();
    private readonly Label _state = new() { AutoSize = true };

    public WaybillCollectionsForm()
    {
        Text = "تحصيلات البوليصة — SHP-011";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        MinimumSize = new Size(980, 560);

        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 52, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
        var record = new Button { Text = "تسجيل تحصيل", AutoSize = true };
        record.Click += (_, _) => RecordRequested?.Invoke(this, EventArgs.Empty);
        var reverse = new Button { Text = "عكس التحصيل المحدد", AutoSize = true };
        reverse.Click += (_, _) => ReverseRequested?.Invoke(this, EventArgs.Empty);
        top.Controls.AddRange([record, reverse, _state]);

        _collections.Dock = DockStyle.Fill;
        Controls.Add(_collections);
        Controls.Add(top);
    }

    public event EventHandler? RecordRequested;
    public event EventHandler? ReverseRequested;

    public void Bind(IReadOnlyList<CollectionResponse> items)
    {
        _collections.DataSource = items.ToList();
        _state.Text = items.Count == 0 ? "لا توجد تحصيلات" : $"عدد الحركات: {items.Count}";
    }

    public CollectionResponse? SelectedCollection
        => _collections.CurrentRow?.DataBoundItem as CollectionResponse;

    private static DataGridView Grid() => new()
    {
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        AutoGenerateColumns = true,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
        ReadOnly = true,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false,
        RightToLeft = RightToLeft.Yes
    };
}

/// <summary>SHP-012 — read-only projection; FinancialStatus is derived by server rules and never edited here.</summary>
public sealed class WaybillFinancialStatusForm : Form
{
    private readonly TextBox _net = Box();
    private readonly TextBox _paid = Box();
    private readonly TextBox _remaining = Box();
    private readonly TextBox _status = Box();
    private readonly TextBox _version = Box();

    public WaybillFinancialStatusForm()
    {
        Text = "حالة السداد والمتبقي — SHP-012";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        MinimumSize = new Size(720, 400);

        var panel = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(16) };
        AddRow(panel, 0, "صافي البوليصة", _net);
        AddRow(panel, 1, "المدفوع", _paid);
        AddRow(panel, 2, "المتبقي", _remaining);
        AddRow(panel, 3, "الحالة المالية", _status);
        AddRow(panel, 4, "إصدار البوليصة", _version);
        Controls.Add(panel);
    }

    public void Bind(WaybillFinancialStatusResponse value)
    {
        _net.Text = $"{value.NetAmount.Amount:N2} / {value.NetAmount.CurrencyId}";
        _paid.Text = $"{value.PaidEquivalent.Amount:N2} / {value.PaidEquivalent.CurrencyId}";
        _remaining.Text = $"{value.RemainingEquivalent.Amount:N2} / {value.RemainingEquivalent.CurrencyId}";
        _status.Text = value.FinancialStatus;
        _version.Text = value.WaybillVersion.ToString();
    }

    private static TextBox Box() => new() { ReadOnly = true, Width = 360 };

    private static void AddRow(TableLayoutPanel table, int row, string label, Control control)
    {
        table.RowCount = Math.Max(table.RowCount, row + 1);
        table.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Right }, 0, row);
        table.Controls.Add(control, 1, row);
    }
}
