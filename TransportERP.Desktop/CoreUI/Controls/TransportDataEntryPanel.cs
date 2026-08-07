using System.ComponentModel;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// حاوية موحدة لترتيب حقول البيانات الرئيسية من اليمين إلى اليسار.
/// تتمدد رأسيًا حسب عدد الصفوف ولا تستخدم شريط تمرير.
/// الحد الأعلى ثلاثة أعمدة حقول في الصف الواحد، وكل حقل يتكون من Label + أداة إدخال.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportDataEntryPanel : TableLayoutPanel
{
    private int _fieldColumnCount = TransportUiMetrics.MainDataMaxFieldColumns;

    public TransportDataEntryPanel()
    {
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        AutoScroll = false;
        Dock = DockStyle.Top;
        RightToLeft = RightToLeft.Yes;
        Padding = Padding.Empty;
        Margin = Padding.Empty;

        ConfigureColumns(_fieldColumnCount);
    }

    /// <summary>
    /// عدد أعمدة الحقول في الصف الواحد. لا يسمح بأكثر من ثلاثة أعمدة حفاظًا على وضوح شاشة ERP.
    /// </summary>
    [Category("TransportERP")]
    [DefaultValue(TransportUiMetrics.MainDataMaxFieldColumns)]
    public int FieldColumnCount
    {
        get => _fieldColumnCount;
        set
        {
            var normalized = Math.Clamp(value, 1, TransportUiMetrics.MainDataMaxFieldColumns);
            if (_fieldColumnCount == normalized)
            {
                return;
            }

            if (Controls.Count > 0)
            {
                throw new InvalidOperationException("يجب تحديد عدد الأعمدة قبل إضافة الحقول إلى الحاوية.");
            }

            _fieldColumnCount = normalized;
            ConfigureColumns(_fieldColumnCount);
        }
    }

    /// <summary>
    /// الارتفاع الفعلي المطلوب لعرض جميع الصفوف بدون أي Scroll.
    /// يستخدمه القالب العام لتمديد حاوية البيانات إلى الأسفل تلقائيًا.
    /// </summary>
    [Browsable(false)]
    public int PreferredContentHeight
    {
        get
        {
            if (RowCount == 0)
            {
                return TransportUiMetrics.MainDataRowHeight;
            }

            var total = 0;
            for (var row = 0; row < RowStyles.Count; row++)
            {
                total += RowStyles[row].SizeType == SizeType.Absolute
                    ? (int)Math.Ceiling(RowStyles[row].Height)
                    : TransportUiMetrics.MainDataRowHeight;
            }

            return Math.Max(TransportUiMetrics.MainDataRowHeight, total);
        }
    }

    /// <summary>
    /// يضيف حقلًا مع عنوانه في المكان الصحيح، ويضبط RTL والمحاذاة والهوامش المركزية تلقائيًا.
    /// الصفوف العادية تعتمد ارتفاع 6 مم مع فجوة 1.5 مم، بينما الملاحظات متعددة الأسطر
    /// تأخذ الحد الأدنى المركزي دون قص المحتوى.
    /// </summary>
    public void AddField(string labelText, Control editor, int index)
    {
        ArgumentNullException.ThrowIfNull(editor);

        var row = index / _fieldColumnCount;
        var fieldPosition = index % _fieldColumnCount;
        var labelColumn = fieldPosition * 2;
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
        PerformLayout();
    }

    private void ConfigureColumns(int fieldColumns)
    {
        SuspendLayout();
        ColumnStyles.Clear();
        ColumnCount = fieldColumns * 2;

        for (var column = 0; column < fieldColumns; column++)
        {
            ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, TransportUiMetrics.MainDataLabelWidth));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / fieldColumns));
        }

        ResumeLayout(true);
    }
}
