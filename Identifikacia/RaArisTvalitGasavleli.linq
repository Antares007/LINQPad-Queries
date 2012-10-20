<Query Kind="Expression">
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

Source_Data
	.Where (sd => sd.MapDate>DateTime.Parse("2012-05-02"))
 	.Where (sd => sd.Unnom == null)
	.Where (sd => VStrukturuladValiduriGanackhadebi.Any (vsvg => vsvg.ID==sd.ID))

	.GroupBy (sd => new {sd.Base_Type,sd.Pirvelckaro,HasJ_ID = sd.J_ID.HasValue,HasUnnom=sd.Unnom.HasValue})
	.Select (g => new {g.Key.Base_Type,g.Key.Pirvelckaro,g.Key.HasJ_ID,g.Key.HasUnnom,Raod=g.Count(),Li=g.Take(10)})
	.OrderBy (x => x.HasJ_ID)
	.OrderBy (x => x.HasUnnom)
	.OrderBy (x => x.Pirvelckaro)