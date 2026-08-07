using System.ComponentModel;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// حاوية البحث والتصفية الموحدة لكل شاشة تعرض قائمة سجلات.
/// الأدوات تبدأ من اليمين، ويأخذ مربع البحث المساحة المرنة بينما تبقى الحالة والفلاتر الصغيرة بمقاسات منضبطة.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportSearchPanel : UserControl
{
    private readonly TableLayoutPanel _layout = new();
    private readonly SearchBox _searchBox = new();
    private readonly Label _statusLabel = new();
    private readonly ComboBox _statusComboBox = new();
    private readonly FlowLayoutPanel _extraFiltersHost = new();

    public TransportSearchPanel()
    {
        InitializeLayout();
    }

    public event EventHandler<SearchTextChangedEventArgs>? SearchTextChanged;
    public event EventHandler? StatusChanged;

    [Browsable(false)]
    public string SearchText => _searchBox.SearchText;

    [Browsable(false)]
    public string SelectedStatus => _statusComboBox.SelectedItem?.ToString() ?? "الكل";

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Category("TransportERP")]
    [Description("الحاوية المخصصة لإضافة فلاتر الشاشة الإضافية.")]
    public FlowLayoutPanel ExtraFiltersHost => _extraFiltersHost;

    [Category("TransportERP")]
    [Description("النص الإرشادي داخل مربع البحث.")]
    [DefaultValue("بحث...")]
    public string SearchPlaceholder
    {
        get => _searchBox.PlaceholderText;
        set => _searchBox.PlaceholderText = value;
    }

    public void SetStatusItems(params string[] items)
    {
        _statusComboBox.Items.Clear();
        _statusComboBox.Items.Add("الكل");

        foreach (var item in items.Where(item => !string.IsNullOrWhiteSpace(item)))
        {
            _statusComboBox.Items.Add(item.Trim());
        }

        _statusComboBox.SelectedIndex = 0;
    }

    public void ResetFilters()
    {
        _searchBox.ClearSearch();
        if (_statusComboBox.Items.Count > 0)
        {
            _statusComboBox.SelectedIndex = 0;
        }
    }

    private void InitializeLayout()
    {
        BackColor = UiTheme.SurfaceBackground;
        Dock = DockStyle.Fill;
        Height = TransportUiMetrics.SearchPanelHeight;
        MinimumSize = new Size(0, TransportUiMetrics.SearchPanelHeight);
        Padding = new Padding(
            TransportUiMetrics.GroupHorizontalPadding,
            TransportUiMetrics.CompactPadding,
            TransportUiMetrics.GroupHorizontalPadding,
            TransportUiMetrics.CompactPadding);
        RightToLeft = RightToLeft.Yes;

        // التخطيط الداخلي هو المسؤول عن الاستجابة لتغير العرض؛ الحاوية نفسها لا تحتاج مقاسات محلية لكل شاشة.
        _layout.ColumnCount = 4;
        _layout.RowCount = 1;
        _layout.Dock = DockStyle.Fill;
        _layout.Margin = Padding.Empty;
        _layout.Padding = Padding.Empty;
        _layout.RightToLeft = RightToLeft.Yes;
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, TransportUiMetrics.SearchStatusWidth));
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, TransportUiMetrics.SearchStatusLabelWidth));
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        _layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        _statusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _statusComboBox.Font = UiTheme.CreateRegularFont(9.5F);
        _statusComboBox.Margin = new Padding(2, TransportUiMetrics.SearchStatusVerticalMargin, 2, TransportUiMetrics.SearchStatusVerticalMargin);
        _statusComboBox.RightToLeft = RightToLeft.Yes;
        _statusComboBox.Dock = DockStyle.Fill;
        _statusComboBox.MinimumSize = new Size(0, TransportUiMetrics.SearchStatusControlHeight);
        _statusComboBox.Items.AddRange(new object[] { "الكل", "نشط", "موقوف" });
        _statusComboBox.SelectedIndex = 0;
        _statusComboBox.SelectedIndexChanged += (_, _) => StatusChanged?.Invoke(this, EventArgs.Empty);

        _statusLabel.AutoSize = false;
        _statusLabel.Dock = DockStyle.Fill;
        _statusLabel.Font = UiTheme.CreateRegularFont(9.5F);
        _statusLabel.Margin = new Padding(2, TransportUiMetrics.SearchStatusVerticalMargin, 2, TransportUiMetrics.SearchStatusVerticalMargin);
        _statusLabel.MinimumSize = new Size(0, TransportUiMetrics.SearchStatusControlHeight);
        _statusLabel.Text = "الحالة:";
        _statusLabel.TextAlign = ContentAlignment.MiddleRight;

        // مربع البحث هو العنصر المرن: يتمدد أو يصغر أفقيًا داخل المساحة المتبقية ولا يتجاوز الحد الأدنى المريح.
        _searchBox.Dock = DockStyle.Fill;
        _searchBox.Height = TransportUiMetrics.SearchControlHeight;
        _searchBox.MinimumSize = new Size(TransportUiMetrics.SearchMinimumWidth, TransportUiMetrics.SearchControlHeight);
        _searchBox.Margin = new Padding(TransportUiMetrics.MainDataHorizontalMargin, 0, TransportUiMetrics.MainDataHorizontalMargin, 0);
        _searchBox.RightToLeft = RightToLeft.Yes;
        _searchBox.SearchTextChanged += (_, e) => SearchTextChanged?.Invoke(this, e);

        _extraFiltersHost.AutoSize = true;
        _extraFiltersHost.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _extraFiltersHost.Dock = DockStyle.Fill;
        _extraFiltersHost.FlowDirection = FlowDirection.RightToLeft;
        _extraFiltersHost.Margin = Padding.Empty;
        _extraFiltersHost.Padding = Padding.Empty;
        _extraFiltersHost.RightToLeft = RightToLeft.Yes;
        _extraFiltersHost.WrapContents = false;
        _extraFiltersHost.MinimumSize = new Size(0, TransportUiMetrics.SearchControlHeight);

        _layout.Controls.Add(_statusComboBox, 0, 0);
        _layout.Controls.Add(_statusLabel, 1, 0);
        _layout.Controls.Add(_searchBox, 2, 0);
        _layout.Controls.Add(_extraFiltersHost, 3, 0);

        Controls.Add(_layout);
    }
}
