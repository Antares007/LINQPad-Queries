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

//VStrukturuladValiduriGanackhadebi.SelectMany (x => Source_Data.Where(x_=> x_.ID == x.ID))
VStrukturuladValiduriGanackhadebi.SelectMany (x => Source_Data.Where(x_=> x_.ID == x.ID))
	.Where(x => !x.Unnom.HasValue)
	.Where(x => x.Source_Rec_Id>0)
	//.Where (x => x.FromSource_Data_Kavshiris.Any (fsdk => fsdk.Source_Data.UnnomisKhariskhi.UnomisKhariskhi!=99))
	.Where (x => x.FromSource_Data_Kavshiris.Any (fsdk => (fsdk.FromKhariskhi&4)== 4 && (fsdk.ToKhariskhi&4)!= 4))
	.OrderByDescending (x => x.FromSource_Data_Kavshiris
								.Where (fsdk => fsdk.Source_Data.UnnomisKhariskhi.UnomisKhariskhi!=99)
								.Where (fsdk => fsdk.Source_Data.Unnom.HasValue).Count ())
	.Select(x=>new {x,	sd=x.FromSource_Data_Kavshiris
									.Where (fsdk => fsdk.Source_Data.Unnom.HasValue)
									.Where (fsdk => fsdk.Source_Data.UnnomisKhariskhi.UnomisKhariskhi!=99)
									.Select (k => k.Source_Data)
									.Select (k => new {	Unnom=k.Unnom.ToString(),Khar=k.UnnomisKhariskhi.UnomisKhariskhi.ToString(),k.J_ID,k.Piroba,
														k.PID,k.FID,k.First_Name,k.Last_Name,k.Birth_Date,k.Pirvelckaro,
														Lnk="Source_Data.First (sd => sd.ID=="+x.ID+").Unnom="+k.Unnom+";",
														})
					})	.Take(5)	.Dump();
return;
SubmitChanges();


Source_Data
	.Where (x => !x.Unnom.HasValue && x.MapDate.HasValue)
	.Select(sdk => new {sdk,Unnoms=sdk.FromSource_Data_Kavshiris
							.Where (s => (s.FromKhariskhi&2)==2)
							.Where (s =>  s.Source_Data.Unnom!=null)
							.Select (s => s.Source_Data.Unnom)
							.Distinct()})
	.Where (sdk => sdk.Unnoms.Count ()==1)
	.Select(x=>new {x.sdk,Unnom=x.Unnoms.First ()})
	.Take(10).Dump();

Source_Data
	.Where (sd => sd.MapDate.HasValue)
	.Where (sd => sd.UnnomisKhariskhi.UnomisKhariskhi==99)
	.Dump();