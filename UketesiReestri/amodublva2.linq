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
</Query>

void Main()
{
	//typeof(Unnoms).GetFields().Select (x => string.Format("{0} {1}",x.FieldType.Name,x.Name)).Dump();return;
	var ucs = UnnomisConventoris.Where (uc => uc.NewUnnom==9389286);
			ucs.Select (uc => new {uc.OldUnnom,uc.NewUnnom})
	.Concat(ucs.Select (uc => new {OldUnnom=uc.NewUnnom,NewUnnom=uc.NewUnnom*1}).Distinct())
	.SelectMany (uc => Unnoms.Where (u => u.Unnom==uc.OldUnnom).Select (u => new {u, uc.NewUnnom} ))
	
	.GroupBy (uc => uc.NewUnnom)
	.OrderByDescending (g => g.Count ())
	.Take(1)
	
	.Select (g => g.Select (x => x.u).OrderBy (x => x.PID).Select (x => new{x.Unnom,x.NewUnnom,x.PID}).GroupBy (x => x.PID))
	
	.Dump()
	;
	
	
}