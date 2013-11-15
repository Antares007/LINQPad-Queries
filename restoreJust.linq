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
  <NuGetReference>System.Net.FtpClient</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Common</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Dmf</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Smo</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Data.Common</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.FtpClient</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

public IEnumerable<string> GetFileList(string ftpServerIP, string ftpUserID, string ftpPassword)
{
	using (FtpClient conn = new FtpClient()) 
	{
		conn.Host = ftpServerIP;
		conn.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
		return conn.GetListing().Select (c => c.FullName).ToArray();
	}

}
public IEnumerable<FtpListItem> GetFileList2(string ftpServerIP, string ftpUserID, string ftpPassword)
{
	using (FtpClient conn = new FtpClient()) 
	{
		conn.Host = ftpServerIP;
		conn.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
		
		return conn.GetListing("/");
	}

}
public static void DownLoad(string ftpServerIP, string ftpUserID, string ftpPassword, string fileName, string toFile) 
{
	using(FtpClient conn = new FtpClient()) 
	{
		conn.Host = ftpServerIP;
		conn.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
		
		using(Stream istream = conn.OpenRead(fileName)) 
		using(Stream to = new FileStream(toFile,FileMode.Create))
		{
			try 
			{
				CopyStream(istream, to);
			}
			finally {
				istream.Close();
				to.Flush();
				to.Close();
			}
		}
	}
}
public static void CopyStream(Stream input, Stream output)
{
    byte[] buffer = new byte[8 * 1024];
    int len;
    while ( (len = input.Read(buffer, 0, buffer.Length)) > 0)
    {
        output.Write(buffer, 0, len);
    }    
}
void Main()
{
//GetFileList2("localhost","anonymous",null).Dump();
//return;
Restore(@"J:\ShareForBakRestore\pens_dazgveva201311","forDeldazgveva20131101");

//GetFileList("81.95.168.42","moh","(Jan-Dacva)").Dump();
return ;

return;
//var p = Process.Start(@"C:\Program Files\WinRAR\Rar.exe",@"t c:\temp\aa.rar");
//p.WaitForExit();
//p.ExitCode.Dump();

//GetFileList("81.95.168.42","moh","(Jan-Dacva)").Dump();


//DownLoad("81.95.168.42","moh","(Jan-Dacva)","/SSA_130901.rar","c:\\temp\\SSA_130901.rar");


var p = Process.Start("rar.exe","t "+"c:\\temp\\SSA_130901.rar");
p.WaitForExit();
if(p.ExitCode != 0)
File.Delete("c:\\temp\\SSA_130901.rar");


//	Restore(@"J:\ShareForBakRestore\Insurence_SUBS_130801.BAK","For_Delete_Insurence_SUBS_130801");
//	Console.WriteLine("Full Database Restore complete.");
}
void Restore(string bakFile,string dbName=null)
{
	var svr = Ex.Triton;

	if(dbName==null){
		dbName = "For_Delete_" + Path.GetFileNameWithoutExtension(bakFile);
	}

	Restore rs = new Restore
    {
		NoRecovery = false,
		
        Database = dbName,
		Devices = { new BackupDeviceItem(bakFile, DeviceType.File) },
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
		rs.RelocateFiles.Add(new RelocateFile(file.LogicalName, @"J:\DAZGVEVA\" + dbName + file.LogicalName));
	
	rs.SqlRestore(svr);
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