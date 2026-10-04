using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TransportERP.Desktop.Forms.Setup.Security;
using TransportERP.Desktop.Forms.SystemSettings.General;
using TransportERP.Desktop.Forms.SystemSettings.DocumentControl;
using TransportERP.Desktop.البوالص_والشحن.الاعدادات;
namespace TransportERP.Desktop;
public partial class FrmMain
{
    internal static readonly (string Code, string Title)[] PhaseOneScreens =
    {
            ("01.01", "شاشة تسجيل الدخول"),
            ("01.02", "الشاشة الرئيسية"),
            ("02.01.01", "إدارة الشركات"),
            ("02.02.01", "إدارة الفروع"),
            ("02.03.01", "المستخدمون"),
            ("02.03.02", "الأدوار"),
            ("02.03.03", "الصلاحيات"),
            ("02.03.06", "تغيير كلمة السر"),
            ("02.04.01", "إعدادات الترقيم"),
            ("02.04.02", "إدارة العملات"),
            ("02.04.03", "أسعار الصرف"),
            ("02.05.21", "ترميز اللغات"),
            ("03.01.01", "إدارة العملاء"),
            ("03.02.01", "إدارة الموردين"),
            ("03.03.01", "إدارة الوكلاء"),
            ("04.01.01", "دليل الحسابات"),
            ("04.02.01", "إدارة مراكز التكلفة"),
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
            ("02.02.04", "الدول"),
            ("02.02.05", "المحافظات"),
            ("02.02.06", "المديريات"),
            ("02.02.07", "المدن"),
            ("02.02.08", "المناطق"),
            ("02.04.06", "السنوات المالية"),
            ("02.04.07", "إعدادات التشغيل العامة والمتغيرات المشتركة"),
            ("04.01.02", "مجموعات وأنواع الحسابات"),
            ("04.03.03", "الحسابات البنكية"),
            ("04.03.04", "طرق الدفع"),
            ("04.11.01", "الفترات المحاسبية"),
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
    };
    private void InitializePhaseOneNavigation()
    {
        // This isolated integration contains only the approved phase-one registry.
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
            var parties = Group("parties", "الأطراف", root);
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
                    "SHIP:001" or "SHIP:002" or "SHIP:003" or "SHIP:004" or "SHIP:005" or "SHIP:006" or "SHIP:007" => shipmentsSetup,
                    "SHIP:012" => shipmentsOperations,
                    "SHIP:013" => shipmentsOperations,
                    "02.04.07" or "02.04.01" => setupGeneral,
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
            else
            {
                root.Expand();
                setup.Expand();
                setupInitial.Expand();
                shipments.Expand();
                shipmentsSetup.Expand();
            }
            SetNavigationStatus(null);
        }
        finally { tvSystemTree.EndUpdate(); }
    }
    internal static Form? CreatePhaseOneScreen(string code) => code switch
    {
            "01.01" => new FrmLogin(),
            "01.02" => null,
            "02.01.01" => new FrmCompanyManagement(),
            "02.02.01" => new FrmBranchManagement(),
            "02.03.01" => new FrmUsersPermissions(),
            "02.03.02" => new FrmUsersPermissions("tabRolesV20"),
            "02.03.03" => new FrmUsersPermissions("tabPermissions"),
            "02.03.06" => new FrmChangePassword(),
            "02.04.01" => new FrmDocumentNumbering(),
            "02.04.02" => new FrmCurrencyManagement(),
            "02.04.03" => new FrmExchangeRates(),
            "02.05.21" => new TransportERP.EmptyForms.FrmScreen_02_05_21(),
            "03.01.01" => new TransportERP.EmptyForms.FrmScreen_03_01_01(),
            "03.02.01" => new TransportERP.EmptyForms.FrmScreen_03_02_01(),
            "03.03.01" => new TransportERP.EmptyForms.FrmScreen_03_03_01(),
            "04.01.01" => new TransportERP.EmptyForms.FrmScreen_04_01_01(),
            "04.02.01" => new TransportERP.EmptyForms.FrmScreen_04_02_01(),
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
            "02.02.04" => new TransportERP.EmptyForms.FrmScreen_02_02_04(),
            "02.02.05" => new TransportERP.EmptyForms.FrmScreen_02_02_05(),
            "02.02.06" => new TransportERP.EmptyForms.FrmScreen_02_02_06(),
            "02.02.07" => new TransportERP.EmptyForms.FrmScreen_02_02_07(),
            "02.02.08" => new TransportERP.EmptyForms.FrmScreen_02_02_08(),
            "02.04.06" => new TransportERP.EmptyForms.FrmScreen_02_04_06(),
            "02.04.07" => new FrmGeneralSettings(),
            "04.01.02" => new TransportERP.EmptyForms.FrmScreen_04_01_02(),
            "04.03.03" => new TransportERP.EmptyForms.FrmScreen_04_03_03(),
            "04.03.04" => new TransportERP.EmptyForms.FrmScreen_04_03_04(),
            "04.11.01" => new TransportERP.EmptyForms.FrmScreen_04_11_01(),
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
            "ONYX:SCREEN-0103" => new TransportERP.EmptyForms.FrmOnyxSCREEN0103(),
            "ONYX:SCREEN-0104" => new TransportERP.EmptyForms.FrmOnyxSCREEN0104(),
            "ONYX:SCREEN-0105" => new TransportERP.EmptyForms.FrmOnyxSCREEN0105(),
            "ONYX:SCREEN-0106" => new TransportERP.EmptyForms.FrmOnyxSCREEN0106(),
            "ONYX:SCREEN-0110" => new TransportERP.EmptyForms.FrmOnyxSCREEN0110(),
            "ONYX:SCREEN-0111" => new TransportERP.EmptyForms.FrmOnyxSCREEN0111(),
        _ => null
    };
}
