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

var res = VStrukturuladValiduriGanackhadebi.SelectMany (x => Source_Data.Where(x_=> x_.ID == x.ID))
			.Where (f => !f.Unnom.HasValue)
			.Select(f => new {f, us = f.FromSource_Data_Kavshiris
										.Where (k => (k.FromKhariskhi&16)==16)
										.Where (k => f.J_ID.HasValue)
										.Where (k => f.J_ID == k.Source_Data.J_ID)
										
										.Where (k =>  k.Source_Data.Unnom.HasValue)
										.Where(k => k.Source_Data.UnnomisKhariskhi.UnomisKhariskhi!=99)
										.Select (k => k.Source_Data.Unnom)
										.Distinct()})
			.Where (x => x.us.Count() == 1)
			.Select (x => new { x.f, UnnomNew = x.us.First ()});//.Count().Dump();
foreach (var x in res)
{
	x.f.Unnom=x.UnnomNew;
}
SubmitChanges();