using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace TransportERP;

internal sealed class RequiredComboAppearance
{
    private static readonly ConditionalWeakTable<ComboBox, RequiredComboAppearance> Editors = new();
    internal static bool Attach(ComboBox combo)
    {
        if (Editors.TryGetValue(combo, out _)) return true;
        if (combo.DrawMode != DrawMode.Normal) return false; // Existing custom rendering belongs to its screen owner.
        Editors.Add(combo, new(combo));
        return true;
    }
    private RequiredComboAppearance(ComboBox combo)
    {
        combo.DrawMode = DrawMode.OwnerDrawFixed;
        void SizeItem() => combo.ItemHeight = Math.Max(combo.ItemHeight, combo.Font.Height + 2);
        SizeItem();combo.FontChanged += (_, _) => SizeItem();
        combo.DrawItem += (_, e) =>
        {
            if (e.Bounds.Width <= 0 || e.Bounds.Height <= 0) return;
            bool closed = (e.State & DrawItemState.ComboBoxEdit) != 0;
            bool selected = (e.State & DrawItemState.Selected) != 0 && (!closed || combo.Focused);
            // Disabled remains noninteractive with grey text/native chrome; the
            // explicit requirement background still identifies the field rule.
            Color back = combo.Enabled && selected ? SystemColors.Highlight : combo.BackColor;
            Color fore = !combo.Enabled ? SystemColors.GrayText : selected ? SystemColors.HighlightText : combo.ForeColor;
            using var brush = new SolidBrush(back);e.Graphics.FillRectangle(brush,e.Bounds);
            string text = e.Index >= 0 && e.Index < combo.Items.Count ? combo.GetItemText(combo.Items[e.Index]) : combo.Text;
            var flags = TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis;
            if (combo.RightToLeft == RightToLeft.Yes) flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            TextRenderer.DrawText(e.Graphics,text,e.Font ?? combo.Font,Rectangle.Inflate(e.Bounds,-2,0),fore,flags);
            if ((e.State & DrawItemState.Focus) != 0) e.DrawFocusRectangle();
        };
    }
}
