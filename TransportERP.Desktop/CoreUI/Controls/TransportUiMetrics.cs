namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// المقاسات الموحدة لجميع المكونات المشتركة في واجهات TransportERP.
/// جميع الشاشات تعتمد هذه القيم حتى يبقى التصميم متناسقًا وقابلًا للتعديل من مكان واحد.
/// القيم تقريبية على أساس 96 DPI.
/// </summary>
internal static class TransportUiMetrics
{
    // المقاسات الأساسية بالمليمتر تقريبًا.
    internal const int Container12Mm = 45;
    internal const int Container10Mm = 38;
    internal const int Control9Mm = 34;
    internal const int Control8Mm = 30;
    internal const int Control6Mm = 23;

    // هوامش الشاشة والحاويات.
    internal const int ScreenOuterPadding = 6;
    internal const int GroupHorizontalPadding = 6;
    internal const int GroupTopPadding = 4;
    internal const int GroupBottomPadding = 4;
    internal const int GroupVerticalPadding = GroupTopPadding + GroupBottomPadding;
    internal const int SectionGap = 6; // قرابة 1.5 مم بين الحاويات حتى لا تلتصق الحدود.

    // شريط الأوامر مصغر حتى يحافظ على مساحة الجدول.
    internal const int ToolbarHeight = 36;
    internal const int ToolbarButtonHeight = 28;
    internal const int ToolbarButtonWidth = 78;
    internal const int ToolbarButtonGap = 4;
    internal const int ToolbarVerticalPadding = (ToolbarHeight - ToolbarButtonHeight) / 2;

    // البيانات الرئيسية: الحقل 6 مم، والمسافة بين الصفوف 1.5 مم تقريبًا.
    // الحد الأعلى ثلاثة أعمدة حقول وخمسة صفوف في التبويب الرئيسي.
    // الحد الأدنى المرئي يعادل ثلاثة صفوف حتى لا تتحول المنطقة في الشاشات قليلة الحقول إلى شريط ضيق.
    internal const int MainDataDefaultGroupHeight = 230;
    internal const int MainDataMaxFieldColumns = 3;
    internal const int MainDataMinRows = 3;
    internal const int MainDataMaxRows = 5;
    internal const int MainDataControlHeight = Control6Mm;
    internal const int MainDataRowGap = 6;
    internal const int MainDataVerticalMargin = MainDataRowGap / 2;
    internal const int MainDataHorizontalMargin = 4;
    internal const int MainDataRowHeight = MainDataControlHeight + MainDataRowGap;
    internal const int MainDataMinContentHeight = MainDataRowHeight * MainDataMinRows;
    internal const int MainDataMaxContentHeight = MainDataRowHeight * MainDataMaxRows;
    internal const int MainDataMultilineMinHeight = 58;
    internal const int MainDataLabelWidth = 120;
    internal const int MainDataLabelFieldGap = 8;
    internal const int MainDataHostPadding = 4;

    // التبويب الإضافي يستخدم فقط للبيانات الثانوية/الأقل أهمية، وليس كبديل لشريط تمرير.
    internal const int AdditionalDataTabHeaderHeight = 34;

    // التبويبات: قيم مركزية لمنع تكرار Padding محلي داخل كل مجموعة.
    internal const int TabHorizontalPadding = 16;
    internal const int TabVerticalPadding = 5;
    internal const int TabContentPadding = 8;
    internal const int TabDescriptionHeight = 34;

    // حاوية الإجراءات الخاصة بالشاشة مثل إضافة عضو أو طلب موافقة.
    internal const int ActionPanelHeight = Container10Mm;
    internal const int ActionButtonHeight = Control8Mm;
    internal const int ActionButtonMinWidth = 90;
    internal const int ActionButtonGap = 4;

    // البحث والتصفية: الحاوية 10 مم، ومربع البحث 8 مم، وحقل الحالة 6 مم.
    internal const int SearchPanelHeight = Container10Mm;
    internal const int SearchControlHeight = Control8Mm;
    internal const int SearchStatusControlHeight = Control6Mm;
    internal const int SearchStatusVerticalMargin = (SearchControlHeight - SearchStatusControlHeight) / 2;
    internal const int SearchMinimumWidth = 180;
    internal const int SearchPreferredWidth = 300;
    internal const int SearchStatusLabelWidth = 54;
    internal const int SearchStatusWidth = 140;

    // التنقل: الحاوية 10 مم والأزرار 8 مم.
    internal const int PaginationHeight = Container10Mm;
    internal const int PaginationButtonHeight = Control8Mm;

    // معلومات الإنشاء والتعديل: الحاوية 10 مم والمحتوى 8 مم.
    internal const int AuditPanelHeight = Container10Mm;
    internal const int AuditContentHeight = Control8Mm;

    // الإشعارات: الحاوية 12 مم والمحتوى 9 مم.
    internal const int AlertBarHeight = Container12Mm;
    internal const int AlertContentHeight = Control9Mm;
    internal const int AlertHorizontalPadding = 8;
    internal const int AlertVerticalPadding = (AlertBarHeight - AlertContentHeight) / 2;

    // الجدول: ارتفاعات مؤسسية مضغوطة وواضحة.
    internal const int GridHeaderHeight = 34;
    internal const int GridRowHeight = 32;
    internal const int GridCellHorizontalPadding = 6;

    // ارتفاع GroupBox = ارتفاع المحتوى + مساحة العنوان + Padding الفعلي أعلى وأسفل.
    internal const int GroupBoxHeaderSpace = 20;
    internal const int SearchGroupHeight = SearchPanelHeight + GroupBoxHeaderSpace + GroupVerticalPadding;
    internal const int AuditGroupHeight = AuditPanelHeight + GroupBoxHeaderSpace + GroupVerticalPadding;
    internal const int AlertGroupHeight = AlertBarHeight + GroupBoxHeaderSpace + GroupVerticalPadding;

    // الصف العلوي يضم الإشعارات والتنقل معًا.
    internal const int TopUtilityRowHeight = AlertGroupHeight;

    internal const int CompactPadding = 4;
    internal const int CompactGap = 6;

    /// <summary>
    /// يحسب الارتفاع الكامل لحاوية البيانات من ارتفاع محتواها الفعلي.
    /// الحافة العليا تبقى ثابتة، والزيادة تكون إلى الأسفل فقط حتى خمسة صفوف.
    /// يحافظ على حد أدنى مريح يعادل ثلاثة صفوف حتى في الشاشات قليلة الحقول.
    /// أي محتوى يتجاوز السعة ينتقل إلى تبويب إضافي بدل تمديد الحاوية أكثر أو إضافة Scroll.
    /// </summary>
    internal static int CalculateMainDataGroupHeight(int contentHeight)
    {
        var boundedContentHeight = Math.Clamp(
            contentHeight,
            MainDataMinContentHeight,
            MainDataMaxContentHeight);

        return boundedContentHeight
            + GroupBoxHeaderSpace
            + GroupVerticalPadding
            + (MainDataHostPadding * 2);
    }
}
