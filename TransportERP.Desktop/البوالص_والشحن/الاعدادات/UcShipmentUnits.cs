using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TransportERP.Desktop.البوالص_والشحن.الاعدادات
{
    public partial class UcShipmentUnits : UserControl
    {
        public UcShipmentUnits()
        {
            InitializeComponent();
            TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
        
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtWarningMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void tlpShipmentCategory_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tlpShipmentCategory_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void nudConversionFactor_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNotes_Click(object sender, EventArgs e)
        {

        }
    }
}
