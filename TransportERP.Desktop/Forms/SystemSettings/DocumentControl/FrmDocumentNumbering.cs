using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.DocumentControl;

public partial class FrmDocumentNumbering : FrmBase
{
    public FrmDocumentNumbering()
    {
        InitializeComponent();
        ApplyRuntimeStyles();
        ApplyReferenceDesign();
        btnClose.Click += (_, _) => Close();
    }

    private sealed record ResetOption(string Code, string Caption);

    private void ApplyReferenceDesign()
    {
        // FIELD DESIGN MATRIX row 16: exact codes, translated display captions.
        cboResetType.Items.Clear();
        cboResetType.DisplayMember = nameof(ResetOption.Caption);
        cboResetType.ValueMember = nameof(ResetOption.Code);
        cboResetType.DataSource = new[] {
            new ResetOption("NONE", "بدون إعادة"), new ResetOption("COMPANY", "حسب الشركة"),
            new ResetOption("BRANCH", "حسب الفرع"), new ResetOption("YEAR", "حسب السنة"),
            new ResetOption("COMPANY_YEAR", "حسب الشركة والسنة"), new ResetOption("BRANCH_YEAR", "حسب الفرع والسنة") };
        cboResetType.SelectedValue = "NONE";
        cboResetType.Tag = "FLD-SET-NUM-001";
        lblResetType.Text = "سياسة إعادة الترقيم";
        colResetType.HeaderText = "سياسة إعادة الترقيم";
        cboResetType.DropDownWidth = ((ResetOption[])cboResetType.DataSource).Max(x => TextRenderer.MeasureText(x.Caption, cboResetType.Font).Width) + SystemInformation.VerticalScrollBarWidth + 16;
        var tabs = new TabControl { Name = "tabNumberingV20", Dock = DockStyle.Fill, RightToLeftLayout = true };
        var general = AddReferenceTab(tabs, "tabGeneral", "عام");
        var document = AddReferenceTab(tabs, "tabDocument", "حسب المستند");
        var exceptions = AddReferenceTab(tabs, "tabExceptions", "الاستثناءات");
        MoveReferenceField(general, lblPrefix, txtPrefix);
        MoveReferenceField(document, lblDocumentType, cboDocumentType);
        MoveReferenceField(document, lblLastNumber, nudLastNumber);
        MoveReferenceField(document, lblNextNumber, nudNextNumber);
        MoveReferenceField(document, lblDigits, nudDigits);
        MoveReferenceField(document, null, chkActive);
        MoveReferenceField(document, lblPreviewCaption, lblNumberPreview);
        MoveReferenceField(exceptions, lblResetType, cboResetType);
        grpDetails.Controls.Remove(tlpDetails);
        tlpDetails.Dispose();
        grpDetails.Controls.Add(tabs);

        var additions = new FlowLayoutPanel { Name = "numberingPolicyActions", Dock = DockStyle.Bottom, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, FlowDirection = FlowDirection.RightToLeft, WrapContents = true, Padding = new Padding(16, 8, 16, 8) };
        foreach (var action in new[] { ("SaveDraft", "حفظ مسودة"), ("Validate", "تحقق"), ("Publish", "نشر/اعتماد"), ("RevertScopeOverride", "إلغاء تجاوز النطاق"), ("ViewAudit", "عرض التدقيق"), ("Refresh", "تحديث") })
        {
            var button = new Button { Name = "btnV20" + action.Item1, Text = action.Item2, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(16, 4, 16, 4), Margin = new Padding(4) };
            SettingsFormStyle.ApplySecondaryButtonStyle(button);
            button.MinimumSize = new Size(TextRenderer.MeasureText(button.Text, button.Font).Width + button.Padding.Horizontal, Math.Max(40, button.GetPreferredSize(Size.Empty).Height));
            additions.Controls.Add(button);
        }
        Controls.Add(additions);
        Controls.SetChildIndex(additions, 1);
        MinimumSize = new Size(MinimumSize.Width, MinimumSize.Height + additions.GetPreferredSize(new Size(ClientSize.Width, 0)).Height);
    }

    private static TableLayoutPanel AddReferenceTab(TabControl tabs, string name, string caption)
    {
        var page = new TabPage(caption) { Name = name, Padding = new Padding(16), AutoScroll = true };
        var fields = new TableLayoutPanel { Name = name + "Fields", Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, ColumnCount = 2, RightToLeft = RightToLeft.Yes };
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        page.Controls.Add(fields);
        tabs.TabPages.Add(page);
        return fields;
    }

    private static void MoveReferenceField(TableLayoutPanel fields, Label? label, Control input)
    {
        int row = fields.RowCount++;
        fields.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        if (label is not null)
        {
            label.Dock = DockStyle.None;
            label.AutoSize = true;
            label.MinimumSize = TextRenderer.MeasureText(label.Text, label.Font);
            label.Anchor = AnchorStyles.Right;
            label.Margin = new Padding(4, 8, 16, 8);
            fields.Controls.Add(label, 0, row);
        }
        input.Dock = DockStyle.None;
        input.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        input.Margin = new Padding(8);
        fields.Controls.Add(input, 1, row);
    }

    private void ApplyRuntimeStyles()
    {
        SettingsFormStyle.ApplyFormStyle(this, true);
        SettingsFormStyle.ApplyHeaderStyle(pnlHeader, lblTitle, lblSubtitle);
        SettingsFormStyle.ApplySectionStyle(grpDetails);
        SettingsFormStyle.ApplyGridStyle(dgvDocumentSequences);
        dgvDocumentSequences.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        foreach (var label in new[] { lblCompany, lblBranch, lblFiscalYear, lblDocumentType, lblPrefix, lblLastNumber, lblNextNumber, lblDigits, lblResetType, lblPreviewCaption }) SettingsFormStyle.ApplyLabelStyle(label);
        foreach (var label in new[] { lblCompany, lblBranch, lblFiscalYear, lblDocumentType, lblNextNumber, lblDigits }) SettingsFormStyle.ApplyRequiredLabelStyle(label);
        foreach (Control input in new Control[] { cboCompany, cboBranch, cboFiscalYear, cboDocumentType, txtPrefix, nudLastNumber, nudNextNumber, nudDigits, cboResetType }) SettingsFormStyle.ApplyInputStyle(input);
        SettingsFormStyle.ApplyReadOnlyStyle(nudLastNumber);
        SettingsFormStyle.ApplyInfoStyle(lblNumberPreview);
        SettingsFormStyle.ApplyNeutralButtonStyle(btnNew);
        SettingsFormStyle.ApplyPrimaryButtonStyle(btnSave);
        SettingsFormStyle.ApplySecondaryButtonStyle(btnEdit);
        SettingsFormStyle.ApplyDangerButtonStyle(btnDisable);
        SettingsFormStyle.ApplyNeutralButtonStyle(btnPrint);
        SettingsFormStyle.ApplySecondaryButtonStyle(btnClose);
        lblTitle.TextAlign = ContentAlignment.MiddleLeft;
        lblSubtitle.TextAlign = ContentAlignment.MiddleLeft;
        MinimumSize = SizeFromClientSize(new Size(1180, 760));
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScaleDimensions = new SizeF(96F, 96F);
        PerformAutoScale();
    }
}
