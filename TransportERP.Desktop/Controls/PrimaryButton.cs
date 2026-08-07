using System.ComponentModel;
using System.Drawing.Drawing2D;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.Controls;

/// <summary>
/// الزر الموحد في TransportERP.
/// يدعم لونًا أساسيًا ولون مرور مستقلين حتى نستطيع تلوين الحفظ والتعديل والحذف دون إنشاء زر جديد لكل عملية.
/// </summary>
[ToolboxItem(true)]
public sealed class PrimaryButton : Button
{
    private int _cornerRadius = 12;
    private Color _normalBackColor = UiTheme.PrimaryBlue;
    private Color _hoverBackColor = UiTheme.PrimaryBlueHover;

    public PrimaryButton()
    {
        ApplyDefaultStyle();
        MouseEnter += HandleMouseEnter;
        MouseLeave += HandleMouseLeave;
        EnabledChanged += HandleEnabledChanged;
        Resize += (_, _) => UpdateRoundedRegion();
    }

    [Category("TransportERP")]
    [Description("نصف قطر استدارة حواف الزر بالبكسل.")]
    [DefaultValue(12)]
    public int CornerRadius
    {
        get => _cornerRadius;
        set
        {
            _cornerRadius = Math.Max(0, value);
            UpdateRoundedRegion();
            Invalidate();
        }
    }

    /// <summary>
    /// لون الزر في الحالة الطبيعية.
    /// DesignerSerializationVisibility يوضح لمصمم WinForms أن هذه الخاصية قابلة للحفظ في Designer.
    /// </summary>
    [Category("TransportERP")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color NormalBackColor
    {
        get => _normalBackColor;
        set
        {
            _normalBackColor = value;
            if (Enabled) BackColor = value;
        }
    }

    /// <summary>
    /// لون الزر عند مرور مؤشر الفأرة.
    /// </summary>
    [Category("TransportERP")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color HoverBackColor
    {
        get => _hoverBackColor;
        set => _hoverBackColor = value;
    }

    private void ApplyDefaultStyle()
    {
        AutoSize = false;
        BackColor = _normalBackColor;
        Cursor = Cursors.Hand;
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        Font = UiTheme.CreateBoldFont(10F);
        ForeColor = Color.White;
        Height = 34;
        MinimumSize = new Size(88, 34);
        Padding = new Padding(10, 0, 10, 0);
        TextAlign = ContentAlignment.MiddleCenter;
        UseVisualStyleBackColor = false;
    }

    private void HandleMouseEnter(object? sender, EventArgs e)
    {
        if (Enabled) BackColor = _hoverBackColor;
    }

    private void HandleMouseLeave(object? sender, EventArgs e)
    {
        BackColor = Enabled ? _normalBackColor : SystemColors.ControlDark;
    }

    private void HandleEnabledChanged(object? sender, EventArgs e)
    {
        BackColor = Enabled ? _normalBackColor : SystemColors.ControlDark;
        Cursor = Enabled ? Cursors.Hand : Cursors.Default;
    }

    private void UpdateRoundedRegion()
    {
        if (Width <= 0 || Height <= 0) return;

        if (_cornerRadius <= 0)
        {
            Region?.Dispose();
            Region = null;
            return;
        }

        var diameter = Math.Min(_cornerRadius * 2, Math.Min(Width, Height));
        var bounds = new Rectangle(0, 0, Width, Height);
        using var path = new GraphicsPath();
        path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
        path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
        path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();

        Region?.Dispose();
        Region = new Region(path);
    }
}
