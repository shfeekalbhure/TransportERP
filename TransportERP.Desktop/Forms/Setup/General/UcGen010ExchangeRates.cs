using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>GEN-010 — واجهة أسعار الصرف متعددة الجوانب دون منطق أعمال محلي.</summary>
public partial class UcGen010ExchangeRates : UserControl
{
    public UcGen010ExchangeRates() => InitializeComponent();
    internal TransportReferenceScreenShell ScreenShell => screenShell;
}
