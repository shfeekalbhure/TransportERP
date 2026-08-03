using System.Drawing.Drawing2D;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop
{
    /// <summary>
    /// نافذة تسجيل الدخول الرئيسية لنظام TransportERP.
    /// تمثل النسخة المجمعة المعتمدة لشاشة LOGIN-001، وتستخدم عناصر CoreUI
    /// للحقول الإلزامية وقوائم الاختيار وزر الدخول وشريط الحالة.
    /// </summary>
    public partial class FrmLogin : Form
    {
        /// <summary>
        /// إنشاء نافذة تسجيل الدخول وتهيئة بياناتها ومظهرها وأحداثها.
        /// </summary>
        public FrmLogin()
        {
            InitializeComponent();
            InitializeLoginData();
            ApplyVisualStyle();
            RegisterInteractionEvents();
            UpdateStatusBarContext();
        }

        /// <summary>
        /// تعبئة قوائم الاختيار بقيم مؤقتة إلى حين ربطها بخدمات API الفعلية.
        /// </summary>
        private void InitializeLoginData()
        {
            cmbCompany.BindItems(new[] { "شركة النقل الرئيسية" });
            cmbBranch.BindItems(new[] { "الفرع الرئيسي" });
            cmbFiscalYear.BindItems(new[] { DateTime.Today.Year.ToString() });

            txtUserName.Focus();
        }

        /// <summary>
        /// تطبيق الخصائص البصرية الخاصة ببطاقة الدخول ولوحة تعريف النظام.
        /// عناصر الإدخال والزر وشريط الحالة تحصل على هويتها من CoreUI وUiTheme.
        /// </summary>
        private void ApplyVisualStyle()
        {
            DoubleBuffered = true;
            AcceptButton = btnLogin;

            pnlLoginCard.Resize += (_, _) => ApplyRoundedRegion(pnlLoginCard, 24);
            pnlBrand.Resize += (_, _) => ApplyRoundedRegion(pnlBrand, 24);
            pnlBrand.Paint += DrawBrandGradient;

            ApplyRoundedRegion(pnlLoginCard, 24);
            ApplyRoundedRegion(pnlBrand, 24);
        }

        /// <summary>
        /// تسجيل الأحداث التفاعلية التي تخص شاشة تسجيل الدخول.
        /// </summary>
        private void RegisterInteractionEvents()
        {
            lnkForgotPassword.LinkClicked += lnkForgotPassword_LinkClicked;
            cmbCompany.SelectedIndexChanged += (_, _) => UpdateStatusBarContext();
            cmbBranch.SelectedIndexChanged += (_, _) => UpdateStatusBarContext();
            cmbFiscalYear.SelectedIndexChanged += (_, _) => UpdateStatusBarContext();
        }

        /// <summary>
        /// رسم خلفية متدرجة للوحة تعريف النظام باستخدام ألوان الهوية المعتمدة.
        /// </summary>
        private void DrawBrandGradient(object? sender, PaintEventArgs e)
        {
            if (pnlBrand.ClientRectangle.Width <= 0 || pnlBrand.ClientRectangle.Height <= 0)
            {
                return;
            }

            using var brush = new LinearGradientBrush(
                pnlBrand.ClientRectangle,
                UiTheme.BrandGradientStart,
                UiTheme.BrandGradientEnd,
                35F);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillRectangle(brush, pnlBrand.ClientRectangle);
        }

        /// <summary>
        /// معالجة الضغط على رابط استعادة كلمة المرور.
        /// يعرض رسالة مؤقتة إلى حين تنفيذ خدمة الاستعادة عبر API.
        /// </summary>
        private void lnkForgotPassword_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(
                "سيتم ربط استعادة كلمة المرور بخدمة الأمان في مرحلة لاحقة.",
                "استعادة كلمة المرور",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// معالجة الضغط على زر الدخول الرئيسي.
        /// تنفذ تحققًا أوليًا فقط ولا تنفذ مصادقة حقيقية في هذه المرحلة.
        /// </summary>
        private void btnLogin_Click(object? sender, EventArgs e)
        {
            if (!ValidateLoginInputs())
            {
                return;
            }

            statusBar.CurrentUser = txtUserName.Text.Trim();
            statusBar.SetConnectionStatus(false, "المصادقة غير مرتبطة بعد");

            MessageBox.Show(
                "تم تجهيز شاشة تسجيل الدخول، وسيتم ربطها بخدمة المصادقة عبر API في المرحلة التالية.",
                "TransportERP",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// التحقق من الحقول الإلزامية باستخدام وظائف عناصر CoreUI نفسها.
        /// </summary>
        /// <returns>صحيح عندما تكون جميع بيانات الدخول مكتملة؛ وإلا يعيد خطأ.</returns>
        private bool ValidateLoginInputs()
        {
            return txtUserName.ValidateRequired()
                && txtPassword.ValidateRequired()
                && cmbCompany.ValidateSelection()
                && cmbBranch.ValidateSelection()
                && cmbFiscalYear.ValidateSelection();
        }

        /// <summary>
        /// تحديث شريط الحالة بقيم الشركة والفرع والسنة المحددة حاليًا.
        /// </summary>
        private void UpdateStatusBarContext()
        {
            statusBar.CompanyName = cmbCompany.SelectedItem?.ToString() ?? "قبل تسجيل الدخول";
            statusBar.BranchName = cmbBranch.SelectedItem?.ToString() ?? "قبل تسجيل الدخول";
            statusBar.FiscalYear = cmbFiscalYear.SelectedItem?.ToString() ?? DateTime.Today.Year.ToString();
            statusBar.FinancialPeriod = "-";
            statusBar.CurrentUser = string.IsNullOrWhiteSpace(txtUserName.Text)
                ? "غير مسجل"
                : txtUserName.Text.Trim();
            statusBar.CurrentRole = "-";
            statusBar.EnvironmentName = "TransportERP API";
            statusBar.SystemVersion = "1.0.0";
            statusBar.SetConnectionStatus(false, "لم يتم الفحص");
        }

        /// <summary>
        /// إنشاء حواف مستديرة لعنصر واجهة رسومي.
        /// </summary>
        /// <param name="control">العنصر الذي سيتم تطبيق الحواف المستديرة عليه.</param>
        /// <param name="radius">نصف قطر الاستدارة بالبكسل.</param>
        private static void ApplyRoundedRegion(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0)
            {
                return;
            }

            var diameter = radius * 2;
            var bounds = new Rectangle(0, 0, control.Width, control.Height);

            using var path = new GraphicsPath();
            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            control.Region?.Dispose();
            control.Region = new Region(path);
        }
    }
}
