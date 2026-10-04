using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.Templates;

[System.ComponentModel.DesignerCategory("Form")]
public partial class FrmMasterDataTemplate : FrmBase
{
    public FrmMasterDataTemplate() {
        InitializeComponent();
        SettingsFormStyle.ApplyHeaderStyle(pnlHeader, lblTitle, lblSubtitle);
        SettingsFormStyle.ApplySectionStyle(grpDetails);
        var wasReadOnly = dgvItems.ReadOnly;
        var allowedAddRows = dgvItems.AllowUserToAddRows;
        var allowedDeleteRows = dgvItems.AllowUserToDeleteRows;
        SettingsFormStyle.ApplyGridStyle(dgvItems);
        dgvItems.ReadOnly = wasReadOnly;
        dgvItems.AllowUserToAddRows = allowedAddRows;
        dgvItems.AllowUserToDeleteRows = allowedDeleteRows;
        SettingsFormStyle.ApplyPrimaryButtonStyle(btnSave);
        SettingsFormStyle.ApplySecondaryButtonStyle(btnClose);
        lblTitle.TextAlign = ContentAlignment.MiddleLeft;
        lblSubtitle.TextAlign = ContentAlignment.MiddleLeft;
        lblTitle.Height = TextRenderer.MeasureText(lblTitle.Text, lblTitle.Font).Height + 4;
        pnlHeader.Height = lblTitle.Height + TextRenderer.MeasureText(lblSubtitle.Text, lblSubtitle.Font).Height + pnlHeader.Padding.Vertical + 8;
        pnlActions.Height = btnSave.Height + btnSave.Margin.Vertical + 16;
        pnlActions.Padding = new Padding(8);
        SettingsFormStyle.ApplyNeutralButtonStyle(btnNew);
        SettingsFormStyle.ApplySecondaryButtonStyle(btnEdit);
        SettingsFormStyle.ApplyDangerButtonStyle(btnDisable);
        SettingsFormStyle.ApplyLabelStyle(lblCode);
        SettingsFormStyle.ApplyLabelStyle(lblName);
        SettingsFormStyle.ApplyInputStyle(txtCode);
        SettingsFormStyle.ApplyInputStyle(txtName);
        var activeTextSize = TextRenderer.MeasureText(chkActive.Text, chkActive.Font);
        chkActive.MinimumSize = new Size(activeTextSize.Width + 24, activeTextSize.Height + 8);
        foreach (RowStyle row in tlpDetails.RowStyles) row.Height = Math.Max(52, chkActive.MinimumSize.Height + chkActive.Margin.Vertical);
    }
}


