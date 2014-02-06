<Query Kind="Program">
  <Reference>D:\Dev\ExcelReader\ExcelReader.XlsReader\bin\Debug\ExcelReader.dll</Reference>
  <Reference>D:\Dev\ExcelReader\ExcelReader.XlsReader\bin\Debug\ExcelReader.XlsReader.dll</Reference>
  <Reference>D:\Dev\ExcelReader\ExcelReader.XlsxReader\bin\Debug\ExcelReader.XlsxReader.dll</Reference>
  <Reference>D:\Dev\Nashti.Core\Nashti.Core\bin\Debug\Nashti.Core.dll</Reference>
  <Reference>D:\RavenDB-Build-2700\Client\Raven.Abstractions.dll</Reference>
  <Reference>D:\RavenDB-Build-2700\Client\Raven.Client.Lightweight.dll</Reference>
  <NuGetReference>EPPlus</NuGetReference>
  <NuGetReference>ServiceStack.Text</NuGetReference>
  <Namespace>ExcelReader</Namespace>
  <Namespace>Nashti.Core</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

public class Partition
{	
	readonly string @ref;
	int i = 1;
	readonly Dictionary<string,Mdebareoba> mdebareobebi =  new Dictionary<string,Mdebareoba>();
	public Partition(string @ref)
	{
		if(string.IsNullOrWhiteSpace(@ref)) throw new ArgumentException("@ref");
		this.@ref = @ref;
	}
	
	public void Sheavse(string mdebareobaId, decimal tankha_, double raodenoba_, string partiaId)
	{
		var tankha = new Tankha(tankha_);
		var raodenoba = new Raodenoba(raodenoba_);
		var girebuleba = new Fasi(tankha, raodenoba);
		var partia = new Partia(@ref+ "-p" + i++, girebuleba);
		var futa = new Futa(partia, raodenoba);
		MomeMdebareoba(mdebareobaId).Sheavse(girebuleba, futa);
	}
	
	IEnumerable<Modzraoba.MimdinareNashti> Nashtebi()
	{
		return (
				from kv in mdebareobebi
				let nashti  = kv.Value.MimdinareNashti()
				select new Modzraoba.MimdinareNashti(kv.Value.Id, nashti == null ? 0 : nashti.Raodenoba.Mnishvneloba)
			);
	}
	public Modzraoba.Chanaceri Gadaitane(string saidanId, string sadId, double raodenoba_)
	{
		var saidan = MomeMdebareoba(saidanId);
		var gamokofa = saidan.Gamokavi(new Raodenoba(raodenoba_));
		if(gamokofa == null) {
			return new Modzraoba.Chanaceri(saidanId, sadId, @ref, 0, 0, nashtebi:Nashtebi());
		}
		var sad = MomeMdebareoba(sadId);
		saidan.Moakeli(gamokofa);
		sad.Sheavse(gamokofa.Item1, gamokofa.Item2.ToArray());
		
		var partiebi = gamokofa.Item2
			.Select (x => new {PartiaId=x.Partia.Mnishvneloba, Raodenoba=x.Raodenoba.Mnishvneloba, Tankha=(x.Partia.Fasi * x.Raodenoba).Daamrgvale(3).Mnishvneloba})
			.GroupBy (x => x.PartiaId)
			.Select (g => new Modzraoba.Partia(g.Key,g.Sum (x => x.Raodenoba),g.Sum (x => x.Tankha)));

		return new Modzraoba.Chanaceri(saidanId, sadId, @ref, gamokofa.Item1.Raodenoba.Mnishvneloba, gamokofa.Item1.Tankha.Mnishvneloba, partiebi, Nashtebi());
	}
	
	Mdebareoba MomeMdebareoba(string mdebareobaKey)
	{
		Mdebareoba mdebareoba;
		if(mdebareobebi.TryGetValue(mdebareobaKey, out mdebareoba)) return mdebareoba;
		mdebareoba = new Mdebareoba(mdebareobaKey);
		mdebareobebi[mdebareobaKey] = mdebareoba;
		return mdebareoba;
	}
}

public class Mdebareoba
{
	Fasi girebuleba;
	LinkedList<Futa> fifo = new LinkedList<Futa>();
	public string Id {get;private set;}
	public Mdebareoba(string id)
	{
		Id = id;
	}
	public Fasi MimdinareNashti()
	{
		return girebuleba;
	}
	
	public void Sheavse(Fasi girebuleba, params Futa[] futebi)
	{
		this.girebuleba = this.girebuleba != null ? this.girebuleba.SheatsonGaasashuale(girebuleba) : girebuleba;
		foreach (var f in futebi)
		{
			fifo.AddLast(f);
		}
	}

	public Tuple<Fasi,List<Futa>> Gamokavi(Raodenoba raodenoba)
	{
		if(girebuleba == null) return null;
		if(raodenoba == Raodenoba.Nuli) return null;
		return Tuple.Create(GamokaviSheconili(raodenoba), GamokaviFifo(fifo.First, raodenoba).ToList());
	}
	
	Fasi GamokaviSheconili(Raodenoba raodenoba)
	{
		if(girebuleba.Raodenoba <= raodenoba) return girebuleba;
		var tankha = (girebuleba * raodenoba).Daamrgvale(3);
		return new Fasi(tankha < girebuleba.Tankha ? tankha : girebuleba.Tankha, raodenoba);
	}

	IEnumerable<Futa> GamokaviFifo(LinkedListNode<Futa> futaNode, Raodenoba raodenoba)
	{
		if(futaNode == null || raodenoba == Raodenoba.Nuli)
		{
			yield break;
		} 
		var futa = futaNode.Value;
		if(raodenoba > futa.Raodenoba)
		{
			yield return futa;
			foreach (var f in GamokaviFifo(futaNode.Next, raodenoba - futa.Raodenoba))
				yield return f;
		}
		else
		{
			yield return new Futa(futa.Partia, raodenoba);
		}
	}
	
	public void Moakeli(Tuple<Fasi,List<Futa>> g)
	{
		MoakeliSheconili(g.Item1);
		MoakeliFifo(g.Item2);
	}
	
	void MoakeliSheconili(Fasi g)
	{
		if(this.girebuleba == g)
		{
			this.girebuleba = null;
		}
		else
		{
			 this.girebuleba = this.girebuleba.GamotsonGaasashuale(g);
		}
	}
	
	void MoakeliFifo(List<Futa> futebi)
	{
		foreach (var mosaklebi in futebi)
		{
			var arsebuli = fifo.First.Value;
			fifo.RemoveFirst();
			if(arsebuli.Partia != mosaklebi.Partia) throw new InvalidOperationException();
			if(arsebuli.Raodenoba > mosaklebi.Raodenoba)
			{
				fifo.AddFirst(new Futa(arsebuli.Partia, arsebuli.Raodenoba - mosaklebi.Raodenoba));
			}
		}
	}
}


List<DokumentiDaModzraoba> Modzraobebi(IEnumerable<Dokumenti> src){
	Func<string,string,string> momeRefi = DaamzadeMomeRefi();
	var dokumentebi = src.OrderBy (i => i, Dokumenti.DokumentiComparer).ToList();
//	var dokumentebi = DokumentiEx.ParseDokumentebi(
//		"migeba			1.1.2014                		[ a 2 2.1, b 2 3.2 ]",
//		"migeba			1.1.2014                		[ a 2 2.2 ]",
//		"gadaadgileba	1.1.2014	sackobi		batumi	[ a 3, b 5 ]"
//	
//	
//	
//	)
//	.OrderBy (x => x, Dokumenti.DokumentiComparer)
//	.ToList();

	var partitions = new ConcurrentDictionary<string,Partition>();
	return (
		from x in dokumentebi.Select ((d,i) => new {d, i=++i})
		let doc = (dynamic)x.d
		let chanacerebi = ((IEnumerable<dynamic>)doc.Chanacerebi)
									.Select((c,i) => 
									{
										var saidanSad = x.d.SaidanSad();
										
										var partition = partitions.GetOrAdd(momeRefi((string)c.Ref, (string)c.Dasakheleba), @ref => new Partition(@ref));
										
										if(doc.GetType() == typeof(Migeba) && c.Raodenoba > 0)
										{	
											var tankha = (decimal)c.Jami;
											partition.Sheavse(saidanSad.Item1, Math.Round(tankha, 3), (double)c.Raodenoba, x.i + "-" + ++i);
										}
									
										var saidan = c.Raodenoba >= 0 ? saidanSad.Item1 : saidanSad.Item2;
										var sad = c.Raodenoba >= 0 ? saidanSad.Item2 : saidanSad.Item1;
										var raodenoba = Math.Abs((double)c.Raodenoba);
										return partition.Gadaitane(saidan, sad, raodenoba);
									})
		select new DokumentiDaModzraoba{Dokumenti=x.d, Modzraoba=new Modzraoba(x.i.ToString(), doc.Dro, chanacerebi)}
	)
	.ToList();
}

void BananasSakonlisReportebi(List<DokumentiDaModzraoba> modzraobebi)
{
	var bananasPartiebi = modzraobebi
		.Where (d => d.Dokumenti.Key() == @"d:\anvol\invoicebi\pakinglists\banana-unimart zeddebuli 26.02.2013.xls\Grid")
		.SelectMany (x=>x.Modzraoba.Chanacerebi.SelectMany (c => c.PartiebiFiFo) )
		.Select (x => x.Id).ToList();	
	
	(
		from m in modzraobebi
		where m.Dokumenti.GetType() == typeof(Gakidva) || m.Dokumenti.GetType() == typeof(Cheki)
		from c in m.Chanacerebi()
		from p in c.GamokofiliPartiebiFiFo
		where bananasPartiebi.Contains(p.Id)
		let tankha = (decimal)(p.Raodenoba / c.GamokofiliRaodenoba) * Math.Abs(c.Jami)
		from t in new []{
							new {c.Ra, Mdebareoba=c.Saidan, Raodenoba = p.Raodenoba * -1, Tankha = tankha * -1, m.Modzraoba.Rodis },
							new {c.Ra, Mdebareoba=c.Sad,    Raodenoba = p.Raodenoba ,     Tankha = tankha ,     m.Modzraoba.Rodis },
						}
		
		group t by new {t.Mdebareoba, t.Ra} into g
		select new {g.Key.Mdebareoba, g.Key.Ra, Raodenoba = g.Sum (x => x.Raodenoba), Tankha=g.Sum (x => x.Tankha)}
	).DumpToExcel("BananasRealizacia");
	return;
	(
		from m in modzraobebi.Select (x => x.Modzraoba)
		from c in m.Chanacerebi
		from p in c.PartiebiFiFo
		where bananasPartiebi.Contains(p.Id)
		from t in new []{
							new {c.Ra, Mdebareoba=c.Saidan, Raodenoba = p.Raodenoba * -1, Tankha = p.Tankha * -1, m.Rodis },
							new {c.Ra, Mdebareoba=c.Sad,    Raodenoba = p.Raodenoba ,     Tankha = p.Tankha ,     m.Rodis },
						}
		group t by new {t.Mdebareoba, t.Ra} into g
		select new {g.Key.Mdebareoba, g.Key.Ra, Raodenoba = g.Sum (x => x.Raodenoba), Tankha=g.Sum (x => x.Tankha)}
	).DumpToExcel("bananasNashti");

}

void RealizaciebisKorektireba()
{
	var chekebi = FromCache<Cheki>("ch03").ToList();
	var gakidvebi = ExcelisFailebi().OfType<Gakidva>().ToList();
	
//	chekebi.Where (d => d.Chanacerebi.Any (c => c.Ref.StartsWith("14657"))).Where (d => d.Dro.Month==9).Dump();
//	return;
	var chemiRealizacia = (
			from d in gakidvebi
			let jami = d.Chanacerebi.Sum(x => x.Jami)
			select new {d.Dro, d.Mdebareoba, Nagdi=0m,Sasachukre=0m, Sakredito=0m, Gare=jami, Sul=jami}
		)
		.Concat(
			from d in chekebi
			let Sul = d.Chanacerebi.Sum (x => x.FasdaklebuliJami)
			let gadakhdebi = d.gadakhdebi.ToDictionary (x => x.forma, x=>x.tankha)
			let Sakredito = gadakhdebi.ContainsKey("sakreditoBarati") ? gadakhdebi["sakreditoBarati"] : 0m
			let Sasachukre = gadakhdebi.ContainsKey("sasachukreBarati") ? gadakhdebi["sasachukreBarati"] : 0m
			let Khurda = gadakhdebi.ContainsKey("khurda") ? gadakhdebi["khurda"] : 0m
			let Nagdi = (gadakhdebi.ContainsKey("nagdi") ? gadakhdebi["nagdi"] : 0m) + Khurda
			select new {d.Dro, d.Mdebareoba, Nagdi, Sasachukre, Sakredito, Gare=0m, Sul}
		)
		.GroupBy (x => x.Dro.Date)
		
		.Select(g => new {
				g.Key,
				Nagdi=g.Sum (x => x.Nagdi),
				Sasachukre=g.Sum (x => x.Sasachukre),
				Sakredito=g.Sum (x => x.Sakredito),
				Gare=g.Sum (x => x.Gare),
				Sul=g.Sum (x => x.Sul),
			})
		.ToDictionary(x => x.Key);
		
	var irinasRealizacia = (
			from wb in new []{MakeExcelImporter().Import(@"D:\Anvol\Realizacia Zetebi.xlsx")}
			from sheet in wb.Sheets
			from row in sheet.Rows
			select new {
				Tarigi = row.Cells[0].AsDateTime(),
				Salaro=row.Cells[1].AsDecimal(),
				Terminali=row.Cells[2].AsDecimal(),
				Skhva=row.Cells[3].AsDecimal(),
				Sul=row.Cells[4].AsDecimal()
				}
			into row2
			where row2.Tarigi.HasValue && row2.Sul.HasValue
			select new {
				Tarigi=row2.Tarigi.Value,
				Salaro=row2.Salaro.HasValue ? row2.Salaro.Value : 0, 
				Terminali=row2.Terminali.HasValue ? row2.Terminali.Value : 0, 
				Skhva = row2.Skhva.HasValue ? row2.Skhva.Value : 0, 
				Sul=row2.Sul.Value
			}
		)
		.GroupBy (x => x.Tarigi.Date)
		.Select (g => new {g.Key,Salaro=g.Sum (x => x.Salaro),Terminali=g.Sum (x => x.Terminali),Skhva=g.Sum (x => x.Skhva),Sul=g.Sum (x => x.Sul)})
		.ToDictionary(x=>x.Key);
	Func<Func<object>, DumpContainer>  expand = lazy => {
		var cont = new LINQPad.DumpContainer();
		cont.Content = new LINQPad.Hyperlinq(()=>{cont.Content=lazy();},"istoria");
		return cont;
	};
	(
		from day in chemiRealizacia.Keys.Concat(irinasRealizacia.Keys).Distinct()
		let chemi = chemiRealizacia.ContainsKey(day) ? chemiRealizacia[day] : null
		let irina = irinasRealizacia.ContainsKey(day) ? irinasRealizacia[day] : null
		let zSul = irina != null ? irina.Sul : 0m
		let pSul = chemi != null ? chemi.Sul : 0m
	
		select new {
			Tarigi=day,
			zSalaro = irina != null ? irina.Salaro : 0m,
			zTerminali = irina != null ? irina.Terminali : 0m,
			zSkhva = irina != null ? irina.Skhva : 0m,
			zSul,
			pNagdi = chemi != null ? chemi.Nagdi : 0m,
			pSakredito = chemi != null ? chemi.Sakredito : 0m,
			pSasachukre = chemi != null ? chemi.Sasachukre : 0m,
			pGare = chemi != null ? chemi.Gare : 0m,
			pSul,
			Sxvaoba = zSul - pSul,
//			Detalebi = expand(()=>new{
//						GareGakidvebi=gakidvebi.Where (d => d.Dro.Date == day).Select (d => new {d.Key,d.Mdebareoba,Tankha=d.Chanacerebi.Sum (c => c.Jami),d.Chanacerebi}),
//						Chekebi=chekebi.Where (c => c.Dro.Date==day).GroupBy (d => d.Mdebareoba).Select (g => new{g.Key,Tankha=g.Sum (x => x.FasdaklebuliGadasakhdeli)})
//						})
			}
	)
	.OrderBy (x => x.Tarigi)
//	.Where(x=>(x.Tarigi.Year*100 + x.Tarigi.Month)==201312)
//	.Dump()
//	.DumpToExcel("realizaciisShedareba", x => (x.Tarigi.Year*100 + x.Tarigi.Month).ToString())
	.DumpToExcel("realizaciisShedareba")
	;

}

void Main()
{


ExcelisFailebi()
	.GroupBy (x => x.GetType())
	.Select (g => new {g.Key, Raodenoba=g.Count ()})
	.Dump()
	;
return;


































return;
	Func<string,Func<object>, DumpContainer>  expand = (label, lazy) => {
			var cont = new LINQPad.DumpContainer();
			cont.Content = new LINQPad.Hyperlinq(()=>{cont.Content=lazy();},label);
			return cont;
		};
		
		
		
		
		
		
		
		
	var excelisFailebi = ExcelisFailebi().ToList();
	var mr = DaamzadeMomeRefi();
	
	var docs = excelisFailebi.Where (x => x.GetType() != typeof(PakingListi) && x.GetType() != typeof(Gadaadgileba))
									.Concat(Migebebi(excelisFailebi.OfType<PakingListi>()))
									.Concat(FromCache<Cheki>("ch04"));
								
	var modzraobebi = Modzraobebi(docs);
	
	var korektirebuliGadaadgilebebi = (
		from m in modzraobebi
		from c in m.Chanacerebi()
		where c.Saidan.StartsWith("sackobebi/") && !c.Saidan.StartsWith("sackobebi/sackobi")
		where c.Sad.StartsWith("klientebi/")
		where c.Raodenoba != c.GamokofiliRaodenoba 
		group c by new {Dro=c.Dro.Date,Sad=c.Saidan.Substring(c.Saidan.IndexOf("/") + 1)} into g
		select new Gadaadgileba{
				Dro= g.Key.Dro,
				Key="autokorektireba"+g.Key.Dro.ToString("yyyyMMdd"),
				Saidan="sackobi",
				Sad = g.Key.Sad, 
				Chanacerebi=(
						from c in g
						group c by new {c.Ref,c.Dasakheleba} into gra
						select new Gadaadgileba.Chanaceri{
							Ref = gra.Key.Ref,
							Dasakheleba=gra.Key.Dasakheleba,
							Raodenoba=gra.Sum (x => x.Raodenoba-x.GamokofiliRaodenoba),
							
							}
					).ToList()
				}
	
	).ToList();
	
	modzraobebi = Modzraobebi(docs.Concat(korektirebuliGadaadgilebebi)).ToList();
	var modzraobebiDict = modzraobebi.Where (m => !m.Dokumenti.Key().StartsWith("autokorektireba"))
									 .SelectMany (m => m.Chanacerebi())
									 .GroupBy (m => m.Ra)
									 .ToDictionary (m => m.Key);
									 
	var refebi = excelisFailebi.OfType<PakingListi>()
				.SelectMany (pl => pl.Chanacerebi)
				.GroupBy (x => new {x.Ref, Ref2=mr(x.Ref,x.Dasakheleba)})
				.Select (g => new {
							Ra = g.Key.Ref2,
							Ref = expand(g.Key.Ref, ()=>modzraobebiDict.ContainsKey(g.Key.Ref)  ?modzraobebiDict[g.Key.Ref] :null),
							Ref2= expand(g.Key.Ref2,()=>modzraobebiDict.ContainsKey(g.Key.Ref2) ?modzraobebiDict[g.Key.Ref2]:null),
							Migebebi = expand("migebebi", ()=>g.ToList()),
							Dasakheleba=string.Join("",g.Select (x => x.Dasakheleba).Where (x => !string.IsNullOrWhiteSpace(x)).Take(1)),
				})
				.ToList()
				;
	
	
	
	
	(
		from m in modzraobebi
		from c in m.Chanacerebi()
		where c.Saidan.StartsWith("sackobebi/")
		where c.Sad.StartsWith("klientebi/")
		where c.Raodenoba != c.GamokofiliRaodenoba 
		group c by c.Ra into g
		select new {g.Key, 
			Raod = g.Sum (x => x.Raodenoba-x.GamokofiliRaodenoba ), 
			a = expand("istoria", () => modzraobebi.SelectMany (d => d.Chanacerebi()
																		.Where (m => m.Ra==g.Key)
																		.Select (m => new {Id=d.Dokumenti.Key(),m.Dro,m.Saidan,m.Sad,m.Ref,m.Ra,m.Dasakheleba,m.Raodenoba,m.GamokofiliRaodenoba,Sxvaoba=m.Raodenoba-m.GamokofiliRaodenoba})
																		.Where (m => !m.Id.StartsWith("autokorektireba"))
																)
					),
			b = expand("kodebi", () => refebi.Where(r => r.Ra.Contains(g.Key) || g.Key.Contains(r.Ra))),
			c = expand("refisCvlilebebi", () => StreamDocs("http://office.anvol.ge:8080/", "movlenebi", "Anvol").Where (x => x.Type == "MienichaRefi").Where (x => x.Ref==g.Key)),
			}
	)
	.OrderBy (x => x.Key)
//	.Dump(6)
//	.DumpToExcel("zedmetadGakiduliKodebi")
	;
	
	modzraobebi = Modzraobebi(	docs
							.Concat(korektirebuliGadaadgilebebi)
							.Concat(excelisFailebi.OfType<Gadaadgileba>().Select (d => { d.Dro = DateTime.Today.AddDays(1); return d; }))
				).ToList();

	var refDict = refebi.GroupBy (r => r.Ra).ToDictionary (r => r.Key,r=>r.Select (x => x.Dasakheleba).First ());
	
	var sakartvelosPartiebi = new HashSet<string>(modzraobebi
					.Where (m => m.Dokumenti.GetType() == typeof(Migeba) && 
								((Migeba)m.Dokumenti).Chanacerebi.Any (c => c.Shenishvna.Contains("shps"))
							)
					.SelectMany (m => m.Modzraoba.Chanacerebi.SelectMany (c => c.PartiebiFiFo).Select (p => p.Id))
					);
	var estonuriPartiebi = new HashSet<string>(modzraobebi
					.Where (m => m.Dokumenti.GetType() == typeof(Migeba))
					.SelectMany (m => m.Chanacerebi())
					.Where (c => c.Dasakheleba.ToUpper().StartsWith("MAT"))
					.SelectMany (c => c.GamokofiliPartiebiFiFo.Select (x => x.Id))
					);
					

	
//	(
//		from m in modzraobebi
//	 	from c in m.Chanacerebi()
//		from p in c.GamokofiliPartiebiFiFo
//		let momcodebeli = sakartvelosPartiebi.Contains(p.Id) 
//									? "Georgia"
//									: estonuriPartiebi.Contains(p.Id) 
//											? "Estonia"
//											: null
//		where momcodebeli != null
//		let koef = (decimal)(p.Raodenoba / c.Raodenoba)
//		let Dasakheleba = refDict.ContainsKey(c.Ra) ? refDict[c.Ra] : "n/a"
//		let Kvebrendi = string.Join(" ", Dasakheleba.Split(' ').Take(2))
//		from t in new [] {
//					new {mdebareoba = c.Saidan.Substring(0, c.Saidan.IndexOf('/')), Kvebrendi, momcodebeli, c.Ra, Dasakheleba, Tankha = c.GamokofiliTvitgirebuleba * koef * -1m },
//					new {mdebareoba = c.Sad.Substring(0, c.Sad.IndexOf('/')), Kvebrendi, momcodebeli, c.Ra, Dasakheleba, Tankha = c.GamokofiliTvitgirebuleba * koef },
//				}					
//		select t
//	).DumpToExcel("matelisreporti");
//	return;
	(
		from m in modzraobebi
	 	from c in m.Chanacerebi()
		where c.Jami != 0m
		where c.Sad.StartsWith("klien") || c.Saidan.StartsWith("klien")
		from p in c.GamokofiliPartiebiFiFo
		let momcodebeli = sakartvelosPartiebi.Contains(p.Id) 
									? "Georgia"
									: estonuriPartiebi.Contains(p.Id) 
											? "Estonia"
											: null
		where momcodebeli != null
		
		let koef = (decimal)(p.Raodenoba / c.Raodenoba)
		let Dasakheleba = refDict.ContainsKey(c.Ra) ? refDict[c.Ra] : "n/a"
		let Kvebrendi = string.Join(" ", Dasakheleba.Split(' ').Take(2))
		from t in new [] {
					new {mdebareoba = c.Saidan.Substring(0, c.Saidan.IndexOf('/')), Kvebrendi, momcodebeli, c.Ra, Dasakheleba, Tankha = Math.Abs(c.Jami) * koef * -1m },
					new {mdebareoba = c.Sad.Substring(0, c.Sad.IndexOf('/')), Kvebrendi, momcodebeli, c.Ra, Dasakheleba, Tankha = Math.Abs(c.Jami) * koef },
				}					
		select t
	).DumpToExcel("matelisreportiRealizacia");
	
	return;
	
	(
		from m in modzraobebi.Select (mo => mo.Modzraoba)
	 	from c in m.Chanacerebi
	 	from t in new []{
	 					new {Mdebareoba=c.Saidan, c.Ra, Raodenoba=c.Raodenoba*-1, Tankha=c.Tankha*-1},
						new {Mdebareoba=c.Sad, c.Ra, c.Raodenoba, c.Tankha}
					}
	 	group t by new {t.Mdebareoba,t.Ra} into g
	 	select new {g.Key.Ra, Dasakheleba = refDict.ContainsKey(g.Key.Ra) ? refDict[g.Key.Ra] : "n/a", g.Key.Mdebareoba, Raodenoba=g.Sum (x => x.Raodenoba), Tankha=g.Sum (x => x.Tankha)}
	)
	//.DumpToExcel("nashti_" + DateTime.Today.ToString("yyyy.MM.dd"))
	.Dump()
	;
//	(
//		from m in modzraobebi.Select (mo => mo.Modzraoba)
//	 	from c in m.Chanacerebi
//	 	from t in new []{
//	 					new {Mdebareoba=c.Saidan, c.Ra, m.Rodis, Raodenoba=c.Raodenoba*-1, Tankha=c.Tankha*-1},
//						new {Mdebareoba=c.Sad, c.Ra, m.Rodis, c.Raodenoba, c.Tankha}
//					}
//	 	group t by new {t.Mdebareoba,Periodi=t.Rodis.Year*100+t.Rodis.Month} into g
//	 	select new {g.Key.Periodi, g.Key.Mdebareoba, Raodenoba=g.Sum (x => x.Raodenoba), Tankha=g.Sum (x => x.Tankha)}
//	)
//	.DumpToExcel("tvitgirebuleba_" + DateTime.Today.ToString("yyyy.MM.dd"))
//	;			
	
	
	
	
//////REALIZACIA

FromCache<Cheki>("ch04")
	.SelectMany (cheki => 
		cheki.gadakhdebi.Select (g => new {Dro=cheki.Dro,cheki.Mdebareoba,g.forma,g.tankha,Shenishvna=(string)cheki.Nomeri})
				)
.Concat(
		excelisFailebi.OfType<Gakidva>()
			.Select (x => new {x.Dro, Mdebareoba=x.Mdebareoba, forma="n/a", tankha = x.Chanacerebi.Sum (c => c.Jami), Shenishvna=(string)x.Key})
	)
	.Select (g => new {Tve = g.Dro.Year * 100 + g.Dro.Month, Dge = g.Dro.Date, g.Dro, g.Mdebareoba, g.forma, Tankha = g.tankha, g.Shenishvna })
	.DumpToExcel("Realizacia_"+DateTime.Today.ToString("yyyy.MM.dd"));


	
	return;
	(
		from m in modzraobebi
		from c in m.Chanacerebi()
		group c by c.Ra into g
		from r in  (
						from c in g
						where c.Raodenoba != c.GamokofiliRaodenoba 
						where !c.Sad.StartsWith("sackobebi/") || !c.Saidan.StartsWith("sackobebi/")
						let mamentNashti = c.Nashtebi.FirstOrDefault (n => n.Mdebareoba == "sackobebi/sackobi")
						let nashti = mamentNashti != null ? mamentNashti.Raodenoba : 0.0
						where nashti >= c.Raodenoba - c.GamokofiliRaodenoba
						select c
					).Take(1)
		select r
	).Dump();
	
	return;
	
	return;
	
	var istoria = modzraobebi
		.SelectMany (m => m.Chanacerebi())
		.GroupBy (x => x.Ra)
		.ToDictionary (x => x.Key, g => g.Select (x => new { x.Dro,x.Saidan,x.Sad, x.Ref, x.Raodenoba, x.GamokofiliRaodenoba, Problema = x.Raodenoba-x.GamokofiliRaodenoba}))
		;
	
	
	
	modzraobebi
		.SelectMany (m => m.Chanacerebi())
		.Where (c => c.Raodenoba != c.GamokofiliRaodenoba)
		.Select (c => new {c.Dro,c.Saidan,c.Sad,c.Ra,c.Dasakheleba,c.Raodenoba,c.GamokofiliRaodenoba,Istoria=expand("istoria",()=>istoria[c.Ra])})
		.Take(10)
		.Dump();

	
	
	
	
	
	return;
	modzraobebi.SelectMany (m => m.Chanacerebi()).GroupBy (c => c.Ra).ToDictionary(x => x.Key, g=>g.Sum(x=>x.Raodenoba-x.GamokofiliRaodenoba)).Where (x => x.Value>0).Dump();

		
	return;
	(
		from m in modzraobebi.Select (mo => mo.Modzraoba)
		from c in m.Chanacerebi
		where c.Ra == "55505"
		from t in new []{
							new {c.Ra,Mdebareoba=c.Saidan, Raodenoba=c.Raodenoba*-1, Tankha=c.Tankha*-1},
							new {c.Ra,Mdebareoba=c.Sad, Raodenoba=c.Raodenoba, Tankha=c.Tankha},
					}
		select t
	)
	.Dump()
	;
		
return;


	return;

	
	
	
	
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