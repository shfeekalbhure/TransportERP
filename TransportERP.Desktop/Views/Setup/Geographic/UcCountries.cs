namespace TransportERP.Desktop.Views.Setup.Geographic;

/// <summary>
/// GEN-003 — شاشة الدول.
/// شاشة عمل مستضافة داخل FrmDashboard وليست نافذة مستقلة.
/// </summary>
public sealed class UcCountries : UserControl
{
    private readonly TextBox _txtCode = CreateTextBox();
    private readonly TextBox _txtArabicName = CreateTextBox(required: true);
    private readonly TextBox _txtEnglishName = CreateTextBox();
    private readonly TextBox _txtIso2 = CreateTextBox();
    private readonly TextBox _txtIso3 = CreateTextBox();
    private readonly TextBox _txtDialCode = CreateTextBox();
    private readonly TextBox _txtCurrencyCode = CreateTextBox();
    private readonly ComboBox _cmbStatus = CreateComboBox();
    private readonly TextBox _txtNotes = CreateTextBox(multiline: true);
    private readonly TextBox _txtSearch = CreateTextBox();
    private readonly ComboBox _cmbStatusFilter = CreateComboBox();
    private readonly DataGridView _grid = new();
    private readonly Label _lblCounter = new();
    private readonly Label _lblCreated = new();
    private readonly Label _lblModified = new();
    private readonly Label _lblEditCount = new();
    private readonly Label _lblPrintCount = new();

    public UcCountries()
    {
        Name = nameof(UcCountries);
        Dock = DockStyle.Fill;
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(247, 249, 252);
        Font = new Font("Segoe UI", 10F);
        RightToLeft = RightToLeft.Yes;

        _cmbStatus.Items.AddRange(["نشطة", "غير نشطة"]);
        _cmbStatus.SelectedIndex = 0;
        _cmbStatusFilter.Items.AddRange(["الكل", "نشطة", "غير نشطة"]);
        _cmbStatusFilter.SelectedIndex = 0;

        Controls.Add(BuildRoot());
        ConfigureGrid();
        LoadPreviewRows();
    }

    private Control BuildRoot()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5,
            Padding = new Padding(18),
            BackColor = BackColor,
            RightToLeft = RightToLeft.Yes
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 62));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 255));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));

        root.Controls.Add(BuildHeader(), 0, 0);
        root.Controls.Add(BuildToolbar(), 0, 1);
        root.Controls.Add(BuildEditorCard(), 0, 2);
        root.Controls.Add(BuildListCard(), 0, 3);
        root.Controls.Add(BuildAuditFooter(), 0, 4);
        return root;
    }

    private Control BuildHeader()
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(16, 8, 16, 8) };
        var title = new Label
        {
            Dock = DockStyle.Top,
            Height = 30,
            Text = "الدول",
            Font = new Font(Font.FontFamily, 16F, FontStyle.Bold),
            ForeColor = Color.FromArgb(24, 55, 95),
            TextAlign = ContentAlignment.MiddleRight
        };
        var subtitle = new Label
        {
            Dock = DockStyle.Fill,
            Text = "GEN-003  |  التهيئة العامة ← البيانات الجغرافية",
            ForeColor = Color.FromArgb(95, 105, 120),
            TextAlign = ContentAlignment.MiddleRight
        };
        panel.Controls.Add(subtitle);
        panel.Controls.Add(title);
        return panel;
    }

    private Control BuildToolbar()
    {
        var bar = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Padding = new Padding(8, 9, 8, 7),
            BackColor = Color.FromArgb(235, 241, 249)
        };

        var btnNew = CreateActionButton("جديد");
        var btnSave = CreateActionButton("حفظ");
        var btnEdit = CreateActionButton("تعديل");
        var btnStop = CreateActionButton("إيقاف");
        var btnDelete = CreateActionButton("حذف");
        var btnPrint = CreateActionButton("طباعة");
        var btnClose = CreateActionButton("إغلاق");

        btnNew.Click += (_, _) => ClearEditor();
        btnClose.Click += (_, _) => CloseHostTab();

        bar.Controls.AddRange([btnNew, btnSave, btnEdit, btnStop, btnDelete, btnPrint, btnClose]);
        return bar;
    }

    private Control BuildEditorCard()
    {
        var card = new GroupBox
        {
            Dock = DockStyle.Fill,
            Text = "البيانات الرئيسية",
            Padding = new Padding(14, 26, 14, 12),
            ForeColor = Color.FromArgb(37, 59, 86),
            BackColor = Color.White,
            RightToLeft = RightToLeft.Yes
        };

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 5,
            RightToLeft = RightToLeft.Yes
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        for (var i = 0; i < 5; i++) layout.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

        AddField(layout, 0, "كود الدولة", _txtCode, 0);
        AddField(layout, 2, "الاسم العربي *", _txtArabicName, 0);
        AddField(layout, 0, "الاسم الإنجليزي", _txtEnglishName, 1);
        AddField(layout, 2, "ISO2", _txtIso2, 1);
        AddField(layout, 0, "ISO3", _txtIso3, 2);
        AddField(layout, 2, "مفتاح الاتصال", _txtDialCode, 2);
        AddField(layout, 0, "رمز العملة", _txtCurrencyCode, 3);
        AddField(layout, 2, "الحالة", _cmbStatus, 3);

        layout.Controls.Add(CreateFieldLabel("الملاحظات"), 0, 4);
        layout.Controls.Add(_txtNotes, 1, 4);
        layout.SetColumnSpan(_txtNotes, 3);

        card.Controls.Add(layout);
        return card;
    }

    private Control BuildListCard()
    {
        var card = new GroupBox
        {
            Dock = DockStyle.Fill,
            Text = "قائمة الدول",
            Padding = new Padding(12, 26, 12, 10),
            ForeColor = Color.FromArgb(37, 59, 86),
            BackColor = Color.White,
            RightToLeft = RightToLeft.Yes
        };

        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));

        var filters = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Padding = new Padding(0, 5, 0, 4)
        };
        _txtSearch.Width = 300;
        _txtSearch.PlaceholderText = "بحث بالاسم أو الكود...";
        _cmbStatusFilter.Width = 145;
        filters.Controls.Add(CreateFieldLabel("بحث"));
        filters.Controls.Add(_txtSearch);
        filters.Controls.Add(CreateFieldLabel("الحالة"));
        filters.Controls.Add(_cmbStatusFilter);
        filters.Controls.Add(CreateActionButton("تصفية", 90));

        var pager = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            Padding = new Padding(6, 4, 6, 2)
        };
        pager.Controls.AddRange([
            CreatePagerButton("الأول"), CreatePagerButton("السابق"), CreatePagerButton("1"),
            CreatePagerButton("التالي"), CreatePagerButton("الأخير")
        ]);
        _lblCounter.AutoSize = true;
        _lblCounter.Margin = new Padding(18, 7, 0, 0);
        _lblCounter.Text = "عرض 1 - 5 من 5";
        pager.Controls.Add(_lblCounter);

        layout.Controls.Add(filters, 0, 0);
        layout.Controls.Add(_grid, 0, 1);
        layout.Controls.Add(pager, 0, 2);
        card.Controls.Add(layout);
        return card;
    }

    private Control BuildAuditFooter()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            Padding = new Padding(12, 7, 12, 5),
            BackColor = Color.FromArgb(238, 242, 249),
            RightToLeft = RightToLeft.Yes
        };
        for (var i = 0; i < 4; i++) panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

        ConfigureFooterLabel(_lblCreated, "الإنشاء: —");
        ConfigureFooterLabel(_lblModified, "آخر تعديل: —");
        ConfigureFooterLabel(_lblEditCount, "مرات التعديل: 0");
        ConfigureFooterLabel(_lblPrintCount, "مرات الطباعة: 0");

        panel.Controls.Add(_lblCreated, 0, 0);
        panel.Controls.Add(_lblModified, 1, 0);
        panel.Controls.Add(_lblEditCount, 2, 0);
        panel.Controls.Add(_lblPrintCount, 3, 0);
        return panel;
    }

    private void ConfigureGrid()
    {
        _grid.Dock = DockStyle.Fill;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.ReadOnly = true;
        _grid.MultiSelect = false;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.RowHeadersVisible = false;
        _grid.BackgroundColor = Color.White;
        _grid.BorderStyle = BorderStyle.FixedSingle;
        _grid.RightToLeft = RightToLeft.Yes;
        _grid.EnableHeadersVisualStyles = false;
        _grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(228, 236, 247);
        _grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 55, 85);
        _grid.ColumnHeadersDefaultCellStyle.Font = new Font(Font, FontStyle.Bold);
        _grid.Columns.Add("Code", "الكود");
        _grid.Columns.Add("ArabicName", "الاسم العربي");
        _grid.Columns.Add("EnglishName", "الاسم الإنجليزي");
        _grid.Columns.Add("Iso2", "ISO2");
        _grid.Columns.Add("Iso3", "ISO3");
        _grid.Columns.Add("DialCode", "مفتاح الاتصال");
        _grid.Columns.Add("Currency", "رمز العملة");
        _grid.Columns.Add("Status", "الحالة");
        _grid.CellDoubleClick += (_, e) => LoadSelectedRow(e.RowIndex);
    }

    private void LoadPreviewRows()
    {
        _grid.Rows.Clear();
        _grid.Rows.Add("001", "اليمن", "Yemen", "YE", "YEM", "+967", "YER", "نشطة");
        _grid.Rows.Add("002", "المملكة العربية السعودية", "Saudi Arabia", "SA", "SAU", "+966", "SAR", "نشطة");
        _grid.Rows.Add("003", "الإمارات العربية المتحدة", "United Arab Emirates", "AE", "ARE", "+971", "AED", "نشطة");
        _grid.Rows.Add("004", "عُمان", "Oman", "OM", "OMN", "+968", "OMR", "نشطة");
        _grid.Rows.Add("005", "مصر", "Egypt", "EG", "EGY", "+20", "EGP", "نشطة");
    }

    private void LoadSelectedRow(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= _grid.Rows.Count) return;
        var row = _grid.Rows[rowIndex];
        _txtCode.Text = Convert.ToString(row.Cells[0].Value) ?? string.Empty;
        _txtArabicName.Text = Convert.ToString(row.Cells[1].Value) ?? string.Empty;
        _txtEnglishName.Text = Convert.ToString(row.Cells[2].Value) ?? string.Empty;
        _txtIso2.Text = Convert.ToString(row.Cells[3].Value) ?? string.Empty;
        _txtIso3.Text = Convert.ToString(row.Cells[4].Value) ?? string.Empty;
        _txtDialCode.Text = Convert.ToString(row.Cells[5].Value) ?? string.Empty;
        _txtCurrencyCode.Text = Convert.ToString(row.Cells[6].Value) ?? string.Empty;
        _cmbStatus.SelectedItem = Convert.ToString(row.Cells[7].Value) ?? "نشطة";
        _lblCreated.Text = "الإنشاء: بيانات تجريبية";
        _lblModified.Text = "آخر تعديل: —";
    }

    private void ClearEditor()
    {
        _txtCode.Clear();
        _txtArabicName.Clear();
        _txtEnglishName.Clear();
        _txtIso2.Clear();
        _txtIso3.Clear();
        _txtDialCode.Clear();
        _txtCurrencyCode.Clear();
        _txtNotes.Clear();
        _cmbStatus.SelectedIndex = 0;
        _txtArabicName.Focus();
    }

    private void CloseHostTab()
    {
        if (Parent is not TabPage page || page.Parent is not TabControl tabs) return;
        tabs.TabPages.Remove(page);
        Dispose();
        page.Dispose();
    }

    private static void AddField(TableLayoutPanel layout, int labelColumn, string label, Control control, int row)
    {
        layout.Controls.Add(CreateFieldLabel(label), labelColumn, row);
        layout.Controls.Add(control, labelColumn + 1, row);
    }

    private static Label CreateFieldLabel(string text) => new()
    {
        Text = text,
        Dock = DockStyle.Fill,
        TextAlign = ContentAlignment.MiddleRight,
        AutoSize = false,
        ForeColor = Color.FromArgb(55, 65, 78),
        Margin = new Padding(6, 3, 6, 3)
    };

    private static TextBox CreateTextBox(bool required = false, bool multiline = false) => new()
    {
        Dock = DockStyle.Fill,
        Multiline = multiline,
        TextAlign = HorizontalAlignment.Right,
        RightToLeft = RightToLeft.Yes,
        BackColor = required ? Color.FromArgb(255, 250, 220) : Color.White,
        BorderStyle = BorderStyle.FixedSingle,
        Margin = new Padding(6, 5, 6, 5)
    };

    private static ComboBox CreateComboBox() => new()
    {
        Dock = DockStyle.Fill,
        DropDownStyle = ComboBoxStyle.DropDownList,
        RightToLeft = RightToLeft.Yes,
        Margin = new Padding(6, 5, 6, 5)
    };

    private static Button CreateActionButton(string text, int width = 92) => new()
    {
        Text = text,
        Width = width,
        Height = 34,
        FlatStyle = FlatStyle.Flat,
        BackColor = Color.White,
        ForeColor = Color.FromArgb(28, 74, 123),
        Margin = new Padding(4, 0, 4, 0),
        Cursor = Cursors.Hand
    };

    private static Button CreatePagerButton(string text) => new()
    {
        Text = text,
        AutoSize = true,
        Height = 28,
        FlatStyle = FlatStyle.Flat,
        Margin = new Padding(3, 0, 3, 0)
    };

    private static void ConfigureFooterLabel(Label label, string text)
    {
        label.Text = text;
        label.Dock = DockStyle.Fill;
        label.TextAlign = ContentAlignment.MiddleRight;
        label.ForeColor = Color.FromArgb(72, 82, 98);
    }
}
