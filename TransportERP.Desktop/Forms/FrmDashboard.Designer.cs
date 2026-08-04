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
        private TableLayoutPanel tblPageHeader = null!;
        private TableLayoutPanel tblKpis = null!;
        private TableLayoutPanel tblAnalytics = null!;
        private TableLayoutPanel tblShortcuts = null!;
        private Panel pnlSidebar = null!;
        private Panel pnlTopBar = null!;
        private Panel pnlPageHeader = null!;
        private Panel pnlTransactions = null!;
        private Panel pnlRevenueCard = null!;
        private Panel pnlActivityCard = null!;
        private Panel pnlRevenueChart = null!;
        private Panel pnlActivityChart = null!;
        private DataGridView dgvRecentTransactions = null!;
        private Label lblPageTitle = null!;
        private Label lblPageSubtitle = null!;
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
            tblPageHeader = new TableLayoutPanel();
            tblKpis = new TableLayoutPanel();
            tblAnalytics = new TableLayoutPanel();
            tblShortcuts = new TableLayoutPanel();
            pnlSidebar = new Panel();
            pnlTopBar = new Panel();
            pnlPageHeader = new Panel();
            pnlTransactions = new Panel();
            pnlRevenueCard = new Panel();
            pnlActivityCard = new Panel();
            pnlRevenueChart = new Panel();
            pnlActivityChart = new Panel();
            dgvRecentTransactions = new DataGridView();
            lblPageTitle = new Label();
            lblPageSubtitle = new Label();
            statusBar = new TransportStatusBar();

            SuspendLayout();

            tblRoot.ColumnCount = 2;
            tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 238F));
            tblRoot.Dock = DockStyle.Fill;
            tblRoot.RowCount = 2;
            tblRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblRoot.BackColor = Color.FromArgb(247, 249, 252);
            tblRoot.RightToLeft = RightToLeft.No;

            ConfigureSidebar();
            ConfigureWorkspace();
            ConfigureStatusBar();

            tblRoot.Controls.Add(tblWorkspace, 0, 0);
            tblRoot.Controls.Add(pnlSidebar, 1, 0);
            tblRoot.Controls.Add(statusBar, 0, 1);
            tblRoot.SetColumnSpan(statusBar, 2);

            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(247, 249, 252);
            ClientSize = new Size(1600, 1000);
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
            pnlSidebar.BackColor = Color.FromArgb(21, 57, 99);
            pnlSidebar.Dock = DockStyle.Fill;
            pnlSidebar.Padding = new Padding(12, 18, 12, 18);
            pnlSidebar.RightToLeft = RightToLeft.Yes;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 15,
                BackColor = Color.Transparent,
                Margin = Padding.Empty
            };

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 84F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            for (var i = 0; i < 12; i++)
            {
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            }
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var brand = new Label
            {
                Dock = DockStyle.Fill,
                Text = "🚚  TransportERP",
                ForeColor = Color.White,
                Font = UiTheme.CreateBoldFont(17F),
                TextAlign = ContentAlignment.MiddleCenter
            };
            layout.Controls.Add(brand, 0, 0);

            var menus = new[]
            {
                "⌂  الرئيسية",
                "◈  الإدارة والأمن",
                "⚙  التهيئة العامة",
                "▣  المحاسبة",
                "▤  السقوف والميزانيات",
                "🛒  المبيعات",
                "▱  المشتريات",
                "▦  المخزون",
                "🚚  النقل والشحن",
                "▧  التذاكر والسفريات",
                "▥  المركبات والصيانة",
                "▤  التقارير والتحليلات",
                "⚙  الإعدادات"
            };

            for (var i = 0; i < menus.Length; i++)
            {
                var button = new Button();
                ConfigureMenuButton(button, menus[i], i == 0);
                layout.Controls.Add(button, 0, i + 2);
            }

            pnlSidebar.Controls.Add(layout);
        }

        private void ConfigureWorkspace()
        {
            tblWorkspace.ColumnCount = 1;
            tblWorkspace.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblWorkspace.Dock = DockStyle.Fill;
            tblWorkspace.RowCount = 6;
            tblWorkspace.RowStyles.Add(new RowStyle(SizeType.Absolute, 96F));
            tblWorkspace.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
            tblWorkspace.RowStyles.Add(new RowStyle(SizeType.Absolute, 176F));
            tblWorkspace.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblWorkspace.RowStyles.Add(new RowStyle(SizeType.Absolute, 132F));
            tblWorkspace.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
            tblWorkspace.BackColor = Color.FromArgb(247, 249, 252);
            tblWorkspace.Padding = new Padding(18, 0, 18, 0);
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
            pnlTopBar.Dock = DockStyle.Fill;
            pnlTopBar.BackColor = Color.White;
            pnlTopBar.Margin = new Padding(-18, 0, -18, 10);
            pnlTopBar.Padding = new Padding(24, 12, 24, 12);

            tblTopBar.ColumnCount = 4;
            tblTopBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tblTopBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblTopBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            tblTopBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tblTopBar.Dock = DockStyle.Fill;
            tblTopBar.RowCount = 1;
            tblTopBar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblTopBar.RightToLeft = RightToLeft.Yes;

            var systemBrand = new Label
            {
                Dock = DockStyle.Fill,
                Text = "🚚  TransportERP",
                ForeColor = Color.FromArgb(21, 57, 99),
                Font = UiTheme.CreateBoldFont(18F),
                TextAlign = ContentAlignment.MiddleRight
            };

            var user = new Label
            {
                Dock = DockStyle.Fill,
                Text = "●   مرحبًا بك في النظام\r\n     أحمد محمد\r\n     مدير النظام",
                ForeColor = Color.FromArgb(37, 48, 67),
                Font = UiTheme.CreateRegularFont(10F),
                TextAlign = ContentAlignment.MiddleRight
            };

            var date = new Label
            {
                Dock = DockStyle.Fill,
                Text = "📅  السبت\r\n03 مايو 2026 م\r\n06 ذو القعدة 1447 هـ",
                ForeColor = Color.FromArgb(65, 78, 99),
                Font = UiTheme.CreateRegularFont(9.5F),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var tools = new Label
            {
                Dock = DockStyle.Fill,
                Text = "🔔     ✉     ◐     ⛶",
                ForeColor = Color.FromArgb(21, 57, 99),
                Font = UiTheme.CreateRegularFont(15F),
                TextAlign = ContentAlignment.MiddleLeft
            };

            tblTopBar.Controls.Add(systemBrand, 0, 0);
            tblTopBar.Controls.Add(user, 1, 0);
            tblTopBar.Controls.Add(date, 2, 0);
            tblTopBar.Controls.Add(tools, 3, 0);
            pnlTopBar.Controls.Add(tblTopBar);
        }

        private void ConfigurePageHeader()
        {
            pnlPageHeader.Dock = DockStyle.Fill;
            pnlPageHeader.BackColor = Color.Transparent;
            pnlPageHeader.Padding = new Padding(0, 8, 0, 8);

            tblPageHeader.ColumnCount = 2;
            tblPageHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72F));
            tblPageHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            tblPageHeader.Dock = DockStyle.Fill;
            tblPageHeader.RowCount = 2;
            tblPageHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 58F));
            tblPageHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 42F));

            lblPageTitle.Dock = DockStyle.Fill;
            lblPageTitle.Text = "الرئيسية";
            lblPageTitle.ForeColor = Color.FromArgb(30, 42, 60);
            lblPageTitle.Font = UiTheme.CreateBoldFont(21F);
            lblPageTitle.TextAlign = ContentAlignment.BottomRight;

            lblPageSubtitle.Dock = DockStyle.Fill;
            lblPageSubtitle.Text = "نظرة عامة على نشاط النظام اليوم";
            lblPageSubtitle.ForeColor = Color.FromArgb(104, 116, 135);
            lblPageSubtitle.Font = UiTheme.CreateRegularFont(10F);
            lblPageSubtitle.TextAlign = ContentAlignment.TopRight;

            var customize = new Button
            {
                Dock = DockStyle.Fill,
                Text = "⚙  تخصيص الشاشة",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(47, 65, 91),
                Font = UiTheme.CreateRegularFont(10F),
                Margin = new Padding(0, 12, 0, 12)
            };
            customize.FlatAppearance.BorderColor = Color.FromArgb(222, 228, 237);

            tblPageHeader.Controls.Add(lblPageTitle, 0, 0);
            tblPageHeader.Controls.Add(lblPageSubtitle, 0, 1);
            tblPageHeader.Controls.Add(customize, 1, 0);
            tblPageHeader.SetRowSpan(customize, 2);
            pnlPageHeader.Controls.Add(tblPageHeader);
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
            tblKpis.BackColor = Color.Transparent;

            tblKpis.Controls.Add(CreateKpiCard("إجمالي الإيرادات", "1,250,000", "ريال يمني", "+12.5% ↑", Color.FromArgb(47, 128, 237)), 0, 0);
            tblKpis.Controls.Add(CreateKpiCard("إجمالي المصروفات", "860,500", "ريال يمني", "+8.3% ↑", Color.FromArgb(76, 175, 80)), 1, 0);
            tblKpis.Controls.Add(CreateKpiCard("إجمالي المعاملات", "342", "معاملة", "+15.7% ↑", Color.FromArgb(155, 81, 224)), 2, 0);
            tblKpis.Controls.Add(CreateKpiCard("العملاء النشطون", "125", "عميل", "+9.6% ↑", Color.FromArgb(242, 153, 74)), 3, 0);
            tblKpis.Controls.Add(CreateKpiCard("البوالص النشطة", "78", "بوليصة", "+6.8% ↑", Color.FromArgb(86, 204, 200)), 4, 0);
        }

        private void ConfigureAnalytics()
        {
            tblAnalytics.ColumnCount = 3;
            tblAnalytics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
            tblAnalytics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
            tblAnalytics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
            tblAnalytics.Dock = DockStyle.Fill;
            tblAnalytics.RowCount = 1;
            tblAnalytics.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblAnalytics.BackColor = Color.Transparent;

            ConfigureTransactionsCard();
            ConfigureRevenueCard();
            ConfigureActivityCard();

            tblAnalytics.Controls.Add(pnlTransactions, 0, 0);
            tblAnalytics.Controls.Add(pnlRevenueCard, 1, 0);
            tblAnalytics.Controls.Add(pnlActivityCard, 2, 0);
        }

        private void ConfigureTransactionsCard()
        {
            pnlTransactions.Dock = DockStyle.Fill;
            pnlTransactions.BackColor = Color.White;
            pnlTransactions.Margin = new Padding(0, 8, 8, 8);
            pnlTransactions.Padding = new Padding(14);

            var title = new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                Text = "آخر المعاملات  ☷",
                ForeColor = Color.FromArgb(30, 42, 60),
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
            dgvRecentTransactions.RightToLeft = RightToLeft.Yes;
            dgvRecentTransactions.DefaultCellStyle.Font = UiTheme.CreateRegularFont(9F);
            dgvRecentTransactions.DefaultCellStyle.SelectionBackColor = Color.FromArgb(240, 246, 255);
            dgvRecentTransactions.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 42, 60);
            dgvRecentTransactions.Columns.Add("Transaction", "المعاملة");
            dgvRecentTransactions.Columns.Add("Amount", "المبلغ");
            dgvRecentTransactions.Columns.Add("Status", "الحالة");

            pnlTransactions.Controls.Add(dgvRecentTransactions);
            pnlTransactions.Controls.Add(title);
        }

        private void ConfigureRevenueCard()
        {
            pnlRevenueCard.Dock = DockStyle.Fill;
            pnlRevenueCard.BackColor = Color.White;
            pnlRevenueCard.Margin = new Padding(8);
            pnlRevenueCard.Padding = new Padding(14);

            var title = new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                Text = "الإيرادات والمصروفات - آخر 6 أشهر  📈",
                ForeColor = Color.FromArgb(30, 42, 60),
                Font = UiTheme.CreateBoldFont(13F),
                TextAlign = ContentAlignment.MiddleRight
            };

            pnlRevenueChart.Dock = DockStyle.Fill;
            pnlRevenueChart.BackColor = Color.White;
            pnlRevenueChart.Paint += pnlRevenueChart_Paint;

            pnlRevenueCard.Controls.Add(pnlRevenueChart);
            pnlRevenueCard.Controls.Add(title);
        }

        private void ConfigureActivityCard()
        {
            pnlActivityCard.Dock = DockStyle.Fill;
            pnlActivityCard.BackColor = Color.White;
            pnlActivityCard.Margin = new Padding(8, 8, 0, 8);
            pnlActivityCard.Padding = new Padding(14);

            var title = new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                Text = "توزيع الإيرادات حسب النشاط  ◔",
                ForeColor = Color.FromArgb(30, 42, 60),
                Font = UiTheme.CreateBoldFont(13F),
                TextAlign = ContentAlignment.MiddleRight
            };

            pnlActivityChart.Dock = DockStyle.Fill;
            pnlActivityChart.BackColor = Color.White;
            pnlActivityChart.Paint += pnlActivityChart_Paint;

            pnlActivityCard.Controls.Add(pnlActivityChart);
            pnlActivityCard.Controls.Add(title);
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
            tblShortcuts.BackColor = Color.White;
            tblShortcuts.Margin = new Padding(0, 10, 0, 0);
            tblShortcuts.Padding = new Padding(8);

            var shortcuts = new[]
            {
                "🚚\r\nالمركبات", "📦\r\nالمخزون", "📗\r\nالحسابات", "👔\r\nالموردون", "👥\r\nالعملاء",
                "📊\r\nالتقارير", "🚛\r\nالبوالص", "📄\r\nقيد يومي", "⬆\r\nسند صرف", "⬇\r\nسند قبض"
            };

            for (var i = 0; i < shortcuts.Length; i++)
            {
                var button = new Button
                {
                    Dock = DockStyle.Fill,
                    Text = shortcuts[i],
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(47, 65, 91),
                    Font = UiTheme.CreateRegularFont(9.5F),
                    Margin = new Padding(5),
                    Cursor = Cursors.Hand
                };
                button.FlatAppearance.BorderColor = Color.FromArgb(229, 233, 240);
                button.Click += QuickAction_Click;
                tblShortcuts.Controls.Add(button, i, 0);
            }
        }

        private void ConfigureStatusBar()
        {
            statusBar.Dock = DockStyle.Fill;
            statusBar.Margin = Padding.Empty;
        }

        private static Panel CreateKpiCard(string title, string value, string unit, string growth, Color accent)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(6),
                Padding = new Padding(14)
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

            var valueLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 48,
                Text = value,
                ForeColor = Color.FromArgb(25, 32, 44),
                Font = UiTheme.CreateBoldFont(20F),
                TextAlign = ContentAlignment.MiddleRight
            };

            var unitLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Text = unit,
                ForeColor = Color.FromArgb(104, 116, 135),
                Font = UiTheme.CreateRegularFont(9F),
                TextAlign = ContentAlignment.MiddleRight
            };

            var growthLabel = new Label
            {
                Dock = DockStyle.Fill,
                Text = $"{growth}    عن الشهر الماضي",
                ForeColor = Color.FromArgb(39, 174, 96),
                Font = UiTheme.CreateRegularFont(8.5F),
                TextAlign = ContentAlignment.BottomRight
            };

            card.Controls.Add(growthLabel);
            card.Controls.Add(unitLabel);
            card.Controls.Add(valueLabel);
            card.Controls.Add(titleLabel);
            return card;
        }

        private static void ConfigureMenuButton(Button button, string text, bool selected)
        {
            button.Dock = DockStyle.Fill;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Text = text;
            button.TextAlign = ContentAlignment.MiddleRight;
            button.Padding = new Padding(12, 0, 12, 0);
            button.Font = UiTheme.CreateRegularFont(10.5F);
            button.ForeColor = Color.White;
            button.BackColor = selected ? Color.FromArgb(34, 111, 216) : Color.Transparent;
            button.Cursor = Cursors.Hand;
            button.Margin = new Padding(0, 2, 0, 2);
        }

        #endregion
    }
}
