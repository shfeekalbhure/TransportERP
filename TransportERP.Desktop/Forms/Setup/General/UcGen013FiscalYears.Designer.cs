namespace TransportERP.Desktop.Forms.Setup.General;

partial class UcGen013FiscalYears
{
    private System.ComponentModel.IContainer? components;
    private TransportERP.Desktop.CoreUI.Controls.TransportReferenceScreenShell screenShell = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        screenShell = GeneralSetupScreenBuilder.Build(
            "GEN-013", "السنوات المالية",
            new[]
            {
                new GeneralSetupScreenBuilder.TabSpec("البيانات الرئيسية", new[]
                {
                    GeneralSetupScreenBuilder.Required("الشركة"), GeneralSetupScreenBuilder.Required("رمز السنة"),
                    GeneralSetupScreenBuilder.Required("اسم السنة المالية"), GeneralSetupScreenBuilder.Date("تاريخ البداية"),
                    GeneralSetupScreenBuilder.Date("تاريخ النهاية"), GeneralSetupScreenBuilder.Combo("الحالة", "مخططة", "مفتوحة", "مغلقة", "مقفلة"),
                    GeneralSetupScreenBuilder.Check("السنة الافتراضية"), GeneralSetupScreenBuilder.Multiline("ملاحظات")
                }),
                new GeneralSetupScreenBuilder.TabSpec("الفترات", new[]
                {
                    GeneralSetupScreenBuilder.Number("عدد الفترات"), GeneralSetupScreenBuilder.Combo("دورية الفترات", "شهري", "ربع سنوي", "مخصص"),
                    GeneralSetupScreenBuilder.Date("بداية الفترة الحالية"), GeneralSetupScreenBuilder.Date("نهاية الفترة الحالية"),
                    GeneralSetupScreenBuilder.Combo("حالة الفترة", "مفتوحة", "مغلقة", "مقفلة"), GeneralSetupScreenBuilder.Check("السماح بالترحيل للفترة")
                }, false, new[] { "إنشاء الفترات", "فتح الفترة", "إغلاق الفترة" }),
                new GeneralSetupScreenBuilder.TabSpec("الإغلاق", new[]
                {
                    GeneralSetupScreenBuilder.Date("تاريخ الإغلاق"), GeneralSetupScreenBuilder.Combo("نوع الإغلاق", "مؤقت", "نهائي"),
                    GeneralSetupScreenBuilder.Text("أغلق بواسطة"), GeneralSetupScreenBuilder.Multiline("سبب الإغلاق"),
                    GeneralSetupScreenBuilder.Check("منع القيود بعد الإغلاق"), GeneralSetupScreenBuilder.Check("يتطلب اعتمادًا")
                }),
                new GeneralSetupScreenBuilder.TabSpec("سجل العمليات", Array.Empty<GeneralSetupScreenBuilder.FieldSpec>(), true)
            },
            "ابحث بالشركة أو السنة أو الحالة", "الشركة", "السنة", "البداية", "النهاية", "الفترة الحالية", "الحالة", "آخر تعديل");
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.White; Controls.Add(screenShell); Name = "UcGen013FiscalYears"; RightToLeft = RightToLeft.Yes; Size = new Size(1180, 760);
    }
}
