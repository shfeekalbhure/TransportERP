extern alias SharedCommands;
using TransportERP.Desktop.CoreUI;
using SharedCommands::TransportERP.Desktop.SharedUI.Commands;

namespace TransportERP.Desktop.Forms.Setup.Security;

public partial class UcUsersPermissions : UserControl, IWorkspaceCloseGuard
{
    public event EventHandler? CloseRequested;

    private bool updatingPermissionChecks;
    private WorkspaceChangeSnapshot newUserSnapshot = null!;
    private readonly ToolbarCommandBindings<UsersToolbarCommand> userCommandBindings = new();

    public UcUsersPermissions()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
        RightToLeft = RightToLeft.Yes;
        Font = new Font("Segoe UI", 10F);
        BackColor = Color.LightCyan;
        // The isolated review form takes its appearance from the Visual Studio Designer.
        dgvModulePermissions.AllowUserToAddRows = false;
        dgvModulePermissions.AllowUserToDeleteRows = false;
        InitializePermissionTree();
        dgvModulePermissions.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        WireUiEvents();
        WireRoleCatalog();
        ApplySourceAdditionsSizing();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.ApplyContent(this);
        DisableUnimplementedPolicyActions();
        newUserSnapshot = ResetUserSnapshot();
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public UcUsersPermissions(string initialTab) : this()
    {
        SelectSection(initialTab);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public void SelectSection(string tabName)
    {
        var page = tabUserDetails.TabPages.Cast<TabPage>().Single(p => p.Name == tabName);
        tabUserDetails.SelectedTab = page;
    }

    // Source fields: Infrastructure/Persistence/P1Entities.cs Role. UI draft operations are local.
    public sealed record RoleChoice(string Id, string Label);
    public sealed record RoleDraft(string? Id, string Code, string NameAr, string? NameEn,
        string? Description, string? CompanyId, string Status, bool IsSystem);
    public sealed record RoleSaveResult(bool Persisted, string Message, IReadOnlyList<RoleDraft>? Catalog = null);
    private Func<IReadOnlyList<RoleDraft>, Task<RoleSaveResult>>? saveRoles;
    public bool IsBusy { get; private set; }
    private List<WorkspaceChangeSnapshot> userSnapshots = new();
    public bool HasUnsavedChanges => userSnapshots.Any(s => s.HasChanges) || roleSnapshot.HasChanges || dgvRoleCatalog.IsCurrentCellDirty;
    private WorkspaceChangeSnapshot ResetUserSnapshot()
    {
        userSnapshots.Clear();
        void Visit(Control c)
        {
            if (ReferenceEquals(c, tabRolesV20)) return;
            if (c is TextBoxBase or ComboBox or CheckBox or DataGridView or TreeView or CheckedListBox) { userSnapshots.Add(new WorkspaceChangeSnapshot(c)); return; }
            foreach (Control child in c.Controls) Visit(child);
        }
        foreach (Control c in Controls) Visit(c);
        return new WorkspaceChangeSnapshot(this);
    }
    public bool ConfirmLeave()
    {
        if (IsBusy) { lblRoleState.Text = "انتظر اكتمال حفظ الأدوار."; return false; }
        if (!dgvRoleCatalog.EndEdit()) return false;
        return !HasUnsavedChanges || MessageBox.Show(this, "توجد تعديلات غير محفوظة. إغلاق الشاشة وفقدانها؟", "إغلاق", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
    }
    public void BindRoleSave(Func<IReadOnlyList<RoleDraft>, Task<RoleSaveResult>>? handler) { if (IsBusy) throw new InvalidOperationException("Save in progress."); saveRoles = handler; btnRoleSave.Enabled = handler is not null; }
    private async Task SaveRoleDraftAsync()
    {
        if (IsBusy || saveRoles is null) return;
        string? error = ValidateRoles(); if (error is not null) { lblRoleState.Text = error; return; }
        var request = ExportRoleDrafts(); IsBusy = true; Enabled = false;
        try
        {
            var result = await saveRoles(request);
            if (result.Persisted && result.Catalog is not null)
            {
                LoadRoleCatalogCore(result.Catalog, true);
                lblRoleState.Text = result.Message;
            }
            else if (result.Persisted && request.Any(r => string.IsNullOrWhiteSpace(r.Id)))
                lblRoleState.Text = "أبلغت الخدمة بالحفظ دون معرفات الأدوار الجديدة؛ بقيت المسودة معلقة حتى إعادة تحميل المعرفات.";
            else
            {
                if (result.Persisted) roleSnapshot = new WorkspaceChangeSnapshot(dgvRoleCatalog);
                lblRoleState.Text = result.Message;
            }
        }
        catch (Exception) { lblRoleState.Text = "تعذر حفظ الأدوار؛ احتفظت المسودة بتعديلاتك."; }
        finally { IsBusy = false; Enabled = true; }
    }
    private void RefreshRoleSelector()
    {
        string? id = (RoleId.SelectedItem as RoleChoice)?.Id;
        RoleId.Items.Clear();
        foreach (DataGridViewRow r in dgvRoleCatalog.Rows)
        {
            string? key = Convert.ToString(r.Cells["RoleId"].Value);
            if (!string.IsNullOrEmpty(key)) RoleId.Items.Add(new RoleChoice(key, Convert.ToString(r.Cells["NameAr"].Value) ?? key));
        }
        RoleId.DisplayMember = "Label"; RoleId.ValueMember = "Id";
        RoleId.SelectedItem = RoleId.Items.Cast<RoleChoice>().FirstOrDefault(c => c.Id == id);
    }
    private bool loadingRoles;
    private WorkspaceChangeSnapshot roleSnapshot = null!;

    private void WireRoleCatalog()
    {
        roleSnapshot = new WorkspaceChangeSnapshot(dgvRoleCatalog);
        btnRoleClear.Click += (_, _) =>
        {
            if (!dgvRoleCatalog.EndEdit()) return;
            if ((roleSnapshot.HasChanges || dgvRoleCatalog.Rows.Count > 0) && MessageBox.Show(this, "تفريغ مسودة الأدوار المحلية؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            loadingRoles = true;
            try { foreach (DataGridViewRow row in dgvRoleCatalog.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["IsSystem"].Value is not true).ToArray()) dgvRoleCatalog.Rows.Remove(row); RefreshRoleSelector(); lblRoleState.Text = "تم تفريغ المسودة محليًا؛ لم تحذف أدوار من النظام."; }
            finally { loadingRoles = false; }
        };
        btnRoleAdd.Click += (_, _) => { int index = dgvRoleCatalog.Rows.Add(null!, "", "", "", "", null!, null!, false); dgvRoleCatalog.CurrentCell = dgvRoleCatalog.Rows[index].Cells["Code"]; };
        btnRoleRemove.Click += (_, _) =>
        {
            if (dgvRoleCatalog.CurrentRow is not DataGridViewRow row) return;
            if (row.Cells["IsSystem"].Value is true) { lblRoleState.Text = "دور النظام للقراءة فقط."; return; }
            if (MessageBox.Show(this, "إزالة الدور من المسودة؟ يلزم الحفظ لتطبيق التغيير.", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes) dgvRoleCatalog.Rows.Remove(row);
        };
        btnRoleValidate.Click += (_, _) => lblRoleState.Text = ValidateRoles() ?? "اجتاز التحقق المحلي؛ لم يتم الحفظ أو تطبيق الصلاحيات.";
        btnRoleSave.Click += async (_, _) => await SaveRoleDraftAsync();
        dgvRoleCatalog.CurrentCellDirtyStateChanged += (_, _) => { if (dgvRoleCatalog.IsCurrentCellDirty) dgvRoleCatalog.CommitEdit(DataGridViewDataErrorContexts.Commit); };
        dgvRoleCatalog.CellValueChanged += (_, _) => { if (!loadingRoles) { RefreshRoleSelector(); lblRoleState.Text = "تعديلات أدوار محلية غير محفوظة."; } };
        dgvRoleCatalog.RowsRemoved += (_, _) => { if (!loadingRoles) { RefreshRoleSelector(); lblRoleState.Text = "تعديلات أدوار محلية غير محفوظة."; } };
        dgvRoleCatalog.DataError += (_, e) => { e.ThrowException = false; e.Cancel = true; lblRoleState.Text = "قيمة غير صالحة؛ اختر من القائمة المحملة."; };
        RoleId.SelectedIndexChanged += (_, _) =>
        {
            if (RoleId.SelectedItem is not RoleChoice choice) return;
            var row = dgvRoleCatalog.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => Convert.ToString(r.Cells["RoleId"].Value) == choice.Id);
            if (row is not null) { dgvRoleCatalog.CurrentCell = row.Cells["Code"]; row.Selected = true; }
        };
    }
    public string? ValidateRoles()
    {
        if (!dgvRoleCatalog.EndEdit()) return "أكمل تحرير الخلية الحالية.";
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (DataGridViewRow row in dgvRoleCatalog.Rows)
        {
            string code = Convert.ToString(row.Cells["Code"].Value)?.Trim() ?? "";
            if (code.Length == 0 || string.IsNullOrWhiteSpace(Convert.ToString(row.Cells["NameAr"].Value)) || string.IsNullOrWhiteSpace(Convert.ToString(row.Cells["Status"].Value))) return $"السطر {row.Index + 1}: الرمز والاسم العربي والحالة مطلوبة.";
            if (!seen.Add((Convert.ToString(row.Cells["CompanyId"].Value) ?? "") + "|" + code)) return "رمز الدور مكرر داخل الشركة.";
        }
        return null;
    }
    public IReadOnlyList<RoleDraft> ExportRoleDrafts()
    {
        string? error = ValidateRoles(); if (error is not null) throw new InvalidOperationException(error);
        return dgvRoleCatalog.Rows.Cast<DataGridViewRow>().Select(r => new RoleDraft(
            Convert.ToString(r.Cells["RoleId"].Value), Convert.ToString(r.Cells["Code"].Value)!.Trim(), Convert.ToString(r.Cells["NameAr"].Value)!.Trim(),
            Convert.ToString(r.Cells["NameEn"].Value), Convert.ToString(r.Cells["Description"].Value), Convert.ToString(r.Cells["CompanyId"].Value), Convert.ToString(r.Cells["Status"].Value)!, r.Cells["IsSystem"].Value is true)).ToArray();
    }
    public void SetRoleChoices(string key, IEnumerable<RoleChoice> choices)
    {
        if (IsBusy) throw new InvalidOperationException("Save in progress.");
        var values = choices.ToArray();
        if (values.Any(c => string.IsNullOrWhiteSpace(c.Id)) || values.Select(c => c.Id).Distinct().Count() != values.Length) throw new ArgumentException("Choice IDs must be nonempty and unique.");
        if (key != "CompanyId" && key != "Status") throw new ArgumentException("Unknown role lookup.");
        var column = (DataGridViewComboBoxColumn)dgvRoleCatalog.Columns[key]!;
        if (dgvRoleCatalog.Rows.Cast<DataGridViewRow>().Any(r => !string.IsNullOrEmpty(Convert.ToString(r.Cells[key].Value)) && !values.Any(v => v.Id == Convert.ToString(r.Cells[key].Value)))) throw new InvalidOperationException("Cannot remove an in-use choice.");
        column.DataSource = values;
    }
    public bool LoadRoleCatalog(IEnumerable<RoleDraft> roles) => LoadRoleCatalogCore(roles, false);
    private bool LoadRoleCatalogCore(IEnumerable<RoleDraft> roles, bool savedResponse)
    {
        if (IsBusy && !savedResponse) return false;
        var values = roles.ToArray();
        if (values.Any(r => string.IsNullOrWhiteSpace(r.Id)) || values.Select(r => r.Id).Distinct().Count() != values.Length) throw new ArgumentException("Role IDs must be unique and nonempty.");
        if (values.Any(r => string.IsNullOrWhiteSpace(r.Code) || string.IsNullOrWhiteSpace(r.NameAr) || string.IsNullOrWhiteSpace(r.Status))) throw new ArgumentException("Role code, Arabic name and status are required.");
        if (values.Select(r => (r.CompanyId ?? "") + "|" + r.Code.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).Count() != values.Length) throw new ArgumentException("Duplicate role code in company.");
        foreach (var role in values)
            foreach (var item in new[] { ("CompanyId", role.CompanyId), ("Status", role.Status) })
                if (!string.IsNullOrEmpty(item.Item2) && !((DataGridViewComboBoxColumn)dgvRoleCatalog.Columns[item.Item1]!).Items.Cast<RoleChoice>().Any(c => c.Id == item.Item2)) throw new ArgumentException("Load referenced choices before roles.");
        if (!dgvRoleCatalog.EndEdit()) return false;
        if (!savedResponse && roleSnapshot.HasChanges && MessageBox.Show(this, "استبدال مسودة الأدوار بالبيانات المحملة؟", "تحميل الأدوار", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return false;
        loadingRoles = true;
        try
        {
            dgvRoleCatalog.Rows.Clear(); RoleId.Items.Clear();
            foreach (var r in values)
            {
                int i = dgvRoleCatalog.Rows.Add(r.Id!, r.Code, r.NameAr, r.NameEn!, r.Description!, r.CompanyId!, r.Status, r.IsSystem);
                dgvRoleCatalog.Rows[i].ReadOnly = r.IsSystem;
                RoleId.Items.Add(new RoleChoice(r.Id!, r.NameAr));
            }
            RoleId.DisplayMember = "Label"; RoleId.ValueMember = "Id";
            roleSnapshot = new WorkspaceChangeSnapshot(dgvRoleCatalog);
            lblRoleState.Text = "تم تحميل الأدوار؛ التعديلات التالية مسودة حتى نجاح خدمة الحفظ.";
        }
        finally { loadingRoles = false; }
        return true;
    }

    private void InitializePermissionTree()
    {
        var permissionGroups = new (string Group, string[] Modules)[]
        {
            ("تهيئة النظام", new[] { "الشركات", "الفروع", "السنوات والفترات المالية", "العملات", "مراكز التكلفة" }),
            ("الحسابات والأستاذ العام", new[] { "دليل الحسابات", "القيود اليومية", "سندات القبض", "سندات الصرف", "الأستاذ العام", "إقفال الفترة المالية" }),
            ("إدارة الشحنات", new[] { "طلبات الشحن", "تمديد الشحن", "متابعة الشحنات" }),
            ("العملاء", new[] { "العملاء" }),
            ("الأسطول", new[] { "الأسطول" }),
            ("التقارير", new[] { "التقارير" }),
            ("الإعدادات", new[] { "المستخدمون والصلاحيات", "إعدادات عامة", "ترقيم المستندات", "إعدادات الطباعة" }),
        };

        tvPermissions.BeginUpdate();
        try
        {
            tvPermissions.Nodes.Clear();
            dgvModulePermissions.Rows.Clear();

            foreach (var (group, modules) in permissionGroups)
            {
                var groupNode = new TreeNode(group);
                foreach (var module in modules)
                {
                    groupNode.Nodes.Add(new TreeNode(module));
                    dgvModulePermissions.Rows.Add(module, false, false, false, false);
                }

                tvPermissions.Nodes.Add(groupNode);
            }

            tvPermissions.ExpandAll();
        }
        finally
        {
            tvPermissions.EndUpdate();
        }
    }

    private void WireUiEvents()
    {
        tvPermissions.AfterCheck += tvPermissions_AfterCheck;
        chkSelectAllPermissions.CheckedChanged += chkSelectAllPermissions_CheckedChanged;
        dgvModulePermissions.CurrentCellDirtyStateChanged += ModulePermissions_CurrentCellDirtyStateChanged;
        dgvModulePermissions.CellValueChanged += ModulePermissions_CellValueChanged;
        userCommandBindings.SetBinding(UsersToolbarCommand.NewUser, BeginNewUser, () => !IsDisposed && !Disposing);
        userCommandBindings.SetBinding(UsersToolbarCommand.CloseScreen, () => CloseRequested?.Invoke(this, EventArgs.Empty), () => !IsDisposed && !Disposing);
        button1.Click += (_, _) => userCommandBindings.TryInvoke(UsersToolbarCommand.NewUser);
        btnClose.Click += (_, _) => userCommandBindings.TryInvoke(UsersToolbarCommand.CloseScreen);
    }

    private void DisableUnimplementedPolicyActions()
    {
        const string message = "غير منفذ: لم يتم ربط هذا الإجراء بخدمة الحفظ أو المعالجة بعد.";
        components ??= new System.ComponentModel.Container();
        var tips = new ToolTip(components) { ShowAlways = true };
        foreach (Button button in pnlToolbar.Controls.OfType<Button>().Where(b => b != button1 && b != btnClose))
        {
            button.Enabled = false;
            button.AccessibleDescription = message;
            tips.SetToolTip(button, message);
        }
        // Disabled buttons do not always receive hover messages; expose the same
        // explanation on their host without enabling an unimplemented command.
        pnlToolbar.AccessibleDescription = message;
        tips.SetToolTip(pnlToolbar, message);
    }

    private void ModulePermissions_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (!updatingPermissionChecks && dgvModulePermissions.IsCurrentCellDirty
            && dgvModulePermissions.CurrentCell?.ColumnIndex == colView.Index)
            dgvModulePermissions.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void ModulePermissions_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (updatingPermissionChecks || e.RowIndex < 0 || e.ColumnIndex != colView.Index) return;
        var row = dgvModulePermissions.Rows[e.RowIndex];
        string module = Convert.ToString(row.Cells[colModule.Index].Value) ?? string.Empty;
        TreeNode? node = FindPermissionLeaf(tvPermissions.Nodes, module);
        if (node is null) return;
        updatingPermissionChecks = true;
        try
        {
            node.Checked = row.Cells[colView.Index].Value is true;
            RefreshPermissionParents();
            UpdateSelectAllState();
        }
        finally { updatingPermissionChecks = false; }
    }

    private static TreeNode? FindPermissionLeaf(TreeNodeCollection nodes, string module)
    {
        foreach (TreeNode node in nodes)
        {
            if (node.Nodes.Count == 0 && string.Equals(node.Text, module, StringComparison.Ordinal)) return node;
            TreeNode? child = FindPermissionLeaf(node.Nodes, module);
            if (child is not null) return child;
        }
        return null;
    }

    private void RefreshPermissionParents()
    {
        foreach (TreeNode node in tvPermissions.Nodes) RefreshPermissionParent(node);
    }

    private static bool RefreshPermissionParent(TreeNode node)
    {
        if (node.Nodes.Count == 0) return node.Checked;
        bool all = true;
        foreach (TreeNode child in node.Nodes) all &= RefreshPermissionParent(child);
        node.Checked = all;
        return all;
    }

    private void tvPermissions_AfterCheck(object? sender, TreeViewEventArgs e)
    {
        if (updatingPermissionChecks || e.Node is null)
        {
            return;
        }

        updatingPermissionChecks = true;
        try
        {
            SetChildNodesChecked(e.Node, e.Node.Checked);
            UpdateModuleViewPermission(e.Node);
            RefreshPermissionParents();
            UpdateSelectAllState();
        }
        finally
        {
            updatingPermissionChecks = false;
        }
    }

    private void chkSelectAllPermissions_CheckedChanged(object? sender, EventArgs e)
    {
        if (updatingPermissionChecks)
        {
            return;
        }

        updatingPermissionChecks = true;
        try
        {
            foreach (TreeNode rootNode in tvPermissions.Nodes)
            {
                rootNode.Checked = chkSelectAllPermissions.Checked;
                SetChildNodesChecked(rootNode, chkSelectAllPermissions.Checked);
            }

            foreach (DataGridViewRow row in dgvModulePermissions.Rows)
            {
                row.Cells[colView.Index].Value = chkSelectAllPermissions.Checked;
            }
        }
        finally
        {
            updatingPermissionChecks = false;
        }
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F6 && userCommandBindings.TryInvoke(UsersToolbarCommand.NewUser))
        {
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void BeginNewUser()
    {
        if (newUserSnapshot.HasChanges && MessageBox.Show(this,
            "توجد تغييرات في الشاشة. هل تريد تجاهلها وبدء مستخدم جديد؟",
            "بدء مستخدم جديد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        {
            return;
        }

        // Restore the existing editor's empty defaults; keep lookup catalogs and filters.
        foreach (var textBox in new[]
        {
            txtUserName, txtFullName, txtForeignName, txtOnyxUserNumber,
            txtOnyxEmployeeNumber, txtOnyxEmployeeName, txtOnyxManagerCode, txtOnyxManager,
            txtAccessStartDate, txtAccessEndDate, txtAccessFromTime, txtAccessToTime,
            txtOnyxProfession, txtOnyxUserComputerName, txtOnyxUserStopReason,
            txtPosNumber, txtPosName, txtBarcodePrinter, txtBarcodePath
        })
        {
            textBox.Clear();
        }

        foreach (var comboBox in new[]
        {
            cboRole, cboBranch, cboStatus, cboOnyxGroupNumber,
            cboOnyxBranchNumber, cboPosConnection, DeviceId, UserId
        })
        {
            comboBox.SelectedIndex = -1;
            comboBox.Text = string.Empty;
        }

        chkRequirePasswordReset.Checked = false;
        chkOnyxUserStopped.Checked = false;
        checkBox1.Checked = false;
        dgvOnyxHandheldDevices.CancelEdit();
        if (dgvOnyxHandheldDevices.DataSource is null)
        {
            dgvOnyxHandheldDevices.Rows.Clear();
        }

        // A partially checked tree does not trigger CheckedChanged on an already-false select-all.
        updatingPermissionChecks = true;
        try
        {
            foreach (TreeNode rootNode in tvPermissions.Nodes)
            {
                rootNode.Checked = false;
                SetChildNodesChecked(rootNode, false);
            }

            chkSelectAllPermissions.Checked = false;
            dgvModulePermissions.CancelEdit();
            foreach (DataGridViewRow row in dgvModulePermissions.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                for (var columnIndex = 1; columnIndex < row.Cells.Count; columnIndex++)
                {
                    row.Cells[columnIndex].Value = false;
                }
            }
        }
        finally
        {
            updatingPermissionChecks = false;
        }

        txtUserName.Focus();
        newUserSnapshot = ResetUserSnapshot();
    }

    private static void SetChildNodesChecked(TreeNode node, bool isChecked)
    {
        foreach (TreeNode childNode in node.Nodes)
        {
            childNode.Checked = isChecked;
            SetChildNodesChecked(childNode, isChecked);
        }
    }

    private void UpdateModuleViewPermission(TreeNode node)
    {
        if (node.Nodes.Count > 0)
        {
            foreach (TreeNode childNode in node.Nodes)
            {
                UpdateModuleViewPermission(childNode);
            }

            return;
        }

        foreach (DataGridViewRow row in dgvModulePermissions.Rows)
        {
            if (string.Equals(Convert.ToString(row.Cells[colModule.Index].Value), node.Text, StringComparison.Ordinal))
            {
                row.Cells[colView.Index].Value = node.Checked;
                break;
            }
        }
    }

    private void UpdateSelectAllState()
    {
        var allChecked = tvPermissions.Nodes.Count > 0;
        foreach (TreeNode rootNode in tvPermissions.Nodes)
        {
            allChecked &= rootNode.Checked && AreAllChildNodesChecked(rootNode);
        }

        chkSelectAllPermissions.Checked = allChecked;
    }

    private static bool AreAllChildNodesChecked(TreeNode node)
    {
        foreach (TreeNode childNode in node.Nodes)
        {
            if (!childNode.Checked || !AreAllChildNodesChecked(childNode))
            {
                return false;
            }
        }

        return true;
    }

}
