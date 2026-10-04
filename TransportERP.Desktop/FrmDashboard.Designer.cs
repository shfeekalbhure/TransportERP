namespace TransportERP.Desktop;

using CoreUI;
using TransportERP.Desktop.البوالص_والشحن.الاعدادات;

partial class FrmDashboard
{
    private const int SidebarWidth = 280;
    private const int AlertsWidth = 260;
    private const int CollapsedWidth = 76;
    private const int HeaderHeight = 76;
    private const int FooterHeight = 34;

    private readonly TableLayoutPanel shell = new();
    private readonly Panel rightSidebar = new();
    private readonly Panel leftAlerts = new();
    private readonly Panel contentHost = new();

    private void InitializeComponent()
    {
        SuspendLayout();
        // 
        // FrmDashboard
        // 
        BackColor = Color.FromArgb(248, 250, 252);
        ClientSize = new Size(1600, 1000);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(1280, 760);
        Name = "FrmDashboard";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "TransportERP | الشاشة الرئيسية";
        shell.Dock = DockStyle.Fill;
        shell.ColumnCount = 3;
        shell.RowCount = 1;
        shell.RightToLeft = RightToLeft.Yes;
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, SidebarWidth));
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, AlertsWidth));
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        shell.Controls.Add(BuildRightSidebar(), 0, 0);
        shell.Controls.Add(BuildWorkspace(), 1, 0);
        shell.Controls.Add(BuildLeftAlerts(), 2, 0);
        Controls.Add(shell);
        Load += FrmDashboard_Load;
        ResumeLayout(false);
    }

    private Control BuildRightSidebar()
    {
        rightSidebar.Dock = DockStyle.Fill;
        rightSidebar.BackColor = UiDesignTokens.PrimaryStrong;
        rightSidebar.Padding = new Padding(16, 16, 16, 22);
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 4, ColumnCount = 1, BackColor = UiDesignTokens.PrimaryStrong, RightToLeft = RightToLeft.Yes };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        var title = new Label { Text = "شجرة النظام V43", Dock = DockStyle.Fill, ForeColor = Color.White, Font = new Font("Segoe UI", 18F, FontStyle.Bold), TextAlign = ContentAlignment.MiddleRight, AutoEllipsis = true };
        var search = new TextBox { Dock = DockStyle.Fill, Font = UiDesignTokens.InputFont(), ForeColor = UiDesignTokens.TextPrimary, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, RightToLeft = RightToLeft.Yes, TextAlign = HorizontalAlignment.Right, PlaceholderText = "ابحث في الشجرة...", Margin = new Padding(0, 4, 0, 4) };
        var treePanel = BuildSystemTreePanel();
        var collapse = SidebarButton("طي");
        collapse.Click += (_, _) => ToggleRightSidebar(title, search, treePanel, collapse);
        layout.Controls.Add(title, 0, 0);
        layout.Controls.Add(search, 0, 1);
        layout.Controls.Add(treePanel, 0, 2);
        layout.Controls.Add(collapse, 0, 3);
        rightSidebar.Controls.Add(layout);
        return rightSidebar;
    }

    private Control BuildSystemTreePanel()
    {
        var scroller = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, AutoScroll = true, RightToLeft = RightToLeft.Yes };
        var list = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, RowCount = 0, ColumnCount = 1, Padding = new Padding(8), BackColor = Color.White, RightToLeft = RightToLeft.Yes };
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        list.SizeChanged += (_, _) => ResizeTreeLabels(list);
        AddTreeSection(list, "التهيئة والإعدادات", new[] { "إعداد فترات النظام", "الأقاليم الدولية", "الترميزات العامة", "الترميزات العامة للموظفين", "المتغيرات العامة", "أنواع الضرائب", "أنواع الهيكل الإداري", "بيانات الدول", "بيانات الشركات", "بيانات الفروع", "بيانات المحافظات", "بيانات المدن", "بيانات المناطق", "ترجمة النصوص", "ترميز الشرائح الضريبية", "ترميز بيانات إضافية للمشاريع", "تسلسلات الفاتورة الإلكترونية", "تفعيل الأدلة الفرعية", "تهيئة الأنشطة", "تهيئة الحقول الإضافية", "تهيئة الدليل المحاسبي", "تهيئة العملات", "تهيئة المشاريع", "تهيئة مراكز التكلفة", "خلفيات الشاشات", "طرق احتساب الضرائب", "مجموعات الفروع", "مستويات الاعتماد" });
        AddTreeSection(list, "المدخلات العامة", new[] { "الأدلة المحاسبية الافتراضية", "الحسابات المدينة والدائنة الأخرى", "الحسابات الوسيطة", "الدليل المحاسبي", "الهيكل الإداري", "بيانات الأنشطة", "بيانات المشاريع", "بيانات الموظفين", "ربط التدفقات النقدية مع تحليل الحسابات", "ربط الحسابات المدينة والدائنة الأخرى", "ربط الموظفين بالمهن الوظيفية", "مجموعات الحسابات المدينة والدائنة الأخرى", "مراكز التكلفة" });
        AddTreeSection(list, "إدارة النظام", new[] { "استرجاع النسخ الاحتياطي", "الإقفالات والتوقيفات", "  الإقفال السنوي", "  الإقفال الشهري الفتري", "  الإقفال الشهري الفتري للشئون الإدارية", "  التوقيف الشهري الفتري", "  إلغاء إقفال فترات الشئون الإدارية", "  إلغاء الإقفالات", "الترويسة حسب المستخدم", "الحقول الإجبارية", "الرقابة", "الشاشات المرتبطة للمستخدم", "المساحات التخزينية للبيانات", "النسخ الاحتياطي", "بيانات المستخدمين", "تحديث قاعدة البيانات", "تحديث قاعدة البيانات كل السنوات", "تغيير كلمة السر", "تنبيهات النظام", "صلاحيات التبويبات", "صلاحيات الشاشات", "صلاحيات العمليات", "صلاحيات المدخلات", "عرض المستخدمين", "قوالب التقارير", "متغيرات قاعدة البيانات", "مجموعة المستخدمين", "نقل البيانات بين الوحدات المحاسبية", "نماذج الطباعة" });
        AddTreeSection(list, "الأستاذ العام", new[] { "استحقاق شيكات سندات الصرف يدويا", "استحقاق شيكات سندات القبض يدويا", "إشعارات دائنة", "إشعارات مدينة", "الأرصدة الافتتاحية", "البنوك", "الشيكات المستحقة للسداد آليا", "الصناديق", "أنواع الإشعارات", "أنواع الطلبات", "أنواع القبض والصرف", "أنواع قيود اليومية", "تحقيق الإيداع النقدي لدى البنوك", "ترميز البيان", "تسوية الإيرادات المقدمة", "تسوية البنوك", "تسوية المصروفات المقدمة", "تهيئة الحدود", "جرد النقدية", "دفاتر الشيكات", "ربط الحسابات بالأنواع الضريبية", "ربط الحسابات بالمراكز", "ربط الحسابات بالمشاريع", "سند الصرف", "سند القبض", "صرف عملة", "طلب صرف عملة", "طلب فتح حساب", "طلبات سندات الصرف", "طلبات سندات القبض", "طلبات قيود اليومية", "قيود اليومية", "متغيرات الأستاذ العام", "مجموعات البنوك", "مجموعات الصناديق", "مصمم التقارير الختامية والتدفقات والقوائم", "مطابقة البنوك" });
        AddTreeSection(list, "البوالص والشحن", new[] { "أنواع الشحن", "فئات وأصناف الشحن" });
        scroller.Controls.Add(list);
        return scroller;
    }

    private static void ResizeTreeLabels(TableLayoutPanel list)
    {
        foreach (Control control in list.Controls) control.Width = Math.Max(210, list.ClientSize.Width - 16);
    }

    private void AddTreeSection(TableLayoutPanel list, string title, string[] items)
    {
        AddTreeLabel(list, TreeLabel(title, true));
        foreach (var item in items) AddTreeLabel(list, TreeLabel(item, false));
    }

    private static void AddTreeLabel(TableLayoutPanel list, Control label)
    {
        var row = list.RowCount++;
        list.RowStyles.Add(new RowStyle(SizeType.Absolute, label.Height + label.Margin.Vertical));
        list.Controls.Add(label, 0, row);
    }

    private Label TreeLabel(string text, bool section)
    {
        var child = text.StartsWith("  ", StringComparison.Ordinal);

        var label = new Label
        {
            Text = child ? $"   {text.Trim()}" : text,
            Dock = DockStyle.Fill,
            Height = section ? 42 : 34,
            BackColor = section ? Color.FromArgb(221, 236, 246) : Color.White,
            ForeColor = section ? UiDesignTokens.PrimaryStrong : UiDesignTokens.TextPrimary,
            Font = section ? UiDesignTokens.SectionTitleFont() : UiDesignTokens.OptionFont(),
            RightToLeft = RightToLeft.Yes,
            TextAlign = ContentAlignment.MiddleRight,
            AutoEllipsis = true,
            Margin = section ? new Padding(0, 8, 0, 4) : new Padding(0, 0, 0, 2),
            Padding = child ? new Padding(0, 0, 26, 0) : new Padding(0, 0, 12, 0)
        };

        if (!section)
        {
            switch (text.Trim())
            {
                case "أنواع الشحن":
                    label.Tag = "ShipmentTypes";
                    label.Cursor = Cursors.Hand;
                    label.Click += TreeItem_Click;
                    break;

                case "فئات وأصناف الشحن":
                    label.Tag = "ShipmentCategories";
                    label.Cursor = Cursors.Hand;
                    label.Click += TreeItem_Click;
                    break;
            }
        }

        return label;
    }

    private void TreeItem_Click(object? sender, EventArgs e)
    {
        if (sender is not Label label)
            return;

        switch (label.Tag?.ToString())
        {
            case "ShipmentTypes":
                OpenUserControl(new UcShipmentTypes());
                break;

            case "ShipmentCategories":
                OpenUserControl(new UcShipmentCategories());
                break;
        }
    }

    private void OpenUserControl(UserControl control)
    {
        contentHost.SuspendLayout();

        try
        {
            while (contentHost.Controls.Count > 0)
            {
                var oldControl = contentHost.Controls[0];
                contentHost.Controls.RemoveAt(0);
                oldControl.Dispose();
            }

            control.Dock = DockStyle.Fill;
            contentHost.Controls.Add(control);
            control.BringToFront();
        }
        finally
        {
            contentHost.ResumeLayout(true);
        }
    }

    private Control BuildWorkspace()
    {
        var main = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3, ColumnCount = 1, BackColor = UiDesignTokens.Surface, RightToLeft = RightToLeft.Yes };
        main.RowStyles.Add(new RowStyle(SizeType.Absolute, HeaderHeight));
        main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        main.RowStyles.Add(new RowStyle(SizeType.Absolute, FooterHeight));
        main.Controls.Add(BuildHeader(), 0, 0);
        main.Controls.Add(BuildContent(), 0, 1);
        main.Controls.Add(BuildFooter(), 0, 2);
        return main;
    }

    private static Control BuildHeader()
    {
        var header = new Panel { Dock = DockStyle.Fill, BackColor = UiDesignTokens.Primary, Padding = new Padding(24, 12, 24, 12) };
        header.Controls.Add(new Label { Dock = DockStyle.Fill, Text = "الشركة: شركة النقل الرئيسية    الفرع: عدن الرئيسي    السنة المالية: 2026    المستخدم: مدير النظام", ForeColor = Color.White, Font = UiDesignTokens.OptionFont(), TextAlign = ContentAlignment.MiddleRight, AutoEllipsis = true });
        return header;
    }

    private Control BuildContent()
    {
        contentHost.Dock = DockStyle.Fill;
        contentHost.BackColor = UiDesignTokens.Surface;
        contentHost.Padding = new Padding(24);

        contentHost.Controls.Add(new Label
        {
            Dock = DockStyle.Top,
            Height = 44,
            Text = "الشاشة الرئيسية",
            ForeColor = UiDesignTokens.TextPrimary,
            Font = UiDesignTokens.TitleFont(),
            TextAlign = ContentAlignment.MiddleRight
        });

        return contentHost;
    }

    private Control BuildLeftAlerts()
    {
        leftAlerts.Dock = DockStyle.Fill;
        leftAlerts.BackColor = UiDesignTokens.PanelSurface;
        leftAlerts.Padding = new Padding(14, 16, 14, 22);
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 7, ColumnCount = 1, BackColor = UiDesignTokens.PanelSurface, RightToLeft = RightToLeft.Yes };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
        for (var i = 0; i < 4; i++) layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        var title = new Label { Dock = DockStyle.Fill, Text = "مركز التنبيهات", ForeColor = UiDesignTokens.TextPrimary, Font = UiDesignTokens.SectionTitleFont(), TextAlign = ContentAlignment.MiddleRight, AutoEllipsis = true };
        var alertRows = new Control[] { AlertRow("تأخر شحنة", "موعد الوصول تجاوز الخطة"), AlertRow("انخفاض وقود", "مركبة تحتاج متابعة"), AlertRow("اعتماد جديد", "طلب ينتظر المراجعة"), AlertRow("فاتورة ضريبية", "لم ترسل بعد") };
        var collapse = SidebarButton("طي");
        collapse.Click += (_, _) => ToggleAlerts(title, alertRows, collapse);
        layout.Controls.Add(title, 0, 0);
        for (var i = 0; i < alertRows.Length; i++) layout.Controls.Add(alertRows[i], 0, i + 1);
        layout.Controls.Add(collapse, 0, 6);
        leftAlerts.Controls.Add(layout);
        return leftAlerts;
    }

    private static Control AlertRow(string title, string detail)
    {
        var panel = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1, BackColor = UiDesignTokens.FieldGroupSurface, Padding = new Padding(10, 6, 10, 6), Margin = new Padding(0, 4, 0, 4), RightToLeft = RightToLeft.Yes };
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        panel.Controls.Add(new Label { Dock = DockStyle.Fill, Text = title, ForeColor = UiDesignTokens.TextPrimary, Font = UiDesignTokens.OptionFont(), TextAlign = ContentAlignment.MiddleRight, AutoEllipsis = true }, 0, 0);
        panel.Controls.Add(new Label { Dock = DockStyle.Fill, Text = detail, ForeColor = UiDesignTokens.TextSecondary, Font = UiDesignTokens.FinePrintFont(), TextAlign = ContentAlignment.TopRight, AutoEllipsis = true }, 0, 1);
        return panel;
    }

    private static Button SidebarButton(string text)
    {
        return new Button { Text = text, Dock = DockStyle.Fill, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(31, 89, 135), ForeColor = Color.White, Font = UiDesignTokens.OptionFont(), TextAlign = ContentAlignment.MiddleCenter, Cursor = Cursors.Hand, UseVisualStyleBackColor = false };
    }

    private static Control BuildFooter()
    {
        return new Label { Text = "TransportERP - جميع الحقوق محفوظة", Dock = DockStyle.Fill, BackColor = UiDesignTokens.PanelSurface, ForeColor = UiDesignTokens.TextSecondary, Font = UiDesignTokens.FinePrintFont(), TextAlign = ContentAlignment.MiddleCenter };
    }
}

