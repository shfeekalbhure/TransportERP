using System.Reflection;
using TransportERP.Desktop;
using TransportERP.Desktop.Forms.SystemSettings.General;

internal static class Program
{
    private static int checks;
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
        Console.WriteLine("PASS: " + message); checks++;
    }
    private static IEnumerable<Control> Descendants(Control root) => root.Controls.Cast<Control>().SelectMany(c => new[] { c }.Concat(Descendants(c)));
    private static T Find<T>(Control root, string name) where T : Control => (T)root.Controls.Find(name, true).Single();
    private static object? Call(object instance, string name, params object?[] args) => instance.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(instance, args);
    private static IEnumerable<TreeNode> Nodes(TreeNodeCollection nodes) => nodes.Cast<TreeNode>().SelectMany(n => new[] { n }.Concat(Nodes(n.Nodes)));

    [STAThread]
    private static int Main(string[] args)
    {
        try
        {
            ApplicationConfiguration.Initialize();
            using var company = new UcCompanyManagement();
            using var alias = new UcCompanyData();
            var controls = Descendants(company).ToArray();
            Check(controls.Select(c => (c.Name, c.GetType())).SequenceEqual(Descendants(alias).Select(c => (c.Name, c.GetType()))), "Legacy alias has exactly the canonical control tree");
            Check(controls.Where(c => c.Name.Length > 0).GroupBy(c => c.Name).All(g => g.Count() == 1), "Unique control identities");
            Check(company.RightToLeft == RightToLeft.Yes, "Canonical RTL");
            foreach (var name in new[] { "txtCompanyNameAr", "cmbActivityType", "cmbBaseCurrency", "cmbCountry" })
                Check(Find<Control>(company, name).BackColor == Color.FromArgb(255, 249, 219), "Required yellow field " + name);
            foreach (var name in new[] { "button1", "btnSave", "btnEdit", "btnDelete", "button2", "btnFirst", "btnPrevious", "btnNext", "btnLast", "btnUndo", "btnSearch", "btnPrint", "button4", "btnAddLogo", "btnRemoveLogo" })
                Check(!Find<Button>(company, name).Enabled, "Unavailable operation remains disabled " + name);
            Check(Find<TabControl>(company, "tabCompanyManagement").TabPages.Count == 3, "All three designed tabs retained");
            int closeEvents = 0;
            alias.CloseRequested += (_, _) => closeEvents++;
            Call(alias, "OnLoad", EventArgs.Empty);
            // Invoke protected OnClick to exercise the actual button event without showing a window.
            typeof(Button).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(Find<Button>(alias, "btnClose"), new object[] { EventArgs.Empty });
            Check(closeEvents == 1, "Alias close event delivered once");
            using (var wrapper = new FrmCompanyManagement())
                Check(Descendants(wrapper).OfType<UcCompanyManagement>().Count() == 1, "Form wrapper hosts one canonical control");
            using var main = new FrmMain();
            var tree = Find<TreeView>(main, "tvSystemTree");
            var node = Nodes(tree.Nodes).Single(n => Equals(n.Tag, "02.01.01"));
            var tabs = Find<TabControl>(main, "tabOpenScreens");
            var before = tabs.TabCount;
            Call(main, "ActivateNavigationNode", node);
            var page = tabs.TabPages["02.01.01"]!;
            var hosted = page.Controls.OfType<UcCompanyManagement>().Single();
            Check(tabs.TabCount == before + 1, "Company tree node opens canonical workspace tab");
            for (int i = 0; i < 5; i++) Call(main, "ActivateNavigationNode", node);
            Check(tabs.TabCount == before + 1 && ReferenceEquals(hosted, page.Controls.OfType<UcCompanyManagement>().Single()), "Repeated activation reuses same tab and instance");
            typeof(Button).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(Find<Button>(hosted, "btnClose"), new object[] { EventArgs.Empty });
            Check(tabs.TabCount == before && hosted.IsDisposed, "Close button removes tab and disposes hosted control");
            Call(main, "ActivateNavigationNode", node);
            Check(tabs.TabCount == before + 1 && !ReferenceEquals(hosted, tabs.TabPages["02.01.01"]!.Controls.OfType<UcCompanyManagement>().Single()), "Reopening creates fresh canonical instance");
            Call(main, "CloseWorkspacePage", tabs.TabPages["02.01.01"]);
            if (args.Length > 0) Directory.CreateDirectory(args[0]);
            foreach (var size in new[] { new Size(1280, 800), new Size(900, 650) })
            {
                using var host = new Form { ClientSize = size, ShowInTaskbar = false, StartPosition = FormStartPosition.Manual, Location = new Point(-10000, -10000) };
                using var screen = new UcCompanyManagement { Dock = DockStyle.Fill };
                host.Controls.Add(screen);
                host.CreateControl(); screen.CreateControl(); host.PerformLayout(); screen.PerformLayout();
                _ = host.Handle; _ = screen.Handle;
                host.Show();
                Application.DoEvents();
                screen.PerformLayout();
                Check(screen.Size == host.ClientSize, $"Hosted resize {size.Width}x{size.Height}");
                Check(Find<TabPage>(screen, "tabCompanies").AutoScroll, "Company tab retains scroll access");
                if (args.Length > 0)
                {
                    using var bitmap = new Bitmap(size.Width, size.Height);
                    screen.DrawToBitmap(bitmap, new Rectangle(Point.Empty, size));
                    bitmap.Save(Path.Combine(args[0], $"company-{size.Width}x{size.Height}.png"));
                }
                Console.WriteLine($"INFO: Runtime DeviceDpi={screen.DeviceDpi}; size={size}");
                host.Close();
            }
            Console.WriteLine($"PASS: {checks} company runtime checks. Real monitor DPI transitions, interactive Visual Studio Designer and backend company CRUD are NOT claimed.");
            return 0;
        }
        catch (Exception ex) { Console.Error.WriteLine(ex); return 1; }
    }
}
