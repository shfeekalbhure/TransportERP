using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

public static class WaybillFoundationScreenCatalog
{
    public static readonly WaybillScreenDefinition Header = new("SHP-005", "رأس البوليصة", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Parties = new("SHP-006", "أطراف البوليصة", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Items = new("SHP-007", "أصناف البوليصة", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Measurements = new("SHP-008", "الأوزان والأبعاد والقيم", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Approval = new("SHP-014", "اعتماد البوليصة", TransportScreenProfile.ControlApproval);
}

public sealed record WaybillScreenDefinition(string ScreenCode, string ArabicTitle, TransportScreenProfile Profile);

/// <summary>
/// P2-C01-A transaction shell. It has no database reference; callers bind it to an HTTP/API client.
/// SHP-005/006/007/008 are represented as governed transaction tabs.
/// </summary>
public sealed class WaybillDraftForm : Form
{
    private readonly TextBox _draftNo = ReadOnlyTextBox();
    private readonly TextBox _officialNo = ReadOnlyTextBox();
    private readonly TextBox _status = ReadOnlyTextBox();
    private readonly DataGridView _parties = Grid();
    private readonly DataGridView _items = Grid();
    private readonly Label _validation = new() { AutoSize = true };

    public WaybillDraftForm()
    {
        Text = "البوليصة — SHP-005";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1000, 680);

        var toolbar = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.RightToLeft,
            Height = 48,
            Padding = new Padding(8)
        };
        toolbar.Controls.AddRange([
            ActionButton("جديد", () => NewRequested?.Invoke(this, EventArgs.Empty)),
            ActionButton("حفظ مسودة", () => SaveRequested?.Invoke(this, EventArgs.Empty)),
            ActionButton("إرسال للاعتماد", () => SubmitRequested?.Invoke(this, EventArgs.Empty)),
            ActionButton("إلغاء", () => CancelRequested?.Invoke(this, EventArgs.Empty)),
            ActionButton("إغلاق", Close)
        ]);

        var summary = new TableLayoutPanel { Dock = DockStyle.Top, Height = 78, ColumnCount = 6, Padding = new Padding(8) };
        summary.ColumnStyles.Clear();
        for (var i = 0; i < 6; i++) summary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66f));
        AddPair(summary, 0, "رقم المسودة", _draftNo);
        AddPair(summary, 2, "رقم البوليصة", _officialNo);
        AddPair(summary, 4, "الحالة", _status);

        var tabs = new TabControl { Dock = DockStyle.Fill, RightToLeft = RightToLeft.Yes, RightToLeftLayout = true };
        tabs.TabPages.Add(BuildHeaderTab());
        tabs.TabPages.Add(BuildPartyTab());
        tabs.TabPages.Add(BuildItemsTab());
        tabs.TabPages.Add(BuildMeasurementsTab());

        var footer = new Panel { Dock = DockStyle.Bottom, Height = 48, Padding = new Padding(8) };
        _validation.Dock = DockStyle.Right;
        footer.Controls.Add(_validation);

        Controls.Add(tabs);
        Controls.Add(footer);
        Controls.Add(summary);
        Controls.Add(toolbar);
    }

    public event EventHandler? NewRequested;
    public event EventHandler? SaveRequested;
    public event EventHandler? SubmitRequested;
    public event EventHandler? CancelRequested;

    public void Bind(WaybillResponse value)
    {
        _draftNo.Text = value.DraftNo;
        _officialNo.Text = value.WaybillNo ?? "— مسودة بلا رقم رسمي —";
        _status.Text = value.Status;
        _parties.DataSource = value.Parties.ToList();
        _items.DataSource = value.Items.ToList();
        _validation.Text = value.WaybillNo is null
            ? "الترقيم الرسمي يتم من السيرفر عند الاعتماد فقط"
            : $"رقم رسمي: {value.WaybillNo}";
    }

    public void SetValidation(IReadOnlyList<string> errors)
        => _validation.Text = errors.Count == 0 ? "جاهزة" : string.Join(" | ", errors);

    private TabPage BuildHeaderTab()
    {
        var page = NewTab("SHP-005 — البيانات الأساسية");
        var form = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 4, Padding = new Padding(12) };
        AddEditor(form, 0, "الفرع *", "Branch");
        AddEditor(form, 2, "تاريخ البوليصة *", "WaybillDateTime");
        AddEditor(form, 4, "المنشأ *", "Origin");
        AddEditor(form, 6, "الوجهة *", "Destination");
        AddEditor(form, 8, "العملة *", "Currency");
        AddEditor(form, 10, "سعر الصرف *", "ExchangeRate");
        AddEditor(form, 12, "نوع الخدمة", "ServiceType");
        AddEditor(form, 14, "الأولوية", "Priority");
        page.Controls.Add(form);
        return page;
    }

    private TabPage BuildPartyTab()
    {
        var page = NewTab("SHP-006 — المرسل والمستلم والدافع");
        _parties.Dock = DockStyle.Fill;
        page.Controls.Add(_parties);
        return page;
    }

    private TabPage BuildItemsTab()
    {
        var page = NewTab("SHP-007 — الأصناف");
        _items.Dock = DockStyle.Fill;
        page.Controls.Add(_items);
        return page;
    }

    private static TabPage BuildMeasurementsTab()
    {
        var page = NewTab("SHP-008 — الأوزان والأبعاد والقيم");
        var label = new Label
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            Padding = new Padding(12),
            Text = "الوزن والطول والعرض والارتفاع والقيمة المعلنة تُحرر داخل سطر الصنف، ولا تقبل قيماً سالبة."
        };
        page.Controls.Add(label);
        return page;
    }

    private static TabPage NewTab(string title) => new(title) { RightToLeft = RightToLeft.Yes };

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

    private static TextBox ReadOnlyTextBox() => new() { ReadOnly = true, Dock = DockStyle.Fill };

    private static Button ActionButton(string text, Action action)
    {
        var button = new Button { Text = text, AutoSize = true };
        button.Click += (_, _) => action();
        return button;
    }

    private static void AddPair(TableLayoutPanel table, int column, string label, Control control)
    {
        table.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Right }, column, 0);
        table.Controls.Add(control, column + 1, 0);
    }

    private static void AddEditor(TableLayoutPanel table, int row, string label, string name)
    {
        while (table.RowCount <= row / 2) table.RowCount++;
        var r = row / 2;
        var c = row % 4;
        table.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Right }, c, r);
        table.Controls.Add(new TextBox { Name = name, Width = 280, Anchor = AnchorStyles.Left | AnchorStyles.Right }, c + 1, r);
    }
}

/// <summary>SHP-014 control/approval surface. Approval is online/server-authoritative.</summary>
public sealed class WaybillApprovalForm : Form
{
    private readonly Label _draft = new() { AutoSize = true };
    private readonly Label _status = new() { AutoSize = true };
    private readonly ListBox _blocking = new() { Dock = DockStyle.Fill };

    public WaybillApprovalForm()
    {
        Text = "اعتماد البوليصة — SHP-014";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(760, 520);

        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 52, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
        var approve = new Button { Text = "اعتماد وإصدار الرقم", AutoSize = true };
        approve.Click += (_, _) => ApproveRequested?.Invoke(this, EventArgs.Empty);
        var returned = new Button { Text = "إرجاع للاستكمال", AutoSize = true };
        returned.Click += (_, _) => ReturnRequested?.Invoke(this, EventArgs.Empty);
        top.Controls.AddRange([approve, returned]);

        var header = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 42, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
        header.Controls.AddRange([new Label { Text = "المسودة:" }, _draft, new Label { Text = "الحالة:" }, _status]);

        Controls.Add(_blocking);
        Controls.Add(header);
        Controls.Add(top);
    }

    public event EventHandler? ApproveRequested;
    public event EventHandler? ReturnRequested;

    public void Bind(WaybillResponse waybill, WaybillValidationResponse validation)
    {
        _draft.Text = waybill.DraftNo;
        _status.Text = waybill.Status;
        _blocking.DataSource = validation.BlockingErrors.ToList();
    }
}
