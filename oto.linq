<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.dll</Reference>
  <NuGetReference>CsQuery</NuGetReference>
  <NuGetReference>Ix-Main</NuGetReference>
  <NuGetReference>Microsoft.Net.Http</NuGetReference>
  <Namespace>CsQuery</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Web</Namespace>
</Query>

public static class Ex
{
	public async static Task<HttpRequestMessage> SubmitFormAsync(this Task<HttpResponseMessage> response, string id, IDictionary<string, string> kvs)
	{
		var res = await response;
		CQ content = await res.Content.ReadAsStringAsync();
		var form = content["form#"+id].First ();
		var actionUri = new Uri(res.RequestMessage.RequestUri, form.Attr("action"));
		var method = form.Attr("method") == "post" ? HttpMethod.Post : HttpMethod.Get;
		
		
		var dict = form.Contents()["input"].ToDictionary (k => k.Attributes["name"], v=>v.Attributes["value"]);
		
		foreach (var x in kvs)
		{
			dict[x.Key] = x.Value;
		}
	
		var formData = new FormUrlEncodedContent(dict);
		return new HttpRequestMessage(method, actionUri)
		{
			Content = formData
		};
	}
}

public class StateFullClient
{
	HttpClient _httpClient;
	CookieContainer _cookieContainer;
	
	public StateFullClient(HttpClient httpClient, CookieContainer cookieContainer)
	{
		_httpClient = httpClient;
		_cookieContainer = cookieContainer;
	}
	public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage req)
	{
		req.Headers.Add("Cookie", GetCookies(req.RequestUri));
		var res = await _httpClient.SendAsync(req);
		SetCookies(res);
		return res;
	}
	public async Task<HttpResponseMessage> GetAsync(string uriString)
	{
		var uri = new Uri(uriString);
		var req = new HttpRequestMessage(HttpMethod.Get, uri);
		req.Headers.Add("Cookie", GetCookies(uri));
		var res = await _httpClient.SendAsync(req);
		SetCookies(res);
		return res;
	}
	
	private void SetCookies(HttpResponseMessage res)
	{
		IEnumerable<string> cookies;
		res.Headers.TryGetValues("Set-Cookie", out cookies);
		if(cookies != null)
			foreach(var cookie in cookies)
				_cookieContainer.SetCookies(res.RequestMessage.RequestUri, cookie);
	}
	
	private IEnumerable<string> GetCookies(Uri uri)
	{
		return _cookieContainer
					.GetCookies(uri)
					.Cast<Cookie>()
					.Where (c => !c.Expired)
					.Select (c => c.ToString());
	}
}

async void  Main()
{


return;
	CQ q = @"<form>
		<input type='text' name='ng1' value='1' >
		<input type='checkbox' name='ng1' value='1' >
		<input type='checkbox' name='ng1' value='2' >
		<input type='checkbox' name='ng1' value='3'>
		
		<input type='radio' name='ng2' value='1' checked='checked'>
		<input type='radio' name='ng2' value='2' >
		<input type='radio' name='ng2' value='3' >
		
		<select name='ng3'>
			<option value selected='selected'>Language neutral</option>
			<option value='en'>English</option>
		</select>
	</form>";
//CQ q=a;

q["form input"].Select (x => new {name=x.Attributes["name"],value=x.Attributes["value"]}).Dump();
return;
	
	return;
	
	var sfc = new StateFullClient(new HttpClient(), new CookieContainer());
	
	var r1 = sfc.GetAsync("http://www.elifai.eu/user/login/lightbox2/lightbox2")
				.SubmitFormAsync("user-login-form", new Dictionary<string,string> {{"name","otari"},{"pass","11234"}})
				.Result;
				
	var r2= sfc	.SendAsync(r1)
				.Result
				.Content.ReadAsStringAsync()
				.Result;
	
	 sfc.GetAsync("http://www.elifai.eu/node/add/product")
	 	.SubmitFormAsync("node-form", new Dictionary<string,string>())
		.Result.Content
		.ReadAsStringAsync()
		.Result.Dump();

	
//	
//	hc.GetAsync("http://www.elifai.eu/user/login/lightbox2/lightbox2")
//	  .SubmitFormAsync(hc, "user-login-form", new KeyValuePair<string,string>[]{
//				new KeyValuePair<string,string>("name","otari"),
//				new KeyValuePair<string,string>("pass","11234")
//			})
//		.Result.Dump();
//	
//var cc = new CookieContainer();
//var res = hc.GetAsync("http://www.elifai.eu/user/login/lightbox2/lightbox2").Result;
//
//foreach(var cookie  in res.Headers.GetValues("Set-Cookie"))
//	cc.SetCookies(res.RequestMessage.RequestUri, cookie);
//cc.GetCookies(new Uri("http://www.elifai.eu")).Dump();

//	var setCookie = r.Headers.GetValues("Set-Cookie");
//	
//	CQ q = r.Content.ReadAsStringAsync().Result;
//	
//	var dict=q["input"].ToDictionary (k => k.Attributes["name"],v=>v.Attributes["value"]);
//	dict["name"]="otari";
//	dict["pass"]="11234";
//	
//	//dict.Dump();
//	var formData = new FormUrlEncodedContent(dict);
//	formData.Headers.Add("Cookie", setCookie);
//	
//	r = hc.PostAsync("http://www.elifai.eu/user/login/lightbox2?destination=",formData)
//		.Result;
//	var rm = new HttpRequestMessage(HttpMethod.Get,"http://www.elifai.eu/node/add/product");
//	rm.Headers.Add("Cookie", setCookie);
//	
//	q = hc.SendAsync(rm).Result.Content.ReadAsStringAsync().Result;
//	
//	
//	var form = q.GetForm("node-form")
//				.GetRequestMessage(new KeyValuePair<string,string>[]{});
//	
	
//	q["#node-form"].Filter("[type='hidden']")
//		.Select (x => new {name=x.Attributes["name"],value=x.Attributes["value"],type=x.Attributes["type"],
//		@checked=x.Attributes["checked"],x.NodeName,x.ParentNode.InnerText,Options=x.Cq().Children("option")})
//		
//	.GroupBy (x => new {x.name,type=x.type??x.NodeName})
//	.Select (g => new{g.Key.name,g.Key.type,Values=g.ToArray()})
	
	
	var mfd = new MultipartFormDataContent("aaa");
	mfd.Dispose();
	
//	
//	Product.New("1750017898","Flat belt", 234.234)
//		.Description("Flat belt Description")
//		.Image(@"c:\sadsad\asdsa.jpg")
//		.Category(101,123,12)
//		.Supplier("")
//		.CostCurrency("")
//		.ListPrice(123.12)
//		.Cost(213.123)
//		.Weight(121)
//		.DefaultQuantity(2)
//		.BrandManufacturer(12)
//		.State(71)
//		.UseInProducts(1,2,3)
//		.DeliveryTerms(21)
//		.SpecialComments("");
		
		
		
		
	
}

void ChatLog()
{
	CQ d = File.ReadAllText(@"D:\Downloads\Logs.htm");

	var log = d["tr td"].Select (x => HttpUtility.HtmlDecode(x.InnerText)).Buffer(9)
						.Select (x => new {Date=DateTime.Parse(x[2]), U=x[3], M=x[6], CId=x[7]})
						.OrderBy (x => x.Date)
						.ToList()
						;
	var colors = new []{"red","green","blue"};
	var chatColors = log.Select (x => x.CId).Distinct().Select ((x,i) => new {x,i}).ToDictionary (x => x.x,x=>colors[x.i % colors.Length]);
	
	Func<string,string,string,object> colorize2 = (m,u,col) => 
	u=="deatooo"? Util.RawHtml(string.Format("<div style='color:{1};font-weight:bolder'>{0}</div>", m, col)) :
				Util.RawHtml(string.Format("<div style='color:{1}'>{0}</div>", m, col));
	
	log.Select (l => new {
			Date=colorize2(l.Date.ToString(),l.U,chatColors[l.CId]),
			U=colorize2(l.U,l.U,chatColors[l.CId]),
			M=colorize2(l.M,l.U,chatColors[l.CId]),
			})
			.Skip(10000)
			.Dump();
}
// Define other methods and classes here
string a = @"<form action=""/node/add/product"" accept-charset=""UTF-8"" method=""post"" id=""node-form"" enctype=""multipart/form-data"">
<div>
<div class=""node-form"">
  <div class=""standard"">
<div class=""body-field-wrapper""><div class=""form-item"" id=""edit-teaser-js-wrapper"">
 <textarea class=""form-textarea teaser resizable"" cols=""60"" rows=""10"" name=""teaser_js"" id=""edit-teaser-js"" disabled=""disabled""></textarea>
</div>
<div class=""teaser-checkbox""><div class=""form-item"" id=""edit-teaser-include-wrapper"">
 <label class=""option"" for=""edit-teaser-include""><input class=""form-checkbox"" type=""checkbox"" name=""teaser_include"" id=""edit-teaser-include"" value=""1"" checked=""checked"" /> Show summary in full view</label>
</div>
</div><div class=""form-item"" id=""edit-body-wrapper"">
 <label for=""edit-body"">Description: </label>
 <textarea class=""form-textarea resizable"" cols=""60"" rows=""20"" name=""body"" id=""edit-body""></textarea>
 <div class=""description"">Enter the product description used for product teasers and pages.</div>
</div>
<fieldset class=""collapsible collapsed""><legend>Input format</legend><div class=""form-item"" id=""edit-format-1-wrapper"">
 <label class=""option"" for=""edit-format-1""><input class=""form-radio"" type=""radio"" id=""edit-format-1"" name=""format"" value=""1"" checked=""checked"" /> Filtered HTML</label>
 <div class=""description""><ul class=""tips""><li>Web page addresses and e-mail addresses turn into links automatically.</li><li>Allowed HTML tags: &lt;a&gt; &lt;em&gt; &lt;strong&gt; &lt;cite&gt; &lt;code&gt; &lt;ul&gt; &lt;ol&gt; &lt;li&gt; &lt;dl&gt; &lt;dt&gt; &lt;dd&gt;</li><li>Lines and paragraphs break automatically.</li></ul></div>
</div>
<div class=""form-item"" id=""edit-format-2-wrapper"">
 <label class=""option"" for=""edit-format-2""><input class=""form-radio"" type=""radio"" id=""edit-format-2"" name=""format"" value=""2"" /> Full HTML</label>
 <div class=""description""><ul class=""tips""><li>Web page addresses and e-mail addresses turn into links automatically.</li><li>Lines and paragraphs break automatically.</li></ul></div>
</div>
<div class=""form-item"" id=""edit-format-3-wrapper"">
 <label class=""option"" for=""edit-format-3""><input class=""form-radio"" type=""radio"" id=""edit-format-3"" name=""format"" value=""3"" /> PHP code</label>
 <div class=""description""><ul class=""tips""><li>You may post PHP code. You should include &lt;?php ?&gt; tags.</li></ul></div>
</div>
<p><a href=""/filter/tips"">More information about formatting options</a></p></fieldset>
</div><div class=""form-item"" id=""edit-title-wrapper"">
 <label for=""edit-title"">Name: <span class=""form-required"" title=""This field is required."">*</span></label>
 <input class=""form-text required"" type=""text"" maxlength=""255"" name=""title"" id=""edit-title"" size=""60"" value />
</div>
<div class=""form-item"" id=""edit-language-wrapper"">
 <label for=""edit-language"">Language: </label>
 <select class=""form-select"" name=""language"" id=""edit-language""><option value selected=""selected"">Language neutral</option><option value=""en"">English</option></select>
</div>
<div id=""field-image-cache-items""><table class=""content-multiple-table sticky-enabled"" id=""field_image_cache_values"">
 <thead><tr><th colspan=""2"">Image: </th><th>Order</th> </tr></thead>
<tbody>
 <tr class=""draggable odd""><td class=""content-multiple-drag""></td><td><div id=""edit-field-image-cache-0-ahah-wrapper""><div class=""form-item"" id=""edit-field-image-cache-0-wrapper"">
 <div class=""filefield-element clear-block""><div class=""widget-edit""><input class=""filefield-progress"" type=""hidden"" name=""field_image_cache[0][UPLOAD_IDENTIFIER]"" id=""edit-field-image-cache-0-UPLOAD-IDENTIFIER"" value=""134412a09a3390dccbd6b9d49e8805f2"" />
<input type=""hidden"" name=""field_image_cache[0][fid]"" id=""edit-field-image-cache-0-fid"" value=""0"" />
<input type=""hidden"" name=""field_image_cache[0][list]"" id=""edit-field-image-cache-0-list"" value=""1"" />
<div class=""form-item"" id=""edit-field-image-cache-0-upload-wrapper"">
 <div class=""filefield-upload clear-block""><input class=""form-file"" type=""file"" name=""files[field_image_cache_0]"" accept=""gif,jpg,png"" id=""edit-field-image-cache-0-upload"" size=""22"" />
<input class=""form-submit"" type=""submit"" name=""op"" id=""edit-field-image-cache-0-filefield-upload"" value=""Upload"" />
</div>
 <div class=""description"">Maximum Filesize: <em>200 MB</em><br />Allowed Extensions: <em>gif jpg png</em></div>
</div>
</div></div>
</div>
</div></td><td class=""delta-order""><div class=""form-item"" id=""edit-field-image-cache-0--weight-wrapper"">
 <select class=""form-select field_image_cache-delta-order"" name=""field_image_cache[0][_weight]"" id=""edit-field-image-cache-0--weight""><option value=""0"" selected=""selected"">0</option></select>
</div>
</td> </tr>
</tbody>
</table>
<div class=""content-add-more clear-block""><input class=""form-submit"" type=""submit"" name=""field_image_cache_add_more"" id=""edit-field-image-cache-field-image-cache-add-more"" value=""Add another item"" />
</div></div><input type=""hidden"" name=""changed"" id=""edit-changed"" value />
<input type=""hidden"" name=""form_build_id"" id=""form-959490b4b8c50c50a8ef455ee91bcec0"" value=""form-959490b4b8c50c50a8ef455ee91bcec0"" />
<input type=""hidden"" name=""form_token"" id=""edit-product-node-form-form-token"" value=""f94b0800bafcd921e1fde4bb5416627b"" />
<input type=""hidden"" name=""form_id"" id=""edit-product-node-form"" value=""product_node_form"" />
<div class=""better-select betterfixed"" id=""better-select-edit-field-catalog-value""><div class=""form-item"">
 <label>Catalog: </label>
 <div class=""form-checkboxes form-checkboxes-scroll""><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-131-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-131""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][131]"" id=""edit-field-catalog-value-131"" value=""131"" /> Small components</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-132-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-132""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][132]"" id=""edit-field-catalog-value-132"" value=""132"" />  - Bearings</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-133-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-133""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][133]"" id=""edit-field-catalog-value-133"" value=""133"" />  - Fasteners</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-134-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-134""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][134]"" id=""edit-field-catalog-value-134"" value=""134"" />  - Springs</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-135-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-135""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][135]"" id=""edit-field-catalog-value-135"" value=""135"" />  - Vacuum</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-83-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-83""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][83]"" id=""edit-field-catalog-value-83"" value=""83"" /> Readers</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-101-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-101""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][101]"" id=""edit-field-catalog-value-101"" value=""101"" />  - Blocks</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-137-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-137""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][137]"" id=""edit-field-catalog-value-137"" value=""137"" />  - Parts</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-100-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-100""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][100]"" id=""edit-field-catalog-value-100"" value=""100"" />  - Repair KITs</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-150-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-150""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][150]"" id=""edit-field-catalog-value-150"" value=""150"" />  - Antiskimming</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-86-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-86""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][86]"" id=""edit-field-catalog-value-86"" value=""86"" /> Input devices</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-136-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-136""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][136]"" id=""edit-field-catalog-value-136"" value=""136"" />  - EPPs</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-139-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-139""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][139]"" id=""edit-field-catalog-value-139"" value=""139"" />  - Functional keys</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-138-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-138""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][138]"" id=""edit-field-catalog-value-138"" value=""138"" />  - Operation panels</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-152-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-152""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][152]"" id=""edit-field-catalog-value-152"" value=""152"" />  - Antipeering</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-85-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-85""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][85]"" id=""edit-field-catalog-value-85"" value=""85"" /> Monitors</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-140-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-140""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][140]"" id=""edit-field-catalog-value-140"" value=""140"" />  - Monitors</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-141-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-141""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][141]"" id=""edit-field-catalog-value-141"" value=""141"" />  - Vandal shield</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-142-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-142""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][142]"" id=""edit-field-catalog-value-142"" value=""142"" />  - Upgrades</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-84-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-84""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][84]"" id=""edit-field-catalog-value-84"" value=""84"" /> Card cage</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-143-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-143""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][143]"" id=""edit-field-catalog-value-143"" value=""143"" />  - System blocks</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-144-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-144""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][144]"" id=""edit-field-catalog-value-144"" value=""144"" />  - Periphery</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-145-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-145""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][145]"" id=""edit-field-catalog-value-145"" value=""145"" />  - Motherboards &#9660;</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-147-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-147""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][147]"" id=""edit-field-catalog-value-147"" value=""147"" />  -  - CPU</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-148-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-148""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][148]"" id=""edit-field-catalog-value-148"" value=""148"" />  -  - RAM</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-168-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-168""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][168]"" id=""edit-field-catalog-value-168"" value=""168"" />  -  - Coolers</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-146-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-146""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][146]"" id=""edit-field-catalog-value-146"" value=""146"" />  - HDD-FDD-CD-DVD</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-82-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-82""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][82]"" id=""edit-field-catalog-value-82"" value=""82"" /> Printers</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-120-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-120""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][120]"" id=""edit-field-catalog-value-120"" value=""120"" />  - Blocks</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-149-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-149""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][149]"" id=""edit-field-catalog-value-149"" value=""149"" />  - CCA</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-121-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-121""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][121]"" id=""edit-field-catalog-value-121"" value=""121"" />  - Repair components</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-87-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-87""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][87]"" id=""edit-field-catalog-value-87"" value=""87"" /> Power supply</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-153-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-153""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][153]"" id=""edit-field-catalog-value-153"" value=""153"" />  - Power unuts</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-154-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-154""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][154]"" id=""edit-field-catalog-value-154"" value=""154"" />  - Distributors</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-156-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-156""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][156]"" id=""edit-field-catalog-value-156"" value=""156"" /> Motors and couplings</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-155-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-155""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][155]"" id=""edit-field-catalog-value-155"" value=""155"" /> Electrical components</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-159-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-159""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][159]"" id=""edit-field-catalog-value-159"" value=""159"" />  - Cable sets</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-158-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-158""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][158]"" id=""edit-field-catalog-value-158"" value=""158"" />  - Fans</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-161-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-161""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][161]"" id=""edit-field-catalog-value-161"" value=""161"" />  - Switches</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-160-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-160""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][160]"" id=""edit-field-catalog-value-160"" value=""160"" />  - Passive elements</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-97-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-97""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][97]"" id=""edit-field-catalog-value-97"" value=""97"" /> Electronic</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-162-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-162""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][162]"" id=""edit-field-catalog-value-162"" value=""162"" />  - Sensor</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-164-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-164""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][164]"" id=""edit-field-catalog-value-164"" value=""164"" />  - CCA</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-163-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-163""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][163]"" id=""edit-field-catalog-value-163"" value=""163"" />  - Special electronic</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-165-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-165""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][165]"" id=""edit-field-catalog-value-165"" value=""165"" />  - MUX &amp; HUB</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-79-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-79""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][79]"" id=""edit-field-catalog-value-79"" value=""79"" /> Extractor</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-92-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-92""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][92]"" id=""edit-field-catalog-value-92"" value=""92"" />  - Extractor modules</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-70-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-70""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][70]"" id=""edit-field-catalog-value-70"" value=""70"" />  - Repair components &#9660;</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-186-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-186""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][186]"" id=""edit-field-catalog-value-186"" value=""186"" />  -  - NCR</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-183-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-183""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][183]"" id=""edit-field-catalog-value-183"" value=""183"" />  -  - CMD-V4(Wincor)</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-182-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-182""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][182]"" id=""edit-field-catalog-value-182"" value=""182"" />  -  - AZM-NG(Wincor)</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-184-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-184""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][184]"" id=""edit-field-catalog-value-184"" value=""184"" />  -  - MMD(Diebold)</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-185-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-185""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][185]"" id=""edit-field-catalog-value-185"" value=""185"" />  -  - AFD(Deibold)</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-166-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-166""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][166]"" id=""edit-field-catalog-value-166"" value=""166"" />  - Repair KITs</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-93-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-93""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][93]"" id=""edit-field-catalog-value-93"" value=""93"" /> Stacker</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-78-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-78""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][78]"" id=""edit-field-catalog-value-78"" value=""78"" />  - Stacker modules</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-94-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-94""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][94]"" id=""edit-field-catalog-value-94"" value=""94"" />  - Repair components &#9660;</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-188-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-188""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][188]"" id=""edit-field-catalog-value-188"" value=""188"" />  -  - NCR</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-190-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-190""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][190]"" id=""edit-field-catalog-value-190"" value=""190"" />  -  - AFD(Deibold)</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-191-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-191""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][191]"" id=""edit-field-catalog-value-191"" value=""191"" />  -  - MMD(Diebold)</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-187-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-187""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][187]"" id=""edit-field-catalog-value-187"" value=""187"" />  -  - AZM-NG(Wincor)</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-189-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-189""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][189]"" id=""edit-field-catalog-value-189"" value=""189"" />  -  - CMD-V4(Wincor)</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-167-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-167""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][167]"" id=""edit-field-catalog-value-167"" value=""167"" />  - Repair KITs</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-95-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-95""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][95]"" id=""edit-field-catalog-value-95"" value=""95"" /> Presenter</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-80-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-80""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][80]"" id=""edit-field-catalog-value-80"" value=""80"" />  - Presenter modules</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-96-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-96""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][96]"" id=""edit-field-catalog-value-96"" value=""96"" />  - Repair components &#9660;</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-193-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-193""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][193]"" id=""edit-field-catalog-value-193"" value=""193"" />  -  - NCR</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-195-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-195""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][195]"" id=""edit-field-catalog-value-195"" value=""195"" />  -  - AFD (Diebold)</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-194-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-194""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][194]"" id=""edit-field-catalog-value-194"" value=""194"" />  -  - MMD(Diebold)</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-196-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-196""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][196]"" id=""edit-field-catalog-value-196"" value=""196"" />  -  - FlintStone, AZM-NG (Wincor)</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-197-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-197""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][197]"" id=""edit-field-catalog-value-197"" value=""197"" />  -  - CMD-V4 (Wincor)</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-169-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-169""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][169]"" id=""edit-field-catalog-value-169"" value=""169"" />  - Repair KITs</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-88-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-88""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][88]"" id=""edit-field-catalog-value-88"" value=""88"" /> Shutters</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-171-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-171""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][171]"" id=""edit-field-catalog-value-171"" value=""171"" />  - Shutter modules</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-170-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-170""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][170]"" id=""edit-field-catalog-value-170"" value=""170"" />  - Repair components</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-172-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-172""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][172]"" id=""edit-field-catalog-value-172"" value=""172"" />  - Repair KITs</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-81-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-81""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][81]"" id=""edit-field-catalog-value-81"" value=""81"" /> Cassettes</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-173-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-173""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][173]"" id=""edit-field-catalog-value-173"" value=""173"" />  - Cassettes</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-129-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-129""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][129]"" id=""edit-field-catalog-value-129"" value=""129"" />  - Repair components</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-126-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-126""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][126]"" id=""edit-field-catalog-value-126"" value=""126"" /> Cabinetry</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-89-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-89""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][89]"" id=""edit-field-catalog-value-89"" value=""89"" />  - Frame &amp; bezel</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-118-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-118""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][118]"" id=""edit-field-catalog-value-118"" value=""118"" />  - Locks &amp; keys</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-174-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-174""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][174]"" id=""edit-field-catalog-value-174"" value=""174"" />  - Mount</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-179-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-179""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][179]"" id=""edit-field-catalog-value-179"" value=""179"" /> Cash-In and recycling</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-175-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-175""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][175]"" id=""edit-field-catalog-value-175"" value=""175"" /> Other</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-178-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-178""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][178]"" id=""edit-field-catalog-value-178"" value=""178"" />  - ATM upgrades</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-176-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-176""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][176]"" id=""edit-field-catalog-value-176"" value=""176"" />  - Consumables </label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-90-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-90""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][90]"" id=""edit-field-catalog-value-90"" value=""90"" />  - Tools</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-177-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-177""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][177]"" id=""edit-field-catalog-value-177"" value=""177"" />  - Software</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-130-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-130""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][130]"" id=""edit-field-catalog-value-130"" value=""130"" /> Literature</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-68-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-68""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][68]"" id=""edit-field-catalog-value-68"" value=""68"" /> ATMs</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-catalog-value-69-wrapper"">
 <label class=""option"" for=""edit-field-catalog-value-69""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_catalog[value][69]"" id=""edit-field-catalog-value-69"" value=""69"" /> POST Terminal</label>
</div>
</div></div>
</div>
</div><div class=""form-item"" id=""edit-field-id-0-value-wrapper"">
 <label for=""edit-field-id-0-value"">Supplier: </label>
 <input class=""form-text text"" type=""text"" maxlength=""60"" name=""field_id[0][value]"" id=""edit-field-id-0-value"" size=""60"" value />
</div>
<div class=""container-inline-date form-item date-clear-block""><div class=""form-item"" id=""edit-field-price-date-0-value-wrapper"">
 <label for=""edit-field-price-date-0-value"">Price date: </label>
 <div class=""form-item"" id=""edit-field-price-date-0-value-datepicker-popup-0-wrapper"">
 <input class=""form-text"" type=""text"" maxlength=""30"" name=""field_price_date[0][value][date]"" id=""edit-field-price-date-0-value-datepicker-popup-0"" size=""20"" value=""20.05.2013"" />
 <div class=""description""> Format: 20.05.2013</div>
</div>

</div>
</div><div class=""form-item"" id=""edit-field-cost-cur-value-wrapper"">
 <label for=""edit-field-cost-cur-value"">COST currency: </label>
 <select class=""form-select"" name=""field_cost_cur[value]"" id=""edit-field-cost-cur-value""><option value selected=""selected"">- None -</option><option value=""EURO"">EURO</option><option value=""USD"">USD</option><option value=""RUB"">RUB</option><option value=""UA"">UA</option></select>
</div>
<fieldset class=""product-field collapsible""><legend>Product information</legend><div class=""form-item"" id=""edit-model-wrapper"">
 <label for=""edit-model"">SKU: <span class=""form-required"" title=""This field is required."">*</span></label>
 <input class=""form-text required"" type=""text"" maxlength=""128"" name=""model"" id=""edit-model"" size=""32"" value />
 <div class=""description"">Product SKU/model.</div>
</div>
<table><tbody><tr><td>
<div class=""form-item"" id=""edit-list-price-wrapper"">
 <label for=""edit-list-price"">List price: </label>
 <span class=""field-prefix""></span> <input class=""form-text"" type=""text"" maxlength=""35"" name=""list_price"" id=""edit-list-price"" size=""20"" value=""0"" /> <span class=""field-suffix""> &#8364;</span>
 <div class=""description"">The listed MSRP.</div>
</div>
</td><td><div class=""form-item"" id=""edit-cost-wrapper"">
 <label for=""edit-cost"">Cost: </label>
 <span class=""field-prefix""></span> <input class=""form-text"" type=""text"" maxlength=""35"" name=""cost"" id=""edit-cost"" size=""20"" value=""0"" /> <span class=""field-suffix""> &#8364;</span>
 <div class=""description"">Your store's cost.</div>
</div>
</td><td><div class=""form-item"" id=""edit-sell-price-wrapper"">
 <label for=""edit-sell-price"">Sell price: <span class=""form-required"" title=""This field is required."">*</span></label>
 <span class=""field-prefix""></span> <input class=""form-text required"" type=""text"" maxlength=""35"" name=""sell_price"" id=""edit-sell-price"" size=""20"" value=""0"" /> <span class=""field-suffix""> &#8364;</span>
 <div class=""description"">Customer purchase price.</div>
</div>
</td></tr></tbody></table>
<div class=""form-item"" id=""edit-shippable-wrapper"">
 <label class=""option"" for=""edit-shippable""><input class=""form-checkbox"" type=""checkbox"" name=""shippable"" id=""edit-shippable"" value=""1"" checked=""checked"" /> Product and its derivatives are shippable.</label>
</div>
<table><tbody><tr><td><div class=""form-item"" id=""edit-weight-wrapper"">
 <label for=""edit-weight"">Weight: </label>
 <input class=""form-text"" type=""text"" maxlength=""15"" name=""weight"" id=""edit-weight"" size=""10"" value=""0"" />
</div>
</td><td><div class=""form-item"" id=""edit-weight-units-wrapper"">
 <label for=""edit-weight-units"">Unit of measurement: </label>
 <select class=""form-select"" name=""weight_units"" id=""edit-weight-units""><option value=""lb"">Pounds</option><option value=""kg"" selected=""selected"">Kilograms</option><option value=""oz"">Ounces</option><option value=""g"">Grams</option></select>
</div>
</td></tr></tbody></table><fieldset><legend>Dimensions</legend><div class=""description"">Physical dimensions of the packaged product.</div><table>
<tbody>
 <tr class=""odd""><td><div class=""form-item"" id=""edit-length-units-wrapper"">
 <label for=""edit-length-units"">Units of measurement: </label>
 <select class=""form-select"" name=""length_units"" id=""edit-length-units""><option value=""in"">Inches</option><option value=""ft"">Feet</option><option value=""cm"">Centimeters</option><option value=""mm"" selected=""selected"">Millimeters</option></select>
</div>
</td><td><div class=""form-item"" id=""edit-dim-length-wrapper"">
 <label for=""edit-dim-length"">Length: </label>
 <input class=""form-text"" type=""text"" maxlength=""128"" name=""dim_length"" id=""edit-dim-length"" size=""10"" value />
</div>
</td><td><div class=""form-item"" id=""edit-dim-width-wrapper"">
 <label for=""edit-dim-width"">Width: </label>
 <input class=""form-text"" type=""text"" maxlength=""128"" name=""dim_width"" id=""edit-dim-width"" size=""10"" value />
</div>
</td><td><div class=""form-item"" id=""edit-dim-height-wrapper"">
 <label for=""edit-dim-height"">Height: </label>
 <input class=""form-text"" type=""text"" maxlength=""128"" name=""dim_height"" id=""edit-dim-height"" size=""10"" value />
</div>
</td> </tr>
</tbody>
</table>
</fieldset>
<div class=""form-item"" id=""edit-pkg-qty-wrapper"">
 <label for=""edit-pkg-qty"">Package quantity: </label>
 <input class=""form-text"" type=""text"" maxlength=""128"" name=""pkg_qty"" id=""edit-pkg-qty"" size=""60"" value=""1"" />
 <div class=""description"">For a package containing only this product, how many are in it?</div>
</div>
<div class=""form-item"" id=""edit-default-qty-wrapper"">
 <label for=""edit-default-qty"">Default quantity to add to cart: </label>
 <input class=""form-text"" type=""text"" maxlength=""6"" name=""default_qty"" id=""edit-default-qty"" size=""5"" value=""1"" />
 <div class=""description"">Leave blank or zero to disable the quantity field next to the add to cart button, if it is enabled <a href=""/admin/store/settings/products/edit"">in general</a>. If it is disabled, this field is ignored.</div>
</div>
<div class=""form-item"" id=""edit-ordering-wrapper"">
 <label for=""edit-ordering"">List position: </label>
 <select class=""form-select"" name=""ordering"" id=""edit-ordering""><option value=""-25"">-25</option><option value=""-24"">-24</option><option value=""-23"">-23</option><option value=""-22"">-22</option><option value=""-21"">-21</option><option value=""-20"">-20</option><option value=""-19"">-19</option><option value=""-18"">-18</option><option value=""-17"">-17</option><option value=""-16"">-16</option><option value=""-15"">-15</option><option value=""-14"">-14</option><option value=""-13"">-13</option><option value=""-12"">-12</option><option value=""-11"">-11</option><option value=""-10"">-10</option><option value=""-9"">-9</option><option value=""-8"">-8</option><option value=""-7"">-7</option><option value=""-6"">-6</option><option value=""-5"">-5</option><option value=""-4"">-4</option><option value=""-3"">-3</option><option value=""-2"">-2</option><option value=""-1"">-1</option><option value=""0"" selected=""selected"">0</option><option value=""1"">1</option><option value=""2"">2</option><option value=""3"">3</option><option value=""4"">4</option><option value=""5"">5</option><option value=""6"">6</option><option value=""7"">7</option><option value=""8"">8</option><option value=""9"">9</option><option value=""10"">10</option><option value=""11"">11</option><option value=""12"">12</option><option value=""13"">13</option><option value=""14"">14</option><option value=""15"">15</option><option value=""16"">16</option><option value=""17"">17</option><option value=""18"">18</option><option value=""19"">19</option><option value=""20"">20</option><option value=""21"">21</option><option value=""22"">22</option><option value=""23"">23</option><option value=""24"">24</option><option value=""25"">25</option></select>
 <div class=""description"">Specify a value to set this product's position in product lists.<br />Products in the same position will be sorted alphabetically.</div>
</div>
</fieldset>
<div class=""form-item"" id=""edit-field-brend-value-wrapper"">
 <label for=""edit-field-brend-value"">Brand Manufacturer block: </label>
 <select class=""form-select"" name=""field_brend[value]"" id=""edit-field-brend-value""><option value selected=""selected"">- None -</option><option value=""63"">WINCOR-NIXDORF</option><option value=""53"">NCR</option><option value=""46"">DIEBOLD</option><option value=""43"">BANQUIT</option><option value=""192"">Nautilus HYOSUNG</option></select>
</div>
<div class=""form-item"">
 <label>State: </label>
 <div class=""form-radios""><div class=""form-item"" id=""edit-field-stale-value--wrapper"">
 <label class=""option"" for=""edit-field-stale-value-""><input class=""form-radio"" type=""radio"" id=""edit-field-stale-value-"" name=""field_stale[value]"" value="""" checked=""checked"" /> N/A</label>
</div>
<div class=""form-item"" id=""edit-field-stale-value-71-wrapper"">
 <label class=""option"" for=""edit-field-stale-value-71""><input class=""form-radio"" type=""radio"" id=""edit-field-stale-value-71"" name=""field_stale[value]"" value=""71"" /> new original</label>
</div>
<div class=""form-item"" id=""edit-field-stale-value-198-wrapper"">
 <label class=""option"" for=""edit-field-stale-value-198""><input class=""form-radio"" type=""radio"" id=""edit-field-stale-value-198"" name=""field_stale[value]"" value=""198"" /> tested, working</label>
</div>
<div class=""form-item"" id=""edit-field-stale-value-72-wrapper"">
 <label class=""option"" for=""edit-field-stale-value-72""><input class=""form-radio"" type=""radio"" id=""edit-field-stale-value-72"" name=""field_stale[value]"" value=""72"" /> refurbished original</label>
</div>
<div class=""form-item"" id=""edit-field-stale-value-73-wrapper"">
 <label class=""option"" for=""edit-field-stale-value-73""><input class=""form-radio"" type=""radio"" id=""edit-field-stale-value-73"" name=""field_stale[value]"" value=""73"" /> new generic</label>
</div>
<div class=""form-item"" id=""edit-field-stale-value-74-wrapper"">
 <label class=""option"" for=""edit-field-stale-value-74""><input class=""form-radio"" type=""radio"" id=""edit-field-stale-value-74"" name=""field_stale[value]"" value=""74"" /> not tested</label>
</div>
</div>
</div>
<div class=""better-select betterfixed"" id=""better-select-edit-field-use-value""><div class=""form-item"">
 <label>Use in products: </label>
 <div class=""form-checkboxes form-checkboxes-scroll""><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-use-value-109-wrapper"">
 <label class=""option"" for=""edit-field-use-value-109""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_use[value][109]"" id=""edit-field-use-value-109"" value=""109"" /> Siemens Nixdorf NG serie</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-use-value-110-wrapper"">
 <label class=""option"" for=""edit-field-use-value-110""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_use[value][110]"" id=""edit-field-use-value-110"" value=""110"" /> Wincor Nixdorf NG/FL serie</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-use-value-111-wrapper"">
 <label class=""option"" for=""edit-field-use-value-111""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_use[value][111]"" id=""edit-field-use-value-111"" value=""111"" /> Wincor Nixdorf XE serie</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-use-value-112-wrapper"">
 <label class=""option"" for=""edit-field-use-value-112""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_use[value][112]"" id=""edit-field-use-value-112"" value=""112"" /> Wincor Nixdorf XE USB serie</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-use-value-180-wrapper"">
 <label class=""option"" for=""edit-field-use-value-180""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_use[value][180]"" id=""edit-field-use-value-180"" value=""180"" /> WINCOR NIXDORF PC3100(XE)</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-use-value-15-wrapper"">
 <label class=""option"" for=""edit-field-use-value-15""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_use[value][15]"" id=""edit-field-use-value-15"" value=""15"" /> Diebold IX serie</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-use-value-108-wrapper"">
 <label class=""option"" for=""edit-field-use-value-108""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_use[value][108]"" id=""edit-field-use-value-108"" value=""108"" /> Diebold OPTEVA serie</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-use-value-113-wrapper"">
 <label class=""option"" for=""edit-field-use-value-113""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_use[value][113]"" id=""edit-field-use-value-113"" value=""113"" /> NCR 56xx serie</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-use-value-114-wrapper"">
 <label class=""option"" for=""edit-field-use-value-114""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_use[value][114]"" id=""edit-field-use-value-114"" value=""114"" /> NCR 58xx serie</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-use-value-115-wrapper"">
 <label class=""option"" for=""edit-field-use-value-115""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_use[value][115]"" id=""edit-field-use-value-115"" value=""115"" /> NCR 66xx serie</label>
</div>
</div><div class=""checkbox-depth-0""><div class=""form-item"" id=""edit-field-use-value-38-wrapper"">
 <label class=""option"" for=""edit-field-use-value-38""><input class=""form-checkbox form-checkboxes-scroll"" type=""checkbox"" name=""field_use[value][38]"" id=""edit-field-use-value-38"" value=""38"" /> BANQUIT W25</label>
</div>
</div></div>
</div>
</div><div class=""attachments""><fieldset class=""collapsible collapsed""><legend>File attachments</legend><div class=""description"">Changes made to the attachments are not permanent until you save this post. The first &quot;listed&quot; file will be included in RSS feeds.</div><div id=""attach-wrapper""><div class=""form-item"" id=""edit-upload-wrapper"">
 <label for=""edit-upload"">Attach new file: </label>
 <input class=""form-file"" type=""file"" name=""files[upload]"" id=""edit-upload"" size=""40"" />

 <div class=""description"">The maximum upload size is <em>8 MB</em>. Only files with the following extensions may be uploaded: <em>jpg jpeg gif png txt doc xls pdf ppt pps odt ods odp</em>. </div>
</div>
<input class=""form-submit"" type=""submit"" name=""attach"" id=""edit-attach"" value=""Attach"" />
</div></fieldset>
</div><fieldset class=""collapsible collapsed""><legend>Revision information</legend><div class=""form-item"" id=""edit-revision-wrapper"">
 <label class=""option"" for=""edit-revision""><input class=""form-checkbox"" type=""checkbox"" name=""revision"" id=""edit-revision"" value=""1"" /> Create new revision</label>
</div>
<div class=""form-item"" id=""edit-log-wrapper"">
 <label for=""edit-log"">Log message: </label>
 <textarea class=""form-textarea resizable"" cols=""60"" rows=""2"" name=""log"" id=""edit-log""></textarea>
 <div class=""description"">An explanation of the additions or updates being made to help other authors understand your motivations.</div>
</div>
</fieldset>
<div class=""form-item"" id=""edit-field-gross-0-value-wrapper"">
 <label for=""edit-field-gross-0-value""> Gross Weight: </label>
 <input class=""form-text number"" type=""text"" maxlength=""10"" name=""field_gross[0][value]"" id=""edit-field-gross-0-value"" size=""12"" value /> <span class=""field-suffix""> kg</span>
</div>
<div class=""form-item"" id=""edit-field-net-0-value-wrapper"">
 <label for=""edit-field-net-0-value"">Net Weight: </label>
 <input class=""form-text number"" type=""text"" maxlength=""10"" name=""field_net[0][value]"" id=""edit-field-net-0-value"" size=""12"" value /> <span class=""field-suffix""> kg</span>
</div>
<div class=""form-item"" id=""edit-field-volume-0-value-wrapper"">
 <label for=""edit-field-volume-0-value"">Volume: </label>
 <input class=""form-text number"" type=""text"" maxlength=""10"" name=""field_volume[0][value]"" id=""edit-field-volume-0-value"" size=""12"" value /> <span class=""field-suffix""> dm&#179;</span>
</div>
<div class=""form-item"" id=""edit-field-delivery-value-wrapper"">
 <label for=""edit-field-delivery-value"">Delivery terms: </label>
 <select class=""form-select"" name=""field_delivery[value]"" id=""edit-field-delivery-value""><option value selected=""selected"">- None -</option><option value=""151"">Information item</option><option value=""64"">In stock</option><option value=""65"">EXW Germany 1 week</option><option value=""75"">EXW Germany 2 weeks</option><option value=""76"">EXW Germany 4 weeks</option><option value=""77"">EXW Germany, more then 4 weeks</option><option value=""127"">EXW Hong-Kong</option><option value=""67"">Customized</option><option value=""66"">Goods in transit</option><option value=""128"">EXW Dubai</option></select>
</div>
<div class=""form-item"" id=""edit-field-note-0-value-wrapper"">
 <label for=""edit-field-note-0-value"">Special Comments: </label>
 <textarea class=""form-textarea resizable"" cols=""60"" rows=""5"" name=""field_note[0][value]"" id=""edit-field-note-0-value""></textarea>
</div>
<fieldset class=""collapsible collapsed""><legend>Input format</legend><div class=""form-item"" id=""edit-field-note-0-format-1-wrapper"">
 <label class=""option"" for=""edit-field-note-0-format-1""><input class=""form-radio"" type=""radio"" id=""edit-field-note-0-format-1"" name=""field_note[0][format]"" value=""1"" /> Filtered HTML</label>
 <div class=""description""><ul class=""tips""><li>Web page addresses and e-mail addresses turn into links automatically.</li><li>Allowed HTML tags: &lt;a&gt; &lt;em&gt; &lt;strong&gt; &lt;cite&gt; &lt;code&gt; &lt;ul&gt; &lt;ol&gt; &lt;li&gt; &lt;dl&gt; &lt;dt&gt; &lt;dd&gt;</li><li>Lines and paragraphs break automatically.</li></ul></div>
</div>
<div class=""form-item"" id=""edit-field-note-0-format-2-wrapper"">
 <label class=""option"" for=""edit-field-note-0-format-2""><input class=""form-radio"" type=""radio"" id=""edit-field-note-0-format-2"" name=""field_note[0][format]"" value=""2"" checked=""checked"" /> Full HTML</label>
 <div class=""description""><ul class=""tips""><li>Web page addresses and e-mail addresses turn into links automatically.</li><li>Lines and paragraphs break automatically.</li></ul></div>
</div>
<div class=""form-item"" id=""edit-field-note-0-format-3-wrapper"">
 <label class=""option"" for=""edit-field-note-0-format-3""><input class=""form-radio"" type=""radio"" id=""edit-field-note-0-format-3"" name=""field_note[0][format]"" value=""3"" /> PHP code</label>
 <div class=""description""><ul class=""tips""><li>You may post PHP code. You should include &lt;?php ?&gt; tags.</li></ul></div>
</div>
<p><a href=""/filter/tips"">More information about formatting options</a></p></fieldset>
<fieldset class=""menu-item-form collapsible collapsed""><legend>Menu settings</legend><div class=""form-item"" id=""edit-menu-link-title-wrapper"">
 <label for=""edit-menu-link-title"">Menu link title: </label>
 <input class=""form-text"" type=""text"" maxlength=""128"" name=""menu[link_title]"" id=""edit-menu-link-title"" size=""60"" value />
 <div class=""description"">The link text corresponding to this item that should appear in the menu. Leave blank if you do not wish to add this post to the menu.</div>
</div>
<div class=""form-item"" id=""edit-menu-parent-wrapper"">
 <label for=""edit-menu-parent"">Parent item: </label>
 <select class=""form-select menu-title-select"" name=""menu[parent]"" id=""edit-menu-parent""><option value=""navigation:0"">&lt;Navigation&gt;</option><option value=""navigation:1480"">-- Catalog (disabled)</option><option value=""navigation:9"">-- Compose tips (disabled)</option><option value=""navigation:773"">-- Contact (disabled)</option><option value=""navigation:21"">-- My account</option><option value=""navigation:775"">-- Search (disabled)</option><option value=""navigation:1362"">-- Tags (disabled)</option><option value=""navigation:778"">-- User list (disabled)</option><option value=""navigation:11"">-- Create content</option><option value=""navigation:811"">---- Email Campaign</option><option value=""navigation:1904"">---- News</option><option value=""navigation:1395"">---- Newsletter issue</option><option value=""navigation:106"">---- Page</option><option value=""navigation:731"">---- Page_Private</option><option value=""navigation:1496"">---- Product</option><option value=""navigation:107"">---- Story</option><option value=""navigation:847"">---- Webform</option><option value=""navigation:2"">-- Administer</option><option value=""navigation:10"">---- Content management</option><option value=""navigation:806"">------ Comments</option><option value=""navigation:28"">------ Content</option><option value=""navigation:29"">------ Content types</option><option value=""navigation:1396"">------ Newsletters</option><option value=""navigation:1405"">-------- Sent issues</option><option value=""navigation:1399"">-------- Draft issues</option><option value=""navigation:1403"">-------- Newsletters</option><option value=""navigation:1407"">-------- Subscriptions</option><option value=""navigation:1782"">------ Node Export: Import</option><option value=""navigation:42"">------ Post settings</option><option value=""navigation:838"">------ Taxonomy</option><option value=""navigation:17"">---- Site building</option><option value=""navigation:25"">------ Blocks</option><option value=""navigation:809"">------ Contact form</option><option value=""navigation:822"">------ Menus</option><option value=""navigation:1034"">-------- Navigation</option><option value=""navigation:1035"">-------- Primary links</option><option value=""navigation:1036"">-------- Secondary links</option><option value=""navigation:18"">---- Site configuration</option><option value=""navigation:23"">------ Actions</option><option value=""navigation:36"">------ Input formats</option><option value=""navigation:820"">------ Lightbox2</option><option value=""navigation:835"">------ Search settings</option><option value=""navigation:1397"">------ Simplenews</option><option value=""navigation:1400"">-------- General</option><option value=""navigation:1401"">-------- Newsletter</option><option value=""navigation:1406"">-------- Subscription</option><option value=""navigation:1404"">-------- Send mail</option><option value=""navigation:878"">------ Taxonomy hide</option><option value=""navigation:1487"">---- Store administration</option><option value=""navigation:1494"">------ Orders</option><option value=""navigation:1537"">-------- View orders</option><option value=""navigation:1514"">-------- Create order</option><option value=""navigation:1529"">-------- Search orders</option><option value=""navigation:1491"">------ Customers</option><option value=""navigation:1536"">-------- View customers</option><option value=""navigation:1528"">-------- Search customers</option><option value=""navigation:1497"">------ Products</option><option value=""navigation:1538"">-------- View products</option><option value=""navigation:1516"">-------- Find orphaned products</option><option value=""navigation:1488"">------ Attributes</option><option value=""navigation:1498"">------ Reports</option><option value=""navigation:1515"">-------- Customer reports</option><option value=""navigation:1524"">-------- Product reports</option><option value=""navigation:1527"">-------- Sales reports</option><option value=""navigation:1489"">------ Conditional actions</option><option value=""navigation:20"">---- User management</option><option value=""navigation:22"">------ Access rules</option><option value=""navigation:41"">------ Permissions</option><option value=""navigation:828"">------ Profiles</option><option value=""navigation:45"">------ Roles</option><option value=""navigation:50"">------ User settings</option><option value=""navigation:51"">------ Users</option><option value=""navigation:16"">---- Reports</option><option value=""navigation:830"">------ Recent log entries</option><option value=""navigation:829"">------ Recent hits</option><option value=""navigation:839"">------ Top 'access denied' errors</option><option value=""navigation:840"">------ Top 'page not found' errors</option><option value=""navigation:842"">------ Top referrers</option><option value=""navigation:843"">------ Top search phrases</option><option value=""navigation:841"">------ Top pages</option><option value=""navigation:844"">------ Top visitors</option><option value=""navigation:792"">---- Help</option><option value=""navigation:4"">-- Log out</option><option value=""primary-links:0"" selected=""selected"">&lt;Primary links&gt;</option><option value=""primary-links:317"">-- About</option><option value=""primary-links:318"">---- Specialization</option><option value=""primary-links:319"">---- History</option><option value=""primary-links:320"">---- Contact</option><option value=""primary-links:325"">---- Details</option><option value=""primary-links:688"">-- Catalog</option><option value=""primary-links:322"">-- Documentation</option><option value=""primary-links:323"">-- Analytics</option><option value=""primary-links:324"">-- Contacts</option><option value=""primary-links:746"">-- Private</option><option value=""primary-links:1389"">-- Price list</option><option value=""primary-links:1874"">-- Admin Price List</option><option value=""secondary-links:0"">&lt;Secondary links&gt;</option></select>
 <div class=""description"">The maximum depth for an item and all its children is fixed at 9. Some menu items may not be available as parents if selecting them would exceed this limit.</div>
</div>
<div class=""form-item"" id=""edit-menu-weight-wrapper"">
 <label for=""edit-menu-weight"">Weight: </label>
 <select class=""form-select"" name=""menu[weight]"" id=""edit-menu-weight""><option value=""-50"">-50</option><option value=""-49"">-49</option><option value=""-48"">-48</option><option value=""-47"">-47</option><option value=""-46"">-46</option><option value=""-45"">-45</option><option value=""-44"">-44</option><option value=""-43"">-43</option><option value=""-42"">-42</option><option value=""-41"">-41</option><option value=""-40"">-40</option><option value=""-39"">-39</option><option value=""-38"">-38</option><option value=""-37"">-37</option><option value=""-36"">-36</option><option value=""-35"">-35</option><option value=""-34"">-34</option><option value=""-33"">-33</option><option value=""-32"">-32</option><option value=""-31"">-31</option><option value=""-30"">-30</option><option value=""-29"">-29</option><option value=""-28"">-28</option><option value=""-27"">-27</option><option value=""-26"">-26</option><option value=""-25"">-25</option><option value=""-24"">-24</option><option value=""-23"">-23</option><option value=""-22"">-22</option><option value=""-21"">-21</option><option value=""-20"">-20</option><option value=""-19"">-19</option><option value=""-18"">-18</option><option value=""-17"">-17</option><option value=""-16"">-16</option><option value=""-15"">-15</option><option value=""-14"">-14</option><option value=""-13"">-13</option><option value=""-12"">-12</option><option value=""-11"">-11</option><option value=""-10"">-10</option><option value=""-9"">-9</option><option value=""-8"">-8</option><option value=""-7"">-7</option><option value=""-6"">-6</option><option value=""-5"">-5</option><option value=""-4"">-4</option><option value=""-3"">-3</option><option value=""-2"">-2</option><option value=""-1"">-1</option><option value=""0"" selected=""selected"">0</option><option value=""1"">1</option><option value=""2"">2</option><option value=""3"">3</option><option value=""4"">4</option><option value=""5"">5</option><option value=""6"">6</option><option value=""7"">7</option><option value=""8"">8</option><option value=""9"">9</option><option value=""10"">10</option><option value=""11"">11</option><option value=""12"">12</option><option value=""13"">13</option><option value=""14"">14</option><option value=""15"">15</option><option value=""16"">16</option><option value=""17"">17</option><option value=""18"">18</option><option value=""19"">19</option><option value=""20"">20</option><option value=""21"">21</option><option value=""22"">22</option><option value=""23"">23</option><option value=""24"">24</option><option value=""25"">25</option><option value=""26"">26</option><option value=""27"">27</option><option value=""28"">28</option><option value=""29"">29</option><option value=""30"">30</option><option value=""31"">31</option><option value=""32"">32</option><option value=""33"">33</option><option value=""34"">34</option><option value=""35"">35</option><option value=""36"">36</option><option value=""37"">37</option><option value=""38"">38</option><option value=""39"">39</option><option value=""40"">40</option><option value=""41"">41</option><option value=""42"">42</option><option value=""43"">43</option><option value=""44"">44</option><option value=""45"">45</option><option value=""46"">46</option><option value=""47"">47</option><option value=""48"">48</option><option value=""49"">49</option><option value=""50"">50</option></select>
 <div class=""description"">Optional. In the menu, the heavier items will sink and the lighter items will be positioned nearer the top.</div>
</div>
</fieldset>
<fieldset class=""collapsible collapsed""><legend>Comment settings</legend><div class=""form-radios""><div class=""form-item"" id=""edit-comment-0-wrapper"">
 <label class=""option"" for=""edit-comment-0""><input class=""form-radio"" type=""radio"" id=""edit-comment-0"" name=""comment"" value=""0"" /> Disabled</label>
</div>
<div class=""form-item"" id=""edit-comment-1-wrapper"">
 <label class=""option"" for=""edit-comment-1""><input class=""form-radio"" type=""radio"" id=""edit-comment-1"" name=""comment"" value=""1"" /> Read only</label>
</div>
<div class=""form-item"" id=""edit-comment-2-wrapper"">
 <label class=""option"" for=""edit-comment-2""><input class=""form-radio"" type=""radio"" id=""edit-comment-2"" name=""comment"" value=""2"" checked=""checked"" /> Read/Write</label>
</div>
</div></fieldset>
  </div>
  <div class=""admin"">
    <div class=""authored"">
<fieldset class=""collapsible collapsed""><legend>Authoring information</legend><div class=""form-item"" id=""edit-name-wrapper"">
 <label for=""edit-name"">Authored by: </label>
 <input class=""form-text form-autocomplete"" type=""text"" maxlength=""60"" name=""name"" id=""edit-name"" size=""60"" value=""Otari"" />
 <div class=""description"">Leave blank for <em>Anonymous</em>.</div>
</div>
<input class=""autocomplete"" type=""hidden"" id=""edit-name-autocomplete"" value=""http://www.elifai.eu/user/autocomplete"" disabled=""disabled"" /><div class=""form-item"" id=""edit-date-wrapper"">
 <label for=""edit-date"">Authored on: </label>
 <input class=""form-text"" type=""text"" maxlength=""25"" name=""date"" id=""edit-date"" size=""60"" value />
 <div class=""description"">Format: <em>2013-05-20 09:38:20 +0200</em>. Leave blank to use the time of form submission.</div>
</div>
</fieldset>
    </div>
    <div class=""options"">
<fieldset class=""collapsible collapsed""><legend>Publishing options</legend><div class=""form-item"" id=""edit-status-wrapper"">
 <label class=""option"" for=""edit-status""><input class=""form-checkbox"" type=""checkbox"" name=""status"" id=""edit-status"" value=""1"" checked=""checked"" /> Published</label>
</div>
<div class=""form-item"" id=""edit-promote-wrapper"">
 <label class=""option"" for=""edit-promote""><input class=""form-checkbox"" type=""checkbox"" name=""promote"" id=""edit-promote"" value=""1"" /> Promoted to front page</label>
</div>
<div class=""form-item"" id=""edit-sticky-wrapper"">
 <label class=""option"" for=""edit-sticky""><input class=""form-checkbox"" type=""checkbox"" name=""sticky"" id=""edit-sticky"" value=""1"" /> Sticky at top of lists</label>
</div>
</fieldset>
    </div>
  </div>
<input class=""form-submit"" type=""submit"" name=""op"" id=""edit-submit"" value=""Save"" />
<input class=""form-submit"" type=""submit"" name=""op"" id=""edit-save-continue"" value=""Save and continue"" />
<input class=""form-submit"" type=""submit"" name=""op"" id=""edit-preview"" value=""Preview"" />
</div>

</div></form>";