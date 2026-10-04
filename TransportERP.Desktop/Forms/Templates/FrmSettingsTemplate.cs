using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.Templates;

[System.ComponentModel.DesignerCategory("Form")]
public partial class FrmSettingsTemplate : FrmBase
{
    public FrmSettingsTemplate()
    {
        InitializeComponent();
        lblTitle.TextAlign = ContentAlignment.MiddleLeft;
        lblSubtitle.TextAlign = ContentAlignment.MiddleLeft;
        lblTitle.Height = TextRenderer.MeasureText(lblTitle.Text, lblTitle.Font).Height + 4;
        pnlHeader.Height = lblTitle.Height + TextRenderer.MeasureText(lblSubtitle.Text, lblSubtitle.Font).Height + pnlHeader.Padding.Vertical + 8;
    }
}


