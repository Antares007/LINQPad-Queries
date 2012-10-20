<Query Kind="Expression">
  <Connection>
    <ID>e9890aa3-87d6-47dd-aa04-7445086a72ab</ID>
    <Persist>true</Persist>
    <Server>.\SQLEXPRESS</Server>
    <Database>DarigebaMovlenebi</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>C:\Temp\EPPlus.dll</Reference>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

Commits
	.Select (c => new {c,Payload=Encoding.UTF8.GetString(c.Payload.ToArray())})
	.AsEnumerable()
	.Where (c => c.Payload.Contains("F0"))
	.Take(100)