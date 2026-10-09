namespace TransportERP.EmptyForms;
partial class UcOnyxSCREEN0082
{
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;

    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
    private Panel designerCloseHost = null!;
    private Button standardCommandAdd = null!;
    private Button standardCommandEdit = null!;
    private Button standardCommandDelete = null!;
    private Button standardCommandCancel = null!;
    private Button standardCommandView = null!;
    private Button standardCommandLast = null!;
    private Button standardCommandNext = null!;
    private Button standardCommandPrevious = null!;
    private Button standardCommandFirst = null!;
    private Button standardCommandPrint = null!;
    private Button standardCommandRefresh = null!;
    private Button standardCommandImport = null!;
    private Button standardCommandExport = null!;
    private Button standardCommandHelp = null!;

    private TableLayoutPanel mainLayout = null!;
    private TableLayoutPanel pnlHeader = null!;
    private Label lblTitle = null!;
    private FlowLayoutPanel pnlToolbar = null!;
    private Button btnClose = null!;
    private Label lblDataStatus = null!;
    private Panel pnlContent = null!;
    private TableLayoutPanel fieldsLayout = null!;
    private Button btn_T03_E0030 = null!;
    private ComboBox field_T03_E0024 = null!;
    private Label lbl_T03_E0024 = null!;
    private TextBox field_T03_E0025 = null!;
    private Label lbl_T03_E0025 = null!;
    private ComboBox field_T03_E0027 = null!;
    private Label lbl_T03_E0027 = null!;
    private CheckBox field_T03_E0028 = null!;
    private Label lbl_T03_E0028 = null!;
    private TextBox field_R05_AT_0149 = null!;
    private Label lbl_R05_AT_0149 = null!;
    private TextBox field_R05_AT_0150 = null!;
    private Label lbl_R05_AT_0150 = null!;
    private TabControl tabMain = null!;
    private TabPage tpMainData = null!;
    private TabPage tpPermissions = null!;
    private Label lblPermissions = null!;
    private void InitializeComponent()
    {
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        designerCloseHost = new Panel();
        standardCommandAdd = new Button();
        standardCommandEdit = new Button();
        standardCommandDelete = new Button();
        standardCommandCancel = new Button();
        standardCommandView = new Button();
        standardCommandLast = new Button();
        standardCommandNext = new Button();
        standardCommandPrevious = new Button();
        standardCommandFirst = new Button();
        standardCommandPrint = new Button();
        standardCommandRefresh = new Button();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
        mainLayout = new TableLayoutPanel();
        pnlHeader = new TableLayoutPanel();
        lblTitle = new Label();
        pnlToolbar = new FlowLayoutPanel();
        btn_T03_E0030 = new Button();
        btnClose = new Button();
        btnClearForm = new Button();
        lblDataStatus = new Label();
        pnlContent = new Panel();
        tabMain = new TabControl();
        tpMainData = new TabPage();
        fieldsLayout = new TableLayoutPanel();
        lbl_T03_E0024 = new Label();
        field_T03_E0024 = new ComboBox();
        lbl_T03_E0025 = new Label();
        field_T03_E0025 = new TextBox();
        lbl_T03_E0027 = new Label();
        field_T03_E0027 = new ComboBox();
        lbl_T03_E0028 = new Label();
        field_T03_E0028 = new CheckBox();
        lbl_R05_AT_0149 = new Label();
        field_R05_AT_0149 = new TextBox();
        lbl_R05_AT_0150 = new Label();
        field_R05_AT_0150 = new TextBox();
        tpPermissions = new TabPage();
        permissionsLayout = new TableLayoutPanel();
        lblPermissions = new Label();
        permissionActions = new FlowLayoutPanel();
        txtPermissionSearch = new TextBox();
        btnAddPermission = new Button();
        btnRemovePermission = new Button();
        dgvPermissions = new DataGridView();
        colPrincipalType = new DataGridViewComboBoxColumn();
        colPrincipal = new DataGridViewTextBoxColumn();
        colPermissionCode = new DataGridViewTextBoxColumn();
        colPermissionAction = new DataGridViewTextBoxColumn();
        colAllowed = new DataGridViewCheckBoxColumn();
        mainLayout.SuspendLayout();
        pnlHeader.SuspendLayout();
        pnlToolbar.SuspendLayout();
        pnlContent.SuspendLayout();
        tabMain.SuspendLayout();
        tpMainData.SuspendLayout();
        fieldsLayout.SuspendLayout();
        tpPermissions.SuspendLayout();
        permissionsLayout.SuspendLayout();
        permissionActions.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPermissions).BeginInit();
        SuspendLayout();
        // 
        // mainLayout
        // 
        mainLayout.ColumnCount = 1;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        mainLayout.Controls.Add(pnlHeader, 0, 0);
        mainLayout.Controls.Add(lblDataStatus, 0, 1);
        mainLayout.Controls.Add(pnlContent, 0, 2);
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Location = new Point(0, 0);
        mainLayout.Name = "mainLayout";
        mainLayout.RowCount = 3;
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainLayout.Size = new Size(1100, 760);
        mainLayout.TabIndex = 0;
        // 
        // pnlHeader
        // 
        pnlHeader.AutoSize = true;
        pnlHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlHeader.ColumnCount = 1;
        pnlHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        pnlHeader.Controls.Add(lblTitle, 0, 0);
        pnlHeader.Controls.Add(designerCommandBar, 0, 1);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(3, 3);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.RowCount = 2;
        pnlHeader.RowStyles.Add(new RowStyle());
        pnlHeader.RowStyles.Add(new RowStyle());
        pnlHeader.Size = new Size(1094, 121);
        pnlHeader.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.BackColor = Color.FromArgb(192, 192, 255);
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.Font = new Font("Segoe UI", 16F);
        lblTitle.Location = new Point(3, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(1088, 37);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "أنواع الطلبات";
        // 
        // pnlToolbar
        // 
        pnlToolbar.AutoSize = true;
        pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        pnlToolbar.Controls.Add(btnClearForm);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Location = new Point(3, 40);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.RightToLeft = RightToLeft.Yes;
        pnlToolbar.Size = new Size(1088, 39);
        pnlToolbar.TabIndex = 1;
        // 
        // btn_T03_E0030
        // 
        btn_T03_E0030.AutoSize = true;
        btn_T03_E0030.Enabled = false;
        btn_T03_E0030.Location = new Point(1015, 3);
        btn_T03_E0030.Name = "btn_T03_E0030";
        btn_T03_E0030.Size = new Size(70, 33);
        btn_T03_E0030.TabIndex = 0;
        btn_T03_E0030.Tag = "T03-E0030";
        btn_T03_E0030.Text = "حفظ";
        btn_T03_E0030.Click += SaveButton_Click;
        // 
        // btnClose
        // 
        btnClose.AutoSize = true;
        btnClose.Location = new Point(939, 3);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(70, 33);
        btnClose.TabIndex = 1;
        btnClose.Text = "إغلاق";
        btnClose.Click += BtnClose_Click;
        // 
        // btnClearForm
        // 
        btnClearForm.AutoSize = true;
        btnClearForm.Location = new Point(815, 3);
        btnClearForm.Name = "btnClearForm";
        btnClearForm.Size = new Size(118, 33);
        btnClearForm.TabIndex = 20;
        btnClearForm.Text = "تفريغ النموذج";
        btnClearForm.Click += ClearForm_Click;
        // 
        // lblDataStatus
        // 
        lblDataStatus.AutoSize = true;
        lblDataStatus.Dock = DockStyle.Top;
        lblDataStatus.Location = new Point(3, 127);
        lblDataStatus.Name = "lblDataStatus";
        lblDataStatus.Size = new Size(1094, 23);
        lblDataStatus.TabIndex = 1;
        lblDataStatus.Text = "لا تتوفر البيانات حاليًا. الحفظ غير متاح.";
        // 
        // pnlContent
        // 
        pnlContent.AutoScroll = true;
        pnlContent.Controls.Add(tabMain);
        pnlContent.Dock = DockStyle.Fill;
        pnlContent.Location = new Point(3, 153);
        pnlContent.Name = "pnlContent";
        pnlContent.Padding = new Padding(8);
        pnlContent.Size = new Size(1094, 604);
        pnlContent.TabIndex = 2;
        // 
        // tabMain
        // 
        tabMain.Controls.Add(tpMainData);
        tabMain.Controls.Add(tpPermissions);
        tabMain.Dock = DockStyle.Fill;
        tabMain.Location = new Point(8, 8);
        tabMain.MinimumSize = new Size(780, 380);
        tabMain.Name = "tabMain";
        tabMain.RightToLeftLayout = true;
        tabMain.SelectedIndex = 0;
        tabMain.Size = new Size(1078, 588);
        tabMain.TabIndex = 0;
        // 
        // tpMainData
        // 
        tpMainData.AutoScroll = true;
        tpMainData.Controls.Add(fieldsLayout);
        tpMainData.Location = new Point(4, 32);
        tpMainData.Name = "tpMainData";
        tpMainData.Size = new Size(1070, 552);
        tpMainData.TabIndex = 0;
        tpMainData.Text = "البيانات الرئيسية";
        // 
        // fieldsLayout
        // 
        fieldsLayout.AutoSize = true;
        fieldsLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        fieldsLayout.ColumnCount = 2;
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
        fieldsLayout.Controls.Add(lbl_T03_E0024, 0, 0);
        fieldsLayout.Controls.Add(field_T03_E0024, 1, 0);
        fieldsLayout.Controls.Add(lbl_T03_E0025, 0, 1);
        fieldsLayout.Controls.Add(field_T03_E0025, 1, 1);
        fieldsLayout.Controls.Add(lbl_T03_E0027, 0, 2);
        fieldsLayout.Controls.Add(field_T03_E0027, 1, 2);
        fieldsLayout.Controls.Add(lbl_T03_E0028, 0, 3);
        fieldsLayout.Controls.Add(field_T03_E0028, 1, 3);
        fieldsLayout.Controls.Add(lbl_R05_AT_0149, 0, 4);
        fieldsLayout.Controls.Add(field_R05_AT_0149, 1, 4);
        fieldsLayout.Controls.Add(lbl_R05_AT_0150, 0, 5);
        fieldsLayout.Controls.Add(field_R05_AT_0150, 1, 5);
        fieldsLayout.Dock = DockStyle.Top;
        fieldsLayout.Location = new Point(0, 0);
        fieldsLayout.MinimumSize = new Size(740, 0);
        fieldsLayout.Name = "fieldsLayout";
        fieldsLayout.RowCount = 6;
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.Size = new Size(1070, 266);
        fieldsLayout.TabIndex = 0;
        // 
        // lbl_T03_E0024
        // 
        lbl_T03_E0024.AutoSize = true;
        lbl_T03_E0024.Dock = DockStyle.Fill;
        lbl_T03_E0024.Location = new Point(488, 8);
        lbl_T03_E0024.Margin = new Padding(6, 8, 6, 8);
        lbl_T03_E0024.Name = "lbl_T03_E0024";
        lbl_T03_E0024.Size = new Size(576, 28);
        lbl_T03_E0024.TabIndex = 0;
        lbl_T03_E0024.Text = "نوع الطلب";
        lbl_T03_E0024.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T03_E0024
        // 
        field_T03_E0024.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_T03_E0024.AccessibleName = "نوع الطلب";
        field_T03_E0024.Dock = DockStyle.Fill;
        field_T03_E0024.DropDownStyle = ComboBoxStyle.DropDownList;
        field_T03_E0024.Location = new Point(6, 8);
        field_T03_E0024.Margin = new Padding(6, 8, 6, 8);
        field_T03_E0024.Name = "field_T03_E0024";
        field_T03_E0024.Size = new Size(470, 31);
        field_T03_E0024.TabIndex = 0;
        field_T03_E0024.Tag = "T03-E0024";
        // 
        // lbl_T03_E0025
        // 
        lbl_T03_E0025.AutoSize = true;
        lbl_T03_E0025.Dock = DockStyle.Fill;
        lbl_T03_E0025.Location = new Point(488, 52);
        lbl_T03_E0025.Margin = new Padding(6, 8, 6, 8);
        lbl_T03_E0025.Name = "lbl_T03_E0025";
        lbl_T03_E0025.Size = new Size(576, 30);
        lbl_T03_E0025.TabIndex = 1;
        lbl_T03_E0025.Text = "رقم النوع";
        lbl_T03_E0025.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T03_E0025
        // 
        field_T03_E0025.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_T03_E0025.AccessibleName = "رقم النوع";
        field_T03_E0025.Dock = DockStyle.Fill;
        field_T03_E0025.Location = new Point(6, 52);
        field_T03_E0025.Margin = new Padding(6, 8, 6, 8);
        field_T03_E0025.Name = "field_T03_E0025";
        field_T03_E0025.Size = new Size(470, 30);
        field_T03_E0025.TabIndex = 1;
        field_T03_E0025.Tag = "T03-E0025";
        // 
        // lbl_T03_E0027
        // 
        lbl_T03_E0027.AutoSize = true;
        lbl_T03_E0027.Dock = DockStyle.Fill;
        lbl_T03_E0027.Location = new Point(488, 98);
        lbl_T03_E0027.Margin = new Padding(6, 8, 6, 8);
        lbl_T03_E0027.Name = "lbl_T03_E0027";
        lbl_T03_E0027.Size = new Size(576, 28);
        lbl_T03_E0027.TabIndex = 2;
        lbl_T03_E0027.Text = "التسلسل";
        lbl_T03_E0027.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T03_E0027
        // 
        field_T03_E0027.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_T03_E0027.AccessibleName = "التسلسل";
        field_T03_E0027.Dock = DockStyle.Fill;
        field_T03_E0027.DropDownStyle = ComboBoxStyle.DropDownList;
        field_T03_E0027.Location = new Point(6, 98);
        field_T03_E0027.Margin = new Padding(6, 8, 6, 8);
        field_T03_E0027.Name = "field_T03_E0027";
        field_T03_E0027.Size = new Size(470, 31);
        field_T03_E0027.TabIndex = 2;
        field_T03_E0027.Tag = "T03-E0027";
        // 
        // lbl_T03_E0028
        // 
        lbl_T03_E0028.AutoSize = true;
        lbl_T03_E0028.Dock = DockStyle.Fill;
        lbl_T03_E0028.Location = new Point(488, 142);
        lbl_T03_E0028.Margin = new Padding(6, 8, 6, 8);
        lbl_T03_E0028.Name = "lbl_T03_E0028";
        lbl_T03_E0028.Size = new Size(576, 24);
        lbl_T03_E0028.TabIndex = 3;
        lbl_T03_E0028.Text = "السماح بتعديل المبالغ في إنزال المستندات";
        lbl_T03_E0028.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T03_E0028
        // 
        field_T03_E0028.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_T03_E0028.AccessibleName = "السماح بتعديل المبالغ في إنزال المستندات";
        field_T03_E0028.Checked = true;
        field_T03_E0028.CheckState = CheckState.Indeterminate;
        field_T03_E0028.Dock = DockStyle.Fill;
        field_T03_E0028.Location = new Point(6, 142);
        field_T03_E0028.Margin = new Padding(6, 8, 6, 8);
        field_T03_E0028.Name = "field_T03_E0028";
        field_T03_E0028.Size = new Size(470, 24);
        field_T03_E0028.TabIndex = 3;
        field_T03_E0028.Tag = "T03-E0028";
        field_T03_E0028.ThreeState = true;
        // 
        // lbl_R05_AT_0149
        // 
        lbl_R05_AT_0149.AutoSize = true;
        lbl_R05_AT_0149.Dock = DockStyle.Fill;
        lbl_R05_AT_0149.Location = new Point(488, 182);
        lbl_R05_AT_0149.Margin = new Padding(6, 8, 6, 8);
        lbl_R05_AT_0149.Name = "lbl_R05_AT_0149";
        lbl_R05_AT_0149.Size = new Size(576, 30);
        lbl_R05_AT_0149.TabIndex = 4;
        lbl_R05_AT_0149.Text = "اسم النوع المحلي";
        lbl_R05_AT_0149.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_R05_AT_0149
        // 
        field_R05_AT_0149.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_R05_AT_0149.AccessibleName = "اسم النوع المحلي";
        field_R05_AT_0149.Dock = DockStyle.Fill;
        field_R05_AT_0149.Location = new Point(6, 182);
        field_R05_AT_0149.Margin = new Padding(6, 8, 6, 8);
        field_R05_AT_0149.Name = "field_R05_AT_0149";
        field_R05_AT_0149.Size = new Size(470, 30);
        field_R05_AT_0149.TabIndex = 4;
        field_R05_AT_0149.Tag = "R05-AT-0149";
        // 
        // lbl_R05_AT_0150
        // 
        lbl_R05_AT_0150.AutoSize = true;
        lbl_R05_AT_0150.Dock = DockStyle.Fill;
        lbl_R05_AT_0150.Location = new Point(488, 228);
        lbl_R05_AT_0150.Margin = new Padding(6, 8, 6, 8);
        lbl_R05_AT_0150.Name = "lbl_R05_AT_0150";
        lbl_R05_AT_0150.Size = new Size(576, 30);
        lbl_R05_AT_0150.TabIndex = 5;
        lbl_R05_AT_0150.Text = "اسم النوع الأجنبي";
        lbl_R05_AT_0150.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_R05_AT_0150
        // 
        field_R05_AT_0150.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_R05_AT_0150.AccessibleName = "اسم النوع الأجنبي";
        field_R05_AT_0150.Dock = DockStyle.Fill;
        field_R05_AT_0150.Location = new Point(6, 228);
        field_R05_AT_0150.Margin = new Padding(6, 8, 6, 8);
        field_R05_AT_0150.Name = "field_R05_AT_0150";
        field_R05_AT_0150.RightToLeft = RightToLeft.No;
        field_R05_AT_0150.Size = new Size(470, 30);
        field_R05_AT_0150.TabIndex = 5;
        field_R05_AT_0150.Tag = "R05-AT-0150";
        // 
        // tpPermissions
        // 
        tpPermissions.AutoScroll = true;
        tpPermissions.Controls.Add(permissionsLayout);
        tpPermissions.Location = new Point(4, 32);
        tpPermissions.Name = "tpPermissions";
        tpPermissions.Size = new Size(772, 344);
        tpPermissions.TabIndex = 1;
        tpPermissions.Tag = "T03-E0029";
        tpPermissions.Text = "الصلاحيات";
        // 
        // permissionsLayout
        // 
        permissionsLayout.ColumnCount = 1;
        permissionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        permissionsLayout.Controls.Add(lblPermissions, 0, 0);
        permissionsLayout.Controls.Add(permissionActions, 0, 1);
        permissionsLayout.Controls.Add(dgvPermissions, 0, 2);
        permissionsLayout.Dock = DockStyle.Fill;
        permissionsLayout.Location = new Point(0, 0);
        permissionsLayout.Name = "permissionsLayout";
        permissionsLayout.RowCount = 3;
        permissionsLayout.RowStyles.Add(new RowStyle());
        permissionsLayout.RowStyles.Add(new RowStyle());
        permissionsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        permissionsLayout.Size = new Size(772, 344);
        permissionsLayout.TabIndex = 0;
        // 
        // lblPermissions
        // 
        lblPermissions.AutoSize = true;
        lblPermissions.Dock = DockStyle.Top;
        lblPermissions.Location = new Point(3, 0);
        lblPermissions.Name = "lblPermissions";
        lblPermissions.Size = new Size(766, 23);
        lblPermissions.TabIndex = 0;
        lblPermissions.Text = "مسودة صلاحيات محلية غير محفوظة. لا تُطبق حتى ربط الخدمة.";
        // 
        // permissionActions
        // 
        permissionActions.AutoSize = true;
        permissionActions.Controls.Add(txtPermissionSearch);
        permissionActions.Controls.Add(btnAddPermission);
        permissionActions.Controls.Add(btnRemovePermission);
        permissionActions.Dock = DockStyle.Top;
        permissionActions.Location = new Point(3, 26);
        permissionActions.Name = "permissionActions";
        permissionActions.RightToLeft = RightToLeft.Yes;
        permissionActions.Size = new Size(766, 39);
        permissionActions.TabIndex = 1;
        // 
        // txtPermissionSearch
        // 
        txtPermissionSearch.AccessibleName = "بحث الصلاحيات المحلية";
        txtPermissionSearch.Location = new Point(523, 3);
        txtPermissionSearch.Name = "txtPermissionSearch";
        txtPermissionSearch.PlaceholderText = "بحث في المسودة المحلية";
        txtPermissionSearch.Size = new Size(240, 30);
        txtPermissionSearch.TabIndex = 0;
        txtPermissionSearch.TextChanged += PermissionSearchChanged;
        // 
        // btnAddPermission
        // 
        btnAddPermission.AutoSize = true;
        btnAddPermission.Location = new Point(414, 3);
        btnAddPermission.Name = "btnAddPermission";
        btnAddPermission.Size = new Size(103, 33);
        btnAddPermission.TabIndex = 1;
        btnAddPermission.Text = "إضافة سطر";
        btnAddPermission.Click += AddPermission_Click;
        // 
        // btnRemovePermission
        // 
        btnRemovePermission.AutoSize = true;
        btnRemovePermission.Location = new Point(247, 3);
        btnRemovePermission.Name = "btnRemovePermission";
        btnRemovePermission.Size = new Size(161, 33);
        btnRemovePermission.TabIndex = 2;
        btnRemovePermission.Text = "حذف السطر المحلي";
        btnRemovePermission.Click += RemovePermission_Click;
        // 
        // dgvPermissions
        // 
        dgvPermissions.AllowUserToAddRows = false;
        dgvPermissions.AllowUserToDeleteRows = false;
        dgvPermissions.ColumnHeadersHeight = 29;
        dgvPermissions.Columns.AddRange(new DataGridViewColumn[] { colPrincipalType, colPrincipal, colPermissionCode, colPermissionAction, colAllowed });
        dgvPermissions.Dock = DockStyle.Fill;
        dgvPermissions.Location = new Point(3, 71);
        dgvPermissions.MultiSelect = false;
        dgvPermissions.Name = "dgvPermissions";
        dgvPermissions.RowHeadersVisible = false;
        dgvPermissions.RowHeadersWidth = 51;
        dgvPermissions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPermissions.Size = new Size(766, 270);
        dgvPermissions.TabIndex = 1;
        dgvPermissions.CellValueChanged += PermissionValueChanged;
        dgvPermissions.CurrentCellDirtyStateChanged += PermissionCellDirtyChanged;
        // 
        // colPrincipalType
        // 
        colPrincipalType.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colPrincipalType.HeaderText = "نوع الجهة";
        colPrincipalType.Items.AddRange(new object[] { "مستخدم", "دور" });
        colPrincipalType.MinimumWidth = 6;
        colPrincipalType.Name = "colPrincipalType";
        // 
        // colPrincipal
        // 
        colPrincipal.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colPrincipal.HeaderText = "المستخدم أو الدور";
        colPrincipal.MinimumWidth = 6;
        colPrincipal.Name = "colPrincipal";
        // 
        // colPermissionCode
        // 
        colPermissionCode.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colPermissionCode.HeaderText = "رمز الصلاحية";
        colPermissionCode.MinimumWidth = 6;
        colPermissionCode.Name = "colPermissionCode";
        // 
        // colPermissionAction
        // 
        colPermissionAction.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colPermissionAction.HeaderText = "اسم الصلاحية";
        colPermissionAction.MinimumWidth = 6;
        colPermissionAction.Name = "colPermissionAction";
        // 
        // colAllowed
        // 
        colAllowed.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colAllowed.HeaderText = "السماح";
        colAllowed.MinimumWidth = 6;
        colAllowed.Name = "colAllowed";
        colAllowed.ThreeState = true;
        // 
        // UcOnyxSCREEN0082
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(mainLayout);
        Font = new Font("Segoe UI", 10F);
        Name = "UcOnyxSCREEN0082";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1100, 760);
        Tag = "ONYX.SCREEN-0082";
        mainLayout.ResumeLayout(false);
        mainLayout.PerformLayout();
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlToolbar.ResumeLayout(false);
        pnlToolbar.PerformLayout();
        pnlContent.ResumeLayout(false);
        tabMain.ResumeLayout(false);
        tpMainData.ResumeLayout(false);
        tpMainData.PerformLayout();
        fieldsLayout.ResumeLayout(false);
        fieldsLayout.PerformLayout();
        tpPermissions.ResumeLayout(false);
        permissionsLayout.ResumeLayout(false);
        permissionsLayout.PerformLayout();
        permissionActions.ResumeLayout(false);
        permissionActions.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPermissions).EndInit();
        ResumeLayout(false);
    
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(3, 40);
        designerCommandBar.Size = new Size(1088, 39);
        designerCommandBar.TabIndex = 1;
        designerCommandBar.RightToLeft = RightToLeft.Yes;
        designerCommandBar.Controls.Add(pnlToolbar);
        pnlToolbar.Dock = DockStyle.Fill;
        pnlToolbar.Margin = Padding.Empty;
        designerCommandBar.Name = "designerCommandBar";
        designerCommandBar.BackColor = Color.FromArgb(239, 239, 239);
        designerCommandBar.BorderStyle = BorderStyle.FixedSingle;
        designerCommandBar.Controls.Add(designerCloseHost);
        designerCloseHost.Dock = DockStyle.Left;
        designerCloseHost.Width = 28;
        designerCloseHost.Name = "designerCloseHost";
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Dock = DockStyle.Fill;
        pnlToolbar.AutoSize = false;
        pnlToolbar.WrapContents = false;
        pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
        pnlToolbar.RightToLeft = RightToLeft.No;
        pnlToolbar.Padding = new Padding(2);
        designerCommandBar.MinimumSize = new Size(0, 30);
        designerCommandBar.Height = 44;
        standardCommandAdd.Name = "standardCommandAdd";
        standardCommandAdd.Enabled = false;
        standardCommandAdd.Visible = true;
        standardCommandAdd.AccessibleName = "إضافة";
        standardCommandEdit.Name = "standardCommandEdit";
        standardCommandEdit.Enabled = false;
        standardCommandEdit.Visible = true;
        standardCommandEdit.AccessibleName = "تعديل";
        standardCommandDelete.Name = "standardCommandDelete";
        standardCommandDelete.Enabled = false;
        standardCommandDelete.Visible = true;
        standardCommandDelete.AccessibleName = "حذف";
        standardCommandCancel.Name = "standardCommandCancel";
        standardCommandCancel.Enabled = false;
        standardCommandCancel.Visible = true;
        standardCommandCancel.AccessibleName = "تراجع";
        standardCommandView.Name = "standardCommandView";
        standardCommandView.Enabled = false;
        standardCommandView.Visible = true;
        standardCommandView.AccessibleName = "عرض";
        standardCommandLast.Name = "standardCommandLast";
        standardCommandLast.Enabled = false;
        standardCommandLast.Visible = true;
        standardCommandLast.AccessibleName = "الأخير";
        standardCommandNext.Name = "standardCommandNext";
        standardCommandNext.Enabled = false;
        standardCommandNext.Visible = true;
        standardCommandNext.AccessibleName = "التالي";
        standardCommandPrevious.Name = "standardCommandPrevious";
        standardCommandPrevious.Enabled = false;
        standardCommandPrevious.Visible = true;
        standardCommandPrevious.AccessibleName = "السابق";
        standardCommandFirst.Name = "standardCommandFirst";
        standardCommandFirst.Enabled = false;
        standardCommandFirst.Visible = true;
        standardCommandFirst.AccessibleName = "الأول";
        standardCommandPrint.Name = "standardCommandPrint";
        standardCommandPrint.Enabled = false;
        standardCommandPrint.Visible = true;
        standardCommandPrint.AccessibleName = "طباعة";
        standardCommandRefresh.Name = "standardCommandRefresh";
        standardCommandRefresh.Enabled = false;
        standardCommandRefresh.Visible = false;
        standardCommandRefresh.AccessibleName = "تحديث";
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
        standardCommandAdd.AutoSize = false;
        standardCommandAdd.Dock = DockStyle.None;
        standardCommandAdd.MinimumSize = Size.Empty;
        standardCommandAdd.Size = new Size(26, 24);
        standardCommandAdd.Margin = new Padding(1);
        standardCommandAdd.FlatStyle = FlatStyle.Flat;
        standardCommandAdd.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandAdd.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Add;
        standardCommandAdd.Text = "";
        pnlToolbar.Controls.Add(standardCommandAdd);
        designerCommandBar.SetCommandRole(standardCommandAdd, TransportERP.Desktop.CoreUI.DesignerCommandRole.Add);
        standardCommandEdit.AutoSize = false;
        standardCommandEdit.Dock = DockStyle.None;
        standardCommandEdit.MinimumSize = Size.Empty;
        standardCommandEdit.Size = new Size(26, 24);
        standardCommandEdit.Margin = new Padding(1);
        standardCommandEdit.FlatStyle = FlatStyle.Flat;
        standardCommandEdit.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandEdit.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Edit;
        standardCommandEdit.Text = "";
        pnlToolbar.Controls.Add(standardCommandEdit);
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
        pnlToolbar.Controls.Add(standardCommandDelete);
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
        pnlToolbar.Controls.Add(standardCommandCancel);
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
        pnlToolbar.Controls.Add(standardCommandView);
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
        pnlToolbar.Controls.Add(standardCommandLast);
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
        pnlToolbar.Controls.Add(standardCommandNext);
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
        pnlToolbar.Controls.Add(standardCommandPrevious);
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
        pnlToolbar.Controls.Add(standardCommandFirst);
        designerCommandBar.SetCommandRole(standardCommandFirst, TransportERP.Desktop.CoreUI.DesignerCommandRole.First);
        btn_T03_E0030.AutoSize = false;
        btn_T03_E0030.Dock = DockStyle.None;
        btn_T03_E0030.MinimumSize = Size.Empty;
        btn_T03_E0030.Size = new Size(26, 24);
        btn_T03_E0030.Margin = new Padding(1);
        btn_T03_E0030.FlatStyle = FlatStyle.Flat;
        btn_T03_E0030.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btn_T03_E0030.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Save;
        btn_T03_E0030.Text = "";
        pnlToolbar.Controls.Add(btn_T03_E0030);
        designerCommandBar.SetCommandRole(btn_T03_E0030, TransportERP.Desktop.CoreUI.DesignerCommandRole.Save);
        standardCommandPrint.AutoSize = false;
        standardCommandPrint.Dock = DockStyle.None;
        standardCommandPrint.MinimumSize = Size.Empty;
        standardCommandPrint.Size = new Size(26, 24);
        standardCommandPrint.Margin = new Padding(1);
        standardCommandPrint.FlatStyle = FlatStyle.Flat;
        standardCommandPrint.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandPrint.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Print;
        standardCommandPrint.Text = "";
        pnlToolbar.Controls.Add(standardCommandPrint);
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
        standardCommandRefresh.AutoSize = false;
        standardCommandRefresh.Dock = DockStyle.None;
        standardCommandRefresh.MinimumSize = Size.Empty;
        standardCommandRefresh.Size = new Size(26, 24);
        standardCommandRefresh.Margin = new Padding(1);
        standardCommandRefresh.FlatStyle = FlatStyle.Flat;
        standardCommandRefresh.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandRefresh.Text = "تحديث";
        pnlToolbar.Controls.Add(standardCommandRefresh);
        designerCommandBar.SetCommandRole(standardCommandRefresh, TransportERP.Desktop.CoreUI.DesignerCommandRole.Refresh);
        standardCommandImport.AutoSize = false;
        standardCommandImport.Dock = DockStyle.None;
        standardCommandImport.MinimumSize = Size.Empty;
        standardCommandImport.Size = new Size(26, 24);
        standardCommandImport.Margin = new Padding(1);
        standardCommandImport.FlatStyle = FlatStyle.Flat;
        standardCommandImport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandImport.Text = "استيراد";
        pnlToolbar.Controls.Add(standardCommandImport);
        designerCommandBar.SetCommandRole(standardCommandImport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Import);
        standardCommandExport.AutoSize = false;
        standardCommandExport.Dock = DockStyle.None;
        standardCommandExport.MinimumSize = Size.Empty;
        standardCommandExport.Size = new Size(26, 24);
        standardCommandExport.Margin = new Padding(1);
        standardCommandExport.FlatStyle = FlatStyle.Flat;
        standardCommandExport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandExport.Text = "تصدير";
        pnlToolbar.Controls.Add(standardCommandExport);
        designerCommandBar.SetCommandRole(standardCommandExport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Export);
        standardCommandHelp.AutoSize = false;
        standardCommandHelp.Dock = DockStyle.None;
        standardCommandHelp.MinimumSize = Size.Empty;
        standardCommandHelp.Size = new Size(26, 24);
        standardCommandHelp.Margin = new Padding(1);
        standardCommandHelp.FlatStyle = FlatStyle.Flat;
        standardCommandHelp.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandHelp.Text = "مساعدة";
        pnlToolbar.Controls.Add(standardCommandHelp);
        designerCommandBar.SetCommandRole(standardCommandHelp, TransportERP.Desktop.CoreUI.DesignerCommandRole.Help);
    
        // Shared audit presentation; original sources remain owned by this screen.
        standardAuditMetadata = new TransportERP.Desktop.CoreUI.AuditMetadataControl();
        standardAuditMetadata.Name = "standardAuditMetadata";
        standardAuditMetadata.Dock = DockStyle.Bottom;
        standardAuditMetadata.Margin = Padding.Empty;
        standardAuditMetadata.TabStop = false;
        standardAuditMetadata.Size = new Size(800, 64);
        Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    private Button btnClearForm = null!;
    private TableLayoutPanel permissionsLayout = null!;
    private FlowLayoutPanel permissionActions = null!;
    private TextBox txtPermissionSearch = null!;
    private Button btnAddPermission = null!;
    private Button btnRemovePermission = null!;
    private DataGridView dgvPermissions = null!;
    private DataGridViewComboBoxColumn colPrincipalType = null!;
    private DataGridViewTextBoxColumn colPrincipal = null!;
    private DataGridViewTextBoxColumn colPermissionAction = null!;
    private DataGridViewTextBoxColumn colPermissionCode = null!;
    private DataGridViewCheckBoxColumn colAllowed = null!;
}
