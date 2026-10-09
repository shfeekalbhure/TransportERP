namespace TransportERP.Desktop.البوالص_والشحن.الاعدادات
{
    partial class UcShipmentPricingRules
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
            tlpAuditInfo = new TableLayoutPanel();
            lblPrintCount = new Label();
            lblLastPrintedAt = new Label();
            lblEditCount = new Label();
            lblModifiedAt = new Label();
            lblModifiedBy = new Label();
            lblCreatedAt = new Label();
            lblCreatedBy = new Label();
            pnlContent = new Panel();
            tabMain = new TabControl();
            tabPricingRules = new TabPage();
            tlpPricingRuleFields = new TableLayoutPanel();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label1 = new Label();
            cmbCurrency = new ComboBox();
            txtNotes = new TextBox();
            lblNotes = new Label();
            nudAdditionalFees = new NumericUpDown();
            lblAdditionalFees = new Label();
            nudPrice = new NumericUpDown();
            lblPrice = new Label();
            nudMaximumValue = new NumericUpDown();
            lblMaximumValue = new Label();
            nudMinimumValue = new NumericUpDown();
            lblMinimumValue = new Label();
            lblEffectiveTo = new Label();
            dtpEffectiveTo = new DateTimePicker();
            lblEffectiveFrom = new Label();
            nudDisplayOrder = new NumericUpDown();
            lblDisplayOrder = new Label();
            cmbPricingBasis = new ComboBox();
            cmbTransportMethod = new ComboBox();
            cmbPriority = new ComboBox();
            txtPricingRuleName = new TextBox();
            lblCurrency = new Label();
            lblPricingBasis = new Label();
            cmbShipmentType = new ComboBox();
            lblShipmentType = new Label();
            lblTransportMethod = new Label();
            lblPriority = new Label();
            lblPricingRuleCode = new Label();
            txtPricingRuleCode = new TextBox();
            dtpEffectiveFrom = new DateTimePicker();
            chkIsActive = new CheckBox();
            tabPage1 = new TabPage();
            lblPricingRuleName = new Label();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            tlpAuditInfo.SuspendLayout();
            pnlContent.SuspendLayout();
            tabMain.SuspendLayout();
            tabPricingRules.SuspendLayout();
            tlpPricingRuleFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudAdditionalFees).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMaximumValue).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMinimumValue).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDisplayOrder).BeginInit();
            SuspendLayout();
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
            pnlHeader.Size = new Size(1200, 53);
            pnlHeader.TabIndex = 6;
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
            pnlToolbar.Location = new Point(252, 0);
            pnlToolbar.Margin = new Padding(5);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.MinimumSize = new Size(0, 48);
            pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlToolbar.AutoSize = true;
            pnlToolbar.Padding = new Padding(5);
            pnlToolbar.Size = new Size(948, 53);
            pnlToolbar.TabIndex = 2;
            pnlToolbar.WrapContents = true;
            // 
            // btnNew
            // 
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Microsoft Sans Serif", 10F);
            btnNew.Location = new Point(844, 9);
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
            btnSave.Location = new Point(746, 9);
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
            btnEdit.Location = new Point(648, 9);
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
            btnDelete.Location = new Point(550, 9);
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
            btnRefresh.Location = new Point(452, 9);
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
            btnClose.Location = new Point(354, 9);
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
            btnFirst.Location = new Point(256, 9);
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
            btnPrevious.Location = new Point(158, 9);
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
            txtCurrentRecordNo.Location = new Point(87, 10);
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
            btnNext.Location = new Point(-12, 9);
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
            btnLast.Location = new Point(-110, 9);
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
            btnUndo.Location = new Point(-208, 9);
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
            lblTitle.Padding = new Padding(0, 0, 15, 0);
            lblTitle.RightToLeft = RightToLeft.Yes;
            lblTitle.Size = new Size(252, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "قواعد تسعير الشحن";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
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
            tlpAuditInfo.Location = new Point(0, 767);
            tlpAuditInfo.Margin = new Padding(0);
            tlpAuditInfo.BackColor = Color.FromArgb(248, 250, 252);
            tlpAuditInfo.Name = "tlpAuditInfo";
            tlpAuditInfo.Padding = new Padding(8);
            tlpAuditInfo.RowCount = 1;
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAuditInfo.Size = new Size(1200, 48);
            tlpAuditInfo.TabIndex = 7;
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
            lblPrintCount.Size = new Size(138, 33);
            lblPrintCount.TabIndex = 8;
            lblPrintCount.Text = "عدد مرات الطباعة :- 00";
            lblPrintCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblLastPrintedAt
            // 
            lblLastPrintedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblLastPrintedAt.Dock = DockStyle.Fill;
            lblLastPrintedAt.Font = new Font("Segoe UI", 10F);
            lblLastPrintedAt.Location = new Point(147, 0);
            lblLastPrintedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblLastPrintedAt.Name = "lblLastPrintedAt";
            lblLastPrintedAt.Margin = new Padding(3);
            lblLastPrintedAt.AutoEllipsis = true;
            lblLastPrintedAt.RightToLeft = RightToLeft.No;
            lblLastPrintedAt.Size = new Size(186, 33);
            lblLastPrintedAt.TabIndex = 5;
            lblLastPrintedAt.Text = "تاريخ اخر طباعة:  27/09/2026";
            lblLastPrintedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblEditCount
            // 
            lblEditCount.BackColor = Color.FromArgb(192, 255, 255);
            lblEditCount.Dock = DockStyle.Fill;
            lblEditCount.Font = new Font("Segoe UI", 10F);
            lblEditCount.Location = new Point(339, 0);
            lblEditCount.ForeColor = Color.FromArgb(16, 24, 40);
            lblEditCount.Name = "lblEditCount";
            lblEditCount.Margin = new Padding(3);
            lblEditCount.AutoEllipsis = true;
            lblEditCount.RightToLeft = RightToLeft.No;
            lblEditCount.Size = new Size(138, 33);
            lblEditCount.TabIndex = 4;
            lblEditCount.Text = "عدد التعديلات :- 00";
            lblEditCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedAt
            // 
            lblModifiedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedAt.Dock = DockStyle.Fill;
            lblModifiedAt.Font = new Font("Segoe UI", 10F);
            lblModifiedAt.Location = new Point(483, 0);
            lblModifiedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblModifiedAt.Name = "lblModifiedAt";
            lblModifiedAt.Margin = new Padding(3);
            lblModifiedAt.AutoEllipsis = true;
            lblModifiedAt.RightToLeft = RightToLeft.No;
            lblModifiedAt.Size = new Size(186, 33);
            lblModifiedAt.TabIndex = 3;
            lblModifiedAt.Text = " تاريخ التعديل:  27/09/2026";
            lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedBy
            // 
            lblModifiedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedBy.Dock = DockStyle.Fill;
            lblModifiedBy.Font = new Font("Segoe UI", 10F);
            lblModifiedBy.Location = new Point(675, 0);
            lblModifiedBy.ForeColor = Color.FromArgb(16, 24, 40);
            lblModifiedBy.Name = "lblModifiedBy";
            lblModifiedBy.Margin = new Padding(3);
            lblModifiedBy.AutoEllipsis = true;
            lblModifiedBy.RightToLeft = RightToLeft.No;
            lblModifiedBy.Size = new Size(162, 33);
            lblModifiedBy.TabIndex = 2;
            lblModifiedBy.Text = "عدل بواسطة: - مدير النظام";
            lblModifiedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedAt
            // 
            lblCreatedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedAt.Dock = DockStyle.Fill;
            lblCreatedAt.Font = new Font("Segoe UI", 10F);
            lblCreatedAt.Location = new Point(843, 0);
            lblCreatedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblCreatedAt.Name = "lblCreatedAt";
            lblCreatedAt.Margin = new Padding(3);
            lblCreatedAt.AutoEllipsis = true;
            lblCreatedAt.RightToLeft = RightToLeft.No;
            lblCreatedAt.Size = new Size(186, 33);
            lblCreatedAt.TabIndex = 1;
            lblCreatedAt.Text = " تاريخ الانشاء:  27/09/2026";
            lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedBy
            // 
            lblCreatedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedBy.Dock = DockStyle.Fill;
            lblCreatedBy.Font = new Font("Segoe UI", 10F);
            lblCreatedBy.Location = new Point(1035, 0);
            lblCreatedBy.ForeColor = Color.FromArgb(16, 24, 40);
            lblCreatedBy.Name = "lblCreatedBy";
            lblCreatedBy.Margin = new Padding(3);
            lblCreatedBy.AutoEllipsis = true;
            lblCreatedBy.RightToLeft = RightToLeft.No;
            lblCreatedBy.Size = new Size(162, 33);
            lblCreatedBy.TabIndex = 0;
            lblCreatedBy.Text = " أنشأ بواسطة: - مدير النظام";
            lblCreatedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.LightCyan;
            pnlContent.Controls.Add(tabMain);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.AutoScroll = true;
            pnlContent.AutoScrollMinSize = new Size(0, 651);
            pnlContent.Location = new Point(0, 53);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1200, 651);
            pnlContent.TabIndex = 8;
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tabPricingRules);
            tabMain.Controls.Add(tabPage1);
            tabMain.Dock = DockStyle.Top;
            tabMain.Location = new Point(0, 0);
            tabMain.Margin = new Padding(0);
            tabMain.Name = "tabMain";
            tabMain.Font = new Font("Segoe UI", 9F);
            tabMain.RightToLeftLayout = true;
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1200, 508);
            tabMain.TabIndex = 0;
            tabMain.SelectedIndexChanged += tabMain_SelectedIndexChanged;
            // 
            // tabPricingRules
            // 
            tabPricingRules.Controls.Add(tlpPricingRuleFields);
            tabPricingRules.Location = new Point(4, 32);
            tabPricingRules.BackColor = Color.LightCyan;
            tabPricingRules.Padding = new Padding(3);
            tabPricingRules.Margin = new Padding(0);
            tabPricingRules.Name = "tabPricingRules";
            tabPricingRules.AutoScroll = true;
            tabPricingRules.Size = new Size(1192, 472);
            tabPricingRules.TabIndex = 2;
            tabPricingRules.Text = "قواعد التسعير";
            tabPricingRules.UseVisualStyleBackColor = false;
            // 
            // tlpPricingRuleFields
            // 
            tlpPricingRuleFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpPricingRuleFields.BackColor = Color.LightCyan;
            tlpPricingRuleFields.ColumnCount = 4;
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tlpPricingRuleFields.Controls.Add(label6, 3, 8);
            tlpPricingRuleFields.Controls.Add(label5, 2, 8);
            tlpPricingRuleFields.Controls.Add(label4, 1, 8);
            tlpPricingRuleFields.Controls.Add(label3, 0, 8);
            tlpPricingRuleFields.Controls.Add(label1, 2, 0);
            tlpPricingRuleFields.Controls.Add(cmbCurrency, 1, 3);
            tlpPricingRuleFields.Controls.Add(txtNotes, 1, 7);
            tlpPricingRuleFields.Controls.Add(lblNotes, 0, 7);
            tlpPricingRuleFields.Controls.Add(nudAdditionalFees, 3, 6);
            tlpPricingRuleFields.Controls.Add(lblAdditionalFees, 2, 6);
            tlpPricingRuleFields.Controls.Add(nudPrice, 1, 6);
            tlpPricingRuleFields.Controls.Add(lblPrice, 0, 6);
            tlpPricingRuleFields.Controls.Add(nudMaximumValue, 3, 5);
            tlpPricingRuleFields.Controls.Add(lblMaximumValue, 2, 5);
            tlpPricingRuleFields.Controls.Add(nudMinimumValue, 1, 5);
            tlpPricingRuleFields.Controls.Add(lblMinimumValue, 0, 5);
            tlpPricingRuleFields.Controls.Add(lblEffectiveTo, 2, 4);
            tlpPricingRuleFields.Controls.Add(dtpEffectiveTo, 3, 4);
            tlpPricingRuleFields.Controls.Add(lblEffectiveFrom, 0, 4);
            tlpPricingRuleFields.Controls.Add(nudDisplayOrder, 3, 3);
            tlpPricingRuleFields.Controls.Add(lblDisplayOrder, 2, 3);
            tlpPricingRuleFields.Controls.Add(cmbPricingBasis, 3, 2);
            tlpPricingRuleFields.Controls.Add(cmbTransportMethod, 1, 2);
            tlpPricingRuleFields.Controls.Add(cmbPriority, 3, 1);
            tlpPricingRuleFields.Controls.Add(txtPricingRuleName, 3, 0);
            tlpPricingRuleFields.Controls.Add(lblCurrency, 0, 3);
            tlpPricingRuleFields.Controls.Add(lblPricingBasis, 2, 2);
            tlpPricingRuleFields.Controls.Add(cmbShipmentType, 1, 1);
            tlpPricingRuleFields.Controls.Add(lblShipmentType, 0, 1);
            tlpPricingRuleFields.Controls.Add(lblTransportMethod, 0, 2);
            tlpPricingRuleFields.Controls.Add(lblPriority, 2, 1);
            tlpPricingRuleFields.Controls.Add(lblPricingRuleCode, 0, 0);
            tlpPricingRuleFields.Controls.Add(txtPricingRuleCode, 1, 0);
            tlpPricingRuleFields.Controls.Add(dtpEffectiveFrom, 1, 4);
            tlpPricingRuleFields.Controls.Add(chkIsActive, 3, 7);
            tlpPricingRuleFields.Dock = DockStyle.Top;
            tlpPricingRuleFields.Location = new Point(0, 0);
            tlpPricingRuleFields.Margin = new Padding(0);
            tlpPricingRuleFields.Name = "tlpPricingRuleFields";
            tlpPricingRuleFields.MinimumSize = new Size(0, 340);
            tlpPricingRuleFields.Padding = new Padding(0);
            tlpPricingRuleFields.RowCount = 9;
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpPricingRuleFields.Size = new Size(1192, 340);
            tlpPricingRuleFields.TabIndex = 11;
            // 
            // label6
            // 
            label6.Dock = DockStyle.Fill;
            label6.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label6.Location = new Point(13, 362);
            label6.ForeColor = Color.FromArgb(16, 24, 40);
            label6.BackColor = Color.Transparent;
            label6.Name = "label6";
            label6.Margin = new Padding(3);
            label6.AutoEllipsis = true;
            label6.Size = new Size(406, 20);
            label6.TabIndex = 104;
            label6.Text = "الأولوية";
            label6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label5.Location = new Point(425, 362);
            label5.ForeColor = Color.FromArgb(16, 24, 40);
            label5.BackColor = Color.Transparent;
            label5.Name = "label5";
            label5.Margin = new Padding(3);
            label5.AutoEllipsis = true;
            label5.Size = new Size(169, 20);
            label5.TabIndex = 103;
            label5.Text = "الأولوية";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label4.Location = new Point(600, 362);
            label4.ForeColor = Color.FromArgb(16, 24, 40);
            label4.BackColor = Color.Transparent;
            label4.Name = "label4";
            label4.Margin = new Padding(3);
            label4.AutoEllipsis = true;
            label4.Size = new Size(404, 20);
            label4.TabIndex = 102;
            label4.Text = "الأولوية";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label3.Location = new Point(1010, 362);
            label3.ForeColor = Color.FromArgb(16, 24, 40);
            label3.BackColor = Color.Transparent;
            label3.Name = "label3";
            label3.Margin = new Padding(3);
            label3.AutoEllipsis = true;
            label3.Size = new Size(169, 20);
            label3.TabIndex = 101;
            label3.Text = "الأولوية";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.Location = new Point(425, 10);
            label1.ForeColor = Color.FromArgb(16, 24, 40);
            label1.BackColor = Color.Transparent;
            label1.Name = "label1";
            label1.Margin = new Padding(3);
            label1.AutoEllipsis = true;
            label1.Size = new Size(169, 44);
            label1.TabIndex = 99;
            label1.Text = "اسم القاعدة";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbCurrency
            // 
            cmbCurrency.Dock = DockStyle.Fill;
            cmbCurrency.FormattingEnabled = true;
            cmbCurrency.Location = new Point(600, 145);
            cmbCurrency.BackColor = Color.White;
            cmbCurrency.ForeColor = Color.FromArgb(16, 24, 40);
            cmbCurrency.Name = "cmbCurrency";
            cmbCurrency.Font = new Font("Segoe UI", 11F);
            cmbCurrency.Margin = new Padding(3);
            cmbCurrency.Size = new Size(404, 31);
            cmbCurrency.TabIndex = 98;
            // 
            // txtNotes
            // 
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Location = new Point(602, 323);
            txtNotes.Margin = new Padding(3);
            txtNotes.BackColor = Color.White;
            txtNotes.ForeColor = Color.FromArgb(16, 24, 40);
            txtNotes.Name = "txtNotes";
            txtNotes.Font = new Font("Segoe UI", 11F);
            txtNotes.Size = new Size(400, 30);
            txtNotes.TabIndex = 97;
            // 
            // lblNotes
            // 
            lblNotes.Dock = DockStyle.Fill;
            lblNotes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNotes.Location = new Point(1010, 318);
            lblNotes.ForeColor = Color.FromArgb(16, 24, 40);
            lblNotes.BackColor = Color.Transparent;
            lblNotes.Name = "lblNotes";
            lblNotes.Margin = new Padding(3);
            lblNotes.AutoEllipsis = true;
            lblNotes.Size = new Size(169, 44);
            lblNotes.TabIndex = 96;
            lblNotes.Text = "ملاحظات";
            lblNotes.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudAdditionalFees
            // 
            nudAdditionalFees.Dock = DockStyle.Fill;
            nudAdditionalFees.Location = new Point(13, 277);
            nudAdditionalFees.BackColor = Color.White;
            nudAdditionalFees.ForeColor = Color.FromArgb(16, 24, 40);
            nudAdditionalFees.Name = "nudAdditionalFees";
            nudAdditionalFees.Font = new Font("Segoe UI", 11F);
            nudAdditionalFees.Margin = new Padding(3);
            nudAdditionalFees.Size = new Size(406, 30);
            nudAdditionalFees.TabIndex = 94;
            // 
            // lblAdditionalFees
            // 
            lblAdditionalFees.Dock = DockStyle.Fill;
            lblAdditionalFees.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblAdditionalFees.Location = new Point(425, 274);
            lblAdditionalFees.ForeColor = Color.FromArgb(16, 24, 40);
            lblAdditionalFees.BackColor = Color.Transparent;
            lblAdditionalFees.Name = "lblAdditionalFees";
            lblAdditionalFees.Margin = new Padding(3);
            lblAdditionalFees.AutoEllipsis = true;
            lblAdditionalFees.Size = new Size(169, 44);
            lblAdditionalFees.TabIndex = 93;
            lblAdditionalFees.Text = "رسوم اضافية";
            lblAdditionalFees.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudPrice
            // 
            nudPrice.Dock = DockStyle.Fill;
            nudPrice.Location = new Point(600, 277);
            nudPrice.BackColor = Color.White;
            nudPrice.ForeColor = Color.FromArgb(16, 24, 40);
            nudPrice.Name = "nudPrice";
            nudPrice.Font = new Font("Segoe UI", 11F);
            nudPrice.Margin = new Padding(3);
            nudPrice.Size = new Size(404, 30);
            nudPrice.TabIndex = 92;
            // 
            // lblPrice
            // 
            lblPrice.Dock = DockStyle.Fill;
            lblPrice.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPrice.Location = new Point(1010, 274);
            lblPrice.ForeColor = Color.FromArgb(16, 24, 40);
            lblPrice.BackColor = Color.Transparent;
            lblPrice.Name = "lblPrice";
            lblPrice.Margin = new Padding(3);
            lblPrice.AutoEllipsis = true;
            lblPrice.Size = new Size(169, 44);
            lblPrice.TabIndex = 91;
            lblPrice.Text = "السعر";
            lblPrice.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudMaximumValue
            // 
            nudMaximumValue.Dock = DockStyle.Fill;
            nudMaximumValue.Location = new Point(13, 233);
            nudMaximumValue.BackColor = Color.White;
            nudMaximumValue.ForeColor = Color.FromArgb(16, 24, 40);
            nudMaximumValue.Name = "nudMaximumValue";
            nudMaximumValue.Font = new Font("Segoe UI", 11F);
            nudMaximumValue.Margin = new Padding(3);
            nudMaximumValue.Size = new Size(406, 30);
            nudMaximumValue.TabIndex = 90;
            // 
            // lblMaximumValue
            // 
            lblMaximumValue.Dock = DockStyle.Fill;
            lblMaximumValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMaximumValue.Location = new Point(425, 230);
            lblMaximumValue.ForeColor = Color.FromArgb(16, 24, 40);
            lblMaximumValue.BackColor = Color.Transparent;
            lblMaximumValue.Name = "lblMaximumValue";
            lblMaximumValue.Margin = new Padding(3);
            lblMaximumValue.AutoEllipsis = true;
            lblMaximumValue.Size = new Size(169, 44);
            lblMaximumValue.TabIndex = 89;
            lblMaximumValue.Text = "الحد الأعلى";
            lblMaximumValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudMinimumValue
            // 
            nudMinimumValue.Dock = DockStyle.Fill;
            nudMinimumValue.Location = new Point(600, 233);
            nudMinimumValue.BackColor = Color.White;
            nudMinimumValue.ForeColor = Color.FromArgb(16, 24, 40);
            nudMinimumValue.Name = "nudMinimumValue";
            nudMinimumValue.Font = new Font("Segoe UI", 11F);
            nudMinimumValue.Margin = new Padding(3);
            nudMinimumValue.Size = new Size(404, 30);
            nudMinimumValue.TabIndex = 88;
            // 
            // lblMinimumValue
            // 
            lblMinimumValue.Dock = DockStyle.Fill;
            lblMinimumValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMinimumValue.Location = new Point(1010, 230);
            lblMinimumValue.ForeColor = Color.FromArgb(16, 24, 40);
            lblMinimumValue.BackColor = Color.Transparent;
            lblMinimumValue.Name = "lblMinimumValue";
            lblMinimumValue.Margin = new Padding(3);
            lblMinimumValue.AutoEllipsis = true;
            lblMinimumValue.Size = new Size(169, 44);
            lblMinimumValue.TabIndex = 87;
            lblMinimumValue.Text = "الحد الأدنى";
            lblMinimumValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEffectiveTo
            // 
            lblEffectiveTo.Dock = DockStyle.Fill;
            lblEffectiveTo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEffectiveTo.Location = new Point(425, 186);
            lblEffectiveTo.ForeColor = Color.FromArgb(16, 24, 40);
            lblEffectiveTo.BackColor = Color.Transparent;
            lblEffectiveTo.Name = "lblEffectiveTo";
            lblEffectiveTo.Margin = new Padding(3);
            lblEffectiveTo.AutoEllipsis = true;
            lblEffectiveTo.Size = new Size(169, 44);
            lblEffectiveTo.TabIndex = 85;
            lblEffectiveTo.Text = "ساري إلى";
            lblEffectiveTo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dtpEffectiveTo
            // 
            dtpEffectiveTo.Location = new Point(169, 189);
            dtpEffectiveTo.Name = "dtpEffectiveTo";
            dtpEffectiveTo.Font = new Font("Segoe UI", 11F);
            dtpEffectiveTo.Margin = new Padding(3);
            dtpEffectiveTo.Dock = DockStyle.Fill;
            dtpEffectiveTo.Size = new Size(250, 30);
            dtpEffectiveTo.TabIndex = 86;
            // 
            // lblEffectiveFrom
            // 
            lblEffectiveFrom.Dock = DockStyle.Fill;
            lblEffectiveFrom.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEffectiveFrom.Location = new Point(1010, 186);
            lblEffectiveFrom.ForeColor = Color.FromArgb(16, 24, 40);
            lblEffectiveFrom.BackColor = Color.Transparent;
            lblEffectiveFrom.Name = "lblEffectiveFrom";
            lblEffectiveFrom.Margin = new Padding(3);
            lblEffectiveFrom.AutoEllipsis = true;
            lblEffectiveFrom.Size = new Size(169, 44);
            lblEffectiveFrom.TabIndex = 83;
            lblEffectiveFrom.Text = "ساري من";
            lblEffectiveFrom.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudDisplayOrder
            // 
            nudDisplayOrder.Dock = DockStyle.Fill;
            nudDisplayOrder.Location = new Point(13, 145);
            nudDisplayOrder.BackColor = Color.White;
            nudDisplayOrder.ForeColor = Color.FromArgb(16, 24, 40);
            nudDisplayOrder.Name = "nudDisplayOrder";
            nudDisplayOrder.Font = new Font("Segoe UI", 11F);
            nudDisplayOrder.Margin = new Padding(3);
            nudDisplayOrder.Size = new Size(406, 30);
            nudDisplayOrder.TabIndex = 82;
            // 
            // lblDisplayOrder
            // 
            lblDisplayOrder.Dock = DockStyle.Fill;
            lblDisplayOrder.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDisplayOrder.Location = new Point(425, 142);
            lblDisplayOrder.ForeColor = Color.FromArgb(16, 24, 40);
            lblDisplayOrder.BackColor = Color.Transparent;
            lblDisplayOrder.Name = "lblDisplayOrder";
            lblDisplayOrder.Margin = new Padding(3);
            lblDisplayOrder.AutoEllipsis = true;
            lblDisplayOrder.Size = new Size(169, 44);
            lblDisplayOrder.TabIndex = 81;
            lblDisplayOrder.Text = "ترتيب العرض";
            lblDisplayOrder.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbPricingBasis
            // 
            cmbPricingBasis.Dock = DockStyle.Fill;
            cmbPricingBasis.FormattingEnabled = true;
            cmbPricingBasis.Items.AddRange(new object[] { "وزن", "حجم", "قطعة", "قيمة", "سعر ثابت" });
            cmbPricingBasis.Location = new Point(13, 101);
            cmbPricingBasis.BackColor = Color.White;
            cmbPricingBasis.ForeColor = Color.FromArgb(16, 24, 40);
            cmbPricingBasis.Name = "cmbPricingBasis";
            cmbPricingBasis.Font = new Font("Segoe UI", 11F);
            cmbPricingBasis.Margin = new Padding(3);
            cmbPricingBasis.Size = new Size(406, 31);
            cmbPricingBasis.TabIndex = 80;
            // 
            // cmbTransportMethod
            // 
            cmbTransportMethod.Dock = DockStyle.Fill;
            cmbTransportMethod.FormattingEnabled = true;
            cmbTransportMethod.Location = new Point(600, 101);
            cmbTransportMethod.BackColor = Color.White;
            cmbTransportMethod.ForeColor = Color.FromArgb(16, 24, 40);
            cmbTransportMethod.Name = "cmbTransportMethod";
            cmbTransportMethod.Font = new Font("Segoe UI", 11F);
            cmbTransportMethod.Margin = new Padding(3);
            cmbTransportMethod.Size = new Size(404, 31);
            cmbTransportMethod.TabIndex = 79;
            // 
            // cmbPriority
            // 
            cmbPriority.Dock = DockStyle.Fill;
            cmbPriority.FormattingEnabled = true;
            cmbPriority.Location = new Point(13, 57);
            cmbPriority.BackColor = Color.White;
            cmbPriority.ForeColor = Color.FromArgb(16, 24, 40);
            cmbPriority.Name = "cmbPriority";
            cmbPriority.Font = new Font("Segoe UI", 11F);
            cmbPriority.Margin = new Padding(3);
            cmbPriority.Size = new Size(406, 31);
            cmbPriority.TabIndex = 76;
            // 
            // txtPricingRuleName
            // 
            txtPricingRuleName.Dock = DockStyle.Fill;
            txtPricingRuleName.Location = new Point(15, 15);
            txtPricingRuleName.Margin = new Padding(3);
            txtPricingRuleName.BackColor = Color.White;
            txtPricingRuleName.ForeColor = Color.FromArgb(16, 24, 40);
            txtPricingRuleName.Name = "txtPricingRuleName";
            txtPricingRuleName.Font = new Font("Segoe UI", 11F);
            txtPricingRuleName.Size = new Size(402, 30);
            txtPricingRuleName.TabIndex = 75;
            // 
            // lblCurrency
            // 
            lblCurrency.Dock = DockStyle.Fill;
            lblCurrency.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCurrency.Location = new Point(1010, 142);
            lblCurrency.ForeColor = Color.FromArgb(16, 24, 40);
            lblCurrency.BackColor = Color.Transparent;
            lblCurrency.Name = "lblCurrency";
            lblCurrency.Margin = new Padding(3);
            lblCurrency.AutoEllipsis = true;
            lblCurrency.Size = new Size(169, 44);
            lblCurrency.TabIndex = 69;
            lblCurrency.Text = "العملة";
            lblCurrency.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPricingBasis
            // 
            lblPricingBasis.Dock = DockStyle.Fill;
            lblPricingBasis.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPricingBasis.Location = new Point(425, 98);
            lblPricingBasis.ForeColor = Color.FromArgb(16, 24, 40);
            lblPricingBasis.BackColor = Color.Transparent;
            lblPricingBasis.Name = "lblPricingBasis";
            lblPricingBasis.Margin = new Padding(3);
            lblPricingBasis.AutoEllipsis = true;
            lblPricingBasis.Size = new Size(169, 44);
            lblPricingBasis.TabIndex = 67;
            lblPricingBasis.Text = "اساس التسعير";
            lblPricingBasis.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbShipmentType
            // 
            cmbShipmentType.Dock = DockStyle.Fill;
            cmbShipmentType.FormattingEnabled = true;
            cmbShipmentType.Location = new Point(600, 57);
            cmbShipmentType.BackColor = Color.White;
            cmbShipmentType.ForeColor = Color.FromArgb(16, 24, 40);
            cmbShipmentType.Name = "cmbShipmentType";
            cmbShipmentType.Font = new Font("Segoe UI", 11F);
            cmbShipmentType.Margin = new Padding(3);
            cmbShipmentType.Size = new Size(404, 31);
            cmbShipmentType.TabIndex = 62;
            // 
            // lblShipmentType
            // 
            lblShipmentType.Dock = DockStyle.Fill;
            lblShipmentType.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblShipmentType.Location = new Point(1010, 54);
            lblShipmentType.ForeColor = Color.FromArgb(16, 24, 40);
            lblShipmentType.BackColor = Color.Transparent;
            lblShipmentType.Name = "lblShipmentType";
            lblShipmentType.Margin = new Padding(3);
            lblShipmentType.AutoEllipsis = true;
            lblShipmentType.Size = new Size(169, 44);
            lblShipmentType.TabIndex = 59;
            lblShipmentType.Text = "نوع الشحن";
            lblShipmentType.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTransportMethod
            // 
            lblTransportMethod.Dock = DockStyle.Fill;
            lblTransportMethod.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTransportMethod.Location = new Point(1010, 98);
            lblTransportMethod.ForeColor = Color.FromArgb(16, 24, 40);
            lblTransportMethod.BackColor = Color.Transparent;
            lblTransportMethod.Name = "lblTransportMethod";
            lblTransportMethod.Margin = new Padding(3);
            lblTransportMethod.AutoEllipsis = true;
            lblTransportMethod.Size = new Size(169, 44);
            lblTransportMethod.TabIndex = 53;
            lblTransportMethod.Text = "وسيلة النقل";
            lblTransportMethod.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPriority
            // 
            lblPriority.Dock = DockStyle.Fill;
            lblPriority.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPriority.Location = new Point(425, 54);
            lblPriority.ForeColor = Color.FromArgb(16, 24, 40);
            lblPriority.BackColor = Color.Transparent;
            lblPriority.Name = "lblPriority";
            lblPriority.Margin = new Padding(3);
            lblPriority.AutoEllipsis = true;
            lblPriority.Size = new Size(169, 44);
            lblPriority.TabIndex = 6;
            lblPriority.Text = "الأولوية";
            lblPriority.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPricingRuleCode
            // 
            lblPricingRuleCode.Dock = DockStyle.Fill;
            lblPricingRuleCode.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPricingRuleCode.Location = new Point(1010, 10);
            lblPricingRuleCode.ForeColor = Color.FromArgb(16, 24, 40);
            lblPricingRuleCode.BackColor = Color.Transparent;
            lblPricingRuleCode.Name = "lblPricingRuleCode";
            lblPricingRuleCode.Margin = new Padding(3);
            lblPricingRuleCode.AutoEllipsis = true;
            lblPricingRuleCode.Size = new Size(169, 44);
            lblPricingRuleCode.TabIndex = 0;
            lblPricingRuleCode.Text = "كود قاعدة التسعير";
            lblPricingRuleCode.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPricingRuleCode
            // 
            txtPricingRuleCode.Dock = DockStyle.Fill;
            txtPricingRuleCode.Location = new Point(722, 15);
            txtPricingRuleCode.Margin = new Padding(3);
            txtPricingRuleCode.BackColor = Color.White;
            txtPricingRuleCode.ForeColor = Color.FromArgb(16, 24, 40);
            txtPricingRuleCode.Name = "txtPricingRuleCode";
            txtPricingRuleCode.Font = new Font("Segoe UI", 11F);
            txtPricingRuleCode.Size = new Size(280, 30);
            txtPricingRuleCode.TabIndex = 1;
            // 
            // dtpEffectiveFrom
            // 
            dtpEffectiveFrom.Location = new Point(754, 189);
            dtpEffectiveFrom.Name = "dtpEffectiveFrom";
            dtpEffectiveFrom.Font = new Font("Segoe UI", 11F);
            dtpEffectiveFrom.Margin = new Padding(3);
            dtpEffectiveFrom.Dock = DockStyle.Fill;
            dtpEffectiveFrom.Size = new Size(250, 30);
            dtpEffectiveFrom.TabIndex = 84;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Font = new Font("Segoe UI", 10F);
            chkIsActive.Location = new Point(349, 321);
            chkIsActive.BackColor = Color.Transparent;
            chkIsActive.ForeColor = Color.FromArgb(16, 24, 40);
            chkIsActive.Dock = DockStyle.Fill;
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Margin = new Padding(3);
            chkIsActive.Size = new Size(70, 29);
            chkIsActive.TabIndex = 95;
            chkIsActive.Text = "نشط";
            chkIsActive.UseVisualStyleBackColor = false;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 32);
            tabPage1.BackColor = Color.LightCyan;
            tabPage1.Margin = new Padding(0);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1192, 472);
            tabPage1.TabIndex = 3;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = false;
            // 
            // lblPricingRuleName
            // 
            lblPricingRuleName.Dock = DockStyle.Fill;
            lblPricingRuleName.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblPricingRuleName.Location = new Point(425, 10);
            lblPricingRuleName.ForeColor = Color.FromArgb(16, 24, 40);
            lblPricingRuleName.BackColor = Color.Transparent;
            lblPricingRuleName.Name = "lblPricingRuleName";
            lblPricingRuleName.Size = new Size(169, 44);
            lblPricingRuleName.TabIndex = 2;
            lblPricingRuleName.Text = "اسم قاعدة التسعير";
            lblPricingRuleName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // UcShipmentPricingRules
            // 
            AccessibleName = "قواعد تسعير الشحن";
            AutoScaleDimensions = new SizeF(120F, 120F);
            ForeColor = Color.FromArgb(16, 24, 40);
            Padding = new Padding(0);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(248, 250, 252);
            Controls.Add(pnlContent);
            Controls.Add(tlpAuditInfo);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 10F);
            Margin = new Padding(0);
            MinimumSize = new Size(1000, 650);
            Name = "UcShipmentPricingRules";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1200, 800);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            tlpAuditInfo.ResumeLayout(false);
            pnlContent.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            tabPricingRules.ResumeLayout(false);
            tlpPricingRuleFields.ResumeLayout(false);
            tlpPricingRuleFields.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudAdditionalFees).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMaximumValue).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMinimumValue).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDisplayOrder).EndInit();
            ResumeLayout(false);
        
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(252, 0);
        designerCommandBar.Size = new Size(948, 53);
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
        private TableLayoutPanel tlpAuditInfo;
        private Label lblPrintCount;
        private Label lblLastPrintedAt;
        private Label lblEditCount;
        private Label lblModifiedAt;
        private Label lblModifiedBy;
        private Label lblCreatedAt;
        private Label lblCreatedBy;
        private Panel pnlContent;
        private TabControl tabMain;
        private TabPage tabPricingRules;
        private TableLayoutPanel tlpPricingRuleFields;
        private Label lblPricingRuleCode;
        private TextBox txtPricingRuleCode;
        private Label lblPricingRuleName;
        private TextBox txtPricingRuleName;
        private Label lblShipmentType;
        private ComboBox cmbShipmentType;
        private Label lblPriority;
        private ComboBox cmbPriority;
        private Label lblTransportMethod;
        private ComboBox cmbTransportMethod;
        private Label lblPricingBasis;
        private ComboBox cmbPricingBasis;
        private Label lblCurrency;
        private ComboBox cmbCurrency;
        private Label lblDisplayOrder;
        private NumericUpDown nudDisplayOrder;
        private Label lblEffectiveFrom;
        private DateTimePicker dtpEffectiveFrom;
        private Label lblEffectiveTo;
        private DateTimePicker dtpEffectiveTo;
        private Label lblMinimumValue;
        private NumericUpDown nudMinimumValue;
        private Label lblMaximumValue;
        private NumericUpDown nudMaximumValue;
        private Label lblPrice;
        private NumericUpDown nudPrice;
        private Label lblAdditionalFees;
        private NumericUpDown nudAdditionalFees;
        private Label lblNotes;
        private TextBox txtNotes;
        private CheckBox chkIsActive;
        private TabPage tabPage1;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label1;
    }
}
