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
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

void Main()
{
var dataOptions = new System.Data.Linq.DataLoadOptions();
dataOptions.LoadWith<PolisisChabarebisIstoria>(i => i.Polisebi);
dataOptions.LoadWith<Polisebi>(p => p.MovlenebiGaformdaGaukmdaKontraktis);
this.LoadOptions = dataOptions;

var periodi="201203";
var q=PolisisChabarebisIstorias
				.Where (pci => pci.Polisebi.ShekmnisTarigi.Year+""
								+pci.Polisebi.ShekmnisTarigi.Month.ToString().PadLeft(2,'0')==periodi)
				.ToList();
				
var paketebi=q.GroupBy (p => p.PaketisNomeri)
				.Select (g => new {g.Key,PolisebiKey= string.Join(";",g.Select (x => x.PolisisNomeri).OrderBy (x => x))})
				.GroupBy (p => p.PolisebiKey)
				.Select (g => g.First ().Key).ToList();
q.Count ().Dump();
q=q.Where (x => paketebi.Contains(x.PaketisNomeri)).ToList();
q.Count ().Dump();				


	var dic = q
				 .AsParallel()
			 	 .GroupBy (pci => pci.PaketisNomeri)
				 .ToDictionary (k => k.Key, v => v.ToList());

	var vizitebi = q
					.GroupBy(pci => new {
								pci.PaketisNomeri, 
								pci.Chambarebeli, 
								pci.Polisebi.ShekmnisTarigi})
					.Select(g =>
						new {
								g.Key.PaketisNomeri,
								g.Key.Chambarebeli,
								g.Key.ShekmnisTarigi,
								Chabarda=g.Any (x => x.Statusi=="Chabarda" ) ? "Chabarda" : "VerChabarda",
								KontraktiGaformda=g.Any (x =>   (x.Polisebi.PolisisStatusi=="Aktiuri" || 
												                 x.Polisebi.PolisisStatusi=="KontraqtisMoqmedebisVadaAmoicura")
															  	 //&& !valdebulebebiArakvs.Contains(x.Polisi.PolisisNomeri)
															  ) 
													
													? "KontraktiGaformda" : "KontraktiVerGaformda"
							})
					.ToList().AsParallel()
					.Select (x => new { 
								x.PaketisNomeri,
								x.Chabarda,
								x.KontraktiGaformda,
								x.Chambarebeli,
								Periodi=x.ShekmnisTarigi.ToString("yyyyMM"), 
								Polisebi=string.Join(", ", dic[x.PaketisNomeri].Select(i => i.PolisisNomeri + " - " + i.Statusi)),
								});
							
	Ext.ToExcel(vizitebi, "GankhorcielebuliVizitebi" + periodi);
//return;
	var gamosartmevebi = q
							.Where (pci => pci.Statusi=="Chabarda")
							.Where (pci =>  (!pci.Polisebi.MovlenebiGaformdaGaukmdaKontraktis.Any ()
												|| pci.Polisebi.MovlenebiGaformdaGaukmdaKontraktis
												.First (mggk => mggk.Dro==pci.Polisebi.MovlenebiGaformdaGaukmdaKontraktis.Max (m => m.Dro))
												.Statusi=="Gaformda" 
												)
											//&& !valdebulebebiArakvs.Contains(pci.Polisi.PolisisNomeri)	
												)
							.GroupBy (pci => new {pci.PaketisNomeri,pci.Polisebi.MzgveveliKompaniisKodi})
							.Select (g => new {g.Key.PaketisNomeri, g.Key.MzgveveliKompaniisKodi,ShekmnisTarigi=g.Max (x => x.Polisebi.ShekmnisTarigi)})
							.AsEnumerable()
							.Select (x => new {x.MzgveveliKompaniisKodi,Periodi=x.ShekmnisTarigi.ToString("yyyyMM"),x.PaketisNomeri,Polisebi=string.Join(", ", dic[x.PaketisNomeri].Select(i => i.PolisisNomeri + " - " + i.Statusi))})
							.ToList();
	
	Ext.ToExcel(gamosartmevebi, "ChabarebuliPaketebi_" + periodi + "_Yvela");
	
	foreach(var g in gamosartmevebi.GroupBy (g => g.MzgveveliKompaniisKodi))
		Ext.ToExcel(g, "ChabarebuliPaketebi_" + periodi + "_" + g.Key);
}

public static class Ext {
	public static IEnumerable<T> ToExcel<T>(IEnumerable<T> items, string filename=null)
	{
		using (ExcelPackage p = new ExcelPackage())
		{
			//Here setting some document properties
//			p.Workbook.Properties.Author = "Zeeshan Umar";
//			p.Workbook.Properties.Title = "Office Open XML Sample";
	
			//Create a sheet
			p.Workbook.Worksheets.Add("Data");
			ExcelWorksheet ws = p.Workbook.Worksheets[1];
			ws.Name = "Data"; //Setting Sheet's name
			//ws.Cells.Style.Font.Size= 11; //Default font size for whole sheet
			//ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
	
			//DataTable dt = CreateDataTable(); //My Function which generates DataTable
	
			var columns = typeof(T).GetProperties().Where (x => x.CanRead && (x.PropertyType.IsValueType||x.PropertyType==typeof(string))).ToDictionary(k=>k.Name);
			
			//Merging cells and create a center heading for out table
//			ws.Cells[1, 1].Value = "Sample DataTable Export";
//			ws.Cells[1, 1, 1, columns.Keys.Count()].Merge = true;
//			ws.Cells[1, 1, 1, columns.Keys.Count()].Style.Font.Bold = true;
//			ws.Cells[1, 1, 1, columns.Keys.Count()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
	
			int colIndex = 1;
			int rowIndex = 1;
	
			
			foreach (var dc in columns.Keys) //Creating Headings
			{
				var cell = ws.Cells[rowIndex, colIndex];
	
				//Setting the background color of header cells to Gray
				var fill = cell.Style.Fill;
				fill.PatternType = ExcelFillStyle.Solid;
				fill.BackgroundColor.SetColor(Color.Gray);
	
	
				//Setting Top/left,right/bottom borders.
				var border = cell.Style.Border;
				border.Bottom.Style = 
					border.Top.Style = 
					border.Left.Style = 
					border.Right.Style = ExcelBorderStyle.Thin;
	
				//Setting Value in cell
				cell.Value = dc;
	
				colIndex++;
			}
	
			foreach (var dr in items) // Adding Data into rows
			{
				colIndex = 1;
				rowIndex++;
				foreach (var colName  in columns.Keys)
				{
					var cell = ws.Cells[rowIndex, colIndex];
					//Setting Value in cell
					
					cell.Value = columns[colName].GetValue(dr, null);
	
					//Setting borders of cell
					var border = cell.Style.Border;
					border.Left.Style =
						border.Right.Style = ExcelBorderStyle.Thin;
					colIndex++;
				}
			}
	
			colIndex = 0;
	//		foreach (var dc in columns) //Creating Headings
	//		{
	//			colIndex++;
	//			var cell = ws.Cells[rowIndex, colIndex];
	// 
	//			//Setting Sum Formula
	//			cell.Formula = "Sum("+ 
	//							ws.Cells[3, colIndex].Address+
	//							":"+
	//							ws.Cells[rowIndex-1, colIndex].Address+
	//							")";
	// 
	//			//Setting Background fill color to Gray
	//			cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
	//			cell.Style.Fill.BackgroundColor.SetColor(Color.Gray);
	//		}
	
			//Generate A File with Random name
			Byte[] bin = p.GetAsByteArray();
			string file = "c:\\temp\\" + (filename?? Guid.NewGuid().ToString()) + ".xlsx";
			File.WriteAllBytes(file, bin);
			Process.Start(file);
		}
		return items;
	}
}