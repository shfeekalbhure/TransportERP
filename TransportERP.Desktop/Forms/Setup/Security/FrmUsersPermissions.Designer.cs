#nullable enable

using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.Setup.Security;

partial class UcUsersPermissions
{
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;

    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
    private FlowLayoutPanel designerHiddenCommands = null!;
    private Button standardCommandImport = null!;
    private Button standardCommandExport = null!;
    private Button standardCommandHelp = null!;

    private TableLayoutPanel securityPolicyV20 = null!;
    private Label lblSettingValue__SET_SEC_001 = null!;
    private ComboBox SettingValue__SET_SEC_001 = null!;
    private Label lblSettingValue__SET_SEC_002 = null!;
    private ComboBox SettingValue__SET_SEC_002 = null!;
    private Label lblSettingValue__SET_SEC_003 = null!;
    private ComboBox SettingValue__SET_SEC_003 = null!;
    private Label lblPermissionTargetIds = null!;
    private CheckedListBox PermissionTargetIds = null!;
    private System.ComponentModel.IContainer? components;
    private SplitContainer splitMain = null!;
    private Panel pnlUsers = null!; private Label lblUsersList = null!; private TextBox txtUserSearch = null!;     private TabControl tabUserDetails = null!; private TabPage tabUserData = null!;     private TabPage tabPermissions = null!; private TableLayoutPanel tlpPermissions = null!; private GroupBox grpPermissionTree = null!; private TreeView tvPermissions = null!; private CheckBox chkSelectAllPermissions = null!; private GroupBox grpModulePermissions = null!; private DataGridView dgvModulePermissions = null!; private DataGridViewTextBoxColumn colModule = null!; private DataGridViewCheckBoxColumn colView = null!; private DataGridViewCheckBoxColumn colAdd = null!; private DataGridViewCheckBoxColumn colEdit = null!; private DataGridViewCheckBoxColumn colPrint = null!;
    private Panel pnlActions = null!; 
    protected override void Dispose(bool disposing) { if (disposing && components is not null) components.Dispose(); base.Dispose(disposing); }

    private void InitializeComponent()
    {
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        designerHiddenCommands = new FlowLayoutPanel();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        auditFooter = new AuditCountersControl();
        splitMain = new SplitContainer();
        pnlUsers = new Panel();
        tvUserRecords = new TreeView();
        txtUserSearch = new TextBox();
        lblUsersList = new Label();
        tabUserDetails = new TabControl();
        tabUserData = new TabPage();
        userLookupV20 = new TableLayoutPanel();
        lblUserId = new Label();
        UserId = new ComboBox();
        label7 = new Label();
        txtAccessStartDate = new TextBox();
        label8 = new Label();
        txtAccessEndDate = new TextBox();
        label9 = new Label();
        txtAccessFromTime = new TextBox();
        label10 = new Label();
        txtAccessToTime = new TextBox();
        label19 = new Label();
        txtOnyxProfession = new TextBox();
        label20 = new Label();
        txtOnyxUserComputerName = new TextBox();
        label21 = new Label();
        txtOnyxUserStopReason = new TextBox();
        chkOnyxUserStopped = new CheckBox();
        label16 = new Label();
        textBox1 = new TextBox();
        textBox2 = new TextBox();
        textBox3 = new TextBox();
        label17 = new Label();
        tabDevicesV20 = new TabPage();
        dgvOnyxHandheldDevices = new DataGridView();
        Column1 = new DataGridViewTextBoxColumn();
        Column2 = new DataGridViewTextBoxColumn();
        Column3 = new DataGridViewCheckBoxColumn();
        Column4 = new DataGridViewTextBoxColumn();
        deviceFieldsV20 = new TableLayoutPanel();
        lblDeviceV20 = new Label();
        DeviceId = new ComboBox();
        checkBox1 = new CheckBox();
        tabPosOnyx = new TabPage();
        posLayoutHost = new TableLayoutPanel();
        label1 = new Label();
        cboPosConnection = new ComboBox();
        txtPosNumber = new TextBox();
        label4 = new Label();
        label5 = new Label();
        txtBarcodePath = new TextBox();
        label2 = new Label();
        label3 = new Label();
        txtPosName = new TextBox();
        txtBarcodePrinter = new TextBox();
        tabRolesV20 = new TabPage();
        roleCatalogLayout = new TableLayoutPanel();
        roleActions = new FlowLayoutPanel();
        btnRoleAdd = new Button();
        btnRoleRemove = new Button();
        btnRoleValidate = new Button();
        btnRoleSave = new Button();
        btnRoleClear = new Button();
        dgvRoleCatalog = new DataGridView();
        dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
        dataGridViewComboBoxColumn1 = new DataGridViewComboBoxColumn();
        dataGridViewComboBoxColumn2 = new DataGridViewComboBoxColumn();
        dataGridViewCheckBoxColumn1 = new DataGridViewCheckBoxColumn();
        lblRoleState = new Label();
        roleLookupV20 = new TableLayoutPanel();
        lblRoleId = new Label();
        RoleId = new ComboBox();
        tabPermissions = new TabPage();
        tlpPermissions = new TableLayoutPanel();
        grpPermissionTree = new GroupBox();
        tvPermissions = new TreeView();
        chkSelectAllPermissions = new CheckBox();
        grpModulePermissions = new GroupBox();
        dgvModulePermissions = new DataGridView();
        colModule = new DataGridViewTextBoxColumn();
        colView = new DataGridViewCheckBoxColumn();
        colAdd = new DataGridViewCheckBoxColumn();
        colEdit = new DataGridViewCheckBoxColumn();
        colPrint = new DataGridViewCheckBoxColumn();
        securityPolicyV20 = new TableLayoutPanel();
        lblSettingValue__SET_SEC_001 = new Label();
        SettingValue__SET_SEC_001 = new ComboBox();
        lblSettingValue__SET_SEC_002 = new Label();
        SettingValue__SET_SEC_002 = new ComboBox();
        lblSettingValue__SET_SEC_003 = new Label();
        SettingValue__SET_SEC_003 = new ComboBox();
        lblPermissionTargetIds = new Label();
        PermissionTargetIds = new CheckedListBox();
        grpUserData = new GroupBox();
        tlpUserData = new TableLayoutPanel();
        label14 = new Label();
        txtOnyxEmployeeName = new TextBox();
        label13 = new Label();
        txtOnyxEmployeeNumber = new TextBox();
        label12 = new Label();
        lblUserName = new Label();
        txtUserName = new TextBox();
        lblFullName = new Label();
        txtFullName = new TextBox();
        lblRole = new Label();
        cboRole = new ComboBox();
        lblBranch = new Label();
        cboBranch = new ComboBox();
        lblStatus = new Label();
        cboStatus = new ComboBox();
        chkRequirePasswordReset = new CheckBox();
        label6 = new Label();
        txtForeignName = new TextBox();
        label11 = new Label();
        txtOnyxUserNumber = new TextBox();
        cboOnyxGroupNumber = new ComboBox();
        label15 = new Label();
        label18 = new Label();
        cboOnyxBranchNumber = new ComboBox();
        tlpOnyxManagerPair = new TableLayoutPanel();
        txtOnyxManager = new TextBox();
        txtOnyxManagerCode = new TextBox();
        pnlActions = new Panel();
        pnlHeader = new Panel();
        pnlToolbar = new FlowLayoutPanel();
        button1 = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        button2 = new Button();
        btnClose = new Button();
        btnFirst = new Button();
        btnPrevious = new Button();
        txtCurrentRecordNo = new TextBox();
        btnNext = new Button();
        btnLast = new Button();
        btnUndo = new Button();
        btnSearch = new Button();
        btnPrint = new Button();
        button4 = new Button();
        lblTitle = new Label();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        pnlUsers.SuspendLayout();
        tabUserDetails.SuspendLayout();
        tabUserData.SuspendLayout();
        userLookupV20.SuspendLayout();
        tabDevicesV20.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvOnyxHandheldDevices).BeginInit();
        deviceFieldsV20.SuspendLayout();
        tabPosOnyx.SuspendLayout();
        posLayoutHost.SuspendLayout();
        tabRolesV20.SuspendLayout();
        roleCatalogLayout.SuspendLayout();
        roleActions.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvRoleCatalog).BeginInit();
        roleLookupV20.SuspendLayout();
        tabPermissions.SuspendLayout();
        tlpPermissions.SuspendLayout();
        grpPermissionTree.SuspendLayout();
        grpModulePermissions.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvModulePermissions).BeginInit();
        securityPolicyV20.SuspendLayout();
        grpUserData.SuspendLayout();
        tlpUserData.SuspendLayout();
        tlpOnyxManagerPair.SuspendLayout();
        pnlActions.SuspendLayout();
        pnlHeader.SuspendLayout();
        pnlToolbar.SuspendLayout();
        SuspendLayout();
        // 
        // auditFooter
        // 
        auditFooter.AutoSize = true;
        auditFooter.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        auditFooter.BackColor = Color.FromArgb(50, 182, 255);
        auditFooter.Dock = DockStyle.Bottom;
        auditFooter.Font = new Font("Segoe UI", 10F);
        auditFooter.Location = new Point(0, 895);
        auditFooter.Name = "auditFooter";
        auditFooter.RightToLeft = RightToLeft.Yes;
        auditFooter.Size = new Size(1509, 76);
        auditFooter.TabIndex = 3;
        // 
        // splitMain
        // 
        splitMain.Dock = DockStyle.Fill;
        splitMain.FixedPanel = FixedPanel.Panel1;
        splitMain.Location = new Point(0, 80);
        splitMain.Margin = new Padding(3);
        splitMain.Name = "splitMain";
        // 
        // splitMain.Panel1
        // 
        splitMain.Panel1.Controls.Add(pnlUsers);
        splitMain.Panel1.Padding = new Padding(4);
        splitMain.Panel1.RightToLeft = RightToLeft.No;
        splitMain.Panel1MinSize = 390;
        // 
        // splitMain.Panel2
        // 
        splitMain.Panel2.Controls.Add(tabUserDetails);
        splitMain.Panel2.Controls.Add(grpUserData);
        splitMain.Panel2.Padding = new Padding(4);
        splitMain.Panel2.RightToLeft = RightToLeft.Yes;
        splitMain.Panel2MinSize = 650;
        splitMain.RightToLeft = RightToLeft.No;
        splitMain.Size = new Size(1509, 815);
        splitMain.SplitterDistance = 491;
        splitMain.SplitterWidth = 7;
        splitMain.TabIndex = 1;
        // 
        // pnlUsers
        // 
        pnlUsers.BackColor = Color.White;
        pnlUsers.Controls.Add(tvUserRecords);
        pnlUsers.Controls.Add(txtUserSearch);
        pnlUsers.Controls.Add(lblUsersList);
        pnlUsers.Dock = DockStyle.Fill;
        pnlUsers.Location = new Point(18, 21);
        pnlUsers.Margin = new Padding(3);
        pnlUsers.Name = "pnlUsers";
        pnlUsers.Padding = new Padding(4);
        pnlUsers.Size = new Size(455, 773);
        pnlUsers.TabIndex = 0;
        // 
        // tvUserRecords
        // 
        tvUserRecords.Dock = DockStyle.Fill;
        tvUserRecords.Location = new Point(18, 102);
        tvUserRecords.Name = "tvUserRecords";
        tvUserRecords.Size = new Size(419, 650);
        tvUserRecords.TabIndex = 3;
        // 
        // txtUserSearch
        // 
        txtUserSearch.Dock = DockStyle.Top;
        txtUserSearch.Location = new Point(18, 72);
        txtUserSearch.Margin = new Padding(3);
        txtUserSearch.Name = "txtUserSearch";
        txtUserSearch.PlaceholderText = "بحث باسم المستخدم أو الاسم الكامل";
        txtUserSearch.Size = new Size(419, 30);
        txtUserSearch.TabIndex = 0;
        // 
        // lblUsersList
        // 
        lblUsersList.Dock = DockStyle.Top;
        lblUsersList.Location = new Point(18, 21);
        lblUsersList.Name = "lblUsersList";
        lblUsersList.Size = new Size(419, 51);
        lblUsersList.TabIndex = 2;
        lblUsersList.Text = "قائمة المستخدمين";
        // 
        // tabUserDetails
        // 
        tabUserDetails.Controls.Add(tabUserData);
        tabUserDetails.Controls.Add(tabDevicesV20);
        tabUserDetails.Controls.Add(tabPosOnyx);
        tabUserDetails.Controls.Add(tabRolesV20);
        tabUserDetails.Controls.Add(tabPermissions);
        tabUserDetails.Dock = DockStyle.Fill;
        tabUserDetails.Font = new Font("Segoe UI", 9F);
        tabUserDetails.Location = new Point(18, 284);
        tabUserDetails.Margin = new Padding(3);
        tabUserDetails.Name = "tabUserDetails";
        tabUserDetails.RightToLeft = RightToLeft.Yes;
        tabUserDetails.RightToLeftLayout = true;
        tabUserDetails.SelectedIndex = 0;
        tabUserDetails.Size = new Size(975, 510);
        tabUserDetails.TabIndex = 0;
        // 
        // tabUserData
        // 
        tabUserData.AutoScroll = true;
        tabUserData.BackColor = Color.LightCyan;
        tabUserData.Controls.Add(userLookupV20);
        tabUserData.Location = new Point(4, 29);
        tabUserData.Margin = new Padding(3);
        tabUserData.Name = "tabUserData";
        tabUserData.Padding = new Padding(4);
        tabUserData.Size = new Size(967, 477);
        tabUserData.TabIndex = 0;
        tabUserData.Text = "بيانات المستخدمين";
        // 
        // userLookupV20
        // 
        userLookupV20.AutoSize = true;
        userLookupV20.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        userLookupV20.ColumnCount = 4;
        userLookupV20.ColumnStyles.Add(new ColumnStyle());
        userLookupV20.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        userLookupV20.ColumnStyles.Add(new ColumnStyle());
        userLookupV20.ColumnStyles.Add(new ColumnStyle());
        userLookupV20.Controls.Add(lblUserId, 0, 0);
        userLookupV20.Controls.Add(UserId, 1, 0);
        userLookupV20.Controls.Add(label7, 0, 1);
        userLookupV20.Controls.Add(txtAccessStartDate, 1, 1);
        userLookupV20.Controls.Add(label8, 2, 1);
        userLookupV20.Controls.Add(txtAccessEndDate, 3, 1);
        userLookupV20.Controls.Add(label9, 0, 2);
        userLookupV20.Controls.Add(txtAccessFromTime, 1, 2);
        userLookupV20.Controls.Add(label10, 2, 2);
        userLookupV20.Controls.Add(txtAccessToTime, 3, 2);
        userLookupV20.Controls.Add(label19, 0, 3);
        userLookupV20.Controls.Add(txtOnyxProfession, 1, 3);
        userLookupV20.Controls.Add(label20, 2, 3);
        userLookupV20.Controls.Add(txtOnyxUserComputerName, 3, 3);
        userLookupV20.Controls.Add(label21, 0, 5);
        userLookupV20.Controls.Add(txtOnyxUserStopReason, 1, 5);
        userLookupV20.Controls.Add(chkOnyxUserStopped, 1, 4);
        userLookupV20.Controls.Add(label16, 0, 6);
        userLookupV20.Controls.Add(textBox1, 1, 6);
        userLookupV20.Controls.Add(textBox2, 2, 6);
        userLookupV20.Controls.Add(textBox3, 1, 7);
        userLookupV20.Controls.Add(label17, 0, 7);
        userLookupV20.Dock = DockStyle.Top;
        userLookupV20.Location = new Point(21, 28);
        userLookupV20.Name = "userLookupV20";
        userLookupV20.RowCount = 8;
        userLookupV20.RowStyles.Add(new RowStyle());
        userLookupV20.RowStyles.Add(new RowStyle());
        userLookupV20.RowStyles.Add(new RowStyle());
        userLookupV20.RowStyles.Add(new RowStyle());
        userLookupV20.RowStyles.Add(new RowStyle());
        userLookupV20.RowStyles.Add(new RowStyle());
        userLookupV20.RowStyles.Add(new RowStyle());
        userLookupV20.RowStyles.Add(new RowStyle());
        userLookupV20.Size = new Size(925, 297);
        userLookupV20.TabIndex = 2;
        // 
        // lblUserId
        // 
        lblUserId.AutoSize = true;
        lblUserId.Dock = DockStyle.Fill;
        lblUserId.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblUserId.Location = new Point(676, 3);
        lblUserId.Margin = new Padding(3);
        lblUserId.Name = "lblUserId";
        lblUserId.Size = new Size(246, 33);
        lblUserId.TabIndex = 0;
        lblUserId.Text = "المستخدم";
        // 
        // UserId
        // 
        UserId.AccessibleName = "المستخدم";
        UserId.Dock = DockStyle.Fill;
        UserId.DropDownStyle = ComboBoxStyle.DropDownList;
        UserId.Font = new Font("Segoe UI", 11F);
        UserId.FormattingEnabled = true;
        UserId.Location = new Point(265, 3);
        UserId.Name = "UserId";
        UserId.Size = new Size(405, 33);
        UserId.TabIndex = 1;
        UserId.Tag = "FLD-SEC-USER";
        // 
        // label7
        // 
        label7.AutoSize = true;
        label7.Dock = DockStyle.Fill;
        label7.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label7.Location = new Point(676, 42);
        label7.Margin = new Padding(3);
        label7.Name = "label7";
        label7.Size = new Size(246, 32);
        label7.TabIndex = 2;
        label7.Text = "تاريخ البداية";
        // 
        // txtAccessStartDate
        // 
        txtAccessStartDate.Dock = DockStyle.Fill;
        txtAccessStartDate.Font = new Font("Segoe UI", 11F);
        txtAccessStartDate.Location = new Point(265, 42);
        txtAccessStartDate.Name = "txtAccessStartDate";
        txtAccessStartDate.Size = new Size(405, 32);
        txtAccessStartDate.TabIndex = 3;
        // 
        // label8
        // 
        label8.AutoSize = true;
        label8.Dock = DockStyle.Fill;
        label8.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label8.Location = new Point(134, 42);
        label8.Margin = new Padding(3);
        label8.Name = "label8";
        label8.Size = new Size(125, 32);
        label8.TabIndex = 4;
        label8.Text = "تاريخ النهاية";
        // 
        // txtAccessEndDate
        // 
        txtAccessEndDate.Dock = DockStyle.Fill;
        txtAccessEndDate.Font = new Font("Segoe UI", 11F);
        txtAccessEndDate.Location = new Point(3, 42);
        txtAccessEndDate.Name = "txtAccessEndDate";
        txtAccessEndDate.Size = new Size(125, 32);
        txtAccessEndDate.TabIndex = 5;
        // 
        // label9
        // 
        label9.AutoSize = true;
        label9.Dock = DockStyle.Fill;
        label9.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label9.Location = new Point(676, 80);
        label9.Margin = new Padding(3);
        label9.Name = "label9";
        label9.Size = new Size(246, 32);
        label9.TabIndex = 6;
        label9.Text = "من الوقت";
        // 
        // txtAccessFromTime
        // 
        txtAccessFromTime.Dock = DockStyle.Fill;
        txtAccessFromTime.Font = new Font("Segoe UI", 11F);
        txtAccessFromTime.Location = new Point(265, 80);
        txtAccessFromTime.Name = "txtAccessFromTime";
        txtAccessFromTime.Size = new Size(405, 32);
        txtAccessFromTime.TabIndex = 7;
        // 
        // label10
        // 
        label10.AutoSize = true;
        label10.Dock = DockStyle.Fill;
        label10.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label10.Location = new Point(134, 80);
        label10.Margin = new Padding(3);
        label10.Name = "label10";
        label10.Size = new Size(125, 32);
        label10.TabIndex = 8;
        label10.Text = "إلى الوقت";
        // 
        // txtAccessToTime
        // 
        txtAccessToTime.Dock = DockStyle.Fill;
        txtAccessToTime.Font = new Font("Segoe UI", 11F);
        txtAccessToTime.Location = new Point(3, 80);
        txtAccessToTime.Name = "txtAccessToTime";
        txtAccessToTime.Size = new Size(125, 32);
        txtAccessToTime.TabIndex = 9;
        // 
        // label19
        // 
        label19.AutoSize = true;
        label19.Dock = DockStyle.Fill;
        label19.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label19.Location = new Point(676, 118);
        label19.Margin = new Padding(3);
        label19.Name = "label19";
        label19.Size = new Size(246, 32);
        label19.TabIndex = 10;
        label19.Text = "المهنة";
        // 
        // txtOnyxProfession
        // 
        txtOnyxProfession.Dock = DockStyle.Fill;
        txtOnyxProfession.Font = new Font("Segoe UI", 11F);
        txtOnyxProfession.Location = new Point(265, 118);
        txtOnyxProfession.Name = "txtOnyxProfession";
        txtOnyxProfession.Size = new Size(405, 32);
        txtOnyxProfession.TabIndex = 11;
        // 
        // label20
        // 
        label20.AutoSize = true;
        label20.Dock = DockStyle.Fill;
        label20.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label20.Location = new Point(134, 118);
        label20.Margin = new Padding(3);
        label20.Name = "label20";
        label20.Size = new Size(125, 32);
        label20.TabIndex = 12;
        label20.Text = "اسم الجهاز";
        // 
        // txtOnyxUserComputerName
        // 
        txtOnyxUserComputerName.Dock = DockStyle.Fill;
        txtOnyxUserComputerName.Font = new Font("Segoe UI", 11F);
        txtOnyxUserComputerName.Location = new Point(3, 118);
        txtOnyxUserComputerName.Name = "txtOnyxUserComputerName";
        txtOnyxUserComputerName.Size = new Size(125, 32);
        txtOnyxUserComputerName.TabIndex = 13;
        // 
        // label21
        // 
        label21.AutoSize = true;
        label21.Dock = DockStyle.Fill;
        label21.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label21.Location = new Point(676, 186);
        label21.Margin = new Padding(3);
        label21.Name = "label21";
        label21.Size = new Size(246, 32);
        label21.TabIndex = 14;
        label21.Text = "سبب التوقيف";
        // 
        // txtOnyxUserStopReason
        // 
        userLookupV20.SetColumnSpan(txtOnyxUserStopReason, 3);
        txtOnyxUserStopReason.Dock = DockStyle.Fill;
        txtOnyxUserStopReason.Font = new Font("Segoe UI", 11F);
        txtOnyxUserStopReason.Location = new Point(3, 186);
        txtOnyxUserStopReason.Name = "txtOnyxUserStopReason";
        txtOnyxUserStopReason.Size = new Size(667, 32);
        txtOnyxUserStopReason.TabIndex = 15;
        // 
        // chkOnyxUserStopped
        // 
        chkOnyxUserStopped.AutoSize = true;
        chkOnyxUserStopped.Dock = DockStyle.Fill;
        chkOnyxUserStopped.Location = new Point(265, 156);
        chkOnyxUserStopped.Name = "chkOnyxUserStopped";
        chkOnyxUserStopped.Size = new Size(405, 24);
        chkOnyxUserStopped.TabIndex = 14;
        chkOnyxUserStopped.Text = "موقف";
        chkOnyxUserStopped.UseVisualStyleBackColor = true;
        // 
        // label16
        // 
        label16.AutoSize = true;
        label16.Dock = DockStyle.Fill;
        label16.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label16.Location = new Point(676, 224);
        label16.Margin = new Padding(3);
        label16.Name = "label16";
        label16.Size = new Size(246, 32);
        label16.TabIndex = 16;
        label16.Text = "الصلاحيات منسوخة من المستخدم";
        // 
        // textBox1
        // 
        textBox1.Dock = DockStyle.Fill;
        textBox1.Font = new Font("Segoe UI", 11F);
        textBox1.Location = new Point(265, 224);
        textBox1.Name = "textBox1";
        textBox1.ReadOnly = true;
        textBox1.Size = new Size(405, 32);
        textBox1.TabIndex = 17;
        // 
        // textBox2
        // 
        textBox2.Dock = DockStyle.Fill;
        textBox2.Font = new Font("Segoe UI", 11F);
        textBox2.Location = new Point(134, 224);
        textBox2.Name = "textBox2";
        textBox2.ReadOnly = true;
        textBox2.Size = new Size(125, 32);
        textBox2.TabIndex = 18;
        // 
        // textBox3
        // 
        textBox3.Dock = DockStyle.Fill;
        textBox3.Font = new Font("Segoe UI", 11F);
        textBox3.Location = new Point(265, 262);
        textBox3.Name = "textBox3";
        textBox3.ReadOnly = true;
        textBox3.Size = new Size(405, 32);
        textBox3.TabIndex = 19;
        // 
        // label17
        // 
        label17.AutoSize = true;
        label17.Dock = DockStyle.Fill;
        label17.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label17.Location = new Point(676, 262);
        label17.Margin = new Padding(3);
        label17.Name = "label17";
        label17.Size = new Size(246, 32);
        label17.TabIndex = 20;
        label17.Text = "تغيير كلمة السر";
        // 
        // tabDevicesV20
        // 
        tabDevicesV20.AutoScroll = true;
        tabDevicesV20.BackColor = Color.LightCyan;
        tabDevicesV20.Controls.Add(dgvOnyxHandheldDevices);
        tabDevicesV20.Controls.Add(deviceFieldsV20);
        tabDevicesV20.Location = new Point(4, 29);
        tabDevicesV20.Name = "tabDevicesV20";
        tabDevicesV20.Padding = new Padding(4);
        tabDevicesV20.Size = new Size(967, 483);
        tabDevicesV20.TabIndex = 4;
        tabDevicesV20.Text = "الأجهزة الكفية";
        // 
        // dgvOnyxHandheldDevices
        // 
        dgvOnyxHandheldDevices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = SystemColors.Control;
        dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
        dgvOnyxHandheldDevices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        dgvOnyxHandheldDevices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvOnyxHandheldDevices.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4 });
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = SystemColors.Window;
        dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
        dgvOnyxHandheldDevices.DefaultCellStyle = dataGridViewCellStyle2;
        dgvOnyxHandheldDevices.Dock = DockStyle.Fill;
        dgvOnyxHandheldDevices.Location = new Point(16, 85);
        dgvOnyxHandheldDevices.Name = "dgvOnyxHandheldDevices";
        dgvOnyxHandheldDevices.RowHeadersWidth = 51;
        dgvOnyxHandheldDevices.Size = new Size(935, 382);
        dgvOnyxHandheldDevices.TabIndex = 2;
        // 
        // Column1
        // 
        Column1.HeaderText = "اسم الجهاز";
        Column1.MinimumWidth = 6;
        Column1.Name = "Column1";
        Column1.Resizable = DataGridViewTriState.True;
        // 
        // Column2
        // 
        Column2.HeaderText = "الرقم التسلسلي";
        Column2.MinimumWidth = 6;
        Column2.Name = "Column2";
        // 
        // Column3
        // 
        Column3.FillWeight = 18F;
        Column3.HeaderText = "موقف";
        Column3.MinimumWidth = 6;
        Column3.Name = "Column3";
        Column3.Resizable = DataGridViewTriState.True;
        Column3.SortMode = DataGridViewColumnSortMode.Automatic;
        // 
        // Column4
        // 
        Column4.HeaderText = "سبب التوقيف";
        Column4.MinimumWidth = 6;
        Column4.Name = "Column4";
        // 
        // deviceFieldsV20
        // 
        deviceFieldsV20.AutoSize = true;
        deviceFieldsV20.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        deviceFieldsV20.ColumnCount = 2;
        deviceFieldsV20.ColumnStyles.Add(new ColumnStyle());
        deviceFieldsV20.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        deviceFieldsV20.Controls.Add(lblDeviceV20, 0, 0);
        deviceFieldsV20.Controls.Add(DeviceId, 1, 0);
        deviceFieldsV20.Controls.Add(checkBox1, 0, 3);
        deviceFieldsV20.Dock = DockStyle.Top;
        deviceFieldsV20.Location = new Point(16, 16);
        deviceFieldsV20.Name = "deviceFieldsV20";
        deviceFieldsV20.RowCount = 4;
        deviceFieldsV20.RowStyles.Add(new RowStyle());
        deviceFieldsV20.RowStyles.Add(new RowStyle());
        deviceFieldsV20.RowStyles.Add(new RowStyle());
        deviceFieldsV20.RowStyles.Add(new RowStyle());
        deviceFieldsV20.Size = new Size(935, 69);
        deviceFieldsV20.TabIndex = 1;
        // 
        // lblDeviceV20
        // 
        lblDeviceV20.AutoSize = true;
        lblDeviceV20.Dock = DockStyle.Fill;
        lblDeviceV20.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblDeviceV20.Location = new Point(861, 3);
        lblDeviceV20.Margin = new Padding(3);
        lblDeviceV20.Name = "lblDeviceV20";
        lblDeviceV20.Size = new Size(71, 33);
        lblDeviceV20.TabIndex = 0;
        lblDeviceV20.Text = "الجهاز";
        // 
        // DeviceId
        // 
        DeviceId.AccessibleName = "الجهاز";
        DeviceId.Dock = DockStyle.Fill;
        DeviceId.DropDownStyle = ComboBoxStyle.DropDownList;
        DeviceId.Font = new Font("Segoe UI", 11F);
        DeviceId.FormattingEnabled = true;
        DeviceId.Location = new Point(3, 3);
        DeviceId.Name = "DeviceId";
        DeviceId.Size = new Size(852, 33);
        DeviceId.TabIndex = 1;
        DeviceId.Tag = "FLD-SEC-DEVICE";
        // 
        // checkBox1
        // 
        checkBox1.AutoSize = true;
        checkBox1.Dock = DockStyle.Fill;
        checkBox1.Location = new Point(861, 42);
        checkBox1.Name = "checkBox1";
        checkBox1.Size = new Size(71, 24);
        checkBox1.TabIndex = 6;
        checkBox1.Text = "موقف";
        checkBox1.UseVisualStyleBackColor = true;
        // 
        // tabPosOnyx
        // 
        tabPosOnyx.AutoScroll = true;
        tabPosOnyx.BackColor = Color.LightCyan;
        tabPosOnyx.Controls.Add(posLayoutHost);
        tabPosOnyx.Location = new Point(4, 29);
        tabPosOnyx.Name = "tabPosOnyx";
        tabPosOnyx.Padding = new Padding(4);
        tabPosOnyx.Size = new Size(967, 483);
        tabPosOnyx.TabIndex = 5;
        tabPosOnyx.Text = "نقاط بيع";
        // 
        // posLayoutHost
        // 
        posLayoutHost.Anchor = AnchorStyles.None;
        posLayoutHost.AutoSize = true;
        posLayoutHost.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        posLayoutHost.ColumnCount = 4;
        posLayoutHost.ColumnStyles.Add(new ColumnStyle());
        posLayoutHost.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        posLayoutHost.ColumnStyles.Add(new ColumnStyle());
        posLayoutHost.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        posLayoutHost.Controls.Add(label1, 0, 0);
        posLayoutHost.Controls.Add(cboPosConnection, 1, 0);
        posLayoutHost.Controls.Add(txtPosNumber, 1, 1);
        posLayoutHost.Controls.Add(label4, 0, 2);
        posLayoutHost.Controls.Add(label5, 0, 3);
        posLayoutHost.Controls.Add(txtBarcodePath, 1, 3);
        posLayoutHost.Controls.Add(label2, 0, 1);
        posLayoutHost.Controls.Add(label3, 2, 1);
        posLayoutHost.Controls.Add(txtPosName, 3, 1);
        posLayoutHost.Controls.Add(txtBarcodePrinter, 1, 2);
        posLayoutHost.Location = new Point(145, 214);
        posLayoutHost.MaximumSize = new Size(677, 0);
        posLayoutHost.Name = "posLayoutHost";
        posLayoutHost.RowCount = 4;
        posLayoutHost.RowStyles.Add(new RowStyle());
        posLayoutHost.RowStyles.Add(new RowStyle());
        posLayoutHost.RowStyles.Add(new RowStyle());
        posLayoutHost.RowStyles.Add(new RowStyle());
        posLayoutHost.Size = new Size(677, 153);
        posLayoutHost.TabIndex = 1;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Dock = DockStyle.Fill;
        label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label1.Location = new Point(485, 3);
        label1.Margin = new Padding(3);
        label1.Name = "label1";
        label1.Size = new Size(189, 33);
        label1.TabIndex = 0;
        label1.Text = "نوع الاتصال";
        // 
        // cboPosConnection
        // 
        cboPosConnection.AccessibleName = "نوع الاتصال";
        posLayoutHost.SetColumnSpan(cboPosConnection, 3);
        cboPosConnection.Dock = DockStyle.Fill;
        cboPosConnection.DropDownStyle = ComboBoxStyle.DropDownList;
        cboPosConnection.Font = new Font("Segoe UI", 11F);
        cboPosConnection.FormattingEnabled = true;
        cboPosConnection.Location = new Point(3, 3);
        cboPosConnection.Name = "cboPosConnection";
        cboPosConnection.Size = new Size(476, 33);
        cboPosConnection.TabIndex = 1;
        cboPosConnection.Tag = "POS-CONNECTION";
        // 
        // txtPosNumber
        // 
        txtPosNumber.Dock = DockStyle.Fill;
        txtPosNumber.Font = new Font("Segoe UI", 11F);
        txtPosNumber.Location = new Point(306, 42);
        txtPosNumber.Name = "txtPosNumber";
        txtPosNumber.Size = new Size(173, 32);
        txtPosNumber.TabIndex = 2;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Dock = DockStyle.Fill;
        label4.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label4.Location = new Point(485, 80);
        label4.Margin = new Padding(3);
        label4.Name = "label4";
        label4.Size = new Size(189, 32);
        label4.TabIndex = 8;
        label4.Text = "اسم طابعة الباركود";
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Dock = DockStyle.Fill;
        label5.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label5.Location = new Point(485, 118);
        label5.Margin = new Padding(3);
        label5.Name = "label5";
        label5.Size = new Size(189, 32);
        label5.TabIndex = 9;
        label5.Text = "مسار ملف طابعة الباركود";
        // 
        // txtBarcodePath
        // 
        posLayoutHost.SetColumnSpan(txtBarcodePath, 3);
        txtBarcodePath.Dock = DockStyle.Fill;
        txtBarcodePath.Font = new Font("Segoe UI", 11F);
        txtBarcodePath.Location = new Point(3, 118);
        txtBarcodePath.Name = "txtBarcodePath";
        txtBarcodePath.RightToLeft = RightToLeft.No;
        txtBarcodePath.Size = new Size(476, 32);
        txtBarcodePath.TabIndex = 5;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Dock = DockStyle.Fill;
        label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label2.Location = new Point(485, 42);
        label2.Margin = new Padding(3);
        label2.Name = "label2";
        label2.Size = new Size(189, 32);
        label2.TabIndex = 6;
        label2.Text = "رقم نقطة البيع";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Dock = DockStyle.Fill;
        label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label3.Location = new Point(182, 42);
        label3.Margin = new Padding(3);
        label3.Name = "label3";
        label3.Size = new Size(118, 32);
        label3.TabIndex = 7;
        label3.Text = "اسم نقطة البيع";
        // 
        // txtPosName
        // 
        txtPosName.Dock = DockStyle.Fill;
        txtPosName.Font = new Font("Segoe UI", 11F);
        txtPosName.Location = new Point(3, 42);
        txtPosName.Name = "txtPosName";
        txtPosName.Size = new Size(173, 32);
        txtPosName.TabIndex = 3;
        // 
        // txtBarcodePrinter
        // 
        posLayoutHost.SetColumnSpan(txtBarcodePrinter, 3);
        txtBarcodePrinter.Dock = DockStyle.Fill;
        txtBarcodePrinter.Font = new Font("Segoe UI", 11F);
        txtBarcodePrinter.Location = new Point(3, 80);
        txtBarcodePrinter.Name = "txtBarcodePrinter";
        txtBarcodePrinter.Size = new Size(476, 32);
        txtBarcodePrinter.TabIndex = 4;
        // 
        // tabRolesV20
        // 
        tabRolesV20.AutoScroll = true;
        tabRolesV20.BackColor = Color.LightCyan;
        tabRolesV20.Controls.Add(roleCatalogLayout);
        tabRolesV20.Controls.Add(roleLookupV20);
        tabRolesV20.Location = new Point(4, 29);
        tabRolesV20.Name = "tabRolesV20";
        tabRolesV20.Padding = new Padding(4);
        tabRolesV20.Size = new Size(967, 483);
        tabRolesV20.TabIndex = 2;
        tabRolesV20.Text = "الأدوار";
        // 
        // roleCatalogLayout
        // 
        roleCatalogLayout.ColumnCount = 1;
        roleCatalogLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        roleCatalogLayout.Controls.Add(roleActions, 0, 0);
        roleCatalogLayout.Controls.Add(dgvRoleCatalog, 0, 1);
        roleCatalogLayout.Controls.Add(lblRoleState, 0, 2);
        roleCatalogLayout.Dock = DockStyle.Fill;
        roleCatalogLayout.Location = new Point(16, 55);
        roleCatalogLayout.Name = "roleCatalogLayout";
        roleCatalogLayout.RowCount = 3;
        roleCatalogLayout.RowStyles.Add(new RowStyle());
        roleCatalogLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        roleCatalogLayout.RowStyles.Add(new RowStyle());
        roleCatalogLayout.Size = new Size(935, 412);
        roleCatalogLayout.TabIndex = 0;
        // 
        // roleActions
        // 
        roleActions.AutoSize = true;
        roleActions.Controls.Add(btnRoleAdd);
        roleActions.Controls.Add(btnRoleRemove);
        roleActions.Controls.Add(btnRoleValidate);
        roleActions.Controls.Add(btnRoleSave);
        roleActions.Controls.Add(btnRoleClear);
        roleActions.Dock = DockStyle.Fill;
        roleActions.Location = new Point(3, 3);
        roleActions.Name = "roleActions";
        roleActions.RightToLeft = RightToLeft.Yes;
        roleActions.Size = new Size(929, 36);
        roleActions.TabIndex = 0;
        // 
        // btnRoleAdd
        // 
        btnRoleAdd.AutoSize = true;
        btnRoleAdd.Location = new Point(842, 3);
        btnRoleAdd.Name = "btnRoleAdd";
        btnRoleAdd.Size = new Size(84, 30);
        btnRoleAdd.TabIndex = 0;
        btnRoleAdd.Text = "إضافة دور";
        // 
        // btnRoleRemove
        // 
        btnRoleRemove.AutoSize = true;
        btnRoleRemove.Location = new Point(709, 3);
        btnRoleRemove.Name = "btnRoleRemove";
        btnRoleRemove.Size = new Size(127, 30);
        btnRoleRemove.TabIndex = 1;
        btnRoleRemove.Text = "إزالة من المسودة";
        // 
        // btnRoleValidate
        // 
        btnRoleValidate.AutoSize = true;
        btnRoleValidate.Location = new Point(628, 3);
        btnRoleValidate.Name = "btnRoleValidate";
        btnRoleValidate.Size = new Size(75, 30);
        btnRoleValidate.TabIndex = 2;
        btnRoleValidate.Text = "تحقق";
        // 
        // btnRoleSave
        // 
        btnRoleSave.AutoSize = true;
        btnRoleSave.Enabled = false;
        btnRoleSave.Location = new Point(530, 3);
        btnRoleSave.Name = "btnRoleSave";
        btnRoleSave.Size = new Size(92, 30);
        btnRoleSave.TabIndex = 3;
        btnRoleSave.Text = "حفظ الأدوار";
        // 
        // btnRoleClear
        // 
        btnRoleClear.Location = new Point(449, 3);
        btnRoleClear.Name = "btnRoleClear";
        btnRoleClear.Size = new Size(75, 30);
        btnRoleClear.TabIndex = 4;
        // 
        // dgvRoleCatalog
        // 
        dgvRoleCatalog.AllowUserToAddRows = false;
        dgvRoleCatalog.AllowUserToDeleteRows = false;
        dgvRoleCatalog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = SystemColors.Control;
        dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
        dgvRoleCatalog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
        dgvRoleCatalog.ColumnHeadersHeight = 29;
        dgvRoleCatalog.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewComboBoxColumn1, dataGridViewComboBoxColumn2, dataGridViewCheckBoxColumn1 });
        dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle4.BackColor = SystemColors.Window;
        dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
        dgvRoleCatalog.DefaultCellStyle = dataGridViewCellStyle4;
        dgvRoleCatalog.Dock = DockStyle.Fill;
        dgvRoleCatalog.Location = new Point(3, 45);
        dgvRoleCatalog.MultiSelect = false;
        dgvRoleCatalog.Name = "dgvRoleCatalog";
        dgvRoleCatalog.RowHeadersWidth = 51;
        dgvRoleCatalog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvRoleCatalog.Size = new Size(929, 344);
        dgvRoleCatalog.TabIndex = 1;
        // 
        // dataGridViewTextBoxColumn1
        // 
        dataGridViewTextBoxColumn1.MinimumWidth = 6;
        dataGridViewTextBoxColumn1.Name = "RoleId";
        dataGridViewTextBoxColumn1.Width = 239;
        // 
        // dataGridViewTextBoxColumn2
        // 
        dataGridViewTextBoxColumn2.MinimumWidth = 6;
        dataGridViewTextBoxColumn2.Name = "Code";
        dataGridViewTextBoxColumn2.Width = 239;
        // 
        // dataGridViewTextBoxColumn3
        // 
        dataGridViewTextBoxColumn3.MinimumWidth = 6;
        dataGridViewTextBoxColumn3.Name = "NameAr";
        dataGridViewTextBoxColumn3.Width = 239;
        // 
        // dataGridViewTextBoxColumn4
        // 
        dataGridViewTextBoxColumn4.MinimumWidth = 6;
        dataGridViewTextBoxColumn4.Name = "NameEn";
        dataGridViewTextBoxColumn4.Width = 239;
        // 
        // dataGridViewTextBoxColumn5
        // 
        dataGridViewTextBoxColumn5.MinimumWidth = 6;
        dataGridViewTextBoxColumn5.Name = "Description";
        dataGridViewTextBoxColumn5.Width = 239;
        // 
        // dataGridViewComboBoxColumn1
        // 
        dataGridViewComboBoxColumn1.DisplayMember = "Label";
        dataGridViewComboBoxColumn1.ValueMember = "Id";
        dataGridViewComboBoxColumn1.MinimumWidth = 6;
        dataGridViewComboBoxColumn1.Name = "CompanyId";
        dataGridViewComboBoxColumn1.Width = 238;
        // 
        // dataGridViewComboBoxColumn2
        // 
        dataGridViewComboBoxColumn2.DisplayMember = "Label";
        dataGridViewComboBoxColumn2.ValueMember = "Id";
        dataGridViewComboBoxColumn2.MinimumWidth = 6;
        dataGridViewComboBoxColumn2.Name = "Status";
        dataGridViewComboBoxColumn2.Width = 238;
        // 
        // dataGridViewCheckBoxColumn1
        // 
        dataGridViewCheckBoxColumn1.MinimumWidth = 6;
        dataGridViewCheckBoxColumn1.Name = "IsSystem";
        dataGridViewCheckBoxColumn1.Width = 228;
        // 
        // lblRoleState
        // 
        lblRoleState.AutoSize = true;
        lblRoleState.Dock = DockStyle.Fill;
        lblRoleState.Location = new Point(3, 392);
        lblRoleState.Name = "lblRoleState";
        lblRoleState.Size = new Size(929, 20);
        lblRoleState.TabIndex = 2;
        lblRoleState.Text = "مسودة أدوار محلية؛ لم تربط خدمة الحفظ.";
        // 
        // roleLookupV20
        // 
        roleLookupV20.AutoSize = true;
        roleLookupV20.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        roleLookupV20.ColumnCount = 2;
        roleLookupV20.ColumnStyles.Add(new ColumnStyle());
        roleLookupV20.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        roleLookupV20.Controls.Add(lblRoleId, 0, 0);
        roleLookupV20.Controls.Add(RoleId, 1, 0);
        roleLookupV20.Dock = DockStyle.Top;
        roleLookupV20.Location = new Point(16, 16);
        roleLookupV20.Name = "roleLookupV20";
        roleLookupV20.RowCount = 1;
        roleLookupV20.RowStyles.Add(new RowStyle());
        roleLookupV20.Size = new Size(935, 39);
        roleLookupV20.TabIndex = 0;
        // 
        // lblRoleId
        // 
        lblRoleId.AutoSize = true;
        lblRoleId.Dock = DockStyle.Fill;
        lblRoleId.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblRoleId.Location = new Point(887, 3);
        lblRoleId.Margin = new Padding(3);
        lblRoleId.Name = "lblRoleId";
        lblRoleId.Size = new Size(45, 33);
        lblRoleId.TabIndex = 0;
        lblRoleId.Text = "الدور";
        // 
        // RoleId
        // 
        RoleId.AccessibleName = "الدور";
        RoleId.Dock = DockStyle.Fill;
        RoleId.DropDownStyle = ComboBoxStyle.DropDownList;
        RoleId.Font = new Font("Segoe UI", 11F);
        RoleId.FormattingEnabled = true;
        RoleId.Location = new Point(3, 3);
        RoleId.Name = "RoleId";
        RoleId.Size = new Size(878, 33);
        RoleId.TabIndex = 1;
        RoleId.Tag = "FLD-SEC-ROLE";
        // 
        // tabPermissions
        // 
        tabPermissions.AutoScroll = true;
        tabPermissions.AutoScrollMinSize = new Size(0, 505);
        tabPermissions.BackColor = Color.LightCyan;
        tabPermissions.Controls.Add(tlpPermissions);
        tabPermissions.Controls.Add(securityPolicyV20);
        tabPermissions.Location = new Point(4, 29);
        tabPermissions.Margin = new Padding(3);
        tabPermissions.Name = "tabPermissions";
        tabPermissions.Padding = new Padding(4);
        tabPermissions.Size = new Size(967, 483);
        tabPermissions.TabIndex = 1;
        tabPermissions.Text = "الصلاحيات";
        // 
        // tlpPermissions
        // 
        tlpPermissions.ColumnCount = 2;
        tlpPermissions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
        tlpPermissions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
        tlpPermissions.Controls.Add(grpPermissionTree, 0, 0);
        tlpPermissions.Controls.Add(grpModulePermissions, 1, 0);
        tlpPermissions.Dock = DockStyle.Fill;
        tlpPermissions.Location = new Point(21, 28);
        tlpPermissions.Margin = new Padding(3);
        tlpPermissions.Name = "tlpPermissions";
        tlpPermissions.RowCount = 1;
        tlpPermissions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tlpPermissions.Size = new Size(904, 449);
        tlpPermissions.TabIndex = 0;
        // 
        // grpPermissionTree
        // 
        grpPermissionTree.Controls.Add(tvPermissions);
        grpPermissionTree.Controls.Add(chkSelectAllPermissions);
        grpPermissionTree.Dock = DockStyle.Fill;
        grpPermissionTree.Location = new Point(528, 4);
        grpPermissionTree.Margin = new Padding(3);
        grpPermissionTree.Name = "grpPermissionTree";
        grpPermissionTree.Padding = new Padding(3, 4, 3, 4);
        grpPermissionTree.Size = new Size(373, 441);
        grpPermissionTree.TabIndex = 0;
        grpPermissionTree.TabStop = false;
        grpPermissionTree.Text = "شجرة الصلاحيات";
        // 
        // tvPermissions
        // 
        tvPermissions.CheckBoxes = true;
        tvPermissions.Dock = DockStyle.Fill;
        tvPermissions.FullRowSelect = true;
        tvPermissions.HideSelection = false;
        tvPermissions.Location = new Point(3, 48);
        tvPermissions.Margin = new Padding(3);
        tvPermissions.Name = "tvPermissions";
        tvPermissions.RightToLeft = RightToLeft.Yes;
        tvPermissions.RightToLeftLayout = true;
        tvPermissions.ShowLines = false;
        tvPermissions.Size = new Size(367, 389);
        tvPermissions.TabIndex = 1;
        // 
        // chkSelectAllPermissions
        // 
        chkSelectAllPermissions.AutoSize = true;
        chkSelectAllPermissions.Dock = DockStyle.Top;
        chkSelectAllPermissions.Location = new Point(3, 24);
        chkSelectAllPermissions.Margin = new Padding(3);
        chkSelectAllPermissions.Name = "chkSelectAllPermissions";
        chkSelectAllPermissions.Size = new Size(367, 24);
        chkSelectAllPermissions.TabIndex = 0;
        chkSelectAllPermissions.Text = "تحديد كل الصلاحيات الظاهرة";
        // 
        // grpModulePermissions
        // 
        grpModulePermissions.Controls.Add(dgvModulePermissions);
        grpModulePermissions.Dock = DockStyle.Fill;
        grpModulePermissions.Location = new Point(3, 4);
        grpModulePermissions.Margin = new Padding(3);
        grpModulePermissions.Name = "grpModulePermissions";
        grpModulePermissions.Padding = new Padding(3, 4, 3, 4);
        grpModulePermissions.Size = new Size(519, 441);
        grpModulePermissions.TabIndex = 1;
        grpModulePermissions.TabStop = false;
        grpModulePermissions.Text = "صلاحيات الوحدات";
        // 
        // dgvModulePermissions
        // 
        dgvModulePermissions.ColumnHeadersHeight = 29;
        dgvModulePermissions.Columns.AddRange(new DataGridViewColumn[] { colModule, colView, colAdd, colEdit, colPrint });
        dgvModulePermissions.Dock = DockStyle.Fill;
        dgvModulePermissions.Location = new Point(3, 24);
        dgvModulePermissions.Margin = new Padding(3);
        dgvModulePermissions.Name = "dgvModulePermissions";
        dgvModulePermissions.RowHeadersWidth = 51;
        dgvModulePermissions.Size = new Size(513, 413);
        dgvModulePermissions.TabIndex = 0;
        // 
        // colModule
        // 
        colModule.FillWeight = 145F;
        colModule.HeaderText = "الوحدة";
        colModule.MinimumWidth = 6;
        colModule.Name = "colModule";
        colModule.ReadOnly = true;
        colModule.Width = 125;
        // 
        // colView
        // 
        colView.HeaderText = "عرض";
        colView.MinimumWidth = 6;
        colView.Name = "colView";
        colView.Width = 125;
        // 
        // colAdd
        // 
        colAdd.HeaderText = "إضافة";
        colAdd.MinimumWidth = 6;
        colAdd.Name = "colAdd";
        colAdd.Width = 125;
        // 
        // colEdit
        // 
        colEdit.HeaderText = "تعديل";
        colEdit.MinimumWidth = 6;
        colEdit.Name = "colEdit";
        colEdit.Width = 125;
        // 
        // colPrint
        // 
        colPrint.HeaderText = "طباعة";
        colPrint.MinimumWidth = 6;
        colPrint.Name = "colPrint";
        colPrint.Width = 125;
        // 
        // securityPolicyV20
        // 
        securityPolicyV20.AutoSize = true;
        securityPolicyV20.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        securityPolicyV20.ColumnCount = 2;
        securityPolicyV20.ColumnStyles.Add(new ColumnStyle());
        securityPolicyV20.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        securityPolicyV20.Controls.Add(lblSettingValue__SET_SEC_001, 0, 0);
        securityPolicyV20.Controls.Add(SettingValue__SET_SEC_001, 1, 0);
        securityPolicyV20.Controls.Add(lblSettingValue__SET_SEC_002, 0, 1);
        securityPolicyV20.Controls.Add(SettingValue__SET_SEC_002, 1, 1);
        securityPolicyV20.Controls.Add(lblSettingValue__SET_SEC_003, 0, 2);
        securityPolicyV20.Controls.Add(SettingValue__SET_SEC_003, 1, 2);
        securityPolicyV20.Controls.Add(lblPermissionTargetIds, 0, 3);
        securityPolicyV20.Controls.Add(PermissionTargetIds, 1, 3);
        securityPolicyV20.Dock = DockStyle.Bottom;
        securityPolicyV20.Location = new Point(21, 477);
        securityPolicyV20.Name = "securityPolicyV20";
        securityPolicyV20.RightToLeft = RightToLeft.Yes;
        securityPolicyV20.RowCount = 4;
        securityPolicyV20.RowStyles.Add(new RowStyle());
        securityPolicyV20.RowStyles.Add(new RowStyle());
        securityPolicyV20.RowStyles.Add(new RowStyle());
        securityPolicyV20.RowStyles.Add(new RowStyle());
        securityPolicyV20.Size = new Size(904, 218);
        securityPolicyV20.TabIndex = 1;
        // 
        // lblSettingValue__SET_SEC_001
        // 
        lblSettingValue__SET_SEC_001.AutoSize = true;
        lblSettingValue__SET_SEC_001.Dock = DockStyle.Fill;
        lblSettingValue__SET_SEC_001.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblSettingValue__SET_SEC_001.ForeColor = Color.FromArgb(16, 24, 40);
        lblSettingValue__SET_SEC_001.Location = new Point(745, 3);
        lblSettingValue__SET_SEC_001.Margin = new Padding(3);
        lblSettingValue__SET_SEC_001.Name = "lblSettingValue__SET_SEC_001";
        lblSettingValue__SET_SEC_001.Size = new Size(156, 33);
        lblSettingValue__SET_SEC_001.TabIndex = 0;
        lblSettingValue__SET_SEC_001.Text = "قرار الصلاحية";
        lblSettingValue__SET_SEC_001.TextAlign = ContentAlignment.MiddleRight;
        // 
        // SettingValue__SET_SEC_001
        // 
        SettingValue__SET_SEC_001.AccessibleName = "قرار الصلاحية";
        SettingValue__SET_SEC_001.Dock = DockStyle.Fill;
        SettingValue__SET_SEC_001.DropDownStyle = ComboBoxStyle.DropDownList;
        SettingValue__SET_SEC_001.Font = new Font("Segoe UI", 11F);
        SettingValue__SET_SEC_001.Items.AddRange(new object[] { "DENY", "ALLOW" });
        SettingValue__SET_SEC_001.Location = new Point(3, 3);
        SettingValue__SET_SEC_001.Name = "SettingValue__SET_SEC_001";
        SettingValue__SET_SEC_001.RightToLeft = RightToLeft.Yes;
        SettingValue__SET_SEC_001.Size = new Size(736, 33);
        SettingValue__SET_SEC_001.TabIndex = 1;
        // 
        // lblSettingValue__SET_SEC_002
        // 
        lblSettingValue__SET_SEC_002.AutoSize = true;
        lblSettingValue__SET_SEC_002.Dock = DockStyle.Fill;
        lblSettingValue__SET_SEC_002.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblSettingValue__SET_SEC_002.ForeColor = Color.FromArgb(16, 24, 40);
        lblSettingValue__SET_SEC_002.Location = new Point(745, 42);
        lblSettingValue__SET_SEC_002.Margin = new Padding(3);
        lblSettingValue__SET_SEC_002.Name = "lblSettingValue__SET_SEC_002";
        lblSettingValue__SET_SEC_002.Size = new Size(156, 33);
        lblSettingValue__SET_SEC_002.TabIndex = 2;
        lblSettingValue__SET_SEC_002.Text = "نوع هدف الصلاحية";
        lblSettingValue__SET_SEC_002.TextAlign = ContentAlignment.MiddleRight;
        // 
        // SettingValue__SET_SEC_002
        // 
        SettingValue__SET_SEC_002.AccessibleName = "نوع هدف الصلاحية";
        SettingValue__SET_SEC_002.Dock = DockStyle.Fill;
        SettingValue__SET_SEC_002.DropDownStyle = ComboBoxStyle.DropDownList;
        SettingValue__SET_SEC_002.Font = new Font("Segoe UI", 11F);
        SettingValue__SET_SEC_002.Items.AddRange(new object[] { "SCREEN", "ACTION", "FIELD", "TAB" });
        SettingValue__SET_SEC_002.Location = new Point(3, 42);
        SettingValue__SET_SEC_002.Name = "SettingValue__SET_SEC_002";
        SettingValue__SET_SEC_002.RightToLeft = RightToLeft.Yes;
        SettingValue__SET_SEC_002.Size = new Size(736, 33);
        SettingValue__SET_SEC_002.TabIndex = 3;
        // 
        // lblSettingValue__SET_SEC_003
        // 
        lblSettingValue__SET_SEC_003.AutoSize = true;
        lblSettingValue__SET_SEC_003.Dock = DockStyle.Fill;
        lblSettingValue__SET_SEC_003.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblSettingValue__SET_SEC_003.ForeColor = Color.FromArgb(16, 24, 40);
        lblSettingValue__SET_SEC_003.Location = new Point(745, 81);
        lblSettingValue__SET_SEC_003.Margin = new Padding(3);
        lblSettingValue__SET_SEC_003.Name = "lblSettingValue__SET_SEC_003";
        lblSettingValue__SET_SEC_003.Size = new Size(156, 33);
        lblSettingValue__SET_SEC_003.TabIndex = 4;
        lblSettingValue__SET_SEC_003.Text = "نطاق إسناد الصلاحية";
        lblSettingValue__SET_SEC_003.TextAlign = ContentAlignment.MiddleRight;
        // 
        // SettingValue__SET_SEC_003
        // 
        SettingValue__SET_SEC_003.AccessibleName = "نطاق إسناد الصلاحية";
        SettingValue__SET_SEC_003.Dock = DockStyle.Fill;
        SettingValue__SET_SEC_003.DropDownStyle = ComboBoxStyle.DropDownList;
        SettingValue__SET_SEC_003.Font = new Font("Segoe UI", 11F);
        SettingValue__SET_SEC_003.Items.AddRange(new object[] { "TENANT", "GROUP", "COMPANY", "BRANCH", "MODULE", "ROLE", "USER" });
        SettingValue__SET_SEC_003.Location = new Point(3, 81);
        SettingValue__SET_SEC_003.Name = "SettingValue__SET_SEC_003";
        SettingValue__SET_SEC_003.RightToLeft = RightToLeft.Yes;
        SettingValue__SET_SEC_003.Size = new Size(736, 33);
        SettingValue__SET_SEC_003.TabIndex = 5;
        // 
        // lblPermissionTargetIds
        // 
        lblPermissionTargetIds.AutoSize = true;
        lblPermissionTargetIds.Dock = DockStyle.Fill;
        lblPermissionTargetIds.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblPermissionTargetIds.ForeColor = Color.FromArgb(16, 24, 40);
        lblPermissionTargetIds.Location = new Point(745, 120);
        lblPermissionTargetIds.Margin = new Padding(3);
        lblPermissionTargetIds.Name = "lblPermissionTargetIds";
        lblPermissionTargetIds.Size = new Size(156, 95);
        lblPermissionTargetIds.TabIndex = 6;
        lblPermissionTargetIds.Text = "الصلاحيات";
        lblPermissionTargetIds.TextAlign = ContentAlignment.MiddleRight;
        // 
        // PermissionTargetIds
        // 
        PermissionTargetIds.AccessibleName = "الصلاحيات";
        PermissionTargetIds.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        PermissionTargetIds.CheckOnClick = true;
        PermissionTargetIds.Font = new Font("Segoe UI", 11F);
        PermissionTargetIds.Location = new Point(8, 125);
        PermissionTargetIds.Margin = new Padding(8);
        PermissionTargetIds.Name = "PermissionTargetIds";
        PermissionTargetIds.RightToLeft = RightToLeft.Yes;
        PermissionTargetIds.Size = new Size(726, 85);
        PermissionTargetIds.TabIndex = 7;
        PermissionTargetIds.Tag = "FLD-SEC-PERMS";
        // 
        // grpUserData
        // 
        grpUserData.AutoSize = true;
        grpUserData.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        grpUserData.Controls.Add(tlpUserData);
        grpUserData.Dock = DockStyle.Top;
        grpUserData.Location = new Point(18, 21);
        grpUserData.Margin = new Padding(3);
        grpUserData.Name = "grpUserData";
        grpUserData.Padding = new Padding(3, 4, 3, 4);
        grpUserData.Size = new Size(975, 263);
        grpUserData.TabIndex = 1;
        grpUserData.TabStop = false;
        grpUserData.Text = "الحساب ونطاق العمل";
        // 
        // tlpUserData
        // 
        tlpUserData.AutoSize = true;
        tlpUserData.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpUserData.ColumnCount = 6;
        tlpUserData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 149F));
        tlpUserData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
        tlpUserData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 149F));
        tlpUserData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
        tlpUserData.ColumnStyles.Add(new ColumnStyle());
        tlpUserData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
        tlpUserData.Controls.Add(label14, 0, 3);
        tlpUserData.Controls.Add(txtOnyxEmployeeName, 5, 0);
        tlpUserData.Controls.Add(label13, 4, 0);
        tlpUserData.Controls.Add(txtOnyxEmployeeNumber, 3, 0);
        tlpUserData.Controls.Add(label12, 2, 0);
        tlpUserData.Controls.Add(lblUserName, 0, 1);
        tlpUserData.Controls.Add(txtUserName, 1, 1);
        tlpUserData.Controls.Add(lblFullName, 2, 1);
        tlpUserData.Controls.Add(txtFullName, 3, 1);
        tlpUserData.Controls.Add(lblRole, 4, 3);
        tlpUserData.Controls.Add(cboRole, 5, 3);
        tlpUserData.Controls.Add(lblBranch, 2, 2);
        tlpUserData.Controls.Add(cboBranch, 3, 2);
        tlpUserData.Controls.Add(lblStatus, 0, 5);
        tlpUserData.Controls.Add(cboStatus, 1, 5);
        tlpUserData.Controls.Add(chkRequirePasswordReset, 1, 4);
        tlpUserData.Controls.Add(label6, 0, 2);
        tlpUserData.Controls.Add(txtForeignName, 1, 2);
        tlpUserData.Controls.Add(label11, 0, 0);
        tlpUserData.Controls.Add(txtOnyxUserNumber, 1, 0);
        tlpUserData.Controls.Add(cboOnyxGroupNumber, 1, 3);
        tlpUserData.Controls.Add(label15, 4, 2);
        tlpUserData.Controls.Add(label18, 4, 1);
        tlpUserData.Controls.Add(cboOnyxBranchNumber, 5, 1);
        tlpUserData.Controls.Add(tlpOnyxManagerPair, 5, 2);
        tlpUserData.Dock = DockStyle.Fill;
        tlpUserData.Location = new Point(3, 27);
        tlpUserData.Margin = new Padding(3);
        tlpUserData.Name = "tlpUserData";
        tlpUserData.RightToLeft = RightToLeft.Yes;
        tlpUserData.RowCount = 6;
        tlpUserData.RowStyles.Add(new RowStyle());
        tlpUserData.RowStyles.Add(new RowStyle());
        tlpUserData.RowStyles.Add(new RowStyle());
        tlpUserData.RowStyles.Add(new RowStyle());
        tlpUserData.RowStyles.Add(new RowStyle());
        tlpUserData.RowStyles.Add(new RowStyle());
        tlpUserData.Size = new Size(969, 232);
        tlpUserData.TabIndex = 0;
        // 
        // label14
        // 
        label14.AutoSize = true;
        label14.Dock = DockStyle.Fill;
        label14.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label14.Location = new Point(823, 124);
        label14.Margin = new Padding(3);
        label14.Name = "label14";
        label14.Size = new Size(143, 33);
        label14.TabIndex = 15;
        label14.Text = "رقم المجموعة";
        // 
        // txtOnyxEmployeeName
        // 
        txtOnyxEmployeeName.Dock = DockStyle.Fill;
        txtOnyxEmployeeName.Font = new Font("Segoe UI", 11F);
        txtOnyxEmployeeName.Location = new Point(3, 3);
        txtOnyxEmployeeName.Name = "txtOnyxEmployeeName";
        txtOnyxEmployeeName.Size = new Size(182, 32);
        txtOnyxEmployeeName.TabIndex = 13;
        // 
        // label13
        // 
        label13.AutoSize = true;
        label13.Dock = DockStyle.Fill;
        label13.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label13.Location = new Point(191, 3);
        label13.Margin = new Padding(3);
        label13.Name = "label13";
        label13.Size = new Size(103, 32);
        label13.TabIndex = 12;
        label13.Text = "اسم الموظف";
        // 
        // txtOnyxEmployeeNumber
        // 
        txtOnyxEmployeeNumber.Dock = DockStyle.Fill;
        txtOnyxEmployeeNumber.Font = new Font("Segoe UI", 11F);
        txtOnyxEmployeeNumber.Location = new Point(300, 3);
        txtOnyxEmployeeNumber.Name = "txtOnyxEmployeeNumber";
        txtOnyxEmployeeNumber.Size = new Size(181, 32);
        txtOnyxEmployeeNumber.TabIndex = 11;
        // 
        // label12
        // 
        label12.AutoSize = true;
        label12.Dock = DockStyle.Fill;
        label12.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label12.Location = new Point(487, 3);
        label12.Margin = new Padding(3);
        label12.Name = "label12";
        label12.Size = new Size(143, 32);
        label12.TabIndex = 10;
        label12.Text = "رقم الموظف";
        // 
        // lblUserName
        // 
        lblUserName.AutoSize = true;
        lblUserName.Dock = DockStyle.Fill;
        lblUserName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblUserName.Location = new Point(823, 41);
        lblUserName.Margin = new Padding(3);
        lblUserName.Name = "lblUserName";
        lblUserName.Size = new Size(143, 33);
        lblUserName.TabIndex = 0;
        lblUserName.Text = "اسم المستخدم *";
        // 
        // txtUserName
        // 
        txtUserName.BackColor = Color.Yellow;
        txtUserName.Dock = DockStyle.Fill;
        txtUserName.Font = new Font("Segoe UI", 11F);
        txtUserName.Location = new Point(636, 41);
        txtUserName.Name = "txtUserName";
        txtUserName.Size = new Size(181, 32);
        txtUserName.TabIndex = 0;
        // 
        // lblFullName
        // 
        lblFullName.AutoSize = true;
        lblFullName.Dock = DockStyle.Fill;
        lblFullName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblFullName.Location = new Point(487, 41);
        lblFullName.Margin = new Padding(3);
        lblFullName.Name = "lblFullName";
        lblFullName.Size = new Size(143, 33);
        lblFullName.TabIndex = 1;
        lblFullName.Text = "الاسم الكامل *";
        // 
        // txtFullName
        // 
        txtFullName.BackColor = Color.Yellow;
        txtFullName.Dock = DockStyle.Fill;
        txtFullName.Font = new Font("Segoe UI", 11F);
        txtFullName.Location = new Point(300, 41);
        txtFullName.Name = "txtFullName";
        txtFullName.Size = new Size(181, 32);
        txtFullName.TabIndex = 1;
        // 
        // lblRole
        // 
        lblRole.AutoSize = true;
        lblRole.Dock = DockStyle.Fill;
        lblRole.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblRole.Location = new Point(191, 124);
        lblRole.Margin = new Padding(3);
        lblRole.Name = "lblRole";
        lblRole.Size = new Size(103, 33);
        lblRole.TabIndex = 2;
        lblRole.Text = "الدور *";
        // 
        // cboRole
        // 
        cboRole.BackColor = Color.Yellow;
        cboRole.Dock = DockStyle.Fill;
        cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
        cboRole.FlatStyle = FlatStyle.Flat;
        cboRole.Font = new Font("Segoe UI", 11F);
        cboRole.Location = new Point(3, 124);
        cboRole.Name = "cboRole";
        cboRole.Size = new Size(182, 33);
        cboRole.TabIndex = 2;
        // 
        // lblBranch
        // 
        lblBranch.AutoSize = true;
        lblBranch.Dock = DockStyle.Fill;
        lblBranch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblBranch.Location = new Point(487, 80);
        lblBranch.Margin = new Padding(3);
        lblBranch.Name = "lblBranch";
        lblBranch.Size = new Size(143, 38);
        lblBranch.TabIndex = 3;
        lblBranch.Text = "الفرع *";
        // 
        // cboBranch
        // 
        cboBranch.BackColor = Color.Yellow;
        cboBranch.Dock = DockStyle.Fill;
        cboBranch.DropDownStyle = ComboBoxStyle.DropDownList;
        cboBranch.FlatStyle = FlatStyle.Flat;
        cboBranch.Font = new Font("Segoe UI", 11F);
        cboBranch.Location = new Point(300, 80);
        cboBranch.Name = "cboBranch";
        cboBranch.Size = new Size(181, 33);
        cboBranch.TabIndex = 3;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblStatus.Location = new Point(823, 196);
        lblStatus.Margin = new Padding(3);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(143, 33);
        lblStatus.TabIndex = 4;
        lblStatus.Text = "الحالة";
        // 
        // cboStatus
        // 
        cboStatus.Dock = DockStyle.Fill;
        cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cboStatus.Font = new Font("Segoe UI", 11F);
        cboStatus.Items.AddRange(new object[] { "ACTIVE", "DISABLED", "LOCKED" });
        cboStatus.Location = new Point(636, 196);
        cboStatus.Name = "cboStatus";
        cboStatus.Size = new Size(181, 33);
        cboStatus.TabIndex = 4;
        // 
        // chkRequirePasswordReset
        // 
        chkRequirePasswordReset.AutoSize = true;
        tlpUserData.SetColumnSpan(chkRequirePasswordReset, 3);
        chkRequirePasswordReset.Dock = DockStyle.Fill;
        chkRequirePasswordReset.Location = new Point(300, 163);
        chkRequirePasswordReset.Name = "chkRequirePasswordReset";
        chkRequirePasswordReset.Size = new Size(517, 27);
        chkRequirePasswordReset.TabIndex = 5;
        chkRequirePasswordReset.Text = "إلزام المستخدم بتغيير كلمة المرور عند الدخول التالي";
        // 
        // label6
        // 
        label6.AutoSize = true;
        label6.Dock = DockStyle.Fill;
        label6.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label6.Location = new Point(823, 80);
        label6.Margin = new Padding(3);
        label6.Name = "label6";
        label6.Size = new Size(143, 38);
        label6.TabIndex = 6;
        label6.Text = "الاسم الأجنبي";
        // 
        // txtForeignName
        // 
        txtForeignName.Dock = DockStyle.Fill;
        txtForeignName.Font = new Font("Segoe UI", 11F);
        txtForeignName.Location = new Point(636, 80);
        txtForeignName.Name = "txtForeignName";
        txtForeignName.Size = new Size(181, 32);
        txtForeignName.TabIndex = 7;
        // 
        // label11
        // 
        label11.AutoSize = true;
        label11.Dock = DockStyle.Fill;
        label11.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label11.Location = new Point(823, 3);
        label11.Margin = new Padding(3);
        label11.Name = "label11";
        label11.Size = new Size(143, 32);
        label11.TabIndex = 8;
        label11.Text = "رقم المستخدم";
        // 
        // txtOnyxUserNumber
        // 
        txtOnyxUserNumber.Dock = DockStyle.Fill;
        txtOnyxUserNumber.Font = new Font("Segoe UI", 11F);
        txtOnyxUserNumber.Location = new Point(636, 3);
        txtOnyxUserNumber.Name = "txtOnyxUserNumber";
        txtOnyxUserNumber.Size = new Size(181, 32);
        txtOnyxUserNumber.TabIndex = 9;
        // 
        // cboOnyxGroupNumber
        // 
        cboOnyxGroupNumber.Dock = DockStyle.Fill;
        cboOnyxGroupNumber.Font = new Font("Segoe UI", 11F);
        cboOnyxGroupNumber.FormattingEnabled = true;
        cboOnyxGroupNumber.Location = new Point(636, 124);
        cboOnyxGroupNumber.Name = "cboOnyxGroupNumber";
        cboOnyxGroupNumber.Size = new Size(181, 33);
        cboOnyxGroupNumber.TabIndex = 14;
        // 
        // label15
        // 
        label15.AutoSize = true;
        label15.Dock = DockStyle.Fill;
        label15.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label15.Location = new Point(191, 80);
        label15.Margin = new Padding(3);
        label15.Name = "label15";
        label15.Size = new Size(103, 38);
        label15.TabIndex = 16;
        label15.Text = "المدير";
        // 
        // label18
        // 
        label18.AutoSize = true;
        label18.Dock = DockStyle.Fill;
        label18.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label18.Location = new Point(191, 41);
        label18.Margin = new Padding(3);
        label18.Name = "label18";
        label18.Size = new Size(103, 33);
        label18.TabIndex = 18;
        label18.Text = "رقم الفرع";
        // 
        // cboOnyxBranchNumber
        // 
        cboOnyxBranchNumber.Dock = DockStyle.Fill;
        cboOnyxBranchNumber.Font = new Font("Segoe UI", 11F);
        cboOnyxBranchNumber.FormattingEnabled = true;
        cboOnyxBranchNumber.Location = new Point(3, 41);
        cboOnyxBranchNumber.Name = "cboOnyxBranchNumber";
        cboOnyxBranchNumber.Size = new Size(182, 33);
        cboOnyxBranchNumber.TabIndex = 19;
        // 
        // tlpOnyxManagerPair
        // 
        tlpOnyxManagerPair.AutoSize = true;
        tlpOnyxManagerPair.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpOnyxManagerPair.ColumnCount = 2;
        tlpOnyxManagerPair.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27F));
        tlpOnyxManagerPair.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 73F));
        tlpOnyxManagerPair.Controls.Add(txtOnyxManager, 1, 0);
        tlpOnyxManagerPair.Controls.Add(txtOnyxManagerCode, 0, 0);
        tlpOnyxManagerPair.Dock = DockStyle.Fill;
        tlpOnyxManagerPair.Location = new Point(3, 80);
        tlpOnyxManagerPair.Name = "tlpOnyxManagerPair";
        tlpOnyxManagerPair.RowCount = 1;
        tlpOnyxManagerPair.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpOnyxManagerPair.Size = new Size(182, 38);
        tlpOnyxManagerPair.TabIndex = 20;
        // 
        // txtOnyxManager
        // 
        txtOnyxManager.Dock = DockStyle.Fill;
        txtOnyxManager.Font = new Font("Segoe UI", 11F);
        txtOnyxManager.Location = new Point(3, 3);
        txtOnyxManager.Name = "txtOnyxManager";
        txtOnyxManager.Size = new Size(127, 32);
        txtOnyxManager.TabIndex = 18;
        // 
        // txtOnyxManagerCode
        // 
        txtOnyxManagerCode.Dock = DockStyle.Fill;
        txtOnyxManagerCode.Font = new Font("Segoe UI", 11F);
        txtOnyxManagerCode.Location = new Point(136, 3);
        txtOnyxManagerCode.Name = "txtOnyxManagerCode";
        txtOnyxManagerCode.Size = new Size(43, 32);
        txtOnyxManagerCode.TabIndex = 0;
        // 
        // pnlActions
        // 
        pnlActions.Controls.Add(pnlHeader);
        pnlActions.Dock = DockStyle.Top;
        pnlActions.Location = new Point(0, 0);
        pnlActions.Margin = new Padding(3);
        pnlActions.Name = "pnlActions";
        pnlActions.Padding = new Padding(4);
        pnlActions.Size = new Size(1509, 80);
        pnlActions.TabIndex = 2;
        // 
        // pnlHeader
        // 
        pnlHeader.AutoSize = true;
        pnlHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlHeader.BackColor = Color.FromArgb(224, 224, 224);
        pnlHeader.Controls.Add(designerCommandBar);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(18, 11);
        pnlHeader.MinimumSize = new Size(0, 48);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1473, 48);
        pnlHeader.TabIndex = 3;
        // 
        // pnlToolbar
        // 
        pnlToolbar.AutoSize = true;
        pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlToolbar.BackColor = Color.FromArgb(224, 224, 224);
        pnlToolbar.Controls.Add(button1);
        pnlToolbar.Controls.Add(btnSave);
        pnlToolbar.Controls.Add(btnEdit);
        pnlToolbar.Controls.Add(btnDelete);
        pnlToolbar.Controls.Add(button2);
        pnlToolbar.Controls.Add(btnClose);
        pnlToolbar.Controls.Add(btnFirst);
        pnlToolbar.Controls.Add(btnPrevious);
        pnlToolbar.Controls.Add(txtCurrentRecordNo);
        pnlToolbar.Controls.Add(btnNext);
        pnlToolbar.Controls.Add(btnLast);
        pnlToolbar.Controls.Add(btnUndo);
        pnlToolbar.Controls.Add(btnSearch);
        pnlToolbar.Controls.Add(btnPrint);
        pnlToolbar.Controls.Add(button4);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Location = new Point(0, 0);
        pnlToolbar.Margin = new Padding(5);
        pnlToolbar.MinimumSize = new Size(0, 48);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Padding = new Padding(5);
        pnlToolbar.RightToLeft = RightToLeft.Yes;
        pnlToolbar.Size = new Size(1314, 48);
        pnlToolbar.TabIndex = 3;
        // 
        // button1
        // 
        button1.AutoEllipsis = true;
        button1.BackColor = Color.FromArgb(224, 224, 224);
        button1.FlatStyle = FlatStyle.Flat;
        button1.Font = new Font("Microsoft Sans Serif", 10F);
        button1.ForeColor = Color.FromArgb(16, 24, 40);
        button1.Location = new Point(1230, 9);
        button1.Margin = new Padding(3);
        button1.Name = "button1";
        button1.Size = new Size(70, 30);
        button1.TabIndex = 0;
        button1.Text = "جديد";
        button1.UseVisualStyleBackColor = false;
        // 
        // btnSave
        // 
        btnSave.AutoEllipsis = true;
        btnSave.BackColor = Color.FromArgb(224, 224, 224);
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Microsoft Sans Serif", 10F);
        btnSave.ForeColor = Color.FromArgb(16, 24, 40);
        btnSave.Location = new Point(1152, 9);
        btnSave.Margin = new Padding(3);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(70, 30);
        btnSave.TabIndex = 1;
        btnSave.Text = "حفظ";
        btnSave.UseVisualStyleBackColor = false;
        // 
        // btnEdit
        // 
        btnEdit.AutoEllipsis = true;
        btnEdit.BackColor = Color.FromArgb(224, 224, 224);
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.Font = new Font("Microsoft Sans Serif", 10F);
        btnEdit.ForeColor = Color.FromArgb(16, 24, 40);
        btnEdit.Location = new Point(1074, 9);
        btnEdit.Margin = new Padding(3);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(70, 30);
        btnEdit.TabIndex = 2;
        btnEdit.Text = "تعديل";
        btnEdit.UseVisualStyleBackColor = false;
        // 
        // btnDelete
        // 
        btnDelete.AutoEllipsis = true;
        btnDelete.BackColor = Color.FromArgb(224, 224, 224);
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.Font = new Font("Microsoft Sans Serif", 10F);
        btnDelete.ForeColor = Color.FromArgb(16, 24, 40);
        btnDelete.Location = new Point(996, 9);
        btnDelete.Margin = new Padding(3);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(70, 30);
        btnDelete.TabIndex = 3;
        btnDelete.Text = "حذف";
        btnDelete.UseVisualStyleBackColor = false;
        // 
        // button2
        // 
        button2.AutoEllipsis = true;
        button2.BackColor = Color.FromArgb(224, 224, 224);
        button2.FlatStyle = FlatStyle.Flat;
        button2.Font = new Font("Microsoft Sans Serif", 10F);
        button2.ForeColor = Color.FromArgb(16, 24, 40);
        button2.Location = new Point(918, 9);
        button2.Margin = new Padding(3);
        button2.Name = "button2";
        button2.Size = new Size(70, 30);
        button2.TabIndex = 4;
        button2.Text = "تحديث";
        button2.UseVisualStyleBackColor = false;
        // 
        // btnClose
        // 
        btnClose.AutoEllipsis = true;
        btnClose.BackColor = Color.FromArgb(224, 224, 224);
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Microsoft Sans Serif", 10F);
        btnClose.ForeColor = Color.FromArgb(16, 24, 40);
        btnClose.Location = new Point(840, 9);
        btnClose.Margin = new Padding(3);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(70, 30);
        btnClose.TabIndex = 5;
        btnClose.Text = "اغلاق";
        btnClose.UseVisualStyleBackColor = false;
        // 
        // btnFirst
        // 
        btnFirst.AutoEllipsis = true;
        btnFirst.BackColor = Color.FromArgb(224, 224, 224);
        btnFirst.FlatStyle = FlatStyle.Flat;
        btnFirst.Font = new Font("Microsoft Sans Serif", 10F);
        btnFirst.ForeColor = Color.FromArgb(16, 24, 40);
        btnFirst.Location = new Point(762, 9);
        btnFirst.Margin = new Padding(3);
        btnFirst.Name = "btnFirst";
        btnFirst.Size = new Size(70, 30);
        btnFirst.TabIndex = 6;
        btnFirst.Text = "الاول ";
        btnFirst.UseVisualStyleBackColor = false;
        // 
        // btnPrevious
        // 
        btnPrevious.AutoEllipsis = true;
        btnPrevious.BackColor = Color.FromArgb(224, 224, 224);
        btnPrevious.FlatStyle = FlatStyle.Flat;
        btnPrevious.Font = new Font("Microsoft Sans Serif", 10F);
        btnPrevious.ForeColor = Color.FromArgb(16, 24, 40);
        btnPrevious.Location = new Point(684, 9);
        btnPrevious.Margin = new Padding(3);
        btnPrevious.Name = "btnPrevious";
        btnPrevious.Size = new Size(70, 30);
        btnPrevious.TabIndex = 7;
        btnPrevious.Text = "السابق";
        btnPrevious.UseVisualStyleBackColor = false;
        // 
        // txtCurrentRecordNo
        // 
        txtCurrentRecordNo.BackColor = Color.White;
        txtCurrentRecordNo.Font = new Font("Segoe UI", 11F);
        txtCurrentRecordNo.ForeColor = Color.FromArgb(16, 24, 40);
        txtCurrentRecordNo.Location = new Point(614, 9);
        txtCurrentRecordNo.Margin = new Padding(3);
        txtCurrentRecordNo.Multiline = true;
        txtCurrentRecordNo.Name = "txtCurrentRecordNo";
        txtCurrentRecordNo.ReadOnly = true;
        txtCurrentRecordNo.Size = new Size(62, 30);
        txtCurrentRecordNo.TabIndex = 8;
        txtCurrentRecordNo.TabStop = false;
        txtCurrentRecordNo.TextAlign = HorizontalAlignment.Center;
        // 
        // btnNext
        // 
        btnNext.AutoEllipsis = true;
        btnNext.BackColor = Color.FromArgb(224, 224, 224);
        btnNext.FlatStyle = FlatStyle.Flat;
        btnNext.Font = new Font("Microsoft Sans Serif", 10F);
        btnNext.ForeColor = Color.FromArgb(16, 24, 40);
        btnNext.Location = new Point(536, 9);
        btnNext.Margin = new Padding(3);
        btnNext.Name = "btnNext";
        btnNext.Size = new Size(70, 30);
        btnNext.TabIndex = 9;
        btnNext.Text = "التالي";
        btnNext.UseVisualStyleBackColor = false;
        // 
        // btnLast
        // 
        btnLast.AutoEllipsis = true;
        btnLast.BackColor = Color.FromArgb(224, 224, 224);
        btnLast.FlatStyle = FlatStyle.Flat;
        btnLast.Font = new Font("Microsoft Sans Serif", 10F);
        btnLast.ForeColor = Color.FromArgb(16, 24, 40);
        btnLast.Location = new Point(458, 9);
        btnLast.Margin = new Padding(3);
        btnLast.Name = "btnLast";
        btnLast.Size = new Size(70, 30);
        btnLast.TabIndex = 10;
        btnLast.Text = "الاخير";
        btnLast.UseVisualStyleBackColor = false;
        // 
        // btnUndo
        // 
        btnUndo.AutoEllipsis = true;
        btnUndo.BackColor = Color.FromArgb(224, 224, 224);
        btnUndo.FlatStyle = FlatStyle.Flat;
        btnUndo.Font = new Font("Microsoft Sans Serif", 10F);
        btnUndo.ForeColor = Color.FromArgb(16, 24, 40);
        btnUndo.Location = new Point(380, 9);
        btnUndo.Margin = new Padding(3);
        btnUndo.Name = "btnUndo";
        btnUndo.Size = new Size(70, 30);
        btnUndo.TabIndex = 11;
        btnUndo.Text = "تراجع";
        btnUndo.UseVisualStyleBackColor = false;
        // 
        // btnSearch
        // 
        btnSearch.AutoEllipsis = true;
        btnSearch.BackColor = Color.FromArgb(224, 224, 224);
        btnSearch.FlatStyle = FlatStyle.Flat;
        btnSearch.Font = new Font("Microsoft Sans Serif", 10F);
        btnSearch.ForeColor = Color.FromArgb(16, 24, 40);
        btnSearch.Location = new Point(302, 9);
        btnSearch.Margin = new Padding(3);
        btnSearch.Name = "btnSearch";
        btnSearch.Size = new Size(70, 30);
        btnSearch.TabIndex = 12;
        btnSearch.Text = "بحث";
        btnSearch.UseVisualStyleBackColor = false;
        // 
        // btnPrint
        // 
        btnPrint.AutoEllipsis = true;
        btnPrint.BackColor = Color.FromArgb(224, 224, 224);
        btnPrint.FlatStyle = FlatStyle.Flat;
        btnPrint.Font = new Font("Microsoft Sans Serif", 10F);
        btnPrint.ForeColor = Color.FromArgb(16, 24, 40);
        btnPrint.Location = new Point(224, 9);
        btnPrint.Margin = new Padding(3);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(70, 30);
        btnPrint.TabIndex = 13;
        btnPrint.Text = "طباعة";
        btnPrint.UseVisualStyleBackColor = false;
        // 
        // button4
        // 
        button4.AutoEllipsis = true;
        button4.BackColor = Color.FromArgb(224, 224, 224);
        button4.FlatStyle = FlatStyle.Flat;
        button4.Font = new Font("Microsoft Sans Serif", 10F);
        button4.ForeColor = Color.FromArgb(16, 24, 40);
        button4.Location = new Point(146, 9);
        button4.Margin = new Padding(3);
        button4.Name = "button4";
        button4.Size = new Size(70, 30);
        button4.TabIndex = 14;
        button4.Text = "اعتماد";
        button4.UseVisualStyleBackColor = false;
        // 
        // lblTitle
        // 
        lblTitle.AutoEllipsis = true;
        lblTitle.AutoSize = true;
        lblTitle.BackColor = Color.FromArgb(192, 192, 255);
        lblTitle.Dock = DockStyle.Right;
        lblTitle.Font = new Font("Segoe UI", 16F);
        lblTitle.ForeColor = Color.FromArgb(16, 24, 40);
        lblTitle.Location = new Point(1314, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Padding = new Padding(0, 0, 15, 0);
        lblTitle.RightToLeft = RightToLeft.Yes;
        lblTitle.Size = new Size(159, 37);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "ادارة الفروع";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // UcUsersPermissions
        // 
        AutoScaleDimensions = new SizeF(9F, 23F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(248, 250, 252);
        Controls.Add(splitMain);
        Controls.Add(auditFooter);
        Controls.Add(pnlActions);
        Font = new Font("Segoe UI", 10F);
        Margin = new Padding(3);
        MinimumSize = new Size(1117, 855);
        Name = "UcUsersPermissions";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1509, 971);
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        splitMain.Panel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
        splitMain.ResumeLayout(false);
        pnlUsers.ResumeLayout(false);
        pnlUsers.PerformLayout();
        tabUserDetails.ResumeLayout(false);
        tabUserData.ResumeLayout(false);
        tabUserData.PerformLayout();
        userLookupV20.ResumeLayout(false);
        userLookupV20.PerformLayout();
        tabDevicesV20.ResumeLayout(false);
        tabDevicesV20.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvOnyxHandheldDevices).EndInit();
        deviceFieldsV20.ResumeLayout(false);
        deviceFieldsV20.PerformLayout();
        tabPosOnyx.ResumeLayout(false);
        tabPosOnyx.PerformLayout();
        posLayoutHost.ResumeLayout(false);
        posLayoutHost.PerformLayout();
        tabRolesV20.ResumeLayout(false);
        tabRolesV20.PerformLayout();
        roleCatalogLayout.ResumeLayout(false);
        roleCatalogLayout.PerformLayout();
        roleActions.ResumeLayout(false);
        roleActions.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvRoleCatalog).EndInit();
        roleLookupV20.ResumeLayout(false);
        roleLookupV20.PerformLayout();
        tabPermissions.ResumeLayout(false);
        tabPermissions.PerformLayout();
        tlpPermissions.ResumeLayout(false);
        grpPermissionTree.ResumeLayout(false);
        grpPermissionTree.PerformLayout();
        grpModulePermissions.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvModulePermissions).EndInit();
        securityPolicyV20.ResumeLayout(false);
        securityPolicyV20.PerformLayout();
        grpUserData.ResumeLayout(false);
        grpUserData.PerformLayout();
        tlpUserData.ResumeLayout(false);
        tlpUserData.PerformLayout();
        tlpOnyxManagerPair.ResumeLayout(false);
        tlpOnyxManagerPair.PerformLayout();
        pnlActions.ResumeLayout(false);
        pnlActions.PerformLayout();
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlToolbar.ResumeLayout(false);
        pnlToolbar.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(0, 0);
        designerCommandBar.Size = new Size(1314, 48);
        designerCommandBar.Margin = new Padding(5);
        designerCommandBar.MinimumSize = new Size(0, 48);
        designerCommandBar.TabIndex = 3;
        designerCommandBar.RightToLeft = RightToLeft.Yes;
        designerCommandBar.Controls.Add(pnlToolbar);
        pnlToolbar.Dock = DockStyle.Fill;
        pnlToolbar.Margin = Padding.Empty;
        designerCommandBar.Name = "designerCommandBar";
        designerCommandBar.BackColor = Color.FromArgb(239, 239, 239);
        designerCommandBar.BorderStyle = BorderStyle.FixedSingle;
        designerHiddenCommands.Name = "designerHiddenCommands";
        designerHiddenCommands.Visible = false;
        designerCommandBar.Controls.Add(designerHiddenCommands);
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
        designerCommandBar.SetCommandRole(button1, TransportERP.Desktop.CoreUI.DesignerCommandRole.Add);
        designerCommandBar.SetCommandRole(btnEdit, TransportERP.Desktop.CoreUI.DesignerCommandRole.Edit);
        designerCommandBar.SetCommandRole(btnDelete, TransportERP.Desktop.CoreUI.DesignerCommandRole.Delete);
        designerCommandBar.SetCommandRole(btnUndo, TransportERP.Desktop.CoreUI.DesignerCommandRole.Cancel);
        designerCommandBar.SetCommandRole(btnSearch, TransportERP.Desktop.CoreUI.DesignerCommandRole.View);
        designerCommandBar.SetCommandRole(btnLast, TransportERP.Desktop.CoreUI.DesignerCommandRole.Last);
        designerCommandBar.SetCommandRole(btnNext, TransportERP.Desktop.CoreUI.DesignerCommandRole.Next);
        designerCommandBar.SetCommandRole(btnPrevious, TransportERP.Desktop.CoreUI.DesignerCommandRole.Previous);
        designerCommandBar.SetCommandRole(btnFirst, TransportERP.Desktop.CoreUI.DesignerCommandRole.First);
        designerCommandBar.SetCommandRole(btnSave, TransportERP.Desktop.CoreUI.DesignerCommandRole.Save);
        designerCommandBar.SetCommandRole(btnPrint, TransportERP.Desktop.CoreUI.DesignerCommandRole.Print);
        designerCommandBar.SetCommandRole(btnClose, TransportERP.Desktop.CoreUI.DesignerCommandRole.Close);
        designerCommandBar.SetCommandRole(button2, TransportERP.Desktop.CoreUI.DesignerCommandRole.Refresh);
        standardCommandImport.AutoSize = false;
        standardCommandImport.Dock = DockStyle.None;
        standardCommandImport.MinimumSize = Size.Empty;
        standardCommandImport.Size = new Size(26, 24);
        standardCommandImport.Margin = new Padding(1);
        standardCommandImport.FlatStyle = FlatStyle.Flat;
        standardCommandImport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandImport.Text = "استيراد";
        designerHiddenCommands.Controls.Add(standardCommandImport);
        designerCommandBar.SetCommandRole(standardCommandImport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Import);
        standardCommandExport.AutoSize = false;
        standardCommandExport.Dock = DockStyle.None;
        standardCommandExport.MinimumSize = Size.Empty;
        standardCommandExport.Size = new Size(26, 24);
        standardCommandExport.Margin = new Padding(1);
        standardCommandExport.FlatStyle = FlatStyle.Flat;
        standardCommandExport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandExport.Text = "تصدير";
        designerHiddenCommands.Controls.Add(standardCommandExport);
        designerCommandBar.SetCommandRole(standardCommandExport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Export);
        standardCommandHelp.AutoSize = false;
        standardCommandHelp.Dock = DockStyle.None;
        standardCommandHelp.MinimumSize = Size.Empty;
        standardCommandHelp.Size = new Size(26, 24);
        standardCommandHelp.Margin = new Padding(1);
        standardCommandHelp.FlatStyle = FlatStyle.Flat;
        standardCommandHelp.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandHelp.Text = "مساعدة";
        designerHiddenCommands.Controls.Add(standardCommandHelp);
        designerCommandBar.SetCommandRole(standardCommandHelp, TransportERP.Desktop.CoreUI.DesignerCommandRole.Help);
    
        // Shared audit presentation; original sources remain owned by this screen.
        standardAuditMetadata = new TransportERP.Desktop.CoreUI.AuditMetadataControl();
        standardAuditMetadata.Name = "standardAuditMetadata";
        standardAuditMetadata.Dock = DockStyle.Bottom;
        standardAuditMetadata.Margin = Padding.Empty;
        standardAuditMetadata.TabStop = false;
        standardAuditMetadata.Size = new Size(800, 64);
        auditFooter.Visible = false;
        Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    private AuditCountersControl auditFooter = null!;
    private TabPage tabRolesV20 = null!;
    private TabPage tabDevicesV20 = null!;
    private TableLayoutPanel roleLookupV20 = null!;
    private Label lblRoleId = null!;
    private ComboBox RoleId = null!;
    private TableLayoutPanel deviceFieldsV20 = null!;
    private Label lblDeviceV20 = null!;
    private ComboBox DeviceId = null!;
    private TableLayoutPanel userLookupV20 = null!;
    private Label lblUserId = null!;
    private ComboBox UserId = null!;
    private TreeView tvUserRecords = null!;
    private GroupBox grpUserData = null!;
    private TableLayoutPanel tlpUserData = null!;
    private Label lblUserName = null!;
    private TextBox txtUserName = null!;
    private Label lblFullName = null!;
    private TextBox txtFullName = null!;
    private Label lblRole = null!;
    private ComboBox cboRole = null!;
    private Label lblBranch = null!;
    private ComboBox cboBranch = null!;
    private Label lblStatus = null!;
    private ComboBox cboStatus = null!;
    private CheckBox chkRequirePasswordReset = null!;
    private TabPage tabPosOnyx;
    private TableLayoutPanel posLayoutHost;
    private Label label1;
    private ComboBox cboPosConnection;
    private TextBox txtBarcodePrinter;
    private TextBox txtPosName;
    private TextBox txtBarcodePath;
    private TextBox txtPosNumber;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private TextBox txtForeignName;
    private Label label7;
    private TextBox txtAccessStartDate;
    private Label label8;
    private TextBox txtAccessEndDate;
    private Label label9;
    private TextBox txtAccessFromTime;
    private Label label10;
    private TextBox txtAccessToTime;
    private Label label11;
    private TextBox txtOnyxUserNumber;
    private TextBox txtOnyxEmployeeNumber;
    private Label label12;
    private TextBox txtOnyxEmployeeName;
    private Label label13;
    private ComboBox cboOnyxGroupNumber;
    private Label label14;
    private Label label15;
    private CheckBox checkBox1;
    private Label label18;
    private ComboBox cboOnyxBranchNumber;
    private Label label19;
    private TextBox txtOnyxProfession;
    private Label label20;
    private TextBox txtOnyxUserComputerName;
    private Label label21;
    private TextBox txtOnyxUserStopReason;
    private DataGridView dgvOnyxHandheldDevices;
    private CheckBox chkOnyxUserStopped;
    private TableLayoutPanel tlpOnyxManagerPair;
    private TextBox txtOnyxManager;
    private TextBox txtOnyxManagerCode;
    private DataGridViewTextBoxColumn Column1;
    private DataGridViewTextBoxColumn Column2;
    private DataGridViewCheckBoxColumn Column3;
    private DataGridViewTextBoxColumn Column4;
    private Label label16;
    private TextBox textBox1;
    private Label label17;
    private TextBox textBox2;
    private TextBox textBox3;
    private TableLayoutPanel roleCatalogLayout = null!;
    private FlowLayoutPanel roleActions = null!;
    private DataGridView dgvRoleCatalog = null!;
    private Button btnRoleAdd = null!;
    private Button btnRoleRemove = null!;
    private Button btnRoleValidate = null!;
    private Button btnRoleSave = null!;
    private Label lblRoleState = null!;
    private Button btnRoleClear = null!;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    private DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
    private DataGridViewComboBoxColumn dataGridViewComboBoxColumn2;
    private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
    private Panel pnlHeader;
    private FlowLayoutPanel pnlToolbar;
    private Button button1;
    private Button btnSave;
    private Button btnEdit;
    private Button btnDelete;
    private Button button2;
    private Button btnClose;
    private Button btnFirst;
    private Button btnPrevious;
    private TextBox txtCurrentRecordNo;
    private Button btnNext;
    private Button btnLast;
    private Button btnUndo;
    private Button btnSearch;
    private Button btnPrint;
    private Button button4;
    private Label lblTitle;
}
