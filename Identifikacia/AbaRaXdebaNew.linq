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
	new {Khar=2		,Foto="PID + First_Name_Fst3 + Last_Name_Fst4 + Birth_Date"},
	new {Khar=4		,Foto="PID + Last_Name_Fst3 + First_Name_Fst4 + Birth_Date"},
	new {Khar=8		,Foto="PID + First_Name_Fst3 + Birth_Date"},
	new {Khar=16	,Foto="PID + Last_Name_Fst4 + Birth_Date"},
	new {Khar=32	,Foto="PID + First_Name_Fst3"},
	new {Khar=64	,Foto="PID + Last_Name_Fst4"},
	new {Khar=128	,Foto="PID + FID + Birth_Date"},
	new {Khar=256	,Foto="PID + Birth_Date"},
	new {Khar=512	,Foto="PID_Lst4 + First_Name_Fst3 + Last_Name_Fst4 + Birth_Date"},
	new {Khar=1024	,Foto="PID_Fst9 + First_Name_Fst3 + Last_Name_Fst4 + Birth_Date"},
	new {Khar=2048	,Foto="DOC_NO_Lst4 + First_Name_Fst3 + Last_Name_Fst4 + Birth_Date"},
	new {Khar=4096	,Foto="FID + First_Name + Last_Name_Fst4 + Birth_Date"},
	new {Khar=8192 ,Foto="FID + First_Name + Birth_Date"},
	new {Khar=16384 ,Foto="FID + First_Name_Fst4 + Birth_Date"},
	new {Khar=32768 ,Foto="FID + First_Name_Fst3 + Birth_Date"},
	new {Khar=65536 ,Foto="DOC_NO_Lst3 + First_Name + Last_Name_Fst4 + Birth_Year + Birth_Month"},
	new {Khar=131072 ,Foto="DOC_NO + First_Name + Last_Name_Fst4 + Birth_Year"}, 
	new {Khar=262144 ,Foto="DOC_NO + First_Name_Fst3 + Last_Name_Fst4 + Birth_Year"},
	new {Khar=524288 ,Foto="DOC_NO_Lst4 + FID + Last_Name + Birth_Month"},
}.ToList();

Func<int,string> getPirobebi= (k)=>
	pirobebi.Aggregate (new System.Text.StringBuilder(), (sb,i) => {
			if((k & i.Khar) == i.Khar){
				sb.AppendFormat("{0} {1}\n",i.Khar,i.Foto);
			}
			return sb;
		}).ToString();

Source_Data
	.Where (sd => sd.Source_Rec_Id>0 && sd.J_ID!=null)
	.Where(sd => sd.FromSource_Data_Kavshiris
								.Where (k => k.Source_Data.Source_Rec_Id<0 && k.Source_Data.J_ID!=null)
								.Where (k => (k.FromSauketesoKhariskhi&(4096))!=0)
								.Count () > 0)
	.OrderByDescending (sd =>  sd.FromSource_Data_Kavshiris.Count ())
	.Select (sd => new {sd,Unoms = sd.FromSource_Data_Kavshiris
									.Where (k => k.Source_Data.Source_Rec_Id<0 && k.Source_Data.J_ID!=null)
									.Where (k => (k.FromSauketesoKhariskhi&(4096))!=0)
									.Select (k => new {
												k.Source_Data.Unnom,
												UnomisKhariskhi=k.Source_Data.UnnomisKhariskhi,
												k.Source_Data.J_ID,
												k.Source_Data.Piroba,
												k.Source_Data.PID,
												k.Source_Data.First_Name,
												k.Source_Data.Last_Name,
												k.Source_Data.Birth_Date,
												Misamarti=k.Source_Data.Rai_Name+" "+k.Source_Data.Full_Address,
												k.FromSauketesoKhariskhi,
												Pirobebi=getPirobebi(k.FromSauketesoKhariskhi)
												})})
									.Take(10).Dump();