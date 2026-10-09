#nullable enable

using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.Setup.Security;

partial class FrmUsersPermissions
{
    private System.ComponentModel.IContainer? components;
    private SplitContainer splitMain = null!;
    private Panel pnlUsers = null!; private Label lblUsersList = null!; private TextBox txtUserSearch = null!;     private TabControl tabUserDetails = null!; private TabPage tabUserData = null!;     private TabPage tabPermissions = null!; private TableLayoutPanel tlpPermissions = null!; private GroupBox grpPermissionTree = null!; private TreeView tvPermissions = null!; private CheckBox chkSelectAllPermissions = null!; private GroupBox grpModulePermissions = null!; private DataGridView dgvModulePermissions = null!; private DataGridViewTextBoxColumn colModule = null!; private DataGridViewCheckBoxColumn colView = null!; private DataGridViewCheckBoxColumn colAdd = null!; private DataGridViewCheckBoxColumn colEdit = null!; private DataGridViewCheckBoxColumn colPrint = null!;
    private Panel pnlActions = null!; private FlowLayoutPanel flpActions = null!; private Button btnNew = null!; private Button btnSave = null!; private Button btnEdit = null!; private Button btnDisable = null!; private Button btnResetPassword = null!; private Button btnClose = null!;

    protected override void Dispose(bool disposing) { if (disposing && components is not null) components.Dispose(); base.Dispose(disposing); }

    private void InitializeComponent()
    {
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
        tabDevicesV20 = new TabPage();
        dgvOnyxHandheldDevices = new DataGridView();
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
        flpActions = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        btnDisable = new Button();
        btnResetPassword = new Button();
        btnClose = new Button();
        Column1 = new DataGridViewTextBoxColumn();
        Column2 = new DataGridViewTextBoxColumn();
        Column3 = new DataGridViewCheckBoxColumn();
        Column4 = new DataGridViewTextBoxColumn();
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
        roleLookupV20.SuspendLayout();
        tabPermissions.SuspendLayout();
        tlpPermissions.SuspendLayout();
        grpPermissionTree.SuspendLayout();
        grpModulePermissions.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvModulePermissions).BeginInit();
        grpUserData.SuspendLayout();
        tlpUserData.SuspendLayout();
        tlpOnyxManagerPair.SuspendLayout();
        pnlActions.SuspendLayout();
        flpActions.SuspendLayout();
        SuspendLayout();
        // 
        // auditFooter
        // 
        auditFooter.AutoSize = true;
        auditFooter.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        auditFooter.BackColor = Color.FromArgb(50, 182, 255);
        auditFooter.Dock = DockStyle.Bottom;
        auditFooter.Font = new Font("Segoe UI", 10F);
        auditFooter.Location = new Point(0, 893);
        auditFooter.Name = "auditFooter";
        auditFooter.RightToLeft = RightToLeft.Yes;
        auditFooter.Size = new Size(1509, 78);
        auditFooter.TabIndex = 3;
        // 
        // splitMain
        // 
        splitMain.Dock = DockStyle.Fill;
        splitMain.FixedPanel = FixedPanel.Panel1;
        splitMain.Location = new Point(0, 80);
        splitMain.Margin = new Padding(3, 4, 3, 4);
        splitMain.Name = "splitMain";
        // 
        // splitMain.Panel1
        // 
        splitMain.Panel1.Controls.Add(pnlUsers);
        splitMain.Panel1.Padding = new Padding(18, 21, 18, 21);
        splitMain.Panel1.RightToLeft = RightToLeft.No;
        splitMain.Panel1MinSize = 390;
        // 
        // splitMain.Panel2
        // 
        splitMain.Panel2.Controls.Add(tabUserDetails);
        splitMain.Panel2.Controls.Add(grpUserData);
        splitMain.Panel2.Padding = new Padding(18, 21, 18, 21);
        splitMain.Panel2.RightToLeft = RightToLeft.Yes;
        splitMain.Panel2MinSize = 650;
        splitMain.RightToLeft = RightToLeft.No;
        splitMain.Size = new Size(1509, 813);
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
        pnlUsers.Margin = new Padding(3, 4, 3, 4);
        pnlUsers.Name = "pnlUsers";
        pnlUsers.Padding = new Padding(18, 21, 18, 21);
        pnlUsers.Size = new Size(455, 771);
        pnlUsers.TabIndex = 0;
        // 
        // tvUserRecords
        // 
        tvUserRecords.Dock = DockStyle.Fill;
        tvUserRecords.Location = new Point(18, 102);
        tvUserRecords.Name = "tvUserRecords";
        tvUserRecords.Size = new Size(419, 648);
        tvUserRecords.TabIndex = 3;
        // 
        // txtUserSearch
        // 
        txtUserSearch.Dock = DockStyle.Top;
        txtUserSearch.Location = new Point(18, 72);
        txtUserSearch.Margin = new Padding(3, 4, 3, 4);
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
        tabUserDetails.Location = new Point(18, 281);
        tabUserDetails.Margin = new Padding(3, 4, 3, 4);
        tabUserDetails.Name = "tabUserDetails";
        tabUserDetails.RightToLeft = RightToLeft.Yes;
        tabUserDetails.RightToLeftLayout = true;
        tabUserDetails.SelectedIndex = 0;
        tabUserDetails.Size = new Size(975, 511);
        tabUserDetails.TabIndex = 0;
        // 
        // tabUserData
        // 
        tabUserData.BackColor = Color.FromArgb(248, 250, 252);
        tabUserData.Controls.Add(userLookupV20);
        tabUserData.Location = new Point(4, 32);
        tabUserData.Margin = new Padding(3, 5, 3, 5);
        tabUserData.Name = "tabUserData";
        tabUserData.Padding = new Padding(21, 28, 21, 28);
        tabUserData.Size = new Size(967, 475);
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
        userLookupV20.Dock = DockStyle.Top;
        userLookupV20.Location = new Point(21, 28);
        userLookupV20.Name = "userLookupV20";
        userLookupV20.RowCount = 6;
        userLookupV20.RowStyles.Add(new RowStyle());
        userLookupV20.RowStyles.Add(new RowStyle());
        userLookupV20.RowStyles.Add(new RowStyle());
        userLookupV20.RowStyles.Add(new RowStyle());
        userLookupV20.RowStyles.Add(new RowStyle());
        userLookupV20.RowStyles.Add(new RowStyle());
        userLookupV20.Size = new Size(925, 224);
        userLookupV20.TabIndex = 2;
        // 
        // lblUserId
        // 
        lblUserId.Anchor = AnchorStyles.Right;
        lblUserId.AutoSize = true;
        lblUserId.Location = new Point(827, 12);
        lblUserId.Margin = new Padding(4, 8, 16, 8);
        lblUserId.Name = "lblUserId";
        lblUserId.Size = new Size(79, 23);
        lblUserId.TabIndex = 0;
        lblUserId.Text = "المستخدم";
        // 
        // UserId
        // 
        UserId.AccessibleName = "المستخدم";
        UserId.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        UserId.DropDownStyle = ComboBoxStyle.DropDownList;
        UserId.FormattingEnabled = true;
        UserId.Location = new Point(238, 8);
        UserId.Margin = new Padding(8);
        UserId.Name = "UserId";
        UserId.Size = new Size(565, 31);
        UserId.TabIndex = 1;
        UserId.Tag = "FLD-SEC-USER";
        // 
        // label7
        // 
        label7.AutoSize = true;
        label7.Location = new Point(829, 47);
        label7.Name = "label7";
        label7.Size = new Size(93, 23);
        label7.TabIndex = 2;
        label7.Text = "تاريخ البداية";
        // 
        // txtAccessStartDate
        // 
        txtAccessStartDate.Location = new Point(683, 50);
        txtAccessStartDate.Name = "txtAccessStartDate";
        txtAccessStartDate.Size = new Size(125, 30);
        txtAccessStartDate.TabIndex = 3;
        // 
        // label8
        // 
        label8.AutoSize = true;
        label8.Location = new Point(134, 47);
        label8.Name = "label8";
        label8.Size = new Size(93, 23);
        label8.TabIndex = 4;
        label8.Text = "تاريخ النهاية";
        // 
        // txtAccessEndDate
        // 
        txtAccessEndDate.Location = new Point(3, 50);
        txtAccessEndDate.Name = "txtAccessEndDate";
        txtAccessEndDate.Size = new Size(125, 30);
        txtAccessEndDate.TabIndex = 5;
        // 
        // label9
        // 
        label9.AutoSize = true;
        label9.Location = new Point(842, 83);
        label9.Name = "label9";
        label9.Size = new Size(80, 23);
        label9.TabIndex = 6;
        label9.Text = "من الوقت";
        // 
        // txtAccessFromTime
        // 
        txtAccessFromTime.Location = new Point(683, 86);
        txtAccessFromTime.Name = "txtAccessFromTime";
        txtAccessFromTime.Size = new Size(125, 30);
        txtAccessFromTime.TabIndex = 7;
        // 
        // label10
        // 
        label10.AutoSize = true;
        label10.Location = new Point(144, 83);
        label10.Name = "label10";
        label10.Size = new Size(83, 23);
        label10.TabIndex = 8;
        label10.Text = "إلى الوقت";
        // 
        // txtAccessToTime
        // 
        txtAccessToTime.Location = new Point(3, 86);
        txtAccessToTime.Name = "txtAccessToTime";
        txtAccessToTime.Size = new Size(125, 30);
        txtAccessToTime.TabIndex = 9;
        // 
        // label19
        // 
        label19.AutoSize = true;
        label19.Location = new Point(868, 119);
        label19.Name = "label19";
        label19.Size = new Size(54, 23);
        label19.TabIndex = 10;
        label19.Text = "المهنة";
        // 
        // txtOnyxProfession
        // 
        txtOnyxProfession.Location = new Point(683, 122);
        txtOnyxProfession.Name = "txtOnyxProfession";
        txtOnyxProfession.Size = new Size(125, 30);
        txtOnyxProfession.TabIndex = 11;
        // 
        // label20
        // 
        label20.AutoSize = true;
        label20.Location = new Point(144, 119);
        label20.Name = "label20";
        label20.Size = new Size(83, 23);
        label20.TabIndex = 12;
        label20.Text = "اسم الجهاز";
        // 
        // txtOnyxUserComputerName
        // 
        txtOnyxUserComputerName.Location = new Point(3, 122);
        txtOnyxUserComputerName.Name = "txtOnyxUserComputerName";
        txtOnyxUserComputerName.Size = new Size(125, 30);
        txtOnyxUserComputerName.TabIndex = 13;
        // 
        // label21
        // 
        label21.AutoSize = true;
        label21.Location = new Point(814, 188);
        label21.Name = "label21";
        label21.Size = new Size(108, 23);
        label21.TabIndex = 14;
        label21.Text = "سبب التوقيف";
        // 
        // txtOnyxUserStopReason
        // 
        userLookupV20.SetColumnSpan(txtOnyxUserStopReason, 3);
        txtOnyxUserStopReason.Dock = DockStyle.Fill;
        txtOnyxUserStopReason.Location = new Point(3, 191);
        txtOnyxUserStopReason.Name = "txtOnyxUserStopReason";
        txtOnyxUserStopReason.Size = new Size(805, 30);
        txtOnyxUserStopReason.TabIndex = 15;
        // 
        // chkOnyxUserStopped
        // 
        chkOnyxUserStopped.AutoSize = true;
        chkOnyxUserStopped.Location = new Point(731, 158);
        chkOnyxUserStopped.Name = "chkOnyxUserStopped";
        chkOnyxUserStopped.Size = new Size(77, 27);
        chkOnyxUserStopped.TabIndex = 14;
        chkOnyxUserStopped.Text = "موقف";
        chkOnyxUserStopped.UseVisualStyleBackColor = true;
        // 
        // tabDevicesV20
        // 
        tabDevicesV20.AutoScroll = true;
        tabDevicesV20.Controls.Add(dgvOnyxHandheldDevices);
        tabDevicesV20.Controls.Add(deviceFieldsV20);
        tabDevicesV20.Location = new Point(4, 32);
        tabDevicesV20.Name = "tabDevicesV20";
        tabDevicesV20.Padding = new Padding(16);
        tabDevicesV20.Size = new Size(967, 475);
        tabDevicesV20.TabIndex = 4;
        tabDevicesV20.Text = "الأجهزة الكفية";
        tabDevicesV20.UseVisualStyleBackColor = true;
        // 
        // dgvOnyxHandheldDevices
        // 
        dgvOnyxHandheldDevices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvOnyxHandheldDevices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvOnyxHandheldDevices.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4 });
        dgvOnyxHandheldDevices.Dock = DockStyle.Fill;
        dgvOnyxHandheldDevices.Location = new Point(16, 96);
        dgvOnyxHandheldDevices.Name = "dgvOnyxHandheldDevices";
        dgvOnyxHandheldDevices.RowHeadersWidth = 51;
        dgvOnyxHandheldDevices.Size = new Size(935, 363);
        dgvOnyxHandheldDevices.TabIndex = 2;
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
        deviceFieldsV20.Size = new Size(935, 80);
        deviceFieldsV20.TabIndex = 1;
        // 
        // lblDeviceV20
        // 
        lblDeviceV20.Anchor = AnchorStyles.Right;
        lblDeviceV20.AutoSize = true;
        lblDeviceV20.Location = new Point(868, 12);
        lblDeviceV20.Margin = new Padding(4, 8, 16, 8);
        lblDeviceV20.Name = "lblDeviceV20";
        lblDeviceV20.Size = new Size(50, 23);
        lblDeviceV20.TabIndex = 0;
        lblDeviceV20.Text = "الجهاز";
        // 
        // DeviceId
        // 
        DeviceId.AccessibleName = "الجهاز";
        DeviceId.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        DeviceId.DropDownStyle = ComboBoxStyle.DropDownList;
        DeviceId.FormattingEnabled = true;
        DeviceId.Location = new Point(8, 8);
        DeviceId.Margin = new Padding(8);
        DeviceId.Name = "DeviceId";
        DeviceId.Size = new Size(836, 31);
        DeviceId.TabIndex = 1;
        DeviceId.Tag = "FLD-SEC-DEVICE";
        // 
        // checkBox1
        // 
        checkBox1.AutoSize = true;
        checkBox1.Location = new Point(855, 50);
        checkBox1.Name = "checkBox1";
        checkBox1.Size = new Size(77, 27);
        checkBox1.TabIndex = 6;
        checkBox1.Text = "موقف";
        checkBox1.UseVisualStyleBackColor = true;
        // 
        // tabPosOnyx
        // 
        tabPosOnyx.AutoScroll = true;
        tabPosOnyx.Controls.Add(posLayoutHost);
        tabPosOnyx.Location = new Point(4, 32);
        tabPosOnyx.Name = "tabPosOnyx";
        tabPosOnyx.Padding = new Padding(16);
        tabPosOnyx.Size = new Size(967, 475);
        tabPosOnyx.TabIndex = 5;
        tabPosOnyx.Text = "نقاط بيع";
        tabPosOnyx.UseVisualStyleBackColor = true;
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
        posLayoutHost.Location = new Point(145, 216);
        posLayoutHost.MaximumSize = new Size(677, 0);
        posLayoutHost.Name = "posLayoutHost";
        posLayoutHost.RowCount = 4;
        posLayoutHost.RowStyles.Add(new RowStyle());
        posLayoutHost.RowStyles.Add(new RowStyle());
        posLayoutHost.RowStyles.Add(new RowStyle());
        posLayoutHost.RowStyles.Add(new RowStyle());
        posLayoutHost.Size = new Size(677, 145);
        posLayoutHost.TabIndex = 1;
        // 
        // label1
        // 
        label1.Anchor = AnchorStyles.Right;
        label1.AutoSize = true;
        label1.Location = new Point(485, 7);
        label1.Margin = new Padding(3);
        label1.Name = "label1";
        label1.Size = new Size(92, 23);
        label1.TabIndex = 0;
        label1.Text = "نوع الاتصال";
        // 
        // cboPosConnection
        // 
        cboPosConnection.AccessibleName = "نوع الاتصال";
        cboPosConnection.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        posLayoutHost.SetColumnSpan(cboPosConnection, 3);
        cboPosConnection.DropDownStyle = ComboBoxStyle.DropDownList;
        cboPosConnection.FormattingEnabled = true;
        cboPosConnection.Location = new Point(3, 3);
        cboPosConnection.Name = "cboPosConnection";
        cboPosConnection.Size = new Size(476, 31);
        cboPosConnection.TabIndex = 1;
        cboPosConnection.Tag = "POS-CONNECTION";
        // 
        // txtPosNumber
        // 
        txtPosNumber.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtPosNumber.Location = new Point(306, 40);
        txtPosNumber.Name = "txtPosNumber";
        txtPosNumber.Size = new Size(173, 30);
        txtPosNumber.TabIndex = 2;
        // 
        // label4
        // 
        label4.Anchor = AnchorStyles.Right;
        label4.AutoSize = true;
        label4.Location = new Point(485, 79);
        label4.Name = "label4";
        label4.Size = new Size(144, 23);
        label4.TabIndex = 8;
        label4.Text = "اسم طابعة الباركود";
        // 
        // label5
        // 
        label5.Anchor = AnchorStyles.Right;
        label5.AutoSize = true;
        label5.Location = new Point(485, 115);
        label5.Name = "label5";
        label5.Size = new Size(189, 23);
        label5.TabIndex = 9;
        label5.Text = "مسار ملف طابعة الباركود";
        // 
        // txtBarcodePath
        // 
        posLayoutHost.SetColumnSpan(txtBarcodePath, 3);
        txtBarcodePath.Dock = DockStyle.Fill;
        txtBarcodePath.Location = new Point(3, 112);
        txtBarcodePath.Name = "txtBarcodePath";
        txtBarcodePath.RightToLeft = RightToLeft.No;
        txtBarcodePath.Size = new Size(476, 30);
        txtBarcodePath.TabIndex = 5;
        // 
        // label2
        // 
        label2.Anchor = AnchorStyles.Right;
        label2.AutoSize = true;
        label2.Location = new Point(485, 43);
        label2.Name = "label2";
        label2.Size = new Size(115, 23);
        label2.TabIndex = 6;
        label2.Text = "رقم نقطة البيع";
        // 
        // label3
        // 
        label3.Anchor = AnchorStyles.Right;
        label3.AutoSize = true;
        label3.Location = new Point(182, 43);
        label3.Name = "label3";
        label3.Size = new Size(118, 23);
        label3.TabIndex = 7;
        label3.Text = "اسم نقطة البيع";
        // 
        // txtPosName
        // 
        txtPosName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtPosName.Location = new Point(3, 40);
        txtPosName.Name = "txtPosName";
        txtPosName.Size = new Size(173, 30);
        txtPosName.TabIndex = 3;
        // 
        // txtBarcodePrinter
        // 
        posLayoutHost.SetColumnSpan(txtBarcodePrinter, 3);
        txtBarcodePrinter.Dock = DockStyle.Fill;
        txtBarcodePrinter.Location = new Point(3, 76);
        txtBarcodePrinter.Name = "txtBarcodePrinter";
        txtBarcodePrinter.Size = new Size(476, 30);
        txtBarcodePrinter.TabIndex = 4;
        // 
        // tabRolesV20
        // 
        tabRolesV20.AutoScroll = true;
        tabRolesV20.Controls.Add(roleLookupV20);
        tabRolesV20.Location = new Point(4, 32);
        tabRolesV20.Name = "tabRolesV20";
        tabRolesV20.Padding = new Padding(16);
        tabRolesV20.Size = new Size(967, 475);
        tabRolesV20.TabIndex = 2;
        tabRolesV20.Text = "إحصائيات";
        tabRolesV20.UseVisualStyleBackColor = true;
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
        roleLookupV20.Size = new Size(935, 47);
        roleLookupV20.TabIndex = 0;
        // 
        // lblRoleId
        // 
        lblRoleId.Anchor = AnchorStyles.Right;
        lblRoleId.AutoSize = true;
        lblRoleId.Location = new Point(886, 12);
        lblRoleId.Margin = new Padding(4, 8, 16, 8);
        lblRoleId.Name = "lblRoleId";
        lblRoleId.Size = new Size(45, 23);
        lblRoleId.TabIndex = 0;
        lblRoleId.Text = "الدور";
        // 
        // RoleId
        // 
        RoleId.AccessibleName = "الدور";
        RoleId.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        RoleId.DropDownStyle = ComboBoxStyle.DropDownList;
        RoleId.FormattingEnabled = true;
        RoleId.Location = new Point(8, 8);
        RoleId.Margin = new Padding(8);
        RoleId.Name = "RoleId";
        RoleId.Size = new Size(854, 31);
        RoleId.TabIndex = 1;
        RoleId.Tag = "FLD-SEC-ROLE";
        // 
        // tabPermissions
        // 
        tabPermissions.BackColor = Color.FromArgb(248, 250, 252);
        tabPermissions.Controls.Add(tlpPermissions);
        tabPermissions.Location = new Point(4, 32);
        tabPermissions.Margin = new Padding(3, 5, 3, 5);
        tabPermissions.Name = "tabPermissions";
        tabPermissions.Padding = new Padding(21, 28, 21, 28);
        tabPermissions.Size = new Size(967, 475);
        tabPermissions.TabIndex = 1;
        tabPermissions.Text = "معلومات رقابية";
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
        tlpPermissions.Margin = new Padding(3, 4, 3, 4);
        tlpPermissions.Name = "tlpPermissions";
        tlpPermissions.RowCount = 1;
        tlpPermissions.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tlpPermissions.Size = new Size(925, 419);
        tlpPermissions.TabIndex = 0;
        // 
        // grpPermissionTree
        // 
        grpPermissionTree.Controls.Add(tvPermissions);
        grpPermissionTree.Controls.Add(chkSelectAllPermissions);
        grpPermissionTree.Dock = DockStyle.Fill;
        grpPermissionTree.Location = new Point(540, 4);
        grpPermissionTree.Margin = new Padding(3, 4, 3, 4);
        grpPermissionTree.Name = "grpPermissionTree";
        grpPermissionTree.Padding = new Padding(3, 4, 3, 4);
        grpPermissionTree.Size = new Size(382, 411);
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
        tvPermissions.Location = new Point(3, 54);
        tvPermissions.Margin = new Padding(3, 4, 3, 4);
        tvPermissions.Name = "tvPermissions";
        tvPermissions.RightToLeft = RightToLeft.Yes;
        tvPermissions.RightToLeftLayout = true;
        tvPermissions.ShowLines = false;
        tvPermissions.Size = new Size(376, 353);
        tvPermissions.TabIndex = 1;
        // 
        // chkSelectAllPermissions
        // 
        chkSelectAllPermissions.AutoSize = true;
        chkSelectAllPermissions.Dock = DockStyle.Top;
        chkSelectAllPermissions.Location = new Point(3, 27);
        chkSelectAllPermissions.Margin = new Padding(3, 4, 3, 4);
        chkSelectAllPermissions.Name = "chkSelectAllPermissions";
        chkSelectAllPermissions.Size = new Size(376, 27);
        chkSelectAllPermissions.TabIndex = 0;
        chkSelectAllPermissions.Text = "تحديد كل الصلاحيات الظاهرة";
        // 
        // grpModulePermissions
        // 
        grpModulePermissions.Controls.Add(dgvModulePermissions);
        grpModulePermissions.Dock = DockStyle.Fill;
        grpModulePermissions.Location = new Point(3, 4);
        grpModulePermissions.Margin = new Padding(3, 4, 3, 4);
        grpModulePermissions.Name = "grpModulePermissions";
        grpModulePermissions.Padding = new Padding(3, 4, 3, 4);
        grpModulePermissions.Size = new Size(531, 411);
        grpModulePermissions.TabIndex = 1;
        grpModulePermissions.TabStop = false;
        grpModulePermissions.Text = "صلاحيات الوحدات";
        // 
        // dgvModulePermissions
        // 
        dgvModulePermissions.ColumnHeadersHeight = 29;
        dgvModulePermissions.Columns.AddRange(new DataGridViewColumn[] { colModule, colView, colAdd, colEdit, colPrint });
        dgvModulePermissions.Dock = DockStyle.Fill;
        dgvModulePermissions.Location = new Point(3, 27);
        dgvModulePermissions.Margin = new Padding(3, 4, 3, 4);
        dgvModulePermissions.Name = "dgvModulePermissions";
        dgvModulePermissions.RowHeadersWidth = 51;
        dgvModulePermissions.Size = new Size(525, 380);
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
        // grpUserData
        // 
        grpUserData.AutoSize = true;
        grpUserData.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        grpUserData.Controls.Add(tlpUserData);
        grpUserData.Dock = DockStyle.Top;
        grpUserData.Location = new Point(18, 21);
        grpUserData.Margin = new Padding(3, 4, 3, 4);
        grpUserData.Name = "grpUserData";
        grpUserData.Padding = new Padding(3, 4, 3, 4);
        grpUserData.Size = new Size(975, 260);
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
        tlpUserData.Margin = new Padding(3, 4, 3, 4);
        tlpUserData.Name = "tlpUserData";
        tlpUserData.RightToLeft = RightToLeft.Yes;
        tlpUserData.RowCount = 6;
        tlpUserData.RowStyles.Add(new RowStyle());
        tlpUserData.RowStyles.Add(new RowStyle());
        tlpUserData.RowStyles.Add(new RowStyle());
        tlpUserData.RowStyles.Add(new RowStyle());
        tlpUserData.RowStyles.Add(new RowStyle());
        tlpUserData.RowStyles.Add(new RowStyle());
        tlpUserData.Size = new Size(969, 229);
        tlpUserData.TabIndex = 0;
        // 
        // label14
        // 
        label14.AutoSize = true;
        label14.Location = new Point(858, 116);
        label14.Name = "label14";
        label14.Size = new Size(108, 23);
        label14.TabIndex = 15;
        label14.Text = "رقم المجموعة";
        // 
        // txtOnyxEmployeeName
        // 
        txtOnyxEmployeeName.Location = new Point(60, 3);
        txtOnyxEmployeeName.Name = "txtOnyxEmployeeName";
        txtOnyxEmployeeName.Size = new Size(125, 30);
        txtOnyxEmployeeName.TabIndex = 13;
        // 
        // label13
        // 
        label13.AutoSize = true;
        label13.Location = new Point(191, 0);
        label13.Name = "label13";
        label13.Size = new Size(103, 23);
        label13.TabIndex = 12;
        label13.Text = "اسم الموظف";
        // 
        // txtOnyxEmployeeNumber
        // 
        txtOnyxEmployeeNumber.Location = new Point(356, 3);
        txtOnyxEmployeeNumber.Name = "txtOnyxEmployeeNumber";
        txtOnyxEmployeeNumber.Size = new Size(125, 30);
        txtOnyxEmployeeNumber.TabIndex = 11;
        // 
        // label12
        // 
        label12.AutoSize = true;
        label12.Location = new Point(530, 0);
        label12.Name = "label12";
        label12.Size = new Size(100, 23);
        label12.TabIndex = 10;
        label12.Text = "رقم الموظف";
        // 
        // lblUserName
        // 
        lblUserName.AutoSize = true;
        lblUserName.Dock = DockStyle.Fill;
        lblUserName.Location = new Point(823, 36);
        lblUserName.Name = "lblUserName";
        lblUserName.Size = new Size(143, 38);
        lblUserName.TabIndex = 0;
        lblUserName.Text = "اسم المستخدم *";
        // 
        // txtUserName
        // 
        txtUserName.BackColor = Color.Yellow;
        txtUserName.Dock = DockStyle.Fill;
        txtUserName.Location = new Point(636, 40);
        txtUserName.Margin = new Padding(3, 4, 3, 4);
        txtUserName.Name = "txtUserName";
        txtUserName.Size = new Size(181, 30);
        txtUserName.TabIndex = 0;
        // 
        // lblFullName
        // 
        lblFullName.AutoSize = true;
        lblFullName.Dock = DockStyle.Fill;
        lblFullName.Location = new Point(487, 36);
        lblFullName.Name = "lblFullName";
        lblFullName.Size = new Size(143, 38);
        lblFullName.TabIndex = 1;
        lblFullName.Text = "الاسم الكامل *";
        // 
        // txtFullName
        // 
        txtFullName.BackColor = Color.Yellow;
        txtFullName.Dock = DockStyle.Fill;
        txtFullName.Location = new Point(300, 40);
        txtFullName.Margin = new Padding(3, 4, 3, 4);
        txtFullName.Name = "txtFullName";
        txtFullName.Size = new Size(181, 30);
        txtFullName.TabIndex = 1;
        // 
        // lblRole
        // 
        lblRole.AutoSize = true;
        lblRole.Dock = DockStyle.Fill;
        lblRole.Location = new Point(191, 116);
        lblRole.Name = "lblRole";
        lblRole.Size = new Size(103, 39);
        lblRole.TabIndex = 2;
        lblRole.Text = "الدور *";
        // 
        // cboRole
        // 
        cboRole.BackColor = Color.Yellow;
        cboRole.Dock = DockStyle.Fill;
        cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
        cboRole.FlatStyle = FlatStyle.Flat;
        cboRole.Location = new Point(3, 120);
        cboRole.Margin = new Padding(3, 4, 3, 4);
        cboRole.Name = "cboRole";
        cboRole.Size = new Size(182, 31);
        cboRole.TabIndex = 2;
        // 
        // lblBranch
        // 
        lblBranch.AutoSize = true;
        lblBranch.Dock = DockStyle.Fill;
        lblBranch.Location = new Point(487, 74);
        lblBranch.Name = "lblBranch";
        lblBranch.Size = new Size(143, 42);
        lblBranch.TabIndex = 3;
        lblBranch.Text = "الفرع *";
        // 
        // cboBranch
        // 
        cboBranch.BackColor = Color.Yellow;
        cboBranch.Dock = DockStyle.Fill;
        cboBranch.DropDownStyle = ComboBoxStyle.DropDownList;
        cboBranch.FlatStyle = FlatStyle.Flat;
        cboBranch.Location = new Point(300, 78);
        cboBranch.Margin = new Padding(3, 4, 3, 4);
        cboBranch.Name = "cboBranch";
        cboBranch.Size = new Size(181, 31);
        cboBranch.TabIndex = 3;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.Location = new Point(823, 190);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(143, 39);
        lblStatus.TabIndex = 4;
        lblStatus.Text = "الحالة";
        // 
        // cboStatus
        // 
        cboStatus.Dock = DockStyle.Fill;
        cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cboStatus.Items.AddRange(new object[] { "ACTIVE", "DISABLED", "LOCKED" });
        cboStatus.Location = new Point(636, 194);
        cboStatus.Margin = new Padding(3, 4, 3, 4);
        cboStatus.Name = "cboStatus";
        cboStatus.Size = new Size(181, 31);
        cboStatus.TabIndex = 4;
        // 
        // chkRequirePasswordReset
        // 
        chkRequirePasswordReset.AutoSize = true;
        tlpUserData.SetColumnSpan(chkRequirePasswordReset, 3);
        chkRequirePasswordReset.Location = new Point(420, 159);
        chkRequirePasswordReset.Margin = new Padding(3, 4, 3, 4);
        chkRequirePasswordReset.Name = "chkRequirePasswordReset";
        chkRequirePasswordReset.Size = new Size(397, 27);
        chkRequirePasswordReset.TabIndex = 5;
        chkRequirePasswordReset.Text = "إلزام المستخدم بتغيير كلمة المرور عند الدخول التالي";
        // 
        // label6
        // 
        label6.AutoSize = true;
        label6.Location = new Point(863, 74);
        label6.Name = "label6";
        label6.Size = new Size(103, 23);
        label6.TabIndex = 6;
        label6.Text = "الاسم الأجنبي";
        // 
        // txtForeignName
        // 
        txtForeignName.Dock = DockStyle.Fill;
        txtForeignName.Location = new Point(636, 77);
        txtForeignName.Name = "txtForeignName";
        txtForeignName.Size = new Size(181, 30);
        txtForeignName.TabIndex = 7;
        // 
        // label11
        // 
        label11.AutoSize = true;
        label11.Location = new Point(857, 0);
        label11.Name = "label11";
        label11.Size = new Size(109, 23);
        label11.TabIndex = 8;
        label11.Text = "رقم المستخدم";
        // 
        // txtOnyxUserNumber
        // 
        txtOnyxUserNumber.Location = new Point(692, 3);
        txtOnyxUserNumber.Name = "txtOnyxUserNumber";
        txtOnyxUserNumber.Size = new Size(125, 30);
        txtOnyxUserNumber.TabIndex = 9;
        // 
        // cboOnyxGroupNumber
        // 
        cboOnyxGroupNumber.FormattingEnabled = true;
        cboOnyxGroupNumber.Location = new Point(666, 119);
        cboOnyxGroupNumber.Name = "cboOnyxGroupNumber";
        cboOnyxGroupNumber.Size = new Size(151, 31);
        cboOnyxGroupNumber.TabIndex = 14;
        // 
        // label15
        // 
        label15.AutoSize = true;
        label15.Location = new Point(242, 74);
        label15.Name = "label15";
        label15.Size = new Size(52, 23);
        label15.TabIndex = 16;
        label15.Text = "المدير";
        // 
        // label18
        // 
        label18.AutoSize = true;
        label18.Location = new Point(218, 36);
        label18.Name = "label18";
        label18.Size = new Size(76, 23);
        label18.TabIndex = 18;
        label18.Text = "رقم الفرع";
        // 
        // cboOnyxBranchNumber
        // 
        cboOnyxBranchNumber.FormattingEnabled = true;
        cboOnyxBranchNumber.Location = new Point(34, 39);
        cboOnyxBranchNumber.Name = "cboOnyxBranchNumber";
        cboOnyxBranchNumber.Size = new Size(151, 31);
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
        tlpOnyxManagerPair.Location = new Point(3, 77);
        tlpOnyxManagerPair.Name = "tlpOnyxManagerPair";
        tlpOnyxManagerPair.RowCount = 1;
        tlpOnyxManagerPair.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpOnyxManagerPair.Size = new Size(182, 36);
        tlpOnyxManagerPair.TabIndex = 20;
        // 
        // txtOnyxManager
        // 
        txtOnyxManager.Dock = DockStyle.Fill;
        txtOnyxManager.Location = new Point(3, 3);
        txtOnyxManager.Name = "txtOnyxManager";
        txtOnyxManager.Size = new Size(127, 30);
        txtOnyxManager.TabIndex = 18;
        // 
        // txtOnyxManagerCode
        // 
        txtOnyxManagerCode.Dock = DockStyle.Fill;
        txtOnyxManagerCode.Location = new Point(136, 3);
        txtOnyxManagerCode.Name = "txtOnyxManagerCode";
        txtOnyxManagerCode.Size = new Size(43, 30);
        txtOnyxManagerCode.TabIndex = 0;
        // 
        // pnlActions
        // 
        pnlActions.Controls.Add(flpActions);
        pnlActions.Dock = DockStyle.Top;
        pnlActions.Location = new Point(0, 0);
        pnlActions.Margin = new Padding(3, 4, 3, 4);
        pnlActions.Name = "pnlActions";
        pnlActions.Padding = new Padding(18, 11, 18, 11);
        pnlActions.Size = new Size(1509, 80);
        pnlActions.TabIndex = 2;
        // 
        // flpActions
        // 
        flpActions.Controls.Add(btnNew);
        flpActions.Controls.Add(btnEdit);
        flpActions.Controls.Add(btnSave);
        flpActions.Controls.Add(btnDisable);
        flpActions.Controls.Add(btnResetPassword);
        flpActions.Controls.Add(btnClose);
        flpActions.Dock = DockStyle.Fill;
        flpActions.Location = new Point(18, 11);
        flpActions.Margin = new Padding(3, 4, 3, 4);
        flpActions.Name = "flpActions";
        flpActions.Size = new Size(1473, 58);
        flpActions.TabIndex = 0;
        flpActions.WrapContents = false;
        // 
        // btnNew
        // 
        btnNew.AccessibleName = "إضافة";
        btnNew.Image = Properties.Resources.onyx_S01;
        btnNew.Location = new Point(1384, 4);
        btnNew.Margin = new Padding(3, 4, 3, 4);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(86, 31);
        btnNew.TabIndex = 0;
        // 
        // btnSave
        // 
        btnSave.AccessibleName = "حفظ";
        btnSave.Image = Properties.Resources.onyx_S10;
        btnSave.Location = new Point(1200, 4);
        btnSave.Margin = new Padding(3, 4, 3, 4);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(86, 31);
        btnSave.TabIndex = 2;
        // 
        // btnEdit
        // 
        btnEdit.AccessibleName = "تعديل";
        btnEdit.Image = Properties.Resources.onyx_S02;
        btnEdit.Location = new Point(1292, 4);
        btnEdit.Margin = new Padding(3, 4, 3, 4);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(86, 31);
        btnEdit.TabIndex = 1;
        // 
        // btnDisable
        // 
        btnDisable.Location = new Point(1108, 4);
        btnDisable.Margin = new Padding(3, 4, 3, 4);
        btnDisable.Name = "btnDisable";
        btnDisable.Size = new Size(86, 31);
        btnDisable.TabIndex = 3;
        btnDisable.Text = "إيقاف";
        // 
        // btnResetPassword
        // 
        btnResetPassword.Location = new Point(891, 4);
        btnResetPassword.Margin = new Padding(3, 4, 3, 4);
        btnResetPassword.Name = "btnResetPassword";
        btnResetPassword.Size = new Size(211, 31);
        btnResetPassword.TabIndex = 4;
        btnResetPassword.Text = "إعادة تعيين كلمة المرور";
        // 
        // btnClose
        // 
        btnClose.AccessibleName = "خروج";
        btnClose.Image = Properties.Resources.onyx_S13;
        btnClose.Location = new Point(799, 4);
        btnClose.Margin = new Padding(3, 4, 3, 4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(86, 31);
        btnClose.TabIndex = 5;
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
        // FrmUsersPermissions
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1509, 971);
        Controls.Add(splitMain);
        Controls.Add(auditFooter);
        Controls.Add(pnlActions);
        Margin = new Padding(3, 4, 3, 4);
        MinimumSize = new Size(1117, 855);
        Name = "FrmUsersPermissions";
        Text = "المستخدمون والصلاحيات";
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
        roleLookupV20.ResumeLayout(false);
        roleLookupV20.PerformLayout();
        tabPermissions.ResumeLayout(false);
        tlpPermissions.ResumeLayout(false);
        grpPermissionTree.ResumeLayout(false);
        grpPermissionTree.PerformLayout();
        grpModulePermissions.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvModulePermissions).EndInit();
        grpUserData.ResumeLayout(false);
        grpUserData.PerformLayout();
        tlpUserData.ResumeLayout(false);
        tlpUserData.PerformLayout();
        tlpOnyxManagerPair.ResumeLayout(false);
        tlpOnyxManagerPair.PerformLayout();
        pnlActions.ResumeLayout(false);
        flpActions.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
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
}
