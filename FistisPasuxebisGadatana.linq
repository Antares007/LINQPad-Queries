<Query Kind="Statements">
  <Connection>
    <ID>45833638-ad3b-4b74-ad51-3184b7afbd7a</ID>
    <Persist>true</Persist>
    <Driver>LinqToSql</Driver>
    <Server>triton</Server>
    <CustomAssemblyPath>C:\Temp\ClassLibrary3\ClassLibrary3\bin\Debug\ClassLibrary3.dll</CustomAssemblyPath>
    <CustomTypeName>ClassLibrary3.CustomSocDazgvevaDataContext</CustomTypeName>
    <SqlSecurity>true</SqlSecurity>
    <Database>SocialuriDazgveva</Database>
    <UserName>DarigebaDzebnisSamsaxuri</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA3XCANzX8A0+HtkxmZtXFGAAAAAACAAAAAAAQZgAAAAEAACAAAAD+ZsZvKoa4DrLJhG9tCzXWhjtCv48LvJsIivHmOBF5wwAAAAAOgAAAAAIAACAAAABPkJWWnUeORKNVmkaG+FTUF4jBjGZpCpIqmV9AsGOxcxAAAAB2Oh1GL7WTOTfA2eg2BtI0QAAAAA2d/39Fnh2p1+56KXdbJfvqo5EYPLCcvcL7ATAWmyGB+Bkr+ffceLZ4oCWkCel6YLeIMv6JmGdptoTfZ6ymTLM=</Password>
  </Connection>
  <Reference>C:\Temp\EPPlus.dll</Reference>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

var gadasacemi = Polisebi
	.Where (p => p.ShekmnisTarigi.Date > new DateTime(2012,01,10))
	.Where (p => !p.MovlenebiGadaecaRaions.Any ())
	.Where (p => !p.PolisisChabarebisIstoriebi.Any(pci => pci.PaketisChabarebisIstoria.Chabarda))
	.AsEnumerable()
	.Select (p => new DasarigebeliPolisi{PolisisNomeri=p.PolisisNomeri,Dasarigebeli=6,Tarigi=DateTime.Today})
	.ToList();
	
gadasacemi.Count.Dump();

foreach (var g in gadasacemi)
{
	var delete = DasarigebeliPolisebi.FirstOrDefault (dp => dp.PolisisNomeri==g.PolisisNomeri);
	if(null != delete)
		DasarigebeliPolisebi.DeleteOnSubmit(delete);
	
}
DasarigebeliPolisebi.InsertAllOnSubmit(gadasacemi);

SubmitChanges();