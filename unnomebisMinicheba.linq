<Query Kind="Program">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAAAD89x4SL38S/4r7NUU2iHNNTcmnVTi1xQPx1AC1vAtFAAAAAAOgAAAAAIAACAAAABgO+xVBio0IKzceIXWbWfgFv0jcxQpOA9YilhDtPA8XxAAAABJcM5+MLInsGd5jUUGfXtnQAAAALHy7sVof5cKLhfpSHxbLdPESALwKOWLElOgeYcZmaeqO1sDG9SHnPN5xOzSlBHlSD7agk+KC9dH/4XMZn4gYvM=</Password>
    <Database>INSURANCEW</Database>
    <ShowServer>true</ShowServer>
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


var ako=Ckhrili.Daamzade(  sql => con.Query<string>(sql),
                                        "[TempO].[dbo].[Population]",
                                        m => m.Expr("rtrim(ltrim(FirstName)) collate SQL_Latin1_General_CP1_CI_AS").As("First_Name")
                                            .Expr("rtrim(ltrim(LastName)) collate SQL_Latin1_General_CP1_CI_AS").As("Last_Name")
                                            .Expr("rtrim(ltrim(PersonID)) collate SQL_Latin1_General_CP1_CI_AS").As("PID")
                                            .Expr("BirthDate").As("Birth_Date")
                                            .Expr("3").As("Base_Type")
                                            .Expr("Unnom").As("Unnom")
                        );
new ReestrisCkhrili().MiucereUnnomebi(ako,s=>(s+"\nGO").Dump(),s=>new []{3});

return;

var ckhrilebi = 
(@"
DAZGVEVA_201108
DAZGVEVA_201109
DAZGVEVA_201110
DAZGVEVA_201111
DAZGVEVA_201112
DAZGVEVA_201201
DAZGVEVA_201202
DAZGVEVA_201203
DAZGVEVA_201204
DAZGVEVA_201205
DAZGVEVA_201206
DAZGVEVA_201207
DAZGVEVA_201208
DAZGVEVA_201209
DAZGVEVA_201210
DAZGVEVA_201211
DAZGVEVA_201212
DAZGVEVA_201301
DAZGVEVA_201302
"  ).Split(new []{'\n','\r'})
    .Select (x => x.Trim())
    .Where (x => x.Length > 10)
    .Select (x => Ckhrili.Daamzade(  sql => con.Query<string>(sql),
                                        "INSURANCEW.." + x,
                                        m => m.Expr("FIRST_NAME collate SQL_Latin1_General_CP1_CI_AS").As("First_Name")
                                            .Expr("LAST_NAME collate SQL_Latin1_General_CP1_CI_AS").As("Last_Name")
                                            .Expr("PID collate SQL_Latin1_General_CP1_CI_AS").As("PID")
                                            .Expr("BIRTH_DATE").As("Birth_Date")
                                            .Expr("Base_type").As("Base_Type")
                                            .Expr("AkhaliUnnomi").As("Unnom")
                        ))
    ;
 

var reestri = new ReestrisCkhrili();

foreach (var c in ckhrilebi)
{
    reestri.MiucereUnnomebi(c, s => (s+"PRINT '" + c.Sakheli+"'\nGO\n").Dump(), s => new []{1,2,3,4,5,6,7,8,9,10,11,12,13,14,21,22,23,24,25,26});
}
   
                       
return;
    
//    reestri.MiucereUnnomebi(polCkhrili, s => s.Dump(), s => con.Query<int>(s));
    
}