namespace TransportERP.EmptyForms;

/// <summary>مصمم التقارير الختامية والتدفقات والقوائم; reference SCREEN-0092, PDF page 43. Empty scaffold.</summary>
public partial class UcOnyxSCREEN0092 : System.Windows.Forms.UserControl
{
    public UcOnyxSCREEN0092() { InitializeComponent(); 
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
