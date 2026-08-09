using TransportERP.Desktop.CoreUI.Architecture;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// أساس W3 للشاشات المرجعية. ترتيب الصفوف وحده يملك القياس الرأسي:
/// Fixed (toolbar/search)، Content (البيانات)، Fill (grid)، من دون Height محلي أو Scroll داخلي.
/// </summary>
public sealed class TransportScreenFrame : UserControl
{
    private readonly TableLayoutPanel _root = new();

    public TransportToolbar Toolbar { get; } = new();
    public TransportSearchPanel Search { get; } = new();
    public Panel MainData { get; } = new();
    public TransportDataGrid Grid { get; } = new();
    public TransportAuditPanel Audit { get; } = new();

    public TransportScreenFrame(ReferenceScreenDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(definition);
        AutoScaleMode = AutoScaleMode.Dpi;
        Dock = DockStyle.Fill;
        BackColor = UiTheme.WorkspaceBackground;
        Padding = new Padding(8);
        RightToLeft = RightToLeft.Yes;

        _root.Dock = DockStyle.Fill;
        _root.ColumnCount = 1;
        _root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _root.RowCount = 5;
        _root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // toolbar: Fixed
        _root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // search: Fixed
        _root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // main data: Content
        _root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // grid: Fill
        _root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // audit: Fixed

        Toolbar.Dock = DockStyle.Top;
        Search.Dock = DockStyle.Top;
        MainData.Dock = DockStyle.Top;
        MainData.AutoSize = true;
        MainData.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        MainData.AutoScroll = false;
        MainData.Padding = new Padding(8);
        MainData.BackColor = UiTheme.SurfaceBackground;
        Grid.Dock = DockStyle.Fill;
        Grid.AutoGenerateColumns = false;
        Audit.Dock = DockStyle.Top;

        _root.Controls.Add(Toolbar, 0, 0);
        _root.Controls.Add(Search, 0, 1);
        _root.Controls.Add(MainData, 0, 2);
        _root.Controls.Add(Grid, 0, 3);
        _root.Controls.Add(Audit, 0, 4);
        Controls.Add(_root);

        BuildDefinition(definition);
    }

    private void BuildDefinition(ReferenceScreenDefinition definition)
    {
        Search.SearchPlaceholder = $"ابحث في {definition.Title}";
        Search.SetStatusItems("نشط", "موقوف");

        var fields = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Top,
            ColumnCount = definition.UsesTree ? 2 : 4,
            Padding = Padding.Empty,
            Margin = Padding.Empty,
            RightToLeft = RightToLeft.Yes
        };
        for (var column = 0; column < fields.ColumnCount; column++)
            fields.ColumnStyles.Add(new ColumnStyle(column % 2 == 0 ? SizeType.AutoSize : SizeType.Percent, column % 2 == 0 ? 0F : 50F));

        foreach (var field in definition.Fields.Select((caption, index) => (caption, index)))
        {
            var row = field.index / 2;
            while (fields.RowCount <= row)
            {
                fields.RowCount++;
                fields.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            var column = definition.UsesTree ? 0 : (field.index % 2) * 2;
            var label = new Label { Text = field.caption, AutoSize = true, Anchor = AnchorStyles.Right, Margin = new Padding(6, 8, 6, 8), TextAlign = ContentAlignment.MiddleRight };
            var editor = new TextBox { Dock = DockStyle.Fill, MinimumSize = new Size(140, 0), RightToLeft = RightToLeft.Yes, ReadOnly = definition.IsReadOnly, Margin = new Padding(6) };
            fields.Controls.Add(label, column, row);
            fields.Controls.Add(editor, column + 1, row);
        }

        if (definition.UsesTree)
        {
            var tree = new TreeView { Dock = DockStyle.Left, Width = 260, RightToLeft = RightToLeft.Yes, RightToLeftLayout = true, HideSelection = false };
            tree.Nodes.Add("الحسابات");
            MainData.Controls.Add(tree);
        }

        MainData.Controls.Add(fields);
        foreach (var column in definition.GridColumns)
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = column, HeaderText = column, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = definition.IsReadOnly });
        Grid.Rows.Add();
    }
}
