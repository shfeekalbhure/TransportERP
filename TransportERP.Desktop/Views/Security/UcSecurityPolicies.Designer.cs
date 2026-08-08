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
                new("كلمة المرور والمصادقة", SecurityTabKind.Settings, "ضوابط كلمة المرور والمصادقة متعددة العوامل.",
                    Fields: new SecurityFieldDefinition[]
                    {
                        new("الحد الأدنى لطول كلمة المرور"),
                        new("التعقيد المطلوب", SecurityFieldKind.Choice, new[] { "أساسي", "متوسط", "قوي" }),
                        new("مدة صلاحية كلمة المرور بالأيام"),
                        new("عدد كلمات المرور الممنوع إعادة استخدامها"),
                        new("السماح بتغيير كلمة المرور", SecurityFieldKind.Boolean),
                        new("إلزام MFA", SecurityFieldKind.Choice, new[] { "إلزامي", "اختياري", "غير مفعل" }),
                        new("طرق MFA المسموحة"),
                        new("فترة السماح للتسجيل")
                    }),
                new("الدخول والجلسات", SecurityTabKind.Settings, "ضوابط محاولات الدخول والجلسات والأجهزة.",
                    Fields: new SecurityFieldDefinition[]
                    {
                        new("الحد الأقصى لمحاولات الدخول"),
                        new("مدة الحظر بالدقائق"),
                        new("مهلة خمول الجلسة بالدقائق"),
                        new("الحد الأقصى للجلسات المتزامنة"),
                        new("السماح بالأجهزة الموثوقة", SecurityFieldKind.Boolean),
                        new("مدة الثقة بالجهاز بالأيام"),
                        new("إنهاء الجلسات عند تغيير كلمة المرور", SecurityFieldKind.Boolean)
                    }),
                new("التدقيق والتنبيهات", SecurityTabKind.Settings, "إعدادات التدقيق وقنوات التنبيه.",
                    Fields: new SecurityFieldDefinition[]
                    {
                        new("مستوى التدقيق", SecurityFieldKind.Choice, new[] { "أساسي", "تفصيلي", "موسع" }),
                        new("تسجيل العمليات الحساسة", SecurityFieldKind.Boolean),
                        new("تنبيه عند فشل الدخول المتكرر", SecurityFieldKind.Boolean),
                        new("تنبيه عند تغيير صلاحية حساسة", SecurityFieldKind.Boolean),
                        new("قنوات التنبيه"),
                        new("مدة الاحتفاظ بالسجلات بالأيام")
                    }),
                new("سجل العمليات", SecurityTabKind.Audit, "سجل تغييرات السياسة وتفعيلها.",
                    Columns: new[] { "التاريخ والوقت", "المستخدم", "العملية", "القيمة السابقة", "القيمة الجديدة", "السبب / المرجع" })
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
        screenShell.DataGroup.Dock = DockStyle.Fill;
        tabDetails.Dock = DockStyle.Fill;
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(247, 249, 252); Controls.Add(screenShell); Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F); Name = "UcSecurityPolicies"; RightToLeft = RightToLeft.Yes; Size = new Size(1280, 760); ResumeLayout(false);
    }
}
