<Query Kind="Statements">
  <Connection>
    <ID>453dabbc-e8b3-4c04-8eec-536e8e4e7b58</ID>
    <Persist>true</Persist>
    <Server>triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <NoPluralization>true</NoPluralization>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA3XCANzX8A0+HtkxmZtXFGAAAAAACAAAAAAAQZgAAAAEAACAAAACp+sEJqJxOPz2BQ1lxRRtXH/XJirk9/kw8mJj0bPf5jQAAAAAOgAAAAAIAACAAAACZd/POK3nxMSCEaCgbVDIUSs/pSsra35l5MYJ8LGMjExAAAABDe6Hrhl+CP58Aq2DzWMGNQAAAAA6bC1RC4u17G4KQF35FDnGlInhYWmmeVWDNOqKPGjJdSaTe9PMIuov4DO+z/r3whP06rpngcdEwF0GcF80cZT0=</Password>
    <IncludeSystemObjects>true</IncludeSystemObjects>
    <Database>SocialuriDazgveva</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>C:\Temp\EPPlus.dll</Reference>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

//var sd = Source_Data.Where (s => !s.Birth_Date.HasValue).Take(5); ia burchuladze
var sd = Source_Data;
var q = (
from f in sd.Where (s => s.Unnom==null).Take(1)
from t in sd.Where (s => s.Unnom!=null)
where f.ID != t.ID
select new {
	FromId=f.ID,
	
	PidEqPid = (bool?)(f.PID != null && t.PID != null && f.PID.Length == 11 && f.PID == t.PID),
	PidFrst9EqPidFrst9 = (bool?)(f.PID.Length==11 && f.PID.Substring(0,9) == t.PID.Substring(0,9)),
	PidLast4EqPidLast4 =  (bool?)(f.PID.Length==11 && f.PID.Substring(f.PID.Length-4) == t.PID.Substring(t.PID.Length-4)),
	
	DocNoEqDocNo = (bool?)(f.PID.Length!=11 && f.PID == t.PID),
	DocNoLast4EqDocNoLast4 = (bool?)(f.PID.Length!=11 && f.PID.Substring(f.PID.Length-4) == t.PID.Substring(t.PID.Length-4)),
	DocNoLast3EqDocNoLast3 = (bool?)(f.PID.Length!=11 && f.PID.Substring(f.PID.Length-3) == t.PID.Substring(t.PID.Length-3)),
	
	FnameEqFname = (bool?)(f.First_Name == t.First_Name),
	LnameEqLname = (bool?)(f.Last_Name == t.Last_Name),
	
	FnameFrst3EqFnameFrst3 = (bool?)(f.First_Name.Substring(0,3)==t.First_Name.Substring(0,3)),
	LnameFrst4EqLnameFrst4 = (bool?)(f.Last_Name.Substring(0,4)==t.Last_Name.Substring(0,4)),
	FnameFrst3EqLnameFrst3 = (bool?)(f.First_Name.Substring(0,3)==t.Last_Name.Substring(0,3)),
	LnameFrst4EqFnameFrst4 = (bool?)(f.Last_Name.Substring(0,4)==t.First_Name.Substring(0,4)),
	
	BirthDateEqBirthDate=(bool?)(f.Birth_Date==t.Birth_Date),
	BirthYearEqBirthYear=(bool?)(f.Birth_Date.Value.Year==t.Birth_Date.Value.Year),
	BirthMonthEqBirthMonth=(bool?)(f.Birth_Date.Value.Month==t.Birth_Date.Value.Month),

	FidEqFid = (bool?)(f.FID == t.FID),

	ToId=t.ID}
).Where (x => x.PidEqPid.Value ||  x.PidFrst9EqPidFrst9.Value || x.PidLast4EqPidLast4.Value || 
			  x.DocNoEqDocNo.Value || x.DocNoLast4EqDocNoLast4.Value || x.DocNoLast3EqDocNoLast3.Value ||
	          x.FnameEqFname.Value || x.LnameEqLname.Value || 
			  x.FnameFrst3EqFnameFrst3.Value || x.LnameFrst4EqLnameFrst4.Value || 
			  x.FnameFrst3EqLnameFrst3.Value || x.LnameFrst4EqFnameFrst4.Value ||
			  x.BirthDateEqBirthDate.Value || x.BirthYearEqBirthYear.Value || x.BirthMonthEqBirthMonth.Value || 
			  x.FidEqFid.Value
			  )
.Take(10)
.Dump();
	
