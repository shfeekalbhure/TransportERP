using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

/// <summary>Shared RTL business-content helpers independent of a window.</summary>
public partial class ShippingRtlControl : UserControl
{
    public ShippingRtlControl() : this(string.Empty)
    {
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }

    protected ShippingRtlControl(string title, int width = 900, int height = 520)
    {
        Load += TransportERP.Desktop.CoreUI.StandardCommandBarPolicy.OnScreenLoad;
        Text = title;
        RightToLeft = RightToLeft.Yes;
        Size = new Size(width, height);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
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
        var button = new Button
        {
            Text = text,
            AutoSize = true
        };
        button.Click += handler;
        return button;
    }

    protected static TextBox InfoBox(int width = 280) => new()
    {
        ReadOnly = true,
        Width = width
    };
    protected static TextBox InputBox(int width = 280) => new()
    {
        Width = width
    };
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
        if (string.IsNullOrWhiteSpace(value) || value.Trim()is "[]" or "{}")
            return "لا توجد مخاطر مسجلة";
        return value.Trim();
    }
}
