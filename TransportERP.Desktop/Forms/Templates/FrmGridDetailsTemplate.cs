using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.Templates;

[System.ComponentModel.DesignerCategory("Form")]
public partial class FrmGridDetailsTemplate : FrmBase
{
    public FrmGridDetailsTemplate() {
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
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }
}


