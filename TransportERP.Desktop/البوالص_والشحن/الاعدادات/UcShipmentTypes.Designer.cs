namespace TransportERP.Desktop.البوالص_والشحن.الاعدادات
{
    partial class UcShipmentTypes
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
        // 00 - تحرير الموارد التي يستخدمها هذا الـ UserControl عند إغلاقه.
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
        // 00 - إنشاء عناصر الشاشة وضبط خصائصها ومواقعها وربط الأحداث.
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
            DataGridView = new Label();
            label12 = new Label();
            lblDescription = new Label();
            textDescription = new TextBox();
            lblShipmentTypeNameEn = new Label();
            txtShipmentTypeNameEn = new TextBox();
            lblDisplay0rder = new Label();
            lblShipmentTypeName = new Label();
            txtShipmentTypeName = new TextBox();
            lblShipmentTypeCode = new Label();
            txtShipmentTypeCode = new TextBox();
            dataGridView1 = new DataGridView();
            chkIsActive = new CheckBox();
            dgvShipmentTypes = new TableLayoutPanel();
            nudDisplay0rder = new NumericUpDown();
            lblPrintCount = new Label();
            lblLastPrintedAt = new Label();
            lblEditCount = new Label();
            pnlContent = new Panel();
            pnlHeader = new Panel();
            pnlToolbar = new FlowLayoutPanel();
            btnNew = new Button();
            btnSave = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            btnClose = new Button();
            lblTitle = new Label();
            panel1 = new Panel();
            tlpAuditInfo = new TableLayoutPanel();
            lblModifiedAt = new Label();
            lblModifiedBy = new Label();
            lblCreatedAt = new Label();
            lblCreatedBy = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            dgvShipmentTypes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudDisplay0rder).BeginInit();
            pnlContent.SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            panel1.SuspendLayout();
            tlpAuditInfo.SuspendLayout();
            SuspendLayout();
            // 
            // DataGridView
            // 
            DataGridView.Dock = DockStyle.Fill;
            DataGridView.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            DataGridView.Location = new Point(412, 90);
            DataGridView.ForeColor = Color.FromArgb(16, 24, 40);
            DataGridView.BackColor = Color.Transparent;
            DataGridView.Name = "DataGridView";
            DataGridView.Margin = new Padding(3);
            DataGridView.AutoEllipsis = true;
            DataGridView.Size = new Size(165, 40);
            DataGridView.TabIndex = 18;
            DataGridView.Text = "قائمة انواع الشحن";
            DataGridView.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            label12.Dock = DockStyle.Fill;
            label12.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label12.Location = new Point(412, 130);
            label12.ForeColor = Color.FromArgb(16, 24, 40);
            label12.BackColor = Color.Transparent;
            label12.Name = "label12";
            label12.Margin = new Padding(3);
            label12.AutoEllipsis = true;
            label12.Size = new Size(165, 40);
            label12.TabIndex = 14;
            label12.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDescription
            // 
            lblDescription.Dock = DockStyle.Fill;
            lblDescription.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDescription.Location = new Point(982, 90);
            lblDescription.ForeColor = Color.FromArgb(16, 24, 40);
            lblDescription.BackColor = Color.Transparent;
            lblDescription.Name = "lblDescription";
            lblDescription.Margin = new Padding(3);
            lblDescription.AutoEllipsis = true;
            lblDescription.Size = new Size(165, 40);
            lblDescription.TabIndex = 8;
            lblDescription.Text = "الوصف";
            lblDescription.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textDescription
            // 
            textDescription.Dock = DockStyle.Fill;
            textDescription.Location = new Point(585, 95);
            textDescription.Margin = new Padding(3);
            textDescription.BackColor = Color.White;
            textDescription.ForeColor = Color.FromArgb(16, 24, 40);
            textDescription.Name = "textDescription";
            textDescription.Font = new Font("Segoe UI", 11F);
            textDescription.Size = new Size(389, 30);
            textDescription.TabIndex = 9;
            // 
            // lblShipmentTypeNameEn
            // 
            lblShipmentTypeNameEn.Dock = DockStyle.Fill;
            lblShipmentTypeNameEn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblShipmentTypeNameEn.Location = new Point(412, 50);
            lblShipmentTypeNameEn.ForeColor = Color.FromArgb(16, 24, 40);
            lblShipmentTypeNameEn.BackColor = Color.Transparent;
            lblShipmentTypeNameEn.Name = "lblShipmentTypeNameEn";
            lblShipmentTypeNameEn.Margin = new Padding(3);
            lblShipmentTypeNameEn.AutoEllipsis = true;
            lblShipmentTypeNameEn.Size = new Size(165, 40);
            lblShipmentTypeNameEn.TabIndex = 6;
            lblShipmentTypeNameEn.Text = "اسم نوع الشحن انجليزي";
            lblShipmentTypeNameEn.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtShipmentTypeNameEn
            // 
            txtShipmentTypeNameEn.Dock = DockStyle.Fill;
            txtShipmentTypeNameEn.Location = new Point(15, 55);
            txtShipmentTypeNameEn.Margin = new Padding(3);
            txtShipmentTypeNameEn.BackColor = Color.White;
            txtShipmentTypeNameEn.ForeColor = Color.FromArgb(16, 24, 40);
            txtShipmentTypeNameEn.Name = "txtShipmentTypeNameEn";
            txtShipmentTypeNameEn.Font = new Font("Segoe UI", 11F);
            txtShipmentTypeNameEn.Size = new Size(389, 30);
            txtShipmentTypeNameEn.TabIndex = 7;
            // 
            // lblDisplay0rder
            // 
            lblDisplay0rder.Dock = DockStyle.Fill;
            lblDisplay0rder.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDisplay0rder.Location = new Point(982, 50);
            lblDisplay0rder.ForeColor = Color.FromArgb(16, 24, 40);
            lblDisplay0rder.BackColor = Color.Transparent;
            lblDisplay0rder.Name = "lblDisplay0rder";
            lblDisplay0rder.Margin = new Padding(3);
            lblDisplay0rder.AutoEllipsis = true;
            lblDisplay0rder.Size = new Size(165, 40);
            lblDisplay0rder.TabIndex = 4;
            lblDisplay0rder.Text = "ترتيب العرض";
            lblDisplay0rder.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblShipmentTypeName
            // 
            lblShipmentTypeName.Dock = DockStyle.Fill;
            lblShipmentTypeName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblShipmentTypeName.Location = new Point(412, 10);
            lblShipmentTypeName.ForeColor = Color.FromArgb(16, 24, 40);
            lblShipmentTypeName.BackColor = Color.Transparent;
            lblShipmentTypeName.Name = "lblShipmentTypeName";
            lblShipmentTypeName.Margin = new Padding(3);
            lblShipmentTypeName.AutoEllipsis = true;
            lblShipmentTypeName.Size = new Size(165, 40);
            lblShipmentTypeName.TabIndex = 2;
            lblShipmentTypeName.Text = "اسم نوع الشحن";
            lblShipmentTypeName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtShipmentTypeName
            // 
            txtShipmentTypeName.Dock = DockStyle.Fill;
            txtShipmentTypeName.Location = new Point(15, 15);
            txtShipmentTypeName.Margin = new Padding(3);
            txtShipmentTypeName.BackColor = Color.White;
            txtShipmentTypeName.ForeColor = Color.FromArgb(16, 24, 40);
            txtShipmentTypeName.Name = "txtShipmentTypeName";
            txtShipmentTypeName.Font = new Font("Segoe UI", 11F);
            txtShipmentTypeName.Size = new Size(389, 30);
            txtShipmentTypeName.TabIndex = 3;
            // 
            // lblShipmentTypeCode
            // 
            lblShipmentTypeCode.Dock = DockStyle.Fill;
            lblShipmentTypeCode.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblShipmentTypeCode.Location = new Point(982, 10);
            lblShipmentTypeCode.ForeColor = Color.FromArgb(16, 24, 40);
            lblShipmentTypeCode.BackColor = Color.Transparent;
            lblShipmentTypeCode.Name = "lblShipmentTypeCode";
            lblShipmentTypeCode.Margin = new Padding(3);
            lblShipmentTypeCode.AutoEllipsis = true;
            lblShipmentTypeCode.Size = new Size(165, 40);
            lblShipmentTypeCode.TabIndex = 0;
            lblShipmentTypeCode.Text = "كود الشحن";
            lblShipmentTypeCode.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtShipmentTypeCode
            // 
            txtShipmentTypeCode.Dock = DockStyle.Fill;
            txtShipmentTypeCode.Location = new Point(804, 15);
            txtShipmentTypeCode.Margin = new Padding(3);
            txtShipmentTypeCode.BackColor = Color.White;
            txtShipmentTypeCode.ForeColor = Color.FromArgb(16, 24, 40);
            txtShipmentTypeCode.Name = "txtShipmentTypeCode";
            txtShipmentTypeCode.Font = new Font("Segoe UI", 11F);
            txtShipmentTypeCode.Size = new Size(170, 30);
            txtShipmentTypeCode.TabIndex = 1;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(13, 93);
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.GridColor = Color.FromArgb(224, 224, 224);
            dataGridView1.Font = new Font("Segoe UI", 10F);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(393, 34);
            dataGridView1.TabIndex = 17;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Location = new Point(1081, 133);
            chkIsActive.BackColor = Color.Transparent;
            chkIsActive.ForeColor = Color.FromArgb(16, 24, 40);
            chkIsActive.Dock = DockStyle.Fill;
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Font = new Font("Segoe UI", 10F);
            chkIsActive.Margin = new Padding(3);
            chkIsActive.Size = new Size(66, 27);
            chkIsActive.TabIndex = 1;
            chkIsActive.Text = "نشط";
            chkIsActive.UseVisualStyleBackColor = false;
            // 
            // dgvShipmentTypes
            // 
            dgvShipmentTypes.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            dgvShipmentTypes.ColumnCount = 4;
            dgvShipmentTypes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            dgvShipmentTypes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            dgvShipmentTypes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            dgvShipmentTypes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            dgvShipmentTypes.Controls.Add(DataGridView, 2, 2);
            dgvShipmentTypes.Controls.Add(label12, 2, 3);
            dgvShipmentTypes.Controls.Add(lblDescription, 0, 2);
            dgvShipmentTypes.Controls.Add(textDescription, 1, 2);
            dgvShipmentTypes.Controls.Add(lblShipmentTypeNameEn, 2, 1);
            dgvShipmentTypes.Controls.Add(txtShipmentTypeNameEn, 3, 1);
            dgvShipmentTypes.Controls.Add(lblDisplay0rder, 0, 1);
            dgvShipmentTypes.Controls.Add(lblShipmentTypeName, 2, 0);
            dgvShipmentTypes.Controls.Add(txtShipmentTypeName, 3, 0);
            dgvShipmentTypes.Controls.Add(lblShipmentTypeCode, 0, 0);
            dgvShipmentTypes.Controls.Add(txtShipmentTypeCode, 1, 0);
            dgvShipmentTypes.Controls.Add(dataGridView1, 3, 2);
            dgvShipmentTypes.Controls.Add(nudDisplay0rder, 1, 1);
            dgvShipmentTypes.Controls.Add(chkIsActive, 0, 3);
            dgvShipmentTypes.Dock = DockStyle.Top;
            dgvShipmentTypes.Location = new Point(20, 20);
            dgvShipmentTypes.Margin = new Padding(0);
            dgvShipmentTypes.BackColor = Color.LightCyan;
            dgvShipmentTypes.Name = "dgvShipmentTypes";
            dgvShipmentTypes.MinimumSize = new Size(0, 180);
            dgvShipmentTypes.Padding = new Padding(0);
            dgvShipmentTypes.RowCount = 4;
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            dgvShipmentTypes.Size = new Size(1160, 180);
            dgvShipmentTypes.TabIndex = 0;
            // 
            // nudDisplay0rder
            // 
            nudDisplay0rder.Location = new Point(585, 53);
            nudDisplay0rder.BackColor = Color.White;
            nudDisplay0rder.ForeColor = Color.FromArgb(16, 24, 40);
            nudDisplay0rder.Name = "nudDisplay0rder";
            nudDisplay0rder.Font = new Font("Segoe UI", 11F);
            nudDisplay0rder.Margin = new Padding(3);
            nudDisplay0rder.Dock = DockStyle.Fill;
            nudDisplay0rder.Size = new Size(391, 30);
            nudDisplay0rder.TabIndex = 19;
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
            // pnlContent
            // 
            pnlContent.Controls.Add(dgvShipmentTypes);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.AutoScrollMinSize = new Size(0, 1008);
            pnlContent.Location = new Point(0, 53);
            pnlContent.BackColor = Color.LightCyan;
            pnlContent.Name = "pnlContent";
            pnlContent.AutoScroll = true;
            pnlContent.Padding = new Padding(20);
            pnlContent.Size = new Size(1200, 1008);
            pnlContent.TabIndex = 5;
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(designerCommandBar);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.BackColor = Color.FromArgb(224, 224, 224);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.MinimumSize = new Size(0, 48);
            pnlHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlHeader.AutoSize = true;
            pnlHeader.Size = new Size(1200, 53);
            pnlHeader.TabIndex = 3;
            // 
            // pnlToolbar
            // 

            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(230, 0);
            pnlToolbar.Margin = new Padding(5);
            pnlToolbar.BackColor = Color.FromArgb(224, 224, 224);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.MinimumSize = new Size(0, 48);
            pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlToolbar.AutoSize = true;
            pnlToolbar.Padding = new Padding(5);
            pnlToolbar.Size = new Size(970, 53);
            pnlToolbar.TabIndex = 1;
            pnlToolbar.WrapContents = true;
            // 
            // btnNew
            // 
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Microsoft Sans Serif", 10F);
            btnNew.Location = new Point(866, 9);
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
            btnSave.Location = new Point(768, 9);
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
            btnEdit.Location = new Point(670, 9);
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
            btnDelete.Location = new Point(572, 9);
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
            btnRefresh.Location = new Point(474, 9);
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
            btnClose.Location = new Point(376, 9);
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
            lblTitle.Padding = new Padding(0, 0, 15, 0);
            lblTitle.RightToLeft = RightToLeft.Yes;
            lblTitle.Size = new Size(230, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "قائمة أنواع الشحن";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.Controls.Add(tlpAuditInfo);
            panel1.Visible = false;
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 1067);
            panel1.BackColor = Color.LightCyan;
            panel1.Name = "panel1";
            panel1.Size = new Size(1200, 48);
            panel1.TabIndex = 4;
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
            tlpAuditInfo.Dock = DockStyle.Fill;
            tlpAuditInfo.Location = new Point(0, 0);
            tlpAuditInfo.Margin = new Padding(0);
            tlpAuditInfo.BackColor = Color.FromArgb(248, 250, 252);
            tlpAuditInfo.Name = "tlpAuditInfo";
            tlpAuditInfo.Padding = new Padding(8);
            tlpAuditInfo.RowCount = 1;
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAuditInfo.Size = new Size(1200, 48);
            tlpAuditInfo.TabIndex = 0;
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
            // UcShipmentTypes
            // 
            AccessibleName = "أنواع الشحن";
            AutoScaleDimensions = new SizeF(120F, 120F);
            BackColor = Color.FromArgb(248, 250, 252);
            ForeColor = Color.FromArgb(16, 24, 40);
            Padding = new Padding(0);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScroll = true;
            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);
            Controls.Add(panel1);
            Font = new Font("Segoe UI", 10F);
            Margin = new Padding(0);
            MinimumSize = new Size(650, 1100);
            Name = "UcShipmentTypes";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1200, 1100);
            Load += UcShipmentTypes_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            dgvShipmentTypes.ResumeLayout(false);
            dgvShipmentTypes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudDisplay0rder).EndInit();
            pnlContent.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tlpAuditInfo.ResumeLayout(false);
            ResumeLayout(false);
        
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(230, 0);
        designerCommandBar.Size = new Size(970, 53);
        designerCommandBar.Margin = new Padding(5);
        designerCommandBar.MinimumSize = new Size(0, 48);
        designerCommandBar.TabIndex = 1;
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

        private Label DataGridView;
        private Label label12;
        private Label lblDescription;
        private TextBox textDescription;
        private Label lblShipmentTypeNameEn;
        private TextBox txtShipmentTypeNameEn;
        private Label lblDisplay0rder;
        private Label lblShipmentTypeName;
        private TextBox txtShipmentTypeName;
        private Label lblShipmentTypeCode;
        private TextBox txtShipmentTypeCode;
        private DataGridView dataGridView1;
        private CheckBox chkIsActive;
        private TableLayoutPanel dgvShipmentTypes;
        private NumericUpDown nudDisplay0rder;
        private Label lblPrintCount;
        private Label lblLastPrintedAt;
        private Label lblEditCount;
        private Panel pnlContent;
        private Panel pnlHeader;
        private FlowLayoutPanel pnlToolbar;
        private Button btnNew;
        private Button btnSave;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private Button btnClose;
        private Label lblTitle;
        private Panel panel1;
        private TableLayoutPanel tlpAuditInfo;
        private Label lblModifiedAt;
        private Label lblModifiedBy;
        private Label lblCreatedAt;
        private Label lblCreatedBy;
    }

}