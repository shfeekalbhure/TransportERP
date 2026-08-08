using TransportERP.Desktop.CoreUI.Profiles;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.Forms.Setup.General;

partial class UcGen009Currencies
{
    private System.ComponentModel.IContainer? components;
    private TransportERP.Desktop.CoreUI.Controls.TransportReferenceScreenShell screenShell = null!;
    private TransportLayoutRoleProvider profileMetadata = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        profileMetadata = new TransportLayoutRoleProvider();
        components.Add(profileMetadata);

        screenShell = GeneralSetupScreenBuilder.BuildProfiled(
            profileMetadata,
            TransportGridProfile.Display,
            "GEN-009",
            "العملات",
            new[]
            {
                new GeneralSetupScreenBuilder.TabSpec("البيانات الرئيسية", new[]
                {
                    GeneralSetupScreenBuilder.Required("رمز العملة", TransportFieldProfile.Input),
                    GeneralSetupScreenBuilder.Required("الاسم العربي", TransportFieldProfile.Input),
                    GeneralSetupScreenBuilder.Text("الاسم الإنجليزي", TransportFieldProfile.Input),
                    GeneralSetupScreenBuilder.Required("رمز ISO", TransportFieldProfile.Input),
                    GeneralSetupScreenBuilder.Text("رمز العرض", TransportFieldProfile.Input),
                    GeneralSetupScreenBuilder.Number("عدد المنازل العشرية", TransportFieldProfile.Input),
                    GeneralSetupScreenBuilder.Check("عملة أساسية", TransportFieldProfile.Input),
                    GeneralSetupScreenBuilder.ProfiledCombo("الحالة", TransportFieldProfile.Status, "نشط", "موقوف"),
                    GeneralSetupScreenBuilder.Multiline("ملاحظات", TransportFieldProfile.Input),
                    GeneralSetupScreenBuilder.Text("اسم الجزء الكسري", TransportFieldProfile.Input),
                    GeneralSetupScreenBuilder.Text("صيغة العرض", TransportFieldProfile.Input),
                    GeneralSetupScreenBuilder.Check("إظهار الرمز قبل القيمة", TransportFieldProfile.Input),
                    GeneralSetupScreenBuilder.Check("السماح بكسور العملة", TransportFieldProfile.Input),
                    GeneralSetupScreenBuilder.Text("فاصل الآلاف", TransportFieldProfile.Input),
                    GeneralSetupScreenBuilder.Text("الفاصل العشري", TransportFieldProfile.Input)
                })
            },
            "ابحث برمز أو اسم العملة",
            "الرمز", "العملة", "ISO", "المنازل العشرية", "الحالة", "آخر تعديل");

        AutoScaleMode = AutoScaleMode.Font;
        BackColor = UiTheme.WorkspaceBackground;
        Controls.Add(screenShell);
        Name = "UcGen009Currencies";
        RightToLeft = RightToLeft.Yes;
        ScreenProfile = TransportScreenProfile.MasterData;
        Size = new Size(1180, 760);
    }
}
