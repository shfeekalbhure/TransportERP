using TransportERP.Desktop;

internal static class PreviewProgram
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        using var form = new Form1();
        form.EnableClickVerification(Path.Combine(AppContext.BaseDirectory, "real-click-results.json"));
        Application.Run(form);
    }
}
