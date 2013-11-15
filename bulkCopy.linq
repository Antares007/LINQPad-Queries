<Query Kind="Program">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAMu0OrNIzSEGjR+CwDmJgrQAAAAACAAAAAAAQZgAAAAEAACAAAACNlqikwyMoMt0aiDtLETWEoUa02do8Ic8jDEnYvjYqfgAAAAAOgAAAAAIAACAAAACZpJJqd8ImbRDzbLE0yBTauvv7Cs2rbmnCGgwx3ZA0dBAAAABGBIxYe471vKmpSzOsZksBQAAAAG0vf+zY2dbQRaaUuN+35670V5n4AeLLNiTQswKIShlclPxlm4FfodwyXTe5QtTNvT/37vie5Pzve1y4qmMBByE=</Password>
    <Database>Pirvelckaroebi</Database>
    <ShowServer>true</ShowServer>
    <LinkedDb>DazgvevaGanckhadebebi</LinkedDb>
    <LinkedDb>INSURANCEW</LinkedDb>
    <LinkedDb>Pirvelckaroebi</LinkedDb>
    <LinkedDb>SocialuriDazgveva</LinkedDb>
    <LinkedDb>UketesiReestri</LinkedDb>
  </Connection>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <NuGetReference>EPPlus</NuGetReference>
  <NuGetReference>LinqToExcel_x64</NuGetReference>
  <NuGetReference>Rhino-Etl</NuGetReference>
  <Namespace>LinqToExcel</Namespace>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>Rhino.Etl.Core.Operations</Namespace>
  <Namespace>System.Configuration</Namespace>
  <Namespace>Rhino.Etl.Core</Namespace>
</Query>


public class ExcelInputOperation : AbstractOperation
{
	public string FileName {get;set;}
	public string SheetName {get;set;}
	
	IEnumerable<Rhino.Etl.Core.Row> GetRows()
	{
		var fi = new FileInfo(FileName);
		var ep = new ExcelPackage(fi);
		
		var lastRowNo = -1;
		var lastRow = default(Rhino.Etl.Core.Row);
		
		var cells = ep	.Workbook
						.Worksheets[SheetName]
						.Cells
						.Select (c => 
							new {
									c,
									col = c.Address.Substring(0, c.Address.IndexOfAny(new []{'0','1','2','3','4','5','6','7','8','9'})),
									row = int.Parse(c.Address.Substring(   c.Address.IndexOfAny(new []{'0','1','2','3','4','5','6','7','8','9'})))
								});
		foreach (var c in cells)
		{
			if(lastRowNo != c.row)
			{
				lastRowNo = c.row;
				if(lastRow != null)
				{
					yield return lastRow;
				}
				lastRow = new Rhino.Etl.Core.Row();
			}
			lastRow[c.col] = c.c.Text;
		}
		if(lastRow != null)
		{
			yield return lastRow;
		}
	}
	
	public override IEnumerable<Rhino.Etl.Core.Row> Execute(IEnumerable<Rhino.Etl.Core.Row> rows)
	{
		var dict = default(Rhino.Etl.Core.Row);
		
		foreach(var row in GetRows())
		{
			if (null == dict)
			{
				dict = row;
				continue;
			}
			foreach(var k in dict.Keys)
			{
				row[dict[k]]=row[k];
			}
			yield return row;
		}
	}
}

public class BazashiChakra : SqlBulkInsertOperation
{
	Action<SchemaBuilder> _schemaBuilder;
	public BazashiChakra(string connStringName, string tableName, Action<SchemaBuilder> schemaBuilder) : base(connStringName, tableName)
	{
		_schemaBuilder = schemaBuilder;
	}
	
	public override void PrepareMapping()
	{
		base.PrepareMapping();
	}
	
	protected override void PrepareSchema()
	{
		_schemaBuilder(new SchemaBuilder(this));
	}
	
	public class SchemaBuilder
	{
		BazashiChakra _bc;
		public SchemaBuilder(BazashiChakra bc)
		{
			_bc = bc;
		}
		public SchemaBuilder Map<T>(string colName)
		{
			_bc.Schema[colName] = typeof(T);
			return this;
		}
	}
}

public class Transform : AbstractOperation
{
	public Func<IEnumerable<Rhino.Etl.Core.Row>,IEnumerable<Rhino.Etl.Core.Row>> Func {get;set;}
	public override IEnumerable<Rhino.Etl.Core.Row> Execute(IEnumerable<Rhino.Etl.Core.Row> rows)
	{
		return Func(rows);
	}
}

public class ImportChabarebuliPaketebiToSql : EtlProcess
{
	string _fileName;
	public ImportChabarebuliPaketebiToSql(string fileName)
	{
		_fileName = fileName;
	}
	protected override void Initialize()
   	{
		 Register(new ExcelInputOperation { FileName=_fileName, SheetName="Data" } );
		 Register(new Transform{Func = rows => 	(from r in rows.Select (c => new { 
		 															MzgveveliKompaniisKodi 	= (string)c["MzgveveliKompaniisKodi"],
		 															Periodi 				= (string)c["Periodi"],
																	PaketisNomeri 			= (string)c["PaketisNomeri"],
																	Dadgenileba 			= (string)c["Dadgenileba"],
																	VizitisTarigi 			= (string)c["VizitisTarigi"],
																	Polisebi				= (string)c["Polisebi"]})
												let pols = r.Polisebi.Split(new [] { ")," }, StringSplitOptions.RemoveEmptyEntries)
												                     .Select (p => p.Split(new [] { " ", ",", "(", ")", "-" }, StringSplitOptions.RemoveEmptyEntries))
												from 	p in pols
												select 	new  
													{	
														FileName 				= Path.GetFileNameWithoutExtension(_fileName),
														Dadgenileba 			= r.Dadgenileba,
														MzgveveliKompaniisKodi 	= r.MzgveveliKompaniisKodi,
														PaketisNomeri 			= r.PaketisNomeri,
														Periodi 				= r.Periodi,
														VizitisTarigi 			= r.VizitisTarigi, 
														PolisisNomeri 			= p[0],
														ChabarebisStatusi 		= p[1],
														PolisisStatusi 			= p.Length > 2 ? p[2] : null,
													}).Select (c => Rhino.Etl.Core.Row.FromObject(c)) } );
		 Register(new BazashiChakra("triton", "SocialuriDazgveva..Test98761", s=>s	.Map<string>("FileName")
		 																			.Map<string>("Dadgenileba")
																					.Map<string>("MzgveveliKompaniisKodi")
																					.Map<string>("PaketisNomeri")
																					.Map<string>("Periodi")
																					.Map<string>("VizitisTarigi")
																					.Map<string>("PolisisNomeri")
																					.Map<string>("ChabarebisStatusi")
																					.Map<string>("PolisisStatusi")));
	}
	public static EtlProcess Create(string fileName)
	{
		return new ImportChabarebuliPaketebiToSql(fileName);
	}
}

public class ImportGankhorcielebuliVizitebiToSql : EtlProcess
{
	string _fileName;
	public ImportGankhorcielebuliVizitebiToSql(string fileName)
	{
		_fileName = fileName;
	}
	protected override void Initialize()
   	{
		 Register(new ExcelInputOperation { FileName=_fileName, SheetName="Data" } );
		 Register(new Transform{Func = rows => 	(from r in rows.Select (c => new { 
		 															PaketisNomeri 			= (string)c["PaketisNomeri"],
		 															Chabarda 				= (string)c["Chabarda"],
																	KontraktiGaformda 		= (string)c["KontraktiGaformda"],
																	Chambarebeli 			= (string)c["Chambarebeli"],
																	Periodi 				= (string)c["Periodi"],
																	Polisebi				= (string)c["Polisebi"],
																	Dadgenileba				= (string)c["Dadgenileba"],
																	VizitisTarigi			= (string)c["VizitisTarigi"],
																	VizitisPeriodi			= (string)c["VizitisPeriodi"]
																	})
												let pols = r.Polisebi.Split(new [] { ")," }, StringSplitOptions.RemoveEmptyEntries)
												                     .Select (p => p.Split(new [] { " ", ",", "(", ")", "-" }, StringSplitOptions.RemoveEmptyEntries))
												from 	p in pols
												
												select 	new  
													{	
														FileName 				= Path.GetFileNameWithoutExtension(_fileName),
														PaketisNomeri 			= r.PaketisNomeri,
														Chabarda 				= r.Chabarda,
														KontraktiGaformda 		= r.KontraktiGaformda,
														Chambarebeli 			= r.Chambarebeli,
														Periodi 				= r.Periodi,
														Dadgenileba 			= r.Dadgenileba,
														VizitisTarigi 			= r.VizitisTarigi,
														VizitisPeriodi 			= r.VizitisPeriodi,
														PolisisNomeri 			= p[0],
														ChabarebisStatusi 		= p[1],
														PolisisStatusi 			= p.Length > 2 ? p[2] : null,
													}).Select (c => Rhino.Etl.Core.Row.FromObject(c)) } );
		 Register(new BazashiChakra("triton","SocialuriDazgveva..Test98769", s=>s	.Map<string>("FileName")
		 																			.Map<string>("PaketisNomeri")
		 																			.Map<string>("Chabarda")
		 																			.Map<string>("KontraktiGaformda")
		 																			.Map<string>("Chambarebeli")
		 																			.Map<string>("Periodi")
		 																			.Map<string>("Dadgenileba")
		 																			.Map<string>("VizitisTarigi")
		 																			.Map<string>("VizitisPeriodi")
		 																			.Map<string>("PolisisNomeri")
		 																			.Map<string>("ChabarebisStatusi")
		 																			.Map<string>("PolisisStatusi")
																					) );
	}
	public static EtlProcess Create(string fileName)
	{
		return new ImportGankhorcielebuliVizitebiToSql(fileName);
	}

}

void Main()
{
	return;
	foreach (var file in Directory.EnumerateFileSystemEntries("c:\\temp","ChabarebuliPaketebi*.xlsx")
									.Select (fn => new FileInfo(fn))
									.OrderByDescending (fn => fn.LastWriteTime)
									.Select (fn => fn.FullName))
	using (var push = ImportChabarebuliPaketebiToSql.Create(file))
	{
		push.Execute();
		Console.WriteLine(file + " " + string.Join(", ", push .GetAllErrors() .Select(x => x.Message) ) );
	}
}

static class DataReaderExtensions
{
   public static IDataReader AsDataReader<TSource>(this IEnumerable<TSource> source, int fieldCount, Func<TSource, int, object> getValue)
   {
       return new EnumerableDataReader<TSource>(source.GetEnumerator(), fieldCount, getValue); 
   }
}


internal class EnumerableDataReader<TSource> : IDataReader
{
   private readonly IEnumerator<TSource> _source;
   private readonly int _fieldCount;
   private readonly Func<TSource, int, object> _getValue;

   internal EnumerableDataReader(IEnumerator<TSource> source, int fieldCount, Func<TSource, int, object> getValue)
   {
       _source = source;
       _getValue = getValue;
       _fieldCount = fieldCount;
   }

   public void Dispose()
   {
       // Nothing.
   }

   public string GetName(int i)
   {
       throw new NotImplementedException();
   }

   public string GetDataTypeName(int i)
   {
       throw new NotImplementedException();
   }

   public Type GetFieldType(int i)
   {
       throw new NotImplementedException();
   }

   public object GetValue(int i)
   {
       return _getValue(_source.Current, i);
   }

   public int GetValues(object[] values)
   {
       throw new NotImplementedException();
   }

   public int GetOrdinal(string name)
   {
       throw new NotImplementedException();
   }

   public bool GetBoolean(int i)
   {
       throw new NotImplementedException();
   }

   public byte GetByte(int i)
   {
       throw new NotImplementedException();
   }

   public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
   {
       throw new NotImplementedException();
   }

   public char GetChar(int i)
   {
       throw new NotImplementedException();
   }

   public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
   {
       throw new NotImplementedException();
   }

   public Guid GetGuid(int i)
   {
       throw new NotImplementedException();
   }

   public short GetInt16(int i)
   {
       throw new NotImplementedException();
   }

   public int GetInt32(int i)
   {
       throw new NotImplementedException();
   }

   public long GetInt64(int i)
   {
       throw new NotImplementedException();
   }

   public float GetFloat(int i)
   {
       throw new NotImplementedException();
   }

   public double GetDouble(int i)
   {
       throw new NotImplementedException();
   }

   public string GetString(int i)
   {
       throw new NotImplementedException();
   }

   public decimal GetDecimal(int i)
   {
       throw new NotImplementedException();
   }

   public DateTime GetDateTime(int i)
   {
       throw new NotImplementedException();
   }

   public IDataReader GetData(int i)
   {
       throw new NotImplementedException();
   }

   public bool IsDBNull(int i)
   {
       throw new NotImplementedException();
   }

   public int FieldCount
   {
       get { return _fieldCount; }
   }

   object IDataRecord.this[int i]
   {
       get { throw new NotImplementedException(); }
   }

   object IDataRecord.this[string name]
   {
       get { throw new NotImplementedException(); }
   }

   public void Close()
   {
       throw new NotImplementedException();
   }

   public DataTable GetSchemaTable()
   {
       throw new NotImplementedException();
   }

   public bool NextResult()
   {
       throw new NotImplementedException();
   }

   public bool Read()
   {
       return _source.MoveNext();
   }

   public int Depth { get; private set; }
   public bool IsClosed { get; private set; }
   public int RecordsAffected { get; private set; }
}