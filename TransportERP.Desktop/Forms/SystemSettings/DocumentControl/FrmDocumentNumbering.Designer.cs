#nullable enable

using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.DocumentControl;

partial class FrmDocumentNumbering
{
    private System.ComponentModel.IContainer? components;
    private Panel pnlHeader = null!;
    private Label lblTitle = null!;
    private Label lblSubtitle = null!;
    private Panel pnlFilters = null!;
    private TableLayoutPanel tlpFilters = null!;
    private Label lblCompany = null!;
    private ComboBox cboCompany = null!;
    private Label lblBranch = null!;
    private ComboBox cboBranch = null!;
    private Label lblFiscalYear = null!;
    private ComboBox cboFiscalYear = null!;
    private SplitContainer splitMain = null!;
    private DataGridView dgvDocumentSequences = null!;
    private DataGridViewTextBoxColumn colDocumentType = null!;
    private DataGridViewTextBoxColumn colPrefix = null!;
    private DataGridViewTextBoxColumn colLastNumber = null!;
    private DataGridViewTextBoxColumn colNextNumber = null!;
    private DataGridViewTextBoxColumn colDigits = null!;
    private DataGridViewTextBoxColumn colResetType = null!;
    private DataGridViewCheckBoxColumn colActive = null!;
    private GroupBox grpDetails = null!;
    private TableLayoutPanel tlpDetails = null!;
    private Label lblDocumentType = null!;
    private ComboBox cboDocumentType = null!;
    private Label lblPrefix = null!;
    private TextBox txtPrefix = null!;
    private Label lblLastNumber = null!;
    private NumericUpDown nudLastNumber = null!;
    private Label lblNextNumber = null!;
    private NumericUpDown nudNextNumber = null!;
    private Label lblDigits = null!;
    private NumericUpDown nudDigits = null!;
    private Label lblResetType = null!;
    private ComboBox cboResetType = null!;
    private CheckBox chkActive = null!;
    private Label lblPreviewCaption = null!;
    private Label lblNumberPreview = null!;
    private Panel pnlActions = null!;
    private FlowLayoutPanel flpActions = null!;
    private Button btnNew = null!;
    private Button btnSave = null!;
    private Button btnEdit = null!;
    private Button btnDisable = null!;
    private Button btnPrint = null!;
    private Button btnClose = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlHeader = new Panel();
        lblSubtitle = new Label();
        lblTitle = new Label();
        pnlFilters = new Panel();
        tlpFilters = new TableLayoutPanel();
        lblCompany = new Label();
        cboCompany = new ComboBox();
        lblBranch = new Label();
        cboBranch = new ComboBox();
        lblFiscalYear = new Label();
        cboFiscalYear = new ComboBox();
        splitMain = new SplitContainer();
        dgvDocumentSequences = new DataGridView();
        colDocumentType = new DataGridViewTextBoxColumn();
        colPrefix = new DataGridViewTextBoxColumn();
        colLastNumber = new DataGridViewTextBoxColumn();
        colNextNumber = new DataGridViewTextBoxColumn();
        colDigits = new DataGridViewTextBoxColumn();
        colResetType = new DataGridViewTextBoxColumn();
        colActive = new DataGridViewCheckBoxColumn();
        grpDetails = new GroupBox();
        tlpDetails = new TableLayoutPanel();
        lblDocumentType = new Label();
        cboDocumentType = new ComboBox();
        lblPrefix = new Label();
        txtPrefix = new TextBox();
        lblLastNumber = new Label();
        nudLastNumber = new NumericUpDown();
        lblNextNumber = new Label();
        nudNextNumber = new NumericUpDown();
        lblDigits = new Label();
        nudDigits = new NumericUpDown();
        lblResetType = new Label();
        cboResetType = new ComboBox();
        chkActive = new CheckBox();
        lblPreviewCaption = new Label();
        lblNumberPreview = new Label();
        pnlActions = new Panel();
        flpActions = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        btnDisable = new Button();
        btnPrint = new Button();
        btnClose = new Button();
        pnlHeader.SuspendLayout();
        pnlFilters.SuspendLayout();
        tlpFilters.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvDocumentSequences).BeginInit();
        grpDetails.SuspendLayout();
        tlpDetails.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudLastNumber).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudNextNumber).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudDigits).BeginInit();
        pnlActions.SuspendLayout();
        flpActions.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.Controls.Add(lblSubtitle);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Margin = new Padding(3, 4, 3, 4);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1240, 88);
        pnlHeader.TabIndex = 0;
        // 
        // lblSubtitle
        // 
        lblSubtitle.Dock = DockStyle.Fill;
        lblSubtitle.Location = new Point(0, 40);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(1240, 48);
        lblSubtitle.TabIndex = 0;
        lblSubtitle.Text = "قواعد الترقيم حسب الشركة والفرع والسنة المالية مع معاينة قبل الحفظ.";
        // 
        // lblTitle
        // 
        lblTitle.Dock = DockStyle.Top;
        lblTitle.Location = new Point(0, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(1240, 40);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "ترقيم المستندات";
        // 
        // pnlFilters
        // 
        pnlFilters.Controls.Add(tlpFilters);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 88);
        pnlFilters.Margin = new Padding(3, 4, 3, 4);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(16);
        pnlFilters.Size = new Size(1240, 80);
        pnlFilters.TabIndex = 1;
        // 
        // tlpFilters
        // 
        tlpFilters.ColumnCount = 6;
        tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 103F));
        tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
        tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 91F));
        tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
        tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 126F));
        tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
        tlpFilters.Controls.Add(lblCompany, 0, 0);
        tlpFilters.Controls.Add(cboCompany, 1, 0);
        tlpFilters.Controls.Add(lblBranch, 2, 0);
        tlpFilters.Controls.Add(cboBranch, 3, 0);
        tlpFilters.Controls.Add(lblFiscalYear, 4, 0);
        tlpFilters.Controls.Add(cboFiscalYear, 5, 0);
        tlpFilters.Dock = DockStyle.Fill;
        tlpFilters.Location = new Point(16, 16);
        tlpFilters.Margin = new Padding(3, 4, 3, 4);
        tlpFilters.Name = "tlpFilters";
        tlpFilters.RowCount = 1;
        tlpFilters.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpFilters.Size = new Size(1208, 48);
        tlpFilters.TabIndex = 0;
        // 
        // lblCompany
        // 
        lblCompany.Dock = DockStyle.Fill;
        lblCompany.Location = new Point(1108, 0);
        lblCompany.Name = "lblCompany";
        lblCompany.Size = new Size(97, 48);
        lblCompany.TabIndex = 0;
        lblCompany.Text = "الشركة *";
        // 
        // cboCompany
        // 
        cboCompany.Dock = DockStyle.Fill;
        cboCompany.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCompany.Location = new Point(807, 4);
        cboCompany.Margin = new Padding(3, 4, 3, 4);
        cboCompany.Name = "cboCompany";
        cboCompany.Size = new Size(295, 31);
        cboCompany.TabIndex = 0;
        // 
        // lblBranch
        // 
        lblBranch.Dock = DockStyle.Fill;
        lblBranch.Location = new Point(716, 0);
        lblBranch.Name = "lblBranch";
        lblBranch.Size = new Size(85, 48);
        lblBranch.TabIndex = 1;
        lblBranch.Text = "الفرع *";
        // 
        // cboBranch
        // 
        cboBranch.Dock = DockStyle.Fill;
        cboBranch.DropDownStyle = ComboBoxStyle.DropDownList;
        cboBranch.Location = new Point(423, 4);
        cboBranch.Margin = new Padding(3, 4, 3, 4);
        cboBranch.Name = "cboBranch";
        cboBranch.Size = new Size(287, 31);
        cboBranch.TabIndex = 1;
        // 
        // lblFiscalYear
        // 
        lblFiscalYear.Dock = DockStyle.Fill;
        lblFiscalYear.Location = new Point(297, 0);
        lblFiscalYear.Name = "lblFiscalYear";
        lblFiscalYear.Size = new Size(120, 48);
        lblFiscalYear.TabIndex = 2;
        lblFiscalYear.Text = "السنة المالية *";
        // 
        // cboFiscalYear
        // 
        cboFiscalYear.Dock = DockStyle.Fill;
        cboFiscalYear.DropDownStyle = ComboBoxStyle.DropDownList;
        cboFiscalYear.Location = new Point(3, 4);
        cboFiscalYear.Margin = new Padding(3, 4, 3, 4);
        cboFiscalYear.Name = "cboFiscalYear";
        cboFiscalYear.Size = new Size(288, 31);
        cboFiscalYear.TabIndex = 2;
        // 
        // splitMain
        // 
        splitMain.Dock = DockStyle.Fill;
        splitMain.FixedPanel = FixedPanel.Panel2;
        splitMain.Location = new Point(0, 168);
        splitMain.Margin = new Padding(3, 4, 3, 4);
        splitMain.Name = "splitMain";
        // 
        // splitMain.Panel1
        // 
        splitMain.Panel1.Controls.Add(dgvDocumentSequences);
        splitMain.Panel1.Padding = new Padding(18, 21, 18, 21);
        splitMain.Panel1.RightToLeft = RightToLeft.Yes;
        splitMain.Panel1MinSize = 550;
        // 
        // splitMain.Panel2
        // 
        splitMain.Panel2.Controls.Add(grpDetails);
        splitMain.Panel2.Padding = new Padding(18, 21, 18, 21);
        splitMain.Panel2.RightToLeft = RightToLeft.Yes;
        splitMain.Panel2MinSize = 380;
        splitMain.RightToLeft = RightToLeft.Yes;
        splitMain.Size = new Size(1240, 568);
        splitMain.SplitterDistance = 582;
        splitMain.SplitterWidth = 7;
        splitMain.TabIndex = 2;
        // 
        // dgvDocumentSequences
        // 
        dgvDocumentSequences.ColumnHeadersHeight = 29;
        dgvDocumentSequences.Columns.AddRange(new DataGridViewColumn[] { colDocumentType, colPrefix, colLastNumber, colNextNumber, colDigits, colResetType, colActive });
        dgvDocumentSequences.Dock = DockStyle.Fill;
        dgvDocumentSequences.Location = new Point(18, 21);
        dgvDocumentSequences.Margin = new Padding(3, 4, 3, 4);
        dgvDocumentSequences.Name = "dgvDocumentSequences";
        dgvDocumentSequences.RowHeadersWidth = 51;
        dgvDocumentSequences.Size = new Size(546, 526);
        dgvDocumentSequences.TabIndex = 0;
        // 
        // colDocumentType
        // 
        colDocumentType.FillWeight = 145F;
        colDocumentType.HeaderText = "نوع المستند";
        colDocumentType.MinimumWidth = 6;
        colDocumentType.Name = "colDocumentType";
        colDocumentType.Width = 125;
        // 
        // colPrefix
        // 
        colPrefix.FillWeight = 70F;
        colPrefix.HeaderText = "البادئة";
        colPrefix.MinimumWidth = 6;
        colPrefix.Name = "colPrefix";
        colPrefix.Width = 125;
        // 
        // colLastNumber
        // 
        colLastNumber.FillWeight = 80F;
        colLastNumber.HeaderText = "آخر رقم";
        colLastNumber.MinimumWidth = 6;
        colLastNumber.Name = "colLastNumber";
        colLastNumber.Width = 125;
        // 
        // colNextNumber
        // 
        colNextNumber.FillWeight = 85F;
        colNextNumber.HeaderText = "الرقم التالي";
        colNextNumber.MinimumWidth = 6;
        colNextNumber.Name = "colNextNumber";
        colNextNumber.Width = 125;
        // 
        // colDigits
        // 
        colDigits.FillWeight = 60F;
        colDigits.HeaderText = "الخانات";
        colDigits.MinimumWidth = 6;
        colDigits.Name = "colDigits";
        colDigits.Width = 125;
        // 
        // colResetType
        // 
        colResetType.FillWeight = 85F;
        colResetType.HeaderText = "إعادة البدء";
        colResetType.MinimumWidth = 6;
        colResetType.Name = "colResetType";
        colResetType.Width = 125;
        // 
        // colActive
        // 
        colActive.FillWeight = 50F;
        colActive.HeaderText = "نشط";
        colActive.MinimumWidth = 6;
        colActive.Name = "colActive";
        colActive.Width = 125;
        // 
        // grpDetails
        // 
        grpDetails.Controls.Add(tlpDetails);
        grpDetails.Dock = DockStyle.Fill;
        grpDetails.Location = new Point(18, 21);
        grpDetails.Margin = new Padding(3, 4, 3, 4);
        grpDetails.Name = "grpDetails";
        grpDetails.Padding = new Padding(3, 4, 3, 4);
        grpDetails.Size = new Size(615, 526);
        grpDetails.TabIndex = 0;
        grpDetails.TabStop = false;
        grpDetails.Text = "تفاصيل قاعدة الترقيم";
        // 
        // tlpDetails
        // 
        tlpDetails.ColumnCount = 2;
        tlpDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 149F));
        tlpDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpDetails.Controls.Add(lblDocumentType, 0, 0);
        tlpDetails.Controls.Add(cboDocumentType, 1, 0);
        tlpDetails.Controls.Add(lblPrefix, 0, 1);
        tlpDetails.Controls.Add(txtPrefix, 1, 1);
        tlpDetails.Controls.Add(lblLastNumber, 0, 2);
        tlpDetails.Controls.Add(nudLastNumber, 1, 2);
        tlpDetails.Controls.Add(lblNextNumber, 0, 3);
        tlpDetails.Controls.Add(nudNextNumber, 1, 3);
        tlpDetails.Controls.Add(lblDigits, 0, 4);
        tlpDetails.Controls.Add(nudDigits, 1, 4);
        tlpDetails.Controls.Add(lblResetType, 0, 5);
        tlpDetails.Controls.Add(cboResetType, 1, 5);
        tlpDetails.Controls.Add(chkActive, 1, 6);
        tlpDetails.Controls.Add(lblPreviewCaption, 0, 7);
        tlpDetails.Controls.Add(lblNumberPreview, 1, 7);
        tlpDetails.Dock = DockStyle.Fill;
        tlpDetails.Location = new Point(3, 27);
        tlpDetails.Margin = new Padding(3, 4, 3, 4);
        tlpDetails.Name = "tlpDetails";
        tlpDetails.RowCount = 8;
        tlpDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tlpDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tlpDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tlpDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tlpDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tlpDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tlpDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tlpDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
        tlpDetails.Size = new Size(609, 495);
        tlpDetails.TabIndex = 0;
        // 
        // lblDocumentType
        // 
        lblDocumentType.Dock = DockStyle.Fill;
        lblDocumentType.Location = new Point(463, 0);
        lblDocumentType.Name = "lblDocumentType";
        lblDocumentType.Size = new Size(143, 50);
        lblDocumentType.TabIndex = 0;
        lblDocumentType.Text = "نوع المستند *";
        // 
        // cboDocumentType
        // 
        cboDocumentType.Dock = DockStyle.Fill;
        cboDocumentType.DropDownStyle = ComboBoxStyle.DropDownList;
        cboDocumentType.Location = new Point(3, 4);
        cboDocumentType.Margin = new Padding(3, 4, 3, 4);
        cboDocumentType.Name = "cboDocumentType";
        cboDocumentType.Size = new Size(454, 31);
        cboDocumentType.TabIndex = 0;
        // 
        // lblPrefix
        // 
        lblPrefix.Dock = DockStyle.Fill;
        lblPrefix.Location = new Point(463, 50);
        lblPrefix.Name = "lblPrefix";
        lblPrefix.Size = new Size(143, 50);
        lblPrefix.TabIndex = 1;
        lblPrefix.Text = "البادئة";
        // 
        // txtPrefix
        // 
        txtPrefix.Dock = DockStyle.Fill;
        txtPrefix.Location = new Point(3, 55);
        txtPrefix.Margin = new Padding(3, 5, 3, 5);
        txtPrefix.Name = "txtPrefix";
        txtPrefix.PlaceholderText = "مثال: INV-";
        txtPrefix.Size = new Size(454, 30);
        txtPrefix.TabIndex = 1;
        // 
        // lblLastNumber
        // 
        lblLastNumber.Dock = DockStyle.Fill;
        lblLastNumber.Location = new Point(463, 100);
        lblLastNumber.Name = "lblLastNumber";
        lblLastNumber.Size = new Size(143, 50);
        lblLastNumber.TabIndex = 2;
        lblLastNumber.Text = "آخر رقم";
        // 
        // nudLastNumber
        // 
        nudLastNumber.Dock = DockStyle.Fill;
        nudLastNumber.Location = new Point(3, 105);
        nudLastNumber.Margin = new Padding(3, 5, 3, 5);
        nudLastNumber.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
        nudLastNumber.Name = "nudLastNumber";
        nudLastNumber.ReadOnly = true;
        nudLastNumber.Size = new Size(454, 30);
        nudLastNumber.TabIndex = 2;
        // 
        // lblNextNumber
        // 
        lblNextNumber.Dock = DockStyle.Fill;
        lblNextNumber.Location = new Point(463, 150);
        lblNextNumber.Name = "lblNextNumber";
        lblNextNumber.Size = new Size(143, 50);
        lblNextNumber.TabIndex = 3;
        lblNextNumber.Text = "الرقم التالي *";
        // 
        // nudNextNumber
        // 
        nudNextNumber.Dock = DockStyle.Fill;
        nudNextNumber.Location = new Point(3, 155);
        nudNextNumber.Margin = new Padding(3, 5, 3, 5);
        nudNextNumber.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
        nudNextNumber.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudNextNumber.Name = "nudNextNumber";
        nudNextNumber.Size = new Size(454, 30);
        nudNextNumber.TabIndex = 3;
        nudNextNumber.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // lblDigits
        // 
        lblDigits.Dock = DockStyle.Fill;
        lblDigits.Location = new Point(463, 200);
        lblDigits.Name = "lblDigits";
        lblDigits.Size = new Size(143, 50);
        lblDigits.TabIndex = 4;
        lblDigits.Text = "عدد الخانات *";
        // 
        // nudDigits
        // 
        nudDigits.Dock = DockStyle.Fill;
        nudDigits.Location = new Point(3, 205);
        nudDigits.Margin = new Padding(3, 5, 3, 5);
        nudDigits.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
        nudDigits.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudDigits.Name = "nudDigits";
        nudDigits.Size = new Size(454, 30);
        nudDigits.TabIndex = 4;
        nudDigits.Value = new decimal(new int[] { 6, 0, 0, 0 });
        // 
        // lblResetType
        // 
        lblResetType.Dock = DockStyle.Fill;
        lblResetType.Location = new Point(463, 250);
        lblResetType.Name = "lblResetType";
        lblResetType.Size = new Size(143, 50);
        lblResetType.TabIndex = 5;
        lblResetType.Text = "نوع إعادة البدء";
        // 
        // cboResetType
        // 
        cboResetType.Dock = DockStyle.Fill;
        cboResetType.DropDownStyle = ComboBoxStyle.DropDownList;
        cboResetType.Items.AddRange(new object[] { "بدون إعادة", "سنوي", "شهري", "يومي" });
        cboResetType.Location = new Point(3, 254);
        cboResetType.Margin = new Padding(3, 4, 3, 4);
        cboResetType.Name = "cboResetType";
        cboResetType.Size = new Size(454, 31);
        cboResetType.TabIndex = 5;
        // 
        // chkActive
        // 
        chkActive.AutoSize = true;
        chkActive.Checked = true;
        chkActive.CheckState = CheckState.Checked;
        chkActive.Location = new Point(326, 304);
        chkActive.Margin = new Padding(3, 4, 3, 4);
        chkActive.Name = "chkActive";
        chkActive.Size = new Size(131, 27);
        chkActive.TabIndex = 6;
        chkActive.Text = "القاعدة نشطة";
        // 
        // lblPreviewCaption
        // 
        lblPreviewCaption.Dock = DockStyle.Fill;
        lblPreviewCaption.Location = new Point(463, 350);
        lblPreviewCaption.Name = "lblPreviewCaption";
        lblPreviewCaption.Size = new Size(143, 145);
        lblPreviewCaption.TabIndex = 7;
        lblPreviewCaption.Text = "المعاينة";
        // 
        // lblNumberPreview
        // 
        lblNumberPreview.BorderStyle = BorderStyle.FixedSingle;
        lblNumberPreview.Dock = DockStyle.Fill;
        lblNumberPreview.Location = new Point(3, 350);
        lblNumberPreview.Name = "lblNumberPreview";
        lblNumberPreview.Size = new Size(454, 145);
        lblNumberPreview.TabIndex = 8;
        lblNumberPreview.Text = "INV-000001";
        lblNumberPreview.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlActions
        // 
        pnlActions.Controls.Add(flpActions);
        pnlActions.Dock = DockStyle.Bottom;
        pnlActions.Location = new Point(0, 736);
        pnlActions.Margin = new Padding(3, 4, 3, 4);
        pnlActions.Name = "pnlActions";
        pnlActions.Padding = new Padding(16, 8, 16, 8);
        pnlActions.Size = new Size(1240, 64);
        pnlActions.TabIndex = 3;
        // 
        // flpActions
        // 
        flpActions.Controls.Add(btnNew);
        flpActions.Controls.Add(btnSave);
        flpActions.Controls.Add(btnEdit);
        flpActions.Controls.Add(btnDisable);
        flpActions.Controls.Add(btnPrint);
        flpActions.Controls.Add(btnClose);
        flpActions.Dock = DockStyle.Fill;
        flpActions.Location = new Point(16, 8);
        flpActions.Margin = new Padding(3, 4, 3, 4);
        flpActions.Name = "flpActions";
        flpActions.Size = new Size(1208, 48);
        flpActions.TabIndex = 0;
        flpActions.WrapContents = false;
        // 
        // btnNew
        // 
        btnNew.Location = new Point(1119, 4);
        btnNew.Margin = new Padding(3, 4, 3, 4);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(86, 31);
        btnNew.TabIndex = 0;
        btnNew.Text = "جديد";
        // 
        // btnSave
        // 
        btnSave.Location = new Point(1027, 4);
        btnSave.Margin = new Padding(3, 4, 3, 4);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(86, 31);
        btnSave.TabIndex = 1;
        btnSave.Text = "حفظ";
        // 
        // btnEdit
        // 
        btnEdit.Location = new Point(935, 4);
        btnEdit.Margin = new Padding(3, 4, 3, 4);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(86, 31);
        btnEdit.TabIndex = 2;
        btnEdit.Text = "تعديل";
        // 
        // btnDisable
        // 
        btnDisable.Location = new Point(843, 4);
        btnDisable.Margin = new Padding(3, 4, 3, 4);
        btnDisable.Name = "btnDisable";
        btnDisable.Size = new Size(86, 31);
        btnDisable.TabIndex = 3;
        btnDisable.Text = "إيقاف";
        // 
        // btnPrint
        // 
        btnPrint.Location = new Point(751, 4);
        btnPrint.Margin = new Padding(3, 4, 3, 4);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(86, 31);
        btnPrint.TabIndex = 4;
        btnPrint.Text = "طباعة";
        // 
        // btnClose
        // 
        btnClose.Location = new Point(659, 4);
        btnClose.Margin = new Padding(3, 4, 3, 4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(86, 31);
        btnClose.TabIndex = 5;
        btnClose.Text = "إغلاق";
        // 
        // FrmDocumentNumbering
        // 
        AutoScaleMode = AutoScaleMode.None;
        ClientSize = new Size(1240, 800);
        Controls.Add(splitMain);
        Controls.Add(pnlFilters);
        Controls.Add(pnlActions);
        Controls.Add(pnlHeader);
        Margin = new Padding(3, 4, 3, 4);
        MinimumSize = new Size(1196, 799);
        Name = "FrmDocumentNumbering";
        Text = "ترقيم المستندات";
        pnlHeader.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        tlpFilters.ResumeLayout(false);
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
        splitMain.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvDocumentSequences).EndInit();
        grpDetails.ResumeLayout(false);
        tlpDetails.ResumeLayout(false);
        tlpDetails.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudLastNumber).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudNextNumber).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudDigits).EndInit();
        pnlActions.ResumeLayout(false);
        flpActions.ResumeLayout(false);
        ResumeLayout(false);
    }
}
