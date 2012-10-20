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
</Query>

while(true)
{
var cmd=GansakhilveliGanacxadebis
	.Where (gg => gg.Base_Type!=1)
	.Select (gg => gg.DamzgvevisSruliMisamarti)
	.Distinct()
	.AsEnumerable()
	
	.Select (gg => new {
		DamzgvevisSruliMisamarti=gg,
		NewAddresses=gg.Split(new string[]{"; "},StringSplitOptions.RemoveEmptyEntries)
		})
	.Select (gg => new {
		gg.DamzgvevisSruliMisamarti,
		gg.NewAddresses,
		NewAddressesDistinct=gg.NewAddresses.Distinct()
	})
	.Where (gg => gg.NewAddresses.Count() != gg.NewAddressesDistinct.Count())
	.Take(1000)
	.Select (gg => new {OldAddress=gg.DamzgvevisSruliMisamarti,NewAddress=string.Join("; ",gg.NewAddressesDistinct)})
	.Aggregate (new System.Text.StringBuilder(),(sb,m)=>{
		sb.AppendFormat("update GansakhilveliGanacxadebi set DamzgvevisSruliMisamarti=N'{0}' WHERE DamzgvevisSruliMisamarti=N'{1}';\n",m.NewAddress.Replace("'","''"),m.OldAddress.Replace("'","''"));
		return sb;
	})
	.ToString();
	cmd.Length.Dump();
	var res=ExecuteCommand(cmd);
if(res==0) return;
};