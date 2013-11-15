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
  <Reference Relative="..\..\AppData\Local\LINQPad\NuGet\ServiceStack.Redis\ServiceStack.Common.3.9.59\lib\net35\ServiceStack.Common.dll">&lt;LocalApplicationData&gt;\LINQPad\NuGet\ServiceStack.Redis\ServiceStack.Common.3.9.59\lib\net35\ServiceStack.Common.dll</Reference>
  <Reference Relative="..\..\AppData\Local\LINQPad\NuGet\ServiceStack.Redis\ServiceStack.Common.3.9.59\lib\net35\ServiceStack.Interfaces.dll">&lt;LocalApplicationData&gt;\LINQPad\NuGet\ServiceStack.Redis\ServiceStack.Common.3.9.59\lib\net35\ServiceStack.Interfaces.dll</Reference>
  <NuGetReference>ManagedEsent</NuGetReference>
  <NuGetReference>ServiceStack.Redis</NuGetReference>
  <NuGetReference>ServiceStack.Text</NuGetReference>
  <Namespace>Microsoft.Isam.Esent.Collections.Generic</Namespace>
  <Namespace>ServiceStack.Redis</Namespace>
</Query>

public class User{
	public string Id { get; set; }
}
void Main()
{
	var redis  = new RedisClient("127.0.0.1",6379);
	using (var redisUsers = redis.GetTypedClient<User>())
	{
		redis.GetById<User>("1").Dump();;
		redisUsers.GetAll().Dump();
	}
		
	
}
