using System.Drawing;
using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.Security;

public enum SecurityScreenLayout
{
    Standard,
    ReadOnly,
    Tree
}

public abstract class SecurityScreenForm : Form
{
    protected SecurityScreenForm(object definition)
    {
        Text = $"{ReadText(definition, "Code", "FormName")} — {ReadText(definition, "ArabicName", "Title")}";
        Name = ReadText(definition, "Code", "FormName");
        RightToLeft = System.Windows.Forms.RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(980, 680);
        Size = new Size(1280, 820);
        Font = new Font("Tahoma", 9F);
        BuildScreen(definition);
    }

    private void BuildScreen(object definition)
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RightToLeft = System.Windows.Forms.RightToLeft.Yes,
            ColumnCount = 1,
            RowCount = 5,
            Padding = new Padding(12),
            BackColor = Color.White
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 58));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 34));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        Controls.Add(root);

        var header = new Label
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            Font = new Font("Tahoma", 12F, FontStyle.Bold),
            Text = $"{ReadText(definition, "Code", "FormName")} — {ReadText(definition, "ArabicName", "Title")}",
            TextAlign = ContentAlignment.MiddleRight,
            Padding = new Padding(8),
            BackColor = Color.FromArgb(239, 244, 250)
        };
        root.Controls.Add(header, 0, 0);

        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = true,
            Padding = new Padding(4)
        };
        foreach (var action in ReadTextArray(definition, "Actions"))
        {
            actions.Controls.Add(new Button
            {
                Text = action,
                AutoSize = true,
                Margin = new Padding(3),
                UseVisualStyleBackColor = true
            });
        }
        root.Controls.Add(actions, 0, 1);

        var tabs = new TabControl
        {
            Dock = DockStyle.Fill,
            RightToLeftLayout = true,
            Alignment = TabAlignment.Top
        };
        foreach (var tabName in ReadTextArray(definition, "Tabs"))
        {
            tabs.TabPages.Add(new TabPage(tabName) { RightToLeft = System.Windows.Forms.RightToLeft.Yes });
        }

        if (tabs.TabPages.Count > 0)
        {
            if (ReadText(definition, "Layout") == nameof(SecurityScreenLayout.Tree))
            {
                var treeLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Padding = new Padding(8) };
                treeLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                treeLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                treeLayout.Controls.Add(new Label { AutoSize = true, Text = "الشجرة هي العرض الرئيسي؛ البحث والتوسيع والطي والتحميل الكسول تُربط لاحقاً بخدمة معتمدة.", TextAlign = ContentAlignment.MiddleRight }, 0, 0);
                treeLayout.Controls.Add(new TreeView { Dock = DockStyle.Fill, RightToLeftLayout = true, RightToLeft = System.Windows.Forms.RightToLeft.Yes }, 0, 1);
                tabs.TabPages[0].Controls.Add(treeLayout);
            }
            else
            {
                tabs.TabPages[0].Controls.Add(CreateFieldsPanel(ReadTextArray(definition, "Fields")));
            }
        }
        root.Controls.Add(tabs, 0, 2);

        root.Controls.Add(CreateSearchAndResultsPanel(definition), 0, 3);
        root.Controls.Add(new Label
        {
            AutoSize = true,
            Text = "بيانات التدقيق: للقراءة فقط وتظهر بعد الربط المعتمد. لا تُنشأ قيم محلية تجريبية.",
            TextAlign = ContentAlignment.MiddleRight,
            Padding = new Padding(6),
            ForeColor = Color.DimGray
        }, 0, 4);
    }

    private static string ReadText(object definition, params string[] propertyNames)
    {
        var type = definition.GetType();
        foreach (var propertyName in propertyNames)
        {
            var value = type.GetProperty(propertyName)?.GetValue(definition);
            if (value is not null) return value.ToString() ?? string.Empty;
        }
        return string.Empty;
    }

    private static IEnumerable<string> ReadTextArray(object definition, string propertyName)
    {
        return definition.GetType().GetProperty(propertyName)?.GetValue(definition) as IEnumerable<string>
            ?? Array.Empty<string>();
    }

    private static Control CreateFieldsPanel(IEnumerable<string> fields)
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            RightToLeft = System.Windows.Forms.RightToLeft.Yes,
            ColumnCount = 2,
            Padding = new Padding(10)
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        var index = 0;
        foreach (var field in fields)
        {
            var item = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 1, Margin = new Padding(6) };
            item.Controls.Add(new Label { Text = field, AutoSize = true, TextAlign = ContentAlignment.MiddleRight });
            item.Controls.Add(new TextBox { Dock = DockStyle.Top, ReadOnly = true, BackColor = Color.White, TabStop = false });
            panel.Controls.Add(item, index % 2, index / 2);
            index++;
        }
        return panel;
    }

    private static Control CreateSearchAndResultsPanel(object definition)
    {
        var panel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Padding = new Padding(4) };
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var filters = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, WrapContents = true };
        filters.Controls.Add(new Label { Text = "البحث العام", AutoSize = true, Padding = new Padding(4) });
        filters.Controls.Add(new TextBox { Width = 180 });
        foreach (var filter in ReadTextArray(definition, "Filters"))
        {
            filters.Controls.Add(new Label { Text = filter, AutoSize = true, Padding = new Padding(4) });
        }
        filters.Controls.Add(new Button { Text = "بحث", AutoSize = true });
        filters.Controls.Add(new Button { Text = "مسح المرشحات", AutoSize = true });
        panel.Controls.Add(filters, 0, 0);

        var resultsHint = ReadText(definition, "Layout") == nameof(SecurityScreenLayout.Tree)
            ? "يعرض الجدول المساعد نتائج البحث أو التقارير فقط؛ الأعمدة غير محددة في المرجع."
            : "جدول النتائج: الأعمدة غير المحددة في المرجع لم تُنشأ.";
        panel.Controls.Add(new Panel
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.FixedSingle,
            Controls =
            {
                new Label { Dock = DockStyle.Fill, Text = resultsHint, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.DimGray }
            }
        }, 0, 1);
        return panel;
    }
}
