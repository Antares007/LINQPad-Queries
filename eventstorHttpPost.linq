<Query Kind="Program">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAAAD89x4SL38S/4r7NUU2iHNNTcmnVTi1xQPx1AC1vAtFAAAAAAOgAAAAAIAACAAAABgO+xVBio0IKzceIXWbWfgFv0jcxQpOA9YilhDtPA8XxAAAABJcM5+MLInsGd5jUUGfXtnQAAAALHy7sVof5cKLhfpSHxbLdPESALwKOWLElOgeYcZmaeqO1sDG9SHnPN5xOzSlBHlSD7agk+KC9dH/4XMZn4gYvM=</Password>
    <Database>Pirvelckaroebi</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <NuGetReference>ServiceStack.Text</NuGetReference>
  <Namespace>ServiceStack.Text</Namespace>
  <Namespace>ServiceStack.Text.Json</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{
    
    var appender=CreateStreamAppender("movlenebi","GankhorcieldaDzebna","192.168.0.100");

    appender(new {sadzieboTexti="01010101", sistema="Polisi165", dro=DateTime.UtcNow});
    appender(new {sadzieboTexti="01010101", sistema="Hibriduli", dro=DateTime.UtcNow});
    appender(new {sadzieboTexti="01010101", sistema="DazgvevisReportebi", dro=DateTime.UtcNow});
}

Action<object> CreateStreamAppender(string stream,string eventType, string hostAddress="172.17.112.230")
{
    var url = string.Format("http://{0}:2113/streams/{1}", hostAddress, stream);
    return (@event) =>
        {
            try
            {
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(url);
                var json = new { expectedVersion=-2,
                                                            events=new[] {
                                                                        new { eventId=Guid.NewGuid(),
                                                                            eventType=eventType,
                                                                            data=396
                                                                            }
                                                            }}.ToJson().Replace("396", @event.ToJson());
                Console.WriteLine(json);
                byte[] data = Encoding.UTF8.GetBytes( json  );
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/json";
                httpWReq.ContentLength = data.Length;
                using (Stream newStream = httpWReq.GetRequestStream())
                {
                    newStream.Write(data, 0 , data.Length);
                }
            }
            catch
            {
            }
        };
}
