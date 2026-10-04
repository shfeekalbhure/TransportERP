extern alias SharedCommands;
using TransportERP.Desktop.CoreUI;
using SharedCommands::TransportERP.Desktop.SharedUI.Commands;

namespace TransportERP.Desktop.Forms.Setup.Security;

public partial class FrmUsersPermissions : FrmBase
{
    private bool updatingPermissionChecks;
    private readonly ToolbarCommandBindings<UsersToolbarCommand> userCommandBindings = new();

    public FrmUsersPermissions()
    {
        InitializeComponent();
        // The isolated review form takes its appearance from the Visual Studio Designer.
        InitializePermissionTree();
        dgvModulePermissions.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        WireUiEvents();
        InitializeSourceAdditions();
    }

    public FrmUsersPermissions(string initialTab) : this()
    {
        var page = tabUserDetails.TabPages.Cast<TabPage>().Single(p => p.Name == initialTab);
        tabUserDetails.SelectedTab = page;
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
        userCommandBindings.SetBinding(UsersToolbarCommand.NewUser, BeginNewUser, () => !IsDisposed && !Disposing);
        userCommandBindings.SetBinding(UsersToolbarCommand.CloseScreen, Close, () => !IsDisposed && !Disposing);
        usersCommandBar1.SetCommandBindings(userCommandBindings);
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
        if (keyData == Keys.F6 && usersCommandBar1.TryInvokeCommand(UsersToolbarCommand.NewUser))
        {
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void BeginNewUser()
    {
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






