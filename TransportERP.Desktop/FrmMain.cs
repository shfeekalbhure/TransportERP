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
        private readonly Dictionary<TabPage, WorkspaceChangeSnapshot> workspaceChanges = new();
        private bool isNavigationCollapsed;
        private bool isNotificationsCollapsed;
        private float notificationsExpandedWidth = 200F;
        private Point notificationsScrollPosition;
        private TreeNode? navigationTopNode;
        private float navigationExpandedWidth;
        private bool workspaceTabVisibilityPending;

        public FrmMain()
        {
            InitializeComponent();
            navigationExpandedWidth = mainLayout.ColumnStyles[2].Width;
            InitializePhaseOneNavigation();
            Shown += (_, _) => UpdateMainMeasurements();
            FontChanged += (_, _) => UpdateMainMeasurements();
            DpiChanged += (_, e) =>
            {
                float ratio = (float)e.DeviceDpiNew / e.DeviceDpiOld;
                navigationExpandedWidth *= ratio;
                notificationsExpandedWidth *= ratio;
                if (IsHandleCreated) BeginInvoke(new Action(UpdateMainMeasurements));
            };
            UpdateMainMeasurements();
            tvSystemTree.NodeMouseDoubleClick += tvSystemTree_NodeMouseDoubleClick;
            tvSystemTree.KeyDown += tvSystemTree_KeyDown;
            txtTreeSearch.TextChanged += (_, _) => InitializePhaseOneNavigation();
            railArrow.Click += (_, _) => ToggleNavigationPane();
            SetNavigationStatus(null);
            tabOpenScreens.SelectedIndexChanged += (_, _) => { UpdateWorkspaceStatus(); tabOpenScreens.Invalidate(); };
            tabOpenScreens.ControlAdded += (_, _) => UpdateMainMeasurements();
            tabOpenScreens.ControlRemoved += (_, _) => UpdateMainMeasurements();
            tabOpenScreens.SizeChanged += (_, _) => UpdateWorkspaceTabMeasurements();
            UpdateWorkspaceStatus();
            uiClockTimer.Tick += (_, _) => UpdateClock();
            UpdateClock();
            uiClockTimer.Start();
            FormClosed += (_, _) => uiClockTimer.Dispose();
        
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        // An empty navigation filter restores the full tree; it is not a required input.
        global::TransportERP.RequiredFieldAppearance.Apply(txtTreeSearch, false);
    }

        // One tab per registry identity; existing controls and their values stay alive.
        private void OpenWorkspaceControl(string code, string caption, Func<UserControl?> create)
        {
            code = NormalizeScreenCode(code);
            foreach (var screen in PhaseOneScreens)
                if (screen.Code == code) { caption = screen.Title; break; }
            string? section = code switch
            {
                "02.03.01" => "tabUserData",
                "02.03.02" => "tabRolesV20",
                "02.03.03" => "tabPermissions",
                _ => null
            };
            string workspaceKey = section is null ? code : "02.03.01";
            if (tabOpenScreens.TabPages.ContainsKey(workspaceKey))
            {
                var existingPage = tabOpenScreens.TabPages[workspaceKey]!;
                tabOpenScreens.SelectedTab = existingPage;
                if (section is not null)
                    foreach (Control existingControl in existingPage.Controls)
                        if (existingControl is UcUsersPermissions users)
                            users.SelectSection(section);
                return;
            }
            if (section is not null) caption = "المستخدمون والأدوار والصلاحيات";

            UserControl? control = create();
            if (control is null) return;
            BindAuthenticatedBranchContext(control);
            if (control.GetType().Namespace == "TransportERP.EmptyForms" && control.Controls.Count == 0)
            {
                control.Dispose();
                SetNavigationStatus(tvSystemTree.SelectedNode, "غير مجهزة — قالب فارغ");
                return;
            }

            var page = new TabPage(caption)
            {
                Name = workspaceKey,
                Tag = workspaceKey,
                ToolTipText = caption,
                AutoScroll = false,
                Padding = new Padding(3)
            };
            // Dock only when hosted; docking the designer root resizes its design surface.
            if (control is TransportERP.EmptyForms.UcScreen_04_05_01)
                control.Dock = DockStyle.Fill;
            else
                SharedScreenProperties.ConfigureWorkspace(control);
            page.Controls.Add(control);
            EventHandler closeRequested = (_, _) => CloseWorkspacePage(page);
            switch (control)
            {
                case TransportERP.EmptyForms.UcForeignPurchaseReceipt foreignReceipt: foreignReceipt.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcPurchaseRequest purchaseRequest: purchaseRequest.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcPurchaseTechnicalComparison technicalComparison: technicalComparison.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcPurchaseComparisonNomination comparisonNomination: comparisonNomination.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcPurchaseAdditionalDiscount purchaseDiscount: purchaseDiscount.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcRepresentativeCommissions commissions: commissions.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcCustomerInstallmentSettlement installments: installments.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcCustomerDebtReport debtReport: debtReport.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcForeignPurchaseCosting costing: costing.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcOnyxSCREEN0110 currencyRequest: currencyRequest.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcOnyxSCREEN0111 currencyExchange: currencyExchange.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcInventoryManualStocktake stocktake: stocktake.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcInventoryTransfer transfer: transfer.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcInventoryMaterialRequest materialRequest: materialRequest.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcInventoryTransferReceipt transferReceipt: transferReceipt.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcInventorySettlement inventorySettlement: inventorySettlement.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcInventoryStocktakeReport stocktakeReport: stocktakeReport.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcInventoryReceiptAuthorization receiptAuthorization: receiptAuthorization.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcInventoryReceiptOrder receiptOrder: receiptOrder.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcInventoryIssueOrder inventoryIssue: inventoryIssue.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcInventoryCustodyReceipt custodyReceipt: custodyReceipt.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcInventoryCustodyIssue custodyIssue: custodyIssue.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcInventoryExternalRepairOrder repairScreen: repairScreen.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcInventoryDamagedIssueOrder damagedScreen: damagedScreen.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcInventoryQuantityReservation reservationScreen: reservationScreen.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcOnyxSCREEN0102 depositScreen: depositScreen.CloseRequested += closeRequested; break;
                case UcCompanyManagement screen0: screen0.CloseRequested += closeRequested; break;
                case TransportERP.Desktop.Forms.SystemSettings.General.الفروع.UcBranchManagement screen1: screen1.CloseRequested += closeRequested; break;
                case UcCurrencyManagement screen2: screen2.CloseRequested += closeRequested; break;
                case UcExchangeRates screen3: screen3.CloseRequested += closeRequested; break;
                case UcDocumentNumbering screen4: screen4.CloseRequested += closeRequested; break;
                case UcGeneralSettings screen5: screen5.CloseRequested += closeRequested; break;
                case UcPrintSettings screen6: screen6.CloseRequested += closeRequested; break;
                case UcUsersPermissions screen7: screen7.CloseRequested += closeRequested; break;
                case UcChangePassword screen8: screen8.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcChartOfAccounts screen9: screen9.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcAccountGroupsAndTypes screen10: screen10.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcGeneralLedgerSettings screen11: screen11.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcAccountOpeningRequest screen12: screen12.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcAccountCostCenterLinking screen13: screen13.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcAccountProjectLinking screen14: screen14.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_02_05_21 screen15: screen15.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_02_02_04 screen16: screen16.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_02_02_05 screen17: screen17.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_02_02_06 screen18: screen18.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_02_02_07 screen19: screen19.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_02_02_08 screen20: screen20.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_03_01 screen21: screen21.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_03_02 screen22: screen22.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_07_16 screen23: screen23.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_02_01 screen24: screen24.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_03_03 screen25: screen25.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_03_04 screen26: screen26.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_11_01 screen27: screen27.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcOnyxSCREEN0079 screen28: screen28.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcOnyxSCREEN0080 screen29: screen29.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcOnyxSCREEN0081 screen30: screen30.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcOnyxSCREEN0082 screen31: screen31.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcOnyxSCREEN0083 screen32: screen32.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcOnyxSCREEN0084 screen33: screen33.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcOnyxSCREEN0085 screen34: screen34.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcOnyxSCREEN0089 screen35: screen35.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcOnyxSCREEN0090 screen36: screen36.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcOnyxSCREEN0091 screen37: screen37.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_05_01 batch5screen0: batch5screen0.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcOnyxSCREEN0100 batch5screen1: batch5screen1.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_04_02 batch5screen2: batch5screen2.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_04_01 batch5screen3: batch5screen3.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcOnyxSCREEN0103 batch5screen4: batch5screen4.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_11_02 batch5screen5: batch5screen5.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_11_03 batch5screen6: batch5screen6.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_08_01 batch5screen7: batch5screen7.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_08_02 batch5screen8: batch5screen8.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_08_05 batch5screen9: batch5screen9.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_09_06 batch6screen0: batch6screen0.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_10_03 batch6screen1: batch6screen1.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_10_04 batch6screen2: batch6screen2.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_10_05 batch6screen3: batch6screen3.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_04_03 batch6screen4: batch6screen4.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_11_04 batch6screen5: batch6screen5.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_11_05 batch6screen6: batch6screen6.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_04_04 batch6screen7: batch6screen7.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_04_05 batch6screen8: batch6screen8.CloseRequested += closeRequested; break;
                case TransportERP.EmptyForms.UcScreen_04_04_06 batch6screen9: batch6screen9.CloseRequested += closeRequested; break;
                default:
                    foreach (Control candidate in control.Controls.Find("btnClose", true))
                        if (candidate is Button close) close.Click += closeRequested;
                    break;
            }
            tabOpenScreens.TabPages.Add(page);
            tabOpenScreens.SelectedTab = page;
            // Capture after synchronous Load/selection initialization.
            workspaceChanges[page] = new WorkspaceChangeSnapshot(control);
            control.Select();
            UpdateWorkspaceStatus();
        }

        private void BindAuthenticatedBranchContext(UserControl control)
        {
            if (control is not TransportERP.EmptyForms.IAuthenticatedBranchView view ||
                AuthenticatedSession?.Current is not { } session || session.ExpiresAt <= DateTimeOffset.UtcNow)
                return;
            var scope = session.Scope;
            if (scope.BranchId == Guid.Empty || string.IsNullOrWhiteSpace(scope.BranchName)) return;
            view.SetCurrentBranch(scope.BranchId, scope.BranchName);
        }

        private int UiPixels(int logicalPixels)
            => Math.Max(1, (int)Math.Round(logicalPixels * DeviceDpi / 120.0));

        private void UpdateMainMeasurements()
        {
            if (IsDisposed) return;
            leftLayout.RowStyles[1].Height = Math.Max(btnRefreshNotifications.PreferredSize.Height,
                btnRefreshNotifications.Font.Height + UiPixels(12)) + btnRefreshNotifications.Margin.Vertical;
            leftLayout.RowStyles[5].Height = Math.Max(btnContact.PreferredSize.Height,
                btnContact.Font.Height + UiPixels(12)) + btnContact.Margin.Vertical;
            int rowHeight = Font.Height + UiPixels(4);
            foreach (RowStyle row in userInfoLayout.RowStyles)
            {
                row.SizeType = SizeType.Absolute;
                row.Height = rowHeight;
            }
            leftLayout.RowStyles[4].Height = rowHeight * userInfoLayout.RowCount
                + userInfoLayout.Padding.Vertical + lblUserInfoTitle.Height + userBox.Margin.Vertical;
            leftLayout.RowStyles[2].Height = lblNotificationsTitle.Height
                + Font.Height + UiPixels(8) + notificationBox.Margin.Vertical;
            treeSearchLayout.Height = Math.Max(txtTreeSearch.PreferredHeight,
                lblTreeSearch.Font.Height) + treeSearchLayout.Padding.Vertical;
            tvSystemTree.ItemHeight = Math.Max(Font.Height + UiPixels(6), UiPixels(24));
            UpdateWorkspaceTabMeasurements();
        }

        private void UpdateWorkspaceTabMeasurements()
        {
            if (IsDisposed || tabOpenScreens.IsDisposed) return;
            int tabWidth = UiPixels(190);
            // Keep one long title from widening every tab; full titles remain in tooltips.
            int availableWidth = Math.Max(UiPixels(90), tabOpenScreens.ClientSize.Width - UiPixels(28));
            var itemSize = new Size(Math.Min(tabWidth, availableWidth),
                Math.Max(UiPixels(36), tabOpenScreens.Font.Height + UiPixels(12)));
            if (tabOpenScreens.ItemSize != itemSize) tabOpenScreens.ItemSize = itemSize;
            QueueSelectedWorkspaceTabVisibility();
            tabOpenScreens.Invalidate();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SendMessageW")]
        private static extern IntPtr SendWorkspaceTabMessage(IntPtr window, int message, IntPtr wParam, IntPtr lParam);

        private void QueueSelectedWorkspaceTabVisibility()
        {
            if (!tabOpenScreens.IsHandleCreated || workspaceTabVisibilityPending) return;
            workspaceTabVisibilityPending = true;
            tabOpenScreens.BeginInvoke(new Action(() =>
            {
                workspaceTabVisibilityPending = false;
                if (!IsDisposed && !tabOpenScreens.IsDisposed)
                    EnsureSelectedWorkspaceTabVisible();
            }));
        }

        private void EnsureSelectedWorkspaceTabVisible()
        {
            if (!tabOpenScreens.IsHandleCreated || tabOpenScreens.SelectedIndex < 0) return;
            int selected = tabOpenScreens.SelectedIndex;
            if (tabOpenScreens.ClientRectangle.Contains(tabOpenScreens.GetTabRect(selected))) return;
            // A resized native tab strip can retain its old scroll position. Refresh
            // its selection without switching managed pages or firing selection events.
            const int setCurrentSelection = 0x130C;
            SendWorkspaceTabMessage(tabOpenScreens.Handle, setCurrentSelection, new IntPtr(-1), IntPtr.Zero);
            SendWorkspaceTabMessage(tabOpenScreens.Handle, setCurrentSelection, new IntPtr(selected), IntPtr.Zero);
        }

        private bool WorkspaceTabsAreMirrored
            => tabOpenScreens.RightToLeft == RightToLeft.Yes && tabOpenScreens.RightToLeftLayout;

        private Rectangle TabCloseBounds(Rectangle bounds)
        {
            int side = Math.Min(UiPixels(22), bounds.Height);
            return new Rectangle(bounds.Left + UiPixels(6), bounds.Top + (bounds.Height - side) / 2, side, side);
        }

        private Rectangle TabCloseBounds(int index)
        {
            Rectangle close = TabCloseBounds(tabOpenScreens.GetTabRect(index));
            // GetTabRect uses native logical coordinates; MouseEventArgs uses
            // physical client coordinates even when the tab window is mirrored.
            if (WorkspaceTabsAreMirrored)
                close.X = tabOpenScreens.ClientSize.Width - close.Right;
            return close;
        }

        private void WorkspaceTabs_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= tabOpenScreens.TabCount) return;
            bool selected = e.Index == tabOpenScreens.SelectedIndex;
            // Draw every layer in the event's coordinate space. Do not mix
            // GDI text with GDI+ painting on the mirrored native tab DC.
            Rectangle bounds = e.Bounds;
            var state = e.Graphics.Save();
            try
            {
                e.Graphics.SetClip(bounds, System.Drawing.Drawing2D.CombineMode.Intersect);
                using var background = new SolidBrush(selected ? Color.White : Color.FromArgb(237, 241, 246));
                e.Graphics.FillRectangle(background, bounds);
                if (selected)
                {
                    using var accent = new SolidBrush(Color.FromArgb(92, 117, 36));
                    e.Graphics.FillRectangle(accent, bounds.Left, bounds.Top, bounds.Width, UiPixels(3));
                }
                var textBounds = new Rectangle(bounds.Left + UiPixels(34), bounds.Top + UiPixels(3),
                    Math.Max(0, bounds.Width - UiPixels(42)), Math.Max(0, bounds.Height - UiPixels(6)));
                using var format = new StringFormat(StringFormatFlags.DirectionRightToLeft | StringFormatFlags.NoWrap)
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter
                };
                using var textBrush = new SolidBrush(Color.FromArgb(24, 48, 76));
                var textState = e.Graphics.Save();
                try
                {
                    // Counter the native DC mirror for glyphs only. The tab,
                    // close mark and text rectangle retain their RTL positions.
                    if (WorkspaceTabsAreMirrored)
                    {
                        e.Graphics.TranslateTransform(textBounds.Left + textBounds.Right, 0);
                        e.Graphics.ScaleTransform(-1, 1);
                    }
                    e.Graphics.DrawString(tabOpenScreens.TabPages[e.Index].Text, tabOpenScreens.Font,
                        textBrush, textBounds, format);
                }
                finally { e.Graphics.Restore(textState); }
                Rectangle close = TabCloseBounds(bounds);
                close.Inflate(-UiPixels(7), -UiPixels(7));
                using var closePen = new Pen(Color.FromArgb(95, 110, 128), UiPixels(1));
                e.Graphics.DrawLine(closePen, close.Left, close.Top, close.Right, close.Bottom);
                e.Graphics.DrawLine(closePen, close.Left, close.Bottom, close.Right, close.Top);
                if (selected && tabOpenScreens.Focused && textBounds.Width > 2 && textBounds.Height > 2)
                {
                    using var focusPen = new Pen(Color.FromArgb(95, 110, 128))
                        { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot };
                    e.Graphics.DrawRectangle(focusPen, textBounds.Left, textBounds.Top,
                        textBounds.Width - 1, textBounds.Height - 1);
                }
            }
            finally { e.Graphics.Restore(state); }
        }

        private void WorkspaceTabs_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            for (int i = 0; i < tabOpenScreens.TabCount; i++)
            {
                if (!TabCloseBounds(i).Contains(e.Location)) continue;
                CloseWorkspacePage(tabOpenScreens.TabPages[i]);
                break;
            }
        }

        private void CloseWorkspacePage(TabPage? page)
        {
            if (page is null || page.IsDisposed) return;
            var guard = page.Controls.OfType<IWorkspaceCloseGuard>().FirstOrDefault();
            if (guard != null && !guard.ConfirmLeave()) return;
            var batchFive = page.Controls.OfType<TransportERP.EmptyForms.IBatchFiveScreen>().FirstOrDefault();
            if (batchFive != null && !batchFive.Binding.ConfirmLeave()) return;
            if (guard == null && batchFive == null && WorkspacePageHasChanges(page)
                && MessageBox.Show(this, "توجد تغييرات في هذه الشاشة. هل تريد الإغلاق والتخلي عن التغييرات غير المحفوظة؟",
                    "إغلاق الشاشة", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            workspaceChanges.Remove(page);
            tabOpenScreens.TabPages.Remove(page);
            page.Dispose();
            UpdateWorkspaceStatus();
        }

        private bool WorkspacePageHasChanges(TabPage page)
        {
            if (page.IsDisposed) return false;
            foreach (Control content in page.Controls)
                if (content is IWorkspaceChangeState state) return state.HasUnsavedChanges;
            return workspaceChanges.TryGetValue(page, out var snapshot) && snapshot.HasChanges;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.Cancel) return;
            // An expired authenticated session must not remain usable by cancelling this prompt.
            if (AuthenticatedSession?.Current is { } session && DateTimeOffset.UtcNow >= session.ExpiresAt)
                return;
            foreach (TabPage busyPage in tabOpenScreens.TabPages)
                foreach (var screen in busyPage.Controls.OfType<TransportERP.EmptyForms.IBatchFiveScreen>())
                    if (screen.Binding.IsBusy)
                    {
                        MessageBox.Show(this,"انتظر انتهاء العملية الحالية قبل الإغلاق.","عملية قيد التنفيذ");
                        e.Cancel=true; return;
                    }
            foreach (TabPage busyPage in tabOpenScreens.TabPages)
                foreach (var guard in busyPage.Controls.OfType<IWorkspaceCloseGuard>())
                    if (guard.IsBusy)
                    {
                        MessageBox.Show(this, "انتظر انتهاء العملية الحالية قبل الإغلاق.", "عملية قيد التنفيذ");
                        e.Cancel = true; return;
                    }
            int changedCount = 0;
            foreach (TabPage page in tabOpenScreens.TabPages)
                if (WorkspacePageHasChanges(page)) changedCount++;
            if (changedCount > 0)
            {
                e.Cancel = MessageBox.Show(this,
                    $"توجد تغييرات في {changedCount} شاشة مفتوحة. هل تريد إغلاق البرنامج والتخلي عن التغييرات غير المحفوظة؟",
                    "إغلاق البرنامج", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes;
            }
        }

        private void UpdateWorkspaceStatus()
        {
            tabOpenScreens.AccessibleDescription = tabOpenScreens.SelectedTab is { } page
                ? $"الشاشة الحالية: {page.Text} — الشاشات المفتوحة: {tabOpenScreens.TabCount}"
                : "لا توجد شاشات مفتوحة — اختر شاشة من الدليل";
        }

        private void WorkspaceTabs_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F4)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                CloseWorkspacePage(tabOpenScreens.SelectedTab);
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
            lblCurrentDate.Text = $"التاريخ: {now:yyyy/MM/dd}";
            lblCurrentTime.Text = $"الوقت: {now:hh:mm:ss tt}";
        }

        private void tvSystemTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetNavigationStatus(e.Node);
        }

        private void ToggleNotifications_Click(object? sender, EventArgs e)
        {
            mainLayout.SuspendLayout();
            try
            {
                if (!isNotificationsCollapsed)
                {
                    notificationsExpandedWidth = mainLayout.ColumnStyles[0].Width;
                    notificationsScrollPosition = pnlNotifications.AutoScrollPosition;
                }
                isNotificationsCollapsed = !isNotificationsCollapsed;
                pnlNotifications.Visible = !isNotificationsCollapsed;
                mainLayout.ColumnStyles[0].Width = isNotificationsCollapsed ? UiPixels(38) : notificationsExpandedWidth;
                btnToggleNotifications.Text = isNotificationsCollapsed ? "▶" : "◀  طي اللوحة الجانبية";
                btnToggleNotifications.AccessibleName = isNotificationsCollapsed
                    ? "إظهار لوحة التنبيهات والمفضلة" : "طي لوحة التنبيهات والمفضلة";
            }
            finally { mainLayout.ResumeLayout(true); }
            if (!isNotificationsCollapsed)
                pnlNotifications.AutoScrollPosition = new Point(
                    Math.Abs(notificationsScrollPosition.X), Math.Abs(notificationsScrollPosition.Y));
        }

        private void ToggleNavigationPane()
        {
            mainLayout.SuspendLayout();
            try
            {
                if (!isNavigationCollapsed)
                {
                    navigationTopNode = tvSystemTree.TopNode;
                    navigationExpandedWidth = mainLayout.ColumnStyles[2].Width;
                }
                isNavigationCollapsed = !isNavigationCollapsed;
                treeBody.Visible = !isNavigationCollapsed;
                mainLayout.ColumnStyles[2].Width = isNavigationCollapsed ? navigationRail.Width : navigationExpandedWidth;
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
            const string instruction = "لفتح الشاشة: انقر نقراً مزدوجاً أو اضغط Enter";
            string description = node?.Tag is string
                ? $"{node.Text} — {status ?? instruction}"
                : instruction;
            tvSystemTree.AccessibleDescription = description;
            if (node is not null) node.ToolTipText = node.Tag is string code
                ? $"{description} — {code}" : description;
            if (status is not null)
                MessageBox.Show(this, description, "حالة الشاشة", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (code == "01.02") { Activate(); SetNavigationStatus(node); return; }
            if (code == "01.01")
            {
                using var login = new FrmLogin();
                login.ShowDialog(this);
                return;
            }
            OpenWorkspaceControl(code, node.Text, () => CreateConnectedWorkspaceControl(code));
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.F4) && tabOpenScreens.SelectedTab is not null)
            {
                CloseWorkspacePage(tabOpenScreens.SelectedTab);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
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
