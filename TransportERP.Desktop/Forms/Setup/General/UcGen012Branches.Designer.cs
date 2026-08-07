namespace TransportERP.Desktop.Forms.Setup.General;

partial class UcGen012Branches
{
    private System.ComponentModel.IContainer? components;
    private TransportERP.Desktop.CoreUI.Controls.TransportReferenceScreenShell screenShell = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        screenShell = GeneralSetupScreenBuilder.Build(
            "GEN-012", "الفروع",
            new[]
            {
                new GeneralSetupScreenBuilder.TabSpec("البيانات الرئيسية", new[]
                {
                    GeneralSetupScreenBuilder.Required("الشركة"), GeneralSetupScreenBuilder.Required("رمز الفرع"),
                    GeneralSetupScreenBuilder.Required("اسم الفرع العربي"), GeneralSetupScreenBuilder.Text("اسم الفرع الإنجليزي"),
                    GeneralSetupScreenBuilder.Combo("نوع الفرع", "رئيسي", "تشغيلي", "مكتب", "محطة", "مستودع"), GeneralSetupScreenBuilder.Combo("الحالة", "نشط", "موقوف"),
                    GeneralSetupScreenBuilder.Multiline("ملاحظات")
                }),
                new GeneralSetupScreenBuilder.TabSpec("العنوان والاتصال", new[]
                {
                    GeneralSetupScreenBuilder.Combo("الدولة"), GeneralSetupScreenBuilder.Combo("المحافظة"),
                    GeneralSetupScreenBuilder.Combo("المدينة"), GeneralSetupScreenBuilder.Text("العنوان التفصيلي"),
                    GeneralSetupScreenBuilder.Text("الهاتف"), GeneralSetupScreenBuilder.Text("البريد الإلكتروني")
                }),
                new GeneralSetupScreenBuilder.TabSpec("الإدارة والمسؤولون", new[]
                {
                    GeneralSetupScreenBuilder.Combo("مدير الفرع"), GeneralSetupScreenBuilder.Text("هاتف المدير"),
                    GeneralSetupScreenBuilder.Combo("المسؤول المالي"), GeneralSetupScreenBuilder.Combo("مسؤول التشغيل"),
                    GeneralSetupScreenBuilder.Text("مركز المسؤولية"), GeneralSetupScreenBuilder.Text("رمز التكلفة")
                }),
                new GeneralSetupScreenBuilder.TabSpec("الإعدادات الافتراضية", new[]
                {
                    GeneralSetupScreenBuilder.Combo("العملة الافتراضية"), GeneralSetupScreenBuilder.Combo("الصندوق الافتراضي"),
                    GeneralSetupScreenBuilder.Combo("الحساب البنكي الافتراضي"), GeneralSetupScreenBuilder.Combo("المستودع الافتراضي"),
                    GeneralSetupScreenBuilder.Combo("لغة الطباعة"), GeneralSetupScreenBuilder.Check("السماح بالعمل دون اتصال")
                }),
                new GeneralSetupScreenBuilder.TabSpec("الإعدادات التشغيلية", new[]
                {
                    GeneralSetupScreenBuilder.Check("فرع مبيعات"), GeneralSetupScreenBuilder.Check("فرع شحن"),
                    GeneralSetupScreenBuilder.Check("فرع نقل ركاب"), GeneralSetupScreenBuilder.Check("فرع صيانة"),
                    GeneralSetupScreenBuilder.Text("رمز التشغيل"), GeneralSetupScreenBuilder.Text("منطقة الخدمة")
                }),
                new GeneralSetupScreenBuilder.TabSpec("سجل العمليات", Array.Empty<GeneralSetupScreenBuilder.FieldSpec>(), true)
            },
            "ابحث بالشركة أو رمز الفرع أو الاسم", "الشركة", "رمز الفرع", "الفرع", "المدينة", "المدير", "الحالة", "آخر تعديل");
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.White; Controls.Add(screenShell); Name = "UcGen012Branches"; RightToLeft = RightToLeft.Yes; Size = new Size(1180, 760);
    }
}
