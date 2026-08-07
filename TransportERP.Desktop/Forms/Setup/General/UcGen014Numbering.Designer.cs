namespace TransportERP.Desktop.Forms.Setup.General;

partial class UcGen014Numbering
{
    private System.ComponentModel.IContainer? components;
    private TransportERP.Desktop.CoreUI.Controls.TransportReferenceScreenShell screenShell = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        screenShell = GeneralSetupScreenBuilder.Build(
            "GEN-014", "الترقيم العام",
            new[]
            {
                new GeneralSetupScreenBuilder.TabSpec("الترقيم العام", new[]
                {
                    GeneralSetupScreenBuilder.Required("نوع المستند"), GeneralSetupScreenBuilder.Combo("الوحدة"),
                    GeneralSetupScreenBuilder.Text("البادئة"), GeneralSetupScreenBuilder.Number("عدد الخانات"),
                    GeneralSetupScreenBuilder.Number("رقم البداية"), GeneralSetupScreenBuilder.Check("السماح بالتعديل اليدوي"),
                    GeneralSetupScreenBuilder.Combo("الحالة", "نشط", "موقوف"), GeneralSetupScreenBuilder.Multiline("ملاحظات")
                }),
                new GeneralSetupScreenBuilder.TabSpec("النطاق", new[]
                {
                    GeneralSetupScreenBuilder.Combo("الشركة"), GeneralSetupScreenBuilder.Combo("الفرع"),
                    GeneralSetupScreenBuilder.Combo("السنة المالية"), GeneralSetupScreenBuilder.Combo("نطاق العداد", "عام", "حسب الشركة", "حسب الفرع", "حسب السنة"),
                    GeneralSetupScreenBuilder.Check("عداد مستقل لكل فرع"), GeneralSetupScreenBuilder.Check("عداد مستقل لكل سنة")
                }),
                new GeneralSetupScreenBuilder.TabSpec("قواعد التصفير", new[]
                {
                    GeneralSetupScreenBuilder.Combo("قاعدة التصفير", "لا يعاد", "سنوي", "شهري", "عند السنة المالية"), GeneralSetupScreenBuilder.Date("تاريخ التصفير القادم"),
                    GeneralSetupScreenBuilder.Check("الاحتفاظ بالأرقام الملغاة"), GeneralSetupScreenBuilder.Check("منع إعادة استخدام الرقم"),
                    GeneralSetupScreenBuilder.Check("قيد فريد بقاعدة البيانات"), GeneralSetupScreenBuilder.Text("سياسة الحجز الذري")
                }),
                new GeneralSetupScreenBuilder.TabSpec("عدادات الترقيم", new[]
                {
                    GeneralSetupScreenBuilder.Text("آخر رقم محجوز"), GeneralSetupScreenBuilder.Text("آخر رقم مستخدم"),
                    GeneralSetupScreenBuilder.Text("الرقم التالي المتوقع"), GeneralSetupScreenBuilder.Date("آخر حجز"),
                    GeneralSetupScreenBuilder.Text("آخر مستخدم للحجز"), GeneralSetupScreenBuilder.Check("العداد مقفل مؤقتًا")
                }, false, new[] { "حجز رقم", "تحديث العداد" }),
                new GeneralSetupScreenBuilder.TabSpec("استثناءات الترقيم", new[]
                {
                    GeneralSetupScreenBuilder.Combo("نوع الاستثناء", "تعديل آخر رقم", "تجاوز بادئة", "نطاق خاص"), GeneralSetupScreenBuilder.Text("القيمة المطلوبة"),
                    GeneralSetupScreenBuilder.Multiline("سبب الاستثناء"), GeneralSetupScreenBuilder.Text("طالب الاستثناء"),
                    GeneralSetupScreenBuilder.Text("معتمد بواسطة"), GeneralSetupScreenBuilder.Combo("الحالة", "مسودة", "بانتظار الاعتماد", "معتمد", "مرفوض")
                }, false, new[] { "طلب استثناء", "اعتماد الاستثناء", "رفض الاستثناء" }),
                new GeneralSetupScreenBuilder.TabSpec("سجل العمليات", Array.Empty<GeneralSetupScreenBuilder.FieldSpec>(), true)
            },
            "ابحث بنوع المستند أو الشركة أو الفرع", "المستند", "الشركة", "الفرع", "السنة", "البادئة", "آخر رقم", "الحالة");
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.White; Controls.Add(screenShell); Name = "UcGen014Numbering"; RightToLeft = RightToLeft.Yes; Size = new Size(1180, 760);
    }
}
