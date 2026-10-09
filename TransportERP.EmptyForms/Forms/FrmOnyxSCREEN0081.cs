using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
/// <summary>Source-backed notice type header and permissions tab; permission row schema is a documented local design.</summary>
public partial class UcOnyxSCREEN0081 : UserControl, IBatchFiveScreen, IWorkspaceChangeState
{
    public BatchFiveUiSession Binding { get; }
    public bool HasUnsavedChanges => Binding.HasPendingChanges;
    public UcOnyxSCREEN0081()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        Binding = new BatchFiveUiSession(this, lblDataStatus, errors,
            new Dictionary<string, BatchFiveField>
            {
                ["Code"] = new(txtCode, true, false, false),
                ["LocalName"] = new(txtLocalName, true, false, false),
                ["ForeignName"] = new(txtForeignName, false, false, false),
                ["NotificationKind"] = new(cmbNotificationKind, true, false, false)
            }, dgvPermissions, new Dictionary<string, Button> { ["Save"] = btnSave },
            detailsTabs, btnNew, btnPermissionAdd, btnPermissionRemove);
        Binding.ValidateCommand = _ => ValidatePermissionDraft();
        btnValidate.Click += (_, _) => lblDataStatus.Text =
            string.IsNullOrWhiteSpace(txtCode.Text) || string.IsNullOrWhiteSpace(txtLocalName.Text) || cmbNotificationKind.SelectedIndex < 0
                ? "رقم النوع والاسم المحلي ونوع الإشعار مطلوبة."
                : ValidatePermissionDraft() ?? "اجتاز التحقق المحلي؛ لم يتم الحفظ أو تطبيق الصلاحيات.";
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
    private string? ValidatePermissionDraft()
    {
        if (!dgvPermissions.EndEdit()) return "أكمل تحرير الخلية الحالية.";
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (DataGridViewRow row in dgvPermissions.Rows)
        {
            var values = new[] { "PrincipalType", "PrincipalId", "PermissionCode" }
                .Select(k => Convert.ToString(row.Cells[k].Value) ?? "").ToArray();
            if (values.Any(string.IsNullOrWhiteSpace)) return $"سطر الصلاحيات {row.Index + 1}: نوع الجهة والجهة والصلاحية مطلوبة.";
            if (row.Cells["Allowed"].Value is not bool && row.Cells["Allowed"].Value is not CheckState.Checked && row.Cells["Allowed"].Value is not CheckState.Unchecked) return $"سطر الصلاحيات {row.Index + 1}: حدد السماح أو المنع.";
            if (!seen.Add(string.Join("\u001f", values))) return "الصلاحية مكررة للجهة نفسها.";
        }
        return null;
    }
    public event EventHandler? CloseRequested;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);
}
