using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.Setup.Security
{
    public partial class AuditCountersControl : UserControl
    {
        public AuditCountersControl()
        {
            InitializeComponent();
            TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this, embedded: true);
        }
    }
}
