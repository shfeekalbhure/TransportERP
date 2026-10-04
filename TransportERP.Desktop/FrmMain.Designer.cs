namespace TransportERP.Desktop
{
    public partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing) { uiClockTimer.Dispose(); components?.Dispose(); }
            base.Dispose(disposing);
        }
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            navigationImages = new ImageList(components);
            mainLayout = new TableLayoutPanel();
            tlpCompanyInfo = new TableLayoutPanel();
            lblCompanyName = new Label();
            lblBranch = new Label();
            lblFinancialYear = new Label();
            lblCurrentUser = new Label();
            lblCurrentDate = new Label();
            pnlNotifications = new Panel();
            leftLayout = new TableLayoutPanel();
            lblCompanyLogo = new Label();
            btnRefreshNotifications = new Button();
            notificationBox = new Panel();
            lblNotificationSummary = new Label();
            lblNotificationsTitle = new Label();
            favoritesBox = new Panel();
            lstDashboardFavorites = new ListBox();
            lblFavoritesTitle = new Label();
            userBox = new Panel();
            userInfoLayout = new TableLayoutPanel();
            lblUserCaption = new Label();
            lblUserValue = new Label();
            lblBranchCaption = new Label();
            lblBranchValue = new Label();
            lblCompanyCaption = new Label();
            lblCompanyValue = new Label();
            lblYearCaption = new Label();
            lblYearValue = new Label();
            lblTimeCaption = new Label();
            lblCurrentTime = new Label();
            lblUserInfoTitle = new Label();
            btnContact = new Button();
            currencyGroup = new GroupBox();
            currencyGrid = new DataGridView();
            currencyCode = new DataGridViewTextBoxColumn();
            currencyName = new DataGridViewTextBoxColumn();
            currencyRate = new DataGridViewTextBoxColumn();
            currencyMinor = new DataGridViewTextBoxColumn();
            pnlMainContent = new Panel();
            workspaceLayout = new TableLayoutPanel();
            workspaceSurface = new Panel();
            lblOpenScreens = new Label();
            pnlSystemTree = new Panel();
            treeBody = new Panel();
            tvSystemTree = new TreeView();
            treeSearchLayout = new TableLayoutPanel();
            lblTreeSearch = new Label();
            txtTreeSearch = new TextBox();
            navigationRail = new Panel();
            railSymbols = new FlowLayoutPanel();
            railArrow = new Button();
            railRefresh = new Label();
            railStar = new Label();
            railPlay = new Label();
            railFlag = new Label();
            railPower = new Label();
            mainLayout.SuspendLayout();
            tlpCompanyInfo.SuspendLayout();
            pnlNotifications.SuspendLayout();
            leftLayout.SuspendLayout();
            notificationBox.SuspendLayout();
            favoritesBox.SuspendLayout();
            userBox.SuspendLayout();
            userInfoLayout.SuspendLayout();
            currencyGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)currencyGrid).BeginInit();
            pnlMainContent.SuspendLayout();
            workspaceLayout.SuspendLayout();
            pnlSystemTree.SuspendLayout();
            treeBody.SuspendLayout();
            treeSearchLayout.SuspendLayout();
            navigationRail.SuspendLayout();
            railSymbols.SuspendLayout();
            SuspendLayout();
            // 
            // navigationImages
            // 
            navigationImages.ColorDepth = ColorDepth.Depth32Bit;
            navigationImages.ImageSize = new Size(16, 16);
            navigationImages.TransparentColor = Color.Transparent;
            // 
            // mainLayout
            // 
            mainLayout.ColumnCount = 3;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 280F));
            mainLayout.Controls.Add(tlpCompanyInfo, 0, 0);
            mainLayout.Controls.Add(pnlNotifications, 0, 1);
            mainLayout.Controls.Add(pnlMainContent, 1, 1);
            mainLayout.Controls.Add(pnlSystemTree, 2, 1);
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.Location = new Point(0, 0);
            mainLayout.Name = "mainLayout";
            mainLayout.Padding = new Padding(5);
            mainLayout.RightToLeft = RightToLeft.No;
            mainLayout.RowCount = 2;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainLayout.Size = new Size(1100, 680);
            mainLayout.TabIndex = 0;
            // 
            // tlpCompanyInfo
            // 
            tlpCompanyInfo.BackColor = Color.FromArgb(238, 238, 230);
            tlpCompanyInfo.ColumnCount = 5;
            mainLayout.SetColumnSpan(tlpCompanyInfo, 3);
            tlpCompanyInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpCompanyInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpCompanyInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpCompanyInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpCompanyInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpCompanyInfo.Controls.Add(lblCompanyName, 0, 0);
            tlpCompanyInfo.Controls.Add(lblBranch, 1, 0);
            tlpCompanyInfo.Controls.Add(lblFinancialYear, 2, 0);
            tlpCompanyInfo.Controls.Add(lblCurrentUser, 3, 0);
            tlpCompanyInfo.Controls.Add(lblCurrentDate, 4, 0);
            tlpCompanyInfo.Dock = DockStyle.Fill;
            tlpCompanyInfo.Location = new Point(5, 5);
            tlpCompanyInfo.Margin = new Padding(0);
            tlpCompanyInfo.Name = "tlpCompanyInfo";
            tlpCompanyInfo.Padding = new Padding(6, 0, 6, 0);
            tlpCompanyInfo.RightToLeft = RightToLeft.Yes;
            tlpCompanyInfo.RowCount = 1;
            tlpCompanyInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpCompanyInfo.Size = new Size(1090, 32);
            tlpCompanyInfo.TabIndex = 0;
            // 
            // lblCompanyName
            // 
            lblCompanyName.AutoEllipsis = true;
            lblCompanyName.Dock = DockStyle.Fill;
            lblCompanyName.Location = new Point(869, 0);
            lblCompanyName.Margin = new Padding(0);
            lblCompanyName.Name = "lblCompanyName";
            lblCompanyName.RightToLeft = RightToLeft.Yes;
            lblCompanyName.Size = new Size(215, 32);
            lblCompanyName.TabIndex = 0;
            lblCompanyName.Text = "الشركة: —";
            lblCompanyName.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblBranch
            // 
            lblBranch.AutoEllipsis = true;
            lblBranch.Dock = DockStyle.Fill;
            lblBranch.Location = new Point(654, 0);
            lblBranch.Margin = new Padding(0);
            lblBranch.Name = "lblBranch";
            lblBranch.RightToLeft = RightToLeft.Yes;
            lblBranch.Size = new Size(215, 32);
            lblBranch.TabIndex = 1;
            lblBranch.Text = "الفرع: —";
            lblBranch.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblFinancialYear
            // 
            lblFinancialYear.AutoEllipsis = true;
            lblFinancialYear.Dock = DockStyle.Fill;
            lblFinancialYear.Location = new Point(439, 0);
            lblFinancialYear.Margin = new Padding(0);
            lblFinancialYear.Name = "lblFinancialYear";
            lblFinancialYear.RightToLeft = RightToLeft.Yes;
            lblFinancialYear.Size = new Size(215, 32);
            lblFinancialYear.TabIndex = 2;
            lblFinancialYear.Text = "السنة المالية: —";
            lblFinancialYear.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCurrentUser
            // 
            lblCurrentUser.AutoEllipsis = true;
            lblCurrentUser.Dock = DockStyle.Fill;
            lblCurrentUser.Location = new Point(224, 0);
            lblCurrentUser.Margin = new Padding(0);
            lblCurrentUser.Name = "lblCurrentUser";
            lblCurrentUser.RightToLeft = RightToLeft.Yes;
            lblCurrentUser.Size = new Size(215, 32);
            lblCurrentUser.TabIndex = 3;
            lblCurrentUser.Text = "المستخدم: —";
            lblCurrentUser.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCurrentDate
            // 
            lblCurrentDate.AutoEllipsis = true;
            lblCurrentDate.Dock = DockStyle.Fill;
            lblCurrentDate.Location = new Point(6, 0);
            lblCurrentDate.Margin = new Padding(0);
            lblCurrentDate.Name = "lblCurrentDate";
            lblCurrentDate.RightToLeft = RightToLeft.Yes;
            lblCurrentDate.Size = new Size(218, 32);
            lblCurrentDate.TabIndex = 4;
            lblCurrentDate.Text = "التاريخ: —";
            lblCurrentDate.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlNotifications
            // 
            pnlNotifications.AutoScroll = true;
            pnlNotifications.BackColor = Color.FromArgb(211, 221, 160);
            pnlNotifications.Controls.Add(leftLayout);
            pnlNotifications.Controls.Add(currencyGroup);
            pnlNotifications.Dock = DockStyle.Fill;
            pnlNotifications.Location = new Point(5, 37);
            pnlNotifications.Margin = new Padding(0);
            pnlNotifications.Name = "pnlNotifications";
            pnlNotifications.Padding = new Padding(7);
            pnlNotifications.Size = new Size(200, 638);
            pnlNotifications.TabIndex = 1;
            // 
            // leftLayout
            // 
            leftLayout.AutoSize = true;
            leftLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            leftLayout.ColumnCount = 1;
            leftLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            leftLayout.Controls.Add(lblCompanyLogo, 0, 0);
            leftLayout.Controls.Add(btnRefreshNotifications, 0, 1);
            leftLayout.Controls.Add(notificationBox, 0, 2);
            leftLayout.Controls.Add(favoritesBox, 0, 3);
            leftLayout.Controls.Add(userBox, 0, 4);
            leftLayout.Controls.Add(btnContact, 0, 5);
            leftLayout.Dock = DockStyle.Top;
            leftLayout.Location = new Point(7, 7);
            leftLayout.Name = "leftLayout";
            leftLayout.RightToLeft = RightToLeft.Yes;
            leftLayout.RowCount = 6;
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 196F));
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            leftLayout.Size = new Size(165, 510);
            leftLayout.TabIndex = 0;
            // 
            // lblCompanyLogo
            // 
            lblCompanyLogo.BackColor = Color.FromArgb(247, 248, 237);
            lblCompanyLogo.BorderStyle = BorderStyle.FixedSingle;
            lblCompanyLogo.Dock = DockStyle.Fill;
            lblCompanyLogo.Font = new Font("Tahoma", 17F, FontStyle.Bold);
            lblCompanyLogo.ForeColor = Color.FromArgb(84, 99, 29);
            lblCompanyLogo.Location = new Point(0, 0);
            lblCompanyLogo.Margin = new Padding(0, 0, 0, 6);
            lblCompanyLogo.Name = "lblCompanyLogo";
            lblCompanyLogo.RightToLeft = RightToLeft.Yes;
            lblCompanyLogo.Size = new Size(165, 94);
            lblCompanyLogo.TabIndex = 0;
            lblCompanyLogo.Text = "شعار الشركة";
            lblCompanyLogo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnRefreshNotifications
            // 
            btnRefreshNotifications.Dock = DockStyle.Fill;
            btnRefreshNotifications.Enabled = false;
            btnRefreshNotifications.Location = new Point(0, 102);
            btnRefreshNotifications.Margin = new Padding(0, 2, 0, 4);
            btnRefreshNotifications.Name = "btnRefreshNotifications";
            btnRefreshNotifications.Size = new Size(165, 24);
            btnRefreshNotifications.TabIndex = 1;
            btnRefreshNotifications.Text = "تحديث التنبيهات";
            // 
            // notificationBox
            // 
            notificationBox.BackColor = Color.FromArgb(246, 247, 235);
            notificationBox.Controls.Add(lblNotificationSummary);
            notificationBox.Controls.Add(lblNotificationsTitle);
            notificationBox.Dock = DockStyle.Fill;
            notificationBox.Location = new Point(0, 134);
            notificationBox.Margin = new Padding(0, 4, 0, 6);
            notificationBox.Name = "notificationBox";
            notificationBox.Size = new Size(165, 46);
            notificationBox.TabIndex = 2;
            // 
            // lblNotificationSummary
            // 
            lblNotificationSummary.Dock = DockStyle.Fill;
            lblNotificationSummary.ForeColor = Color.DimGray;
            lblNotificationSummary.Location = new Point(0, 23);
            lblNotificationSummary.Margin = new Padding(0);
            lblNotificationSummary.Name = "lblNotificationSummary";
            lblNotificationSummary.Padding = new Padding(5, 23, 5, 0);
            lblNotificationSummary.RightToLeft = RightToLeft.Yes;
            lblNotificationSummary.Size = new Size(165, 23);
            lblNotificationSummary.TabIndex = 0;
            lblNotificationSummary.Text = "لا توجد بيانات تنبيهات";
            lblNotificationSummary.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblNotificationsTitle
            // 
            lblNotificationsTitle.BackColor = Color.FromArgb(139, 163, 45);
            lblNotificationsTitle.Dock = DockStyle.Top;
            lblNotificationsTitle.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblNotificationsTitle.ForeColor = Color.White;
            lblNotificationsTitle.Location = new Point(0, 0);
            lblNotificationsTitle.Margin = new Padding(0);
            lblNotificationsTitle.Name = "lblNotificationsTitle";
            lblNotificationsTitle.Padding = new Padding(5, 0, 5, 0);
            lblNotificationsTitle.RightToLeft = RightToLeft.Yes;
            lblNotificationsTitle.Size = new Size(165, 23);
            lblNotificationsTitle.TabIndex = 1;
            lblNotificationsTitle.Text = "إدارة التنبيهات";
            lblNotificationsTitle.TextAlign = ContentAlignment.MiddleRight;
            // 
            // favoritesBox
            // 
            favoritesBox.BackColor = Color.White;
            favoritesBox.Controls.Add(lstDashboardFavorites);
            favoritesBox.Controls.Add(lblFavoritesTitle);
            favoritesBox.Dock = DockStyle.Fill;
            favoritesBox.Location = new Point(0, 190);
            favoritesBox.Margin = new Padding(0, 4, 0, 6);
            favoritesBox.Name = "favoritesBox";
            favoritesBox.Size = new Size(165, 90);
            favoritesBox.TabIndex = 3;
            // 
            // lstDashboardFavorites
            // 
            lstDashboardFavorites.BorderStyle = BorderStyle.FixedSingle;
            lstDashboardFavorites.Dock = DockStyle.Fill;
            lstDashboardFavorites.IntegralHeight = false;
            lstDashboardFavorites.Location = new Point(0, 23);
            lstDashboardFavorites.Margin = new Padding(0);
            lstDashboardFavorites.Name = "lstDashboardFavorites";
            lstDashboardFavorites.RightToLeft = RightToLeft.Yes;
            lstDashboardFavorites.Size = new Size(165, 67);
            lstDashboardFavorites.TabIndex = 0;
            // 
            // lblFavoritesTitle
            // 
            lblFavoritesTitle.BackColor = Color.FromArgb(139, 163, 45);
            lblFavoritesTitle.Dock = DockStyle.Top;
            lblFavoritesTitle.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblFavoritesTitle.ForeColor = Color.White;
            lblFavoritesTitle.Location = new Point(0, 0);
            lblFavoritesTitle.Margin = new Padding(0);
            lblFavoritesTitle.Name = "lblFavoritesTitle";
            lblFavoritesTitle.Padding = new Padding(5, 0, 5, 0);
            lblFavoritesTitle.RightToLeft = RightToLeft.Yes;
            lblFavoritesTitle.Size = new Size(165, 23);
            lblFavoritesTitle.TabIndex = 1;
            lblFavoritesTitle.Text = "الشاشات المفضلة";
            lblFavoritesTitle.TextAlign = ContentAlignment.MiddleRight;
            // 
            // userBox
            // 
            userBox.BackColor = Color.FromArgb(246, 247, 235);
            userBox.Controls.Add(userInfoLayout);
            userBox.Controls.Add(lblUserInfoTitle);
            userBox.Dock = DockStyle.Fill;
            userBox.Location = new Point(0, 290);
            userBox.Margin = new Padding(0, 4, 0, 6);
            userBox.Name = "userBox";
            userBox.Size = new Size(165, 186);
            userBox.TabIndex = 4;
            // 
            // userInfoLayout
            // 
            userInfoLayout.ColumnCount = 1;
            userInfoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            userInfoLayout.Controls.Add(lblUserCaption, 0, 0);
            userInfoLayout.Controls.Add(lblUserValue, 0, 1);
            userInfoLayout.Controls.Add(lblBranchCaption, 0, 2);
            userInfoLayout.Controls.Add(lblBranchValue, 0, 3);
            userInfoLayout.Controls.Add(lblCompanyCaption, 0, 4);
            userInfoLayout.Controls.Add(lblCompanyValue, 0, 5);
            userInfoLayout.Controls.Add(lblYearCaption, 0, 6);
            userInfoLayout.Controls.Add(lblYearValue, 0, 7);
            userInfoLayout.Controls.Add(lblTimeCaption, 0, 8);
            userInfoLayout.Controls.Add(lblCurrentTime, 0, 9);
            userInfoLayout.Dock = DockStyle.Fill;
            userInfoLayout.Location = new Point(0, 23);
            userInfoLayout.Margin = new Padding(0);
            userInfoLayout.Name = "userInfoLayout";
            userInfoLayout.Padding = new Padding(4, 2, 4, 2);
            userInfoLayout.RightToLeft = RightToLeft.Yes;
            userInfoLayout.RowCount = 10;
            userInfoLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            userInfoLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            userInfoLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            userInfoLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            userInfoLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            userInfoLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            userInfoLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            userInfoLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            userInfoLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            userInfoLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            userInfoLayout.Size = new Size(165, 163);
            userInfoLayout.TabIndex = 0;
            // 
            // lblUserCaption
            // 
            lblUserCaption.AutoEllipsis = true;
            lblUserCaption.BackColor = Color.FromArgb(241, 237, 154);
            lblUserCaption.Dock = DockStyle.Fill;
            lblUserCaption.Location = new Point(4, 2);
            lblUserCaption.Margin = new Padding(0);
            lblUserCaption.Name = "lblUserCaption";
            lblUserCaption.RightToLeft = RightToLeft.Yes;
            lblUserCaption.Size = new Size(157, 15);
            lblUserCaption.TabIndex = 0;
            lblUserCaption.Text = "الاسم";
            lblUserCaption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUserValue
            // 
            lblUserValue.AutoEllipsis = true;
            lblUserValue.BackColor = Color.FromArgb(246, 247, 235);
            lblUserValue.Dock = DockStyle.Fill;
            lblUserValue.Location = new Point(4, 17);
            lblUserValue.Margin = new Padding(0);
            lblUserValue.Name = "lblUserValue";
            lblUserValue.RightToLeft = RightToLeft.Yes;
            lblUserValue.Size = new Size(157, 15);
            lblUserValue.TabIndex = 1;
            lblUserValue.Text = "—";
            lblUserValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblBranchCaption
            // 
            lblBranchCaption.AutoEllipsis = true;
            lblBranchCaption.BackColor = Color.FromArgb(241, 237, 154);
            lblBranchCaption.Dock = DockStyle.Fill;
            lblBranchCaption.Location = new Point(4, 32);
            lblBranchCaption.Margin = new Padding(0);
            lblBranchCaption.Name = "lblBranchCaption";
            lblBranchCaption.RightToLeft = RightToLeft.Yes;
            lblBranchCaption.Size = new Size(157, 15);
            lblBranchCaption.TabIndex = 2;
            lblBranchCaption.Text = "الفرع";
            lblBranchCaption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblBranchValue
            // 
            lblBranchValue.AutoEllipsis = true;
            lblBranchValue.BackColor = Color.FromArgb(246, 247, 235);
            lblBranchValue.Dock = DockStyle.Fill;
            lblBranchValue.Location = new Point(4, 47);
            lblBranchValue.Margin = new Padding(0);
            lblBranchValue.Name = "lblBranchValue";
            lblBranchValue.RightToLeft = RightToLeft.Yes;
            lblBranchValue.Size = new Size(157, 15);
            lblBranchValue.TabIndex = 3;
            lblBranchValue.Text = "—";
            lblBranchValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCompanyCaption
            // 
            lblCompanyCaption.AutoEllipsis = true;
            lblCompanyCaption.BackColor = Color.FromArgb(241, 237, 154);
            lblCompanyCaption.Dock = DockStyle.Fill;
            lblCompanyCaption.Location = new Point(4, 62);
            lblCompanyCaption.Margin = new Padding(0);
            lblCompanyCaption.Name = "lblCompanyCaption";
            lblCompanyCaption.RightToLeft = RightToLeft.Yes;
            lblCompanyCaption.Size = new Size(157, 15);
            lblCompanyCaption.TabIndex = 4;
            lblCompanyCaption.Text = "الشركة";
            lblCompanyCaption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCompanyValue
            // 
            lblCompanyValue.AutoEllipsis = true;
            lblCompanyValue.BackColor = Color.FromArgb(246, 247, 235);
            lblCompanyValue.Dock = DockStyle.Fill;
            lblCompanyValue.Location = new Point(4, 77);
            lblCompanyValue.Margin = new Padding(0);
            lblCompanyValue.Name = "lblCompanyValue";
            lblCompanyValue.RightToLeft = RightToLeft.Yes;
            lblCompanyValue.Size = new Size(157, 15);
            lblCompanyValue.TabIndex = 5;
            lblCompanyValue.Text = "—";
            lblCompanyValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblYearCaption
            // 
            lblYearCaption.AutoEllipsis = true;
            lblYearCaption.BackColor = Color.FromArgb(241, 237, 154);
            lblYearCaption.Dock = DockStyle.Fill;
            lblYearCaption.Location = new Point(4, 92);
            lblYearCaption.Margin = new Padding(0);
            lblYearCaption.Name = "lblYearCaption";
            lblYearCaption.RightToLeft = RightToLeft.Yes;
            lblYearCaption.Size = new Size(157, 15);
            lblYearCaption.TabIndex = 6;
            lblYearCaption.Text = "السنة المالية";
            lblYearCaption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblYearValue
            // 
            lblYearValue.AutoEllipsis = true;
            lblYearValue.BackColor = Color.FromArgb(246, 247, 235);
            lblYearValue.Dock = DockStyle.Fill;
            lblYearValue.Location = new Point(4, 107);
            lblYearValue.Margin = new Padding(0);
            lblYearValue.Name = "lblYearValue";
            lblYearValue.RightToLeft = RightToLeft.Yes;
            lblYearValue.Size = new Size(157, 15);
            lblYearValue.TabIndex = 7;
            lblYearValue.Text = "—";
            lblYearValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTimeCaption
            // 
            lblTimeCaption.AutoEllipsis = true;
            lblTimeCaption.BackColor = Color.FromArgb(241, 237, 154);
            lblTimeCaption.Dock = DockStyle.Fill;
            lblTimeCaption.Location = new Point(4, 122);
            lblTimeCaption.Margin = new Padding(0);
            lblTimeCaption.Name = "lblTimeCaption";
            lblTimeCaption.RightToLeft = RightToLeft.Yes;
            lblTimeCaption.Size = new Size(157, 15);
            lblTimeCaption.TabIndex = 8;
            lblTimeCaption.Text = "الوقت الحالي";
            lblTimeCaption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCurrentTime
            // 
            lblCurrentTime.AutoEllipsis = true;
            lblCurrentTime.BackColor = Color.FromArgb(246, 247, 235);
            lblCurrentTime.Dock = DockStyle.Fill;
            lblCurrentTime.Location = new Point(4, 137);
            lblCurrentTime.Margin = new Padding(0);
            lblCurrentTime.Name = "lblCurrentTime";
            lblCurrentTime.RightToLeft = RightToLeft.Yes;
            lblCurrentTime.Size = new Size(157, 24);
            lblCurrentTime.TabIndex = 9;
            lblCurrentTime.Text = "—";
            lblCurrentTime.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUserInfoTitle
            // 
            lblUserInfoTitle.BackColor = Color.FromArgb(139, 163, 45);
            lblUserInfoTitle.Dock = DockStyle.Top;
            lblUserInfoTitle.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblUserInfoTitle.ForeColor = Color.White;
            lblUserInfoTitle.Location = new Point(0, 0);
            lblUserInfoTitle.Margin = new Padding(0);
            lblUserInfoTitle.Name = "lblUserInfoTitle";
            lblUserInfoTitle.Padding = new Padding(5, 0, 5, 0);
            lblUserInfoTitle.RightToLeft = RightToLeft.Yes;
            lblUserInfoTitle.Size = new Size(165, 23);
            lblUserInfoTitle.TabIndex = 1;
            lblUserInfoTitle.Text = "معلومات المستخدم";
            lblUserInfoTitle.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnContact
            // 
            btnContact.Dock = DockStyle.Fill;
            btnContact.Enabled = false;
            btnContact.Location = new Point(0, 485);
            btnContact.Margin = new Padding(0, 3, 0, 0);
            btnContact.Name = "btnContact";
            btnContact.Size = new Size(165, 25);
            btnContact.TabIndex = 5;
            btnContact.Text = "اتصل بنا";
            // 
            // currencyGroup
            // 
            currencyGroup.Controls.Add(currencyGrid);
            currencyGroup.Dock = DockStyle.Bottom;
            currencyGroup.Location = new Point(7, 510);
            currencyGroup.Name = "currencyGroup";
            currencyGroup.Padding = new Padding(5);
            currencyGroup.RightToLeft = RightToLeft.Yes;
            currencyGroup.Size = new Size(165, 135);
            currencyGroup.TabIndex = 1;
            currencyGroup.TabStop = false;
            currencyGroup.Text = "العملات";
            // 
            // currencyGrid
            // 
            currencyGrid.AccessibleName = "جدول العملات — لا توجد بيانات محملة";
            currencyGrid.AllowUserToAddRows = false;
            currencyGrid.AllowUserToDeleteRows = false;
            currencyGrid.AllowUserToResizeRows = false;
            currencyGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            currencyGrid.BackgroundColor = Color.FromArgb(246, 247, 238);
            currencyGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            currencyGrid.Columns.AddRange(new DataGridViewColumn[] { currencyCode, currencyName, currencyRate, currencyMinor });
            currencyGrid.Dock = DockStyle.Fill;
            currencyGrid.Location = new Point(5, 24);
            currencyGrid.Margin = new Padding(0);
            currencyGrid.MultiSelect = false;
            currencyGrid.Name = "currencyGrid";
            currencyGrid.ReadOnly = true;
            currencyGrid.RightToLeft = RightToLeft.Yes;
            currencyGrid.RowHeadersVisible = false;
            currencyGrid.RowHeadersWidth = 51;
            currencyGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            currencyGrid.Size = new Size(155, 106);
            currencyGrid.TabIndex = 0;
            // 
            // currencyCode
            // 
            currencyCode.HeaderText = "رمز العملة";
            currencyCode.MinimumWidth = 6;
            currencyCode.Name = "currencyCode";
            currencyCode.ReadOnly = true;
            // 
            // currencyName
            // 
            currencyName.HeaderText = "اسم العملة";
            currencyName.MinimumWidth = 6;
            currencyName.Name = "currencyName";
            currencyName.ReadOnly = true;
            // 
            // currencyRate
            // 
            currencyRate.HeaderText = "سعر التحويل";
            currencyRate.MinimumWidth = 6;
            currencyRate.Name = "currencyRate";
            currencyRate.ReadOnly = true;
            // 
            // currencyMinor
            // 
            currencyMinor.HeaderText = "NmEn";
            currencyMinor.MinimumWidth = 6;
            currencyMinor.Name = "currencyMinor";
            currencyMinor.ReadOnly = true;
            // 
            // pnlMainContent
            // 
            pnlMainContent.BackColor = Color.FromArgb(247, 247, 240);
            pnlMainContent.Controls.Add(workspaceLayout);
            pnlMainContent.Dock = DockStyle.Fill;
            pnlMainContent.Location = new Point(215, 47);
            pnlMainContent.Margin = new Padding(10);
            pnlMainContent.Name = "pnlMainContent";
            pnlMainContent.Padding = new Padding(10);
            pnlMainContent.Size = new Size(590, 618);
            pnlMainContent.TabIndex = 2;
            // 
            // workspaceLayout
            // 
            workspaceLayout.ColumnCount = 1;
            workspaceLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            workspaceLayout.Controls.Add(workspaceSurface, 0, 0);
            workspaceLayout.Controls.Add(lblOpenScreens, 0, 1);
            workspaceLayout.Dock = DockStyle.Fill;
            workspaceLayout.Location = new Point(10, 10);
            workspaceLayout.Margin = new Padding(0);
            workspaceLayout.Name = "workspaceLayout";
            workspaceLayout.RowCount = 2;
            workspaceLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 95F));
            workspaceLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 5F));
            workspaceLayout.Size = new Size(570, 598);
            workspaceLayout.TabIndex = 0;
            // 
            // workspaceSurface
            // 
            workspaceSurface.Dock = DockStyle.Fill;
            workspaceSurface.Location = new Point(0, 0);
            workspaceSurface.Margin = new Padding(0);
            workspaceSurface.Name = "workspaceSurface";
            workspaceSurface.Size = new Size(570, 568);
            workspaceSurface.TabIndex = 0;
            workspaceSurface.Paint += workspaceSurface_Paint;
            // 
            // lblOpenScreens
            // 
            lblOpenScreens.BackColor = Color.FromArgb(237, 238, 227);
            lblOpenScreens.BorderStyle = BorderStyle.FixedSingle;
            lblOpenScreens.Dock = DockStyle.Fill;
            lblOpenScreens.Location = new Point(0, 568);
            lblOpenScreens.Margin = new Padding(0);
            lblOpenScreens.Name = "lblOpenScreens";
            lblOpenScreens.Padding = new Padding(5);
            lblOpenScreens.RightToLeft = RightToLeft.Yes;
            lblOpenScreens.Size = new Size(570, 30);
            lblOpenScreens.TabIndex = 2;
            lblOpenScreens.Text = "لفتح الشاشة: انقر نقراً مزدوجاً أو اضغط Enter";
            lblOpenScreens.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlSystemTree
            // 
            pnlSystemTree.BackColor = Color.FromArgb(20, 26, 143);
            pnlSystemTree.Controls.Add(treeBody);
            pnlSystemTree.Controls.Add(navigationRail);
            pnlSystemTree.Dock = DockStyle.Fill;
            pnlSystemTree.Location = new Point(815, 37);
            pnlSystemTree.Margin = new Padding(0);
            pnlSystemTree.Name = "pnlSystemTree";
            pnlSystemTree.Size = new Size(280, 638);
            pnlSystemTree.TabIndex = 3;
            // 
            // treeBody
            // 
            treeBody.Controls.Add(tvSystemTree);
            treeBody.Controls.Add(treeSearchLayout);
            treeBody.Dock = DockStyle.Fill;
            treeBody.Location = new Point(0, 0);
            treeBody.Margin = new Padding(0);
            treeBody.Name = "treeBody";
            treeBody.Padding = new Padding(0, 0, 1, 0);
            treeBody.Size = new Size(244, 638);
            treeBody.TabIndex = 0;
            // 
            // tvSystemTree
            // 
            tvSystemTree.AccessibleName = "شجرة النظام";
            tvSystemTree.BackColor = Color.FromArgb(247, 247, 235);
            tvSystemTree.BorderStyle = BorderStyle.FixedSingle;
            tvSystemTree.Dock = DockStyle.Fill;
            tvSystemTree.FullRowSelect = true;
            tvSystemTree.HideSelection = false;
            tvSystemTree.HotTracking = true;
            tvSystemTree.ImageIndex = 0;
            tvSystemTree.ImageList = navigationImages;
            tvSystemTree.Indent = 16;
            tvSystemTree.ItemHeight = 24;
            tvSystemTree.Location = new Point(0, 36);
            tvSystemTree.Margin = new Padding(5);
            tvSystemTree.Name = "tvSystemTree";
            tvSystemTree.RightToLeft = RightToLeft.Yes;
            tvSystemTree.RightToLeftLayout = true;
            tvSystemTree.SelectedImageIndex = 0;
            tvSystemTree.ShowLines = false;
            tvSystemTree.ShowNodeToolTips = true;
            tvSystemTree.ShowRootLines = false;
            tvSystemTree.Size = new Size(243, 602);
            tvSystemTree.TabIndex = 0;
            tvSystemTree.AfterSelect += tvSystemTree_AfterSelect;
            // 
            // treeSearchLayout
            // 
            treeSearchLayout.ColumnCount = 2;
            treeSearchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 28F));
            treeSearchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            treeSearchLayout.Controls.Add(lblTreeSearch, 0, 0);
            treeSearchLayout.Controls.Add(txtTreeSearch, 1, 0);
            treeSearchLayout.Dock = DockStyle.Top;
            treeSearchLayout.Location = new Point(0, 0);
            treeSearchLayout.Name = "treeSearchLayout";
            treeSearchLayout.Padding = new Padding(8, 5, 8, 5);
            treeSearchLayout.RightToLeft = RightToLeft.No;
            treeSearchLayout.RowCount = 1;
            treeSearchLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            treeSearchLayout.Size = new Size(243, 36);
            treeSearchLayout.TabIndex = 1;
            // 
            // lblTreeSearch
            // 
            lblTreeSearch.Dock = DockStyle.Fill;
            lblTreeSearch.Font = new Font("Segoe UI Symbol", 17F);
            lblTreeSearch.ForeColor = Color.White;
            lblTreeSearch.Location = new Point(8, 5);
            lblTreeSearch.Margin = new Padding(0);
            lblTreeSearch.Name = "lblTreeSearch";
            lblTreeSearch.RightToLeft = RightToLeft.Yes;
            lblTreeSearch.Size = new Size(28, 26);
            lblTreeSearch.TabIndex = 0;
            lblTreeSearch.Text = "⌕";
            lblTreeSearch.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtTreeSearch
            // 
            txtTreeSearch.AccessibleName = "بحث في شجرة النظام";
            txtTreeSearch.Dock = DockStyle.Fill;
            txtTreeSearch.Location = new Point(36, 5);
            txtTreeSearch.Margin = new Padding(0);
            txtTreeSearch.Name = "txtTreeSearch";
            txtTreeSearch.PlaceholderText = "بحث في الشجرة";
            txtTreeSearch.RightToLeft = RightToLeft.Yes;
            txtTreeSearch.Size = new Size(199, 26);
            txtTreeSearch.TabIndex = 1;
            // 
            // navigationRail
            // 
            navigationRail.BackColor = Color.FromArgb(20, 26, 143);
            navigationRail.Controls.Add(railSymbols);
            navigationRail.Controls.Add(railPower);
            navigationRail.Dock = DockStyle.Right;
            navigationRail.Location = new Point(244, 0);
            navigationRail.Margin = new Padding(0);
            navigationRail.Name = "navigationRail";
            navigationRail.Size = new Size(36, 638);
            navigationRail.TabIndex = 1;
            // 
            // railSymbols
            // 
            railSymbols.Controls.Add(railArrow);
            railSymbols.Controls.Add(railRefresh);
            railSymbols.Controls.Add(railStar);
            railSymbols.Controls.Add(railPlay);
            railSymbols.Controls.Add(railFlag);
            railSymbols.Dock = DockStyle.Top;
            railSymbols.FlowDirection = FlowDirection.TopDown;
            railSymbols.Location = new Point(0, 0);
            railSymbols.Name = "railSymbols";
            railSymbols.Padding = new Padding(0, 8, 0, 0);
            railSymbols.Size = new Size(36, 260);
            railSymbols.TabIndex = 0;
            railSymbols.WrapContents = false;
            // 
            // railArrow
            // 
            railArrow.AccessibleName = "إخفاء شجرة النظام";
            railArrow.BackColor = Color.Transparent;
            railArrow.Cursor = Cursors.Hand;
            railArrow.FlatAppearance.BorderSize = 0;
            railArrow.FlatStyle = FlatStyle.Flat;
            railArrow.Font = new Font("Segoe UI Symbol", 20F);
            railArrow.ForeColor = Color.DeepSkyBlue;
            railArrow.Location = new Point(0, 8);
            railArrow.Margin = new Padding(0);
            railArrow.Name = "railArrow";
            railArrow.RightToLeft = RightToLeft.Yes;
            railArrow.Size = new Size(36, 42);
            railArrow.TabIndex = 0;
            railArrow.Text = "➜";
            railArrow.UseVisualStyleBackColor = false;
            // 
            // railRefresh
            // 
            railRefresh.AccessibleName = "رمز مرجعي";
            railRefresh.BackColor = Color.Transparent;
            railRefresh.Font = new Font("Segoe UI Symbol", 20F);
            railRefresh.ForeColor = Color.LightYellow;
            railRefresh.Location = new Point(0, 50);
            railRefresh.Margin = new Padding(0);
            railRefresh.Name = "railRefresh";
            railRefresh.RightToLeft = RightToLeft.Yes;
            railRefresh.Size = new Size(36, 42);
            railRefresh.TabIndex = 1;
            railRefresh.Text = "↻";
            railRefresh.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // railStar
            // 
            railStar.AccessibleName = "رمز مرجعي";
            railStar.BackColor = Color.Transparent;
            railStar.Font = new Font("Segoe UI Symbol", 20F);
            railStar.ForeColor = Color.Yellow;
            railStar.Location = new Point(0, 92);
            railStar.Margin = new Padding(0);
            railStar.Name = "railStar";
            railStar.RightToLeft = RightToLeft.Yes;
            railStar.Size = new Size(36, 42);
            railStar.TabIndex = 2;
            railStar.Text = "★";
            railStar.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // railPlay
            // 
            railPlay.AccessibleName = "رمز مرجعي";
            railPlay.BackColor = Color.Firebrick;
            railPlay.Font = new Font("Segoe UI Symbol", 20F);
            railPlay.ForeColor = Color.White;
            railPlay.Location = new Point(0, 134);
            railPlay.Margin = new Padding(0);
            railPlay.Name = "railPlay";
            railPlay.RightToLeft = RightToLeft.Yes;
            railPlay.Size = new Size(36, 42);
            railPlay.TabIndex = 3;
            railPlay.Text = "▶";
            railPlay.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // railFlag
            // 
            railFlag.AccessibleName = "رمز مرجعي";
            railFlag.BackColor = Color.Transparent;
            railFlag.Font = new Font("Segoe UI Symbol", 20F);
            railFlag.ForeColor = Color.LightYellow;
            railFlag.Location = new Point(0, 176);
            railFlag.Margin = new Padding(0);
            railFlag.Name = "railFlag";
            railFlag.RightToLeft = RightToLeft.Yes;
            railFlag.Size = new Size(36, 42);
            railFlag.TabIndex = 4;
            railFlag.Text = "⚑";
            railFlag.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // railPower
            // 
            railPower.AccessibleName = "رمز مرجعي";
            railPower.Dock = DockStyle.Bottom;
            railPower.Font = new Font("Segoe UI Symbol", 20F);
            railPower.ForeColor = Color.Red;
            railPower.Location = new Point(0, 598);
            railPower.Margin = new Padding(0);
            railPower.Name = "railPower";
            railPower.RightToLeft = RightToLeft.Yes;
            railPower.Size = new Size(36, 40);
            railPower.TabIndex = 1;
            railPower.Text = "\u23fb";
            railPower.TextAlign = ContentAlignment.MiddleCenter;
            railPower.Click += railPower_Click;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1100, 680);
            Controls.Add(mainLayout);
            Font = new Font("Tahoma", 9F);
            MinimumSize = new Size(920, 560);
            Name = "FrmMain";
            RightToLeftLayout = false;
            Text = "الشاشة الرئيسية";
            WindowState = FormWindowState.Maximized;
            Load += FrmMain_Load;
            mainLayout.ResumeLayout(false);
            tlpCompanyInfo.ResumeLayout(false);
            pnlNotifications.ResumeLayout(false);
            pnlNotifications.PerformLayout();
            leftLayout.ResumeLayout(false);
            notificationBox.ResumeLayout(false);
            favoritesBox.ResumeLayout(false);
            userBox.ResumeLayout(false);
            userInfoLayout.ResumeLayout(false);
            currencyGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)currencyGrid).EndInit();
            pnlMainContent.ResumeLayout(false);
            workspaceLayout.ResumeLayout(false);
            pnlSystemTree.ResumeLayout(false);
            treeBody.ResumeLayout(false);
            treeSearchLayout.ResumeLayout(false);
            treeSearchLayout.PerformLayout();
            navigationRail.ResumeLayout(false);
            railSymbols.ResumeLayout(false);
            ResumeLayout(false);
        }
        #endregion
        private TableLayoutPanel mainLayout;
        private TableLayoutPanel tlpCompanyInfo;
        private Label lblCompanyName;
        private Label lblBranch;
        private Label lblFinancialYear;
        private Label lblCurrentUser;
        private Label lblCurrentDate;
        private Panel pnlNotifications;
        private TableLayoutPanel leftLayout;
        private Label lblCompanyLogo;
        private Button btnRefreshNotifications;
        private Panel notificationBox;
        private Label lblNotificationsTitle;
        private Label lblNotificationSummary;
        private Panel favoritesBox;
        private Label lblFavoritesTitle;
        private ListBox lstDashboardFavorites;
        private Panel userBox;
        private Label lblUserInfoTitle;
        private TableLayoutPanel userInfoLayout;
        private Label lblUserCaption;
        private Label lblUserValue;
        private Label lblBranchCaption;
        private Label lblBranchValue;
        private Label lblCompanyCaption;
        private Label lblCompanyValue;
        private Label lblYearCaption;
        private Label lblYearValue;
        private Label lblTimeCaption;
        private Label lblCurrentTime;
        private Button btnContact;
        private Panel pnlMainContent;
        private TableLayoutPanel workspaceLayout;
        private Panel workspaceSurface;
        private GroupBox currencyGroup;
        private DataGridView currencyGrid;
        private DataGridViewTextBoxColumn currencyCode;
        private DataGridViewTextBoxColumn currencyName;
        private DataGridViewTextBoxColumn currencyRate;
        private DataGridViewTextBoxColumn currencyMinor;
        private Label lblOpenScreens;
        private Panel pnlSystemTree;
        private Panel navigationRail;
        private FlowLayoutPanel railSymbols;
        private Button railArrow;
        private ImageList navigationImages;
        private Label railRefresh;
        private Label railStar;
        private Label railPlay;
        private Label railFlag;
        private Label railPower;
        private Panel treeBody;
        private TableLayoutPanel treeSearchLayout;
        private Label lblTreeSearch;
        private TextBox txtTreeSearch;
        private TreeView tvSystemTree;
    }
}
