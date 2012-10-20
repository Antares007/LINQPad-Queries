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

public IEnumerable<string> GetKeys(Source_Data sd) {
		yield return "0";
}
public IEnumerable<string> GetAttrs(Source_Data sd) {
		yield return sd.First_Name;
		yield return sd.Last_Name;
		yield return sd.PID;
		//yield return sd.Birth_Date.HasValue ? sd.Birth_Date.Value.ToString("yyyyMMdd"):null;
}
void Main()
{
	var ids=new int[]{9030226};
	var sdss=Source_Data.Where (sd => ids.Contains(sd.ID)).Dump();
	foreach (var sd in sdss)
	{
		sd.Unnom=1077549;		
	}
	SubmitChanges();
	sdss.Dump();

	var sds = Source_Data.Where (sd => sd.J_ID.HasValue);
//	var sds = Source_Data.Where (sd => !sd.Birth_Date.HasValue || 
//									   sd.PID == null || sd.First_Name == null || sd.Last_Name == null || 
//									   sd.PID.Contains("არ") || 
//									   sd.First_Name.Contains("უცნობ") || sd.First_Name.Contains("მამრ")  || sd.First_Name.Contains("მდედრ") || 
//									   sd.Last_Name.Contains("უცნობ")  || sd.Last_Name.Contains("მამრ")   || sd.Last_Name.Contains("მდედრ"));
	var sakhelebi=sds	.Where (sd => !sd.Unnom.HasValue)
			.Where (sd => sd.MapDate > DateTime.Parse("2012-04-04")).Select (sd => sd.First_Name.Substring(0,3)).Distinct().ToList();
	var araIdent=new []{3,4,6,7,11,12}.ToList();
	var marjvena = sds.GroupBy (s => s.Unnom)
						.Select (g => g.First (sd => sd.ID==g.Max (x => x.ID)))
						.Where (sd => sd.J_ID.HasValue)
						.Where (sd => sakhelebi.Contains(sd.First_Name.Substring(0,3)))
						.DaadeSustiAttributebi(GetKeys, GetAttrs);
	
	var marchxena =					
		sds	.Where (sd => !sd.Unnom.HasValue)
			.Where (sd => sd.MapDate > DateTime.Parse("2012-04-04"))
			//.Where (sd => araIdent.Contains(sd.Base_Type))
			.DaadeSustiAttributebi(GetKeys, GetAttrs);
	
	marchxena.AsParallel()
				.Select(s1 => new { s1.sd, msgavsebi= marjvena
														.Where (s2 => s1.sd.ID != s2.sd.ID && s1.keys.SelectMany (k1 => s2.keys.Where (k2 => k1==k2)).Any ())
														.Select(s2 => new { s2.sd, Dist=s1.Attribs.Select (a1 => s2.Attribs.Select (a2 => LevenshteinDistance(a1,a2)).Min ()).Sum ()})
														.Where (x => x.Dist<=40)
														.OrderBy (x=>x.Dist)
														.Take(10)
														.Select (x => x.sd)
														
							}
							)
			.Where (x => x.msgavsebi.Count ()>1)
			.Select (x => (new[]{x.sd}).ToList().Concat(x.msgavsebi))
			.Dump();
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
public class Shedarebadi{
	public Source_Data sd {get;set;}
	public List<string> keys {get;set;}
	public List<string> Attribs  {get;set;}
}
public static class ExtendedSd{
	public static List<Shedarebadi> DaadeSustiAttributebi(this IQueryable<Source_Data> sources, Func<Source_Data,IEnumerable<string>> getKeys, Func<Source_Data,IEnumerable<string>> getAttrs){
		return sources.AsEnumerable().Select (sd => new Shedarebadi {
									sd=sd,
									keys=getKeys(sd).Where (x => !string.IsNullOrWhiteSpace(x)).ToList() ,
									Attribs=getAttrs(sd).Where (x => !string.IsNullOrWhiteSpace(x)).SelectMany (x => x.Split(new[]{' ',',','(',')'})).ToList() 
						})
				.Where (sd => sd.Attribs.Count>0)
				.ToList();
	}
}