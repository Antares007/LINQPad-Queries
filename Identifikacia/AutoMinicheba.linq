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

var unnoms = Source_Data_Kavshiris 
	.Where (k => k.FromSource_Data.Unnom == null )
	.Where (k => k.FromSource_Data.MapDate>DateTime.Today ||k.FromSource_Data.Source_Rec_Id==null)
	.Where (k => k.Source_Data.Unnom != null)
	.Where (k => k.FromSource_Data.J_ID.HasValue || k.Source_Data.J_ID.HasValue)
	.Select (k => new { 
						ID = k.From, 
						SauketesoKhariskhi = k.FromSauketesoKhariskhi,
						UnomisKhariskhi = k.Source_Data.UnnomisKhariskhi,
						NewUnnom = k.Source_Data.Unnom, 
					})
	.GroupBy (x =>x.ID)
	.SelectMany (skg => skg.Where (x => x.SauketesoKhariskhi==skg.Min (x_ => x_.SauketesoKhariskhi))
							.GroupBy (x => x.ID)
							.SelectMany (ukg => ukg.Where (x => x.UnomisKhariskhi==ukg.Min(x_=>x_.UnomisKhariskhi))
													.Select (x => new {x.NewUnnom,x.ID})
													.Distinct()
													.GroupBy (x => x.ID)
													.SelectMany (ug => ug.Where (x => x.NewUnnom == ug.Max (x_=>x_.NewUnnom))
																			.Select (u => new 
																							{
																								ID=skg.Key,
																								UnomisKhariskhi=ukg.Min(x_=>x_.UnomisKhariskhi),
																								Unnom=ug.Max (x_=>x_.NewUnnom),
																								KavshirisKhariskhi=skg.Min (x_ => x_.SauketesoKhariskhi),
																								UnomebisRaodenoba=ug.Count(),
																								UnomisKharisRaodenoba=skg.Select (x => x.UnomisKhariskhi).Distinct().Count ()
																							})
																)		
										)
				)
	.Select (x => new{	
							x.ID,
							x.KavshirisKhariskhi,
							x.UnomisKhariskhi,
							x.Unnom,
							x.UnomebisRaodenoba,
							x.UnomisKharisRaodenoba,
							SaechvoUnomi = x.UnomisKhariskhi < 3 && x.UnomebisRaodenoba != 1
					})
	.Where (x => x.KavshirisKhariskhi<=8 && !x.SaechvoUnomi)				
	;
unnoms.Dump();
	
//unnoms.SelectMany (u => Source_Data.Where (sd => sd.ID==u.ID && sd.Unnom != u.Unnom)).Dump();