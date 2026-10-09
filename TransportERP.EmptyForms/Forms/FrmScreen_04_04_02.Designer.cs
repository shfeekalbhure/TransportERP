#nullable enable
using System.Drawing;
using System.Windows.Forms;
namespace TransportERP.EmptyForms;

partial class UcScreen_04_04_02
{
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;

    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
    private FlowLayoutPanel designerHiddenCommands = null!;
    private Button standardCommandAdd = null!;
    private Button standardCommandDelete = null!;
    private Button standardCommandLast = null!;
    private Button standardCommandNext = null!;
    private Button standardCommandPrevious = null!;
    private Button standardCommandFirst = null!;
    private Button standardCommandSave = null!;
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
    private TabControl tabs = null!;
    private Label lblStatus = null!;
    private TabPage tp0 = null!;
    private TableLayoutPanel layout0 = null!;
    private TextBox field_voucherNumber = null!;
    private Label lbl_voucherNumber = null!;
    private DateTimePicker field_voucherDate = null!;
    private Label lbl_voucherDate = null!;
    private ComboBox field_partyRef = null!;
    private Label lbl_partyRef = null!;
    private ComboBox field_sourceCashBankRef = null!;
    private Label lbl_sourceCashBankRef = null!;
    private ComboBox field_destinationCashBankRef = null!;
    private Label lbl_destinationCashBankRef = null!;
    private ComboBox field_currencyRef = null!;
    private Label lbl_currencyRef = null!;
    private TextBox field_amount = null!;
    private Label lbl_amount = null!;
    private TextBox field_exchangeRate = null!;
    private Label lbl_exchangeRate = null!;
    private ComboBox field_counterAccountRef = null!;
    private Label lbl_counterAccountRef = null!;
    private TextBox field_description = null!;
    private Label lbl_description = null!;
    private TextBox field_state = null!;
    private Label lbl_state = null!;
    private TabPage tp1 = null!;
    private TableLayoutPanel layout1 = null!;
    private DataGridView dgvLines = null!;
    private DataGridViewTextBoxColumn col_rowNo = null!;
    private DataGridViewComboBoxColumn col_partyRef = null!;
    private DataGridViewComboBoxColumn col_counterAccountRef = null!;
    private DataGridViewTextBoxColumn col_lineDescription = null!;
    private DataGridViewComboBoxColumn col_currencyRef = null!;
    private DataGridViewTextBoxColumn col_amount = null!;
    private DataGridViewTextBoxColumn col_exchangeRate = null!;
    private DataGridViewTextBoxColumn col_accountingAmount = null!;
    private TableLayoutPanel linesHost = null!;
    private FlowLayoutPanel lineActions = null!;
    private Button btnAddRow = null!;
    private Button btnRemoveRow = null!;
    private TabPage tp2 = null!;
    private TableLayoutPanel layout2 = null!;
    private RichTextBox context2 = null!;
    private TabPage tp3 = null!;
    private TableLayoutPanel layout3 = null!;
    private RichTextBox context3 = null!;
    private TabPage tp4 = null!;
    private TableLayoutPanel layout4 = null!;
    private RichTextBox context4 = null!;
    private System.ComponentModel.IContainer? components;
    private ErrorProvider validationErrors = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }
    private TableLayoutPanel referenceHeader = null!;
    private TabPage referenceExtra = null!;
    private Label referenceAudit = null!;
    private Label referenceTotals = null!;
    private Label referenceLabel0 = null!;
    private TextBox referenceField0 = null!;
    private Label referenceLabel1 = null!;
    private TextBox referenceField1 = null!;
    private Label referenceLabel7 = null!;
    private TextBox referenceField7 = null!;
    private Label referenceLabel10 = null!;
    private TextBox referenceField10 = null!;
    private Label referenceLabel11 = null!;
    private TextBox referenceField11 = null!;
    private Label referenceLabel12 = null!;
    private TextBox referenceField12 = null!;
    private Label referenceLabel13 = null!;
    private TextBox referenceField13 = null!;
    private Label referenceLabel14 = null!;
    private TextBox referenceField14 = null!;
    private Label referenceLabel15 = null!;
    private TextBox referenceField15 = null!;
    private Label referenceLabel16 = null!;
    private TextBox referenceField16 = null!;
    private Label referenceLabel17 = null!;
    private TextBox referenceField17 = null!;
    private Label referenceLabel18 = null!;
    private TextBox referenceField18 = null!;
    private Label referenceLabel19 = null!;
    private TextBox referenceField19 = null!;
    private Label referenceLabel20 = null!;
    private TextBox referenceField20 = null!;
    private DataGridViewTextBoxColumn referenceCol0 = null!;
    private DataGridViewTextBoxColumn referenceCol1 = null!;
    private DataGridViewTextBoxColumn referenceCol3 = null!;
    private Button referencePrint = null!;
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
    private System.Windows.Forms.Panel rootWorkspaceViewport;

        private void InitializeComponent()
    {
        rootWorkspaceViewport = new System.Windows.Forms.Panel();
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        designerHiddenCommands = new FlowLayoutPanel();
        standardCommandAdd = new Button();
        standardCommandDelete = new Button();
        standardCommandLast = new Button();
        standardCommandNext = new Button();
        standardCommandPrevious = new Button();
        standardCommandFirst = new Button();
        standardCommandSave = new Button();
        standardCommandRefresh = new Button();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
        components = new System.ComponentModel.Container();
        DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
        documentAdditional = new TabPage();
        documentAdditionalSections = new TabControl();
        tp1 = new TabPage();
        linesHost = new TableLayoutPanel();
        layout1 = new TableLayoutPanel();
        lineActions = new FlowLayoutPanel();
        tp2 = new TabPage();
        layout2 = new TableLayoutPanel();
        context2 = new RichTextBox();
        tp3 = new TabPage();
        layout3 = new TableLayoutPanel();
        context3 = new RichTextBox();
        tp4 = new TabPage();
        layout4 = new TableLayoutPanel();
        context4 = new RichTextBox();
        referenceExtra = new TabPage();
        layout0 = new TableLayoutPanel();
        lbl_partyRef = new Label();
        field_partyRef = new ComboBox();
        lbl_destinationCashBankRef = new Label();
        field_destinationCashBankRef = new ComboBox();
        lbl_counterAccountRef = new Label();
        field_counterAccountRef = new ComboBox();
        lbl_state = new Label();
        field_state = new TextBox();
        documentDefaults = new TabPage();
        documentDefaultsLayout = new TableLayoutPanel();
        lblDefaultCurrency = new Label();
        cboDefaultCurrency = new ComboBox();
        lblDefaultCostCenter = new Label();
        cboDefaultCostCenter = new ComboBox();
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
        documentImport = new TabPage();
        documentImportLayout = new TableLayoutPanel();
        documentImportFormat = new Label();
        documentImportButtons = new FlowLayoutPanel();
        documentImportPath = new TextBox();
        documentImportChoose = new Button();
        documentImportApply = new Button();
        documentImportPreview = new DataGridView();
        referenceHeader = new TableLayoutPanel();
        referenceLabel0 = new Label();
        referenceField0 = new TextBox();
        referenceLabel1 = new Label();
        referenceField1 = new TextBox();
        lbl_voucherNumber = new Label();
        field_voucherNumber = new TextBox();
        lbl_voucherDate = new Label();
        field_voucherDate = new DateTimePicker();
        lbl_sourceCashBankRef = new Label();
        field_sourceCashBankRef = new ComboBox();
        lbl_currencyRef = new Label();
        field_currencyRef = new ComboBox();
        lbl_amount = new Label();
        field_amount = new TextBox();
        referenceLabel7 = new Label();
        referenceField7 = new TextBox();
        lbl_exchangeRate = new Label();
        field_exchangeRate = new TextBox();
        lbl_description = new Label();
        field_description = new TextBox();
        referenceLabel10 = new Label();
        referenceField10 = new TextBox();
        referenceLabel11 = new Label();
        referenceField11 = new TextBox();
        referenceLabel12 = new Label();
        referenceField12 = new TextBox();
        referenceLabel13 = new Label();
        referenceField13 = new TextBox();
        referenceLabel14 = new Label();
        referenceField14 = new TextBox();
        referenceLabel15 = new Label();
        referenceField15 = new TextBox();
        referenceLabel16 = new Label();
        referenceField16 = new TextBox();
        referenceLabel17 = new Label();
        referenceField17 = new TextBox();
        referenceLabel18 = new Label();
        referenceField18 = new TextBox();
        referenceLabel19 = new Label();
        referenceField19 = new TextBox();
        referenceLabel20 = new Label();
        referenceField20 = new TextBox();
        referenceAudit = new Label();
        referenceTotals = new Label();
        referenceCol0 = new DataGridViewTextBoxColumn();
        referenceCol1 = new DataGridViewTextBoxColumn();
        referenceCol3 = new DataGridViewTextBoxColumn();
        referencePrint = new Button();
        validationErrors = new ErrorProvider(components);
        mainLayout = new TableLayoutPanel();
        lblTitle = new Label();
        flpActions = new FlowLayoutPanel();
        btnView = new Button();
        btnCreate = new Button();
        btnEdit = new Button();
        btnCancel = new Button();
        btnPost = new Button();
        btnReverse = new Button();
        btnClear = new Button();
        btnClose = new Button();
        btnAddRow = new Button();
        btnRemoveRow = new Button();
        tabs = new TabControl();
        tp0 = new TabPage();
        dgvLines = new DataGridView();
        col_rowNo = new DataGridViewTextBoxColumn();
        col_counterAccountRef = new DataGridViewComboBoxColumn();
        col_lineDescription = new DataGridViewTextBoxColumn();
        col_currencyRef = new DataGridViewComboBoxColumn();
        col_exchangeRate = new DataGridViewTextBoxColumn();
        col_amount = new DataGridViewTextBoxColumn();
        col_partyRef = new DataGridViewComboBoxColumn();
        col_accountingAmount = new DataGridViewTextBoxColumn();
        lblStatus = new Label();
        documentAdditional.SuspendLayout();
        documentAdditionalSections.SuspendLayout();
        tp1.SuspendLayout();
        linesHost.SuspendLayout();
        tp2.SuspendLayout();
        layout2.SuspendLayout();
        tp3.SuspendLayout();
        layout3.SuspendLayout();
        tp4.SuspendLayout();
        layout4.SuspendLayout();
        referenceExtra.SuspendLayout();
        layout0.SuspendLayout();
        documentDefaults.SuspendLayout();
        documentDefaultsLayout.SuspendLayout();
        documentAccounts.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)documentAccountsGrid).BeginInit();
        documentImport.SuspendLayout();
        documentImportLayout.SuspendLayout();
        documentImportButtons.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)documentImportPreview).BeginInit();
        referenceHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)validationErrors).BeginInit();
        mainLayout.SuspendLayout();
        flpActions.SuspendLayout();
        tabs.SuspendLayout();
        tp0.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
        SuspendLayout();
        // 
        // documentAdditional
        // 
        documentAdditional.AutoScroll = true;
        documentAdditional.Controls.Add(documentAdditionalSections);
        documentAdditional.Location = new Point(4, 23);
        documentAdditional.Name = "documentAdditional";
        documentAdditional.Size = new Size(1086, 246);
        documentAdditional.TabIndex = 2;
        documentAdditional.Text = "بيانات إضافية";
        // 
        // documentAdditionalSections
        // 
        documentAdditionalSections.Controls.Add(tp1);
        documentAdditionalSections.Controls.Add(tp2);
        documentAdditionalSections.Controls.Add(tp3);
        documentAdditionalSections.Controls.Add(tp4);
        documentAdditionalSections.Controls.Add(referenceExtra);
        documentAdditionalSections.Dock = DockStyle.Fill;
        documentAdditionalSections.Location = new Point(0, 0);
        documentAdditionalSections.Name = "documentAdditionalSections";
        documentAdditionalSections.RightToLeft = RightToLeft.Yes;
        documentAdditionalSections.RightToLeftLayout = true;
        documentAdditionalSections.SelectedIndex = 0;
        documentAdditionalSections.Size = new Size(1086, 246);
        documentAdditionalSections.TabIndex = 0;
        // 
        // tp1
        // 
        tp1.AutoScroll = true;
        tp1.Controls.Add(linesHost);
        tp1.Location = new Point(4, 23);
        tp1.Name = "tp1";
        tp1.Size = new Size(1078, 219);
        tp1.TabIndex = 1;
        tp1.Text = "التفاصيل والحركات";
        // 
        // linesHost
        // 
        linesHost.ColumnCount = 1;
        linesHost.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        linesHost.Controls.Add(layout1, 0, 0);
        linesHost.Controls.Add(lineActions, 0, 1);
        linesHost.Dock = DockStyle.Fill;
        linesHost.Location = new Point(0, 0);
        linesHost.MinimumSize = new Size(680, 380);
        linesHost.Name = "linesHost";
        linesHost.RowCount = 3;
        linesHost.RowStyles.Add(new RowStyle());
        linesHost.RowStyles.Add(new RowStyle());
        linesHost.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        linesHost.Size = new Size(1078, 380);
        linesHost.TabIndex = 0;
        // 
        // layout1
        // 
        layout1.AutoScroll = true;
        layout1.AutoSize = true;
        layout1.ColumnCount = 2;
        layout1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 195F));
        layout1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout1.Dock = DockStyle.Top;
        layout1.Location = new Point(3, 3);
        layout1.MinimumSize = new Size(650, 0);
        layout1.Name = "layout1";
        layout1.Size = new Size(1072, 0);
        layout1.TabIndex = 0;
        // 
        // lineActions
        // 
        lineActions.AutoSize = true;
        lineActions.Dock = DockStyle.Fill;
        lineActions.Location = new Point(3, 9);
        lineActions.Name = "lineActions";
        lineActions.Size = new Size(1072, 1);
        lineActions.TabIndex = 1;
        // 
        // tp2
        // 
        tp2.AutoScroll = true;
        tp2.Controls.Add(layout2);
        tp2.Location = new Point(4, 23);
        tp2.Name = "tp2";
        tp2.Size = new Size(1078, 219);
        tp2.TabIndex = 2;
        tp2.Text = "المرفقات والربط بالمستندات";
        // 
        // layout2
        // 
        layout2.AutoScroll = true;
        layout2.AutoSize = true;
        layout2.ColumnCount = 2;
        layout2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 195F));
        layout2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout2.Controls.Add(context2, 0, 0);
        layout2.Dock = DockStyle.Fill;
        layout2.Location = new Point(0, 0);
        layout2.MinimumSize = new Size(650, 0);
        layout2.Name = "layout2";
        layout2.RowCount = 1;
        layout2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout2.Size = new Size(1078, 219);
        layout2.TabIndex = 0;
        // 
        // context2
        // 
        context2.AccessibleName = "المرفقات والربط بالمستندات";
        layout2.SetColumnSpan(context2, 2);
        context2.DetectUrls = false;
        context2.Dock = DockStyle.Fill;
        context2.Location = new Point(3, 3);
        context2.Name = "context2";
        context2.ReadOnly = true;
        context2.Size = new Size(1072, 213);
        context2.TabIndex = 0;
        context2.Text = "";
        // 
        // tp3
        // 
        tp3.AutoScroll = true;
        tp3.Controls.Add(layout3);
        tp3.Location = new Point(4, 23);
        tp3.Name = "tp3";
        tp3.Size = new Size(1078, 219);
        tp3.TabIndex = 3;
        tp3.Text = "الاعتمادات";
        // 
        // layout3
        // 
        layout3.AutoScroll = true;
        layout3.AutoSize = true;
        layout3.ColumnCount = 2;
        layout3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 195F));
        layout3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout3.Controls.Add(context3, 0, 0);
        layout3.Dock = DockStyle.Fill;
        layout3.Location = new Point(0, 0);
        layout3.MinimumSize = new Size(650, 0);
        layout3.Name = "layout3";
        layout3.RowCount = 1;
        layout3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout3.Size = new Size(1078, 219);
        layout3.TabIndex = 0;
        // 
        // context3
        // 
        context3.AccessibleName = "الاعتمادات";
        layout3.SetColumnSpan(context3, 2);
        context3.DetectUrls = false;
        context3.Dock = DockStyle.Fill;
        context3.Location = new Point(3, 3);
        context3.Name = "context3";
        context3.ReadOnly = true;
        context3.Size = new Size(1072, 213);
        context3.TabIndex = 0;
        context3.Text = "";
        // 
        // tp4
        // 
        tp4.AutoScroll = true;
        tp4.Controls.Add(layout4);
        tp4.Location = new Point(4, 23);
        tp4.Name = "tp4";
        tp4.Size = new Size(1078, 219);
        tp4.TabIndex = 4;
        tp4.Text = "سجل العمليات";
        // 
        // layout4
        // 
        layout4.AutoScroll = true;
        layout4.AutoSize = true;
        layout4.ColumnCount = 2;
        layout4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 195F));
        layout4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout4.Controls.Add(context4, 0, 0);
        layout4.Dock = DockStyle.Fill;
        layout4.Location = new Point(0, 0);
        layout4.MinimumSize = new Size(650, 0);
        layout4.Name = "layout4";
        layout4.RowCount = 1;
        layout4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout4.Size = new Size(1078, 219);
        layout4.TabIndex = 0;
        // 
        // context4
        // 
        context4.AccessibleName = "سجل العمليات";
        layout4.SetColumnSpan(context4, 2);
        context4.DetectUrls = false;
        context4.Dock = DockStyle.Fill;
        context4.Location = new Point(3, 3);
        context4.Name = "context4";
        context4.ReadOnly = true;
        context4.Size = new Size(1072, 213);
        context4.TabIndex = 0;
        context4.Text = "";
        // 
        // referenceExtra
        // 
        referenceExtra.AutoScroll = true;
        referenceExtra.Controls.Add(layout0);
        referenceExtra.Location = new Point(4, 23);
        referenceExtra.Name = "referenceExtra";
        referenceExtra.Size = new Size(1078, 219);
        referenceExtra.TabIndex = 5;
        referenceExtra.Text = "حقول تكميلية";
        // 
        // layout0
        // 
        layout0.AutoScroll = true;
        layout0.AutoSize = true;
        layout0.ColumnCount = 2;
        layout0.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 195F));
        layout0.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout0.Controls.Add(lbl_partyRef, 0, 2);
        layout0.Controls.Add(field_partyRef, 1, 2);
        layout0.Controls.Add(lbl_destinationCashBankRef, 0, 4);
        layout0.Controls.Add(field_destinationCashBankRef, 1, 4);
        layout0.Controls.Add(lbl_counterAccountRef, 0, 8);
        layout0.Controls.Add(field_counterAccountRef, 1, 8);
        layout0.Controls.Add(lbl_state, 0, 10);
        layout0.Controls.Add(field_state, 1, 10);
        layout0.Dock = DockStyle.Top;
        layout0.Location = new Point(0, 0);
        layout0.MinimumSize = new Size(650, 0);
        layout0.Name = "layout0";
        layout0.RowCount = 11;
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.Size = new Size(1078, 112);
        layout0.TabIndex = 0;
        // 
        // lbl_partyRef
        // 
        lbl_partyRef.AutoSize = true;
        lbl_partyRef.Dock = DockStyle.Fill;
        lbl_partyRef.Location = new Point(886, 0);
        lbl_partyRef.Name = "lbl_partyRef";
        lbl_partyRef.Size = new Size(189, 28);
        lbl_partyRef.TabIndex = 0;
        lbl_partyRef.Text = "الطرف";
        lbl_partyRef.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_partyRef
        // 
        field_partyRef.AccessibleName = "الطرف";
        field_partyRef.Dock = DockStyle.Fill;
        field_partyRef.DropDownStyle = ComboBoxStyle.DropDownList;
        field_partyRef.DropDownWidth = 420;
        field_partyRef.Location = new Point(3, 3);
        field_partyRef.Name = "field_partyRef";
        field_partyRef.Size = new Size(877, 22);
        field_partyRef.TabIndex = 2;
        field_partyRef.Tag = "ACC-044:partyRef";
        // 
        // lbl_destinationCashBankRef
        // 
        lbl_destinationCashBankRef.AutoSize = true;
        lbl_destinationCashBankRef.Dock = DockStyle.Fill;
        lbl_destinationCashBankRef.Location = new Point(886, 28);
        lbl_destinationCashBankRef.Name = "lbl_destinationCashBankRef";
        lbl_destinationCashBankRef.Size = new Size(189, 28);
        lbl_destinationCashBankRef.TabIndex = 3;
        lbl_destinationCashBankRef.Text = "الصندوق/البنك الوجهة";
        lbl_destinationCashBankRef.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_destinationCashBankRef
        // 
        field_destinationCashBankRef.AccessibleName = "الصندوق/البنك الوجهة";
        field_destinationCashBankRef.Dock = DockStyle.Fill;
        field_destinationCashBankRef.DropDownStyle = ComboBoxStyle.DropDownList;
        field_destinationCashBankRef.DropDownWidth = 420;
        field_destinationCashBankRef.Location = new Point(3, 31);
        field_destinationCashBankRef.Name = "field_destinationCashBankRef";
        field_destinationCashBankRef.Size = new Size(877, 22);
        field_destinationCashBankRef.TabIndex = 4;
        field_destinationCashBankRef.Tag = "ACC-044:destinationCashBankRef";
        // 
        // lbl_counterAccountRef
        // 
        lbl_counterAccountRef.AutoSize = true;
        lbl_counterAccountRef.Dock = DockStyle.Fill;
        lbl_counterAccountRef.Location = new Point(886, 56);
        lbl_counterAccountRef.Name = "lbl_counterAccountRef";
        lbl_counterAccountRef.Size = new Size(189, 28);
        lbl_counterAccountRef.TabIndex = 5;
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
        field_counterAccountRef.Location = new Point(3, 59);
        field_counterAccountRef.Name = "field_counterAccountRef";
        field_counterAccountRef.Size = new Size(877, 22);
        field_counterAccountRef.TabIndex = 8;
        field_counterAccountRef.Tag = "ACC-044:counterAccountRef";
        // 
        // lbl_state
        // 
        lbl_state.AutoSize = true;
        lbl_state.Dock = DockStyle.Fill;
        lbl_state.Location = new Point(886, 84);
        lbl_state.Name = "lbl_state";
        lbl_state.Size = new Size(189, 28);
        lbl_state.TabIndex = 9;
        lbl_state.Text = "الحالة";
        lbl_state.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_state
        // 
        field_state.AccessibleName = "الحالة";
        field_state.Dock = DockStyle.Fill;
        field_state.Location = new Point(3, 87);
        field_state.Name = "field_state";
        field_state.ReadOnly = true;
        field_state.Size = new Size(877, 22);
        field_state.TabIndex = 10;
        field_state.TabStop = false;
        field_state.Tag = "ACC-044:state";
        // 
        // documentDefaults
        // 
        documentDefaults.AutoScroll = true;
        documentDefaults.Controls.Add(documentDefaultsLayout);
        documentDefaults.Location = new Point(4, 23);
        documentDefaults.Name = "documentDefaults";
        documentDefaults.Size = new Size(1086, 246);
        documentDefaults.TabIndex = 3;
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
        documentDefaultsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        documentDefaultsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        documentDefaultsLayout.Size = new Size(1086, 64);
        documentDefaultsLayout.TabIndex = 0;
        // 
        // lblDefaultCurrency
        // 
        lblDefaultCurrency.AutoSize = true;
        lblDefaultCurrency.Location = new Point(1034, 12);
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
        cboDefaultCurrency.Size = new Size(916, 22);
        cboDefaultCurrency.TabIndex = 1;
        // 
        // lblDefaultCostCenter
        // 
        lblDefaultCostCenter.AutoSize = true;
        lblDefaultCostCenter.Location = new Point(1005, 32);
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
        cboDefaultCostCenter.Location = new Point(15, 35);
        cboDefaultCostCenter.Name = "cboDefaultCostCenter";
        cboDefaultCostCenter.Size = new Size(916, 22);
        cboDefaultCostCenter.TabIndex = 3;
        // 
        // documentAccounts
        // 
        documentAccounts.AutoScroll = true;
        documentAccounts.Controls.Add(documentAccountsGrid);
        documentAccounts.Location = new Point(4, 23);
        documentAccounts.Name = "documentAccounts";
        documentAccounts.Size = new Size(1086, 246);
        documentAccounts.TabIndex = 1;
        documentAccounts.Text = "الحسابات";
        // 
        // documentAccountsGrid
        // 
        documentAccountsGrid.AccessibleDescription = "الحسابات المتعددة؛ ربط خدمة السند والصلاحيات غير مكتمل";
        documentAccountsGrid.AllowUserToAddRows = false;
        documentAccountsGrid.AllowUserToDeleteRows = false;
        documentAccountsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        documentAccountsGrid.BackgroundColor = Color.White;
        dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle9.BackColor = Color.FromArgb(232, 232, 232);
        dataGridViewCellStyle9.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle9.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle9.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
        documentAccountsGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
        documentAccountsGrid.ColumnHeadersHeight = 29;
        documentAccountsGrid.Columns.AddRange(new DataGridViewColumn[] { documentAccountsGridColumn0, documentAccountsGridColumn1, documentAccountsGridColumn2, documentAccountsGridColumn3, documentAccountsGridColumn4, documentAccountsGridColumn5, documentAccountsGridColumn6, documentAccountsGridColumn7 });
        dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle10.BackColor = Color.FromArgb(255, 255, 226);
        dataGridViewCellStyle10.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle10.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle10.WrapMode = DataGridViewTriState.False;
        documentAccountsGrid.DefaultCellStyle = dataGridViewCellStyle10;
        documentAccountsGrid.Dock = DockStyle.Fill;
        documentAccountsGrid.EnableHeadersVisualStyles = false;
        documentAccountsGrid.Location = new Point(0, 0);
        documentAccountsGrid.Name = "documentAccountsGrid";
        documentAccountsGrid.ReadOnly = true;
        documentAccountsGrid.RightToLeft = RightToLeft.Yes;
        documentAccountsGrid.RowHeadersWidth = 51;
        documentAccountsGrid.Size = new Size(1086, 246);
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
        // documentImport
        // 
        documentImport.AutoScroll = true;
        documentImport.Controls.Add(documentImportLayout);
        documentImport.Location = new Point(4, 23);
        documentImport.Name = "documentImport";
        documentImport.Size = new Size(1086, 246);
        documentImport.TabIndex = 4;
        documentImport.Text = "استيراد من ملف";
        // 
        // documentImportLayout
        // 
        documentImportLayout.ColumnCount = 1;
        documentImportLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        documentImportLayout.Controls.Add(documentImportFormat, 0, 0);
        documentImportLayout.Controls.Add(documentImportButtons, 0, 1);
        documentImportLayout.Controls.Add(documentImportPreview, 0, 2);
        documentImportLayout.Dock = DockStyle.Fill;
        documentImportLayout.Location = new Point(0, 0);
        documentImportLayout.Name = "documentImportLayout";
        documentImportLayout.RowCount = 3;
        documentImportLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        documentImportLayout.RowStyles.Add(new RowStyle());
        documentImportLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        documentImportLayout.Size = new Size(1086, 246);
        documentImportLayout.TabIndex = 0;
        // 
        // documentImportFormat
        // 
        documentImportFormat.Dock = DockStyle.Fill;
        documentImportFormat.Location = new Point(3, 0);
        documentImportFormat.Name = "documentImportFormat";
        documentImportFormat.Size = new Size(1080, 28);
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
        documentImportButtons.Size = new Size(1080, 36);
        documentImportButtons.TabIndex = 1;
        // 
        // documentImportPath
        // 
        documentImportPath.Location = new Point(717, 3);
        documentImportPath.Name = "documentImportPath";
        documentImportPath.ReadOnly = true;
        documentImportPath.Size = new Size(360, 22);
        documentImportPath.TabIndex = 0;
        // 
        // documentImportChoose
        // 
        documentImportChoose.AutoSize = true;
        documentImportChoose.Location = new Point(574, 3);
        documentImportChoose.Name = "documentImportChoose";
        documentImportChoose.Size = new Size(137, 30);
        documentImportChoose.TabIndex = 1;
        documentImportChoose.Text = "اختيار ملف ومعاينة";
        // 
        // documentImportApply
        // 
        documentImportApply.AutoSize = true;
        documentImportApply.Location = new Point(425, 3);
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
        documentImportPreview.Size = new Size(1080, 170);
        documentImportPreview.TabIndex = 2;
        // 
        // referenceHeader
        // 
        referenceHeader.AutoSize = true;
        referenceHeader.BackColor = Color.FromArgb(245, 245, 245);
        referenceHeader.ColumnCount = 6;
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115F));
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115F));
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115F));
        referenceHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
        referenceHeader.Controls.Add(referenceLabel0, 0, 0);
        referenceHeader.Controls.Add(referenceField0, 1, 0);
        referenceHeader.Controls.Add(referenceLabel1, 2, 0);
        referenceHeader.Controls.Add(referenceField1, 3, 0);
        referenceHeader.Controls.Add(lbl_voucherNumber, 4, 0);
        referenceHeader.Controls.Add(field_voucherNumber, 5, 0);
        referenceHeader.Controls.Add(lbl_voucherDate, 0, 1);
        referenceHeader.Controls.Add(field_voucherDate, 1, 1);
        referenceHeader.Controls.Add(lbl_sourceCashBankRef, 2, 1);
        referenceHeader.Controls.Add(field_sourceCashBankRef, 3, 1);
        referenceHeader.Controls.Add(lbl_currencyRef, 4, 1);
        referenceHeader.Controls.Add(field_currencyRef, 5, 1);
        referenceHeader.Controls.Add(lbl_amount, 0, 2);
        referenceHeader.Controls.Add(field_amount, 1, 2);
        referenceHeader.Controls.Add(referenceLabel7, 2, 2);
        referenceHeader.Controls.Add(referenceField7, 3, 2);
        referenceHeader.Controls.Add(lbl_exchangeRate, 4, 2);
        referenceHeader.Controls.Add(field_exchangeRate, 5, 2);
        referenceHeader.Controls.Add(lbl_description, 0, 3);
        referenceHeader.Controls.Add(field_description, 1, 3);
        referenceHeader.Controls.Add(referenceLabel10, 2, 3);
        referenceHeader.Controls.Add(referenceField10, 3, 3);
        referenceHeader.Controls.Add(referenceLabel11, 4, 3);
        referenceHeader.Controls.Add(referenceField11, 5, 3);
        referenceHeader.Controls.Add(referenceLabel12, 0, 4);
        referenceHeader.Controls.Add(referenceField12, 1, 4);
        referenceHeader.Controls.Add(referenceLabel13, 2, 4);
        referenceHeader.Controls.Add(referenceField13, 3, 4);
        referenceHeader.Controls.Add(referenceLabel14, 4, 4);
        referenceHeader.Controls.Add(referenceField14, 5, 4);
        referenceHeader.Controls.Add(referenceLabel15, 0, 5);
        referenceHeader.Controls.Add(referenceField15, 1, 5);
        referenceHeader.Controls.Add(referenceLabel16, 2, 5);
        referenceHeader.Controls.Add(referenceField16, 3, 5);
        referenceHeader.Controls.Add(referenceLabel17, 4, 5);
        referenceHeader.Controls.Add(referenceField17, 5, 5);
        referenceHeader.Controls.Add(referenceLabel18, 0, 6);
        referenceHeader.Controls.Add(referenceField18, 1, 6);
        referenceHeader.Controls.Add(referenceLabel19, 2, 6);
        referenceHeader.Controls.Add(referenceField19, 3, 6);
        referenceHeader.Controls.Add(referenceLabel20, 4, 6);
        referenceHeader.Controls.Add(referenceField20, 5, 6);
        referenceHeader.Dock = DockStyle.Top;
        referenceHeader.Location = new Point(0, 0);
        referenceHeader.Name = "referenceHeader";
        referenceHeader.Padding = new Padding(8);
        referenceHeader.RightToLeft = RightToLeft.Yes;
        referenceHeader.RowCount = 7;
        referenceHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        referenceHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        referenceHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        referenceHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        referenceHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        referenceHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        referenceHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        referenceHeader.Size = new Size(1086, 240);
        referenceHeader.TabIndex = 0;
        // 
        // referenceLabel0
        // 
        referenceLabel0.Dock = DockStyle.Fill;
        referenceLabel0.Location = new Point(965, 10);
        referenceLabel0.Margin = new Padding(2);
        referenceLabel0.Name = "referenceLabel0";
        referenceLabel0.Size = new Size(111, 28);
        referenceLabel0.TabIndex = 0;
        referenceLabel0.Text = "رقم الفرع";
        referenceLabel0.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField0
        // 
        referenceField0.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField0.BackColor = Color.FromArgb(245, 245, 245);
        referenceField0.Dock = DockStyle.Fill;
        referenceField0.Location = new Point(724, 10);
        referenceField0.Margin = new Padding(2);
        referenceField0.Name = "referenceField0";
        referenceField0.ReadOnly = true;
        referenceField0.Size = new Size(237, 22);
        referenceField0.TabIndex = 1;
        referenceField0.TabStop = false;
        // 
        // referenceLabel1
        // 
        referenceLabel1.Dock = DockStyle.Fill;
        referenceLabel1.Location = new Point(609, 10);
        referenceLabel1.Margin = new Padding(2);
        referenceLabel1.Name = "referenceLabel1";
        referenceLabel1.Size = new Size(111, 28);
        referenceLabel1.TabIndex = 2;
        referenceLabel1.Text = "نوع السند";
        referenceLabel1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField1
        // 
        referenceField1.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField1.BackColor = Color.FromArgb(245, 245, 245);
        referenceField1.Dock = DockStyle.Fill;
        referenceField1.Location = new Point(368, 10);
        referenceField1.Margin = new Padding(2);
        referenceField1.Name = "referenceField1";
        referenceField1.ReadOnly = true;
        referenceField1.Size = new Size(237, 22);
        referenceField1.TabIndex = 3;
        referenceField1.TabStop = false;
        // 
        // lbl_voucherNumber
        // 
        lbl_voucherNumber.Dock = DockStyle.Fill;
        lbl_voucherNumber.Location = new Point(253, 10);
        lbl_voucherNumber.Margin = new Padding(2);
        lbl_voucherNumber.Name = "lbl_voucherNumber";
        lbl_voucherNumber.Size = new Size(111, 28);
        lbl_voucherNumber.TabIndex = 4;
        lbl_voucherNumber.Text = "رقم السند";
        lbl_voucherNumber.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_voucherNumber
        // 
        field_voucherNumber.AccessibleName = "رقم السند";
        field_voucherNumber.Dock = DockStyle.Fill;
        field_voucherNumber.Location = new Point(10, 10);
        field_voucherNumber.Margin = new Padding(2);
        field_voucherNumber.Name = "field_voucherNumber";
        field_voucherNumber.ReadOnly = true;
        field_voucherNumber.Size = new Size(239, 22);
        field_voucherNumber.TabIndex = 0;
        field_voucherNumber.TabStop = false;
        field_voucherNumber.Tag = "ACC-044:voucherNumber";
        // 
        // lbl_voucherDate
        // 
        lbl_voucherDate.Dock = DockStyle.Fill;
        lbl_voucherDate.Location = new Point(965, 42);
        lbl_voucherDate.Margin = new Padding(2);
        lbl_voucherDate.Name = "lbl_voucherDate";
        lbl_voucherDate.Size = new Size(111, 28);
        lbl_voucherDate.TabIndex = 5;
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
        field_voucherDate.Format = DateTimePickerFormat.Custom;
        field_voucherDate.Location = new Point(724, 42);
        field_voucherDate.Margin = new Padding(2);
        field_voucherDate.Name = "field_voucherDate";
        field_voucherDate.ShowCheckBox = true;
        field_voucherDate.Size = new Size(237, 22);
        field_voucherDate.TabIndex = 1;
        field_voucherDate.Tag = "ACC-044:voucherDate";
        // 
        // lbl_sourceCashBankRef
        // 
        lbl_sourceCashBankRef.Dock = DockStyle.Fill;
        lbl_sourceCashBankRef.Location = new Point(609, 42);
        lbl_sourceCashBankRef.Margin = new Padding(2);
        lbl_sourceCashBankRef.Name = "lbl_sourceCashBankRef";
        lbl_sourceCashBankRef.Size = new Size(111, 28);
        lbl_sourceCashBankRef.TabIndex = 6;
        lbl_sourceCashBankRef.Text = "الصندوق/البنك المصدر";
        lbl_sourceCashBankRef.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_sourceCashBankRef
        // 
        field_sourceCashBankRef.AccessibleName = "الصندوق/البنك المصدر";
        field_sourceCashBankRef.Dock = DockStyle.Fill;
        field_sourceCashBankRef.DropDownStyle = ComboBoxStyle.DropDownList;
        field_sourceCashBankRef.DropDownWidth = 420;
        field_sourceCashBankRef.Location = new Point(368, 42);
        field_sourceCashBankRef.Margin = new Padding(2);
        field_sourceCashBankRef.Name = "field_sourceCashBankRef";
        field_sourceCashBankRef.Size = new Size(237, 22);
        field_sourceCashBankRef.TabIndex = 3;
        field_sourceCashBankRef.Tag = "ACC-044:sourceCashBankRef";
        // 
        // lbl_currencyRef
        // 
        lbl_currencyRef.Dock = DockStyle.Fill;
        lbl_currencyRef.Location = new Point(253, 42);
        lbl_currencyRef.Margin = new Padding(2);
        lbl_currencyRef.Name = "lbl_currencyRef";
        lbl_currencyRef.Size = new Size(111, 28);
        lbl_currencyRef.TabIndex = 7;
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
        field_currencyRef.Location = new Point(10, 42);
        field_currencyRef.Margin = new Padding(2);
        field_currencyRef.Name = "field_currencyRef";
        field_currencyRef.Size = new Size(239, 22);
        field_currencyRef.TabIndex = 5;
        field_currencyRef.Tag = "ACC-044:currencyRef";
        // 
        // lbl_amount
        // 
        lbl_amount.Dock = DockStyle.Fill;
        lbl_amount.Location = new Point(965, 74);
        lbl_amount.Margin = new Padding(2);
        lbl_amount.Name = "lbl_amount";
        lbl_amount.Size = new Size(111, 28);
        lbl_amount.TabIndex = 8;
        lbl_amount.Text = "المبلغ";
        lbl_amount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_amount
        // 
        field_amount.AccessibleName = "المبلغ";
        field_amount.BackColor = Color.LightYellow;
        field_amount.Dock = DockStyle.Fill;
        field_amount.Location = new Point(724, 74);
        field_amount.Margin = new Padding(2);
        field_amount.Name = "field_amount";
        field_amount.RightToLeft = RightToLeft.No;
        field_amount.Size = new Size(237, 22);
        field_amount.TabIndex = 6;
        field_amount.Tag = "ACC-044:amount";
        field_amount.TextAlign = HorizontalAlignment.Right;
        // 
        // referenceLabel7
        // 
        referenceLabel7.Dock = DockStyle.Fill;
        referenceLabel7.Location = new Point(609, 74);
        referenceLabel7.Margin = new Padding(2);
        referenceLabel7.Name = "referenceLabel7";
        referenceLabel7.Size = new Size(111, 28);
        referenceLabel7.TabIndex = 9;
        referenceLabel7.Text = "المستلم";
        referenceLabel7.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField7
        // 
        referenceField7.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField7.BackColor = Color.FromArgb(245, 245, 245);
        referenceField7.Dock = DockStyle.Fill;
        referenceField7.Location = new Point(368, 74);
        referenceField7.Margin = new Padding(2);
        referenceField7.Name = "referenceField7";
        referenceField7.ReadOnly = true;
        referenceField7.Size = new Size(237, 22);
        referenceField7.TabIndex = 10;
        referenceField7.TabStop = false;
        // 
        // lbl_exchangeRate
        // 
        lbl_exchangeRate.Dock = DockStyle.Fill;
        lbl_exchangeRate.Location = new Point(253, 74);
        lbl_exchangeRate.Margin = new Padding(2);
        lbl_exchangeRate.Name = "lbl_exchangeRate";
        lbl_exchangeRate.Size = new Size(111, 28);
        lbl_exchangeRate.TabIndex = 11;
        lbl_exchangeRate.Text = "سعر الصرف";
        lbl_exchangeRate.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_exchangeRate
        // 
        field_exchangeRate.AccessibleName = "سعر الصرف";
        field_exchangeRate.BackColor = Color.LightYellow;
        field_exchangeRate.Dock = DockStyle.Fill;
        field_exchangeRate.Location = new Point(10, 74);
        field_exchangeRate.Margin = new Padding(2);
        field_exchangeRate.Name = "field_exchangeRate";
        field_exchangeRate.RightToLeft = RightToLeft.No;
        field_exchangeRate.Size = new Size(239, 22);
        field_exchangeRate.TabIndex = 7;
        field_exchangeRate.Tag = "ACC-044:exchangeRate";
        field_exchangeRate.TextAlign = HorizontalAlignment.Right;
        // 
        // lbl_description
        // 
        lbl_description.Dock = DockStyle.Fill;
        lbl_description.Location = new Point(965, 106);
        lbl_description.Margin = new Padding(2);
        lbl_description.Name = "lbl_description";
        lbl_description.Size = new Size(111, 28);
        lbl_description.TabIndex = 12;
        lbl_description.Text = "البيان";
        lbl_description.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_description
        // 
        field_description.AccessibleName = "البيان";
        field_description.BackColor = Color.LightYellow;
        field_description.Dock = DockStyle.Fill;
        field_description.Location = new Point(724, 106);
        field_description.Margin = new Padding(2);
        field_description.Name = "field_description";
        field_description.Size = new Size(237, 22);
        field_description.TabIndex = 9;
        field_description.Tag = "ACC-044:description";
        // 
        // referenceLabel10
        // 
        referenceLabel10.Dock = DockStyle.Fill;
        referenceLabel10.Location = new Point(609, 106);
        referenceLabel10.Margin = new Padding(2);
        referenceLabel10.Name = "referenceLabel10";
        referenceLabel10.Size = new Size(111, 28);
        referenceLabel10.TabIndex = 13;
        referenceLabel10.Text = "رقم المرجع";
        referenceLabel10.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField10
        // 
        referenceField10.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField10.BackColor = Color.FromArgb(245, 245, 245);
        referenceField10.Dock = DockStyle.Fill;
        referenceField10.Location = new Point(368, 106);
        referenceField10.Margin = new Padding(2);
        referenceField10.Name = "referenceField10";
        referenceField10.ReadOnly = true;
        referenceField10.Size = new Size(237, 22);
        referenceField10.TabIndex = 14;
        referenceField10.TabStop = false;
        // 
        // referenceLabel11
        // 
        referenceLabel11.Dock = DockStyle.Fill;
        referenceLabel11.Location = new Point(253, 106);
        referenceLabel11.Margin = new Padding(2);
        referenceLabel11.Name = "referenceLabel11";
        referenceLabel11.Size = new Size(111, 28);
        referenceLabel11.TabIndex = 15;
        referenceLabel11.Text = "اسم المرجع";
        referenceLabel11.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField11
        // 
        referenceField11.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField11.BackColor = Color.FromArgb(245, 245, 245);
        referenceField11.Dock = DockStyle.Fill;
        referenceField11.Location = new Point(10, 106);
        referenceField11.Margin = new Padding(2);
        referenceField11.Name = "referenceField11";
        referenceField11.ReadOnly = true;
        referenceField11.Size = new Size(239, 22);
        referenceField11.TabIndex = 16;
        referenceField11.TabStop = false;
        // 
        // referenceLabel12
        // 
        referenceLabel12.Dock = DockStyle.Fill;
        referenceLabel12.Location = new Point(965, 138);
        referenceLabel12.Margin = new Padding(2);
        referenceLabel12.Name = "referenceLabel12";
        referenceLabel12.Size = new Size(111, 28);
        referenceLabel12.TabIndex = 17;
        referenceLabel12.Text = "عدد المرفقات";
        referenceLabel12.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField12
        // 
        referenceField12.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField12.BackColor = Color.FromArgb(245, 245, 245);
        referenceField12.Dock = DockStyle.Fill;
        referenceField12.Location = new Point(724, 138);
        referenceField12.Margin = new Padding(2);
        referenceField12.Name = "referenceField12";
        referenceField12.ReadOnly = true;
        referenceField12.Size = new Size(237, 22);
        referenceField12.TabIndex = 18;
        referenceField12.TabStop = false;
        // 
        // referenceLabel13
        // 
        referenceLabel13.Dock = DockStyle.Fill;
        referenceLabel13.Location = new Point(609, 138);
        referenceLabel13.Margin = new Padding(2);
        referenceLabel13.Name = "referenceLabel13";
        referenceLabel13.Size = new Size(111, 28);
        referenceLabel13.TabIndex = 19;
        referenceLabel13.Text = "طريقة الترحيل";
        referenceLabel13.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField13
        // 
        referenceField13.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField13.BackColor = Color.FromArgb(245, 245, 245);
        referenceField13.Dock = DockStyle.Fill;
        referenceField13.Location = new Point(368, 138);
        referenceField13.Margin = new Padding(2);
        referenceField13.Name = "referenceField13";
        referenceField13.ReadOnly = true;
        referenceField13.Size = new Size(237, 22);
        referenceField13.TabIndex = 20;
        referenceField13.TabStop = false;
        // 
        // referenceLabel14
        // 
        referenceLabel14.Dock = DockStyle.Fill;
        referenceLabel14.Location = new Point(253, 138);
        referenceLabel14.Margin = new Padding(2);
        referenceLabel14.Name = "referenceLabel14";
        referenceLabel14.Size = new Size(111, 28);
        referenceLabel14.TabIndex = 21;
        referenceLabel14.Text = "نوع الوثيقة";
        referenceLabel14.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField14
        // 
        referenceField14.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField14.BackColor = Color.FromArgb(245, 245, 245);
        referenceField14.Dock = DockStyle.Fill;
        referenceField14.Location = new Point(10, 138);
        referenceField14.Margin = new Padding(2);
        referenceField14.Name = "referenceField14";
        referenceField14.ReadOnly = true;
        referenceField14.Size = new Size(239, 22);
        referenceField14.TabIndex = 22;
        referenceField14.TabStop = false;
        // 
        // referenceLabel15
        // 
        referenceLabel15.Dock = DockStyle.Fill;
        referenceLabel15.Location = new Point(965, 170);
        referenceLabel15.Margin = new Padding(2);
        referenceLabel15.Name = "referenceLabel15";
        referenceLabel15.Size = new Size(111, 28);
        referenceLabel15.TabIndex = 23;
        referenceLabel15.Text = "رقم الوثيقة";
        referenceLabel15.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField15
        // 
        referenceField15.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField15.BackColor = Color.FromArgb(245, 245, 245);
        referenceField15.Dock = DockStyle.Fill;
        referenceField15.Location = new Point(724, 170);
        referenceField15.Margin = new Padding(2);
        referenceField15.Name = "referenceField15";
        referenceField15.ReadOnly = true;
        referenceField15.Size = new Size(237, 22);
        referenceField15.TabIndex = 24;
        referenceField15.TabStop = false;
        // 
        // referenceLabel16
        // 
        referenceLabel16.Dock = DockStyle.Fill;
        referenceLabel16.Location = new Point(609, 170);
        referenceLabel16.Margin = new Padding(2);
        referenceLabel16.Name = "referenceLabel16";
        referenceLabel16.Size = new Size(111, 28);
        referenceLabel16.TabIndex = 25;
        referenceLabel16.Text = "رقم المركز";
        referenceLabel16.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField16
        // 
        referenceField16.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField16.BackColor = Color.FromArgb(245, 245, 245);
        referenceField16.Dock = DockStyle.Fill;
        referenceField16.Location = new Point(368, 170);
        referenceField16.Margin = new Padding(2);
        referenceField16.Name = "referenceField16";
        referenceField16.ReadOnly = true;
        referenceField16.Size = new Size(237, 22);
        referenceField16.TabIndex = 26;
        referenceField16.TabStop = false;
        // 
        // referenceLabel17
        // 
        referenceLabel17.Dock = DockStyle.Fill;
        referenceLabel17.Location = new Point(253, 170);
        referenceLabel17.Margin = new Padding(2);
        referenceLabel17.Name = "referenceLabel17";
        referenceLabel17.Size = new Size(111, 28);
        referenceLabel17.TabIndex = 27;
        referenceLabel17.Text = "رقم المشروع";
        referenceLabel17.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField17
        // 
        referenceField17.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField17.BackColor = Color.FromArgb(245, 245, 245);
        referenceField17.Dock = DockStyle.Fill;
        referenceField17.Location = new Point(10, 170);
        referenceField17.Margin = new Padding(2);
        referenceField17.Name = "referenceField17";
        referenceField17.ReadOnly = true;
        referenceField17.Size = new Size(239, 22);
        referenceField17.TabIndex = 28;
        referenceField17.TabStop = false;
        // 
        // referenceLabel18
        // 
        referenceLabel18.Dock = DockStyle.Fill;
        referenceLabel18.Location = new Point(965, 202);
        referenceLabel18.Margin = new Padding(2);
        referenceLabel18.Name = "referenceLabel18";
        referenceLabel18.Size = new Size(111, 28);
        referenceLabel18.TabIndex = 29;
        referenceLabel18.Text = "رقم النشاط";
        referenceLabel18.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField18
        // 
        referenceField18.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField18.BackColor = Color.FromArgb(245, 245, 245);
        referenceField18.Dock = DockStyle.Fill;
        referenceField18.Location = new Point(724, 202);
        referenceField18.Margin = new Padding(2);
        referenceField18.Name = "referenceField18";
        referenceField18.ReadOnly = true;
        referenceField18.Size = new Size(237, 22);
        referenceField18.TabIndex = 30;
        referenceField18.TabStop = false;
        // 
        // referenceLabel19
        // 
        referenceLabel19.Dock = DockStyle.Fill;
        referenceLabel19.Location = new Point(609, 202);
        referenceLabel19.Margin = new Padding(2);
        referenceLabel19.Name = "referenceLabel19";
        referenceLabel19.Size = new Size(111, 28);
        referenceLabel19.TabIndex = 31;
        referenceLabel19.Text = "رقم الشيك";
        referenceLabel19.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField19
        // 
        referenceField19.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField19.BackColor = Color.FromArgb(245, 245, 245);
        referenceField19.Dock = DockStyle.Fill;
        referenceField19.Location = new Point(368, 202);
        referenceField19.Margin = new Padding(2);
        referenceField19.Name = "referenceField19";
        referenceField19.ReadOnly = true;
        referenceField19.Size = new Size(237, 22);
        referenceField19.TabIndex = 32;
        referenceField19.TabStop = false;
        // 
        // referenceLabel20
        // 
        referenceLabel20.Dock = DockStyle.Fill;
        referenceLabel20.Location = new Point(253, 202);
        referenceLabel20.Margin = new Padding(2);
        referenceLabel20.Name = "referenceLabel20";
        referenceLabel20.Size = new Size(111, 28);
        referenceLabel20.TabIndex = 33;
        referenceLabel20.Text = "تاريخ الاستحقاق";
        referenceLabel20.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField20
        // 
        referenceField20.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField20.BackColor = Color.FromArgb(245, 245, 245);
        referenceField20.Dock = DockStyle.Fill;
        referenceField20.Location = new Point(10, 202);
        referenceField20.Margin = new Padding(2);
        referenceField20.Name = "referenceField20";
        referenceField20.ReadOnly = true;
        referenceField20.Size = new Size(239, 22);
        referenceField20.TabIndex = 34;
        referenceField20.TabStop = false;
        // 
        // referenceAudit
        // 
        referenceAudit.BackColor = Color.FromArgb(225, 223, 247);
        referenceAudit.Dock = DockStyle.Fill;
        referenceAudit.Location = new Point(3, 658);
        referenceAudit.Name = "referenceAudit";
        referenceAudit.Size = new Size(1094, 36);
        referenceAudit.TabIndex = 3;
        referenceAudit.Text = "مدخل السجل: —    تاريخ الإدخال: —    الجهاز: —\r\nمعدل السجل: —    تاريخ التعديل: —    مرات التعديل: —    مرات الطباعة: —";
        referenceAudit.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceTotals
        // 
        referenceTotals.BackColor = Color.FromArgb(250, 223, 249);
        referenceTotals.Dock = DockStyle.Fill;
        referenceTotals.Location = new Point(3, 626);
        referenceTotals.Name = "referenceTotals";
        referenceTotals.Size = new Size(1094, 32);
        referenceTotals.TabIndex = 2;
        referenceTotals.Text = "الإجمالي: —          الفارق: —";
        referenceTotals.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // referenceCol0
        // 
        referenceCol0.HeaderText = "الحساب التحليلي";
        referenceCol0.MinimumWidth = 6;
        referenceCol0.Name = "referenceCol0";
        referenceCol0.ReadOnly = true;
        referenceCol0.ToolTipText = "للعرض؛ يحتاج ربطًا بخدمة السند";
        referenceCol0.Width = 140;
        // 
        // referenceCol1
        // 
        referenceCol1.HeaderText = "اسم الحساب";
        referenceCol1.MinimumWidth = 6;
        referenceCol1.Name = "referenceCol1";
        referenceCol1.ReadOnly = true;
        referenceCol1.ToolTipText = "للعرض؛ يحتاج ربطًا بخدمة السند";
        referenceCol1.Width = 140;
        // 
        // referenceCol3
        // 
        referenceCol3.HeaderText = "المبلغ الأجنبي";
        referenceCol3.MinimumWidth = 6;
        referenceCol3.Name = "referenceCol3";
        referenceCol3.ReadOnly = true;
        referenceCol3.ToolTipText = "للعرض؛ يحتاج ربطًا بخدمة السند";
        referenceCol3.Width = 140;
        // 
        // referencePrint
        // 
        referencePrint.AutoSize = true;
        referencePrint.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        referencePrint.Enabled = false;
        referencePrint.Location = new Point(34, 3);
        referencePrint.MinimumSize = new Size(88, 36);
        referencePrint.Name = "referencePrint";
        referencePrint.Size = new Size(88, 36);
        referencePrint.TabIndex = 10;
        referencePrint.Text = "طباعة";
        // 
        // validationErrors
        // 
        validationErrors.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        validationErrors.ContainerControl = this;
        // 
        // mainLayout
        // 
        mainLayout.ColumnCount = 1;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        mainLayout.Controls.Add(lblTitle, 0, 0);
        mainLayout.Controls.Add(designerCommandBar, 0, 1);
        mainLayout.Controls.Add(tabs, 0, 2);
        mainLayout.Controls.Add(dgvLines, 0, 3);
        mainLayout.Controls.Add(referenceTotals, 0, 4);
        mainLayout.Controls.Add(referenceAudit, 0, 5);
        mainLayout.Controls.Add(lblStatus, 0, 6);
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Location = new Point(0, 0);
        mainLayout.MinimumSize = new Size(980, 680);
        mainLayout.Name = "mainLayout";
        mainLayout.RowCount = 7;
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 279F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.Size = new Size(1100, 720);
        mainLayout.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.BackColor = Color.FromArgb(232, 232, 242);
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.ForeColor = Color.FromArgb(35, 35, 50);
        lblTitle.Location = new Point(0, 0);
        lblTitle.Margin = new Padding(0);
        lblTitle.Name = "lblTitle";
        lblTitle.Padding = new Padding(8);
        lblTitle.Size = new Size(1100, 34);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "سند الصرف";
        // 
        // flpActions
        // 
        flpActions.AutoScroll = true;
        flpActions.BackColor = Color.FromArgb(239, 239, 244);
        flpActions.Controls.Add(btnView);
        flpActions.Controls.Add(btnCreate);
        flpActions.Controls.Add(btnEdit);
        flpActions.Controls.Add(btnCancel);
        flpActions.Controls.Add(btnPost);
        flpActions.Controls.Add(btnReverse);
        flpActions.Controls.Add(btnClear);
        flpActions.Controls.Add(btnClose);
        flpActions.Controls.Add(btnAddRow);
        flpActions.Controls.Add(btnRemoveRow);
        flpActions.Controls.Add(referencePrint);
        flpActions.Dock = DockStyle.Fill;
        flpActions.Location = new Point(3, 37);
        flpActions.Name = "flpActions";
        flpActions.RightToLeft = RightToLeft.Yes;
        flpActions.Size = new Size(1094, 58);
        flpActions.TabIndex = 1;
        flpActions.WrapContents = false;
        flpActions.Paint += flpActions_Paint;
        // 
        // btnView
        // 
        btnView.AccessibleName = "عرض / إعادة تحميل";
        btnView.AutoSize = true;
        btnView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnView.Enabled = false;
        btnView.Location = new Point(974, 3);
        btnView.MinimumSize = new Size(88, 36);
        btnView.Name = "btnView";
        btnView.Size = new Size(117, 36);
        btnView.TabIndex = 0;
        btnView.Text = "عرض / إعادة تحميل";
        // 
        // btnCreate
        // 
        btnCreate.AccessibleName = "حفظ جديد";
        btnCreate.AutoSize = true;
        btnCreate.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnCreate.Enabled = false;
        btnCreate.Location = new Point(880, 3);
        btnCreate.MinimumSize = new Size(88, 36);
        btnCreate.Name = "btnCreate";
        btnCreate.Size = new Size(88, 36);
        btnCreate.TabIndex = 1;
        btnCreate.Text = "حفظ جديد";
        // 
        // btnEdit
        // 
        btnEdit.AccessibleName = "حفظ التعديل";
        btnEdit.AutoSize = true;
        btnEdit.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnEdit.Enabled = false;
        btnEdit.Location = new Point(786, 3);
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
        btnCancel.Location = new Point(692, 3);
        btnCancel.MinimumSize = new Size(88, 36);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(88, 36);
        btnCancel.TabIndex = 3;
        btnCancel.Text = "إلغاء المستند";
        // 
        // btnPost
        // 
        btnPost.AccessibleName = "ترحيل";
        btnPost.AutoSize = true;
        btnPost.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnPost.Enabled = false;
        btnPost.Location = new Point(598, 3);
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
        btnReverse.Location = new Point(504, 3);
        btnReverse.MinimumSize = new Size(88, 36);
        btnReverse.Name = "btnReverse";
        btnReverse.Size = new Size(88, 36);
        btnReverse.TabIndex = 5;
        btnReverse.Text = "عكس القيد";
        // 
        // btnClear
        // 
        btnClear.AccessibleName = "تفريغ المسودة";
        btnClear.AutoSize = true;
        btnClear.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnClear.Location = new Point(410, 3);
        btnClear.MinimumSize = new Size(88, 36);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(88, 36);
        btnClear.TabIndex = 6;
        btnClear.Text = "تفريغ المسودة";
        // 
        // btnClose
        // 
        btnClose.AccessibleName = "إغلاق";
        btnClose.AutoSize = true;
        btnClose.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnClose.Location = new Point(316, 3);
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
        btnAddRow.Location = new Point(222, 3);
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
        btnRemoveRow.Location = new Point(128, 3);
        btnRemoveRow.MinimumSize = new Size(88, 36);
        btnRemoveRow.Name = "btnRemoveRow";
        btnRemoveRow.Size = new Size(88, 36);
        btnRemoveRow.TabIndex = 9;
        btnRemoveRow.Text = "حذف سطر";
        // 
        // tabs
        // 
        tabs.Controls.Add(tp0);
        tabs.Controls.Add(documentAccounts);
        tabs.Controls.Add(documentAdditional);
        tabs.Controls.Add(documentDefaults);
        tabs.Controls.Add(documentImport);
        tabs.Dock = DockStyle.Fill;
        tabs.Font = new Font("Tahoma", 9F);
        tabs.Location = new Point(3, 101);
        tabs.Multiline = true;
        tabs.Name = "tabs";
        tabs.RightToLeft = RightToLeft.Yes;
        tabs.RightToLeftLayout = true;
        tabs.SelectedIndex = 0;
        tabs.Size = new Size(1094, 273);
        tabs.TabIndex = 1;
        // 
        // tp0
        // 
        tp0.AutoScroll = true;
        tp0.Controls.Add(referenceHeader);
        tp0.Location = new Point(4, 23);
        tp0.Name = "tp0";
        tp0.Size = new Size(1086, 246);
        tp0.TabIndex = 0;
        tp0.Text = "البيانات الرئيسية";
        // 
        // dgvLines
        // 
        dgvLines.AccessibleName = "التفاصيل والحركات";
        dgvLines.AllowUserToAddRows = false;
        dgvLines.AllowUserToDeleteRows = false;
        dataGridViewCellStyle11.BackColor = Color.White;
        dgvLines.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle11;
        dgvLines.BackgroundColor = Color.White;
        dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle12.BackColor = Color.FromArgb(232, 232, 232);
        dataGridViewCellStyle12.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle12.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle12.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle12.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle12.WrapMode = DataGridViewTriState.True;
        dgvLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle12;
        dgvLines.ColumnHeadersHeight = 34;
        dgvLines.Columns.AddRange(new DataGridViewColumn[] { col_rowNo, col_counterAccountRef, referenceCol0, referenceCol1, col_lineDescription, col_currencyRef, col_exchangeRate, col_amount, referenceCol3, col_partyRef, col_accountingAmount });
        dataGridViewCellStyle16.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle16.BackColor = Color.FromArgb(255, 255, 226);
        dataGridViewCellStyle16.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle16.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle16.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle16.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle16.WrapMode = DataGridViewTriState.False;
        dgvLines.DefaultCellStyle = dataGridViewCellStyle16;
        dgvLines.Dock = DockStyle.Fill;
        dgvLines.EnableHeadersVisualStyles = false;
        dgvLines.Location = new Point(3, 380);
        dgvLines.MinimumSize = new Size(900, 80);
        dgvLines.MultiSelect = false;
        dgvLines.Name = "dgvLines";
        dgvLines.RightToLeft = RightToLeft.Yes;
        dgvLines.RowHeadersVisible = false;
        dgvLines.RowHeadersWidth = 51;
        dgvLines.RowTemplate.Height = 27;
        dgvLines.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvLines.Size = new Size(1094, 243);
        dgvLines.TabIndex = 1;
        dgvLines.CellContentClick += dgvLines_CellContentClick;
        // 
        // col_rowNo
        // 
        col_rowNo.HeaderText = "#";
        col_rowNo.MinimumWidth = 90;
        col_rowNo.Name = "col_rowNo";
        col_rowNo.ReadOnly = true;
        col_rowNo.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_rowNo.Width = 140;
        // 
        // col_counterAccountRef
        // 
        col_counterAccountRef.HeaderText = "الحساب المقابل";
        col_counterAccountRef.MinimumWidth = 90;
        col_counterAccountRef.Name = "col_counterAccountRef";
        col_counterAccountRef.Width = 140;
        // 
        // col_lineDescription
        // 
        col_lineDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        col_lineDescription.HeaderText = "البيان";
        col_lineDescription.MinimumWidth = 90;
        col_lineDescription.Name = "col_lineDescription";
        col_lineDescription.SortMode = DataGridViewColumnSortMode.NotSortable;
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
        dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleRight;
        col_exchangeRate.DefaultCellStyle = dataGridViewCellStyle13;
        col_exchangeRate.HeaderText = "سعر الصرف";
        col_exchangeRate.MinimumWidth = 90;
        col_exchangeRate.Name = "col_exchangeRate";
        col_exchangeRate.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_exchangeRate.Width = 140;
        // 
        // col_amount
        // 
        dataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.MiddleRight;
        col_amount.DefaultCellStyle = dataGridViewCellStyle14;
        col_amount.HeaderText = "المبلغ";
        col_amount.MinimumWidth = 90;
        col_amount.Name = "col_amount";
        col_amount.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_amount.Width = 140;
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
        dataGridViewCellStyle15.Alignment = DataGridViewContentAlignment.MiddleRight;
        col_accountingAmount.DefaultCellStyle = dataGridViewCellStyle15;
        col_accountingAmount.HeaderText = "المبلغ المحاسبي";
        col_accountingAmount.MinimumWidth = 90;
        col_accountingAmount.Name = "col_accountingAmount";
        col_accountingAmount.ReadOnly = true;
        col_accountingAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_accountingAmount.Width = 140;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.Location = new Point(3, 694);
        lblStatus.Name = "lblStatus";
        lblStatus.Padding = new Padding(6);
        lblStatus.Size = new Size(1094, 26);
        lblStatus.TabIndex = 4;
        lblStatus.Text = "مسودة واجهة غير محفوظة — الخدمات غير موصولة";
        // 
        // UcScreen_04_04_02
        // 
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScroll = true;
        BackColor = Color.FromArgb(240, 240, 240);
        rootWorkspaceViewport.Name = "rootWorkspaceViewport";
        rootWorkspaceViewport.Dock = DockStyle.Fill;
        rootWorkspaceViewport.AutoScroll = true;
        rootWorkspaceViewport.AutoScrollMinSize = new Size(1225, 850);
        rootWorkspaceViewport.Margin = Padding.Empty;
        rootWorkspaceViewport.Controls.Add(mainLayout);
        Controls.Add(rootWorkspaceViewport);
        Font = new Font("Tahoma", 9F);
        Name = "UcScreen_04_04_02";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1100, 720);
        Tag = "04.04.02";
        documentAdditional.ResumeLayout(false);
        documentAdditionalSections.ResumeLayout(false);
        tp1.ResumeLayout(false);
        linesHost.ResumeLayout(false);
        linesHost.PerformLayout();
        tp2.ResumeLayout(false);
        tp2.PerformLayout();
        layout2.ResumeLayout(false);
        tp3.ResumeLayout(false);
        tp3.PerformLayout();
        layout3.ResumeLayout(false);
        tp4.ResumeLayout(false);
        tp4.PerformLayout();
        layout4.ResumeLayout(false);
        referenceExtra.ResumeLayout(false);
        referenceExtra.PerformLayout();
        layout0.ResumeLayout(false);
        layout0.PerformLayout();
        documentDefaults.ResumeLayout(false);
        documentDefaults.PerformLayout();
        documentDefaultsLayout.ResumeLayout(false);
        documentDefaultsLayout.PerformLayout();
        documentAccounts.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)documentAccountsGrid).EndInit();
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
        flpActions.ResumeLayout(false);
        flpActions.PerformLayout();
        tabs.ResumeLayout(false);
        tp0.ResumeLayout(false);
        tp0.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
        ResumeLayout(false);
    
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Fill;
        designerCommandBar.Location = new Point(3, 37);
        designerCommandBar.Size = new Size(1094, 58);
        designerCommandBar.TabIndex = 1;
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
        standardCommandAdd.Name = "standardCommandAdd";
        standardCommandAdd.Enabled = false;
        standardCommandAdd.Visible = false;
        standardCommandAdd.AccessibleName = "إضافة";
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
        standardCommandSave.Name = "standardCommandSave";
        standardCommandSave.Enabled = false;
        standardCommandSave.Visible = false;
        standardCommandSave.AccessibleName = "حفظ";
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
        designerHiddenCommands.Controls.Add(standardCommandAdd);
        designerCommandBar.SetCommandRole(standardCommandAdd, TransportERP.Desktop.CoreUI.DesignerCommandRole.Add);
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
        standardCommandSave.AutoSize = false;
        standardCommandSave.Dock = DockStyle.None;
        standardCommandSave.MinimumSize = Size.Empty;
        standardCommandSave.Size = new Size(26, 24);
        standardCommandSave.Margin = new Padding(1);
        standardCommandSave.FlatStyle = FlatStyle.Flat;
        standardCommandSave.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandSave.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Save;
        standardCommandSave.Text = "";
        designerHiddenCommands.Controls.Add(standardCommandSave);
        designerCommandBar.SetCommandRole(standardCommandSave, TransportERP.Desktop.CoreUI.DesignerCommandRole.Save);
        designerCommandBar.SetCommandRole(referencePrint, TransportERP.Desktop.CoreUI.DesignerCommandRole.Print);
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
        standardAuditMetadata.Size = new Size(800, 64);
        referenceAudit.Visible = false;
        Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}