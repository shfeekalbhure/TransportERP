namespace TransportERP.Desktop.Forms.Setup.Security;

public partial class UcUsersPermissions
{
    private void ApplySourceAdditionsSizing()
    {
        ApplyReferenceContainerVisuals(auditFooter, true);
        ApplyReferenceContainerVisuals(pnlToolbar, false);
        // The active toolbar is already included in pnlActions by the Designer.
        // No second policy action bar exists in this screen.
        var addedHeight = 0;
        MinimumSize = new Size(MinimumSize.Width, MinimumSize.Height + addedHeight + userLookupV20.GetPreferredSize(new Size(tabUserData.ClientSize.Width, 0)).Height);
    }
    private static void ApplyReferenceContainerVisuals(Control host, bool audit)
    {
        host.BackColor = audit ? Color.FromArgb(248, 250, 252) : Color.FromArgb(224, 224, 224);
        foreach (Control child in host.Controls)
        {
            if (child is Button button && !audit)
            {
                button.Dock = DockStyle.None;
                button.Margin = new Padding(4);
                button.BackColor = Color.FromArgb(224, 224, 224);
                button.ForeColor = Color.FromArgb(16, 24, 40);
                button.Font = new Font("Microsoft Sans Serif", 10F);
            }
            else if (audit && child is Label label)
            {
                label.Font = new Font("Segoe UI", 10F);
                label.Margin = new Padding(3);
                label.BackColor = Color.FromArgb(192, 255, 255);
                label.ForeColor = Color.FromArgb(16, 24, 40);
            }
            else if (audit && child is TextBox input)
            {
                input.Font = new Font("Segoe UI", 10F);
                input.Margin = new Padding(3);
            }
            if (child.HasChildren) ApplyReferenceContainerVisuals(child, audit);
        }
    }

}
