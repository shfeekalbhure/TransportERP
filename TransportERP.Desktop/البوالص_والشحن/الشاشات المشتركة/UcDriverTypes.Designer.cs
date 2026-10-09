namespace TransportERP.Desktop.البوالص_والشحن.الشاشات_المشتركة
{
    partial class UcDriverTypes
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
            txtNotes = new TextBox();
            label6 = new Label();
            chkRequiresSpecialPackaging = new CheckBox();
            nudDefaultPackageWeight = new NumericUpDown();
            label14 = new Label();
            txtDescription = new TextBox();
            nudDisplayOrder = new NumericUpDown();
            nudDefaultPackagingCost = new NumericUpDown();
            txtPackagingTypeNameEn = new TextBox();
            label7 = new Label();
            label2 = new Label();
            label1 = new Label();
            txtPackagingTypeNameAr = new TextBox();
            cmbPackagingCategory = new ComboBox();
            lblShipmentType = new Label();
            lblTransportMethod = new Label();
            lblPriority = new Label();
            lblPricingRuleCode = new Label();
            txtPackagingTypeCode = new TextBox();
            chkIsReusable = new CheckBox();
            chkSuitableForFragile = new CheckBox();
            chkSuitableForLiquids = new CheckBox();
            chkIsActive = new CheckBox();
            tabPage1 = new TabPage();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            tlpAuditInfo.SuspendLayout();
            pnlContent.SuspendLayout();
            tabMain.SuspendLayout();
            tabPricingRules.SuspendLayout();
            tlpPricingRuleFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudDefaultPackageWeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDisplayOrder).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDefaultPackagingCost).BeginInit();
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
            pnlHeader.Size = new Size(1451, 53);
            pnlHeader.TabIndex = 10;
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
            pnlToolbar.Location = new Point(186, 0);
            pnlToolbar.Margin = new Padding(5);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.MinimumSize = new Size(0, 48);
            pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlToolbar.AutoSize = true;
            pnlToolbar.Padding = new Padding(5);
            pnlToolbar.Size = new Size(1265, 53);
            pnlToolbar.TabIndex = 2;
            pnlToolbar.WrapContents = true;
            // 
            // btnNew
            // 
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Microsoft Sans Serif", 10F);
            btnNew.Location = new Point(1161, 9);
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
            btnSave.Location = new Point(1063, 9);
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
            btnEdit.Location = new Point(965, 9);
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
            btnDelete.Location = new Point(867, 9);
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
            btnRefresh.Location = new Point(769, 9);
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
            btnClose.Location = new Point(671, 9);
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
            btnFirst.Location = new Point(573, 9);
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
            btnPrevious.Location = new Point(475, 9);
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
            txtCurrentRecordNo.Location = new Point(404, 10);
            txtCurrentRecordNo.Margin = new Padding(4);
            txtCurrentRecordNo.Multiline = true;
            txtCurrentRecordNo.BackColor = Color.White;
            txtCurrentRecordNo.ForeColor = Color.FromArgb(16, 24, 40);
            txtCurrentRecordNo.Dock = DockStyle.None;
            txtCurrentRecordNo.Name = "txtCurrentRecordNo";
            txtCurrentRecordNo.Font = new Font("Segoe UI", 11F);
            txtCurrentRecordNo.ReadOnly = true;
            txtCurrentRecordNo.Size = new Size(62, 30);
            txtCurrentRecordNo.TabIndex = 8;
            txtCurrentRecordNo.TextAlign = HorizontalAlignment.Center;
            // 
            // btnNext
            // 
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("Microsoft Sans Serif", 10F);
            btnNext.Location = new Point(305, 9);
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
            btnLast.Location = new Point(207, 9);
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
            btnUndo.Location = new Point(109, 9);
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
            lblTitle.Size = new Size(186, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "انواع السائقين";
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
            tlpAuditInfo.Location = new Point(0, 837);
            tlpAuditInfo.Margin = new Padding(0);
            tlpAuditInfo.BackColor = Color.FromArgb(248, 250, 252);
            tlpAuditInfo.Name = "tlpAuditInfo";
            tlpAuditInfo.Padding = new Padding(8);
            tlpAuditInfo.RowCount = 1;
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpAuditInfo.Size = new Size(1451, 48);
            tlpAuditInfo.TabIndex = 11;
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
            lblPrintCount.AutoSize = false;
            lblPrintCount.RightToLeft = RightToLeft.No;
            lblPrintCount.Size = new Size(169, 33);
            lblPrintCount.TabIndex = 8;
            lblPrintCount.Text = "عدد مرات الطباعة :- 00";
            lblPrintCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblLastPrintedAt
            // 
            lblLastPrintedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblLastPrintedAt.Dock = DockStyle.Fill;
            lblLastPrintedAt.Font = new Font("Segoe UI", 10F);
            lblLastPrintedAt.Location = new Point(178, 0);
            lblLastPrintedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblLastPrintedAt.Name = "lblLastPrintedAt";
            lblLastPrintedAt.Margin = new Padding(3);
            lblLastPrintedAt.AutoEllipsis = true;
            lblLastPrintedAt.AutoSize = false;
            lblLastPrintedAt.RightToLeft = RightToLeft.No;
            lblLastPrintedAt.Size = new Size(226, 33);
            lblLastPrintedAt.TabIndex = 5;
            lblLastPrintedAt.Text = "تاريخ اخر طباعة:  27/09/2026";
            lblLastPrintedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblEditCount
            // 
            lblEditCount.BackColor = Color.FromArgb(192, 255, 255);
            lblEditCount.Dock = DockStyle.Fill;
            lblEditCount.Font = new Font("Segoe UI", 10F);
            lblEditCount.Location = new Point(410, 0);
            lblEditCount.ForeColor = Color.FromArgb(16, 24, 40);
            lblEditCount.Name = "lblEditCount";
            lblEditCount.Margin = new Padding(3);
            lblEditCount.AutoEllipsis = true;
            lblEditCount.AutoSize = false;
            lblEditCount.RightToLeft = RightToLeft.No;
            lblEditCount.Size = new Size(168, 33);
            lblEditCount.TabIndex = 4;
            lblEditCount.Text = "عدد التعديلات :- 00";
            lblEditCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedAt
            // 
            lblModifiedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedAt.Dock = DockStyle.Fill;
            lblModifiedAt.Font = new Font("Segoe UI", 10F);
            lblModifiedAt.Location = new Point(584, 0);
            lblModifiedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblModifiedAt.Name = "lblModifiedAt";
            lblModifiedAt.Margin = new Padding(3);
            lblModifiedAt.AutoEllipsis = true;
            lblModifiedAt.AutoSize = false;
            lblModifiedAt.RightToLeft = RightToLeft.No;
            lblModifiedAt.Size = new Size(226, 33);
            lblModifiedAt.TabIndex = 3;
            lblModifiedAt.Text = " تاريخ التعديل:  27/09/2026";
            lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedBy
            // 
            lblModifiedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedBy.Dock = DockStyle.Fill;
            lblModifiedBy.Font = new Font("Segoe UI", 10F);
            lblModifiedBy.Location = new Point(816, 0);
            lblModifiedBy.ForeColor = Color.FromArgb(16, 24, 40);
            lblModifiedBy.Name = "lblModifiedBy";
            lblModifiedBy.Margin = new Padding(3);
            lblModifiedBy.AutoEllipsis = true;
            lblModifiedBy.AutoSize = false;
            lblModifiedBy.RightToLeft = RightToLeft.No;
            lblModifiedBy.Size = new Size(197, 33);
            lblModifiedBy.TabIndex = 2;
            lblModifiedBy.Text = "عدل بواسطة: - مدير النظام";
            lblModifiedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedAt
            // 
            lblCreatedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedAt.Dock = DockStyle.Fill;
            lblCreatedAt.Font = new Font("Segoe UI", 10F);
            lblCreatedAt.Location = new Point(1019, 0);
            lblCreatedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblCreatedAt.Name = "lblCreatedAt";
            lblCreatedAt.Margin = new Padding(3);
            lblCreatedAt.AutoEllipsis = true;
            lblCreatedAt.AutoSize = false;
            lblCreatedAt.RightToLeft = RightToLeft.No;
            lblCreatedAt.Size = new Size(226, 33);
            lblCreatedAt.TabIndex = 1;
            lblCreatedAt.Text = " تاريخ الانشاء:  27/09/2026";
            lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedBy
            // 
            lblCreatedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedBy.Dock = DockStyle.Fill;
            lblCreatedBy.Font = new Font("Segoe UI", 10F);
            lblCreatedBy.Location = new Point(1251, 0);
            lblCreatedBy.ForeColor = Color.FromArgb(16, 24, 40);
            lblCreatedBy.Name = "lblCreatedBy";
            lblCreatedBy.Margin = new Padding(3);
            lblCreatedBy.AutoEllipsis = true;
            lblCreatedBy.AutoSize = false;
            lblCreatedBy.RightToLeft = RightToLeft.No;
            lblCreatedBy.Size = new Size(197, 33);
            lblCreatedBy.TabIndex = 0;
            lblCreatedBy.Text = " أنشأ بواسطة: - مدير النظام";
            lblCreatedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.LightCyan;
            pnlContent.Controls.Add(tabMain);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 53);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1451, 784);
            pnlContent.TabIndex = 12;
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tabPricingRules);
            tabMain.Controls.Add(tabPage1);
            tabMain.Dock = DockStyle.Fill;
            tabMain.Location = new Point(0, 0);
            tabMain.Margin = new Padding(0);
            tabMain.Name = "tabMain";
            tabMain.Font = new Font("Segoe UI", 9F);
            tabMain.RightToLeftLayout = true;
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1451, 450);
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
            tabPricingRules.Size = new Size(1443, 417);
            tabPricingRules.TabIndex = 2;
            tabPricingRules.Text = "انواع السائقين";
            tabPricingRules.UseVisualStyleBackColor = false;
            // 
            // tlpPricingRuleFields
            // 
            tlpPricingRuleFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpPricingRuleFields.BackColor = Color.LightCyan;
            tlpPricingRuleFields.ColumnCount = 6;
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.1111107F));
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22.2222214F));
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.1111107F));
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22.2222214F));
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.1111107F));
            tlpPricingRuleFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22.2222214F));
            tlpPricingRuleFields.Controls.Add(txtNotes, 1, 4);
            tlpPricingRuleFields.Controls.Add(label6, 0, 4);
            tlpPricingRuleFields.Controls.Add(chkRequiresSpecialPackaging, 3, 2);
            tlpPricingRuleFields.Controls.Add(nudDefaultPackageWeight, 3, 1);
            tlpPricingRuleFields.Controls.Add(label14, 4, 4);
            tlpPricingRuleFields.Controls.Add(txtDescription, 5, 4);
            tlpPricingRuleFields.Controls.Add(nudDisplayOrder, 1, 2);
            tlpPricingRuleFields.Controls.Add(nudDefaultPackagingCost, 5, 1);
            tlpPricingRuleFields.Controls.Add(txtPackagingTypeNameEn, 5, 0);
            tlpPricingRuleFields.Controls.Add(label7, 4, 1);
            tlpPricingRuleFields.Controls.Add(label2, 4, 0);
            tlpPricingRuleFields.Controls.Add(label1, 2, 0);
            tlpPricingRuleFields.Controls.Add(txtPackagingTypeNameAr, 3, 0);
            tlpPricingRuleFields.Controls.Add(cmbPackagingCategory, 1, 1);
            tlpPricingRuleFields.Controls.Add(lblShipmentType, 0, 1);
            tlpPricingRuleFields.Controls.Add(lblTransportMethod, 0, 2);
            tlpPricingRuleFields.Controls.Add(lblPriority, 2, 1);
            tlpPricingRuleFields.Controls.Add(lblPricingRuleCode, 0, 0);
            tlpPricingRuleFields.Controls.Add(txtPackagingTypeCode, 1, 0);
            tlpPricingRuleFields.Controls.Add(chkIsReusable, 5, 2);
            tlpPricingRuleFields.Controls.Add(chkSuitableForFragile, 1, 3);
            tlpPricingRuleFields.Controls.Add(chkSuitableForLiquids, 3, 3);
            tlpPricingRuleFields.Controls.Add(chkIsActive, 5, 3);
            tlpPricingRuleFields.Dock = DockStyle.Top;
            tlpPricingRuleFields.Location = new Point(0, 0);
            tlpPricingRuleFields.Margin = new Padding(0);
            tlpPricingRuleFields.Name = "tlpPricingRuleFields";
            tlpPricingRuleFields.Padding = new Padding(0);
            tlpPricingRuleFields.RowCount = 5;
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            tlpPricingRuleFields.Size = new Size(1443, 232);
            tlpPricingRuleFields.TabIndex = 11;
            // 
            // txtNotes
            // 
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Location = new Point(988, 175);
            txtNotes.Margin = new Padding(3);
            txtNotes.Multiline = true;
            txtNotes.BackColor = Color.White;
            txtNotes.ForeColor = Color.FromArgb(16, 24, 40);
            txtNotes.Name = "txtNotes";
            txtNotes.Font = new Font("Segoe UI", 11F);
            txtNotes.Size = new Size(282, 51);
            txtNotes.TabIndex = 156;
            // 
            // label6
            // 
            label6.Dock = DockStyle.Fill;
            label6.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label6.Location = new Point(1278, 170);
            label6.ForeColor = Color.FromArgb(16, 24, 40);
            label6.BackColor = Color.Transparent;
            label6.Name = "label6";
            label6.Margin = new Padding(3);
            label6.AutoEllipsis = true;
            label6.AutoSize = false;
            label6.Size = new Size(152, 61);
            label6.TabIndex = 155;
            label6.Text = "ملاحظات";
            label6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // chkRequiresSpecialPackaging
            // 
            chkRequiresSpecialPackaging.AutoSize = true;
            chkRequiresSpecialPackaging.Font = new Font("Segoe UI", 10F);
            chkRequiresSpecialPackaging.Location = new Point(617, 93);
            chkRequiresSpecialPackaging.BackColor = Color.Transparent;
            chkRequiresSpecialPackaging.ForeColor = Color.FromArgb(16, 24, 40);
            chkRequiresSpecialPackaging.Dock = DockStyle.Fill;
            chkRequiresSpecialPackaging.Name = "chkRequiresSpecialPackaging";
            chkRequiresSpecialPackaging.Margin = new Padding(3);
            chkRequiresSpecialPackaging.Size = new Size(181, 29);
            chkRequiresSpecialPackaging.TabIndex = 152;
            chkRequiresSpecialPackaging.Text = "يتطلب تغليف خاص";
            chkRequiresSpecialPackaging.UseVisualStyleBackColor = false;
            // 
            // nudDefaultPackageWeight
            // 
            nudDefaultPackageWeight.Dock = DockStyle.Fill;
            nudDefaultPackageWeight.Location = new Point(488, 53);
            nudDefaultPackageWeight.BackColor = Color.White;
            nudDefaultPackageWeight.ForeColor = Color.FromArgb(16, 24, 40);
            nudDefaultPackageWeight.Name = "nudDefaultPackageWeight";
            nudDefaultPackageWeight.Font = new Font("Segoe UI", 11F);
            nudDefaultPackageWeight.Margin = new Padding(3);
            nudDefaultPackageWeight.Size = new Size(310, 27);
            nudDefaultPackageWeight.TabIndex = 151;
            // 
            // label14
            // 
            label14.Dock = DockStyle.Fill;
            label14.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label14.Location = new Point(330, 170);
            label14.ForeColor = Color.FromArgb(16, 24, 40);
            label14.BackColor = Color.Transparent;
            label14.Name = "label14";
            label14.Margin = new Padding(3);
            label14.AutoEllipsis = true;
            label14.AutoSize = false;
            label14.Size = new Size(152, 61);
            label14.TabIndex = 140;
            label14.Text = "الوصف";
            label14.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDescription
            // 
            txtDescription.Dock = DockStyle.Fill;
            txtDescription.Location = new Point(42, 175);
            txtDescription.Margin = new Padding(3);
            txtDescription.Multiline = true;
            txtDescription.BackColor = Color.White;
            txtDescription.ForeColor = Color.FromArgb(16, 24, 40);
            txtDescription.Name = "txtDescription";
            txtDescription.Font = new Font("Segoe UI", 11F);
            txtDescription.Size = new Size(280, 51);
            txtDescription.TabIndex = 134;
            // 
            // nudDisplayOrder
            // 
            nudDisplayOrder.Dock = DockStyle.Fill;
            nudDisplayOrder.Location = new Point(962, 93);
            nudDisplayOrder.BackColor = Color.White;
            nudDisplayOrder.ForeColor = Color.FromArgb(16, 24, 40);
            nudDisplayOrder.Name = "nudDisplayOrder";
            nudDisplayOrder.Font = new Font("Segoe UI", 11F);
            nudDisplayOrder.Margin = new Padding(3);
            nudDisplayOrder.Size = new Size(310, 27);
            nudDisplayOrder.TabIndex = 113;
            // 
            // nudDefaultPackagingCost
            // 
            nudDefaultPackagingCost.Dock = DockStyle.Fill;
            nudDefaultPackagingCost.Location = new Point(13, 53);
            nudDefaultPackagingCost.BackColor = Color.White;
            nudDefaultPackagingCost.ForeColor = Color.FromArgb(16, 24, 40);
            nudDefaultPackagingCost.Name = "nudDefaultPackagingCost";
            nudDefaultPackagingCost.Font = new Font("Segoe UI", 11F);
            nudDefaultPackagingCost.Margin = new Padding(3);
            nudDefaultPackagingCost.Size = new Size(311, 27);
            nudDefaultPackagingCost.TabIndex = 112;
            // 
            // txtPackagingTypeNameEn
            // 
            txtPackagingTypeNameEn.Dock = DockStyle.Fill;
            txtPackagingTypeNameEn.Location = new Point(15, 15);
            txtPackagingTypeNameEn.Margin = new Padding(3);
            txtPackagingTypeNameEn.BackColor = Color.White;
            txtPackagingTypeNameEn.ForeColor = Color.FromArgb(16, 24, 40);
            txtPackagingTypeNameEn.Name = "txtPackagingTypeNameEn";
            txtPackagingTypeNameEn.Font = new Font("Segoe UI", 11F);
            txtPackagingTypeNameEn.Size = new Size(307, 27);
            txtPackagingTypeNameEn.TabIndex = 111;
            // 
            // label7
            // 
            label7.Dock = DockStyle.Fill;
            label7.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label7.Location = new Point(330, 50);
            label7.ForeColor = Color.FromArgb(16, 24, 40);
            label7.BackColor = Color.Transparent;
            label7.Name = "label7";
            label7.Margin = new Padding(3);
            label7.AutoEllipsis = true;
            label7.AutoSize = false;
            label7.Size = new Size(152, 40);
            label7.TabIndex = 110;
            label7.Text = "يسمح باستقبال الطلبات الخارجية";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label2.Location = new Point(330, 10);
            label2.ForeColor = Color.FromArgb(16, 24, 40);
            label2.BackColor = Color.Transparent;
            label2.Name = "label2";
            label2.Margin = new Padding(3);
            label2.AutoEllipsis = true;
            label2.AutoSize = false;
            label2.Size = new Size(152, 40);
            label2.TabIndex = 106;
            label2.Text = "اسم نوع السائق انجليزي";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.Location = new Point(804, 10);
            label1.ForeColor = Color.FromArgb(16, 24, 40);
            label1.BackColor = Color.Transparent;
            label1.Name = "label1";
            label1.Margin = new Padding(3);
            label1.AutoEllipsis = true;
            label1.AutoSize = false;
            label1.Size = new Size(152, 40);
            label1.TabIndex = 99;
            label1.Text = "اسم نوع السائق عربي";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPackagingTypeNameAr
            // 
            txtPackagingTypeNameAr.Dock = DockStyle.Fill;
            txtPackagingTypeNameAr.Location = new Point(490, 15);
            txtPackagingTypeNameAr.Margin = new Padding(3);
            txtPackagingTypeNameAr.BackColor = Color.White;
            txtPackagingTypeNameAr.ForeColor = Color.FromArgb(16, 24, 40);
            txtPackagingTypeNameAr.Name = "txtPackagingTypeNameAr";
            txtPackagingTypeNameAr.Font = new Font("Segoe UI", 11F);
            txtPackagingTypeNameAr.Size = new Size(306, 27);
            txtPackagingTypeNameAr.TabIndex = 75;
            // 
            // cmbPackagingCategory
            // 
            cmbPackagingCategory.Dock = DockStyle.Fill;
            cmbPackagingCategory.FormattingEnabled = true;
            cmbPackagingCategory.Location = new Point(962, 53);
            cmbPackagingCategory.BackColor = Color.White;
            cmbPackagingCategory.ForeColor = Color.FromArgb(16, 24, 40);
            cmbPackagingCategory.Name = "cmbPackagingCategory";
            cmbPackagingCategory.Font = new Font("Segoe UI", 11F);
            cmbPackagingCategory.Margin = new Padding(3);
            cmbPackagingCategory.Size = new Size(310, 28);
            cmbPackagingCategory.TabIndex = 62;
            // 
            // lblShipmentType
            // 
            lblShipmentType.Dock = DockStyle.Fill;
            lblShipmentType.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblShipmentType.Location = new Point(1278, 50);
            lblShipmentType.ForeColor = Color.FromArgb(16, 24, 40);
            lblShipmentType.BackColor = Color.Transparent;
            lblShipmentType.Name = "lblShipmentType";
            lblShipmentType.Margin = new Padding(3);
            lblShipmentType.AutoEllipsis = true;
            lblShipmentType.AutoSize = false;
            lblShipmentType.Size = new Size(152, 40);
            lblShipmentType.TabIndex = 59;
            lblShipmentType.Text = "يتطلب التسجيل عبر التطبيق";
            lblShipmentType.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTransportMethod
            // 
            lblTransportMethod.Dock = DockStyle.Fill;
            lblTransportMethod.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTransportMethod.Location = new Point(1278, 90);
            lblTransportMethod.ForeColor = Color.FromArgb(16, 24, 40);
            lblTransportMethod.BackColor = Color.Transparent;
            lblTransportMethod.Name = "lblTransportMethod";
            lblTransportMethod.Margin = new Padding(3);
            lblTransportMethod.AutoEllipsis = true;
            lblTransportMethod.AutoSize = false;
            lblTransportMethod.Size = new Size(152, 40);
            lblTransportMethod.TabIndex = 53;
            lblTransportMethod.Text = "ترتيب العرض";
            lblTransportMethod.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPriority
            // 
            lblPriority.Dock = DockStyle.Fill;
            lblPriority.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPriority.Location = new Point(804, 50);
            lblPriority.ForeColor = Color.FromArgb(16, 24, 40);
            lblPriority.BackColor = Color.Transparent;
            lblPriority.Name = "lblPriority";
            lblPriority.Margin = new Padding(3);
            lblPriority.AutoEllipsis = true;
            lblPriority.AutoSize = false;
            lblPriority.Size = new Size(152, 40);
            lblPriority.TabIndex = 6;
            lblPriority.Text = "يتطلب تفعيل GBS";
            lblPriority.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPricingRuleCode
            // 
            lblPricingRuleCode.Dock = DockStyle.Fill;
            lblPricingRuleCode.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPricingRuleCode.Location = new Point(1278, 10);
            lblPricingRuleCode.ForeColor = Color.FromArgb(16, 24, 40);
            lblPricingRuleCode.BackColor = Color.Transparent;
            lblPricingRuleCode.Name = "lblPricingRuleCode";
            lblPricingRuleCode.Margin = new Padding(3);
            lblPricingRuleCode.AutoEllipsis = true;
            lblPricingRuleCode.AutoSize = false;
            lblPricingRuleCode.Size = new Size(152, 40);
            lblPricingRuleCode.TabIndex = 0;
            lblPricingRuleCode.Text = "كود نوع السائق";
            lblPricingRuleCode.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPackagingTypeCode
            // 
            txtPackagingTypeCode.Dock = DockStyle.Fill;
            txtPackagingTypeCode.Location = new Point(990, 15);
            txtPackagingTypeCode.Margin = new Padding(3);
            txtPackagingTypeCode.BackColor = Color.White;
            txtPackagingTypeCode.ForeColor = Color.FromArgb(16, 24, 40);
            txtPackagingTypeCode.Name = "txtPackagingTypeCode";
            txtPackagingTypeCode.Font = new Font("Segoe UI", 11F);
            txtPackagingTypeCode.Size = new Size(280, 27);
            txtPackagingTypeCode.TabIndex = 1;
            // 
            // chkIsReusable
            // 
            chkIsReusable.AutoSize = true;
            chkIsReusable.Font = new Font("Segoe UI", 10F);
            chkIsReusable.Location = new Point(130, 93);
            chkIsReusable.BackColor = Color.Transparent;
            chkIsReusable.ForeColor = Color.FromArgb(16, 24, 40);
            chkIsReusable.Dock = DockStyle.Fill;
            chkIsReusable.Name = "chkIsReusable";
            chkIsReusable.Margin = new Padding(3);
            chkIsReusable.Size = new Size(194, 29);
            chkIsReusable.TabIndex = 145;
            chkIsReusable.Text = "قابل لأعادة الاستخدام";
            chkIsReusable.UseVisualStyleBackColor = false;
            // 
            // chkSuitableForFragile
            // 
            chkSuitableForFragile.AutoSize = true;
            chkSuitableForFragile.Font = new Font("Segoe UI", 10F);
            chkSuitableForFragile.Location = new Point(1034, 133);
            chkSuitableForFragile.BackColor = Color.Transparent;
            chkSuitableForFragile.ForeColor = Color.FromArgb(16, 24, 40);
            chkSuitableForFragile.Dock = DockStyle.Fill;
            chkSuitableForFragile.Name = "chkSuitableForFragile";
            chkSuitableForFragile.Margin = new Padding(3);
            chkSuitableForFragile.Size = new Size(238, 29);
            chkSuitableForFragile.TabIndex = 149;
            chkSuitableForFragile.Text = "مناسب للمواد القابله للكسر";
            chkSuitableForFragile.UseVisualStyleBackColor = false;
            // 
            // chkSuitableForLiquids
            // 
            chkSuitableForLiquids.AutoSize = true;
            chkSuitableForLiquids.Font = new Font("Segoe UI", 10F);
            chkSuitableForLiquids.Location = new Point(602, 133);
            chkSuitableForLiquids.BackColor = Color.Transparent;
            chkSuitableForLiquids.ForeColor = Color.FromArgb(16, 24, 40);
            chkSuitableForLiquids.Dock = DockStyle.Fill;
            chkSuitableForLiquids.Name = "chkSuitableForLiquids";
            chkSuitableForLiquids.Margin = new Padding(3);
            chkSuitableForLiquids.Size = new Size(196, 29);
            chkSuitableForLiquids.TabIndex = 150;
            chkSuitableForLiquids.Text = "مناسب للمواد السائلة";
            chkSuitableForLiquids.UseVisualStyleBackColor = false;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Font = new Font("Segoe UI", 10F);
            chkIsActive.Location = new Point(254, 133);
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
            tabPage1.Location = new Point(4, 29);
            tabPage1.BackColor = Color.LightCyan;
            tabPage1.Margin = new Padding(0);
            tabPage1.Name = "tabPage1";
            tabPage1.AutoScroll = true;
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1443, 417);
            tabPage1.TabIndex = 3;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = false;
            // 
            // UcDriverTypes
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            BackColor = Color.FromArgb(248, 250, 252);
            ForeColor = Color.FromArgb(16, 24, 40);
            Margin = new Padding(0);
            Padding = new Padding(0);
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 10F);
            Controls.Add(pnlContent);
            Controls.Add(tlpAuditInfo);
            Controls.Add(pnlHeader);
            Name = "UcDriverTypes";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1451, 870);
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
            ((System.ComponentModel.ISupportInitialize)nudDefaultPackageWeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDisplayOrder).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDefaultPackagingCost).EndInit();
            ResumeLayout(false);
        
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(186, 0);
        designerCommandBar.Size = new Size(1265, 53);
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
        private TextBox txtNotes;
        private Label label6;
        private CheckBox chkRequiresSpecialPackaging;
        private NumericUpDown nudDefaultPackageWeight;
        private Label label14;
        private TextBox txtDescription;
        private NumericUpDown nudDisplayOrder;
        private NumericUpDown nudDefaultPackagingCost;
        private TextBox txtPackagingTypeNameEn;
        private Label label7;
        private Label label2;
        private Label label1;
        private TextBox txtPackagingTypeNameAr;
        private ComboBox cmbPackagingCategory;
        private Label lblShipmentType;
        private Label lblTransportMethod;
        private Label lblPriority;
        private Label lblPricingRuleCode;
        private TextBox txtPackagingTypeCode;
        private CheckBox chkIsReusable;
        private CheckBox chkSuitableForFragile;
        private CheckBox chkSuitableForLiquids;
        private CheckBox chkIsActive;
        private TabPage tabPage1;
    }
}
