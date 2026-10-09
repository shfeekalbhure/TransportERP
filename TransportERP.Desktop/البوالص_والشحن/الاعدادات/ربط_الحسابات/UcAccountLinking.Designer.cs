namespace TransportERP.Desktop.البوالص_والشحن.الاعدادات.ربط_الحسابات
{
    partial class UcAccountLinking
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
            tableLayoutPanel2 = new TableLayoutPanel();
            label3 = new Label();
            cboDriverCollectionAccount = new ComboBox();
            cboCustomerReceivableAccount = new ComboBox();
            cboWaybillsUnderCollectionAccount = new ComboBox();
            label1 = new Label();
            cboShipmentType = new ComboBox();
            cboBankAccount = new ComboBox();
            label65 = new Label();
            cboBank = new ComboBox();
            label66 = new Label();
            cboCashAccount = new ComboBox();
            label77 = new Label();
            cboCashbox = new ComboBox();
            label78 = new Label();
            cboCurrency = new ComboBox();
            label2 = new Label();
            cboBranch = new ComboBox();
            cboCompany = new ComboBox();
            label88 = new Label();
            label87 = new Label();
            cboDeferredRevenueAccount = new ComboBox();
            label86 = new Label();
            cboTransportRevenueAccount = new ComboBox();
            label85 = new Label();
            label81 = new Label();
            cboLinkType = new ComboBox();
            label82 = new Label();
            label83 = new Label();
            label84 = new Label();
            cboPaymentMethod = new ComboBox();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            tlpAuditInfo.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
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
            pnlHeader.Size = new Size(1492, 53);
            pnlHeader.TabIndex = 12;
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
            pnlToolbar.Location = new Point(130, 0);
            pnlToolbar.Margin = new Padding(5);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.MinimumSize = new Size(0, 48);
            pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlToolbar.AutoSize = true;
            pnlToolbar.Padding = new Padding(5);
            pnlToolbar.RightToLeft = RightToLeft.Yes;
            pnlToolbar.Size = new Size(1362, 53);
            pnlToolbar.TabIndex = 2;
            pnlToolbar.WrapContents = true;
            // 
            // btnNew
            // 
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Microsoft Sans Serif", 10F);
            btnNew.Location = new Point(1258, 9);
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
            btnSave.Location = new Point(1160, 9);
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
            btnEdit.Location = new Point(1062, 9);
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
            btnDelete.Location = new Point(964, 9);
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
            btnRefresh.Location = new Point(866, 9);
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
            btnClose.Location = new Point(768, 9);
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
            btnFirst.Location = new Point(670, 9);
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
            btnPrevious.Location = new Point(572, 9);
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
            txtCurrentRecordNo.Location = new Point(501, 10);
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
            btnNext.Location = new Point(402, 9);
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
            btnLast.Location = new Point(304, 9);
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
            btnUndo.Location = new Point(206, 9);
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
            lblTitle.Size = new Size(130, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "ربط حسابات البوالص";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tlpAuditInfo
            // 
            tlpAuditInfo.AccessibleName = "البوليصه";
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
            tlpAuditInfo.Location = new Point(0, 644);
            tlpAuditInfo.Margin = new Padding(0);
            tlpAuditInfo.BackColor = Color.FromArgb(248, 250, 252);
            tlpAuditInfo.Name = "tlpAuditInfo";
            tlpAuditInfo.Padding = new Padding(8);
            tlpAuditInfo.RightToLeft = RightToLeft.Yes;
            tlpAuditInfo.RowCount = 1;
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAuditInfo.Size = new Size(1492, 48);
            tlpAuditInfo.TabIndex = 13;
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
            lblPrintCount.Size = new Size(177, 33);
            lblPrintCount.TabIndex = 8;
            lblPrintCount.Text = "عدد مرات الطباعة :- 00";
            lblPrintCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblLastPrintedAt
            // 
            lblLastPrintedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblLastPrintedAt.Dock = DockStyle.Fill;
            lblLastPrintedAt.Font = new Font("Segoe UI", 10F);
            lblLastPrintedAt.Location = new Point(186, 0);
            lblLastPrintedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblLastPrintedAt.Name = "lblLastPrintedAt";
            lblLastPrintedAt.Margin = new Padding(3);
            lblLastPrintedAt.AutoEllipsis = true;
            lblLastPrintedAt.RightToLeft = RightToLeft.No;
            lblLastPrintedAt.Size = new Size(232, 33);
            lblLastPrintedAt.TabIndex = 5;
            lblLastPrintedAt.Text = "تاريخ اخر طباعة:  27/09/2026";
            lblLastPrintedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblEditCount
            // 
            lblEditCount.BackColor = Color.FromArgb(192, 255, 255);
            lblEditCount.Dock = DockStyle.Fill;
            lblEditCount.Font = new Font("Segoe UI", 10F);
            lblEditCount.Location = new Point(424, 0);
            lblEditCount.ForeColor = Color.FromArgb(16, 24, 40);
            lblEditCount.Name = "lblEditCount";
            lblEditCount.Margin = new Padding(3);
            lblEditCount.AutoEllipsis = true;
            lblEditCount.RightToLeft = RightToLeft.No;
            lblEditCount.Size = new Size(173, 33);
            lblEditCount.TabIndex = 4;
            lblEditCount.Text = "عدد التعديلات :- 00";
            lblEditCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedAt
            // 
            lblModifiedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedAt.Dock = DockStyle.Fill;
            lblModifiedAt.Font = new Font("Segoe UI", 10F);
            lblModifiedAt.Location = new Point(603, 0);
            lblModifiedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblModifiedAt.Name = "lblModifiedAt";
            lblModifiedAt.Margin = new Padding(3);
            lblModifiedAt.AutoEllipsis = true;
            lblModifiedAt.RightToLeft = RightToLeft.No;
            lblModifiedAt.Size = new Size(232, 33);
            lblModifiedAt.TabIndex = 3;
            lblModifiedAt.Text = " تاريخ التعديل:  27/09/2026";
            lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedBy
            // 
            lblModifiedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedBy.Dock = DockStyle.Fill;
            lblModifiedBy.Font = new Font("Segoe UI", 10F);
            lblModifiedBy.Location = new Point(841, 0);
            lblModifiedBy.ForeColor = Color.FromArgb(16, 24, 40);
            lblModifiedBy.Name = "lblModifiedBy";
            lblModifiedBy.Margin = new Padding(3);
            lblModifiedBy.AutoEllipsis = true;
            lblModifiedBy.RightToLeft = RightToLeft.No;
            lblModifiedBy.Size = new Size(202, 33);
            lblModifiedBy.TabIndex = 2;
            lblModifiedBy.Text = "عدل بواسطة: - مدير النظام";
            lblModifiedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedAt
            // 
            lblCreatedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedAt.Dock = DockStyle.Fill;
            lblCreatedAt.Font = new Font("Segoe UI", 10F);
            lblCreatedAt.Location = new Point(1049, 0);
            lblCreatedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblCreatedAt.Name = "lblCreatedAt";
            lblCreatedAt.Margin = new Padding(3);
            lblCreatedAt.AutoEllipsis = true;
            lblCreatedAt.RightToLeft = RightToLeft.No;
            lblCreatedAt.Size = new Size(232, 33);
            lblCreatedAt.TabIndex = 1;
            lblCreatedAt.Text = " تاريخ الانشاء:  27/09/2026";
            lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedBy
            // 
            lblCreatedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedBy.Dock = DockStyle.Fill;
            lblCreatedBy.Font = new Font("Segoe UI", 10F);
            lblCreatedBy.Location = new Point(1287, 0);
            lblCreatedBy.ForeColor = Color.FromArgb(16, 24, 40);
            lblCreatedBy.Name = "lblCreatedBy";
            lblCreatedBy.Margin = new Padding(3);
            lblCreatedBy.AutoEllipsis = true;
            lblCreatedBy.RightToLeft = RightToLeft.No;
            lblCreatedBy.Size = new Size(202, 33);
            lblCreatedBy.TabIndex = 0;
            lblCreatedBy.Text = " أنشأ بواسطة: - مدير النظام";
            lblCreatedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AccessibleName = "البوليصه";
            tableLayoutPanel2.ColumnCount = 10;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 7F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 7F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 7F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 6.22406626F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.2406635F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.229599F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13.2088518F));
            tableLayoutPanel2.Controls.Add(label3, 8, 2);
            tableLayoutPanel2.Controls.Add(cboDriverCollectionAccount, 9, 2);
            tableLayoutPanel2.Controls.Add(cboCustomerReceivableAccount, 7, 2);
            tableLayoutPanel2.Controls.Add(cboWaybillsUnderCollectionAccount, 5, 2);
            tableLayoutPanel2.Controls.Add(label1, 8, 1);
            tableLayoutPanel2.Controls.Add(cboShipmentType, 9, 1);
            tableLayoutPanel2.Controls.Add(cboBankAccount, 7, 1);
            tableLayoutPanel2.Controls.Add(label65, 6, 1);
            tableLayoutPanel2.Controls.Add(cboBank, 5, 1);
            tableLayoutPanel2.Controls.Add(label66, 4, 1);
            tableLayoutPanel2.Controls.Add(cboCashAccount, 3, 1);
            tableLayoutPanel2.Controls.Add(label77, 2, 1);
            tableLayoutPanel2.Controls.Add(cboCashbox, 1, 1);
            tableLayoutPanel2.Controls.Add(label78, 0, 1);
            tableLayoutPanel2.Controls.Add(cboCurrency, 9, 0);
            tableLayoutPanel2.Controls.Add(label2, 8, 0);
            tableLayoutPanel2.Controls.Add(cboBranch, 3, 0);
            tableLayoutPanel2.Controls.Add(cboCompany, 1, 0);
            tableLayoutPanel2.Controls.Add(label88, 6, 2);
            tableLayoutPanel2.Controls.Add(label87, 4, 2);
            tableLayoutPanel2.Controls.Add(cboDeferredRevenueAccount, 3, 2);
            tableLayoutPanel2.Controls.Add(label86, 2, 2);
            tableLayoutPanel2.Controls.Add(cboTransportRevenueAccount, 1, 2);
            tableLayoutPanel2.Controls.Add(label85, 0, 2);
            tableLayoutPanel2.Controls.Add(label81, 6, 0);
            tableLayoutPanel2.Controls.Add(cboLinkType, 5, 0);
            tableLayoutPanel2.Controls.Add(label82, 4, 0);
            tableLayoutPanel2.Controls.Add(label83, 2, 0);
            tableLayoutPanel2.Controls.Add(label84, 0, 0);
            tableLayoutPanel2.Controls.Add(cboPaymentMethod, 7, 0);
            tableLayoutPanel2.Dock = DockStyle.Top;
            tableLayoutPanel2.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel2.Location = new Point(0, 53);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.BackColor = Color.LightCyan;
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.MinimumSize = new Size(0, 120);
            tableLayoutPanel2.Padding = new Padding(0);
            tableLayoutPanel2.RightToLeft = RightToLeft.Yes;
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel2.Size = new Size(1492, 120);
            tableLayoutPanel2.TabIndex = 15;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label3.Location = new Point(205, 110);
            label3.Margin = new Padding(3);
            label3.ForeColor = Color.FromArgb(16, 24, 40);
            label3.BackColor = Color.Transparent;
            label3.Name = "label3";
            label3.AutoEllipsis = true;
            label3.Size = new Size(116, 48);
            label3.TabIndex = 172;
            label3.Text = "حساب عهدة تحصيل السائقين";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboDriverCollectionAccount
            // 
            cboDriverCollectionAccount.FormattingEnabled = true;
            cboDriverCollectionAccount.Location = new Point(18, 110);
            cboDriverCollectionAccount.BackColor = Color.White;
            cboDriverCollectionAccount.ForeColor = Color.FromArgb(16, 24, 40);
            cboDriverCollectionAccount.Name = "cboDriverCollectionAccount";
            cboDriverCollectionAccount.Font = new Font("Segoe UI", 11F);
            cboDriverCollectionAccount.Margin = new Padding(3);
            cboDriverCollectionAccount.Dock = DockStyle.Fill;
            cboDriverCollectionAccount.Size = new Size(181, 31);
            cboDriverCollectionAccount.TabIndex = 171;
            // 
            // cboCustomerReceivableAccount
            // 
            cboCustomerReceivableAccount.Dock = DockStyle.Fill;
            cboCustomerReceivableAccount.FormattingEnabled = true;
            cboCustomerReceivableAccount.Location = new Point(327, 110);
            cboCustomerReceivableAccount.BackColor = Color.White;
            cboCustomerReceivableAccount.ForeColor = Color.FromArgb(16, 24, 40);
            cboCustomerReceivableAccount.Name = "cboCustomerReceivableAccount";
            cboCustomerReceivableAccount.Font = new Font("Segoe UI", 11F);
            cboCustomerReceivableAccount.Margin = new Padding(3);
            cboCustomerReceivableAccount.RightToLeft = RightToLeft.No;
            cboCustomerReceivableAccount.Size = new Size(176, 31);
            cboCustomerReceivableAccount.TabIndex = 170;
            // 
            // cboWaybillsUnderCollectionAccount
            // 
            cboWaybillsUnderCollectionAccount.Dock = DockStyle.Fill;
            cboWaybillsUnderCollectionAccount.FormattingEnabled = true;
            cboWaybillsUnderCollectionAccount.Location = new Point(601, 110);
            cboWaybillsUnderCollectionAccount.BackColor = Color.White;
            cboWaybillsUnderCollectionAccount.ForeColor = Color.FromArgb(16, 24, 40);
            cboWaybillsUnderCollectionAccount.Name = "cboWaybillsUnderCollectionAccount";
            cboWaybillsUnderCollectionAccount.Font = new Font("Segoe UI", 11F);
            cboWaybillsUnderCollectionAccount.Margin = new Padding(3);
            cboWaybillsUnderCollectionAccount.RightToLeft = RightToLeft.No;
            cboWaybillsUnderCollectionAccount.Size = new Size(187, 31);
            cboWaybillsUnderCollectionAccount.TabIndex = 169;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.Location = new Point(205, 58);
            label1.Margin = new Padding(3);
            label1.ForeColor = Color.FromArgb(16, 24, 40);
            label1.BackColor = Color.Transparent;
            label1.Name = "label1";
            label1.AutoEllipsis = true;
            label1.Size = new Size(116, 46);
            label1.TabIndex = 168;
            label1.Text = "نوع الشحن";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboShipmentType
            // 
            cboShipmentType.FormattingEnabled = true;
            cboShipmentType.Location = new Point(18, 58);
            cboShipmentType.BackColor = Color.White;
            cboShipmentType.ForeColor = Color.FromArgb(16, 24, 40);
            cboShipmentType.Name = "cboShipmentType";
            cboShipmentType.Font = new Font("Segoe UI", 11F);
            cboShipmentType.Margin = new Padding(3);
            cboShipmentType.Dock = DockStyle.Fill;
            cboShipmentType.Size = new Size(181, 31);
            cboShipmentType.TabIndex = 167;
            // 
            // cboBankAccount
            // 
            cboBankAccount.FormattingEnabled = true;
            cboBankAccount.Location = new Point(327, 58);
            cboBankAccount.BackColor = Color.White;
            cboBankAccount.ForeColor = Color.FromArgb(16, 24, 40);
            cboBankAccount.Name = "cboBankAccount";
            cboBankAccount.Font = new Font("Segoe UI", 11F);
            cboBankAccount.Margin = new Padding(3);
            cboBankAccount.Dock = DockStyle.Fill;
            cboBankAccount.Size = new Size(176, 31);
            cboBankAccount.TabIndex = 165;
            // 
            // label65
            // 
            label65.Dock = DockStyle.Fill;
            label65.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label65.Location = new Point(509, 55);
            label65.ForeColor = Color.FromArgb(16, 24, 40);
            label65.BackColor = Color.Transparent;
            label65.Name = "label65";
            label65.Margin = new Padding(3);
            label65.AutoEllipsis = true;
            label65.Size = new Size(86, 52);
            label65.TabIndex = 164;
            label65.Text = "حساب البنك";
            label65.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboBank
            // 
            cboBank.FormattingEnabled = true;
            cboBank.Location = new Point(607, 58);
            cboBank.BackColor = Color.White;
            cboBank.ForeColor = Color.FromArgb(16, 24, 40);
            cboBank.Name = "cboBank";
            cboBank.Font = new Font("Segoe UI", 11F);
            cboBank.Margin = new Padding(3);
            cboBank.Dock = DockStyle.Fill;
            cboBank.Size = new Size(181, 31);
            cboBank.TabIndex = 163;
            // 
            // label66
            // 
            label66.Dock = DockStyle.Fill;
            label66.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label66.Location = new Point(794, 55);
            label66.ForeColor = Color.FromArgb(16, 24, 40);
            label66.BackColor = Color.Transparent;
            label66.Name = "label66";
            label66.Margin = new Padding(3);
            label66.AutoEllipsis = true;
            label66.Size = new Size(98, 52);
            label66.TabIndex = 162;
            label66.Text = "البنك";
            label66.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboCashAccount
            // 
            cboCashAccount.FormattingEnabled = true;
            cboCashAccount.Location = new Point(904, 58);
            cboCashAccount.BackColor = Color.White;
            cboCashAccount.ForeColor = Color.FromArgb(16, 24, 40);
            cboCashAccount.Name = "cboCashAccount";
            cboCashAccount.Font = new Font("Segoe UI", 11F);
            cboCashAccount.Margin = new Padding(3);
            cboCashAccount.Dock = DockStyle.Fill;
            cboCashAccount.Size = new Size(181, 31);
            cboCashAccount.TabIndex = 161;
            // 
            // label77
            // 
            label77.Dock = DockStyle.Fill;
            label77.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label77.Location = new Point(1091, 55);
            label77.ForeColor = Color.FromArgb(16, 24, 40);
            label77.BackColor = Color.Transparent;
            label77.Name = "label77";
            label77.Margin = new Padding(3);
            label77.AutoEllipsis = true;
            label77.Size = new Size(98, 52);
            label77.TabIndex = 160;
            label77.Text = "حساب الصندوق";
            label77.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboCashbox
            // 
            cboCashbox.FormattingEnabled = true;
            cboCashbox.Location = new Point(1201, 58);
            cboCashbox.BackColor = Color.White;
            cboCashbox.ForeColor = Color.FromArgb(16, 24, 40);
            cboCashbox.Name = "cboCashbox";
            cboCashbox.Font = new Font("Segoe UI", 11F);
            cboCashbox.Margin = new Padding(3);
            cboCashbox.Dock = DockStyle.Fill;
            cboCashbox.Size = new Size(181, 31);
            cboCashbox.TabIndex = 159;
            // 
            // label78
            // 
            label78.Dock = DockStyle.Fill;
            label78.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label78.Location = new Point(1388, 55);
            label78.ForeColor = Color.FromArgb(16, 24, 40);
            label78.BackColor = Color.Transparent;
            label78.Name = "label78";
            label78.Margin = new Padding(3);
            label78.AutoEllipsis = true;
            label78.Size = new Size(98, 52);
            label78.TabIndex = 158;
            label78.Text = "الصندوق";
            label78.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboCurrency
            // 
            cboCurrency.FormattingEnabled = true;
            cboCurrency.Location = new Point(18, 6);
            cboCurrency.BackColor = Color.White;
            cboCurrency.ForeColor = Color.FromArgb(16, 24, 40);
            cboCurrency.Name = "cboCurrency";
            cboCurrency.Font = new Font("Segoe UI", 11F);
            cboCurrency.Margin = new Padding(3);
            cboCurrency.Dock = DockStyle.Fill;
            cboCurrency.Size = new Size(181, 31);
            cboCurrency.TabIndex = 157;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label2.Location = new Point(205, 6);
            label2.Margin = new Padding(3);
            label2.ForeColor = Color.FromArgb(16, 24, 40);
            label2.BackColor = Color.Transparent;
            label2.Name = "label2";
            label2.AutoEllipsis = true;
            label2.Size = new Size(116, 46);
            label2.TabIndex = 156;
            label2.Text = "العمله";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboBranch
            // 
            cboBranch.FormattingEnabled = true;
            cboBranch.Location = new Point(904, 6);
            cboBranch.BackColor = Color.White;
            cboBranch.ForeColor = Color.FromArgb(16, 24, 40);
            cboBranch.Name = "cboBranch";
            cboBranch.Font = new Font("Segoe UI", 11F);
            cboBranch.Margin = new Padding(3);
            cboBranch.Dock = DockStyle.Fill;
            cboBranch.Size = new Size(181, 31);
            cboBranch.TabIndex = 148;
            // 
            // cboCompany
            // 
            cboCompany.FormattingEnabled = true;
            cboCompany.Location = new Point(1201, 6);
            cboCompany.BackColor = Color.White;
            cboCompany.ForeColor = Color.FromArgb(16, 24, 40);
            cboCompany.Name = "cboCompany";
            cboCompany.Font = new Font("Segoe UI", 11F);
            cboCompany.Margin = new Padding(3);
            cboCompany.Dock = DockStyle.Fill;
            cboCompany.Size = new Size(181, 31);
            cboCompany.TabIndex = 16;
            // 
            // label88
            // 
            label88.Dock = DockStyle.Fill;
            label88.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label88.Location = new Point(509, 107);
            label88.ForeColor = Color.FromArgb(16, 24, 40);
            label88.BackColor = Color.Transparent;
            label88.Name = "label88";
            label88.Margin = new Padding(3);
            label88.AutoEllipsis = true;
            label88.Size = new Size(86, 54);
            label88.TabIndex = 137;
            label88.Text = "حساب ذمم العملاء";
            label88.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label87
            // 
            label87.Dock = DockStyle.Fill;
            label87.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label87.Location = new Point(794, 107);
            label87.ForeColor = Color.FromArgb(16, 24, 40);
            label87.BackColor = Color.Transparent;
            label87.Name = "label87";
            label87.Margin = new Padding(3);
            label87.AutoEllipsis = true;
            label87.Size = new Size(98, 54);
            label87.TabIndex = 135;
            label87.Text = "حساب البوالص تحت التحصيل";
            label87.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboDeferredRevenueAccount
            // 
            cboDeferredRevenueAccount.Dock = DockStyle.Fill;
            cboDeferredRevenueAccount.FormattingEnabled = true;
            cboDeferredRevenueAccount.Location = new Point(898, 110);
            cboDeferredRevenueAccount.BackColor = Color.White;
            cboDeferredRevenueAccount.ForeColor = Color.FromArgb(16, 24, 40);
            cboDeferredRevenueAccount.Name = "cboDeferredRevenueAccount";
            cboDeferredRevenueAccount.Font = new Font("Segoe UI", 11F);
            cboDeferredRevenueAccount.Margin = new Padding(3);
            cboDeferredRevenueAccount.RightToLeft = RightToLeft.No;
            cboDeferredRevenueAccount.Size = new Size(187, 31);
            cboDeferredRevenueAccount.TabIndex = 134;
            // 
            // label86
            // 
            label86.Dock = DockStyle.Fill;
            label86.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label86.Location = new Point(1091, 107);
            label86.ForeColor = Color.FromArgb(16, 24, 40);
            label86.BackColor = Color.Transparent;
            label86.Name = "label86";
            label86.Margin = new Padding(3);
            label86.AutoEllipsis = true;
            label86.Size = new Size(98, 54);
            label86.TabIndex = 133;
            label86.Text = "حساب ايرادات النقل المقدمة";
            label86.TextAlign = ContentAlignment.MiddleLeft;
            label86.Click += label86_Click;
            // 
            // cboTransportRevenueAccount
            // 
            cboTransportRevenueAccount.Dock = DockStyle.Fill;
            cboTransportRevenueAccount.FormattingEnabled = true;
            cboTransportRevenueAccount.Items.AddRange(new object[] { "مارب", "المرور", "نجد قسيم" });
            cboTransportRevenueAccount.Location = new Point(1195, 110);
            cboTransportRevenueAccount.BackColor = Color.White;
            cboTransportRevenueAccount.ForeColor = Color.FromArgb(16, 24, 40);
            cboTransportRevenueAccount.Name = "cboTransportRevenueAccount";
            cboTransportRevenueAccount.Font = new Font("Segoe UI", 11F);
            cboTransportRevenueAccount.Margin = new Padding(3);
            cboTransportRevenueAccount.RightToLeft = RightToLeft.No;
            cboTransportRevenueAccount.Size = new Size(187, 31);
            cboTransportRevenueAccount.TabIndex = 132;
            // 
            // label85
            // 
            label85.Dock = DockStyle.Fill;
            label85.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label85.Location = new Point(1388, 110);
            label85.Margin = new Padding(3);
            label85.ForeColor = Color.FromArgb(16, 24, 40);
            label85.BackColor = Color.Transparent;
            label85.Name = "label85";
            label85.AutoEllipsis = true;
            label85.Size = new Size(98, 48);
            label85.TabIndex = 131;
            label85.Text = "حساب ايراد النقل";
            label85.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label81
            // 
            label81.AutoSize = true;
            label81.Dock = DockStyle.Fill;
            label81.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label81.Location = new Point(509, 3);
            label81.ForeColor = Color.FromArgb(16, 24, 40);
            label81.BackColor = Color.Transparent;
            label81.Name = "label81";
            label81.Margin = new Padding(3);
            label81.AutoEllipsis = true;
            label81.Size = new Size(86, 52);
            label81.TabIndex = 126;
            label81.Text = "طريقة الدفع";
            label81.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboLinkType
            // 
            cboLinkType.Dock = DockStyle.Fill;
            cboLinkType.Font = new Font("Segoe UI", 11F);
            cboLinkType.FormattingEnabled = true;
            cboLinkType.Location = new Point(601, 6);
            cboLinkType.BackColor = Color.White;
            cboLinkType.ForeColor = Color.FromArgb(16, 24, 40);
            cboLinkType.Name = "cboLinkType";
            cboLinkType.Margin = new Padding(3);
            cboLinkType.RightToLeft = RightToLeft.No;
            cboLinkType.Size = new Size(187, 28);
            cboLinkType.TabIndex = 125;
            // 
            // label82
            // 
            label82.AutoSize = true;
            label82.Dock = DockStyle.Fill;
            label82.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label82.Location = new Point(794, 3);
            label82.ForeColor = Color.FromArgb(16, 24, 40);
            label82.BackColor = Color.Transparent;
            label82.Name = "label82";
            label82.Margin = new Padding(3);
            label82.AutoEllipsis = true;
            label82.Size = new Size(98, 52);
            label82.TabIndex = 124;
            label82.Text = "نوع الربط";
            label82.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label83
            // 
            label83.AutoSize = true;
            label83.Dock = DockStyle.Fill;
            label83.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label83.Location = new Point(1091, 3);
            label83.ForeColor = Color.FromArgb(16, 24, 40);
            label83.BackColor = Color.Transparent;
            label83.Name = "label83";
            label83.Margin = new Padding(3);
            label83.AutoEllipsis = true;
            label83.Size = new Size(98, 52);
            label83.TabIndex = 122;
            label83.Text = "الفرع";
            label83.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label84
            // 
            label84.AutoSize = true;
            label84.Dock = DockStyle.Fill;
            label84.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label84.Location = new Point(1388, 6);
            label84.Margin = new Padding(3);
            label84.ForeColor = Color.FromArgb(16, 24, 40);
            label84.BackColor = Color.Transparent;
            label84.Name = "label84";
            label84.AutoEllipsis = true;
            label84.Size = new Size(98, 46);
            label84.TabIndex = 120;
            label84.Text = "الشركة";
            label84.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboPaymentMethod
            // 
            cboPaymentMethod.Dock = DockStyle.Fill;
            cboPaymentMethod.FormattingEnabled = true;
            cboPaymentMethod.Items.AddRange(new object[] { "بوليصة نقدية", "بوليصة اجلة" });
            cboPaymentMethod.Location = new Point(327, 6);
            cboPaymentMethod.BackColor = Color.White;
            cboPaymentMethod.ForeColor = Color.FromArgb(16, 24, 40);
            cboPaymentMethod.Name = "cboPaymentMethod";
            cboPaymentMethod.Font = new Font("Segoe UI", 11F);
            cboPaymentMethod.Margin = new Padding(3);
            cboPaymentMethod.RightToLeft = RightToLeft.No;
            cboPaymentMethod.Size = new Size(176, 31);
            cboPaymentMethod.TabIndex = 109;
            // 
            // UcAccountLinking
            // 
            AccessibleName = "ربط الحسابات المحاسبية";
            AutoScaleDimensions = new SizeF(120F, 120F);
            BackColor = Color.FromArgb(248, 250, 252);
            ForeColor = Color.FromArgb(16, 24, 40);
            Padding = new Padding(0);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(tableLayoutPanel2);
            Controls.Add(tlpAuditInfo);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 10F);
            Margin = new Padding(0);
            Name = "UcAccountLinking";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1492, 677);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            tlpAuditInfo.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(130, 0);
        designerCommandBar.Size = new Size(1362, 53);
        designerCommandBar.Margin = new Padding(5);
        designerCommandBar.MinimumSize = new Size(0, 48);
        designerCommandBar.TabIndex = 2;
        designerCommandBar.RightToLeft = RightToLeft.Yes;
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
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox txtRemainingAmount;
        private TextBox txtPaidAmount;
        private Label lblTotalAmount;
        private ComboBox cboCollectionStatus;
        private TextBox txtNotes;
        private Label label88;
        private TextBox txtReferenceNumber;
        private Label label87;
        private ComboBox cboCollectorDriver;
        private Label label86;
        private ComboBox cboCollectionBranch;
        private Label label85;
        private TextBox txtLocalAmount;
        private Label label81;
        private ComboBox cboPaymentMethod;
        private Label label82;
        private ComboBox cboPaymentLocation;
        private Label label83;
        private ComboBox cboPayer;
        private Label label84;
        private Label label65;
        private Label label66;
        private Label label77;
        private Label label78;
        private Label label79;
        private ComboBox cboCurrency;
        private TextBox txtExchangeRate;
        private Label label80;
        private ComboBox comboBox1;
        private ComboBox cboCompany;
        private Label label1;
        private ComboBox cboBranch;
        private ComboBox cboLinkType;
        private ComboBox comboBox2;
        private ComboBox comboBox3;
        private ComboBox cboBankAccount;
        private ComboBox cboBank;
        private ComboBox cboCashAccount;
        private ComboBox cboCashbox;
        private Label label2;
        private ComboBox cboShipmentType;
        private Label label3;
        private ComboBox cboDriverCollectionAccount;
        private ComboBox cboCustomerReceivableAccount;
        private ComboBox cboWaybillsUnderCollectionAccount;
        private ComboBox cboDeferredRevenueAccount;
        private ComboBox cboTransportRevenueAccount;
    }
}
