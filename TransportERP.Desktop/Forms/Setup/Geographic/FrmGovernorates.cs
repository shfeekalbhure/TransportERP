using System;
using System.Drawing;
using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.Setup.Geographic
{
    /// <summary>
    /// شاشة GEN-004 — المحافظات.
    /// </summary>
    public partial class FrmGovernorates : Form
    {
        public FrmGovernorates()
        {
            InitializeComponent();
            NormalizeControlNames();
            EnsureAuditFooterVisible();
            Resize += (_, _) => EnsureAuditFooterVisible();
        }

        /// <summary>
        /// يضمن بقاء بيانات الإنشاء والتعديل والعدادات ظاهرة عند عرض الشاشة
        /// داخل تبويبات الشاشة الرئيسية وعلى الشاشات ذات الارتفاع المحدود.
        /// </summary>
        private void EnsureAuditFooterVisible()
        {
            MinimumSize = new Size(900, 700);
            pnlAuditFooter.Visible = true;
            pnlAuditFooter.Height = 100;
            pnlAuditFooter.BringToFront();
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
            comboBox1.Name = "cmbCountry";
            textBox2.Name = "txtSearchValue";
            dgvGovernorates.Name = "dgvGovernorates";
            Column1.Name = "colSequence";
            Column2.Name = "colGovernorateNameAr";

            button1.Name = "btnSearchAllTitle";
            button2.Name = "btnSearchFieldTitle";

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

            Text = "المحافظات";
            lblScreenTitle.Text = "المحافظات";
            grpGrid.Text = "جدول البيانات";
            grpSearch.Text = "بحث وتصفية";
        }

        private void FrmGovernorates_Load(object sender, EventArgs e)
        {
            EnsureAuditFooterVisible();
        }

        private void pnlToolbar_Paint(object sender, PaintEventArgs e)
        {
        }

        private void flpToolbar_Paint(object sender, PaintEventArgs e)
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void lblSearchAll_Click(object sender, EventArgs e)
        {
        }

        private void dgvGovernorates_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void lblSearchAll_Click_1(object sender, EventArgs e)
        {
        }

        private void tlpMainData_Paint(object sender, PaintEventArgs e)
        {
        }

        private void lblResultCountTitle_Click(object sender, EventArgs e)
        {
        }

        private void btnSearchFilter_Click(object sender, EventArgs e)
        {
        }
    }
}
