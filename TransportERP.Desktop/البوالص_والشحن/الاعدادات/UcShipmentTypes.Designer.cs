namespace TransportERP.Desktop.البوالص_والشحن.الاعدادات
{
    partial class UcShipmentTypes
    {
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
            DataGridView.Font = new Font("Segoe UI", 12F);
            DataGridView.Location = new Point(412, 90);
            DataGridView.Name = "DataGridView";
            DataGridView.Size = new Size(165, 40);
            DataGridView.TabIndex = 18;
            DataGridView.Text = "قائمة انواع الشحن";
            DataGridView.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            label12.Dock = DockStyle.Fill;
            label12.Font = new Font("Segoe UI", 12F);
            label12.Location = new Point(412, 130);
            label12.Name = "label12";
            label12.Size = new Size(165, 40);
            label12.TabIndex = 14;
            label12.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDescription
            // 
            lblDescription.Dock = DockStyle.Fill;
            lblDescription.Font = new Font("Segoe UI", 12F);
            lblDescription.Location = new Point(982, 90);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(165, 40);
            lblDescription.TabIndex = 8;
            lblDescription.Text = "الوصف";
            lblDescription.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textDescription
            // 
            textDescription.Dock = DockStyle.Fill;
            textDescription.Location = new Point(585, 95);
            textDescription.Margin = new Padding(5);
            textDescription.Name = "textDescription";
            textDescription.Size = new Size(389, 30);
            textDescription.TabIndex = 9;
            // 
            // lblShipmentTypeNameEn
            // 
            lblShipmentTypeNameEn.Dock = DockStyle.Fill;
            lblShipmentTypeNameEn.Font = new Font("Segoe UI", 12F);
            lblShipmentTypeNameEn.Location = new Point(412, 50);
            lblShipmentTypeNameEn.Name = "lblShipmentTypeNameEn";
            lblShipmentTypeNameEn.Size = new Size(165, 40);
            lblShipmentTypeNameEn.TabIndex = 6;
            lblShipmentTypeNameEn.Text = "اسم نوع الشحن انجليزي";
            lblShipmentTypeNameEn.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtShipmentTypeNameEn
            // 
            txtShipmentTypeNameEn.Dock = DockStyle.Fill;
            txtShipmentTypeNameEn.Location = new Point(15, 55);
            txtShipmentTypeNameEn.Margin = new Padding(5);
            txtShipmentTypeNameEn.Name = "txtShipmentTypeNameEn";
            txtShipmentTypeNameEn.Size = new Size(389, 30);
            txtShipmentTypeNameEn.TabIndex = 7;
            // 
            // lblDisplay0rder
            // 
            lblDisplay0rder.Dock = DockStyle.Fill;
            lblDisplay0rder.Font = new Font("Segoe UI", 12F);
            lblDisplay0rder.Location = new Point(982, 50);
            lblDisplay0rder.Name = "lblDisplay0rder";
            lblDisplay0rder.Size = new Size(165, 40);
            lblDisplay0rder.TabIndex = 4;
            lblDisplay0rder.Text = "ترتيب العرض";
            lblDisplay0rder.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblShipmentTypeName
            // 
            lblShipmentTypeName.Dock = DockStyle.Fill;
            lblShipmentTypeName.Font = new Font("Segoe UI", 12F);
            lblShipmentTypeName.Location = new Point(412, 10);
            lblShipmentTypeName.Name = "lblShipmentTypeName";
            lblShipmentTypeName.Size = new Size(165, 40);
            lblShipmentTypeName.TabIndex = 2;
            lblShipmentTypeName.Text = "اسم نوع الشحن";
            lblShipmentTypeName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtShipmentTypeName
            // 
            txtShipmentTypeName.Dock = DockStyle.Fill;
            txtShipmentTypeName.Location = new Point(15, 15);
            txtShipmentTypeName.Margin = new Padding(5);
            txtShipmentTypeName.Name = "txtShipmentTypeName";
            txtShipmentTypeName.Size = new Size(389, 30);
            txtShipmentTypeName.TabIndex = 3;
            // 
            // lblShipmentTypeCode
            // 
            lblShipmentTypeCode.Dock = DockStyle.Fill;
            lblShipmentTypeCode.Font = new Font("Segoe UI", 12F);
            lblShipmentTypeCode.Location = new Point(982, 10);
            lblShipmentTypeCode.Name = "lblShipmentTypeCode";
            lblShipmentTypeCode.Size = new Size(165, 40);
            lblShipmentTypeCode.TabIndex = 0;
            lblShipmentTypeCode.Text = "كود الشحن";
            lblShipmentTypeCode.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtShipmentTypeCode
            // 
            txtShipmentTypeCode.Dock = DockStyle.Left;
            txtShipmentTypeCode.Location = new Point(804, 15);
            txtShipmentTypeCode.Margin = new Padding(5);
            txtShipmentTypeCode.Name = "txtShipmentTypeCode";
            txtShipmentTypeCode.Size = new Size(170, 30);
            txtShipmentTypeCode.TabIndex = 1;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(13, 93);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(393, 34);
            dataGridView1.TabIndex = 17;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Location = new Point(1081, 133);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(66, 27);
            chkIsActive.TabIndex = 1;
            chkIsActive.Text = "نشط";
            chkIsActive.UseVisualStyleBackColor = true;
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
            dgvShipmentTypes.Name = "dgvShipmentTypes";
            dgvShipmentTypes.Padding = new Padding(10);
            dgvShipmentTypes.RowCount = 4;
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            dgvShipmentTypes.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            dgvShipmentTypes.Size = new Size(1160, 180);
            dgvShipmentTypes.TabIndex = 0;
            // 
            // nudDisplay0rder
            // 
            nudDisplay0rder.Location = new Point(585, 53);
            nudDisplay0rder.Name = "nudDisplay0rder";
            nudDisplay0rder.Size = new Size(391, 30);
            nudDisplay0rder.TabIndex = 19;
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
            // pnlContent
            // 
            pnlContent.Controls.Add(dgvShipmentTypes);
            pnlContent.Dock = DockStyle.Top;
            pnlContent.Location = new Point(0, 53);
            pnlContent.Name = "pnlContent";
            pnlContent.Padding = new Padding(20);
            pnlContent.Size = new Size(1200, 1008);
            pnlContent.TabIndex = 5;
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(pnlToolbar);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1200, 53);
            pnlHeader.TabIndex = 3;
            // 
            // pnlToolbar
            // 
            pnlToolbar.Controls.Add(btnNew);
            pnlToolbar.Controls.Add(btnSave);
            pnlToolbar.Controls.Add(btnEdit);
            pnlToolbar.Controls.Add(btnDelete);
            pnlToolbar.Controls.Add(btnRefresh);
            pnlToolbar.Controls.Add(btnClose);
            pnlToolbar.Dock = DockStyle.Fill;
            pnlToolbar.Location = new Point(230, 0);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Padding = new Padding(5);
            pnlToolbar.Size = new Size(970, 53);
            pnlToolbar.TabIndex = 1;
            pnlToolbar.WrapContents = false;
            // 
            // btnNew
            // 
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Microsoft Sans Serif", 10F);
            btnNew.Location = new Point(866, 9);
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
            btnSave.Location = new Point(768, 9);
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
            btnEdit.Location = new Point(670, 9);
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
            btnDelete.Location = new Point(572, 9);
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
            btnRefresh.Location = new Point(474, 9);
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
            btnClose.Location = new Point(376, 9);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(90, 38);
            btnClose.TabIndex = 5;
            btnClose.Text = "اغلاق";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Dock = DockStyle.Left;
            lblTitle.Font = new Font("Segoe UI", 16F);
            lblTitle.Location = new Point(0, 0);
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
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 1067);
            panel1.Name = "panel1";
            panel1.Size = new Size(1200, 33);
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
            tlpAuditInfo.Name = "tlpAuditInfo";
            tlpAuditInfo.RowCount = 1;
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAuditInfo.Size = new Size(1200, 33);
            tlpAuditInfo.TabIndex = 0;
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
            // UcShipmentTypes
            // 
            AccessibleName = "أنواع الشحن";
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
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