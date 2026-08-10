using TransportERP.Desktop.Controls;
using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop
{
    partial class FrmLogin
    {
        /// <summary>
        /// حاوية المكونات التي ينشئها مصمم Windows Forms.
        /// </summary>
        private System.ComponentModel.IContainer? components = null;

        private TableLayoutPanel tblRoot = null!;
        private TableLayoutPanel tblContent = null!;
        private TableLayoutPanel tblLoginLayout = null!;
        private TableLayoutPanel tblOptions = null!;
        private TableLayoutPanel tblBrandLayout = null!;
        private Panel pnlLoginCard = null!;
        private Panel pnlBrand = null!;
        private Label lblUserName = null!;
        private Label lblPassword = null!;
        private Label lblCompany = null!;
        private Label lblFiscalYear = null!;
        private PasswordTextBox txtPassword = null!;
        private LookupComboBox cmbCompany = null!;
        private LookupComboBox cmbFiscalYear = null!;
        private CheckBox chkRememberMe = null!;
        private LinkLabel lnkForgotPassword = null!;
        private PrimaryButton btnLogin = null!;
        private Label lblLanguage = null!;
        private Label lblBrandName = null!;
        private Label lblBrandDescription = null!;
        private Label lblBrandFeatures = null!;
        private TransportStatusBar statusBar = null!;

        /// <summary>
        /// تحرير الموارد المستخدمة بواسطة النافذة.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components is not null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// إنشاء شاشة تسجيل الدخول الجديدة باستخدام تخطيط مرن قابل للتعديل من المصمم.
        /// </summary>
        private void InitializeComponent()
        {
            tblRoot = new TableLayoutPanel();
            tblContent = new TableLayoutPanel();
            pnlLoginCard = new Panel();
            tblLoginLayout = new TableLayoutPanel();
            lblUserName = new Label();
            lblPassword = new Label();
            txtPassword = new PasswordTextBox();
            lblCompany = new Label();
            cmbCompany = new LookupComboBox();
            lblFiscalYear = new Label();
            cmbFiscalYear = new LookupComboBox();
            tblOptions = new TableLayoutPanel();
            chkRememberMe = new CheckBox();
            lnkForgotPassword = new LinkLabel();
            btnLogin = new PrimaryButton();
            lblLanguage = new Label();
            pnlBrand = new Panel();
            tblBrandLayout = new TableLayoutPanel();
            lblBrandName = new Label();
            lblBrandDescription = new Label();
            lblBrandFeatures = new Label();
            statusBar = new TransportStatusBar();
            txtUserName = new RequiredTextBox();
            lblSubtitle = new Label();
            lblBranch = new Label();
            cmbBranch = new LookupComboBox();
            lblTitle = new Label();
            tblRoot.SuspendLayout();
            tblContent.SuspendLayout();
            pnlLoginCard.SuspendLayout();
            tblLoginLayout.SuspendLayout();
            tblOptions.SuspendLayout();
            pnlBrand.SuspendLayout();
            tblBrandLayout.SuspendLayout();
            SuspendLayout();
            // 
            // tblRoot
            // 
            tblRoot.BackColor = Color.FromArgb(239, 245, 252);
            tblRoot.ColumnCount = 1;
            tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblRoot.Controls.Add(tblContent, 0, 0);
            tblRoot.Controls.Add(statusBar, 0, 1);
            tblRoot.Dock = DockStyle.Fill;
            tblRoot.Location = new Point(0, 0);
            tblRoot.Margin = new Padding(0);
            tblRoot.Name = "tblRoot";
            tblRoot.Padding = new Padding(24, 18, 24, 0);
            tblRoot.RowCount = 2;
            tblRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tblRoot.Size = new Size(1360, 820);
            tblRoot.TabIndex = 0;
            // 
            // tblContent
            // 
            tblContent.BackColor = Color.Transparent;
            tblContent.ColumnCount = 2;
            tblContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            tblContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
            tblContent.Controls.Add(pnlLoginCard, 0, 0);
            tblContent.Controls.Add(pnlBrand, 1, 0);
            tblContent.Dock = DockStyle.Fill;
            tblContent.Location = new Point(24, 18);
            tblContent.Margin = new Padding(0);
            tblContent.Name = "tblContent";
            tblContent.RightToLeft = RightToLeft.Yes;
            tblContent.RowCount = 1;
            tblContent.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblContent.Size = new Size(1312, 766);
            tblContent.TabIndex = 0;
            // 
            // pnlLoginCard
            // 
            pnlLoginCard.BackColor = Color.White;
            pnlLoginCard.Controls.Add(tblLoginLayout);
            pnlLoginCard.Dock = DockStyle.Fill;
            pnlLoginCard.Location = new Point(695, 0);
            pnlLoginCard.Margin = new Padding(0, 0, 12, 8);
            pnlLoginCard.Name = "pnlLoginCard";
            pnlLoginCard.Padding = new Padding(48, 28, 48, 22);
            pnlLoginCard.Size = new Size(617, 758);
            pnlLoginCard.TabIndex = 0;
            // 
            // tblLoginLayout
            // 
            tblLoginLayout.BackColor = Color.Transparent;
            tblLoginLayout.ColumnCount = 2;
            tblLoginLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tblLoginLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tblLoginLayout.Controls.Add(lblTitle, 0, 0);
            tblLoginLayout.Controls.Add(tblOptions, 0, 12);
            tblLoginLayout.Controls.Add(btnLogin, 0, 13);
            tblLoginLayout.Controls.Add(lblLanguage, 0, 14);
            tblLoginLayout.Controls.Add(cmbCompany, 1, 1);
            tblLoginLayout.Controls.Add(cmbBranch, 1, 3);
            tblLoginLayout.Controls.Add(cmbFiscalYear, 1, 5);
            tblLoginLayout.Controls.Add(lblSubtitle, 0, 6);
            tblLoginLayout.Controls.Add(lblCompany, 0, 1);
            tblLoginLayout.Controls.Add(lblBranch, 0, 3);
            tblLoginLayout.Controls.Add(lblFiscalYear, 0, 5);
            tblLoginLayout.Controls.Add(lblUserName, 0, 7);
            tblLoginLayout.Controls.Add(txtUserName, 1, 7);
            tblLoginLayout.Controls.Add(lblPassword, 0, 9);
            tblLoginLayout.Controls.Add(txtPassword, 1, 9);
            tblLoginLayout.Dock = DockStyle.Fill;
            tblLoginLayout.Location = new Point(48, 28);
            tblLoginLayout.Margin = new Padding(0);
            tblLoginLayout.Name = "tblLoginLayout";
            tblLoginLayout.RightToLeft = RightToLeft.Yes;
            tblLoginLayout.RowCount = 16;
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblLoginLayout.Size = new Size(521, 708);
            tblLoginLayout.TabIndex = 0;
            // 
            // lblUserName
            // 
            lblUserName.Dock = DockStyle.Fill;
            lblUserName.Location = new Point(404, 274);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(114, 42);
            lblUserName.TabIndex = 2;
            lblUserName.Text = "اسم المستخدم";
            // 
            // lblPassword
            // 
            lblPassword.Dock = DockStyle.Fill;
            lblPassword.Location = new Point(404, 348);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(114, 42);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "كلمه المرور";
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.FromArgb(255, 250, 214);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Dock = DockStyle.Fill;
            txtPassword.Location = new Point(3, 351);
            txtPassword.MinimumSize = new Size(180, 38);
            txtPassword.Name = "txtPassword";
            txtPassword.Padding = new Padding(6, 4, 6, 4);
            txtPassword.RightToLeft = RightToLeft.Yes;
            txtPassword.Size = new Size(395, 38);
            txtPassword.TabIndex = 5;
            txtPassword.TextAlign = HorizontalAlignment.Left;
            // 
            // lblCompany
            // 
            lblCompany.Dock = DockStyle.Fill;
            lblCompany.Location = new Point(404, 52);
            lblCompany.Name = "lblCompany";
            lblCompany.Size = new Size(114, 42);
            lblCompany.TabIndex = 6;
            lblCompany.Text = "اسم الشركه";
            // 
            // cmbCompany
            // 
            cmbCompany.BackColor = Color.FromArgb(255, 250, 214);
            cmbCompany.Dock = DockStyle.Fill;
            cmbCompany.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCompany.FlatStyle = FlatStyle.Flat;
            cmbCompany.Font = new Font("Segoe UI", 10.5F);
            cmbCompany.ForeColor = Color.FromArgb(17, 43, 78);
            cmbCompany.Location = new Point(3, 55);
            cmbCompany.Name = "cmbCompany";
            cmbCompany.RightToLeft = RightToLeft.Yes;
            cmbCompany.Size = new Size(395, 31);
            cmbCompany.TabIndex = 7;
            // 
            // lblFiscalYear
            // 
            lblFiscalYear.Dock = DockStyle.Fill;
            lblFiscalYear.Location = new Point(404, 200);
            lblFiscalYear.Name = "lblFiscalYear";
            lblFiscalYear.Size = new Size(114, 42);
            lblFiscalYear.TabIndex = 10;
            lblFiscalYear.Text = "السنه الماليه";
            // 
            // cmbFiscalYear
            // 
            cmbFiscalYear.BackColor = Color.FromArgb(255, 250, 214);
            cmbFiscalYear.Dock = DockStyle.Fill;
            cmbFiscalYear.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiscalYear.FlatStyle = FlatStyle.Flat;
            cmbFiscalYear.Font = new Font("Segoe UI", 10.5F);
            cmbFiscalYear.ForeColor = Color.FromArgb(17, 43, 78);
            cmbFiscalYear.Location = new Point(3, 203);
            cmbFiscalYear.Name = "cmbFiscalYear";
            cmbFiscalYear.RightToLeft = RightToLeft.Yes;
            cmbFiscalYear.Size = new Size(395, 31);
            cmbFiscalYear.TabIndex = 11;
            // 
            // tblOptions
            // 
            tblOptions.ColumnCount = 2;
            tblOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblOptions.Controls.Add(chkRememberMe, 0, 0);
            tblOptions.Controls.Add(lnkForgotPassword, 1, 0);
            tblOptions.Dock = DockStyle.Fill;
            tblOptions.Location = new Point(401, 464);
            tblOptions.Margin = new Padding(0);
            tblOptions.Name = "tblOptions";
            tblOptions.RightToLeft = RightToLeft.Yes;
            tblOptions.RowCount = 1;
            tblOptions.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblOptions.Size = new Size(120, 42);
            tblOptions.TabIndex = 12;
            // 
            // chkRememberMe
            // 
            chkRememberMe.AutoSize = true;
            chkRememberMe.Dock = DockStyle.Fill;
            chkRememberMe.ForeColor = Color.FromArgb(17, 43, 78);
            chkRememberMe.Location = new Point(60, 0);
            chkRememberMe.Margin = new Padding(0);
            chkRememberMe.Name = "chkRememberMe";
            chkRememberMe.Size = new Size(60, 42);
            chkRememberMe.TabIndex = 0;
            chkRememberMe.Text = "تذكرني";
            chkRememberMe.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lnkForgotPassword
            // 
            lnkForgotPassword.ActiveLinkColor = Color.FromArgb(24, 88, 197);
            lnkForgotPassword.Dock = DockStyle.Fill;
            lnkForgotPassword.LinkColor = Color.FromArgb(35, 111, 229);
            lnkForgotPassword.Location = new Point(0, 0);
            lnkForgotPassword.Margin = new Padding(0);
            lnkForgotPassword.Name = "lnkForgotPassword";
            lnkForgotPassword.Size = new Size(60, 42);
            lnkForgotPassword.TabIndex = 1;
            lnkForgotPassword.TabStop = true;
            lnkForgotPassword.Text = "نسيت كلمة المرور؟";
            lnkForgotPassword.TextAlign = ContentAlignment.MiddleLeft;
            lnkForgotPassword.LinkClicked += lnkForgotPassword_LinkClicked_1;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(35, 111, 229);
            btnLogin.Dock = DockStyle.Fill;
            btnLogin.FlatAppearance.MouseDownBackColor = Color.FromArgb(24, 88, 197);
            btnLogin.FlatAppearance.MouseOverBackColor = Color.FromArgb(24, 88, 197);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.HoverBackColor = Color.FromArgb(24, 88, 197);
            btnLogin.Location = new Point(401, 510);
            btnLogin.Margin = new Padding(0, 4, 0, 4);
            btnLogin.MinimumSize = new Size(88, 34);
            btnLogin.Name = "btnLogin";
            btnLogin.NormalBackColor = Color.FromArgb(35, 111, 229);
            btnLogin.Padding = new Padding(10, 0, 10, 0);
            btnLogin.Size = new Size(120, 48);
            btnLogin.TabIndex = 13;
            btnLogin.Text = "دخول";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // lblLanguage
            // 
            lblLanguage.Dock = DockStyle.Fill;
            lblLanguage.ForeColor = Color.FromArgb(91, 111, 139);
            lblLanguage.Location = new Point(401, 562);
            lblLanguage.Margin = new Padding(0);
            lblLanguage.Name = "lblLanguage";
            lblLanguage.Size = new Size(120, 34);
            lblLanguage.TabIndex = 14;
            lblLanguage.Text = "العربية  |  English";
            lblLanguage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlBrand
            // 
            pnlBrand.BackColor = Color.FromArgb(17, 58, 140);
            pnlBrand.Controls.Add(tblBrandLayout);
            pnlBrand.Dock = DockStyle.Fill;
            pnlBrand.Location = new Point(0, 0);
            pnlBrand.Margin = new Padding(12, 0, 0, 8);
            pnlBrand.Name = "pnlBrand";
            pnlBrand.Padding = new Padding(54, 54, 54, 44);
            pnlBrand.Size = new Size(671, 758);
            pnlBrand.TabIndex = 1;
            // 
            // tblBrandLayout
            // 
            tblBrandLayout.BackColor = Color.Transparent;
            tblBrandLayout.ColumnCount = 1;
            tblBrandLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblBrandLayout.Controls.Add(lblBrandName, 0, 0);
            tblBrandLayout.Controls.Add(lblBrandDescription, 0, 1);
            tblBrandLayout.Controls.Add(lblBrandFeatures, 0, 3);
            tblBrandLayout.Dock = DockStyle.Fill;
            tblBrandLayout.Location = new Point(54, 54);
            tblBrandLayout.Margin = new Padding(0);
            tblBrandLayout.Name = "tblBrandLayout";
            tblBrandLayout.RightToLeft = RightToLeft.Yes;
            tblBrandLayout.RowCount = 4;
            tblBrandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));
            tblBrandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tblBrandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tblBrandLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblBrandLayout.Size = new Size(563, 660);
            tblBrandLayout.TabIndex = 0;
            // 
            // lblBrandName
            // 
            lblBrandName.Location = new Point(460, 0);
            lblBrandName.Name = "lblBrandName";
            lblBrandName.Size = new Size(100, 23);
            lblBrandName.TabIndex = 0;
            // 
            // lblBrandDescription
            // 
            lblBrandDescription.Location = new Point(460, 76);
            lblBrandDescription.Name = "lblBrandDescription";
            lblBrandDescription.Size = new Size(100, 23);
            lblBrandDescription.TabIndex = 1;
            // 
            // lblBrandFeatures
            // 
            lblBrandFeatures.Location = new Point(460, 164);
            lblBrandFeatures.Name = "lblBrandFeatures";
            lblBrandFeatures.Size = new Size(100, 23);
            lblBrandFeatures.TabIndex = 2;
            // 
            // statusBar
            // 
            statusBar.BackColor = Color.White;
            statusBar.BorderStyle = BorderStyle.FixedSingle;
            statusBar.Dock = DockStyle.Fill;
            statusBar.Font = new Font("Segoe UI", 9F);
            statusBar.Location = new Point(24, 784);
            statusBar.Margin = new Padding(0);
            statusBar.MinimumSize = new Size(0, 38);
            statusBar.Name = "statusBar";
            statusBar.Padding = new Padding(8, 3, 8, 3);
            statusBar.RightToLeft = RightToLeft.Yes;
            statusBar.Size = new Size(1312, 38);
            statusBar.TabIndex = 1;
            // 
            // txtUserName
            // 
            txtUserName.BackColor = Color.FromArgb(255, 250, 214);
            txtUserName.BorderStyle = BorderStyle.FixedSingle;
            txtUserName.Dock = DockStyle.Fill;
            txtUserName.Font = new Font("Segoe UI", 10.5F);
            txtUserName.ForeColor = Color.FromArgb(17, 43, 78);
            txtUserName.Location = new Point(3, 277);
            txtUserName.Name = "txtUserName";
            txtUserName.RightToLeft = RightToLeft.Yes;
            txtUserName.Size = new Size(395, 31);
            txtUserName.TabIndex = 15;
            // 
            // lblSubtitle
            // 
            lblSubtitle.Location = new Point(418, 242);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(100, 23);
            lblSubtitle.TabIndex = 1;
            // 
            // lblBranch
            // 
            lblBranch.Dock = DockStyle.Fill;
            lblBranch.Location = new Point(404, 126);
            lblBranch.Name = "lblBranch";
            lblBranch.Size = new Size(114, 42);
            lblBranch.TabIndex = 8;
            lblBranch.Text = "الفرع";
            // 
            // cmbBranch
            // 
            cmbBranch.BackColor = Color.FromArgb(255, 250, 214);
            cmbBranch.Dock = DockStyle.Fill;
            cmbBranch.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBranch.FlatStyle = FlatStyle.Flat;
            cmbBranch.Font = new Font("Segoe UI", 10.5F);
            cmbBranch.ForeColor = Color.FromArgb(17, 43, 78);
            cmbBranch.Location = new Point(3, 129);
            cmbBranch.Name = "cmbBranch";
            cmbBranch.RightToLeft = RightToLeft.Yes;
            cmbBranch.Size = new Size(395, 31);
            cmbBranch.TabIndex = 9;
            // 
            // lblTitle
            // 
            lblTitle.Location = new Point(418, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(100, 23);
            lblTitle.TabIndex = 0;
            // 
            // FrmLogin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(239, 245, 252);
            ClientSize = new Size(1360, 820);
            Controls.Add(tblRoot);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimumSize = new Size(1180, 720);
            Name = "FrmLogin";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TransportERP - تسجيل الدخول";
            tblRoot.ResumeLayout(false);
            tblContent.ResumeLayout(false);
            pnlLoginCard.ResumeLayout(false);
            tblLoginLayout.ResumeLayout(false);
            tblLoginLayout.PerformLayout();
            tblOptions.ResumeLayout(false);
            tblOptions.PerformLayout();
            pnlBrand.ResumeLayout(false);
            tblBrandLayout.ResumeLayout(false);
            ResumeLayout(false);
        }

        /// <summary>
        /// تنسيق عنوان رئيسي أو فرعي داخل بطاقة الدخول.
        /// </summary>
        private static void ConfigureTitleLabel(Label label, string text, float fontSize, Color foreColor)
        {
            label.Dock = DockStyle.Fill;
            label.Font = fontSize >= 20F
                ? UiTheme.CreateBoldFont(fontSize)
                : UiTheme.CreateRegularFont(fontSize);
            label.ForeColor = foreColor;
            label.Margin = Padding.Empty;
            label.RightToLeft = RightToLeft.Yes;
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleRight;
        }

        /// <summary>
        /// تنسيق عناوين الحقول العربية بمحاذاة يمين ثابتة.
        /// </summary>
        private static void ConfigureFieldLabel(Label label, string text)
        {
            label.Dock = DockStyle.Fill;
            label.Font = UiTheme.CreateBoldFont(10F);
            label.ForeColor = UiTheme.HeadingText;
            label.Margin = Padding.Empty;
            label.RightToLeft = RightToLeft.Yes;
            label.Text = text;
            label.TextAlign = ContentAlignment.BottomRight;
        }

        /// <summary>
        /// تنسيق حقل النص الإلزامي.
        /// </summary>
        private static void ConfigureRequiredTextBox(RequiredTextBox textBox, string requiredMessage)
        {
            textBox.Dock = DockStyle.Fill;
            textBox.Font = UiTheme.CreateRegularFont(11F);
            textBox.IsRequired = true;
            textBox.Margin = new Padding(0, 2, 0, 6);
            textBox.RequiredMessage = requiredMessage;
            textBox.RightToLeft = RightToLeft.Yes;
            textBox.TextAlign = HorizontalAlignment.Right;
        }

        /// <summary>
        /// تنسيق حقل كلمة المرور الموحد.
        /// </summary>
        private static void ConfigurePasswordTextBox(PasswordTextBox textBox, string requiredMessage)
        {
            textBox.Dock = DockStyle.Fill;
            textBox.Font = UiTheme.CreateRegularFont(11F);
            textBox.Margin = new Padding(0, 2, 0, 8);
            textBox.RequiredMessage = requiredMessage;
            textBox.RightToLeft = RightToLeft.Yes;
            textBox.TextAlign = HorizontalAlignment.Right;
        }

        /// <summary>
        /// تنسيق قائمة الاختيار الإلزامية.
        /// </summary>
        private static void ConfigureLookupComboBox(LookupComboBox comboBox, string requiredMessage)
        {
            comboBox.Dock = DockStyle.Fill;
            comboBox.Font = UiTheme.CreateRegularFont(11F);
            comboBox.IsRequired = true;
            comboBox.Margin = new Padding(0, 2, 0, 6);
            comboBox.RequiredMessage = requiredMessage;
            comboBox.RightToLeft = RightToLeft.Yes;
        }

        /// <summary>
        /// تنسيق نصوص لوحة تعريف النظام.
        /// </summary>
        private static void ConfigureBrandLabel(
            Label label,
            string text,
            float fontSize,
            Color foreColor,
            ContentAlignment alignment)
        {
            label.Dock = DockStyle.Fill;
            label.Font = fontSize >= 20F
                ? UiTheme.CreateBoldFont(fontSize)
                : UiTheme.CreateRegularFont(fontSize);
            label.ForeColor = foreColor;
            label.Margin = Padding.Empty;
            label.RightToLeft = RightToLeft.Yes;
            label.Text = text;
            label.TextAlign = alignment;
        }

        #endregion

        private RequiredTextBox txtUserName;
        private Label lblTitle;
        private LookupComboBox cmbBranch;
        private Label lblSubtitle;
        private Label lblBranch;
    }
}
