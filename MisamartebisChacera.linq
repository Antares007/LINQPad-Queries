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

//var misamartebi = 
//	VGansakhilveliGanacxadebi
//	.Where (sd => sd.Source_Rec_Id > 0)
//	.Where (sd => sd.Unnom != null)
//	.Where (sd => sd.Base_Type == 1 || sd.Base_Type == 2)
//	//.Where (sd => OjakhisMisamartebis.All(m=>m.FID != sd.FID))
//	.Select (sd => new {sd.FID,sd.Rai,sd.DamzgvevisRaioni,sd.DamzgvevisKalaki,sd.DamzgvevisSofeli,sd.DamzgvevisSruliMisamarti})
//	.Distinct()
//	.Take(1).Dump();
//	MISTEMPs
//	.Select (sd => new {sd,((sd.Rai??"")+(sd.DamzgvevisRaioni ?? "")+(sd.DamzgvevisKalaki??"")+(sd.DamzgvevisSofeli??"")+(sd.DamzgvevisSruliMisamarti??"")).Length})
//	.Take(1)
//	.Dump();
var misamartebi = 
MISTEMP2s
	.Where (sd => OjakhisMisamartebis.All(m=>m.FID != sd.FID))
	.GroupBy (sd => sd.FID)
	.Select (g => g.First(x => x.Length==g.Max (x_ => x_.Length)))
	.Take(10000)
	.AsEnumerable()
	.Select (x => new OjakhisMisamartebi
	{
		FID=x.FID,
		Rai=x.Rai,
		DamzgvevisRaioni=x.DamzgvevisRaioni,
		DamzgvevisKalaki=x.DamzgvevisKalaki,
		DamzgvevisSofeli=x.DamzgvevisSofeli,
		DamzgvevisSruliMisamarti=string.Join("; ",x.DamzgvevisSruliMisamarti.Split(new string[]{"; "},StringSplitOptions.RemoveEmptyEntries).Distinct()),
		Dro=DateTime.Now,
	
	});
while (true)
{
	var todo=misamartebi.ToList();
	if(todo.Count==0) break;
	OjakhisMisamartebis.InsertAllOnSubmit(todo);
	SubmitChanges();
}
	
//	
//	.Where (g => g.Count ()>1)
//	.Count ()
//