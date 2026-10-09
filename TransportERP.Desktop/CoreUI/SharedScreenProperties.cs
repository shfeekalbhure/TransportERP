using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace TransportERP.Desktop.CoreUI;

/// <summary>The screen's Designer owns its appearance; hosting may set workspace bounds only.</summary>
public interface IExplicitScreenLayout { }

/// <summary>Shared appearance and hosting policy for the existing screen trees.</summary>
public static class SharedScreenProperties
{
    private static readonly Color Surface = Color.LightCyan;
    private static readonly Color Background = Color.FromArgb(248, 250, 252);
    private static readonly Color ToolbarBackground = Color.FromArgb(224, 224, 224);
    private static readonly Font FieldFont = new("Segoe UI", 11F);
    private static readonly Font LabelFont = new("Segoe UI", 10F, FontStyle.Bold);
    private static readonly Font NormalFont = new("Segoe UI", 10F);
    private static readonly Font TabFont = new("Segoe UI", 9F);
    private static readonly Font ButtonFont = new("Microsoft Sans Serif", 10F);
    // Explicit semantic map: each screen/control pair was verified against its
    // designer Name and adjacent Arabic label/AccessibleName, not name heuristics.
    private static readonly HashSet<string> LeftToRightFields = new(StringComparer.Ordinal)
    {
        "UcUsersPermissions/txtBarcodePath",
        "UcCurrencyManagement/txtCurrencyNameEn",
        "UcCompanyManagement/txtPostalCode",
        "UcCompanyManagement/txtWebsite",
        "UcCompanyManagement/txtEmail",
        "UcCompanyManagement/txtMobile",
        "UcCompanyManagement/txtPhone",
        "UcCompanyManagement/txtCommercialRegistrationNo",
        "UcCompanyManagement/txtTaxNumber",
        "UcCompanyManagement/txtCompanyNameEn",
        "UcCompanyManagement/txtCompanyId",
        "UcCompanyManagement/txtCompanyCode",
        "UcDocumentNumbering/txtCode",
        "UcDocumentNumbering/txtEnglishName",
        "UcBranchManagement/txtPostalCode",
        "UcBranchManagement/txtWebsite",
        "UcBranchManagement/txtEmail",
        "UcBranchManagement/txtMobile",
        "UcBranchManagement/txtPhone",
        "UcBranchManagement/txtCommercialRegistrationNo",
        "UcBranchManagement/txtBranchNameEn",
        "UcBranchManagement/txtBranchId",
        "UcBranchManagement/txtBranchCode",
        "UcShipmentTransportMethods/txtTransportMethodNameEn",
        "UcShipmentTransportMethods/txtTransportMethodCode",
        "UcShipmentUnits/txtCategoryCode",
        "UcShipmentUnits/txtCategoryNameEn",
        "dgvShipmentServiceFees/txtServiceFeeNameEn",
        "dgvShipmentServiceFees/txtServiceFeeCode",
        "UcShipmentStatuses/txtStatusNameEn",
        "UcShipmentStatuses/txtStatusCode",
        "UcShipmentPricingRules/txtPricingRuleCode",
        "dgvShipmentPriorities/txtPriorityNameEn",
        "dgvShipmentPriorities/txtPriorityCode",
        "UcRoutes/txtRouteNameEn",
        "UcRoutes/txtRouteCode",
        "UcShipmentCategories/txtCategoryCode",
        "UcShipmentCategories/txtCategoryNameEn",
        "UcShipmentTypes/txtShipmentTypeNameEn",
        "UcShipmentTypes/txtShipmentTypeCode",
        "UcTransportTypes/txtTransportMethodNameEN",
        "UcTransportTypes/txtTransportMethodCode",
        "UcTransportGroups/txtTransportGroupNameEn",
        "UcTransportGroups/txtTransportGroupCode",
        "UcPackagingTypes/txtPackagingTypeNameEn",
        "UcPackagingTypes/txtPackagingTypeCode",
        "UcShipmentBooks/txtBookCode",
        "UcDriverTypes/txtPackagingTypeNameEn",
        "UcDriverTypes/txtPackagingTypeCode",
        "UcShipmentWaybill/txtReceiverPhone",
        "UcShipmentWaybill/txtReceiver",
        "UcShipmentWaybill/txtSenderPhone",
        "UcShipmentWaybill/txtSenderMobile",
        "UcShipmentDispatch/txtBarcode",
        "UcDispatchWaybillRow/txtBarcodeScan",
        "UcChartOfAccounts/txtAccountNameEn",
    };

    private sealed class ScreenState { public bool Embedded; }
    private static readonly ConditionalWeakTable<UserControl, ScreenState> States = new();

    public static void ConfigureWorkspace(UserControl screen)
        => ScreenProperties.ConfigureWorkspace(screen);

    // First step after InitializeComponent. A final ApplyContent call also covers
    // controls constructed in code and overrides made by screen-specific setup.
    public static void Apply(UserControl screen, bool embedded = false)
    {
        States.GetValue(screen, _ => new ScreenState()).Embedded = embedded;
        ApplyContent(screen);
    }

    public static void ApplyContent(UserControl screen)
    {
        screen.SuspendLayout();
        try
        {
            bool embedded = States.GetValue(screen, _ => new ScreenState()).Embedded;
            if (screen is IExplicitScreenLayout)
            {
                if (!embedded) ConfigureWorkspace(screen);
                return;
            }
            ScreenProperties.Apply(screen, embedded);
            // Reference dimensions were measured in the company's 120-DPI design.
            // Dpi mode records the pixel scale already applied to this layout.
            // None/code-built screens use unscaled 96-DPI logical dimensions.
            // Font-mode dimensions are preserved instead of inventing a DPI ratio.
            float? scale = screen.AutoScaleMode == AutoScaleMode.Dpi
                ? screen.AutoScaleDimensions.Width / 120F
                : screen.AutoScaleMode == AutoScaleMode.Font ? null : 96F / 120F;
            ApplyChildren(screen, screen, scale);
            if (!embedded) ConfigureWorkspace(screen);
        }
        finally { screen.ResumeLayout(true); }
    }

    private static int Pixels(float scale, int value) => (int)Math.Round(value * scale);

    // Explicit names verified in current designers, not inferred from button children.
    private static bool IsToolbar(Control control, UserControl root)
    {
        if (control is not FlowLayoutPanel) return false;
        return control.Name is "pnlToolbar" or "pnlActions" or "flpActions" or "flpClose"
            || (root.GetType().Name == "UcShipmentDispatch" && control.Name == "flowLayoutPanel1")
            || (root.GetType().Name == "UcUsersPermissions" && control.Name == "policyActionsV20")
            || (root.GetType().Name == "UcChangePassword" && control.Name == "passwordActions")
            || (root.GetType().Name == "UcScreen_02_04_06" && control.Name == "actions");
    }

    // Fiscal-year roles were verified from their existing container declarations.
    private static bool IsAuditFooter(Control control, UserControl root)
        => control is TableLayoutPanel && (control.Name == "tlpAuditInfo"
            || (root.GetType().Name == "UcScreen_02_04_06" && control.Name == "audit"));

    private static bool IsNavigationBar(Control control, UserControl root)
        => control is FlowLayoutPanel && root.GetType().Name == "UcScreen_02_04_06"
            && control.Name == "paging";

    private static bool Neutral(Color color)
        => color == SystemColors.Control || color == Color.White || color == Surface
            || color == Background || color == ToolbarBackground || color == Color.Transparent;

    private static void ApplyChildren(Control parent, UserControl root, float? scale)
    {
        foreach (Control control in parent.Controls)
        {
            // Command layout is persisted by its owning Designer.
            if (control is DesignerCommandBar) continue;
            // This verified command component predates the common policy. Its
            // actions need the same rules; other nested screens style themselves.
            if (control is UserControl nested)
            {
                if (root.GetType().Name == "UcUsersPermissions"
                    && nested.GetType().FullName == "TransportERP.Desktop.UsersCommandBar"
                    && nested.Name == "usersCommandBar1")
                {
                    ApplyChildren(nested, root, null);
                    if (nested.Controls["flpActions"] is FlowLayoutPanel commands)
                        commands.Dock = DockStyle.Top;
                    nested.Dock = DockStyle.Top;
                    nested.AutoSize = true;
                    nested.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    if (parent is Panel host && host.Name == "pnlActions")
                    {
                        host.AutoSize = true;
                        host.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    }
                }
                continue;
            }
            // A grid manages internal editing and scroll controls itself.
            if (control is DataGridView grid)
            {
                grid.RightToLeft = RightToLeft.Yes;
                grid.BackgroundColor = Color.White;
                grid.GridColor = ToolbarBackground;
                grid.Font = NormalFont;
                if (grid.DefaultCellStyle.Alignment is DataGridViewContentAlignment.NotSet or DataGridViewContentAlignment.MiddleLeft)
                    grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                if (grid.ColumnHeadersDefaultCellStyle.Alignment is DataGridViewContentAlignment.NotSet or DataGridViewContentAlignment.MiddleLeft)
                    grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                // Explicit column/row styles, numeric formats and state colors survive.
                continue;
            }
            // Root RTL is inherited; explicit child RightToLeft.No is semantic.
            bool toolbar = IsToolbar(control, root);
            bool header = control is Panel && control.Name == "pnlHeader";
            bool audit = IsAuditFooter(control, root);
            if ((control is Panel || control is GroupBox || control is TabPage) && Neutral(control.BackColor))
                control.BackColor = toolbar || header ? ToolbarBackground : audit ? Background : Surface;
            if (control is TableLayoutPanel)
            {
                control.Margin = Padding.Empty;
                if (scale is float tableScale)
                    control.Padding = audit ? new Padding(Pixels(tableScale, 8)) : Padding.Empty;
            }
            if (control is TabControl)
            {
                control.Font = TabFont;
                control.Margin = Padding.Empty;
                // Preserve RightToLeftLayout and owner-drawing coordinate policy.
            }
            if (control is TabPage page)
            {
                if (scale is float pageScale) page.Padding = new Padding(Pixels(pageScale, 3));
                page.Margin = Padding.Empty;
                page.UseVisualStyleBackColor = false;
            }
            if (toolbar && control is FlowLayoutPanel flow)
            {
                // Preserve the designer flow direction; RTL can mirror this order.
                flow.WrapContents = true;
                flow.AutoSize = true;
                flow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                // Verified PrintSettings host: wrapping must grow the outer bar,
                // not be confined to its former fixed 64-pixel height.
                if (root.GetType().Name == "UcPrintSettings" && flow.Name == "flpActions"
                    && flow.Parent is Panel host && host.Name == "pnlActions")
                {
                    flow.Dock = DockStyle.Top;
                    host.AutoSize = true;
                    host.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                }
                if (scale is float flowScale)
                {
                    flow.Padding = new Padding(Pixels(flowScale, 5));
                    flow.Margin = new Padding(Pixels(flowScale, 5));
                    flow.MinimumSize = new Size(0, Pixels(flowScale, 48));
                }
            }
            if (header && scale is float headerScale)
                control.MinimumSize = new Size(0, Pixels(headerScale, 48));
            bool field = control is TextBoxBase || control is ComboBox || control is DateTimePicker
                || control is NumericUpDown || control is CheckedListBox;
            if (field) control.Font = FieldFont;
            if (parent is TableLayoutPanel && (field || control is Label || control is CheckBox || control is RadioButton))
            {
                control.Dock = DockStyle.Fill;
                if (scale is float fieldScale) control.Margin = new Padding(Pixels(fieldScale, 3));
                if (control is Label && control.Font.SizeInPoints <= 11F)
                    control.Font = IsAuditFooter(parent, root) ? NormalFont : LabelFont;
            }
            switch (control)
            {
                case Label label:
                    if (label.TextAlign is not (ContentAlignment.TopCenter or ContentAlignment.MiddleCenter or ContentAlignment.BottomCenter))
                        label.TextAlign = ContentAlignment.MiddleRight;
                    break;
                case TextBox text:
                    if (text.RightToLeft != RightToLeft.No && text.TextAlign != HorizontalAlignment.Center)
                        text.TextAlign = HorizontalAlignment.Right;
                    break;
                case NumericUpDown number:
                    if (number.RightToLeft != RightToLeft.No) number.TextAlign = HorizontalAlignment.Right;
                    break;
                case CheckBox check:
                    check.TextAlign = ContentAlignment.MiddleRight;
                    check.CheckAlign = ContentAlignment.MiddleRight;
                    check.Font = NormalFont;
                    break;
                case RadioButton radio:
                    radio.TextAlign = ContentAlignment.MiddleRight;
                    radio.CheckAlign = ContentAlignment.MiddleRight;
                    radio.Font = NormalFont;
                    break;
                case Button button:
                    button.TextAlign = ContentAlignment.MiddleCenter;
                    button.Font = ButtonFont;
                    if (IsToolbar(parent, root) || IsNavigationBar(parent, root))
                    {
                        button.Dock = DockStyle.None;
                        // Content-aware minimum retains long approved Arabic captions.
                        button.AutoSize = true;
                        button.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                        if (scale is float buttonScale)
                        {
                            button.MinimumSize = new Size(Pixels(buttonScale, 70), Pixels(buttonScale, 30));
                            button.Margin = new Padding(Pixels(buttonScale, 4));
                        }
                        button.Padding = Padding.Empty;
                    }
                    break;
            }
            if (field && LeftToRightFields.Contains(root.GetType().Name + "/" + control.Name))
            {
                control.RightToLeft = RightToLeft.No;
                if (control is TextBox latin && latin.TextAlign != HorizontalAlignment.Center)
                    latin.TextAlign = HorizontalAlignment.Left;
            }
            // Preserve input backgrounds (required yellow), read-only state, text,
            // custom status colors, event wiring, row/column sizes and data formats.
            if (!field) ApplyChildren(control, root, scale);
        }
    }
}
