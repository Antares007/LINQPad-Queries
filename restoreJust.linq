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
  <Namespace>System.Collections.Concurrent</Namespace>
</Query>

void Main()
{
	var svr = Ex.Triton;
	
//	svr.KillAllProcesses("ForDeleteJUST20121210");
//	svr.KillDatabase("ForDeleteJUST20121210");
//	svr.Databases["ForDeleteJUST20121210"].Drop();
//
//	return;

	Restore rs = new Restore
    {
		NoRecovery = false,
		Database = "ForDeleteInsurence_SUBS20130101",
		Devices = { new BackupDeviceItem(@"J:\ShareForBakRestore\Insurence_SUBS_130101.BAK", DeviceType.File) },
		Action = RestoreActionType.Database,
		ReplaceDatabase = true,
		PercentCompleteNotification = 10,
	};
	rs.PercentComplete += (s, p) => p.Percent.Dump();
	
	var files = rs.ReadFileList(svr).Rows.Cast<DataRow>()
				  .Select (r => new {LogicalName=r["LogicalName"].ToString(),
				                     PhysicalName=Path.GetFileName(r["PhysicalName"].ToString())
									});
	foreach(var file in files)
		rs.RelocateFiles.Add(new RelocateFile(file.LogicalName, @"J:\ShareForBakRestore\SMOSSA"+file.LogicalName));
	
	rs.SqlRestore(svr);
	Console.WriteLine("Full Database Restore complete.");
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