using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-024 — تنبيهات الأمان؛ تعريف قواعد التنبيه ومستويات الخطورة والمستلمين والتصعيد.</summary>
public partial class UcSecurityAlertRules : UserControl
{
    public UcSecurityAlertRules()
    {
        InitializeComponent();
        SecurityViewRuntime.Initialize(this, screenShell, "تنبيهات الأمان", "ابحث في قواعد التنبيه...", SecurityWorkspaceMode.Edit);
    }
}