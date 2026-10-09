using System.ComponentModel;

namespace TransportERP.Desktop.CoreUI;

public enum StandardCommand { Add, Edit, Delete, Cancel, View, Last, Next, Previous, First, Save, Print, Close, Refresh, Import, Export, Help }

/// <summary>Designer-owned complete command catalog. Unavailable commands are hidden/disabled, never deleted.</summary>
public partial class StandardCommandBar : UserControl
{
    private readonly Dictionary<StandardCommand, Button> active;
    public StandardCommandBar()
    {
        InitializeComponent();
        active = new()
        {
            [StandardCommand.Add] = cmdAdd, [StandardCommand.Edit] = cmdEdit,
            [StandardCommand.Delete] = cmdDelete, [StandardCommand.Cancel] = cmdCancel,
            [StandardCommand.View] = cmdView, [StandardCommand.Last] = cmdLast,
            [StandardCommand.Next] = cmdNext, [StandardCommand.Previous] = cmdPrevious,
            [StandardCommand.First] = cmdFirst, [StandardCommand.Save] = cmdSave,
            [StandardCommand.Print] = cmdPrint, [StandardCommand.Close] = cmdClose,
            [StandardCommand.Refresh] = cmdRefresh, [StandardCommand.Import] = cmdImport,
            [StandardCommand.Export] = cmdExport, [StandardCommand.Help] = cmdHelp
        };
    }
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IReadOnlyDictionary<StandardCommand, Button> Commands => active;
    internal void SetProfile(IEnumerable<StandardCommand> visible)
    {
        var set = visible.ToHashSet();
        foreach(var pair in active) pair.Value.Visible = set.Contains(pair.Key);
    }
    internal void Adopt(StandardCommand command, Button original, bool visible)
    {
        var placeholder = active[command];
        var owner = placeholder.Parent!;
        var index = owner.Controls.GetChildIndex(placeholder);
        var image = placeholder.Image;
        retainedControls.Controls.Add(placeholder);
        placeholder.Visible = false;
        active[command] = original;
        Style(original, image ?? original.Image);
        owner.Controls.Add(original);
        owner.Controls.SetChildIndex(original, index);
        // The old FlowLayoutPanel may reposition the button while it is being styled.
        if(ReferenceEquals(owner, closeHost)) original.Location = new Point(1, 1);
        original.Visible = visible;
    }
    internal void AddSpecial(Button button, bool visible)
    {
        var caption = button.Text;
        Style(button, button.Image);
        // Business-specific commands without source artwork retain their readable caption.
        if(button.Image == null)
        {
            button.Text = caption;
            button.Width = Math.Max(26, TextRenderer.MeasureText(caption, button.Font).Width + 12);
        }
        commandFlow.Controls.Add(button);
        button.Visible = visible;
    }
    internal void AddAuxiliary(Control control)
    { control.Dock = DockStyle.None; control.Margin = new Padding(1); commandFlow.Controls.Add(control); }
    internal void UseCompactReferenceHeight()
    { MinimumSize = new Size(0, 30); Height = 30; }
    internal void BindReference(StandardCommand command, Button original) => active[command] = original;
    internal void UseReferenceLayout(Control original)
    {
        commandFlow.Visible = false;
        closeHost.Visible = false;
        commandBarContainer.Padding = Padding.Empty;
        commandBarContainer.BorderStyle = BorderStyle.None;
        int height = Math.Max(24, original.Height);
        MinimumSize = new Size(0, height); Height = height;
        commandBarContainer.Controls.Add(original);
        original.Dock = DockStyle.Fill;
    }
    internal void Retain(Control original) { retainedControls.Controls.Add(original); original.Visible = false; }
    private void Style(Button button, Image? image)
    {
        string caption = string.IsNullOrWhiteSpace(button.AccessibleName) ? button.Text : button.AccessibleName;
        if(string.IsNullOrWhiteSpace(caption)) caption = button.Name;
        button.AccessibleName = caption;
        button.AutoSize = false;
        button.Dock = DockStyle.None;
        button.Location = new Point(1, 1);
        button.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        button.MinimumSize = Size.Empty;
        button.MaximumSize = Size.Empty;
        button.Size = new Size(26, 24);
        button.Margin = new Padding(1);
        button.Padding = Padding.Empty;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        button.Image = image;
        button.ImageAlign = ContentAlignment.MiddleCenter;
        button.Text = "";
        if(image == null && !(button.AccessibleDescription ?? "").Contains("Missing icon"))
            button.AccessibleDescription = "Missing icon; " + button.AccessibleDescription;
        toolTips.SetToolTip(button, caption + (image == null ? " — Missing icon" : ""));
    }
}
