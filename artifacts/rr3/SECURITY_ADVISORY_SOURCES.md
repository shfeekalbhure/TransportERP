# مصادر RR2-C-07 — تنبيهات NuGet

تاريخ الفحص: 2026-08-19

## Microsoft.OpenApi

المصدر: https://github.com/advisories/GHSA-v5pm-xwqc-g5wc

العنوان: Microsoft.OpenAPI: Circular schema references may terminate OpenAPI parsing (CVE-2026-49451).

النطاق المتأثر: Microsoft.OpenApi من 2.0.0-preview.11 إلى 2.7.4.

الإصدارات المصححة: 2.7.5 وما فوق في خط 2.x، أو 3.5.4 وما فوق في خط 3.x.

النسخة الحالية في TransportERP: Microsoft.OpenApi 2.0.0، تأتي بصورة انتقالية من Microsoft.AspNetCore.OpenApi 10.0.10.

## System.Security.Cryptography.Xml

المصدر: https://advisories.gitlab.com/nuget/system.security.cryptography.xml/CVE-2026-47304/

المصدر الإضافي: https://github.com/advisories/GHSA-cvvh-rhrc-wg4q

النطاق المتأثر في .NET 10: الإصدارات 10.0.0 إلى 10.0.9.

الإصدار المصحح: 10.0.10.

النسخة التي تظهر في Restore graph: 9.0.0، وهي مسحوبة انتقالياً من Microsoft.Build.Tasks.Core ضمن اعتماديات design/build، وليست مرجعاً صريحاً في TransportERP.Infrastructure.csproj.

## الإجراء المقترح

تثبيت Microsoft.OpenApi >= 2.7.5 كمرجع مباشر متوافق مع خط OpenAPI 2.x، وتثبيت System.Security.Cryptography.Xml >= 10.0.10 كمرجع مباشر/تقييد مركزي إذا قبل توافق .NET 10، ثم تشغيل restore وdotnet list package --vulnerable --include-transitive. إذا تعارضت حزمة design-time مع 10.x، يجب توثيق تعارض الإصدار وقرار أمني منفصل بدل إخفاء التحذير.

لا يعتبر RR2-C-07 مغلقاً حتى يثبت سجل NuGet عدم وجود التنبيه أو يصدر المالك استثناءً أمنياً صريحاً ومحدداً النطاق.

## ملاحظة عن الأدلة المحلية

في الفحص المحلي قبل المعالجة:
- Microsoft.OpenApi 2.0.0: High، GHSA-v5pm-xwqc-g5wc.
- System.Security.Cryptography.Xml 9.0.0: عدة تنبيهات High، منها GHSA-g8r8-53c2-pm3f وGHSA-cvvh-rhrc-wg4q.

## مصادر إضافية

- https://learn.microsoft.com/en-us/nuget/concepts/auditing-packages
- https://nvd.nist.gov/vuln/detail/CVE-2026-49451
- https://msrc.microsoft.com/update-guide/vulnerability/CVE-2026-47304

هذه المذكرة توثق مصادر خارجية فقط، ولا تعد قراراً بترقية الاعتماديات قبل نجاح البناء والاختبارات.
