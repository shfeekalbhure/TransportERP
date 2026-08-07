using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-023 — المصادقة متعددة العوامل؛ إدارة طرق MFA وحالة التسجيل والاسترداد دون كشف الأسرار.</summary>
public partial class UcMultiFactorAuthentication : UserControl
{
    public UcMultiFactorAuthentication()
    {
        InitializeComponent();
        SecurityViewRuntime.Initialize(this, screenShell, "المصادقة متعددة العوامل", "ابحث بالمستخدم أو طريقة المصادقة...", SecurityWorkspaceMode.Edit);
    }
}