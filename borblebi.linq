<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <NuGetReference>CsQuery</NuGetReference>
  <NuGetReference>EPPlus</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference>RavenDB.Client</NuGetReference>
  <Namespace>CsQuery</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.ConditionalFormatting</Namespace>
  <Namespace>OfficeOpenXml.ConditionalFormatting.Contracts</Namespace>
  <Namespace>OfficeOpenXml.DataValidation</Namespace>
  <Namespace>OfficeOpenXml.DataValidation.Contracts</Namespace>
  <Namespace>OfficeOpenXml.DataValidation.Formulas.Contracts</Namespace>
  <Namespace>OfficeOpenXml.Drawing</Namespace>
  <Namespace>OfficeOpenXml.Drawing.Chart</Namespace>
  <Namespace>OfficeOpenXml.Drawing.Vml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>OfficeOpenXml.Style.Dxf</Namespace>
  <Namespace>OfficeOpenXml.Style.XmlAccess</Namespace>
  <Namespace>OfficeOpenXml.Table</Namespace>
  <Namespace>OfficeOpenXml.Table.PivotTable</Namespace>
  <Namespace>OfficeOpenXml.Utils</Namespace>
  <Namespace>OfficeOpenXml.VBA</Namespace>
  <Namespace>Raven.Abstractions.Extensions</Namespace>
  <Namespace>Raven.Client.Document</Namespace>
  <Namespace>Raven.Json.Linq</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

[Serializable]
public class FinalResult
{
	public SearchOption Brand { get; set; }
	public SearchOption Range { get; set; }
	public SearchOption Model { get; set; }
	public SearchOption Engine { get; set; }
	public SearchOption Year { get; set; }
	public List<Fitment> Fitments { get; set; }
}
[Serializable]
public class SearchOptionResult
{
	public List<SearchOption> TopBrands { get; set; }
	public List<SearchOption> SearchOptions { get; set; }
}
[Serializable]
public class SearchOption
{
	public string Name { get; set; }
	public string Value { get; set; }
	public string Description { get; set; }
	public int? NumberOfItems { get; set; }
}
[Serializable]
public class Result
{
	public List<Fitment> Fitments { get; set; }
}
[Serializable]
public class Fitment
{
	public List<Dimension> Dimensions { get; set; }
}
[Serializable]
public class Dimension
{
	public string Position { get; set; }
	public string Width { get; set; }
	public string Ratio { get; set; }
	public string Radial { get; set; }
	public string Load { get; set; }
	public string Speed { get; set; }
	public string Normalpressure { get; set; }
	public string Highwaypressure { get; set; }
	public string Season { get; set; }
}
Task<SearchOptionResult> Brands(Func<string, Task<SearchOptionResult>> getOptions)
{
	return getOptions("http://www.michelin.de/tyreSearch/tyreSelectorSearchOption.action?outputType=1&tyreSegment=123&optionName=brand&dependencyValue=brandDependencyValue");
}
Task<SearchOptionResult> Range(Func<string, Task<SearchOptionResult>> getOptions, string brand)
{
	return getOptions(string.Format("http://www.michelin.de/tyreSearch/tyreSelectorSearchOption.action?outputType=1&tyreSegment=123&optionName=range&dependencyValue=rangeDependencyValue&brandDependencyValue={0}", brand));
}
Task<SearchOptionResult> Model(Func<string, Task<SearchOptionResult>> getOptions, string brand, string range)
{
	return getOptions(string.Format("http://www.michelin.de/tyreSearch/tyreSelectorSearchOption.action?outputType=1&tyreSegment=123&optionName=model&dependencyValue=modelDependencyValue&brandDependencyValue={0}&rangeDependencyValue={1}", brand, range));
}
Task<SearchOptionResult> Engine(Func<string, Task<SearchOptionResult>> getOptions, string brand, string range, string model)
{
	return getOptions(string.Format("http://www.michelin.de/tyreSearch/tyreSelectorSearchOption.action?outputType=1&tyreSegment=123&optionName=engine&dependencyValue=engineDependencyValue&brandDependencyValue={0}&rangeDependencyValue={1}&modelDependencyValue={2}", brand, range, model));
}
Task<SearchOptionResult> Year(Func<string, Task<SearchOptionResult>> getOptions, string brand, string range, string model, string engine)
{
	return getOptions(string.Format("http://www.michelin.de/tyreSearch/tyreSelectorSearchOption.action?outputType=1&tyreSegment=123&optionName=year&dependencyValue=yearDependencyValue&brandDependencyValue={0}&rangeDependencyValue={1}&modelDependencyValue={2}&engineDependencyValue={3}", brand, range, model, engine));
}
Task<Result> Results(Func<string, Task<Result>> getResults, string brand, string range, string model, string engine, string year)
{
	return getResults(string.Format("http://www.michelin.de/tyreSearch/tyreSelectorSearchResult.action?brand={0}&range={1}&model={2}&engine={3}&year={4}&needFitments=true", brand, range, model, engine, year));
}

async Task Run()
{
	var cookies = new CookieContainer();
	
	var handler = new HttpClientHandler {
		UseDefaultCredentials = false,
		Proxy = new WebProxy("http://172.17.7.40:8998", false, new string[]{}),
		UseProxy = true,
	};
	
	var client = new HttpClient(handler);
	
	Func<string,Task<SearchOptionResult>> getOptions = async (url) => {
		var rezult = await client.GetStringAsync(url);
		return JsonConvert.DeserializeObject<SearchOptionResult>(rezult);
	};
	Func<string,Task<Result>> getResults = async (url) => {
		var rezult = await client.GetStringAsync(url);
		return JsonConvert.DeserializeObject<Result>(rezult);
	};
	var meta = new RavenJObject();
	meta.Add("Raven-Entity-Name", RavenJToken.FromObject("FinalResult3"));
	
	using(var docStore = (new DocumentStore() {		Url = "http://localhost:8080",
													DefaultDatabase="Borblebi", 
													Conventions = { 
															FindTypeTagName = (t) => t.Name, 
															FindClrTypeName = t => t.Name 
														}
													}).Initialize())
	{
		var brands = await Brands(getOptions);
		var topBrands = brands.TopBrands.Select (tb => tb.Value.ToLowerInvariant()).ToList();
		var restBrands = brands.SearchOptions.Where (so => !topBrands.Contains(so.Value.ToLowerInvariant()));
		
		foreach (var brand in restBrands)
		foreach (var range in (await Range(getOptions, brand.Value)).SearchOptions)
		foreach (var model in (await Model(getOptions, brand.Value, range.Value)).SearchOptions)
		foreach (var engine in (await Engine(getOptions, brand.Value, range.Value, model.Value)).SearchOptions)
		foreach (var year in (await Year(getOptions, brand.Value, range.Value, model.Value, engine.Value)).SearchOptions)
		{
			var result = await Results(getResults, brand.Value, range.Value, model.Value, engine.Value, year.Value);
			var finalResult = new FinalResult {
				Brand = brand,
				Range = range,
				Model = model,
				Engine = engine,
				Year = year,
				Fitments = result.Fitments
			};
			docStore.DatabaseCommands.Put("finalresult3/", null, RavenJObject.FromObject(finalResult), meta);
		}
	}
}

IEnumerable<Fitment> GamiertianeDimenionebi(IEnumerable<Fitment> fitments)
{
	foreach (var fitment in fitments)
	{
		if(fitment.Dimensions.Count () < 2)
		{
			yield return fitment;
		}
		else
		{
			var f = fitment.Dimensions.First ();
			var r = fitment.Dimensions.Last ();
			if(f.Width==r.Width && f.Ratio == r.Ratio && f.Radial == r.Radial)
			{
				f = RavenJObject.FromObject(f).JsonDeserialization<Dimension>();
				f.Position = "front&rear";
				yield return new Fitment{Dimensions=new []{f}.ToList()};
			}else
			{
				yield return fitment;
			}
		}
	}
}

IEnumerable<Fitment> SiganisKorektireba(IEnumerable<Fitment> fitments)
{
	return (
		from fitment in fitments.Where (f => f.Dimensions.Count () == 1)
		let dim = fitment.Dimensions.First ()
		group dim by new {dim.Radial, dim.Ratio} into g
		select g.OrderByDescending (x => decimal.Parse(x.Width)).First () into d
		select new Fitment{Dimensions=new []{d}.ToList()}
	)
	.Concat(fitments.Where (f => f.Dimensions.Count () != 1))
	.ToList();
}

string Fkey(IEnumerable<Fitment> fitments)
{
	return string.Join("", fitments.Select (f => string.Join("",f.Dimensions.Select (d => d.Width + d.Ratio + d.Radial))).OrderBy (x => x));
}

void Main()
{
	var finalResults = Util.Cache(
		() => StreamDocs("http://localhost:8080", "finalresult3", "Borblebi")
					.Select (x => x.JsonDeserialization<FinalResult>())
					.ToList(), 
		"finalresult"
	);
//	finalResults
//		.SelectMany (x => new []{new {t="Brand", v=x.Brand},new {t="Range", v=x.Range},new {t="Model", v=x.Model},new {t="Engine", v=x.Engine},new {t="Year", v=x.Year}})
//		.Where (x => new []{x.v.Name,x.v.Value,x.v.Description}.Distinct().Count () != 1)
//		.Where (x => x.t != "Year")
//		.Distinct()
//		.Dump();
//	return;
	
//	finalResults.Take(10).Dump();
	var frs= from x in finalResults
			 let Fitments=SiganisKorektireba(GamiertianeDimenionebi(x.Fitments))
			 select new {
				Brand=x.Brand.Name.ToLowerInvariant(),
				Range=x.Range.Name.ToLowerInvariant(),
				Model=x.Model.Name.ToLowerInvariant(),
				Engine=x.Engine.Name.ToLowerInvariant(),
				Year=x.Year.Name.ToLowerInvariant(),
				Fitments,
				FitmentsOriginal=x.Fitments,
				Fkey=Fkey(Fitments)
			 };
	frs.Where (f => f.Fitments.Any (fi => fi.Dimensions.Count ()>1)).Dump();
	return;
	
	
	
 	var borblebi = (
		from result in frs
		from fitment in result.Fitments
		from dimension in fitment.Dimensions
		select new {
			Brand=result.Brand,
			Range=result.Range,
			Model=result.Model,
			Engine=result.Engine,
			Year=result.Year,
			dimension.Position,
			dimension.Width,
			dimension.Ratio,
			dimension.Radial,
			dimension.Load,
			dimension.Speed,
			dimension.Normalpressure,
			dimension.Highwaypressure,
			dimension.Season 
		}
	).ToList();
	var conebi = (
		from b in borblebi.Select (bo => new {bo.Brand,bo.Range,bo.Year,bo.Width,bo.Ratio,bo.Radial} ).Distinct()
		group b by b.Width+"/"+b.Ratio+"/"+b.Radial into g
		select new {g.Key,Raodenoba=g.Count ()}
	).ToDictionary (x => x.Key,x=>x.Raodenoba);
	
	(from b in borblebi
	let Borbali = b.Width+"/"+b.Ratio+"/"+b.Radial
	select new {b.Brand,b.Range,b.Model,b.Year,b.Width,b.Ratio,b.Radial,b.Position,ModelebisRaodenoba=conebi[Borbali],Borbali}
	).Distinct()
	.DumpToExcel("BorblebiKatalogi");
	
	return;
	var excelNewLine = new String((char)10, 1);
	(
		from b in borblebi.Select (bo => new {bo.Brand,bo.Range,bo.Year,bo.Width,bo.Ratio,bo.Radial} ).Distinct()
		group b by new {b.Width,b.Radial,b.Ratio} into g
		orderby g.Count () descending
		let BrandsAndModels = (
						from brand in g
						group brand by brand.Brand into bg
						let Models= ( 
									from model in bg
									group model by model.Range into mg
									orderby mg.Count () descending
									select mg.Count () + "/" + mg.Key
								)
						orderby bg.Count () descending	
						select bg.Count()+"="+bg.Key + "(" + string.Join("; ", Models) +")"
						
				)
		let BrandsAndYears = (
						from brand in g
						group brand by brand.Brand into bg
						let Models= ( 
									from year in bg
									group year by year.Year into mg
									orderby mg.Count () descending
									select mg.Count () + "/" + mg.Key
								)
						orderby bg.Count () descending	
						select bg.Count()+"="+bg.Key + "(" + string.Join("; ", Models) +")"
						
				)
		select new {
			
			Borbali = g.Key.Width + "/" + g.Key.Ratio + "/" + g.Key.Radial,
			g.Key.Width, g.Key.Ratio, g.Key.Radial,
			Raodenoba = g.Count(),
			BrandsAndModels = string.Join(excelNewLine, BrandsAndModels),
			BrandsAndYears = string.Join(excelNewLine, BrandsAndYears),
		}
	)
	.DumpToExcel("Borblebi");
}

public IEnumerable<RavenJObject> StreamDocs(string url, string doc, string database="Borblebi"){
		using(var docStore = (new DocumentStore() {	Url = url,
													DefaultDatabase=database, 
													Conventions = { 
															FindTypeTagName = (t) => t.Name, 
															FindClrTypeName = t => t.Name 
														}
													}).Initialize())
		{
		var enmerator = docStore.DatabaseCommands.StreamDocs(null, doc, null);
			while(enmerator.MoveNext()){
				yield return enmerator.Current;
			}
		}
}

public static class EnumerableEx
{
	public static void DumpToExcel<T>(this IEnumerable<T> src, string wbName)
	{
		ExcelPackage pck = new ExcelPackage();
		var wsEnum = pck.Workbook.Worksheets.Add("Data");
		wsEnum.Cells["A1"].LoadFromCollection(src, true, OfficeOpenXml.Table.TableStyles.Medium9);
		wsEnum.Cells[wsEnum.Dimension.Address].AutoFitColumns();
		var fileName = @"d:\anvol\"+wbName+".xlsx";
		File.Delete(fileName);
		pck.SaveAs(new FileInfo(fileName));
		Process.Start(fileName);
	}

}