using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

partial class UcPermissionDelegations
{
    private System.ComponentModel.IContainer? components = null;
    private TransportReferenceScreenShell screenShell = null!;
    private TabControl tabDetails = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        screenShell = new TransportReferenceScreenShell();
        tabDetails = new TabControl();
        SuspendLayout();

        SecurityDesignerSupport.ConfigureScreen(
            screenShell, tabDetails,
            new SecurityTabDefinition[]
            {
                new("البيانات الرئيسية", SecurityTabKind.Details, "البيانات الأساسية المعتمدة للتفويض."),
                new("نطاق الصلاحيات", SecurityTabKind.CheckList, "نطاقات الصلاحيات المحددة للتفويض."),
                new("مدة التفويض والموافقة", SecurityTabKind.Details, "مدة التفويض ومسار الموافقة.",
                    Fields: new SecurityFieldDefinition[]
                    {
                        new("تاريخ البداية", SecurityFieldKind.Date),
                        new("تاريخ النهاية", SecurityFieldKind.Date),
                        new("حالة الموافقة", SecurityFieldKind.Choice, new[] { "مسودة", "بانتظار الموافقة", "معتمد", "مرفوض", "ملغى" }),
                        new("مسار الموافقة"),
                        new("المعتمد بواسطة"),
                        new("تاريخ الاعتماد", SecurityFieldKind.Date),
                        new("سبب الرفض / الإلغاء", SecurityFieldKind.Multiline)
                    },
                    Actions: new[] { "إرسال للموافقة", "اعتماد", "رفض" }),
                new("سجل التدقيق", SecurityTabKind.Audit, "سجل تدقيق التفويض.",
                    Columns: new[] { "التاريخ والوقت", "المستخدم", "العملية", "الحالة السابقة", "الحالة الجديدة", "السبب / المرجع" })
            },
            new SecurityFieldDefinition[]
            {
                new("رقم التفويض", SecurityFieldKind.RequiredText), new("المفوِّض", SecurityFieldKind.RequiredText),
                new("المفوَّض إليه", SecurityFieldKind.RequiredText), new("الشركة", SecurityFieldKind.Choice, Array.Empty<string>()),
                new("الفرع", SecurityFieldKind.Choice, Array.Empty<string>()), new("الوحدة التنظيمية", SecurityFieldKind.Choice, Array.Empty<string>()),
                new("تاريخ البداية", SecurityFieldKind.Date), new("تاريخ النهاية", SecurityFieldKind.Date),
                new("حالة التفويض", SecurityFieldKind.Choice, new[] { "نشط", "موقوف", "معلق", "منتهي" }),
                new("سبب التفويض", SecurityFieldKind.Multiline), new("ملاحظات", SecurityFieldKind.Multiline)
            },
            new[] { "رقم التفويض", "المفوِّض", "المفوَّض إليه", "الشركة", "الفرع", "تاريخ البداية", "تاريخ النهاية", "الحالة" },
            new[] { "تحديد نطاق الصلاحيات", "طلب موافقة" }, SecurityWorkspaceMode.Edit);

        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(247, 249, 252);
        Controls.Add(screenShell);
        Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F);
        Name = "UcPermissionDelegations";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1280, 760);
        ResumeLayout(false);
    }
}
