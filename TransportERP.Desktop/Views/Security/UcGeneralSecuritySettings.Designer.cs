using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

partial class UcGeneralSecuritySettings
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
                new("الإعدادات العامة", SecurityTabKind.Settings, "القيم الأمنية العامة ضمن النطاق."),
                new("الجلسات والأجهزة", SecurityTabKind.Settings, "إعدادات الجلسات والأجهزة الموثوقة.",
                    Fields: new SecurityFieldDefinition[]
                    {
                        new("مهلة الخمول بالدقائق"),
                        new("الحد الأقصى للجلسات المتزامنة"),
                        new("السماح بتذكر الجلسة", SecurityFieldKind.Boolean),
                        new("سياسة الجهاز الموثوق", SecurityFieldKind.Choice, new[] { "غير مسموح", "بموافقة", "مسموح ضمن السياسة" }),
                        new("مدة الثقة بالجهاز بالأيام"),
                        new("إنهاء الجلسة عند تغيير بيانات الأمان", SecurityFieldKind.Boolean),
                        new("تسجيل بصمة الجهاز", SecurityFieldKind.Boolean)
                    }),
                new("التنبيهات والحماية", SecurityTabKind.Settings, "التنبيهات والحماية ومستوى التسجيل.",
                    Fields: new SecurityFieldDefinition[]
                    {
                        new("مستوى السجل", SecurityFieldKind.Choice, new[] { "أساسي", "تفصيلي", "موسع" }),
                        new("قنوات التنبيه"),
                        new("تنبيه المسؤول عند حدث حرج", SecurityFieldKind.Boolean),
                        new("تنبيه المستخدم عند دخول جديد", SecurityFieldKind.Boolean),
                        new("تفعيل الحماية من المحاولات المتكررة", SecurityFieldKind.Boolean),
                        new("حالة الصيانة الأمنية", SecurityFieldKind.Boolean)
                    }),
                new("الشبكة والتكامل", SecurityTabKind.Settings, "قيود الشبكة والتكامل دون أسرار صريحة.",
                    Fields: new SecurityFieldDefinition[]
                    {
                        new("قوائم IP المسموحة"),
                        new("قوائم IP المحظورة"),
                        new("قيود الموقع الجغرافي"),
                        new("السماح بتكاملات API", SecurityFieldKind.Boolean),
                        new("فرض HTTPS", SecurityFieldKind.Boolean),
                        new("الحد الافتراضي لمعدل الطلبات"),
                        new("تسجيل طلبات التكامل", SecurityFieldKind.Boolean)
                    }),
                new("سجل العمليات", SecurityTabKind.Audit, "القيم المعدلة ومصدرها واعتمادها.",
                    Columns: new[] { "التاريخ والوقت", "المستخدم", "الإعداد", "القيمة السابقة", "القيمة الجديدة", "النطاق", "الاعتماد / المرجع" })
            },
            new SecurityFieldDefinition[]
            {
                new("نطاق الإعداد", SecurityFieldKind.Choice, Array.Empty<string>()), new("مفاتيح الأمان"), new("السماح بالتذكر", SecurityFieldKind.Boolean),
                new("مهلة الخمول"), new("عدد الجلسات"), new("سياسة الجهاز الموثوق"), new("قوائم IP"), new("قيود الموقع"),
                new("مستوى السجل", SecurityFieldKind.Choice, Array.Empty<string>()), new("قنوات التنبيه"), new("حالة الصيانة الأمنية", SecurityFieldKind.Boolean),
                new("تاريخ السريان", SecurityFieldKind.Date)
            },
            Array.Empty<string>(), new[] { "اختبار الإعدادات", "استعادة القيم المعتمدة", "تفعيل" }, SecurityWorkspaceMode.Settings);
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(247, 249, 252); Controls.Add(screenShell); Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F); Name = "UcGeneralSecuritySettings"; RightToLeft = RightToLeft.Yes; Size = new Size(1280, 760); ResumeLayout(false);
    }
}
