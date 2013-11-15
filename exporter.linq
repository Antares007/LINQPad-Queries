<Query Kind="Program">
  <NuGetReference>Dapper</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

public void ExportToAccess(string tableName, string fileName, string sql)
{
	const string s = @"D:\saved\Acho\Documents\Visual Studio 2012\Projects\ClassLibrary1\ClassLibrary1\bin\Debug\export.exe";
	if(File.Exists(fileName))
	{
		File.Delete(fileName);
	}
	var args = string.Format("/FailisSakheli \"{0}\" /CkhrilisSakheli \"{1}\" /Sql  \"{2}\"", fileName, tableName, sql);

	var p = Process.Start(s, args);
	p.WaitForExit();

	if(p.ExitCode != 0)
	{
		throw new InvalidOperationException("aaa");
	};
}

void Main()
{
	Task.Factory.StartNew(async () => {
		while (true)
		{
			var gasagzaniSimravleebi = new List<GasagzavniSimravle>(0); 
			SqlConnection(conn => gasagzaniSimravleebi.AddRange(conn.Query<GasagzavniSimravle>("select * from INSURANCEW..SadazgveostvisGasagzavniSelectebi where CkhrilisShekmnisDro is not null and GagzavnisDro is null")));
			
			
			foreach (var gasagzaniSimravle in gasagzaniSimravleebi)
			{
				SqlConnection(conn => {
					foreach (var kodi in conn.Query<string>("select distinct MzgveveliKompaniisKodi from DazgvevaExport..[" + gasagzaniSimravle.OperaciisShedegi + "]"))
					{
						ExportToAccess(gasagzaniSimravle.Ganmarteba, "c:\\temp\\"+kodi+".accdb","select * from DazgvevaExport..[" + gasagzaniSimravle.OperaciisShedegi + "] where MzgveveliKompaniisKodi='" + kodi + "'");
					}
					conn.Execute("update INSURANCEW..SadazgveostvisGasagzavniSelectebi set GagzavnisDro='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' where Id=" + gasagzaniSimravle.Id, commandTimeout : 999);
				}, ex => {
					ex.Message.Dump();
				});
			}
			
			await Task.Delay(1000);
		}
	}, TaskCreationOptions.LongRunning);
	
	Task.Factory.StartNew(async () => {
		while (true)
		{
			var gasagzaniSimravleebi = new List<GasagzavniSimravle>(0); 
			SqlConnection(conn => gasagzaniSimravleebi.AddRange(conn.Query<GasagzavniSimravle>("select * from INSURANCEW..SadazgveostvisGasagzavniSelectebi where OperaciisShedegi is null")));
			
			foreach (var gasagzaniSimravle in gasagzaniSimravleebi)
			{
				SqlConnection(conn => {
					var cols = conn.MomeVelebiSqlSelectidan(gasagzaniSimravle.Sql);
					var companiisIdVeli = IpoveCompaniisIdVelisDasakheleba(cols);
					
					if(companiisIdVeli == null)
					{
						conn.Execute("update INSURANCEW..SadazgveostvisGasagzavniSelectebi set OperaciisShedegi=N'ვერ მოხერხდა Company_Id ველის დასახელების დადგენა' where Id=" + gasagzaniSimravle.Id, commandTimeout : 999);
					}
					else if (conn.Query<int>("select count(*) Raodenoba from (" + gasagzaniSimravle.Sql + ") gc").First () == 0)
					{
						conn.Execute("update INSURANCEW..SadazgveostvisGasagzavniSelectebi set OperaciisShedegi=N'სიმრავლე შეიცავს 0 ჩანაწერს' where Id=" + gasagzaniSimravle.Id, commandTimeout : 999);
					}
					else
					{
						var dro = DateTime.Now;
						var ckhrilisSakheli = gasagzaniSimravle.Ganmarteba + "_" + dro.ToString("yyyyMMdd_HHmmss");
						conn.Execute("select k.MzgveveliKompaniisKodi, gc.* into DazgvevaExport..[" + ckhrilisSakheli + "] from (" + gasagzaniSimravle.Sql + ") gc join SocialuriDazgveva..MzgvevelisIdDaKodi k on k.Id = gc." + companiisIdVeli, commandTimeout : 999);
						conn.Execute("update INSURANCEW..SadazgveostvisGasagzavniSelectebi set OperaciisShedegi=N'" + ckhrilisSakheli + "', CkhrilisShekmnisDro='" + dro.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'  where Id=" + gasagzaniSimravle.Id, commandTimeout : 999);
					}
				}, ex =>{
					SqlConnection(conn => conn.Execute("update INSURANCEW..SadazgveostvisGasagzavniSelectebi set OperaciisShedegi=N'" + ex.Message + " where Id=" + gasagzaniSimravle.Id, commandTimeout : 999));
				});
			}
			await Task.Delay(1000);
		}
	}, TaskCreationOptions.LongRunning);
}

string IpoveCompaniisIdVelisDasakheleba(IEnumerable<Veli> xs)
{
	var regex = new Regex(@"^Company_ID_*(\d\d\d\d\d\d)*$", RegexOptions.IgnoreCase);
	return xs	.Where (x => typeof(long) == x.Tipi || typeof(int) == x.Tipi)
				.Select (x => regex.Match(x.Sakheli))
				.Where (x => x.Success)
				.Select (x => new {x.Value, Order = x.Groups[1].Success ? int.Parse(x.Groups[1].Value):0})
				.OrderByDescending (x => x.Order)
				.Select (x => x.Value)
				.FirstOrDefault();
}
static class ExtendedConnection
{
	public static void ShekmeniDasaeksportebeliCkhrili(this SqlConnection conn, string ganmarteba, string sql, string companiisIdVeli)
	{
	}
	
	public static IEnumerable<Veli> MomeVelebiSqlSelectidan(this SqlConnection conn, string sql)
	{
		using (var command = new SqlCommand("select top 0 * from (" + sql +") gs where 1 = 2", conn))
		using(var reader = command.ExecuteReader())
		{
		
			return reader.GetSchemaTable().AsEnumerable()
				.Select (x => new Veli{Sakheli=x.Field<string>("ColumnName"), Tipi=x.Field<Type>("DataType")});
		}
	}
}
class GasagzavniSimravle
{
	public int Id { get; set; }
	public string Ganmarteba { get; set; }
	public string Sql { get; set; }
	public string OperaciisShedegi { get; set; }
	
}
class Veli
{
	public string Sakheli { get; set; }
	public Type Tipi { get; set; }
}
void SqlConnection(Action<SqlConnection> actOnConnection, Action<Exception> actOnEx = null){
	try
	{
		using(var txs = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(3600)))
		using (var conn = new SqlConnection("Data Source=Triton;User ID=sa;Password=ssa$20;Initial Catalog=904;app=Worker1"))
		{
			conn.Open();
			actOnConnection(conn);
			txs.Complete();
		}
	}
	catch(Exception ex)
	{
		if(actOnEx!=null)
		{
			actOnEx(ex);
		}
	}
}