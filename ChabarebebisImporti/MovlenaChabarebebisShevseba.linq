<Query Kind="Statements">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAAAD89x4SL38S/4r7NUU2iHNNTcmnVTi1xQPx1AC1vAtFAAAAAAOgAAAAAIAACAAAABgO+xVBio0IKzceIXWbWfgFv0jcxQpOA9YilhDtPA8XxAAAABJcM5+MLInsGd5jUUGfXtnQAAAALHy7sVof5cKLhfpSHxbLdPESALwKOWLElOgeYcZmaeqO1sDG9SHnPN5xOzSlBHlSD7agk+KC9dH/4XMZn4gYvM=</Password>
    <Database>SocialuriDazgveva</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

var chabarda=(	from p in PolisisChabarebisIstorias.Where (p => p.Polisebi.MovlenaChabarebebi == null && p.Statusi == "Chabarda")
				group p by new{p.PolisisNomeri,p.Polisebi.ShekmnisTarigi } into g
				let minViz=g.Min (x => x.VizitisTarigi)
				let i=g.Where (x => x.VizitisTarigi==minViz).First ()
				select new {g.Key.PolisisNomeri, 
							i.PaketisNomeri,
							i.VizitisTarigi, 
							g.Key.ShekmnisTarigi,i.Chambarebeli}
			).Take(70000).AsEnumerable();
chabarda.Count().Dump();

MovlenaChabarebebis.InsertAllOnSubmit(
	chabarda.AsEnumerable().Select (x => new MovlenaChabarebebi
					{
						PolisisNomeri=x.PolisisNomeri,
						PaketisNomeri=x.PaketisNomeri,
						ChabarebisTarigi=x.VizitisTarigi.Date,
						Chambarebeli=x.Chambarebeli,
						Dro=DateTime.Today,
					}).ToList()
);

SubmitChanges();