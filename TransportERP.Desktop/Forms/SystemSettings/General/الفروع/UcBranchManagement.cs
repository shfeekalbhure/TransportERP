using System;
using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.SystemSettings.General.الفروع
{
    public partial class UcBranchManagement : UserControl, TransportERP.Desktop.CoreUI.IFoundationScreen
    {
    public TransportERP.Desktop.CoreUI.FoundationUiSession Foundation { get; }
    public bool HasUnsavedChanges => Foundation.HasUnsavedChanges;
    public bool IsBusy => Foundation.IsBusy;
    public bool ConfirmLeave() => Foundation.ConfirmLeave();

        private bool adjustingSectionHeights;

        public UcBranchManagement()
        {
            InitializeComponent();
            TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
            btnClose.Click += BtnClose_Click;
            pnlContent.SizeChanged += (_, _) => AdjustSectionHeights();
            pnlHeader.SizeChanged += (_, _) => AdjustSectionHeights();
            tlpAuditInfo.SizeChanged += (_, _) => AdjustSectionHeights();
            Load += (_, _) => AdjustSectionHeights();
            AdjustSectionHeights();
            ConfigureUnavailableActions();
            Foundation = new TransportERP.Desktop.CoreUI.FoundationUiSession(this);
        Foundation.RequireFields(new[] { "btnSave", "btnPublish", "btnValidate" }, txtBranchNameAr.Name, cmbBranchType.Name, cmbBaseCurrency.Name, cmbCountry.Name);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

        private void ConfigureUnavailableActions()
        {
            const string reason = "غير متاح حاليًا: لم يُنفَّذ هذا الإجراء ولم يُربط بخدمة تشغيلية.";
            components ??= new System.ComponentModel.Container();
            var availabilityTip = new ToolTip(components) { ShowAlways = true };
            tabCompaniesSection.Text += " — الحفظ غير مرتبط";
            availabilityTip.SetToolTip(btnClose, "إغلاق الشاشة");
            foreach (var button in new[] { button1, btnSave, btnEdit, btnDelete, button2, btnFirst, btnPrevious, btnNext, btnLast, btnUndo, btnSearch, btnPrint, button4, btnAddLogo, btnRemoveLogo })
            {
                button.Enabled = false;
                button.AccessibleDescription = reason;
                availabilityTip.SetToolTip(button, reason);
                // Disabled buttons do not reliably receive hover messages. Their host
                // also explains the disabled state without changing the layout.
                if (button.Parent is Control host)
                    availabilityTip.SetToolTip(host, reason);
            }
        }

        // Keep both existing sections visible; their tabs scroll internally at smaller sizes.
        private void AdjustSectionHeights()
        {
            if (adjustingSectionHeights || IsDisposed) return;
            int available = pnlContent.ClientSize.Height - pnlContent.Padding.Vertical
                - pnlHeader.Height - tlpAuditInfo.Height;
            if (available <= 0) return;
            adjustingSectionHeights = true;
            try
            {
                int basicTabChrome = Math.Max(0, tabCompanyManagement.Height - pnlBasicData.Height);
                int preferredBasic = tabCompaniesSection.MinimumSize.Height
                    + pnlBasicData.Padding.Vertical + basicTabChrome;
                int additionalTabChrome = Math.Max(0, tabAdditionalData.Height - tpAddresses.Height);
                int additionalGroupChrome = Math.Max(0, groupBox2.Height - groupBox2.DisplayRectangle.Height);
                int preferredAdditional = tlpReceiverData.MinimumSize.Height
                    + tpAddresses.Padding.Vertical + additionalTabChrome + additionalGroupChrome
                    + tableLayoutPanel3.Margin.Vertical;
                int basicHeight = Math.Min(preferredBasic,
                    Math.Max(available / 2, available - preferredAdditional));
                if (tabCompanyManagement.Height != basicHeight)
                    tabCompanyManagement.Height = basicHeight;
            }
            finally { adjustingSectionHeights = false; }
        }

        public event EventHandler? CloseRequested;
        internal Button CloseButton => btnClose;

        private void BtnClose_Click(object? sender, EventArgs e)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
