<Query Kind="Program">
  <NuGetReference>Dapper</NuGetReference>
  <NuGetReference>RavenDB.Client</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>Raven.Client.Document</Namespace>
  <Namespace>Raven.Json.Linq</Namespace>
  <Namespace>Raven.Abstractions.Data</Namespace>
</Query>

void Main()
{
var sql = "select * from INSURANCEW.dbo.DAZGVEVA_201309";
using(var conn = new SqlConnection("Data Source=Triton;User ID=sa;Password=ssa$20;Initial Catalog=INSURANCEW;app=Worker1"))
{
	conn.Open();
	using(var cmd = new SqlCommand(sql, conn){CommandTimeout=999})
	using(var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
	using(var ds = new DocumentStore{Url="http://localhost:8080"}.Initialize())
	{
		var enumerator = ((IEnumerable)reader).GetEnumerator();
		using (var bi = ds.BulkInsert("Soc",new BulkInsertOptions{CheckForUpdates=true,BatchSize=1024}))
		{
			bi.Report += (s) => {
			Util.ClearResults();
			s.Dump();
			};
			

			while (enumerator.MoveNext() )
			{
				var rec = ((IDataRecord)enumerator.Current);
				var o = Enumerable
					.Range(0,rec.FieldCount)
					.Select (i => rec.GetName(i))
					.Aggregate (new RavenJObject(), (seed,x) => {
						seed.Add(x, RavenJToken.FromObject(rec[x]));
						return seed;
					});
				var m = new RavenJObject();
				m.Add("Raven-Entity-Name", RavenJToken.FromObject("DazgvevaChanacerebi"));
				bi.Store(o, m, "dazgvevischanaceri/" + rec["ID"]);
			}
		}
		reader.Close();
			
	}
}

return;
using(var enumer = new ReaderEnumerator("select  top 2 * from [SocialuriDazgveva].[dbo].[Polisebi] (nolock)"))
{
	var i=0;
	while (enumer.MoveNext() )
	{
		enumer.Current.Dump();
	}
}
//	SqlConnection(conn=>{
//		conn.Query("select * from [SocialuriDazgveva].[dbo].[Polisebi] p")
//		.Take(10).Dump();
//	});
}
class ReaderEnumerator:IEnumerator<IDataRecord>
{
	SqlConnection _conn;
	SqlCommand _cmd;
	SqlDataReader _reader;
	IEnumerator _enumerator;
	string _sql;
	
	public ReaderEnumerator(string sql)
	{
		_sql = sql;
		this.Reset();
	}
	
	object IEnumerator.Current{get{return this.Current;}}
    public IDataRecord Current{get{return (IDataRecord)_enumerator.Current;}}
	public bool MoveNext(){ return _enumerator.MoveNext(); }
    public void Reset(){
		((IDisposable)this).Dispose();
		_conn = new SqlConnection("Data Source=Triton;User ID=sa;Password=ssa$20;Initial Catalog=INSURANCEW;app=Worker1");
		_conn.Open();
		_cmd = new SqlCommand(_sql, _conn);
		_reader = _cmd.ExecuteReader();
		_enumerator = _reader.GetEnumerator();
	}
	void IDisposable.Dispose()
    {
		using (var oldconn = _conn)
		using (var oldcmd = _cmd)
		using (var oldreader = _reader)
		{
			_conn = null;
			_cmd = null;
			_reader = null;			
		}
    }
}
void SqlConnection(Action<SqlConnection> actOnConnection, Action<Exception> actOnEx = null){
	using(var txs = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(3600)))
	using (var conn = new SqlConnection("Data Source=Triton;User ID=sa;Password=ssa$20;Initial Catalog=INSURANCEW;app=Worker1"))
	{
		conn.Open();
		actOnConnection(conn);
		txs.Complete();
	}
}