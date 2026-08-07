using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

partial class UcOrganizationalUnits
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
                new("البيانات الرئيسية", SecurityTabKind.Details, "بيانات الوحدة المحددة."),
                new("الهيكل التنظيمي", SecurityTabKind.Tree, "الشجرة التنظيمية متعددة المستويات."),
                new("المسؤولون والاتصال", SecurityTabKind.Details, "المدير وبديله وبيانات الاتصال.",
                    Fields: new SecurityFieldDefinition[]
                    {
                        new("المدير", SecurityFieldKind.Choice, Array.Empty<string>()),
                        new("بديل المدير", SecurityFieldKind.Choice, Array.Empty<string>()),
                        new("البريد الإلكتروني"),
                        new("الهاتف"),
                        new("هاتف بديل"),
                        new("جهة الاتصال للطوارئ"),
                        new("ملاحظات الاتصال", SecurityFieldKind.Multiline)
                    }),
                new("الصلاحيات والنطاق", SecurityTabKind.CheckList, "نطاق الوحدة والصلاحيات المرتبطة."),
                new("سجل العمليات", SecurityTabKind.Audit, "الإنشاء والنقل وتغيير الأب والمدير والنطاق.",
                    Columns: new[] { "التاريخ والوقت", "المستخدم", "العملية", "الوحدة السابقة", "الوحدة الجديدة", "السبب / المرجع" })
            },
            new SecurityFieldDefinition[]
            {
                new("رمز الوحدة", SecurityFieldKind.RequiredText), new("الاسم العربي", SecurityFieldKind.RequiredText), new("الاسم الإنجليزي"),
                new("الوحدة الأب", SecurityFieldKind.Choice, Array.Empty<string>()), new("النوع", SecurityFieldKind.Choice, Array.Empty<string>()), new("المستوى"),
                new("الشركة", SecurityFieldKind.Choice, Array.Empty<string>()), new("الفرع", SecurityFieldKind.Choice, Array.Empty<string>()),
                new("المدير", SecurityFieldKind.Choice, Array.Empty<string>()), new("بديل المدير", SecurityFieldKind.Choice, Array.Empty<string>()),
                new("البريد"), new("الهاتف"), new("تاريخ البداية", SecurityFieldKind.Date), new("تاريخ النهاية", SecurityFieldKind.Date),
                new("الحالة", SecurityFieldKind.Choice, new[] { "نشط", "موقوف" }), new("الملاحظات", SecurityFieldKind.Multiline)
            },
            Array.Empty<string>(), new[] { "جديد جذر", "جديد فرعي", "نقل عقدة", "توسيع/طي" }, SecurityWorkspaceMode.Tree);
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(247, 249, 252); Controls.Add(screenShell); Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F); Name = "UcOrganizationalUnits"; RightToLeft = RightToLeft.Yes; Size = new Size(1280, 760); ResumeLayout(false);
    }
}
