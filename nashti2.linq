<Query Kind="Program">
  <Reference Relative="..\Visual Studio 2013\Projects\Nashti.Core\Nashti.Core\bin\Debug\Nashti.Core.dll">&lt;MyDocuments&gt;\Visual Studio 2013\Projects\Nashti.Core\Nashti.Core\bin\Debug\Nashti.Core.dll</Reference>
  <NuGetReference>Ix-Main</NuGetReference>
  <NuGetReference>RavenDB.Client</NuGetReference>
  <Namespace>Nashti.Core</Namespace>
  <Namespace>Raven.Abstractions.Data</Namespace>
  <Namespace>Raven.Abstractions.Linq</Namespace>
  <Namespace>Raven.Client.Connection</Namespace>
  <Namespace>Raven.Client.Document</Namespace>
  <Namespace>Raven.Json.Linq</Namespace>
</Query>

IEnumerable<Refi> Refebi()
{
	return StoreEx.StreamQuery("http://25.88.224.201:8080/", "Refebi")
				 .Select(d => new Refi(d.Ref, ((IEnumerable<dynamic>)d.Eans).Cast<string>().ToList(), d.Dasakheleba, decimal.Parse(d.Fasi.ToString())))
				 .ToList();
}

public class Tankha
{
	public readonly decimal odenoba;
	public Tankha(decimal odenoba)
	{
		this.odenoba = odenoba;
	}
	public Tankha Gavamravlot(double odenoba)
	{
		return new Tankha(this.odenoba * (decimal)odenoba);
	}
	public TvitGirebuleba Gavamravlot(Raodenoba raodenoba)
	{
		return new TvitGirebuleba(this, raodenoba);
	}
	public Tankha Davumatot(Tankha tankha)
	{
		return new Tankha(this.odenoba + tankha.odenoba);
	}
}

public class Raodenoba
{
	public readonly double odenoba;
	public Raodenoba(double odenoba)
	{
		this.odenoba = odenoba;
	}
	public double Gavkot(Raodenoba raodenoba)
	{
		return this.odenoba / raodenoba.odenoba;
	}
	public Raodenoba Davumatot(Raodenoba raodenoba)
	{
		return new Raodenoba(this.odenoba + raodenoba.odenoba);
	}
}

public class TvitGirebuleba
{
	public readonly Tankha tankha; public readonly Raodenoba raodenoba;
	public TvitGirebuleba(Tankha tankha, Raodenoba raodenoba)
	{
		this.tankha = tankha; this.raodenoba = raodenoba;
	}
	public Tankha Gavkot(Raodenoba raodenoba)
	{
		return tankha.Gavamravlot(raodenoba.Gavkot(this.raodenoba));
	}
	public TvitGirebuleba Davumatot(TvitGirebuleba tvitgirebuleba)
	{
		return new TvitGirebuleba(this.tankha.Davumatot(tvitgirebuleba.tankha), this.raodenoba.Davumatot(tvitgirebuleba.raodenoba));
	}
}

void Main()
{
	Func<object,dynamic> rdo = o => new DynamicJsonObject(RavenJObject.FromObject(o));

	var results = (
		from d in Sheskidvebi()
		let sulMoculoba = (double)Enumerable.Sum(Enumerable.Where((IEnumerable<dynamic>)d.Chanacerebi, x => x.Moculoba != null), x => (double)x.Moculoba)
		from c in (IEnumerable<dynamic>)d.Chanacerebi
		let Tankha = (decimal)((c.Moculoba != null ? d.Kharji * c.Moculoba / sulMoculoba : 0.0) + c.Jami*d.Kursi)
		select new { 
			Ref = (string)c.MomcodeblisRefi,
			Movlenebi = new []{
				rdo(new {
					Tipi = "Sheskidva", Dro = (DateTimeOffset)d.Dro, Raodenoba = c.Raodenoba, Tankha, Rigi = 1, DocId = d.__document_id
				})
			},
			Gatarebebi = new object[0],
		}
	).Concat(
		from d in Gadatanebi()
		from c in (IEnumerable<dynamic>)d.Chanacerebi
		select new { 
			Ref = (string)c.Kodi,
			Movlenebi = new []{
				rdo(new {
					Tipi="Gadaadgileba", Saidan = (string)d.Saidan, Sad = (string)d.Sad, Dro = (DateTimeOffset)d.Dro, Raodenoba = (double)c.Raodenoba, Rigi = 2, DocId = d.__document_id
				})
			},
			Gatarebebi = new object[0],
		}
	).Concat(
		from d in Gakidvebi()
		from c in (IEnumerable<dynamic>)d.Chanacerebi
		select new { 
			Ref = (string)c.Kodi,
			Movlenebi = new []{
				rdo(new {
					Tipi="Gakidva", d.Mdebareoba, Dro = (DateTimeOffset)d.Dro, Raodenoba = (double)c.Raodenoba, Rigi = 3, DocId = d.__document_id
				})
			},
			Gatarebebi = new object[0],
		}
	).Concat(
		from d in Chekebi()
		from c in (IEnumerable<dynamic>)d.produktebi
		select new { 
			Ref = (string)c.@ref,
			Movlenebi = new []{
				rdo(new {
					Tipi="Cheki", d.mdebareoba, 
					Dro = (DateTimeOffset)d.chekisGamocerisDro, 
					Raodenoba = (double)c.raodenoba, 
					c.klientisBaratisNomeri,
					c.fasdaklebuliJami,
					Rigi = 4, 
					DocId = d.__document_id
				})
			},
			Gatarebebi = new object[0],
		}
	)
	.ToList();
	
	var gatarebebi = (
		from r in results
		group r by r.Ref into g
		let Movlenebi = g.SelectMany(x=>x.Movlenebi).OrderBy(x=>x.Dro).ThenBy(x => x.Rigi).ToList()
					
		select new {
			Ref = g.Key,
			Movlenebi,
			Gatarebebi=Gamtarebeli.Gaatare(Movlenebi),
		}
	).ToList();
	
	gatarebebi
	.Where (m => m.Ref == "13010000")
//	.Dump()
	.SelectMany(ri => ri.Gatarebebi.SelectMany (ga => ((IEnumerable<dynamic>)ga.Tranzakcia).Select(tr=>new {
																ri.Ref,ga.Dro,tr.Angarishi,tr.Debeti,tr.Krediti
														})
												)
				)
	.GroupBy (x => new {x.Ref,x.Angarishi})
	.Select (g => new {g.Key.Ref,g.Key.Angarishi,Debeti=g.Sum (x => (decimal)x.Debeti),Krediti=g.Sum (x => (decimal)x.Krediti)})
	.Dump()
	;
	
}

public static class Gamtarebeli{
	
	public class State
	{
		public IEnumerable<object> Sheskidva(dynamic m)
		{
			return  tranzakciaT("momcodebeli", "mdebareoba/sackobi", (decimal) m.Tankha)
				.Concat(tranzakciaR("momcodebeli", "mdebareoba/sackobi", (decimal) m.Raodenoba));
		}
		public IEnumerable<object> Gadaadgileba(dynamic m)
		{
			string saidan = ("mdebareoba/" + m.Saidan).ToLowerInvariant();
			string sad = ("mdebareoba/" + m.Sad).ToLowerInvariant();
			decimal gadasataniRaodenoba = (decimal)m.Raodenoba;
			return  tranzakciaT(saidan, sad, datvaleGirebuleba(saidan, gadasataniRaodenoba))
			.Concat(tranzakciaR(saidan, sad, gadasataniRaodenoba));
		}
		public IEnumerable<object> Gakidva(dynamic m)
		{
			return new object[]{};
		}
		public IEnumerable<object> Cheki(dynamic m)
		{
			string kapitaliAng = ("kapitali/" + m.mdebareoba).ToLowerInvariant();
			string klientisAng = (m.klientisBaratisNomeri == null ? "klienti": "klienti/" + m.klientisBaratisNomeri).ToLowerInvariant();
			string sackobiAng = ("mdebareoba/" + m.mdebareoba).ToLowerInvariant();
			decimal gakiduliRaodenoba = (decimal)m.Raodenoba;
			return	tranzakciaT(sackobiAng, kapitaliAng, datvaleGirebuleba(sackobiAng, gakiduliRaodenoba))
			.Concat(tranzakciaR(sackobiAng, kapitaliAng, gakiduliRaodenoba))
			.Concat(tranzakciaT(kapitaliAng, klientisAng, (decimal)m.fasdaklebuliJami))
			.Concat(tranzakciaR(kapitaliAng, klientisAng, gakiduliRaodenoba))
			;
		}
		
		Dictionary<string, decimal> dicTg = new Dictionary<string, decimal>();
		
		decimal get (string x)
		{
			var tg = default(decimal);
			if(dicTg.TryGetValue(x, out tg))
			{
				return tg;
			}
			return 0m;
		}
		decimal getT (string x)
		{ 
			return get("t/" + x);
		}
		decimal getR (string x) { return get("r/" + x); }
		void set(string x, decimal tg)
		{
			dicTg[x] = tg;
		}
		void setT (string x, decimal tg) {set("t/"+x, tg);}
		void setR (string x, decimal tg) {set("r/"+x, tg);}
		
		IEnumerable<object> tranzakcia  (string saidan, string sad, decimal odenoba)  {
			var saidanSackisi = get(saidan);
			var sadSackisi = get(sad);
			var saidanSaboloo = saidanSackisi - odenoba;
			var sadSaboloo = sadSackisi + odenoba;
			set(saidan, saidanSaboloo);
			set(sad, sadSaboloo);
			return new []{
				new { Angarishi = saidan, Debeti = 0.0m,    Krediti = odenoba, Balansi = saidanSaboloo},
				new { Angarishi = sad,    Debeti = odenoba, Krediti = 0.0m,    Balansi = sadSaboloo},
			};
		}
		IEnumerable<object> tranzakciaT (string saidan, string sad, decimal odenoba)
		{ 
			return tranzakcia("t/" + saidan, "t/" + sad, odenoba);
		}
		IEnumerable<object> tranzakciaR (string saidan, string sad, decimal odenoba)  
		{ 
			return tranzakcia("r/" + saidan, "r/" + sad, odenoba);
		}
		
		decimal datvaleGirebuleba (string sackobi, decimal gadasataniRaodenoba) 
		{
			var saidanTankha = getT(sackobi);
			var saidanRaodenoba = getR(sackobi);
			if(saidanRaodenoba > gadasataniRaodenoba){
					return Math.Min(saidanTankha, Math.Round(saidanTankha / saidanRaodenoba * gadasataniRaodenoba, 2));
			} 
			return saidanTankha;
		}
	}
	
	public static IEnumerable<dynamic> Gaatare(IEnumerable<dynamic> movlenebi)
	{
		var state = new State();
		var dic = new Dictionary<string, Func<dynamic, IEnumerable<dynamic>>>{
			{"Sheskidva", state.Sheskidva},
			{"Gadaadgileba", state.Gadaadgileba},
			{"Gakidva", state.Gakidva},
			{"Cheki", state.Cheki},
		};
		foreach (var m in movlenebi)
		{
			yield return new {m.Dro,m.DocId,Tranzakcia=dic[m.Tipi](m)};
		}
	}
}

List<dynamic> docs = Util.Cache(() => StoreEx.StreamDocs("http://office.anvol.ge:8080", null, "Dokumentebi").ToList(), "docs");

IEnumerable<dynamic> Chekebi(){
	
	return docs.Where (d => d["@metadata"]["Raven-Entity-Name"] == "cheki");
}
IEnumerable<dynamic> Gakidvebi(){
	return docs.Where (d => d["@metadata"]["Raven-Entity-Name"] == "Gakidvebi");
}
IEnumerable<dynamic> Gadatanebi(){
	return docs.Where (d => d["@metadata"]["Raven-Entity-Name"] == "Gadatanebi");
}
IEnumerable<dynamic> Sheskidvebi(){
	return docs.Where (d => d["@metadata"]["Raven-Entity-Name"] == "Sheskidvebi");
}

public static class StoreEx{
	
	public static IEnumerable<dynamic> StreamDocs(string url, string doc, string database="Anvol"){
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
	public static IEnumerable<dynamic> StreamQuery(string url, string index, string database="Anvol"){
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
// Define other methods and classes here



public static class Ex
{
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