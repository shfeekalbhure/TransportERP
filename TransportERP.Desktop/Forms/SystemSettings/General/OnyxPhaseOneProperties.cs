namespace TransportERP.Desktop.Forms.SystemSettings.General;

/// <summary>
/// Property-only fallback for existing code-built General settings screens.
/// Does not create/reparent controls, change data, or replace the screen layout.
/// Remove this opt-in when these screens are migrated to the WinForms Designer.
/// </summary>
internal static class OnyxPhaseOneProperties
{
    internal static void Apply(UserControl screen)
    {
        screen.SuspendLayout();
        try
        {
            screen.Dock = DockStyle.Fill;
            screen.AutoScroll = true;
            screen.Margin = Padding.Empty;
            screen.Padding = new Padding(2);
            screen.RightToLeft = RightToLeft.Yes;
            screen.BackColor = Color.FromArgb(244, 244, 244);
            screen.ForeColor = Color.FromArgb(45, 45, 45);
            if (screen.Font.Name != "Tahoma" || screen.Font.Size != 9F || screen.Font.Style != FontStyle.Regular)
                screen.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            ApplyChildren(screen, screen.Font);
        }
        finally
        {
            screen.ResumeLayout(true);
        }
    }

    private static void ApplyChildren(Control parent, Font font)
    {
        foreach (Control control in parent.Controls)
        {
            if (control is TransportERP.Desktop.CoreUI.DesignerCommandBar) continue;
            control.Font = font;
            control.Margin = new Padding(2);
            if (control is DataGridView grid)
            {
                grid.RightToLeft = RightToLeft.Yes;
                grid.BackgroundColor = Color.White;
                grid.BorderStyle = BorderStyle.FixedSingle;
                grid.GridColor = Color.FromArgb(176, 176, 176);
                grid.EnableHeadersVisualStyles = false;
                grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(216, 255, 255);
                grid.ColumnHeadersDefaultCellStyle.Font = font;
                grid.DefaultCellStyle.Font = font;
                // Leave column formats, values, editing permissions and row population intact.
                continue;
            }
            if (control is Panel or TabPage)
            {
                control.BackColor = Color.FromArgb(244, 244, 244);
                control.Padding = new Padding(3);
                control.TabStop = false;
            }
            if (control is GroupBox group)
            {
                group.BackColor = Color.FromArgb(244, 244, 244);
                group.Padding = new Padding(3, Math.Max(20, group.Padding.Top), 3, 3);
                group.TabStop = false;
            }
            if (control is TableLayoutPanel table)
            {
                table.Margin = Padding.Empty;
                if (table.Name == "tlpAuditInfo")
                {
                    table.BackColor = Color.FromArgb(232, 227, 246);
                    table.BorderStyle = BorderStyle.FixedSingle;
                }
                // Existing column counts/cells are preserved: converting them is a Designer task.
                if (table.Name == "layout")
                {
                    table.Dock = DockStyle.Fill;
                    table.AutoSize = false;
                    table.AutoScroll = true;
                }
            }
            if (control is FlowLayoutPanel toolbar && toolbar.Name == "pnlToolbar")
            {
                toolbar.BackColor = Color.FromArgb(239, 239, 239);
                toolbar.BorderStyle = BorderStyle.FixedSingle;
                toolbar.Margin = Padding.Empty;
                // Keep growth/wrapping to prevent clipping at small host sizes.
            }
            if (control is TabControl tabs)
            {
                tabs.RightToLeft = RightToLeft.Yes;
                tabs.RightToLeftLayout = true;
                tabs.Margin = Padding.Empty;
            }
            if (control is Label label)
            {
                label.ForeColor = Color.FromArgb(45, 45, 45);
                label.TabStop = false;
                if (label.Name is "lblTitle" or "lblStatus")
                    label.Padding = new Padding(3);
            }
            if (control is TextBox text)
            {
                text.BorderStyle = BorderStyle.FixedSingle;
                if (!text.Multiline)
                    text.MinimumSize = new Size(text.MinimumSize.Width, Math.Max(24, text.MinimumSize.Height));
                // Keep required-field and other semantic color markers already set by the screen.
                if (text.BackColor == SystemColors.Window || text.BackColor == SystemColors.Control)
                    text.BackColor = text.ReadOnly ? SystemColors.Control : Color.White;
                text.TextAlign = text.RightToLeft == RightToLeft.No
                    ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                // Multiline editor heights and semantic LTR fields remain unchanged.
            }
            if (control is ComboBox or NumericUpDown or DateTimePicker)
                control.MinimumSize = new Size(control.MinimumSize.Width, Math.Max(24, control.MinimumSize.Height));
            if (control is Button button)
            {
                button.Padding = new Padding(2);
                button.Margin = new Padding(2);
                button.MinimumSize = new Size(button.MinimumSize.Width, Math.Max(24, button.MinimumSize.Height));
                // Do not invent icon resources or alter command availability/order.
            }
            ApplyChildren(control, font);
        }
    }
}
