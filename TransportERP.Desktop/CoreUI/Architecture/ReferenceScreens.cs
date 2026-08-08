namespace TransportERP.Desktop.CoreUI.Architecture;

public sealed class MasterDataReferenceScreen : CoreUiReferenceScreen
{
    public MasterDataReferenceScreen() : base(TransportScreenProfile.MasterData, "البيانات الأساسية") { }
}

public sealed class TreeMasterReferenceScreen : CoreUiReferenceScreen
{
    public TreeMasterReferenceScreen() : base(TransportScreenProfile.TreeMaster, "البيانات الشجرية") { }
}

public sealed class TransactionReferenceScreen : CoreUiReferenceScreen
{
    public TransactionReferenceScreen() : base(TransportScreenProfile.Transaction, "العملية") { }
}

public sealed class ControlApprovalReferenceScreen : CoreUiReferenceScreen
{
    public ControlApprovalReferenceScreen() : base(TransportScreenProfile.ControlApproval, "الرقابة والاعتماد") { }
}

public sealed class ReportInquiryReferenceScreen : CoreUiReferenceScreen
{
    public ReportInquiryReferenceScreen() : base(TransportScreenProfile.ReportInquiry, "التقرير والاستعلام") { }
}

public sealed class SettingsReferenceScreen : CoreUiReferenceScreen
{
    public SettingsReferenceScreen() : base(TransportScreenProfile.Settings, "الإعدادات") { }
}
