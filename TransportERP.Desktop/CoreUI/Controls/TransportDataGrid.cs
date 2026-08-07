using System.ComponentModel;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// جدول البيانات الموحد لشاشات TransportERP.
/// يطبق الهوية البصرية القياسية، ويمنع الإضافة والحذف المباشر افتراضيًا،
/// ويوفر إعدادات جاهزة لعرض البيانات المؤسسية بطريقة واضحة وآمنة.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportDataGrid : DataGridView
{
    /// <summary>
    /// إنشاء جدول بيانات بالإعدادات القياسية المعتمدة للنظام.
    /// </summary>
    public TransportDataGrid()
    {
        ApplyDefaultStyle();
        DataBindingComplete += HandleDataBindingComplete;
    }

    /// <summary>
    /// تطبيق التنسيق والسلوك الافتراضي للجدول من المقاسات المركزية.
    /// </summary>
    private void ApplyDefaultStyle()
    {
        AllowUserToAddRows = false;
        AllowUserToDeleteRows = false;
        AllowUserToOrderColumns = true;
        AllowUserToResizeRows = false;
        AutoGenerateColumns = true;
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        BackgroundColor = Color.White;
        BorderStyle = BorderStyle.None;
        CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        ColumnHeadersHeight = TransportUiMetrics.GridHeaderHeight;
        ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        Dock = DockStyle.Fill;
        EnableHeadersVisualStyles = false;
        GridColor = Color.FromArgb(226, 232, 240);
        MultiSelect = false;
        ReadOnly = true;
        RightToLeft = RightToLeft.Yes;
        RowHeadersVisible = false;
        RowTemplate.Height = TransportUiMetrics.GridRowHeight;
        SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        DefaultCellStyle = new DataGridViewCellStyle
        {
            Alignment = DataGridViewContentAlignment.MiddleRight,
            BackColor = Color.White,
            Font = UiTheme.CreateRegularFont(9.5F),
            ForeColor = UiTheme.HeadingText,
            Padding = new Padding(TransportUiMetrics.GridCellHorizontalPadding, 0, TransportUiMetrics.GridCellHorizontalPadding, 0),
            SelectionBackColor = Color.FromArgb(228, 238, 255),
            SelectionForeColor = UiTheme.HeadingText,
            WrapMode = DataGridViewTriState.False
        };

        AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(248, 250, 252),
            ForeColor = UiTheme.HeadingText,
            SelectionBackColor = Color.FromArgb(228, 238, 255),
            SelectionForeColor = UiTheme.HeadingText
        };

        ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            Alignment = DataGridViewContentAlignment.MiddleCenter,
            BackColor = Color.FromArgb(241, 245, 249),
            Font = UiTheme.CreateBoldFont(9.5F),
            ForeColor = UiTheme.HeadingText,
            Padding = new Padding(TransportUiMetrics.GridCellHorizontalPadding),
            SelectionBackColor = Color.FromArgb(241, 245, 249),
            SelectionForeColor = UiTheme.HeadingText,
            WrapMode = DataGridViewTriState.True
        };
    }

    /// <summary>
    /// تحميل مصدر بيانات جديد داخل الجدول مع مسح التحديد السابق.
    /// </summary>
    public void BindData(object? dataSource)
    {
        DataSource = null;
        DataSource = dataSource;
        ClearSelection();
    }

    /// <summary>
    /// إخفاء عمود محدد باستخدام اسم الخاصية البرمجية المرتبطة به.
    /// </summary>
    public void HideColumn(string columnName)
    {
        if (string.IsNullOrWhiteSpace(columnName))
        {
            return;
        }

        if (Columns.Contains(columnName))
        {
            Columns[columnName].Visible = false;
        }
    }

    /// <summary>
    /// تغيير عنوان عمود مع إبقاء الاسم البرمجي كما هو.
    /// </summary>
    public void SetArabicHeader(string columnName, string arabicHeaderText)
    {
        if (string.IsNullOrWhiteSpace(columnName) || string.IsNullOrWhiteSpace(arabicHeaderText))
        {
            return;
        }

        if (Columns.Contains(columnName))
        {
            Columns[columnName].HeaderText = arabicHeaderText.Trim();
        }
    }

    /// <summary>
    /// الحصول على العنصر المرتبط بالصف المحدد حاليًا.
    /// </summary>
    public TItem? GetSelectedItem<TItem>()
    {
        if (CurrentRow?.DataBoundItem is TItem item)
        {
            return item;
        }

        return default;
    }

    private void HandleDataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        ClearSelection();

        foreach (DataGridViewColumn column in Columns)
        {
            column.SortMode = DataGridViewColumnSortMode.Automatic;
        }
    }
}
