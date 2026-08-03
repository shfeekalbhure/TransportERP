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
        /// إنشاء نافذة تسجيل الدخول وتهيئة عناصرها وقيمها الافتراضية.
        /// </summary>
        public FrmLogin()
        {
            InitializeComponent();
            InitializeLoginData();
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
    }
}
