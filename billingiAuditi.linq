<Query Kind="Program">
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
  <Reference>D:\Dev\ExcelReader\ExcelReader.XlsReader\bin\Debug\ExcelReader.dll</Reference>
  <Reference>D:\Dev\ExcelReader\ExcelReader.XlsReader\bin\Debug\ExcelReader.XlsReader.dll</Reference>
  <Reference>D:\Dev\ExcelReader\ExcelReader.XlsxReader\bin\Debug\ExcelReader.XlsxReader.dll</Reference>
  <Reference>D:\Dev\Nashti.Core\Nashti.Core\bin\Debug\Nashti.Core.dll</Reference>
  <Reference>D:\RavenDB-Build-2700\Client\Raven.Abstractions.dll</Reference>
  <Reference>D:\RavenDB-Build-2700\Client\Raven.Client.Lightweight.dll</Reference>
  <NuGetReference>Dapper</NuGetReference>
  <NuGetReference>EPPlus</NuGetReference>
  <NuGetReference>ServiceStack.Text</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>ExcelReader</Namespace>
  <Namespace>Nashti.Core</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

void Main()
{
//	var conn = new SqlConnection(this.Connection.ConnectionString);
//	conn.Open();
//	conn.Query("select top 10 FileName FilePath,MzgveveliKompaniisKodi from ChabarebuliPaketebi").Dump();
//	conn.Close();conn.Dispose();
//	return;
	var importer = MakeExcelImporter();
	(
		from wb in DirWalk(@"D:\temp\billingi").Select (x => importer.Import(x))
		from sheet in wb.Sheets
		where sheet.Name=="Data"
		from row in sheet.Rows
		select new {
			wb.FilePath,
			MzgveveliKompaniisKodi=row.Cells[0].AsString(),
			Periodi=row.Cells[1].AsString(),
			PaketisNomeri=row.Cells[2].AsString(),
			Polisebi=row.Cells[3].AsString(),
			}
		into s0
		where s0.MzgveveliKompaniisKodi != "MzgveveliKompaniisKodi"
		let PolisisNomrebi = s0.Polisebi.IndexOf('(') > 0 
						? s0.Polisebi.Split(new[]{')'},StringSplitOptions.RemoveEmptyEntries).Select (p => p.Split(new []{'-',' ','(',','},StringSplitOptions.RemoveEmptyEntries))
						: s0.Polisebi.Split(new[]{','},StringSplitOptions.RemoveEmptyEntries).Select (p => p.Split(new []{'-',' ','(',','},StringSplitOptions.RemoveEmptyEntries))
		from polNo in PolisisNomrebi
		select new {s0.FilePath,s0.MzgveveliKompaniisKodi,s0.Periodi,s0.PaketisNomeri,PolisisNomeri=polNo[0],ChabarebisStatusi=polNo[1],PolisisStatusi=polNo.Length>2 ? polNo[2] : null}
		
	)
	.DumpToExcel("billingi");
//	.GroupBy (x => x.FilePath)
//	.Select(fg => new {
//			fg.Key, 
//			Kompaniebi =  fg.GroupBy (x => x.MzgveveliKompaniisKodi)
//							.Select (mg => new {
//											mg.Key,
//											Raod = mg.Select (x => x.PaketisNomeri)
//													 .Distinct()
//													 .Count (),
////											SheidlebaAmogeba= mg.GroupBy (m => m.PaketisNomeri).Where (pg => pg.All(x => x.ChabarebisStatusi!="Chabarda"))
//											
//									})
//							.OrderBy (mg => mg.Key)
//		})
//	.OrderBy (fg => fg.Key)
//	.Dump();
}




IEnumerable<Dokumenti> ExcelisFailebi()
{
	var yvelaFaili = DirWalk(@"D:\Anvol\modzraobebi")
			 .Concat(DirWalk(@"D:\Anvol\Invoicebi\PakingLists"))
			 .Concat(DirWalk(@"D:\Anvol\Invoicebi\PakingLists2"))
			 .Concat(DirWalk(@"D:\Anvol\Invoicebi\PakingLists3"))
			 .Concat(DirWalk(@"D:\Anvol\Invoicebi\PakingLists4"))
			 .Concat(DirWalk(@"D:\Anvol\Invoicebi\PakingLists5"))
			 .Concat(DirWalk(@"D:\Anvol\Invoicebi\PakingLists6"))
			 .Concat(DirWalk(@"D:\Anvol\Invoicebi\PakingLists7"))
			 .Concat(DirWalk(@"D:\Anvol\Invoicebi\PakingLists8"))
			 .Select (x => x.ToLowerInvariant());
		
	var dokumentisKarkhnebisCnobari = new Dictionary<string, Func<WorkBook, Sheet, Dokumenti>>()
	{
		{"PakingListi",	PakingListisKarkhana},
		{"Gakidva",	GakidvisKarkhana},
		{"Gadaadgileba", GadaadgilebisKarkhana},
	};
	
	return (
		from wb in yvelaFaili.Select (f => MakeExcelImporter().Import(f))
		from sheet in wb.Sheets
		select dokumentisKarkhnebisCnobari[DaadgineDokumentisTipi(wb.FilePath)](wb, sheet)
	);

}

PakingListi PakingListisKarkhana(WorkBook workBook, Sheet s)
{
	var yvelaUjra = s.Rows
		.SelectMany (row => row.Cells)
		.Select(cell => cell.AsString())
		.Where (cell => !string.IsNullOrWhiteSpace(cell));
	var valutebi = yvelaUjra
			.Select(cell => cell.Contains("EUR") ? "EUR" : cell.Contains("USD") ? "USD" : null )
			.Where (v => v != null)
			.Distinct()
			.ToList();
	if(valutebi.Count > 1) throw new InvalidOperationException("ver mokerkhda valutis dadgena");
	var valuta = valutebi.FirstOrDefault () ?? "GEL";
	var id = workBook.FilePath + "\\" + s.Name;
	return new PakingListi{
		Dro = TarigiFailisSakhelidan(workBook.FilePath),
		Id = Hash(id),
		Shenishvna = id,
		Valuta = valuta,
		Chanacerebi = (
			from rowx in s.Rows.Select ((r,i) =>  new {r,i})
			let c = rowx.r.Cells
			where c.Count > 5
			select new { Kodi = c[0].AsString(), Dasakheleba=c[1].AsString(), Ean=c[2].AsString(),Raodenoba=c[3].AsDouble(), Jami=c[5].AsDecimal(), Id=rowx.i}
			into c
			where !string.IsNullOrWhiteSpace(c.Kodi) && !string.IsNullOrWhiteSpace(c.Dasakheleba) && c.Raodenoba.HasValue && c.Jami.HasValue && c.Raodenoba.Value > 0
			let jami = Math.Round(c.Jami.Value, 2)
			select new PakingListi.Chanaceri(c.Kodi, c.Raodenoba.Value, c.Id.ToString(), null, c.Ean, c.Dasakheleba, jami / (decimal)c.Raodenoba.Value, jami, null)
		).ToList()
	};
}

Gakidva GakidvisKarkhana(WorkBook workBook, Sheet sheet)
{
	return new Gakidva{
		Dro = TarigiFailisSakhelidan(workBook.FilePath),
		Key = workBook.FilePath + "\\" + sheet.Name,
		Mdebareoba = workBook.FilePath.Split('\\')[3].ToLower(),
		Chanacerebi = (
			from rowx in sheet.Rows.Select ((r,i) =>  new {r,i})
			let c = rowx.r.Cells
			where c.Count > 6
			select new { Kodi = c[1].AsString(), Dasakheleba=c[2].AsString(), Raodenoba=c[4].AsDouble(), Jami=c[6].AsDecimal(), Id=rowx.i}
			into c
			where !string.IsNullOrWhiteSpace(c.Kodi) && c.Raodenoba.HasValue && c.Jami.HasValue
			select new Gakidva.Chanaceri{Ref=c.Kodi,Dasakheleba=c.Dasakheleba,Raodenoba=c.Raodenoba.Value,Jami=c.Jami.Value}
		).ToList()
	};
}

Gadaadgileba GadaadgilebisKarkhana(WorkBook workBook, Sheet sheet)
{
	var segments = workBook.FilePath.Split('\\');
	return new Gadaadgileba 
	{
		Dro = TarigiFailisSakhelidan(workBook.FilePath),
		Key = workBook.FilePath + "\\" + sheet.Name,
		Saidan = segments[3].ToLower(),
		Sad = segments[4].ToLower(),
		Chanacerebi = (
			from rowx in sheet.Rows.Select ((r,i) =>  new {r,i})
			let c = rowx.r.Cells
			where c.Count > 4
			select new { Kodi = c[1].AsString(), Dasakheleba=c[2].AsString(), Raodenoba=c[4].AsDouble(), Id=rowx.i}
			into c
			where !string.IsNullOrWhiteSpace(c.Kodi) && c.Raodenoba.HasValue
			select new Gadaadgileba.Chanaceri{Ref=c.Kodi,Dasakheleba=c.Dasakheleba,Raodenoba=c.Raodenoba.Value}
		).ToList()
	};
}

IEnumerable<Migeba> Migebebi(IEnumerable<PakingListi> pakingListebi)
{
	var  migebebiDic = (
	new Dictionary<string, Tuple<decimal, decimal>>()
	{
		{"pakinglists_20130523_EUR", Tuple.Create( 330.97m, 2.1153m) },
		{"pakinglists_20130523_USD", Tuple.Create(412.00m, 1.636m) },
		
		{"pakinglists2_20130728_EUR", Tuple.Create(4181.75m, 2.1948m)},
		{"pakinglists2_20130728_USD", Tuple.Create(1442.53m, 1.6546m)},
		
		{"pakinglists4_20131018_EUR", Tuple.Create(6844.20m, 2.2555m)},
		{"pakinglists4_20131018_USD", Tuple.Create(3377.43m, 1.6651m)},
		
		{"pakinglists5_20131122_EUR", Tuple.Create(449.00m, 2.2519m)},
		{"pakinglists5_20131122_USD", Tuple.Create(400.00m, 1.6758m)},
		
		{"pakinglists6_20131125_USD", Tuple.Create(1553.96m, 1.6821m)},
		{"pakinglists6_20131125_EUR", Tuple.Create(7481.68m, 2.2732m)},
		
		{"pakinglists7_20131204_EUR", Tuple.Create(5300m * 2.3055m + 400, 2.3055m)},
	}).Select (d => new {d.Key, d.Value});
	
	return (
		from pl in pakingListebi
	 	group pl by string.Format("{0}_{1}_{2}", pl.Shenishvna.Split('\\')[3], pl.Dro.ToString("yyyyMMdd"), pl.Valuta) into g
	 	from kv in migebebiDic.Where (d => d.Key == g.Key).DefaultIfEmpty()
	 	select new Migeba(g, kv != null ? kv.Value.Item1 : 0m, kv != null ? kv.Value.Item2 : 1m)
	);
}

string DaadgineDokumentisTipi(string failisMisamarti)
{	
	failisMisamarti = failisMisamarti.ToLowerInvariant();
	if(failisMisamarti.Contains("pakinglists")) return "PakingListi";
	if(failisMisamarti.Contains("\\gakidvebi\\")) return "Gakidva";
	if(failisMisamarti.Contains("\\modzraobebi\\")) return "Gadaadgileba";
	return null;
}

public string Hash(string temp)
{
    using(SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider()){
	byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(temp));
	string delimitedHexHash = BitConverter.ToString(hash);
	return delimitedHexHash.Replace("-", "");
	}
}

DateTime TarigiFailisSakhelidan(string failisMisamarti)
{
	var fileName = Path.GetFileNameWithoutExtension(failisMisamarti);
	return DateTime.Parse(fileName.Substring(fileName.Length - 10), new CultureInfo("ka-ge"));
}


ExcelImporter MakeExcelImporter()
{
	return new ExcelImporter(new XlsWorkBookReader(),new XlsxWorkBookReader());
}


public IEnumerable<string> DirWalk(string sDir, string searchPattern = "*.xls")
{
  if (File.Exists(sDir))
  {
      yield return sDir;
  }

  if (Directory.Exists(sDir))
  {
      foreach (var file in Directory.GetFiles(sDir, searchPattern))
      {
          yield return file;
      }
      foreach (var dir in Directory.GetDirectories(sDir))
      {
          foreach (var file in DirWalk(dir, searchPattern))
          {
              yield return file;
          }
      }
  }
}

public static class EnumerableEx
{
	public static void DumpToExcel<T>(this IEnumerable<T> src, string wbName, Func<T,string> bySheets=null)
	{
		var pck = new OfficeOpenXml.ExcelPackage();
		var sheets = bySheets!=null
			? src.GroupBy (x => bySheets(x)).Select (g => new {sheetName=g.Key, data=g.ToList()})
			: new[]{new {sheetName="data", data=src.ToList()}};
		foreach (var sheet in sheets)
		{
			var wsEnum = pck.Workbook.Worksheets.Add(sheet.sheetName);
			wsEnum.Cells["A1"].LoadFromCollection(sheet.data, true, OfficeOpenXml.Table.TableStyles.Medium9);
			wsEnum.Cells[wsEnum.Dimension.Address].AutoFitColumns();
		}	
		var fileName = @"d:\anvol\"+wbName+".xlsx";
		File.Delete(fileName);
		pck.SaveAs(new FileInfo(fileName));
		Process.Start(fileName);
	}
}

IEnumerable<T> FromCache<T>(string key)
{
	var fileName = key + ".json";
	using (var stream = File.Open(fileName, FileMode.Open))
	{
		return ServiceStack.Text.JsonSerializer.DeserializeFromStream<List<T>>(stream);
	}
}

IEnumerable<T> Cache<T>(Func<IEnumerable<T>> src, string key)
{
	var fileName = key + ".json";
	
	if (!File.Exists(fileName))
	{
		using (var stream = File.Open(fileName, FileMode.Create))
		{
			var list = new List<T>();
			list.AddRange(src());
			ServiceStack.Text.JsonSerializer.SerializeToStream(list, stream);
			stream.Flush();
			stream.Close();
		}
	}
	using (var stream = File.Open(fileName, FileMode.Open))
	{
		return ServiceStack.Text.JsonSerializer.DeserializeFromStream<List<T>>(stream);
	}
}

Func<string,string,string> DaamzadeMomeRefi()
{
	var refisCvlilebebi = StreamDocs("http://office.anvol.ge:8080/", "movlenebi", "Anvol").Where (x => x.Type == "MienichaRefi")
			.Select (x => new {x.Ref, x.AkhaliRefi,x.Dasakheleba,Dro=(DateTimeOffset)x["@metadata"]["Last-Modified"]})
			.Distinct()
			.OrderBy (x => x.Ref)
			.OrderBy (x => x.Dro)
			.GroupBy (x => new {x.Ref,x.Dasakheleba})
			.Select (g => new {g.Key.Ref,g.Key.Dasakheleba,AkhaliRefi=g.OrderByDescending (x => x.Dro).Select (x => x.AkhaliRefi).First ()})
			.ToList();		
	Dictionary<string,string> codebisKenvertori = new Dictionary<string,string>()
	{
		{"105957720026", "105957720"},
		{"780547", "9001890780547"},
		{"K5904-0", "K5904"},
		{"N4875-1", "N4895-1"},
		{"517344", "521365"},
	};
	
	Func<string,string,string> momeRefi = null;
	momeRefi = (kodi, dasakheleba) =>
	{
		kodi = codebisKenvertori.ContainsKey(kodi) ? codebisKenvertori[kodi] : kodi;
		var c = refisCvlilebebi.FirstOrDefault (x => x.Ref==kodi && x.Dasakheleba == dasakheleba);
		
		var re = (string)(c == null ? kodi : c.AkhaliRefi).ToUpperInvariant().Replace(" ","");
		if(codebisKenvertori.ContainsKey(re)){
			return momeRefi(codebisKenvertori[re],dasakheleba);
		}
		var rl = re.GetLong();
		return rl.HasValue ? rl.Value.ToString() : re;
	};
	return momeRefi;
}
public IEnumerable<dynamic> StreamDocs(string url, string doc, string database="Anvol"){
		using(var docStore = (new Raven.Client.Document.DocumentStore() {	Url = url,
													DefaultDatabase=database, 
													Conventions = { 
															FindTypeTagName = (t) => t.Name, 
															FindClrTypeName = t => t.Name 
														}
													}).Initialize())
		{
		var enmerator = docStore.DatabaseCommands.StreamDocs(null, doc, null);
			while(enmerator.MoveNext()){
				yield return new Raven.Abstractions.Linq.DynamicJsonObject(enmerator.Current);
			}
		}
}
public IEnumerable<dynamic> StreamQuery(string url, string index, string database="Anvol"){
		using(var docStore = (new Raven.Client.Document.DocumentStore() {	Url = url,
													DefaultDatabase=database, 
													Conventions = { 
															FindTypeTagName = (t) => t.Name, 
															FindClrTypeName = t => t.Name 
														}
													}).Initialize())
		{
		Raven.Abstractions.Data.QueryHeaderInformation qh;
		
		var enmerator = docStore.DatabaseCommands.StreamQuery(index,new Raven.Abstractions.Data.IndexQuery{}, out qh);
			while(enmerator.MoveNext()){
				yield return new Raven.Abstractions.Linq.DynamicJsonObject(enmerator.Current);
			}
		}
}
public static class Ex
{
	public static long? GetLong(this object o) 
	{
		if(o==null)
			return default(long?);
		long res;
		if(long.TryParse(o.ToString().Trim(), out res))
			return res;
		return null;
	}
}


static int lev(string s, string t)
    {
	int n = s.Length;
	int m = t.Length;
	int[,] d = new int[n + 1, m + 1];

	// Step 1
	if (n == 0)
	{
	    return m;
	}

	if (m == 0)
	{
	    return n;
	}

	// Step 2
	for (int i = 0; i <= n; d[i, 0] = i++)
	{
	}

	for (int j = 0; j <= m; d[0, j] = j++)
	{
	}

	// Step 3
	for (int i = 1; i <= n; i++)
	{
	    //Step 4
	    for (int j = 1; j <= m; j++)
	    {
		// Step 5
		int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

		// Step 6
		d[i, j] = Math.Min(
		    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
		    d[i - 1, j - 1] + cost);
	    }
	}
	// Step 7
	return d[n, m];
}