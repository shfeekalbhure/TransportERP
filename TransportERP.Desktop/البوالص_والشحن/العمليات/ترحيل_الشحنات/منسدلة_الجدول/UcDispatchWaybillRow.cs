using System;
using System.Windows.Forms;

namespace TransportERP.Desktop.البوالص_والشحن.العمليات.ترحيل_الشحنات.منسدلة_الجدول
{
    public partial class UcDispatchWaybillRow : UserControl
    {
        // Keep expansion state when an ancestor is temporarily hidden.
        private bool isExpanded;

        public UcDispatchWaybillRow()
        {
            InitializeComponent();
            TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this, embedded: true);
            Load += (_, _) => AdjustExpandedHeight();
            pnlWaybillHeader.SizeChanged += (_, _) => AdjustExpandedHeight();
            pnlPackages.SizeChanged += (_, _) => AdjustExpandedHeight();
            AdjustExpandedHeight();
        }

        private void btnExpandWaybill_Click(object sender, EventArgs e)
        {
            isExpanded = !isExpanded;

            pnlPackages.Visible = isExpanded;
            AdjustExpandedHeight();
            btnExpandWaybill.Text = isExpanded ? "−" : "+";

            Parent?.PerformLayout();
        }

        private void AdjustExpandedHeight()
        {
            int desiredHeight = Padding.Vertical + pnlWaybillHeader.Height
                + (isExpanded ? pnlPackages.Height : 0);
            if (Height != desiredHeight) Height = desiredHeight;
        }

        private void pnlPackages_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}