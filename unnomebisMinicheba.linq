<Query Kind="Program">
  <Connection>
    <ID>ced05258-b3f5-4292-8b8d-577da55d0081</ID>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAN27c6lA2WkeiuPoKsF6zVAAAAAACAAAAAAAQZgAAAAEAACAAAAAe6fEoA74iMkrfJZa6XuxfzQh6cY+5cD/VGmkvkAFZHgAAAAAOgAAAAAIAACAAAAAvSa58+4v5Ek9QdqmoHFRtbjSsRtGtZOiiFFwZuP9l4hAAAADSD159L0suWS7o5rny5Q3sQAAAAFcvMLTxKkyAI0tWzIRYIyYLH7PYk2LvzCPR+00AaEffUskcT8F2IaOy6O9Btu50kIHx2PxD4ahCWKTfrpSexFY=</Password>
    <Database>Pirvelckaroebi</Database>
    <ShowServer>true</ShowServer>
    <Persist>true</Persist>
  </Connection>
  <Reference>D:\Dev\Ganckhadebebi.Domain\Ganckhadebebi.Domain\bin\Debug\Ganckhadebebi.Domain.dll</Reference>
  <NuGetReference>Dapper</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>Ganckhadebebi.Domain</Namespace>
</Query>

void Main()
{

var con = this.Connection;
con.Open();


var ckhrili = Ckhrili.Daamzade(
	sql => con.Query<string>(sql), 
	"SocialuriDazgveva..Dazgveva_ForUnnom",
	m=>{}
	);
var reestri = new ReestrisCkhrili();
reestri.MiucereUnnomebi(ckhrili, sql => sql.Dump(),s => new []{1,2,3,4,5,6,7,8,9,10,11,12,13,14,21,22,23,24,25,26});

//var ako=Ckhrili.Daamzade(  sql => con.Query<string>(sql),
//                                        "SocialuriDazgveva.dbo.Polisebi",
//                                        m => m.Expr("PiradiNomeri").As("PID")
//											.Expr("Sakheli").As("First_Name")
//                                            .Expr("Gvari").As("Last_Name")
//                                            .Expr("DabadebisTarigi").As("Birth_Date")
//                                            .Expr("ProgramisId").As("Base_Type")
//                                            .Expr("AkhaliUnnomi").As("Unnom")
//                        );
//new ReestrisCkhrili().MiucereUnnomebi(ako,s => ( s +"\nGO\n\n\n").Dump(), s => con.Query<int>(s));
//
//return;

   
                       
return;
    
//    reestri.MiucereUnnomebi(polCkhrili, s => s.Dump(), s => con.Query<int>(s));
    
}