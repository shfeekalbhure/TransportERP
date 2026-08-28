# TEAM-B Unknown and Blockers Register

| ID | الموضوع | الحالة الحاكمة | المطلوب للتحقق | الأولوية |
|---|---|---|---|---|
| BLK-B-001 | لا يوجد multi-reviewer فعلي داخل TEAM-B | UNKNOWN — REQUIRES VERIFICATION / governance blocker | تعيين مراجعين مستقلين فعليين وتوقيعات منفصلة على exact SHA | P1 |
| BLK-B-002 | build/test محلي | ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION | بيئة معزولة تحتوي .NET 10 وPostgreSQL 18 وتشغيل كامل موثق | P1 |
| BLK-B-003 | حالة Production DB والمigrations الفعلية | ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION | inventory read-only مع schema/migration hashes دون لمس Production | P1 |
| BLK-B-004 | branch protection وrequired checks | ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION | تصدير rulesets/protection من GitHub | P1 |
| BLK-B-005 | deployment/release environments | ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION | سجل بيئات، artifacts، signing، rollback، backup/restore drill | P1 |
| BLK-B-006 | worktrees/stashes على جهاز المالك | ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION | جرد من كل workspace أصلي مع SHA وstatus | P2 |
| BLK-B-007 | Codex sessions/workspaces غير المنشورة | ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION | سجل مركزي للجلسات والـworkspaces والـunmerged outputs | P2 |
| BLK-B-008 | سبب failure وإصلاح PR #69 | CI FAILURE CONFIRMED؛ root cause/remediation UNKNOWN — REQUIRES VERIFICATION | تحليل sanitized logs، إصلاح Android Release UI E2E/restart، ثم exact-SHA green run ومراجعة مستقلة | P1 |
| BLK-B-009 | dependency vulnerabilities الحالية | UNKNOWN — REQUIRES VERIFICATION | restore مقفل وNuGet audit/SCA/SBOM على exact SHA | P1 |
| BLK-B-010 | privacy legal basis/retention/DSAR/data residency | UNKNOWN — REQUIRES VERIFICATION | سجل بيانات وسياسة معتمدة واختبارات حذف/تقييد/احتفاظ | P1 |
| BLK-B-011 | قدرة backup/restore وRPO/RTO | UNKNOWN — REQUIRES VERIFICATION | استعادة فعلية مع قياسات وevidence | P1 |
| BLK-B-012 | أداء/سعة production-like | UNKNOWN — REQUIRES VERIFICATION | اختبارات حمل وتزامن وفشل شبكة على build قابل للإطلاق | P2 |
