using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

public partial class UcGeneralSettings : UserControl, TransportERP.Desktop.CoreUI.IFoundationScreen, TransportERP.Desktop.CoreUI.IExplicitScreenLayout
{
    public TransportERP.Desktop.CoreUI.FoundationUiSession Foundation { get; }
    public bool HasUnsavedChanges => Foundation.HasUnsavedChanges;
    public bool IsBusy => Foundation.IsBusy;
    public bool ConfirmLeave() => Foundation.ConfirmLeave();

    public event EventHandler? CloseRequested;

    public UcGeneralSettings()
    {
        InitializeComponent();
        OnyxPhaseOneProperties.Apply(this);
        ApplyRuntimeStyles();
        btnClose.Click += (_, _) => CloseRequested?.Invoke(this, EventArgs.Empty);
        OnyxPhaseOneProperties.Apply(this);
        ConfigureUnavailableActions();
        Foundation = new TransportERP.Desktop.CoreUI.FoundationUiSession(this);
        Foundation.RequireFields(new[] { "btnSave", "btnPublish", "btnValidate" }, cboCompanyGroup.Name, cboCompany.Name, cboBranch.Name);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    private void ConfigureUnavailableActions()
    {
        const string reason = "غير متاح حاليًا: لم يُنفَّذ هذا الإجراء ولم يُربط بخدمة تشغيلية.";
        components ??= new System.ComponentModel.Container();
        var availabilityTip = new ToolTip(components) { ShowAlways = true };
        lblSubtitle.Text += " — واجهة تصميم؛ الحفظ والبيانات غير مرتبطة بعد";
        availabilityTip.SetToolTip(btnClose, "إغلاق الشاشة");
        foreach (var button in new[] { btnSave, btnReset, btnModuleActivation, btnNew, btnValidate, btnPublish, btnRevertScope, btnViewAudit, btnRefresh })
        {
            button.Enabled = false;
            button.AccessibleDescription = reason;
            availabilityTip.SetToolTip(button, reason);
            // Disabled buttons do not reliably receive hover messages. Their host
            // also explains the disabled state without changing the layout.
            if (button.Parent is Control host)
                availabilityTip.SetToolTip(host, reason);
        }
    }

    private void ApplyRuntimeStyles()
    {
        BackColor = Color.LightCyan;
        Font = new Font("Segoe UI", 10F);
        RightToLeft = RightToLeft.Yes;
        SettingsFormStyle.ApplyHeaderStyle(pnlHeader, lblTitle, lblSubtitle);
        pnlHeader.BackColor = Color.FromArgb(224, 224, 224);
        lblTitle.BackColor = Color.FromArgb(192, 192, 255);
        lblTitle.ForeColor = Color.Black;
        lblTitle.Font = new Font("Segoe UI", 16F);
        lblSubtitle.ForeColor = Color.Black;
        SettingsFormStyle.ApplySectionStyle(grpCompanyScope);
        SettingsFormStyle.ApplySectionStyle(grpGeneralDefaults);
        SettingsFormStyle.ApplySectionStyle(grpAppearance);
        SettingsFormStyle.ApplySectionStyle(grpNotifications);
        SettingsFormStyle.ApplySectionStyle(grpPlatform);
        foreach (var label in new[] { lblCompanyGroup, lblCompany, lblBranchGroup, lblBranch, lblCompanyVisibility, lblBranchVisibility, lblCurrency, lblLanguage, lblDateFormat, lblSessionTimeout, lblTheme, lblDensity, lblPlatformMode, lblOfflineMode, lblSyncPolicy }) SettingsFormStyle.ApplyLabelStyle(label);
        foreach (var label in new[] { lblCompanyGroup, lblCompany, lblBranch }) SettingsFormStyle.ApplyRequiredLabelStyle(label);
        foreach (Control input in new Control[] { cboCompanyGroup, cboCompany, cboBranchGroup, cboBranch, cboCompanyVisibility, cboBranchVisibility, cboDefaultCurrency, cboDefaultLanguage, cboDateFormat, nudSessionTimeout, cboTheme, cboDensity, cboPlatformMode, cboOfflineMode, cboSyncPolicy }) SettingsFormStyle.ApplyInputStyle(input);
        cboCompanyGroup.BackColor = Color.FromArgb(255, 249, 219);
        cboCompany.BackColor = Color.FromArgb(255, 249, 219);
        cboBranch.BackColor = Color.FromArgb(255, 249, 219);
        SettingsFormStyle.ApplyPrimaryButtonStyle(btnSave);
        SettingsFormStyle.ApplyNeutralButtonStyle(btnReset);
        SettingsFormStyle.ApplySecondaryButtonStyle(btnClose);
        SettingsFormStyle.ApplySecondaryButtonStyle(btnModuleActivation);
        foreach (var button in new[] { btnNew, btnValidate, btnPublish, btnRevertScope, btnViewAudit, btnRefresh })
            SettingsFormStyle.ApplySecondaryButtonStyle(button);

        // Preserve the measured widths after the shared style applies its defaults.
        lblTitle.TextAlign = ContentAlignment.MiddleRight;
        lblSubtitle.TextAlign = ContentAlignment.MiddleRight;
        btnModuleActivation.Width = 168;
        btnReset.Width = 140;
        btnRevertScope.Width = 136;
        btnViewAudit.Width = 116;
        // Workspace minimum size is owned by SharedScreenProperties.
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScaleDimensions = new SizeF(96F, 96F);
        foreach (Control input in new Control[] { cboCompanyGroup, cboCompany, cboBranchGroup, cboBranch, cboCompanyVisibility, cboBranchVisibility, cboDefaultCurrency, cboDefaultLanguage, cboDateFormat, nudSessionTimeout, cboTheme, cboDensity, cboPlatformMode, cboOfflineMode, cboSyncPolicy })
        {
            input.Margin = new Padding(3);
            input.Font = new Font("Segoe UI", 11F);
            input.Dock = DockStyle.Fill;
        }
        foreach (var button in new[] { btnSave, btnReset, btnClose, btnModuleActivation, btnNew, btnValidate, btnPublish, btnRevertScope, btnViewAudit, btnRefresh })
        {
            button.Dock = DockStyle.None;
            button.Margin = new Padding(4);
            button.BackColor = Color.FromArgb(224, 224, 224);
            button.ForeColor = Color.FromArgb(16, 24, 40);
            button.Font = new Font("Microsoft Sans Serif", 10F);
            button.AutoSize = true;
            button.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button.MinimumSize = new Size(70, 30);
            button.UseVisualStyleBackColor = false;
        }
        PerformAutoScale();

    }
}
