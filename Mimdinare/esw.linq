<Query Kind="Statements">
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
  <Namespace>Lokad.Cqrs</Namespace>
</Query>

var conf=Lokad.Cqrs.FileStorage.CreateConfig("c:\\temp\\dom");
conf.EnsureDirectory();
var tape=conf.CreateTape();
var s1=tape.GetOrCreateStream("st2");
s1.TryAppend(BitConverter.GetBytes(7));

