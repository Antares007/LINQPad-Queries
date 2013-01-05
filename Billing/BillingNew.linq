<Query Kind="Program">
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
	var gadaxdiliPaketebi=new HashSet<string>(this.Connection.Query<string>(@"
SELECT PaketisNomeri
FROM   [SocialuriDazgveva].[dbo].[BillingGankhorcielebuliVizitebi]
WHERE  FName in ( 'GankhorcielebuliVizitebi_165_2012xx',
                  'GankhorcielebuliVizitebi_165_2012x3',
				  'GankhorcielebuliVizitebi_165_2012x4korektirebuli',
                  'GankhorcielebuliVizitebi_165_2012x5'
                ) 
AND    (    (Chambarebeli = 'Banki'    AND Chabarda          = 'Chabarda')
         OR (Chabarda     = 'Chabarda' AND KontraktiGaformda = 'KontraktiGaformda')
         OR (Dadgenileba  = 218        AND Chambarebeli      = 'Fosta' AND KontraktiGaformda = 'KontraktiGaformda')
       )"));
	var gamortmeuliPolisebi=new HashSet<string>(this.Connection.Query<string>(@"
SELECT Polisebi
FROM [SocialuriDazgveva].[dbo].[BillingChabarebuliPaketebi]
WHERE FName in ( 'ChabarebuliPaketebi_165_2012xx_Yvela',
                 'ChabarebuliPaketebi_165_2012x3_Yvela',
				 'ChabarebuliPaketebi_165_2012x4_Yvela_korektirebuli',
                 'ChabarebuliPaketebi_165_2012x5_Yvela'
			   )")
			.SelectMany (s => s.Split(',',' '))
			.Select (s => s.Trim())
			.Where (s => s.Length == 9)
			.Where (s => s.All(Char.IsNumber))
			);	   
//return;	   
	var periodi="2012x5test";
	var chabarebebi = PolisisChabarebisIstorias
						.SelectMany(i => VGadarickhvaFull.Where(g=>g.PolisisNomeri==i.PolisisNomeri).DefaultIfEmpty().Select(g=>new {i,g}))
						//.Where (x => (x.i.Polisebi.ShekmnisTarigi.Year * 100 + x.i.Polisebi.ShekmnisTarigi.Month).ToString() == periodi)
						.Where (x => x.i.Polisebi.ProgramisId > 20)
						.Where (x => x.i.VizitisTarigi.Date < x.i.Polisebi.ChabarebisBoloVada.Date  )
						.Select (x => new {	x.i.PaketisNomeri,
											Chambarebeli = x.i.Chambarebeli == "Socagenti" || x.i.Chambarebeli == "Banki" || x.i.Chambarebeli == "Fosta" ? x.i.Chambarebeli : "AdgilidanGacema",
											x.i.Statusi,
											x.i.Polisebi.PolisisStatusi,
											x.i.Polisebi.PolisisNomeri,
											x.i.Polisebi.ShekmnisTarigi,
											x.i.Polisebi.MzgveveliKompaniisKodi,
											GaformdaKontrakti = x.g != null,
											Dadgenileba = x.i.Polisebi.ProgramisId < 20 ? 218 : 165,
										}).ToList();
	var paketebi = (
		from pci in chabarebebi
		where !gadaxdiliPaketebi.Contains(pci.PaketisNomeri)
//		where !gamortmeuliPolisebi.Contains(pci.PolisisNomeri)
		group pci by pci.PaketisNomeri into g
		let fp=g.First ()
		let polisebi=g.Select (x => new Polisi_(x.PolisisNomeri,x.MzgveveliKompaniisKodi,x.Statusi, x.PolisisStatusi, x.GaformdaKontrakti, x.Dadgenileba))
		select new Paketi_(g.Key, fp.Chambarebeli, fp.ShekmnisTarigi.ToString("yyyyMM"), polisebi)
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
//		Ext.ToExcel(gamosartmeviPaketebi      .Where (m => m.Dadgenileba == dad), "ChabarebuliPaketebi_" + dad.ToString() + "_"  + periodi + "_Yvela");
//		foreach(var g in gamosartmeviPaketebi .Where (m => m.Dadgenileba == dad) .GroupBy (g => g.MzgveveliKompaniisKodi))
//			Ext.ToExcel(g, "ChabarebuliPaketebi_" + dad.ToString() + "_" + periodi + "_" + g.Key);
	}
}


public class Paketi_{
	public Paketi_(string paketisNomeri,string chambarebeli,string periodi,IEnumerable<Polisi_> polisebi)
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
	
	public IEnumerable<Paketi_> DakhlicheMzgveveliKompaniebisMikhedvit()
	{
		if(MzgveveliKompaniebi.Count ()==1) return new []{this};
		return MzgveveliKompaniebi
					.Select (mk => new Paketi_(PaketisNomeri,Chambarebeli,Periodi,Polisebi.Where (p => p.MzgveveliKompaniisKodi==mk)))
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