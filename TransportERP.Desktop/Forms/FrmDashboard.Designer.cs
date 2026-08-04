using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop
{
    partial class FrmDashboard
    {
        private System.ComponentModel.IContainer? components = null;

        private TableLayoutPanel tblRoot = null!;
        private TableLayoutPanel tblMain = null!;
        private TableLayoutPanel tblHeader = null!;
        private TableLayoutPanel tblCards = null!;
        private TableLayoutPanel tblBody = null!;
        private FlowLayoutPanel flpQuickActions = null!;
        private Panel pnlSidebar = null!;
        private Panel pnlHeader = null!;
        private Panel pnlCards = null!;
        private Panel pnlRecent = null!;
        private Panel pnlAlerts = null!;
        private Label lblLogo = null!;
        private Label lblAppName = null!;
        private Label lblPageTitle = null!;
        private Label lblWelcome = null!;
        private Label lblRecentTitle = null!;
        private Label lblAlertsTitle = null!;
        private Label lblAlert1 = null!;
        private Label lblAlert2 = null!;
        private Button btnDashboard = null!;
        private Button btnGeneralSetup = null!;
        private Button btnSecurity = null!;
        private Button btnAccounting = null!;
        private Button btnReports = null!;
        private Button btnSettings = null!;
        private Button btnLogout = null!;
        private Button btnNewReceipt = null!;
        private Button btnNewPayment = null!;
        private Button btnJournalEntry = null!;
        private Button btnUsers = null!;
        private DataGridView dgvRecentOperations = null!;
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
            tblMain = new TableLayoutPanel();
            tblHeader = new TableLayoutPanel();
            tblCards = new TableLayoutPanel();
            tblBody = new TableLayoutPanel();
            flpQuickActions = new FlowLayoutPanel();
            pnlSidebar = new Panel();
            pnlHeader = new Panel();
            pnlCards = new Panel();
            pnlRecent = new Panel();
            pnlAlerts = new Panel();
            lblLogo = new Label();
            lblAppName = new Label();
            lblPageTitle = new Label();
            lblWelcome = new Label();
            lblRecentTitle = new Label();
            lblAlertsTitle = new Label();
            lblAlert1 = new Label();
            lblAlert2 = new Label();
            btnDashboard = new Button();
            btnGeneralSetup = new Button();
            btnSecurity = new Button();
            btnAccounting = new Button();
            btnReports = new Button();
            btnSettings = new Button();
            btnLogout = new Button();
            btnNewReceipt = new Button();
            btnNewPayment = new Button();
            btnJournalEntry = new Button();
            btnUsers = new Button();
            dgvRecentOperations = new DataGridView();
            statusBar = new TransportStatusBar();

            SuspendLayout();

            tblRoot.ColumnCount = 2;
            tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            tblRoot.Dock = DockStyle.Fill;
            tblRoot.RowCount = 2;
            tblRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tblRoot.BackColor = UiTheme.WindowBackground;
            tblRoot.RightToLeft = RightToLeft.Yes;

            ConfigureSidebar();
            ConfigureMainArea();
            ConfigureStatusBar();

            tblRoot.Controls.Add(pnlSidebar, 1, 0);
            tblRoot.Controls.Add(tblMain, 0, 0);
            tblRoot.Controls.Add(statusBar, 0, 1);
            tblRoot.SetColumnSpan(statusBar, 2);

            AutoScaleMode = AutoScaleMode.Font;
            BackColor = UiTheme.WindowBackground;
            ClientSize = new Size(1600, 920);
            Controls.Add(tblRoot);
            Font = UiTheme.CreateRegularFont(10F);
            MinimumSize = new Size(1280, 760);
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
            pnlSidebar.BackColor = Color.FromArgb(17, 41, 84);
            pnlSidebar.Dock = DockStyle.Fill;
            pnlSidebar.Padding = new Padding(18, 24, 18, 18);

            var sidebarLayout = new TableLayoutPanel
            {
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                RowCount = 10,
                BackColor = Color.Transparent,
                Margin = Padding.Empty
            };

            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            for (var i = 0; i < 6; i++)
            {
                sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            }
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));

            lblLogo.Dock = DockStyle.Fill;
            lblLogo.Text = "TransportERP";
            lblLogo.ForeColor = Color.White;
            lblLogo.Font = UiTheme.CreateBoldFont(19F);
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;

            lblAppName.Dock = DockStyle.Fill;
            lblAppName.Text = "نظام النقل والخدمات اللوجستية";
            lblAppName.ForeColor = Color.FromArgb(196, 214, 244);
            lblAppName.Font = UiTheme.CreateRegularFont(9F);
            lblAppName.TextAlign = ContentAlignment.MiddleCenter;

            ConfigureMenuButton(btnDashboard, "⌂  لوحة المعلومات", true);
            ConfigureMenuButton(btnGeneralSetup, "⚙  التهيئة العامة", false);
            ConfigureMenuButton(btnSecurity, "◈  الأمن والإدارة", false);
            ConfigureMenuButton(btnAccounting, "▣  المحاسبة", false);
            ConfigureMenuButton(btnReports, "▤  التقارير", false);
            ConfigureMenuButton(btnSettings, "☰  الإعدادات", false);
            ConfigureMenuButton(btnLogout, "↪  تسجيل الخروج", false);
            btnLogout.Click += btnLogout_Click;

            sidebarLayout.Controls.Add(lblLogo, 0, 0);
            sidebarLayout.Controls.Add(lblAppName, 0, 1);
            sidebarLayout.Controls.Add(btnDashboard, 0, 2);
            sidebarLayout.Controls.Add(btnGeneralSetup, 0, 3);
            sidebarLayout.Controls.Add(btnSecurity, 0, 4);
            sidebarLayout.Controls.Add(btnAccounting, 0, 5);
            sidebarLayout.Controls.Add(btnReports, 0, 6);
            sidebarLayout.Controls.Add(btnSettings, 0, 7);
            sidebarLayout.Controls.Add(btnLogout, 0, 9);
            pnlSidebar.Controls.Add(sidebarLayout);
        }

        private void ConfigureMainArea()
        {
            tblMain.ColumnCount = 1;
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblMain.Dock = DockStyle.Fill;
            tblMain.Padding = new Padding(24, 20, 24, 14);
            tblMain.RowCount = 4;
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 168F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 104F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblMain.BackColor = UiTheme.WindowBackground;

            ConfigureHeader();
            ConfigureCards();
            ConfigureQuickActions();
            ConfigureBody();

            tblMain.Controls.Add(pnlHeader, 0, 0);
            tblMain.Controls.Add(pnlCards, 0, 1);
            tblMain.Controls.Add(flpQuickActions, 0, 2);
            tblMain.Controls.Add(tblBody, 0, 3);
        }

        private void ConfigureHeader()
        {
            pnlHeader.BackColor = Color.White;
            pnlHeader.Dock = DockStyle.Fill;
            pnlHeader.Padding = new Padding(20, 12, 20, 12);
            pnlHeader.Margin = new Padding(0, 0, 0, 12);

            tblHeader.ColumnCount = 2;
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tblHeader.Dock = DockStyle.Fill;
            tblHeader.RowCount = 2;
            tblHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 58F));
            tblHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 42F));

            lblPageTitle.Dock = DockStyle.Fill;
            lblPageTitle.Text = "لوحة المعلومات";
            lblPageTitle.ForeColor = UiTheme.HeadingText;
            lblPageTitle.Font = UiTheme.CreateBoldFont(22F);
            lblPageTitle.TextAlign = ContentAlignment.BottomRight;

            lblWelcome.Dock = DockStyle.Fill;
            lblWelcome.Text = "مرحبًا بك، إليك ملخص حالة النظام اليوم";
            lblWelcome.ForeColor = UiTheme.SecondaryText;
            lblWelcome.Font = UiTheme.CreateRegularFont(10F);
            lblWelcome.TextAlign = ContentAlignment.TopRight;

            var dateLabel = new Label
            {
                Dock = DockStyle.Fill,
                Text = DateTime.Today.ToString("yyyy/MM/dd"),
                ForeColor = UiTheme.SecondaryText,
                Font = UiTheme.CreateRegularFont(10F),
                TextAlign = ContentAlignment.MiddleLeft
            };

            tblHeader.Controls.Add(lblPageTitle, 0, 0);
            tblHeader.Controls.Add(lblWelcome, 0, 1);
            tblHeader.Controls.Add(dateLabel, 1, 0);
            tblHeader.SetRowSpan(dateLabel, 2);
            pnlHeader.Controls.Add(tblHeader);
        }

        private void ConfigureCards()
        {
            pnlCards.BackColor = Color.Transparent;
            pnlCards.Dock = DockStyle.Fill;
            pnlCards.Margin = Padding.Empty;

            tblCards.ColumnCount = 4;
            for (var i = 0; i < 4; i++)
            {
                tblCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            }
            tblCards.Dock = DockStyle.Fill;
            tblCards.RowCount = 1;
            tblCards.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            tblCards.Controls.Add(CreateSummaryCard("إجمالي الشركات", "12", "+2 هذا الشهر"), 0, 0);
            tblCards.Controls.Add(CreateSummaryCard("الفروع النشطة", "34", "جميعها متصلة"), 1, 0);
            tblCards.Controls.Add(CreateSummaryCard("المستخدمون", "86", "72 مستخدمًا نشطًا"), 2, 0);
            tblCards.Controls.Add(CreateSummaryCard("عمليات اليوم", "248", "+18% عن الأمس"), 3, 0);

            pnlCards.Controls.Add(tblCards);
        }

        private void ConfigureQuickActions()
        {
            flpQuickActions.Dock = DockStyle.Fill;
            flpQuickActions.FlowDirection = FlowDirection.RightToLeft;
            flpQuickActions.WrapContents = false;
            flpQuickActions.Padding = new Padding(0, 10, 0, 10);
            flpQuickActions.BackColor = Color.Transparent;
            flpQuickActions.Margin = Padding.Empty;

            ConfigureQuickButton(btnNewReceipt, "سند قبض جديد");
            ConfigureQuickButton(btnNewPayment, "سند صرف جديد");
            ConfigureQuickButton(btnJournalEntry, "قيد يومي");
            ConfigureQuickButton(btnUsers, "إدارة المستخدمين");

            flpQuickActions.Controls.Add(btnNewReceipt);
            flpQuickActions.Controls.Add(btnNewPayment);
            flpQuickActions.Controls.Add(btnJournalEntry);
            flpQuickActions.Controls.Add(btnUsers);
        }

        private void ConfigureBody()
        {
            tblBody.ColumnCount = 2;
            tblBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tblBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tblBody.Dock = DockStyle.Fill;
            tblBody.RowCount = 1;
            tblBody.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            ConfigureRecentPanel();
            ConfigureAlertsPanel();

            tblBody.Controls.Add(pnlRecent, 0, 0);
            tblBody.Controls.Add(pnlAlerts, 1, 0);
        }

        private void ConfigureRecentPanel()
        {
            pnlRecent.BackColor = Color.White;
            pnlRecent.Dock = DockStyle.Fill;
            pnlRecent.Padding = new Padding(18);
            pnlRecent.Margin = new Padding(0, 0, 10, 0);

            lblRecentTitle.Dock = DockStyle.Top;
            lblRecentTitle.Height = 38;
            lblRecentTitle.Text = "آخر العمليات";
            lblRecentTitle.ForeColor = UiTheme.HeadingText;
            lblRecentTitle.Font = UiTheme.CreateBoldFont(14F);
            lblRecentTitle.TextAlign = ContentAlignment.MiddleRight;

            dgvRecentOperations.Dock = DockStyle.Fill;
            dgvRecentOperations.AllowUserToAddRows = false;
            dgvRecentOperations.AllowUserToDeleteRows = false;
            dgvRecentOperations.AllowUserToResizeRows = false;
            dgvRecentOperations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRecentOperations.BackgroundColor = Color.White;
            dgvRecentOperations.BorderStyle = BorderStyle.None;
            dgvRecentOperations.ColumnHeadersHeight = 40;
            dgvRecentOperations.RowHeadersVisible = false;
            dgvRecentOperations.ReadOnly = true;
            dgvRecentOperations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecentOperations.RightToLeft = RightToLeft.Yes;
            dgvRecentOperations.Columns.Add("OperationType", "نوع العملية");
            dgvRecentOperations.Columns.Add("Reference", "المرجع");
            dgvRecentOperations.Columns.Add("Entity", "الجهة");
            dgvRecentOperations.Columns.Add("Amount", "المبلغ");
            dgvRecentOperations.Columns.Add("Status", "الحالة");

            pnlRecent.Controls.Add(dgvRecentOperations);
            pnlRecent.Controls.Add(lblRecentTitle);
        }

        private void ConfigureAlertsPanel()
        {
            pnlAlerts.BackColor = Color.White;
            pnlAlerts.Dock = DockStyle.Fill;
            pnlAlerts.Padding = new Padding(18);
            pnlAlerts.Margin = new Padding(10, 0, 0, 0);

            lblAlertsTitle.Dock = DockStyle.Top;
            lblAlertsTitle.Height = 38;
            lblAlertsTitle.Text = "التنبيهات";
            lblAlertsTitle.ForeColor = UiTheme.HeadingText;
            lblAlertsTitle.Font = UiTheme.CreateBoldFont(14F);
            lblAlertsTitle.TextAlign = ContentAlignment.MiddleRight;

            lblAlert1.Dock = DockStyle.Top;
            lblAlert1.Height = 74;
            lblAlert1.Padding = new Padding(10);
            lblAlert1.Text = "• لم يتم ربط واجهة API بعد.\r\n  تعمل الشاشة ببيانات معاينة.";
            lblAlert1.ForeColor = Color.FromArgb(157, 98, 0);
            lblAlert1.BackColor = Color.FromArgb(255, 248, 225);
            lblAlert1.TextAlign = ContentAlignment.MiddleRight;

            lblAlert2.Dock = DockStyle.Top;
            lblAlert2.Height = 74;
            lblAlert2.Padding = new Padding(10);
            lblAlert2.Margin = new Padding(0, 10, 0, 0);
            lblAlert2.Text = "• توجد 3 مهام إعداد تحتاج مراجعة قبل التشغيل الفعلي.";
            lblAlert2.ForeColor = Color.FromArgb(33, 85, 122);
            lblAlert2.BackColor = Color.FromArgb(232, 244, 253);
            lblAlert2.TextAlign = ContentAlignment.MiddleRight;

            pnlAlerts.Controls.Add(lblAlert2);
            pnlAlerts.Controls.Add(lblAlert1);
            pnlAlerts.Controls.Add(lblAlertsTitle);
        }

        private void ConfigureStatusBar()
        {
            statusBar.Dock = DockStyle.Fill;
            statusBar.Margin = Padding.Empty;
        }

        private static Panel CreateSummaryCard(string title, string value, string note)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(8),
                Padding = new Padding(16)
            };

            var lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Text = title,
                ForeColor = UiTheme.SecondaryText,
                Font = UiTheme.CreateRegularFont(10F),
                TextAlign = ContentAlignment.MiddleRight
            };

            var lblValue = new Label
            {
                Dock = DockStyle.Top,
                Height = 52,
                Text = value,
                ForeColor = UiTheme.PrimaryBlue,
                Font = UiTheme.CreateBoldFont(24F),
                TextAlign = ContentAlignment.MiddleRight
            };

            var lblNote = new Label
            {
                Dock = DockStyle.Fill,
                Text = note,
                ForeColor = UiTheme.SecondaryText,
                Font = UiTheme.CreateRegularFont(9F),
                TextAlign = ContentAlignment.MiddleRight
            };

            card.Controls.Add(lblNote);
            card.Controls.Add(lblValue);
            card.Controls.Add(lblTitle);
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
            button.BackColor = selected ? Color.FromArgb(28, 92, 190) : Color.Transparent;
            button.Cursor = Cursors.Hand;
        }

        private void ConfigureQuickButton(Button button, string text)
        {
            button.Width = 190;
            button.Height = 54;
            button.Margin = new Padding(8);
            button.Text = text;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = UiTheme.PrimaryBlue;
            button.ForeColor = Color.White;
            button.Font = UiTheme.CreateBoldFont(10F);
            button.Cursor = Cursors.Hand;
            button.Click += QuickAction_Click;
        }

        #endregion
    }
}
