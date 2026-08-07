using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

partial class UcSecurityAlertLog
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
                new("معايير البحث", SecurityTabKind.Details, "مرشحات السجل الأمني."),
                new("التنبيهات والتفاصيل", SecurityTabKind.Details, "تفاصيل التنبيه المحدد للقراءة فقط."),
                new("المعالجة والتعليقات", SecurityTabKind.Details, "إجراءات المعالجة كأثر مستقل ومدقق."),
                new("سجل التصدير", SecurityTabKind.Audit, "عمليات التصدير والوصول.")
            },
            new SecurityFieldDefinition[]
            {
                new("رقم التنبيه"), new("الوقت", SecurityFieldKind.Date), new("القاعدة"), new("نوع الحدث"),
                new("الخطورة", SecurityFieldKind.Choice, new[] { "منخفض", "متوسط", "عال", "حرج" }), new("الشركة"), new("الفرع"),
                new("المستخدم"), new("IP"), new("الجهاز"), new("الحالة", SecurityFieldKind.Choice, new[] { "جديد", "مقر", "قيد المعالجة", "مغلق" }),
                new("المسؤول"), new("وقت الإقرار", SecurityFieldKind.Date), new("نتيجة المعالجة"), new("المرجع")
            },
            new[] { "رقم التنبيه", "الوقت", "القاعدة", "نوع الحدث", "الخطورة", "المستخدم", "الحالة", "المسؤول" },
            new[] { "إقرار الاستلام", "إسناد", "إغلاق بسبب", "إعادة فتح" }, SecurityWorkspaceMode.ReadOnlyWithActions);
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.FromArgb(247, 249, 252); Controls.Add(screenShell); Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 10F); Name = "UcSecurityAlertLog"; RightToLeft = RightToLeft.Yes; Size = new Size(1280, 760); ResumeLayout(false);
    }
}