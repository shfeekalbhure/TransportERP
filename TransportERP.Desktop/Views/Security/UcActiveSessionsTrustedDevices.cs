using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-032 — إدارة الجلسات النشطة والأجهزة الموثوقة؛ عرض الحالة وتنفيذ إجراءات إنهاء أو إلغاء الثقة المدققة.</summary>
public partial class UcActiveSessionsTrustedDevices : UserControl
{
    public UcActiveSessionsTrustedDevices()
    {
        InitializeComponent();
        SecurityViewRuntime.Initialize(this, screenShell, "الجلسات النشطة والأجهزة الموثوقة", "ابحث بالمستخدم أو الجهاز أو IP...", SecurityWorkspaceMode.ReadOnlyWithActions);
    }
}