using System.Data.Common;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using Npgsql;

var engine = Environment.GetEnvironmentVariable("ENGINE") ?? throw new Exception("ENGINE missing");
var dbcs = Environment.GetEnvironmentVariable("DBCS") ?? throw new Exception("DBCS missing");
var localPath = $"/tmp/wave02-{engine}.db";
if (File.Exists(localPath)) File.Delete(localPath);

DbConnection OpenDb() => engine == "postgres" ? new NpgsqlConnection(dbcs) : new MySqlConnection(dbcs);
DbParameter Param(DbCommand c,string n,object v){var p=c.CreateParameter();p.ParameterName=n;p.Value=v;return p;}
async Task Exec(DbConnection cn,DbTransaction? tx,string sql,params (string,object)[] ps){await using var c=cn.CreateCommand();c.Transaction=tx;c.CommandText=sql;foreach(var x in ps)c.Parameters.Add(Param(c,x.Item1,x.Item2));await c.ExecuteNonQueryAsync();}

await using(var cn=OpenDb()){
  await cn.OpenAsync();
  var ddl=engine=="postgres"
    ? "DROP TABLE IF EXISTS outbox;DROP TABLE IF EXISTS inbox;DROP TABLE IF EXISTS effects;CREATE TABLE effects(effect_id varchar(64) primary key,event_id varchar(64) unique not null,amount numeric(22,6) not null);CREATE TABLE inbox(idempotency_key varchar(100) primary key,request_hash char(64) not null,effect_id varchar(64) not null);CREATE TABLE outbox(outbox_id varchar(64) primary key,effect_id varchar(64) not null);"
    : "DROP TABLE IF EXISTS outbox;DROP TABLE IF EXISTS inbox;DROP TABLE IF EXISTS effects;CREATE TABLE effects(effect_id varchar(64) primary key,event_id varchar(64) unique not null,amount decimal(22,6) not null) ENGINE=InnoDB;CREATE TABLE inbox(idempotency_key varchar(100) primary key,request_hash char(64) not null,effect_id varchar(64) not null) ENGINE=InnoDB;CREATE TABLE outbox(outbox_id varchar(64) primary key,effect_id varchar(64) not null) ENGINE=InnoDB;";
  foreach(var s in ddl.Split(';',StringSplitOptions.RemoveEmptyEntries)) await Exec(cn,null,s);
}

var builder=WebApplication.CreateBuilder(args);builder.WebHost.UseUrls("http://127.0.0.1:5080");var app=builder.Build();
app.MapGet("/health",()=>new{ok=true,engine});
app.MapGet("/count",async()=>{await using var cn=OpenDb();await cn.OpenAsync();await using var c=cn.CreateCommand();c.CommandText="select count(*) from effects";return new{count=Convert.ToInt64(await c.ExecuteScalarAsync())};});
app.MapPost("/command",async(CommandDto d)=>{
  await using var cn=OpenDb();await cn.OpenAsync();await using var tx=await cn.BeginTransactionAsync();
  await using var q=cn.CreateCommand();q.Transaction=tx;q.CommandText="select request_hash,effect_id from inbox where idempotency_key=@k";q.Parameters.Add(Param(q,"@k",d.IdempotencyKey));
  await using var r=await q.ExecuteReaderAsync();
  if(await r.ReadAsync()){
    var h=r.GetString(0);var id=r.GetString(1);await r.DisposeAsync();
    if(h!=d.RequestHash){await tx.RollbackAsync();return Results.Conflict(new{code="IDEMPOTENCY_CONFLICT"});}
    await tx.CommitAsync();return Results.Ok(new CommandResult(id,true));
  }
  await r.DisposeAsync();
  var effect=Guid.NewGuid().ToString("N");
  await Exec(cn,tx,"insert into effects(effect_id,event_id,amount) values(@e,@v,@a)",("@e",effect),("@v",d.EventId),("@a",d.Amount));
  await Exec(cn,tx,"insert into outbox(outbox_id,effect_id) values(@o,@e)",("@o",Guid.NewGuid().ToString("N")),("@e",effect));
  await Exec(cn,tx,"insert into inbox(idempotency_key,request_hash,effect_id) values(@k,@h,@e)",("@k",d.IdempotencyKey),("@h",d.RequestHash),("@e",effect));
  await tx.CommitAsync();return Results.Ok(new CommandResult(effect,false));
});

await app.StartAsync();
using var http=new HttpClient{BaseAddress=new Uri("http://127.0.0.1:5080")};
await using var local=new SqliteConnection($"Data Source={localPath}");await local.OpenAsync();
await using(var c=local.CreateCommand()){c.CommandText="create table queue(event_id text primary key,idempotency_key text unique not null,request_hash text not null,amount real not null,state text not null)";await c.ExecuteNonQueryAsync();}
for(int i=0;i<500;i++){
  var eventId=Guid.NewGuid().ToString("N");var key=$"K-{i:D4}";var hash=Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(eventId))).ToLowerInvariant();
  await using var c=local.CreateCommand();c.CommandText="insert into queue values($e,$k,$h,$a,'PENDING')";c.Parameters.AddWithValue("$e",eventId);c.Parameters.AddWithValue("$k",key);c.Parameters.AddWithValue("$h",hash);c.Parameters.AddWithValue("$a",i+1);await c.ExecuteNonQueryAsync();
}
var rows=new List<CommandDto>();await using(var c=local.CreateCommand()){c.CommandText="select event_id,idempotency_key,request_hash,amount from queue order by idempotency_key";await using var r=await c.ExecuteReaderAsync();while(await r.ReadAsync())rows.Add(new(r.GetString(0),r.GetString(1),r.GetString(2),r.GetDecimal(3)));}
int replay=0;for(int i=0;i<rows.Count;i++){
  var d=rows[i];var resp=await http.PostAsJsonAsync("/command",d);resp.EnsureSuccessStatusCode();
  if(i%20==0){var dup=await http.PostAsJsonAsync("/command",d);dup.EnsureSuccessStatusCode();var rr=await dup.Content.ReadFromJsonAsync<CommandResult>();if(rr?.IdempotentReplay==true)replay++;}
  await using var c=local.CreateCommand();c.CommandText="update queue set state='ACCEPTED' where event_id=$e";c.Parameters.AddWithValue("$e",d.EventId);await c.ExecuteNonQueryAsync();
}
long pending;await using(var c=local.CreateCommand()){c.CommandText="select count(*) from queue where state<>'ACCEPTED'";pending=Convert.ToInt64(await c.ExecuteScalarAsync());}
var count=await http.GetFromJsonAsync<CountResult>("/count");var pass=pending==0&&count?.Count==500&&replay==25;
Console.WriteLine(JsonSerializer.Serialize(new{engine,localDurable=500,centralEffects=count?.Count,pending,duplicateReplays=replay,pass}));
await app.StopAsync();if(!pass)Environment.ExitCode=2;

record CommandDto(string EventId,string IdempotencyKey,string RequestHash,decimal Amount);
record CommandResult(string EffectId,bool IdempotentReplay);
record CountResult(long Count);
