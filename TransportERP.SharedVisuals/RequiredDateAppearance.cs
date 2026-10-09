using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TransportERP;

// Paint only the idle date text area. Native checkbox, dropdown, focused segment
// editing, accessibility, Value/Checked, events and calendar remain the original control.
internal sealed class RequiredDateAppearance : NativeWindow
{
    private static readonly ConditionalWeakTable<DateTimePicker, RequiredDateAppearance> Windows = new();
    private readonly DateTimePicker editor;
    private RequiredDateAppearance(DateTimePicker editor)
    {
        this.editor = editor;
        editor.HandleCreated += (_, _) => AssignHandle(editor.Handle);
        editor.HandleDestroyed += (_, _) => ReleaseHandle();
        editor.Disposed += (_, _) => ReleaseHandle();
        editor.ValueChanged += (_, _) => editor.Invalidate();
        editor.GotFocus += (_, _) => editor.Invalidate();
        editor.LostFocus += (_, _) => editor.Invalidate();
        editor.BackColorChanged += (_, _) => editor.Invalidate();
        editor.EnabledChanged += (_, _) => editor.Invalidate();
        if (editor.IsHandleCreated) AssignHandle(editor.Handle);
    }
    internal static void Attach(DateTimePicker editor) => Windows.GetValue(editor, c => new(c));

    protected override void WndProc(ref Message message)
    {
        int kind = message.Msg;
        nint target = message.WParam;
        base.WndProc(ref message);
        if (kind is not (0x000F or 0x0317 or 0x0318) || editor.IsDisposed || editor.ContainsFocus)
            return; // WM_PAINT / WM_PRINT / WM_PRINTCLIENT; retain native focused-segment editing.
        var info = new PickerInfo { Size = (uint)Marshal.SizeOf<PickerInfo>() };
        SendMessageW(Handle, 0x100E, 0, ref info); // DTM_GETDATETIMEPICKERINFO
        var textArea = Rectangle.Inflate(editor.ClientRectangle, -2, -2);
        Trim(ref textArea, info.Button);
        if (editor.ShowCheckBox) Trim(ref textArea, info.Check);
        if (textArea.Width <= 0 || textArea.Height <= 0) return;
        using var graphics = kind != 0x000F && target != 0 ? Graphics.FromHdc(target) : editor.CreateGraphics();
        using var brush = new SolidBrush(editor.BackColor);
        graphics.FillRectangle(brush, textArea);
        var flags = TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis;
        if (editor.RightToLeft == RightToLeft.Yes) flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
        TextRenderer.DrawText(graphics, editor.Text, editor.Font, textArea,
            !editor.Enabled || editor.ShowCheckBox && !editor.Checked ? SystemColors.GrayText : editor.ForeColor, flags);
    }
    private static void Trim(ref Rectangle area, NativeRect occupied)
    {
        if (occupied.Right <= occupied.Left) return;
        if ((occupied.Left + occupied.Right) / 2 < area.Left + area.Width / 2)
        { int right = area.Right; area.X = Math.Max(area.Left, occupied.Right + 2); area.Width = right - area.Left; }
        else area.Width = Math.Min(area.Right, occupied.Left - 2) - area.Left;
    }
    [StructLayout(LayoutKind.Sequential)]
    private struct NativeRect { public int Left, Top, Right, Bottom; }
    [StructLayout(LayoutKind.Sequential)]
    private struct PickerInfo
    {
        public uint Size;
        public NativeRect Check;
        public uint CheckState;
        public NativeRect Button;
        public uint ButtonState;
        public nint Edit, UpDown, DropDown;
    }
    [DllImport("user32.dll", ExactSpelling = true)]
    private static extern nint SendMessageW(nint window, uint message, nint wParam, ref PickerInfo info);
}
