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
        private Label lblTitle = null!;
        private Label lblSubtitle = null!;
        private Label lblUserName = null!;
        private Label lblPassword = null!;
        private Label lblCompany = null!;
        private Label lblBranch = null!;
        private Label lblFiscalYear = null!;
        private RequiredTextBox txtUserName = null!;
        private PasswordTextBox txtPassword = null!;
        private LookupComboBox cmbCompany = null!;
        private LookupComboBox cmbBranch = null!;
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
            components = new System.ComponentModel.Container();
            tblRoot = new TableLayoutPanel();
            tblContent = new TableLayoutPanel();
            tblLoginLayout = new TableLayoutPanel();
            tblOptions = new TableLayoutPanel();
            tblBrandLayout = new TableLayoutPanel();
            pnlLoginCard = new Panel();
            pnlBrand = new Panel();
            lblTitle = new Label();
            lblSubtitle = new Label();
            lblUserName = new Label();
            lblPassword = new Label();
            lblCompany = new Label();
            lblBranch = new Label();
            lblFiscalYear = new Label();
            txtUserName = new RequiredTextBox();
            txtPassword = new PasswordTextBox();
            cmbCompany = new LookupComboBox();
            cmbBranch = new LookupComboBox();
            cmbFiscalYear = new LookupComboBox();
            chkRememberMe = new CheckBox();
            lnkForgotPassword = new LinkLabel();
            btnLogin = new PrimaryButton();
            lblLanguage = new Label();
            lblBrandName = new Label();
            lblBrandDescription = new Label();
            lblBrandFeatures = new Label();
            statusBar = new TransportStatusBar();

            SuspendLayout();

            // الحاوية العامة: المحتوى في الأعلى وشريط الحالة ثابت في الأسفل.
            tblRoot.BackColor = UiTheme.WindowBackground;
            tblRoot.ColumnCount = 1;
            tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblRoot.Dock = DockStyle.Fill;
            tblRoot.Margin = Padding.Empty;
            tblRoot.Padding = new Padding(24, 18, 24, 0);
            tblRoot.RowCount = 2;
            tblRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));

            // منطقة المحتوى: بطاقة الدخول في اليمين ولوحة الهوية في اليسار.
            tblContent.BackColor = Color.Transparent;
            tblContent.ColumnCount = 2;
            tblContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            tblContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
            tblContent.Dock = DockStyle.Fill;
            tblContent.Margin = Padding.Empty;
            tblContent.RowCount = 1;
            tblContent.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblContent.RightToLeft = RightToLeft.Yes;

            // بطاقة تسجيل الدخول.
            pnlLoginCard.BackColor = Color.White;
            pnlLoginCard.Dock = DockStyle.Fill;
            pnlLoginCard.Margin = new Padding(0, 0, 12, 8);
            pnlLoginCard.Padding = new Padding(48, 28, 48, 22);

            // التخطيط الداخلي المرن لبطاقة الدخول.
            tblLoginLayout.BackColor = Color.Transparent;
            tblLoginLayout.ColumnCount = 1;
            tblLoginLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblLoginLayout.Dock = DockStyle.Fill;
            tblLoginLayout.Margin = Padding.Empty;
            tblLoginLayout.RowCount = 16;
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tblLoginLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblLoginLayout.RightToLeft = RightToLeft.Yes;

            ConfigureTitleLabel(lblTitle, "تسجيل الدخول", 24F, UiTheme.HeadingText);
            ConfigureTitleLabel(lblSubtitle, "أدخل بياناتك للوصول إلى النظام", 11F, UiTheme.SecondaryText);
            ConfigureFieldLabel(lblUserName, "اسم المستخدم");
            ConfigureFieldLabel(lblPassword, "كلمة المرور");
            ConfigureFieldLabel(lblCompany, "الشركة");
            ConfigureFieldLabel(lblBranch, "الفرع");
            ConfigureFieldLabel(lblFiscalYear, "السنة المالية");

            ConfigureRequiredTextBox(txtUserName, "يرجى إدخال اسم المستخدم.");
            ConfigurePasswordTextBox(txtPassword, "يرجى إدخال كلمة المرور.");
            ConfigureLookupComboBox(cmbCompany, "يرجى اختيار الشركة.");
            ConfigureLookupComboBox(cmbBranch, "يرجى اختيار الفرع.");
            ConfigureLookupComboBox(cmbFiscalYear, "يرجى اختيار السنة المالية.");

            // خيارات تذكر المستخدم واستعادة كلمة المرور.
            tblOptions.ColumnCount = 2;
            tblOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblOptions.Dock = DockStyle.Fill;
            tblOptions.Margin = Padding.Empty;
            tblOptions.RowCount = 1;
            tblOptions.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblOptions.RightToLeft = RightToLeft.Yes;

            chkRememberMe.AutoSize = true;
            chkRememberMe.Dock = DockStyle.Fill;
            chkRememberMe.Font = UiTheme.CreateRegularFont(10F);
            chkRememberMe.ForeColor = UiTheme.HeadingText;
            chkRememberMe.Margin = Padding.Empty;
            chkRememberMe.Text = "تذكرني";
            chkRememberMe.TextAlign = ContentAlignment.MiddleRight;

            lnkForgotPassword.ActiveLinkColor = UiTheme.PrimaryBlueHover;
            lnkForgotPassword.Dock = DockStyle.Fill;
            lnkForgotPassword.Font = UiTheme.CreateRegularFont(10F);
            lnkForgotPassword.LinkColor = UiTheme.PrimaryBlue;
            lnkForgotPassword.Margin = Padding.Empty;
            lnkForgotPassword.Text = "نسيت كلمة المرور؟";
            lnkForgotPassword.TextAlign = ContentAlignment.MiddleLeft;

            tblOptions.Controls.Add(chkRememberMe, 0, 0);
            tblOptions.Controls.Add(lnkForgotPassword, 1, 0);

            // زر الإجراء الرئيسي الوحيد.
            btnLogin.CornerRadius = 12;
            btnLogin.Dock = DockStyle.Fill;
            btnLogin.Font = UiTheme.CreateBoldFont(12F);
            btnLogin.Margin = new Padding(0, 4, 0, 4);
            btnLogin.Text = "دخول";
            btnLogin.Click += btnLogin_Click;

            lblLanguage.Dock = DockStyle.Fill;
            lblLanguage.Font = UiTheme.CreateRegularFont(9F);
            lblLanguage.ForeColor = UiTheme.SecondaryText;
            lblLanguage.Margin = Padding.Empty;
            lblLanguage.Text = "العربية  |  English";
            lblLanguage.TextAlign = ContentAlignment.MiddleCenter;

            tblLoginLayout.Controls.Add(lblTitle, 0, 0);
            tblLoginLayout.Controls.Add(lblSubtitle, 0, 1);
            tblLoginLayout.Controls.Add(lblUserName, 0, 2);
            tblLoginLayout.Controls.Add(txtUserName, 0, 3);
            tblLoginLayout.Controls.Add(lblPassword, 0, 4);
            tblLoginLayout.Controls.Add(txtPassword, 0, 5);
            tblLoginLayout.Controls.Add(lblCompany, 0, 6);
            tblLoginLayout.Controls.Add(cmbCompany, 0, 7);
            tblLoginLayout.Controls.Add(lblBranch, 0, 8);
            tblLoginLayout.Controls.Add(cmbBranch, 0, 9);
            tblLoginLayout.Controls.Add(lblFiscalYear, 0, 10);
            tblLoginLayout.Controls.Add(cmbFiscalYear, 0, 11);
            tblLoginLayout.Controls.Add(tblOptions, 0, 12);
            tblLoginLayout.Controls.Add(btnLogin, 0, 13);
            tblLoginLayout.Controls.Add(lblLanguage, 0, 14);
            pnlLoginCard.Controls.Add(tblLoginLayout);

            // لوحة تعريف النظام.
            pnlBrand.BackColor = UiTheme.BrandGradientStart;
            pnlBrand.Dock = DockStyle.Fill;
            pnlBrand.Margin = new Padding(12, 0, 0, 8);
            pnlBrand.Padding = new Padding(54, 54, 54, 44);

            tblBrandLayout.BackColor = Color.Transparent;
            tblBrandLayout.ColumnCount = 1;
            tblBrandLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblBrandLayout.Dock = DockStyle.Fill;
            tblBrandLayout.Margin = Padding.Empty;
            tblBrandLayout.RowCount = 4;
            tblBrandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));
            tblBrandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tblBrandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tblBrandLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblBrandLayout.RightToLeft = RightToLeft.Yes;

            ConfigureBrandLabel(lblBrandName, "TransportERP", 30F, Color.White, ContentAlignment.MiddleRight);
            ConfigureBrandLabel(lblBrandDescription, "نظام النقل والخدمات اللوجستية المتكامل", 15F, Color.FromArgb(220, 235, 255), ContentAlignment.MiddleRight);
            ConfigureBrandLabel(
                lblBrandFeatures,
                "إدارة الشركات والفروع والحسابات من منصة موحدة وآمنة.\r\n\r\n✓ واجهة عربية حديثة\r\n✓ متعدد الشركات والفروع\r\n✓ اتصال مركزي عبر API\r\n✓ قابل للتوسع لتطبيقات الجوال",
                13F,
                Color.White,
                ContentAlignment.TopRight);

            tblBrandLayout.Controls.Add(lblBrandName, 0, 0);
            tblBrandLayout.Controls.Add(lblBrandDescription, 0, 1);
            tblBrandLayout.Controls.Add(lblBrandFeatures, 0, 3);
            pnlBrand.Controls.Add(tblBrandLayout);

            // شريط حالة مختصر خاص بشاشة الدخول.
            statusBar.CompanyName = "قبل تسجيل الدخول";
            statusBar.Dock = DockStyle.Fill;
            statusBar.FiscalYear = DateTime.Today.Year.ToString();
            statusBar.Margin = Padding.Empty;
            statusBar.SystemVersion = "1.0.0";
            statusBar.BranchName = "قبل تسجيل الدخول";
            statusBar.SetConnectionStatus(false, "لم يتم الفحص");
            statusBar.UseLoginCompactMode();

            tblContent.Controls.Add(pnlLoginCard, 0, 0);
            tblContent.Controls.Add(pnlBrand, 1, 0);
            tblRoot.Controls.Add(tblContent, 0, 0);
            tblRoot.Controls.Add(statusBar, 0, 1);

            // خصائص النافذة.
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = UiTheme.WindowBackground;
            ClientSize = new Size(1360, 820);
            Controls.Add(tblRoot);
            Font = UiTheme.CreateRegularFont(10F);
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
    }
}
