namespace TransportERP.Desktop;

/// <summary>
/// ربط شاشة GEN-004 — المحافظات بقائمة البيانات الجغرافية وتبويبات Dashboard.
/// </summary>
public partial class FrmDashboard
{
    private const string GovernoratesTabKey = "GEN-004";

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        EnsureGovernoratesMenuItem();
    }

    private void EnsureGovernoratesMenuItem()
    {
        if (_generalSetupMenu is null)
        {
            return;
        }

        var geographicDataItem = _generalSetupMenu.Items
            .OfType<ToolStripMenuItem>()
            .FirstOrDefault(item => string.Equals(item.Text, "البيانات الجغرافية", StringComparison.Ordinal));

        if (geographicDataItem is null || geographicDataItem.DropDownItems.ContainsKey("mnuGovernorates"))
        {
            return;
        }

        var governoratesItem = new ToolStripMenuItem("المحافظات")
        {
            Name = "mnuGovernorates",
            ToolTipText = "GEN-004 — المحافظات",
            RightToLeft = RightToLeft.Yes
        };
        governoratesItem.Click += (_, _) => OpenGovernoratesTab();
        geographicDataItem.DropDownItems.Add(governoratesItem);
    }

    private void OpenGovernoratesTab()
    {
        if (_workspaceTabs is null)
        {
            return;
        }

        var existingPage = _workspaceTabs.TabPages
            .Cast<TabPage>()
            .FirstOrDefault(page => string.Equals(page.Name, GovernoratesTabKey, StringComparison.Ordinal));

        if (existingPage is not null)
        {
            _workspaceTabs.SelectedTab = existingPage;
            existingPage.Focus();
            return;
        }

        var governoratesForm = new FrmGovernorates();
        governoratesForm.ConfigureForTabHosting();

        var governoratesPage = new TabPage
        {
            Name = GovernoratesTabKey,
            Text = "المحافظات  ×",
            BackColor = Color.FromArgb(239, 245, 252),
            RightToLeft = RightToLeft.Yes,
            Padding = Padding.Empty,
            Tag = governoratesForm
        };

        governoratesForm.FormClosed += (_, _) =>
        {
            if (_workspaceTabs.TabPages.Contains(governoratesPage))
            {
                _workspaceTabs.TabPages.Remove(governoratesPage);
                governoratesPage.Dispose();
            }
        };

        governoratesPage.Controls.Add(governoratesForm);
        _workspaceTabs.TabPages.Add(governoratesPage);
        _workspaceTabs.SelectedTab = governoratesPage;
        governoratesForm.Show();
    }
}
