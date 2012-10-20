<Query Kind="Statements">
  <Connection>
    <ID>c8afd521-dd29-4636-a948-8f39c543719c</ID>
    <Persist>true</Persist>
    <Server>.\mssqlserver12</Server>
    <Database>Gela</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>C:\Temp\EPPlus.dll</Reference>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

var shesArt = Sheskidvebis.Select (s => s.Artikuli).Distinct();
var gakArt=Gakidvebis.Select (g => g.Artikuli).Distinct();

shesArt.Where (sa => gakArt.All (ga => ga!=sa));

shesArt.Count ( );
gakArt.Count ( );

Gakidvebis.Where (g => Prais.All (p => p.Artikuli!=g.Artikuli)).AsEnumerable()
.GroupBy (g => g.Artikuli)
.Select (g => g.First ())
.Dump();

Prais.Where (p => gakArt.Any (a => a==p.Artikuli))
.GroupBy (p => p.Strixkodi)
.Where (g => g.Count ()>1)
.SelectMany (x => x);

var prais = Prais.ToList();
var misacerebi=Gakidvebis.Where (s => Prais.All (p => p.Strixkodi!=s.Strixkodi) ).Distinct()//.Where (s => s.Strixkodi.Contains("#"))
				.ToList()
.Dump()
;

var uechveli=misacerebi.SelectMany (m => prais.Where (p => p.Artikuli.Contains(m.Artikuli.Trim()) && p.Produqti==m.Produqtisdasaxeleba)
											.Select (p => new{Sheskidva=m,m.ID,m.Artikuli,ArtikuliPrice=p.Artikuli,p.Strixkodi,m.Produqtisdasaxeleba,p.Produqti}))
.ToList()

.OrderBy (m => m.ID)
.Dump()
;

//foreach (var x in uechveli)
//{
//	x.Sheskidva.Strixkodi=x.Strixkodi;
//	x.Sheskidva.Artikuli=x.ArtikuliPrice;
//}
//SubmitChanges();

Sheskidvebis.Where (s => Prais.All (p => p.Strixkodi!=s.Strixkodi ) && Prais.Any(p => p.Artikuli==s.Artikuli ))
.SelectMany (s => Prais.Where (p => p.Artikuli==s.Artikuli).Select (p => new{s,p}))

.Dump();

var sheskidvebis=Gakidvebis.ToList();
var prais=Prais.ToList();
sheskidvebis
.Where (s => !s.Strixkodi.Contains("Bar") && !s.Strixkodi.Contains("bar"))
.GroupBy (s => new {s.Strixkodi,s.Artikuli})
.Select (g => g.First ())
.SelectMany (s => prais.Where (p => p.Strixkodi==s.Strixkodi && p.Artikuli!=s.Artikuli)
							.SelectMany (p => prais.Where (pr => pr.Artikuli==s.Artikuli).Select (pr => new {s,p,pr})
							))
.Where (x => x.p.Strixkodi!=x.pr.Strixkodi || x.p.Gasayidifasil!=x.pr.Gasayidifasil)
.Dump();


