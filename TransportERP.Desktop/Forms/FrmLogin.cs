using System.Drawing.Drawing2D;

namespace TransportERP.Desktop
{
    /// <summary>
    /// نافذة تسجيل الدخول الرئيسية لنظام TransportERP.
    /// تعرض حقول اختيار الشركة والفرع والسنة المالية، وتتحقق مبدئيًا من بيانات المستخدم
    /// قبل الانتقال لاحقًا إلى عملية المصادقة الفعلية عبر واجهة API.
    /// </summary>
    public partial class FrmLogin : Form
    {
        /// <summary>
        /// لون زر الدخول في حالته الطبيعية.
        /// </summary>
        private readonly Color _loginButtonColor = Color.FromArgb(35, 111, 229);

        /// <summary>
        /// لون زر الدخول عند مرور مؤشر الفأرة فوقه.
        /// </summary>
        private readonly Color _loginButtonHoverColor = Color.FromArgb(24, 88, 197);

        /// <summary>
        /// إنشاء نافذة تسجيل الدخول وتهيئة عناصرها وقيمها الافتراضية.
        /// </summary>
        public FrmLogin()
        {
            InitializeComponent();
            InitializeLoginData();
            ApplyVisualStyle();
            RegisterInteractionEvents();
        }

        /// <summary>
        /// تعبئة قوائم الاختيار بقيم مؤقتة إلى حين ربطها بواجهات النظام الفعلية.
        /// </summary>
        private void InitializeLoginData()
        {
            cmbCompany.Items.Clear();
            cmbCompany.Items.Add("شركة النقل الرئيسية");
            cmbCompany.SelectedIndex = 0;

            cmbBranch.Items.Clear();
            cmbBranch.Items.Add("الفرع الرئيسي");
            cmbBranch.SelectedIndex = 0;

            cmbFiscalYear.Items.Clear();
            cmbFiscalYear.Items.Add(DateTime.Today.Year.ToString());
            cmbFiscalYear.SelectedIndex = 0;

            txtUserName.Focus();
        }

        /// <summary>
        /// تطبيق الهوية البصرية المعتمدة على بطاقة تسجيل الدخول ولوحة تعريف النظام.
        /// </summary>
        private void ApplyVisualStyle()
        {
            DoubleBuffered = true;
            AcceptButton = btnLogin;

            pnlLoginCard.Resize += (_, _) => ApplyRoundedRegion(pnlLoginCard, 24);
            pnlBrand.Resize += (_, _) => ApplyRoundedRegion(pnlBrand, 24);
            btnLogin.Resize += (_, _) => ApplyRoundedRegion(btnLogin, 12);

            pnlBrand.Paint += DrawBrandGradient;

            ApplyRoundedRegion(pnlLoginCard, 24);
            ApplyRoundedRegion(pnlBrand, 24);
            ApplyRoundedRegion(btnLogin, 12);

            txtUserName.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.BorderStyle = BorderStyle.FixedSingle;

            cmbCompany.FlatStyle = FlatStyle.Flat;
            cmbBranch.FlatStyle = FlatStyle.Flat;
            cmbFiscalYear.FlatStyle = FlatStyle.Flat;
        }

        /// <summary>
        /// تسجيل الأحداث البصرية والتفاعلية لعناصر شاشة الدخول.
        /// </summary>
        private void RegisterInteractionEvents()
        {
            btnLogin.MouseEnter += (_, _) => btnLogin.BackColor = _loginButtonHoverColor;
            btnLogin.MouseLeave += (_, _) => btnLogin.BackColor = _loginButtonColor;

            txtUserName.Enter += HighlightInput;
            txtPassword.Enter += HighlightInput;
            txtUserName.Leave += RestoreInput;
            txtPassword.Leave += RestoreInput;

            lnkForgotPassword.LinkClicked += lnkForgotPassword_LinkClicked;
        }

        /// <summary>
        /// رسم خلفية متدرجة للوحة تعريف النظام بدل اللون الثابت.
        /// </summary>
        private void DrawBrandGradient(object? sender, PaintEventArgs e)
        {
            if (pnlBrand.ClientRectangle.Width <= 0 || pnlBrand.ClientRectangle.Height <= 0)
            {
                return;
            }

            using var brush = new LinearGradientBrush(
                pnlBrand.ClientRectangle,
                Color.FromArgb(17, 58, 140),
                Color.FromArgb(38, 132, 232),
                35F);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillRectangle(brush, pnlBrand.ClientRectangle);
        }

        /// <summary>
        /// تمييز حقل النص النشط بخلفية فاتحة لتوضيح موضع الإدخال للمستخدم.
        /// </summary>
        private static void HighlightInput(object? sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.BackColor = Color.FromArgb(245, 249, 255);
            }
        }

        /// <summary>
        /// إعادة لون حقل النص إلى حالته الطبيعية بعد مغادرة الحقل.
        /// </summary>
        private static void RestoreInput(object? sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.BackColor = Color.White;
            }
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
        /// تنفذ تحققًا أوليًا من الحقول المطلوبة فقط، ولا تنفذ مصادقة حقيقية حاليًا.
        /// </summary>
        private void btnLogin_Click(object? sender, EventArgs e)
        {
            if (!ValidateLoginInputs())
            {
                return;
            }

            MessageBox.Show(
                "تم تجهيز شاشة تسجيل الدخول، وسيتم ربطها بخدمة المصادقة عبر API في المرحلة التالية.",
                "TransportERP",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// التحقق من إدخال اسم المستخدم وكلمة المرور واختيار بيانات بيئة العمل.
        /// </summary>
        /// <returns>صحيح عندما تكون جميع البيانات المطلوبة مكتملة؛ وإلا يعيد خطأ.</returns>
        private bool ValidateLoginInputs()
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                ShowValidationMessage("يرجى إدخال اسم المستخدم.", txtUserName);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowValidationMessage("يرجى إدخال كلمة المرور.", txtPassword);
                return false;
            }

            if (cmbCompany.SelectedIndex < 0)
            {
                ShowValidationMessage("يرجى اختيار الشركة.", cmbCompany);
                return false;
            }

            if (cmbBranch.SelectedIndex < 0)
            {
                ShowValidationMessage("يرجى اختيار الفرع.", cmbBranch);
                return false;
            }

            if (cmbFiscalYear.SelectedIndex < 0)
            {
                ShowValidationMessage("يرجى اختيار السنة المالية.", cmbFiscalYear);
                return false;
            }

            return true;
        }

        /// <summary>
        /// عرض رسالة تحقق موحدة ثم نقل التركيز إلى العنصر الذي يحتاج إلى إدخال.
        /// </summary>
        /// <param name="message">نص رسالة التحقق المعروضة للمستخدم.</param>
        /// <param name="control">العنصر المطلوب نقل التركيز إليه.</param>
        private static void ShowValidationMessage(string message, Control control)
        {
            MessageBox.Show(
                message,
                "تنبيه",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            control.Focus();
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
