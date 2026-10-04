namespace TransportERP.Desktop;

using CoreUI;

public sealed class FrmLogin : FrmBase
{
    private readonly ComboBox cmbCompany = CreateComboBox();
    private readonly ComboBox cmbBranch = CreateComboBox();
    // Tenant belongs to the session contract; it has no visible editor.
    private string? SessionTenantId { get; set; }
    private readonly ComboBox cmbCompanyGroup = CreateComboBox();
    private readonly ComboBox cmbBranchGroup = CreateComboBox();
    private readonly ComboBox cmbConnection = CreateComboBox();
    private readonly ComboBox cmbSyncState = CreateComboBox();
    private readonly DateTimePicker dtLastSync = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "'لم تتم المزامنة'", Enabled = false };
    private readonly CheckBox chkOfflineEligible = new() { AutoSize = true, Enabled = false, Checked = false };
    private readonly TabControl loginTabs = new() { Dock = DockStyle.Fill, RightToLeftLayout = true };
    private readonly Button btnOffline = new();
    private readonly Button btnConnection = new();
    private readonly Button btnRetry = new();
    private readonly ComboBox cmbLanguage = CreateComboBox();
    private readonly TextBox txtUsername = CreateTextBox("اسم المستخدم", autoDirection: true);
    private readonly TextBox txtPassword = CreateTextBox("كلمة المرور", password: true, autoDirection: true);
    private readonly CheckBox chkRememberScope = new();
    private readonly CheckBox chkShowPassword = new();
    private readonly Button btnLogin = new();
    private readonly Button btnExit = new();
    private readonly Label lblStatus = new();
    private Panel panel1;
    private SplitContainer splitContainer1;
    private readonly FormLayoutMetrics metrics = FormLayoutMetrics.For(FormDensity.Standard);

    public FrmLogin()
    {
        InitializeScreen();
    }

    private void InitializeScreen()
    {
        Text = "TransportERP | تسجيل الدخول";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = SizeFromClientSize(new Size(1120, 750));
        ClientSize = new Size(1120, 750);
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        Font = new Font("Segoe UI", 10F);
        BackColor = UiDesignTokens.Surface;

        SeedSelections();

        var shell = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            Padding = new Padding(24),
            BackColor = UiDesignTokens.Surface,
            RightToLeft = RightToLeft.Yes
        };
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        shell.RowCount = 1;
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        shell.Controls.Add(BuildFormPanel(), 0, 0);
        // The login form uses the full available width for its two source-defined tabs.
        Controls.Add(shell);

        AcceptButton = btnLogin;
        CancelButton = btnExit;
        AttachAutoDirection(txtUsername);
        AttachAutoDirection(txtPassword);
        chkShowPassword.CheckedChanged += (_, _) => txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        btnExit.Click += (_, _) => Close();
        btnLogin.Click += (_, _) => OpenMainScreen();
        txtPassword.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                OpenMainScreen();
                e.SuppressKeyPress = true;
            }
        };
    }

    private Control BuildFormPanel()
    {
        var card = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = UiDesignTokens.PanelSurface,
            Padding = new Padding(24),
            Margin = Padding.Empty
        };

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 7,
            BackColor = UiDesignTokens.PanelSurface,
            RightToLeft = RightToLeft.Yes
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));

        layout.Controls.Add(new Label
        {
            Text = "تسجيل الدخول",
            Dock = DockStyle.Fill,
            Font = UiDesignTokens.TitleFont(),
            ForeColor = UiDesignTokens.TextPrimary,
            TextAlign = ContentAlignment.BottomRight
        }, 0, 0);

        layout.Controls.Add(new Label
        {
            Text = "اختر نطاق العمل ثم تابع إلى لوحة التحكم.",
            Dock = DockStyle.Fill,
            Font = UiDesignTokens.SecondaryFont(),
            ForeColor = UiDesignTokens.TextSecondary,
            TextAlign = ContentAlignment.MiddleRight
        }, 0, 1);

        layout.Controls.Add(BuildFieldsContainer(), 0, 2);
        layout.Controls.Add(BuildOptions(), 0, 3);
        layout.Controls.Add(BuildActions(), 0, 4);

        lblStatus.Text = "بيئة عرض تجريبية - الدخول يفتح لوحة التحكم بدون مصادقة فعلية.";
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.ForeColor = UiDesignTokens.TextSecondary;
        lblStatus.Font = UiDesignTokens.FinePrintFont();
        lblStatus.TextAlign = ContentAlignment.MiddleCenter;
        layout.Controls.Add(lblStatus, 0, 5);

        card.Controls.Add(layout);
        return card;
    }

    private Control BuildFieldsContainer()
    {
        loginTabs.Name = "loginTabs";
        var identityPage = new TabPage("الدخول") { Name = "tabLogin", Padding = new Padding(16), AutoScroll = true };
        var connectionPage = new TabPage("الاتصال") { Name = "tabConnection", Padding = new Padding(16), AutoScroll = true };
        loginTabs.TabPages.AddRange([identityPage, connectionPage]);
        // Field-to-tab mapping approved by the user after source reconciliation.
        var identity = SourceFields("identityFields", [
            ("اسم المستخدم", txtUsername), ("كلمة المرور", txtPassword), ("اللغة", cmbLanguage)]);
        var connection = SourceFields("connectionFields", [
            ("مجموعة الشركات", cmbCompanyGroup), ("الشركة", cmbCompany),
            ("مجموعة الفروع", cmbBranchGroup), ("الفرع", cmbBranch),
            ("إعداد الاتصال", cmbConnection), ("حالة المزامنة", cmbSyncState),
            ("آخر مزامنة", dtLastSync), ("السماح بالفتح دون اتصال", chkOfflineEligible)]);
        identityPage.Controls.Add(identity);
        connectionPage.Controls.Add(connection);
        cmbCompanyGroup.Enabled = cmbBranchGroup.Enabled = cmbConnection.Enabled = cmbSyncState.Enabled = false;
        btnConnection.Click += (_, _) => loginTabs.SelectedTab = connectionPage;
        return loginTabs;
    }

    private TableLayoutPanel SourceFields(string name, (string Text, Control Input)[] fields)
    {
        var table = new TableLayoutPanel { Name = name, ColumnCount = 2, RowCount = fields.Length, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Dock = DockStyle.Top, RightToLeft = RightToLeft.Yes };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        for (int i = 0; i < fields.Length; i++)
        {
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            var field = fields[i];
            var label = new Label { Name = name + "Label" + i, Text = field.Text, AutoSize = true, MinimumSize = new Size(0, 32), Font = UiDesignTokens.LabelFont(), Anchor = AnchorStyles.Right, TextAlign = ContentAlignment.MiddleRight, Margin = new Padding(0, 4, 16, 4) };
            var measuredLabel = TextRenderer.MeasureText(field.Text, label.Font, Size.Empty, TextFormatFlags.SingleLine);
            label.MinimumSize = new Size(measuredLabel.Width, Math.Max(32, measuredLabel.Height));
            field.Input.Name = name + "Input" + i;
            field.Input.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            field.Input.Margin = new Padding(4);
            table.Controls.Add(label, 0, i);
            table.Controls.Add(field.Input, 1, i);
        }
        return table;
    }

    private Control BuildOptions()
    {
        chkRememberScope.Text = "تذكر نطاق العمل";
        chkRememberScope.AutoSize = true;
        chkRememberScope.ForeColor = UiDesignTokens.TextPrimary;
        chkRememberScope.Font = UiDesignTokens.OptionFont();

        chkShowPassword.Text = "إظهار كلمة المرور";
        chkShowPassword.AutoSize = true;
        chkShowPassword.ForeColor = UiDesignTokens.TextPrimary;
        chkShowPassword.Font = UiDesignTokens.OptionFont();

        var options = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            BackColor = UiDesignTokens.OptionsSurface,
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(10, 8, 10, 0),
            Margin = new Padding(0, 2, 0, 6)
        };
        options.Controls.Add(chkRememberScope);
        options.Controls.Add(chkShowPassword);
        return options;
    }

    private Control BuildActions()
    {
        btnLogin.Text = "دخول إلى النظام";
        btnLogin.Dock = DockStyle.Fill;
        btnLogin.BackColor = UiDesignTokens.Primary;
        btnLogin.ForeColor = Color.White;
        btnLogin.FlatStyle = FlatStyle.Flat;
        btnLogin.FlatAppearance.BorderSize = 0;
        btnLogin.Font = UiDesignTokens.SectionTitleFont();
        btnLogin.Cursor = Cursors.Hand;

        btnExit.Text = "إغلاق";
        btnExit.Dock = DockStyle.Fill;
        btnExit.BackColor = Color.White;
        btnExit.ForeColor = UiDesignTokens.Primary;
        btnExit.FlatStyle = FlatStyle.Flat;
        btnExit.FlatAppearance.BorderColor = UiDesignTokens.Border;
        btnExit.FlatAppearance.BorderSize = 1;
        btnExit.Font = UiDesignTokens.OptionFont();
        btnExit.Cursor = Cursors.Hand;

        btnOffline.Text = "العمل دون اتصال";
        btnConnection.Text = "إعداد الاتصال";
        btnRetry.Text = "إعادة محاولة الاتصال";
        // No cached-session or connectivity services exist in the current demo.
        btnOffline.Enabled = btnRetry.Enabled = false;
        var actions = new FlowLayoutPanel { Name = "loginActions", Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, WrapContents = true };
        foreach (var button in new[] { btnLogin, btnOffline, btnConnection, btnRetry, btnExit })
        {
            button.Dock = DockStyle.None;
            button.AutoSize = true;
            button.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button.MinimumSize = new Size(0, 40);
            button.Padding = new Padding(16, 4, 16, 4);
            button.Margin = new Padding(4);
            actions.Controls.Add(button);
        }
        return actions;
    }

    private void SeedSelections()
    {
        SetItems(cmbCompany, "شركة النقل الرئيسية", "شركة النقل الرئيسية", "فرع تجريبي آخر");
        SetItems(cmbBranch, "الفرع الرئيسي - عدن", "الفرع الرئيسي - عدن", "فرع صنعاء", "فرع المكلا");
        SetItems(cmbLanguage, "العربية", "العربية", "English");
    }

    private static ComboBox CreateComboBox()
    {
        var comboBox = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            RightToLeft = RightToLeft.Yes,
            DrawMode = DrawMode.OwnerDrawFixed,
            ItemHeight = 24,
            FlatStyle = FlatStyle.Flat,
            Font = UiDesignTokens.InputFont(),
            BackColor = Color.White,
            ForeColor = UiDesignTokens.TextPrimary,
            Margin = new Padding(0, 0, 0, 0)
        };
        comboBox.DrawItem += (_, e) =>
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                TextRenderer.DrawText(
                    e.Graphics,
                    comboBox.Items[e.Index]?.ToString() ?? string.Empty,
                    comboBox.Font,
                    e.Bounds,
                    comboBox.ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.RightToLeft);
            }
            e.DrawFocusRectangle();
        };
        return comboBox;
    }

    private static TextBox CreateTextBox(string placeholder, bool password = false, bool autoDirection = false)
    {
        return new TextBox
        {
            PlaceholderText = placeholder,
            RightToLeft = autoDirection ? RightToLeft.No : RightToLeft.Yes,
            TextAlign = autoDirection ? HorizontalAlignment.Left : HorizontalAlignment.Right,
            UseSystemPasswordChar = password,
            BorderStyle = BorderStyle.FixedSingle,
            Font = UiDesignTokens.InputFont(),
            ForeColor = UiDesignTokens.TextPrimary,
            BackColor = Color.White
        };
    }

    private static void AttachAutoDirection(TextBox textBox)
    {
        textBox.TextChanged += (_, _) => ApplyAutoDirection(textBox);
    }

    private static void ApplyAutoDirection(TextBox textBox)
    {
        var text = textBox.Text.TrimStart();
        if (text.Length == 0)
        {
            textBox.RightToLeft = RightToLeft.No;
            textBox.TextAlign = HorizontalAlignment.Left;
            return;
        }

        var isArabic = text.Any(IsArabicLetter);
        textBox.RightToLeft = isArabic ? RightToLeft.Yes : RightToLeft.No;
        textBox.TextAlign = isArabic ? HorizontalAlignment.Right : HorizontalAlignment.Left;
    }

    private static bool IsArabicLetter(char value)
    {
        return value is >= '\u0600' and <= '\u06FF'
            or >= '\u0750' and <= '\u077F'
            or >= '\u08A0' and <= '\u08FF'
            or >= '\uFB50' and <= '\uFDFF'
            or >= '\uFE70' and <= '\uFEFF';
    }

    private static void SetItems(ComboBox comboBox, string selected, params string[] items)
    {
        comboBox.Items.Clear();
        comboBox.Items.AddRange(items);
        comboBox.SelectedItem = selected;
    }

    private void InitializeComponent()
    {
        panel1 = new Panel();
        splitContainer1 = new SplitContainer();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.SuspendLayout();
        SuspendLayout();
        // 
        // panel1
        // 
        panel1.Dock = DockStyle.Bottom;
        panel1.Location = new Point(0, 573);
        panel1.Name = "panel1";
        panel1.Size = new Size(962, 60);
        panel1.TabIndex = 0;
        // 
        // splitContainer1
        // 
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.IsSplitterFixed = true;
        splitContainer1.Location = new Point(0, 0);
        splitContainer1.Name = "splitContainer1";
        // 
        // splitContainer1.Panel1
        // 
        splitContainer1.Panel1.RightToLeft = RightToLeft.Yes;
        splitContainer1.Panel1.Paint += splitContainer1_Panel1_Paint;
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.RightToLeft = RightToLeft.Yes;
        splitContainer1.Size = new Size(962, 573);
        splitContainer1.SplitterDistance = 516;
        splitContainer1.TabIndex = 1;
        // 
        // FrmLogin
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        ClientSize = new Size(962, 633);
        Controls.Add(splitContainer1);
        Controls.Add(panel1);
        ForeColor = Color.BlanchedAlmond;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "FrmLogin";
        Text = "تسجيل نظام الدخول";
        Load += FrmLogin_Load;
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        ResumeLayout(false);

    }

    private void OpenMainScreen()
    {
        using var mainScreen = new FrmMain();
        Hide();
        mainScreen.ShowDialog(this);
        Show();
    }

    private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
    {

    }

    private void FrmLogin_Load(object sender, EventArgs e)
    {

    }
}





