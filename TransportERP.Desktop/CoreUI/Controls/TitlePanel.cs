using System.ComponentModel;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// رأس الشاشة الموحد في نظام TransportERP.
/// يعرض عنوان الشاشة ووصفها المختصر ومسار التنقل، ويمكن استخدامه أعلى جميع شاشات النظام.
/// </summary>
[ToolboxItem(true)]
public sealed class TitlePanel : UserControl
{
    private readonly TableLayoutPanel _layout = new();
    private readonly Label _titleLabel = new();
    private readonly Label _descriptionLabel = new();
    private readonly Label _breadcrumbLabel = new();

    /// <summary>
    /// إنشاء رأس الشاشة وتطبيق الهوية البصرية المعتمدة.
    /// </summary>
    public TitlePanel()
    {
        InitializeLayout();
        ApplyDefaultValues();
    }

    /// <summary>
    /// عنوان الشاشة الرئيسي.
    /// </summary>
    [Category("TransportERP")]
    [Description("العنوان الرئيسي المعروض أعلى الشاشة.")]
    [DefaultValue("عنوان الشاشة")]
    public string TitleText
    {
        get => _titleLabel.Text;
        set => _titleLabel.Text = string.IsNullOrWhiteSpace(value)
            ? "عنوان الشاشة"
            : value.Trim();
    }

    /// <summary>
    /// الوصف المختصر لوظيفة الشاشة.
    /// </summary>
    [Category("TransportERP")]
    [Description("الوصف المختصر المعروض أسفل عنوان الشاشة.")]
    [DefaultValue("وصف مختصر لوظيفة الشاشة")]
    public string DescriptionText
    {
        get => _descriptionLabel.Text;
        set => _descriptionLabel.Text = string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : value.Trim();
    }

    /// <summary>
    /// مسار التنقل داخل النظام.
    /// </summary>
    [Category("TransportERP")]
    [Description("مسار التنقل المعروض في رأس الشاشة، مثل: التهيئة العامة / الدول.")]
    [DefaultValue("")]
    public string BreadcrumbText
    {
        get => _breadcrumbLabel.Text;
        set
        {
            _breadcrumbLabel.Text = string.IsNullOrWhiteSpace(value)
                ? string.Empty
                : value.Trim();

            _breadcrumbLabel.Visible = !string.IsNullOrWhiteSpace(_breadcrumbLabel.Text);
        }
    }

    /// <summary>
    /// تحديث بيانات رأس الشاشة دفعة واحدة.
    /// </summary>
    /// <param name="title">عنوان الشاشة.</param>
    /// <param name="description">الوصف المختصر.</param>
    /// <param name="breadcrumb">مسار التنقل.</param>
    public void SetContent(string title, string? description = null, string? breadcrumb = null)
    {
        TitleText = title;
        DescriptionText = description ?? string.Empty;
        BreadcrumbText = breadcrumb ?? string.Empty;
    }

    /// <summary>
    /// تهيئة ترتيب العناصر داخل رأس الشاشة.
    /// </summary>
    private void InitializeLayout()
    {
        AutoSize = false;
        BackColor = Color.White;
        Dock = DockStyle.Top;
        Height = 108;
        MinimumSize = new Size(0, 96);
        Padding = new Padding(24, 14, 24, 12);
        RightToLeft = RightToLeft.Yes;

        _layout.ColumnCount = 2;
        _layout.Dock = DockStyle.Fill;
        _layout.RowCount = 2;
        _layout.BackColor = Color.Transparent;
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72F));
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
        _layout.RowStyles.Add(new RowStyle(SizeType.Percent, 56F));
        _layout.RowStyles.Add(new RowStyle(SizeType.Percent, 44F));

        _titleLabel.Dock = DockStyle.Fill;
        _titleLabel.Font = UiTheme.CreateBoldFont(20F);
        _titleLabel.ForeColor = UiTheme.HeadingText;
        _titleLabel.TextAlign = ContentAlignment.MiddleRight;
        _titleLabel.AutoEllipsis = true;

        _descriptionLabel.Dock = DockStyle.Fill;
        _descriptionLabel.Font = UiTheme.CreateRegularFont(10F);
        _descriptionLabel.ForeColor = UiTheme.SecondaryText;
        _descriptionLabel.TextAlign = ContentAlignment.TopRight;
        _descriptionLabel.AutoEllipsis = true;

        _breadcrumbLabel.Dock = DockStyle.Fill;
        _breadcrumbLabel.Font = UiTheme.CreateRegularFont(9F);
        _breadcrumbLabel.ForeColor = UiTheme.PrimaryBlue;
        _breadcrumbLabel.TextAlign = ContentAlignment.MiddleLeft;
        _breadcrumbLabel.AutoEllipsis = true;

        _layout.Controls.Add(_titleLabel, 0, 0);
        _layout.Controls.Add(_descriptionLabel, 0, 1);
        _layout.Controls.Add(_breadcrumbLabel, 1, 0);
        _layout.SetRowSpan(_breadcrumbLabel, 2);

        Controls.Add(_layout);
    }

    /// <summary>
    /// وضع قيم افتراضية واضحة قبل تخصيص العنصر داخل الشاشة.
    /// </summary>
    private void ApplyDefaultValues()
    {
        TitleText = "عنوان الشاشة";
        DescriptionText = "وصف مختصر لوظيفة الشاشة";
        BreadcrumbText = string.Empty;
    }
}
