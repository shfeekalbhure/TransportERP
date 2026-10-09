using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace TransportERP;

/// <summary>Only the Form/UserControl root is changed. No child traversal or layout construction.</summary>
public static class ScreenRootProperties
{
    private sealed class Registration { public EventHandler? Handler; }
    private static readonly ConditionalWeakTable<ContainerControl, Registration> Registrations = new();
    private static readonly Font FallbackFont = new("Tahoma", 9F, FontStyle.Regular);

    public static void Attach(ContainerControl root)
    {
        Apply(root);
        var registration = Registrations.GetOrCreateValue(root);
        registration.Handler ??= (_, _) => Apply(root);
        // A compatibility Form may set a hosted UserControl's properties after its constructor.
        // Reapply only root defaults after existing Load handlers, never rebuild its children.
        if (root is Form form) { form.Load -= registration.Handler; form.Load += registration.Handler; }
        else if (root is UserControl screen) { screen.Load -= registration.Handler; screen.Load += registration.Handler; }
    }

    public static void Apply(ContainerControl root)
    {
        ArgumentNullException.ThrowIfNull(root);
        bool embedded = root.GetType().Name is "AuditCountersControl" or "UsersCommandBar" or "UcDispatchWaybillRow";
        root.SuspendLayout();
        try
        {
            if (string.IsNullOrWhiteSpace(root.Name)) root.Name = root.GetType().Name;
            root.RightToLeft = RightToLeft.Yes;
            // Retain approved screen-specific fonts and the existing scaling calibration.
            // Setting an unapproved/small font uses normal WinForms autoscaling; no child bounds are assigned.
            if (root.Font.Name is not ("Tahoma" or "Segoe UI") || root.Font.SizeInPoints < 9F)
                root.Font = FallbackFont;

            if (!embedded)
            {
                if (root.BackColor == SystemColors.Control || root.BackColor == Color.LightCyan
                    || root.BackColor == Color.FromArgb(248, 250, 252))
                    root.BackColor = Color.FromArgb(244, 244, 244);
                if (root.ForeColor == SystemColors.ControlText)
                    root.ForeColor = root.BackColor.GetBrightness() < 0.4F ? Color.White : Color.FromArgb(45, 45, 45);
                root.Margin = Padding.Empty;
                root.Padding = new Padding(Math.Min(4, root.Padding.Left), Math.Min(4, root.Padding.Top),
                    Math.Min(4, root.Padding.Right), Math.Min(4, root.Padding.Bottom));
            }

            if (root.AutoScaleMode == AutoScaleMode.Inherit)
            {
                root.AutoScaleMode = AutoScaleMode.Dpi;
                root.AutoScaleDimensions = new SizeF(96F, 96F);
            }
            else if (root.AutoScaleMode == AutoScaleMode.Dpi &&
                     (root.AutoScaleDimensions.Width <= 0 || root.AutoScaleDimensions.Height <= 0))
                root.AutoScaleDimensions = new SizeF(96F, 96F);
            else if (root.AutoScaleMode == AutoScaleMode.Font &&
                     (root.AutoScaleDimensions.Width <= 0 || root.AutoScaleDimensions.Height <= 0))
                root.AutoScaleDimensions = root.CurrentAutoScaleDimensions;

            if (root is Form window)
            {
                window.RightToLeftLayout = true;
                if (window.StartPosition is FormStartPosition.WindowsDefaultBounds or FormStartPosition.WindowsDefaultLocation)
                    window.StartPosition = FormStartPosition.CenterScreen;
                if (!window.IsMdiContainer) window.AutoScroll = true;
                // Keep modal/window state, fixed dialog borders, bounds, min/max sizes and visibility.
            }
            else if (root is UserControl screen && !embedded)
            {
                screen.AutoScroll = true;
                screen.TabStop = false;
                // Docking the design surface itself can shrink it. The runtime parent owns its bounds.
                if (screen.Parent != null && LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                {
                    screen.AutoSize = false;
                    screen.Dock = DockStyle.Fill;
                }
            }
            // Enabled/Visible, Size/MinimumSize/MaximumSize, Text, Tag, Icon and events are intentional state.
        }
        finally { root.ResumeLayout(true); }
    }
}
