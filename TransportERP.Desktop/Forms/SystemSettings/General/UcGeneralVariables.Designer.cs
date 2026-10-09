#nullable enable
namespace TransportERP.Desktop.Forms.SystemSettings.General;

partial class UcGeneralVariables
{
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;

    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
    private Panel designerCloseHost = null!;
    private Button standardCommandAdd = null!;
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

    private System.ComponentModel.IContainer? components;
    private GroupBox grpMainData = null!;
    private TextBox txtMobileFilesPath = null!;
    private Button btnEdit = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        designerCloseHost = new Panel();
        standardCommandAdd = new Button();
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
        components = new System.ComponentModel.Container();
        SuspendLayout();
        Name = nameof(UcGeneralVariables);
        Text = "المتغيرات العامة";
        Size = new Size(1000, 640);
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        RightToLeft = RightToLeft.Yes;
        AutoScroll = true;

        var layout = new TableLayoutPanel
        {
            Name = "layout", Dock = DockStyle.Top, AutoSize = true,
            ColumnCount = 1, RowCount = 4, Padding = new Padding(16)
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        var title = new Label
        {
            Name = "lblTitle", Text = Text, AutoSize = true, Dock = DockStyle.Fill,
            Font = new Font("Tahoma", 9F), TextAlign = ContentAlignment.MiddleRight,
            BackColor = Color.FromArgb(192, 192, 255), Padding = new Padding(12)
        };
        var status = new Label
        {
            Name = "lblStatus", AutoSize = true, Dock = DockStyle.Fill,
            Text = "معاينة جزئية — القيم مؤقتة ولا تُحفظ ولا تُطبّق على النظام.",
            TextAlign = ContentAlignment.MiddleRight, Padding = new Padding(8)
        };
        var toolbar = new FlowLayoutPanel
        {
            Name = "pnlToolbar", Dock = DockStyle.Fill, AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight
        };
        btnEdit = new Button { Name = "btnEdit", Text = "تعديل", AutoSize = true, TabIndex = 0 };

        grpMainData = new GroupBox
        {
            Name = "grpMainData", Text = "البيانات الرئيسية", Dock = DockStyle.Fill,
            AutoSize = true, Padding = new Padding(16, 28, 16, 16), Enabled = false
        };
        var fields = new TableLayoutPanel
        {
            Name = "fields", Dock = DockStyle.Top, AutoSize = true,
            ColumnCount = 1, RowCount = 4
        };
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        var pathLabel = new Label
        {
            Name = "lblMobileFilesPath", Text = "مسار ملفات وأيقونات تطبيقات الموبايل",
            AutoSize = true, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight
        };
        txtMobileFilesPath = new TextBox
        {
            Name = "txtMobileFilesPath", AccessibleName = pathLabel.Text,
            Dock = DockStyle.Fill, TabIndex = 0
        };
        var bindDevice = new CheckBox
        {
            Name = "chkBindUserToDevice", Text = "ربط المستخدم بجهاز واحد",
            AutoSize = true, Dock = DockStyle.Fill, TabIndex = 1, Margin = new Padding(4, 12, 4, 4)
        };
        var reportHeader = new CheckBox
        {
            Name = "chkReportHeaderByUser", Text = "ترويسة التقارير حسب المستخدم",
            AutoSize = true, Dock = DockStyle.Fill, TabIndex = 2, Margin = new Padding(4, 12, 4, 4)
        };
        fields.Controls.Add(pathLabel, 0, 0);
        fields.Controls.Add(txtMobileFilesPath, 0, 1);
        fields.Controls.Add(bindDevice, 0, 2);
        fields.Controls.Add(reportHeader, 0, 3);
        grpMainData.Controls.Add(fields);
        layout.Controls.Add(title, 0, 0);
        layout.Controls.Add(status, 0, 1);
        layout.Controls.Add(designerCommandBar, 0, 2);
        layout.Controls.Add(grpMainData, 0, 3);
        Controls.Add(layout);
        ResumeLayout(true);
    
        // Explicit Designer-owned command catalog.
        designerCommandBar.Controls.Add(toolbar);
        toolbar.Dock = DockStyle.Fill;
        toolbar.Margin = Padding.Empty;
        designerCommandBar.Name = "designerCommandBar";
        designerCommandBar.BackColor = Color.FromArgb(239, 239, 239);
        designerCommandBar.BorderStyle = BorderStyle.FixedSingle;
        designerCommandBar.Controls.Add(designerCloseHost);
        designerCloseHost.Dock = DockStyle.Left;
        designerCloseHost.Width = 28;
        designerCloseHost.Name = "designerCloseHost";
        toolbar.Name = "toolbar";
        toolbar.Dock = DockStyle.Fill;
        toolbar.AutoSize = false;
        toolbar.WrapContents = false;
        toolbar.FlowDirection = FlowDirection.RightToLeft;
        toolbar.RightToLeft = RightToLeft.No;
        toolbar.Padding = new Padding(2);
        designerCommandBar.MinimumSize = new Size(0, 30);
        designerCommandBar.Height = 30;
        standardCommandAdd.Name = "standardCommandAdd";
        standardCommandAdd.Enabled = false;
        standardCommandAdd.Visible = false;
        standardCommandAdd.AccessibleName = "إضافة";
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
        toolbar.Controls.Add(standardCommandAdd);
        designerCommandBar.SetCommandRole(standardCommandAdd, TransportERP.Desktop.CoreUI.DesignerCommandRole.Add);
        btnEdit.AutoSize = false;
        btnEdit.Dock = DockStyle.None;
        btnEdit.MinimumSize = Size.Empty;
        btnEdit.Size = new Size(26, 24);
        btnEdit.Margin = new Padding(1);
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnEdit.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Edit;
        btnEdit.Text = "";
        toolbar.Controls.Add(btnEdit);
        designerCommandBar.SetCommandRole(btnEdit, TransportERP.Desktop.CoreUI.DesignerCommandRole.Edit);
        standardCommandDelete.AutoSize = false;
        standardCommandDelete.Dock = DockStyle.None;
        standardCommandDelete.MinimumSize = Size.Empty;
        standardCommandDelete.Size = new Size(26, 24);
        standardCommandDelete.Margin = new Padding(1);
        standardCommandDelete.FlatStyle = FlatStyle.Flat;
        standardCommandDelete.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandDelete.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Delete;
        standardCommandDelete.Text = "";
        toolbar.Controls.Add(standardCommandDelete);
        designerCommandBar.SetCommandRole(standardCommandDelete, TransportERP.Desktop.CoreUI.DesignerCommandRole.Delete);
        standardCommandCancel.AutoSize = false;
        standardCommandCancel.Dock = DockStyle.None;
        standardCommandCancel.MinimumSize = Size.Empty;
        standardCommandCancel.Size = new Size(26, 24);
        standardCommandCancel.Margin = new Padding(1);
        standardCommandCancel.FlatStyle = FlatStyle.Flat;
        standardCommandCancel.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandCancel.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Cancel;
        standardCommandCancel.Text = "";
        toolbar.Controls.Add(standardCommandCancel);
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
        toolbar.Controls.Add(standardCommandView);
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
        toolbar.Controls.Add(standardCommandLast);
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
        toolbar.Controls.Add(standardCommandNext);
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
        toolbar.Controls.Add(standardCommandPrevious);
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
        toolbar.Controls.Add(standardCommandFirst);
        designerCommandBar.SetCommandRole(standardCommandFirst, TransportERP.Desktop.CoreUI.DesignerCommandRole.First);
        standardCommandSave.AutoSize = false;
        standardCommandSave.Dock = DockStyle.None;
        standardCommandSave.MinimumSize = Size.Empty;
        standardCommandSave.Size = new Size(26, 24);
        standardCommandSave.Margin = new Padding(1);
        standardCommandSave.FlatStyle = FlatStyle.Flat;
        standardCommandSave.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandSave.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Save;
        standardCommandSave.Text = "";
        toolbar.Controls.Add(standardCommandSave);
        designerCommandBar.SetCommandRole(standardCommandSave, TransportERP.Desktop.CoreUI.DesignerCommandRole.Save);
        standardCommandPrint.AutoSize = false;
        standardCommandPrint.Dock = DockStyle.None;
        standardCommandPrint.MinimumSize = Size.Empty;
        standardCommandPrint.Size = new Size(26, 24);
        standardCommandPrint.Margin = new Padding(1);
        standardCommandPrint.FlatStyle = FlatStyle.Flat;
        standardCommandPrint.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandPrint.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Print;
        standardCommandPrint.Text = "";
        toolbar.Controls.Add(standardCommandPrint);
        designerCommandBar.SetCommandRole(standardCommandPrint, TransportERP.Desktop.CoreUI.DesignerCommandRole.Print);
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
        designerCommandBar.SetCommandRole(standardCommandClose, TransportERP.Desktop.CoreUI.DesignerCommandRole.Close);
        standardCommandRefresh.AutoSize = false;
        standardCommandRefresh.Dock = DockStyle.None;
        standardCommandRefresh.MinimumSize = Size.Empty;
        standardCommandRefresh.Size = new Size(26, 24);
        standardCommandRefresh.Margin = new Padding(1);
        standardCommandRefresh.FlatStyle = FlatStyle.Flat;
        standardCommandRefresh.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandRefresh.Text = "تحديث";
        toolbar.Controls.Add(standardCommandRefresh);
        designerCommandBar.SetCommandRole(standardCommandRefresh, TransportERP.Desktop.CoreUI.DesignerCommandRole.Refresh);
        standardCommandImport.AutoSize = false;
        standardCommandImport.Dock = DockStyle.None;
        standardCommandImport.MinimumSize = Size.Empty;
        standardCommandImport.Size = new Size(26, 24);
        standardCommandImport.Margin = new Padding(1);
        standardCommandImport.FlatStyle = FlatStyle.Flat;
        standardCommandImport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandImport.Text = "استيراد";
        toolbar.Controls.Add(standardCommandImport);
        designerCommandBar.SetCommandRole(standardCommandImport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Import);
        standardCommandExport.AutoSize = false;
        standardCommandExport.Dock = DockStyle.None;
        standardCommandExport.MinimumSize = Size.Empty;
        standardCommandExport.Size = new Size(26, 24);
        standardCommandExport.Margin = new Padding(1);
        standardCommandExport.FlatStyle = FlatStyle.Flat;
        standardCommandExport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandExport.Text = "تصدير";
        toolbar.Controls.Add(standardCommandExport);
        designerCommandBar.SetCommandRole(standardCommandExport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Export);
        standardCommandHelp.AutoSize = false;
        standardCommandHelp.Dock = DockStyle.None;
        standardCommandHelp.MinimumSize = Size.Empty;
        standardCommandHelp.Size = new Size(26, 24);
        standardCommandHelp.Margin = new Padding(1);
        standardCommandHelp.FlatStyle = FlatStyle.Flat;
        standardCommandHelp.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandHelp.Text = "مساعدة";
        toolbar.Controls.Add(standardCommandHelp);
        designerCommandBar.SetCommandRole(standardCommandHelp, TransportERP.Desktop.CoreUI.DesignerCommandRole.Help);
    
        // Shared audit presentation; original sources remain owned by this screen.
        standardAuditMetadata = new TransportERP.Desktop.CoreUI.AuditMetadataControl();
        standardAuditMetadata.Name = "standardAuditMetadata";
        standardAuditMetadata.Dock = DockStyle.Bottom;
        standardAuditMetadata.Margin = Padding.Empty;
        standardAuditMetadata.TabStop = false;
        standardAuditMetadata.Profile = TransportERP.Desktop.CoreUI.AuditMetadataProfile.ModificationOnly;
        standardAuditMetadata.Size = new Size(800, 36);
        Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
