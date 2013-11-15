<Query Kind="Program">
  <Connection>
    <ID>393fc2a1-3d2f-4fc3-8b9d-c916c0bb1c52</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAN27c6lA2WkeiuPoKsF6zVAAAAAACAAAAAAAQZgAAAAEAACAAAACbLLAsNsDt7paFHc5L9mKrBtKrPuHB+O2Pe8qohNTpzwAAAAAOgAAAAAIAACAAAAARwiWVIvi4yqvM5LGOqZqOnRK7TKpCRjPXZ7PkGNEFvxAAAAA0DMltrt8SinSUpeRajEf6QAAAACzIm5Lx/1cDUpvEPcBjgOVVcG4y9njzOG5jRo3BFxCqffNXq8PX7vXvt6t2LlSsl7mTqha7tgg+9E46F4mXHMQ=</Password>
    <Database>SocialuriDazgveva</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>D:\anycpu\EventStore.ClientAPI.dll</Reference>
  <NuGetReference>Ix-Main</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>EventStore.ClientAPI</Namespace>
  <Namespace>EventStore.ClientAPI.SystemData</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{
//	var uc =  new UserCredentials("admin", "changeit");
//	var es = EventStoreConnection.Create(new IPEndPoint(IPAddress.Parse("127.0.0.1"),1113));
//	es.Connect();
//	
////	es.DeleteStream("Polisebi", -2, uc);
////	es.DeleteStream("$et-Polisi", -2, uc);
//
//	Polisebis
//		.Select (p => new {p.PolisisNomeri,p.PiradiNomeri,p.Sakheli,p.Gvari,p.DabadebisTarigi,p.Raioni,p.Kalaki,p.Sopeli,p.SruliMisamarti})
//		.AsEnumerable()
//		.Select (p => JsonConvert.SerializeObject(p))
//		.Select (p => new EventData(Guid.NewGuid(), "Polisi", true, Encoding.UTF8.GetBytes(p), null))
//		.Buffer(1024)
//		.ForEach(b => es.AppendToStream("Polisebi", -2, uc, b.ToArray()));
//	
////	es.GetStreamMetadata("$stats-127.0.0.1:2113", new UserCredentials("admin", "changeit")).Dump();
////	es.SetStreamMetadata("$stats-127.0.0.1:2113", -2, StreamMetadata.Build().SetMaxCount(10), new UserCredentials("admin", "changeit"));
//	es.Dispose();
//	return;

	int? lastProcessedEventNumber = null;
	var connSettingsBuilder = ConnectionSettings
		.Create().KeepReconnecting()
		.SetDefaultUserCredentials(new UserCredentials("admin", "changeit"))
		.OnConnected((conn,endPoint) => {
			//get lastProcessedEventNumber 
			conn.SubscribeToStreamFrom(
				  "$stats-127.0.0.1:2113"
				, lastProcessedEventNumber
				, false
				, (s, e) => {
					lastProcessedEventNumber = e.OriginalEventNumber.Dump();
					// store lastProcessedEventNumber
				  }
			);
		});
		
	Func<IEventStoreConnection> cf = () => EventStoreConnection.Create(connSettingsBuilder, new IPEndPoint(IPAddress.Parse("127.0.0.1"),1113));
	connSettingsBuilder.OnClosed((conn, str) => {
		conn.Dispose();
		"Reconecting...".Dump();
//		cf().Connect();
	});
	cf().Connect();
}