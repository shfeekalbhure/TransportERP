from pathlib import Path
p=Path(__import__('sys').argv[1])
s=p.read_text()
old=s
s=s.replace('static object IdVal(string provider,string g)=>IdVal(provider,Guid.Parse(g));',
            'static object IdValText(string provider,string g)=>IdVal(provider,Guid.Parse(g));')
s=s.replace('var mode=Arg(args,"--mode","validate-contract");\n// REM-003 governing resolution:',
            'var mode=Arg(args,"--mode","validate-contract");\nvar required=new[]{"W-001","W-014","W-016","W-017","W-018","W-021","W-022","W-028","W-029"};\n// REM-003 governing resolution:')
s=s.replace(' var required=new[]{"W-001","W-014","W-016","W-017","W-018","W-021","W-022","W-028","W-029"};\n','')
for v in ('company','branch'):
    s=s.replace(f'IdVal(provider,{v})',f'IdValText(provider,{v})')
if s==old:
    raise SystemExit('PATCH_NO_CHANGE')
p.write_text(s)
