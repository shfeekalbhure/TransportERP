using System.Drawing;
using TransportERP.Contracts.Wave1;

namespace TransportERP.Desktop.Wave1;

/// <summary>
/// Structural WAVE-1 WinForms shell. Final pixel/layout approval remains a separate visual gate.
/// The shell deliberately omits a Close toolbar command; window closure is owned by the host/tab layer.
/// </summary>
public sealed class Wave1ScreenForm : Form
{
    public Wave1ScreenDefinition Definition { get; }

    public Wave1ScreenForm(string screenId)
        : this(Wave1ScreenCatalog.GetRequired(screenId))
    {
    }

    public Wave1ScreenForm(Wave1ScreenDefinition definition)
    {
        Definition = definition ?? throw new ArgumentNullException(nameof(definition));
        InitializeScreen();
    }

    private void InitializeScreen()
    {
        SuspendLayout();

        Text = $"{Definition.ArabicName} — {Definition.ScreenId}";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(900, 600);
        AutoScaleMode = AutoScaleMode.Font;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            RightToLeft = RightToLeft.Yes,
            Padding = new Padding(12)
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var title = new Label
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            Font = new Font(Font, FontStyle.Bold),
            Text = $"{Definition.ArabicName}  ({Definition.ScreenId})",
            TextAlign = ContentAlignment.MiddleRight,
            Padding = new Padding(4, 8, 4, 8)
        };

        var body = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Horizontal,
            FixedPanel = FixedPanel.Panel1,
            SplitterDistance = 72,
            RightToLeft = RightToLeft.Yes
        };

        var toolbar = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = true,
            AutoScroll = true,
            Padding = new Padding(4)
        };

        foreach (var action in Definition.Actions
                     .Select(x => x.Action)
                     .Distinct(StringComparer.OrdinalIgnoreCase))
        {
            if (string.Equals(action, "Close", StringComparison.OrdinalIgnoreCase))
                continue;

            toolbar.Controls.Add(new Button
            {
                AutoSize = true,
                Text = action,
                Tag = action,
                MinimumSize = new Size(90, 32)
            });
        }

        var areas = new TabControl
        {
            Dock = DockStyle.Fill,
            RightToLeft = RightToLeft.Yes,
            RightToLeftLayout = true
        };

        foreach (var area in Definition.Areas)
        {
            var tab = new TabPage(area)
            {
                RightToLeft = RightToLeft.Yes,
                AutoScroll = true
            };
            tab.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                AutoSize = false,
                Height = 42,
                TextAlign = ContentAlignment.MiddleRight,
                Text = $"منطقة {area} — التنفيذ التفصيلي يرتبط بعقد W1/W2 الحاكم للشاشة."
            });
            areas.TabPages.Add(tab);
        }

        body.Panel1.Controls.Add(toolbar);
        body.Panel2.Controls.Add(areas);

        var status = new StatusStrip { SizingGrip = false };
        status.Items.Add(new ToolStripStatusLabel($"Profile: {Definition.Profile}"));
        status.Items.Add(new ToolStripStatusLabel($"Variant: {Definition.Variant}"));
        status.Items.Add(new ToolStripStatusLabel("RTL"));

        root.Controls.Add(title, 0, 0);
        root.Controls.Add(body, 0, 1);
        root.Controls.Add(status, 0, 2);
        Controls.Add(root);

        ResumeLayout(performLayout: true);
    }
}

public static class Wave1ScreenFormFactory
{
    public static Form Create(string screenId) => new Wave1ScreenForm(screenId);
}
