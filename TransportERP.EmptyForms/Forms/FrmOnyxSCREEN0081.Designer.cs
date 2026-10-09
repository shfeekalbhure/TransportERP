namespace TransportERP.EmptyForms;
partial class UcOnyxSCREEN0081
{
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;

    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
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
    private Button standardCommandRefresh = null!;
    private Button standardCommandImport = null!;
    private Button standardCommandExport = null!;
    private Button standardCommandHelp = null!;

    private TableLayoutPanel mainLayout = null!;
    private TableLayoutPanel pnlHeader = null!;
    private Label lblTitle = null!;
    private FlowLayoutPanel pnlToolbar = null!;
    private Button btnNew = null!;
    private Button btnSave = null!;
    private Button btnClose = null!;
    private Label lblDataStatus = null!;
    private Panel pnlContent = null!;
    private TableLayoutPanel fieldsLayout = null!;
    private Label lblCode = null!;
    private TextBox txtCode = null!;
    private Label lblLocalName = null!;
    private TextBox txtLocalName = null!;
    private Label lblForeignName = null!;
    private TextBox txtForeignName = null!;
    private Label lblNotificationKind = null!;
    private ComboBox cmbNotificationKind = null!;
    private TabControl detailsTabs = null!;
    private TabPage tabMain = null!;
    private TabPage tabPermissions = null!;
    private Label lblPermissionsState = null!;
    private TableLayoutPanel permissionsLayout = null!;
    private FlowLayoutPanel permissionsToolbar = null!;
    private Button btnPermissionAdd = null!;
    private Button btnPermissionRemove = null!;
    private Button btnValidate = null!;
    private DataGridView dgvPermissions = null!;
    private ErrorProvider errors = null!;
    protected override void Dispose(bool disposing) { if (disposing) errors?.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
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
        standardCommandRefresh = new Button();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
        components = new System.ComponentModel.Container();
        mainLayout = new TableLayoutPanel();
        pnlHeader = new TableLayoutPanel();
        lblTitle = new Label();
        pnlToolbar = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnClose = new Button();
        btnValidate = new Button();
        lblDataStatus = new Label();
        pnlContent = new Panel();
        detailsTabs = new TabControl();
        tabMain = new TabPage();
        fieldsLayout = new TableLayoutPanel();
        lblCode = new Label();
        txtCode = new TextBox();
        lblLocalName = new Label();
        txtLocalName = new TextBox();
        lblForeignName = new Label();
        txtForeignName = new TextBox();
        lblNotificationKind = new Label();
        cmbNotificationKind = new ComboBox();
        tabPermissions = new TabPage();
        permissionsLayout = new TableLayoutPanel();
        permissionsToolbar = new FlowLayoutPanel();
        btnPermissionAdd = new Button();
        btnPermissionRemove = new Button();
        dgvPermissions = new DataGridView();
        dataGridViewComboBoxColumn1 = new DataGridViewComboBoxColumn();
        dataGridViewComboBoxColumn2 = new DataGridViewComboBoxColumn();
        dataGridViewComboBoxColumn3 = new DataGridViewComboBoxColumn();
        dataGridViewCheckBoxColumn1 = new DataGridViewCheckBoxColumn();
        lblPermissionsState = new Label();
        errors = new ErrorProvider(components);
        mainLayout.SuspendLayout();
        pnlHeader.SuspendLayout();
        pnlToolbar.SuspendLayout();
        pnlContent.SuspendLayout();
        detailsTabs.SuspendLayout();
        tabMain.SuspendLayout();
        fieldsLayout.SuspendLayout();
        tabPermissions.SuspendLayout();
        permissionsLayout.SuspendLayout();
        permissionsToolbar.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPermissions).BeginInit();
        ((System.ComponentModel.ISupportInitialize)errors).BeginInit();
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
        mainLayout.Size = new Size(1100, 720);
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
        pnlHeader.Size = new Size(1094, 119);
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
        lblTitle.Text = "أنواع الإشعارات";
        // 
        // pnlToolbar
        // 
        pnlToolbar.AutoSize = true;
        pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        pnlToolbar.Controls.Add(btnValidate);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Location = new Point(3, 40);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.RightToLeft = RightToLeft.Yes;
        pnlToolbar.Size = new Size(1088, 38);
        pnlToolbar.TabIndex = 1;
        // 
        // btnNew
        // 
        btnNew.Location = new Point(1014, 4);
        btnNew.Margin = new Padding(4);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(70, 30);
        btnNew.TabIndex = 0;
        btnNew.Text = "مسودة جديدة";
        // 
        // btnSave
        // 
        btnSave.Enabled = false;
        btnSave.Location = new Point(936, 4);
        btnSave.Margin = new Padding(4);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(70, 30);
        btnSave.TabIndex = 1;
        btnSave.Text = "حفظ";
        // 
        // btnClose
        // 
        btnClose.Location = new Point(858, 4);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(70, 30);
        btnClose.TabIndex = 2;
        btnClose.Text = "إغلاق";
        btnClose.Click += BtnClose_Click;
        // 
        // btnValidate
        // 
        btnValidate.Location = new Point(776, 3);
        btnValidate.Name = "btnValidate";
        btnValidate.Size = new Size(75, 23);
        btnValidate.TabIndex = 3;
        // 
        // lblDataStatus
        // 
        lblDataStatus.AutoSize = true;
        lblDataStatus.Dock = DockStyle.Top;
        lblDataStatus.Location = new Point(3, 125);
        lblDataStatus.Name = "lblDataStatus";
        lblDataStatus.Size = new Size(1094, 23);
        lblDataStatus.TabIndex = 1;
        lblDataStatus.Text = "وضع المعاينة — الحفظ وتحميل البيانات غير متاحين";
        // 
        // pnlContent
        // 
        pnlContent.AutoScroll = true;
        pnlContent.Controls.Add(detailsTabs);
        pnlContent.Dock = DockStyle.Fill;
        pnlContent.Location = new Point(3, 151);
        pnlContent.Name = "pnlContent";
        pnlContent.Padding = new Padding(8);
        pnlContent.Size = new Size(1094, 566);
        pnlContent.TabIndex = 2;
        // 
        // detailsTabs
        // 
        detailsTabs.Controls.Add(tabMain);
        detailsTabs.Controls.Add(tabPermissions);
        detailsTabs.Dock = DockStyle.Fill;
        detailsTabs.Location = new Point(8, 8);
        detailsTabs.Name = "detailsTabs";
        detailsTabs.RightToLeft = RightToLeft.Yes;
        detailsTabs.RightToLeftLayout = true;
        detailsTabs.SelectedIndex = 0;
        detailsTabs.Size = new Size(1078, 550);
        detailsTabs.TabIndex = 0;
        // 
        // tabMain
        // 
        tabMain.AutoScroll = true;
        tabMain.Controls.Add(fieldsLayout);
        tabMain.Location = new Point(4, 32);
        tabMain.Name = "tabMain";
        tabMain.Size = new Size(1070, 514);
        tabMain.TabIndex = 0;
        tabMain.Text = "البيانات الرئيسية";
        // 
        // fieldsLayout
        // 
        fieldsLayout.AutoSize = true;
        fieldsLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        fieldsLayout.ColumnCount = 2;
        fieldsLayout.ColumnStyles.Add(new ColumnStyle());
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        fieldsLayout.Controls.Add(lblCode, 0, 0);
        fieldsLayout.Controls.Add(txtCode, 1, 0);
        fieldsLayout.Controls.Add(lblLocalName, 0, 1);
        fieldsLayout.Controls.Add(txtLocalName, 1, 1);
        fieldsLayout.Controls.Add(lblForeignName, 0, 2);
        fieldsLayout.Controls.Add(txtForeignName, 1, 2);
        fieldsLayout.Controls.Add(lblNotificationKind, 0, 3);
        fieldsLayout.Controls.Add(cmbNotificationKind, 1, 3);
        fieldsLayout.Dock = DockStyle.Top;
        fieldsLayout.Location = new Point(0, 0);
        fieldsLayout.MinimumSize = new Size(520, 0);
        fieldsLayout.Name = "fieldsLayout";
        fieldsLayout.RowCount = 4;
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.Size = new Size(1070, 142);
        fieldsLayout.TabIndex = 0;
        // 
        // lblCode
        // 
        lblCode.AutoSize = true;
        lblCode.Dock = DockStyle.Fill;
        lblCode.Location = new Point(934, 0);
        lblCode.Name = "lblCode";
        lblCode.Size = new Size(133, 36);
        lblCode.TabIndex = 0;
        lblCode.Text = "رقم النوع";
        lblCode.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCode
        // 
        txtCode.AccessibleName = "رقم النوع";
        txtCode.BackColor = Color.LightYellow;
        txtCode.Dock = DockStyle.Fill;
        txtCode.Location = new Point(3, 3);
        txtCode.Name = "txtCode";
        txtCode.RightToLeft = RightToLeft.No;
        txtCode.Size = new Size(925, 30);
        txtCode.TabIndex = 0;
        txtCode.Tag = "UI_ALIAS:Code";
        // 
        // lblLocalName
        // 
        lblLocalName.AutoSize = true;
        lblLocalName.Dock = DockStyle.Fill;
        lblLocalName.Location = new Point(934, 36);
        lblLocalName.Name = "lblLocalName";
        lblLocalName.Size = new Size(133, 36);
        lblLocalName.TabIndex = 1;
        lblLocalName.Text = "اسم النوع المحلي";
        lblLocalName.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtLocalName
        // 
        txtLocalName.AccessibleName = "اسم النوع المحلي";
        txtLocalName.BackColor = Color.LightYellow;
        txtLocalName.Dock = DockStyle.Fill;
        txtLocalName.Location = new Point(3, 39);
        txtLocalName.Name = "txtLocalName";
        txtLocalName.Size = new Size(925, 30);
        txtLocalName.TabIndex = 1;
        txtLocalName.Tag = "UI_ALIAS:LocalName";
        // 
        // lblForeignName
        // 
        lblForeignName.AutoSize = true;
        lblForeignName.Dock = DockStyle.Fill;
        lblForeignName.Location = new Point(934, 72);
        lblForeignName.Name = "lblForeignName";
        lblForeignName.Size = new Size(133, 36);
        lblForeignName.TabIndex = 2;
        lblForeignName.Text = "اسم النوع الأجنبي";
        lblForeignName.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtForeignName
        // 
        txtForeignName.AccessibleName = "اسم النوع الأجنبي";
        txtForeignName.Dock = DockStyle.Fill;
        txtForeignName.Location = new Point(3, 75);
        txtForeignName.Name = "txtForeignName";
        txtForeignName.RightToLeft = RightToLeft.No;
        txtForeignName.Size = new Size(925, 30);
        txtForeignName.TabIndex = 2;
        txtForeignName.Tag = "UI_ALIAS:ForeignName";
        // 
        // lblNotificationKind
        // 
        lblNotificationKind.AutoSize = true;
        lblNotificationKind.Dock = DockStyle.Fill;
        lblNotificationKind.Location = new Point(934, 108);
        lblNotificationKind.Name = "lblNotificationKind";
        lblNotificationKind.Size = new Size(133, 34);
        lblNotificationKind.TabIndex = 3;
        lblNotificationKind.Text = "نوع الإشعار";
        lblNotificationKind.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cmbNotificationKind
        // 
        cmbNotificationKind.AccessibleDescription = "خيارات نوع الإشعار غير موثقة في المقتطف؛ لم يربط مزود القيم.";
        cmbNotificationKind.AccessibleName = "نوع الإشعار";
        cmbNotificationKind.BackColor = Color.LightYellow;
        cmbNotificationKind.Dock = DockStyle.Fill;
        cmbNotificationKind.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbNotificationKind.Location = new Point(3, 111);
        cmbNotificationKind.Name = "cmbNotificationKind";
        cmbNotificationKind.Size = new Size(925, 31);
        cmbNotificationKind.TabIndex = 3;
        cmbNotificationKind.Tag = "UI_ALIAS:NotificationKind";
        // 
        // tabPermissions
        // 
        tabPermissions.AutoScroll = true;
        tabPermissions.Controls.Add(permissionsLayout);
        tabPermissions.Location = new Point(4, 32);
        tabPermissions.Name = "tabPermissions";
        tabPermissions.Size = new Size(170, 0);
        tabPermissions.TabIndex = 1;
        tabPermissions.Text = "الصلاحيات";
        // 
        // permissionsLayout
        // 
        permissionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        permissionsLayout.Controls.Add(permissionsToolbar, 0, 0);
        permissionsLayout.Controls.Add(dgvPermissions, 0, 1);
        permissionsLayout.Location = new Point(0, 0);
        permissionsLayout.Name = "permissionsLayout";
        permissionsLayout.RowStyles.Add(new RowStyle());
        permissionsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        permissionsLayout.Size = new Size(200, 100);
        permissionsLayout.TabIndex = 0;
        // 
        // permissionsToolbar
        // 
        permissionsToolbar.Controls.Add(btnPermissionAdd);
        permissionsToolbar.Controls.Add(btnPermissionRemove);
        permissionsToolbar.Location = new Point(3, 3);
        permissionsToolbar.Name = "permissionsToolbar";
        permissionsToolbar.Size = new Size(194, 100);
        permissionsToolbar.TabIndex = 0;
        // 
        // btnPermissionAdd
        // 
        btnPermissionAdd.Location = new Point(116, 3);
        btnPermissionAdd.Name = "btnPermissionAdd";
        btnPermissionAdd.Size = new Size(75, 23);
        btnPermissionAdd.TabIndex = 0;
        // 
        // btnPermissionRemove
        // 
        btnPermissionRemove.Location = new Point(35, 3);
        btnPermissionRemove.Name = "btnPermissionRemove";
        btnPermissionRemove.Size = new Size(75, 23);
        btnPermissionRemove.TabIndex = 1;
        // 
        // dgvPermissions
        // 
        dgvPermissions.ColumnHeadersHeight = 29;
        dgvPermissions.Columns.AddRange(new DataGridViewColumn[] { dataGridViewComboBoxColumn1, dataGridViewComboBoxColumn2, dataGridViewComboBoxColumn3, dataGridViewCheckBoxColumn1 });
        dgvPermissions.Location = new Point(3, 109);
        dgvPermissions.Name = "dgvPermissions";
        dgvPermissions.RowHeadersWidth = 51;
        dgvPermissions.Size = new Size(194, 1);
        dgvPermissions.TabIndex = 1;
        // 
        // dataGridViewComboBoxColumn1
        // 
        dataGridViewComboBoxColumn1.MinimumWidth = 6;
        dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
        dataGridViewComboBoxColumn1.Width = 125;
        // 
        // dataGridViewComboBoxColumn2
        // 
        dataGridViewComboBoxColumn2.MinimumWidth = 6;
        dataGridViewComboBoxColumn2.Name = "dataGridViewComboBoxColumn2";
        dataGridViewComboBoxColumn2.Width = 125;
        // 
        // dataGridViewComboBoxColumn3
        // 
        dataGridViewComboBoxColumn3.MinimumWidth = 6;
        dataGridViewComboBoxColumn3.Name = "dataGridViewComboBoxColumn3";
        dataGridViewComboBoxColumn3.Width = 125;
        // 
        // dataGridViewCheckBoxColumn1
        // 
        dataGridViewCheckBoxColumn1.MinimumWidth = 6;
        dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
        dataGridViewCheckBoxColumn1.Width = 125;
        // 
        // lblPermissionsState
        // 
        lblPermissionsState.AutoSize = true;
        lblPermissionsState.Dock = DockStyle.Top;
        lblPermissionsState.Location = new Point(0, 0);
        lblPermissionsState.Name = "lblPermissionsState";
        lblPermissionsState.Size = new Size(100, 23);
        lblPermissionsState.TabIndex = 0;
        lblPermissionsState.Text = "الصلاحيات غير متاحة في المعاينة.";
        // 
        // errors
        // 
        errors.ContainerControl = this;
        // 
        // UcOnyxSCREEN0081
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(mainLayout);
        Font = new Font("Segoe UI", 10F);
        Name = "UcOnyxSCREEN0081";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1100, 720);
        mainLayout.ResumeLayout(false);
        mainLayout.PerformLayout();
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlToolbar.ResumeLayout(false);
        pnlContent.ResumeLayout(false);
        detailsTabs.ResumeLayout(false);
        tabMain.ResumeLayout(false);
        tabMain.PerformLayout();
        fieldsLayout.ResumeLayout(false);
        fieldsLayout.PerformLayout();
        tabPermissions.ResumeLayout(false);
        permissionsLayout.ResumeLayout(false);
        permissionsToolbar.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvPermissions).EndInit();
        ((System.ComponentModel.ISupportInitialize)errors).EndInit();
        ResumeLayout(false);
    
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(3, 40);
        designerCommandBar.Size = new Size(1088, 38);
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
        btnNew.AutoSize = false;
        btnNew.Dock = DockStyle.None;
        btnNew.MinimumSize = Size.Empty;
        btnNew.Size = new Size(26, 24);
        btnNew.Margin = new Padding(1);
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnNew.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Add;
        btnNew.Text = "";
        pnlToolbar.Controls.Add(btnNew);
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
        btnSave.AutoSize = false;
        btnSave.Dock = DockStyle.None;
        btnSave.MinimumSize = Size.Empty;
        btnSave.Size = new Size(26, 24);
        btnSave.Margin = new Padding(1);
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnSave.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Save;
        btnSave.Text = "";
        pnlToolbar.Controls.Add(btnSave);
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

    private DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
    private DataGridViewComboBoxColumn dataGridViewComboBoxColumn2;
    private DataGridViewComboBoxColumn dataGridViewComboBoxColumn3;
    private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
    private System.ComponentModel.IContainer components;
}
