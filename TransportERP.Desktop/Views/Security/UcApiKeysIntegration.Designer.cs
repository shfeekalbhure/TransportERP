using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

partial class UcApiKeysIntegration
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
                new("البيانات الرئيسية", SecurityTabKind.Details, "هوية المفتاح والعميل دون حفظ السر الخام."),
                new("النطاقات والصلاحيات", SecurityTabKind.CheckList, "الصلاحيات والنطاقات وفق مبدأ أقل صلاحية."),
                new("القيود والدوران", SecurityTabKind.Settings, "IP ومعدل الطلبات والانتهاء والدوران."),
                new("سجل الاستخدام", SecurityTabKind.Audit, "الاستخدام والأخطاء دون عرض السر."),
                new("سجل العمليات", SecurityTabKind.Audit, "الإنشاء والكشف الأول والدوران والإلغاء وتغيير النطاق.")
            },
            new SecurityFieldDefinition[]
            {
                new("معرف المفتاح", SecurityFieldKind.RequiredText), new("اسم العميل", SecurityFieldKind.RequiredText), new("المالك"),
                new("الشركة", SecurityFieldKind.Choice, Array.Empty<string>()), new("الفروع"), new("الصلاحيات"), new("عناوين IP المسموحة"),
                new("معدل الطلبات"), new("تاريخ الإصدار", SecurityFieldKind.Date), new("تاريخ الانتهاء", SecurityFieldKind.Date), new("آخر استخدام", SecurityFieldKind.Date),
                new("حالة المفتاح", SecurityFieldKind.Choice, new[] { "نشط", "موقوف", "ملغى", "منتهي" }), new("تاريخ الدوران", SecurityFieldKind.Date),
                new("بصمة المفتاح"), new("الملاحظات", SecurityFieldKind.Multiline)
            },
            new[] { "معرف المفتاح", "العميل", "المالك", "الحالة", "الانتهاء", "آخر استخدام", "الدوران", "البصمة" },
            new[] { "إنشاء مفتاح", "كشف السر مرة واحدة", "تدوير", "إلغاء", "اختبار وصول" }, SecurityWorkspaceMode.Edit);
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(247, 249, 252); Controls.Add(screenShell); Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F); Name = "UcApiKeysIntegration"; RightToLeft = RightToLeft.Yes; Size = new Size(1280, 760); ResumeLayout(false);
    }
}