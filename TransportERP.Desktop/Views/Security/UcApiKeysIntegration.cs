using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-033 — مفاتيح API والتكامل؛ إدارة المفاتيح والنطاقات والدوران دون كشف السر بعد الإنشاء.</summary>
public partial class UcApiKeysIntegration : UserControl
{
    public UcApiKeysIntegration()
    {
        InitializeComponent();
        SecurityViewRuntime.Initialize(this, screenShell, "مفاتيح API والتكامل", "ابحث بمعرف المفتاح أو العميل أو المالك...", SecurityWorkspaceMode.Edit);
    }
}