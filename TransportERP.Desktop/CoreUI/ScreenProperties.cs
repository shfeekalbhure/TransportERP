using System;
using System.Drawing;
using System.Windows.Forms;

namespace TransportERP.Desktop.CoreUI;

/// <summary>Root-only screen policy. Never walks or changes child controls.</summary>
public static class ScreenProperties
{
    private static readonly Font DefaultFont = new("Segoe UI", 10F);
    private static readonly Color ScreenBackground = Color.FromArgb(248, 250, 252);

    /// <summary>Call after InitializeComponent. Embedded components retain their layout bounds.</summary>
    public static void Apply(UserControl screen, bool embedded = false)
    {
        ArgumentNullException.ThrowIfNull(screen);
        screen.SuspendLayout();
        try
        {
            screen.RightToLeft = RightToLeft.Yes;
            // Changing a Font-mode root font changes its measured scaling basis.
            // Keep its designer font until that screen is explicitly migrated.
            if (screen.AutoScaleMode != AutoScaleMode.Font) screen.Font = DefaultFont;
            screen.BackColor = ScreenBackground;
            if (!embedded)
            {
                screen.Margin = Padding.Empty;
                screen.Padding = Padding.Empty;
                screen.BorderStyle = BorderStyle.None;
                ConfigureWorkspace(screen);
            }
        }
        finally { screen.ResumeLayout(true); }
    }

    /// <summary>The workspace owns the screen size; preserve content scroll extents.</summary>
    public static void ConfigureWorkspace(UserControl screen)
    {
        ArgumentNullException.ThrowIfNull(screen);
        screen.AutoSize = false;
        screen.MinimumSize = Size.Empty;
        screen.MaximumSize = Size.Empty;
        screen.AutoScroll = true;
        screen.Dock = DockStyle.Fill;
    }
}
