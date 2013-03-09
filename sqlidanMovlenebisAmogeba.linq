<Query Kind="Program">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAAAD89x4SL38S/4r7NUU2iHNNTcmnVTi1xQPx1AC1vAtFAAAAAAOgAAAAAIAACAAAABgO+xVBio0IKzceIXWbWfgFv0jcxQpOA9YilhDtPA8XxAAAABJcM5+MLInsGd5jUUGfXtnQAAAALHy7sVof5cKLhfpSHxbLdPESALwKOWLElOgeYcZmaeqO1sDG9SHnPN5xOzSlBHlSD7agk+KC9dH/4XMZn4gYvM=</Password>
    <Database>SocialuriDazgveva</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>D:\Dev\EventStore\bin\eventstore\debug\anycpu\EventStore.ClientAPI.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.Tasks.dll</Reference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Dynamic</Namespace>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
  <Namespace>System.Reactive.Disposables</Namespace>
  <Namespace>System.Reactive.Joins</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Reactive.PlatformServices</Namespace>
  <Namespace>System.Reactive.Subjects</Namespace>
  <Namespace>System.Reactive.Threading.Tasks</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>EventStore.ClientAPI</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

IObservable<JObject> Moaselekte(string sql)
{
    return Observable.Create<JObject>(async o=>{
            var disposed=false;
            try
            {	        
                var cmd = new SqlCommand(sql,(SqlConnection)Connection);
                var reader = await cmd.ExecuteReaderAsync();
                while(await reader.ReadAsync() && !disposed)
                {
                    var obj = new JObject();
                    for (int i = 0; i < reader.FieldCount; i++)
                        obj.Add(reader.GetName(i), JToken.FromObject(reader[i]));
                    o.OnNext(obj);
                }
            }
            catch (Exception ex)
            {
                o.OnError(ex);        
            }
            o.OnCompleted();
            return () => {disposed=true;};
        });
}

EventStore.ClientAPI.EventStoreConnection ConnectoToEs()
{
    var con = EventStore.ClientAPI.EventStoreConnection.Create();
    con.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1113));
    return con;
}
    
void SheinakheMovlenataSacavshi(string streamName,string eventId,string eventType,string body){

//new {streamName,eventId,eventType,body}.Dump();
}
static Guid ToGuid( string input)
{
    using(var provider = new MD5CryptoServiceProvider())
    {
        byte[] inputBytes = Encoding.Default.GetBytes(input);
        byte[] hashBytes = provider.ComputeHash(inputBytes);
        var hashGuid = new Guid(hashBytes);
        return hashGuid;
    }
}

void Main()
{
    Connection.Open();

    var movlenisTipebi = new [] {
        new { 
                Sakheli = "Polisi", 
                Sql = "select * from SocialuriDazgveva..Polisebi ",
                MomeId = (Func<JObject,string>)((o) => (string)o.GetValue("PolisisNomeri").Value<string>()),
                MomeUpdateSqli = (Func<string,string>)(o => "update SocialuriDazgveva..MovlenaChabarebebi set GagzavnisDro=getdate() where PolisisNomeri='" + o + "'")
            }
        };
    
    foreach (var movlenaTipi in movlenisTipebi)
    {
        Observable
            .Return(ConnectoToEs())
            .SelectMany(c => Moaselekte(movlenaTipi.Sql)
                                    .Buffer( 1024 )
                                    .Select(buf => new {c, buf}))
            .Subscribe(x => {
                    var events = x.buf.Select (e => new EventData(ToGuid(movlenaTipi.MomeId(e)), 
                                                                  movlenaTipi.Sakheli, 
                                                                  true, 
                                                                  Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(e)), 
                                                                  null));
                    //var id = movlenaTipi.MomeId(x.jo);
                    x.c.AppendToStream("mainstream", -2, events);
                    //var uSql = movlenaTipi.MomeUpdateSqli(id);
                    //uSql.Dump();
            });
    }
}

// Define other methods and classes here
