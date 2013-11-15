<Query Kind="Statements">
  <Connection>
    <ID>393fc2a1-3d2f-4fc3-8b9d-c916c0bb1c52</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAN27c6lA2WkeiuPoKsF6zVAAAAAACAAAAAAAQZgAAAAEAACAAAACbLLAsNsDt7paFHc5L9mKrBtKrPuHB+O2Pe8qohNTpzwAAAAAOgAAAAAIAACAAAAARwiWVIvi4yqvM5LGOqZqOnRK7TKpCRjPXZ7PkGNEFvxAAAAA0DMltrt8SinSUpeRajEf6QAAAACzIm5Lx/1cDUpvEPcBjgOVVcG4y9njzOG5jRo3BFxCqffNXq8PX7vXvt6t2LlSsl7mTqha7tgg+9E46F4mXHMQ=</Password>
    <Database>SocialuriDazgveva</Database>
    <ShowServer>true</ShowServer>
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
var id = 90041;

var pols = Polisebis
			.Where (p => p.ProgramisId < 20)
			.Where (p => p.ChabarebisBoloVada >= DateTime.Parse("2013-12-01"))
			.Where (p => p.PolisisStatusi == null)
			.Where (p => p.DasarigebeliPolisebi == null)
			.AsEnumerable()
			.Select (p => new DasarigebeliPolisebi { PolisisNomeri = p.PolisisNomeri, Dasarigebeli = id, Tarigi = DateTime.Today })
			.ToList();
				
pols.Count ().Dump();

DasarigebeliPolisebis.InsertAllOnSubmit(pols);
SubmitChanges();
DasarigebeliPolisebis.Where (dp => dp.Dasarigebeli==id ).Count ( ).Dump();