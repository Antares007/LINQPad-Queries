<Query Kind="Program">
  <Connection>
    <ID>ced05258-b3f5-4292-8b8d-577da55d0081</ID>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAN27c6lA2WkeiuPoKsF6zVAAAAAACAAAAAAAQZgAAAAEAACAAAAAe6fEoA74iMkrfJZa6XuxfzQh6cY+5cD/VGmkvkAFZHgAAAAAOgAAAAAIAACAAAAAvSa58+4v5Ek9QdqmoHFRtbjSsRtGtZOiiFFwZuP9l4hAAAADSD159L0suWS7o5rny5Q3sQAAAAFcvMLTxKkyAI0tWzIRYIyYLH7PYk2LvzCPR+00AaEffUskcT8F2IaOy6O9Btu50kIHx2PxD4ahCWKTfrpSexFY=</Password>
    <Database>Pirvelckaroebi</Database>
    <ShowServer>true</ShowServer>
    <Persist>true</Persist>
  </Connection>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\110\SDK\Assemblies\Microsoft.SqlServer.ConnectionInfo.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\110\SDK\Assemblies\Microsoft.SqlServer.Management.Sdk.Sfc.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\110\SDK\Assemblies\Microsoft.SqlServer.Smo.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\110\SDK\Assemblies\Microsoft.SqlServer.SmoExtended.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\110\SDK\Assemblies\Microsoft.SqlServer.SqlEnum.dll</Reference>
  <NuGetReference>Dapper</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Common</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Dmf</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Smo</Namespace>
  <Namespace>System.Data.Common</Namespace>
</Query>

class PirvelckarosCkhrilebi
{
    Microsoft.SqlServer.Management.Smo.Server _server;
    static string[] _cxrilebisSakhelebi = new []{"Pirvelckaro_01_UMCEOEBI","Pirvelckaro_02_DEVNILEBI","Pirvelckaro_03_BAVSHVEBI","Pirvelckaro_04_REINTEGRACIA","Pirvelckaro_05_KULTURA","Pirvelckaro_06_XANDAZMULEBI","Pirvelckaro_07_SKOLA_PANSIONEBI","Pirvelckaro_08_TEACHERS","Pirvelckaro_09_UFROSI_AGMZRDELEBI","Pirvelckaro_10_APKHAZETIS_OJAKHEBI","Pirvelckaro_11_SATEMO","Pirvelckaro_12_MCIRE_SAOJAXO","Pirvelckaro_13_TEACHERS_AFX","Pirvelckaro_14_RESURSCENTRIS_TANAMSHROMLEBI","Pirvelckaro_21_SAPENSIO_ASAKIS_MOSAXLEOBA","Pirvelckaro_22_STUDENTEBI","Pirvelckaro_23_BAVSHVEBI(165)","Pirvelckaro_24_INVALIDI_BAVSHVEBI","Pirvelckaro_25_MKVETRAD_GAMOXATULI_INVALIDI_BAVSHVEBI","Pirvelckaro_26_ARASAQARTVELOS_MOQALAQE_PENSIONREBI","Pirvelckaro_27_AXALSHOBILEBI(165)","Pirvelckaro_100_DevniltaMisamartebi_201210"};
    public PirvelckarosCkhrilebi(Microsoft.SqlServer.Management.Smo.Server server)
    {
        _server = server;
    }
    public IEnumerable<Table> MomeKvela()
    {
        foreach (var ckhilisSakheli in _cxrilebisSakhelebi)
        {
            yield return Ex.Triton.Databases["Pirvelckaroebi"].Tables[ckhilisSakheli];
        }
    }
}
void Main()
{
//(
//	from db in Ex.Triton.Databases()
//	where db.Name.StartsWith("INSURANCEW")
//	from table in db.Tables()
//	where table.Name.StartsWith("DAZGVEVA_201309")
//	from index in table.Indexes.Cast<Index>()
//	select new {index.Name,Cols = index.IndexedColumns.Cast<IndexedColumn>().Select (ic => new {ic.Name,ic.IsIncluded})}
//).Dump();
//return;
//Ex.Triton.Databases["INSURANCEW"]
//	.Tables().Select (x => new {x.Name,x.DateLastModified}).OrderBy (x => x.DateLastModified).Dump();
//return;
//var dbName = "INSURANCEW";
//var q = Ex.Triton.Databases[dbName]
//	.Tables().Where (x => x.Name.StartsWith("DAZGVEVA_"))
//	.Where (t => t.Indexes().Count () > 0)
//	
//	.OrderBy (x => x.Name)
//	.Select(t => new {TName=t.Name,DropSql=t.Indexes().Select (i => string.Format("--Indexed:{4} Included:{5}\nDROP INDEX [{0}] ON [{3}].[dbo].[{1}] -- {2:N}kb",i.Name,t.Name,i.SpaceUsed,dbName,string.Join(",",i.IndexedColumns().Where (x => !x.IsIncluded).Select (ic => ic.Name)),string.Join(",",i.IndexedColumns().Where (x => x.IsIncluded).Select (ic => ic.Name))))})
//	.Select (t => string.Format("--{0}\n{1}",t.TName,string.Join("\nGO\n",t.DropSql)))
//	;
//string.Join("\nGO\n\n\n\n",q).Dump();
//return;
//
//(
//	from db in Ex.Triton.Databases()
//	from f in 	(
//					from fg in db.FileGroups.Cast<FileGroup>()
//					from file in fg.Files.Cast<DataFile>()
//					select new {file.FileName,file.Size}
//				).Concat(
//					from log in db.LogFiles.Cast<LogFile>()
//					select new {log.FileName, log.Size}
//				)
//	select new {db.Name, f.FileName, f.Size, DateLastModified = db.Tables().Select(table => table.DateLastModified).OrderByDescending (x=>x).FirstOrDefault ()}
//).Cache()
//.Dump();
//
////Ex.Triton.Databases()
////	.SelectMany (d => d.LogFiles.Cast<LogFile>().Select(lf => new {lf.FileName,lf.Size,d.Name})
////							   .Concat(d.FileGroups.Cast<FileGroup>().SelectMany (fg => fg.Files.Cast<DataFile>().Select (df => new {df.FileName,df.Size,d.Name})))
////					).Select (x => new {PathRoot=Path.GetPathRoot(x.FileName),x.FileName,x.Name,x.Size}).Dump();
//
//return;
Func<string,bool,string> gaakhvie = (s,isUnicode) =>
@"ltrim(rtrim(replace(replace(replace(replace(replace(replace(replace(replace(replace("+(isUnicode?"Pirvelckaroebi.dbo.fn_con2utf8([" + s + @"])":"[" + s + @"]")+",nchar(10),' '),nchar(13),' '),nchar(9),' '),'   ',' '),'   ',' '),'   ',' '),'  ',' '),'  ',' '),'  ',' ')))";

this.Connection.Open();
var z = this.Connection.Query("SELECT TABLE_NAME,COLUMN_NAME,DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME in (SELECT distinct TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME like 'Pirvelckaro%' and COLUMN_NAME='SourceDataId') order by TABLE_NAME, COLUMN_NAME")
	.Select (x => new {TABLE_NAME=(string)x.TABLE_NAME,COLUMN_NAME=(string)x.COLUMN_NAME,DATA_TYPE=((string)x.DATA_TYPE).ToLower()})
	.OrderBy (x => x.TABLE_NAME)
	.Where (x => x.DATA_TYPE == "nvarchar" || x.DATA_TYPE == "varchar" || x.DATA_TYPE == "nchar" || x.DATA_TYPE == "char")
	.Select(x => new {x.TABLE_NAME, x.COLUMN_NAME, IsUnicode=x.DATA_TYPE == "nvarchar" || x.DATA_TYPE == "nchar" })
	//.Where(x=>dta.Any (d => d.tn==x.TABLE_NAME && d.cn==x.COLUMN_NAME))
    ;
var xs = z
.GroupBy (x => x.TABLE_NAME)
.Select (g => new {
		TABLE_NAME=g.Key,
		Sql="\n\nPRINT '"+g.Key+"'\nupdate s\nSET "+string.Join(", ", g.Select (x => "[" + x.COLUMN_NAME + "] = "+gaakhvie(x.COLUMN_NAME,x.IsUnicode)))+"\n,s.Gakrechilia=1 \n--select count(*)\nfrom ["+g.Key+"] s where Gakrechilia is null \ngo"
});
string.Join(" UNION ALL\n",xs.Select (x => "select '"+x.TABLE_NAME+"' tname, year(RecDate)*100+month(RecDate) periodi  FROM ["+x.TABLE_NAME+"] WHERE RecDate>='20130701'")).Dump();
foreach (var x in xs)
{
	x.Sql.Dump();
	//this.Connection.Execute(x.Sql, null, null, 9999);
}

return;


var svr = Ex.Triton;
this.Connection.Open();
Func<string,object> evalSqlExpression = exp => this.Connection.Query("select "+exp+" as ex").Cast<IDictionary<string,object>>().First ().Values.First ();

var pc = new PirvelckarosCkhrilebi(svr);

pc.MomeKvela().Select (p => string.Format("update sd set sd.Unnom=p.Unnom from {0} p join Source_Data sd on sd.Id=p.SourceDataId where sd.Unnom is null",p.Name)).Dump();

return;



1.Dump();
var pirvelckarosCkhrilebi = (from  t in Ex.Triton.Databases["Pirvelckaroebi"].Tables()
							where t.Name.StartsWith("Pirvelckaro_") && !t.Name.Contains("_20_")
							select t).ToList();
foreach (var t in from  t in pirvelckarosCkhrilebi
				  where !t.Indexes().Where (i => i.IndexedColumns.Count == 1).Any(i => i.IndexedColumns.Contains("RecDate"))
		          select t)
{
	t.CreateIndexOn("RecDate");
}
		
return;

//foreach (var t in from  t in pirvelckarosCkhrilebi where !t.Columns.Contains("RecDate") select t)
//{
//	t.AddColumun(c => { c.Name="Rai"; c.DataType=DataType.NVarChar(4); });
//	t.Alter();
//}
foreach (var t in from  t in pirvelckarosCkhrilebi
				  where !t.Indexes().Where (i => i.IndexedColumns.Count == 1).Any(i => i.IndexedColumns.Contains("RecDate"))
		          select t)
{
    t.Name.Dump("indexed rai");
	t.CreateIndexOn("Rai");
}

foreach (var t in from  t in pirvelckarosCkhrilebi where !t.Columns.Contains("Gakrechilia") select t)
{
	t.AddColumun(c => { c.Name="Gakrechilia"; c.DataType=DataType.Bit; });
	t.Alter();
}
foreach (var t in from  t in pirvelckarosCkhrilebi
				  where !t.Indexes().Where (i => i.IndexedColumns.Count == 1).Any(i => i.IndexedColumns.Contains("Gakrechilia"))
		          select t)
{
	t.CreateIndexOn("Gakrechilia");
}




foreach (var t in from  t in pirvelckarosCkhrilebi
				  where !t.Indexes().Where (i => i.IndexedColumns.Count == 1).Any(i => i.IndexedColumns.Contains("Gakrechilia"))
		          select t)
{
	t.CreateIndexOn("Gakrechilia");
}

foreach (var t in from  t in pirvelckarosCkhrilebi where !t.Columns.Contains("Unnom") select t )
	t.AddColumun(c => {c.Name="Unnom";c.DataType=DataType.Int;})
	 .Alter();
foreach (var t in from  t in pirvelckarosCkhrilebi
				  where !t.Indexes().Where (i => i.IndexedColumns.Count == 1).Any(i => i.IndexedColumns.Contains("Unnom"))
		          select t)
{
	t.Name.Dump();
	t.CreateIndexOn("Unnom");
}


return ;

this.Connection.Open();
	var dta=
@"Pirvelckaro_20_GANCXADEBEBI dokumentis_coderaion 
Pirvelckaro_21_SAPENSIO_ASAKIS_MOSAXLEOBA PRIVATE_NUMBER 
Pirvelckaro_23_BAVSHVEBI(165) PRIVATE_NUMBER 
Pirvelckaro_24_INVALIDI_BAVSHVEBI PiradiNomeri 
Pirvelckaro_24_INVALIDI_BAVSHVEBI MeurvisPiradiNoneri 
Pirvelckaro_25_MKVETRAD_GAMOXATULI_INVALIDI_BAVSHVEBI PiradiNomeri 
Pirvelckaro_25_MKVETRAD_GAMOXATULI_INVALIDI_BAVSHVEBI MeurvisPiradiNoneri 
Pirvelckaro_26_ARASAQARTVELOS_MOQALAQE_PENSIONREBI PiradiNomeri 
Pirvelckaro_26_ARASAQARTVELOS_MOQALAQE_PENSIONREBI MeurvisPiradiNoneri 
Pirvelckaro_27_AXALSHOBILEBI(165) PRIVATE_NUMBER "
.Split('\n')
.Select (x => x.Split(' '))
.Select (x => new { tn=x[0].Trim(), cn=x[1].Trim() })
.ToList()
.Dump()
;






return;

(
from d in Ex.Triton.Databases()
from file in d.LogFiles.Cast<LogFile>()
select new {d.Name,file.FileName,Size=file.Size}
).Concat
(
from d in Ex.Triton.Databases()
from fg in d.FileGroups.Cast<FileGroup>()
from file in fg.Files.Cast<DataFile>()
select new {d.Name,file.FileName,Size=file.Size}
)
.GroupBy (x => new {x.Name,Disk=x.FileName.Substring(0,1).ToUpper()})
.Select (g => new {g.Key.Name,g.Key.Disk,Size=g.Sum (x => x.Size)/1024.0/1024.0,Files=string.Join(", ",g.Select (x => x.FileName))})
.OrderByDescending (x => x.Size)
.Dump();

;

var kvelaVeli = new []{"Base_Type","FID","PID","First_Name","Last_Name","Birth_Date","Sex","Rai_Name","City","Village","Street","Full_Address","Dacesebuleba","Dac_Region_Name","Dac_Rai_Name","Dac_City","Dac_Village","Dac_Full_Address"};
var map = (new []
	{
	 	new []{ "Pirvelckaroebi.dbo.Pirvelckaro_04_REINTEGRACIA", "C_PID",      "PID"},
		new []{ "Pirvelckaroebi.dbo.Pirvelckaro_04_REINTEGRACIA", "C_F_NAME",   "First_Name"},
		new []{ "Pirvelckaroebi.dbo.Pirvelckaro_04_REINTEGRACIA", "C_L_NAME",   "Last_Name"},
		new []{ "Pirvelckaroebi.dbo.Pirvelckaro_04_REINTEGRACIA", "C_BIRTH_DA", "Birth_Date"},
		new []{ "Pirvelckaroebi.dbo.Pirvelckaro_04_REINTEGRACIA", "RANAM",      "Rai_Name"},
		new []{ "Pirvelckaroebi.dbo.Pirvelckaro_04_REINTEGRACIA", "SOFELI",     "Village"},
		new []{ "Pirvelckaroebi.dbo.Pirvelckaro_04_REINTEGRACIA", "isnull(MISAM+'' '', '''')+isnull(SAXLI+'', '', '''')+isnull(BINA+'', '', '''')","Full_Address"},
	})
	.GroupBy (x => x[0])
	.ToDictionary (x => x.Key,cols=>cols.Select (v => Tuple.Create(v[1], v[2])))
	.Dump();
	

	
	
return;



}

public static class Ex
{
	public static IEnumerable<Database> Databases(this Server src) 
	{
		return src.Databases.Cast<Database>();
	}	

	public static IEnumerable<Table> Tables(this Database src) 
	{
		return src.Tables.Cast<Table>();
	}	
	public static IEnumerable<View> Views(this Database src) 
	{
		return src.Views.Cast<View>();
	}	

	public static IEnumerable<Column> Columns(this Table src) 
	{
		return src.Columns.Cast<Column>();
	}	

	public static IEnumerable<Index> Indexes(this Table src) 
	{
		return src.Indexes.Cast<Index>();
	}	

	public static IEnumerable<IndexedColumn> IndexedColumns(this Index src) 
	{
		return src.IndexedColumns.Cast<IndexedColumn>();
	}

	public static Table AddColumun(this Table tb, params Action<Column>[] colBulders) 
	{
		foreach(var colB in colBulders)
		{
			var col=new Column(){Parent=tb};
			colB (col);
			tb.Columns.Add(col);
		}
		return tb;
	}
	
	public static void CreateIndexOn(this Table tb, params string[] columuns) {
		Index idx;
		idx = new Index(tb, "IX_"+tb.Name+"_"+string.Join("_", columuns));

		foreach(var ic  in columuns.Select (c => new IndexedColumn(idx, c, false))){
			idx.IndexedColumns.Add(ic);
		}
		idx.IsClustered = false;
		idx.Create();
	}
	
	private static readonly Lazy<Server> lazy = new Lazy<Server>(() => {
			var conn = new ServerConnection("triton", "sa", "ssa$20");
    		var srv = new Server(conn);
			return srv;
			});
    
    public static Server Triton { get { return lazy.Value; } }
}