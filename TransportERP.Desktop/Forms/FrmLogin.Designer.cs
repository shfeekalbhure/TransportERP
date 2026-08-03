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
        /// إنشاء عناصر واجهة تسجيل الدخول وترتيبها باستخدام مكتبة CoreUI المعتمدة.
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

            // الحاوية الرئيسية تقسم الشاشة إلى بطاقة دخول ولوحة تعريف بالنظام وشريط حالة سفلي.
            // أزيل الهامش السفلي حتى يثبت شريط الحالة ملاصقًا لحافة النافذة.
            tblRoot.Dock = DockStyle.Fill;
            tblRoot.ColumnCount = 2;
            tblRoot.RowCount = 2;
            tblRoot.Padding = new Padding(28, 28, 28, 0);
            tblRoot.BackColor = UiTheme.WindowBackground;
            tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
            tblRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));

            // بطاقة إدخال بيانات المستخدم.
            pnlLoginCard.Dock = DockStyle.Fill;
            pnlLoginCard.Margin = new Padding(0, 0, 14, 10);
            pnlLoginCard.Padding = new Padding(54, 38, 54, 24);
            pnlLoginCard.BackColor = Color.White;

            lblTitle.Text = "تسجيل الدخول";
            lblTitle.Font = UiTheme.CreateBoldFont(24F);
            lblTitle.ForeColor = UiTheme.HeadingText;
            lblTitle.Location = new Point(54, 38);
            lblTitle.Size = new Size(470, 50);
            lblTitle.TextAlign = ContentAlignment.MiddleRight;

            lblSubtitle.Text = "أدخل بياناتك للوصول إلى النظام";
            lblSubtitle.Font = UiTheme.CreateRegularFont(11F);
            lblSubtitle.ForeColor = UiTheme.SecondaryText;
            lblSubtitle.Location = new Point(54, 88);
            lblSubtitle.Size = new Size(470, 32);
            lblSubtitle.TextAlign = ContentAlignment.MiddleRight;

            ConfigureLabel(lblUserName, "اسم المستخدم", 142);
            ConfigureRequiredTextBox(txtUserName, 174, "يرجى إدخال اسم المستخدم.");

            ConfigureLabel(lblPassword, "كلمة المرور", 232);
            ConfigurePasswordTextBox(txtPassword, 264, "يرجى إدخال كلمة المرور.");

            ConfigureLabel(lblCompany, "الشركة", 322);
            ConfigureLookupComboBox(cmbCompany, 354, "يرجى اختيار الشركة.");

            ConfigureLabel(lblBranch, "الفرع", 412);
            ConfigureLookupComboBox(cmbBranch, 444, "يرجى اختيار الفرع.");

            ConfigureLabel(lblFiscalYear, "السنة المالية", 502);
            ConfigureLookupComboBox(cmbFiscalYear, 534, "يرجى اختيار السنة المالية.");

            chkRememberMe.Text = "تذكرني";
            chkRememberMe.Font = UiTheme.CreateRegularFont(10F);
            chkRememberMe.ForeColor = UiTheme.HeadingText;
            chkRememberMe.Location = new Point(54, 590);
            chkRememberMe.Size = new Size(180, 30);
            chkRememberMe.TextAlign = ContentAlignment.MiddleRight;

            lnkForgotPassword.Text = "نسيت كلمة المرور؟";
            lnkForgotPassword.Font = UiTheme.CreateRegularFont(10F);
            lnkForgotPassword.LinkColor = UiTheme.PrimaryBlue;
            lnkForgotPassword.ActiveLinkColor = UiTheme.PrimaryBlueHover;
            lnkForgotPassword.Location = new Point(334, 590);
            lnkForgotPassword.Size = new Size(190, 30);
            lnkForgotPassword.TextAlign = ContentAlignment.MiddleLeft;

            // زر الإجراء الرئيسي الوحيد في الشاشة ويستخدم العنصر الموحد PrimaryButton.
            btnLogin.Text = "دخول";
            btnLogin.CornerRadius = 12;
            btnLogin.Font = UiTheme.CreateBoldFont(12F);
            btnLogin.Location = new Point(54, 638);
            btnLogin.Size = new Size(470, 52);
            btnLogin.Click += btnLogin_Click;

            lblLanguage.Text = "العربية  |  English";
            lblLanguage.Font = UiTheme.CreateRegularFont(9F);
            lblLanguage.ForeColor = UiTheme.SecondaryText;
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
            pnlBrand.Margin = new Padding(14, 0, 0, 10);
            pnlBrand.Padding = new Padding(56);
            pnlBrand.BackColor = UiTheme.BrandGradientStart;

            lblBrandName.Text = "TransportERP";
            lblBrandName.Font = UiTheme.CreateBoldFont(30F);
            lblBrandName.ForeColor = Color.White;
            lblBrandName.Location = new Point(56, 90);
            lblBrandName.Size = new Size(560, 64);
            lblBrandName.TextAlign = ContentAlignment.MiddleRight;

            lblBrandDescription.Text = "نظام النقل والخدمات اللوجستية المتكامل";
            lblBrandDescription.Font = UiTheme.CreateRegularFont(15F);
            lblBrandDescription.ForeColor = Color.FromArgb(220, 235, 255);
            lblBrandDescription.Location = new Point(56, 160);
            lblBrandDescription.Size = new Size(560, 54);
            lblBrandDescription.TextAlign = ContentAlignment.MiddleRight;

            lblBrandFeatures.Text = "إدارة الشركات والفروع والحسابات من منصة موحدة وآمنة.\r\n\r\n✓ واجهة عربية حديثة\r\n✓ متعدد الشركات والفروع\r\n✓ اتصال مركزي عبر API\r\n✓ قابل للتوسع لتطبيقات الجوال";
            lblBrandFeatures.Font = UiTheme.CreateRegularFont(13F);
            lblBrandFeatures.ForeColor = Color.White;
            lblBrandFeatures.Location = new Point(56, 250);
            lblBrandFeatures.Size = new Size(560, 360);
            lblBrandFeatures.TextAlign = ContentAlignment.TopRight;

            pnlBrand.Controls.AddRange(new Control[] { lblBrandName, lblBrandDescription, lblBrandFeatures });

            // شريط حالة مختصر خاص بشاشة الدخول، مثبت في أسفل النافذة.
            statusBar.Dock = DockStyle.Fill;
            statusBar.Margin = new Padding(0);
            statusBar.CompanyName = "قبل تسجيل الدخول";
            statusBar.BranchName = "قبل تسجيل الدخول";
            statusBar.FiscalYear = DateTime.Today.Year.ToString();
            statusBar.SystemVersion = "1.0.0";
            statusBar.SetConnectionStatus(false, "لم يتم الفحص");
            statusBar.UseLoginCompactMode();

            tblRoot.Controls.Add(pnlLoginCard, 0, 0);
            tblRoot.Controls.Add(pnlBrand, 1, 0);
            tblRoot.Controls.Add(statusBar, 0, 1);
            tblRoot.SetColumnSpan(statusBar, 2);

            // خصائص النافذة الرئيسية.
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
        /// توحيد خصائص عناوين الحقول داخل بطاقة تسجيل الدخول.
        /// </summary>
        private static void ConfigureLabel(Label label, string text, int top)
        {
            label.Text = text;
            label.Font = UiTheme.CreateBoldFont(10F);
            label.ForeColor = UiTheme.HeadingText;
            label.Location = new Point(54, top);
            label.Size = new Size(470, 28);
            label.TextAlign = ContentAlignment.MiddleRight;
        }

        /// <summary>
        /// توحيد خصائص الحقول الإلزامية باستخدام RequiredTextBox.
        /// </summary>
        private static void ConfigureRequiredTextBox(
            RequiredTextBox textBox,
            int top,
            string requiredMessage)
        {
            textBox.Font = UiTheme.CreateRegularFont(11F);
            textBox.Location = new Point(54, top);
            textBox.Size = new Size(470, 32);
            textBox.IsRequired = true;
            textBox.RequiredMessage = requiredMessage;
            textBox.RightToLeft = RightToLeft.Yes;
        }

        /// <summary>
        /// توحيد خصائص حقل كلمة المرور مع زر الإظهار والإخفاء.
        /// </summary>
        private static void ConfigurePasswordTextBox(
            PasswordTextBox textBox,
            int top,
            string requiredMessage)
        {
            textBox.Font = UiTheme.CreateRegularFont(11F);
            textBox.Location = new Point(54, top);
            textBox.Size = new Size(470, 38);
            textBox.RequiredMessage = requiredMessage;
            textBox.RightToLeft = RightToLeft.Yes;
        }

        /// <summary>
        /// توحيد خصائص قوائم الاختيار باستخدام LookupComboBox.
        /// </summary>
        private static void ConfigureLookupComboBox(
            LookupComboBox comboBox,
            int top,
            string requiredMessage)
        {
            comboBox.Font = UiTheme.CreateRegularFont(11F);
            comboBox.Location = new Point(54, top);
            comboBox.Size = new Size(470, 33);
            comboBox.IsRequired = true;
            comboBox.RequiredMessage = requiredMessage;
            comboBox.RightToLeft = RightToLeft.Yes;
        }

        #endregion
    }
}
