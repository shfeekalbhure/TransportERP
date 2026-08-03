using System.ComponentModel;
using TransportERP.Desktop.Controls;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// حقل كلمة مرور موحد يدعم الإلزامية وإظهار أو إخفاء القيمة.
/// يُستخدم في تسجيل الدخول وتغيير كلمة المرور وإدارة المستخدمين.
/// </summary>
[ToolboxItem(true)]
public sealed class PasswordTextBox : UserControl
{
    private readonly RequiredTextBox _textBox = new();
    private readonly Button _toggleButton = new();
    private readonly ToolTip _toolTip = new();
    private bool _isPasswordVisible;

    /// <summary>
    /// إنشاء حقل كلمة المرور وتطبيق التنسيق والأحداث.
    /// </summary>
    public PasswordTextBox()
    {
        InitializeLayout();
        UpdatePasswordVisibility();
    }

    /// <summary>
    /// قيمة كلمة المرور الحالية.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override string Text
    {
        get => _textBox.Text;
        set => _textBox.Text = value ?? string.Empty;
    }

    /// <summary>
    /// رسالة التحقق العربية للحقل الإلزامي.
    /// </summary>
    [Category("TransportERP")]
    [DefaultValue("يرجى إدخال كلمة المرور.")]
    public string RequiredMessage
    {
        get => _textBox.RequiredMessage;
        set => _textBox.RequiredMessage = value;
    }

    /// <summary>
    /// التحقق من إدخال كلمة المرور.
    /// </summary>
    public bool ValidateRequired(bool showMessage = true) => _textBox.ValidateRequired(showMessage);

    /// <summary>
    /// نقل التركيز إلى مربع كلمة المرور الداخلي.
    /// </summary>
    public new void Focus() => _textBox.Focus();

    /// <summary>
    /// تهيئة مكونات الحقل وترتيب زر الإظهار داخله.
    /// </summary>
    private void InitializeLayout()
    {
        Height = 38;
        MinimumSize = new Size(180, 38);
        BackColor = Color.White;
        RightToLeft = RightToLeft.Yes;

        _toggleButton.Dock = DockStyle.Left;
        _toggleButton.Width = 42;
        _toggleButton.FlatStyle = FlatStyle.Flat;
        _toggleButton.FlatAppearance.BorderSize = 0;
        _toggleButton.BackColor = Color.Transparent;
        _toggleButton.ForeColor = UiTheme.SecondaryText;
        _toggleButton.Font = UiTheme.CreateRegularFont(12F);
        _toggleButton.Cursor = Cursors.Hand;
        _toggleButton.TabStop = false;
        _toggleButton.Click += (_, _) =>
        {
            _isPasswordVisible = !_isPasswordVisible;
            UpdatePasswordVisibility();
            _textBox.Focus();
        };

        _textBox.Dock = DockStyle.Fill;
        _textBox.IsRequired = true;
        _textBox.RequiredMessage = "يرجى إدخال كلمة المرور.";
        _textBox.Font = UiTheme.CreateRegularFont(11F);
        _textBox.RightToLeft = RightToLeft.Yes;

        Controls.Add(_textBox);
        Controls.Add(_toggleButton);
    }

    /// <summary>
    /// تحديث طريقة عرض كلمة المرور ونص التلميح حسب الحالة الحالية.
    /// </summary>
    private void UpdatePasswordVisibility()
    {
        _textBox.UseSystemPasswordChar = !_isPasswordVisible;
        _toggleButton.Text = _isPasswordVisible ? "◉" : "○";
        _toolTip.SetToolTip(
            _toggleButton,
            _isPasswordVisible ? "إخفاء كلمة المرور" : "إظهار كلمة المرور");
    }
}
