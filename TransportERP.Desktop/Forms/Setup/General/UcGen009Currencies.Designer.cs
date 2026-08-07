namespace TransportERP.Desktop.Forms.Setup.General;

partial class UcGen009Currencies
{
    private System.ComponentModel.IContainer? components;
    private TransportERP.Desktop.CoreUI.Controls.TransportReferenceScreenShell screenShell = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        screenShell = GeneralSetupScreenBuilder.Build(
            "GEN-009", "العملات",
            new[]
            {
                new GeneralSetupScreenBuilder.TabSpec("البيانات الرئيسية", new[]
                {
                    GeneralSetupScreenBuilder.Required("رمز العملة"), GeneralSetupScreenBuilder.Required("الاسم العربي"),
                    GeneralSetupScreenBuilder.Text("الاسم الإنجليزي"), GeneralSetupScreenBuilder.Required("رمز ISO"),
                    GeneralSetupScreenBuilder.Text("رمز العرض"), GeneralSetupScreenBuilder.Number("عدد المنازل العشرية"),
                    GeneralSetupScreenBuilder.Check("عملة أساسية"), GeneralSetupScreenBuilder.Combo("الحالة", "نشط", "موقوف"),
                    GeneralSetupScreenBuilder.Multiline("ملاحظات")
                }),
                new GeneralSetupScreenBuilder.TabSpec("خصائص العملة", new[]
                {
                    GeneralSetupScreenBuilder.Text("اسم الجزء الكسري"), GeneralSetupScreenBuilder.Text("صيغة العرض"),
                    GeneralSetupScreenBuilder.Check("إظهار الرمز قبل القيمة"), GeneralSetupScreenBuilder.Check("السماح بكسور العملة"),
                    GeneralSetupScreenBuilder.Text("فاصل الآلاف"), GeneralSetupScreenBuilder.Text("الفاصل العشري")
                }),
                new GeneralSetupScreenBuilder.TabSpec("حدود أسعار الصرف", new[]
                {
                    GeneralSetupScreenBuilder.Number("أقل سعر صرف"), GeneralSetupScreenBuilder.Number("أعلى سعر صرف"),
                    GeneralSetupScreenBuilder.Check("السماح بتعديل السعر يدويًا"), GeneralSetupScreenBuilder.Check("يتطلب صلاحية خاصة"),
                    GeneralSetupScreenBuilder.Number("نسبة السماح بالانحراف %"), GeneralSetupScreenBuilder.Combo("مرجع السعر", "سعر النظام", "سعر البنك", "سعر مخصص")
                }),
                new GeneralSetupScreenBuilder.TabSpec("ربط المستخدمين", new[]
                {
                    GeneralSetupScreenBuilder.Combo("المستخدم"), GeneralSetupScreenBuilder.Number("سعر الصرف المحدد"),
                    GeneralSetupScreenBuilder.Date("ساري من"), GeneralSetupScreenBuilder.Date("ساري إلى"),
                    GeneralSetupScreenBuilder.Check("السماح بالتعديل"), GeneralSetupScreenBuilder.Text("سبب التخصيص")
                }),
                new GeneralSetupScreenBuilder.TabSpec("سجل العمليات", Array.Empty<GeneralSetupScreenBuilder.FieldSpec>(), true)
            },
            "ابحث برمز أو اسم العملة", "الرمز", "العملة", "ISO", "أقل سعر", "أعلى سعر", "الحالة", "آخر تعديل");
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.White; Controls.Add(screenShell); Name = "UcGen009Currencies"; RightToLeft = RightToLeft.Yes; Size = new Size(1180, 760);
    }
}
