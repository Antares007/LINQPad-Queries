<Query Kind="Program">
  <Connection>
    <ID>017788df-d9dd-4969-97fd-2fc9007eeae6</ID>
    <Persist>true</Persist>
    <Server>MegaMozg</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAALfKU1NXA60ORp4O7hQvFoAAAAAACAAAAAAAQZgAAAAEAACAAAABMeezKjaWaUcEjyUsFMOHDvCD/d61Cv5PvcMIcMqSpCQAAAAAOgAAAAAIAACAAAADAlTSsOo1g+bcilyK8tf2gthLC9N6zDiqu2f0VrjGiuRAAAABFnsPhAx9h3VhCY9rJ1gQyQAAAAHXtraBFbB949Ee34bXZvn1xESe5WlsHd1cxLm3zq18Zb5F079zPUyL7b0sV+PjHuPS51hxww/2cvcQl3qEDhog=</Password>
    <Database>Pirvelckaroebi</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

void Main()
{
	var unnomMinichebuli=Source_Data.WhereUnnomMinichebuli();
	var kargiKavshirebi=Source_Data_Kavshiris.Where (sdk => sdk.From>sdk.To).WhereSauketesoKavshirebi().WhereEgAris();

	(from sd1 in unnomMinichebuli
	 from sd2 in unnomMinichebuli
	 
	 where sd1.Unnom==sd2.Unnom && sd1.ID>sd2.ID
	 where !kargiKavshirebi.Any(k => k.From == sd1.ID && k.To == sd2.ID)
	 select new {sd1,sd2}
	 )
	.Take(10)
	.Dump();
}

public static class QueryEx
{
	private static IQueryable<Source_Data> WhereGansakhilveli(this IQueryable<Source_Data> source)
	{
		var araIdent=new []{ 3, 4, 6, 7, 11, 12 }.ToList();
		return source.Where (sd => araIdent.Contains(sd.Base_Type) || sd.J_ID.HasValue)
					 .Where (sd => 0 < sd.Base_Type && sd.Base_Type <= 13 || sd.Base_Type == 99);
	}
	public static IQueryable<Source_Data> WhereUnnomMinichebuli(this IQueryable<Source_Data> source)
	{
		return source.WhereGansakhilveli().Where (sd => sd.Unnom.HasValue && sd.UnnomisKhariskhi==1);
	}
	public static IQueryable<Source_Data> WhereUnnomMisanichebeli(this IQueryable<Source_Data> source)
	{
		return source.WhereGansakhilveli().Where (sd => !sd.Unnom.HasValue);
	}
	public static IQueryable<Source_Data> WhereMimdinarePeriodi(this IQueryable<Source_Data> source)
	{
		return source.Where (sd => sd.MapDate > DateTime.Parse("2012-05-02"));
	}
	public static IQueryable<Source_Data> WhereArUkavshirdebaAravis(this IQueryable<Source_Data> source)
	{
		return source.Where (sd => !sd.FromSource_Data_Kavshiris.Any());
	}
	public static IQueryable<Source_Data_Kavshiri> WhereEgAris(this IQueryable<Source_Data_Kavshiri> source)
	{
		var kargiKhariskhi=new[]{2, 4, 8, 512, 2048, 4096, 8192, 131072, 262144, 524288};
		return source.Where (k => 
			   k.FromSource_Data.J_ID.HasValue && k.Source_Data.J_ID.HasValue && k.FromSource_Data.PID == k.Source_Data.PID
			|| kargiKhariskhi.Contains(k.FromSauketesoKhariskhi)
		);
	}
	public static IQueryable<Source_Data_Kavshiri> WhereUnomtanKavshiri(this IQueryable<Source_Data_Kavshiri> source)
	{
		return source.Where     (k => k.Source_Data.UnnomisKhariskhi <= 4)
		
					.GroupBy    (k => k.From)
					.Select     (gFrom => new {ks=gFrom, minFromSauketesoKhariskhi=gFrom.Min (x => x.FromSauketesoKhariskhi)})
					.SelectMany (gFrom => gFrom.ks.Where (k => k.FromSauketesoKhariskhi == gFrom.minFromSauketesoKhariskhi)
				
						.GroupBy    (k => k.FromSauketesoKhariskhi)
						.Select     (gFromSauketesoKhariskhi => new {ks=gFromSauketesoKhariskhi, minUnnomisKhariskhi = gFromSauketesoKhariskhi.Min (x => x.Source_Data.UnnomisKhariskhi) })
						.SelectMany (gFromSauketesoKhariskhi => gFromSauketesoKhariskhi.ks.Where (k => k.Source_Data.UnnomisKhariskhi == gFromSauketesoKhariskhi.minUnnomisKhariskhi)
				
							.GroupBy    (k => k.Source_Data.UnnomisKhariskhi)
							.Select     (gUnnomisKhariskhi => new {ks=gUnnomisKhariskhi,minUnnom=gUnnomisKhariskhi.Min (k => k.Source_Data.Unnom), maxUnnom=gUnnomisKhariskhi.Max (k => k.Source_Data.Unnom)})
							.Where      (gUnnomisKhariskhi => gUnnomisKhariskhi.minUnnom==gUnnomisKhariskhi.maxUnnom || gUnnomisKhariskhi.ks.Key > 2)
							.SelectMany (gUnnomisKhariskhi => gUnnomisKhariskhi.ks.Where (k => k.Source_Data.Unnom==gUnnomisKhariskhi.minUnnom )
								
								.GroupBy    (k => k.Source_Data.Unnom)
								.Select     (gUnnom => new {ks=gUnnom, minTo=gUnnom.Min (k => k.To)})
								.SelectMany (gUnnom => gUnnom.ks.Where (k => k.To==gUnnom.minTo)
									.Select (k => k)
								)
							)
						)
					);
	}
	public static IQueryable<Source_Data_Kavshiri> WhereSauketesoKavshirebi(this IQueryable<Source_Data_Kavshiri> source)
	{
		return source
			.Where     (k => k.Source_Data.UnnomisKhariskhi <= 4)

			.GroupBy    (k => k.From)
			.Select     (gFrom => new {ks=gFrom, minFromSauketesoKhariskhi=gFrom.Min (x => x.FromSauketesoKhariskhi)})
			.SelectMany (gFrom => gFrom.ks.Where (k => k.FromSauketesoKhariskhi == gFrom.minFromSauketesoKhariskhi)
		
				.GroupBy    (k => k.FromSauketesoKhariskhi)
				.Select     (gFromSauketesoKhariskhi => new {ks=gFromSauketesoKhariskhi, minUnnomisKhariskhi = gFromSauketesoKhariskhi.Min (x => x.Source_Data.UnnomisKhariskhi) })
				.SelectMany (gFromSauketesoKhariskhi => gFromSauketesoKhariskhi.ks.Where (k => k.Source_Data.UnnomisKhariskhi == gFromSauketesoKhariskhi.minUnnomisKhariskhi)
				)
			);

	}
	public static IQueryable<Source_Data_Kavshiri> SelectManySauketesoKavshirebi(this IQueryable<Source_Data> source)
	{
		return source.SelectMany (s => s.FromSource_Data_Kavshiris)
					.WhereSauketesoKavshirebi();
	}
	
	
}