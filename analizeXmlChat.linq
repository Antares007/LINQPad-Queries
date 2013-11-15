<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
	var str = File.ReadAllText(@"C:\Users\Acho\Documents\t.xml").Replace("<br>"," ");
	
	var messages = XDocument
		.Parse(str)
		.Descendants("item")
		
		.Select (xd => xd.Descendants().ToList())
		
		.Select ((x,i) => new {Id=long.Parse(x[0].Value),Dt=x[2].Value,User=x[3].Value,Mesage=x[6].Value,ChatId=x[7].Value}).ToList();
	
//	messages
//		.SelectMany (x => x.Mesage.ToLowerInvariant().Split(new []{' ',';',',','.','-','_','?'}))
//		.Where (x => (x.StartsWith("a") && x.Contains("ko")) || 
//					 (x.StartsWith("ar") && x.Contains("l"))
//				)
//		.GroupBy (x => x)
//		.Select (g => new {g.Key,Raod=g.Count (),Ids=})
//		.OrderByDescending (x => x.Raod)
//		.Dump();
		messages
		.OrderByDescending(x => x.Id)
		//.Where (x => (159889-10)<x.Id&&x.Id<(159889+10))
		.Where (x => x.ChatId.IndexOf(';')>=0)
		.GroupBy (x => string.Join("-",x.ChatId.Substring(0,x.ChatId.IndexOf(';')).Split(new []{'#','/','$'},StringSplitOptions.RemoveEmptyEntries).OrderBy (ci => ci)))
		.Select (g => g.OrderByDescending (x => x.Id))
		.Dump();
	
	
}

// Define other methods and classes here