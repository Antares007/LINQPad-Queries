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
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Text</Namespace>
</Query>

var chasascorebeli = GansakhilveliGanacxadebis
	.Where (vgg => vgg.Base_Type!=1)
	.Where (vgg => vgg.FID.Length>2).GroupBy (vgg => vgg.FID)
	.Select (g => new {	g.Key,
						Misamartebi = g.Select (x => new {x.DamzgvevisRaioni,x.DamzgvevisSruliMisamarti,g.Key})
											.Distinct()
						})
	.Where (x => x.Misamartebi.Count ()>1).OrderByDescending (x => x.Misamartebi.Count ()).AsEnumerable();
var sb = new StringBuilder();
var i=0;
foreach (var f in chasascorebeli)
{	
	var m =f.Misamartebi.OrderByDescending (mi=>mi.DamzgvevisSruliMisamarti.Length).First ();
		sb.AppendFormat("DECLARE @pM{2} NVarChar(1000) = N'{0}';DECLARE @pR{2} NVarChar(1000) = N'{1}';",m.DamzgvevisSruliMisamarti,m.DamzgvevisRaioni,i);
		sb.AppendLine();
		
	sb.AppendFormat("Update S  SET s.DamzgvevisRaioni=@pR{1}, s.DamzgvevisSruliMisamarti=@pM{1} from GansakhilveliGanacxadebi S where s.FID = N'{0}';",f.Key,i);
	sb.AppendLine();
	sb.AppendLine();
	i++;
}
sb.ToString().Dump();