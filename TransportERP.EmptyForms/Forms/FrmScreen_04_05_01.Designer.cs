#nullable enable
using System.Drawing;
using System.Windows.Forms;
namespace TransportERP.EmptyForms;
partial class UcScreen_04_05_01
{
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;

    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
    private FlowLayoutPanel designerHiddenCommands = null!;
    private Button standardCommandDelete = null!;
    private Button standardCommandLast = null!;
    private Button standardCommandNext = null!;
    private Button standardCommandPrevious = null!;
    private Button standardCommandFirst = null!;
    private Button standardCommandRefresh = null!;
    private Button standardCommandImport = null!;
    private Button standardCommandExport = null!;
    private Button standardCommandHelp = null!;

    private TableLayoutPanel mainLayout = null!;
    private Label lblTitle = null!;
    private FlowLayoutPanel flpActions = null!;
    private Button btnView = null!;
    private Button btnCreate = null!;
    private Button btnEdit = null!;
    private Button btnCancel = null!;
    private Button btnPost = null!;
    private Button btnReverse = null!;
    private Button btnClear = null!;
    private Button btnClose = null!;
    private Label lblStatus = null!;
    private TabPage tp1 = null!;
    private Button btnAddRow = null!;
    private Button btnRemoveRow = null!;
    private ToolTip journalHints = null!;
    private Button btnPrint = null!;
    private System.ComponentModel.IContainer? components;
    private ErrorProvider validationErrors = null!;
    protected override void Dispose(bool disposing) { if(disposing) components?.Dispose(); base.Dispose(disposing); }
    private System.Windows.Forms.Panel rootWorkspaceViewport;

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcScreen_04_05_01));
        rootWorkspaceViewport = new Panel();
        mainLayout = new TableLayoutPanel();
        totalsPanel = new TableLayoutPanel();
        label12 = new Label();
        field_total = new TextBox();
        label13 = new Label();
        field_difference = new TextBox();
        lblTotalsMessage = new Label();
        tlpAuditInfo = new TableLayoutPanel();
        lblCreatedBy = new Label();
        lblCreatedAt = new Label();
        lblCreatedDevice = new Label();
        lblPrintCount = new Label();
        lblAccountName = new Label();
        lblModifiedBy = new Label();
        lblModifiedAt = new Label();
        lblModifiedDevice = new Label();
        lblLastPrintedAt = new Label();
        lblEditCount = new Label();
        tabs = new TabControl();
        tp0 = new TabPage();
        referenceHeader = new TableLayoutPanel();
        textBox2 = new TextBox();
        lbl_currencyRef = new Label();
        field_currencyRef = new ComboBox();
        lbl_exchangeRate = new Label();
        field_exchangeRate = new TextBox();
        tableLayoutPanel2 = new TableLayoutPanel();
        label11 = new Label();
        label10 = new Label();
        label9 = new Label();
        label8 = new Label();
        label7 = new Label();
        label6 = new Label();
        checkBox12 = new CheckBox();
        checkBox10 = new CheckBox();
        checkBox8 = new CheckBox();
        checkBox6 = new CheckBox();
        checkBox4 = new CheckBox();
        checkBox2 = new CheckBox();
        tableLayoutPanel1 = new TableLayoutPanel();
        label5 = new Label();
        checkBox1 = new CheckBox();
        textBox6 = new TextBox();
        field_accountingDate = new ReceiptDateTimePicker();
        textBox5 = new TextBox();
        label4 = new Label();
        field_documentNumber = new TextBox();
        label3 = new Label();
        label2 = new Label();
        comboBox5 = new ComboBox();
        field_description = new TextBox();
        label1 = new Label();
        textBox1 = new ComboBox();
        lbl_destinationCashBankRef = new Label();
        field_destinationCashBankRef = new ComboBox();
        lbl_collector = new Label();
        lbl_voucherNumber = new Label();
        field_externalReference = new TextBox();
        lbl_attachmentCount = new Label();
        field_attachmentCount = new TextBox();
        lbl_amount = new Label();
        field_amount = new TextBox();
        lbl_foreignReceipt = new Label();
        field_foreignReceipt = new TextBox();
        lbl_partyRef = new Label();
        lbl_description = new Label();
        lblAmountWords = new Label();
        documentAdditional = new TabPage();
        documentAdditionalSections = new TabControl();
        tabPage1 = new TabPage();
        layout0 = new TableLayoutPanel();
        lbl_sourceCashBankRef = new Label();
        field_sourceCashBankRef = new ComboBox();
        lbl_counterAccountRef = new Label();
        field_counterAccountRef = new ComboBox();
        lbl_state = new Label();
        field_state = new TextBox();
        referenceLabel16 = new Label();
        referenceField16 = new ComboBox();
        referenceLabel17 = new Label();
        referenceField17 = new ComboBox();
        lbl_referenceType = new Label();
        field_referenceType = new TextBox();
        lblReceiptFlags = new Label();
        documentAccounts = new TabPage();
        documentAccountsGrid = new DataGridView();
        documentAccountsGridColumn0 = new DataGridViewTextBoxColumn();
        documentAccountsGridColumn1 = new DataGridViewTextBoxColumn();
        documentAccountsGridColumn2 = new DataGridViewTextBoxColumn();
        documentAccountsGridColumn3 = new DataGridViewTextBoxColumn();
        documentAccountsGridColumn4 = new DataGridViewTextBoxColumn();
        documentAccountsGridColumn5 = new DataGridViewTextBoxColumn();
        documentAccountsGridColumn6 = new DataGridViewTextBoxColumn();
        documentAccountsGridColumn7 = new DataGridViewTextBoxColumn();
        tp2 = new TabPage();
        layout2 = new TableLayoutPanel();
        context2 = new RichTextBox();
        tp3 = new TabPage();
        layout3 = new TableLayoutPanel();
        context3 = new RichTextBox();
        btnReceiptApprove = new Button();
        tp4 = new TabPage();
        layout4 = new TableLayoutPanel();
        context4 = new RichTextBox();
        documentDefaults = new TabPage();
        documentDefaultsLayout = new TableLayoutPanel();
        lblDefaultCurrency = new Label();
        cboDefaultCurrency = new ComboBox();
        lblDefaultCostCenter = new Label();
        cboDefaultCostCenter = new ComboBox();
        btnReceiptSettings = new Button();
        documentImport = new TabPage();
        documentImportLayout = new TableLayoutPanel();
        documentImportFormat = new Label();
        documentImportButtons = new FlowLayoutPanel();
        documentImportPath = new TextBox();
        documentImportChoose = new Button();
        documentImportApply = new Button();
        documentImportPreview = new DataGridView();
        dgvLines = new DataGridView();
        col_accountRef = new DataGridViewComboBoxColumn();
        col_analyticalAccount = new DataGridViewTextBoxColumn();
        col_accountName = new DataGridViewTextBoxColumn();
        col_lineDescription = new DataGridViewTextBoxColumn();
        col_currencyRef = new DataGridViewComboBoxColumn();
        col_exchangeRate = new DataGridViewTextBoxColumn();
        col_foreignDebit = new DataGridViewTextBoxColumn();
        col_foreignCredit = new DataGridViewTextBoxColumn();
        col_rowNo = new DataGridViewTextBoxColumn();
        col_costCenterRef = new DataGridViewComboBoxColumn();
        col_debit = new DataGridViewTextBoxColumn();
        col_credit = new DataGridViewTextBoxColumn();
        col_accountingAmount = new DataGridViewTextBoxColumn();
        col_approvalNumber = new DataGridViewTextBoxColumn();
        col_salesperson = new DataGridViewTextBoxColumn();
        col_collector = new DataGridViewTextBoxColumn();
        col_referenceNumber = new DataGridViewTextBoxColumn();
        lblTitle = new Label();
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        flpActions = new FlowLayoutPanel();
        btnClear = new Button();
        btnView = new Button();
        btnCreate = new Button();
        btnEdit = new Button();
        btnCancel = new Button();
        btnPrint = new Button();
        btnPost = new Button();
        btnReverse = new Button();
        btnClose = new Button();
        btnAddRow = new Button();
        btnRemoveRow = new Button();
        designerHiddenCommands = new FlowLayoutPanel();
        standardCommandDelete = new Button();
        standardCommandLast = new Button();
        standardCommandNext = new Button();
        standardCommandPrevious = new Button();
        standardCommandFirst = new Button();
        standardCommandRefresh = new Button();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
        lblStatus = new Label();
        validationErrors = new ErrorProvider(components);
        tp1 = new TabPage();
        journalHints = new ToolTip(components);
        standardAuditMetadata = new TransportERP.Desktop.CoreUI.AuditMetadataControl();
        rootWorkspaceViewport.SuspendLayout();
        mainLayout.SuspendLayout();
        totalsPanel.SuspendLayout();
        tlpAuditInfo.SuspendLayout();
        tabs.SuspendLayout();
        tp0.SuspendLayout();
        referenceHeader.SuspendLayout();
        tableLayoutPanel2.SuspendLayout();
        tableLayoutPanel1.SuspendLayout();
        documentAdditional.SuspendLayout();
        documentAdditionalSections.SuspendLayout();
        tabPage1.SuspendLayout();
        layout0.SuspendLayout();
        documentAccounts.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)documentAccountsGrid).BeginInit();
        tp2.SuspendLayout();
        layout2.SuspendLayout();
        tp3.SuspendLayout();
        layout3.SuspendLayout();
        tp4.SuspendLayout();
        layout4.SuspendLayout();
        documentDefaults.SuspendLayout();
        documentDefaultsLayout.SuspendLayout();
        documentImport.SuspendLayout();
        documentImportLayout.SuspendLayout();
        documentImportButtons.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)documentImportPreview).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
        designerCommandBar.SuspendLayout();
        flpActions.SuspendLayout();
        designerHiddenCommands.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)validationErrors).BeginInit();
        SuspendLayout();
        // 
        // rootWorkspaceViewport
        // 
        rootWorkspaceViewport.AutoScroll = true;
        rootWorkspaceViewport.AutoScrollMinSize = new Size(1225, 850);
        rootWorkspaceViewport.Controls.Add(mainLayout);
        rootWorkspaceViewport.Dock = DockStyle.Fill;
        rootWorkspaceViewport.Location = new Point(0, 0);
        rootWorkspaceViewport.Margin = new Padding(0);
        rootWorkspaceViewport.Name = "rootWorkspaceViewport";
        rootWorkspaceViewport.Size = new Size(1200, 836);
        rootWorkspaceViewport.TabIndex = 0;
        // 
        // mainLayout
        // 
        mainLayout.BackColor = Color.FromArgb(250, 249, 240);
        mainLayout.ColumnCount = 1;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        mainLayout.Controls.Add(totalsPanel, 0, 4);
        mainLayout.Controls.Add(tlpAuditInfo, 0, 5);
        mainLayout.Controls.Add(tabs, 0, 2);
        mainLayout.Controls.Add(dgvLines, 0, 3);
        mainLayout.Controls.Add(lblTitle, 0, 0);
        mainLayout.Controls.Add(designerCommandBar, 0, 1);
        mainLayout.Controls.Add(lblStatus, 0, 6);
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Location = new Point(0, 0);
        mainLayout.MinimumSize = new Size(980, 680);
        mainLayout.Name = "mainLayout";
        mainLayout.Padding = new Padding(8);
        mainLayout.RowCount = 7;
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 219F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.Size = new Size(1225, 850);
        mainLayout.TabIndex = 0;
        // 
        // totalsPanel
        // 
        totalsPanel.AutoSize = true;
        totalsPanel.BackColor = Color.FromArgb(225, 237, 248);
        totalsPanel.ColumnCount = 6;
        totalsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
        totalsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 75F));
        totalsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
        totalsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 75F));
        totalsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
        totalsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
        totalsPanel.Controls.Add(label12, 1, 0);
        totalsPanel.Controls.Add(field_total, 2, 0);
        totalsPanel.Controls.Add(label13, 3, 0);
        totalsPanel.Controls.Add(field_difference, 4, 0);
        totalsPanel.Controls.Add(lblTotalsMessage, 0, 0);
        totalsPanel.Dock = DockStyle.Fill;
        totalsPanel.Location = new Point(11, 711);
        totalsPanel.Name = "totalsPanel";
        totalsPanel.Padding = new Padding(6, 3, 6, 3);
        totalsPanel.RightToLeft = RightToLeft.Yes;
        totalsPanel.RowCount = 1;
        totalsPanel.RowStyles.Add(new RowStyle());
        totalsPanel.Size = new Size(1203, 36);
        totalsPanel.TabIndex = 8;
        // 
        // label12
        // 
        label12.AutoSize = true;
        label12.Dock = DockStyle.Fill;
        label12.Location = new Point(705, 3);
        label12.Name = "label12";
        label12.Size = new Size(69, 30);
        label12.TabIndex = 15;
        label12.Text = "المجموع";
        label12.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_total
        // 
        field_total.AccessibleName = "المجموع";
        field_total.BackColor = Color.White;
        field_total.Dock = DockStyle.Fill;
        field_total.Location = new Point(535, 6);
        field_total.Name = "field_total";
        field_total.ReadOnly = true;
        field_total.Size = new Size(164, 22);
        field_total.TabIndex = 14;
        field_total.TabStop = false;
        field_total.Text = "—";
        field_total.TextAlign = HorizontalAlignment.Right;
        // 
        // label13
        // 
        label13.AutoSize = true;
        label13.Dock = DockStyle.Fill;
        label13.Location = new Point(460, 3);
        label13.Name = "label13";
        label13.Size = new Size(69, 30);
        label13.TabIndex = 13;
        label13.Text = "الفارق";
        label13.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_difference
        // 
        field_difference.AccessibleName = "الفارق";
        field_difference.BackColor = Color.White;
        field_difference.Dock = DockStyle.Fill;
        field_difference.Location = new Point(290, 6);
        field_difference.Name = "field_difference";
        field_difference.ReadOnly = true;
        field_difference.Size = new Size(164, 22);
        field_difference.TabIndex = 11;
        field_difference.TabStop = false;
        field_difference.Text = "—";
        field_difference.TextAlign = HorizontalAlignment.Right;
        // 
        // lblTotalsMessage
        // 
        lblTotalsMessage.AutoSize = true;
        lblTotalsMessage.Dock = DockStyle.Fill;
        lblTotalsMessage.Location = new Point(783, 6);
        lblTotalsMessage.Margin = new Padding(6, 3, 6, 3);
        lblTotalsMessage.Name = "lblTotalsMessage";
        lblTotalsMessage.RightToLeft = RightToLeft.Yes;
        lblTotalsMessage.Size = new Size(408, 24);
        lblTotalsMessage.TabIndex = 16;
        lblTotalsMessage.TextAlign = ContentAlignment.MiddleRight;
        // 
        // tlpAuditInfo
        // 
        tlpAuditInfo.AccessibleName = "بيانات متابعة قيد اليومية";
        tlpAuditInfo.AutoSize = true;
        tlpAuditInfo.BackColor = Color.FromArgb(248, 250, 252);
        tlpAuditInfo.ColumnCount = 5;
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        tlpAuditInfo.Controls.Add(lblCreatedBy, 0, 0);
        tlpAuditInfo.Controls.Add(lblCreatedAt, 1, 0);
        tlpAuditInfo.Controls.Add(lblCreatedDevice, 2, 0);
        tlpAuditInfo.Controls.Add(lblPrintCount, 3, 0);
        tlpAuditInfo.Controls.Add(lblAccountName, 4, 0);
        tlpAuditInfo.Controls.Add(lblModifiedBy, 0, 1);
        tlpAuditInfo.Controls.Add(lblModifiedAt, 1, 1);
        tlpAuditInfo.Controls.Add(lblModifiedDevice, 2, 1);
        tlpAuditInfo.Controls.Add(lblLastPrintedAt, 3, 1);
        tlpAuditInfo.Controls.Add(lblEditCount, 4, 1);
        tlpAuditInfo.Dock = DockStyle.Fill;
        tlpAuditInfo.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
        tlpAuditInfo.Location = new Point(8, 750);
        tlpAuditInfo.Margin = new Padding(0);
        tlpAuditInfo.Name = "tlpAuditInfo";
        tlpAuditInfo.Padding = new Padding(4);
        tlpAuditInfo.RightToLeft = RightToLeft.Yes;
        tlpAuditInfo.RowCount = 2;
        tlpAuditInfo.RowStyles.Add(new RowStyle());
        tlpAuditInfo.RowStyles.Add(new RowStyle());
        tlpAuditInfo.Size = new Size(1209, 66);
        tlpAuditInfo.TabIndex = 7;
        tlpAuditInfo.Visible = false;
        // 
        // lblCreatedBy
        // 
        lblCreatedBy.AutoSize = true;
        lblCreatedBy.BackColor = Color.FromArgb(232, 246, 248);
        lblCreatedBy.Dock = DockStyle.Fill;
        lblCreatedBy.Font = new Font("Tahoma", 9F);
        lblCreatedBy.ForeColor = Color.FromArgb(16, 24, 40);
        lblCreatedBy.Location = new Point(967, 6);
        lblCreatedBy.Margin = new Padding(2);
        lblCreatedBy.MinimumSize = new Size(0, 26);
        lblCreatedBy.Name = "lblCreatedBy";
        lblCreatedBy.Padding = new Padding(6, 3, 6, 3);
        lblCreatedBy.RightToLeft = RightToLeft.Yes;
        lblCreatedBy.Size = new Size(236, 26);
        lblCreatedBy.TabIndex = 0;
        lblCreatedBy.Text = "أنشأ بواسطة: —";
        lblCreatedBy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCreatedAt
        // 
        lblCreatedAt.AutoSize = true;
        lblCreatedAt.BackColor = Color.FromArgb(232, 246, 248);
        lblCreatedAt.Dock = DockStyle.Fill;
        lblCreatedAt.Font = new Font("Tahoma", 9F);
        lblCreatedAt.ForeColor = Color.FromArgb(16, 24, 40);
        lblCreatedAt.Location = new Point(727, 6);
        lblCreatedAt.Margin = new Padding(2);
        lblCreatedAt.MinimumSize = new Size(0, 26);
        lblCreatedAt.Name = "lblCreatedAt";
        lblCreatedAt.Padding = new Padding(6, 3, 6, 3);
        lblCreatedAt.RightToLeft = RightToLeft.Yes;
        lblCreatedAt.Size = new Size(236, 26);
        lblCreatedAt.TabIndex = 1;
        lblCreatedAt.Text = "تاريخ الانشاء: —";
        lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCreatedDevice
        // 
        lblCreatedDevice.AutoSize = true;
        lblCreatedDevice.Dock = DockStyle.Fill;
        lblCreatedDevice.Location = new Point(487, 6);
        lblCreatedDevice.Margin = new Padding(2);
        lblCreatedDevice.MinimumSize = new Size(0, 26);
        lblCreatedDevice.Name = "lblCreatedDevice";
        lblCreatedDevice.Padding = new Padding(6, 3, 6, 3);
        lblCreatedDevice.RightToLeft = RightToLeft.Yes;
        lblCreatedDevice.Size = new Size(236, 26);
        lblCreatedDevice.TabIndex = 2;
        lblCreatedDevice.Text = "جهاز الإدخال: —";
        lblCreatedDevice.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblPrintCount
        // 
        lblPrintCount.AutoSize = true;
        lblPrintCount.BackColor = Color.FromArgb(232, 246, 248);
        lblPrintCount.Dock = DockStyle.Fill;
        lblPrintCount.Font = new Font("Tahoma", 9F);
        lblPrintCount.ForeColor = Color.FromArgb(16, 24, 40);
        lblPrintCount.Location = new Point(247, 6);
        lblPrintCount.Margin = new Padding(2);
        lblPrintCount.MinimumSize = new Size(0, 26);
        lblPrintCount.Name = "lblPrintCount";
        lblPrintCount.Padding = new Padding(6, 3, 6, 3);
        lblPrintCount.RightToLeft = RightToLeft.Yes;
        lblPrintCount.Size = new Size(236, 26);
        lblPrintCount.TabIndex = 3;
        lblPrintCount.Text = "عدد مرات الطباعة: —";
        lblPrintCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblAccountName
        // 
        lblAccountName.AutoSize = true;
        lblAccountName.Dock = DockStyle.Fill;
        lblAccountName.Location = new Point(6, 6);
        lblAccountName.Margin = new Padding(2);
        lblAccountName.MinimumSize = new Size(0, 26);
        lblAccountName.Name = "lblAccountName";
        lblAccountName.Padding = new Padding(6, 3, 6, 3);
        lblAccountName.RightToLeft = RightToLeft.Yes;
        lblAccountName.Size = new Size(237, 26);
        lblAccountName.TabIndex = 4;
        lblAccountName.Text = "اسم الحساب: —";
        lblAccountName.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblModifiedBy
        // 
        lblModifiedBy.AutoSize = true;
        lblModifiedBy.BackColor = Color.FromArgb(232, 246, 248);
        lblModifiedBy.Dock = DockStyle.Fill;
        lblModifiedBy.Font = new Font("Tahoma", 9F);
        lblModifiedBy.ForeColor = Color.FromArgb(16, 24, 40);
        lblModifiedBy.Location = new Point(967, 36);
        lblModifiedBy.Margin = new Padding(2);
        lblModifiedBy.MinimumSize = new Size(0, 26);
        lblModifiedBy.Name = "lblModifiedBy";
        lblModifiedBy.Padding = new Padding(6, 3, 6, 3);
        lblModifiedBy.RightToLeft = RightToLeft.Yes;
        lblModifiedBy.Size = new Size(236, 26);
        lblModifiedBy.TabIndex = 5;
        lblModifiedBy.Text = "عدل بواسطة: —";
        lblModifiedBy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblModifiedAt
        // 
        lblModifiedAt.AutoSize = true;
        lblModifiedAt.BackColor = Color.FromArgb(232, 246, 248);
        lblModifiedAt.Dock = DockStyle.Fill;
        lblModifiedAt.Font = new Font("Tahoma", 9F);
        lblModifiedAt.ForeColor = Color.FromArgb(16, 24, 40);
        lblModifiedAt.Location = new Point(727, 36);
        lblModifiedAt.Margin = new Padding(2);
        lblModifiedAt.MinimumSize = new Size(0, 26);
        lblModifiedAt.Name = "lblModifiedAt";
        lblModifiedAt.Padding = new Padding(6, 3, 6, 3);
        lblModifiedAt.RightToLeft = RightToLeft.Yes;
        lblModifiedAt.Size = new Size(236, 26);
        lblModifiedAt.TabIndex = 6;
        lblModifiedAt.Text = "تاريخ التعديل: —";
        lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblModifiedDevice
        // 
        lblModifiedDevice.AutoSize = true;
        lblModifiedDevice.Dock = DockStyle.Fill;
        lblModifiedDevice.Location = new Point(487, 36);
        lblModifiedDevice.Margin = new Padding(2);
        lblModifiedDevice.MinimumSize = new Size(0, 26);
        lblModifiedDevice.Name = "lblModifiedDevice";
        lblModifiedDevice.Padding = new Padding(6, 3, 6, 3);
        lblModifiedDevice.RightToLeft = RightToLeft.Yes;
        lblModifiedDevice.Size = new Size(236, 26);
        lblModifiedDevice.TabIndex = 7;
        lblModifiedDevice.Text = "جهاز التعديل: —";
        lblModifiedDevice.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblLastPrintedAt
        // 
        lblLastPrintedAt.AutoSize = true;
        lblLastPrintedAt.BackColor = Color.FromArgb(232, 246, 248);
        lblLastPrintedAt.Dock = DockStyle.Fill;
        lblLastPrintedAt.Font = new Font("Tahoma", 9F);
        lblLastPrintedAt.ForeColor = Color.FromArgb(16, 24, 40);
        lblLastPrintedAt.Location = new Point(247, 36);
        lblLastPrintedAt.Margin = new Padding(2);
        lblLastPrintedAt.MinimumSize = new Size(0, 26);
        lblLastPrintedAt.Name = "lblLastPrintedAt";
        lblLastPrintedAt.Padding = new Padding(6, 3, 6, 3);
        lblLastPrintedAt.RightToLeft = RightToLeft.Yes;
        lblLastPrintedAt.Size = new Size(236, 26);
        lblLastPrintedAt.TabIndex = 8;
        lblLastPrintedAt.Text = "تاريخ اخر طباعة: —";
        lblLastPrintedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblEditCount
        // 
        lblEditCount.AutoSize = true;
        lblEditCount.BackColor = Color.FromArgb(232, 246, 248);
        lblEditCount.Dock = DockStyle.Fill;
        lblEditCount.Font = new Font("Tahoma", 9F);
        lblEditCount.ForeColor = Color.FromArgb(16, 24, 40);
        lblEditCount.Location = new Point(6, 36);
        lblEditCount.Margin = new Padding(2);
        lblEditCount.MinimumSize = new Size(0, 26);
        lblEditCount.Name = "lblEditCount";
        lblEditCount.Padding = new Padding(6, 3, 6, 3);
        lblEditCount.RightToLeft = RightToLeft.Yes;
        lblEditCount.Size = new Size(237, 26);
        lblEditCount.TabIndex = 9;
        lblEditCount.Text = "عدد التعديلات: —";
        lblEditCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // tabs
        // 
        tabs.Controls.Add(tp0);
        tabs.Controls.Add(documentAdditional);
        tabs.Controls.Add(documentDefaults);
        tabs.Controls.Add(documentImport);
        tabs.Dock = DockStyle.Fill;
        tabs.Font = new Font("Tahoma", 9F);
        tabs.Location = new Point(10, 108);
        tabs.Margin = new Padding(2);
        tabs.Multiline = true;
        tabs.Name = "tabs";
        tabs.RightToLeft = RightToLeft.Yes;
        tabs.RightToLeftLayout = true;
        tabs.SelectedIndex = 0;
        tabs.Size = new Size(1205, 215);
        tabs.TabIndex = 6;
        // 
        // tp0
        // 
        tp0.BackColor = Color.FromArgb(250, 249, 240);
        tp0.Controls.Add(referenceHeader);
        tp0.Location = new Point(4, 23);
        tp0.Margin = new Padding(4);
        tp0.Name = "tp0";
        tp0.RightToLeft = RightToLeft.Yes;
        tp0.Size = new Size(1197, 188);
        tp0.TabIndex = 0;
        tp0.Text = "البيانات الرئيسية";
        // 
        // referenceHeader
        // 
        referenceHeader.AutoSize = true;
        referenceHeader.BackColor = Color.FromArgb(245, 245, 245);
        referenceHeader.ColumnCount = 7;
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32.8947372F));
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27.6315784F));
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 169F));
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 121F));
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 39.4736862F));
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
        referenceHeader.Controls.Add(textBox2, 6, 5);
        referenceHeader.Controls.Add(lbl_currencyRef, 2, 3);
        referenceHeader.Controls.Add(field_currencyRef, 3, 3);
        referenceHeader.Controls.Add(lbl_exchangeRate, 4, 4);
        referenceHeader.Controls.Add(field_exchangeRate, 5, 4);
        referenceHeader.Controls.Add(tableLayoutPanel2, 6, 0);
        referenceHeader.Controls.Add(tableLayoutPanel1, 4, 0);
        referenceHeader.Controls.Add(field_accountingDate, 5, 1);
        referenceHeader.Controls.Add(textBox5, 3, 2);
        referenceHeader.Controls.Add(label4, 2, 2);
        referenceHeader.Controls.Add(field_documentNumber, 3, 1);
        referenceHeader.Controls.Add(label3, 2, 1);
        referenceHeader.Controls.Add(label2, 3, 5);
        referenceHeader.Controls.Add(comboBox5, 1, 5);
        referenceHeader.Controls.Add(field_description, 1, 4);
        referenceHeader.Controls.Add(label1, 0, 0);
        referenceHeader.Controls.Add(textBox1, 1, 0);
        referenceHeader.Controls.Add(lbl_destinationCashBankRef, 0, 1);
        referenceHeader.Controls.Add(field_destinationCashBankRef, 1, 1);
        referenceHeader.Controls.Add(lbl_collector, 4, 1);
        referenceHeader.Controls.Add(lbl_voucherNumber, 0, 2);
        referenceHeader.Controls.Add(field_externalReference, 1, 2);
        referenceHeader.Controls.Add(lbl_attachmentCount, 4, 2);
        referenceHeader.Controls.Add(field_attachmentCount, 5, 2);
        referenceHeader.Controls.Add(lbl_amount, 0, 3);
        referenceHeader.Controls.Add(field_amount, 1, 3);
        referenceHeader.Controls.Add(lbl_foreignReceipt, 4, 3);
        referenceHeader.Controls.Add(field_foreignReceipt, 5, 3);
        referenceHeader.Controls.Add(lbl_partyRef, 0, 4);
        referenceHeader.Controls.Add(lbl_description, 0, 5);
        referenceHeader.Controls.Add(lblAmountWords, 0, 6);
        referenceHeader.Dock = DockStyle.Fill;
        referenceHeader.Location = new Point(0, 0);
        referenceHeader.Name = "referenceHeader";
        referenceHeader.Padding = new Padding(2);
        referenceHeader.RightToLeft = RightToLeft.Yes;
        referenceHeader.RowCount = 7;
        referenceHeader.RowStyles.Add(new RowStyle());
        referenceHeader.RowStyles.Add(new RowStyle());
        referenceHeader.RowStyles.Add(new RowStyle());
        referenceHeader.RowStyles.Add(new RowStyle());
        referenceHeader.RowStyles.Add(new RowStyle());
        referenceHeader.RowStyles.Add(new RowStyle());
        referenceHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        referenceHeader.Size = new Size(1197, 188);
        referenceHeader.TabIndex = 1;
        // 
        // textBox2
        // 
        textBox2.AccessibleName = "الرقم";
        textBox2.BackColor = Color.White;
        textBox2.Dock = DockStyle.Fill;
        textBox2.Font = new Font("Tahoma", 9.25F);
        textBox2.Location = new Point(3, 136);
        textBox2.Margin = new Padding(1, 2, 1, 2);
        textBox2.Name = "textBox2";
        textBox2.ReadOnly = true;
        textBox2.Size = new Size(199, 22);
        textBox2.TabIndex = 63;
        textBox2.TabStop = false;
        // 
        // lbl_currencyRef
        // 
        lbl_currencyRef.AutoSize = true;
        lbl_currencyRef.Dock = DockStyle.Fill;
        lbl_currencyRef.Location = new Point(436, 82);
        lbl_currencyRef.Name = "lbl_currencyRef";
        lbl_currencyRef.Size = new Size(115, 26);
        lbl_currencyRef.TabIndex = 0;
        lbl_currencyRef.Text = "العملة";
        lbl_currencyRef.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_currencyRef
        // 
        field_currencyRef.AccessibleName = "العملة";
        field_currencyRef.BackColor = Color.LightYellow;
        field_currencyRef.FlatStyle = FlatStyle.Flat;        field_currencyRef.Dock = DockStyle.Fill;
        field_currencyRef.DropDownStyle = ComboBoxStyle.DropDownList;
        field_currencyRef.DropDownWidth = 420;
        field_currencyRef.Location = new Point(204, 84);
        field_currencyRef.Margin = new Padding(1, 2, 1, 2);
        field_currencyRef.Name = "field_currencyRef";
        field_currencyRef.Size = new Size(228, 22);
        field_currencyRef.TabIndex = 59;
        field_currencyRef.Tag = "ACC-042:currencyRef";
        // 
        // lbl_exchangeRate
        // 
        lbl_exchangeRate.AutoSize = true;
        lbl_exchangeRate.Dock = DockStyle.Fill;
        lbl_exchangeRate.Location = new Point(1078, 134);
        lbl_exchangeRate.Name = "lbl_exchangeRate";
        lbl_exchangeRate.Size = new Size(114, 26);
        lbl_exchangeRate.TabIndex = 60;
        lbl_exchangeRate.Text = "سعر الصرف";
        lbl_exchangeRate.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_exchangeRate
        // 
        field_exchangeRate.AccessibleName = "سعر الصرف";
        field_exchangeRate.BackColor = Color.LightYellow;
        field_exchangeRate.Dock = DockStyle.Fill;
        field_exchangeRate.Location = new Point(885, 136);
        field_exchangeRate.Margin = new Padding(1, 2, 1, 2);
        field_exchangeRate.Name = "field_exchangeRate";
        field_exchangeRate.RightToLeft = RightToLeft.No;
        field_exchangeRate.Size = new Size(189, 22);
        field_exchangeRate.TabIndex = 60;
        field_exchangeRate.Tag = "ACC-042:exchangeRate";
        field_exchangeRate.TextAlign = HorizontalAlignment.Right;
        // 
        // tableLayoutPanel2
        // 
        tableLayoutPanel2.AutoSize = true;
        tableLayoutPanel2.ColumnCount = 2;
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 59F));
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel2.Controls.Add(label11, 1, 5);
        tableLayoutPanel2.Controls.Add(label10, 1, 4);
        tableLayoutPanel2.Controls.Add(label9, 1, 3);
        tableLayoutPanel2.Controls.Add(label8, 1, 2);
        tableLayoutPanel2.Controls.Add(label7, 1, 1);
        tableLayoutPanel2.Controls.Add(label6, 1, 0);
        tableLayoutPanel2.Controls.Add(checkBox12, 0, 5);
        tableLayoutPanel2.Controls.Add(checkBox10, 0, 4);
        tableLayoutPanel2.Controls.Add(checkBox8, 0, 3);
        tableLayoutPanel2.Controls.Add(checkBox6, 0, 2);
        tableLayoutPanel2.Controls.Add(checkBox4, 0, 1);
        tableLayoutPanel2.Controls.Add(checkBox2, 0, 0);
        tableLayoutPanel2.Dock = DockStyle.Fill;
        tableLayoutPanel2.Location = new Point(2, 2);
        tableLayoutPanel2.Margin = new Padding(0);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        tableLayoutPanel2.RightToLeft = RightToLeft.Yes;
        tableLayoutPanel2.RowCount = 5;
        referenceHeader.SetRowSpan(tableLayoutPanel2, 5);
        tableLayoutPanel2.RowStyles.Add(new RowStyle());
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tableLayoutPanel2.Size = new Size(201, 132);
        tableLayoutPanel2.TabIndex = 62;
        // 
        // label11
        // 
        label11.AutoSize = true;
        label11.Dock = DockStyle.Fill;
        label11.Font = new Font("Tahoma", 8.25F);
        label11.Location = new Point(1, 101);
        label11.Margin = new Padding(1);
        label11.Name = "label11";
        label11.Size = new Size(140, 30);
        label11.TabIndex = 22;
        label11.Text = "تعليق";
        label11.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // label10
        // 
        label10.AutoSize = true;
        label10.Dock = DockStyle.Fill;
        label10.Font = new Font("Tahoma", 8.25F);
        label10.Location = new Point(1, 81);
        label10.Margin = new Padding(1);
        label10.Name = "label10";
        label10.Size = new Size(140, 18);
        label10.TabIndex = 21;
        label10.Text = "فروق عملة";
        label10.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // label9
        // 
        label9.AutoSize = true;
        label9.Dock = DockStyle.Fill;
        label9.Font = new Font("Tahoma", 8.25F);
        label9.Location = new Point(1, 61);
        label9.Margin = new Padding(1);
        label9.Name = "label9";
        label9.Size = new Size(140, 18);
        label9.TabIndex = 20;
        label9.Text = "قيد دوري";
        label9.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // label8
        // 
        label8.AutoSize = true;
        label8.Dock = DockStyle.Fill;
        label8.Font = new Font("Tahoma", 8.25F);
        label8.Location = new Point(1, 41);
        label8.Margin = new Padding(1);
        label8.Name = "label8";
        label8.Size = new Size(140, 18);
        label8.TabIndex = 19;
        label8.Text = "قيد عكسي";
        label8.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // label7
        // 
        label7.AutoSize = true;
        label7.Dock = DockStyle.Fill;
        label7.Font = new Font("Tahoma", 8.25F);
        label7.Location = new Point(1, 21);
        label7.Margin = new Padding(1);
        label7.Name = "label7";
        label7.Size = new Size(140, 18);
        label7.TabIndex = 18;
        label7.Text = "مرحل";
        label7.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // label6
        // 
        label6.AutoSize = true;
        label6.Dock = DockStyle.Fill;
        label6.Font = new Font("Tahoma", 8.25F);
        label6.Location = new Point(1, 1);
        label6.Margin = new Padding(1);
        label6.Name = "label6";
        label6.Size = new Size(140, 18);
        label6.TabIndex = 17;
        label6.Text = "روجع";
        label6.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // checkBox12
        // 
        checkBox12.AutoSize = true;
        checkBox12.Dock = DockStyle.Right;
        checkBox12.Location = new Point(145, 103);
        checkBox12.Name = "checkBox12";
        checkBox12.RightToLeft = RightToLeft.Yes;
        checkBox12.Size = new Size(15, 26);
        checkBox12.TabIndex = 14;
        checkBox12.TextAlign = ContentAlignment.MiddleCenter;
        checkBox12.UseVisualStyleBackColor = true;
        // 
        // checkBox10
        // 
        checkBox10.AutoSize = true;
        checkBox10.Dock = DockStyle.Right;
        checkBox10.Location = new Point(145, 83);
        checkBox10.Name = "checkBox10";
        checkBox10.RightToLeft = RightToLeft.Yes;
        checkBox10.Size = new Size(15, 14);
        checkBox10.TabIndex = 12;
        checkBox10.TextAlign = ContentAlignment.MiddleCenter;
        checkBox10.UseVisualStyleBackColor = true;
        // 
        // checkBox8
        // 
        checkBox8.AutoSize = true;
        checkBox8.Dock = DockStyle.Right;
        checkBox8.Location = new Point(145, 63);
        checkBox8.Name = "checkBox8";
        checkBox8.RightToLeft = RightToLeft.Yes;
        checkBox8.Size = new Size(15, 14);
        checkBox8.TabIndex = 10;
        checkBox8.TextAlign = ContentAlignment.MiddleCenter;
        checkBox8.UseVisualStyleBackColor = true;
        // 
        // checkBox6
        // 
        checkBox6.AutoSize = true;
        checkBox6.Dock = DockStyle.Right;
        checkBox6.Location = new Point(145, 43);
        checkBox6.Name = "checkBox6";
        checkBox6.RightToLeft = RightToLeft.Yes;
        checkBox6.Size = new Size(15, 14);
        checkBox6.TabIndex = 8;
        checkBox6.TextAlign = ContentAlignment.MiddleCenter;
        checkBox6.UseVisualStyleBackColor = true;
        // 
        // checkBox4
        // 
        checkBox4.AutoSize = true;
        checkBox4.Dock = DockStyle.Right;
        checkBox4.Location = new Point(145, 23);
        checkBox4.Name = "checkBox4";
        checkBox4.RightToLeft = RightToLeft.Yes;
        checkBox4.Size = new Size(15, 14);
        checkBox4.TabIndex = 6;
        checkBox4.TextAlign = ContentAlignment.MiddleCenter;
        checkBox4.UseVisualStyleBackColor = true;
        // 
        // checkBox2
        // 
        checkBox2.AutoSize = true;
        checkBox2.Dock = DockStyle.Right;
        checkBox2.Location = new Point(145, 3);
        checkBox2.Name = "checkBox2";
        checkBox2.RightToLeft = RightToLeft.Yes;
        checkBox2.Size = new Size(15, 14);
        checkBox2.TabIndex = 4;
        checkBox2.TextAlign = ContentAlignment.MiddleCenter;
        checkBox2.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.AutoSize = true;
        tableLayoutPanel1.ColumnCount = 3;
        referenceHeader.SetColumnSpan(tableLayoutPanel1, 2);
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 121F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.Controls.Add(label5, 1, 0);
        tableLayoutPanel1.Controls.Add(checkBox1, 0, 0);
        tableLayoutPanel1.Controls.Add(textBox6, 2, 0);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(203, 2);
        tableLayoutPanel1.Margin = new Padding(0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 1;
        tableLayoutPanel1.RowStyles.Add(new RowStyle());
        tableLayoutPanel1.Size = new Size(351, 28);
        tableLayoutPanel1.TabIndex = 59;
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Dock = DockStyle.Fill;
        label5.Font = new Font("Tahoma", 8.25F);
        label5.Location = new Point(116, 1);
        label5.Margin = new Padding(1);
        label5.Name = "label5";
        label5.Size = new Size(113, 26);
        label5.TabIndex = 18;
        label5.Text = "رقم القيد الدوري";
        label5.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // checkBox1
        // 
        checkBox1.AutoSize = true;
        checkBox1.Dock = DockStyle.Right;
        checkBox1.Location = new Point(233, 3);
        checkBox1.Name = "checkBox1";
        checkBox1.Size = new Size(15, 22);
        checkBox1.TabIndex = 3;
        checkBox1.TextAlign = ContentAlignment.MiddleCenter;
        checkBox1.UseVisualStyleBackColor = true;
        // 
        // textBox6
        // 
        textBox6.AccessibleName = "رقم القيد الدوري";
        textBox6.Dock = DockStyle.Fill;
        textBox6.Location = new Point(3, 3);
        textBox6.Name = "textBox6";
        textBox6.Size = new Size(109, 22);
        textBox6.TabIndex = 19;
        // 
        // field_accountingDate
        // 
        field_accountingDate.AccessibleName = "التاريخ";
        field_accountingDate.BackColor = Color.LightYellow;
        lbl_collector.BackColor = Color.LightYellow;        field_accountingDate.Checked = false;
        field_accountingDate.CustomFormat = "yyyy-MM-dd";
        field_accountingDate.Dock = DockStyle.Fill;
        field_accountingDate.Font = new Font("Tahoma", 9.25F);
        field_accountingDate.Format = DateTimePickerFormat.Custom;
        field_accountingDate.Location = new Point(204, 32);
        field_accountingDate.Margin = new Padding(1, 2, 1, 2);
        field_accountingDate.Name = "field_accountingDate";
        field_accountingDate.ShowCheckBox = true;
        field_accountingDate.Size = new Size(228, 22);
        field_accountingDate.TabIndex = 58;
        field_accountingDate.Tag = "ACC-042:accountingDate";
        // 
        // textBox5
        // 
        textBox5.AccessibleName = "عدد المرفقات";
        textBox5.BackColor = Color.White;
        textBox5.Dock = DockStyle.Fill;
        textBox5.Font = new Font("Tahoma", 9.25F);
        textBox5.Location = new Point(555, 58);
        textBox5.Margin = new Padding(1, 2, 1, 2);
        textBox5.Name = "textBox5";
        textBox5.ReadOnly = true;
        textBox5.Size = new Size(167, 22);
        textBox5.TabIndex = 57;
        textBox5.TabStop = false;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Dock = DockStyle.Fill;
        label4.Font = new Font("Tahoma", 8.25F);
        label4.Location = new Point(724, 57);
        label4.Margin = new Padding(1);
        label4.Name = "label4";
        label4.Size = new Size(159, 24);
        label4.TabIndex = 56;
        label4.Text = "عدد المرفقات";
        label4.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_documentNumber
        // 
        field_documentNumber.AccessibleName = "رقم المستند";
        field_documentNumber.BackColor = Color.LightYellow;
        field_documentNumber.Dock = DockStyle.Fill;
        field_documentNumber.Font = new Font("Tahoma", 9.25F);
        field_documentNumber.Location = new Point(555, 32);
        field_documentNumber.Margin = new Padding(1, 2, 1, 2);
        field_documentNumber.Name = "field_documentNumber";
        field_documentNumber.ReadOnly = true;
        field_documentNumber.Size = new Size(167, 22);
        field_documentNumber.TabIndex = 55;
        field_documentNumber.TabStop = false;
        field_documentNumber.Tag = "ACC-042:documentNumber";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Dock = DockStyle.Fill;
        label3.Font = new Font("Tahoma", 8.25F);
        label3.Location = new Point(724, 31);
        label3.Margin = new Padding(1);
        label3.Name = "label3";
        label3.Size = new Size(159, 24);
        label3.TabIndex = 54;
        label3.Text = "رقم المستند";
        label3.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Dock = DockStyle.Fill;
        label2.Font = new Font("Tahoma", 8.25F);
        label2.Location = new Point(204, 135);
        label2.Margin = new Padding(1);
        label2.Name = "label2";
        label2.Size = new Size(228, 24);
        label2.TabIndex = 52;
        label2.Text = "الرقم";
        label2.TextAlign = ContentAlignment.MiddleRight;
        // 
        // comboBox5
        // 
        comboBox5.AccessibleName = "انزال البينات";
        comboBox5.BackColor = Color.LightYellow;
        referenceHeader.SetColumnSpan(comboBox5, 2);
        comboBox5.Dock = DockStyle.Fill;
        comboBox5.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBox5.DropDownWidth = 420;
        comboBox5.Font = new Font("Tahoma", 9.25F);
        comboBox5.Location = new Point(434, 136);
        comboBox5.Margin = new Padding(1, 2, 1, 2);
        comboBox5.Name = "comboBox5";
        comboBox5.Size = new Size(288, 22);
        comboBox5.TabIndex = 51;
        // 
        // field_description
        // 
        field_description.AccessibleName = "البيان";
        field_description.BackColor = Color.White;
        referenceHeader.SetColumnSpan(field_description, 3);
        field_description.Dock = DockStyle.Fill;
        field_description.Font = new Font("Tahoma", 9.25F);
        field_description.Location = new Point(204, 110);
        field_description.Margin = new Padding(1, 2, 1, 2);
        field_description.Name = "field_description";
        field_description.RightToLeft = RightToLeft.Yes;
        field_description.Size = new Size(518, 22);
        field_description.TabIndex = 46;
        field_description.Tag = "ACC-042:description";
        field_description.TextAlign = HorizontalAlignment.Right;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Dock = DockStyle.Fill;
        label1.Font = new Font("Tahoma", 8.25F);
        label1.Location = new Point(1076, 3);
        label1.Margin = new Padding(1);
        label1.Name = "label1";
        label1.Size = new Size(118, 26);
        label1.TabIndex = 0;
        label1.Text = "رقم الفرع";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // textBox1
        // 
        textBox1.AccessibleName = "رقم الفرع";
        textBox1.BackColor = Color.LightYellow;
        referenceHeader.SetColumnSpan(textBox1, 3);
        textBox1.Dock = DockStyle.Fill;
        textBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        textBox1.Enabled = false;
        textBox1.Font = new Font("Tahoma", 9.25F);
        textBox1.Location = new Point(555, 4);
        textBox1.Margin = new Padding(1, 2, 1, 2);
        textBox1.Name = "textBox1";
        textBox1.Size = new Size(519, 22);
        textBox1.TabIndex = 1;
        // 
        // lbl_destinationCashBankRef
        // 
        lbl_destinationCashBankRef.AutoSize = true;
        lbl_destinationCashBankRef.Dock = DockStyle.Fill;
        lbl_destinationCashBankRef.Font = new Font("Tahoma", 8.25F);
        lbl_destinationCashBankRef.Location = new Point(1076, 31);
        lbl_destinationCashBankRef.Margin = new Padding(1);
        lbl_destinationCashBankRef.Name = "lbl_destinationCashBankRef";
        lbl_destinationCashBankRef.Size = new Size(118, 24);
        lbl_destinationCashBankRef.TabIndex = 6;
        lbl_destinationCashBankRef.Text = "نوع السند";
        lbl_destinationCashBankRef.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_destinationCashBankRef
        // 
        field_destinationCashBankRef.AccessibleName = "نوع السند";
        field_destinationCashBankRef.BackColor = Color.LightYellow;
        field_destinationCashBankRef.Dock = DockStyle.Fill;
        field_destinationCashBankRef.DropDownStyle = ComboBoxStyle.DropDownList;
        field_destinationCashBankRef.DropDownWidth = 420;
        field_destinationCashBankRef.Font = new Font("Tahoma", 9.25F);
        field_destinationCashBankRef.Location = new Point(885, 32);
        field_destinationCashBankRef.Margin = new Padding(1, 2, 1, 2);
        field_destinationCashBankRef.Name = "field_destinationCashBankRef";
        field_destinationCashBankRef.Size = new Size(189, 22);
        field_destinationCashBankRef.TabIndex = 7;
        // 
        // lbl_collector
        // 
        lbl_collector.AutoSize = true;
        lbl_collector.Dock = DockStyle.Fill;
        lbl_collector.Font = new Font("Tahoma", 8.25F);
        lbl_collector.Location = new Point(434, 31);
        lbl_collector.Margin = new Padding(1);
        lbl_collector.Name = "lbl_collector";
        lbl_collector.Size = new Size(119, 24);
        lbl_collector.TabIndex = 9;
        lbl_collector.Text = "التاريخ";
        lbl_collector.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lbl_voucherNumber
        // 
        lbl_voucherNumber.AutoSize = true;
        lbl_voucherNumber.Dock = DockStyle.Fill;
        lbl_voucherNumber.Font = new Font("Tahoma", 8.25F);
        lbl_voucherNumber.Location = new Point(1076, 57);
        lbl_voucherNumber.Margin = new Padding(1);
        lbl_voucherNumber.Name = "lbl_voucherNumber";
        lbl_voucherNumber.Size = new Size(118, 24);
        lbl_voucherNumber.TabIndex = 13;
        lbl_voucherNumber.Text = "رقم المرجع";
        lbl_voucherNumber.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_externalReference
        // 
        field_externalReference.AccessibleName = "رقم المرجع";
        field_externalReference.BackColor = Color.White;
        field_externalReference.Dock = DockStyle.Fill;
        field_externalReference.Font = new Font("Tahoma", 9.25F);
        field_externalReference.Location = new Point(885, 58);
        field_externalReference.Margin = new Padding(1, 2, 1, 2);
        field_externalReference.Name = "field_externalReference";
        field_externalReference.Size = new Size(189, 22);
        field_externalReference.TabIndex = 14;
        field_externalReference.Tag = "ACC-042:externalReference";
        // 
        // lbl_attachmentCount
        // 
        lbl_attachmentCount.AutoSize = true;
        lbl_attachmentCount.Dock = DockStyle.Fill;
        lbl_attachmentCount.Font = new Font("Tahoma", 8.25F);
        lbl_attachmentCount.Location = new Point(434, 57);
        lbl_attachmentCount.Margin = new Padding(1);
        lbl_attachmentCount.Name = "lbl_attachmentCount";
        lbl_attachmentCount.Size = new Size(119, 24);
        lbl_attachmentCount.TabIndex = 17;
        lbl_attachmentCount.Text = "اجمالي المبلغ";
        lbl_attachmentCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_attachmentCount
        // 
        field_attachmentCount.AccessibleName = "اجمالي المبلغ";
        field_attachmentCount.BackColor = Color.White;
        field_attachmentCount.Dock = DockStyle.Fill;
        field_attachmentCount.Font = new Font("Tahoma", 9.25F);
        field_attachmentCount.Location = new Point(204, 58);
        field_attachmentCount.Margin = new Padding(1, 2, 1, 2);
        field_attachmentCount.Name = "field_attachmentCount";
        field_attachmentCount.ReadOnly = true;
        field_attachmentCount.Size = new Size(228, 22);
        field_attachmentCount.TabIndex = 18;
        field_attachmentCount.TabStop = false;
        // 
        // lbl_amount
        // 
        lbl_amount.AutoSize = true;
        lbl_amount.Dock = DockStyle.Fill;
        lbl_amount.Font = new Font("Tahoma", 8.25F);
        lbl_amount.Location = new Point(1076, 83);
        lbl_amount.Margin = new Padding(1);
        lbl_amount.Name = "lbl_amount";
        lbl_amount.Size = new Size(118, 24);
        lbl_amount.TabIndex = 21;
        lbl_amount.Text = "المستلم";
        lbl_amount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_amount
        // 
        field_amount.AccessibleName = "المبلغ";
        field_amount.BackColor = Color.White;
        referenceHeader.SetColumnSpan(field_amount, 3);
        field_amount.Dock = DockStyle.Fill;
        field_amount.Font = new Font("Tahoma", 9.25F);
        field_amount.Location = new Point(555, 84);
        field_amount.Margin = new Padding(1, 2, 1, 2);
        field_amount.Name = "field_amount";
        field_amount.RightToLeft = RightToLeft.Yes;
        field_amount.Size = new Size(519, 22);
        field_amount.TabIndex = 22;
        field_amount.TextAlign = HorizontalAlignment.Right;
        // 
        // lbl_foreignReceipt
        // 
        lbl_foreignReceipt.AutoSize = true;
        lbl_foreignReceipt.Dock = DockStyle.Fill;
        lbl_foreignReceipt.Font = new Font("Tahoma", 8.25F);
        lbl_foreignReceipt.Location = new Point(1076, 109);
        lbl_foreignReceipt.Margin = new Padding(1);
        lbl_foreignReceipt.Name = "lbl_foreignReceipt";
        lbl_foreignReceipt.Size = new Size(118, 24);
        lbl_foreignReceipt.TabIndex = 25;
        lbl_foreignReceipt.Text = "المستفيد";
        lbl_foreignReceipt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_foreignReceipt
        // 
        field_foreignReceipt.AccessibleName = "المستفيد";
        field_foreignReceipt.BackColor = Color.White;
        field_foreignReceipt.Dock = DockStyle.Fill;
        field_foreignReceipt.Font = new Font("Tahoma", 9.25F);
        field_foreignReceipt.Location = new Point(885, 110);
        field_foreignReceipt.Margin = new Padding(1, 2, 1, 2);
        field_foreignReceipt.Name = "field_foreignReceipt";
        field_foreignReceipt.ReadOnly = true;
        field_foreignReceipt.Size = new Size(189, 22);
        field_foreignReceipt.TabIndex = 26;
        field_foreignReceipt.TabStop = false;
        // 
        // lbl_partyRef
        // 
        lbl_partyRef.AutoSize = true;
        lbl_partyRef.Dock = DockStyle.Fill;
        lbl_partyRef.Font = new Font("Tahoma", 8.25F);
        lbl_partyRef.Location = new Point(724, 109);
        lbl_partyRef.Margin = new Padding(1);
        lbl_partyRef.Name = "lbl_partyRef";
        lbl_partyRef.Size = new Size(159, 24);
        lbl_partyRef.TabIndex = 29;
        lbl_partyRef.Text = "البيان";
        lbl_partyRef.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lbl_description
        // 
        lbl_description.AutoSize = true;
        lbl_description.Dock = DockStyle.Fill;
        lbl_description.Font = new Font("Tahoma", 8.25F);
        lbl_description.Location = new Point(724, 135);
        lbl_description.Margin = new Padding(1);
        lbl_description.Name = "lbl_description";
        lbl_description.Size = new Size(159, 24);
        lbl_description.TabIndex = 35;
        lbl_description.Text = "انزال البينات ";
        lbl_description.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblAmountWords
        // 
        lblAmountWords.AutoSize = true;
        lblAmountWords.BackColor = Color.FromArgb(229, 240, 222);
        referenceHeader.SetColumnSpan(lblAmountWords, 7);
        lblAmountWords.Dock = DockStyle.Fill;
        lblAmountWords.Font = new Font("Tahoma", 8.25F);
        lblAmountWords.Location = new Point(3, 161);
        lblAmountWords.Margin = new Padding(1);
        lblAmountWords.Name = "lblAmountWords";
        lblAmountWords.Size = new Size(1191, 24);
        lblAmountWords.TabIndex = 43;
        lblAmountWords.Text = "المبلغ كتابةً: —";
        lblAmountWords.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // documentAdditional
        // 
        documentAdditional.AutoScroll = true;
        documentAdditional.Controls.Add(documentAdditionalSections);
        documentAdditional.Location = new Point(4, 23);
        documentAdditional.Name = "documentAdditional";
        documentAdditional.Size = new Size(1197, 188);
        documentAdditional.TabIndex = 6;
        documentAdditional.Text = "بيانات إضافية";
        // 
        // documentAdditionalSections
        // 
        documentAdditionalSections.Controls.Add(tabPage1);
        documentAdditionalSections.Controls.Add(documentAccounts);
        documentAdditionalSections.Controls.Add(tp2);
        documentAdditionalSections.Controls.Add(tp3);
        documentAdditionalSections.Controls.Add(tp4);
        documentAdditionalSections.Dock = DockStyle.Fill;
        documentAdditionalSections.Location = new Point(0, 0);
        documentAdditionalSections.Name = "documentAdditionalSections";
        documentAdditionalSections.RightToLeft = RightToLeft.Yes;
        documentAdditionalSections.RightToLeftLayout = true;
        documentAdditionalSections.SelectedIndex = 0;
        documentAdditionalSections.Size = new Size(1197, 188);
        documentAdditionalSections.TabIndex = 0;
        // 
        // tabPage1
        // 
        tabPage1.AutoScroll = true;
        tabPage1.Controls.Add(layout0);
        tabPage1.Location = new Point(4, 23);
        tabPage1.Margin = new Padding(4);
        tabPage1.Name = "tabPage1";
        tabPage1.Size = new Size(1189, 161);
        tabPage1.TabIndex = 1;
        tabPage1.Text = "تفاصيل السند";
        // 
        // layout0
        // 
        layout0.AutoScroll = true;
        layout0.AutoSize = true;
        layout0.ColumnCount = 6;
        layout0.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.42857F));
        layout0.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 21.9047623F));
        layout0.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.4285717F));
        layout0.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 21.9047623F));
        layout0.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.4285717F));
        layout0.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 21.9047623F));
        layout0.Controls.Add(lbl_sourceCashBankRef, 0, 0);
        layout0.Controls.Add(field_sourceCashBankRef, 1, 0);
        layout0.Controls.Add(lbl_counterAccountRef, 2, 0);
        layout0.Controls.Add(field_counterAccountRef, 3, 0);
        layout0.Controls.Add(lbl_state, 4, 0);
        layout0.Controls.Add(field_state, 5, 0);
        layout0.Controls.Add(referenceLabel16, 0, 1);
        layout0.Controls.Add(referenceField16, 1, 1);
        layout0.Controls.Add(referenceLabel17, 2, 1);
        layout0.Controls.Add(referenceField17, 3, 1);
        layout0.Controls.Add(lbl_referenceType, 4, 1);
        layout0.Controls.Add(field_referenceType, 5, 1);
        layout0.Controls.Add(lblReceiptFlags, 0, 3);
        layout0.Dock = DockStyle.Top;
        layout0.Location = new Point(0, 0);
        layout0.Margin = new Padding(4);
        layout0.Name = "layout0";
        layout0.Padding = new Padding(4);
        layout0.RightToLeft = RightToLeft.Yes;
        layout0.RowCount = 4;
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.Size = new Size(1189, 107);
        layout0.TabIndex = 0;
        // 
        // lbl_sourceCashBankRef
        // 
        lbl_sourceCashBankRef.Dock = DockStyle.Fill;
        lbl_sourceCashBankRef.Location = new Point(1054, 4);
        lbl_sourceCashBankRef.Name = "lbl_sourceCashBankRef";
        lbl_sourceCashBankRef.Size = new Size(128, 36);
        lbl_sourceCashBankRef.TabIndex = 0;
        lbl_sourceCashBankRef.Text = "الصندوق/البنك المصدر";
        lbl_sourceCashBankRef.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_sourceCashBankRef
        // 
        field_sourceCashBankRef.BackColor = Color.White;
        field_sourceCashBankRef.Dock = DockStyle.Fill;
        field_sourceCashBankRef.DropDownStyle = ComboBoxStyle.DropDownList;
        field_sourceCashBankRef.Location = new Point(796, 7);
        field_sourceCashBankRef.Name = "field_sourceCashBankRef";
        field_sourceCashBankRef.Size = new Size(252, 22);
        field_sourceCashBankRef.TabIndex = 1;
        // 
        // lbl_counterAccountRef
        // 
        lbl_counterAccountRef.AutoSize = true;
        lbl_counterAccountRef.Dock = DockStyle.Fill;
        lbl_counterAccountRef.Location = new Point(663, 4);
        lbl_counterAccountRef.Margin = new Padding(4, 0, 4, 0);
        lbl_counterAccountRef.Name = "lbl_counterAccountRef";
        lbl_counterAccountRef.Size = new Size(126, 36);
        lbl_counterAccountRef.TabIndex = 2;
        lbl_counterAccountRef.Text = "الحساب المقابل";
        lbl_counterAccountRef.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_counterAccountRef
        // 
        field_counterAccountRef.AccessibleName = "الحساب المقابل";
        field_counterAccountRef.BackColor = Color.LightYellow;
        field_counterAccountRef.Dock = DockStyle.Fill;
        field_counterAccountRef.DropDownStyle = ComboBoxStyle.DropDownList;
        field_counterAccountRef.DropDownWidth = 420;
        field_counterAccountRef.Location = new Point(405, 8);
        field_counterAccountRef.Margin = new Padding(4);
        field_counterAccountRef.Name = "field_counterAccountRef";
        field_counterAccountRef.Size = new Size(250, 22);
        field_counterAccountRef.TabIndex = 3;
        // 
        // lbl_state
        // 
        lbl_state.AutoSize = true;
        lbl_state.Dock = DockStyle.Fill;
        lbl_state.Location = new Point(271, 4);
        lbl_state.Margin = new Padding(4, 0, 4, 0);
        lbl_state.Name = "lbl_state";
        lbl_state.Size = new Size(126, 36);
        lbl_state.TabIndex = 4;
        lbl_state.Text = "الحالة";
        lbl_state.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_state
        // 
        field_state.AccessibleName = "الحالة";
        field_state.Dock = DockStyle.Fill;
        field_state.Location = new Point(8, 8);
        field_state.Margin = new Padding(4);
        field_state.Name = "field_state";
        field_state.ReadOnly = true;
        field_state.Size = new Size(255, 22);
        field_state.TabIndex = 5;
        field_state.TabStop = false;
        field_state.Tag = "ACC-042:state";
        // 
        // referenceLabel16
        // 
        referenceLabel16.Dock = DockStyle.Fill;
        referenceLabel16.Location = new Point(1053, 42);
        referenceLabel16.Margin = new Padding(2);
        referenceLabel16.Name = "referenceLabel16";
        referenceLabel16.Size = new Size(130, 28);
        referenceLabel16.TabIndex = 6;
        referenceLabel16.Text = "رقم المشروع";
        referenceLabel16.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField16
        // 
        referenceField16.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField16.BackColor = Color.FromArgb(245, 245, 245);
        referenceField16.Dock = DockStyle.Fill;
        referenceField16.DropDownStyle = ComboBoxStyle.DropDownList;
        referenceField16.Location = new Point(795, 42);
        referenceField16.Margin = new Padding(2);
        referenceField16.Name = "referenceField16";
        referenceField16.Size = new Size(254, 22);
        referenceField16.TabIndex = 7;
        referenceField16.TabStop = false;
        // 
        // referenceLabel17
        // 
        referenceLabel17.Dock = DockStyle.Fill;
        referenceLabel17.Location = new Point(661, 42);
        referenceLabel17.Margin = new Padding(2);
        referenceLabel17.Name = "referenceLabel17";
        referenceLabel17.Size = new Size(130, 28);
        referenceLabel17.TabIndex = 8;
        referenceLabel17.Text = "رقم النشاط";
        referenceLabel17.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField17
        // 
        referenceField17.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField17.BackColor = Color.FromArgb(245, 245, 245);
        referenceField17.Dock = DockStyle.Fill;
        referenceField17.DropDownStyle = ComboBoxStyle.DropDownList;
        referenceField17.Location = new Point(403, 42);
        referenceField17.Margin = new Padding(2);
        referenceField17.Name = "referenceField17";
        referenceField17.Size = new Size(254, 22);
        referenceField17.TabIndex = 9;
        referenceField17.TabStop = false;
        // 
        // lbl_referenceType
        // 
        lbl_referenceType.Dock = DockStyle.Fill;
        lbl_referenceType.Location = new Point(270, 40);
        lbl_referenceType.Name = "lbl_referenceType";
        lbl_referenceType.Size = new Size(128, 32);
        lbl_referenceType.TabIndex = 10;
        lbl_referenceType.Text = "النوع";
        lbl_referenceType.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_referenceType
        // 
        field_referenceType.AccessibleName = "النوع";
        field_referenceType.Dock = DockStyle.Fill;
        field_referenceType.Location = new Point(7, 43);
        field_referenceType.Name = "field_referenceType";
        field_referenceType.ReadOnly = true;
        field_referenceType.Size = new Size(257, 22);
        field_referenceType.TabIndex = 11;
        field_referenceType.TabStop = false;
        // 
        // lblReceiptFlags
        // 
        layout0.SetColumnSpan(lblReceiptFlags, 6);
        lblReceiptFlags.Dock = DockStyle.Fill;
        lblReceiptFlags.Location = new Point(7, 72);
        lblReceiptFlags.Name = "lblReceiptFlags";
        lblReceiptFlags.Size = new Size(1175, 31);
        lblReceiptFlags.TabIndex = 12;
        lblReceiptFlags.Text = "المراجعة: —    التعليق: —";
        lblReceiptFlags.TextAlign = ContentAlignment.MiddleRight;
        // 
        // documentAccounts
        // 
        documentAccounts.AutoScroll = true;
        documentAccounts.Controls.Add(documentAccountsGrid);
        documentAccounts.Location = new Point(4, 23);
        documentAccounts.Name = "documentAccounts";
        documentAccounts.Size = new Size(1189, 161);
        documentAccounts.TabIndex = 8;
        documentAccounts.Text = "الحسابات";
        // 
        // documentAccountsGrid
        // 
        documentAccountsGrid.AccessibleDescription = "الحسابات المتعددة؛ ربط خدمة السند والصلاحيات غير مكتمل";
        documentAccountsGrid.AllowUserToAddRows = false;
        documentAccountsGrid.AllowUserToDeleteRows = false;
        documentAccountsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        documentAccountsGrid.BackgroundColor = Color.White;
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(232, 232, 232);
        dataGridViewCellStyle1.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
        documentAccountsGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        documentAccountsGrid.ColumnHeadersHeight = 29;
        documentAccountsGrid.Columns.AddRange(new DataGridViewColumn[] { documentAccountsGridColumn0, documentAccountsGridColumn1, documentAccountsGridColumn2, documentAccountsGridColumn3, documentAccountsGridColumn4, documentAccountsGridColumn5, documentAccountsGridColumn6, documentAccountsGridColumn7 });
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = Color.White;
        dataGridViewCellStyle2.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
        documentAccountsGrid.DefaultCellStyle = dataGridViewCellStyle2;
        documentAccountsGrid.Dock = DockStyle.Fill;
        documentAccountsGrid.EnableHeadersVisualStyles = false;
        documentAccountsGrid.Location = new Point(0, 0);
        documentAccountsGrid.Name = "documentAccountsGrid";
        documentAccountsGrid.ReadOnly = true;
        documentAccountsGrid.RightToLeft = RightToLeft.Yes;
        documentAccountsGrid.RowHeadersWidth = 51;
        documentAccountsGrid.Size = new Size(1189, 161);
        documentAccountsGrid.TabIndex = 0;
        // 
        // documentAccountsGridColumn0
        // 
        documentAccountsGridColumn0.HeaderText = "رقم الحساب";
        documentAccountsGridColumn0.MinimumWidth = 6;
        documentAccountsGridColumn0.Name = "documentAccountsGridColumn0";
        documentAccountsGridColumn0.ReadOnly = true;
        // 
        // documentAccountsGridColumn1
        // 
        documentAccountsGridColumn1.HeaderText = "الحساب التحليلي";
        documentAccountsGridColumn1.MinimumWidth = 6;
        documentAccountsGridColumn1.Name = "documentAccountsGridColumn1";
        documentAccountsGridColumn1.ReadOnly = true;
        // 
        // documentAccountsGridColumn2
        // 
        documentAccountsGridColumn2.HeaderText = "اسم الحساب";
        documentAccountsGridColumn2.MinimumWidth = 6;
        documentAccountsGridColumn2.Name = "documentAccountsGridColumn2";
        documentAccountsGridColumn2.ReadOnly = true;
        // 
        // documentAccountsGridColumn3
        // 
        documentAccountsGridColumn3.HeaderText = "البيان";
        documentAccountsGridColumn3.MinimumWidth = 6;
        documentAccountsGridColumn3.Name = "documentAccountsGridColumn3";
        documentAccountsGridColumn3.ReadOnly = true;
        // 
        // documentAccountsGridColumn4
        // 
        documentAccountsGridColumn4.HeaderText = "العملة";
        documentAccountsGridColumn4.MinimumWidth = 6;
        documentAccountsGridColumn4.Name = "documentAccountsGridColumn4";
        documentAccountsGridColumn4.ReadOnly = true;
        // 
        // documentAccountsGridColumn5
        // 
        documentAccountsGridColumn5.HeaderText = "سعر التحويل";
        documentAccountsGridColumn5.MinimumWidth = 6;
        documentAccountsGridColumn5.Name = "documentAccountsGridColumn5";
        documentAccountsGridColumn5.ReadOnly = true;
        // 
        // documentAccountsGridColumn6
        // 
        documentAccountsGridColumn6.HeaderText = "المبلغ المحلي";
        documentAccountsGridColumn6.MinimumWidth = 6;
        documentAccountsGridColumn6.Name = "documentAccountsGridColumn6";
        documentAccountsGridColumn6.ReadOnly = true;
        // 
        // documentAccountsGridColumn7
        // 
        documentAccountsGridColumn7.HeaderText = "المبلغ الأجنبي";
        documentAccountsGridColumn7.MinimumWidth = 6;
        documentAccountsGridColumn7.Name = "documentAccountsGridColumn7";
        documentAccountsGridColumn7.ReadOnly = true;
        // 
        // tp2
        // 
        tp2.AutoScroll = true;
        tp2.Controls.Add(layout2);
        tp2.Location = new Point(4, 23);
        tp2.Margin = new Padding(4);
        tp2.Name = "tp2";
        tp2.Size = new Size(1189, 161);
        tp2.TabIndex = 2;
        tp2.Text = "المرفقات والربط بالمستندات";
        // 
        // layout2
        // 
        layout2.AutoScroll = true;
        layout2.AutoSize = true;
        layout2.ColumnCount = 2;
        layout2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 244F));
        layout2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout2.Controls.Add(context2, 0, 0);
        layout2.Dock = DockStyle.Fill;
        layout2.Location = new Point(0, 0);
        layout2.Margin = new Padding(4);
        layout2.MinimumSize = new Size(812, 0);
        layout2.Name = "layout2";
        layout2.RowCount = 1;
        layout2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout2.Size = new Size(1189, 161);
        layout2.TabIndex = 0;
        // 
        // context2
        // 
        context2.AccessibleName = "المرفقات والربط بالمستندات";
        layout2.SetColumnSpan(context2, 2);
        context2.DetectUrls = false;
        context2.Dock = DockStyle.Fill;
        context2.Location = new Point(4, 4);
        context2.Margin = new Padding(4);
        context2.Name = "context2";
        context2.ReadOnly = true;
        context2.Size = new Size(1181, 153);
        context2.TabIndex = 0;
        context2.TabStop = false;
        context2.Text = "";
        // 
        // tp3
        // 
        tp3.AutoScroll = true;
        tp3.Controls.Add(layout3);
        tp3.Location = new Point(4, 23);
        tp3.Margin = new Padding(4);
        tp3.Name = "tp3";
        tp3.Size = new Size(1189, 161);
        tp3.TabIndex = 3;
        tp3.Text = "الاعتمادات";
        // 
        // layout3
        // 
        layout3.AutoScroll = true;
        layout3.AutoSize = true;
        layout3.ColumnCount = 2;
        layout3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 244F));
        layout3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout3.Controls.Add(context3, 0, 0);
        layout3.Controls.Add(btnReceiptApprove, 0, 1);
        layout3.Dock = DockStyle.Fill;
        layout3.Location = new Point(0, 0);
        layout3.Margin = new Padding(4);
        layout3.MinimumSize = new Size(812, 0);
        layout3.Name = "layout3";
        layout3.RowCount = 2;
        layout3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout3.RowStyles.Add(new RowStyle());
        layout3.Size = new Size(1189, 161);
        layout3.TabIndex = 0;
        // 
        // context3
        // 
        context3.AccessibleName = "الاعتمادات";
        layout3.SetColumnSpan(context3, 2);
        context3.DetectUrls = false;
        context3.Dock = DockStyle.Fill;
        context3.Location = new Point(4, 4);
        context3.Margin = new Padding(4);
        context3.Name = "context3";
        context3.ReadOnly = true;
        context3.Size = new Size(1181, 122);
        context3.TabIndex = 0;
        context3.TabStop = false;
        context3.Text = "";
        // 
        // btnReceiptApprove
        // 
        btnReceiptApprove.AutoSize = true;
        layout3.SetColumnSpan(btnReceiptApprove, 2);
        btnReceiptApprove.Dock = DockStyle.Bottom;
        btnReceiptApprove.Enabled = false;
        btnReceiptApprove.Location = new Point(3, 133);
        btnReceiptApprove.Name = "btnReceiptApprove";
        btnReceiptApprove.Size = new Size(1183, 25);
        btnReceiptApprove.TabIndex = 1;
        btnReceiptApprove.Text = "اعتماد سند القبض";
        btnReceiptApprove.Visible = false;
        // 
        // tp4
        // 
        tp4.AutoScroll = true;
        tp4.Controls.Add(layout4);
        tp4.Location = new Point(4, 23);
        tp4.Margin = new Padding(4);
        tp4.Name = "tp4";
        tp4.Size = new Size(1189, 161);
        tp4.TabIndex = 4;
        tp4.Text = "سجل العمليات";
        // 
        // layout4
        // 
        layout4.AutoScroll = true;
        layout4.AutoSize = true;
        layout4.ColumnCount = 2;
        layout4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 244F));
        layout4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout4.Controls.Add(context4, 0, 0);
        layout4.Dock = DockStyle.Fill;
        layout4.Location = new Point(0, 0);
        layout4.Margin = new Padding(4);
        layout4.MinimumSize = new Size(812, 0);
        layout4.Name = "layout4";
        layout4.RowCount = 1;
        layout4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout4.Size = new Size(1189, 161);
        layout4.TabIndex = 0;
        // 
        // context4
        // 
        context4.AccessibleName = "سجل العمليات";
        layout4.SetColumnSpan(context4, 2);
        context4.DetectUrls = false;
        context4.Dock = DockStyle.Fill;
        context4.Location = new Point(4, 4);
        context4.Margin = new Padding(4);
        context4.Name = "context4";
        context4.ReadOnly = true;
        context4.Size = new Size(1181, 153);
        context4.TabIndex = 0;
        context4.TabStop = false;
        context4.Text = "";
        // 
        // documentDefaults
        // 
        documentDefaults.AutoScroll = true;
        documentDefaults.Controls.Add(documentDefaultsLayout);
        documentDefaults.Controls.Add(btnReceiptSettings);
        documentDefaults.Location = new Point(4, 23);
        documentDefaults.Name = "documentDefaults";
        documentDefaults.Size = new Size(1197, 188);
        documentDefaults.TabIndex = 7;
        documentDefaults.Text = "البيانات الافتراضية";
        // 
        // documentDefaultsLayout
        // 
        documentDefaultsLayout.AutoSize = true;
        documentDefaultsLayout.ColumnCount = 2;
        documentDefaultsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
        documentDefaultsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        documentDefaultsLayout.Controls.Add(lblDefaultCurrency, 0, 0);
        documentDefaultsLayout.Controls.Add(cboDefaultCurrency, 1, 0);
        documentDefaultsLayout.Controls.Add(lblDefaultCostCenter, 0, 1);
        documentDefaultsLayout.Controls.Add(cboDefaultCostCenter, 1, 1);
        documentDefaultsLayout.Dock = DockStyle.Top;
        documentDefaultsLayout.Location = new Point(0, 0);
        documentDefaultsLayout.Name = "documentDefaultsLayout";
        documentDefaultsLayout.Padding = new Padding(12);
        documentDefaultsLayout.RowCount = 2;
        documentDefaultsLayout.RowStyles.Add(new RowStyle());
        documentDefaultsLayout.RowStyles.Add(new RowStyle());
        documentDefaultsLayout.Size = new Size(1197, 80);
        documentDefaultsLayout.TabIndex = 0;
        // 
        // lblDefaultCurrency
        // 
        lblDefaultCurrency.AutoSize = true;
        lblDefaultCurrency.Location = new Point(1145, 12);
        lblDefaultCurrency.Name = "lblDefaultCurrency";
        lblDefaultCurrency.Size = new Size(37, 14);
        lblDefaultCurrency.TabIndex = 0;
        lblDefaultCurrency.Text = "العملة";
        // 
        // cboDefaultCurrency
        // 
        cboDefaultCurrency.AccessibleDescription = "تحتاج ربط قوائم القيم وتطبيقها على التفاصيل";
        cboDefaultCurrency.Dock = DockStyle.Fill;
        cboDefaultCurrency.DropDownStyle = ComboBoxStyle.DropDownList;
        cboDefaultCurrency.Enabled = false;
        cboDefaultCurrency.Location = new Point(15, 15);
        cboDefaultCurrency.Name = "cboDefaultCurrency";
        cboDefaultCurrency.Size = new Size(1027, 22);
        cboDefaultCurrency.TabIndex = 1;
        // 
        // lblDefaultCostCenter
        // 
        lblDefaultCostCenter.AutoSize = true;
        lblDefaultCostCenter.Location = new Point(1116, 40);
        lblDefaultCostCenter.Name = "lblDefaultCostCenter";
        lblDefaultCostCenter.Size = new Size(66, 14);
        lblDefaultCostCenter.TabIndex = 2;
        lblDefaultCostCenter.Text = "مركز التكلفة";
        // 
        // cboDefaultCostCenter
        // 
        cboDefaultCostCenter.AccessibleDescription = "تحتاج ربط قوائم القيم وتطبيقها على التفاصيل";
        cboDefaultCostCenter.Dock = DockStyle.Fill;
        cboDefaultCostCenter.DropDownStyle = ComboBoxStyle.DropDownList;
        cboDefaultCostCenter.Enabled = false;
        cboDefaultCostCenter.Location = new Point(15, 43);
        cboDefaultCostCenter.Name = "cboDefaultCostCenter";
        cboDefaultCostCenter.Size = new Size(1027, 22);
        cboDefaultCostCenter.TabIndex = 3;
        // 
        // btnReceiptSettings
        // 
        btnReceiptSettings.AutoSize = true;
        btnReceiptSettings.Dock = DockStyle.Bottom;
        btnReceiptSettings.Enabled = false;
        btnReceiptSettings.Location = new Point(0, 163);
        btnReceiptSettings.Name = "btnReceiptSettings";
        btnReceiptSettings.Size = new Size(1197, 25);
        btnReceiptSettings.TabIndex = 1;
        btnReceiptSettings.Text = "إعدادات سند القبض";
        btnReceiptSettings.Visible = false;
        // 
        // documentImport
        // 
        documentImport.AutoScroll = true;
        documentImport.Controls.Add(documentImportLayout);
        documentImport.Location = new Point(4, 23);
        documentImport.Name = "documentImport";
        documentImport.Size = new Size(1197, 188);
        documentImport.TabIndex = 9;
        documentImport.Text = "استيراد من ملف";
        // 
        // documentImportLayout
        // 
        documentImportLayout.ColumnCount = 1;
        documentImportLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        documentImportLayout.Controls.Add(documentImportFormat, 0, 0);
        documentImportLayout.Controls.Add(documentImportButtons, 0, 1);
        documentImportLayout.Controls.Add(documentImportPreview, 0, 2);
        documentImportLayout.Dock = DockStyle.Fill;
        documentImportLayout.Location = new Point(0, 0);
        documentImportLayout.Name = "documentImportLayout";
        documentImportLayout.RowCount = 3;
        documentImportLayout.RowStyles.Add(new RowStyle());
        documentImportLayout.RowStyles.Add(new RowStyle());
        documentImportLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        documentImportLayout.Size = new Size(1197, 188);
        documentImportLayout.TabIndex = 0;
        // 
        // documentImportFormat
        // 
        documentImportFormat.Dock = DockStyle.Fill;
        documentImportFormat.Location = new Point(3, 0);
        documentImportFormat.Name = "documentImportFormat";
        documentImportFormat.Size = new Size(1191, 28);
        documentImportFormat.TabIndex = 0;
        documentImportFormat.Text = "نوع الملف: Excel (.xlsx) أو CSV (UTF-8)";
        // 
        // documentImportButtons
        // 
        documentImportButtons.AutoSize = true;
        documentImportButtons.Controls.Add(documentImportPath);
        documentImportButtons.Controls.Add(documentImportChoose);
        documentImportButtons.Controls.Add(documentImportApply);
        documentImportButtons.Dock = DockStyle.Fill;
        documentImportButtons.Location = new Point(3, 31);
        documentImportButtons.Name = "documentImportButtons";
        documentImportButtons.Size = new Size(1191, 36);
        documentImportButtons.TabIndex = 1;
        // 
        // documentImportPath
        // 
        documentImportPath.Location = new Point(828, 3);
        documentImportPath.Name = "documentImportPath";
        documentImportPath.ReadOnly = true;
        documentImportPath.Size = new Size(360, 22);
        documentImportPath.TabIndex = 0;
        documentImportPath.TabStop = false;
        // 
        // documentImportChoose
        // 
        documentImportChoose.AutoSize = true;
        documentImportChoose.Location = new Point(685, 3);
        documentImportChoose.Name = "documentImportChoose";
        documentImportChoose.Size = new Size(137, 30);
        documentImportChoose.TabIndex = 1;
        documentImportChoose.Text = "اختيار ملف ومعاينة";
        // 
        // documentImportApply
        // 
        documentImportApply.AutoSize = true;
        documentImportApply.Location = new Point(536, 3);
        documentImportApply.Name = "documentImportApply";
        documentImportApply.Size = new Size(143, 30);
        documentImportApply.TabIndex = 2;
        documentImportApply.Text = "إضافة إلى المسودة";
        // 
        // documentImportPreview
        // 
        documentImportPreview.AllowUserToAddRows = false;
        documentImportPreview.BackgroundColor = Color.White;
        documentImportPreview.ColumnHeadersHeight = 29;
        documentImportPreview.Dock = DockStyle.Fill;
        documentImportPreview.Location = new Point(3, 73);
        documentImportPreview.Name = "documentImportPreview";
        documentImportPreview.ReadOnly = true;
        documentImportPreview.RowHeadersWidth = 51;
        documentImportPreview.Size = new Size(1191, 112);
        documentImportPreview.TabIndex = 2;
        // 
        // dgvLines
        // 
        dgvLines.AccessibleName = "التفاصيل والحركات";
        dgvLines.AllowUserToAddRows = false;
        dgvLines.AllowUserToDeleteRows = false;
        dataGridViewCellStyle3.BackColor = Color.Empty;
        dgvLines.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
        dgvLines.BackgroundColor = Color.White;
        dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle4.BackColor = Color.FromArgb(232, 232, 232);
        dataGridViewCellStyle4.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
        dgvLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
        dgvLines.ColumnHeadersHeight = 34;
        dgvLines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;        dgvLines.Columns.AddRange(new DataGridViewColumn[] { col_accountRef, col_analyticalAccount, col_accountName, col_lineDescription, col_currencyRef, col_exchangeRate, col_foreignDebit, col_foreignCredit, col_rowNo, col_costCenterRef, col_debit, col_credit, col_accountingAmount, col_approvalNumber, col_salesperson, col_collector, col_referenceNumber });
        dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle10.BackColor = Color.White;
        dataGridViewCellStyle10.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle10.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle10.WrapMode = DataGridViewTriState.False;
        dgvLines.DefaultCellStyle = dataGridViewCellStyle10;
        dgvLines.Dock = DockStyle.Fill;
        dgvLines.EnableHeadersVisualStyles = false;
        dgvLines.GridColor = Color.FromArgb(207, 216, 224);
        dgvLines.Location = new Point(11, 328);
        dgvLines.MinimumSize = new Size(900, 80);
        dgvLines.MultiSelect = false;
        dgvLines.Name = "dgvLines";
        dgvLines.RightToLeft = RightToLeft.Yes;
        dgvLines.RowHeadersVisible = false;
        dgvLines.RowHeadersWidth = 51;
        dgvLines.RowTemplate.Height = 27;
        dgvLines.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvLines.Size = new Size(1203, 377);
        dgvLines.TabIndex = 5;
        // 
        // col_accountRef
        // 
        dataGridViewCellStyle5.BackColor = Color.LightYellow;
        col_accountRef.DefaultCellStyle = dataGridViewCellStyle5;
        col_accountRef.FlatStyle = FlatStyle.Flat;        col_accountRef.HeaderText = "رقم الحساب";
        col_accountRef.MinimumWidth = 90;
        col_accountRef.Name = "col_accountRef";
        col_accountRef.Width = 140;
        // 
        // col_analyticalAccount
        // 
        col_analyticalAccount.HeaderText = "الحساب التحليلي";
        col_analyticalAccount.MinimumWidth = 80;
        col_analyticalAccount.Name = "col_analyticalAccount";
        col_analyticalAccount.ReadOnly = true;
        col_analyticalAccount.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_analyticalAccount.ToolTipText = "للعرض؛ يحتاج ربط بيانات القيد";
        col_analyticalAccount.Width = 120;
        // 
        // col_accountName
        // 
        col_accountName.HeaderText = "اسم الحساب";
        col_accountName.MinimumWidth = 80;
        col_accountName.Name = "col_accountName";
        col_accountName.ReadOnly = true;
        col_accountName.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_accountName.ToolTipText = "للعرض؛ يحتاج ربط بيانات القيد";
        col_accountName.Width = 180;
        // 
        // col_lineDescription
        // 
        col_lineDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        col_lineDescription.HeaderText = "البيان";
        col_lineDescription.MinimumWidth = 90;
        col_lineDescription.Name = "col_lineDescription";
        col_lineDescription.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_lineDescription.Width = 190;
        // 
        // col_currencyRef
        // 
        col_currencyRef.HeaderText = "العملة";
        col_currencyRef.MinimumWidth = 90;
        col_currencyRef.Name = "col_currencyRef";
        col_currencyRef.Width = 140;
        // 
        // col_exchangeRate
        // 
        dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
        col_exchangeRate.DefaultCellStyle = dataGridViewCellStyle6;
        col_exchangeRate.HeaderText = "سعر الصرف";
        col_exchangeRate.MinimumWidth = 90;
        col_exchangeRate.Name = "col_exchangeRate";
        col_exchangeRate.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_exchangeRate.Width = 140;
        // 
        // col_foreignDebit
        // 
        col_foreignDebit.HeaderText = "مدين أجنبي";
        col_foreignDebit.MinimumWidth = 80;
        col_foreignDebit.Name = "col_foreignDebit";
        col_foreignDebit.ReadOnly = true;
        col_foreignDebit.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_foreignDebit.ToolTipText = "للعرض؛ يحتاج ربط بيانات القيد";
        col_foreignDebit.Width = 110;
        // 
        // col_foreignCredit
        // 
        col_foreignCredit.HeaderText = "دائن أجنبي";
        col_foreignCredit.MinimumWidth = 80;
        col_foreignCredit.Name = "col_foreignCredit";
        col_foreignCredit.ReadOnly = true;
        col_foreignCredit.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_foreignCredit.ToolTipText = "للعرض؛ يحتاج ربط بيانات القيد";
        col_foreignCredit.Width = 110;
        // 
        // col_rowNo
        // 
        col_rowNo.HeaderText = "م";
        col_rowNo.MinimumWidth = 36;
        col_rowNo.Name = "col_rowNo";
        col_rowNo.ReadOnly = true;
        col_rowNo.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_rowNo.Width = 36;
        // 
        // col_costCenterRef
        // 
        col_costCenterRef.HeaderText = "مركز التكلفة/الأبعاد";
        col_costCenterRef.MinimumWidth = 90;
        col_costCenterRef.Name = "col_costCenterRef";
        col_costCenterRef.Width = 140;
        // 
        // col_debit
        // 
        dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
        col_debit.DefaultCellStyle = dataGridViewCellStyle7;
        col_debit.HeaderText = "مدين";
        col_debit.MinimumWidth = 90;
        col_debit.Name = "col_debit";
        col_debit.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_debit.Width = 140;
        // 
        // col_credit
        // 
        dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight;
        col_credit.DefaultCellStyle = dataGridViewCellStyle8;
        col_credit.HeaderText = "دائن";
        col_credit.MinimumWidth = 90;
        col_credit.Name = "col_credit";
        col_credit.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_credit.Width = 140;
        // 
        // col_accountingAmount
        // 
        dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight;
        col_accountingAmount.DefaultCellStyle = dataGridViewCellStyle9;
        col_accountingAmount.HeaderText = "المبلغ المحاسبي";
        col_accountingAmount.MinimumWidth = 90;
        col_accountingAmount.Name = "col_accountingAmount";
        col_accountingAmount.ReadOnly = true;
        col_accountingAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_accountingAmount.Width = 140;
        // 
        // col_approvalNumber
        // 
        col_approvalNumber.HeaderText = "رقم الاعتماد";
        col_approvalNumber.MinimumWidth = 80;
        col_approvalNumber.Name = "col_approvalNumber";
        col_approvalNumber.ReadOnly = true;
        col_approvalNumber.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_approvalNumber.ToolTipText = "للعرض؛ يحتاج ربط بيانات القيد";
        col_approvalNumber.Width = 110;
        // 
        // col_salesperson
        // 
        col_salesperson.HeaderText = "المندوب";
        col_salesperson.MinimumWidth = 80;
        col_salesperson.Name = "col_salesperson";
        col_salesperson.ReadOnly = true;
        col_salesperson.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_salesperson.ToolTipText = "للعرض؛ يحتاج ربط بيانات القيد";
        col_salesperson.Width = 125;
        // 
        // col_collector
        // 
        col_collector.HeaderText = "رقم المحصل";
        col_collector.MinimumWidth = 80;
        col_collector.Name = "col_collector";
        col_collector.ReadOnly = true;
        col_collector.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_collector.ToolTipText = "للعرض؛ يحتاج ربط بيانات القيد";
        col_collector.Width = 110;
        // 
        // col_referenceNumber
        // 
        col_referenceNumber.HeaderText = "رقم المرجع";
        col_referenceNumber.MinimumWidth = 80;
        col_referenceNumber.Name = "col_referenceNumber";
        col_referenceNumber.ReadOnly = true;
        col_referenceNumber.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_referenceNumber.ToolTipText = "للعرض؛ يحتاج ربط بيانات القيد";
        col_referenceNumber.Width = 110;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.BackColor = Color.FromArgb(232, 232, 242);
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(35, 35, 50);
        lblTitle.Location = new Point(8, 8);
        lblTitle.Margin = new Padding(0);
        lblTitle.Name = "lblTitle";
        lblTitle.Padding = new Padding(8);
        lblTitle.Size = new Size(1209, 34);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "قيود اليومية";
        lblTitle.TextAlign = ContentAlignment.MiddleRight;
        // 
        // designerCommandBar
        // 
        designerCommandBar.BackColor = Color.FromArgb(239, 239, 239);
        designerCommandBar.BorderStyle = BorderStyle.FixedSingle;
        designerCommandBar.Controls.Add(flpActions);
        designerCommandBar.Controls.Add(designerHiddenCommands);
        designerCommandBar.Dock = DockStyle.Fill;
        designerCommandBar.Location = new Point(11, 45);
        designerCommandBar.Name = "designerCommandBar";
        designerCommandBar.RightToLeft = RightToLeft.Yes;
        designerCommandBar.Size = new Size(1203, 58);
        designerCommandBar.TabIndex = 1;
        // 
        // flpActions
        // 
        flpActions.AutoScroll = true;
        flpActions.BackColor = Color.FromArgb(239, 239, 244);
        flpActions.Controls.Add(btnClear);
        flpActions.Controls.Add(btnView);
        flpActions.Controls.Add(btnCreate);
        flpActions.Controls.Add(btnEdit);
        flpActions.Controls.Add(btnCancel);
        flpActions.Controls.Add(btnPrint);
        flpActions.Controls.Add(btnPost);
        flpActions.Controls.Add(btnReverse);
        flpActions.Controls.Add(btnClose);
        flpActions.Controls.Add(btnAddRow);
        flpActions.Controls.Add(btnRemoveRow);
        flpActions.Dock = DockStyle.Fill;
        flpActions.Location = new Point(0, 0);
        flpActions.Margin = new Padding(0);
        flpActions.Name = "flpActions";
        flpActions.RightToLeft = RightToLeft.Yes;
        flpActions.Size = new Size(1201, 56);
        flpActions.TabIndex = 1;
        flpActions.WrapContents = false;
        // 
        // btnClear
        // 
        btnClear.AccessibleName = "تفريغ المسودة";
        btnClear.AutoSize = true;
        btnClear.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        designerCommandBar.SetCommandRole(btnClear, Desktop.CoreUI.DesignerCommandRole.Add);
        btnClear.Location = new Point(1110, 3);
        btnClear.MinimumSize = new Size(88, 36);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(88, 36);
        btnClear.TabIndex = 6;
        btnClear.Text = "إضافة";
        // 
        // btnView
        // 
        btnView.AccessibleName = "عرض / إعادة تحميل";
        btnView.AutoSize = true;
        btnView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        designerCommandBar.SetCommandRole(btnView, Desktop.CoreUI.DesignerCommandRole.View);
        btnView.Enabled = false;
        btnView.Location = new Point(1016, 3);
        btnView.MinimumSize = new Size(88, 36);
        btnView.Name = "btnView";
        btnView.Size = new Size(88, 36);
        btnView.TabIndex = 0;
        btnView.Text = "بحث / عرض";
        // 
        // btnCreate
        // 
        btnCreate.AccessibleName = "حفظ جديد";
        btnCreate.AutoSize = true;
        btnCreate.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        designerCommandBar.SetCommandRole(btnCreate, Desktop.CoreUI.DesignerCommandRole.Save);
        btnCreate.Enabled = false;
        btnCreate.Location = new Point(922, 3);
        btnCreate.MinimumSize = new Size(88, 36);
        btnCreate.Name = "btnCreate";
        btnCreate.Size = new Size(88, 36);
        btnCreate.TabIndex = 1;
        btnCreate.Text = "حفظ";
        // 
        // btnEdit
        // 
        btnEdit.AccessibleName = "حفظ التعديل";
        btnEdit.AutoSize = true;
        btnEdit.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        designerCommandBar.SetCommandRole(btnEdit, Desktop.CoreUI.DesignerCommandRole.Edit);
        btnEdit.Enabled = false;
        btnEdit.Location = new Point(828, 3);
        btnEdit.MinimumSize = new Size(88, 36);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(88, 36);
        btnEdit.TabIndex = 2;
        btnEdit.Text = "حفظ التعديل";
        // 
        // btnCancel
        // 
        btnCancel.AccessibleName = "إلغاء المستند";
        btnCancel.AutoSize = true;
        btnCancel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        designerCommandBar.SetCommandRole(btnCancel, Desktop.CoreUI.DesignerCommandRole.Cancel);
        btnCancel.Enabled = false;
        btnCancel.Location = new Point(734, 3);
        btnCancel.MinimumSize = new Size(88, 36);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(88, 36);
        btnCancel.TabIndex = 3;
        btnCancel.Text = "إلغاء المستند";
        // 
        // btnPrint
        // 
        btnPrint.AutoSize = true;
        btnPrint.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        designerCommandBar.SetCommandRole(btnPrint, Desktop.CoreUI.DesignerCommandRole.Print);
        btnPrint.Enabled = false;
        btnPrint.Location = new Point(640, 3);
        btnPrint.MinimumSize = new Size(88, 36);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(88, 36);
        btnPrint.TabIndex = 7;
        btnPrint.Text = "طباعة";
        journalHints.SetToolTip(btnPrint, "يلزم ربط قالب طباعة سند القيد");
        // 
        // btnPost
        // 
        btnPost.AccessibleName = "ترحيل";
        btnPost.AutoSize = true;
        btnPost.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnPost.Enabled = false;
        btnPost.Location = new Point(546, 3);
        btnPost.MinimumSize = new Size(88, 36);
        btnPost.Name = "btnPost";
        btnPost.Size = new Size(88, 36);
        btnPost.TabIndex = 4;
        btnPost.Text = "ترحيل";
        // 
        // btnReverse
        // 
        btnReverse.AccessibleName = "عكس القيد";
        btnReverse.AutoSize = true;
        btnReverse.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnReverse.Enabled = false;
        btnReverse.Location = new Point(452, 3);
        btnReverse.MinimumSize = new Size(88, 36);
        btnReverse.Name = "btnReverse";
        btnReverse.Size = new Size(88, 36);
        btnReverse.TabIndex = 5;
        btnReverse.Text = "عكس القيد";
        // 
        // btnClose
        // 
        btnClose.AccessibleName = "إغلاق";
        btnClose.AutoSize = true;
        btnClose.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        designerCommandBar.SetCommandRole(btnClose, Desktop.CoreUI.DesignerCommandRole.Close);
        btnClose.Location = new Point(358, 3);
        btnClose.MinimumSize = new Size(88, 36);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(88, 36);
        btnClose.TabIndex = 7;
        btnClose.Text = "إغلاق";
        // 
        // btnAddRow
        // 
        btnAddRow.AutoSize = true;
        btnAddRow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnAddRow.Location = new Point(264, 3);
        btnAddRow.MinimumSize = new Size(88, 36);
        btnAddRow.Name = "btnAddRow";
        btnAddRow.Size = new Size(88, 36);
        btnAddRow.TabIndex = 8;
        btnAddRow.Text = "إضافة سطر";
        // 
        // btnRemoveRow
        // 
        btnRemoveRow.AutoSize = true;
        btnRemoveRow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnRemoveRow.Location = new Point(170, 3);
        btnRemoveRow.MinimumSize = new Size(88, 36);
        btnRemoveRow.Name = "btnRemoveRow";
        btnRemoveRow.Size = new Size(88, 36);
        btnRemoveRow.TabIndex = 9;
        btnRemoveRow.Text = "حذف سطر";
        // 
        // designerHiddenCommands
        // 
        designerHiddenCommands.Controls.Add(standardCommandDelete);
        designerHiddenCommands.Controls.Add(standardCommandLast);
        designerHiddenCommands.Controls.Add(standardCommandNext);
        designerHiddenCommands.Controls.Add(standardCommandPrevious);
        designerHiddenCommands.Controls.Add(standardCommandFirst);
        designerHiddenCommands.Controls.Add(standardCommandRefresh);
        designerHiddenCommands.Controls.Add(standardCommandImport);
        designerHiddenCommands.Controls.Add(standardCommandExport);
        designerHiddenCommands.Controls.Add(standardCommandHelp);
        designerHiddenCommands.Location = new Point(0, 0);
        designerHiddenCommands.Name = "designerHiddenCommands";
        designerHiddenCommands.Size = new Size(200, 100);
        designerHiddenCommands.TabIndex = 2;
        designerHiddenCommands.Visible = false;
        // 
        // standardCommandDelete
        // 
        standardCommandDelete.AccessibleName = "حذف";
        designerCommandBar.SetCommandRole(standardCommandDelete, Desktop.CoreUI.DesignerCommandRole.Delete);
        standardCommandDelete.Enabled = false;
        standardCommandDelete.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandDelete.FlatStyle = FlatStyle.Flat;
        standardCommandDelete.Image = (Image)resources.GetObject("standardCommandDelete.Image");
        standardCommandDelete.Location = new Point(173, 1);
        standardCommandDelete.Margin = new Padding(1);
        standardCommandDelete.Name = "standardCommandDelete";
        standardCommandDelete.Size = new Size(26, 24);
        standardCommandDelete.TabIndex = 0;
        standardCommandDelete.Visible = false;
        // 
        // standardCommandLast
        // 
        standardCommandLast.AccessibleName = "الأخير";
        designerCommandBar.SetCommandRole(standardCommandLast, Desktop.CoreUI.DesignerCommandRole.Last);
        standardCommandLast.Enabled = false;
        standardCommandLast.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandLast.FlatStyle = FlatStyle.Flat;
        standardCommandLast.Image = (Image)resources.GetObject("standardCommandLast.Image");
        standardCommandLast.Location = new Point(145, 1);
        standardCommandLast.Margin = new Padding(1);
        standardCommandLast.Name = "standardCommandLast";
        standardCommandLast.Size = new Size(26, 24);
        standardCommandLast.TabIndex = 1;
        standardCommandLast.Visible = false;
        // 
        // standardCommandNext
        // 
        standardCommandNext.AccessibleName = "التالي";
        designerCommandBar.SetCommandRole(standardCommandNext, Desktop.CoreUI.DesignerCommandRole.Next);
        standardCommandNext.Enabled = false;
        standardCommandNext.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandNext.FlatStyle = FlatStyle.Flat;
        standardCommandNext.Image = (Image)resources.GetObject("standardCommandNext.Image");
        standardCommandNext.Location = new Point(117, 1);
        standardCommandNext.Margin = new Padding(1);
        standardCommandNext.Name = "standardCommandNext";
        standardCommandNext.Size = new Size(26, 24);
        standardCommandNext.TabIndex = 2;
        standardCommandNext.Visible = false;
        // 
        // standardCommandPrevious
        // 
        standardCommandPrevious.AccessibleName = "السابق";
        designerCommandBar.SetCommandRole(standardCommandPrevious, Desktop.CoreUI.DesignerCommandRole.Previous);
        standardCommandPrevious.Enabled = false;
        standardCommandPrevious.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandPrevious.FlatStyle = FlatStyle.Flat;
        standardCommandPrevious.Image = (Image)resources.GetObject("standardCommandPrevious.Image");
        standardCommandPrevious.Location = new Point(89, 1);
        standardCommandPrevious.Margin = new Padding(1);
        standardCommandPrevious.Name = "standardCommandPrevious";
        standardCommandPrevious.Size = new Size(26, 24);
        standardCommandPrevious.TabIndex = 3;
        standardCommandPrevious.Visible = false;
        // 
        // standardCommandFirst
        // 
        standardCommandFirst.AccessibleName = "الأول";
        designerCommandBar.SetCommandRole(standardCommandFirst, Desktop.CoreUI.DesignerCommandRole.First);
        standardCommandFirst.Enabled = false;
        standardCommandFirst.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandFirst.FlatStyle = FlatStyle.Flat;
        standardCommandFirst.Image = (Image)resources.GetObject("standardCommandFirst.Image");
        standardCommandFirst.Location = new Point(61, 1);
        standardCommandFirst.Margin = new Padding(1);
        standardCommandFirst.Name = "standardCommandFirst";
        standardCommandFirst.Size = new Size(26, 24);
        standardCommandFirst.TabIndex = 4;
        standardCommandFirst.Visible = false;
        // 
        // standardCommandRefresh
        // 
        standardCommandRefresh.AccessibleName = "تحديث";
        designerCommandBar.SetCommandRole(standardCommandRefresh, Desktop.CoreUI.DesignerCommandRole.Refresh);
        standardCommandRefresh.Enabled = false;
        standardCommandRefresh.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandRefresh.FlatStyle = FlatStyle.Flat;
        standardCommandRefresh.Location = new Point(33, 1);
        standardCommandRefresh.Margin = new Padding(1);
        standardCommandRefresh.Name = "standardCommandRefresh";
        standardCommandRefresh.Size = new Size(26, 24);
        standardCommandRefresh.TabIndex = 5;
        standardCommandRefresh.Text = "تحديث";
        standardCommandRefresh.Visible = false;
        // 
        // standardCommandImport
        // 
        standardCommandImport.AccessibleName = "استيراد";
        designerCommandBar.SetCommandRole(standardCommandImport, Desktop.CoreUI.DesignerCommandRole.Import);
        standardCommandImport.Enabled = false;
        standardCommandImport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandImport.FlatStyle = FlatStyle.Flat;
        standardCommandImport.Location = new Point(5, 1);
        standardCommandImport.Margin = new Padding(1);
        standardCommandImport.Name = "standardCommandImport";
        standardCommandImport.Size = new Size(26, 24);
        standardCommandImport.TabIndex = 6;
        standardCommandImport.Text = "استيراد";
        standardCommandImport.Visible = false;
        // 
        // standardCommandExport
        // 
        standardCommandExport.AccessibleName = "تصدير";
        designerCommandBar.SetCommandRole(standardCommandExport, Desktop.CoreUI.DesignerCommandRole.Export);
        standardCommandExport.Enabled = false;
        standardCommandExport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandExport.FlatStyle = FlatStyle.Flat;
        standardCommandExport.Location = new Point(173, 27);
        standardCommandExport.Margin = new Padding(1);
        standardCommandExport.Name = "standardCommandExport";
        standardCommandExport.Size = new Size(26, 24);
        standardCommandExport.TabIndex = 7;
        standardCommandExport.Text = "تصدير";
        standardCommandExport.Visible = false;
        // 
        // standardCommandHelp
        // 
        standardCommandHelp.AccessibleName = "مساعدة";
        designerCommandBar.SetCommandRole(standardCommandHelp, Desktop.CoreUI.DesignerCommandRole.Help);
        standardCommandHelp.Enabled = false;
        standardCommandHelp.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandHelp.FlatStyle = FlatStyle.Flat;
        standardCommandHelp.Location = new Point(145, 27);
        standardCommandHelp.Margin = new Padding(1);
        standardCommandHelp.Name = "standardCommandHelp";
        standardCommandHelp.Size = new Size(26, 24);
        standardCommandHelp.TabIndex = 8;
        standardCommandHelp.Text = "مساعدة";
        standardCommandHelp.Visible = false;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.Location = new Point(11, 816);
        lblStatus.Name = "lblStatus";
        lblStatus.Padding = new Padding(6);
        lblStatus.Size = new Size(1203, 26);
        lblStatus.TabIndex = 4;
        lblStatus.Text = "مسودة واجهة غير محفوظة — الخدمات غير موصولة";
        // 
        // validationErrors
        // 
        validationErrors.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        validationErrors.ContainerControl = this;
        // 
        // tp1
        // 
        tp1.AutoScroll = true;
        tp1.Location = new Point(0, 0);
        tp1.Name = "tp1";
        tp1.Size = new Size(200, 100);
        tp1.TabIndex = 1;
        tp1.Text = "التفاصيل والحركات";
        // 
        // standardAuditMetadata
        // 
        standardAuditMetadata.AutoScroll = true;
        standardAuditMetadata.Dock = DockStyle.Bottom;
        standardAuditMetadata.Font = new Font("Tahoma", 9F);
        standardAuditMetadata.Location = new Point(0, 836);
        standardAuditMetadata.Margin = new Padding(0);
        standardAuditMetadata.MinimumSize = new Size(0, 64);
        standardAuditMetadata.Name = "standardAuditMetadata";
        standardAuditMetadata.RightToLeft = RightToLeft.Yes;
        standardAuditMetadata.Size = new Size(1200, 64);
        standardAuditMetadata.TabIndex = 1;
        standardAuditMetadata.TabStop = false;
        // 
        // UcScreen_04_05_01
        // 
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScroll = true;
        BackColor = Color.FromArgb(240, 240, 240);
        Controls.Add(rootWorkspaceViewport);
        Controls.Add(standardAuditMetadata);
        Font = new Font("Tahoma", 9F);
        Margin = new Padding(0);
        Name = "UcScreen_04_05_01";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1200, 900);
        Tag = "04.05.01";
        rootWorkspaceViewport.ResumeLayout(false);
        mainLayout.ResumeLayout(false);
        mainLayout.PerformLayout();
        totalsPanel.ResumeLayout(false);
        totalsPanel.PerformLayout();
        tlpAuditInfo.ResumeLayout(false);
        tlpAuditInfo.PerformLayout();
        tabs.ResumeLayout(false);
        tp0.ResumeLayout(false);
        tp0.PerformLayout();
        referenceHeader.ResumeLayout(false);
        referenceHeader.PerformLayout();
        tableLayoutPanel2.ResumeLayout(false);
        tableLayoutPanel2.PerformLayout();
        tableLayoutPanel1.ResumeLayout(false);
        tableLayoutPanel1.PerformLayout();
        documentAdditional.ResumeLayout(false);
        documentAdditionalSections.ResumeLayout(false);
        tabPage1.ResumeLayout(false);
        tabPage1.PerformLayout();
        layout0.ResumeLayout(false);
        layout0.PerformLayout();
        documentAccounts.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)documentAccountsGrid).EndInit();
        tp2.ResumeLayout(false);
        tp2.PerformLayout();
        layout2.ResumeLayout(false);
        tp3.ResumeLayout(false);
        tp3.PerformLayout();
        layout3.ResumeLayout(false);
        layout3.PerformLayout();
        tp4.ResumeLayout(false);
        tp4.PerformLayout();
        layout4.ResumeLayout(false);
        documentDefaults.ResumeLayout(false);
        documentDefaults.PerformLayout();
        documentDefaultsLayout.ResumeLayout(false);
        documentDefaultsLayout.PerformLayout();
        documentImport.ResumeLayout(false);
        documentImportLayout.ResumeLayout(false);
        documentImportLayout.PerformLayout();
        documentImportButtons.ResumeLayout(false);
        documentImportButtons.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)documentImportPreview).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
        designerCommandBar.ResumeLayout(false);
        flpActions.ResumeLayout(false);
        flpActions.PerformLayout();
        designerHiddenCommands.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)validationErrors).EndInit();
        ResumeLayout(false);
    }

    private DataGridView dgvLines;
    private DataGridViewComboBoxColumn col_accountRef;
    private DataGridViewTextBoxColumn col_analyticalAccount;
    private DataGridViewTextBoxColumn col_accountName;
    private DataGridViewTextBoxColumn col_lineDescription;
    private DataGridViewComboBoxColumn col_currencyRef;
    private DataGridViewTextBoxColumn col_exchangeRate;
    private DataGridViewTextBoxColumn col_foreignDebit;
    private DataGridViewTextBoxColumn col_foreignCredit;
    private DataGridViewTextBoxColumn col_rowNo;
    private DataGridViewComboBoxColumn col_costCenterRef;
    private DataGridViewTextBoxColumn col_debit;
    private DataGridViewTextBoxColumn col_credit;
    private DataGridViewTextBoxColumn col_accountingAmount;
    private DataGridViewTextBoxColumn col_approvalNumber;
    private DataGridViewTextBoxColumn col_salesperson;
    private DataGridViewTextBoxColumn col_collector;
    private DataGridViewTextBoxColumn col_referenceNumber;
    private TabControl tabs;
    private TabPage tp0;
    private TableLayoutPanel referenceHeader;
    private Label label1;
    private ComboBox textBox1;
    private Label lbl_destinationCashBankRef;
    private ComboBox field_destinationCashBankRef;
    private Label lbl_collector;
    private Label lbl_voucherNumber;
    private TextBox field_externalReference;
    private Label lbl_attachmentCount;
    private TextBox field_attachmentCount;
    private Label lbl_amount;
    private TextBox field_amount;
    private Label lbl_foreignReceipt;
    private TextBox field_foreignReceipt;
    private Label lbl_partyRef;
    private Label lbl_description;
    private Label lblAmountWords;
    private TabPage documentAdditional;
    private TabControl documentAdditionalSections;
    private TabPage tabPage1;
    private TableLayoutPanel layout0;
    private Label lbl_sourceCashBankRef;
    private ComboBox field_sourceCashBankRef;
    private Label lbl_counterAccountRef;
    private ComboBox field_counterAccountRef;
    private Label lbl_state;
    private TextBox field_state;
    private Label referenceLabel16;
    private ComboBox referenceField16;
    private Label referenceLabel17;
    private ComboBox referenceField17;
    private Label lbl_referenceType;
    private TextBox field_referenceType;
    private Label lblReceiptFlags;
    private TabPage documentAccounts;
    private DataGridView documentAccountsGrid;
    private DataGridViewTextBoxColumn documentAccountsGridColumn0;
    private DataGridViewTextBoxColumn documentAccountsGridColumn1;
    private DataGridViewTextBoxColumn documentAccountsGridColumn2;
    private DataGridViewTextBoxColumn documentAccountsGridColumn3;
    private DataGridViewTextBoxColumn documentAccountsGridColumn4;
    private DataGridViewTextBoxColumn documentAccountsGridColumn5;
    private DataGridViewTextBoxColumn documentAccountsGridColumn6;
    private DataGridViewTextBoxColumn documentAccountsGridColumn7;
    private TabPage tp2;
    private TableLayoutPanel layout2;
    private RichTextBox context2;
    private TabPage tp3;
    private TableLayoutPanel layout3;
    private RichTextBox context3;
    private Button btnReceiptApprove;
    private TabPage tp4;
    private TableLayoutPanel layout4;
    private RichTextBox context4;
    private TabPage documentDefaults;
    private TableLayoutPanel documentDefaultsLayout;
    private Label lblDefaultCurrency;
    private ComboBox cboDefaultCurrency;
    private Label lblDefaultCostCenter;
    private ComboBox cboDefaultCostCenter;
    private Button btnReceiptSettings;
    private TabPage documentImport;
    private TableLayoutPanel documentImportLayout;
    private Label documentImportFormat;
    private FlowLayoutPanel documentImportButtons;
    private TextBox documentImportPath;
    private Button documentImportChoose;
    private Button documentImportApply;
    private DataGridView documentImportPreview;
    private Label label2;
    private ComboBox comboBox5;
    private TextBox field_description;
    private TextBox textBox5;
    private Label label4;
    private TextBox field_documentNumber;
    private Label label3;
    private TableLayoutPanel tableLayoutPanel1;
    private ReceiptDateTimePicker field_accountingDate;
    private Label label5;
    private CheckBox checkBox1;
    private TextBox textBox6;
    private TableLayoutPanel tableLayoutPanel2;
    private Label label11;
    private Label label10;
    private Label label9;
    private Label label8;
    private Label label7;
    private Label label6;
    private CheckBox checkBox12;
    private CheckBox checkBox10;
    private CheckBox checkBox8;
    private CheckBox checkBox6;
    private CheckBox checkBox4;
    private CheckBox checkBox2;
    private TableLayoutPanel tlpAuditInfo;
    private Label lblCreatedBy;
    private Label lblCreatedAt;
    private Label lblCreatedDevice;
    private Label lblPrintCount;
    private Label lblAccountName;
    private Label lblModifiedBy;
    private Label lblModifiedAt;
    private Label lblModifiedDevice;
    private Label lblLastPrintedAt;
    private Label lblEditCount;
    private TableLayoutPanel totalsPanel;
    private Label label12;
    private TextBox field_total;
    private Label label13;
    private ComboBox field_currencyRef = null!;
    private TextBox field_exchangeRate = null!;
    private Label lbl_currencyRef = null!;
    private Label lbl_exchangeRate = null!;
    private TextBox field_difference;
    private Label lblTotalsMessage;
    private TextBox textBox2;
}
