using System.Drawing;
using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

partial class UcCurrencyManagement
{
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;

    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
    private FlowLayoutPanel designerHiddenCommands = null!;
    private Button standardCommandImport = null!;
    private Button standardCommandExport = null!;
    private Button standardCommandHelp = null!;

    private System.ComponentModel.IContainer? components;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        designerHiddenCommands = new FlowLayoutPanel();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        mainLayout = new TableLayoutPanel();
        pnlHeader = new Panel();
        pnlActions = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        btnFirst = new Button();
        btnPrevious = new Button();
        txtCurrentRecordNo = new TextBox();
        btnNext = new Button();
        btnLast = new Button();
        btnUndo = new Button();
        btnSearch = new Button();
        btnPrint = new Button();
        btnApprove = new Button();
        lblTitle = new Label();
        tlpAuditInfo = new TableLayoutPanel();
        lblCreatedBy = new Label();
        lblCreatedAt = new Label();
        lblModifiedBy = new Label();
        lblModifiedAt = new Label();
        lblEditCount = new Label();
        lblLastPrintedAt = new Label();
        lblPrintCount = new Label();
        tabMain = new TabControl();
        tabCurrencies = new TabPage();
        Currencies = new TableLayoutPanel();
        CurrenciesFields = new TableLayoutPanel();
        lbltxtCurrencyCode = new Label();
        txtCurrencyCode = new TextBox();
        lbltxtCurrencyNameAr = new Label();
        txtCurrencyNameAr = new TextBox();
        lbltxtCurrencyNameEn = new Label();
        txtCurrencyNameEn = new TextBox();
        lbltxtCurrencySymbol = new Label();
        txtCurrencySymbol = new TextBox();
        lblnudDecimalPlaces = new Label();
        nudDecimalPlaces = new NumericUpDown();
        lblchkCurrencyActive = new Label();
        chkCurrencyActive = new CheckBox();
        dgvCurrencies = new DataGridView();
        dgvCurrenciesColCode = new DataGridViewTextBoxColumn();
        dgvCurrenciesColNameAr = new DataGridViewTextBoxColumn();
        dgvCurrenciesColNameEn = new DataGridViewTextBoxColumn();
        dgvCurrenciesColSymbol = new DataGridViewTextBoxColumn();
        dgvCurrenciesColDecimals = new DataGridViewTextBoxColumn();
        dgvCurrenciesColActive = new DataGridViewTextBoxColumn();
        tabCurrencyRoles = new TabPage();
        rolesLayout = new TableLayoutPanel();
        CurrencyRoles = new TableLayoutPanel();
        lblcmbCompany = new Label();
        cmbCompany = new ComboBox();
        lblcmbAccountingCurrency = new Label();
        cmbAccountingCurrency = new ComboBox();
        lblcmbReportingCurrency = new Label();
        cmbReportingCurrency = new ComboBox();
        lblcmbDefaultTransactionCurrency = new Label();
        cmbDefaultTransactionCurrency = new ComboBox();
        grpAllowedCurrencies = new GroupBox();
        SettingValue__SET_CUR_001 = new CheckedListBox();
        tpNotes = new TabPage();
        txtNotes = new TextBox();
        lblPreview = new Label();
        mainLayout.SuspendLayout();
        pnlHeader.SuspendLayout();
        pnlActions.SuspendLayout();
        tlpAuditInfo.SuspendLayout();
        tabMain.SuspendLayout();
        tabCurrencies.SuspendLayout();
        Currencies.SuspendLayout();
        CurrenciesFields.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudDecimalPlaces).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvCurrencies).BeginInit();
        tabCurrencyRoles.SuspendLayout();
        rolesLayout.SuspendLayout();
        CurrencyRoles.SuspendLayout();
        grpAllowedCurrencies.SuspendLayout();
        tpNotes.SuspendLayout();
        SuspendLayout();
        // 
        // mainLayout
        // 
        mainLayout.ColumnCount = 1;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        mainLayout.Controls.Add(pnlHeader, 0, 0);
        mainLayout.Controls.Add(tlpAuditInfo, 0, 3);
        mainLayout.Controls.Add(lblPreview, 0, 2);
        mainLayout.Controls.Add(tabMain, 0, 1);
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Location = new Point(0, 0);
        mainLayout.Name = "mainLayout";
        mainLayout.RowCount = 4;
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        mainLayout.Size = new Size(1200, 900);
        mainLayout.TabIndex = 0;
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
        pnlActions.AutoScroll = true;
        pnlActions.AutoSize = true;
        pnlActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlActions.BackColor = Color.FromArgb(224, 224, 224);
        pnlActions.Controls.Add(btnNew);
        pnlActions.Controls.Add(btnSave);
        pnlActions.Controls.Add(btnEdit);
        pnlActions.Controls.Add(btnDelete);
        pnlActions.Controls.Add(btnRefresh);
        pnlActions.Controls.Add(btnClose);
        pnlActions.Controls.Add(btnFirst);
        pnlActions.Controls.Add(btnPrevious);
        pnlActions.Controls.Add(txtCurrentRecordNo);
        pnlActions.Controls.Add(btnNext);
        pnlActions.Controls.Add(btnLast);
        pnlActions.Controls.Add(btnUndo);
        pnlActions.Controls.Add(btnSearch);
        pnlActions.Controls.Add(btnPrint);
        pnlActions.Controls.Add(btnApprove);
        pnlActions.Dock = DockStyle.Top;
        pnlActions.Location = new Point(0, 0);
        pnlActions.Margin = new Padding(0);
        pnlActions.MinimumSize = new Size(0, 48);
        pnlActions.Name = "pnlActions";
        pnlActions.Padding = new Padding(5);
        pnlActions.RightToLeft = RightToLeft.Yes;
        pnlActions.Size = new Size(1013, 86);
        pnlActions.TabIndex = 0;
        // 
        // btnNew
        // 
        btnNew.BackColor = Color.FromArgb(224, 224, 224);
        btnNew.Enabled = false;
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Font = new Font("Tahoma", 9F);
        btnNew.ForeColor = Color.FromArgb(16, 24, 40);
        btnNew.Location = new Point(929, 9);
        btnNew.Margin = new Padding(4);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(70, 30);
        btnNew.TabIndex = 0;
        btnNew.Text = "جديد";
        btnNew.UseVisualStyleBackColor = false;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(224, 224, 224);
        btnSave.Enabled = false;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 9F);
        btnSave.ForeColor = Color.FromArgb(16, 24, 40);
        btnSave.Location = new Point(851, 9);
        btnSave.Margin = new Padding(4);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(70, 30);
        btnSave.TabIndex = 1;
        btnSave.Text = "حفظ";
        btnSave.UseVisualStyleBackColor = false;
        // 
        // btnEdit
        // 
        btnEdit.BackColor = Color.FromArgb(224, 224, 224);
        btnEdit.Enabled = false;
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.Font = new Font("Tahoma", 9F);
        btnEdit.ForeColor = Color.FromArgb(16, 24, 40);
        btnEdit.Location = new Point(773, 9);
        btnEdit.Margin = new Padding(4);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(70, 30);
        btnEdit.TabIndex = 2;
        btnEdit.Text = "تعديل";
        btnEdit.UseVisualStyleBackColor = false;
        // 
        // btnDelete
        // 
        btnDelete.BackColor = Color.FromArgb(224, 224, 224);
        btnDelete.Enabled = false;
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.Font = new Font("Tahoma", 9F);
        btnDelete.ForeColor = Color.FromArgb(16, 24, 40);
        btnDelete.Location = new Point(695, 9);
        btnDelete.Margin = new Padding(4);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(70, 30);
        btnDelete.TabIndex = 3;
        btnDelete.Text = "حذف";
        btnDelete.UseVisualStyleBackColor = false;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.FromArgb(224, 224, 224);
        btnRefresh.Enabled = false;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 9F);
        btnRefresh.ForeColor = Color.FromArgb(16, 24, 40);
        btnRefresh.Location = new Point(617, 9);
        btnRefresh.Margin = new Padding(4);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(70, 30);
        btnRefresh.TabIndex = 4;
        btnRefresh.Text = "تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(224, 224, 224);
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 9F);
        btnClose.ForeColor = Color.FromArgb(16, 24, 40);
        btnClose.Location = new Point(539, 9);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(70, 30);
        btnClose.TabIndex = 5;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += BtnClose_Click;
        // 
        // btnFirst
        // 
        btnFirst.BackColor = Color.FromArgb(224, 224, 224);
        btnFirst.Enabled = false;
        btnFirst.FlatStyle = FlatStyle.Flat;
        btnFirst.Font = new Font("Tahoma", 9F);
        btnFirst.ForeColor = Color.FromArgb(16, 24, 40);
        btnFirst.Location = new Point(461, 9);
        btnFirst.Margin = new Padding(4);
        btnFirst.Name = "btnFirst";
        btnFirst.Size = new Size(70, 30);
        btnFirst.TabIndex = 6;
        btnFirst.Text = "الأول";
        btnFirst.UseVisualStyleBackColor = false;
        // 
        // btnPrevious
        // 
        btnPrevious.BackColor = Color.FromArgb(224, 224, 224);
        btnPrevious.Enabled = false;
        btnPrevious.FlatStyle = FlatStyle.Flat;
        btnPrevious.Font = new Font("Tahoma", 9F);
        btnPrevious.ForeColor = Color.FromArgb(16, 24, 40);
        btnPrevious.Location = new Point(383, 9);
        btnPrevious.Margin = new Padding(4);
        btnPrevious.Name = "btnPrevious";
        btnPrevious.Size = new Size(70, 30);
        btnPrevious.TabIndex = 7;
        btnPrevious.Text = "السابق";
        btnPrevious.UseVisualStyleBackColor = false;
        // 
        // txtCurrentRecordNo
        // 
        txtCurrentRecordNo.AccessibleName = "رقم السجل الحالي";
        txtCurrentRecordNo.Font = new Font("Tahoma", 9F);
        txtCurrentRecordNo.Location = new Point(313, 9);
        txtCurrentRecordNo.Margin = new Padding(4);
        txtCurrentRecordNo.Name = "txtCurrentRecordNo";
        txtCurrentRecordNo.ReadOnly = true;
        txtCurrentRecordNo.Size = new Size(62, 26);
        txtCurrentRecordNo.TabIndex = 8;
        txtCurrentRecordNo.TabStop = false;
        txtCurrentRecordNo.Text = "—";
        txtCurrentRecordNo.TextAlign = HorizontalAlignment.Center;
        // 
        // btnNext
        // 
        btnNext.BackColor = Color.FromArgb(224, 224, 224);
        btnNext.Enabled = false;
        btnNext.FlatStyle = FlatStyle.Flat;
        btnNext.Font = new Font("Tahoma", 9F);
        btnNext.ForeColor = Color.FromArgb(16, 24, 40);
        btnNext.Location = new Point(235, 9);
        btnNext.Margin = new Padding(4);
        btnNext.Name = "btnNext";
        btnNext.Size = new Size(70, 30);
        btnNext.TabIndex = 8;
        btnNext.Text = "التالي";
        btnNext.UseVisualStyleBackColor = false;
        // 
        // btnLast
        // 
        btnLast.BackColor = Color.FromArgb(224, 224, 224);
        btnLast.Enabled = false;
        btnLast.FlatStyle = FlatStyle.Flat;
        btnLast.Font = new Font("Tahoma", 9F);
        btnLast.ForeColor = Color.FromArgb(16, 24, 40);
        btnLast.Location = new Point(157, 9);
        btnLast.Margin = new Padding(4);
        btnLast.Name = "btnLast";
        btnLast.Size = new Size(70, 30);
        btnLast.TabIndex = 9;
        btnLast.Text = "الأخير";
        btnLast.UseVisualStyleBackColor = false;
        // 
        // btnUndo
        // 
        btnUndo.BackColor = Color.FromArgb(224, 224, 224);
        btnUndo.Enabled = false;
        btnUndo.FlatStyle = FlatStyle.Flat;
        btnUndo.Font = new Font("Tahoma", 9F);
        btnUndo.ForeColor = Color.FromArgb(16, 24, 40);
        btnUndo.Location = new Point(79, 9);
        btnUndo.Margin = new Padding(4);
        btnUndo.Name = "btnUndo";
        btnUndo.Size = new Size(70, 30);
        btnUndo.TabIndex = 10;
        btnUndo.Text = "تراجع";
        btnUndo.UseVisualStyleBackColor = false;
        // 
        // btnSearch
        // 
        btnSearch.BackColor = Color.FromArgb(224, 224, 224);
        btnSearch.Enabled = false;
        btnSearch.FlatStyle = FlatStyle.Flat;
        btnSearch.Font = new Font("Tahoma", 9F);
        btnSearch.ForeColor = Color.FromArgb(16, 24, 40);
        btnSearch.Location = new Point(929, 47);
        btnSearch.Margin = new Padding(4);
        btnSearch.Name = "btnSearch";
        btnSearch.Size = new Size(70, 30);
        btnSearch.TabIndex = 11;
        btnSearch.Text = "بحث";
        btnSearch.UseVisualStyleBackColor = false;
        // 
        // btnPrint
        // 
        btnPrint.BackColor = Color.FromArgb(224, 224, 224);
        btnPrint.Enabled = false;
        btnPrint.FlatStyle = FlatStyle.Flat;
        btnPrint.Font = new Font("Tahoma", 9F);
        btnPrint.ForeColor = Color.FromArgb(16, 24, 40);
        btnPrint.Location = new Point(851, 47);
        btnPrint.Margin = new Padding(4);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(70, 30);
        btnPrint.TabIndex = 12;
        btnPrint.Text = "طباعة";
        btnPrint.UseVisualStyleBackColor = false;
        // 
        // btnApprove
        // 
        btnApprove.BackColor = Color.FromArgb(224, 224, 224);
        btnApprove.Enabled = false;
        btnApprove.FlatStyle = FlatStyle.Flat;
        btnApprove.Font = new Font("Tahoma", 9F);
        btnApprove.ForeColor = Color.FromArgb(16, 24, 40);
        btnApprove.Location = new Point(773, 47);
        btnApprove.Margin = new Padding(4);
        btnApprove.Name = "btnApprove";
        btnApprove.Size = new Size(70, 30);
        btnApprove.TabIndex = 13;
        btnApprove.Text = "اعتماد";
        btnApprove.UseVisualStyleBackColor = false;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.BackColor = Color.FromArgb(192, 192, 255);
        lblTitle.Dock = DockStyle.Right;
        lblTitle.Font = new Font("Tahoma", 9F);
        lblTitle.Location = new Point(1013, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Padding = new Padding(8, 0, 15, 0);
        lblTitle.RightToLeft = RightToLeft.Yes;
        lblTitle.Size = new Size(187, 37);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "إدارة العملات";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // tlpAuditInfo
        // 
        tlpAuditInfo.AccessibleName = "بيانات الإنشاء والتعديل والطباعة";
        tlpAuditInfo.BackColor = Color.FromArgb(248, 250, 252);
        tlpAuditInfo.ColumnCount = 7;
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
        tlpAuditInfo.Controls.Add(lblCreatedBy, 0, 0);
        tlpAuditInfo.Controls.Add(lblCreatedAt, 1, 0);
        tlpAuditInfo.Controls.Add(lblModifiedBy, 2, 0);
        tlpAuditInfo.Controls.Add(lblModifiedAt, 3, 0);
        tlpAuditInfo.Controls.Add(lblEditCount, 4, 0);
        tlpAuditInfo.Controls.Add(lblLastPrintedAt, 5, 0);
        tlpAuditInfo.Controls.Add(lblPrintCount, 6, 0);
        tlpAuditInfo.Dock = DockStyle.Fill;
        tlpAuditInfo.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
        tlpAuditInfo.Location = new Point(0, 852);
        tlpAuditInfo.Margin = new Padding(0);
        tlpAuditInfo.Name = "tlpAuditInfo";
        tlpAuditInfo.Padding = new Padding(8);
        tlpAuditInfo.RightToLeft = RightToLeft.Yes;
        tlpAuditInfo.RowCount = 1;
        tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpAuditInfo.Size = new Size(1200, 48);
        tlpAuditInfo.TabIndex = 1;
        // 
        // lblCreatedBy
        // 
        lblCreatedBy.AutoEllipsis = true;
        lblCreatedBy.BackColor = Color.FromArgb(192, 255, 255);
        lblCreatedBy.Dock = DockStyle.Fill;
        lblCreatedBy.Font = new Font("Tahoma", 9F);
        lblCreatedBy.Location = new Point(1030, 11);
        lblCreatedBy.Margin = new Padding(3);
        lblCreatedBy.Name = "lblCreatedBy";
        lblCreatedBy.Size = new Size(159, 26);
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
        lblCreatedAt.Location = new Point(841, 11);
        lblCreatedAt.Margin = new Padding(3);
        lblCreatedAt.Name = "lblCreatedAt";
        lblCreatedAt.Size = new Size(183, 26);
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
        lblModifiedBy.Location = new Point(676, 11);
        lblModifiedBy.Margin = new Padding(3);
        lblModifiedBy.Name = "lblModifiedBy";
        lblModifiedBy.Size = new Size(159, 26);
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
        lblModifiedAt.Location = new Point(487, 11);
        lblModifiedAt.Margin = new Padding(3);
        lblModifiedAt.Name = "lblModifiedAt";
        lblModifiedAt.Size = new Size(183, 26);
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
        lblEditCount.Location = new Point(345, 11);
        lblEditCount.Margin = new Padding(3);
        lblEditCount.Name = "lblEditCount";
        lblEditCount.Size = new Size(136, 26);
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
        lblLastPrintedAt.Location = new Point(156, 11);
        lblLastPrintedAt.Margin = new Padding(3);
        lblLastPrintedAt.Name = "lblLastPrintedAt";
        lblLastPrintedAt.Size = new Size(183, 26);
        lblLastPrintedAt.TabIndex = 5;
        lblLastPrintedAt.Text = "آخر طباعة: —";
        lblLastPrintedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblPrintCount
        // 
        lblPrintCount.AutoEllipsis = true;
        lblPrintCount.BackColor = Color.FromArgb(192, 255, 255);
        lblPrintCount.Dock = DockStyle.Fill;
        lblPrintCount.Font = new Font("Tahoma", 9F);
        lblPrintCount.Location = new Point(11, 11);
        lblPrintCount.Margin = new Padding(3);
        lblPrintCount.Name = "lblPrintCount";
        lblPrintCount.Size = new Size(139, 26);
        lblPrintCount.TabIndex = 6;
        lblPrintCount.Text = "عدد مرات الطباعة: —";
        lblPrintCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // tabMain
        // 
        tabMain.Controls.Add(tabCurrencies);
        tabMain.Controls.Add(tabCurrencyRoles);
        tabMain.Controls.Add(tpNotes);
        tabMain.Dock = DockStyle.Fill;
        tabMain.Font = new Font("Tahoma", 9F);
        tabMain.Location = new Point(3, 51);
        tabMain.Name = "tabMain";
        tabMain.RightToLeftLayout = true;
        tabMain.SelectedIndex = 0;
        tabMain.Size = new Size(1194, 770);
        tabMain.TabIndex = 3;
        // 
        // tabCurrencies
        // 
        tabCurrencies.AutoScroll = true;
        tabCurrencies.BackColor = Color.LightCyan;
        tabCurrencies.Controls.Add(Currencies);
        tabCurrencies.Location = new Point(4, 29);
        tabCurrencies.Name = "tabCurrencies";
        tabCurrencies.Padding = new Padding(12);
        tabCurrencies.Size = new Size(1186, 737);
        tabCurrencies.TabIndex = 0;
        tabCurrencies.Text = "تعريف العملات";
        // 
        // Currencies
        // 
        Currencies.ColumnCount = 1;
        Currencies.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        Currencies.Controls.Add(CurrenciesFields, 0, 0);
        Currencies.Controls.Add(dgvCurrencies, 0, 1);
        Currencies.Dock = DockStyle.Fill;
        Currencies.Location = new Point(12, 12);
        Currencies.Name = "Currencies";
        Currencies.RowCount = 2;
        Currencies.RowStyles.Add(new RowStyle());
        Currencies.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        Currencies.Size = new Size(1162, 713);
        Currencies.TabIndex = 0;
        // 
        // CurrenciesFields
        // 
        CurrenciesFields.AutoSize = true;
        CurrenciesFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        CurrenciesFields.ColumnCount = 4;
        CurrenciesFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        CurrenciesFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        CurrenciesFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        CurrenciesFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        CurrenciesFields.Controls.Add(lbltxtCurrencyCode, 0, 0);
        CurrenciesFields.Controls.Add(txtCurrencyCode, 1, 0);
        CurrenciesFields.Controls.Add(lbltxtCurrencyNameAr, 2, 0);
        CurrenciesFields.Controls.Add(txtCurrencyNameAr, 3, 0);
        CurrenciesFields.Controls.Add(lbltxtCurrencyNameEn, 0, 1);
        CurrenciesFields.Controls.Add(txtCurrencyNameEn, 1, 1);
        CurrenciesFields.Controls.Add(lbltxtCurrencySymbol, 2, 1);
        CurrenciesFields.Controls.Add(txtCurrencySymbol, 3, 1);
        CurrenciesFields.Controls.Add(lblnudDecimalPlaces, 0, 2);
        CurrenciesFields.Controls.Add(nudDecimalPlaces, 1, 2);
        CurrenciesFields.Controls.Add(lblchkCurrencyActive, 2, 2);
        CurrenciesFields.Controls.Add(chkCurrencyActive, 3, 2);
        CurrenciesFields.Dock = DockStyle.Top;
        CurrenciesFields.Location = new Point(3, 3);
        CurrenciesFields.Name = "CurrenciesFields";
        CurrenciesFields.Padding = new Padding(4);
        CurrenciesFields.RowCount = 3;
        CurrenciesFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        CurrenciesFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        CurrenciesFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        CurrenciesFields.Size = new Size(1156, 128);
        CurrenciesFields.TabIndex = 0;
        // 
        // lbltxtCurrencyCode
        // 
        lbltxtCurrencyCode.Dock = DockStyle.Fill;
        lbltxtCurrencyCode.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lbltxtCurrencyCode.ForeColor = Color.FromArgb(180, 35, 24);
        lbltxtCurrencyCode.Location = new Point(995, 7);
        lbltxtCurrencyCode.Margin = new Padding(3);
        lbltxtCurrencyCode.Name = "lbltxtCurrencyCode";
        lbltxtCurrencyCode.Size = new Size(154, 34);
        lbltxtCurrencyCode.TabIndex = 0;
        lbltxtCurrencyCode.Text = "رمز العملة *";
        lbltxtCurrencyCode.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtCurrencyCode
        // 
        txtCurrencyCode.AccessibleName = "رمز العملة";
        txtCurrencyCode.BackColor = Color.FromArgb(255, 249, 219);
        txtCurrencyCode.CharacterCasing = CharacterCasing.Upper;
        txtCurrencyCode.Dock = DockStyle.Fill;
        txtCurrencyCode.Font = new Font("Tahoma", 9F);
        txtCurrencyCode.ForeColor = Color.FromArgb(16, 24, 40);
        txtCurrencyCode.Location = new Point(581, 7);
        txtCurrencyCode.MaxLength = 12;
        txtCurrencyCode.Name = "txtCurrencyCode";
        txtCurrencyCode.Size = new Size(408, 32);
        txtCurrencyCode.TabIndex = 0;
        // 
        // lbltxtCurrencyNameAr
        // 
        lbltxtCurrencyNameAr.Dock = DockStyle.Fill;
        lbltxtCurrencyNameAr.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lbltxtCurrencyNameAr.ForeColor = Color.FromArgb(180, 35, 24);
        lbltxtCurrencyNameAr.Location = new Point(421, 7);
        lbltxtCurrencyNameAr.Margin = new Padding(3);
        lbltxtCurrencyNameAr.Name = "lbltxtCurrencyNameAr";
        lbltxtCurrencyNameAr.Size = new Size(154, 34);
        lbltxtCurrencyNameAr.TabIndex = 1;
        lbltxtCurrencyNameAr.Text = "اسم العملة بالعربي *";
        lbltxtCurrencyNameAr.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtCurrencyNameAr
        // 
        txtCurrencyNameAr.AccessibleName = "اسم العملة بالعربي";
        txtCurrencyNameAr.BackColor = Color.FromArgb(255, 249, 219);
        txtCurrencyNameAr.Dock = DockStyle.Fill;
        txtCurrencyNameAr.Font = new Font("Tahoma", 9F);
        txtCurrencyNameAr.ForeColor = Color.FromArgb(16, 24, 40);
        txtCurrencyNameAr.Location = new Point(7, 7);
        txtCurrencyNameAr.MaxLength = 100;
        txtCurrencyNameAr.Name = "txtCurrencyNameAr";
        txtCurrencyNameAr.Size = new Size(408, 32);
        txtCurrencyNameAr.TabIndex = 1;
        // 
        // lbltxtCurrencyNameEn
        // 
        lbltxtCurrencyNameEn.Dock = DockStyle.Fill;
        lbltxtCurrencyNameEn.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lbltxtCurrencyNameEn.ForeColor = Color.FromArgb(16, 24, 40);
        lbltxtCurrencyNameEn.Location = new Point(995, 47);
        lbltxtCurrencyNameEn.Margin = new Padding(3);
        lbltxtCurrencyNameEn.Name = "lbltxtCurrencyNameEn";
        lbltxtCurrencyNameEn.Size = new Size(154, 34);
        lbltxtCurrencyNameEn.TabIndex = 2;
        lbltxtCurrencyNameEn.Text = "اسم العملة بالإنجليزي";
        lbltxtCurrencyNameEn.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtCurrencyNameEn
        // 
        txtCurrencyNameEn.AccessibleName = "اسم العملة بالإنجليزي";
        txtCurrencyNameEn.Dock = DockStyle.Fill;
        txtCurrencyNameEn.Font = new Font("Tahoma", 9F);
        txtCurrencyNameEn.ForeColor = Color.FromArgb(16, 24, 40);
        txtCurrencyNameEn.Location = new Point(581, 47);
        txtCurrencyNameEn.MaxLength = 100;
        txtCurrencyNameEn.Name = "txtCurrencyNameEn";
        txtCurrencyNameEn.Size = new Size(408, 32);
        txtCurrencyNameEn.TabIndex = 2;
        // 
        // lbltxtCurrencySymbol
        // 
        lbltxtCurrencySymbol.Dock = DockStyle.Fill;
        lbltxtCurrencySymbol.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lbltxtCurrencySymbol.ForeColor = Color.FromArgb(16, 24, 40);
        lbltxtCurrencySymbol.Location = new Point(421, 47);
        lbltxtCurrencySymbol.Margin = new Padding(3);
        lbltxtCurrencySymbol.Name = "lbltxtCurrencySymbol";
        lbltxtCurrencySymbol.Size = new Size(154, 34);
        lbltxtCurrencySymbol.TabIndex = 3;
        lbltxtCurrencySymbol.Text = "رمز العرض";
        lbltxtCurrencySymbol.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtCurrencySymbol
        // 
        txtCurrencySymbol.AccessibleName = "رمز العرض";
        txtCurrencySymbol.Dock = DockStyle.Fill;
        txtCurrencySymbol.Font = new Font("Tahoma", 9F);
        txtCurrencySymbol.ForeColor = Color.FromArgb(16, 24, 40);
        txtCurrencySymbol.Location = new Point(7, 47);
        txtCurrencySymbol.MaxLength = 12;
        txtCurrencySymbol.Name = "txtCurrencySymbol";
        txtCurrencySymbol.Size = new Size(408, 32);
        txtCurrencySymbol.TabIndex = 3;
        // 
        // lblnudDecimalPlaces
        // 
        lblnudDecimalPlaces.Dock = DockStyle.Fill;
        lblnudDecimalPlaces.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblnudDecimalPlaces.ForeColor = Color.FromArgb(180, 35, 24);
        lblnudDecimalPlaces.Location = new Point(995, 87);
        lblnudDecimalPlaces.Margin = new Padding(3);
        lblnudDecimalPlaces.Name = "lblnudDecimalPlaces";
        lblnudDecimalPlaces.Size = new Size(154, 34);
        lblnudDecimalPlaces.TabIndex = 4;
        lblnudDecimalPlaces.Text = "المنازل العشرية *";
        lblnudDecimalPlaces.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // nudDecimalPlaces
        // 
        nudDecimalPlaces.AccessibleName = "المنازل العشرية";
        nudDecimalPlaces.BackColor = Color.FromArgb(255, 249, 219);
        nudDecimalPlaces.Dock = DockStyle.Fill;
        nudDecimalPlaces.Font = new Font("Tahoma", 9F);
        nudDecimalPlaces.ForeColor = Color.FromArgb(16, 24, 40);
        nudDecimalPlaces.Location = new Point(581, 87);
        nudDecimalPlaces.Maximum = new decimal(new int[] { 6, 0, 0, 0 });
        nudDecimalPlaces.Name = "nudDecimalPlaces";
        nudDecimalPlaces.Size = new Size(408, 32);
        nudDecimalPlaces.TabIndex = 4;
        nudDecimalPlaces.Value = new decimal(new int[] { 2, 0, 0, 0 });
        // 
        // lblchkCurrencyActive
        // 
        lblchkCurrencyActive.Dock = DockStyle.Fill;
        lblchkCurrencyActive.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblchkCurrencyActive.ForeColor = Color.FromArgb(16, 24, 40);
        lblchkCurrencyActive.Location = new Point(421, 87);
        lblchkCurrencyActive.Margin = new Padding(3);
        lblchkCurrencyActive.Name = "lblchkCurrencyActive";
        lblchkCurrencyActive.Size = new Size(154, 34);
        lblchkCurrencyActive.TabIndex = 5;
        lblchkCurrencyActive.Text = "نشطة";
        lblchkCurrencyActive.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // chkCurrencyActive
        // 
        chkCurrencyActive.AccessibleName = "نشطة";
        chkCurrencyActive.AutoSize = true;
        chkCurrencyActive.Dock = DockStyle.Fill;
        chkCurrencyActive.Location = new Point(7, 87);
        chkCurrencyActive.Name = "chkCurrencyActive";
        chkCurrencyActive.Size = new Size(408, 34);
        chkCurrencyActive.TabIndex = 5;
        // 
        // dgvCurrencies
        // 
        dgvCurrencies.AccessibleName = "قائمة البيانات — لم يتم تحميل بيانات";
        dgvCurrencies.AllowUserToAddRows = false;
        dgvCurrencies.AllowUserToDeleteRows = false;
        dgvCurrencies.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvCurrencies.BackgroundColor = Color.White;
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(224, 224, 224);
        dataGridViewCellStyle1.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
        dgvCurrencies.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        dgvCurrencies.ColumnHeadersHeight = 29;
        dgvCurrencies.Columns.AddRange(new DataGridViewColumn[] { dgvCurrenciesColCode, dgvCurrenciesColNameAr, dgvCurrenciesColNameEn, dgvCurrenciesColSymbol, dgvCurrenciesColDecimals, dgvCurrenciesColActive });
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = SystemColors.Window;
        dataGridViewCellStyle2.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
        dgvCurrencies.DefaultCellStyle = dataGridViewCellStyle2;
        dgvCurrencies.Dock = DockStyle.Fill;
        dgvCurrencies.EnableHeadersVisualStyles = false;
        dgvCurrencies.Location = new Point(3, 137);
        dgvCurrencies.MinimumSize = new Size(0, 160);
        dgvCurrencies.MultiSelect = false;
        dgvCurrencies.Name = "dgvCurrencies";
        dgvCurrencies.ReadOnly = true;
        dgvCurrencies.RowHeadersVisible = false;
        dgvCurrencies.RowHeadersWidth = 51;
        dgvCurrencies.RowTemplate.Height = 30;
        dgvCurrencies.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCurrencies.Size = new Size(1156, 573);
        dgvCurrencies.TabIndex = 1;
        // 
        // dgvCurrenciesColCode
        // 
        dgvCurrenciesColCode.HeaderText = "رمز العملة";
        dgvCurrenciesColCode.MinimumWidth = 90;
        dgvCurrenciesColCode.Name = "dgvCurrenciesColCode";
        dgvCurrenciesColCode.ReadOnly = true;
        dgvCurrenciesColCode.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // dgvCurrenciesColNameAr
        // 
        dgvCurrenciesColNameAr.HeaderText = "الاسم العربي";
        dgvCurrenciesColNameAr.MinimumWidth = 90;
        dgvCurrenciesColNameAr.Name = "dgvCurrenciesColNameAr";
        dgvCurrenciesColNameAr.ReadOnly = true;
        dgvCurrenciesColNameAr.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // dgvCurrenciesColNameEn
        // 
        dgvCurrenciesColNameEn.HeaderText = "الاسم الإنجليزي";
        dgvCurrenciesColNameEn.MinimumWidth = 90;
        dgvCurrenciesColNameEn.Name = "dgvCurrenciesColNameEn";
        dgvCurrenciesColNameEn.ReadOnly = true;
        dgvCurrenciesColNameEn.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // dgvCurrenciesColSymbol
        // 
        dgvCurrenciesColSymbol.HeaderText = "رمز العرض";
        dgvCurrenciesColSymbol.MinimumWidth = 90;
        dgvCurrenciesColSymbol.Name = "dgvCurrenciesColSymbol";
        dgvCurrenciesColSymbol.ReadOnly = true;
        dgvCurrenciesColSymbol.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // dgvCurrenciesColDecimals
        // 
        dgvCurrenciesColDecimals.HeaderText = "المنازل العشرية";
        dgvCurrenciesColDecimals.MinimumWidth = 90;
        dgvCurrenciesColDecimals.Name = "dgvCurrenciesColDecimals";
        dgvCurrenciesColDecimals.ReadOnly = true;
        dgvCurrenciesColDecimals.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // dgvCurrenciesColActive
        // 
        dgvCurrenciesColActive.HeaderText = "الحالة";
        dgvCurrenciesColActive.MinimumWidth = 90;
        dgvCurrenciesColActive.Name = "dgvCurrenciesColActive";
        dgvCurrenciesColActive.ReadOnly = true;
        dgvCurrenciesColActive.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // tabCurrencyRoles
        // 
        tabCurrencyRoles.AutoScroll = true;
        tabCurrencyRoles.BackColor = Color.LightCyan;
        tabCurrencyRoles.Controls.Add(rolesLayout);
        tabCurrencyRoles.Location = new Point(4, 29);
        tabCurrencyRoles.Name = "tabCurrencyRoles";
        tabCurrencyRoles.Padding = new Padding(12);
        tabCurrencyRoles.Size = new Size(1186, 737);
        tabCurrencyRoles.TabIndex = 1;
        tabCurrencyRoles.Text = "إعدادات عملات الشركة";
        // 
        // rolesLayout
        // 
        rolesLayout.AutoSize = true;
        rolesLayout.ColumnCount = 1;
        rolesLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rolesLayout.Controls.Add(CurrencyRoles, 0, 0);
        rolesLayout.Controls.Add(grpAllowedCurrencies, 0, 1);
        rolesLayout.Dock = DockStyle.Top;
        rolesLayout.Location = new Point(12, 12);
        rolesLayout.Name = "rolesLayout";
        rolesLayout.RowCount = 2;
        rolesLayout.RowStyles.Add(new RowStyle());
        rolesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 180F));
        rolesLayout.Size = new Size(1162, 274);
        rolesLayout.TabIndex = 0;
        // 
        // CurrencyRoles
        // 
        CurrencyRoles.AutoSize = true;
        CurrencyRoles.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        CurrencyRoles.ColumnCount = 4;
        CurrencyRoles.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        CurrencyRoles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        CurrencyRoles.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        CurrencyRoles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        CurrencyRoles.Controls.Add(lblcmbCompany, 0, 0);
        CurrencyRoles.Controls.Add(cmbCompany, 1, 0);
        CurrencyRoles.Controls.Add(lblcmbAccountingCurrency, 2, 0);
        CurrencyRoles.Controls.Add(cmbAccountingCurrency, 3, 0);
        CurrencyRoles.Controls.Add(lblcmbReportingCurrency, 0, 1);
        CurrencyRoles.Controls.Add(cmbReportingCurrency, 1, 1);
        CurrencyRoles.Controls.Add(lblcmbDefaultTransactionCurrency, 2, 1);
        CurrencyRoles.Controls.Add(cmbDefaultTransactionCurrency, 3, 1);
        CurrencyRoles.Dock = DockStyle.Top;
        CurrencyRoles.Location = new Point(3, 3);
        CurrencyRoles.Name = "CurrencyRoles";
        CurrencyRoles.Padding = new Padding(4);
        CurrencyRoles.RowCount = 2;
        CurrencyRoles.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        CurrencyRoles.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        CurrencyRoles.Size = new Size(1156, 88);
        CurrencyRoles.TabIndex = 0;
        // 
        // lblcmbCompany
        // 
        lblcmbCompany.Dock = DockStyle.Fill;
        lblcmbCompany.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblcmbCompany.ForeColor = Color.FromArgb(180, 35, 24);
        lblcmbCompany.Location = new Point(995, 7);
        lblcmbCompany.Margin = new Padding(3);
        lblcmbCompany.Name = "lblcmbCompany";
        lblcmbCompany.Size = new Size(154, 34);
        lblcmbCompany.TabIndex = 0;
        lblcmbCompany.Text = "الشركة *";
        lblcmbCompany.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cmbCompany
        // 
        cmbCompany.AccessibleName = "الشركة";
        cmbCompany.BackColor = Color.FromArgb(255, 249, 219);
        cmbCompany.Dock = DockStyle.Fill;
        cmbCompany.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCompany.Font = new Font("Tahoma", 9F);
        cmbCompany.ForeColor = Color.FromArgb(16, 24, 40);
        cmbCompany.Location = new Point(581, 7);
        cmbCompany.Name = "cmbCompany";
        cmbCompany.Size = new Size(408, 33);
        cmbCompany.TabIndex = 0;
        // 
        // lblcmbAccountingCurrency
        // 
        lblcmbAccountingCurrency.Dock = DockStyle.Fill;
        lblcmbAccountingCurrency.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblcmbAccountingCurrency.ForeColor = Color.FromArgb(180, 35, 24);
        lblcmbAccountingCurrency.Location = new Point(421, 7);
        lblcmbAccountingCurrency.Margin = new Padding(3);
        lblcmbAccountingCurrency.Name = "lblcmbAccountingCurrency";
        lblcmbAccountingCurrency.Size = new Size(154, 34);
        lblcmbAccountingCurrency.TabIndex = 1;
        lblcmbAccountingCurrency.Text = "العملة المحاسبية *";
        lblcmbAccountingCurrency.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cmbAccountingCurrency
        // 
        cmbAccountingCurrency.AccessibleName = "العملة المحاسبية";
        cmbAccountingCurrency.BackColor = Color.FromArgb(255, 249, 219);
        cmbAccountingCurrency.Dock = DockStyle.Fill;
        cmbAccountingCurrency.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbAccountingCurrency.Font = new Font("Tahoma", 9F);
        cmbAccountingCurrency.ForeColor = Color.FromArgb(16, 24, 40);
        cmbAccountingCurrency.Location = new Point(7, 7);
        cmbAccountingCurrency.Name = "cmbAccountingCurrency";
        cmbAccountingCurrency.Size = new Size(408, 33);
        cmbAccountingCurrency.TabIndex = 1;
        // 
        // lblcmbReportingCurrency
        // 
        lblcmbReportingCurrency.Dock = DockStyle.Fill;
        lblcmbReportingCurrency.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblcmbReportingCurrency.ForeColor = Color.FromArgb(16, 24, 40);
        lblcmbReportingCurrency.Location = new Point(995, 47);
        lblcmbReportingCurrency.Margin = new Padding(3);
        lblcmbReportingCurrency.Name = "lblcmbReportingCurrency";
        lblcmbReportingCurrency.Size = new Size(154, 34);
        lblcmbReportingCurrency.TabIndex = 2;
        lblcmbReportingCurrency.Text = "عملة التقارير";
        lblcmbReportingCurrency.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cmbReportingCurrency
        // 
        cmbReportingCurrency.AccessibleName = "عملة التقارير";
        cmbReportingCurrency.Dock = DockStyle.Fill;
        cmbReportingCurrency.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbReportingCurrency.Font = new Font("Tahoma", 9F);
        cmbReportingCurrency.ForeColor = Color.FromArgb(16, 24, 40);
        cmbReportingCurrency.Location = new Point(581, 47);
        cmbReportingCurrency.Name = "cmbReportingCurrency";
        cmbReportingCurrency.Size = new Size(408, 33);
        cmbReportingCurrency.TabIndex = 2;
        // 
        // lblcmbDefaultTransactionCurrency
        // 
        lblcmbDefaultTransactionCurrency.Dock = DockStyle.Fill;
        lblcmbDefaultTransactionCurrency.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblcmbDefaultTransactionCurrency.ForeColor = Color.FromArgb(16, 24, 40);
        lblcmbDefaultTransactionCurrency.Location = new Point(421, 47);
        lblcmbDefaultTransactionCurrency.Margin = new Padding(3);
        lblcmbDefaultTransactionCurrency.Name = "lblcmbDefaultTransactionCurrency";
        lblcmbDefaultTransactionCurrency.Size = new Size(154, 34);
        lblcmbDefaultTransactionCurrency.TabIndex = 3;
        lblcmbDefaultTransactionCurrency.Text = "عملة المعاملة الافتراضية";
        lblcmbDefaultTransactionCurrency.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cmbDefaultTransactionCurrency
        // 
        cmbDefaultTransactionCurrency.AccessibleName = "عملة المعاملة الافتراضية";
        cmbDefaultTransactionCurrency.Dock = DockStyle.Fill;
        cmbDefaultTransactionCurrency.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDefaultTransactionCurrency.Font = new Font("Tahoma", 9F);
        cmbDefaultTransactionCurrency.ForeColor = Color.FromArgb(16, 24, 40);
        cmbDefaultTransactionCurrency.Location = new Point(7, 47);
        cmbDefaultTransactionCurrency.Name = "cmbDefaultTransactionCurrency";
        cmbDefaultTransactionCurrency.Size = new Size(408, 33);
        cmbDefaultTransactionCurrency.TabIndex = 3;
        // 
        // grpAllowedCurrencies
        // 
        grpAllowedCurrencies.Controls.Add(SettingValue__SET_CUR_001);
        grpAllowedCurrencies.Dock = DockStyle.Fill;
        grpAllowedCurrencies.Location = new Point(3, 97);
        grpAllowedCurrencies.Name = "grpAllowedCurrencies";
        grpAllowedCurrencies.Padding = new Padding(12);
        grpAllowedCurrencies.Size = new Size(1156, 174);
        grpAllowedCurrencies.TabIndex = 1;
        grpAllowedCurrencies.TabStop = false;
        grpAllowedCurrencies.Text = "العملات المسموح بها للشركة";
        // 
        // SettingValue__SET_CUR_001
        // 
        SettingValue__SET_CUR_001.AccessibleName = "العملات المسموح بها للشركة";
        SettingValue__SET_CUR_001.CheckOnClick = true;
        SettingValue__SET_CUR_001.Dock = DockStyle.Fill;
        SettingValue__SET_CUR_001.IntegralHeight = false;
        SettingValue__SET_CUR_001.Location = new Point(12, 32);
        SettingValue__SET_CUR_001.Name = "SettingValue__SET_CUR_001";
        SettingValue__SET_CUR_001.Size = new Size(1132, 130);
        SettingValue__SET_CUR_001.TabIndex = 0;
        // 
        // tpNotes
        // 
        tpNotes.AutoScroll = true;
        tpNotes.BackColor = Color.LightCyan;
        tpNotes.Controls.Add(txtNotes);
        tpNotes.Location = new Point(4, 29);
        tpNotes.Name = "tpNotes";
        tpNotes.Padding = new Padding(12);
        tpNotes.Size = new Size(1186, 737);
        tpNotes.TabIndex = 2;
        tpNotes.Text = "الملاحظات";
        // 
        // txtNotes
        // 
        txtNotes.AccessibleName = "الملاحظات";
        txtNotes.Dock = DockStyle.Fill;
        txtNotes.Font = new Font("Tahoma", 9F);
        txtNotes.ForeColor = Color.FromArgb(16, 24, 40);
        txtNotes.Location = new Point(12, 12);
        txtNotes.MaxLength = 4000;
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(1162, 713);
        txtNotes.TabIndex = 0;
        // 
        // lblPreview
        // 
        lblPreview.BackColor = Color.FromArgb(248, 250, 252);
        lblPreview.Dock = DockStyle.Fill;
        lblPreview.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblPreview.ForeColor = Color.DimGray;
        lblPreview.Location = new Point(3, 827);
        lblPreview.Margin = new Padding(3);
        lblPreview.Name = "lblPreview";
        lblPreview.Padding = new Padding(8, 0, 8, 0);
        lblPreview.Size = new Size(1194, 22);
        lblPreview.TabIndex = 2;
        lblPreview.Text = "وضع المعاينة — التغييرات غير محفوظة";
        lblPreview.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // UcCurrencyManagement
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(248, 250, 252);
        Controls.Add(mainLayout);
        Font = new Font("Tahoma", 9F);
        MinimumSize = new Size(936, 600);
        Name = "UcCurrencyManagement";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1200, 900);
        Tag = "02.04.02";
        mainLayout.ResumeLayout(false);
        mainLayout.PerformLayout();
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlActions.ResumeLayout(false);
        pnlActions.PerformLayout();
        tlpAuditInfo.ResumeLayout(false);
        tabMain.ResumeLayout(false);
        tabCurrencies.ResumeLayout(false);
        Currencies.ResumeLayout(false);
        Currencies.PerformLayout();
        CurrenciesFields.ResumeLayout(false);
        CurrenciesFields.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudDecimalPlaces).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvCurrencies).EndInit();
        tabCurrencyRoles.ResumeLayout(false);
        tabCurrencyRoles.PerformLayout();
        rolesLayout.ResumeLayout(false);
        rolesLayout.PerformLayout();
        CurrencyRoles.ResumeLayout(false);
        grpAllowedCurrencies.ResumeLayout(false);
        tpNotes.ResumeLayout(false);
        tpNotes.PerformLayout();
        ResumeLayout(false);
    
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(0, 0);
        designerCommandBar.Size = new Size(1013, 86);
        designerCommandBar.Margin = new Padding(0);
        designerCommandBar.MinimumSize = new Size(0, 48);
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
        designerCommandBar.SetCommandRole(btnDelete, TransportERP.Desktop.CoreUI.DesignerCommandRole.Delete);
        designerCommandBar.SetCommandRole(btnUndo, TransportERP.Desktop.CoreUI.DesignerCommandRole.Cancel);
        designerCommandBar.SetCommandRole(btnSearch, TransportERP.Desktop.CoreUI.DesignerCommandRole.View);
        designerCommandBar.SetCommandRole(btnLast, TransportERP.Desktop.CoreUI.DesignerCommandRole.Last);
        designerCommandBar.SetCommandRole(btnNext, TransportERP.Desktop.CoreUI.DesignerCommandRole.Next);
        designerCommandBar.SetCommandRole(btnPrevious, TransportERP.Desktop.CoreUI.DesignerCommandRole.Previous);
        designerCommandBar.SetCommandRole(btnFirst, TransportERP.Desktop.CoreUI.DesignerCommandRole.First);
        designerCommandBar.SetCommandRole(btnSave, TransportERP.Desktop.CoreUI.DesignerCommandRole.Save);
        designerCommandBar.SetCommandRole(btnPrint, TransportERP.Desktop.CoreUI.DesignerCommandRole.Print);
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

    private Panel pnlHeader = null!;
    private Label lblTitle = null!;
    private TableLayoutPanel tlpAuditInfo = null!;
    private TextBox txtCurrentRecordNo = null!;
    private Button btnFirst = null!;
    private Button btnPrevious = null!;
    private Button btnNext = null!;
    private Button btnLast = null!;
    private Button btnUndo = null!;
    private Button btnSearch = null!;
    private Button btnPrint = null!;
    private Button btnApprove = null!;
    private Label lblCreatedBy = null!;
    private Label lblCreatedAt = null!;
    private Label lblModifiedBy = null!;
    private Label lblModifiedAt = null!;
    private Label lblEditCount = null!;
    private Label lblLastPrintedAt = null!;
    private Label lblPrintCount = null!;
    private TableLayoutPanel mainLayout = null!;
    private FlowLayoutPanel pnlActions = null!;
    private Button btnNew = null!;
    private Button btnSave = null!;
    private Button btnEdit = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Button btnClose = null!;
    private Label lblPreview = null!;
    private TabControl tabMain = null!;
    private TabPage tabCurrencies = null!;
    private TableLayoutPanel Currencies = null!;
    private TableLayoutPanel CurrenciesFields = null!;
    private Label lbltxtCurrencyCode = null!;
    private TextBox txtCurrencyCode = null!;
    private Label lbltxtCurrencyNameAr = null!;
    private TextBox txtCurrencyNameAr = null!;
    private Label lbltxtCurrencyNameEn = null!;
    private TextBox txtCurrencyNameEn = null!;
    private Label lbltxtCurrencySymbol = null!;
    private TextBox txtCurrencySymbol = null!;
    private Label lblnudDecimalPlaces = null!;
    private NumericUpDown nudDecimalPlaces = null!;
    private Label lblchkCurrencyActive = null!;
    private CheckBox chkCurrencyActive = null!;
    private DataGridView dgvCurrencies = null!;
    private DataGridViewTextBoxColumn dgvCurrenciesColCode = null!;
    private DataGridViewTextBoxColumn dgvCurrenciesColNameAr = null!;
    private DataGridViewTextBoxColumn dgvCurrenciesColNameEn = null!;
    private DataGridViewTextBoxColumn dgvCurrenciesColSymbol = null!;
    private DataGridViewTextBoxColumn dgvCurrenciesColDecimals = null!;
    private DataGridViewTextBoxColumn dgvCurrenciesColActive = null!;
    private TabPage tabCurrencyRoles = null!;
    private TableLayoutPanel rolesLayout = null!;
    private TableLayoutPanel CurrencyRoles = null!;
    private Label lblcmbCompany = null!;
    private ComboBox cmbCompany = null!;
    private Label lblcmbAccountingCurrency = null!;
    private ComboBox cmbAccountingCurrency = null!;
    private Label lblcmbReportingCurrency = null!;
    private ComboBox cmbReportingCurrency = null!;
    private Label lblcmbDefaultTransactionCurrency = null!;
    private ComboBox cmbDefaultTransactionCurrency = null!;
    private CheckedListBox SettingValue__SET_CUR_001 = null!;
    private GroupBox grpAllowedCurrencies = null!;
    private TabPage tpNotes = null!;
    private TextBox txtNotes = null!;
}
