<Query Kind="Program">
  <NuGetReference>CsQuery</NuGetReference>
  <NuGetReference>HtmlAgilityPack</NuGetReference>
  <NuGetReference>Microsoft.AspNet.WebApi.Client</NuGetReference>
  <Namespace>HtmlAgilityPack</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>CsQuery</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{ 
//	CQ dom0 = "<html></html>";
	var next = "http://stackoverflow.com/users?tab=reputation&filter=all";
	while (true)
	{
		
		var r = new HttpClient().GetAsync(next).Result.Content.ReadAsStringAsync().Result;
		CQ dom = r;
		
		var userInfos = dom["div[class*='user-details']"]
							.Select (x => {
								var d = x.Cq();
								return new {
									a = d.Children ("a").Text(),
									l = d.Children ("span[class='user-location']").Text(),
									r = d.Children ("span[class='reputation-score']").Text(),
								};
							});
		userInfos.Count ().Dump();
		
		var nextLink = dom["a[rel='next']"].FirstOrDefault();
		if(nextLink == null) break;
		next = "http://stackoverflow.com"+nextLink.GetAttribute("href");
		next.Dump();
	}
}
public static class Ex
{
	public static HtmlNodeCollection Dump( HtmlNodeCollection src)
	{
		src.Select (s => s.ToXElement()).Dump();
		return src;
	}
	public static XElement ToXElement(this HtmlNode src)
	{
		var sb = new StringBuilder();
		src.WriteTo(new StringWriter(sb));
		return XElement.Parse(sb.ToString());
	}
	public static HtmlNode Dump( HtmlNode src)
	{
		src.ToXElement().Dump();
		return src;
	}
}
private XDocument RemoveNamespace(XDocument xdoc)
{
	foreach (XElement e in xdoc.Root.DescendantsAndSelf())
	{
		if (e.Name.Namespace != XNamespace.None)
      	{
        	e.Name = XNamespace.None.GetName(e.Name.LocalName);
      	}
      	if (e.Attributes().Where(a => a.IsNamespaceDeclaration || a.Name.Namespace != XNamespace.None).Any())
      	{
        	e.ReplaceAttributes(e.Attributes().Select(a => a.IsNamespaceDeclaration ? null : a.Name.Namespace != XNamespace.None ? new XAttribute(XNamespace.None.GetName(a.Name.LocalName), a.Value) : a));
      	}
  	}
	return xdoc;
}