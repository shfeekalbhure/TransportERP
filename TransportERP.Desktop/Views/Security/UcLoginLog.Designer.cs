using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

partial class UcLoginLog
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
                new("معايير البحث", SecurityTabKind.Details, "معايير البحث الخادمي في سجل الدخول."),
                new("النتائج والتفاصيل", SecurityTabKind.Details, "تفاصيل المحاولة المحددة للقراءة فقط."),
                new("سجل التصدير", SecurityTabKind.Audit, "عمليات التصدير والوصول للبيانات الحساسة.")
            },
            new SecurityFieldDefinition[]
            {
                new("من تاريخ", SecurityFieldKind.Date), new("إلى تاريخ", SecurityFieldKind.Date), new("المستخدم أو البريد"),
                new("الشركة", SecurityFieldKind.Choice, Array.Empty<string>()), new("الفرع", SecurityFieldKind.Choice, Array.Empty<string>()),
                new("النتيجة", SecurityFieldKind.Choice, new[] { "ناجح", "فاشل" }), new("سبب الفشل"), new("عنوان IP"),
                new("الجهاز"), new("نظام التشغيل"), new("المتصفح/العميل"), new("معرف الجلسة"),
                new("MFA", SecurityFieldKind.Choice, new[] { "مستخدم", "غير مستخدم" })
            },
            new[] { "التاريخ والوقت", "المستخدم", "الشركة", "الفرع", "النتيجة", "سبب الفشل", "IP", "الجهاز", "MFA" },
            new[] { "فتح المستخدم", "فتح الجلسة" }, SecurityWorkspaceMode.ReadOnly);
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(247, 249, 252); Controls.Add(screenShell); Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F); Name = "UcLoginLog"; RightToLeft = RightToLeft.Yes; Size = new Size(1280, 760); ResumeLayout(false);
    }
}