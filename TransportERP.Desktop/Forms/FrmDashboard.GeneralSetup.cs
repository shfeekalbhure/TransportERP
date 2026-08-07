using TransportERP.Desktop.Forms.Setup.General;

namespace TransportERP.Desktop;

/// <summary>
/// ربط المجموعة الثانية — التهيئة العامة — بمساحة العمل الحالية.
/// يبقى فتح الشاشة داخل Tab واحد، ويمنع OpenWorkspaceView إنشاء نسخة ثانية من نفس الشاشة.
/// </summary>
public partial class FrmDashboard
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ConfigureGeneralSetupGroupTwoMenu();
    }

    private void ConfigureGeneralSetupGroupTwoMenu()
    {
        if (_generalSetupMenu is null || _generalSetupMenu.Items.Find("mnuGen008VehicleTypes", false).Length > 0)
        {
            return;
        }

        _generalSetupMenu.Items.Add(new ToolStripSeparator());
        _generalSetupMenu.Items.Add(CreateSetupItem("mnuGen008VehicleTypes", "GEN-008 — أنواع المركبات", () => OpenWorkspaceView("GEN-008", "أنواع المركبات", new UcGen008VehicleTypes())));
        _generalSetupMenu.Items.Add(CreateSetupItem("mnuGen009Currencies", "GEN-009 — العملات", () => OpenWorkspaceView("GEN-009", "العملات", new UcGen009Currencies())));
        _generalSetupMenu.Items.Add(CreateSetupItem("mnuGen010ExchangeRates", "GEN-010 — أسعار الصرف", () => OpenWorkspaceView("GEN-010", "أسعار الصرف", new UcGen010ExchangeRates())));
        _generalSetupMenu.Items.Add(CreateSetupItem("mnuGen011Companies", "GEN-011 — الشركات", () => OpenWorkspaceView("GEN-011", "الشركات", new UcGen011Companies())));
        _generalSetupMenu.Items.Add(CreateSetupItem("mnuGen012Branches", "GEN-012 — الفروع", () => OpenWorkspaceView("GEN-012", "الفروع", new UcGen012Branches())));
        _generalSetupMenu.Items.Add(CreateSetupItem("mnuGen013FiscalYears", "GEN-013 — السنوات المالية", () => OpenWorkspaceView("GEN-013", "السنوات المالية", new UcGen013FiscalYears())));
        _generalSetupMenu.Items.Add(CreateSetupItem("mnuGen014Numbering", "GEN-014 — الترقيم العام", () => OpenWorkspaceView("GEN-014", "الترقيم العام", new UcGen014Numbering())));
        _generalSetupMenu.Items.Add(CreateSetupItem("mnuGen015Languages", "GEN-015 — اللغات", () => OpenWorkspaceView("GEN-015", "اللغات", new UcGen015Languages())));
        _generalSetupMenu.Items.Add(CreateSetupItem("mnuGen016GlobalVariables", "GEN-016 — المتغيرات العامة", () => OpenWorkspaceView("GEN-016", "المتغيرات العامة", new UcGen016GlobalVariables())));
    }

    private static ToolStripMenuItem CreateSetupItem(string name, string text, Action openAction)
    {
        var item = new ToolStripMenuItem(text)
        {
            Name = name,
            RightToLeft = RightToLeft.Yes,
            ToolTipText = text
        };
        item.Click += (_, _) => openAction();
        return item;
    }
}
