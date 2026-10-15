namespace TransportERP.Desktop;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        // These explicit bootstrap calls also compile in OutputType=Library CI builds.
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new FrmLogin());
    }
}
