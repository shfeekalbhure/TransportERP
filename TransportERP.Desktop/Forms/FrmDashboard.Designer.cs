using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop
{
    public partial class FrmDashboard
    {
        private System.ComponentModel.IContainer? components = null;

        private TableLayoutPanel tblRoot = null!;
        private TableLayoutPanel tblWorkspace = null!;
        private TableLayoutPanel tblTopBar = null!;
        private TableLayoutPanel tblKpis = null!;
        private TableLayoutPanel tblAnalytics = null!;
        private TableLayoutPanel tblShortcuts = null!;
        private FlowLayoutPanel flpMenu = null!;
        private Panel pnlSidebar = null!;
        private Panel pnlTopBar = null!;
        private Panel pnlPageHeader = null!;
        private Panel pnlTransactions = null!;
        private Panel pnlRevenueChart = null!;
        private Panel pnlActivityChart = null!;
        private Label lblPageTitle = null!;
        private Label lblPageSubtitle = null!;
        private DataGridView dgvRecentTransactions = null!;
        private TransportStatusBar statusBar = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components is not null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tblRoot = new TableLayoutPanel();
            tblWorkspace = new TableLayoutPanel();
            tblTopBar = new TableLayoutPanel();
            tblKpis = new TableLayoutPanel();
            tblAnalytics = new TableLayoutPanel();
            tblShortcuts = new TableLayoutPanel();
            flpMenu = new FlowLayoutPanel();
            pnlSidebar = new Panel();
            pnlTopBar = new Panel();
            pnlPageHeader = new Panel();
            pnlTransactions = new Panel();
            pnlRevenueChart = new Panel();
            pnlActivityChart = new Panel();
            lblPageTitle = new Label();
            lblPageSubtitle = new Label();
            dgvRecentTransactions = new DataGridView();
            statusBar = new TransportStatusBar();

            SuspendLayout();

            tblRoot.ColumnCount = 2;
            tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 238F));
            tblRoot.Dock = DockStyle.Fill;
            tblRoot.RowCount = 2;
            tblRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tblRoot.BackColor = Color.FromArgb(247, 249, 252);
            tblRoot.RightToLeft = RightToLeft.No;

            ConfigureSidebar();
            ConfigureWorkspace();

            statusBar.Dock = DockStyle.Fill;
            statusBar.Margin = Padding.Empty;

            tblRoot.Controls.Add(tblWorkspace, 0, 0);
            tblRoot.Controls.Add(pnlSidebar, 1, 0);
            tblRoot.Controls.Add(statusBar, 0, 1);
            tblRoot.SetColumnSpan(statusBar, 2);

            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(247, 249, 252);
            ClientSize = new Size(1600, 960);
            Controls.Add(tblRoot);
            Font = UiTheme.CreateRegularFont(10F);
            MinimumSize = new Size(1366, 820);
            Name = "FrmDashboard";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TransportERP - الشاشة الرئيسية";
            WindowState = FormWindowState.Maximized;

            ResumeLayout(false);
        }

        private void ConfigureSidebar()
        {
            pnlSidebar.BackColor = Color.FromArgb(27, 58, 103);
            pnlSidebar.Dock = DockStyle.Fill;
            pnlSidebar.Padding = new Padding(12, 18, 12, 14);
            pnlSidebar.RightToLeft = RightToLeft.Yes;

            var sidebar = new TableLayoutPanel
            {
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                RowCount = 3,
                BackColor = Color.Transparent
            };
            sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
            sidebar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));

            var logo = new Label
            {
                Dock = DockStyle.Fill,
                Text = "🚚  TransportERP",
                ForeColor = Color.White,
                Font = UiTheme.CreateBoldFont(18F),
                TextAlign = ContentAlignment.MiddleCenter
            };

            flpMenu.Dock = DockStyle.Fill;
            flpMenu.FlowDirection = FlowDirection.TopDown;
            flpMenu.WrapContents = false;
            flpMenu.AutoScroll = true;
            flpMenu.Padding = new Padding(0, 8, 0, 8);
            flpMenu.BackColor = Color.Transparent;

            var menuItems = new[] { "⌂  الرئيسية" };

            for (var i = 0; i < menuItems.Length; i++)
            {
                var button = CreateSidebarButton(menuItems[i], i == 0);
                flpMenu.Controls.Add(button);
            }

            var logout = CreateSidebarButton("↪  تسجيل الخروج", false);
            logout.Click += btnLogout_Click;

            sidebar.Controls.Add(logo, 0, 0);
            sidebar.Controls.Add(flpMenu, 0, 1);
            sidebar.Controls.Add(logout, 0, 2);
            pnlSidebar.Controls.Add(sidebar);
        }

        private void ConfigureWorkspace()
        {
            tblWorkspace.ColumnCount = 1;
            tblWorkspace.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblWorkspace.Dock = DockStyle.Fill;
            tblWorkspace.Padding = new Padding(22, 14, 22, 10);
            tblWorkspace.RowCount = 6;
            tblWorkspace.RowStyles.Add(new RowStyle(SizeType.Absolute, 84F));
            tblWorkspace.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
            tblWorkspace.RowStyles.Add(new RowStyle(SizeType.Absolute, 164F));
            tblWorkspace.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblWorkspace.RowStyles.Add(new RowStyle(SizeType.Absolute, 128F));
            tblWorkspace.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
            tblWorkspace.BackColor = Color.FromArgb(247, 249, 252);
            tblWorkspace.RightToLeft = RightToLeft.Yes;

            ConfigureTopBar();
            ConfigurePageHeader();
            ConfigureKpis();
            ConfigureAnalytics();
            ConfigureShortcuts();

            tblWorkspace.Controls.Add(pnlTopBar, 0, 0);
            tblWorkspace.Controls.Add(pnlPageHeader, 0, 1);
            tblWorkspace.Controls.Add(tblKpis, 0, 2);
            tblWorkspace.Controls.Add(tblAnalytics, 0, 3);
            tblWorkspace.Controls.Add(tblShortcuts, 0, 4);
        }

        private void ConfigureTopBar()
        {
            pnlTopBar.BackColor = Color.White;
            pnlTopBar.Dock = DockStyle.Fill;
            pnlTopBar.Margin = new Padding(0, 0, 0, 10);
            pnlTopBar.Padding = new Padding(18, 8, 18, 8);

            tblTopBar.ColumnCount = 4;
            tblTopBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27F));
            tblTopBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblTopBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23F));
            tblTopBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblTopBar.Dock = DockStyle.Fill;
            tblTopBar.RowCount = 1;

            var brand = new Label
            {
                Dock = DockStyle.Fill,
                Text = "🚚  TransportERP",
                ForeColor = Color.FromArgb(24, 65, 112),
                Font = UiTheme.CreateBoldFont(17F),
                TextAlign = ContentAlignment.MiddleRight
            };

            var user = new Label
            {
                Dock = DockStyle.Fill,
                Text = "●  مرحبًا بك في النظام\r\n    أحمد محمد  |  مدير النظام",
                ForeColor = Color.FromArgb(44, 53, 67),
                Font = UiTheme.CreateRegularFont(10F),
                TextAlign = ContentAlignment.MiddleRight
            };

            var date = new Label
            {
                Dock = DockStyle.Fill,
                Text = "▣  السبت\r\n03 مايو 2026 م\r\n06 ذو القعدة 1447 هـ",
                ForeColor = Color.FromArgb(70, 78, 93),
                Font = UiTheme.CreateRegularFont(9F),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var tools = new Label
            {
                Dock = DockStyle.Fill,
                Text = "🔔     ✉     ◐     ⛶",
                ForeColor = Color.FromArgb(24, 65, 112),
                Font = UiTheme.CreateRegularFont(15F),
                TextAlign = ContentAlignment.MiddleLeft
            };

            tblTopBar.Controls.Add(brand, 0, 0);
            tblTopBar.Controls.Add(user, 1, 0);
            tblTopBar.Controls.Add(date, 2, 0);
            tblTopBar.Controls.Add(tools, 3, 0);
            pnlTopBar.Controls.Add(tblTopBar);
        }

        private void ConfigurePageHeader()
        {
            pnlPageHeader.BackColor = Color.Transparent;
            pnlPageHeader.Dock = DockStyle.Fill;
            pnlPageHeader.Padding = new Padding(2, 4, 2, 4);

            lblPageTitle.Dock = DockStyle.Top;
            lblPageTitle.Height = 38;
            lblPageTitle.Text = "الرئيسية";
            lblPageTitle.ForeColor = Color.FromArgb(33, 45, 65);
            lblPageTitle.Font = UiTheme.CreateBoldFont(20F);
            lblPageTitle.TextAlign = ContentAlignment.MiddleRight;

            lblPageSubtitle.Dock = DockStyle.Top;
            lblPageSubtitle.Height = 28;
            lblPageSubtitle.Text = "نظرة عامة على نشاط النظام اليوم";
            lblPageSubtitle.ForeColor = Color.FromArgb(105, 116, 133);
            lblPageSubtitle.Font = UiTheme.CreateRegularFont(10F);
            lblPageSubtitle.TextAlign = ContentAlignment.MiddleRight;

            var customize = new Button
            {
                Dock = DockStyle.Left,
                Width = 160,
                Text = "⚙  تخصيص الشاشة",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(45, 58, 75),
                Font = UiTheme.CreateRegularFont(9.5F)
            };
            customize.FlatAppearance.BorderColor = Color.FromArgb(224, 229, 236);

            pnlPageHeader.Controls.Add(customize);
            pnlPageHeader.Controls.Add(lblPageSubtitle);
            pnlPageHeader.Controls.Add(lblPageTitle);
        }

        private void ConfigureKpis()
        {
            tblKpis.ColumnCount = 5;
            for (var i = 0; i < 5; i++)
            {
                tblKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            }
            tblKpis.Dock = DockStyle.Fill;
            tblKpis.RowCount = 1;
            tblKpis.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblKpis.Margin = Padding.Empty;

            tblKpis.Controls.Add(CreateKpiCard("إجمالي الإيرادات", "1,250,000", "ريال يمني", "+12.5% ↑", "💼", Color.FromArgb(47, 128, 237)), 0, 0);
            tblKpis.Controls.Add(CreateKpiCard("إجمالي المصروفات", "860,500", "ريال يمني", "+8.3% ↑", "💵", Color.FromArgb(111, 207, 151)), 1, 0);
            tblKpis.Controls.Add(CreateKpiCard("إجمالي المعاملات", "342", "معاملة", "+15.7% ↑", "▤", Color.FromArgb(155, 81, 224)), 2, 0);
            tblKpis.Controls.Add(CreateKpiCard("العملاء النشطون", "125", "عميل", "+9.6% ↑", "👥", Color.FromArgb(242, 153, 74)), 3, 0);
            tblKpis.Controls.Add(CreateKpiCard("البوالص النشطة", "78", "بوليصة", "+6.8% ↑", "🧳", Color.FromArgb(86, 204, 200)), 4, 0);
        }

        private void ConfigureAnalytics()
        {
            tblAnalytics.ColumnCount = 3;
            tblAnalytics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            tblAnalytics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
            tblAnalytics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tblAnalytics.Dock = DockStyle.Fill;
            tblAnalytics.RowCount = 1;
            tblAnalytics.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblAnalytics.Margin = new Padding(0, 10, 0, 10);

            ConfigureTransactions();
            ConfigureChartPanel(pnlRevenueChart, "الإيرادات والمصروفات - آخر 6 أشهر");
            ConfigureChartPanel(pnlActivityChart, "توزيع الإيرادات حسب النشاط");
            pnlRevenueChart.Paint += pnlRevenueChart_Paint;
            pnlActivityChart.Paint += pnlActivityChart_Paint;

            tblAnalytics.Controls.Add(pnlTransactions, 0, 0);
            tblAnalytics.Controls.Add(pnlRevenueChart, 1, 0);
            tblAnalytics.Controls.Add(pnlActivityChart, 2, 0);
        }

        private void ConfigureTransactions()
        {
            pnlTransactions.BackColor = Color.White;
            pnlTransactions.Dock = DockStyle.Fill;
            pnlTransactions.Margin = new Padding(0, 0, 10, 0);
            pnlTransactions.Padding = new Padding(14);

            var title = new Label
            {
                Dock = DockStyle.Top,
                Height = 38,
                Text = "☷  آخر المعاملات",
                ForeColor = Color.FromArgb(33, 45, 65),
                Font = UiTheme.CreateBoldFont(13F),
                TextAlign = ContentAlignment.MiddleRight
            };

            dgvRecentTransactions.Dock = DockStyle.Fill;
            dgvRecentTransactions.AllowUserToAddRows = false;
            dgvRecentTransactions.AllowUserToDeleteRows = false;
            dgvRecentTransactions.AllowUserToResizeRows = false;
            dgvRecentTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRecentTransactions.BackgroundColor = Color.White;
            dgvRecentTransactions.BorderStyle = BorderStyle.None;
            dgvRecentTransactions.ColumnHeadersVisible = false;
            dgvRecentTransactions.RowHeadersVisible = false;
            dgvRecentTransactions.ReadOnly = true;
            dgvRecentTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecentTransactions.RowTemplate.Height = 42;
            dgvRecentTransactions.RightToLeft = RightToLeft.Yes;
            dgvRecentTransactions.DefaultCellStyle.Font = UiTheme.CreateRegularFont(9.5F);
            dgvRecentTransactions.DefaultCellStyle.SelectionBackColor = Color.FromArgb(240, 245, 252);
            dgvRecentTransactions.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 45, 65);
            dgvRecentTransactions.Columns.Add("Transaction", "المعاملة");
            dgvRecentTransactions.Columns.Add("Amount", "المبلغ");
            dgvRecentTransactions.Columns.Add("Status", "الحالة");

            var footer = new Button
            {
                Dock = DockStyle.Bottom,
                Height = 42,
                Text = "عرض جميع المعاملات",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(45, 58, 75),
                Font = UiTheme.CreateRegularFont(9.5F)
            };
            footer.FlatAppearance.BorderColor = Color.FromArgb(224, 229, 236);

            pnlTransactions.Controls.Add(dgvRecentTransactions);
            pnlTransactions.Controls.Add(footer);
            pnlTransactions.Controls.Add(title);
        }

        private void ConfigureShortcuts()
        {
            tblShortcuts.ColumnCount = 10;
            for (var i = 0; i < 10; i++)
            {
                tblShortcuts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            }
            tblShortcuts.Dock = DockStyle.Fill;
            tblShortcuts.RowCount = 1;
            tblShortcuts.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblShortcuts.Margin = new Padding(0, 4, 0, 0);

            var items = new[]
            {
                ("🚚", "المركبات"), ("📦", "المخزون"), ("📗", "الحسابات"), ("👔", "الموردون"),
                ("👥", "العملاء"), ("📊", "التقارير"), ("🚛", "البوالص"), ("▤", "قيد يومي"),
                ("⬆", "سند صرف"), ("⬇", "سند قبض")
            };

            for (var i = 0; i < items.Length; i++)
            {
                tblShortcuts.Controls.Add(CreateShortcut(items[i].Item1, items[i].Item2), i, 0);
            }
        }

        private static Button CreateSidebarButton(string text, bool selected)
        {
            var button = new Button
            {
                Width = 204,
                Height = 44,
                Margin = new Padding(0, 3, 0, 3),
                Text = text,
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(8, 0, 8, 0),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = selected ? Color.FromArgb(35, 112, 218) : Color.Transparent,
                Font = UiTheme.CreateRegularFont(10F),
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(42, 83, 132);
            return button;
        }

        private static Panel CreateKpiCard(string title, string value, string unit, string growth, string icon, Color accent)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(7),
                Padding = new Padding(14)
            };

            var iconLabel = new Label
            {
                Dock = DockStyle.Right,
                Width = 62,
                Text = icon,
                BackColor = Color.FromArgb(238, 245, 255),
                ForeColor = accent,
                Font = new Font("Segoe UI Emoji", 20F),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var growthLabel = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 28,
                Text = $"عن الشهر الماضي   {growth}",
                ForeColor = Color.FromArgb(39, 174, 96),
                Font = UiTheme.CreateRegularFont(8.5F),
                TextAlign = ContentAlignment.MiddleRight
            };

            var unitLabel = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 24,
                Text = unit,
                ForeColor = Color.FromArgb(100, 110, 125),
                Font = UiTheme.CreateRegularFont(9F),
                TextAlign = ContentAlignment.MiddleRight
            };

            var valueLabel = new Label
            {
                Dock = DockStyle.Fill,
                Text = value,
                ForeColor = Color.FromArgb(31, 39, 51),
                Font = UiTheme.CreateBoldFont(20F),
                TextAlign = ContentAlignment.MiddleRight
            };

            var titleLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                Text = title,
                ForeColor = accent,
                Font = UiTheme.CreateBoldFont(9.5F),
                TextAlign = ContentAlignment.MiddleRight
            };

            card.Controls.Add(valueLabel);
            card.Controls.Add(unitLabel);
            card.Controls.Add(growthLabel);
            card.Controls.Add(titleLabel);
            card.Controls.Add(iconLabel);
            return card;
        }

        private static void ConfigureChartPanel(Panel panel, string title)
        {
            panel.BackColor = Color.White;
            panel.Dock = DockStyle.Fill;
            panel.Margin = new Padding(10, 0, 0, 0);
            panel.Padding = new Padding(14, 42, 14, 14);

            var titleLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 38,
                Text = title,
                ForeColor = Color.FromArgb(33, 45, 65),
                Font = UiTheme.CreateBoldFont(12.5F),
                TextAlign = ContentAlignment.MiddleRight
            };
            panel.Controls.Add(titleLabel);
        }

        private Button CreateShortcut(string icon, string text)
        {
            var button = new Button
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(6),
                Text = $"{icon}\r\n{text}",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(40, 52, 70),
                Font = UiTheme.CreateRegularFont(9.5F),
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderColor = Color.FromArgb(226, 231, 238);
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(244, 248, 253);
            button.Click += QuickAction_Click;
            return button;
        }

        #endregion
    }
}
