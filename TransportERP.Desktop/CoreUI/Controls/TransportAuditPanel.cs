using System.ComponentModel;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// حاوية معلومات الإنشاء والتعديل والطباعة الموحدة لكل شاشة.
/// تم تصغيرها حتى لا تزاحم الجدول مع إبقاء المعلومات الست ثابتة في جميع الشاشات.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportAuditPanel : UserControl
{
    private readonly Label _createdByValue = CreateValueLabel();
    private readonly Label _createdAtValue = CreateValueLabel();
    private readonly Label _modifiedByValue = CreateValueLabel();
    private readonly Label _modifiedAtValue = CreateValueLabel();
    private readonly Label _editCountValue = CreateValueLabel();
    private readonly Label _printCountValue = CreateValueLabel();

    public TransportAuditPanel()
    {
        InitializeLayout();
        ClearAuditInfo();
    }

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
    /// الحاوية الداخلية 10 مم والمحتوى 8 مم لتوفير مساحة أكبر للجدول.
    /// الهوامش تأتي من TransportUiMetrics حتى تبقى مطابقة لبقية CoreUI.
    /// </summary>
    private void InitializeLayout()
    {
        BackColor = UiTheme.SurfaceBackground;
        Dock = DockStyle.Fill;
        Height = TransportUiMetrics.AuditPanelHeight;
        MinimumSize = new Size(0, TransportUiMetrics.AuditPanelHeight);
        Padding = new Padding(
            TransportUiMetrics.GroupHorizontalPadding,
            TransportUiMetrics.MainDataVerticalMargin,
            TransportUiMetrics.GroupHorizontalPadding,
            TransportUiMetrics.MainDataVerticalMargin);
        RightToLeft = RightToLeft.Yes;

        var table = new TableLayoutPanel
        {
            ColumnCount = 12,
            Dock = DockStyle.Fill,
            RightToLeft = RightToLeft.Yes,
            RowCount = 1,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };

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

    private static void AddPair(TableLayoutPanel table, int column, string caption, Label valueLabel)
    {
        table.Controls.Add(CreateCaptionLabel(caption), column, 0);
        table.Controls.Add(valueLabel, column + 1, 0);
    }

    private static Label CreateCaptionLabel(string text) => new()
    {
        Dock = DockStyle.Fill,
        Font = UiTheme.CreateBoldFont(8F),
        ForeColor = UiTheme.SecondaryText,
        MinimumSize = new Size(0, TransportUiMetrics.AuditContentHeight),
        Margin = new Padding(1),
        Text = text,
        TextAlign = ContentAlignment.MiddleRight
    };

    private static Label CreateValueLabel() => new()
    {
        Dock = DockStyle.Fill,
        Font = UiTheme.CreateRegularFont(8F),
        ForeColor = UiTheme.HeadingText,
        MinimumSize = new Size(0, TransportUiMetrics.AuditContentHeight),
        Margin = new Padding(1),
        TextAlign = ContentAlignment.MiddleRight
    };

    private static string Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? "—" : value.Trim();

    private static string FormatDate(DateTime? value) =>
        value.HasValue ? value.Value.ToString("yyyy/MM/dd HH:mm") : "—";
}
