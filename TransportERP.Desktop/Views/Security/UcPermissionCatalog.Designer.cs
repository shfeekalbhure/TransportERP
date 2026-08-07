using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

partial class UcPermissionCatalog
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
                new("البيانات الرئيسية", SecurityTabKind.Details, "تعريف الصلاحية النظامية."),
                new("الارتباط بالوحدات والشاشات", SecurityTabKind.Tree, "الارتباط بالوحدات والشاشات والإجراءات."),
                new("الاعتماد والاستخدام", SecurityTabKind.Details, "الأدوار والاستخدام ومسار الاعتماد.",
                    Columns: new[] { "الدور / المجموعة", "نوع الارتباط", "الشركة", "الفرع", "عدد المستخدمين", "حالة الاعتماد", "آخر استخدام", "أسند بواسطة" }),
                new("سجل العمليات", SecurityTabKind.Audit, "سجل عمليات الكتالوج.",
                    Columns: new[] { "التاريخ والوقت", "المستخدم", "العملية", "الإصدار", "الحالة", "السبب / المرجع" })
            },
            new SecurityFieldDefinition[]
            {
                new("مفتاح الصلاحية", SecurityFieldKind.RequiredText), new("الاسم العربي", SecurityFieldKind.RequiredText), new("الاسم الإنجليزي"),
                new("الوحدة", SecurityFieldKind.Choice, Array.Empty<string>()), new("الشاشة", SecurityFieldKind.Choice, Array.Empty<string>()),
                new("نوع الصلاحية", SecurityFieldKind.Choice, Array.Empty<string>()), new("مستوى الحساسية", SecurityFieldKind.Choice, Array.Empty<string>()),
                new("الوصف", SecurityFieldKind.Multiline), new("قابلة للتفويض", SecurityFieldKind.Boolean), new("تتطلب موافقة", SecurityFieldKind.Boolean),
                new("الحالة", SecurityFieldKind.Choice, new[] { "نشط", "موقوف", "معلق", "منتهي" }), new("الإصدار")
            },
            new[] { "مفتاح الصلاحية", "الاسم العربي", "الوحدة", "الشاشة", "نوع الصلاحية", "الحساسية", "الحالة", "الإصدار" },
            new[] { "مزامنة الكتالوج", "فحص الاستخدام" }, SecurityWorkspaceMode.Edit);
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(247, 249, 252); Controls.Add(screenShell); Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F); Name = "UcPermissionCatalog"; RightToLeft = RightToLeft.Yes; Size = new Size(1280, 760); ResumeLayout(false);
    }
}
