using System.ComponentModel;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// شريط الحالة الموحد لنظام TransportERP.
/// يعرض بيانات بيئة العمل الحالية، ويدعم وضعًا مختصرًا خاصًا بشاشة الدخول.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportStatusBar : UserControl
{
    private readonly TableLayoutPanel _layout = new();
    private readonly Label _companyLabel = CreateStatusLabel();
    private readonly Label _branchLabel = CreateStatusLabel();
    private readonly Label _fiscalYearLabel = CreateStatusLabel();
    private readonly Label _financialPeriodLabel = CreateStatusLabel();
    private readonly Label _userLabel = CreateStatusLabel();
    private readonly Label _roleLabel = CreateStatusLabel();
    private readonly Label _connectionLabel = CreateStatusLabel();
    private readonly Label _environmentLabel = CreateStatusLabel();
    private readonly Label _versionLabel = CreateStatusLabel();

    /// <summary>
    /// إنشاء شريط الحالة وتطبيق الهوية البصرية المعتمدة.
    /// </summary>
    public TransportStatusBar()
    {
        InitializeLayout();
        ApplyDefaultValues();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string CompanyName
    {
        get => GetValue(_companyLabel, "الشركة");
        set => SetValue(_companyLabel, "الشركة", value);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string BranchName
    {
        get => GetValue(_branchLabel, "الفرع");
        set => SetValue(_branchLabel, "الفرع", value);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string FiscalYear
    {
        get => GetValue(_fiscalYearLabel, "السنة");
        set => SetValue(_fiscalYearLabel, "السنة", value);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string FinancialPeriod
    {
        get => GetValue(_financialPeriodLabel, "الفترة");
        set => SetValue(_financialPeriodLabel, "الفترة", value);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string CurrentUser
    {
        get => GetValue(_userLabel, "المستخدم");
        set => SetValue(_userLabel, "المستخدم", value);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string CurrentRole
    {
        get => GetValue(_roleLabel, "الدور");
        set => SetValue(_roleLabel, "الدور", value);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string EnvironmentName
    {
        get => GetValue(_environmentLabel, "البيئة");
        set => SetValue(_environmentLabel, "البيئة", value);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string SystemVersion
    {
        get => GetValue(_versionLabel, "الإصدار");
        set => SetValue(_versionLabel, "الإصدار", value);
    }

    /// <summary>
    /// تحويل الشريط إلى الوضع المختصر الخاص بشاشة الدخول.
    /// يعرض الشركة والفرع والسنة والاتصال والإصدار فقط.
    /// </summary>
    public void UseLoginCompactMode()
    {
        _financialPeriodLabel.Visible = false;
        _userLabel.Visible = false;
        _roleLabel.Visible = false;
        _environmentLabel.Visible = false;

        _layout.ColumnStyles.Clear();
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0F));
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0F));
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0F));
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0F));
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
    }

    /// <summary>
    /// تحديث حالة الاتصال مع الخادم.
    /// </summary>
    public void SetConnectionStatus(bool isConnected, string? details = null)
    {
        var statusText = isConnected ? "متصل" : "غير متصل";
        var detailText = string.IsNullOrWhiteSpace(details)
            ? statusText
            : $"{statusText} - {details.Trim()}";

        _connectionLabel.Text = $"الاتصال: {detailText}";
        _connectionLabel.ForeColor = isConnected
            ? Color.FromArgb(22, 130, 82)
            : Color.FromArgb(190, 52, 52);
    }

    /// <summary>
    /// تحديث جميع بيانات شريط الحالة دفعة واحدة.
    /// </summary>
    public void UpdateContext(
        string? companyName,
        string? branchName,
        string? fiscalYear,
        string? financialPeriod,
        string? currentUser,
        string? currentRole,
        string? environmentName,
        string? systemVersion,
        bool isConnected)
    {
        CompanyName = companyName ?? string.Empty;
        BranchName = branchName ?? string.Empty;
        FiscalYear = fiscalYear ?? string.Empty;
        FinancialPeriod = financialPeriod ?? string.Empty;
        CurrentUser = currentUser ?? string.Empty;
        CurrentRole = currentRole ?? string.Empty;
        EnvironmentName = environmentName ?? string.Empty;
        SystemVersion = systemVersion ?? string.Empty;
        SetConnectionStatus(isConnected);
    }

    private void InitializeLayout()
    {
        AutoSize = false;
        BackColor = Color.White;
        BorderStyle = BorderStyle.FixedSingle;
        Dock = DockStyle.Bottom;
        Font = UiTheme.CreateRegularFont(9F);
        Height = 38;
        MinimumSize = new Size(0, 38);
        Padding = new Padding(8, 3, 8, 3);
        RightToLeft = RightToLeft.Yes;

        _layout.BackColor = Color.Transparent;
        _layout.ColumnCount = 9;
        _layout.Dock = DockStyle.Fill;
        _layout.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
        _layout.RowCount = 1;
        _layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        for (var index = 0; index < 9; index++)
        {
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.111F));
        }

        _layout.Controls.Add(_companyLabel, 0, 0);
        _layout.Controls.Add(_branchLabel, 1, 0);
        _layout.Controls.Add(_fiscalYearLabel, 2, 0);
        _layout.Controls.Add(_financialPeriodLabel, 3, 0);
        _layout.Controls.Add(_userLabel, 4, 0);
        _layout.Controls.Add(_roleLabel, 5, 0);
        _layout.Controls.Add(_connectionLabel, 6, 0);
        _layout.Controls.Add(_environmentLabel, 7, 0);
        _layout.Controls.Add(_versionLabel, 8, 0);

        Controls.Add(_layout);
    }

    private void ApplyDefaultValues()
    {
        CompanyName = "غير محددة";
        BranchName = "غير محدد";
        FiscalYear = DateTime.Today.Year.ToString();
        FinancialPeriod = "غير محددة";
        CurrentUser = "غير مسجل";
        CurrentRole = "غير محدد";
        EnvironmentName = "TransportERP API";
        SystemVersion = "1.0.0";
        SetConnectionStatus(false);
    }

    private static Label CreateStatusLabel()
    {
        return new Label
        {
            AutoEllipsis = true,
            Dock = DockStyle.Fill,
            Font = UiTheme.CreateRegularFont(8.5F),
            ForeColor = UiTheme.SecondaryText,
            Margin = new Padding(2),
            TextAlign = ContentAlignment.MiddleCenter
        };
    }

    private static void SetValue(Label label, string title, string? value)
    {
        var displayValue = string.IsNullOrWhiteSpace(value) ? "-" : value.Trim();
        label.Text = $"{title}: {displayValue}";
    }

    private static string GetValue(Label label, string title)
    {
        var prefix = $"{title}: ";
        return label.Text.StartsWith(prefix, StringComparison.Ordinal)
            ? label.Text[prefix.Length..]
            : label.Text;
    }
}
