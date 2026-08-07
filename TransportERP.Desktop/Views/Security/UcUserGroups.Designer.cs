using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

partial class UcUserGroups
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
                new("البيانات الرئيسية", SecurityTabKind.Details, "بيانات المجموعة الأساسية."),
                new("الأعضاء", SecurityTabKind.Details, "أعضاء المجموعة الفعليون."),
                new("الإشعارات والتوزيع", SecurityTabKind.Settings, "قنوات الإشعار وقواعد التوزيع والتصعيد."),
                new("سجل العمليات", SecurityTabKind.Audit, "سجل عمليات المجموعة.")
            },
            new SecurityFieldDefinition[]
            {
                new("رمز المجموعة", SecurityFieldKind.RequiredText), new("الاسم العربي", SecurityFieldKind.RequiredText), new("الاسم الإنجليزي"),
                new("الوصف", SecurityFieldKind.Multiline), new("الشركة", SecurityFieldKind.Choice, Array.Empty<string>()), new("نطاق الفروع"),
                new("نوع المجموعة", SecurityFieldKind.Choice, Array.Empty<string>()), new("مالك المجموعة", SecurityFieldKind.Choice, Array.Empty<string>()),
                new("الحالة", SecurityFieldKind.Choice, new[] { "نشط", "موقوف", "معلق", "منتهي" }), new("تاريخ البداية", SecurityFieldKind.Date),
                new("تاريخ النهاية", SecurityFieldKind.Date), new("الملاحظات", SecurityFieldKind.Multiline)
            },
            new[] { "رمز المجموعة", "الاسم العربي", "الشركة", "نوع المجموعة", "المالك", "الحالة", "عدد الأعضاء" },
            new[] { "إضافة عضو", "إزالة عضو" }, SecurityWorkspaceMode.Edit);
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(247, 249, 252); Controls.Add(screenShell); Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F); Name = "UcUserGroups"; RightToLeft = RightToLeft.Yes; Size = new Size(1280, 760); ResumeLayout(false);
    }
}