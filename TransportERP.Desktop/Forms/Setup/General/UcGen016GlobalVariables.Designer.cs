namespace TransportERP.Desktop.Forms.Setup.General;

partial class UcGen016GlobalVariables
{
    private System.ComponentModel.IContainer? components;
    private TransportERP.Desktop.CoreUI.Controls.TransportReferenceScreenShell screenShell = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        screenShell = GeneralSetupScreenBuilder.Build(
            "GEN-016", "المتغيرات العامة",
            new[]
            {
                new GeneralSetupScreenBuilder.TabSpec("البيانات الرئيسية", new[]
                {
                    GeneralSetupScreenBuilder.Required("رمز المتغير"), GeneralSetupScreenBuilder.Required("اسم المتغير"),
                    GeneralSetupScreenBuilder.Combo("نوع القيمة", "نص", "رقم", "منطقي", "تاريخ", "قائمة"), GeneralSetupScreenBuilder.Text("القيمة"),
                    GeneralSetupScreenBuilder.Combo("النطاق", "نظام", "شركة", "فرع", "مستخدم"), GeneralSetupScreenBuilder.Combo("الحالة", "نشط", "موقوف"),
                    GeneralSetupScreenBuilder.Multiline("الوصف/الملاحظات")
                }),
                new GeneralSetupScreenBuilder.TabSpec("إعدادات النظام", new[]
                {
                    GeneralSetupScreenBuilder.Text("اسم النظام"), GeneralSetupScreenBuilder.Combo("اللغة الافتراضية"),
                    GeneralSetupScreenBuilder.Combo("المنطقة الزمنية"), GeneralSetupScreenBuilder.Text("صيغة التاريخ"),
                    GeneralSetupScreenBuilder.Text("صيغة الأرقام"), GeneralSetupScreenBuilder.Check("السماح بالعمل دون اتصال"),
                    GeneralSetupScreenBuilder.Number("مدة مزامنة البيانات بالدقائق"), GeneralSetupScreenBuilder.Text("عنوان خدمة API")
                }),
                new GeneralSetupScreenBuilder.TabSpec("سياسات البيانات", new[]
                {
                    GeneralSetupScreenBuilder.Check("فرض نطاق الشركة"), GeneralSetupScreenBuilder.Check("فرض نطاق الفرع"),
                    GeneralSetupScreenBuilder.Check("فرض السنة المالية"), GeneralSetupScreenBuilder.Check("منع حذف السجل المرتبط"),
                    GeneralSetupScreenBuilder.Number("مدة الاحتفاظ بالسجلات بالأيام"), GeneralSetupScreenBuilder.Check("تفعيل سجل التدقيق"),
                    GeneralSetupScreenBuilder.Check("تفعيل التعديل المتزامن"), GeneralSetupScreenBuilder.Text("سياسة التعارض")
                }),
                new GeneralSetupScreenBuilder.TabSpec("الأمان والجلسات", new[]
                {
                    GeneralSetupScreenBuilder.Number("مدة الجلسة بالدقائق"), GeneralSetupScreenBuilder.Number("عدد محاولات الدخول"),
                    GeneralSetupScreenBuilder.Check("فرض المصادقة الثنائية"), GeneralSetupScreenBuilder.Check("السماح بالأجهزة الموثوقة"),
                    GeneralSetupScreenBuilder.Number("مدة قفل الحساب بالدقائق"), GeneralSetupScreenBuilder.Check("إخفاء البيانات الحساسة"),
                    GeneralSetupScreenBuilder.Check("تسجيل تغييرات الإعدادات"), GeneralSetupScreenBuilder.Text("سياسة كلمة المرور")
                }),
                new GeneralSetupScreenBuilder.TabSpec("الطباعة والاتصال", new[]
                {
                    GeneralSetupScreenBuilder.Combo("طابعة التقارير الافتراضية"), GeneralSetupScreenBuilder.Combo("طابعة التذاكر الافتراضية"),
                    GeneralSetupScreenBuilder.Check("إظهار شعار الشركة"), GeneralSetupScreenBuilder.Check("إظهار بيانات الفرع"),
                    GeneralSetupScreenBuilder.Number("مهلة الاتصال بالثواني"), GeneralSetupScreenBuilder.Number("عدد محاولات إعادة الاتصال"),
                    GeneralSetupScreenBuilder.Check("العمل بوضع اتصال منخفض"), GeneralSetupScreenBuilder.Text("خادم التقارير")
                }),
                new GeneralSetupScreenBuilder.TabSpec("سجل العمليات", Array.Empty<GeneralSetupScreenBuilder.FieldSpec>(), true)
            },
            "ابحث برمز المتغير أو الاسم أو النطاق", "الرمز", "المتغير", "النوع", "القيمة", "النطاق", "الحالة", "آخر تعديل");
        AutoScaleMode = AutoScaleMode.Font; BackColor = Color.White; Controls.Add(screenShell); Name = "UcGen016GlobalVariables"; RightToLeft = RightToLeft.Yes; Size = new Size(1180, 760);
    }
}
