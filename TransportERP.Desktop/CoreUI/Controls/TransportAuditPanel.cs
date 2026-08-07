using System.ComponentModel;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// حاوية معلومات الإنشاء والتعديل والطباعة الموحدة لكل شاشة.
/// تحفظ نفس التسمية ونفس ترتيب المعلومات في جميع الشاشات،
/// وتمنع تكرار Labels مختلفة أو أسماء مختلفة لنفس البيانات.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportAuditPanel : UserControl
{
    // القيم التالية هي النصوص التي تتغير حسب السجل الحالي.
    private readonly Label _createdByValue = CreateValueLabel();
    private readonly Label _createdAtValue = CreateValueLabel();
    private readonly Label _modifiedByValue = CreateValueLabel();
    private readonly Label _modifiedAtValue = CreateValueLabel();
    private readonly Label _editCountValue = CreateValueLabel();
    private readonly Label _printCountValue = CreateValueLabel();

    /// <summary>
    /// إنشاء الحاوية ووضع جميع معلومات التدقيق بنفس الترتيب المعتمد.
    /// </summary>
    public TransportAuditPanel()
    {
        InitializeLayout();
        ClearAuditInfo();
    }

    /// <summary>
    /// تعبئة معلومات السجل الحالي بعد تحميله من API.
    /// </summary>
    public void SetAuditInfo(
        string? createdBy,
        DateTime? createdAt,
        string? modifiedBy,
        DateTime? modifiedAt,
        int editCount,
        int printCount)
    {
        _createdByValue.Text = Normalize(createdBy);
        _createdAtValue.Text = FormatDate(createdAt);
        _modifiedByValue.Text = Normalize(modifiedBy);
        _modifiedAtValue.Text = FormatDate(modifiedAt);
        _editCountValue.Text = Math.Max(0, editCount).ToString();
        _printCountValue.Text = Math.Max(0, printCount).ToString();
    }

    /// <summary>
    /// إعادة الحاوية إلى القيم الفارغة عند إنشاء سجل جديد.
    /// </summary>
    public void ClearAuditInfo()
    {
        _createdByValue.Text = "—";
        _createdAtValue.Text = "—";
        _modifiedByValue.Text = "—";
        _modifiedAtValue.Text = "—";
        _editCountValue.Text = "0";
        _printCountValue.Text = "0";
    }

    /// <summary>
    /// بناء جدول ثابت يضم اسم كل معلومة وقيمتها.
    /// </summary>
    private void InitializeLayout()
    {
        BackColor = Color.White;
        Dock = DockStyle.Top;
        Height = 64;
        MinimumSize = new Size(0, 64);
        Padding = new Padding(10, 8, 10, 8);
        RightToLeft = RightToLeft.Yes;

        var table = new TableLayoutPanel
        {
            ColumnCount = 12,
            Dock = DockStyle.Fill,
            RightToLeft = RightToLeft.Yes,
            RowCount = 1
        };

        // كل معلومة تستخدم عمودًا للتسمية وعمودًا للقيمة، ولذلك لدينا 12 عمودًا لست معلومات.
        for (var i = 0; i < 12; i++)
        {
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, i % 2 == 0 ? 7F : 9.666F));
        }

        AddPair(table, 0, "أنشأ بواسطة:", _createdByValue);
        AddPair(table, 2, "تاريخ الإنشاء:", _createdAtValue);
        AddPair(table, 4, "عدّل بواسطة:", _modifiedByValue);
        AddPair(table, 6, "آخر تعديل:", _modifiedAtValue);
        AddPair(table, 8, "مرات التعديل:", _editCountValue);
        AddPair(table, 10, "مرات الطباعة:", _printCountValue);

        Controls.Add(table);
    }

    /// <summary>
    /// إضافة تسمية وقيمة متجاورتين إلى الجدول لتبقى البنية متطابقة دائمًا.
    /// </summary>
    private static void AddPair(TableLayoutPanel table, int column, string caption, Label valueLabel)
    {
        table.Controls.Add(CreateCaptionLabel(caption), column, 0);
        table.Controls.Add(valueLabel, column + 1, 0);
    }

    /// <summary>
    /// إنشاء عنوان ثابت لمعلومة التدقيق مثل "تاريخ الإنشاء".
    /// </summary>
    private static Label CreateCaptionLabel(string text) => new()
    {
        Dock = DockStyle.Fill,
        Font = UiTheme.CreateBoldFont(9F),
        ForeColor = UiTheme.SecondaryText,
        Text = text,
        TextAlign = ContentAlignment.MiddleRight
    };

    /// <summary>
    /// إنشاء Label خاص بالقيمة التي ستتغير مع السجل الحالي.
    /// </summary>
    private static Label CreateValueLabel() => new()
    {
        Dock = DockStyle.Fill,
        Font = UiTheme.CreateRegularFont(9F),
        ForeColor = UiTheme.HeadingText,
        TextAlign = ContentAlignment.MiddleRight
    };

    /// <summary>
    /// تحويل القيمة الفارغة إلى شرطة موحدة بدل ترك الحقل بلا نص.
    /// </summary>
    private static string Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? "—" : value.Trim();

    /// <summary>
    /// تنسيق التاريخ بنفس الشكل في جميع الشاشات.
    /// </summary>
    private static string FormatDate(DateTime? value) =>
        value.HasValue ? value.Value.ToString("yyyy/MM/dd HH:mm") : "—";
}
