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

//AraIdent=1&JIDHasValue=0&KavshirisKhariskhi=4194304&SaechvoUnomi=1&UnomisKhariskhi=1
var araIdent=new []{3,4,6,7,11,12}.ToList();

Source_Data_Kavshiris
	.Where (sdk => !araIdent.Contains(sdk.FromSource_Data.Base_Type))
	.Where (sdk => sdk.FromSource_Data.J_ID == null)
	.Where (sdk => sdk.Source_Data.UnnomisKhariskhi==1)

	.Where (sdk => sdk.FromSource_Data.Unnom==null)
	.Where (sdk => sdk.Source_Data.Unnom!=null)
	.GroupBy (sdk => sdk.From)
	.SelectMany (g => g.Where(x =>x.FromSauketesoKhariskhi == g.Min (x_ => x_.FromSauketesoKhariskhi))
	
	
	)
	
	//.Where(g => g.Min (x_ => x_.FromSauketesoKhariskhi)==1)
	.Where(g => g.Select (x => x.Source_Data.Unnom).Distinct().Count ()==1)
	.Count().Dump();
//	.SelectMany (g => g.SelectMany (k => Source_Data.Where (sd => sd.ID == k.Source_Data.ID).Select (sd => new {FromId=g.Key, ToId=sd.ID})))
//	.Dump();