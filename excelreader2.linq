<Query Kind="Program">
  <Reference>D:\Dev\Nashti.Core\Nashti.Core\bin\Debug\Nashti.Core.dll</Reference>
  <NuGetReference>EPPlus</NuGetReference>
  <NuGetReference>ExcelLibrary</NuGetReference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>Nashti.Core</Namespace>
  <Namespace>Nashti.Excel</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
  <Namespace>System.Reactive.Disposables</Namespace>
  <Namespace>System.Reactive.Joins</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Reactive.PlatformServices</Namespace>
  <Namespace>System.Reactive.Subjects</Namespace>
  <Namespace>System.Reactive.Threading.Tasks</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

public class Modzraoba
{
	public Modzraoba(string id, DateTime rodis, IEnumerable<Chanaceri> chanacerebi)
	{
		Id = id;
		Rodis = rodis;
		Chanacerebi = chanacerebi.ToList().AsReadOnly();
	}
	public string Id { get; private set; }
	public DateTime Rodis { get; private set; }
	
	public IList<Chanaceri> Chanacerebi { get; private set; }	
	
	public class Chanaceri
	{
		public Chanaceri(string saidan, string sad, string ra, double raodenoba, decimal tankha, IEnumerable<Partia> partiebiFiFo = null)
		{
			Saidan = saidan;
			Sad = sad;
			Ra = ra;
			Raodenoba = raodenoba;
			Tankha = tankha;
			PartiebiFiFo =(partiebiFiFo ?? new Partia[]{}).ToList().AsReadOnly();
		}		
		public string Saidan { get; private set; }
		public string Sad { get; private set; }
		public string Ra { get; private set; }
		public double Raodenoba { get; private set; }
		public decimal Tankha {get; private set;}
		public IList<Partia> PartiebiFiFo { get; private set; }
	}
	
	public class Partia 
	{
		public Partia(string id, double raodenoba, decimal tankha)
		{
			Id = id;
			Raodenoba = raodenoba;
			Tankha = tankha;
		}
		public string Id { get; private set; }
		public double Raodenoba { get; private set; }
		public decimal Tankha { get; private set; }
	}
}

class Commit
{
	public List<Dokumenti> Damatebebi {get; set;}
	public List<Tuple<Dokumenti, Dokumenti>> Cvlilebebi {get; set;}
	public List<Dokumenti> Cashlilebi {get; set;}
	
}

interface IEvent
{
	
}
class Sheavse : IEvent{}
class Gadaitane : IEvent{}
void Main()
{
	IEnumerable<Dokumenti> dokumentebi = Cache(()=> ExcelisFailebi().Where (x => x.GetType() != typeof(PakingListi))
								.Concat(Migebebi(ExcelisFailebi().OfType<PakingListi>()))
								.Concat(FromCache<Cheki>("ch03"))
								.OrderBy (i => i, Dokumenti.DokumentiComparer), "alldocs02");
	
}




IEnumerable<Dokumenti> ExcelisFailebi()
{
	var yvelaFaili = DirSearch(@"D:\Anvol\modzraobebi")
			 .Concat(DirSearch(@"D:\Anvol\Invoicebi\PakingLists"))
			 .Concat(DirSearch(@"D:\Anvol\Invoicebi\PakingLists2"))
			 .Concat(DirSearch(@"D:\Anvol\Invoicebi\PakingLists3"))
			 .Concat(DirSearch(@"D:\Anvol\Invoicebi\PakingLists4"))
			 .Concat(DirSearch(@"D:\Anvol\Invoicebi\PakingLists5"))
			 .Concat(DirSearch(@"D:\Anvol\Invoicebi\PakingLists6"))
			 .Concat(DirSearch(@"D:\Anvol\Invoicebi\PakingLists7"))
			 .Concat(DirSearch(@"D:\Anvol\Invoicebi\PakingLists8"))
			 .Select (x => x.ToLowerInvariant());
		
	var dokumentisKarkhnebisCnobari = new Dictionary<string, Func<WorkBook, Sheet, Dokumenti>>()
	{
		{"PakingListi",	PakingListisKarkhana},
		{"Gakidva",	GakidvisKarkhana},
		{"Gadaadgileba", GadaadgilebisKarkhana},
	};
	
	return (
		from wb in yvelaFaili.Select (f => MakeExcelImporter().GetWb(f))
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
	return new Gadaadgileba{
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

IEnumerable<T> ToEnumerable<T>(IEnumerator<T> enumerator)
{
	while (enumerator.MoveNext())
	{
		yield return enumerator.Current;
	}
}

ExcelImporter MakeExcelImporter()
{
	return new ExcelImporter(
					new WorkBookReader("xlsx", (filePath) => 
					() =>	{
						using(var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
						using(var ep = new OfficeOpenXml.ExcelPackage(stream))
						{
							return ep.Workbook.Worksheets.Select (sheet => 
								Tuple.Create<string,Func<IEnumerable<Tuple<int,int,object>>>>(sheet.Name, () => 
									sheet.Cells["a:z"].Select (c => Tuple.Create(c.Start.Row,c.Start.Column,c.Value))
								)
							);
						}
					}),
					new WorkBookReader("xls", (filePath) =>
					() => {
						using(var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
						{
							return ExcelLibrary.SpreadSheet.Workbook.Load(stream).Worksheets.Select (sheet => 
								Tuple.Create<string,Func<IEnumerable<Tuple<int,int,object>>>>(sheet.Name, ()=> 
									ToEnumerable(sheet.Cells.GetEnumerator()).Select (c => 
										{
											var cell = c.Right;
											var value = default(object);
											if(	   cell.Format.FormatType == ExcelLibrary.SpreadSheet.CellFormatType.Date 
												|| cell.Format.FormatType == ExcelLibrary.SpreadSheet.CellFormatType.DateTime 
												|| cell.Format.FormatType == ExcelLibrary.SpreadSheet.CellFormatType.Time
												|| (cell.Format.FormatType == ExcelLibrary.SpreadSheet.CellFormatType.Custom && (cell.FormatString.Contains("yy") || cell.FormatString.Contains("mm")))
												)
											{
												value = cell.DateTimeValue;
											}
											else
											{
												value = cell.Value;
											}
											return	Tuple.Create(c.Left.Left+1,c.Left.Right+1,value);
										}
									)
								)
							);		
						}
					})
					);
}

public IEnumerable<string> DirSearch(string sDir, string searchPattern = "*.xls")
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
          foreach (var file in DirSearch(dir, searchPattern))
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
	var fileName = key + ".bin";
	var bin = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
	using (var stream = File.Open(fileName, FileMode.Open))
	{
		return (List<T>)bin.Deserialize(stream);
	}
}
IEnumerable<T> Cache<T>(Func<IEnumerable<T>> src,string key)
{
	var fileName = key + ".bin";
	var bin = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
	if (!File.Exists(fileName))
	{
		using (var stream = File.Open(fileName, FileMode.Create))
		{
			var list = new List<T>();
			list.AddRange(src());
			bin.Serialize(stream, list);
		}
	}
	using (var stream = File.Open(fileName, FileMode.Open))
	{
		return (List<T>)bin.Deserialize(stream);
	}
}