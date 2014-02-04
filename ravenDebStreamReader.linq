<Query Kind="Program">
  <NuGetReference>RavenDB.Client</NuGetReference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>Raven.Abstractions.Data</Namespace>
  <Namespace>Raven.Client.Document</Namespace>
  <Namespace>Raven.Json.Linq</Namespace>
</Query>

void Main()
{
	var url = "http://batumi.anvol.ge:8080/";
	var defaultDatabase = "Pos";
	using(var docStore = (new DocumentStore() {	Url = url,
												DefaultDatabase = defaultDatabase, 
												Conventions = { 
														FindTypeTagName = t => t.Name, 
														FindClrTypeName = t => t.Name 
													}
												}).Initialize())
	{
		var enumerator = docStore.DatabaseCommands.StreamDocs(fromEtag : Etag.Parse("01000000-0000-0000-0000-000000000000"));
		
		var i = 0;
		while (enumerator.MoveNext())
		{
//			enumerator.Current
//				.Value<RavenJObject>("@metadata")
//				.Dump();
			i++;
		}
		if(enumerator.Current != null)
		{
			enumerator.Current
				.Value<RavenJObject>("@metadata")
				.Value<string>("@etag")
				.Dump(i.ToString());
		}
	}
}