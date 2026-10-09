#nullable enable

namespace TransportERP.Desktop.Forms.SystemSettings.DocumentControl;

partial class UcDocumentNumbering
{
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;

    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
    private Panel designerCloseHost = null!;
    private Button standardCommandAdd = null!;
    private Button standardCommandDelete = null!;
    private Button standardCommandCancel = null!;
    private Button standardCommandLast = null!;
    private Button standardCommandNext = null!;
    private Button standardCommandPrevious = null!;
    private Button standardCommandFirst = null!;
    private Button standardCommandSave = null!;
    private Button standardCommandPrint = null!;
    private Button standardCommandRefresh = null!;
    private Button standardCommandImport = null!;
    private Button standardCommandExport = null!;
    private Button standardCommandHelp = null!;

    private System.ComponentModel.IContainer? components;
    private DataGridViewTextBoxColumn colReservationNumber = null!;
    private DataGridViewTextBoxColumn colReservationValue = null!;
    private DataGridViewTextBoxColumn colReservationState = null!;
    private DataGridViewTextBoxColumn colReservationId = null!;
    private DataGridViewTextBoxColumn colReservationSequence = null!;
    private DataGridViewTextBoxColumn colPolicyCode = null!;
    private DataGridViewTextBoxColumn colPolicyName = null!;
    private DataGridViewTextBoxColumn colPolicyDocument = null!;
    private DataGridViewTextBoxColumn colPolicyPrefix = null!;
    private DataGridViewTextBoxColumn colPolicyLast = null!;
    private DataGridViewTextBoxColumn colPolicyReset = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        designerCloseHost = new Panel();
        standardCommandAdd = new Button();
        standardCommandDelete = new Button();
        standardCommandCancel = new Button();
        standardCommandLast = new Button();
        standardCommandNext = new Button();
        standardCommandPrevious = new Button();
        standardCommandFirst = new Button();
        standardCommandSave = new Button();
        standardCommandPrint = new Button();
        standardCommandRefresh = new Button();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
        lblDataStatus = new Label();
        tabNumberingV20 = new TabControl();
        tabGeneral = new TabPage();
        policyLayout = new TableLayoutPanel();
        policyFields = new TableLayoutPanel();
        lblCode = new Label();
        txtCode = new TextBox();
        lblArabicName = new Label();
        txtArabicName = new TextBox();
        lblEnglishName = new Label();
        txtEnglishName = new TextBox();
        lblStatus = new Label();
        txtStatus = new TextBox();
        lblDocumentType = new Label();
        cboDocumentType = new ComboBox();
        lblPrefix = new Label();
        txtPrefix = new TextBox();
        lblLastNumber = new Label();
        nudLastNumber = new NumericUpDown();
        lblResetType = new Label();
        cboResetType = new ComboBox();
        lblNotes = new Label();
        txtNotes = new TextBox();
        dgvDocumentSequences = new DataGridView();
        tabDocument = new TabPage();
        scopeFields = new TableLayoutPanel();
        lblScopeCompany = new Label();
        txtScopeCompany = new TextBox();
        lblScopeBranch = new Label();
        txtScopeBranch = new TextBox();
        lblScopeYear = new Label();
        txtScopeYear = new TextBox();
        lblScopeHelp = new Label();
        tabExceptions = new TabPage();
        exceptionLayout = new TableLayoutPanel();
        exceptionFields = new TableLayoutPanel();
        lblOverridePolicy = new Label();
        txtOverridePolicy = new TextBox();
        lblExpectedVersion = new Label();
        txtExpectedVersion = new TextBox();
        lblPermissionStatus = new Label();
        txtPermissionStatus = new TextBox();
        lblApprovalStatus = new Label();
        txtApprovalStatus = new TextBox();
        lblOverrideReason = new Label();
        txtOverrideReason = new TextBox();
        lblExceptionsInfo = new Label();
        grpReservationDetails = new GroupBox();
        reservationFields = new TableLayoutPanel();
        lblReservationNumber = new Label();
        txtReservationNumber = new TextBox();
        lblReservationState = new Label();
        txtReservationState = new TextBox();
        lblReservationId = new Label();
        txtReservationId = new TextBox();
        tabAllocationHistory = new TabPage();
        historyLayout = new TableLayoutPanel();
        lblHistoryInfo = new Label();
        dgvAllocationHistory = new DataGridView();
        flpActions = new FlowLayoutPanel();
        btnView = new Button();
        btnEdit = new Button();
        btnReserve = new Button();
        btnCommit = new Button();
        btnCancelReservation = new Button();
        btnOverride = new Button();
        btnClose = new Button();
        tlpFilters = new TableLayoutPanel();
        lblCompany = new Label();
        cboCompany = new ComboBox();
        lblBranch = new Label();
        cboBranch = new ComboBox();
        lblFiscalYear = new Label();
        cboFiscalYear = new ComboBox();
        lblTitle = new Label();
        layoutRoot = new TableLayoutPanel();
        tlpAuditInfo = new TableLayoutPanel();
        lblPrintCount = new Label();
        lblLastPrintedAt = new Label();
        lblEditCount = new Label();
        lblModifiedAt = new Label();
        lblModifiedBy = new Label();
        lblCreatedAt = new Label();
        lblCreatedBy = new Label();
        tabNumberingV20.SuspendLayout();
        tabGeneral.SuspendLayout();
        policyLayout.SuspendLayout();
        policyFields.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudLastNumber).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvDocumentSequences).BeginInit();
        tabDocument.SuspendLayout();
        scopeFields.SuspendLayout();
        tabExceptions.SuspendLayout();
        exceptionLayout.SuspendLayout();
        exceptionFields.SuspendLayout();
        grpReservationDetails.SuspendLayout();
        reservationFields.SuspendLayout();
        tabAllocationHistory.SuspendLayout();
        historyLayout.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAllocationHistory).BeginInit();
        flpActions.SuspendLayout();
        tlpFilters.SuspendLayout();
        layoutRoot.SuspendLayout();
        tlpAuditInfo.SuspendLayout();
        SuspendLayout();
        // 
        // lblDataStatus
        // 
        lblDataStatus.BackColor = Color.FromArgb(248, 250, 252);
        lblDataStatus.Dock = DockStyle.Top;
        lblDataStatus.Font = new Font("Segoe UI", 9F);
        lblDataStatus.ForeColor = Color.FromArgb(16, 24, 40);
        lblDataStatus.Location = new Point(11, 860);
        lblDataStatus.Name = "lblDataStatus";
        lblDataStatus.Size = new Size(1178, 23);
        lblDataStatus.TabIndex = 4;
        lblDataStatus.Text = "لم تُحمّل بيانات الترقيم بعد؛ إجراءات الحفظ والحجز والاعتماد غير متاحة.";
        lblDataStatus.TextAlign = ContentAlignment.MiddleRight;
        // 
        // tabNumberingV20
        // 
        tabNumberingV20.Controls.Add(tabGeneral);
        tabNumberingV20.Controls.Add(tabDocument);
        tabNumberingV20.Controls.Add(tabExceptions);
        tabNumberingV20.Controls.Add(tabAllocationHistory);
        tabNumberingV20.Dock = DockStyle.Fill;
        tabNumberingV20.Font = new Font("Segoe UI", 9F);
        tabNumberingV20.Location = new Point(8, 168);
        tabNumberingV20.Margin = new Padding(0);
        tabNumberingV20.Name = "tabNumberingV20";
        tabNumberingV20.RightToLeft = RightToLeft.Yes;
        tabNumberingV20.RightToLeftLayout = true;
        tabNumberingV20.SelectedIndex = 0;
        tabNumberingV20.Size = new Size(1184, 692);
        tabNumberingV20.TabIndex = 3;
        // 
        // tabGeneral
        // 
        tabGeneral.AutoScroll = true;
        tabGeneral.BackColor = Color.LightCyan;
        tabGeneral.Controls.Add(policyLayout);
        tabGeneral.Location = new Point(4, 29);
        tabGeneral.Name = "tabGeneral";
        tabGeneral.Padding = new Padding(4);
        tabGeneral.Size = new Size(1176, 659);
        tabGeneral.TabIndex = 0;
        tabGeneral.Text = "سياسات الترقيم";
        // 
        // policyLayout
        // 
        policyLayout.BackColor = Color.LightCyan;
        policyLayout.ColumnCount = 1;
        policyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        policyLayout.Controls.Add(policyFields, 0, 0);
        policyLayout.Controls.Add(dgvDocumentSequences, 0, 1);
        policyLayout.Dock = DockStyle.Fill;
        policyLayout.Location = new Point(8, 8);
        policyLayout.Margin = new Padding(0);
        policyLayout.Name = "policyLayout";
        policyLayout.Padding = new Padding(4);
        policyLayout.RowCount = 2;
        policyLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 256F));
        policyLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        policyLayout.Size = new Size(1160, 643);
        policyLayout.TabIndex = 0;
        // 
        // policyFields
        // 
        policyFields.BackColor = Color.LightCyan;
        policyFields.ColumnCount = 4;
        policyFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 165F));
        policyFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        policyFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 165F));
        policyFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        policyFields.Controls.Add(lblCode, 0, 0);
        policyFields.Controls.Add(txtCode, 1, 0);
        policyFields.Controls.Add(lblArabicName, 2, 0);
        policyFields.Controls.Add(txtArabicName, 3, 0);
        policyFields.Controls.Add(lblEnglishName, 0, 1);
        policyFields.Controls.Add(txtEnglishName, 1, 1);
        policyFields.Controls.Add(lblStatus, 2, 1);
        policyFields.Controls.Add(txtStatus, 3, 1);
        policyFields.Controls.Add(lblDocumentType, 0, 2);
        policyFields.Controls.Add(cboDocumentType, 1, 2);
        policyFields.Controls.Add(lblPrefix, 2, 2);
        policyFields.Controls.Add(txtPrefix, 3, 2);
        policyFields.Controls.Add(lblLastNumber, 0, 3);
        policyFields.Controls.Add(nudLastNumber, 1, 3);
        policyFields.Controls.Add(lblResetType, 2, 3);
        policyFields.Controls.Add(cboResetType, 3, 3);
        policyFields.Controls.Add(lblNotes, 0, 4);
        policyFields.Controls.Add(txtNotes, 1, 4);
        policyFields.Dock = DockStyle.Fill;
        policyFields.Location = new Point(8, 8);
        policyFields.Margin = new Padding(0);
        policyFields.Name = "policyFields";
        policyFields.Padding = new Padding(4);
        policyFields.RowCount = 5;
        policyFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        policyFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        policyFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        policyFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        policyFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
        policyFields.Size = new Size(1144, 256);
        policyFields.TabIndex = 0;
        // 
        // lblCode
        // 
        lblCode.Dock = DockStyle.Fill;
        lblCode.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblCode.ForeColor = Color.FromArgb(16, 24, 40);
        lblCode.Location = new Point(974, 11);
        lblCode.Margin = new Padding(3);
        lblCode.Name = "lblCode";
        lblCode.Size = new Size(159, 34);
        lblCode.TabIndex = 0;
        lblCode.Text = "الرمز";
        lblCode.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCode
        // 
        txtCode.BackColor = SystemColors.Control;
        txtCode.Dock = DockStyle.Fill;
        txtCode.Font = new Font("Segoe UI", 11F);
        txtCode.ForeColor = Color.FromArgb(16, 24, 40);
        txtCode.Location = new Point(575, 11);
        txtCode.Name = "txtCode";
        txtCode.ReadOnly = true;
        txtCode.Size = new Size(393, 32);
        txtCode.TabIndex = 1;
        txtCode.TabStop = false;
        // 
        // lblArabicName
        // 
        lblArabicName.Dock = DockStyle.Fill;
        lblArabicName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblArabicName.ForeColor = Color.FromArgb(16, 24, 40);
        lblArabicName.Location = new Point(410, 11);
        lblArabicName.Margin = new Padding(3);
        lblArabicName.Name = "lblArabicName";
        lblArabicName.Size = new Size(159, 34);
        lblArabicName.TabIndex = 2;
        lblArabicName.Text = "الاسم العربي";
        lblArabicName.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtArabicName
        // 
        txtArabicName.BackColor = SystemColors.Control;
        txtArabicName.Dock = DockStyle.Fill;
        txtArabicName.Font = new Font("Segoe UI", 11F);
        txtArabicName.ForeColor = Color.FromArgb(16, 24, 40);
        txtArabicName.Location = new Point(11, 11);
        txtArabicName.Name = "txtArabicName";
        txtArabicName.ReadOnly = true;
        txtArabicName.Size = new Size(393, 32);
        txtArabicName.TabIndex = 3;
        txtArabicName.TabStop = false;
        // 
        // lblEnglishName
        // 
        lblEnglishName.Dock = DockStyle.Fill;
        lblEnglishName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblEnglishName.ForeColor = Color.FromArgb(16, 24, 40);
        lblEnglishName.Location = new Point(974, 51);
        lblEnglishName.Margin = new Padding(3);
        lblEnglishName.Name = "lblEnglishName";
        lblEnglishName.Size = new Size(159, 34);
        lblEnglishName.TabIndex = 4;
        lblEnglishName.Text = "الاسم الإنجليزي";
        lblEnglishName.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtEnglishName
        // 
        txtEnglishName.BackColor = SystemColors.Control;
        txtEnglishName.Dock = DockStyle.Fill;
        txtEnglishName.Font = new Font("Segoe UI", 11F);
        txtEnglishName.ForeColor = Color.FromArgb(16, 24, 40);
        txtEnglishName.Location = new Point(575, 51);
        txtEnglishName.Name = "txtEnglishName";
        txtEnglishName.ReadOnly = true;
        txtEnglishName.Size = new Size(393, 32);
        txtEnglishName.TabIndex = 5;
        txtEnglishName.TabStop = false;
        // 
        // lblStatus
        // 
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblStatus.ForeColor = Color.FromArgb(16, 24, 40);
        lblStatus.Location = new Point(410, 51);
        lblStatus.Margin = new Padding(3);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(159, 34);
        lblStatus.TabIndex = 6;
        lblStatus.Text = "الحالة";
        lblStatus.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtStatus
        // 
        txtStatus.BackColor = SystemColors.Control;
        txtStatus.Dock = DockStyle.Fill;
        txtStatus.Font = new Font("Segoe UI", 11F);
        txtStatus.ForeColor = Color.FromArgb(16, 24, 40);
        txtStatus.Location = new Point(11, 51);
        txtStatus.Name = "txtStatus";
        txtStatus.ReadOnly = true;
        txtStatus.Size = new Size(393, 32);
        txtStatus.TabIndex = 7;
        txtStatus.TabStop = false;
        // 
        // lblDocumentType
        // 
        lblDocumentType.Dock = DockStyle.Fill;
        lblDocumentType.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblDocumentType.ForeColor = Color.FromArgb(180, 35, 24);
        lblDocumentType.Location = new Point(974, 91);
        lblDocumentType.Margin = new Padding(3);
        lblDocumentType.Name = "lblDocumentType";
        lblDocumentType.Size = new Size(159, 34);
        lblDocumentType.TabIndex = 8;
        lblDocumentType.Text = "نوع المستند *";
        lblDocumentType.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cboDocumentType
        // 
        cboDocumentType.BackColor = Color.FromArgb(255, 249, 219);
        cboDocumentType.Dock = DockStyle.Fill;
        cboDocumentType.DropDownStyle = ComboBoxStyle.DropDownList;
        cboDocumentType.Font = new Font("Segoe UI", 11F);
        cboDocumentType.ForeColor = Color.FromArgb(16, 24, 40);
        cboDocumentType.Location = new Point(575, 91);
        cboDocumentType.Name = "cboDocumentType";
        cboDocumentType.Size = new Size(393, 33);
        cboDocumentType.TabIndex = 9;
        // 
        // lblPrefix
        // 
        lblPrefix.Dock = DockStyle.Fill;
        lblPrefix.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblPrefix.ForeColor = Color.FromArgb(16, 24, 40);
        lblPrefix.Location = new Point(410, 91);
        lblPrefix.Margin = new Padding(3);
        lblPrefix.Name = "lblPrefix";
        lblPrefix.Size = new Size(159, 34);
        lblPrefix.TabIndex = 10;
        lblPrefix.Text = "البادئة";
        lblPrefix.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtPrefix
        // 
        txtPrefix.BackColor = Color.White;
        txtPrefix.Dock = DockStyle.Fill;
        txtPrefix.Font = new Font("Segoe UI", 11F);
        txtPrefix.ForeColor = Color.FromArgb(16, 24, 40);
        txtPrefix.Location = new Point(11, 91);
        txtPrefix.Name = "txtPrefix";
        txtPrefix.Size = new Size(393, 32);
        txtPrefix.TabIndex = 11;
        // 
        // lblLastNumber
        // 
        lblLastNumber.Dock = DockStyle.Fill;
        lblLastNumber.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblLastNumber.ForeColor = Color.FromArgb(16, 24, 40);
        lblLastNumber.Location = new Point(974, 131);
        lblLastNumber.Margin = new Padding(3);
        lblLastNumber.Name = "lblLastNumber";
        lblLastNumber.Size = new Size(159, 34);
        lblLastNumber.TabIndex = 12;
        lblLastNumber.Text = "آخر رقم";
        lblLastNumber.TextAlign = ContentAlignment.MiddleRight;
        // 
        // nudLastNumber
        // 
        nudLastNumber.BackColor = SystemColors.Control;
        nudLastNumber.Dock = DockStyle.Fill;
        nudLastNumber.Enabled = false;
        nudLastNumber.Font = new Font("Segoe UI", 11F);
        nudLastNumber.ForeColor = Color.FromArgb(16, 24, 40);
        nudLastNumber.Location = new Point(575, 131);
        nudLastNumber.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
        nudLastNumber.Name = "nudLastNumber";
        nudLastNumber.ReadOnly = true;
        nudLastNumber.Size = new Size(393, 32);
        nudLastNumber.TabIndex = 13;
        nudLastNumber.TabStop = false;
        // 
        // lblResetType
        // 
        lblResetType.Dock = DockStyle.Fill;
        lblResetType.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblResetType.ForeColor = Color.FromArgb(16, 24, 40);
        lblResetType.Location = new Point(410, 131);
        lblResetType.Margin = new Padding(3);
        lblResetType.Name = "lblResetType";
        lblResetType.Size = new Size(159, 34);
        lblResetType.TabIndex = 14;
        lblResetType.Text = "سياسة إعادة الترقيم";
        lblResetType.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cboResetType
        // 
        cboResetType.BackColor = Color.White;
        cboResetType.Dock = DockStyle.Fill;
        cboResetType.DropDownStyle = ComboBoxStyle.DropDownList;
        cboResetType.Font = new Font("Segoe UI", 11F);
        cboResetType.ForeColor = Color.FromArgb(16, 24, 40);
        cboResetType.Location = new Point(11, 131);
        cboResetType.Name = "cboResetType";
        cboResetType.Size = new Size(393, 33);
        cboResetType.TabIndex = 15;
        // 
        // lblNotes
        // 
        lblNotes.Dock = DockStyle.Fill;
        lblNotes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblNotes.ForeColor = Color.FromArgb(16, 24, 40);
        lblNotes.Location = new Point(974, 171);
        lblNotes.Margin = new Padding(3);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(159, 74);
        lblNotes.TabIndex = 16;
        lblNotes.Text = "ملاحظات السياسة";
        lblNotes.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtNotes
        // 
        txtNotes.BackColor = Color.White;
        policyFields.SetColumnSpan(txtNotes, 3);
        txtNotes.Dock = DockStyle.Fill;
        txtNotes.Font = new Font("Segoe UI", 11F);
        txtNotes.ForeColor = Color.FromArgb(16, 24, 40);
        txtNotes.Location = new Point(11, 171);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(957, 74);
        txtNotes.TabIndex = 17;
        // 
        // dgvDocumentSequences
        // 
        dgvDocumentSequences.AllowUserToAddRows = false;
        dgvDocumentSequences.AllowUserToDeleteRows = false;
        dgvDocumentSequences.BackgroundColor = Color.White;
        dgvDocumentSequences.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvDocumentSequences.Dock = DockStyle.Fill;
        dgvDocumentSequences.Font = new Font("Segoe UI", 10F);
        dgvDocumentSequences.Location = new Point(11, 267);
        dgvDocumentSequences.MultiSelect = false;
        dgvDocumentSequences.Name = "dgvDocumentSequences";
        dgvDocumentSequences.ReadOnly = true;
        dgvDocumentSequences.RowHeadersVisible = false;
        dgvDocumentSequences.RowHeadersWidth = 51;
        dgvDocumentSequences.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvDocumentSequences.Size = new Size(1138, 365);
        dgvDocumentSequences.TabIndex = 1;
        // 
        // tabDocument
        // 
        tabDocument.AutoScroll = true;
        tabDocument.BackColor = Color.LightCyan;
        tabDocument.Controls.Add(scopeFields);
        tabDocument.Location = new Point(4, 29);
        tabDocument.Name = "tabDocument";
        tabDocument.Padding = new Padding(4);
        tabDocument.Size = new Size(1176, 659);
        tabDocument.TabIndex = 1;
        tabDocument.Text = "نطاقات الترقيم";
        // 
        // scopeFields
        // 
        scopeFields.BackColor = Color.LightCyan;
        scopeFields.ColumnCount = 2;
        scopeFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 165F));
        scopeFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        scopeFields.Controls.Add(lblScopeCompany, 0, 0);
        scopeFields.Controls.Add(txtScopeCompany, 1, 0);
        scopeFields.Controls.Add(lblScopeBranch, 0, 1);
        scopeFields.Controls.Add(txtScopeBranch, 1, 1);
        scopeFields.Controls.Add(lblScopeYear, 0, 2);
        scopeFields.Controls.Add(txtScopeYear, 1, 2);
        scopeFields.Controls.Add(lblScopeHelp, 0, 3);
        scopeFields.Dock = DockStyle.Top;
        scopeFields.Location = new Point(8, 8);
        scopeFields.Margin = new Padding(0);
        scopeFields.Name = "scopeFields";
        scopeFields.Padding = new Padding(4);
        scopeFields.RowCount = 4;
        scopeFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        scopeFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        scopeFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        scopeFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
        scopeFields.Size = new Size(1160, 216);
        scopeFields.TabIndex = 0;
        // 
        // lblScopeCompany
        // 
        lblScopeCompany.Dock = DockStyle.Fill;
        lblScopeCompany.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblScopeCompany.ForeColor = Color.FromArgb(16, 24, 40);
        lblScopeCompany.Location = new Point(990, 11);
        lblScopeCompany.Margin = new Padding(3);
        lblScopeCompany.Name = "lblScopeCompany";
        lblScopeCompany.Size = new Size(159, 34);
        lblScopeCompany.TabIndex = 0;
        lblScopeCompany.Text = "الشركة";
        lblScopeCompany.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtScopeCompany
        // 
        txtScopeCompany.BackColor = SystemColors.Control;
        txtScopeCompany.Dock = DockStyle.Left;
        txtScopeCompany.Font = new Font("Segoe UI", 11F);
        txtScopeCompany.ForeColor = Color.FromArgb(16, 24, 40);
        txtScopeCompany.Location = new Point(450, 11);
        txtScopeCompany.Name = "txtScopeCompany";
        txtScopeCompany.ReadOnly = true;
        txtScopeCompany.Size = new Size(534, 32);
        txtScopeCompany.TabIndex = 1;
        txtScopeCompany.TabStop = false;
        // 
        // lblScopeBranch
        // 
        lblScopeBranch.Dock = DockStyle.Fill;
        lblScopeBranch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblScopeBranch.ForeColor = Color.FromArgb(16, 24, 40);
        lblScopeBranch.Location = new Point(990, 51);
        lblScopeBranch.Margin = new Padding(3);
        lblScopeBranch.Name = "lblScopeBranch";
        lblScopeBranch.Size = new Size(159, 34);
        lblScopeBranch.TabIndex = 2;
        lblScopeBranch.Text = "الفرع";
        lblScopeBranch.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtScopeBranch
        // 
        txtScopeBranch.BackColor = SystemColors.Control;
        txtScopeBranch.Dock = DockStyle.Left;
        txtScopeBranch.Font = new Font("Segoe UI", 11F);
        txtScopeBranch.ForeColor = Color.FromArgb(16, 24, 40);
        txtScopeBranch.Location = new Point(450, 51);
        txtScopeBranch.Name = "txtScopeBranch";
        txtScopeBranch.ReadOnly = true;
        txtScopeBranch.Size = new Size(534, 32);
        txtScopeBranch.TabIndex = 3;
        txtScopeBranch.TabStop = false;
        // 
        // lblScopeYear
        // 
        lblScopeYear.Dock = DockStyle.Fill;
        lblScopeYear.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblScopeYear.ForeColor = Color.FromArgb(16, 24, 40);
        lblScopeYear.Location = new Point(990, 91);
        lblScopeYear.Margin = new Padding(3);
        lblScopeYear.Name = "lblScopeYear";
        lblScopeYear.Size = new Size(159, 34);
        lblScopeYear.TabIndex = 4;
        lblScopeYear.Text = "السنة المالية";
        lblScopeYear.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtScopeYear
        // 
        txtScopeYear.BackColor = SystemColors.Control;
        txtScopeYear.Dock = DockStyle.Left;
        txtScopeYear.Font = new Font("Segoe UI", 11F);
        txtScopeYear.ForeColor = Color.FromArgb(16, 24, 40);
        txtScopeYear.Location = new Point(450, 91);
        txtScopeYear.Name = "txtScopeYear";
        txtScopeYear.ReadOnly = true;
        txtScopeYear.Size = new Size(534, 32);
        txtScopeYear.TabIndex = 5;
        txtScopeYear.TabStop = false;
        // 
        // lblScopeHelp
        // 
        scopeFields.SetColumnSpan(lblScopeHelp, 2);
        lblScopeHelp.Dock = DockStyle.Fill;
        lblScopeHelp.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblScopeHelp.ForeColor = Color.FromArgb(16, 24, 40);
        lblScopeHelp.Location = new Point(11, 131);
        lblScopeHelp.Margin = new Padding(3);
        lblScopeHelp.Name = "lblScopeHelp";
        lblScopeHelp.Size = new Size(1138, 74);
        lblScopeHelp.TabIndex = 6;
        lblScopeHelp.Text = "النطاق المعروض يتبع اختيار الشركة والفرع والسنة المالية أعلى الشاشة.";
        lblScopeHelp.TextAlign = ContentAlignment.MiddleRight;
        // 
        // tabExceptions
        // 
        tabExceptions.AutoScroll = true;
        tabExceptions.BackColor = Color.LightCyan;
        tabExceptions.Controls.Add(exceptionLayout);
        tabExceptions.Location = new Point(4, 29);
        tabExceptions.Name = "tabExceptions";
        tabExceptions.Padding = new Padding(4);
        tabExceptions.Size = new Size(1176, 659);
        tabExceptions.TabIndex = 2;
        tabExceptions.Text = "الاستثناءات والاعتماد";
        // 
        // exceptionLayout
        // 
        exceptionLayout.AutoSize = true;
        exceptionLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        exceptionLayout.BackColor = Color.LightCyan;
        exceptionLayout.ColumnCount = 1;
        exceptionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        exceptionLayout.Controls.Add(exceptionFields, 0, 0);
        exceptionLayout.Controls.Add(lblExceptionsInfo, 0, 1);
        exceptionLayout.Controls.Add(grpReservationDetails, 0, 2);
        exceptionLayout.Dock = DockStyle.Top;
        exceptionLayout.Location = new Point(8, 8);
        exceptionLayout.Name = "exceptionLayout";
        exceptionLayout.Padding = new Padding(4);
        exceptionLayout.RowCount = 3;
        exceptionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 264F));
        exceptionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        exceptionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 180F));
        exceptionLayout.Size = new Size(1160, 516);
        exceptionLayout.TabIndex = 0;
        // 
        // exceptionFields
        // 
        exceptionFields.ColumnCount = 2;
        exceptionFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));
        exceptionFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        exceptionFields.Controls.Add(lblOverridePolicy, 0, 0);
        exceptionFields.Controls.Add(txtOverridePolicy, 1, 0);
        exceptionFields.Controls.Add(lblExpectedVersion, 0, 1);
        exceptionFields.Controls.Add(txtExpectedVersion, 1, 1);
        exceptionFields.Controls.Add(lblPermissionStatus, 0, 2);
        exceptionFields.Controls.Add(txtPermissionStatus, 1, 2);
        exceptionFields.Controls.Add(lblApprovalStatus, 0, 3);
        exceptionFields.Controls.Add(txtApprovalStatus, 1, 3);
        exceptionFields.Controls.Add(lblOverrideReason, 0, 4);
        exceptionFields.Controls.Add(txtOverrideReason, 1, 4);
        exceptionFields.Dock = DockStyle.Fill;
        exceptionFields.Location = new Point(11, 11);
        exceptionFields.Name = "exceptionFields";
        exceptionFields.RowCount = 5;
        exceptionFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        exceptionFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        exceptionFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        exceptionFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        exceptionFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 96F));
        exceptionFields.Size = new Size(1138, 258);
        exceptionFields.TabIndex = 0;
        // 
        // lblOverridePolicy
        // 
        lblOverridePolicy.Dock = DockStyle.Fill;
        lblOverridePolicy.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblOverridePolicy.ForeColor = Color.FromArgb(16, 24, 40);
        lblOverridePolicy.Location = new Point(951, 3);
        lblOverridePolicy.Margin = new Padding(3);
        lblOverridePolicy.Name = "lblOverridePolicy";
        lblOverridePolicy.Size = new Size(184, 34);
        lblOverridePolicy.TabIndex = 0;
        lblOverridePolicy.Text = "سياسة الترقيم المحددة";
        lblOverridePolicy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtOverridePolicy
        // 
        txtOverridePolicy.BackColor = SystemColors.Control;
        txtOverridePolicy.Dock = DockStyle.Left;
        txtOverridePolicy.Font = new Font("Segoe UI", 11F);
        txtOverridePolicy.Location = new Point(269, 3);
        txtOverridePolicy.Name = "txtOverridePolicy";
        txtOverridePolicy.ReadOnly = true;
        txtOverridePolicy.Size = new Size(676, 32);
        txtOverridePolicy.TabIndex = 1;
        txtOverridePolicy.TabStop = false;
        // 
        // lblExpectedVersion
        // 
        lblExpectedVersion.Dock = DockStyle.Fill;
        lblExpectedVersion.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblExpectedVersion.ForeColor = Color.FromArgb(16, 24, 40);
        lblExpectedVersion.Location = new Point(951, 43);
        lblExpectedVersion.Margin = new Padding(3);
        lblExpectedVersion.Name = "lblExpectedVersion";
        lblExpectedVersion.Size = new Size(184, 34);
        lblExpectedVersion.TabIndex = 2;
        lblExpectedVersion.Text = "إصدار السياسة الحالي";
        lblExpectedVersion.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtExpectedVersion
        // 
        txtExpectedVersion.BackColor = SystemColors.Control;
        txtExpectedVersion.Dock = DockStyle.Left;
        txtExpectedVersion.Font = new Font("Segoe UI", 11F);
        txtExpectedVersion.Location = new Point(269, 43);
        txtExpectedVersion.Name = "txtExpectedVersion";
        txtExpectedVersion.ReadOnly = true;
        txtExpectedVersion.Size = new Size(676, 32);
        txtExpectedVersion.TabIndex = 3;
        txtExpectedVersion.TabStop = false;
        // 
        // lblPermissionStatus
        // 
        lblPermissionStatus.Dock = DockStyle.Fill;
        lblPermissionStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblPermissionStatus.ForeColor = Color.FromArgb(16, 24, 40);
        lblPermissionStatus.Location = new Point(951, 83);
        lblPermissionStatus.Margin = new Padding(3);
        lblPermissionStatus.Name = "lblPermissionStatus";
        lblPermissionStatus.Size = new Size(184, 34);
        lblPermissionStatus.TabIndex = 4;
        lblPermissionStatus.Text = "صلاحية التجاوز";
        lblPermissionStatus.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtPermissionStatus
        // 
        txtPermissionStatus.BackColor = SystemColors.Control;
        txtPermissionStatus.Dock = DockStyle.Left;
        txtPermissionStatus.Font = new Font("Segoe UI", 11F);
        txtPermissionStatus.Location = new Point(269, 83);
        txtPermissionStatus.Name = "txtPermissionStatus";
        txtPermissionStatus.ReadOnly = true;
        txtPermissionStatus.Size = new Size(676, 32);
        txtPermissionStatus.TabIndex = 5;
        txtPermissionStatus.TabStop = false;
        // 
        // lblApprovalStatus
        // 
        lblApprovalStatus.Dock = DockStyle.Fill;
        lblApprovalStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblApprovalStatus.ForeColor = Color.FromArgb(16, 24, 40);
        lblApprovalStatus.Location = new Point(951, 123);
        lblApprovalStatus.Margin = new Padding(3);
        lblApprovalStatus.Name = "lblApprovalStatus";
        lblApprovalStatus.Size = new Size(184, 34);
        lblApprovalStatus.TabIndex = 6;
        lblApprovalStatus.Text = "حالة الاعتماد";
        lblApprovalStatus.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtApprovalStatus
        // 
        txtApprovalStatus.BackColor = SystemColors.Control;
        txtApprovalStatus.Dock = DockStyle.Left;
        txtApprovalStatus.Font = new Font("Segoe UI", 11F);
        txtApprovalStatus.Location = new Point(269, 123);
        txtApprovalStatus.Name = "txtApprovalStatus";
        txtApprovalStatus.ReadOnly = true;
        txtApprovalStatus.Size = new Size(676, 32);
        txtApprovalStatus.TabIndex = 7;
        txtApprovalStatus.TabStop = false;
        // 
        // lblOverrideReason
        // 
        lblOverrideReason.Dock = DockStyle.Fill;
        lblOverrideReason.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblOverrideReason.ForeColor = Color.FromArgb(180, 35, 24);
        lblOverrideReason.Location = new Point(951, 163);
        lblOverrideReason.Margin = new Padding(3);
        lblOverrideReason.Name = "lblOverrideReason";
        lblOverrideReason.Size = new Size(184, 92);
        lblOverrideReason.TabIndex = 8;
        lblOverrideReason.Text = "سبب التجاوز / إعادة الضبط *";
        lblOverrideReason.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtOverrideReason
        // 
        txtOverrideReason.BackColor = Color.FromArgb(255, 249, 219);
        txtOverrideReason.Dock = DockStyle.Left;
        txtOverrideReason.Font = new Font("Segoe UI", 11F);
        txtOverrideReason.Location = new Point(269, 163);
        txtOverrideReason.Multiline = true;
        txtOverrideReason.Name = "txtOverrideReason";
        txtOverrideReason.ScrollBars = ScrollBars.Vertical;
        txtOverrideReason.Size = new Size(676, 92);
        txtOverrideReason.TabIndex = 9;
        // 
        // lblExceptionsInfo
        // 
        lblExceptionsInfo.Dock = DockStyle.Fill;
        lblExceptionsInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblExceptionsInfo.ForeColor = Color.FromArgb(16, 24, 40);
        lblExceptionsInfo.Location = new Point(11, 275);
        lblExceptionsInfo.Margin = new Padding(3);
        lblExceptionsInfo.Name = "lblExceptionsInfo";
        lblExceptionsInfo.Size = new Size(1138, 50);
        lblExceptionsInfo.TabIndex = 1;
        lblExceptionsInfo.Text = "التجاوز وإعادة الضبط يتطلبان الصلاحية والسبب، والاعتماد عندما تشترطه السياسة. الأرقام الملغاة لا يعاد استخدامها.";
        lblExceptionsInfo.TextAlign = ContentAlignment.MiddleRight;
        // 
        // grpReservationDetails
        // 
        grpReservationDetails.Controls.Add(reservationFields);
        grpReservationDetails.Dock = DockStyle.Top;
        grpReservationDetails.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        grpReservationDetails.ForeColor = Color.FromArgb(0, 53, 128);
        grpReservationDetails.Location = new Point(11, 331);
        grpReservationDetails.Name = "grpReservationDetails";
        grpReservationDetails.Padding = new Padding(4);
        grpReservationDetails.Size = new Size(1138, 174);
        grpReservationDetails.TabIndex = 2;
        grpReservationDetails.TabStop = false;
        grpReservationDetails.Text = "تفاصيل الحجز المحدد";
        // 
        // reservationFields
        // 
        reservationFields.ColumnCount = 2;
        reservationFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));
        reservationFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        reservationFields.Controls.Add(lblReservationNumber, 0, 0);
        reservationFields.Controls.Add(txtReservationNumber, 1, 0);
        reservationFields.Controls.Add(lblReservationState, 0, 1);
        reservationFields.Controls.Add(txtReservationState, 1, 1);
        reservationFields.Controls.Add(lblReservationId, 0, 2);
        reservationFields.Controls.Add(txtReservationId, 1, 2);
        reservationFields.Dock = DockStyle.Fill;
        reservationFields.Location = new Point(8, 31);
        reservationFields.Name = "reservationFields";
        reservationFields.Padding = new Padding(3);
        reservationFields.RowCount = 3;
        reservationFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        reservationFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        reservationFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        reservationFields.Size = new Size(1122, 135);
        reservationFields.TabIndex = 0;
        // 
        // lblReservationNumber
        // 
        lblReservationNumber.Dock = DockStyle.Fill;
        lblReservationNumber.Location = new Point(932, 6);
        lblReservationNumber.Margin = new Padding(3);
        lblReservationNumber.Name = "lblReservationNumber";
        lblReservationNumber.Size = new Size(184, 34);
        lblReservationNumber.TabIndex = 0;
        lblReservationNumber.Text = "الرقم المخصص";
        lblReservationNumber.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtReservationNumber
        // 
        txtReservationNumber.BackColor = SystemColors.Control;
        txtReservationNumber.Dock = DockStyle.Left;
        txtReservationNumber.Font = new Font("Segoe UI", 11F);
        txtReservationNumber.Location = new Point(253, 6);
        txtReservationNumber.Name = "txtReservationNumber";
        txtReservationNumber.ReadOnly = true;
        txtReservationNumber.Size = new Size(673, 32);
        txtReservationNumber.TabIndex = 1;
        txtReservationNumber.TabStop = false;
        // 
        // lblReservationState
        // 
        lblReservationState.Dock = DockStyle.Fill;
        lblReservationState.Location = new Point(932, 46);
        lblReservationState.Margin = new Padding(3);
        lblReservationState.Name = "lblReservationState";
        lblReservationState.Size = new Size(184, 34);
        lblReservationState.TabIndex = 2;
        lblReservationState.Text = "حالة الحجز";
        lblReservationState.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtReservationState
        // 
        txtReservationState.BackColor = SystemColors.Control;
        txtReservationState.Dock = DockStyle.Left;
        txtReservationState.Font = new Font("Segoe UI", 11F);
        txtReservationState.Location = new Point(253, 46);
        txtReservationState.Name = "txtReservationState";
        txtReservationState.ReadOnly = true;
        txtReservationState.Size = new Size(673, 32);
        txtReservationState.TabIndex = 3;
        txtReservationState.TabStop = false;
        // 
        // lblReservationId
        // 
        lblReservationId.Dock = DockStyle.Fill;
        lblReservationId.Location = new Point(932, 86);
        lblReservationId.Margin = new Padding(3);
        lblReservationId.Name = "lblReservationId";
        lblReservationId.Size = new Size(184, 43);
        lblReservationId.TabIndex = 4;
        lblReservationId.Text = "معرّف الحجز";
        lblReservationId.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtReservationId
        // 
        txtReservationId.BackColor = SystemColors.Control;
        txtReservationId.Dock = DockStyle.Left;
        txtReservationId.Font = new Font("Segoe UI", 11F);
        txtReservationId.Location = new Point(253, 86);
        txtReservationId.Name = "txtReservationId";
        txtReservationId.ReadOnly = true;
        txtReservationId.Size = new Size(673, 32);
        txtReservationId.TabIndex = 5;
        txtReservationId.TabStop = false;
        // 
        // tabAllocationHistory
        // 
        tabAllocationHistory.AutoScroll = true;
        tabAllocationHistory.BackColor = Color.LightCyan;
        tabAllocationHistory.Controls.Add(historyLayout);
        tabAllocationHistory.Location = new Point(4, 29);
        tabAllocationHistory.Name = "tabAllocationHistory";
        tabAllocationHistory.Padding = new Padding(4);
        tabAllocationHistory.Size = new Size(1176, 659);
        tabAllocationHistory.TabIndex = 3;
        tabAllocationHistory.Text = "سجل التخصيص";
        // 
        // historyLayout
        // 
        historyLayout.BackColor = Color.LightCyan;
        historyLayout.ColumnCount = 1;
        historyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        historyLayout.Controls.Add(lblHistoryInfo, 0, 0);
        historyLayout.Controls.Add(dgvAllocationHistory, 0, 1);
        historyLayout.Dock = DockStyle.Fill;
        historyLayout.Location = new Point(8, 8);
        historyLayout.Margin = new Padding(0);
        historyLayout.Name = "historyLayout";
        historyLayout.Padding = new Padding(4);
        historyLayout.RowCount = 2;
        historyLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        historyLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        historyLayout.Size = new Size(1160, 643);
        historyLayout.TabIndex = 0;
        // 
        // lblHistoryInfo
        // 
        lblHistoryInfo.Dock = DockStyle.Fill;
        lblHistoryInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblHistoryInfo.ForeColor = Color.FromArgb(16, 24, 40);
        lblHistoryInfo.Location = new Point(11, 11);
        lblHistoryInfo.Margin = new Padding(3);
        lblHistoryInfo.Name = "lblHistoryInfo";
        lblHistoryInfo.Size = new Size(1138, 42);
        lblHistoryInfo.TabIndex = 0;
        lblHistoryInfo.Text = "سجل الأرقام المحجوزة والمثبتة والملغاة — للقراءة فقط";
        lblHistoryInfo.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dgvAllocationHistory
        // 
        dgvAllocationHistory.AllowUserToAddRows = false;
        dgvAllocationHistory.AllowUserToDeleteRows = false;
        dgvAllocationHistory.BackgroundColor = Color.White;
        dgvAllocationHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvAllocationHistory.Dock = DockStyle.Fill;
        dgvAllocationHistory.Font = new Font("Segoe UI", 10F);
        dgvAllocationHistory.Location = new Point(11, 59);
        dgvAllocationHistory.MultiSelect = false;
        dgvAllocationHistory.Name = "dgvAllocationHistory";
        dgvAllocationHistory.ReadOnly = true;
        dgvAllocationHistory.RowHeadersVisible = false;
        dgvAllocationHistory.RowHeadersWidth = 51;
        dgvAllocationHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAllocationHistory.Size = new Size(1138, 573);
        dgvAllocationHistory.TabIndex = 1;
        // 
        // flpActions
        // 
        flpActions.AutoSize = true;
        flpActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flpActions.BackColor = Color.FromArgb(224, 224, 224);

        flpActions.Controls.Add(btnReserve);
        flpActions.Controls.Add(btnCommit);
        flpActions.Controls.Add(btnCancelReservation);
        flpActions.Controls.Add(btnOverride);

        flpActions.Dock = DockStyle.Fill;
        flpActions.Location = new Point(8, 120);
        flpActions.Margin = new Padding(0);
        flpActions.MinimumSize = new Size(0, 48);
        flpActions.Name = "flpActions";
        flpActions.Padding = new Padding(5);
        flpActions.Size = new Size(1184, 48);
        flpActions.TabIndex = 2;
        // 
        // btnView
        // 
        btnView.BackColor = Color.FromArgb(224, 224, 224);
        btnView.Enabled = false;
        btnView.FlatStyle = FlatStyle.Flat;
        btnView.Font = new Font("Microsoft Sans Serif", 10F);
        btnView.Location = new Point(1100, 9);
        btnView.Margin = new Padding(3);
        btnView.Name = "btnView";
        btnView.Size = new Size(70, 30);
        btnView.TabIndex = 0;
        btnView.Text = "عرض";
        btnView.UseVisualStyleBackColor = false;
        // 
        // btnEdit
        // 
        btnEdit.BackColor = Color.FromArgb(224, 224, 224);
        btnEdit.Enabled = false;
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.Font = new Font("Microsoft Sans Serif", 10F);
        btnEdit.Location = new Point(1022, 9);
        btnEdit.Margin = new Padding(3);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(70, 30);
        btnEdit.TabIndex = 1;
        btnEdit.Text = "تعديل";
        btnEdit.UseVisualStyleBackColor = false;
        // 
        // btnReserve
        // 
        btnReserve.BackColor = Color.FromArgb(224, 224, 224);
        btnReserve.Enabled = false;
        btnReserve.FlatStyle = FlatStyle.Flat;
        btnReserve.Font = new Font("Microsoft Sans Serif", 10F);
        btnReserve.Location = new Point(944, 9);
        btnReserve.Margin = new Padding(3);
        btnReserve.Name = "btnReserve";
        btnReserve.Size = new Size(70, 30);
        btnReserve.TabIndex = 2;
        btnReserve.Text = "حجز";
        btnReserve.UseVisualStyleBackColor = false;
        // 
        // btnCommit
        // 
        btnCommit.BackColor = Color.FromArgb(224, 224, 224);
        btnCommit.Enabled = false;
        btnCommit.FlatStyle = FlatStyle.Flat;
        btnCommit.Font = new Font("Microsoft Sans Serif", 10F);
        btnCommit.Location = new Point(831, 9);
        btnCommit.Margin = new Padding(3);
        btnCommit.Name = "btnCommit";
        btnCommit.Size = new Size(105, 30);
        btnCommit.TabIndex = 3;
        btnCommit.Text = "تثبيت الحجز";
        btnCommit.UseVisualStyleBackColor = false;
        // 
        // btnCancelReservation
        // 
        btnCancelReservation.BackColor = Color.FromArgb(224, 224, 224);
        btnCancelReservation.Enabled = false;
        btnCancelReservation.FlatStyle = FlatStyle.Flat;
        btnCancelReservation.Font = new Font("Microsoft Sans Serif", 10F);
        btnCancelReservation.Location = new Point(718, 9);
        btnCancelReservation.Margin = new Padding(3);
        btnCancelReservation.Name = "btnCancelReservation";
        btnCancelReservation.Size = new Size(105, 30);
        btnCancelReservation.TabIndex = 4;
        btnCancelReservation.Text = "إلغاء الحجز";
        btnCancelReservation.UseVisualStyleBackColor = false;
        // 
        // btnOverride
        // 
        btnOverride.BackColor = Color.FromArgb(224, 224, 224);
        btnOverride.Enabled = false;
        btnOverride.FlatStyle = FlatStyle.Flat;
        btnOverride.Font = new Font("Microsoft Sans Serif", 10F);
        btnOverride.Location = new Point(555, 9);
        btnOverride.Margin = new Padding(3);
        btnOverride.Name = "btnOverride";
        btnOverride.Size = new Size(155, 30);
        btnOverride.TabIndex = 5;
        btnOverride.Text = "تجاوز / إعادة ضبط";
        btnOverride.UseVisualStyleBackColor = false;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(224, 224, 224);
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Microsoft Sans Serif", 10F);
        btnClose.Location = new Point(477, 9);
        btnClose.Margin = new Padding(3);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(70, 30);
        btnClose.TabIndex = 6;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        // 
        // tlpFilters
        // 
        tlpFilters.BackColor = Color.LightCyan;
        tlpFilters.ColumnCount = 6;
        tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 105F));
        tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
        tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
        tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
        tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125F));
        tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
        tlpFilters.Controls.Add(lblCompany, 0, 0);
        tlpFilters.Controls.Add(cboCompany, 1, 0);
        tlpFilters.Controls.Add(lblBranch, 2, 0);
        tlpFilters.Controls.Add(cboBranch, 3, 0);
        tlpFilters.Controls.Add(lblFiscalYear, 4, 0);
        tlpFilters.Controls.Add(cboFiscalYear, 5, 0);
        tlpFilters.Dock = DockStyle.Fill;
        tlpFilters.Location = new Point(8, 56);
        tlpFilters.Margin = new Padding(0);
        tlpFilters.Name = "tlpFilters";
        tlpFilters.Padding = new Padding(4);
        tlpFilters.RowCount = 1;
        tlpFilters.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpFilters.Size = new Size(1184, 64);
        tlpFilters.TabIndex = 1;
        // 
        // lblCompany
        // 
        lblCompany.Dock = DockStyle.Fill;
        lblCompany.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblCompany.ForeColor = Color.FromArgb(180, 35, 24);
        lblCompany.Location = new Point(1074, 11);
        lblCompany.Margin = new Padding(3);
        lblCompany.Name = "lblCompany";
        lblCompany.Size = new Size(99, 42);
        lblCompany.TabIndex = 0;
        lblCompany.Text = "الشركة *";
        lblCompany.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cboCompany
        // 
        cboCompany.BackColor = Color.FromArgb(255, 249, 219);
        cboCompany.Dock = DockStyle.Fill;
        cboCompany.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCompany.Font = new Font("Segoe UI", 11F);
        cboCompany.ForeColor = Color.FromArgb(16, 24, 40);
        cboCompany.Location = new Point(786, 11);
        cboCompany.Name = "cboCompany";
        cboCompany.Size = new Size(282, 33);
        cboCompany.TabIndex = 1;
        // 
        // lblBranch
        // 
        lblBranch.Dock = DockStyle.Fill;
        lblBranch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblBranch.ForeColor = Color.FromArgb(180, 35, 24);
        lblBranch.Location = new Point(696, 11);
        lblBranch.Margin = new Padding(3);
        lblBranch.Name = "lblBranch";
        lblBranch.Size = new Size(84, 42);
        lblBranch.TabIndex = 2;
        lblBranch.Text = "الفرع *";
        lblBranch.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cboBranch
        // 
        cboBranch.BackColor = Color.FromArgb(255, 249, 219);
        cboBranch.Dock = DockStyle.Fill;
        cboBranch.DropDownStyle = ComboBoxStyle.DropDownList;
        cboBranch.Font = new Font("Segoe UI", 11F);
        cboBranch.ForeColor = Color.FromArgb(16, 24, 40);
        cboBranch.Location = new Point(417, 11);
        cboBranch.Name = "cboBranch";
        cboBranch.Size = new Size(273, 33);
        cboBranch.TabIndex = 3;
        // 
        // lblFiscalYear
        // 
        lblFiscalYear.Dock = DockStyle.Fill;
        lblFiscalYear.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblFiscalYear.ForeColor = Color.FromArgb(180, 35, 24);
        lblFiscalYear.Location = new Point(292, 11);
        lblFiscalYear.Margin = new Padding(3);
        lblFiscalYear.Name = "lblFiscalYear";
        lblFiscalYear.Size = new Size(119, 42);
        lblFiscalYear.TabIndex = 4;
        lblFiscalYear.Text = "السنة المالية *";
        lblFiscalYear.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cboFiscalYear
        // 
        cboFiscalYear.BackColor = Color.FromArgb(255, 249, 219);
        cboFiscalYear.Dock = DockStyle.Fill;
        cboFiscalYear.DropDownStyle = ComboBoxStyle.DropDownList;
        cboFiscalYear.Font = new Font("Segoe UI", 11F);
        cboFiscalYear.ForeColor = Color.FromArgb(16, 24, 40);
        cboFiscalYear.Location = new Point(11, 11);
        cboFiscalYear.Name = "cboFiscalYear";
        cboFiscalYear.Size = new Size(275, 33);
        cboFiscalYear.TabIndex = 5;
        // 
        // lblTitle
        // 
        lblTitle.BackColor = Color.FromArgb(192, 192, 255);
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.Font = new Font("Segoe UI", 16F);
        lblTitle.ForeColor = Color.FromArgb(16, 24, 40);
        lblTitle.Location = new Point(11, 11);
        lblTitle.Margin = new Padding(3);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(1178, 42);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "إعدادات الترقيم";
        lblTitle.TextAlign = ContentAlignment.MiddleRight;
        // 
        // layoutRoot
        // 
        layoutRoot.BackColor = Color.FromArgb(248, 250, 252);
        layoutRoot.ColumnCount = 1;
        layoutRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layoutRoot.Controls.Add(lblTitle, 0, 0);
        layoutRoot.Controls.Add(tlpFilters, 0, 1);
        layoutRoot.Controls.Add(designerCommandBar, 0, 2);
        layoutRoot.Controls.Add(tabNumberingV20, 0, 3);
        layoutRoot.Controls.Add(lblDataStatus, 0, 4);
        layoutRoot.Dock = DockStyle.Fill;
        layoutRoot.Location = new Point(0, 0);
        layoutRoot.Margin = new Padding(0);
        layoutRoot.Name = "layoutRoot";
        layoutRoot.Padding = new Padding(4);
        layoutRoot.RowCount = 5;
        layoutRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        layoutRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
        layoutRoot.RowStyles.Add(new RowStyle());
        layoutRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layoutRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        layoutRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        layoutRoot.Size = new Size(1200, 900);
        layoutRoot.TabIndex = 0;
        // 
        // tlpAuditInfo
        // 
        tlpAuditInfo.AccessibleName = "البوليصه";
        tlpAuditInfo.BackColor = Color.FromArgb(248, 250, 252);
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
        tlpAuditInfo.Location = new Point(0, 852);
        tlpAuditInfo.Margin = new Padding(0);
        tlpAuditInfo.Name = "tlpAuditInfo";
        tlpAuditInfo.Padding = new Padding(4);
        tlpAuditInfo.RightToLeft = RightToLeft.Yes;
        tlpAuditInfo.RowCount = 1;
        tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpAuditInfo.Size = new Size(1200, 48);
        tlpAuditInfo.TabIndex = 4;
        // 
        // lblPrintCount
        // 
        lblPrintCount.AutoEllipsis = true;
        lblPrintCount.BackColor = Color.FromArgb(192, 255, 255);
        lblPrintCount.Dock = DockStyle.Fill;
        lblPrintCount.Font = new Font("Segoe UI", 10F);
        lblPrintCount.ForeColor = Color.FromArgb(16, 24, 40);
        lblPrintCount.Location = new Point(11, 11);
        lblPrintCount.Margin = new Padding(3);
        lblPrintCount.Name = "lblPrintCount";
        lblPrintCount.RightToLeft = RightToLeft.Yes;
        lblPrintCount.Size = new Size(139, 26);
        lblPrintCount.TabIndex = 8;
        lblPrintCount.Text = "عدد مرات الطباعة: —";
        lblPrintCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblLastPrintedAt
        // 
        lblLastPrintedAt.AutoEllipsis = true;
        lblLastPrintedAt.BackColor = Color.FromArgb(192, 255, 255);
        lblLastPrintedAt.Dock = DockStyle.Fill;
        lblLastPrintedAt.Font = new Font("Segoe UI", 10F);
        lblLastPrintedAt.ForeColor = Color.FromArgb(16, 24, 40);
        lblLastPrintedAt.Location = new Point(156, 11);
        lblLastPrintedAt.Margin = new Padding(3);
        lblLastPrintedAt.Name = "lblLastPrintedAt";
        lblLastPrintedAt.RightToLeft = RightToLeft.Yes;
        lblLastPrintedAt.Size = new Size(183, 26);
        lblLastPrintedAt.TabIndex = 5;
        lblLastPrintedAt.Text = "تاريخ اخر طباعة: —";
        lblLastPrintedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblEditCount
        // 
        lblEditCount.AutoEllipsis = true;
        lblEditCount.BackColor = Color.FromArgb(192, 255, 255);
        lblEditCount.Dock = DockStyle.Fill;
        lblEditCount.Font = new Font("Segoe UI", 10F);
        lblEditCount.ForeColor = Color.FromArgb(16, 24, 40);
        lblEditCount.Location = new Point(345, 11);
        lblEditCount.Margin = new Padding(3);
        lblEditCount.Name = "lblEditCount";
        lblEditCount.RightToLeft = RightToLeft.Yes;
        lblEditCount.Size = new Size(136, 26);
        lblEditCount.TabIndex = 4;
        lblEditCount.Text = "عدد التعديلات: —";
        lblEditCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblModifiedAt
        // 
        lblModifiedAt.AutoEllipsis = true;
        lblModifiedAt.BackColor = Color.FromArgb(192, 255, 255);
        lblModifiedAt.Dock = DockStyle.Fill;
        lblModifiedAt.Font = new Font("Segoe UI", 10F);
        lblModifiedAt.ForeColor = Color.FromArgb(16, 24, 40);
        lblModifiedAt.Location = new Point(487, 11);
        lblModifiedAt.Margin = new Padding(3);
        lblModifiedAt.Name = "lblModifiedAt";
        lblModifiedAt.RightToLeft = RightToLeft.Yes;
        lblModifiedAt.Size = new Size(183, 26);
        lblModifiedAt.TabIndex = 3;
        lblModifiedAt.Text = "تاريخ التعديل: —";
        lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblModifiedBy
        // 
        lblModifiedBy.AutoEllipsis = true;
        lblModifiedBy.BackColor = Color.FromArgb(192, 255, 255);
        lblModifiedBy.Dock = DockStyle.Fill;
        lblModifiedBy.Font = new Font("Segoe UI", 10F);
        lblModifiedBy.ForeColor = Color.FromArgb(16, 24, 40);
        lblModifiedBy.Location = new Point(676, 11);
        lblModifiedBy.Margin = new Padding(3);
        lblModifiedBy.Name = "lblModifiedBy";
        lblModifiedBy.RightToLeft = RightToLeft.Yes;
        lblModifiedBy.Size = new Size(159, 26);
        lblModifiedBy.TabIndex = 2;
        lblModifiedBy.Text = "عدل بواسطة: —";
        lblModifiedBy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCreatedAt
        // 
        lblCreatedAt.AutoEllipsis = true;
        lblCreatedAt.BackColor = Color.FromArgb(192, 255, 255);
        lblCreatedAt.Dock = DockStyle.Fill;
        lblCreatedAt.Font = new Font("Segoe UI", 10F);
        lblCreatedAt.ForeColor = Color.FromArgb(16, 24, 40);
        lblCreatedAt.Location = new Point(841, 11);
        lblCreatedAt.Margin = new Padding(3);
        lblCreatedAt.Name = "lblCreatedAt";
        lblCreatedAt.RightToLeft = RightToLeft.Yes;
        lblCreatedAt.Size = new Size(183, 26);
        lblCreatedAt.TabIndex = 1;
        lblCreatedAt.Text = "تاريخ الانشاء: —";
        lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCreatedBy
        // 
        lblCreatedBy.AutoEllipsis = true;
        lblCreatedBy.BackColor = Color.FromArgb(192, 255, 255);
        lblCreatedBy.Dock = DockStyle.Fill;
        lblCreatedBy.Font = new Font("Segoe UI", 10F);
        lblCreatedBy.ForeColor = Color.FromArgb(16, 24, 40);
        lblCreatedBy.Location = new Point(1030, 11);
        lblCreatedBy.Margin = new Padding(3);
        lblCreatedBy.Name = "lblCreatedBy";
        lblCreatedBy.RightToLeft = RightToLeft.Yes;
        lblCreatedBy.Size = new Size(159, 26);
        lblCreatedBy.TabIndex = 0;
        lblCreatedBy.Text = "أنشأ بواسطة: —";
        lblCreatedBy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // UcDocumentNumbering
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(248, 250, 252);
        Controls.Add(tlpAuditInfo);
        Controls.Add(layoutRoot);
        Font = new Font("Segoe UI", 10F);
        ForeColor = Color.FromArgb(16, 24, 40);
        Margin = new Padding(0);
        MinimumSize = new Size(1000, 700);
        Name = "UcDocumentNumbering";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1200, 900);
        tabNumberingV20.ResumeLayout(false);
        tabGeneral.ResumeLayout(false);
        policyLayout.ResumeLayout(false);
        policyFields.ResumeLayout(false);
        policyFields.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudLastNumber).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvDocumentSequences).EndInit();
        tabDocument.ResumeLayout(false);
        scopeFields.ResumeLayout(false);
        scopeFields.PerformLayout();
        tabExceptions.ResumeLayout(false);
        tabExceptions.PerformLayout();
        exceptionLayout.ResumeLayout(false);
        exceptionFields.ResumeLayout(false);
        exceptionFields.PerformLayout();
        grpReservationDetails.ResumeLayout(false);
        reservationFields.ResumeLayout(false);
        reservationFields.PerformLayout();
        tabAllocationHistory.ResumeLayout(false);
        historyLayout.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvAllocationHistory).EndInit();
        flpActions.ResumeLayout(false);
        tlpFilters.ResumeLayout(false);
        layoutRoot.ResumeLayout(false);
        layoutRoot.PerformLayout();
        tlpAuditInfo.ResumeLayout(false);
        ResumeLayout(false);
    
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Fill;
        designerCommandBar.Location = new Point(8, 120);
        designerCommandBar.Size = new Size(1184, 48);
        designerCommandBar.Margin = new Padding(0);
        designerCommandBar.MinimumSize = new Size(0, 48);
        designerCommandBar.TabIndex = 2;
        designerCommandBar.Controls.Add(flpActions);
        flpActions.Dock = DockStyle.Fill;
        flpActions.Margin = Padding.Empty;
        designerCommandBar.Name = "designerCommandBar";
        designerCommandBar.BackColor = Color.FromArgb(239, 239, 239);
        designerCommandBar.BorderStyle = BorderStyle.FixedSingle;
        designerCommandBar.Controls.Add(designerCloseHost);
        designerCloseHost.Dock = DockStyle.Left;
        designerCloseHost.Width = 28;
        designerCloseHost.Name = "designerCloseHost";
        flpActions.Name = "flpActions";
        flpActions.Dock = DockStyle.Fill;
        flpActions.AutoSize = false;
        flpActions.WrapContents = false;
        flpActions.FlowDirection = FlowDirection.RightToLeft;
        flpActions.RightToLeft = RightToLeft.No;
        flpActions.Padding = new Padding(2);
        designerCommandBar.MinimumSize = new Size(0, 30);
        designerCommandBar.Height = 44;
        standardCommandAdd.Name = "standardCommandAdd";
        standardCommandAdd.Enabled = false;
        standardCommandAdd.Visible = true;
        standardCommandAdd.AccessibleName = "إضافة";
        standardCommandDelete.Name = "standardCommandDelete";
        standardCommandDelete.Enabled = false;
        standardCommandDelete.Visible = true;
        standardCommandDelete.AccessibleName = "حذف";
        standardCommandCancel.Name = "standardCommandCancel";
        standardCommandCancel.Enabled = false;
        standardCommandCancel.Visible = true;
        standardCommandCancel.AccessibleName = "تراجع";
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
        standardCommandSave.Name = "standardCommandSave";
        standardCommandSave.Enabled = false;
        standardCommandSave.Visible = true;
        standardCommandSave.AccessibleName = "حفظ";
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
        flpActions.Controls.Add(standardCommandAdd);
        designerCommandBar.SetCommandRole(standardCommandAdd, TransportERP.Desktop.CoreUI.DesignerCommandRole.Add);
        btnEdit.AutoSize = false;
        btnEdit.Dock = DockStyle.None;
        btnEdit.MinimumSize = Size.Empty;
        btnEdit.Size = new Size(26, 24);
        btnEdit.Margin = new Padding(1);
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnEdit.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Edit;
        btnEdit.Text = "";
        flpActions.Controls.Add(btnEdit);
        designerCommandBar.SetCommandRole(btnEdit, TransportERP.Desktop.CoreUI.DesignerCommandRole.Edit);
        standardCommandDelete.AutoSize = false;
        standardCommandDelete.Dock = DockStyle.None;
        standardCommandDelete.MinimumSize = Size.Empty;
        standardCommandDelete.Size = new Size(26, 24);
        standardCommandDelete.Margin = new Padding(1);
        standardCommandDelete.FlatStyle = FlatStyle.Flat;
        standardCommandDelete.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandDelete.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Delete;
        standardCommandDelete.Text = "";
        flpActions.Controls.Add(standardCommandDelete);
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
        flpActions.Controls.Add(standardCommandCancel);
        designerCommandBar.SetCommandRole(standardCommandCancel, TransportERP.Desktop.CoreUI.DesignerCommandRole.Cancel);
        btnView.AutoSize = false;
        btnView.Dock = DockStyle.None;
        btnView.MinimumSize = Size.Empty;
        btnView.Size = new Size(26, 24);
        btnView.Margin = new Padding(1);
        btnView.FlatStyle = FlatStyle.Flat;
        btnView.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnView.Image = TransportERP.Desktop.CoreUI.CommandBarImages.View;
        btnView.Text = "";
        flpActions.Controls.Add(btnView);
        designerCommandBar.SetCommandRole(btnView, TransportERP.Desktop.CoreUI.DesignerCommandRole.View);
        standardCommandLast.AutoSize = false;
        standardCommandLast.Dock = DockStyle.None;
        standardCommandLast.MinimumSize = Size.Empty;
        standardCommandLast.Size = new Size(26, 24);
        standardCommandLast.Margin = new Padding(1);
        standardCommandLast.FlatStyle = FlatStyle.Flat;
        standardCommandLast.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandLast.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Last;
        standardCommandLast.Text = "";
        flpActions.Controls.Add(standardCommandLast);
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
        flpActions.Controls.Add(standardCommandNext);
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
        flpActions.Controls.Add(standardCommandPrevious);
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
        flpActions.Controls.Add(standardCommandFirst);
        designerCommandBar.SetCommandRole(standardCommandFirst, TransportERP.Desktop.CoreUI.DesignerCommandRole.First);
        standardCommandSave.AutoSize = false;
        standardCommandSave.Dock = DockStyle.None;
        standardCommandSave.MinimumSize = Size.Empty;
        standardCommandSave.Size = new Size(26, 24);
        standardCommandSave.Margin = new Padding(1);
        standardCommandSave.FlatStyle = FlatStyle.Flat;
        standardCommandSave.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandSave.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Save;
        standardCommandSave.Text = "";
        flpActions.Controls.Add(standardCommandSave);
        designerCommandBar.SetCommandRole(standardCommandSave, TransportERP.Desktop.CoreUI.DesignerCommandRole.Save);
        standardCommandPrint.AutoSize = false;
        standardCommandPrint.Dock = DockStyle.None;
        standardCommandPrint.MinimumSize = Size.Empty;
        standardCommandPrint.Size = new Size(26, 24);
        standardCommandPrint.Margin = new Padding(1);
        standardCommandPrint.FlatStyle = FlatStyle.Flat;
        standardCommandPrint.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandPrint.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Print;
        standardCommandPrint.Text = "";
        flpActions.Controls.Add(standardCommandPrint);
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
        flpActions.Controls.Add(standardCommandRefresh);
        designerCommandBar.SetCommandRole(standardCommandRefresh, TransportERP.Desktop.CoreUI.DesignerCommandRole.Refresh);
        standardCommandImport.AutoSize = false;
        standardCommandImport.Dock = DockStyle.None;
        standardCommandImport.MinimumSize = Size.Empty;
        standardCommandImport.Size = new Size(26, 24);
        standardCommandImport.Margin = new Padding(1);
        standardCommandImport.FlatStyle = FlatStyle.Flat;
        standardCommandImport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandImport.Text = "استيراد";
        flpActions.Controls.Add(standardCommandImport);
        designerCommandBar.SetCommandRole(standardCommandImport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Import);
        standardCommandExport.AutoSize = false;
        standardCommandExport.Dock = DockStyle.None;
        standardCommandExport.MinimumSize = Size.Empty;
        standardCommandExport.Size = new Size(26, 24);
        standardCommandExport.Margin = new Padding(1);
        standardCommandExport.FlatStyle = FlatStyle.Flat;
        standardCommandExport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandExport.Text = "تصدير";
        flpActions.Controls.Add(standardCommandExport);
        designerCommandBar.SetCommandRole(standardCommandExport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Export);
        standardCommandHelp.AutoSize = false;
        standardCommandHelp.Dock = DockStyle.None;
        standardCommandHelp.MinimumSize = Size.Empty;
        standardCommandHelp.Size = new Size(26, 24);
        standardCommandHelp.Margin = new Padding(1);
        standardCommandHelp.FlatStyle = FlatStyle.Flat;
        standardCommandHelp.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandHelp.Text = "مساعدة";
        flpActions.Controls.Add(standardCommandHelp);
        designerCommandBar.SetCommandRole(standardCommandHelp, TransportERP.Desktop.CoreUI.DesignerCommandRole.Help);
    
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

    private Label lblDataStatus;
    private TabControl tabNumberingV20;
    private TabPage tabGeneral;
    private TableLayoutPanel policyLayout;
    private TableLayoutPanel policyFields;
    private Label lblCode;
    private TextBox txtCode;
    private Label lblArabicName;
    private TextBox txtArabicName;
    private Label lblEnglishName;
    private TextBox txtEnglishName;
    private Label lblStatus;
    private TextBox txtStatus;
    private Label lblDocumentType;
    private ComboBox cboDocumentType;
    private Label lblPrefix;
    private TextBox txtPrefix;
    private Label lblLastNumber;
    private NumericUpDown nudLastNumber;
    private Label lblResetType;
    private ComboBox cboResetType;
    private Label lblNotes;
    private TextBox txtNotes;
    private DataGridView dgvDocumentSequences;
    private TabPage tabDocument;
    private TableLayoutPanel scopeFields;
    private Label lblScopeCompany;
    private TextBox txtScopeCompany;
    private Label lblScopeBranch;
    private TextBox txtScopeBranch;
    private Label lblScopeYear;
    private TextBox txtScopeYear;
    private Label lblScopeHelp;
    private TabPage tabExceptions;
    private TableLayoutPanel exceptionLayout;
    private TableLayoutPanel exceptionFields;
    private Label lblOverridePolicy;
    private TextBox txtOverridePolicy;
    private Label lblExpectedVersion;
    private TextBox txtExpectedVersion;
    private Label lblPermissionStatus;
    private TextBox txtPermissionStatus;
    private Label lblApprovalStatus;
    private TextBox txtApprovalStatus;
    private Label lblOverrideReason;
    private TextBox txtOverrideReason;
    private Label lblExceptionsInfo;
    private GroupBox grpReservationDetails;
    private TableLayoutPanel reservationFields;
    private Label lblReservationNumber;
    private TextBox txtReservationNumber;
    private Label lblReservationState;
    private TextBox txtReservationState;
    private Label lblReservationId;
    private TextBox txtReservationId;
    private TabPage tabAllocationHistory;
    private TableLayoutPanel historyLayout;
    private Label lblHistoryInfo;
    private DataGridView dgvAllocationHistory;
    private FlowLayoutPanel flpActions;
    private Button btnView;
    private Button btnEdit;
    private Button btnReserve;
    private Button btnCommit;
    private Button btnCancelReservation;
    private Button btnOverride;
    private Button btnClose;
    private TableLayoutPanel tlpFilters;
    private Label lblCompany;
    private ComboBox cboCompany;
    private Label lblBranch;
    private ComboBox cboBranch;
    private Label lblFiscalYear;
    private ComboBox cboFiscalYear;
    private Label lblTitle;
    private TableLayoutPanel layoutRoot;
    private TableLayoutPanel tlpAuditInfo;
    private Label lblPrintCount;
    private Label lblLastPrintedAt;
    private Label lblEditCount;
    private Label lblModifiedAt;
    private Label lblModifiedBy;
    private Label lblCreatedAt;
    private Label lblCreatedBy;
}
