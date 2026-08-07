using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

partial class UcPasswordManagement
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
                new("طلبات إعادة التعيين", SecurityTabKind.Details, "طلبات إعادة التعيين وحالتها."),
                new("المستخدم والحالة", SecurityTabKind.Details, "حالة المستخدم والقفل والانتهاء.",
                    Fields: new SecurityFieldDefinition[]
                    {
                        new("المستخدم"),
                        new("حالة الحساب", SecurityFieldKind.Choice, new[] { "نشط", "موقوف", "مقفل" }),
                        new("حالة القفل", SecurityFieldKind.Choice, new[] { "مفتوح", "مقفل" }),
                        new("وقت القفل", SecurityFieldKind.Date),
                        new("انتهاء كلمة المرور", SecurityFieldKind.Date),
                        new("آخر تغيير لكلمة المرور", SecurityFieldKind.Date),
                        new("عدد محاولات الدخول الفاشلة"),
                        new("يتطلب تغييرًا عند الدخول", SecurityFieldKind.Boolean)
                    }),
                new("الموافقة والتنفيذ", SecurityTabKind.Details, "الموافقة والتنفيذ الآمن دون عرض السر.",
                    Fields: new SecurityFieldDefinition[]
                    {
                        new("حالة الموافقة", SecurityFieldKind.Choice, new[] { "بانتظار الموافقة", "معتمد", "مرفوض" }),
                        new("المعتمد بواسطة"),
                        new("تاريخ الاعتماد", SecurityFieldKind.Date),
                        new("قناة التسليم", SecurityFieldKind.Choice, new[] { "داخل النظام", "بريد إلكتروني", "رسالة" }),
                        new("صلاحية الرمز المؤقت بالدقائق"),
                        new("يتطلب تغييرًا عند أول دخول", SecurityFieldKind.Boolean),
                        new("سبب الرفض / الإلغاء", SecurityFieldKind.Multiline)
                    },
                    Actions: new[] { "موافقة", "رفض", "إصدار رمز مؤقت" }),
                new("سجل العمليات", SecurityTabKind.Audit, "الطلب والموافقة والرفض والإصدار والإلغاء.",
                    Columns: new[] { "التاريخ والوقت", "المستخدم", "العملية", "حالة الطلب", "نفذ بواسطة", "قناة التنفيذ", "السبب / المرجع" })
            },
            new SecurityFieldDefinition[]
            {
                new("رقم الطلب", SecurityFieldKind.RequiredText), new("المستخدم", SecurityFieldKind.RequiredText), new("الشركة", SecurityFieldKind.Choice, Array.Empty<string>()),
                new("الفرع", SecurityFieldKind.Choice, Array.Empty<string>()), new("سبب الطلب", SecurityFieldKind.Multiline), new("المصدر"),
                new("الحالة", SecurityFieldKind.Choice, new[] { "جديد", "بانتظار الموافقة", "معتمد", "مرفوض", "منتهي", "ملغى" }),
                new("تاريخ الطلب", SecurityFieldKind.Date), new("تاريخ الانتهاء", SecurityFieldKind.Date), new("المعتمد"), new("قناة التسليم"),
                new("عدد المحاولات"), new("حالة القفل", SecurityFieldKind.Choice, new[] { "مفتوح", "مقفل" }), new("يتطلب تغييرًا عند الدخول", SecurityFieldKind.Boolean),
                new("الملاحظات", SecurityFieldKind.Multiline)
            },
            new[] { "رقم الطلب", "المستخدم", "الشركة", "الحالة", "تاريخ الطلب", "الانتهاء", "حالة القفل", "المعتمد" },
            new[] { "إنشاء طلب", "إرسال للموافقة", "موافقة", "رفض بسبب", "إصدار رمز مؤقت", "فتح المستخدم" }, SecurityWorkspaceMode.Edit);
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(247, 249, 252); Controls.Add(screenShell); Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F); Name = "UcPasswordManagement"; RightToLeft = RightToLeft.Yes; Size = new Size(1280, 760); ResumeLayout(false);
    }
}
