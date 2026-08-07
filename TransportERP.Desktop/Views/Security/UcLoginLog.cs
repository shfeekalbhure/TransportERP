using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-022 — سجل الدخول؛ استعلام أمني للقراءة فقط مع تقنيع البيانات الحساسة حسب الصلاحية.</summary>
public partial class UcLoginLog : UserControl
{
    public UcLoginLog()
    {
        InitializeComponent();
        SecurityViewRuntime.Initialize(this, screenShell, "سجل الدخول", "ابحث بالمستخدم أو البريد أو IP أو الجهاز...", SecurityWorkspaceMode.ReadOnly);
    }
}