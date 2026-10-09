namespace TransportERP.EmptyForms;

/// <summary>مطابقة البنوك; reference SCREEN-0106, PDF page 102. Empty scaffold.</summary>
public partial class UcOnyxSCREEN0106 : System.Windows.Forms.UserControl
{
    public UcOnyxSCREEN0106() { InitializeComponent(); 
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
