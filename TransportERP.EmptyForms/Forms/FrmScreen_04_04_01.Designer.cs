#nullable enable
using System.Drawing;
using System.Windows.Forms;
namespace TransportERP.EmptyForms;
partial class UcScreen_04_04_01
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

    private DataGridViewComboBoxColumn receiptWaybillColumn = null!;
    private Button btnReceiptSettings = null!;
    private Button btnReceiptApprove = null!;
    private TableLayoutPanel bankCurrencyGroup = null!;
    private TextBox field_bankName = null!;
    private Label lbl_foreignReceipt = null!;
    private TextBox field_foreignReceipt = null!;
    private Label lbl_postingMethod = null!;
    private TextBox field_postingMethod = null!;
    private Label lbl_importNumber = null!;
    private TextBox field_importNumber = null!;
    private ToolTip receiptHints = null!;
    private Button btnPrint = null!;
    private ComboBox field_sourceCashBankRef = null!;
    private Label lbl_sourceCashBankRef = null!;
    private Label lblAmountWords = null!;
    private Label lblTotals = null!;
    private TableLayoutPanel totalsPanel = null!;
    private Label lblTotalsMessage = null!;
    private Label lblReceiptFlags = null!;
    private Label lblCreatedDevice = null!;
    private Label lblModifiedDevice = null!;
    private Label lblAccountName = null!;
    private ComboBox field_operation = null!;
    private Label lbl_operation = null!;
    private ComboBox field_voucherType = null!;
    private Label lbl_voucherType = null!;
    private ComboBox field_salesperson = null!;
    private Label lbl_salesperson = null!;
    private ComboBox field_collector = null!;
    private Label lbl_collector = null!;
    private TextBox field_commission = null!;
    private Label lbl_commission = null!;
    private TextBox field_costCenter = null!;
    private Label lbl_costCenter = null!;
    private TextBox field_project = null!;
    private Label lbl_project = null!;
    private TextBox field_activity = null!;
    private Label lbl_activity = null!;
    private TextBox field_attachmentCount = null!;
    private Label lbl_attachmentCount = null!;
    private TextBox field_referenceNumber = null!;
    private Label lbl_referenceNumber = null!;
    private TextBox field_referenceName = null!;
    private Label lbl_referenceName = null!;
    private TextBox field_importSource = null!;
    private Label lbl_importSource = null!;
    private TextBox field_referenceType = null!;
    private Label lbl_referenceType = null!;
    private System.ComponentModel.IContainer? components;
    private ErrorProvider validationErrors = null!;
    protected override void Dispose(bool disposing) { if(disposing) components?.Dispose(); base.Dispose(disposing); }
    private TableLayoutPanel referenceHeader = null!;
    private TabPage referenceExtra = null!;
    private Label referenceAudit = null!;
    private Label referenceTotals = null!;
    private Label referenceLabel15 = null!;
    private ComboBox referenceField15 = null!;
    private Label referenceLabel16 = null!;
    private ComboBox referenceField16 = null!;
    private Label referenceLabel17 = null!;
    private ComboBox referenceField17 = null!;
    private TabPage documentAdditional = null!;
    private TabControl documentAdditionalSections = null!;
    private TabPage documentDefaults = null!;
    private TableLayoutPanel documentDefaultsLayout = null!;
    private Label lblDefaultCurrency = null!;
    private ComboBox cboDefaultCurrency = null!;
    private Label lblDefaultCostCenter = null!;
    private ComboBox cboDefaultCostCenter = null!;
    private TabPage documentAccounts = null!;
    private DataGridView documentAccountsGrid = null!;
    private DataGridViewTextBoxColumn documentAccountsGridColumn0 = null!;
    private DataGridViewTextBoxColumn documentAccountsGridColumn1 = null!;
    private DataGridViewTextBoxColumn documentAccountsGridColumn2 = null!;
    private DataGridViewTextBoxColumn documentAccountsGridColumn3 = null!;
    private DataGridViewTextBoxColumn documentAccountsGridColumn4 = null!;
    private DataGridViewTextBoxColumn documentAccountsGridColumn5 = null!;
    private DataGridViewTextBoxColumn documentAccountsGridColumn6 = null!;
    private DataGridViewTextBoxColumn documentAccountsGridColumn7 = null!;
    private TabPage documentImport = null!;
    private TableLayoutPanel documentImportLayout = null!;
    private TextBox documentImportPath = null!;
    private Button documentImportChoose = null!;
    private Button documentImportApply = null!;
    private Label documentImportFormat = null!;
    private FlowLayoutPanel documentImportButtons = null!;
    private DataGridView documentImportPreview = null!;
    private void InitializeComponent()
    {
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
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
        components = new System.ComponentModel.Container();
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
        bankCurrencyGroup = new TableLayoutPanel();
        field_bankName = new TextBox();
        lbl_currencyRef = new Label();
        field_currencyRef = new ComboBox();
        lbl_foreignReceipt = new Label();
        field_foreignReceipt = new TextBox();
        lbl_postingMethod = new Label();
        field_postingMethod = new TextBox();
        lbl_importNumber = new Label();
        field_importNumber = new TextBox();
        documentAdditional = new TabPage();
        documentAdditionalSections = new TabControl();
        tp1 = new TabPage();
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
        referenceExtra = new TabPage();
        lblAmountWords = new Label();
        lbl_salesperson = new Label();
        field_salesperson = new ComboBox();
        lbl_costCenter = new Label();
        field_costCenter = new TextBox();
        lbl_project = new Label();
        field_project = new TextBox();
        lbl_activity = new Label();
        field_activity = new TextBox();
        lbl_importSource = new Label();
        field_importSource = new TextBox();
        lbl_operation = new Label();
        field_operation = new ComboBox();
        field_description = new TextBox();
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
        referenceHeader = new TableLayoutPanel();
        label1 = new Label();
        textBox1 = new ComboBox();
        lbl_destinationCashBankRef = new Label();
        field_destinationCashBankRef = new ComboBox();
        lbl_collector = new Label();
        field_collector = new ComboBox();
        lbl_commission = new Label();
        field_commission = new TextBox();
        lbl_voucherNumber = new Label();
        field_voucherNumber = new TextBox();
        lbl_voucherDate = new Label();
        field_voucherDate = new ReceiptDateTimePicker();
        lbl_attachmentCount = new Label();
        field_attachmentCount = new TextBox();
        lbl_voucherType = new Label();
        field_voucherType = new ComboBox();
        lbl_amount = new Label();
        field_amount = new TextBox();
        lbl_exchangeRate = new Label();
        field_exchangeRate = new TextBox();
        referenceLabel15 = new Label();
        referenceField15 = new ComboBox();
        lbl_partyRef = new Label();
        field_partyRef = new ComboBox();
        lbl_referenceNumber = new Label();
        field_referenceNumber = new TextBox();
        lbl_referenceName = new Label();
        field_referenceName = new TextBox();
        lbl_description = new Label();
        referenceAudit = new Label();
        referenceTotals = new Label();
        validationErrors = new ErrorProvider(components);
        mainLayout = new TableLayoutPanel();
        dgvLines = new DataGridView();
        col_rowNo = new DataGridViewTextBoxColumn();
        col_counterAccountRef = new DataGridViewComboBoxColumn();
        col_analyticalAccount = new DataGridViewTextBoxColumn();
        col_accountName = new DataGridViewTextBoxColumn();
        col_lineDescription = new DataGridViewTextBoxColumn();
        col_currencyRef = new DataGridViewComboBoxColumn();
        col_exchangeRate = new DataGridViewTextBoxColumn();
        col_amount = new DataGridViewTextBoxColumn();
        col_foreignAmount = new DataGridViewTextBoxColumn();
        col_costCenter = new DataGridViewTextBoxColumn();
        col_referenceNumber = new DataGridViewTextBoxColumn();
        col_salespersonNumber = new DataGridViewTextBoxColumn();
        col_cashierNumber = new DataGridViewTextBoxColumn();
        col_chequeNumber = new DataGridViewTextBoxColumn();
        col_chequeDueDate = new DataGridViewTextBoxColumn();
        col_partyRef = new DataGridViewComboBoxColumn();
        col_accountingAmount = new DataGridViewTextBoxColumn();
        col_project = new DataGridViewTextBoxColumn();
        col_activity = new DataGridViewTextBoxColumn();
        receiptWaybillColumn = new DataGridViewComboBoxColumn();
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
        tabs = new TabControl();
        tp0 = new TabPage();
        linesHost = new TableLayoutPanel();
        totalsPanel = new TableLayoutPanel();
        label3 = new Label();
        field_total = new TextBox();
        label2 = new Label();
        field_difference = new TextBox();
        lblTotalsMessage = new Label();
        lblStatus = new Label();
        lblTotals = new Label();
        receiptHints = new ToolTip(components);
        bankCurrencyGroup.SuspendLayout();
        documentAdditional.SuspendLayout();
        documentAdditionalSections.SuspendLayout();
        tp1.SuspendLayout();
        layout0.SuspendLayout();
        documentAccounts.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)documentAccountsGrid).BeginInit();
        tp2.SuspendLayout();
        layout2.SuspendLayout();
        tp3.SuspendLayout();
        layout3.SuspendLayout();
        tp4.SuspendLayout();
        layout4.SuspendLayout();
        tlpAuditInfo.SuspendLayout();
        documentDefaults.SuspendLayout();
        documentDefaultsLayout.SuspendLayout();
        documentImport.SuspendLayout();
        documentImportLayout.SuspendLayout();
        documentImportButtons.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)documentImportPreview).BeginInit();
        referenceHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)validationErrors).BeginInit();
        mainLayout.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
        flpActions.SuspendLayout();
        tabs.SuspendLayout();
        tp0.SuspendLayout();
        linesHost.SuspendLayout();
        totalsPanel.SuspendLayout();
        SuspendLayout();
        // 
        // bankCurrencyGroup
        // 
        bankCurrencyGroup.AutoSize = true;
        bankCurrencyGroup.ColumnCount = 3;
        referenceHeader.SetColumnSpan(bankCurrencyGroup, 2);
        bankCurrencyGroup.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67F));
        bankCurrencyGroup.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 59F));
        bankCurrencyGroup.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
        bankCurrencyGroup.Controls.Add(field_bankName, 0, 0);
        bankCurrencyGroup.Controls.Add(lbl_currencyRef, 1, 0);
        bankCurrencyGroup.Controls.Add(field_currencyRef, 2, 0);
        bankCurrencyGroup.Dock = DockStyle.Fill;
        bankCurrencyGroup.Location = new Point(629, 28);
        bankCurrencyGroup.Margin = new Padding(0);
        bankCurrencyGroup.Name = "bankCurrencyGroup";
        bankCurrencyGroup.RowCount = 1;
        bankCurrencyGroup.RowStyles.Add(new RowStyle());
        bankCurrencyGroup.Size = new Size(407, 28);
        bankCurrencyGroup.TabIndex = 8;
        // 
        // field_bankName
        // 
        field_bankName.AccessibleName = "اسم البنك أو الصندوق";
        field_bankName.BackColor = Color.White;
        field_bankName.Dock = DockStyle.Fill;
        field_bankName.Font = new Font("Tahoma", 9.25F);
        field_bankName.Location = new Point(175, 2);
        field_bankName.Margin = new Padding(1, 2, 1, 2);
        field_bankName.Name = "field_bankName";
        field_bankName.ReadOnly = true;
        field_bankName.Size = new Size(231, 22);
        field_bankName.TabIndex = 0;
        field_bankName.TabStop = false;
        // 
        // lbl_currencyRef
        // 
        lbl_currencyRef.AutoSize = true;
        lbl_currencyRef.Dock = DockStyle.Fill;
        lbl_currencyRef.Font = new Font("Tahoma", 8.25F);
        lbl_currencyRef.Location = new Point(116, 1);
        lbl_currencyRef.Margin = new Padding(1);
        lbl_currencyRef.Name = "lbl_currencyRef";
        lbl_currencyRef.Size = new Size(57, 26);
        lbl_currencyRef.TabIndex = 1;
        lbl_currencyRef.Text = "العملة";
        lbl_currencyRef.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_currencyRef
        // 
        field_currencyRef.AccessibleName = "العملة";
        field_currencyRef.BackColor = Color.LightYellow;
        field_currencyRef.Dock = DockStyle.Fill;
        field_currencyRef.DropDownStyle = ComboBoxStyle.DropDownList;
        field_currencyRef.DropDownWidth = 420;
        field_currencyRef.Font = new Font("Tahoma", 9.25F);
        field_currencyRef.Location = new Point(1, 2);
        field_currencyRef.Margin = new Padding(1, 2, 1, 2);
        field_currencyRef.Name = "field_currencyRef";
        field_currencyRef.Size = new Size(113, 22);
        field_currencyRef.TabIndex = 2;
        field_currencyRef.Tag = "ACC-043:currencyRef";
        // 
        // lbl_foreignReceipt
        // 
        lbl_foreignReceipt.AutoSize = true;
        lbl_foreignReceipt.Dock = DockStyle.Fill;
        lbl_foreignReceipt.Font = new Font("Tahoma", 8.25F);
        lbl_foreignReceipt.Location = new Point(530, 83);
        lbl_foreignReceipt.Margin = new Padding(1);
        lbl_foreignReceipt.Name = "lbl_foreignReceipt";
        lbl_foreignReceipt.Size = new Size(98, 24);
        lbl_foreignReceipt.TabIndex = 25;
        lbl_foreignReceipt.Text = "المبلغ الأجنبي";
        lbl_foreignReceipt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_foreignReceipt
        // 
        field_foreignReceipt.AccessibleName = "المبلغ الأجنبي";
        field_foreignReceipt.BackColor = Color.White;
        field_foreignReceipt.Dock = DockStyle.Fill;
        field_foreignReceipt.Font = new Font("Tahoma", 9.25F);
        field_foreignReceipt.Location = new Point(317, 84);
        field_foreignReceipt.Margin = new Padding(1, 2, 1, 2);
        field_foreignReceipt.Name = "field_foreignReceipt";
        field_foreignReceipt.ReadOnly = true;
        field_foreignReceipt.Size = new Size(211, 22);
        field_foreignReceipt.TabIndex = 26;
        field_foreignReceipt.TabStop = false;
        receiptHints.SetToolTip(field_foreignReceipt, "حقل ظاهر في المرجع؛ يحتاج ربط بيانات السند");
        // 
        // lbl_postingMethod
        // 
        lbl_postingMethod.AutoSize = true;
        lbl_postingMethod.Dock = DockStyle.Fill;
        lbl_postingMethod.Font = new Font("Tahoma", 8.25F);
        lbl_postingMethod.Location = new Point(1303, 161);
        lbl_postingMethod.Margin = new Padding(1);
        lbl_postingMethod.Name = "lbl_postingMethod";
        lbl_postingMethod.Size = new Size(98, 24);
        lbl_postingMethod.TabIndex = 37;
        lbl_postingMethod.Text = "طريقة الترحيل";
        lbl_postingMethod.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_postingMethod
        // 
        field_postingMethod.AccessibleName = "طريقة الترحيل";
        field_postingMethod.BackColor = Color.White;
        field_postingMethod.Dock = DockStyle.Fill;
        field_postingMethod.Font = new Font("Tahoma", 9.25F);
        field_postingMethod.Location = new Point(1037, 162);
        field_postingMethod.Margin = new Padding(1, 2, 1, 2);
        field_postingMethod.Name = "field_postingMethod";
        field_postingMethod.ReadOnly = true;
        field_postingMethod.Size = new Size(264, 22);
        field_postingMethod.TabIndex = 38;
        field_postingMethod.TabStop = false;
        receiptHints.SetToolTip(field_postingMethod, "حقل ظاهر في المرجع؛ يحتاج ربط بيانات السند");
        // 
        // lbl_importNumber
        // 
        lbl_importNumber.AutoSize = true;
        lbl_importNumber.Dock = DockStyle.Fill;
        lbl_importNumber.Font = new Font("Tahoma", 8.25F);
        lbl_importNumber.Location = new Point(217, 161);
        lbl_importNumber.Margin = new Padding(1);
        lbl_importNumber.Name = "lbl_importNumber";
        lbl_importNumber.Size = new Size(98, 24);
        lbl_importNumber.TabIndex = 41;
        lbl_importNumber.Text = "الرقم";
        lbl_importNumber.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_importNumber
        // 
        field_importNumber.AccessibleName = "الرقم";
        field_importNumber.BackColor = Color.White;
        field_importNumber.Dock = DockStyle.Fill;
        field_importNumber.Font = new Font("Tahoma", 9.25F);
        field_importNumber.Location = new Point(3, 162);
        field_importNumber.Margin = new Padding(1, 2, 1, 2);
        field_importNumber.Name = "field_importNumber";
        field_importNumber.ReadOnly = true;
        field_importNumber.Size = new Size(212, 22);
        field_importNumber.TabIndex = 42;
        field_importNumber.TabStop = false;
        receiptHints.SetToolTip(field_importNumber, "حقل ظاهر في المرجع؛ يحتاج ربط بيانات السند");
        // 
        // documentAdditional
        // 
        documentAdditional.AutoScroll = true;
        documentAdditional.Controls.Add(documentAdditionalSections);
        documentAdditional.Location = new Point(4, 23);
        documentAdditional.Name = "documentAdditional";
        documentAdditional.Size = new Size(1404, 267);
        documentAdditional.TabIndex = 6;
        documentAdditional.Text = "بيانات إضافية";
        // 
        // documentAdditionalSections
        // 
        documentAdditionalSections.Controls.Add(tp1);
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
        documentAdditionalSections.Size = new Size(1404, 267);
        documentAdditionalSections.TabIndex = 0;
        // 
        // tp1
        // 
        tp1.AutoScroll = true;
        tp1.Controls.Add(layout0);
        tp1.Location = new Point(4, 23);
        tp1.Margin = new Padding(4);
        tp1.Name = "tp1";
        tp1.Size = new Size(1396, 240);
        tp1.TabIndex = 1;
        tp1.Text = "تفاصيل السند";
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
        layout0.Size = new Size(1396, 107);
        layout0.TabIndex = 0;
        // 
        // lbl_sourceCashBankRef
        // 
        lbl_sourceCashBankRef.Dock = DockStyle.Fill;
        lbl_sourceCashBankRef.Location = new Point(1237, 4);
        lbl_sourceCashBankRef.Name = "lbl_sourceCashBankRef";
        lbl_sourceCashBankRef.Size = new Size(152, 36);
        lbl_sourceCashBankRef.TabIndex = 0;
        lbl_sourceCashBankRef.Text = "الصندوق/البنك المصدر";
        lbl_sourceCashBankRef.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_sourceCashBankRef
        // 
        field_sourceCashBankRef.BackColor = Color.White;
        field_sourceCashBankRef.Dock = DockStyle.Fill;
        field_sourceCashBankRef.DropDownStyle = ComboBoxStyle.DropDownList;
        field_sourceCashBankRef.Location = new Point(933, 7);
        field_sourceCashBankRef.Name = "field_sourceCashBankRef";
        field_sourceCashBankRef.Size = new Size(298, 22);
        field_sourceCashBankRef.TabIndex = 1;
        // 
        // lbl_counterAccountRef
        // 
        lbl_counterAccountRef.AutoSize = true;
        lbl_counterAccountRef.Dock = DockStyle.Fill;
        lbl_counterAccountRef.Location = new Point(776, 4);
        lbl_counterAccountRef.Margin = new Padding(4, 0, 4, 0);
        lbl_counterAccountRef.Name = "lbl_counterAccountRef";
        lbl_counterAccountRef.Size = new Size(150, 36);
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
        field_counterAccountRef.Location = new Point(472, 8);
        field_counterAccountRef.Margin = new Padding(4);
        field_counterAccountRef.Name = "field_counterAccountRef";
        field_counterAccountRef.Size = new Size(296, 22);
        field_counterAccountRef.TabIndex = 3;
        field_counterAccountRef.Tag = "ACC-043:counterAccountRef";
        // 
        // lbl_state
        // 
        lbl_state.AutoSize = true;
        lbl_state.Dock = DockStyle.Fill;
        lbl_state.Location = new Point(314, 4);
        lbl_state.Margin = new Padding(4, 0, 4, 0);
        lbl_state.Name = "lbl_state";
        lbl_state.Size = new Size(150, 36);
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
        field_state.Size = new Size(298, 22);
        field_state.TabIndex = 5;
        field_state.TabStop = false;
        field_state.Tag = "ACC-043:state";
        // 
        // referenceLabel16
        // 
        referenceLabel16.Dock = DockStyle.Fill;
        referenceLabel16.Location = new Point(1236, 42);
        referenceLabel16.Margin = new Padding(2);
        referenceLabel16.Name = "referenceLabel16";
        referenceLabel16.Size = new Size(154, 28);
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
        referenceField16.Location = new Point(932, 42);
        referenceField16.Margin = new Padding(2);
        referenceField16.Name = "referenceField16";
        referenceField16.Size = new Size(300, 22);
        referenceField16.TabIndex = 7;
        referenceField16.TabStop = false;
        // 
        // referenceLabel17
        // 
        referenceLabel17.Dock = DockStyle.Fill;
        referenceLabel17.Location = new Point(774, 42);
        referenceLabel17.Margin = new Padding(2);
        referenceLabel17.Name = "referenceLabel17";
        referenceLabel17.Size = new Size(154, 28);
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
        referenceField17.Location = new Point(470, 42);
        referenceField17.Margin = new Padding(2);
        referenceField17.Name = "referenceField17";
        referenceField17.Size = new Size(300, 22);
        referenceField17.TabIndex = 9;
        referenceField17.TabStop = false;
        // 
        // lbl_referenceType
        // 
        lbl_referenceType.Dock = DockStyle.Fill;
        lbl_referenceType.Location = new Point(313, 40);
        lbl_referenceType.Name = "lbl_referenceType";
        lbl_referenceType.Size = new Size(152, 32);
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
        field_referenceType.Size = new Size(300, 22);
        field_referenceType.TabIndex = 11;
        field_referenceType.TabStop = false;
        receiptHints.SetToolTip(field_referenceType, "حقل مرجعي؛ يحتاج ربط بيانات السند قبل الإدخال");
        // 
        // lblReceiptFlags
        // 
        layout0.SetColumnSpan(lblReceiptFlags, 6);
        lblReceiptFlags.Dock = DockStyle.Fill;
        lblReceiptFlags.Location = new Point(7, 72);
        lblReceiptFlags.Name = "lblReceiptFlags";
        lblReceiptFlags.Size = new Size(1382, 31);
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
        documentAccounts.Size = new Size(1396, 240);
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
        documentAccountsGrid.Size = new Size(1396, 240);
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
        tp2.Size = new Size(1396, 240);
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
        layout2.Size = new Size(1396, 240);
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
        context2.Size = new Size(1388, 232);
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
        tp3.Size = new Size(1396, 240);
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
        layout3.Size = new Size(1396, 240);
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
        context3.Size = new Size(1388, 201);
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
        btnReceiptApprove.Location = new Point(3, 212);
        btnReceiptApprove.Name = "btnReceiptApprove";
        btnReceiptApprove.Size = new Size(1390, 25);
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
        tp4.Size = new Size(1396, 240);
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
        layout4.Size = new Size(1396, 240);
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
        context4.Size = new Size(1388, 232);
        context4.TabIndex = 0;
        context4.TabStop = false;
        context4.Text = "";
        // 
        // referenceExtra
        // 
        referenceExtra.AutoScroll = true;
        referenceExtra.Location = new Point(4, 27);
        referenceExtra.Name = "referenceExtra";
        referenceExtra.Size = new Size(1392, 253);
        referenceExtra.TabIndex = 5;
        referenceExtra.Text = "حقول تكميلية";
        // 
        // lblAmountWords
        // 
        lblAmountWords.AutoSize = true;
        lblAmountWords.BackColor = Color.FromArgb(229, 240, 222);
        referenceHeader.SetColumnSpan(lblAmountWords, 8);
        lblAmountWords.Dock = DockStyle.Fill;
        lblAmountWords.Font = new Font("Tahoma", 8.25F);
        lblAmountWords.Location = new Point(3, 187);
        lblAmountWords.Margin = new Padding(1);
        lblAmountWords.Name = "lblAmountWords";
        lblAmountWords.Size = new Size(1398, 77);
        lblAmountWords.TabIndex = 43;
        lblAmountWords.Text = "المبلغ كتابةً: —";
        lblAmountWords.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lbl_salesperson
        // 
        lbl_salesperson.AutoSize = true;
        lbl_salesperson.Dock = DockStyle.Fill;
        lbl_salesperson.Font = new Font("Tahoma", 8.25F);
        lbl_salesperson.Location = new Point(217, 3);
        lbl_salesperson.Margin = new Padding(1);
        lbl_salesperson.Name = "lbl_salesperson";
        lbl_salesperson.Size = new Size(98, 24);
        lbl_salesperson.TabIndex = 4;
        lbl_salesperson.Text = "رقم المندوب";
        lbl_salesperson.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_salesperson
        // 
        field_salesperson.AccessibleName = "رقم المندوب";
        field_salesperson.BackColor = Color.White;
        field_salesperson.Dock = DockStyle.Fill;
        field_salesperson.DropDownStyle = ComboBoxStyle.DropDownList;
        field_salesperson.DropDownWidth = 320;
        field_salesperson.Font = new Font("Tahoma", 9.25F);
        field_salesperson.Location = new Point(3, 4);
        field_salesperson.Margin = new Padding(1, 2, 1, 2);
        field_salesperson.Name = "field_salesperson";
        field_salesperson.Size = new Size(212, 22);
        field_salesperson.TabIndex = 5;
        receiptHints.SetToolTip(field_salesperson, "حقل مرجعي؛ يحتاج ربط بيانات السند قبل الإدخال");
        // 
        // lbl_costCenter
        // 
        lbl_costCenter.Dock = DockStyle.Fill;
        lbl_costCenter.Location = new Point(309, 128);
        lbl_costCenter.Name = "lbl_costCenter";
        lbl_costCenter.Size = new Size(149, 31);
        lbl_costCenter.TabIndex = 19;
        lbl_costCenter.Text = "مركز التكلفة";
        lbl_costCenter.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_costCenter
        // 
        field_costCenter.AccessibleName = "مركز التكلفة";
        field_costCenter.Dock = DockStyle.Fill;
        field_costCenter.Location = new Point(7, 131);
        field_costCenter.Name = "field_costCenter";
        field_costCenter.ReadOnly = true;
        field_costCenter.Size = new Size(296, 23);
        field_costCenter.TabIndex = 20;
        field_costCenter.TabStop = false;
        receiptHints.SetToolTip(field_costCenter, "حقل مرجعي؛ يحتاج ربط بيانات السند قبل الإدخال");
        // 
        // lbl_project
        // 
        lbl_project.Dock = DockStyle.Fill;
        lbl_project.Location = new Point(309, 159);
        lbl_project.Name = "lbl_project";
        lbl_project.Size = new Size(149, 31);
        lbl_project.TabIndex = 21;
        lbl_project.Text = "المشروع";
        lbl_project.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_project
        // 
        field_project.AccessibleName = "المشروع";
        field_project.Dock = DockStyle.Fill;
        field_project.Location = new Point(7, 162);
        field_project.Name = "field_project";
        field_project.ReadOnly = true;
        field_project.Size = new Size(296, 23);
        field_project.TabIndex = 22;
        field_project.TabStop = false;
        receiptHints.SetToolTip(field_project, "حقل مرجعي؛ يحتاج ربط بيانات السند قبل الإدخال");
        // 
        // lbl_activity
        // 
        lbl_activity.Dock = DockStyle.Fill;
        lbl_activity.Location = new Point(309, 190);
        lbl_activity.Name = "lbl_activity";
        lbl_activity.Size = new Size(149, 31);
        lbl_activity.TabIndex = 23;
        lbl_activity.Text = "النشاط";
        lbl_activity.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_activity
        // 
        field_activity.AccessibleName = "النشاط";
        field_activity.Dock = DockStyle.Fill;
        field_activity.Location = new Point(7, 193);
        field_activity.Name = "field_activity";
        field_activity.ReadOnly = true;
        field_activity.Size = new Size(296, 23);
        field_activity.TabIndex = 24;
        field_activity.TabStop = false;
        receiptHints.SetToolTip(field_activity, "حقل مرجعي؛ يحتاج ربط بيانات السند قبل الإدخال");
        // 
        // lbl_importSource
        // 
        lbl_importSource.AutoSize = true;
        lbl_importSource.Dock = DockStyle.Fill;
        lbl_importSource.Font = new Font("Tahoma", 8.25F);
        lbl_importSource.Location = new Point(825, 161);
        lbl_importSource.Margin = new Padding(1);
        lbl_importSource.Name = "lbl_importSource";
        lbl_importSource.Size = new Size(210, 24);
        lbl_importSource.TabIndex = 39;
        lbl_importSource.Text = "إدراج البيانات من";
        lbl_importSource.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_importSource
        // 
        field_importSource.AccessibleName = "إدراج البيانات من";
        field_importSource.BackColor = Color.White;
        referenceHeader.SetColumnSpan(field_importSource, 3);
        field_importSource.Dock = DockStyle.Fill;
        field_importSource.Font = new Font("Tahoma", 9.25F);
        field_importSource.Location = new Point(317, 162);
        field_importSource.Margin = new Padding(1, 2, 1, 2);
        field_importSource.Name = "field_importSource";
        field_importSource.ReadOnly = true;
        field_importSource.Size = new Size(506, 22);
        field_importSource.TabIndex = 40;
        field_importSource.TabStop = false;
        receiptHints.SetToolTip(field_importSource, "حقل مرجعي؛ يحتاج ربط بيانات السند قبل الإدخال");
        // 
        // lbl_operation
        // 
        lbl_operation.AutoSize = true;
        lbl_operation.Dock = DockStyle.Fill;
        lbl_operation.Font = new Font("Tahoma", 8.25F);
        lbl_operation.Location = new Point(530, 3);
        lbl_operation.Margin = new Padding(1);
        lbl_operation.Name = "lbl_operation";
        lbl_operation.Size = new Size(98, 24);
        lbl_operation.TabIndex = 2;
        lbl_operation.Text = "نوع العملية";
        lbl_operation.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_operation
        // 
        field_operation.AccessibleName = "نوع العملية";
        field_operation.BackColor = Color.LightYellow;
        field_operation.Dock = DockStyle.Fill;
        field_operation.DropDownStyle = ComboBoxStyle.DropDownList;
        field_operation.Font = new Font("Tahoma", 9.25F);
        field_operation.Location = new Point(317, 4);
        field_operation.Margin = new Padding(1, 2, 1, 2);
        field_operation.Name = "field_operation";
        field_operation.Size = new Size(211, 22);
        field_operation.TabIndex = 3;
        receiptHints.SetToolTip(field_operation, "طريقة القبض؛ تظهر بيانات الشيك عند اختياره");
        // 
        // field_description
        // 
        field_description.AccessibleName = "البيان";
        field_description.BackColor = Color.LightYellow;
        referenceHeader.SetColumnSpan(field_description, 3);
        field_description.Dock = DockStyle.Fill;
        field_description.Font = new Font("Tahoma", 9.25F);
        field_description.Location = new Point(630, 136);
        field_description.Margin = new Padding(1, 2, 1, 2);
        field_description.Name = "field_description";
        field_description.Size = new Size(671, 22);
        field_description.TabIndex = 36;
        field_description.Tag = "ACC-043:description";
        // 
        // tlpAuditInfo
        // 
        tlpAuditInfo.AccessibleName = "بيانات متابعة سند القبض";
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
        tlpAuditInfo.Location = new Point(4, 834);
        tlpAuditInfo.Margin = new Padding(0);
        tlpAuditInfo.Name = "tlpAuditInfo";
        tlpAuditInfo.Padding = new Padding(4);
        tlpAuditInfo.RightToLeft = RightToLeft.Yes;
        tlpAuditInfo.RowCount = 2;
        tlpAuditInfo.RowStyles.Add(new RowStyle());
        tlpAuditInfo.RowStyles.Add(new RowStyle());
        tlpAuditInfo.Size = new Size(1416, 68);
        tlpAuditInfo.TabIndex = 4;
        // 
        // lblCreatedBy
        // 
        lblCreatedBy.AutoSize = true;
        lblCreatedBy.BackColor = Color.FromArgb(232, 246, 248);
        lblCreatedBy.Dock = DockStyle.Fill;
        lblCreatedBy.Font = new Font("Tahoma", 9F);
        lblCreatedBy.ForeColor = Color.FromArgb(16, 24, 40);
        lblCreatedBy.Location = new Point(1133, 6);
        lblCreatedBy.Margin = new Padding(2);
        lblCreatedBy.MinimumSize = new Size(0, 26);
        lblCreatedBy.Name = "lblCreatedBy";
        lblCreatedBy.Padding = new Padding(6, 3, 6, 3);
        lblCreatedBy.RightToLeft = RightToLeft.Yes;
        lblCreatedBy.Size = new Size(277, 26);
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
        lblCreatedAt.Location = new Point(852, 6);
        lblCreatedAt.Margin = new Padding(2);
        lblCreatedAt.MinimumSize = new Size(0, 26);
        lblCreatedAt.Name = "lblCreatedAt";
        lblCreatedAt.Padding = new Padding(6, 3, 6, 3);
        lblCreatedAt.RightToLeft = RightToLeft.Yes;
        lblCreatedAt.Size = new Size(277, 26);
        lblCreatedAt.TabIndex = 1;
        lblCreatedAt.Text = "تاريخ الانشاء: —";
        lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCreatedDevice
        // 
        lblCreatedDevice.AutoSize = true;
        lblCreatedDevice.Dock = DockStyle.Fill;
        lblCreatedDevice.Location = new Point(571, 6);
        lblCreatedDevice.Margin = new Padding(2);
        lblCreatedDevice.MinimumSize = new Size(0, 26);
        lblCreatedDevice.Name = "lblCreatedDevice";
        lblCreatedDevice.Padding = new Padding(6, 3, 6, 3);
        lblCreatedDevice.RightToLeft = RightToLeft.Yes;
        lblCreatedDevice.Size = new Size(277, 26);
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
        lblPrintCount.Location = new Point(290, 6);
        lblPrintCount.Margin = new Padding(2);
        lblPrintCount.MinimumSize = new Size(0, 26);
        lblPrintCount.Name = "lblPrintCount";
        lblPrintCount.Padding = new Padding(6, 3, 6, 3);
        lblPrintCount.RightToLeft = RightToLeft.Yes;
        lblPrintCount.Size = new Size(277, 26);
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
        lblAccountName.Size = new Size(280, 26);
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
        lblModifiedBy.Location = new Point(1133, 36);
        lblModifiedBy.Margin = new Padding(2);
        lblModifiedBy.MinimumSize = new Size(0, 26);
        lblModifiedBy.Name = "lblModifiedBy";
        lblModifiedBy.Padding = new Padding(6, 3, 6, 3);
        lblModifiedBy.RightToLeft = RightToLeft.Yes;
        lblModifiedBy.Size = new Size(277, 26);
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
        lblModifiedAt.Location = new Point(852, 36);
        lblModifiedAt.Margin = new Padding(2);
        lblModifiedAt.MinimumSize = new Size(0, 26);
        lblModifiedAt.Name = "lblModifiedAt";
        lblModifiedAt.Padding = new Padding(6, 3, 6, 3);
        lblModifiedAt.RightToLeft = RightToLeft.Yes;
        lblModifiedAt.Size = new Size(277, 26);
        lblModifiedAt.TabIndex = 6;
        lblModifiedAt.Text = "تاريخ التعديل: —";
        lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblModifiedDevice
        // 
        lblModifiedDevice.AutoSize = true;
        lblModifiedDevice.Dock = DockStyle.Fill;
        lblModifiedDevice.Location = new Point(571, 36);
        lblModifiedDevice.Margin = new Padding(2);
        lblModifiedDevice.MinimumSize = new Size(0, 26);
        lblModifiedDevice.Name = "lblModifiedDevice";
        lblModifiedDevice.Padding = new Padding(6, 3, 6, 3);
        lblModifiedDevice.RightToLeft = RightToLeft.Yes;
        lblModifiedDevice.Size = new Size(277, 26);
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
        lblLastPrintedAt.Location = new Point(290, 36);
        lblLastPrintedAt.Margin = new Padding(2);
        lblLastPrintedAt.MinimumSize = new Size(0, 26);
        lblLastPrintedAt.Name = "lblLastPrintedAt";
        lblLastPrintedAt.Padding = new Padding(6, 3, 6, 3);
        lblLastPrintedAt.RightToLeft = RightToLeft.Yes;
        lblLastPrintedAt.Size = new Size(277, 26);
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
        lblEditCount.Size = new Size(280, 26);
        lblEditCount.TabIndex = 9;
        lblEditCount.Text = "عدد التعديلات: —";
        lblEditCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // documentDefaults
        // 
        documentDefaults.AutoScroll = true;
        documentDefaults.Controls.Add(documentDefaultsLayout);
        documentDefaults.Controls.Add(btnReceiptSettings);
        documentDefaults.Location = new Point(4, 23);
        documentDefaults.Name = "documentDefaults";
        documentDefaults.Size = new Size(1404, 267);
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
        documentDefaultsLayout.Size = new Size(1404, 80);
        documentDefaultsLayout.TabIndex = 0;
        // 
        // lblDefaultCurrency
        // 
        lblDefaultCurrency.AutoSize = true;
        lblDefaultCurrency.Location = new Point(1352, 12);
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
        cboDefaultCurrency.Size = new Size(1234, 22);
        cboDefaultCurrency.TabIndex = 1;
        // 
        // lblDefaultCostCenter
        // 
        lblDefaultCostCenter.AutoSize = true;
        lblDefaultCostCenter.Location = new Point(1323, 40);
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
        cboDefaultCostCenter.Size = new Size(1234, 22);
        cboDefaultCostCenter.TabIndex = 3;
        // 
        // btnReceiptSettings
        // 
        btnReceiptSettings.AutoSize = true;
        btnReceiptSettings.Dock = DockStyle.Bottom;
        btnReceiptSettings.Enabled = false;
        btnReceiptSettings.Location = new Point(0, 242);
        btnReceiptSettings.Name = "btnReceiptSettings";
        btnReceiptSettings.Size = new Size(1404, 25);
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
        documentImport.Size = new Size(1404, 267);
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
        documentImportLayout.Size = new Size(1404, 267);
        documentImportLayout.TabIndex = 0;
        // 
        // documentImportFormat
        // 
        documentImportFormat.Dock = DockStyle.Fill;
        documentImportFormat.Location = new Point(3, 0);
        documentImportFormat.Name = "documentImportFormat";
        documentImportFormat.Size = new Size(1398, 28);
        documentImportFormat.TabIndex = 0;
        documentImportFormat.Text = "نوع الملف: CSV (UTF-8)";
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
        documentImportButtons.Size = new Size(1398, 36);
        documentImportButtons.TabIndex = 1;
        // 
        // documentImportPath
        // 
        documentImportPath.Location = new Point(1035, 3);
        documentImportPath.Name = "documentImportPath";
        documentImportPath.ReadOnly = true;
        documentImportPath.Size = new Size(360, 22);
        documentImportPath.TabIndex = 0;
        documentImportPath.TabStop = false;
        // 
        // documentImportChoose
        // 
        documentImportChoose.AutoSize = true;
        documentImportChoose.Location = new Point(892, 3);
        documentImportChoose.Name = "documentImportChoose";
        documentImportChoose.Size = new Size(137, 30);
        documentImportChoose.TabIndex = 1;
        documentImportChoose.Text = "اختيار ملف ومعاينة";
        // 
        // documentImportApply
        // 
        documentImportApply.AutoSize = true;
        documentImportApply.Location = new Point(743, 3);
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
        documentImportPreview.Size = new Size(1398, 191);
        documentImportPreview.TabIndex = 2;
        // 
        // referenceHeader
        // 
        referenceHeader.AutoSize = true;
        referenceHeader.BackColor = Color.FromArgb(245, 245, 245);
        referenceHeader.ColumnCount = 8;
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 212F));
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
        referenceHeader.Controls.Add(label1, 0, 0);
        referenceHeader.Controls.Add(textBox1, 1, 0);
        referenceHeader.Controls.Add(lbl_operation, 4, 0);
        referenceHeader.Controls.Add(field_operation, 5, 0);
        referenceHeader.Controls.Add(lbl_salesperson, 6, 0);
        referenceHeader.Controls.Add(field_salesperson, 7, 0);
        referenceHeader.Controls.Add(lbl_destinationCashBankRef, 0, 1);
        referenceHeader.Controls.Add(field_destinationCashBankRef, 1, 1);
        referenceHeader.Controls.Add(bankCurrencyGroup, 2, 1);
        referenceHeader.Controls.Add(lbl_collector, 4, 1);
        referenceHeader.Controls.Add(field_collector, 5, 1);
        referenceHeader.Controls.Add(lbl_commission, 6, 1);
        referenceHeader.Controls.Add(field_commission, 7, 1);
        referenceHeader.Controls.Add(lbl_voucherNumber, 0, 2);
        referenceHeader.Controls.Add(field_voucherNumber, 1, 2);
        referenceHeader.Controls.Add(lbl_voucherDate, 2, 2);
        referenceHeader.Controls.Add(field_voucherDate, 3, 2);
        referenceHeader.Controls.Add(lbl_attachmentCount, 4, 2);
        referenceHeader.Controls.Add(field_attachmentCount, 5, 2);
        referenceHeader.Controls.Add(lbl_voucherType, 6, 2);
        referenceHeader.Controls.Add(field_voucherType, 7, 2);
        referenceHeader.Controls.Add(lbl_amount, 0, 3);
        referenceHeader.Controls.Add(field_amount, 1, 3);
        referenceHeader.Controls.Add(lbl_exchangeRate, 2, 3);
        referenceHeader.Controls.Add(field_exchangeRate, 3, 3);
        referenceHeader.Controls.Add(lbl_foreignReceipt, 4, 3);
        referenceHeader.Controls.Add(field_foreignReceipt, 5, 3);
        referenceHeader.Controls.Add(referenceLabel15, 6, 3);
        referenceHeader.Controls.Add(referenceField15, 7, 3);
        referenceHeader.Controls.Add(lbl_partyRef, 0, 4);
        referenceHeader.Controls.Add(field_partyRef, 1, 4);
        referenceHeader.Controls.Add(lbl_referenceNumber, 2, 4);
        referenceHeader.Controls.Add(field_referenceNumber, 3, 4);
        referenceHeader.Controls.Add(lbl_referenceName, 4, 4);
        referenceHeader.Controls.Add(field_referenceName, 5, 4);
        referenceHeader.Controls.Add(lbl_description, 0, 5);
        referenceHeader.Controls.Add(field_description, 1, 5);
        referenceHeader.Controls.Add(lbl_postingMethod, 0, 6);
        referenceHeader.Controls.Add(field_postingMethod, 1, 6);
        referenceHeader.Controls.Add(lbl_importSource, 2, 6);
        referenceHeader.Controls.Add(field_importSource, 3, 6);
        referenceHeader.Controls.Add(lbl_importNumber, 6, 6);
        referenceHeader.Controls.Add(field_importNumber, 7, 6);
        referenceHeader.Controls.Add(lblAmountWords, 0, 7);
        referenceHeader.Dock = DockStyle.Fill;
        referenceHeader.Location = new Point(0, 0);
        referenceHeader.Name = "referenceHeader";
        referenceHeader.Padding = new Padding(2);
        referenceHeader.RightToLeft = RightToLeft.Yes;
        referenceHeader.RowCount = 8;
        referenceHeader.RowStyles.Add(new RowStyle());
        referenceHeader.RowStyles.Add(new RowStyle());
        referenceHeader.RowStyles.Add(new RowStyle());
        referenceHeader.RowStyles.Add(new RowStyle());
        referenceHeader.RowStyles.Add(new RowStyle());
        referenceHeader.RowStyles.Add(new RowStyle());
        referenceHeader.RowStyles.Add(new RowStyle());
        referenceHeader.RowStyles.Add(new RowStyle());
        referenceHeader.Size = new Size(1404, 267);
        referenceHeader.TabIndex = 1;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Dock = DockStyle.Fill;
        label1.Font = new Font("Tahoma", 8.25F);
        label1.Location = new Point(1303, 3);
        label1.Margin = new Padding(1);
        label1.Name = "label1";
        label1.Size = new Size(98, 24);
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
        textBox1.Location = new Point(630, 4);
        textBox1.Margin = new Padding(1, 2, 1, 2);
        textBox1.Name = "textBox1";
        textBox1.Size = new Size(671, 22);
        textBox1.TabIndex = 1;
        textBox1.Tag = "ACC-043:branchScope";
        receiptHints.SetToolTip(textBox1, "فروع مسموحة للشركة والفترة الحالية؛ التغيير في مسودة جديدة نظيفة فقط");
        // 
        // lbl_destinationCashBankRef
        // 
        lbl_destinationCashBankRef.AutoSize = true;
        lbl_destinationCashBankRef.Dock = DockStyle.Fill;
        lbl_destinationCashBankRef.Font = new Font("Tahoma", 8.25F);
        lbl_destinationCashBankRef.Location = new Point(1303, 29);
        lbl_destinationCashBankRef.Margin = new Padding(1);
        lbl_destinationCashBankRef.Name = "lbl_destinationCashBankRef";
        lbl_destinationCashBankRef.Size = new Size(98, 26);
        lbl_destinationCashBankRef.TabIndex = 6;
        lbl_destinationCashBankRef.Text = "رقم البنك / الصندوق";
        lbl_destinationCashBankRef.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_destinationCashBankRef
        // 
        field_destinationCashBankRef.AccessibleName = "الصندوق/البنك الوجهة";
        field_destinationCashBankRef.BackColor = Color.LightYellow;
        field_destinationCashBankRef.Dock = DockStyle.Fill;
        field_destinationCashBankRef.DropDownStyle = ComboBoxStyle.DropDownList;
        field_destinationCashBankRef.DropDownWidth = 420;
        field_destinationCashBankRef.Font = new Font("Tahoma", 9.25F);
        field_destinationCashBankRef.Location = new Point(1037, 30);
        field_destinationCashBankRef.Margin = new Padding(1, 2, 1, 2);
        field_destinationCashBankRef.Name = "field_destinationCashBankRef";
        field_destinationCashBankRef.Size = new Size(264, 22);
        field_destinationCashBankRef.TabIndex = 7;
        field_destinationCashBankRef.Tag = "ACC-043:destinationCashBankRef";
        // 
        // lbl_collector
        // 
        lbl_collector.AutoSize = true;
        lbl_collector.Dock = DockStyle.Fill;
        lbl_collector.Font = new Font("Tahoma", 8.25F);
        lbl_collector.Location = new Point(530, 29);
        lbl_collector.Margin = new Padding(1);
        lbl_collector.Name = "lbl_collector";
        lbl_collector.Size = new Size(98, 26);
        lbl_collector.TabIndex = 9;
        lbl_collector.Text = "رقم المحصل";
        lbl_collector.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_collector
        // 
        field_collector.AccessibleName = "رقم المحصل";
        field_collector.BackColor = Color.White;
        field_collector.Dock = DockStyle.Fill;
        field_collector.DropDownStyle = ComboBoxStyle.DropDownList;
        field_collector.DropDownWidth = 320;
        field_collector.Font = new Font("Tahoma", 9.25F);
        field_collector.Location = new Point(317, 30);
        field_collector.Margin = new Padding(1, 2, 1, 2);
        field_collector.Name = "field_collector";
        field_collector.Size = new Size(211, 22);
        field_collector.TabIndex = 10;
        receiptHints.SetToolTip(field_collector, "حقل مرجعي؛ يحتاج ربط بيانات السند قبل الإدخال");
        // 
        // lbl_commission
        // 
        lbl_commission.AutoSize = true;
        lbl_commission.Dock = DockStyle.Fill;
        lbl_commission.Font = new Font("Tahoma", 8.25F);
        lbl_commission.Location = new Point(217, 29);
        lbl_commission.Margin = new Padding(1);
        lbl_commission.Name = "lbl_commission";
        lbl_commission.Size = new Size(98, 26);
        lbl_commission.TabIndex = 11;
        lbl_commission.Text = "نسبة العمولة";
        lbl_commission.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_commission
        // 
        field_commission.AccessibleName = "نسبة العمولة";
        field_commission.BackColor = Color.White;
        field_commission.Dock = DockStyle.Fill;
        field_commission.Font = new Font("Tahoma", 9.25F);
        field_commission.Location = new Point(3, 30);
        field_commission.Margin = new Padding(1, 2, 1, 2);
        field_commission.Name = "field_commission";
        field_commission.ReadOnly = true;
        field_commission.Size = new Size(212, 22);
        field_commission.TabIndex = 12;
        field_commission.TabStop = false;
        receiptHints.SetToolTip(field_commission, "حقل مرجعي؛ يحتاج ربط بيانات السند قبل الإدخال");
        // 
        // lbl_voucherNumber
        // 
        lbl_voucherNumber.AutoSize = true;
        lbl_voucherNumber.Dock = DockStyle.Fill;
        lbl_voucherNumber.Font = new Font("Tahoma", 8.25F);
        lbl_voucherNumber.Location = new Point(1303, 57);
        lbl_voucherNumber.Margin = new Padding(1);
        lbl_voucherNumber.Name = "lbl_voucherNumber";
        lbl_voucherNumber.Size = new Size(98, 24);
        lbl_voucherNumber.TabIndex = 13;
        lbl_voucherNumber.Text = "رقم السند";
        lbl_voucherNumber.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_voucherNumber
        // 
        field_voucherNumber.AccessibleName = "رقم السند";
        field_voucherNumber.BackColor = Color.LightYellow;
        field_voucherNumber.Dock = DockStyle.Fill;
        field_voucherNumber.Font = new Font("Tahoma", 9.25F);
        field_voucherNumber.Location = new Point(1037, 58);
        field_voucherNumber.Margin = new Padding(1, 2, 1, 2);
        field_voucherNumber.Name = "field_voucherNumber";
        field_voucherNumber.ReadOnly = true;
        field_voucherNumber.Size = new Size(264, 22);
        field_voucherNumber.TabIndex = 14;
        field_voucherNumber.TabStop = false;
        field_voucherNumber.Tag = "ACC-043:voucherNumber";
        // 
        // lbl_voucherDate
        // 
        lbl_voucherDate.AutoSize = true;
        lbl_voucherDate.Dock = DockStyle.Fill;
        lbl_voucherDate.Font = new Font("Tahoma", 8.25F);
        lbl_voucherDate.Location = new Point(825, 57);
        lbl_voucherDate.Margin = new Padding(1);
        lbl_voucherDate.Name = "lbl_voucherDate";
        lbl_voucherDate.Size = new Size(210, 24);
        lbl_voucherDate.TabIndex = 15;
        lbl_voucherDate.Text = "التاريخ";
        lbl_voucherDate.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_voucherDate
        // 
        field_voucherDate.AccessibleName = "التاريخ";
        field_voucherDate.BackColor = Color.LightYellow;
        field_voucherDate.Checked = false;
        field_voucherDate.CustomFormat = "yyyy-MM-dd";
        field_voucherDate.Dock = DockStyle.Fill;
        field_voucherDate.Font = new Font("Tahoma", 9.25F);
        field_voucherDate.Format = DateTimePickerFormat.Custom;
        field_voucherDate.Location = new Point(630, 58);
        field_voucherDate.Margin = new Padding(1, 2, 1, 2);
        field_voucherDate.Name = "field_voucherDate";
        field_voucherDate.ShowCheckBox = true;
        field_voucherDate.Size = new Size(193, 22);
        field_voucherDate.TabIndex = 16;
        field_voucherDate.Tag = "ACC-043:voucherDate";
        // 
        // lbl_attachmentCount
        // 
        lbl_attachmentCount.AutoSize = true;
        lbl_attachmentCount.Dock = DockStyle.Fill;
        lbl_attachmentCount.Font = new Font("Tahoma", 8.25F);
        lbl_attachmentCount.Location = new Point(530, 57);
        lbl_attachmentCount.Margin = new Padding(1);
        lbl_attachmentCount.Name = "lbl_attachmentCount";
        lbl_attachmentCount.Size = new Size(98, 24);
        lbl_attachmentCount.TabIndex = 17;
        lbl_attachmentCount.Text = "عدد المرفقات";
        lbl_attachmentCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_attachmentCount
        // 
        field_attachmentCount.AccessibleName = "عدد المرفقات";
        field_attachmentCount.BackColor = Color.White;
        field_attachmentCount.Dock = DockStyle.Fill;
        field_attachmentCount.Font = new Font("Tahoma", 9.25F);
        field_attachmentCount.Location = new Point(317, 58);
        field_attachmentCount.Margin = new Padding(1, 2, 1, 2);
        field_attachmentCount.Name = "field_attachmentCount";
        field_attachmentCount.ReadOnly = true;
        field_attachmentCount.Size = new Size(211, 22);
        field_attachmentCount.TabIndex = 18;
        field_attachmentCount.TabStop = false;
        receiptHints.SetToolTip(field_attachmentCount, "حقل مرجعي؛ يحتاج ربط بيانات السند قبل الإدخال");
        // 
        // lbl_voucherType
        // 
        lbl_voucherType.AutoSize = true;
        lbl_voucherType.Dock = DockStyle.Fill;
        lbl_voucherType.Font = new Font("Tahoma", 8.25F);
        lbl_voucherType.Location = new Point(217, 57);
        lbl_voucherType.Margin = new Padding(1);
        lbl_voucherType.Name = "lbl_voucherType";
        lbl_voucherType.Size = new Size(98, 24);
        lbl_voucherType.TabIndex = 19;
        lbl_voucherType.Text = "نوع السند";
        lbl_voucherType.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_voucherType
        // 
        field_voucherType.AccessibleName = "نوع السند";
        field_voucherType.BackColor = Color.LightYellow;
        field_voucherType.Dock = DockStyle.Fill;
        field_voucherType.DropDownStyle = ComboBoxStyle.DropDownList;
        field_voucherType.DropDownWidth = 320;
        field_voucherType.FlatStyle = FlatStyle.Flat;
        field_voucherType.Font = new Font("Tahoma", 9.25F);
        field_voucherType.Location = new Point(3, 58);
        field_voucherType.Margin = new Padding(1, 2, 1, 2);
        field_voucherType.Name = "field_voucherType";
        field_voucherType.Size = new Size(212, 22);
        field_voucherType.TabIndex = 20;
        receiptHints.SetToolTip(field_voucherType, "حقل مرجعي؛ يحتاج ربط بيانات السند قبل الإدخال");
        // 
        // lbl_amount
        // 
        lbl_amount.AutoSize = true;
        lbl_amount.Dock = DockStyle.Fill;
        lbl_amount.Font = new Font("Tahoma", 8.25F);
        lbl_amount.Location = new Point(1303, 83);
        lbl_amount.Margin = new Padding(1);
        lbl_amount.Name = "lbl_amount";
        lbl_amount.Size = new Size(98, 24);
        lbl_amount.TabIndex = 21;
        lbl_amount.Text = "المبلغ";
        lbl_amount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_amount
        // 
        field_amount.AccessibleName = "المبلغ";
        field_amount.BackColor = Color.LightYellow;
        field_amount.Dock = DockStyle.Fill;
        field_amount.Font = new Font("Tahoma", 9.25F);
        field_amount.Location = new Point(1037, 84);
        field_amount.Margin = new Padding(1, 2, 1, 2);
        field_amount.Name = "field_amount";
        field_amount.RightToLeft = RightToLeft.No;
        field_amount.Size = new Size(264, 22);
        field_amount.TabIndex = 22;
        field_amount.Tag = "ACC-043:amount";
        field_amount.TextAlign = HorizontalAlignment.Right;
        // 
        // lbl_exchangeRate
        // 
        lbl_exchangeRate.AutoSize = true;
        lbl_exchangeRate.Dock = DockStyle.Fill;
        lbl_exchangeRate.Font = new Font("Tahoma", 8.25F);
        lbl_exchangeRate.Location = new Point(825, 83);
        lbl_exchangeRate.Margin = new Padding(1);
        lbl_exchangeRate.Name = "lbl_exchangeRate";
        lbl_exchangeRate.Size = new Size(210, 24);
        lbl_exchangeRate.TabIndex = 23;
        lbl_exchangeRate.Text = "سعر الصرف";
        lbl_exchangeRate.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_exchangeRate
        // 
        field_exchangeRate.AccessibleName = "سعر الصرف";
        field_exchangeRate.BackColor = Color.LightYellow;
        field_exchangeRate.Dock = DockStyle.Fill;
        field_exchangeRate.Font = new Font("Tahoma", 9.25F);
        field_exchangeRate.Location = new Point(630, 84);
        field_exchangeRate.Margin = new Padding(1, 2, 1, 2);
        field_exchangeRate.Name = "field_exchangeRate";
        field_exchangeRate.RightToLeft = RightToLeft.No;
        field_exchangeRate.Size = new Size(193, 22);
        field_exchangeRate.TabIndex = 24;
        field_exchangeRate.Tag = "ACC-043:exchangeRate";
        field_exchangeRate.TextAlign = HorizontalAlignment.Right;
        // 
        // referenceLabel15
        // 
        referenceLabel15.AutoSize = true;
        referenceLabel15.Dock = DockStyle.Fill;
        referenceLabel15.Font = new Font("Tahoma", 8.25F);
        referenceLabel15.Location = new Point(217, 83);
        referenceLabel15.Margin = new Padding(1);
        referenceLabel15.Name = "referenceLabel15";
        referenceLabel15.Size = new Size(98, 24);
        referenceLabel15.TabIndex = 27;
        referenceLabel15.Text = "رقم المركز";
        referenceLabel15.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField15
        // 
        referenceField15.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField15.BackColor = Color.White;
        referenceField15.Dock = DockStyle.Fill;
        referenceField15.DropDownStyle = ComboBoxStyle.DropDownList;
        referenceField15.Font = new Font("Tahoma", 9.25F);
        referenceField15.Location = new Point(3, 84);
        referenceField15.Margin = new Padding(1, 2, 1, 2);
        referenceField15.Name = "referenceField15";
        referenceField15.Size = new Size(212, 22);
        referenceField15.TabIndex = 28;
        referenceField15.TabStop = false;
        // 
        // lbl_partyRef
        // 
        lbl_partyRef.AutoSize = true;
        lbl_partyRef.Dock = DockStyle.Fill;
        lbl_partyRef.Font = new Font("Tahoma", 8.25F);
        lbl_partyRef.Location = new Point(1303, 109);
        lbl_partyRef.Margin = new Padding(1);
        lbl_partyRef.Name = "lbl_partyRef";
        lbl_partyRef.Size = new Size(98, 24);
        lbl_partyRef.TabIndex = 29;
        lbl_partyRef.Text = "المستلم";
        lbl_partyRef.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_partyRef
        // 
        field_partyRef.AccessibleName = "الطرف";
        field_partyRef.BackColor = Color.White;
        field_partyRef.Dock = DockStyle.Fill;
        field_partyRef.DropDownStyle = ComboBoxStyle.DropDownList;
        field_partyRef.DropDownWidth = 420;
        field_partyRef.Font = new Font("Tahoma", 9.25F);
        field_partyRef.Location = new Point(1037, 110);
        field_partyRef.Margin = new Padding(1, 2, 1, 2);
        field_partyRef.Name = "field_partyRef";
        field_partyRef.Size = new Size(264, 22);
        field_partyRef.TabIndex = 30;
        field_partyRef.Tag = "ACC-043:partyRef";
        // 
        // lbl_referenceNumber
        // 
        lbl_referenceNumber.AutoSize = true;
        lbl_referenceNumber.Dock = DockStyle.Fill;
        lbl_referenceNumber.Font = new Font("Tahoma", 8.25F);
        lbl_referenceNumber.Location = new Point(825, 109);
        lbl_referenceNumber.Margin = new Padding(1);
        lbl_referenceNumber.Name = "lbl_referenceNumber";
        lbl_referenceNumber.Size = new Size(210, 24);
        lbl_referenceNumber.TabIndex = 31;
        lbl_referenceNumber.Text = "رقم المرجع";
        lbl_referenceNumber.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_referenceNumber
        // 
        field_referenceNumber.AccessibleName = "رقم المرجع";
        field_referenceNumber.BackColor = Color.White;
        field_referenceNumber.Dock = DockStyle.Fill;
        field_referenceNumber.Font = new Font("Tahoma", 9.25F);
        field_referenceNumber.Location = new Point(630, 110);
        field_referenceNumber.Margin = new Padding(1, 2, 1, 2);
        field_referenceNumber.Name = "field_referenceNumber";
        field_referenceNumber.ReadOnly = true;
        field_referenceNumber.Size = new Size(193, 22);
        field_referenceNumber.TabIndex = 32;
        field_referenceNumber.TabStop = false;
        receiptHints.SetToolTip(field_referenceNumber, "حقل مرجعي؛ يحتاج ربط بيانات السند قبل الإدخال");
        // 
        // lbl_referenceName
        // 
        lbl_referenceName.AutoSize = true;
        lbl_referenceName.Dock = DockStyle.Fill;
        lbl_referenceName.Font = new Font("Tahoma", 8.25F);
        lbl_referenceName.Location = new Point(530, 109);
        lbl_referenceName.Margin = new Padding(1);
        lbl_referenceName.Name = "lbl_referenceName";
        lbl_referenceName.Size = new Size(98, 24);
        lbl_referenceName.TabIndex = 33;
        lbl_referenceName.Text = "اسم المرجع";
        lbl_referenceName.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_referenceName
        // 
        field_referenceName.AccessibleName = "اسم المرجع";
        field_referenceName.BackColor = Color.White;
        field_referenceName.Dock = DockStyle.Fill;
        field_referenceName.Font = new Font("Tahoma", 9.25F);
        field_referenceName.Location = new Point(317, 110);
        field_referenceName.Margin = new Padding(1, 2, 1, 2);
        field_referenceName.Name = "field_referenceName";
        field_referenceName.ReadOnly = true;
        field_referenceName.Size = new Size(211, 22);
        field_referenceName.TabIndex = 34;
        field_referenceName.TabStop = false;
        receiptHints.SetToolTip(field_referenceName, "حقل مرجعي؛ يحتاج ربط بيانات السند قبل الإدخال");
        // 
        // lbl_description
        // 
        lbl_description.AutoSize = true;
        lbl_description.Dock = DockStyle.Fill;
        lbl_description.Font = new Font("Tahoma", 8.25F);
        lbl_description.Location = new Point(1303, 135);
        lbl_description.Margin = new Padding(1);
        lbl_description.Name = "lbl_description";
        lbl_description.Size = new Size(98, 24);
        lbl_description.TabIndex = 35;
        lbl_description.Text = "البيان";
        lbl_description.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceAudit
        // 
        referenceAudit.BackColor = Color.FromArgb(225, 223, 247);
        referenceAudit.Dock = DockStyle.Fill;
        referenceAudit.Location = new Point(11, 770);
        referenceAudit.Name = "referenceAudit";
        referenceAudit.Size = new Size(1402, 48);
        referenceAudit.TabIndex = 6;
        referenceAudit.Text = "مدخل السجل: —    تاريخ الإدخال: —    الجهاز: —\r\nمعدل السجل: —    تاريخ التعديل: —    مرات التعديل: —    مرات الطباعة: —";
        referenceAudit.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceTotals
        // 
        referenceTotals.BackColor = Color.FromArgb(250, 223, 249);
        referenceTotals.Dock = DockStyle.Fill;
        referenceTotals.Location = new Point(11, 738);
        referenceTotals.Name = "referenceTotals";
        referenceTotals.Size = new Size(1402, 32);
        referenceTotals.TabIndex = 5;
        referenceTotals.Text = "الإجمالي: —          الفارق: —";
        referenceTotals.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // validationErrors
        // 
        validationErrors.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        validationErrors.ContainerControl = this;
        // 
        // mainLayout
        // 
        mainLayout.BackColor = Color.FromArgb(250, 249, 240);
        mainLayout.ColumnCount = 1;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        mainLayout.Controls.Add(dgvLines, 0, 2);
        mainLayout.Controls.Add(designerCommandBar, 0, 0);
        mainLayout.Controls.Add(tabs, 0, 1);
        mainLayout.Controls.Add(linesHost, 0, 3);
        mainLayout.Controls.Add(tlpAuditInfo, 0, 4);
        mainLayout.Controls.Add(lblStatus, 0, 5);
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Location = new Point(8, 8);
        mainLayout.Margin = new Padding(2);
        mainLayout.MinimumSize = new Size(980, 0);
        mainLayout.Name = "mainLayout";
        mainLayout.Padding = new Padding(4);
        mainLayout.RightToLeft = RightToLeft.Yes;
        mainLayout.RowCount = 6;
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 47F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 298F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.Size = new Size(1424, 924);
        mainLayout.TabIndex = 5;
        // 
        // dgvLines
        // 
        dgvLines.AccessibleName = "التفاصيل والحركات";
        dgvLines.AllowUserToAddRows = false;
        dgvLines.AllowUserToDeleteRows = false;
        dataGridViewCellStyle3.BackColor = Color.White;
        dgvLines.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
        dgvLines.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
        dgvLines.BackgroundColor = Color.White;
        dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle4.BackColor = Color.FromArgb(232, 232, 232);
        dataGridViewCellStyle4.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle4.ForeColor = Color.FromArgb(34, 66, 96);
        dataGridViewCellStyle4.Padding = new Padding(3, 6, 3, 6);
        dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
        dgvLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
        dgvLines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvLines.Columns.AddRange(new DataGridViewColumn[] { col_rowNo, col_counterAccountRef, col_analyticalAccount, col_accountName, col_lineDescription, col_currencyRef, col_exchangeRate, col_amount, col_foreignAmount, col_costCenter, col_referenceNumber, col_salespersonNumber, col_cashierNumber, col_chequeNumber, col_chequeDueDate, col_partyRef, col_accountingAmount, col_project, col_activity, receiptWaybillColumn });
        dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle13.BackColor = Color.White;
        dataGridViewCellStyle13.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle13.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle13.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle13.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle13.WrapMode = DataGridViewTriState.False;
        dgvLines.DefaultCellStyle = dataGridViewCellStyle13;
        dgvLines.Dock = DockStyle.Fill;
        dgvLines.EnableHeadersVisualStyles = false;
        dgvLines.GridColor = Color.FromArgb(207, 216, 224);
        dgvLines.Location = new Point(6, 351);
        dgvLines.Margin = new Padding(2);
        dgvLines.MinimumSize = new Size(900, 80);
        dgvLines.MultiSelect = false;
        dgvLines.Name = "dgvLines";
        dgvLines.RightToLeft = RightToLeft.Yes;
        dgvLines.RowHeadersVisible = false;
        dgvLines.RowHeadersWidth = 51;
        dgvLines.RowTemplate.Height = 27;
        dgvLines.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvLines.Size = new Size(1412, 439);
        dgvLines.TabIndex = 6;
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
        // col_counterAccountRef
        // 
        col_counterAccountRef.HeaderText = "رقم الحساب";
        col_counterAccountRef.MinimumWidth = 90;
        col_counterAccountRef.Name = "col_counterAccountRef";
        col_counterAccountRef.Width = 120;
        // 
        // col_analyticalAccount
        // 
        col_analyticalAccount.HeaderText = "الحساب التحليلي";
        col_analyticalAccount.MinimumWidth = 80;
        col_analyticalAccount.Name = "col_analyticalAccount";
        col_analyticalAccount.ReadOnly = true;
        col_analyticalAccount.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_analyticalAccount.ToolTipText = "للعرض؛ يحتاج ربط بيانات السند";
        col_analyticalAccount.Width = 105;
        // 
        // col_accountName
        // 
        dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
        col_accountName.DefaultCellStyle = dataGridViewCellStyle5;
        col_accountName.HeaderText = "الاسم";
        col_accountName.MinimumWidth = 80;
        col_accountName.Name = "col_accountName";
        col_accountName.ReadOnly = true;
        col_accountName.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_accountName.ToolTipText = "للعرض؛ يحتاج ربط بيانات السند";
        col_accountName.Width = 190;
        // 
        // col_lineDescription
        // 
        col_lineDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
        col_lineDescription.DefaultCellStyle = dataGridViewCellStyle6;
        col_lineDescription.HeaderText = "البيان";
        col_lineDescription.MinimumWidth = 90;
        col_lineDescription.Name = "col_lineDescription";
        col_lineDescription.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_lineDescription.Width = 180;
        // 
        // col_currencyRef
        // 
        col_currencyRef.DropDownWidth = 200;
        col_currencyRef.HeaderText = "العملة";
        col_currencyRef.MinimumWidth = 90;
        col_currencyRef.Name = "col_currencyRef";
        col_currencyRef.Width = 140;
        // 
        // col_exchangeRate
        // 
        dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle7.Format = "0.############################";
        dataGridViewCellStyle7.WrapMode = DataGridViewTriState.False;
        col_exchangeRate.DefaultCellStyle = dataGridViewCellStyle7;
        col_exchangeRate.HeaderText = "سعر الصرف";
        col_exchangeRate.MinimumWidth = 90;
        col_exchangeRate.Name = "col_exchangeRate";
        col_exchangeRate.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_exchangeRate.Width = 90;
        // 
        // col_amount
        // 
        dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle8.Format = "0.00##########################";
        dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
        col_amount.DefaultCellStyle = dataGridViewCellStyle8;
        col_amount.HeaderText = "المبلغ";
        col_amount.MinimumWidth = 90;
        col_amount.Name = "col_amount";
        col_amount.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_amount.Width = 120;
        // 
        // col_foreignAmount
        // 
        dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle9.Format = "0.00##########################";
        dataGridViewCellStyle9.WrapMode = DataGridViewTriState.False;
        col_foreignAmount.DefaultCellStyle = dataGridViewCellStyle9;
        col_foreignAmount.HeaderText = "مدين أجنبي";
        col_foreignAmount.MinimumWidth = 80;
        col_foreignAmount.Name = "col_foreignAmount";
        col_foreignAmount.ReadOnly = true;
        col_foreignAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_foreignAmount.ToolTipText = "للعرض؛ يحتاج ربط بيانات السند";
        col_foreignAmount.Width = 115;
        // 
        // col_costCenter
        // 
        col_costCenter.HeaderText = "رقم المركز";
        col_costCenter.MinimumWidth = 80;
        col_costCenter.Name = "col_costCenter";
        col_costCenter.ReadOnly = true;
        col_costCenter.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_costCenter.ToolTipText = "للعرض؛ يحتاج ربط بيانات السند";
        col_costCenter.Width = 90;
        // 
        // col_referenceNumber
        // 
        col_referenceNumber.HeaderText = "رقم المرجع";
        col_referenceNumber.MinimumWidth = 6;
        col_referenceNumber.Name = "col_referenceNumber";
        col_referenceNumber.Width = 90;
        // 
        // col_salespersonNumber
        // 
        col_salespersonNumber.HeaderText = "رقم المندوب";
        col_salespersonNumber.MinimumWidth = 6;
        col_salespersonNumber.Name = "col_salespersonNumber";
        col_salespersonNumber.ReadOnly = true;
        col_salespersonNumber.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_salespersonNumber.ToolTipText = "للعرض؛ يحتاج ربط بيانات تفاصيل السند";
        col_salespersonNumber.Width = 95;
        // 
        // col_cashierNumber
        // 
        col_cashierNumber.HeaderText = "رقم الكاشير";
        col_cashierNumber.MinimumWidth = 6;
        col_cashierNumber.Name = "col_cashierNumber";
        col_cashierNumber.ReadOnly = true;
        col_cashierNumber.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_cashierNumber.ToolTipText = "للعرض؛ يحتاج ربط بيانات تفاصيل السند";
        col_cashierNumber.Width = 95;
        // 
        // col_chequeNumber
        // 
        dataGridViewCellStyle10.BackColor = Color.White;
        col_chequeNumber.DefaultCellStyle = dataGridViewCellStyle10;
        col_chequeNumber.HeaderText = "رقم الشيك";
        col_chequeNumber.MinimumWidth = 6;
        col_chequeNumber.Name = "col_chequeNumber";
        col_chequeNumber.Width = 95;
        // 
        // col_chequeDueDate
        // 
        dataGridViewCellStyle11.BackColor = Color.White;
        col_chequeDueDate.DefaultCellStyle = dataGridViewCellStyle11;
        col_chequeDueDate.HeaderText = "تاريخ الاستحقاق";
        col_chequeDueDate.MinimumWidth = 6;
        col_chequeDueDate.Name = "col_chequeDueDate";
        col_chequeDueDate.ToolTipText = "yyyy-MM-dd";
        col_chequeDueDate.Width = 115;
        // 
        // col_partyRef
        // 
        col_partyRef.HeaderText = "الطرف/الجهة";
        col_partyRef.MinimumWidth = 90;
        col_partyRef.Name = "col_partyRef";
        col_partyRef.Width = 140;
        // 
        // col_accountingAmount
        // 
        dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle12.Format = "0.00##########################";
        dataGridViewCellStyle12.WrapMode = DataGridViewTriState.False;
        col_accountingAmount.DefaultCellStyle = dataGridViewCellStyle12;
        col_accountingAmount.HeaderText = "المبلغ المحاسبي";
        col_accountingAmount.MinimumWidth = 90;
        col_accountingAmount.Name = "col_accountingAmount";
        col_accountingAmount.ReadOnly = true;
        col_accountingAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_accountingAmount.Width = 140;
        // 
        // col_project
        // 
        col_project.HeaderText = "المشروع";
        col_project.MinimumWidth = 80;
        col_project.Name = "col_project";
        col_project.ReadOnly = true;
        col_project.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_project.ToolTipText = "للعرض؛ يحتاج ربط بيانات السند";
        col_project.Width = 90;
        // 
        // col_activity
        // 
        col_activity.HeaderText = "النشاط";
        col_activity.MinimumWidth = 80;
        col_activity.Name = "col_activity";
        col_activity.ReadOnly = true;
        col_activity.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_activity.ToolTipText = "للعرض؛ يحتاج ربط بيانات السند";
        col_activity.Width = 90;
        // 
        // receiptWaybillColumn
        // 
        receiptWaybillColumn.HeaderText = "رقم البوليصة";
        receiptWaybillColumn.Name = "receiptWaybillColumn";
        receiptWaybillColumn.Visible = false;
        receiptWaybillColumn.Width = 160;
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
        flpActions.Location = new Point(6, 6);
        flpActions.Margin = new Padding(2);
        flpActions.Name = "flpActions";
        flpActions.RightToLeft = RightToLeft.Yes;
        flpActions.Size = new Size(1412, 43);
        flpActions.TabIndex = 0;
        flpActions.WrapContents = false;
        // 
        // btnClear
        // 
        btnClear.AccessibleName = "تفريغ المسودة";
        btnClear.AutoSize = true;
        btnClear.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnClear.Location = new Point(1321, 3);
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
        btnView.Enabled = false;
        btnView.Location = new Point(1227, 3);
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
        btnCreate.Enabled = false;
        btnCreate.Location = new Point(1133, 3);
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
        btnEdit.Enabled = false;
        btnEdit.Location = new Point(1039, 3);
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
        btnCancel.Enabled = false;
        btnCancel.Location = new Point(945, 3);
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
        btnPrint.Enabled = false;
        btnPrint.Location = new Point(851, 3);
        btnPrint.MinimumSize = new Size(88, 36);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(88, 36);
        btnPrint.TabIndex = 8;
        btnPrint.Text = "طباعة";
        receiptHints.SetToolTip(btnPrint, "يحتاج ربط قالب طباعة سند القبض");
        // 
        // btnPost
        // 
        btnPost.AccessibleName = "ترحيل";
        btnPost.AutoSize = true;
        btnPost.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnPost.Enabled = false;
        btnPost.Location = new Point(757, 3);
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
        btnReverse.Location = new Point(663, 3);
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
        btnClose.Location = new Point(569, 3);
        btnClose.MinimumSize = new Size(88, 36);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(88, 36);
        btnClose.TabIndex = 7;
        btnClose.Text = "إغلاق";
        // 
        // btnAddRow
        // 
        btnAddRow.Location = new Point(431, 3);
        btnAddRow.MinimumSize = new Size(132, 36);
        btnAddRow.Name = "btnAddRow";
        btnAddRow.Size = new Size(132, 36);
        btnAddRow.TabIndex = 0;
        btnAddRow.Text = "إضافة سطر";
        // 
        // btnRemoveRow
        // 
        btnRemoveRow.Location = new Point(293, 3);
        btnRemoveRow.MinimumSize = new Size(132, 36);
        btnRemoveRow.Name = "btnRemoveRow";
        btnRemoveRow.Size = new Size(132, 36);
        btnRemoveRow.TabIndex = 1;
        btnRemoveRow.Text = "حذف سطر";
        // 
        // tabs
        // 
        tabs.Controls.Add(tp0);
        tabs.Controls.Add(documentAdditional);
        tabs.Controls.Add(documentDefaults);
        tabs.Controls.Add(documentImport);
        tabs.Dock = DockStyle.Fill;
        tabs.Font = new Font("Tahoma", 9F);
        tabs.Location = new Point(6, 53);
        tabs.Margin = new Padding(2);
        tabs.Multiline = true;
        tabs.Name = "tabs";
        tabs.RightToLeft = RightToLeft.Yes;
        tabs.RightToLeftLayout = true;
        tabs.SelectedIndex = 0;
        tabs.Size = new Size(1412, 294);
        tabs.TabIndex = 1;
        // 
        // tp0
        // 
        tp0.BackColor = Color.FromArgb(250, 249, 240);
        tp0.Controls.Add(referenceHeader);
        tp0.Location = new Point(4, 23);
        tp0.Margin = new Padding(4);
        tp0.Name = "tp0";
        tp0.RightToLeft = RightToLeft.Yes;
        tp0.Size = new Size(1404, 267);
        tp0.TabIndex = 0;
        tp0.Text = "البيانات الرئيسية";
        // 
        // linesHost
        // 
        linesHost.AutoSize = true;
        linesHost.ColumnCount = 1;
        linesHost.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        linesHost.Controls.Add(totalsPanel, 0, 0);
        linesHost.Dock = DockStyle.Fill;
        linesHost.Location = new Point(5, 793);
        linesHost.Margin = new Padding(1);
        linesHost.Name = "linesHost";
        linesHost.RowCount = 1;
        linesHost.RowStyles.Add(new RowStyle());
        linesHost.Size = new Size(1414, 40);
        linesHost.TabIndex = 3;
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
        totalsPanel.Controls.Add(label3, 1, 0);
        totalsPanel.Controls.Add(field_total, 2, 0);
        totalsPanel.Controls.Add(label2, 3, 0);
        totalsPanel.Controls.Add(field_difference, 4, 0);
        totalsPanel.Controls.Add(lblTotalsMessage, 0, 0);
        totalsPanel.Dock = DockStyle.Fill;
        totalsPanel.Location = new Point(3, 3);
        totalsPanel.Name = "totalsPanel";
        totalsPanel.Padding = new Padding(6, 3, 6, 3);
        totalsPanel.RightToLeft = RightToLeft.Yes;
        totalsPanel.RowCount = 1;
        totalsPanel.RowStyles.Add(new RowStyle());
        totalsPanel.Size = new Size(1408, 34);
        totalsPanel.TabIndex = 2;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Dock = DockStyle.Fill;
        label3.Location = new Point(787, 3);
        label3.Name = "label3";
        label3.Size = new Size(69, 28);
        label3.TabIndex = 15;
        label3.Text = "المجموع";
        label3.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_total
        // 
        field_total.AccessibleName = "المجموع";
        field_total.BackColor = Color.White;
        field_total.Dock = DockStyle.Fill;
        field_total.Location = new Point(617, 6);
        field_total.Name = "field_total";
        field_total.ReadOnly = true;
        field_total.Size = new Size(164, 22);
        field_total.TabIndex = 14;
        field_total.TabStop = false;
        field_total.Text = "—";
        field_total.TextAlign = HorizontalAlignment.Right;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Dock = DockStyle.Fill;
        label2.Location = new Point(542, 3);
        label2.Name = "label2";
        label2.Size = new Size(69, 28);
        label2.TabIndex = 13;
        label2.Text = "الفارق";
        label2.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_difference
        // 
        field_difference.AccessibleName = "الفارق";
        field_difference.BackColor = Color.White;
        field_difference.Dock = DockStyle.Fill;
        field_difference.Location = new Point(372, 6);
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
        lblTotalsMessage.Location = new Point(865, 6);
        lblTotalsMessage.Margin = new Padding(6, 3, 6, 3);
        lblTotalsMessage.Name = "lblTotalsMessage";
        lblTotalsMessage.RightToLeft = RightToLeft.Yes;
        lblTotalsMessage.Size = new Size(531, 22);
        lblTotalsMessage.TabIndex = 16;
        lblTotalsMessage.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Dock = DockStyle.Bottom;
        lblStatus.Location = new Point(8, 902);
        lblStatus.Margin = new Padding(4, 0, 4, 0);
        lblStatus.Name = "lblStatus";
        lblStatus.Padding = new Padding(4, 2, 4, 2);
        lblStatus.RightToLeft = RightToLeft.Yes;
        lblStatus.Size = new Size(1408, 18);
        lblStatus.TabIndex = 5;
        lblStatus.Text = "مسودة واجهة غير محفوظة — الخدمات غير موصولة";
        // 
        // lblTotals
        // 
        lblTotals.AutoSize = true;
        lblTotals.BackColor = Color.FromArgb(225, 237, 248);
        lblTotals.Dock = DockStyle.Fill;
        lblTotals.Location = new Point(3, 16);
        lblTotals.Name = "lblTotals";
        lblTotals.Padding = new Padding(5);
        lblTotals.Size = new Size(1394, 28);
        lblTotals.TabIndex = 3;
        lblTotals.Text = "المجموع: —        الفارق: —";
        lblTotals.TextAlign = ContentAlignment.MiddleRight;
        // 
        // UcScreen_04_04_01
        // 
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScroll = true;
        BackColor = Color.FromArgb(240, 240, 240);
        Controls.Add(mainLayout);
        Font = new Font("Tahoma", 9F);
        Margin = new Padding(4);
        Name = "UcScreen_04_04_01";
        Padding = new Padding(8);
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1440, 940);
        Tag = "04.04.01";
        bankCurrencyGroup.ResumeLayout(false);
        bankCurrencyGroup.PerformLayout();
        documentAdditional.ResumeLayout(false);
        documentAdditionalSections.ResumeLayout(false);
        tp1.ResumeLayout(false);
        tp1.PerformLayout();
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
        tlpAuditInfo.ResumeLayout(false);
        tlpAuditInfo.PerformLayout();
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
        referenceHeader.ResumeLayout(false);
        referenceHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)validationErrors).EndInit();
        mainLayout.ResumeLayout(false);
        mainLayout.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
        flpActions.ResumeLayout(false);
        flpActions.PerformLayout();
        tabs.ResumeLayout(false);
        tp0.ResumeLayout(false);
        tp0.PerformLayout();
        linesHost.ResumeLayout(false);
        linesHost.PerformLayout();
        totalsPanel.ResumeLayout(false);
        totalsPanel.PerformLayout();
        ResumeLayout(false);
    
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Fill;
        designerCommandBar.Location = new Point(6, 6);
        designerCommandBar.Size = new Size(1412, 43);
        designerCommandBar.Margin = new Padding(2);
        designerCommandBar.TabIndex = 0;
        designerCommandBar.RightToLeft = RightToLeft.Yes;
        designerCommandBar.Controls.Add(flpActions);
        flpActions.Dock = DockStyle.Fill;
        flpActions.Margin = Padding.Empty;
        designerCommandBar.Name = "designerCommandBar";
        designerCommandBar.BackColor = Color.FromArgb(239, 239, 239);
        designerCommandBar.BorderStyle = BorderStyle.FixedSingle;
        designerHiddenCommands.Name = "designerHiddenCommands";
        designerHiddenCommands.Visible = false;
        designerCommandBar.Controls.Add(designerHiddenCommands);
        standardCommandDelete.Name = "standardCommandDelete";
        standardCommandDelete.Enabled = false;
        standardCommandDelete.Visible = false;
        standardCommandDelete.AccessibleName = "حذف";
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
        designerCommandBar.SetCommandRole(btnClear, TransportERP.Desktop.CoreUI.DesignerCommandRole.Add);
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
        designerHiddenCommands.Controls.Add(standardCommandDelete);
        designerCommandBar.SetCommandRole(standardCommandDelete, TransportERP.Desktop.CoreUI.DesignerCommandRole.Delete);
        designerCommandBar.SetCommandRole(btnCancel, TransportERP.Desktop.CoreUI.DesignerCommandRole.Cancel);
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
        designerHiddenCommands.Controls.Add(standardCommandLast);
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
        designerHiddenCommands.Controls.Add(standardCommandNext);
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
        designerHiddenCommands.Controls.Add(standardCommandPrevious);
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
        designerHiddenCommands.Controls.Add(standardCommandFirst);
        designerCommandBar.SetCommandRole(standardCommandFirst, TransportERP.Desktop.CoreUI.DesignerCommandRole.First);
        designerCommandBar.SetCommandRole(btnCreate, TransportERP.Desktop.CoreUI.DesignerCommandRole.Save);
        designerCommandBar.SetCommandRole(btnPrint, TransportERP.Desktop.CoreUI.DesignerCommandRole.Print);
        designerCommandBar.SetCommandRole(btnClose, TransportERP.Desktop.CoreUI.DesignerCommandRole.Close);
        standardCommandRefresh.AutoSize = false;
        standardCommandRefresh.Dock = DockStyle.None;
        standardCommandRefresh.MinimumSize = Size.Empty;
        standardCommandRefresh.Size = new Size(26, 24);
        standardCommandRefresh.Margin = new Padding(1);
        standardCommandRefresh.FlatStyle = FlatStyle.Flat;
        standardCommandRefresh.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandRefresh.Text = "تحديث";
        designerHiddenCommands.Controls.Add(standardCommandRefresh);
        designerCommandBar.SetCommandRole(standardCommandRefresh, TransportERP.Desktop.CoreUI.DesignerCommandRole.Refresh);
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
        standardAuditMetadata.Profile = TransportERP.Desktop.CoreUI.AuditMetadataProfile.ReceiptReference;
        standardAuditMetadata.Size = new Size(800, 90);
        referenceAudit.Visible = false;
        tlpAuditInfo.Visible = false;
        Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    private TableLayoutPanel tlpAuditInfo;
    private Label lblPrintCount;
    private Label lblLastPrintedAt;
    private Label lblEditCount;
    private Label lblModifiedAt;
    private Label lblModifiedBy;
    private Label lblCreatedAt;
    private Label lblCreatedBy;
    private TableLayoutPanel mainLayout;
    private FlowLayoutPanel flpActions;
    private Button btnView;
    private Button btnCreate;
    private Button btnEdit;
    private Button btnCancel;
    private Button btnPost;
    private Button btnReverse;
    private Button btnClear;
    private Button btnClose;
    private TabControl tabs;
    private TabPage tp0;
    private TableLayoutPanel layout0;
    private Label lbl_voucherNumber;
    private TextBox field_voucherNumber;
    private Label lbl_counterAccountRef;
    private ComboBox field_counterAccountRef;
    private Label lbl_description;
    private TextBox field_description;
    private Label lbl_state;
    private TextBox field_state;
    private TabPage tp1;
    private TableLayoutPanel linesHost;
    private Button btnAddRow;
    private Button btnRemoveRow;
    private TabPage tp2;
    private TableLayoutPanel layout2;
    private RichTextBox context2;
    private TabPage tp3;
    private TableLayoutPanel layout3;
    private RichTextBox context3;
    private TabPage tp4;
    private TableLayoutPanel layout4;
    private RichTextBox context4;
    private Label lblStatus;
    private Label label1;
    private ComboBox textBox1;
    private ReceiptDateTimePicker field_voucherDate;
    private Label lbl_voucherDate;
    private Label lbl_destinationCashBankRef;
    private ComboBox field_destinationCashBankRef;
    private Label lbl_amount;
    private TextBox field_amount;
    private Label lbl_exchangeRate;
    private TextBox field_exchangeRate;
    private Label lbl_currencyRef;
    private ComboBox field_currencyRef;
    private Label lbl_partyRef;
    private ComboBox field_partyRef;
    private DataGridView dgvLines;
    private DataGridViewTextBoxColumn col_rowNo;
    private DataGridViewComboBoxColumn col_counterAccountRef;
    private DataGridViewTextBoxColumn col_analyticalAccount;
    private DataGridViewTextBoxColumn col_accountName;
    private DataGridViewTextBoxColumn col_lineDescription;
    private DataGridViewComboBoxColumn col_currencyRef;
    private DataGridViewTextBoxColumn col_exchangeRate;
    private DataGridViewTextBoxColumn col_amount;
    private DataGridViewTextBoxColumn col_foreignAmount;
    private DataGridViewTextBoxColumn col_costCenter;
    private DataGridViewTextBoxColumn col_referenceNumber;
    private DataGridViewTextBoxColumn col_salespersonNumber;
    private DataGridViewTextBoxColumn col_cashierNumber;
    private DataGridViewTextBoxColumn col_chequeNumber;
    private DataGridViewTextBoxColumn col_chequeDueDate;
    private DataGridViewComboBoxColumn col_partyRef;
    private DataGridViewTextBoxColumn col_accountingAmount;
    private DataGridViewTextBoxColumn col_project;
    private DataGridViewTextBoxColumn col_activity;
    private Label label3;
    private TextBox field_total;
    private Label label2;
    private TextBox field_difference;
}
