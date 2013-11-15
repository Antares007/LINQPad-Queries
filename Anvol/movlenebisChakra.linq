<Query Kind="Program">
  <Reference>D:\Dev\EventStore\bin\eventstore\debug\anycpu\EventStore.ClientAPI.dll</Reference>
  <NuGetReference>Ix-Main</NuGetReference>
  <NuGetReference Prerelease="true">RavenDB.Client</NuGetReference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>EventStore.ClientAPI</Namespace>
  <Namespace>Raven.Abstractions.Data</Namespace>
  <Namespace>Raven.Abstractions.Linq</Namespace>
  <Namespace>Raven.Client.Document</Namespace>
  <Namespace>Raven.Json.Linq</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{
StoreEx.Initialize((docStore)=>{
var enumerator = docStore
	.DatabaseCommands.ForDatabase("Anvol")
	.StreamDocs("01000000-0000-000F-0000-000000000003");
enumerator.MoveNext();
enumerator.Current.Dump();
},"http://25.88.224.201:8080/","Anvol");

return;
//var connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1113));
//connection.Connect();

	using(var docStore = (new DocumentStore() {	Url = "http://localhost:8080/",
												DefaultDatabase="Anvol", 
												Conventions = { 
														FindTypeTagName = (t) => t.Name, 
														FindClrTypeName = t => t.Name 
													}
												}).Initialize())
	{
//	var pakings=docStore
//						.DatabaseCommands
//						.ForDatabase("Anvol")
//						.StreamDocs(startsWith:"PakingListi")
//						.ToEnumerable()
//						;
//	var i=1;
//	foreach (var d in pakings)
//	{
//		d.Add("Id",RavenJValue.FromObject(i++));
//		connection.AppendToStream("PakingListi",-2,
//			new EventData(
//				Guid.NewGuid()
//				, "PakingListi"
//				, true
//				, Encoding.UTF8.GetBytes(d.ToString())
//				, null)
//		);
//	}
//	
//	connection.Dispose();
//	return;
		QueryHeaderInformation qh;
		var enumerator=docStore
						.DatabaseCommands
						.ForDatabase("Db2")
						.StreamQuery("Movlenebi", new IndexQuery {  }, out qh);
		var meta= new RavenJObject();
		meta.Add("Raven-Entity-Name", RavenJToken.FromObject("Movlenebi"));
		var movlenebi = enumerator.ToEnumerable()
			.SelectMany (x => x.Value<RavenJArray>("Movlenebi"))
			.Cast<RavenJObject>()
			.OrderBy (rja => rja.Value<int>("Id"))
			.Select (x => {x.Remove("Id");return x;})
			.Select (x => new {o=x,m=meta})
			.ToList();				
		
		var remote = (new DocumentStore() {	Url = "http://localhost:8080/",
												DefaultDatabase="Anvol", 
												Conventions = { 
														FindTypeTagName = (t) => t.Name, 
														FindClrTypeName = t => t.Name 
													}
												}).Initialize();	
		foreach (var b in movlenebi)
		{
			remote.DatabaseCommands.ForDatabase("Anvol")
				.Put("movlenebi/", null, b.o, b.m);
		}
	}
}
public static class Ex{
	public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> enumerator){
		while(enumerator.MoveNext())
		{	
			yield return enumerator.Current;
		}
	}
}
// Define other methods and classes here

public static class StoreEx{
	public static void OpenSession(this Raven.Client.IDocumentStore store,string dbName, Action<Raven.Client.IDocumentSession> action){
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