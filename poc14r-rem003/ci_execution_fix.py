from pathlib import Path
import sys
p=Path(sys.argv[1])
lines=p.read_text().splitlines()
needle='for lane in DEEP_POSTGRES DEEP_MYSQL POSTGRES_EF MYSQL_ORACLE_EF MYSQL_POMELO_CONTINUITY PROVIDER_ADO; do'
start=lines.index(needle)
end=next(i for i in range(start+1,len(lines)) if lines[i]=='done')
new='''for lane in DEEP_POSTGRES DEEP_MYSQL POSTGRES_EF MYSQL_ORACLE_EF MYSQL_POMELO_CONTINUITY PROVIDER_ADO; do
 dir="$EV/07_BUILD_OUTPUTS/$lane"
 case "$lane" in
  DEEP_POSTGRES) dll="DeepPostgres.dll"; expected="POC14R_PG_PREFLIGHT";;
  DEEP_MYSQL) dll="DeepMySql.dll"; expected="POC14R_MY_PREFLIGHT";;
  POSTGRES_EF) dll="Poc14.PostgresEf.dll"; expected="POC14_PG";;
  MYSQL_ORACLE_EF) dll="Poc14.MySqlOracleEf.dll"; expected="POC14_MYSQL";;
  MYSQL_POMELO_CONTINUITY) dll="Poc14.MySqlPomeloEf.dll"; expected="POC14_MYSQL";;
  PROVIDER_ADO) dll="Poc14.ProviderAdo.dll"; expected="POC14R_PG";;
 esac
 [[ -f "$dir/$dll" ]] || { echo "PRIMARY_BINARY_MISSING $lane $dll"; exit 120; }
 set +e; out=$(env -u POC14R_PG_PREFLIGHT -u POC14R_MY_PREFLIGHT -u POC14_PG -u POC14_MYSQL -u POC14R_PG -u POC14R_MYSQL dotnet "$dir/$dll" 2>&1); rc=$?; set -e
 status=FAIL; [[ $rc -ne 0 && "$out" == *"$expected"* ]] && status=PASS
 printf '%s,NON_LIVE_PRIMARY_BINARY_EXECUTION,ENVIRONMENT_REQUIRED_REFUSAL,%d,"%s",%s\n' "$lane" "$rc" "$(echo "$out"|head -4|tr '\n' ' '|sed 's/,/;/g;s/"/'"'"'/g')" "$status" >> "$EV/12_NONLIVE_EXECUTION_RECEIPTS.csv"
 [[ "$status" == PASS ]]
done'''.splitlines()
lines[start:end+1]=new
p.write_text('\n'.join(lines)+'\n')
