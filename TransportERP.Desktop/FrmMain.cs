using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TransportERP.Desktop.CoreUI;
using TransportERP.Desktop.Forms.Setup.Security;
using TransportERP.Desktop.Forms.SystemSettings.General;
using TransportERP.Desktop.Forms.SystemSettings.DocumentControl;
using TransportERP.Desktop.البوالص_والشحن.الاعدادات;
using TransportERP.Desktop.البوالص_والشحن.المدخلات;
using TransportERP.Desktop.البوالص_والشحن.العمليات.البوليصه;
using TransportERP.Desktop.البوالص_والشحن.العمليات.الشحن_والترحيل;

namespace TransportERP.Desktop
{
    public partial class FrmMain : FrmBase
    {
        private readonly System.Windows.Forms.Timer uiClockTimer = new() { Interval = 1000 };
        private bool isNavigationCollapsed;
        private TreeNode? navigationTopNode;

        public FrmMain()
        {
            InitializeComponent();
            InitializePhaseOneNavigation();
            tvSystemTree.NodeMouseDoubleClick += tvSystemTree_NodeMouseDoubleClick;
            tvSystemTree.KeyDown += tvSystemTree_KeyDown;
            txtTreeSearch.TextChanged += (_, _) => InitializePhaseOneNavigation();
            railArrow.Click += (_, _) => ToggleNavigationPane();
            SetNavigationStatus(null);
            uiClockTimer.Tick += (_, _) => UpdateClock();
            UpdateClock();
            uiClockTimer.Start();
            FormClosed += (_, _) => uiClockTimer.Dispose();
        }

        private void OpenWorkspaceControl(UserControl control)
        {
            workspaceSurface.SuspendLayout();

            try
            {
                while (workspaceSurface.Controls.Count > 0)
                {
                    Control oldControl = workspaceSurface.Controls[0];
                    workspaceSurface.Controls.RemoveAt(0);
                    oldControl.Dispose();
                }

                control.Dock = DockStyle.Fill;
                workspaceSurface.Controls.Add(control);
                control.BringToFront();
            }
            finally
            {
                workspaceSurface.ResumeLayout(true);
            }
        }

        private void AddExchangeRatesNavigation()
        {
            foreach (TreeNode root in tvSystemTree.Nodes)
            {
                foreach (TreeNode child in root.Nodes)
                {
                    if (child.Text != "العملات") continue;
                    if (!root.Nodes.ContainsKey("02.04.03"))
                        root.Nodes.Insert(child.Index + 1, new TreeNode("أسعار الصرف") { Name = "02.04.03" });
                    return;
                }
            }
        }

        private void AddSecurityNavigation()
        {
            foreach (TreeNode root in tvSystemTree.Nodes)
            {
                foreach (TreeNode child in root.Nodes)
                {
                    if (child.Text == "المستخدمون والصلاحيات")
                    {
                        if (!child.Nodes.ContainsKey("02.03.02")) child.Nodes.Add(new TreeNode("الأدوار") { Name = "02.03.02" });
                        if (!child.Nodes.ContainsKey("02.03.03")) child.Nodes.Add(new TreeNode("الصلاحيات") { Name = "02.03.03" });
                        if (!child.Nodes.ContainsKey("02.03.06")) child.Nodes.Add(new TreeNode("تغيير كلمة السر") { Name = "02.03.06" });
                        return;
                    }
                }
            }
        }
        private void UpdateClock()
        {
            var now = DateTime.Now;
            lblCurrentUser.Text = "المستخدم: —";
            lblCurrentDate.Text = $"التاريخ: {now:yyyy/MM/dd}";
            lblCurrentTime.Text = $"الوقت: {now:hh:mm:ss tt}";
        }

        private void tvSystemTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetNavigationStatus(e.Node);
        }

        private void ToggleNavigationPane()
        {
            mainLayout.SuspendLayout();
            try
            {
                if (!isNavigationCollapsed) navigationTopNode = tvSystemTree.TopNode;
                isNavigationCollapsed = !isNavigationCollapsed;
                treeBody.Visible = !isNavigationCollapsed;
                mainLayout.ColumnStyles[2].Width = isNavigationCollapsed ? navigationRail.Width : 280F;
                railArrow.Text = isNavigationCollapsed ? "←" : "➜";
                railArrow.AccessibleName = isNavigationCollapsed ? "إظهار شجرة النظام" : "إخفاء شجرة النظام";
                railRefresh.Visible = railStar.Visible = railPlay.Visible = railFlag.Visible = railPower.Visible = !isNavigationCollapsed;
            }
            finally { mainLayout.ResumeLayout(true); }
            if (!isNavigationCollapsed && navigationTopNode?.TreeView == tvSystemTree)
                tvSystemTree.TopNode = navigationTopNode;
        }

        private void SetNavigationStatus(TreeNode? node, string? status = null)
        {
            var labels = pnlMainContent.Controls.Find("lblOpenScreens", true);
            if (labels.Length == 0) return;
            var instruction = "لفتح الشاشة: انقر نقراً مزدوجاً أو اضغط Enter";
            labels[0].Text = node?.Tag is string
                ? $"{node.Text} — {status ?? instruction}"
                : instruction;
        }

        private void tvSystemTree_NodeMouseDoubleClick(object? sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left) ActivateNavigationNode(e.Node);
        }

        private void tvSystemTree_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None) return;
            e.Handled = true;
            e.SuppressKeyPress = true;
            ActivateNavigationNode(tvSystemTree.SelectedNode);
        }

        private void ActivateNavigationNode(TreeNode? node)
        {
            if (node?.Tag is not string code) return;
            if (code == "SHIP:001")
            {
                OpenWorkspaceControl(new UcShipmentTypes());
                SetNavigationStatus(node, "مفتوحة");
                return;
            }

            if (code == "SHIP:002")
            {
                OpenWorkspaceControl(new UcShipmentCategories());
                SetNavigationStatus(node, "مفتوحة");
                return;
            }
            if (code == "SHIP:003")
            {
                OpenWorkspaceControl(new UcShipmentUnits());
                SetNavigationStatus(node, "مفتوحة");
                return;
            }

            if (code == "SHIP:004")
            {
                OpenWorkspaceControl(new dgvShipmentPriorities());
                SetNavigationStatus(node, "مفتوحة");
                return;
            }

            if (code == "SHIP:005")
            {
                OpenWorkspaceControl(new UcTransportTypes());
                SetNavigationStatus(node, "مفتوحة");
                return;
            }

            if (code == "SHIP:006")
            {
                OpenWorkspaceControl(new UcTransportGroups());
                SetNavigationStatus(node, "مفتوحة");
                return;
            }

            if (code == "SHIP:007")
            {
                OpenWorkspaceControl(new UcShipmentTransportMethods());
                SetNavigationStatus(node, "مفتوحة");
                return;
            }
            if (code == "SHIP:008")
            {
                OpenWorkspaceControl(new UcRoutes());
                SetNavigationStatus(node, "مفتوحة");
                return;
            }
            if (code == "SHIP:009")
            {
                OpenWorkspaceControl(new UcShipmentPricingRules());
                SetNavigationStatus(node, "مفتوحة");
                return;
            }
            if (code == "SHIP:010")
            {
                OpenWorkspaceControl(new UcShipmentStatuses());
                SetNavigationStatus(node, "مفتوحة");
                return;
            }
            if (code == "SHIP:011")
            {
                OpenWorkspaceControl(new UcShipmentBooks());
                SetNavigationStatus(node, "مفتوحة");
                return;
            }
            if (code == "SHIP:012")
            {
                // Prefer UcShipmentWaybill2 if available
                try
                {
                    OpenWorkspaceControl(new UcShipmentWaybill());
                }
                catch
                {
                    OpenWorkspaceControl(new UcShipmentWaybill());
                }
                SetNavigationStatus(node, "مفتوحة");
                return;
            }
            if (code == "SHIP:013")
            {
                OpenWorkspaceControl(new UcShipmentDispatch());
                SetNavigationStatus(node, "مفتوحة");
                return;
            }
            if (code == "01.02") { Activate(); SetNavigationStatus(node); return; }
            using Form? screen = CreatePhaseOneScreen(code);

            if (screen is null ||
                (screen.GetType().Namespace == "TransportERP.EmptyForms" && screen.Controls.Count == 0))
            {
                SetNavigationStatus(node, "غير مجهزة");
                return;
            }

            try
            {
                SetNavigationStatus(node, "مفتوحة");
                screen.ShowDialog(this);
            }
            finally
            {
                // Activation is independent of selection, so the same item can reopen.
                SetNavigationStatus(node);
            }
        }

        private void workspaceSurface_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }

        private void railPower_Click(object sender, EventArgs e)
        {

            Application.Exit();
        }
    }
}
