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

var tipebiUnomisGareshe = Source_Data.Where (tug => tug.Unnom == null && tug.FID != null);
var unomianiTipebi      = Source_Data.Where (tug => tug.Unnom != null && tug.FID != null);
tipebiUnomisGareshe
	.SelectMany (tug => unomianiTipebi.Where (ut => ut.FID == tug.FID)
								.SelectMany (ioc => unomianiTipebi
								.Where(ut => ut.FID != ioc.FID && (ut.Unnom==ioc.Unnom || ut.J_ID==ioc.J_ID)))
									.SelectMany (msoc => unomianiTipebi.Where (ut => ut.FID == msoc.FID && ut.ID != msoc.ID))
									.Where(soc => (soc.J_ID == null || tug.J_ID==null) &&
												  soc.First_Name.Substring(0,3)==tug.First_Name.Substring(0,3) && 
												  soc.Last_Name .Substring(0,4)==tug.Last_Name .Substring(0,4) && 
												  soc.Birth_Date == tug.Birth_Date)
									.Select (soc => new 
													{
														tug.ID,
														UnomisKhariskhi = soc.UnnomisKhariskhi != null ?soc.UnnomisKhariskhi.UnomisKhariskhi:1,
														NewUnnom = soc.Unnom,
														ToId = soc.ID
													}));
	ForDelete3s										
	.GroupBy (x => x.ID)
	.Select (g => new 
					{
					ID  = g.Key, 
					U1  = (int?)g.Where(x => x.UnomisKhariskhi == 1).Select (x => x.NewUnnom).FirstOrDefault(),
					U1R = g.Where(x => x.UnomisKhariskhi == 1).Select (x => x.NewUnnom).Distinct().Count(),
					U2  = (int?)g.Where(x => x.UnomisKhariskhi == 2).Select (x => x.NewUnnom).FirstOrDefault (),
					U2R = g.Where(x => x.UnomisKhariskhi == 2).Select (x => x.NewUnnom).Distinct().Count(),
					U34 = (int?)g.Where(x => x.UnomisKhariskhi == 3 || x.UnomisKhariskhi == 4).Select (x => x.NewUnnom).OrderByDescending(x => x).FirstOrDefault (),
					})
	.Select (x => new {x.ID,NewUnnom = x.U1R == 1 ? x.U1 : 
														x.U1R > 1 ? null :
															x.U2R == 1 ? x.U2 :
																x.U2R > 1 ? null :
																	x.U34})
	
	.Where (x => x.NewUnnom!=null)
	.Dump();
