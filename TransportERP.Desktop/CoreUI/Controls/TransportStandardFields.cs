using System.ComponentModel;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// حقول موحدة للاستخدام داخل شاشات TransportERP.
/// الهدف أن تأخذ جميع الحقول اتجاه RTL والارتفاع والمحاذاة من مكان واحد.
/// </summary>
[ToolboxItem(true)]
public class TransportTextBox : TextBox
{
    public TransportTextBox()
    {
        Height = TransportUiMetrics.MainDataControlHeight;
        RightToLeft = RightToLeft.Yes;
        TextAlign = HorizontalAlignment.Right;
    }
}

/// <summary>
/// قائمة اختيار موحدة بارتفاع واتجاه ثابتين.
/// </summary>
[ToolboxItem(true)]
public class TransportComboBox : ComboBox
{
    public TransportComboBox()
    {
        Height = TransportUiMetrics.MainDataControlHeight;
        RightToLeft = RightToLeft.Yes;
        DropDownStyle = ComboBoxStyle.DropDownList;
    }
}

/// <summary>
/// منتقي تاريخ موحد للاستخدام في شاشات النظام.
/// </summary>
[ToolboxItem(true)]
public class TransportDatePicker : DateTimePicker
{
    public TransportDatePicker()
    {
        Height = TransportUiMetrics.MainDataControlHeight;
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        Format = DateTimePickerFormat.Custom;
        CustomFormat = "yyyy/MM/dd";
    }
}

/// <summary>
/// حقل ملاحظات موحد متعدد الأسطر مع محاذاة عربية صحيحة.
/// </summary>
[ToolboxItem(true)]
public class TransportMultilineTextBox : TextBox
{
    public TransportMultilineTextBox()
    {
        Multiline = true;
        ScrollBars = ScrollBars.Vertical;
        MinimumSize = new Size(0, TransportUiMetrics.MainDataMultilineMinHeight);
        RightToLeft = RightToLeft.Yes;
        TextAlign = HorizontalAlignment.Right;
    }
}
