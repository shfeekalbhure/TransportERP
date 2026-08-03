using System.ComponentModel;
using System.Drawing.Drawing2D;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.Controls;

/// <summary>
/// زر الإجراء الرئيسي الموحد في نظام TransportERP.
/// يستخدم في العمليات الأساسية مثل الدخول والحفظ والاعتماد والتنفيذ.
/// </summary>
[ToolboxItem(true)]
public sealed class PrimaryButton : Button
{
    private int _cornerRadius = 12;

    /// <summary>
    /// إنشاء زر رئيسي بالهوية البصرية المعتمدة للنظام.
    /// </summary>
    public PrimaryButton()
    {
        ApplyDefaultStyle();

        MouseEnter += HandleMouseEnter;
        MouseLeave += HandleMouseLeave;
        EnabledChanged += HandleEnabledChanged;
        Resize += (_, _) => UpdateRoundedRegion();
    }

    /// <summary>
    /// نصف قطر استدارة حواف الزر بالبكسل.
    /// </summary>
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
    /// تطبيق الإعدادات الافتراضية للزر الموحد.
    /// </summary>
    private void ApplyDefaultStyle()
    {
        AutoSize = false;
        BackColor = UiTheme.PrimaryBlue;
        Cursor = Cursors.Hand;
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        Font = UiTheme.CreateBoldFont(11F);
        ForeColor = Color.White;
        Height = 44;
        MinimumSize = new Size(110, 40);
        Padding = new Padding(14, 0, 14, 0);
        TextAlign = ContentAlignment.MiddleCenter;
        UseVisualStyleBackColor = false;
    }

    /// <summary>
    /// تغيير لون الزر عند مرور مؤشر الفأرة فوقه.
    /// </summary>
    private void HandleMouseEnter(object? sender, EventArgs e)
    {
        if (Enabled)
        {
            BackColor = UiTheme.PrimaryBlueHover;
        }
    }

    /// <summary>
    /// إعادة لون الزر إلى حالته الطبيعية عند مغادرة مؤشر الفأرة.
    /// </summary>
    private void HandleMouseLeave(object? sender, EventArgs e)
    {
        BackColor = Enabled
            ? UiTheme.PrimaryBlue
            : SystemColors.ControlDark;
    }

    /// <summary>
    /// تحديث مظهر الزر عند تفعيله أو تعطيله.
    /// </summary>
    private void HandleEnabledChanged(object? sender, EventArgs e)
    {
        BackColor = Enabled
            ? UiTheme.PrimaryBlue
            : SystemColors.ControlDark;

        Cursor = Enabled
            ? Cursors.Hand
            : Cursors.Default;
    }

    /// <summary>
    /// تطبيق الحواف المستديرة على الزر بحسب الحجم الحالي.
    /// </summary>
    private void UpdateRoundedRegion()
    {
        if (Width <= 0 || Height <= 0)
        {
            return;
        }

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
