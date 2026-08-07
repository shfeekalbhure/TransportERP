using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

partial class UcSecurityAlertRules
{
    private System.ComponentModel.IContainer? components = null;
    private TransportReferenceScreenShell screenShell = null!;
    private TabControl tabDetails = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container(); screenShell = new TransportReferenceScreenShell(); tabDetails = new TabControl(); SuspendLayout();
        SecurityDesignerSupport.ConfigureScreen(screenShell, tabDetails,
            new SecurityTabDefinition[]
            {
                new("البيانات الرئيسية", SecurityTabKind.Details, "تعريف قاعدة التنبيه."),
                new("شروط التفعيل", SecurityTabKind.Settings, "الشروط والعتبات ونافذة الزمن."),
                new("المستلمون والقنوات", SecurityTabKind.CheckList, "المستلمون والقنوات المعتمدة."),
                new("التصعيد", SecurityTabKind.Settings, "إجراءات التصعيد المسموحة."),
                new("سجل العمليات", SecurityTabKind.Audit, "تغييرات القاعدة واختبارها وتفعيلها.")
            },
            new SecurityFieldDefinition[]
            {
                new("رمز القاعدة", SecurityFieldKind.RequiredText), new("الاسم", SecurityFieldKind.RequiredText), new("نوع الحدث", SecurityFieldKind.Choice, Array.Empty<string>()),
                new("مستوى الخطورة", SecurityFieldKind.Choice, new[] { "منخفض", "متوسط", "عال", "حرج" }), new("النطاق", SecurityFieldKind.Choice, Array.Empty<string>()),
                new("الشرط والعتبة"), new("نافذة الزمن"), new("منع التكرار", SecurityFieldKind.Boolean), new("المستلمون"), new("القنوات"),
                new("زمن التصعيد"), new("الإجراء التلقائي المسموح"), new("الحالة", SecurityFieldKind.Choice, new[] { "نشط", "موقوف", "معلق" })
            },
            new[] { "رمز القاعدة", "الاسم", "نوع الحدث", "الخطورة", "النطاق", "الحالة", "آخر تشغيل", "عدد التنبيهات" },
            new[] { "اختبار القاعدة", "نسخ القاعدة" }, SecurityWorkspaceMode.Edit);
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(247, 249, 252); Controls.Add(screenShell); Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F); Name = "UcSecurityAlertRules"; RightToLeft = RightToLeft.Yes; Size = new Size(1280, 760); ResumeLayout(false);
    }
}