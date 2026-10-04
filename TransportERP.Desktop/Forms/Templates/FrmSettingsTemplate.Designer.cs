#nullable enable

using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.Templates;

partial class FrmSettingsTemplate
{
    private System.ComponentModel.IContainer? components;
    private Panel pnlHeader = null!; private Label lblTitle = null!; private Label lblSubtitle = null!; private TabControl tabContent = null!; private TabPage tabGeneral = null!; private GroupBox grpContent = null!; private Panel pnlContentPlaceholder = null!; private Panel pnlActions = null!; private FlowLayoutPanel flpActions = null!; private Button btnSave = null!; private Button btnReset = null!; private Button btnClose = null!; private StatusStrip statusStrip = null!; private ToolStripStatusLabel lblStatus = null!;
    protected override void Dispose(bool disposing) { if (disposing && components is not null) components.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container(); pnlHeader = new Panel(); lblSubtitle = new Label(); lblTitle = new Label(); tabContent = new TabControl(); tabGeneral = new TabPage(); grpContent = new GroupBox(); pnlContentPlaceholder = new Panel(); pnlActions = new Panel(); flpActions = new FlowLayoutPanel(); btnSave = new Button(); btnReset = new Button(); btnClose = new Button(); statusStrip = new StatusStrip(); lblStatus = new ToolStripStatusLabel();
        pnlHeader.Controls.Add(lblSubtitle); pnlHeader.Controls.Add(lblTitle); pnlHeader.Dock = DockStyle.Top; pnlHeader.Name = "pnlHeader"; lblTitle.Dock = DockStyle.Top; lblTitle.Height = 45; lblTitle.Name = "lblTitle"; lblTitle.Text = "عنوان شاشة الإعدادات"; lblSubtitle.Dock = DockStyle.Fill; lblSubtitle.Name = "lblSubtitle"; lblSubtitle.Text = "وصف مختصر ونطاق الشاشة";
        tabContent.Controls.Add(tabGeneral); tabContent.Dock = DockStyle.Fill; tabContent.Name = "tabContent"; tabContent.RightToLeftLayout = true; tabContent.TabIndex = 0; tabGeneral.Controls.Add(grpContent); tabGeneral.Name = "tabGeneral"; tabGeneral.Padding = UiDesignTokens.FormPadding; tabGeneral.Text = "إعدادات عامة"; grpContent.Controls.Add(pnlContentPlaceholder); grpContent.Dock = DockStyle.Fill; grpContent.Name = "grpContent"; grpContent.Text = "قسم إعدادات"; pnlContentPlaceholder.Dock = DockStyle.Fill; pnlContentPlaceholder.Name = "pnlContentPlaceholder"; pnlContentPlaceholder.TabIndex = 0;
        pnlActions.Controls.Add(flpActions); pnlActions.Dock = DockStyle.Bottom; pnlActions.Height = UiDesignTokens.ActionBarHeight; pnlActions.Name = "pnlActions"; flpActions.Controls.Add(btnSave); flpActions.Controls.Add(btnReset); flpActions.Controls.Add(btnClose); flpActions.Dock = DockStyle.Fill; flpActions.FlowDirection = FlowDirection.RightToLeft; flpActions.Name = "flpActions"; btnSave.Name = "btnSave"; btnSave.TabIndex = 0; btnSave.Text = "حفظ"; btnReset.Name = "btnReset"; btnReset.TabIndex = 1; btnReset.Text = "استعادة"; btnClose.Name = "btnClose"; btnClose.TabIndex = 2; btnClose.Text = "إغلاق";
        statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus }); statusStrip.Name = "statusStrip"; lblStatus.Name = "lblStatus"; lblStatus.Text = "جاهز";
        AutoScaleMode = AutoScaleMode.Font; ClientSize = new Size(1100, 720); Controls.Add(tabContent); Controls.Add(pnlActions); Controls.Add(statusStrip); Controls.Add(pnlHeader); Name = "FrmSettingsTemplate"; Text = "Settings Form Template"; SettingsFormStyle.ApplyFormStyle(this, true); SettingsFormStyle.ApplyHeaderStyle(pnlHeader, lblTitle, lblSubtitle); SettingsFormStyle.ApplySectionStyle(grpContent); SettingsFormStyle.ApplyPrimaryButtonStyle(btnSave); SettingsFormStyle.ApplyNeutralButtonStyle(btnReset); SettingsFormStyle.ApplySecondaryButtonStyle(btnClose);
    }
}
