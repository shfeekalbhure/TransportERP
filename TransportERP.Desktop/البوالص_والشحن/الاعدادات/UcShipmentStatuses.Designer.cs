namespace TransportERP.Desktop.البوالص_والشحن.الاعدادات
{
    partial class UcShipmentStatuses
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
            tabMain = new TabControl();
            tabPricingRules = new TabPage();
            tlpPricingRuleFields = new TableLayoutPanel();
            label2 = new Label();
            chkIsActive = new CheckBox();
            chkRequiresApproval = new CheckBox();
            chkAllowTransfer = new CheckBox();
            txtDescription = new TextBox();
            label7 = new Label();
            chkAllowDelivery = new CheckBox();
            chkAllowCancel = new CheckBox();
            chkIsFinal = new CheckBox();
            chkIsInitial = new CheckBox();
            chkAllowEdit = new CheckBox();
            nudDisplayOrder = new NumericUpDown();
            label8 = new Label();
            cmbStatusColor = new ComboBox();
            label9 = new Label();
            txtStatusNameEn = new TextBox();
            label1 = new Label();
            txtStatusNameAr = new TextBox();
            lblPriority = new Label();
            lblPricingRuleCode = new Label();
            txtStatusCode = new TextBox();
            txtNotes = new TextBox();
            tabPage1 = new TabPage();
            tlpAuditInfo.SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            tabMain.SuspendLayout();
            tabPricingRules.SuspendLayout();
            tlpPricingRuleFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudDisplayOrder).BeginInit();
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
            tlpAuditInfo.Location = new Point(0, 967);
            tlpAuditInfo.Margin = new Padding(0);
            tlpAuditInfo.BackColor = Color.FromArgb(248, 250, 252);
            tlpAuditInfo.Name = "tlpAuditInfo";
            tlpAuditInfo.Padding = new Padding(8);
            tlpAuditInfo.RowCount = 1;
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAuditInfo.Size = new Size(1255, 48);
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
            lblPrintCount.Size = new Size(149, 33);
            lblPrintCount.TabIndex = 8;
            lblPrintCount.Text = "عدد مرات الطباعة :- 00";
            lblPrintCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblLastPrintedAt
            // 
            lblLastPrintedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblLastPrintedAt.Dock = DockStyle.Fill;
            lblLastPrintedAt.Font = new Font("Segoe UI", 10F);
            lblLastPrintedAt.Location = new Point(158, 0);
            lblLastPrintedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblLastPrintedAt.Name = "lblLastPrintedAt";
            lblLastPrintedAt.Margin = new Padding(3);
            lblLastPrintedAt.AutoEllipsis = true;
            lblLastPrintedAt.RightToLeft = RightToLeft.No;
            lblLastPrintedAt.Size = new Size(194, 33);
            lblLastPrintedAt.TabIndex = 5;
            lblLastPrintedAt.Text = "تاريخ اخر طباعة:  27/09/2026";
            lblLastPrintedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblEditCount
            // 
            lblEditCount.BackColor = Color.FromArgb(192, 255, 255);
            lblEditCount.Dock = DockStyle.Fill;
            lblEditCount.Font = new Font("Segoe UI", 10F);
            lblEditCount.Location = new Point(358, 0);
            lblEditCount.ForeColor = Color.FromArgb(16, 24, 40);
            lblEditCount.Name = "lblEditCount";
            lblEditCount.Margin = new Padding(3);
            lblEditCount.AutoEllipsis = true;
            lblEditCount.RightToLeft = RightToLeft.No;
            lblEditCount.Size = new Size(144, 33);
            lblEditCount.TabIndex = 4;
            lblEditCount.Text = "عدد التعديلات :- 00";
            lblEditCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedAt
            // 
            lblModifiedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedAt.Dock = DockStyle.Fill;
            lblModifiedAt.Font = new Font("Segoe UI", 10F);
            lblModifiedAt.Location = new Point(508, 0);
            lblModifiedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblModifiedAt.Name = "lblModifiedAt";
            lblModifiedAt.Margin = new Padding(3);
            lblModifiedAt.AutoEllipsis = true;
            lblModifiedAt.RightToLeft = RightToLeft.No;
            lblModifiedAt.Size = new Size(194, 33);
            lblModifiedAt.TabIndex = 3;
            lblModifiedAt.Text = " تاريخ التعديل:  27/09/2026";
            lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedBy
            // 
            lblModifiedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedBy.Dock = DockStyle.Fill;
            lblModifiedBy.Font = new Font("Segoe UI", 10F);
            lblModifiedBy.Location = new Point(708, 0);
            lblModifiedBy.ForeColor = Color.FromArgb(16, 24, 40);
            lblModifiedBy.Name = "lblModifiedBy";
            lblModifiedBy.Margin = new Padding(3);
            lblModifiedBy.AutoEllipsis = true;
            lblModifiedBy.RightToLeft = RightToLeft.No;
            lblModifiedBy.Size = new Size(169, 33);
            lblModifiedBy.TabIndex = 2;
            lblModifiedBy.Text = "عدل بواسطة: - مدير النظام";
            lblModifiedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedAt
            // 
            lblCreatedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedAt.Dock = DockStyle.Fill;
            lblCreatedAt.Font = new Font("Segoe UI", 10F);
            lblCreatedAt.Location = new Point(883, 0);
            lblCreatedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblCreatedAt.Name = "lblCreatedAt";
            lblCreatedAt.Margin = new Padding(3);
            lblCreatedAt.AutoEllipsis = true;
            lblCreatedAt.RightToLeft = RightToLeft.No;
            lblCreatedAt.Size = new Size(194, 33);
            lblCreatedAt.TabIndex = 1;
            lblCreatedAt.Text = " تاريخ الانشاء:  27/09/2026";
            lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedBy
            // 
            lblCreatedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedBy.Dock = DockStyle.Fill;
            lblCreatedBy.Font = new Font("Segoe UI", 10F);
            lblCreatedBy.Location = new Point(1083, 0);
            lblCreatedBy.ForeColor = Color.FromArgb(16, 24, 40);
            lblCreatedBy.Name = "lblCreatedBy";
            lblCreatedBy.Margin = new Padding(3);
            lblCreatedBy.AutoEllipsis = true;
            lblCreatedBy.RightToLeft = RightToLeft.No;
            lblCreatedBy.Size = new Size(169, 33);
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
            pnlHeader.Size = new Size(1255, 53);
            pnlHeader.TabIndex = 8;
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
            pnlToolbar.Location = new Point(194, 0);
            pnlToolbar.Margin = new Padding(5);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.MinimumSize = new Size(0, 48);
            pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlToolbar.AutoSize = true;
            pnlToolbar.Padding = new Padding(5);
            pnlToolbar.Size = new Size(1061, 53);
            pnlToolbar.TabIndex = 2;
            pnlToolbar.WrapContents = true;
            // 
            // btnNew
            // 
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Microsoft Sans Serif", 10F);
            btnNew.Location = new Point(957, 9);
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
            btnSave.Location = new Point(859, 9);
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
            btnEdit.Location = new Point(761, 9);
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
            btnDelete.Location = new Point(663, 9);
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
            btnRefresh.Location = new Point(565, 9);
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
            btnClose.Location = new Point(467, 9);
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
            btnFirst.Location = new Point(369, 9);
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
            btnPrevious.Location = new Point(271, 9);
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
            txtCurrentRecordNo.Location = new Point(200, 10);
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
            btnNext.Location = new Point(101, 9);
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
            btnLast.Location = new Point(3, 9);
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
            btnUndo.Location = new Point(-95, 9);
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
            lblTitle.Size = new Size(194, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "حالات البوالص";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tabPricingRules);
            tabMain.Controls.Add(tabPage1);
            tabMain.Dock = DockStyle.Top;
            tabMain.Location = new Point(0, 53);
            tabMain.Margin = new Padding(0);
            tabMain.Name = "tabMain";
            tabMain.Font = new Font("Segoe UI", 9F);
            tabMain.RightToLeftLayout = true;
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1255, 328);
            tabMain.TabIndex = 9;
            // 
            // tabPricingRules
            // 
            tabPricingRules.BackColor = Color.LightCyan;
            tabPricingRules.Controls.Add(tlpPricingRuleFields);
            tabPricingRules.Location = new Point(4, 29);
            tabPricingRules.Margin = new Padding(0);
            tabPricingRules.UseVisualStyleBackColor = false;
            tabPricingRules.Padding = new Padding(3);
            tabPricingRules.Name = "tabPricingRules";
            tabPricingRules.AutoScroll = true;
            tabPricingRules.Size = new Size(1247, 295);
            tabPricingRules.TabIndex = 2;
            tabPricingRules.Text = "حالات البوالص";
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
            tlpPricingRuleFields.Controls.Add(label2, 0, 5);
            tlpPricingRuleFields.Controls.Add(chkIsActive, 3, 4);
            tlpPricingRuleFields.Controls.Add(chkRequiresApproval, 2, 4);
            tlpPricingRuleFields.Controls.Add(chkAllowTransfer, 3, 3);
            tlpPricingRuleFields.Controls.Add(txtDescription, 3, 2);
            tlpPricingRuleFields.Controls.Add(label7, 2, 2);
            tlpPricingRuleFields.Controls.Add(chkAllowDelivery, 2, 3);
            tlpPricingRuleFields.Controls.Add(chkAllowCancel, 0, 4);
            tlpPricingRuleFields.Controls.Add(chkIsFinal, 1, 3);
            tlpPricingRuleFields.Controls.Add(chkIsInitial, 0, 3);
            tlpPricingRuleFields.Controls.Add(chkAllowEdit, 1, 4);
            tlpPricingRuleFields.Controls.Add(nudDisplayOrder, 1, 2);
            tlpPricingRuleFields.Controls.Add(label8, 0, 2);
            tlpPricingRuleFields.Controls.Add(cmbStatusColor, 1, 1);
            tlpPricingRuleFields.Controls.Add(label9, 0, 1);
            tlpPricingRuleFields.Controls.Add(txtStatusNameEn, 3, 1);
            tlpPricingRuleFields.Controls.Add(label1, 2, 0);
            tlpPricingRuleFields.Controls.Add(txtStatusNameAr, 3, 0);
            tlpPricingRuleFields.Controls.Add(lblPriority, 2, 1);
            tlpPricingRuleFields.Controls.Add(lblPricingRuleCode, 0, 0);
            tlpPricingRuleFields.Controls.Add(txtStatusCode, 1, 0);
            tlpPricingRuleFields.Controls.Add(txtNotes, 1, 5);
            tlpPricingRuleFields.Dock = DockStyle.Top;
            tlpPricingRuleFields.Location = new Point(0, 0);
            tlpPricingRuleFields.Margin = new Padding(0);
            tlpPricingRuleFields.Name = "tlpPricingRuleFields";
            tlpPricingRuleFields.MinimumSize = new Size(0, 272);
            tlpPricingRuleFields.Padding = new Padding(0);
            tlpPricingRuleFields.RowCount = 6;
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            tlpPricingRuleFields.Size = new Size(1247, 272);
            tlpPricingRuleFields.TabIndex = 11;
            tlpPricingRuleFields.Paint += tlpPricingRuleFields_Paint;
            // 
            // label2
            // 
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label2.Location = new Point(1056, 210);
            label2.ForeColor = Color.FromArgb(16, 24, 40);
            label2.BackColor = Color.Transparent;
            label2.Name = "label2";
            label2.Margin = new Padding(3);
            label2.AutoEllipsis = true;
            label2.Size = new Size(178, 40);
            label2.TabIndex = 184;
            label2.Text = "ملاحظات";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Font = new Font("Segoe UI", 10F);
            chkIsActive.Location = new Point(367, 173);
            chkIsActive.BackColor = Color.Transparent;
            chkIsActive.ForeColor = Color.FromArgb(16, 24, 40);
            chkIsActive.Dock = DockStyle.Fill;
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Margin = new Padding(3);
            chkIsActive.Size = new Size(70, 29);
            chkIsActive.TabIndex = 183;
            chkIsActive.Text = "نشط";
            chkIsActive.UseVisualStyleBackColor = false;
            // 
            // chkRequiresApproval
            // 
            chkRequiresApproval.AutoSize = true;
            chkRequiresApproval.Font = new Font("Segoe UI", 10F);
            chkRequiresApproval.Location = new Point(485, 173);
            chkRequiresApproval.BackColor = Color.Transparent;
            chkRequiresApproval.ForeColor = Color.FromArgb(16, 24, 40);
            chkRequiresApproval.Dock = DockStyle.Fill;
            chkRequiresApproval.Name = "chkRequiresApproval";
            chkRequiresApproval.Margin = new Padding(3);
            chkRequiresApproval.Size = new Size(136, 29);
            chkRequiresApproval.TabIndex = 182;
            chkRequiresApproval.Text = "يتطلب اعتماد";
            chkRequiresApproval.UseVisualStyleBackColor = false;
            // 
            // chkAllowTransfer
            // 
            chkAllowTransfer.AutoSize = true;
            chkAllowTransfer.Font = new Font("Segoe UI", 10F);
            chkAllowTransfer.Location = new Point(295, 133);
            chkAllowTransfer.BackColor = Color.Transparent;
            chkAllowTransfer.ForeColor = Color.FromArgb(16, 24, 40);
            chkAllowTransfer.Dock = DockStyle.Fill;
            chkAllowTransfer.Name = "chkAllowTransfer";
            chkAllowTransfer.Margin = new Padding(3);
            chkAllowTransfer.Size = new Size(142, 29);
            chkAllowTransfer.TabIndex = 181;
            chkAllowTransfer.Text = "يسمح بالترحيل";
            chkAllowTransfer.UseVisualStyleBackColor = false;
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(16, 93);
            txtDescription.BackColor = Color.White;
            txtDescription.ForeColor = Color.FromArgb(16, 24, 40);
            txtDescription.Name = "txtDescription";
            txtDescription.Font = new Font("Segoe UI", 11F);
            txtDescription.Margin = new Padding(3);
            txtDescription.Dock = DockStyle.Fill;
            txtDescription.Size = new Size(421, 27);
            txtDescription.TabIndex = 180;
            // 
            // label7
            // 
            label7.Dock = DockStyle.Fill;
            label7.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label7.Location = new Point(443, 90);
            label7.ForeColor = Color.FromArgb(16, 24, 40);
            label7.BackColor = Color.Transparent;
            label7.Name = "label7";
            label7.Margin = new Padding(3);
            label7.AutoEllipsis = true;
            label7.Size = new Size(178, 40);
            label7.TabIndex = 179;
            label7.Text = "الوصف";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // chkAllowDelivery
            // 
            chkAllowDelivery.AutoSize = true;
            chkAllowDelivery.Font = new Font("Segoe UI", 10F);
            chkAllowDelivery.Location = new Point(478, 133);
            chkAllowDelivery.BackColor = Color.Transparent;
            chkAllowDelivery.ForeColor = Color.FromArgb(16, 24, 40);
            chkAllowDelivery.Dock = DockStyle.Fill;
            chkAllowDelivery.Name = "chkAllowDelivery";
            chkAllowDelivery.Margin = new Padding(3);
            chkAllowDelivery.Size = new Size(143, 29);
            chkAllowDelivery.TabIndex = 178;
            chkAllowDelivery.Text = "يسمح بالتسليم";
            chkAllowDelivery.UseVisualStyleBackColor = false;
            // 
            // chkAllowCancel
            // 
            chkAllowCancel.AutoSize = true;
            chkAllowCancel.Font = new Font("Segoe UI", 10F);
            chkAllowCancel.Location = new Point(1100, 173);
            chkAllowCancel.BackColor = Color.Transparent;
            chkAllowCancel.ForeColor = Color.FromArgb(16, 24, 40);
            chkAllowCancel.Dock = DockStyle.Fill;
            chkAllowCancel.Name = "chkAllowCancel";
            chkAllowCancel.Margin = new Padding(3);
            chkAllowCancel.Size = new Size(134, 29);
            chkAllowCancel.TabIndex = 175;
            chkAllowCancel.Text = "يسمح بالالغاء";
            chkAllowCancel.UseVisualStyleBackColor = false;
            // 
            // chkIsFinal
            // 
            chkIsFinal.AutoSize = true;
            chkIsFinal.Font = new Font("Segoe UI", 10F);
            chkIsFinal.Location = new Point(939, 133);
            chkIsFinal.BackColor = Color.Transparent;
            chkIsFinal.ForeColor = Color.FromArgb(16, 24, 40);
            chkIsFinal.Dock = DockStyle.Fill;
            chkIsFinal.Name = "chkIsFinal";
            chkIsFinal.Margin = new Padding(3);
            chkIsFinal.Size = new Size(111, 29);
            chkIsFinal.TabIndex = 172;
            chkIsFinal.Text = "حالة نهائية";
            chkIsFinal.UseVisualStyleBackColor = false;
            // 
            // chkIsInitial
            // 
            chkIsInitial.AutoSize = true;
            chkIsInitial.Font = new Font("Segoe UI", 10F);
            chkIsInitial.Location = new Point(1112, 133);
            chkIsInitial.BackColor = Color.Transparent;
            chkIsInitial.ForeColor = Color.FromArgb(16, 24, 40);
            chkIsInitial.Dock = DockStyle.Fill;
            chkIsInitial.Name = "chkIsInitial";
            chkIsInitial.Margin = new Padding(3);
            chkIsInitial.Size = new Size(122, 29);
            chkIsInitial.TabIndex = 170;
            chkIsInitial.Text = "حالة ابتدائية";
            chkIsInitial.UseVisualStyleBackColor = false;
            // 
            // chkAllowEdit
            // 
            chkAllowEdit.AutoSize = true;
            chkAllowEdit.Font = new Font("Segoe UI", 10F);
            chkAllowEdit.Location = new Point(907, 173);
            chkAllowEdit.BackColor = Color.Transparent;
            chkAllowEdit.ForeColor = Color.FromArgb(16, 24, 40);
            chkAllowEdit.Dock = DockStyle.Fill;
            chkAllowEdit.Name = "chkAllowEdit";
            chkAllowEdit.Margin = new Padding(3);
            chkAllowEdit.Size = new Size(143, 29);
            chkAllowEdit.TabIndex = 168;
            chkAllowEdit.Text = "يسمح بالتعديل";
            chkAllowEdit.UseVisualStyleBackColor = false;
            // 
            // nudDisplayOrder
            // 
            nudDisplayOrder.Dock = DockStyle.Fill;
            nudDisplayOrder.Location = new Point(627, 93);
            nudDisplayOrder.BackColor = Color.White;
            nudDisplayOrder.ForeColor = Color.FromArgb(16, 24, 40);
            nudDisplayOrder.Name = "nudDisplayOrder";
            nudDisplayOrder.Font = new Font("Segoe UI", 11F);
            nudDisplayOrder.Margin = new Padding(3);
            nudDisplayOrder.Size = new Size(423, 27);
            nudDisplayOrder.TabIndex = 161;
            // 
            // label8
            // 
            label8.Dock = DockStyle.Fill;
            label8.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label8.Location = new Point(1056, 90);
            label8.ForeColor = Color.FromArgb(16, 24, 40);
            label8.BackColor = Color.Transparent;
            label8.Name = "label8";
            label8.Margin = new Padding(3);
            label8.AutoEllipsis = true;
            label8.Size = new Size(178, 40);
            label8.TabIndex = 160;
            label8.Text = "ترتيب العرض";
            label8.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbStatusColor
            // 
            cmbStatusColor.Dock = DockStyle.Fill;
            cmbStatusColor.FormattingEnabled = true;
            cmbStatusColor.Items.AddRange(new object[] { "وزن", "حجم", "قطعة", "قيمة", "سعر ثابت" });
            cmbStatusColor.Location = new Point(627, 53);
            cmbStatusColor.BackColor = Color.White;
            cmbStatusColor.ForeColor = Color.FromArgb(16, 24, 40);
            cmbStatusColor.Name = "cmbStatusColor";
            cmbStatusColor.Font = new Font("Segoe UI", 11F);
            cmbStatusColor.Margin = new Padding(3);
            cmbStatusColor.Size = new Size(423, 28);
            cmbStatusColor.TabIndex = 124;
            // 
            // label9
            // 
            label9.Dock = DockStyle.Fill;
            label9.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label9.Location = new Point(1056, 50);
            label9.ForeColor = Color.FromArgb(16, 24, 40);
            label9.BackColor = Color.Transparent;
            label9.Name = "label9";
            label9.Margin = new Padding(3);
            label9.AutoEllipsis = true;
            label9.Size = new Size(178, 40);
            label9.TabIndex = 123;
            label9.Text = "لون الحالة ";
            label9.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtStatusNameEn
            // 
            txtStatusNameEn.Dock = DockStyle.Fill;
            txtStatusNameEn.Location = new Point(15, 55);
            txtStatusNameEn.Margin = new Padding(3);
            txtStatusNameEn.BackColor = Color.White;
            txtStatusNameEn.ForeColor = Color.FromArgb(16, 24, 40);
            txtStatusNameEn.Name = "txtStatusNameEn";
            txtStatusNameEn.Font = new Font("Segoe UI", 11F);
            txtStatusNameEn.Size = new Size(420, 27);
            txtStatusNameEn.TabIndex = 105;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.Location = new Point(443, 10);
            label1.ForeColor = Color.FromArgb(16, 24, 40);
            label1.BackColor = Color.Transparent;
            label1.Name = "label1";
            label1.Margin = new Padding(3);
            label1.AutoEllipsis = true;
            label1.Size = new Size(178, 40);
            label1.TabIndex = 99;
            label1.Text = "اسم الحاله عربي ";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtStatusNameAr
            // 
            txtStatusNameAr.Dock = DockStyle.Fill;
            txtStatusNameAr.Location = new Point(15, 15);
            txtStatusNameAr.Margin = new Padding(3);
            txtStatusNameAr.BackColor = Color.White;
            txtStatusNameAr.ForeColor = Color.FromArgb(16, 24, 40);
            txtStatusNameAr.Name = "txtStatusNameAr";
            txtStatusNameAr.Font = new Font("Segoe UI", 11F);
            txtStatusNameAr.Size = new Size(420, 27);
            txtStatusNameAr.TabIndex = 75;
            // 
            // lblPriority
            // 
            lblPriority.Dock = DockStyle.Fill;
            lblPriority.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPriority.Location = new Point(443, 50);
            lblPriority.ForeColor = Color.FromArgb(16, 24, 40);
            lblPriority.BackColor = Color.Transparent;
            lblPriority.Name = "lblPriority";
            lblPriority.Margin = new Padding(3);
            lblPriority.AutoEllipsis = true;
            lblPriority.Size = new Size(178, 40);
            lblPriority.TabIndex = 6;
            lblPriority.Text = "اسم الحاله انجليزي";
            lblPriority.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPricingRuleCode
            // 
            lblPricingRuleCode.Dock = DockStyle.Fill;
            lblPricingRuleCode.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPricingRuleCode.Location = new Point(1056, 10);
            lblPricingRuleCode.ForeColor = Color.FromArgb(16, 24, 40);
            lblPricingRuleCode.BackColor = Color.Transparent;
            lblPricingRuleCode.Name = "lblPricingRuleCode";
            lblPricingRuleCode.Margin = new Padding(3);
            lblPricingRuleCode.AutoEllipsis = true;
            lblPricingRuleCode.Size = new Size(178, 40);
            lblPricingRuleCode.TabIndex = 0;
            lblPricingRuleCode.Text = "كود الحاله";
            lblPricingRuleCode.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtStatusCode
            // 
            txtStatusCode.Dock = DockStyle.Fill;
            txtStatusCode.Location = new Point(629, 15);
            txtStatusCode.Margin = new Padding(3);
            txtStatusCode.BackColor = Color.White;
            txtStatusCode.ForeColor = Color.FromArgb(16, 24, 40);
            txtStatusCode.Name = "txtStatusCode";
            txtStatusCode.Font = new Font("Segoe UI", 11F);
            txtStatusCode.Size = new Size(419, 27);
            txtStatusCode.TabIndex = 1;
            // 
            // txtNotes
            // 
            txtNotes.Location = new Point(629, 213);
            txtNotes.Multiline = true;
            txtNotes.BackColor = Color.White;
            txtNotes.ForeColor = Color.FromArgb(16, 24, 40);
            txtNotes.Name = "txtNotes";
            txtNotes.Font = new Font("Segoe UI", 11F);
            txtNotes.Margin = new Padding(3);
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Size = new Size(421, 34);
            txtNotes.TabIndex = 185;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 29);
            tabPage1.BackColor = Color.LightCyan;
            tabPage1.Margin = new Padding(0);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1247, 295);
            tabPage1.TabIndex = 3;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = false;
            // 
            // UcShipmentStatuses
            // 
            AccessibleName = "حالات البوالص";
            AutoScaleDimensions = new SizeF(120F, 120F);
            BackColor = Color.FromArgb(248, 250, 252);
            ForeColor = Color.FromArgb(16, 24, 40);
            Padding = new Padding(0);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            Controls.Add(tabMain);
            Controls.Add(pnlHeader);
            Controls.Add(tlpAuditInfo);
            Margin = new Padding(0);
            MinimumSize = new Size(650, 1000);
            Name = "UcShipmentStatuses";
            Font = new Font("Segoe UI", 10F);
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1255, 1000);
            tlpAuditInfo.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            tabMain.ResumeLayout(false);
            tabPricingRules.ResumeLayout(false);
            tlpPricingRuleFields.ResumeLayout(false);
            tlpPricingRuleFields.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudDisplayOrder).EndInit();
            ResumeLayout(false);
        
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(194, 0);
        designerCommandBar.Size = new Size(1061, 53);
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
        private TabControl tabMain;
        private TabPage tabPricingRules;
        private NumericUpDown nudPrice;
        private Label lblPrice;
        private NumericUpDown nudMaximumValue;
        private NumericUpDown nudMinimumValue;
        private DateTimePicker dtpEffectiveTo;
        private ComboBox cmbPricingBasis;
        private ComboBox cmbTransportMethod;
        private ComboBox cmbShipmentType;
        private TabPage tabPage1;
        private TableLayoutPanel tlpPricingRuleFields;
        private NumericUpDown nudDisplayOrder;
        private Label label8;
        private ComboBox cmbStatusColor;
        private Label label9;
        private TextBox txtStatusNameEn;
        private Label label1;
        private TextBox txtStatusNameAr;
        private Label lblPriority;
        private Label lblPricingRuleCode;
        private TextBox txtStatusCode;
        private Label label2;
        private CheckBox chkIsActive;
        private CheckBox chkRequiresApproval;
        private CheckBox chkAllowTransfer;
        private TextBox txtDescription;
        private Label label7;
        private CheckBox chkAllowDelivery;
        private CheckBox chkAllowCancel;
        private CheckBox chkIsFinal;
        private CheckBox chkIsInitial;
        private CheckBox chkAllowEdit;
        private TextBox txtNotes;
    }
}
