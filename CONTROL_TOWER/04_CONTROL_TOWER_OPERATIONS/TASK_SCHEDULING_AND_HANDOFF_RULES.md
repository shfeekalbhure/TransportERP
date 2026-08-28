# Task Scheduling and Handoff Rules

1. كل مهمة لها `CURRENT_DIRECTIVE.md` محلي داخل مجلدها الرسمي، بالإضافة إلى حالتها في Control Tower.
2. لا يكفي تغيير `CONTROL_TOWER_TASK_QUEUE.md` وحده لاعتبار المرحلة التالية مشتغلة.
3. عند تحقق prerequisite وختم المخرج السابق، يقوم Control Tower في نفس انتقال الحوكمة بـ:
   - التحقق من التقرير + Evidence + Manifest + SHA-256 + Seal + Handoff؛
   - تحديث `CONTROL_TOWER_TASK_QUEUE.md`؛
   - تحديث `CONTROL_TOWER_LIVE_STATUS.md`؛
   - تحديث `CONTROL_TOWER_TEAM_DIRECTIVES.md`؛
   - وتغيير `CURRENT_DIRECTIVE.md` داخل مجلد المهمة التالية من WAIT إلى START.
4. الحالة `START AUTHORIZED` تعني أن الشروط مكتملة والتوجيه المحلي صار START.
5. الحالة `IN PROGRESS` لا تسجل إلا إذا وُجد دليل أن worker/session بدأ فعليًا وكتب مخرجات داخل مجلد المهمة.
6. إذا كان START مصرحًا لكن لا توجد جلسة عامل نشطة، تسجل الحالة: `START AUTHORIZED — WAITING FOR WORKER SESSION`؛ لا ترجع إلى WAIT ولا تطلب من المالك نسخ التقارير.
7. الفريق المستلم يعيد التحقق من الواقع ولا يعتمد على النقل وحده.
8. كل مخرج يبقى في مجلد الفريق، ثم يسجل في سجل التسليم.
9. بعد الإغلاق يمكن أرشفة نسخة في مكتبة المشروع، مع بقاء النسخة المختومة مرجعًا قابلًا للتتبع.
10. مرجع الانتقال التشغيلي الكامل: `CONTROL_TOWER/04_CONTROL_TOWER_OPERATIONS/CONTINUOUS_MISSION_DISPATCHER.md`.
11. استقلال MISSION-04 إلزامي؛ عامل MISSION-03 لا يصدق على عمله بنفسه.
