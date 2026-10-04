namespace TransportERP.Desktop.البوالص_والشحن.المدخلات
{
    partial class UcRoutes
    {
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
            dgvShipmentTypes = new TableLayoutPanel();
            nudDisplayOrder = new NumericUpDown();
            nub3 = new Label();
            labl2 = new Label();
            txtNotes = new TextBox();
            label2 = new Label();
            nudEstimatedHours = new NumericUpDown();
            nud2 = new Label();
            nud1 = new Label();
            cmbDestinationPoint = new ComboBox();
            cmbOriginPoint = new ComboBox();
            label1 = new Label();
            txtRouteNameEn = new TextBox();
            txtRouteCode = new TextBox();
            combox2 = new Label();
            combox1 = new Label();
            textbox1 = new Label();
            combox22 = new Label();
            textBox13 = new Label();
            txtRouteNameAr = new TextBox();
            cmbRouteType = new ComboBox();
            nudDistanceKm = new NumericUpDown();
            txtDescription = new TextBox();
            chkIsActive = new CheckBox();
            tabPage1 = new TabPage();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            tlpAuditInfo.SuspendLayout();
            pnlContent.SuspendLayout();
            tabMain.SuspendLayout();
            tabPricingRules.SuspendLayout();
            dgvShipmentTypes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudDisplayOrder).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudEstimatedHours).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDistanceKm).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(192, 192, 255);
            pnlHeader.Controls.Add(pnlToolbar);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1200, 53);
            pnlHeader.TabIndex = 8;
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.FromArgb(192, 192, 255);
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
            pnlToolbar.Dock = DockStyle.Fill;
            pnlToolbar.Location = new Point(174, 0);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Padding = new Padding(5);
            pnlToolbar.Size = new Size(1026, 53);
            pnlToolbar.TabIndex = 2;
            pnlToolbar.WrapContents = false;
            // 
            // btnNew
            // 
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Microsoft Sans Serif", 10F);
            btnNew.Location = new Point(922, 9);
            btnNew.Margin = new Padding(4);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(90, 38);
            btnNew.TabIndex = 0;
            btnNew.Text = "جديد";
            btnNew.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Microsoft Sans Serif", 10F);
            btnSave.Location = new Point(824, 9);
            btnSave.Margin = new Padding(4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 38);
            btnSave.TabIndex = 1;
            btnSave.Text = "حفظ";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Microsoft Sans Serif", 10F);
            btnEdit.Location = new Point(726, 9);
            btnEdit.Margin = new Padding(4);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(90, 38);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "تعديل";
            btnEdit.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Microsoft Sans Serif", 10F);
            btnDelete.Location = new Point(628, 9);
            btnDelete.Margin = new Padding(4);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(90, 38);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "حذف";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Microsoft Sans Serif", 10F);
            btnRefresh.Location = new Point(530, 9);
            btnRefresh.Margin = new Padding(4);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(90, 38);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "تحديث";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Microsoft Sans Serif", 10F);
            btnClose.Location = new Point(432, 9);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(90, 38);
            btnClose.TabIndex = 5;
            btnClose.Text = "اغلاق";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // btnFirst
            // 
            btnFirst.FlatStyle = FlatStyle.Flat;
            btnFirst.Font = new Font("Microsoft Sans Serif", 10F);
            btnFirst.Location = new Point(334, 9);
            btnFirst.Margin = new Padding(4);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(90, 38);
            btnFirst.TabIndex = 6;
            btnFirst.Text = "الاول ";
            btnFirst.UseVisualStyleBackColor = true;
            // 
            // btnPrevious
            // 
            btnPrevious.FlatStyle = FlatStyle.Flat;
            btnPrevious.Font = new Font("Microsoft Sans Serif", 10F);
            btnPrevious.Location = new Point(236, 9);
            btnPrevious.Margin = new Padding(4);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(90, 38);
            btnPrevious.TabIndex = 7;
            btnPrevious.Text = "السابق";
            btnPrevious.UseVisualStyleBackColor = true;
            // 
            // txtCurrentRecordNo
            // 
            txtCurrentRecordNo.Location = new Point(165, 10);
            txtCurrentRecordNo.Margin = new Padding(5);
            txtCurrentRecordNo.Multiline = true;
            txtCurrentRecordNo.Name = "txtCurrentRecordNo";
            txtCurrentRecordNo.ReadOnly = true;
            txtCurrentRecordNo.Size = new Size(62, 36);
            txtCurrentRecordNo.TabIndex = 8;
            txtCurrentRecordNo.TextAlign = HorizontalAlignment.Center;
            // 
            // btnNext
            // 
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("Microsoft Sans Serif", 10F);
            btnNext.Location = new Point(66, 9);
            btnNext.Margin = new Padding(4);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(90, 38);
            btnNext.TabIndex = 9;
            btnNext.Text = "التالي";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnLast
            // 
            btnLast.FlatStyle = FlatStyle.Flat;
            btnLast.Font = new Font("Microsoft Sans Serif", 10F);
            btnLast.Location = new Point(-32, 9);
            btnLast.Margin = new Padding(4);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(90, 38);
            btnLast.TabIndex = 10;
            btnLast.Text = "الاخير";
            btnLast.UseVisualStyleBackColor = true;
            // 
            // btnUndo
            // 
            btnUndo.FlatStyle = FlatStyle.Flat;
            btnUndo.Font = new Font("Microsoft Sans Serif", 10F);
            btnUndo.Location = new Point(-130, 9);
            btnUndo.Margin = new Padding(4);
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new Size(90, 38);
            btnUndo.TabIndex = 11;
            btnUndo.Text = "تراجع";
            btnUndo.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.FromArgb(192, 192, 255);
            lblTitle.Dock = DockStyle.Left;
            lblTitle.Font = new Font("Segoe UI", 16F);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Padding = new Padding(0, 0, 15, 0);
            lblTitle.RightToLeft = RightToLeft.Yes;
            lblTitle.Size = new Size(174, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "خطوط السير";
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
            tlpAuditInfo.Location = new Point(0, 1117);
            tlpAuditInfo.Margin = new Padding(0);
            tlpAuditInfo.Name = "tlpAuditInfo";
            tlpAuditInfo.RowCount = 1;
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpAuditInfo.Size = new Size(1200, 33);
            tlpAuditInfo.TabIndex = 9;
            // 
            // lblPrintCount
            // 
            lblPrintCount.BackColor = Color.FromArgb(192, 255, 255);
            lblPrintCount.Dock = DockStyle.Fill;
            lblPrintCount.Font = new Font("Microsoft Sans Serif", 9F);
            lblPrintCount.Location = new Point(3, 0);
            lblPrintCount.Name = "lblPrintCount";
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
            lblLastPrintedAt.Font = new Font("Microsoft Sans Serif", 9F);
            lblLastPrintedAt.Location = new Point(147, 0);
            lblLastPrintedAt.Name = "lblLastPrintedAt";
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
            lblEditCount.Font = new Font("Microsoft Sans Serif", 9F);
            lblEditCount.Location = new Point(339, 0);
            lblEditCount.Name = "lblEditCount";
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
            lblModifiedAt.Font = new Font("Microsoft Sans Serif", 9F);
            lblModifiedAt.Location = new Point(483, 0);
            lblModifiedAt.Name = "lblModifiedAt";
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
            lblModifiedBy.Font = new Font("Microsoft Sans Serif", 9F);
            lblModifiedBy.Location = new Point(675, 0);
            lblModifiedBy.Name = "lblModifiedBy";
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
            lblCreatedAt.Font = new Font("Microsoft Sans Serif", 9F);
            lblCreatedAt.Location = new Point(843, 0);
            lblCreatedAt.Name = "lblCreatedAt";
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
            lblCreatedBy.Font = new Font("Microsoft Sans Serif", 9F);
            lblCreatedBy.Location = new Point(1035, 0);
            lblCreatedBy.Name = "lblCreatedBy";
            lblCreatedBy.RightToLeft = RightToLeft.No;
            lblCreatedBy.Size = new Size(162, 33);
            lblCreatedBy.TabIndex = 0;
            lblCreatedBy.Text = " أنشأ بواسطة: - مدير النظام";
            lblCreatedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlContent
            // 
            pnlContent.BackColor = SystemColors.ButtonHighlight;
            pnlContent.Controls.Add(tabMain);
            pnlContent.Dock = DockStyle.Top;
            pnlContent.Location = new Point(0, 53);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1200, 651);
            pnlContent.TabIndex = 11;
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tabPricingRules);
            tabMain.Controls.Add(tabPage1);
            tabMain.Dock = DockStyle.Top;
            tabMain.Location = new Point(0, 0);
            tabMain.Name = "tabMain";
            tabMain.RightToLeftLayout = true;
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1200, 338);
            tabMain.TabIndex = 0;
            // 
            // tabPricingRules
            // 
            tabPricingRules.BackColor = Color.SeaShell;
            tabPricingRules.Controls.Add(dgvShipmentTypes);
            tabPricingRules.Location = new Point(4, 32);
            tabPricingRules.Name = "tabPricingRules";
            tabPricingRules.Size = new Size(1192, 302);
            tabPricingRules.TabIndex = 2;
            tabPricingRules.Text = "خطوط السير";
            // 
            // dgvShipmentTypes
            // 
            dgvShipmentTypes.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            dgvShipmentTypes.BackColor = Color.Snow;
            dgvShipmentTypes.ColumnCount = 4;
            dgvShipmentTypes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            dgvShipmentTypes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            dgvShipmentTypes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            dgvShipmentTypes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            dgvShipmentTypes.Controls.Add(nudDisplayOrder, 1, 5);
            dgvShipmentTypes.Controls.Add(nub3, 0, 5);
            dgvShipmentTypes.Controls.Add(labl2, 2, 4);
            dgvShipmentTypes.Controls.Add(txtNotes, 3, 4);
            dgvShipmentTypes.Controls.Add(label2, 0, 4);
            dgvShipmentTypes.Controls.Add(nudEstimatedHours, 3, 3);
            dgvShipmentTypes.Controls.Add(nud2, 2, 3);
            dgvShipmentTypes.Controls.Add(nud1, 0, 3);
            dgvShipmentTypes.Controls.Add(cmbDestinationPoint, 3, 2);
            dgvShipmentTypes.Controls.Add(cmbOriginPoint, 1, 2);
            dgvShipmentTypes.Controls.Add(label1, 2, 1);
            dgvShipmentTypes.Controls.Add(txtRouteNameEn, 3, 1);
            dgvShipmentTypes.Controls.Add(txtRouteCode, 1, 0);
            dgvShipmentTypes.Controls.Add(combox2, 2, 2);
            dgvShipmentTypes.Controls.Add(combox1, 0, 2);
            dgvShipmentTypes.Controls.Add(textbox1, 0, 1);
            dgvShipmentTypes.Controls.Add(combox22, 2, 0);
            dgvShipmentTypes.Controls.Add(textBox13, 0, 0);
            dgvShipmentTypes.Controls.Add(txtRouteNameAr, 1, 1);
            dgvShipmentTypes.Controls.Add(cmbRouteType, 3, 0);
            dgvShipmentTypes.Controls.Add(nudDistanceKm, 1, 3);
            dgvShipmentTypes.Controls.Add(txtDescription, 1, 4);
            dgvShipmentTypes.Controls.Add(chkIsActive, 2, 5);
            dgvShipmentTypes.Dock = DockStyle.Top;
            dgvShipmentTypes.Location = new Point(0, 0);
            dgvShipmentTypes.Margin = new Padding(0);
            dgvShipmentTypes.Name = "dgvShipmentTypes";
            dgvShipmentTypes.Padding = new Padding(10);
            dgvShipmentTypes.RowCount = 6;
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            dgvShipmentTypes.Size = new Size(1192, 256);
            dgvShipmentTypes.TabIndex = 11;
            // 
            // nudDisplayOrder
            // 
            nudDisplayOrder.Dock = DockStyle.Left;
            nudDisplayOrder.Location = new Point(738, 213);
            nudDisplayOrder.Name = "nudDisplayOrder";
            nudDisplayOrder.Size = new Size(266, 30);
            nudDisplayOrder.TabIndex = 41;
            // 
            // nub3
            // 
            nub3.Dock = DockStyle.Fill;
            nub3.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            nub3.Location = new Point(1010, 210);
            nub3.Name = "nub3";
            nub3.Size = new Size(169, 40);
            nub3.TabIndex = 40;
            nub3.Text = "ترتيب العرض";
            nub3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labl2
            // 
            labl2.Dock = DockStyle.Fill;
            labl2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            labl2.Location = new Point(425, 170);
            labl2.Name = "labl2";
            labl2.Size = new Size(169, 40);
            labl2.TabIndex = 38;
            labl2.Text = "ملاحظات";
            labl2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNotes
            // 
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Location = new Point(13, 173);
            txtNotes.Name = "txtNotes";
            txtNotes.Size = new Size(406, 30);
            txtNotes.TabIndex = 39;
            // 
            // label2
            // 
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label2.Location = new Point(1010, 170);
            label2.Name = "label2";
            label2.Size = new Size(169, 40);
            label2.TabIndex = 36;
            label2.Text = "الوصف";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudEstimatedHours
            // 
            nudEstimatedHours.Location = new Point(83, 133);
            nudEstimatedHours.Name = "nudEstimatedHours";
            nudEstimatedHours.Size = new Size(336, 30);
            nudEstimatedHours.TabIndex = 35;
            // 
            // nud2
            // 
            nud2.Dock = DockStyle.Fill;
            nud2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            nud2.Location = new Point(425, 130);
            nud2.Name = "nud2";
            nud2.Size = new Size(169, 40);
            nud2.TabIndex = 34;
            nud2.Text = "المدة التقديريه بالساعه";
            nud2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nud1
            // 
            nud1.Dock = DockStyle.Fill;
            nud1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            nud1.Location = new Point(1010, 130);
            nud1.Name = "nud1";
            nud1.Size = new Size(169, 40);
            nud1.TabIndex = 31;
            nud1.Text = "المسافة بالكيلو متر";
            nud1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbDestinationPoint
            // 
            cmbDestinationPoint.Dock = DockStyle.Fill;
            cmbDestinationPoint.FormattingEnabled = true;
            cmbDestinationPoint.Location = new Point(13, 93);
            cmbDestinationPoint.Name = "cmbDestinationPoint";
            cmbDestinationPoint.Size = new Size(406, 31);
            cmbDestinationPoint.TabIndex = 28;
            // 
            // cmbOriginPoint
            // 
            cmbOriginPoint.Dock = DockStyle.Fill;
            cmbOriginPoint.FormattingEnabled = true;
            cmbOriginPoint.Location = new Point(600, 93);
            cmbOriginPoint.Name = "cmbOriginPoint";
            cmbOriginPoint.Size = new Size(404, 31);
            cmbOriginPoint.TabIndex = 27;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label1.Location = new Point(425, 50);
            label1.Name = "label1";
            label1.Size = new Size(169, 40);
            label1.TabIndex = 25;
            label1.Text = "اسم خط السير انجليزي";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtRouteNameEn
            // 
            txtRouteNameEn.Dock = DockStyle.Fill;
            txtRouteNameEn.Location = new Point(13, 53);
            txtRouteNameEn.Name = "txtRouteNameEn";
            txtRouteNameEn.Size = new Size(406, 30);
            txtRouteNameEn.TabIndex = 26;
            // 
            // txtRouteCode
            // 
            txtRouteCode.Location = new Point(808, 13);
            txtRouteCode.Name = "txtRouteCode";
            txtRouteCode.Size = new Size(196, 30);
            txtRouteCode.TabIndex = 21;
            // 
            // combox2
            // 
            combox2.Dock = DockStyle.Fill;
            combox2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            combox2.Location = new Point(425, 90);
            combox2.Name = "combox2";
            combox2.Size = new Size(169, 40);
            combox2.TabIndex = 18;
            combox2.Text = "نطقة الوصول";
            combox2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // combox1
            // 
            combox1.Dock = DockStyle.Fill;
            combox1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            combox1.Location = new Point(1010, 90);
            combox1.Name = "combox1";
            combox1.Size = new Size(169, 40);
            combox1.TabIndex = 8;
            combox1.Text = "نقطة الأنطلاق";
            combox1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textbox1
            // 
            textbox1.Dock = DockStyle.Fill;
            textbox1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            textbox1.Location = new Point(1010, 50);
            textbox1.Name = "textbox1";
            textbox1.Size = new Size(169, 40);
            textbox1.TabIndex = 4;
            textbox1.Text = "اسم خط السير عربي";
            textbox1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // combox22
            // 
            combox22.Dock = DockStyle.Fill;
            combox22.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            combox22.Location = new Point(425, 10);
            combox22.Name = "combox22";
            combox22.Size = new Size(169, 40);
            combox22.TabIndex = 2;
            combox22.Text = "نوع خط السير";
            combox22.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBox13
            // 
            textBox13.Dock = DockStyle.Fill;
            textBox13.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            textBox13.Location = new Point(1010, 10);
            textBox13.Name = "textBox13";
            textBox13.Size = new Size(169, 40);
            textBox13.TabIndex = 0;
            textBox13.Text = "كود خط السير";
            textBox13.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtRouteNameAr
            // 
            txtRouteNameAr.Dock = DockStyle.Fill;
            txtRouteNameAr.Location = new Point(600, 53);
            txtRouteNameAr.Name = "txtRouteNameAr";
            txtRouteNameAr.Size = new Size(404, 30);
            txtRouteNameAr.TabIndex = 22;
            // 
            // cmbRouteType
            // 
            cmbRouteType.Dock = DockStyle.Fill;
            cmbRouteType.FormattingEnabled = true;
            cmbRouteType.Location = new Point(13, 13);
            cmbRouteType.Name = "cmbRouteType";
            cmbRouteType.Size = new Size(406, 31);
            cmbRouteType.TabIndex = 24;
            // 
            // nudDistanceKm
            // 
            nudDistanceKm.Dock = DockStyle.Left;
            nudDistanceKm.Location = new Point(738, 133);
            nudDistanceKm.Name = "nudDistanceKm";
            nudDistanceKm.Size = new Size(266, 30);
            nudDistanceKm.TabIndex = 32;
            // 
            // txtDescription
            // 
            txtDescription.Dock = DockStyle.Fill;
            txtDescription.Location = new Point(600, 173);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(404, 30);
            txtDescription.TabIndex = 37;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkIsActive.Location = new Point(524, 213);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(70, 29);
            chkIsActive.TabIndex = 42;
            chkIsActive.Text = "نشط";
            chkIsActive.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 32);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1192, 399);
            tabPage1.TabIndex = 3;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // UcRoutes
            // 
            AccessibleName = "خطوط السير";
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(pnlContent);
            Controls.Add(tlpAuditInfo);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(731, 1150);
            Name = "UcRoutes";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1200, 1150);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            tlpAuditInfo.ResumeLayout(false);
            pnlContent.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            tabPricingRules.ResumeLayout(false);
            dgvShipmentTypes.ResumeLayout(false);
            dgvShipmentTypes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudDisplayOrder).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudEstimatedHours).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDistanceKm).EndInit();
            ResumeLayout(false);
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
        private TableLayoutPanel dgvShipmentTypes;
        private NumericUpDown nudDisplayOrder;
        private Label nub3;
        private Label labl2;
        private TextBox txtNotes;
        private Label label2;
        private NumericUpDown nudEstimatedHours;
        private Label nud2;
        private Label nud1;
        private ComboBox cmbDestinationPoint;
        private ComboBox cmbOriginPoint;
        private Label label1;
        private TextBox txtRouteNameEn;
        private TextBox txtRouteCode;
        private Label combox2;
        private Label combox1;
        private Label textbox1;
        private Label combox22;
        private Label textBox13;
        private TextBox txtRouteNameAr;
        private ComboBox cmbRouteType;
        private NumericUpDown nudDistanceKm;
        private TextBox txtDescription;
        private CheckBox chkIsActive;
        private TabPage tabPage1;
        private Label DataGridView;
        private TextBox textDescription;
        private Label lblShipmentTypeNameEn;
        private TextBox txtShipmentTypeNameEn;
        private TextBox txtShipmentTypeName;
        private DataGridView dataGridView1;
    }
}
