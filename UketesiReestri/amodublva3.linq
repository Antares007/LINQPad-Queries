<Query Kind="Program">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAAAD89x4SL38S/4r7NUU2iHNNTcmnVTi1xQPx1AC1vAtFAAAAAAOgAAAAAIAACAAAABgO+xVBio0IKzceIXWbWfgFv0jcxQpOA9YilhDtPA8XxAAAABJcM5+MLInsGd5jUUGfXtnQAAAALHy7sVof5cKLhfpSHxbLdPESALwKOWLElOgeYcZmaeqO1sDG9SHnPN5xOzSlBHlSD7agk+KC9dH/4XMZn4gYvM=</Password>
    <Database>UketesiReestri</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference Relative="..\..\Visual Studio 2012\Projects\UketesiReestri\UketesiReestri\bin\Debug\UketesiReestri.dll">&lt;MyDocuments&gt;\Visual Studio 2012\Projects\UketesiReestri\UketesiReestri\bin\Debug\UketesiReestri.dll</Reference>
  <Namespace>Ur</Namespace>
</Query>

void Main()
{
	var snapshots = (from us in UnnomSnapshots
	                from c  in UnnomisConventoris
					where c.OldUnnom == us.Unnom || c.NewUnnom == us.Unnom
					select us)
					.AsEnumerable()
					.GroupBy (x => x.Unnom)
					.ToDictionary(k => k.Key, g => g.Select (v => new Ur.Chanaceri(v.Base_Type,v.Dacesebuleba,v.FID,v.PID,v.First_Name,v.Last_Name,v.Birth_Date,v.Tarigi,v.IdentPID)));
					
	var ucs = UnnomisConventoris;//.Where (uc => uc.NewUnnom==9384672);
	
	var a = ucs	.Select (uc => new {uc.OldUnnom, uc.NewUnnom})
				.Concat (ucs.Select (uc => new {OldUnnom=uc.NewUnnom, NewUnnom=uc.NewUnnom*1}).Distinct())
				.SelectMany (uc => Unnoms.Where (u => u.Unnom==uc.OldUnnom).Select (u => new {u, uc.NewUnnom} ))
				.AsEnumerable()
				.Select(x => new { x.u, us = snapshots[x.u.Unnom], x.NewUnnom})
				.GroupBy (uc => uc.NewUnnom)
				.Select (g => {
							var unms = g.Select (x => new {
									x.u.Unnom,
									NewUnnom=x.u.Amoidubla.HasValue ? x.u.Amoidubla:x.u.NewUnnom, 
									Unnomi = new Ur.Unnom(x.u.Unnom, new Ur.Chanaceri(x.u.Base_Type,x.u.Dacesebuleba,x.u.FID,x.u.PID,x.u.First_Name,x.u.Last_Name,x.u.Birth_Date,x.u.Tarigi,x.u.IdentPID), x.us.ToList())
									}).ToDictionary (x => x.Unnom);
							
							foreach(var e in unms.Where (x => x.Value.NewUnnom.HasValue ).Select (x => x.Value))
							{
								e.Unnomi.Gaduble(unms[e.NewUnnom.Value].Unnomi);
							}
							return unms.First (u => !u.Value.NewUnnom.HasValue).Value.Unnomi;
						})
				.Where (u => u.SaechvoDublebi().Count()>0)
				.OrderByDescending (u => u.SaechvoDublebi().Count())
				//.SelectMany (u => u.SaechvoDublebi())
				.ToList ().Dump();
				;
	//a.SelectMany (x => new []{x.Item1,x.Item2}).Select (x => x.Value).Distinct().Dump();
}