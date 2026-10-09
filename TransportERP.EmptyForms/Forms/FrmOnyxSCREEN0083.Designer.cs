namespace TransportERP.EmptyForms;
partial class UcOnyxSCREEN0083
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
    private Button btn_T03_E0043 = null!;
    private TextBox field_T03_E0033 = null!;
    private Label lbl_T03_E0033 = null!;
    private ComboBox field_T03_E0035 = null!;
    private Label lbl_T03_E0035 = null!;
    private CheckBox field_T03_E0036 = null!;
    private Label lbl_T03_E0036 = null!;
    private CheckBox field_T03_E0037 = null!;
    private Label lbl_T03_E0037 = null!;
    private CheckBox field_T03_E0038 = null!;
    private Label lbl_T03_E0038 = null!;
    private ComboBox field_T03_E0039 = null!;
    private Label lbl_T03_E0039 = null!;
    private CheckBox field_T03_E0040 = null!;
    private Label lbl_T03_E0040 = null!;
    private CheckBox field_T03_E0041 = null!;
    private Label lbl_T03_E0041 = null!;
    private TextBox field_R05_AT_0151 = null!;
    private Label lbl_R05_AT_0151 = null!;
    private TextBox field_R05_AT_0152 = null!;
    private Label lbl_R05_AT_0152 = null!;
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
        btn_T03_E0043 = new Button();
        btnClose = new Button();
        btnClearForm = new Button();
        lblDataStatus = new Label();
        pnlContent = new Panel();
        tabMain = new TabControl();
        tpMainData = new TabPage();
        fieldsLayout = new TableLayoutPanel();
        lbl_T03_E0033 = new Label();
        field_T03_E0033 = new TextBox();
        lbl_T03_E0035 = new Label();
        field_T03_E0035 = new ComboBox();
        lbl_T03_E0036 = new Label();
        field_T03_E0036 = new CheckBox();
        lbl_T03_E0037 = new Label();
        field_T03_E0037 = new CheckBox();
        lbl_T03_E0038 = new Label();
        field_T03_E0038 = new CheckBox();
        lbl_T03_E0039 = new Label();
        field_T03_E0039 = new ComboBox();
        lbl_T03_E0040 = new Label();
        field_T03_E0040 = new CheckBox();
        lbl_T03_E0041 = new Label();
        field_T03_E0041 = new CheckBox();
        lbl_R05_AT_0151 = new Label();
        field_R05_AT_0151 = new TextBox();
        lbl_R05_AT_0152 = new Label();
        field_R05_AT_0152 = new TextBox();
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
        pnlHeader.Size = new Size(1094, 114);
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
        lblTitle.Size = new Size(1088, 30);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "أنواع قيود اليومية";
        // 
        // pnlToolbar
        // 
        pnlToolbar.AutoSize = true;
        pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        pnlToolbar.Controls.Add(btnClearForm);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Location = new Point(3, 33);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.RightToLeft = RightToLeft.Yes;
        pnlToolbar.Size = new Size(1088, 39);
        pnlToolbar.TabIndex = 1;
        // 
        // btn_T03_E0043
        // 
        btn_T03_E0043.AutoSize = true;
        btn_T03_E0043.Enabled = false;
        btn_T03_E0043.Location = new Point(1015, 3);
        btn_T03_E0043.Name = "btn_T03_E0043";
        btn_T03_E0043.Size = new Size(70, 33);
        btn_T03_E0043.TabIndex = 0;
        btn_T03_E0043.Tag = "T03-E0043";
        btn_T03_E0043.Text = "حفظ";
        btn_T03_E0043.Click += SaveButton_Click;
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
        lblDataStatus.Location = new Point(3, 120);
        lblDataStatus.Name = "lblDataStatus";
        lblDataStatus.Size = new Size(1094, 19);
        lblDataStatus.TabIndex = 1;
        lblDataStatus.Text = "لا تتوفر البيانات حاليًا. الحفظ غير متاح.";
        // 
        // pnlContent
        // 
        pnlContent.AutoScroll = true;
        pnlContent.Controls.Add(tabMain);
        pnlContent.Dock = DockStyle.Fill;
        pnlContent.Location = new Point(3, 142);
        pnlContent.Name = "pnlContent";
        pnlContent.Padding = new Padding(8);
        pnlContent.Size = new Size(1094, 615);
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
        tabMain.Size = new Size(1078, 599);
        tabMain.TabIndex = 0;
        // 
        // tpMainData
        // 
        tpMainData.AutoScroll = true;
        tpMainData.Controls.Add(fieldsLayout);
        tpMainData.Location = new Point(4, 26);
        tpMainData.Name = "tpMainData";
        tpMainData.Size = new Size(1070, 569);
        tpMainData.TabIndex = 0;
        tpMainData.Text = "البيانات الرئيسية";
        // 
        // fieldsLayout
        // 
        fieldsLayout.AutoSize = true;
        fieldsLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        fieldsLayout.ColumnCount = 4;
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857141F));
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 39.2857132F));
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32.1428566F));
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857141F));
        fieldsLayout.Controls.Add(lbl_T03_E0033, 1, 0);
        fieldsLayout.Controls.Add(field_T03_E0033, 2, 0);
        fieldsLayout.Controls.Add(lbl_T03_E0035, 1, 1);
        fieldsLayout.Controls.Add(field_T03_E0035, 2, 1);
        fieldsLayout.Controls.Add(lbl_T03_E0036, 1, 2);
        fieldsLayout.Controls.Add(field_T03_E0036, 2, 2);
        fieldsLayout.Controls.Add(lbl_T03_E0037, 1, 3);
        fieldsLayout.Controls.Add(field_T03_E0037, 2, 3);
        fieldsLayout.Controls.Add(lbl_T03_E0038, 1, 4);
        fieldsLayout.Controls.Add(field_T03_E0038, 2, 4);
        fieldsLayout.Controls.Add(lbl_T03_E0039, 1, 5);
        fieldsLayout.Controls.Add(field_T03_E0039, 2, 5);
        fieldsLayout.Controls.Add(lbl_T03_E0040, 1, 6);
        fieldsLayout.Controls.Add(field_T03_E0040, 2, 6);
        fieldsLayout.Controls.Add(lbl_T03_E0041, 1, 7);
        fieldsLayout.Controls.Add(field_T03_E0041, 2, 7);
        fieldsLayout.Controls.Add(lbl_R05_AT_0151, 1, 8);
        fieldsLayout.Controls.Add(field_R05_AT_0151, 2, 8);
        fieldsLayout.Controls.Add(lbl_R05_AT_0152, 1, 9);
        fieldsLayout.Controls.Add(field_R05_AT_0152, 2, 9);
        fieldsLayout.Dock = DockStyle.Top;
        fieldsLayout.Location = new Point(0, 0);
        fieldsLayout.MinimumSize = new Size(740, 0);
        fieldsLayout.Name = "fieldsLayout";
        fieldsLayout.RowCount = 10;
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.Size = new Size(1070, 401);
        fieldsLayout.TabIndex = 0;
        fieldsLayout.Paint += fieldsLayout_Paint;
        // 
        // lbl_T03_E0033
        // 
        lbl_T03_E0033.AutoSize = true;
        lbl_T03_E0033.Dock = DockStyle.Fill;
        lbl_T03_E0033.Location = new Point(504, 8);
        lbl_T03_E0033.Margin = new Padding(6, 8, 6, 8);
        lbl_T03_E0033.Name = "lbl_T03_E0033";
        lbl_T03_E0033.Size = new Size(408, 25);
        lbl_T03_E0033.TabIndex = 0;
        lbl_T03_E0033.Text = "رقم النوع";
        lbl_T03_E0033.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T03_E0033
        // 
        field_T03_E0033.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_T03_E0033.AccessibleName = "رقم النوع";
        field_T03_E0033.Dock = DockStyle.Fill;
        field_T03_E0033.Location = new Point(161, 8);
        field_T03_E0033.Margin = new Padding(6, 8, 6, 8);
        field_T03_E0033.Name = "field_T03_E0033";
        field_T03_E0033.Size = new Size(331, 25);
        field_T03_E0033.TabIndex = 0;
        field_T03_E0033.Tag = "T03-E0033";
        // 
        // lbl_T03_E0035
        // 
        lbl_T03_E0035.AutoSize = true;
        lbl_T03_E0035.Dock = DockStyle.Fill;
        lbl_T03_E0035.Location = new Point(504, 49);
        lbl_T03_E0035.Margin = new Padding(6, 8, 6, 8);
        lbl_T03_E0035.Name = "lbl_T03_E0035";
        lbl_T03_E0035.Size = new Size(408, 23);
        lbl_T03_E0035.TabIndex = 1;
        lbl_T03_E0035.Text = "التسلسل";
        lbl_T03_E0035.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T03_E0035
        // 
        field_T03_E0035.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_T03_E0035.AccessibleName = "التسلسل";
        field_T03_E0035.Dock = DockStyle.Fill;
        field_T03_E0035.DropDownStyle = ComboBoxStyle.DropDownList;
        field_T03_E0035.Location = new Point(161, 49);
        field_T03_E0035.Margin = new Padding(6, 8, 6, 8);
        field_T03_E0035.Name = "field_T03_E0035";
        field_T03_E0035.Size = new Size(331, 25);
        field_T03_E0035.TabIndex = 1;
        field_T03_E0035.Tag = "T03-E0035";
        // 
        // lbl_T03_E0036
        // 
        lbl_T03_E0036.AutoSize = true;
        lbl_T03_E0036.Dock = DockStyle.Fill;
        lbl_T03_E0036.Location = new Point(504, 88);
        lbl_T03_E0036.Margin = new Padding(6, 8, 6, 8);
        lbl_T03_E0036.Name = "lbl_T03_E0036";
        lbl_T03_E0036.Size = new Size(408, 24);
        lbl_T03_E0036.TabIndex = 2;
        lbl_T03_E0036.Text = "صرف";
        lbl_T03_E0036.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T03_E0036
        // 
        field_T03_E0036.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_T03_E0036.AccessibleName = "صرف";
        field_T03_E0036.Checked = true;
        field_T03_E0036.CheckState = CheckState.Indeterminate;
        field_T03_E0036.Dock = DockStyle.Fill;
        field_T03_E0036.Location = new Point(161, 88);
        field_T03_E0036.Margin = new Padding(6, 8, 6, 8);
        field_T03_E0036.Name = "field_T03_E0036";
        field_T03_E0036.Size = new Size(331, 24);
        field_T03_E0036.TabIndex = 2;
        field_T03_E0036.Tag = "T03-E0036";
        field_T03_E0036.ThreeState = true;
        // 
        // lbl_T03_E0037
        // 
        lbl_T03_E0037.AutoSize = true;
        lbl_T03_E0037.Dock = DockStyle.Fill;
        lbl_T03_E0037.Location = new Point(504, 128);
        lbl_T03_E0037.Margin = new Padding(6, 8, 6, 8);
        lbl_T03_E0037.Name = "lbl_T03_E0037";
        lbl_T03_E0037.Size = new Size(408, 24);
        lbl_T03_E0037.TabIndex = 3;
        lbl_T03_E0037.Text = "قبض";
        lbl_T03_E0037.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T03_E0037
        // 
        field_T03_E0037.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_T03_E0037.AccessibleName = "قبض";
        field_T03_E0037.Checked = true;
        field_T03_E0037.CheckState = CheckState.Indeterminate;
        field_T03_E0037.Dock = DockStyle.Fill;
        field_T03_E0037.Location = new Point(161, 128);
        field_T03_E0037.Margin = new Padding(6, 8, 6, 8);
        field_T03_E0037.Name = "field_T03_E0037";
        field_T03_E0037.Size = new Size(331, 24);
        field_T03_E0037.TabIndex = 3;
        field_T03_E0037.Tag = "T03-E0037";
        field_T03_E0037.ThreeState = true;
        // 
        // lbl_T03_E0038
        // 
        lbl_T03_E0038.AutoSize = true;
        lbl_T03_E0038.Dock = DockStyle.Fill;
        lbl_T03_E0038.Location = new Point(504, 168);
        lbl_T03_E0038.Margin = new Padding(6, 8, 6, 8);
        lbl_T03_E0038.Name = "lbl_T03_E0038";
        lbl_T03_E0038.Size = new Size(408, 24);
        lbl_T03_E0038.TabIndex = 4;
        lbl_T03_E0038.Text = "الكمبيالات";
        lbl_T03_E0038.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T03_E0038
        // 
        field_T03_E0038.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_T03_E0038.AccessibleName = "الكمبيالات";
        field_T03_E0038.Checked = true;
        field_T03_E0038.CheckState = CheckState.Indeterminate;
        field_T03_E0038.Dock = DockStyle.Fill;
        field_T03_E0038.Location = new Point(161, 168);
        field_T03_E0038.Margin = new Padding(6, 8, 6, 8);
        field_T03_E0038.Name = "field_T03_E0038";
        field_T03_E0038.Size = new Size(331, 24);
        field_T03_E0038.TabIndex = 4;
        field_T03_E0038.Tag = "T03-E0038";
        field_T03_E0038.ThreeState = true;
        // 
        // lbl_T03_E0039
        // 
        lbl_T03_E0039.AutoSize = true;
        lbl_T03_E0039.Dock = DockStyle.Fill;
        lbl_T03_E0039.Location = new Point(504, 208);
        lbl_T03_E0039.Margin = new Padding(6, 8, 6, 8);
        lbl_T03_E0039.Name = "lbl_T03_E0039";
        lbl_T03_E0039.Size = new Size(408, 23);
        lbl_T03_E0039.TabIndex = 5;
        lbl_T03_E0039.Text = "نموذج الطباعة";
        lbl_T03_E0039.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T03_E0039
        // 
        field_T03_E0039.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_T03_E0039.AccessibleName = "نموذج الطباعة";
        field_T03_E0039.Dock = DockStyle.Fill;
        field_T03_E0039.DropDownStyle = ComboBoxStyle.DropDownList;
        field_T03_E0039.Location = new Point(161, 208);
        field_T03_E0039.Margin = new Padding(6, 8, 6, 8);
        field_T03_E0039.Name = "field_T03_E0039";
        field_T03_E0039.Size = new Size(331, 25);
        field_T03_E0039.TabIndex = 5;
        field_T03_E0039.Tag = "T03-E0039";
        // 
        // lbl_T03_E0040
        // 
        lbl_T03_E0040.AutoSize = true;
        lbl_T03_E0040.Dock = DockStyle.Fill;
        lbl_T03_E0040.Location = new Point(504, 247);
        lbl_T03_E0040.Margin = new Padding(6, 8, 6, 8);
        lbl_T03_E0040.Name = "lbl_T03_E0040";
        lbl_T03_E0040.Size = new Size(408, 24);
        lbl_T03_E0040.TabIndex = 6;
        lbl_T03_E0040.Text = "الضريبة";
        lbl_T03_E0040.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T03_E0040
        // 
        field_T03_E0040.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_T03_E0040.AccessibleName = "الضريبة";
        field_T03_E0040.Checked = true;
        field_T03_E0040.CheckState = CheckState.Indeterminate;
        field_T03_E0040.Dock = DockStyle.Fill;
        field_T03_E0040.Location = new Point(161, 247);
        field_T03_E0040.Margin = new Padding(6, 8, 6, 8);
        field_T03_E0040.Name = "field_T03_E0040";
        field_T03_E0040.Size = new Size(331, 24);
        field_T03_E0040.TabIndex = 6;
        field_T03_E0040.Tag = "T03-E0040";
        field_T03_E0040.ThreeState = true;
        // 
        // lbl_T03_E0041
        // 
        lbl_T03_E0041.AutoSize = true;
        lbl_T03_E0041.Dock = DockStyle.Fill;
        lbl_T03_E0041.Location = new Point(504, 287);
        lbl_T03_E0041.Margin = new Padding(6, 8, 6, 8);
        lbl_T03_E0041.Name = "lbl_T03_E0041";
        lbl_T03_E0041.Size = new Size(408, 24);
        lbl_T03_E0041.TabIndex = 7;
        lbl_T03_E0041.Text = "مرتبط بطلب قيد";
        lbl_T03_E0041.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T03_E0041
        // 
        field_T03_E0041.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_T03_E0041.AccessibleName = "مرتبط بطلب قيد";
        field_T03_E0041.Checked = true;
        field_T03_E0041.CheckState = CheckState.Indeterminate;
        field_T03_E0041.Dock = DockStyle.Fill;
        field_T03_E0041.Location = new Point(161, 287);
        field_T03_E0041.Margin = new Padding(6, 8, 6, 8);
        field_T03_E0041.Name = "field_T03_E0041";
        field_T03_E0041.Size = new Size(331, 24);
        field_T03_E0041.TabIndex = 7;
        field_T03_E0041.Tag = "T03-E0041";
        field_T03_E0041.ThreeState = true;
        // 
        // lbl_R05_AT_0151
        // 
        lbl_R05_AT_0151.AutoSize = true;
        lbl_R05_AT_0151.Dock = DockStyle.Fill;
        lbl_R05_AT_0151.Location = new Point(504, 327);
        lbl_R05_AT_0151.Margin = new Padding(6, 8, 6, 8);
        lbl_R05_AT_0151.Name = "lbl_R05_AT_0151";
        lbl_R05_AT_0151.Size = new Size(408, 25);
        lbl_R05_AT_0151.TabIndex = 8;
        lbl_R05_AT_0151.Text = "اسم النوع";
        lbl_R05_AT_0151.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_R05_AT_0151
        // 
        field_R05_AT_0151.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_R05_AT_0151.AccessibleName = "اسم النوع";
        field_R05_AT_0151.Dock = DockStyle.Fill;
        field_R05_AT_0151.Location = new Point(161, 327);
        field_R05_AT_0151.Margin = new Padding(6, 8, 6, 8);
        field_R05_AT_0151.Name = "field_R05_AT_0151";
        field_R05_AT_0151.Size = new Size(331, 25);
        field_R05_AT_0151.TabIndex = 8;
        field_R05_AT_0151.Tag = "R05-AT-0151";
        // 
        // lbl_R05_AT_0152
        // 
        lbl_R05_AT_0152.AutoSize = true;
        lbl_R05_AT_0152.Dock = DockStyle.Fill;
        lbl_R05_AT_0152.Location = new Point(504, 368);
        lbl_R05_AT_0152.Margin = new Padding(6, 8, 6, 8);
        lbl_R05_AT_0152.Name = "lbl_R05_AT_0152";
        lbl_R05_AT_0152.Size = new Size(408, 25);
        lbl_R05_AT_0152.TabIndex = 9;
        lbl_R05_AT_0152.Text = "الاسم الأجنبي";
        lbl_R05_AT_0152.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_R05_AT_0152
        // 
        field_R05_AT_0152.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_R05_AT_0152.AccessibleName = "الاسم الأجنبي";
        field_R05_AT_0152.Dock = DockStyle.Fill;
        field_R05_AT_0152.Location = new Point(161, 368);
        field_R05_AT_0152.Margin = new Padding(6, 8, 6, 8);
        field_R05_AT_0152.Name = "field_R05_AT_0152";
        field_R05_AT_0152.RightToLeft = RightToLeft.No;
        field_R05_AT_0152.Size = new Size(331, 25);
        field_R05_AT_0152.TabIndex = 9;
        field_R05_AT_0152.Tag = "R05-AT-0152";
        // 
        // tpPermissions
        // 
        tpPermissions.AutoScroll = true;
        tpPermissions.Controls.Add(permissionsLayout);
        tpPermissions.Location = new Point(4, 26);
        tpPermissions.Name = "tpPermissions";
        tpPermissions.Size = new Size(1070, 573);
        tpPermissions.TabIndex = 1;
        tpPermissions.Tag = "T03-E0042";
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
        permissionsLayout.Size = new Size(1070, 573);
        permissionsLayout.TabIndex = 0;
        // 
        // lblPermissions
        // 
        lblPermissions.AutoSize = true;
        lblPermissions.Dock = DockStyle.Top;
        lblPermissions.Location = new Point(3, 0);
        lblPermissions.Name = "lblPermissions";
        lblPermissions.Size = new Size(1064, 19);
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
        permissionActions.Location = new Point(3, 22);
        permissionActions.Name = "permissionActions";
        permissionActions.RightToLeft = RightToLeft.Yes;
        permissionActions.Size = new Size(1064, 39);
        permissionActions.TabIndex = 1;
        // 
        // txtPermissionSearch
        // 
        txtPermissionSearch.AccessibleName = "بحث الصلاحيات المحلية";
        txtPermissionSearch.Location = new Point(821, 3);
        txtPermissionSearch.Name = "txtPermissionSearch";
        txtPermissionSearch.PlaceholderText = "بحث في المسودة المحلية";
        txtPermissionSearch.Size = new Size(240, 25);
        txtPermissionSearch.TabIndex = 0;
        txtPermissionSearch.TextChanged += PermissionSearchChanged;
        // 
        // btnAddPermission
        // 
        btnAddPermission.AutoSize = true;
        btnAddPermission.Location = new Point(712, 3);
        btnAddPermission.Name = "btnAddPermission";
        btnAddPermission.Size = new Size(103, 33);
        btnAddPermission.TabIndex = 1;
        btnAddPermission.Text = "إضافة سطر";
        btnAddPermission.Click += AddPermission_Click;
        // 
        // btnRemovePermission
        // 
        btnRemovePermission.AutoSize = true;
        btnRemovePermission.Location = new Point(545, 3);
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
        dgvPermissions.Location = new Point(3, 67);
        dgvPermissions.MultiSelect = false;
        dgvPermissions.Name = "dgvPermissions";
        dgvPermissions.RowHeadersVisible = false;
        dgvPermissions.RowHeadersWidth = 51;
        dgvPermissions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPermissions.Size = new Size(1064, 503);
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
        // UcOnyxSCREEN0083
        // 
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(mainLayout);
        Font = new Font("Segoe UI", 10F);
        Name = "UcOnyxSCREEN0083";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1100, 760);
        Tag = "ONYX.SCREEN-0083";
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
        designerCommandBar.Location = new Point(3, 33);
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
        btn_T03_E0043.AutoSize = false;
        btn_T03_E0043.Dock = DockStyle.None;
        btn_T03_E0043.MinimumSize = Size.Empty;
        btn_T03_E0043.Size = new Size(26, 24);
        btn_T03_E0043.Margin = new Padding(1);
        btn_T03_E0043.FlatStyle = FlatStyle.Flat;
        btn_T03_E0043.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btn_T03_E0043.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Save;
        btn_T03_E0043.Text = "";
        pnlToolbar.Controls.Add(btn_T03_E0043);
        designerCommandBar.SetCommandRole(btn_T03_E0043, TransportERP.Desktop.CoreUI.DesignerCommandRole.Save);
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
