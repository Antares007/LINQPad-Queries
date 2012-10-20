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

//var fsd=Source_Data.First (sd => sd.ID==4761600);
//fsd.First_Name="დავითი";
//fsd.Last_Name="მარგველაშვილი";
//SubmitChanges();

var newSds = Source_Data_NewIds.Select (nId => nId.Source_Data);
var oldSds = Source_Data_OldIds.Select (oId => oId.Source_Data);

var xs=newSds.SelectMany (nsd => oldSds.Where (osd => osd.Unnom==nsd.Unnom).Select (osd => new{nsd,osd}))
	.Where (x =>   x.nsd.PID 		!= x.osd.PID
				|| x.nsd.First_Name != x.osd.First_Name 
				|| x.nsd.Last_Name 	!= x.osd.Last_Name 
				|| x.nsd.Birth_Date != x.osd.Birth_Date 
				)

.Select (x => new {x.nsd.ID,Source_Rec_Id=Math.Abs(x.osd.Source_Rec_Id.Value),nPID = x.nsd.PID, nFirst_Name = x.nsd.First_Name, nLast_Name = x.nsd.Last_Name, nBirth_Date = x.nsd.Birth_Date,
	               oPID = x.osd.PID, oFirst_Name = x.osd.First_Name, oLast_Name = x.osd.Last_Name, oBirth_Date = x.osd.Birth_Date})
	.Dump()
	;
