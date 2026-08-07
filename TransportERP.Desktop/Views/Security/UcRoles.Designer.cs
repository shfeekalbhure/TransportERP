using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

partial class UcRoles
{
    private System.ComponentModel.IContainer? components = null;
    private TransportReferenceScreenShell screenShell = null!;
    private TabControl tabDetails = null!;

    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        screenShell = new TransportReferenceScreenShell();
        tabDetails = new TabControl();
        SuspendLayout();
        SecurityDesignerSupport.ConfigureScreen(screenShell, tabDetails,
            new SecurityTabDefinition[]
            {
                new("البيانات الرئيسية", SecurityTabKind.Details, "بيانات الدور الأساسية."),
                new("الصلاحيات", SecurityTabKind.Tree, "شجرة الصلاحيات المسموح إسنادها للدور."),
                new("نطاق البيانات الافتراضي", SecurityTabKind.CheckList, "نطاق الشركات والفروع والبيانات الافتراضي."),
                new("المستخدمون المرتبطون", SecurityTabKind.Details, "المستخدمون المرتبطون بالدور.",
                    Columns: new[] { "المستخدم", "الاسم", "الشركة", "الفرع", "الوحدة التنظيمية", "الحالة", "تاريخ الإسناد", "أسند بواسطة" }),
                new("سجل العمليات", SecurityTabKind.Audit, "سجل عمليات الدور.",
                    Columns: new[] { "التاريخ والوقت", "المستخدم", "العملية", "الصلاحية / النطاق", "السبب / المرجع" })
            },
            new SecurityFieldDefinition[]
            {
                new("رمز الدور", SecurityFieldKind.RequiredText), new("الاسم العربي", SecurityFieldKind.RequiredText),
                new("الاسم الإنجليزي"), new("الوصف", SecurityFieldKind.Multiline), new("نوع الدور", SecurityFieldKind.Choice, Array.Empty<string>()),
                new("الشركة", SecurityFieldKind.Choice, Array.Empty<string>()), new("الفروع المسموحة"), new("مستوى الحساسية", SecurityFieldKind.Choice, Array.Empty<string>()),
                new("الدور الأب", SecurityFieldKind.Choice, Array.Empty<string>()), new("الحالة", SecurityFieldKind.Choice, new[] { "نشط", "موقوف", "معلق", "منتهي" }),
                new("تاريخ السريان", SecurityFieldKind.Date), new("تاريخ الانتهاء", SecurityFieldKind.Date), new("الملاحظات", SecurityFieldKind.Multiline)
            },
            new[] { "رمز الدور", "الاسم العربي", "الشركة", "نوع الدور", "الحالة", "عدد المستخدمين", "عدد الصلاحيات" },
            new[] { "نسخ دور", "إدارة الصلاحيات" }, SecurityWorkspaceMode.Edit);
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(247, 249, 252); Controls.Add(screenShell); Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F); Name = "UcRoles"; RightToLeft = RightToLeft.Yes; Size = new Size(1280, 760); ResumeLayout(false);
    }
}
