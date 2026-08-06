namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة مراجعات الوصول الفعلية (SEC-017) وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmAccessReviews : SecurityWorkspaceForm
{
    public FrmAccessReviews() : base(new SecurityScreenDefinition(
        "FrmAccessReviews", "SEC-017", "مراجعات الوصول", "اسم المراجعة *", "المالك المسؤول", "تاريخ الاستحقاق"))
    {
    }
}
