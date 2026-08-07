using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-025 — سجل التنبيهات الأمنية؛ السجل الأصلي للقراءة فقط وإجراءات المعالجة تسجل كأثر مستقل.</summary>
public partial class UcSecurityAlertLog : UserControl
{
    public UcSecurityAlertLog()
    {
        InitializeComponent();
        SecurityViewRuntime.Initialize(this, screenShell, "سجل التنبيهات الأمنية", "ابحث في التنبيهات الأمنية...", SecurityWorkspaceMode.ReadOnlyWithActions);
    }
}