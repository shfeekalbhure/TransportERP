namespace TransportERP.Desktop.Forms.SystemSettings.General;

internal static class SourceSetupLayout
{
    internal static void AddToolbarAction(Control root, string targetName, string caption)
    {
        var target = (Button)root.Controls.Find(targetName, true).Single();
        var toolbar = (FlowLayoutPanel)root.Controls.Find("pnlToolbar", true).Single();
        var action = toolbar.Controls.Find("toolbar_" + targetName, false).OfType<Button>().FirstOrDefault() ?? new Button
        {
            Name = "toolbar_" + targetName, Text = caption, AutoSize = true,
            Enabled = target.Enabled, AccessibleName = caption,
            AccessibleDescription = target.AccessibleDescription
        };
        target.EnabledChanged += (_, _) => action.Enabled = target.Enabled;
        action.Click += (_, _) =>
        {
            for (Control? parent = target.Parent; parent is not null; parent = parent.Parent)
                if (parent is TabPage page && page.Parent is TabControl tabs) tabs.SelectedTab = page;
            target.PerformClick();
            target.Focus();
        };
        if (action.Parent != toolbar) toolbar.Controls.Add(action);
    }

    // Opt-in for this reviewed group; do not change unreviewed screens or service behavior.
    internal static void ApplyReviewedProperties(UserControl root)
    {
        root.RightToLeft = RightToLeft.Yes;
        root.AutoScaleMode = AutoScaleMode.Dpi;
        root.AutoScroll = true;
        root.Dock = DockStyle.Fill;
        root.Margin = Padding.Empty;
        TransportERP.Desktop.CoreUI.SharedScreenProperties.ApplyContent(root);
        var layout = (TableLayoutPanel)root.Controls.Find("layout", true).Single();
        layout.RowStyles.Clear();
        for (int row = 0; row < layout.RowCount; row++)
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        var toolbar = (FlowLayoutPanel)root.Controls.Find("pnlToolbar", true).Single();
        if (toolbar.Parent is not TransportERP.Desktop.CoreUI.DesignerCommandBar)
        {
            toolbar.FlowDirection = FlowDirection.LeftToRight; // Mirrored by the inherited RTL direction.
            toolbar.WrapContents = true;
        }
        var tips = new ToolTip { ShowAlways = true };
        root.Disposed += (_, _) => tips.Dispose();
        void Visit(Control parent)
        {
            IEnumerable<Control> children = parent.Controls.Cast<Control>();
            if (parent is TableLayoutPanel table)
                children = children.OrderBy(table.GetRow).ThenBy(table.GetColumn);
            int order = 0;
            foreach (var child in children)
            {
                if (child is TransportERP.Desktop.CoreUI.DesignerCommandBar) continue;
                child.TabIndex = order++;
                if (child is Label or Panel or GroupBox) child.TabStop = false;
                // An expanded selector must not move its field caption away from the editor.
                if (child is Label label && parent is TableLayoutPanel && parent.Name != "layout")
                    label.TextAlign = ContentAlignment.TopRight;
                if (child is CheckBox check)
                {
                    check.CheckAlign = ContentAlignment.TopRight;
                    check.TextAlign = ContentAlignment.TopRight;
                }
                if (child is ComboBox combo) combo.DropDownStyle = ComboBoxStyle.DropDownList;
                if (child is TextBox text) text.TextAlign = text.RightToLeft == RightToLeft.No
                    ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                if (child is ListBox list)
                {
                    list.BorderStyle = BorderStyle.FixedSingle;
                    list.IntegralHeight = false;
                    list.SelectionMode = SelectionMode.One;
                }
                if (child is Button button)
                {
                    button.FlatStyle = FlatStyle.Standard;
                    button.UseVisualStyleBackColor = true;
                    button.TextAlign = ContentAlignment.MiddleCenter;
                    tips.SetToolTip(button, button.AccessibleDescription is { Length: > 0 } description
                        ? description : button.AccessibleName ?? button.Text);
                }
                Visit(child);
            }
        }
        Visit(root);
    }

    // Inline selector surfaces contain no assumed records or invented grid columns.
    // Existing editors are reparented, preserving Foundation dirty-state bindings.
    internal static Button AddSelector(Control root, string editorName, string name, string caption, bool multiple = true)
    {
        var editor = root.Controls.Find(editorName, true).Single();
        var parent = (TableLayoutPanel)editor.Parent!;
        var cell = parent.GetPositionFromControl(editor);
        var host = new TableLayoutPanel
        {
            Name = "pnl" + name, Dock = DockStyle.Top, AutoSize = true,
            ColumnCount = 2, RowCount = 2, Margin = editor.Margin, RightToLeft = RightToLeft.Yes
        };
        host.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        host.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        host.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        host.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        parent.Controls.Remove(editor);
        editor.Margin = Padding.Empty;
        host.Controls.Add(editor, 0, 0);
        var button = new Button
        {
            Name = name, Text = "…", AccessibleName = caption, AutoSize = true,
            MinimumSize = new Size(36, 30), Margin = new Padding(4, 0, 0, 0)
        };
        host.Controls.Add(button, 1, 0);
        var selection = new GroupBox
        {
            Name = "grp" + name, Text = caption, Dock = DockStyle.Top, Height = 190,
            MinimumSize = new Size(0, 190), Padding = new Padding(8, 24, 8, 8), Visible = false
        };
        Control list = multiple
            ? new CheckedListBox { CheckOnClick = true, IntegralHeight = false }
            : new ListBox { IntegralHeight = false };
        list.Name = "lst" + name;
        list.AccessibleName = caption;
        list.Enabled = false;
        list.Dock = DockStyle.Fill;
        selection.Controls.Add(list);
        selection.Controls.Add(new Label
        {
            Text = "القائمة غير مرتبطة ببيانات؛ لا توجد سجلات للاختيار.", AutoSize = true,
            Dock = DockStyle.Bottom, Padding = new Padding(4), MaximumSize = new Size(550, 0)
        });
        host.Controls.Add(selection, 0, 1);
        host.SetColumnSpan(selection, 2);
        parent.Controls.Add(host, cell.Column, cell.Row);
        button.Click += (_, _) => selection.Visible = !selection.Visible;
        button.AccessibleDescription = caption + " — عرض قائمة فارغة؛ مصدر البيانات غير مرتبط.";
        return button;
    }

    internal static void Multiline(Control root, string name, int height)
    {
        var editor = (TextBox)root.Controls.Find(name, true).Single();
        editor.Multiline = true;
        editor.ScrollBars = ScrollBars.Vertical;
        editor.MinimumSize = new Size(0, height);
        editor.Height = height;
    }

    internal static void AddNotice(TabPage page, string message)
    {
        page.Controls.Add(new Label { Text = message, Dock = DockStyle.Top, AutoSize = true,
            Padding = new Padding(16), MaximumSize = new Size(650, 0) });
    }

    internal static void AddLogo(TabPage page, string caption)
    {
        var group = new GroupBox { Text = caption, Name = "grpLogo", Dock = DockStyle.Left,
            Width = 190, Padding = new Padding(10, 28, 10, 10) };
        group.Controls.Add(new PictureBox { Name = "picLogo", Dock = DockStyle.Top,
            Height = 150, BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom,
            AccessibleName = caption });
        group.Controls.Add(new Button { Name = "btnAddLogo", Text = "إضافة", Dock = DockStyle.Bottom,
            Height = 35, Enabled = false, AccessibleDescription = "استيراد الشعار غير مرتبط." });
        // The source explicitly locates the logo at the left of the main data.
        var fields = page.Controls.OfType<TableLayoutPanel>().Single();
        page.Controls.Remove(fields);
        var host = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
        host.Controls.Add(fields);
        page.Controls.Add(host);
        page.Controls.Add(group);
    }
}
