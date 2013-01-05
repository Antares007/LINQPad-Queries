<Query Kind="Program">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAAAD89x4SL38S/4r7NUU2iHNNTcmnVTi1xQPx1AC1vAtFAAAAAAOgAAAAAIAACAAAABgO+xVBio0IKzceIXWbWfgFv0jcxQpOA9YilhDtPA8XxAAAABJcM5+MLInsGd5jUUGfXtnQAAAALHy7sVof5cKLhfpSHxbLdPESALwKOWLElOgeYcZmaeqO1sDG9SHnPN5xOzSlBHlSD7agk+KC9dH/4XMZn4gYvM=</Password>
    <Database>SocialuriDazgveva</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference Relative="..\..\Visual Studio 2012\Projects\UketesiReestri\UketesiReestri\bin\Debug\UketesiReestri.dll">&lt;MyDocuments&gt;\Visual Studio 2012\Projects\UketesiReestri\UketesiReestri\bin\Debug\UketesiReestri.dll</Reference>
  <Namespace>Ur</Namespace>
</Query>

void Main()
{
	var mzgDic = MzgveveliKompaniebis.ToDictionary (mk => mk.MzgveveliKompaniisKodi);
	DirSearch(@"\\db\i$\სადაზღვევოებს")
		.Select (x => new {x,Substring17=x.Substring(22)})
		.Select (x => new {FailisMisamarti=x.x,Shinaarsi=x.Substring17.Substring(0,x.Substring17.IndexOf("\\")),ShekmnisTarigi=File.GetCreationTimeUtc(x.x),Kompania=mzgDic.Keys.First(k=>x.x.Contains(k))})
		
		.GroupBy (x => x.Kompania)
		.ToDictionary(x=>x.Key,x=>x.OrderByDescending (x_ => x_.ShekmnisTarigi).ToList())
		.Dump();
}

// Define other methods and classes here
static IEnumerable<string> DirSearch(string sDir)
   {
           foreach (string d in Directory.GetDirectories(sDir))
           {
               foreach (string f in Directory.GetFiles(d,"*.accdb"))
                   yield return f;
               foreach (var f in DirSearch(d))
				   yield return f;
           }
   }