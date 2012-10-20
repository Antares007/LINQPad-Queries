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
	var araIdent=new []{3,4,6,7,11,12}.ToList();
	var q1=Source_Data
			//.Where (sd => sd.PID != null && sd.PID.Length > 3)
			.Select (sd => new {Dist=-1,sd.Pirvelckaro,sd.Base_Type,sd.J_ID,sd.ID,sd.PID,sd.First_Name,sd.Last_Name,sd.Birth_Date,sd.Dacesebuleba,sd.Unnom,sd.UnnomisKhariskhi,sd.Source_Rec_Id})
			.Where (sd => sd.First_Name != null && sd.Last_Name != null && sd.First_Name.Length >= 1 && sd.Last_Name.Length >= 1)
			.AsEnumerable();

	var q3 = q1.Select (sd => new {
									sd,
									keys=new[]{sd.First_Name.Substring(0,1), sd.Last_Name.Substring(0,1)},
									Attribs=(new string[]{  
															sd.First_Name, sd.Last_Name, sd.PID, sd.Birth_Date.HasValue ? sd.Birth_Date.Value.ToString("yyyyMMdd"):null
														 }).Where (x => !string.IsNullOrWhiteSpace(x)).ToList() 
						})
				.Where (sd => sd.Attribs.Count>0)
				.ToList();
	q3.Count.Dump();

	q3.AsParallel()
		.Where (q => q.sd.ID==4760939)
		//.Where (q => q.sd.Source_Rec_Id > 0 && araIdent.Contains(q.sd.Base_Type) && !q.sd.J_ID.HasValue)
				.Select(s1 => new { s1.sd, msgavsebi= q3.AsParallel()
														.Where (s2 => s1.sd.ID != s2.sd.ID && s1.keys.SelectMany (k1 => s2.keys.Where (k2 => k1==k2)).Any ())
														.Select(s2 => new { s2.sd, Dist=s1.Attribs.Select (a1 => s2.Attribs.Select (a2 => LevenshteinDistance(a1,a2)).Min ()).Sum ()})
														.Where (x => x.Dist<=20)
														.OrderBy (x=>x.Dist)
														.Take(10)
														.Select (x => new{x.Dist,x.sd.Pirvelckaro,x.sd.Base_Type,x.sd.J_ID,x.sd.ID,x.sd.PID,x.sd.First_Name,x.sd.Last_Name,x.sd.Birth_Date,x.sd.Dacesebuleba,x.sd.Unnom,x.sd.UnnomisKhariskhi,x.sd.Source_Rec_Id}).ToList()
							}
							)
			.Where (x => x.msgavsebi.Count ()>1)
			.Where (x => !x.msgavsebi.Take(4).Where (m => m.Source_Rec_Id<0).Any(m => m.UnnomisKhariskhi==1 && m.Unnom==x.sd.Unnom))
			.Select (x => (new[]{x.sd}).Concat( x.msgavsebi))
			.Dump();

	return;
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