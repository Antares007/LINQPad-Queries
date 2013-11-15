<Query Kind="Program">
  <Namespace>System.Data.OleDb</Namespace>
  <Namespace>System.Data.Odbc</Namespace>
  <Namespace>System.Data.Common</Namespace>
</Query>

void Main()
{


	foreach(var file in DirSearch(@"Z:\სადაზღვევოებს\","*.accdb").Select (i => new FileInfo(i)).Where (i => i.CreationTime>=DateTime.Parse("2013-08-01")).OrderBy (i => i.CreationTimeUtc).Select (i => i.FullName))
	{
		using(var conn = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=\"" + file +"\""))
		{
			conn.Open();
			
			string[] restrictions = new string[4];
			restrictions[3] = "Table";
			var userTables = conn.GetSchema("Tables", restrictions);
			foreach (var tableName in userTables.Rows.Cast<DataRow>().Select (dr => (string)dr["TABLE_NAME"]))
			{
				using(var command = new OleDbCommand("SELECT * FROM ["+tableName+"];",conn))
				{
					command.CommandType = CommandType.Text;
					using(var reader = command.ExecuteReader())
					using(var schemaTable = reader.GetSchemaTable())
					{
						var pidColName = schemaTable.Rows.Cast<DataRow>().Select (dr => (string)dr["ColumnName"]).FirstOrDefault (dr => dr.ToLowerInvariant() == "pid");
						if(pidColName  == null)
							continue;
						using(var command2 = new OleDbCommand("SELECT * FROM ["+tableName+"] WHERE [" + pidColName + "] in (\"01850039215\",\"01650036897\",\"24001002250\");",conn))
						{
							command2.CommandType = CommandType.Text;
							using(var reader2 = command2.ExecuteReader())
							{
								if(reader2.HasRows){
									file.Dump();
									reader2.Dump();
								}
							}
						}
					}
				}
			}
			conn.Close();
		}
	}

}
IEnumerable<string> DirSearch(string sDir, string searchPattern="*.xls") 
{
	if(File.Exists(sDir)) {
		yield return sDir;
	}

	if(Directory.Exists(sDir)) {
		foreach(var file in Directory.GetFiles(sDir, searchPattern)) {
			yield return file;
		}
		foreach(var dir in Directory.GetDirectories(sDir)) {
			foreach(var file in DirSearch(dir, searchPattern)){
				yield return file;
			}
		}
	}
}

// Define other methods and classes here
