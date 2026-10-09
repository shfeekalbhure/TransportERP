using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.Setup.Security;

/// <summary>Screen 02.03.06. Designer-editable; password persistence is not connected.</summary>
public sealed partial class UcChangePassword : UserControl, IWorkspaceCloseGuard
{
    public UcChangePassword()
    {
        InitializeComponent();
        SharedScreenProperties.Apply(this);
        lblTitle.MinimumSize = TextRenderer.MeasureText(lblTitle.Text, lblTitle.Font);
        SharedScreenProperties.ApplyContent(this);
        btnChangePassword.Click += async (_, _) => await ChangePasswordAsync();
        ApplyPasswordRequirementColors();
        Load += (_, _) => ApplyPasswordRequirementColors();
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }

    private Func<string, string, System.Threading.Tasks.Task<FoundationResult>>? changePassword;
    private void ApplyPasswordRequirementColors()
    {
        foreach (Control editor in new Control[] { CurrentPassword, NewPassword, ConfirmPassword })
            global::TransportERP.RequiredFieldAppearance.Apply(editor, true);
    }
    public bool IsBusy { get; private set; }
    public bool HasUnsavedChanges => CurrentPassword.TextLength > 0 || NewPassword.TextLength > 0 || ConfirmPassword.TextLength > 0;
    public void BindPasswordChange(Func<string, string, System.Threading.Tasks.Task<FoundationResult>> operation)
    {
        if (IsBusy) throw new InvalidOperationException("An operation is active.");
        changePassword = operation ?? throw new ArgumentNullException(nameof(operation));
        btnChangePassword.Enabled = true;
    }
    public bool ConfirmLeave()
    {
        if (IsBusy) { MessageBox.Show(this, "انتظر اكتمال العملية الحالية."); return false; }
        return !HasUnsavedChanges || MessageBox.Show(this, "تجاهل بيانات كلمة السر المدخلة؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
    }
    private async System.Threading.Tasks.Task ChangePasswordAsync()
    {
        if (IsBusy || changePassword == null) return;
        if (CurrentPassword.TextLength == 0 || NewPassword.TextLength == 0 || ConfirmPassword.TextLength == 0)
        { MessageBox.Show(this, "أكمل حقول كلمة السر الثلاثة."); return; }
        if (NewPassword.Text != ConfirmPassword.Text)
        { ConfirmPassword.Focus(); MessageBox.Show(this, "تأكيد كلمة السر غير مطابق."); return; }
        IsBusy = true; Enabled = false;
        try
        {
            var result = await changePassword(CurrentPassword.Text, NewPassword.Text);
            if (result.Success && result.Persisted) { CurrentPassword.Clear(); NewPassword.Clear(); ConfirmPassword.Clear(); }
            MessageBox.Show(this, result.Message);
        }
        catch { MessageBox.Show(this, "تعذر تغيير كلمة السر. تحقق من الاتصال وحاول مجددًا."); }
        finally { IsBusy = false; if (!IsDisposed) Enabled = true; }
    }

    public event EventHandler? CloseRequested;
    internal Button CloseButton => btnClose;

    private void BtnClose_Click(object? sender, EventArgs e)
        => CloseRequested?.Invoke(this, EventArgs.Empty);
}
