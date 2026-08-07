using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

partial class UcSecurityPolicies
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
                new("البيانات الرئيسية", SecurityTabKind.Details, "تعريف السياسة ونطاق تطبيقها."),
                new("كلمة المرور والمصادقة", SecurityTabKind.Settings, "ضوابط كلمة المرور والمصادقة متعددة العوامل."),
                new("الدخول والجلسات", SecurityTabKind.Settings, "ضوابط محاولات الدخول والجلسات والأجهزة."),
                new("التدقيق والتنبيهات", SecurityTabKind.Settings, "إعدادات التدقيق وقنوات التنبيه."),
                new("سجل العمليات", SecurityTabKind.Audit, "سجل تغييرات السياسة وتفعيلها.")
            },
            new SecurityFieldDefinition[]
            {
                new("رمز السياسة", SecurityFieldKind.RequiredText), new("الاسم", SecurityFieldKind.RequiredText), new("النطاق", SecurityFieldKind.Choice, Array.Empty<string>()),
                new("الحد الأدنى للطول والتعقيد"), new("مدة الصلاحية"), new("منع إعادة الاستخدام", SecurityFieldKind.Boolean),
                new("عدد المحاولات"), new("مدة الحظر"), new("مهلة الجلسة"), new("الأجهزة المتزامنة"),
                new("MFA", SecurityFieldKind.Choice, new[] { "إلزامي", "اختياري", "غير مفعل" }), new("قنوات التنبيه"),
                new("الحالة", SecurityFieldKind.Choice, new[] { "نشط", "موقوف", "معلق", "منتهي" })
            },
            new[] { "رمز السياسة", "الاسم", "النطاق", "الحالة", "MFA", "تاريخ السريان" },
            new[] { "محاكاة السياسة", "تفعيل السياسة" }, SecurityWorkspaceMode.Edit);
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(247, 249, 252); Controls.Add(screenShell); Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F); Name = "UcSecurityPolicies"; RightToLeft = RightToLeft.Yes; Size = new Size(1280, 760); ResumeLayout(false);
    }
}