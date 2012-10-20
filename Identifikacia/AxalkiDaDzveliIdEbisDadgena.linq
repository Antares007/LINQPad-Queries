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

var newId = Source_Data
				.Where (sd => sd.Unnom!=null)
				.Where (sd => sd.UnnomisKhariskhi==1)
				.Where (sd => sd.Base_Type>0)
				
				.GroupBy (sd => sd.Unnom)
				.SelectMany(g => g.Where(sd=> sd.J_ID.HasValue == g.Any (x => x.J_ID.HasValue)))
				
				.GroupBy (sd => sd.Unnom)
				.Select(g => g.Max (sd => sd.ID))
				
				.Take(3)
				.Dump();

var oldId = Source_Data
				.Where (sd => sd.Unnom!=null)
				.Where (sd => sd.Source_Rec_Id<0)
				
				.GroupBy (sd => sd.Unnom)
				.SelectMany(g => g.Where(sd => sd.UnnomisKhariskhi== g.Min (x => x.UnnomisKhariskhi)))
				
				.GroupBy (sd => sd.Unnom)
				.Select(g => g.Max (sd => sd.ID))
				
				.Take(3)
				.Dump();