<Query Kind="Program">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAAAD89x4SL38S/4r7NUU2iHNNTcmnVTi1xQPx1AC1vAtFAAAAAAOgAAAAAIAACAAAABgO+xVBio0IKzceIXWbWfgFv0jcxQpOA9YilhDtPA8XxAAAABJcM5+MLInsGd5jUUGfXtnQAAAALHy7sVof5cKLhfpSHxbLdPESALwKOWLElOgeYcZmaeqO1sDG9SHnPN5xOzSlBHlSD7agk+KC9dH/4XMZn4gYvM=</Password>
    <Database>Pirvelckaroebi</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>D:\Dev\EventStore\bin\eventstore\debug\anycpu\EventStore.ClientAPI.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ComponentModel.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.Tasks.dll</Reference>
  <NuGetReference>Dapper</NuGetReference>
  <NuGetReference>Hive.Sharp.Lib</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference>protobuf-net</NuGetReference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>EventStore.ClientAPI</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Data.Common</Namespace>
  <Namespace>System.IO.Compression</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
  <Namespace>System.Reactive.Disposables</Namespace>
  <Namespace>System.Reactive.Joins</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Reactive.PlatformServices</Namespace>
  <Namespace>System.Reactive.Subjects</Namespace>
  <Namespace>System.Reactive.Threading.Tasks</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{   

var obs = Observable.Interval(TimeSpan.FromMilliseconds(500));
obs.Select((t,i) => new {t,i})
    .GroupBy(x=>x.i%4)
    .Select(g=>g.Scan(new {ms=new List<long>(),items=default(long[])},
                                     (a,l)=>{   
                                                  a.ms.Add(l.t);
                                                  if(a.ms.Count == 3)
                                                    return new {ms=new List<long>(),items=a.ms.ToArray()};
                                                  return new {ms=a.ms, items=default(long[])};
                                         }).
                                         Publish(o => 
                                                o.sk.(o.TakeLast(1).Amb())
                                            )
           )
    .SelectMany(x=>x)
    .Dump();


    return;
    var con = Connection;
    con.Open();
    con.DumpQueryToHive("JUST_20121102", @"SELECT * FROM UketesiReestri..JUST_20121101")
       
       .Dump();
    
}





public static class Store
{
    public static void Local(Action<EventStoreConnection> actOnConnection)
    {
        using(var es = EventStore.ClientAPI.EventStoreConnection.Create())
        {
            es.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1113));
            actOnConnection(es);
            es.Close();
        }
    }
}

public static class ExtendedObservable
{
    public static string SqlRowToHiveRow(this ExtendedDbConnection.SqlRow sqlRow)
    {
        var colSeparator = ASCIIEncoding.ASCII.GetString(new byte[] { 1 } );
        var sb = new StringBuilder();
        foreach (var kv in sqlRow)
        {
            sb.Append(kv.Value.ToString().Replace(colSeparator,"").Replace("\n",""));
            sb.Append(colSeparator);
        }
        sb.Append('\n');
        return sb.ToString();
    }
    
    public static System.Threading.Tasks.Task DumpToFile(this IObservable<ExtendedDbConnection.SqlRow> obs, string filename)
    {
        //var ifs = new FileStream(filename, FileMode.CreateNew);
        //var fs = new GZipStream(ifs,CompressionMode.Compress);
        
        var fs = new FileStream(filename, FileMode.CreateNew);
        return obs.Select(row=>Encoding.UTF8.GetBytes(SqlRowToHiveRow(row)))
                .DumpToStream(fs)
                .Finally(()=> {
                    fs.Flush(); fs.Close(); fs.Dispose();
                }).ToTask();
    }
    
    public static IObservable<byte[]> DumpToStream(this IObservable<byte[]> obs, Stream stream)
    {
        return obs.Do(bytes=>stream.Write(bytes,0,bytes.Length));
    }

}



public static class ExtendedDbConnection
{
    public class SqlRow : Dictionary<string, object>
    {
    }
    public class DumpOperation
    {
        SqlConnection _con;string _tname;string _query;
        public DumpOperation(SqlConnection con,string tname,string query)
        {
            _con   = con;
            _tname = tname;
            _query = query;
        }
        
        public async void Dump()
        {
            var filename = "D:\\Temp\\"+_tname+".hive.gz";
            File.Delete(filename);
            var total = (int)(await _con.QueryObserver("select count(*) raod from ( "+_query+") t").ToTask())["raod"];
            
            var q = _con.QueryObserver(_query).Publish();
            
            
            
            
                
            q.ObserveOn(Scheduler.Default)
                .Buffer(q.ObserveOn(Scheduler.Default)
                            .Select((_,i)=>Math.Round(i*100.0/total))
                            .Where(p=>p%5==0).DistinctUntilChanged())
                .TimeInterval()
                .ForEachAsync((ti,i)=>(ti.Value.Count/(double)ti.Interval.TotalSeconds).Dump("row/s "+i*5+"%"));
            var dumpt = q.DumpToFile(filename);
            q.Connect();
            await dumpt;
            
            var row = await _con.QueryObserver(_query).Take(1).ToTask();

            var soc= new Thrift.Transport.TSocket("hadoop",10000);
            var hc = new Apache.Hadoop.Hive.ThriftHive.Client(new Thrift.Protocol.TBinaryProtocol(soc));
            soc.Open();
            hc.execute("drop table " + _tname);
            hc.execute("show tables");
            hc.fetchAll().Dump();
            var create="create table "+_tname+" ("+string.Join(",",row.Keys.Select(k=>k.Replace("-","_").Replace("  ","_").Replace(" ","_") + "_ string" ))+")";
            create.Dump();
            
            hc.execute(create);
            hc.execute("show tables");
            hc.fetchAll().Dump();
            hc.execute("LOAD DATA LOCAL INPATH  '/home/jani/Data/"+_tname+".hive.gz' OVERWRITE INTO TABLE " + _tname);
            soc.Close();
        }
    }
   
    public static DumpOperation DumpQueryToHive(this DbConnection con, string tname,string query)
    {
        return new DumpOperation((SqlConnection)con,tname,query);
    }
    public static IObservable<SqlRow> QueryObserver(this DbConnection con, string sql)
    {
        return Observable.Create<SqlRow>((o,ct) => {
            return System.Threading.Tasks.Task.Run(()=>
            {
                using(var cmd = new SqlCommand(sql, (SqlConnection)con))
                {
                    cmd.CommandTimeout = 9999;
                    using(var reader = cmd.ExecuteReader())
                    {
                        while(reader.Read() && !ct.IsCancellationRequested)
                        {
                            var se = new SqlRow();
                            for (var i = 0; i < reader.FieldCount ; i++)
                            {
                                se[reader.GetName(i)] = reader.GetValue(i);
                            }
                            o.OnNext(se);
                        }
                        reader.Close();
                        o.OnCompleted();
                    }
                }
            });
        });
    }
}