using System.ComponentModel;
using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>
/// GEN-016 — إعدادات التشغيل العامة والمتغيرات المشتركة.
/// الشاشة تعرض كتالوجًا مغلقًا من 24 Property مع Metadata صريحة للنطاق والتحقق.
/// لا تنفذ أي وصول مباشر للبيانات، ولا تحتوي محرك حسم Backend.
/// </summary>
public partial class UcGen016GlobalVariables : UserControl
{
    private const string ResolutionPolicyName = "NearestOverride";
    private readonly BindingList<PropertyCatalogRow> _catalogRows = new();
    private IReadOnlySet<string> _grantedPermissions = new HashSet<string>(StringComparer.Ordinal);
    private PropertyDefinition? _selectedProperty;
    private Control? _overrideEditor;

    public UcGen016GlobalVariables()
    {
        InitializeComponent();
        InitializeCatalog();
        WireEvents();
    }

    internal TransportReferenceScreenShell ScreenShell => screenShell;

    /// <summary>
    /// يطبق صلاحيات GEN-016 التي تأتي من سياق المستخدم بعد المصادقة.
    /// User يعني المستخدم الحالي فقط ولا يفتح إدارة تفضيلات مستخدم آخر.
    /// </summary>
    internal void ApplyPermissions(IEnumerable<string> permissions)
    {
        _grantedPermissions = new HashSet<string>(permissions ?? Array.Empty<string>(), StringComparer.Ordinal);
        RefreshScopeChoices();
        RefreshEditability();
    }

    private void InitializeCatalog()
    {
        foreach (var definition in PropertyCatalog)
        {
            _catalogRows.Add(new PropertyCatalogRow(
                definition.PropertyCode,
                definition.ArabicName,
                definition.Group,
                "—",
                definition.DefaultDisplayValue,
                "Built-in Default",
                "نشط"));
        }

        screenShell.Grid.DataSource = _catalogRows;
        screenShell.Pagination.SetPageInfo(1, 1, _catalogRows.Count > 0 ? 1 : 0, _catalogRows.Count, _catalogRows.Count);

        if (screenShell.Grid.Rows.Count > 0)
        {
            screenShell.Grid.Rows[0].Selected = true;
            ShowDefinition(PropertyCatalog[0]);
        }
    }

    private void WireEvents()
    {
        screenShell.Grid.SelectionChanged += (_, _) => ShowSelectedGridProperty();
        screenShell.SearchPanel.SearchTextChanged += (_, _) => ApplyCatalogFilter();
        screenShell.SearchPanel.StatusChanged += (_, _) => ApplyCatalogFilter();

        screenShell.Toolbar.NewRequested += (_, _) =>
            screenShell.AlertBar.Text = "GEN-016 كتالوج مغلق؛ لا يمكن إنشاء PropertyCode جديد.";

        screenShell.Toolbar.EditRequested += (_, _) =>
        {
            RefreshEditability();
            screenShell.AlertBar.Text = CanEditSelectedScope()
                ? "يمكن تعديل Current Override فقط ضمن النطاق والصلاحية المسموحين."
                : "لا توجد صلاحية صالحة لتعديل الـScope المحدد.";
        };

        screenShell.Toolbar.SaveRequested += (_, _) => ValidatePendingOverride();
        screenShell.Toolbar.DeleteRequested += (_, _) =>
            screenShell.AlertBar.Text = "تعريف Property لا يُحذف من GEN-016؛ إزالة Override تتطلب خدمة API معتمدة.";
        screenShell.Toolbar.DisableRequested += (_, _) =>
            screenShell.AlertBar.Text = "حالة تعريف Property ReadOnly وتدار من كتالوج النظام.";
        screenShell.Toolbar.PrintRequested += (_, _) =>
            screenShell.AlertBar.Text = "طباعة كتالوج الإعدادات تمر عبر خدمة التقارير عند توفرها.";
        screenShell.Toolbar.CloseRequested += (_, _) =>
            screenShell.AlertBar.Text = "أغلق الشاشة من تبويب مساحة العمل.";

        cmbScope.SelectedIndexChanged += (_, _) =>
        {
            UpdateScopeIdentity();
            RefreshEditability();
        };

        dtEffectiveFrom.ValueChanged += (_, _) => ValidateEffectiveDates(silent: true);
        dtEffectiveTo.ValueChanged += (_, _) => ValidateEffectiveDates(silent: true);
    }

    private void ShowSelectedGridProperty()
    {
        if (screenShell.Grid.CurrentRow?.DataBoundItem is not PropertyCatalogRow row)
        {
            return;
        }

        var definition = PropertyCatalog.FirstOrDefault(p =>
            string.Equals(p.PropertyCode, row.PropertyCode, StringComparison.Ordinal));

        if (definition is not null)
        {
            ShowDefinition(definition);
        }
    }

    private void ShowDefinition(PropertyDefinition definition)
    {
        _selectedProperty = definition;

        txtPropertyCode.Text = definition.PropertyCode;
        txtArabicName.Text = definition.ArabicName;
        txtEnglishName.Text = definition.PropertyCode;
        txtGroup.Text = definition.Group;
        txtDescription.Text = "الوصف التفصيلي غير معرف كحقل مستقل في العقد الحاكم.";
        txtValueType.Text = definition.ValueTypeDisplay;
        txtAllowedScopes.Text = string.Join("، ", definition.AllowedScopes.Select(ToArabicScope));
        txtDefaultValue.Text = definition.DefaultDisplayValue;
        txtCurrentOverride.Text = "لا توجد قيمة محملة من API";
        txtEffectiveValue.Text = definition.DefaultDisplayValue;
        txtValueSource.Text = "Built-in Default";
        txtResolutionPolicy.Text = ResolutionPolicyName;
        txtStatus.Text = "نشط";
        txtValidation.Text = definition.ValidationSummary;

        dtEffectiveFrom.Checked = false;
        dtEffectiveTo.Checked = false;
        txtReason.Clear();

        RefreshScopeChoices();
        BuildOverrideEditor(definition);
        RefreshEditability();
    }

    private void RefreshScopeChoices()
    {
        var previous = cmbScope.SelectedItem?.ToString();
        cmbScope.BeginUpdate();
        try
        {
            cmbScope.Items.Clear();
            if (_selectedProperty is null)
            {
                return;
            }

            foreach (var scope in _selectedProperty.AllowedScopes)
            {
                if (HasScopePermission(scope))
                {
                    cmbScope.Items.Add(scope);
                }
            }

            if (previous is not null && cmbScope.Items.Contains(previous))
            {
                cmbScope.SelectedItem = previous;
            }
            else if (cmbScope.Items.Count > 0)
            {
                cmbScope.SelectedIndex = 0;
            }
        }
        finally
        {
            cmbScope.EndUpdate();
        }

        UpdateScopeIdentity();
    }

    private bool HasScopePermission(string scope)
    {
        if (!_grantedPermissions.Contains("GEN016.EditSettings"))
        {
            return false;
        }

        return scope switch
        {
            "System" => _grantedPermissions.Contains("GEN016.EditSystemScope"),
            "Company" => _grantedPermissions.Contains("GEN016.EditCompanyScope"),
            "Branch" => _grantedPermissions.Contains("GEN016.EditBranchScope"),
            "User" => _grantedPermissions.Contains("GEN016.EditOwnPreferences"),
            _ => false
        };
    }

    private bool CanEditSelectedScope()
    {
        var selectedScope = cmbScope.SelectedItem?.ToString();
        return _selectedProperty is not null
            && selectedScope is not null
            && _selectedProperty.AllowedScopes.Contains(selectedScope, StringComparer.Ordinal)
            && HasScopePermission(selectedScope);
    }

    private void RefreshEditability()
    {
        var editable = CanEditSelectedScope();
        cmbScope.Enabled = _selectedProperty is not null && cmbScope.Items.Count > 0;
        pnlOverrideEditor.Enabled = editable;
        dtEffectiveFrom.Enabled = editable;
        dtEffectiveTo.Enabled = editable;
        txtReason.ReadOnly = !editable;
    }

    private void UpdateScopeIdentity()
    {
        txtScopeIdentity.Text = cmbScope.SelectedItem?.ToString() switch
        {
            "System" => "النظام",
            "Company" => "الشركة الحالية",
            "Branch" => "الفرع الحالي",
            "User" => "المستخدم الحالي فقط",
            _ => "لا يوجد Scope متاح بالصلاحيات الحالية"
        };
    }

    private void BuildOverrideEditor(PropertyDefinition definition)
    {
        pnlOverrideEditor.Controls.Clear();

        _overrideEditor = definition.ValueKind switch
        {
            PropertyValueKind.Boolean => CreateBooleanEditor(),
            PropertyValueKind.Integer or PropertyValueKind.DurationMinutes => CreateNumericEditor(definition),
            PropertyValueKind.List => CreateListEditor(definition),
            _ => new TextBox { ReadOnly = true }
        };

        _overrideEditor.Dock = DockStyle.Fill;
        _overrideEditor.RightToLeft = RightToLeft.Yes;
        pnlOverrideEditor.Controls.Add(_overrideEditor);
    }

    private static Control CreateBooleanEditor()
    {
        var combo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        combo.Items.AddRange(new object[] { "نعم", "لا" });
        return combo;
    }

    private static Control CreateNumericEditor(PropertyDefinition definition)
    {
        var number = new NumericUpDown
        {
            DecimalPlaces = definition.DecimalPlaces,
            Minimum = definition.Minimum ?? 0M,
            Maximum = definition.Maximum ?? decimal.MaxValue,
            Increment = definition.Increment ?? 1M,
            ThousandsSeparator = true,
            TextAlign = HorizontalAlignment.Right
        };
        return number;
    }

    private static Control CreateListEditor(PropertyDefinition definition)
    {
        var combo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        combo.Items.AddRange(definition.AllowedValues.Cast<object>().ToArray());
        return combo;
    }

    private void ValidatePendingOverride()
    {
        if (_selectedProperty is null)
        {
            screenShell.AlertBar.Text = "اختر Property من القائمة أولًا.";
            return;
        }

        if (!CanEditSelectedScope())
        {
            screenShell.AlertBar.Text = "الحفظ مرفوض محليًا: الصلاحية أو الـScope غير مسموح.";
            return;
        }

        if (!ValidateEffectiveDates(silent: false))
        {
            return;
        }

        if (!ValidateOverrideValue())
        {
            return;
        }

        // لا يوجد Backend في نطاق المهمة: لا نخترع Repository أو اتصال قاعدة بيانات.
        screenShell.AlertBar.Text =
            "تم التحقق من القيمة محليًا. الحفظ الفعلي يتطلب HTTP API مع تطبيق حدود الخادم والصلاحيات والتدقيق.";
    }

    private bool ValidateEffectiveDates(bool silent)
    {
        if (dtEffectiveFrom.Checked && dtEffectiveTo.Checked && dtEffectiveTo.Value < dtEffectiveFrom.Value)
        {
            if (!silent)
            {
                screenShell.AlertBar.Text = "EffectiveTo يجب أن يكون أكبر من أو مساويًا لـ EffectiveFrom.";
            }
            return false;
        }

        return true;
    }

    private bool ValidateOverrideValue()
    {
        if (_selectedProperty is null || _overrideEditor is null)
        {
            return false;
        }

        if (_overrideEditor is NumericUpDown number)
        {
            if (_selectedProperty.Minimum.HasValue && number.Value < _selectedProperty.Minimum.Value)
            {
                screenShell.AlertBar.Text = $"القيمة أقل من الحد الأدنى {_selectedProperty.Minimum}.";
                return false;
            }

            if (_selectedProperty.Maximum.HasValue && number.Value > _selectedProperty.Maximum.Value)
            {
                screenShell.AlertBar.Text = $"القيمة أكبر من الحد الأعلى {_selectedProperty.Maximum}.";
                return false;
            }
        }

        return true;
    }

    private void ApplyCatalogFilter()
    {
        // TransportDataGrid مربوط حاليًا بكتالوج محلي ثابت فقط لعرض تعريفات GEN-016.
        // التصفية الحقيقية للسجلات/Overrides تصبح Server-side عند توفير API.
        var search = screenShell.SearchPanel.SearchText?.Trim() ?? string.Empty;
        var selectedStatus = screenShell.SearchPanel.SelectedStatus;
        var textFiltered = string.IsNullOrWhiteSpace(search)
            ? PropertyCatalog
            : PropertyCatalog.Where(p =>
                p.PropertyCode.Contains(search, StringComparison.OrdinalIgnoreCase)
                || p.ArabicName.Contains(search, StringComparison.OrdinalIgnoreCase)
                || p.Group.Contains(search, StringComparison.OrdinalIgnoreCase)).ToArray();

        var filtered = selectedStatus == "موقوف"
            ? Array.Empty<PropertyDefinition>()
            : textFiltered;

        _catalogRows.RaiseListChangedEvents = false;
        _catalogRows.Clear();
        foreach (var definition in filtered)
        {
            _catalogRows.Add(new PropertyCatalogRow(
                definition.PropertyCode,
                definition.ArabicName,
                definition.Group,
                "—",
                definition.DefaultDisplayValue,
                "Built-in Default",
                "نشط"));
        }
        _catalogRows.RaiseListChangedEvents = true;
        _catalogRows.ResetBindings();
        screenShell.Pagination.SetPageInfo(1, 1, _catalogRows.Count > 0 ? 1 : 0, _catalogRows.Count, _catalogRows.Count);
    }

    private static string ToArabicScope(string scope) => scope switch
    {
        "System" => "النظام",
        "Company" => "الشركة",
        "Branch" => "الفرع",
        "User" => "المستخدم",
        _ => scope
    };

    private static readonly PropertyDefinition[] PropertyCatalog =
    {
        P("OPS.DEFAULT_PAGE_SIZE", "عدد السجلات الافتراضي للصفحة", "التشغيل العام", PropertyValueKind.Integer, "50",
            S("System","Company","Branch","User"), min:10, max:500, increment:10,
            validation:"10–500، Increment=10، Requested/Preferred فقط؛ MaximumPageSize يفرضه API."),
        P("OPS.DEFAULT_QUERY_RANGE_DAYS", "المدى الزمني الافتراضي للاستعلام", "التشغيل العام", PropertyValueKind.Integer, "30 يوم",
            S("System","Company","Branch","User"), min:1, max:366, validation:"1–366 يومًا."),
        P("OPS.REMEMBER_FILTERS", "تذكر مرشحات المستخدم", "التشغيل العام", PropertyValueKind.Boolean, "نعم",
            S("System","User")),
        P("UI.DATE_FORMAT", "تنسيق التاريخ", "اللغة والتنسيق", PropertyValueKind.List, "dd/MM/yyyy",
            S("System","Company","User"), values:new[]{"DateFormat.DMY_SLASH"},
            validation:"معرف نظام ثابت فقط؛ لا يقبل Format String حرًا."),
        P("UI.TIME_FORMAT", "تنسيق الوقت", "اللغة والتنسيق", PropertyValueKind.List, "HH:mm",
            S("System","Company","User"), values:new[]{"TimeFormat.H24_MIN"},
            validation:"معرف نظام ثابت فقط؛ لا يقبل Format String حرًا."),
        P("UI.NUMBER_FORMAT", "نمط عرض الأرقام", "اللغة والتنسيق", PropertyValueKind.List, "1,234.56",
            S("System","Company","User"), values:new[]{"NumberFormat.GROUPED_DOT_DECIMAL"},
            validation:"معرف نظام ثابت فقط؛ لا يقبل Format String حرًا."),
        P("UI.DISPLAY_DECIMAL_PLACES", "المنازل العشرية للعرض العام", "اللغة والتنسيق", PropertyValueKind.Integer, "2",
            S("System","Company","User"), min:0, max:6,
            validation:"0–6 للعرض العام فقط؛ لا يغير Precision المتخصص."),
        P("UI.ROW_DENSITY", "كثافة عرض الصفوف", "سلوك الواجهة", PropertyValueKind.List, "عادي",
            S("System","User"), values:new[]{"عادي"}),
        P("UI.SHOW_TOOLTIPS", "إظهار التلميحات", "سلوك الواجهة", PropertyValueKind.Boolean, "نعم",
            S("System","User")),
        P("UI.RESTORE_LAST_WORKSPACE", "استعادة مساحة العمل السابقة", "سلوك الواجهة", PropertyValueKind.Boolean, "لا",
            S("System","User")),
        P("SCOPE.REMEMBER_LAST_COMPANY_ENABLED", "تفعيل تذكر آخر شركة", "الشركات والفروع", PropertyValueKind.Boolean, "نعم",
            S("User"), validation:"Boolean فقط؛ CompanyId الأخير User Runtime/Preference State خارج GEN-016."),
        P("SCOPE.REMEMBER_LAST_BRANCH_ENABLED", "تفعيل تذكر آخر فرع", "الشركات والفروع", PropertyValueKind.Boolean, "نعم",
            S("User"), validation:"Boolean فقط؛ BranchId الأخير User Runtime/Preference State خارج GEN-016."),
        P("PRINT.SHOW_COMPANY_LOGO", "إظهار شعار الشركة في المخرجات", "الطباعة والتقارير", PropertyValueKind.Boolean, "نعم",
            S("System","Company")),
        P("PRINT.SHOW_BRANCH_INFO", "إظهار بيانات الفرع", "الطباعة والتقارير", PropertyValueKind.Boolean, "نعم",
            S("System","Company","Branch"), validation:"يتحكم في الإظهار فقط؛ بيانات الفرع تأتي من الشاشة المالكة."),
        P("PRINT.DEFAULT_PAPER_SIZE", "حجم الورق الافتراضي", "الطباعة والتقارير", PropertyValueKind.List, "A4",
            S("System","Company","Branch","User"), values:new[]{"A4"}),
        P("PRINT.DEFAULT_ORIENTATION", "اتجاه الصفحة الافتراضي", "الطباعة والتقارير", PropertyValueKind.List, "عمودي",
            S("System","Company","User"), values:new[]{"عمودي"}),
        P("PRINT.DEFAULT_COPIES", "عدد النسخ الافتراضي", "الطباعة والتقارير", PropertyValueKind.Integer, "1",
            S("System","Company","Branch","User"), min:1, max:10, validation:"1–10 نسخ."),
        P("REPORT.DEFAULT_EXPORT_FORMAT", "صيغة التصدير الافتراضية", "الطباعة والتقارير", PropertyValueKind.List, "PDF",
            S("System","Company","User"), values:new[]{"PDF"}),
        P("SYNC.BACKGROUND_ENABLED", "تفعيل المزامنة الخلفية", "الاتصال والمزامنة", PropertyValueKind.Boolean, "نعم",
            S("System","Company","Branch"),
            validation:"يشغل المحرك العام للوحدات المصممة أصلًا لـ Offline/Sync فقط."),
        P("SYNC.INTERVAL_MINUTES", "فترة المزامنة", "الاتصال والمزامنة", PropertyValueKind.DurationMinutes, "15 دقيقة",
            S("System","Company","Branch"), min:1, max:1440, validation:"1–1440 دقيقة."),
        P("SYNC.BATCH_SIZE", "حجم دفعة المزامنة", "الاتصال والمزامنة", PropertyValueKind.Integer, "500",
            S("System","Company","Branch"), min:50, max:5000, increment:50,
            validation:"50–5000، Increment=50، Requested/Preferred فقط؛ ServerMaxBatchSize هو الحد النهائي."),
        P("SYNC.PENDING_WARNING_MINUTES", "مهلة تنبيه المزامنة المعلقة", "الاتصال والمزامنة", PropertyValueKind.DurationMinutes, "30 دقيقة",
            S("System","Company","Branch"), min:5, max:1440, increment:5, validation:"5–1440 دقيقة، Increment=5."),
        P("NOTIFY.OPERATIONAL_INAPP_ENABLED", "التنبيهات التشغيلية داخل النظام", "التنبيهات العامة", PropertyValueKind.Boolean, "نعم",
            S("System","Company","User"),
            validation:"يخص التنبيهات التشغيلية الاختيارية فقط ولا يعطل Mandatory/Required."),
        P("NOTIFY.DEFAULT_REMINDER_MINUTES", "مهلة التذكير التشغيلية الافتراضية", "التنبيهات العامة", PropertyValueKind.DurationMinutes, "15 دقيقة",
            S("System","Company","User"), min:0, max:10080, increment:5, validation:"0–10080 دقيقة، Increment=5.")
    };

    private static string[] S(params string[] scopes) => scopes;

    private static PropertyDefinition P(
        string code,
        string arabicName,
        string group,
        PropertyValueKind kind,
        string defaultDisplayValue,
        string[] scopes,
        decimal? min = null,
        decimal? max = null,
        decimal? increment = null,
        int decimalPlaces = 0,
        string[]? values = null,
        string? validation = null) =>
        new(code, arabicName, group, kind, defaultDisplayValue, scopes, min, max, increment,
            decimalPlaces, values ?? Array.Empty<string>(), validation ?? "قيمة معرفة مسبقًا حسب النوع والنطاق المسموح.");

    private enum PropertyValueKind
    {
        Boolean,
        Integer,
        DurationMinutes,
        List
    }

    private sealed record PropertyDefinition(
        string PropertyCode,
        string ArabicName,
        string Group,
        PropertyValueKind ValueKind,
        string DefaultDisplayValue,
        string[] AllowedScopes,
        decimal? Minimum,
        decimal? Maximum,
        decimal? Increment,
        int DecimalPlaces,
        string[] AllowedValues,
        string ValidationSummary)
    {
        public string ValueTypeDisplay => ValueKind switch
        {
            PropertyValueKind.Boolean => "نعم/لا",
            PropertyValueKind.Integer => "عدد صحيح",
            PropertyValueKind.DurationMinutes => "مدة بالدقائق",
            PropertyValueKind.List => "قائمة ثابتة",
            _ => ValueKind.ToString()
        };
    }

    private sealed record PropertyCatalogRow(
        string PropertyCode,
        string Name,
        string Group,
        string Scope,
        string EffectiveValue,
        string ValueSource,
        string Status);
}
