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
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\100\SDK\Assemblies\Microsoft.SqlServer.ConnectionInfo.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\100\SDK\Assemblies\Microsoft.SqlServer.Management.Sdk.Sfc.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\100\SDK\Assemblies\Microsoft.SqlServer.Smo.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\100\SDK\Assemblies\Microsoft.SqlServer.SmoExtended.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\100\SDK\Assemblies\Microsoft.SqlServer.SqlEnum.dll</Reference>
  <NuGetReference>Dapper</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Common</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Dmf</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Smo</Namespace>
  <Namespace>System.Data.Common</Namespace>
</Query>

void Main()
{

Func<string,bool,string> gaakhvie = (s,isUnicode) =>
@"ltrim(rtrim(replace(replace(replace(replace(replace(replace(replace(replace(replace("+(isUnicode?"Pirvelckaroebi.dbo.fn_con2utf8([" + s + @"])":"[" + s + @"]")+",nchar(10),' '),nchar(13),' '),nchar(9),' '),'   ',' '),'   ',' '),'   ',' '),'  ',' '),'  ',' '),'  ',' ')))";

this.Connection.Open();
var z = this.Connection.Query("SELECT TABLE_NAME,COLUMN_NAME,DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME like 'Pirvelckaro%'")
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
		Sql="\n\nupdate s\nSET "+string.Join("\n, ", g.Select (x => "[" + x.COLUMN_NAME + "] = "+gaakhvie(x.COLUMN_NAME,x.IsUnicode)))+"\n,s.Gakrechilia=1 \n--select count(*)\nfrom ["+g.Key+"] s where Gakrechilia is null \ngo"
});
foreach (var x in xs)
{
	x.Sql.Dump();
	//this.Connection.Execute(x.Sql, null, null, 9999);
}

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