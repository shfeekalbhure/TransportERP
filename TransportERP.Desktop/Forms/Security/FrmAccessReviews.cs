namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة مراجعات الوصول الفعلية وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmAccessReviews : SecurityWorkspaceForm
{
    public FrmAccessReviews() : base(new SecurityScreenDefinition(
        "FrmAccessReviews", "SEC-018", "مراجعات الوصول", "اسم المراجعة *", "المالك المسؤول", "تاريخ الاستحقاق"))
    {
    }
}
