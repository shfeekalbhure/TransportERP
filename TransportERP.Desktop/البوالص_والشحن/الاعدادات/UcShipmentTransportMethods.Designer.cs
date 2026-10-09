namespace TransportERP.Desktop.البوالص_والشحن.الاعدادات
{
    partial class UcShipmentTransportMethods
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
            panel1 = new Panel();
            tabMain = new TabControl();
            tabTransportMethods = new TabPage();
            dgvShipmentTypes = new TableLayoutPanel();
            checkBox2 = new CheckBox();
            txtDescription = new TextBox();
            label3 = new Label();
            nudDisplayOrder = new NumericUpDown();
            label1 = new Label();
            txtTransportMethodNameEn = new TextBox();
            cmbTransportGroup = new ComboBox();
            cmbTransportType = new ComboBox();
            label11 = new Label();
            lblDescription = new Label();
            lblPriorityNameEn = new Label();
            txtTransportMethodNameAr = new TextBox();
            lblPriorityNameAr = new Label();
            lblPriorityCode = new Label();
            txtTransportMethodCode = new TextBox();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            tlpAuditInfo.SuspendLayout();
            panel1.SuspendLayout();
            tabMain.SuspendLayout();
            tabTransportMethods.SuspendLayout();
            dgvShipmentTypes.SuspendLayout();
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
            pnlHeader.Size = new Size(1379, 53);
            pnlHeader.TabIndex = 5;
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
            pnlToolbar.Location = new Point(167, 0);
            pnlToolbar.Margin = new Padding(5);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.MinimumSize = new Size(0, 48);
            pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlToolbar.AutoSize = true;
            pnlToolbar.Padding = new Padding(5);
            pnlToolbar.Size = new Size(1212, 53);
            pnlToolbar.TabIndex = 2;
            pnlToolbar.WrapContents = true;
            // 
            // btnNew
            // 
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Microsoft Sans Serif", 10F);
            btnNew.Location = new Point(1108, 9);
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
            btnSave.Location = new Point(1010, 9);
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
            btnEdit.Location = new Point(912, 9);
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
            btnDelete.Location = new Point(814, 9);
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
            btnRefresh.Location = new Point(716, 9);
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
            btnClose.Location = new Point(618, 9);
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
            btnFirst.Location = new Point(520, 9);
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
            btnPrevious.Location = new Point(422, 9);
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
            txtCurrentRecordNo.Location = new Point(351, 10);
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
            btnNext.Location = new Point(252, 9);
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
            btnLast.Location = new Point(154, 9);
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
            btnUndo.Location = new Point(56, 9);
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
            lblTitle.Size = new Size(167, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "وسائل النقل";
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
            tlpAuditInfo.Size = new Size(1379, 48);
            tlpAuditInfo.TabIndex = 6;
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
            lblPrintCount.Size = new Size(162, 33);
            lblPrintCount.TabIndex = 8;
            lblPrintCount.Text = "عدد مرات الطباعة :- 00";
            lblPrintCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblLastPrintedAt
            // 
            lblLastPrintedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblLastPrintedAt.Dock = DockStyle.Fill;
            lblLastPrintedAt.Font = new Font("Segoe UI", 10F);
            lblLastPrintedAt.Location = new Point(171, 0);
            lblLastPrintedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblLastPrintedAt.Name = "lblLastPrintedAt";
            lblLastPrintedAt.Margin = new Padding(3);
            lblLastPrintedAt.AutoEllipsis = true;
            lblLastPrintedAt.RightToLeft = RightToLeft.No;
            lblLastPrintedAt.Size = new Size(214, 33);
            lblLastPrintedAt.TabIndex = 5;
            lblLastPrintedAt.Text = "تاريخ اخر طباعة:  27/09/2026";
            lblLastPrintedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblEditCount
            // 
            lblEditCount.BackColor = Color.FromArgb(192, 255, 255);
            lblEditCount.Dock = DockStyle.Fill;
            lblEditCount.Font = new Font("Segoe UI", 10F);
            lblEditCount.Location = new Point(391, 0);
            lblEditCount.ForeColor = Color.FromArgb(16, 24, 40);
            lblEditCount.Name = "lblEditCount";
            lblEditCount.Margin = new Padding(3);
            lblEditCount.AutoEllipsis = true;
            lblEditCount.RightToLeft = RightToLeft.No;
            lblEditCount.Size = new Size(159, 33);
            lblEditCount.TabIndex = 4;
            lblEditCount.Text = "عدد التعديلات :- 00";
            lblEditCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedAt
            // 
            lblModifiedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedAt.Dock = DockStyle.Fill;
            lblModifiedAt.Font = new Font("Segoe UI", 10F);
            lblModifiedAt.Location = new Point(556, 0);
            lblModifiedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblModifiedAt.Name = "lblModifiedAt";
            lblModifiedAt.Margin = new Padding(3);
            lblModifiedAt.AutoEllipsis = true;
            lblModifiedAt.RightToLeft = RightToLeft.No;
            lblModifiedAt.Size = new Size(214, 33);
            lblModifiedAt.TabIndex = 3;
            lblModifiedAt.Text = " تاريخ التعديل:  27/09/2026";
            lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedBy
            // 
            lblModifiedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedBy.Dock = DockStyle.Fill;
            lblModifiedBy.Font = new Font("Segoe UI", 10F);
            lblModifiedBy.Location = new Point(776, 0);
            lblModifiedBy.ForeColor = Color.FromArgb(16, 24, 40);
            lblModifiedBy.Name = "lblModifiedBy";
            lblModifiedBy.Margin = new Padding(3);
            lblModifiedBy.AutoEllipsis = true;
            lblModifiedBy.RightToLeft = RightToLeft.No;
            lblModifiedBy.Size = new Size(187, 33);
            lblModifiedBy.TabIndex = 2;
            lblModifiedBy.Text = "عدل بواسطة: - مدير النظام";
            lblModifiedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedAt
            // 
            lblCreatedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedAt.Dock = DockStyle.Fill;
            lblCreatedAt.Font = new Font("Segoe UI", 10F);
            lblCreatedAt.Location = new Point(969, 0);
            lblCreatedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblCreatedAt.Name = "lblCreatedAt";
            lblCreatedAt.Margin = new Padding(3);
            lblCreatedAt.AutoEllipsis = true;
            lblCreatedAt.RightToLeft = RightToLeft.No;
            lblCreatedAt.Size = new Size(214, 33);
            lblCreatedAt.TabIndex = 1;
            lblCreatedAt.Text = " تاريخ الانشاء:  27/09/2026";
            lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedBy
            // 
            lblCreatedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedBy.Dock = DockStyle.Fill;
            lblCreatedBy.Font = new Font("Segoe UI", 10F);
            lblCreatedBy.Location = new Point(1189, 0);
            lblCreatedBy.ForeColor = Color.FromArgb(16, 24, 40);
            lblCreatedBy.Name = "lblCreatedBy";
            lblCreatedBy.Margin = new Padding(3);
            lblCreatedBy.AutoEllipsis = true;
            lblCreatedBy.RightToLeft = RightToLeft.No;
            lblCreatedBy.Size = new Size(187, 33);
            lblCreatedBy.TabIndex = 0;
            lblCreatedBy.Text = " أنشأ بواسطة: - مدير النظام";
            lblCreatedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            panel1.BackColor = Color.LightCyan;
            panel1.Controls.Add(tabMain);
            panel1.Dock = DockStyle.Fill;
            panel1.AutoScroll = true;
            panel1.AutoScrollMinSize = new Size(0, 703);
            panel1.Location = new Point(0, 53);
            panel1.Name = "panel1";
            panel1.Size = new Size(1379, 703);
            panel1.TabIndex = 7;
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tabTransportMethods);
            tabMain.Dock = DockStyle.Top;
            tabMain.Location = new Point(0, 0);
            tabMain.Margin = new Padding(0);
            tabMain.Name = "tabMain";
            tabMain.Font = new Font("Segoe UI", 9F);
            tabMain.RightToLeftLayout = true;
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1379, 317);
            tabMain.TabIndex = 0;
            // 
            // tabTransportMethods
            // 
            tabTransportMethods.Controls.Add(dgvShipmentTypes);
            tabTransportMethods.Location = new Point(4, 29);
            tabTransportMethods.BackColor = Color.LightCyan;
            tabTransportMethods.Padding = new Padding(3);
            tabTransportMethods.Margin = new Padding(0);
            tabTransportMethods.Name = "tabTransportMethods";
            tabTransportMethods.AutoScroll = true;
            tabTransportMethods.Size = new Size(1371, 284);
            tabTransportMethods.TabIndex = 2;
            tabTransportMethods.Text = "وسائل النقل";
            tabTransportMethods.UseVisualStyleBackColor = false;
            // 
            // dgvShipmentTypes
            // 
            dgvShipmentTypes.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            dgvShipmentTypes.BackColor = Color.LightCyan;
            dgvShipmentTypes.ColumnCount = 4;
            dgvShipmentTypes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            dgvShipmentTypes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            dgvShipmentTypes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            dgvShipmentTypes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            dgvShipmentTypes.Controls.Add(checkBox2, 2, 3);
            dgvShipmentTypes.Controls.Add(txtDescription, 1, 3);
            dgvShipmentTypes.Controls.Add(label3, 0, 3);
            dgvShipmentTypes.Controls.Add(nudDisplayOrder, 3, 2);
            dgvShipmentTypes.Controls.Add(label1, 2, 2);
            dgvShipmentTypes.Controls.Add(txtTransportMethodNameEn, 1, 2);
            dgvShipmentTypes.Controls.Add(cmbTransportGroup, 1, 1);
            dgvShipmentTypes.Controls.Add(cmbTransportType, 3, 0);
            dgvShipmentTypes.Controls.Add(label11, 0, 1);
            dgvShipmentTypes.Controls.Add(lblDescription, 0, 2);
            dgvShipmentTypes.Controls.Add(lblPriorityNameEn, 2, 1);
            dgvShipmentTypes.Controls.Add(txtTransportMethodNameAr, 3, 1);
            dgvShipmentTypes.Controls.Add(lblPriorityNameAr, 2, 0);
            dgvShipmentTypes.Controls.Add(lblPriorityCode, 0, 0);
            dgvShipmentTypes.Controls.Add(txtTransportMethodCode, 1, 0);
            dgvShipmentTypes.Dock = DockStyle.Top;
            dgvShipmentTypes.Location = new Point(0, 0);
            dgvShipmentTypes.Margin = new Padding(0);
            dgvShipmentTypes.Name = "dgvShipmentTypes";
            dgvShipmentTypes.Padding = new Padding(0);
            dgvShipmentTypes.RowCount = 6;
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            dgvShipmentTypes.Size = new Size(1371, 237);
            dgvShipmentTypes.TabIndex = 10;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Font = new Font("Segoe UI", 10F);
            checkBox2.Location = new Point(614, 121);
            checkBox2.BackColor = Color.Transparent;
            checkBox2.ForeColor = Color.FromArgb(16, 24, 40);
            checkBox2.Dock = DockStyle.Fill;
            checkBox2.Name = "checkBox2";
            checkBox2.Margin = new Padding(3);
            checkBox2.Size = new Size(70, 29);
            checkBox2.TabIndex = 71;
            checkBox2.Text = "نشط";
            checkBox2.UseVisualStyleBackColor = false;
            // 
            // txtDescription
            // 
            txtDescription.Dock = DockStyle.Fill;
            txtDescription.Location = new Point(692, 123);
            txtDescription.Margin = new Padding(3);
            txtDescription.BackColor = Color.White;
            txtDescription.ForeColor = Color.FromArgb(16, 24, 40);
            txtDescription.Name = "txtDescription";
            txtDescription.Font = new Font("Segoe UI", 11F);
            txtDescription.Size = new Size(462, 27);
            txtDescription.TabIndex = 70;
            // 
            // label3
            // 
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label3.Location = new Point(1162, 118);
            label3.ForeColor = Color.FromArgb(16, 24, 40);
            label3.BackColor = Color.Transparent;
            label3.Name = "label3";
            label3.Margin = new Padding(3);
            label3.AutoEllipsis = true;
            label3.Size = new Size(196, 36);
            label3.TabIndex = 69;
            label3.Text = "الوصف";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudDisplayOrder
            // 
            nudDisplayOrder.Dock = DockStyle.Fill;
            nudDisplayOrder.Location = new Point(13, 85);
            nudDisplayOrder.BackColor = Color.White;
            nudDisplayOrder.ForeColor = Color.FromArgb(16, 24, 40);
            nudDisplayOrder.Name = "nudDisplayOrder";
            nudDisplayOrder.Font = new Font("Segoe UI", 11F);
            nudDisplayOrder.Margin = new Padding(3);
            nudDisplayOrder.Size = new Size(469, 27);
            nudDisplayOrder.TabIndex = 68;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.Location = new Point(488, 82);
            label1.ForeColor = Color.FromArgb(16, 24, 40);
            label1.BackColor = Color.Transparent;
            label1.Name = "label1";
            label1.Margin = new Padding(3);
            label1.AutoEllipsis = true;
            label1.Size = new Size(196, 36);
            label1.TabIndex = 67;
            label1.Text = "ترتيب العرض";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtTransportMethodNameEn
            // 
            txtTransportMethodNameEn.Dock = DockStyle.Fill;
            txtTransportMethodNameEn.Location = new Point(692, 87);
            txtTransportMethodNameEn.Margin = new Padding(3);
            txtTransportMethodNameEn.BackColor = Color.White;
            txtTransportMethodNameEn.ForeColor = Color.FromArgb(16, 24, 40);
            txtTransportMethodNameEn.Name = "txtTransportMethodNameEn";
            txtTransportMethodNameEn.Font = new Font("Segoe UI", 11F);
            txtTransportMethodNameEn.Size = new Size(462, 27);
            txtTransportMethodNameEn.TabIndex = 66;
            // 
            // cmbTransportGroup
            // 
            cmbTransportGroup.Dock = DockStyle.Fill;
            cmbTransportGroup.FormattingEnabled = true;
            cmbTransportGroup.Location = new Point(690, 49);
            cmbTransportGroup.BackColor = Color.White;
            cmbTransportGroup.ForeColor = Color.FromArgb(16, 24, 40);
            cmbTransportGroup.Name = "cmbTransportGroup";
            cmbTransportGroup.Font = new Font("Segoe UI", 11F);
            cmbTransportGroup.Margin = new Padding(3);
            cmbTransportGroup.Size = new Size(466, 28);
            cmbTransportGroup.TabIndex = 62;
            // 
            // cmbTransportType
            // 
            cmbTransportType.Dock = DockStyle.Fill;
            cmbTransportType.FormattingEnabled = true;
            cmbTransportType.Location = new Point(13, 13);
            cmbTransportType.BackColor = Color.White;
            cmbTransportType.ForeColor = Color.FromArgb(16, 24, 40);
            cmbTransportType.Name = "cmbTransportType";
            cmbTransportType.Font = new Font("Segoe UI", 11F);
            cmbTransportType.Margin = new Padding(3);
            cmbTransportType.Size = new Size(469, 28);
            cmbTransportType.TabIndex = 61;
            // 
            // label11
            // 
            label11.Dock = DockStyle.Fill;
            label11.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label11.Location = new Point(1162, 46);
            label11.ForeColor = Color.FromArgb(16, 24, 40);
            label11.BackColor = Color.Transparent;
            label11.Name = "label11";
            label11.Margin = new Padding(3);
            label11.AutoEllipsis = true;
            label11.Size = new Size(196, 36);
            label11.TabIndex = 59;
            label11.Text = "مجموعة وسائل النقل";
            label11.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDescription
            // 
            lblDescription.Dock = DockStyle.Fill;
            lblDescription.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDescription.Location = new Point(1162, 82);
            lblDescription.ForeColor = Color.FromArgb(16, 24, 40);
            lblDescription.BackColor = Color.Transparent;
            lblDescription.Name = "lblDescription";
            lblDescription.Margin = new Padding(3);
            lblDescription.AutoEllipsis = true;
            lblDescription.Size = new Size(196, 36);
            lblDescription.TabIndex = 53;
            lblDescription.Text = "اسم وسيلة النقل انجليزي";
            lblDescription.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPriorityNameEn
            // 
            lblPriorityNameEn.Dock = DockStyle.Fill;
            lblPriorityNameEn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPriorityNameEn.Location = new Point(488, 46);
            lblPriorityNameEn.ForeColor = Color.FromArgb(16, 24, 40);
            lblPriorityNameEn.BackColor = Color.Transparent;
            lblPriorityNameEn.Name = "lblPriorityNameEn";
            lblPriorityNameEn.Margin = new Padding(3);
            lblPriorityNameEn.AutoEllipsis = true;
            lblPriorityNameEn.Size = new Size(196, 36);
            lblPriorityNameEn.TabIndex = 6;
            lblPriorityNameEn.Text = "اسم وسيلة النقل عربي";
            lblPriorityNameEn.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtTransportMethodNameAr
            // 
            txtTransportMethodNameAr.Dock = DockStyle.Fill;
            txtTransportMethodNameAr.Location = new Point(15, 51);
            txtTransportMethodNameAr.Margin = new Padding(3);
            txtTransportMethodNameAr.BackColor = Color.White;
            txtTransportMethodNameAr.ForeColor = Color.FromArgb(16, 24, 40);
            txtTransportMethodNameAr.Name = "txtTransportMethodNameAr";
            txtTransportMethodNameAr.Font = new Font("Segoe UI", 11F);
            txtTransportMethodNameAr.Size = new Size(465, 27);
            txtTransportMethodNameAr.TabIndex = 7;
            // 
            // lblPriorityNameAr
            // 
            lblPriorityNameAr.Dock = DockStyle.Fill;
            lblPriorityNameAr.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPriorityNameAr.Location = new Point(488, 10);
            lblPriorityNameAr.ForeColor = Color.FromArgb(16, 24, 40);
            lblPriorityNameAr.BackColor = Color.Transparent;
            lblPriorityNameAr.Name = "lblPriorityNameAr";
            lblPriorityNameAr.Margin = new Padding(3);
            lblPriorityNameAr.AutoEllipsis = true;
            lblPriorityNameAr.Size = new Size(196, 36);
            lblPriorityNameAr.TabIndex = 2;
            lblPriorityNameAr.Text = "نوع وسيلة النقل";
            lblPriorityNameAr.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPriorityCode
            // 
            lblPriorityCode.Dock = DockStyle.Fill;
            lblPriorityCode.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPriorityCode.Location = new Point(1162, 10);
            lblPriorityCode.ForeColor = Color.FromArgb(16, 24, 40);
            lblPriorityCode.BackColor = Color.Transparent;
            lblPriorityCode.Name = "lblPriorityCode";
            lblPriorityCode.Margin = new Padding(3);
            lblPriorityCode.AutoEllipsis = true;
            lblPriorityCode.Size = new Size(196, 36);
            lblPriorityCode.TabIndex = 0;
            lblPriorityCode.Text = "كود وسيلة النقل";
            lblPriorityCode.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtTransportMethodCode
            // 
            txtTransportMethodCode.Dock = DockStyle.Fill;
            txtTransportMethodCode.Location = new Point(874, 15);
            txtTransportMethodCode.Margin = new Padding(3);
            txtTransportMethodCode.BackColor = Color.White;
            txtTransportMethodCode.ForeColor = Color.FromArgb(16, 24, 40);
            txtTransportMethodCode.Name = "txtTransportMethodCode";
            txtTransportMethodCode.Font = new Font("Segoe UI", 11F);
            txtTransportMethodCode.Size = new Size(280, 27);
            txtTransportMethodCode.TabIndex = 1;
            // 
            // UcShipmentTransportMethods
            // 
            AccessibleDescription = "وسائل النقل";
            AutoScaleDimensions = new SizeF(120F, 120F);
            BackColor = Color.FromArgb(248, 250, 252);
            ForeColor = Color.FromArgb(16, 24, 40);
            Margin = new Padding(0);
            Padding = new Padding(0);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(panel1);
            Controls.Add(tlpAuditInfo);
            Controls.Add(pnlHeader);
            Name = "UcShipmentTransportMethods";
            Font = new Font("Segoe UI", 10F);
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1379, 800);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            tlpAuditInfo.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            tabTransportMethods.ResumeLayout(false);
            dgvShipmentTypes.ResumeLayout(false);
            dgvShipmentTypes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudDisplayOrder).EndInit();
            ResumeLayout(false);
        
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(167, 0);
        designerCommandBar.Size = new Size(1212, 53);
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
        private Panel panel1;
        private TabControl tabMain;
        private TabPage tabTransportMethods;
        private TableLayoutPanel dgvShipmentTypes;
        private Label lblDescription;
        private Label lblPriorityNameEn;
        private TextBox txtTransportMethodNameAr;
        private Label lblPriorityNameAr;
        private Label lblPriorityCode;
        private TextBox txtTransportMethodCode;
        private Label label11;
        private TextBox txtTransportMethodNameEn;
        private ComboBox cmbTransportGroup;
        private ComboBox cmbTransportType;
        private CheckBox checkBox2;
        private TextBox txtDescription;
        private Label label3;
        private NumericUpDown nudDisplayOrder;
        private Label label1;
    }
}
