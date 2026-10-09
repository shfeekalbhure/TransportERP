namespace TransportERP.EmptyForms;

/// <summary>إدارة العملاء — 03.01.01. Empty scaffold only.</summary>
public partial class UcScreen_03_01_01 : System.Windows.Forms.UserControl
{
    public UcScreen_03_01_01()
    {
        InitializeComponent();
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
