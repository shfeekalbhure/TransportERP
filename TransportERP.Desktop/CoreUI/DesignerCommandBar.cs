using System.ComponentModel;

namespace TransportERP.Desktop.CoreUI;

public enum DesignerCommandRole
{
    None, Add, Edit, Delete, Cancel, View, Last, Next, Previous, First, Save, Print, Close,
    Refresh, Import, Export, Help
}

/// <summary>An empty stock Panel. Its owning Designer creates and owns every button.</summary>
[ToolboxItem(true)]
[ProvideProperty("CommandRole", typeof(Button))]
public sealed class DesignerCommandBar : Panel, IExtenderProvider
{
    private readonly Dictionary<Button, DesignerCommandRole> roles = new();
    public bool CanExtend(object extendee)
    {
        if(extendee is not Button button) return false;
        for(Control? parent = button.Parent; parent != null; parent = parent.Parent)
            if(ReferenceEquals(parent, this)) return true;
        return false;
    }
    [Category("Commands"), DefaultValue(DesignerCommandRole.None)]
    public DesignerCommandRole GetCommandRole(Button button) => roles.GetValueOrDefault(button);
    public void SetCommandRole(Button button, DesignerCommandRole role)
    {
        ArgumentNullException.ThrowIfNull(button);
        if(role == DesignerCommandRole.None) { roles.Remove(button); return; }
        if(roles.Any(pair => pair.Value == role && !ReferenceEquals(pair.Key, button)))
            throw new ArgumentException($"Command {role} already has a button.", nameof(role));
        roles[button] = role;
    }
    public bool ShouldSerializeCommandRole(Button button) => GetCommandRole(button) != DesignerCommandRole.None;
    public void ResetCommandRole(Button button) => roles.Remove(button);
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IReadOnlyDictionary<StandardCommand, Button> Commands => roles
        .Where(pair => !pair.Key.IsDisposed && CanExtend(pair.Key))
        .ToDictionary(pair => Enum.Parse<StandardCommand>(pair.Value.ToString()), pair => pair.Key);
}
