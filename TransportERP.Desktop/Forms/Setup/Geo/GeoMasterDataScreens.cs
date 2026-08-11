using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.Forms.Setup.Geo;

/// <summary>Shared MasterData composition for GEN-003..GEN-007. It contains no local toolbar, paging, validation or lookup implementation.</summary>
public abstract class GeoMasterDataScreen : UserControl
{
    protected GeoMasterDataScreen(string screenCode, string title, string parentLabel, bool hasParent, bool hasNationality = false)
    {
        ScreenCode = screenCode;
        Dock = DockStyle.Fill; RightToLeft = RightToLeft.Yes;
        Shell = new TransportReferenceScreenShell { Dock = DockStyle.Fill, DataGroupTitle = title };
        Shell.Toolbar.SetActionVisible(ToolbarAction.Delete, false); // Disable is the approved lifecycle operation.
        Shell.Toolbar.SetActionVisible(ToolbarAction.Print, false);  // GEO API contract contains no print endpoint.
        Shell.SearchPanel.SearchPlaceholder = "بحث بالرمز أو الاسم العربي أو الاسم الإنجليزي";
        Shell.SearchPanel.SetStatusItems("نشط", "موقوف");

        var data = new TransportDataEntryPanel { FieldColumnCount = 2 };
        if (hasParent)
        {
            ParentLookup = new LookupComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            data.AddField(parentLabel, ParentLookup, 0);
        }
        Code = new RequiredTextBox { MaxLength = 64, RequiredMessage = "الرمز مطلوب." };
        ArabicName = new RequiredTextBox { MaxLength = 200, RequiredMessage = "الاسم العربي مطلوب." };
        EnglishName = new TextBox { MaxLength = 200 };
        var index = hasParent ? 1 : 0;
        data.AddField("الرمز *", Code, index++);
        data.AddField("الاسم العربي *", ArabicName, index++);
        data.AddField("الاسم الإنجليزي", EnglishName, index++);
        if (hasNationality)
        {
            NationalityName = new TextBox { MaxLength = 200 };
            data.AddField("اسم الجنسية", NationalityName, index++);
        }
        IsActive = new CheckBox { Text = "نشط", Checked = true, AutoSize = true };
        data.AddField("الحالة", IsActive, index);
        Shell.DataHost.Controls.Add(data);
        Shell.Grid.AutoGenerateColumns = false;
        Shell.Grid.Columns.Add("Code", "الرمز"); Shell.Grid.Columns.Add("ArabicName", "الاسم العربي"); Shell.Grid.Columns.Add("EnglishName", "الاسم الإنجليزي");
        if (hasParent) Shell.Grid.Columns.Add("ParentName", parentLabel);
        Shell.Grid.Columns.Add("IsActive", "الحالة");
        Controls.Add(Shell);
    }

    public string ScreenCode { get; }
    public TransportReferenceScreenShell Shell { get; }
    public LookupComboBox? ParentLookup { get; }
    public RequiredTextBox Code { get; }
    public RequiredTextBox ArabicName { get; }
    public TextBox EnglishName { get; }
    public TextBox? NationalityName { get; }
    public CheckBox IsActive { get; }
}

public sealed class FrmCountries : GeoMasterDataScreen { public FrmCountries() : base("GEN-003", "الدول", string.Empty, false, true) { } }
public sealed class FrmGovernorates : GeoMasterDataScreen { public FrmGovernorates() : base("GEN-004", "المحافظات", "الدولة *", true) { } }
public sealed class FrmDirectorates : GeoMasterDataScreen { public FrmDirectorates() : base("GEN-005", "المديريات", "المحافظة *", true) { } }
public sealed class FrmCities : GeoMasterDataScreen { public FrmCities() : base("GEN-006", "المدن", "المديرية *", true) { } }
public sealed class FrmAreas : GeoMasterDataScreen { public FrmAreas() : base("GEN-007", "المناطق", "المدينة *", true) { } }
