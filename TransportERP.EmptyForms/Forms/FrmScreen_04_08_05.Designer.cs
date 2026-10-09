#nullable enable
using System.Drawing;
using System.Windows.Forms;
namespace TransportERP.EmptyForms;

partial class UcScreen_04_08_05
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
    private Button standardCommandRefresh = null!;
    private Button standardCommandImport = null!;
    private Button standardCommandExport = null!;
    private Button standardCommandHelp = null!;

    private TableLayoutPanel mainLayout = null!;
    private Label lblTitle = null!;
    private FlowLayoutPanel flpActions = null!;
    private Button btnSave = null!;
    private Button btnApprove = null!;
    private Button btnClear = null!;
    private Button btnClose = null!;
    private TabControl tabs = null!;
    private Label lblStatus = null!;
    private TabPage tp0 = null!;
    private TableLayoutPanel layout0 = null!;
    private ComboBox field_T04_EV_0267 = null!;
    private Label lbl_T04_EV_0267 = null!;
    private ComboBox field_T04_EV_0268 = null!;
    private Label lbl_T04_EV_0268 = null!;
    private TextBox field_T04_EV_0269 = null!;
    private Label lbl_T04_EV_0269 = null!;
    private DateTimePicker field_T04_EV_0270 = null!;
    private Label lbl_T04_EV_0270 = null!;
    private TextBox field_T04_EV_0271 = null!;
    private Label lbl_T04_EV_0271 = null!;
    private TextBox field_T04_EV_0272 = null!;
    private Label lbl_T04_EV_0272 = null!;
    private TextBox field_T04_EV_0273 = null!;
    private Label lbl_T04_EV_0273 = null!;
    private TextBox field_T04_EV_0274 = null!;
    private Label lbl_T04_EV_0274 = null!;
    private CheckBox field_T04_EV_0277 = null!;
    private Label lbl_T04_EV_0277 = null!;
    private CheckBox field_T04_EV_0279 = null!;
    private Label lbl_T04_EV_0279 = null!;
    private CheckBox field_R05_AT_0292 = null!;
    private Label lbl_R05_AT_0292 = null!;
    private CheckBox field_R05_AT_0293 = null!;
    private Label lbl_R05_AT_0293 = null!;
    private ComboBox field_T04_EV_0278 = null!;
    private Label lbl_T04_EV_0278 = null!;
    private TextBox field_T04_EV_0275 = null!;
    private Label lbl_T04_EV_0275 = null!;
    private TextBox field_T04_EV_0280 = null!;
    private Label lbl_T04_EV_0280 = null!;
    private TextBox field_T04_EV_0281 = null!;
    private Label lbl_T04_EV_0281 = null!;
    private TextBox field_R05_AT_0296 = null!;
    private Label lbl_R05_AT_0296 = null!;
    private TextBox field_R05_AT_0297 = null!;
    private Label lbl_R05_AT_0297 = null!;
    private TextBox field_R05_AT_0298 = null!;
    private Label lbl_R05_AT_0298 = null!;
    private TabPage tp1 = null!;
    private TableLayoutPanel layout1 = null!;
    private TextBox txtCsvPath = null!;
    private FlowLayoutPanel csvActions = null!;
    private Button btnChooseCsv = null!;
    private Button btnImportCsv = null!;
    private TextBox txtCsvSchema = null!;
    private DataGridView dgvImport = null!;
    private TabPage tp2 = null!;
    private TableLayoutPanel layout2 = null!;
    private TextBox field_localNotes = null!;
    private Label lbl_localNotes = null!;
    private TabPage tp3 = null!;
    private TableLayoutPanel layout3 = null!;
    private DataGridView dgvLines = null!;
    private DataGridViewTextBoxColumn col_rowNo = null!;
    private DataGridViewTextBoxColumn col_T04_EV_0287 = null!;
    private DataGridViewComboBoxColumn col_T04_EV_0470 = null!;
    private DataGridViewTextBoxColumn col_T04_EV_0471 = null!;
    private DataGridViewTextBoxColumn col_T04_EV_0472 = null!;
    private DataGridViewTextBoxColumn col_T04_EV_0473 = null!;
    private DataGridViewTextBoxColumn col_T04_EV_0474 = null!;
    private DataGridViewTextBoxColumn col_T04_EV_0475 = null!;
    private DataGridViewComboBoxColumn col_R05_AT_0294 = null!;
    private DataGridViewTextBoxColumn col_R05_AT_0295 = null!;
    private TableLayoutPanel linesHost = null!;
    private FlowLayoutPanel lineActions = null!;
    private Button btnAddRow = null!;
    private Button btnRemoveRow = null!;
    private System.ComponentModel.IContainer? components;
    private ErrorProvider validationErrors = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }
    private TableLayoutPanel referenceHeader = null!;
    private TabPage referenceExtra = null!;
    private Label referenceAudit = null!;
    private Label referenceTotals = null!;
    private Label referenceLabel7 = null!;
    private TextBox referenceField7 = null!;
    private Label referenceLabel8 = null!;
    private TextBox referenceField8 = null!;
    private DataGridViewTextBoxColumn referenceCol0 = null!;
    private DataGridViewTextBoxColumn referenceCol2 = null!;
    private Button referencePrint = null!;
    private TabPage documentAdditional = null!;
    private TabControl documentAdditionalSections = null!;
    private TabPage documentDefaults = null!;
    private TableLayoutPanel documentDefaultsLayout = null!;
    private Label lblDefaultCurrency = null!;
    private ComboBox cboDefaultCurrency = null!;
    private Label lblDefaultCostCenter = null!;
    private ComboBox cboDefaultCostCenter = null!;
    private System.Windows.Forms.Panel rootWorkspaceViewport;
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
        standardCommandRefresh = new Button();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
        components = new System.ComponentModel.Container();
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
        documentAdditional = new TabPage();
        documentAdditionalSections = new TabControl();
        tp2 = new TabPage();
        layout2 = new TableLayoutPanel();
        lbl_localNotes = new Label();
        field_localNotes = new TextBox();
        referenceExtra = new TabPage();
        layout0 = new TableLayoutPanel();
        lbl_R05_AT_0292 = new Label();
        field_R05_AT_0292 = new CheckBox();
        lbl_R05_AT_0293 = new Label();
        field_R05_AT_0293 = new CheckBox();
        lbl_T04_EV_0275 = new Label();
        field_T04_EV_0275 = new TextBox();
        lbl_R05_AT_0296 = new Label();
        field_R05_AT_0296 = new TextBox();
        lbl_R05_AT_0297 = new Label();
        field_R05_AT_0297 = new TextBox();
        lbl_R05_AT_0298 = new Label();
        field_R05_AT_0298 = new TextBox();
        documentDefaults = new TabPage();
        documentDefaultsLayout = new TableLayoutPanel();
        lblDefaultCurrency = new Label();
        cboDefaultCurrency = new ComboBox();
        lblDefaultCostCenter = new Label();
        cboDefaultCostCenter = new ComboBox();
        referenceHeader = new TableLayoutPanel();
        lbl_T04_EV_0267 = new Label();
        field_T04_EV_0267 = new ComboBox();
        lbl_T04_EV_0268 = new Label();
        field_T04_EV_0268 = new ComboBox();
        lbl_T04_EV_0269 = new Label();
        field_T04_EV_0269 = new TextBox();
        lbl_T04_EV_0270 = new Label();
        field_T04_EV_0270 = new DateTimePicker();
        lbl_T04_EV_0271 = new Label();
        field_T04_EV_0271 = new TextBox();
        lbl_T04_EV_0272 = new Label();
        field_T04_EV_0272 = new TextBox();
        lbl_T04_EV_0273 = new Label();
        field_T04_EV_0273 = new TextBox();
        referenceLabel7 = new Label();
        referenceField7 = new TextBox();
        referenceLabel8 = new Label();
        referenceField8 = new TextBox();
        lbl_T04_EV_0274 = new Label();
        field_T04_EV_0274 = new TextBox();
        lbl_T04_EV_0278 = new Label();
        field_T04_EV_0278 = new ComboBox();
        lbl_T04_EV_0277 = new Label();
        field_T04_EV_0277 = new CheckBox();
        lbl_T04_EV_0279 = new Label();
        field_T04_EV_0279 = new CheckBox();
        lbl_T04_EV_0280 = new Label();
        field_T04_EV_0280 = new TextBox();
        lbl_T04_EV_0281 = new Label();
        field_T04_EV_0281 = new TextBox();
        referenceAudit = new Label();
        referenceTotals = new Label();
        referenceCol0 = new DataGridViewTextBoxColumn();
        referenceCol2 = new DataGridViewTextBoxColumn();
        referencePrint = new Button();
        validationErrors = new ErrorProvider(components);
        mainLayout = new TableLayoutPanel();
        lblTitle = new Label();
        flpActions = new FlowLayoutPanel();
        btnSave = new Button();
        btnApprove = new Button();
        btnClear = new Button();
        btnClose = new Button();
        tabs = new TabControl();
        tp3 = new TabPage();
        layout3 = new TableLayoutPanel();
        linesHost = new TableLayoutPanel();
        lineActions = new FlowLayoutPanel();
        btnAddRow = new Button();
        btnRemoveRow = new Button();
        tp0 = new TabPage();
        tp1 = new TabPage();
        layout1 = new TableLayoutPanel();
        txtCsvPath = new TextBox();
        csvActions = new FlowLayoutPanel();
        btnChooseCsv = new Button();
        btnImportCsv = new Button();
        txtCsvSchema = new TextBox();
        dgvImport = new DataGridView();
        dgvLines = new DataGridView();
        col_rowNo = new DataGridViewTextBoxColumn();
        col_T04_EV_0287 = new DataGridViewTextBoxColumn();
        col_T04_EV_0470 = new DataGridViewComboBoxColumn();
        col_T04_EV_0471 = new DataGridViewTextBoxColumn();
        col_T04_EV_0472 = new DataGridViewTextBoxColumn();
        col_T04_EV_0473 = new DataGridViewTextBoxColumn();
        col_T04_EV_0474 = new DataGridViewTextBoxColumn();
        col_T04_EV_0475 = new DataGridViewTextBoxColumn();
        col_R05_AT_0294 = new DataGridViewComboBoxColumn();
        col_R05_AT_0295 = new DataGridViewTextBoxColumn();
        lblStatus = new Label();
        documentAdditional.SuspendLayout();
        tp2.SuspendLayout();
        layout2.SuspendLayout();
        layout0.SuspendLayout();
        documentDefaults.SuspendLayout();
        documentDefaultsLayout.SuspendLayout();
        referenceHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)validationErrors).BeginInit();
        mainLayout.SuspendLayout();
        flpActions.SuspendLayout();
        tabs.SuspendLayout();
        tp3.SuspendLayout();
        linesHost.SuspendLayout();
        lineActions.SuspendLayout();
        tp0.SuspendLayout();
        tp1.SuspendLayout();
        layout1.SuspendLayout();
        csvActions.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvImport).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
        SuspendLayout();
        // 
        // documentAdditional
        // 
        documentAdditional.AutoScroll = true;
        documentAdditional.Controls.Add(documentAdditionalSections);
        documentAdditional.Location = new Point(4, 23);
        documentAdditional.Name = "documentAdditional";
        documentAdditional.Size = new Size(1086, 182);
        documentAdditional.TabIndex = 4;
        documentAdditional.Text = "بيانات إضافية";
        // 
        // documentAdditionalSections
        // 
        documentAdditionalSections.Dock = DockStyle.Fill;
        documentAdditionalSections.Location = new Point(0, 0);
        documentAdditionalSections.Name = "documentAdditionalSections";
        documentAdditionalSections.RightToLeft = RightToLeft.Yes;
        documentAdditionalSections.RightToLeftLayout = true;
        documentAdditionalSections.SelectedIndex = 0;
        documentAdditionalSections.Size = new Size(1086, 182);
        documentAdditionalSections.TabIndex = 0;
        // 
        // tp2
        // 
        tp2.AutoScroll = true;
        tp2.Controls.Add(layout2);
        tp2.Location = new Point(4, 23);
        tp2.Name = "tp2";
        tp2.Size = new Size(1086, 182);
        tp2.TabIndex = 2;
        tp2.Text = "بيانات إضافية";
        // 
        // layout2
        // 
        layout2.AutoScroll = true;
        layout2.AutoSize = true;
        layout2.ColumnCount = 2;
        layout2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 195F));
        layout2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout2.Controls.Add(lbl_localNotes, 0, 0);
        layout2.Controls.Add(field_localNotes, 1, 0);
        layout2.Dock = DockStyle.Top;
        layout2.Location = new Point(0, 0);
        layout2.MinimumSize = new Size(650, 0);
        layout2.Name = "layout2";
        layout2.RowCount = 1;
        layout2.RowStyles.Add(new RowStyle());
        layout2.Size = new Size(1086, 76);
        layout2.TabIndex = 0;
        // 
        // lbl_localNotes
        // 
        lbl_localNotes.AutoSize = true;
        lbl_localNotes.Dock = DockStyle.Fill;
        lbl_localNotes.Location = new Point(894, 0);
        lbl_localNotes.Name = "lbl_localNotes";
        lbl_localNotes.Size = new Size(189, 76);
        lbl_localNotes.TabIndex = 0;
        lbl_localNotes.Text = "ملاحظات";
        lbl_localNotes.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_localNotes
        // 
        field_localNotes.AccessibleName = "ملاحظات";
        field_localNotes.Dock = DockStyle.Fill;
        field_localNotes.Location = new Point(3, 3);
        field_localNotes.Multiline = true;
        field_localNotes.Name = "field_localNotes";
        field_localNotes.ScrollBars = ScrollBars.Vertical;
        field_localNotes.Size = new Size(885, 70);
        field_localNotes.TabIndex = 0;
        field_localNotes.Tag = "LOCAL:notes";
        // 
        // referenceExtra
        // 
        referenceExtra.AutoScroll = true;
        referenceExtra.Location = new Point(4, 23);
        referenceExtra.Name = "referenceExtra";
        referenceExtra.Size = new Size(1086, 182);
        referenceExtra.TabIndex = 3;
        referenceExtra.Text = "حقول تكميلية";
        // 
        // layout0
        // 
        layout0.AutoScroll = true;
        layout0.AutoSize = true;
        layout0.ColumnCount = 2;
        layout0.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 195F));
        layout0.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout0.Controls.Add(lbl_R05_AT_0292, 0, 10);
        layout0.Controls.Add(field_R05_AT_0292, 1, 10);
        layout0.Controls.Add(lbl_R05_AT_0293, 0, 11);
        layout0.Controls.Add(field_R05_AT_0293, 1, 11);
        layout0.Controls.Add(lbl_T04_EV_0275, 0, 13);
        layout0.Controls.Add(field_T04_EV_0275, 1, 13);
        layout0.Controls.Add(lbl_R05_AT_0296, 0, 16);
        layout0.Controls.Add(field_R05_AT_0296, 1, 16);
        layout0.Controls.Add(lbl_R05_AT_0297, 0, 17);
        layout0.Controls.Add(field_R05_AT_0297, 1, 17);
        layout0.Controls.Add(lbl_R05_AT_0298, 0, 18);
        layout0.Controls.Add(field_R05_AT_0298, 1, 18);
        layout0.Dock = DockStyle.Top;
        layout0.Location = new Point(0, 0);
        layout0.MinimumSize = new Size(650, 0);
        layout0.Name = "layout0";
        layout0.RowCount = 19;
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
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.RowStyles.Add(new RowStyle());
        layout0.Size = new Size(1069, 172);
        layout0.TabIndex = 0;
        // 
        // lbl_R05_AT_0292
        // 
        lbl_R05_AT_0292.AutoSize = true;
        lbl_R05_AT_0292.Dock = DockStyle.Fill;
        lbl_R05_AT_0292.Location = new Point(877, 0);
        lbl_R05_AT_0292.Name = "lbl_R05_AT_0292";
        lbl_R05_AT_0292.Size = new Size(189, 30);
        lbl_R05_AT_0292.TabIndex = 0;
        lbl_R05_AT_0292.Text = "يعكس عند إقفال الفترة";
        lbl_R05_AT_0292.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_R05_AT_0292
        // 
        field_R05_AT_0292.AccessibleName = "يعكس عند إقفال الفترة";
        field_R05_AT_0292.Checked = true;
        field_R05_AT_0292.CheckState = CheckState.Indeterminate;
        field_R05_AT_0292.Dock = DockStyle.Fill;
        field_R05_AT_0292.Location = new Point(3, 3);
        field_R05_AT_0292.Name = "field_R05_AT_0292";
        field_R05_AT_0292.Size = new Size(868, 24);
        field_R05_AT_0292.TabIndex = 10;
        field_R05_AT_0292.Tag = "R05-AT-0292";
        field_R05_AT_0292.ThreeState = true;
        // 
        // lbl_R05_AT_0293
        // 
        lbl_R05_AT_0293.AutoSize = true;
        lbl_R05_AT_0293.Dock = DockStyle.Fill;
        lbl_R05_AT_0293.Location = new Point(877, 30);
        lbl_R05_AT_0293.Name = "lbl_R05_AT_0293";
        lbl_R05_AT_0293.Size = new Size(189, 30);
        lbl_R05_AT_0293.TabIndex = 11;
        lbl_R05_AT_0293.Text = "يعكس عند إقفال الشهر";
        lbl_R05_AT_0293.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_R05_AT_0293
        // 
        field_R05_AT_0293.AccessibleName = "يعكس عند إقفال الشهر";
        field_R05_AT_0293.Checked = true;
        field_R05_AT_0293.CheckState = CheckState.Indeterminate;
        field_R05_AT_0293.Dock = DockStyle.Fill;
        field_R05_AT_0293.Location = new Point(3, 33);
        field_R05_AT_0293.Name = "field_R05_AT_0293";
        field_R05_AT_0293.Size = new Size(868, 24);
        field_R05_AT_0293.TabIndex = 11;
        field_R05_AT_0293.Tag = "R05-AT-0293";
        field_R05_AT_0293.ThreeState = true;
        // 
        // lbl_T04_EV_0275
        // 
        lbl_T04_EV_0275.AutoSize = true;
        lbl_T04_EV_0275.Dock = DockStyle.Fill;
        lbl_T04_EV_0275.Location = new Point(877, 60);
        lbl_T04_EV_0275.Name = "lbl_T04_EV_0275";
        lbl_T04_EV_0275.Size = new Size(189, 28);
        lbl_T04_EV_0275.TabIndex = 12;
        lbl_T04_EV_0275.Text = "مستخدم";
        lbl_T04_EV_0275.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T04_EV_0275
        // 
        field_T04_EV_0275.AccessibleName = "مستخدم";
        field_T04_EV_0275.Dock = DockStyle.Fill;
        field_T04_EV_0275.Location = new Point(3, 63);
        field_T04_EV_0275.Name = "field_T04_EV_0275";
        field_T04_EV_0275.ReadOnly = true;
        field_T04_EV_0275.Size = new Size(868, 22);
        field_T04_EV_0275.TabIndex = 13;
        field_T04_EV_0275.TabStop = false;
        field_T04_EV_0275.Tag = "T04-EV-0275";
        // 
        // lbl_R05_AT_0296
        // 
        lbl_R05_AT_0296.AutoSize = true;
        lbl_R05_AT_0296.Dock = DockStyle.Fill;
        lbl_R05_AT_0296.Location = new Point(877, 88);
        lbl_R05_AT_0296.Name = "lbl_R05_AT_0296";
        lbl_R05_AT_0296.Size = new Size(189, 28);
        lbl_R05_AT_0296.TabIndex = 14;
        lbl_R05_AT_0296.Text = "إجمالي المدين";
        lbl_R05_AT_0296.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_R05_AT_0296
        // 
        field_R05_AT_0296.AccessibleName = "إجمالي المدين";
        field_R05_AT_0296.Dock = DockStyle.Fill;
        field_R05_AT_0296.Location = new Point(3, 91);
        field_R05_AT_0296.Name = "field_R05_AT_0296";
        field_R05_AT_0296.ReadOnly = true;
        field_R05_AT_0296.Size = new Size(868, 22);
        field_R05_AT_0296.TabIndex = 16;
        field_R05_AT_0296.TabStop = false;
        field_R05_AT_0296.Tag = "R05-AT-0296";
        // 
        // lbl_R05_AT_0297
        // 
        lbl_R05_AT_0297.AutoSize = true;
        lbl_R05_AT_0297.Dock = DockStyle.Fill;
        lbl_R05_AT_0297.Location = new Point(877, 116);
        lbl_R05_AT_0297.Name = "lbl_R05_AT_0297";
        lbl_R05_AT_0297.Size = new Size(189, 28);
        lbl_R05_AT_0297.TabIndex = 17;
        lbl_R05_AT_0297.Text = "إجمالي الدائن";
        lbl_R05_AT_0297.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_R05_AT_0297
        // 
        field_R05_AT_0297.AccessibleName = "إجمالي الدائن";
        field_R05_AT_0297.Dock = DockStyle.Fill;
        field_R05_AT_0297.Location = new Point(3, 119);
        field_R05_AT_0297.Name = "field_R05_AT_0297";
        field_R05_AT_0297.ReadOnly = true;
        field_R05_AT_0297.Size = new Size(868, 22);
        field_R05_AT_0297.TabIndex = 17;
        field_R05_AT_0297.TabStop = false;
        field_R05_AT_0297.Tag = "R05-AT-0297";
        // 
        // lbl_R05_AT_0298
        // 
        lbl_R05_AT_0298.AutoSize = true;
        lbl_R05_AT_0298.Dock = DockStyle.Fill;
        lbl_R05_AT_0298.Location = new Point(877, 144);
        lbl_R05_AT_0298.Name = "lbl_R05_AT_0298";
        lbl_R05_AT_0298.Size = new Size(189, 28);
        lbl_R05_AT_0298.TabIndex = 18;
        lbl_R05_AT_0298.Text = "الفارق";
        lbl_R05_AT_0298.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_R05_AT_0298
        // 
        field_R05_AT_0298.AccessibleName = "الفارق";
        field_R05_AT_0298.Dock = DockStyle.Fill;
        field_R05_AT_0298.Location = new Point(3, 147);
        field_R05_AT_0298.Name = "field_R05_AT_0298";
        field_R05_AT_0298.ReadOnly = true;
        field_R05_AT_0298.Size = new Size(868, 22);
        field_R05_AT_0298.TabIndex = 18;
        field_R05_AT_0298.TabStop = false;
        field_R05_AT_0298.Tag = "R05-AT-0298";
        // 
        // documentDefaults
        // 
        documentDefaults.AutoScroll = true;
        documentDefaults.Controls.Add(documentDefaultsLayout);
        documentDefaults.Location = new Point(4, 23);
        documentDefaults.Name = "documentDefaults";
        documentDefaults.Size = new Size(1086, 182);
        documentDefaults.TabIndex = 5;
        documentDefaults.Text = "البيانات الافتراضية";
        // 
        // documentDefaultsLayout
        // 
        documentDefaultsLayout.AutoSize = true;
        documentDefaultsLayout.ColumnCount = 2;
        documentDefaultsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
        documentDefaultsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 280F));
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
        referenceHeader.Controls.Add(lbl_T04_EV_0267, 0, 0);
        referenceHeader.Controls.Add(field_T04_EV_0267, 1, 0);
        referenceHeader.Controls.Add(lbl_T04_EV_0268, 2, 0);
        referenceHeader.Controls.Add(field_T04_EV_0268, 3, 0);
        referenceHeader.Controls.Add(lbl_T04_EV_0269, 4, 0);
        referenceHeader.Controls.Add(field_T04_EV_0269, 5, 0);
        referenceHeader.Controls.Add(lbl_T04_EV_0270, 0, 1);
        referenceHeader.Controls.Add(field_T04_EV_0270, 1, 1);
        referenceHeader.Controls.Add(lbl_T04_EV_0271, 2, 1);
        referenceHeader.Controls.Add(field_T04_EV_0271, 3, 1);
        referenceHeader.Controls.Add(lbl_T04_EV_0272, 4, 1);
        referenceHeader.Controls.Add(field_T04_EV_0272, 5, 1);
        referenceHeader.Controls.Add(lbl_T04_EV_0273, 0, 2);
        referenceHeader.Controls.Add(field_T04_EV_0273, 1, 2);
        referenceHeader.Controls.Add(referenceLabel7, 2, 2);
        referenceHeader.Controls.Add(referenceField7, 3, 2);
        referenceHeader.Controls.Add(referenceLabel8, 4, 2);
        referenceHeader.Controls.Add(referenceField8, 5, 2);
        referenceHeader.Controls.Add(lbl_T04_EV_0274, 0, 3);
        referenceHeader.Controls.Add(field_T04_EV_0274, 1, 3);
        referenceHeader.Controls.Add(lbl_T04_EV_0278, 2, 3);
        referenceHeader.Controls.Add(field_T04_EV_0278, 3, 3);
        referenceHeader.Controls.Add(lbl_T04_EV_0277, 4, 3);
        referenceHeader.Controls.Add(field_T04_EV_0277, 5, 3);
        referenceHeader.Controls.Add(lbl_T04_EV_0279, 0, 4);
        referenceHeader.Controls.Add(field_T04_EV_0279, 1, 4);
        referenceHeader.Controls.Add(lbl_T04_EV_0280, 2, 4);
        referenceHeader.Controls.Add(field_T04_EV_0280, 3, 4);
        referenceHeader.Controls.Add(lbl_T04_EV_0281, 4, 4);
        referenceHeader.Controls.Add(field_T04_EV_0281, 5, 4);
        referenceHeader.Dock = DockStyle.Top;
        referenceHeader.Location = new Point(0, 172);
        referenceHeader.Name = "referenceHeader";
        referenceHeader.Padding = new Padding(8);
        referenceHeader.RightToLeft = RightToLeft.Yes;
        referenceHeader.RowCount = 5;
        referenceHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        referenceHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        referenceHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        referenceHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        referenceHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        referenceHeader.Size = new Size(1069, 176);
        referenceHeader.TabIndex = 1;
        // 
        // lbl_T04_EV_0267
        // 
        lbl_T04_EV_0267.Dock = DockStyle.Fill;
        lbl_T04_EV_0267.Location = new Point(948, 10);
        lbl_T04_EV_0267.Margin = new Padding(2);
        lbl_T04_EV_0267.Name = "lbl_T04_EV_0267";
        lbl_T04_EV_0267.Size = new Size(111, 28);
        lbl_T04_EV_0267.TabIndex = 0;
        lbl_T04_EV_0267.Text = "رقم الفرع";
        lbl_T04_EV_0267.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T04_EV_0267
        // 
        field_T04_EV_0267.AccessibleName = "رقم الفرع";
        field_T04_EV_0267.Dock = DockStyle.Fill;
        field_T04_EV_0267.DropDownStyle = ComboBoxStyle.DropDownList;
        field_T04_EV_0267.DropDownWidth = 420;
        field_T04_EV_0267.Location = new Point(712, 10);
        field_T04_EV_0267.Margin = new Padding(2);
        field_T04_EV_0267.Name = "field_T04_EV_0267";
        field_T04_EV_0267.Size = new Size(232, 22);
        field_T04_EV_0267.TabIndex = 0;
        field_T04_EV_0267.Tag = "T04-EV-0267";
        // 
        // lbl_T04_EV_0268
        // 
        lbl_T04_EV_0268.Dock = DockStyle.Fill;
        lbl_T04_EV_0268.Location = new Point(597, 10);
        lbl_T04_EV_0268.Margin = new Padding(2);
        lbl_T04_EV_0268.Name = "lbl_T04_EV_0268";
        lbl_T04_EV_0268.Size = new Size(111, 28);
        lbl_T04_EV_0268.TabIndex = 1;
        lbl_T04_EV_0268.Text = "نوع الوثيقة";
        lbl_T04_EV_0268.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T04_EV_0268
        // 
        field_T04_EV_0268.AccessibleName = "نوع الوثيقة";
        field_T04_EV_0268.Dock = DockStyle.Fill;
        field_T04_EV_0268.DropDownStyle = ComboBoxStyle.DropDownList;
        field_T04_EV_0268.DropDownWidth = 420;
        field_T04_EV_0268.Location = new Point(361, 10);
        field_T04_EV_0268.Margin = new Padding(2);
        field_T04_EV_0268.Name = "field_T04_EV_0268";
        field_T04_EV_0268.Size = new Size(232, 22);
        field_T04_EV_0268.TabIndex = 1;
        field_T04_EV_0268.Tag = "T04-EV-0268";
        // 
        // lbl_T04_EV_0269
        // 
        lbl_T04_EV_0269.Dock = DockStyle.Fill;
        lbl_T04_EV_0269.Location = new Point(246, 10);
        lbl_T04_EV_0269.Margin = new Padding(2);
        lbl_T04_EV_0269.Name = "lbl_T04_EV_0269";
        lbl_T04_EV_0269.Size = new Size(111, 28);
        lbl_T04_EV_0269.TabIndex = 2;
        lbl_T04_EV_0269.Text = "رقم المستند";
        lbl_T04_EV_0269.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T04_EV_0269
        // 
        field_T04_EV_0269.AccessibleName = "رقم المستند";
        field_T04_EV_0269.Dock = DockStyle.Fill;
        field_T04_EV_0269.Location = new Point(10, 10);
        field_T04_EV_0269.Margin = new Padding(2);
        field_T04_EV_0269.Name = "field_T04_EV_0269";
        field_T04_EV_0269.ReadOnly = true;
        field_T04_EV_0269.Size = new Size(232, 22);
        field_T04_EV_0269.TabIndex = 2;
        field_T04_EV_0269.TabStop = false;
        field_T04_EV_0269.Tag = "T04-EV-0269";
        // 
        // lbl_T04_EV_0270
        // 
        lbl_T04_EV_0270.Dock = DockStyle.Fill;
        lbl_T04_EV_0270.Location = new Point(948, 42);
        lbl_T04_EV_0270.Margin = new Padding(2);
        lbl_T04_EV_0270.Name = "lbl_T04_EV_0270";
        lbl_T04_EV_0270.Size = new Size(111, 28);
        lbl_T04_EV_0270.TabIndex = 3;
        lbl_T04_EV_0270.Text = "التاريخ";
        lbl_T04_EV_0270.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T04_EV_0270
        // 
        field_T04_EV_0270.AccessibleName = "التاريخ";
        field_T04_EV_0270.Checked = false;
        field_T04_EV_0270.CustomFormat = "yyyy-MM-dd";
        field_T04_EV_0270.Dock = DockStyle.Fill;
        field_T04_EV_0270.Format = DateTimePickerFormat.Custom;
        field_T04_EV_0270.Location = new Point(712, 42);
        field_T04_EV_0270.Margin = new Padding(2);
        field_T04_EV_0270.Name = "field_T04_EV_0270";
        field_T04_EV_0270.ShowCheckBox = true;
        field_T04_EV_0270.Size = new Size(232, 22);
        field_T04_EV_0270.TabIndex = 3;
        field_T04_EV_0270.Tag = "T04-EV-0270";
        // 
        // lbl_T04_EV_0271
        // 
        lbl_T04_EV_0271.Dock = DockStyle.Fill;
        lbl_T04_EV_0271.Location = new Point(597, 42);
        lbl_T04_EV_0271.Margin = new Padding(2);
        lbl_T04_EV_0271.Name = "lbl_T04_EV_0271";
        lbl_T04_EV_0271.Size = new Size(111, 28);
        lbl_T04_EV_0271.TabIndex = 4;
        lbl_T04_EV_0271.Text = "رقم المرجع";
        lbl_T04_EV_0271.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T04_EV_0271
        // 
        field_T04_EV_0271.AccessibleName = "رقم المرجع";
        field_T04_EV_0271.Dock = DockStyle.Fill;
        field_T04_EV_0271.Location = new Point(361, 42);
        field_T04_EV_0271.Margin = new Padding(2);
        field_T04_EV_0271.Name = "field_T04_EV_0271";
        field_T04_EV_0271.Size = new Size(232, 22);
        field_T04_EV_0271.TabIndex = 4;
        field_T04_EV_0271.Tag = "T04-EV-0271";
        // 
        // lbl_T04_EV_0272
        // 
        lbl_T04_EV_0272.Dock = DockStyle.Fill;
        lbl_T04_EV_0272.Location = new Point(246, 42);
        lbl_T04_EV_0272.Margin = new Padding(2);
        lbl_T04_EV_0272.Name = "lbl_T04_EV_0272";
        lbl_T04_EV_0272.Size = new Size(111, 28);
        lbl_T04_EV_0272.TabIndex = 5;
        lbl_T04_EV_0272.Text = "عدد المرفقات";
        lbl_T04_EV_0272.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T04_EV_0272
        // 
        field_T04_EV_0272.AccessibleName = "عدد المرفقات";
        field_T04_EV_0272.Dock = DockStyle.Fill;
        field_T04_EV_0272.Location = new Point(10, 42);
        field_T04_EV_0272.Margin = new Padding(2);
        field_T04_EV_0272.Name = "field_T04_EV_0272";
        field_T04_EV_0272.ReadOnly = true;
        field_T04_EV_0272.Size = new Size(232, 22);
        field_T04_EV_0272.TabIndex = 5;
        field_T04_EV_0272.TabStop = false;
        field_T04_EV_0272.Tag = "T04-EV-0272";
        // 
        // lbl_T04_EV_0273
        // 
        lbl_T04_EV_0273.Dock = DockStyle.Fill;
        lbl_T04_EV_0273.Location = new Point(948, 74);
        lbl_T04_EV_0273.Margin = new Padding(2);
        lbl_T04_EV_0273.Name = "lbl_T04_EV_0273";
        lbl_T04_EV_0273.Size = new Size(111, 28);
        lbl_T04_EV_0273.TabIndex = 6;
        lbl_T04_EV_0273.Text = "إجمالي المبلغ";
        lbl_T04_EV_0273.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T04_EV_0273
        // 
        field_T04_EV_0273.AccessibleName = "إجمالي المبلغ";
        field_T04_EV_0273.Dock = DockStyle.Fill;
        field_T04_EV_0273.Location = new Point(712, 74);
        field_T04_EV_0273.Margin = new Padding(2);
        field_T04_EV_0273.Name = "field_T04_EV_0273";
        field_T04_EV_0273.ReadOnly = true;
        field_T04_EV_0273.Size = new Size(232, 22);
        field_T04_EV_0273.TabIndex = 6;
        field_T04_EV_0273.TabStop = false;
        field_T04_EV_0273.Tag = "T04-EV-0273";
        // 
        // referenceLabel7
        // 
        referenceLabel7.Dock = DockStyle.Fill;
        referenceLabel7.Location = new Point(597, 74);
        referenceLabel7.Margin = new Padding(2);
        referenceLabel7.Name = "referenceLabel7";
        referenceLabel7.Size = new Size(111, 28);
        referenceLabel7.TabIndex = 7;
        referenceLabel7.Text = "المستفيد";
        referenceLabel7.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField7
        // 
        referenceField7.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField7.BackColor = Color.FromArgb(245, 245, 245);
        referenceField7.Dock = DockStyle.Fill;
        referenceField7.Location = new Point(361, 74);
        referenceField7.Margin = new Padding(2);
        referenceField7.Name = "referenceField7";
        referenceField7.ReadOnly = true;
        referenceField7.Size = new Size(232, 22);
        referenceField7.TabIndex = 8;
        referenceField7.TabStop = false;
        // 
        // referenceLabel8
        // 
        referenceLabel8.Dock = DockStyle.Fill;
        referenceLabel8.Location = new Point(246, 74);
        referenceLabel8.Margin = new Padding(2);
        referenceLabel8.Name = "referenceLabel8";
        referenceLabel8.Size = new Size(111, 28);
        referenceLabel8.TabIndex = 9;
        referenceLabel8.Text = "المستلم";
        referenceLabel8.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceField8
        // 
        referenceField8.AccessibleDescription = "حقل مرجعي؛ لم يربط بخدمة السند بعد";
        referenceField8.BackColor = Color.FromArgb(245, 245, 245);
        referenceField8.Dock = DockStyle.Fill;
        referenceField8.Location = new Point(10, 74);
        referenceField8.Margin = new Padding(2);
        referenceField8.Name = "referenceField8";
        referenceField8.ReadOnly = true;
        referenceField8.Size = new Size(232, 22);
        referenceField8.TabIndex = 10;
        referenceField8.TabStop = false;
        // 
        // lbl_T04_EV_0274
        // 
        lbl_T04_EV_0274.Dock = DockStyle.Fill;
        lbl_T04_EV_0274.Location = new Point(948, 106);
        lbl_T04_EV_0274.Margin = new Padding(2);
        lbl_T04_EV_0274.Name = "lbl_T04_EV_0274";
        lbl_T04_EV_0274.Size = new Size(111, 28);
        lbl_T04_EV_0274.TabIndex = 11;
        lbl_T04_EV_0274.Text = "البيان";
        lbl_T04_EV_0274.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T04_EV_0274
        // 
        field_T04_EV_0274.AccessibleName = "البيان";
        field_T04_EV_0274.Dock = DockStyle.Fill;
        field_T04_EV_0274.Location = new Point(712, 106);
        field_T04_EV_0274.Margin = new Padding(2);
        field_T04_EV_0274.Multiline = true;
        field_T04_EV_0274.Name = "field_T04_EV_0274";
        field_T04_EV_0274.ScrollBars = ScrollBars.Vertical;
        field_T04_EV_0274.Size = new Size(232, 28);
        field_T04_EV_0274.TabIndex = 7;
        field_T04_EV_0274.Tag = "T04-EV-0274";
        // 
        // lbl_T04_EV_0278
        // 
        lbl_T04_EV_0278.Dock = DockStyle.Fill;
        lbl_T04_EV_0278.Location = new Point(597, 106);
        lbl_T04_EV_0278.Margin = new Padding(2);
        lbl_T04_EV_0278.Name = "lbl_T04_EV_0278";
        lbl_T04_EV_0278.Size = new Size(111, 28);
        lbl_T04_EV_0278.TabIndex = 12;
        lbl_T04_EV_0278.Text = "رقم القيد الدوري";
        lbl_T04_EV_0278.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T04_EV_0278
        // 
        field_T04_EV_0278.AccessibleName = "رقم القيد الدوري";
        field_T04_EV_0278.Dock = DockStyle.Fill;
        field_T04_EV_0278.DropDownStyle = ComboBoxStyle.DropDownList;
        field_T04_EV_0278.DropDownWidth = 420;
        field_T04_EV_0278.Location = new Point(361, 106);
        field_T04_EV_0278.Margin = new Padding(2);
        field_T04_EV_0278.Name = "field_T04_EV_0278";
        field_T04_EV_0278.Size = new Size(232, 22);
        field_T04_EV_0278.TabIndex = 12;
        field_T04_EV_0278.Tag = "T04-EV-0278";
        // 
        // lbl_T04_EV_0277
        // 
        lbl_T04_EV_0277.Dock = DockStyle.Fill;
        lbl_T04_EV_0277.Location = new Point(246, 106);
        lbl_T04_EV_0277.Margin = new Padding(2);
        lbl_T04_EV_0277.Name = "lbl_T04_EV_0277";
        lbl_T04_EV_0277.Size = new Size(111, 28);
        lbl_T04_EV_0277.TabIndex = 13;
        lbl_T04_EV_0277.Text = "قيد دوري";
        lbl_T04_EV_0277.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T04_EV_0277
        // 
        field_T04_EV_0277.AccessibleName = "قيد دوري";
        field_T04_EV_0277.Checked = true;
        field_T04_EV_0277.CheckState = CheckState.Indeterminate;
        field_T04_EV_0277.Dock = DockStyle.Fill;
        field_T04_EV_0277.Location = new Point(10, 106);
        field_T04_EV_0277.Margin = new Padding(2);
        field_T04_EV_0277.Name = "field_T04_EV_0277";
        field_T04_EV_0277.Size = new Size(232, 28);
        field_T04_EV_0277.TabIndex = 8;
        field_T04_EV_0277.Tag = "T04-EV-0277";
        field_T04_EV_0277.ThreeState = true;
        // 
        // lbl_T04_EV_0279
        // 
        lbl_T04_EV_0279.Dock = DockStyle.Fill;
        lbl_T04_EV_0279.Location = new Point(948, 138);
        lbl_T04_EV_0279.Margin = new Padding(2);
        lbl_T04_EV_0279.Name = "lbl_T04_EV_0279";
        lbl_T04_EV_0279.Size = new Size(111, 28);
        lbl_T04_EV_0279.TabIndex = 14;
        lbl_T04_EV_0279.Text = "قيد فروق العملة";
        lbl_T04_EV_0279.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T04_EV_0279
        // 
        field_T04_EV_0279.AccessibleName = "قيد فروق العملة";
        field_T04_EV_0279.Checked = true;
        field_T04_EV_0279.CheckState = CheckState.Indeterminate;
        field_T04_EV_0279.Dock = DockStyle.Fill;
        field_T04_EV_0279.Location = new Point(712, 138);
        field_T04_EV_0279.Margin = new Padding(2);
        field_T04_EV_0279.Name = "field_T04_EV_0279";
        field_T04_EV_0279.Size = new Size(232, 28);
        field_T04_EV_0279.TabIndex = 9;
        field_T04_EV_0279.Tag = "T04-EV-0279";
        field_T04_EV_0279.ThreeState = true;
        // 
        // lbl_T04_EV_0280
        // 
        lbl_T04_EV_0280.Dock = DockStyle.Fill;
        lbl_T04_EV_0280.Location = new Point(597, 138);
        lbl_T04_EV_0280.Margin = new Padding(2);
        lbl_T04_EV_0280.Name = "lbl_T04_EV_0280";
        lbl_T04_EV_0280.Size = new Size(111, 28);
        lbl_T04_EV_0280.TabIndex = 15;
        lbl_T04_EV_0280.Text = "قيد معلق";
        lbl_T04_EV_0280.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T04_EV_0280
        // 
        field_T04_EV_0280.AccessibleName = "قيد معلق";
        field_T04_EV_0280.Dock = DockStyle.Fill;
        field_T04_EV_0280.Location = new Point(361, 138);
        field_T04_EV_0280.Margin = new Padding(2);
        field_T04_EV_0280.Name = "field_T04_EV_0280";
        field_T04_EV_0280.ReadOnly = true;
        field_T04_EV_0280.Size = new Size(232, 22);
        field_T04_EV_0280.TabIndex = 14;
        field_T04_EV_0280.TabStop = false;
        field_T04_EV_0280.Tag = "T04-EV-0280";
        // 
        // lbl_T04_EV_0281
        // 
        lbl_T04_EV_0281.Dock = DockStyle.Fill;
        lbl_T04_EV_0281.Location = new Point(246, 138);
        lbl_T04_EV_0281.Margin = new Padding(2);
        lbl_T04_EV_0281.Name = "lbl_T04_EV_0281";
        lbl_T04_EV_0281.Size = new Size(111, 28);
        lbl_T04_EV_0281.TabIndex = 16;
        lbl_T04_EV_0281.Text = "موقف";
        lbl_T04_EV_0281.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T04_EV_0281
        // 
        field_T04_EV_0281.AccessibleName = "موقف";
        field_T04_EV_0281.Dock = DockStyle.Fill;
        field_T04_EV_0281.Location = new Point(10, 138);
        field_T04_EV_0281.Margin = new Padding(2);
        field_T04_EV_0281.Name = "field_T04_EV_0281";
        field_T04_EV_0281.ReadOnly = true;
        field_T04_EV_0281.Size = new Size(232, 22);
        field_T04_EV_0281.TabIndex = 15;
        field_T04_EV_0281.TabStop = false;
        field_T04_EV_0281.Tag = "T04-EV-0281";
        // 
        // referenceAudit
        // 
        referenceAudit.BackColor = Color.FromArgb(225, 223, 247);
        referenceAudit.Dock = DockStyle.Fill;
        referenceAudit.Location = new Point(3, 646);
        referenceAudit.Name = "referenceAudit";
        referenceAudit.Size = new Size(1094, 48);
        referenceAudit.TabIndex = 4;
        referenceAudit.Text = "مدخل السجل: —    تاريخ الإدخال: —    الجهاز: —\r\nمعدل السجل: —    تاريخ التعديل: —    مرات التعديل: —    مرات الطباعة: —";
        referenceAudit.TextAlign = ContentAlignment.MiddleRight;
        // 
        // referenceTotals
        // 
        referenceTotals.BackColor = Color.FromArgb(250, 223, 249);
        referenceTotals.Dock = DockStyle.Fill;
        referenceTotals.Location = new Point(3, 614);
        referenceTotals.Name = "referenceTotals";
        referenceTotals.Size = new Size(1094, 32);
        referenceTotals.TabIndex = 3;
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
        // referenceCol2
        // 
        referenceCol2.HeaderText = "البيان";
        referenceCol2.MinimumWidth = 6;
        referenceCol2.Name = "referenceCol2";
        referenceCol2.ReadOnly = true;
        referenceCol2.ToolTipText = "للعرض؛ يحتاج ربطًا بخدمة السند";
        referenceCol2.Width = 140;
        // 
        // referencePrint
        // 
        referencePrint.Enabled = false;
        referencePrint.Location = new Point(407, 3);
        referencePrint.MinimumSize = new Size(132, 36);
        referencePrint.Name = "referencePrint";
        referencePrint.Size = new Size(132, 36);
        referencePrint.TabIndex = 6;
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
        mainLayout.Controls.Add(referenceAudit, 0, 5);
        mainLayout.Controls.Add(referenceTotals, 0, 4);
        mainLayout.Controls.Add(dgvLines, 0, 3);
        mainLayout.Controls.Add(tabs, 0, 2);
        mainLayout.Controls.Add(lblStatus, 0, 6);
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Location = new Point(0, 0);
        mainLayout.MinimumSize = new Size(980, 680);
        mainLayout.Name = "mainLayout";
        mainLayout.RowCount = 7;
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 215F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
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
        lblTitle.Text = "طلبات قيود اليومية";
        // 
        // flpActions
        // 
        flpActions.AutoSize = true;
        flpActions.BackColor = Color.FromArgb(239, 239, 244);

        flpActions.Controls.Add(btnApprove);
        flpActions.Controls.Add(btnClear);

        flpActions.Dock = DockStyle.Fill;
        flpActions.Location = new Point(3, 37);
        flpActions.Name = "flpActions";
        flpActions.RightToLeft = RightToLeft.Yes;
        flpActions.Size = new Size(1094, 42);
        flpActions.TabIndex = 1;
        // 
        // btnSave
        // 
        btnSave.AccessibleName = "حفظ";
        btnSave.Enabled = false;
        btnSave.Location = new Point(959, 3);
        btnSave.MinimumSize = new Size(132, 36);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(132, 36);
        btnSave.TabIndex = 0;
        btnSave.Text = "حفظ";
        // 
        // btnApprove
        // 
        btnApprove.AccessibleName = "اعتماد";
        btnApprove.Enabled = false;
        btnApprove.Location = new Point(821, 3);
        btnApprove.MinimumSize = new Size(132, 36);
        btnApprove.Name = "btnApprove";
        btnApprove.Size = new Size(132, 36);
        btnApprove.TabIndex = 1;
        btnApprove.Text = "اعتماد";
        // 
        // btnClear
        // 
        btnClear.AccessibleName = "تفريغ المسودة";
        btnClear.Location = new Point(683, 3);
        btnClear.MinimumSize = new Size(132, 36);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(132, 36);
        btnClear.TabIndex = 2;
        btnClear.Text = "تفريغ المسودة";
        // 
        // btnClose
        // 
        btnClose.AccessibleName = "إغلاق";
        btnClose.Location = new Point(545, 3);
        btnClose.MinimumSize = new Size(132, 36);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(132, 36);
        btnClose.TabIndex = 3;
        btnClose.Text = "إغلاق";
        // 
        // tabs
        // 
        tabs.Controls.Add(tp2);
        tabs.Controls.Add(tp3);
        tabs.Controls.Add(referenceExtra);
        tabs.Controls.Add(tp0);
        tabs.Controls.Add(documentAdditional);
        tabs.Controls.Add(documentDefaults);
        tabs.Controls.Add(tp1);
        tabs.Dock = DockStyle.Fill;
        tabs.Font = new Font("Tahoma", 9F);
        tabs.Location = new Point(3, 85);
        tabs.Multiline = true;
        tabs.Name = "tabs";
        tabs.RightToLeft = RightToLeft.Yes;
        tabs.RightToLeftLayout = true;
        tabs.SelectedIndex = 0;
        tabs.Size = new Size(1094, 209);
        tabs.TabIndex = 1;
        // 
        // tp3
        // 
        tp3.AutoScroll = true;
        tp3.Controls.Add(linesHost);
        tp3.Location = new Point(4, 23);
        tp3.Name = "tp3";
        tp3.Size = new Size(1086, 182);
        tp3.TabIndex = 3;
        tp3.Text = "البيانات التفصيلية";
        // 
        // layout3
        // 
        layout3.AutoScroll = true;
        layout3.AutoSize = true;
        layout3.ColumnCount = 2;
        layout3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 195F));
        layout3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout3.Dock = DockStyle.Top;
        layout3.Location = new Point(3, 3);
        layout3.MinimumSize = new Size(650, 0);
        layout3.Name = "layout3";
        layout3.Size = new Size(1080, 0);
        layout3.TabIndex = 0;
        // 
        // linesHost
        // 
        linesHost.ColumnCount = 1;
        linesHost.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        linesHost.Controls.Add(layout3, 0, 0);
        linesHost.Controls.Add(lineActions, 0, 1);
        linesHost.Dock = DockStyle.Fill;
        linesHost.Location = new Point(0, 0);
        linesHost.MinimumSize = new Size(680, 380);
        linesHost.Name = "linesHost";
        linesHost.RowCount = 3;
        linesHost.RowStyles.Add(new RowStyle());
        linesHost.RowStyles.Add(new RowStyle());
        linesHost.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        linesHost.Size = new Size(1086, 380);
        linesHost.TabIndex = 1;
        // 
        // lineActions
        // 
        lineActions.AutoSize = true;
        lineActions.Controls.Add(btnAddRow);
        lineActions.Controls.Add(btnRemoveRow);
        lineActions.Dock = DockStyle.Fill;
        lineActions.Location = new Point(3, 9);
        lineActions.Name = "lineActions";
        lineActions.Size = new Size(1080, 42);
        lineActions.TabIndex = 2;
        // 
        // btnAddRow
        // 
        btnAddRow.Location = new Point(945, 3);
        btnAddRow.MinimumSize = new Size(132, 36);
        btnAddRow.Name = "btnAddRow";
        btnAddRow.Size = new Size(132, 36);
        btnAddRow.TabIndex = 4;
        btnAddRow.Text = "إضافة سطر";
        // 
        // btnRemoveRow
        // 
        btnRemoveRow.Location = new Point(807, 3);
        btnRemoveRow.MinimumSize = new Size(132, 36);
        btnRemoveRow.Name = "btnRemoveRow";
        btnRemoveRow.Size = new Size(132, 36);
        btnRemoveRow.TabIndex = 5;
        btnRemoveRow.Text = "حذف سطر";
        // 
        // tp0
        // 
        tp0.AutoScroll = true;
        tp0.Controls.Add(referenceHeader);
        tp0.Controls.Add(layout0);
        tp0.Location = new Point(4, 23);
        tp0.Name = "tp0";
        tp0.Size = new Size(1086, 182);
        tp0.TabIndex = 0;
        tp0.Text = "البيانات الرئيسية";
        // 
        // tp1
        // 
        tp1.AutoScroll = true;
        tp1.Controls.Add(layout1);
        tp1.Location = new Point(4, 23);
        tp1.Name = "tp1";
        tp1.Size = new Size(1086, 182);
        tp1.TabIndex = 1;
        tp1.Text = "استيراد من ملف";
        // 
        // layout1
        // 
        layout1.AutoScroll = true;
        layout1.AutoSize = true;
        layout1.ColumnCount = 2;
        layout1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 195F));
        layout1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout1.Controls.Add(txtCsvPath, 1, 0);
        layout1.Controls.Add(csvActions, 0, 0);
        layout1.Controls.Add(txtCsvSchema, 0, 1);
        layout1.Controls.Add(dgvImport, 0, 2);
        layout1.Dock = DockStyle.Fill;
        layout1.Location = new Point(0, 0);
        layout1.MinimumSize = new Size(650, 0);
        layout1.Name = "layout1";
        layout1.RowCount = 3;
        layout1.RowStyles.Add(new RowStyle());
        layout1.RowStyles.Add(new RowStyle());
        layout1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout1.Size = new Size(1086, 182);
        layout1.TabIndex = 0;
        // 
        // txtCsvPath
        // 
        txtCsvPath.AccessibleName = "مسار ملف CSV";
        txtCsvPath.Dock = DockStyle.Fill;
        txtCsvPath.Location = new Point(3, 3);
        txtCsvPath.Name = "txtCsvPath";
        txtCsvPath.ReadOnly = true;
        txtCsvPath.Size = new Size(885, 22);
        txtCsvPath.TabIndex = 0;
        // 
        // csvActions
        // 
        csvActions.AutoSize = true;
        csvActions.Controls.Add(btnChooseCsv);
        csvActions.Controls.Add(btnImportCsv);
        csvActions.Dock = DockStyle.Fill;
        csvActions.Location = new Point(894, 3);
        csvActions.Name = "csvActions";
        csvActions.Size = new Size(189, 72);
        csvActions.TabIndex = 1;
        // 
        // btnChooseCsv
        // 
        btnChooseCsv.AutoSize = true;
        btnChooseCsv.Location = new Point(52, 3);
        btnChooseCsv.Name = "btnChooseCsv";
        btnChooseCsv.Size = new Size(134, 30);
        btnChooseCsv.TabIndex = 0;
        btnChooseCsv.Text = "اختيار ومعاينة CSV";
        // 
        // btnImportCsv
        // 
        btnImportCsv.AutoSize = true;
        btnImportCsv.Location = new Point(43, 39);
        btnImportCsv.Name = "btnImportCsv";
        btnImportCsv.Size = new Size(143, 30);
        btnImportCsv.TabIndex = 1;
        btnImportCsv.Text = "إضافة إلى المسودة";
        // 
        // txtCsvSchema
        // 
        layout1.SetColumnSpan(txtCsvSchema, 2);
        txtCsvSchema.Dock = DockStyle.Fill;
        txtCsvSchema.Location = new Point(3, 81);
        txtCsvSchema.Multiline = true;
        txtCsvSchema.Name = "txtCsvSchema";
        txtCsvSchema.ReadOnly = true;
        txtCsvSchema.Size = new Size(1080, 75);
        txtCsvSchema.TabIndex = 2;
        txtCsvSchema.Text = "قالب CSV محلي: UTF-8، فاصل فاصلة، الرموز من القوائم المرتبطة. عناوين الأعمدة بالترتيب:\r\nT04_EV_0287,T04_EV_0470,T04_EV_0471,T04_EV_0472,T04_EV_0473,T04_EV_0474,T04_EV_0475,R05_AT_0294";
        // 
        // dgvImport
        // 
        dgvImport.AllowUserToAddRows = false;
        dgvImport.AllowUserToDeleteRows = false;
        dgvImport.ColumnHeadersHeight = 29;
        layout1.SetColumnSpan(dgvImport, 2);
        dgvImport.Dock = DockStyle.Fill;
        dgvImport.Location = new Point(3, 162);
        dgvImport.MinimumSize = new Size(650, 200);
        dgvImport.Name = "dgvImport";
        dgvImport.ReadOnly = true;
        dgvImport.RowHeadersWidth = 51;
        dgvImport.Size = new Size(1080, 200);
        dgvImport.TabIndex = 3;
        // 
        // dgvLines
        // 
        dgvLines.AccessibleName = "البيانات التفصيلية";
        dgvLines.AllowUserToAddRows = false;
        dgvLines.AllowUserToDeleteRows = false;
        dataGridViewCellStyle1.BackColor = Color.White;
        dgvLines.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvLines.BackgroundColor = Color.White;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = Color.FromArgb(232, 232, 232);
        dataGridViewCellStyle2.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        dgvLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvLines.ColumnHeadersHeight = 34;
        dgvLines.Columns.AddRange(new DataGridViewColumn[] { col_rowNo, col_T04_EV_0287, col_T04_EV_0470, col_T04_EV_0471, col_T04_EV_0472, col_T04_EV_0473, col_T04_EV_0474, col_T04_EV_0475, col_R05_AT_0294, col_R05_AT_0295, referenceCol0, referenceCol2 });
        dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle9.BackColor = Color.FromArgb(255, 255, 226);
        dataGridViewCellStyle9.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle9.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle9.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle9.WrapMode = DataGridViewTriState.False;
        dgvLines.DefaultCellStyle = dataGridViewCellStyle9;
        dgvLines.Dock = DockStyle.Fill;
        dgvLines.EnableHeadersVisualStyles = false;
        dgvLines.Location = new Point(3, 300);
        dgvLines.MinimumSize = new Size(900, 80);
        dgvLines.MultiSelect = false;
        dgvLines.Name = "dgvLines";
        dgvLines.RightToLeft = RightToLeft.Yes;
        dgvLines.RowHeadersVisible = false;
        dgvLines.RowHeadersWidth = 51;
        dgvLines.RowTemplate.Height = 27;
        dgvLines.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvLines.Size = new Size(1094, 311);
        dgvLines.TabIndex = 1;
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
        // col_T04_EV_0287
        // 
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
        col_T04_EV_0287.DefaultCellStyle = dataGridViewCellStyle3;
        col_T04_EV_0287.HeaderText = "النسبة";
        col_T04_EV_0287.MinimumWidth = 90;
        col_T04_EV_0287.Name = "col_T04_EV_0287";
        col_T04_EV_0287.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_T04_EV_0287.Width = 140;
        // 
        // col_T04_EV_0470
        // 
        col_T04_EV_0470.HeaderText = "العملة";
        col_T04_EV_0470.MinimumWidth = 90;
        col_T04_EV_0470.Name = "col_T04_EV_0470";
        col_T04_EV_0470.Width = 140;
        // 
        // col_T04_EV_0471
        // 
        dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
        col_T04_EV_0471.DefaultCellStyle = dataGridViewCellStyle4;
        col_T04_EV_0471.HeaderText = "سعر التحويل";
        col_T04_EV_0471.MinimumWidth = 90;
        col_T04_EV_0471.Name = "col_T04_EV_0471";
        col_T04_EV_0471.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_T04_EV_0471.Width = 140;
        // 
        // col_T04_EV_0472
        // 
        dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
        col_T04_EV_0472.DefaultCellStyle = dataGridViewCellStyle5;
        col_T04_EV_0472.HeaderText = "مدين محلي";
        col_T04_EV_0472.MinimumWidth = 90;
        col_T04_EV_0472.Name = "col_T04_EV_0472";
        col_T04_EV_0472.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_T04_EV_0472.Width = 140;
        // 
        // col_T04_EV_0473
        // 
        dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
        col_T04_EV_0473.DefaultCellStyle = dataGridViewCellStyle6;
        col_T04_EV_0473.HeaderText = "دائن محلي";
        col_T04_EV_0473.MinimumWidth = 90;
        col_T04_EV_0473.Name = "col_T04_EV_0473";
        col_T04_EV_0473.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_T04_EV_0473.Width = 140;
        // 
        // col_T04_EV_0474
        // 
        dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
        col_T04_EV_0474.DefaultCellStyle = dataGridViewCellStyle7;
        col_T04_EV_0474.HeaderText = "مدين أجنبي";
        col_T04_EV_0474.MinimumWidth = 90;
        col_T04_EV_0474.Name = "col_T04_EV_0474";
        col_T04_EV_0474.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_T04_EV_0474.Width = 140;
        // 
        // col_T04_EV_0475
        // 
        dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight;
        col_T04_EV_0475.DefaultCellStyle = dataGridViewCellStyle8;
        col_T04_EV_0475.HeaderText = "دائن أجنبي";
        col_T04_EV_0475.MinimumWidth = 90;
        col_T04_EV_0475.Name = "col_T04_EV_0475";
        col_T04_EV_0475.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_T04_EV_0475.Width = 140;
        // 
        // col_R05_AT_0294
        // 
        col_R05_AT_0294.HeaderText = "رقم الحساب";
        col_R05_AT_0294.MinimumWidth = 90;
        col_R05_AT_0294.Name = "col_R05_AT_0294";
        col_R05_AT_0294.Width = 140;
        // 
        // col_R05_AT_0295
        // 
        col_R05_AT_0295.HeaderText = "اسم الحساب";
        col_R05_AT_0295.MinimumWidth = 90;
        col_R05_AT_0295.Name = "col_R05_AT_0295";
        col_R05_AT_0295.ReadOnly = true;
        col_R05_AT_0295.SortMode = DataGridViewColumnSortMode.NotSortable;
        col_R05_AT_0295.Width = 140;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.Location = new Point(3, 694);
        lblStatus.Name = "lblStatus";
        lblStatus.Padding = new Padding(6);
        lblStatus.Size = new Size(1094, 26);
        lblStatus.TabIndex = 2;
        lblStatus.Text = "مسودة واجهة غير محفوظة — الخدمات غير موصولة";
        // 
        // UcScreen_04_08_05
        // 
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScroll = true;
        BackColor = Color.FromArgb(240, 240, 240);
        rootWorkspaceViewport = new System.Windows.Forms.Panel();
        rootWorkspaceViewport.Name = "rootWorkspaceViewport";
        rootWorkspaceViewport.Dock = DockStyle.Fill;
        rootWorkspaceViewport.AutoScroll = true;
        rootWorkspaceViewport.AutoScrollMinSize = new Size(1225, 850);
        rootWorkspaceViewport.Margin = Padding.Empty;
        rootWorkspaceViewport.Controls.Add(mainLayout);
        Controls.Add(rootWorkspaceViewport);
        Font = new Font("Tahoma", 9F);
        Name = "UcScreen_04_08_05";
        Size = new Size(1100, 720);
        Tag = "04.08.05";
        documentAdditional.ResumeLayout(false);
        tp2.ResumeLayout(false);
        tp2.PerformLayout();
        layout2.ResumeLayout(false);
        layout2.PerformLayout();
        layout0.ResumeLayout(false);
        layout0.PerformLayout();
        documentDefaults.ResumeLayout(false);
        documentDefaults.PerformLayout();
        documentDefaultsLayout.ResumeLayout(false);
        documentDefaultsLayout.PerformLayout();
        referenceHeader.ResumeLayout(false);
        referenceHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)validationErrors).EndInit();
        mainLayout.ResumeLayout(false);
        mainLayout.PerformLayout();
        flpActions.ResumeLayout(false);
        tabs.ResumeLayout(false);
        tp3.ResumeLayout(false);
        linesHost.ResumeLayout(false);
        linesHost.PerformLayout();
        lineActions.ResumeLayout(false);
        tp0.ResumeLayout(false);
        tp0.PerformLayout();
        tp1.ResumeLayout(false);
        tp1.PerformLayout();
        layout1.ResumeLayout(false);
        layout1.PerformLayout();
        csvActions.ResumeLayout(false);
        csvActions.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvImport).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
        ResumeLayout(false);
    
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Fill;
        designerCommandBar.Location = new Point(3, 37);
        designerCommandBar.Size = new Size(1094, 42);
        designerCommandBar.TabIndex = 1;
        designerCommandBar.RightToLeft = RightToLeft.Yes;
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
        standardCommandEdit.AutoSize = false;
        standardCommandEdit.Dock = DockStyle.None;
        standardCommandEdit.MinimumSize = Size.Empty;
        standardCommandEdit.Size = new Size(26, 24);
        standardCommandEdit.Margin = new Padding(1);
        standardCommandEdit.FlatStyle = FlatStyle.Flat;
        standardCommandEdit.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandEdit.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Edit;
        standardCommandEdit.Text = "";
        flpActions.Controls.Add(standardCommandEdit);
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
        standardCommandView.AutoSize = false;
        standardCommandView.Dock = DockStyle.None;
        standardCommandView.MinimumSize = Size.Empty;
        standardCommandView.Size = new Size(26, 24);
        standardCommandView.Margin = new Padding(1);
        standardCommandView.FlatStyle = FlatStyle.Flat;
        standardCommandView.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandView.Image = TransportERP.Desktop.CoreUI.CommandBarImages.View;
        standardCommandView.Text = "";
        flpActions.Controls.Add(standardCommandView);
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
        btnSave.AutoSize = false;
        btnSave.Dock = DockStyle.None;
        btnSave.MinimumSize = Size.Empty;
        btnSave.Size = new Size(26, 24);
        btnSave.Margin = new Padding(1);
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnSave.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Save;
        btnSave.Text = "";
        flpActions.Controls.Add(btnSave);
        designerCommandBar.SetCommandRole(btnSave, TransportERP.Desktop.CoreUI.DesignerCommandRole.Save);
        referencePrint.AutoSize = false;
        referencePrint.Dock = DockStyle.None;
        referencePrint.MinimumSize = Size.Empty;
        referencePrint.Size = new Size(26, 24);
        referencePrint.Margin = new Padding(1);
        referencePrint.FlatStyle = FlatStyle.Flat;
        referencePrint.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        referencePrint.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Print;
        referencePrint.Text = "";
        flpActions.Controls.Add(referencePrint);
        designerCommandBar.SetCommandRole(referencePrint, TransportERP.Desktop.CoreUI.DesignerCommandRole.Print);
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
        referenceAudit.Visible = false;
        Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
