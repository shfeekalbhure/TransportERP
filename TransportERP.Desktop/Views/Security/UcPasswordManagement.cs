using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-030 — إدارة كلمات المرور؛ إدارة طلبات إعادة التعيين والقفل دون عرض أو تخزين كلمة المرور في الواجهة.</summary>
public partial class UcPasswordManagement : UserControl
{
    public UcPasswordManagement()
    {
        InitializeComponent();
        SecurityViewRuntime.Initialize(this, screenShell, "إدارة كلمات المرور", "ابحث بالمستخدم أو رقم الطلب...", SecurityWorkspaceMode.Edit);
    }
}