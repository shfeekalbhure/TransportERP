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
    // الحاوية الأفقية ترتب جميع أدوات البحث من اليمين إلى اليسار.
    private readonly FlowLayoutPanel _layout = new();

    // مربع البحث الموحد الموجود مسبقًا في CoreUI.
    private readonly SearchBox _searchBox = new();

    // عنوان ثابت لحقل الحالة حتى تكون التسمية واحدة في جميع الشاشات.
    private readonly Label _statusLabel = new();

    // قائمة الحالة الثابتة وتبدأ افتراضيًا بخيار "الكل".
    private readonly ComboBox _statusComboBox = new();

    // هذه الحاوية تستقبل أي فلاتر إضافية خاصة بالشاشة مثل الدولة أو الفرع.
    private readonly FlowLayoutPanel _extraFiltersHost = new();

    /// <summary>
    /// إنشاء حاوية البحث وتطبيق التخطيط الموحد.
    /// </summary>
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
    public string SearchPlaceholder
    {
        get => _searchBox.PlaceholderText;
        set => _searchBox.PlaceholderText = value;
    }

    /// <summary>
    /// استبدال عناصر فلتر الحالة عندما تكون للشاشة حالات مختلفة عن نشط وموقوف.
    /// </summary>
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

    /// <summary>إعادة البحث والتصفية إلى الوضع الافتراضي.</summary>
    public void ResetFilters()
    {
        _searchBox.ClearSearch();
        if (_statusComboBox.Items.Count > 0)
        {
            _statusComboBox.SelectedIndex = 0;
        }
    }

    /// <summary>
    /// إنشاء الحاوية بارتفاع 10 مم تقريبًا، وجميع الحقول داخلها بارتفاع 8 مم تقريبًا.
    /// </summary>
    private void InitializeLayout()
    {
        BackColor = Color.White;
        Dock = DockStyle.Fill;
        Height = TransportUiMetrics.Container10Mm;
        MinimumSize = new Size(0, TransportUiMetrics.Container10Mm);
        Padding = new Padding(8, 4, 8, 4);
        RightToLeft = RightToLeft.Yes;

        _layout.Dock = DockStyle.Fill;
        _layout.FlowDirection = FlowDirection.RightToLeft;
        _layout.RightToLeft = RightToLeft.Yes;
        _layout.WrapContents = false;

        _searchBox.Width = 300;
        _searchBox.Height = TransportUiMetrics.Control8Mm;
        _searchBox.Margin = new Padding(4, 0, 4, 0);
        _searchBox.SearchTextChanged += (_, e) => SearchTextChanged?.Invoke(this, e);

        _statusLabel.AutoSize = false;
        _statusLabel.Font = UiTheme.CreateRegularFont(9.5F);
        _statusLabel.Margin = new Padding(6, 0, 2, 0);
        _statusLabel.Size = new Size(54, TransportUiMetrics.Control8Mm);
        _statusLabel.Text = "الحالة:";
        _statusLabel.TextAlign = ContentAlignment.MiddleRight;

        _statusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _statusComboBox.Font = UiTheme.CreateRegularFont(9.5F);
        _statusComboBox.Margin = new Padding(2, 0, 6, 0);
        _statusComboBox.RightToLeft = RightToLeft.Yes;
        _statusComboBox.Size = new Size(140, TransportUiMetrics.Control8Mm);
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
