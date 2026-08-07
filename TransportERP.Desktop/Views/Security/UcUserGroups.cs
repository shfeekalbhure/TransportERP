using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-019 — مجموعات المستخدمين؛ تنظيم العضوية والتوزيع والإشعارات دون منح صلاحيات.</summary>
public partial class UcUserGroups : UserControl
{
    public UcUserGroups()
    {
        InitializeComponent();
        SecurityViewRuntime.Initialize(this, screenShell, "مجموعات المستخدمين", "ابحث في مجموعات المستخدمين...", SecurityWorkspaceMode.Edit);
    }
}