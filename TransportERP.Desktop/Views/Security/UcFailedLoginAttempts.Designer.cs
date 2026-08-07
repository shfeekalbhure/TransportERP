using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

partial class UcFailedLoginAttempts
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
                new("المحاولات والحالة", SecurityTabKind.Details, "محاولات الدخول الفاشلة وحالة الحساب للقراءة فقط."),
                new("إدارة الحظر والمعالجة", SecurityTabKind.Details, "إجراءات الحظر والرفع مع السبب والتدقيق."),
                new("الأنماط والمصادر", SecurityTabKind.Details, "تجميع المحاولات حسب المصدر أو الحساب والاتجاه."),
                new("سجل العمليات", SecurityTabKind.Audit, "الحظر والتمديد والرفع والوصول للبيانات الحساسة.")
            },
            new SecurityFieldDefinition[]
            {
                new("المستخدم/المعرف المدخل"), new("الشركة"), new("الفرع"), new("وقت المحاولة", SecurityFieldKind.Date), new("سبب الفشل"),
                new("IP"), new("الجهاز"), new("الموقع"), new("عدد المحاولات ضمن النافذة"), new("حالة الحساب", SecurityFieldKind.Choice, new[] { "مفتوح", "مقفل", "موقوف" }),
                new("نوع الحظر", SecurityFieldKind.Choice, new[] { "لا يوجد", "حساب", "مصدر/IP" }), new("بداية الحظر", SecurityFieldKind.Date), new("نهاية الحظر", SecurityFieldKind.Date),
                new("المسؤول"), new("سبب المعالجة", SecurityFieldKind.Multiline), new("المرجع")
            },
            new[] { "الوقت", "المستخدم/المعرف", "سبب الفشل", "IP", "الجهاز", "عدد المحاولات", "حالة الحساب", "نوع الحظر" },
            new[] { "تحديث", "حظر حساب", "حظر IP", "تمديد الحظر", "رفع الحظر بسبب", "إنشاء تنبيه" }, SecurityWorkspaceMode.ReadOnlyWithActions);
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(247, 249, 252); Controls.Add(screenShell); Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F); Name = "UcFailedLoginAttempts"; RightToLeft = RightToLeft.Yes; Size = new Size(1280, 760); ResumeLayout(false);
    }
}