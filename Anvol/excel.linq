<Query Kind="Program">
  <NuGetReference>ExcelLibrary</NuGetReference>
  <Namespace>ExcelLibrary.SpreadSheet</Namespace>
</Query>

public PakinkListi AmoikePackingListi(ExcelisFaili excelisFaili)
{
	foreach(var shiiti in excelisFaili.Shitebi)
	{
		if (shiiti.Sekciebi.length != 2) continue;
		
		var satauri = shiiti.Sekciebi[0].Tipi != ;
		var pakingCkhrili = CaikitkheSataturi(shiiti.Sekciebi[0]);
	}
}

void Main()
{
	//var pakingListi = AmoikePackingListi();
	var q = 
	(
		from filename in DirSearch(@"D:\Anvol")
		let wb = Workbook.Load(filename)
		from ws in wb.Worksheets
		from row in ws.Cells.Rows
		let cols = Enumerable.Range(row.Value.FirstColIndex,(row.Value.LastColIndex-row.Value.FirstColIndex)+1).Select (i => row.Value.GetCell(i))
		where cols.Any (c => c.StringValue.ToLower()=="item nr.")
		select new {filename,ws.Name,row,cols=cols.Select (i => i.Value)}
	)
	;

	( 	
		from x in q
		group x.filename by string.Join(", ",x.cols) into g
		select new {g.Key,Files=g.ToList()}
	)
	.Distinct()
	.Dump()
	;
return;

	
//	wb.Worksheets[0].Cells.Rows
//			.GroupBy (r => new {r.Value.FirstColIndex,r.Value.LastColIndex})
//			.Select (g => new {g.Key,Raod=g.Count ()})
//			.Dump();
//	
//	for (int r = 0; r < 100000; r++)
//	{
//		for (int c = 0; c < 255; c++)
//		{
//		}
//	}
	
}

IEnumerable<string> DirSearch(string sDir) 
{
   foreach (string d in Directory.GetDirectories(sDir)) 
   {
		foreach (string f in Directory.GetFiles(d, "*.xls")) 
		{
			yield return f;
		}
		DirSearch(d);
   }
}