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
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

void Main()
{
	var araIdent=new []{3,4,6,7,11,12}.ToList();
	Source_Data_Kavshiris
		.Where (k => k.FromSource_Data.UnnomisKhariskhi!=99)
		.Where (k => k.FromSource_Data.Unnom == null)
		.Where (k => k.Source_Data.Unnom != null)
		.GroupBy (x => x.From)
		.SelectMany (idg => idg.GroupBy (x => x.FromSauketesoKhariskhi)
							.Where (skg  => skg.Key==idg.Min (x => x.FromSauketesoKhariskhi))
							.SelectMany (skg => skg.GroupBy (x => x.Source_Data.UnnomisKhariskhi)
												.Where (ukg => ukg.Key==skg.Min(x => x.Source_Data.UnnomisKhariskhi))
												.SelectMany(ukg => ukg.GroupBy (x => x.Source_Data.Unnom)
																	.Where (mug => mug.Key == ukg.Max (x_ => x_.Source_Data.Unnom))
																	.SelectMany (mug => mug.GroupBy (x => x.To)
																						.Where (idtg => idtg.Key==mug.Max (x => x.To))
																						.SelectMany (idtg =>idtg.Select (
																												k => new
																													{
																														k.From,
																														k.To,
																														FromBase_Type=k.FromSource_Data.Base_Type,
																														ToBase_Type=k.Source_Data.Base_Type,
																														
																														FromJ_ID=k.FromSource_Data.J_ID,
																														ToJ_ID=k.Source_Data.J_ID,
																														
																														k.FromSauketesoKhariskhi,
																														
																														k.Source_Data.UnnomisKhariskhi,
																														k.Source_Data.Unnom,
																														
																														ArisKargiUnnomi=ukg.Sum (x => x.Source_Data.Unnom)/ukg.Count()==mug.Key || ukg.Key > 2,
																													}
																														)
																									)
																				)
															)
										)
					)
		.Where (x => Source_Data_Sauketeso_Kavshiris.All(x_=>x_.From!=x.From))
		.Take(10).Dump();
//	.Where (sdsk => sdsk.FromBase_Type==1 )
//	.Where (sdsk => sdsk.FromJ_ID != null)
//		.Count().Dump();
}

public static class Ex
{
	public static IQueryable<Source_Data_Kavshiri> WhereIckebaUnomisGaresheChanaceridan(this IQueryable<Source_Data_Kavshiri> s){
		return s;
	}
	
	public static IQueryable<Source_Data_Kavshiri> WhereMtavrdebaUnomianiChanacerit(this IQueryable<Source_Data_Kavshiri> s){
		return s.Where (k => k.Source_Data.Unnom != null);
	}
}
// Define other methods and classes here
