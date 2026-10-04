namespace TransportERP.Desktop.البوالص_والشحن.الاعدادات
{
    partial class UcPackagingTypes
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
            pnlHeader.BackColor = Color.FromArgb(192, 192, 255);
            pnlHeader.Controls.Add(pnlToolbar);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1346, 53);
            pnlHeader.TabIndex = 9;
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
            pnlToolbar.Location = new Point(176, 0);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Padding = new Padding(5);
            pnlToolbar.Size = new Size(1170, 53);
            pnlToolbar.TabIndex = 2;
            pnlToolbar.WrapContents = false;
            // 
            // btnNew
            // 
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Microsoft Sans Serif", 10F);
            btnNew.Location = new Point(1066, 9);
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
            btnSave.Location = new Point(968, 9);
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
            btnEdit.Location = new Point(870, 9);
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
            btnDelete.Location = new Point(772, 9);
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
            btnRefresh.Location = new Point(674, 9);
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
            btnClose.Location = new Point(576, 9);
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
            btnFirst.Location = new Point(478, 9);
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
            btnPrevious.Location = new Point(380, 9);
            btnPrevious.Margin = new Padding(4);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(90, 38);
            btnPrevious.TabIndex = 7;
            btnPrevious.Text = "السابق";
            btnPrevious.UseVisualStyleBackColor = true;
            // 
            // txtCurrentRecordNo
            // 
            txtCurrentRecordNo.Location = new Point(309, 10);
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
            btnNext.Location = new Point(210, 9);
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
            btnLast.Location = new Point(112, 9);
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
            btnUndo.Location = new Point(14, 9);
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
            lblTitle.Size = new Size(176, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "انواع التغليف";
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
            tlpAuditInfo.Location = new Point(0, 783);
            tlpAuditInfo.Margin = new Padding(0);
            tlpAuditInfo.Name = "tlpAuditInfo";
            tlpAuditInfo.RowCount = 1;
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpAuditInfo.Size = new Size(1346, 33);
            tlpAuditInfo.TabIndex = 10;
            // 
            // lblPrintCount
            // 
            lblPrintCount.BackColor = Color.FromArgb(192, 255, 255);
            lblPrintCount.Dock = DockStyle.Fill;
            lblPrintCount.Font = new Font("Microsoft Sans Serif", 9F);
            lblPrintCount.Location = new Point(3, 0);
            lblPrintCount.Name = "lblPrintCount";
            lblPrintCount.RightToLeft = RightToLeft.No;
            lblPrintCount.Size = new Size(158, 33);
            lblPrintCount.TabIndex = 8;
            lblPrintCount.Text = "عدد مرات الطباعة :- 00";
            lblPrintCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblLastPrintedAt
            // 
            lblLastPrintedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblLastPrintedAt.Dock = DockStyle.Fill;
            lblLastPrintedAt.Font = new Font("Microsoft Sans Serif", 9F);
            lblLastPrintedAt.Location = new Point(167, 0);
            lblLastPrintedAt.Name = "lblLastPrintedAt";
            lblLastPrintedAt.RightToLeft = RightToLeft.No;
            lblLastPrintedAt.Size = new Size(209, 33);
            lblLastPrintedAt.TabIndex = 5;
            lblLastPrintedAt.Text = "تاريخ اخر طباعة:  27/09/2026";
            lblLastPrintedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblEditCount
            // 
            lblEditCount.BackColor = Color.FromArgb(192, 255, 255);
            lblEditCount.Dock = DockStyle.Fill;
            lblEditCount.Font = new Font("Microsoft Sans Serif", 9F);
            lblEditCount.Location = new Point(382, 0);
            lblEditCount.Name = "lblEditCount";
            lblEditCount.RightToLeft = RightToLeft.No;
            lblEditCount.Size = new Size(155, 33);
            lblEditCount.TabIndex = 4;
            lblEditCount.Text = "عدد التعديلات :- 00";
            lblEditCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedAt
            // 
            lblModifiedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedAt.Dock = DockStyle.Fill;
            lblModifiedAt.Font = new Font("Microsoft Sans Serif", 9F);
            lblModifiedAt.Location = new Point(543, 0);
            lblModifiedAt.Name = "lblModifiedAt";
            lblModifiedAt.RightToLeft = RightToLeft.No;
            lblModifiedAt.Size = new Size(209, 33);
            lblModifiedAt.TabIndex = 3;
            lblModifiedAt.Text = " تاريخ التعديل:  27/09/2026";
            lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedBy
            // 
            lblModifiedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedBy.Dock = DockStyle.Fill;
            lblModifiedBy.Font = new Font("Microsoft Sans Serif", 9F);
            lblModifiedBy.Location = new Point(758, 0);
            lblModifiedBy.Name = "lblModifiedBy";
            lblModifiedBy.RightToLeft = RightToLeft.No;
            lblModifiedBy.Size = new Size(182, 33);
            lblModifiedBy.TabIndex = 2;
            lblModifiedBy.Text = "عدل بواسطة: - مدير النظام";
            lblModifiedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedAt
            // 
            lblCreatedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedAt.Dock = DockStyle.Fill;
            lblCreatedAt.Font = new Font("Microsoft Sans Serif", 9F);
            lblCreatedAt.Location = new Point(946, 0);
            lblCreatedAt.Name = "lblCreatedAt";
            lblCreatedAt.RightToLeft = RightToLeft.No;
            lblCreatedAt.Size = new Size(209, 33);
            lblCreatedAt.TabIndex = 1;
            lblCreatedAt.Text = " تاريخ الانشاء:  27/09/2026";
            lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedBy
            // 
            lblCreatedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedBy.Dock = DockStyle.Fill;
            lblCreatedBy.Font = new Font("Microsoft Sans Serif", 9F);
            lblCreatedBy.Location = new Point(1161, 0);
            lblCreatedBy.Name = "lblCreatedBy";
            lblCreatedBy.RightToLeft = RightToLeft.No;
            lblCreatedBy.Size = new Size(182, 33);
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
            pnlContent.Size = new Size(1346, 308);
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
            tabMain.Size = new Size(1346, 508);
            tabMain.TabIndex = 0;
            // 
            // tabPricingRules
            // 
            tabPricingRules.Controls.Add(tlpPricingRuleFields);
            tabPricingRules.Location = new Point(4, 29);
            tabPricingRules.Name = "tabPricingRules";
            tabPricingRules.Size = new Size(1338, 475);
            tabPricingRules.TabIndex = 2;
            tabPricingRules.Text = "انواع التغليف";
            tabPricingRules.UseVisualStyleBackColor = true;
            // 
            // tlpPricingRuleFields
            // 
            tlpPricingRuleFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpPricingRuleFields.BackColor = Color.WhiteSmoke;
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
            tlpPricingRuleFields.Padding = new Padding(10);
            tlpPricingRuleFields.RowCount = 5;
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.Size = new Size(1338, 241);
            tlpPricingRuleFields.TabIndex = 11;
            // 
            // txtNotes
            // 
            txtNotes.Dock = DockStyle.Left;
            txtNotes.Location = new Point(895, 175);
            txtNotes.Margin = new Padding(5);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.Size = new Size(282, 51);
            txtNotes.TabIndex = 156;
            // 
            // label6
            // 
            label6.Dock = DockStyle.Fill;
            label6.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label6.Location = new Point(1185, 170);
            label6.Name = "label6";
            label6.Size = new Size(140, 61);
            label6.TabIndex = 155;
            label6.Text = "ملاحظات";
            label6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // chkRequiresSpecialPackaging
            // 
            chkRequiresSpecialPackaging.AutoSize = true;
            chkRequiresSpecialPackaging.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkRequiresSpecialPackaging.Location = new Point(560, 93);
            chkRequiresSpecialPackaging.Name = "chkRequiresSpecialPackaging";
            chkRequiresSpecialPackaging.Size = new Size(181, 29);
            chkRequiresSpecialPackaging.TabIndex = 152;
            chkRequiresSpecialPackaging.Text = "يتطلب تغليف خاص";
            chkRequiresSpecialPackaging.UseVisualStyleBackColor = true;
            // 
            // nudDefaultPackageWeight
            // 
            nudDefaultPackageWeight.Dock = DockStyle.Fill;
            nudDefaultPackageWeight.Location = new Point(455, 53);
            nudDefaultPackageWeight.Name = "nudDefaultPackageWeight";
            nudDefaultPackageWeight.Size = new Size(286, 27);
            nudDefaultPackageWeight.TabIndex = 151;
            // 
            // label14
            // 
            label14.Dock = DockStyle.Fill;
            label14.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label14.Location = new Point(309, 170);
            label14.Name = "label14";
            label14.Size = new Size(140, 61);
            label14.TabIndex = 140;
            label14.Text = "الوصف";
            label14.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDescription
            // 
            txtDescription.Dock = DockStyle.Left;
            txtDescription.Location = new Point(21, 175);
            txtDescription.Margin = new Padding(5);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(280, 51);
            txtDescription.TabIndex = 134;
            // 
            // nudDisplayOrder
            // 
            nudDisplayOrder.Dock = DockStyle.Fill;
            nudDisplayOrder.Location = new Point(893, 93);
            nudDisplayOrder.Name = "nudDisplayOrder";
            nudDisplayOrder.Size = new Size(286, 27);
            nudDisplayOrder.TabIndex = 113;
            // 
            // nudDefaultPackagingCost
            // 
            nudDefaultPackagingCost.Dock = DockStyle.Fill;
            nudDefaultPackagingCost.Location = new Point(13, 53);
            nudDefaultPackagingCost.Name = "nudDefaultPackagingCost";
            nudDefaultPackagingCost.Size = new Size(290, 27);
            nudDefaultPackagingCost.TabIndex = 112;
            // 
            // txtPackagingTypeNameEn
            // 
            txtPackagingTypeNameEn.Dock = DockStyle.Fill;
            txtPackagingTypeNameEn.Location = new Point(15, 15);
            txtPackagingTypeNameEn.Margin = new Padding(5);
            txtPackagingTypeNameEn.Name = "txtPackagingTypeNameEn";
            txtPackagingTypeNameEn.Size = new Size(286, 27);
            txtPackagingTypeNameEn.TabIndex = 111;
            // 
            // label7
            // 
            label7.Dock = DockStyle.Fill;
            label7.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label7.Location = new Point(309, 50);
            label7.Name = "label7";
            label7.Size = new Size(140, 40);
            label7.TabIndex = 110;
            label7.Text = "تكلفة التغليف الافتراضي";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label2.Location = new Point(309, 10);
            label2.Name = "label2";
            label2.Size = new Size(140, 40);
            label2.TabIndex = 106;
            label2.Text = "اسم نوع التغليف انجليزي";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label1.Location = new Point(747, 10);
            label1.Name = "label1";
            label1.Size = new Size(140, 40);
            label1.TabIndex = 99;
            label1.Text = "اسم نوع التغليف عربي";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPackagingTypeNameAr
            // 
            txtPackagingTypeNameAr.Dock = DockStyle.Fill;
            txtPackagingTypeNameAr.Location = new Point(457, 15);
            txtPackagingTypeNameAr.Margin = new Padding(5);
            txtPackagingTypeNameAr.Name = "txtPackagingTypeNameAr";
            txtPackagingTypeNameAr.Size = new Size(282, 27);
            txtPackagingTypeNameAr.TabIndex = 75;
            // 
            // cmbPackagingCategory
            // 
            cmbPackagingCategory.Dock = DockStyle.Fill;
            cmbPackagingCategory.FormattingEnabled = true;
            cmbPackagingCategory.Location = new Point(893, 53);
            cmbPackagingCategory.Name = "cmbPackagingCategory";
            cmbPackagingCategory.Size = new Size(286, 28);
            cmbPackagingCategory.TabIndex = 62;
            // 
            // lblShipmentType
            // 
            lblShipmentType.Dock = DockStyle.Fill;
            lblShipmentType.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblShipmentType.Location = new Point(1185, 50);
            lblShipmentType.Name = "lblShipmentType";
            lblShipmentType.Size = new Size(140, 40);
            lblShipmentType.TabIndex = 59;
            lblShipmentType.Text = "فئة التغليف";
            lblShipmentType.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTransportMethod
            // 
            lblTransportMethod.Dock = DockStyle.Fill;
            lblTransportMethod.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblTransportMethod.Location = new Point(1185, 90);
            lblTransportMethod.Name = "lblTransportMethod";
            lblTransportMethod.Size = new Size(140, 40);
            lblTransportMethod.TabIndex = 53;
            lblTransportMethod.Text = "ترتيب العرض";
            lblTransportMethod.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPriority
            // 
            lblPriority.Dock = DockStyle.Fill;
            lblPriority.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblPriority.Location = new Point(747, 50);
            lblPriority.Name = "lblPriority";
            lblPriority.Size = new Size(140, 40);
            lblPriority.TabIndex = 6;
            lblPriority.Text = "وزن التغليف الافتراضي";
            lblPriority.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPricingRuleCode
            // 
            lblPricingRuleCode.Dock = DockStyle.Fill;
            lblPricingRuleCode.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblPricingRuleCode.Location = new Point(1185, 10);
            lblPricingRuleCode.Name = "lblPricingRuleCode";
            lblPricingRuleCode.Size = new Size(140, 40);
            lblPricingRuleCode.TabIndex = 0;
            lblPricingRuleCode.Text = "كود نوع التغليف";
            lblPricingRuleCode.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPackagingTypeCode
            // 
            txtPackagingTypeCode.Dock = DockStyle.Left;
            txtPackagingTypeCode.Location = new Point(897, 15);
            txtPackagingTypeCode.Margin = new Padding(5);
            txtPackagingTypeCode.Name = "txtPackagingTypeCode";
            txtPackagingTypeCode.Size = new Size(280, 27);
            txtPackagingTypeCode.TabIndex = 1;
            // 
            // chkIsReusable
            // 
            chkIsReusable.AutoSize = true;
            chkIsReusable.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkIsReusable.Location = new Point(109, 93);
            chkIsReusable.Name = "chkIsReusable";
            chkIsReusable.Size = new Size(194, 29);
            chkIsReusable.TabIndex = 145;
            chkIsReusable.Text = "قابل لأعادة الاستخدام";
            chkIsReusable.UseVisualStyleBackColor = true;
            // 
            // chkSuitableForFragile
            // 
            chkSuitableForFragile.AutoSize = true;
            chkSuitableForFragile.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkSuitableForFragile.Location = new Point(941, 133);
            chkSuitableForFragile.Name = "chkSuitableForFragile";
            chkSuitableForFragile.Size = new Size(238, 29);
            chkSuitableForFragile.TabIndex = 149;
            chkSuitableForFragile.Text = "مناسب للمواد القابله للكسر";
            chkSuitableForFragile.UseVisualStyleBackColor = true;
            // 
            // chkSuitableForLiquids
            // 
            chkSuitableForLiquids.AutoSize = true;
            chkSuitableForLiquids.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkSuitableForLiquids.Location = new Point(545, 133);
            chkSuitableForLiquids.Name = "chkSuitableForLiquids";
            chkSuitableForLiquids.Size = new Size(196, 29);
            chkSuitableForLiquids.TabIndex = 150;
            chkSuitableForLiquids.Text = "مناسب للمواد السائلة";
            chkSuitableForLiquids.UseVisualStyleBackColor = true;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkIsActive.Location = new Point(233, 133);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(70, 29);
            chkIsActive.TabIndex = 95;
            chkIsActive.Text = "نشط";
            chkIsActive.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1338, 475);
            tabPage1.TabIndex = 3;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // UcPackagingTypes
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlContent);
            Controls.Add(tlpAuditInfo);
            Controls.Add(pnlHeader);
            Name = "UcPackagingTypes";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1346, 816);
            Load += UcPackagingTypes_Load;
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
        private TextBox txtDescription;
        private NumericUpDown nudDisplayOrder;
        private NumericUpDown nudDefaultPackagingCost;
        private TextBox txtPackagingTypeNameEn;
        private Label label7;
        private Label label2;
        private Label label1;
        private TextBox txtPackagingTypeNameAr;
        private Label lblCurrency;
        private ComboBox cmbPackagingCategory;
        private Label lblShipmentType;
        private Label lblTransportMethod;
        private Label lblPriority;
        private Label lblPricingRuleCode;
        private TextBox txtPackagingTypeCode;
        private CheckBox chkIsActive;
        private CheckBox chkIsReusable;
        private CheckBox chkSuitableForFragile;
        private CheckBox chkSuitableForLiquids;
        private TabPage tabPage1;
        private NumericUpDown nudDefaultPackageWeight;
        private CheckBox chkRequiresSpecialPackaging;
        private TextBox txtNotes;
        private Label label6;
        private Label label14;
    }
}
