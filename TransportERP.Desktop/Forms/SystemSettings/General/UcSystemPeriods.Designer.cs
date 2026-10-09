namespace TransportERP.Desktop.Forms.SystemSettings.General;
partial class UcSystemPeriods
{
    private FlowLayoutPanel designerCommandFlow = null!;
    private Panel designerCloseHost = null!;
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

    private TableLayoutPanel periodCanvas = null!;
    private TableLayoutPanel periodFrame = null!;
    private TableLayoutPanel referenceLayout = null!;
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;
    private TableLayoutPanel referenceFields = null!;
    private DataGridViewTextBoxColumn colPeriod = null!;
    private DataGridViewTextBoxColumn colName = null!;
    private DataGridViewTextBoxColumn colForeignName = null!;

    private ComboBox cboPeriodType = null!;
    private Label lblcboPeriodType = null!;
    private TextBox txtPeriodCount = null!;
    private Label lbltxtPeriodCount = null!;
    private TextBox txtFromMonth = null!;
    private Label lbltxtFromMonth = null!;
    private TextBox txtToMonth = null!;
    private Label lbltxtToMonth = null!;
    private TextBox txtFromYear = null!;
    private Label lbltxtFromYear = null!;
    private TextBox txtToYear = null!;
    private Label lbltxtToYear = null!;
    private TransportERP.Desktop.CoreUI.DesignerCommandBar commandBar = null!;
    private DataGridView gridPeriods = null!;
    private Button btnLoadPeriods = null!;
    private Label lblTitle = null!;
    private Label lblStatus = null!;
    private DataGridViewTextBoxColumn colStart = null!;
    private DataGridViewTextBoxColumn colEnd = null!;

    private void InitializeComponent()
    {
        designerCommandFlow = new FlowLayoutPanel();
        designerCloseHost = new Panel();
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
        standardCommandClose = new Button();
        standardCommandRefresh = new Button();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
        cboPeriodType = new ComboBox();
        lblcboPeriodType = new Label();
        txtPeriodCount = new TextBox();
        lbltxtPeriodCount = new Label();
        txtFromMonth = new TextBox();
        lbltxtFromMonth = new Label();
        txtToMonth = new TextBox();
        lbltxtToMonth = new Label();
        txtFromYear = new TextBox();
        lbltxtFromYear = new Label();
        txtToYear = new TextBox();
        lbltxtToYear = new Label();
        referenceLayout = new TableLayoutPanel();
        standardAuditMetadata = new TransportERP.Desktop.CoreUI.AuditMetadataControl();
        referenceFields = new TableLayoutPanel();
        commandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        gridPeriods = new DataGridView();
        btnLoadPeriods = new Button();
        lblTitle = new Label();
        lblStatus = new Label();
        colStart = new DataGridViewTextBoxColumn();
        colEnd = new DataGridViewTextBoxColumn();
        SuspendLayout(); referenceLayout.SuspendLayout(); referenceFields.SuspendLayout();
        periodCanvas = new TableLayoutPanel();
        periodCanvas.Name = "periodCanvas";
        periodCanvas.Dock = DockStyle.Fill;
        periodCanvas.RightToLeft = RightToLeft.No;
        periodCanvas.ColumnCount = 3;
        periodCanvas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17F));
        periodCanvas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 74F));
        periodCanvas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9F));
        periodCanvas.RowCount = 3;
        periodCanvas.RowStyles.Add(new RowStyle(SizeType.Percent, 9F));
        periodCanvas.RowStyles.Add(new RowStyle(SizeType.Percent, 84F));
        periodCanvas.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
        periodFrame = new TableLayoutPanel();
        periodFrame.Name = "periodFrame";
        periodFrame.Dock = DockStyle.Fill;
        periodFrame.RightToLeft = RightToLeft.No;
        periodFrame.BorderStyle = BorderStyle.FixedSingle;
        periodFrame.ColumnCount = 3;
        periodFrame.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27F));
        periodFrame.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
        periodFrame.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));
        periodFrame.RowCount = 2;
        periodFrame.RowStyles.Add(new RowStyle(SizeType.Absolute, 136F));
        periodFrame.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));


        periodCanvas.Controls.Add(periodFrame, 1, 1);
        periodFrame.Controls.Add(referenceFields, 0, 0);
        periodFrame.SetColumnSpan(referenceFields, 3);
        periodFrame.Controls.Add(gridPeriods, 1, 1);
        referenceLayout.Controls.Add(commandBar, 0, 1);
        referenceLayout.Controls.Add(lblStatus, 0, 3);
        referenceLayout.Controls.Add(periodCanvas, 0, 2);
        referenceLayout.RowCount = 4;
        referenceLayout.RowStyles.Clear();
        referenceLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        referenceLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        referenceLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        referenceLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        referenceFields.AutoSize = false;
        referenceFields.Dock = DockStyle.Fill;
        referenceFields.RightToLeft = RightToLeft.No;
        referenceFields.BorderStyle = BorderStyle.FixedSingle;
        referenceFields.ColumnCount = 10;
        referenceFields.ColumnStyles.Clear();
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8F));
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8F));
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9F));
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8F));
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));
        referenceFields.RowCount = 2;
        referenceFields.RowStyles.Clear();
        referenceFields.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        referenceFields.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        referenceFields.Controls.Add(cboPeriodType, 2, 0);
        referenceFields.SetColumnSpan(cboPeriodType, 3);
        referenceFields.Controls.Add(lblcboPeriodType, 5, 0);
        referenceFields.SetColumnSpan(lblcboPeriodType, 2);
        referenceFields.Controls.Add(txtPeriodCount, 8, 0);
        referenceFields.Controls.Add(lbltxtPeriodCount, 9, 0);
        referenceFields.Controls.Add(txtFromMonth, 8, 1);
        referenceFields.Controls.Add(lbltxtFromMonth, 9, 1);
        referenceFields.Controls.Add(txtFromYear, 6, 1);
        referenceFields.Controls.Add(lbltxtFromYear, 7, 1);
        referenceFields.Controls.Add(txtToMonth, 4, 1);
        referenceFields.Controls.Add(lbltxtToMonth, 5, 1);
        referenceFields.Controls.Add(txtToYear, 2, 1);
        referenceFields.Controls.Add(lbltxtToYear, 3, 1);
        referenceFields.Controls.Add(btnLoadPeriods, 0, 1);
        btnLoadPeriods.Dock = DockStyle.Top;
        btnLoadPeriods.Height = 26;
        colPeriod = new DataGridViewTextBoxColumn();
        colPeriod.Name = "colPeriod";
        colPeriod.HeaderText = "الفترات";
        colPeriod.ReadOnly = true;
        colPeriod.SortMode = DataGridViewColumnSortMode.NotSortable;
        colName = new DataGridViewTextBoxColumn();
        colName.Name = "colName";
        colName.HeaderText = "الاسم";
        colName.SortMode = DataGridViewColumnSortMode.NotSortable;
        colForeignName = new DataGridViewTextBoxColumn();
        colForeignName.Name = "colForeignName";
        colForeignName.HeaderText = "الاسم الأجنبي";
        colForeignName.SortMode = DataGridViewColumnSortMode.NotSortable;

        gridPeriods.Columns.AddRange(colPeriod, colStart, colEnd, colName, colForeignName);
        colStart.HeaderText = "من تاريخ";
        colEnd.HeaderText = "إلى تاريخ";
        gridPeriods.MinimumSize = Size.Empty;
        gridPeriods.Dock = DockStyle.Fill;
        gridPeriods.RightToLeft = RightToLeft.Yes;
        gridPeriods.RowHeadersVisible = false;
        gridPeriods.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        colPeriod.FillWeight = 12F;
        colStart.FillWeight = 20F;
        colEnd.FillWeight = 20F;
        colName.FillWeight = 24F;
        colForeignName.FillWeight = 24F;
        lblTitle.Text = "تهيئة النظام · التهيئة · إعداد فترات النظام";
        lblStatus.Text = "GENS003 — توليد الفترات والحفظ غير متاحين";
        cboPeriodType.Name = "cboPeriodType";
        cboPeriodType.Dock = DockStyle.Fill;
        cboPeriodType.Margin = new Padding(2);
        cboPeriodType.MinimumSize = new Size(0, 24);
        cboPeriodType.TabIndex = 0;
        cboPeriodType.BackColor = Color.FromArgb(255, 255, 225);
        lblcboPeriodType.Name = "lblcboPeriodType";
        lblcboPeriodType.Text = "أنواع الفترات";
        lblcboPeriodType.Dock = DockStyle.Top;
        lblcboPeriodType.Height = 24;
        lblcboPeriodType.RightToLeft = RightToLeft.No;
        lblcboPeriodType.TextAlign = ContentAlignment.MiddleLeft;
        lblcboPeriodType.Margin = new Padding(2);
        lblcboPeriodType.TabStop = false;
        txtPeriodCount.Name = "txtPeriodCount";
        txtPeriodCount.Dock = DockStyle.Fill;
        txtPeriodCount.Margin = new Padding(2);
        txtPeriodCount.MinimumSize = new Size(0, 24);
        txtPeriodCount.TabIndex = 1;
        txtPeriodCount.BackColor = Color.FromArgb(255, 255, 225);
        txtPeriodCount.BorderStyle = BorderStyle.FixedSingle;
        lbltxtPeriodCount.Name = "lbltxtPeriodCount";
        lbltxtPeriodCount.Text = "عدد الفترات";
        lbltxtPeriodCount.Dock = DockStyle.Top;
        lbltxtPeriodCount.Height = 24;
        lbltxtPeriodCount.RightToLeft = RightToLeft.No;
        lbltxtPeriodCount.TextAlign = ContentAlignment.MiddleLeft;
        lbltxtPeriodCount.Margin = new Padding(2);
        lbltxtPeriodCount.TabStop = false;
        txtFromMonth.Name = "txtFromMonth";
        txtFromMonth.Dock = DockStyle.Fill;
        txtFromMonth.Margin = new Padding(2);
        txtFromMonth.MinimumSize = new Size(0, 24);
        txtFromMonth.TabIndex = 2;
        txtFromMonth.BackColor = Color.FromArgb(255, 255, 225);
        txtFromMonth.BorderStyle = BorderStyle.FixedSingle;
        lbltxtFromMonth.Name = "lbltxtFromMonth";
        lbltxtFromMonth.Text = "من شهر";
        lbltxtFromMonth.Dock = DockStyle.Top;
        lbltxtFromMonth.Height = 24;
        lbltxtFromMonth.RightToLeft = RightToLeft.No;
        lbltxtFromMonth.TextAlign = ContentAlignment.MiddleLeft;
        lbltxtFromMonth.Margin = new Padding(2);
        lbltxtFromMonth.TabStop = false;
        txtToMonth.Name = "txtToMonth";
        txtToMonth.Dock = DockStyle.Fill;
        txtToMonth.Margin = new Padding(2);
        txtToMonth.MinimumSize = new Size(0, 24);
        txtToMonth.TabIndex = 3;
        txtToMonth.BackColor = Color.FromArgb(255, 255, 225);
        txtToMonth.BorderStyle = BorderStyle.FixedSingle;
        lbltxtToMonth.Name = "lbltxtToMonth";
        lbltxtToMonth.Text = "إلى شهر";
        lbltxtToMonth.Dock = DockStyle.Top;
        lbltxtToMonth.Height = 24;
        lbltxtToMonth.RightToLeft = RightToLeft.No;
        lbltxtToMonth.TextAlign = ContentAlignment.MiddleLeft;
        lbltxtToMonth.Margin = new Padding(2);
        lbltxtToMonth.TabStop = false;
        txtFromYear.Name = "txtFromYear";
        txtFromYear.Dock = DockStyle.Fill;
        txtFromYear.Margin = new Padding(2);
        txtFromYear.MinimumSize = new Size(0, 24);
        txtFromYear.TabIndex = 4;
        txtFromYear.BackColor = Color.FromArgb(255, 255, 225);
        txtFromYear.BorderStyle = BorderStyle.FixedSingle;
        lbltxtFromYear.Name = "lbltxtFromYear";
        lbltxtFromYear.Text = "من سنة";
        lbltxtFromYear.Dock = DockStyle.Top;
        lbltxtFromYear.Height = 24;
        lbltxtFromYear.RightToLeft = RightToLeft.No;
        lbltxtFromYear.TextAlign = ContentAlignment.MiddleLeft;
        lbltxtFromYear.Margin = new Padding(2);
        lbltxtFromYear.TabStop = false;
        txtToYear.Name = "txtToYear";
        txtToYear.Dock = DockStyle.Fill;
        txtToYear.Margin = new Padding(2);
        txtToYear.MinimumSize = new Size(0, 24);
        txtToYear.TabIndex = 5;
        txtToYear.BackColor = Color.FromArgb(255, 255, 225);
        txtToYear.BorderStyle = BorderStyle.FixedSingle;
        lbltxtToYear.Name = "lbltxtToYear";
        lbltxtToYear.Text = "إلى سنة";
        lbltxtToYear.Dock = DockStyle.Top;
        lbltxtToYear.Height = 24;
        lbltxtToYear.RightToLeft = RightToLeft.No;
        lbltxtToYear.TextAlign = ContentAlignment.MiddleLeft;
        lbltxtToYear.Margin = new Padding(2);
        lbltxtToYear.TabStop = false;
        referenceLayout.Name = "layout";
        referenceLayout.Dock = DockStyle.Fill;
        referenceLayout.ColumnCount = 1;
        referenceLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        referenceLayout.Margin = Padding.Empty;
        referenceLayout.Controls.Add(lblTitle, 0, 0);
        referenceFields.Name = "fields";
        referenceFields.Enabled = false;
        referenceFields.Margin = Padding.Empty;
        referenceFields.Padding = new Padding(12, 24, 12, 20);
        periodCanvas.Margin = Padding.Empty;
        periodCanvas.Padding = Padding.Empty;
        periodFrame.Margin = Padding.Empty;
        periodFrame.Padding = Padding.Empty;
        commandBar.Name = "standardCommandBar";
        commandBar.Dock = DockStyle.Fill;
        commandBar.MinimumSize = new Size(0, 30);
        commandBar.Size = new Size(1000, 30);
        commandBar.Margin = Padding.Empty;
        lblTitle.Name = "lblTitle";
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.AutoSize = true;
        lblTitle.Padding = new Padding(3);
        lblTitle.Margin = Padding.Empty;
        lblStatus.Name = "lblStatus";
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.AutoSize = true;
        lblStatus.Padding = new Padding(3);
        lblStatus.Margin = Padding.Empty;
        cboPeriodType.DropDownStyle = ComboBoxStyle.DropDownList;
        cboPeriodType.Items.AddRange(new object[] { "فترات شهرية", "فترات المستخدم" });
        cboPeriodType.SelectedIndexChanged += periodType_SelectedIndexChanged;
        btnLoadPeriods.Name = "btnLoadPeriods";
        btnLoadPeriods.Text = "إنزال البيانات";
        btnLoadPeriods.Enabled = false;
        btnLoadPeriods.AccessibleDescription = "توليد الفترات غير مرتبط بخدمة بيانات.";
        colStart.Name = "colStart";
        colEnd.Name = "colEnd";
        colStart.SortMode = DataGridViewColumnSortMode.NotSortable;
        colEnd.SortMode = DataGridViewColumnSortMode.NotSortable;
        gridPeriods.Name = "gridPeriods";
        gridPeriods.RowTemplate.Height = 24;
        gridPeriods.Paint += gridPeriods_Paint;
        gridPeriods.AllowUserToAddRows = false;
        gridPeriods.AllowUserToDeleteRows = false;
        gridPeriods.Margin = new Padding(0, 26, 0, 35);
        gridPeriods.BackgroundColor = Color.FromArgb(244, 244, 244);
        gridPeriods.BorderStyle = BorderStyle.None;
        gridPeriods.GridColor = Color.FromArgb(176, 176, 176);
        gridPeriods.EnableHeadersVisualStyles = false;
        gridPeriods.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(244, 244, 244);
        gridPeriods.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        gridPeriods.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 225);
        gridPeriods.DefaultCellStyle.SelectionBackColor = Color.FromArgb(216, 255, 255);
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScroll = true;
        Font = new Font("Tahoma", 9F);
        BackColor = Color.FromArgb(244, 244, 244);
        ForeColor = Color.FromArgb(45, 45, 45);
        Name = "UcSystemPeriods";
        Text = "إعداد فترات النظام";
        RightToLeft = RightToLeft.Yes;
        Dock = DockStyle.Fill;
        Margin = Padding.Empty;
        Padding = new Padding(2);
        Size = new Size(1000, 680);
        standardAuditMetadata.Name = "standardAuditMetadata";
        standardAuditMetadata.Dock = DockStyle.Bottom;
        standardAuditMetadata.Margin = Padding.Empty;
        standardAuditMetadata.Size = new Size(1000, 64);
        Controls.Add(referenceLayout);
        Controls.Add(standardAuditMetadata);
        referenceFields.ResumeLayout(false); referenceLayout.ResumeLayout(false); ResumeLayout(false);
    
        // Explicit Designer-owned command catalog.
        commandBar.Name = "commandBar";
        commandBar.BackColor = Color.FromArgb(239, 239, 239);
        commandBar.BorderStyle = BorderStyle.FixedSingle;
        commandBar.Controls.Add(designerCommandFlow);
        commandBar.Controls.Add(designerCloseHost);
        designerCloseHost.Dock = DockStyle.Left;
        designerCloseHost.Width = 28;
        designerCloseHost.Name = "designerCloseHost";
        designerCommandFlow.Name = "designerCommandFlow";
        designerCommandFlow.Dock = DockStyle.Fill;
        designerCommandFlow.AutoSize = false;
        designerCommandFlow.WrapContents = false;
        designerCommandFlow.FlowDirection = FlowDirection.RightToLeft;
        designerCommandFlow.RightToLeft = RightToLeft.No;
        designerCommandFlow.Padding = new Padding(2);
        commandBar.MinimumSize = new Size(0, 30);
        commandBar.Height = 30;
        standardCommandAdd.Name = "standardCommandAdd";
        standardCommandAdd.Enabled = false;
        standardCommandAdd.Visible = true;
        standardCommandAdd.AccessibleName = "إضافة";
        standardCommandEdit.Name = "standardCommandEdit";
        standardCommandEdit.Enabled = false;
        standardCommandEdit.Visible = true;
        standardCommandEdit.AccessibleName = "تعديل";
        standardCommandDelete.Name = "standardCommandDelete";
        standardCommandDelete.Enabled = false;
        standardCommandDelete.Visible = true;
        standardCommandDelete.AccessibleName = "حذف";
        standardCommandCancel.Name = "standardCommandCancel";
        standardCommandCancel.Enabled = false;
        standardCommandCancel.Visible = false;
        standardCommandCancel.AccessibleName = "تراجع";
        standardCommandView.Name = "standardCommandView";
        standardCommandView.Enabled = false;
        standardCommandView.Visible = false;
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
        standardCommandSave.Name = "standardCommandSave";
        standardCommandSave.Enabled = false;
        standardCommandSave.Visible = true;
        standardCommandSave.AccessibleName = "حفظ";
        standardCommandPrint.Name = "standardCommandPrint";
        standardCommandPrint.Enabled = false;
        standardCommandPrint.Visible = false;
        standardCommandPrint.AccessibleName = "طباعة";
        standardCommandClose.Name = "standardCommandClose";
        standardCommandClose.Enabled = false;
        standardCommandClose.Visible = true;
        standardCommandClose.AccessibleName = "إغلاق";
        standardCommandRefresh.Name = "standardCommandRefresh";
        standardCommandRefresh.Enabled = false;
        standardCommandRefresh.Visible = false;
        standardCommandRefresh.AccessibleName = "تحديث";
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
        standardCommandAdd.AutoSize = false;
        standardCommandAdd.Dock = DockStyle.None;
        standardCommandAdd.MinimumSize = Size.Empty;
        standardCommandAdd.Size = new Size(26, 24);
        standardCommandAdd.Margin = new Padding(1);
        standardCommandAdd.FlatStyle = FlatStyle.Flat;
        standardCommandAdd.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandAdd.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Add;
        standardCommandAdd.Text = "";
        designerCommandFlow.Controls.Add(standardCommandAdd);
        commandBar.SetCommandRole(standardCommandAdd, TransportERP.Desktop.CoreUI.DesignerCommandRole.Add);
        standardCommandEdit.AutoSize = false;
        standardCommandEdit.Dock = DockStyle.None;
        standardCommandEdit.MinimumSize = Size.Empty;
        standardCommandEdit.Size = new Size(26, 24);
        standardCommandEdit.Margin = new Padding(1);
        standardCommandEdit.FlatStyle = FlatStyle.Flat;
        standardCommandEdit.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandEdit.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Edit;
        standardCommandEdit.Text = "";
        designerCommandFlow.Controls.Add(standardCommandEdit);
        commandBar.SetCommandRole(standardCommandEdit, TransportERP.Desktop.CoreUI.DesignerCommandRole.Edit);
        standardCommandDelete.AutoSize = false;
        standardCommandDelete.Dock = DockStyle.None;
        standardCommandDelete.MinimumSize = Size.Empty;
        standardCommandDelete.Size = new Size(26, 24);
        standardCommandDelete.Margin = new Padding(1);
        standardCommandDelete.FlatStyle = FlatStyle.Flat;
        standardCommandDelete.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandDelete.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Delete;
        standardCommandDelete.Text = "";
        designerCommandFlow.Controls.Add(standardCommandDelete);
        commandBar.SetCommandRole(standardCommandDelete, TransportERP.Desktop.CoreUI.DesignerCommandRole.Delete);
        standardCommandCancel.AutoSize = false;
        standardCommandCancel.Dock = DockStyle.None;
        standardCommandCancel.MinimumSize = Size.Empty;
        standardCommandCancel.Size = new Size(26, 24);
        standardCommandCancel.Margin = new Padding(1);
        standardCommandCancel.FlatStyle = FlatStyle.Flat;
        standardCommandCancel.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandCancel.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Cancel;
        standardCommandCancel.Text = "";
        designerCommandFlow.Controls.Add(standardCommandCancel);
        commandBar.SetCommandRole(standardCommandCancel, TransportERP.Desktop.CoreUI.DesignerCommandRole.Cancel);
        standardCommandView.AutoSize = false;
        standardCommandView.Dock = DockStyle.None;
        standardCommandView.MinimumSize = Size.Empty;
        standardCommandView.Size = new Size(26, 24);
        standardCommandView.Margin = new Padding(1);
        standardCommandView.FlatStyle = FlatStyle.Flat;
        standardCommandView.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandView.Image = TransportERP.Desktop.CoreUI.CommandBarImages.View;
        standardCommandView.Text = "";
        designerCommandFlow.Controls.Add(standardCommandView);
        commandBar.SetCommandRole(standardCommandView, TransportERP.Desktop.CoreUI.DesignerCommandRole.View);
        standardCommandLast.AutoSize = false;
        standardCommandLast.Dock = DockStyle.None;
        standardCommandLast.MinimumSize = Size.Empty;
        standardCommandLast.Size = new Size(26, 24);
        standardCommandLast.Margin = new Padding(1);
        standardCommandLast.FlatStyle = FlatStyle.Flat;
        standardCommandLast.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandLast.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Last;
        standardCommandLast.Text = "";
        designerCommandFlow.Controls.Add(standardCommandLast);
        commandBar.SetCommandRole(standardCommandLast, TransportERP.Desktop.CoreUI.DesignerCommandRole.Last);
        standardCommandNext.AutoSize = false;
        standardCommandNext.Dock = DockStyle.None;
        standardCommandNext.MinimumSize = Size.Empty;
        standardCommandNext.Size = new Size(26, 24);
        standardCommandNext.Margin = new Padding(1);
        standardCommandNext.FlatStyle = FlatStyle.Flat;
        standardCommandNext.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandNext.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Next;
        standardCommandNext.Text = "";
        designerCommandFlow.Controls.Add(standardCommandNext);
        commandBar.SetCommandRole(standardCommandNext, TransportERP.Desktop.CoreUI.DesignerCommandRole.Next);
        standardCommandPrevious.AutoSize = false;
        standardCommandPrevious.Dock = DockStyle.None;
        standardCommandPrevious.MinimumSize = Size.Empty;
        standardCommandPrevious.Size = new Size(26, 24);
        standardCommandPrevious.Margin = new Padding(1);
        standardCommandPrevious.FlatStyle = FlatStyle.Flat;
        standardCommandPrevious.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandPrevious.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Previous;
        standardCommandPrevious.Text = "";
        designerCommandFlow.Controls.Add(standardCommandPrevious);
        commandBar.SetCommandRole(standardCommandPrevious, TransportERP.Desktop.CoreUI.DesignerCommandRole.Previous);
        standardCommandFirst.AutoSize = false;
        standardCommandFirst.Dock = DockStyle.None;
        standardCommandFirst.MinimumSize = Size.Empty;
        standardCommandFirst.Size = new Size(26, 24);
        standardCommandFirst.Margin = new Padding(1);
        standardCommandFirst.FlatStyle = FlatStyle.Flat;
        standardCommandFirst.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandFirst.Image = TransportERP.Desktop.CoreUI.CommandBarImages.First;
        standardCommandFirst.Text = "";
        designerCommandFlow.Controls.Add(standardCommandFirst);
        commandBar.SetCommandRole(standardCommandFirst, TransportERP.Desktop.CoreUI.DesignerCommandRole.First);
        standardCommandSave.AutoSize = false;
        standardCommandSave.Dock = DockStyle.None;
        standardCommandSave.MinimumSize = Size.Empty;
        standardCommandSave.Size = new Size(26, 24);
        standardCommandSave.Margin = new Padding(1);
        standardCommandSave.FlatStyle = FlatStyle.Flat;
        standardCommandSave.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandSave.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Save;
        standardCommandSave.Text = "";
        designerCommandFlow.Controls.Add(standardCommandSave);
        commandBar.SetCommandRole(standardCommandSave, TransportERP.Desktop.CoreUI.DesignerCommandRole.Save);
        standardCommandPrint.AutoSize = false;
        standardCommandPrint.Dock = DockStyle.None;
        standardCommandPrint.MinimumSize = Size.Empty;
        standardCommandPrint.Size = new Size(26, 24);
        standardCommandPrint.Margin = new Padding(1);
        standardCommandPrint.FlatStyle = FlatStyle.Flat;
        standardCommandPrint.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandPrint.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Print;
        standardCommandPrint.Text = "";
        designerCommandFlow.Controls.Add(standardCommandPrint);
        commandBar.SetCommandRole(standardCommandPrint, TransportERP.Desktop.CoreUI.DesignerCommandRole.Print);
        standardCommandClose.AutoSize = false;
        standardCommandClose.Dock = DockStyle.None;
        standardCommandClose.MinimumSize = Size.Empty;
        standardCommandClose.Size = new Size(26, 24);
        standardCommandClose.Margin = new Padding(1);
        standardCommandClose.FlatStyle = FlatStyle.Flat;
        standardCommandClose.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandClose.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Close;
        standardCommandClose.Text = "";
        designerCloseHost.Controls.Add(standardCommandClose);
        standardCommandClose.Location = new Point(1, 1);
        commandBar.SetCommandRole(standardCommandClose, TransportERP.Desktop.CoreUI.DesignerCommandRole.Close);
        standardCommandRefresh.AutoSize = false;
        standardCommandRefresh.Dock = DockStyle.None;
        standardCommandRefresh.MinimumSize = Size.Empty;
        standardCommandRefresh.Size = new Size(26, 24);
        standardCommandRefresh.Margin = new Padding(1);
        standardCommandRefresh.FlatStyle = FlatStyle.Flat;
        standardCommandRefresh.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandRefresh.Text = "تحديث";
        designerCommandFlow.Controls.Add(standardCommandRefresh);
        commandBar.SetCommandRole(standardCommandRefresh, TransportERP.Desktop.CoreUI.DesignerCommandRole.Refresh);
        standardCommandImport.AutoSize = false;
        standardCommandImport.Dock = DockStyle.None;
        standardCommandImport.MinimumSize = Size.Empty;
        standardCommandImport.Size = new Size(26, 24);
        standardCommandImport.Margin = new Padding(1);
        standardCommandImport.FlatStyle = FlatStyle.Flat;
        standardCommandImport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandImport.Text = "استيراد";
        designerCommandFlow.Controls.Add(standardCommandImport);
        commandBar.SetCommandRole(standardCommandImport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Import);
        standardCommandExport.AutoSize = false;
        standardCommandExport.Dock = DockStyle.None;
        standardCommandExport.MinimumSize = Size.Empty;
        standardCommandExport.Size = new Size(26, 24);
        standardCommandExport.Margin = new Padding(1);
        standardCommandExport.FlatStyle = FlatStyle.Flat;
        standardCommandExport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandExport.Text = "تصدير";
        designerCommandFlow.Controls.Add(standardCommandExport);
        commandBar.SetCommandRole(standardCommandExport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Export);
        standardCommandHelp.AutoSize = false;
        standardCommandHelp.Dock = DockStyle.None;
        standardCommandHelp.MinimumSize = Size.Empty;
        standardCommandHelp.Size = new Size(26, 24);
        standardCommandHelp.Margin = new Padding(1);
        standardCommandHelp.FlatStyle = FlatStyle.Flat;
        standardCommandHelp.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandHelp.Text = "مساعدة";
        designerCommandFlow.Controls.Add(standardCommandHelp);
        commandBar.SetCommandRole(standardCommandHelp, TransportERP.Desktop.CoreUI.DesignerCommandRole.Help);
    }
}
