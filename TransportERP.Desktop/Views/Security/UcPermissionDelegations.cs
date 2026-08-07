using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>
/// SEC-017 — تفويض الصلاحيات.
/// واجهة أمنية تعمل داخل FrmDashboard كـ UserControl، ولا تحتوي على منطق أعمال أو وصول مباشر لقاعدة البيانات.
/// </summary>
public partial class UcPermissionDelegations : UserControl
{
    public UcPermissionDelegations()
    {
        InitializeComponent();
        SecurityViewRuntime.Initialize(this, screenShell, "تفويض الصلاحيات", "ابحث في تفويض الصلاحيات...", SecurityWorkspaceMode.Edit);
    }
}