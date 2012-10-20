<Query Kind="Program">
  <Connection>
    <ID>e9890aa3-87d6-47dd-aa04-7445086a72ab</ID>
    <Persist>true</Persist>
    <Server>.\SQLEXPRESS</Server>
    <Database>Lutecia</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

void Main()
{
	var q = (from a in Modzraobas
	group a by a.Kodi into g
	select new {g.Key,Dasakhelebebi=g.Select (x => x.Dasakheleba).Distinct().ToList(), DasakhRaod=g.Select (x => x.Dasakheleba).Distinct().Count()});
	
	    var q2 = from x in q
			 where x.DasakhRaod>1
			 select x;
	    q2.Dump();
}

// Define other methods and classes here
