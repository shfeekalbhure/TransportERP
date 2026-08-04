using TransportERP.Desktop.Controls;
using TransportERP.Desktop.Services;

namespace TransportERP.Desktop;

/// <summary>
/// شاشة GEN-004 — المحافظات بالقالب الموحد المعتمد.
/// </summary>
public sealed class FrmGovernorates : Form
{
    private readonly ScreenHeaderControl _header;
    private readonly ComboBox _cboCountry;
    private readonly TextBox _txtCode;
    private readonly TextBox _txtNameAr;
    private readonly TextBox _txtNameEn;
    private readonly ComboBox _cboStatus;
    private readonly TextBox _txtNotes;
    private readonly TextBox _txtSearchAll;
    private readonly TextBox _txtSearchCode;
    private readonly TextBox _txtSearchName;
    private readonly ComboBox _cboSearchStatus;
    private readonly DataGridView _grid;
    private readonly Label _lblResultCount;
    private int _currentIndex;
    private bool _hostedInsideDashboard;

    public FrmGovernorates()
    {
        Text = "TransportERP - المحافظات";
        BackColor = Color.FromArgb(244, 247, 251);
        Font = new Font("Segoe UI", 10F);
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        WindowState = FormWindowState.Maximized;
        MinimumSize = new Size(1280, 800);

        _header = new ScreenHeaderControl
        {
            ScreenTitle = "المحافظات",
            Breadcrumb = "التهيئة العامة  ‹  البيانات الجغرافية  ‹  المحافظات",
            RecordPosition = "0 / 0"
        };

        _cboCountry = CreateRequiredCombo("اليمن", "المملكة العربية السعودية", "الإمارات العربية المتحدة");
        _txtCode = CreateRequiredTextBox();
        _txtNameAr = CreateRequiredTextBox();
        _txtNameEn = CreateRequiredTextBox();
        _cboStatus = CreateRequiredCombo("نشط", "غير نشط");
        _txtNotes = CreateOptionalTextBox(multiline: true);

        _txtSearchAll = CreateSearchTextBox("ابحث في جميع الحقول...");
        _txtSearchCode = CreateSearchTextBox("كود المحافظة");
        _txtSearchName = CreateSearchTextBox("اسم المحافظة");
        _cboSearchStatus = new ComboBox
        {
            Dock = DockStyle.Fill,
            DropDownStyle = ComboBoxStyle.DropDownList,
            RightToLeft = RightToLeft.Yes,
            FlatStyle = FlatStyle.Standard
        };
        _cboSearchStatus.Items.AddRange(new object[] { "الكل", "نشط", "غير نشط" });
        _cboSearchStatus.SelectedIndex = 0;

        _lblResultCount = new Label
        {
            Dock = DockStyle.Fill,
            Text = "0",
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.FromArgb(245, 248, 252),
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            BorderStyle = BorderStyle.FixedSingle
        };

        _grid = CreateGrid();

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 6,
            BackColor = BackColor,
            Padding = new Padding(14, 10, 14, 8),
            Margin = Padding.Empty
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 118F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 210F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 102F));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 132F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 4F));

        _header.Dock = DockStyle.Fill;
        root.Controls.Add(_header, 0, 0);
        root.Controls.Add(BuildMainDataGroup(), 0, 1);
        root.Controls.Add(BuildSearchGroup(), 0, 2);
        root.Controls.Add(BuildGridGroup(), 0, 3);
        root.Controls.Add(BuildFooter(), 0, 4);
        Controls.Add(root);

        WireHeaderActions();
        LoadPreviewData();
    }

    public void ConfigureForTabHosting()
    {
        if (_hostedInsideDashboard)
        {
            return;
        }

        _hostedInsideDashboard = true;
        TopLevel = false;
        FormBorderStyle = FormBorderStyle.None;
        Dock = DockStyle.Fill;
        WindowState = FormWindowState.Normal;
        ShowInTaskbar = false;
        ControlBox = false;
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        UiTypographyService.Apply(this, "GEN-004");
    }

    private Control BuildMainDataGroup()
    {
        var group = CreateSectionPanel("البيانات الرئيسية", out var content);
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 3,
            RightToLeft = RightToLeft.Yes,
            Padding = new Padding(12, 6, 12, 8)
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        for (var i = 0; i < 3; i++) layout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.333F));

        AddField(layout, 0, 0, "الدولة *", _cboCountry);
        AddField(layout, 0, 2, "كود المحافظة *", _txtCode);
        AddField(layout, 1, 0, "اسم المحافظة (عربي) *", _txtNameAr);
        AddField(layout, 1, 2, "اسم المحافظة (إنجليزي) *", _txtNameEn);
        AddField(layout, 2, 0, "الحالة *", _cboStatus);
        AddField(layout, 2, 2, "ملاحظات", _txtNotes);

        content.Controls.Add(layout);
        return group;
    }

    private Control BuildSearchGroup()
    {
        var group = CreateSectionPanel("بحث وتصفية", out var content);
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 6,
            RowCount = 1,
            RightToLeft = RightToLeft.Yes,
            Padding = new Padding(12, 7, 12, 9)
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 85F));

        var btnSearch = CreateButton("بحث", Color.FromArgb(28, 105, 225), Color.White);
        btnSearch.Click += (_, _) => ApplySearch();
        layout.Controls.Add(_txtSearchAll, 0, 0);
        layout.Controls.Add(_txtSearchCode, 1, 0);
        layout.Controls.Add(_txtSearchName, 2, 0);
        layout.Controls.Add(_cboSearchStatus, 3, 0);
        layout.Controls.Add(btnSearch, 4, 0);
        layout.Controls.Add(_lblResultCount, 5, 0);
        content.Controls.Add(layout);
        return group;
    }

    private Control BuildGridGroup()
    {
        var group = CreateSectionPanel("قائمة المحافظات", out var content);
        content.Padding = new Padding(10, 4, 10, 10);
        content.Controls.Add(_grid);
        return group;
    }

    private Control BuildFooter()
    {
        var footer = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = BackColor,
            Margin = Padding.Empty
        };
        footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
        footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));

        var audit = CreateSectionPanel("بيانات الإنشاء والتعديل", out var auditContent);
        var auditLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 2, RightToLeft = RightToLeft.Yes, Padding = new Padding(8) };
        auditLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        auditLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        auditLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        auditLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        AddReadOnly(auditLayout, 0, "تاريخ الإنشاء", "2026-08-04 17:30");
        AddReadOnly(auditLayout, 2, "أنشئ بواسطة", "مدير النظام");
        AddReadOnly(auditLayout, 4, "آخر تعديل", "2026-08-04 17:35");
        AddReadOnly(auditLayout, 6, "آخر تعديل بواسطة", "مدير النظام");
        auditContent.Controls.Add(auditLayout);

        var counters = CreateSectionPanel("العدادات والإحصاءات", out var counterContent);
        var counterLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 2, RightToLeft = RightToLeft.Yes, Padding = new Padding(8) };
        counterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        counterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        counterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        counterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        AddReadOnly(counterLayout, 0, "عدد التعديلات", "3");
        AddReadOnly(counterLayout, 2, "عدد الطباعات", "5");
        AddReadOnly(counterLayout, 4, "آخر طباعة", "2026-08-04");
        AddReadOnly(counterLayout, 6, "طبع بواسطة", "مدير النظام");
        counterContent.Controls.Add(counterLayout);

        audit.Margin = new Padding(0, 0, 6, 0);
        counters.Margin = new Padding(6, 0, 0, 0);
        footer.Controls.Add(audit, 0, 0);
        footer.Controls.Add(counters, 1, 0);
        return footer;
    }

    private void WireHeaderActions()
    {
        _header.NewClicked += (_, _) => ClearFields();
        _header.SaveClicked += (_, _) => MessageBox.Show("تم حفظ بيانات المعاينة.", "المحافظات", MessageBoxButtons.OK, MessageBoxIcon.Information);
        _header.EditClicked += (_, _) => _txtNameAr.Focus();
        _header.SearchClicked += (_, _) => _txtSearchAll.Focus();
        _header.FirstClicked += (_, _) => SelectRow(0);
        _header.PreviousClicked += (_, _) => SelectRow(_currentIndex - 1);
        _header.NextClicked += (_, _) => SelectRow(_currentIndex + 1);
        _header.LastClicked += (_, _) => SelectRow(_grid.Rows.Count - 1);
        _header.DeleteClicked += (_, _) => MessageBox.Show("الحذف غير مفعل في بيانات المعاينة.", "المحافظات", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        _header.PrintClicked += (_, _) => MessageBox.Show("تم تجهيز معاينة الطباعة.", "المحافظات", MessageBoxButtons.OK, MessageBoxIcon.Information);
        _header.RefreshClicked += (_, _) => LoadPreviewData();
        _header.CloseClicked += (_, _) => Close();
        _grid.SelectionChanged += (_, _) =>
        {
            if (_grid.CurrentRow is not null)
            {
                _currentIndex = _grid.CurrentRow.Index;
                UpdateNavigation();
            }
        };
    }

    private void LoadPreviewData()
    {
        _grid.Rows.Clear();
        _grid.Rows.Add("1", "YE", "ADN", "عدن", "Aden", "نشط");
        _grid.Rows.Add("2", "YE", "SAN", "صنعاء", "Sana'a", "نشط");
        _grid.Rows.Add("3", "YE", "TAI", "تعز", "Taiz", "نشط");
        _grid.Rows.Add("4", "YE", "HDR", "حضرموت", "Hadramout", "نشط");
        _grid.Rows.Add("5", "YE", "IBB", "إب", "Ibb", "نشط");
        _lblResultCount.Text = _grid.Rows.Count.ToString();
        SelectRow(0);
    }

    private void ApplySearch()
    {
        var term = _txtSearchAll.Text.Trim();
        var code = _txtSearchCode.Text.Trim();
        var name = _txtSearchName.Text.Trim();
        var status = _cboSearchStatus.SelectedItem?.ToString() ?? "الكل";
        var visible = 0;

        foreach (DataGridViewRow row in _grid.Rows)
        {
            var allText = string.Join(" ", row.Cells.Cast<DataGridViewCell>().Select(cell => cell.Value?.ToString() ?? string.Empty));
            var rowCode = row.Cells[2].Value?.ToString() ?? string.Empty;
            var rowName = row.Cells[3].Value?.ToString() ?? string.Empty;
            var rowStatus = row.Cells[5].Value?.ToString() ?? string.Empty;
            var match = (string.IsNullOrEmpty(term) || allText.Contains(term, StringComparison.OrdinalIgnoreCase))
                && (string.IsNullOrEmpty(code) || rowCode.Contains(code, StringComparison.OrdinalIgnoreCase))
                && (string.IsNullOrEmpty(name) || rowName.Contains(name, StringComparison.OrdinalIgnoreCase))
                && (status == "الكل" || rowStatus == status);
            row.Visible = match;
            if (match) visible++;
        }

        _lblResultCount.Text = visible.ToString();
    }

    private void ClearFields()
    {
        _txtCode.Clear();
        _txtNameAr.Clear();
        _txtNameEn.Clear();
        _txtNotes.Clear();
        _cboCountry.SelectedIndex = 0;
        _cboStatus.SelectedIndex = 0;
        _txtCode.Focus();
    }

    private void SelectRow(int index)
    {
        if (_grid.Rows.Count == 0)
        {
            _currentIndex = 0;
            UpdateNavigation();
            return;
        }

        index = Math.Clamp(index, 0, _grid.Rows.Count - 1);
        _currentIndex = index;
        _grid.ClearSelection();
        _grid.Rows[index].Selected = true;
        _grid.CurrentCell = _grid.Rows[index].Cells[0];
        UpdateNavigation();
    }

    private void UpdateNavigation()
    {
        var count = _grid.Rows.Count;
        _header.RecordPosition = count == 0 ? "0 / 0" : $"{_currentIndex + 1} / {count}";
        _header.SetNavigationState(count > 0, _currentIndex <= 0, _currentIndex >= count - 1);
    }

    private static Panel CreateSectionPanel(string title, out Panel content)
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(0, 0, 0, 8), Padding = new Padding(1) };
        var titleLabel = new Label
        {
            Dock = DockStyle.Top,
            Height = 34,
            Text = title,
            TextAlign = ContentAlignment.MiddleRight,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.FromArgb(28, 105, 225),
            Padding = new Padding(0, 0, 12, 0)
        };
        content = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
        panel.Controls.Add(content);
        panel.Controls.Add(titleLabel);
        return panel;
    }

    private static void AddField(TableLayoutPanel layout, int row, int labelColumn, string labelText, Control field)
    {
        var label = new Label
        {
            Dock = DockStyle.Fill,
            Text = labelText,
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.FromArgb(55, 65, 81),
            Font = new Font("Segoe UI", 10F, FontStyle.Regular),
            Padding = new Padding(0, 0, 8, 0)
        };
        field.Margin = new Padding(6);
        layout.Controls.Add(label, labelColumn, row);
        layout.Controls.Add(field, labelColumn + 1, row);
    }

    private static void AddReadOnly(TableLayoutPanel layout, int position, string labelText, string value)
    {
        var row = position / 4;
        var column = position % 4;
        layout.Controls.Add(new Label { Dock = DockStyle.Fill, Text = labelText, TextAlign = ContentAlignment.MiddleRight, ForeColor = Color.FromArgb(90, 100, 115) }, column, row);
        layout.Controls.Add(new Label { Dock = DockStyle.Fill, Text = value, TextAlign = ContentAlignment.MiddleRight, BackColor = Color.FromArgb(245, 247, 250), BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(6, 0, 6, 0) }, column + 1, row);
    }

    private static TextBox CreateRequiredTextBox() => new()
    {
        Dock = DockStyle.Fill,
        BackColor = Color.FromArgb(255, 250, 205),
        BorderStyle = BorderStyle.FixedSingle,
        TextAlign = HorizontalAlignment.Right,
        RightToLeft = RightToLeft.Yes
    };

    private static TextBox CreateOptionalTextBox(bool multiline = false) => new()
    {
        Dock = DockStyle.Fill,
        BackColor = Color.White,
        BorderStyle = BorderStyle.FixedSingle,
        TextAlign = HorizontalAlignment.Right,
        RightToLeft = RightToLeft.Yes,
        Multiline = multiline
    };

    private static TextBox CreateSearchTextBox(string placeholder) => new()
    {
        Dock = DockStyle.Fill,
        PlaceholderText = placeholder,
        BorderStyle = BorderStyle.FixedSingle,
        TextAlign = HorizontalAlignment.Right,
        RightToLeft = RightToLeft.Yes,
        Margin = new Padding(5)
    };

    private static ComboBox CreateRequiredCombo(params string[] items)
    {
        var combo = new ComboBox
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(255, 250, 205),
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle = FlatStyle.Standard,
            RightToLeft = RightToLeft.Yes
        };
        combo.Items.AddRange(items.Cast<object>().ToArray());
        if (combo.Items.Count > 0) combo.SelectedIndex = 0;
        return combo;
    }

    private static Button CreateButton(string text, Color backColor, Color foreColor) => new()
    {
        Dock = DockStyle.Fill,
        Text = text,
        BackColor = backColor,
        ForeColor = foreColor,
        FlatStyle = FlatStyle.Flat,
        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
        Margin = new Padding(5),
        UseVisualStyleBackColor = false
    };

    private static DataGridView CreateGrid()
    {
        var grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            RowHeadersVisible = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            RightToLeft = RightToLeft.Yes,
            EnableHeadersVisualStyles = false
        };
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 241, 248);
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        grid.RowTemplate.Height = 32;
        grid.Columns.Add("No", "#");
        grid.Columns.Add("Country", "الدولة");
        grid.Columns.Add("Code", "كود المحافظة");
        grid.Columns.Add("NameAr", "اسم المحافظة (عربي)");
        grid.Columns.Add("NameEn", "اسم المحافظة (إنجليزي)");
        grid.Columns.Add("Status", "الحالة");
        return grid;
    }
}
