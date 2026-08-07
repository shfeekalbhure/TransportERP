using System.ComponentModel;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// عنصر التصفح الموحد بين صفحات السجلات.
/// يستخدم نفس الأسماء والترتيب في كل شاشة: الأول، السابق، رقم الصفحة، التالي، الأخير.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportPagination : UserControl
{
    // الحاوية التي تضع أزرار التصفح في صف واحد ثابت.
    private readonly FlowLayoutPanel _layout = new();

    // الأزرار التالية ثابتة في جميع الشاشات ولا يعاد إنشاؤها داخل كل Designer.
    private readonly Button _firstButton = CreateButton("الأول");
    private readonly Button _previousButton = CreateButton("السابق");
    private readonly Label _pageLabel = new();
    private readonly Button _nextButton = CreateButton("التالي");
    private readonly Button _lastButton = CreateButton("الأخير");
    private readonly Label _recordsLabel = new();

    private int _currentPage = 1;
    private int _totalPages = 1;

    /// <summary>
    /// إنشاء عنصر التصفح وربط أزراره بأحداث عامة تستخدمها أي شاشة.
    /// </summary>
    public TransportPagination()
    {
        InitializeLayout();
        RegisterEvents();
        SetPageInfo(1, 1, 0, 0, 0);
    }

    public event EventHandler? FirstRequested;
    public event EventHandler? PreviousRequested;
    public event EventHandler? NextRequested;
    public event EventHandler? LastRequested;

    /// <summary>
    /// رقم الصفحة الحالية المعروض للمستخدم.
    /// </summary>
    [Browsable(false)]
    public int CurrentPage => _currentPage;

    /// <summary>
    /// إجمالي عدد الصفحات.
    /// </summary>
    [Browsable(false)]
    public int TotalPages => _totalPages;

    /// <summary>
    /// تحديث معلومات الصفحة والعداد السفلي بعد جلب البيانات من API.
    /// </summary>
    /// <param name="currentPage">الصفحة الحالية.</param>
    /// <param name="totalPages">إجمالي الصفحات.</param>
    /// <param name="fromRecord">رقم أول سجل ظاهر.</param>
    /// <param name="toRecord">رقم آخر سجل ظاهر.</param>
    /// <param name="totalRecords">إجمالي عدد السجلات.</param>
    public void SetPageInfo(int currentPage, int totalPages, int fromRecord, int toRecord, int totalRecords)
    {
        _currentPage = Math.Max(1, currentPage);
        _totalPages = Math.Max(1, totalPages);

        _pageLabel.Text = $"{_currentPage} / {_totalPages}";
        _recordsLabel.Text = totalRecords <= 0
            ? "لا توجد سجلات"
            : $"عرض {Math.Max(1, fromRecord)} - {Math.Max(fromRecord, toRecord)} من {totalRecords}";

        // تعطيل الأزرار غير الممكنة يمنع إرسال طلبات تصفح غير صحيحة.
        _firstButton.Enabled = _currentPage > 1;
        _previousButton.Enabled = _currentPage > 1;
        _nextButton.Enabled = _currentPage < _totalPages;
        _lastButton.Enabled = _currentPage < _totalPages;
    }

    /// <summary>
    /// تجهيز الشكل الموحد ومحاذاة التصفح في منتصف الشاشة.
    /// </summary>
    private void InitializeLayout()
    {
        BackColor = Color.White;
        Dock = DockStyle.Top;
        Height = 52;
        MinimumSize = new Size(0, 52);
        RightToLeft = RightToLeft.Yes;

        _layout.AutoSize = true;
        _layout.Anchor = AnchorStyles.None;
        _layout.FlowDirection = FlowDirection.RightToLeft;
        _layout.RightToLeft = RightToLeft.Yes;
        _layout.WrapContents = false;

        _pageLabel.AutoSize = false;
        _pageLabel.Font = UiTheme.CreateBoldFont(10F);
        _pageLabel.Margin = new Padding(6, 4, 6, 4);
        _pageLabel.Size = new Size(82, 34);
        _pageLabel.TextAlign = ContentAlignment.MiddleCenter;

        _recordsLabel.AutoSize = false;
        _recordsLabel.Font = UiTheme.CreateRegularFont(9.5F);
        _recordsLabel.ForeColor = UiTheme.SecondaryText;
        _recordsLabel.Margin = new Padding(18, 4, 6, 4);
        _recordsLabel.Size = new Size(190, 34);
        _recordsLabel.TextAlign = ContentAlignment.MiddleRight;

        _layout.Controls.Add(_firstButton);
        _layout.Controls.Add(_previousButton);
        _layout.Controls.Add(_pageLabel);
        _layout.Controls.Add(_nextButton);
        _layout.Controls.Add(_lastButton);
        _layout.Controls.Add(_recordsLabel);

        // استخدام TableLayoutPanel يجعل مجموعة الأزرار في المنتصف مهما تغير عرض الشاشة.
        var centeringPanel = new TableLayoutPanel
        {
            ColumnCount = 3,
            Dock = DockStyle.Fill,
            RightToLeft = RightToLeft.Yes
        };
        centeringPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        centeringPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        centeringPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        centeringPanel.Controls.Add(_layout, 1, 0);

        Controls.Add(centeringPanel);
    }

    /// <summary>
    /// ربط النقر على الأزرار بأحداث عامة بدل كتابة نفس الأحداث في كل شاشة.
    /// </summary>
    private void RegisterEvents()
    {
        _firstButton.Click += (_, _) => FirstRequested?.Invoke(this, EventArgs.Empty);
        _previousButton.Click += (_, _) => PreviousRequested?.Invoke(this, EventArgs.Empty);
        _nextButton.Click += (_, _) => NextRequested?.Invoke(this, EventArgs.Empty);
        _lastButton.Click += (_, _) => LastRequested?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// إنشاء زر تصفح بنفس الحجم والخط لجميع الشاشات.
    /// </summary>
    private static Button CreateButton(string text) => new()
    {
        AutoSize = false,
        FlatStyle = FlatStyle.System,
        Font = UiTheme.CreateRegularFont(9.5F),
        Margin = new Padding(4),
        Size = new Size(78, 34),
        Text = text,
        UseVisualStyleBackColor = true
    };
}
