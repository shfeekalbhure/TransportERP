using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

public partial class UcPrintSettings : UserControl, TransportERP.Desktop.CoreUI.IExplicitScreenLayout
{
    public event EventHandler? CloseRequested;

    public UcPrintSettings()
    {
        InitializeComponent();
        OnyxPhaseOneProperties.Apply(this);
        ApplyRuntimeStyles();
        btnClose.Click += (_, _) => CloseRequested?.Invoke(this, EventArgs.Empty);
        OnyxPhaseOneProperties.Apply(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    internal void ConfigureEmbeddedView()
    {
        StandardCommandBarPolicy.ExcludeEmbeddedView(this);
        pnlHeader.Visible = false;
        pnlActions.Visible = false;
        designerCommandBar.Visible = false;
        MinimumSize = Size.Empty;
        Margin = new Padding(0);
    }

    private void ApplyRuntimeStyles()
    {
        BackColor = Color.LightCyan;
        Font = UiDesignTokens.BodyFont();
        RightToLeft = RightToLeft.Yes;
        SettingsFormStyle.ApplyHeaderStyle(pnlHeader, lblTitle, lblSubtitle);
        pnlHeader.BackColor = Color.FromArgb(224, 224, 224);
        lblTitle.BackColor = Color.FromArgb(192, 192, 255);
        lblTitle.ForeColor = Color.Black;
        lblTitle.Font = new Font("Segoe UI", 16F);
        lblSubtitle.ForeColor = Color.Black;
        foreach (var section in new[] { grpPrinter, grpMargins, grpBranding, grpPreview }) SettingsFormStyle.ApplySectionStyle(section);
        foreach (var label in new[] { lblPrinter, lblPaperSize, lblOrientation, lblCopies, lblTopMargin, lblBottomMargin, lblRightMargin, lblLeftMargin, lblHeaderText, lblFooterText }) SettingsFormStyle.ApplyLabelStyle(label);
        foreach (var label in new[] { lblPrinter, lblPaperSize }) SettingsFormStyle.ApplyRequiredLabelStyle(label);
        foreach (Control input in new Control[] { cboPrinter, cboPaperSize, cboOrientation, nudCopies, nudTopMargin, nudBottomMargin, nudRightMargin, nudLeftMargin, txtHeaderText, txtFooterText }) SettingsFormStyle.ApplyInputStyle(input);
        SettingsFormStyle.ApplySecondaryButtonStyle(btnChooseLogo);
        cboPrinter.BackColor = Color.FromArgb(255, 249, 219);
        cboPaperSize.BackColor = Color.FromArgb(255, 249, 219);
        SettingsFormStyle.ApplyPrimaryButtonStyle(btnSave);
        SettingsFormStyle.ApplyNeutralButtonStyle(btnReset);
        SettingsFormStyle.ApplySecondaryButtonStyle(btnTestPrint);
        SettingsFormStyle.ApplySecondaryButtonStyle(btnClose);
        SettingsFormStyle.ApplyInfoStyle(pnlPreviewSurface);
        btnReset.Width = 140;
        lblTitle.TextAlign = ContentAlignment.MiddleLeft;
        lblSubtitle.TextAlign = ContentAlignment.MiddleLeft;
        // Four 50-pixel input rows plus the section caption/padding require the upper row.
        // Workspace minimum size is owned by SharedScreenProperties.
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScaleDimensions = new SizeF(96F, 96F);
        PerformAutoScale();
    }
}
