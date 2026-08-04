namespace TransportERP.Desktop.Controls;

/// <summary>
/// رأس موحد وثابت لجميع شاشات TransportERP.
/// يحتوي اسم الشاشة والمسار التعريفي وشريط الأدوات والتنقل.
/// </summary>
public sealed class ScreenHeaderControl : UserControl
{
    private readonly Label _titleLabel;
    private readonly Label _breadcrumbLabel;
    private readonly Label _recordLabel;

    public ScreenHeaderControl()
    {
        Dock = DockStyle.Top;
        Height = 118;
        BackColor = Color.White;
        RightToLeft = RightToLeft.Yes;
        Padding = new Padding(14, 10, 14, 8);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Color.White,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            RightToLeft = RightToLeft.Yes
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var titleRow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            BackColor = Color.White,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            RightToLeft = RightToLeft.Yes
        };

        _titleLabel = new Label
        {
            AutoSize = true,
            Text = "اسم الشاشة",
            Font = new Font("Segoe UI", 20F, FontStyle.Bold),
            ForeColor = Color.FromArgb(31, 45, 61),
            TextAlign = ContentAlignment.MiddleRight,
            Margin = new Padding(0, 2, 18, 0)
        };

        _breadcrumbLabel = new Label
        {
            AutoSize = true,
            Text = "المسار التعريفي",
            Font = new Font("Segoe UI", 10F, FontStyle.Regular),
            ForeColor = Color.FromArgb(100, 110, 125),
            TextAlign = ContentAlignment.MiddleRight,
            Margin = new Padding(0, 13, 8, 0)
        };

        titleRow.Controls.Add(_titleLabel);
        titleRow.Controls.Add(_breadcrumbLabel);

        var toolbar = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            AutoScroll = true,
            BackColor = Color.White,
            Margin = Padding.Empty,
            Padding = new Padding(0, 4, 0, 0),
            RightToLeft = RightToLeft.Yes
        };

        var btnNew = CreateActionButton("جديد", Color.FromArgb(28, 105, 225), Color.White);
        var btnSave = CreateActionButton("حفظ", Color.FromArgb(45, 157, 83), Color.White);
        var btnEdit = CreateActionButton("تعديل", Color.FromArgb(245, 145, 20), Color.White);
        var btnSearch = CreateActionButton("بحث", Color.FromArgb(28, 105, 225), Color.White);
        var btnFirst = CreateNavigationButton("|◀", "الأول");
        var btnPrevious = CreateNavigationButton("◀", "السابق");

        _recordLabel = new Label
        {
            Width = 76,
            Height = 36,
            Text = "0 / 0",
            TextAlign = ContentAlignment.MiddleCenter,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.FromArgb(245, 248, 252),
            ForeColor = Color.FromArgb(33, 45, 65),
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            Margin = new Padding(4, 0, 4, 0)
        };

        var btnNext = CreateNavigationButton("▶", "التالي");
        var btnLast = CreateNavigationButton("▶|", "الأخير");
        var btnDelete = CreateActionButton("حذف", Color.FromArgb(225, 55, 55), Color.White);
        var btnPrint = CreateActionButton("طباعة", Color.White, Color.FromArgb(33, 45, 65));
        var btnRefresh = CreateActionButton("تحديث", Color.White, Color.FromArgb(33, 45, 65));
        var btnClose = CreateActionButton("إغلاق", Color.White, Color.FromArgb(33, 45, 65));

        toolbar.Controls.AddRange(new Control[]
        {
            btnNew, btnSave, btnEdit, btnSearch,
            btnFirst, btnPrevious, _recordLabel, btnNext, btnLast,
            btnDelete, btnPrint, btnRefresh, btnClose
        });

        root.Controls.Add(titleRow, 0, 0);
        root.Controls.Add(toolbar, 0, 1);
        Controls.Add(root);
    }

    public string ScreenTitle
    {
        get => _titleLabel.Text;
        set => _titleLabel.Text = value;
    }

    public string Breadcrumb
    {
        get => _breadcrumbLabel.Text;
        set => _breadcrumbLabel.Text = value;
    }

    public string RecordPosition
    {
        get => _recordLabel.Text;
        set => _recordLabel.Text = value;
    }

    private static Button CreateActionButton(string text, Color backColor, Color foreColor)
    {
        return new Button
        {
            Width = 84,
            Height = 36,
            Text = text,
            FlatStyle = FlatStyle.Flat,
            BackColor = backColor,
            ForeColor = foreColor,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            Margin = new Padding(4, 0, 4, 0),
            UseVisualStyleBackColor = false
        };
    }

    private static Button CreateNavigationButton(string text, string accessibleName)
    {
        return new Button
        {
            Width = 50,
            Height = 36,
            Text = text,
            AccessibleName = accessibleName,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            ForeColor = Color.FromArgb(33, 45, 65),
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Margin = new Padding(4, 0, 4, 0),
            UseVisualStyleBackColor = false
        };
    }
}
