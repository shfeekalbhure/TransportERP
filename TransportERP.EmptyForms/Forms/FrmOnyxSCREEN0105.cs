namespace TransportERP.EmptyForms;

/// <summary>استحقاق شيكات سندات الصرف – يدويًا; reference SCREEN-0105, PDF page 100. Empty scaffold.</summary>
public partial class UcOnyxSCREEN0105 : System.Windows.Forms.UserControl
{
    public UcOnyxSCREEN0105() { InitializeComponent(); 
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
