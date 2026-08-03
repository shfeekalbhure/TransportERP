using System.Drawing.Drawing2D;

namespace TransportERP.Desktop.Forms;

public sealed class FrmLogin : Form
{
    private readonly TextBox _txtUserName = new();
    private readonly TextBox _txtPassword = new();
    private readonly ComboBox _cmbCompany = new();
    private readonly ComboBox _cmbBranch = new();
    private readonly ComboBox _cmbFiscalYear = new();
    private readonly CheckBox _chkRememberMe = new();
    private readonly Button _btnLogin = new();

    public FrmLogin()
    {
        InitializeForm();
        BuildLayout();
    }

    private void InitializeForm()
    {
        Text = "TransportERP - تسجيل الدخول";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1180, 720);
        ClientSize = new Size(1360, 820);
        BackColor = Color.FromArgb(239, 245, 252);
        Font = new Font("Segoe UI", 10F);
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
    }

    private void BuildLayout()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2,
            Padding = new Padding(28),
            BackColor = BackColor
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        Controls.Add(root);

        root.Controls.Add(BuildLoginPanel(), 0, 0);
        root.Controls.Add(BuildBrandPanel(), 1, 0);

        var status = new Label
        {
            Dock = DockStyle.Fill,
            Text = "الإصدار 1.0.0     •     متصل     •     الخادم: TransportERP API",
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.FromArgb(91, 111, 139),
            Font = new Font("Segoe UI", 9F)
        };
        root.SetColumnSpan(status, 2);
        root.Controls.Add(status, 0, 1);
    }

    private Control BuildLoginPanel()
    {
        var host = new Panel
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 0, 14, 0),
            Padding = new Padding(54, 42, 54, 42),
            BackColor = Color.White
        };
        host.Resize += (_, _) => ApplyRoundedRegion(host, 24);

        var title = new Label
        {
            Dock = DockStyle.Top,
            Height = 58,
            Text = "تسجيل الدخول",
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.FromArgb(17, 43, 78),
            Font = new Font("Segoe UI", 24F, FontStyle.Bold)
        };
        host.Controls.Add(title);

        var subtitle = new Label
        {
            Dock = DockStyle.Top,
            Height = 42,
            Text = "أدخل بياناتك للوصول إلى النظام",
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.FromArgb(104, 122, 147),
            Font = new Font("Segoe UI", 11F)
        };
        host.Controls.Add(subtitle);
        subtitle.BringToFront();

        var fields = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 13,
            Padding = new Padding(0, 20, 0, 0)
        };
        for (var i = 0; i < 13; i++)
            fields.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        AddField(fields, "اسم المستخدم", _txtUserName, 0);
        AddField(fields, "كلمة المرور", _txtPassword, 2);
        _txtPassword.UseSystemPasswordChar = true;

        _cmbCompany.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbBranch.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbFiscalYear.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbCompany.Items.AddRange(["شركة النقل الرئيسية"]);
        _cmbBranch.Items.AddRange(["الفرع الرئيسي"]);
        _cmbFiscalYear.Items.AddRange([DateTime.Today.Year.ToString()]);
        _cmbCompany.SelectedIndex = 0;
        _cmbBranch.SelectedIndex = 0;
        _cmbFiscalYear.SelectedIndex = 0;

        AddField(fields, "الشركة", _cmbCompany, 4);
        AddField(fields, "الفرع", _cmbBranch, 6);
        AddField(fields, "السنة المالية", _cmbFiscalYear, 8);

        var options = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 38,
            ColumnCount = 2,
            Margin = new Padding(0, 8, 0, 8)
        };
        options.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        options.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

        _chkRememberMe.Text = "تذكرني";
        _chkRememberMe.Dock = DockStyle.Fill;
        _chkRememberMe.TextAlign = ContentAlignment.MiddleRight;

        var forgotPassword = new LinkLabel
        {
            Text = "نسيت كلمة المرور؟",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            LinkColor = Color.FromArgb(38, 112, 235),
            ActiveLinkColor = Color.FromArgb(26, 82, 180)
        };
        options.Controls.Add(_chkRememberMe, 0, 0);
        options.Controls.Add(forgotPassword, 1, 0);
        fields.Controls.Add(options, 0, 10);

        _btnLogin.Text = "دخول";
        _btnLogin.Dock = DockStyle.Top;
        _btnLogin.Height = 52;
        _btnLogin.Margin = new Padding(0, 12, 0, 0);
        _btnLogin.FlatStyle = FlatStyle.Flat;
        _btnLogin.FlatAppearance.BorderSize = 0;
        _btnLogin.BackColor = Color.FromArgb(35, 111, 229);
        _btnLogin.ForeColor = Color.White;
        _btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        _btnLogin.Cursor = Cursors.Hand;
        _btnLogin.Resize += (_, _) => ApplyRoundedRegion(_btnLogin, 12);
        _btnLogin.Click += HandleLogin;
        fields.Controls.Add(_btnLogin, 0, 11);

        var language = new Label
        {
            Dock = DockStyle.Top,
            Height = 34,
            Margin = new Padding(0, 18, 0, 0),
            Text = "العربية  |  English",
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.FromArgb(91, 111, 139)
        };
        fields.Controls.Add(language, 0, 12);

        host.Controls.Add(fields);
        fields.BringToFront();
        return host;
    }

    private static void AddField(TableLayoutPanel container, string labelText, Control control, int row)
    {
        var label = new Label
        {
            Dock = DockStyle.Top,
            Height = 28,
            Text = labelText,
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.FromArgb(50, 67, 91),
            Font = new Font("Segoe UI", 10F, FontStyle.Bold)
        };

        control.Dock = DockStyle.Top;
        control.Height = 42;
        control.Margin = new Padding(0, 0, 0, 12);
        control.Font = new Font("Segoe UI", 11F);
        control.RightToLeft = RightToLeft.Yes;

        container.Controls.Add(label, 0, row);
        container.Controls.Add(control, 0, row + 1);
    }

    private Control BuildBrandPanel()
    {
        var brand = new GradientPanel
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(14, 0, 0, 0),
            Padding = new Padding(54),
            StartColor = Color.FromArgb(18, 58, 137),
            EndColor = Color.FromArgb(35, 126, 230)
        };
        brand.Resize += (_, _) => ApplyRoundedRegion(brand, 24);

        var content = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5,
            BackColor = Color.Transparent
        };
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 18F));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 18F));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 22F));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 12F));

        content.Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            Text = "TransportERP",
            TextAlign = ContentAlignment.BottomRight,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 30F, FontStyle.Bold)
        }, 0, 0);

        content.Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            Text = "نظام النقل والخدمات اللوجستية المتكامل",
            TextAlign = ContentAlignment.TopRight,
            ForeColor = Color.FromArgb(220, 235, 255),
            Font = new Font("Segoe UI", 15F)
        }, 0, 1);

        content.Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            Text = "إدارة الشركات والفروع والرحلات والحسابات\nمن منصة موحدة وآمنة.",
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 16F, FontStyle.Bold)
        }, 0, 2);

        content.Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            Text = "✓ واجهة عربية حديثة\n\n✓ متعدد الشركات والفروع\n\n✓ يعمل عبر API مركزي\n\n✓ قابل للتوسع لتطبيقات الجوال",
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.FromArgb(232, 242, 255),
            Font = new Font("Segoe UI", 12F)
        }, 0, 3);

        content.Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            Text = "TransportERP © 2026",
            TextAlign = ContentAlignment.BottomRight,
            ForeColor = Color.FromArgb(200, 222, 250),
            Font = new Font("Segoe UI", 9F)
        }, 0, 4);

        brand.Controls.Add(content);
        return brand;
    }

    private void HandleLogin(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtUserName.Text))
        {
            MessageBox.Show("يرجى إدخال اسم المستخدم.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _txtUserName.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(_txtPassword.Text))
        {
            MessageBox.Show("يرجى إدخال كلمة المرور.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _txtPassword.Focus();
            return;
        }

        MessageBox.Show("واجهة تسجيل الدخول جاهزة للربط مع الـ API.", "TransportERP", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private static void ApplyRoundedRegion(Control control, int radius)
    {
        if (control.Width <= 0 || control.Height <= 0)
            return;

        using var path = new GraphicsPath();
        var diameter = radius * 2;
        var rectangle = new Rectangle(0, 0, control.Width, control.Height);
        path.AddArc(rectangle.X, rectangle.Y, diameter, diameter, 180, 90);
        path.AddArc(rectangle.Right - diameter, rectangle.Y, diameter, diameter, 270, 90);
        path.AddArc(rectangle.Right - diameter, rectangle.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(rectangle.X, rectangle.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();
        control.Region?.Dispose();
        control.Region = new Region(path);
    }

    private sealed class GradientPanel : Panel
    {
        public Color StartColor { get; init; } = Color.Navy;
        public Color EndColor { get; init; } = Color.RoyalBlue;

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using var brush = new LinearGradientBrush(ClientRectangle, StartColor, EndColor, 35F);
            e.Graphics.FillRectangle(brush, ClientRectangle);
        }
    }
}
