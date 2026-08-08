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
    private bool _profileContentSizingEnabled;

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
    /// في مسار Profile V1 يصبح هذا هو مصدر الحقيقة الذي تقرأه الـPolicy.
    /// </summary>
    [Browsable(false)]
    public int PreferredContentHeight
    {
        get
        {
            if (_profileContentSizingEnabled)
            {
                return GetProfilePreferredSize(new Size(Math.Max(1, ClientSize.Width), 0)).Height;
            }

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
    /// تفعيل سلوك Content للـPilot المعلن فقط. لا يغير السلوك الافتراضي للشاشات غير المهاجرة.
    /// الحقول تصبح ثابتة رأسيًا ومتمددة أفقيًا، والصفوف تقاس من Preferred Content بدل Dock=Fill رأسيًا.
    /// </summary>
    internal void EnableProfileContentSizing()
    {
        if (_profileContentSizingEnabled)
        {
            return;
        }

        _profileContentSizingEnabled = true;
        AutoSize = false;
        AutoScroll = false;
        Dock = DockStyle.Top;

        SuspendLayout();
        try
        {
            for (var row = 0; row < RowStyles.Count; row++)
            {
                RowStyles[row].SizeType = SizeType.AutoSize;
                RowStyles[row].Height = 0F;
            }

            foreach (Control control in Controls)
            {
                ConfigureProfileControl(control);
            }
        }
        finally
        {
            ResumeLayout(true);
        }
    }

    /// <summary>
    /// يحسب Preferred Size للـPilot من الصفوف الفعلية وأكبر عنصر في كل صف.
    /// لا يعتمد على Height محلي سحري، ويحتسب Margins/Padding المركزية.
    /// </summary>
    internal Size GetProfilePreferredSize(Size proposedSize)
    {
        var rowCount = Math.Max(1, RowCount);
        var rowHeights = new int[rowCount];

        foreach (Control control in Controls)
        {
            var row = GetRow(control);
            if (row < 0 || row >= rowHeights.Length)
            {
                continue;
            }

            var preferredHeight = GetProfileControlHeight(control, proposedSize.Width);
            var totalHeight = preferredHeight + control.Margin.Vertical;
            rowHeights[row] = Math.Max(rowHeights[row], totalHeight);
        }

        var height = Padding.Vertical;
        for (var row = 0; row < rowHeights.Length; row++)
        {
            height += Math.Max(TransportUiMetrics.MainDataRowHeight, rowHeights[row]);
        }

        var width = proposedSize.Width > 0 ? proposedSize.Width : Math.Max(1, ClientSize.Width);
        return new Size(width, Math.Max(TransportUiMetrics.MainDataRowHeight, height));
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

        if (_profileContentSizingEnabled)
        {
            RowStyles[row].SizeType = SizeType.AutoSize;
            ConfigureProfileControl(label);
            ConfigureProfileControl(editor);
        }

        PerformLayout();
    }

    private static void ConfigureProfileControl(Control control)
    {
        control.RightToLeft = RightToLeft.Yes;

        if (control is Label label)
        {
            label.Dock = DockStyle.None;
            label.Anchor = AnchorStyles.Right;
            label.AutoSize = true;
            label.TextAlign = ContentAlignment.MiddleRight;
            return;
        }

        if (control is CheckBox checkBox)
        {
            checkBox.Dock = DockStyle.None;
            checkBox.Anchor = AnchorStyles.Right;
            checkBox.AutoSize = true;
            return;
        }

        control.Dock = DockStyle.None;
        control.Anchor = AnchorStyles.Left | AnchorStyles.Right;

        switch (control)
        {
            case TextBox textBox when textBox.Multiline:
                textBox.Height = Math.Max(textBox.Height, TransportUiMetrics.MainDataMultilineMinHeight);
                textBox.MinimumSize = new Size(textBox.MinimumSize.Width, TransportUiMetrics.MainDataMultilineMinHeight);
                textBox.TextAlign = HorizontalAlignment.Right;
                break;

            case TextBox textBox:
                textBox.AutoSize = false;
                textBox.Height = TransportUiMetrics.MainDataControlHeight;
                textBox.MinimumSize = new Size(textBox.MinimumSize.Width, TransportUiMetrics.MainDataControlHeight);
                textBox.MaximumSize = new Size(0, TransportUiMetrics.MainDataControlHeight);
                textBox.TextAlign = HorizontalAlignment.Right;
                break;

            case ComboBox comboBox:
                comboBox.Height = TransportUiMetrics.MainDataControlHeight;
                comboBox.MinimumSize = new Size(comboBox.MinimumSize.Width, TransportUiMetrics.MainDataControlHeight);
                comboBox.MaximumSize = new Size(0, TransportUiMetrics.MainDataControlHeight);
                break;

            case NumericUpDown numericUpDown:
                numericUpDown.Height = TransportUiMetrics.MainDataControlHeight;
                numericUpDown.MinimumSize = new Size(numericUpDown.MinimumSize.Width, TransportUiMetrics.MainDataControlHeight);
                numericUpDown.MaximumSize = new Size(0, TransportUiMetrics.MainDataControlHeight);
                numericUpDown.TextAlign = HorizontalAlignment.Right;
                break;

            case DateTimePicker dateTimePicker:
                dateTimePicker.Height = TransportUiMetrics.MainDataControlHeight;
                dateTimePicker.MinimumSize = new Size(dateTimePicker.MinimumSize.Width, TransportUiMetrics.MainDataControlHeight);
                dateTimePicker.MaximumSize = new Size(0, TransportUiMetrics.MainDataControlHeight);
                dateTimePicker.RightToLeftLayout = true;
                break;

            case Button button:
                button.Height = Math.Max(button.MinimumSize.Height, TransportUiMetrics.MainDataControlHeight);
                break;
        }
    }

    private static int GetProfileControlHeight(Control control, int proposedWidth)
    {
        return control switch
        {
            Label label => Math.Max(label.GetPreferredSize(new Size(Math.Max(1, proposedWidth), 0)).Height, TransportUiMetrics.MainDataControlHeight),
            TextBox textBox when textBox.Multiline => Math.Max(textBox.Height, TransportUiMetrics.MainDataMultilineMinHeight),
            TextBox => TransportUiMetrics.MainDataControlHeight,
            ComboBox => TransportUiMetrics.MainDataControlHeight,
            NumericUpDown => TransportUiMetrics.MainDataControlHeight,
            DateTimePicker => TransportUiMetrics.MainDataControlHeight,
            CheckBox checkBox => Math.Max(checkBox.GetPreferredSize(Size.Empty).Height, TransportUiMetrics.MainDataControlHeight),
            Button button => Math.Max(button.Height, TransportUiMetrics.MainDataControlHeight),
            _ => Math.Max(control.GetPreferredSize(new Size(Math.Max(1, proposedWidth), 0)).Height, TransportUiMetrics.MainDataControlHeight)
        };
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
