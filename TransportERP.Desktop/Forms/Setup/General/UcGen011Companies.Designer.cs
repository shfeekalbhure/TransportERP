namespace TransportERP.Desktop.Forms.Setup.General;

partial class UcGen011Companies
{
    private System.ComponentModel.IContainer? components;
    private TransportERP.Desktop.CoreUI.Controls.TransportReferenceScreenShell screenShell = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        screenShell = GeneralSetupScreenBuilder.Build(
            "GEN-011", "الشركات",
            new[]
            {
                new GeneralSetupScreenBuilder.TabSpec("البيانات الرئيسية", new[]
                {
                    GeneralSetupScreenBuilder.Required("رمز الشركة"), GeneralSetupScreenBuilder.Required("الاسم العربي"),
                    GeneralSetupScreenBuilder.Text("الاسم الإنجليزي"), GeneralSetupScreenBuilder.Combo("نوع الشركة", "نقل", "لوجستيات", "خدمات", "قابضة", "أخرى"),
                    GeneralSetupScreenBuilder.Combo("العملة الأساسية"), GeneralSetupScreenBuilder.Combo("الدولة"),
                    GeneralSetupScreenBuilder.Combo("الحالة", "نشط", "موقوف"), GeneralSetupScreenBuilder.Multiline("ملاحظات")
                }),
                new GeneralSetupScreenBuilder.TabSpec("بيانات الاتصال", new[]
                {
                    GeneralSetupScreenBuilder.Text("الهاتف"), GeneralSetupScreenBuilder.Text("الجوال"),
                    GeneralSetupScreenBuilder.Text("البريد الإلكتروني"), GeneralSetupScreenBuilder.Text("الموقع الإلكتروني"),
                    GeneralSetupScreenBuilder.Text("العنوان"), GeneralSetupScreenBuilder.Text("المدينة")
                }),
                new GeneralSetupScreenBuilder.TabSpec("البيانات النظامية والضريبية", new[]
                {
                    GeneralSetupScreenBuilder.Text("السجل التجاري"), GeneralSetupScreenBuilder.Text("الرقم الضريبي"),
                    GeneralSetupScreenBuilder.Text("رقم الترخيص"), GeneralSetupScreenBuilder.Date("تاريخ انتهاء الترخيص"),
                    GeneralSetupScreenBuilder.Text("الرقم الموحد"), GeneralSetupScreenBuilder.Text("جهة الترخيص")
                }),
                new GeneralSetupScreenBuilder.TabSpec("الشعار والمرفقات", new[]
                {
                    GeneralSetupScreenBuilder.Picture("الشعار الحالي"), GeneralSetupScreenBuilder.Text("وصف المرفق")
                }, false, new[] { "إضافة شعار", "حذف شعار", "إضافة مرفق" }),
                new GeneralSetupScreenBuilder.TabSpec("الإعدادات", new[]
                {
                    GeneralSetupScreenBuilder.Combo("اللغة الافتراضية"), GeneralSetupScreenBuilder.Combo("المنطقة الزمنية"),
                    GeneralSetupScreenBuilder.Check("السماح بتعدد الفروع"), GeneralSetupScreenBuilder.Check("السماح بتعدد العملات"),
                    GeneralSetupScreenBuilder.Text("صيغة التاريخ"), GeneralSetupScreenBuilder.Text("صيغة الأرقام")
                }),
                new GeneralSetupScreenBuilder.TabSpec("سجل العمليات", Array.Empty<GeneralSetupScreenBuilder.FieldSpec>(), true)
            },
            "ابحث برمز الشركة أو الاسم أو السجل التجاري", "الرمز", "الشركة", "السجل التجاري", "العملة", "الدولة", "الحالة", "آخر تعديل");
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.White; Controls.Add(screenShell); Name = "UcGen011Companies"; RightToLeft = RightToLeft.Yes; Size = new Size(1180, 760);
    }
}
