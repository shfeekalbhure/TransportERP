using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

/// <summary>Shared RTL draft layout for source-described setup fields. No persistence or sample data.</summary>
public partial class UcTextSetup : UserControl, IFoundationScreen, IExplicitScreenLayout
{
    protected override void OnLoad(EventArgs e)
    {
        // Derived constructors may add existing source-defined sections. Style their
        // properties only after construction, without rebuilding the control tree.
        OnyxPhaseOneProperties.Apply(this);
        base.OnLoad(e);
    }

    // Rehome existing editors into source-named tabs without replacing their dirty-state bindings.
    protected void ArrangeFieldTabs(params (string Title, string[] Fields)[] sections)
    {
        var layout = (TableLayoutPanel)Controls.Find("layout", true)[0];
        var sourceFields = (TableLayoutPanel)Controls.Find("fields", true)[0];
        var tabs = new TabControl
        {
            Name = "tabsFields", Dock = DockStyle.Fill, Height = 520,
            MinimumSize = new Size(0, 520), RightToLeft = RightToLeft.Yes, RightToLeftLayout = true
        };
        var panels = new List<TableLayoutPanel>();
        foreach (var section in sections)
        {
            var page = new TabPage(section.Title) { AutoScroll = true };
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2,
                RowCount = section.Fields.Length, Enabled = false
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68F));
            for (var row = 0; row < section.Fields.Length; row++)
            {
                string name = section.Fields[row];
                var editor = sourceFields.Controls.Find(name, false).Single();
                var label = sourceFields.Controls.Find("lbl" + name, false).Single();
                panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                panel.Controls.Add(label, 0, row);
                panel.Controls.Add(editor, 1, row);
            }
            page.Controls.Add(panel);
            tabs.TabPages.Add(page);
            panels.Add(panel);
        }
        sourceFields.Visible = false;
        layout.RowCount = 5;
        layout.Controls.Add(tabs, 0, 4);
        (Controls.Find("btnAdd", true).FirstOrDefault() ?? Controls.Find("btnEdit", true).Single()).Click += (_, _) =>
        {
            foreach (var panel in panels)
                if (!panel.IsDisposed) panel.Enabled = true;
            tabs.SelectNextControl(null, true, true, true, false);
        };
    }

    protected enum FieldKind { Text, ForeignText, Lookup, Derived, Check, SingleCheck, Choice, DerivedCheck, MultiLookup }
    protected sealed record Field(string Name, string Label, FieldKind Kind = FieldKind.Text,
        string[]? Choices = null);
    protected sealed record Table(string Name, string? Label, bool ReadOnly, params Field[] Columns);

    public FoundationUiSession Foundation { get; }
    public bool HasUnsavedChanges => Foundation.HasUnsavedChanges;
    public bool IsBusy => Foundation.IsBusy;
    public bool ConfirmLeave() => Foundation.ConfirmLeave();

    public UcTextSetup() : this(nameof(UcTextSetup), "", Array.Empty<Field>()) { }

    protected UcTextSetup(string name, string title, params Field[] definitions)
        : this(name, title, definitions, (Table?)null) { }

    protected UcTextSetup(string name, string title, Field[] definitions, string action)
        : this(name, title, definitions, Array.Empty<Table>(), false, action) { }

    protected UcTextSetup(string name, string title, Field[] definitions, Table? table)
        : this(name, title, definitions, table is null ? Array.Empty<Table>() : new[] { table }, false) { }

    protected UcTextSetup(string name, string title, Table[] tabs)
        : this(name, title, Array.Empty<Field>(), tabs, true) { }

    protected UcTextSetup(string name, string title, Field[] definitions, Table[] tabs)
        : this(name, title, definitions, tabs, true) { }

    protected UcTextSetup(string name, string title, Field[] definitions, Table table, string action)
        : this(name, title, definitions, new[] { table }, false, action) { }

    protected UcTextSetup(string name, string title, Field[] definitions, Table[] tabs, string action)
        : this(name, title, definitions, tabs, true, action) { }

    private UcTextSetup(string name, string title, Field[] definitions, Table[] tables, bool tabbed,
        string action = "إضافة")
    {
        InitializeComponent();
        SuspendLayout();
        Name = name;
        Text = title;
        Size = new Size(1000, 640);
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        RightToLeft = RightToLeft.Yes;

        var layout = new TableLayoutPanel
        {
            Name = "layout", Dock = DockStyle.Top, AutoSize = true,
            ColumnCount = 1, RowCount = tables.Length == 0 ? 4 : 5
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.Controls.Add(new Label
        {
            Name = "lblTitle", Text = title, AutoSize = true, Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 16F), TextAlign = ContentAlignment.MiddleRight,
            BackColor = Color.FromArgb(192, 192, 255), Padding = new Padding(12)
        }, 0, 0);
        layout.Controls.Add(new Label
        {
            Name = "lblStatus", AutoSize = true, Dock = DockStyle.Fill,
            Text = "معاينة إدخال مؤقتة — الحفظ والترقيم التلقائي والقوائم غير متاحة؛ القيم لا تُطبّق على النظام.",
            TextAlign = ContentAlignment.MiddleRight, Padding = new Padding(8)
        }, 0, 1);
        var toolbar = standardCommandFlow;
        var add = action == "تعديل" ? standardCommandEdit : standardCommandAdd;
        add.Name = action == "تعديل" ? "btnEdit" : "btnAdd";
        add.AccessibleName = action;
        add.Enabled = true;
        var save = standardCommandSave;
        layout.Controls.Add(designerCommandBar, 0, 2);

        var fields = new TableLayoutPanel
        {
            Name = "fields", Dock = DockStyle.Top, AutoSize = true,
            ColumnCount = 2, RowCount = definitions.Length, Enabled = false, TabIndex = 1
        };
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68F));
        var foreignFields = new List<TextBox>();
        for (var row = 0; row < definitions.Length; row++)
        {
            var definition = definitions[row];
            fields.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            fields.Controls.Add(new Label
            {
                Name = "lbl" + definition.Name, Text = definition.Label, AutoSize = true,
                Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight,
                Margin = new Padding(6, 8, 6, 8)
            }, 0, row);
            Control editor;
            if (definition.Kind == FieldKind.MultiLookup)
            {
                editor = new CheckedListBox
                {
                    Enabled = false, CheckOnClick = true, IntegralHeight = false,
                    MinimumSize = new Size(0, 88),
                    AccessibleDescription = "القائمة غير مرتبطة ببيانات بعد."
                };
            }
            else if (definition.Kind is FieldKind.Lookup or FieldKind.Choice)
            {
                var choice = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Enabled = definition.Kind == FieldKind.Choice,
                    AccessibleDescription = definition.Kind == FieldKind.Lookup
                        ? "القائمة غير مرتبطة ببيانات بعد." : ""
                };
                if (definition.Kind == FieldKind.Choice && definition.Choices is not null)
                    choice.Items.AddRange(definition.Choices.Cast<object>().ToArray());
                editor = choice;
            }
            else if (definition.Kind is FieldKind.Check or FieldKind.DerivedCheck)
            {
                editor = new CheckBox { AutoSize = true, AutoCheck = definition.Kind != FieldKind.DerivedCheck };
            }
            else
            {
                var text = new TextBox { ReadOnly = definition.Kind == FieldKind.Derived };
                if (definition.Kind == FieldKind.Derived)
                    text.AccessibleDescription = "قيمة تلقائية؛ تظهر بعد ربط القوائم بخدمة البيانات.";
                if (definition.Kind == FieldKind.ForeignText) foreignFields.Add(text);
                editor = text;
            }
            editor.Name = definition.Name;
            editor.AccessibleName = definition.Label;
            editor.Dock = DockStyle.Fill;
            editor.TabIndex = row;
            editor.Margin = new Padding(6, 8, 6, 8);
            fields.Controls.Add(editor, 1, row);
        }
        layout.Controls.Add(fields, 0, 3);
        var grids = new List<DataGridView>();
        TabControl? tabControl = null;
        if (tabbed)
        {
            tabControl = new TabControl
            {
                Name = "tabsSetup", Dock = DockStyle.Fill, Height = 340,
                MinimumSize = new Size(0, 340), RightToLeft = RightToLeft.Yes,
                RightToLeftLayout = true, TabIndex = 2
            };
            layout.Controls.Add(tabControl, 0, 4);
        }
        foreach (var table in tables)
        {
            var grid = new DataGridView
            {
                Name = table.Name, AccessibleName = table.Label ?? title,
                Dock = DockStyle.Fill, Height = 220, MinimumSize = new Size(0, 220),
                AutoGenerateColumns = false, ReadOnly = table.ReadOnly,
                AllowUserToAddRows = !table.ReadOnly && action != "تعديل", AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false, Enabled = table.ReadOnly, TabIndex = 2
            };
            foreach (var column in table.Columns)
            {
                DataGridViewColumn gridColumn;
                if (column.Kind is FieldKind.Lookup or FieldKind.Choice)
                {
                    var choice = new DataGridViewComboBoxColumn
                    {
                        ReadOnly = column.Kind == FieldKind.Lookup,
                        DisplayStyle = column.Kind == FieldKind.Lookup
                            ? DataGridViewComboBoxDisplayStyle.Nothing : DataGridViewComboBoxDisplayStyle.DropDownButton,
                        ToolTipText = column.Kind == FieldKind.Lookup ? "القائمة غير مرتبطة ببيانات بعد." : ""
                    };
                    if (column.Kind == FieldKind.Choice && column.Choices is not null)
                        choice.Items.AddRange(column.Choices.Cast<object>().ToArray());
                    gridColumn = choice;
                }
                else
                {
                    gridColumn = column.Kind is FieldKind.Check or FieldKind.SingleCheck
                        ? new DataGridViewCheckBoxColumn() : new DataGridViewTextBoxColumn();
                    gridColumn.ReadOnly = column.Kind == FieldKind.Derived;
                }
                gridColumn.Name = column.Name;
                gridColumn.HeaderText = column.Label;
                gridColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                if (column.Kind == FieldKind.ForeignText)
                    gridColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                grid.Columns.Add(gridColumn);
            }
            // The source permits only one transaction-affected type. This is a draft UI constraint.
            var exclusiveColumns = table.Columns.Where(c => c.Kind == FieldKind.SingleCheck)
                .Select(c => c.Name).ToHashSet();
            bool updatingChecks = false;
            grid.CurrentCellDirtyStateChanged += (_, _) =>
            {
                if (grid.IsCurrentCellDirty && grid.CurrentCell is DataGridViewCheckBoxCell)
                    grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
            grid.CellValueChanged += (_, e) =>
            {
                if (updatingChecks || e.RowIndex < 0 || e.ColumnIndex < 0
                    || !exclusiveColumns.Contains(grid.Columns[e.ColumnIndex].Name)
                    || grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value is not true) return;
                updatingChecks = true;
                try
                {
                    foreach (DataGridViewRow row in grid.Rows)
                        if (!row.IsNewRow && row.Index != e.RowIndex)
                            row.Cells[e.ColumnIndex].Value = false;
                }
                finally { updatingChecks = false; }
            };
            grids.Add(grid);
            if (tabControl is not null)
            {
                var page = new TabPage(table.Label ?? title) { Name = "tab" + table.Name };
                page.Controls.Add(grid);
                tabControl.TabPages.Add(page);
            }
            else if (table.Label is not null)
            {
                var group = new GroupBox
                {
                    Name = "grp" + table.Name, Text = table.Label,
                    Dock = DockStyle.Fill, Height = 260, MinimumSize = new Size(0, 260),
                    Padding = new Padding(8, 28, 8, 8)
                };
                group.Controls.Add(grid);
                layout.Controls.Add(group, 0, 4);
            }
            else layout.Controls.Add(grid, 0, 4);
        }
        Controls.Add(layout);
        SharedScreenProperties.Apply(this);
        foreach (var text in foreignFields)
        {
            text.RightToLeft = RightToLeft.No;
            text.TextAlign = HorizontalAlignment.Left;
        }
        // Leave Save disabled: this session supplies dirty tracking and close protection only.
        Foundation = new FoundationUiSession(this, bindDisabledActions: false);
        add.Click += (_, _) =>
        {
            fields.Enabled = true;
            foreach (var grid in grids)
                if (!grid.ReadOnly) grid.Enabled = true;
            add.Enabled = false;
            var firstEditor = fields.Controls.Cast<Control>().FirstOrDefault(c =>
                c.Enabled && (c is TextBox { ReadOnly: false } || c is CheckBox));
            if (firstEditor is not null) firstEditor.Focus();
            else grids.FirstOrDefault()?.Focus();
        };
        ResumeLayout(true);
    }
}
