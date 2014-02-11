<Query Kind="Program">
  <GACReference>Microsoft.Office.Interop.Access, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c</GACReference>
  <GACReference>Microsoft.Office.Interop.Access.Dao, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c</GACReference>
  <NuGetReference>Dapper</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>Microsoft.Office.Interop.Access</Namespace>
  <Namespace>Microsoft.Office.Interop.Access.Dao</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Mail</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

Action<SqlConnection,string,string> OperaciaGaukmeba(string operaciisTipi )
{
	return (SqlConnection conn, string dazgCkhilisSaxeli, string periodi)=>
	{
		var dt = DateTime.Now;
		conn.Execute("update l set l.GagzavnisDro = '"+dt.ToString("yyyy-MM-dd HH:mm:ss.fff")+"' from INSURANCEW..ChatarebuliOperaciebisLogi l join INSURANCEW..[" + dazgCkhilisSaxeli + "] d on l.DazgvevisID = d.ID where GagzavnisDro is null and OperaciisTipi='" + operaciisTipi + "'", commandTimeout:59*60);
		
		conn.Execute(@"insert into INSURANCEW..SadazgveostvisGasagzavniSelectebi(Ganmarteba, Sql) select @a,@b",
		new { 
		a = "გაუქმებული_კონტრაქტები",  
		b = @"SELECT d.ID
	,STOP_DATE_" + periodi + @"_TMP
	,Statusi
	,Ganmarteba
	,u.DEATH_DATE as GardacvalebisTarigi
	,d.Company_ID_"+periodi+@" Company_ID
	,'" + operaciisTipi + @"' Ganmarteba2
FROM		INSURANCEW.dbo.ChatarebuliOperaciebisLogi (nolock) l 
JOIN		INSURANCEW.dbo." + dazgCkhilisSaxeli + @" (nolock) d on d.ID = l.DazgvevisID 
LEFT JOIN   UketesiReestri.dbo.vMimdinare_Reestri (nolock) u on d.PID collate SQL_Latin1_General_CP1_CI_AS = u.PID
LEFT JOIN	INSURANCEW..StatusebisGanmarteba (nolock) sg on d.STATE_"+periodi+@" = sg.Statusi
WHERE		l.OperaciisTipi ='"+ operaciisTipi + @"' and GagzavnisDro = '" + dt.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'"}, commandTimeout:59*60);
	};
}

void OperaciaAkhaliKontraqti(SqlConnection conn, string dazgCkhilisSaxeli, string periodi )
{
	var dt = DateTime.Now;
	conn.Execute("update l set l.GagzavnisDro = '"+dt.ToString("yyyy-MM-dd HH:mm:ss.fff")+"' from INSURANCEW..ChatarebuliOperaciebisLogi l join INSURANCEW..[" + dazgCkhilisSaxeli + "] d on l.DazgvevisID = d.ID where GagzavnisDro is null and OperaciisTipi='AkhaliKontraqti'", commandTimeout:59*60);
	conn.Execute(@"insert into INSURANCEW..SadazgveostvisGasagzavniSelectebi(Ganmarteba, Sql) select @a,@b",
	new {a="დამატებული_კონტრაქტები", b=@"SELECT	 d.ID,d.Base_type,d.Base_Description
        ,d.FID
        ,d.Unnom,u.PID, u.FIRST_NAME, u.LAST_NAME, u.BIRTH_DATE, isnull(pir.Gender,u.Sex) Sex, d.SAG_DACESEBULEBA
        ,d.REGION_ID, d.RAI, d.RAI_NAME, d.CITY, d.VILLAGE, d.ADDRESS_FULL
		,d.Company_ID_"+periodi+@" as Company_ID,d.Company_"+periodi+@" as Company
		,d.[dagv-tar],[End_Date],[STOP_DATE]
        ,d.STATE_"+periodi+@" as STATE
        , p.PolisisNomeri
        ,["+periodi+@"] - M_"+periodi+@" as ["+periodi+@"] 
FROM	INSURANCEW.dbo.ChatarebuliOperaciebisLogi (nolock) l 
JOIN	INSURANCEW.dbo.["+dazgCkhilisSaxeli+@"] (nolock) d on d.ID = l.DazgvevisID 
JOIN	SocialuriDazgveva.dbo.Polisebi (nolock) p on d.ID = p.DzveliSistemisId and (p.PolisisStatusi <> 'Gaukmebuli' or p.PolisisStatusi is null)
JOIN	UketesiReestri.dbo.vUnnomBoloKargiChanaceri (nolock) u on d.Unnom = u.Unnom
LEFT JOIN ( select boloChanaceriUnnomze.*
            from (
                    select Max(RecId) RecId
                    from  Pirvelckaroebi.dbo.[Pirvelckaro_27_AXALSHOBILEBI(165)] (nolock)
                    group by Unnom
                ) bolo
            outer apply (
                    select a.Unnom, a.Gender
                    from  Pirvelckaroebi.dbo.[Pirvelckaro_27_AXALSHOBILEBI(165)] (nolock) a
                    where a.RecId = bolo.RecId
                    ) boloChanaceriUnnomze
          ) pir on d.Unnom = pir.Unnom
WHERE	l.OperaciisTipi='AkhaliKontraqti' and d.STATE_"+periodi+@" > 0 and l.GagzavnisDro = '" + dt.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'"}, commandTimeout:59*60);
}

void Main()
{
	Task.Factory.StartNew(async () => {
		while (true)
		{
			var operaciebi = new Dictionary<string,Action<SqlConnection,string,string>>(){
					{"AkhaliKontraqti", OperaciaAkhaliKontraqti},
					{"DamtavrebuliKontraqti", OperaciaGaukmeba("DamtavrebuliKontraqti")},
					{"GauqmebuliKharvezianiKontraqti", OperaciaGaukmeba("GauqmebuliKharvezianiKontraqti")},
					{"GaukmebuliKharvezianiKontraqti", OperaciaGaukmeba("GaukmebuliKharvezianiKontraqti")},
					{"GauqmebuliDublirebuliKontraqti", OperaciaGaukmeba("GauqmebuliDublirebuliKontraqti")},
					{"GauqmebuliPolisisChaubareblobisGamo", OperaciaGaukmeba("GauqmebuliPolisisChaubareblobisGamo")},
					{"GauqmebuliGardacvalebisGamo", OperaciaGaukmeba("GauqmebuliGardacvalebisGamo")},
					{"GauqmebuliDazgvevazeUarisGamo", OperaciaGaukmeba("GauqmebuliDazgvevazeUarisGamo")},
				};
			
			SqlConnection(conn => {
				var dazgCkhilisSaxeli = conn.Query<string>("SELECT TOP 1 TABLE_NAME FROM INSURANCEW.INFORMATION_SCHEMA.TABLES WHERE UPPER(TABLE_NAME) LIKE 'DAZGVEVA_201[0-9][0-9][0-9]' ORDER BY UPPER(TABLE_NAME) DESC").First ();
				var periodi = dazgCkhilisSaxeli.Substring(9, 6);
				var gasagzaniSimravleebi = new List<string>(); 
				gasagzaniSimravleebi.AddRange(
					conn.Query("select l.OperaciisTipi, max(l.Dro) Dro from INSURANCEW..ChatarebuliOperaciebisLogi l join INSURANCEW..[" + dazgCkhilisSaxeli + "] (nolock) d on l.DazgvevisID = d.ID where GagzavnisDro is null group by l.OperaciisTipi", commandTimeout : 59*60)
						.Where (x => x.Dro < DateTime.Now.AddHours( -1 ))
						.Select (x => (string)x.OperaciisTipi)
					);
				foreach (var op in operaciebi.Where(x => gasagzaniSimravleebi.Contains(x.Key)))
				{
					op.Value(conn, dazgCkhilisSaxeli, periodi);
				}
			}, ex => {
					ex.Message.Dump();
				});
			await Task.Delay(15000);
		}
	}, TaskCreationOptions.LongRunning)
	.ContinueWith(t => t.Exception.Dump(), TaskContinuationOptions.OnlyOnFaulted);

	Task.Factory.StartNew(async () => {
		while (true)
		{
			var gasagzaniSimravleebi = new List<GasagzavniSimravle>(0); 
			SqlConnection(conn => gasagzaniSimravleebi.AddRange(conn.Query<GasagzavniSimravle>("select * from INSURANCEW..SadazgveostvisGasagzavniSelectebi where CkhrilisShekmnisDro is not null and GagzavnisDro is null")));
			
			
			foreach (var gasagzaniSimravle in gasagzaniSimravleebi)
			{
				SqlConnection(conn => {
					foreach (var kodi in conn.Query<string>("select distinct MzgveveliKompaniisKodi from DazgvevaExport..[" + gasagzaniSimravle.OperaciisShedegi + "]")
												.Where (x => x != "IML")
												.Select(x=>new {key=x, values=x!="ALD"?new []{x}:new []{x,"IML"}})
												.ToDictionary (x => x.key,x=>string.Join(" OR ", x.values.Select (v => string.Format("MzgveveliKompaniisKodi='{0}'", v))))
										)
					{
						Exporter.Export(gasagzaniSimravle.Ganmarteba, kodi.Key + "_" + gasagzaniSimravle.OperaciisShedegi, "select * from DazgvevaExport..[" + gasagzaniSimravle.OperaciisShedegi + "] where " + kodi.Value);
					}
					conn.Execute("update INSURANCEW..SadazgveostvisGasagzavniSelectebi set GagzavnisDro='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' where Id=" + gasagzaniSimravle.Id, commandTimeout : 59*60);
					gasagzaniSimravle.OperaciisShedegi.Dump();
				}, ex => {
					ex.Message.Dump();
				});
				
			}
			
			await Task.Delay(1000);
		}
	}, TaskCreationOptions.LongRunning)
	.ContinueWith(t => t.Exception.Dump(), TaskContinuationOptions.OnlyOnFaulted);
	
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
						conn.Execute("update INSURANCEW..SadazgveostvisGasagzavniSelectebi set OperaciisShedegi=N'ვერ მოხერხდა Company_Id ველის დასახელების დადგენა' where Id=" + gasagzaniSimravle.Id, commandTimeout : 59*60);
					}
					else if (conn.Query<int>("select count(*) Raodenoba from (" + gasagzaniSimravle.Sql + ") gc",commandTimeout:59*60).First () == 0)
					{
						conn.Execute("update INSURANCEW..SadazgveostvisGasagzavniSelectebi set OperaciisShedegi=N'სიმრავლე შეიცავს 0 ჩანაწერს' where Id=" + gasagzaniSimravle.Id, commandTimeout : 59*60);
					}
					else
					{
						var dro = DateTime.Now;
						var ckhrilisSakheli = gasagzaniSimravle.Ganmarteba + "_" + dro.ToString("yyyyMMdd_HHmmss");
						conn.Execute("select k.MzgveveliKompaniisKodi, gc.* into DazgvevaExport..[" + ckhrilisSakheli + "] from (" + gasagzaniSimravle.Sql + ") gc join SocialuriDazgveva..MzgvevelisIdDaKodi k on k.Id = gc." + companiisIdVeli, commandTimeout : 59*60);
						conn.Execute("CREATE NONCLUSTERED INDEX [IX_"+ckhrilisSakheli+"] ON DazgvevaExport.dbo.["+ckhrilisSakheli+"] (MzgveveliKompaniisKodi)", commandTimeout : 59*60);
						conn.Execute("update INSURANCEW..SadazgveostvisGasagzavniSelectebi set OperaciisShedegi=N'" + ckhrilisSakheli + "', CkhrilisShekmnisDro='" + dro.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'  where Id=" + gasagzaniSimravle.Id, commandTimeout : 59*60);
					}
				}, ex =>{
					ex.Message.Dump();
					SqlConnection(conn => conn.Execute("update INSURANCEW..SadazgveostvisGasagzavniSelectebi set OperaciisShedegi=N'" + ex.Message + " where Id=" + gasagzaniSimravle.Id, commandTimeout : 59*60));
				});
			}
			await Task.Delay(1000);
		}
	}, TaskCreationOptions.LongRunning)
	.ContinueWith(t => t.Exception.Dump(), TaskContinuationOptions.OnlyOnFaulted);
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
	public static IEnumerable<Veli> MomeVelebiSqlSelectidan(this SqlConnection conn, string sql)
	{
		using (var command = new SqlCommand("select top 0 * from (" + sql +") gs where 1 = 2", conn){CommandTimeout=59*60})
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
		using(var txs = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(59)))
		using (var conn = new SqlConnection("Data Source=Triton;User ID=sa;Password=ssa$20;Initial Catalog=INSURANCEW;app=DazgvevaWorkers"))
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

public class Exporter
{
	public static void Export(string tableName, string fileName, string sql)
	{
		var baseDir = @"C:\სადაზღვევოებს";
		var folder = Path.Combine(baseDir, tableName.Replace("_"," "));
		var fullfileName = Path.Combine(folder, fileName+".accdb");
		
		if(!Directory.Exists(folder))
		{
			Directory.CreateDirectory(folder);
		}
		if(File.Exists(fullfileName))
		{
			File.Delete(fullfileName);
		}
		ExportToAccess(fullfileName, tableName, sql);
	}
	
	static void ExportToAccess(string failisSakheli, string ckhrilisSakheli, string sql)
	{
		MokaliKvelaAccessisProcesi();
		var application = new Application();
		if (File.Exists(failisSakheli))
		{
			application.OpenCurrentDatabase(failisSakheli);
		}
		else
		{
			application.NewCurrentDatabase(failisSakheli);
		}
		
		var currentDb = application.CurrentDb();
		foreach(var qdName in currentDb.QueryDefs.Cast<dynamic>().Select (x => (string)x.Name))
		{
			currentDb.DeleteQueryDef(qdName);
		}
		if (currentDb.TableDefs.Cast<dynamic>().Any(x => x.Name.ToLower() == ckhrilisSakheli.ToLower()))
		{
			QueryDef dropQd = currentDb.CreateQueryDef("qdDropTable", string.Format("DROP TABLE {0}", ckhrilisSakheli));
			dropQd.Execute();
			currentDb.DeleteQueryDef("qdDropTable");
		}
		currentDb.QueryDefs.Refresh();
		currentDb.TableDefs.Refresh();
		
		var getDataQd = currentDb.CreateQueryDef("qdSelectFromSql");
		getDataQd.Connect = @"ODBC;DRIVER=SQL Server;SERVER=triton;UID=sa;PWD=ssa$20;DATABASE=SocialuriDazgveva";
		getDataQd.SQL = sql;
		getDataQd.ODBCTimeout = 59*60;
		
		QueryDef insertIntoQd = currentDb.CreateQueryDef("qdSelectInto", string.Format("select * into {0} from qdSelectFromSql", ckhrilisSakheli));
		insertIntoQd.Execute();
		currentDb.DeleteQueryDef("qdSelectInto");
		currentDb.DeleteQueryDef("qdSelectFromSql");
		application.Quit(AcQuitOption.acQuitSaveAll);
		Thread.Sleep(5000);
	}
	
	static void MokaliKvelaAccessisProcesi()
	{
		foreach (Process p in Process.GetProcesses().Where(x => x.ProcessName.ToUpper().StartsWith("MSACCESS")))
		{
			try
			{
				p.Kill();
				p.WaitForExit(); // possibly with a timeout
			}
			//catch (Win32Exception winException)
			//{
			//    // process was terminating or can't be terminated - deal with it
			//}
			catch (InvalidOperationException invalidException)
			{
				// process has already exited - might be able to let this one go
			}
		}
	}
}

void SendMail(string to, string subject, string body)
{
	  MailMessage mail = new MailMessage("abolkvadze@ssa.gov.ge", to);
      SmtpClient client = new SmtpClient();
      client.Port = 25;
      client.DeliveryMethod = SmtpDeliveryMethod.Network;
      client.UseDefaultCredentials = false;
	  client.Credentials = new NetworkCredential("abolkvadze@ssa.gov.ge","08271114");
      client.Host = "mail.moh.gov.ge";
      mail.Subject = subject;
      mail.Body = body;
      client.Send(mail);
}