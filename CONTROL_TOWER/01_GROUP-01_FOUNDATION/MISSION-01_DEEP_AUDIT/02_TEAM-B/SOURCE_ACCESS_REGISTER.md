# TEAM-B Source Access Register

| Source ID | المصدر | طريقة الوصول | النطاق/الإصدار | النتيجة |
|---|---|---|---|---|
| SRC-B-001 | مستودع Git المحلي المعزول | قراءة فقط | governance/control-tower-20260828@8a36f88b56a43cd5b47277b645ba2030ed3da4f1 | ACCESS OK |
| SRC-B-002 | مرآة Git مستقلة مؤقتة | clone --mirror وقراءة refs فقط | 50 branch، 0 tag | ACCESS OK |
| SRC-B-003 | GitHub repository metadata | موصل GitHub مصادق، قراءة فقط | shfeekalbhure/TransportERP | ACCESS OK |
| SRC-B-004 | GitHub PRs | بحث وقراءة metadata | 10 PRs مفتوحة وقت اللقطة؛ #69 draft | ACCESS OK |
| SRC-B-005 | GitHub Actions | قراءة runs/jobs/artifacts | master run 32867082533 وPR #69 exact head | ACCESS OK محدود |
| SRC-B-006 | GitHub branch-protection/rulesets | لا توجد واجهة قراءة متاحة في الموصل المستخدم | master | ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION |
| SRC-B-007 | Kurrasa في ChatGPT Library | Library native read/find | الملف الرسمي version 72، 783 سطرًا | ACCESS OK |
| SRC-B-008 | Production database | غير مصرح وغير متصل | جميع البيئات | ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION |
| SRC-B-009 | Deployment/runtime environments | لا موصل ولا سجل بيئة | dev/stage/prod | ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION |
| SRC-B-010 | Codex workspaces/sessions خارج هذه الجلسة | لا واجهة جرد عامة متاحة | أعمال محلية غير منشورة | ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION |
| SRC-B-011 | Worktrees/stashes في جهاز المالك الأصلي | audit clone فقط متاح | خارج clone الحالي | ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION |
| SRC-B-012 | TEAM-A artifacts | محظورة بموجب أمر الاستقلال | كامل نطاق TEAM-A | NOT ACCESSED BY DESIGN |
| SRC-B-013 | أداة .NET المحلية | command not found | build/test/migrations runtime | ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION |

