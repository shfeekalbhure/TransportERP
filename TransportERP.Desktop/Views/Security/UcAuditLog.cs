using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-027 — سجل التدقيق العام؛ استعلام للقراءة فقط مع مقارنة القيم قبل/بعد وتقنيع القيم الحساسة.</summary>
public partial class UcAuditLog : UserControl
{
    public UcAuditLog()
    {
        InitializeComponent();
        SecurityViewRuntime.Initialize(this, screenShell, "سجل التدقيق العام", "ابحث بمعرف العملية أو الطلب أو السجل...", SecurityWorkspaceMode.ReadOnly);
    }
}