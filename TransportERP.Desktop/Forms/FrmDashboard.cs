using System.Drawing.Drawing2D;
using TransportERP.Desktop.Views.Setup.Geographic;

namespace TransportERP.Desktop;

/// <summary>
/// النافذة الرئيسية لنظام TransportERP.
/// تستضيف شاشات العمل كعناصر UserControl داخل تبويبات مساحة العمل.
/// </summary>
public partial class FrmDashboard : Form
{
    private const string DashboardTabKey = "DASHBOARD";
    private const string CountriesTabKey = "GEN-003";
    private const string GovernoratesTabKey = "GEN-004";
    private const string DirectoratesTabKey = "GEN-005";
    private const string CitiesTabKey = "GEN-006";

    private TabControl? _workspaceTabs;
    private ContextMenuStrip? _generalSetupMenu;

    public FrmDashboard()
    {
        InitializeComponent();
        ConfigureTabbedWorkspace();
        ConfigureGeneralSetupMenu();
        LoadDevelopmentPreviewData();
    }

    private void ConfigureTabbedWorkspace()
    {
        if (_workspaceTabs is not null) return;

        tblRoot.SuspendLayout();
        tblRoot.Controls.Remove(tblWorkspace);

        _workspaceTabs = new TabControl
        {
            Name = "tabWorkspace",
            Dock = DockStyle.Fill,
            RightToLeft = RightToLeft.Yes,
            RightToLeftLayout = true,
            Font = new Font(Font.FontFamily, 10F, FontStyle.Bold),
            Padding = new Point(18, 7),
            HotTrack = true
        };
        _workspaceTabs.MouseDoubleClick += WorkspaceTabs_MouseDoubleClick;

        var dashboardPage = new TabPage
        {
            Name = DashboardTabKey,
            Text = "الرئيسية",
            BackColor = Color.FromArgb(247, 249, 252),
            RightToLeft = RightToLeft.Yes,
            Padding = Padding.Empty
        };

        tblWorkspace.Dock = DockStyle.Fill;
        dashboardPage.Controls.Add(tblWorkspace);
        _workspaceTabs.TabPages.Add(dashboardPage);
        tblRoot.Controls.Add(_workspaceTabs, 0, 0);
        tblRoot.ResumeLayout(true);
    }

    private void ConfigureGeneralSetupMenu()
    {
        var generalSetupButton = FindButtonByText(this, "التهيئة العامة");
        if (generalSetupButton is null) return;

        _generalSetupMenu?.Dispose();
        _generalSetupMenu = new ContextMenuStrip
        {
            RightToLeft = RightToLeft.Yes,
            Font = new Font(Font.FontFamily, 10F),
            ShowImageMargin = false,
            AutoSize = true
        };

        var geographicDataItem = new ToolStripMenuItem("البيانات الجغرافية")
        {
            Name = "mnuGeographicData",
            RightToLeft = RightToLeft.Yes
        };

        var countriesItem = new ToolStripMenuItem("الدول")
        {
            Name = "mnuCountries",
            ToolTipText = "GEN-003 — الدول",
            RightToLeft = RightToLeft.Yes
        };
        countriesItem.Click += (_, _) => OpenCountriesView();

        var governoratesItem = new ToolStripMenuItem("المحافظات")
        {
            Name = "mnuGovernorates",
            ToolTipText = "GEN-004 — المحافظات",
            RightToLeft = RightToLeft.Yes
        };
        governoratesItem.Click += (_, _) => OpenGovernoratesView();

        var directoratesItem = new ToolStripMenuItem("المديريات")
        {
            Name = "mnuDirectorates",
            ToolTipText = "GEN-005 — المديريات",
            RightToLeft = RightToLeft.Yes
        };
        directoratesItem.Click += (_, _) => OpenDirectoratesView();

        var citiesItem = new ToolStripMenuItem("المدن")
        {
            Name = "mnuCities",
            ToolTipText = "GEN-006 — المدن",
            RightToLeft = RightToLeft.Yes
        };
        citiesItem.Click += (_, _) => OpenCitiesView();

        geographicDataItem.DropDownItems.Add(countriesItem);
        geographicDataItem.DropDownItems.Add(governoratesItem);
        geographicDataItem.DropDownItems.Add(directoratesItem);
        geographicDataItem.DropDownItems.Add(citiesItem);
        _generalSetupMenu.Items.Add(geographicDataItem);

        generalSetupButton.Click -= GeneralSetupButton_Click;
        generalSetupButton.Click += GeneralSetupButton_Click;
    }

    private void GeneralSetupButton_Click(object? sender, EventArgs e)
    {
        if (sender is not Button button || _generalSetupMenu is null) return;
        _generalSetupMenu.Show(button, new Point(0, button.Height));
    }

    private void OpenCountriesView() =>
        OpenWorkspaceView(CountriesTabKey, "الدول", new UcCountries());

    private void OpenGovernoratesView() =>
        OpenWorkspaceView(GovernoratesTabKey, "المحافظات", new UcGovernorates());

    private void OpenDirectoratesView() =>
        OpenWorkspaceView(DirectoratesTabKey, "المديريات", new UcDirectorates());

    private void OpenCitiesView() =>
        OpenWorkspaceView(CitiesTabKey, "المدن", new UcCities());

    public void OpenWorkspaceView(string screenKey, string title, UserControl view)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(screenKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentNullException.ThrowIfNull(view);

        if (_workspaceTabs is null)
        {
            view.Dispose();
            return;
        }

        var existingPage = _workspaceTabs.TabPages.Cast<TabPage>()
            .FirstOrDefault(page => string.Equals(page.Name, screenKey, StringComparison.Ordinal));

        if (existingPage is not null)
        {
            view.Dispose();
            _workspaceTabs.SelectedTab = existingPage;
            existingPage.Focus();
            return;
        }

        view.Dock = DockStyle.Fill;
        view.RightToLeft = RightToLeft.Yes;

        var page = new TabPage
        {
            Name = screenKey,
            Text = $"{title}  ×",
            BackColor = Color.FromArgb(247, 249, 252),
            RightToLeft = RightToLeft.Yes,
            Padding = Padding.Empty,
            Tag = view
        };

        page.Controls.Add(view);
        _workspaceTabs.TabPages.Add(page);
        _workspaceTabs.SelectedTab = page;
        view.Focus();
    }

    private void WorkspaceTabs_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        if (_workspaceTabs is null) return;

        for (var index = 0; index < _workspaceTabs.TabPages.Count; index++)
        {
            if (!_workspaceTabs.GetTabRect(index).Contains(e.Location)) continue;

            var page = _workspaceTabs.TabPages[index];
            if (string.Equals(page.Name, DashboardTabKey, StringComparison.Ordinal)) return;

            CloseWorkspacePage(page);
            return;
        }
    }

    private void CloseWorkspacePage(TabPage page)
    {
        if (page.Tag is UserControl view && !view.IsDisposed) view.Dispose();
        _workspaceTabs?.TabPages.Remove(page);
        page.Dispose();
    }

    private static Button? FindButtonByText(Control root, string text)
    {
        foreach (Control control in root.Controls)
        {
            if (control is Button button && button.Text.Contains(text, StringComparison.Ordinal)) return button;
            var nested = FindButtonByText(control, text);
            if (nested is not null) return nested;
        }
        return null;
    }

    private void LoadDevelopmentPreviewData()
    {
        dgvRecentTransactions.Rows.Clear();
        dgvRecentTransactions.Rows.Add("لا توجد معاملات بعد", "—", "—");

        statusBar.UpdateContext(
            "الشركة الحالية",
            "الفرع الحالي",
            DateTime.Today.Year.ToString(),
            "الفترة الحالية",
            "المستخدم الحالي",
            "الدور الحالي",
            "TransportERP",
            "1.0.0",
            false);
    }

    private void btnLogout_Click(object? sender, EventArgs e)
    {
        var answer = MessageBox.Show(
            "هل تريد تسجيل الخروج؟",
            "تسجيل الخروج",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2,
            MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);

        if (answer == DialogResult.Yes) Close();
    }

    private void QuickAction_Click(object? sender, EventArgs e)
    {
        if (sender is not Button button) return;

        MessageBox.Show(
            $"الاختصار «{button.Text.Replace("\r\n", " ", StringComparison.Ordinal)}» غير مفعل في النطاق الحالي.",
            "TransportERP",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information,
            MessageBoxDefaultButton.Button1,
            MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
    }

    private void pnlRevenueChart_Paint(object? sender, PaintEventArgs e) =>
        DrawPreviewLineChart(e.Graphics, pnlRevenueChart.ClientRectangle);

    private void pnlActivityChart_Paint(object? sender, PaintEventArgs e) =>
        DrawPreviewActivityChart(e.Graphics, pnlActivityChart.ClientRectangle);

    private static void DrawPreviewLineChart(Graphics graphics, Rectangle bounds)
    {
        if (bounds.Width < 120 || bounds.Height < 100) return;
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var chart = Rectangle.Inflate(bounds, -28, -58);
        if (chart.Width <= 0 || chart.Height <= 0) return;

        using var axisPen = new Pen(Color.FromArgb(220, 225, 232), 1F);
        graphics.DrawLine(axisPen, chart.Left, chart.Bottom, chart.Right, chart.Bottom);
        graphics.DrawLine(axisPen, chart.Left, chart.Top, chart.Left, chart.Bottom);

        var points = new[]
        {
            new PointF(chart.Left, chart.Bottom - chart.Height * 0.25F),
            new PointF(chart.Left + chart.Width * 0.2F, chart.Bottom - chart.Height * 0.45F),
            new PointF(chart.Left + chart.Width * 0.4F, chart.Bottom - chart.Height * 0.36F),
            new PointF(chart.Left + chart.Width * 0.6F, chart.Bottom - chart.Height * 0.62F),
            new PointF(chart.Left + chart.Width * 0.8F, chart.Bottom - chart.Height * 0.55F),
            new PointF(chart.Right, chart.Bottom - chart.Height * 0.72F)
        };

        using var linePen = new Pen(Color.FromArgb(47, 128, 237), 2.5F);
        graphics.DrawLines(linePen, points);
    }

    private static void DrawPreviewActivityChart(Graphics graphics, Rectangle bounds)
    {
        if (bounds.Width < 120 || bounds.Height < 100) return;
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var area = Rectangle.Inflate(bounds, -34, -62);
        if (area.Width <= 0 || area.Height <= 0) return;

        var values = new[] { 0.72F, 0.54F, 0.38F, 0.63F };
        var gap = 12;
        var barWidth = Math.Max(14, (area.Width - gap * (values.Length - 1)) / values.Length);
        using var brush = new SolidBrush(Color.FromArgb(86, 204, 200));

        for (var i = 0; i < values.Length; i++)
        {
            var height = (int)(area.Height * values[i]);
            var x = area.Left + i * (barWidth + gap);
            graphics.FillRectangle(brush, new Rectangle(x, area.Bottom - height, barWidth, height));
        }
    }
}
