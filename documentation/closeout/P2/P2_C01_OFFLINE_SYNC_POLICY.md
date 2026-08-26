# TransportERP — P2-C01 Offline / Sync Policy

**Release:** `P2-C01-WAYBILL-SHIPPING-2026-08`  
**Baseline dependency:** P1 SyncOperation / ConflictCase / ClientOperationId contracts on `master`  
**Status:** `G3_POLICY_ACCEPTED — RUNTIME_CONFORMANCE_PENDING_G4`
**Decision ID:** `DEC-G3-SYNC-20260825-01`

**Exact baseline:** `2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`
**Authority:** قرار مفوض من المالك داخل المحادثة بتاريخ 2026-08-25؛ لا يدعي توقيع أشخاص أو مراجعين غير مثبتين.

**Governance decision — 2026-08-25:** هذه السياسة جزء من قرار G3 المعتمد في `P1_SYNC_CONTRACT.md`. يقبل القرار أعمدة Offline/Sync في W2 فقط، ولا يقبل بقية W2 أو يغير status مراجعتها أو يمنح تفويض تنفيذ لمرحلة P2-C01 غير مغلقة. يبقى `sync.offline.enabled=false` حتى تثبت مطابقة التنفيذ في G4 ثم يصدر تفويض G5.

## 1. Governing rule

No offline client is authoritative for official numbering, financial close, financial reopen, or any operation that creates an irreversible server-side accounting effect. Offline work creates local drafts or queued operations only. Final state exists only after server validation and acknowledgement.

Offline is `deny by default`. لا تكفي صحة `OperationType` أو `EntityType`؛ يجب أن يطابق الطلب Action في allowlist الحصرية، وأن يجتاز registered device وUser وCompany/Branch وpermission وstate وquantity وHash وidempotency عند الخادم. لا يسمح بأي generic `DELETE` Offline.

## 2. Operation classes

| Class | Examples | Offline behavior | Server result |
|---|---|---|---|
| Draft-local | CreateWaybillDraft; UpdateWaybillDraft | Allowed | Sync creates/updates server draft; no official number |
| Capture-and-queue | Load; Arrival; Unload; POD; Collection by authorized field user; Exception | Allowed when role/device policy permits | Server revalidates scope; quantity; idempotency; state; then Accept/Reject/Conflict |
| Online-authoritative | Submit; Approve; official numbering; Hold/Release; Redirect; Trip settlement; Financial close/reopen | Not allowed as final offline operation | Must execute online |
| Read-cache | `SearchOperationalParties`; `ReadBasicWaybillCache` | خارج write queue؛ لا SyncOperation | Cache is never proof of current authorization or final state |

### 2.1 Exact Action mapping

`SyncP1Operations` transport capability موروث من P1 وليس Payload ActionCode. Read-cache منفصل عن write queue ولا ينشئ SyncOperation؛ الاسمان الصريحان هما `SearchOperationalParties` و`ReadBasicWaybillCache`.

| ActionCode | Class | EntityId | BaseVersion | ResultEntityId | Runtime availability on exact baseline |
|---|---|---|---|---|---|
| `CreateWaybillDraft` | Draft CREATE queue | اختياري فقط مع server-generated ID | غير مطلوب | WaybillId مطلوب | `ONLINE_RUNTIME_PRESENT; OFFLINE_DISPATCH_UNAVAILABLE` |
| `UpdateWaybillDraft` | Optimistic aggregate UPDATE queue | WaybillId مطلوب | **مطلوب** | WaybillId مطلوب | `ONLINE_RUNTIME_PRESENT; OFFLINE_DISPATCH_UNAVAILABLE` |
| `CreateOperationalParty` | CREATE queue | اختياري فقط مع server-generated ID | غير مطلوب | OperationalPartyId مطلوب | `ONLINE_RUNTIME_PRESENT; OFFLINE_DISPATCH_UNAVAILABLE` |
| `AddWaybillAttachment` | Metadata append queue | owner WaybillId مطلوب | غير مطلوب | attachment metadata ID مطلوب | `PHASE_RUNTIME_UNAVAILABLE`; لا binary |
| `RecordCollection` | Append-only business command | WaybillId مطلوب | غير مطلوب؛ ClientOperationId + server state/serialization | CollectionTransactionId مطلوب | `ONLINE_RUNTIME_PRESENT; OFFLINE_DISPATCH_UNAVAILABLE` |
| `LoadAllocatedQuantity` | Append-only quantity command | ManifestLineId مطلوب | غير مطلوب؛ ClientOperationId + serialized quantity/domain state | load/movement result ID مطلوب | `ONLINE_RUNTIME_PRESENT; OFFLINE_DISPATCH_UNAVAILABLE` |
| `RecordArrival` | Append-only business command | TripId مطلوب | غير مطلوب؛ ClientOperationId + server serialization/domain state | ArrivalReceiptId مطلوب | `PHASE_RUNTIME_UNAVAILABLE` |
| `RecordUnload` | Append-only quantity command | ArrivalReceiptId مطلوب | غير مطلوب؛ ClientOperationId + serialized quantity/domain state | unload/movement result ID مطلوب | `PHASE_RUNTIME_UNAVAILABLE` |
| `DeliverQuantity` | Append-only quantity command | WaybillId مطلوب | غير مطلوب؛ ClientOperationId + serialized availability/domain state | DeliveryId مطلوب | `PHASE_RUNTIME_UNAVAILABLE` |
| `RecordProofOfDelivery` | Metadata append queue | DeliveryId مطلوب | غير مطلوب | proof metadata ID مطلوب | `PHASE_RUNTIME_UNAVAILABLE`; لا binary |
| `CreateShipmentException` | Append-only business command | WaybillId مطلوب | غير مطلوب؛ ClientOperationId + domain state | ShipmentExceptionId مطلوب | `PHASE_RUNTIME_UNAVAILABLE` |
| `SearchOperationalParties` | Read-cache؛ لا write queue | لا ينطبق | لا ينطبق | لا ينطبق | Online source موجود؛ client cache غير مثبت |
| `ReadBasicWaybillCache` | Read-cache؛ لا write queue | WaybillId مطلوب | لا ينطبق | لا ينطبق | `RUNTIME_UNAVAILABLE` |

كل Action آخر `ONLINE_REQUIRED`. وإذا كان Action policy-allowed لكن runtime غير متاح في الإصدار، يرفض قبل enqueue بـ`ACTION_RUNTIME_UNAVAILABLE`. لا تفوض الخانة Offline تنفيذ Arrival/Delivery أو أي مرحلة لاحقة.

`AddWaybillAttachment` و`RecordProofOfDelivery` يرسلان metadata وcontent hash وlocal correlation فقط. لا يحمل Sync Payload binary ولا يفعّل binary upload؛ يبقى مؤجلًا حتى عقد مستقل لـresumable upload/hash/size/storage/failure واجتياز G4.

## 3. Idempotency

Every queueable write must carry a stable `ClientOperationId`. Retrying the same logical operation must not create a second collection, movement event, delivery, release, allocation, or number reservation.

Server behavior:

1. للصفوف الجديدة، locate prior operation بالمفتاح الدقيق `(CompanyId, RegisteredDeviceId, ClientOperationId)`؛ ولـlegacy فقط بالمفتاح `(CompanyId, DeviceId, ClientOperationId)` وفق preflight والفهارس الجزئية الحاكمة في `P1_SYNC_CONTRACT.md`؛
2. إذا طابقت بصمة `fp-v1` الكاملة، أعد النتيجة المقبولة/المرفوضة المحفوظة؛ لا يكفي تطابق `PayloadHash` وحده؛
3. إذا تطابق المفتاح واختلف أي حقل في البصمة، ارفض بـ`IDEMPOTENCY_MISMATCH`؛ وتصادم legacy يرفض بـ`LEGACY_IDEMPOTENCY_CONFLICT`؛
4. يجوز تسجيل محاولة HTTP بهوية `AttemptCorrelationId`، لكن business replay لا يكرر Audit القبول أو الأثر التجاري.

كل item جديد يحمل `ActionCode`, `ProtocolVersion`, `OperationCorrelationId` وconditional `EntityId`. القيمة الأولية الوحيدة المقبولة هي `ProtocolVersion=sync-v1` عبر `sync.protocol.allowed_versions=["sync-v1"]`. `OperationCorrelationId` UUID غير صفري ثابت للعملية، يخزن ويدخل `fp-v1`؛ أما `AttemptCorrelationId` فهو جديد لكل HTTP attempt ويأتي من header `X-Correlation-Id` المطابق لـproof claim `cid`، ولا يخزن في `SyncOperation` ولا يدخل البصمة. وجود `AttemptCorrelationId` أو الاسم القديم `RequestCorrelationId` داخل JSON لا يعمل كـalias ويعطي `REQUEST_SCHEMA_INVALID` عند إمكان parsing. عند نجاح CREATE/append تعاد `ResultEntityId` وتحفظ الخريطة `(CompanyId, RegisteredDeviceId, ClientOperationId) → ActionCode + ResultEntityId + outcome`؛ يدخل `ActionCode` في البصمة ولا يدخل مفتاح uniqueness، ويعيد business replay الخريطة والنتيجة نفسيهما.

## 4. Concurrency

Optimistic mutable aggregate UPDATE writes carry `BaseVersion` / ExpectedVersion. A stale operation becomes `CONFLICT` instead of silently overwriting server state. CREATE and append-only actions do not acquire a BaseVersion requirement; quantity-ledger operations are serialized at server transaction level.

`BaseVersion` إلزامي فقط للoptimistic aggregate mutation، وأهمه `UpdateWaybillDraft` وأي UPDATE مماثل. لا يلزم لـappend-only `RecordCollection`, `LoadAllocatedQuantity`, `RecordArrival`, `RecordUnload`, `DeliverQuantity`, `RecordProofOfDelivery`, `CreateShipmentException`; تعتمد هذه الأفعال `ClientOperationId` وserver serialization وdomain state. القيمة الأولية `sync.conflict.auto_merge=false`: لا last-write-wins ولا silent merge.

## 5. Clock and event time

Clients send `ClientOccurredAt`; server records `ServerReceivedAt`. Operational ordering uses server acceptance plus domain OccurredAt policy. Client clocks never control official numbering or accounting period selection.

## 6. Field collections

A collection captured by driver/agent while offline is not financially final merely because the device shows it. It remains queued/pending until server acceptance. Once accepted it is immutable and becomes part of the collector accountability until settlement/remittance.

## 7. POD

Photo/signature/identity capture remains a future client capability where device policy permits. قرار G3 الحالي يجيز metadata/hash/local correlation queue فقط، ولا يجيز نقل binary أو اعتبار proof مكتملًا. لا يفعّل POD/attachment binary runtime قبل عقد resumable upload/hash/size/storage/failure مستقل؛ وبعده فقط يمكن أن تبلغ Delivery حالة خادم نهائية عقب verified upload وserver acknowledgement.

## 8. Movement events

Movement events are append-only. Offline retries must deduplicate by ClientOperationId. A correction is a new reversal/correction event and never an update/delete of an accepted movement.

## 9. Conflict authority

- كل حل يحتاج `sync.conflicts.resolve` مع صلاحية الفعل الأصلي وregistered device ونطاق Company/Branch مطابق وسبب إلزامي؛ لا تكفي `sync.operations.execute` وحدها.
- Draft text/nonfinancial conflicts: designated clerk may `REAPPLY_AS_NEW` after reviewing server state.
- Quantity/custody conflicts: operations supervisor or role configured by policy.
- Collection conflicts: finance/cashier authority.
- Delivery/POD conflicts: delivery supervisor.
- Numbering/approval/posting/reversal/finalization/settlement/financial close/reopen: no offline conflict resolution; repeat online server command.
- `KEEP_SERVER_AND_REJECT_LOCAL`: original SyncOperation تصبح `REJECTED` وConflictCase يصبح `RESOLVED` مع القرار والسبب والمقرر والتوقيت.
- `REAPPLY_AS_NEW`: ينشأ replacement مستقل أولًا بحالة `QUEUED` ويرتبط بـ`ReplacedByOperationId`؛ ثم تصبح original `RESOLVED` بنتيجة `SUPERSEDED` ويصبح ConflictCase `RESOLVED`. يخضع replacement لكل التحققات والـbudgets.
- فشل replacement لاحقًا لا يعيد فتح original أو ConflictCase؛ يظهر failure/conflict مستقلًا مرتبطًا بسلسلة الاستبدال. لا يسمح بـ`USE_DEVICE_OVERWRITE`.

السجل stale وغير النهائي يصبح `CONFLICT`. أما approved/finalized/settled/closed أو invalid transition أو delete ذي تبعيات أو scope mismatch فيصبح `REJECTED`، ولا يُصحح أثر نهائي إلا بأمر Online معتمد.

## 10. Device and scope controls

Every queued item must carry registered DeviceId, UserId, CompanyId, BranchId, `ActionCode`, `ProtocolVersion`, `OperationCorrelationId`, operation type, payload hash, client time, conditional EntityId and BaseVersion according to table 2.1. Every HTTP attempt separately carries `AttemptCorrelationId` in `X-Correlation-Id`; `RequestCorrelationId` is not an accepted JSON field. Server re-evaluates current permission and scope on receipt; prior offline permission does not guarantee acceptance.

لا تكفي claim `device_registered`; يجب أن يتحقق الخادم من سجل جهاز حقيقي ونشط وغير ملغى في كل قبول أو retry أو conflict resolution.

## 11. Retention

| Data | Accepted retention |
|---|---|
| Local `SUCCEEDED` / `RESOLVED` | purge payload after verified server acknowledgement + `24 hours` |
| Local `REJECTED` | retain payload `7 days` then purge; retain metadata/hash/result |
| Local non-terminal | encrypted until terminal; at age `7 days` block new Offline writes and require sync/escalation; never silently delete unacknowledged collection/POD |
| POD/identity binary | غير داخل Sync queue/runtime الحالي؛ بعد عقد resumable upload المستقبلي يحذف محليًا بعد verified upload + `24 hours` ولا يدخل read cache |
| Non-sensitive read cache | maximum `24 hours`; never proof of permission or final state |
| Server PayloadJson/conflict snapshots | purge `90 days` after terminal/resolution; do not purge open conflicts |

تبقى IDs وHashes والحالات والأوقات وAuditEvent وفق سياسة Audit/Legal Hold المنفصلة. يسبق Legal Hold الحذف الخادمي، لكنه لا يجيز الاحتفاظ المحلي غير الضروري.

بعد retention يصبح `PayloadJson=NULL` إن سمح المخطط، أو redacted ثابتًا بلا بيانات أعمال إلى أن تكتمل migration nullable؛ ويبقى PayloadHash/metadata. تطبق القاعدة نفسها على resolved conflict snapshots. يجب أن تثبت اختبارات الحدود عدم تسرب المحتوى المحذوف من API أو audit أو logs.

## 12. Acceptance references

This policy is validated by UAT-P2C01-003, UAT-P2C01-021, UAT-P2C01-030, UAT-P2C01-031 and the W2 action Offline_Policy column.

## 13. Retry and batch decisions

- ميزانية client transport مستقلة: خمس محاولات بعد الأصل، يعدها `ClientTransportRetryCount` في durable client queue.
- ميزانية server business execution مستقلة: خمس محاولات بعد الأصل، يعدها `RetryCount/ServerExecutionRetryCount` في SyncOperation. replay/duplicate enqueue لا يزيدها.
- default الفعال لكلاهما عند base=`5s` هو `5s, 10s, 20s, 40s, 80s` وhard cap `30 minutes`. إذا شدد lower scope base delay، يعاد حساب exponential schedule داخل max؛ فلا يدعى ثبات الجدول بعد override.
- client retryable فقط بعد timeout/no response أو `INTERNAL_ERROR` أو `RATE_LIMITED` مع ClientOperationId/Payload/Hash نفسها. server persisted automatic retry محصور في `RATE_LIMITED` في baseline.
- validation/auth/permission/scope/device/hash/idempotency mismatch/invalid state/business-rule و`CONFLICT` لا تعاد تلقائيًا. الاستنفاد => `REJECTED/RETRY_EXHAUSTED`.
- حجم batch `1..100`. الذرية والنتيجة لكل عملية؛ partial success مسموح، ولا atomic group عابرة للعناصر.

## 14. Settings hierarchy

Global platform policy هو السقف؛ Company ثم Branch يضيّقان فقط؛ Device policy وcurrent permission تقاطع أخير.

- `effectiveAllowedActions = Global ∩ Company ∩ Branch ∩ Device ∩ Permissions`.
- `enabled = AND`.
- client/server max retries، وbatch/cache/retention exposure = `MIN`; client/server base/max delay = `MAX`، ثم يعاد حساب الجدول من effective base.
- غياب lower override يعني fallback `Branch → Company → Global`؛ invalid override يعطل Offline لذلك scope، ولا fallback صامت.
- كل تغيير Online-only وversioned ومدقق مع effective source.

القيم الابتدائية: `sync.offline.enabled=false`, `sync.protocol.allowed_versions=["sync-v1"]`, `sync.retry.client_transport.max_count=5`, `sync.retry.client_transport.base_seconds=5`, `sync.retry.client_transport.max_delay_minutes=30`, `sync.retry.server_execution.max_count=5`, `sync.retry.server_execution.base_seconds=5`, `sync.retry.server_execution.max_delay_minutes=30`, `sync.batch.max_operations=100`, `sync.conflict.auto_merge=false`, `sync.retention.local_success_hours=24`, `sync.retention.local_rejected_days=7`, `sync.retention.server_payload_days=90`, `sync.cache.max_age_hours=24`.

## 15. G4 conformance gate

لا يفعّل Offline قبل إثبات ActionCode و`sync-v1` allowlists قبل enqueue، ورفض unknown/generic DELETE و`ACTION_RUNTIME_UNAVAILABLE`، وسجل جهاز وإلغاءه، وصلاحيات resolver، وقواعد EntityId/ResultEntityId/client↔server map، وBaseVersion للoptimistic actions فقط، وميزانيتي retry المنفصلتين، وworkers/endpoints، وطابور عميل durable ومشفر، وretention cleanup/redaction، وsettings fail-closed.

يجب وجود اختبار allowlist مستقل table-driven **لكل Action** في 2.1، واختبار رفض مستقل لكل runtime-unavailable Action، وإثبات أن read-cache لا ينشئ SyncOperation. كما تنفذ `T-SYNC-001..010` ومراجع UAT، وbatch `0/1/100/101` والنجاح الجزئي، والـreplay دون زيادة server counter، وتسلسلا KEEP_SERVER/REAPPLY، وحدود `24h/7d/90d` وPayload NULL/redaction. يجب أن يرفض Attachment/POD binary حتى يصدر resumable/hash contract ويختبر. كل ذلك على exact SHA مع مراجعة مستقلة.
