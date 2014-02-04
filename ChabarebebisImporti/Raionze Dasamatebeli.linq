<Query Kind="Statements">
  <Connection>
    <ID>393fc2a1-3d2f-4fc3-8b9d-c916c0bb1c52</ID>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA2LGCigg0bUqfvV+5fr69FwAAAAACAAAAAAAQZgAAAAEAACAAAACcweYNfVdHbduk84GFtEdAPuSkqyq1f325WOB4ML95NAAAAAAOgAAAAAIAACAAAAD36Sg/oROiTUg/42mIUi6NiAfDI74nSKxIv07xmKcvtBAAAAB2muXkj5NigwmlBpZY/1pIQAAAAFksUka7cbQZQT6Q1yIE0ma77Yk31wW4wRSQ4l3uFjF52bpNqR0IC7RQ8f9J/ibc6PgXSfcWBPmK6ilXFruEoz8=</Password>
    <Database>SocialuriDazgveva</Database>
    <ShowServer>true</ShowServer>
    <Persist>true</Persist>
  </Connection>
</Query>

Polisebis
	.Where (p => p.PolisisStatusi == null)
//	.Where (p => !GadaecaFostas.Any( gf => gf.PolisisNomeri==p.PolisisNomeri))
//	.Where (p => !BankzeGadasacemiPaketisPolisebi_alls.Any( gf => gf.PolisisNomeri==p.PolisisNomeri))
//	.Where (p => !GadaecaRaions.Any( gf => gf.PolisisNomeri==p.PolisisNomeri))
	.GroupBy(p => new{p.ShekmnisTarigi,p.ChabarebisBoloVada, Dadgenileba = p.ProgramisId < 20 ? 218 : 165 })
	.Select (g => new {g.Key.ShekmnisTarigi, g.Key.ChabarebisBoloVada, g.Key.Dadgenileba, Raod=g.Count ()})
	.OrderByDescending (x => x.ShekmnisTarigi)
	//.Dump()
	;
//return;
var dt = "2013-03-01";
var id = 90053;
//110117910671
var pols = Polisebis
			.Where (p => p.ProgramisId < 20)
			.Where (p => p.ChabarebisBoloVada >= DateTime.Parse("2014-01-01"))
			.Where (p => p.PolisisStatusi == null)
			.Where (p => p.DasarigebeliPolisebi == null)
			.AsEnumerable()
			.Select (p => new DasarigebeliPolisebi { PolisisNomeri = p.PolisisNomeri, Dasarigebeli = id, Tarigi = DateTime.Today })
			.ToList();
				
pols.Count ().Dump();

DasarigebeliPolisebis.InsertAllOnSubmit(pols);
SubmitChanges();
DasarigebeliPolisebis.Where (dp => dp.Dasarigebeli==id ).Count ( ).Dump();