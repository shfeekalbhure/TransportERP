using System.ComponentModel;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// قسم بيانات متكيف يفصل الحقول الأساسية عن الحقول الثانوية تلقائيًا.
/// لا يستخدم شريط تمرير: الحقول الأساسية تظهر أولًا، وعند وجود بيانات إضافية
/// ينشئ تبويبًا ثانيًا باسم "بيانات إضافية" مع إبقاء الحد الأعلى ثلاثة أعمدة حقول.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportAdaptiveDataSection : UserControl
{
    private readonly TabControl _tabs = new();
    private readonly TabPage _primaryTab = new("البيانات الرئيسية");
    private readonly TabPage _additionalTab = new("بيانات إضافية");
    private readonly TransportDataEntryPanel _primaryPanel = new();
    private readonly TransportDataEntryPanel _additionalPanel = new();

    private int _primaryFieldCount;
    private int _additionalFieldCount;

    public TransportAdaptiveDataSection()
    {
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Dock = DockStyle.Top;
        RightToLeft = RightToLeft.Yes;
        Margin = Padding.Empty;
        Padding = Padding.Empty;

        ConfigurePanel(_primaryPanel);
        ConfigurePanel(_additionalPanel);
        ConfigureTabs();
        ShowPrimaryOnly();
    }

    [Browsable(false)]
    public TransportDataEntryPanel PrimaryFields => _primaryPanel;

    [Browsable(false)]
    public TransportDataEntryPanel AdditionalFields => _additionalPanel;

    /// <summary>
    /// يضيف حقلًا أساسيًا يجب أن يبقى في الواجهة الأولى لأنه مهم للاستخدام اليومي.
    /// </summary>
    public void AddPrimaryField(string labelText, Control editor)
    {
        _primaryPanel.AddField(labelText, editor, _primaryFieldCount++);
        RefreshPreferredHeight();
    }

    /// <summary>
    /// يضيف حقلًا ثانويًا أو أقل استخدامًا. عند أول حقل إضافي تتحول المنطقة إلى تبويبين
    /// بدل زيادة ازدحام شاشة البيانات الرئيسية.
    /// </summary>
    public void AddAdditionalField(string labelText, Control editor)
    {
        _additionalPanel.AddField(labelText, editor, _additionalFieldCount++);

        if (_additionalFieldCount == 1)
        {
            ShowTabbedLayout();
        }

        RefreshPreferredHeight();
    }

    /// <summary>
    /// إضافة موحدة تسمح للمستدعي بتحديد أهمية الحقل صراحةً.
    /// القرار الوظيفي لما هو أساسي أو إضافي يبقى في تعريف الشاشة وليس داخل CoreUI.
    /// </summary>
    public void AddField(string labelText, Control editor, TransportFieldImportance importance)
    {
        if (importance == TransportFieldImportance.Additional)
        {
            AddAdditionalField(labelText, editor);
            return;
        }

        AddPrimaryField(labelText, editor);
    }

    public override Size GetPreferredSize(Size proposedSize)
    {
        var contentHeight = _additionalFieldCount == 0
            ? _primaryPanel.PreferredContentHeight
            : Math.Max(_primaryPanel.PreferredContentHeight, _additionalPanel.PreferredContentHeight)
              + TransportUiMetrics.AdditionalDataTabHeaderHeight;

        return new Size(proposedSize.Width, Math.Max(TransportUiMetrics.MainDataRowHeight, contentHeight));
    }

    private static void ConfigurePanel(TransportDataEntryPanel panel)
    {
        panel.FieldColumnCount = TransportUiMetrics.MainDataMaxFieldColumns;
        panel.Dock = DockStyle.Top;
        panel.Margin = Padding.Empty;
        panel.Padding = Padding.Empty;
    }

    private void ConfigureTabs()
    {
        _tabs.Dock = DockStyle.Top;
        _tabs.RightToLeft = RightToLeft.Yes;
        _tabs.RightToLeftLayout = true;
        _tabs.Multiline = false;
        _tabs.Padding = new Point(TransportUiMetrics.TabHorizontalPadding, TransportUiMetrics.TabVerticalPadding);
        _tabs.Margin = Padding.Empty;

        ConfigureTabPage(_primaryTab, _primaryPanel);
        ConfigureTabPage(_additionalTab, _additionalPanel);
    }

    private static void ConfigureTabPage(TabPage page, Control content)
    {
        page.BackColor = TransportERP.Desktop.Themes.UiTheme.SurfaceBackground;
        page.RightToLeft = RightToLeft.Yes;
        page.Padding = new Padding(TransportUiMetrics.TabContentPadding);
        content.Dock = DockStyle.Top;
        page.Controls.Add(content);
    }

    private void ShowPrimaryOnly()
    {
        Controls.Clear();
        _primaryTab.Controls.Remove(_primaryPanel);
        _primaryPanel.Dock = DockStyle.Top;
        Controls.Add(_primaryPanel);
        RefreshPreferredHeight();
    }

    private void ShowTabbedLayout()
    {
        Controls.Clear();

        if (!_primaryTab.Controls.Contains(_primaryPanel))
        {
            _primaryTab.Controls.Add(_primaryPanel);
        }

        if (!_additionalTab.Controls.Contains(_additionalPanel))
        {
            _additionalTab.Controls.Add(_additionalPanel);
        }

        _tabs.TabPages.Clear();
        _tabs.TabPages.Add(_primaryTab);
        _tabs.TabPages.Add(_additionalTab);
        Controls.Add(_tabs);
        RefreshPreferredHeight();
    }

    private void RefreshPreferredHeight()
    {
        var preferredHeight = GetPreferredSize(new Size(Width, 0)).Height;
        Height = preferredHeight;
        MinimumSize = new Size(0, preferredHeight);

        if (_additionalFieldCount > 0)
        {
            _tabs.Height = preferredHeight;
        }

        PerformLayout();
        Parent?.PerformLayout();
    }
}

/// <summary>
/// أهمية الحقل من منظور العرض: الأساسي يبقى في التبويب الأول، والإضافي يذهب للتبويب الثانوي.
/// </summary>
public enum TransportFieldImportance
{
    Primary,
    Additional
}
