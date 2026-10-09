namespace TransportERP.Desktop.البوالص_والشحن.الاعدادات
{
    partial class UcPackagingTypes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcPackagingTypes));
            designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
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
            designerHiddenCommands = new FlowLayoutPanel();
            standardCommandView = new Button();
            standardCommandPrint = new Button();
            standardCommandImport = new Button();
            standardCommandExport = new Button();
            standardCommandHelp = new Button();
            pnlHeader = new Panel();
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
            standardAuditMetadata = new TransportERP.Desktop.CoreUI.AuditMetadataControl();
            designerCommandBar.SuspendLayout();
            pnlToolbar.SuspendLayout();
            designerHiddenCommands.SuspendLayout();
            pnlHeader.SuspendLayout();
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
            // designerCommandBar
            // 
            designerCommandBar.BackColor = Color.FromArgb(239, 239, 239);
            designerCommandBar.BorderStyle = BorderStyle.FixedSingle;
            designerCommandBar.Controls.Add(pnlToolbar);
            designerCommandBar.Controls.Add(designerHiddenCommands);
            designerCommandBar.Dock = DockStyle.Top;
            designerCommandBar.Location = new Point(176, 0);
            designerCommandBar.Margin = new Padding(5);
            designerCommandBar.MinimumSize = new Size(0, 48);
            designerCommandBar.Name = "designerCommandBar";
            designerCommandBar.Size = new Size(1170, 53);
            designerCommandBar.TabIndex = 2;
            // 
            // pnlToolbar
            // 
            pnlToolbar.AutoSize = true;
            pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
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
            pnlToolbar.Dock = DockStyle.Fill;
            pnlToolbar.Location = new Point(0, 0);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.MinimumSize = new Size(0, 48);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Padding = new Padding(5);
            pnlToolbar.Size = new Size(1168, 51);
            pnlToolbar.TabIndex = 2;
            // 
            // btnNew
            // 
            btnNew.BackColor = Color.FromArgb(224, 224, 224);
            designerCommandBar.SetCommandRole(btnNew, CoreUI.DesignerCommandRole.Add);
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Microsoft Sans Serif", 10F);
            btnNew.ForeColor = Color.FromArgb(16, 24, 40);
            btnNew.Location = new Point(1084, 9);
            btnNew.Margin = new Padding(4);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(70, 30);
            btnNew.TabIndex = 0;
            btnNew.Text = "جديد";
            btnNew.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(224, 224, 224);
            designerCommandBar.SetCommandRole(btnSave, CoreUI.DesignerCommandRole.Save);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Microsoft Sans Serif", 10F);
            btnSave.ForeColor = Color.FromArgb(16, 24, 40);
            btnSave.Location = new Point(1006, 9);
            btnSave.Margin = new Padding(4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(70, 30);
            btnSave.TabIndex = 1;
            btnSave.Text = "حفظ";
            btnSave.UseVisualStyleBackColor = false;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.FromArgb(224, 224, 224);
            designerCommandBar.SetCommandRole(btnEdit, CoreUI.DesignerCommandRole.Edit);
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Microsoft Sans Serif", 10F);
            btnEdit.ForeColor = Color.FromArgb(16, 24, 40);
            btnEdit.Location = new Point(928, 9);
            btnEdit.Margin = new Padding(4);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(70, 30);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "تعديل";
            btnEdit.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(224, 224, 224);
            designerCommandBar.SetCommandRole(btnDelete, CoreUI.DesignerCommandRole.Delete);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Microsoft Sans Serif", 10F);
            btnDelete.ForeColor = Color.FromArgb(16, 24, 40);
            btnDelete.Location = new Point(850, 9);
            btnDelete.Margin = new Padding(4);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(70, 30);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "حذف";
            btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(224, 224, 224);
            designerCommandBar.SetCommandRole(btnRefresh, CoreUI.DesignerCommandRole.Refresh);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Microsoft Sans Serif", 10F);
            btnRefresh.ForeColor = Color.FromArgb(16, 24, 40);
            btnRefresh.Location = new Point(772, 9);
            btnRefresh.Margin = new Padding(4);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(70, 30);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "تحديث";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(224, 224, 224);
            designerCommandBar.SetCommandRole(btnClose, CoreUI.DesignerCommandRole.Close);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Microsoft Sans Serif", 10F);
            btnClose.ForeColor = Color.FromArgb(16, 24, 40);
            btnClose.Location = new Point(694, 9);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(70, 30);
            btnClose.TabIndex = 5;
            btnClose.Text = "اغلاق";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // btnFirst
            // 
            btnFirst.BackColor = Color.FromArgb(224, 224, 224);
            designerCommandBar.SetCommandRole(btnFirst, CoreUI.DesignerCommandRole.First);
            btnFirst.FlatStyle = FlatStyle.Flat;
            btnFirst.Font = new Font("Microsoft Sans Serif", 10F);
            btnFirst.ForeColor = Color.FromArgb(16, 24, 40);
            btnFirst.Location = new Point(616, 9);
            btnFirst.Margin = new Padding(4);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(70, 30);
            btnFirst.TabIndex = 6;
            btnFirst.Text = "الاول ";
            btnFirst.UseVisualStyleBackColor = false;
            // 
            // btnPrevious
            // 
            btnPrevious.BackColor = Color.FromArgb(224, 224, 224);
            designerCommandBar.SetCommandRole(btnPrevious, CoreUI.DesignerCommandRole.Previous);
            btnPrevious.FlatStyle = FlatStyle.Flat;
            btnPrevious.Font = new Font("Microsoft Sans Serif", 10F);
            btnPrevious.ForeColor = Color.FromArgb(16, 24, 40);
            btnPrevious.Location = new Point(538, 9);
            btnPrevious.Margin = new Padding(4);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(70, 30);
            btnPrevious.TabIndex = 7;
            btnPrevious.Text = "السابق";
            btnPrevious.UseVisualStyleBackColor = false;
            // 
            // txtCurrentRecordNo
            // 
            txtCurrentRecordNo.BackColor = Color.White;
            txtCurrentRecordNo.Font = new Font("Segoe UI", 11F);
            txtCurrentRecordNo.ForeColor = Color.FromArgb(16, 24, 40);
            txtCurrentRecordNo.Location = new Point(468, 9);
            txtCurrentRecordNo.Margin = new Padding(4);
            txtCurrentRecordNo.Multiline = true;
            txtCurrentRecordNo.Name = "txtCurrentRecordNo";
            txtCurrentRecordNo.ReadOnly = true;
            txtCurrentRecordNo.Size = new Size(62, 30);
            txtCurrentRecordNo.TabIndex = 8;
            txtCurrentRecordNo.TextAlign = HorizontalAlignment.Center;
            // 
            // btnNext
            // 
            btnNext.BackColor = Color.FromArgb(224, 224, 224);
            designerCommandBar.SetCommandRole(btnNext, CoreUI.DesignerCommandRole.Next);
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("Microsoft Sans Serif", 10F);
            btnNext.ForeColor = Color.FromArgb(16, 24, 40);
            btnNext.Location = new Point(390, 9);
            btnNext.Margin = new Padding(4);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(70, 30);
            btnNext.TabIndex = 9;
            btnNext.Text = "التالي";
            btnNext.UseVisualStyleBackColor = false;
            // 
            // btnLast
            // 
            btnLast.BackColor = Color.FromArgb(224, 224, 224);
            designerCommandBar.SetCommandRole(btnLast, CoreUI.DesignerCommandRole.Last);
            btnLast.FlatStyle = FlatStyle.Flat;
            btnLast.Font = new Font("Microsoft Sans Serif", 10F);
            btnLast.ForeColor = Color.FromArgb(16, 24, 40);
            btnLast.Location = new Point(312, 9);
            btnLast.Margin = new Padding(4);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(70, 30);
            btnLast.TabIndex = 10;
            btnLast.Text = "الاخير";
            btnLast.UseVisualStyleBackColor = false;
            // 
            // btnUndo
            // 
            btnUndo.BackColor = Color.FromArgb(224, 224, 224);
            designerCommandBar.SetCommandRole(btnUndo, CoreUI.DesignerCommandRole.Cancel);
            btnUndo.FlatStyle = FlatStyle.Flat;
            btnUndo.Font = new Font("Microsoft Sans Serif", 10F);
            btnUndo.ForeColor = Color.FromArgb(16, 24, 40);
            btnUndo.Location = new Point(234, 9);
            btnUndo.Margin = new Padding(4);
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new Size(70, 30);
            btnUndo.TabIndex = 11;
            btnUndo.Text = "تراجع";
            btnUndo.UseVisualStyleBackColor = false;
            // 
            // designerHiddenCommands
            // 
            designerHiddenCommands.Controls.Add(standardCommandView);
            designerHiddenCommands.Controls.Add(standardCommandPrint);
            designerHiddenCommands.Controls.Add(standardCommandImport);
            designerHiddenCommands.Controls.Add(standardCommandExport);
            designerHiddenCommands.Controls.Add(standardCommandHelp);
            designerHiddenCommands.Location = new Point(0, 0);
            designerHiddenCommands.Name = "designerHiddenCommands";
            designerHiddenCommands.Size = new Size(200, 100);
            designerHiddenCommands.TabIndex = 3;
            designerHiddenCommands.Visible = false;
            // 
            // standardCommandView
            // 
            standardCommandView.AccessibleName = "عرض";
            designerCommandBar.SetCommandRole(standardCommandView, CoreUI.DesignerCommandRole.View);
            standardCommandView.Enabled = false;
            standardCommandView.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
            standardCommandView.FlatStyle = FlatStyle.Flat;
            standardCommandView.Image = (Image)resources.GetObject("standardCommandView.Image");
            standardCommandView.Location = new Point(173, 1);
            standardCommandView.Margin = new Padding(1);
            standardCommandView.Name = "standardCommandView";
            standardCommandView.Size = new Size(26, 24);
            standardCommandView.TabIndex = 0;
            standardCommandView.Visible = false;
            // 
            // standardCommandPrint
            // 
            standardCommandPrint.AccessibleName = "طباعة";
            designerCommandBar.SetCommandRole(standardCommandPrint, CoreUI.DesignerCommandRole.Print);
            standardCommandPrint.Enabled = false;
            standardCommandPrint.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
            standardCommandPrint.FlatStyle = FlatStyle.Flat;
            standardCommandPrint.Image = (Image)resources.GetObject("standardCommandPrint.Image");
            standardCommandPrint.Location = new Point(145, 1);
            standardCommandPrint.Margin = new Padding(1);
            standardCommandPrint.Name = "standardCommandPrint";
            standardCommandPrint.Size = new Size(26, 24);
            standardCommandPrint.TabIndex = 1;
            standardCommandPrint.Visible = false;
            // 
            // standardCommandImport
            // 
            standardCommandImport.AccessibleName = "استيراد";
            designerCommandBar.SetCommandRole(standardCommandImport, CoreUI.DesignerCommandRole.Import);
            standardCommandImport.Enabled = false;
            standardCommandImport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
            standardCommandImport.FlatStyle = FlatStyle.Flat;
            standardCommandImport.Location = new Point(117, 1);
            standardCommandImport.Margin = new Padding(1);
            standardCommandImport.Name = "standardCommandImport";
            standardCommandImport.Size = new Size(26, 24);
            standardCommandImport.TabIndex = 2;
            standardCommandImport.Text = "استيراد";
            standardCommandImport.Visible = false;
            // 
            // standardCommandExport
            // 
            standardCommandExport.AccessibleName = "تصدير";
            designerCommandBar.SetCommandRole(standardCommandExport, CoreUI.DesignerCommandRole.Export);
            standardCommandExport.Enabled = false;
            standardCommandExport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
            standardCommandExport.FlatStyle = FlatStyle.Flat;
            standardCommandExport.Location = new Point(89, 1);
            standardCommandExport.Margin = new Padding(1);
            standardCommandExport.Name = "standardCommandExport";
            standardCommandExport.Size = new Size(26, 24);
            standardCommandExport.TabIndex = 3;
            standardCommandExport.Text = "تصدير";
            standardCommandExport.Visible = false;
            // 
            // standardCommandHelp
            // 
            standardCommandHelp.AccessibleName = "مساعدة";
            designerCommandBar.SetCommandRole(standardCommandHelp, CoreUI.DesignerCommandRole.Help);
            standardCommandHelp.Enabled = false;
            standardCommandHelp.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
            standardCommandHelp.FlatStyle = FlatStyle.Flat;
            standardCommandHelp.Location = new Point(61, 1);
            standardCommandHelp.Margin = new Padding(1);
            standardCommandHelp.Name = "standardCommandHelp";
            standardCommandHelp.Size = new Size(26, 24);
            standardCommandHelp.TabIndex = 4;
            standardCommandHelp.Text = "مساعدة";
            standardCommandHelp.Visible = false;
            // 
            // pnlHeader
            // 
            pnlHeader.AutoSize = true;
            pnlHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlHeader.BackColor = Color.FromArgb(224, 224, 224);
            pnlHeader.Controls.Add(designerCommandBar);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.MinimumSize = new Size(0, 48);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1346, 53);
            pnlHeader.TabIndex = 9;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.FromArgb(192, 192, 255);
            lblTitle.Dock = DockStyle.Left;
            lblTitle.Font = new Font("Segoe UI", 16F);
            lblTitle.ForeColor = Color.FromArgb(16, 24, 40);
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
            tlpAuditInfo.BackColor = Color.FromArgb(248, 250, 252);
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
            tlpAuditInfo.Location = new Point(0, 688);
            tlpAuditInfo.Margin = new Padding(0);
            tlpAuditInfo.Name = "tlpAuditInfo";
            tlpAuditInfo.Padding = new Padding(8);
            tlpAuditInfo.RowCount = 1;
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAuditInfo.Size = new Size(1346, 48);
            tlpAuditInfo.TabIndex = 10;
            tlpAuditInfo.Visible = false;
            // 
            // lblPrintCount
            // 
            lblPrintCount.AutoEllipsis = true;
            lblPrintCount.BackColor = Color.FromArgb(192, 255, 255);
            lblPrintCount.Dock = DockStyle.Fill;
            lblPrintCount.Font = new Font("Segoe UI", 10F);
            lblPrintCount.ForeColor = Color.FromArgb(16, 24, 40);
            lblPrintCount.Location = new Point(11, 11);
            lblPrintCount.Margin = new Padding(3);
            lblPrintCount.Name = "lblPrintCount";
            lblPrintCount.RightToLeft = RightToLeft.No;
            lblPrintCount.Size = new Size(157, 26);
            lblPrintCount.TabIndex = 8;
            lblPrintCount.Text = "عدد مرات الطباعة :- 00";
            lblPrintCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblLastPrintedAt
            // 
            lblLastPrintedAt.AutoEllipsis = true;
            lblLastPrintedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblLastPrintedAt.Dock = DockStyle.Fill;
            lblLastPrintedAt.Font = new Font("Segoe UI", 10F);
            lblLastPrintedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblLastPrintedAt.Location = new Point(174, 11);
            lblLastPrintedAt.Margin = new Padding(3);
            lblLastPrintedAt.Name = "lblLastPrintedAt";
            lblLastPrintedAt.RightToLeft = RightToLeft.No;
            lblLastPrintedAt.Size = new Size(206, 26);
            lblLastPrintedAt.TabIndex = 5;
            lblLastPrintedAt.Text = "تاريخ اخر طباعة:  27/09/2026";
            lblLastPrintedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblEditCount
            // 
            lblEditCount.AutoEllipsis = true;
            lblEditCount.BackColor = Color.FromArgb(192, 255, 255);
            lblEditCount.Dock = DockStyle.Fill;
            lblEditCount.Font = new Font("Segoe UI", 10F);
            lblEditCount.ForeColor = Color.FromArgb(16, 24, 40);
            lblEditCount.Location = new Point(386, 11);
            lblEditCount.Margin = new Padding(3);
            lblEditCount.Name = "lblEditCount";
            lblEditCount.RightToLeft = RightToLeft.No;
            lblEditCount.Size = new Size(153, 26);
            lblEditCount.TabIndex = 4;
            lblEditCount.Text = "عدد التعديلات :- 00";
            lblEditCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedAt
            // 
            lblModifiedAt.AutoEllipsis = true;
            lblModifiedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedAt.Dock = DockStyle.Fill;
            lblModifiedAt.Font = new Font("Segoe UI", 10F);
            lblModifiedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblModifiedAt.Location = new Point(545, 11);
            lblModifiedAt.Margin = new Padding(3);
            lblModifiedAt.Name = "lblModifiedAt";
            lblModifiedAt.RightToLeft = RightToLeft.No;
            lblModifiedAt.Size = new Size(206, 26);
            lblModifiedAt.TabIndex = 3;
            lblModifiedAt.Text = " تاريخ التعديل:  27/09/2026";
            lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedBy
            // 
            lblModifiedBy.AutoEllipsis = true;
            lblModifiedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedBy.Dock = DockStyle.Fill;
            lblModifiedBy.Font = new Font("Segoe UI", 10F);
            lblModifiedBy.ForeColor = Color.FromArgb(16, 24, 40);
            lblModifiedBy.Location = new Point(757, 11);
            lblModifiedBy.Margin = new Padding(3);
            lblModifiedBy.Name = "lblModifiedBy";
            lblModifiedBy.RightToLeft = RightToLeft.No;
            lblModifiedBy.Size = new Size(180, 26);
            lblModifiedBy.TabIndex = 2;
            lblModifiedBy.Text = "عدل بواسطة: - مدير النظام";
            lblModifiedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedAt
            // 
            lblCreatedAt.AutoEllipsis = true;
            lblCreatedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedAt.Dock = DockStyle.Fill;
            lblCreatedAt.Font = new Font("Segoe UI", 10F);
            lblCreatedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblCreatedAt.Location = new Point(943, 11);
            lblCreatedAt.Margin = new Padding(3);
            lblCreatedAt.Name = "lblCreatedAt";
            lblCreatedAt.RightToLeft = RightToLeft.No;
            lblCreatedAt.Size = new Size(206, 26);
            lblCreatedAt.TabIndex = 1;
            lblCreatedAt.Text = " تاريخ الانشاء:  27/09/2026";
            lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedBy
            // 
            lblCreatedBy.AutoEllipsis = true;
            lblCreatedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedBy.Dock = DockStyle.Fill;
            lblCreatedBy.Font = new Font("Segoe UI", 10F);
            lblCreatedBy.ForeColor = Color.FromArgb(16, 24, 40);
            lblCreatedBy.Location = new Point(1155, 11);
            lblCreatedBy.Margin = new Padding(3);
            lblCreatedBy.Name = "lblCreatedBy";
            lblCreatedBy.RightToLeft = RightToLeft.No;
            lblCreatedBy.Size = new Size(180, 26);
            lblCreatedBy.TabIndex = 0;
            lblCreatedBy.Text = " أنشأ بواسطة: - مدير النظام";
            lblCreatedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.LightCyan;
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
            tabMain.Dock = DockStyle.Fill;
            tabMain.Font = new Font("Segoe UI", 9F);
            tabMain.Location = new Point(0, 0);
            tabMain.Margin = new Padding(0);
            tabMain.Name = "tabMain";
            tabMain.RightToLeftLayout = true;
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1346, 308);
            tabMain.TabIndex = 0;
            // 
            // tabPricingRules
            // 
            tabPricingRules.AutoScroll = true;
            tabPricingRules.BackColor = Color.LightCyan;
            tabPricingRules.Controls.Add(tlpPricingRuleFields);
            tabPricingRules.Location = new Point(4, 29);
            tabPricingRules.Margin = new Padding(0);
            tabPricingRules.Name = "tabPricingRules";
            tabPricingRules.Padding = new Padding(3);
            tabPricingRules.Size = new Size(1338, 275);
            tabPricingRules.TabIndex = 2;
            tabPricingRules.Text = "انواع التغليف";
            // 
            // tlpPricingRuleFields
            // 
            tlpPricingRuleFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpPricingRuleFields.BackColor = Color.LightCyan;
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
            tlpPricingRuleFields.Location = new Point(3, 3);
            tlpPricingRuleFields.Margin = new Padding(0);
            tlpPricingRuleFields.MinimumSize = new Size(0, 232);
            tlpPricingRuleFields.Name = "tlpPricingRuleFields";
            tlpPricingRuleFields.RowCount = 5;
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPricingRuleFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            tlpPricingRuleFields.Size = new Size(1332, 232);
            tlpPricingRuleFields.TabIndex = 11;
            // 
            // txtNotes
            // 
            txtNotes.BackColor = Color.White;
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Font = new Font("Segoe UI", 11F);
            txtNotes.ForeColor = Color.FromArgb(16, 24, 40);
            txtNotes.Location = new Point(891, 163);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.Size = new Size(290, 66);
            txtNotes.TabIndex = 156;
            // 
            // label6
            // 
            label6.AutoEllipsis = true;
            label6.BackColor = Color.Transparent;
            label6.Dock = DockStyle.Fill;
            label6.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label6.ForeColor = Color.FromArgb(16, 24, 40);
            label6.Location = new Point(1187, 163);
            label6.Margin = new Padding(3);
            label6.Name = "label6";
            label6.Size = new Size(142, 66);
            label6.TabIndex = 155;
            label6.Text = "ملاحظات";
            label6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // chkRequiresSpecialPackaging
            // 
            chkRequiresSpecialPackaging.AutoSize = true;
            chkRequiresSpecialPackaging.BackColor = Color.Transparent;
            chkRequiresSpecialPackaging.Dock = DockStyle.Fill;
            chkRequiresSpecialPackaging.Font = new Font("Segoe UI", 10F);
            chkRequiresSpecialPackaging.ForeColor = Color.FromArgb(16, 24, 40);
            chkRequiresSpecialPackaging.Location = new Point(447, 83);
            chkRequiresSpecialPackaging.Name = "chkRequiresSpecialPackaging";
            chkRequiresSpecialPackaging.Size = new Size(290, 34);
            chkRequiresSpecialPackaging.TabIndex = 152;
            chkRequiresSpecialPackaging.Text = "يتطلب تغليف خاص";
            chkRequiresSpecialPackaging.UseVisualStyleBackColor = false;
            // 
            // nudDefaultPackageWeight
            // 
            nudDefaultPackageWeight.BackColor = Color.White;
            nudDefaultPackageWeight.Dock = DockStyle.Fill;
            nudDefaultPackageWeight.Font = new Font("Segoe UI", 11F);
            nudDefaultPackageWeight.ForeColor = Color.FromArgb(16, 24, 40);
            nudDefaultPackageWeight.Location = new Point(447, 43);
            nudDefaultPackageWeight.Name = "nudDefaultPackageWeight";
            nudDefaultPackageWeight.Size = new Size(290, 32);
            nudDefaultPackageWeight.TabIndex = 151;
            // 
            // label14
            // 
            label14.AutoEllipsis = true;
            label14.BackColor = Color.Transparent;
            label14.Dock = DockStyle.Fill;
            label14.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label14.ForeColor = Color.FromArgb(16, 24, 40);
            label14.Location = new Point(299, 163);
            label14.Margin = new Padding(3);
            label14.Name = "label14";
            label14.Size = new Size(142, 66);
            label14.TabIndex = 140;
            label14.Text = "الوصف";
            label14.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDescription
            // 
            txtDescription.BackColor = Color.White;
            txtDescription.Dock = DockStyle.Fill;
            txtDescription.Font = new Font("Segoe UI", 11F);
            txtDescription.ForeColor = Color.FromArgb(16, 24, 40);
            txtDescription.Location = new Point(3, 163);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(290, 66);
            txtDescription.TabIndex = 134;
            // 
            // nudDisplayOrder
            // 
            nudDisplayOrder.BackColor = Color.White;
            nudDisplayOrder.Dock = DockStyle.Fill;
            nudDisplayOrder.Font = new Font("Segoe UI", 11F);
            nudDisplayOrder.ForeColor = Color.FromArgb(16, 24, 40);
            nudDisplayOrder.Location = new Point(891, 83);
            nudDisplayOrder.Name = "nudDisplayOrder";
            nudDisplayOrder.Size = new Size(290, 32);
            nudDisplayOrder.TabIndex = 113;
            // 
            // nudDefaultPackagingCost
            // 
            nudDefaultPackagingCost.BackColor = Color.White;
            nudDefaultPackagingCost.Dock = DockStyle.Fill;
            nudDefaultPackagingCost.Font = new Font("Segoe UI", 11F);
            nudDefaultPackagingCost.ForeColor = Color.FromArgb(16, 24, 40);
            nudDefaultPackagingCost.Location = new Point(3, 43);
            nudDefaultPackagingCost.Name = "nudDefaultPackagingCost";
            nudDefaultPackagingCost.Size = new Size(290, 32);
            nudDefaultPackagingCost.TabIndex = 112;
            // 
            // txtPackagingTypeNameEn
            // 
            txtPackagingTypeNameEn.BackColor = Color.White;
            txtPackagingTypeNameEn.Dock = DockStyle.Fill;
            txtPackagingTypeNameEn.Font = new Font("Segoe UI", 11F);
            txtPackagingTypeNameEn.ForeColor = Color.FromArgb(16, 24, 40);
            txtPackagingTypeNameEn.Location = new Point(3, 3);
            txtPackagingTypeNameEn.Name = "txtPackagingTypeNameEn";
            txtPackagingTypeNameEn.Size = new Size(290, 32);
            txtPackagingTypeNameEn.TabIndex = 111;
            // 
            // label7
            // 
            label7.AutoEllipsis = true;
            label7.BackColor = Color.Transparent;
            label7.Dock = DockStyle.Fill;
            label7.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label7.ForeColor = Color.FromArgb(16, 24, 40);
            label7.Location = new Point(299, 43);
            label7.Margin = new Padding(3);
            label7.Name = "label7";
            label7.Size = new Size(142, 34);
            label7.TabIndex = 110;
            label7.Text = "تكلفة التغليف الافتراضي";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.AutoEllipsis = true;
            label2.BackColor = Color.Transparent;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(16, 24, 40);
            label2.Location = new Point(299, 3);
            label2.Margin = new Padding(3);
            label2.Name = "label2";
            label2.Size = new Size(142, 34);
            label2.TabIndex = 106;
            label2.Text = "اسم نوع التغليف انجليزي";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.AutoEllipsis = true;
            label1.BackColor = Color.Transparent;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(16, 24, 40);
            label1.Location = new Point(743, 3);
            label1.Margin = new Padding(3);
            label1.Name = "label1";
            label1.Size = new Size(142, 34);
            label1.TabIndex = 99;
            label1.Text = "اسم نوع التغليف عربي";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPackagingTypeNameAr
            // 
            txtPackagingTypeNameAr.BackColor = Color.White;
            txtPackagingTypeNameAr.Dock = DockStyle.Fill;
            txtPackagingTypeNameAr.Font = new Font("Segoe UI", 11F);
            txtPackagingTypeNameAr.ForeColor = Color.FromArgb(16, 24, 40);
            txtPackagingTypeNameAr.Location = new Point(447, 3);
            txtPackagingTypeNameAr.Name = "txtPackagingTypeNameAr";
            txtPackagingTypeNameAr.Size = new Size(290, 32);
            txtPackagingTypeNameAr.TabIndex = 75;
            // 
            // cmbPackagingCategory
            // 
            cmbPackagingCategory.BackColor = Color.White;
            cmbPackagingCategory.Dock = DockStyle.Fill;
            cmbPackagingCategory.Font = new Font("Segoe UI", 11F);
            cmbPackagingCategory.ForeColor = Color.FromArgb(16, 24, 40);
            cmbPackagingCategory.FormattingEnabled = true;
            cmbPackagingCategory.Location = new Point(891, 43);
            cmbPackagingCategory.Name = "cmbPackagingCategory";
            cmbPackagingCategory.Size = new Size(290, 33);
            cmbPackagingCategory.TabIndex = 62;
            // 
            // lblShipmentType
            // 
            lblShipmentType.AutoEllipsis = true;
            lblShipmentType.BackColor = Color.Transparent;
            lblShipmentType.Dock = DockStyle.Fill;
            lblShipmentType.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblShipmentType.ForeColor = Color.FromArgb(16, 24, 40);
            lblShipmentType.Location = new Point(1187, 43);
            lblShipmentType.Margin = new Padding(3);
            lblShipmentType.Name = "lblShipmentType";
            lblShipmentType.Size = new Size(142, 34);
            lblShipmentType.TabIndex = 59;
            lblShipmentType.Text = "فئة التغليف";
            lblShipmentType.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTransportMethod
            // 
            lblTransportMethod.AutoEllipsis = true;
            lblTransportMethod.BackColor = Color.Transparent;
            lblTransportMethod.Dock = DockStyle.Fill;
            lblTransportMethod.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTransportMethod.ForeColor = Color.FromArgb(16, 24, 40);
            lblTransportMethod.Location = new Point(1187, 83);
            lblTransportMethod.Margin = new Padding(3);
            lblTransportMethod.Name = "lblTransportMethod";
            lblTransportMethod.Size = new Size(142, 34);
            lblTransportMethod.TabIndex = 53;
            lblTransportMethod.Text = "ترتيب العرض";
            lblTransportMethod.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPriority
            // 
            lblPriority.AutoEllipsis = true;
            lblPriority.BackColor = Color.Transparent;
            lblPriority.Dock = DockStyle.Fill;
            lblPriority.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPriority.ForeColor = Color.FromArgb(16, 24, 40);
            lblPriority.Location = new Point(743, 43);
            lblPriority.Margin = new Padding(3);
            lblPriority.Name = "lblPriority";
            lblPriority.Size = new Size(142, 34);
            lblPriority.TabIndex = 6;
            lblPriority.Text = "وزن التغليف الافتراضي";
            lblPriority.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPricingRuleCode
            // 
            lblPricingRuleCode.AutoEllipsis = true;
            lblPricingRuleCode.BackColor = Color.Transparent;
            lblPricingRuleCode.Dock = DockStyle.Fill;
            lblPricingRuleCode.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPricingRuleCode.ForeColor = Color.FromArgb(16, 24, 40);
            lblPricingRuleCode.Location = new Point(1187, 3);
            lblPricingRuleCode.Margin = new Padding(3);
            lblPricingRuleCode.Name = "lblPricingRuleCode";
            lblPricingRuleCode.Size = new Size(142, 34);
            lblPricingRuleCode.TabIndex = 0;
            lblPricingRuleCode.Text = "كود نوع التغليف";
            lblPricingRuleCode.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPackagingTypeCode
            // 
            txtPackagingTypeCode.BackColor = Color.White;
            txtPackagingTypeCode.Dock = DockStyle.Fill;
            txtPackagingTypeCode.Font = new Font("Segoe UI", 11F);
            txtPackagingTypeCode.ForeColor = Color.FromArgb(16, 24, 40);
            txtPackagingTypeCode.Location = new Point(891, 3);
            txtPackagingTypeCode.Name = "txtPackagingTypeCode";
            txtPackagingTypeCode.Size = new Size(290, 32);
            txtPackagingTypeCode.TabIndex = 1;
            // 
            // chkIsReusable
            // 
            chkIsReusable.AutoSize = true;
            chkIsReusable.BackColor = Color.Transparent;
            chkIsReusable.Dock = DockStyle.Fill;
            chkIsReusable.Font = new Font("Segoe UI", 10F);
            chkIsReusable.ForeColor = Color.FromArgb(16, 24, 40);
            chkIsReusable.Location = new Point(3, 83);
            chkIsReusable.Name = "chkIsReusable";
            chkIsReusable.Size = new Size(290, 34);
            chkIsReusable.TabIndex = 145;
            chkIsReusable.Text = "قابل لأعادة الاستخدام";
            chkIsReusable.UseVisualStyleBackColor = false;
            // 
            // chkSuitableForFragile
            // 
            chkSuitableForFragile.AutoSize = true;
            chkSuitableForFragile.BackColor = Color.Transparent;
            chkSuitableForFragile.Dock = DockStyle.Fill;
            chkSuitableForFragile.Font = new Font("Segoe UI", 10F);
            chkSuitableForFragile.ForeColor = Color.FromArgb(16, 24, 40);
            chkSuitableForFragile.Location = new Point(891, 123);
            chkSuitableForFragile.Name = "chkSuitableForFragile";
            chkSuitableForFragile.Size = new Size(290, 34);
            chkSuitableForFragile.TabIndex = 149;
            chkSuitableForFragile.Text = "مناسب للمواد القابله للكسر";
            chkSuitableForFragile.UseVisualStyleBackColor = false;
            // 
            // chkSuitableForLiquids
            // 
            chkSuitableForLiquids.AutoSize = true;
            chkSuitableForLiquids.BackColor = Color.Transparent;
            chkSuitableForLiquids.Dock = DockStyle.Fill;
            chkSuitableForLiquids.Font = new Font("Segoe UI", 10F);
            chkSuitableForLiquids.ForeColor = Color.FromArgb(16, 24, 40);
            chkSuitableForLiquids.Location = new Point(447, 123);
            chkSuitableForLiquids.Name = "chkSuitableForLiquids";
            chkSuitableForLiquids.Size = new Size(290, 34);
            chkSuitableForLiquids.TabIndex = 150;
            chkSuitableForLiquids.Text = "مناسب للمواد السائلة";
            chkSuitableForLiquids.UseVisualStyleBackColor = false;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.BackColor = Color.Transparent;
            chkIsActive.Dock = DockStyle.Fill;
            chkIsActive.Font = new Font("Segoe UI", 10F);
            chkIsActive.ForeColor = Color.FromArgb(16, 24, 40);
            chkIsActive.Location = new Point(3, 123);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(290, 34);
            chkIsActive.TabIndex = 95;
            chkIsActive.Text = "نشط";
            chkIsActive.UseVisualStyleBackColor = false;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.LightCyan;
            tabPage1.Location = new Point(4, 29);
            tabPage1.Margin = new Padding(0);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1338, 275);
            tabPage1.TabIndex = 3;
            tabPage1.Text = "tabPage1";
            // 
            // standardAuditMetadata
            // 
            standardAuditMetadata.AutoScroll = true;
            standardAuditMetadata.Dock = DockStyle.Bottom;
            standardAuditMetadata.Font = new Font("Tahoma", 9F);
            standardAuditMetadata.Location = new Point(0, 736);
            standardAuditMetadata.Margin = new Padding(0);
            standardAuditMetadata.MinimumSize = new Size(0, 80);
            standardAuditMetadata.Name = "standardAuditMetadata";
            standardAuditMetadata.RightToLeft = RightToLeft.Yes;
            standardAuditMetadata.Size = new Size(1346, 80);
            standardAuditMetadata.TabIndex = 12;
            standardAuditMetadata.TabStop = false;
            // 
            // UcPackagingTypes
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(248, 250, 252);
            Controls.Add(pnlContent);
            Controls.Add(tlpAuditInfo);
            Controls.Add(pnlHeader);
            Controls.Add(standardAuditMetadata);
            Font = new Font("Segoe UI", 10F);
            ForeColor = Color.FromArgb(16, 24, 40);
            Margin = new Padding(0);
            Name = "UcPackagingTypes";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1346, 816);
            Load += UcPackagingTypes_Load;
            designerCommandBar.ResumeLayout(false);
            designerCommandBar.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            designerHiddenCommands.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
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
            PerformLayout();
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
