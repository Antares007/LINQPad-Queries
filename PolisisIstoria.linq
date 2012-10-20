<Query Kind="Expression">
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

Polisebi.Where (p => p.PolisisNomeri=="000014818")
.Select (p => new{
p,
p.PolisisStatusisCvlilebebi,
p.DasarigebeliPolisi,
p.MovlenebiGadaecaFostas,
p.MovlenebiGadaecaRaions,
p.MovlenebiDaibechda,
p.MovlenaChabarda,
p.PolisisChabarebisIstoriebi
})