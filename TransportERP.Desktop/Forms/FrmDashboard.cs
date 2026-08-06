using System.Drawing.Drawing2D;
using TransportERP.Desktop.Forms.Accounting;
using TransportERP.Desktop.Forms.Security;
using TransportERP.Desktop.Forms.Setup.General;

namespace TransportERP.Desktop;

/// <summary>
/// الشاشة الرئيسية لنظام TransportERP.
/// تعرض مؤشرات الأداء وتستضيف شاشات النظام داخل تبويبات متعددة.
/// </summary>
public partial class FrmDashboard : Form
{
    private const string DashboardTabKey = "DASHBOARD";
    private const string CountriesTabKey = "GEN-003";

    private TabControl? _workspaceTabs;
    private ContextMenuStrip? _generalSetupMenu;
    private ContextMenuStrip? _securityAdministrationMenu;
    private ContextMenuStrip? _accountingMenu;

    public FrmDashboard()
    {
        InitializeComponent();
        ConfigureTabbedWorkspace();
        ConfigureGeneralSetupMenu();
        ConfigureSecurityAndAccountingMenus();
        LoadDevelopmentPreviewData();
    }

    /// <summary>
    /// تحويل مساحة العمل الحالية إلى نظام تبويبات، مع إبقاء الرئيسية تبويبًا ثابتًا.
    /// </summary>
    private void ConfigureTabbedWorkspace()
    {
        if (_workspaceTabs is not null)
        {
            return;
        }

        tblRoot.SuspendLayout();
        tblRoot.Controls.Remove(tblWorkspace);

        _workspaceTabs = new TabControl
        {
            Name = "tabWorkspace",
            Dock = DockStyle.Fill,
            RightToLeft = RightToLeft.Yes,
            RightToLeftLayout = true,
            Font = new Font(Font.FontFamily, 10F, FontStyle.Bold),
            Padding = new Point(18, 7),
            HotTrack = true
        };
        _workspaceTabs.MouseDoubleClick += WorkspaceTabs_MouseDoubleClick;

        var dashboardPage = new TabPage
        {
            Name = DashboardTabKey,
            Text = "الرئيسية",
            BackColor = Color.FromArgb(247, 249, 252),
            RightToLeft = RightToLeft.Yes,
            Padding = Padding.Empty
        };

        tblWorkspace.Dock = DockStyle.Fill;
        dashboardPage.Controls.Add(tblWorkspace);
        _workspaceTabs.TabPages.Add(dashboardPage);

        tblRoot.Controls.Add(_workspaceTabs, 0, 0);
        tblRoot.ResumeLayout(true);
    }

    /// <summary>
    /// إعداد قائمة التهيئة العامة المتفرعة وربط شاشة الدول بها.
    /// </summary>
    private void ConfigureGeneralSetupMenu()
    {
        var generalSetupButton = FindButtonByText(this, "التهيئة العامة");
        if (generalSetupButton is null)
        {
            return;
        }

        _generalSetupMenu?.Dispose();
        _generalSetupMenu = new ContextMenuStrip
        {
            RightToLeft = RightToLeft.Yes,
            Font = new Font(Font.FontFamily, 10F),
            ShowImageMargin = false,
            AutoSize = true
        };

        var geographicDataItem = new ToolStripMenuItem("البيانات الجغرافية")
        {
            RightToLeft = RightToLeft.Yes
        };

        var countriesItem = new ToolStripMenuItem("الدول")
        {
            Name = "mnuCountries",
            ToolTipText = "GEN-003 — الدول"
        };
        countriesItem.Click += (_, _) => OpenCountriesTab();

        geographicDataItem.DropDownItems.Add(countriesItem);
        _generalSetupMenu.Items.Add(geographicDataItem);

        var setupScreensItem = new ToolStripMenuItem("شاشات التهيئة العامة")
        {
            RightToLeft = RightToLeft.Yes
        };
        AddScreenItems(setupScreensItem, new[]
        {
            ("GEN-008", "أنواع المركبات"),
            ("GEN-009", "العملات"),
            ("GEN-010", "أسعار الصرف"),
            ("GEN-011", "الشركات"),
            ("GEN-012", "الفروع"),
            ("GEN-013", "السنوات المالية"),
            ("GEN-014", "الترقيم العام"),
            ("GEN-015", "اللغات"),
            ("GEN-016", "المتغيرات العامة")
        });
        _generalSetupMenu.Items.Add(setupScreensItem);

        generalSetupButton.Click -= GeneralSetupButton_Click;
        generalSetupButton.Click += GeneralSetupButton_Click;
    }

    /// <summary>
    /// إظهار قائمة شاشات التهيئة العامة بمحاذاة الزر الجانبي.
    /// </summary>
    private void GeneralSetupButton_Click(object? sender, EventArgs e)
    {
        if (sender is not Button button || _generalSetupMenu is null)
        {
            return;
        }

        _generalSetupMenu.Show(button, new Point(0, button.Height));
    }

    /// <summary>
    /// ربط مجموعتي الأمن والمحاسبة بالشاشة الرئيسية عبر قوائمها الجانبية.
    /// </summary>
    private void ConfigureSecurityAndAccountingMenus()
    {
        _securityAdministrationMenu?.Dispose();
        _accountingMenu?.Dispose();

        _securityAdministrationMenu = CreateScreenMenu(new[]
        {
            ("SEC-001", "المستخدمون"),
            ("SEC-002", "الأدوار"),
            ("SEC-003", "الصلاحيات"),
            ("SEC-004", "سياسات الأمان"),
            ("SEC-005", "الأجهزة الموثوقة"),
            ("SEC-006", "الجلسات النشطة"),
            ("SEC-007", "سجل التدقيق العام"),
            ("SEC-008", "الإشعارات"),
            ("SEC-009", "قوالب الإشعارات"),
            ("SEC-010", "إدارة كلمات المرور"),
            ("SEC-011", "مفاتيح API والتكامل"),
            ("SEC-012", "سجلات تسجيل الدخول"),
            ("SEC-013", "المصادقة الثنائية"),
            ("SEC-014", "إعدادات الأمان العامة"),
            ("SEC-015", "محاولات الدخول الفاشلة"),
            ("SEC-016", "الوحدات التنظيمية"),
            ("SEC-017", "تفويض الصلاحيات"),
            ("SEC-018", "نطاقات الوصول للبيانات")
        });

        _accountingMenu = CreateScreenMenu(new[]
        {
            ("ACC-001", "دليل الحسابات"), ("ACC-002", "أنواع الحسابات"),
            ("ACC-003", "مجموعات الحسابات"), ("ACC-004", "مراكز التكلفة"),
            ("ACC-005", "الصناديق"), ("ACC-006", "الحسابات البنكية"),
            ("ACC-007", "طرق الدفع"), ("ACC-008", "السنوات والفترات المالية"),
            ("ACC-009", "القيود اليومية"), ("ACC-010", "سندات القبض"),
            ("ACC-011", "سندات الصرف"), ("ACC-012", "التحويلات المالية"),
            ("ACC-013", "تسويات القيود"), ("ACC-014", "إقفال الفترات"),
            ("ACC-015", "عكس القيود"), ("ACC-016", "دفتر اليومية"),
            ("ACC-017", "الأستاذ العام"), ("ACC-018", "كشف الحساب"),
            ("ACC-019", "ميزان المراجعة"), ("ACC-020", "قائمة الدخل"),
            ("ACC-021", "الميزانية العمومية"), ("ACC-022", "التدفقات النقدية"),
            ("ACC-023", "ميزان حسب العملة"), ("ACC-024", "التسويات البنكية"),
            ("ACC-025", "الأرصدة الافتتاحية"), ("ACC-026", "طلبات الاعتماد المحاسبية"),
            ("ACC-027", "مذكرات التسوية"), ("ACC-028", "دفتر الأستاذ المساعد"),
            ("ACC-029", "حركة مركز التكلفة"), ("ACC-030", "تحليل المصروفات"),
            ("ACC-031", "تحليل الإيرادات"), ("ACC-032", "كشف الصندوق"),
            ("ACC-033", "كشف البنك"), ("ACC-034", "مطابقة أرصدة العملاء"),
            ("ACC-035", "مطابقة أرصدة الموردين"), ("ACC-036", "قيود الاستحقاق"),
            ("ACC-037", "قيود الإهلاك"), ("ACC-038", "الفترات المقفلة"),
            ("ACC-039", "صلاحيات الاعتماد المحاسبي"), ("ACC-040", "مستويات الاعتماد"),
            ("ACC-041", "سجل الترحيل"), ("ACC-042", "سجل الإلغاء والعكس"),
            ("ACC-043", "القيود المعلقة"), ("ACC-044", "مراقبة الميزان")
        });

        BindSidebarMenu("الإدارة والأمن", _securityAdministrationMenu);
        BindSidebarMenu("المحاسبة", _accountingMenu);
    }

    private ContextMenuStrip CreateScreenMenu(IEnumerable<(string Code, string Name)> screens)
    {
        var menu = new ContextMenuStrip
        {
            RightToLeft = RightToLeft.Yes,
            Font = new Font(Font.FontFamily, 10F),
            ShowImageMargin = false,
            AutoSize = true
        };
        AddScreenItems(menu.Items, screens);
        return menu;
    }

    private void BindSidebarMenu(string buttonText, ContextMenuStrip menu)
    {
        var button = FindButtonByText(this, buttonText);
        if (button is null)
        {
            return;
        }

        button.Click += (_, _) => menu.Show(button, new Point(0, button.Height));
    }

    private void AddScreenItems(ToolStripItemCollection items, IEnumerable<(string Code, string Name)> screens)
    {
        foreach (var (code, name) in screens)
        {
            var item = new ToolStripMenuItem(name)
            {
                Name = $"mnu{code.Replace("-", string.Empty, StringComparison.Ordinal)}",
                ToolTipText = code
            };
            item.Click += (_, _) => OpenPlannedScreenTab(code, name);
            items.Add(item);
        }
    }

    private void AddScreenItems(ToolStripMenuItem parent, IEnumerable<(string Code, string Name)> screens)
    {
        AddScreenItems(parent.DropDownItems, screens);
    }

    /// <summary>
    /// فتح نموذج الشاشة الفعلي داخل تبويب واحد ومنع التكرار.
    /// </summary>
    private void OpenPlannedScreenTab(string screenCode, string screenName)
    {
        Form? screenForm = screenCode switch
        {
            "GEN-008" => new FrmVehicleTypes(),
            "GEN-009" => new FrmCurrencies(),
            "GEN-010" => new FrmExchangeRates(),
            "GEN-011" => new FrmCompanies(),
            "GEN-012" => new FrmBranches(),
            "GEN-013" => new FrmFiscalYears(),
            "GEN-014" => new FrmNumbering(),
            "GEN-015" => new FrmLanguages(),
            "GEN-016" => new FrmGeneralSettings(),
            _ => null
        };

        if (screenForm is null && SecurityScreenCatalog.TryCreate(screenCode, out var securityForm))
        {
            screenForm = securityForm;
        }

        if (screenForm is null && AccountingScreenCatalog.TryCreate(screenCode, out var accountingForm))
        {
            screenForm = accountingForm;
        }

        if (screenForm is not null)
        {
            OpenHostedScreenTab(screenCode, screenName, screenForm);
            return;
        }

        if (screenCode.StartsWith("SEC-", StringComparison.Ordinal)
            || screenCode.StartsWith("ACC-", StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"لم يتم العثور على نموذج فعلي للشاشة {screenCode}.");
        }

        if (_workspaceTabs is null)
        {
            return;
        }

        var existingPage = _workspaceTabs.TabPages
            .Cast<TabPage>()
            .FirstOrDefault(page => string.Equals(page.Name, screenCode, StringComparison.Ordinal));
        if (existingPage is not null)
        {
            _workspaceTabs.SelectedTab = existingPage;
            existingPage.Focus();
            return;
        }

        var page = new TabPage
        {
            Name = screenCode,
            Text = $"{screenName}  ×",
            BackColor = Color.FromArgb(247, 249, 252),
            RightToLeft = RightToLeft.Yes,
            Padding = new Padding(24)
        };

        var notice = new Label
        {
            Dock = DockStyle.Fill,
            Text = $"{screenCode} — {screenName}\r\n\r\nتم ربط هذه الشاشة بالشاشة الرئيسية.\r\nسيُستبدل هذا التبويب بنموذج الشاشة الفعلي عند بدء التنفيذ البرمجي المعتمد.",
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.FromArgb(70, 78, 93),
            Font = new Font(Font.FontFamily, 12F)
        };
        page.Controls.Add(notice);
        _workspaceTabs.TabPages.Add(page);
        _workspaceTabs.SelectedTab = page;
    }

    /// <summary>استضافة نموذج شاشة فعلي داخل تبويب واحد ومنع التكرار.</summary>
    private void OpenHostedScreenTab(string screenCode, string screenName, Form screenForm)
    {
        if (_workspaceTabs is null) return;
        var existing = _workspaceTabs.TabPages.Cast<TabPage>()
            .FirstOrDefault(page => string.Equals(page.Name, screenCode, StringComparison.Ordinal));
        if (existing is not null)
        {
            screenForm.Dispose();
            _workspaceTabs.SelectedTab = existing;
            return;
        }

        screenForm.Dock = DockStyle.Fill;
        screenForm.TopLevel = false;
        var page = new TabPage
        {
            Name = screenCode,
            Text = $"{screenName}  ×",
            BackColor = Color.FromArgb(247, 249, 252),
            RightToLeft = RightToLeft.Yes,
            Padding = Padding.Empty,
            Tag = screenForm
        };
        screenForm.FormClosed += (_, _) =>
        {
            if (_workspaceTabs.TabPages.Contains(page))
            {
                _workspaceTabs.TabPages.Remove(page);
                page.Dispose();
            }
        };
        page.Controls.Add(screenForm);
        _workspaceTabs.TabPages.Add(page);
        _workspaceTabs.SelectedTab = page;
        screenForm.Show();
    }

    /// <summary>
    /// فتح شاشة الدول داخل تبويب واحد ومنع فتح نسخة مكررة منها.
    /// </summary>
    private void OpenCountriesTab()
    {
        if (_workspaceTabs is null)
        {
            return;
        }

        var existingPage = _workspaceTabs.TabPages
            .Cast<TabPage>()
            .FirstOrDefault(page => string.Equals(page.Name, CountriesTabKey, StringComparison.Ordinal));

        if (existingPage is not null)
        {
            _workspaceTabs.SelectedTab = existingPage;
            existingPage.Focus();
            return;
        }

        var countriesForm = new FrmCountries();
        countriesForm.ConfigureForTabHosting();

        var countriesPage = new TabPage
        {
            Name = CountriesTabKey,
            Text = "الدول  ×",
            BackColor = Color.FromArgb(239, 245, 252),
            RightToLeft = RightToLeft.Yes,
            Padding = Padding.Empty,
            Tag = countriesForm
        };

        countriesForm.FormClosed += (_, _) =>
        {
            if (_workspaceTabs.TabPages.Contains(countriesPage))
            {
                _workspaceTabs.TabPages.Remove(countriesPage);
                countriesPage.Dispose();
            }
        };

        countriesPage.Controls.Add(countriesForm);
        _workspaceTabs.TabPages.Add(countriesPage);
        _workspaceTabs.SelectedTab = countriesPage;
        countriesForm.Show();
    }

    /// <summary>
    /// إغلاق تبويب شاشة الأعمال بالنقر المزدوج، مع حماية تبويب الرئيسية من الإغلاق.
    /// </summary>
    private void WorkspaceTabs_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        if (_workspaceTabs is null)
        {
            return;
        }

        for (var index = 0; index < _workspaceTabs.TabPages.Count; index++)
        {
            if (!_workspaceTabs.GetTabRect(index).Contains(e.Location))
            {
                continue;
            }

            var page = _workspaceTabs.TabPages[index];
            if (string.Equals(page.Name, DashboardTabKey, StringComparison.Ordinal))
            {
                return;
            }

            if (page.Tag is Form hostedForm && !hostedForm.IsDisposed)
            {
                hostedForm.Close();
            }
            else
            {
                _workspaceTabs.TabPages.Remove(page);
                page.Dispose();
            }

            return;
        }
    }

    /// <summary>
    /// البحث داخل عناصر النموذج عن زر يحتوي نصًا محددًا.
    /// </summary>
    private static Button? FindButtonByText(Control parent, string text)
    {
        foreach (Control control in parent.Controls)
        {
            if (control is Button button && button.Text.Contains(text, StringComparison.Ordinal))
            {
                return button;
            }

            var nestedButton = FindButtonByText(control, text);
            if (nestedButton is not null)
            {
                return nestedButton;
            }
        }

        return null;
    }

    /// <summary>
    /// تحميل بيانات معاينة مؤقتة إلى أن يتم ربط الشاشة بخدمات النظام وواجهة API.
    /// </summary>
    private void LoadDevelopmentPreviewData()
    {
        dgvRecentTransactions.Rows.Clear();
        dgvRecentTransactions.Rows.Add("سند قبض رقم CP-000123", "25,000 ريال", "مكتمل");
        dgvRecentTransactions.Rows.Add("سند صرف رقم PV-000455", "15,750 ريال", "معتمد");
        dgvRecentTransactions.Rows.Add("قيد يومي رقم JV-000799", "8,900 ريال", "معلق");
        dgvRecentTransactions.Rows.Add("تحويل بنكي رقم TR-000321", "12,600 ريال", "مكتمل");
        dgvRecentTransactions.Rows.Add("سند قبض رقم CP-000122", "7,250 ريال", "ملغي");

        statusBar.CompanyName = "شركة النقل الرئيسية";
        statusBar.BranchName = "الرئيسي";
        statusBar.FiscalYear = "2026";
        statusBar.FinancialPeriod = "مايو - 2026";
        statusBar.CurrentUser = "أحمد محمد";
        statusBar.CurrentRole = "مدير النظام";
        statusBar.EnvironmentName = "التطوير";
        statusBar.SystemVersion = "1.0.0.0";
        statusBar.SetConnectionStatus(true, "متصل");

        pnlRevenueChart.Invalidate();
        pnlActivityChart.Invalidate();
    }

    private void btnLogout_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void QuickAction_Click(object? sender, EventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        MessageBox.Show(
            $"سيتم فتح شاشة: {button.Text} بعد تنفيذها وربطها.",
            "TransportERP",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    /// <summary>
    /// رسم مخطط الإيرادات والمصروفات لآخر ستة أشهر.
    /// </summary>
    private void pnlRevenueChart_Paint(object? sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var area = new Rectangle(54, 48, Math.Max(220, pnlRevenueChart.ClientSize.Width - 84), Math.Max(150, pnlRevenueChart.ClientSize.Height - 92));
        var months = new[] { "ديسمبر", "يناير", "فبراير", "مارس", "أبريل", "مايو" };
        var revenues = new[] { 0.62F, 0.68F, 0.73F, 0.79F, 0.86F, 0.94F };
        var expenses = new[] { 0.42F, 0.47F, 0.51F, 0.56F, 0.61F, 0.67F };

        using var gridPen = new Pen(Color.FromArgb(230, 234, 241), 1F);
        using var revenueBrush = new SolidBrush(Color.FromArgb(47, 128, 237));
        using var expenseBrush = new SolidBrush(Color.FromArgb(235, 87, 87));
        using var textBrush = new SolidBrush(Color.FromArgb(70, 78, 93));
        using var labelFont = new Font(Font.FontFamily, 8.5F);

        for (var i = 0; i <= 5; i++)
        {
            var y = area.Bottom - (area.Height * i / 5F);
            e.Graphics.DrawLine(gridPen, area.Left, y, area.Right, y);
        }

        var groupWidth = area.Width / 6F;
        for (var i = 0; i < 6; i++)
        {
            var baseX = area.Left + (i * groupWidth) + (groupWidth * 0.2F);
            var barWidth = groupWidth * 0.22F;
            var revenueHeight = area.Height * revenues[i];
            var expenseHeight = area.Height * expenses[i];
            e.Graphics.FillRectangle(revenueBrush, baseX, area.Bottom - revenueHeight, barWidth, revenueHeight);
            e.Graphics.FillRectangle(expenseBrush, baseX + barWidth + 5F, area.Bottom - expenseHeight, barWidth, expenseHeight);
            var labelSize = e.Graphics.MeasureString(months[i], labelFont);
            e.Graphics.DrawString(months[i], labelFont, textBrush, baseX + barWidth - (labelSize.Width / 2F), area.Bottom + 8F);
        }
    }

    /// <summary>
    /// رسم مخطط دائري لتوزيع الإيرادات حسب النشاط.
    /// </summary>
    private void pnlActivityChart_Paint(object? sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var size = Math.Min(pnlActivityChart.ClientSize.Width - 170, pnlActivityChart.ClientSize.Height - 70);
        size = Math.Max(140, size);

        var donut = new Rectangle(28, 86, size, size);
        var values = new[] { 35F, 25F, 20F, 10F, 10F };
        var colors = new[]
        {
            Color.FromArgb(47, 128, 237),
            Color.FromArgb(111, 207, 151),
            Color.FromArgb(242, 153, 74),
            Color.FromArgb(155, 81, 224),
            Color.FromArgb(86, 204, 200)
        };
        var startAngle = -90F;
        for (var i = 0; i < values.Length; i++)
        {
            var sweep = values[i] * 3.6F;
            using var brush = new SolidBrush(colors[i]);
            e.Graphics.FillPie(brush, donut, startAngle, sweep);
            startAngle += sweep;
        }

        var inner = Rectangle.Inflate(donut, -(int)(size * 0.28F), -(int)(size * 0.28F));
        using var centerBrush = new SolidBrush(Color.White);
        e.Graphics.FillEllipse(centerBrush, inner);
    }
}
