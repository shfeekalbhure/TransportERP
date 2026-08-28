# Control Tower Automation Policy

يُسمح بمهمة رقابية دورية للتحقق من `CONTROL_TOWER_TASK_QUEUE.md` ومن شروط الختم والتسليم.

أثناء بقاء جلسة Control Tower نشطة وقادرة على المتابعة، تكون دورة الفحص كل `10 دقائق` وتشمل مجلدات الفرق، التقارير الجديدة، Evidence/Seal/Handoff registers، Task Queue، Blockers، وحالات المهام. إذا لم يتغير شيء فلا تعدل السجلات شكليًا ولا تنشئ قرارًا أو تنبيهًا وهميًا.

قبل توقف المتابعة النشطة، يحدث `CONTROL_TOWER_LIVE_STATUS.md` بقيم `LAST VERIFIED CHECK` و`NEXT PLANNED CHECK` و`MONITORING STATE`. عند تعذر الاستمرار تسجل `MONITORING PAUSED — REQUIRES RESUME`، ولا يدعى أن الفحص مستمر.

المهمة الدورية لا تعتبر فريقًا فنيًا ولا تنشئ حقائق أو نتائج من تلقاء نفسها. عند تحقق شرط انتقال موثق، تحدث حالة التوجيه/التسليم داخل مساحة Control Tower فقط. إذا ظهر مانع أو خطر عالٍ، تسجل HOLD وترفع تنبيهًا بدل تجاوز القيد.
