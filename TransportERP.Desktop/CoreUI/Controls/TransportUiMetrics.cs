namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// المقاسات الموحدة لجميع المكونات المشتركة في واجهات TransportERP.
/// جميع الشاشات تعتمد هذه القيم حتى يبقى التصميم متناسقًا وقابلًا للتعديل من مكان واحد.
/// القيم تقريبية على أساس 96 DPI.
/// </summary>
internal static class TransportUiMetrics
{
    internal const int Container12Mm = 45;
    internal const int Container10Mm = 38;
    internal const int Control9Mm = 34;
    internal const int Control8Mm = 30;
    internal const int Control6Mm = 23;

    internal const int ScreenOuterPadding = CoreUIProperties.ContainerPadding;
    internal const int GroupHorizontalPadding = CoreUIProperties.ContainerPadding;
    internal const int GroupTopPadding = 4;
    internal const int GroupBottomPadding = 4;
    internal const int GroupVerticalPadding = GroupTopPadding + GroupBottomPadding;
    internal const int SectionGap = CoreUIProperties.RowGap;

    internal const int ToolbarHeight = 36;
    internal const int ToolbarButtonHeight = 28;
    internal const int ToolbarButtonWidth = 78;
    internal const int ToolbarButtonGap = 4;
    internal const int ToolbarVerticalPadding = (ToolbarHeight - ToolbarButtonHeight) / 2;

    // البيانات الرئيسية: الحقل 6 مم، فجوة الصفوف 1.5 مم، 3 أعمدة و5 صفوف كحد أقصى.
    // لا نفرض ارتفاعًا ثابتًا كبيرًا؛ الحد الأدنى هو صف واحد فعلي + Chrome الحاوية.
    internal const int MainDataDefaultGroupHeight = 230;
    internal const int MainDataMaxFieldColumns = CoreUIProperties.MaximumInputColumns;
    internal const int MainDataMaxRows = 5;
    internal const int MainDataControlHeight = CoreUIProperties.ControlHeight;
    internal const int MainDataRowGap = CoreUIProperties.RowGap;
    internal const int MainDataVerticalMargin = MainDataRowGap / 2;
    internal const int MainDataHorizontalMargin = CoreUIProperties.ColumnGap / 2;
    internal const int MainDataRowHeight = MainDataControlHeight + MainDataRowGap;
    internal const int MainDataGroupChromeHeight = GroupBoxHeaderSpace + GroupVerticalPadding + (MainDataHostPadding * 2);
    internal const int MainDataMinContentHeight = MainDataRowHeight;
    internal const int MainDataMinimumGroupHeight = MainDataMinContentHeight + MainDataGroupChromeHeight;
    internal const int MainDataMaxFieldContentHeight = MainDataRowHeight * MainDataMaxRows;
    internal const int AdditionalDataTabHeaderHeight = 34;
    // سقف المحتوى يسمح بخمسة صفوف فعلية، رأس التبويب وPadding، وحتى مستويين من GroupBox
    // في الشاشات المتخصصة؛ الـChrome لا يجوز أن يقتطع من مساحة الحقول.
    internal const int MainDataMaxContentHeight = MainDataMaxFieldContentHeight
        + AdditionalDataTabHeaderHeight
        + (TabContentPadding * 2)
        + ((GroupBoxHeaderSpace + GroupVerticalPadding) * 2);
    internal const int MainDataMultilineMinHeight = 58;
    internal const int MainDataLabelWidth = 120;
    internal const int MainDataLabelFieldGap = 8;
    internal const int MainDataHostPadding = 4;

    internal const int TabHorizontalPadding = 16;
    internal const int TabVerticalPadding = 5;
    internal const int TabContentPadding = 8;
    internal const int TabDescriptionHeight = 34;

    internal const int ActionPanelHeight = Container10Mm;
    internal const int ActionButtonHeight = Control8Mm;
    internal const int ActionButtonMinWidth = 90;
    internal const int ActionButtonGap = 4;

    internal const int SearchPanelHeight = Container10Mm;
    internal const int SearchControlHeight = Control8Mm;
    internal const int SearchStatusControlHeight = Control6Mm;
    internal const int SearchStatusVerticalMargin = (SearchControlHeight - SearchStatusControlHeight) / 2;
    internal const int SearchMinimumWidth = 180;
    internal const int SearchPreferredWidth = 300;
    internal const int SearchStatusLabelWidth = 54;
    internal const int SearchStatusWidth = 140;

    internal const int PaginationHeight = Container10Mm;
    internal const int PaginationButtonHeight = Control8Mm;

    internal const int AuditPanelHeight = Container10Mm;
    internal const int AuditContentHeight = Control8Mm;

    internal const int AlertBarHeight = Container12Mm;
    internal const int AlertContentHeight = Control9Mm;
    internal const int AlertHorizontalPadding = 8;
    internal const int AlertVerticalPadding = (AlertBarHeight - AlertContentHeight) / 2;

    internal const int GridHeaderHeight = CoreUIProperties.GridHeaderHeight;
    internal const int GridRowHeight = CoreUIProperties.GridRowHeight;
    internal const int GridCellHorizontalPadding = 6;

    internal const int GroupBoxHeaderSpace = 20;
    internal const int SearchGroupHeight = SearchPanelHeight + GroupBoxHeaderSpace + GroupVerticalPadding;
    internal const int AuditGroupHeight = AuditPanelHeight + GroupBoxHeaderSpace + GroupVerticalPadding;
    internal const int AlertGroupHeight = AlertBarHeight + GroupBoxHeaderSpace + GroupVerticalPadding;

    internal const int TopUtilityRowHeight = AlertGroupHeight;

    internal const int CompactPadding = 4;
    internal const int CompactGap = 6;

    /// <summary>
    /// يعيد عدد أعمدة الحقول وفق القرار الحاكم: 1–5 عمود، 6–10 عمودان، 11–15 ثلاثة أعمدة.
    /// </summary>
    internal static int ResolveMainDataFieldColumns(int fieldCount)
    {
        var count = Math.Max(0, fieldCount);
        return count switch
        {
            <= 5 => 1,
            <= 10 => 2,
            _ => MainDataMaxFieldColumns
        };
    }

    /// <summary>
    /// يحسب ارتفاع حاوية البيانات الرئيسية من المحتوى الحقيقي + Chrome الحاوية.
    /// الحافة العليا لا تتحرك، ولا يفرض حد أدنى أكبر من صف واحد، ولا يستخدم Scroll لتعويض القياس.
    /// </summary>
    internal static int CalculateMainDataGroupHeight(int contentHeight)
    {
        var boundedContentHeight = Math.Clamp(contentHeight, MainDataMinContentHeight, MainDataMaxContentHeight);
        return boundedContentHeight + MainDataGroupChromeHeight;
    }
}
