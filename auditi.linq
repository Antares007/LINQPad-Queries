<Query Kind="Statements">
  <Connection>
    <ID>ba81614b-7b3d-4771-990b-3896ca4c920e</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <Database>tempdb</Database>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAN27c6lA2WkeiuPoKsF6zVAAAAAACAAAAAAAQZgAAAAEAACAAAADOjBuabYrQXKzD+V5qCDHhnUl31vD/RtKQgj8M/zhT3QAAAAAOgAAAAAIAACAAAADtNpfv/JBmG56Wxv7I5SFtcCkwLuWSP2anF9nVs/OWlBAAAAAvx0hFOmPNsjUCXW4z8oMBQAAAABUjFDZT2yCpArlijx7Yp1DWZWt6jf5mlNPfFxwtNfLCy1SoahQtA4+rE3x8wiU94VkgRdPH5FNYRPhPdUUFZOE=</Password>
    <IncludeSystemObjects>true</IncludeSystemObjects>
    <LinkedDb>DazgvevaGanckhadebebi</LinkedDb>
    <LinkedDb>INSURANCEW</LinkedDb>
    <LinkedDb>Pirvelckaroebi</LinkedDb>
    <LinkedDb>SocialuriDazgveva</LinkedDb>
    <LinkedDb>UketesiReestri</LinkedDb>
  </Connection>
</Query>

var chabarebebi = (
	from c in this.SocialuriDazgveva.MovlenaChabarebebis
	join g in this.SocialuriDazgveva.VGadarickhvaFull on c.PolisisNomeri equals g.PolisisNomeri
	join p in this.SocialuriDazgveva.Polisebis on c.PolisisNomeri equals p.PolisisNomeri
	join r in this.Pirvelckaroebi.RRCs.Select (rrc => new {rrc.REGION_ID,rrc.REGION_NAME}).Distinct() on p.RaionisKodi.Substring(0,2) equals r.REGION_ID
	where DateTime.Parse("2012-01-01") <= c.ChabarebisTarigi && c.ChabarebisTarigi < DateTime.Parse("2013-01-01")
	let Chambarebeli = c.Chambarebeli == "Fosta" || c.Chambarebeli == "Banki" ? c.Chambarebeli : "ChvensMierGacema"
	let ChabarebisTarigi = c.ChabarebisTarigi.Year * 100 + c.ChabarebisTarigi.Month
	let Debuleba = p.ProgramisId < 20 ? 218 : 165
	let Raioni =  r.REGION_NAME
	select new {Chambarebeli, Raioni, ChabarebisTarigi, Debuleba, c.PolisisNomeri}
).Take(10).Dump();

//(
//	from c in chabarebebi
//	group c by new {c.Chambarebeli, c.ChabarebisTarigi, c.Debuleba, c.Raioni} into g
//	select new {g.Key.Chambarebeli,  g.Key.ChabarebisTarigi, g.Key.Debuleba, g.Key.Raioni, Raodenoba=g.Count ()}
//);