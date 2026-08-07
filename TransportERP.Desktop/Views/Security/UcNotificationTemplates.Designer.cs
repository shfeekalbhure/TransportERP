using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

partial class UcNotificationTemplates
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
                new("البيانات الرئيسية", SecurityTabKind.Details, "تعريف القالب ونوعه وإصداره."),
                new("محتوى القالب", SecurityTabKind.Details, "عنوان الرسالة والمحتوى والنص البديل.",
                    Fields: new SecurityFieldDefinition[]
                    {
                        new("عنوان الرسالة", SecurityFieldKind.RequiredText),
                        new("المحتوى", SecurityFieldKind.Multiline),
                        new("النص البديل", SecurityFieldKind.Multiline),
                        new("تنسيق المحتوى", SecurityFieldKind.Choice, new[] { "نص", "HTML", "Markdown" }),
                        new("الحد الأقصى للطول"),
                        new("رابط الإجراء"),
                        new("ملاحظات المصمم", SecurityFieldKind.Multiline)
                    },
                    Actions: new[] { "معاينة", "اختبار القالب" }),
                new("المتغيرات", SecurityTabKind.CheckList, "المتغيرات المسموحة والإلزامية."),
                new("القنوات واللغات", SecurityTabKind.CheckList, "القنوات واللغات المدعومة."),
                new("الإصدارات والمعاينة", SecurityTabKind.Details, "الإصدارات ومعاينة بيانات الاختبار.",
                    Columns: new[] { "الإصدار", "الحالة", "اللغة", "القناة", "تاريخ الإنشاء", "تاريخ الاعتماد", "اعتمد بواسطة", "تاريخ السريان" }),
                new("سجل العمليات", SecurityTabKind.Audit, "إنشاء وتعديل واعتماد وإيقاف الإصدارات.",
                    Columns: new[] { "التاريخ والوقت", "المستخدم", "العملية", "الإصدار", "الحالة", "السبب / المرجع" })
            },
            new SecurityFieldDefinition[]
            {
                new("رمز القالب", SecurityFieldKind.RequiredText), new("الاسم", SecurityFieldKind.RequiredText), new("النوع", SecurityFieldKind.Choice, Array.Empty<string>()),
                new("اللغة", SecurityFieldKind.Choice, Array.Empty<string>()), new("القناة", SecurityFieldKind.Choice, Array.Empty<string>()), new("عنوان الرسالة"),
                new("المحتوى", SecurityFieldKind.Multiline), new("المتغيرات المسموحة"), new("المتغيرات الإلزامية"), new("النص البديل", SecurityFieldKind.Multiline),
                new("الحد الأقصى للطول"), new("الإصدار"), new("الحالة", SecurityFieldKind.Choice, new[] { "مسودة", "معتمد", "موقوف" }), new("تاريخ السريان", SecurityFieldKind.Date)
            },
            new[] { "رمز القالب", "الاسم", "النوع", "اللغة", "القناة", "الإصدار", "الحالة", "تاريخ السريان" },
            new[] { "نسخ إصدار", "معاينة", "اعتماد" }, SecurityWorkspaceMode.Edit);
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(247, 249, 252); Controls.Add(screenShell); Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F); Name = "UcNotificationTemplates"; RightToLeft = RightToLeft.Yes; Size = new Size(1280, 760); ResumeLayout(false);
    }
}
