namespace TransportERP.EmptyForms;

/// <summary>استحقاق شيكات سندات القبض – يدويًا; reference SCREEN-0104, PDF page 96. Empty scaffold.</summary>
public partial class UcOnyxSCREEN0104 : System.Windows.Forms.UserControl
{
    public UcOnyxSCREEN0104() { InitializeComponent(); 
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
