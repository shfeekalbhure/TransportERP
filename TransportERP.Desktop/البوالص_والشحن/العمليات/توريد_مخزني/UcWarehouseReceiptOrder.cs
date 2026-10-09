using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TransportERP.Desktop.البوالص_والشحن.العمليات.توريد_مخزني
{
    public partial class UcWarehouseReceiptOrder : UserControl
    {
        public UcWarehouseReceiptOrder()
        {
            InitializeComponent();
            TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
        
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
    }
}
