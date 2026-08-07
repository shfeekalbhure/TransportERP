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
                new("الجلسات والأجهزة", SecurityTabKind.Settings, "إعدادات الجلسات والأجهزة الموثوقة."),
                new("التنبيهات والحماية", SecurityTabKind.Settings, "التنبيهات والحماية ومستوى التسجيل."),
                new("الشبكة والتكامل", SecurityTabKind.Settings, "قيود الشبكة والتكامل دون أسرار صريحة."),
                new("سجل العمليات", SecurityTabKind.Audit, "القيم المعدلة ومصدرها واعتمادها.")
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