<Query Kind="Program">
  <Connection>
    <ID>ba81614b-7b3d-4771-990b-3896ca4c920e</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <Database>Pirvelckaroebi</Database>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAN27c6lA2WkeiuPoKsF6zVAAAAAACAAAAAAAQZgAAAAEAACAAAADOjBuabYrQXKzD+V5qCDHhnUl31vD/RtKQgj8M/zhT3QAAAAAOgAAAAAIAACAAAADtNpfv/JBmG56Wxv7I5SFtcCkwLuWSP2anF9nVs/OWlBAAAAAvx0hFOmPNsjUCXW4z8oMBQAAAABUjFDZT2yCpArlijx7Yp1DWZWt6jf5mlNPfFxwtNfLCy1SoahQtA4+rE3x8wiU94VkgRdPH5FNYRPhPdUUFZOE=</Password>
    <IncludeSystemObjects>true</IncludeSystemObjects>
    <LinkedDb>DazgvevaGanckhadebebi</LinkedDb>
    <LinkedDb>INSURANCEW</LinkedDb>
    <LinkedDb>SocialuriDazgveva</LinkedDb>
    <LinkedDb>UketesiReestri</LinkedDb>
  </Connection>
  <NuGetReference>Microsoft.AspNet.WebApi.Client</NuGetReference>
  <Namespace>System.Net.Http</Namespace>
</Query>

class CraPerson{
	public string PrivateNumber { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public DateTime BirthDate { get; set; }
	public string RegionStr { get; set; }
	public string LivingPlace { get; set; }
}

CraPerson GetCraPerson(string pid)
{
	using(var c = new HttpClient())
	{
		var url = string.Format("http://172.17.8.125/CRA_Rest/PersonInfo/JSONPersonInfoPid?piradiNomeri={0}&ckaro=Cra&userName=zurabbat", pid);
		var str = c.GetStringAsync(url).Result;
		var o = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(str,
			new {
				PersonInformacia = new {
						PrivateNumber=default(string),
						FirstName=default(string),
						LastName=default(string),
						BirthDate=default(DateTimeOffset),
						RegionStr=default(string),
						LivingPlace=default(string),
					}
				}).PersonInformacia;
		return new CraPerson{PrivateNumber=o.PrivateNumber,FirstName=o.FirstName,LastName=o.LastName,BirthDate=o.BirthDate.LocalDateTime,RegionStr = o.RegionStr,LivingPlace=o.LivingPlace};
		
	}
}

void Main()
{
	var p = GetCraPerson("08001014001").Dump();
	RRCs.Where (rrc => rrc.RAI=="0103").Dump();
	RRCs
		.AsEnumerable()
		.SelectMany (rv => rv.RAI_NAME.Split(new []{' ','-','-'}).Select (rn => new { rv.RAI,RaiName=rn.Trim() }))
		.Where (rv => !rv.RaiName.Contains("რაიონ"))
		.Where (rv => !rv.RaiName.Contains("ქ."))
		.Where (rv => p.RegionStr.Contains(rv.RaiName.Substring(0,rv.RaiName.Length-2)))
	.Dump();
}

// Define other methods and classes here
