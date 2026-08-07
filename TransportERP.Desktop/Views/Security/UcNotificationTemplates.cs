using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-029 — قوالب الإشعارات؛ قوالب متعددة القنوات واللغات مع متغيرات وإصدارات معتمدة.</summary>
public partial class UcNotificationTemplates : UserControl
{
    public UcNotificationTemplates()
    {
        InitializeComponent();
        SecurityViewRuntime.Initialize(this, screenShell, "قوالب الإشعارات", "ابحث برمز القالب أو الاسم أو القناة...", SecurityWorkspaceMode.Edit);
    }
}