namespace TransportERP.Desktop.البوالص_والشحن.الاعدادات
{
    partial class UcShipmentBooks
    {
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;

    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
    private FlowLayoutPanel designerHiddenCommands = null!;
    private Button standardCommandView = null!;
    private Button standardCommandPrint = null!;
    private Button standardCommandImport = null!;
    private Button standardCommandExport = null!;
    private Button standardCommandHelp = null!;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        designerHiddenCommands = new FlowLayoutPanel();
        standardCommandView = new Button();
        standardCommandPrint = new Button();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
            tlpAuditInfo = new TableLayoutPanel();
            lblPrintCount = new Label();
            lblLastPrintedAt = new Label();
            lblEditCount = new Label();
            lblModifiedAt = new Label();
            lblModifiedBy = new Label();
            lblCreatedAt = new Label();
            lblCreatedBy = new Label();
            pnlHeader = new Panel();
            pnlToolbar = new FlowLayoutPanel();
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
            lblTitle = new Label();
            cmbCustodyType = new ComboBox();
            txtBookCode = new TextBox();
            lblPricingRuleCode = new Label();
            label9 = new Label();
            label2 = new Label();
            label3 = new Label();
            nudSheetCount = new NumericUpDown();
            nudCurrentDocumentNo = new NumericUpDown();
            label4 = new Label();
            label5 = new Label();
            tlpPricingRuleFields = new TableLayoutPanel();
            dtpAssignedDate = new DateTimePicker();
            label11 = new Label();
            dtpReturnedDate = new DateTimePicker();
            label12 = new Label();
            cmbBookStatus = new ComboBox();
            label1 = new Label();
            txtBookNumber = new TextBox();
            label15 = new Label();
            nudStartDocumentNo = new NumericUpDown();
            label8 = new Label();
            nudEndDocumentNo = new NumericUpDown();
            label7 = new Label();
            txtNotes = new TextBox();
            label14 = new Label();
            cmbBookType = new ComboBox();
            txtDescription = new TextBox();
            label13 = new Label();
            cmbBranch = new ComboBox();
            cmbCustodyHolder = new ComboBox();
            label6 = new Label();
            chkIsActive = new CheckBox();
            tlpAuditInfo.SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSheetCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCurrentDocumentNo).BeginInit();
            tlpPricingRuleFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudStartDocumentNo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudEndDocumentNo).BeginInit();
            SuspendLayout();
            // 
            // tlpAuditInfo
            // 
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
            tlpAuditInfo.Location = new Point(0, 828);
            tlpAuditInfo.Margin = new Padding(0);
            tlpAuditInfo.BackColor = Color.FromArgb(248, 250, 252);
            tlpAuditInfo.Name = "tlpAuditInfo";
            tlpAuditInfo.Padding = new Padding(8);
            tlpAuditInfo.RowCount = 1;
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAuditInfo.Size = new Size(1406, 48);
            tlpAuditInfo.TabIndex = 8;
            // 
            // lblPrintCount
            // 
            lblPrintCount.BackColor = Color.FromArgb(192, 255, 255);
            lblPrintCount.Dock = DockStyle.Fill;
            lblPrintCount.Font = new Font("Segoe UI", 10F);
            lblPrintCount.Location = new Point(3, 0);
            lblPrintCount.ForeColor = Color.FromArgb(16, 24, 40);
            lblPrintCount.Name = "lblPrintCount";
            lblPrintCount.Margin = new Padding(3);
            lblPrintCount.AutoEllipsis = true;
            lblPrintCount.RightToLeft = RightToLeft.No;
            lblPrintCount.Size = new Size(168, 38);
            lblPrintCount.TabIndex = 8;
            lblPrintCount.Text = "عدد مرات الطباعة :- 00";
            lblPrintCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblLastPrintedAt
            // 
            lblLastPrintedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblLastPrintedAt.Dock = DockStyle.Fill;
            lblLastPrintedAt.Font = new Font("Segoe UI", 10F);
            lblLastPrintedAt.Location = new Point(177, 0);
            lblLastPrintedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblLastPrintedAt.Name = "lblLastPrintedAt";
            lblLastPrintedAt.Margin = new Padding(3);
            lblLastPrintedAt.AutoEllipsis = true;
            lblLastPrintedAt.RightToLeft = RightToLeft.No;
            lblLastPrintedAt.Size = new Size(218, 38);
            lblLastPrintedAt.TabIndex = 5;
            lblLastPrintedAt.Text = "تاريخ اخر طباعة:  27/09/2026";
            lblLastPrintedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblEditCount
            // 
            lblEditCount.BackColor = Color.FromArgb(192, 255, 255);
            lblEditCount.Dock = DockStyle.Fill;
            lblEditCount.Font = new Font("Segoe UI", 10F);
            lblEditCount.Location = new Point(401, 0);
            lblEditCount.ForeColor = Color.FromArgb(16, 24, 40);
            lblEditCount.Name = "lblEditCount";
            lblEditCount.Margin = new Padding(3);
            lblEditCount.AutoEllipsis = true;
            lblEditCount.RightToLeft = RightToLeft.No;
            lblEditCount.Size = new Size(162, 38);
            lblEditCount.TabIndex = 4;
            lblEditCount.Text = "عدد التعديلات :- 00";
            lblEditCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedAt
            // 
            lblModifiedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedAt.Dock = DockStyle.Fill;
            lblModifiedAt.Font = new Font("Segoe UI", 10F);
            lblModifiedAt.Location = new Point(569, 0);
            lblModifiedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblModifiedAt.Name = "lblModifiedAt";
            lblModifiedAt.Margin = new Padding(3);
            lblModifiedAt.AutoEllipsis = true;
            lblModifiedAt.RightToLeft = RightToLeft.No;
            lblModifiedAt.Size = new Size(218, 38);
            lblModifiedAt.TabIndex = 3;
            lblModifiedAt.Text = " تاريخ التعديل:  27/09/2026";
            lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedBy
            // 
            lblModifiedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedBy.Dock = DockStyle.Fill;
            lblModifiedBy.Font = new Font("Segoe UI", 10F);
            lblModifiedBy.Location = new Point(793, 0);
            lblModifiedBy.ForeColor = Color.FromArgb(16, 24, 40);
            lblModifiedBy.Name = "lblModifiedBy";
            lblModifiedBy.Margin = new Padding(3);
            lblModifiedBy.AutoEllipsis = true;
            lblModifiedBy.RightToLeft = RightToLeft.No;
            lblModifiedBy.Size = new Size(190, 38);
            lblModifiedBy.TabIndex = 2;
            lblModifiedBy.Text = "عدل بواسطة: - مدير النظام";
            lblModifiedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedAt
            // 
            lblCreatedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedAt.Dock = DockStyle.Fill;
            lblCreatedAt.Font = new Font("Segoe UI", 10F);
            lblCreatedAt.Location = new Point(989, 0);
            lblCreatedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblCreatedAt.Name = "lblCreatedAt";
            lblCreatedAt.Margin = new Padding(3);
            lblCreatedAt.AutoEllipsis = true;
            lblCreatedAt.RightToLeft = RightToLeft.No;
            lblCreatedAt.Size = new Size(218, 38);
            lblCreatedAt.TabIndex = 1;
            lblCreatedAt.Text = " تاريخ الانشاء:  27/09/2026";
            lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedBy
            // 
            lblCreatedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedBy.Dock = DockStyle.Fill;
            lblCreatedBy.Font = new Font("Segoe UI", 10F);
            lblCreatedBy.Location = new Point(1213, 0);
            lblCreatedBy.ForeColor = Color.FromArgb(16, 24, 40);
            lblCreatedBy.Name = "lblCreatedBy";
            lblCreatedBy.Margin = new Padding(3);
            lblCreatedBy.AutoEllipsis = true;
            lblCreatedBy.RightToLeft = RightToLeft.No;
            lblCreatedBy.Size = new Size(190, 38);
            lblCreatedBy.TabIndex = 0;
            lblCreatedBy.Text = " أنشأ بواسطة: - مدير النظام";
            lblCreatedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(224, 224, 224);
            pnlHeader.Controls.Add(designerCommandBar);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.MinimumSize = new Size(0, 48);
            pnlHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlHeader.AutoSize = true;
            pnlHeader.Size = new Size(1406, 61);
            pnlHeader.TabIndex = 9;
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.FromArgb(224, 224, 224);
            pnlToolbar.Controls.Add(btnNew);
            pnlToolbar.Controls.Add(btnSave);
            pnlToolbar.Controls.Add(btnEdit);
            pnlToolbar.Controls.Add(btnDelete);
            pnlToolbar.Controls.Add(btnRefresh);
            pnlToolbar.Controls.Add(btnClose);
            pnlToolbar.Controls.Add(btnFirst);
            pnlToolbar.Controls.Add(btnPrevious);
            pnlToolbar.Controls.Add(txtCurrentRecordNo);
            pnlToolbar.Controls.Add(btnNext);
            pnlToolbar.Controls.Add(btnLast);
            pnlToolbar.Controls.Add(btnUndo);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(187, 0);
            pnlToolbar.Margin = new Padding(5);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.MinimumSize = new Size(0, 48);
            pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlToolbar.AutoSize = true;
            pnlToolbar.Padding = new Padding(5);
            pnlToolbar.Size = new Size(1219, 61);
            pnlToolbar.TabIndex = 2;
            pnlToolbar.WrapContents = true;
            // 
            // btnNew
            // 
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Microsoft Sans Serif", 10F);
            btnNew.Location = new Point(1102, 11);
            btnNew.Margin = new Padding(4);
            btnNew.BackColor = Color.FromArgb(224, 224, 224);
            btnNew.ForeColor = Color.FromArgb(16, 24, 40);
            btnNew.AutoSize = false;
            btnNew.Padding = new Padding(0);
            btnNew.Name = "btnNew";
            btnNew.Dock = DockStyle.None;
            btnNew.Size = new Size(70, 30);
            btnNew.TabIndex = 0;
            btnNew.Text = "جديد";
            btnNew.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Microsoft Sans Serif", 10F);
            btnSave.Location = new Point(993, 11);
            btnSave.Margin = new Padding(4);
            btnSave.BackColor = Color.FromArgb(224, 224, 224);
            btnSave.ForeColor = Color.FromArgb(16, 24, 40);
            btnSave.AutoSize = false;
            btnSave.Padding = new Padding(0);
            btnSave.Name = "btnSave";
            btnSave.Dock = DockStyle.None;
            btnSave.Size = new Size(70, 30);
            btnSave.TabIndex = 1;
            btnSave.Text = "حفظ";
            btnSave.UseVisualStyleBackColor = false;
            // 
            // btnEdit
            // 
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Microsoft Sans Serif", 10F);
            btnEdit.Location = new Point(884, 11);
            btnEdit.Margin = new Padding(4);
            btnEdit.BackColor = Color.FromArgb(224, 224, 224);
            btnEdit.ForeColor = Color.FromArgb(16, 24, 40);
            btnEdit.AutoSize = false;
            btnEdit.Padding = new Padding(0);
            btnEdit.Name = "btnEdit";
            btnEdit.Dock = DockStyle.None;
            btnEdit.Size = new Size(70, 30);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "تعديل";
            btnEdit.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Microsoft Sans Serif", 10F);
            btnDelete.Location = new Point(775, 11);
            btnDelete.Margin = new Padding(4);
            btnDelete.BackColor = Color.FromArgb(224, 224, 224);
            btnDelete.ForeColor = Color.FromArgb(16, 24, 40);
            btnDelete.AutoSize = false;
            btnDelete.Padding = new Padding(0);
            btnDelete.Name = "btnDelete";
            btnDelete.Dock = DockStyle.None;
            btnDelete.Size = new Size(70, 30);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "حذف";
            btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Microsoft Sans Serif", 10F);
            btnRefresh.Location = new Point(666, 11);
            btnRefresh.Margin = new Padding(4);
            btnRefresh.BackColor = Color.FromArgb(224, 224, 224);
            btnRefresh.ForeColor = Color.FromArgb(16, 24, 40);
            btnRefresh.AutoSize = false;
            btnRefresh.Padding = new Padding(0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Dock = DockStyle.None;
            btnRefresh.Size = new Size(70, 30);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "تحديث";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Microsoft Sans Serif", 10F);
            btnClose.Location = new Point(557, 11);
            btnClose.Margin = new Padding(4);
            btnClose.BackColor = Color.FromArgb(224, 224, 224);
            btnClose.ForeColor = Color.FromArgb(16, 24, 40);
            btnClose.AutoSize = false;
            btnClose.Padding = new Padding(0);
            btnClose.Name = "btnClose";
            btnClose.Dock = DockStyle.None;
            btnClose.Size = new Size(70, 30);
            btnClose.TabIndex = 5;
            btnClose.Text = "اغلاق";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // btnFirst
            // 
            btnFirst.FlatStyle = FlatStyle.Flat;
            btnFirst.Font = new Font("Microsoft Sans Serif", 10F);
            btnFirst.Location = new Point(448, 11);
            btnFirst.Margin = new Padding(4);
            btnFirst.BackColor = Color.FromArgb(224, 224, 224);
            btnFirst.ForeColor = Color.FromArgb(16, 24, 40);
            btnFirst.AutoSize = false;
            btnFirst.Padding = new Padding(0);
            btnFirst.Name = "btnFirst";
            btnFirst.Dock = DockStyle.None;
            btnFirst.Size = new Size(70, 30);
            btnFirst.TabIndex = 6;
            btnFirst.Text = "الاول ";
            btnFirst.UseVisualStyleBackColor = false;
            // 
            // btnPrevious
            // 
            btnPrevious.FlatStyle = FlatStyle.Flat;
            btnPrevious.Font = new Font("Microsoft Sans Serif", 10F);
            btnPrevious.Location = new Point(339, 11);
            btnPrevious.Margin = new Padding(4);
            btnPrevious.BackColor = Color.FromArgb(224, 224, 224);
            btnPrevious.ForeColor = Color.FromArgb(16, 24, 40);
            btnPrevious.AutoSize = false;
            btnPrevious.Padding = new Padding(0);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Dock = DockStyle.None;
            btnPrevious.Size = new Size(70, 30);
            btnPrevious.TabIndex = 7;
            btnPrevious.Text = "السابق";
            btnPrevious.UseVisualStyleBackColor = false;
            // 
            // txtCurrentRecordNo
            // 
            txtCurrentRecordNo.Location = new Point(260, 12);
            txtCurrentRecordNo.Margin = new Padding(4);
            txtCurrentRecordNo.Multiline = true;
            txtCurrentRecordNo.BackColor = Color.White;
            txtCurrentRecordNo.ForeColor = Color.FromArgb(16, 24, 40);
            txtCurrentRecordNo.Name = "txtCurrentRecordNo";
            txtCurrentRecordNo.Font = new Font("Segoe UI", 11F);
            txtCurrentRecordNo.Dock = DockStyle.None;
            txtCurrentRecordNo.ReadOnly = true;
            txtCurrentRecordNo.Size = new Size(62, 30);
            txtCurrentRecordNo.TabIndex = 8;
            txtCurrentRecordNo.TextAlign = HorizontalAlignment.Center;
            // 
            // btnNext
            // 
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("Microsoft Sans Serif", 10F);
            btnNext.Location = new Point(149, 11);
            btnNext.Margin = new Padding(4);
            btnNext.BackColor = Color.FromArgb(224, 224, 224);
            btnNext.ForeColor = Color.FromArgb(16, 24, 40);
            btnNext.AutoSize = false;
            btnNext.Padding = new Padding(0);
            btnNext.Name = "btnNext";
            btnNext.Dock = DockStyle.None;
            btnNext.Size = new Size(70, 30);
            btnNext.TabIndex = 9;
            btnNext.Text = "التالي";
            btnNext.UseVisualStyleBackColor = false;
            // 
            // btnLast
            // 
            btnLast.FlatStyle = FlatStyle.Flat;
            btnLast.Font = new Font("Microsoft Sans Serif", 10F);
            btnLast.Location = new Point(40, 11);
            btnLast.Margin = new Padding(4);
            btnLast.BackColor = Color.FromArgb(224, 224, 224);
            btnLast.ForeColor = Color.FromArgb(16, 24, 40);
            btnLast.AutoSize = false;
            btnLast.Padding = new Padding(0);
            btnLast.Name = "btnLast";
            btnLast.Dock = DockStyle.None;
            btnLast.Size = new Size(70, 30);
            btnLast.TabIndex = 10;
            btnLast.Text = "الاخير";
            btnLast.UseVisualStyleBackColor = false;
            // 
            // btnUndo
            // 
            btnUndo.FlatStyle = FlatStyle.Flat;
            btnUndo.Font = new Font("Microsoft Sans Serif", 10F);
            btnUndo.Location = new Point(-69, 11);
            btnUndo.Margin = new Padding(4);
            btnUndo.BackColor = Color.FromArgb(224, 224, 224);
            btnUndo.ForeColor = Color.FromArgb(16, 24, 40);
            btnUndo.AutoSize = false;
            btnUndo.Padding = new Padding(0);
            btnUndo.Name = "btnUndo";
            btnUndo.Dock = DockStyle.None;
            btnUndo.Size = new Size(70, 30);
            btnUndo.TabIndex = 11;
            btnUndo.Text = "تراجع";
            btnUndo.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.FromArgb(192, 192, 255);
            lblTitle.Dock = DockStyle.Left;
            lblTitle.Font = new Font("Segoe UI", 16F);
            lblTitle.Location = new Point(0, 0);
            lblTitle.ForeColor = Color.FromArgb(16, 24, 40);
            lblTitle.Name = "lblTitle";
            lblTitle.Padding = new Padding(0, 0, 17, 0);
            lblTitle.RightToLeft = RightToLeft.Yes;
            lblTitle.Size = new Size(187, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "دفاتر البوالص";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cmbCustodyType
            // 
            cmbCustodyType.FormattingEnabled = true;
            cmbCustodyType.Location = new Point(709, 173);
            cmbCustodyType.BackColor = Color.White;
            cmbCustodyType.ForeColor = Color.FromArgb(16, 24, 40);
            cmbCustodyType.Name = "cmbCustodyType";
            cmbCustodyType.Font = new Font("Segoe UI", 11F);
            cmbCustodyType.Margin = new Padding(3);
            cmbCustodyType.Dock = DockStyle.Fill;
            cmbCustodyType.Size = new Size(477, 31);
            cmbCustodyType.TabIndex = 194;
            // 
            // txtBookCode
            // 
            txtBookCode.Dock = DockStyle.Fill;
            txtBookCode.Location = new Point(710, 15);
            txtBookCode.Margin = new Padding(3);
            txtBookCode.BackColor = Color.White;
            txtBookCode.ForeColor = Color.FromArgb(16, 24, 40);
            txtBookCode.Name = "txtBookCode";
            txtBookCode.Font = new Font("Segoe UI", 11F);
            txtBookCode.Size = new Size(474, 30);
            txtBookCode.TabIndex = 1;
            // 
            // lblPricingRuleCode
            // 
            lblPricingRuleCode.Dock = DockStyle.Fill;
            lblPricingRuleCode.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPricingRuleCode.Location = new Point(1192, 10);
            lblPricingRuleCode.ForeColor = Color.FromArgb(16, 24, 40);
            lblPricingRuleCode.BackColor = Color.Transparent;
            lblPricingRuleCode.Name = "lblPricingRuleCode";
            lblPricingRuleCode.Margin = new Padding(3);
            lblPricingRuleCode.AutoEllipsis = true;
            lblPricingRuleCode.Size = new Size(201, 40);
            lblPricingRuleCode.TabIndex = 0;
            lblPricingRuleCode.Text = "كود الدفتر";
            lblPricingRuleCode.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            label9.Dock = DockStyle.Fill;
            label9.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label9.Location = new Point(1192, 50);
            label9.ForeColor = Color.FromArgb(16, 24, 40);
            label9.BackColor = Color.Transparent;
            label9.Name = "label9";
            label9.Margin = new Padding(3);
            label9.AutoEllipsis = true;
            label9.Size = new Size(201, 40);
            label9.TabIndex = 123;
            label9.Text = "نوع الدفتر";
            label9.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label2.Location = new Point(1192, 210);
            label2.ForeColor = Color.FromArgb(16, 24, 40);
            label2.BackColor = Color.Transparent;
            label2.Name = "label2";
            label2.Margin = new Padding(3);
            label2.AutoEllipsis = true;
            label2.Size = new Size(201, 40);
            label2.TabIndex = 184;
            label2.Text = "الفرع ";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label3.Location = new Point(1192, 130);
            label3.ForeColor = Color.FromArgb(16, 24, 40);
            label3.BackColor = Color.Transparent;
            label3.Name = "label3";
            label3.Margin = new Padding(3);
            label3.AutoEllipsis = true;
            label3.Size = new Size(201, 40);
            label3.TabIndex = 188;
            label3.Text = "عدد اوراق الدفتر";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudSheetCount
            // 
            nudSheetCount.Dock = DockStyle.Fill;
            nudSheetCount.Location = new Point(708, 133);
            nudSheetCount.BackColor = Color.White;
            nudSheetCount.ForeColor = Color.FromArgb(16, 24, 40);
            nudSheetCount.Name = "nudSheetCount";
            nudSheetCount.Font = new Font("Segoe UI", 11F);
            nudSheetCount.Margin = new Padding(3);
            nudSheetCount.Size = new Size(478, 30);
            nudSheetCount.TabIndex = 189;
            // 
            // nudCurrentDocumentNo
            // 
            nudCurrentDocumentNo.Dock = DockStyle.Fill;
            nudCurrentDocumentNo.Location = new Point(13, 133);
            nudCurrentDocumentNo.BackColor = Color.White;
            nudCurrentDocumentNo.ForeColor = Color.FromArgb(16, 24, 40);
            nudCurrentDocumentNo.Name = "nudCurrentDocumentNo";
            nudCurrentDocumentNo.Font = new Font("Segoe UI", 11F);
            nudCurrentDocumentNo.Margin = new Padding(3);
            nudCurrentDocumentNo.Size = new Size(526, 30);
            nudCurrentDocumentNo.TabIndex = 191;
            // 
            // label4
            // 
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label4.Location = new Point(545, 130);
            label4.ForeColor = Color.FromArgb(16, 24, 40);
            label4.BackColor = Color.Transparent;
            label4.Name = "label4";
            label4.Margin = new Padding(3);
            label4.AutoEllipsis = true;
            label4.Size = new Size(157, 40);
            label4.TabIndex = 192;
            label4.Text = "رقم المستند الحالي";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label5.Location = new Point(1192, 170);
            label5.ForeColor = Color.FromArgb(16, 24, 40);
            label5.BackColor = Color.Transparent;
            label5.Name = "label5";
            label5.Margin = new Padding(3);
            label5.AutoEllipsis = true;
            label5.Size = new Size(201, 40);
            label5.TabIndex = 193;
            label5.Text = "جهة العهدة";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tlpPricingRuleFields
            // 
            tlpPricingRuleFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpPricingRuleFields.BackColor = Color.LightCyan;
            tlpPricingRuleFields.ColumnCount = 4;
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.97839F));
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34.9495659F));
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.8155622F));
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38.25648F));
            tlpPricingRuleFields.Controls.Add(dtpAssignedDate, 1, 6);
            tlpPricingRuleFields.Controls.Add(label11, 0, 6);
            tlpPricingRuleFields.Controls.Add(dtpReturnedDate, 3, 6);
            tlpPricingRuleFields.Controls.Add(label12, 2, 6);
            tlpPricingRuleFields.Controls.Add(cmbBookStatus, 3, 0);
            tlpPricingRuleFields.Controls.Add(label1, 2, 0);
            tlpPricingRuleFields.Controls.Add(txtBookNumber, 3, 1);
            tlpPricingRuleFields.Controls.Add(label15, 2, 1);
            tlpPricingRuleFields.Controls.Add(nudStartDocumentNo, 1, 2);
            tlpPricingRuleFields.Controls.Add(label8, 0, 2);
            tlpPricingRuleFields.Controls.Add(nudEndDocumentNo, 3, 2);
            tlpPricingRuleFields.Controls.Add(label7, 2, 2);
            tlpPricingRuleFields.Controls.Add(txtNotes, 3, 7);
            tlpPricingRuleFields.Controls.Add(label14, 2, 7);
            tlpPricingRuleFields.Controls.Add(cmbBookType, 1, 1);
            tlpPricingRuleFields.Controls.Add(txtDescription, 1, 7);
            tlpPricingRuleFields.Controls.Add(label13, 0, 7);
            tlpPricingRuleFields.Controls.Add(cmbBranch, 1, 5);
            tlpPricingRuleFields.Controls.Add(cmbCustodyHolder, 3, 4);
            tlpPricingRuleFields.Controls.Add(label6, 2, 4);
            tlpPricingRuleFields.Controls.Add(label5, 0, 4);
            tlpPricingRuleFields.Controls.Add(label4, 2, 3);
            tlpPricingRuleFields.Controls.Add(nudCurrentDocumentNo, 3, 3);
            tlpPricingRuleFields.Controls.Add(nudSheetCount, 1, 3);
            tlpPricingRuleFields.Controls.Add(label3, 0, 3);
            tlpPricingRuleFields.Controls.Add(label2, 0, 5);
            tlpPricingRuleFields.Controls.Add(label9, 0, 1);
            tlpPricingRuleFields.Controls.Add(lblPricingRuleCode, 0, 0);
            tlpPricingRuleFields.Controls.Add(txtBookCode, 1, 0);
            tlpPricingRuleFields.Controls.Add(cmbCustodyType, 1, 4);
            tlpPricingRuleFields.Controls.Add(chkIsActive, 1, 8);
            tlpPricingRuleFields.Dock = DockStyle.Top;
            tlpPricingRuleFields.Location = new Point(0, 61);
            tlpPricingRuleFields.Margin = new Padding(0);
            tlpPricingRuleFields.Name = "tlpPricingRuleFields";
            tlpPricingRuleFields.MinimumSize = new Size(0, 392);
            tlpPricingRuleFields.Padding = new Padding(0);
            tlpPricingRuleFields.RowCount = 9;
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.Size = new Size(1406, 392);
            tlpPricingRuleFields.TabIndex = 12;
            tlpPricingRuleFields.Paint += tlpPricingRuleFields_Paint;
            // 
            // dtpAssignedDate
            // 
            dtpAssignedDate.Location = new Point(708, 253);
            dtpAssignedDate.Name = "dtpAssignedDate";
            dtpAssignedDate.Font = new Font("Segoe UI", 11F);
            dtpAssignedDate.Margin = new Padding(3);
            dtpAssignedDate.Dock = DockStyle.Fill;
            dtpAssignedDate.Size = new Size(478, 30);
            dtpAssignedDate.TabIndex = 222;
            // 
            // label11
            // 
            label11.Dock = DockStyle.Fill;
            label11.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label11.Location = new Point(1192, 250);
            label11.ForeColor = Color.FromArgb(16, 24, 40);
            label11.BackColor = Color.Transparent;
            label11.Name = "label11";
            label11.Margin = new Padding(3);
            label11.AutoEllipsis = true;
            label11.Size = new Size(201, 40);
            label11.TabIndex = 221;
            label11.Text = "تاريخ التسليم للعهدة";
            label11.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dtpReturnedDate
            // 
            dtpReturnedDate.Location = new Point(61, 253);
            dtpReturnedDate.Name = "dtpReturnedDate";
            dtpReturnedDate.Font = new Font("Segoe UI", 11F);
            dtpReturnedDate.Margin = new Padding(3);
            dtpReturnedDate.Dock = DockStyle.Fill;
            dtpReturnedDate.Size = new Size(478, 30);
            dtpReturnedDate.TabIndex = 220;
            // 
            // label12
            // 
            label12.Dock = DockStyle.Fill;
            label12.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label12.Location = new Point(545, 250);
            label12.ForeColor = Color.FromArgb(16, 24, 40);
            label12.BackColor = Color.Transparent;
            label12.Name = "label12";
            label12.Margin = new Padding(3);
            label12.AutoEllipsis = true;
            label12.Size = new Size(157, 40);
            label12.TabIndex = 219;
            label12.Text = "تاريخ الارجاع ";
            label12.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbBookStatus
            // 
            cmbBookStatus.FormattingEnabled = true;
            cmbBookStatus.Location = new Point(13, 13);
            cmbBookStatus.BackColor = Color.White;
            cmbBookStatus.ForeColor = Color.FromArgb(16, 24, 40);
            cmbBookStatus.Name = "cmbBookStatus";
            cmbBookStatus.Font = new Font("Segoe UI", 11F);
            cmbBookStatus.Margin = new Padding(3);
            cmbBookStatus.Dock = DockStyle.Fill;
            cmbBookStatus.Size = new Size(526, 31);
            cmbBookStatus.TabIndex = 218;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.Location = new Point(545, 10);
            label1.ForeColor = Color.FromArgb(16, 24, 40);
            label1.BackColor = Color.Transparent;
            label1.Name = "label1";
            label1.Margin = new Padding(3);
            label1.AutoEllipsis = true;
            label1.Size = new Size(157, 40);
            label1.TabIndex = 217;
            label1.Text = "حالة الدفتر";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtBookNumber
            // 
            txtBookNumber.Dock = DockStyle.Fill;
            txtBookNumber.Location = new Point(15, 55);
            txtBookNumber.Margin = new Padding(3);
            txtBookNumber.BackColor = Color.White;
            txtBookNumber.ForeColor = Color.FromArgb(16, 24, 40);
            txtBookNumber.Name = "txtBookNumber";
            txtBookNumber.Font = new Font("Segoe UI", 11F);
            txtBookNumber.Size = new Size(522, 30);
            txtBookNumber.TabIndex = 216;
            // 
            // label15
            // 
            label15.Dock = DockStyle.Fill;
            label15.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label15.Location = new Point(545, 50);
            label15.ForeColor = Color.FromArgb(16, 24, 40);
            label15.BackColor = Color.Transparent;
            label15.Name = "label15";
            label15.Margin = new Padding(3);
            label15.AutoEllipsis = true;
            label15.Size = new Size(157, 40);
            label15.TabIndex = 215;
            label15.Text = "رقم الدفتر ";
            label15.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudStartDocumentNo
            // 
            nudStartDocumentNo.Dock = DockStyle.Fill;
            nudStartDocumentNo.Location = new Point(708, 93);
            nudStartDocumentNo.BackColor = Color.White;
            nudStartDocumentNo.ForeColor = Color.FromArgb(16, 24, 40);
            nudStartDocumentNo.Name = "nudStartDocumentNo";
            nudStartDocumentNo.Font = new Font("Segoe UI", 11F);
            nudStartDocumentNo.Margin = new Padding(3);
            nudStartDocumentNo.Size = new Size(478, 30);
            nudStartDocumentNo.TabIndex = 214;
            // 
            // label8
            // 
            label8.Dock = DockStyle.Fill;
            label8.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label8.Location = new Point(1192, 90);
            label8.ForeColor = Color.FromArgb(16, 24, 40);
            label8.BackColor = Color.Transparent;
            label8.Name = "label8";
            label8.Margin = new Padding(3);
            label8.AutoEllipsis = true;
            label8.Size = new Size(201, 40);
            label8.TabIndex = 213;
            label8.Text = "اول رقم مستند ";
            label8.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudEndDocumentNo
            // 
            nudEndDocumentNo.Dock = DockStyle.Fill;
            nudEndDocumentNo.Location = new Point(13, 93);
            nudEndDocumentNo.BackColor = Color.White;
            nudEndDocumentNo.ForeColor = Color.FromArgb(16, 24, 40);
            nudEndDocumentNo.Name = "nudEndDocumentNo";
            nudEndDocumentNo.Font = new Font("Segoe UI", 11F);
            nudEndDocumentNo.Margin = new Padding(3);
            nudEndDocumentNo.Size = new Size(526, 30);
            nudEndDocumentNo.TabIndex = 212;
            // 
            // label7
            // 
            label7.Dock = DockStyle.Fill;
            label7.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label7.Location = new Point(545, 90);
            label7.ForeColor = Color.FromArgb(16, 24, 40);
            label7.BackColor = Color.Transparent;
            label7.Name = "label7";
            label7.Margin = new Padding(3);
            label7.AutoEllipsis = true;
            label7.Size = new Size(157, 40);
            label7.TabIndex = 210;
            label7.Text = "اخر رقم مستند";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNotes
            // 
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Location = new Point(15, 295);
            txtNotes.Margin = new Padding(3);
            txtNotes.Multiline = true;
            txtNotes.BackColor = Color.White;
            txtNotes.ForeColor = Color.FromArgb(16, 24, 40);
            txtNotes.Name = "txtNotes";
            txtNotes.Font = new Font("Segoe UI", 11F);
            txtNotes.Size = new Size(522, 30);
            txtNotes.TabIndex = 208;
            // 
            // label14
            // 
            label14.Dock = DockStyle.Fill;
            label14.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label14.Location = new Point(545, 290);
            label14.ForeColor = Color.FromArgb(16, 24, 40);
            label14.BackColor = Color.Transparent;
            label14.Name = "label14";
            label14.Margin = new Padding(3);
            label14.AutoEllipsis = true;
            label14.Size = new Size(157, 40);
            label14.TabIndex = 207;
            label14.Text = "ملاحظات";
            label14.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbBookType
            // 
            cmbBookType.FormattingEnabled = true;
            cmbBookType.Location = new Point(709, 53);
            cmbBookType.BackColor = Color.White;
            cmbBookType.ForeColor = Color.FromArgb(16, 24, 40);
            cmbBookType.Name = "cmbBookType";
            cmbBookType.Font = new Font("Segoe UI", 11F);
            cmbBookType.Margin = new Padding(3);
            cmbBookType.Dock = DockStyle.Fill;
            cmbBookType.Size = new Size(477, 31);
            cmbBookType.TabIndex = 206;
            // 
            // txtDescription
            // 
            txtDescription.Dock = DockStyle.Fill;
            txtDescription.Location = new Point(710, 295);
            txtDescription.Margin = new Padding(3);
            txtDescription.Multiline = true;
            txtDescription.BackColor = Color.White;
            txtDescription.ForeColor = Color.FromArgb(16, 24, 40);
            txtDescription.Name = "txtDescription";
            txtDescription.Font = new Font("Segoe UI", 11F);
            txtDescription.Size = new Size(474, 30);
            txtDescription.TabIndex = 205;
            // 
            // label13
            // 
            label13.Dock = DockStyle.Fill;
            label13.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label13.Location = new Point(1192, 290);
            label13.ForeColor = Color.FromArgb(16, 24, 40);
            label13.BackColor = Color.Transparent;
            label13.Name = "label13";
            label13.Margin = new Padding(3);
            label13.AutoEllipsis = true;
            label13.Size = new Size(201, 40);
            label13.TabIndex = 204;
            label13.Text = "الوصف";
            label13.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbBranch
            // 
            cmbBranch.FormattingEnabled = true;
            cmbBranch.Location = new Point(709, 213);
            cmbBranch.BackColor = Color.White;
            cmbBranch.ForeColor = Color.FromArgb(16, 24, 40);
            cmbBranch.Name = "cmbBranch";
            cmbBranch.Font = new Font("Segoe UI", 11F);
            cmbBranch.Margin = new Padding(3);
            cmbBranch.Dock = DockStyle.Fill;
            cmbBranch.Size = new Size(477, 31);
            cmbBranch.TabIndex = 197;
            // 
            // cmbCustodyHolder
            // 
            cmbCustodyHolder.FormattingEnabled = true;
            cmbCustodyHolder.Location = new Point(15, 173);
            cmbCustodyHolder.BackColor = Color.White;
            cmbCustodyHolder.ForeColor = Color.FromArgb(16, 24, 40);
            cmbCustodyHolder.Name = "cmbCustodyHolder";
            cmbCustodyHolder.Font = new Font("Segoe UI", 11F);
            cmbCustodyHolder.Margin = new Padding(3);
            cmbCustodyHolder.Dock = DockStyle.Fill;
            cmbCustodyHolder.Size = new Size(524, 31);
            cmbCustodyHolder.TabIndex = 196;
            // 
            // label6
            // 
            label6.Dock = DockStyle.Fill;
            label6.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label6.Location = new Point(545, 170);
            label6.ForeColor = Color.FromArgb(16, 24, 40);
            label6.BackColor = Color.Transparent;
            label6.Name = "label6";
            label6.Margin = new Padding(3);
            label6.AutoEllipsis = true;
            label6.Size = new Size(157, 40);
            label6.TabIndex = 195;
            label6.Text = "صاحب العهدة";
            label6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Location = new Point(1120, 333);
            chkIsActive.BackColor = Color.Transparent;
            chkIsActive.ForeColor = Color.FromArgb(16, 24, 40);
            chkIsActive.Dock = DockStyle.Fill;
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Font = new Font("Segoe UI", 10F);
            chkIsActive.Margin = new Padding(3);
            chkIsActive.Size = new Size(66, 27);
            chkIsActive.TabIndex = 226;
            chkIsActive.Text = "نشط";
            chkIsActive.UseVisualStyleBackColor = false;
            // 
            // UcShipmentBooks
            // 
            AccessibleName = "دفاتر البوالص";
            AutoScaleDimensions = new SizeF(120F, 120F);
            BackColor = Color.FromArgb(248, 250, 252);
            ForeColor = Color.FromArgb(16, 24, 40);
            Padding = new Padding(0);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(tlpPricingRuleFields);
            Controls.Add(pnlHeader);
            Controls.Add(tlpAuditInfo);
            Font = new Font("Segoe UI", 10F);
            Margin = new Padding(0);
            Name = "UcShipmentBooks";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1406, 866);
            tlpAuditInfo.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudSheetCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCurrentDocumentNo).EndInit();
            tlpPricingRuleFields.ResumeLayout(false);
            tlpPricingRuleFields.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudStartDocumentNo).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudEndDocumentNo).EndInit();
            ResumeLayout(false);
        
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(187, 0);
        designerCommandBar.Size = new Size(1219, 61);
        designerCommandBar.Margin = new Padding(5);
        designerCommandBar.MinimumSize = new Size(0, 48);
        designerCommandBar.TabIndex = 2;
        designerCommandBar.Controls.Add(pnlToolbar);
        pnlToolbar.Dock = DockStyle.Fill;
        pnlToolbar.Margin = Padding.Empty;
        designerCommandBar.Name = "designerCommandBar";
        designerCommandBar.BackColor = Color.FromArgb(239, 239, 239);
        designerCommandBar.BorderStyle = BorderStyle.FixedSingle;
        designerHiddenCommands.Name = "designerHiddenCommands";
        designerHiddenCommands.Visible = false;
        designerCommandBar.Controls.Add(designerHiddenCommands);
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
        designerCommandBar.SetCommandRole(btnDelete, TransportERP.Desktop.CoreUI.DesignerCommandRole.Delete);
        designerCommandBar.SetCommandRole(btnUndo, TransportERP.Desktop.CoreUI.DesignerCommandRole.Cancel);
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

        #endregion

        private TableLayoutPanel tlpAuditInfo;
        private Label lblPrintCount;
        private Label lblLastPrintedAt;
        private Label lblEditCount;
        private Label lblModifiedAt;
        private Label lblModifiedBy;
        private Label lblCreatedAt;
        private Label lblCreatedBy;
        private Panel pnlHeader;
        private FlowLayoutPanel pnlToolbar;
        private Button btnNew;
        private Button btnSave;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private Button btnClose;
        private Button btnFirst;
        private Button btnPrevious;
        private TextBox txtCurrentRecordNo;
        private Button btnNext;
        private Button btnLast;
        private Button btnUndo;
        private Label lblTitle;
        private ComboBox cmbCustodyType;
        private TextBox txtBookCode;
        private Label lblPricingRuleCode;
        private Label label9;
        private Label label2;
        private Label label3;
        private NumericUpDown nudSheetCount;
        private NumericUpDown nudCurrentDocumentNo;
        private Label label4;
        private Label label5;
        private TableLayoutPanel tlpPricingRuleFields;
        private ComboBox cmbBranch;
        private ComboBox cmbCustodyHolder;
        private Label label6;
        private ComboBox cmbBookType;
        private TextBox txtDescription;
        private Label label13;
        private TextBox txtNotes;
        private Label label14;
        private NumericUpDown nudStartDocumentNo;
        private Label label8;
        private NumericUpDown nudEndDocumentNo;
        private Label label7;
        private DateTimePicker dtpAssignedDate;
        private Label label11;
        private DateTimePicker dtpReturnedDate;
        private Label label12;
        private ComboBox cmbBookStatus;
        private Label label1;
        private TextBox txtBookNumber;
        private Label label15;
        private CheckBox chkIsActive;
    }
}
