namespace TransportERP.Desktop.Forms.SystemSettings.General;
partial class UcTaxBracketCodes
{
    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
    private FlowLayoutPanel standardCommandFlow = null!;
    private Panel standardCloseHost = null!;
    private Button standardCommandAdd = null!;
    private Button standardCommandEdit = null!;
    private Button standardCommandDelete = null!;
    private Button standardCommandCancel = null!;
    private Button standardCommandView = null!;
    private Button standardCommandLast = null!;
    private Button standardCommandNext = null!;
    private Button standardCommandPrevious = null!;
    private Button standardCommandFirst = null!;
    private Button standardCommandSave = null!;
    private Button standardCommandPrint = null!;
    private Button standardCommandClose = null!;
    private Button standardCommandRefresh = null!;
    private Button standardCommandImport = null!;
    private Button standardCommandExport = null!;
    private Button standardCommandHelp = null!;

    private TableLayoutPanel layout = null!;
    private TableLayoutPanel fields = null!;
    private Label lblTitle = null!;
    private Label lblStatus = null!;
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;
    private DataGridView gridTaxBrackets = null!;
    private DataGridViewTextBoxColumn gridTaxBrackets_colBracketNumber = null!;
    private DataGridViewTextBoxColumn gridTaxBrackets_colNameLocal = null!;
    private DataGridViewTextBoxColumn gridTaxBrackets_colNameForeign = null!;
    private DataGridViewTextBoxColumn gridTaxBrackets_colTaxRate = null!;
    private DataGridViewCheckBoxColumn gridTaxBrackets_colDefault = null!;
    private DataGridViewCheckBoxColumn gridTaxBrackets_colStopped = null!;
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcTaxBracketCodes));
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        standardCommandFlow = new FlowLayoutPanel();
        standardCommandAdd = new Button();
        standardCommandEdit = new Button();
        standardCommandDelete = new Button();
        standardCommandCancel = new Button();
        standardCommandView = new Button();
        standardCommandLast = new Button();
        standardCommandNext = new Button();
        standardCommandPrevious = new Button();
        standardCommandFirst = new Button();
        standardCommandSave = new Button();
        standardCommandPrint = new Button();
        standardCommandRefresh = new Button();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
        standardCloseHost = new Panel();
        standardCommandClose = new Button();
        layout = new TableLayoutPanel();
        gridTaxBrackets = new DataGridView();
        gridTaxBrackets_colBracketNumber = new DataGridViewTextBoxColumn();
        gridTaxBrackets_colNameLocal = new DataGridViewTextBoxColumn();
        gridTaxBrackets_colNameForeign = new DataGridViewTextBoxColumn();
        gridTaxBrackets_colTaxRate = new DataGridViewTextBoxColumn();
        gridTaxBrackets_colDefault = new DataGridViewCheckBoxColumn();
        gridTaxBrackets_colStopped = new DataGridViewCheckBoxColumn();
        lblTitle = new Label();
        lblStatus = new Label();
        fields = new TableLayoutPanel();
        standardAuditMetadata = new TransportERP.Desktop.CoreUI.AuditMetadataControl();
        designerCommandBar.SuspendLayout();
        standardCommandFlow.SuspendLayout();
        standardCloseHost.SuspendLayout();
        layout.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)gridTaxBrackets).BeginInit();
        SuspendLayout();
        // 
        // designerCommandBar
        // 
        designerCommandBar.BackColor = Color.FromArgb(239, 239, 239);
        designerCommandBar.BorderStyle = BorderStyle.FixedSingle;
        designerCommandBar.Controls.Add(standardCommandFlow);
        designerCommandBar.Controls.Add(standardCloseHost);
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(0, 100);
        designerCommandBar.Margin = new Padding(0);
        designerCommandBar.MinimumSize = new Size(2, 37);
        designerCommandBar.Name = "designerCommandBar";
        designerCommandBar.Size = new Size(1250, 54);
        designerCommandBar.TabIndex = 5;
        // 
        // standardCommandFlow
        // 
        standardCommandFlow.Controls.Add(standardCommandAdd);
        standardCommandFlow.Controls.Add(standardCommandEdit);
        standardCommandFlow.Controls.Add(standardCommandDelete);
        standardCommandFlow.Controls.Add(standardCommandCancel);
        standardCommandFlow.Controls.Add(standardCommandView);
        standardCommandFlow.Controls.Add(standardCommandLast);
        standardCommandFlow.Controls.Add(standardCommandNext);
        standardCommandFlow.Controls.Add(standardCommandPrevious);
        standardCommandFlow.Controls.Add(standardCommandFirst);
        standardCommandFlow.Controls.Add(standardCommandSave);
        standardCommandFlow.Controls.Add(standardCommandPrint);
        standardCommandFlow.Controls.Add(standardCommandRefresh);
        standardCommandFlow.Controls.Add(standardCommandImport);
        standardCommandFlow.Controls.Add(standardCommandExport);
        standardCommandFlow.Controls.Add(standardCommandHelp);
        standardCommandFlow.Dock = DockStyle.Fill;
        standardCommandFlow.FlowDirection = FlowDirection.RightToLeft;
        standardCommandFlow.Location = new Point(35, 0);
        standardCommandFlow.Margin = new Padding(4);
        standardCommandFlow.Name = "standardCommandFlow";
        standardCommandFlow.Padding = new Padding(2);
        standardCommandFlow.RightToLeft = RightToLeft.No;
        standardCommandFlow.Size = new Size(1213, 52);
        standardCommandFlow.TabIndex = 0;
        standardCommandFlow.WrapContents = false;
        // 
        // standardCommandAdd
        // 
        standardCommandAdd.AccessibleName = "إضافة";
        designerCommandBar.SetCommandRole(standardCommandAdd, CoreUI.DesignerCommandRole.Add);
        standardCommandAdd.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandAdd.FlatStyle = FlatStyle.Flat;
        standardCommandAdd.Image = (Image)resources.GetObject("standardCommandAdd.Image");
        standardCommandAdd.Location = new Point(1176, 3);
        standardCommandAdd.Margin = new Padding(1);
        standardCommandAdd.Name = "standardCommandAdd";
        standardCommandAdd.Size = new Size(32, 30);
        standardCommandAdd.TabIndex = 0;
        standardCommandAdd.Click += draftAction_Click;
        // 
        // standardCommandEdit
        // 
        standardCommandEdit.AccessibleName = "تعديل";
        designerCommandBar.SetCommandRole(standardCommandEdit, CoreUI.DesignerCommandRole.Edit);
        standardCommandEdit.Enabled = false;
        standardCommandEdit.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandEdit.FlatStyle = FlatStyle.Flat;
        standardCommandEdit.Image = (Image)resources.GetObject("standardCommandEdit.Image");
        standardCommandEdit.Location = new Point(1142, 3);
        standardCommandEdit.Margin = new Padding(1);
        standardCommandEdit.Name = "standardCommandEdit";
        standardCommandEdit.Size = new Size(32, 30);
        standardCommandEdit.TabIndex = 1;
        // 
        // standardCommandDelete
        // 
        standardCommandDelete.AccessibleName = "حذف";
        designerCommandBar.SetCommandRole(standardCommandDelete, CoreUI.DesignerCommandRole.Delete);
        standardCommandDelete.Enabled = false;
        standardCommandDelete.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandDelete.FlatStyle = FlatStyle.Flat;
        standardCommandDelete.Image = (Image)resources.GetObject("standardCommandDelete.Image");
        standardCommandDelete.Location = new Point(1108, 3);
        standardCommandDelete.Margin = new Padding(1);
        standardCommandDelete.Name = "standardCommandDelete";
        standardCommandDelete.Size = new Size(32, 30);
        standardCommandDelete.TabIndex = 2;
        // 
        // standardCommandCancel
        // 
        standardCommandCancel.AccessibleName = "تراجع";
        designerCommandBar.SetCommandRole(standardCommandCancel, CoreUI.DesignerCommandRole.Cancel);
        standardCommandCancel.Enabled = false;
        standardCommandCancel.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandCancel.FlatStyle = FlatStyle.Flat;
        standardCommandCancel.Image = (Image)resources.GetObject("standardCommandCancel.Image");
        standardCommandCancel.Location = new Point(1074, 3);
        standardCommandCancel.Margin = new Padding(1);
        standardCommandCancel.Name = "standardCommandCancel";
        standardCommandCancel.Size = new Size(32, 30);
        standardCommandCancel.TabIndex = 3;
        // 
        // standardCommandView
        // 
        standardCommandView.AccessibleName = "عرض";
        designerCommandBar.SetCommandRole(standardCommandView, CoreUI.DesignerCommandRole.View);
        standardCommandView.Enabled = false;
        standardCommandView.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandView.FlatStyle = FlatStyle.Flat;
        standardCommandView.Image = (Image)resources.GetObject("standardCommandView.Image");
        standardCommandView.Location = new Point(1040, 3);
        standardCommandView.Margin = new Padding(1);
        standardCommandView.Name = "standardCommandView";
        standardCommandView.Size = new Size(32, 30);
        standardCommandView.TabIndex = 4;
        // 
        // standardCommandLast
        // 
        standardCommandLast.AccessibleName = "الأخير";
        designerCommandBar.SetCommandRole(standardCommandLast, CoreUI.DesignerCommandRole.Last);
        standardCommandLast.Enabled = false;
        standardCommandLast.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandLast.FlatStyle = FlatStyle.Flat;
        standardCommandLast.Image = (Image)resources.GetObject("standardCommandLast.Image");
        standardCommandLast.Location = new Point(1006, 3);
        standardCommandLast.Margin = new Padding(1);
        standardCommandLast.Name = "standardCommandLast";
        standardCommandLast.Size = new Size(32, 30);
        standardCommandLast.TabIndex = 5;
        // 
        // standardCommandNext
        // 
        standardCommandNext.AccessibleName = "التالي";
        designerCommandBar.SetCommandRole(standardCommandNext, CoreUI.DesignerCommandRole.Next);
        standardCommandNext.Enabled = false;
        standardCommandNext.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandNext.FlatStyle = FlatStyle.Flat;
        standardCommandNext.Image = (Image)resources.GetObject("standardCommandNext.Image");
        standardCommandNext.Location = new Point(972, 3);
        standardCommandNext.Margin = new Padding(1);
        standardCommandNext.Name = "standardCommandNext";
        standardCommandNext.Size = new Size(32, 30);
        standardCommandNext.TabIndex = 6;
        // 
        // standardCommandPrevious
        // 
        standardCommandPrevious.AccessibleName = "السابق";
        designerCommandBar.SetCommandRole(standardCommandPrevious, CoreUI.DesignerCommandRole.Previous);
        standardCommandPrevious.Enabled = false;
        standardCommandPrevious.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandPrevious.FlatStyle = FlatStyle.Flat;
        standardCommandPrevious.Image = (Image)resources.GetObject("standardCommandPrevious.Image");
        standardCommandPrevious.Location = new Point(938, 3);
        standardCommandPrevious.Margin = new Padding(1);
        standardCommandPrevious.Name = "standardCommandPrevious";
        standardCommandPrevious.Size = new Size(32, 30);
        standardCommandPrevious.TabIndex = 7;
        // 
        // standardCommandFirst
        // 
        standardCommandFirst.AccessibleName = "الأول";
        designerCommandBar.SetCommandRole(standardCommandFirst, CoreUI.DesignerCommandRole.First);
        standardCommandFirst.Enabled = false;
        standardCommandFirst.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandFirst.FlatStyle = FlatStyle.Flat;
        standardCommandFirst.Image = (Image)resources.GetObject("standardCommandFirst.Image");
        standardCommandFirst.Location = new Point(904, 3);
        standardCommandFirst.Margin = new Padding(1);
        standardCommandFirst.Name = "standardCommandFirst";
        standardCommandFirst.Size = new Size(32, 30);
        standardCommandFirst.TabIndex = 8;
        // 
        // standardCommandSave
        // 
        standardCommandSave.AccessibleDescription = "الحفظ غير متاح؛ لم تُربط الشاشة بخدمة بيانات.";
        standardCommandSave.AccessibleName = "حفظ";
        designerCommandBar.SetCommandRole(standardCommandSave, CoreUI.DesignerCommandRole.Save);
        standardCommandSave.Enabled = false;
        standardCommandSave.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandSave.FlatStyle = FlatStyle.Flat;
        standardCommandSave.Image = (Image)resources.GetObject("standardCommandSave.Image");
        standardCommandSave.Location = new Point(870, 3);
        standardCommandSave.Margin = new Padding(1);
        standardCommandSave.Name = "standardCommandSave";
        standardCommandSave.Size = new Size(32, 30);
        standardCommandSave.TabIndex = 9;
        // 
        // standardCommandPrint
        // 
        standardCommandPrint.AccessibleName = "طباعة";
        designerCommandBar.SetCommandRole(standardCommandPrint, CoreUI.DesignerCommandRole.Print);
        standardCommandPrint.Enabled = false;
        standardCommandPrint.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandPrint.FlatStyle = FlatStyle.Flat;
        standardCommandPrint.Image = (Image)resources.GetObject("standardCommandPrint.Image");
        standardCommandPrint.Location = new Point(836, 3);
        standardCommandPrint.Margin = new Padding(1);
        standardCommandPrint.Name = "standardCommandPrint";
        standardCommandPrint.Size = new Size(32, 30);
        standardCommandPrint.TabIndex = 10;
        // 
        // standardCommandRefresh
        // 
        standardCommandRefresh.AccessibleName = "تحديث";
        designerCommandBar.SetCommandRole(standardCommandRefresh, CoreUI.DesignerCommandRole.Refresh);
        standardCommandRefresh.Enabled = false;
        standardCommandRefresh.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandRefresh.FlatStyle = FlatStyle.Flat;
        standardCommandRefresh.Location = new Point(802, 3);
        standardCommandRefresh.Margin = new Padding(1);
        standardCommandRefresh.Name = "standardCommandRefresh";
        standardCommandRefresh.Size = new Size(32, 30);
        standardCommandRefresh.TabIndex = 12;
        standardCommandRefresh.Text = "تحديث";
        standardCommandRefresh.Visible = false;
        // 
        // standardCommandImport
        // 
        standardCommandImport.AccessibleName = "استيراد";
        designerCommandBar.SetCommandRole(standardCommandImport, CoreUI.DesignerCommandRole.Import);
        standardCommandImport.Enabled = false;
        standardCommandImport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandImport.FlatStyle = FlatStyle.Flat;
        standardCommandImport.Location = new Point(768, 3);
        standardCommandImport.Margin = new Padding(1);
        standardCommandImport.Name = "standardCommandImport";
        standardCommandImport.Size = new Size(32, 30);
        standardCommandImport.TabIndex = 13;
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
        standardCommandExport.Location = new Point(734, 3);
        standardCommandExport.Margin = new Padding(1);
        standardCommandExport.Name = "standardCommandExport";
        standardCommandExport.Size = new Size(32, 30);
        standardCommandExport.TabIndex = 14;
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
        standardCommandHelp.Location = new Point(700, 3);
        standardCommandHelp.Margin = new Padding(1);
        standardCommandHelp.Name = "standardCommandHelp";
        standardCommandHelp.Size = new Size(32, 30);
        standardCommandHelp.TabIndex = 15;
        standardCommandHelp.Text = "مساعدة";
        standardCommandHelp.Visible = false;
        // 
        // standardCloseHost
        // 
        standardCloseHost.Controls.Add(standardCommandClose);
        standardCloseHost.Dock = DockStyle.Left;
        standardCloseHost.Location = new Point(0, 0);
        standardCloseHost.Margin = new Padding(4);
        standardCloseHost.Name = "standardCloseHost";
        standardCloseHost.Size = new Size(35, 52);
        standardCloseHost.TabIndex = 1;
        // 
        // standardCommandClose
        // 
        standardCommandClose.AccessibleName = "إغلاق";
        designerCommandBar.SetCommandRole(standardCommandClose, CoreUI.DesignerCommandRole.Close);
        standardCommandClose.Enabled = false;
        standardCommandClose.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandClose.FlatStyle = FlatStyle.Flat;
        standardCommandClose.Image = (Image)resources.GetObject("standardCommandClose.Image");
        standardCommandClose.Location = new Point(1, 1);
        standardCommandClose.Margin = new Padding(1);
        standardCommandClose.Name = "standardCommandClose";
        standardCommandClose.Size = new Size(32, 30);
        standardCommandClose.TabIndex = 11;
        // 
        // layout
        // 
        layout.AutoSize = true;
        layout.ColumnCount = 1;
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.Controls.Add(gridTaxBrackets, 0, 4);
        layout.Controls.Add(lblTitle, 0, 0);
        layout.Controls.Add(lblStatus, 0, 1);
        layout.Controls.Add(fields, 0, 3);
        layout.Dock = DockStyle.Top;
        layout.Location = new Point(0, 0);
        layout.Margin = new Padding(4);
        layout.Name = "layout";
        layout.RowCount = 5;
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        layout.Size = new Size(1250, 100);
        layout.TabIndex = 6;
        // 
        // gridTaxBrackets
        // 
        gridTaxBrackets.AccessibleName = "ترميز الشرائح الضريبية";
        gridTaxBrackets.AllowUserToDeleteRows = false;
        gridTaxBrackets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        gridTaxBrackets.ColumnHeadersHeight = 29;
        gridTaxBrackets.Columns.AddRange(new DataGridViewColumn[] { gridTaxBrackets_colBracketNumber, gridTaxBrackets_colNameLocal, gridTaxBrackets_colNameForeign, gridTaxBrackets_colTaxRate, gridTaxBrackets_colDefault, gridTaxBrackets_colStopped });
        gridTaxBrackets.Dock = DockStyle.Fill;
        gridTaxBrackets.Enabled = false;
        gridTaxBrackets.Location = new Point(4, 84);
        gridTaxBrackets.Margin = new Padding(4);
        gridTaxBrackets.MinimumSize = new Size(0, 275);
        gridTaxBrackets.Name = "gridTaxBrackets";
        gridTaxBrackets.RowHeadersVisible = false;
        gridTaxBrackets.RowHeadersWidth = 51;
        gridTaxBrackets.Size = new Size(1242, 275);
        gridTaxBrackets.TabIndex = 2;
        gridTaxBrackets.CurrentCellDirtyStateChanged += grid_CurrentCellDirtyStateChanged;
        // 
        // gridTaxBrackets_colBracketNumber
        // 
        gridTaxBrackets_colBracketNumber.HeaderText = "رقم الشريحة";
        gridTaxBrackets_colBracketNumber.MinimumWidth = 6;
        gridTaxBrackets_colBracketNumber.Name = "gridTaxBrackets_colBracketNumber";
        gridTaxBrackets_colBracketNumber.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // gridTaxBrackets_colNameLocal
        // 
        gridTaxBrackets_colNameLocal.HeaderText = "الاسم (المحلي)";
        gridTaxBrackets_colNameLocal.MinimumWidth = 6;
        gridTaxBrackets_colNameLocal.Name = "gridTaxBrackets_colNameLocal";
        gridTaxBrackets_colNameLocal.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // gridTaxBrackets_colNameForeign
        // 
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        gridTaxBrackets_colNameForeign.DefaultCellStyle = dataGridViewCellStyle1;
        gridTaxBrackets_colNameForeign.HeaderText = "الاسم (الأجنبي)";
        gridTaxBrackets_colNameForeign.MinimumWidth = 6;
        gridTaxBrackets_colNameForeign.Name = "gridTaxBrackets_colNameForeign";
        gridTaxBrackets_colNameForeign.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // gridTaxBrackets_colTaxRate
        // 
        gridTaxBrackets_colTaxRate.HeaderText = "نسبة الضريبة";
        gridTaxBrackets_colTaxRate.MinimumWidth = 6;
        gridTaxBrackets_colTaxRate.Name = "gridTaxBrackets_colTaxRate";
        gridTaxBrackets_colTaxRate.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // gridTaxBrackets_colDefault
        // 
        gridTaxBrackets_colDefault.HeaderText = "الافتراضي";
        gridTaxBrackets_colDefault.MinimumWidth = 6;
        gridTaxBrackets_colDefault.Name = "gridTaxBrackets_colDefault";
        // 
        // gridTaxBrackets_colStopped
        // 
        gridTaxBrackets_colStopped.HeaderText = "موقف";
        gridTaxBrackets_colStopped.MinimumWidth = 6;
        gridTaxBrackets_colStopped.Name = "gridTaxBrackets_colStopped";
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.BackColor = Color.FromArgb(192, 192, 255);
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.Font = new Font("Segoe UI", 16F);
        lblTitle.Location = new Point(4, 0);
        lblTitle.Margin = new Padding(4, 0, 4, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Padding = new Padding(15);
        lblTitle.Size = new Size(1242, 20);
        lblTitle.TabIndex = 3;
        lblTitle.Text = "ترميز الشرائح الضريبية";
        lblTitle.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.Location = new Point(4, 20);
        lblStatus.Margin = new Padding(4, 0, 4, 0);
        lblStatus.Name = "lblStatus";
        lblStatus.Padding = new Padding(10);
        lblStatus.Size = new Size(1242, 20);
        lblStatus.TabIndex = 4;
        lblStatus.Text = "معاينة إدخال مؤقتة — الحفظ والترقيم التلقائي والقوائم غير متاحة؛ القيم لا تُطبّق على النظام.";
        lblStatus.TextAlign = ContentAlignment.MiddleRight;
        // 
        // fields
        // 
        fields.AutoSize = true;
        fields.ColumnCount = 2;
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68F));
        fields.Dock = DockStyle.Top;
        fields.Enabled = false;
        fields.Location = new Point(4, 64);
        fields.Margin = new Padding(4);
        fields.Name = "fields";
        fields.Size = new Size(1242, 0);
        fields.TabIndex = 1;
        // 
        // standardAuditMetadata
        // 
        standardAuditMetadata.AutoScroll = true;
        standardAuditMetadata.Dock = DockStyle.Bottom;
        standardAuditMetadata.Font = new Font("Tahoma", 9F);
        standardAuditMetadata.Location = new Point(0, 700);
        standardAuditMetadata.Margin = new Padding(0);
        standardAuditMetadata.MinimumSize = new Size(0, 100);
        standardAuditMetadata.Name = "standardAuditMetadata";
        standardAuditMetadata.RightToLeft = RightToLeft.Yes;
        standardAuditMetadata.Size = new Size(1250, 100);
        standardAuditMetadata.TabIndex = 7;
        standardAuditMetadata.TabStop = false;
        // 
        // UcTaxBracketCodes
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(designerCommandBar);
        Controls.Add(layout);
        Controls.Add(standardAuditMetadata);
        Margin = new Padding(4);
        Name = "UcTaxBracketCodes";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1250, 800);
        designerCommandBar.ResumeLayout(false);
        standardCommandFlow.ResumeLayout(false);
        standardCloseHost.ResumeLayout(false);
        layout.ResumeLayout(false);
        layout.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)gridTaxBrackets).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
