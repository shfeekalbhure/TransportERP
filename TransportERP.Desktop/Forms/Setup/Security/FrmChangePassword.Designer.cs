namespace TransportERP.Desktop.Forms.Setup.Security;

partial class UcChangePassword
{
    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
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
    private Button standardCommandRefresh = null!;
    private Button standardCommandImport = null!;
    private Button standardCommandExport = null!;
    private Button standardCommandHelp = null!;

    private System.ComponentModel.IContainer? components;
    private TableLayoutPanel passwordLayout = null!;
    private Label lblTitle = null!;
    private TableLayoutPanel passwordFields = null!;
    private Label lblCurrentPassword = null!;
    private TextBox CurrentPassword = null!;
    private Label lblNewPassword = null!;
    private TextBox NewPassword = null!;
    private Label lblConfirmPassword = null!;
    private TextBox ConfirmPassword = null!;
    private FlowLayoutPanel passwordActions = null!;
    private Button btnChangePassword = null!;
    private Button btnClose = null!;

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
        passwordLayout = new TableLayoutPanel();
        lblTitle = new Label();
        passwordFields = new TableLayoutPanel();
        lblCurrentPassword = new Label();
        CurrentPassword = new TextBox();
        lblNewPassword = new Label();
        NewPassword = new TextBox();
        lblConfirmPassword = new Label();
        ConfirmPassword = new TextBox();
        passwordActions = new FlowLayoutPanel();
        btnChangePassword = new Button();
        btnClose = new Button();
        passwordLayout.SuspendLayout();
        passwordFields.SuspendLayout();
        passwordActions.SuspendLayout();
        SuspendLayout();
        // 
        // passwordLayout
        // 
        passwordLayout.ColumnCount = 1;
        passwordLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        passwordLayout.Controls.Add(lblTitle, 0, 0);
        passwordLayout.Controls.Add(passwordFields, 0, 1);
        passwordLayout.Controls.Add(designerCommandBar, 0, 3);
        passwordLayout.Dock = DockStyle.Fill;
        passwordLayout.Location = new Point(0, 0);
        passwordLayout.Name = "passwordLayout";
        passwordLayout.Margin = new Padding(0);
        passwordLayout.Padding = new Padding(4);
        passwordLayout.RowCount = 4;
        passwordLayout.RowStyles.Add(new RowStyle());
        passwordLayout.RowStyles.Add(new RowStyle());
        passwordLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        passwordLayout.RowStyles.Add(new RowStyle());
        passwordLayout.Size = new Size(640, 420);
        passwordLayout.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.BackColor = Color.FromArgb(192, 192, 255);
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(16, 24, 40);
        lblTitle.Location = new Point(24, 24);
        lblTitle.Margin = new Padding(3);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(592, 28);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "تغيير كلمة السر";
        lblTitle.TextAlign = ContentAlignment.MiddleRight;
        // 
        // passwordFields
        // 
        passwordFields.AutoSize = true;
        passwordFields.ColumnCount = 2;
        passwordFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 225F));
        passwordFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        passwordFields.Controls.Add(lblCurrentPassword, 0, 0);
        passwordFields.Controls.Add(CurrentPassword, 1, 0);
        passwordFields.Controls.Add(lblNewPassword, 0, 1);
        passwordFields.Controls.Add(NewPassword, 1, 1);
        passwordFields.Controls.Add(lblConfirmPassword, 0, 2);
        passwordFields.Controls.Add(ConfirmPassword, 1, 2);
        passwordFields.Dock = DockStyle.Fill;
        passwordFields.Location = new Point(19, 71);
        passwordFields.Name = "passwordFields";
        passwordFields.RightToLeft = RightToLeft.Yes;
        passwordFields.RowCount = 3;
        passwordFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        passwordFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        passwordFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        passwordFields.Size = new Size(602, 120);
        passwordFields.TabIndex = 1;
        // 
        // lblCurrentPassword
        // 
        lblCurrentPassword.Dock = DockStyle.Fill;
        lblCurrentPassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblCurrentPassword.ForeColor = Color.FromArgb(180, 35, 24);
        lblCurrentPassword.Location = new Point(380, 3);
        lblCurrentPassword.Margin = new Padding(3);
        lblCurrentPassword.Name = "lblCurrentPassword";
        lblCurrentPassword.Size = new Size(219, 34);
        lblCurrentPassword.TabIndex = 0;
        lblCurrentPassword.Text = "كلمة السر الحالية *";
        lblCurrentPassword.TextAlign = ContentAlignment.MiddleRight;
        // 
        // CurrentPassword
        // 
        CurrentPassword.AccessibleName = "كلمة السر الحالية";
        CurrentPassword.BackColor = Color.FromArgb(255, 249, 219);
        CurrentPassword.Dock = DockStyle.Fill;
        CurrentPassword.Font = new Font("Segoe UI", 11F);
        CurrentPassword.Location = new Point(3, 3);
        CurrentPassword.Name = "CurrentPassword";
        CurrentPassword.RightToLeft = RightToLeft.Yes;
        CurrentPassword.Size = new Size(371, 32);
        CurrentPassword.TabIndex = 0;
        CurrentPassword.UseSystemPasswordChar = true;
        // 
        // lblNewPassword
        // 
        lblNewPassword.Dock = DockStyle.Fill;
        lblNewPassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblNewPassword.ForeColor = Color.FromArgb(180, 35, 24);
        lblNewPassword.Location = new Point(380, 43);
        lblNewPassword.Margin = new Padding(3);
        lblNewPassword.Name = "lblNewPassword";
        lblNewPassword.Size = new Size(219, 34);
        lblNewPassword.TabIndex = 1;
        lblNewPassword.Text = "كلمة السر الجديدة *";
        lblNewPassword.TextAlign = ContentAlignment.MiddleRight;
        // 
        // NewPassword
        // 
        NewPassword.AccessibleName = "كلمة السر الجديدة";
        NewPassword.BackColor = Color.FromArgb(255, 249, 219);
        NewPassword.Dock = DockStyle.Fill;
        NewPassword.Font = new Font("Segoe UI", 11F);
        NewPassword.Location = new Point(3, 43);
        NewPassword.Name = "NewPassword";
        NewPassword.RightToLeft = RightToLeft.Yes;
        NewPassword.Size = new Size(371, 32);
        NewPassword.TabIndex = 1;
        NewPassword.UseSystemPasswordChar = true;
        // 
        // lblConfirmPassword
        // 
        lblConfirmPassword.Dock = DockStyle.Fill;
        lblConfirmPassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblConfirmPassword.ForeColor = Color.FromArgb(180, 35, 24);
        lblConfirmPassword.Location = new Point(380, 83);
        lblConfirmPassword.Margin = new Padding(3);
        lblConfirmPassword.Name = "lblConfirmPassword";
        lblConfirmPassword.Size = new Size(219, 34);
        lblConfirmPassword.TabIndex = 2;
        lblConfirmPassword.Text = "تأكيد كلمة السر الجديدة *";
        lblConfirmPassword.TextAlign = ContentAlignment.MiddleRight;
        // 
        // ConfirmPassword
        // 
        ConfirmPassword.AccessibleName = "تأكيد كلمة السر الجديدة";
        ConfirmPassword.BackColor = Color.FromArgb(255, 249, 219);
        ConfirmPassword.Dock = DockStyle.Fill;
        ConfirmPassword.Font = new Font("Segoe UI", 11F);
        ConfirmPassword.Location = new Point(3, 83);
        ConfirmPassword.Name = "ConfirmPassword";
        ConfirmPassword.RightToLeft = RightToLeft.Yes;
        ConfirmPassword.Size = new Size(371, 32);
        ConfirmPassword.TabIndex = 2;
        ConfirmPassword.UseSystemPasswordChar = true;
        // 
        // passwordActions
        // 
        passwordActions.AutoSize = true;
        passwordActions.BackColor = Color.FromArgb(224, 224, 224);
        passwordActions.Controls.Add(btnChangePassword);

        passwordActions.Dock = DockStyle.Fill;
        passwordActions.FlowDirection = FlowDirection.RightToLeft;
        passwordActions.Location = new Point(19, 355);
        passwordActions.Name = "passwordActions";
        passwordActions.Size = new Size(602, 46);
        passwordActions.TabIndex = 2;
        // 
        // btnChangePassword
        // 
        btnChangePassword.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnChangePassword.BackColor = Color.FromArgb(224, 224, 224);
        btnChangePassword.Enabled = false;
        btnChangePassword.FlatStyle = FlatStyle.Flat;
        btnChangePassword.Font = new Font("Segoe UI", 10F);
        btnChangePassword.ForeColor = Color.FromArgb(16, 24, 40);
        btnChangePassword.Location = new Point(4, 4);
        btnChangePassword.Margin = new Padding(3);
        btnChangePassword.Name = "btnChangePassword";
        btnChangePassword.Padding = new Padding(4);
        btnChangePassword.Size = new Size(160, 38);
        btnChangePassword.TabIndex = 0;
        btnChangePassword.Text = "تغيير كلمة السر";
        btnChangePassword.UseVisualStyleBackColor = false;
        // 
        // btnClose
        // 
        btnClose.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnClose.BackColor = Color.FromArgb(224, 224, 224);
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Segoe UI", 10F);
        btnClose.ForeColor = Color.FromArgb(16, 24, 40);
        btnClose.Location = new Point(124, 4);
        btnClose.Margin = new Padding(3);
        btnClose.Name = "btnClose";
        btnClose.Padding = new Padding(4);
        btnClose.Size = new Size(160, 38);
        btnClose.TabIndex = 1;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += BtnClose_Click;
        // 
        // UcChangePassword
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.LightCyan;
        Controls.Add(passwordLayout);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(640, 420);
        Name = "UcChangePassword";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(640, 420);
        Tag = "02.03.06";
        passwordLayout.ResumeLayout(false);
        passwordLayout.PerformLayout();
        passwordFields.ResumeLayout(false);
        passwordFields.PerformLayout();
        passwordActions.ResumeLayout(false);
        ResumeLayout(false);
    
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Fill;
        designerCommandBar.Location = new Point(19, 355);
        designerCommandBar.Size = new Size(602, 46);
        designerCommandBar.TabIndex = 2;
        designerCommandBar.Controls.Add(passwordActions);
        passwordActions.Dock = DockStyle.Fill;
        passwordActions.Margin = Padding.Empty;
        designerCommandBar.Name = "designerCommandBar";
        designerCommandBar.BackColor = Color.FromArgb(239, 239, 239);
        designerCommandBar.BorderStyle = BorderStyle.FixedSingle;
        designerCommandBar.Controls.Add(designerCloseHost);
        designerCloseHost.Dock = DockStyle.Left;
        designerCloseHost.Width = 28;
        designerCloseHost.Name = "designerCloseHost";
        passwordActions.Name = "passwordActions";
        passwordActions.Dock = DockStyle.Fill;
        passwordActions.AutoSize = false;
        passwordActions.WrapContents = false;
        passwordActions.FlowDirection = FlowDirection.RightToLeft;
        passwordActions.RightToLeft = RightToLeft.No;
        passwordActions.Padding = new Padding(2);
        designerCommandBar.MinimumSize = new Size(0, 30);
        designerCommandBar.Height = 44;
        standardCommandAdd.Name = "standardCommandAdd";
        standardCommandAdd.Enabled = false;
        standardCommandAdd.Visible = false;
        standardCommandAdd.AccessibleName = "إضافة";
        standardCommandEdit.Name = "standardCommandEdit";
        standardCommandEdit.Enabled = false;
        standardCommandEdit.Visible = false;
        standardCommandEdit.AccessibleName = "تعديل";
        standardCommandDelete.Name = "standardCommandDelete";
        standardCommandDelete.Enabled = false;
        standardCommandDelete.Visible = false;
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
        standardCommandSave.Visible = false;
        standardCommandSave.AccessibleName = "حفظ";
        standardCommandPrint.Name = "standardCommandPrint";
        standardCommandPrint.Enabled = false;
        standardCommandPrint.Visible = false;
        standardCommandPrint.AccessibleName = "طباعة";
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
        passwordActions.Controls.Add(standardCommandAdd);
        designerCommandBar.SetCommandRole(standardCommandAdd, TransportERP.Desktop.CoreUI.DesignerCommandRole.Add);
        standardCommandEdit.AutoSize = false;
        standardCommandEdit.Dock = DockStyle.None;
        standardCommandEdit.MinimumSize = Size.Empty;
        standardCommandEdit.Size = new Size(26, 24);
        standardCommandEdit.Margin = new Padding(1);
        standardCommandEdit.FlatStyle = FlatStyle.Flat;
        standardCommandEdit.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandEdit.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Edit;
        standardCommandEdit.Text = "";
        passwordActions.Controls.Add(standardCommandEdit);
        designerCommandBar.SetCommandRole(standardCommandEdit, TransportERP.Desktop.CoreUI.DesignerCommandRole.Edit);
        standardCommandDelete.AutoSize = false;
        standardCommandDelete.Dock = DockStyle.None;
        standardCommandDelete.MinimumSize = Size.Empty;
        standardCommandDelete.Size = new Size(26, 24);
        standardCommandDelete.Margin = new Padding(1);
        standardCommandDelete.FlatStyle = FlatStyle.Flat;
        standardCommandDelete.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandDelete.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Delete;
        standardCommandDelete.Text = "";
        passwordActions.Controls.Add(standardCommandDelete);
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
        passwordActions.Controls.Add(standardCommandCancel);
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
        passwordActions.Controls.Add(standardCommandView);
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
        passwordActions.Controls.Add(standardCommandLast);
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
        passwordActions.Controls.Add(standardCommandNext);
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
        passwordActions.Controls.Add(standardCommandPrevious);
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
        passwordActions.Controls.Add(standardCommandFirst);
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
        passwordActions.Controls.Add(standardCommandSave);
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
        passwordActions.Controls.Add(standardCommandPrint);
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
        standardCommandRefresh.AutoSize = false;
        standardCommandRefresh.Dock = DockStyle.None;
        standardCommandRefresh.MinimumSize = Size.Empty;
        standardCommandRefresh.Size = new Size(26, 24);
        standardCommandRefresh.Margin = new Padding(1);
        standardCommandRefresh.FlatStyle = FlatStyle.Flat;
        standardCommandRefresh.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandRefresh.Text = "تحديث";
        passwordActions.Controls.Add(standardCommandRefresh);
        designerCommandBar.SetCommandRole(standardCommandRefresh, TransportERP.Desktop.CoreUI.DesignerCommandRole.Refresh);
        standardCommandImport.AutoSize = false;
        standardCommandImport.Dock = DockStyle.None;
        standardCommandImport.MinimumSize = Size.Empty;
        standardCommandImport.Size = new Size(26, 24);
        standardCommandImport.Margin = new Padding(1);
        standardCommandImport.FlatStyle = FlatStyle.Flat;
        standardCommandImport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandImport.Text = "استيراد";
        passwordActions.Controls.Add(standardCommandImport);
        designerCommandBar.SetCommandRole(standardCommandImport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Import);
        standardCommandExport.AutoSize = false;
        standardCommandExport.Dock = DockStyle.None;
        standardCommandExport.MinimumSize = Size.Empty;
        standardCommandExport.Size = new Size(26, 24);
        standardCommandExport.Margin = new Padding(1);
        standardCommandExport.FlatStyle = FlatStyle.Flat;
        standardCommandExport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandExport.Text = "تصدير";
        passwordActions.Controls.Add(standardCommandExport);
        designerCommandBar.SetCommandRole(standardCommandExport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Export);
        standardCommandHelp.AutoSize = false;
        standardCommandHelp.Dock = DockStyle.None;
        standardCommandHelp.MinimumSize = Size.Empty;
        standardCommandHelp.Size = new Size(26, 24);
        standardCommandHelp.Margin = new Padding(1);
        standardCommandHelp.FlatStyle = FlatStyle.Flat;
        standardCommandHelp.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandHelp.Text = "مساعدة";
        passwordActions.Controls.Add(standardCommandHelp);
        designerCommandBar.SetCommandRole(standardCommandHelp, TransportERP.Desktop.CoreUI.DesignerCommandRole.Help);
    }
}
