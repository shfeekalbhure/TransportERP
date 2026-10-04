using System;
using System.Windows.Forms;

namespace TransportERP.Desktop.البوالص_والشحن.العمليات.ترحيل_الشحنات.منسدلة_الجدول
{
    public partial class UcDispatchWaybillRow : UserControl
    {
        public UcDispatchWaybillRow()
        {
            InitializeComponent();
        }

        private void btnExpandWaybill_Click(object sender, EventArgs e)
        {
            var expanded = !pnlPackages.Visible;

            pnlPackages.Visible = expanded;
            this.Height = expanded ? 225 : 45;
            btnExpandWaybill.Text = expanded ? "−" : "+";

            Parent?.PerformLayout();
        }

        private void pnlPackages_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}