using System.Reflection;
using TransportERP.EmptyForms;
using TransportERP.Desktop.Forms.SystemSettings.DocumentControl;

internal static class CodeRepairChecks
{
    private static int checks;
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
        Console.WriteLine("PASS: " + message); checks++;
    }
    private static object? Call(object instance, string name, params object?[] args) =>
        instance.GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(instance, args);
    private static T Find<T>(Control root, string name) where T : Control =>
        (T)root.Controls.Find(name, true).Single();

    [STAThread]
    private static int Main()
    {
        try
        {
            ApplicationConfiguration.Initialize();
            using (var numbering = new UcDocumentNumbering())
            {
                foreach (string id in new[] { "NONE", "COMPANY", "BRANCH", "YEAR", "COMPANY_YEAR", "BRANCH_YEAR" })
                {
                    Check(numbering.Foundation.LoadFields(new Dictionary<string, object?> { ["cboResetType"] = id }), "Load reset " + id);
                    Check(Equals(numbering.Foundation.CaptureFields()["cboResetType"], id), "Capture reset " + id);
                }
            }
            using var receipt = new UcScreen_04_04_01();
            var tabs = Find<TabControl>(receipt, "tabs");
            Check(tabs.SelectedTab?.Name == "tp0", "Receipt opens main tab by reference");
            var grid = Find<DataGridView>(receipt, "dgvLines");
            string[] keys = { "rowNo", "counterAccountRef", "lineDescription", "currencyRef", "exchangeRate", "amount", "partyRef", "accountingAmount", "analyticalAccount", "accountName", "foreignAmount", "costCenter", "project", "activity" };
            Check(grid.Columns.Cast<DataGridViewColumn>().Select(c => c.Name).SequenceEqual(keys), "Receipt contract keys and existing order");
            // Exercise actual session add/snapshot/renumber methods without a backend or a destructive command.
            Call(receipt.Binding, "AddRow"); Call(receipt.Binding, "AddRow"); Call(receipt.Binding, "AddRow");
            Check(Enumerable.Range(0, 3).All(i => Equals(grid.Rows[i].Cells["rowNo"].Value, i + 1)), "Add rows assigns sequence");
            grid.Rows.RemoveAt(1); Call(receipt.Binding, "Renumber");
            Check(grid.Rows.Count == 2 && Equals(grid.Rows[1].Cells["rowNo"].Value, 2), "Renumber after middle-row removal");
            var snapshot = (BatchFiveRequest)Call(receipt.Binding, "Snapshot", "Create", null)!;
            Check(snapshot.Rows.All(r => !r.Keys.Intersect(new[] { "analyticalAccount", "accountName", "foreignAmount", "costCenter", "project", "activity" }).Any()), "Snapshot excludes display-only columns");
            // Check actual validation error cells; required header fields may be empty in this isolated test.
            foreach (string key in new[] { "amount", "exchangeRate" })
            {
                grid.Rows[0].Cells[key].Value = "abc";
                Check(!receipt.Binding.ValidateEditors() && grid.Rows[0].Cells[key].ErrorText.Length > 0, "Reject nonnumeric editor " + key);
                grid.Rows[0].Cells[key].Value = "1";
                using var preview = new DataGridView { AllowUserToAddRows = false, Tag = new[] { key } };
                preview.Columns.Add(key, key); preview.Rows.Add("abc");
                int before = grid.Rows.Count;
                receipt.Binding.ImportPreview(preview);
                Check(grid.Rows.Count == before && preview.Rows.Count == 1 && Find<Label>(receipt, "lblStatus").Text.Contains("CSV"), "Reject invalid CSV before append " + key);
            }
            // Fresh screen avoids leave-confirmation dialogs and uses canonical keys on load.
            using var loaded = new UcScreen_04_04_01();
            Check(loaded.Binding.LoadDocument(new Dictionary<string, object?>(), new[] { new Dictionary<string, object?> { ["amount"] = "2", ["exchangeRate"] = "1", ["foreignAmount"] = "2" } }, "test-version", true), "Load canonical keys");
            var loadedGrid = Find<DataGridView>(loaded, "dgvLines");
            Check(Equals(loadedGrid.Rows[0].Cells["amount"].Value, "2") && Equals(loadedGrid.Rows[0].Cells["rowNo"].Value, 1), "Loaded data and sequence preserved");
            CodeRepairManifestChecks.Run(Check);
            Console.WriteLine($"PASS: {checks} checks. No backend/persistence or visual/DPI acceptance is claimed.");
            Console.WriteLine("Manual Windows acceptance still required: confirm deletion, valid CSV append, visual comparison, DPI and backend save/reload.");
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
    }
}
