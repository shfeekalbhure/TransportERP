using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

partial class UcAuditLog
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
                new("معايير البحث", SecurityTabKind.Details, "مرشحات سجل التدقيق العام."),
                new("النتائج", SecurityTabKind.Details, "نتائج التدقيق للقراءة فقط.",
                    Columns: new[] { "التاريخ والوقت", "المستخدم", "الدور", "الوحدة", "الشاشة", "الكيان", "معرف السجل", "العملية", "النتيجة", "IP", "معرف الطلب" }),
                new("تفاصيل العملية والقيم قبل/بعد", SecurityTabKind.Comparison, "مقارنة منظمة للقيم قبل وبعد العملية."),
                new("سجل التصدير", SecurityTabKind.Audit, "أثر التصدير والوصول للبيانات الحساسة.",
                    Columns: new[] { "التاريخ والوقت", "المستخدم", "نوع التصدير", "معايير البحث", "عدد السجلات", "الغرض", "النتيجة", "معرف الطلب" })
            },
            new SecurityFieldDefinition[]
            {
                new("معرف العملية"), new("من تاريخ", SecurityFieldKind.Date), new("إلى تاريخ", SecurityFieldKind.Date), new("المستخدم"), new("الدور"),
                new("الشركة"), new("الفرع"), new("الوحدة"), new("الشاشة"), new("الكيان"), new("معرف السجل"), new("نوع العملية"),
                new("النتيجة"), new("السبب"), new("IP"), new("الجهاز"), new("معرف الطلب")
            },
            new[] { "التاريخ والوقت", "المستخدم", "الوحدة", "الشاشة", "الكيان", "العملية", "النتيجة", "معرف الطلب" },
            new[] { "مقارنة قبل/بعد", "فتح المرجع", "حفظ مرشح" }, SecurityWorkspaceMode.ReadOnly);
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(247, 249, 252); Controls.Add(screenShell); Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F); Name = "UcAuditLog"; RightToLeft = RightToLeft.Yes; Size = new Size(1280, 760); ResumeLayout(false);
    }
}
