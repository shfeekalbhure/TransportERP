using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TransportERP.Desktop.البوالص_والشحن.الاعدادات
{
    public partial class UcShipmentTypes : UserControl
    {
        public UcShipmentTypes()
        {
            InitializeComponent();
            TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
        
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void UcShipmentTypes_Load(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void txtShipmentTypeNamehEn_TextChanged(object sender, EventArgs e)
        {

        }

        private void pnlToolbar_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
