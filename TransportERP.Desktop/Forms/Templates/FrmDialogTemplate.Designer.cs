#nullable enable

using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.Templates;

partial class FrmDialogTemplate
{
    private System.ComponentModel.IContainer? components; private Panel pnlHeader = null!; private Label lblTitle = null!; private Label lblSubtitle = null!; private Panel pnlContent = null!; private Label lblMessage = null!; private Panel pnlActions = null!; private FlowLayoutPanel flpActions = null!; private Button btnConfirm = null!; private Button btnCancel = null!; private Label lblStatus = null!;
    protected override void Dispose(bool disposing) { if (disposing && components is not null) components.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container(); pnlHeader = new Panel(); lblSubtitle = new Label(); lblTitle = new Label(); pnlContent = new Panel(); lblMessage = new Label(); pnlActions = new Panel(); flpActions = new FlowLayoutPanel(); btnConfirm = new Button(); btnCancel = new Button(); lblStatus = new Label();
        pnlHeader.Controls.Add(lblSubtitle); pnlHeader.Controls.Add(lblTitle); pnlHeader.Dock = DockStyle.Top; pnlHeader.Name = "pnlHeader"; lblTitle.Dock = DockStyle.Top; lblTitle.Height = 45; lblTitle.Name = "lblTitle"; lblTitle.Text = "عنوان مربع الحوار"; lblSubtitle.Dock = DockStyle.Fill; lblSubtitle.Name = "lblSubtitle"; lblSubtitle.Text = "وصف الأثر المطلوب تأكيده";
        pnlContent.Controls.Add(lblMessage); pnlContent.Controls.Add(lblStatus); pnlContent.Dock = DockStyle.Fill; pnlContent.Name = "pnlContent"; pnlContent.Padding = UiDesignTokens.FormPadding; lblMessage.Dock = DockStyle.Fill; lblMessage.Name = "lblMessage"; lblMessage.Text = "محتوى مربع الحوار يوضع هنا."; lblMessage.TextAlign = ContentAlignment.MiddleRight; lblStatus.Dock = DockStyle.Bottom; lblStatus.Height = 36; lblStatus.Name = "lblStatus"; lblStatus.Text = "حالة معلوماتية";
        pnlActions.Controls.Add(flpActions); pnlActions.Dock = DockStyle.Bottom; pnlActions.Height = UiDesignTokens.ActionBarHeight; pnlActions.Name = "pnlActions"; flpActions.Controls.Add(btnConfirm); flpActions.Controls.Add(btnCancel); flpActions.Dock = DockStyle.Fill; flpActions.FlowDirection = FlowDirection.RightToLeft; flpActions.Name = "flpActions"; btnConfirm.DialogResult = DialogResult.OK; btnConfirm.Name = "btnConfirm"; btnConfirm.TabIndex = 0; btnConfirm.Text = "تأكيد"; btnCancel.DialogResult = DialogResult.Cancel; btnCancel.Name = "btnCancel"; btnCancel.TabIndex = 1; btnCancel.Text = "إلغاء";
        AcceptButton = btnConfirm; AutoScaleMode = AutoScaleMode.Font; CancelButton = btnCancel; ClientSize = new Size(640, 420); Controls.Add(pnlContent); Controls.Add(pnlActions); Controls.Add(pnlHeader); FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false; MinimizeBox = false; Name = "FrmDialogTemplate"; Text = "Dialog Template"; SettingsFormStyle.ApplyFormStyle(this); MinimumSize = new Size(640, 420); SettingsFormStyle.ApplyHeaderStyle(pnlHeader, lblTitle, lblSubtitle); SettingsFormStyle.ApplyLabelStyle(lblMessage); SettingsFormStyle.ApplyInfoStyle(lblStatus); SettingsFormStyle.ApplyPrimaryButtonStyle(btnConfirm); SettingsFormStyle.ApplySecondaryButtonStyle(btnCancel);
    }
}
