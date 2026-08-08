using TransportERP.Desktop.CoreUI.Profiles;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.Forms.Setup.General;

partial class UcGen010ExchangeRates
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
            "GEN-010",
            "أسعار الصرف",
            new[]
            {
                new GeneralSetupScreenBuilder.TabSpec("البيانات الرئيسية", new[]
                {
                    GeneralSetupScreenBuilder.Required("العملة", TransportFieldProfile.Lookup),
                    GeneralSetupScreenBuilder.Required("العملة الأساسية/المقابلة", TransportFieldProfile.Lookup),
                    GeneralSetupScreenBuilder.Date("تاريخ السعر", TransportFieldProfile.Input),
                    GeneralSetupScreenBuilder.ProfiledCombo("نوع السعر", TransportFieldProfile.Input, "مرجعي", "شراء", "بيع", "تحويل داخلي"),
                    GeneralSetupScreenBuilder.Number("سعر الشراء", TransportFieldProfile.Input),
                    GeneralSetupScreenBuilder.Number("سعر البيع", TransportFieldProfile.Input),
                    GeneralSetupScreenBuilder.Number("السعر المرجعي", TransportFieldProfile.Input),
                    GeneralSetupScreenBuilder.ProfiledCombo("الحالة", TransportFieldProfile.Status, "نشط", "موقوف"),
                    GeneralSetupScreenBuilder.Multiline("ملاحظات", TransportFieldProfile.Input)
                })
            },
            "ابحث بالعملة أو التاريخ أو نوع السعر",
            "العملة", "العملة المقابلة", "التاريخ", "الشراء", "البيع", "المرجعي", "الحالة");

        AutoScaleMode = AutoScaleMode.Font;
        BackColor = UiTheme.WorkspaceBackground;
        Controls.Add(screenShell);
        Name = "UcGen010ExchangeRates";
        RightToLeft = RightToLeft.Yes;
        ScreenProfile = TransportScreenProfile.MasterData;
        Size = new Size(1180, 760);
    }
}
