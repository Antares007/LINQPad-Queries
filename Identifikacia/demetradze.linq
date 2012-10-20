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

Source_Data.Where (sd => (sd.First_Name=="დიანა" && sd.Last_Name=="დიმიტრაძე") || 
							(sd.First_Name=="დიანა" && sd.Last_Name=="დემეტრაძე") || 
							(sd.Last_Name=="დიანა" && sd.First_Name=="დიმიტრაძე") || 
							(sd.Last_Name=="დიანა" && sd.First_Name=="დემეტრაძე"))
							.Where (sd => sd.Birth_Date==DateTime.Parse("06.02.1998 0:00:00"))
							.OrderBy (sd => sd.Birth_Date)
							.Dump();
Source_Data.Where (sd => (sd.First_Name=="გიორგი" && sd.Last_Name=="უცნობი") )
							.OrderBy (sd => sd.Birth_Date)
							.Dump();
							
Source_Data.Where (sd => sd.Unnom==465001 || sd.Unnom==5490373).Dump();