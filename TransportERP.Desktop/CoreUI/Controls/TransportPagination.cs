using System.ComponentModel;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// عنصر التصفح الموحد بين صفحات السجلات.
/// يعرض أسهمًا مختصرة في مساحة صغيرة حتى يظهر بجانب حاوية الإشعارات أعلى الشاشة.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportPagination : UserControl
{
    private readonly FlowLayoutPanel _layout = new();
    private readonly Button _firstButton = CreateButton("⏮", "الأول");
    private readonly Button _previousButton = CreateButton("◀", "السابق");
    private readonly Label _pageLabel = new();
    private readonly Button _nextButton = CreateButton("▶", "التالي");
    private readonly Button _lastButton = CreateButton("⏭", "الأخير");
    private readonly Label _recordsLabel = new();
    private readonly ToolTip _toolTip = new();

    private int _currentPage = 1;
    private int _totalPages = 1;

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

    [Browsable(false)] public int CurrentPage => _currentPage;
    [Browsable(false)] public int TotalPages => _totalPages;

    public void SetPageInfo(int currentPage, int totalPages, int fromRecord, int toRecord, int totalRecords)
    {
        _currentPage = Math.Max(1, currentPage);
        _totalPages = Math.Max(1, totalPages);
        _pageLabel.Text = $"{_currentPage}/{_totalPages}";
        _recordsLabel.Text = totalRecords <= 0 ? "0 سجل" : $"{totalRecords} سجل";

        _firstButton.Enabled = _currentPage > 1;
        _previousButton.Enabled = _currentPage > 1;
        _nextButton.Enabled = _currentPage < _totalPages;
        _lastButton.Enabled = _currentPage < _totalPages;
    }

    private void InitializeLayout()
    {
        BackColor = Color.White;
        Dock = DockStyle.Fill;
        Height = TransportUiMetrics.PaginationHeight;
        MinimumSize = new Size(0, TransportUiMetrics.PaginationHeight);
        RightToLeft = RightToLeft.Yes;

        _layout.AutoSize = true;
        _layout.Anchor = AnchorStyles.None;
        _layout.FlowDirection = FlowDirection.RightToLeft;
        _layout.RightToLeft = RightToLeft.Yes;
        _layout.WrapContents = false;

        _pageLabel.AutoSize = false;
        _pageLabel.Font = UiTheme.CreateBoldFont(9F);
        _pageLabel.Margin = new Padding(3, 0, 3, 0);
        _pageLabel.Size = new Size(50, TransportUiMetrics.PaginationButtonHeight);
        _pageLabel.TextAlign = ContentAlignment.MiddleCenter;

        _recordsLabel.AutoSize = false;
        _recordsLabel.Font = UiTheme.CreateRegularFont(8.5F);
        _recordsLabel.ForeColor = UiTheme.SecondaryText;
        _recordsLabel.Margin = new Padding(6, 0, 2, 0);
        _recordsLabel.Size = new Size(72, TransportUiMetrics.PaginationButtonHeight);
        _recordsLabel.TextAlign = ContentAlignment.MiddleRight;

        _toolTip.SetToolTip(_firstButton, "الأول");
        _toolTip.SetToolTip(_previousButton, "السابق");
        _toolTip.SetToolTip(_nextButton, "التالي");
        _toolTip.SetToolTip(_lastButton, "الأخير");

        _layout.Controls.Add(_firstButton);
        _layout.Controls.Add(_previousButton);
        _layout.Controls.Add(_pageLabel);
        _layout.Controls.Add(_nextButton);
        _layout.Controls.Add(_lastButton);
        _layout.Controls.Add(_recordsLabel);

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

    private void RegisterEvents()
    {
        _firstButton.Click += (_, _) => FirstRequested?.Invoke(this, EventArgs.Empty);
        _previousButton.Click += (_, _) => PreviousRequested?.Invoke(this, EventArgs.Empty);
        _nextButton.Click += (_, _) => NextRequested?.Invoke(this, EventArgs.Empty);
        _lastButton.Click += (_, _) => LastRequested?.Invoke(this, EventArgs.Empty);
    }

    private static Button CreateButton(string symbol, string accessibleName) => new()
    {
        AccessibleName = accessibleName,
        AutoSize = false,
        FlatStyle = FlatStyle.System,
        Font = UiTheme.CreateBoldFont(9F),
        Margin = new Padding(2, 0, 2, 0),
        Size = new Size(34, TransportUiMetrics.PaginationButtonHeight),
        Text = symbol,
        UseVisualStyleBackColor = true
    };
}
