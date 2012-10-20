<Query Kind="Program">
  <Connection>
    <ID>8c49975d-7243-4e66-bf5a-92b155c73773</ID>
    <Persist>true</Persist>
    <Server>MegaMozg</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA3XCANzX8A0+HtkxmZtXFGAAAAAACAAAAAAAQZgAAAAEAACAAAACwZJTj6RI6G9b/trIDVOasBfrbl2/EeQ2symplXOxXjgAAAAAOgAAAAAIAACAAAACXcRvebWOVOdKm5bohQ6HFL7D5ZCdnJk1K+ulOvqgM9RAAAADzUWAKzvQZ+zIgoC2VCkD9QAAAAEjICu0vvEQYXF3gevv5l9vF9oZmUg9AyecZZw5eTwc1lDjBuJgZDUtRsZwzbxxaX1vttFBABp76hg+1Cy00Dq0=</Password>
    <Database>Pirvelckaroebi</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>C:\Temp\EPPlus.dll</Reference>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

void Main()
{
	var ids = new[]{5133796, 152896, 175363, 354743, 382314, 5119200, 6496246, 154303, 930217, 938482, 959461, 1318528, 1458939, 5991959, 6004069, 6015398, 6503909, 286299, 976134, 983652, 1928645, 5290523, 6045348, 982502, 890426, 986707, 989135, 1139973, 1667189, 4840755, 6091674, 6501970, 1380520, 12807, 24423, 213519, 567966, 663400, 680961, 808312, 819317, 952831, 1310191, 1393521, 1698447, 1758515, 1832463, 1960098, 4754630, 4760273, 5016218, 5526314, 5612032, 5870275, 5969922, 6095422, 6102914, 6172692, 6434462, 6491967, 4990619, 50101, 60642, 489335, 647932, 696169, 700802, 703934, 783910, 1596624, 1665925, 1667517, 1944592, 4935636, 5460203, 5542049, 5609528, 5623202, 5627503, 5638156, 5856123, 1805726, 793416, 834646, 5808542, 5952546, 901941, 98803, 139249, 179175, 296886, 994968, 1813749, 5213194, 5793335, 6059606, 1604802, 186393, 230252, 233250, 963453, 1063323, 1148876, 1609042, 5119014, 5148505, 5401699, 5484149, 6018763, 6196207, 6306824, 1749540, 86431, 230307, 244880, 1691964, 1878785, 1946388, 4813518, 4947886, 5024246, 5162217, 5667671, 6121404, 584745, 155249, 464855, 480757, 641827, 721703, 725328, 776675, 843969, 1659468, 1824885, 1931745, 5084258, 5122642, 5569186, 5599839, 5793495, 6031497, 6289808, 6350565, 6179720, 172811, 181162, 850915, 857999, 1328714, 1378631, 1820909, 4761071, 4761397, 5096784, 5813332, 5933637, 1193483, 110534, 435861, 612687, 5057170, 5598318, 6505679, 136838, 941211, 943625, 955798, 1024532, 1322480, 1459458, 1846506, 5049835, 5980542, 6002400, 6015753, 1934320, 1311793, 1320750, 1381125, 1458631, 1626304, 1840825, 1928711, 4755086, 5356666, 5513138, 5530893, 5968197, 6106533, 6259479, 6376873, 990097, 231322, 942711, 992340, 1886332, 4835324, 5187796, 5970762, 6054397, 1382807, 1623003, 1798215, 1960287, 4892320, 5021231, 5148382, 5215430, 6376286, 50711, 143671, 471135, 684357, 689080, 744181, 1621521, 1626092, 1931773, 1952090, 4758788, 4963422, 5626960, 5878681, 5967994, 6018386};
	var sakheebi=Source_Data.Where (sd => ids.Contains(sd.ID)).AsEnumerable()
			.Select (x => new {
								Id			= x.ID,
								Fid			= !string.IsNullOrWhiteSpace(x.FID) ? x.FID : null,
								Pid 		= x.J_ID.HasValue ? x.PID : null,
								AlbatPid 	= !x.J_ID.HasValue && !string.IsNullOrWhiteSpace(x.PID) && x.PID.Length == 11 && x.PID.All(c=>char.IsNumber(c)) ? x.PID : null,
								DocNo 		= !string.IsNullOrWhiteSpace(x.PID) && (x.PID.Length != 11 || x.PID.Any(c=>!char.IsNumber(c))) ? x.PID : null,
								Sakheli 	= !string.IsNullOrWhiteSpace(x.First_Name) ? x.First_Name : null,
								Gvari 		= !string.IsNullOrWhiteSpace(x.Last_Name) ? x.Last_Name : null,
								DabTarigi 	= x.Birth_Date.HasValue ? x.Birth_Date.Value.ToString("yyyy-MM-dd") : null,
								DabCeli 	= x.Birth_Date.HasValue ? x.Birth_Date.Value.ToString("yyyy") + "celi" : null,
								DabTve 		= x.Birth_Date.HasValue ? x.Birth_Date.Value.ToString("MM") + "tve" : null,
								DabDge 		= x.Birth_Date.HasValue ? x.Birth_Date.Value.ToString("dd") + "dge" : null,
							  }
					).ToList();
sakheebi.Count ().Dump();

var maps=(
	from s in sakheebi
	from p in new [] {
					new { Msgaveba=s.Pid, 		Khariskhi=1, Foto = string.Format("{0} {1} {2}", 			s.Sakheli, s.Gvari, s.DabTarigi)},
					new { Msgaveba=s.Pid, 		Khariskhi=2, Foto = string.Format("{0} {1}", 				s.Sakheli, s.DabTarigi)},
					new { Msgaveba=s.Pid, 		Khariskhi=3, Foto = string.Format("{0}", 					s.DabTarigi)},
					new { Msgaveba=s.AlbatPid, 	Khariskhi=4, Foto = string.Format("{0} {1} {2}", 			s.Sakheli, s.Gvari, s.DabTarigi)},
					new { Msgaveba=s.Fid,		Khariskhi=5, Foto = string.Format("{0} {1} {2} {3} {4}",	s.Pid,s.DocNo,s.Sakheli, s.Gvari, s.DabTarigi)},
					new { Msgaveba=s.DocNo,		Khariskhi=6, Foto = string.Format("{0} {1} {2} {3} {4}",	s.Pid,s.Fid,s.Sakheli, s.Gvari, s.DabTarigi)},
					new { Msgaveba=s.Sakheli,	Khariskhi=7, Foto = string.Format("{0} {1} {2} {3} {4}",	s.Pid,s.Fid,s.Sakheli, s.Gvari, s.DabTarigi)},
				}
	where p.Msgaveba != null
	select new {p.Msgaveba, Fotoebi=new[]{new {p.Foto,p.Khariskhi, s.Id}}}
	);

	
var reduce1 = (
	from r in maps
	group r by r.Msgaveba into g
	let fotoebi = g.SelectMany (x => x.Fotoebi).ToList()
	select new {
				Msgaveba = g.Key,
				Fotoebi = fotoebi, 
				Kavshirebi = fotoebi
								.SelectMany (
											  f1 => fotoebi.Where (f2 => f1.Id != f2.Id)
											      			.Select (f2 => new { 	From = f1.Id, 
																					f1.Khariskhi,
																					Distance = LevenshteinDistance(f1.Foto,f2.Foto), 
																					To = f2.Id
																			}
																	)
											)
								.GroupBy (x  => new { x.From, x.To })
								.Select  (g2 => g2.OrderBy(x => x.Distance).OrderBy (x  => x.Khariskhi).First())
				}
			)
	//.Dump()
	;
var maps2=
	(from m in reduce1
	from k in m.Kavshirebi
	select new {k.From,k.Khariskhi,k.Distance,k.To}
	);
var reduce2=
	(from r in maps2
	 group r by r.From into g
	 from gk in g
	 where 
	 select new {g.Key.From,})
	
	var st1="0123456789";
	var st2="0123456789";

	var d = (double)LevenshteinDistance(st1,st2)		.Dump();

	(1- d / ( st1.Length) )					.Dump();

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