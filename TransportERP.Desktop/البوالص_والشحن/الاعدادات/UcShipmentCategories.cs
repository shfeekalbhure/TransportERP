using System;
using System.Windows.Forms;

namespace TransportERP.Desktop.البوالص_والشحن.الاعدادات
{
    // شاشة إعداد فئات وأصناف الشحن.
    // ترث من UserControl حتى تعمل داخل حاوية الشاشات في النظام.
    public partial class UcShipmentCategories : UserControl
    {
        // إنشاء الشاشة وتحميل عناصر الـ Designer.
        public UcShipmentCategories()
        {
            InitializeComponent();
            TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
        
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

        // حدث تحميل الشاشة.
        // يوضع فيه لاحقاً تحميل البيانات من الـ API أو قاعدة البيانات.
        private void UcShipmentCategories_Load(object sender, EventArgs e)
        {
        }

        private void tlpShipmentCategory_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
