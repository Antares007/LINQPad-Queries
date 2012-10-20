<Query Kind="Program">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAAAD89x4SL38S/4r7NUU2iHNNTcmnVTi1xQPx1AC1vAtFAAAAAAOgAAAAAIAACAAAABgO+xVBio0IKzceIXWbWfgFv0jcxQpOA9YilhDtPA8XxAAAABJcM5+MLInsGd5jUUGfXtnQAAAALHy7sVof5cKLhfpSHxbLdPESALwKOWLElOgeYcZmaeqO1sDG9SHnPN5xOzSlBHlSD7agk+KC9dH/4XMZn4gYvM=</Password>
    <Database>JUST</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>D:\Dev\lokad-cqrs\Cqrs.Portable\bin\Debug\Lokad.Cqrs.Portable.dll</Reference>
  <NuGetReference>EventStore</NuGetReference>
  <NuGetReference>EventStore.Serialization.ServiceStack</NuGetReference>
  <Namespace>EventStore</Namespace>
  <Namespace>EventStore.Conversion</Namespace>
  <Namespace>EventStore.Dispatcher</Namespace>
  <Namespace>EventStore.Logging</Namespace>
  <Namespace>EventStore.Persistence</Namespace>
  <Namespace>EventStore.Persistence.InMemoryPersistence</Namespace>
  <Namespace>EventStore.Persistence.SqlPersistence</Namespace>
  <Namespace>EventStore.Persistence.SqlPersistence.SqlDialects</Namespace>
  <Namespace>EventStore.Serialization</Namespace>
  <Namespace>Lokad.Cqrs</Namespace>
</Query>

void Main()
{
	AppDomain.CurrentDomain.SetupInformation.ConfigurationFile.Dump();
	var store = Wireup.Init()
	    .UsingSqlPersistence("Eventstore")
	        .InitializeStorageEngine()
	        .UsingServiceStackJsonSerialization()
	//            .EncryptWith(EncryptionKey)
	    //.HookIntoPipelineUsing(new[] { new AuthorizationPipelineHook() })
	    .UsingAsynchronousDispatchScheduler()
	        // Example of NServiceBus dispatcher: https://gist.github.com/1311195
	      //  .DispatchTo(new My_NServiceBus_Or_MassTransit_OrEven_WCF_Adapter_Code())
	    .Build();   
		
	using (store)
	{
	    using (var stream = store.OpenStream(new Guid("33520cfa-2a15-4252-a8e0-8c36b363d225"), 0, int.MaxValue))
	    {
	        stream.Add(new EventMessage { Body = new Foo{Value=1} });
	        stream.CommitChanges(Guid.NewGuid());
	    }
	}
}
public class Foo{
	public int Value{get;set;}
}

// Define other methods and classes here