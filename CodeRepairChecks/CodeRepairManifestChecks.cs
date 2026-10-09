using System.Drawing.Printing;
using System.Reflection;
using System.Text;
using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.Waybills;

// Synthetic fixtures are test-only. No printer, provider, persistence or visual acceptance is simulated.
internal static class CodeRepairManifestChecks
{
    private static T Get<T>(object instance, string name) =>
        (T)instance.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(instance)!;
    private static void Call(object instance, string name, EventArgs args) =>
        instance.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(instance, new object?[] { null, args });
    private static ManifestResponse Fixture(string number, ManifestLineResponse[] lines) => new(
        Guid.NewGuid(), Guid.NewGuid(), number, DateTimeOffset.UtcNow, null, null,
        "TEST-DRAFT", 3, 4, lines, Guid.NewGuid());

    public static void Run(Action<bool, string> check)
    {
        using var manifest = new UcManifest();
        var noData = new PrintEventArgs();
        Call(manifest, "BeginPrint", noData);
        check(noData.Cancel, "Manifest refuses unloaded print job");
        string lastMarker = "END-OF-FINAL-LINE-100";
        var lines = Enumerable.Range(1, 100).Select(i => new ManifestLineResponse(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            i + 0.125m, i, i * 1.25m, i * 0.01m,
            string.Join(" ", Enumerable.Repeat("حالة تحميل طويلة لاختبار التفاف النص وتعدد الصفحات", 12)) +
            (i == 100 ? " " + lastMarker : ""))).ToArray();
        var first = Fixture("TEST-MANIFEST-FIRST", lines);
        manifest.Bind(first);
        var begin = new PrintEventArgs();
        Call(manifest, "BeginPrint", begin);
        check(!begin.Cancel, "Manifest accepts loaded test fixture");
        string body = Get<string>(manifest, "_printBody");
        string title = Get<string>(manifest, "_printTitle");
        string summary = Get<string>(manifest, "_printSummary");
        check(body.Contains(lines[0].Id.ToString()) && body.EndsWith(lastMarker), "Manifest body contains first and final detail");
        check(summary.Contains($"إجمالي الكمية: {lines.Sum(x => x.Quantity):N3}") &&
              summary.Contains($"إجمالي الوزن: {lines.Sum(x => x.Weight):N3}") &&
              summary.Contains($"إجمالي الحجم: {lines.Sum(x => x.Volume):N3}"), "Manifest snapshot totals match bound lines");

        // Rebind after BeginPrint to prove in-flight print state is immutable.
        var next = Fixture("TEST-MANIFEST-SECOND", new[] { lines[0] with { Quantity = 999m, LoadStatus = "SECOND-JOB" } });
        manifest.Bind(next);
        check(Get<string>(manifest, "_printBody") == body && Get<string>(manifest, "_printTitle") == title &&
              Get<string>(manifest, "_printSummary") == summary, "Rebinding does not change frozen print snapshot");

        using var bitmap = new Bitmap(850, 1100);
        using var graphics = Graphics.FromImage(bitmap);
        int pages = 0;
        var consumed = new StringBuilder();
        bool hasMore;
        do
        {
            int before = Get<int>(manifest, "_printOffset");
            var args = new PrintPageEventArgs(graphics, new Rectangle(50, 50, 750, 1000),
                new Rectangle(0, 0, 850, 1100), new PageSettings());
            Call(manifest, "PrintPage", args);
            int after = Get<int>(manifest, "_printOffset");
            check(after > before && after <= body.Length, $"Manifest page {pages + 1} advances within body bounds");
            consumed.Append(body.AsSpan(before, after - before));
            hasMore = args.HasMorePages;
            check(hasMore == (after < body.Length), $"Manifest page {pages + 1} continuation matches unconsumed body");
            if (++pages > 1000) throw new InvalidOperationException("Print pagination did not terminate.");
        } while (hasMore);
        check(pages > 1 && consumed.ToString() == body && consumed.ToString().EndsWith(lastMarker),
            "Multipage manifest consumes complete long body including final marker");
        check(Get<int>(manifest, "_printPageNumber") == pages, "Manifest page counter matches generated pages");
        check(Get<string>(manifest, "_printSummary") == summary, "Original totals remain frozen across all pages");
        var restart = new PrintEventArgs();
        Call(manifest, "BeginPrint", restart);
        check(!restart.Cancel && Get<int>(manifest, "_printOffset") == 0 && Get<int>(manifest, "_printPageNumber") == 0,
            "New print job resets offset and page counter");
        check(Get<string>(manifest, "_printTitle").Contains(next.ManifestNo) &&
              Get<string>(manifest, "_printSummary").Contains($"إجمالي الكمية: {999m:N3}") &&
              Get<string>(manifest, "_printBody").EndsWith("SECOND-JOB"), "Next print job captures new bound version and totals");
        Console.WriteLine("Manifest checks validate pagination state and text consumption only; no visual or physical-printer acceptance is claimed.");
    }
}
