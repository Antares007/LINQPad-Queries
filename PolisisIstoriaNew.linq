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

var nomeri="000001368";

MovlenebiChabarda.Where(x=>x.PolisisNomeri==nomeri).Dump();
MovlenebiGaformdaGaukmdaKontrakti.Where(x=>x.PolisisNomeri==nomeri).Dump();

   var viziti = PolisisChabarebisIstoriebi.Select(x => new {x.PolisisNomeri, Movlena="GankhorcieldaViziti", x.Chambarebeli, x.PaketisNomeri, Shenishvna=x.Statusi, Tarigi=x.VizitisTarigi});
 var gadaecaR = MovlenebiGadaecaRaions    .Select(x => new {x.PolisisNomeri, Movlena="Gadaeca",Chambarebeli="Raioni",PaketisNomeri=(string)null,Shenishvna=(string)null,Tarigi=x.GadacemisTarigi});
var daibechda = MovlenebiDaibechda        .Select(x => new {x.PolisisNomeri, Movlena="Daibechda", Chambarebeli="Raioni", x.PaketisNomeri, Shenishvna=(string)null, Tarigi=x.GadacemisTarigi});
 var gadaecaF = MovlenebiGadaecaFostas    .Select(x => new {x.PolisisNomeri, Movlena="Gadaeca", Chambarebeli="Fosta",x.PaketisNomeri,Shenishvna=(string)null,Tarigi=x.GadacemisTarigi});

viziti.Concat(gadaecaR).Concat(gadaecaF).Concat(daibechda).Where (x => x.PolisisNomeri==nomeri)
	.OrderBy (x => x.Tarigi).Dump();