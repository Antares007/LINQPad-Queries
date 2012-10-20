<Query Kind="Program">
  <Connection>
    <ID>45833638-ad3b-4b74-ad51-3184b7afbd7a</ID>
    <Driver>LinqToSql</Driver>
    <Server>triton</Server>
    <CustomAssemblyPath>D:\Dev\PolisebisDarigebaWeb\Lib\ClassLibrary3.dll</CustomAssemblyPath>
    <CustomTypeName>ClassLibrary3.CustomSocDazgvevaDataContext</CustomTypeName>
    <SqlSecurity>true</SqlSecurity>
    <Database>SocialuriDazgveva</Database>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAADxPHDwSPIL7QulpsphOG+fmVrNcZu3pDIwjy5uBeB3WAAAAAAOgAAAAAIAACAAAABfmVLxrKUQ1hdXqrs5XBhYYoMt8qEHxuLpoViWIeVy2BAAAABW/F+DJNDmafrhsMq392DwQAAAAFa3Qn792JGQobKVm9Gwp3IcDZyvhLKuEEyTKVFuS/5Y605ykBk4no/nBgyhamr3YQuRQ3Vl5LSKReOABNrmk6E=</Password>
  </Connection>
</Query>

void Main()
{
	var sadPeriodebi=Polisebi
			.Where (p => p.ShekmnisTarigi != DateTime.Parse("04.12.2011 0:00:00"))
			.Where (p => p.ProgramisId<20) ;
			
	var chasabarebeli = sadPeriodebi
			.Where (p => p.MovlenaChabarda == null)
			.Where (p => !p.PolisisChabarebisIstoriebi.Any())
			.Where (p => !p.MovlenebiGaformdaGaukmdaKontrakti.Any ())
			.Select (p => new {
					p.PolisisNomeri,
					ShekmnisPeriodi=p.ShekmnisTarigi.Year + p.ShekmnisTarigi.Month.ToString().PadLeft(2,'0'),
					DarigebisStatusi="Chasabarebeli",
					DarigebisMdebareoba = p.MovlenebiGaformdaGaukmdaKontrakti.Any() ? "GaukmdaChaubareblobisGamo" :
														(p.MovlenebiGadaecaRaions.Any()||p.MovlenebiDaibechda.Any() ?  "Raioni":
																	p.MovlenebiGadaecaFostas.Any() ? "Fosta" :
																		"...")
																});
	var chabarebuli = sadPeriodebi
			.Where (p => p.MovlenaChabarda != null)
			.Select (p => new {
					p.PolisisNomeri,
					ShekmnisPeriodi = p.ShekmnisTarigi.Year + p.ShekmnisTarigi.Month.ToString().PadLeft(2,'0'),
					DarigebisStatusi = "Chabarda",
					DarigebisMdebareoba=p.MovlenebiGaformdaGaukmdaKontrakti.Any (mggk => mggk.Statusi == "Gaformda") ? "KontraktiGaformda" : "..."
					});
	var verchabarebuli = sadPeriodebi
			.Where (p => p.MovlenaChabarda == null)
			.Where (p => p.PolisisChabarebisIstoriebi.Any ())
			.Where (p => !p.PolisisChabarebisIstoriebi.Any (pci => pci.Statusi=="Chabarda"))
			.Select (p => new {
					p.PolisisNomeri,
					ShekmnisPeriodi = p.ShekmnisTarigi.Year + p.ShekmnisTarigi.Month.ToString().PadLeft(2,'0'),
					DarigebisStatusi = "VerChabarda",
					DarigebisMdebareoba = p.PolisisChabarebisIstoriebi.First(x=>x.VizitisTarigi==p.PolisisChabarebisIstoriebi.Min(pci => pci.VizitisTarigi)).Statusi
					});
	var gaukmdaPeriodi = sadPeriodebi
			.Where (p => p.MovlenaChabarda == null)
			.Where (p => !p.PolisisChabarebisIstoriebi.Any ())
			.Where (p => p.MovlenebiGaformdaGaukmdaKontrakti.First(i=>i.Dro==p.MovlenebiGaformdaGaukmdaKontrakti.Max(x=>x.Dro)).Statusi!="Gaformda")
			.Select (p => new {
					p.PolisisNomeri,
					ShekmnisPeriodi = p.ShekmnisTarigi.Year + "" + p.ShekmnisTarigi.Month.ToString().PadLeft(2,'0'),
					DarigebisStatusi = "Gaukmda",
					DarigebisMdebareoba = p.MovlenebiGaformdaGaukmdaKontrakti.First(i=>i.Dro==p.MovlenebiGaformdaGaukmdaKontrakti.Max(x=>x.Dro)).Statusi
					});

//	Ext.ToExcel(chasabarebeli.Concat(chabarebuli).Concat(verchabarebuli).Concat(gaukmdaPeriodi),"Irakli");
	chasabarebeli.Concat(chabarebuli).Concat(verchabarebuli).Concat(gaukmdaPeriodi).ToList().Dump()
		.GroupBy (x => new {x.ShekmnisPeriodi,x.DarigebisStatusi,x.DarigebisMdebareoba})
		.Select (g => new { g.Key.ShekmnisPeriodi, g.Key.DarigebisStatusi, g.Key.DarigebisMdebareoba, Raod=g.Count ()})
		.OrderBy (g => g.DarigebisMdebareoba).OrderBy (g => g.DarigebisStatusi).OrderBy (g => g.ShekmnisPeriodi)
		.Dump();
}
