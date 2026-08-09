using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.CoreUI.Architecture;

public sealed class Gen003CountriesReferenceScreen : CoreUiReferenceScreen
{
    public Gen003CountriesReferenceScreen() : base(TransportScreenProfile.MasterData, "الدول")
    {
        ReferenceScreenLayout.ConfigureMasterData(Shell, ["Code", "ArabicName", "EnglishName", "Status", "Notes", "ISO2", "ISO3", "DialingCode"], ["Code", "ArabicName", "EnglishName", "ISO2", "ISO3", "DialingCode", "Status"]);
    }
}
public sealed class Acc035ChartOfAccountsReferenceScreen : CoreUiReferenceScreen
{
    public Acc035ChartOfAccountsReferenceScreen() : base(TransportScreenProfile.TreeMaster, "دليل الحسابات") => ReferenceScreenLayout.ConfigureTree(Shell);
}
public sealed class Acc041AccountingPeriodsReferenceScreen : CoreUiReferenceScreen
{
    public Acc041AccountingPeriodsReferenceScreen() : base(TransportScreenProfile.ControlApproval, "الفترات المحاسبية") => ReferenceScreenLayout.ConfigurePeriodLifecycle(Shell);
}
public sealed class Acc042JournalEntryReferenceScreen : CoreUiReferenceScreen
{
    public Acc042JournalEntryReferenceScreen() : base(TransportScreenProfile.Transaction, "القيد اليومي") => ReferenceScreenLayout.ConfigureTransaction(Shell);
}
public sealed class Acc046TrialBalanceReferenceScreen : CoreUiReferenceScreen
{
    public Acc046TrialBalanceReferenceScreen() : base(TransportScreenProfile.ReportInquiry, "ميزان المراجعة") => ReferenceScreenLayout.ConfigureReport(Shell);
}
public sealed class Gen015OperationalSettingsReferenceScreen : CoreUiReferenceScreen
{
    public Gen015OperationalSettingsReferenceScreen() : base(TransportScreenProfile.Settings, "إعدادات التشغيل العامة") => ReferenceScreenLayout.ConfigureScopedSettings(Shell);
}

internal static class ReferenceScreenLayout
{
    internal static void ConfigureMasterData(TransportReferenceScreenShell shell, IEnumerable<string> fields, IEnumerable<string> columns)
    {
        shell.DataHost.Controls.Add(CreateFields(fields));
        ConfigureGrid(shell.Grid, columns, false);
    }
    internal static void ConfigureTree(TransportReferenceScreenShell shell)
    {
        shell.ConfigureWorkspaceMode(false, false, true);
        var split = new SplitContainer { Dock = DockStyle.Fill, RightToLeft = RightToLeft.Yes, FixedPanel = FixedPanel.Panel1, SplitterDistance = 300, AccessibleName = "SplitHost" };
        var tree = new TreeView { Dock = DockStyle.Fill, RightToLeft = RightToLeft.Yes, RightToLeftLayout = true, AccessibleName = "TreeHost" };
        tree.Nodes.Add("الأصول").Nodes.Add("الأصول المتداولة"); tree.Nodes.Add("الالتزامات"); tree.Nodes.Add("الإيرادات"); tree.Nodes.Add("المصروفات"); tree.ExpandAll();
        split.Panel1.Controls.Add(tree); split.Panel2.Controls.Add(CreateFields(["رمز الحساب", "اسم الحساب", "نوع الحساب", "الحساب الأب"])); split.Panel2.AccessibleName = "DetailsHost";
        shell.DataHost.Controls.Add(split);
    }
    internal static void ConfigurePeriodLifecycle(TransportReferenceScreenShell shell)
    {
        shell.DataHost.Controls.Add(CreateFields(["السنة المالية", "اسم الفترة", "من تاريخ", "إلى تاريخ", "حالة الفترة"]));
        shell.DataHost.Controls.Add(CreateActions(["مفتوحة", "إقفال الفترة", "إعادة فتح", "عرض السجل"]));
        ConfigureGrid(shell.Grid, ["التاريخ", "الإجراء", "الحالة السابقة", "الحالة الجديدة", "المنفذ", "السبب"], true);
    }
    internal static void ConfigureTransaction(TransportReferenceScreenShell shell)
    {
        shell.DataHost.Controls.Add(CreateFields(["رقم القيد", "التاريخ", "العملة", "الوصف", "الحالة: مرحّل"], true));
        shell.DataHost.Controls.Add(CreateActions(["إجمالي المدين: 0.00", "إجمالي الدائن: 0.00", "الفرق: 0.00"]));
        ConfigureGrid(shell.Grid, ["الحساب", "البيان", "مركز التكلفة", "مدين", "دائن"], true);
    }
    internal static void ConfigureReport(TransportReferenceScreenShell shell)
    {
        shell.DataHost.Controls.Add(CreateFields(["من تاريخ", "إلى تاريخ", "الفرع", "مستوى الحساب"]));
        shell.DataHost.Controls.Add(CreateActions(["إجمالي مدين: من الخادم", "إجمالي دائن: من الخادم", "صافي الرصيد: من الخادم"]));
        ConfigureGrid(shell.Grid, ["الحساب", "مدين", "دائن", "الرصيد"], true);
        shell.Pagination.SetPageInfo(1, 1, 0, 0, 0);
    }
    internal static void ConfigureScopedSettings(TransportReferenceScreenShell shell)
    {
        shell.ConfigureWorkspaceMode(false, false, true);
        var host = new SplitContainer { Dock = DockStyle.Fill, RightToLeft = RightToLeft.Yes, FixedPanel = FixedPanel.Panel1, SplitterDistance = 350, AccessibleName = "SettingsHost" };
        host.Panel1.Controls.Add(new Label { Dock = DockStyle.Fill, Text = "نطاق التحرير\r\nالمستخدم ← الفرع ← الشركة ← النظام ← الافتراضي", TextAlign = ContentAlignment.TopRight, Padding = new Padding(16) });
        var grid = new TransportDataGrid { Dock = DockStyle.Fill, ReadOnly = true, AccessibleName = "NearestOverride" }; ConfigureGrid(grid, ["المفتاح", "القيمة الفعالة", "المصدر", "الأولوية"], true); host.Panel2.Controls.Add(grid);
        shell.DataHost.Controls.Add(host);
    }
    private static TableLayoutPanel CreateFields(IEnumerable<string> fields, bool readOnly = false)
    {
        var table = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 4, RightToLeft = RightToLeft.Yes, Padding = new Padding(8) };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        foreach (var (field, index) in fields.Select((f, i) => (f, i))) { var row = index / 2; table.RowStyles.Add(new RowStyle(SizeType.Absolute, CoreUIProperties.FieldRowHeight)); table.Controls.Add(new Label { Text = field, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight }, (index % 2) * 2, row); table.Controls.Add(new TextBox { Dock = DockStyle.Fill, ReadOnly = readOnly, RightToLeft = RightToLeft.Yes }, (index % 2) * 2 + 1, row); }
        return table;
    }
    private static FlowLayoutPanel CreateActions(IEnumerable<string> labels) { var panel = new FlowLayoutPanel { Dock = DockStyle.Bottom, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, RightToLeft = RightToLeft.Yes }; foreach (var label in labels) panel.Controls.Add(new Label { Text = label, AutoSize = true, Padding = new Padding(8) }); return panel; }
    private static void ConfigureGrid(TransportDataGrid grid, IEnumerable<string> columns, bool readOnly) { grid.AutoGenerateColumns = false; grid.Columns.Clear(); grid.ReadOnly = readOnly; foreach (var column in columns) grid.Columns.Add(new DataGridViewTextBoxColumn { Name = column, HeaderText = column, ReadOnly = readOnly, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill }); }
}
