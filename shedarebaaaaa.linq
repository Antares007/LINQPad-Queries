<Query Kind="Program">
  <Connection>
    <ID>ced05258-b3f5-4292-8b8d-577da55d0081</ID>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA2LGCigg0bUqfvV+5fr69FwAAAAACAAAAAAAQZgAAAAEAACAAAABQjNqSoAChVc7pY40t5LwDSBgnqpqKpfGpztnrGU+Y2wAAAAAOgAAAAAIAACAAAACCDIBRLvjlNFjqIX5467Yafqrbi6HdDapIvcUgoSj9PxAAAABbOvffR/aHk+Wx3Yeu7FoiQAAAAC4eTdCoj/RY73eNWXEwWXOmecEVUn5J058U5ATdQJGxHG/+oeJKVPpg+FeqWtSGeM7adASTT3h8vPCAuNfk9Yk=</Password>
    <Database>Pirvelckaroebi</Database>
    <ShowServer>true</ShowServer>
    <Persist>true</Persist>
  </Connection>
  <Reference>D:\Dev\Ganckhadebebi.Domain\Ganckhadebebi.Domain\bin\Debug\Ganckhadebebi.Domain.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\110\SDK\Assemblies\Microsoft.SqlServer.ConnectionInfo.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\110\SDK\Assemblies\Microsoft.SqlServer.Management.Sdk.Sfc.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\110\SDK\Assemblies\Microsoft.SqlServer.Smo.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\110\SDK\Assemblies\Microsoft.SqlServer.SmoExtended.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\110\SDK\Assemblies\Microsoft.SqlServer.SqlEnum.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationFramework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\wpf\PresentationCore.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Xaml.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\wpf\WindowsBase.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\wpf\UIAutomationProvider.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Deployment.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\wpf\PresentationUI.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\wpf\System.Printing.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\wpf\ReachFramework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\wpf\UIAutomationTypes.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Accessibility.dll</Reference>
  <NuGetReference>Dapper</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Common</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Dmf</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Smo</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Data.Common</Namespace>
  <Namespace>Ganckhadebebi.Domain</Namespace>
  <Namespace>System.Windows.Controls</Namespace>
  <Namespace>System.Windows</Namespace>
  <Namespace>System.Collections.ObjectModel</Namespace>
  <Namespace>System.Windows.Data</Namespace>
  <Namespace>System.Windows.Markup</Namespace>
</Query>

void Main()
{
	this.Connection.Open();
	var momeChrili=getCkhrilebisSacavi(this.Connection);
	var regEx=new Regex(@"Pirvelckaroebi\..+_([0-9]+)_.+");
	var ckhrilebi=(
		from k in mapDict
		let r = regEx.Matches(k.Key).Cast<Match>()
		where r.Count ( )==1
		let v=r.First ().Groups[1].Value
		select new {BaseType = int.Parse(v), ckhrili=new UnnomCkhrili(k.Key,GetFieldsNames(this.Connection,k.Key),k.Value)}
	) .Select (n => new {n.BaseType,Ckhrili=n.ckhrili})
	.Where (n => n.BaseType <= 100);
	foreach(var ckhrili in ckhrilebi.Select (c => c.Ckhrili))
	{
			ckhrili.Dump();
			RunInTransaction(con => { DaadgineUnnomebi(con, ckhrili).Dump(); });
			
			RunInTransaction(con => {
				MianicheUnnomebi(con, ckhrili);
			});
			
			RunInTransaction(con => { DaadgineUnnomebi(con, ckhrili).Dump(); });
	}
}
Tuple<int,int> DaadgineUnnomebi(Tuple<SqlConnection,  SqlTransaction> con, UnnomCkhrili ckhrili)
{
    var reestrisCkhrili="UketesiReestri..UnnomShesadarebeliReestri";
	var shemdarebeli = new ShedarebisSkriptisGeneratori(new ReestrisCkhrili());
    
    var pirobebi = ckhrili.BaseTypes(s => con.Query<int>(s))
        .SelectMany (bt => bt.ShedarebisPirobebi.Select (x => new {Piroba=x,Bt=bt}) )
        .GroupBy (x => x.Piroba)
        .Select (g => g.Key.DaamateBazisTipebisShezgudva(g.Select (x => x.Bt).ToList()));
    
	var updateSkriptebi =  shemdarebeli.Damigenerire(ckhrili, pirobebi);
    var minicUnnomebi=0;
	foreach( var skripti in updateSkriptebi )
	{
        //skripti.Sql.Dump();
		minicUnnomebi += con.Execute(skripti.Sql, commandTimeout:9999);
	}
    return Tuple.Create(minicUnnomebi,con.Query<int>("select count(*) from " + ckhrili.Sakheli + " where Unnom is null", commandTimeout:9999).First ());
}
void Gaduble(int u1,int u2)
{
    RunInTransaction(con => con.Execute(@"exec UketesiReestri.dbo.[spGadublva] 'select "+u1+" S_Unnom, "+u2+" R_Unnom '",commandTimeout:9999));
}
public static class ConTranExtensions
{
	public static int Execute(this Tuple<SqlConnection,  SqlTransaction> contr, string sql, object param=null, int? commandTimeout=null, CommandType? commandType=null)
	{
		return contr.Item1.Execute(sql,param,contr.Item2,commandTimeout,commandType);
	}
	public static IEnumerable<T> Query<T>(this Tuple<SqlConnection,  SqlTransaction> contr, string sql, object param=null,bool buffered=true,int? commandTimeout=null, CommandType? commandType=null)
	{
		return contr.Item1.Query<T>(sql,param,contr.Item2,buffered,commandTimeout,commandType);
	}
}
void MianicheUnnomebi(Tuple<SqlConnection,  SqlTransaction> con, UnnomCkhrili ckhrili)
{
    var minichebuliUnnomebi = 0;
    foreach(var bt in ckhrili.BaseTypes(s=>con.Query<int>(s)))
    {
        var velebi = bt.VelebiUnnomisMisanicheblad;
        if(velebi == null) continue;
        
    	var str=@"INSERT INTO UketesiReestri.dbo.Unnoms("+string.Join(",", velebi)+@", Tarigi)
SELECT distinct         "+string.Join(",", velebi)+@", '"+DateTime.Now.Date.ToString("yyyyMMdd")+@"' Dro
FROM     (" + ckhrili.MomeciSelectNaciliVelebistvis(velebi.Union(new []{"Unnom", "Base_Type"}).ToArray() )+@") r
WHERE r.Unnom IS NULL AND r.Base_Type = " + bt.Value ;
       str.Dump();
       minichebuliUnnomebi += con.Execute(str,commandTimeout:9999).Dump();
    }

    if(minichebuliUnnomebi > 0)
    {
       var gadublvisPirobebi=    new []
       {
    @"select distinct u1.Unnom S_Unnom,u2.Unnom R_Unnom 
from UnnomShesadarebeliReestri u1 join UnnomShesadarebeliReestri u2 
on u1.IdentPID=u2.IdentPID 
WHERE u1.Unnom>u2.Unnom",

@"select distinct u1.Unnom S_Unnom,u2.Unnom R_Unnom 
from UnnomShesadarebeliReestri u1 join UnnomShesadarebeliReestri u2 
on u1.IdentPID=u2.PID 
where u2.Base_Type=99 and u1.Unnom!=u2.Unnom",

@"select distinct u1.Unnom S_Unnom, u2.Unnom R_Unnom 
from UnnomShesadarebeliReestri u1 join UnnomShesadarebeliReestri u2 
on u1.IdentPID=u2.PID 
AND (u1.First_Name=u2.First_Name or (u1.First_Name is null and u2.First_Name is null))
AND (      (substring(u1.Last_Name,1,4)=substring(u2.Last_Name,1,4) or (u1.Last_Name is null and u2.Last_Name is null)) 
        OR (year(u1.Birth_Date)=year(u2.Birth_Date) or (u1.Birth_Date is null and u2.Birth_Date is null))
    )
AND u2.IdentPID is null
WHERE u1.Unnom!=u2.Unnom",
@"select distinct u1.Unnom S_Unnom, u2.Unnom R_Unnom 
from UnnomShesadarebeliReestri u1 join UnnomShesadarebeliReestri u2 
on  (u1.PID=u2.PID or (u1.PID is null and u2.PID is null))
AND (u1.First_Name=u2.First_Name or (u1.First_Name is null and u2.First_Name is null))
AND (u1.Last_Name=u2.Last_Name or (u1.Last_Name is null and u2.Last_Name is null))
AND (u1.Birth_Date=u2.Birth_Date or (u1.Birth_Date is null and u2.Birth_Date is null))
WHERE u1.Unnom!=u2.Unnom"
        };
        foreach (var gp in gadublvisPirobebi)
        {
            var tsql = "exec UketesiReestri.[dbo].[spGadublva] '"+gp+"'";
            while(con.Execute(tsql,commandTimeout:9999).Dump(tsql)>0);
        }  
    }
}


IEnumerable<string> GetFieldsNames(IDbConnection con, string table_Name)
{
    var nameParts=table_Name.Split('.').Select (x => x.Replace("[","").Replace("]","")).ToArray();
    if(nameParts.Length!=3)
        throw new InvalidArgumentException("table_Name");
    return con.Query<string>("select COLUMN_NAME from "+nameParts[0]+".INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='"+nameParts[2]+"'");
}
Func<string, UnnomCkhrili> getCkhrilebisSacavi(IDbConnection con)
{
	var dict = new ConcurrentDictionary<string,UnnomCkhrili>();
	return  sakheli => dict.GetOrAdd(sakheli, s => {
														Dictionary<string,string> map;
														mapDict.TryGetValue(sakheli, out map);
														return new UnnomCkhrili(s, GetFieldsNames(con, s), map);
													});
}

private Dictionary<string,Dictionary<string,string>> mapDict=@"
    Pirvelckaroebi..Pirvelckaro_01_UMCEOEBI,1,Base_Type
	Pirvelckaroebi..Pirvelckaro_01_UMCEOEBI,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_01_UMCEOEBI,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_01_UMCEOEBI,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_01_UMCEOEBI,BIRTH_DATE,[Birth_Date]
	
	Pirvelckaroebi..Pirvelckaro_02_DEVNILEBI,2,Base_Type
	Pirvelckaroebi..Pirvelckaro_02_DEVNILEBI,PersonalNumber,[PID]
	Pirvelckaroebi..Pirvelckaro_02_DEVNILEBI,Name,[First_Name]
	Pirvelckaroebi..Pirvelckaro_02_DEVNILEBI,Surname,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_02_DEVNILEBI,DateOfBirth,[Birth_Date]
    
    Pirvelckaroebi..Pirvelckaro_03_BAVSHVEBI,3,Base_Type
	Pirvelckaroebi..Pirvelckaro_03_BAVSHVEBI,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_03_BAVSHVEBI,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_03_BAVSHVEBI,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_03_BAVSHVEBI,BIRTH_DATE,[Birth_Date]
	Pirvelckaroebi..Pirvelckaro_03_BAVSHVEBI,DACESEBULEBA,Dacesebuleba
    
    Pirvelckaroebi..Pirvelckaro_04_REINTEGRACIA,4,Base_Type
	Pirvelckaroebi..Pirvelckaro_04_REINTEGRACIA,C_PID,[PID]
	Pirvelckaroebi..Pirvelckaro_04_REINTEGRACIA,C_F_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_04_REINTEGRACIA,C_L_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_04_REINTEGRACIA,C_BIRTH_DA,[Birth_Date]
    
    Pirvelckaroebi..Pirvelckaro_05_KULTURA,5,Base_Type
	Pirvelckaroebi..Pirvelckaro_05_KULTURA,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_05_KULTURA,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_05_KULTURA,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_05_KULTURA,BIRTH_DATE,[Birth_Date]	
	Pirvelckaroebi..Pirvelckaro_05_KULTURA,DACESEBULEBA,Dacesebuleba	
    
    Pirvelckaroebi..Pirvelckaro_06_XANDAZMULEBI,6,Base_Type
	Pirvelckaroebi..Pirvelckaro_06_XANDAZMULEBI,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_06_XANDAZMULEBI,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_06_XANDAZMULEBI,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_06_XANDAZMULEBI,BIRTH_DATE,[Birth_Date]	
    
    Pirvelckaroebi..Pirvelckaro_07_SKOLA_PANSIONEBI,7,Base_Type
	Pirvelckaroebi..Pirvelckaro_07_SKOLA_PANSIONEBI,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_07_SKOLA_PANSIONEBI,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_07_SKOLA_PANSIONEBI,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_07_SKOLA_PANSIONEBI,BIRTH_DATE,[Birth_Date]	
	Pirvelckaroebi..Pirvelckaro_07_SKOLA_PANSIONEBI,DACESEBULEBA,Dacesebuleba	
	
    Pirvelckaroebi..Pirvelckaro_08_TEACHERS,8,Base_Type
	Pirvelckaroebi..Pirvelckaro_08_TEACHERS,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_08_TEACHERS,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_08_TEACHERS,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_08_TEACHERS,BIRTH_DATE,[Birth_Date]
	Pirvelckaroebi..Pirvelckaro_08_TEACHERS,SAG_DACESEBULEBA,Dacesebuleba
	
    Pirvelckaroebi..Pirvelckaro_09_UFROSI_AGMZRDELEBI,9,Base_Type
	Pirvelckaroebi..Pirvelckaro_09_UFROSI_AGMZRDELEBI,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_09_UFROSI_AGMZRDELEBI,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_09_UFROSI_AGMZRDELEBI,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_09_UFROSI_AGMZRDELEBI,BIRTH_DATE,[Birth_Date]
	Pirvelckaroebi..Pirvelckaro_09_UFROSI_AGMZRDELEBI,DACESEBULEBA,Dacesebuleba
    
    Pirvelckaroebi..Pirvelckaro_10_APKHAZETIS_OJAKHEBI,10,Base_Type
	Pirvelckaroebi..Pirvelckaro_10_APKHAZETIS_OJAKHEBI,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_10_APKHAZETIS_OJAKHEBI,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_10_APKHAZETIS_OJAKHEBI,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_10_APKHAZETIS_OJAKHEBI,BIRTH_DATE,[Birth_Date]
    
    Pirvelckaroebi..Pirvelckaro_11_SATEMO,11,Base_Type
	Pirvelckaroebi..Pirvelckaro_11_SATEMO,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_11_SATEMO,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_11_SATEMO,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_11_SATEMO,BIRTH_DATE,[Birth_Date]
	Pirvelckaroebi..Pirvelckaro_11_SATEMO,DACESEBULEBA,Dacesebuleba
	
    Pirvelckaroebi..Pirvelckaro_12_MCIRE_SAOJAXO,12,Base_Type
	Pirvelckaroebi..Pirvelckaro_12_MCIRE_SAOJAXO,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_12_MCIRE_SAOJAXO,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_12_MCIRE_SAOJAXO,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_12_MCIRE_SAOJAXO,BIRTH_DATE,[Birth_Date]
    
    Pirvelckaroebi..Pirvelckaro_13_TEACHERS_AFX,13,Base_Type
	Pirvelckaroebi..Pirvelckaro_13_TEACHERS_AFX,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_13_TEACHERS_AFX,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_13_TEACHERS_AFX,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_13_TEACHERS_AFX,BIRTH_DATE,[Birth_Date]
	Pirvelckaroebi..Pirvelckaro_13_TEACHERS_AFX,SAG_DACESEBULEBA,Dacesebuleba
	
    Pirvelckaroebi..Pirvelckaro_14_RESURSCENTRIS_TANAMSHROMLEBI,14,Base_Type
	Pirvelckaroebi..Pirvelckaro_14_RESURSCENTRIS_TANAMSHROMLEBI,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_14_RESURSCENTRIS_TANAMSHROMLEBI,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_14_RESURSCENTRIS_TANAMSHROMLEBI,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_14_RESURSCENTRIS_TANAMSHROMLEBI,BIRTH_DATE,[Birth_Date]
	Pirvelckaroebi..Pirvelckaro_14_RESURSCENTRIS_TANAMSHROMLEBI,SAG_DACESEBULEBA,Dacesebuleba

	Pirvelckaroebi..Pirvelckaro_21_SAPENSIO_ASAKIS_MOSAXLEOBA,21,Base_Type
	Pirvelckaroebi..Pirvelckaro_21_SAPENSIO_ASAKIS_MOSAXLEOBA,PRIVATE_NUMBER,[PID]
	Pirvelckaroebi..Pirvelckaro_21_SAPENSIO_ASAKIS_MOSAXLEOBA,PRIVATE_NUMBER,[IdentPID]
	Pirvelckaroebi..Pirvelckaro_21_SAPENSIO_ASAKIS_MOSAXLEOBA,FIRST,[First_Name]
	Pirvelckaroebi..Pirvelckaro_21_SAPENSIO_ASAKIS_MOSAXLEOBA,LAST,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_21_SAPENSIO_ASAKIS_MOSAXLEOBA,BIRTH_DATE,[Birth_Date]
    
    Pirvelckaroebi..Pirvelckaro_22_STUDENTEBI,22,Base_Type
	Pirvelckaroebi..Pirvelckaro_22_STUDENTEBI,პირადობა,[PID]
	Pirvelckaroebi..Pirvelckaro_22_STUDENTEBI,სახელი,[First_Name]
	Pirvelckaroebi..Pirvelckaro_22_STUDENTEBI,გვარი,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_22_STUDENTEBI,დაბ_თარიღი,[Birth_Date]
	Pirvelckaroebi..Pirvelckaro_22_STUDENTEBI,უსდ,Dacesebuleba
	
    Pirvelckaroebi..[Pirvelckaro_23_BAVSHVEBI(165)],23,Base_Type
	Pirvelckaroebi..[Pirvelckaro_23_BAVSHVEBI(165)],PRIVATE_NUMBER,[PID]
	Pirvelckaroebi..[Pirvelckaro_23_BAVSHVEBI(165)],PRIVATE_NUMBER,[IdentPID]
	Pirvelckaroebi..[Pirvelckaro_23_BAVSHVEBI(165)],FIRST,[First_Name]
	Pirvelckaroebi..[Pirvelckaro_23_BAVSHVEBI(165)],LAST,[Last_Name]
	Pirvelckaroebi..[Pirvelckaro_23_BAVSHVEBI(165)],BIRTH_DATE,[Birth_Date]
	
    Pirvelckaroebi..Pirvelckaro_24_INVALIDI_BAVSHVEBI,24,Base_Type
	Pirvelckaroebi..Pirvelckaro_24_INVALIDI_BAVSHVEBI,PiradiNomeri,[PID]
	Pirvelckaroebi..Pirvelckaro_24_INVALIDI_BAVSHVEBI,Sakheli,[First_Name]
	Pirvelckaroebi..Pirvelckaro_24_INVALIDI_BAVSHVEBI,Gvari,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_24_INVALIDI_BAVSHVEBI,DabadebisTarigi,[Birth_Date]
    
    Pirvelckaroebi..Pirvelckaro_25_MKVETRAD_GAMOXATULI_INVALIDI_BAVSHVEBI,25,Base_Type
	Pirvelckaroebi..Pirvelckaro_25_MKVETRAD_GAMOXATULI_INVALIDI_BAVSHVEBI,PiradiNomeri,[PID]
	Pirvelckaroebi..Pirvelckaro_25_MKVETRAD_GAMOXATULI_INVALIDI_BAVSHVEBI,Sakheli,[First_Name]
	Pirvelckaroebi..Pirvelckaro_25_MKVETRAD_GAMOXATULI_INVALIDI_BAVSHVEBI,Gvari,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_25_MKVETRAD_GAMOXATULI_INVALIDI_BAVSHVEBI,DabadebisTarigi,[Birth_Date]
	
    Pirvelckaroebi..Pirvelckaro_26_ARASAQARTVELOS_MOQALAQE_PENSIONREBI,26,Base_Type
	Pirvelckaroebi..Pirvelckaro_26_ARASAQARTVELOS_MOQALAQE_PENSIONREBI,PiradiNomeri,[PID]
	Pirvelckaroebi..Pirvelckaro_26_ARASAQARTVELOS_MOQALAQE_PENSIONREBI,Sakheli,[First_Name]
	Pirvelckaroebi..Pirvelckaro_26_ARASAQARTVELOS_MOQALAQE_PENSIONREBI,Gvari,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_26_ARASAQARTVELOS_MOQALAQE_PENSIONREBI,DabadebisTarigi,[Birth_Date]
	
    Pirvelckaroebi..[Pirvelckaro_27_AXALSHOBILEBI(165)],27,Base_Type
	Pirvelckaroebi..[Pirvelckaro_27_AXALSHOBILEBI(165)],PRIVATE_NUMBER,[PID]
	Pirvelckaroebi..[Pirvelckaro_27_AXALSHOBILEBI(165)],PRIVATE_NUMBER,[IdentPID]
	Pirvelckaroebi..[Pirvelckaro_27_AXALSHOBILEBI(165)],FIRST,[First_Name]
	Pirvelckaroebi..[Pirvelckaro_27_AXALSHOBILEBI(165)],LAST,[Last_Name]
	Pirvelckaroebi..[Pirvelckaro_27_AXALSHOBILEBI(165)],BIRTH_DATE,[Birth_Date]
	
    Pirvelckaroebi..Pirvelckaro_100_DevniltaMisamartebi_201210,100,Base_Type
	Pirvelckaroebi..Pirvelckaro_100_DevniltaMisamartebi_201210,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_100_DevniltaMisamartebi_201210,First_Name,[First_Name]
	Pirvelckaroebi..Pirvelckaro_100_DevniltaMisamartebi_201210,Last_Name,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_100_DevniltaMisamartebi_201210,Birth_Date,[Birth_Date]
	
    Pirvelckaroebi..Pirvelckaro_999_s2,1,Base_Type
	Pirvelckaroebi..Pirvelckaro_999_s2,[10],[PID]
	Pirvelckaroebi..Pirvelckaro_999_s2,[6],[First_Name]
	Pirvelckaroebi..Pirvelckaro_999_s2,[7],[Last_Name]
	Pirvelckaroebi..Pirvelckaro_999_s2,[c9],[Birth_Date]
	"
	.Split('\n')
	.Select (x => x.Split(',').Select (v=>v.Trim()).ToArray())
	.Where(x=>x.Length==3)
	.Select (x => new {Tanme=x[0],From=x[1],To=x[2]})
	.GroupBy (x => x.Tanme)
	.ToDictionary(x=>x.Key,x=>x.ToDictionary (k => k.To,v=>v.From));

	
	
void RunInTransaction(Action<Tuple<SqlConnection,  SqlTransaction>> action)
{
		using(var con = new SqlConnection("Data Source=Triton;User ID=sa;Password=ssa$20;Initial Catalog=Pirvelckaroebi;app=LINQPad SHEDAREBAAA"))
		{
			con.Open();
			
			using(var tr = con.BeginTransaction())
			{
				try
				{
					action(Tuple.Create(con, tr));
					tr.Commit();
				}
				catch(Exception ex)
				{
					tr.Rollback();
					throw new AggregateException(ex);
				}
			}
		}
}

public static class Ex
{
	public static IEnumerable<Database> Databases(this Server src) 
	{
		return src.Databases.Cast<Database>();
	}	

	public static IEnumerable<Table> Tables(this Database src) 
	{
		return src.Tables.Cast<Table>();
	}	

	public static IEnumerable<Column> Columns(this Table src) 
	{
		return src.Columns.Cast<Column>();
	}	

	public static IEnumerable<Index> Indexes(this Table src) 
	{
		return src.Indexes.Cast<Index>();
	}	

	public static IEnumerable<IndexedColumn> IndexedColumns(this Index src) 
	{
		return src.IndexedColumns.Cast<IndexedColumn>();
	}

	public static Table AddColumun(this Table tb, params Action<Column>[] colBulders) 
	{
		foreach(var colB in colBulders)
		{
			var col=new Column(){Parent=tb};
			colB (col);
			tb.Columns.Add(col);
		}
		return tb;
	}
	
	public static void CreateIndexOn(this Table tb, params string[] columuns) {
		Index idx;
		idx = new Index(tb, "IX_"+tb.Name+"_"+string.Join("_", columuns));

		foreach(var ic  in columuns.Select (c => new IndexedColumn(idx, c, false))){
			idx.IndexedColumns.Add(ic);
		}
		idx.IsClustered = false;
		idx.Create();
	}
	
	private static readonly Lazy<Server> lazy = new Lazy<Server>(() => {
			var conn = new ServerConnection("triton", "sa", "ssa$20");
    		var srv = new Server(conn);
			return srv;
			});
    
    public static Server Triton { get { return lazy.Value; } }
}


public static class Ex2
{
	public static IEnumerable<T> Show<T>(this IEnumerable<T> src, Action done = null)
	{
		
		var grid = new DataGrid(){ ItemsSource= src, AutoGenerateColumns=false,IsReadOnly=true };
		var props = typeof(T).GetProperties(BindingFlags.Instance|BindingFlags.Public).Select (x => new {x.Name,Type=x.PropertyType});
        props.Dump();
		foreach(var p in props)
		{
			
			if(typeof(Button).IsAssignableFrom(p.Type))
			{
			
				DataTemplate dt = ("<ContentPresenter Content=\"{Binding "+p.Name+"}\" />").ToDataTemplate();
				grid.Columns.Add(new DataGridTemplateColumn{Header = p.Name, CellTemplate=dt});
			}
			else
			{
				if (p.Type.IsValueType || p.Type==typeof(string))
				grid.Columns.Add(new DataGridTextColumn{Header = p.Name, Binding=new Binding{Path=new PropertyPath(p.Name)}});
			}
		}
		var g = new Grid();
		g.RowDefinitions.Add(new RowDefinition(){Height=new GridLength(1,GridUnitType.Star)});
		g.RowDefinitions.Add(new RowDefinition(){Height=new GridLength(1,GridUnitType.Auto)});
		grid.SetValue(Grid.RowProperty,0);
		grid.SetValue(Grid.ColumnProperty,0);
		g.Children.Add(grid);
		if(done != null)
		{
			var doneButton = new Button{ Content="Done" };
			doneButton.Click += (sender, args) => done();
			var stackPanel = new StackPanel(){ Children={ doneButton } };
			stackPanel.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel.SetValue(Grid.RowProperty,1);
			g.Children.Add(stackPanel);
		}
		PanelManager.DisplayWpfElement (g,null);
		return src;
	}
	
	public static object InstantiateXAML(string xaml)
    {
        return XamlReader.Load
        (
            XmlReader.Create(new StringReader(xaml))
        );    
    }
 	public static Button OnClick(this string src,Action action)
    {
		return OnClick(src, _ => action());
	}
	public static Button OnClick(this string src,Action<Button> action)
    {
		var b =new Button(){ Content = src };
		b.Click+= (sender, args) => action(b);
		return b;
	}
    public static DataTemplate ToDataTemplate(this string template)
    {
        string templateFormat = @"<DataTemplate  
                                xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' 
                                xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
                                {0}
                            </DataTemplate>";
        
        return (DataTemplate) InstantiateXAML(string.Format(templateFormat, template));
    }
	public static ObservableCollection<T> ToObsCol<T>(this IEnumerable<T> src){
		return new ObservableCollection<T>(src);
	}
	public static Func<T,U> CreateSelector<T,U>(this IEnumerable<T> src,Func<T,U> selector){
		return selector;
	}
    
    public static int LevenshteinDistance(this string source, string target){
            if(String.IsNullOrEmpty(source)){
                    if(String.IsNullOrEmpty(target)) return 0;
                        return target.Length;
            }
            if(String.IsNullOrEmpty(target)) return source.Length;
    
            if(source.Length > target.Length){
                    var temp = target;
                    target = source;
                    source = temp;
            }
    
            var m = target.Length;
            var n = source.Length;
            var distance = new int[2, m + 1];
            // Initialize the distance 'matrix'
            for(var j = 1; j <= m; j++) distance[0, j] = j;
    
            var currentRow = 0;
            for(var i = 1; i <= n; ++i){
                    currentRow = i & 1;
                    distance[currentRow, 0] = i;
                    var previousRow = currentRow ^ 1;
                    for(var j = 1; j <= m; j++){
                            var cost = (target[j - 1] == source[i - 1] ? 0 : 1);
                            distance[currentRow, j] = Math.Min(Math.Min(
                                                    distance[previousRow, j] + 1,
                                                    distance[currentRow, j - 1] + 1),
                                                    distance[previousRow, j - 1] + cost);
                    }
            }
            return distance[currentRow, m];
    }

}