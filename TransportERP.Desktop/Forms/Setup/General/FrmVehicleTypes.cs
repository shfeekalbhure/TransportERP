using System.ComponentModel;
using System.Drawing;
using TransportERP.Contracts.Setup.VehicleTypes;
using TransportERP.Desktop.Services;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>GEN-008 — أنواع المركبات؛ لا تحتفظ هذه الشاشة بأي بيانات تشغيلية محلية.</summary>
public sealed class FrmVehicleTypes : Form
{
    private readonly IVehicleTypesApiClient _client;
    private readonly TextBox _code = new();
    private readonly TextBox _arabicName = new();
    private readonly TextBox _englishName = new();
    private readonly ComboBox _category = NewCombo("حافلة", "سيارة أجرة", "شاحنة", "مركبة خدمة", "أخرى");
    private readonly NumericUpDown _passengers = NewNumber();
    private readonly NumericUpDown _cargo = NewNumber();
    private readonly ComboBox _status = NewCombo("نشط", "موقوف");
    private readonly TextBox _notes = new() { Multiline = true, Height = 58, ScrollBars = ScrollBars.Vertical };
    private readonly TextBox _search = new();
    private readonly ComboBox _filterStatus = NewCombo("الكل", "نشط", "موقوف");
    private readonly DataGridView _grid = new();
    private readonly Label _result = new() { AutoSize = true };
    private readonly Label _audit = new() { AutoSize = true };
    private VehicleTypeDto? _selected;
    private bool _dirty;
    private int _page = 1;

    public FrmVehicleTypes(IVehicleTypesApiClient? client = null)
    {
        _client = client ?? VehicleTypesApiClient.CreateDefault();
        Text = "GEN-008 — أنواع المركبات";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        Font = new Font("Segoe UI", 10F);
        BackColor = Color.FromArgb(247, 249, 252);
        Dock = DockStyle.Fill;
        TopLevel = false;
        FormBorderStyle = FormBorderStyle.None;
        BuildLayout();
        Shown += async (_, _) => await LoadAsync();
        FormClosing += async (_, e) => { if (_dirty && !await ConfirmCloseAsync()) e.Cancel = true; };
    }

    private void BuildLayout()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 6, Padding = new Padding(16), BackColor = BackColor };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 34));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 66));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.Controls.Add(BuildToolbar(), 0, 0);
        root.Controls.Add(BuildTabs(), 0, 1);
        root.Controls.Add(BuildSearch(), 0, 2);
        root.Controls.Add(BuildGrid(), 0, 3);
        root.Controls.Add(BuildPager(), 0, 4);
        root.Controls.Add(_audit, 0, 5);
        Controls.Add(root);
        Track(_code, _arabicName, _englishName, _category, _passengers, _cargo, _status, _notes);
    }

    private Control BuildToolbar()
    {
        var panel = Bar();
        AddButton(panel, "جديد", (_, _) => NewRecord(), Color.FromArgb(47, 128, 237));
        AddButton(panel, "حفظ", async (_, _) => await SaveAsync(), Color.FromArgb(39, 174, 96));
        AddButton(panel, "تعديل", async (_, _) => await SaveAsync(), Color.FromArgb(242, 153, 74));
        AddButton(panel, "إيقاف", async (_, _) => await SuspendAsync(), Color.FromArgb(112, 112, 112));
        AddButton(panel, "حذف", async (_, _) => await DeleteAsync(), Color.FromArgb(235, 87, 87));
        AddButton(panel, "طباعة", (_, _) => MessageBox.Show("الطباعة تتطلب خدمة التقارير المعتمدة.", Text), Color.FromArgb(155, 81, 224));
        return panel;
    }

    private Control BuildTabs()
    {
        var tabs = new TabControl { Dock = DockStyle.Fill, RightToLeft = RightToLeft.Yes, RightToLeftLayout = true };
        var page = new TabPage("البيانات الرئيسية") { BackColor = BackColor, Padding = new Padding(10) };
        var form = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 4, RightToLeft = RightToLeft.Yes };
        form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14)); form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36));
        form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14)); form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36));
        AddField(form, 0, "كود النوع *", _code, true); AddField(form, 1, "الاسم العربي *", _arabicName, true);
        AddField(form, 2, "الاسم الإنجليزي", _englishName, false); AddField(form, 3, "فئة المركبة *", _category, true);
        AddField(form, 4, "سعة الركاب", _passengers, false); AddField(form, 5, "سعة الحمولة", _cargo, false);
        AddField(form, 6, "الحالة *", _status, true); AddField(form, 7, "ملاحظات", _notes, false);
        page.Controls.Add(form); tabs.TabPages.Add(page); return tabs;
    }

    private Control BuildSearch()
    {
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(0, 8, 0, 8) };
        panel.Controls.Add(new Label { Text = "البحث والتصفية:", AutoSize = true, Padding = new Padding(3, 8, 3, 0) });
        _search.Width = 260; _search.PlaceholderText = "الكود أو الاسم أو الفئة"; _search.TextChanged += async (_, _) => { _page = 1; await LoadAsync(); }; panel.Controls.Add(_search);
        _filterStatus.SelectedIndexChanged += async (_, _) => { _page = 1; await LoadAsync(); }; panel.Controls.Add(_filterStatus);
        var clear = new Button { Text = "مسح التصفية", AutoSize = true }; clear.Click += (_, _) => { _search.Clear(); _filterStatus.SelectedIndex = 0; }; panel.Controls.Add(clear);
        panel.Controls.Add(_result); return panel;
    }

    private Control BuildGrid()
    {
        _grid.Dock = DockStyle.Fill; _grid.ReadOnly = true; _grid.AllowUserToAddRows = false; _grid.AutoGenerateColumns = false;
        _grid.BackgroundColor = Color.White; _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; _grid.MultiSelect = false;
        AddColumn(nameof(VehicleTypeDto.Code), "الكود", 110); AddColumn(nameof(VehicleTypeDto.ArabicName), "الاسم العربي", 200);
        AddColumn(nameof(VehicleTypeDto.Category), "الفئة", 140); AddColumn(nameof(VehicleTypeDto.PassengerCapacity), "سعة الركاب", 110);
        AddColumn(nameof(VehicleTypeDto.CargoCapacity), "سعة الحمولة", 110); AddColumn(nameof(VehicleTypeDto.Status), "الحالة", 100);
        _grid.CellClick += (_, _) => LoadSelection(); return _grid;
    }

    private Control BuildPager()
    {
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(0, 8, 0, 8) };
        AddButton(panel, "الأول", async (_, _) => { _page = 1; await LoadAsync(); }, Color.Gray);
        AddButton(panel, "السابق", async (_, _) => { _page = Math.Max(1, _page - 1); await LoadAsync(); }, Color.Gray);
        AddButton(panel, "التالي", async (_, _) => { _page++; await LoadAsync(); }, Color.Gray);
        return panel;
    }

    private async Task LoadAsync()
    {
        try
        {
            var status = _filterStatus.Text switch { "نشط" => VehicleTypeStatus.Active, "موقوف" => VehicleTypeStatus.Suspended, _ => (VehicleTypeStatus?)null };
            var response = await _client.SearchAsync(new VehicleTypeSearchRequest(_search.Text, status, _page), CancellationToken.None);
            _grid.DataSource = new BindingList<VehicleTypeDto>(response.Items.ToList());
            _result.Text = response.StorageAvailable ? $"عدد السجلات: {response.TotalCount}" : response.Message ?? "مانع التخزين المعتمد";
            if (!response.StorageAvailable) _audit.Text = $"الحالة: {response.BlockerCode} — لا توجد بيانات بديلة.";
        }
        catch (HttpRequestException)
        {
            _grid.DataSource = new BindingList<VehicleTypeDto>();
            _result.Text = "تعذر الاتصال بخدمة أنواع المركبات.";
        }
    }

    private async Task SaveAsync()
    {
        if (!ValidateInputs()) return;
        var response = _selected is null
            ? await _client.CreateAsync(new CreateVehicleTypeRequest(_code.Text.Trim(), _arabicName.Text.Trim(), EmptyToNull(_englishName), _category.Text, ToNullableInt(_passengers), ToNullable(_cargo), ToStatus(), EmptyToNull(_notes)), CancellationToken.None)
            : await _client.UpdateAsync(_selected.Id, new UpdateVehicleTypeRequest(_arabicName.Text.Trim(), EmptyToNull(_englishName), _category.Text, ToNullableInt(_passengers), ToNullable(_cargo), ToStatus(), EmptyToNull(_notes)), CancellationToken.None);
        ShowResponse(response); if (response.Succeeded) { _dirty = false; await LoadAsync(); }
    }

    private async Task SuspendAsync()
    {
        if (_selected is null) { MessageBox.Show("اختر سجلًا من الجدول أولًا.", Text); return; }
        ShowResponse(await _client.SuspendAsync(_selected.Id, CancellationToken.None));
        await LoadAsync();
    }

    private async Task DeleteAsync()
    {
        if (_selected is null) { MessageBox.Show("اختر سجلًا من الجدول أولًا.", Text); return; }
        if (MessageBox.Show("هل تريد حذف السجل المحدد؟", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        ShowResponse(await _client.DeleteAsync(_selected.Id, CancellationToken.None));
        await LoadAsync();
    }

    private void NewRecord()
    {
        _selected = null; _code.Clear(); _arabicName.Clear(); _englishName.Clear(); _category.SelectedIndex = -1; _passengers.Value = 0; _cargo.Value = 0; _status.SelectedIndex = 0; _notes.Clear(); _dirty = false; _audit.Text = "سجل جديد";
    }

    private void LoadSelection()
    {
        if (_grid.CurrentRow?.DataBoundItem is not VehicleTypeDto item) return;
        _selected = item; _code.Text = item.Code; _arabicName.Text = item.ArabicName; _englishName.Text = item.EnglishName ?? string.Empty; _category.Text = item.Category;
        _passengers.Value = item.PassengerCapacity ?? 0; _cargo.Value = item.CargoCapacity ?? 0; _status.Text = item.Status == VehicleTypeStatus.Active ? "نشط" : "موقوف"; _notes.Text = item.Notes ?? string.Empty; _dirty = false;
        _audit.Text = $"أنشئ بواسطة: {item.CreatedBy} في {item.CreatedAt:yyyy/MM/dd HH:mm} | آخر تعديل: {item.ModifiedBy ?? "—"} | مرات التعديل: {item.EditCount} | مرات الطباعة: {item.PrintCount}";
    }

    private bool ValidateInputs()
    {
        if (string.IsNullOrWhiteSpace(_code.Text) || string.IsNullOrWhiteSpace(_arabicName.Text) || string.IsNullOrWhiteSpace(_category.Text) || string.IsNullOrWhiteSpace(_status.Text))
        { MessageBox.Show("كود النوع والاسم العربي وفئة المركبة والحالة حقول إلزامية.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
        return true;
    }

    private async Task<bool> ConfirmCloseAsync()
    {
        var result = MessageBox.Show("توجد تغييرات غير محفوظة. اختر نعم للحفظ، أو لا للتجاهل، أو إلغاء للعودة.", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
        if (result == DialogResult.Cancel) return false;
        if (result == DialogResult.Yes) await SaveAsync();
        return !_dirty || result == DialogResult.No;
    }

    private void Track(params Control[] controls)
    {
        foreach (var control in controls)
        {
            if (control is TextBox textBox) textBox.TextChanged += (_, _) => _dirty = true;
            else if (control is ComboBox comboBox) comboBox.SelectedIndexChanged += (_, _) => _dirty = true;
            else if (control is NumericUpDown number) number.ValueChanged += (_, _) => _dirty = true;
        }
    }

    private void ShowResponse(VehicleTypeCommandResponse response)
        => MessageBox.Show(response.Message ?? (response.Succeeded ? "تم الحفظ." : "تعذر تنفيذ العملية."), Text, MessageBoxButtons.OK, response.Succeeded ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

    private void AddColumn(string property, string title, int width) => _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = property, HeaderText = title, Width = width });
    private static FlowLayoutPanel Bar() => new() { Dock = DockStyle.Fill, AutoSize = true, WrapContents = false, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8), BackColor = Color.FromArgb(28, 80, 130) };
    private static void AddButton(FlowLayoutPanel panel, string text, EventHandler handler, Color color) { var button = new Button { Text = text, AutoSize = true, Height = 36, Margin = new Padding(4), BackColor = color, ForeColor = Color.White, FlatStyle = FlatStyle.Flat }; button.FlatAppearance.BorderSize = 0; button.Click += handler; panel.Controls.Add(button); }
    private static ComboBox NewCombo(params string[] items) { var box = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Height = 30 }; box.Items.AddRange(items); return box; }
    private static NumericUpDown NewNumber() => new() { Minimum = 0, Maximum = 9_999_999, DecimalPlaces = 2, ThousandsSeparator = true, Height = 30 };
    private static void AddField(TableLayoutPanel panel, int index, string label, Control input, bool required) { var row = index / 2; var col = (index % 2) * 2; panel.Controls.Add(new Label { Text = label, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight }, col, row); input.Dock = DockStyle.Fill; input.Margin = new Padding(6, 4, 16, 4); if (required) input.BackColor = Color.FromArgb(255, 252, 220); panel.Controls.Add(input, col + 1, row); }
    private static string? EmptyToNull(TextBox box) => string.IsNullOrWhiteSpace(box.Text) ? null : box.Text.Trim();
    private static int? ToNullableInt(NumericUpDown value) => value.Value == 0 ? null : (int)value.Value;
    private static decimal? ToNullable(NumericUpDown value) => value.Value == 0 ? null : value.Value;
    private VehicleTypeStatus ToStatus() => _status.Text == "موقوف" ? VehicleTypeStatus.Suspended : VehicleTypeStatus.Active;
}
