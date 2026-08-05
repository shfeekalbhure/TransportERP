using System;
using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.Setup.Geographic
{
    /// <summary>
    /// شاشة GEN-007 — المناطق.
    /// </summary>
    public partial class FrmAreas : Form
    {
        public FrmAreas()
        {
            InitializeComponent();
            NormalizeControlNames();
        }

        /// <summary>
        /// توحيد الأسماء البرمجية للعناصر التي أُنشئت سابقًا بأسماء افتراضية من المصمم.
        /// لا يغيّر هذا الإجراء توزيع الشاشة أو تصميمها.
        /// </summary>
        private void NormalizeControlNames()
        {
            panel2.Name = "pnlMainDataSection";
            groupBox1.Name = "grpMainData";
            tableLayoutPanel1.Name = "tlpSearch";
            tableLayoutPanel2.Name = "tlpAuditInfo";
            tableLayoutPanel3.Name = "tlpStatistics";
            dgvGovernorates.Name = "dgvAreas";
            Column1.Name = "colSequence";

            txtDirectorateNameAr.Name = "cmbDirectorate";
            lblDirectorateNameArTitle.Name = "lblDirectorateTitle";

            label1.Name = "lblLegacyBreadcrumb";
            label2.Name = "lblLegacyScreenTitle";
            label3.Name = "lblLegacyToolbarSpacer";
            label4.Name = "lblLegacySectionTitle";

            button12.Name = "btnLegacyNew";
            button11.Name = "btnLegacySave";
            button10.Name = "btnLegacyEdit";
            button9.Name = "btnLegacySearch";
            button8.Name = "btnLegacyFirst";
            button7.Name = "btnLegacyPrevious";
            button6.Name = "btnLegacyNext";
            button5.Name = "btnLegacyLast";
            button4.Name = "btnLegacyDelete";
            button3.Name = "btnLegacyPrint";

            Text = "المناطق";
            lblScreenTitle.Text = "المناطق";
            grpGrid.Text = "جدول البيانات";
            grpSearch.Text = "بحث وتصفية";
        }

        private void tlpMainData_Paint(object sender, PaintEventArgs e)
        {
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void txtDirectorateNameAr_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
