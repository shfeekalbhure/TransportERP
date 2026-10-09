namespace TransportERP.Desktop.Forms.SystemSettings.General;

partial class UcInternationalRegions
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

    private TableLayoutPanel regionCanvas = null!;
    private TableLayoutPanel referenceLayout = null!;
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;
    private TableLayoutPanel referenceFields = null!;
    private TransportERP.Desktop.CoreUI.DesignerCommandBar commandBar = null!;
    private TextBox txtRegionNumber = null!;
    private TextBox txtRegionNameLocal = null!;
    private TextBox txtRegionNameForeign = null!;
    private TextBox txtShortName = null!;
    private Label lbltxtRegionNumber = null!;
    private Label lbltxtRegionNameLocal = null!;
    private Label lbltxtRegionNameForeign = null!;
    private Label lbltxtShortName = null!;
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
        txtRegionNumber = new TextBox();
        txtRegionNameLocal = new TextBox();
        txtRegionNameForeign = new TextBox();
        txtShortName = new TextBox();
        lbltxtRegionNumber = new Label();
        lbltxtRegionNameLocal = new Label();
        lbltxtRegionNameForeign = new Label();
        lbltxtShortName = new Label();
        lblTitle = new Label();
        lblStatus = new Label();
        SuspendLayout();
        referenceLayout.SuspendLayout();
        referenceFields.SuspendLayout();
        regionCanvas = new TableLayoutPanel();
        regionCanvas.Name = "regionCanvas";
        regionCanvas.Dock = DockStyle.Fill;
        regionCanvas.RightToLeft = RightToLeft.No;
        regionCanvas.Margin = Padding.Empty;
        regionCanvas.Padding = Padding.Empty;
        regionCanvas.ColumnCount = 3;
        regionCanvas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5F));
        regionCanvas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90F));
        regionCanvas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5F));
        regionCanvas.RowCount = 3;
        regionCanvas.RowStyles.Add(new RowStyle(SizeType.Percent, 18F));
        regionCanvas.RowStyles.Add(new RowStyle(SizeType.Percent, 53F));
        regionCanvas.RowStyles.Add(new RowStyle(SizeType.Percent, 29F));

        regionCanvas.Controls.Add(referenceFields, 1, 1);
        referenceLayout.Controls.Add(commandBar, 0, 1);
        referenceLayout.Controls.Add(lblStatus, 0, 3);
        referenceLayout.Controls.Add(regionCanvas, 0, 2);
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
        referenceFields.ColumnCount = 4;
        referenceFields.ColumnStyles.Clear();
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 21F));
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 53F));
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
        referenceFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
        referenceFields.RowCount = 6;
        referenceFields.RowStyles.Clear();
        referenceFields.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
        referenceFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        referenceFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        referenceFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        referenceFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        referenceFields.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
        referenceFields.Controls.Add(txtRegionNumber, 1, 1);
        referenceFields.Controls.Add(lbltxtRegionNumber, 2, 1);
        referenceFields.Controls.Add(txtRegionNameLocal, 1, 2);
        referenceFields.Controls.Add(lbltxtRegionNameLocal, 2, 2);
        referenceFields.Controls.Add(txtRegionNameForeign, 1, 3);
        referenceFields.Controls.Add(lbltxtRegionNameForeign, 2, 3);
        referenceFields.Controls.Add(txtShortName, 1, 4);
        referenceFields.Controls.Add(lbltxtShortName, 2, 4);
        txtRegionNumber.Dock = DockStyle.None;
        txtRegionNumber.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtRegionNumber.Size = new Size(110, 24);
        txtRegionNumber.BackColor = Color.FromArgb(255, 255, 225);
        txtRegionNameLocal.Dock = DockStyle.Fill;
        txtRegionNameLocal.BackColor = Color.FromArgb(255, 255, 225);
        txtRegionNameForeign.Dock = DockStyle.Fill;
        txtRegionNameForeign.BackColor = Color.White;
        txtRegionNameForeign.RightToLeft = RightToLeft.No;
        txtRegionNameForeign.TextAlign = HorizontalAlignment.Left;
        txtShortName.Dock = DockStyle.None;
        txtShortName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtShortName.Size = new Size(110, 24);
        txtShortName.BackColor = Color.FromArgb(255, 255, 225);
        lbltxtRegionNameLocal.Text = "اسم الإقليم";
        lbltxtRegionNameForeign.Text = "الاسم الأجنبي";
        lblTitle.Text = "تهيئة النظام · التهيئة · الأقاليم الدولية";
        lblStatus.Text = "GENS015 — الحفظ غير متاح";
        txtRegionNumber.Name = "txtRegionNumber";
        txtRegionNumber.AccessibleName = "رقم الإقليم";
        txtRegionNumber.BorderStyle = BorderStyle.FixedSingle;
        txtRegionNumber.Margin = new Padding(1);
        txtRegionNumber.MinimumSize = new Size(0, 24);
        lbltxtRegionNumber.Name = "lbltxtRegionNumber";
        lbltxtRegionNumber.Text = "رقم الإقليم";
        lbltxtRegionNumber.Dock = DockStyle.Fill;
        lbltxtRegionNumber.RightToLeft = RightToLeft.No;
        lbltxtRegionNumber.TextAlign = ContentAlignment.MiddleLeft;
        lbltxtRegionNumber.Margin = new Padding(1);
        lbltxtRegionNumber.TabStop = false;
        txtRegionNameLocal.Name = "txtRegionNameLocal";
        txtRegionNameLocal.AccessibleName = "اسم الإقليم";
        txtRegionNameLocal.BorderStyle = BorderStyle.FixedSingle;
        txtRegionNameLocal.Margin = new Padding(1);
        txtRegionNameLocal.MinimumSize = new Size(0, 24);
        lbltxtRegionNameLocal.Name = "lbltxtRegionNameLocal";
        lbltxtRegionNameLocal.Text = "اسم الإقليم";
        lbltxtRegionNameLocal.Dock = DockStyle.Fill;
        lbltxtRegionNameLocal.RightToLeft = RightToLeft.No;
        lbltxtRegionNameLocal.TextAlign = ContentAlignment.MiddleLeft;
        lbltxtRegionNameLocal.Margin = new Padding(1);
        lbltxtRegionNameLocal.TabStop = false;
        txtRegionNameForeign.Name = "txtRegionNameForeign";
        txtRegionNameForeign.AccessibleName = "الاسم الأجنبي";
        txtRegionNameForeign.BorderStyle = BorderStyle.FixedSingle;
        txtRegionNameForeign.Margin = new Padding(1);
        txtRegionNameForeign.MinimumSize = new Size(0, 24);
        lbltxtRegionNameForeign.Name = "lbltxtRegionNameForeign";
        lbltxtRegionNameForeign.Text = "الاسم الأجنبي";
        lbltxtRegionNameForeign.Dock = DockStyle.Fill;
        lbltxtRegionNameForeign.RightToLeft = RightToLeft.No;
        lbltxtRegionNameForeign.TextAlign = ContentAlignment.MiddleLeft;
        lbltxtRegionNameForeign.Margin = new Padding(1);
        lbltxtRegionNameForeign.TabStop = false;
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
        txtRegionNumber.TextAlign = HorizontalAlignment.Right;
        txtRegionNameLocal.TextAlign = HorizontalAlignment.Right;
        txtShortName.TextAlign = HorizontalAlignment.Right;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScroll = true;
        Font = new Font("Tahoma", 9F);
        BackColor = Color.FromArgb(244, 244, 244);
        ForeColor = Color.FromArgb(45, 45, 45);
        Name = "UcInternationalRegions";
        Text = "الأقاليم الدولية";
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
