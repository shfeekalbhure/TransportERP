using System.Globalization;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;

namespace TransportERP.EmptyForms;

// General-ledger manual pp.30-31: ignore row1, then fourteen positional columns.
// This reader supports XLSX, not legacy binary XLS, macros, formulas or external data.
internal static class JournalXlsxReader
{
    private static readonly XNamespace Sheet = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
    private static readonly XNamespace Rel = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
    internal static IReadOnlyList<string[]> Read(string path)
    {
        if (!string.Equals(Path.GetExtension(path), ".xlsx", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("الصيغة المدعومة Excel .xlsx؛ احفظ ملف .xls بهذه الصيغة أولًا");
        if (new FileInfo(path).Length > 5 * 1024 * 1024) throw new InvalidOperationException("الحد الأقصى للملف 5 ميجابايت");
        using var zip = ZipFile.OpenRead(path);
        if (zip.Entries.Count > 256 || zip.Entries.Sum(e => e.Length) > 20 * 1024 * 1024)
            throw new InvalidOperationException("حجم محتويات Excel يتجاوز حد القراءة");
        if (zip.Entries.Select(e => e.FullName).Distinct(StringComparer.Ordinal).Count() != zip.Entries.Count)
            throw new InvalidOperationException("ملف Excel يحتوي أجزاء مكررة");
        XDocument Xml(string name)
        {
            var entry = zip.GetEntry(name) ?? throw new InvalidOperationException("جزء مطلوب مفقود من Excel: " + name);
            using var stream = entry.Open();
            using var reader = XmlReader.Create(stream, new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null, MaxCharactersInDocument = 20 * 1024 * 1024 });
            return XDocument.Load(reader);
        }
        var workbook = Xml("xl/workbook.xml");
        var sheets = workbook.Descendants(Sheet + "sheet").ToArray();
        if (sheets.Length != 1) throw new InvalidOperationException("استخدم ملف Excel بورقة واحدة لمنع تجاهل بيانات أوراق أخرى");
        string id = (string?)sheets[0].Attribute(Rel + "id") ?? "";
        var relationships = Xml("xl/_rels/workbook.xml.rels").Root?.Elements().ToArray() ?? [];
        string Part(XElement relationship)
        {
            if ((string?)relationship.Attribute("TargetMode") == "External") throw new InvalidOperationException("روابط Excel الخارجية غير مدعومة");
            string target = (string?)relationship.Attribute("Target") ?? "";
            var uri = new Uri(new Uri("https://xlsx.local/xl/workbook.xml"), target);
            if (uri.Host != "xlsx.local" || !uri.AbsolutePath.StartsWith("/xl/", StringComparison.Ordinal) || uri.Query.Length > 0 || uri.Fragment.Length > 0)
                throw new InvalidOperationException("مسار جزء Excel غير صالح");
            return Uri.UnescapeDataString(uri.AbsolutePath.TrimStart('/'));
        }
        var sheetRel = relationships.SingleOrDefault(r => (string?)r.Attribute("Id") == id && ((string?)r.Attribute("Type"))?.EndsWith("/worksheet", StringComparison.Ordinal) == true)
            ?? throw new InvalidOperationException("مرجع ورقة Excel غير صالح");
        if (relationships.Any(r => ((string?)r.Attribute("Type"))?.Contains("externalLink", StringComparison.Ordinal) == true))
            throw new InvalidOperationException("روابط Excel الخارجية غير مدعومة");
        var stringsRel = relationships.SingleOrDefault(r => ((string?)r.Attribute("Type"))?.EndsWith("/sharedStrings", StringComparison.Ordinal) == true);
        var strings = stringsRel == null ? [] : Xml(Part(stringsRel)).Descendants(Sheet + "si").Select(Text).ToArray();
        var worksheet = Xml(Part(sheetRel));
        if (worksheet.Descendants(Sheet + "mergeCell").Any()) throw new InvalidOperationException("الخلايا المدمجة غير مدعومة في ملف الاستيراد");
        var rows = new List<string[]>();
        int previousRow = 0;
        foreach (var row in worksheet.Descendants(Sheet + "sheetData").Elements(Sheet + "row"))
        {
            if (!int.TryParse((string?)row.Attribute("r"), out int number) || number <= previousRow || number > 1001)
                throw new InvalidOperationException("ترقيم الصفوف غير صحيح أو تجاوز الملف 1000 سطر بيانات");
            previousRow = number;
            if (number == 1) continue;
            var values = Enumerable.Repeat("", 14).ToArray();
            var seen = new HashSet<int>();
            foreach (var cell in row.Elements(Sheet + "c"))
            {
                string address = (string?)cell.Attribute("r") ?? "";
                int i = 0, column = 0;
                while (i < address.Length && address[i] is >= 'A' and <= 'Z') { column = checked(column * 26 + address[i++] - 'A' + 1); }
                if (!int.TryParse(address[i..], out int cellRow) || cellRow != number || column < 1 || !seen.Add(column))
                    throw new InvalidOperationException("عنوان خلية Excel غير صالح");
                if (cell.Element(Sheet + "f") != null) throw new InvalidOperationException("استبدل المعادلات بقيم ثابتة قبل الاستيراد");
                string type = (string?)cell.Attribute("t") ?? "n";
                string raw = (string?)cell.Element(Sheet + "v") ?? "";
                string value;
                if (type == "s")
                {
                    if (!int.TryParse(raw, out int index) || index < 0 || index >= strings.Length) throw new InvalidOperationException("مرجع نص Excel غير صالح");
                    value = strings[index];
                }
                else if (type == "inlineStr") value = Text(cell.Element(Sheet + "is") ?? new XElement("empty"));
                else if (type is "str") value = raw;
                else if (type == "n" && raw.Length == 0) value = "";
                else if (type == "n" && decimal.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal numeric))
                    value = numeric.ToString(CultureInfo.CurrentCulture);
                else throw new InvalidOperationException("نوع أو قيمة خلية Excel غير صالح؛ استخدم نصًا أو رقمًا ثابتًا");
                if (value.Length > 32767) throw new InvalidOperationException("نص خلية أطول من الحد المسموح");
                if (column > 14) { if (!string.IsNullOrEmpty(value)) throw new InvalidOperationException("بيانات خارج الأعمدة الأربعة عشر"); continue; }
                values[column - 1] = value;
            }
            if (values.Any(v => !string.IsNullOrWhiteSpace(v))) rows.Add(values);
        }
        if (rows.Count == 0) throw new InvalidOperationException("لا توجد بيانات بعد الصف الأول");
        return rows;
    }
    private static string Text(XElement node) => string.Concat(node.Elements().Select(e => e.Name == Sheet + "t" ? e.Value : e.Name == Sheet + "r" ? string.Concat(e.Elements(Sheet + "t").Select(t => t.Value)) : ""));
}
