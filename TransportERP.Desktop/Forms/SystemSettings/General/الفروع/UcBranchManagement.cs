using System;
using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.SystemSettings.General.الفروع
{
    public partial class UcBranchManagement : UserControl
    {
        public UcBranchManagement()
        {
            InitializeComponent();
            btnClose.Click += BtnClose_Click;
        }

        public event EventHandler? CloseRequested;
        internal Button CloseButton => btnClose;

        private void BtnClose_Click(object? sender, EventArgs e)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
