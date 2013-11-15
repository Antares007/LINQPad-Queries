<Query Kind="Program">
  <NuGetReference>CsQuery</NuGetReference>
  <NuGetReference>Microsoft.Net.Http</NuGetReference>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>CsQuery</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

async void  Main()
{
	
//CQ q=a;

	var parsers = new Dictionary<string, Func<string, CsQuery.IDomObject, Tuple<string, bool, string[], string> >>()
	{
		{ "text", 		(name, o) => Tuple.Create(name, false, new string[]{null}, o.Attributes["value"]) },
		{ "file", 		(name, o) => Tuple.Create(name, false, new string[]{null}, o.Attributes["value"]) },
		{ "hidden", 	(name, o) => Tuple.Create(name, false, new string[]{null}, o.Attributes["value"]) },
		{ "textarea", 	(name, o) => Tuple.Create(name, false, new string[]{null}, o.Attributes["value"]) },

		{ "submit", 	(name, o) => Tuple.Create(name, true, new []{o.Attributes["value"]}, default(string)) },
		{ "checkbox", 	(name, o) => Tuple.Create(name, false, new []{o.Attributes["value"]}, o.Attributes["checked"] == null ? null : o.Attributes["value"] ) },
		{ "radio", 		(name, o) => Tuple.Create(name, true, new []{o.Attributes["value"]}, o.Attributes["checked"] == null ? null : o.Attributes["value"] ) },
		{ "select", 	(name, o) => {
				var opts = o.Cq().Children("option")
									.Select (x_ => new {
												value = x_.Attributes["value"],
												selected = x_.Attributes["selected"] != null
												});
				var fe = Tuple.Create(name, false, opts.Select(x => x.value).ToArray(), opts.First(x => x.selected).value);
				return fe;
			} },
	};
	CQ q = @"<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
<title>Home</title>
</head>
<body>
<ul>
	<li><span class='id'>1</span><span class='name'>avoee1</span></li>
	<li><span class='id'>2</span><span class='name'>avoee2</span></li>
</ul>
<p><a href='/'>home</a></p><h1>Home</h1>
<form id='airchiePosi' method='post' action='pos'>
<fieldset>
<input type='text' name='posisNomeri' value='' required class='a'>
<input type='submit' value='Submit'>
dsf
</fieldset>
</form>
</body>
</html>";
	q["ul li"].Select (x => x.ChildNodes.Select (cn => cn.Classes)).Dump();
	return;
	var fields = q["form"].First ().Contents()["[name]"]
		.Select (x => parsers[(x.Attributes["type"] ?? x.NodeName).ToLower()](x.Attributes["name"], x))
		.Select (x => new {Name = x.Item1, OneValue = x.Item2, AllowedValues = x.Item3,Value=x.Item4});
    
	var options = fields		
		.SelectMany ((e,i)=> e.AllowedValues.Select ((av) => new {e.Name,AllowedValue=av,Group=e.Name+"_"+(e.OneValue? "0" : i.ToString()) }))
		;
	
	var defaults = fields.Where (f => f.Value != null)
		.Select (f => new {f.Name,f.Value})
		;
	
	var values = new []{
				new {Name="ng3", Value="1"},
				};
	
	(from opt in options
	 from val in values.Concat(defaults).Select((v,i)=>new {v,i})
	 where val.v.Name==opt.Name 
	 where opt.AllowedValue == null || opt.AllowedValue == val.v.Value
	 group val by opt.Group into g
	 select g.OrderBy (x => x.i).First ().v
	 ).Dump();
	 
//		.GroupBy(x => new {x.Item1,x.Item2})
//		.Select (g => new {Name=g.Key.Item1,OneValue=g.Key.Item2,Value=g.First (x=>x.Item4 != null).Item4,AllowedValues=g.SelectMany (x => x.Item3)})
//		.GroupBy(x => x.Name)
//		.Select(g => new NameGroup(g.Key, g))
//		.ToList();
//	1.Dump("-----------");
//	fields.SelectMany (f => f.GetValues().Select(v=>new {f.Name,v})).Dump();
//	fields.First (f => f.Name=="ng1").SetValue("asdasd");
//	fields.SelectMany (f => f.GetValues().Select(v=>new {f.Name,v})).Dump();
	
}