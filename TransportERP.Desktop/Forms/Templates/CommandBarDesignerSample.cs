using System.ComponentModel;
namespace TransportERP.Desktop.Forms.Templates;
public partial class CommandBarDesignerSample : UserControl
{
    public CommandBarDesignerSample() => InitializeComponent();
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int AddClickCount { get; private set; }
    private void AddClicked(object? sender, EventArgs e) => AddClickCount++;
}
