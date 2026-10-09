using System.Drawing;
using System.Windows.Forms;

namespace TransportERP;

/// <summary>Opt-in appearance only. Callers supply their existing validation decision.</summary>
public static class RequiredFieldAppearance
{
    public static readonly Color RequiredColor = Color.LightYellow;
    public static readonly Color OrdinaryColor = Color.White;

    // Unknown is deliberately not treated as a proven optional rule. The caller retains
    // its validation/error state; this result lets coverage audits distinguish it.
    public static bool Apply(Control editor, bool? required)
    {
        ArgumentNullException.ThrowIfNull(editor);
        if (editor is not (TextBoxBase or ComboBox or NumericUpDown or DateTimePicker or CheckBox or RadioButton))
            return false;
        editor.BackColor = required == true ? RequiredColor : OrdinaryColor;
        if (editor is ComboBox combo)
        {
            combo.FlatStyle = FlatStyle.Flat;
            if (!RequiredComboAppearance.Attach(combo)) return false;
        }
        if (editor is DateTimePicker date) RequiredDateAppearance.Attach(date);
        return required != null;
    }

    public static void Apply(DataGridViewColumn column, bool required)
    {
        ArgumentNullException.ThrowIfNull(column);
        column.DefaultCellStyle.BackColor = required ? RequiredColor : OrdinaryColor;
        if (column is DataGridViewComboBoxColumn combo) combo.FlatStyle = FlatStyle.Flat;
        // Selection, disabled/read-only behavior and per-row state indicators are not
        // rewritten. The screen owner must check inherited cell styles in its audit.
    }
}
