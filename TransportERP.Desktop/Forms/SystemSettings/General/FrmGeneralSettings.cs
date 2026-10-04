using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

public partial class FrmGeneralSettings : FrmBase
{
    public FrmGeneralSettings()
    {
        InitializeComponent();
        ApplyRuntimeStyles();
        btnClose.Click += (_, _) => Close();
    }

    private void ApplyRuntimeStyles()
    {
        SettingsFormStyle.ApplyFormStyle(this, true);
        SettingsFormStyle.ApplyHeaderStyle(pnlHeader, lblTitle, lblSubtitle);
        SettingsFormStyle.ApplySectionStyle(grpCompanyScope);
        SettingsFormStyle.ApplySectionStyle(grpGeneralDefaults);
        SettingsFormStyle.ApplySectionStyle(grpAppearance);
        SettingsFormStyle.ApplySectionStyle(grpNotifications);
        SettingsFormStyle.ApplySectionStyle(grpPlatform);
        foreach (var label in new[] { lblCompanyGroup, lblCompany, lblBranchGroup, lblBranch, lblCompanyVisibility, lblBranchVisibility, lblCurrency, lblLanguage, lblDateFormat, lblSessionTimeout, lblTheme, lblDensity, lblPlatformMode, lblOfflineMode, lblSyncPolicy }) SettingsFormStyle.ApplyLabelStyle(label);
        foreach (var label in new[] { lblCompanyGroup, lblCompany, lblBranch }) SettingsFormStyle.ApplyRequiredLabelStyle(label);
        foreach (Control input in new Control[] { cboCompanyGroup, cboCompany, cboBranchGroup, cboBranch, cboCompanyVisibility, cboBranchVisibility, cboDefaultCurrency, cboDefaultLanguage, cboDateFormat, nudSessionTimeout, cboTheme, cboDensity, cboPlatformMode, cboOfflineMode, cboSyncPolicy }) SettingsFormStyle.ApplyInputStyle(input);
        SettingsFormStyle.ApplyPrimaryButtonStyle(btnSave);
        SettingsFormStyle.ApplyNeutralButtonStyle(btnReset);
        SettingsFormStyle.ApplySecondaryButtonStyle(btnClose);
        SettingsFormStyle.ApplySecondaryButtonStyle(btnModuleActivation);
        foreach (var button in new[] { btnNew, btnValidate, btnPublish, btnRevertScope, btnViewAudit, btnRefresh })
            SettingsFormStyle.ApplySecondaryButtonStyle(button);

        // Preserve the measured widths after the shared style applies its defaults.
        lblTitle.TextAlign = ContentAlignment.MiddleLeft;
        lblSubtitle.TextAlign = ContentAlignment.MiddleLeft;
        btnModuleActivation.Width = 168;
        btnReset.Width = 140;
        btnRevertScope.Width = 136;
        btnViewAudit.Width = 116;
        MinimumSize = SizeFromClientSize(new Size(1120, 720));
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScaleDimensions = new SizeF(96F, 96F);
        PerformAutoScale();

    }
}
