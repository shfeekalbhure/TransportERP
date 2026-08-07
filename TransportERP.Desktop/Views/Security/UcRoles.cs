using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-018 — الأدوار؛ تعريف الأدوار وربطها بالصلاحيات ونطاقات البيانات والمستخدمين.</summary>
public partial class UcRoles : UserControl
{
    public UcRoles()
    {
        InitializeComponent();
        SecurityViewRuntime.Initialize(this, screenShell, "الأدوار", "ابحث في الأدوار...", SecurityWorkspaceMode.Edit);
    }
}