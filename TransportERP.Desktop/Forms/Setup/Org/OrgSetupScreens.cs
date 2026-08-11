using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.Forms.Setup.Org;

/// <summary>CoreUI-only declarations for the authorised GEN-008..015 setup screens. Data access remains HTTP/API-only.</summary>
public abstract class OrgSetupScreen : UserControl
{
    protected OrgSetupScreen(string screenCode, string title, params string[] fields)
    {
        ScreenCode = screenCode; Dock = DockStyle.Fill; RightToLeft = RightToLeft.Yes;
        Shell = new TransportReferenceScreenShell { Dock = DockStyle.Fill, DataGroupTitle = title };
        Shell.Toolbar.SetActionVisible(ToolbarAction.Delete, false); Shell.Toolbar.SetActionVisible(ToolbarAction.Print, false);
        Shell.SearchPanel.SearchPlaceholder = "بحث بالرمز أو الاسم"; Shell.SearchPanel.SetStatusItems("نشط", "موقوف");
        var data = new TransportDataEntryPanel { FieldColumnCount = 2 };
        Code = new RequiredTextBox { MaxLength = 100, RequiredMessage = "الرمز مطلوب." }; ArabicName = new RequiredTextBox { MaxLength = 200, RequiredMessage = "الاسم العربي مطلوب." }; EnglishName = new TextBox { MaxLength = 200 };
        data.AddField("الرمز *", Code, 0); data.AddField("الاسم العربي *", ArabicName, 1); data.AddField("الاسم الإنجليزي", EnglishName, 2);
        for (var i = 0; i < fields.Length; i++) { var value = new TextBox { MaxLength = 500 }; ExtraFields.Add(fields[i], value); data.AddField(fields[i], value, i + 3); }
        IsActive = new CheckBox { Text = "نشط", Checked = true, AutoSize = true }; data.AddField("الحالة", IsActive, fields.Length + 3); Shell.DataHost.Controls.Add(data);
        Shell.Grid.AutoGenerateColumns = false; Shell.Grid.Columns.Add("Code", "الرمز"); Shell.Grid.Columns.Add("ArabicName", "الاسم العربي"); Shell.Grid.Columns.Add("EnglishName", "الاسم الإنجليزي"); Shell.Grid.Columns.Add("IsActive", "الحالة"); Controls.Add(Shell);
    }
    public string ScreenCode { get; } public TransportReferenceScreenShell Shell { get; } public RequiredTextBox Code { get; } public RequiredTextBox ArabicName { get; } public TextBox EnglishName { get; } public CheckBox IsActive { get; } public Dictionary<string, TextBox> ExtraFields { get; } = new();
}
public sealed class FrmCurrencies : OrgSetupScreen { public FrmCurrencies() : base("GEN-008", "العملات", "الرمز الدولي", "الرمز", "المنازل العشرية") { } }
public sealed class FrmExchangeRates : OrgSetupScreen { public FrmExchangeRates() : base("GEN-009", "أسعار الصرف", "الشركة", "العملة الأساسية", "عملة المقارنة", "السعر", "الحد الأدنى", "الحد الأعلى", "تاريخ السريان") { } }
public sealed class FrmCompanies : OrgSetupScreen { public FrmCompanies() : base("GEN-010", "الشركات", "الاسم القانوني", "العملة الأساسية", "الرقم الضريبي") { } }
public sealed class FrmBranches : OrgSetupScreen { public FrmBranches() : base("GEN-011", "الفروع", "الشركة", "المنطقة الزمنية") { } }
public sealed class FrmFiscalYears : OrgSetupScreen { public FrmFiscalYears() : base("GEN-012", "السنوات المالية", "الشركة", "تاريخ البداية", "تاريخ النهاية", "الحالة") { } }
public sealed class FrmNumbering : OrgSetupScreen { public FrmNumbering() : base("GEN-013", "الترقيم العام", "نوع النطاق", "نوع المستند", "البادئة", "آخر رقم", "سياسة إعادة الضبط") { } }
public sealed class FrmLanguages : OrgSetupScreen { public FrmLanguages() : base("GEN-014", "اللغات", "رمز اللغة", "اتجاه العرض") { } }
public sealed class FrmOperationalSettings : OrgSetupScreen { public FrmOperationalSettings() : base("GEN-015", "إعدادات التشغيل العامة", "مفتاح الإعداد", "النطاق", "القيمة", "تاريخ السريان") { } }
