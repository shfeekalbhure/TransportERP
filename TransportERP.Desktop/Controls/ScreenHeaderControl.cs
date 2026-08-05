using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

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

    private readonly Button _btnNew;
    private readonly Button _btnSave;
    private readonly Button _btnEdit;
    private readonly Button _btnSearch;
    private readonly Button _btnFirst;
    private readonly Button _btnPrevious;
    private readonly Button _btnNext;
    private readonly Button _btnLast;
    private readonly Button _btnDelete;
    private readonly Button _btnPrint;
    private readonly Button _btnRefresh;
    private readonly Button _btnClose;

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
            Font = new Font("Segoe UI", 10F),
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

        _btnNew = CreateActionButton("جديد", Color.FromArgb(28, 105, 225), Color.White);
        _btnSave = CreateActionButton("حفظ", Color.FromArgb(45, 157, 83), Color.White);
        _btnEdit = CreateActionButton("تعديل", Color.FromArgb(245, 145, 20), Color.White);
        _btnSearch = CreateActionButton("بحث", Color.FromArgb(28, 105, 225), Color.White);

        _btnFirst = CreateNavigationButton("|◀", "الأول");
        _btnPrevious = CreateNavigationButton("◀", "السابق");

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

        _btnNext = CreateNavigationButton("▶", "التالي");
        _btnLast = CreateNavigationButton("▶|", "الأخير");

        _btnDelete = CreateActionButton("حذف", Color.FromArgb(225, 55, 55), Color.White);
        _btnPrint = CreateActionButton("طباعة", Color.White, Color.FromArgb(33, 45, 65));
        _btnRefresh = CreateActionButton("تحديث", Color.White, Color.FromArgb(33, 45, 65));
        _btnClose = CreateActionButton("إغلاق", Color.White, Color.FromArgb(33, 45, 65));

        _btnNew.Click += (_, e) => NewClicked?.Invoke(this, e);
        _btnSave.Click += (_, e) => SaveClicked?.Invoke(this, e);
        _btnEdit.Click += (_, e) => EditClicked?.Invoke(this, e);
        _btnSearch.Click += (_, e) => SearchClicked?.Invoke(this, e);
        _btnFirst.Click += (_, e) => FirstClicked?.Invoke(this, e);
        _btnPrevious.Click += (_, e) => PreviousClicked?.Invoke(this, e);
        _btnNext.Click += (_, e) => NextClicked?.Invoke(this, e);
        _btnLast.Click += (_, e) => LastClicked?.Invoke(this, e);
        _btnDelete.Click += (_, e) => DeleteClicked?.Invoke(this, e);
        _btnPrint.Click += (_, e) => PrintClicked?.Invoke(this, e);
        _btnRefresh.Click += (_, e) => RefreshClicked?.Invoke(this, e);
        _btnClose.Click += (_, e) => CloseClicked?.Invoke(this, e);

        toolbar.Controls.AddRange(
        [
            _btnNew, _btnSave, _btnEdit, _btnSearch,
            _btnFirst, _btnPrevious, _recordLabel, _btnNext, _btnLast,
            _btnDelete, _btnPrint, _btnRefresh, _btnClose
        ]);

        root.Controls.Add(titleRow, 0, 0);
        root.Controls.Add(toolbar, 0, 1);

        Controls.Add(root);

        SetNavigationState(false, true, true);
    }

    public event EventHandler? NewClicked;
    public event EventHandler? SaveClicked;
    public event EventHandler? EditClicked;
    public event EventHandler? SearchClicked;
    public event EventHandler? FirstClicked;
    public event EventHandler? PreviousClicked;
    public event EventHandler? NextClicked;
    public event EventHandler? LastClicked;
    public event EventHandler? DeleteClicked;
    public event EventHandler? PrintClicked;
    public event EventHandler? RefreshClicked;
    public event EventHandler? CloseClicked;

    [Category("TransportERP")]
    [Description("اسم الشاشة الظاهر في رأس النافذة.")]
    [DefaultValue("اسم الشاشة")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string ScreenTitle
    {
        get => _titleLabel.Text;
        set => _titleLabel.Text = value ?? string.Empty;
    }

    [Category("TransportERP")]
    [Description("المسار التعريفي للشاشة.")]
    [DefaultValue("المسار التعريفي")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Breadcrumb
    {
        get => _breadcrumbLabel.Text;
        set => _breadcrumbLabel.Text = value ?? string.Empty;
    }

    [Category("TransportERP")]
    [Description("موضع السجل الحالي، مثال: 1 / 25.")]
    [DefaultValue("0 / 0")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string RecordPosition
    {
        get => _recordLabel.Text;
        set => _recordLabel.Text = value ?? string.Empty;
    }

    public void SetNavigationState(bool hasRecords, bool isFirst, bool isLast)
    {
        _btnFirst.Enabled = hasRecords && !isFirst;
        _btnPrevious.Enabled = hasRecords && !isFirst;
        _btnNext.Enabled = hasRecords && !isLast;
        _btnLast.Enabled = hasRecords && !isLast;
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