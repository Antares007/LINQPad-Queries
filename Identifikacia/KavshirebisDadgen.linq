<Query Kind="Expression">
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

Source_Data_Fotos
	.SelectMany (f => Source_Data_Fotos
								.Where (t => t.ID>0)
								.Where (t => f.Foto == t.Foto && f.ID != t.ID)
									   .Select (t => new {
														From=f.ID,
														FromKhariskhi=f.Khariskhi,
														ToKhariskhi=t.Khariskhi, 
														To=t.ID
														}
												)
				)
	.GroupBy (x => new {x.From,x.To})
	.SelectMany (fg => fg.Where (x => x.FromKhariskhi==fg.Min (x_ => x_.FromKhariskhi) && x.ToKhariskhi==fg.Min (x_ => x_.ToKhariskhi)))
	.Take(10)