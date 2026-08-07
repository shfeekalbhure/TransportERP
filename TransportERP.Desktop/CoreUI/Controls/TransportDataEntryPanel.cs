using System.ComponentModel;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// حاوية موحدة لترتيب حقول البيانات الرئيسية في عمودين من اليمين إلى اليسار.
/// هذه الحاوية تمنع تكرار المقاسات والمسافات داخل كل شاشة، وتضمن أن كل أداة تكون داخل حاوية منظمة.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportDataEntryPanel : TableLayoutPanel
{
    public TransportDataEntryPanel()
    {
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        ColumnCount = 4;
        Dock = DockStyle.Top;
        RightToLeft = RightToLeft.Yes;
        Padding = new Padding(TransportUiMetrics.CompactPadding);
        Margin = Padding.Empty;

        ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, TransportUiMetrics.MainDataLabelWidth));
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, TransportUiMetrics.MainDataLabelWidth));
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
    }

    /// <summary>
    /// يضيف حقلًا مع عنوانه في المكان الصحيح، ويضبط RTL والمحاذاة والهوامش المركزية تلقائيًا.
    /// الصفوف العادية تعتمد ارتفاع 6 مم مع فجوة 1.5 مم، بينما الملاحظات متعددة الأسطر
    /// تأخذ الحد الأدنى المركزي دون قص المحتوى.
    /// </summary>
    public void AddField(string labelText, Control editor, int index)
    {
        var row = index / 2;
        var pair = index % 2;
        var labelColumn = pair == 0 ? 0 : 2;
        var fieldColumn = labelColumn + 1;

        while (RowCount <= row)
        {
            RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.MainDataRowHeight));
            RowCount++;
        }

        var isMultiline = editor is TextBox textBox && textBox.Multiline;
        var requiredRowHeight = isMultiline
            ? TransportUiMetrics.MainDataMultilineMinHeight + TransportUiMetrics.MainDataRowGap
            : TransportUiMetrics.MainDataRowHeight;

        if (RowStyles[row].SizeType == SizeType.Absolute)
        {
            RowStyles[row].Height = Math.Max(RowStyles[row].Height, requiredRowHeight);
        }

        var label = new Label
        {
            Dock = DockStyle.Fill,
            Text = labelText,
            TextAlign = ContentAlignment.MiddleRight,
            RightToLeft = RightToLeft.Yes,
            Margin = new Padding(
                TransportUiMetrics.MainDataHorizontalMargin,
                TransportUiMetrics.MainDataVerticalMargin,
                TransportUiMetrics.MainDataLabelFieldGap,
                TransportUiMetrics.MainDataVerticalMargin)
        };

        editor.Dock = DockStyle.Fill;
        editor.RightToLeft = RightToLeft.Yes;
        editor.Margin = new Padding(
            TransportUiMetrics.MainDataHorizontalMargin,
            TransportUiMetrics.MainDataVerticalMargin,
            TransportUiMetrics.MainDataHorizontalMargin,
            TransportUiMetrics.MainDataVerticalMargin);

        if (editor is TextBox fieldTextBox)
        {
            fieldTextBox.TextAlign = HorizontalAlignment.Right;
        }

        Controls.Add(label, labelColumn, row);
        Controls.Add(editor, fieldColumn, row);
    }
}
