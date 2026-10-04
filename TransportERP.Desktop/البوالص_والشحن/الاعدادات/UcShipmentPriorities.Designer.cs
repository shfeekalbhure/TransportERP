namespace TransportERP.Desktop.البوالص_والشحن.الاعدادات
{
    partial class dgvShipmentPriorities
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
            components = new System.ComponentModel.Container();
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
            dgvShipmentTypes = new TableLayoutPanel();
            textDescription = new TextBox();
            lblDescription = new Label();
            nudDefaultSurcharge = new NumericUpDown();
            nudDisplayOrder = new NumericUpDown();
            lblDefaultSurcharge = new Label();
            lblPriorityNameEn = new Label();
            txtPriorityNameEn = new TextBox();
            lblDisplayOrder = new Label();
            lblPriorityNameAr = new Label();
            txtPriorityName = new TextBox();
            lblPriorityCode = new Label();
            txtPriorityCode = new TextBox();
            chkIsActive = new CheckBox();
            lblPrioritiesList = new Label();
            dataGridView1 = new DataGridView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            tlpAuditInfo.SuspendLayout();
            panel1.SuspendLayout();
            dgvShipmentTypes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudDefaultSurcharge).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDisplayOrder).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
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
            pnlHeader.Size = new Size(1435, 53);
            pnlHeader.TabIndex = 4;
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
            pnlToolbar.Location = new Point(199, 0);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Padding = new Padding(5);
            pnlToolbar.Size = new Size(1236, 53);
            pnlToolbar.TabIndex = 2;
            pnlToolbar.WrapContents = false;
            // 
            // btnNew
            // 
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Microsoft Sans Serif", 10F);
            btnNew.Location = new Point(1132, 9);
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
            btnSave.Location = new Point(1034, 9);
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
            btnEdit.Location = new Point(936, 9);
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
            btnDelete.Location = new Point(838, 9);
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
            btnRefresh.Location = new Point(740, 9);
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
            btnClose.Location = new Point(642, 9);
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
            btnFirst.Location = new Point(544, 9);
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
            btnPrevious.Location = new Point(446, 9);
            btnPrevious.Margin = new Padding(4);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(90, 38);
            btnPrevious.TabIndex = 7;
            btnPrevious.Text = "السابق";
            btnPrevious.UseVisualStyleBackColor = true;
            // 
            // txtCurrentRecordNo
            // 
            txtCurrentRecordNo.Location = new Point(375, 10);
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
            btnNext.Location = new Point(276, 9);
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
            btnLast.Location = new Point(178, 9);
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
            btnUndo.Location = new Point(80, 9);
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
            lblTitle.Size = new Size(199, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "أولويات الشحن";
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
            tlpAuditInfo.Name = "tlpAuditInfo";
            tlpAuditInfo.RowCount = 1;
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAuditInfo.Size = new Size(1435, 33);
            tlpAuditInfo.TabIndex = 5;
            // 
            // lblPrintCount
            // 
            lblPrintCount.BackColor = Color.FromArgb(192, 255, 255);
            lblPrintCount.Dock = DockStyle.Fill;
            lblPrintCount.Font = new Font("Microsoft Sans Serif", 9F);
            lblPrintCount.Location = new Point(3, 0);
            lblPrintCount.Name = "lblPrintCount";
            lblPrintCount.RightToLeft = RightToLeft.No;
            lblPrintCount.Size = new Size(170, 33);
            lblPrintCount.TabIndex = 8;
            lblPrintCount.Text = "عدد مرات الطباعة :- 00";
            lblPrintCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblLastPrintedAt
            // 
            lblLastPrintedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblLastPrintedAt.Dock = DockStyle.Fill;
            lblLastPrintedAt.Font = new Font("Microsoft Sans Serif", 9F);
            lblLastPrintedAt.Location = new Point(179, 0);
            lblLastPrintedAt.Name = "lblLastPrintedAt";
            lblLastPrintedAt.RightToLeft = RightToLeft.No;
            lblLastPrintedAt.Size = new Size(223, 33);
            lblLastPrintedAt.TabIndex = 5;
            lblLastPrintedAt.Text = "تاريخ اخر طباعة:  27/09/2026";
            lblLastPrintedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblEditCount
            // 
            lblEditCount.BackColor = Color.FromArgb(192, 255, 255);
            lblEditCount.Dock = DockStyle.Fill;
            lblEditCount.Font = new Font("Microsoft Sans Serif", 9F);
            lblEditCount.Location = new Point(408, 0);
            lblEditCount.Name = "lblEditCount";
            lblEditCount.RightToLeft = RightToLeft.No;
            lblEditCount.Size = new Size(166, 33);
            lblEditCount.TabIndex = 4;
            lblEditCount.Text = "عدد التعديلات :- 00";
            lblEditCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedAt
            // 
            lblModifiedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedAt.Dock = DockStyle.Fill;
            lblModifiedAt.Font = new Font("Microsoft Sans Serif", 9F);
            lblModifiedAt.Location = new Point(580, 0);
            lblModifiedAt.Name = "lblModifiedAt";
            lblModifiedAt.RightToLeft = RightToLeft.No;
            lblModifiedAt.Size = new Size(223, 33);
            lblModifiedAt.TabIndex = 3;
            lblModifiedAt.Text = " تاريخ التعديل:  27/09/2026";
            lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedBy
            // 
            lblModifiedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedBy.Dock = DockStyle.Fill;
            lblModifiedBy.Font = new Font("Microsoft Sans Serif", 9F);
            lblModifiedBy.Location = new Point(809, 0);
            lblModifiedBy.Name = "lblModifiedBy";
            lblModifiedBy.RightToLeft = RightToLeft.No;
            lblModifiedBy.Size = new Size(194, 33);
            lblModifiedBy.TabIndex = 2;
            lblModifiedBy.Text = "عدل بواسطة: - مدير النظام";
            lblModifiedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedAt
            // 
            lblCreatedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedAt.Dock = DockStyle.Fill;
            lblCreatedAt.Font = new Font("Microsoft Sans Serif", 9F);
            lblCreatedAt.Location = new Point(1009, 0);
            lblCreatedAt.Name = "lblCreatedAt";
            lblCreatedAt.RightToLeft = RightToLeft.No;
            lblCreatedAt.Size = new Size(223, 33);
            lblCreatedAt.TabIndex = 1;
            lblCreatedAt.Text = " تاريخ الانشاء:  27/09/2026";
            lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedBy
            // 
            lblCreatedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedBy.Dock = DockStyle.Fill;
            lblCreatedBy.Font = new Font("Microsoft Sans Serif", 9F);
            lblCreatedBy.Location = new Point(1238, 0);
            lblCreatedBy.Name = "lblCreatedBy";
            lblCreatedBy.RightToLeft = RightToLeft.No;
            lblCreatedBy.Size = new Size(194, 33);
            lblCreatedBy.TabIndex = 0;
            lblCreatedBy.Text = " أنشأ بواسطة: - مدير النظام";
            lblCreatedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ButtonHighlight;
            panel1.Controls.Add(dgvShipmentTypes);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 53);
            panel1.Name = "panel1";
            panel1.Size = new Size(1435, 229);
            panel1.TabIndex = 6;
            // 
            // dgvShipmentTypes
            // 
            dgvShipmentTypes.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            dgvShipmentTypes.BackColor = Color.WhiteSmoke;
            dgvShipmentTypes.ColumnCount = 4;
            dgvShipmentTypes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            dgvShipmentTypes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            dgvShipmentTypes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            dgvShipmentTypes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            dgvShipmentTypes.Controls.Add(textDescription, 1, 3);
            dgvShipmentTypes.Controls.Add(lblDescription, 0, 3);
            dgvShipmentTypes.Controls.Add(nudDefaultSurcharge, 3, 2);
            dgvShipmentTypes.Controls.Add(nudDisplayOrder, 1, 1);
            dgvShipmentTypes.Controls.Add(lblDefaultSurcharge, 2, 2);
            dgvShipmentTypes.Controls.Add(lblPriorityNameEn, 2, 1);
            dgvShipmentTypes.Controls.Add(txtPriorityNameEn, 3, 1);
            dgvShipmentTypes.Controls.Add(lblDisplayOrder, 0, 1);
            dgvShipmentTypes.Controls.Add(lblPriorityNameAr, 2, 0);
            dgvShipmentTypes.Controls.Add(txtPriorityName, 3, 0);
            dgvShipmentTypes.Controls.Add(lblPriorityCode, 0, 0);
            dgvShipmentTypes.Controls.Add(txtPriorityCode, 1, 0);
            dgvShipmentTypes.Controls.Add(chkIsActive, 2, 3);
            dgvShipmentTypes.Controls.Add(lblPrioritiesList, 0, 2);
            dgvShipmentTypes.Controls.Add(dataGridView1, 1, 2);
            dgvShipmentTypes.Dock = DockStyle.Top;
            dgvShipmentTypes.Location = new Point(0, 0);
            dgvShipmentTypes.Margin = new Padding(0);
            dgvShipmentTypes.Name = "dgvShipmentTypes";
            dgvShipmentTypes.Padding = new Padding(10);
            dgvShipmentTypes.RowCount = 4;
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            dgvShipmentTypes.Size = new Size(1435, 222);
            dgvShipmentTypes.TabIndex = 1;
            // 
            // textDescription
            // 
            textDescription.Dock = DockStyle.Fill;
            textDescription.Location = new Point(723, 165);
            textDescription.Margin = new Padding(5);
            textDescription.Multiline = true;
            textDescription.Name = "textDescription";
            textDescription.Size = new Size(485, 42);
            textDescription.TabIndex = 37;
            // 
            // lblDescription
            // 
            lblDescription.Dock = DockStyle.Fill;
            lblDescription.Font = new Font("Segoe UI", 12F);
            lblDescription.Location = new Point(1216, 160);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(206, 52);
            lblDescription.TabIndex = 36;
            lblDescription.Text = "الوصف";
            lblDescription.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudDefaultSurcharge
            // 
            nudDefaultSurcharge.Dock = DockStyle.Fill;
            nudDefaultSurcharge.Location = new Point(13, 113);
            nudDefaultSurcharge.Name = "nudDefaultSurcharge";
            nudDefaultSurcharge.Size = new Size(490, 27);
            nudDefaultSurcharge.TabIndex = 26;
            // 
            // nudDisplayOrder
            // 
            nudDisplayOrder.Dock = DockStyle.Fill;
            nudDisplayOrder.Location = new Point(721, 63);
            nudDisplayOrder.Name = "nudDisplayOrder";
            nudDisplayOrder.Size = new Size(489, 27);
            nudDisplayOrder.TabIndex = 25;
            // 
            // lblDefaultSurcharge
            // 
            lblDefaultSurcharge.Dock = DockStyle.Fill;
            lblDefaultSurcharge.Font = new Font("Segoe UI", 12F);
            lblDefaultSurcharge.Location = new Point(509, 110);
            lblDefaultSurcharge.Name = "lblDefaultSurcharge";
            lblDefaultSurcharge.Size = new Size(206, 50);
            lblDefaultSurcharge.TabIndex = 23;
            lblDefaultSurcharge.Text = "الزيادة الأفتراضيه";
            lblDefaultSurcharge.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPriorityNameEn
            // 
            lblPriorityNameEn.Dock = DockStyle.Fill;
            lblPriorityNameEn.Font = new Font("Segoe UI", 12F);
            lblPriorityNameEn.Location = new Point(509, 60);
            lblPriorityNameEn.Name = "lblPriorityNameEn";
            lblPriorityNameEn.Size = new Size(206, 50);
            lblPriorityNameEn.TabIndex = 6;
            lblPriorityNameEn.Text = "اسم الأولويه بالانجليزي";
            lblPriorityNameEn.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPriorityNameEn
            // 
            txtPriorityNameEn.Dock = DockStyle.Fill;
            txtPriorityNameEn.Location = new Point(15, 65);
            txtPriorityNameEn.Margin = new Padding(5);
            txtPriorityNameEn.Name = "txtPriorityNameEn";
            txtPriorityNameEn.Size = new Size(486, 27);
            txtPriorityNameEn.TabIndex = 7;
            // 
            // lblDisplayOrder
            // 
            lblDisplayOrder.Dock = DockStyle.Fill;
            lblDisplayOrder.Font = new Font("Segoe UI", 12F);
            lblDisplayOrder.Location = new Point(1216, 60);
            lblDisplayOrder.Name = "lblDisplayOrder";
            lblDisplayOrder.Size = new Size(206, 50);
            lblDisplayOrder.TabIndex = 4;
            lblDisplayOrder.Text = "ترتيب العرض ";
            lblDisplayOrder.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPriorityNameAr
            // 
            lblPriorityNameAr.Dock = DockStyle.Fill;
            lblPriorityNameAr.Font = new Font("Segoe UI", 12F);
            lblPriorityNameAr.Location = new Point(509, 10);
            lblPriorityNameAr.Name = "lblPriorityNameAr";
            lblPriorityNameAr.Size = new Size(206, 50);
            lblPriorityNameAr.TabIndex = 2;
            lblPriorityNameAr.Text = "اسم الأولويه بالعربي";
            lblPriorityNameAr.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPriorityName
            // 
            txtPriorityName.Dock = DockStyle.Fill;
            txtPriorityName.Location = new Point(15, 15);
            txtPriorityName.Margin = new Padding(5);
            txtPriorityName.Name = "txtPriorityName";
            txtPriorityName.Size = new Size(486, 27);
            txtPriorityName.TabIndex = 3;
            // 
            // lblPriorityCode
            // 
            lblPriorityCode.Dock = DockStyle.Fill;
            lblPriorityCode.Font = new Font("Segoe UI", 12F);
            lblPriorityCode.Location = new Point(1216, 10);
            lblPriorityCode.Name = "lblPriorityCode";
            lblPriorityCode.Size = new Size(206, 50);
            lblPriorityCode.TabIndex = 0;
            lblPriorityCode.Text = "كود الأولوية";
            lblPriorityCode.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPriorityCode
            // 
            txtPriorityCode.Dock = DockStyle.Left;
            txtPriorityCode.Location = new Point(928, 15);
            txtPriorityCode.Margin = new Padding(5);
            txtPriorityCode.Name = "txtPriorityCode";
            txtPriorityCode.Size = new Size(280, 27);
            txtPriorityCode.TabIndex = 1;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Location = new Point(653, 163);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(62, 24);
            chkIsActive.TabIndex = 1;
            chkIsActive.Text = "نشط";
            chkIsActive.UseVisualStyleBackColor = true;
            // 
            // lblPrioritiesList
            // 
            lblPrioritiesList.Dock = DockStyle.Fill;
            lblPrioritiesList.Font = new Font("Segoe UI", 12F);
            lblPrioritiesList.Location = new Point(1216, 110);
            lblPrioritiesList.Name = "lblPrioritiesList";
            lblPrioritiesList.RightToLeft = RightToLeft.No;
            lblPrioritiesList.Size = new Size(206, 50);
            lblPrioritiesList.TabIndex = 33;
            lblPrioritiesList.Text = "قائمة أولويات الشحن";
            lblPrioritiesList.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(721, 113);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(489, 44);
            dataGridView1.TabIndex = 34;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // dgvShipmentPriorities
            // 
            AccessibleName = "أولويات الشحن";
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 224, 224);
            Controls.Add(panel1);
            Controls.Add(tlpAuditInfo);
            Controls.Add(pnlHeader);
            Name = "dgvShipmentPriorities";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1435, 800);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            tlpAuditInfo.ResumeLayout(false);
            panel1.ResumeLayout(false);
            dgvShipmentTypes.ResumeLayout(false);
            dgvShipmentTypes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudDefaultSurcharge).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDisplayOrder).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
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
        private TableLayoutPanel dgvShipmentTypes;
        private Label DataGridView;
        private Label lblPriorityNameEn;
        private TextBox txtPriorityNameEn;
        private Label lblDisplayOrder;
        private Label lblPriorityNameAr;
        private TextBox txtPriorityName;
        private Label lblPriorityCode;
        private TextBox txtPriorityCode;
        private CheckBox chkIsActive;
        private NumericUpDown nudDefaultSurcharge;
        private NumericUpDown nudDisplayOrder;
        private Label lblDefaultSurcharge;
        private ContextMenuStrip contextMenuStrip1;
        private Label lblPrioritiesList;
        private DataGridView dataGridView1;
        private TextBox textDescription;
        private Label lblDescription;
        private Button btnFirst;
        private Button btnPrevious;
        private TextBox txtCurrentRecordNo;
        private Button btnNext;
        private Button btnLast;
        private Button btnUndo;
      //  private TextBox txtPriorityName;
        private Label label1;
    }
}
