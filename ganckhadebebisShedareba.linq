<Query Kind="Program">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAAAD89x4SL38S/4r7NUU2iHNNTcmnVTi1xQPx1AC1vAtFAAAAAAOgAAAAAIAACAAAABgO+xVBio0IKzceIXWbWfgFv0jcxQpOA9YilhDtPA8XxAAAABJcM5+MLInsGd5jUUGfXtnQAAAALHy7sVof5cKLhfpSHxbLdPESALwKOWLElOgeYcZmaeqO1sDG9SHnPN5xOzSlBHlSD7agk+KC9dH/4XMZn4gYvM=</Password>
    <Database>DazgvevaGanckhadebebi</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>&lt;RuntimeDirectory&gt;\Accessibility.dll</Reference>
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
  <Reference>&lt;RuntimeDirectory&gt;\System.Xaml.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\wpf\UIAutomationProvider.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\wpf\UIAutomationTypes.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\wpf\WindowsBase.dll</Reference>
  <GACReference>System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</GACReference>
  <GACReference>System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</GACReference>
  <NuGetReference>Dapper</NuGetReference>
  <NuGetReference>Rhino-Etl</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>Ganckhadebebi.Domain</Namespace>
  <Namespace>Itenso.TimePeriod</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Common</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Dmf</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Smo</Namespace>
  <Namespace>Rhino.Etl.Core</Namespace>
  <Namespace>Rhino.Etl.Core.Operations</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Collections.ObjectModel</Namespace>
  <Namespace>System.Data.Common</Namespace>
  <Namespace>System.Windows</Namespace>
  <Namespace>System.Windows.Controls</Namespace>
  <Namespace>System.Windows.Data</Namespace>
  <Namespace>System.Windows.Markup</Namespace>
</Query>

void Main()
{

    var con = this.Connection;
    con.Open();
    Func<DateTime,DateTime> round = dt => new DateTime(dt.Year,dt.Month,1);
    
    var dzvelebi = con.Query(@"select g.Id,g.Unnom, g.Base_Type, g.RegistraciisTarigi, g.DadasturebisTarigi
from ( select t0.* 
       from DazgvevaGanckhadebebi..Ganckhadebebi t0
       where not exists (select null from DazgvevaGanckhadebebi..GaukmebuliGanckhadebebi t1  where t1.GaukmebuliGanckhId=t0.Id)
     ) g
left join DazgvevaGanckhadebebi..Ganckhadebebi2 g2 on g2.Unnom = g.Unnom 
                                                  and g.Base_Type = g2.Base_Type
                                                  and year(g.RegistraciisTarigi)=year(g2.RegistraciisTarigi) 
                                                  and month(g.RegistraciisTarigi)=month(g2.RegistraciisTarigi) 
                                                  and year(g.DadasturebisTarigi)=year(g2.DadasturebisTarigi) 
                                                  and month(g.DadasturebisTarigi)=month(g2.DadasturebisTarigi) 
                                                  
where g2.Unnom is null ")
                .Select (x =>new {Id=(int)x.Id, Unnom=(int)x.Unnom, Base_Type=(int)x.Base_Type, Per = new TimeRange(round(x.RegistraciisTarigi), round(x.DadasturebisTarigi), true)})
                .ToList();
                
    var akhlebi = con.Query(@"select g.Id,g.Unnom, g.Base_Type, g.RegistraciisTarigi, g.DadasturebisTarigi
from DazgvevaGanckhadebebi..Ganckhadebebi2 g
left join ( select t0.* 
            from DazgvevaGanckhadebebi..Ganckhadebebi t0
            where not exists (select null from DazgvevaGanckhadebebi..GaukmebuliGanckhadebebi t1  where t1.GaukmebuliGanckhId=t0.Id)
            ) g2 on g2.Unnom = g.Unnom 
                                                  and g.Base_Type = g2.Base_Type
                                                  and year(g.RegistraciisTarigi)=year(g2.RegistraciisTarigi) 
                                                  and month(g.RegistraciisTarigi)=month(g2.RegistraciisTarigi) 
                                                  and year(g.DadasturebisTarigi)=year(g2.DadasturebisTarigi) 
                                                  and month(g.DadasturebisTarigi)=month(g2.DadasturebisTarigi) 
where g2.Unnom is null ")
                .Select (x =>new {Id=(int)x.Id, Unnom=(int)x.Unnom, Base_Type=(int)x.Base_Type, Per=new TimeRange(round(x.RegistraciisTarigi),round(x.DadasturebisTarigi),true)})
                .ToList();
                
dzvelebi .Count() .Dump("Dzvelebi");
akhlebi  .Count() .Dump("Akhlebi");
return;
var arascorebi = (   
    from a in akhlebi
    from d in dzvelebi
    where d.Base_Type==a.Base_Type && d.Unnom==a.Unnom 
    select new {a,d}
).ToList();

// .GroupBy (x => new{x.a.Id,x.a.Base_Type})
// .Select (g => new {g.Key.Base_Type,list=g.Take(10).ToList()})
// .GroupBy (x => x.Base_Type).Select (g => new {g.Key,Raod=g.Count (),List=g.ToList()})
// .Select (x => new {x.a.Id,OldId=x.d.Id})
var p =EtlEx
    .Process(arascorebi
                    .Select (x => new {AkhaliGanckhId=x.a.Id, GaukmebuliGanckhId=x.d.Id, Mizezi="ArascoredGansazgvruliPeriodi"})
                    .EtlInput()
            ,EtlEx.DumpToSql("GaukmebuliGanckhadebebi", (m)=> m.Map<int>("AkhaliGanckhId","GaukmebuliGanckhId").Map<string>("Mizezi"))
            );
    p.Execute();
p.Dump();


var arUndaGansazgvrodat = (   
    from d in dzvelebi.Where (dz => arascorebi.All(x => x.d.Id != dz.Id))
    from a in akhlebi.Where (x => x.Base_Type==d.Base_Type && x.Unnom==d.Unnom).DefaultIfEmpty()
    where a == null
    select new {d,a}
).ToList();

p =EtlEx.Process(arUndaGansazgvrodat
    .Select (x => new { GaukmebuliGanckhId=x.d.Id, Mizezi="ArUndaGansazgvrodatPeriodi"})
    .EtlInput()
    ,EtlEx.DumpToSql("GaukmebuliGanckhadebebi", (m)=> m.Map<int>("GaukmebuliGanckhId").Map<string>("Mizezi"))
    );
    p.Execute();
    p.Dump();
return;  

arUndaGansazgvrodat .GroupBy (x => x.d.Base_Type)
 .Select (g => new {g.Key,Raod=g.Count ()})
 .Dump("ar unda gansazgvrodat periodi");

return;
(   from a in akhlebi
    from d in dzvelebi.Where (x => x.Base_Type==a.Base_Type && x.Unnom==a.Unnom).DefaultIfEmpty()
    where d == null
    select new {d,a}
).GroupBy (x => x.a.Base_Type)
 .Select (g => new {g.Key,Raod=g.Count ()})
 .Dump("Akhali");
}

public class PrintNumbersProcess : EtlProcess
{
	protected override void Initialize()
	{
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
                _maps.Add(colName,typeof(T));
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
	public static IEnumerable<T> Show<T>(this IEnumerable<T> src, Action done = null)
	{
		
		var grid = new DataGrid(){ ItemsSource= src, AutoGenerateColumns=false,IsReadOnly=true };
		var props = typeof(T).GetProperties(BindingFlags.Instance|BindingFlags.Public).Select (x => new {x.Name,Type=x.PropertyType});
        props.Dump();
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