namespace TransportERP.Desktop.Forms.Setup.General;

partial class UcGen008VehicleTypes
{
    private System.ComponentModel.IContainer? components;
    private TransportERP.Desktop.CoreUI.Controls.TransportReferenceScreenShell screenShell = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        screenShell = GeneralSetupScreenBuilder.Build(
            "GEN-008", "أنواع المركبات",
            new[]
            {
                new GeneralSetupScreenBuilder.TabSpec("البيانات الرئيسية", new[]
                {
                    GeneralSetupScreenBuilder.Required("رمز النوع"),
                    GeneralSetupScreenBuilder.Required("الاسم العربي"),
                    GeneralSetupScreenBuilder.Text("الاسم الإنجليزي"),
                    GeneralSetupScreenBuilder.Combo("تصنيف المركبة", "حافلة", "شاحنة", "سيارة", "مقطورة", "أخرى"),
                    GeneralSetupScreenBuilder.Number("الحمولة/السعة الافتراضية"),
                    GeneralSetupScreenBuilder.Text("وحدة القياس"),
                    GeneralSetupScreenBuilder.Check("يتطلب سائقًا متخصصًا"),
                    GeneralSetupScreenBuilder.Combo("الحالة", "نشط", "موقوف"),
                    GeneralSetupScreenBuilder.Multiline("ملاحظات")
                })
            },
            "ابحث برمز أو اسم نوع المركبة", "الرمز", "الاسم العربي", "التصنيف", "السعة", "الحالة", "آخر تعديل");
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        Controls.Add(screenShell);
        Name = "UcGen008VehicleTypes";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1180, 760);
    }
}
