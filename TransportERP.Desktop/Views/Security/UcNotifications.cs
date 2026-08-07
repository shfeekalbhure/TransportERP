using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-028 — الإشعارات؛ إنشاء وجدولة وإرسال الإشعارات وتتبع التسليم والقراءة.</summary>
public partial class UcNotifications : UserControl
{
    public UcNotifications()
    {
        InitializeComponent();
        SecurityViewRuntime.Initialize(this, screenShell, "الإشعارات", "ابحث برقم الإشعار أو العنوان أو المستلم...", SecurityWorkspaceMode.Edit);
    }
}