using System;
using System.Drawing;
using System.Windows.Forms;

namespace TransportERP.Desktop.البوالص_والشحن.الاعدادات
{
    partial class UcShipmentCategories
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
            pnlToolbar.Location = new Point(265, 0);
            pnlToolbar.Margin = new Padding(5);
            pnlToolbar.BackColor = Color.FromArgb(224, 224, 224);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.MinimumSize = new Size(0, 48);
            pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlToolbar.AutoSize = true;
            pnlToolbar.Padding = new Padding(5);
            pnlToolbar.Size = new Size(935, 53);
            pnlToolbar.TabIndex = 2;
            pnlToolbar.WrapContents = true;
            // 
            // btnNew
            // 
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Microsoft Sans Serif", 10F);
            btnNew.Location = new Point(831, 9);
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
            btnSave.Location = new Point(733, 9);
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
            btnEdit.Location = new Point(635, 9);
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
            btnDelete.Location = new Point(537, 9);
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
            btnRefresh.Location = new Point(439, 9);
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
            btnClose.Location = new Point(341, 9);
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
            pnlContent.MinimumSize = Size.Empty;
            pnlContent.AutoScrollMinSize = new Size(500, 650);
            pnlContent.BackColor = Color.LightCyan;
            pnlContent.Name = "pnlContent";
            pnlContent.AutoScroll = true;
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
            dgvShipmentCategories.BackgroundColor = Color.White;
            dgvShipmentCategories.GridColor = Color.FromArgb(224, 224, 224);
            dgvShipmentCategories.Font = new Font("Segoe UI", 10F);
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
            tlpShipmentCategory.BackColor = Color.LightCyan;
            tlpShipmentCategory.Name = "tlpShipmentCategory";
            tlpShipmentCategory.MinimumSize = new Size(0, 572);
            tlpShipmentCategory.Padding = new Padding(0);
            tlpShipmentCategory.RowCount = 7;
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpShipmentCategory.RowStyles.Add(new RowStyle(SizeType.Absolute, 220F));
            tlpShipmentCategory.Size = new Size(1160, 572);
            tlpShipmentCategory.TabIndex = 0;
            tlpShipmentCategory.Paint += tlpShipmentCategory_Paint;
            // 
            // lblCategoryCode
            // 
            lblCategoryCode.Dock = DockStyle.Fill;
            lblCategoryCode.Location = new Point(982, 10);
            lblCategoryCode.ForeColor = Color.FromArgb(16, 24, 40);
            lblCategoryCode.BackColor = Color.Transparent;
            lblCategoryCode.Name = "lblCategoryCode";
            lblCategoryCode.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCategoryCode.Margin = new Padding(3);
            lblCategoryCode.AutoEllipsis = true;
            lblCategoryCode.Size = new Size(165, 42);
            lblCategoryCode.TabIndex = 0;
            lblCategoryCode.Text = "كود الفئة";
            // 
            // txtCategoryCode
            // 
            txtCategoryCode.Dock = DockStyle.Fill;
            txtCategoryCode.Location = new Point(804, 15);
            txtCategoryCode.Margin = new Padding(3);
            txtCategoryCode.BackColor = Color.White;
            txtCategoryCode.ForeColor = Color.FromArgb(16, 24, 40);
            txtCategoryCode.Name = "txtCategoryCode";
            txtCategoryCode.Font = new Font("Segoe UI", 11F);
            txtCategoryCode.Size = new Size(170, 30);
            txtCategoryCode.TabIndex = 0;
            // 
            // lblCategoryName
            // 
            lblCategoryName.Dock = DockStyle.Fill;
            lblCategoryName.Location = new Point(412, 10);
            lblCategoryName.ForeColor = Color.FromArgb(16, 24, 40);
            lblCategoryName.BackColor = Color.Transparent;
            lblCategoryName.Name = "lblCategoryName";
            lblCategoryName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCategoryName.Margin = new Padding(3);
            lblCategoryName.AutoEllipsis = true;
            lblCategoryName.Size = new Size(165, 42);
            lblCategoryName.TabIndex = 1;
            lblCategoryName.Text = "اسم الفئه";
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
            txtCategoryName.Size = new Size(389, 30);
            txtCategoryName.TabIndex = 1;
            // 
            // lblDisplayOrder
            // 
            lblDisplayOrder.Dock = DockStyle.Fill;
            lblDisplayOrder.Location = new Point(982, 52);
            lblDisplayOrder.ForeColor = Color.FromArgb(16, 24, 40);
            lblDisplayOrder.BackColor = Color.Transparent;
            lblDisplayOrder.Name = "lblDisplayOrder";
            lblDisplayOrder.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDisplayOrder.Margin = new Padding(3);
            lblDisplayOrder.AutoEllipsis = true;
            lblDisplayOrder.Size = new Size(165, 42);
            lblDisplayOrder.TabIndex = 2;
            lblDisplayOrder.Text = "ترتيب العرض";
            // 
            // nudDisplayOrder
            // 
            nudDisplayOrder.Location = new Point(585, 57);
            nudDisplayOrder.Margin = new Padding(3);
            nudDisplayOrder.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            nudDisplayOrder.BackColor = Color.White;
            nudDisplayOrder.ForeColor = Color.FromArgb(16, 24, 40);
            nudDisplayOrder.Name = "nudDisplayOrder";
            nudDisplayOrder.Font = new Font("Segoe UI", 11F);
            nudDisplayOrder.Dock = DockStyle.Fill;
            nudDisplayOrder.Size = new Size(389, 30);
            nudDisplayOrder.TabIndex = 2;
            // 
            // lblCategoryNameEn
            // 
            lblCategoryNameEn.Dock = DockStyle.Fill;
            lblCategoryNameEn.Location = new Point(412, 52);
            lblCategoryNameEn.ForeColor = Color.FromArgb(16, 24, 40);
            lblCategoryNameEn.BackColor = Color.Transparent;
            lblCategoryNameEn.Name = "lblCategoryNameEn";
            lblCategoryNameEn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCategoryNameEn.Margin = new Padding(3);
            lblCategoryNameEn.AutoEllipsis = true;
            lblCategoryNameEn.Size = new Size(165, 42);
            lblCategoryNameEn.TabIndex = 3;
            lblCategoryNameEn.Text = "اسم الفئة انجليزي";
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
            txtCategoryNameEn.Size = new Size(389, 30);
            txtCategoryNameEn.TabIndex = 3;
            // 
            // lblDescription
            // 
            lblDescription.Dock = DockStyle.Fill;
            lblDescription.Location = new Point(982, 94);
            lblDescription.ForeColor = Color.FromArgb(16, 24, 40);
            lblDescription.BackColor = Color.Transparent;
            lblDescription.Name = "lblDescription";
            lblDescription.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDescription.Margin = new Padding(3);
            lblDescription.AutoEllipsis = true;
            lblDescription.Size = new Size(165, 72);
            lblDescription.TabIndex = 4;
            lblDescription.Text = "الوصف";
            // 
            // txtDescription
            // 
            tlpShipmentCategory.SetColumnSpan(txtDescription, 3);
            txtDescription.Dock = DockStyle.Fill;
            txtDescription.Location = new Point(15, 99);
            txtDescription.Margin = new Padding(3);
            txtDescription.Multiline = true;
            txtDescription.BackColor = Color.White;
            txtDescription.ForeColor = Color.FromArgb(16, 24, 40);
            txtDescription.Name = "txtDescription";
            txtDescription.Font = new Font("Segoe UI", 11F);
            txtDescription.ScrollBars = ScrollBars.Vertical;
            txtDescription.Size = new Size(959, 62);
            txtDescription.TabIndex = 4;
            // 
            // lblWarningMessage
            // 
            lblWarningMessage.Dock = DockStyle.Fill;
            lblWarningMessage.Location = new Point(982, 166);
            lblWarningMessage.ForeColor = Color.FromArgb(16, 24, 40);
            lblWarningMessage.BackColor = Color.Transparent;
            lblWarningMessage.Name = "lblWarningMessage";
            lblWarningMessage.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblWarningMessage.Margin = new Padding(3);
            lblWarningMessage.AutoEllipsis = true;
            lblWarningMessage.Size = new Size(165, 72);
            lblWarningMessage.TabIndex = 5;
            lblWarningMessage.Text = "رسالة التحذير";
            // 
            // txtWarningMessage
            // 
            tlpShipmentCategory.SetColumnSpan(txtWarningMessage, 3);
            txtWarningMessage.Dock = DockStyle.Fill;
            txtWarningMessage.Location = new Point(15, 171);
            txtWarningMessage.Margin = new Padding(3);
            txtWarningMessage.Multiline = true;
            txtWarningMessage.BackColor = Color.White;
            txtWarningMessage.ForeColor = Color.FromArgb(16, 24, 40);
            txtWarningMessage.Name = "txtWarningMessage";
            txtWarningMessage.Font = new Font("Segoe UI", 11F);
            txtWarningMessage.ScrollBars = ScrollBars.Vertical;
            txtWarningMessage.Size = new Size(959, 62);
            txtWarningMessage.TabIndex = 5;
            // 
            // lblProperties
            // 
            lblProperties.Dock = DockStyle.Fill;
            lblProperties.Location = new Point(982, 238);
            lblProperties.ForeColor = Color.FromArgb(16, 24, 40);
            lblProperties.BackColor = Color.Transparent;
            lblProperties.Name = "lblProperties";
            lblProperties.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblProperties.Margin = new Padding(3);
            lblProperties.AutoEllipsis = true;
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
            flpProperties.BackColor = Color.LightCyan;
            flpProperties.Name = "flpProperties";
            flpProperties.Padding = new Padding(5);
            flpProperties.RightToLeft = RightToLeft.Yes;
            flpProperties.Size = new Size(959, 82);
            flpProperties.TabIndex = 6;
            // 
            // chkIsFragile
            // 
            chkIsFragile.Location = new Point(823, 8);
            chkIsFragile.BackColor = Color.Transparent;
            chkIsFragile.ForeColor = Color.FromArgb(16, 24, 40);
            chkIsFragile.UseVisualStyleBackColor = false;
            chkIsFragile.Name = "chkIsFragile";
            chkIsFragile.Size = new Size(123, 30);
            chkIsFragile.TabIndex = 0;
            chkIsFragile.Text = "قابل للكسر";
            // 
            // chkIsImportant
            // 
            chkIsImportant.Location = new Point(731, 8);
            chkIsImportant.BackColor = Color.Transparent;
            chkIsImportant.ForeColor = Color.FromArgb(16, 24, 40);
            chkIsImportant.UseVisualStyleBackColor = false;
            chkIsImportant.Name = "chkIsImportant";
            chkIsImportant.Size = new Size(86, 30);
            chkIsImportant.TabIndex = 1;
            chkIsImportant.Text = "مهم";
            // 
            // chkIsValuable
            // 
            chkIsValuable.Location = new Point(621, 8);
            chkIsValuable.BackColor = Color.Transparent;
            chkIsValuable.ForeColor = Color.FromArgb(16, 24, 40);
            chkIsValuable.UseVisualStyleBackColor = false;
            chkIsValuable.Name = "chkIsValuable";
            chkIsValuable.Size = new Size(104, 30);
            chkIsValuable.TabIndex = 2;
            chkIsValuable.Text = "ثمين";
            // 
            // chkIsPerishable
            // 
            chkIsPerishable.Location = new Point(494, 8);
            chkIsPerishable.BackColor = Color.Transparent;
            chkIsPerishable.ForeColor = Color.FromArgb(16, 24, 40);
            chkIsPerishable.UseVisualStyleBackColor = false;
            chkIsPerishable.Name = "chkIsPerishable";
            chkIsPerishable.Size = new Size(121, 30);
            chkIsPerishable.TabIndex = 3;
            chkIsPerishable.Text = "قابل للتلف";
            // 
            // chkIsProhibited
            // 
            chkIsProhibited.Location = new Point(384, 8);
            chkIsProhibited.BackColor = Color.Transparent;
            chkIsProhibited.ForeColor = Color.FromArgb(16, 24, 40);
            chkIsProhibited.UseVisualStyleBackColor = false;
            chkIsProhibited.Name = "chkIsProhibited";
            chkIsProhibited.Size = new Size(104, 30);
            chkIsProhibited.TabIndex = 4;
            chkIsProhibited.Text = "ممنوع";
            // 
            // chkRequiresInspection
            // 
            chkRequiresInspection.Location = new Point(247, 8);
            chkRequiresInspection.BackColor = Color.Transparent;
            chkRequiresInspection.ForeColor = Color.FromArgb(16, 24, 40);
            chkRequiresInspection.UseVisualStyleBackColor = false;
            chkRequiresInspection.Name = "chkRequiresInspection";
            chkRequiresInspection.Size = new Size(131, 30);
            chkRequiresInspection.TabIndex = 5;
            chkRequiresInspection.Text = "يتطلب تغليف";
            // 
            // chkRequiresDeclaredValue
            // 
            chkRequiresDeclaredValue.Location = new Point(137, 8);
            chkRequiresDeclaredValue.BackColor = Color.Transparent;
            chkRequiresDeclaredValue.ForeColor = Color.FromArgb(16, 24, 40);
            chkRequiresDeclaredValue.UseVisualStyleBackColor = false;
            chkRequiresDeclaredValue.Name = "chkRequiresDeclaredValue";
            chkRequiresDeclaredValue.Size = new Size(104, 30);
            chkRequiresDeclaredValue.TabIndex = 6;
            chkRequiresDeclaredValue.Text = "جمارك";
            // 
            // chkRequiresCustoms
            // 
            chkRequiresCustoms.Location = new Point(815, 44);
            chkRequiresCustoms.BackColor = Color.Transparent;
            chkRequiresCustoms.ForeColor = Color.FromArgb(16, 24, 40);
            chkRequiresCustoms.UseVisualStyleBackColor = false;
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
            chkIsActive.BackColor = Color.Transparent;
            chkIsActive.ForeColor = Color.FromArgb(16, 24, 40);
            chkIsActive.UseVisualStyleBackColor = false;
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(104, 30);
            chkIsActive.TabIndex = 8;
            chkIsActive.Text = "نشط";
            // 
            // lblCategoriesList
            // 
            tlpShipmentCategory.SetColumnSpan(lblCategoriesList, 4);
            lblCategoriesList.Dock = DockStyle.Fill;
            lblCategoriesList.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCategoriesList.Location = new Point(13, 330);
            lblCategoriesList.ForeColor = Color.FromArgb(16, 24, 40);
            lblCategoriesList.BackColor = Color.Transparent;
            lblCategoriesList.Name = "lblCategoriesList";
            lblCategoriesList.Margin = new Padding(3);
            lblCategoriesList.AutoEllipsis = true;
            lblCategoriesList.Size = new Size(1134, 36);
            lblCategoriesList.TabIndex = 7;
            lblCategoriesList.Text = "قائمة فئات وأصناف الشحن";
            lblCategoriesList.TextAlign = ContentAlignment.MiddleRight;
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
            tlpAuditInfo.BackColor = Color.FromArgb(248, 250, 252);
            tlpAuditInfo.Name = "tlpAuditInfo";
            tlpAuditInfo.Padding = new Padding(8);
            tlpAuditInfo.RowCount = 1;
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAuditInfo.Size = new Size(1200, 48);
            tlpAuditInfo.TabIndex = 0;
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
            // UcShipmentCategories
            // 
            AccessibleName = "فئات وأصناف الشحن";
            AutoScaleDimensions = new SizeF(120F, 120F);
            BackColor = Color.FromArgb(248, 250, 252);
            ForeColor = Color.FromArgb(16, 24, 40);
            Padding = new Padding(0);
            AutoScaleMode = AutoScaleMode.Dpi;
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
        
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(265, 0);
        designerCommandBar.Size = new Size(935, 53);
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
