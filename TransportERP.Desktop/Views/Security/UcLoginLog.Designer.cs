using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.CoreUI.Profiles;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.Views.Security;

partial class UcLoginLog
{
    private System.ComponentModel.IContainer? components = null;
    private TransportReferenceScreenShell screenShell = null!;
    private TransportLayoutRoleProvider profileMetadata = null!;
    private TransportDataEntryPanel filtersPanel = null!;
    private TransportDatePicker dtFrom = null!;
    private TransportDatePicker dtTo = null!;
    private TransportTextBox txtUser = null!;
    private TransportComboBox cmbCompany = null!;
    private TransportComboBox cmbBranch = null!;
    private TransportComboBox cmbResult = null!;
    private TransportTextBox txtIp = null!;
    private TransportComboBox cmbLoginType = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        profileMetadata = new TransportLayoutRoleProvider();
        components.Add(profileMetadata);

        screenShell = new TransportReferenceScreenShell();
        filtersPanel = new TransportDataEntryPanel();
        dtFrom = CreateOptionalDate();
        dtTo = CreateOptionalDate();
        txtUser = new TransportTextBox();
        cmbCompany = new TransportComboBox();
        cmbBranch = new TransportComboBox();
        cmbResult = new TransportComboBox();
        txtIp = new TransportTextBox();
        cmbLoginType = new TransportComboBox();

        SuspendLayout();

        screenShell.Dock = DockStyle.Fill;
        screenShell.RightToLeft = RightToLeft.Yes;
        screenShell.DataGroupTitle = "فلاتر سجل تسجيل الدخول";
        screenShell.GridGroup.Text = "سجل تسجيل الدخول";
        screenShell.SearchPanel.SearchPlaceholder = "ابحث بالمستخدم أو البريد أو IP أو الجهاز...";
        screenShell.SearchPanel.SetStatusItems("الكل", "ناجح", "فاشل");
        screenShell.ConfigureWorkspaceMode(showSearch: true, showGrid: true, expandDataWorkspace: false);
        screenShell.AuditGroup.Visible = false;

        // Filters ليست MainData. اختيار عمودين هنا خاص بمنطقة الفلاتر ولا يفرض قاعدة 3 أعمدة/5 صفوف عليها.
        filtersPanel.FieldColumnCount = 2;
        filtersPanel.Dock = DockStyle.Top;
        filtersPanel.AutoScroll = false;
        filtersPanel.Margin = Padding.Empty;
        filtersPanel.Padding = Padding.Empty;

        cmbResult.Items.AddRange(new object[] { "الكل", "ناجح", "فاشل" });
        cmbLoginType.Items.AddRange(new object[] { "الكل", "كلمة مرور", "MFA", "جلسة موثوقة", "تكامل" });

        filtersPanel.AddField("من تاريخ", dtFrom, 0);
        filtersPanel.AddField("إلى تاريخ", dtTo, 1);
        filtersPanel.AddField("المستخدم أو البريد", txtUser, 2);
        filtersPanel.AddField("الشركة", cmbCompany, 3);
        filtersPanel.AddField("الفرع", cmbBranch, 4);
        filtersPanel.AddField("النتيجة", cmbResult, 5);
        filtersPanel.AddField("عنوان IP", txtIp, 6);
        filtersPanel.AddField("نوع الدخول", cmbLoginType, 7);
        screenShell.DataHost.Controls.Add(filtersPanel);

        ConfigureLogGrid();
        ConfigureProfileMetadata();

        AutoScaleMode = AutoScaleMode.Font;
        BackColor = UiTheme.WorkspaceBackground;
        Controls.Add(screenShell);
        Dock = DockStyle.Fill;
        Name = "UcLoginLog";
        RightToLeft = RightToLeft.Yes;
        ScreenProfile = TransportScreenProfile.ReadOnlyLog;
        Size = new Size(1280, 760);

        ResumeLayout(false);
    }

    private void ConfigureLogGrid()
    {
        var grid = screenShell.Grid;
        grid.AutoGenerateColumns = false;
        grid.Columns.Clear();
        AddLogColumn(grid, "التاريخ والوقت", 145);
        AddLogColumn(grid, "المستخدم", 145);
        AddLogColumn(grid, "الشركة", 130);
        AddLogColumn(grid, "الفرع", 120);
        AddLogColumn(grid, "النتيجة", 90);
        AddLogColumn(grid, "سبب الفشل", null);
        AddLogColumn(grid, "IP", 120);
        AddLogColumn(grid, "الجهاز", 140);
        AddLogColumn(grid, "نوع الدخول", 110);
    }

    private void ConfigureProfileMetadata()
    {
        profileMetadata.SetLayoutRole(screenShell.Toolbar, TransportLayoutRole.Toolbar);
        profileMetadata.SetLayoutRole(screenShell.SearchPanel, TransportLayoutRole.Search);
        profileMetadata.SetLayoutRole(filtersPanel, TransportLayoutRole.Filters);
        profileMetadata.SetLayoutRole(screenShell.Grid, TransportLayoutRole.Grid);
        profileMetadata.SetLayoutRole(screenShell.Pagination, TransportLayoutRole.Pagination);
        profileMetadata.SetLayoutRole(screenShell.AlertBar, TransportLayoutRole.Alerts);
        profileMetadata.SetGridProfile(screenShell.Grid, TransportGridProfile.Log);

        profileMetadata.SetFieldProfile(dtFrom, TransportFieldProfile.Input);
        profileMetadata.SetFieldProfile(dtTo, TransportFieldProfile.Input);
        profileMetadata.SetFieldProfile(txtUser, TransportFieldProfile.Input);
        profileMetadata.SetFieldProfile(cmbCompany, TransportFieldProfile.Lookup);
        profileMetadata.SetFieldProfile(cmbBranch, TransportFieldProfile.Lookup);
        profileMetadata.SetFieldProfile(cmbResult, TransportFieldProfile.Status);
        profileMetadata.SetFieldProfile(txtIp, TransportFieldProfile.Input);
        profileMetadata.SetFieldProfile(cmbLoginType, TransportFieldProfile.Lookup);
    }

    private static TransportDatePicker CreateOptionalDate() => new()
    {
        Format = DateTimePickerFormat.Short,
        ShowCheckBox = true,
        Checked = false
    };

    private static void AddLogColumn(DataGridView grid, string header, int? width)
    {
        var column = new DataGridViewTextBoxColumn
        {
            HeaderText = header,
            ReadOnly = true,
            SortMode = DataGridViewColumnSortMode.Automatic,
            AutoSizeMode = width.HasValue ? DataGridViewAutoSizeColumnMode.None : DataGridViewAutoSizeColumnMode.Fill
        };
        if (width.HasValue) column.Width = width.Value;
        grid.Columns.Add(column);
    }
}
