namespace TransportERP.Desktop
{
    partial class FrmLogin
    {
        /// <summary>
        /// حاوية المكونات التي ينشئها مصمم Windows Forms.
        /// </summary>
        private System.ComponentModel.IContainer? components = null;

        private TableLayoutPanel tblRoot = null!;
        private Panel pnlLoginCard = null!;
        private Panel pnlBrand = null!;
        private Label lblTitle = null!;
        private Label lblSubtitle = null!;
        private Label lblUserName = null!;
        private Label lblPassword = null!;
        private Label lblCompany = null!;
        private Label lblBranch = null!;
        private Label lblFiscalYear = null!;
        private TextBox txtUserName = null!;
        private TextBox txtPassword = null!;
        private ComboBox cmbCompany = null!;
        private ComboBox cmbBranch = null!;
        private ComboBox cmbFiscalYear = null!;
        private CheckBox chkRememberMe = null!;
        private LinkLabel lnkForgotPassword = null!;
        private Button btnLogin = null!;
        private Label lblLanguage = null!;
        private Label lblBrandName = null!;
        private Label lblBrandDescription = null!;
        private Label lblBrandFeatures = null!;
        private Label lblStatus = null!;

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
        /// إنشاء عناصر واجهة تسجيل الدخول وترتيبها.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tblRoot = new TableLayoutPanel();
            pnlLoginCard = new Panel();
            pnlBrand = new Panel();
            lblTitle = new Label();
            lblSubtitle = new Label();
            lblUserName = new Label();
            lblPassword = new Label();
            lblCompany = new Label();
            lblBranch = new Label();
            lblFiscalYear = new Label();
            txtUserName = new TextBox();
            txtPassword = new TextBox();
            cmbCompany = new ComboBox();
            cmbBranch = new ComboBox();
            cmbFiscalYear = new ComboBox();
            chkRememberMe = new CheckBox();
            lnkForgotPassword = new LinkLabel();
            btnLogin = new Button();
            lblLanguage = new Label();
            lblBrandName = new Label();
            lblBrandDescription = new Label();
            lblBrandFeatures = new Label();
            lblStatus = new Label();

            SuspendLayout();

            // الحاوية الرئيسية تقسم الشاشة إلى بطاقة دخول ولوحة تعريف بالنظام.
            tblRoot.Dock = DockStyle.Fill;
            tblRoot.ColumnCount = 2;
            tblRoot.RowCount = 2;
            tblRoot.Padding = new Padding(28);
            tblRoot.BackColor = Color.FromArgb(239, 245, 252);
            tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
            tblRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));

            // بطاقة إدخال بيانات المستخدم.
            pnlLoginCard.Dock = DockStyle.Fill;
            pnlLoginCard.Margin = new Padding(0, 0, 14, 0);
            pnlLoginCard.Padding = new Padding(54, 38, 54, 34);
            pnlLoginCard.BackColor = Color.White;

            lblTitle.Text = "تسجيل الدخول";
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(17, 43, 78);
            lblTitle.Location = new Point(54, 38);
            lblTitle.Size = new Size(470, 50);
            lblTitle.TextAlign = ContentAlignment.MiddleRight;

            lblSubtitle.Text = "أدخل بياناتك للوصول إلى النظام";
            lblSubtitle.Font = new Font("Segoe UI", 11F);
            lblSubtitle.ForeColor = Color.FromArgb(104, 122, 147);
            lblSubtitle.Location = new Point(54, 88);
            lblSubtitle.Size = new Size(470, 32);
            lblSubtitle.TextAlign = ContentAlignment.MiddleRight;

            ConfigureLabel(lblUserName, "اسم المستخدم", 142);
            ConfigureTextBox(txtUserName, 174);

            ConfigureLabel(lblPassword, "كلمة المرور", 232);
            ConfigureTextBox(txtPassword, 264);
            txtPassword.UseSystemPasswordChar = true;

            ConfigureLabel(lblCompany, "الشركة", 322);
            ConfigureComboBox(cmbCompany, 354);

            ConfigureLabel(lblBranch, "الفرع", 412);
            ConfigureComboBox(cmbBranch, 444);

            ConfigureLabel(lblFiscalYear, "السنة المالية", 502);
            ConfigureComboBox(cmbFiscalYear, 534);

            chkRememberMe.Text = "تذكرني";
            chkRememberMe.Font = new Font("Segoe UI", 10F);
            chkRememberMe.ForeColor = Color.FromArgb(50, 67, 91);
            chkRememberMe.Location = new Point(54, 590);
            chkRememberMe.Size = new Size(180, 30);
            chkRememberMe.TextAlign = ContentAlignment.MiddleRight;

            lnkForgotPassword.Text = "نسيت كلمة المرور؟";
            lnkForgotPassword.Font = new Font("Segoe UI", 10F);
            lnkForgotPassword.LinkColor = Color.FromArgb(35, 111, 229);
            lnkForgotPassword.ActiveLinkColor = Color.FromArgb(23, 78, 170);
            lnkForgotPassword.Location = new Point(334, 590);
            lnkForgotPassword.Size = new Size(190, 30);
            lnkForgotPassword.TextAlign = ContentAlignment.MiddleLeft;

            // زر الإجراء الرئيسي الوحيد في الشاشة.
            btnLogin.Text = "دخول";
            btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.BackColor = Color.FromArgb(35, 111, 229);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Location = new Point(54, 638);
            btnLogin.Size = new Size(470, 52);
            btnLogin.Click += btnLogin_Click;

            lblLanguage.Text = "العربية  |  English";
            lblLanguage.Font = new Font("Segoe UI", 9F);
            lblLanguage.ForeColor = Color.FromArgb(91, 111, 139);
            lblLanguage.Location = new Point(54, 704);
            lblLanguage.Size = new Size(470, 28);
            lblLanguage.TextAlign = ContentAlignment.MiddleCenter;

            pnlLoginCard.Controls.AddRange(new Control[]
            {
                lblTitle, lblSubtitle, lblUserName, txtUserName, lblPassword, txtPassword,
                lblCompany, cmbCompany, lblBranch, cmbBranch, lblFiscalYear, cmbFiscalYear,
                chkRememberMe, lnkForgotPassword, btnLogin, lblLanguage
            });

            // لوحة تعريف النظام وهوية المنتج.
            pnlBrand.Dock = DockStyle.Fill;
            pnlBrand.Margin = new Padding(14, 0, 0, 0);
            pnlBrand.Padding = new Padding(56);
            pnlBrand.BackColor = Color.FromArgb(28, 91, 190);

            lblBrandName.Text = "TransportERP";
            lblBrandName.Font = new Font("Segoe UI", 30F, FontStyle.Bold);
            lblBrandName.ForeColor = Color.White;
            lblBrandName.Location = new Point(56, 90);
            lblBrandName.Size = new Size(560, 64);
            lblBrandName.TextAlign = ContentAlignment.MiddleRight;

            lblBrandDescription.Text = "نظام النقل والخدمات اللوجستية المتكامل";
            lblBrandDescription.Font = new Font("Segoe UI", 15F);
            lblBrandDescription.ForeColor = Color.FromArgb(220, 235, 255);
            lblBrandDescription.Location = new Point(56, 160);
            lblBrandDescription.Size = new Size(560, 54);
            lblBrandDescription.TextAlign = ContentAlignment.MiddleRight;

            lblBrandFeatures.Text = "إدارة الشركات والفروع والحسابات من منصة موحدة وآمنة.\r\n\r\n✓ واجهة عربية حديثة\r\n✓ متعدد الشركات والفروع\r\n✓ اتصال مركزي عبر API\r\n✓ قابل للتوسع لتطبيقات الجوال";
            lblBrandFeatures.Font = new Font("Segoe UI", 13F);
            lblBrandFeatures.ForeColor = Color.White;
            lblBrandFeatures.Location = new Point(56, 250);
            lblBrandFeatures.Size = new Size(560, 360);
            lblBrandFeatures.TextAlign = ContentAlignment.TopRight;

            pnlBrand.Controls.AddRange(new Control[] { lblBrandName, lblBrandDescription, lblBrandFeatures });

            // شريط الحالة السفلي.
            lblStatus.Text = "الإصدار 1.0.0     •     متصل     •     الخادم: TransportERP API";
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.FromArgb(91, 111, 139);
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;

            tblRoot.Controls.Add(pnlLoginCard, 0, 0);
            tblRoot.Controls.Add(pnlBrand, 1, 0);
            tblRoot.Controls.Add(lblStatus, 0, 1);
            tblRoot.SetColumnSpan(lblStatus, 2);

            // خصائص النافذة الرئيسية.
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(239, 245, 252);
            ClientSize = new Size(1360, 820);
            Controls.Add(tblRoot);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimumSize = new Size(1180, 720);
            Name = "FrmLogin";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TransportERP - تسجيل الدخول";

            ResumeLayout(false);
        }

        /// <summary>
        /// توحيد خصائص عناوين الحقول داخل بطاقة تسجيل الدخول.
        /// </summary>
        private static void ConfigureLabel(Label label, string text, int top)
        {
            label.Text = text;
            label.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label.ForeColor = Color.FromArgb(50, 67, 91);
            label.Location = new Point(54, top);
            label.Size = new Size(470, 28);
            label.TextAlign = ContentAlignment.MiddleRight;
        }

        /// <summary>
        /// توحيد خصائص حقول النص في الشاشة.
        /// </summary>
        private static void ConfigureTextBox(TextBox textBox, int top)
        {
            textBox.Font = new Font("Segoe UI", 11F);
            textBox.Location = new Point(54, top);
            textBox.Size = new Size(470, 32);
            textBox.RightToLeft = RightToLeft.Yes;
        }

        /// <summary>
        /// توحيد خصائص قوائم الاختيار في الشاشة.
        /// </summary>
        private static void ConfigureComboBox(ComboBox comboBox, int top)
        {
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.Font = new Font("Segoe UI", 11F);
            comboBox.Location = new Point(54, top);
            comboBox.Size = new Size(470, 33);
            comboBox.RightToLeft = RightToLeft.Yes;
        }

        #endregion
    }
}
