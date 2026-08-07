using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

partial class UcActiveSessionsTrustedDevices
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
                new("الجلسات النشطة", SecurityTabKind.Details, "الجلسات النشطة وحالتها الحالية."),
                new("الأجهزة الموثوقة", SecurityTabKind.Details, "الأجهزة الموثوقة وتاريخ الثقة وآخر استخدام."),
                new("تفاصيل الجلسة والجهاز", SecurityTabKind.Details, "تفاصيل الجلسة والجهاز والبيانات الشبكية حسب الصلاحية."),
                new("سجل العمليات", SecurityTabKind.Audit, "إنهاء الجلسات وإلغاء الثقة والوصول الحساس.")
            },
            new SecurityFieldDefinition[]
            {
                new("المستخدم"), new("الشركة"), new("الفرع"), new("معرف الجلسة"), new("وقت البدء", SecurityFieldKind.Date),
                new("آخر نشاط", SecurityFieldKind.Date), new("الانتهاء", SecurityFieldKind.Date), new("IP"), new("الجهاز"), new("نظام التشغيل"),
                new("العميل"), new("الموقع"), new("MFA", SecurityFieldKind.Choice, new[] { "مستخدم", "غير مستخدم" }),
                new("حالة الجلسة", SecurityFieldKind.Choice, new[] { "نشطة", "منتهية" }), new("حالة الثقة", SecurityFieldKind.Choice, new[] { "موثوق", "ملغى" }),
                new("تاريخ الثقة", SecurityFieldKind.Date), new("آخر استخدام", SecurityFieldKind.Date), new("سبب الإنهاء")
            },
            new[] { "المستخدم", "معرف الجلسة", "آخر نشاط", "IP", "الجهاز", "MFA", "حالة الجلسة", "حالة الثقة" },
            new[] { "تحديث", "إنهاء جلسة", "إنهاء كل جلسات المستخدم", "إلغاء الثقة", "فتح المستخدم" }, SecurityWorkspaceMode.ReadOnlyWithActions);
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(247, 249, 252); Controls.Add(screenShell); Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F); Name = "UcActiveSessionsTrustedDevices"; RightToLeft = RightToLeft.Yes; Size = new Size(1280, 760); ResumeLayout(false);
    }
}