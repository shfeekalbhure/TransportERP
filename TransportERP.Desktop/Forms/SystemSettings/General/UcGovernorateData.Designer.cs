namespace TransportERP.Desktop.Forms.SystemSettings.General;

partial class UcGovernorateData
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

    private TableLayoutPanel governorateCanvas = null!;
    private TableLayoutPanel referenceLayout = null!;
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;
    private TableLayoutPanel referenceFields = null!;
    private TransportERP.Desktop.CoreUI.DesignerCommandBar commandBar = null!;
    private TextBox txtGovernorateNumber = null!;
    private TextBox txtGovernorateNameLocal = null!;
    private TextBox txtGovernorateNameForeign = null!;
    private TextBox txtShortName = null!;
    private Label lbltxtGovernorateNumber = null!;
    private Label lbltxtGovernorateNameLocal = null!;
    private Label lbltxtGovernorateNameForeign = null!;
    private Label lbltxtShortName = null!;
    private ComboBox cboCountryNumber = null!;
    private TextBox txtRegionNumber = null!;
    private ComboBox cboRegionDisplay = null!;
    private Label lblcboCountryNumber = null!;
    private Label lbltxtRegionNumber = null!;
    private Label lblTitle = null!;
    private Label lblStatus = null!;

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
        referenceLayout = new TableLayoutPanel();
        standardAuditMetadata = new TransportERP.Desktop.CoreUI.AuditMetadataControl();
        referenceFields = new TableLayoutPanel();
        commandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        txtGovernorateNumber = new TextBox();
        txtGovernorateNameLocal = new TextBox();
        txtGovernorateNameForeign = new TextBox();
        txtShortName = new TextBox();
        lbltxtGovernorateNumber = new Label();
        lbltxtGovernorateNameLocal = new Label();
        lbltxtGovernorateNameForeign = new Label();
        lbltxtShortName = new Label();
        cboCountryNumber = new ComboBox();
        txtRegionNumber = new TextBox();
        cboRegionDisplay = new ComboBox();
        lblcboCountryNumber = new Label();
        lbltxtRegionNumber = new Label();
        lblTitle = new Label();
        lblStatus = new Label();
        SuspendLayout();
        referenceLayout.SuspendLayout();
        referenceFields.SuspendLayout();
        governorateCanvas = new TableLayoutPanel();
        governorateCanvas.Name = "governorateCanvas";
        governorateCanvas.Dock = DockStyle.Fill;
        governorateCanvas.RightToLeft = RightToLeft.No;
        governorateCanvas.Margin = Padding.Empty;
        governorateCanvas.Padding = Padding.Empty;
        governorateCanvas.ColumnCount = 3;
        governorateCanvas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5F));
        governorateCanvas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90F));
        governorateCanvas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5F));
        governorateCanvas.RowCount = 3;
        governorateCanvas.RowStyles.Add(new RowStyle(SizeType.Percent, 18F));
        governorateCanvas.RowStyles.Add(new RowStyle(SizeType.Percent, 53F));
        governorateCanvas.RowStyles.Add(new RowStyle(SizeType.Percent, 29F));

        governorateCanvas.Controls.Add(referenceFields, 1, 1);
        referenceLayout.Controls.Add(commandBar, 0, 1);
        referenceLayout.Controls.Add(lblStatus, 0, 3);
        referenceLayout.Controls.Add(governorateCanvas, 0, 2);
        referenceLayout.RowCount = 4;
        referenceLayout.RowStyles.Clear();
        referenceLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        referenceLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        referenceLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        referenceLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        referenceFields.AutoSize = false;
        referenceFields.Dock = DockStyle.Fill;
        referenceFields.BorderStyle = BorderStyle.FixedSingle;
        referenceFields.RightToLeft = RightToLeft.No;
        referenceFields.ColumnCount = 6;
        referenceFields.ColumnStyles.Clear();
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 21F));
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19F));
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
        referenceFields.RowCount = 7;
        referenceFields.RowStyles.Clear();
        referenceFields.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
        referenceFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        referenceFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        referenceFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        referenceFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        referenceFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        referenceFields.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
        referenceFields.Controls.Add(txtGovernorateNumber, 3, 1);
        referenceFields.Controls.Add(lbltxtGovernorateNumber, 4, 1);
        referenceFields.Controls.Add(txtGovernorateNameLocal, 1, 2);
        referenceFields.Controls.Add(lbltxtGovernorateNameLocal, 4, 2);
        referenceFields.Controls.Add(txtGovernorateNameForeign, 1, 3);
        referenceFields.Controls.Add(lbltxtGovernorateNameForeign, 4, 3);
        referenceFields.Controls.Add(txtShortName, 3, 4);
        referenceFields.Controls.Add(lbltxtShortName, 4, 4);
        referenceFields.SetColumnSpan(txtGovernorateNameLocal, 3);
        referenceFields.SetColumnSpan(txtGovernorateNameForeign, 3);
        referenceFields.Controls.Add(cboCountryNumber, 3, 5);
        referenceFields.Controls.Add(lblcboCountryNumber, 4, 5);
        referenceFields.Controls.Add(cboRegionDisplay, 1, 5);
        governorateCanvas.Controls.Add(txtRegionNumber, 0, 0);
        referenceFields.Controls.Add(lbltxtRegionNumber, 2, 5);
        cboCountryNumber.Name = "cboCountryNumber";
        cboCountryNumber.AccessibleName = "رقم الدولة";
        cboCountryNumber.AccessibleDescription = "القائمة غير مرتبطة ببيانات بعد.";
        cboCountryNumber.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCountryNumber.Enabled = false;
        cboCountryNumber.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cboCountryNumber.Size = new Size(110, 24);
        cboCountryNumber.Margin = new Padding(1);
        cboCountryNumber.TabIndex = 4;
        txtRegionNumber.Name = "txtRegionNumber";
        txtRegionNumber.AccessibleName = "رقم الإقليم";
        txtRegionNumber.AccessibleDescription = "قيمة تلقائية؛ تظهر بعد ربط القوائم بخدمة البيانات.";
        txtRegionNumber.ReadOnly = true;
        txtRegionNumber.BackColor = Color.White;
        txtRegionNumber.BorderStyle = BorderStyle.FixedSingle;
        txtRegionNumber.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        txtRegionNumber.Size = new Size(110, 24);
        txtRegionNumber.Margin = new Padding(1);
        txtRegionNumber.TabIndex = 5;
        txtRegionNumber.Visible = false;
        txtRegionNumber.TextChanged += txtRegionNumber_TextChanged;
        // Unnamed presentation editor: FoundationUiSession collects only named editors.
        // The hidden txtRegionNumber remains the sole business key and load target.
        cboRegionDisplay.Name = "";
        cboRegionDisplay.AccessibleName = "رقم الإقليم";
        cboRegionDisplay.AccessibleDescription = "عرض القيمة المشتقة من الدولة؛ لا توجد خيارات غير موصولة.";
        cboRegionDisplay.DropDownStyle = ComboBoxStyle.DropDownList;
        cboRegionDisplay.Enabled = false;
        cboRegionDisplay.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        cboRegionDisplay.Size = new Size(110, 24);
        cboRegionDisplay.Margin = new Padding(1);
        cboRegionDisplay.TabIndex = 5;
        lblcboCountryNumber.Name = "lblcboCountryNumber";
        lblcboCountryNumber.Text = "رقم الدولة";
        lblcboCountryNumber.Dock = DockStyle.Fill;
        lblcboCountryNumber.TextAlign = ContentAlignment.MiddleLeft;
        lblcboCountryNumber.RightToLeft = RightToLeft.No;
        lblcboCountryNumber.Margin = new Padding(1);
        lbltxtRegionNumber.Name = "lbltxtRegionNumber";
        lbltxtRegionNumber.Text = "رقم الإقليم";
        lbltxtRegionNumber.Dock = DockStyle.Fill;
        lbltxtRegionNumber.TextAlign = ContentAlignment.MiddleLeft;
        lbltxtRegionNumber.RightToLeft = RightToLeft.No;
        lbltxtRegionNumber.Margin = new Padding(1);
        txtGovernorateNumber.TabIndex = 0;
        txtGovernorateNameLocal.TabIndex = 1;
        txtGovernorateNameForeign.TabIndex = 2;
        txtShortName.TabIndex = 3;
        txtGovernorateNumber.Dock = DockStyle.None;
        txtGovernorateNumber.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtGovernorateNumber.Size = new Size(110, 24);
        txtGovernorateNumber.BackColor = Color.FromArgb(255, 255, 225);
        txtGovernorateNameLocal.Dock = DockStyle.Fill;
        txtGovernorateNameLocal.BackColor = Color.FromArgb(255, 255, 225);
        txtGovernorateNameForeign.Dock = DockStyle.Fill;
        txtGovernorateNameForeign.BackColor = Color.White;
        txtGovernorateNameForeign.RightToLeft = RightToLeft.No;
        txtGovernorateNameForeign.TextAlign = HorizontalAlignment.Left;
        txtShortName.Dock = DockStyle.None;
        txtShortName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtShortName.Size = new Size(110, 24);
        txtShortName.BackColor = Color.FromArgb(255, 255, 225);
        lbltxtGovernorateNameLocal.Text = "اسم المحافظة";
        lbltxtGovernorateNameForeign.Text = "الاسم الأجنبي";
        lblTitle.Text = "تهيئة النظام · التهيئة · بيانات المحافظات";
        lblStatus.Text = "GENS006 — الحفظ غير متاح";
        txtGovernorateNumber.Name = "txtGovernorateNumber";
        txtGovernorateNumber.AccessibleName = "رقم المحافظة";
        txtGovernorateNumber.BorderStyle = BorderStyle.FixedSingle;
        txtGovernorateNumber.Margin = new Padding(1);
        txtGovernorateNumber.MinimumSize = new Size(0, 24);
        lbltxtGovernorateNumber.Name = "lbltxtGovernorateNumber";
        lbltxtGovernorateNumber.Text = "رقم المحافظة";
        lbltxtGovernorateNumber.Dock = DockStyle.Fill;
        lbltxtGovernorateNumber.RightToLeft = RightToLeft.No;
        lbltxtGovernorateNumber.TextAlign = ContentAlignment.MiddleLeft;
        lbltxtGovernorateNumber.Margin = new Padding(1);
        lbltxtGovernorateNumber.TabStop = false;
        txtGovernorateNameLocal.Name = "txtGovernorateNameLocal";
        txtGovernorateNameLocal.AccessibleName = "اسم المحافظة";
        txtGovernorateNameLocal.BorderStyle = BorderStyle.FixedSingle;
        txtGovernorateNameLocal.Margin = new Padding(1);
        txtGovernorateNameLocal.MinimumSize = new Size(0, 24);
        lbltxtGovernorateNameLocal.Name = "lbltxtGovernorateNameLocal";
        lbltxtGovernorateNameLocal.Text = "اسم المحافظة";
        lbltxtGovernorateNameLocal.Dock = DockStyle.Fill;
        lbltxtGovernorateNameLocal.RightToLeft = RightToLeft.No;
        lbltxtGovernorateNameLocal.TextAlign = ContentAlignment.MiddleLeft;
        lbltxtGovernorateNameLocal.Margin = new Padding(1);
        lbltxtGovernorateNameLocal.TabStop = false;
        txtGovernorateNameForeign.Name = "txtGovernorateNameForeign";
        txtGovernorateNameForeign.AccessibleName = "الاسم الأجنبي";
        txtGovernorateNameForeign.BorderStyle = BorderStyle.FixedSingle;
        txtGovernorateNameForeign.Margin = new Padding(1);
        txtGovernorateNameForeign.MinimumSize = new Size(0, 24);
        lbltxtGovernorateNameForeign.Name = "lbltxtGovernorateNameForeign";
        lbltxtGovernorateNameForeign.Text = "الاسم الأجنبي";
        lbltxtGovernorateNameForeign.Dock = DockStyle.Fill;
        lbltxtGovernorateNameForeign.RightToLeft = RightToLeft.No;
        lbltxtGovernorateNameForeign.TextAlign = ContentAlignment.MiddleLeft;
        lbltxtGovernorateNameForeign.Margin = new Padding(1);
        lbltxtGovernorateNameForeign.TabStop = false;
        txtShortName.Name = "txtShortName";
        txtShortName.AccessibleName = "الاسم المختصر";
        txtShortName.BorderStyle = BorderStyle.FixedSingle;
        txtShortName.Margin = new Padding(1);
        txtShortName.MinimumSize = new Size(0, 24);
        lbltxtShortName.Name = "lbltxtShortName";
        lbltxtShortName.Text = "الاسم المختصر";
        lbltxtShortName.Dock = DockStyle.Fill;
        lbltxtShortName.RightToLeft = RightToLeft.No;
        lbltxtShortName.TextAlign = ContentAlignment.MiddleLeft;
        lbltxtShortName.Margin = new Padding(1);
        lbltxtShortName.TabStop = false;
        referenceLayout.Name = "layout";
        referenceLayout.Dock = DockStyle.Fill;
        referenceLayout.ColumnCount = 1;
        referenceLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        referenceLayout.Margin = Padding.Empty;
        referenceLayout.Controls.Add(lblTitle, 0, 0);
        referenceFields.Name = "fields";
        referenceFields.Enabled = false;
        referenceFields.Margin = Padding.Empty;
        referenceFields.Padding = Padding.Empty;
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
        txtGovernorateNumber.TextAlign = HorizontalAlignment.Right;
        txtGovernorateNameLocal.TextAlign = HorizontalAlignment.Right;
        txtShortName.TextAlign = HorizontalAlignment.Right;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScroll = true;
        Font = new Font("Tahoma", 9F);
        BackColor = Color.FromArgb(244, 244, 244);
        ForeColor = Color.FromArgb(45, 45, 45);
        Name = "UcGovernorateData";
        Text = "بيانات المحافظات";
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
        referenceFields.ResumeLayout(false);
        referenceLayout.ResumeLayout(false);
        ResumeLayout(false);
    
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
        standardCommandSave.Name = "standardCommandSave";
        standardCommandSave.Enabled = false;
        standardCommandSave.Visible = true;
        standardCommandSave.AccessibleName = "حفظ";
        standardCommandPrint.Name = "standardCommandPrint";
        standardCommandPrint.Enabled = false;
        standardCommandPrint.Visible = true;
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

