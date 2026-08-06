namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة سياسات الجلسات الفعلية وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmSessionPolicies : SecurityWorkspaceForm
{
    public FrmSessionPolicies() : base(new SecurityScreenDefinition(
        "FrmSessionPolicies", "SEC-017", "سياسات الجلسات", "اسم السياسة *", "الحد الأقصى للجلسات", "مهلة الخمول"))
    {
    }
}
