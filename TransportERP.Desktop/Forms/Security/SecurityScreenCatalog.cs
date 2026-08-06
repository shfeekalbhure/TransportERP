using TransportERP.Desktop.Services;

using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.Security;

public static class SecurityScreenCatalog
{
    public static bool TryCreate(string code, out Form? form) => TryCreate(code, null, out form);

    /// <summary>نقطة إنشاء شاشات الأمن؛ يمرر التركيب الرئيسي عميل API المعتمد.</summary>
    public static bool TryCreate(string code, AccessControlApiClient? accessControlClient, out Form? form)
    {
        form = code.Trim().ToUpperInvariant() switch
        {
            "SEC-001" => new FrmUsers(accessControlClient),
            "SEC-002" => new FrmRoles(),
            "SEC-003" => new FrmPermissions(),
            "SEC-004" => new FrmSecurityPolicies(),
            "SEC-005" => new FrmTrustedDevices(),
            "SEC-006" => new FrmActiveSessions(),
            "SEC-007" => new FrmAuditLog(),
            "SEC-008" => new FrmNotifications(),
            "SEC-009" => new FrmNotificationTemplates(),
            "SEC-010" => new FrmPasswordManagement(),
            "SEC-011" => new FrmApiKeysAndIntegration(),
            "SEC-012" => new FrmLoginLogs(),
            "SEC-013" => new FrmTwoFactorAuthentication(),
            "SEC-014" => new FrmGeneralSecuritySettings(),
            "SEC-015" => new FrmFailedLoginAttempts(),
            "SEC-016" => new FrmOrganizationalUnits(),
            "SEC-017" => new FrmAccessReviews(),
            "SEC-018" => new FrmDataAccessScopes(),
            _ => null
        };
        return form is not null;
    }
}

/// <summary>إدارة مفاتيح التكامل؛ قيمة المفتاح نفسها تُخفى دائماً ولا تُدرج في الجدول.</summary>
public sealed class FrmApiKeysAndIntegration : SecurityWorkspaceForm
{
    public FrmApiKeysAndIntegration() : base(new SecurityScreenDefinition(
        "FrmApiKeysAndIntegration", "SEC-011", "مفاتيح API والتكامل", "اسم التكامل *", "مفتاح API", "حالة التكامل")) { }
}

/// <summary>تحديد نطاق وصول المستخدم أو الدور إلى بيانات الشركات والفروع.</summary>
public sealed class FrmDataAccessScopes : SecurityWorkspaceForm
{
    public FrmDataAccessScopes() : base(new SecurityScreenDefinition(
        "FrmDataAccessScopes", "SEC-018", "نطاقات الوصول للبيانات", "المستخدم أو الدور *", "الشركة/الفرع", "نطاق البيانات")) { }
}
