<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\SMDiagnostics.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Serialization.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ServiceModel.Internals.dll</Reference>
  <NuGetReference>Dapper</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Runtime.Serialization.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task<Dictionary<string, string>> daakorektire(string pid)
{
	var url = string.Format("http://172.17.8.125/CRA_Rest/PersonInfo/PersonInfoPid?piradiNomeri={0}&ckaro=Cra&userName=zurabbat", pid);
	var responseStr = await new HttpClient().GetStringAsync(url);
	return XDocument.Parse("<root>" + responseStr.Replace("</br>", " ") + "</root>")
			.Descendants("label")
			.Where(x => x.Attributes("id").Any ())
			.Where (x => !x.Value.Contains("ასაკი:"))
			.ToDictionary (x => x.Attribute("id").Value.Trim(),x=>x.Value.Trim());
}
void execMisamartisKorektireba(SqlConnection conn, string piradiNomeri, string raioni, string misamarti)
{
	var arisDevnilebisMierMocvdili = 0 != conn.Query<int>("select count(*) raod "+
														  "from Pirvelckaroebi.[dbo].[Pirvelckaro_100_DevniltaMisamartebi_201210] d "+
														  "where PID=@pid and RecDate>@recDate", 
														  new { 
														  		pid = piradiNomeri, 
																recDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1) 
											}).First ();
	arisDevnilebisMierMocvdili.Dump();
	if(!arisDevnilebisMierMocvdili){
		conn.Execute("[INSURANCEW].[dbo].[MisamartisKorektireba] @PID,@RAI_NAME,@CITY,@VILLAGE,@ADDRESS_FULL", 
					new {PID=piradiNomeri, RAI_NAME=raioni, CITY=default(string), VILLAGE=default(string), ADDRESS_FULL=misamarti})
			.Dump();
	}
}

void ganckhadebisMisamartisKorektireba(SqlConnection conn, string piradiNomeri, string raioni, string misamarti)
{
	
	var arisDevnilebisMierMocvdili = 0 != conn.Query<int>("select count(*) raod "+
														  "from Pirvelckaroebi.[dbo].[Pirvelckaro_100_DevniltaMisamartebi_201210] d "+
														  "where PID=@pid", new { pid = piradiNomeri})
										      .First ();
	arisDevnilebisMierMocvdili.Dump();
	if(!arisDevnilebisMierMocvdili){
		conn.Execute(@"update g 
		set [Rai]=[DazgvevaGanckhadebebi].[dbo].[DaadgineRaionisKodi] (@RAI_NAME,null,null,null), 
			[Rai_Name] = @RAI_NAME, 
			[City] = null, 
			[Village] = null, 
			[Street] = null, 
			[Full_Address] = @ADDRESS_FULL 
		from [DazgvevaGanckhadebebi].[dbo].[Ganckhadebebi] g
		where [PID]=@PID", 
					new {PID=piradiNomeri, RAI_NAME=raioni, CITY=default(string), VILLAGE=default(string), ADDRESS_FULL=misamarti}
		).Dump();
	}
	
}


void Main()
{
	var r = daakorektire("20850007176").Result;
	string.Join(", ", r.Values)	.Dump();
	using (var conn = new SqlConnection(@"Data Source=triton;Initial Catalog=INSURANCEW;Persist Security Info=True;User ID=sa;Password=ssa$20"))
	{
		conn.Open();
//		execMisamartisKorektireba(conn, "01150044885", "ვაკე-საბურთალო", "ვაკე, ბაგეები, წყნეთის გზატკ. ბაგეების სტუდქალაქი კორპ 5");
		execMisamartisKorektireba(conn, r["piradiNomeri"], r["raioni"], r["misamarti"]);
		ganckhadebisMisamartisKorektireba(conn, r["piradiNomeri"], r["raioni"], r["misamarti"]);
	}
}

public static T JsonDeserialize<T> (string jsonString)
{
    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
    T obj = (T)ser.ReadObject(ms);
    return obj;
}
public class PersonInfo {
	public PersonInformacia PersonInformacia { get; set; }
	public string Shedegi { get; set; }
	public DateTime PersonInfoDate { get; set; }
}
public class PersonInformacia{
	public string PrivateNumber { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public DateTime BirthDate { get; set; }
}