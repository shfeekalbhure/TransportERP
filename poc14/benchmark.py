import argparse, json, math, os, statistics, time, uuid
from concurrent.futures import ThreadPoolExecutor, as_completed
from datetime import datetime, timezone


def percentile(values, p):
    if not values: return 0.0
    xs=sorted(values); k=(len(xs)-1)*p; f=math.floor(k); c=math.ceil(k)
    if f==c: return xs[int(k)]
    return xs[f]*(c-k)+xs[c]*(k-f)


def stats(name, values, count=None, extra=None):
    values=list(values)
    elapsed=sum(values)
    d={"workload":name,"operations":count if count is not None else len(values),
       "elapsed_s":round(elapsed,6),"ops_per_s":round((count if count is not None else len(values))/elapsed,3) if elapsed>0 else 0,
       "p50_ms":round(percentile(values,.50)*1000,3),"p95_ms":round(percentile(values,.95)*1000,3),"p99_ms":round(percentile(values,.99)*1000,3)}
    if extra: d.update(extra)
    return d

class DB:
    def __init__(self,args):
        self.engine=args.engine; self.args=args
        if self.engine=='postgres':
            import psycopg
            self.mod=psycopg
        else:
            import mysql.connector
            self.mod=mysql.connector
    def connect(self):
        a=self.args
        if self.engine=='postgres':
            return self.mod.connect(host=a.host,port=a.port,user=a.user,password=a.password,dbname=a.database,autocommit=False)
        return self.mod.connect(host=a.host,port=a.port,user=a.user,password=a.password,database=a.database,autocommit=False)
    def ident(self,g):
        return g if self.engine=='postgres' else g.bytes
    def now(self):
        return datetime.now(timezone.utc) if self.engine=='postgres' else datetime.utcnow()
    def jsonv(self,obj):
        s=json.dumps(obj,separators=(',',':'))
        if self.engine=='postgres':
            from psycopg.types.json import Jsonb
            return Jsonb(obj)
        return s
    def version(self,conn):
        cur=conn.cursor()
        if self.engine=='postgres':
            cur.execute("select version(), postgis_full_version()")
        else:
            cur.execute("select version(), @@version_comment")
        row=cur.fetchone(); cur.close(); return [str(x) for x in row]
    def reset_schema(self,conn):
        c=conn.cursor()
        if self.engine=='postgres':
            c.execute("DROP SCHEMA public CASCADE; CREATE SCHEMA public; CREATE EXTENSION IF NOT EXISTS postgis")
            ddl='''
CREATE TABLE companies(company_id uuid PRIMARY KEY,code varchar(30) UNIQUE NOT NULL,name_ar varchar(200) NOT NULL,version bigint NOT NULL DEFAULT 1);
CREATE TABLE branches(branch_id uuid PRIMARY KEY,company_id uuid NOT NULL REFERENCES companies(company_id),code varchar(30) NOT NULL,name_ar varchar(200) NOT NULL,version bigint NOT NULL DEFAULT 1,UNIQUE(company_id,code));
CREATE TABLE waybills(waybill_id uuid PRIMARY KEY,company_id uuid NOT NULL REFERENCES companies(company_id),origin_branch_id uuid NOT NULL REFERENCES branches(branch_id),destination_branch_id uuid NOT NULL REFERENCES branches(branch_id),business_no varchar(60),status varchar(40) NOT NULL,currency_code char(3) NOT NULL,total_amount numeric(22,6) NOT NULL DEFAULT 0,version bigint NOT NULL DEFAULT 1,attributes jsonb,created_at timestamptz NOT NULL,UNIQUE(company_id,business_no));
CREATE INDEX ix_waybill_scope_status ON waybills(company_id,origin_branch_id,status,created_at DESC);
CREATE TABLE waybill_items(waybill_item_id uuid PRIMARY KEY,waybill_id uuid NOT NULL REFERENCES waybills(waybill_id),item_code varchar(80),original_qty numeric(20,6) NOT NULL,released_qty numeric(20,6) NOT NULL DEFAULT 0,allocated_qty numeric(20,6) NOT NULL DEFAULT 0,loaded_qty numeric(20,6) NOT NULL DEFAULT 0,delivered_qty numeric(20,6) NOT NULL DEFAULT 0,returned_qty numeric(20,6) NOT NULL DEFAULT 0,version bigint NOT NULL DEFAULT 1,CHECK(allocated_qty<=released_qty),CHECK(loaded_qty<=allocated_qty),CHECK(delivered_qty+returned_qty<=loaded_qty));
CREATE TABLE number_reservations(reservation_id uuid PRIMARY KEY,company_id uuid NOT NULL,branch_id uuid,fiscal_key varchar(20),document_type varchar(40) NOT NULL,reserved_number bigint NOT NULL,state varchar(20) NOT NULL,owner_node_id uuid,created_at timestamptz NOT NULL,UNIQUE(company_id,branch_id,fiscal_key,document_type,reserved_number));
CREATE TABLE inbox(inbox_id uuid PRIMARY KEY,operation_scope varchar(80) NOT NULL,idempotency_key varchar(160) NOT NULL,request_hash char(64) NOT NULL,state varchar(20) NOT NULL,result_ref varchar(200),created_at timestamptz NOT NULL,completed_at timestamptz,UNIQUE(operation_scope,idempotency_key));
CREATE TABLE outbox(outbox_id uuid PRIMARY KEY,aggregate_type varchar(80) NOT NULL,aggregate_id uuid NOT NULL,aggregate_version bigint NOT NULL,event_type varchar(100) NOT NULL,payload jsonb NOT NULL,occurred_at timestamptz NOT NULL,created_at timestamptz NOT NULL,dispatched_at timestamptz,attempt_count int NOT NULL DEFAULT 0,UNIQUE(aggregate_type,aggregate_id,aggregate_version,event_type));
CREATE INDEX ix_outbox_pending ON outbox(created_at,outbox_id) WHERE dispatched_at IS NULL;
CREATE TABLE gps_events(event_id uuid PRIMARY KEY,device_id uuid NOT NULL,device_sequence bigint NOT NULL,occurred_at timestamptz NOT NULL,recorded_at timestamptz NOT NULL,received_at timestamptz NOT NULL,position geography(Point,4326) NOT NULL,accuracy_m numeric(10,3),UNIQUE(device_id,device_sequence));
CREATE INDEX ix_gps_position_gist ON gps_events USING gist(position);
'''
        else:
            for t in ['gps_events','outbox','inbox','number_reservations','waybill_items','waybills','branches','companies']:
                c.execute(f"DROP TABLE IF EXISTS {t}")
            ddl='''
CREATE TABLE companies(company_id BINARY(16) PRIMARY KEY,code varchar(30) UNIQUE NOT NULL,name_ar varchar(200) NOT NULL,version bigint NOT NULL DEFAULT 1) ENGINE=InnoDB;
CREATE TABLE branches(branch_id BINARY(16) PRIMARY KEY,company_id BINARY(16) NOT NULL,code varchar(30) NOT NULL,name_ar varchar(200) NOT NULL,version bigint NOT NULL DEFAULT 1,UNIQUE KEY uq_branch(company_id,code),FOREIGN KEY(company_id) REFERENCES companies(company_id)) ENGINE=InnoDB;
CREATE TABLE waybills(waybill_id BINARY(16) PRIMARY KEY,company_id BINARY(16) NOT NULL,origin_branch_id BINARY(16) NOT NULL,destination_branch_id BINARY(16) NOT NULL,business_no varchar(60),status varchar(40) NOT NULL,currency_code char(3) NOT NULL,total_amount decimal(22,6) NOT NULL DEFAULT 0,version bigint NOT NULL DEFAULT 1,attributes json,created_at datetime(6) NOT NULL,UNIQUE KEY uq_wb(company_id,business_no),KEY ix_scope(company_id,origin_branch_id,status,created_at),FOREIGN KEY(company_id) REFERENCES companies(company_id),FOREIGN KEY(origin_branch_id) REFERENCES branches(branch_id),FOREIGN KEY(destination_branch_id) REFERENCES branches(branch_id)) ENGINE=InnoDB;
CREATE TABLE waybill_items(waybill_item_id BINARY(16) PRIMARY KEY,waybill_id BINARY(16) NOT NULL,item_code varchar(80),original_qty decimal(20,6) NOT NULL,released_qty decimal(20,6) NOT NULL DEFAULT 0,allocated_qty decimal(20,6) NOT NULL DEFAULT 0,loaded_qty decimal(20,6) NOT NULL DEFAULT 0,delivered_qty decimal(20,6) NOT NULL DEFAULT 0,returned_qty decimal(20,6) NOT NULL DEFAULT 0,version bigint NOT NULL DEFAULT 1,FOREIGN KEY(waybill_id) REFERENCES waybills(waybill_id),CHECK(allocated_qty<=released_qty),CHECK(loaded_qty<=allocated_qty),CHECK(delivered_qty+returned_qty<=loaded_qty)) ENGINE=InnoDB;
CREATE TABLE number_reservations(reservation_id BINARY(16) PRIMARY KEY,company_id BINARY(16) NOT NULL,branch_id BINARY(16),fiscal_key varchar(20),document_type varchar(40) NOT NULL,reserved_number bigint NOT NULL,state varchar(20) NOT NULL,owner_node_id BINARY(16),created_at datetime(6) NOT NULL,UNIQUE KEY uq_num(company_id,branch_id,fiscal_key,document_type,reserved_number)) ENGINE=InnoDB;
CREATE TABLE inbox(inbox_id BINARY(16) PRIMARY KEY,operation_scope varchar(80) NOT NULL,idempotency_key varchar(160) NOT NULL,request_hash char(64) NOT NULL,state varchar(20) NOT NULL,result_ref varchar(200),created_at datetime(6) NOT NULL,completed_at datetime(6),UNIQUE KEY uq_inbox(operation_scope,idempotency_key)) ENGINE=InnoDB;
CREATE TABLE outbox(outbox_id BINARY(16) PRIMARY KEY,aggregate_type varchar(80) NOT NULL,aggregate_id BINARY(16) NOT NULL,aggregate_version bigint NOT NULL,event_type varchar(100) NOT NULL,payload json NOT NULL,occurred_at datetime(6) NOT NULL,created_at datetime(6) NOT NULL,dispatched_at datetime(6),attempt_count int NOT NULL DEFAULT 0,UNIQUE KEY uq_out(aggregate_type,aggregate_id,aggregate_version,event_type),KEY ix_out(dispatched_at,created_at,outbox_id)) ENGINE=InnoDB;
CREATE TABLE gps_events(event_id BINARY(16) PRIMARY KEY,device_id BINARY(16) NOT NULL,device_sequence bigint NOT NULL,occurred_at datetime(6) NOT NULL,recorded_at datetime(6) NOT NULL,received_at datetime(6) NOT NULL,position POINT SRID 4326 NOT NULL,accuracy_m decimal(10,3),UNIQUE KEY uq_gps(device_id,device_sequence),SPATIAL INDEX ix_gps(position)) ENGINE=InnoDB;
'''
        for statement in [x.strip() for x in ddl.split(';') if x.strip()]: c.execute(statement)
        conn.commit(); c.close()

def timed(fn):
    t=time.perf_counter(); fn(); return time.perf_counter()-t

def run(args):
    db=DB(args); conn=db.connect(); db.reset_schema(conn); versions=db.version(conn)
    c=conn.cursor(); company=uuid.uuid4(); b1=uuid.uuid4(); b2=uuid.uuid4()
    c.execute("INSERT INTO companies(company_id,code,name_ar) VALUES(%s,%s,%s)",(db.ident(company),'C1','شركة'))
    c.execute("INSERT INTO branches(branch_id,company_id,code,name_ar) VALUES(%s,%s,%s,%s)",(db.ident(b1),db.ident(company),'B1','فرع 1'))
    c.execute("INSERT INTO branches(branch_id,company_id,code,name_ar) VALUES(%s,%s,%s,%s)",(db.ident(b2),db.ident(company),'B2','فرع 2')); conn.commit()
    results=[]; integrity={}

    # W-001 Waybill + 3 items atomically
    lat=[]; waybill_ids=[]
    for i in range(1000):
        wid=uuid.uuid4(); waybill_ids.append(wid)
        def op(i=i,wid=wid):
            cc=conn.cursor();
            try:
                cc.execute("INSERT INTO waybills(waybill_id,company_id,origin_branch_id,destination_branch_id,business_no,status,currency_code,total_amount,attributes,created_at) VALUES(%s,%s,%s,%s,%s,'CREATED','YER',100,%s,%s)",(db.ident(wid),db.ident(company),db.ident(b1),db.ident(b2),f'WB-{i}',db.jsonv({'priority':i%3}),db.now()))
                for j in range(3):
                    cc.execute("INSERT INTO waybill_items(waybill_item_id,waybill_id,item_code,original_qty,released_qty) VALUES(%s,%s,%s,10,10)",(db.ident(uuid.uuid4()),db.ident(wid),f'I{j}'))
                conn.commit()
            except: conn.rollback(); raise
            finally: cc.close()
        lat.append(timed(op))
    results.append(stats('W-001',lat))
    c=conn.cursor(); c.execute("SELECT count(*) FROM waybills"); wbc=c.fetchone()[0]; c.execute("SELECT count(*) FROM waybill_items"); itc=c.fetchone()[0]; integrity['W-001']=(wbc==1000 and itc==3000); c.close()

    # W-014 scoped lookup
    lat=[]
    for i in range(2000):
        def op():
            cc=conn.cursor(); cc.execute("SELECT waybill_id,business_no,status FROM waybills WHERE company_id=%s AND origin_branch_id=%s AND status='CREATED' ORDER BY created_at DESC LIMIT 50",(db.ident(company),db.ident(b1))); cc.fetchall(); cc.close()
        lat.append(timed(op))
    results.append(stats('W-014',lat)); integrity['W-014']=True

    # W-016 numbering uniqueness pressure
    lat=[]; duplicate_rejected=False
    for i in range(3000):
        rid=uuid.uuid4()
        def op(i=i,rid=rid):
            cc=conn.cursor(); cc.execute("INSERT INTO number_reservations(reservation_id,company_id,branch_id,fiscal_key,document_type,reserved_number,state,created_at) VALUES(%s,%s,%s,'2026','WAYBILL',%s,'RESERVED',%s)",(db.ident(rid),db.ident(company),db.ident(b1),i+1,db.now())); conn.commit(); cc.close()
        lat.append(timed(op))
    try:
        cc=conn.cursor(); cc.execute("INSERT INTO number_reservations(reservation_id,company_id,branch_id,fiscal_key,document_type,reserved_number,state,created_at) VALUES(%s,%s,%s,'2026','WAYBILL',1,'RESERVED',%s)",(db.ident(uuid.uuid4()),db.ident(company),db.ident(b1),db.now())); conn.commit(); cc.close()
    except Exception:
        conn.rollback(); duplicate_rejected=True
    results.append(stats('W-016',lat,extra={'duplicate_rejected':duplicate_rejected})); integrity['W-016']=duplicate_rejected

    # W-017 transactional outbox insert
    lat=[]
    for i in range(3000):
        oid=uuid.uuid4(); agg=waybill_ids[i%len(waybill_ids)]
        def op(i=i,oid=oid,agg=agg):
            cc=conn.cursor(); cc.execute("INSERT INTO outbox(outbox_id,aggregate_type,aggregate_id,aggregate_version,event_type,payload,occurred_at,created_at) VALUES(%s,'Waybill',%s,%s,'WaybillTouched',%s,%s,%s)",(db.ident(oid),db.ident(agg),i+1,db.jsonv({'i':i}),db.now(),db.now())); conn.commit(); cc.close()
        lat.append(timed(op))
    results.append(stats('W-017',lat)); c=conn.cursor(); c.execute("select count(*) from outbox"); integrity['W-017']=c.fetchone()[0]==3000; c.close()

    # W-018 inbox/idempotency duplicate replay
    lat=[]; dup=0
    for i in range(2500):
        iid=uuid.uuid4(); key=f'K-{i}'
        def op(iid=iid,key=key):
            cc=conn.cursor(); cc.execute("INSERT INTO inbox(inbox_id,operation_scope,idempotency_key,request_hash,state,created_at) VALUES(%s,'collection',%s,%s,'COMPLETED',%s)",(db.ident(iid),key,'a'*64,db.now())); conn.commit(); cc.close()
        lat.append(timed(op))
    for i in range(250):
        try:
            cc=conn.cursor(); cc.execute("INSERT INTO inbox(inbox_id,operation_scope,idempotency_key,request_hash,state,created_at) VALUES(%s,'collection',%s,%s,'COMPLETED',%s)",(db.ident(uuid.uuid4()),f'K-{i}','a'*64,db.now())); conn.commit(); cc.close()
        except Exception:
            conn.rollback(); dup+=1
    results.append(stats('W-018',lat,extra={'duplicate_replays_rejected':dup})); integrity['W-018']=dup==250

    # W-021 GPS append
    device=uuid.uuid4(); lat=[]
    for i in range(5000):
        eid=uuid.uuid4(); lon=45.0+(i%100)*.0001; la=12.8+(i%100)*.0001
        def op(i=i,eid=eid,lon=lon,la=la):
            cc=conn.cursor()
            if db.engine=='postgres':
                cc.execute("INSERT INTO gps_events(event_id,device_id,device_sequence,occurred_at,recorded_at,received_at,position,accuracy_m) VALUES(%s,%s,%s,%s,%s,%s,ST_SetSRID(ST_MakePoint(%s,%s),4326)::geography,5)",(db.ident(eid),db.ident(device),i+1,db.now(),db.now(),db.now(),lon,la))
            else:
                cc.execute("INSERT INTO gps_events(event_id,device_id,device_sequence,occurred_at,recorded_at,received_at,position,accuracy_m) VALUES(%s,%s,%s,%s,%s,%s,ST_SRID(POINT(%s,%s),4326),5)",(db.ident(eid),db.ident(device),i+1,db.now(),db.now(),db.now(),lon,la))
            conn.commit(); cc.close()
        lat.append(timed(op))
    results.append(stats('W-021',lat)); c=conn.cursor(); c.execute("select count(*) from gps_events"); integrity['W-021']=c.fetchone()[0]==5000; c.close()

    # W-022 nearby spatial query
    lat=[]; nonempty=0
    for _ in range(200):
        def op():
            nonlocal nonempty
            cc=conn.cursor()
            if db.engine=='postgres': cc.execute("SELECT count(*) FROM gps_events WHERE ST_DWithin(position,ST_SetSRID(ST_MakePoint(45.005,12.805),4326)::geography,5000)")
            else: cc.execute("SELECT count(*) FROM gps_events WHERE ST_Distance_Sphere(position,ST_SRID(POINT(45.005,12.805),4326))<=5000")
            if cc.fetchone()[0]>0: nonempty+=1
            cc.close()
        lat.append(timed(op))
    results.append(stats('W-022',lat)); integrity['W-022']=nonempty==200

    # W-028 expected-version optimistic updates
    target=waybill_ids[0]; lat=[]; conflicts=0
    version=1
    for i in range(500):
        def op(version=version):
            cc=conn.cursor(); cc.execute("UPDATE waybills SET version=version+1 WHERE waybill_id=%s AND version=%s",(db.ident(target),version)); n=cc.rowcount; conn.commit(); cc.close(); return n
        t=time.perf_counter(); n=op(); lat.append(time.perf_counter()-t)
        if n!=1: conflicts+=1
        else: version+=1
    cc=conn.cursor(); cc.execute("UPDATE waybills SET status='STALE-WRITE' WHERE waybill_id=%s AND version=1",(db.ident(target),)); stale_n=cc.rowcount; conn.commit(); cc.close()
    results.append(stats('W-028',lat,extra={'unexpected_conflicts':conflicts,'stale_update_rows':stale_n})); integrity['W-028']=(conflicts==0 and stale_n==0)

    # W-029 concurrent short queries; one connection per task
    def qtask(_):
        cn=db.connect(); cc=cn.cursor(); t=time.perf_counter(); cc.execute("SELECT count(*) FROM waybills WHERE company_id=%s",(db.ident(company),)); cc.fetchone(); elapsed=time.perf_counter()-t; cc.close(); cn.close(); return elapsed
    t0=time.perf_counter(); lats=[]
    with ThreadPoolExecutor(max_workers=100) as ex:
        for v in ex.map(qtask, range(1000)): lats.append(v)
    wall=time.perf_counter()-t0
    r=stats('W-029',lats,count=1000,extra={'wall_s':round(wall,6),'wall_ops_per_s':round(1000/wall,3)}); results.append(r); integrity['W-029']=True

    all_ok=all(integrity.values())
    c=conn.cursor();
    if db.engine=='postgres': c.execute("select pg_database_size(current_database())")
    else: c.execute("select coalesce(sum(data_length+index_length),0) from information_schema.tables where table_schema=database()")
    db_bytes=int(c.fetchone()[0]); c.close(); conn.close()
    return {'engine':args.engine,'run_id':args.run_id,'versions':versions,'database_bytes':db_bytes,'integrity':integrity,'mandatory_subset_integrity_pass':all_ok,'results':results}

if __name__=='__main__':
    p=argparse.ArgumentParser(); p.add_argument('--engine',choices=['postgres','mysql'],required=True); p.add_argument('--host',default='127.0.0.1'); p.add_argument('--port',type=int,required=True); p.add_argument('--user',required=True); p.add_argument('--password',required=True); p.add_argument('--database',required=True); p.add_argument('--run-id',required=True); p.add_argument('--output',required=True); a=p.parse_args()
    result=run(a)
    with open(a.output,'w') as f: json.dump(result,f,indent=2,default=str)
    print(json.dumps(result,indent=2,default=str))
