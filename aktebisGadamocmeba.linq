<Query Kind="Statements">
  <Connection>
    <ID>e816bc3b-05af-4eb1-a9da-d5a61bf4c2aa</ID>
    <Persist>true</Persist>
    <Driver Assembly="RavenLinqpadDriver" PublicKeyToken="585b2b0c3c4c2d89">RavenLinqpadDriver.RavenDriver</Driver>
    <CustomAssemblyPathEncoded>&lt;CommonApplicationData&gt;\LINQPad\Drivers\DataContext\4.0\RavenLinqpadDriver (585b2b0c3c4c2d89)\RavenLinqpadDriver.dll</CustomAssemblyPathEncoded>
    <CustomAssemblyPath>C:\ProgramData\LINQPad\Drivers\DataContext\4.0\RavenLinqpadDriver (585b2b0c3c4c2d89)\RavenLinqpadDriver.dll</CustomAssemblyPath>
    <CustomTypeName>RavenLinqpadDriver.RavenContext</CustomTypeName>
    <DriverData>
      <RavenConnectionInfo>{"Url":"http://172.17.7.40:8080","DefaultDatabase":null,"ResourceManagerId":null,"Username":null,"Password":null,"IsInDesignMode":false}</RavenConnectionInfo>
    </DriverData>
  </Connection>
  <Reference>C:\Temp\EPPlus.dll</Reference>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

var paNomrebi = @"0001229303
0001229346
0001238027
0001229397
0001229389
0001229362
0001229354
0001229281
0001237942
0001229249
0001226339
0000909432
0000909432
0000909432
0000909424
0000909416
0000909408
0000909394
0000909378
0000909386
0000909440
0001229370
0000922153
".Split('\n').Select (x => x.Trim()).Where (x => x.Length==10).Distinct().ToList();

var paketebi = ((IEnumerable<dynamic>)Session.Load<dynamic>(paNomrebi.Select (x => "paketi/" + x)).ToList())
	.ToDictionary(x=>(string)x.PaketisNomeri);
var aktebi = ((IEnumerable<dynamic>)Session.Advanced
		.LuceneQuery<dynamic>("Temp/I419420972")
		.Where(string.Join(" OR ", paNomrebi.Select (x => string.Format("PaketisNomeri:{0}", x) ))).ToList())
		.ToDictionary(x=>(string)x.PaketisNomeri,v=>v.Akti);

paketebi.Select (p => {
					object akti=null;
					aktebi.TryGetValue(p.Key,out akti);
					return new {Paketi=p.Value,Akti = akti};
			}).Dump();




