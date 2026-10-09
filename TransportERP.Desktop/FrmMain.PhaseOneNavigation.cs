using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TransportERP.Desktop.Forms.Setup.Security;
using TransportERP.Desktop.Forms.SystemSettings.General;
using TransportERP.Desktop.Forms.SystemSettings.DocumentControl;
using TransportERP.Desktop.البوالص_والشحن.الاعدادات;
using TransportERP.Desktop.البوالص_والشحن.المدخلات;
namespace TransportERP.Desktop;
public partial class FrmMain
{
    internal static readonly (string Code, string Title)[] PhaseOneScreens =
    {
            ("01.01", "شاشة تسجيل الدخول"),
            ("01.02", "الشاشة الرئيسية"),
            ("02.01.01", "بيانات الشركات"),
            ("02.02.01", "بيانات الفروع"),
            ("02.03.01", "المستخدمون"),
            ("02.03.02", "الأدوار"),
            ("02.03.03", "الصلاحيات"),
            ("02.03.06", "تغيير كلمة السر"),
            ("02.04.01", "إعدادات الترقيم"),
            ("02.04.02", "تهيئة العملات"),
            ("02.04.03", "أسعار الصرف"),
            ("02.05.21", "ترميز اللغات"),
            ("03.01.01", "إدارة العملاء"),
            ("03.02.01", "إدارة الموردين"),
            ("03.03.01", "إدارة الوكلاء"),
            ("04.01.01", "دليل الحسابات"),
            ("04.02.01", "مراكز التكلفة"),
            ("04.03.01", "إدارة الصناديق"),
            ("04.03.02", "إدارة البنوك"),
            ("04.04.01", "سند القبض"),
            ("04.04.02", "سند الصرف"),
            ("04.05.01", "قيد اليومية"),
            ("04.07.16", "الأرصدة الافتتاحية"),
            ("04.08.01", "إشعارات مدينة"),
            ("04.08.02", "إشعارات دائنة"),
            ("04.08.05", "طلبات قيود اليومية"),
            ("04.09.06", "تسوية البنوك"),
            ("04.10.03", "تسوية المصروفات المقدمة"),
            ("04.10.04", "تسوية الإيرادات المقدمة"),
            ("04.10.05", "جرد النقدية"),
            ("02.02.04", "بيانات الدول"),
            ("02.02.05", "بيانات المحافظات"),
            ("02.02.06", "المديريات"),
            ("02.02.07", "بيانات المدن"),
            ("02.02.08", "بيانات المناطق"),
            ("02.04.06", "السنوات المالية"),
            ("02.04.07", "المتغيرات العامة"),
            ("SETUP:INTERNATIONAL-REGIONS", "الأقاليم الدولية"),
            ("SETUP:BRANCH-GROUPS", "مجموعات الفروع"),
            ("SETUP:SCREEN-BACKGROUNDS", "خلفيات الشاشات"),
            ("SETUP:COST-CENTERS", "تهيئة مراكز التكلفة"),
            ("SETUP:PROJECTS", "تهيئة المشاريع"),
            ("SETUP:PROJECT-EXTRA-FIELDS", "ترميز بيانات إضافية للمشاريع"),
            ("SETUP:ACTIVITIES", "تهيئة الأنشطة"),
            ("SETUP:GENERAL-CODES", "الترميزات العامة"),
            ("SETUP:STRUCTURE-TYPES", "أنواع الهيكل الإداري"),
            ("SETUP:TAX-BRACKETS", "ترميز الشرائح الضريبية"),
            ("SETUP:TAX-TYPES", "أنواع الضرائب"),
            ("SETUP:TAX-METHODS", "طرق احتساب الضرائب"),
            ("SETUP:EINVOICE-SEQUENCES", "تسلسلات الفاتورة الإلكترونية"),
            ("SETUP:ADDITIONAL-FIELDS", "تهيئة الحقول الإضافية"),
            ("SETUP:EMPLOYEE-CODES", "الترميزات العامة للموظفين"),
            ("SETUP:TEXT-TRANSLATION", "ترجمة النصوص"),
            ("SETUP:APPROVAL-LEVELS", "مستويات الاعتماد"),
            ("INPUT:ADMINISTRATIVE-STRUCTURE", "الهيكل الإداري"),
            ("INPUT:ACTIVITY-DATA", "بيانات الأنشطة"),
            ("INPUT:PROJECT-DATA", "بيانات المشاريع"),
            ("INPUT:INTERMEDIATE-ACCOUNTS", "الحسابات الوسيطة"),
            ("INPUT:EMPLOYEE-DATA", "بيانات الموظفين"),
            ("INPUT:EMPLOYEE-PROFESSIONS", "ربط الموظفين بالمهن الوظيفية"),
            ("INPUT:DEFAULT-CHARTS", "الأدلة المحاسبية الافتراضية"),
            ("INPUT:CASHFLOW-ACCOUNT-LINKS", "ربط التدفقات النقدية مع تحليل الحسابات"),
            ("INPUT:OTHER-ACCOUNT-LINKS", "ربط الحسابات المدينة والدائنة الأخرى"),
            ("INPUT:OTHER-ACCOUNT-GROUPS", "مجموعات الحسابات المدينة والدائنة الأخرى"),
            ("INPUT:OTHER-ANALYTICAL-ACCOUNTS", "الحسابات المدينة والدائنة الأخرى"),
            ("04.01.02", "تهيئة الدليل المحاسبي"),
            ("04.03.03", "الحسابات البنكية"),
            ("04.03.04", "طرق الدفع"),
            ("04.11.01", "إعداد فترات النظام"),
            ("04.04.03", "سند التحويل بين الصناديق والبنوك"),
            ("04.11.02", "إغلاق وفتح الفترات المحاسبية"),
            ("04.11.03", "قيود التسوية والإقفال السنوي"),
            ("04.11.04", "القيود العكسية وربط القيد الأصلي بالقيد العاكس"),
            ("04.11.05", "طلبات الاعتماد المحاسبية"),
            ("04.04.04", "تحويل نقدي بين الصناديق"),
            ("04.04.05", "إيداع نقدي في البنك"),
            ("04.04.06", "سحب من البنك إلى الصندوق"),
            ("04.11.06", "إقفال وردية الصندوق"),
            ("04.11.07", "تخصيص الدفعات وتسوية الأرصدة"),
            ("ONYX:SCREEN-0078", "متغيرات الأستاذ العام"),
            ("ONYX:SCREEN-0079", "مجموعات الصناديق"),
            ("ONYX:SCREEN-0080", "مجموعات البنوك"),
            ("ONYX:SCREEN-0081", "أنواع الإشعارات"),
            ("ONYX:SCREEN-0082", "أنواع الطلبات"),
            ("ONYX:SCREEN-0083", "أنواع قيود اليومية"),
            ("ONYX:SCREEN-0084", "أنواع القبض والصرف"),
            ("ONYX:SCREEN-0085", "ترميز البيان"),
            ("ONYX:SCREEN-0086", "طلب فتح حساب"),
            ("ONYX:SCREEN-0089", "دفاتر الشيكات"),
            ("ONYX:SCREEN-0090", "ربط الحسابات بالأنواع الضريبية"),
            ("ONYX:SCREEN-0091", "تهيئة الحدود"),
            ("ONYX:SCREEN-0092", "مصمم التقارير الختامية والتدفقات والقوائم"),
            ("ONYX:SCREEN-0093", "ربط الحسابات بالمراكز"),
            ("ONYX:SCREEN-0094", "ربط الحسابات بالمشاريع"),
            ("ONYX:SCREEN-0098", "طلبات سندات القبض"),
            ("ONYX:SCREEN-0100", "طلبات سندات الصرف"),
            ("ONYX:SCREEN-0102", "تحقيق الإيداع النقدي لدى البنوك"),
            ("ONYX:SCREEN-0103", "الشيكات المستحقة للسداد – آليًا"),
            ("ONYX:SCREEN-0104", "استحقاق شيكات سندات القبض – يدويًا"),
            ("ONYX:SCREEN-0105", "استحقاق شيكات سندات الصرف – يدويًا"),
            ("ONYX:SCREEN-0106", "مطابقة البنوك"),
            ("ONYX:SCREEN-0110", "طلب صرف عملة"),
            ("ONYX:SCREEN-0111", "صرف عملة"),
            ("INV:EXTERNAL-REPAIR", "أمر إصلاح خارجي"),
            ("INV:CUSTODY-ISSUE", "إذن صرف أمانات"),
            ("INV:CUSTODY-RECEIPT", "إذن توريد أمانات"),
            ("INV:DAMAGED-ISSUE", "أمر صرف توالف"),
            ("INV:QUANTITY-RESERVATION", "حجز كميات الأصناف"),
            ("INV:MANUAL-STOCKTAKE", "الجرد اليدوي"),
            ("INV:TRANSFER", "التحويل المخزني"),
            ("INV:MATERIAL-REQUEST", "طلب صرف/تحويل مواد"),
            ("INV:TRANSFER-RECEIPT", "استلام تحويل مخزني"),
            ("INV:SETTLEMENT", "تسوية مخزون"),
            ("INV:STOCKTAKE-REPORT", "تقارير الجرد"),
            ("INV:RECEIPT-AUTHORIZATION", "إذن التوريد المخزني"),
            ("INV:RECEIPT-ORDER", "أمر التوريد المخزني"),
            ("INV:ISSUE-ORDER", "أمر الصرف المخزني"),
            ("PUR:FOREIGN-RECEIPT", "إذن توريد المشتريات الخارجية"),
            ("PUR:FOREIGN-COSTING", "تكاليف المشتريات الخارجية"),
            ("PUR:REQUEST", "طلبات الشراء"),
            ("PUR:TECHNICAL-COMPARISON", "المقارنة الفنية لعروض الشراء"),
            ("PUR:COMPARISON-NOMINATION", "ترشيح المقارنة الفنية لعروض الشراء"),
            ("SUP:ADDITIONAL-PURCHASE-DISCOUNT", "الخصومات الإضافية لفواتير المشتريات"),
            ("CUS:REPRESENTATIVE-COMMISSIONS", "احتساب عمولات المندوبين"),
            ("CUS:INSTALLMENT-SETTLEMENT", "تسوية أقساط العملاء"),
            ("CUS:DEBT-REPORT", "تقارير مديونية العملاء"),
            ("SHIP:001", "أنواع الشحن"),
            ("SHIP:002", "فئات وأصناف الشحن"),
            ("SHIP:003", "وحدات القياس والتعبئة"),
            ("SHIP:004", "أولويات الشحن"),
            ("SHIP:005", "أنواع وسائل النقل"),
            ("SHIP:006", "مجموعات وسائل النقل"),
            ("SHIP:007", "وسائل النقل"),
            ("SHIP:008", "خطوط السير"),
            ("SHIP:009", "قواعد تسعير الشحن"),
            ("SHIP:010", "حالات البوالص"),
            ("SHIP:011", "دفاتر البوالص"),
            ("SHIP:012", "بوليصة الشحن"),
            ("SHIP:013", "ترحيل الشحنات"),
            ("SHIP:014", "التوريد المخزني"),
    };
    private readonly HashSet<string> expandedNavigationNames = new(StringComparer.Ordinal);
    private string? navigationSelectionName;
    private string? navigationScrollName;
    private bool navigationStateSaved;
    private bool navigationFilterActive;

    private static IEnumerable<TreeNode> EnumerateNavigation(TreeNodeCollection nodes)
    {
        foreach (TreeNode node in nodes)
        {
            yield return node;
            foreach (var child in EnumerateNavigation(node.Nodes)) yield return child;
        }
    }

    private void InitializePhaseOneNavigation()
    {
        string? selectedBeforeRebuild = tvSystemTree.SelectedNode?.Name;
        if (!navigationFilterActive && tvSystemTree.Nodes.Count != 0)
        {
            expandedNavigationNames.Clear();
            foreach (var node in EnumerateNavigation(tvSystemTree.Nodes))
                if (node.IsExpanded) expandedNavigationNames.Add(node.Name);
            navigationSelectionName = selectedBeforeRebuild;
            navigationScrollName = tvSystemTree.TopNode?.Name;
            navigationStateSaved = true;
        }
        tvSystemTree.BeginUpdate();
        try
        {
            tvSystemTree.Nodes.Clear();
            var root = new TreeNode("شجرة النظام") { Name = "system-root", ImageKey = "folder", SelectedImageKey = "folder" };
            tvSystemTree.Nodes.Add(root);
            var groups = new Dictionary<string, TreeNode>();
            TreeNode Group(string key, string title, TreeNode parent)
            {
                if (!groups.TryGetValue(key, out var group))
                {
                    group = new TreeNode(title) { Name = key, ImageKey = "folder", SelectedImageKey = "folder" };
                    groups.Add(key, group);
                    parent.Nodes.Add(group);
                }
                return group;
            }
            var setup = Group("setup", "المتغيرات والإعدادات", root);
            var setupGeneral = Group("setup-general", "الإعدادات العامة", setup);
            var setupInitial = Group("setup-initial", "التهيئة الأولية", setup);
            var setupUsers = Group("setup-users", "إدارة المستخدمين", setup);
            var setupCodes = Group("setup-codes", "الترميزات العامة", setup);
            var accounts = Group("accounts", "نظام الأستاذ العام", root);
            var accountsSetup = Group("accounts-setup", "التهيئة", accounts);
            var accountsOperations = Group("accounts-operations", "عمليات", accounts);
            var accountsReports = Group("accounts-reports", "تقارير", accounts);
            var inventorySystems = Group("inventory-systems", "أنظمة المخازن", root);
            var inventory = Group("inventory", "نظام إدارة المخازن", inventorySystems);
            var inventoryOperations = Group("inventory-operations", "عمليات المخزون", inventory);
            var inventoryStocktaking = Group("inventory-stocktaking", "نظام الجرد", inventorySystems);
            var parties = Group("parties", "الأطراف", root);
            var supplierSystems = Group("supplier-systems", "أنظمة الموردين", root);
            var customerSystems = Group("customer-systems", "أنظمة العملاء", root);
            var customerManagement = Group("customer-management", "نظام إدارة العملاء", customerSystems);
            var customerOperations = Group("customer-operations", "عمليات العملاء", customerManagement);
            var customerReports = Group("customer-reports", "تقارير العملاء", customerManagement);
            var purchasing = Group("purchasing", "نظام إدارة المشتريات", supplierSystems);
            var purchasingOperations = Group("purchasing-operations", "العمليات", purchasing);
            var supplierManagement = Group("supplier-management", "نظام إدارة الموردين", supplierSystems);
            var supplierOperations = Group("supplier-operations", "عمليات الموردين", supplierManagement);
            var session = Group("session", "الدخول والرئيسية", root);

            var shipments = Group("shipments", "البوالص والشحن", root);
            var shipmentsSetup = Group("shipments-setup", "الإعدادات والتهيئة", shipments);
            var shipmentsOperations = Group("shipments-operations", "العمليات", shipments);


            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var item in PhaseOneScreens)
            {
                if (!seen.Add(item.Code)) throw new InvalidOperationException("Duplicate screen code: " + item.Code);
                var parent = item.Code switch
                {
                    "PUR:FOREIGN-RECEIPT" => purchasingOperations,
                    "PUR:FOREIGN-COSTING" => purchasingOperations,
                    "PUR:REQUEST" => purchasingOperations,
                    "PUR:TECHNICAL-COMPARISON" => purchasingOperations,
                    "PUR:COMPARISON-NOMINATION" => purchasingOperations,
                    "SUP:ADDITIONAL-PURCHASE-DISCOUNT" => supplierOperations,
                    "CUS:REPRESENTATIVE-COMMISSIONS" => customerOperations,
                    "CUS:INSTALLMENT-SETTLEMENT" => customerOperations,
                    "CUS:DEBT-REPORT" => customerReports,
                    "INV:QUANTITY-RESERVATION" or "INV:EXTERNAL-REPAIR" or "INV:CUSTODY-RECEIPT" or "INV:CUSTODY-ISSUE" or "INV:DAMAGED-ISSUE" => inventoryOperations,
                    "INV:MANUAL-STOCKTAKE" => inventoryStocktaking,
                    "INV:TRANSFER" => inventoryOperations,
                    "INV:MATERIAL-REQUEST" => inventoryOperations,
                    "INV:TRANSFER-RECEIPT" => inventoryOperations,
                    "INV:SETTLEMENT" => inventoryOperations,
                    "INV:STOCKTAKE-REPORT" => inventoryStocktaking,
                    "INV:RECEIPT-AUTHORIZATION" => inventoryOperations,
                    "INV:RECEIPT-ORDER" => inventoryOperations,
                    "INV:ISSUE-ORDER" => inventoryOperations,
                    "SHIP:001" or "SHIP:002" or "SHIP:003" or "SHIP:004" or "SHIP:005" or "SHIP:006" or "SHIP:007" or "SHIP:008" or "SHIP:009" or "SHIP:010" or "SHIP:011" => shipmentsSetup,
                    "SHIP:012" => shipmentsOperations,
                    "SHIP:013" => shipmentsOperations,
                    "SHIP:014" => shipmentsOperations,
                    "02.04.07" or "02.04.01" => setupGeneral,
                    "SETUP:INTERNATIONAL-REGIONS" or "SETUP:BRANCH-GROUPS" => setupInitial,
                    "SETUP:SCREEN-BACKGROUNDS" => setupGeneral,
                    "02.02.04" or "02.02.05" or "02.02.07" or "02.02.08" => setupInitial,
                    "SETUP:COST-CENTERS" or "SETUP:PROJECTS" => accountsSetup,
                    "SETUP:PROJECT-EXTRA-FIELDS" or "SETUP:ACTIVITIES" => accountsSetup,
                    "SETUP:GENERAL-CODES" or "SETUP:STRUCTURE-TYPES" => setupCodes,
                    "SETUP:TAX-BRACKETS" or "SETUP:TAX-TYPES" => setupCodes,
                    "SETUP:TAX-METHODS" => setupCodes,
                    "SETUP:EINVOICE-SEQUENCES" or "SETUP:ADDITIONAL-FIELDS" => setupGeneral,
                    "SETUP:EMPLOYEE-CODES" => setupCodes,
                    "SETUP:TEXT-TRANSLATION" => setupGeneral,
                    "SETUP:APPROVAL-LEVELS" => setupUsers,
                    "INPUT:ADMINISTRATIVE-STRUCTURE" => setupInitial,
                    "INPUT:ACTIVITY-DATA" => accountsSetup,
                    "INPUT:PROJECT-DATA" => accountsSetup,
                    "INPUT:INTERMEDIATE-ACCOUNTS" => accountsSetup,
                    "INPUT:EMPLOYEE-DATA" => setupInitial,
                    "INPUT:EMPLOYEE-PROFESSIONS" => setupInitial,
                    "INPUT:DEFAULT-CHARTS" or "INPUT:CASHFLOW-ACCOUNT-LINKS" => accountsSetup,
                    "INPUT:OTHER-ACCOUNT-LINKS" or "INPUT:OTHER-ACCOUNT-GROUPS" or "INPUT:OTHER-ANALYTICAL-ACCOUNTS" => accountsSetup,
                    "02.01.01" or "02.02.01" or "02.04.06" => setupInitial,
                    "ONYX:SCREEN-0092" => accountsReports,
                    "04.01.01" or "04.01.02" or "04.02.01" or "04.03.01" or "04.03.02" or "04.03.03" or "04.03.04" or "04.07.16" or "04.11.01" => accountsSetup,
                    _ when item.Code.StartsWith("01.", StringComparison.Ordinal) => session,
                    _ when item.Code.StartsWith("02.03.", StringComparison.Ordinal) => setupUsers,
                    _ when item.Code.StartsWith("02.", StringComparison.Ordinal) => setupCodes,
                    _ when item.Code.StartsWith("03.", StringComparison.Ordinal) => parties,
                    _ when item.Code.StartsWith("ONYX:", StringComparison.Ordinal) &&
                        string.CompareOrdinal(item.Code, "ONYX:SCREEN-0094") <= 0 => accountsSetup,
                    _ => accountsOperations


                };
                parent.Nodes.Add(new TreeNode(item.Title) { Name = item.Code, Tag = item.Code, ImageKey = "page", SelectedImageKey = "page", ToolTipText = item.Title + " — " + item.Code });
            }
            // Order only the identities supported by the reference and existing registry.
            void Order(TreeNode group, params string[] codes)
            {
                for (var i = codes.Length - 1; i >= 0; i--)
                {
                    var node = group.Nodes[codes[i]];
                    if (node is null) continue;
                    group.Nodes.Remove(node);
                    group.Nodes.Insert(0, node);
                }
            }
            Order(setupInitial, "02.04.06", "02.01.01", "02.02.01");
            Order(accountsSetup, "04.01.01", "04.03.01", "04.03.02", "04.07.16", "04.02.01");
            Order(accountsOperations, "04.05.01", "ONYX:SCREEN-0100", "04.04.02", "04.04.01", "ONYX:SCREEN-0103", "04.11.02", "04.11.03", "04.08.01", "04.08.02");
            var filter = txtTreeSearch.Text.Trim();
            if (filter.Length != 0)
            {
                bool Keep(TreeNode node)
                {
                    if (node.Tag is string code)
                        return node.Text.Contains(filter, StringComparison.OrdinalIgnoreCase) || code.Contains(filter, StringComparison.OrdinalIgnoreCase);
                    for (var i = node.Nodes.Count - 1; i >= 0; i--)
                        if (!Keep(node.Nodes[i])) node.Nodes.RemoveAt(i);
                    return node.Nodes.Count != 0;
                }
                if (!Keep(root)) tvSystemTree.Nodes.Clear();
                tvSystemTree.ExpandAll();
            }
            else if (navigationStateSaved)
            {
                foreach (var node in EnumerateNavigation(tvSystemTree.Nodes))
                    if (expandedNavigationNames.Contains(node.Name)) node.Expand();
            }
            else
            {
                root.Expand();
                setup.Expand();
                setupInitial.Expand();
                shipments.Expand();
                shipmentsSetup.Expand();
            }
            string? selection = filter.Length == 0 ? navigationSelectionName : selectedBeforeRebuild;
            TreeNode? selected = null;
            TreeNode? top = null;
            foreach (var node in EnumerateNavigation(tvSystemTree.Nodes))
            {
                if (node.Name == selection) selected = node;
                if (node.Name == navigationScrollName) top = node;
            }
            if (selected is not null) tvSystemTree.SelectedNode = selected;
            if (filter.Length == 0 && top is not null) tvSystemTree.TopNode = top;
            navigationFilterActive = filter.Length != 0;
            SetNavigationStatus(selected);
        }
        finally { tvSystemTree.EndUpdate(); }
    }
    // Retain earlier text-driven route IDs as aliases, but expose one menu/tab identity.
    // Legacy control files and their validation/binding helpers are intentionally preserved.
    internal static string NormalizeScreenCode(string code) => code switch
    {
        "SETUP:GENERAL-VARIABLES" => "02.04.07",
        "SETUP:COUNTRY-DATA" => "02.02.04",
        "SETUP:GOVERNORATE-DATA" => "02.02.05",
        "SETUP:CITY-DATA" => "02.02.07",
        "SETUP:AREA-DATA" => "02.02.08",
        "SETUP:CHART" => "04.01.02",
        _ => code
    };

    internal static Form? CreatePhaseOneScreen(string code) => NormalizeScreenCode(code) switch
    {
            "01.01" => new FrmLogin(),
            "01.02" => null,
            "02.01.01" => new FrmTextSetup(new UcCompanyData()),
            "02.02.01" => new FrmTextSetup(new UcBranchData()),
            "02.03.01" => new FrmUsersPermissions(),
            "02.03.02" => new FrmUsersPermissions("tabRolesV20"),
            "02.03.03" => new FrmUsersPermissions("tabPermissions"),
            "02.03.06" => new FrmChangePassword(),
            "02.04.01" => new FrmDocumentNumbering(),
            "02.04.02" => new FrmTextSetup(new UcCurrencySetup()),
            "02.04.03" => new FrmExchangeRates(),
            "02.05.21" => new TransportERP.EmptyForms.FrmScreen_02_05_21(),
            "03.01.01" => new TransportERP.EmptyForms.FrmScreen_03_01_01(),
            "03.02.01" => new TransportERP.EmptyForms.FrmScreen_03_02_01(),
            "03.03.01" => new TransportERP.EmptyForms.FrmScreen_03_03_01(),
            "04.01.01" => new FrmTextSetup(new UcAccountingChartData()),
            "04.02.01" => new FrmTextSetup(new UcCostCenterData()),
            "04.03.01" => new TransportERP.EmptyForms.FrmScreen_04_03_01(),
            "04.03.02" => new TransportERP.EmptyForms.FrmScreen_04_03_02(),
            "04.04.01" => new TransportERP.EmptyForms.FrmScreen_04_04_01(),
            "04.04.02" => new TransportERP.EmptyForms.FrmScreen_04_04_02(),
            "04.05.01" => new TransportERP.EmptyForms.FrmScreen_04_05_01(),
            "04.07.16" => new TransportERP.EmptyForms.FrmScreen_04_07_16(),
            "04.08.01" => new TransportERP.EmptyForms.FrmScreen_04_08_01(),
            "04.08.02" => new TransportERP.EmptyForms.FrmScreen_04_08_02(),
            "04.08.05" => new TransportERP.EmptyForms.FrmScreen_04_08_05(),
            "04.09.06" => new TransportERP.EmptyForms.FrmScreen_04_09_06(),
            "04.10.03" => new TransportERP.EmptyForms.FrmScreen_04_10_03(),
            "04.10.04" => new TransportERP.EmptyForms.FrmScreen_04_10_04(),
            "04.10.05" => new TransportERP.EmptyForms.FrmScreen_04_10_05(),
            "02.02.04" => new FrmTextSetup(new UcCountryData()),
            "02.02.05" => new FrmTextSetup(new UcGovernorateData()),
            "02.02.06" => new TransportERP.EmptyForms.FrmScreen_02_02_06(),
            "02.02.07" => new FrmTextSetup(new UcCityData()),
            "02.02.08" => new FrmTextSetup(new UcAreaData()),
            "02.04.06" => new TransportERP.EmptyForms.FrmScreen_02_04_06(),
            "02.04.07" => new FrmTextSetup(new UcGeneralVariablesSetup()),
            "SETUP:INTERNATIONAL-REGIONS" => new FrmTextSetup(new UcInternationalRegions()),
            "SETUP:BRANCH-GROUPS" => new FrmTextSetup(new UcBranchGroups()),
            "SETUP:SCREEN-BACKGROUNDS" => new FrmTextSetup(new UcScreenBackgrounds()),
            "SETUP:COST-CENTERS" => new FrmTextSetup(new UcCostCenterSetup()),
            "SETUP:PROJECTS" => new FrmTextSetup(new UcProjectSetup()),
            "SETUP:PROJECT-EXTRA-FIELDS" => new FrmTextSetup(new UcProjectExtraFieldCodes()),
            "SETUP:ACTIVITIES" => new FrmTextSetup(new UcActivitySetup()),
            "SETUP:GENERAL-CODES" => new FrmTextSetup(new UcGeneralCodes()),
            "SETUP:STRUCTURE-TYPES" => new FrmTextSetup(new UcAdministrativeStructureTypes()),
            "SETUP:TAX-BRACKETS" => new FrmTextSetup(new UcTaxBracketCodes()),
            "SETUP:TAX-TYPES" => new FrmTextSetup(new UcTaxTypes()),
            "SETUP:TAX-METHODS" => new FrmTextSetup(new UcTaxCalculationMethods()),
            "SETUP:EINVOICE-SEQUENCES" => new FrmTextSetup(new UcElectronicInvoiceSequences()),
            "SETUP:ADDITIONAL-FIELDS" => new FrmTextSetup(new UcAdditionalFieldSetup()),
            "SETUP:EMPLOYEE-CODES" => new FrmTextSetup(new UcEmployeeGeneralCodes()),
            "SETUP:TEXT-TRANSLATION" => new FrmTextSetup(new UcTextTranslation()),
            "SETUP:APPROVAL-LEVELS" => new FrmTextSetup(new UcApprovalLevels()),
            "INPUT:ADMINISTRATIVE-STRUCTURE" => new FrmTextSetup(new UcAdministrativeStructure()),
            "INPUT:ACTIVITY-DATA" => new FrmTextSetup(new UcActivityData()),
            "INPUT:PROJECT-DATA" => new FrmTextSetup(new UcProjectData()),
            "INPUT:INTERMEDIATE-ACCOUNTS" => new FrmTextSetup(new UcIntermediateAccounts()),
            "INPUT:EMPLOYEE-DATA" => new FrmTextSetup(new UcEmployeeData()),
            "INPUT:EMPLOYEE-PROFESSIONS" => new FrmTextSetup(new UcEmployeeProfessionLinks()),
            "INPUT:DEFAULT-CHARTS" => new FrmTextSetup(new UcDefaultAccountingCharts()),
            "INPUT:CASHFLOW-ACCOUNT-LINKS" => new FrmTextSetup(new UcCashFlowAccountLinks()),
            "INPUT:OTHER-ACCOUNT-LINKS" => new FrmTextSetup(new UcOtherAccountLinks()),
            "INPUT:OTHER-ACCOUNT-GROUPS" => new FrmTextSetup(new UcOtherAccountGroups()),
            "INPUT:OTHER-ANALYTICAL-ACCOUNTS" => new FrmTextSetup(new UcOtherAnalyticalAccounts()),
            "04.01.02" => new FrmTextSetup(new UcChartSetup()),
            "04.03.03" => new TransportERP.EmptyForms.FrmScreen_04_03_03(),
            "04.03.04" => new TransportERP.EmptyForms.FrmScreen_04_03_04(),
            "04.11.01" => new FrmTextSetup(new UcSystemPeriods()),
            "04.04.03" => new TransportERP.EmptyForms.FrmScreen_04_04_03(),
            "04.11.02" => new TransportERP.EmptyForms.FrmScreen_04_11_02(),
            "04.11.03" => new TransportERP.EmptyForms.FrmScreen_04_11_03(),
            "04.11.04" => new TransportERP.EmptyForms.FrmScreen_04_11_04(),
            "04.11.05" => new TransportERP.EmptyForms.FrmScreen_04_11_05(),
            "04.04.04" => new TransportERP.EmptyForms.FrmScreen_04_04_04(),
            "04.04.05" => new TransportERP.EmptyForms.FrmScreen_04_04_05(),
            "04.04.06" => new TransportERP.EmptyForms.FrmScreen_04_04_06(),
            "04.11.06" => new TransportERP.EmptyForms.FrmScreen_04_11_06(),
            "04.11.07" => new TransportERP.EmptyForms.FrmScreen_04_11_07(),
            "ONYX:SCREEN-0078" => new TransportERP.EmptyForms.FrmOnyxSCREEN0078(),
            "ONYX:SCREEN-0079" => new TransportERP.EmptyForms.FrmOnyxSCREEN0079(),
            "ONYX:SCREEN-0080" => new TransportERP.EmptyForms.FrmOnyxSCREEN0080(),
            "ONYX:SCREEN-0081" => new TransportERP.EmptyForms.FrmOnyxSCREEN0081(),
            "ONYX:SCREEN-0082" => new TransportERP.EmptyForms.FrmOnyxSCREEN0082(),
            "ONYX:SCREEN-0083" => new TransportERP.EmptyForms.FrmOnyxSCREEN0083(),
            "ONYX:SCREEN-0084" => new TransportERP.EmptyForms.FrmOnyxSCREEN0084(),
            "ONYX:SCREEN-0085" => new TransportERP.EmptyForms.FrmOnyxSCREEN0085(),
            "ONYX:SCREEN-0086" => new TransportERP.EmptyForms.FrmOnyxSCREEN0086(),
            "ONYX:SCREEN-0089" => new TransportERP.EmptyForms.FrmOnyxSCREEN0089(),
            "ONYX:SCREEN-0090" => new TransportERP.EmptyForms.FrmOnyxSCREEN0090(),
            "ONYX:SCREEN-0091" => new TransportERP.EmptyForms.FrmOnyxSCREEN0091(),
            "ONYX:SCREEN-0092" => new TransportERP.EmptyForms.FrmOnyxSCREEN0092(),
            "ONYX:SCREEN-0093" => new TransportERP.EmptyForms.FrmOnyxSCREEN0093(),
            "ONYX:SCREEN-0094" => new TransportERP.EmptyForms.FrmOnyxSCREEN0094(),
            "ONYX:SCREEN-0098" => new TransportERP.EmptyForms.FrmOnyxSCREEN0098(),
            "ONYX:SCREEN-0100" => new TransportERP.EmptyForms.FrmOnyxSCREEN0100(),
            "ONYX:SCREEN-0102" => new TransportERP.EmptyForms.FrmOnyxSCREEN0102(),
            "INV:EXTERNAL-REPAIR" => new TransportERP.EmptyForms.FrmInventoryExternalRepairOrder(),
        "INV:CUSTODY-ISSUE" => new TransportERP.EmptyForms.FrmInventoryCustodyIssue(),
        "INV:CUSTODY-RECEIPT" => new TransportERP.EmptyForms.FrmInventoryCustodyReceipt(),
        "INV:DAMAGED-ISSUE" => new TransportERP.EmptyForms.FrmInventoryDamagedIssueOrder(),
        "PUR:FOREIGN-RECEIPT" => new TransportERP.EmptyForms.FrmForeignPurchaseReceipt(),
        "PUR:FOREIGN-COSTING" => new TransportERP.EmptyForms.FrmForeignPurchaseCosting(),
        "INV:MANUAL-STOCKTAKE" => new TransportERP.EmptyForms.FrmInventoryManualStocktake(),
        "INV:TRANSFER" => new TransportERP.EmptyForms.FrmInventoryTransfer(),
        "INV:MATERIAL-REQUEST" => new TransportERP.EmptyForms.FrmInventoryMaterialRequest(),
        "INV:TRANSFER-RECEIPT" => new TransportERP.EmptyForms.FrmInventoryTransferReceipt(),
        "INV:SETTLEMENT" => new TransportERP.EmptyForms.FrmInventorySettlement(),
        "INV:STOCKTAKE-REPORT" => new TransportERP.EmptyForms.FrmInventoryStocktakeReport(),
        "INV:RECEIPT-AUTHORIZATION" => new TransportERP.EmptyForms.FrmInventoryReceiptAuthorization(),
        "INV:RECEIPT-ORDER" => new TransportERP.EmptyForms.FrmInventoryReceiptOrder(),
        "INV:ISSUE-ORDER" => new TransportERP.EmptyForms.FrmInventoryIssueOrder(),
        "PUR:REQUEST" => new TransportERP.EmptyForms.FrmPurchaseRequest(),
        "PUR:TECHNICAL-COMPARISON" => new TransportERP.EmptyForms.FrmPurchaseTechnicalComparison(),
        "PUR:COMPARISON-NOMINATION" => new TransportERP.EmptyForms.FrmPurchaseComparisonNomination(),
        "SUP:ADDITIONAL-PURCHASE-DISCOUNT" => new TransportERP.EmptyForms.FrmPurchaseAdditionalDiscount(),
        "CUS:REPRESENTATIVE-COMMISSIONS" => new TransportERP.EmptyForms.FrmRepresentativeCommissions(),
        "CUS:INSTALLMENT-SETTLEMENT" => new TransportERP.EmptyForms.FrmCustomerInstallmentSettlement(),
        "CUS:DEBT-REPORT" => new TransportERP.EmptyForms.FrmCustomerDebtReport(),
        "INV:QUANTITY-RESERVATION" => new TransportERP.EmptyForms.FrmInventoryQuantityReservation(),
            "ONYX:SCREEN-0103" => new TransportERP.EmptyForms.FrmOnyxSCREEN0103(),
            "ONYX:SCREEN-0104" => new TransportERP.EmptyForms.FrmOnyxSCREEN0104(),
            "ONYX:SCREEN-0105" => new TransportERP.EmptyForms.FrmOnyxSCREEN0105(),
            "ONYX:SCREEN-0106" => new TransportERP.EmptyForms.FrmOnyxSCREEN0106(),
            "ONYX:SCREEN-0110" => new TransportERP.EmptyForms.FrmOnyxSCREEN0110(),
            "ONYX:SCREEN-0111" => new TransportERP.EmptyForms.FrmOnyxSCREEN0111(),
        _ => null
    };

    internal static UserControl? CreatePhaseOneControl(string code) => NormalizeScreenCode(code) switch
    {
        "02.01.01" => new UcCompanyData(),
        "02.02.01" => new UcBranchData(),
        "02.03.01" => new UcUsersPermissions(),
        "02.03.02" => new UcUsersPermissions("tabRolesV20"),
        "02.03.03" => new UcUsersPermissions("tabPermissions"),
        "02.03.06" => new UcChangePassword(),
        "02.04.01" => new UcDocumentNumbering(),
        "02.04.02" => new UcCurrencySetup(),
        "02.04.03" => new UcExchangeRates(),
        "02.05.21" => new TransportERP.EmptyForms.UcScreen_02_05_21(),
        "03.01.01" => new TransportERP.EmptyForms.UcScreen_03_01_01(),
        "03.02.01" => new TransportERP.EmptyForms.UcScreen_03_02_01(),
        "03.03.01" => new TransportERP.EmptyForms.UcScreen_03_03_01(),
        "04.01.01" => new UcAccountingChartData(),
        "04.02.01" => new UcCostCenterData(),
        "04.03.01" => new TransportERP.EmptyForms.UcScreen_04_03_01(),
        "04.03.02" => new TransportERP.EmptyForms.UcScreen_04_03_02(),
        "04.04.01" => new TransportERP.EmptyForms.UcScreen_04_04_01(),
        "04.04.02" => new TransportERP.EmptyForms.UcScreen_04_04_02(),
        "04.05.01" => new TransportERP.EmptyForms.UcScreen_04_05_01(),
        "04.07.16" => new TransportERP.EmptyForms.UcScreen_04_07_16(),
        "04.08.01" => new TransportERP.EmptyForms.UcScreen_04_08_01(),
        "04.08.02" => new TransportERP.EmptyForms.UcScreen_04_08_02(),
        "04.08.05" => new TransportERP.EmptyForms.UcScreen_04_08_05(),
        "04.09.06" => new TransportERP.EmptyForms.UcScreen_04_09_06(),
        "04.10.03" => new TransportERP.EmptyForms.UcScreen_04_10_03(),
        "04.10.04" => new TransportERP.EmptyForms.UcScreen_04_10_04(),
        "04.10.05" => new TransportERP.EmptyForms.UcScreen_04_10_05(),
        "02.02.04" => new UcCountryData(),
        "02.02.05" => new UcGovernorateData(),
        "02.02.06" => new TransportERP.EmptyForms.UcScreen_02_02_06(),
        "02.02.07" => new UcCityData(),
        "02.02.08" => new UcAreaData(),
        "02.04.06" => new TransportERP.EmptyForms.UcScreen_02_04_06(),
        "02.04.07" => new UcGeneralVariablesSetup(),
        "SETUP:INTERNATIONAL-REGIONS" => new UcInternationalRegions(),
        "SETUP:BRANCH-GROUPS" => new UcBranchGroups(),
        "SETUP:SCREEN-BACKGROUNDS" => new UcScreenBackgrounds(),
        "SETUP:COST-CENTERS" => new UcCostCenterSetup(),
        "SETUP:PROJECTS" => new UcProjectSetup(),
        "SETUP:PROJECT-EXTRA-FIELDS" => new UcProjectExtraFieldCodes(),
        "SETUP:ACTIVITIES" => new UcActivitySetup(),
        "SETUP:GENERAL-CODES" => new UcGeneralCodes(),
        "SETUP:STRUCTURE-TYPES" => new UcAdministrativeStructureTypes(),
        "SETUP:TAX-BRACKETS" => new UcTaxBracketCodes(),
        "SETUP:TAX-TYPES" => new UcTaxTypes(),
        "SETUP:TAX-METHODS" => new UcTaxCalculationMethods(),
        "SETUP:EINVOICE-SEQUENCES" => new UcElectronicInvoiceSequences(),
        "SETUP:ADDITIONAL-FIELDS" => new UcAdditionalFieldSetup(),
        "SETUP:EMPLOYEE-CODES" => new UcEmployeeGeneralCodes(),
        "SETUP:TEXT-TRANSLATION" => new UcTextTranslation(),
        "SETUP:APPROVAL-LEVELS" => new UcApprovalLevels(),
        "INPUT:ADMINISTRATIVE-STRUCTURE" => new UcAdministrativeStructure(),
        "INPUT:ACTIVITY-DATA" => new UcActivityData(),
        "INPUT:PROJECT-DATA" => new UcProjectData(),
        "INPUT:INTERMEDIATE-ACCOUNTS" => new UcIntermediateAccounts(),
        "INPUT:EMPLOYEE-DATA" => new UcEmployeeData(),
        "INPUT:EMPLOYEE-PROFESSIONS" => new UcEmployeeProfessionLinks(),
        "INPUT:DEFAULT-CHARTS" => new UcDefaultAccountingCharts(),
        "INPUT:CASHFLOW-ACCOUNT-LINKS" => new UcCashFlowAccountLinks(),
        "INPUT:OTHER-ACCOUNT-LINKS" => new UcOtherAccountLinks(),
        "INPUT:OTHER-ACCOUNT-GROUPS" => new UcOtherAccountGroups(),
        "INPUT:OTHER-ANALYTICAL-ACCOUNTS" => new UcOtherAnalyticalAccounts(),
        "04.01.02" => new UcChartSetup(),
        "04.03.03" => new TransportERP.EmptyForms.UcScreen_04_03_03(),
        "04.03.04" => new TransportERP.EmptyForms.UcScreen_04_03_04(),
        "04.11.01" => new UcSystemPeriods(),
        "04.04.03" => new TransportERP.EmptyForms.UcScreen_04_04_03(),
        "04.11.02" => new TransportERP.EmptyForms.UcScreen_04_11_02(),
        "04.11.03" => new TransportERP.EmptyForms.UcScreen_04_11_03(),
        "04.11.04" => new TransportERP.EmptyForms.UcScreen_04_11_04(),
        "04.11.05" => new TransportERP.EmptyForms.UcScreen_04_11_05(),
        "04.04.04" => new TransportERP.EmptyForms.UcScreen_04_04_04(),
        "04.04.05" => new TransportERP.EmptyForms.UcScreen_04_04_05(),
        "04.04.06" => new TransportERP.EmptyForms.UcScreen_04_04_06(),
        "04.11.06" => new TransportERP.EmptyForms.UcScreen_04_11_06(),
        "04.11.07" => new TransportERP.EmptyForms.UcScreen_04_11_07(),
        "ONYX:SCREEN-0078" => new TransportERP.EmptyForms.UcGeneralLedgerSettings(),
        "ONYX:SCREEN-0079" => new TransportERP.EmptyForms.UcOnyxSCREEN0079(),
        "ONYX:SCREEN-0080" => new TransportERP.EmptyForms.UcOnyxSCREEN0080(),
        "ONYX:SCREEN-0081" => new TransportERP.EmptyForms.UcOnyxSCREEN0081(),
        "ONYX:SCREEN-0082" => new TransportERP.EmptyForms.UcOnyxSCREEN0082(),
        "ONYX:SCREEN-0083" => new TransportERP.EmptyForms.UcOnyxSCREEN0083(),
        "ONYX:SCREEN-0084" => new TransportERP.EmptyForms.UcOnyxSCREEN0084(),
        "ONYX:SCREEN-0085" => new TransportERP.EmptyForms.UcOnyxSCREEN0085(),
        "ONYX:SCREEN-0086" => new TransportERP.EmptyForms.UcAccountOpeningRequest(),
        "ONYX:SCREEN-0089" => new TransportERP.EmptyForms.UcOnyxSCREEN0089(),
        "ONYX:SCREEN-0090" => new TransportERP.EmptyForms.UcOnyxSCREEN0090(),
        "ONYX:SCREEN-0091" => new TransportERP.EmptyForms.UcOnyxSCREEN0091(),
        "ONYX:SCREEN-0092" => new TransportERP.EmptyForms.UcOnyxSCREEN0092(),
        "ONYX:SCREEN-0093" => new TransportERP.EmptyForms.UcAccountCostCenterLinking(),
        "ONYX:SCREEN-0094" => new TransportERP.EmptyForms.UcAccountProjectLinking(),
        "ONYX:SCREEN-0098" => new TransportERP.EmptyForms.UcOnyxSCREEN0098(),
        "ONYX:SCREEN-0100" => new TransportERP.EmptyForms.UcOnyxSCREEN0100(),
        "ONYX:SCREEN-0102" => new TransportERP.EmptyForms.UcOnyxSCREEN0102(),
        "INV:EXTERNAL-REPAIR" => new TransportERP.EmptyForms.UcInventoryExternalRepairOrder(),
        "INV:CUSTODY-ISSUE" => new TransportERP.EmptyForms.UcInventoryCustodyIssue(),
        "INV:CUSTODY-RECEIPT" => new TransportERP.EmptyForms.UcInventoryCustodyReceipt(),
        "INV:DAMAGED-ISSUE" => new TransportERP.EmptyForms.UcInventoryDamagedIssueOrder(),
        "PUR:FOREIGN-RECEIPT" => new TransportERP.EmptyForms.UcForeignPurchaseReceipt(),
        "PUR:FOREIGN-COSTING" => new TransportERP.EmptyForms.UcForeignPurchaseCosting(),
        "INV:MANUAL-STOCKTAKE" => new TransportERP.EmptyForms.UcInventoryManualStocktake(),
        "INV:TRANSFER" => new TransportERP.EmptyForms.UcInventoryTransfer(),
        "INV:MATERIAL-REQUEST" => new TransportERP.EmptyForms.UcInventoryMaterialRequest(),
        "INV:TRANSFER-RECEIPT" => new TransportERP.EmptyForms.UcInventoryTransferReceipt(),
        "INV:SETTLEMENT" => new TransportERP.EmptyForms.UcInventorySettlement(),
        "INV:STOCKTAKE-REPORT" => new TransportERP.EmptyForms.UcInventoryStocktakeReport(),
        "INV:RECEIPT-AUTHORIZATION" => new TransportERP.EmptyForms.UcInventoryReceiptAuthorization(),
        "INV:RECEIPT-ORDER" => new TransportERP.EmptyForms.UcInventoryReceiptOrder(),
        "INV:ISSUE-ORDER" => new TransportERP.EmptyForms.UcInventoryIssueOrder(),
        "PUR:REQUEST" => new TransportERP.EmptyForms.UcPurchaseRequest(),
        "PUR:TECHNICAL-COMPARISON" => new TransportERP.EmptyForms.UcPurchaseTechnicalComparison(),
        "PUR:COMPARISON-NOMINATION" => new TransportERP.EmptyForms.UcPurchaseComparisonNomination(),
        "SUP:ADDITIONAL-PURCHASE-DISCOUNT" => new TransportERP.EmptyForms.UcPurchaseAdditionalDiscount(),
        "CUS:REPRESENTATIVE-COMMISSIONS" => new TransportERP.EmptyForms.UcRepresentativeCommissions(),
        "CUS:INSTALLMENT-SETTLEMENT" => new TransportERP.EmptyForms.UcCustomerInstallmentSettlement(),
        "CUS:DEBT-REPORT" => new TransportERP.EmptyForms.UcCustomerDebtReport(),
        "INV:QUANTITY-RESERVATION" => new TransportERP.EmptyForms.UcInventoryQuantityReservation(),
        "ONYX:SCREEN-0103" => new TransportERP.EmptyForms.UcOnyxSCREEN0103(),
        "ONYX:SCREEN-0104" => new TransportERP.EmptyForms.UcOnyxSCREEN0104(),
        "ONYX:SCREEN-0105" => new TransportERP.EmptyForms.UcOnyxSCREEN0105(),
        "ONYX:SCREEN-0106" => new TransportERP.EmptyForms.UcOnyxSCREEN0106(),
        "ONYX:SCREEN-0110" => new TransportERP.EmptyForms.UcOnyxSCREEN0110(),
        "ONYX:SCREEN-0111" => new TransportERP.EmptyForms.UcOnyxSCREEN0111(),
        "SHIP:001" => new UcShipmentTypes(),
        "SHIP:002" => new UcShipmentCategories(),
        "SHIP:003" => new UcShipmentUnits(),
        "SHIP:004" => new dgvShipmentPriorities(),
        "SHIP:005" => new UcTransportTypes(),
        "SHIP:006" => new UcTransportGroups(),
        "SHIP:007" => new UcShipmentTransportMethods(),
        "SHIP:008" => new UcRoutes(),
        "SHIP:009" => new UcShipmentPricingRules(),
        "SHIP:010" => new UcShipmentStatuses(),
        "SHIP:011" => new UcShipmentBooks(),
        "SHIP:012" => new TransportERP.Desktop.البوالص_والشحن.العمليات.البوليصه.UcShipmentWaybill(),
        "SHIP:013" => new TransportERP.Desktop.البوالص_والشحن.العمليات.الشحن_والترحيل.UcShipmentDispatch(),
        "SHIP:014" => new TransportERP.Desktop.البوالص_والشحن.العمليات.توريد_مخزني.UcWarehouseReceiptOrder(),
        _ => null
    };
}
