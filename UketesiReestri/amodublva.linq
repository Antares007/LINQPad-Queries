<Query Kind="Statements">
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

var saechvoDublebi =  UnnomShesadarebeliReestris
							.Select (usr => new {usr.Unnom,usr.PID,usr.IdentPID})
							.Distinct()
							.GroupBy (usr => usr.Unnom)
							.Where (g => g.Any (x => x.IdentPID != null))
							.Where (g => g.Count ()>1)
							.Select (g => new {Unnom = g.Key, Raod=g.Count ()});

(
	from sd in saechvoDublebi
	from r in UnnomShesadarebeliReestris
	where sd.Unnom == r.Unnom
	group r by r.Unnom into g
	orderby g.Count () descending
	select g
).Dump();