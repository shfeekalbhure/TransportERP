namespace TransportERP.Desktop.البوالص_والشحن.الاعدادات
{
    partial class UcShipmentUnits
    {
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;

    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
    private Panel designerCloseHost = null!;
    private Button standardCommandCancel = null!;
    private Button standardCommandView = null!;
    private Button standardCommandLast = null!;
    private Button standardCommandNext = null!;
    private Button standardCommandPrevious = null!;
    private Button standardCommandFirst = null!;
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
        designerCloseHost = new Panel();
        standardCommandCancel = new Button();
        standardCommandView = new Button();
        standardCommandLast = new Button();
        standardCommandNext = new Button();
        standardCommandPrevious = new Button();
        standardCommandFirst = new Button();
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
            lblTitle = new Label();
            tlpAuditInfo = new TableLayoutPanel();
            lblPrintCount = new Label();
            lblLastPrintedAt = new Label();
            lblEditCount = new Label();
            lblModifiedAt = new Label();
            lblModifiedBy = new Label();
            lblCreatedAt = new Label();
            lblCreatedBy = new Label();
            panel1 = new Panel();
            tlpShipmentCategory = new TableLayoutPanel();
            txtNotes = new Label();
            textBox3 = new TextBox();
            numericUpDown2 = new NumericUpDown();
            nudDisplayOrder = new Label();
            cmbBaseUnit = new Label();
            txtUnitCode = new Label();
            txtCategoryCode = new TextBox();
            txtUnitNameAr = new Label();
            txtCategoryName = new TextBox();
            cmbUnitType = new Label();
            txtCategoryNameEn = new TextBox();
            txtUnitSymbol = new Label();
            comboBox1 = new ComboBox();
            textBox1 = new TextBox();
            numericUpDown1 = new NumericUpDown();
            label1 = new Label();
            txtUnitNameEn = new Label();
            comboBox2 = new ComboBox();
            txtDescription = new Label();
            textBox2 = new TextBox();
            chkIsPackageUnit = new CheckBox();
            chkIsActive = new CheckBox();
            lblProperties = new Label();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            tlpAuditInfo.SuspendLayout();
            panel1.SuspendLayout();
            tlpShipmentCategory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
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
            pnlHeader.Size = new Size(1200, 61);
            pnlHeader.TabIndex = 4;
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.FromArgb(224, 224, 224);

            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(289, 0);
            pnlToolbar.Margin = new Padding(5);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.MinimumSize = new Size(0, 48);
            pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlToolbar.AutoSize = true;
            pnlToolbar.Padding = new Padding(5);
            pnlToolbar.Size = new Size(911, 61);
            pnlToolbar.TabIndex = 2;
            pnlToolbar.WrapContents = true;
            // 
            // btnNew
            // 
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Microsoft Sans Serif", 10F);
            btnNew.Location = new Point(794, 11);
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
            btnSave.Location = new Point(685, 11);
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
            btnEdit.Location = new Point(576, 11);
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
            btnDelete.Location = new Point(467, 11);
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
            btnRefresh.Location = new Point(358, 11);
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
            btnClose.Location = new Point(249, 11);
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
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Dock = DockStyle.Left;
            lblTitle.Font = new Font("Segoe UI", 16F);
            lblTitle.Location = new Point(0, 0);
            lblTitle.ForeColor = Color.FromArgb(16, 24, 40);
            lblTitle.BackColor = Color.FromArgb(192, 192, 255);
            lblTitle.Name = "lblTitle";
            lblTitle.Padding = new Padding(0, 0, 17, 0);
            lblTitle.RightToLeft = RightToLeft.Yes;
            lblTitle.Size = new Size(289, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "وحدات القياس والتعبئة";
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
            tlpAuditInfo.TabIndex = 5;
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
            // panel1
            // 
            panel1.BackColor = Color.LightCyan;
            panel1.Controls.Add(tlpShipmentCategory);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 61);
            panel1.Name = "panel1";
            panel1.AutoScroll = true;
            panel1.Size = new Size(1200, 309);
            panel1.TabIndex = 6;
            // 
            // tlpShipmentCategory
            // 
            tlpShipmentCategory.BackColor = Color.LightCyan;
            tlpShipmentCategory.ColumnCount = 4;
            tlpShipmentCategory.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
            tlpShipmentCategory.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36F));
            tlpShipmentCategory.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
            tlpShipmentCategory.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36F));
            tlpShipmentCategory.Controls.Add(txtNotes, 0, 5);
            tlpShipmentCategory.Controls.Add(textBox3, 1, 5);
            tlpShipmentCategory.Controls.Add(numericUpDown2, 3, 3);
            tlpShipmentCategory.Controls.Add(nudDisplayOrder, 2, 3);
            tlpShipmentCategory.Controls.Add(cmbBaseUnit, 2, 2);
            tlpShipmentCategory.Controls.Add(txtUnitCode, 0, 0);
            tlpShipmentCategory.Controls.Add(txtCategoryCode, 1, 0);
            tlpShipmentCategory.Controls.Add(txtUnitNameAr, 2, 0);
            tlpShipmentCategory.Controls.Add(txtCategoryName, 3, 0);
            tlpShipmentCategory.Controls.Add(cmbUnitType, 0, 1);
            tlpShipmentCategory.Controls.Add(txtCategoryNameEn, 3, 1);
            tlpShipmentCategory.Controls.Add(txtUnitSymbol, 0, 2);
            tlpShipmentCategory.Controls.Add(comboBox1, 1, 1);
            tlpShipmentCategory.Controls.Add(textBox1, 1, 2);
            tlpShipmentCategory.Controls.Add(numericUpDown1, 1, 3);
            tlpShipmentCategory.Controls.Add(label1, 0, 3);
            tlpShipmentCategory.Controls.Add(txtUnitNameEn, 2, 1);
            tlpShipmentCategory.Controls.Add(comboBox2, 3, 2);
            tlpShipmentCategory.Controls.Add(txtDescription, 0, 4);
            tlpShipmentCategory.Controls.Add(textBox2, 1, 4);
            tlpShipmentCategory.Controls.Add(chkIsPackageUnit, 3, 4);
            tlpShipmentCategory.Controls.Add(chkIsActive, 2, 4);
            tlpShipmentCategory.Dock = DockStyle.Top;
            tlpShipmentCategory.Location = new Point(0, 0);
            tlpShipmentCategory.Margin = new Padding(0);
            tlpShipmentCategory.Name = "tlpShipmentCategory";
            tlpShipmentCategory.MinimumSize = new Size(0, 272);
            tlpShipmentCategory.Padding = new Padding(0);
            tlpShipmentCategory.RowCount = 6;
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            tlpShipmentCategory.Size = new Size(1200, 272);
            tlpShipmentCategory.TabIndex = 1;
            tlpShipmentCategory.TabStop = true;
            tlpShipmentCategory.Paint += tlpShipmentCategory_Paint_1;
            // 
            // txtNotes
            // 
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Location = new Point(1028, 220);
            txtNotes.ForeColor = Color.FromArgb(16, 24, 40);
            txtNotes.BackColor = Color.Transparent;
            txtNotes.Name = "txtNotes";
            txtNotes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            txtNotes.Margin = new Padding(3);
            txtNotes.AutoEllipsis = true;
            txtNotes.Size = new Size(159, 47);
            txtNotes.TabIndex = 29;
            txtNotes.Text = "ملاحظات";
            // 
            // textBox3
            // 
            textBox3.Dock = DockStyle.Fill;
            textBox3.Location = new Point(604, 223);
            textBox3.Multiline = true;
            textBox3.BackColor = Color.White;
            textBox3.ForeColor = Color.FromArgb(16, 24, 40);
            textBox3.Name = "textBox3";
            textBox3.Font = new Font("Segoe UI", 11F);
            textBox3.Margin = new Padding(3);
            textBox3.Size = new Size(418, 41);
            textBox3.TabIndex = 30;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(13, 139);
            numericUpDown2.BackColor = Color.White;
            numericUpDown2.ForeColor = Color.FromArgb(16, 24, 40);
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Font = new Font("Segoe UI", 11F);
            numericUpDown2.Margin = new Padding(3);
            numericUpDown2.Dock = DockStyle.Fill;
            numericUpDown2.Size = new Size(420, 30);
            numericUpDown2.TabIndex = 23;
            // 
            // nudDisplayOrder
            // 
            nudDisplayOrder.Location = new Point(439, 136);
            nudDisplayOrder.ForeColor = Color.FromArgb(16, 24, 40);
            nudDisplayOrder.BackColor = Color.Transparent;
            nudDisplayOrder.Name = "nudDisplayOrder";
            nudDisplayOrder.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            nudDisplayOrder.Margin = new Padding(3);
            nudDisplayOrder.AutoEllipsis = true;
            nudDisplayOrder.Dock = DockStyle.Fill;
            nudDisplayOrder.Size = new Size(159, 34);
            nudDisplayOrder.TabIndex = 22;
            nudDisplayOrder.Text = "ترتيب العرض";
            // 
            // cmbBaseUnit
            // 
            cmbBaseUnit.Dock = DockStyle.Fill;
            cmbBaseUnit.Location = new Point(439, 94);
            cmbBaseUnit.ForeColor = Color.FromArgb(16, 24, 40);
            cmbBaseUnit.BackColor = Color.Transparent;
            cmbBaseUnit.Name = "cmbBaseUnit";
            cmbBaseUnit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            cmbBaseUnit.Margin = new Padding(3);
            cmbBaseUnit.AutoEllipsis = true;
            cmbBaseUnit.Size = new Size(159, 42);
            cmbBaseUnit.TabIndex = 10;
            cmbBaseUnit.Text = "الوحده الاساسيه ";
            // 
            // txtUnitCode
            // 
            txtUnitCode.Dock = DockStyle.Fill;
            txtUnitCode.Location = new Point(1028, 10);
            txtUnitCode.ForeColor = Color.FromArgb(16, 24, 40);
            txtUnitCode.BackColor = Color.Transparent;
            txtUnitCode.Name = "txtUnitCode";
            txtUnitCode.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            txtUnitCode.Margin = new Padding(3);
            txtUnitCode.AutoEllipsis = true;
            txtUnitCode.Size = new Size(159, 42);
            txtUnitCode.TabIndex = 0;
            txtUnitCode.Text = "كود الوحده";
            // 
            // txtCategoryCode
            // 
            txtCategoryCode.Dock = DockStyle.Fill;
            txtCategoryCode.Location = new Point(850, 15);
            txtCategoryCode.Margin = new Padding(3);
            txtCategoryCode.BackColor = Color.White;
            txtCategoryCode.ForeColor = Color.FromArgb(16, 24, 40);
            txtCategoryCode.Name = "txtCategoryCode";
            txtCategoryCode.Font = new Font("Segoe UI", 11F);
            txtCategoryCode.Size = new Size(170, 30);
            txtCategoryCode.TabIndex = 0;
            // 
            // txtUnitNameAr
            // 
            txtUnitNameAr.Dock = DockStyle.Fill;
            txtUnitNameAr.Location = new Point(439, 10);
            txtUnitNameAr.ForeColor = Color.FromArgb(16, 24, 40);
            txtUnitNameAr.BackColor = Color.Transparent;
            txtUnitNameAr.Name = "txtUnitNameAr";
            txtUnitNameAr.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            txtUnitNameAr.Margin = new Padding(3);
            txtUnitNameAr.AutoEllipsis = true;
            txtUnitNameAr.Size = new Size(159, 42);
            txtUnitNameAr.TabIndex = 1;
            txtUnitNameAr.Text = "اسم الوحده بالعربي";
            // 
            // txtCategoryName
            // 
            txtCategoryName.Dock = DockStyle.Fill;
            txtCategoryName.Location = new Point(15, 15);
            txtCategoryName.Margin = new Padding(3);
            txtCategoryName.BackColor = Color.White;
            txtCategoryName.ForeColor = Color.FromArgb(16, 24, 40);
            txtCategoryName.Name = "txtCategoryName";
            txtCategoryName.Font = new Font("Segoe UI", 11F);
            txtCategoryName.Size = new Size(416, 30);
            txtCategoryName.TabIndex = 1;
            // 
            // cmbUnitType
            // 
            cmbUnitType.Dock = DockStyle.Fill;
            cmbUnitType.Location = new Point(1028, 52);
            cmbUnitType.ForeColor = Color.FromArgb(16, 24, 40);
            cmbUnitType.BackColor = Color.Transparent;
            cmbUnitType.Name = "cmbUnitType";
            cmbUnitType.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            cmbUnitType.Margin = new Padding(3);
            cmbUnitType.AutoEllipsis = true;
            cmbUnitType.Size = new Size(159, 42);
            cmbUnitType.TabIndex = 2;
            cmbUnitType.Text = "نوع الوحده";
            // 
            // txtCategoryNameEn
            // 
            txtCategoryNameEn.Dock = DockStyle.Fill;
            txtCategoryNameEn.Location = new Point(15, 57);
            txtCategoryNameEn.Margin = new Padding(3);
            txtCategoryNameEn.BackColor = Color.White;
            txtCategoryNameEn.ForeColor = Color.FromArgb(16, 24, 40);
            txtCategoryNameEn.Name = "txtCategoryNameEn";
            txtCategoryNameEn.Font = new Font("Segoe UI", 11F);
            txtCategoryNameEn.RightToLeft = RightToLeft.No;
            txtCategoryNameEn.Size = new Size(416, 30);
            txtCategoryNameEn.TabIndex = 3;
            // 
            // txtUnitSymbol
            // 
            txtUnitSymbol.Location = new Point(1028, 94);
            txtUnitSymbol.ForeColor = Color.FromArgb(16, 24, 40);
            txtUnitSymbol.BackColor = Color.Transparent;
            txtUnitSymbol.Name = "txtUnitSymbol";
            txtUnitSymbol.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            txtUnitSymbol.Margin = new Padding(3);
            txtUnitSymbol.AutoEllipsis = true;
            txtUnitSymbol.Dock = DockStyle.Fill;
            txtUnitSymbol.Size = new Size(159, 34);
            txtUnitSymbol.TabIndex = 4;
            txtUnitSymbol.Text = "الرمز المختصر";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(718, 55);
            comboBox1.BackColor = Color.White;
            comboBox1.ForeColor = Color.FromArgb(16, 24, 40);
            comboBox1.Name = "comboBox1";
            comboBox1.Font = new Font("Segoe UI", 11F);
            comboBox1.Margin = new Padding(3);
            comboBox1.Dock = DockStyle.Fill;
            comboBox1.Size = new Size(304, 31);
            comboBox1.TabIndex = 8;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(733, 97);
            textBox1.BackColor = Color.White;
            textBox1.ForeColor = Color.FromArgb(16, 24, 40);
            textBox1.Name = "textBox1";
            textBox1.Font = new Font("Segoe UI", 11F);
            textBox1.Margin = new Padding(3);
            textBox1.Dock = DockStyle.Fill;
            textBox1.Size = new Size(289, 30);
            textBox1.TabIndex = 9;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(604, 139);
            numericUpDown1.BackColor = Color.White;
            numericUpDown1.ForeColor = Color.FromArgb(16, 24, 40);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Font = new Font("Segoe UI", 11F);
            numericUpDown1.Margin = new Padding(3);
            numericUpDown1.Dock = DockStyle.Fill;
            numericUpDown1.Size = new Size(418, 30);
            numericUpDown1.TabIndex = 13;
            // 
            // label1
            // 
            label1.Location = new Point(1028, 136);
            label1.ForeColor = Color.FromArgb(16, 24, 40);
            label1.BackColor = Color.Transparent;
            label1.Name = "label1";
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.Margin = new Padding(3);
            label1.AutoEllipsis = true;
            label1.Dock = DockStyle.Fill;
            label1.Size = new Size(159, 34);
            label1.TabIndex = 16;
            label1.Text = "معامل التحويل";
            // 
            // txtUnitNameEn
            // 
            txtUnitNameEn.Dock = DockStyle.Fill;
            txtUnitNameEn.Location = new Point(439, 52);
            txtUnitNameEn.ForeColor = Color.FromArgb(16, 24, 40);
            txtUnitNameEn.BackColor = Color.Transparent;
            txtUnitNameEn.Name = "txtUnitNameEn";
            txtUnitNameEn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            txtUnitNameEn.Margin = new Padding(3);
            txtUnitNameEn.AutoEllipsis = true;
            txtUnitNameEn.Size = new Size(159, 42);
            txtUnitNameEn.TabIndex = 3;
            txtUnitNameEn.Text = "اسم الوحده بالانجليزي";
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(129, 97);
            comboBox2.BackColor = Color.White;
            comboBox2.ForeColor = Color.FromArgb(16, 24, 40);
            comboBox2.Name = "comboBox2";
            comboBox2.Font = new Font("Segoe UI", 11F);
            comboBox2.Margin = new Padding(3);
            comboBox2.Dock = DockStyle.Fill;
            comboBox2.Size = new Size(304, 31);
            comboBox2.TabIndex = 11;
            // 
            // txtDescription
            // 
            txtDescription.Dock = DockStyle.Fill;
            txtDescription.Location = new Point(1028, 178);
            txtDescription.ForeColor = Color.FromArgb(16, 24, 40);
            txtDescription.BackColor = Color.Transparent;
            txtDescription.Name = "txtDescription";
            txtDescription.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            txtDescription.Margin = new Padding(3);
            txtDescription.AutoEllipsis = true;
            txtDescription.Size = new Size(159, 42);
            txtDescription.TabIndex = 25;
            txtDescription.Text = "الوصف";
            // 
            // textBox2
            // 
            textBox2.Dock = DockStyle.Fill;
            textBox2.Location = new Point(604, 181);
            textBox2.BackColor = Color.White;
            textBox2.ForeColor = Color.FromArgb(16, 24, 40);
            textBox2.Name = "textBox2";
            textBox2.Font = new Font("Segoe UI", 11F);
            textBox2.Margin = new Padding(3);
            textBox2.Size = new Size(418, 30);
            textBox2.TabIndex = 26;
            // 
            // chkIsPackageUnit
            // 
            chkIsPackageUnit.Location = new Point(272, 181);
            chkIsPackageUnit.BackColor = Color.Transparent;
            chkIsPackageUnit.ForeColor = Color.FromArgb(16, 24, 40);
            chkIsPackageUnit.UseVisualStyleBackColor = false;
            chkIsPackageUnit.Dock = DockStyle.Fill;
            chkIsPackageUnit.Name = "chkIsPackageUnit";
            chkIsPackageUnit.Font = new Font("Segoe UI", 10F);
            chkIsPackageUnit.Margin = new Padding(3);
            chkIsPackageUnit.Size = new Size(161, 24);
            chkIsPackageUnit.TabIndex = 7;
            chkIsPackageUnit.Text = "وحدة التعبئة";
            // 
            // chkIsActive
            // 
            chkIsActive.Checked = true;
            chkIsActive.CheckState = CheckState.Checked;
            chkIsActive.Location = new Point(455, 181);
            chkIsActive.BackColor = Color.Transparent;
            chkIsActive.ForeColor = Color.FromArgb(16, 24, 40);
            chkIsActive.UseVisualStyleBackColor = false;
            chkIsActive.Dock = DockStyle.Fill;
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Font = new Font("Segoe UI", 10F);
            chkIsActive.Margin = new Padding(3);
            chkIsActive.Size = new Size(143, 24);
            chkIsActive.TabIndex = 9;
            chkIsActive.Text = "نشط";
            // 
            // lblProperties
            // 
            lblProperties.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblProperties.AutoSize = true;
            lblProperties.Location = new Point(1013, 330);
            lblProperties.ForeColor = Color.FromArgb(16, 24, 40);
            lblProperties.BackColor = Color.Transparent;
            lblProperties.Name = "lblProperties";
            lblProperties.Size = new Size(0, 23);
            lblProperties.TabIndex = 6;
            // 
            // UcShipmentUnits
            // 
            AccessibleName = "وحدات القياس والتعبئة";
            AutoScaleDimensions = new SizeF(120F, 120F);
            ForeColor = Color.FromArgb(16, 24, 40);
            Padding = new Padding(0);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScroll = true;
            BackColor = Color.FromArgb(248, 250, 252);
            Controls.Add(panel1);
            Controls.Add(tlpAuditInfo);
            Controls.Add(pnlHeader);
            Controls.Add(lblProperties);
            Font = new Font("Segoe UI", 10F);
            Margin = new Padding(0);
            MinimumSize = new Size(1000, 650);
            Name = "UcShipmentUnits";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1200, 800);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            tlpAuditInfo.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tlpShipmentCategory.ResumeLayout(false);
            tlpShipmentCategory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(289, 0);
        designerCommandBar.Size = new Size(911, 61);
        designerCommandBar.Margin = new Padding(5);
        designerCommandBar.MinimumSize = new Size(0, 48);
        designerCommandBar.TabIndex = 2;
        designerCommandBar.Controls.Add(pnlToolbar);
        pnlToolbar.Dock = DockStyle.Fill;
        pnlToolbar.Margin = Padding.Empty;
        designerCommandBar.Name = "designerCommandBar";
        designerCommandBar.BackColor = Color.FromArgb(239, 239, 239);
        designerCommandBar.BorderStyle = BorderStyle.FixedSingle;
        designerCommandBar.Controls.Add(designerCloseHost);
        designerCloseHost.Dock = DockStyle.Left;
        designerCloseHost.Width = 28;
        designerCloseHost.Name = "designerCloseHost";
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Dock = DockStyle.Fill;
        pnlToolbar.AutoSize = false;
        pnlToolbar.WrapContents = false;
        pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
        pnlToolbar.RightToLeft = RightToLeft.No;
        pnlToolbar.Padding = new Padding(2);
        designerCommandBar.MinimumSize = new Size(0, 30);
        designerCommandBar.Height = 44;
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
        standardCommandPrint.Name = "standardCommandPrint";
        standardCommandPrint.Enabled = false;
        standardCommandPrint.Visible = true;
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
        btnNew.AutoSize = false;
        btnNew.Dock = DockStyle.None;
        btnNew.MinimumSize = Size.Empty;
        btnNew.Size = new Size(26, 24);
        btnNew.Margin = new Padding(1);
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnNew.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Add;
        btnNew.Text = "";
        pnlToolbar.Controls.Add(btnNew);
        designerCommandBar.SetCommandRole(btnNew, TransportERP.Desktop.CoreUI.DesignerCommandRole.Add);
        btnEdit.AutoSize = false;
        btnEdit.Dock = DockStyle.None;
        btnEdit.MinimumSize = Size.Empty;
        btnEdit.Size = new Size(26, 24);
        btnEdit.Margin = new Padding(1);
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnEdit.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Edit;
        btnEdit.Text = "";
        pnlToolbar.Controls.Add(btnEdit);
        designerCommandBar.SetCommandRole(btnEdit, TransportERP.Desktop.CoreUI.DesignerCommandRole.Edit);
        btnDelete.AutoSize = false;
        btnDelete.Dock = DockStyle.None;
        btnDelete.MinimumSize = Size.Empty;
        btnDelete.Size = new Size(26, 24);
        btnDelete.Margin = new Padding(1);
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnDelete.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Delete;
        btnDelete.Text = "";
        pnlToolbar.Controls.Add(btnDelete);
        designerCommandBar.SetCommandRole(btnDelete, TransportERP.Desktop.CoreUI.DesignerCommandRole.Delete);
        standardCommandCancel.AutoSize = false;
        standardCommandCancel.Dock = DockStyle.None;
        standardCommandCancel.MinimumSize = Size.Empty;
        standardCommandCancel.Size = new Size(26, 24);
        standardCommandCancel.Margin = new Padding(1);
        standardCommandCancel.FlatStyle = FlatStyle.Flat;
        standardCommandCancel.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandCancel.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Cancel;
        standardCommandCancel.Text = "";
        pnlToolbar.Controls.Add(standardCommandCancel);
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
        pnlToolbar.Controls.Add(standardCommandView);
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
        pnlToolbar.Controls.Add(standardCommandLast);
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
        pnlToolbar.Controls.Add(standardCommandNext);
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
        pnlToolbar.Controls.Add(standardCommandPrevious);
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
        pnlToolbar.Controls.Add(standardCommandFirst);
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
        pnlToolbar.Controls.Add(btnSave);
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
        pnlToolbar.Controls.Add(standardCommandPrint);
        designerCommandBar.SetCommandRole(standardCommandPrint, TransportERP.Desktop.CoreUI.DesignerCommandRole.Print);
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
        btnRefresh.AutoSize = false;
        btnRefresh.Dock = DockStyle.None;
        btnRefresh.MinimumSize = Size.Empty;
        btnRefresh.Size = new Size(26, 24);
        btnRefresh.Margin = new Padding(1);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnRefresh.Text = "تحديث";
        pnlToolbar.Controls.Add(btnRefresh);
        designerCommandBar.SetCommandRole(btnRefresh, TransportERP.Desktop.CoreUI.DesignerCommandRole.Refresh);
        standardCommandImport.AutoSize = false;
        standardCommandImport.Dock = DockStyle.None;
        standardCommandImport.MinimumSize = Size.Empty;
        standardCommandImport.Size = new Size(26, 24);
        standardCommandImport.Margin = new Padding(1);
        standardCommandImport.FlatStyle = FlatStyle.Flat;
        standardCommandImport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandImport.Text = "استيراد";
        pnlToolbar.Controls.Add(standardCommandImport);
        designerCommandBar.SetCommandRole(standardCommandImport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Import);
        standardCommandExport.AutoSize = false;
        standardCommandExport.Dock = DockStyle.None;
        standardCommandExport.MinimumSize = Size.Empty;
        standardCommandExport.Size = new Size(26, 24);
        standardCommandExport.Margin = new Padding(1);
        standardCommandExport.FlatStyle = FlatStyle.Flat;
        standardCommandExport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandExport.Text = "تصدير";
        pnlToolbar.Controls.Add(standardCommandExport);
        designerCommandBar.SetCommandRole(standardCommandExport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Export);
        standardCommandHelp.AutoSize = false;
        standardCommandHelp.Dock = DockStyle.None;
        standardCommandHelp.MinimumSize = Size.Empty;
        standardCommandHelp.Size = new Size(26, 24);
        standardCommandHelp.Margin = new Padding(1);
        standardCommandHelp.FlatStyle = FlatStyle.Flat;
        standardCommandHelp.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandHelp.Text = "مساعدة";
        pnlToolbar.Controls.Add(standardCommandHelp);
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
        private Label lblTitle;
        private TableLayoutPanel tlpAuditInfo;
        private Label lblPrintCount;
        private Label lblLastPrintedAt;
        private Label lblEditCount;
        private Label lblModifiedAt;
        private Label lblModifiedBy;
        private Label lblCreatedAt;
        private Label lblCreatedBy;
        private Panel panel1;
        private Label lblProperties;
        private TableLayoutPanel tlpShipmentCategory;
        private Label cmbBaseUnit;
        private Label txtUnitCode;
        private TextBox txtCategoryCode;
        private Label txtUnitNameAr;
        private TextBox txtCategoryName;
        private Label cmbUnitType;
        private TextBox txtCategoryNameEn;
        private Label txtUnitSymbol;
        private CheckBox chkIsPackageUnit;
        private ComboBox comboBox1;
        private TextBox textBox1;
        private NumericUpDown numericUpDown1;
        private Label label1;
        private Label txtUnitNameEn;
        private ComboBox comboBox2;
        private NumericUpDown numericUpDown2;
        private Label nudDisplayOrder;
        private Label txtDescription;
        private TextBox textBox2;
        private CheckBox chkIsActive;
        private Label txtNotes;
        private TextBox textBox3;
    }
}
