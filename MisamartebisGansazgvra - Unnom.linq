<Query Kind="Statements">
  <Connection>
    <ID>8c49975d-7243-4e66-bf5a-92b155c73773</ID>
    <Persist>true</Persist>
    <Server>MegaMozg</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA3XCANzX8A0+HtkxmZtXFGAAAAAACAAAAAAAQZgAAAAEAACAAAACwZJTj6RI6G9b/trIDVOasBfrbl2/EeQ2symplXOxXjgAAAAAOgAAAAAIAACAAAACXcRvebWOVOdKm5bohQ6HFL7D5ZCdnJk1K+ulOvqgM9RAAAADzUWAKzvQZ+zIgoC2VCkD9QAAAAEjICu0vvEQYXF3gevv5l9vF9oZmUg9AyecZZw5eTwc1lDjBuJgZDUtRsZwzbxxaX1vttFBABp76hg+1Cy00Dq0=</Password>
    <Database>Pirvelckaroebi</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>C:\Temp\EPPlus.dll</Reference>
  <Reference>C:\Projects\PolisebisDarigebaWeb\packages\Ix_Experimental-Main.1.1.10823\lib\Net4\System.Interactive.dll</Reference>
  <GACReference>WebMatrix.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35</GACReference>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

var sequence=VGansakhilveliGanacxadebi
	.Where (sd => sd.MapDate>DateTime.Parse("2012-04-04"))
	.Where (sd => sd.Base_Type!=1 && sd.Base_Type!=2 && sd.Base_Type!=10)
	.Select (x => new { x.Unnom,x.PID, x.Rai, x.DamzgvevisRaioni, x.DamzgvevisKalaki, x.DamzgvevisSofeli, x.DamzgvevisSruliMisamarti,
										x.DamkhmareRaioni,x.DamkhmareKalaki,x.DamkhmareSofeli,x.DamkhmareMisamarti})
	.AsEnumerable().AsParallel()
	.GroupBy (sd => sd.FID)
	.Select (g => g.First (sd => sd.DamzgvevisSruliMisamarti.Length == g.Max (x => x.DamzgvevisSruliMisamarti.Length)))
	.Select (x => new {
		x.FID,
		x.Rai,
		x.DamzgvevisRaioni,
		x.DamzgvevisKalaki,
		x.DamzgvevisSofeli,
		DamzgvevisSruliMisamarti=string.Join("; ",x.DamzgvevisSruliMisamarti.Split(new string[]{"; "}, StringSplitOptions.RemoveEmptyEntries).Distinct())
	})
	.Buffer(128)
	.Select (
		b => b.Aggregate (new System.Text.StringBuilder(), 
		(sb,m) => {
			sb.AppendLine("INSERT INTO [dbo].[OjakhisMisamartebi] ([FID],[Rai],[DamzgvevisRaioni],[DamzgvevisKalaki],[DamzgvevisSofeli],[DamzgvevisSruliMisamarti],[Dro])");
			sb.AppendLine(string.Format("VALUES ({0}, {1}, {2}, {3}, {4}, {5}, GETDATE())",
									"'"+m.Un+"'",
									"'"+m.PID+"'",
									"'"+m.Rai+"'",
									m.DamzgvevisRaioni==null?"null":"N'"+m.DamzgvevisRaioni.Replace("'","''")+"'",
									m.DamzgvevisKalaki==null?"null":"N'"+m.DamzgvevisKalaki.Replace("'","''")+"'",
									m.DamzgvevisSofeli==null?"null":"N'"+m.DamzgvevisSofeli.Replace("'","''")+"'",
									m.DamzgvevisSruliMisamarti==null?"null":"N'"+m.DamzgvevisSruliMisamarti.Replace("'","''")+"'"
									));
			return sb;
			})
			.ToString())
	;
var db = WebMatrix.Data.Database.OpenConnectionString(this.Connection.ConnectionString,"System.Data.SqlClient");
var i =0;
foreach (var sql in sequence)
{
	db.Execute(sql);
	if(i%128==0) i.Dump();
	i++;
}