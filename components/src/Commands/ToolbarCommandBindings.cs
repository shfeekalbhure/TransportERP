namespace TransportERP.Desktop.SharedUI.Commands;

/// <summary>
/// UI-thread command bindings owned by a screen. Contains no controls, layout, shortcuts,
/// service lookup, data mutation, window closing, permission rules or success notifications.
/// The Designer-created toolbar will forward a click to TryInvoke; its visual adapter is separate.
/// </summary>
public sealed class ToolbarCommandBindings<TCommand> where TCommand : struct, Enum
{
    private sealed class Binding(Action invoke, Func<bool> canInvoke)
    {
        public Action Invoke { get; } = invoke;
        public Func<bool> CanInvoke { get; } = canInvoke;
    }

    private readonly Dictionary<TCommand, Binding> bindings = new();
    private readonly HashSet<TCommand> invoking = new();

    /// <summary>Replaces the previous binding, preventing duplicate handlers on reinitialization.</summary>
    public void SetBinding(TCommand command, Action invoke, Func<bool> canInvoke)
    {
        if (!Enum.IsDefined(command)) throw new ArgumentOutOfRangeException(nameof(command));
        ArgumentNullException.ThrowIfNull(invoke);
        ArgumentNullException.ThrowIfNull(canInvoke);
        bindings[command] = new Binding(invoke, canInvoke);
    }

    public bool RemoveBinding(TCommand command) => bindings.Remove(command);
    public void ClearBindings() => bindings.Clear();
    public bool IsBound(TCommand command) => bindings.ContainsKey(command);

    /// <summary>Rechecks the owner's current condition; an unbound command is never invokable.</summary>
    public bool CanInvoke(TCommand command) =>
        !invoking.Contains(command) && bindings.TryGetValue(command, out var binding) && binding.CanInvoke();

    /// <summary>
    /// True means the supplied callback returned; it does not mean data was persisted.
    /// Owner exceptions propagate and no command is substituted for another command.
    /// </summary>
    public bool TryInvoke(TCommand command)
    {
        if (invoking.Contains(command) || !bindings.TryGetValue(command, out var binding) || !binding.CanInvoke())
            return false;
        invoking.Add(command);
        try
        {
            binding.Invoke();
            return true;
        }
        finally
        {
            invoking.Remove(command);
        }
    }
}
