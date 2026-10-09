namespace TransportERP.Desktop.البوالص_والشحن.الاعدادات.الفروع
{
    partial class dgvShipmentServiceFees
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
            pnlContent = new Panel();
            tabMain = new TabControl();
            tabPricingRules = new TabPage();
            tlpPricingRuleFields = new TableLayoutPanel();
            chkAllowManualEdit = new CheckBox();
            nudMaximumFee = new NumericUpDown();
            nudMinimumFee = new NumericUpDown();
            dtpEffectiveTo = new DateTimePicker();
            dtpEffectiveFrom = new DateTimePicker();
            label14 = new Label();
            txtNotes = new TextBox();
            txtDescription = new TextBox();
            nudDisplayOrder = new NumericUpDown();
            cmbFeeBasis = new ComboBox();
            label11 = new Label();
            label10 = new Label();
            cmbTransportMethod = new ComboBox();
            label9 = new Label();
            cmbPriority = new ComboBox();
            cmbShipmentType = new ComboBox();
            label8 = new Label();
            nudPercentage = new NumericUpDown();
            nudAmount = new NumericUpDown();
            txtServiceFeeNameEn = new TextBox();
            label7 = new Label();
            label2 = new Label();
            label3 = new Label();
            label1 = new Label();
            cmbShipmentCategory = new ComboBox();
            lblNotes = new Label();
            lblMaximumValue = new Label();
            lblMinimumValue = new Label();
            lblEffectiveFrom = new Label();
            lblDisplayOrder = new Label();
            cmbCurrency = new ComboBox();
            cmbCalculationMethod = new ComboBox();
            txtServiceFeeNameAr = new TextBox();
            lblCurrency = new Label();
            lblPricingBasis = new Label();
            cmbFeeType = new ComboBox();
            lblShipmentType = new Label();
            lblTransportMethod = new Label();
            lblPriority = new Label();
            lblPricingRuleCode = new Label();
            txtServiceFeeCode = new TextBox();
            chkIsActive = new CheckBox();
            chkIsMandatory = new CheckBox();
            chkIncludeInShipmentTotal = new CheckBox();
            chkShowInPrint = new CheckBox();
            tabPage1 = new TabPage();
            tlpAuditInfo.SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            pnlContent.SuspendLayout();
            tabMain.SuspendLayout();
            tabPricingRules.SuspendLayout();
            tlpPricingRuleFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudMaximumFee).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMinimumFee).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDisplayOrder).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPercentage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAmount).BeginInit();
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
            tlpAuditInfo.Location = new Point(0, 756);
            tlpAuditInfo.Margin = new Padding(0);
            tlpAuditInfo.BackColor = Color.FromArgb(248, 250, 252);
            tlpAuditInfo.Name = "tlpAuditInfo";
            tlpAuditInfo.Padding = new Padding(8);
            tlpAuditInfo.RowCount = 1;
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAuditInfo.Size = new Size(1336, 48);
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
            lblPrintCount.Size = new Size(157, 33);
            lblPrintCount.TabIndex = 8;
            lblPrintCount.Text = "عدد مرات الطباعة :- 00";
            lblPrintCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblLastPrintedAt
            // 
            lblLastPrintedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblLastPrintedAt.Dock = DockStyle.Fill;
            lblLastPrintedAt.Font = new Font("Segoe UI", 10F);
            lblLastPrintedAt.Location = new Point(166, 0);
            lblLastPrintedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblLastPrintedAt.Name = "lblLastPrintedAt";
            lblLastPrintedAt.Margin = new Padding(3);
            lblLastPrintedAt.AutoEllipsis = true;
            lblLastPrintedAt.RightToLeft = RightToLeft.No;
            lblLastPrintedAt.Size = new Size(207, 33);
            lblLastPrintedAt.TabIndex = 5;
            lblLastPrintedAt.Text = "تاريخ اخر طباعة:  27/09/2026";
            lblLastPrintedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblEditCount
            // 
            lblEditCount.BackColor = Color.FromArgb(192, 255, 255);
            lblEditCount.Dock = DockStyle.Fill;
            lblEditCount.Font = new Font("Segoe UI", 10F);
            lblEditCount.Location = new Point(379, 0);
            lblEditCount.ForeColor = Color.FromArgb(16, 24, 40);
            lblEditCount.Name = "lblEditCount";
            lblEditCount.Margin = new Padding(3);
            lblEditCount.AutoEllipsis = true;
            lblEditCount.RightToLeft = RightToLeft.No;
            lblEditCount.Size = new Size(154, 33);
            lblEditCount.TabIndex = 4;
            lblEditCount.Text = "عدد التعديلات :- 00";
            lblEditCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedAt
            // 
            lblModifiedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedAt.Dock = DockStyle.Fill;
            lblModifiedAt.Font = new Font("Segoe UI", 10F);
            lblModifiedAt.Location = new Point(539, 0);
            lblModifiedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblModifiedAt.Name = "lblModifiedAt";
            lblModifiedAt.Margin = new Padding(3);
            lblModifiedAt.AutoEllipsis = true;
            lblModifiedAt.RightToLeft = RightToLeft.No;
            lblModifiedAt.Size = new Size(207, 33);
            lblModifiedAt.TabIndex = 3;
            lblModifiedAt.Text = " تاريخ التعديل:  27/09/2026";
            lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedBy
            // 
            lblModifiedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedBy.Dock = DockStyle.Fill;
            lblModifiedBy.Font = new Font("Segoe UI", 10F);
            lblModifiedBy.Location = new Point(752, 0);
            lblModifiedBy.ForeColor = Color.FromArgb(16, 24, 40);
            lblModifiedBy.Name = "lblModifiedBy";
            lblModifiedBy.Margin = new Padding(3);
            lblModifiedBy.AutoEllipsis = true;
            lblModifiedBy.RightToLeft = RightToLeft.No;
            lblModifiedBy.Size = new Size(181, 33);
            lblModifiedBy.TabIndex = 2;
            lblModifiedBy.Text = "عدل بواسطة: - مدير النظام";
            lblModifiedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedAt
            // 
            lblCreatedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedAt.Dock = DockStyle.Fill;
            lblCreatedAt.Font = new Font("Segoe UI", 10F);
            lblCreatedAt.Location = new Point(939, 0);
            lblCreatedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblCreatedAt.Name = "lblCreatedAt";
            lblCreatedAt.Margin = new Padding(3);
            lblCreatedAt.AutoEllipsis = true;
            lblCreatedAt.RightToLeft = RightToLeft.No;
            lblCreatedAt.Size = new Size(207, 33);
            lblCreatedAt.TabIndex = 1;
            lblCreatedAt.Text = " تاريخ الانشاء:  27/09/2026";
            lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedBy
            // 
            lblCreatedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedBy.Dock = DockStyle.Fill;
            lblCreatedBy.Font = new Font("Segoe UI", 10F);
            lblCreatedBy.Location = new Point(1152, 0);
            lblCreatedBy.ForeColor = Color.FromArgb(16, 24, 40);
            lblCreatedBy.Name = "lblCreatedBy";
            lblCreatedBy.Margin = new Padding(3);
            lblCreatedBy.AutoEllipsis = true;
            lblCreatedBy.RightToLeft = RightToLeft.No;
            lblCreatedBy.Size = new Size(181, 33);
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
            pnlHeader.Size = new Size(1336, 53);
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
            pnlToolbar.Location = new Point(273, 0);
            pnlToolbar.Margin = new Padding(5);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.MinimumSize = new Size(0, 48);
            pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlToolbar.AutoSize = true;
            pnlToolbar.Padding = new Padding(5);
            pnlToolbar.Size = new Size(1063, 53);
            pnlToolbar.TabIndex = 2;
            pnlToolbar.WrapContents = true;
            // 
            // btnNew
            // 
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Microsoft Sans Serif", 10F);
            btnNew.Location = new Point(959, 9);
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
            btnSave.Location = new Point(861, 9);
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
            btnEdit.Location = new Point(763, 9);
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
            btnDelete.Location = new Point(665, 9);
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
            btnRefresh.Location = new Point(567, 9);
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
            btnClose.Location = new Point(469, 9);
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
            btnFirst.Location = new Point(371, 9);
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
            btnPrevious.Location = new Point(273, 9);
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
            txtCurrentRecordNo.Location = new Point(202, 10);
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
            btnNext.Location = new Point(103, 9);
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
            btnLast.Location = new Point(5, 9);
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
            btnUndo.Location = new Point(-93, 9);
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
            lblTitle.Size = new Size(273, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "خدمات ورسوم الشحن";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.LightCyan;
            pnlContent.Controls.Add(tabMain);
            pnlContent.Dock = DockStyle.Top;
            pnlContent.Location = new Point(0, 53);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1336, 566);
            pnlContent.TabIndex = 10;
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
            tabMain.Size = new Size(1336, 508);
            tabMain.TabIndex = 0;
            // 
            // tabPricingRules
            // 
            tabPricingRules.Controls.Add(tlpPricingRuleFields);
            tabPricingRules.Location = new Point(4, 29);
            tabPricingRules.BackColor = Color.LightCyan;
            tabPricingRules.Padding = new Padding(3);
            tabPricingRules.Margin = new Padding(0);
            tabPricingRules.Name = "tabPricingRules";
            tabPricingRules.AutoScroll = true;
            tabPricingRules.Size = new Size(1328, 475);
            tabPricingRules.TabIndex = 2;
            tabPricingRules.Text = "خدمات ورسوم الشحن";
            tabPricingRules.UseVisualStyleBackColor = false;
            // 
            // tlpPricingRuleFields
            // 
            tlpPricingRuleFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpPricingRuleFields.BackColor = Color.LightCyan;
            tlpPricingRuleFields.ColumnCount = 6;
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.1111107F));
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22.2222233F));
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.1111116F));
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22.2222233F));
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.1111116F));
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22.2222233F));
            tlpPricingRuleFields.Controls.Add(chkAllowManualEdit, 1, 6);
            tlpPricingRuleFields.Controls.Add(nudMaximumFee, 5, 4);
            tlpPricingRuleFields.Controls.Add(nudMinimumFee, 3, 4);
            tlpPricingRuleFields.Controls.Add(dtpEffectiveTo, 3, 5);
            tlpPricingRuleFields.Controls.Add(dtpEffectiveFrom, 1, 5);
            tlpPricingRuleFields.Controls.Add(label14, 4, 7);
            tlpPricingRuleFields.Controls.Add(txtNotes, 1, 8);
            tlpPricingRuleFields.Controls.Add(txtDescription, 5, 7);
            tlpPricingRuleFields.Controls.Add(nudDisplayOrder, 1, 7);
            tlpPricingRuleFields.Controls.Add(cmbFeeBasis, 1, 4);
            tlpPricingRuleFields.Controls.Add(label11, 2, 4);
            tlpPricingRuleFields.Controls.Add(label10, 4, 4);
            tlpPricingRuleFields.Controls.Add(cmbTransportMethod, 5, 3);
            tlpPricingRuleFields.Controls.Add(label9, 4, 3);
            tlpPricingRuleFields.Controls.Add(cmbPriority, 3, 3);
            tlpPricingRuleFields.Controls.Add(cmbShipmentType, 5, 2);
            tlpPricingRuleFields.Controls.Add(label8, 4, 2);
            tlpPricingRuleFields.Controls.Add(nudPercentage, 1, 2);
            tlpPricingRuleFields.Controls.Add(nudAmount, 5, 1);
            tlpPricingRuleFields.Controls.Add(txtServiceFeeNameEn, 5, 0);
            tlpPricingRuleFields.Controls.Add(label7, 4, 1);
            tlpPricingRuleFields.Controls.Add(label2, 4, 0);
            tlpPricingRuleFields.Controls.Add(label3, 0, 8);
            tlpPricingRuleFields.Controls.Add(label1, 2, 0);
            tlpPricingRuleFields.Controls.Add(cmbShipmentCategory, 1, 3);
            tlpPricingRuleFields.Controls.Add(lblNotes, 0, 7);
            tlpPricingRuleFields.Controls.Add(lblMaximumValue, 2, 5);
            tlpPricingRuleFields.Controls.Add(lblMinimumValue, 0, 5);
            tlpPricingRuleFields.Controls.Add(lblEffectiveFrom, 0, 4);
            tlpPricingRuleFields.Controls.Add(lblDisplayOrder, 2, 3);
            tlpPricingRuleFields.Controls.Add(cmbCurrency, 3, 2);
            tlpPricingRuleFields.Controls.Add(cmbCalculationMethod, 3, 1);
            tlpPricingRuleFields.Controls.Add(txtServiceFeeNameAr, 3, 0);
            tlpPricingRuleFields.Controls.Add(lblCurrency, 0, 3);
            tlpPricingRuleFields.Controls.Add(lblPricingBasis, 2, 2);
            tlpPricingRuleFields.Controls.Add(cmbFeeType, 1, 1);
            tlpPricingRuleFields.Controls.Add(lblShipmentType, 0, 1);
            tlpPricingRuleFields.Controls.Add(lblTransportMethod, 0, 2);
            tlpPricingRuleFields.Controls.Add(lblPriority, 2, 1);
            tlpPricingRuleFields.Controls.Add(lblPricingRuleCode, 0, 0);
            tlpPricingRuleFields.Controls.Add(txtServiceFeeCode, 1, 0);
            tlpPricingRuleFields.Controls.Add(chkIsActive, 3, 7);
            tlpPricingRuleFields.Controls.Add(chkIsMandatory, 5, 5);
            tlpPricingRuleFields.Controls.Add(chkIncludeInShipmentTotal, 3, 6);
            tlpPricingRuleFields.Controls.Add(chkShowInPrint, 5, 6);
            tlpPricingRuleFields.Dock = DockStyle.Top;
            tlpPricingRuleFields.Location = new Point(0, 0);
            tlpPricingRuleFields.Margin = new Padding(0);
            tlpPricingRuleFields.Name = "tlpPricingRuleFields";
            tlpPricingRuleFields.MinimumSize = new Size(0, 424);
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
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            tlpPricingRuleFields.Size = new Size(1328, 424);
            tlpPricingRuleFields.TabIndex = 11;
            tlpPricingRuleFields.Paint += tlpPricingRuleFields_Paint;
            // 
            // chkAllowManualEdit
            // 
            chkAllowManualEdit.AutoSize = true;
            chkAllowManualEdit.Font = new Font("Segoe UI", 10F);
            chkAllowManualEdit.Location = new Point(984, 253);
            chkAllowManualEdit.BackColor = Color.Transparent;
            chkAllowManualEdit.ForeColor = Color.FromArgb(16, 24, 40);
            chkAllowManualEdit.Dock = DockStyle.Fill;
            chkAllowManualEdit.Name = "chkAllowManualEdit";
            chkAllowManualEdit.Margin = new Padding(3);
            chkAllowManualEdit.Size = new Size(186, 29);
            chkAllowManualEdit.TabIndex = 148;
            chkAllowManualEdit.Text = "يسمح بالتعديل يدوي";
            chkAllowManualEdit.UseVisualStyleBackColor = false;
            // 
            // nudMaximumFee
            // 
            nudMaximumFee.Dock = DockStyle.Fill;
            nudMaximumFee.Location = new Point(13, 173);
            nudMaximumFee.BackColor = Color.White;
            nudMaximumFee.ForeColor = Color.FromArgb(16, 24, 40);
            nudMaximumFee.Name = "nudMaximumFee";
            nudMaximumFee.Font = new Font("Segoe UI", 11F);
            nudMaximumFee.Margin = new Padding(3);
            nudMaximumFee.Size = new Size(287, 27);
            nudMaximumFee.TabIndex = 144;
            // 
            // nudMinimumFee
            // 
            nudMinimumFee.Dock = DockStyle.Fill;
            nudMinimumFee.Location = new Point(451, 173);
            nudMinimumFee.BackColor = Color.White;
            nudMinimumFee.ForeColor = Color.FromArgb(16, 24, 40);
            nudMinimumFee.Name = "nudMinimumFee";
            nudMinimumFee.Font = new Font("Segoe UI", 11F);
            nudMinimumFee.Margin = new Padding(3);
            nudMinimumFee.Size = new Size(284, 27);
            nudMinimumFee.TabIndex = 143;
            // 
            // dtpEffectiveTo
            // 
            dtpEffectiveTo.Location = new Point(485, 213);
            dtpEffectiveTo.Name = "dtpEffectiveTo";
            dtpEffectiveTo.Font = new Font("Segoe UI", 11F);
            dtpEffectiveTo.Margin = new Padding(3);
            dtpEffectiveTo.Dock = DockStyle.Fill;
            dtpEffectiveTo.Size = new Size(250, 27);
            dtpEffectiveTo.TabIndex = 142;
            // 
            // dtpEffectiveFrom
            // 
            dtpEffectiveFrom.Location = new Point(920, 213);
            dtpEffectiveFrom.Name = "dtpEffectiveFrom";
            dtpEffectiveFrom.Font = new Font("Segoe UI", 11F);
            dtpEffectiveFrom.Margin = new Padding(3);
            dtpEffectiveFrom.Dock = DockStyle.Fill;
            dtpEffectiveFrom.Size = new Size(250, 27);
            dtpEffectiveFrom.TabIndex = 141;
            // 
            // label14
            // 
            label14.Dock = DockStyle.Fill;
            label14.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label14.Location = new Point(306, 290);
            label14.ForeColor = Color.FromArgb(16, 24, 40);
            label14.BackColor = Color.Transparent;
            label14.Name = "label14";
            label14.Margin = new Padding(3);
            label14.AutoEllipsis = true;
            label14.Size = new Size(139, 40);
            label14.TabIndex = 140;
            label14.Text = "الوصف";
            label14.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNotes
            // 
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Location = new Point(888, 335);
            txtNotes.Margin = new Padding(3);
            txtNotes.Multiline = true;
            txtNotes.BackColor = Color.White;
            txtNotes.ForeColor = Color.FromArgb(16, 24, 40);
            txtNotes.Name = "txtNotes";
            txtNotes.Font = new Font("Segoe UI", 11F);
            txtNotes.Size = new Size(280, 34);
            txtNotes.TabIndex = 135;
            // 
            // txtDescription
            // 
            txtDescription.Dock = DockStyle.Fill;
            txtDescription.Location = new Point(18, 295);
            txtDescription.Margin = new Padding(3);
            txtDescription.Multiline = true;
            txtDescription.BackColor = Color.White;
            txtDescription.ForeColor = Color.FromArgb(16, 24, 40);
            txtDescription.Name = "txtDescription";
            txtDescription.Font = new Font("Segoe UI", 11F);
            txtDescription.Size = new Size(280, 30);
            txtDescription.TabIndex = 134;
            // 
            // nudDisplayOrder
            // 
            nudDisplayOrder.Dock = DockStyle.Fill;
            nudDisplayOrder.Location = new Point(886, 293);
            nudDisplayOrder.BackColor = Color.White;
            nudDisplayOrder.ForeColor = Color.FromArgb(16, 24, 40);
            nudDisplayOrder.Name = "nudDisplayOrder";
            nudDisplayOrder.Font = new Font("Segoe UI", 11F);
            nudDisplayOrder.Margin = new Padding(3);
            nudDisplayOrder.Size = new Size(284, 27);
            nudDisplayOrder.TabIndex = 131;
            // 
            // cmbFeeBasis
            // 
            cmbFeeBasis.Dock = DockStyle.Fill;
            cmbFeeBasis.FormattingEnabled = true;
            cmbFeeBasis.Location = new Point(886, 173);
            cmbFeeBasis.BackColor = Color.White;
            cmbFeeBasis.ForeColor = Color.FromArgb(16, 24, 40);
            cmbFeeBasis.Name = "cmbFeeBasis";
            cmbFeeBasis.Font = new Font("Segoe UI", 11F);
            cmbFeeBasis.Margin = new Padding(3);
            cmbFeeBasis.Size = new Size(284, 28);
            cmbFeeBasis.TabIndex = 122;
            // 
            // label11
            // 
            label11.Dock = DockStyle.Fill;
            label11.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label11.Location = new Point(741, 170);
            label11.ForeColor = Color.FromArgb(16, 24, 40);
            label11.BackColor = Color.Transparent;
            label11.Name = "label11";
            label11.Margin = new Padding(3);
            label11.AutoEllipsis = true;
            label11.Size = new Size(139, 40);
            label11.TabIndex = 121;
            label11.Text = "الحد الادنى للرسم";
            label11.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            label10.Dock = DockStyle.Fill;
            label10.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label10.Location = new Point(306, 170);
            label10.ForeColor = Color.FromArgb(16, 24, 40);
            label10.BackColor = Color.Transparent;
            label10.Name = "label10";
            label10.Margin = new Padding(3);
            label10.AutoEllipsis = true;
            label10.Size = new Size(139, 40);
            label10.TabIndex = 119;
            label10.Text = "الحد الاعلى للرسم";
            label10.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbTransportMethod
            // 
            cmbTransportMethod.Dock = DockStyle.Fill;
            cmbTransportMethod.FormattingEnabled = true;
            cmbTransportMethod.Items.AddRange(new object[] { "وزن", "حجم", "قطعة", "قيمة", "سعر ثابت" });
            cmbTransportMethod.Location = new Point(13, 133);
            cmbTransportMethod.BackColor = Color.White;
            cmbTransportMethod.ForeColor = Color.FromArgb(16, 24, 40);
            cmbTransportMethod.Name = "cmbTransportMethod";
            cmbTransportMethod.Font = new Font("Segoe UI", 11F);
            cmbTransportMethod.Margin = new Padding(3);
            cmbTransportMethod.Size = new Size(287, 28);
            cmbTransportMethod.TabIndex = 118;
            // 
            // label9
            // 
            label9.Dock = DockStyle.Fill;
            label9.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label9.Location = new Point(306, 130);
            label9.ForeColor = Color.FromArgb(16, 24, 40);
            label9.BackColor = Color.Transparent;
            label9.Name = "label9";
            label9.Margin = new Padding(3);
            label9.AutoEllipsis = true;
            label9.Size = new Size(139, 40);
            label9.TabIndex = 117;
            label9.Text = "وسيلة النقل";
            label9.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbPriority
            // 
            cmbPriority.Dock = DockStyle.Fill;
            cmbPriority.FormattingEnabled = true;
            cmbPriority.Items.AddRange(new object[] { "وزن", "حجم", "قطعة", "قيمة", "سعر ثابت" });
            cmbPriority.Location = new Point(451, 133);
            cmbPriority.BackColor = Color.White;
            cmbPriority.ForeColor = Color.FromArgb(16, 24, 40);
            cmbPriority.Name = "cmbPriority";
            cmbPriority.Font = new Font("Segoe UI", 11F);
            cmbPriority.Margin = new Padding(3);
            cmbPriority.Size = new Size(284, 28);
            cmbPriority.TabIndex = 116;
            // 
            // cmbShipmentType
            // 
            cmbShipmentType.Dock = DockStyle.Fill;
            cmbShipmentType.FormattingEnabled = true;
            cmbShipmentType.Items.AddRange(new object[] { "وزن", "حجم", "قطعة", "قيمة", "سعر ثابت" });
            cmbShipmentType.Location = new Point(13, 93);
            cmbShipmentType.BackColor = Color.White;
            cmbShipmentType.ForeColor = Color.FromArgb(16, 24, 40);
            cmbShipmentType.Name = "cmbShipmentType";
            cmbShipmentType.Font = new Font("Segoe UI", 11F);
            cmbShipmentType.Margin = new Padding(3);
            cmbShipmentType.Size = new Size(287, 28);
            cmbShipmentType.TabIndex = 115;
            // 
            // label8
            // 
            label8.Dock = DockStyle.Fill;
            label8.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label8.Location = new Point(306, 90);
            label8.ForeColor = Color.FromArgb(16, 24, 40);
            label8.BackColor = Color.Transparent;
            label8.Name = "label8";
            label8.Margin = new Padding(3);
            label8.AutoEllipsis = true;
            label8.Size = new Size(139, 40);
            label8.TabIndex = 114;
            label8.Text = "نوع الشحن";
            label8.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudPercentage
            // 
            nudPercentage.Dock = DockStyle.Fill;
            nudPercentage.Location = new Point(886, 93);
            nudPercentage.BackColor = Color.White;
            nudPercentage.ForeColor = Color.FromArgb(16, 24, 40);
            nudPercentage.Name = "nudPercentage";
            nudPercentage.Font = new Font("Segoe UI", 11F);
            nudPercentage.Margin = new Padding(3);
            nudPercentage.Size = new Size(284, 27);
            nudPercentage.TabIndex = 113;
            // 
            // nudAmount
            // 
            nudAmount.Dock = DockStyle.Fill;
            nudAmount.Location = new Point(13, 53);
            nudAmount.BackColor = Color.White;
            nudAmount.ForeColor = Color.FromArgb(16, 24, 40);
            nudAmount.Name = "nudAmount";
            nudAmount.Font = new Font("Segoe UI", 11F);
            nudAmount.Margin = new Padding(3);
            nudAmount.Size = new Size(287, 27);
            nudAmount.TabIndex = 112;
            // 
            // txtServiceFeeNameEn
            // 
            txtServiceFeeNameEn.Dock = DockStyle.Fill;
            txtServiceFeeNameEn.Location = new Point(15, 15);
            txtServiceFeeNameEn.Margin = new Padding(3);
            txtServiceFeeNameEn.BackColor = Color.White;
            txtServiceFeeNameEn.ForeColor = Color.FromArgb(16, 24, 40);
            txtServiceFeeNameEn.Name = "txtServiceFeeNameEn";
            txtServiceFeeNameEn.Font = new Font("Segoe UI", 11F);
            txtServiceFeeNameEn.Size = new Size(283, 27);
            txtServiceFeeNameEn.TabIndex = 111;
            // 
            // label7
            // 
            label7.Dock = DockStyle.Fill;
            label7.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label7.Location = new Point(306, 50);
            label7.ForeColor = Color.FromArgb(16, 24, 40);
            label7.BackColor = Color.Transparent;
            label7.Name = "label7";
            label7.Margin = new Padding(3);
            label7.AutoEllipsis = true;
            label7.Size = new Size(139, 40);
            label7.TabIndex = 110;
            label7.Text = "القيمة / مبلغ الرسم";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label2.Location = new Point(306, 10);
            label2.ForeColor = Color.FromArgb(16, 24, 40);
            label2.BackColor = Color.Transparent;
            label2.Name = "label2";
            label2.Margin = new Padding(3);
            label2.AutoEllipsis = true;
            label2.Size = new Size(139, 40);
            label2.TabIndex = 106;
            label2.Text = "اسم الخدمة انجليزي";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label3.Location = new Point(1176, 330);
            label3.ForeColor = Color.FromArgb(16, 24, 40);
            label3.BackColor = Color.Transparent;
            label3.Name = "label3";
            label3.Margin = new Padding(3);
            label3.AutoEllipsis = true;
            label3.Size = new Size(139, 44);
            label3.TabIndex = 101;
            label3.Text = "ملاحظات";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.Location = new Point(741, 10);
            label1.ForeColor = Color.FromArgb(16, 24, 40);
            label1.BackColor = Color.Transparent;
            label1.Name = "label1";
            label1.Margin = new Padding(3);
            label1.AutoEllipsis = true;
            label1.Size = new Size(139, 40);
            label1.TabIndex = 99;
            label1.Text = "اسم الخدمة عربي";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbShipmentCategory
            // 
            cmbShipmentCategory.Dock = DockStyle.Fill;
            cmbShipmentCategory.FormattingEnabled = true;
            cmbShipmentCategory.Location = new Point(886, 133);
            cmbShipmentCategory.BackColor = Color.White;
            cmbShipmentCategory.ForeColor = Color.FromArgb(16, 24, 40);
            cmbShipmentCategory.Name = "cmbShipmentCategory";
            cmbShipmentCategory.Font = new Font("Segoe UI", 11F);
            cmbShipmentCategory.Margin = new Padding(3);
            cmbShipmentCategory.Size = new Size(284, 28);
            cmbShipmentCategory.TabIndex = 98;
            // 
            // lblNotes
            // 
            lblNotes.Dock = DockStyle.Fill;
            lblNotes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNotes.Location = new Point(1176, 290);
            lblNotes.ForeColor = Color.FromArgb(16, 24, 40);
            lblNotes.BackColor = Color.Transparent;
            lblNotes.Name = "lblNotes";
            lblNotes.Margin = new Padding(3);
            lblNotes.AutoEllipsis = true;
            lblNotes.Size = new Size(139, 40);
            lblNotes.TabIndex = 96;
            lblNotes.Text = "ترتيب العرض";
            lblNotes.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMaximumValue
            // 
            lblMaximumValue.Dock = DockStyle.Fill;
            lblMaximumValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMaximumValue.Location = new Point(741, 210);
            lblMaximumValue.ForeColor = Color.FromArgb(16, 24, 40);
            lblMaximumValue.BackColor = Color.Transparent;
            lblMaximumValue.Name = "lblMaximumValue";
            lblMaximumValue.Margin = new Padding(3);
            lblMaximumValue.AutoEllipsis = true;
            lblMaximumValue.Size = new Size(139, 40);
            lblMaximumValue.TabIndex = 89;
            lblMaximumValue.Text = "ساري الى";
            lblMaximumValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMinimumValue
            // 
            lblMinimumValue.Dock = DockStyle.Fill;
            lblMinimumValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMinimumValue.Location = new Point(1176, 210);
            lblMinimumValue.ForeColor = Color.FromArgb(16, 24, 40);
            lblMinimumValue.BackColor = Color.Transparent;
            lblMinimumValue.Name = "lblMinimumValue";
            lblMinimumValue.Margin = new Padding(3);
            lblMinimumValue.AutoEllipsis = true;
            lblMinimumValue.Size = new Size(139, 40);
            lblMinimumValue.TabIndex = 87;
            lblMinimumValue.Text = "ساري من";
            lblMinimumValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEffectiveFrom
            // 
            lblEffectiveFrom.Dock = DockStyle.Fill;
            lblEffectiveFrom.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEffectiveFrom.Location = new Point(1176, 170);
            lblEffectiveFrom.ForeColor = Color.FromArgb(16, 24, 40);
            lblEffectiveFrom.BackColor = Color.Transparent;
            lblEffectiveFrom.Name = "lblEffectiveFrom";
            lblEffectiveFrom.Margin = new Padding(3);
            lblEffectiveFrom.AutoEllipsis = true;
            lblEffectiveFrom.Size = new Size(139, 40);
            lblEffectiveFrom.TabIndex = 83;
            lblEffectiveFrom.Text = "اساس احتساب الرسم";
            lblEffectiveFrom.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDisplayOrder
            // 
            lblDisplayOrder.Dock = DockStyle.Fill;
            lblDisplayOrder.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDisplayOrder.Location = new Point(741, 130);
            lblDisplayOrder.ForeColor = Color.FromArgb(16, 24, 40);
            lblDisplayOrder.BackColor = Color.Transparent;
            lblDisplayOrder.Name = "lblDisplayOrder";
            lblDisplayOrder.Margin = new Padding(3);
            lblDisplayOrder.AutoEllipsis = true;
            lblDisplayOrder.Size = new Size(139, 40);
            lblDisplayOrder.TabIndex = 81;
            lblDisplayOrder.Text = "أولوية";
            lblDisplayOrder.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbCurrency
            // 
            cmbCurrency.Dock = DockStyle.Fill;
            cmbCurrency.FormattingEnabled = true;
            cmbCurrency.Items.AddRange(new object[] { "وزن", "حجم", "قطعة", "قيمة", "سعر ثابت" });
            cmbCurrency.Location = new Point(451, 93);
            cmbCurrency.BackColor = Color.White;
            cmbCurrency.ForeColor = Color.FromArgb(16, 24, 40);
            cmbCurrency.Name = "cmbCurrency";
            cmbCurrency.Font = new Font("Segoe UI", 11F);
            cmbCurrency.Margin = new Padding(3);
            cmbCurrency.Size = new Size(284, 28);
            cmbCurrency.TabIndex = 80;
            // 
            // cmbCalculationMethod
            // 
            cmbCalculationMethod.Dock = DockStyle.Fill;
            cmbCalculationMethod.FormattingEnabled = true;
            cmbCalculationMethod.Location = new Point(451, 53);
            cmbCalculationMethod.BackColor = Color.White;
            cmbCalculationMethod.ForeColor = Color.FromArgb(16, 24, 40);
            cmbCalculationMethod.Name = "cmbCalculationMethod";
            cmbCalculationMethod.Font = new Font("Segoe UI", 11F);
            cmbCalculationMethod.Margin = new Padding(3);
            cmbCalculationMethod.Size = new Size(284, 28);
            cmbCalculationMethod.TabIndex = 76;
            // 
            // txtServiceFeeNameAr
            // 
            txtServiceFeeNameAr.Dock = DockStyle.Fill;
            txtServiceFeeNameAr.Location = new Point(453, 15);
            txtServiceFeeNameAr.Margin = new Padding(3);
            txtServiceFeeNameAr.BackColor = Color.White;
            txtServiceFeeNameAr.ForeColor = Color.FromArgb(16, 24, 40);
            txtServiceFeeNameAr.Name = "txtServiceFeeNameAr";
            txtServiceFeeNameAr.Font = new Font("Segoe UI", 11F);
            txtServiceFeeNameAr.Size = new Size(280, 27);
            txtServiceFeeNameAr.TabIndex = 75;
            // 
            // lblCurrency
            // 
            lblCurrency.Dock = DockStyle.Fill;
            lblCurrency.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCurrency.Location = new Point(1176, 130);
            lblCurrency.ForeColor = Color.FromArgb(16, 24, 40);
            lblCurrency.BackColor = Color.Transparent;
            lblCurrency.Name = "lblCurrency";
            lblCurrency.Margin = new Padding(3);
            lblCurrency.AutoEllipsis = true;
            lblCurrency.Size = new Size(139, 40);
            lblCurrency.TabIndex = 69;
            lblCurrency.Text = "فئة / صنف الشحن";
            lblCurrency.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPricingBasis
            // 
            lblPricingBasis.Dock = DockStyle.Fill;
            lblPricingBasis.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPricingBasis.Location = new Point(741, 90);
            lblPricingBasis.ForeColor = Color.FromArgb(16, 24, 40);
            lblPricingBasis.BackColor = Color.Transparent;
            lblPricingBasis.Name = "lblPricingBasis";
            lblPricingBasis.Margin = new Padding(3);
            lblPricingBasis.AutoEllipsis = true;
            lblPricingBasis.Size = new Size(139, 40);
            lblPricingBasis.TabIndex = 67;
            lblPricingBasis.Text = "العملة";
            lblPricingBasis.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbFeeType
            // 
            cmbFeeType.Dock = DockStyle.Fill;
            cmbFeeType.FormattingEnabled = true;
            cmbFeeType.Location = new Point(886, 53);
            cmbFeeType.BackColor = Color.White;
            cmbFeeType.ForeColor = Color.FromArgb(16, 24, 40);
            cmbFeeType.Name = "cmbFeeType";
            cmbFeeType.Font = new Font("Segoe UI", 11F);
            cmbFeeType.Margin = new Padding(3);
            cmbFeeType.Size = new Size(284, 28);
            cmbFeeType.TabIndex = 62;
            // 
            // lblShipmentType
            // 
            lblShipmentType.Dock = DockStyle.Fill;
            lblShipmentType.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblShipmentType.Location = new Point(1176, 50);
            lblShipmentType.ForeColor = Color.FromArgb(16, 24, 40);
            lblShipmentType.BackColor = Color.Transparent;
            lblShipmentType.Name = "lblShipmentType";
            lblShipmentType.Margin = new Padding(3);
            lblShipmentType.AutoEllipsis = true;
            lblShipmentType.Size = new Size(139, 40);
            lblShipmentType.TabIndex = 59;
            lblShipmentType.Text = "نوع البند";
            lblShipmentType.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTransportMethod
            // 
            lblTransportMethod.Dock = DockStyle.Fill;
            lblTransportMethod.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTransportMethod.Location = new Point(1176, 90);
            lblTransportMethod.ForeColor = Color.FromArgb(16, 24, 40);
            lblTransportMethod.BackColor = Color.Transparent;
            lblTransportMethod.Name = "lblTransportMethod";
            lblTransportMethod.Margin = new Padding(3);
            lblTransportMethod.AutoEllipsis = true;
            lblTransportMethod.Size = new Size(139, 40);
            lblTransportMethod.TabIndex = 53;
            lblTransportMethod.Text = "النسبة";
            lblTransportMethod.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPriority
            // 
            lblPriority.Dock = DockStyle.Fill;
            lblPriority.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPriority.Location = new Point(741, 50);
            lblPriority.ForeColor = Color.FromArgb(16, 24, 40);
            lblPriority.BackColor = Color.Transparent;
            lblPriority.Name = "lblPriority";
            lblPriority.Margin = new Padding(3);
            lblPriority.AutoEllipsis = true;
            lblPriority.Size = new Size(139, 40);
            lblPriority.TabIndex = 6;
            lblPriority.Text = "طريقة الاحتساب";
            lblPriority.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPricingRuleCode
            // 
            lblPricingRuleCode.Dock = DockStyle.Fill;
            lblPricingRuleCode.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPricingRuleCode.Location = new Point(1176, 10);
            lblPricingRuleCode.ForeColor = Color.FromArgb(16, 24, 40);
            lblPricingRuleCode.BackColor = Color.Transparent;
            lblPricingRuleCode.Name = "lblPricingRuleCode";
            lblPricingRuleCode.Margin = new Padding(3);
            lblPricingRuleCode.AutoEllipsis = true;
            lblPricingRuleCode.Size = new Size(139, 40);
            lblPricingRuleCode.TabIndex = 0;
            lblPricingRuleCode.Text = "كود الخدمه";
            lblPricingRuleCode.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtServiceFeeCode
            // 
            txtServiceFeeCode.Dock = DockStyle.Fill;
            txtServiceFeeCode.Location = new Point(888, 15);
            txtServiceFeeCode.Margin = new Padding(3);
            txtServiceFeeCode.BackColor = Color.White;
            txtServiceFeeCode.ForeColor = Color.FromArgb(16, 24, 40);
            txtServiceFeeCode.Name = "txtServiceFeeCode";
            txtServiceFeeCode.Font = new Font("Segoe UI", 11F);
            txtServiceFeeCode.Size = new Size(280, 27);
            txtServiceFeeCode.TabIndex = 1;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Font = new Font("Segoe UI", 10F);
            chkIsActive.Location = new Point(665, 293);
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
            // chkIsMandatory
            // 
            chkIsMandatory.AutoSize = true;
            chkIsMandatory.Font = new Font("Segoe UI", 10F);
            chkIsMandatory.Location = new Point(218, 213);
            chkIsMandatory.BackColor = Color.Transparent;
            chkIsMandatory.ForeColor = Color.FromArgb(16, 24, 40);
            chkIsMandatory.Dock = DockStyle.Fill;
            chkIsMandatory.Name = "chkIsMandatory";
            chkIsMandatory.Margin = new Padding(3);
            chkIsMandatory.Size = new Size(82, 29);
            chkIsMandatory.TabIndex = 145;
            chkIsMandatory.Text = "الزامي";
            chkIsMandatory.UseVisualStyleBackColor = false;
            // 
            // chkIncludeInShipmentTotal
            // 
            chkIncludeInShipmentTotal.AutoSize = true;
            chkIncludeInShipmentTotal.Font = new Font("Segoe UI", 10F);
            chkIncludeInShipmentTotal.Location = new Point(489, 253);
            chkIncludeInShipmentTotal.BackColor = Color.Transparent;
            chkIncludeInShipmentTotal.ForeColor = Color.FromArgb(16, 24, 40);
            chkIncludeInShipmentTotal.Dock = DockStyle.Fill;
            chkIncludeInShipmentTotal.Name = "chkIncludeInShipmentTotal";
            chkIncludeInShipmentTotal.Margin = new Padding(3);
            chkIncludeInShipmentTotal.Size = new Size(246, 29);
            chkIncludeInShipmentTotal.TabIndex = 149;
            chkIncludeInShipmentTotal.Text = "يدخل ضمن اجمالي البوليصه";
            chkIncludeInShipmentTotal.UseVisualStyleBackColor = false;
            // 
            // chkShowInPrint
            // 
            chkShowInPrint.AutoSize = true;
            chkShowInPrint.Font = new Font("Segoe UI", 10F);
            chkShowInPrint.Location = new Point(134, 253);
            chkShowInPrint.BackColor = Color.Transparent;
            chkShowInPrint.ForeColor = Color.FromArgb(16, 24, 40);
            chkShowInPrint.Dock = DockStyle.Fill;
            chkShowInPrint.Name = "chkShowInPrint";
            chkShowInPrint.Margin = new Padding(3);
            chkShowInPrint.Size = new Size(166, 29);
            chkShowInPrint.TabIndex = 150;
            chkShowInPrint.Text = "يضهر في الطباعه";
            chkShowInPrint.UseVisualStyleBackColor = false;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 29);
            tabPage1.BackColor = Color.LightCyan;
            tabPage1.Margin = new Padding(0);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1328, 475);
            tabPage1.TabIndex = 3;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = false;
            // 
            // dgvShipmentServiceFees
            // 
            AccessibleName = "خدمات ورسوم الشحن";
            AutoScaleDimensions = new SizeF(120F, 120F);
            BackColor = Color.FromArgb(248, 250, 252);
            ForeColor = Color.FromArgb(16, 24, 40);
            Margin = new Padding(0);
            Padding = new Padding(0);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);
            Controls.Add(tlpAuditInfo);
            Name = "dgvShipmentServiceFees";
            Font = new Font("Segoe UI", 10F);
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1336, 789);
            tlpAuditInfo.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            pnlContent.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            tabPricingRules.ResumeLayout(false);
            tlpPricingRuleFields.ResumeLayout(false);
            tlpPricingRuleFields.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudMaximumFee).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMinimumFee).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDisplayOrder).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPercentage).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAmount).EndInit();
            ResumeLayout(false);
        
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(273, 0);
        designerCommandBar.Size = new Size(1063, 53);
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
        private Panel pnlContent;
        private TabControl tabMain;
        private TabPage tabPricingRules;
        private TableLayoutPanel tlpPricingRuleFields;
        private Label label3;
        private Label label1;
        private ComboBox cmbShipmentCategory;
        private Label lblNotes;
        private Label lblMaximumValue;
        private Label lblMinimumValue;
        private Label lblEffectiveFrom;
        private Label lblDisplayOrder;
        private ComboBox cmbCurrency;
        private ComboBox cmbCalculationMethod;
        private TextBox txtServiceFeeNameAr;
        private Label lblCurrency;
        private Label lblPricingBasis;
        private ComboBox cmbFeeType;
        private Label lblShipmentType;
        private Label lblTransportMethod;
        private Label lblPriority;
        private Label lblPricingRuleCode;
        private TextBox txtServiceFeeCode;
        private CheckBox chkIsActive;
        private TabPage tabPage1;
        private Label label11;
        private Label label10;
        private ComboBox cmbTransportMethod;
        private Label label9;
        private ComboBox cmbPriority;
        private ComboBox cmbShipmentType;
        private Label label8;
        private NumericUpDown nudPercentage;
        private NumericUpDown nudAmount;
        private TextBox txtServiceFeeNameEn;
        private Label label7;
        private Label label2;
        private TextBox txtNotes;
        private TextBox txtDescription;
        private NumericUpDown nudDisplayOrder;
        private ComboBox cmbFeeBasis;
        private Label label14;
        private NumericUpDown nudMaximumFee;
        private NumericUpDown nudMinimumFee;
        private DateTimePicker dtpEffectiveTo;
        private DateTimePicker dtpEffectiveFrom;
        private CheckBox chkAllowManualEdit;
        private CheckBox chkIsMandatory;
        private CheckBox chkIncludeInShipmentTotal;
        private CheckBox chkShowInPrint;
    }
}
