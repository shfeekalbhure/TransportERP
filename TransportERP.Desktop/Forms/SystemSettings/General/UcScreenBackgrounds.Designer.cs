namespace TransportERP.Desktop.Forms.SystemSettings.General;

partial class UcScreenBackgrounds
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

    private TableLayoutPanel referenceLayout = null!;
    private TableLayoutPanel themeCanvas = null!;
    private TransportERP.Desktop.CoreUI.DesignerCommandBar commandBar = null!;
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;
    private Label lblTitle = null!;
    private Label lblStatus = null!;
    private DataGridView gridThemes = null!;
    private DataGridViewTextBoxColumn colNumber = null!;
    private DataGridViewTextBoxColumn colName = null!;
    private DataGridViewCheckBoxColumn colSelected = null!;

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
        themeCanvas = new TableLayoutPanel();
        commandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        standardAuditMetadata = new TransportERP.Desktop.CoreUI.AuditMetadataControl();
        lblTitle = new Label();
        lblStatus = new Label();
        gridThemes = new DataGridView();
        colNumber = new DataGridViewTextBoxColumn();
        colName = new DataGridViewTextBoxColumn();
        colSelected = new DataGridViewCheckBoxColumn();
        SuspendLayout();
        referenceLayout.SuspendLayout();
        themeCanvas.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)gridThemes).BeginInit();
        referenceLayout.Name = "layout";
        referenceLayout.Dock = DockStyle.Fill;
        referenceLayout.ColumnCount = 1;
        referenceLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        referenceLayout.RowCount = 4;
        referenceLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        referenceLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        referenceLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        referenceLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        referenceLayout.Margin = Padding.Empty;
        referenceLayout.Controls.Add(lblTitle, 0, 0);
        referenceLayout.Controls.Add(commandBar, 0, 1);
        referenceLayout.Controls.Add(themeCanvas, 0, 2);
        referenceLayout.Controls.Add(lblStatus, 0, 3);
        themeCanvas.Name = "themeCanvas";
        themeCanvas.Dock = DockStyle.Fill;
        themeCanvas.RightToLeft = RightToLeft.No;
        themeCanvas.Margin = Padding.Empty;
        themeCanvas.ColumnCount = 3;
        themeCanvas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
        themeCanvas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26F));
        themeCanvas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
        themeCanvas.RowCount = 3;
        themeCanvas.RowStyles.Add(new RowStyle(SizeType.Percent, 27F));
        themeCanvas.RowStyles.Add(new RowStyle(SizeType.Percent, 36F));
        themeCanvas.RowStyles.Add(new RowStyle(SizeType.Percent, 37F));
        themeCanvas.Controls.Add(gridThemes, 1, 1);
        gridThemes.Name = "gridThemes";
        gridThemes.AccessibleName = "خلفيات الشاشات";
        gridThemes.Dock = DockStyle.Fill;
        gridThemes.Margin = Padding.Empty;
        gridThemes.RightToLeft = RightToLeft.Yes;
        gridThemes.Enabled = false;
        gridThemes.AutoGenerateColumns = false;
        gridThemes.AllowUserToAddRows = false;
        gridThemes.AllowUserToDeleteRows = false;
        gridThemes.AllowUserToOrderColumns = false;
        gridThemes.RowHeadersVisible = false;
        gridThemes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        gridThemes.BackgroundColor = Color.White;
        gridThemes.BorderStyle = BorderStyle.None;
        gridThemes.GridColor = Color.FromArgb(176, 176, 176);
        gridThemes.EnableHeadersVisualStyles = false;
        gridThemes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(244, 244, 244);
        gridThemes.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        gridThemes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(216, 255, 255);
        gridThemes.DefaultCellStyle.SelectionForeColor = Color.Black;
        gridThemes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        gridThemes.ColumnHeadersHeight = 24;
        gridThemes.RowTemplate.Height = 22;
        gridThemes.Columns.AddRange(colNumber, colName, colSelected);
        gridThemes.CurrentCellDirtyStateChanged += gridThemes_CurrentCellDirtyStateChanged;
        gridThemes.CellValueChanged += gridThemes_CellValueChanged;
        colNumber.Name = "colNumber";
        colNumber.HeaderText = "الرقم";
        colNumber.ReadOnly = true;
        colNumber.SortMode = DataGridViewColumnSortMode.NotSortable;
        colNumber.FillWeight = 24F;
        colNumber.DisplayIndex = 1;
        colName.Name = "colName";
        colName.HeaderText = "الاسم";
        colName.ReadOnly = true;
        colName.SortMode = DataGridViewColumnSortMode.NotSortable;
        colName.FillWeight = 64F;
        colName.DisplayIndex = 2;
        colSelected.Name = "colSelected";
        colSelected.HeaderText = "";
        colSelected.FillWeight = 12F;
        colSelected.DisplayIndex = 0;
        colSelected.SortMode = DataGridViewColumnSortMode.NotSortable;
        commandBar.Name = "standardCommandBar";
        commandBar.Dock = DockStyle.Fill;
        commandBar.MinimumSize = new Size(0, 30);
        commandBar.Size = new Size(1000, 30);
        commandBar.Margin = Padding.Empty;
        lblTitle.Name = "lblTitle";
        lblTitle.Text = "تهيئة النظام · التهيئة · خلفيات الشاشات";
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.AutoSize = true;
        lblTitle.Padding = new Padding(3);
        lblTitle.Margin = Padding.Empty;
        lblStatus.Name = "lblStatus";
        lblStatus.Text = "GENS018 — الثيمات والحفظ غير متاحين";
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.AutoSize = true;
        lblStatus.Padding = new Padding(3);
        lblStatus.Margin = Padding.Empty;
        standardAuditMetadata.Name = "standardAuditMetadata";
        standardAuditMetadata.Profile = TransportERP.Desktop.CoreUI.AuditMetadataProfile.CyanModification;
        standardAuditMetadata.Dock = DockStyle.Bottom;
        standardAuditMetadata.Margin = Padding.Empty;
        standardAuditMetadata.Size = new Size(1000, 32);
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScroll = true;
        Font = new Font("Tahoma", 9F);
        BackColor = Color.FromArgb(244, 244, 244);
        ForeColor = Color.FromArgb(45, 45, 45);
        Name = "UcScreenBackgrounds";
        Text = "خلفيات الشاشات";
        RightToLeft = RightToLeft.Yes;
        Dock = DockStyle.Fill;
        Margin = Padding.Empty;
        Padding = new Padding(2);
        Size = new Size(1000, 680);
        Controls.Add(referenceLayout);
        Controls.Add(standardAuditMetadata);
        ((System.ComponentModel.ISupportInitialize)gridThemes).EndInit();
        themeCanvas.ResumeLayout(false);
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
        standardCommandAdd.Visible = false;
        standardCommandAdd.AccessibleName = "إضافة";
        standardCommandEdit.Name = "standardCommandEdit";
        standardCommandEdit.Enabled = false;
        standardCommandEdit.Visible = true;
        standardCommandEdit.AccessibleName = "تعديل";
        standardCommandDelete.Name = "standardCommandDelete";
        standardCommandDelete.Enabled = false;
        standardCommandDelete.Visible = false;
        standardCommandDelete.AccessibleName = "حذف";
        standardCommandCancel.Name = "standardCommandCancel";
        standardCommandCancel.Enabled = false;
        standardCommandCancel.Visible = true;
        standardCommandCancel.AccessibleName = "تراجع";
        standardCommandView.Name = "standardCommandView";
        standardCommandView.Enabled = false;
        standardCommandView.Visible = false;
        standardCommandView.AccessibleName = "عرض";
        standardCommandLast.Name = "standardCommandLast";
        standardCommandLast.Enabled = false;
        standardCommandLast.Visible = false;
        standardCommandLast.AccessibleName = "الأخير";
        standardCommandNext.Name = "standardCommandNext";
        standardCommandNext.Enabled = false;
        standardCommandNext.Visible = false;
        standardCommandNext.AccessibleName = "التالي";
        standardCommandPrevious.Name = "standardCommandPrevious";
        standardCommandPrevious.Enabled = false;
        standardCommandPrevious.Visible = false;
        standardCommandPrevious.AccessibleName = "السابق";
        standardCommandFirst.Name = "standardCommandFirst";
        standardCommandFirst.Enabled = false;
        standardCommandFirst.Visible = false;
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
