<Query Kind="Statements">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAAAD89x4SL38S/4r7NUU2iHNNTcmnVTi1xQPx1AC1vAtFAAAAAAOgAAAAAIAACAAAABgO+xVBio0IKzceIXWbWfgFv0jcxQpOA9YilhDtPA8XxAAAABJcM5+MLInsGd5jUUGfXtnQAAAALHy7sVof5cKLhfpSHxbLdPESALwKOWLElOgeYcZmaeqO1sDG9SHnPN5xOzSlBHlSD7agk+KC9dH/4XMZn4gYvM=</Password>
    <Database>SocialuriDazgveva</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

//Polisebis
//	.Where (p => p.PolisisStatusi == null)
//	.Where (p => !GadaecaFostas.Any( gf => gf.PolisisNomeri==p.PolisisNomeri))
//	.Where (p => !BankzeGadasacemiPaketisPolisebi_alls.Any( gf => gf.PolisisNomeri==p.PolisisNomeri))
//	.Where (p => !GadaecaRaions.Any( gf => gf.PolisisNomeri==p.PolisisNomeri))
//	.GroupBy(p => new{p.ShekmnisTarigi,p.ChabarebisBoloVada, Dadgenileba = p.ProgramisId < 20 ? 218 : 165 })
//	.Select (g => new {g.Key.ShekmnisTarigi, g.Key.ChabarebisBoloVada, g.Key.Dadgenileba, Raod=g.Count ()})
//	.OrderByDescending (x => x.ShekmnisTarigi)
//	.Dump();
//return;
var dt="2012-12-11";
var id=83755;


var pols = Polisebis
            .Where (p => p.PolisisNomeri == "039265722")
//			.Where (p => p.ProgramisId < 20)
//			.Where (p => p.ShekmnisTarigi > DateTime.Parse("2012-11-01"))
//			.Where (p => p.ChabarebisBoloVada > DateTime.Parse("2012-12-31"))
			.Where (p => p.PolisisStatusi == null)
			.Where (p => p.DasarigebeliPolisebi == null)
			.AsEnumerable()
			.Select (p => new DasarigebeliPolisebi { PolisisNomeri = p.PolisisNomeri, Dasarigebeli = id, Tarigi = DateTime.Today })
			.ToList();
				
pols.Count ().Dump();
//return;

DasarigebeliPolisebis.InsertAllOnSubmit(pols);
SubmitChanges();
DasarigebeliPolisebis.Where (dp => dp.Dasarigebeli==id ).Count ( ).Dump();