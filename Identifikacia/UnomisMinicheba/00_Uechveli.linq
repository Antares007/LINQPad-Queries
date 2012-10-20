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
			.Where (f => !f.Unnom.HasValue && f.Piroba.HasValue)
			.Select(f => new {f, us = f.FromSource_Data_Kavshiris
										.Where (k => (k.FromKhariskhi&(2+8+16+32+64+128+256))!=0)
										.Where (k => f.J_ID.HasValue)
										.Where (k => f.J_ID == k.Source_Data.J_ID)
										
										.Where (k =>  k.Source_Data.Unnom.HasValue)
										.Where(k => k.Source_Data.UnnomisKhariskhi.UnomisKhariskhi!=99)
										.Select (k => k.Source_Data)})
										.Select (x => new {
														x.f,
														x.us,
														U3=x.us.Where(k => k.UnnomisKhariskhi.UnomisKhariskhi==3 || k.UnnomisKhariskhi.UnomisKhariskhi==4 )
																	.OrderByDescending (k => k.Periodi)
																	.Select (k=>k.Unnom)
																	.First (),
														U1=x.us.Where(k => k.UnnomisKhariskhi.UnomisKhariskhi==1 )
																	.Select (k=>k.Unnom)
																	.Distinct()
											})
			.Where (x => x.U3.HasValue || x.U1.Distinct().Count ()==1)
			.Select (x => new {x.f, newUnnom=x.U3.HasValue ? x.U3 : x.U1.First ()}).Take(1).Dump();
foreach (var x in res)
{
	x.f.Unnom=x.newUnnom;
}
//SubmitChanges();