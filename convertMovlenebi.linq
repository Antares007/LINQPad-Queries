<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.Tasks.dll</Reference>
  <NuGetReference>RavenDB.Client</NuGetReference>
  <Namespace>Raven.Abstractions.Data</Namespace>
  <Namespace>Raven.Abstractions.Linq</Namespace>
  <Namespace>Raven.Client.Document</Namespace>
  <Namespace>Raven.Json.Linq</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void CopyDocs(string sourceUrl,string sourceDb,string targetUrl,string targetDb, Func<IEnumerable<dynamic>,IEnumerable<dynamic>> map, string syncid=null){
	StoreEx.Initialize(source => {
		StoreEx.Initialize(target => {
			var syncDocId = "syncId/" + Uri.EscapeDataString(sourceUrl) + "/" + Uri.EscapeDataString(sourceDb);
			if(syncid!=null);
				syncDocId += "/" + syncid;
			var etag = Etag.Empty;
			var sincDoc = target.DatabaseCommands.Get(syncDocId);
			if(sincDoc != null) {
				etag =  Etag.Parse(sincDoc.DataAsJson.Value<string>("etag"));
			}
			var docs = source.StreamDocs(sourceDb, etag).ToList();
			if(docs.Count>0)
				etag = Etag.Parse(docs[docs.Count-1]["@metadata"]["@etag"]);
			using(var bi = target.BulkInsert(targetDb, new BulkInsertOptions{CheckForUpdates=true, BatchSize=2048}))
			{
				foreach (var d in map(docs))
				{
					bi.Store((RavenJObject)d.Inner, (RavenJObject)d["@metadata"].Inner, (string)d["@metadata"]["@id"]);
				};
			}
			if(docs.Count>0)
			{
				var syncmeta = new RavenJObject();
				syncmeta.Add("Raven-Entity-Name", "SyncIds");
				target.DatabaseCommands.Put(syncDocId, null, RavenJObject.FromObject(new {etag=etag}), syncmeta);
			}
		}, targetUrl, targetDb);
	}, sourceUrl, sourceDb);
}

void SheavseChekebi(string sourceUrl,string sourceDb,string targetUrl,string targetDb){
CopyDocs(sourceUrl, sourceDb, targetUrl, targetDb,
	 ds => {
		var entities = new HashSet<string>(new[] {"cheki", "movlenebi"});
		var damatebuli = ds
			.Where (d => entities.Contains(d["@metadata"]["Raven-Entity-Name"]))
			.Select (d => 
			{
				d.Inner.Remove("prevState");
				
				var oldMeta = d["@metadata"];
				d.Inner.Remove("@metadata");
				var newMeta = new RavenJObject();
				d.Inner.Add("@metadata", newMeta);
				
				newMeta.Add("Raven-Entity-Name", RavenJToken.FromObject(oldMeta["Raven-Entity-Name"]));
				if(oldMeta["Raven-Entity-Name"] == "cheki"){
					var jid = RavenJToken.FromObject(d.nomeri == null ? d.posisNomeri + "/" + d.chekisNomrisMrickhveli : d.nomeri);
					d.Inner.Add("chekisNomeri", jid);
					newMeta.Add("@id", "cheki/" + jid);
				} else {
					newMeta.Add("@id", RavenJToken.FromObject(oldMeta["@id"]));
				}
				return d;
			}).ToList();
			
	var mdebareobaPosisNomerze = new Dictionary<string,string >{{"1", "merani"},{"2", "batumi"}};
	Func<object, decimal> tankha = d => ((dynamic)d).mnishvneloba / 100.0m;
	Func<object, double> raodenoba = d => ((dynamic)d).mnishvneloba;
	Func<object, decimal> fasi = d => ((dynamic)d).tankha.mnishvneloba / 100.0m;
	
	var gamoceriliCkebi = damatebuli
		.Where (d => d.Dro != null)
		.Where (d => d["@metadata"]["Raven-Entity-Name"] == "movlenebi" && d.type == "GamoiceraCheki")
		.GroupBy (d => d.chekisNomeri)
		.ToDictionary (g => (string)g.Key, g => (DateTimeOffset)g.Min(x=>(DateTimeOffset)x.Dro));
	
	return damatebuli.Where(d=> d["@metadata"]["Raven-Entity-Name"] != "cheki" || gamoceriliCkebi.ContainsKey(d.chekisNomeri) ).Select (d => {
		if(d["@metadata"]["Raven-Entity-Name"] != "cheki"){
			return d;
		}
		var jo = RavenJObject.FromObject(new {
						chekisGamocerisDro = gamoceriliCkebi[d.chekisNomeri],
						gadasakhdeli = tankha(d.gadasakhdeli),
						fasdaklebuliGadasakhdeli = tankha(d.fasdaklebuliGadasakhdeli),
						klientisBaratisNomeri = (string)d.klientisBaratisNomeri,
						posisNomeri = (int)d.posisNomeri,
						sulGadakhdili = tankha(d.sulGadakhdili),
						gasacemi = tankha(d.sulGadakhdili),
						chekisNomeri = (string)d.chekisNomeri,
						mdebareoba = (string)mdebareobaPosisNomerze[d.chekisNomeri.Split('/')[0]],
						gadakhdebi = ((IEnumerable<string>)d.gadakhdebi.Inner.Keys)
											.Select (key => new{
												gadakhdisForma = key,
												migebuliTankha = tankha(d.gadakhdebi[key])
											}),
						produktebi = ((IEnumerable<string>)d.produktebi.Inner.Keys).Select (x => d.produktebi[x])
											.Select (x => new {
												x.@ref, 
												x.dasakheleba, 
												raodenoba = raodenoba(x.raodenoba),
												fasi = fasi(x.fasi),
												fasdaklebuliFasi = fasi(x.fasdaklebuliFasi),
												jami = tankha(x.jami),
												fasdaklebuliJami = tankha(x.fasdaklebuliJami) ,
											})
					
					});
		jo.Add("@metadata", d["@metadata"].Inner);
		return new DynamicJsonObject(jo);
	});
});
}
void SheavseFasdaklebaebi(string sourceUrl,string sourceDb,string targetUrl,string targetDb){
	StreamQuery(sourceUrl,"Klientebi2", sourceDb)
		.Select (x => new {x.baratisNomeri,  x.mimdinareProcenti})
		.Store(x=>"baratebi/"+x.baratisNomeri,"baratebi",targetUrl,targetDb).Count ().Dump();
}
void GannaakhleFasebi(string sourceUrl,string sourceDb,string targetUrl,string targetDb){
CopyDocs(sourceUrl, sourceDb, targetUrl, targetDb,
	 ds => {
		var entities = new HashSet<string>(new[] {"Refebi"});
		var damatebuli = ds
			.Where (d => entities.Contains(d["@metadata"]["Raven-Entity-Name"])).ToList();
		damatebuli.Count.Dump();
		return damatebuli;
	},"refs");
}
void Main()
{
GannaakhleFasebi("http://office.anvol.ge:8080", "Anvol", "http://batumi.anvol.ge:8080", "Pos");
GannaakhleFasebi("http://office.anvol.ge:8080", "Anvol", "http://merani.anvol.ge:8080", "Pos");
return;
SheavseChekebi("http://batumi.anvol.ge:8080", "Pos", "http://office.anvol.ge:8080", "Anketebi");
SheavseChekebi("http://merani.anvol.ge:8080", "Pos", "http://office.anvol.ge:8080", "Anketebi");
SheavseFasdaklebaebi("http://office.anvol.ge:8080", "Anketebi", "http://merani.anvol.ge:8080", "Pos");
SheavseFasdaklebaebi("http://office.anvol.ge:8080", "Anketebi", "http://batumi.anvol.ge:8080", "Pos");
return;
	copy("http://batumi.anvol.ge:8080", "pos", "http://office.anvol.ge:8080", "posebi");
	copy("http://merani.anvol.ge:8080", "Pos", "http://office.anvol.ge:8080", "Posebi");
	return;

	return;
	
//	copy("http://batumi.anvol.ge:8080", "Pos", "http://office.anvol.ge:8080", "Posebi");
//	copy("http://merani.anvol.ge:8080", "Pos", "http://office.anvol.ge:8080", "Posebi");
	return;
	
	
	var docs = Util.Cache(()=>StreamDocs("http://localhost:8080",null,"Pos").ToList(),"docs");
	var chekebiMovlenebidan = docs
		.AsParallel()
		.Where (d => d.type!=null && d.chekisNomeri != null && d.Dro != null)
		.GroupBy (d => d.chekisNomeri)
		.Where (g => g.Any (e => e.type == "GamoiceraCheki"))
		.Select (g => new {ChekisNomeri=(string)g.Key, 
					Produktebi=g .OrderBy (x => x.Dro)
						.Aggregate (new Dictionary<string, int>(), (Dictionary<string, int> dic, dynamic e) => {
							if(e.type == "Gaasuftavda"){
								return new Dictionary<string, int>();
							} else if(e.type == "DaemataProdukti" || e.type == "MoemataRaodenoba"){
								if(dic.ContainsKey(e.@ref)){
									dic[e.@ref] += (int)e.raodenoba.mnishvneloba;
								}else{
									dic.Add(e.@ref,e.raodenoba.mnishvneloba);
								}
							} else if(e.type == "MoakldaRaodenoba"){
								dic[e.@ref] -= (int)e.raodenoba.mnishvneloba;
							}
							return dic;
						}).ToList()
					}).ToList();
	var chekebi = docs
		.AsParallel()
		.Where (d => d["@metadata"]["Raven-Entity-Name"] == "cheki")
		
		.Select (c => new {
							ChekisNomeri =(string)(c.nomeri == null ? c.posisNomeri + "/" + c.chekisNomrisMrickhveli : c.nomeri),
							Produktebi = ((IEnumerable<string>)c.produktebi.Inner.Keys).Select (k => new {k,p=c.produktebi[k]})
									.ToDictionary (x => (string)x.k, x=>(int)x.p.raodenoba.mnishvneloba)
						});
	chekebi.Count ().Dump();
	chekebiMovlenebidan.Count ().Dump();
	return;

	StoreEx.Initialize(ds=>{
	
		ds.DatabaseCommands.Get("movlenebi/2/32136").Dump();
	},"http://ipv4.fiddler:8080","TempMerani");

	return;
	
		
	return;
	StreamQuery("http://office.anvol.ge:8080","Raven/ConflictDocuments","Posebi").Cache()
		.Where (x => x.Id.Split('/')[0] != "Refebi")
		.Dump();
		
	return;
	(
		from doc in StreamDocs("http://localhost:8080","movlenebi/2","TempMerani")
	 	let id = int.Parse(((string)doc["@metadata"]["@id"]).Split('/').Last ())
		orderby id
	 	select doc
	).Store(x=>(string)x["@metadata"]["@id"],"movlenebi","http://batumi.anvol.ge:8080","Pos");
	return;
	var i=1;
	(
		from doc in StreamDocs("http://batumi.anvol.ge:8080","movlenebi","Pos")
	 	let id = (string)doc["@metadata"]["@id"]
		let segmentebi = id.Split('/')
		let v2 = segmentebi.Length == 3
		let s0 = int.Parse(segmentebi.Last())
		let s1 = v2 ? 1 : 0
		orderby s0
		orderby s1
	 	select doc
	)
	.Store(x => "movlenebi/2/"+ (i++).ToString(), "movlenebi", "http://localhost:8080", "TempMerani")
	;
	
	//
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
public IEnumerable<dynamic> StreamDocs(string url,string doc,string database="Anvol", Etag etag = null){
		using(var docStore = (new DocumentStore() {	Url = url,
													DefaultDatabase=database, 
													Conventions = { 
															FindTypeTagName = (t) => t.Name, 
															FindClrTypeName = t => t.Name 
														}
													}).Initialize())
		{
		var enmerator = docStore.DatabaseCommands.StreamDocs(etag, doc, null);
			while(enmerator.MoveNext()){
				yield return new DynamicJsonObject(enmerator.Current);
			}
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


	public static IEnumerable<dynamic> StreamDocs(this Raven.Client.IDocumentStore docStore, string database="Anvol", Etag etag = null, string startWith = null){
		var enmerator = docStore.DatabaseCommands.ForDatabase(database).StreamDocs(etag, startWith);
		while(enmerator.MoveNext()){
			yield return new DynamicJsonObject(enmerator.Current);
		}
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


void GaascoreChekisNomeri(RavenJObject jo){

	dynamic c = new DynamicJsonObject(jo);
	if(c["@metadata"]["Raven-Entity-Name"] != "cheki")
		return;
	var chekisNomeri = (string)(c.nomeri == null ? c.posisNomeri + "/" + c.chekisNomrisMrickhveli : c.nomeri);
	jo.Add("chekisNomeri", RavenJToken.FromObject(chekisNomeri));
}



void copy(string fromUrl, string fromDb, string toUrl, string toDb){
	StoreEx.Initialize(s => {
		StoreEx.Initialize(t => {
			var syncDocId = "syncId/" + Uri.EscapeDataString(fromUrl) + "/" + Uri.EscapeDataString(fromDb);
			var etag = default(string);
			var sincDoc = t.DatabaseCommands.Get(syncDocId);
			if(sincDoc != null) {
				etag = sincDoc.DataAsJson.Value<string>("etag");
			}
			var se = s.DatabaseCommands.StreamDocs(etag == null ? Etag.Empty : Etag.Parse(etag));
			var counter = 0;
			using(var bi = t.BulkInsert(toDb, new BulkInsertOptions{CheckForUpdates=true, BatchSize=2048}))
			{
				while(se.MoveNext())
				{
					counter++;
					var doc = se.Current;
					
					GaascoreChekisNomeri(doc);
					var oldMd = doc.Value<RavenJObject>("@metadata");
					var id = oldMd.Value<string>("@id");
					etag = oldMd.Value<string>("@etag");
					var entityName = oldMd.Value<string>("Raven-Entity-Name");
					
					if(string.IsNullOrWhiteSpace(entityName)  || id.ToLower().StartsWith("raven"))
						continue;
						
					var md = new RavenJObject();
					md.Add("Raven-Entity-Name", entityName);
					doc.Remove("prevState");
					doc.Remove("@metadata");
					bi.Store(doc, md, id);
				}
			}
			if(counter > 0){
				var syncmeta = new RavenJObject();
				syncmeta.Add("Raven-Entity-Name", "SyncIds");
				t.DatabaseCommands.Put(syncDocId, null, RavenJObject.FromObject(new {etag=etag}), syncmeta);
			}
		}, toUrl, toDb);
	}, fromUrl,fromDb);
}