using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

partial class UcNotifications
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
                new("البيانات الرئيسية", SecurityTabKind.Details, "بيانات الإشعار ومحتواه الأساسي."),
                new("المستلمون", SecurityTabKind.CheckList, "المستخدمون والأدوار والمجموعات المستلمة."),
                new("قنوات الإرسال", SecurityTabKind.CheckList, "القنوات المعتمدة للإرسال."),
                new("المعاينة والجدولة", SecurityTabKind.Details, "معاينة وجدولة الإشعار قبل الإرسال."),
                new("سجل الإرسال والقراءة", SecurityTabKind.Audit, "نتيجة التسليم والقراءة حسب القناة."),
                new("سجل العمليات", SecurityTabKind.Audit, "إنشاء وتعديل وجدولة وإرسال وإلغاء الإشعار.")
            },
            new SecurityFieldDefinition[]
            {
                new("رقم الإشعار", SecurityFieldKind.RequiredText), new("العنوان", SecurityFieldKind.RequiredText), new("النص", SecurityFieldKind.Multiline),
                new("القالب", SecurityFieldKind.Choice, Array.Empty<string>()), new("نوع الإشعار", SecurityFieldKind.Choice, Array.Empty<string>()),
                new("الأولوية", SecurityFieldKind.Choice, new[] { "عادية", "عالية", "حرجة" }), new("المستلمون"), new("الشركة", SecurityFieldKind.Choice, Array.Empty<string>()),
                new("الفرع", SecurityFieldKind.Choice, Array.Empty<string>()), new("القنوات"), new("تاريخ الجدولة", SecurityFieldKind.Date), new("تاريخ الانتهاء", SecurityFieldKind.Date),
                new("الرابط المرجعي"), new("يتطلب إقرارًا", SecurityFieldKind.Boolean), new("الحالة", SecurityFieldKind.Choice, new[] { "مسودة", "مجدول", "مرسل", "ملغى" })
            },
            new[] { "رقم الإشعار", "العنوان", "النوع", "الأولوية", "الجدولة", "الحالة", "عدد المستلمين", "حالة التسليم" },
            new[] { "حفظ كمسودة", "معاينة", "جدولة", "إرسال", "إعادة إرسال للفاشل" }, SecurityWorkspaceMode.Edit);
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(247, 249, 252); Controls.Add(screenShell); Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F); Name = "UcNotifications"; RightToLeft = RightToLeft.Yes; Size = new Size(1280, 760); ResumeLayout(false);
    }
}