using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.Views.Security.Shared;

/// <summary>
/// تهيئة السلوك العام لشاشات الأمن دون وضع منطق أعمال داخل الواجهة.
/// الربط الحقيقي بالـHTTP/API يضاف لاحقًا من خلال خدمات Desktop المتخصصة.
/// </summary>
internal static class SecurityViewRuntime
{
    internal static void Initialize(
        UserControl owner,
        TransportReferenceScreenShell shell,
        string screenTitle,
        string searchPlaceholder,
        SecurityWorkspaceMode mode)
    {
        owner.RightToLeft = RightToLeft.Yes;
        shell.SearchPanel.SearchPlaceholder = searchPlaceholder;
        shell.SearchPanel.SetStatusItems("نشط", "موقوف", "معلق", "منتهي");
        shell.Pagination.SetPageInfo(1, 1, 0, 0, 0);
        shell.AuditPanel.ClearAuditInfo();
        shell.AlertBar.HideMessage();

        shell.Toolbar.CloseRequested += (_, _) => CloseHostTab(owner);
        shell.Toolbar.NewRequested += (_, _) => ClearEditableFields(shell.DataHost);

        if (mode == SecurityWorkspaceMode.ReadOnly)
        {
            SetCrudEnabled(shell.Toolbar, false);
        }
        else if (mode == SecurityWorkspaceMode.ReadOnlyWithActions)
        {
            shell.Toolbar.NewButton.Enabled = false;
            shell.Toolbar.SaveButton.Enabled = false;
            shell.Toolbar.EditButton.Enabled = false;
            shell.Toolbar.DeleteButton.Enabled = false;
        }

        if (mode == SecurityWorkspaceMode.Settings)
        {
            shell.Toolbar.NewButton.Enabled = false;
            shell.Toolbar.DeleteButton.Enabled = false;
        }

        shell.Grid.ReadOnly = true;
        shell.Grid.RightToLeft = RightToLeft.Yes;
        shell.GridGroup.Text = screenTitle;
    }

    private static void SetCrudEnabled(TransportToolbar toolbar, bool enabled)
    {
        toolbar.NewButton.Enabled = enabled;
        toolbar.SaveButton.Enabled = enabled;
        toolbar.EditButton.Enabled = enabled;
        toolbar.DisableButton.Enabled = enabled;
        toolbar.DeleteButton.Enabled = enabled;
    }

    private static void ClearEditableFields(Control root)
    {
        foreach (Control child in root.Controls)
        {
            if (child is RequiredTextBox required)
            {
                required.Clear();
            }
            else if (child is TextBox textBox && !textBox.ReadOnly)
            {
                textBox.Clear();
            }
            else if (child is ComboBox comboBox && comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = -1;
            }
            else if (child is DateTimePicker dateTimePicker)
            {
                dateTimePicker.Value = DateTime.Today;
                dateTimePicker.Checked = false;
            }

            if (child.HasChildren)
            {
                ClearEditableFields(child);
            }
        }
    }

    private static void CloseHostTab(UserControl owner)
    {
        if (owner.Parent is not TabPage page || page.Parent is not TabControl tabs)
        {
            return;
        }

        tabs.TabPages.Remove(page);
        owner.Dispose();
        page.Dispose();
    }
}