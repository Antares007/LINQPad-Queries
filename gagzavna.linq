<Query Kind="Program">
  <Reference>D:\Dev\EventStore\bin\eventstore\release\anycpu\EventStore.ClientAPI.dll</Reference>
  <NuGetReference>Dapper</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>EventStore.ClientAPI</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>EventStore.ClientAPI.SystemData</Namespace>
</Query>

void Sql(Action<SqlConnection> actOnConnection){
}
void Main()
{
DateTime.Now.ToString("yyyyMMdd_hhmmss").Dump();
return;
SqlConnection con=null;

con.Execute(@"
SELECT	d.ID,d.Base_type,d.Base_Description,d.Unnom,d.REGION_ID,d.RAI,d.RAI_NAME,d.CITY,d.VILLAGE,d.ADDRESS_FULL,d.FID,u.PID
		,u.FIRST_NAME,u.LAST_NAME,u.BIRTH_DATE,isnull(pir.GENDER,u.Sex) Sex,d.Company_ID_201308 as Company_ID,d.Company_201308 as Company
		,d.[dagv-tar],[End_Date],[STOP_DATE],d.SAG_DACESEBULEBA,d.STATE_201308 as STATE, p.PolisisNomeri,[201308] - M_201308 as [201308] 
--INTO	tempAkhaliKontraqti
FROM	INSURANCEW.dbo.DAZGVEVA_201308 d 
JOIN	SocialuriDazgveva.dbo.Polisebi p on d.ID = p.DzveliSistemisId and (p.PolisisStatusi <> 'Gaukmebuli' or p.PolisisStatusi is null)
JOIN	UketesiReestri.dbo.vUnnomBoloKargiChanaceri u on d.Unnom = u.Unnom
LEFT JOIN (
			select distinct Unnom ,GENDER from  Pirvelckaroebi.dbo.[Pirvelckaro_27_AXALSHOBILEBI(165)]) pir on d.Unnom = pir.Unnom
				JOIN	INSURANCEW.dbo.ChatarebuliOperaciebisLogi l on d.ID = l.DazgvevisID 
				WHERE	l.OperaciisTipi='AkhaliKontraqti' 
	and GagzavnisDro is null
");

}

class Es
{
	public static void Handle<T>(Action<T> action)
	{
	}
}
class Movlena{
}
// Define other methods and classes here
//e.egnatashvili@evillage.gov.ge