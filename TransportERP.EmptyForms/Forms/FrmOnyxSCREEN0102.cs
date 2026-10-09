using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;

/// <summary>UI reference: Ultimate Academy, General Ledger beginner course, PDF pp. 64-66.
/// Data-service and permission binding are not supplied by that reference or the existing scaffold.</summary>
public partial class UcOnyxSCREEN0102 : UserControl, IExplicitScreenLayout
{
    public UcOnyxSCREEN0102()
    {
        InitializeComponent();
        foreach (var entry in commandBar.Commands)
        {
            entry.Value.Visible = entry.Key is StandardCommand.Edit or StandardCommand.Save or StandardCommand.Cancel or StandardCommand.Print or StandardCommand.Close;
            entry.Value.Enabled = entry.Key == StandardCommand.Close;
        }
        commandBar.Commands[StandardCommand.Close].Click += (_, _) => CloseRequested?.Invoke(this, EventArgs.Empty);
        global::TransportERP.ScreenRootProperties.Attach(this);
    }
    public event EventHandler? CloseRequested;
    private void BtnView_Click(object? sender, EventArgs e)
    {
        // Do not report an empty result as a successful server query or invent financial data.
        lblOperationStatus.Text = "عرض العمليات غير متاح: لم يتم ربط مصدر بيانات الإيداع.";
    }
}
