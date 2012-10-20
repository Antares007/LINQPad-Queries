<Query Kind="Statements">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAAAD89x4SL38S/4r7NUU2iHNNTcmnVTi1xQPx1AC1vAtFAAAAAAOgAAAAAIAACAAAABgO+xVBio0IKzceIXWbWfgFv0jcxQpOA9YilhDtPA8XxAAAABJcM5+MLInsGd5jUUGfXtnQAAAALHy7sVof5cKLhfpSHxbLdPESALwKOWLElOgeYcZmaeqO1sDG9SHnPN5xOzSlBHlSD7agk+KC9dH/4XMZn4gYvM=</Password>
    <Database>SocialuriDazgveva</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

var q = "60001121273";  //61254001618 - Maiko
var f = Polisebis.Where( p => p.PolisisNomeri == q || p.PiradiNomeri == q || p.Fid==q || p.PolisisNomeri==q || p.UnikaluriKodi == Int64.Parse(q))
				.Select (p => p.Fid)
				.FirstOrDefault ();
var fid = string.IsNullOrWhiteSpace(f) ? "Error" : f;

Polisebis
	.Where( p => p.PolisisNomeri == q || p.PiradiNomeri == q || p.Fid==fid || p.PolisisNomeri==q || p.UnikaluriKodi == Int64.Parse(q))
	.GroupBy (p => new {p.ShekmnisTarigi, p.PolisisStatusi})
	.Select (g => new {
				g.Key.ShekmnisTarigi,
				g.Key.PolisisStatusi,
				Polisebi= g.Select (x => new {
											Polisebi = x,
											Istoriebi = x.PolisisChabarebisIstorias.ToList(),
											Statusebi = x.PolisisStatusisCvlilebebis,
											Dasarigebeli= x.DasarigebeliPolisebi,
											x.GadaecaFostas,
											x.GadaecaRaions,
											x.Daibechdas,
											x.MovlenaChabarebebi
											})})
	.Dump();