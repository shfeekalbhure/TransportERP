# TransportERP — P2-C01 Owner Scope Decision

**التاريخ:** 2026-08-20 UTC+3  
**الحالة:** `SELECTED_FOR_CONTRACTING`  
**Baseline after W0-2B:** `master@545d9ed8f0859e313c78ef13f6ef6edbdeb3c11c`

## 1. قرار المالك

يتم اعتماد `P2-C01 — البوالص والشحن` كأول مجال تشغيلي بعد P1 ضمن مسار تطوير كراسة 8-8 الحالية.

لا يعني هذا القرار تفويض إنشاء Physical Schema أو Migration أو شاشات نهائية قبل إغلاق W1/W2/W3 واختبارات القبول الخاصة بالمجال.

## 2. داخل النطاق

- مسودات البوليصة المتعددة والعمل المتوازي.
- المرسل والمستلم والدافع وشاشة الزبائن التشغيلية.
- الأصناف والطرود والأوزان والأبعاد والقيم والمخاطر.
- الاعتماد والترقيم الخادمي الذري.
- خطة الدفع والتحصيل وحالة السداد.
- Release / Allocation / Trip / Manifest / Load.
- حركة البوليصة والصنف والطرد.
- الوصول الجزئي والترانزيت والمخزن.
- التسليم الجزئي والكامل وPOD وCOD.
- عمولات السائق والمركبة من كشف الرحلة.
- الإغلاق المالي وربط FIN.
- بيانات الاستعداد الجمركي والتكامل مع CUS.

## 3. خارج التنفيذ المباشر في P2-C01

- منصة GPS/Telematics الكاملة.
- نظام التذاكر والمسافرين.
- منصة Last-Mile المستقلة.
- نظام الصيانة الكامل.
- الموارد البشرية والرواتب.

تظل هذه المجالات نقاط تكامل وتصميم مستقبلي ولا تدمج في PRs الخاصة بـP2-C01.

## 4. تبعيات P1/W0-2B المعتمدة

P2-C01 يرث ولا يعيد بناء: Company, Branch, Currency, Accounting primitives, Audit, Sync/Idempotency, JWT/Scope patterns، والعقود المشتركة المسترجعة في W0-2B مثل OperationContext وGeo Contracts وNumberReservation taxonomy.

## 5. Offline / Online

- Draft Waybill: يسمح Offline وفق سياسة الجهاز.
- Approval / Official Numbering: Server authoritative / Online.
- Field Movement/POD: يمكن Queue عند التفويض مع ClientOperationId ثم Server Ack.
- Financial Close/Reopen: Online only.

## 6. قاعدة الانتقال الصارمة

لا تبدأ أي مرحلة لاحقة قبل إغلاق المرحلة السابقة بالكامل بالأدلة. ترتيب البوابات الملزم: `W0-2B CLOSED -> W0-3 CLOSED -> W0-5 CLOSED -> P2-C01-A ...`. لا يجوز تنفيذ Physical Schema أو Migration أو Production UI لـP2-C01 أثناء W0-3.
