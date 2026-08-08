using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.CoreUI.Profiles;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>GEN-010 — سجل سعر الصرف الفعلي؛ السياسات والحدود والصلاحيات خارج ملكية الشاشة.</summary>
public partial class UcGen010ExchangeRates : TransportScreenBase
{
    public UcGen010ExchangeRates()
    {
        InitializeComponent();
        TransportScreenProfilePolicy.Apply(this, profileMetadata);
    }

    internal TransportReferenceScreenShell ScreenShell => screenShell;
}
