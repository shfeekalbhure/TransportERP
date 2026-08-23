using TransportERP.Contracts.Localization;

namespace TransportERP.Desktop.CoreUI;

/// <summary>Common governed shell; concrete screens supply a validated contract and content only.</summary>
public abstract class ScreenShellForm : Form
{
    protected ScreenShellForm(ScreenIdentityContract contract, ILocalizedTextCatalog text, string? cultureName = null)
    {
        contract.Validate();
        Text = text.Get(contract.LocalizationKey, string.IsNullOrWhiteSpace(cultureName) ? "ar" : cultureName);
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        Font = SystemFonts.DefaultFont;
        StartPosition = FormStartPosition.CenterParent;
        Controls.Add(new TransportDataEntryPanel());
        Controls.Add(new ContextAlertHost());
    }

    protected static Label TechnicalValue(string value) => new() { Text = value, AutoSize = true, RightToLeft = RightToLeft.No, TextAlign = ContentAlignment.MiddleLeft };
}
