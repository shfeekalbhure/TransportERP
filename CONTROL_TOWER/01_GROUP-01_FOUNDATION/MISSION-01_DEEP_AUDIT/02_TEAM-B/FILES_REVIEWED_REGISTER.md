# TEAM-B Files Reviewed Register

## Mandatory governance — READ FULL

- CONTROL_TOWER/README.md
- CONTROL_TOWER/00_GOVERNANCE/MASTER_CONTROL_ORDER.md
- CONTROL_TOWER/00_GOVERNANCE/REPORT_OUTPUT_RULES.md
- CONTROL_TOWER/00_GOVERNANCE/DECISIONS/DB-GOV-001.md
- CONTROL_TOWER/00_GOVERNANCE/DECISIONS/MISSION-SEQUENCE-001.md
- CONTROL_TOWER/00_GOVERNANCE/DECISIONS/PROJECT-GROUPING-001.md
- CONTROL_TOWER/00_GOVERNANCE/REGISTERS/CONTROL_TOWER_TASK_QUEUE.md
- CONTROL_TOWER/00_GOVERNANCE/REGISTERS/MISSION_HANDOFF_AND_SEAL_REGISTER.md
- CONTROL_TOWER/00_GOVERNANCE/REGISTERS/REPORT_ARCHIVE_REGISTER.md
- CONTROL_TOWER/00_GOVERNANCE/REGISTERS/TEAM_FORMATION_AND_ASSIGNMENT_REGISTER.md
- CONTROL_TOWER/01_GROUP-01_FOUNDATION/MISSION-01_DEEP_AUDIT/00_COMMAND/TRANSPORTERP_MASTER_DEEP_AUDIT_COMMAND_2026-08-28_AR_FINAL.md
- CONTROL_TOWER/01_GROUP-01_FOUNDATION/MISSION-01_DEEP_AUDIT/02_TEAM-B/START_ORDER.md

## Solution/build/project definitions — READ FULL

- TransportERP.slnx
- TransportERP.Api/TransportERP.Api.csproj
- TransportERP.Application/TransportERP.Application.csproj
- TransportERP.Contracts/TransportERP.Contracts.csproj
- TransportERP.Desktop/TransportERP.Desktop.csproj
- TransportERP.Infrastructure/TransportERP.Infrastructure.csproj
- TransportERP.Mobile.Admin/TransportERP.Mobile.Admin.csproj
- TransportERP.Mobile.Customer/TransportERP.Mobile.Customer.csproj
- TransportERP.Mobile.Driver/TransportERP.Mobile.Driver.csproj
- TransportERP.Tests/TransportERP.Tests.csproj
- TransportERP/TransportERP.Domain.csproj

## Source — DEEP READ

- TransportERP.Api/Program.cs
- TransportERP.Api/Waybills/WaybillApiModule.cs
- TransportERP.Api/Waybills/WaybillFinanceApiModule.cs
- TransportERP.Api/Waybills/ShippingExecutionApiModule.cs
- TransportERP.Infrastructure/Persistence/TransportErpDbContext.cs
- TransportERP.Infrastructure/Persistence/P1Entities.cs
- TransportERP.Infrastructure/Persistence/P2WaybillEntities.cs
- TransportERP.Infrastructure/Persistence/P2ShippingEntities.cs
- TransportERP.Infrastructure/Persistence/SyncOperationService.cs
- TransportERP.Infrastructure/Persistence/AuditEventService.cs
- TransportERP.Infrastructure/Persistence/VoucherLifecycleService.cs
- TransportERP.Infrastructure/Persistence/WaybillPersistenceServices.cs
- TransportERP.Infrastructure/Persistence/WaybillFinancePersistence.cs
- TransportERP.Infrastructure/Persistence/ShippingExecutionPersistence.cs
- TransportERP.Infrastructure/Persistence/TransportErpP2FinanceModel.cs
- TransportERP.Infrastructure/Persistence/TransportErpP2ShippingModel.cs
- TransportERP.Infrastructure/Persistence/P2FinanceAppendOnlyInterceptor.cs
- TransportERP.Infrastructure/Persistence/P2ShippingAppendOnlyInterceptor.cs
- TransportERP.Application/P1Baseline/P1InMemoryBaseline.cs
- TransportERP.Application/Waybills/WaybillApplicationService.cs
- TransportERP.Application/Waybills/WaybillFinanceApplicationService.cs
- TransportERP.Application/Waybills/ShippingExecutionApplicationService.cs
- TransportERP/Waybills/WaybillAggregate.cs
- TransportERP/Waybills/WaybillFinancialRules.cs
- TransportERP/Waybills/ShippingExecutionRules.cs
- TransportERP.Desktop/Waybills/WaybillFoundationForms.cs
- TransportERP.Desktop/Waybills/WaybillFinanceForms.cs
- TransportERP.Desktop/Waybills/ShippingExecutionForms.cs
- TransportERP.Desktop/Waybills/ShippingExecutionW3Models.cs
- TransportERP.Desktop/CoreUI/Architecture/TransportScreenProfile.cs

## Source — ENUMERATED AND STATIC-SCANNED

- جميع 14 ملفًا في TransportERP.Contracts.
- جميع 10 migrations غير المولدة وTransportErpDbContextModelSnapshot.cs.
- جميع ملفات Infrastructure غير المولدة وعددها 39.
- جميع ملفات Tests وعددها 22.
- جميع ملفات Domain/Application/API/Desktop غير المولدة.
- جميع مجلدات Mobile الثلاثة؛ لا تحتوي ملفات C#.

## Tests/workflows — READ OR ASSERTION-SCANNED

- TransportERP.Tests/PostgreSqlTestEnvironment.cs
- TransportERP.Tests/ApiAuthenticationAndAuditTests.cs
- TransportERP.Tests/AuditEventPersistenceTests.cs
- TransportERP.Tests/SyncOperationPersistenceTests.cs
- TransportERP.Tests/VoucherLifecyclePersistenceTests.cs
- جميع ملفات P2C01A/B/C وP1/W05 للاختبار: أسماء الحالات، categories، assertions، ومراجع المشروع.
- جميع ملفات .github/workflows السبعة.

## Documentation/Kurrasa — TARGETED PRIMARY REVIEW

- documentation/architecture/ADR-001-PostgreSQL-18.6-DB-Selection.md
- documentation/architecture/P1_PHYSICAL_SCHEMA_POSTGRESQL.md
- documentation/design/00_DESIGN_OPERATING_MODEL_V1.md
- documentation/design/04_SCREEN_WORK_QUEUE.csv
- documentation/closeout/P1/W1_DATA_CONTRACT_REGISTER.csv
- documentation/closeout/P1/W2_ACTION_CONTRACT_REGISTER.csv
- documentation/closeout/P1/W3_SCREEN_CONTRACT_REGISTER.csv
- documentation/closeout/P1/P1_FINAL_RELEASE_NOTE.md
- documentation/closeout/P2/P2_C01_W1_DATA_CONTRACT_REGISTER.csv
- documentation/closeout/P2/P2_C01_W2_ACTION_CONTRACT_REGISTER.csv
- documentation/closeout/P2/P2_C01_W3_SCREEN_CONTRACT_REGISTER.csv
- documentation/closeout/P2/P2_C01_DOMAIN_COVERAGE_REGISTER.csv
- documentation/closeout/P2/P2_C01_OFFLINE_SYNC_POLICY.md
- documentation/closeout/P2/P2_C01_C_CLOSURE_2026-08-21.md
- 70 ملفات screen-spec.md: جرد وحالة وربط، مع قراءة عينات حاكمة.
- Kurrasa main official file version 72: READ FULL.
- Ticket decisions file version 2: READ FULL.
- TRV-BOOK contract version 3: TARGETED READ.
- Shipping draft/latest screen register: inventory and targeted status review.

## Explicit exclusion

لم يُفتح أي ملف داخل 01_TEAM-A. أسماء المسارات وحدها التي ظهرت في جرد filesystem لا تعد قراءة محتوى.

