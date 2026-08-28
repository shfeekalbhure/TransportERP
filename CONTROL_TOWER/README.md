# TransportERP — Control Tower

هذه المساحة هي مركز قيادة المهام والفرق لمشروع TransportERP.

- الفرع: `governance/control-tower-20260828`
- الغرض: أوامر الفرق، الحوكمة، سجلات التسليم، التقارير والأدلة.
- ممنوع استخدام هذه المساحة لتعديل Source أو Migrations أو Production DB.
- كل فريق يعمل داخل مجلد مهمته/فريقه ويرفع مخرجاته هناك.
- انتقال المهمة يتم بعد إقفال المخرج السابق وتسجيل التسليم.
- الإجراءات عالية الخطورة (حذف/دمج/نشر/تغيير مدمر للبيانات) لا تنفذ تلقائيًا.

## التشغيل المباشر

- تفويض المالك الحاكم: `00_GOVERNANCE/OWNER_DELEGATION_AND_OPERATING_DIRECTIVE.md`
- الحالة الحية: `00_GOVERNANCE/CONTROL_TOWER_LIVE_STATUS.md`
- أوامر الفرق الحالية: `00_GOVERNANCE/CONTROL_TOWER_TEAM_DIRECTIVES.md`
- قائمة المهام: `00_GOVERNANCE/REGISTERS/CONTROL_TOWER_TASK_QUEUE.md`
- سجل التسليم والختم: `00_GOVERNANCE/REGISTERS/MISSION_HANDOFF_AND_SEAL_REGISTER.md`

## المجموعات

1. `01_GROUP-01_FOUNDATION` — التأسيس والتثبيت.
2. `02_GROUP-02_EXPANSION` — الاستكمال والتوسعة بعد إقفال المجموعة الأولى.
3. `03_DATABASE_GOVERNANCE` — المرجع الحاكم لأي تغيير قاعدة بيانات.
4. `04_CONTROL_TOWER_OPERATIONS` — التوجيه والرقابة والتسليم.
