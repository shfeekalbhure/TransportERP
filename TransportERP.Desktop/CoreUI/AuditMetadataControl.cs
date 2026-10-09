using System.ComponentModel;

namespace TransportERP.Desktop.CoreUI;

/// <summary>Designer-backed, display-only audit footer. No persistence or inferred record values.</summary>
public partial class AuditMetadataControl : UserControl, IExplicitScreenLayout
{
    public AuditMetadataControl() { InitializeComponent(); }

    private AuditMetadataProfile profile;
    [Category("Audit"), DefaultValue(AuditMetadataProfile.Standard)]
    [Description("The persisted audit layout used by this screen in the Designer and at runtime.")]
    public AuditMetadataProfile Profile
    {
        get => profile;
        set
        {
            if (profile == value) return;
            if (!Enum.IsDefined(value)) throw new ArgumentOutOfRangeException(nameof(value));
            profile = value;
            bool single = value is AuditMetadataProfile.ModificationOnly or AuditMetadataProfile.CyanModification;
            SuspendLayout();
            foreach (Control child in auditInfoContainer.Controls)
                if (auditInfoContainer.GetRow(child) == 0) child.Visible = !single;
            auditInfoContainer.RowStyles[0].SizeType = single ? SizeType.Absolute : SizeType.Percent;
            auditInfoContainer.RowStyles[0].Height = single ? 0 : 50;
            auditInfoContainer.RowStyles[1].SizeType = SizeType.Percent;
            auditInfoContainer.RowStyles[1].Height = single ? 100 : 50;
            auditInfoContainer.MinimumSize = new Size(LogicalToDeviceUnits(800), LogicalToDeviceUnits(single ? 32 : 60));
            auditInfoContainer.BackColor = value == AuditMetadataProfile.CyanModification ? Color.FromArgb(48,190,238)
                : value == AuditMetadataProfile.ReceiptReference ? Color.FromArgb(244,243,210) : Color.FromArgb(232,227,246);
            if (value == AuditMetadataProfile.ReceiptReference) EnsureReceiptInformation();
            if (Controls.Find("auditReceiptInformation",false).FirstOrDefault() is Control header)
                header.Visible = value == AuditMetadataProfile.ReceiptReference;
            int details = Controls.OfType<Label>().Where(c => c.Name == "auditAdditionalMetadata").Sum(c => c.Height);
            int height = LogicalToDeviceUnits(single ? 36 : value == AuditMetadataProfile.ReceiptReference ? 90 : 64) + details;
            MaximumSize = Size.Empty;
            MinimumSize = new Size(0,height);
            Height = height;
            ResumeLayout(true);
        }
    }

    internal TextBox Value(string key) => (TextBox)Controls.Find("auditValue" + key, true).Single();

    /// <summary>True for presentation controls, but false for retained original
    /// sources. Data collectors can exclude the mirrored editors without losing
    /// the original record fields after the adapter reparents them.</summary>
    public static bool IsPresentationControl(Control control)
    {
        for (Control? current = control; current is not null; current = current.Parent)
        {
            if (current.Name is "auditLegacySources" or "auditLegacySourcesDesigner") return false;
            if (current is AuditMetadataControl) return true;
        }
        return false;
    }

    internal void ReceiptReference()
        => Profile = AuditMetadataProfile.ReceiptReference;

    private void EnsureReceiptInformation()
    {
        if (Controls.Find("auditReceiptInformation",false).Length != 0) return;
        // PDF p.35 has the common four audit groups, with the account name above.
        // Keep the existing last-print timestamp in that additional information band.
        auditInfoContainer.BackColor = Color.FromArgb(244,243,210);
        var header = new TableLayoutPanel { Name="auditReceiptInformation", Dock=DockStyle.Top,
            Height=LogicalToDeviceUnits(26), Margin=Padding.Empty, Padding=Padding.Empty, RowCount=1, ColumnCount=4,
            RightToLeft=RightToLeft.Yes, BackColor=auditInfoContainer.BackColor };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,110));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,70));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,110));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,30));
        var account = CreateReceiptMetadataField("AccountName","اسم الحساب");
        var printed = CreateReceiptMetadataField("LastPrintedAt","تاريخ آخر طباعة");
        header.Controls.Add(account.Caption,0,0); header.Controls.Add(account.Value,1,0);
        header.Controls.Add(printed.Caption,2,0); header.Controls.Add(printed.Value,3,0);
        Controls.Add(header);
    }

    internal void ModificationOnly(bool cyan)
        => Profile = cyan ? AuditMetadataProfile.CyanModification : AuditMetadataProfile.ModificationOnly;

    // Unknown legacy metadata is retained visibly instead of silently dropping a field.
    internal void AddLegacyDetails(IEnumerable<Control> sources)
    {
        var items = sources.ToArray();
        if (items.Length == 0) return;
        var details = new Label { Name = "auditAdditionalMetadata", Dock = DockStyle.Bottom,
            Height = 26, AutoEllipsis = true, TextAlign = ContentAlignment.MiddleRight,
            Font = Font, ForeColor=Color.FromArgb(32,32,32), RightToLeft=RightToLeft.No,
            BackColor = auditInfoContainer.BackColor, TabStop = false };
        void Refresh(object? sender, EventArgs e) => details.Text = string.Join("   |   ",
            items.Select(x => x.Text).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct());
        Refresh(null, EventArgs.Empty);
        foreach (var source in items) source.TextChanged += Refresh;
        Disposed += (_, _) => { foreach (var source in items) source.TextChanged -= Refresh; };
        Controls.Add(details);
        details.SendToBack(); // Reserve its band before the Fill table is laid out.
        Height += 26;
        MinimumSize = new Size(0, Height);
    }

    internal void BindValue(string key, Control source, bool captioned)
    {
        var target = Value(key);
        void Refresh(object? sender, EventArgs e)
        {
            string value = source.Text;
            if (captioned && value.IndexOf(':') is var colon && colon >= 0)
                value = value[(colon + 1)..].Trim();
            target.Text = value;
        }
        Refresh(null, EventArgs.Empty);
        source.TextChanged += Refresh;
        Disposed += (_, _) => source.TextChanged -= Refresh;
    }

    internal void BindAggregateValue(string key, Control source, string caption, IEnumerable<string> captions)
    {
        var target = Value(key);
        string nextCaption = string.Join("|", captions.Select(System.Text.RegularExpressions.Regex.Escape));
        string pattern = System.Text.RegularExpressions.Regex.Escape(caption)
            + @"\s*:\s*(.*?)(?=\s{2,}(?:" + nextCaption + @")\s*:|\r?\n|$)";
        void Refresh(object? sender, EventArgs e)
        {
            var match=System.Text.RegularExpressions.Regex.Match(source.Text,pattern,
                System.Text.RegularExpressions.RegexOptions.Singleline);
            target.Text=match.Success?match.Groups[1].Value.Trim():string.Empty;
        }
        Refresh(null,EventArgs.Empty);
        source.TextChanged+=Refresh;
        Disposed+=(_,_)=>source.TextChanged-=Refresh;
    }
}
