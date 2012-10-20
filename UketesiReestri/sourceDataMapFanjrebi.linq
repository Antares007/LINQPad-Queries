<Query Kind="Program">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAAAD89x4SL38S/4r7NUU2iHNNTcmnVTi1xQPx1AC1vAtFAAAAAAOgAAAAAIAACAAAABgO+xVBio0IKzceIXWbWfgFv0jcxQpOA9YilhDtPA8XxAAAABJcM5+MLInsGd5jUUGfXtnQAAAALHy7sVof5cKLhfpSHxbLdPESALwKOWLElOgeYcZmaeqO1sDG9SHnPN5xOzSlBHlSD7agk+KC9dH/4XMZn4gYvM=</Password>
    <Database>Pirvelckaroebi</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>System</Namespace>
  <Namespace>System.Linq</Namespace>
</Query>

void Main()
{
	var chanacerebi =
		Source_Data.Where (sd => sd.Source_Rec_Id>0)
			.Select (sd => new Chanaceri{MapDate=sd.MapDate,Id=sd.ID,BaseType=sd.Base_Type})
			.AsEnumerable()
			.OrderBy (sd => sd.MapDate)
			.ToList();

	var rez = chanacerebi.Aggregate (new Fanjerebi(), (fanjrebi, chanaceri) => {
			if(fanjrebi.BoloChanaceri == null || (chanaceri.MapDate - fanjrebi.BoloChanaceri.MapDate).TotalSeconds>2 )
				fanjrebi.GakhseniAkhaliFanjara();
			fanjrebi.Daamate(chanaceri);
			return fanjrebi; })
	.Where (f => f.Count () < 50)
	.Dump();

	var ids = rez.SelectMany (r => r.Select (x => x.Id)).ToList();
	Source_Data
		.Where (sd => ids.Contains(sd.ID))
		.OrderBy (sd => sd.MapDate)
		.Dump();
}

public class Fanjerebi :List<List<Chanaceri>>{
	public Chanaceri BoloChanaceri;
	
	private List<Chanaceri> boloFanjara ;
	
	public void GakhseniAkhaliFanjara(){
		boloFanjara=new List<Chanaceri>();
		this.Add(boloFanjara);
	}
	
	public void Daamate(Chanaceri chan){
		BoloChanaceri=chan;
		boloFanjara.Add(chan);
	}
}
public class Chanaceri{
	public DateTime MapDate;
	public int Id;
	public int BaseType;
}