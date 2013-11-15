<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\Accessibility.dll</Reference>
  <Reference>D:\Dev\EventStore\bin\eventstore\release\anycpu\EventStore.ClientAPI.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Deployment.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Serialization.Formatters.Soap.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <NuGetReference>ServiceStack.Text</NuGetReference>
  <Namespace>EventStore.ClientAPI</Namespace>
  <Namespace>EventStore.ClientAPI.SystemData</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

void ConnectTest2()
{
	var connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1113));
	connection.Connect();
	var sw = new System.Diagnostics.Stopwatch();
	sw.Start();
	
	var evc=Enumerable.Range(0, 500)
			  .Select (_ => connection.ReadAllEventsBackward().ToList())
			  .Select (x => new {Count = x.Count, Bytes = x.Sum(x_ => ((byte[])x_.Data).Length + (long)((byte[])x_.Metadata).Length) })
			  .ToList()
			  ;
	
	sw.Stop();
	((evc.Sum (e => e.Count) / (double)sw.Elapsed.TotalMilliseconds)*1000).Dump();
	((evc.Sum (e => e.Bytes) / (double)sw.Elapsed.TotalMilliseconds)*1000/1024.0/1024.0).Dump();

	evc.Dump();
	connection.Dispose();
}
void ConnectTest()
{
	Position? pos=null;
	var settings =  ConnectionSettings.Create()
		.OnConnected((conn,endpoint)=>{
			conn.SubscribeToAllFrom(pos??Position.Start,true,(s,e)=>{new {t="eventAppeared",e.OriginalPosition}.Dump();pos=e.OriginalPosition;},s=>{new {t="liveProcessingStarted"}.Dump();},(s,dr,ex)=>{new{t="subscriptionDropped",dr,ex}.Dump();});
		})
		.KeepRetrying()
		.KeepReconnecting()
		.SetDefaultUserCredentials(new UserCredentials("admin","changeit"))
		.OnReconnecting(conn => { "Reconnecting".Dump(); });

	var connection = EventStoreConnection.Create(settings,new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1113));
	connection.Connect();
}
void Main()
{
ConnectTest2();
return;
	var docs = new []{
		new { v=1, doc = new { id = 1, refs = new[]{ "A" }}},
		new { v=1, doc = new { id = 2, refs = new[]{ "B" }}},
		new { v=1, doc = new { id = 3, refs = new[]{ "C" }}},
		new { v=1, doc = new { id = 4, refs = new[]{ "A" }}},
		new { v=1, doc = new { id = 5, refs = new[]{ "C" }}},
	};
	
	docs.Select (d => ServiceStack.Text.JsonSerializer.SerializeToString(d))
		.Dump();

	using(var connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1113)))
	{
		connection.Connect();
		connection.AppendToStream("Jurnali",-2,
			docs.Select (d => new EventData(Guid.NewGuid(),"DaemataDocumenti",true, Encoding.UTF8.GetBytes(ServiceStack.Text.JsonSerializer.SerializeToString(d)),null)));
			
		connection.ReadStreamEventsForward("Jurnali", 0, 30, true, new UserCredentials("admin","changeit"))
					.Events.Select (e => new {
								Event = e.Event.ToJsonEvent(),
								Link = e.Link.ToJsonEvent(),
								OriginalEvent = e.OriginalEvent.ToJsonEvent(),
								e.IsResolved,
								e.OriginalEventNumber,
								e.OriginalPosition,
								e.OriginalStreamId
							}).Dump();
	
	}
	
}
public class JsonEvent
{
   	public readonly string EventStreamId;
   	public readonly Guid EventId;
   	public readonly int EventNumber;
   	public readonly string EventType;
   	public readonly Hyperlinq JsonData;
   	public readonly Hyperlinq JsonMetadata;
   	public readonly byte[] Data;
   	public readonly byte[] Metadata;	
	public readonly DumpContainer DumpContainer;
	internal JsonEvent(RecordedEvent systemRecord)
	{
		EventStreamId = systemRecord.EventStreamId;
		EventId = systemRecord.EventId;
		EventNumber = systemRecord.EventNumber;
		EventType = systemRecord.EventType;
		DumpContainer = new DumpContainer();
		JsonData = new Hyperlinq(()=>{DumpContainer.Content = (Encoding.UTF8.GetString(systemRecord.Data));},"Data");
		JsonMetadata = new Hyperlinq(()=>{DumpContainer.Content = Encoding.UTF8.GetString(systemRecord.Metadata);},"Metadata");
		Data = systemRecord.Data;
		Metadata = systemRecord.Metadata;
	}
}
public static class  Ex
{
	public static IEnumerable<dynamic> ReadAllEventsBackward(this IEventStoreConnection conn)
	{
		Position pos = Position.End;
		var uc = new UserCredentials("admin","changeit");
		while (true)
		{
			var slice = conn.ReadAllEventsBackward(pos, 1024, false, uc);
			foreach(var e in slice.Events.Select (ev => ev.Event.ToJsonEvent()))
			 	yield return e;
			pos = slice.NextPosition;
			if(slice.IsEndOfStream)
				break;
		}
	}

	public static JsonEvent ToJsonEvent(this RecordedEvent e)
	{
		if (e == null)
			return null;
		return new JsonEvent(e);
	}
	
	public static ServiceStack.Text.JsonObject ToJsonObject(this byte[] bytes){
		return ServiceStack.Text.JsonSerializer.DeserializeFromString<ServiceStack.Text.JsonObject>(Encoding.UTF8.GetString(bytes));
	}
}