<Query Kind="Program">
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
  <Reference>D:\Dev\Libs\EPPlus\EPPlus.dll</Reference>
  <NuGetReference>Dapper</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

void Main()
{
	this.Connection.Open();
	var gadaxdiliPaketebi = new HashSet<string>(this.Connection.Query<string>(@"
SELECT PaketisNomeri
FROM   [SocialuriDazgveva].[dbo].[GankhorcielebuliVizitebi]
WHERE  FileName in (  'GankhorcielebuliVizitebi_165_2012xx'
				  ,'GankhorcielebuliVizitebi_165_2012x3'
				  ,'GankhorcielebuliVizitebi_165_2012x4korektirebuli'
				  ,'GankhorcielebuliVizitebi_165_2012x5'
				  ,'GankhorcielebuliVizitebi_165_2012x6'
				  ,'GankhorcielebuliVizitebi_165_2012x7'
				  ,'GankhorcielebuliVizitebi_165_2013x8'

				  ,'GankhorcielebuliVizitebi201203'
				  ,'GankhorcielebuliVizitebi201204'
				  ,'GankhorcielebuliVizitebi201205'
				  ,'GankhorcielebuliVizitebi201206'
				  ,'GankhorcielebuliVizitebi_218_201207'
				  ,'GankhorcielebuliVizitebi_218_201208'
				  ,'GankhorcielebuliVizitebi_218_201209'
				  ,'GankhorcielebuliVizitebi_218_201210'
				  ,'GankhorcielebuliVizitebi_218_201211'
				  ,'GankhorcielebuliVizitebi_218_201212'
				  ,'GankhorcielebuliVizitebi_218_201301'
                ) 
AND    (    (Chambarebeli = 'Banki'    AND Chabarda          = 'Chabarda')
         OR (Chabarda     = 'Chabarda' AND KontraktiGaformda = 'KontraktiGaformda')
         OR (Dadgenileba  = 218        AND Chambarebeli      = 'Fosta' AND KontraktiGaformda = 'KontraktiGaformda')
       )"));
//	var gamortmeuliPolisebi = new HashSet<string>(this.Connection.Query<string>(@"
//SELECT PolisisNomeri
//FROM [SocialuriDazgveva].[dbo].[ChabarebuliPaketebi]
//WHERE FileName in ( 'ChabarebuliPaketebi_165_2012xx_Yvela'
//				,'ChabarebuliPaketebi_165_2012x3_Yvela'
//				,'ChabarebuliPaketebi_165_2012x4_Yvela_korektirebuli'
//				,'ChabarebuliPaketebi_165_2012x5_Yvela'
//				,'ChabarebuliPaketebi_165_2012x6_Yvela'
//				,'ChabarebuliPaketebi_165_2012x7_Yvela'
//			    ,'ChabarebuliPaketebi_165_2013x8_Yvela'
//			 
//			    ,'ChabarebuliPaketebi_201203_Yvela'
//			    ,'ChabarebuliPaketebi_201204_Yvela'
//			    ,'ChabarebuliPaketebi_201205_Yvela'
//			    ,'ChabarebuliPaketebi_201206_Yvela'
//			    ,'ChabarebuliPaketebi_218_201207_Yvela'
//			    ,'ChabarebuliPaketebi_218_201208_Yvela'
//			    ,'ChabarebuliPaketebi_218_201209_Yvela'
//			    ,'ChabarebuliPaketebi_218_201210_Yvela'
//			    ,'ChabarebuliPaketebi_218_201211_Yvela'
//			    ,'ChabarebuliPaketebi_218_201212_Yvela'
//			    ,'ChabarebuliPaketebi_218_201301_Yvela'
//			   )"));	   
//	gadaxdiliPaketebi.Count().Dump();
//	gamortmeuliPolisebi.Count().Dump();

	var periodi="2013x9";
	var chabarebebi = PolisisChabarebisIstorias
						.Where (i => i.Polisebi.ShekmnisTarigi>DateTime.Parse("2012-10-01"))
						.SelectMany(i => VGadarickhvaFull.Where(g=>g.PolisisNomeri==i.PolisisNomeri).DefaultIfEmpty().Select(g => new { i, g }))
//						.Where (x => (x.i.Polisebi.ShekmnisTarigi.Year * 100 + x.i.Polisebi.ShekmnisTarigi.Month).ToString() == periodi)
//						.Where (x => x.i.Polisebi.ProgramisId < 20)
						.Where (x => x.i.VizitisTarigi.Date < x.i.Polisebi.ChabarebisBoloVada.Date  )
						.Select (x => new {	x.i.PaketisNomeri,
											Chambarebeli = x.i.Chambarebeli == "Socagenti" || x.i.Chambarebeli == "Banki" || x.i.Chambarebeli == "Fosta" ? x.i.Chambarebeli : "AdgilidanGacema",
											x.i.Statusi,
											x.i.Polisebi.PolisisStatusi,
											x.i.Polisebi.PolisisNomeri,
											x.i.Polisebi.ShekmnisTarigi,
											x.i.Polisebi.MzgveveliKompaniisKodi,
											GaformdaKontrakti = x.g != null,
                                            VizitisPeriodi = x.i.VizitisTarigi.Year*100+x.i.VizitisTarigi.Month,
											Dadgenileba = x.i.Polisebi.ProgramisId < 20 ? 218 : 165,
										}).ToList();
	return;
	var paketebi = (
		from pci in chabarebebi
//		where !gadaxdiliPaketebi.Contains(pci.PaketisNomeri)
//		where !gamortmeuliPolisebi.Contains(pci.PolisisNomeri)
		group pci by pci.PaketisNomeri into g
		let fp=g.First ()
		let polisebi=g.Select (x => new Polisi_(x.PolisisNomeri,x.MzgveveliKompaniisKodi,x.Statusi, x.PolisisStatusi, x.GaformdaKontrakti, x.Dadgenileba))
		select new Paketi_(g.Key, fp.Chambarebeli, fp.ShekmnisTarigi.ToString("yyyyMM"), polisebi,fp.VizitisPeriodi)
		);
		
var amodubluliPaketebi=paketebi
	.Select (p => new {	
						Paketi = p,
						UnikaluriKey = string.Join(";", p.Polisebi.Select (po => po.PolisisNomeri).OrderBy (po => po))
					  })
	.GroupBy (x => x.UnikaluriKey)
	.Select (g => new {
		Fosta=g.FirstOrDefault(x=>x.Paketi.Chambarebeli=="Fosta" && x.Paketi.Chabarda),
		Banki=g.FirstOrDefault(x=>x.Paketi.Chambarebeli=="Banki" && x.Paketi.Chabarda),
		Socagenti=g.FirstOrDefault(x=>x.Paketi.Chambarebeli=="Socagenti" && x.Paketi.Chabarda),
		Chabarda=g.FirstOrDefault(x=>x.Paketi.Chabarda),
		First=g.First(),
		})
	.Select (x => x.Fosta!=null?x.Fosta.Paketi:
					x.Banki!=null?x.Banki.Paketi:
						x.Socagenti!=null?x.Socagenti.Paketi:
							x.Chabarda!=null?x.Chabarda.Paketi:
								x.First.Paketi).ToList();
amodubluliPaketebi.Where (p => p.KontraktiGaformda).GroupBy (p => p.Dadgenileba).Select (g => new {g.Key,Raod=g.Count()}).Dump();

var q = amodubluliPaketebi.Select(x => new { 
											x.PaketisNomeri,
											Chabarda=x.Chabarda ? "Chabarda" : "VerChabarda",
											KontraktiGaformda=x.KontraktiGaformda ? "KontraktiGaformda" : "KontraktiVerGaformda",
											x.Chambarebeli,
											x.Periodi, 
											x.Polisebi,
											x.Dadgenileba,
											x.VizitisTarigi,
                                            x.VizitisPeriodi,
								});
var misacemi = q
				
				.Select (x => new {         x.PaketisNomeri,
											x.Chabarda,
											x.KontraktiGaformda,
											x.Chambarebeli,
											x.Periodi, 
											Polisebi=string.Join(", ", x.Polisebi.Select (pol => pol.ToString())),
											x.Dadgenileba,
											x.VizitisTarigi,
                                            x.VizitisPeriodi,
								})
								.ToList();

var gamosartmeviPaketebi=amodubluliPaketebi
		.SelectMany (p => p.DakhlicheMzgveveliKompaniebisMikhedvit()
								.Where (x => x.KontraktiGaformda && x.Chabarda)
					)
		
		.Select (x => new {
					MzgveveliKompaniisKodi=x.MzgveveliKompaniebi.First (),
					Periodi=x.Periodi,
					x.PaketisNomeri,
					x.Dadgenileba,
					x.VizitisTarigi,
					Polisebi=string.Join(", ", x.Polisebi.Select(i => i.ToString()))
					})
		.ToList();


	foreach (var dad in new []{218, 165})
	{
		
		Ext.ToExcel(misacemi.Where (m => m.Dadgenileba == dad), "GankhorcielebuliVizitebi_" + dad.ToString() + "_" + periodi);
		Ext.ToExcel(gamosartmeviPaketebi      .Where (m => m.Dadgenileba == dad), "ChabarebuliPaketebi_" + dad.ToString() + "_"  + periodi + "_Yvela");
		foreach(var g in gamosartmeviPaketebi .Where (m => m.Dadgenileba == dad) .GroupBy (g => g.MzgveveliKompaniisKodi))
			Ext.ToExcel(g, "ChabarebuliPaketebi_" + dad.ToString() + "_" + periodi + "_" + g.Key);
	}
}


public class Paketi_{
	public Paketi_(string paketisNomeri,string chambarebeli,string periodi,IEnumerable<Polisi_> polisebi,int vizitisPeriodi)
	{
		if(polisebi.Select (p => p.Dadgenileba).Distinct().Count ()!=1)
			throw new InvalidOperationException();	
		PaketisNomeri=paketisNomeri;
		Chambarebeli=chambarebeli;
		Periodi=periodi;
		Polisebi=polisebi.ToList();
		Chabarda=Polisebi.Any (x => x.Statusi=="Chabarda");
		KontraktiGaformda=Polisebi.Any (x => x.GaformdaKontrakti);
		MzgveveliKompaniebi=Polisebi.Select (po => po.MzgveveliKompaniisKodi).Distinct().ToList();
		VizitisPeriodi=vizitisPeriodi;
		Dadgenileba=Polisebi.First ().Dadgenileba;
	}
	public readonly string PaketisNomeri;
	public readonly string Chambarebeli;
	public readonly string Periodi;
	public readonly bool Chabarda;
	public readonly bool KontraktiGaformda; 
	public readonly IEnumerable<Polisi_> Polisebi;
	public readonly IEnumerable<string> MzgveveliKompaniebi;
	public readonly int Dadgenileba;
	public readonly DateTime VizitisTarigi;
	public readonly int VizitisPeriodi;
	public IEnumerable<Paketi_> DakhlicheMzgveveliKompaniebisMikhedvit()
	{
		if(MzgveveliKompaniebi.Count ()==1) return new []{this};
		return MzgveveliKompaniebi
					.Select (mk => new Paketi_(PaketisNomeri,Chambarebeli,Periodi,Polisebi.Where (p => p.MzgveveliKompaniisKodi==mk),VizitisPeriodi))
					.ToList();
	}
	
}

public class Polisi_{
	public Polisi_(string polisisNomeri, string mzgveveliKompaniisKodi, string statusi, string polisisStatusi, bool gaformdaKontrakti, int dadgenileba)
	{
		PolisisNomeri=polisisNomeri;
		MzgveveliKompaniisKodi=mzgveveliKompaniisKodi;
		Statusi=statusi;
		PolisisStatusi=polisisStatusi;
		GaformdaKontrakti=gaformdaKontrakti;
		Dadgenileba = dadgenileba;
	}
	public readonly string PolisisNomeri;
	public readonly string MzgveveliKompaniisKodi;
	public readonly string Statusi;
	public readonly string PolisisStatusi;
	public readonly bool GaformdaKontrakti;
	public readonly int Dadgenileba;
	
	public override string ToString()
	{
		return string.Format("{0} - ({1}, {2})", PolisisNomeri, Statusi, PolisisStatusi);
	}
}

public static class Ext {
	public static IEnumerable<T> ToExcel<T>(IEnumerable<T> items, string filename=null)
	{
		using (ExcelPackage p = new ExcelPackage())
		{
			p.Workbook.Worksheets.Add("Data");
			ExcelWorksheet ws = p.Workbook.Worksheets[1];
			ws.Name = "Data"; //Setting Sheet's name
			var columns = typeof(T).GetProperties().Where (x => x.CanRead && (x.PropertyType.IsValueType||x.PropertyType==typeof(string))).ToDictionary(k=>k.Name);
	
			int colIndex = 1;
			int rowIndex = 1;
	
			
			foreach (var dc in columns.Keys) //Creating Headings
			{
				var cell = ws.Cells[rowIndex, colIndex];
				var fill = cell.Style.Fill;
				fill.PatternType = ExcelFillStyle.Solid;
				fill.BackgroundColor.SetColor(Color.Gray);
				var border = cell.Style.Border;
				border.Bottom.Style = 
					border.Top.Style = 
					border.Left.Style = 
					border.Right.Style = ExcelBorderStyle.Thin;
				cell.Value = dc;
				colIndex++;
			}
			foreach (var dr in items) 
			{
				colIndex = 1;
				rowIndex++;
				foreach (var colName  in columns.Keys)
				{
					var cell = ws.Cells[rowIndex, colIndex];
					cell.Value = columns[colName].GetValue(dr, null);
					var border = cell.Style.Border;
					border.Left.Style =
						border.Right.Style = ExcelBorderStyle.Thin;
					colIndex++;
				}
			}
			colIndex = 0;
			Byte[] bin = p.GetAsByteArray();
			string file = "c:\\temp\\" + (filename?? Guid.NewGuid().ToString()) + ".xlsx";
			File.WriteAllBytes(file, bin);
		}
		return items;
	}
}