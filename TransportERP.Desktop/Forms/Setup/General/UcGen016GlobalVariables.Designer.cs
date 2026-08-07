using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.Forms.Setup.General;

partial class UcGen016GlobalVariables
{
    private System.ComponentModel.IContainer? components;
    private TransportReferenceScreenShell screenShell = null!;
    private TabControl tabControl = null!;
    private TabPage tabMain = null!;
    private TabPage tabScope = null!;
    private TabPage tabEffective = null!;
    private TabPage tabChangeLog = null!;
    private TransportDataEntryPanel mainFields = null!;
    private TransportDataEntryPanel scopeFields = null!;
    private TransportDataEntryPanel effectiveFields = null!;
    private TransportDataGrid changeLogGrid = null!;
    private TextBox txtPropertyCode = null!;
    private TextBox txtArabicName = null!;
    private TextBox txtEnglishName = null!;
    private TextBox txtGroup = null!;
    private TextBox txtDescription = null!;
    private TextBox txtValueType = null!;
    private TextBox txtAllowedScopes = null!;
    private TextBox txtDefaultValue = null!;
    private TextBox txtCurrentOverride = null!;
    private TextBox txtEffectiveValue = null!;
    private TextBox txtValueSource = null!;
    private TextBox txtResolutionPolicy = null!;
    private TextBox txtStatus = null!;
    private ComboBox cmbScope = null!;
    private TextBox txtScopeIdentity = null!;
    private Panel pnlOverrideEditor = null!;
    private DateTimePicker dtEffectiveFrom = null!;
    private DateTimePicker dtEffectiveTo = null!;
    private TextBox txtReason = null!;
    private TextBox txtValidation = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        screenShell = new TransportReferenceScreenShell();
        tabControl = new TabControl();
        tabMain = new TabPage();
        tabScope = new TabPage();
        tabEffective = new TabPage();
        tabChangeLog = new TabPage();
        mainFields = new TransportDataEntryPanel();
        scopeFields = new TransportDataEntryPanel();
        effectiveFields = new TransportDataEntryPanel();
        changeLogGrid = new TransportDataGrid();

        txtPropertyCode = ReadOnlyTextBox();
        txtArabicName = ReadOnlyTextBox();
        txtEnglishName = ReadOnlyTextBox();
        txtGroup = ReadOnlyTextBox();
        txtDescription = ReadOnlyTextBox();
        txtValueType = ReadOnlyTextBox();
        txtAllowedScopes = ReadOnlyTextBox();
        txtDefaultValue = ReadOnlyTextBox();
        txtCurrentOverride = ReadOnlyTextBox();
        txtEffectiveValue = ReadOnlyTextBox();
        txtValueSource = ReadOnlyTextBox();
        txtResolutionPolicy = ReadOnlyTextBox();
        txtStatus = ReadOnlyTextBox();
        cmbScope = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, RightToLeft = RightToLeft.Yes };
        txtScopeIdentity = ReadOnlyTextBox();
        pnlOverrideEditor = new Panel { AutoScroll = false, Margin = Padding.Empty, Padding = Padding.Empty };
        dtEffectiveFrom = OptionalDatePicker();
        dtEffectiveTo = OptionalDatePicker();
        txtReason = new TextBox { TextAlign = HorizontalAlignment.Right };
        txtValidation = ReadOnlyTextBox();

        SuspendLayout();

        screenShell.Dock = DockStyle.Fill;
        screenShell.RightToLeft = RightToLeft.Yes;
        screenShell.DataGroupTitle = "إعدادات التشغيل العامة والمتغيرات المشتركة";
        screenShell.AlertBar.Text = "GEN-016 — إعدادات التشغيل العامة والمتغيرات المشتركة";
        screenShell.SearchPanel.SearchPlaceholder = "ابحث برمز الخاصية أو الاسم أو المجموعة";
        screenShell.SearchPanel.SetStatusItems("نشط", "موقوف");
        screenShell.ConfigureWorkspaceMode(showSearch: true, showGrid: true, expandDataWorkspace: false);

        ConfigureTabs();
        ConfigureMainTab();
        ConfigureScopeTab();
        ConfigureEffectiveTab();
        ConfigureChangeLogTab();
        ConfigureMainGrid();

        screenShell.DataHost.Controls.Add(tabControl);

        AutoScaleMode = AutoScaleMode.Font;
        AutoScroll = false;
        BackColor = Color.White;
        Controls.Add(screenShell);
        Name = "UcGen016GlobalVariables";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1180, 760);

        ResumeLayout(false);
    }

    private void ConfigureTabs()
    {
        tabControl.Dock = DockStyle.Fill;
        tabControl.RightToLeft = RightToLeft.Yes;
        tabControl.RightToLeftLayout = true;
        tabControl.Multiline = false;
        tabControl.HotTrack = true;

        ConfigureTabPage(tabMain, "البيانات الرئيسية");
        ConfigureTabPage(tabScope, "النطاق والقيمة");
        ConfigureTabPage(tabEffective, "السريان والتحقق");
        ConfigureTabPage(tabChangeLog, "سجل التغييرات");

        tabControl.TabPages.AddRange(new[] { tabMain, tabScope, tabEffective, tabChangeLog });
    }

    private static void ConfigureTabPage(TabPage page, string title)
    {
        page.Text = title;
        page.RightToLeft = RightToLeft.Yes;
        page.BackColor = Color.White;
        page.Padding = new Padding(TransportUiMetrics.TabContentPadding);
        page.AutoScroll = false;
    }

    private void ConfigureMainTab()
    {
        mainFields.FieldColumnCount = 3;
        mainFields.Dock = DockStyle.Top;
        mainFields.AutoScroll = false;

        mainFields.AddField("PropertyCode", txtPropertyCode, 0);
        mainFields.AddField("الاسم العربي", txtArabicName, 1);
        mainFields.AddField("الاسم الإنجليزي", txtEnglishName, 2);
        mainFields.AddField("المجموعة", txtGroup, 3);
        mainFields.AddField("نوع القيمة", txtValueType, 4);
        mainFields.AddField("الحالة", txtStatus, 5);
        mainFields.AddField("AllowedScopes", txtAllowedScopes, 6);
        mainFields.AddField("ResolutionPolicy", txtResolutionPolicy, 7);
        mainFields.AddField("الوصف", txtDescription, 8);

        tabMain.Controls.Add(mainFields);
    }

    private void ConfigureScopeTab()
    {
        scopeFields.FieldColumnCount = 3;
        scopeFields.Dock = DockStyle.Top;
        scopeFields.AutoScroll = false;

        scopeFields.AddField("النطاق المحدد", cmbScope, 0);
        scopeFields.AddField("ScopeId", txtScopeIdentity, 1);
        scopeFields.AddField("Default Value", txtDefaultValue, 2);
        scopeFields.AddField("Current Override", pnlOverrideEditor, 3);
        scopeFields.AddField("القيمة المحملة", txtCurrentOverride, 4);
        scopeFields.AddField("Effective Value", txtEffectiveValue, 5);
        scopeFields.AddField("Value Source", txtValueSource, 6);

        tabScope.Controls.Add(scopeFields);
    }

    private void ConfigureEffectiveTab()
    {
        effectiveFields.FieldColumnCount = 2;
        effectiveFields.Dock = DockStyle.Top;
        effectiveFields.AutoScroll = false;

        effectiveFields.AddField("EffectiveFrom", dtEffectiveFrom, 0);
        effectiveFields.AddField("EffectiveTo", dtEffectiveTo, 1);
        effectiveFields.AddField("سبب التغيير", txtReason, 2);
        effectiveFields.AddField("قواعد التحقق", txtValidation, 3);

        tabEffective.Controls.Add(effectiveFields);
    }

    private void ConfigureChangeLogTab()
    {
        var container = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = false,
            Padding = Padding.Empty,
            Margin = Padding.Empty,
            RightToLeft = RightToLeft.Yes
        };

        changeLogGrid.Dock = DockStyle.Fill;
        changeLogGrid.AutoGenerateColumns = false;
        changeLogGrid.Columns.Clear();

        AddReadOnlyColumn(changeLogGrid, "PropertyCode", "PropertyCode", 170);
        AddReadOnlyColumn(changeLogGrid, "Scope", "Scope", 90);
        AddReadOnlyColumn(changeLogGrid, "ScopeId", "ScopeId", 120);
        AddReadOnlyColumn(changeLogGrid, "Old Value", "OldValue");
        AddReadOnlyColumn(changeLogGrid, "New Value", "NewValue");
        AddReadOnlyColumn(changeLogGrid, "المستخدم", "User", 120);
        AddReadOnlyColumn(changeLogGrid, "التاريخ والوقت", "Timestamp", 150);
        AddReadOnlyColumn(changeLogGrid, "السبب", "Reason");
        AddReadOnlyColumn(changeLogGrid, "EffectiveFrom", "EffectiveFrom", 130);
        AddReadOnlyColumn(changeLogGrid, "EffectiveTo", "EffectiveTo", 130);
        AddReadOnlyColumn(changeLogGrid, "حالة الموافقة", "ApprovalStatus", 120);

        container.Controls.Add(changeLogGrid);
        tabChangeLog.Controls.Add(container);
    }

    private void ConfigureMainGrid()
    {
        var grid = screenShell.Grid;
        grid.Dock = DockStyle.Fill;
        grid.RightToLeft = RightToLeft.Yes;
        grid.AutoGenerateColumns = false;
        grid.Columns.Clear();

        AddReadOnlyColumn(grid, "PropertyCode", nameof(PropertyCatalogRow.PropertyCode), 210);
        AddReadOnlyColumn(grid, "الاسم", nameof(PropertyCatalogRow.Name));
        AddReadOnlyColumn(grid, "المجموعة", nameof(PropertyCatalogRow.Group), 150);
        AddReadOnlyColumn(grid, "Scope", nameof(PropertyCatalogRow.Scope), 100);
        AddReadOnlyColumn(grid, "Effective Value", nameof(PropertyCatalogRow.EffectiveValue), 150);
        AddReadOnlyColumn(grid, "Value Source", nameof(PropertyCatalogRow.ValueSource), 130);
        AddReadOnlyColumn(grid, "الحالة", nameof(PropertyCatalogRow.Status), 85);
    }

    private static TextBox ReadOnlyTextBox() => new()
    {
        ReadOnly = true,
        TextAlign = HorizontalAlignment.Right,
        RightToLeft = RightToLeft.Yes,
        TabStop = false
    };

    private static DateTimePicker OptionalDatePicker() => new()
    {
        Format = DateTimePickerFormat.Short,
        ShowCheckBox = true,
        Checked = false,
        RightToLeft = RightToLeft.Yes,
        RightToLeftLayout = true
    };

    private static void AddReadOnlyColumn(DataGridView grid, string header, string propertyName, int? width = null)
    {
        var column = new DataGridViewTextBoxColumn
        {
            HeaderText = header,
            DataPropertyName = propertyName,
            ReadOnly = true,
            SortMode = DataGridViewColumnSortMode.Automatic
        };

        if (width.HasValue)
        {
            column.Width = width.Value;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        }
        else
        {
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        grid.Columns.Add(column);
    }
}
