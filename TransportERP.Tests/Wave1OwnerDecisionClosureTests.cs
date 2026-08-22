using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Wave1;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class Wave1OwnerDecisionClosureTests
{
    [Fact]
    public async Task GEN003_promotes_ISO_fields_with_validation_and_uniqueness()
    {
        await using var db = new Wave1CountryAuthorityDbContext(new DbContextOptionsBuilder<Wave1CountryAuthorityDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options);
        var service = new Wave1CountryAuthorityService(db);
        var context = new Wave1GeoOperationContext(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "device", "127.0.0.1");
        var yemen = await service.CreateAsync(new CreateCountryRequest("YE", "اليمن", "Yemen", "يمني", "ye", "yem", "+967"), context);
        Assert.Equal("YE", yemen.ISO2); Assert.Equal("YEM", yemen.ISO3); Assert.Equal("+967", yemen.DialingCode);
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(new CreateCountryRequest("Y2", "اختبار", null, null, "YE", null, null), context));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(new CreateCountryRequest("NOISO", "بدون", null, null), context));
    }

    [Fact]
    public async Task GEN013_derives_LastNumber_without_reusing_historical_reservations_and_persists_metadata()
    {
        await using var db = new Wave1NumberingAuthorityDbContext(new DbContextOptionsBuilder<Wave1NumberingAuthorityDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options);
        var company = Guid.NewGuid(); var branch = Guid.NewGuid(); var sequenceId = Guid.NewGuid();
        db.Sequences.Add(new Wave1NumberSequenceRecord { Id=sequenceId, CompanyId=company, BranchId=branch, DocumentType="WAYBILL", Prefix="WB-", NextValue=10, ResetPolicy="NONE", Status="ACTIVE", Version=1 });
        db.Reservations.Add(new Wave1NumberReservationRecord { Id=Guid.NewGuid(), SequenceId=sequenceId, CompanyId=company, BranchId=branch, IdempotencyKey="old", NumberValue=12, RenderedNumber="WB-00000012", ReservedAt=DateTimeOffset.UtcNow, State="VOID" });
        await db.SaveChangesAsync();
        var service = new Wave1NumberingAuthorityService(db); var ctx = new OperationContext(Guid.NewGuid(), company, branch, Guid.NewGuid());
        Assert.Equal(12, Assert.Single(await service.ListAsync(ctx)).LastNumber);
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ProtectedActionAsync(ctx, sequenceId, new ProtectedNumberSequenceActionRequest(11,1,"too low")));
        var changed = await service.ProtectedActionAsync(ctx, sequenceId, new ProtectedNumberSequenceActionRequest(15,1,"approved adjustment"));
        Assert.Equal(15, changed!.LastNumber);
        var updated = await service.UpdateAsync(ctx, sequenceId, new UpdateNumberSequenceRequest("WB-","NONE","ACTIVE",changed.Version,"metadata","WB","ترقيم البوالص","Waybill numbering","ملاحظات",Guid.NewGuid()));
        Assert.Equal("WB", updated!.Code); Assert.Equal("ملاحظات", updated.Notes); Assert.Contains("FISCAL_YEAR", updated.Scope);
        var reservation = await service.ReserveAsync(ctx, sequenceId, new NumberReservationCommandRequest("new-16"));
        Assert.Equal((ulong)16, reservation.NumberValue);
    }

    [Fact]
    public async Task ACC036_keeps_groups_and_types_physically_separate_and_kind_discriminated()
    {
        await using var db = CreateAuthorityDb(); var company=Guid.NewGuid(); var branch=Guid.NewGuid(); var ctx=new OperationContext(Guid.NewGuid(),company,branch,Guid.NewGuid());
        var service=new Wave1AccountClassificationAuthorityService(db);
        var group=await service.CreateAsync(ctx,new CreateACC036Request("GROUP","MAIN","رئيسي","Main",AllowsPostingAccounts:true,ShowInFinancialStatements:true,DisplayOrder:1));
        var type=await service.CreateAsync(ctx,new CreateACC036Request("TYPE","MAIN","أصول","Assets",FinancialClassification:"ASSET",NormalBalance:"DEBIT"));
        Assert.Equal("GROUP",group.Kind); Assert.Equal("TYPE",type.Kind); Assert.Single(db.AccountGroups); Assert.Single(db.AccountTypes);
        Assert.Equal(2, await db.Set<AuditEvent>().CountAsync());
    }

    [Fact]
    public async Task ACC074_aging_joins_customer_and_source_document_and_subtracts_applied_allocations()
    {
        await using var authority=CreateAuthorityDb(); await using var accounting=CreateAccountingDb();
        var company=Guid.NewGuid(); var branch=Guid.NewGuid(); var currency=Guid.NewGuid(); var customer=Guid.NewGuid(); var je=Guid.NewGuid(); var item=Guid.NewGuid();
        authority.Customers.Add(new Wave1CustomerRecord{Id=customer,CompanyId=company,Code="C-001",ArabicName="عميل اختبار",ControlAccountId=Guid.NewGuid(),IsActive=true,Version=1});
        authority.OpenItems.Add(new Wave1OpenItemRecord{Id=item,CompanyId=company,BranchId=branch,PartyType="CUSTOMER",CustomerId=customer,SourceDocumentType="JOURNAL_ENTRY",SourceDocumentId=je,JournalEntryId=je,JournalLineNo=1,CurrencyId=currency,OriginalAmount=100m,DueDate=new DateTime(2026,8,10),Status="OPEN",Version=1});
        authority.PaymentAllocations.Add(new Wave1PaymentAllocationRecord{Id=Guid.NewGuid(),CompanyId=company,SourcePaymentType="RECEIPT_VOUCHER",SourcePaymentId=Guid.NewGuid(),TargetOpenItemId=item,Amount=40m,CurrencyId=currency,AllocationDate=DateTimeOffset.UtcNow,Status="APPLIED",Version=1});
        accounting.JournalEntries.Add(new JournalEntry{Id=je,CompanyId=company,BranchId=branch,DocumentNo="JE-100",FiscalPeriodId=Guid.NewGuid(),EntryDate=new DateTime(2026,8,1),Status="POSTED",SourceType="TEST",TotalDebit=100,TotalCredit=100,CurrencyId=currency,ExchangeRate=1});
        await authority.SaveChangesAsync(); await accounting.SaveChangesAsync();
        var service=new Wave1AgingAuthorityService(authority,accounting);
        var page=await service.QueryCustomerAsync(company,branch,new ACC074QueryRequest(new DateTime(2026,8,20),branch,currency,customer,Page:1,PageSize:200));
        var row=Assert.Single(page.Items); Assert.Equal("C-001",row.PartyCode); Assert.Equal("عميل اختبار",row.PartyName); Assert.Equal(60m,row.Days1To30); Assert.Equal(60m,row.TotalOutstanding);
        var detail=Assert.Single((await service.DrillCustomerAsync(company,branch,new ACC074DrillDownRequest(customer,new DateTime(2026,8,20),branch,currency,Page:1,PageSize:200))).Items);
        Assert.Equal("JE-100",detail.DocumentNo); Assert.Equal(40m,detail.SettledAmount); Assert.Equal(60m,detail.OutstandingAmount);
        Assert.Null(typeof(Wave1OpenItemRecord).GetProperty("PartyName")); Assert.Null(typeof(Wave1OpenItemRecord).GetProperty("DocumentNo"));
    }

    [Fact]
    public async Task ACC050_uses_account_mapping_and_controlled_override_not_reference_type_keywords()
    {
        await using var authority=CreateAuthorityDb(); await using var accounting=CreateAccountingDb();
        var company=Guid.NewGuid(); var branch=Guid.NewGuid(); var currency=Guid.NewGuid(); var account=Guid.NewGuid(); var receipt=Guid.NewGuid(); var payment=Guid.NewGuid();
        accounting.ReceiptVouchers.Add(new ReceiptVoucher{Id=receipt,CompanyId=company,BranchId=branch,VoucherNo="RV-1",VoucherDate=new DateTime(2026,8,5),PayerName="A",ReferenceType="CAPEX_INVEST_KEYWORD_MUST_NOT_WIN",PaymentMethodCode="CASH",Amount=100,CurrencyId=currency,Status="POSTED",CollectedBy=Guid.NewGuid()});
        accounting.PaymentVouchers.Add(new PaymentVoucher{Id=payment,CompanyId=company,BranchId=branch,VoucherNo="PV-1",VoucherDate=new DateTime(2026,8,6),PayeeName="B",ReferenceType="OPERATING_KEYWORD_MUST_NOT_WIN",PaymentMethodCode="BANK",Amount=40,CurrencyId=currency,Status="POSTED",PaidBy=Guid.NewGuid()});
        var entry=Guid.NewGuid(); accounting.JournalEntries.Add(new JournalEntry{Id=entry,CompanyId=company,BranchId=branch,DocumentNo="JE-RV",FiscalPeriodId=Guid.NewGuid(),EntryDate=new DateTime(2026,8,5),Status="POSTED",SourceType="RECEIPT_VOUCHER",SourceId=receipt,TotalDebit=100,TotalCredit=100,CurrencyId=currency,ExchangeRate=1});
        accounting.JournalEntryLines.Add(new JournalEntryLine{JournalEntryId=entry,LineNo=1,AccountId=account,Debit=100,Credit=0,ForeignAmount=100,CurrencyId=currency});
        authority.CashFlowAccountMappings.Add(new Wave1CashFlowAccountMappingRecord{Id=Guid.NewGuid(),CompanyId=company,AccountId=account,Activity="OPERATING",IsActive=true,Version=1});
        authority.CashFlowMovementOverrides.Add(new Wave1CashFlowMovementOverrideRecord{Id=Guid.NewGuid(),CompanyId=company,MovementType="RECEIPT_VOUCHER",MovementId=receipt,Activity="FINANCING",Reason="approved",ApprovedByUserId=Guid.NewGuid(),IsActive=true,Version=1});
        await authority.SaveChangesAsync(); await accounting.SaveChangesAsync();
        var service=new Wave1CashFlowAuthorityService(authority,accounting);
        var page=await service.QueryAsync(company,branch,new ACC050QueryRequest(new DateTime(2026,8,1),new DateTime(2026,8,31),branch,currency,Page:1,PageSize:200));
        Assert.Equal("FINANCING",Assert.Single(page.Items,x=>x.DocumentNo=="RV-1").Activity);
        Assert.Equal("UNCLASSIFIED",Assert.Single(page.Items,x=>x.DocumentNo=="PV-1").Activity);
    }

    private static Wave1AccountingAuthorityDbContext CreateAuthorityDb()
        => new(new DbContextOptionsBuilder<Wave1AccountingAuthorityDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString("N")).ReplaceService<IModelCustomizer,Wave1AccountingAuthorityModelCustomizer>().Options);
    private static TransportErpDbContext CreateAccountingDb()
        => new(new DbContextOptionsBuilder<TransportErpDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options);
}
