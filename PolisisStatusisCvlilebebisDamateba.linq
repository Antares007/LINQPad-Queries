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
</Query>

var polNomrebi= @"

001734474
001734482

".Split('\n')
.Select (x => x.Trim())
.Where (x => x.Length == 9)
.ToList();

var polisebi = Polisebi.Where (p => polNomrebi.Contains(p.PolisisNomeri));

foreach(var p in polisebi){
	p.PolisisStatusisCvlilebebi.Add(new PolisisStatusisCvlilebeba{ PolisisNomeri=p.PolisisNomeri, Tarigi=new DateTime(2011,12,20), Safudzveli="KhelitKaaktiureba", Statusi="Aktiuri"});
	p.PolisisStatusi="Aktiuri";
}
polisebi.Dump();
SubmitChanges();