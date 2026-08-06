using System.ComponentModel;
using System.Drawing;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>
/// GEN-008 — أنواع المركبات.
/// شاشة بيانات مرجعية وفق النمط P001، تستضيفها مساحة العمل في تبويب C015.
/// </summary>
public sealed class FrmVehicleTypes : Form
{
    private const int PageSize = 10;

    private readonly BindingList<VehicleTypeRecord> _records = new();
    private readonly DataGridView _grid = new();
    private readonly TextBox _codeInput = CreateInput();
    private readonly TextBox _arabicNameInput = CreateInput();
    private readonly TextBox _englishNameInput = CreateInput();
    private readonly ComboBox _categoryInput = CreateCombo("حافلة", "سيارة أجرة", "شاحنة", "مركبة خدمة", "أخرى");
    private readonly NumericUpDown _passengerCapacityInput = CreateNumberInput();
    private readonly NumericUpDown _cargoCapacityInput = CreateNumberInput();
    private readonly ComboBox _statusInput = CreateCombo("نشط", "موقوف");
    private readonly TextBox _notesInput = new() { Multiline = true, Height = 62, ScrollBars = ScrollBars.Vertical };
    private readonly TextBox _searchInput = new();
    private readonly ComboBox _statusFilter = CreateCombo("الكل", "نشط", "موقوف");
    private readonly Label _pageLabel = new();
    private readonly Label _auditLabel = new();
    private readonly Label _usageLabel = new();
    private VehicleTypeRecord? _selectedRecord;
    private bool _isDirty;
    private int _currentPage = 1;
    private int _printCount;

    public FrmVehicleTypes()
    {
        Text = "GEN-008 — أنواع المركبات";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        Font = new Font("Segoe UI", 10F);
        BackColor = Color.FromArgb(247, 249, 252);
        Dock = DockStyle.Fill;
        TopLevel = false;
        FormBorderStyle = FormBorderStyle.None;

        BuildLayout();
        SeedRecords();
        RefreshGrid();
        NewRecord();
    }

    private void BuildLayout()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 6,
            Padding = new Padding(16),
            BackColor = BackColor
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 36));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 64));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        root.Controls.Add(BuildToolbar(), 0, 0);
        root.Controls.Add(BuildDataTabs(), 0, 1);
        root.Controls.Add(BuildSearchPanel(), 0, 2);
        root.Controls.Add(BuildGrid(), 0, 3);
        root.Controls.Add(BuildPagingPanel(), 0, 4);
        root.Controls.Add(BuildAuditPanel(), 0, 5);
        Controls.Add(root);

        TrackChanges(_codeInput, _arabicNameInput, _englishNameInput, _categoryInput,
            _passengerCapacityInput, _cargoCapacityInput, _statusInput, _notesInput);
    }

    private Control BuildToolbar()
    {
        var toolbar = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            WrapContents = false,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(8),
            BackColor = Color.FromArgb(28, 80, 130)
        };
        AddButton(toolbar, "جديد", (_, _) => NewRecord(), Color.FromArgb(47, 128, 237));
        AddButton(toolbar, "حفظ", (_, _) => SaveRecord(), Color.FromArgb(39, 174, 96));
        AddButton(toolbar, "تعديل", (_, _) => EditRecord(), Color.FromArgb(242, 153, 74));
        AddButton(toolbar, "إيقاف", (_, _) => ToggleStatus(), Color.FromArgb(112, 112, 112));
        AddButton(toolbar, "حذف", (_, _) => DeleteRecord(), Color.FromArgb(235, 87, 87));
        AddButton(toolbar, "طباعة", (_, _) => PrintPreview(), Color.FromArgb(155, 81, 224));
        return toolbar;
    }

    private Control BuildDataTabs()
    {
        var tabs = new TabControl
        {
            Dock = DockStyle.Fill,
            RightToLeft = RightToLeft.Yes,
            RightToLeftLayout = true,
            Padding = new Point(14, 5)
        };
        var page = new TabPage("البيانات الرئيسية") { BackColor = BackColor, Padding = new Padding(12) };
        page.Controls.Add(BuildDataForm());
        tabs.TabPages.Add(page);
        return tabs;
    }

    private Control BuildDataForm()
    {
        var fields = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 4,
            RightToLeft = RightToLeft.Yes,
            Padding = new Padding(8)
        };
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));

        AddField(fields, 0, "كود النوع *", _codeInput);
        AddField(fields, 1, "الاسم العربي *", _arabicNameInput);
        AddField(fields, 2, "الاسم الإنجليزي", _englishNameInput);
        AddField(fields, 3, "فئة المركبة *", _categoryInput);
        AddField(fields, 4, "سعة الركاب", _passengerCapacityInput);
        AddField(fields, 5, "سعة الحمولة", _cargoCapacityInput);
        AddField(fields, 6, "الحالة *", _statusInput);
        AddField(fields, 7, "ملاحظات", _notesInput);
        return fields;
    }

    private Control BuildSearchPanel()
    {
        var search = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            WrapContents = false,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(0, 10, 0, 8)
        };
        search.Controls.Add(new Label { Text = "البحث والتصفية:", AutoSize = true, Padding = new Padding(4, 8, 5, 0), Font = new Font(Font, FontStyle.Bold) });
        _searchInput.Width = 270;
        _searchInput.PlaceholderText = "ابحث بالكود أو الاسم أو الفئة";
        _searchInput.TextChanged += (_, _) => { _currentPage = 1; RefreshGrid(); };
        search.Controls.Add(_searchInput);
        _statusFilter.Width = 125;
        _statusFilter.SelectedIndexChanged += (_, _) => { _currentPage = 1; RefreshGrid(); };
        search.Controls.Add(_statusFilter);
        var clear = new Button { Text = "مسح التصفية", AutoSize = true };
        clear.Click += (_, _) => { _searchInput.Clear(); _statusFilter.SelectedIndex = 0; };
        search.Controls.Add(clear);
        return search;
    }

    private Control BuildGrid()
    {
        _grid.Dock = DockStyle.Fill;
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.AutoGenerateColumns = false;
        _grid.BackgroundColor = Color.White;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        _grid.RightToLeft = RightToLeft.Yes;
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(VehicleTypeRecord.Code), HeaderText = "الكود", Width = 105 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(VehicleTypeRecord.ArabicName), HeaderText = "الاسم العربي", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(VehicleTypeRecord.Category), HeaderText = "الفئة", Width = 140 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(VehicleTypeRecord.PassengerCapacity), HeaderText = "سعة الركاب", Width = 110 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(VehicleTypeRecord.CargoCapacity), HeaderText = "سعة الحمولة", Width = 110 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(VehicleTypeRecord.Status), HeaderText = "الحالة", Width = 100 });
        _grid.CellClick += (_, _) => LoadSelectedRecord();
        return _grid;
    }

    private Control BuildPagingPanel()
    {
        var paging = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(0, 8, 0, 8)
        };
        AddButton(paging, "الأول", (_, _) => GoToPage(1), Color.FromArgb(91, 104, 121));
        AddButton(paging, "السابق", (_, _) => GoToPage(_currentPage - 1), Color.FromArgb(91, 104, 121));
        _pageLabel.AutoSize = true;
        _pageLabel.Padding = new Padding(14, 8, 14, 0);
        paging.Controls.Add(_pageLabel);
        AddButton(paging, "التالي", (_, _) => GoToPage(_currentPage + 1), Color.FromArgb(91, 104, 121));
        AddButton(paging, "الأخير", (_, _) => GoToPage(GetPageCount()), Color.FromArgb(91, 104, 121));
        return paging;
    }

    private Control BuildAuditPanel()
    {
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            FlowDirection = FlowDirection.RightToLeft,
            BackColor = Color.White,
            Padding = new Padding(10, 6, 10, 6)
        };
        _auditLabel.AutoSize = true;
        _usageLabel.AutoSize = true;
        panel.Controls.Add(_auditLabel);
        panel.Controls.Add(new Label { Text = " | ", AutoSize = true });
        panel.Controls.Add(_usageLabel);
        return panel;
    }

    private void AddField(TableLayoutPanel host, int index, string label, Control input)
    {
        var row = index / 2;
        var column = (index % 2) * 2;
        host.Controls.Add(new Label { Text = label, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight, Padding = new Padding(4) }, column, row);
        input.Dock = DockStyle.Fill;
        input.Margin = new Padding(6, 4, 18, 4);
        if (label.EndsWith("*", StringComparison.Ordinal))
        {
            input.BackColor = Color.FromArgb(255, 252, 220);
        }
        host.Controls.Add(input, column + 1, row);
    }

    private void SeedRecords()
    {
        _records.Add(new VehicleTypeRecord("VT-001", "حافلة كبيرة", "Large Bus", "حافلة", 49, 0, "نشط", "مركبة تشغيل بين المدن", "مدير النظام", DateTime.Now.AddDays(-8)));
        _records.Add(new VehicleTypeRecord("VT-002", "شاحنة نقل", "Cargo Truck", "شاحنة", 2, 12, "نشط", string.Empty, "مدير النظام", DateTime.Now.AddDays(-2)));
    }

    private void NewRecord()
    {
        if (!ConfirmDiscardChanges()) return;
        _selectedRecord = null;
        _codeInput.Text = $"VT-{_records.Count + 1:000}";
        _arabicNameInput.Clear();
        _englishNameInput.Clear();
        _categoryInput.SelectedIndex = -1;
        _passengerCapacityInput.Value = 0;
        _cargoCapacityInput.Value = 0;
        _statusInput.SelectedIndex = 0;
        _notesInput.Clear();
        _isDirty = false;
        RefreshAudit();
        _arabicNameInput.Focus();
    }

    private void SaveRecord()
    {
        if (!ValidateInputs()) return;
        if (_selectedRecord is not null)
        {
            EditRecord();
            return;
        }

        if (_records.Any(record => string.Equals(record.Code, _codeInput.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show("كود النوع مستخدم بالفعل.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _codeInput.Focus();
            return;
        }

        var record = CreateRecord();
        _records.Add(record);
        _selectedRecord = record;
        _isDirty = false;
        RefreshGrid();
    }

    private void EditRecord()
    {
        if (_selectedRecord is null)
        {
            MessageBox.Show("اختر سجلًا من الجدول أولًا.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!ValidateInputs()) return;
        _selectedRecord.Code = _codeInput.Text.Trim();
        _selectedRecord.ArabicName = _arabicNameInput.Text.Trim();
        _selectedRecord.EnglishName = _englishNameInput.Text.Trim();
        _selectedRecord.Category = _categoryInput.Text;
        _selectedRecord.PassengerCapacity = (int)_passengerCapacityInput.Value;
        _selectedRecord.CargoCapacity = _cargoCapacityInput.Value;
        _selectedRecord.Status = _statusInput.Text;
        _selectedRecord.Notes = _notesInput.Text.Trim();
        _selectedRecord.ModifiedBy = "المستخدم الحالي";
        _selectedRecord.ModifiedAt = DateTime.Now;
        _selectedRecord.EditCount++;
        _isDirty = false;
        RefreshGrid();
    }

    private void ToggleStatus()
    {
        if (_selectedRecord is null)
        {
            MessageBox.Show("اختر سجلًا من الجدول أولًا.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        _selectedRecord.Status = _selectedRecord.Status == "نشط" ? "موقوف" : "نشط";
        _statusInput.Text = _selectedRecord.Status;
        _selectedRecord.ModifiedBy = "المستخدم الحالي";
        _selectedRecord.ModifiedAt = DateTime.Now;
        _selectedRecord.EditCount++;
        _isDirty = false;
        RefreshGrid();
    }

    private void DeleteRecord()
    {
        if (_selectedRecord is null)
        {
            MessageBox.Show("اختر سجلًا من الجدول أولًا.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (MessageBox.Show("هل تريد حذف السجل المحدد؟", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
        {
            return;
        }
        _records.Remove(_selectedRecord);
        _selectedRecord = null;
        _isDirty = false;
        RefreshGrid();
        NewRecord();
    }

    private void PrintPreview()
    {
        _printCount++;
        RefreshAudit();
        MessageBox.Show($"تم تجهيز {GetFilteredRecords().Count} سجلًا للطباعة.", "طباعة أنواع المركبات", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void LoadSelectedRecord()
    {
        if (_grid.CurrentRow?.DataBoundItem is not VehicleTypeRecord record) return;
        _selectedRecord = record;
        _codeInput.Text = record.Code;
        _arabicNameInput.Text = record.ArabicName;
        _englishNameInput.Text = record.EnglishName;
        _categoryInput.Text = record.Category;
        _passengerCapacityInput.Value = record.PassengerCapacity;
        _cargoCapacityInput.Value = record.CargoCapacity;
        _statusInput.Text = record.Status;
        _notesInput.Text = record.Notes;
        _isDirty = false;
        RefreshAudit();
    }

    private void RefreshGrid()
    {
        var filtered = GetFilteredRecords();
        var pages = Math.Max(1, (int)Math.Ceiling(filtered.Count / (double)PageSize));
        _currentPage = Math.Clamp(_currentPage, 1, pages);
        _grid.DataSource = new BindingList<VehicleTypeRecord>(filtered.Skip((_currentPage - 1) * PageSize).Take(PageSize).ToList());
        _pageLabel.Text = $"صفحة {_currentPage} من {pages} | {filtered.Count} سجل";
        RefreshAudit();
    }

    private List<VehicleTypeRecord> GetFilteredRecords()
    {
        var search = _searchInput.Text.Trim();
        var status = _statusFilter.Text;
        return _records.Where(record =>
            (string.IsNullOrWhiteSpace(search)
                || record.Code.Contains(search, StringComparison.OrdinalIgnoreCase)
                || record.ArabicName.Contains(search, StringComparison.OrdinalIgnoreCase)
                || record.Category.Contains(search, StringComparison.OrdinalIgnoreCase))
            && (string.IsNullOrWhiteSpace(status) || status == "الكل" || record.Status == status)).ToList();
    }

    private int GetPageCount() => Math.Max(1, (int)Math.Ceiling(GetFilteredRecords().Count / (double)PageSize));

    private void GoToPage(int page)
    {
        _currentPage = Math.Clamp(page, 1, GetPageCount());
        RefreshGrid();
    }

    private bool ValidateInputs()
    {
        if (string.IsNullOrWhiteSpace(_codeInput.Text)) return ShowValidationError(_codeInput, "كود النوع");
        if (string.IsNullOrWhiteSpace(_arabicNameInput.Text)) return ShowValidationError(_arabicNameInput, "الاسم العربي");
        if (string.IsNullOrWhiteSpace(_categoryInput.Text)) return ShowValidationError(_categoryInput, "فئة المركبة");
        if (string.IsNullOrWhiteSpace(_statusInput.Text)) return ShowValidationError(_statusInput, "الحالة");
        return true;
    }

    private bool ShowValidationError(Control control, string fieldName)
    {
        MessageBox.Show($"حقل «{fieldName}» إلزامي.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        control.Focus();
        return false;
    }

    private VehicleTypeRecord CreateRecord() => new(
        _codeInput.Text.Trim(), _arabicNameInput.Text.Trim(), _englishNameInput.Text.Trim(),
        _categoryInput.Text, (int)_passengerCapacityInput.Value, _cargoCapacityInput.Value,
        _statusInput.Text, _notesInput.Text.Trim(), "المستخدم الحالي", DateTime.Now);

    private void RefreshAudit()
    {
        if (_selectedRecord is null)
        {
            _auditLabel.Text = "سجل جديد — أنشئ بواسطة: المستخدم الحالي";
            _usageLabel.Text = $"مرات التعديل: 0 | مرات الطباعة: {_printCount}";
            return;
        }
        _auditLabel.Text = $"أنشئ بواسطة: {_selectedRecord.CreatedBy} في {_selectedRecord.CreatedAt:yyyy/MM/dd HH:mm} | آخر تعديل: {_selectedRecord.ModifiedBy} في {_selectedRecord.ModifiedAt:yyyy/MM/dd HH:mm}";
        _usageLabel.Text = $"مرات التعديل: {_selectedRecord.EditCount} | مرات الطباعة: {_printCount}";
    }

    private void TrackChanges(params Control[] controls)
    {
        foreach (var control in controls)
        {
            switch (control)
            {
                case TextBox textBox:
                    textBox.TextChanged += (_, _) => _isDirty = true;
                    break;
                case ComboBox comboBox:
                    comboBox.SelectedIndexChanged += (_, _) => _isDirty = true;
                    break;
                case NumericUpDown numeric:
                    numeric.ValueChanged += (_, _) => _isDirty = true;
                    break;
            }
        }
        FormClosing += (_, eventArgs) =>
        {
            if (!ConfirmDiscardChanges()) eventArgs.Cancel = true;
        };
    }

    private bool ConfirmDiscardChanges()
    {
        if (!_isDirty) return true;

        return ShowUnsavedChangesPrompt() switch
        {
            UnsavedChangesChoice.Save => SaveAndConfirm(),
            UnsavedChangesChoice.Discard => true,
            _ => false
        };
    }

    private bool SaveAndConfirm()
    {
        SaveRecord();
        return !_isDirty;
    }

    private UnsavedChangesChoice ShowUnsavedChangesPrompt()
    {
        var choice = UnsavedChangesChoice.Cancel;
        using var dialog = new Form
        {
            Text = "تغييرات غير محفوظة",
            RightToLeft = RightToLeft.Yes,
            RightToLeftLayout = true,
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MinimizeBox = false,
            MaximizeBox = false,
            ShowInTaskbar = false,
            ClientSize = new Size(430, 145)
        };

        var message = new Label
        {
            Dock = DockStyle.Top,
            Height = 70,
            Padding = new Padding(18),
            Text = "توجد تغييرات غير محفوظة. اختر الإجراء المطلوب قبل إغلاق التبويب.",
            TextAlign = ContentAlignment.MiddleRight
        };
        var commands = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 56,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(12, 8, 12, 8)
        };
        AddPromptButton(commands, "حفظ", UnsavedChangesChoice.Save);
        AddPromptButton(commands, "تجاهل", UnsavedChangesChoice.Discard);
        AddPromptButton(commands, "إلغاء", UnsavedChangesChoice.Cancel);
        dialog.Controls.Add(message);
        dialog.Controls.Add(commands);
        dialog.ShowDialog(this);

        return choice;

        void AddPromptButton(FlowLayoutPanel host, string text, UnsavedChangesChoice action)
        {
            var button = new Button { Text = text, AutoSize = true, Height = 32, DialogResult = DialogResult.None };
            button.Click += (_, _) => { choice = action; dialog.Close(); };
            host.Controls.Add(button);
        }
    }

    private static TextBox CreateInput() => new() { Height = 30 };

    private static ComboBox CreateCombo(params string[] items)
    {
        var combo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Height = 30 };
        combo.Items.AddRange(items);
        return combo;
    }

    private static NumericUpDown CreateNumberInput() => new()
    {
        Minimum = 0,
        Maximum = 9_999_999,
        DecimalPlaces = 2,
        ThousandsSeparator = true,
        Height = 30
    };

    private static void AddButton(FlowLayoutPanel host, string text, EventHandler click, Color color)
    {
        var button = new Button
        {
            Text = text,
            AutoSize = true,
            Height = 36,
            Margin = new Padding(4),
            BackColor = color,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        button.FlatAppearance.BorderSize = 0;
        button.Click += click;
        host.Controls.Add(button);
    }

    private sealed class VehicleTypeRecord
    {
        public VehicleTypeRecord(string code, string arabicName, string englishName, string category,
            int passengerCapacity, decimal cargoCapacity, string status, string notes,
            string createdBy, DateTime createdAt)
        {
            Code = code;
            ArabicName = arabicName;
            EnglishName = englishName;
            Category = category;
            PassengerCapacity = passengerCapacity;
            CargoCapacity = cargoCapacity;
            Status = status;
            Notes = notes;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = createdBy;
            ModifiedAt = createdAt;
        }

        public string Code { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string Category { get; set; }
        public int PassengerCapacity { get; set; }
        public decimal CargoCapacity { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; }
        public DateTime CreatedAt { get; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public int EditCount { get; set; }
    }
}
