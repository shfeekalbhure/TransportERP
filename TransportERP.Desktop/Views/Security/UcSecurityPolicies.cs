using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-021 — سياسات الأمان؛ إدارة سياسات كلمات المرور والمصادقة والجلسات والتدقيق ضمن النطاق المعتمد.</summary>
public partial class UcSecurityPolicies : UserControl
{
    public UcSecurityPolicies()
    {
        InitializeComponent();
        SecurityViewRuntime.Initialize(this, screenShell, "سياسات الأمان", "ابحث في سياسات الأمان...", SecurityWorkspaceMode.Edit);
    }
}