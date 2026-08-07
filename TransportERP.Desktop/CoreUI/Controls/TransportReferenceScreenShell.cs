using System.ComponentModel;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// القالب الموحد لشاشات البيانات المرجعية.
/// جميع المقاسات والمحاذاة والحاويات العامة تُدار من هنا ومن TransportUiMetrics
/// حتى لا تختلف شاشة عن أخرى.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportReferenceScreenShell : UserControl
{
    private readonly TableLayoutPanel _root = new();
    private readonly TableLayoutPanel _topUtilityRow = new();

    public TransportGroupBox NotificationGroup { get; } = CreateGroupBox("الإشعارات");
    public TransportGroupBox DataGroup { get; } = CreateGroupBox("البيانات الرئيسية");
    public TransportGroupBox SearchGroup { get; } = CreateGroupBox("البحث والتصفية");
    public TransportGroupBox GridGroup { get; } = CreateGroupBox("قائمة السجلات");
    public TransportGroupBox AuditGroup { get; } = CreateGroupBox("معلومات الإنشاء والتعديل");

    public TransportAlertBar AlertBar { get; } = new();
    public TransportToolbar Toolbar { get; } = new();
    public Panel DataHost { get; } = new();
    public TransportSearchPanel SearchPanel { get; } = new();
    public TransportDataGrid Grid { get; } = new();
    public TransportPagination Pagination { get; } = new();
    public TransportAuditPanel AuditPanel { get; } = new();

    public TransportReferenceScreenShell()
    {
        InitializeLayout();
    }

    [Category("TransportERP")]
    [Description("عنوان حاوية البيانات الرئيسية.")]
    [DefaultValue("البيانات الرئيسية")]
    public string DataGroupTitle
    {
        get => DataGroup.Text;
        set => DataGroup.Text = string.IsNullOrWhiteSpace(value) ? "البيانات الرئيسية" : value.Trim();
    }

    private void InitializeLayout()
    {
        BackColor = Color.FromArgb(247, 249, 252);
        Dock = DockStyle.Fill;
        Padding = new Padding(TransportUiMetrics.ScreenOuterPadding);
        RightToLeft = RightToLeft.Yes;

        _root.ColumnCount = 1;
        _root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _root.Dock = DockStyle.Fill;
        _root.Margin = Padding.Empty;
        _root.Padding = Padding.Empty;
        _root.RowCount = 6;
        _root.RightToLeft = RightToLeft.Yes;

        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.TopUtilityRowHeight));
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.ToolbarHeight));
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.MainDataGroupHeight));
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.SearchGroupHeight));
        _root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.AuditGroupHeight));

        _topUtilityRow.ColumnCount = 2;
        _topUtilityRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72F));
        _topUtilityRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
        _topUtilityRow.Dock = DockStyle.Fill;
        _topUtilityRow.Margin = Padding.Empty;
        _topUtilityRow.Padding = Padding.Empty;
        _topUtilityRow.RightToLeft = RightToLeft.Yes;
        _topUtilityRow.RowCount = 1;
        _topUtilityRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        DataHost.BackColor = Color.White;
        DataHost.Dock = DockStyle.Fill;
        DataHost.Margin = Padding.Empty;
        DataHost.Padding = new Padding(TransportUiMetrics.CompactPadding);
        DataHost.RightToLeft = RightToLeft.Yes;

        // أي جدول حقول يضاف إلى البيانات الرئيسية يأخذ المقاسات العالمية تلقائيًا.
        DataHost.ControlAdded += (_, e) => ApplyMainDataMetrics(e.Control);
        DataGroup.Controls.Add(DataHost);

        AlertBar.Dock = DockStyle.Fill;
        AlertBar.Margin = Padding.Empty;
        NotificationGroup.Controls.Add(AlertBar);

        SearchPanel.Dock = DockStyle.Fill;
        SearchPanel.Margin = Padding.Empty;
        SearchGroup.Controls.Add(SearchPanel);

        Grid.Dock = DockStyle.Fill;
        Grid.Margin = Padding.Empty;
        GridGroup.Controls.Add(Grid);

        AuditPanel.Dock = DockStyle.Fill;
        AuditPanel.Margin = Padding.Empty;
        AuditGroup.Controls.Add(AuditPanel);

        Toolbar.Dock = DockStyle.Fill;
        Toolbar.Margin = Padding.Empty;
        Pagination.Dock = DockStyle.Fill;
        Pagination.Margin = Padding.Empty;

        _topUtilityRow.Controls.Add(NotificationGroup, 0, 0);
        _topUtilityRow.Controls.Add(Pagination, 1, 0);

        _root.Controls.Add(_topUtilityRow, 0, 0);
        _root.Controls.Add(Toolbar, 0, 1);
        _root.Controls.Add(DataGroup, 0, 2);
        _root.Controls.Add(SearchGroup, 0, 3);
        _root.Controls.Add(GridGroup, 0, 4);
        _root.Controls.Add(AuditGroup, 0, 5);
        Controls.Add(_root);
    }

    /// <summary>
    /// يكيّف القالب للشاشات المتخصصة مثل الإعدادات والشاشات الشجرية
    /// من دون إنشاء قالب مكرر خارج CoreUI.
    /// </summary>
    public void ConfigureWorkspaceMode(bool showSearch, bool showGrid, bool expandDataWorkspace)
    {
        SearchGroup.Visible = showSearch;
        GridGroup.Visible = showGrid;
        _root.RowStyles[3].SizeType = SizeType.Absolute;
        _root.RowStyles[3].Height = showSearch ? TransportUiMetrics.SearchGroupHeight : 0F;

        if (expandDataWorkspace)
        {
            _root.RowStyles[2].SizeType = SizeType.Percent;
            _root.RowStyles[2].Height = 100F;
            _root.RowStyles[4].SizeType = SizeType.Absolute;
            _root.RowStyles[4].Height = 0F;
        }
        else
        {
            _root.RowStyles[2].SizeType = SizeType.Absolute;
            _root.RowStyles[2].Height = TransportUiMetrics.MainDataGroupHeight;
            _root.RowStyles[4].SizeType = SizeType.Percent;
            _root.RowStyles[4].Height = showGrid ? 100F : 0F;
        }
    }

    /// <summary>
    /// يفرض ارتفاع الحقول والمسافة بين الصفوف والمحاذاة RTL على البيانات الرئيسية.
    /// المسافة بين صف وآخر ثابتة 1.5 مم تقريبًا في جميع الشاشات.
    /// </summary>
    private static void ApplyMainDataMetrics(Control control)
    {
        if (control is not TableLayoutPanel table)
        {
            return;
        }

        table.Margin = Padding.Empty;
        table.Padding = Padding.Empty;
        table.RightToLeft = RightToLeft.Yes;

        for (var row = 0; row < table.RowStyles.Count; row++)
        {
            var style = table.RowStyles[row];
            if (style.SizeType == SizeType.Absolute && !IsMultilineRow(table, row))
            {
                style.Height = TransportUiMetrics.MainDataRowHeight;
            }
        }

        foreach (Control child in table.Controls)
        {
            ApplyStandardControlMetrics(child);
        }
    }

    /// <summary>
    /// يوحّد ارتفاع ومحاذاة كل أداة داخل البيانات الرئيسية دون تكرار الخصائص في Designer لكل شاشة.
    /// </summary>
    private static void ApplyStandardControlMetrics(Control control)
    {
        var isMultiline = control is TextBox textBox && textBox.Multiline;
        control.Margin = new Padding(
            TransportUiMetrics.MainDataHorizontalMargin,
            TransportUiMetrics.MainDataVerticalMargin,
            TransportUiMetrics.MainDataHorizontalMargin,
            TransportUiMetrics.MainDataVerticalMargin);
        control.RightToLeft = RightToLeft.Yes;

        switch (control)
        {
            case TextBox textBox when !textBox.Multiline:
                textBox.AutoSize = false;
                textBox.Height = TransportUiMetrics.MainDataControlHeight;
                textBox.MinimumSize = new Size(textBox.MinimumSize.Width, TransportUiMetrics.MainDataControlHeight);
                textBox.TextAlign = HorizontalAlignment.Right;
                break;

            case TextBox textBox when textBox.Multiline:
                textBox.MinimumSize = new Size(textBox.MinimumSize.Width, TransportUiMetrics.MainDataMultilineMinHeight);
                textBox.TextAlign = HorizontalAlignment.Right;
                break;

            case ComboBox comboBox:
                comboBox.Height = TransportUiMetrics.MainDataControlHeight;
                comboBox.MinimumSize = new Size(comboBox.MinimumSize.Width, TransportUiMetrics.MainDataControlHeight);
                break;

            case NumericUpDown numericUpDown:
                numericUpDown.Height = TransportUiMetrics.MainDataControlHeight;
                numericUpDown.MinimumSize = new Size(numericUpDown.MinimumSize.Width, TransportUiMetrics.MainDataControlHeight);
                numericUpDown.TextAlign = HorizontalAlignment.Right;
                break;

            case DateTimePicker dateTimePicker:
                dateTimePicker.Height = TransportUiMetrics.MainDataControlHeight;
                dateTimePicker.MinimumSize = new Size(dateTimePicker.MinimumSize.Width, TransportUiMetrics.MainDataControlHeight);
                dateTimePicker.RightToLeftLayout = true;
                break;

            case Label label:
                label.AutoSize = false;
                label.MinimumSize = new Size(label.MinimumSize.Width, TransportUiMetrics.MainDataControlHeight);
                label.TextAlign = ContentAlignment.MiddleRight;
                break;
        }

        if (!isMultiline && control is not Label)
        {
            control.MinimumSize = new Size(control.MinimumSize.Width, TransportUiMetrics.MainDataControlHeight);
        }
    }

    private static bool IsMultilineRow(TableLayoutPanel table, int row)
    {
        foreach (Control child in table.Controls)
        {
            if (table.GetRow(child) == row && child is TextBox textBox && textBox.Multiline)
            {
                return true;
            }
        }

        return false;
    }

    private static TransportGroupBox CreateGroupBox(string title) => new()
    {
        Dock = DockStyle.Fill,
        Margin = Padding.Empty,
        Text = title
    };
}
