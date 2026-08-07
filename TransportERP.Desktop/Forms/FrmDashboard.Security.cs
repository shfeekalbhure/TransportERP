using TransportERP.Desktop.Views.Security;

namespace TransportERP.Desktop;

/// <summary>
/// ربط المجموعة الثالثة — الأمن والإدارة — بقائمة FrmDashboard.
/// فصل الربط في Partial مستقل يحافظ على الشاشة الرئيسية الحالية ولا يعيد بناءها من الصفر.
/// </summary>
public partial class FrmDashboard
{
    private ContextMenuStrip? _securityMenu;
    private bool _securityMenuConfigured;

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);

        if (_securityMenuConfigured)
        {
            return;
        }

        ConfigureSecurityMenu();
        _securityMenuConfigured = true;
    }

    private void ConfigureSecurityMenu()
    {
        var securityButton = FindButtonByText(this, "الإدارة والأمن");
        if (securityButton is null)
        {
            return;
        }

        _securityMenu?.Dispose();
        _securityMenu = new ContextMenuStrip
        {
            RightToLeft = RightToLeft.Yes,
            Font = new Font(Font.FontFamily, 10F),
            ShowImageMargin = false,
            AutoSize = true
        };

        AddSecurityMenuItem("SEC-017", "تفويض الصلاحيات", static () => new UcPermissionDelegations());
        AddSecurityMenuItem("SEC-018", "الأدوار", static () => new UcRoles());
        AddSecurityMenuItem("SEC-019", "مجموعات المستخدمين", static () => new UcUserGroups());
        AddSecurityMenuItem("SEC-020", "كتالوج الصلاحيات", static () => new UcPermissionCatalog());
        AddSecurityMenuItem("SEC-021", "سياسات الأمان", static () => new UcSecurityPolicies());
        AddSecurityMenuItem("SEC-022", "سجل الدخول", static () => new UcLoginLog());
        AddSecurityMenuItem("SEC-023", "المصادقة متعددة العوامل", static () => new UcMultiFactorAuthentication());
        AddSecurityMenuItem("SEC-024", "تنبيهات الأمان", static () => new UcSecurityAlertRules());
        AddSecurityMenuItem("SEC-025", "سجل التنبيهات الأمنية", static () => new UcSecurityAlertLog());
        AddSecurityMenuItem("SEC-026", "الوحدات التنظيمية", static () => new UcOrganizationalUnits());
        AddSecurityMenuItem("SEC-027", "سجل التدقيق العام", static () => new UcAuditLog());
        AddSecurityMenuItem("SEC-028", "الإشعارات", static () => new UcNotifications());
        AddSecurityMenuItem("SEC-029", "قوالب الإشعارات", static () => new UcNotificationTemplates());
        AddSecurityMenuItem("SEC-030", "إدارة كلمات المرور", static () => new UcPasswordManagement());
        AddSecurityMenuItem("SEC-031", "إعدادات الأمان العامة", static () => new UcGeneralSecuritySettings());
        AddSecurityMenuItem("SEC-032", "الجلسات النشطة والأجهزة الموثوقة", static () => new UcActiveSessionsTrustedDevices());
        AddSecurityMenuItem("SEC-033", "مفاتيح API والتكامل", static () => new UcApiKeysIntegration());
        AddSecurityMenuItem("SEC-034", "محاولات الدخول الفاشلة", static () => new UcFailedLoginAttempts());

        securityButton.Click += SecurityButton_Click;
    }

    private void AddSecurityMenuItem(string screenKey, string title, Func<UserControl> viewFactory)
    {
        if (_securityMenu is null)
        {
            return;
        }

        var item = new ToolStripMenuItem(title)
        {
            Name = $"mnu{screenKey.Replace("-", string.Empty, StringComparison.Ordinal)}",
            ToolTipText = $"{screenKey} — {title}",
            RightToLeft = RightToLeft.Yes
        };

        item.Click += (_, _) => OpenWorkspaceView(screenKey, title, viewFactory());
        _securityMenu.Items.Add(item);
    }

    private void SecurityButton_Click(object? sender, EventArgs e)
    {
        if (sender is not Button button || _securityMenu is null)
        {
            return;
        }

        _securityMenu.Show(button, new Point(0, button.Height));
    }
}