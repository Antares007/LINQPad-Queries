<Query Kind="Program">
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
  <Reference>C:\Projects\lokad-cqrs\Core\Lokad.Cqrs.Portable\bin\Debug\Lokad.Cqrs.Portable.dll</Reference>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>Lokad.Cqrs</Namespace>
</Query>

void Main()
{
	const string integrationPath = @"C:\Temp\";
	var config = FileStorage.CreateConfig(integrationPath, "files");
	var nuclear=config.CreateNuclear("store");
	
	List<Sakhe> dt=nuclear.GetSingleton<List<Sakhe>>().Value;
	dt.Take(10).Dump();
	
//	var testData= Source_Data.Where (sd => sd.Unnom.HasValue)
//				.Select (sd => new {
//							sd.Unnom,
//							sd.J_ID,
//							sd.Base_Type,
//							sd.FID,
//							sd.PID, sd.First_Name, sd.Last_Name, sd.Birth_Date,
//							sd.Rai_Name, sd.City, sd.Village, sd.Full_Address,
//							sd.Dacesebuleba,
//							sd.Dac_Rai_Name, sd.Dac_City, sd.Dac_Village, sd.Dac_Full_Address
//						})
//				.Distinct()
//	
//				.GroupBy (sd => sd.Unnom)
//				.OrderByDescending (g => g.Count ())
//				
//				.Take(1000)
//				.SelectMany (f => f)
//				.AsEnumerable()
//				.Select (x => new Sakhe{
//										Jid=x.J_ID, BaseType=x.Base_Type, Fid = x.FID, 
//										Pid = x.PID, FirstName = x.First_Name, LastName = x.Last_Name, BirthDate=x.Birth_Date.HasValue?x.Birth_Date.Value.ToString("yyyy-MM-dd"):null,
//										RaiName = x.Rai_Name, City = x.City, Village = x.Village, FullAddress = x.Full_Address,
//										Dacesebuleba = x.Dacesebuleba,
//										DacRaiName=x.Dac_Rai_Name, DacCity=x.Dac_City, DacVillage = x.Dac_Village, DacFullAddress = x.Dac_Full_Address
//										})
//				.ToList();
//					
//	nuclear.TryDeleteSingleton<List<Sakhe>>();
//	nuclear.AddOrUpdateSingleton(()=>testData, s=>s);
}

public class Sakhe 
{
	public int? Jid {get; set;}
	public int BaseType {get; set;}
	
	public string Fid {get; set;}
	public string Pid {get; set;}
	public string FirstName {get; set;}
	public string LastName {get; set;}
	public string BirthDate {get; set;}

	public string RaiName {get; set;}
	public string City {get; set;}
	public string Village {get; set;}
	public string FullAddress {get; set;}
	
	public string Dacesebuleba {get; set;}
	
	public string DacRaiName {get; set;}
	public string DacCity {get; set;}
	public string DacVillage {get; set;}
	public string DacFullAddress {get; set;}
}