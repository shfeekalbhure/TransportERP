using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-034 — محاولات الدخول الفاشلة؛ تحليل السجل وإدارة الحظر دون تعديل سجل المحاولة الأصلي.</summary>
public partial class UcFailedLoginAttempts : UserControl
{
    public UcFailedLoginAttempts()
    {
        InitializeComponent();
        SecurityViewRuntime.Initialize(this, screenShell, "محاولات الدخول الفاشلة", "ابحث بالمستخدم أو IP أو الجهاز أو سبب الفشل...", SecurityWorkspaceMode.ReadOnlyWithActions);
    }
}