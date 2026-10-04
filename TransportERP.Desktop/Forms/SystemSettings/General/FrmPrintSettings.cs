using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

public partial class FrmPrintSettings : FrmBase
{
    public FrmPrintSettings()
    {
        InitializeComponent();
        ApplyRuntimeStyles();
        btnClose.Click += (_, _) => Close();
    }

    private void ApplyRuntimeStyles()
    {
        SettingsFormStyle.ApplyFormStyle(this, true);
        SettingsFormStyle.ApplyHeaderStyle(pnlHeader, lblTitle, lblSubtitle);
        foreach (var section in new[] { grpPrinter, grpMargins, grpBranding, grpPreview }) SettingsFormStyle.ApplySectionStyle(section);
        foreach (var label in new[] { lblPrinter, lblPaperSize, lblOrientation, lblCopies, lblTopMargin, lblBottomMargin, lblRightMargin, lblLeftMargin, lblHeaderText, lblFooterText }) SettingsFormStyle.ApplyLabelStyle(label);
        foreach (var label in new[] { lblPrinter, lblPaperSize }) SettingsFormStyle.ApplyRequiredLabelStyle(label);
        foreach (Control input in new Control[] { cboPrinter, cboPaperSize, cboOrientation, nudCopies, nudTopMargin, nudBottomMargin, nudRightMargin, nudLeftMargin, txtHeaderText, txtFooterText }) SettingsFormStyle.ApplyInputStyle(input);
        SettingsFormStyle.ApplySecondaryButtonStyle(btnChooseLogo);
        SettingsFormStyle.ApplyPrimaryButtonStyle(btnSave);
        SettingsFormStyle.ApplyNeutralButtonStyle(btnReset);
        SettingsFormStyle.ApplySecondaryButtonStyle(btnTestPrint);
        SettingsFormStyle.ApplySecondaryButtonStyle(btnClose);
        SettingsFormStyle.ApplyInfoStyle(pnlPreviewSurface);
        btnReset.Width = 140;
        lblTitle.TextAlign = ContentAlignment.MiddleLeft;
        lblSubtitle.TextAlign = ContentAlignment.MiddleLeft;
        // Four 50-pixel input rows plus the section caption/padding require the upper row.
        MinimumSize = SizeFromClientSize(new Size(1240, 800));
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScaleDimensions = new SizeF(96F, 96F);
        PerformAutoScale();
    }
}
