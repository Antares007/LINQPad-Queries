<Query Kind="Program">
  <Connection>
    <ID>45833638-ad3b-4b74-ad51-3184b7afbd7a</ID>
    <Persist>true</Persist>
    <Driver>LinqToSql</Driver>
    <Server>triton</Server>
    <CustomAssemblyPath>C:\Temp\ClassLibrary3\ClassLibrary3\bin\Debug\ClassLibrary3.dll</CustomAssemblyPath>
    <CustomTypeName>ClassLibrary3.CustomSocDazgvevaDataContext</CustomTypeName>
    <SqlSecurity>true</SqlSecurity>
    <Database>SocialuriDazgveva</Database>
    <UserName>DarigebaDzebnisSamsaxuri</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA3XCANzX8A0+HtkxmZtXFGAAAAAACAAAAAAAQZgAAAAEAACAAAAD+ZsZvKoa4DrLJhG9tCzXWhjtCv48LvJsIivHmOBF5wwAAAAAOgAAAAAIAACAAAABPkJWWnUeORKNVmkaG+FTUF4jBjGZpCpIqmV9AsGOxcxAAAAB2Oh1GL7WTOTfA2eg2BtI0QAAAAA2d/39Fnh2p1+56KXdbJfvqo5EYPLCcvcL7ATAWmyGB+Bkr+ffceLZ4oCWkCel6YLeIMv6JmGdptoTfZ6ymTLM=</Password>
  </Connection>
  <Reference>C:\Temp\EPPlus.dll</Reference>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

void Main()
{
	var dekemberi=PolisisChabarebisIstoriebi
		.Where (p => p.Polisi.MzgveveliKompaniisKodi == "ALP")
		.Where (p => p.Polisi.ShekmnisTarigi.Month==11)
		.Where (p => p.Statusi=="Chabarda").Select(p=>new {p.PaketisNomeri,p.PolisisNomeri,p.Chambarebeli,p.Statusi,p.Polisi.ShekmnisTarigi,p.Polisi.SadazgveoperiodisDasackisi});
	
	Ext.ToExcel(dekemberi, "DekembrisChabarebuliPolisebiALP" );
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
// Define other methods and classes here
