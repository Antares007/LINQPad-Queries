<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\Accessibility.dll</Reference>
  <Reference>D:\anycpu\EventStore.ClientAPI.dll</Reference>
  <Reference>D:\RavenDB-Build-2700\Client\Microsoft.CompilerServices.AsyncTargetingPack.Net4.dll</Reference>
  <Reference>C:\Windows\assembly\GAC_MSIL\Microsoft.Office.Interop.Excel\14.0.0.0__71e9bce111e9429c\Microsoft.Office.Interop.Excel.dll</Reference>
  <Reference>D:\Dev\Nashti.Core\Nashti.Core\bin\Debug\Nashti.Core.dll</Reference>
  <Reference>D:\RavenDB-Build-2700\Client\Raven.Abstractions.dll</Reference>
  <Reference>D:\RavenDB-Build-2700\Client\Raven.Client.Lightweight.dll</Reference>
  <NuGetReference>CsvHelper</NuGetReference>
  <NuGetReference>EPPlus</NuGetReference>
  <NuGetReference>Ix-Main</NuGetReference>
  <NuGetReference>ServiceStack.Razor</NuGetReference>
  <NuGetReference>ServiceStack.Text</NuGetReference>
  <Namespace>CsvHelper</Namespace>
  <Namespace>CsvHelper.TypeConversion</Namespace>
  <Namespace>Microsoft.Office.Interop.Excel</Namespace>
  <Namespace>Nashti.Core</Namespace>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.ConditionalFormatting</Namespace>
  <Namespace>OfficeOpenXml.ConditionalFormatting.Contracts</Namespace>
  <Namespace>OfficeOpenXml.DataValidation</Namespace>
  <Namespace>OfficeOpenXml.DataValidation.Contracts</Namespace>
  <Namespace>OfficeOpenXml.DataValidation.Formulas.Contracts</Namespace>
  <Namespace>OfficeOpenXml.Drawing</Namespace>
  <Namespace>OfficeOpenXml.Drawing.Chart</Namespace>
  <Namespace>OfficeOpenXml.Drawing.Vml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>OfficeOpenXml.Style.Dxf</Namespace>
  <Namespace>OfficeOpenXml.Style.XmlAccess</Namespace>
  <Namespace>OfficeOpenXml.Table</Namespace>
  <Namespace>OfficeOpenXml.Table.PivotTable</Namespace>
  <Namespace>OfficeOpenXml.Utils</Namespace>
  <Namespace>OfficeOpenXml.VBA</Namespace>
  <Namespace>Raven.Abstractions.Data</Namespace>
  <Namespace>Raven.Abstractions.Linq</Namespace>
  <Namespace>Raven.Client.Connection</Namespace>
  <Namespace>Raven.Client.Document</Namespace>
  <Namespace>Raven.Json.Linq</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Runtime.Serialization.Formatters.Binary</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

DateTime TarigiFailisSakhelidan(string fn)
{
	var fileName = Path.GetFileNameWithoutExtension( Path.GetDirectoryName(fn) );
	return DateTime.Parse(fileName.Substring(fileName.Length-10), new CultureInfo("ka-ge"));
}

public string Hash(string temp)
{
    using(SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider()){
	byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(temp));
	string delimitedHexHash = BitConverter.ToString(hash);
	return delimitedHexHash.Replace("-", "");
	}
}

IEnumerable<Tuple<string,List<string>>> Gadaadgileba(string path)
{
	return (
		from wb in DirSearch(path).ToWorkBooks()
		from ws in wb.Worksheets<Worksheet>()
		from r  in ws.Rng("A1:W10000").ToRows()
		select new {Wb = wb.Path + "\\" + wb.Name+"\\"+ws.Name, Cols = r.Select (x => x.GetString()).ToList() }
	)
	.Where (x => x.Cols.Where (c => !string.IsNullOrWhiteSpace(c.GetString())).Count () > 1)
	.Select (x => Tuple.Create(x.Wb, x.Cols));
}

public IEnumerable<dynamic> StreamDocs(string url, string doc, string database="Anvol"){
		using(var docStore = (new DocumentStore() {	Url = url,
													DefaultDatabase=database, 
													Conventions = { 
															FindTypeTagName = (t) => t.Name, 
															FindClrTypeName = t => t.Name 
														}
													}).Initialize())
		{
		var enmerator = docStore.DatabaseCommands.StreamDocs(null, doc, null);
			while(enmerator.MoveNext()){
				yield return new DynamicJsonObject(enmerator.Current);
			}
		}
}
public IEnumerable<dynamic> StreamQuery(string url, string index, string database="Anvol"){
		using(var docStore = (new DocumentStore() {	Url = url,
													DefaultDatabase=database, 
													Conventions = { 
															FindTypeTagName = (t) => t.Name, 
															FindClrTypeName = t => t.Name 
														}
													}).Initialize())
		{
		QueryHeaderInformation qh;
		
		var enmerator = docStore.DatabaseCommands.StreamQuery(index,new IndexQuery{}, out qh);
			while(enmerator.MoveNext()){
				yield return new DynamicJsonObject(enmerator.Current);
			}
		}
}

static string[] amosagebiStringebi = new []{"."," ","(",")","-","#",","};
static string NormalizeString (string str)
{
	return amosagebiStringebi.Aggregate (str,(memo,x)=>memo.Replace(x,"")).ToLowerInvariant();
}

IEnumerable<Refi> Refebi()
{
	return StreamQuery("http://25.88.224.201:8080/", "Refebi")
				 .Select(d => new Refi(d.Ref, ((IEnumerable<dynamic>)d.Eans).Cast<string>().ToList(), d.Dasakheleba, decimal.Parse(d.Fasi.ToString())))
				 .ToList();
}

class Projection
{
	public Dictionary<Type,int> dic = new Dictionary<Type,int>();

	public void Handle(Migeba d)
	{ 
		foreach (var c in d.Chanacerebi)
		{
			
		}
		Add(d.GetType());
	}
	
	public void Handle(Gadaadgileba d)
	{ 
		Add(d.GetType());
	}
	
	public void Handle(Cheki d)
	{ 
		Add(d.GetType());
	}
	
	public void Handle(Gakidva d)
	{ 
		Add(d.GetType());
	}
	
	void Add(Type t){
		int sum = 0;
		dic.TryGetValue(t, out sum);
		sum = sum + 1;
		dic[t] = sum;
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

void Main()
{

//Migebebi(Chache(() => MomePakingListebi((r,s)=>r),"pakingListebi0").OfType<PakingListi>())
//		.Select (m =>new {Jami=m.Chanacerebi.Sum (c => c.Jami), m.Dro,m._kursi})
//		.OrderBy (m => m.Jami)
//		.Dump();
//		return;
//	Chache(() => MomePakingListebi((r,s)=>r),"pakingListebi").Count ().Dump();
//	Chache(() => MomeGadaadgilebebi((r,s)=>r),"gadaadgilebebi2").Count ().Dump();
//	Chache(() => MomeGakidvebi((r,s)=>r),"gakidvebi2").Count ().Dump();
//	Chache(() => MomeChekebi((r,s)=>r),"chekebi3").Count ().Dump();
//return;


Func<string,string,string> momeRefi = DaamzadeMomeRefi();



//LKIENTEBIS SIA
//StreamQuery("http://office.anvol.ge:8080/","Klientebi2", "Anketebi")
//	.ToList()
////	.Where(x => ((IEnumerable<dynamic>)x.bavshvebisDabTarigebi).Any(y=>y.GetType()==typeof(string))).Dump()
//	.Select (x => new{
//		  baratisNomeri=x.baratisNomeri
//		, pid=x.pid
//		, x.sakheli
//		, x.gvari
//		, dabTarigi=x.dabTarigi.ToString("yyyy-MM-dd")
//		, x.mimdinareProcenti
//		, x.vizitisRaodenoba
//		, bavshvebisDabTarigebi= string.Join(",", Enumerable.Select((IEnumerable<dynamic>)x.bavshvebisDabTarigebi, e => e.ToString("yyyy-MM-dd")) )
//		, mobiluri=Enumerable.Select((IEnumerable<dynamic>)x._anketebi, x_ => x_.mobiluri).FirstOrDefault ()
//	})
//	.DumpToExcel("klientebi_"+DateTime.Now.ToString("yyyy.MM.dd"))
//;
//return;


var refebi = Cache(() => Refebi(), "ref01");
//refebi.Select (r => new{ Ref = "'" + r.Id, r.Dasakheleba, r.Fasi })
//	.Dump();
//return;

//30/05
//31/05
//07/06

var shekveta = Cache(() => MomePakingListebi(momeRefi).ToList(), "pl04");
//shekveta
//	.Select (s => new {s.Shenishvna,s.Dro,Jami=s.Chanacerebi.Sum (c => c.Jami)})
//	.OrderBy (x => x.Dro)
//	.Dump();
//return;

var migeba = Cache(() => Migebebi(shekveta), "mig01");

var chekebi = Cache(() => MomeChekebi(momeRefi).Where (x => !gasaukmebeliChekebi.Contains(x.Nomeri)).ToList(), "ch04");

var gadaadgilebebi = Cache(() => 	(
										from l in gamorcheniliGadaadgilebebi.Split('\n').Where (x => x.Length>0)
										let segs = l.Split(' ').Where (x => x.Length>0).ToArray()
										select new {f="sackobi",t=segs[0],Kodi=segs[1], Raodenoba=(long?)long.Parse(segs[2]),Dro=DateTime.Parse(segs[3], new CultureInfo("ka-ge")),Dasakheleba=default(string),Jami=default(decimal?)}
										into r2
										where r2.Raodenoba.HasValue && r2.Kodi != null
										group r2 by r2.Dro into g2
										select new AnonymGadaadgileba{
													Key=g2.Key.ToString(),
													Saidan=g2.First ().f,
													Sad=g2.First ().t,
													Dro=g2.Key,
													Chanacerebi=g2.Select (x => new AnonymGadaadgileba.Chanaceri{Ref = momeRefi(x.Kodi, x.Dasakheleba), Dasakheleba=x.Dasakheleba, Raodenoba=x.Raodenoba.Value, Saidan=x.f, Sad=x.t,Jami=x.Jami})
																.Where (x => !gadaadgilebidanAmosagebi.Contains(x.Ref))
																.ToList()
													}
										
								).Concat(
										from wb  in DirSearch(@"D:\Anvol\modzraobebi\").ToWorkBooks()
										let pathSegments = wb.Path.Split('\\')
										from ws  in wb.Worksheets<Worksheet>()
										from r in ws.Rng("A1:W10000").ToRows()
										select new { Wb = wb.Path + "\\" + wb.Name + "\\" + ws.Name, f=pathSegments[3],t=pathSegments[4], Id=r[0].GetLong(), Kodi=r[1].GetString(), Dasakheleba=r[2].GetString(), Raodenoba=r[4].GetLong(),ErtFasi=r[5].GetDecimal(),Jami=r[6].GetDecimal()} 
										into r2
										where r2.Raodenoba.HasValue && r2.Kodi != null
										group r2 by r2.Wb into g2
										select new AnonymGadaadgileba{
													Key=g2.Key,
													Saidan=g2.First ().f,
													Sad=g2.First ().t,
													Dro=TarigiFailisSakhelidan(g2.Key),
													Chanacerebi = g2.Select (x => new AnonymGadaadgileba.Chanaceri{Ref = momeRefi(x.Kodi, x.Dasakheleba), Dasakheleba=x.Dasakheleba, Raodenoba=x.Raodenoba.Value, Saidan=x.f, Sad=x.t,Jami=x.Jami})
																.Where (x => !gadaadgilebidanAmosagebi.Contains(x.Ref) || g2.Key.ToLower().Contains("gakidvebi"))
																.ToList()
													}
								
								).ToList(), "gad06");

return;
//klientebis analizi
//chekebi.Select (c => new {
//	c.Mdebareoba,
//	c.Dro,
//	c.klientisBaratisNomeri,
//	c.Gadasakhdeli,
//	c.FasdaklebuliGadasakhdeli
//}).Dump();
//return;
		


//migeba.Select (m => new {m._kursi,m.Dro.Date,Tankha=m.Chanacerebi.Sum (c => c.Jami)})
//	.OrderBy (m => m.Date)
//	.Dump();
//return;
//migeba.Select (m => new {m.Dro,m._kursi,m._kharji,Jami=m.Chanacerebi.Sum (x => x.Jami)}).OrderBy (x => x.Dro).Dump();

////problemuri refebi
//var migebuliRefebi = migeba.SelectMany (m => m.Chanacerebi.Select (c => c.Ref)).Distinct().ToList();
//
//var prefebi = gadaadgilebebi
////	.Where (x => !x.Sad.ToLower().StartsWith("gak"))
//	.SelectMany( x=>x.Chanacerebi.Select (c => c.Ref))
//	.Concat(chekebi.SelectMany (c => c.Chanacerebi.Select(x=>x.Ref)))
//	.Distinct()
//	.Where (x => !migebuliRefebi.Contains(x))
//	.Select (x => new {
//		x, 
//		Chekebi = chekebi.Where (c => c.Chanacerebi.Any (ch => ch.Ref == x)),
//		Gadaadgilebebi = gadaadgilebebi.Where (c => c.Chanacerebi.Any (ch => ch.Ref == x)),
//		Msgavsebi = migebuliRefebi.Select (r => new { lev = lev(r,x), r}).Where (x_ => x_.lev<4).OrderBy(r => r.lev)
//		})
//	.Select (x => new {x.x})
//	.ToList()
//	.Dump()
//	;
//return;


//var bananasKodebi = shekveta.Where (s => s.Shenishvna == @"D:\Anvol\Invoicebi\PakingLists\banana-unimart zeddebuli 26.02.2013.xls\Grid")
//	.SelectMany (x => x.Chanacerebi.Select (c=>c.Ref))
//	.ToList()
//	;
//chekebi.SelectMany (c => c.Chanacerebi.Select (ch => new {Ref=(string)ch.Ref,Raodenoba=(double)ch.Raodenoba,Jami=(decimal)ch.FasdaklebuliJami,Dro=c.Dro.DateTime}))
//	.Where(x=>bananasKodebi.Contains(x.Ref))
//.Concat(
//gadaadgilebebi.Where (x => x.Sad.ToLower().StartsWith("gak"))
//	.SelectMany (c => c.Chanacerebi.Select (ch => new {Ref=(string)ch.Ref,Raodenoba=(double)ch.Raodenoba,Jami=ch.Jami.Value,c.Dro}))
//	.Where(x=>bananasKodebi.Contains(x.Ref))
//	).Dump();
//	
//return;

////gdaaadgilebebi plius gakidvebi
//gadaadgilebebi
//	.Select (x => new {x.Key, x.Dro, x.Saidan, x.Sad, Tankha=x.Chanacerebi.Sum(z=>z.Jami)})
//	.OrderBy (x => x.Dro)
//	.GroupBy (x => x.Dro.Year*100+x.Dro.Month)
//	.Dump();
//return;

////refit baratiani chekis modzebna
//chekebi
//.Where (c => c.klientisBaratisNomeri != null)
//.Where (c => c.Chanacerebi.Any (ch => ch.Ref.EndsWith("8811-0")))
//.Take(10).Dump();
//return;

//////REALIZACIA
//
//chekebi
//	.SelectMany (cheki => 
//		cheki.gadakhdebi.Select (g => new {Dro=cheki.Dro,cheki.Mdebareoba,g.forma,g.tankha,Shenishvna=(string)cheki.Nomeri})
//				)
//.Concat(
//		gadaadgilebebi.Where (x => x.Sad.ToLower().StartsWith("gak"))	
//			.Select (x => new {x.Dro, Mdebareoba=x.Saidan, forma="n/a", tankha = x.Chanacerebi.Sum (c => c.Jami.Value), Shenishvna=(string)x.Key})
//	)
//	.Select (g => new {Tve = g.Dro.Year * 100 + g.Dro.Month, Dge = g.Dro.Date, g.Dro, g.Mdebareoba, g.forma, Tankha = g.tankha, g.Shenishvna })
//	.DumpToExcel("Realizacia_"+DateTime.Today.ToString("yyyy.MM.dd"));


//var yvelaCiknisRefi = shekveta.Where (s => s.Shenishvna==@"D:\Anvol\Invoicebi\PakingLists\Paking წიგნები ქართული 30.05.2013.xlsx\Sheet1" || s.Shenishvna == @"D:\Anvol\Invoicebi\PakingLists\Paking წიგნები რუსული 31.05.2013.xlsx\Sheet1")
//	.SelectMany (s => s.Chanacerebi.Select (c => c.Ref))
//	.Distinct()
//	.ToList();
//var diplomatisBaratebi = new []{"001686","004999","000265"}.ToList();
//
//chekebi
//	.SelectMany (cheki => 
//		cheki.Chanacerebi.Select (c => new {cheki.Dro, cheki.Mdebareoba, Tankha=c.FasdaklebuliFasi,Diplomati=diplomatisBaratebi.Contains(cheki.klientisBaratisNomeri)?"diplomati":"",VATisGareshe=yvelaCiknisRefi.Contains(c.Ref)?"cigni":""})
//				)
//.Concat(
//		gadaadgilebebi.Where (x => x.Sad.ToLower().StartsWith("gak"))	
//			.SelectMany (x => 
//				x.Chanacerebi.Select(c => new {x.Dro, Mdebareoba=x.Saidan, Tankha = c.Jami.Value, Diplomati="",VATisGareshe=yvelaCiknisRefi.Contains(c.Ref)?"cigni":""})
//			)
//	).Where(x=>!string.IsNullOrWhiteSpace(x.Diplomati) || !string.IsNullOrWhiteSpace(x.VATisGareshe))
//.DumpToExcel("Realizacia_Diplomati_VAT"+DateTime.Today.ToString("yyyy.MM.dd"));
//
//return;
//	
//return;
var refDict = refebi.GroupBy (r => momeRefi(r.Id, r.Dasakheleba)).ToDictionary (g => g.Key, g=>g.First ());


////BrendebisReporti
//var realizaciisChanacerebi = (
//				from d in chekebi
//				from c in d.Chanacerebi
//				select new {d.Dro, d.Mdebareoba,c.Ref,c.Raodenoba,Tankha=c.FasdaklebuliFasi}
//			).Concat(
//				from d in gadaadgilebebi.Where (x => x.Sad.ToLower().StartsWith("gak"))
//				from c in d.Chanacerebi
//				select new {d.Dro, Mdebareoba=d.Saidan, c.Ref,Raodenoba=(double)c.Raodenoba,Tankha=c.Jami.Value}
//			).ToList();
//var brandisReportisChanacerebi = (
//	from c in realizaciisChanacerebi
//	let Dasakheleba = (refDict.ContainsKey(c.Ref) ? refDict[c.Ref].Dasakheleba ?? "N/A" : "N/A").ToUpperInvariant().Trim()
//	let segmentebi = Dasakheleba.Split(' ')
//	select new {
//		Periodi = c.Dro.Year * 100 + c.Dro.Month,
//		c.Mdebareoba,
//		c.Ref,
//		Dasakheleba=Dasakheleba + ":" + c.Ref, 
//		Brendi = string.Join(" ", segmentebi.Take(1)),
//		KveBrendi = string.Join(" ",segmentebi.Take(2)),
//		c.Raodenoba,
//		c.Tankha,
//	}
//).ToList();
//
//var realizaciaPeriodebisMikhedvit = brandisReportisChanacerebi.GroupBy (x => x.Periodi).ToDictionary (x => x.Key,x=>x.Sum (x_ => x_.Tankha));
//var reports = (
//	(
//		from c in brandisReportisChanacerebi
//		group c by new {c.Periodi,c.Mdebareoba} into g
//		let Tankha = g.Sum (x => x.Tankha)
//		select new {g.Key.Periodi, Sheet="Stock",Value=g.Key.Mdebareoba, Tankha, Procenti= Tankha / realizaciaPeriodebisMikhedvit[g.Key.Periodi]}
//	).GroupBy (x => x.Periodi).SelectMany (g => g.OrderByDescending (x => x.Tankha))
//).Concat(
//	(
//		from c in brandisReportisChanacerebi
//		group c by new {c.Periodi,c.Brendi} into g
//		let Tankha = g.Sum (x => x.Tankha)
//		select new {g.Key.Periodi, Sheet="BrendiTop30", Value=g.Key.Brendi, Tankha, Procenti= Tankha / realizaciaPeriodebisMikhedvit[g.Key.Periodi]}
//	).GroupBy (x => x.Periodi).SelectMany (g => g.OrderByDescending (x => x.Tankha).Take(30))
//).Concat(
//	(
//		from c in brandisReportisChanacerebi
//		group c by new {c.Periodi,c.KveBrendi} into g
//		let Tankha = g.Sum (x => x.Tankha)
//		select new {g.Key.Periodi, Sheet="KveBrendiTop50", Value=g.Key.KveBrendi, Tankha, Procenti= Tankha / realizaciaPeriodebisMikhedvit[g.Key.Periodi]}
//	).GroupBy (x => x.Periodi).SelectMany (g => g.OrderByDescending (x => x.Tankha).Take(50))
//)
//.Concat(
//	(
//		from c in brandisReportisChanacerebi
//		group c by new {c.Periodi,c.Dasakheleba} into g
//		let Tankha = g.Sum (x => x.Tankha)
//		select new {g.Key.Periodi, Sheet="ArtikuliTop50", Value=g.Key.Dasakheleba, Tankha, Procenti= Tankha / realizaciaPeriodebisMikhedvit[g.Key.Periodi]}
//	).GroupBy (x => x.Periodi).SelectMany (g => g.OrderByDescending (x => x.Tankha).Take(50))
//).ToList();
//foreach (var report in reports.GroupBy (x => x.Periodi))
//{
//	report.ToList().DumpToExcel("brendisReporti"+report.Key, x => x.Sheet);
//}
//
//return;

//
//
//var gakidvebi2 = (from cheki in gadaadgilebebi.Where (x => x.Sad.ToLower().StartsWith("gak"))
// from cha in cheki.Chanacerebi
// select new {cha.Ref,Periodi=cheki.Dro.Year*100+cheki.Dro.Month,Mdebareoba=cheki.Saidan,Tankha=cha.Jami.Value}
//).Concat
//(from cheki in chekebi
// from cha in cheki.Chanacerebi
// select new {cha.Ref,Periodi=cheki.Dro.Year*100+cheki.Dro.Month,Mdebareoba=cheki.Mdebareoba,Tankha=cha.FasdaklebuliJami}
//);
//(from ga in gakidvebi2
// group ga by new {ga.Ref,Periodi=ga.Periodi,ga.Mdebareoba} into g
// let Dasakheleba = refDict[g.Key.Ref].Dasakheleba
// let seg =  (Dasakheleba ?? "N/A").Split(' ')
// select new { g.Key.Periodi, g.Key.Mdebareoba, Ref=Dasakheleba+"; "+g.Key.Ref, Brendi = seg.FirstOrDefault(), KveBrendi = string.Join(" ", seg.Take(2)), Dasakheleba, Tankha = g.Sum (x => x.Tankha)}
//).Dump();
//return;


//return;

//chekebi	.Where (c => c.Dro<new DateTime(2013,9,1))
//		.GroupBy (c => c.Mdebareoba)
//		.Select (g => new {g.Key,sum=g.SelectMany (x => x.Chanacerebi).Sum (x => x.FasdaklebuliJami)})
//		.Dump();
//		
////return;

//(
//	from ce in chekebi
//	from ca in ce.Chanacerebi
//	group ca by ce.Mdebareoba into g
//	select new {g.Key, Sum=g.Sum (x => x.FasdaklebuliJami)}
//).Dump();
//return;
//list gadaadgilebebi
//gadaadgilebebi
//.Select (x => new {x.Key,x.Saidan,x.Sad,x.Dro,RaodJami = x.Chanacerebi.Sum (x_ => x_.Raodenoba),x.Chanacerebi})
//.OrderBy (x => x.Dro)
//.GroupBy (x => x.Dro.Year*100 + x.Dro.Month)
//.Dump();
//return;
//6019675;49492
//return;
migeba.SelectMany (m => m.Chanacerebi.Select (c => new {
	f="sheskidvebi",
	t="sackobi",
	Ref=c.Ref.Trim(),
	c.Raodenoba,
	c.Jami,
	m.Dro,
	rigi=1
}));

var moves =
migeba.SelectMany (m => m.Chanacerebi.Select (c => new {
	f="sheskidvebi",
	t="sackobi",
	Ref=c.Ref.Trim(),
	c.Raodenoba,
	c.Jami,
	m.Dro,
	rigi=1,
	c.Shenishvna
})).Concat(
gadaadgilebebi.SelectMany (g => g.Chanacerebi.Select (c => new{
	f=c.Raodenoba>0.0?c.Saidan.ToLower():c.Sad.ToLower(),
	t=c.Raodenoba>0.0?c.Sad.ToLower():c.Saidan.ToLower(),
	Ref=c.Ref.Trim(),
	Raodenoba=(double)(c.Raodenoba>0?c.Raodenoba:c.Raodenoba * -1),
	Jami=0m,
	g.Dro,
	rigi=2,
	Shenishvna = g.Key,
})).Concat(
chekebi.SelectMany (ch => ch.Chanacerebi.Select (c => new {
	f=(string)ch.Mdebareoba.ToLower(),
	t=(string)"gakidvebi/"+ch.Mdebareoba.ToLower(),
	Ref=(string)c.Ref.Trim(),
	Raodenoba=(double)c.Raodenoba,
	Jami=0m,
	Dro=ch.Dro,
	rigi=3,
	Shenishvna = ch.Nomeri,
}))
))
.OrderBy (x => x.rigi)
.OrderBy (x => x.Dro)
.GroupBy (x => x.Ref)
.Select  (g => {
				var tg = new Tvitgirebulebebi();
				return g.OrderBy (x => x.Dro)
					.Scan(new { f="", t="", Ref="", Raodenoba=0.0, Jami=0m, Dro=DateTime.Now, nastebi="", Shenishvna=""}, (a,x)=> {
					
					if(x.Jami>0){
						tg.Daamate(x.t, x.Raodenoba, x.Jami);
						return new { x.f, x.t, x.Ref, Raodenoba=x.Raodenoba, Jami=x.Jami, Dro=x.Dro, nastebi=tg.DumpSackobisNashtebi(), Shenishvna=x.Shenishvna};
					} else {
						try{
							tg.MomeGirebuleba(x.f, x.Raodenoba);
						}
						catch{
							new {tg,x.f, x.Raodenoba}.Dump();
						}
						var girebuleba = tg.MomeGirebuleba(x.f, x.Raodenoba);
						
						tg.Moakeli(x.f, x.Raodenoba, girebuleba);
						tg.Daamate(x.t, x.Raodenoba, girebuleba);
						
						return new { x.f, x.t, Ref=x.Ref, Raodenoba=x.Raodenoba, Jami=girebuleba, Dro=x.Dro, nastebi=tg.DumpSackobisNashtebi(),Shenishvna=x.Shenishvna};
					}
				});
			}
		)
.SelectMany (g => g)
.ToList();
//moves.Where (m => m.Ref.EndsWith("3654")).Dump();
//return;

//refebi.Where (r => r.Id.Contains("150")).Dump();
//return;
//.Dump()

//moves.GroupBy (m => m.Ref)
//.OrderByDescending (g => g.Count ())
//.Take(10)
//.Dump();
//return;
//migeba.SelectMany (m => m.Chanacerebi).Sum (x => x.Jami).Dump();
//	moves.Select (m => new {l=m.f, Ref="'"+m.Ref, Raodenoba=m.Raodenoba*-1, Tankha=m.Jami*-1})
//.Concat(
//	moves.Select (m => new {l=m.t, Ref="'"+m.Ref, m.Raodenoba, Tankha=m.Jami})
//).GroupBy (m => new {m.l,m.Ref})
//.OrderByDescending (m => m.Count ())
//.Take(10)
//.Dump();

//var problemuriModzraobebi = moves
//			.Where (m => m.Jami <= 0 && m.t.StartsWith("gak"))
//			.GroupBy (m => new {m.f,m.Ref})
//			.Select (g => new {g.Key.f,Ref=g.Key.Ref,Raod=g.Sum (x => x.Raodenoba),MinDro=g.Min (x => x.Dro.Date).ToString("yyyy.MM.dd")})
//			.Where (x => x.f != "sackobi")
//			.Select (x => string.Format("{0}\t{1}\t{2}\t{3}",x.f,x.Ref,x.Raod,x.MinDro))
//			.Dump();
//
//return;


var dt = //DateTime.Parse("2014-01-01")
		 DateTime.Today
		 ;

var gatarebebi2 = 	
    moves.Select (m => new {l=m.f,Ref="'"+m.Ref,Raodenoba=m.Raodenoba*-1, Tankha=m.Jami*-1, m.Dro})
.Concat(
	moves.Select (m => new {l=m.t,Ref="'"+m.Ref,m.Raodenoba, Tankha=m.Jami, m.Dro})
).Where (x => x.Dro < dt);

var gatarebebi=
	gatarebebi2
	.GroupBy (x => new {x.l, x.Ref, Periodi=x.Dro.Year*100+x.Dro.Month})
	.Select (g => new {g.Key.l, g.Key.Ref,g.Key.Periodi, Raodenoba=g.Sum (x => x.Raodenoba), Tankha=g.Sum (x => x.Tankha)});
gatarebebi.DumpToExcel("Tvitgirebuleba_"+DateTime.Today.ToString("yyyy.MM.dd"));
return;
////NASHTI TVITGIREBULEBIT
//gatarebebi2
//.GroupBy (m => new {m.l, Periodi = m.Dro.Year * 100 + m.Dro.Month, m.Dro.Date})
//.Select (g => new{ g.Key.l, g.Key.Periodi,g.Key.Date, Raodenoba=g.Sum (x => x.Raodenoba),Tankha=g.Sum (x => x.Tankha)})
//.Dump();
//return;

(
	(
		from r in gatarebebi
		select new {r.Ref,r.Periodi,l=r.l+" stock (qty)",value=r.Raodenoba}
	).Concat(
		from t in gatarebebi
		select new {t.Ref,t.Periodi,l=t.l+" stock (purch. price)",value=(double)t.Tankha}
	).Concat(
		chekebi.Where (c => c.Dro < dt).SelectMany (d => d.Chanacerebi
					.Select (x => new {Ref="'"+x.Ref, Periodi = d.Dro.Year*100+d.Dro.Month,l=d.Mdebareoba+" sales (sum+vat)",value=(double)x.FasdaklebuliJami})
				)
	.Concat(
		gadaadgilebebi.Where (g => g.Dro < dt).Where (x => x.Sad.ToLower().StartsWith("gak"))
				.SelectMany (d => d.Chanacerebi
					.Select (x => new {Ref="'"+x.Ref,Periodi = d.Dro.Year*100+d.Dro.Month,l=d.Saidan+" sales (sum+vat)",value=(double)x.Jami.Value})
				)
				
		).GroupBy (x => new {x.Ref,x.l,x.Periodi})
		.Select (g => new {g.Key.Ref,g.Key.Periodi,g.Key.l, value = g.Sum (x => x.value)})
	
	).Select (wc => {
		var l  = wc.l
					.Replace("gakidvebi/batumi", "batumi sales")
					.Replace("gakidvebi/merani", "merani sales")
					.Replace("gakidvebi/mall", "mall sales")
					
					.Replace("gakidvebi stock (qty)", "warehouse sales stock (qty)")
					.Replace("gakidvebi stock (purch. price)", "warehouse sales stock (purch. price)")
					.Replace("sackobi sales (sum+vat)", "warehouse sales stock (sum+vat)")
					.Replace("sackobi stock (purch. price)", "warehouse stock (purch. price)")
					.Replace("sackobi stock (qty)", "warehouse stock (qty)")
					.Replace("sheskidvebi stock (purch. price)", "total purchase stock (purch. price)")
					.Replace("sheskidvebi stock (qty)", "total purchase stock (qty)")
					;
		
		
		var mosadzebni = wc.Ref.Substring(1);
		
		var re = Enumerable
		.Range(0,12)
		.Select(i=>{
			Refi r0 = null;
			refDict.TryGetValue(mosadzebni.PadLeft(i, '0'), out r0);
			return r0;
		}).FirstOrDefault (refi => refi != null);
		
		
		if(re == null) {
		}
		return new {wc.Ref,Dasakheleba=re==null?"":re.Dasakheleba,Ean="'"+(re==null?"":string.Join(";",re.Eans)),wc.Periodi,l,wc.value};
	}
)
).DumpToExcel("Nashtebi_"+DateTime.Today.ToString("yyyy.MM.dd"));


//return;
//(
//
//from x in gatarebebi
//let r =  refebi.Where(r => ("'"+momeRefi(r.Id, r.Dasakheleba))==x.Ref).Take(1).ToList()
//select new {
//		x.l, 
//		x.Raodenoba, 
//		
//		x.Ref, 
//		Dasakheleba = r.Select (x_ => x_.Dasakheleba).FirstOrDefault (),
//		Fasi = r.Select (x_ => x_.Fasi).FirstOrDefault (),
//}
//
//
//).Dump();

return;
////return;
//Util.WriteCsv((
//from x in gatarebebi
//let r =  refebi.Where(r => ("'"+momeRefi(r.Id, r.Dasakheleba))==x.Ref).Take(1).ToList()
//select new {
//		x.l, 
//		x.Raodenoba, 
//		x.Ref, 
//		Dasakheleba = r.Select (x_ => x_.Dasakheleba).FirstOrDefault (),
//		Fasi = r.Select (x_ => x_.Fasi).FirstOrDefault (),
//}
//
//), @"d:\anvol\tvitgir20131006.csv");
//
//return;

	
//
//chekebi.Store(o=>"cheki/","Chekebi")
//							.GroupBy (x => new {x.Mdebareoba, Periodi = x.Dro.Year * 10000 + x.Dro.Month * 100 + x.Dro.Day })
//							.Select (g => new {g.Key.Mdebareoba, g.Key.Periodi, Realizacia = g.SelectMany(x => x.Chanacerebi).Sum (x => x.FasdaklebuliJami)})
//							.OrderBy (x => x.Mdebareoba)
//							.OrderBy (x => x.Periodi)
//							.Dump();
//
//shekveta.Store(o=>"shekveta/","Shekvetebi")
//								.Count ()
//								.Dump();
//migeba.Store(o=>"migeba/","Migebebi")
//								.Count ()
//								.Dump();
//
//
//gadaadgilebebi.Store(o=>"gadaadgileba/","Gadaadgilebebi")
//								.Count ()
//								.Dump();
//refebi.Store(o=>"refi/","Refebi")
//								.Count ()
//								.Dump();
		
}


public class Tvitgirebulebebi
{
	class Gir{
		public decimal Tankha;
		public double Raodenoba;
	}
	Dictionary<string,Gir> _dic = new Dictionary<string,Gir>();
	public string DumpSackobisNashtebi(){
		var rez = _dic.OrderBy (x => x.Key).Select (x => string.Format("{0}(t:{1:N}/r:{2:N})",x.Key,x.Value.Tankha,x.Value.Raodenoba));
		return string.Join(", ", rez);
	}
	public void Daamate(string sackobi, double raodenoba, decimal tankha)
	{	
		Gir tvitGir;
		if(_dic.TryGetValue(sackobi, out tvitGir))
		{
			_dic[sackobi] = new Gir{ Raodenoba = tvitGir.Raodenoba + raodenoba, Tankha = tvitGir.Tankha + tankha};
		}
		else
		{
			_dic.Add(sackobi,new Gir{ Raodenoba = raodenoba, Tankha = tankha});
		}
	}
	
	public decimal MomeGirebuleba(string sackobi, double raodenoba)
	{
		Gir tvitGir;
		if(_dic.TryGetValue(sackobi, out tvitGir)){
			if(raodenoba >= tvitGir.Raodenoba)
				return tvitGir.Tankha;
			return Math.Min(tvitGir.Tankha, Math.Round(((decimal)raodenoba*tvitGir.Tankha / (decimal)tvitGir.Raodenoba), 2));
		}
		return 0m;
		
	}
	public void Moakeli(string sackobi, double raodenoba, decimal tankha)
	{
		Gir tvitGir;
		if(_dic.TryGetValue(sackobi, out tvitGir)){
			var aRaod = tvitGir.Raodenoba - raodenoba;
			var aTankha = tvitGir.Tankha - tankha;
			_dic[sackobi] = new Gir{ Raodenoba = aRaod<0?0.0:aRaod, Tankha = aTankha};
			
		}
	}
	
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
	if(File.Exists(sDir)) {
		yield return sDir;
	}

	if(Directory.Exists(sDir)) {
		foreach(var file in Directory.GetFiles(sDir, searchPattern)) {
			yield return file;
		}
		foreach(var dir in Directory.GetDirectories(sDir)) {
			foreach(var file in DirSearch(dir, searchPattern)){
				yield return file;
			}
		}
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

public static class EsEx
{
	static EsEx()
	{
		ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;
		
	}
	public static IEnumerable<T> StoreToEs<T>(this IEnumerable<T> source, string streamName, Func<T,string> eventType)
	{
		using(var conn = EventStore.ClientAPI.EventStoreConnection.Create(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1113)))
		{
			conn.Connect();
			foreach (var buff in source.Buffer(128))
			{
				conn.AppendToStream(streamName,-2, buff.Select (x => {
					var str  = ServiceStack.Text.JsonSerializer.SerializeToString(x);
					var bytes = Encoding.UTF8.GetBytes(str);
					return new EventStore.ClientAPI.EventData(Guid.NewGuid(),eventType(x),true, bytes,null);
				}).ToArray());
			}
		}
		return source;
	}
}

public static class StoreEx{
	public static IEnumerable<T> Store<T>(this IEnumerable<T> source, Func<T,string> idGenerator, 
											string entityName = "Gadaadgilebebi", string url="http://localhost:8080", 
											string dataBase = "TestDb") {
		StoreEx.Initialize(ds => {
			foreach(var g in source)
			{
				var metadata = new RavenJObject();
				metadata.Add("Raven-Entity-Name", RavenJToken.FromObject(entityName));
				ds.DatabaseCommands.Put(idGenerator(g), null, RavenJObject.FromObject(g), metadata);
			}
			
		}, url,dataBase);
		return source;
	}



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


static List<string> gadaadgilebidanAmosagebi = @"
18-43200 
14166 
34043 
16174 
1002 
914-155 
A16 
A30DOUBLE 
B18 
B28A 
B28B 
399414 
30558.43 
A0142T 
LC56006 
86420 
40.83453 
40.83455 
51.213 
556057 
13481 
71666 
1633 
16100 
900335 
100036 
110312 

".Split('\n').Select (x => x.Trim()).Where (x => x.Length>0).ToList();
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

static List<string> gasaukmebeliChekebi = @"
2/1822
1/1822
2/10074
1/3099
3/61
"   .Split('\n')
	.Where (x => x.Length > 0)
	.ToList();
static string gamorcheniliGadaadgilebebi = @"
batumi  500902E  1  2013.08.24 
merani  500902E  5  2013.09.03 
batumi  8466  1  2013.08.16 
batumi  A16(JJ8007)  2  2013.09.08 
batumi  B18(JJ8011)  1  2013.09.22 
batumi  A30DOUBLE(JJ8012)  1  2013.09.07 
batumi  B28A(JJ8013)  1  2013.09.13 
batumi  97730  1  2013.09.06 
merani  51.2130  1  2013.10.14 
merani  8665338  4  2013.09.01 
merani  8620288  15  2013.08.19 
batumi  4189117  5  2013.09.04 
merani  5630386  4  2013.08.24 
merani  2612275  4  2013.08.19 
batumi  8650182  15  2013.09.04 
merani  SCAC26288-7  1  2013.09.07 
merani  54690  3  2013.08.20 
merani  A0142T5  2  2013.08.19 
batumi  A0142T5  5  2013.08.24 
batumi  BC914-155  2  2013.08.21 
merani  42336W  6  2013.08.02 
merani  42353W  5  2013.08.13 
merani  49740W  8  2013.08.10 
batumi  49740W  71  2013.08.19 
batumi  17192T  2  2013.08.17 
batumi  17211T  1  2013.09.09 
batumi  34043T  1  2013.08.12 
merani  34047  1  2013.09.01 
batumi  3407-0  1  2013.09.11 
merani  54101  5  2013.07.13 
batumi  90134  1  2013.08.25 
merani  C058H  1  2013.08.30 
merani  C060H  1  2013.07.23 
merani  C067H  1  2013.07.11 
merani  C075H  2  2013.08.19 
merani  MC087H  1  2013.10.08 
merani  MC093H  3  2013.08.03 
merani  C096H  1  2013.08.16 
merani  C107H  1  2013.08.26 
merani  C114H  2  2013.07.17 
merani  MC117H  1  2013.08.03 
merani  C123H  2  2013.07.23 
merani  C702H  1  2013.08.03 
merani  C705H  1  2013.07.25 
merani  C707H  1  2013.08.24 
merani  C714H  2  2013.07.14 
merani  MC072H  1  2013.07.23 
merani  S3001  1  2013.08.25 
merani  S3004  1  2013.08.28 
merani  S3006  2  2013.08.19 
merani  S3008  2  2013.07.25 
merani  S3010  2  2013.07.25 
merani  T4005H  4  2013.07.15 
merani  T4009H  5  2013.07.29 
merani  T4010H  1  2013.10.05 
merani  T4011H  1  2013.09.27 
batumi  9001890110016  3  2013.08.24 
merani  9001890132117  5  2013.07.18 
batumi  9001890132117  6  2013.08.19 
batumi  9001890132216  3  2013.08.17 
batumi  9001890136016  5  2013.08.30 
merani  9001890136016  1  2013.10.12 
batumi  9001890138218  3  2013.08.26 
batumi  9001890139017  1  2013.09.01 
merani  9001890139512  1  2013.08.11 
merani  9001890141416  1  2013.08.27 
batumi  9001890141416  4  2013.09.01 
merani  9001890151316  1  2013.08.19 
batumi  9001890151316  1  2013.08.31 
batumi  9001890151712  1  2013.09.15 
batumi  9001890152511  1  2013.10.04 
batumi  9001890153112  1  2013.08.15 
batumi  9001890188619  1  2013.10.07 
merani  9001890201943  1  2013.08.14 
batumi  9001890221347  1  2013.09.01 
merani  9001890221347  1  2013.09.04 
merani  9001890222344  1  2013.10.05 
batumi  9001890222542  1  2013.10.06 
batumi  9001890227547  1  2013.08.24 
batumi  9001890228049  1  2013.09.06 
batumi  9001890230035  1  2013.08.26 
merani  9001890236037  4  2013.07.31 
batumi  9001890409196  1  2013.08.30 
batumi  9001890790591  5  2013.08.05 
merani  HWA404301  1  2013.08.05 
merani  CRONA1  11  2013.07.11 
batumi  CRONA1  21  2013.08.09 
merani  53700  4  2013.08.21 
batumi  ELEMENTIAA  48  2013.08.17 
merani  ELEMENTIALKALINEAA  327  2013.07.11 
batumi  ELEMENTIALKALINEAA  724  2013.08.04 
merani  38587  2  2013.09.13 
merani  869-310  1  2013.10.14 
merani  9426  1  2013.08.27 
batumi  9426  1  2013.09.04 
batumi  WD7344  1  2013.09.09 
merani  10232-10  1  2013.08.25 
merani  14680  1  2013.09.04 
merani  Y0421-0  1  2013.08.19 
merani  W7853-0  1  2013.09.14 
batumi  80503  1  2013.09.08 
merani  511342  2  2013.08.22 
merani  516613  2  2013.10.12 
batumi  18-30263  3  2013.08.08 
batumi  18-30270  12  2013.08.10 
batumi  18-43202  1  2013.09.21 
merani  18-43203  1  2013.07.12 
merani  18-43204  1  2013.07.12 
batumi  18-43204  1  2013.10.01 
batumi  18-43206  1  2013.09.07 
merani  18-43206  1  2013.10.02 
batumi  18-43207  1  2013.08.08 
batumi  18-43209  1  2013.08.11 
batumi  18-43212  3  2013.09.03 
merani  2502  6  2013.07.31 
batumi  27280  6  2013.08.27 
merani  27280  3  2013.09.24 
merani  4356  1  2013.10.09 
batumi  903653  1  2013.09.16 
batumi  1000042  2  2013.08.13 
merani  1000042  1  2013.08.25 
batumi  1000245  3  2013.09.07 
merani  1100468  1  2013.07.14 
batumi  600242  1  2013.09.09 
merani  64024  1  2013.10.06 
batumi  30558  1  2013.09.06 
merani  978-9941-19-156-5  1  2013.08.08 
merani  978-9941-19-105-3  1  2013.08.10 
merani  978-9941-19-842-7  1  2013.07.29 
merani  978-9941-19-290-6  1  2013.07.26 
merani  978-9941-19-555-6  1  2013.08.14 
merani  978-9941-19-562-4  1  2013.08.03 
merani  978-9941-19-557-0  1  2013.09.02 
merani  978-9941-19-478-8  1  2013.09.17 
merani  978-9941-19-475-7  1  2013.08.14 
merani  978-9941-19-319-4  1  2013.09.17 
merani  978-9941-19-452-8  1  2013.07.12 
merani  9789941194320  1  2013.10.02 
merani  978-9941-19-993-6  1  2013.09.12 
merani  978-9941-19-101-5  1  2013.10.02 
merani  978-9941-19-024-7  1  2013.10.08 
merani  978-9941-19-023-0  1  2013.09.17 
merani  978-9941-19-664-5  1  2013.10.10 
merani  978-9941-19-106-0  1  2013.10.08 
merani  978-9941-19-854-0  1  2013.07.17 
merani  978-9941-19-634-8  1  2013.07.11 
merani  978-9941-19-628-7  1  2013.07.11 
merani  978-9941-19-484-9  1  2013.08.31 
merani  978-9941-19-747-5  1  2013.08.08 
merani  9789941195037  1  2013.09.28 
merani  978-9941-19-836-6  1  2013.08.31 
merani  9789941198373  1  2013.08.14 
merani  978-9941-19-318-7  1  2013.08.10 
merani  978-99940-42-80-7  1  2013.07.17 
merani  978-9941-19-700-0  1  2013.07.22 
merani  978-9941-19-686-7  1  2013.08.17 
merani  978-9941-19-356-9  1  2013.09.24 
merani  9785945821453  1  2013.09.28 
merani  9785945821743  1  2013.08.28 
merani  9785378021512  1  2013.09.14 
merani  9785378001545  1  2013.09.05 
merani  9785353002789  1  2013.07.22 
merani  9785353003182  1  2013.07.22 
merani  9785353006824  1  2013.08.12 
merani  9785353031130  1  2013.07.20 
merani  9785378103317  1  2013.08.12 
merani  9785378090341  1  2013.07.21 
merani  9785378100675  1  2013.07.18 
merani  9785378009923  1  2013.09.02 
merani  9785378010103  1  2013.09.05 
merani  9785378006427  1  2013.09.02 
merani  9785783314599  1  2013.10.04 
merani  9785783314612  1  2013.07.26 
merani  9785783314605  1  2013.10.04 
merani  9785783313547  1  2013.07.27 
merani  9785378023806  1  2013.07.22 
merani  9785378014545  1  2013.07.16 
merani  9785378025428  1  2013.08.05 
merani  9785378025015  1  2013.08.17 
merani  9785378022687  1  2013.07.22 
merani  9785378023189  1  2013.07.22 
merani  9785378024971  1  2013.07.22 
merani  9785378036905  1  2013.07.26 
merani  9785378022090  1  2013.08.23 
merani  9785378012497  1  2013.07.26 
merani  9785783313011  1  2013.09.28 
merani  9785783311758  1  2013.09.03 
merani  9785783313387  1  2013.07.26 
merani  9785474006024  1  2013.09.14 
merani  9785474005614  1  2013.09.28 
merani  9785980885120  1  2013.07.25 
merani  9785980885113  1  2013.07.21 
merani  9785980886141  1  2013.07.21 
merani  9785980885779  1  2013.07.14 
merani  9785980885786  1  2013.08.20 
merani  9785980886282  1  2013.07.22 
merani  9785980886974  1  2013.07.29 
merani  9785378025626  1  2013.10.03 
merani  9785378025800  1  2013.09.13 
merani  9785378026142  1  2013.07.25 
merani  9785378025817  1  2013.09.13 
merani  9785378025275  1  2013.09.13 
merani  9785378042531  1  2013.08.14 
merani  9785945821781  1  2013.08.25 
merani  9785378025657  1  2013.08.21 
merani  9785378025718  1  2013.08.04 
merani  9785783314117  1  2013.08.29 
merani  9785783316944  1  2013.09.02 
merani  9785783312373  1  2013.07.25 
merani  9785783314124  1  2013.08.23 
merani  9785783310874  1  2013.08.03 
merani  9785783314490  1  2013.09.28 
merani  9785378032327  1  2013.08.25 
merani  9785378018482  1  2013.10.13 
merani  9785387008346  1  2013.08.02 
merani  9785387011377  1  2013.07.18 
merani  9785387004249  1  2013.07.18 
merani  9785378005499  1  2013.09.02 
merani  9785378007325  1  2013.09.02 
merani  9785378005468  1  2013.08.24 
merani  9785378031863  1  2013.08.01 
merani  9785699595938  1  2013.07.26 
merani  9785783313530  1  2013.10.05 
merani  9785783312991  1  2013.08.03 
merani  9785783310485  1  2013.07.21 
merani  9785783316036  1  2013.08.02 
merani  9785783316029  1  2013.08.03 
merani  9785402013094  1  2013.08.26 
merani  9785402013131  1  2013.09.26 
merani  9785378032143  1  2013.09.14 
merani  2082  1  2013.10.14 
merani  92652  1  2013.10.12 
batumi  35682  1  2013.10.06 
merani  1348I  1  2013.09.12 
merani  16100I  1  2013.09.11 
merani  1633I  2  2013.08.17 
batumi  1200234  1  2013.09.08 
merani  56057  1  2013.09.15 
batumi  6F22REL/1BP  1  2013.10.08 
merani  6F22REL/1BP  1  2013.10.11 
";


IEnumerable<PakingListi> MomePakingListebi(Func<string,string,string> momeRefi)
{
	return       PakingLists(momeRefi, @"D:\Anvol\Invoicebi\PakingLists" )
		.Concat( PakingLists(momeRefi, @"D:\Anvol\Invoicebi\PakingLists2") )
		.Concat( PakingLists(momeRefi, @"D:\Anvol\Invoicebi\PakingLists3") )
		.Concat( PakingLists(momeRefi, @"D:\Anvol\Invoicebi\PakingLists4") )
		.Concat( PakingLists(momeRefi, @"D:\Anvol\Invoicebi\PakingLists5") )
		.Concat( PakingLists(momeRefi, @"D:\Anvol\Invoicebi\PakingLists6") )
		.Concat( PakingLists(momeRefi, @"D:\Anvol\Invoicebi\PakingLists7") )
		.Concat( PakingLists(momeRefi, @"D:\Anvol\Invoicebi\PakingLists8") )
		.ToList();
}
IEnumerable<Gadaadgileba> MomeGadaadgilebebi(Func<string,string,string> momeRefi)
{
	return (
		from wb  in DirSearch(@"D:\Anvol\modzraobebi\").Where (x => !x.ToLower().Contains("\\gakidvebi\\")).ToWorkBooks()
		let pathSegments = wb.Path.Split('\\')
		from ws  in wb.Worksheets<Worksheet>()
		from r in ws.Rng("A1:W10000").ToRows()
		select new { Wb = wb.Path + "\\" + wb.Name + "\\" + ws.Name, f=pathSegments[3],t=pathSegments[4], Id=r[0].GetLong(), Kodi=r[1].GetString(), Dasakheleba=r[2].GetString(), Raodenoba=r[4].GetLong(),ErtFasi=r[5].GetDecimal(),Jami=r[6].GetDecimal()} 
		into r2
		where r2.Raodenoba.HasValue && r2.Kodi != null
		group r2 by r2.Wb into g2
		select new Gadaadgileba{
					Key=g2.Key,
					Saidan=g2.First ().f,
					Sad=g2.First ().t,
					Dro=TarigiFailisSakhelidan(g2.Key),
					Chanacerebi=g2.Select (x => new Gadaadgileba.Chanaceri{
						Ref = momeRefi(x.Kodi, x.Dasakheleba), 
						Dasakheleba=x.Dasakheleba, 
						Raodenoba=(double)x.Raodenoba.Value
						})
							.ToList()
					}
	
	).ToList();
}
IEnumerable<Cheki> MomeChekebi(Func<string,string,string> momeRefi)
{
							var posebi = new Dictionary<string,string>{
								{"1", "merani"},
								{"2", "batumi"},
								{"3", "mall"},
								{"4", "mall"},
							};
							var gamocerilebi = 
							            StreamDocs("http://batumi.anvol.ge:8080/", "movlenebi", "Pos")
                            	.Concat(StreamDocs("http://merani.anvol.ge:8080/", "movlenebi", "Pos"))
								.Concat(StreamDocs("http://mall1.anvol.ge:8080/", "movlenebi", "Pos"))
								.Concat(StreamDocs("http://mall2.anvol.ge:8080/", "movlenebi", "Pos"))

								.Where (x => x.type == "GamoiceraCheki" && x.Dro != null)
								.Select (x =>  new {chekisNomeri=(string)x.chekisNomeri,dro=(DateTimeOffset)x.Dro})
								.GroupBy (x => x.chekisNomeri)
								.ToDictionary (x => x.Key, x=>x.Min (x_ => x_.dro));
							
							return (
							from c in            StreamDocs("http://batumi.anvol.ge:8080/", "cheki", "Pos")
													.Where(x => int.Parse(x["@metadata"]["@id"].Split('/')[2])>=1035)
                                         .Concat(StreamDocs("http://merani.anvol.ge:8080/", "cheki", "Pos"))
                                         .Concat(StreamDocs("http://mall1.anvol.ge:8080/", "cheki", "Pos"))
                                         .Concat(StreamDocs("http://mall2.anvol.ge:8080/", "cheki", "Pos"))
										 
							let chekisNomeri = (string)(c.nomeri == null ? c.posisNomeri + "/" + c.chekisNomrisMrickhveli : c.nomeri)
							let Dro = (DateTimeOffset)gamocerilebi[chekisNomeri]
							let posisNomeri = chekisNomeri.Split('/')[0] 
							let Mdebareoba = posebi[posisNomeri]
							where gamocerilebi.ContainsKey(chekisNomeri)
							where (Dro >= new DateTime(2013, 07, 11) || Mdebareoba != "merani") && 
							      (Dro >= new DateTime(2013, 08, 4) || Mdebareoba != "batumi") &&
								  (Dro >= new DateTime(2013, 12, 1) || Mdebareoba != "mall")
							select new Nashti.Core.Cheki {
								Nomeri = chekisNomeri,
								Mdebareoba=Mdebareoba,
								Dro = Dro.DateTime,
								klientisBaratisNomeri= c.klientisBaratisNomeri,
								Gadasakhdeli = (decimal)c.gadasakhdeli.mnishvneloba / 100m,
								FasdaklebuliGadasakhdeli = (decimal)c.fasdaklebuliGadasakhdeli.mnishvneloba / 100m,
								gadakhdebi=((IEnumerable<dynamic>)c.gadakhdebi).Select(x => new Cheki.Gadakhda{forma=(string)x.Key,tankha=(decimal)x.Value.mnishvneloba/100m})
											.Concat(new []{new Cheki.Gadakhda {forma="khurda", tankha=-1m*(decimal)c.gasacemi.mnishvneloba/100m}}).ToList(),
								Chanacerebi = ((IEnumerable<dynamic>)c.produktebi)
										.Select (p => new Cheki.Chanaceri { 
												Ref = momeRefi((string)p.Key, (string)p.Value.dasakheleba),
												Dasakheleba=(string)p.Value.dasakheleba,
												Raodenoba=(double)p.Value.raodenoba.mnishvneloba,
												Fasi=(decimal)p.Value.fasi.tankha.mnishvneloba / 100m,
												FasdaklebuliFasi=(decimal)p.Value.fasdaklebuliFasi.tankha.mnishvneloba / 100m,
												Jami=(decimal)p.Value.jami.mnishvneloba / 100m,
												FasdaklebuliJami=(decimal)p.Value.fasdaklebuliJami.mnishvneloba / 100m,
												}).ToList()
											
											}
							  ).ToList();
}

IEnumerable<Gakidva> MomeGakidvebi(Func<string,string,string> momeRefi)
{

	return (
		from wb in DirSearch(@"D:\Anvol\modzraobebi\").Where (x => x.ToLower().Contains("\\gakidvebi\\")).ToWorkBooks()
		let pathSegments = wb.Path.Split('\\')
		from ws  in wb.Worksheets<Worksheet>()
		from r in ws.Rng("A1:W10000").ToRows()
		select new { Wb = wb.Path + "\\" + wb.Name + "\\" + ws.Name, f=pathSegments[3],t=pathSegments[4], Id=r[0].GetLong(), Kodi=r[1].GetString(), Dasakheleba=r[2].GetString(), Raodenoba=r[4].GetLong(),ErtFasi=r[5].GetDecimal(),Jami=r[6].GetDecimal()} 
		into r2
		where r2.Raodenoba.HasValue && r2.Kodi != null && r2.Jami.HasValue
		group r2 by r2.Wb into g2
		select new Gakidva {
				Key=g2.Key,
				Mdebareoba=g2.First ().f,
				Dro=TarigiFailisSakhelidan(g2.Key),
				Chanacerebi= g2.Select (x => new Gakidva.Chanaceri{
								Ref = momeRefi(x.Kodi, x.Dasakheleba), 
								Dasakheleba=x.Dasakheleba, 
								Raodenoba=(double)x.Raodenoba.Value, 
								Jami=(decimal)x.Jami.Value
							}).ToList()
			}
		).ToList();
}

IEnumerable<Migeba> Migebebi(IEnumerable<PakingListi> shekveta)
{
	var  migebebiDic = new List<Tuple<List<string>,decimal,decimal>>()
	{
		Tuple.Create(new List<string>{@"D:\Anvol\Invoicebi\PakingLists\Packing list for inv.132456 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Packing list for inv.132469 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Packing list for inv.132473 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Packing list for inv.132497 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Packing list for inv.132498 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Packing list for inv.132499 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Packing list for inv.132551 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Packing list for inv.132578 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Packing list for inv.132579 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Packing list for inv.132671 Mattel 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Packing list for inv.132735 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Packing list for inv.132804 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Packing list Invoice nr 0813U 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Packing list Invoice nr 0913U 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Pasking list for inv.132810 23.05.2013.xls\Hansa Report"}, 330.97m, 2.1153m),
		Tuple.Create(new List<string>{@"D:\Anvol\Invoicebi\PakingLists\Packing list for inv.132457 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Packing list for inv.132466 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Packing list for inv.132475 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Packing list for inv.132493 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Packing list for inv.132739 23.05.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists\Packing list Invoice nr 0713U 23.05.2013.xls\Hansa Report"},412.00m,1.636m),
		Tuple.Create(new List<string>{@"D:\Anvol\Invoicebi\PakingLists2\Packing list for inv 133638 28.07.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists2\Packing list for inv 133644 28.07.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists2\Packing list for inv 133645 28.07.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists2\Packing list for inv 133765 Mattel 28.07.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists2\Packing list for inv 133807 28.07.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists2\Packing list for inv.134171 28.07.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists2\Packing list for inv.134172 28.07.2013.xls\Hansa Report"},4181.75m,2.1948m),
		Tuple.Create(new List<string>{@"D:\Anvol\Invoicebi\PakingLists2\Packing list for inv 133666 28.07.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists2\Packing list for inv 133667 28.07.2013.xls\Hansa Report", @"D:\Anvol\Invoicebi\PakingLists2\Packing list for inv 133808 28.07.2013.xls\Hansa Report"},1442.53m,1.6546m),
		
		Tuple.Create(new List<string>{@"D:\Anvol\Invoicebi\PakingLists4\dolar invoicebi gaertinebuli18.10.2013.xlsx\Sheet1"}, 3377.43m, 1.6651m),
		Tuple.Create(new List<string>{@"D:\Anvol\Invoicebi\PakingLists4\EURO INVOICEBI GAERTIENBULI18.10.2013.xlsx\Sheet1"}, 6844.20m, 2.2555m),
		
		Tuple.Create(new List<string>{@"D:\Anvol\Invoicebi\PakingLists5\gaertianebuli evro 22.11.2013.xlsx\Sheet1"}, 449.00m, 2.2519m),
		Tuple.Create(new List<string>{@"D:\Anvol\Invoicebi\PakingLists5\gaertianebuli dolari 22.11.2013.xlsx\Sheet1"}, 400.00m, 1.6758m),
		Tuple.Create(new List<string>{@"D:\Anvol\Invoicebi\PakingLists6\gaertianebuli dolari 25.11.2013.xlsx\Sheet1"}, 1553.96m, 1.6821m),
		Tuple.Create(new List<string>{@"D:\Anvol\Invoicebi\PakingLists6\gaertinebuli evro 25.11.2013.xlsx\Sheet1"}, 7481.68m, 2.2732m),
		Tuple.Create(new List<string>{@"D:\Anvol\Invoicebi\PakingLists7\2013 11 20 Specification GE 04.12.2013.xlsx\Specyfikacja załadunku"}, 5300m*2.3055m+400, 2.3055m),
	};
	var ertadGanbajebuli = migebebiDic.SelectMany(x => x.Item1).ToList();
	return migebebiDic.Concat(
			shekveta.Where (s => !ertadGanbajebuli.Contains(s.Shenishvna))
					.Select (s => Tuple.Create(new List<string>{s.Shenishvna},0m,1m))
			).Select (x => new {p1=x.Item1.SelectMany (i => shekveta.Where (s => s.Shenishvna==i)),p2=x.Item2,p3=x.Item3})
			.Where (x => x.p1.Any ())
			.Select (x => new Migeba(x.p1, x.p2, x.p3))
			;
}

IEnumerable<PakingListi> PakingLists(Func<string,string,string> momeRefi,string path=@"D:\Anvol\Invoicebi\PakingLists")
{
	var valutebi = (
		from wb  in DirSearch(path).ToWorkBooks()
		from ws  in wb.Worksheets<Worksheet>()
		let r = ws.Rng("A1:W20").ToRows().SelectMany (x => x).Select (x => x.GetString()).Where (x => !string.IsNullOrWhiteSpace(x)).Select (x => x.ToUpper()).ToList()
		where r.Count > 3
		select new {Wb = wb.Path + "\\" + wb.Name+"\\"+ ws.Name, Eur = r.Any(x=> x.Contains("EUR")), Usd = r.Any(x=>x.Contains("USD")) }
	).ToList();
	
	if(valutebi.Any (v => v.Eur && v.Usd)){
		throw new InvalidOperationException("ver dadginda Valuta");
	}
	var valutebiDic = valutebi.ToDictionary (x => x.Wb,x => x.Eur ? "EUR" : x.Usd ? "USD" : "GEL");
	var q1 = (
		from wb  in DirSearch(path).ToWorkBooks()
		from ws  in wb.Worksheets<Worksheet>()
		from r in ws.Rng("A1:W10000").ToRows()
		select new {Wb = wb.Path + "\\" + wb.Name+"\\"+ ws.Name, SheetName = ws.Name, A=r[0].GetString(), B=r[1].GetString(), C=r[2].GetString(), D=r[3].GetDouble(), E=r[4].GetDecimal(), F=r[5].GetDecimal(), G=r[6], H=r[7], I=r[8], J=r[9].GetDouble(), K=r[10], L=r[11], M=r[12], N=r[13], O=r[14], P=r[15], Q=r[16], R=r[17], S=r[18], T=r[19], U=r[20],V=r[21],W=r[22] }
	)
	.Where (x => x.D.HasValue && x.E.HasValue && x.F.HasValue && x.D.Value > 0);
	return (
		from x in q1
		group x by x.Wb into g
		select new PakingListi {
				Id=Hash(g.Key),
				Shenishvna=g.Key,
				Dro = TarigiFailisSakhelidan(g.Key),
				Valuta = valutebiDic[g.Key],
				Chanacerebi = g.Select ((x,i) => new PakingListi.Chanaceri(
				momeRefi(x.A, x.B),x.D.Value,i.ToString(),null,x.C,x.B,x.E.Value,Math.Round(x.F.Value,2),x.J
				)).ToList()
			}
	);
}

Func<string,string,string> DaamzadeMomeRefi()
{
	var refisCvlilebebi = StreamDocs("http://office.anvol.ge:8080/", "movlenebi", "Anvol").Where (x => x.Type == "MienichaRefi")
			.Select (x => new {x.Ref, x.AkhaliRefi,x.Dasakheleba,Dro=(DateTimeOffset)x["@metadata"]["Last-Modified"]}).Distinct().OrderBy (x => x.Ref)
			.OrderBy (x => x.Dro)
			.GroupBy (x => new {x.Ref,x.Dasakheleba})
			.Select (g => new {g.Key.Ref,g.Key.Dasakheleba,AkhaliRefi=g.OrderByDescending (x => x.Dro).Select (x => x.AkhaliRefi).First ()})
			.ToList();		
	Dictionary<string,string> codebisKenvertori = new Dictionary<string,string>()
	{
		//	{"A16", "A16 (jj8007)"},
		//	{"A30DOUBLE", "A30 Double  (jj8012)"},
		//	{"B18", "B18  (jj8011)"},
		//	{"B28A", "B28A  (jj8013)"},
		//	{"B28B", "B28B(jj8015)"},
		//	{"914-155", "BC914-155"},
		//	{"34043", "34043T"},
		//	{"16174", "16174T"},
		//	{"A0142T", "A0142T5"},
		//	{"51.213", "51.2130"},
		//	{"40.83453", "40.8353"},
		//	{"40.83455", "40.8355"},
		//	{"16100", "16100I"},
		//	{"86420", "86420Z"},
		//	{"13481", "1348I"},
		//	{"110312", "1100312"},
	};
	
	Func<string,string,string> momeRefi = null;
	momeRefi = (kodi, dasakheleba) =>
	{
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


public static class EnumerableEx
{
	public static void DumpToExcel<T>(this IEnumerable<T> src, string wbName, Func<T,string> bySheets=null)
	{
		ExcelPackage pck = new ExcelPackage();
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