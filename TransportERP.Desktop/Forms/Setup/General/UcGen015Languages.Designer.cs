namespace TransportERP.Desktop.Forms.Setup.General;

partial class UcGen015Languages
{
    private System.ComponentModel.IContainer? components;
    private TransportERP.Desktop.CoreUI.Controls.TransportReferenceScreenShell screenShell = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        screenShell = GeneralSetupScreenBuilder.Build(
            "GEN-015", "اللغات",
            new[]
            {
                new GeneralSetupScreenBuilder.TabSpec("البيانات الرئيسية", new[]
                {
                    GeneralSetupScreenBuilder.Required("اسم اللغة"), GeneralSetupScreenBuilder.Required("الرمز"),
                    GeneralSetupScreenBuilder.Required("رمز الثقافة"), GeneralSetupScreenBuilder.Combo("الاتجاه", "RTL", "LTR"),
                    GeneralSetupScreenBuilder.Check("اللغة الافتراضية"), GeneralSetupScreenBuilder.Combo("الحالة", "نشط", "موقوف"),
                    GeneralSetupScreenBuilder.Text("خط العرض المقترح"), GeneralSetupScreenBuilder.Text("صيغة الأرقام"),
                    GeneralSetupScreenBuilder.Multiline("ملاحظات")
                })
            },
            "ابحث باسم اللغة أو الرمز", "اللغة", "الرمز", "الثقافة", "الاتجاه", "افتراضية", "الحالة", "آخر تعديل");
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.White; Controls.Add(screenShell); Name = "UcGen015Languages"; RightToLeft = RightToLeft.Yes; Size = new Size(1180, 760);
    }
}
