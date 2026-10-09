using System.Net;
using System.Text;

namespace TransportERP.EmptyForms;

/// <summary>A self-contained, script-free RTL receipt; all document values are HTML encoded.</summary>
public static class ReceiptPrintTemplate
{
    public sealed record Item(string Label, string Value);
    public sealed record Table(string Title, IReadOnlyList<string> Headers, IReadOnlyList<IReadOnlyList<string>> Rows);

    public static string Render(string method, string state, bool unsaved, IReadOnlyList<Item> header,
        IReadOnlyList<Table> tables, string total, string difference, string totalsMessage)
    {
        static string E(string? value) => WebUtility.HtmlEncode(value ?? "");
        var html = new StringBuilder("<!doctype html><html lang=\"ar\" dir=\"rtl\"><head><meta charset=\"utf-8\"><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"><title>سند قبض</title><style>");
        html.Append("@page{size:A4 landscape;margin:12mm}body{font-family:Tahoma,Arial,sans-serif;color:#142c42;margin:24px;font-size:12px}h1{text-align:center}h2{font-size:15px;margin-top:24px}table{border-collapse:collapse;width:100%;table-layout:fixed;margin:12px 0}th,td{border:1px solid #b9c9d5;padding:8px;word-wrap:break-word;white-space:pre-wrap;vertical-align:top}th{background:#eaf1f6}thead{display:table-header-group}tr{page-break-inside:avoid}.notice{border:2px solid #9c6700;padding:12px;text-align:center}.totals{font-weight:bold;font-size:15px}.signatures{margin-top:35px;display:flex;justify-content:space-around}@media print{body{margin:0}}")
            .Append("</style></head><body><h1>سند قبض — ").Append(E(method)).Append("</h1>")
            .Append("<p class=\"notice\">").Append(unsaved ? "مسودة / تعديلات غير محفوظة — ليست إثباتًا للحفظ أو الترحيل" : "الحالة: " + E(state))
            .Append("</p><table><tbody>");
        for (int index = 0; index < header.Count; index += 2)
        {
            html.Append("<tr>");
            foreach (var item in header.Skip(index).Take(2))
                html.Append("<th style=\"width:16%\">").Append(E(item.Label)).Append("</th><td style=\"width:34%\">").Append(E(item.Value)).Append("</td>");
            if (index + 1 == header.Count) html.Append("<th></th><td></td>");
            html.Append("</tr>");
        }
        html.Append("</tbody></table>");
        foreach (var table in tables)
        {
            html.Append("<h2>").Append(E(table.Title)).Append("</h2><table><thead><tr>");
            foreach (var heading in table.Headers) html.Append("<th>").Append(E(heading)).Append("</th>");
            html.Append("</tr></thead><tbody>");
            foreach (var row in table.Rows)
            {
                if (row.Count != table.Headers.Count) throw new ArgumentException("Receipt print column count mismatch.");
                html.Append("<tr>");
                foreach (var value in row) html.Append("<td>").Append(E(value)).Append("</td>");
                html.Append("</tr>");
            }
            html.Append("</tbody></table>");
        }
        return html.Append("<p class=\"totals\">المجموع: ").Append(E(total)).Append(" — الفارق: ").Append(E(difference))
            .Append("</p><p>").Append(E(totalsMessage)).Append("</p><div class=\"signatures\"><span>المستلم: ______________</span><span>المسلّم: ______________</span></div></body></html>").ToString();
    }
}
