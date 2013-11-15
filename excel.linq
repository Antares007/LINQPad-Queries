<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\Accessibility.dll</Reference>
  <Reference>D:\Dev\EventStore\bin\eventstore\release\anycpu\EventStore.ClientAPI.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio 11.0\Visual Studio Tools for Office\PIA\Office14\Microsoft.Office.Interop.Excel.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Deployment.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Drawing.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Serialization.Formatters.Soap.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <NuGetReference>CsvHelper</NuGetReference>
  <NuGetReference>Ix-Main</NuGetReference>
  <NuGetReference>RavenDB.Client</NuGetReference>
  <NuGetReference>ServiceStack.Text</NuGetReference>
  <Namespace>CsvHelper</Namespace>
  <Namespace>CsvHelper.TypeConversion</Namespace>
  <Namespace>EventStore.ClientAPI</Namespace>
  <Namespace>Microsoft.Office.Interop.Excel</Namespace>
  <Namespace>Raven.Abstractions.Data</Namespace>
  <Namespace>Raven.Abstractions.Linq</Namespace>
  <Namespace>Raven.Client.Document</Namespace>
  <Namespace>Raven.Json.Linq</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

IEnumerable<PakingListi> PakingLists(string path=@"D:\Anvol\Invoicebi\PakingLists2")
{
	return (
	 from wb  in DirSearch(path).ToWorkBooks()
	 from ws  in wb.Worksheets<Worksheet>()
	 from r in ws.Rng("A1:W10000").ToRows()
	 select new {Wb = wb.Path + "\\" + wb.Name, SheetName = ws.Name, A=r[0].GetString(), B=r[1].GetString(), C=r[2].GetString(), D=r[3].GetDouble(), E=r[4].GetDecimal(), F=r[5].GetDecimal(), G=r[6], H=r[7], I=r[8], J=r[9], K=r[10], L=r[11], M=r[12], N=r[13], O=r[14], P=r[15], Q=r[16], R=r[17], S=r[18], T=r[19], U=r[20],V=r[21],W=r[22] }
	)
	.Where (x => x.D.HasValue && x.E.HasValue && x.F.HasValue)
	.GroupBy (x => x.Wb)
	.Select (g => new PakingListi{
			Shenishvna=g.Key,
			Chanacerebi = g.Select ((x,i) => new PakingListi.Chanaceri{
												Id = i.ToString(),
												Ref = x.A,
												Ean = x.C,
												Dasakheleba = x.B,
												Raodenoba = x.D.Value,
												ErtFasi = x.E.Value,
												Jami = x.F.Value,
												}).ToList()
		})
	.ToList();
}
IEnumerable<Tuple<string,string>> PakingListsCur(string path=@"D:\Anvol\Invoicebi\PakingLists2")
{
	var cols = (
	 from wb  in DirSearch(path).ToWorkBooks()
	 from ws  in wb.Worksheets<Worksheet>()
	 from r in ws.Rng("A1:W10000").ToRows()
	 from c in r.Select (x => x.GetString()).Where (x => !string.IsNullOrWhiteSpace(x))
	 select new {Wb = wb.Path + "\\" + wb.Name, col = c.ToUpper()}
	);
	return (
		from c in cols
		group c.col by c.Wb into g
		let cur = string.Format("{0}{1}", g.Any (x => x.Contains("(EUR)")) ? "EUR":"", g.Any (x => x.Contains("(USD)"))?"USD":"")
		select Tuple.Create(g.Key,cur.Length == 0?"GEL":cur)
	);
}
IEnumerable<PriceListisChanaceri> PriceLists(string path=@"D:\Anvol\Achikostvis sarealizacio fasebi")
{
	return 
	(
	from wb  in DirSearch(path)
	 				//.Where(x => x == @"D:\Anvol\Achikostvis sarealizacio fasebi\MGA PRICELIST 1-2013 GEORGIA USD.xls")
					.ToWorkBooks()
	let tags = new []{ wb.Name }
	from ws  in wb.Worksheets<Worksheet>()
	from r   in ws.Rng("A1:E10000").ToRows()
	
	select new {Tags = new List<string>(tags), Wb = wb.Path + "\\" + wb.Name, SheetName = ws.Name, Ref=r[1].GetString(),  EAN=r[2].GetString(), Desc=r[3].GetString(), Price=r[4].GetDecimal(), r}
	)
	.Where (x => x.Price.HasValue && x.Price.Value > 0m && !string.IsNullOrWhiteSpace(x.EAN))
	.Select ((x,i) => new PriceListisChanaceri{
		Id = i.ToString(),
		Ref = x.Ref,
		Ean = x.EAN,
		Dasakheleba = x.Desc,
		Fasi = x.Price.Value ,
		Shenishvna = x.Wb + " / " + x.SheetName
	}).ToList();
}
void DumpImages()
{
	Func<Picture,Image> getImage = p => {
				p.Width = p.Width * 2;
				p.Height = p.Height * 2;
				p.CopyPicture(Microsoft.Office.Interop.Excel.XlPictureAppearance.xlScreen, Microsoft.Office.Interop.Excel.XlCopyPictureFormat.xlBitmap);
				Image img = null;
				if (Clipboard.ContainsImage())
					img = Clipboard.GetImage();
				return img;
			};
	var pics = (
		from wb  in DirSearch(@"D:\Anvol\Achikostvis sarealizacio fasebi")
						//.Where(x => x == @"D:\Anvol\Achikostvis sarealizacio fasebi\BBURAGO PRICELIST 1-2013 GEORGIA USD.xls")
						.ToWorkBooks()
		let tags = new []{ wb.Name }
		from ws  in wb.Worksheets<Worksheet>()
		let pictures = (ws.Pictures() as Pictures)
		where null != pictures
		from p   in pictures.Cast<object>().Select(x => x as Picture).Where(x => null != x)
		where ws.Rng(p.TopLeftCell.Address +":"+ p.BottomRightCell.Address).Rows.Count == 1
		let picRow = p.TopLeftCell.Row
		let eancolAddress = string.Format("B{0}:B{0}", picRow)
		let eanText = ws.Rng(eancolAddress).Value.GetString()
		where !string.IsNullOrWhiteSpace(eanText)
		let eans = (eanText).Split(new[]{'/','*'}).Select (ean=>ean.Replace("'","").Trim()).ToArray()
		let img = getImage(p)
		from ean in eans
		select new {ean, img }
	)
	 .GroupBy (x => x.ean)
	 .SelectMany (g => g.Select ((x,i) => new {fileName = x.ean+"-" + i + ".jpg",x.img}))
	 .ToList().Dump();
	
	foreach (var pic in pics.Where (p => !Path.GetInvalidFileNameChars().Any (inv => p.fileName.Contains(inv.ToString()))))
	{
		pic.img.Save(@"D:\Anvol\Pics\" + pic.fileName, ImageFormat.Jpeg);
	}
	pics.Where (p => Path.GetInvalidFileNameChars().Any (inv => p.fileName.Contains(inv.ToString())))
		.Dump();
}

public void UpdateRates2(IEnumerable<PakingListi> pakings, IEnumerable<Tuple<string,string>> pakingsCurs){
	var rates = new Dictionary<string,double>(){
		{"GEL",1.00},
		{"USD",1.65},
		{"EUR",2.14}
	};
	foreach (var p in pakings)
	{
		var cur = pakingsCurs.First (c => c.Item1==p.Shenishvna).Item2;
		var rate = rates[cur];
		foreach (var c in p.Chanacerebi)
		{
			c.Jami= Math.Round(c.Jami * (decimal)rate, 2);
			c.ErtFasi = Math.Round(c.Jami / (decimal)c.Raodenoba, 3);
		}
	}
}

public void UpdateRates(IEnumerable<PakingListi> pakings, IEnumerable<Tuple<string,string>> pakingsCurs){
	var rates = new Dictionary<string,double>(){
		{"GEL",1.00},
		{"USD",1.65},
		{"EUR",2.14}
	};
	
	var ratesByPakingId = 
	(from pa in pakings
	from c in pa.Chanacerebi
	from pcur in pakingsCurs
	where pcur.Item1 == pa.Shenishvna
	select new {PId = pa.Shenishvna + "/" + c.Id, Rate = rates[pcur.Item2] }
	).ToDictionary (x => x.PId,x=>x.Rate);

//	StoreEx.Initialize(url :"http://localhost:8080/",(store1) => {
//		StoreEx.Initialize(url :"http://localhost:8080/",(store2) => {
//		
//			foreach(var id in store1.DatabaseCommands.ForDatabase("Anvol").GetIndexes(0, 100)){
//				store2.DatabaseCommands.PutIndex(id.Name, id, true);
//			}
//			
//			foreach(var doc in store1.DatabaseCommands.ForDatabase("Anvol").StartsWith("Raven",null,0,100)){
//				doc.Metadata.Remove("@id");
//				doc.Metadata.Remove("@etag");
//				store2.DatabaseCommands.Put(doc.Key,null,doc.DataAsJson,doc.Metadata);
//			}
//			
//		});
//	});
	
	StoreEx.Initialize((store1) => {
		store1.Session("Db2", (session1) => {
			var db2 = store1.DatabaseCommands.ForDatabase("Db2");
			var pakgns = session1.Load<PakingListi>(db2.StartsWith("Paking",null,0,100).Select (x => x.Key).ToArray()).ToList();
			
			StoreEx.Initialize((store2) => {
				var bio=store2.BulkInsert("Anvol", new Raven.Abstractions.Data.BulkInsertOptions()
					{
					CheckForUpdates=true
					});
				foreach (var p in pakgns.SelectMany (pa => pa.Chanacerebi.Select (c => new {c,Rate=ratesByPakingId[pa.Shenishvna+"/"+c.Id], pId=pa.Shenishvna+"/"+c.Id})))
				{
					p.c.Jami= Math.Round(p.c.Jami* (decimal)p.Rate,2);
					p.c.ErtFasi = Math.Round(p.c.Jami / (decimal)p.c.Raodenoba,3);
				}
				pakgns.ForEach(x=>bio.Store(x));
			},url :"http://localhost:8080/");
		});
	},url :"http://localhost:8080/");
	
}
public void DaamateAkhaliPakingListebi(IEnumerable<PakingListi> pakings, IEnumerable<Tuple<string,string>> pakingsCurs){

	UpdateRates2(pakings.Select (p => {
				foreach(var c in p.Chanacerebi)
					c.Ean = string.Join(", ", toEans(c.Ean));
				return p;
			}), pakingsCurs);
	pakings.Dump();
	
	using(var docStore = (new DocumentStore() {	Url = "http://25.88.224.201:8080/",
												DefaultDatabase="Anvol", 
												Conventions = { 
														FindTypeTagName = (t) => t.Name, 
														FindClrTypeName = t => t.Name 
													}
												}).Initialize())
	using(var session = docStore.OpenSession())
	{
	
		var bio = docStore.BulkInsert("Anvol", new Raven.Abstractions.Data.BulkInsertOptions());
		pakings.ForEach(bio.Store);
		
	
	}
}

public void PutInEventStore(IEnumerable<PakingListi> pakings){
	using(var conn = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback,1113)))
	{
		conn.Connect();
		var events = pakings.Select (p => { p.Id = Path.GetFileName(p.Shenishvna); return p; })
							.Select (p => ServiceStack.Text.JsonSerializer.SerializeToString(p))
							.Select (s => Encoding.UTF8.GetBytes(s))
							.Select (b => new EventData(Guid.NewGuid(), "PO", true, b, null));
		
		conn.AppendToStream(Uri.EscapeUriString("Jurnali-#$% _,+="), ExpectedVersion.Any, events);
	}
}
void Main()
{
IEnumerable<PakingListi> pakings = Util
	.Cache(()=>PakingLists(@"D:\Anvol\Invoicebi\PakingLists").Concat(PakingLists(@"D:\Anvol\Invoicebi\PakingLists2")).ToList(), "01");

//IEnumerable<Tuple<string,string>> pakingsCurs = Util
//	.Cache(()=>PakingListsCur(@"D:\Anvol\Invoicebi\PakingLists").Concat(PakingListsCur(@"D:\Anvol\Invoicebi\PakingLists2")).ToList(), "03");
PutInEventStore(pakings);

return;


IEnumerable<PriceListisChanaceri> prices = Util
	.Cache(()=>PriceLists(@"D:\Anvol\Fasebi2").ToList(), "02");
	prices.Dump();
StoreEx.Initialize(ds => {
	var meta=new RavenJObject();
	meta.Add("Raven-Entity-Name",RavenJToken.FromObject("Movlenebi"));
	
	foreach (var p in prices)
	{
		var fasi=RavenJObject.FromObject(new {
			Type="MienichaFasi",
			Ref=p.Ref,
			Tankha=(Math.Round(p.Fasi / 0.05m+0.05m) * 0.05m).ToString()
		});
		ds.DatabaseCommands.Put("movlenebi/", null, fasi, meta);
	}
},"http://25.88.224.201:8080/", "Anvol");
return;
//suratebis gareshe
//var photosWithRefs = (from f in DirSearch(@"D:\Anvol\Pics", "*.jpg").Select (x => new FileInfo(x).Name)
// let parts = f.Split(new []{'-'})
// select new {f,Ref=string.Join("-", parts.Take(parts.Length-1))}
//).ToList();
//
//(
//from r in pakings.SelectMany (p => p.Chanacerebi.Select (c => new {c.Ref,c.Ean,p.Shenishvna})).Distinct()
//from f in photosWithRefs.Where (wr => wr.Ref==r.Ref).DefaultIfEmpty()
//where f == null			
//select r
//).Dump(); 

//pakings.SelectMany (p => p.Chanacerebi.Select (c => new {c.Dasakheleba,p.Shenishvna}))
//	.Where (x => x.Dasakheleba.Split(new []{' '}).Length>1)
//	.Select (x => new { x.Shenishvna, Brendi=x.Dasakheleba.Split(new []{' '})[0]})
//	.Distinct()
//	.Dump();

//pakings.Where (p => p.Chanacerebi.Any (c => c.Ref=="005207")).Dump();
//prices.GroupBy (p => p.Ref)
//.Where (g => g.Select (x => x.Fasi).Distinct().Count ()>1)
//.Dump();
//return;
//	return;
//	DumpImages();
	

	
//	
//	(
//		from p in pakings
//		from c in p.Chanacerebi
//		from f in prices.Where (pr => pr.Ref == c.Ref).DefaultIfEmpty()
//		where f == null
//		select new {c.Ref,c.Ean, c.Dasakheleba, c.ErtFasi, c.Raodenoba, p.Shenishvna}
//	)
//	.GroupBy (x => x.Ref)
//	.Where (g => g.Count ()>1)
//	.Where (g => g.Select(x => x.Ean).Distinct().Count ()>1)
//	.Dump();	
	

//	(from p in prices
//	 from e in toEans(p.Ean)
//	 select new {p.Shenishvna,e,IsValid = IsValidGtin(e)}
//	).Where (x => !x.IsValid).Dump();
//	return;
//	var q0=(
//		from pk in pakings
//		from c in pk.Chanacerebi
//		from ean in toEans(c.Ean)
//		group new {c,pk.Shenishvna }by ean into g
//		where g.Select (x => x.c.Ref).Distinct().Count ()>1
//		select g.Select (x => new {x.Shenishvna,Ref="'"+x.c.Ref,Ean="'"+x.c.Ean,x.c.Dasakheleba})
//	);
 
//var pasebi = (
//			from ean in pakings .SelectMany (x => x.Chanacerebi)
//								.SelectMany (x => toEans(x.Ean)
//													.Select (e => new {x.Ref, Ean = e})
//											)
//			from p in prices.SelectMany (x => toEans(x.Ean).Select (e => new {x.Fasi, Ean=e}))
//			where p.Ean.GetLong() == ean.Ean.GetLong()
//			group p.Fasi by ean.Ref into g
//			select new { g.Key, Fasi = g.Max () }
//		).Concat(
//			from ean in pakings .SelectMany (x => x.Chanacerebi)
//			from p in prices
//			where p.Ref.Trim() == ean.Ref.Trim()
//			group p.Fasi by ean.Ref into g
//			select new { g.Key, Fasi = g.Max () }
//		)
//		.GroupBy (x => x.Key)
//		.Select (g => new { Ref=g.Key, Tankha = g.Max (x=>x.Fasi) })
//		.Select (x => new Fasi{ Ref = x.Ref, Tankha = (Math.Round(x.Tankha / 0.05m+0.05m) * 0.05m).ToString() })
//		.ToList();
//	using(var docStore = (new DocumentStore() {	Url = "http://localhost:8080/",
//												DefaultDatabase="Anvol", 
//												Conventions = { 
//														FindTypeTagName = (t) => t.Name, 
//														FindClrTypeName = t => t.Name 
//													}
//												}).Initialize())
//	using(var session = docStore.OpenSession())
//	{
//
//		var bio = docStore.BulkInsert("Anvol", new Raven.Abstractions.Data.BulkInsertOptions());
////		pakings.Select (p => {
////			foreach(var c in p.Chanacerebi)
////				c.Ean = string.Join(", ", toEans(c.Ean));
////			return p;
////		}).ForEach(bio .Store);
//		pasebi.ForEach(bio.Store);
//
//	}
return;
	
}

public static class Ex
{
	public static IEnumerable<Workbook> ToWorkBooks(this IEnumerable<string> files)
	{
		var a = new Microsoft.Office.Interop.Excel.Application();
		foreach(var fileName in files.ToArray())
		{	
			var wb = a.Workbooks.Open(fileName);
			yield return wb;
			wb.Close(false);
		}
		a.Quit();
	}	
	
	public static IEnumerable<T> Worksheets<T>(this Workbook wb)
	{
		 return Enumerable.Range(1,wb.Worksheets.Count).Select (i => (T)wb.Worksheets[i]);
	}

	public static Range Rng(this Worksheet ws, object rowIndex, object colIndex = null)
	{
		var range = (Range) (colIndex != null ? ws.Range[rowIndex, colIndex] : ws.Range[rowIndex]);
		range.UnMerge();
		return range;
	}

	public static IEnumerable<IList<object>> ToRows(this Range range)
	{
		return ((IEnumerable)range.Value).Cast<object>().Buffer(range.Columns.Count).Where (b => b.Any (x => x != null));
	}
	
	public static string GetString(this object o) 
	{
		if(o == null)
			return null;
		return o.ToString();
	}
	
	public static decimal? GetDecimal(this object o) 
	{
		if(o==null)
			return default(decimal?);
		decimal res;
		if(decimal.TryParse(o.ToString().Trim(), out res))
			return res;
		return null;
	}
	
	public static double? GetDouble(this object o) 
	{
		if(o==null)
			return default(double?);
		double res;
		if(double.TryParse(o.ToString().Trim(), out res))
			return res;
		return null;
	}
	
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

IEnumerable<string> DirSearch(string sDir, string searchPattern="*.xls") 
{
   var dirs = Directory.GetDirectories(sDir).AsEnumerable();
   if(Directory.Exists(sDir))
	   dirs = new []{sDir}.Concat(dirs);
   foreach (string d in dirs) 
   {
		foreach (string f in Directory.GetFiles(d, searchPattern)) 
		{
			yield return f;
		}
		DirSearch(d);
   }
}

public static bool IsValidGtin(string code)
{
    if (code != (new Regex("[^0-9]")).Replace(code, ""))
    {
        // is not numeric
        return false;
    }
    // pad with zeros to lengthen to 14 digits
    switch (code.Length)
    {
        case 8:
            code = "000000" + code;
            break;
        case 12:
            code = "00" + code;
            break;
        case 13:
            code = "0" + code;
            break;
        case 14:
            break;
        default:
            // wrong number of digits
            return false;
    }
    // calculate check digit
    int[] a = new int[13];
    a[0] = int.Parse(code[0].ToString()) * 3;
    a[1] = int.Parse(code[1].ToString());
    a[2] = int.Parse(code[2].ToString()) * 3;
    a[3] = int.Parse(code[3].ToString());
    a[4] = int.Parse(code[4].ToString()) * 3;
    a[5] = int.Parse(code[5].ToString());
    a[6] = int.Parse(code[6].ToString()) * 3;
    a[7] = int.Parse(code[7].ToString());
    a[8] = int.Parse(code[8].ToString()) * 3;
    a[9] = int.Parse(code[9].ToString());
    a[10] = int.Parse(code[10].ToString()) * 3;
    a[11] = int.Parse(code[11].ToString());
    a[12] = int.Parse(code[12].ToString()) * 3;
    int sum = a[0] + a[1] + a[2] + a[3] + a[4] + a[5] + a[6] + a[7] + a[8] + a[9] + a[10] + a[11] + a[12];
    int check = (10 - (sum % 10)) % 10;
    // evaluate check digit
    int last = int.Parse(code[13].ToString());
    return check == last;
}




[Serializable] 
public class Fasi
{
	public string Id { get; set; }
	public string Ref { get; set; }
	public string Tankha { get; set; }
}

[Serializable]
public class PakingListi
{
	public string Id { get; set; }
	public IEnumerable<Chanaceri> Chanacerebi {get; set;}
	public string Shenishvna {get; set;}
	[Serializable]
	public class Chanaceri
	{
		public string Id { get; set; }
		public string Ref { get; set;}
		public string Ref2 { get; set; }
		public string Ean {get;set;}
		public string Dasakheleba { get; set; }
		public double Raodenoba {get;set;}
		public decimal ErtFasi {get; set;}
		public decimal Jami { get; set; }
	}
}
[Serializable]
public class PriceListisChanaceri
{
	public string Id { get; set; }
	public string Ref { get; set; }
	public string Ean { get; set; }
	public string Dasakheleba { get; set; }
	public decimal Fasi { get; set; }
	public string Shenishvna { get; set; }
}
public class SkusMinicheba
{
	public string Id { get; set; }
	public string Partia { get; set; }
	public string Sku { get; set; }
}


public static class StoreEx{
	public static void Session(this Raven.Client.IDocumentStore store,string dbName, Action<Raven.Client.IDocumentSession> action){
		using(var session = store.OpenSession(dbName)){
			action(session);
		}
	}
	public static void Initialize(Action<Raven.Client.IDocumentStore> action,
				string url="http://localhost:8080/",
				string defaultDatabase="Anvol")
	{
		using(var docStore = (new DocumentStore() {	Url = url,
													DefaultDatabase=defaultDatabase, 
													Conventions = { 
															FindTypeTagName = (t) => t.Name, 
															FindClrTypeName = t => t.Name 
														}
													}).Initialize())
		{
			action(docStore);
		}
	}
}

static Func<IEnumerable<string>,IEnumerable<string>,bool> akvsSaertoEani = (l,r) =>
	l.Select (x => x.GetLong())
	 .Intersect(r.Select (x => x.GetLong())).Any();

static Func<string,IEnumerable<string>> toEans = s=> (s??string.Empty).Replace("  ","/")
				.Split  ( new[] { '/', ',' } )
				.Select ( ean => new string( ean.Where(char.IsNumber).ToArray() ) )
				.Where  ( ean => ean.GetLong().HasValue )
				.ToArray();