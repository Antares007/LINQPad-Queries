<Query Kind="Program">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAAAD89x4SL38S/4r7NUU2iHNNTcmnVTi1xQPx1AC1vAtFAAAAAAOgAAAAAIAACAAAABgO+xVBio0IKzceIXWbWfgFv0jcxQpOA9YilhDtPA8XxAAAABJcM5+MLInsGd5jUUGfXtnQAAAALHy7sVof5cKLhfpSHxbLdPESALwKOWLElOgeYcZmaeqO1sDG9SHnPN5xOzSlBHlSD7agk+KC9dH/4XMZn4gYvM=</Password>
    <Database>Pirvelckaroebi</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>&lt;RuntimeDirectory&gt;\Accessibility.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\100\SDK\Assemblies\Microsoft.SqlServer.ConnectionInfo.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\100\SDK\Assemblies\Microsoft.SqlServer.Management.Sdk.Sfc.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\100\SDK\Assemblies\Microsoft.SqlServer.Smo.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\100\SDK\Assemblies\Microsoft.SqlServer.SmoExtended.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\100\SDK\Assemblies\Microsoft.SqlServer.SqlEnum.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationCore.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Deployment.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Serialization.Formatters.Soap.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Xaml.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\WindowsBase.dll</Reference>
  <GACReference>PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35</GACReference>
  <GACReference>System.Windows, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</GACReference>
  <NuGetReference>Dapper</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Common</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Dmf</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Smo</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Data.Common</Namespace>
  <Namespace>System.Windows.Controls</Namespace>
  <Namespace>System.Windows.Data</Namespace>
  <Namespace>System.Windows</Namespace>
  <Namespace>System.Windows.Markup</Namespace>
  <Namespace>System.Windows.Controls.Primitives</Namespace>
  <Namespace>System.Collections.ObjectModel</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

public static class Ex2
{
	public static IEnumerable<T> Show<T>(this IEnumerable<T> src, Action done = null)
	{
		
		var grid = new DataGrid(){ ItemsSource= src, AutoGenerateColumns=false,IsReadOnly=true };
		var props = typeof(T).GetProperties(BindingFlags.Instance|BindingFlags.Public).Select (x => new {x.Name,Type=x.PropertyType})
								.Concat(typeof(T).GetFields().Select (x => new {x.Name,Type=x.FieldType}));
		props.Dump(typeof(T).Name);
		foreach(var p in props)
		{
			
			if(typeof(Button).IsAssignableFrom(p.Type))
			{
			
				DataTemplate dt = ("<ContentPresenter Content=\"{Binding "+p.Name+"}\" />").ToDataTemplate();
				grid.Columns.Add(new DataGridTemplateColumn{Header = p.Name, CellTemplate=dt});
			}
			else
			{
				if (p.Type.IsValueType || p.Type==typeof(string))
				grid.Columns.Add(new DataGridTextColumn{Header = p.Name, Binding=new Binding{Path=new PropertyPath(p.Name)}});
			}
		}
		var g = new Grid();
		g.RowDefinitions.Add(new RowDefinition(){Height=new GridLength(1,GridUnitType.Star)});
		g.RowDefinitions.Add(new RowDefinition(){Height=new GridLength(1,GridUnitType.Auto)});
		grid.SetValue(Grid.RowProperty,0);
		grid.SetValue(Grid.ColumnProperty,0);
		g.Children.Add(grid);
		if(done != null)
		{
			var doneButton = new Button{ Content="Done" };
			doneButton.Click += (sender, args) => done();
			var stackPanel = new StackPanel(){ Children={ doneButton } };
			stackPanel.HorizontalAlignment = HorizontalAlignment.Right;
			stackPanel.SetValue(Grid.RowProperty,1);
			g.Children.Add(stackPanel);
		}
		PanelManager.DisplayWpfElement (g,null);
		return src;
	}
	
	public static object InstantiateXAML(string xaml)
    {
        return XamlReader.Load
        (
            XmlReader.Create(new StringReader(xaml))
        );    
    }
 	public static Button OnClick(this string src,Action action)
    {
		return OnClick(src, _ => action());
	}
	public static Button OnClick(this string src,Action<Button> action)
    {
		var b =new Button(){ Content = src };
		b.Click+= (sender, args) => action(b);
		return b;
	}
    public static DataTemplate ToDataTemplate(this string template)
    {
        string templateFormat = @"<DataTemplate  
                                xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' 
                                xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
                                {0}
                            </DataTemplate>";
        
        return (DataTemplate) InstantiateXAML(string.Format(templateFormat, template));
    }
	public static ObservableCollection<T> ToObsCol<T>(this IEnumerable<T> src){
		return new ObservableCollection<T>(src);
	}
	public static Func<T,U> CreateSelector<T,U>(this IEnumerable<T> src,Func<T,U> selector){
		return selector;
	}
}

void Main()
{
	var sel = Pirvelckaro_02_DEVNILEBIs.CreateSelector(
			x  => new {
					Col2 = "ak dachera mxolod ertkhel sheidzleba"
								.OnClick((b) => { b.IsEnabled = false; }),
					Col3 = "ok"
								.OnClick(() => {  }),
					x.IdpID,x.Name,x.Surname
				});
				
	var xs = Pirvelckaro_02_DEVNILEBIs.Take(2).AsEnumerable().Select (sel).ToObsCol();
	  xs.Show(done : () => { 
		xs.Clear();
		foreach(var e in Pirvelckaro_02_DEVNILEBIs.Take(5).Select (sel))
			xs.Add(e);
	  });
	  
}




Func<string, PirvelckarosCkhrili> getCkhrilebisSacavi()
{
	
	var dict = new ConcurrentDictionary<string,PirvelckarosCkhrili>();
	return  sakheli => dict.GetOrAdd(sakheli, s => {
														Dictionary<string,string> map;
														mapDict.TryGetValue(sakheli, out map);
														return new PirvelckarosCkhrili(s, map);
													});
}
class PirvelckarosCkhrili 
{
	string _sakheli;
	Dictionary<string,string> _map;
	public PirvelckarosCkhrili(string sakheli, Dictionary<string,string> map)
	{
		_sakheli = sakheli;
		_map = map;
	}
	public string MomeciSelectNaciliVelebistvis(IEnumerable<string> velebi)
	{
	
		return "select "+string.Join(", ", velebi.Select (v => {
			if (_map == null)
				return v;
							
			string fromVeli;
			if(_map.TryGetValue(v,out fromVeli))
			 	return fromVeli + " as " + v;
			if(_map.TryGetValue("["+v+"]",out fromVeli))
			 	return fromVeli + " as " + v;
			return v;
		}) )+" from " + _sakheli;
	}
}
class ShedarebisSkripti
{
	public string Sql {get;set;}
}
class WhereNacili
{
	public string Sql {get;private set;}
	public WhereNacili(string whereNacili)
	{
		Sql = whereNacili;
		var dict = Sql.Split(',',' ','\n','\t','(',')','=')
			.Select (x => x.Trim())
			.Where (x => x.Length > 2  && (x.Substring(0,2)=="l." || x.Substring(0,2)=="r."))
			.Distinct()
			.GroupBy (x => x.Substring(0,2))
			.ToDictionary (x => x.Key,v=>v.Select (x => x.Substring(2)).ToList());
		MarckhenaCkhrilisVelebi = dict["l."];
		MarjvebaCkhrilisVelebi = dict["r."];
	}
	public IEnumerable<string> MarckhenaCkhrilisVelebi {get;set;}
	public IEnumerable<string> MarjvebaCkhrilisVelebi {get;set;}
}
class ShedarepisPiroba
{
	string _piroba;
	public ShedarepisPiroba() :this(@"
	l.PID = r.PID AND (l.First_Name = r.First_Name OR l.First_Name is null  and r.First_Name is null)
				  AND (l.Last_Name = r.Last_Name   OR l.Last_Name is null   and r.Last_Name is null)
                  AND (l.Birth_Date = r.Birth_Date OR l.Birth_Date is null  and r.Birth_Date is null)	
	")
	{}
	public ShedarepisPiroba(string piroba)
	{
		_piroba = piroba;
		
	}
	
	public IEnumerable<WhereNacili> MomeWhereNacili(PirvelckarosCkhrili l, PirvelckarosCkhrili r)
	{
		yield return new WhereNacili(_piroba);
		
		yield return new WhereNacili(_piroba.Replace("l.First_Name","l.First_Name_tmp")
						    				.Replace("l.Last_Name","l.First_Name")
											.Replace("l.First_Name_tmp","l.Last_Name"));
	}
	
	
}
class ShedarebisSkriptisGeneratori
{
	List<ShedarepisPiroba> _pirobebi;
	PirvelckarosCkhrili _reestrisCkhrili;
	
	public ShedarebisSkriptisGeneratori(IEnumerable<ShedarepisPiroba> pirobebi, PirvelckarosCkhrili reestrisCkhrili)
	{
		_reestrisCkhrili=reestrisCkhrili;
		_pirobebi=pirobebi.ToList();;
	}
	string _updateSql=@"
update l
set l.Unnom = r.Unnom
from (@LeftSet) l
join (@RightSet) r on 
(@WhereNacili)
where l.Unnom is null
	";
	
	public IEnumerable<ShedarebisSkripti> Damigenerire(PirvelckarosCkhrili ckhrili)
	{
		foreach (var piroba in _pirobebi)
		{
			foreach (var whereNacili in piroba.MomeWhereNacili(ckhrili, _reestrisCkhrili))
			{
				var sql = _updateSql
					.Replace("@WhereNacili", whereNacili.Sql)
					.Replace("@LeftSet", ckhrili.MomeciSelectNaciliVelebistvis(whereNacili.MarckhenaCkhrilisVelebi.Concat(new []{"Unnom"}).Distinct()))
					.Replace("@RightSet", _reestrisCkhrili.MomeciSelectNaciliVelebistvis(whereNacili.MarjvebaCkhrilisVelebi.Concat(new []{"Unnom"}).Distinct()))
					;
				yield return new ShedarebisSkripti{Sql=sql};
			}
		}	
		yield break;
	}
}
public static class Ex
{
	public static IEnumerable<Database> Databases(this Server src) 
	{
		return src.Databases.Cast<Database>();
	}	

	public static IEnumerable<Table> Tables(this Database src) 
	{
		return src.Tables.Cast<Table>();
	}	

	public static IEnumerable<Column> Columns(this Table src) 
	{
		return src.Columns.Cast<Column>();
	}	

	public static IEnumerable<Index> Indexes(this Table src) 
	{
		return src.Indexes.Cast<Index>();
	}	

	public static IEnumerable<IndexedColumn> IndexedColumns(this Index src) 
	{
		return src.IndexedColumns.Cast<IndexedColumn>();
	}

	public static Table AddColumun(this Table tb, params Action<Column>[] colBulders) 
	{
		foreach(var colB in colBulders)
		{
			var col=new Column(){Parent=tb};
			colB (col);
			tb.Columns.Add(col);
		}
		return tb;
	}
	
	public static void CreateIndexOn(this Table tb, params string[] columuns) {
		Index idx;
		idx = new Index(tb, "IX_"+tb.Name+"_"+string.Join("_", columuns));

		foreach(var ic  in columuns.Select (c => new IndexedColumn(idx, c, false))){
			idx.IndexedColumns.Add(ic);
		}
		idx.IsClustered = false;
		idx.Create();
	}
	
	private static readonly Lazy<Server> lazy = new Lazy<Server>(() => {
			var conn = new ServerConnection("triton", "sa", "ssa$20");
    		var srv = new Server(conn);
			return srv;
			});
    
    public static Server Triton { get { return lazy.Value; } }
}
private Dictionary<string,Dictionary<string,string>> mapDict=@"
	Pirvelckaroebi..Pirvelckaro_01_UMCEOEBI,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_01_UMCEOEBI,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_01_UMCEOEBI,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_01_UMCEOEBI,BIRTH_DATE,[Birth_Date]
	
	Pirvelckaroebi..Pirvelckaro_03_BAVSHVEBI,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_03_BAVSHVEBI,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_03_BAVSHVEBI,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_03_BAVSHVEBI,BIRTH_DATE,[Birth_Date]

	Pirvelckaroebi..Pirvelckaro_02_DEVNILEBI,PersonalNumber,[PID]
	Pirvelckaroebi..Pirvelckaro_02_DEVNILEBI,Name,[First_Name]
	Pirvelckaroebi..Pirvelckaro_02_DEVNILEBI,Surname,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_02_DEVNILEBI,DateOfBirth,[Birth_Date]


	Pirvelckaroebi..Pirvelckaro_04_REINTEGRACIA,C_PID,[PID]
	Pirvelckaroebi..Pirvelckaro_04_REINTEGRACIA,C_F_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_04_REINTEGRACIA,C_L_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_04_REINTEGRACIA,C_BIRTH_DA,[Birth_Date]

	Pirvelckaroebi..Pirvelckaro_05_KULTURA,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_05_KULTURA,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_05_KULTURA,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_05_KULTURA,BIRTH_DATE,[Birth_Date]	

	Pirvelckaroebi..Pirvelckaro_06_XANDAZMULEBI,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_06_XANDAZMULEBI,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_06_XANDAZMULEBI,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_06_XANDAZMULEBI,BIRTH_DATE,[Birth_Date]	

	Pirvelckaroebi..Pirvelckaro_07_SKOLA_PANSIONEBI,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_07_SKOLA_PANSIONEBI,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_07_SKOLA_PANSIONEBI,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_07_SKOLA_PANSIONEBI,BIRTH_DATE,[Birth_Date]	
	
	Pirvelckaroebi..Pirvelckaro_08_TEACHERS,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_08_TEACHERS,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_08_TEACHERS,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_08_TEACHERS,BIRTH_DATE,[Birth_Date]
	
	Pirvelckaroebi..Pirvelckaro_09_UFROSI_AGMZRDELEBI,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_09_UFROSI_AGMZRDELEBI,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_09_UFROSI_AGMZRDELEBI,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_09_UFROSI_AGMZRDELEBI,BIRTH_DATE,[Birth_Date]

	Pirvelckaroebi..Pirvelckaro_10_APKHAZETIS_OJAKHEBI,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_10_APKHAZETIS_OJAKHEBI,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_10_APKHAZETIS_OJAKHEBI,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_10_APKHAZETIS_OJAKHEBI,BIRTH_DATE,[Birth_Date]

	Pirvelckaroebi..Pirvelckaro_11_SATEMO,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_11_SATEMO,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_11_SATEMO,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_11_SATEMO,BIRTH_DATE,[Birth_Date]
	
	Pirvelckaroebi..Pirvelckaro_12_MCIRE_SAOJAXO,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_12_MCIRE_SAOJAXO,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_12_MCIRE_SAOJAXO,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_12_MCIRE_SAOJAXO,BIRTH_DATE,[Birth_Date]

	Pirvelckaroebi..Pirvelckaro_13_TEACHERS_AFX,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_13_TEACHERS_AFX,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_13_TEACHERS_AFX,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_13_TEACHERS_AFX,BIRTH_DATE,[Birth_Date]
	
	Pirvelckaroebi..Pirvelckaro_14_RESURSCENTRIS_TANAMSHROMLEBI,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_14_RESURSCENTRIS_TANAMSHROMLEBI,FIRST_NAME,[First_Name]
	Pirvelckaroebi..Pirvelckaro_14_RESURSCENTRIS_TANAMSHROMLEBI,LAST_NAME,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_14_RESURSCENTRIS_TANAMSHROMLEBI,BIRTH_DATE,[Birth_Date]

	
	Pirvelckaroebi..Pirvelckaro_21_SAPENSIO_ASAKIS_MOSAXLEOBA,PRIVATE_NUMBER,[PID]
	Pirvelckaroebi..Pirvelckaro_21_SAPENSIO_ASAKIS_MOSAXLEOBA,FIRST,[First_Name]
	Pirvelckaroebi..Pirvelckaro_21_SAPENSIO_ASAKIS_MOSAXLEOBA,LAST,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_21_SAPENSIO_ASAKIS_MOSAXLEOBA,BIRTH_DATE,[Birth_Date]

	Pirvelckaroebi..Pirvelckaro_22_STUDENTEBI,პირადობა,[PID]
	Pirvelckaroebi..Pirvelckaro_22_STUDENTEBI,სახელი,[First_Name]
	Pirvelckaroebi..Pirvelckaro_22_STUDENTEBI,გვარი,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_22_STUDENTEBI,დაბ_თარიღი,[Birth_Date]
	
	Pirvelckaroebi..[Pirvelckaro_23_BAVSHVEBI(165)],PRIVATE_NUMBER,[PID]
	Pirvelckaroebi..[Pirvelckaro_23_BAVSHVEBI(165)],FIRST,[First_Name]
	Pirvelckaroebi..[Pirvelckaro_23_BAVSHVEBI(165)],LAST,[Last_Name]
	Pirvelckaroebi..[Pirvelckaro_23_BAVSHVEBI(165)],BIRTH_DATE,[Birth_Date]
	
	Pirvelckaroebi..Pirvelckaro_24_INVALIDI_BAVSHVEBI,PiradiNomeri,[PID]
	Pirvelckaroebi..Pirvelckaro_24_INVALIDI_BAVSHVEBI,Sakheli,[First_Name]
	Pirvelckaroebi..Pirvelckaro_24_INVALIDI_BAVSHVEBI,Gvari,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_24_INVALIDI_BAVSHVEBI,DabadebisTarigi,[Birth_Date]

	Pirvelckaroebi..Pirvelckaro_25_MKVETRAD_GAMOXATULI_INVALIDI_BAVSHVEBI,PiradiNomeri,[PID]
	Pirvelckaroebi..Pirvelckaro_25_MKVETRAD_GAMOXATULI_INVALIDI_BAVSHVEBI,Sakheli,[First_Name]
	Pirvelckaroebi..Pirvelckaro_25_MKVETRAD_GAMOXATULI_INVALIDI_BAVSHVEBI,Gvari,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_25_MKVETRAD_GAMOXATULI_INVALIDI_BAVSHVEBI,DabadebisTarigi,[Birth_Date]
	
	Pirvelckaroebi..Pirvelckaro_26_ARASAQARTVELOS_MOQALAQE_PENSIONREBI,PiradiNomeri,[PID]
	Pirvelckaroebi..Pirvelckaro_26_ARASAQARTVELOS_MOQALAQE_PENSIONREBI,Sakheli,[First_Name]
	Pirvelckaroebi..Pirvelckaro_26_ARASAQARTVELOS_MOQALAQE_PENSIONREBI,Gvari,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_26_ARASAQARTVELOS_MOQALAQE_PENSIONREBI,DabadebisTarigi,[Birth_Date]
	
	Pirvelckaroebi..[Pirvelckaro_27_AXALSHOBILEBI(165)],PRIVATE_NUMBER,[PID]
	Pirvelckaroebi..[Pirvelckaro_27_AXALSHOBILEBI(165)],FIRST,[First_Name]
	Pirvelckaroebi..[Pirvelckaro_27_AXALSHOBILEBI(165)],LAST,[Last_Name]
	Pirvelckaroebi..[Pirvelckaro_27_AXALSHOBILEBI(165)],BIRTH_DATE,[Birth_Date]
	
	Pirvelckaroebi..Pirvelckaro_100_DevniltaMisamartebi_201210,PID,[PID]
	Pirvelckaroebi..Pirvelckaro_100_DevniltaMisamartebi_201210,First_Name,[First_Name]
	Pirvelckaroebi..Pirvelckaro_100_DevniltaMisamartebi_201210,Last_Name,[Last_Name]
	Pirvelckaroebi..Pirvelckaro_100_DevniltaMisamartebi_201210,Birth_Date,[Birth_Date]
		
	"
	.Split('\n')
	.Select (x => x.Split(',').Select (v=>v.Trim()).ToArray())
	.Where(x=>x.Length==3)
	.Select (x => new {Tanme=x[0],From=x[1],To=x[2]})
	.GroupBy (x => x.Tanme)
	.ToDictionary(x=>x.Key,x=>x.ToDictionary (k => k.To,v=>v.From));