#nullable enable
using TransportERP.Desktop.SharedUI.Commands;

namespace TransportERP.Desktop;

/// <summary>Non-layout adapter for the saved six-button Designer draft. No services or shortcuts.</summary>
public partial class UsersCommandBar
{
    private ToolbarCommandBindings<UsersToolbarCommand>? commandBindings;

    /// <summary>Replace or detach the screen owner's routing without adding Click subscriptions.</summary>
    public void SetCommandBindings(ToolbarCommandBindings<UsersToolbarCommand>? bindings)
    {
        ObjectDisposedException.ThrowIf(IsDisposed || Disposing, this);
        commandBindings = bindings;
    }

    private void InitializeCommandRouting()
    {
        btnNew.Click += OnCommandButtonClick;
        btnEdit.Click += OnCommandButtonClick;
        btnSave.Click += OnCommandButtonClick;
        btnDisable.Click += OnCommandButtonClick;
        btnResetPassword.Click += OnCommandButtonClick;
        btnClose.Click += OnCommandButtonClick;
        Disposed += (_, _) => commandBindings = null;
    }

    private void OnCommandButtonClick(object? sender, EventArgs e)
    {
        if (ReferenceEquals(sender, btnNew)) TryInvokeCommand(UsersToolbarCommand.NewUser);
        else if (ReferenceEquals(sender, btnEdit)) TryInvokeCommand(UsersToolbarCommand.EditUser);
        else if (ReferenceEquals(sender, btnSave)) TryInvokeCommand(UsersToolbarCommand.SaveUser);
        else if (ReferenceEquals(sender, btnDisable)) TryInvokeCommand(UsersToolbarCommand.DisableUser);
        else if (ReferenceEquals(sender, btnResetPassword)) TryInvokeCommand(UsersToolbarCommand.ResetPassword);
        else if (ReferenceEquals(sender, btnClose)) TryInvokeCommand(UsersToolbarCommand.CloseScreen);
    }

    /// <summary>
    /// Optional entry point for an existing owner shortcut. Checks the associated button's Enabled
    /// state and current owner condition. Does not register keys, close windows, or report success.
    /// True only means the supplied owner callback returned.
    /// </summary>
    public bool TryInvokeCommand(UsersToolbarCommand command)
    {
        if (IsDisposed || Disposing || !Enabled || commandBindings is null) return false;
        var button = command switch
        {
            UsersToolbarCommand.NewUser => btnNew,
            UsersToolbarCommand.EditUser => btnEdit,
            UsersToolbarCommand.SaveUser => btnSave,
            UsersToolbarCommand.DisableUser => btnDisable,
            UsersToolbarCommand.ResetPassword => btnResetPassword,
            UsersToolbarCommand.CloseScreen => btnClose,
            _ => throw new ArgumentOutOfRangeException(nameof(command))
        };
        return button.Enabled && commandBindings.TryInvoke(command);
    }
}
