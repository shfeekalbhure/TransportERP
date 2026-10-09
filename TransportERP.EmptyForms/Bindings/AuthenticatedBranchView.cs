namespace TransportERP.EmptyForms;

/// <summary>Displays the already-authorized login scope; grants no document capabilities.</summary>
public interface IAuthenticatedBranchView
{
    void SetCurrentBranch(Guid branchId, string branchName);
}

internal static class AuthenticatedBranchView
{
    private sealed record Choice(Guid Id, string Label);

    internal static void Bind(ComboBox field, Guid branchId, string branchName)
    {
        if (branchId == Guid.Empty || string.IsNullOrWhiteSpace(branchName))
            throw new ArgumentException("An authoritative branch identity and name are required.");
        field.DisplayMember = nameof(Choice.Label);
        field.ValueMember = nameof(Choice.Id);
        field.DataSource = new[] { new Choice(branchId, branchName) };
        field.SelectedIndex = 0;
        field.Enabled = false;
    }
}

public partial class UcInventoryQuantityReservation : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(cmbBranch, branchId, branchName);
}
public partial class UcInventoryExternalRepairOrder : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcInventoryCustodyReceipt : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcInventoryCustodyIssue : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcInventoryDamagedIssueOrder : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcForeignPurchaseReceipt : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcRepresentativeCommissions : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcCustomerInstallmentSettlement : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcPurchaseRequest : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcPurchaseAdditionalDiscount : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcForeignPurchaseCosting : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcInventoryManualStocktake : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcInventoryTransfer : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcInventoryMaterialRequest : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcInventoryTransferReceipt : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcInventorySettlement : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcInventoryReceiptAuthorization : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcInventoryIssueOrder : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcPurchaseTechnicalComparison : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcPurchaseComparisonNomination : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
public partial class UcInventoryReceiptOrder : IAuthenticatedBranchView
{
    public void SetCurrentBranch(Guid branchId, string branchName) => AuthenticatedBranchView.Bind(fieldBranch, branchId, branchName);
}
