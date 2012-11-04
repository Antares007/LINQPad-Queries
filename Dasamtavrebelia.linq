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
  <Reference>C:\Temp\EPPlus.dll</Reference>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

var movlenebiDaregistrirdaChasabarebeliPolisi = Polisebi
	.Where (p => p.ShekmnisTarigi != DateTime.Parse("04.12.2011 0:00:00"))
	.Select(p=> new  { p.PolisisNomeri, p.ShekmnisTarigi, p.ChabarebisBoloVada });
	
var movlenebiGadaecaFostas = Polisebi
		.SelectMany (p => p.PolisisChabarebisIstoriebi
							.Where (pci => pci.Chambarebeli=="Fosta")
							.Select (pci => new {pci.PaketisNomeri,pci.PolisisNomeri,MovlenisTarigi=p.ShekmnisTarigi}))
							.Dump();