namespace TransportERP.EmptyForms;

/// <summary>إقفال وردية الصندوق — 04.11.06. Empty scaffold only.</summary>
public partial class UcScreen_04_11_06 : System.Windows.Forms.UserControl
{
    public UcScreen_04_11_06()
    {
        InitializeComponent();
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
