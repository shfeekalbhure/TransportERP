namespace TransportERP.Desktop.البوالص_والشحن.الاعدادات
{
    partial class UcShipmentPricingRules
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
            pnlHeader.BackColor = Color.FromArgb(192, 192, 255);
            pnlHeader.Controls.Add(pnlToolbar);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1200, 53);
            pnlHeader.TabIndex = 6;
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
            pnlToolbar.Location = new Point(252, 0);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Padding = new Padding(5);
            pnlToolbar.Size = new Size(948, 53);
            pnlToolbar.TabIndex = 2;
            pnlToolbar.WrapContents = false;
            // 
            // btnNew
            // 
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Microsoft Sans Serif", 10F);
            btnNew.Location = new Point(844, 9);
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
            btnSave.Location = new Point(746, 9);
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
            btnEdit.Location = new Point(648, 9);
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
            btnDelete.Location = new Point(550, 9);
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
            btnRefresh.Location = new Point(452, 9);
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
            btnClose.Location = new Point(354, 9);
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
            btnFirst.Location = new Point(256, 9);
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
            btnPrevious.Location = new Point(158, 9);
            btnPrevious.Margin = new Padding(4);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(90, 38);
            btnPrevious.TabIndex = 7;
            btnPrevious.Text = "السابق";
            btnPrevious.UseVisualStyleBackColor = true;
            // 
            // txtCurrentRecordNo
            // 
            txtCurrentRecordNo.Location = new Point(87, 10);
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
            btnNext.Location = new Point(-12, 9);
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
            btnLast.Location = new Point(-110, 9);
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
            btnUndo.Location = new Point(-208, 9);
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
            tlpAuditInfo.Name = "tlpAuditInfo";
            tlpAuditInfo.RowCount = 1;
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAuditInfo.Size = new Size(1200, 33);
            tlpAuditInfo.TabIndex = 7;
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
            pnlContent.TabIndex = 8;
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
            tabMain.Size = new Size(1200, 508);
            tabMain.TabIndex = 0;
            tabMain.SelectedIndexChanged += tabMain_SelectedIndexChanged;
            // 
            // tabPricingRules
            // 
            tabPricingRules.Controls.Add(tlpPricingRuleFields);
            tabPricingRules.Location = new Point(4, 32);
            tabPricingRules.Name = "tabPricingRules";
            tabPricingRules.Size = new Size(1192, 472);
            tabPricingRules.TabIndex = 2;
            tabPricingRules.Text = "قواعد التسعير";
            tabPricingRules.UseVisualStyleBackColor = true;
            // 
            // tlpPricingRuleFields
            // 
            tlpPricingRuleFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpPricingRuleFields.BackColor = Color.WhiteSmoke;
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
            tlpPricingRuleFields.Padding = new Padding(10);
            tlpPricingRuleFields.RowCount = 9;
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpPricingRuleFields.Size = new Size(1192, 365);
            tlpPricingRuleFields.TabIndex = 11;
            // 
            // label6
            // 
            label6.Dock = DockStyle.Fill;
            label6.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label6.Location = new Point(13, 362);
            label6.Name = "label6";
            label6.Size = new Size(406, 20);
            label6.TabIndex = 104;
            label6.Text = "الأولوية";
            label6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label5.Location = new Point(425, 362);
            label5.Name = "label5";
            label5.Size = new Size(169, 20);
            label5.TabIndex = 103;
            label5.Text = "الأولوية";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label4.Location = new Point(600, 362);
            label4.Name = "label4";
            label4.Size = new Size(404, 20);
            label4.TabIndex = 102;
            label4.Text = "الأولوية";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label3.Location = new Point(1010, 362);
            label3.Name = "label3";
            label3.Size = new Size(169, 20);
            label3.TabIndex = 101;
            label3.Text = "الأولوية";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label1.Location = new Point(425, 10);
            label1.Name = "label1";
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
            cmbCurrency.Name = "cmbCurrency";
            cmbCurrency.Size = new Size(404, 31);
            cmbCurrency.TabIndex = 98;
            // 
            // txtNotes
            // 
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Location = new Point(602, 323);
            txtNotes.Margin = new Padding(5);
            txtNotes.Name = "txtNotes";
            txtNotes.Size = new Size(400, 30);
            txtNotes.TabIndex = 97;
            // 
            // lblNotes
            // 
            lblNotes.Dock = DockStyle.Fill;
            lblNotes.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblNotes.Location = new Point(1010, 318);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new Size(169, 44);
            lblNotes.TabIndex = 96;
            lblNotes.Text = "ملاحظات";
            lblNotes.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudAdditionalFees
            // 
            nudAdditionalFees.Dock = DockStyle.Fill;
            nudAdditionalFees.Location = new Point(13, 277);
            nudAdditionalFees.Name = "nudAdditionalFees";
            nudAdditionalFees.Size = new Size(406, 30);
            nudAdditionalFees.TabIndex = 94;
            // 
            // lblAdditionalFees
            // 
            lblAdditionalFees.Dock = DockStyle.Fill;
            lblAdditionalFees.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblAdditionalFees.Location = new Point(425, 274);
            lblAdditionalFees.Name = "lblAdditionalFees";
            lblAdditionalFees.Size = new Size(169, 44);
            lblAdditionalFees.TabIndex = 93;
            lblAdditionalFees.Text = "رسوم اضافية";
            lblAdditionalFees.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudPrice
            // 
            nudPrice.Dock = DockStyle.Fill;
            nudPrice.Location = new Point(600, 277);
            nudPrice.Name = "nudPrice";
            nudPrice.Size = new Size(404, 30);
            nudPrice.TabIndex = 92;
            // 
            // lblPrice
            // 
            lblPrice.Dock = DockStyle.Fill;
            lblPrice.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblPrice.Location = new Point(1010, 274);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(169, 44);
            lblPrice.TabIndex = 91;
            lblPrice.Text = "السعر";
            lblPrice.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudMaximumValue
            // 
            nudMaximumValue.Dock = DockStyle.Fill;
            nudMaximumValue.Location = new Point(13, 233);
            nudMaximumValue.Name = "nudMaximumValue";
            nudMaximumValue.Size = new Size(406, 30);
            nudMaximumValue.TabIndex = 90;
            // 
            // lblMaximumValue
            // 
            lblMaximumValue.Dock = DockStyle.Fill;
            lblMaximumValue.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblMaximumValue.Location = new Point(425, 230);
            lblMaximumValue.Name = "lblMaximumValue";
            lblMaximumValue.Size = new Size(169, 44);
            lblMaximumValue.TabIndex = 89;
            lblMaximumValue.Text = "الحد الأعلى";
            lblMaximumValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudMinimumValue
            // 
            nudMinimumValue.Dock = DockStyle.Fill;
            nudMinimumValue.Location = new Point(600, 233);
            nudMinimumValue.Name = "nudMinimumValue";
            nudMinimumValue.Size = new Size(404, 30);
            nudMinimumValue.TabIndex = 88;
            // 
            // lblMinimumValue
            // 
            lblMinimumValue.Dock = DockStyle.Fill;
            lblMinimumValue.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblMinimumValue.Location = new Point(1010, 230);
            lblMinimumValue.Name = "lblMinimumValue";
            lblMinimumValue.Size = new Size(169, 44);
            lblMinimumValue.TabIndex = 87;
            lblMinimumValue.Text = "الحد الأدنى";
            lblMinimumValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEffectiveTo
            // 
            lblEffectiveTo.Dock = DockStyle.Fill;
            lblEffectiveTo.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblEffectiveTo.Location = new Point(425, 186);
            lblEffectiveTo.Name = "lblEffectiveTo";
            lblEffectiveTo.Size = new Size(169, 44);
            lblEffectiveTo.TabIndex = 85;
            lblEffectiveTo.Text = "ساري إلى";
            lblEffectiveTo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dtpEffectiveTo
            // 
            dtpEffectiveTo.Location = new Point(169, 189);
            dtpEffectiveTo.Name = "dtpEffectiveTo";
            dtpEffectiveTo.Size = new Size(250, 30);
            dtpEffectiveTo.TabIndex = 86;
            // 
            // lblEffectiveFrom
            // 
            lblEffectiveFrom.Dock = DockStyle.Fill;
            lblEffectiveFrom.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblEffectiveFrom.Location = new Point(1010, 186);
            lblEffectiveFrom.Name = "lblEffectiveFrom";
            lblEffectiveFrom.Size = new Size(169, 44);
            lblEffectiveFrom.TabIndex = 83;
            lblEffectiveFrom.Text = "ساري من";
            lblEffectiveFrom.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudDisplayOrder
            // 
            nudDisplayOrder.Dock = DockStyle.Fill;
            nudDisplayOrder.Location = new Point(13, 145);
            nudDisplayOrder.Name = "nudDisplayOrder";
            nudDisplayOrder.Size = new Size(406, 30);
            nudDisplayOrder.TabIndex = 82;
            // 
            // lblDisplayOrder
            // 
            lblDisplayOrder.Dock = DockStyle.Fill;
            lblDisplayOrder.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblDisplayOrder.Location = new Point(425, 142);
            lblDisplayOrder.Name = "lblDisplayOrder";
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
            cmbPricingBasis.Name = "cmbPricingBasis";
            cmbPricingBasis.Size = new Size(406, 31);
            cmbPricingBasis.TabIndex = 80;
            // 
            // cmbTransportMethod
            // 
            cmbTransportMethod.Dock = DockStyle.Fill;
            cmbTransportMethod.FormattingEnabled = true;
            cmbTransportMethod.Location = new Point(600, 101);
            cmbTransportMethod.Name = "cmbTransportMethod";
            cmbTransportMethod.Size = new Size(404, 31);
            cmbTransportMethod.TabIndex = 79;
            // 
            // cmbPriority
            // 
            cmbPriority.Dock = DockStyle.Fill;
            cmbPriority.FormattingEnabled = true;
            cmbPriority.Location = new Point(13, 57);
            cmbPriority.Name = "cmbPriority";
            cmbPriority.Size = new Size(406, 31);
            cmbPriority.TabIndex = 76;
            // 
            // txtPricingRuleName
            // 
            txtPricingRuleName.Dock = DockStyle.Fill;
            txtPricingRuleName.Location = new Point(15, 15);
            txtPricingRuleName.Margin = new Padding(5);
            txtPricingRuleName.Name = "txtPricingRuleName";
            txtPricingRuleName.Size = new Size(402, 30);
            txtPricingRuleName.TabIndex = 75;
            // 
            // lblCurrency
            // 
            lblCurrency.Dock = DockStyle.Fill;
            lblCurrency.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblCurrency.Location = new Point(1010, 142);
            lblCurrency.Name = "lblCurrency";
            lblCurrency.Size = new Size(169, 44);
            lblCurrency.TabIndex = 69;
            lblCurrency.Text = "العملة";
            lblCurrency.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPricingBasis
            // 
            lblPricingBasis.Dock = DockStyle.Fill;
            lblPricingBasis.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblPricingBasis.Location = new Point(425, 98);
            lblPricingBasis.Name = "lblPricingBasis";
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
            cmbShipmentType.Name = "cmbShipmentType";
            cmbShipmentType.Size = new Size(404, 31);
            cmbShipmentType.TabIndex = 62;
            // 
            // lblShipmentType
            // 
            lblShipmentType.Dock = DockStyle.Fill;
            lblShipmentType.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblShipmentType.Location = new Point(1010, 54);
            lblShipmentType.Name = "lblShipmentType";
            lblShipmentType.Size = new Size(169, 44);
            lblShipmentType.TabIndex = 59;
            lblShipmentType.Text = "نوع الشحن";
            lblShipmentType.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTransportMethod
            // 
            lblTransportMethod.Dock = DockStyle.Fill;
            lblTransportMethod.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblTransportMethod.Location = new Point(1010, 98);
            lblTransportMethod.Name = "lblTransportMethod";
            lblTransportMethod.Size = new Size(169, 44);
            lblTransportMethod.TabIndex = 53;
            lblTransportMethod.Text = "وسيلة النقل";
            lblTransportMethod.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPriority
            // 
            lblPriority.Dock = DockStyle.Fill;
            lblPriority.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblPriority.Location = new Point(425, 54);
            lblPriority.Name = "lblPriority";
            lblPriority.Size = new Size(169, 44);
            lblPriority.TabIndex = 6;
            lblPriority.Text = "الأولوية";
            lblPriority.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPricingRuleCode
            // 
            lblPricingRuleCode.Dock = DockStyle.Fill;
            lblPricingRuleCode.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblPricingRuleCode.Location = new Point(1010, 10);
            lblPricingRuleCode.Name = "lblPricingRuleCode";
            lblPricingRuleCode.Size = new Size(169, 44);
            lblPricingRuleCode.TabIndex = 0;
            lblPricingRuleCode.Text = "كود قاعدة التسعير";
            lblPricingRuleCode.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPricingRuleCode
            // 
            txtPricingRuleCode.Dock = DockStyle.Left;
            txtPricingRuleCode.Location = new Point(722, 15);
            txtPricingRuleCode.Margin = new Padding(5);
            txtPricingRuleCode.Name = "txtPricingRuleCode";
            txtPricingRuleCode.Size = new Size(280, 30);
            txtPricingRuleCode.TabIndex = 1;
            // 
            // dtpEffectiveFrom
            // 
            dtpEffectiveFrom.Location = new Point(754, 189);
            dtpEffectiveFrom.Name = "dtpEffectiveFrom";
            dtpEffectiveFrom.Size = new Size(250, 30);
            dtpEffectiveFrom.TabIndex = 84;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkIsActive.Location = new Point(349, 321);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(70, 29);
            chkIsActive.TabIndex = 95;
            chkIsActive.Text = "نشط";
            chkIsActive.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 32);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1192, 472);
            tabPage1.TabIndex = 3;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblPricingRuleName
            // 
            lblPricingRuleName.Dock = DockStyle.Fill;
            lblPricingRuleName.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblPricingRuleName.Location = new Point(425, 10);
            lblPricingRuleName.Name = "lblPricingRuleName";
            lblPricingRuleName.Size = new Size(169, 44);
            lblPricingRuleName.TabIndex = 2;
            lblPricingRuleName.Text = "اسم قاعدة التسعير";
            lblPricingRuleName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // UcShipmentPricingRules
            // 
            AccessibleName = "قواعد تسعير الشحن";
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(pnlContent);
            Controls.Add(tlpAuditInfo);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
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
