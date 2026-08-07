namespace TransportERP.Desktop.Forms.Setup.General;

partial class UcGen010ExchangeRates
{
    private System.ComponentModel.IContainer? components;
    private TransportERP.Desktop.CoreUI.Controls.TransportReferenceScreenShell screenShell = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        screenShell = GeneralSetupScreenBuilder.Build(
            "GEN-010", "أسعار الصرف",
            new[]
            {
                new GeneralSetupScreenBuilder.TabSpec("البيانات الرئيسية", new[]
                {
                    GeneralSetupScreenBuilder.Required("العملة"), GeneralSetupScreenBuilder.Required("العملة الأساسية/المقابلة"),
                    GeneralSetupScreenBuilder.Date("تاريخ السعر"), GeneralSetupScreenBuilder.Combo("نوع السعر", "مرجعي", "شراء", "بيع", "تحويل داخلي"),
                    GeneralSetupScreenBuilder.Number("سعر الشراء"), GeneralSetupScreenBuilder.Number("سعر البيع"),
                    GeneralSetupScreenBuilder.Number("السعر المرجعي"), GeneralSetupScreenBuilder.Combo("الحالة", "نشط", "موقوف"),
                    GeneralSetupScreenBuilder.Multiline("ملاحظات")
                }),
                new GeneralSetupScreenBuilder.TabSpec("الحدود والصلاحيات", new[]
                {
                    GeneralSetupScreenBuilder.Number("أقل سعر مسموح"), GeneralSetupScreenBuilder.Number("أعلى سعر مسموح"),
                    GeneralSetupScreenBuilder.Check("السماح بالتعديل اليدوي"), GeneralSetupScreenBuilder.Check("يتطلب اعتمادًا"),
                    GeneralSetupScreenBuilder.Combo("نطاق الصلاحية", "عام", "شركة", "فرع", "مستخدم"), GeneralSetupScreenBuilder.Text("سبب تجاوز الحدود")
                }),
                new GeneralSetupScreenBuilder.TabSpec("ربط المستخدمين والجهات", new[]
                {
                    GeneralSetupScreenBuilder.Combo("نوع الجهة", "مستخدم", "مجموعة مستخدمين", "فرع"), GeneralSetupScreenBuilder.Combo("الجهة"),
                    GeneralSetupScreenBuilder.Number("السعر المخصص"), GeneralSetupScreenBuilder.Date("ساري من"),
                    GeneralSetupScreenBuilder.Date("ساري إلى"), GeneralSetupScreenBuilder.Check("قفل السعر للمستخدم")
                }),
                new GeneralSetupScreenBuilder.TabSpec("سجل التغييرات", Array.Empty<GeneralSetupScreenBuilder.FieldSpec>(), true)
            },
            "ابحث بالعملة أو التاريخ أو نوع السعر", "العملة", "العملة المقابلة", "التاريخ", "الشراء", "البيع", "المرجعي", "الحالة");
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.White; Controls.Add(screenShell); Name = "UcGen010ExchangeRates"; RightToLeft = RightToLeft.Yes; Size = new Size(1180, 760);
    }
}
