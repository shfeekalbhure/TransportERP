using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

partial class UcMultiFactorAuthentication
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
                new("الطرق والسياسات", SecurityTabKind.Settings, "طرق المصادقة وسياسة الإلزام."),
                new("المستخدمون المسجلون", SecurityTabKind.Details, "حالة تسجيل المستخدمين وأجهزتهم.",
                    Columns: new[] { "المستخدم", "الطريقة", "الجهاز", "حالة التسجيل", "تاريخ التسجيل", "آخر تحقق", "النطاق", "الحالة" }),
                new("الاسترداد والرموز الاحتياطية", SecurityTabKind.Details, "حالة الاسترداد دون عرض الرموز الخام.",
                    Fields: new SecurityFieldDefinition[]
                    {
                        new("المستخدم"),
                        new("حالة الاسترداد", SecurityFieldKind.Choice, new[] { "متاح", "مستخدم", "موقوف" }),
                        new("عدد الرموز الاحتياطية المتبقية"),
                        new("تاريخ آخر توليد", SecurityFieldKind.Date),
                        new("تاريخ آخر استخدام", SecurityFieldKind.Date),
                        new("قناة الاسترداد"),
                        new("يتطلب موافقة أمنية", SecurityFieldKind.Boolean),
                        new("ملاحظات", SecurityFieldKind.Multiline)
                    },
                    Actions: new[] { "توليد رموز جديدة", "إلغاء الرموز الحالية", "بدء استرداد" }),
                new("سجل العمليات", SecurityTabKind.Audit, "التسجيل والإلغاء وإعادة الضبط والاسترداد.",
                    Columns: new[] { "التاريخ والوقت", "المستخدم", "العملية", "الطريقة / الجهاز", "النتيجة", "نفذ بواسطة", "السبب / المرجع" })
            },
            new SecurityFieldDefinition[]
            {
                new("طريقة المصادقة", SecurityFieldKind.Choice, new[] { "تطبيق مصادقة", "بريد إلكتروني", "رسالة", "مفتاح أمان" }),
                new("النطاق", SecurityFieldKind.Choice, Array.Empty<string>()), new("الإلزام", SecurityFieldKind.Choice, new[] { "إلزامي", "اختياري" }),
                new("المستخدم"), new("حالة التسجيل", SecurityFieldKind.Choice, new[] { "مسجل", "غير مسجل", "معلق" }), new("تاريخ التسجيل", SecurityFieldKind.Date),
                new("آخر تحقق", SecurityFieldKind.Date), new("الجهاز"), new("الرموز الاحتياطية المتبقية"),
                new("حالة الاسترداد", SecurityFieldKind.Choice, new[] { "متاح", "مستخدم", "موقوف" }), new("الملاحظات", SecurityFieldKind.Multiline)
            },
            new[] { "المستخدم", "الطريقة", "النطاق", "حالة التسجيل", "آخر تحقق", "الجهاز", "الرموز المتبقية" },
            new[] { "إلزام التسجيل", "إعادة ضبط MFA", "إلغاء جهاز" }, SecurityWorkspaceMode.Edit);
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(247, 249, 252); Controls.Add(screenShell); Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F); Name = "UcMultiFactorAuthentication"; RightToLeft = RightToLeft.Yes; Size = new Size(1280, 760); ResumeLayout(false);
    }
}
