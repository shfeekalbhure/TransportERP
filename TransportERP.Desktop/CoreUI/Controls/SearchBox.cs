using System.ComponentModel;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// مربع البحث الموحد في نظام TransportERP.
/// يوفر بحثًا فوريًا، ونصًا إرشاديًا، وزرًا لمسح البحث، وحدثًا موحدًا لإرسال قيمة البحث للشاشة المستضيفة.
/// </summary>
[ToolboxItem(true)]
public sealed class SearchBox : UserControl
{
    private readonly TextBox _searchTextBox = new();
    private readonly Label _searchIconLabel = new();
    private readonly Label _clearLabel = new();
    private string _placeholderText = "بحث...";
    private bool _isShowingPlaceholder = true;

    /// <summary>
    /// إنشاء مربع البحث وتطبيق الهوية البصرية المعتمدة.
    /// </summary>
    public SearchBox()
    {
        InitializeLayout();
        RegisterEvents();
        ShowPlaceholder();
    }

    public event EventHandler<SearchTextChangedEventArgs>? SearchTextChanged;

    [Category("TransportERP")]
    [Description("النص الإرشادي المعروض داخل مربع البحث عندما لا توجد قيمة.")]
    [DefaultValue("بحث...")]
    public string PlaceholderText
    {
        get => _placeholderText;
        set
        {
            _placeholderText = string.IsNullOrWhiteSpace(value) ? "بحث..." : value.Trim();
            if (_isShowingPlaceholder)
            {
                ShowPlaceholder();
            }
        }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string SearchText
    {
        get => _isShowingPlaceholder ? string.Empty : _searchTextBox.Text.Trim();
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                ClearSearch();
                return;
            }

            _isShowingPlaceholder = false;
            _searchTextBox.ForeColor = UiTheme.HeadingText;
            _searchTextBox.Text = value.Trim();
            _searchTextBox.SelectionStart = _searchTextBox.TextLength;
            UpdateClearVisibility();
        }
    }

    /// <summary>مسح قيمة البحث وإعادة النص الإرشادي.</summary>
    public void ClearSearch()
    {
        ShowPlaceholder();
        RaiseSearchTextChanged();
    }

    /// <summary>نقل التركيز إلى حقل البحث.</summary>
    public void FocusSearch() => _searchTextBox.Focus();

    /// <summary>
    /// تهيئة مربع البحث بارتفاع 8 مم تقريبًا حتى يطابق بقية حقول البحث والتصفية.
    /// </summary>
    private void InitializeLayout()
    {
        BackColor = Color.White;
        BorderStyle = BorderStyle.FixedSingle;
        Height = TransportUiMetrics.Control8Mm;
        MinimumSize = new Size(180, TransportUiMetrics.Control8Mm);
        Padding = new Padding(6, 4, 6, 4);
        RightToLeft = RightToLeft.Yes;

        _searchIconLabel.AutoSize = false;
        _searchIconLabel.Dock = DockStyle.Right;
        _searchIconLabel.Font = UiTheme.CreateRegularFont(11F);
        _searchIconLabel.ForeColor = UiTheme.SecondaryText;
        _searchIconLabel.Text = "⌕";
        _searchIconLabel.TextAlign = ContentAlignment.MiddleCenter;
        _searchIconLabel.Width = 24;

        _clearLabel.AutoSize = false;
        _clearLabel.Cursor = Cursors.Hand;
        _clearLabel.Dock = DockStyle.Left;
        _clearLabel.Font = UiTheme.CreateBoldFont(9.5F);
        _clearLabel.ForeColor = UiTheme.SecondaryText;
        _clearLabel.Text = "×";
        _clearLabel.TextAlign = ContentAlignment.MiddleCenter;
        _clearLabel.Visible = false;
        _clearLabel.Width = 24;

        _searchTextBox.BackColor = Color.White;
        _searchTextBox.BorderStyle = BorderStyle.None;
        _searchTextBox.Dock = DockStyle.Fill;
        _searchTextBox.Font = UiTheme.CreateRegularFont(9.5F);
        _searchTextBox.ForeColor = UiTheme.HeadingText;
        _searchTextBox.RightToLeft = RightToLeft.Yes;
        _searchTextBox.TextAlign = HorizontalAlignment.Right;

        Controls.Add(_searchTextBox);
        Controls.Add(_clearLabel);
        Controls.Add(_searchIconLabel);
    }

    /// <summary>تسجيل أحداث التركيز والكتابة ومسح البحث.</summary>
    private void RegisterEvents()
    {
        _searchTextBox.Enter += HandleEnter;
        _searchTextBox.Leave += HandleLeave;
        _searchTextBox.TextChanged += HandleTextChanged;
        _searchTextBox.KeyDown += HandleKeyDown;
        _clearLabel.Click += (_, _) => ClearSearch();
    }

    private void HandleEnter(object? sender, EventArgs e)
    {
        BackColor = UiTheme.FocusedInputBackground;
        _searchTextBox.BackColor = UiTheme.FocusedInputBackground;

        if (_isShowingPlaceholder)
        {
            _isShowingPlaceholder = false;
            _searchTextBox.Clear();
            _searchTextBox.ForeColor = UiTheme.HeadingText;
        }
    }

    private void HandleLeave(object? sender, EventArgs e)
    {
        BackColor = Color.White;
        _searchTextBox.BackColor = Color.White;
        if (string.IsNullOrWhiteSpace(_searchTextBox.Text))
        {
            ShowPlaceholder();
        }
    }

    private void HandleTextChanged(object? sender, EventArgs e)
    {
        if (_isShowingPlaceholder) return;
        UpdateClearVisibility();
        RaiseSearchTextChanged();
    }

    private void HandleKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Escape) return;
        ClearSearch();
        e.SuppressKeyPress = true;
    }

    private void ShowPlaceholder()
    {
        _isShowingPlaceholder = true;
        _searchTextBox.ForeColor = UiTheme.SecondaryText;
        _searchTextBox.Text = _placeholderText;
        _clearLabel.Visible = false;
    }

    private void UpdateClearVisibility() =>
        _clearLabel.Visible = !_isShowingPlaceholder && !string.IsNullOrWhiteSpace(_searchTextBox.Text);

    private void RaiseSearchTextChanged() =>
        SearchTextChanged?.Invoke(this, new SearchTextChangedEventArgs(SearchText));
}

/// <summary>بيانات حدث تغير نص البحث.</summary>
public sealed class SearchTextChangedEventArgs : EventArgs
{
    public SearchTextChangedEventArgs(string searchText) => SearchText = searchText;
    public string SearchText { get; }
}
