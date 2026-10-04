using System;
using System.Drawing;
using System.Windows.Forms;

namespace TransportERP.Desktop.البوالص_والشحن.الاعدادات
{
    partial class UcShipmentCategories
    {
        private System.ComponentModel.IContainer components = null;

        // 00 - تحرير الموارد عند إغلاق الشاشة.
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        // 00 - إنشاء عناصر الشاشة وضبط خصائصها ومواقعها وربط الأحداث.
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
            lblTitle = new Label();
            pnlContent = new Panel();
            dgvShipmentCategories = new DataGridView();
            colCategoryCode = new DataGridViewTextBoxColumn();
            colCategoryName = new DataGridViewTextBoxColumn();
            colCategoryNameEn = new DataGridViewTextBoxColumn();
            colIsFragile = new DataGridViewCheckBoxColumn();
            colIsValuable = new DataGridViewCheckBoxColumn();
            colIsPerishable = new DataGridViewCheckBoxColumn();
            colIsProhibited = new DataGridViewCheckBoxColumn();
            colIsActive = new DataGridViewCheckBoxColumn();
            tlpShipmentCategory = new TableLayoutPanel();
            lblCategoryCode = new Label();
            txtCategoryCode = new TextBox();
            lblCategoryName = new Label();
            txtCategoryName = new TextBox();
            lblDisplayOrder = new Label();
            nudDisplayOrder = new NumericUpDown();
            lblCategoryNameEn = new Label();
            txtCategoryNameEn = new TextBox();
            lblDescription = new Label();
            txtDescription = new TextBox();
            lblWarningMessage = new Label();
            txtWarningMessage = new TextBox();
            lblProperties = new Label();
            flpProperties = new FlowLayoutPanel();
            chkIsFragile = new CheckBox();
            chkIsImportant = new CheckBox();
            chkIsValuable = new CheckBox();
            chkIsPerishable = new CheckBox();
            chkIsProhibited = new CheckBox();
            chkRequiresInspection = new CheckBox();
            chkRequiresDeclaredValue = new CheckBox();
            chkRequiresCustoms = new CheckBox();
            chkIsActive = new CheckBox();
            lblCategoriesList = new Label();
            panel1 = new Panel();
            tlpAuditInfo = new TableLayoutPanel();
            lblPrintCount = new Label();
            lblLastPrintedAt = new Label();
            lblEditCount = new Label();
            lblModifiedAt = new Label();
            lblModifiedBy = new Label();
            lblCreatedAt = new Label();
            lblCreatedBy = new Label();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvShipmentCategories).BeginInit();
            tlpShipmentCategory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudDisplayOrder).BeginInit();
            flpProperties.SuspendLayout();
            panel1.SuspendLayout();
            tlpAuditInfo.SuspendLayout();
            SuspendLayout();
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
            pnlToolbar.Location = new Point(265, 0);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Padding = new Padding(5);
            pnlToolbar.Size = new Size(935, 53);
            pnlToolbar.TabIndex = 2;
            pnlToolbar.WrapContents = false;
            // 
            // btnNew
            // 
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Microsoft Sans Serif", 10F);
            btnNew.Location = new Point(831, 9);
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
            btnSave.Location = new Point(733, 9);
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
            btnEdit.Location = new Point(635, 9);
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
            btnDelete.Location = new Point(537, 9);
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
            btnRefresh.Location = new Point(439, 9);
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
            btnClose.Location = new Point(341, 9);
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
            lblTitle.Size = new Size(265, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "فئات وأصناف الشحن";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlContent
            // 
            pnlContent.Controls.Add(dgvShipmentCategories);
            pnlContent.Controls.Add(tlpShipmentCategory);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 53);
            pnlContent.MinimumSize = new Size(500, 650);
            pnlContent.Name = "pnlContent";
            pnlContent.Padding = new Padding(20);
            pnlContent.Size = new Size(1200, 1047);
            pnlContent.TabIndex = 5;
            // 
            // dgvShipmentCategories
            // 
            dgvShipmentCategories.AllowUserToAddRows = false;
            dgvShipmentCategories.AllowUserToDeleteRows = false;
            dgvShipmentCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvShipmentCategories.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvShipmentCategories.Columns.AddRange(new DataGridViewColumn[] { colCategoryCode, colCategoryName, colCategoryNameEn, colIsFragile, colIsValuable, colIsPerishable, colIsProhibited, colIsActive });
            dgvShipmentCategories.Dock = DockStyle.Fill;
            dgvShipmentCategories.Location = new Point(20, 385);
            dgvShipmentCategories.MultiSelect = false;
            dgvShipmentCategories.Name = "dgvShipmentCategories";
            dgvShipmentCategories.ReadOnly = true;
            dgvShipmentCategories.RowHeadersWidth = 51;
            dgvShipmentCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvShipmentCategories.Size = new Size(1160, 642);
            dgvShipmentCategories.TabIndex = 8;
            // 
            // colCategoryCode
            // 
            colCategoryCode.HeaderText = "الكود";
            colCategoryCode.MinimumWidth = 6;
            colCategoryCode.Name = "colCategoryCode";
            colCategoryCode.ReadOnly = true;
            // 
            // colCategoryName
            // 
            colCategoryName.HeaderText = "اسم الفئة";
            colCategoryName.MinimumWidth = 6;
            colCategoryName.Name = "colCategoryName";
            colCategoryName.ReadOnly = true;
            // 
            // colCategoryNameEn
            // 
            colCategoryNameEn.HeaderText = "الاسم بالإنجليزي";
            colCategoryNameEn.MinimumWidth = 6;
            colCategoryNameEn.Name = "colCategoryNameEn";
            colCategoryNameEn.ReadOnly = true;
            // 
            // colIsFragile
            // 
            colIsFragile.HeaderText = "قابل للكسر";
            colIsFragile.MinimumWidth = 6;
            colIsFragile.Name = "colIsFragile";
            colIsFragile.ReadOnly = true;
            // 
            // colIsValuable
            // 
            colIsValuable.HeaderText = "ثمين";
            colIsValuable.MinimumWidth = 6;
            colIsValuable.Name = "colIsValuable";
            colIsValuable.ReadOnly = true;
            // 
            // colIsPerishable
            // 
            colIsPerishable.HeaderText = "سريع التلف";
            colIsPerishable.MinimumWidth = 6;
            colIsPerishable.Name = "colIsPerishable";
            colIsPerishable.ReadOnly = true;
            // 
            // colIsProhibited
            // 
            colIsProhibited.HeaderText = "ممنوع";
            colIsProhibited.MinimumWidth = 6;
            colIsProhibited.Name = "colIsProhibited";
            colIsProhibited.ReadOnly = true;
            // 
            // colIsActive
            // 
            colIsActive.HeaderText = "نشط";
            colIsActive.MinimumWidth = 6;
            colIsActive.Name = "colIsActive";
            colIsActive.ReadOnly = true;
            // 
            // tlpShipmentCategory
            // 
            tlpShipmentCategory.ColumnCount = 4;
            tlpShipmentCategory.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tlpShipmentCategory.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tlpShipmentCategory.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tlpShipmentCategory.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tlpShipmentCategory.Controls.Add(lblCategoryCode, 0, 0);
            tlpShipmentCategory.Controls.Add(txtCategoryCode, 1, 0);
            tlpShipmentCategory.Controls.Add(lblCategoryName, 2, 0);
            tlpShipmentCategory.Controls.Add(txtCategoryName, 3, 0);
            tlpShipmentCategory.Controls.Add(lblDisplayOrder, 0, 1);
            tlpShipmentCategory.Controls.Add(nudDisplayOrder, 1, 1);
            tlpShipmentCategory.Controls.Add(lblCategoryNameEn, 2, 1);
            tlpShipmentCategory.Controls.Add(txtCategoryNameEn, 3, 1);
            tlpShipmentCategory.Controls.Add(lblDescription, 0, 2);
            tlpShipmentCategory.Controls.Add(txtDescription, 1, 2);
            tlpShipmentCategory.Controls.Add(lblWarningMessage, 0, 3);
            tlpShipmentCategory.Controls.Add(txtWarningMessage, 1, 3);
            tlpShipmentCategory.Controls.Add(lblProperties, 0, 4);
            tlpShipmentCategory.Controls.Add(flpProperties, 1, 4);
            tlpShipmentCategory.Controls.Add(lblCategoriesList, 0, 5);
            tlpShipmentCategory.Dock = DockStyle.Top;
            tlpShipmentCategory.Location = new Point(20, 20);
            tlpShipmentCategory.Margin = new Padding(0);
            tlpShipmentCategory.Name = "tlpShipmentCategory";
            tlpShipmentCategory.Padding = new Padding(10);
            tlpShipmentCategory.RowCount = 7;
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 220F));
            tlpShipmentCategory.Size = new Size(1160, 365);
            tlpShipmentCategory.TabIndex = 0;
            tlpShipmentCategory.Paint += tlpShipmentCategory_Paint;
            // 
            // lblCategoryCode
            // 
            lblCategoryCode.Dock = DockStyle.Fill;
            lblCategoryCode.Location = new Point(982, 10);
            lblCategoryCode.Name = "lblCategoryCode";
            lblCategoryCode.Size = new Size(165, 42);
            lblCategoryCode.TabIndex = 0;
            lblCategoryCode.Text = "كود الفئة";
            // 
            // txtCategoryCode
            // 
            txtCategoryCode.Dock = DockStyle.Left;
            txtCategoryCode.Location = new Point(804, 15);
            txtCategoryCode.Margin = new Padding(5);
            txtCategoryCode.Name = "txtCategoryCode";
            txtCategoryCode.Size = new Size(170, 30);
            txtCategoryCode.TabIndex = 0;
            // 
            // lblCategoryName
            // 
            lblCategoryName.Dock = DockStyle.Fill;
            lblCategoryName.Location = new Point(412, 10);
            lblCategoryName.Name = "lblCategoryName";
            lblCategoryName.Size = new Size(165, 42);
            lblCategoryName.TabIndex = 1;
            lblCategoryName.Text = "اسم الفئه";
            // 
            // txtCategoryName
            // 
            txtCategoryName.Dock = DockStyle.Fill;
            txtCategoryName.Location = new Point(15, 15);
            txtCategoryName.Margin = new Padding(5);
            txtCategoryName.Name = "txtCategoryName";
            txtCategoryName.Size = new Size(389, 30);
            txtCategoryName.TabIndex = 1;
            // 
            // lblDisplayOrder
            // 
            lblDisplayOrder.Dock = DockStyle.Fill;
            lblDisplayOrder.Location = new Point(982, 52);
            lblDisplayOrder.Name = "lblDisplayOrder";
            lblDisplayOrder.Size = new Size(165, 42);
            lblDisplayOrder.TabIndex = 2;
            lblDisplayOrder.Text = "ترتيب العرض";
            // 
            // nudDisplayOrder
            // 
            nudDisplayOrder.Location = new Point(585, 57);
            nudDisplayOrder.Margin = new Padding(5);
            nudDisplayOrder.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            nudDisplayOrder.Name = "nudDisplayOrder";
            nudDisplayOrder.Size = new Size(389, 30);
            nudDisplayOrder.TabIndex = 2;
            // 
            // lblCategoryNameEn
            // 
            lblCategoryNameEn.Dock = DockStyle.Fill;
            lblCategoryNameEn.Location = new Point(412, 52);
            lblCategoryNameEn.Name = "lblCategoryNameEn";
            lblCategoryNameEn.Size = new Size(165, 42);
            lblCategoryNameEn.TabIndex = 3;
            lblCategoryNameEn.Text = "اسم الفئة انجليزي";
            // 
            // txtCategoryNameEn
            // 
            txtCategoryNameEn.Dock = DockStyle.Fill;
            txtCategoryNameEn.Location = new Point(15, 57);
            txtCategoryNameEn.Margin = new Padding(5);
            txtCategoryNameEn.Name = "txtCategoryNameEn";
            txtCategoryNameEn.RightToLeft = RightToLeft.No;
            txtCategoryNameEn.Size = new Size(389, 30);
            txtCategoryNameEn.TabIndex = 3;
            // 
            // lblDescription
            // 
            lblDescription.Dock = DockStyle.Fill;
            lblDescription.Location = new Point(982, 94);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(165, 72);
            lblDescription.TabIndex = 4;
            lblDescription.Text = "الوصف";
            // 
            // txtDescription
            // 
            tlpShipmentCategory.SetColumnSpan(txtDescription, 3);
            txtDescription.Dock = DockStyle.Fill;
            txtDescription.Location = new Point(15, 99);
            txtDescription.Margin = new Padding(5);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.ScrollBars = ScrollBars.Vertical;
            txtDescription.Size = new Size(959, 62);
            txtDescription.TabIndex = 4;
            // 
            // lblWarningMessage
            // 
            lblWarningMessage.Dock = DockStyle.Fill;
            lblWarningMessage.Location = new Point(982, 166);
            lblWarningMessage.Name = "lblWarningMessage";
            lblWarningMessage.Size = new Size(165, 72);
            lblWarningMessage.TabIndex = 5;
            lblWarningMessage.Text = "رسالة التحذير";
            // 
            // txtWarningMessage
            // 
            tlpShipmentCategory.SetColumnSpan(txtWarningMessage, 3);
            txtWarningMessage.Dock = DockStyle.Fill;
            txtWarningMessage.Location = new Point(15, 171);
            txtWarningMessage.Margin = new Padding(5);
            txtWarningMessage.Multiline = true;
            txtWarningMessage.Name = "txtWarningMessage";
            txtWarningMessage.ScrollBars = ScrollBars.Vertical;
            txtWarningMessage.Size = new Size(959, 62);
            txtWarningMessage.TabIndex = 5;
            // 
            // lblProperties
            // 
            lblProperties.Dock = DockStyle.Fill;
            lblProperties.Location = new Point(982, 238);
            lblProperties.Name = "lblProperties";
            lblProperties.Size = new Size(165, 92);
            lblProperties.TabIndex = 6;
            // 
            // flpProperties
            // 
            flpProperties.AutoScroll = true;
            tlpShipmentCategory.SetColumnSpan(flpProperties, 3);
            flpProperties.Controls.Add(chkIsFragile);
            flpProperties.Controls.Add(chkIsImportant);
            flpProperties.Controls.Add(chkIsValuable);
            flpProperties.Controls.Add(chkIsPerishable);
            flpProperties.Controls.Add(chkIsProhibited);
            flpProperties.Controls.Add(chkRequiresInspection);
            flpProperties.Controls.Add(chkRequiresDeclaredValue);
            flpProperties.Controls.Add(chkRequiresCustoms);
            flpProperties.Controls.Add(chkIsActive);
            flpProperties.Dock = DockStyle.Fill;
            flpProperties.Location = new Point(15, 243);
            flpProperties.Margin = new Padding(5);
            flpProperties.Name = "flpProperties";
            flpProperties.Padding = new Padding(5);
            flpProperties.RightToLeft = RightToLeft.Yes;
            flpProperties.Size = new Size(959, 82);
            flpProperties.TabIndex = 6;
            // 
            // chkIsFragile
            // 
            chkIsFragile.Location = new Point(823, 8);
            chkIsFragile.Name = "chkIsFragile";
            chkIsFragile.Size = new Size(123, 30);
            chkIsFragile.TabIndex = 0;
            chkIsFragile.Text = "قابل للكسر";
            // 
            // chkIsImportant
            // 
            chkIsImportant.Location = new Point(731, 8);
            chkIsImportant.Name = "chkIsImportant";
            chkIsImportant.Size = new Size(86, 30);
            chkIsImportant.TabIndex = 1;
            chkIsImportant.Text = "مهم";
            // 
            // chkIsValuable
            // 
            chkIsValuable.Location = new Point(621, 8);
            chkIsValuable.Name = "chkIsValuable";
            chkIsValuable.Size = new Size(104, 30);
            chkIsValuable.TabIndex = 2;
            chkIsValuable.Text = "ثمين";
            // 
            // chkIsPerishable
            // 
            chkIsPerishable.Location = new Point(494, 8);
            chkIsPerishable.Name = "chkIsPerishable";
            chkIsPerishable.Size = new Size(121, 30);
            chkIsPerishable.TabIndex = 3;
            chkIsPerishable.Text = "قابل للتلف";
            // 
            // chkIsProhibited
            // 
            chkIsProhibited.Location = new Point(384, 8);
            chkIsProhibited.Name = "chkIsProhibited";
            chkIsProhibited.Size = new Size(104, 30);
            chkIsProhibited.TabIndex = 4;
            chkIsProhibited.Text = "ممنوع";
            // 
            // chkRequiresInspection
            // 
            chkRequiresInspection.Location = new Point(247, 8);
            chkRequiresInspection.Name = "chkRequiresInspection";
            chkRequiresInspection.Size = new Size(131, 30);
            chkRequiresInspection.TabIndex = 5;
            chkRequiresInspection.Text = "يتطلب تغليف";
            // 
            // chkRequiresDeclaredValue
            // 
            chkRequiresDeclaredValue.Location = new Point(137, 8);
            chkRequiresDeclaredValue.Name = "chkRequiresDeclaredValue";
            chkRequiresDeclaredValue.Size = new Size(104, 30);
            chkRequiresDeclaredValue.TabIndex = 6;
            chkRequiresDeclaredValue.Text = "جمارك";
            // 
            // chkRequiresCustoms
            // 
            chkRequiresCustoms.Location = new Point(815, 44);
            chkRequiresCustoms.Name = "chkRequiresCustoms";
            chkRequiresCustoms.Size = new Size(131, 30);
            chkRequiresCustoms.TabIndex = 7;
            chkRequiresCustoms.Text = "يتطلب رسوم";
            // 
            // chkIsActive
            // 
            chkIsActive.Checked = true;
            chkIsActive.CheckState = CheckState.Checked;
            chkIsActive.Location = new Point(705, 44);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(104, 30);
            chkIsActive.TabIndex = 8;
            chkIsActive.Text = "نشط";
            // 
            // lblCategoriesList
            // 
            tlpShipmentCategory.SetColumnSpan(lblCategoriesList, 4);
            lblCategoriesList.Dock = DockStyle.Fill;
            lblCategoriesList.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCategoriesList.Location = new Point(13, 330);
            lblCategoriesList.Name = "lblCategoriesList";
            lblCategoriesList.Size = new Size(1134, 36);
            lblCategoriesList.TabIndex = 7;
            lblCategoriesList.Text = "قائمة فئات وأصناف الشحن";
            lblCategoriesList.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            panel1.Controls.Add(tlpAuditInfo);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 1067);
            panel1.Name = "panel1";
            panel1.Size = new Size(1200, 33);
            panel1.TabIndex = 6;
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
            tlpAuditInfo.Location = new Point(0, 0);
            tlpAuditInfo.Margin = new Padding(0);
            tlpAuditInfo.Name = "tlpAuditInfo";
            tlpAuditInfo.RowCount = 1;
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAuditInfo.Size = new Size(1200, 33);
            tlpAuditInfo.TabIndex = 0;
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
            // UcShipmentCategories
            // 
            AccessibleName = "فئات وأصناف الشحن";
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(panel1);
            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 10F);
            Margin = new Padding(0);
            MinimumSize = new Size(650, 1100);
            Name = "UcShipmentCategories";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1200, 1100);
            Load += UcShipmentCategories_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            pnlContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvShipmentCategories).EndInit();
            tlpShipmentCategory.ResumeLayout(false);
            tlpShipmentCategory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudDisplayOrder).EndInit();
            flpProperties.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tlpAuditInfo.ResumeLayout(false);
            ResumeLayout(false);
        }

        // 08 - توحيد خصائص أزرار الشاشة لتكون بنفس شكل شاشة أنواع الشحن.
        private static void SetupButton(Button button, string text, int tabIndex)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.Font = new Font("Microsoft Sans Serif", 10F);
            button.Margin = new Padding(4);
            button.Size = new Size(90, 38);
            button.TabIndex = tabIndex;
            button.Text = text;
            button.UseVisualStyleBackColor = true;
        }

        // 09 - توحيد خصائص عناوين الحقول: الخط، المحاذاة، والتمدد.
        private static void SetupFieldLabel(Label label, string text)
        {
            label.Dock = DockStyle.Fill;
            label.Font = new Font("Segoe UI", 12F);
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleLeft;
        }

        // 10 - توحيد خصائص مربعات اختيار خصائص الصنف.
        private static void SetupCheckBox(CheckBox checkBox, string text)
        {
            checkBox.AutoSize = true;
            checkBox.Margin = new Padding(10, 7, 10, 7);
            checkBox.Text = text;
            checkBox.UseVisualStyleBackColor = true;
        }

        // 11 - توحيد خصائص بيانات الإنشاء والتعديل والطباعة.
        private static void SetupAuditLabel(Label label, string text)
        {
            label.BackColor = Color.FromArgb(192, 255, 255);
            label.Dock = DockStyle.Fill;
            label.Font = new Font("Microsoft Sans Serif", 9F);
            label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleRight;
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;

        private Panel pnlContent;
        private TableLayoutPanel tlpShipmentCategory;

        private Label lblCategoryCode;
        private TextBox txtCategoryCode;
        private Label lblCategoryName;
        private TextBox txtCategoryName;

        private Label lblDisplayOrder;
        private NumericUpDown nudDisplayOrder;
        private Label lblCategoryNameEn;
        private TextBox txtCategoryNameEn;

        private Label lblDescription;
        private TextBox txtDescription;
        private Label lblWarningMessage;
        private TextBox txtWarningMessage;

        private Label lblProperties;
        private FlowLayoutPanel flpProperties;
        private CheckBox chkIsFragile;
        private CheckBox chkIsImportant;
        private CheckBox chkIsValuable;
        private CheckBox chkIsPerishable;
        private CheckBox chkIsProhibited;
        private CheckBox chkRequiresInspection;
        private CheckBox chkRequiresDeclaredValue;
        private CheckBox chkRequiresCustoms;
        private FlowLayoutPanel pnlToolbar;
        private Button btnNew;
        private Button btnSave;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private Button btnClose;
        private Panel panel1;
        private TableLayoutPanel tlpAuditInfo;
        private Label lblPrintCount;
        private Label lblLastPrintedAt;
        private Label lblEditCount;
        private Label lblModifiedAt;
        private Label lblModifiedBy;
        private Label lblCreatedAt;
        private Label lblCreatedBy;
        private DataGridView dgvShipmentCategories;
        private DataGridViewTextBoxColumn colCategoryCode;
        private DataGridViewTextBoxColumn colCategoryName;
        private DataGridViewTextBoxColumn colCategoryNameEn;
        private DataGridViewCheckBoxColumn colIsFragile;
        private DataGridViewCheckBoxColumn colIsValuable;
        private DataGridViewCheckBoxColumn colIsPerishable;
        private DataGridViewCheckBoxColumn colIsProhibited;
        private DataGridViewCheckBoxColumn colIsActive;
        private Label lblCategoriesList;
        private CheckBox chkIsActive;
    }
}
