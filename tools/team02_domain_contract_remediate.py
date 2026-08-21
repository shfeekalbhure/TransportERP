from pathlib import Path


def replace_once(path: str, old: str, new: str) -> None:
    p = Path(path)
    text = p.read_text(encoding="utf-8")
    count = text.count(old)
    if count != 1:
        raise SystemExit(f"{path}: expected exactly one anchor, found {count}: {old!r}")
    p.write_text(text.replace(old, new, 1), encoding="utf-8")


# W1-P2C01-004 / W3 SHP-008: preserve explicit WaybillItem.Volume through contracts.
replace_once(
    "TransportERP.Contracts/Waybills/WaybillContracts.cs",
    "    IReadOnlyList<string>? RiskFlags,\n    string? Notes);",
    "    IReadOnlyList<string>? RiskFlags,\n    string? Notes,\n    decimal? Volume = null);",
)
replace_once(
    "TransportERP.Contracts/Waybills/WaybillContracts.cs",
    "    IReadOnlyList<string> RiskFlags,\n    string? Notes);",
    "    IReadOnlyList<string> RiskFlags,\n    string? Notes,\n    decimal? Volume = null);",
)

replace_once(
    "TransportERP/Waybills/WaybillAggregate.cs",
    "    string RiskFlagsJson,\n    string? Notes)",
    "    string RiskFlagsJson,\n    string? Notes,\n    decimal? Volume = null)",
)
replace_once(
    "TransportERP/Waybills/WaybillAggregate.cs",
    "new[] { Weight, Length, Width, Height, DeclaredValue }",
    "new[] { Weight, Length, Width, Height, Volume, DeclaredValue }",
)

replace_once(
    "TransportERP.Application/Waybills/WaybillApplicationService.cs",
    "            JsonSerializer.Serialize(input.RiskFlags ?? Array.Empty<string>()), input.Notes);",
    "            JsonSerializer.Serialize(input.RiskFlags ?? Array.Empty<string>()), input.Notes, input.Volume);",
)
replace_once(
    "TransportERP.Application/Waybills/WaybillApplicationService.cs",
    "                JsonSerializer.Deserialize<string[]>(x.RiskFlagsJson) ?? Array.Empty<string>(), x.Notes)).ToList(),",
    "                JsonSerializer.Deserialize<string[]>(x.RiskFlagsJson) ?? Array.Empty<string>(), x.Notes, x.Volume)).ToList(),",
)

replace_once(
    "TransportERP.Infrastructure/Persistence/P2WaybillEntities.cs",
    "    public decimal? Height { get; set; }\n    public decimal? DeclaredValue { get; set; }",
    "    public decimal? Height { get; set; }\n    public decimal? Volume { get; set; }\n    public decimal? DeclaredValue { get; set; }",
)

replace_once(
    "TransportERP.Infrastructure/Persistence/WaybillPersistenceServices.cs",
    "                i.Length, i.Width, i.Height, i.DeclaredValue, i.OriginCountryId, i.RiskFlagsJson, i.Notes)));",
    "                i.Length, i.Width, i.Height, i.DeclaredValue, i.OriginCountryId, i.RiskFlagsJson, i.Notes, i.Volume)));",
)
replace_once(
    "TransportERP.Infrastructure/Persistence/WaybillPersistenceServices.cs",
    "        target.Height = source.Height;\n        target.DeclaredValue = source.DeclaredValue;",
    "        target.Height = source.Height;\n        target.Volume = source.Volume;\n        target.DeclaredValue = source.DeclaredValue;",
)

replace_once(
    "TransportERP.Infrastructure/Persistence/TransportErpP2ModelCustomizer.cs",
    "(\\\"Height\\\" IS NULL OR \\\"Height\\\" >= 0) AND (\\\"DeclaredValue\\\" IS NULL OR \\\"DeclaredValue\\\" >= 0)",
    "(\\\"Height\\\" IS NULL OR \\\"Height\\\" >= 0) AND (\\\"Volume\\\" IS NULL OR \\\"Volume\\\" >= 0) AND (\\\"DeclaredValue\\\" IS NULL OR \\\"DeclaredValue\\\" >= 0)",
)
replace_once(
    "TransportERP.Infrastructure/Persistence/TransportErpP2ModelCustomizer.cs",
    "        item.Property(x => x.Height).HasPrecision(19, 4);\n        item.Property(x => x.DeclaredValue).HasPrecision(19, 4);",
    "        item.Property(x => x.Height).HasPrecision(19, 4);\n        item.Property(x => x.Volume).HasPrecision(19, 4);\n        item.Property(x => x.DeclaredValue).HasPrecision(19, 4);",
)

# Complete release replay equality and feed authoritative volume into split normalization.
replace_once(
    "TransportERP.Infrastructure/Persistence/ShippingExecutionPersistence.cs",
    "            if (replay.WaybillItemId != itemId || replay.Quantity != request.Quantity)\n                throw new WaybillPersistenceException(\"IDEMPOTENCY_CONFLICT\");",
    "            if (replay.WaybillItemId != itemId || replay.Quantity != request.Quantity ||\n                !SameInstant(replay.ReleasedAt, request.ReleasedAt))\n                throw new WaybillPersistenceException(\"IDEMPOTENCY_CONFLICT\");",
)
replace_once(
    "TransportERP.Infrastructure/Persistence/ShippingExecutionPersistence.cs",
    "            if (replay is not null && replay.WaybillItemId == itemId && replay.Quantity == request.Quantity)\n                return await ItemState(context, waybillId, itemId, cancellationToken);",
    "            if (replay is not null && replay.WaybillItemId == itemId && replay.Quantity == request.Quantity &&\n                SameInstant(replay.ReleasedAt, request.ReleasedAt))\n                return await ItemState(context, waybillId, itemId, cancellationToken);",
)
replace_once(
    "TransportERP.Infrastructure/Persistence/ShippingExecutionPersistence.cs",
    "                    candidate.Item.Width,\n                    candidate.Item.Height);",
    "                    candidate.Item.Width,\n                    candidate.Item.Height,\n                    candidate.Item.Volume);",
)

# PostgreSQL proof: explicit volume differs from LxWxH and must be allocated proportionally.
replace_once(
    "TransportERP.Tests/P2C01CPhysicalMeasurePostgreSqlTests.cs",
    "            Quantity = 10m, Pieces = 10, Weight = 100m, Length = 2m, Width = 3m, Height = 4m,\n            RiskFlagsJson = \"[]\"",
    "            Quantity = 10m, Pieces = 10, Weight = 100m, Length = 2m, Width = 3m, Height = 4m, Volume = 50m,\n            RiskFlagsJson = \"[]\"",
)
replace_once(
    "TransportERP.Tests/P2C01CPhysicalMeasurePostgreSqlTests.cs",
    "        Assert.Equal(9.6m, first.Lines.Single().Volume);\n        Assert.Equal(60m, second.Lines.Single().Weight);\n        Assert.Equal(14.4m, second.Lines.Single().Volume);\n        Assert.Equal(100m, first.Lines.Single().Weight + second.Lines.Single().Weight);\n        Assert.Equal(24m, first.Lines.Single().Volume + second.Lines.Single().Volume);",
    "        Assert.Equal(20m, first.Lines.Single().Volume);\n        Assert.Equal(60m, second.Lines.Single().Weight);\n        Assert.Equal(30m, second.Lines.Single().Volume);\n        Assert.Equal(100m, first.Lines.Single().Weight + second.Lines.Single().Weight);\n        Assert.Equal(50m, first.Lines.Single().Volume + second.Lines.Single().Volume);",
)

replace_once(
    "TransportERP.Tests/P2C01CShippingExecutionTests.cs",
    "    [Fact]\n    public void Split_allocation_measure_totals_do_not_duplicate_the_original_line()",
    "    [Fact]\n    public void Explicit_line_volume_is_authoritative_for_split_allocation()\n    {\n        var part = ShippingExecutionRules.AllocatePhysicalMeasures(\n            itemQuantity: 10m, allocatedQuantity: 4m,\n            lineWeight: 100m, length: 2m, width: 3m, height: 4m, lineVolume: 50m);\n\n        Assert.Equal(40m, part.AllocatedWeight);\n        Assert.Equal(20m, part.AllocatedVolume);\n    }\n\n    [Fact]\n    public void Split_allocation_measure_totals_do_not_duplicate_the_original_line()",
)

print("TEAM-02 bounded domain/contracts remediation applied.")
