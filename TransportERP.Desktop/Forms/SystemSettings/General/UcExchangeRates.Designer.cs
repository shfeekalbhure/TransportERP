using System.Drawing;
using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

partial class UcExchangeRates
{
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;

    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
    private FlowLayoutPanel designerHiddenCommands = null!;
    private Button standardCommandDelete = null!;
    private Button standardCommandCancel = null!;
    private Button standardCommandView = null!;
    private Button standardCommandPrint = null!;
    private Button standardCommandImport = null!;
    private Button standardCommandExport = null!;
    private Button standardCommandHelp = null!;

    private System.ComponentModel.IContainer? components;
    private TableLayoutPanel mainLayout = null!;
    private Panel pnlHeader = null!;
    private FlowLayoutPanel pnlActions = null!;
    private Label lblTitle = null!;
    private Button btnNew = null!;
    private Button btnSave = null!;
    private Button btnEdit = null!;
    private Button btnDisable = null!;
    private Button btnRefresh = null!;
    private Button btnClose = null!;
    private TableLayoutPanel contentLayout = null!;
    private GroupBox grpMainData = null!;
    private TableLayoutPanel RatesFields = null!;
    private ComboBox cmbCompany = null!;
    private ComboBox cmbFromCurrency = null!;
    private ComboBox cmbToCurrency = null!;
    private ComboBox cmbRateType = null!;
    private DateTimePicker dtpRateDate = null!;
    private DateTimePicker dtpEffectiveTo = null!;
    private NumericUpDown nudExchangeRate = null!;
    private TextBox txtStatus = null!;
    private CheckBox chkMinimumRate = null!;
    private NumericUpDown nudMinimumRate = null!;
    private CheckBox chkMaximumRate = null!;
    private NumericUpDown nudMaximumRate = null!;
    private TextBox txtDisableReason = null!;
    private Label lblIdentityNote = null!;
    private Label lblCompany = null!;
    private Label lblFrom = null!;
    private Label lblTo = null!;
    private Label lblType = null!;
    private Label lblFromDate = null!;
    private Label lblToDate = null!;
    private Label lblRate = null!;
    private Label lblStatus = null!;
    private Label lblReason = null!;
    private GroupBox grpSearch = null!;
    private TableLayoutPanel SearchFields = null!;
    private ComboBox cmbFilterCompany = null!;
    private ComboBox cmbFilterFromCurrency = null!;
    private ComboBox cmbFilterToCurrency = null!;
    private ComboBox cmbFilterRateType = null!;
    private ComboBox cmbFilterStatus = null!;
    private DateTimePicker dtpFilterEffectiveAt = null!;
    private TextBox txtSearch = null!;
    private Button btnSearch = null!;
    private Label lblFilterCompany = null!;
    private Label lblFilterFrom = null!;
    private Label lblFilterTo = null!;
    private Label lblFilterType = null!;
    private Label lblFilterAt = null!;
    private Label lblFilterStatus = null!;
    private Label lblSearch = null!;
    private Label lblSearchAction = null!;
    private DataGridView dgvRates = null!;
    private DataGridViewTextBoxColumn dgvRatesColFrom = null!;
    private DataGridViewTextBoxColumn dgvRatesColTo = null!;
    private DataGridViewTextBoxColumn dgvRatesColType = null!;
    private DataGridViewTextBoxColumn dgvRatesColDate = null!;
    private DataGridViewTextBoxColumn dgvRatesColEffectiveTo = null!;
    private DataGridViewTextBoxColumn dgvRatesColValue = null!;
    private DataGridViewTextBoxColumn dgvRatesColMinimum = null!;
    private DataGridViewTextBoxColumn dgvRatesColMaximum = null!;
    private DataGridViewTextBoxColumn dgvRatesColStatus = null!;
    private Label lblRateConvention = null!;
    private FlowLayoutPanel pnlPagination = null!;
    private Button btnFirst = null!;
    private Button btnPrevious = null!;
    private Button btnNext = null!;
    private Button btnLast = null!;
    private TextBox txtCurrentRecordNo = null!;
    private Label lblPreview = null!;
    private TableLayoutPanel tlpAuditInfo = null!;
    private Label lblCreatedBy = null!;
    private Label lblCreatedAt = null!;
    private Label lblModifiedBy = null!;
    private Label lblModifiedAt = null!;
    private Label lblEditCount = null!;
    private Label lblLastPrintedAt = null!;
    private Label lblPrintCount = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        designerHiddenCommands = new FlowLayoutPanel();
        standardCommandDelete = new Button();
        standardCommandCancel = new Button();
        standardCommandView = new Button();
        standardCommandPrint = new Button();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
        mainLayout = new TableLayoutPanel();
        pnlHeader = new Panel();
        pnlActions = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        lblTitle = new Label();
        contentLayout = new TableLayoutPanel();
        grpMainData = new GroupBox();
        RatesFields = new TableLayoutPanel();
        lblCompany = new Label();
        cmbCompany = new ComboBox();
        lblFrom = new Label();
        cmbFromCurrency = new ComboBox();
        lblTo = new Label();
        cmbToCurrency = new ComboBox();
        lblType = new Label();
        cmbRateType = new ComboBox();
        lblFromDate = new Label();
        dtpRateDate = new DateTimePicker();
        lblToDate = new Label();
        dtpEffectiveTo = new DateTimePicker();
        lblRate = new Label();
        nudExchangeRate = new NumericUpDown();
        lblStatus = new Label();
        txtStatus = new TextBox();
        chkMinimumRate = new CheckBox();
        nudMinimumRate = new NumericUpDown();
        chkMaximumRate = new CheckBox();
        nudMaximumRate = new NumericUpDown();
        lblReason = new Label();
        txtDisableReason = new TextBox();
        lblIdentityNote = new Label();
        grpSearch = new GroupBox();
        SearchFields = new TableLayoutPanel();
        lblFilterCompany = new Label();
        cmbFilterCompany = new ComboBox();
        lblFilterFrom = new Label();
        cmbFilterFromCurrency = new ComboBox();
        lblFilterTo = new Label();
        cmbFilterToCurrency = new ComboBox();
        lblFilterType = new Label();
        cmbFilterRateType = new ComboBox();
        lblFilterAt = new Label();
        dtpFilterEffectiveAt = new DateTimePicker();
        lblFilterStatus = new Label();
        cmbFilterStatus = new ComboBox();
        lblSearch = new Label();
        txtSearch = new TextBox();
        lblSearchAction = new Label();
        btnSearch = new Button();
        dgvRates = new DataGridView();
        dgvRatesColFrom = new DataGridViewTextBoxColumn();
        dgvRatesColTo = new DataGridViewTextBoxColumn();
        dgvRatesColType = new DataGridViewTextBoxColumn();
        dgvRatesColDate = new DataGridViewTextBoxColumn();
        dgvRatesColEffectiveTo = new DataGridViewTextBoxColumn();
        dgvRatesColValue = new DataGridViewTextBoxColumn();
        dgvRatesColMinimum = new DataGridViewTextBoxColumn();
        dgvRatesColMaximum = new DataGridViewTextBoxColumn();
        dgvRatesColStatus = new DataGridViewTextBoxColumn();
        lblRateConvention = new Label();
        pnlPagination = new FlowLayoutPanel();
        lblPreview = new Label();
        tlpAuditInfo = new TableLayoutPanel();
        lblCreatedBy = new Label();
        lblCreatedAt = new Label();
        lblModifiedBy = new Label();
        lblModifiedAt = new Label();
        lblEditCount = new Label();
        lblLastPrintedAt = new Label();
        lblPrintCount = new Label();
        btnFirst = new Button();
        btnPrevious = new Button();
        txtCurrentRecordNo = new TextBox();
        btnNext = new Button();
        btnLast = new Button();
        btnDisable = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        mainLayout.SuspendLayout();
        pnlHeader.SuspendLayout();
        pnlActions.SuspendLayout();
        contentLayout.SuspendLayout();
        grpMainData.SuspendLayout();
        RatesFields.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudExchangeRate).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudMinimumRate).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudMaximumRate).BeginInit();
        grpSearch.SuspendLayout();
        SearchFields.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvRates).BeginInit();
        tlpAuditInfo.SuspendLayout();
        SuspendLayout();
        // 
        // mainLayout
        // 
        mainLayout.ColumnCount = 1;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        mainLayout.Controls.Add(pnlHeader, 0, 0);
        mainLayout.Controls.Add(contentLayout, 0, 1);
        mainLayout.Controls.Add(pnlPagination, 0, 2);
        mainLayout.Controls.Add(lblPreview, 0, 3);
        mainLayout.Controls.Add(tlpAuditInfo, 0, 4);
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Location = new Point(0, 0);
        mainLayout.Margin = new Padding(0);
        mainLayout.Name = "mainLayout";
        mainLayout.RowCount = 5;
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        mainLayout.Size = new Size(1200, 900);
        mainLayout.TabIndex = 0;
        mainLayout.SizeChanged += ContentLayout_SizeChanged;
        // 
        // pnlHeader
        // 
        pnlHeader.AutoSize = true;
        pnlHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlHeader.BackColor = Color.FromArgb(224, 224, 224);
        pnlHeader.Controls.Add(designerCommandBar);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Fill;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Margin = new Padding(0);
        pnlHeader.MinimumSize = new Size(0, 48);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1200, 48);
        pnlHeader.TabIndex = 0;
        // 
        // pnlActions
        // 
        pnlActions.AutoSize = true;
        pnlActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlActions.Controls.Add(btnNew);
        pnlActions.Controls.Add(btnSave);
        pnlActions.Controls.Add(btnEdit);
        pnlActions.Controls.Add(btnFirst);
        pnlActions.Controls.Add(btnPrevious);
        pnlActions.Controls.Add(txtCurrentRecordNo);
        pnlActions.Controls.Add(btnNext);
        pnlActions.Controls.Add(btnLast);
        pnlActions.Controls.Add(btnDisable);
        pnlActions.Controls.Add(btnRefresh);
        pnlActions.Controls.Add(btnClose);
        pnlActions.Dock = DockStyle.Top;
        pnlActions.Location = new Point(0, 0);
        pnlActions.Name = "pnlActions";
        pnlActions.RightToLeft = RightToLeft.Yes;
        pnlActions.Size = new Size(1012, 40);
        pnlActions.TabIndex = 0;
        // 
        // btnNew
        // 
        btnNew.AutoSize = true;
        btnNew.Enabled = false;
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Font = new Font("Tahoma", 9F);
        btnNew.Location = new Point(933, 4);
        btnNew.Margin = new Padding(4);
        btnNew.MinimumSize = new Size(70, 30);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(75, 32);
        btnNew.TabIndex = 0;
        btnNew.Text = "جديد";
        // 
        // btnSave
        // 
        btnSave.AutoSize = true;
        btnSave.Enabled = false;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 9F);
        btnSave.Location = new Point(850, 4);
        btnSave.Margin = new Padding(4);
        btnSave.MinimumSize = new Size(70, 30);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 32);
        btnSave.TabIndex = 1;
        btnSave.Text = "حفظ";
        // 
        // btnEdit
        // 
        btnEdit.AutoSize = true;
        btnEdit.Enabled = false;
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.Font = new Font("Tahoma", 9F);
        btnEdit.Location = new Point(767, 4);
        btnEdit.Margin = new Padding(4);
        btnEdit.MinimumSize = new Size(70, 30);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(75, 32);
        btnEdit.TabIndex = 2;
        btnEdit.Text = "تعديل";
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.BackColor = Color.FromArgb(192, 192, 255);
        lblTitle.Dock = DockStyle.Right;
        lblTitle.Font = new Font("Tahoma", 9F);
        lblTitle.Location = new Point(1012, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Padding = new Padding(8, 0, 15, 0);
        lblTitle.Size = new Size(188, 37);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "أسعار الصرف";
        lblTitle.TextAlign = ContentAlignment.MiddleRight;
        // 
        // contentLayout
        // 
        contentLayout.AutoScroll = true;
        contentLayout.ColumnCount = 1;
        contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        contentLayout.Controls.Add(grpMainData, 0, 0);
        contentLayout.Controls.Add(grpSearch, 0, 1);
        contentLayout.Controls.Add(dgvRates, 0, 2);
        contentLayout.Controls.Add(lblRateConvention, 0, 3);
        contentLayout.Dock = DockStyle.Fill;
        contentLayout.Location = new Point(0, 48);
        contentLayout.Margin = new Padding(0);
        contentLayout.Name = "contentLayout";
        contentLayout.RowCount = 4;
        contentLayout.RowStyles.Add(new RowStyle());
        contentLayout.RowStyles.Add(new RowStyle());
        contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        contentLayout.RowStyles.Add(new RowStyle());
        contentLayout.Size = new Size(1200, 775);
        contentLayout.TabIndex = 1;
        contentLayout.SizeChanged += ContentLayout_SizeChanged;
        // 
        // grpMainData
        // 
        grpMainData.AutoSize = true;
        grpMainData.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        grpMainData.Controls.Add(RatesFields);
        grpMainData.Dock = DockStyle.Top;
        grpMainData.Location = new Point(3, 3);
        grpMainData.Name = "grpMainData";
        grpMainData.Size = new Size(1194, 257);
        grpMainData.TabIndex = 0;
        grpMainData.TabStop = false;
        grpMainData.Text = "بيانات سعر الصرف";
        // 
        // RatesFields
        // 
        RatesFields.AutoSize = true;
        RatesFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        RatesFields.ColumnCount = 4;
        RatesFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        RatesFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        RatesFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        RatesFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        RatesFields.Controls.Add(lblCompany, 0, 0);
        RatesFields.Controls.Add(cmbCompany, 1, 0);
        RatesFields.Controls.Add(lblFrom, 2, 0);
        RatesFields.Controls.Add(cmbFromCurrency, 3, 0);
        RatesFields.Controls.Add(lblTo, 0, 1);
        RatesFields.Controls.Add(cmbToCurrency, 1, 1);
        RatesFields.Controls.Add(lblType, 2, 1);
        RatesFields.Controls.Add(cmbRateType, 3, 1);
        RatesFields.Controls.Add(lblFromDate, 0, 2);
        RatesFields.Controls.Add(dtpRateDate, 1, 2);
        RatesFields.Controls.Add(lblToDate, 2, 2);
        RatesFields.Controls.Add(dtpEffectiveTo, 3, 2);
        RatesFields.Controls.Add(lblRate, 0, 3);
        RatesFields.Controls.Add(nudExchangeRate, 1, 3);
        RatesFields.Controls.Add(lblStatus, 2, 3);
        RatesFields.Controls.Add(txtStatus, 3, 3);
        RatesFields.Controls.Add(chkMinimumRate, 0, 4);
        RatesFields.Controls.Add(nudMinimumRate, 1, 4);
        RatesFields.Controls.Add(chkMaximumRate, 2, 4);
        RatesFields.Controls.Add(nudMaximumRate, 3, 4);
        RatesFields.Controls.Add(lblReason, 0, 5);
        RatesFields.Controls.Add(txtDisableReason, 1, 5);
        RatesFields.Controls.Add(lblIdentityNote, 2, 5);
        RatesFields.Dock = DockStyle.Top;
        RatesFields.Location = new Point(3, 26);
        RatesFields.Margin = new Padding(0);
        RatesFields.Name = "RatesFields";
        RatesFields.RowCount = 6;
        RatesFields.RowStyles.Add(new RowStyle());
        RatesFields.RowStyles.Add(new RowStyle());
        RatesFields.RowStyles.Add(new RowStyle());
        RatesFields.RowStyles.Add(new RowStyle());
        RatesFields.RowStyles.Add(new RowStyle());
        RatesFields.RowStyles.Add(new RowStyle());
        RatesFields.Size = new Size(1188, 228);
        RatesFields.TabIndex = 0;
        // 
        // lblCompany
        // 
        lblCompany.AutoSize = true;
        lblCompany.Dock = DockStyle.Fill;
        lblCompany.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblCompany.ForeColor = Color.FromArgb(180, 35, 24);
        lblCompany.Location = new Point(1031, 0);
        lblCompany.Name = "lblCompany";
        lblCompany.Size = new Size(154, 34);
        lblCompany.TabIndex = 0;
        lblCompany.Text = "الشركة *";
        lblCompany.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cmbCompany
        // 
        cmbCompany.BackColor = Color.FromArgb(255, 249, 219);
        cmbCompany.Dock = DockStyle.Fill;
        cmbCompany.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCompany.Font = new Font("Tahoma", 9F);
        cmbCompany.Location = new Point(597, 3);
        cmbCompany.Name = "cmbCompany";
        cmbCompany.Size = new Size(428, 33);
        cmbCompany.TabIndex = 0;
        // 
        // lblFrom
        // 
        lblFrom.AutoSize = true;
        lblFrom.Dock = DockStyle.Fill;
        lblFrom.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblFrom.ForeColor = Color.FromArgb(180, 35, 24);
        lblFrom.Location = new Point(437, 0);
        lblFrom.Name = "lblFrom";
        lblFrom.Size = new Size(154, 34);
        lblFrom.TabIndex = 1;
        lblFrom.Text = "من عملة *";
        lblFrom.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cmbFromCurrency
        // 
        cmbFromCurrency.BackColor = Color.FromArgb(255, 249, 219);
        cmbFromCurrency.Dock = DockStyle.Fill;
        cmbFromCurrency.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFromCurrency.Font = new Font("Tahoma", 9F);
        cmbFromCurrency.Location = new Point(3, 3);
        cmbFromCurrency.Name = "cmbFromCurrency";
        cmbFromCurrency.Size = new Size(428, 33);
        cmbFromCurrency.TabIndex = 1;
        // 
        // lblTo
        // 
        lblTo.AutoSize = true;
        lblTo.Dock = DockStyle.Fill;
        lblTo.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblTo.ForeColor = Color.FromArgb(180, 35, 24);
        lblTo.Location = new Point(1031, 34);
        lblTo.Name = "lblTo";
        lblTo.Size = new Size(154, 34);
        lblTo.TabIndex = 2;
        lblTo.Text = "إلى عملة *";
        lblTo.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cmbToCurrency
        // 
        cmbToCurrency.BackColor = Color.FromArgb(255, 249, 219);
        cmbToCurrency.Dock = DockStyle.Fill;
        cmbToCurrency.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbToCurrency.Font = new Font("Tahoma", 9F);
        cmbToCurrency.Location = new Point(597, 37);
        cmbToCurrency.Name = "cmbToCurrency";
        cmbToCurrency.Size = new Size(428, 33);
        cmbToCurrency.TabIndex = 2;
        // 
        // lblType
        // 
        lblType.AutoSize = true;
        lblType.Dock = DockStyle.Fill;
        lblType.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblType.ForeColor = Color.FromArgb(180, 35, 24);
        lblType.Location = new Point(437, 34);
        lblType.Name = "lblType";
        lblType.Size = new Size(154, 34);
        lblType.TabIndex = 3;
        lblType.Text = "نوع السعر *";
        lblType.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cmbRateType
        // 
        cmbRateType.BackColor = Color.FromArgb(255, 249, 219);
        cmbRateType.Dock = DockStyle.Fill;
        cmbRateType.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbRateType.Font = new Font("Tahoma", 9F);
        cmbRateType.Items.AddRange(new object[] { "Standard" });
        cmbRateType.Location = new Point(3, 37);
        cmbRateType.Name = "cmbRateType";
        cmbRateType.Size = new Size(428, 33);
        cmbRateType.TabIndex = 3;
        // 
        // lblFromDate
        // 
        lblFromDate.AutoSize = true;
        lblFromDate.Dock = DockStyle.Fill;
        lblFromDate.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblFromDate.ForeColor = Color.FromArgb(180, 35, 24);
        lblFromDate.Location = new Point(1031, 68);
        lblFromDate.Name = "lblFromDate";
        lblFromDate.Size = new Size(154, 46);
        lblFromDate.TabIndex = 4;
        lblFromDate.Text = "ساري من (UTC) *";
        lblFromDate.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpRateDate
        // 
        dtpRateDate.BackColor = Color.FromArgb(255, 249, 219);
        dtpRateDate.CustomFormat = "yyyy/MM/dd HH:mm:ss 'UTC'";
        dtpRateDate.Dock = DockStyle.Fill;
        dtpRateDate.Font = new Font("Tahoma", 9F);
        dtpRateDate.Format = DateTimePickerFormat.Custom;
        dtpRateDate.Location = new Point(597, 71);
        dtpRateDate.Name = "dtpRateDate";
        dtpRateDate.RightToLeft = RightToLeft.No;
        dtpRateDate.Size = new Size(428, 32);
        dtpRateDate.TabIndex = 4;
        // 
        // lblToDate
        // 
        lblToDate.AutoSize = true;
        lblToDate.Dock = DockStyle.Fill;
        lblToDate.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblToDate.Location = new Point(437, 68);
        lblToDate.Name = "lblToDate";
        lblToDate.Size = new Size(154, 46);
        lblToDate.TabIndex = 5;
        lblToDate.Text = "ساري إلى (UTC) — اختياري";
        lblToDate.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpEffectiveTo
        // 
        dtpEffectiveTo.Checked = false;
        dtpEffectiveTo.CustomFormat = "yyyy/MM/dd HH:mm:ss 'UTC'";
        dtpEffectiveTo.Dock = DockStyle.Fill;
        dtpEffectiveTo.Font = new Font("Tahoma", 9F);
        dtpEffectiveTo.Format = DateTimePickerFormat.Custom;
        dtpEffectiveTo.Location = new Point(3, 71);
        dtpEffectiveTo.Name = "dtpEffectiveTo";
        dtpEffectiveTo.RightToLeft = RightToLeft.No;
        dtpEffectiveTo.ShowCheckBox = true;
        dtpEffectiveTo.Size = new Size(428, 32);
        dtpEffectiveTo.TabIndex = 5;
        // 
        // lblRate
        // 
        lblRate.AutoSize = true;
        lblRate.Dock = DockStyle.Fill;
        lblRate.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblRate.ForeColor = Color.FromArgb(180, 35, 24);
        lblRate.Location = new Point(1031, 114);
        lblRate.Name = "lblRate";
        lblRate.Size = new Size(154, 38);
        lblRate.TabIndex = 6;
        lblRate.Text = "السعر *";
        lblRate.TextAlign = ContentAlignment.MiddleRight;
        // 
        // nudExchangeRate
        // 
        nudExchangeRate.BackColor = Color.FromArgb(255, 249, 219);
        nudExchangeRate.DecimalPlaces = 10;
        nudExchangeRate.Dock = DockStyle.Fill;
        nudExchangeRate.Font = new Font("Tahoma", 9F);
        nudExchangeRate.Increment = new decimal(new int[] { 1, 0, 0, 655360 });
        nudExchangeRate.Location = new Point(597, 117);
        nudExchangeRate.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 655360 });
        nudExchangeRate.Minimum = new decimal(new int[] { 1, 0, 0, 655360 });
        nudExchangeRate.Name = "nudExchangeRate";
        nudExchangeRate.RightToLeft = RightToLeft.No;
        nudExchangeRate.Size = new Size(428, 32);
        nudExchangeRate.TabIndex = 6;
        nudExchangeRate.TextAlign = HorizontalAlignment.Right;
        nudExchangeRate.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblStatus.Location = new Point(437, 114);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(154, 38);
        lblStatus.TabIndex = 7;
        lblStatus.Text = "الحالة";
        lblStatus.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtStatus
        // 
        txtStatus.AccessibleName = "الحالة — للقراءة فقط";
        txtStatus.Dock = DockStyle.Fill;
        txtStatus.Font = new Font("Tahoma", 9F);
        txtStatus.Location = new Point(3, 117);
        txtStatus.Name = "txtStatus";
        txtStatus.PlaceholderText = "لم يتم تحميل سجل";
        txtStatus.ReadOnly = true;
        txtStatus.Size = new Size(428, 32);
        txtStatus.TabIndex = 8;
        txtStatus.TabStop = false;
        // 
        // chkMinimumRate
        // 
        chkMinimumRate.AutoSize = true;
        chkMinimumRate.CheckAlign = ContentAlignment.MiddleRight;
        chkMinimumRate.Dock = DockStyle.Fill;
        chkMinimumRate.Location = new Point(1031, 155);
        chkMinimumRate.Name = "chkMinimumRate";
        chkMinimumRate.Size = new Size(154, 32);
        chkMinimumRate.TabIndex = 8;
        chkMinimumRate.Text = "تحديد حد أدنى";
        chkMinimumRate.TextAlign = ContentAlignment.MiddleRight;
        chkMinimumRate.CheckedChanged += OptionalBounds_CheckedChanged;
        // 
        // nudMinimumRate
        // 
        nudMinimumRate.BackColor = Color.White;
        nudMinimumRate.DecimalPlaces = 10;
        nudMinimumRate.Dock = DockStyle.Fill;
        nudMinimumRate.Enabled = false;
        nudMinimumRate.Font = new Font("Tahoma", 9F);
        nudMinimumRate.Increment = new decimal(new int[] { 1, 0, 0, 655360 });
        nudMinimumRate.Location = new Point(597, 155);
        nudMinimumRate.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 655360 });
        nudMinimumRate.Minimum = new decimal(new int[] { 1, 0, 0, 655360 });
        nudMinimumRate.Name = "nudMinimumRate";
        nudMinimumRate.RightToLeft = RightToLeft.No;
        nudMinimumRate.Size = new Size(428, 32);
        nudMinimumRate.TabIndex = 9;
        nudMinimumRate.TextAlign = HorizontalAlignment.Right;
        nudMinimumRate.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // chkMaximumRate
        // 
        chkMaximumRate.AutoSize = true;
        chkMaximumRate.CheckAlign = ContentAlignment.MiddleRight;
        chkMaximumRate.Dock = DockStyle.Fill;
        chkMaximumRate.Location = new Point(437, 155);
        chkMaximumRate.Name = "chkMaximumRate";
        chkMaximumRate.Size = new Size(154, 32);
        chkMaximumRate.TabIndex = 10;
        chkMaximumRate.Text = "تحديد حد أعلى";
        chkMaximumRate.TextAlign = ContentAlignment.MiddleRight;
        chkMaximumRate.CheckedChanged += OptionalBounds_CheckedChanged;
        // 
        // nudMaximumRate
        // 
        nudMaximumRate.BackColor = Color.White;
        nudMaximumRate.DecimalPlaces = 10;
        nudMaximumRate.Dock = DockStyle.Fill;
        nudMaximumRate.Enabled = false;
        nudMaximumRate.Font = new Font("Tahoma", 9F);
        nudMaximumRate.Increment = new decimal(new int[] { 1, 0, 0, 655360 });
        nudMaximumRate.Location = new Point(3, 155);
        nudMaximumRate.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 655360 });
        nudMaximumRate.Minimum = new decimal(new int[] { 1, 0, 0, 655360 });
        nudMaximumRate.Name = "nudMaximumRate";
        nudMaximumRate.RightToLeft = RightToLeft.No;
        nudMaximumRate.Size = new Size(428, 32);
        nudMaximumRate.TabIndex = 11;
        nudMaximumRate.TextAlign = HorizontalAlignment.Right;
        nudMaximumRate.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // lblReason
        // 
        lblReason.AutoSize = true;
        lblReason.Dock = DockStyle.Fill;
        lblReason.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblReason.Location = new Point(1031, 190);
        lblReason.Name = "lblReason";
        lblReason.Size = new Size(154, 38);
        lblReason.TabIndex = 12;
        lblReason.Text = "سبب الإيقاف";
        lblReason.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtDisableReason
        // 
        txtDisableReason.AccessibleName = "سبب الإيقاف";
        txtDisableReason.Dock = DockStyle.Fill;
        txtDisableReason.Font = new Font("Tahoma", 9F);
        txtDisableReason.Location = new Point(597, 193);
        txtDisableReason.MaxLength = 1000;
        txtDisableReason.Name = "txtDisableReason";
        txtDisableReason.PlaceholderText = "مطلوب عند إيقاف سجل";
        txtDisableReason.Size = new Size(428, 32);
        txtDisableReason.TabIndex = 12;
        // 
        // lblIdentityNote
        // 
        lblIdentityNote.AutoSize = true;
        RatesFields.SetColumnSpan(lblIdentityNote, 2);
        lblIdentityNote.Dock = DockStyle.Fill;
        lblIdentityNote.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblIdentityNote.Location = new Point(3, 190);
        lblIdentityNote.Name = "lblIdentityNote";
        lblIdentityNote.Size = new Size(588, 38);
        lblIdentityNote.TabIndex = 13;
        lblIdentityNote.Text = "حقول الشركة والعملتين والنوع وساري من ثابتة بعد الإنشاء.";
        lblIdentityNote.TextAlign = ContentAlignment.MiddleRight;
        // 
        // grpSearch
        // 
        grpSearch.AutoSize = true;
        grpSearch.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        grpSearch.Controls.Add(SearchFields);
        grpSearch.Dock = DockStyle.Top;
        grpSearch.Location = new Point(3, 266);
        grpSearch.Name = "grpSearch";
        grpSearch.Size = new Size(1194, 173);
        grpSearch.TabIndex = 1;
        grpSearch.TabStop = false;
        grpSearch.Text = "البحث";
        // 
        // SearchFields
        // 
        SearchFields.AutoSize = true;
        SearchFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        SearchFields.ColumnCount = 4;
        SearchFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        SearchFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        SearchFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        SearchFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        SearchFields.Controls.Add(lblFilterCompany, 0, 0);
        SearchFields.Controls.Add(cmbFilterCompany, 1, 0);
        SearchFields.Controls.Add(lblFilterFrom, 2, 0);
        SearchFields.Controls.Add(cmbFilterFromCurrency, 3, 0);
        SearchFields.Controls.Add(lblFilterTo, 0, 1);
        SearchFields.Controls.Add(cmbFilterToCurrency, 1, 1);
        SearchFields.Controls.Add(lblFilterType, 2, 1);
        SearchFields.Controls.Add(cmbFilterRateType, 3, 1);
        SearchFields.Controls.Add(lblFilterAt, 0, 2);
        SearchFields.Controls.Add(dtpFilterEffectiveAt, 1, 2);
        SearchFields.Controls.Add(lblFilterStatus, 2, 2);
        SearchFields.Controls.Add(cmbFilterStatus, 3, 2);
        SearchFields.Controls.Add(lblSearch, 0, 3);
        SearchFields.Controls.Add(txtSearch, 1, 3);
        SearchFields.Controls.Add(lblSearchAction, 2, 3);
        SearchFields.Controls.Add(btnSearch, 3, 3);
        SearchFields.Dock = DockStyle.Top;
        SearchFields.Location = new Point(3, 26);
        SearchFields.Margin = new Padding(0);
        SearchFields.Name = "SearchFields";
        SearchFields.RowCount = 4;
        SearchFields.RowStyles.Add(new RowStyle());
        SearchFields.RowStyles.Add(new RowStyle());
        SearchFields.RowStyles.Add(new RowStyle());
        SearchFields.RowStyles.Add(new RowStyle());
        SearchFields.Size = new Size(1188, 144);
        SearchFields.TabIndex = 0;
        // 
        // lblFilterCompany
        // 
        lblFilterCompany.AutoSize = true;
        lblFilterCompany.Dock = DockStyle.Fill;
        lblFilterCompany.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblFilterCompany.Location = new Point(1031, 0);
        lblFilterCompany.Name = "lblFilterCompany";
        lblFilterCompany.Size = new Size(154, 34);
        lblFilterCompany.TabIndex = 0;
        lblFilterCompany.Text = "الشركة";
        lblFilterCompany.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cmbFilterCompany
        // 
        cmbFilterCompany.BackColor = Color.White;
        cmbFilterCompany.Dock = DockStyle.Fill;
        cmbFilterCompany.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFilterCompany.Font = new Font("Tahoma", 9F);
        cmbFilterCompany.Location = new Point(597, 3);
        cmbFilterCompany.Name = "cmbFilterCompany";
        cmbFilterCompany.Size = new Size(428, 33);
        cmbFilterCompany.TabIndex = 0;
        // 
        // lblFilterFrom
        // 
        lblFilterFrom.AutoSize = true;
        lblFilterFrom.Dock = DockStyle.Fill;
        lblFilterFrom.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblFilterFrom.Location = new Point(437, 0);
        lblFilterFrom.Name = "lblFilterFrom";
        lblFilterFrom.Size = new Size(154, 34);
        lblFilterFrom.TabIndex = 1;
        lblFilterFrom.Text = "من عملة";
        lblFilterFrom.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cmbFilterFromCurrency
        // 
        cmbFilterFromCurrency.BackColor = Color.White;
        cmbFilterFromCurrency.Dock = DockStyle.Fill;
        cmbFilterFromCurrency.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFilterFromCurrency.Font = new Font("Tahoma", 9F);
        cmbFilterFromCurrency.Location = new Point(3, 3);
        cmbFilterFromCurrency.Name = "cmbFilterFromCurrency";
        cmbFilterFromCurrency.Size = new Size(428, 33);
        cmbFilterFromCurrency.TabIndex = 1;
        // 
        // lblFilterTo
        // 
        lblFilterTo.AutoSize = true;
        lblFilterTo.Dock = DockStyle.Fill;
        lblFilterTo.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblFilterTo.Location = new Point(1031, 34);
        lblFilterTo.Name = "lblFilterTo";
        lblFilterTo.Size = new Size(154, 34);
        lblFilterTo.TabIndex = 2;
        lblFilterTo.Text = "إلى عملة";
        lblFilterTo.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cmbFilterToCurrency
        // 
        cmbFilterToCurrency.BackColor = Color.White;
        cmbFilterToCurrency.Dock = DockStyle.Fill;
        cmbFilterToCurrency.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFilterToCurrency.Font = new Font("Tahoma", 9F);
        cmbFilterToCurrency.Location = new Point(597, 37);
        cmbFilterToCurrency.Name = "cmbFilterToCurrency";
        cmbFilterToCurrency.Size = new Size(428, 33);
        cmbFilterToCurrency.TabIndex = 2;
        // 
        // lblFilterType
        // 
        lblFilterType.AutoSize = true;
        lblFilterType.Dock = DockStyle.Fill;
        lblFilterType.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblFilterType.Location = new Point(437, 34);
        lblFilterType.Name = "lblFilterType";
        lblFilterType.Size = new Size(154, 34);
        lblFilterType.TabIndex = 3;
        lblFilterType.Text = "نوع السعر";
        lblFilterType.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cmbFilterRateType
        // 
        cmbFilterRateType.BackColor = Color.White;
        cmbFilterRateType.Dock = DockStyle.Fill;
        cmbFilterRateType.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFilterRateType.Font = new Font("Tahoma", 9F);
        cmbFilterRateType.Items.AddRange(new object[] { "الكل", "Standard" });
        cmbFilterRateType.Location = new Point(3, 37);
        cmbFilterRateType.Name = "cmbFilterRateType";
        cmbFilterRateType.Size = new Size(428, 33);
        cmbFilterRateType.TabIndex = 3;
        // 
        // lblFilterAt
        // 
        lblFilterAt.AutoSize = true;
        lblFilterAt.Dock = DockStyle.Fill;
        lblFilterAt.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblFilterAt.Location = new Point(1031, 68);
        lblFilterAt.Name = "lblFilterAt";
        lblFilterAt.Size = new Size(154, 38);
        lblFilterAt.TabIndex = 4;
        lblFilterAt.Text = "ساري في (UTC)";
        lblFilterAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpFilterEffectiveAt
        // 
        dtpFilterEffectiveAt.Checked = false;
        dtpFilterEffectiveAt.CustomFormat = "yyyy/MM/dd HH:mm:ss 'UTC'";
        dtpFilterEffectiveAt.Dock = DockStyle.Fill;
        dtpFilterEffectiveAt.Font = new Font("Tahoma", 9F);
        dtpFilterEffectiveAt.Format = DateTimePickerFormat.Custom;
        dtpFilterEffectiveAt.Location = new Point(597, 71);
        dtpFilterEffectiveAt.Name = "dtpFilterEffectiveAt";
        dtpFilterEffectiveAt.RightToLeft = RightToLeft.No;
        dtpFilterEffectiveAt.ShowCheckBox = true;
        dtpFilterEffectiveAt.Size = new Size(428, 32);
        dtpFilterEffectiveAt.TabIndex = 4;
        // 
        // lblFilterStatus
        // 
        lblFilterStatus.AutoSize = true;
        lblFilterStatus.Dock = DockStyle.Fill;
        lblFilterStatus.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblFilterStatus.Location = new Point(437, 68);
        lblFilterStatus.Name = "lblFilterStatus";
        lblFilterStatus.Size = new Size(154, 38);
        lblFilterStatus.TabIndex = 5;
        lblFilterStatus.Text = "الحالة";
        lblFilterStatus.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cmbFilterStatus
        // 
        cmbFilterStatus.BackColor = Color.White;
        cmbFilterStatus.Dock = DockStyle.Fill;
        cmbFilterStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFilterStatus.Font = new Font("Tahoma", 9F);
        cmbFilterStatus.Items.AddRange(new object[] { "الكل", "Active", "Stopped" });
        cmbFilterStatus.Location = new Point(3, 71);
        cmbFilterStatus.Name = "cmbFilterStatus";
        cmbFilterStatus.Size = new Size(428, 33);
        cmbFilterStatus.TabIndex = 5;
        // 
        // lblSearch
        // 
        lblSearch.AutoSize = true;
        lblSearch.Dock = DockStyle.Fill;
        lblSearch.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblSearch.Location = new Point(1031, 106);
        lblSearch.Name = "lblSearch";
        lblSearch.Size = new Size(154, 38);
        lblSearch.TabIndex = 6;
        lblSearch.Text = "نص البحث";
        lblSearch.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtSearch
        // 
        txtSearch.Dock = DockStyle.Fill;
        txtSearch.Font = new Font("Tahoma", 9F);
        txtSearch.Location = new Point(597, 109);
        txtSearch.MaxLength = 250;
        txtSearch.Name = "txtSearch";
        txtSearch.Size = new Size(428, 32);
        txtSearch.TabIndex = 6;
        // 
        // lblSearchAction
        // 
        lblSearchAction.AutoSize = true;
        lblSearchAction.Dock = DockStyle.Fill;
        lblSearchAction.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblSearchAction.Location = new Point(437, 106);
        lblSearchAction.Name = "lblSearchAction";
        lblSearchAction.Size = new Size(154, 38);
        lblSearchAction.TabIndex = 7;
        lblSearchAction.Text = "بحث على الخادم";
        lblSearchAction.TextAlign = ContentAlignment.MiddleRight;
        // 
        // btnSearch
        // 
        btnSearch.AutoSize = true;
        btnSearch.Enabled = false;
        btnSearch.Font = new Font("Tahoma", 9F);
        btnSearch.Location = new Point(387, 109);
        btnSearch.Name = "btnSearch";
        btnSearch.Size = new Size(44, 30);
        btnSearch.TabIndex = 7;
        btnSearch.Text = "بحث";
        // 
        // dgvRates
        // 
        dgvRates.AllowUserToAddRows = false;
        dgvRates.AllowUserToDeleteRows = false;
        dgvRates.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvRates.BackgroundColor = Color.White;
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(224, 224, 224);
        dataGridViewCellStyle1.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
        dgvRates.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        dgvRates.ColumnHeadersHeight = 29;
        dgvRates.Columns.AddRange(new DataGridViewColumn[] { dgvRatesColFrom, dgvRatesColTo, dgvRatesColType, dgvRatesColDate, dgvRatesColEffectiveTo, dgvRatesColValue, dgvRatesColMinimum, dgvRatesColMaximum, dgvRatesColStatus });
        dgvRates.Dock = DockStyle.Fill;
        dgvRates.EnableHeadersVisualStyles = false;
        dgvRates.Location = new Point(3, 445);
        dgvRates.MinimumSize = new Size(0, 160);
        dgvRates.MultiSelect = false;
        dgvRates.Name = "dgvRates";
        dgvRates.ReadOnly = true;
        dgvRates.RowHeadersVisible = false;
        dgvRates.RowHeadersWidth = 51;
        dgvRates.RowTemplate.Height = 30;
        dgvRates.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvRates.Size = new Size(1194, 281);
        dgvRates.TabIndex = 2;
        // 
        // dgvRatesColFrom
        // 
        dgvRatesColFrom.HeaderText = "من عملة";
        dgvRatesColFrom.MinimumWidth = 90;
        dgvRatesColFrom.Name = "dgvRatesColFrom";
        dgvRatesColFrom.ReadOnly = true;
        dgvRatesColFrom.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // dgvRatesColTo
        // 
        dgvRatesColTo.HeaderText = "إلى عملة";
        dgvRatesColTo.MinimumWidth = 90;
        dgvRatesColTo.Name = "dgvRatesColTo";
        dgvRatesColTo.ReadOnly = true;
        dgvRatesColTo.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // dgvRatesColType
        // 
        dgvRatesColType.HeaderText = "نوع السعر";
        dgvRatesColType.MinimumWidth = 90;
        dgvRatesColType.Name = "dgvRatesColType";
        dgvRatesColType.ReadOnly = true;
        dgvRatesColType.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // dgvRatesColDate
        // 
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle2.Format = "yyyy/MM/dd HH:mm:ss";
        dgvRatesColDate.DefaultCellStyle = dataGridViewCellStyle2;
        dgvRatesColDate.HeaderText = "ساري من (UTC)";
        dgvRatesColDate.MinimumWidth = 110;
        dgvRatesColDate.Name = "dgvRatesColDate";
        dgvRatesColDate.ReadOnly = true;
        dgvRatesColDate.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // dgvRatesColEffectiveTo
        // 
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle3.Format = "yyyy/MM/dd HH:mm:ss";
        dgvRatesColEffectiveTo.DefaultCellStyle = dataGridViewCellStyle3;
        dgvRatesColEffectiveTo.HeaderText = "ساري إلى (UTC)";
        dgvRatesColEffectiveTo.MinimumWidth = 110;
        dgvRatesColEffectiveTo.Name = "dgvRatesColEffectiveTo";
        dgvRatesColEffectiveTo.ReadOnly = true;
        dgvRatesColEffectiveTo.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // dgvRatesColValue
        // 
        dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle4.Format = "0.##########";
        dgvRatesColValue.DefaultCellStyle = dataGridViewCellStyle4;
        dgvRatesColValue.HeaderText = "السعر";
        dgvRatesColValue.MinimumWidth = 90;
        dgvRatesColValue.Name = "dgvRatesColValue";
        dgvRatesColValue.ReadOnly = true;
        dgvRatesColValue.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // dgvRatesColMinimum
        // 
        dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle5.Format = "0.##########";
        dgvRatesColMinimum.DefaultCellStyle = dataGridViewCellStyle5;
        dgvRatesColMinimum.HeaderText = "الحد الأدنى";
        dgvRatesColMinimum.MinimumWidth = 90;
        dgvRatesColMinimum.Name = "dgvRatesColMinimum";
        dgvRatesColMinimum.ReadOnly = true;
        dgvRatesColMinimum.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // dgvRatesColMaximum
        // 
        dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle6.Format = "0.##########";
        dgvRatesColMaximum.DefaultCellStyle = dataGridViewCellStyle6;
        dgvRatesColMaximum.HeaderText = "الحد الأعلى";
        dgvRatesColMaximum.MinimumWidth = 90;
        dgvRatesColMaximum.Name = "dgvRatesColMaximum";
        dgvRatesColMaximum.ReadOnly = true;
        dgvRatesColMaximum.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // dgvRatesColStatus
        // 
        dgvRatesColStatus.HeaderText = "الحالة";
        dgvRatesColStatus.MinimumWidth = 90;
        dgvRatesColStatus.Name = "dgvRatesColStatus";
        dgvRatesColStatus.ReadOnly = true;
        dgvRatesColStatus.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // lblRateConvention
        // 
        lblRateConvention.AutoSize = true;
        lblRateConvention.Dock = DockStyle.Fill;
        lblRateConvention.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblRateConvention.Location = new Point(3, 729);
        lblRateConvention.MaximumSize = new Size(900, 0);
        lblRateConvention.Name = "lblRateConvention";
        lblRateConvention.Size = new Size(900, 46);
        lblRateConvention.TabIndex = 3;
        lblRateConvention.Text = "الاتجاه: وحدة واحدة من العملة المصدر = سعر الصرف من العملة الهدف. الفترات بتوقيت UTC؛ عدم تحديد ساري إلى يعني فترة مفتوحة.";
        lblRateConvention.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlPagination
        // 
        pnlPagination.AutoSize = true;
        pnlPagination.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlPagination.Dock = DockStyle.Top;
        pnlPagination.Location = new Point(3, 826);
        pnlPagination.Name = "pnlPagination";
        pnlPagination.RightToLeft = RightToLeft.Yes;
        pnlPagination.Size = new Size(1194, 0);
        pnlPagination.TabIndex = 2;
        // 
        // lblPreview
        // 
        lblPreview.AutoSize = true;
        lblPreview.Dock = DockStyle.Fill;
        lblPreview.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblPreview.ForeColor = Color.DimGray;
        lblPreview.Location = new Point(3, 829);
        lblPreview.MaximumSize = new Size(1100, 0);
        lblPreview.Name = "lblPreview";
        lblPreview.Size = new Size(1100, 23);
        lblPreview.TabIndex = 3;
        lblPreview.Text = "واجهة تصميم — لا توجد بيانات محملة؛ الحفظ والإيقاف والبحث وترقيم الصفحات غير مرتبطة بالخادم بعد.";
        lblPreview.TextAlign = ContentAlignment.MiddleRight;
        // 
        // tlpAuditInfo
        // 
        tlpAuditInfo.ColumnCount = 7;
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857141F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857141F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857141F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857141F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857141F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857141F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857141F));
        tlpAuditInfo.Controls.Add(lblCreatedBy, 0, 0);
        tlpAuditInfo.Controls.Add(lblCreatedAt, 1, 0);
        tlpAuditInfo.Controls.Add(lblModifiedBy, 2, 0);
        tlpAuditInfo.Controls.Add(lblModifiedAt, 3, 0);
        tlpAuditInfo.Controls.Add(lblEditCount, 4, 0);
        tlpAuditInfo.Controls.Add(lblLastPrintedAt, 5, 0);
        tlpAuditInfo.Controls.Add(lblPrintCount, 6, 0);
        tlpAuditInfo.Dock = DockStyle.Fill;
        tlpAuditInfo.Location = new Point(0, 852);
        tlpAuditInfo.Margin = new Padding(0);
        tlpAuditInfo.Name = "tlpAuditInfo";
        tlpAuditInfo.RowCount = 1;
        tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpAuditInfo.Size = new Size(1200, 48);
        tlpAuditInfo.TabIndex = 4;
        // 
        // lblCreatedBy
        // 
        lblCreatedBy.AutoEllipsis = true;
        lblCreatedBy.BackColor = Color.FromArgb(192, 255, 255);
        lblCreatedBy.Dock = DockStyle.Fill;
        lblCreatedBy.Font = new Font("Tahoma", 9F);
        lblCreatedBy.Location = new Point(1032, 0);
        lblCreatedBy.Name = "lblCreatedBy";
        lblCreatedBy.Size = new Size(165, 48);
        lblCreatedBy.TabIndex = 0;
        lblCreatedBy.Text = "أنشأ بواسطة: —";
        lblCreatedBy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCreatedAt
        // 
        lblCreatedAt.AutoEllipsis = true;
        lblCreatedAt.BackColor = Color.FromArgb(192, 255, 255);
        lblCreatedAt.Dock = DockStyle.Fill;
        lblCreatedAt.Font = new Font("Tahoma", 9F);
        lblCreatedAt.Location = new Point(861, 0);
        lblCreatedAt.Name = "lblCreatedAt";
        lblCreatedAt.Size = new Size(165, 48);
        lblCreatedAt.TabIndex = 1;
        lblCreatedAt.Text = "تاريخ الإنشاء: —";
        lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblModifiedBy
        // 
        lblModifiedBy.AutoEllipsis = true;
        lblModifiedBy.BackColor = Color.FromArgb(192, 255, 255);
        lblModifiedBy.Dock = DockStyle.Fill;
        lblModifiedBy.Font = new Font("Tahoma", 9F);
        lblModifiedBy.Location = new Point(690, 0);
        lblModifiedBy.Name = "lblModifiedBy";
        lblModifiedBy.Size = new Size(165, 48);
        lblModifiedBy.TabIndex = 2;
        lblModifiedBy.Text = "عدّل بواسطة: —";
        lblModifiedBy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblModifiedAt
        // 
        lblModifiedAt.AutoEllipsis = true;
        lblModifiedAt.BackColor = Color.FromArgb(192, 255, 255);
        lblModifiedAt.Dock = DockStyle.Fill;
        lblModifiedAt.Font = new Font("Tahoma", 9F);
        lblModifiedAt.Location = new Point(519, 0);
        lblModifiedAt.Name = "lblModifiedAt";
        lblModifiedAt.Size = new Size(165, 48);
        lblModifiedAt.TabIndex = 3;
        lblModifiedAt.Text = "تاريخ التعديل: —";
        lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblEditCount
        // 
        lblEditCount.AutoEllipsis = true;
        lblEditCount.BackColor = Color.FromArgb(192, 255, 255);
        lblEditCount.Dock = DockStyle.Fill;
        lblEditCount.Font = new Font("Tahoma", 9F);
        lblEditCount.Location = new Point(348, 0);
        lblEditCount.Name = "lblEditCount";
        lblEditCount.Size = new Size(165, 48);
        lblEditCount.TabIndex = 4;
        lblEditCount.Text = "عدد التعديلات: —";
        lblEditCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblLastPrintedAt
        // 
        lblLastPrintedAt.AutoEllipsis = true;
        lblLastPrintedAt.BackColor = Color.FromArgb(192, 255, 255);
        lblLastPrintedAt.Dock = DockStyle.Fill;
        lblLastPrintedAt.Font = new Font("Tahoma", 9F);
        lblLastPrintedAt.Location = new Point(177, 0);
        lblLastPrintedAt.Name = "lblLastPrintedAt";
        lblLastPrintedAt.Size = new Size(165, 48);
        lblLastPrintedAt.TabIndex = 5;
        lblLastPrintedAt.Text = "تاريخ آخر طباعة: —";
        lblLastPrintedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblPrintCount
        // 
        lblPrintCount.AutoEllipsis = true;
        lblPrintCount.BackColor = Color.FromArgb(192, 255, 255);
        lblPrintCount.Dock = DockStyle.Fill;
        lblPrintCount.Font = new Font("Tahoma", 9F);
        lblPrintCount.Location = new Point(3, 0);
        lblPrintCount.Name = "lblPrintCount";
        lblPrintCount.Size = new Size(168, 48);
        lblPrintCount.TabIndex = 6;
        lblPrintCount.Text = "عدد مرات الطباعة: —";
        lblPrintCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // btnFirst
        // 
        btnFirst.AutoSize = true;
        btnFirst.Enabled = false;
        btnFirst.Font = new Font("Tahoma", 9F);
        btnFirst.Location = new Point(685, 3);
        btnFirst.MinimumSize = new Size(70, 30);
        btnFirst.Name = "btnFirst";
        btnFirst.Size = new Size(75, 30);
        btnFirst.TabIndex = 6;
        btnFirst.Text = "الأول";
        // 
        // btnPrevious
        // 
        btnPrevious.AutoSize = true;
        btnPrevious.Enabled = false;
        btnPrevious.Font = new Font("Tahoma", 9F);
        btnPrevious.Location = new Point(604, 3);
        btnPrevious.MinimumSize = new Size(70, 30);
        btnPrevious.Name = "btnPrevious";
        btnPrevious.Size = new Size(75, 30);
        btnPrevious.TabIndex = 7;
        btnPrevious.Text = "السابق";
        // 
        // txtCurrentRecordNo
        // 
        txtCurrentRecordNo.Font = new Font("Tahoma", 9F);
        txtCurrentRecordNo.Location = new Point(518, 3);
        txtCurrentRecordNo.Name = "txtCurrentRecordNo";
        txtCurrentRecordNo.ReadOnly = true;
        txtCurrentRecordNo.Size = new Size(80, 32);
        txtCurrentRecordNo.TabIndex = 8;
        txtCurrentRecordNo.TabStop = false;
        txtCurrentRecordNo.Text = "—";
        txtCurrentRecordNo.TextAlign = HorizontalAlignment.Center;
        // 
        // btnNext
        // 
        btnNext.AutoSize = true;
        btnNext.Enabled = false;
        btnNext.Font = new Font("Tahoma", 9F);
        btnNext.Location = new Point(437, 3);
        btnNext.MinimumSize = new Size(70, 30);
        btnNext.Name = "btnNext";
        btnNext.Size = new Size(75, 30);
        btnNext.TabIndex = 9;
        btnNext.Text = "التالي";
        // 
        // btnLast
        // 
        btnLast.AutoSize = true;
        btnLast.Enabled = false;
        btnLast.Font = new Font("Tahoma", 9F);
        btnLast.Location = new Point(356, 3);
        btnLast.MinimumSize = new Size(70, 30);
        btnLast.Name = "btnLast";
        btnLast.Size = new Size(75, 30);
        btnLast.TabIndex = 10;
        btnLast.Text = "الأخير";
        // 
        // btnDisable
        // 
        btnDisable.AutoSize = true;
        btnDisable.Enabled = false;
        btnDisable.FlatStyle = FlatStyle.Flat;
        btnDisable.Font = new Font("Tahoma", 9F);
        btnDisable.Location = new Point(274, 4);
        btnDisable.Margin = new Padding(4);
        btnDisable.MinimumSize = new Size(70, 30);
        btnDisable.Name = "btnDisable";
        btnDisable.Size = new Size(75, 32);
        btnDisable.TabIndex = 11;
        btnDisable.Text = "إيقاف";
        // 
        // btnRefresh
        // 
        btnRefresh.AutoSize = true;
        btnRefresh.Enabled = false;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 9F);
        btnRefresh.Location = new Point(191, 4);
        btnRefresh.Margin = new Padding(4);
        btnRefresh.MinimumSize = new Size(70, 30);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(75, 32);
        btnRefresh.TabIndex = 12;
        btnRefresh.Text = "تحديث";
        // 
        // btnClose
        // 
        btnClose.AutoSize = true;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 9F);
        btnClose.Location = new Point(108, 4);
        btnClose.Margin = new Padding(4);
        btnClose.MinimumSize = new Size(70, 30);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(75, 32);
        btnClose.TabIndex = 13;
        btnClose.Text = "إغلاق";
        // 
        // UcExchangeRates
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(248, 250, 252);
        Controls.Add(mainLayout);
        Font = new Font("Tahoma", 9F);
        Name = "UcExchangeRates";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1200, 900);
        Tag = "02.04.03";
        mainLayout.ResumeLayout(false);
        mainLayout.PerformLayout();
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlActions.ResumeLayout(false);
        pnlActions.PerformLayout();
        contentLayout.ResumeLayout(false);
        contentLayout.PerformLayout();
        grpMainData.ResumeLayout(false);
        grpMainData.PerformLayout();
        RatesFields.ResumeLayout(false);
        RatesFields.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudExchangeRate).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudMinimumRate).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudMaximumRate).EndInit();
        grpSearch.ResumeLayout(false);
        grpSearch.PerformLayout();
        SearchFields.ResumeLayout(false);
        SearchFields.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvRates).EndInit();
        tlpAuditInfo.ResumeLayout(false);
        ResumeLayout(false);
    
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(0, 0);
        designerCommandBar.Size = new Size(1012, 40);
        designerCommandBar.TabIndex = 0;
        designerCommandBar.RightToLeft = RightToLeft.Yes;
        designerCommandBar.Controls.Add(pnlActions);
        pnlActions.Dock = DockStyle.Fill;
        pnlActions.Margin = Padding.Empty;
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
        standardCommandCancel.Name = "standardCommandCancel";
        standardCommandCancel.Enabled = false;
        standardCommandCancel.Visible = false;
        standardCommandCancel.AccessibleName = "تراجع";
        standardCommandView.Name = "standardCommandView";
        standardCommandView.Enabled = false;
        standardCommandView.Visible = false;
        standardCommandView.AccessibleName = "عرض";
        standardCommandPrint.Name = "standardCommandPrint";
        standardCommandPrint.Enabled = false;
        standardCommandPrint.Visible = false;
        standardCommandPrint.AccessibleName = "طباعة";
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
        designerCommandBar.SetCommandRole(btnNew, TransportERP.Desktop.CoreUI.DesignerCommandRole.Add);
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
        standardCommandCancel.AutoSize = false;
        standardCommandCancel.Dock = DockStyle.None;
        standardCommandCancel.MinimumSize = Size.Empty;
        standardCommandCancel.Size = new Size(26, 24);
        standardCommandCancel.Margin = new Padding(1);
        standardCommandCancel.FlatStyle = FlatStyle.Flat;
        standardCommandCancel.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandCancel.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Cancel;
        standardCommandCancel.Text = "";
        designerHiddenCommands.Controls.Add(standardCommandCancel);
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
        designerHiddenCommands.Controls.Add(standardCommandView);
        designerCommandBar.SetCommandRole(standardCommandView, TransportERP.Desktop.CoreUI.DesignerCommandRole.View);
        designerCommandBar.SetCommandRole(btnLast, TransportERP.Desktop.CoreUI.DesignerCommandRole.Last);
        designerCommandBar.SetCommandRole(btnNext, TransportERP.Desktop.CoreUI.DesignerCommandRole.Next);
        designerCommandBar.SetCommandRole(btnPrevious, TransportERP.Desktop.CoreUI.DesignerCommandRole.Previous);
        designerCommandBar.SetCommandRole(btnFirst, TransportERP.Desktop.CoreUI.DesignerCommandRole.First);
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
        designerHiddenCommands.Controls.Add(standardCommandPrint);
        designerCommandBar.SetCommandRole(standardCommandPrint, TransportERP.Desktop.CoreUI.DesignerCommandRole.Print);
        designerCommandBar.SetCommandRole(btnClose, TransportERP.Desktop.CoreUI.DesignerCommandRole.Close);
        designerCommandBar.SetCommandRole(btnRefresh, TransportERP.Desktop.CoreUI.DesignerCommandRole.Refresh);
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
        tlpAuditInfo.Visible = false;
        Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
