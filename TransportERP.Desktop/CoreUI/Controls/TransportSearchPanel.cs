using System.ComponentModel;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// حاوية البحث والتصفية الموحدة لكل شاشة تعرض قائمة سجلات.
/// تحتوي دائمًا على البحث والحالة، وتوفر مساحة إضافية لفلاتر الشاشة الخاصة.
/// الهدف منها منع تكرار نفس أدوات البحث والحالة في كل Designer.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportSearchPanel : UserControl
{
    private readonly FlowLayoutPanel _layout = new();
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

    /// <summary>
    /// النص الإرشادي داخل مربع البحث.
    /// تعريف DefaultValue يمنع تحذير WFO1000 ويجعل مصمم WinForms يعرف متى يحتاج إلى حفظ القيمة.
    /// </summary>
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
        BackColor = Color.White;
        Dock = DockStyle.Fill;
        Height = TransportUiMetrics.SearchPanelHeight;
        MinimumSize = new Size(0, TransportUiMetrics.SearchPanelHeight);
        Padding = new Padding(8, 4, 8, 4);
        RightToLeft = RightToLeft.Yes;

        _layout.Dock = DockStyle.Fill;
        _layout.FlowDirection = FlowDirection.RightToLeft;
        _layout.RightToLeft = RightToLeft.Yes;
        _layout.WrapContents = false;

        _searchBox.Width = 300;
        _searchBox.Height = TransportUiMetrics.SearchControlHeight;
        _searchBox.Margin = new Padding(4, 0, 4, 0);
        _searchBox.SearchTextChanged += (_, e) => SearchTextChanged?.Invoke(this, e);

        _statusLabel.AutoSize = false;
        _statusLabel.Font = UiTheme.CreateRegularFont(9.5F);
        _statusLabel.Margin = new Padding(6, 0, 2, 0);
        _statusLabel.Size = new Size(54, TransportUiMetrics.SearchControlHeight);
        _statusLabel.Text = "الحالة:";
        _statusLabel.TextAlign = ContentAlignment.MiddleRight;

        _statusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _statusComboBox.Font = UiTheme.CreateRegularFont(9.5F);
        _statusComboBox.Margin = new Padding(2, 0, 6, 0);
        _statusComboBox.RightToLeft = RightToLeft.Yes;
        _statusComboBox.Size = new Size(140, TransportUiMetrics.SearchControlHeight);
        _statusComboBox.Items.AddRange(new object[] { "الكل", "نشط", "موقوف" });
        _statusComboBox.SelectedIndex = 0;
        _statusComboBox.SelectedIndexChanged += (_, _) => StatusChanged?.Invoke(this, EventArgs.Empty);

        _extraFiltersHost.AutoSize = true;
        _extraFiltersHost.FlowDirection = FlowDirection.RightToLeft;
        _extraFiltersHost.Margin = Padding.Empty;
        _extraFiltersHost.RightToLeft = RightToLeft.Yes;
        _extraFiltersHost.WrapContents = false;

        _layout.Controls.Add(_searchBox);
        _layout.Controls.Add(_statusLabel);
        _layout.Controls.Add(_statusComboBox);
        _layout.Controls.Add(_extraFiltersHost);
        Controls.Add(_layout);
    }
}
