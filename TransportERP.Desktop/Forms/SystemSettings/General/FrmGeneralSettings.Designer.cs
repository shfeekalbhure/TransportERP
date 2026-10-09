#nullable enable

using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

partial class UcGeneralSettings
{
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;

    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
    private FlowLayoutPanel designerCommandFlow = null!;
    private Panel designerCloseHost = null!;
    private Button standardCommandEdit = null!;
    private Button standardCommandDelete = null!;
    private Button standardCommandCancel = null!;
    private Button standardCommandView = null!;
    private Button standardCommandLast = null!;
    private Button standardCommandNext = null!;
    private Button standardCommandPrevious = null!;
    private Button standardCommandFirst = null!;
    private Button standardCommandPrint = null!;
    private Button standardCommandImport = null!;
    private Button standardCommandExport = null!;
    private Button standardCommandHelp = null!;

    private System.ComponentModel.IContainer? components;
    private Panel pnlHeader = null!;
    private Label lblTitle = null!;
    private Label lblSubtitle = null!;
    private TabControl tabSettings = null!;
    private TabPage tabCompanyScope = null!;
    private TabPage tabAppearance = null!;
    private TabPage tabNotifications = null!;
    private TabPage tabPlatformOffline = null!;
    private GroupBox grpCompanyScope = null!;
    private TableLayoutPanel tlpCompanyScope = null!;
    private Label lblCompanyGroup = null!;
    private ComboBox cboCompanyGroup = null!;
    private Label lblCompany = null!;
    private ComboBox cboCompany = null!;
    private Label lblBranchGroup = null!;
    private ComboBox cboBranchGroup = null!;
    private Label lblBranch = null!;
    private ComboBox cboBranch = null!;
    private Label lblCompanyVisibility = null!;
    private ComboBox cboCompanyVisibility = null!;
    private Label lblBranchVisibility = null!;
    private ComboBox cboBranchVisibility = null!;
    private CheckBox chkCompanyEnabled = null!;
    private CheckBox chkBranchEnabled = null!;
    private GroupBox grpGeneralDefaults = null!;
    private TableLayoutPanel tlpGeneralDefaults = null!;
    private Label lblCurrency = null!;
    private ComboBox cboDefaultCurrency = null!;
    private Label lblLanguage = null!;
    private ComboBox cboDefaultLanguage = null!;
    private Label lblDateFormat = null!;
    private ComboBox cboDateFormat = null!;
    private Label lblSessionTimeout = null!;
    private NumericUpDown nudSessionTimeout = null!;
    private GroupBox grpAppearance = null!;
    private TableLayoutPanel tlpAppearance = null!;
    private Label lblTheme = null!;
    private ComboBox cboTheme = null!;
    private Label lblDensity = null!;
    private ComboBox cboDensity = null!;
    private CheckBox chkShowDashboardAtStartup = null!;
    private CheckBox chkConfirmBeforeClose = null!;
    private CheckBox chkRememberLastScope = null!;
    private GroupBox grpNotifications = null!;
    private FlowLayoutPanel flpNotifications = null!;
    private CheckBox chkEnableDesktopAlerts = null!;
    private CheckBox chkEnableSoundAlerts = null!;
    private CheckBox chkEnableEmailAlerts = null!;
    private CheckBox chkEnableApprovalAlerts = null!;
    private GroupBox grpPlatform = null!;
    private TableLayoutPanel tlpPlatform = null!;
    private Label lblPlatformMode = null!;
    private ComboBox cboPlatformMode = null!;
    private Label lblOfflineMode = null!;
    private ComboBox cboOfflineMode = null!;
    private Label lblSyncPolicy = null!;
    private ComboBox cboSyncPolicy = null!;
    private CheckBox chkAllowOfflineRead = null!;
    private CheckBox chkSyncOnReconnect = null!;
    private Button btnModuleActivation = null!;
    private Panel pnlActions = null!;
    private FlowLayoutPanel flpActions = null!;
    private Button btnNew = null!;
    private Button btnValidate = null!;
    private Button btnPublish = null!;
    private Button btnRevertScope = null!;
    private Button btnViewAudit = null!;
    private Button btnRefresh = null!;
    private FlowLayoutPanel flpClose = null!;
    private Panel pnlScopeSpacing = null!;
    private Button btnSave = null!;
    private Button btnReset = null!;
    private Button btnClose = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null) components.Dispose();
        base.Dispose(disposing);
    }

    private TableLayoutPanel tlpAuditInfo = null!;
    private Label lblPrintCount = null!;
    private Label lblLastPrintedAt = null!;
    private Label lblEditCount = null!;
    private Label lblModifiedAt = null!;
    private Label lblModifiedBy = null!;
    private Label lblCreatedAt = null!;
    private Label lblCreatedBy = null!;

    private TabPage tabOperation = null!;
    private TabPage tabLanguage = null!;
    private TabPage tabPrintReports = null!;
    private TabPage tabChangeHistory = null!;
    private TableLayoutPanel languageFields = null!;
    private UcPrintSettings printSettingsContent = null!;
    private DataGridView changeHistory = null!;
    private Label historyState = null!;
    private void InitializeComponent()
    {
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        designerCommandFlow = new FlowLayoutPanel();
        designerCloseHost = new Panel();
        standardCommandEdit = new Button();
        standardCommandDelete = new Button();
        standardCommandCancel = new Button();
        standardCommandView = new Button();
        standardCommandLast = new Button();
        standardCommandNext = new Button();
        standardCommandPrevious = new Button();
        standardCommandFirst = new Button();
        standardCommandPrint = new Button();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
        tabOperation = new TabPage();
        tabLanguage = new TabPage();
        tabPrintReports = new TabPage();
        tabChangeHistory = new TabPage();
        languageFields = new TableLayoutPanel();
        printSettingsContent = new UcPrintSettings();
        changeHistory = new DataGridView();
        historyState = new Label();

        pnlHeader = new Panel();
        lblSubtitle = new Label();
        lblTitle = new Label();
        tabSettings = new TabControl();
        tabCompanyScope = new TabPage();
        grpGeneralDefaults = new GroupBox();
        tlpGeneralDefaults = new TableLayoutPanel();
        lblCurrency = new Label();
        cboDefaultCurrency = new ComboBox();
        lblLanguage = new Label();
        cboDefaultLanguage = new ComboBox();
        lblDateFormat = new Label();
        cboDateFormat = new ComboBox();
        lblSessionTimeout = new Label();
        nudSessionTimeout = new NumericUpDown();
        pnlScopeSpacing = new Panel();
        grpCompanyScope = new GroupBox();
        tlpCompanyScope = new TableLayoutPanel();
        lblCompanyGroup = new Label();
        cboCompanyGroup = new ComboBox();
        lblCompany = new Label();
        cboCompany = new ComboBox();
        lblBranchGroup = new Label();
        cboBranchGroup = new ComboBox();
        lblBranch = new Label();
        cboBranch = new ComboBox();
        lblCompanyVisibility = new Label();
        cboCompanyVisibility = new ComboBox();
        lblBranchVisibility = new Label();
        cboBranchVisibility = new ComboBox();
        chkCompanyEnabled = new CheckBox();
        chkBranchEnabled = new CheckBox();
        tabAppearance = new TabPage();
        grpAppearance = new GroupBox();
        tlpAppearance = new TableLayoutPanel();
        lblTheme = new Label();
        cboTheme = new ComboBox();
        lblDensity = new Label();
        cboDensity = new ComboBox();
        chkShowDashboardAtStartup = new CheckBox();
        chkConfirmBeforeClose = new CheckBox();
        chkRememberLastScope = new CheckBox();
        btnReset = new Button();
        tabNotifications = new TabPage();
        grpNotifications = new GroupBox();
        flpNotifications = new FlowLayoutPanel();
        chkEnableDesktopAlerts = new CheckBox();
        chkEnableSoundAlerts = new CheckBox();
        chkEnableEmailAlerts = new CheckBox();
        chkEnableApprovalAlerts = new CheckBox();
        tabPlatformOffline = new TabPage();
        grpPlatform = new GroupBox();
        tlpPlatform = new TableLayoutPanel();
        lblPlatformMode = new Label();
        cboPlatformMode = new ComboBox();
        lblOfflineMode = new Label();
        cboOfflineMode = new ComboBox();
        lblSyncPolicy = new Label();
        cboSyncPolicy = new ComboBox();
        chkAllowOfflineRead = new CheckBox();
        chkSyncOnReconnect = new CheckBox();
        btnModuleActivation = new Button();
        pnlActions = new Panel();
        flpActions = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnValidate = new Button();
        btnPublish = new Button();
        btnRevertScope = new Button();
        btnViewAudit = new Button();
        btnRefresh = new Button();
        flpClose = new FlowLayoutPanel();
        btnClose = new Button();
        pnlHeader.SuspendLayout();
        tabSettings.SuspendLayout();
        tabCompanyScope.SuspendLayout();
        grpGeneralDefaults.SuspendLayout();
        tlpGeneralDefaults.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudSessionTimeout).BeginInit();
        grpCompanyScope.SuspendLayout();
        tlpCompanyScope.SuspendLayout();
        tabAppearance.SuspendLayout();
        grpAppearance.SuspendLayout();
        tlpAppearance.SuspendLayout();
        tabNotifications.SuspendLayout();
        grpNotifications.SuspendLayout();
        flpNotifications.SuspendLayout();
        tabPlatformOffline.SuspendLayout();
        grpPlatform.SuspendLayout();
        tlpPlatform.SuspendLayout();
        pnlActions.SuspendLayout();
        flpActions.SuspendLayout();
        flpClose.SuspendLayout();
        tlpAuditInfo = new TableLayoutPanel();
        lblPrintCount = new Label();
        lblLastPrintedAt = new Label();
        lblEditCount = new Label();
        lblModifiedAt = new Label();
        lblModifiedBy = new Label();
        lblCreatedAt = new Label();
        lblCreatedBy = new Label();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.Controls.Add(lblSubtitle);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Margin = new Padding(3, 4, 3, 4);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1180, 88);
        pnlHeader.TabIndex = 0;
        // 
        // lblSubtitle
        // 
        lblSubtitle.Dock = DockStyle.Fill;
        lblSubtitle.Location = new Point(0, 40);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(1180, 48);
        lblSubtitle.TabIndex = 0;
        lblSubtitle.Text = "نطاق الشركة والفرع والمظهر والإشعارات وسياسات المنصة والعمل دون اتصال.";
        // 
        // lblTitle
        // 
        lblTitle.Dock = DockStyle.Top;
        lblTitle.Location = new Point(0, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(1180, 40);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "الإعدادات العامة";
        // 
        // tabSettings
        // 
        tabSettings.Dock = DockStyle.Fill;
        tabSettings.Location = new Point(0, 88);
        tabSettings.Margin = new Padding(3, 4, 3, 4);
        tabSettings.Name = "tabSettings";
        tabSettings.RightToLeft = RightToLeft.Yes;
        tabSettings.RightToLeftLayout = true;
        tabSettings.SelectedIndex = 0;
        tabSettings.Size = new Size(1180, 608);
        tabSettings.TabIndex = 1;
        // 
        // tabCompanyScope
        // 
        tabCompanyScope.AutoScroll = true;
        tabCompanyScope.BackColor = Color.LightCyan;
        tabOperation.Controls.Add(grpGeneralDefaults);
        tabCompanyScope.Controls.Add(pnlScopeSpacing);
        tabCompanyScope.Controls.Add(grpCompanyScope);
        tabCompanyScope.Location = new Point(4, 32);
        tabCompanyScope.Margin = new Padding(3, 4, 3, 4);
        tabCompanyScope.Name = "tabCompanyScope";
        tabCompanyScope.Padding = new Padding(16);
        tabCompanyScope.Size = new Size(1172, 572);
        tabCompanyScope.Text = "الشركات والفروع";
        // 
        // grpGeneralDefaults
        // 
        grpGeneralDefaults.Controls.Add(tlpGeneralDefaults);
        grpGeneralDefaults.Dock = DockStyle.Top;
        grpGeneralDefaults.Location = new Point(16, 328);
        grpGeneralDefaults.Margin = new Padding(0, 16, 0, 0);
        grpGeneralDefaults.Name = "grpGeneralDefaults";
        grpGeneralDefaults.Padding = new Padding(3, 4, 3, 4);
        grpGeneralDefaults.Size = new Size(1140, 185);
        grpGeneralDefaults.TabIndex = 1;
        grpGeneralDefaults.TabStop = false;
        grpGeneralDefaults.Text = "الافتراضيات العامة";
        // 
        // tlpGeneralDefaults
        // 
        tlpGeneralDefaults.ColumnCount = 4;
        tlpGeneralDefaults.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 176F));
        tlpGeneralDefaults.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpGeneralDefaults.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 176F));
        tlpGeneralDefaults.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpGeneralDefaults.Controls.Add(lblCurrency, 0, 0);
        tlpGeneralDefaults.Controls.Add(cboDefaultCurrency, 1, 0);
        tlpGeneralDefaults.Controls.Add(lblSessionTimeout, 0, 1);
        tlpGeneralDefaults.Controls.Add(nudSessionTimeout, 1, 1);
        tlpGeneralDefaults.Dock = DockStyle.Fill;
        tlpGeneralDefaults.Location = new Point(3, 27);
        tlpGeneralDefaults.Margin = new Padding(3, 4, 3, 4);
        tlpGeneralDefaults.Name = "tlpGeneralDefaults";
        tlpGeneralDefaults.RowCount = 2;
        tlpGeneralDefaults.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        tlpGeneralDefaults.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        tlpGeneralDefaults.Size = new Size(1134, 154);
        tlpGeneralDefaults.TabIndex = 0;
        // 
        // lblCurrency
        // 
        lblCurrency.Dock = DockStyle.Fill;
        lblCurrency.Location = new Point(961, 0);
        lblCurrency.Name = "lblCurrency";
        lblCurrency.Size = new Size(170, 56);
        lblCurrency.TabIndex = 0;
        lblCurrency.Text = "العملة الافتراضية";
        // 
        // cboDefaultCurrency
        // 
        cboDefaultCurrency.Dock = DockStyle.Fill;
        cboDefaultCurrency.DropDownStyle = ComboBoxStyle.DropDownList;
        cboDefaultCurrency.Location = new Point(570, 4);
        cboDefaultCurrency.Margin = new Padding(3);
        cboDefaultCurrency.Name = "cboDefaultCurrency";
        cboDefaultCurrency.Size = new Size(385, 31);
        cboDefaultCurrency.TabIndex = 8;
        // 
        // lblLanguage
        // 
        lblLanguage.Dock = DockStyle.Fill;
        lblLanguage.Location = new Point(394, 0);
        lblLanguage.Name = "lblLanguage";
        lblLanguage.Size = new Size(170, 56);
        lblLanguage.TabIndex = 9;
        lblLanguage.Text = "اللغة الافتراضية";
        // 
        // cboDefaultLanguage
        // 
        cboDefaultLanguage.Dock = DockStyle.Fill;
        cboDefaultLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
        cboDefaultLanguage.Location = new Point(3, 4);
        cboDefaultLanguage.Margin = new Padding(3);
        cboDefaultLanguage.Name = "cboDefaultLanguage";
        cboDefaultLanguage.Size = new Size(385, 31);
        cboDefaultLanguage.TabIndex = 9;
        // 
        // lblDateFormat
        // 
        lblDateFormat.Dock = DockStyle.Fill;
        lblDateFormat.Location = new Point(961, 56);
        lblDateFormat.Name = "lblDateFormat";
        lblDateFormat.Size = new Size(170, 98);
        lblDateFormat.TabIndex = 10;
        lblDateFormat.Text = "تنسيق التاريخ";
        // 
        // cboDateFormat
        // 
        cboDateFormat.Dock = DockStyle.Fill;
        cboDateFormat.DropDownStyle = ComboBoxStyle.DropDownList;
        cboDateFormat.Location = new Point(570, 60);
        cboDateFormat.Margin = new Padding(3);
        cboDateFormat.Name = "cboDateFormat";
        cboDateFormat.Size = new Size(385, 31);
        cboDateFormat.TabIndex = 10;
        // 
        // lblSessionTimeout
        // 
        lblSessionTimeout.Dock = DockStyle.Fill;
        lblSessionTimeout.Location = new Point(394, 56);
        lblSessionTimeout.Name = "lblSessionTimeout";
        lblSessionTimeout.Size = new Size(170, 98);
        lblSessionTimeout.TabIndex = 11;
        lblSessionTimeout.Text = "مهلة الجلسة (دقيقة)";
        // 
        // nudSessionTimeout
        // 
        nudSessionTimeout.Dock = DockStyle.Fill;
        nudSessionTimeout.Location = new Point(3, 60);
        nudSessionTimeout.Margin = new Padding(3);
        nudSessionTimeout.Maximum = new decimal(new int[] { 240, 0, 0, 0 });
        nudSessionTimeout.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
        nudSessionTimeout.Name = "nudSessionTimeout";
        nudSessionTimeout.Size = new Size(385, 30);
        nudSessionTimeout.TabIndex = 11;
        nudSessionTimeout.Value = new decimal(new int[] { 30, 0, 0, 0 });
        // 
        // pnlScopeSpacing
        // 
        pnlScopeSpacing.Dock = DockStyle.Top;
        pnlScopeSpacing.Location = new Point(16, 316);
        pnlScopeSpacing.Name = "pnlScopeSpacing";
        pnlScopeSpacing.Size = new Size(1140, 12);
        pnlScopeSpacing.TabIndex = 2;
        // 
        // grpCompanyScope
        // 
        grpCompanyScope.Controls.Add(tlpCompanyScope);
        grpCompanyScope.Dock = DockStyle.Top;
        grpCompanyScope.Location = new Point(16, 16);
        grpCompanyScope.Margin = new Padding(3, 4, 3, 4);
        grpCompanyScope.Name = "grpCompanyScope";
        grpCompanyScope.Padding = new Padding(3, 4, 3, 4);
        grpCompanyScope.Size = new Size(1140, 300);
        grpCompanyScope.TabIndex = 0;
        grpCompanyScope.TabStop = false;
        grpCompanyScope.Text = "نطاق الشركة والفرع";
        // 
        // tlpCompanyScope
        // 
        tlpCompanyScope.ColumnCount = 4;
        tlpCompanyScope.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 176F));
        tlpCompanyScope.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpCompanyScope.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 176F));
        tlpCompanyScope.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpCompanyScope.Controls.Add(lblCompanyGroup, 0, 0);
        tlpCompanyScope.Controls.Add(cboCompanyGroup, 1, 0);
        tlpCompanyScope.Controls.Add(lblCompany, 2, 0);
        tlpCompanyScope.Controls.Add(cboCompany, 3, 0);
        tlpCompanyScope.Controls.Add(lblBranchGroup, 0, 1);
        tlpCompanyScope.Controls.Add(cboBranchGroup, 1, 1);
        tlpCompanyScope.Controls.Add(lblBranch, 2, 1);
        tlpCompanyScope.Controls.Add(cboBranch, 3, 1);
        tlpCompanyScope.Controls.Add(lblCompanyVisibility, 0, 2);
        tlpCompanyScope.Controls.Add(cboCompanyVisibility, 1, 2);
        tlpCompanyScope.Controls.Add(lblBranchVisibility, 2, 2);
        tlpCompanyScope.Controls.Add(cboBranchVisibility, 3, 2);
        tlpCompanyScope.Controls.Add(chkCompanyEnabled, 1, 3);
        tlpCompanyScope.Controls.Add(chkBranchEnabled, 3, 3);
        tlpCompanyScope.Dock = DockStyle.Fill;
        tlpCompanyScope.Location = new Point(3, 27);
        tlpCompanyScope.Margin = new Padding(3, 4, 3, 4);
        tlpCompanyScope.Name = "tlpCompanyScope";
        tlpCompanyScope.RowCount = 4;
        tlpCompanyScope.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        tlpCompanyScope.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        tlpCompanyScope.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        tlpCompanyScope.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        tlpCompanyScope.Size = new Size(1134, 269);
        tlpCompanyScope.TabIndex = 0;
        // 
        // lblCompanyGroup
        // 
        lblCompanyGroup.Dock = DockStyle.Fill;
        lblCompanyGroup.Location = new Point(961, 0);
        lblCompanyGroup.Name = "lblCompanyGroup";
        lblCompanyGroup.Size = new Size(170, 56);
        lblCompanyGroup.TabIndex = 0;
        lblCompanyGroup.Text = "مجموعة الشركات *";
        // 
        // cboCompanyGroup
        // 
        cboCompanyGroup.Dock = DockStyle.Fill;
        cboCompanyGroup.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCompanyGroup.Location = new Point(570, 4);
        cboCompanyGroup.Margin = new Padding(3);
        cboCompanyGroup.Name = "cboCompanyGroup";
        cboCompanyGroup.Size = new Size(385, 31);
        cboCompanyGroup.TabIndex = 0;
        // 
        // lblCompany
        // 
        lblCompany.Dock = DockStyle.Fill;
        lblCompany.Location = new Point(394, 0);
        lblCompany.Name = "lblCompany";
        lblCompany.Size = new Size(170, 56);
        lblCompany.TabIndex = 1;
        lblCompany.Text = "الشركة *";
        // 
        // cboCompany
        // 
        cboCompany.Dock = DockStyle.Fill;
        cboCompany.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCompany.Location = new Point(3, 4);
        cboCompany.Margin = new Padding(3);
        cboCompany.Name = "cboCompany";
        cboCompany.Size = new Size(385, 31);
        cboCompany.TabIndex = 1;
        // 
        // lblBranchGroup
        // 
        lblBranchGroup.Dock = DockStyle.Fill;
        lblBranchGroup.Location = new Point(961, 56);
        lblBranchGroup.Name = "lblBranchGroup";
        lblBranchGroup.Size = new Size(170, 56);
        lblBranchGroup.TabIndex = 2;
        lblBranchGroup.Text = "مجموعة الفروع";
        // 
        // cboBranchGroup
        // 
        cboBranchGroup.Dock = DockStyle.Fill;
        cboBranchGroup.DropDownStyle = ComboBoxStyle.DropDownList;
        cboBranchGroup.Location = new Point(570, 60);
        cboBranchGroup.Margin = new Padding(3);
        cboBranchGroup.Name = "cboBranchGroup";
        cboBranchGroup.Size = new Size(385, 31);
        cboBranchGroup.TabIndex = 2;
        // 
        // lblBranch
        // 
        lblBranch.Dock = DockStyle.Fill;
        lblBranch.Location = new Point(394, 56);
        lblBranch.Name = "lblBranch";
        lblBranch.Size = new Size(170, 56);
        lblBranch.TabIndex = 3;
        lblBranch.Text = "الفرع *";
        // 
        // cboBranch
        // 
        cboBranch.Dock = DockStyle.Fill;
        cboBranch.DropDownStyle = ComboBoxStyle.DropDownList;
        cboBranch.Location = new Point(3, 60);
        cboBranch.Margin = new Padding(3);
        cboBranch.Name = "cboBranch";
        cboBranch.Size = new Size(385, 31);
        cboBranch.TabIndex = 3;
        // 
        // lblCompanyVisibility
        // 
        lblCompanyVisibility.Dock = DockStyle.Fill;
        lblCompanyVisibility.Location = new Point(961, 112);
        lblCompanyVisibility.Name = "lblCompanyVisibility";
        lblCompanyVisibility.Size = new Size(170, 56);
        lblCompanyVisibility.TabIndex = 4;
        lblCompanyVisibility.Text = "سلوك اختيار الشركة";
        // 
        // cboCompanyVisibility
        // 
        cboCompanyVisibility.Dock = DockStyle.Fill;
        cboCompanyVisibility.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCompanyVisibility.Items.AddRange(new object[] { "AUTO", "SHOW", "HIDE" });
        cboCompanyVisibility.Location = new Point(570, 116);
        cboCompanyVisibility.Margin = new Padding(3);
        cboCompanyVisibility.Name = "cboCompanyVisibility";
        cboCompanyVisibility.Size = new Size(385, 31);
        cboCompanyVisibility.TabIndex = 4;
        // 
        // lblBranchVisibility
        // 
        lblBranchVisibility.Dock = DockStyle.Fill;
        lblBranchVisibility.Location = new Point(394, 112);
        lblBranchVisibility.Name = "lblBranchVisibility";
        lblBranchVisibility.Size = new Size(170, 56);
        lblBranchVisibility.TabIndex = 5;
        lblBranchVisibility.Text = "سلوك اختيار الفرع";
        // 
        // cboBranchVisibility
        // 
        cboBranchVisibility.Dock = DockStyle.Fill;
        cboBranchVisibility.DropDownStyle = ComboBoxStyle.DropDownList;
        cboBranchVisibility.Items.AddRange(new object[] { "AUTO", "SHOW", "HIDE" });
        cboBranchVisibility.Location = new Point(3, 116);
        cboBranchVisibility.Margin = new Padding(3);
        cboBranchVisibility.Name = "cboBranchVisibility";
        cboBranchVisibility.Size = new Size(385, 31);
        cboBranchVisibility.TabIndex = 5;
        // 
        // chkCompanyEnabled
        // 
        chkCompanyEnabled.AutoSize = true;
        chkCompanyEnabled.Checked = true;
        chkCompanyEnabled.CheckState = CheckState.Checked;
        chkCompanyEnabled.Location = new Point(799, 172);
        chkCompanyEnabled.Margin = new Padding(3);
        chkCompanyEnabled.Name = "chkCompanyEnabled";
        chkCompanyEnabled.Size = new Size(156, 27);
        chkCompanyEnabled.TabIndex = 6;
        chkCompanyEnabled.Text = "الشركة ENABLED";
        // 
        // chkBranchEnabled
        // 
        chkBranchEnabled.AutoSize = true;
        chkBranchEnabled.Checked = true;
        chkBranchEnabled.CheckState = CheckState.Checked;
        chkBranchEnabled.Location = new Point(243, 172);
        chkBranchEnabled.Margin = new Padding(3);
        chkBranchEnabled.Name = "chkBranchEnabled";
        chkBranchEnabled.Size = new Size(145, 27);
        chkBranchEnabled.TabIndex = 7;
        chkBranchEnabled.Text = "الفرع ENABLED";
        // 
        // tabAppearance
        // 
        tabAppearance.BackColor = Color.LightCyan;
        tabAppearance.Controls.Add(grpAppearance);
        tabAppearance.Location = new Point(4, 32);
        tabAppearance.Margin = new Padding(3, 4, 3, 4);
        tabAppearance.Name = "tabAppearance";
        tabAppearance.Padding = new Padding(16);
        tabAppearance.Size = new Size(1172, 572);
        tabAppearance.Text = "المظهر والتفضيلات";
        // 
        // grpAppearance
        // 
        grpAppearance.Controls.Add(tlpAppearance);
        grpAppearance.Dock = DockStyle.Top;
        grpAppearance.Location = new Point(16, 16);
        grpAppearance.Margin = new Padding(3, 4, 3, 4);
        grpAppearance.Name = "grpAppearance";
        grpAppearance.Padding = new Padding(3, 4, 3, 4);
        grpAppearance.Size = new Size(1140, 372);
        grpAppearance.TabIndex = 0;
        grpAppearance.TabStop = false;
        grpAppearance.Text = "Theme / Appearance / User Preferences";
        // 
        // tlpAppearance
        // 
        tlpAppearance.ColumnCount = 2;
        tlpAppearance.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
        tlpAppearance.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpAppearance.Controls.Add(lblTheme, 0, 0);
        tlpAppearance.Controls.Add(cboTheme, 1, 0);
        tlpAppearance.Controls.Add(lblDensity, 0, 1);
        tlpAppearance.Controls.Add(cboDensity, 1, 1);
        tlpAppearance.Controls.Add(chkShowDashboardAtStartup, 1, 2);
        tlpAppearance.Controls.Add(chkConfirmBeforeClose, 1, 3);
        tlpAppearance.Controls.Add(chkRememberLastScope, 1, 4);
        tlpAppearance.Controls.Add(btnReset, 1, 5);
        tlpAppearance.Dock = DockStyle.Fill;
        tlpAppearance.Location = new Point(3, 27);
        tlpAppearance.Margin = new Padding(3, 4, 3, 4);
        tlpAppearance.Name = "tlpAppearance";
        tlpAppearance.RowCount = 6;
        tlpAppearance.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tlpAppearance.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tlpAppearance.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tlpAppearance.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tlpAppearance.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tlpAppearance.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tlpAppearance.Size = new Size(1134, 341);
        tlpAppearance.TabIndex = 0;
        // 
        // lblTheme
        // 
        lblTheme.Dock = DockStyle.Fill;
        lblTheme.Location = new Point(917, 0);
        lblTheme.Name = "lblTheme";
        lblTheme.Size = new Size(214, 50);
        lblTheme.TabIndex = 0;
        lblTheme.Text = "السمة";
        // 
        // cboTheme
        // 
        cboTheme.Dock = DockStyle.Fill;
        cboTheme.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTheme.Items.AddRange(new object[] { "فاتح", "داكن", "تلقائي" });
        cboTheme.Location = new Point(3, 4);
        cboTheme.Margin = new Padding(3);
        cboTheme.Name = "cboTheme";
        cboTheme.Size = new Size(908, 31);
        cboTheme.TabIndex = 0;
        // 
        // lblDensity
        // 
        lblDensity.Dock = DockStyle.Fill;
        lblDensity.Location = new Point(917, 50);
        lblDensity.Name = "lblDensity";
        lblDensity.Size = new Size(214, 50);
        lblDensity.TabIndex = 1;
        lblDensity.Text = "كثافة العرض";
        // 
        // cboDensity
        // 
        cboDensity.Dock = DockStyle.Fill;
        cboDensity.DropDownStyle = ComboBoxStyle.DropDownList;
        cboDensity.Items.AddRange(new object[] { "COMFORTABLE", "COMPACT" });
        cboDensity.Location = new Point(3, 54);
        cboDensity.Margin = new Padding(3);
        cboDensity.Name = "cboDensity";
        cboDensity.Size = new Size(908, 31);
        cboDensity.TabIndex = 1;
        // 
        // chkShowDashboardAtStartup
        // 
        chkShowDashboardAtStartup.AutoSize = true;
        chkShowDashboardAtStartup.Location = new Point(615, 104);
        chkShowDashboardAtStartup.Margin = new Padding(3);
        chkShowDashboardAtStartup.Name = "chkShowDashboardAtStartup";
        chkShowDashboardAtStartup.Size = new Size(296, 27);
        chkShowDashboardAtStartup.TabIndex = 2;
        chkShowDashboardAtStartup.Text = "عرض لوحة التحكم بعد تسجيل الدخول";
        // 
        // chkConfirmBeforeClose
        // 
        chkConfirmBeforeClose.AutoSize = true;
        chkConfirmBeforeClose.Location = new Point(716, 154);
        chkConfirmBeforeClose.Margin = new Padding(3);
        chkConfirmBeforeClose.Name = "chkConfirmBeforeClose";
        chkConfirmBeforeClose.Size = new Size(195, 27);
        chkConfirmBeforeClose.TabIndex = 3;
        chkConfirmBeforeClose.Text = "تأكيد قبل إغلاق الشاشة";
        // 
        // chkRememberLastScope
        // 
        chkRememberLastScope.AutoSize = true;
        chkRememberLastScope.Location = new Point(664, 204);
        chkRememberLastScope.Margin = new Padding(3);
        chkRememberLastScope.Name = "chkRememberLastScope";
        chkRememberLastScope.Size = new Size(247, 27);
        chkRememberLastScope.TabIndex = 4;
        chkRememberLastScope.Text = "تذكر آخر شركة وفرع للمستخدم";
        // 
        // btnReset
        // 
        btnReset.Location = new Point(771, 254);
        btnReset.Margin = new Padding(3, 4, 3, 4);
        btnReset.Name = "btnReset";
        btnReset.Size = new Size(140, 38);
        btnReset.TabIndex = 1;
        btnReset.Text = "استعادة الافتراضي";
        // 
        // tabNotifications
        // 
        tabNotifications.BackColor = Color.LightCyan;
        tabNotifications.Controls.Add(grpNotifications);
        tabNotifications.Location = new Point(4, 32);
        tabNotifications.Margin = new Padding(3, 4, 3, 4);
        tabNotifications.Name = "tabNotifications";
        tabNotifications.Padding = new Padding(16);
        tabNotifications.Size = new Size(1172, 572);
        tabNotifications.Text = "الإشعارات";
        // 
        // grpNotifications
        // 
        grpNotifications.Controls.Add(flpNotifications);
        grpNotifications.Dock = DockStyle.Top;
        grpNotifications.Location = new Point(16, 16);
        grpNotifications.Margin = new Padding(3, 4, 3, 4);
        grpNotifications.Name = "grpNotifications";
        grpNotifications.Padding = new Padding(3, 4, 3, 4);
        grpNotifications.Size = new Size(1140, 260);
        grpNotifications.TabIndex = 0;
        grpNotifications.TabStop = false;
        grpNotifications.Text = "قنوات التنبيه";
        // 
        // flpNotifications
        // 
        flpNotifications.Controls.Add(chkEnableDesktopAlerts);
        flpNotifications.Controls.Add(chkEnableSoundAlerts);
        flpNotifications.Controls.Add(chkEnableEmailAlerts);
        flpNotifications.Controls.Add(chkEnableApprovalAlerts);
        flpNotifications.Dock = DockStyle.Fill;
        flpNotifications.FlowDirection = FlowDirection.TopDown;
        flpNotifications.Location = new Point(3, 27);
        flpNotifications.Margin = new Padding(3, 4, 3, 4);
        flpNotifications.Name = "flpNotifications";
        flpNotifications.Padding = new Padding(18, 21, 18, 21);
        flpNotifications.Size = new Size(1134, 229);
        flpNotifications.TabIndex = 0;
        flpNotifications.WrapContents = false;
        // 
        // chkEnableDesktopAlerts
        // 
        chkEnableDesktopAlerts.AutoSize = true;
        chkEnableDesktopAlerts.Location = new Point(911, 25);
        chkEnableDesktopAlerts.Margin = new Padding(3, 4, 3, 4);
        chkEnableDesktopAlerts.Name = "chkEnableDesktopAlerts";
        chkEnableDesktopAlerts.Size = new Size(184, 27);
        chkEnableDesktopAlerts.TabIndex = 0;
        chkEnableDesktopAlerts.Text = "تنبيهات سطح المكتب";
        // 
        // chkEnableSoundAlerts
        // 
        chkEnableSoundAlerts.AutoSize = true;
        chkEnableSoundAlerts.Location = new Point(959, 60);
        chkEnableSoundAlerts.Margin = new Padding(3, 4, 3, 4);
        chkEnableSoundAlerts.Name = "chkEnableSoundAlerts";
        chkEnableSoundAlerts.Size = new Size(136, 27);
        chkEnableSoundAlerts.TabIndex = 1;
        chkEnableSoundAlerts.Text = "التنبيه الصوتي";
        // 
        // chkEnableEmailAlerts
        // 
        chkEnableEmailAlerts.AutoSize = true;
        chkEnableEmailAlerts.Location = new Point(891, 95);
        chkEnableEmailAlerts.Margin = new Padding(3, 4, 3, 4);
        chkEnableEmailAlerts.Name = "chkEnableEmailAlerts";
        chkEnableEmailAlerts.Size = new Size(204, 27);
        chkEnableEmailAlerts.TabIndex = 2;
        chkEnableEmailAlerts.Text = "تنبيهات البريد الإلكتروني";
        // 
        // chkEnableApprovalAlerts
        // 
        chkEnableApprovalAlerts.AutoSize = true;
        chkEnableApprovalAlerts.Location = new Point(902, 130);
        chkEnableApprovalAlerts.Margin = new Padding(3, 4, 3, 4);
        chkEnableApprovalAlerts.Name = "chkEnableApprovalAlerts";
        chkEnableApprovalAlerts.Size = new Size(193, 27);
        chkEnableApprovalAlerts.TabIndex = 3;
        chkEnableApprovalAlerts.Text = "تنبيهات الاعتماد والنشر";
        // 
        // tabPlatformOffline
        // 
        tabPlatformOffline.BackColor = Color.LightCyan;
        tabPlatformOffline.Controls.Add(grpPlatform);
        tabPlatformOffline.Location = new Point(4, 32);
        tabPlatformOffline.Margin = new Padding(3, 4, 3, 4);
        tabPlatformOffline.Name = "tabPlatformOffline";
        tabPlatformOffline.Padding = new Padding(16);
        tabPlatformOffline.Size = new Size(1172, 572);
        tabPlatformOffline.Text = "المنصة والمزامنة";
        // 
        // grpPlatform
        // 
        grpPlatform.Controls.Add(tlpPlatform);
        grpPlatform.Dock = DockStyle.Top;
        grpPlatform.Location = new Point(16, 16);
        grpPlatform.Margin = new Padding(3, 4, 3, 4);
        grpPlatform.Name = "grpPlatform";
        grpPlatform.Padding = new Padding(3, 4, 3, 4);
        grpPlatform.Size = new Size(1140, 380);
        grpPlatform.TabIndex = 0;
        grpPlatform.TabStop = false;
        grpPlatform.Text = "Platform / Offline & Sync / Module Activation";
        // 
        // tlpPlatform
        // 
        tlpPlatform.ColumnCount = 2;
        tlpPlatform.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
        tlpPlatform.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpPlatform.Controls.Add(lblPlatformMode, 0, 0);
        tlpPlatform.Controls.Add(cboPlatformMode, 1, 0);
        tlpPlatform.Controls.Add(lblOfflineMode, 0, 1);
        tlpPlatform.Controls.Add(cboOfflineMode, 1, 1);
        tlpPlatform.Controls.Add(lblSyncPolicy, 0, 2);
        tlpPlatform.Controls.Add(cboSyncPolicy, 1, 2);
        tlpPlatform.Controls.Add(chkAllowOfflineRead, 1, 3);
        tlpPlatform.Controls.Add(chkSyncOnReconnect, 1, 4);
        tlpPlatform.Controls.Add(btnModuleActivation, 1, 5);
        tlpPlatform.Dock = DockStyle.Fill;
        tlpPlatform.Location = new Point(3, 27);
        tlpPlatform.Margin = new Padding(3, 4, 3, 4);
        tlpPlatform.Name = "tlpPlatform";
        tlpPlatform.RowCount = 6;
        tlpPlatform.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        tlpPlatform.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        tlpPlatform.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        tlpPlatform.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        tlpPlatform.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        tlpPlatform.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        tlpPlatform.Size = new Size(1134, 349);
        tlpPlatform.TabIndex = 0;
        // 
        // lblPlatformMode
        // 
        lblPlatformMode.Dock = DockStyle.Fill;
        lblPlatformMode.Location = new Point(917, 0);
        lblPlatformMode.Name = "lblPlatformMode";
        lblPlatformMode.Size = new Size(214, 52);
        lblPlatformMode.TabIndex = 0;
        lblPlatformMode.Text = "نطاق المنصة";
        // 
        // cboPlatformMode
        // 
        cboPlatformMode.Dock = DockStyle.Fill;
        cboPlatformMode.DropDownStyle = ComboBoxStyle.DropDownList;
        cboPlatformMode.Items.AddRange(new object[] { "DESKTOP", "MOBILE", "SHARED" });
        cboPlatformMode.Location = new Point(3, 4);
        cboPlatformMode.Margin = new Padding(3);
        cboPlatformMode.Name = "cboPlatformMode";
        cboPlatformMode.Size = new Size(908, 31);
        cboPlatformMode.TabIndex = 0;
        // 
        // lblOfflineMode
        // 
        lblOfflineMode.Dock = DockStyle.Fill;
        lblOfflineMode.Location = new Point(917, 52);
        lblOfflineMode.Name = "lblOfflineMode";
        lblOfflineMode.Size = new Size(214, 52);
        lblOfflineMode.TabIndex = 1;
        lblOfflineMode.Text = "تصنيف Offline";
        // 
        // cboOfflineMode
        // 
        cboOfflineMode.Dock = DockStyle.Fill;
        cboOfflineMode.DropDownStyle = ComboBoxStyle.DropDownList;
        cboOfflineMode.Items.AddRange(new object[] { "FULL", "PARTIAL", "ONLINE_ONLY" });
        cboOfflineMode.Location = new Point(3, 56);
        cboOfflineMode.Margin = new Padding(3);
        cboOfflineMode.Name = "cboOfflineMode";
        cboOfflineMode.Size = new Size(908, 31);
        cboOfflineMode.TabIndex = 1;
        // 
        // lblSyncPolicy
        // 
        lblSyncPolicy.Dock = DockStyle.Fill;
        lblSyncPolicy.Location = new Point(917, 104);
        lblSyncPolicy.Name = "lblSyncPolicy";
        lblSyncPolicy.Size = new Size(214, 52);
        lblSyncPolicy.TabIndex = 2;
        lblSyncPolicy.Text = "سياسة المزامنة";
        // 
        // cboSyncPolicy
        // 
        cboSyncPolicy.Dock = DockStyle.Fill;
        cboSyncPolicy.DropDownStyle = ComboBoxStyle.DropDownList;
        cboSyncPolicy.Location = new Point(3, 108);
        cboSyncPolicy.Margin = new Padding(3);
        cboSyncPolicy.Name = "cboSyncPolicy";
        cboSyncPolicy.Size = new Size(908, 31);
        cboSyncPolicy.TabIndex = 2;
        // 
        // chkAllowOfflineRead
        // 
        chkAllowOfflineRead.AutoSize = true;
        chkAllowOfflineRead.Location = new Point(694, 160);
        chkAllowOfflineRead.Margin = new Padding(3);
        chkAllowOfflineRead.Name = "chkAllowOfflineRead";
        chkAllowOfflineRead.Size = new Size(217, 27);
        chkAllowOfflineRead.TabIndex = 3;
        chkAllowOfflineRead.Text = "السماح بالقراءة دون اتصال";
        // 
        // chkSyncOnReconnect
        // 
        chkSyncOnReconnect.AutoSize = true;
        chkSyncOnReconnect.Location = new Point(649, 212);
        chkSyncOnReconnect.Margin = new Padding(3);
        chkSyncOnReconnect.Name = "chkSyncOnReconnect";
        chkSyncOnReconnect.Size = new Size(262, 27);
        chkSyncOnReconnect.TabIndex = 4;
        chkSyncOnReconnect.Text = "المزامنة تلقائيًا عند عودة الاتصال";
        // 
        // btnModuleActivation
        // 
        btnModuleActivation.Location = new Point(743, 264);
        btnModuleActivation.Margin = new Padding(3, 4, 3, 4);
        btnModuleActivation.Name = "btnModuleActivation";
        btnModuleActivation.Size = new Size(168, 38);
        btnModuleActivation.TabIndex = 5;
        btnModuleActivation.Text = "إعدادات تفعيل الوحدات";
        // 
        // pnlActions
        // 
        pnlActions.Controls.Add(flpActions);
        pnlActions.Controls.Add(flpClose);
        pnlActions.Dock = DockStyle.Bottom;
        pnlActions.Location = new Point(0, 696);
        pnlActions.Margin = new Padding(3, 4, 3, 4);
        pnlActions.Name = "pnlActions";
        pnlActions.Padding = new Padding(16, 8, 16, 8);
        pnlActions.Size = new Size(1180, 64);
        pnlActions.TabIndex = 2;
        // 
        // flpActions
        // 

        flpActions.Dock = DockStyle.Top;
        flpActions.Location = new Point(136, 8);
        flpActions.Margin = new Padding(3, 4, 3, 4);
        flpActions.Name = "flpActions";
        flpActions.RightToLeft = RightToLeft.Yes;
        flpActions.Size = new Size(1028, 48);
        flpActions.TabIndex = 0;
        flpActions.WrapContents = true;
        // 
        // btnNew
        // 
        btnNew.Location = new Point(912, 4);
        btnNew.Margin = new Padding(4);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(112, 38);
        btnNew.TabIndex = 0;
        btnNew.Text = "جديد";
        // 
        // btnSave
        // 
        btnSave.Location = new Point(793, 4);
        btnSave.Margin = new Padding(4);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(112, 38);
        btnSave.TabIndex = 1;
        btnSave.Text = "حفظ مسودة";
        // 
        // btnValidate
        // 
        btnValidate.Location = new Point(674, 4);
        btnValidate.Margin = new Padding(4);
        btnValidate.Name = "btnValidate";
        btnValidate.Size = new Size(112, 38);
        btnValidate.TabIndex = 2;
        btnValidate.Text = "تحقق";
        // 
        // btnPublish
        // 
        btnPublish.Location = new Point(554, 4);
        btnPublish.Margin = new Padding(4);
        btnPublish.Name = "btnPublish";
        btnPublish.Size = new Size(112, 38);
        btnPublish.TabIndex = 3;
        btnPublish.Text = "نشر/اعتماد";
        // 
        // btnRevertScope
        // 
        btnRevertScope.Location = new Point(410, 4);
        btnRevertScope.Margin = new Padding(4);
        btnRevertScope.Name = "btnRevertScope";
        btnRevertScope.Size = new Size(136, 38);
        btnRevertScope.TabIndex = 4;
        btnRevertScope.Text = "إلغاء تجاوز النطاق";
        // 
        // btnViewAudit
        // 
        btnViewAudit.Location = new Point(286, 4);
        btnViewAudit.Margin = new Padding(4);
        btnViewAudit.Name = "btnViewAudit";
        btnViewAudit.Size = new Size(116, 38);
        btnViewAudit.TabIndex = 5;
        btnViewAudit.Text = "عرض التدقيق";
        // 
        // btnRefresh
        // 
        btnRefresh.Location = new Point(166, 4);
        btnRefresh.Margin = new Padding(4);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(112, 38);
        btnRefresh.TabIndex = 6;
        btnRefresh.Text = "تحديث";
        // 
        // flpClose
        // 

        flpClose.Dock = DockStyle.Left;
        flpClose.Location = new Point(16, 8);
        flpClose.Name = "flpClose";
        flpClose.Size = new Size(120, 48);
        flpClose.TabIndex = 1;
        flpClose.WrapContents = true;
        // 
        // btnClose
        // 
        btnClose.Location = new Point(5, 4);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(112, 38);
        btnClose.TabIndex = 7;
        btnClose.Text = "إغلاق";
        // 
        // FrmGeneralSettings
        // 
        AutoScaleMode = AutoScaleMode.None;
        Size = new Size(1180, 760);
        Controls.Add(tabSettings);
        Controls.Add(designerCommandBar);
        Controls.Add(pnlHeader);
        Margin = new Padding(3, 4, 3, 4);
        MinimumSize = new Size(1136, 759);
        Name = "UcGeneralSettings";
        Text = "الإعدادات العامة";
        pnlHeader.ResumeLayout(false);
        tabSettings.ResumeLayout(false);
        tabCompanyScope.ResumeLayout(false);
        grpGeneralDefaults.ResumeLayout(false);
        tlpGeneralDefaults.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)nudSessionTimeout).EndInit();
        grpCompanyScope.ResumeLayout(false);
        tlpCompanyScope.ResumeLayout(false);
        tlpCompanyScope.PerformLayout();
        tabAppearance.ResumeLayout(false);
        grpAppearance.ResumeLayout(false);
        tlpAppearance.ResumeLayout(false);
        tlpAppearance.PerformLayout();
        tabNotifications.ResumeLayout(false);
        grpNotifications.ResumeLayout(false);
        flpNotifications.ResumeLayout(false);
        flpNotifications.PerformLayout();
        tabPlatformOffline.ResumeLayout(false);
        grpPlatform.ResumeLayout(false);
        tlpPlatform.ResumeLayout(false);
        tlpPlatform.PerformLayout();
        pnlActions.ResumeLayout(false);
        flpActions.ResumeLayout(false);
        flpClose.ResumeLayout(false);
        tabSettings.Font = new Font("Tahoma", 9F);
        tabCompanyScope.UseVisualStyleBackColor = false;
        lblCurrency.Margin = new Padding(3);
        lblCurrency.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        cboDefaultCurrency.Font = new Font("Tahoma", 9F);
        lblLanguage.Margin = new Padding(3);
        lblLanguage.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        cboDefaultLanguage.Font = new Font("Tahoma", 9F);
        lblDateFormat.Margin = new Padding(3);
        lblDateFormat.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        cboDateFormat.Font = new Font("Tahoma", 9F);
        lblSessionTimeout.Margin = new Padding(3);
        lblSessionTimeout.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        nudSessionTimeout.Font = new Font("Tahoma", 9F);
        lblCompanyGroup.Margin = new Padding(3);
        lblCompanyGroup.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        cboCompanyGroup.Font = new Font("Tahoma", 9F);
        lblCompany.Margin = new Padding(3);
        lblCompany.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        cboCompany.Font = new Font("Tahoma", 9F);
        lblBranchGroup.Margin = new Padding(3);
        lblBranchGroup.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        cboBranchGroup.Font = new Font("Tahoma", 9F);
        lblBranch.Margin = new Padding(3);
        lblBranch.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        cboBranch.Font = new Font("Tahoma", 9F);
        lblCompanyVisibility.Margin = new Padding(3);
        lblCompanyVisibility.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        cboCompanyVisibility.Font = new Font("Tahoma", 9F);
        lblBranchVisibility.Margin = new Padding(3);
        lblBranchVisibility.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        cboBranchVisibility.Font = new Font("Tahoma", 9F);
        chkCompanyEnabled.Dock = DockStyle.Fill;
        chkBranchEnabled.Dock = DockStyle.Fill;
        tabAppearance.AutoScroll = true;
        tabAppearance.UseVisualStyleBackColor = false;
        lblTheme.Margin = new Padding(3);
        lblTheme.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        cboTheme.Font = new Font("Tahoma", 9F);
        lblDensity.Margin = new Padding(3);
        lblDensity.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        cboDensity.Font = new Font("Tahoma", 9F);
        chkShowDashboardAtStartup.Dock = DockStyle.Fill;
        chkConfirmBeforeClose.Dock = DockStyle.Fill;
        chkRememberLastScope.Dock = DockStyle.Fill;
        tabNotifications.AutoScroll = true;
        tabNotifications.UseVisualStyleBackColor = false;
        tabPlatformOffline.AutoScroll = true;
        tabPlatformOffline.UseVisualStyleBackColor = false;
        lblPlatformMode.Margin = new Padding(3);
        lblPlatformMode.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        cboPlatformMode.Font = new Font("Tahoma", 9F);
        lblOfflineMode.Margin = new Padding(3);
        lblOfflineMode.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        cboOfflineMode.Font = new Font("Tahoma", 9F);
        lblSyncPolicy.Margin = new Padding(3);
        lblSyncPolicy.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        cboSyncPolicy.Font = new Font("Tahoma", 9F);
        chkAllowOfflineRead.Dock = DockStyle.Fill;
        chkSyncOnReconnect.Dock = DockStyle.Fill;
        flpActions.AutoSize = true;
        flpActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flpActions.Padding = new Padding(5);
        flpActions.BackColor = Color.FromArgb(224, 224, 224);
        btnNew.Dock = DockStyle.None;
        btnNew.BackColor = Color.FromArgb(224, 224, 224);
        btnNew.ForeColor = Color.FromArgb(16, 24, 40);
        btnNew.Font = new Font("Tahoma", 9F);
        btnNew.UseVisualStyleBackColor = false;
        btnSave.Dock = DockStyle.None;
        btnSave.BackColor = Color.FromArgb(224, 224, 224);
        btnSave.ForeColor = Color.FromArgb(16, 24, 40);
        btnSave.Font = new Font("Tahoma", 9F);
        btnSave.UseVisualStyleBackColor = false;
        btnValidate.Dock = DockStyle.None;
        btnValidate.BackColor = Color.FromArgb(224, 224, 224);
        btnValidate.ForeColor = Color.FromArgb(16, 24, 40);
        btnValidate.Font = new Font("Tahoma", 9F);
        btnValidate.UseVisualStyleBackColor = false;
        btnPublish.Dock = DockStyle.None;
        btnPublish.BackColor = Color.FromArgb(224, 224, 224);
        btnPublish.ForeColor = Color.FromArgb(16, 24, 40);
        btnPublish.Font = new Font("Tahoma", 9F);
        btnPublish.UseVisualStyleBackColor = false;
        btnRevertScope.Dock = DockStyle.None;
        btnRevertScope.BackColor = Color.FromArgb(224, 224, 224);
        btnRevertScope.ForeColor = Color.FromArgb(16, 24, 40);
        btnRevertScope.Font = new Font("Tahoma", 9F);
        btnRevertScope.UseVisualStyleBackColor = false;
        btnViewAudit.Dock = DockStyle.None;
        btnViewAudit.BackColor = Color.FromArgb(224, 224, 224);
        btnViewAudit.ForeColor = Color.FromArgb(16, 24, 40);
        btnViewAudit.Font = new Font("Tahoma", 9F);
        btnViewAudit.UseVisualStyleBackColor = false;
        btnRefresh.Dock = DockStyle.None;
        btnRefresh.BackColor = Color.FromArgb(224, 224, 224);
        btnRefresh.ForeColor = Color.FromArgb(16, 24, 40);
        btnRefresh.Font = new Font("Tahoma", 9F);
        btnRefresh.UseVisualStyleBackColor = false;
        flpClose.AutoSize = true;
        flpClose.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flpClose.Padding = new Padding(5);
        flpClose.BackColor = Color.FromArgb(224, 224, 224);
        btnClose.Dock = DockStyle.None;
        btnClose.BackColor = Color.FromArgb(224, 224, 224);
        btnClose.ForeColor = Color.FromArgb(16, 24, 40);
        btnClose.Font = new Font("Tahoma", 9F);
        btnClose.UseVisualStyleBackColor = false;
        Font = new Font("Tahoma", 9F);
        BackColor = Color.FromArgb(248, 250, 252);
        tlpAuditInfo.SuspendLayout();
        tlpAuditInfo.ColumnCount = 7;
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
        tlpAuditInfo.Controls.Add(lblPrintCount, 6, 0);
        tlpAuditInfo.Controls.Add(lblLastPrintedAt, 5, 0);
        tlpAuditInfo.Controls.Add(lblEditCount, 4, 0);
        tlpAuditInfo.Controls.Add(lblModifiedAt, 3, 0);
        tlpAuditInfo.Controls.Add(lblModifiedBy, 2, 0);
        tlpAuditInfo.Controls.Add(lblCreatedAt, 1, 0);
        tlpAuditInfo.Controls.Add(lblCreatedBy, 0, 0);
        tlpAuditInfo.Dock = DockStyle.Bottom;
        tlpAuditInfo.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
        tlpAuditInfo.Margin = new Padding(0);
        tlpAuditInfo.BackColor = Color.FromArgb(248, 250, 252);
        tlpAuditInfo.Name = "tlpAuditInfo";
        tlpAuditInfo.Padding = new Padding(8);
        tlpAuditInfo.RightToLeft = RightToLeft.Yes;
        tlpAuditInfo.RowCount = 1;
        tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpAuditInfo.ResumeLayout(false);
        lblPrintCount.BackColor = Color.FromArgb(192, 255, 255);
        lblPrintCount.AutoEllipsis = true;
        lblPrintCount.Dock = DockStyle.Fill;
        lblPrintCount.Font = new Font("Tahoma", 9F);
        lblPrintCount.Margin = new Padding(3);
        lblPrintCount.ForeColor = Color.FromArgb(16, 24, 40);
        lblPrintCount.Name = "lblPrintCount";
        lblPrintCount.TabStop = false;
        lblPrintCount.RightToLeft = RightToLeft.Yes;
        lblPrintCount.Text = "عدد مرات الطباعة: —";
        lblPrintCount.TextAlign = ContentAlignment.MiddleRight;
        lblLastPrintedAt.BackColor = Color.FromArgb(192, 255, 255);
        lblLastPrintedAt.AutoEllipsis = true;
        lblLastPrintedAt.Dock = DockStyle.Fill;
        lblLastPrintedAt.Font = new Font("Tahoma", 9F);
        lblLastPrintedAt.Margin = new Padding(3);
        lblLastPrintedAt.ForeColor = Color.FromArgb(16, 24, 40);
        lblLastPrintedAt.Name = "lblLastPrintedAt";
        lblLastPrintedAt.TabStop = false;
        lblLastPrintedAt.RightToLeft = RightToLeft.Yes;
        lblLastPrintedAt.Text = "تاريخ اخر طباعة: —";
        lblLastPrintedAt.TextAlign = ContentAlignment.MiddleRight;
        lblEditCount.BackColor = Color.FromArgb(192, 255, 255);
        lblEditCount.AutoEllipsis = true;
        lblEditCount.Dock = DockStyle.Fill;
        lblEditCount.Font = new Font("Tahoma", 9F);
        lblEditCount.Margin = new Padding(3);
        lblEditCount.ForeColor = Color.FromArgb(16, 24, 40);
        lblEditCount.Name = "lblEditCount";
        lblEditCount.TabStop = false;
        lblEditCount.RightToLeft = RightToLeft.Yes;
        lblEditCount.Text = "عدد التعديلات: —";
        lblEditCount.TextAlign = ContentAlignment.MiddleRight;
        lblModifiedAt.BackColor = Color.FromArgb(192, 255, 255);
        lblModifiedAt.AutoEllipsis = true;
        lblModifiedAt.Dock = DockStyle.Fill;
        lblModifiedAt.Font = new Font("Tahoma", 9F);
        lblModifiedAt.Margin = new Padding(3);
        lblModifiedAt.ForeColor = Color.FromArgb(16, 24, 40);
        lblModifiedAt.Name = "lblModifiedAt";
        lblModifiedAt.TabStop = false;
        lblModifiedAt.RightToLeft = RightToLeft.Yes;
        lblModifiedAt.Text = "تاريخ التعديل: —";
        lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
        lblModifiedBy.BackColor = Color.FromArgb(192, 255, 255);
        lblModifiedBy.AutoEllipsis = true;
        lblModifiedBy.Dock = DockStyle.Fill;
        lblModifiedBy.Font = new Font("Tahoma", 9F);
        lblModifiedBy.Margin = new Padding(3);
        lblModifiedBy.ForeColor = Color.FromArgb(16, 24, 40);
        lblModifiedBy.Name = "lblModifiedBy";
        lblModifiedBy.TabStop = false;
        lblModifiedBy.RightToLeft = RightToLeft.Yes;
        lblModifiedBy.Text = "عدل بواسطة: —";
        lblModifiedBy.TextAlign = ContentAlignment.MiddleRight;
        lblCreatedAt.BackColor = Color.FromArgb(192, 255, 255);
        lblCreatedAt.AutoEllipsis = true;
        lblCreatedAt.Dock = DockStyle.Fill;
        lblCreatedAt.Font = new Font("Tahoma", 9F);
        lblCreatedAt.Margin = new Padding(3);
        lblCreatedAt.ForeColor = Color.FromArgb(16, 24, 40);
        lblCreatedAt.Name = "lblCreatedAt";
        lblCreatedAt.TabStop = false;
        lblCreatedAt.RightToLeft = RightToLeft.Yes;
        lblCreatedAt.Text = "تاريخ الانشاء: —";
        lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
        lblCreatedBy.BackColor = Color.FromArgb(192, 255, 255);
        lblCreatedBy.AutoEllipsis = true;
        lblCreatedBy.Dock = DockStyle.Fill;
        lblCreatedBy.Font = new Font("Tahoma", 9F);
        lblCreatedBy.Margin = new Padding(3);
        lblCreatedBy.ForeColor = Color.FromArgb(16, 24, 40);
        lblCreatedBy.Name = "lblCreatedBy";
        lblCreatedBy.TabStop = false;
        lblCreatedBy.RightToLeft = RightToLeft.Yes;
        lblCreatedBy.Text = "أنشأ بواسطة: —";
        lblCreatedBy.TextAlign = ContentAlignment.MiddleRight;
        tlpAuditInfo.Height = 48;
        Controls.Add(tlpAuditInfo);
        pnlActions.AutoSize = true;
        pnlActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlActions.MinimumSize = new Size(0, 48);
        tabOperation.Name = "tabOperation";
        tabOperation.Text = "التشغيل العام";
        tabOperation.BackColor = Color.LightCyan;
        tabOperation.AutoScroll = true;
        tabOperation.Padding = new Padding(16);
        tabLanguage.Name = "tabLanguage";
        tabLanguage.Text = "اللغة والتنسيق";
        tabLanguage.BackColor = Color.LightCyan;
        tabLanguage.AutoScroll = true;
        tabLanguage.Padding = new Padding(16);
        tabPrintReports.Name = "tabPrintReports";
        tabPrintReports.Text = "الطباعة والتقارير";
        tabPrintReports.BackColor = Color.LightCyan;
        tabPrintReports.AutoScroll = true;
        tabPrintReports.Padding = new Padding(16);
        tabChangeHistory.Name = "tabChangeHistory";
        tabChangeHistory.Text = "سجل التغييرات";
        tabChangeHistory.BackColor = Color.LightCyan;
        tabChangeHistory.AutoScroll = true;
        tabChangeHistory.Padding = new Padding(16);
        languageFields.Name = "languageFields";
        languageFields.Dock = DockStyle.Top;
        languageFields.Height = 96;
        languageFields.ColumnCount = 2;
        languageFields.RowCount = 2;
        languageFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));
        languageFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        languageFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        languageFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        tabLanguage.Controls.Add(languageFields);
        languageFields.Controls.Add(lblLanguage, 0, 0);
        languageFields.Controls.Add(cboDefaultLanguage, 1, 0);
        languageFields.Controls.Add(lblDateFormat, 0, 1);
        languageFields.Controls.Add(cboDateFormat, 1, 1);
        tlpGeneralDefaults.ColumnCount = 2;
        tlpGeneralDefaults.ColumnStyles.Clear();
        tlpGeneralDefaults.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));
        tlpGeneralDefaults.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tabAppearance.Text = "سلوك الواجهة";
        tabPlatformOffline.Text = "الاتصال والمزامنة";
        tabNotifications.Text = "التنبيهات العامة";
        printSettingsContent.Name = "printSettingsContent";
        printSettingsContent.ConfigureEmbeddedView();
        printSettingsContent.Dock = DockStyle.Fill;
        tabPrintReports.Controls.Add(printSettingsContent);
        changeHistory.Name = "changeHistory";
        changeHistory.Dock = DockStyle.Fill;
        changeHistory.ReadOnly = true;
        changeHistory.AllowUserToAddRows = false;
        changeHistory.AllowUserToDeleteRows = false;
        changeHistory.AutoGenerateColumns = true;
        changeHistory.BackgroundColor = Color.White;
        changeHistory.Font = new Font("Tahoma", 9F);
        tabChangeHistory.Controls.Add(changeHistory);
        historyState.Name = "historyState";
        historyState.Text = "سجل التغييرات — لم تُحمّل بيانات التدقيق بعد";
        historyState.Dock = DockStyle.Top;
        historyState.Height = 40;
        historyState.TextAlign = ContentAlignment.MiddleRight;
        tabChangeHistory.Controls.Add(historyState);
        tabSettings.Controls.Add(tabOperation);
        tabSettings.Controls.Add(tabLanguage);
        tabSettings.Controls.Add(tabAppearance);
        tabSettings.Controls.Add(tabCompanyScope);
        tabSettings.Controls.Add(tabPrintReports);
        tabSettings.Controls.Add(tabPlatformOffline);
        tabSettings.Controls.Add(tabNotifications);
        tabSettings.Controls.Add(tabChangeHistory);
        tabOperation.TabIndex = 0;
        tabLanguage.TabIndex = 1;
        tabAppearance.TabIndex = 2;
        tabCompanyScope.TabIndex = 3;
        tabPrintReports.TabIndex = 4;
        tabPlatformOffline.TabIndex = 5;
        tabNotifications.TabIndex = 6;
        tabChangeHistory.TabIndex = 7;
        ResumeLayout(false);
    
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Bottom;
        designerCommandBar.Location = new Point(0, 696);
        designerCommandBar.Size = new Size(1180, 64);
        designerCommandBar.Margin = new Padding(3, 4, 3, 4);
        designerCommandBar.MinimumSize = new Size(0, 48);
        designerCommandBar.TabIndex = 2;
        designerCommandBar.Controls.Add(pnlActions);
        pnlActions.Dock = DockStyle.Fill;
        pnlActions.Margin = Padding.Empty;
        designerCommandBar.Name = "designerCommandBar";
        designerCommandBar.BackColor = Color.FromArgb(239, 239, 239);
        designerCommandBar.BorderStyle = BorderStyle.FixedSingle;
        designerCommandBar.Controls.Add(designerCommandFlow);
        designerCommandBar.Controls.Add(designerCloseHost);
        designerCloseHost.Dock = DockStyle.Left;
        designerCloseHost.Width = 28;
        designerCloseHost.Name = "designerCloseHost";
        designerCommandFlow.Name = "designerCommandFlow";
        designerCommandFlow.Dock = DockStyle.Fill;
        designerCommandFlow.AutoSize = false;
        designerCommandFlow.WrapContents = false;
        designerCommandFlow.FlowDirection = FlowDirection.RightToLeft;
        designerCommandFlow.RightToLeft = RightToLeft.No;
        designerCommandFlow.Padding = new Padding(2);
        designerCommandBar.MinimumSize = new Size(0, 30);
        designerCommandBar.Height = 30;
        standardCommandEdit.Name = "standardCommandEdit";
        standardCommandEdit.Enabled = false;
        standardCommandEdit.Visible = true;
        standardCommandEdit.AccessibleName = "تعديل";
        standardCommandDelete.Name = "standardCommandDelete";
        standardCommandDelete.Enabled = false;
        standardCommandDelete.Visible = false;
        standardCommandDelete.AccessibleName = "حذف";
        standardCommandCancel.Name = "standardCommandCancel";
        standardCommandCancel.Enabled = false;
        standardCommandCancel.Visible = true;
        standardCommandCancel.AccessibleName = "تراجع";
        standardCommandView.Name = "standardCommandView";
        standardCommandView.Enabled = false;
        standardCommandView.Visible = false;
        standardCommandView.AccessibleName = "عرض";
        standardCommandLast.Name = "standardCommandLast";
        standardCommandLast.Enabled = false;
        standardCommandLast.Visible = false;
        standardCommandLast.AccessibleName = "الأخير";
        standardCommandNext.Name = "standardCommandNext";
        standardCommandNext.Enabled = false;
        standardCommandNext.Visible = false;
        standardCommandNext.AccessibleName = "التالي";
        standardCommandPrevious.Name = "standardCommandPrevious";
        standardCommandPrevious.Enabled = false;
        standardCommandPrevious.Visible = false;
        standardCommandPrevious.AccessibleName = "السابق";
        standardCommandFirst.Name = "standardCommandFirst";
        standardCommandFirst.Enabled = false;
        standardCommandFirst.Visible = false;
        standardCommandFirst.AccessibleName = "الأول";
        standardCommandPrint.Name = "standardCommandPrint";
        standardCommandPrint.Enabled = false;
        standardCommandPrint.Visible = true;
        standardCommandPrint.AccessibleName = "طباعة";
        standardCommandImport.Name = "standardCommandImport";
        standardCommandImport.Enabled = false;
        standardCommandImport.Visible = false;
        standardCommandImport.AccessibleName = "استيراد";
        standardCommandExport.Name = "standardCommandExport";
        standardCommandExport.Enabled = false;
        standardCommandExport.Visible = false;
        standardCommandExport.AccessibleName = "تصدير";
        standardCommandHelp.Name = "standardCommandHelp";
        standardCommandHelp.Enabled = false;
        standardCommandHelp.Visible = false;
        standardCommandHelp.AccessibleName = "مساعدة";
        btnNew.AutoSize = false;
        btnNew.Dock = DockStyle.None;
        btnNew.MinimumSize = Size.Empty;
        btnNew.Size = new Size(26, 24);
        btnNew.Margin = new Padding(1);
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnNew.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Add;
        btnNew.Text = "";
        designerCommandFlow.Controls.Add(btnNew);
        designerCommandBar.SetCommandRole(btnNew, TransportERP.Desktop.CoreUI.DesignerCommandRole.Add);
        standardCommandEdit.AutoSize = false;
        standardCommandEdit.Dock = DockStyle.None;
        standardCommandEdit.MinimumSize = Size.Empty;
        standardCommandEdit.Size = new Size(26, 24);
        standardCommandEdit.Margin = new Padding(1);
        standardCommandEdit.FlatStyle = FlatStyle.Flat;
        standardCommandEdit.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandEdit.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Edit;
        standardCommandEdit.Text = "";
        designerCommandFlow.Controls.Add(standardCommandEdit);
        designerCommandBar.SetCommandRole(standardCommandEdit, TransportERP.Desktop.CoreUI.DesignerCommandRole.Edit);
        standardCommandDelete.AutoSize = false;
        standardCommandDelete.Dock = DockStyle.None;
        standardCommandDelete.MinimumSize = Size.Empty;
        standardCommandDelete.Size = new Size(26, 24);
        standardCommandDelete.Margin = new Padding(1);
        standardCommandDelete.FlatStyle = FlatStyle.Flat;
        standardCommandDelete.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandDelete.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Delete;
        standardCommandDelete.Text = "";
        designerCommandFlow.Controls.Add(standardCommandDelete);
        designerCommandBar.SetCommandRole(standardCommandDelete, TransportERP.Desktop.CoreUI.DesignerCommandRole.Delete);
        standardCommandCancel.AutoSize = false;
        standardCommandCancel.Dock = DockStyle.None;
        standardCommandCancel.MinimumSize = Size.Empty;
        standardCommandCancel.Size = new Size(26, 24);
        standardCommandCancel.Margin = new Padding(1);
        standardCommandCancel.FlatStyle = FlatStyle.Flat;
        standardCommandCancel.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandCancel.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Cancel;
        standardCommandCancel.Text = "";
        designerCommandFlow.Controls.Add(standardCommandCancel);
        designerCommandBar.SetCommandRole(standardCommandCancel, TransportERP.Desktop.CoreUI.DesignerCommandRole.Cancel);
        standardCommandView.AutoSize = false;
        standardCommandView.Dock = DockStyle.None;
        standardCommandView.MinimumSize = Size.Empty;
        standardCommandView.Size = new Size(26, 24);
        standardCommandView.Margin = new Padding(1);
        standardCommandView.FlatStyle = FlatStyle.Flat;
        standardCommandView.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandView.Image = TransportERP.Desktop.CoreUI.CommandBarImages.View;
        standardCommandView.Text = "";
        designerCommandFlow.Controls.Add(standardCommandView);
        designerCommandBar.SetCommandRole(standardCommandView, TransportERP.Desktop.CoreUI.DesignerCommandRole.View);
        standardCommandLast.AutoSize = false;
        standardCommandLast.Dock = DockStyle.None;
        standardCommandLast.MinimumSize = Size.Empty;
        standardCommandLast.Size = new Size(26, 24);
        standardCommandLast.Margin = new Padding(1);
        standardCommandLast.FlatStyle = FlatStyle.Flat;
        standardCommandLast.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandLast.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Last;
        standardCommandLast.Text = "";
        designerCommandFlow.Controls.Add(standardCommandLast);
        designerCommandBar.SetCommandRole(standardCommandLast, TransportERP.Desktop.CoreUI.DesignerCommandRole.Last);
        standardCommandNext.AutoSize = false;
        standardCommandNext.Dock = DockStyle.None;
        standardCommandNext.MinimumSize = Size.Empty;
        standardCommandNext.Size = new Size(26, 24);
        standardCommandNext.Margin = new Padding(1);
        standardCommandNext.FlatStyle = FlatStyle.Flat;
        standardCommandNext.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandNext.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Next;
        standardCommandNext.Text = "";
        designerCommandFlow.Controls.Add(standardCommandNext);
        designerCommandBar.SetCommandRole(standardCommandNext, TransportERP.Desktop.CoreUI.DesignerCommandRole.Next);
        standardCommandPrevious.AutoSize = false;
        standardCommandPrevious.Dock = DockStyle.None;
        standardCommandPrevious.MinimumSize = Size.Empty;
        standardCommandPrevious.Size = new Size(26, 24);
        standardCommandPrevious.Margin = new Padding(1);
        standardCommandPrevious.FlatStyle = FlatStyle.Flat;
        standardCommandPrevious.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandPrevious.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Previous;
        standardCommandPrevious.Text = "";
        designerCommandFlow.Controls.Add(standardCommandPrevious);
        designerCommandBar.SetCommandRole(standardCommandPrevious, TransportERP.Desktop.CoreUI.DesignerCommandRole.Previous);
        standardCommandFirst.AutoSize = false;
        standardCommandFirst.Dock = DockStyle.None;
        standardCommandFirst.MinimumSize = Size.Empty;
        standardCommandFirst.Size = new Size(26, 24);
        standardCommandFirst.Margin = new Padding(1);
        standardCommandFirst.FlatStyle = FlatStyle.Flat;
        standardCommandFirst.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandFirst.Image = TransportERP.Desktop.CoreUI.CommandBarImages.First;
        standardCommandFirst.Text = "";
        designerCommandFlow.Controls.Add(standardCommandFirst);
        designerCommandBar.SetCommandRole(standardCommandFirst, TransportERP.Desktop.CoreUI.DesignerCommandRole.First);
        btnSave.AutoSize = false;
        btnSave.Dock = DockStyle.None;
        btnSave.MinimumSize = Size.Empty;
        btnSave.Size = new Size(26, 24);
        btnSave.Margin = new Padding(1);
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnSave.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Save;
        btnSave.Text = "";
        designerCommandFlow.Controls.Add(btnSave);
        designerCommandBar.SetCommandRole(btnSave, TransportERP.Desktop.CoreUI.DesignerCommandRole.Save);
        standardCommandPrint.AutoSize = false;
        standardCommandPrint.Dock = DockStyle.None;
        standardCommandPrint.MinimumSize = Size.Empty;
        standardCommandPrint.Size = new Size(26, 24);
        standardCommandPrint.Margin = new Padding(1);
        standardCommandPrint.FlatStyle = FlatStyle.Flat;
        standardCommandPrint.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandPrint.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Print;
        standardCommandPrint.Text = "";
        designerCommandFlow.Controls.Add(standardCommandPrint);
        designerCommandBar.SetCommandRole(standardCommandPrint, TransportERP.Desktop.CoreUI.DesignerCommandRole.Print);
        btnClose.AutoSize = false;
        btnClose.Dock = DockStyle.None;
        btnClose.MinimumSize = Size.Empty;
        btnClose.Size = new Size(26, 24);
        btnClose.Margin = new Padding(1);
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnClose.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Close;
        btnClose.Text = "";
        designerCloseHost.Controls.Add(btnClose);
        btnClose.Location = new Point(1, 1);
        designerCommandBar.SetCommandRole(btnClose, TransportERP.Desktop.CoreUI.DesignerCommandRole.Close);
        btnRefresh.AutoSize = false;
        btnRefresh.Dock = DockStyle.None;
        btnRefresh.MinimumSize = Size.Empty;
        btnRefresh.Size = new Size(26, 24);
        btnRefresh.Margin = new Padding(1);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnRefresh.Text = "تحديث";
        designerCommandFlow.Controls.Add(btnRefresh);
        designerCommandBar.SetCommandRole(btnRefresh, TransportERP.Desktop.CoreUI.DesignerCommandRole.Refresh);
        standardCommandImport.AutoSize = false;
        standardCommandImport.Dock = DockStyle.None;
        standardCommandImport.MinimumSize = Size.Empty;
        standardCommandImport.Size = new Size(26, 24);
        standardCommandImport.Margin = new Padding(1);
        standardCommandImport.FlatStyle = FlatStyle.Flat;
        standardCommandImport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandImport.Text = "استيراد";
        designerCommandFlow.Controls.Add(standardCommandImport);
        designerCommandBar.SetCommandRole(standardCommandImport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Import);
        standardCommandExport.AutoSize = false;
        standardCommandExport.Dock = DockStyle.None;
        standardCommandExport.MinimumSize = Size.Empty;
        standardCommandExport.Size = new Size(26, 24);
        standardCommandExport.Margin = new Padding(1);
        standardCommandExport.FlatStyle = FlatStyle.Flat;
        standardCommandExport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandExport.Text = "تصدير";
        designerCommandFlow.Controls.Add(standardCommandExport);
        designerCommandBar.SetCommandRole(standardCommandExport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Export);
        standardCommandHelp.AutoSize = false;
        standardCommandHelp.Dock = DockStyle.None;
        standardCommandHelp.MinimumSize = Size.Empty;
        standardCommandHelp.Size = new Size(26, 24);
        standardCommandHelp.Margin = new Padding(1);
        standardCommandHelp.FlatStyle = FlatStyle.Flat;
        standardCommandHelp.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandHelp.Text = "مساعدة";
        designerCommandFlow.Controls.Add(standardCommandHelp);
        designerCommandBar.SetCommandRole(standardCommandHelp, TransportERP.Desktop.CoreUI.DesignerCommandRole.Help);
        designerCommandFlow.Controls.Add(btnValidate);
        designerCommandFlow.Controls.Add(btnPublish);
        designerCommandFlow.Controls.Add(btnRevertScope);
        designerCommandFlow.Controls.Add(btnViewAudit);
    
        // Shared audit presentation; original sources remain owned by this screen.
        standardAuditMetadata = new TransportERP.Desktop.CoreUI.AuditMetadataControl();
        standardAuditMetadata.Name = "standardAuditMetadata";
        standardAuditMetadata.Dock = DockStyle.Bottom;
        standardAuditMetadata.Margin = Padding.Empty;
        standardAuditMetadata.TabStop = false;
        standardAuditMetadata.Size = new Size(800, 64);
        tlpAuditInfo.Visible = false;
        Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
