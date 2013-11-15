<Query Kind="Program">
  <NuGetReference>Ix-Main</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference>RavenDB.Client</NuGetReference>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Raven.Abstractions.Linq</Namespace>
  <Namespace>Raven.Json.Linq</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

IEnumerable<dynamic> TbcisAmonaceri(string faili){
	var doc = new XmlDocument();
	doc.LoadXml(System.IO.File.ReadAllText(faili).Replace("gemini:", ""));
	dynamic o = new DynamicJsonObject(RavenJObject.Parse(XmlToJSON(doc)));
	return (IEnumerable<dynamic>)o.AccountStatement.Record;
}

void Main()
{
//;
//
var ganvadeba =	(
		from r in TbcisAmonaceri(@"C:\Users\Acho\Downloads\account_statement_GE94TB7943145063600032_01102012_08102013.xml")
		select new {
			Barati = "ganvadeba",
			PaidIn=r.PaidIn == null ? 0m : decimal.Parse(r.PaidIn),
			PaidOut=r.PaidOut == null ? 0m : decimal.Parse(r.PaidOut),
			Balance=decimal.Parse(r.Balance),
			Date = DateTime.Parse(r.Date, new CultureInfo("Ka-ge")),
			DocumentDate = DateTime.Parse(r.DocumentDate, new CultureInfo("Ka-ge")),
			Description= (string)r.Description,
			r.AdditionalInformation
		}
	);
var visa =	(
		from r in TbcisAmonaceri(@"C:\Users\Acho\Downloads\account_statement_GE26TB1143145061122339_01102012_08102013.xml")
		select new {
			Barati = "visa",
			PaidIn=r.PaidIn == null ? 0m : decimal.Parse(r.PaidIn),
			PaidOut=r.PaidOut == null ? 0m : decimal.Parse(r.PaidOut),
			Balance=decimal.Parse(r.Balance),
			Date = DateTime.Parse(r.Date, new CultureInfo("Ka-ge")),
			DocumentDate = DateTime.Parse(r.DocumentDate, new CultureInfo("Ka-ge")),
			Description= (string)r.Description,
			r.AdditionalInformation
		}
	);
	visa
//	.Where (v => !v.Description.ToUpperInvariant().Contains("VISA"))
//	.Concat(ganvadeba)
//	.OrderBy (x => x.Date)
	//.SelectMany (x => x.Description.Split(',').Where (d => d.ToUpperInvariant().Contains("VISA")))
	.SelectMany (x => x.Description.Split(new []{' ', ',', ';'}))
	.GroupBy (x => x)
	.Dump();

}

private static string XmlToJSON(XmlDocument xmlDoc)
{
    StringBuilder sbJSON = new StringBuilder();
    sbJSON.Append("{ ");
    XmlToJSONnode(sbJSON, xmlDoc.DocumentElement, true);
    sbJSON.Append("}");
    return sbJSON.ToString();
}

//  XmlToJSONnode:  Output an XmlElement, possibly as part of a higher array
private static void XmlToJSONnode(StringBuilder sbJSON, XmlElement node, bool showNodeName)
{
    if (showNodeName)
        sbJSON.Append("\"" + SafeJSON(node.Name) + "\": ");
    sbJSON.Append("{");
    // Build a sorted list of key-value pairs
    //  where   key is case-sensitive nodeName
    //          value is an ArrayList of string or XmlElement
    //  so that we know whether the nodeName is an array or not.
    SortedList childNodeNames = new SortedList();

    //  Add in all node attributes
    if( node.Attributes!=null)
        foreach (XmlAttribute attr in node.Attributes)
            StoreChildNode(childNodeNames,attr.Name,attr.InnerText);

    //  Add in all nodes
    foreach (XmlNode cnode in node.ChildNodes)
    {
        if (cnode is XmlText)
            StoreChildNode(childNodeNames, "value", cnode.InnerText);
        else if (cnode is XmlElement)
            StoreChildNode(childNodeNames, cnode.Name, cnode);
    }

    // Now output all stored info
    foreach (string childname in childNodeNames.Keys)
    {
        ArrayList alChild = (ArrayList)childNodeNames[childname];
        if (alChild.Count == 1)
            OutputNode(childname, alChild[0], sbJSON, true);
        else
        {
            sbJSON.Append(" \"" + SafeJSON(childname) + "\": [ ");
            foreach (object Child in alChild)
                OutputNode(childname, Child, sbJSON, false);
            sbJSON.Remove(sbJSON.Length - 2, 2);
            sbJSON.Append(" ], ");
        }
    }
    sbJSON.Remove(sbJSON.Length - 2, 2);
    sbJSON.Append(" }");
}

//  StoreChildNode: Store data associated with each nodeName
//                  so that we know whether the nodeName is an array or not.
private static void StoreChildNode(SortedList childNodeNames, string nodeName, object nodeValue)
{
	// Pre-process contraction of XmlElement-s
    if (nodeValue is XmlElement)
    {
        // Convert  <aa></aa> into "aa":null
        //          <aa>xx</aa> into "aa":"xx"
        XmlNode cnode = (XmlNode)nodeValue;
        if( cnode.Attributes.Count == 0)
        {
            XmlNodeList children = cnode.ChildNodes;
            if( children.Count==0)
                nodeValue = null;
            else if (children.Count == 1 && (children[0] is XmlText))
                nodeValue = ((XmlText)(children[0])).InnerText;
        }
    }
    // Add nodeValue to ArrayList associated with each nodeName
    // If nodeName doesn't exist then add it
    object oValuesAL = childNodeNames[nodeName];
    ArrayList ValuesAL;
    if (oValuesAL == null)
    {
        ValuesAL = new ArrayList();
        childNodeNames[nodeName] = ValuesAL;
    }
    else
        ValuesAL = (ArrayList)oValuesAL;
    ValuesAL.Add(nodeValue);
}

private static void OutputNode(string childname, object alChild, StringBuilder sbJSON, bool showNodeName)
{
    if (alChild == null)
    {
        if (showNodeName)
            sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
        sbJSON.Append("null");
    }
    else if (alChild is string)
    {
        if (showNodeName)
            sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
        string sChild = (string)alChild;
        sChild = sChild.Trim();
        sbJSON.Append("\"" + SafeJSON(sChild) + "\"");
    }
    else
        XmlToJSONnode(sbJSON, (XmlElement)alChild, showNodeName);
    sbJSON.Append(", ");
}

// Make a string safe for JSON
private static string SafeJSON(string sIn)
{
    StringBuilder sbOut = new StringBuilder(sIn.Length);
    foreach (char ch in sIn)
    {
        if (Char.IsControl(ch) || ch == '\'')
        {
            int ich = (int)ch;
            sbOut.Append(@"\u" + ich.ToString("x4"));
            continue;
        }
        else if (ch == '\"' || ch == '\\' || ch == '/')
        {
            sbOut.Append('\\');
        }
        sbOut.Append(ch);
    }
    return sbOut.ToString();
}