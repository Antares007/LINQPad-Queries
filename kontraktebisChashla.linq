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
  <Reference>D:\Dev\EventStore\bin\eventstore\debug\anycpu\EventStore.ClientAPI.dll</Reference>
  <Reference>D:\Dev\Ganckhadebebi.Domain\Ganckhadebebi.Domain\bin\Debug\Ganckhadebebi.Domain.dll</Reference>
  <Reference>D:\Dev\TimePeriod_v1.4.11\Pub\Desktop.Release\Itenso.TimePeriod.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\100\SDK\Assemblies\Microsoft.SqlServer.ConnectionInfo.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\100\SDK\Assemblies\Microsoft.SqlServer.Management.Sdk.Sfc.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\100\SDK\Assemblies\Microsoft.SqlServer.Smo.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\100\SDK\Assemblies\Microsoft.SqlServer.SmoExtended.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SQL Server\100\SDK\Assemblies\Microsoft.SqlServer.SqlEnum.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\wpf\PresentationCore.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationFramework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\wpf\PresentationUI.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\wpf\ReachFramework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Deployment.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\wpf\System.Printing.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.Tasks.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Xaml.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\wpf\UIAutomationProvider.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\wpf\UIAutomationTypes.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\wpf\WindowsBase.dll</Reference>
  <GACReference>System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</GACReference>
  <GACReference>System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</GACReference>
  <GACReference>System.Web.Helpers, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35</GACReference>
  <NuGetReference>Dapper-Async</NuGetReference>
  <NuGetReference>Ix_Experimental-Main</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference>Rhino-Etl</NuGetReference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>EventStore.ClientAPI</Namespace>
  <Namespace>Ganckhadebebi.Domain</Namespace>
  <Namespace>Itenso.TimePeriod</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Common</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Dmf</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Smo</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Rhino.Etl.Core</Namespace>
  <Namespace>Rhino.Etl.Core.Operations</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Collections.ObjectModel</Namespace>
  <Namespace>System.Data.Common</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
  <Namespace>System.Reactive.Disposables</Namespace>
  <Namespace>System.Reactive.Joins</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Reactive.PlatformServices</Namespace>
  <Namespace>System.Reactive.Subjects</Namespace>
  <Namespace>System.Reactive.Threading.Tasks</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Windows</Namespace>
  <Namespace>System.Windows.Controls</Namespace>
  <Namespace>System.Windows.Data</Namespace>
  <Namespace>System.Windows.Markup</Namespace>
</Query>


async Task a(Action< List<IDictionary<string,object>>> nexts)
{
    
    using(var con = new SqlConnection("Data Source=Triton;User ID=sa;Initial Catalog=Pirvelckaroebi;Password=ssa$20;app=LINQPad [kontraktebisChashla]"))
    using(var com = new SqlCommand("select top 10000 * from UketesiReestri..JUST_20121201",con))
    {
        await con.OpenAsync();
        var dr = await com.ExecuteReaderAsync();
        var buf = new List<IDictionary<string,object>>(10240);
        while (await dr.ReadAsync())
        {
            var o = Enumerable.Range(0,dr.FieldCount).ToDictionary(i => dr.GetName(i),i=>dr.GetValue(i));
            if(buf.Count==10240){
                nexts(buf);
                buf.Clear();
            }
            buf.Add(o);
        }
        
        if(buf.Count > 0){
            nexts(buf);
        }
    }
}
public static class JSON
{
    public static byte[] ToJsonBytes(this object o)
    {
        var str = ToJsonString(o);
        return Encoding.UTF8.GetBytes(str);
    }

    public static string ToJsonString(this object o)
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(o);
    }
    public static T Parse<T>(this string str)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
    }

    public static T Parse<T>(this byte[] bytes)
    {
        var str = Encoding.UTF8.GetString(bytes);
        return Parse<T>(str);
    }
}
void Main()
{

var dazgvevisCkhrilebi =    from t in Ex.Triton.Databases["INSURANCEW"].Tables()
                            let split = t.Name.ToLower().Split('_')
                            where split.Length == 2
                            let pir = split[0]
                            let meore = split[1]
                            where pir.ToLower().Contains("dazgveva") && meore.All(Char.IsNumber)
                            let perInt = int.Parse(meore)
                            where perInt>201107
                            select new {t, periodi = (perInt < 2000) ? 201000 + perInt : perInt};
                            
                            
var selectebi=dazgvevisCkhrilebi
    .OrderBy (c => c.t.Name)
    .Select (c => string.Format(@"select
     {1} Periodi
    ,d.ID,d.AkhaliUnnomi, d.Unnom, d.Base_type
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_{1}_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_{1}_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_{1}_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_{1} STATE_Shemdegi
    ,d.Company_ID, d.Company_ID_{1} Company_ID_Shemdegi
from INSURANCEW..{0} d",c.t.Name,c.periodi))
;
string.Join("\nUnion all\n",selectebi).Dump();
return;

EtlEx.Process(
    Enumerable.Range(1,10).Select (e => new {e}).EtlInput()//EtlEx.DumpToSql("Tname")
).Dump();

return;
var reestrisCkhrilebi = from db in Ex.Triton.Databases()
                        from t in db.Tables()
                        let split = t.Name.ToLower().Split('_')
                        where split.Length == 2
                        let pir = split[0]
                        let meore = split[1]
                        where pir.ToUpper() == "JUST" && meore.All(Char.IsNumber)
                        let perInt = int.Parse(meore)
                        select new {dbName=db.Name,tName=t.Name, periodi=perInt};
                            
var apends = reestrisCkhrilebi
    .OrderBy (c => c.periodi)
    .Select (x => string.Format("AppendToStream.exe \"{0}-{1}\" \"{1}\" \"select * from {0}..{1}\" 4096",x.dbName,x.tName))
    ;
string.Join("\n",apends).Dump();
return;




return;

    

return;

return;

    foreach (var t in dazgvevisCkhrilebi.Where (x => x.t.Columns().All (cn => cn.Name != "AkhaliUnnomi")))
    {
        t.t.AddColumun(c => { c.Name = "AkhaliUnnomi"; c.DataType = DataType.Int; });
        t.t.Alter();
    }
    
}

IEnumerable<string> GetFieldsNames(IDbConnection con, string table_Name)
{
    var nameParts=table_Name.Split('.').Select (x => x.Replace("[","").Replace("]","")).ToArray();
    if(nameParts.Length!=3)
        throw new InvalidArgumentException("table_Name");
    return con.Query<string>("select COLUMN_NAME from "+nameParts[0]+".INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='"+nameParts[2]+"'");
}
public static class JsonEx
{
    public static IEnumerable<T> Cache<T>(this IEnumerable<T> src, string name, bool clear=false)
    {
        var fullFileName = Path.Combine((string)Environment.GetEnvironmentVariables()["TEMP"], name + ".json");
        if(!File.Exists(fullFileName) || clear)
        {
            var list = new List<string>();
            foreach(var o in src)
            {
                list.Add(Newtonsoft.Json.JsonConvert.SerializeObject(o));
            }
            
            var listSer = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            File.WriteAllText(fullFileName, listSer);
            return src;
        }
        
        try{
            return Newtonsoft.Json
                        .JsonConvert
                        .DeserializeObject<List<string>>(File.ReadAllText(fullFileName))
                        .Select (x => Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(x,default(T) ));
            
                }
        catch{
            return src.Cache(name, true);    
        }
            
    }
}
// Define other methods and classes here
public static class EtlEx
{
    class EnumerableInputOperation<T> : AbstractOperation
    {
        IEnumerable<T> _src;
        public EnumerableInputOperation(IEnumerable<T> src)
        {
            _src = src;
        }
        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (var s in _src)
            {
                yield return Row.FromObject(s);
            }
            yield break;
        }
    }
    class AnonymousEtlProcess : EtlProcess
    {
        AbstractOperation[] _operations;
        public AnonymousEtlProcess(AbstractOperation[] operations)
        {
            _operations=operations;
        }
        
        protected override void Initialize()
        {
            foreach (var op in _operations)
            {
                 Register(op);
            }
        }
    }
    public class MapDefinition
    {
        Dictionary<string,Type> _maps;
        
        public MapDefinition(Dictionary<string,Type> maps)
        {
            _maps = maps;
        }
        public MapDefinition Map<T>(params string[] colNames)
        {
            foreach(var colName in colNames)
                _maps.Add(colName, typeof(T));
           return this;
        }
    }
    public class DumpToSqlOperation : SqlBulkInsertOperation
    {
        Dictionary<string,Type> _maps = new Dictionary<string,Type>();
        public DumpToSqlOperation(string tableName, Dictionary<string,Type> maps) : base(new System.Configuration.ConnectionStringSettings("aa"
                                        ,"Data Source=Triton;User ID=sa;Password=ssa$20;Initial Catalog=DazgvevaGanckhadebebi;app=LINQPad [Query 7]"
                                        ,"System.Data.SqlClient.SqlConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
            ,tableName)
        {
            _maps = maps;
        }
        protected override void PrepareSchema()
        {
            foreach (var m in _maps)
            {
                Schema[m.Key] = m.Value;
            }
        }
    }
    
    public static AbstractOperation DumpToSql(string tableName, Action<MapDefinition> mapAction)
    {
        var maps=new Dictionary<string,Type>();
        mapAction(new MapDefinition(maps));
        return new DumpToSqlOperation(tableName, maps);
    }
    
    public static AbstractOperation EtlInput<T>(this IEnumerable<T> src)
    {
        return new EnumerableInputOperation<T>(src);
    }
    
    public static EtlProcess Process(params AbstractOperation[] operations)
    {
        return new AnonymousEtlProcess(operations);
    }
}

public static class EnumerableEx
{
    public static IEnumerable<T> Daakeshire<T>(this IEnumerable<T> src, int count)
    {
        
       return null;
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


public static class Ex2
{
	public static IEnumerable<T> Show<T>(this IEnumerable<T> src,string name=null, Action done = null)
	{
		
		var grid = new DataGrid(){ ItemsSource= src, AutoGenerateColumns=false,IsReadOnly=true };
		var props = src.First ().GetType().GetProperties(BindingFlags.Instance|BindingFlags.Public).Select (x => new {x.Name,Type=x.PropertyType});
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
		PanelManager.DisplayWpfElement (g,name);
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
		var b =new Button(){ Content = src ,HorizontalContentAlignment=HorizontalAlignment.Left};
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
    
    public static int LevenshteinDistance(this string source, string target){
            if(String.IsNullOrEmpty(source)){
                    if(String.IsNullOrEmpty(target)) return 0;
                        return target.Length;
            }
            if(String.IsNullOrEmpty(target)) return source.Length;
    
            if(source.Length > target.Length){
                    var temp = target;
                    target = source;
                    source = temp;
            }
    
            var m = target.Length;
            var n = source.Length;
            var distance = new int[2, m + 1];
            // Initialize the distance 'matrix'
            for(var j = 1; j <= m; j++) distance[0, j] = j;
    
            var currentRow = 0;
            for(var i = 1; i <= n; ++i){
                    currentRow = i & 1;
                    distance[currentRow, 0] = i;
                    var previousRow = currentRow ^ 1;
                    for(var j = 1; j <= m; j++){
                            var cost = (target[j - 1] == source[i - 1] ? 0 : 1);
                            distance[currentRow, j] = Math.Min(Math.Min(
                                                    distance[previousRow, j] + 1,
                                                    distance[currentRow, j - 1] + 1),
                                                    distance[previousRow, j - 1] + cost);
                    }
            }
            return distance[currentRow, m];
    }

}

	
void RunInTransaction(Action<IDbConnection> action)
{
	try
	{
		using(var con = new SqlConnection("Data Source=Triton;User ID=sa;Password=ssa$20;Initial Catalog=Pirvelckaroebi;app=LINQPad SHEDAREBAAA"))
		{
			using(var tr = new TransactionScope(TransactionScopeOption.RequiresNew,TimeSpan.FromDays(1)))
			{
				con.Open();
				action(con);
				tr.Complete();
			}
		con.Close();
		}
	}
	catch(Exception ex)
	{
		throw new AggregateException(ex);
	}
}