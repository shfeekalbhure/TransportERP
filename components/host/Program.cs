using TransportERP.Desktop.Forms.Setup.Security;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();
        using var host = new FrmUsersPermissions();
        // Values only: no language/layout switch, added controls, or synthetic event counting.
        if (args.Contains("--values-ar"))
            host.SetAuditFixture(new("قيمة اختبار عربية طويلة لمدخل السجل", "قيمة اختبار عربية طويلة لمعدل السجل", "2000-01-01 00:00", "2000-01-02 00:00", "TEST-CREATION-DEVICE", "TEST-MODIFICATION-DEVICE", "123456", "654321"));
        else if (args.Contains("--values-en"))
            host.SetAuditFixture(new("Long English creator test value", "Long English modifier test value", "2000-01-01 00:00", "2000-01-02 00:00", "TEST-CREATION-DEVICE", "TEST-MODIFICATION-DEVICE", "123456", "654321"));
        Application.Run(host);
    }
}
