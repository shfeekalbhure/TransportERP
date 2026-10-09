namespace TransportERP.Desktop;

partial class FrmLogin
{
    private System.ComponentModel.IContainer? components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        pnlBrand = new Panel();
        lblBrand = new Label();
        lblTagline = new Label();
        lblServices = new Label();
        lblWelcome = new Label();
        lblSubtitle = new Label();
        lblUsername = new Label();
        txtUsername = new TextBox();
        lblPassword = new Label();
        txtPassword = new TextBox();
        chkShowPassword = new CheckBox();
        lblLanguage = new Label();
        cmbLanguage = new ComboBox();
        btnLogin = new Button();
        btnExit = new Button();
        lblStatus = new Label();
        pnlFooter = new Panel();
        txtApiAddress = new TextBox();
        lblCompany = new Label();
        lblConnection = new Label();
        btnConnection = new Button();
        btnPreview = new Button();
        pnlBrand.SuspendLayout();
        pnlFooter.SuspendLayout();
        SuspendLayout();
        // 
        // pnlBrand
        // 
        pnlBrand.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        pnlBrand.BackColor = Color.FromArgb(24, 48, 76);
        pnlBrand.Controls.Add(lblBrand);
        pnlBrand.Controls.Add(lblTagline);
        pnlBrand.Controls.Add(lblServices);
        pnlBrand.Location = new Point(12, 0);
        pnlBrand.Margin = new Padding(3, 4, 3, 4);
        pnlBrand.Name = "pnlBrand";
        pnlBrand.Size = new Size(617, 680);
        pnlBrand.TabIndex = 9;
        // 
        // lblBrand
        // 
        lblBrand.Font = new Font("Segoe UI", 30F, FontStyle.Bold);
        lblBrand.ForeColor = Color.White;
        lblBrand.Location = new Point(40, 120);
        lblBrand.Name = "lblBrand";
        lblBrand.Size = new Size(549, 93);
        lblBrand.TabIndex = 0;
        lblBrand.Text = "الطائر السعيد";
        lblBrand.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblTagline
        // 
        lblTagline.Font = new Font("Segoe UI", 17F);
        lblTagline.ForeColor = Color.Gold;
        lblTagline.Location = new Point(40, 227);
        lblTagline.Name = "lblTagline";
        lblTagline.Size = new Size(549, 67);
        lblTagline.TabIndex = 1;
        lblTagline.Text = "للنقل والخدمات اللوجستية";
        lblTagline.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblServices
        // 
        lblServices.Font = new Font("Segoe UI", 16F);
        lblServices.ForeColor = Color.White;
        lblServices.Location = new Point(57, 333);
        lblServices.Name = "lblServices";
        lblServices.Size = new Size(514, 253);
        lblServices.TabIndex = 2;
        lblServices.Text = "نقل الركاب\r\n\r\nشحن الطرود\r\n\r\nرحلات داخلية وعبر الحدود";
        lblServices.TextAlign = ContentAlignment.TopCenter;
        // 
        // lblWelcome
        // 
        lblWelcome.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
        lblWelcome.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblWelcome.Location = new Point(686, 47);
        lblWelcome.Name = "lblWelcome";
        lblWelcome.Size = new Size(491, 87);
        lblWelcome.TabIndex = 10;
        lblWelcome.Text = "مرحباً بك";
        lblWelcome.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblSubtitle
        // 
        lblSubtitle.Location = new Point(686, 140);
        lblSubtitle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(491, 47);
        lblSubtitle.TabIndex = 11;
        lblSubtitle.Text = "تسجيل الدخول إلى نظام الطائر";
        lblSubtitle.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblUsername
        // 
        lblUsername.Location = new Point(1000, 213);
        lblUsername.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblUsername.Name = "lblUsername";
        lblUsername.Size = new Size(183, 43);
        lblUsername.TabIndex = 12;
        lblUsername.Text = "اسم المستخدم *";
        lblUsername.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtUsername
        // 
        txtUsername.BackColor = Color.LightYellow;
        txtUsername.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtUsername.Font = new Font("Segoe UI", 12F);
        txtUsername.Location = new Point(686, 213);
        txtUsername.Margin = new Padding(3, 4, 3, 4);
        txtUsername.Name = "txtUsername";
        txtUsername.RightToLeft = RightToLeft.No;
        txtUsername.Size = new Size(302, 34);
        txtUsername.TabIndex = 0;
        // 
        // lblPassword
        // 
        lblPassword.Location = new Point(1000, 313);
        lblPassword.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(183, 43);
        lblPassword.TabIndex = 13;
        lblPassword.Text = "كلمة المرور *";
        lblPassword.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtPassword
        // 
        txtPassword.BackColor = Color.LightYellow;
        txtPassword.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtPassword.Font = new Font("Segoe UI", 12F);
        txtPassword.Location = new Point(686, 313);
        txtPassword.Margin = new Padding(3, 4, 3, 4);
        txtPassword.Name = "txtPassword";
        txtPassword.RightToLeft = RightToLeft.No;
        txtPassword.Size = new Size(302, 34);
        txtPassword.TabIndex = 1;
        txtPassword.UseSystemPasswordChar = true;
        // 
        // chkShowPassword
        // 
        chkShowPassword.Location = new Point(743, 373);
        chkShowPassword.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        chkShowPassword.Margin = new Padding(3, 4, 3, 4);
        chkShowPassword.Name = "chkShowPassword";
        chkShowPassword.Size = new Size(246, 40);
        chkShowPassword.TabIndex = 2;
        chkShowPassword.Text = "إظهار كلمة المرور";
        chkShowPassword.CheckedChanged += ShowPassword_CheckedChanged;
        // 
        // lblLanguage
        // 
        lblLanguage.Location = new Point(1000, 440);
        lblLanguage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblLanguage.Name = "lblLanguage";
        lblLanguage.Size = new Size(183, 43);
        lblLanguage.TabIndex = 14;
        lblLanguage.Text = "اللغة";
        lblLanguage.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cmbLanguage
        // 
        cmbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbLanguage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cmbLanguage.Items.AddRange(new object[] { "العربية" });
        cmbLanguage.Location = new Point(686, 440);
        cmbLanguage.Margin = new Padding(3, 4, 3, 4);
        cmbLanguage.Name = "cmbLanguage";
        cmbLanguage.Size = new Size(302, 31);
        cmbLanguage.TabIndex = 3;
        // 
        // btnLogin
        // 
        btnLogin.BackColor = Color.FromArgb(92, 117, 36);
        btnLogin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnLogin.FlatStyle = FlatStyle.Flat;
        btnLogin.ForeColor = Color.White;
        btnLogin.Location = new Point(874, 527);
        btnLogin.Margin = new Padding(3, 4, 3, 4);
        btnLogin.Name = "btnLogin";
        btnLogin.Size = new Size(309, 64);
        btnLogin.TabIndex = 4;
        btnLogin.Text = "تسجيل الدخول";
        btnLogin.UseVisualStyleBackColor = false;
        btnLogin.Click += Login_Click;
        // 
        // btnExit
        // 
        btnExit.Location = new Point(686, 527);
        btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnExit.Margin = new Padding(3, 4, 3, 4);
        btnExit.Name = "btnExit";
        btnExit.Size = new Size(171, 64);
        btnExit.TabIndex = 5;
        btnExit.Text = "إغلاق";
        btnExit.Click += Exit_Click;
        // 
        // lblStatus
        // 
        lblStatus.ForeColor = Color.DarkGoldenrod;
        lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblStatus.Location = new Point(663, 653);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(531, 51);
        lblStatus.TabIndex = 15;
        lblStatus.Text = "أدخل بياناتك لتسجيل الدخول.";
        lblStatus.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(240, 243, 246);
        pnlFooter.Controls.Add(txtApiAddress);
        pnlFooter.Controls.Add(lblCompany);
        pnlFooter.Controls.Add(lblConnection);
        pnlFooter.Controls.Add(btnConnection);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 751);
        pnlFooter.Margin = new Padding(3, 4, 3, 4);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(1234, 93);
        pnlFooter.TabIndex = 16;
        // 
        // txtApiAddress
        // 
        txtApiAddress.AccessibleName = "عنوان خادم API";
        txtApiAddress.Location = new Point(246, 33);
        txtApiAddress.Margin = new Padding(3, 4, 3, 4);
        txtApiAddress.Name = "txtApiAddress";
        txtApiAddress.RightToLeft = RightToLeft.No;
        txtApiAddress.Size = new Size(411, 30);
        txtApiAddress.TabIndex = 7;
        // 
        // lblCompany
        // 
        lblCompany.Location = new Point(869, 24);
        lblCompany.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblCompany.Name = "lblCompany";
        lblCompany.Size = new Size(326, 47);
        lblCompany.TabIndex = 8;
        lblCompany.Text = "الطائر السعيد للنقل";
        lblCompany.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblConnection
        // 
        lblConnection.Location = new Point(669, 24);
        lblConnection.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblConnection.Name = "lblConnection";
        lblConnection.Size = new Size(206, 47);
        lblConnection.TabIndex = 9;
        lblConnection.Text = "حالة الاتصال: غير مفحوص";
        lblConnection.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnConnection
        // 
        btnConnection.Location = new Point(29, 20);
        btnConnection.Margin = new Padding(3, 4, 3, 4);
        btnConnection.Name = "btnConnection";
        btnConnection.Size = new Size(194, 53);
        btnConnection.TabIndex = 6;
        btnConnection.Text = "إعداد الاتصال";
        btnConnection.Click += Connection_Click;
        // 
        // btnPreview
        // 
        btnPreview.Location = new Point(674, 600);
        btnPreview.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnPreview.Margin = new Padding(3, 4, 3, 4);
        btnPreview.Name = "btnPreview";
        btnPreview.Size = new Size(509, 48);
        btnPreview.TabIndex = 8;
        btnPreview.Text = "معاينة الشاشات بدون خادم";
        btnPreview.Visible = false;
        btnPreview.Click += Preview_Click;
        // 
        // FrmLogin
        // 
        AcceptButton = btnLogin;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        CancelButton = btnExit;
        ClientSize = new Size(1234, 844);
        Controls.Add(btnPreview);
        Controls.Add(pnlBrand);
        Controls.Add(lblWelcome);
        Controls.Add(lblSubtitle);
        Controls.Add(lblUsername);
        Controls.Add(txtUsername);
        Controls.Add(lblPassword);
        Controls.Add(txtPassword);
        Controls.Add(chkShowPassword);
        Controls.Add(lblLanguage);
        Controls.Add(cmbLanguage);
        Controls.Add(btnLogin);
        Controls.Add(btnExit);
        Controls.Add(lblStatus);
        Controls.Add(pnlFooter);
        ForeColor = Color.FromArgb(24, 48, 76);
        FormBorderStyle = FormBorderStyle.Sizable;
        Margin = new Padding(3, 4, 3, 4);
        MaximizeBox = true;
        MinimumSize = new Size(1250, 850);
        Name = "FrmLogin";
        RightToLeftLayout = false;
        Text = "TransportERP | تسجيل الدخول";
        pnlBrand.ResumeLayout(false);
        pnlFooter.ResumeLayout(false);
        pnlFooter.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
    private Button btnPreview = null!;
    private TextBox txtApiAddress = null!;
    private Panel pnlBrand = null!;
    private Label lblBrand = null!;
    private Label lblTagline = null!;
    private Label lblServices = null!;
    private Label lblWelcome = null!;
    private Label lblSubtitle = null!;
    private Label lblUsername = null!;
    private TextBox txtUsername = null!;
    private Label lblPassword = null!;
    private TextBox txtPassword = null!;
    private CheckBox chkShowPassword = null!;
    private Label lblLanguage = null!;
    private ComboBox cmbLanguage = null!;
    private Button btnLogin = null!;
    private Button btnExit = null!;
    private Label lblStatus = null!;
    private Panel pnlFooter = null!;
    private Label lblCompany = null!;
    private Label lblConnection = null!;
    private Button btnConnection = null!;
}
