using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.Setup.Security;

public partial class FrmUsersPermissions
{
    private void InitializeSourceAdditions()
    {
        // TAB SECTION MATRIX rows 33,35,36. Keep existing controls in their original tabs.
        // These existing project tabs are now owned by the Visual Studio Designer.
        // UserId and its bottom-docked layout are now serialized by the Designer.
        // RoleId and its existing layout are now serialized by the Designer.

        // FIELD DESIGN MATRIX row 218: DeviceId remains in the Devices tab,
        // with its label and layout now serialized by the Designer.
        // Placement approved by the user; options are exact V66 v20 policy values.
        var policy = NewSourceTable("securityPolicyV20");
        AddSourceField(policy, "قرار الصلاحية", "SettingValue__SET_SEC_001", PolicyOptions("DENY", "ALLOW"));
        AddSourceField(policy, "نوع هدف الصلاحية", "SettingValue__SET_SEC_002", PolicyOptions("SCREEN", "ACTION", "FIELD", "TAB"));
        AddSourceField(policy, "نطاق إسناد الصلاحية", "SettingValue__SET_SEC_003", PolicyOptions("TENANT", "GROUP", "COMPANY", "BRANCH", "MODULE", "ROLE", "USER"));
        ((ComboBox)policy.Controls.Find("SettingValue__SET_SEC_001", true)[0]).SelectedItem = "DENY";
        AddSourceField(policy, "الصلاحيات", "PermissionTargetIds", new CheckedListBox { Tag = "FLD-SEC-PERMS", CheckOnClick = true, IntegralHeight = true });
        policy.Dock = DockStyle.Bottom;
        tabPermissions.Controls.Add(policy);
        tabPermissions.Controls.SetChildIndex(policy, 1);

        // Company and branch access lists are now serialized by the Designer.

        var extraActions = new FlowLayoutPanel { Name = "policyActionsV20", AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, WrapContents = true, Padding = new Padding(16, 8, 16, 8) };
        foreach (var action in new[] { ("SaveDraft", "حفظ مسودة"), ("Validate", "تحقق"), ("Publish", "نشر/اعتماد"), ("RevertScopeOverride", "إلغاء تجاوز النطاق"), ("ViewAudit", "عرض التدقيق"), ("Refresh", "تحديث") })
        {
            var button = new Button { Name = "btnV20" + action.Item1, Text = action.Item2, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(16, 4, 16, 4), Margin = new Padding(4) };
            SettingsFormStyle.ApplySecondaryButtonStyle(button);
            button.MinimumSize = new Size(TextRenderer.MeasureText(button.Text, button.Font).Width + button.Padding.Horizontal, Math.Max(40, button.GetPreferredSize(Size.Empty).Height));
            extraActions.Controls.Add(button);
        }
        Controls.Add(extraActions);
        Controls.SetChildIndex(extraActions, 1);
        // Preserve the old content capacity after adding the second action bar.
        var addedHeight = extraActions.GetPreferredSize(new Size(ClientSize.Width, 0)).Height;
        MinimumSize = new Size(MinimumSize.Width, MinimumSize.Height + addedHeight + userLookupV20.GetPreferredSize(new Size(tabUserData.ClientSize.Width, 0)).Height);
    }
    private static ComboBox PolicyOptions(params string[] values)
    {
        var combo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        combo.Items.AddRange(values);
        return combo;
    }
    private static TableLayoutPanel NewSourceTable(string name)
    {
        var table = new TableLayoutPanel { Name = name, Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, ColumnCount = 2, RightToLeft = RightToLeft.Yes };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        return table;
    }
    private static void AddSourceField(TableLayoutPanel table, string text, string name, Control input)
    {
        int row = table.RowCount++;
        table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        var label = new Label { Text = text, Name = "lbl" + name, AutoSize = true, Anchor = AnchorStyles.Right, Margin = new Padding(4, 8, 16, 8) };
        SettingsFormStyle.ApplyLabelStyle(label);
        label.MinimumSize = TextRenderer.MeasureText(text, label.Font);
        var preferredListHeight = input is CheckedListBox ? input.Height : 0;
        SettingsFormStyle.ApplyInputStyle(input);
        if (preferredListHeight > 0) input.Height = preferredListHeight;
        input.Name = name;
        input.AccessibleName = text;
        input.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        input.Margin = new Padding(8);
        table.Controls.Add(label, 0, row);
        table.Controls.Add(input, 1, row);
    }}




