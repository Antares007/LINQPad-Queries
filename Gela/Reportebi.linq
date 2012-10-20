<Query Kind="Program">
  <Connection>
    <ID>c8afd521-dd29-4636-a948-8f39c543719c</ID>
    <Persist>true</Persist>
    <Server>.\mssqlserver12</Server>
    <Database>Gela</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>C:\Temp\EPPlus.dll</Reference>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

void Main()
{
	var sheskidvebi=Sheskidvebis.Select (s => new {Sort=0,s.ID,Dro=s.Tarigi,Dasaxeleba=s.Produqtisdasaxeleba,Strixkodi="'"+s.Strixkodi,Artikuli="'"+s.Artikuli,Shemovida=(int)s.Raodenoba,Gavida=0,Debeti=(decimal)(s.Raodenoba * s.Fasilari),Krediti=0.0m,Realizacia=0.0m}).ToList();
	var gakidevbi=Gakidvebis.Select (g => new {Sort=1,g.ID,Dro=g.Dro,Dasaxeleba=g.Dasakheleba,Strixkodi="'"+g.Strixkodi,Artikuli="'"+g.Artikuli,Shemovida=0, Gavida=(int)g.Raod,Debeti=0.0m,Krediti=0.0m,Realizacia=(decimal)g.JamiLari}).ToList();

	
	var kodebi=new []{"'8411061665077","'8411061607152"};
	var res=sheskidvebi.Concat(gakidevbi)
				.AsParallel()
				.GroupBy (m => m.Strixkodi)
				.Select (artDin => {
					var nShemovida=0;
					var nGavida=0;
					var nNashti=0;
					var nDebeti=0.0m;
					var nKrediti=0.0m;
					var nRealizacia=0.0m;
					var i=0;
					return artDin.OrderBy (d => d.Sort).OrderBy (x => x.Dro).Select (d => 
					{
						nDebeti+=d.Debeti;
						nShemovida+=d.Shemovida;
						
						var kred = nShemovida-nGavida<=d.Gavida ? nDebeti-nKrediti : nShemovida==0?0.0m:Math.Round(nDebeti/nShemovida*d.Gavida,2);
						nKrediti += kred;
						
						nGavida+=d.Gavida;
						nNashti=nShemovida-nGavida;
						
						nRealizacia+=d.Realizacia;
						return new {
									No=++i,d.ID,
									d.Dro,d.Dasaxeleba, d.Strixkodi,d.Artikuli, 
									d.Shemovida, d.Gavida, Nashti=d.Shemovida-d.Gavida,
									d.Debeti,Krediti=kred,Balansi=d.Debeti-d.Krediti,
									Mogeba=d.Realizacia-kred,d.Realizacia,
									
									nShemovida, nGavida, nNashti,
									nDebeti, nKrediti, nBalansi=nDebeti-nKrediti,
									nMogeba=nRealizacia-nKrediti, nRealizacia
									};
					});  
	
			}).SelectMany (artDin => artDin).ToList();
			
var cnobari=(from g in res.GroupBy (x => new {x.Strixkodi,Periodi=(x.Dro.Year*100+x.Dro.Month) })
	let lastNo=g.Max (x => x.No)
	let lastM=g.First (x => x.No==lastNo)
	let shemovida=g.Sum (x => x.Shemovida)
	let gavida=g.Sum(x=>x.Gavida)
	let sackisi=lastM.nNashti-shemovida+gavida
	let debeti=g.Sum (x => x.Debeti)
	let krediti=g.Sum (x => x.Krediti)
	let sackisiBalansi=lastM.nBalansi-debeti+krediti
	let realizacia = g.Sum (x => x.Realizacia)
	let mogeba = realizacia-krediti
select new {g.Key.Periodi,g.Key.Strixkodi,sackisi,shemovida,gavida,saboloo=lastM.nNashti,
			  		sackisiBalansi,debeti,krediti,sabolloBalansi=lastM.nBalansi,nMogeba=mogeba,nRealizacia=realizacia}
).ToList();

var periodebi=  res.Select (r => (r.Dro.Year*100+r.Dro.Month)).Distinct();
var produktebi = res.Select (r => new {r.Dasaxeleba,r.Strixkodi,r.Artikuli}).GroupBy (r => r.Strixkodi).Select (g => g.First ());

periodebi
	.SelectMany (per => produktebi
						.Select (prod => new {per, prod, conb = cnobari.Where (cnob => cnob.Periodi <= per && cnob.Strixkodi==prod.Strixkodi).OrderByDescending (cnob => cnob.Periodi).FirstOrDefault ()}
									)
				)
				.Where (x => x.conb!=null)
//				.Where(s => kodebi.Contains(s.prod.Strixkodi))
				.Select (x => {
								if(x.per!=x.conb.Periodi)
								return new {Periodi=x.per,x.prod.Artikuli,x.prod.Strixkodi,x.prod.Dasaxeleba,
											sackisi=x.conb.saboloo,shemovida=0,gavida=0,x.conb.saboloo,sackisiBalansi=x.conb.sabolloBalansi,debeti=0m,krediti=0m,x.conb.sabolloBalansi,nMogeba=0m,nRealizacia=0m};
								
								return new {
										Periodi=x.per,x.prod.Artikuli,x.prod.Strixkodi,x.prod.Dasaxeleba,
										x.conb.sackisi,x.conb.shemovida,x.conb.gavida,x.conb.saboloo,x.conb.sackisiBalansi,x.conb.debeti,x.conb.krediti,x.conb.sabolloBalansi,x.conb.nMogeba,x.conb.nRealizacia
										};
								}
						)
				.Where (x => x.Periodi==201112)
				.Dump();
					
//							.Select (r => new { Produkti=r,Periodi=x,Ciprebi=cnobari.Where(c => c.Periodi<=x &&c.Strixkodi==r.Strixkodi).OrderByDescending (c => c.Periodi).FirstOrDefault () ?? 
//																																			new {Periodi=x,Strixkodi=r.Strixkodi,sackisi=0,shemovida=0,gavida=0,saboloo=0,
//			  																																				sackisiBalansi=0m,debeti=0m,krediti=0m,sabolloBalansi=0m,nMogeba=0m,nRealizacia=0m}
//							}))
//							.Select (x => new {x.Periodi,x.Produkti.Artikuli,x.Produkti.Strixkodi,x.Produkti.Dasaxeleba,x.Ciprebi.sackisi,x.Ciprebi.shemovida,x.Ciprebi.gavida,x.Ciprebi.saboloo,x.Ciprebi.sackisiBalansi,x.Ciprebi.debeti,x.Ciprebi.krediti,x.Ciprebi.sabolloBalansi,x.Ciprebi.nMogeba,x.Ciprebi.nRealizacia})
//							//.Where (x => x.Periodi==201108)
//							.Where (s => kodebi.Contains(s.Strixkodi))
//							.OrderBy (x => x.Periodi).OrderBy (x => x.Strixkodi)
//							.Dump();

//			res.Select (d => new {     d.No,d.ID,
//									d.Dro,d.Dasaxeleba, d.Strixkodi,d.Artikuli, 
//									d.nShemovida, d.nGavida, d.nNashti,
//									d.nDebeti, d.nKrediti, d.nBalansi,
//									d.nMogeba,d.nRealizacia})
//			.Where (x => x.Dro < DateTime.Parse("2011-10-01"))
//			.GroupBy (x => x.Strixkodi)
//			.Select (g => g.First (x => x.No==g.Max (x_ => x_.No)))
//			.Dump();
}



public int LevenshteinDistance(string source, string target){
		if(String.IsNullOrEmpty(source)){
				if(String.IsNullOrEmpty(target)) return 0;
				return target.Length;
		}
		if(String.IsNullOrEmpty(target)) return source.Length;
 
		if(source.Length > target.Length){
				var temp = target;
				target = source;
				source = temp;
		}
 
		var m = target.Length;
		var n = source.Length;
		var distance = new int[2, m + 1];
		// Initialize the distance 'matrix'
		for(var j = 1; j <= m; j++) distance[0, j] = j;
 
		var currentRow = 0;
		for(var i = 1; i <= n; ++i){
				currentRow = i & 1;
				distance[currentRow, 0] = i;
				var previousRow = currentRow ^ 1;
				for(var j = 1; j <= m; j++){
						var cost = (target[j - 1] == source[i - 1] ? 0 : 1);
						distance[currentRow, j] = Math.Min(Math.Min(
												distance[previousRow, j] + 1,
												distance[currentRow, j - 1] + 1),
												distance[previousRow, j - 1] + cost);
				}
		}
		return distance[currentRow, m];
}
private Int32 levenshtein(String a, String b)
{

	if (string.IsNullOrEmpty(a))
	{
		if (!string.IsNullOrEmpty(b))
		{
			return b.Length;
		}
		return 0;
	}

	if (string.IsNullOrEmpty(b))
	{
		if (!string.IsNullOrEmpty(a))
		{
			return a.Length;
		}
		return 0;
	}

	Int32 cost;
	Int32[,] d = new int[a.Length + 1, b.Length + 1];
	Int32 min1;
	Int32 min2;
	Int32 min3;

	for (Int32 i = 0; i <= d.GetUpperBound(0); i += 1)
	{
		d[i, 0] = i;
	}

	for (Int32 i = 0; i <= d.GetUpperBound(1); i += 1)
	{
		d[0, i] = i;
	}

	for (Int32 i = 1; i <= d.GetUpperBound(0); i += 1)
	{
		for (Int32 j = 1; j <= d.GetUpperBound(1); j += 1)
		{
			cost = Convert.ToInt32(!(a[i-1] == b[j - 1]));

			min1 = d[i - 1, j] + 1;
			min2 = d[i, j - 1] + 1;
			min3 = d[i - 1, j - 1] + cost;
			d[i, j] = Math.Min(Math.Min(min1, min2), min3);
		}
	}

	return d[d.GetUpperBound(0), d.GetUpperBound(1)];

}
