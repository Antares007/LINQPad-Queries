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

var pirobebi=new []{
	new {Khar=2		,	Foto="PID + First_Name_Fst3 + Last_Name_Fst4 + Birth_Date"},
	new {Khar=4		,	Foto="PID + Last_Name_Fst3 + First_Name_Fst4 + Birth_Date"},
	new {Khar=8		,	Foto="PID + First_Name_Fst3 + Birth_Date"},
	new {Khar=16	,	Foto="PID + Last_Name_Fst4 + Birth_Date"},
	new {Khar=32	,	Foto="PID + First_Name_Fst3"},
	new {Khar=64	,	Foto="PID + Last_Name_Fst4"},
	new {Khar=128	,	Foto="PID + FID + Birth_Date"},
	new {Khar=256	,	Foto="PID + Birth_Date"},
	new {Khar=512	,	Foto="PID_Lst4 + First_Name_Fst3 + Last_Name_Fst4 + Birth_Date"},
	new {Khar=1024	,	Foto="PID_Fst9 + First_Name_Fst3 + Last_Name_Fst4 + Birth_Date"},
	new {Khar=2048	,	Foto="DOC_NO_Lst4 + First_Name_Fst3 + Last_Name_Fst4 + Birth_Date"},
	new {Khar=4096	,	Foto="FID + First_Name + Last_Name_Fst4 + Birth_Date"},
	new {Khar=8192 ,	Foto="FID + First_Name + Birth_Date"},
	new {Khar=16384 ,	Foto="FID + First_Name_Fst4 + Birth_Date"},
	new {Khar=32768 ,	Foto="FID + First_Name_Fst3 + Birth_Date"},
	new {Khar=65536 ,	Foto="DOC_NO_Lst3 + First_Name + Last_Name_Fst4 + Birth_Year + Birth_Month"},
	new {Khar=131072 ,	Foto="DOC_NO + First_Name + Last_Name_Fst4 + Birth_Year"}, 
	new {Khar=262144 ,	Foto="DOC_NO + First_Name_Fst3 + Last_Name_Fst4 + Birth_Year"},
	new {Khar=524288 ,	Foto="DOC_NO_Lst4 + FID + Last_Name + Birth_Month"},
	new {Khar=1048576 ,	Foto="First_Name_Fst3 + Last_Name_Fst4 + Birth_Date"},
	new {Khar=2097152 ,	Foto="First_Name_Fst3 + Last_Name_Fst4 + Birth_Day + Birth_Month"},
	new {Khar=4194304 ,	Foto="First_Name_Fst3 + Last_Name_Fst4 + Birth_Year"},
}.ToList();
Func<int,string> getPirobebi= (k)=>
	pirobebi.Aggregate (new System.Text.StringBuilder(), (sb,i) => {
			if((k & i.Khar) == i.Khar){
				sb.AppendFormat("{0}\n",i.Foto);
			}
			return sb;
		}).ToString();





var id = 5426484;
var unnom = 5104885;

Source_Data.First (sd => sd.ID==id).Unnom = unnom;
Source_Data_Unnomis_Minichebas.InsertOnSubmit(new Source_Data_Unnomis_Minicheba{ SourceDataId =id, Unnom=unnom, Dro=DateTime.Now});

var gasanaxkebeliIdebi = Source_Data_Kavshiris.Where(x=>x.From==id)
								.Select (x => x.To).Distinct()
								.AsEnumerable().Concat(new[]{ id }).ToList();
								
Source_Data_Sauketeso_Kavshiris.DeleteAllOnSubmit(Source_Data_Sauketeso_Kavshiris.Where (sdk => gasanaxkebeliIdebi.Contains(sdk.ID)));
var araIdent=new []{3,4,6,7,11,12}.ToList();

var dasamatebeli= Source_Data_Kavshiris
				.Where (k => k.FromSource_Data.Unnom == null )
				.Where (k => k.Source_Data.Unnom != null)
				.Where (k => !Source_Data_Sauketeso_Kavshiris.Any(x=>x.ID==k.From))
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
				.SelectMany (x => Source_Data.Where (sd => sd.ID==x.ID)
				.Select (sd => new{	
										sd.ID,
										sd.Base_Type,
										AraIdent=araIdent.Contains(sd.Base_Type),
										JIDHasValue=sd.J_ID.HasValue,
										x.KavshirisKhariskhi,
										x.UnomisKhariskhi,
										x.Unnom,
										x.UnomebisRaodenoba,
										x.UnomisKharisRaodenoba,
										SaechvoUnomi = x.UnomisKhariskhi < 3 && x.UnomebisRaodenoba != 1
								})).Take(100).Dump();
								.AsEnumerable()
								.Select (x => new Source_Data_Sauketeso_Kavshiri{
										AraIdent=x.AraIdent,
										Base_Type=x.Base_Type,
										JIDHasValue=x.JIDHasValue,
										KavshirisKhariskhi=x.KavshirisKhariskhi,
										SaechvoUnomi=x.SaechvoUnomi,
										Unnom=x.Unnom.Value,
										UnomebisRaodenoba=x.UnomebisRaodenoba,
										UnomisKhariskhi=x.UnomisKhariskhi,
										UnomisKharisRaodenoba=x.UnomisKharisRaodenoba,
										ID=x.ID
										});

Source_Data_Sauketeso_Kavshiris.InsertAllOnSubmit(dasamatebeli);
SubmitChanges();
								
						
						
//		.GroupBy (x => new{x.AraIdent,x.JIDHasValue,x.KavshirisKhariskhi,x.UnomisKhariskhi,x.SaechvoUnomi})
//		.Select (x => new{x.Key,Raod=x.Count()})
//		.Take(10).Dump();
//	
	
	
	//4761139
	//4837227