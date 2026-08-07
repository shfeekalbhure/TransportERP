using System.ComponentModel;
using System.Drawing.Drawing2D;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.Controls;

/// <summary>
/// الزر الموحد في TransportERP.
/// يدعم لونًا أساسيًا ولون مرور مستقلين، بينما حالة Disabled موحدة من UiTheme.
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

    [Category("TransportERP")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color HoverBackColor
    {
        get => _hoverBackColor;
        set
        {
            _hoverBackColor = value;
            FlatAppearance.MouseOverBackColor = value;
            FlatAppearance.MouseDownBackColor = value;
        }
    }

    private void ApplyDefaultStyle()
    {
        AutoSize = false;
        BackColor = _normalBackColor;
        Cursor = Cursors.Hand;
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        FlatAppearance.MouseOverBackColor = _hoverBackColor;
        FlatAppearance.MouseDownBackColor = _hoverBackColor;
        Font = UiTheme.CreateBoldFont(10F);
        ForeColor = UiTheme.ActionText;
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
        ApplyEnabledVisualState();
    }

    private void HandleEnabledChanged(object? sender, EventArgs e)
    {
        ApplyEnabledVisualState();
        Cursor = Enabled ? Cursors.Hand : Cursors.Default;
    }

    private void ApplyEnabledVisualState()
    {
        BackColor = Enabled ? _normalBackColor : UiTheme.ActionDisabledBackground;
        ForeColor = Enabled ? UiTheme.ActionText : UiTheme.ActionDisabledText;
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
