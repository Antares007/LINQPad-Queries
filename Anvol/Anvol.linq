<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\Accessibility.dll</Reference>
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
  <NuGetReference>iTextSharp</NuGetReference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>iTextSharp.text.pdf</Namespace>
  <Namespace>iTextSharp.text.pdf.parser</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
  <Namespace>System.Reactive.Disposables</Namespace>
  <Namespace>System.Reactive.Joins</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Reactive.PlatformServices</Namespace>
  <Namespace>System.Reactive.Subjects</Namespace>
  <Namespace>System.Reactive.Threading.Tasks</Namespace>
  <Namespace>System.Windows</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
	DirSearch(@"D:\Anvol")
		.Select (x => ReadPdfFile(x))
		.Select(x => ParseInvoice(x))
		.SelectMany (x => x)
		.Select (x => new{ean=x[1],art=x[2]})
		.Distinct()
		.Dump();
}

public IEnumerable<string[]> ParseInvoice(string str)
{
	return str.Replace("\t"," ")
			.Replace("  "," ").Replace("  "," ").Replace("  "," ").Replace("  "," ").Replace("  "," ").Replace("  "," ")
			.Replace("  "," ").Replace("  "," ").Replace("  "," ").Replace("  "," ").Replace("  "," ").Replace("  "," ")
			.Split(new []{'\n'})
			.Select (l => l.Split(new []{' '}))
			.Where (a => a.Length>=7 && a[1].All(Char.IsNumber));
}

public string ReadPdfFile(object Filename)
{
	string strText = string.Empty;
	PdfReader reader = new PdfReader((string)Filename);
	for (int page = 1; page <= reader.NumberOfPages; page++)
	{
		ITextExtractionStrategy its = new LocationTextExtractionStrategy2();
		String s = PdfTextExtractor.GetTextFromPage(reader, page, its);
		
		s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
		strText = strText + s;
	}
	reader.Close();
	return strText;
}

IEnumerable<string> DirSearch(string sDir) 
{
   foreach (string d in Directory.GetDirectories(sDir)) 
   {
		foreach (string f in Directory.GetFiles(d, "*.pdf")) 
		{
			yield return f;
		}
		DirSearch(d);
   }
}

public class LocationTextExtractionStrategy2 : ITextExtractionStrategy, IRenderListener
  {
    private List<LocationTextExtractionStrategy2.TextChunk> locationalResult = new List<LocationTextExtractionStrategy2.TextChunk>();
    public static bool DUMP_STATE;

    public virtual void BeginTextBlock()
    {
    }

    public virtual void EndTextBlock()
    {
    }

    private bool StartsWithSpace(string str)
    {
      if (str.Length == 0)
        return false;
      else
        return (int) str[0] == 32;
    }

    private bool EndsWithSpace(string str)
    {
      if (str.Length == 0)
        return false;
      else
        return (int) str[str.Length - 1] == 32;
    }

    public virtual string GetResultantText()
    {
      if (LocationTextExtractionStrategy2.DUMP_STATE)
        this.DumpState();
      this.locationalResult.Sort();
      StringBuilder stringBuilder = new StringBuilder();
      LocationTextExtractionStrategy2.TextChunk textChunk1 = (LocationTextExtractionStrategy2.TextChunk) null;
      foreach (LocationTextExtractionStrategy2.TextChunk textChunk2 in this.locationalResult)
      {
        if (textChunk1 == null)
          stringBuilder.Append(textChunk2.text);
        else if (textChunk2.SameLine(textChunk1))
        {
          float num = textChunk2.DistanceFromEndOf(textChunk1);
          if ((double) num < -(double) textChunk2.charSpaceWidth)
            stringBuilder.Append(' ');
          else if ((double) num > (double) textChunk2.charSpaceWidth / 2.0 && !this.StartsWithSpace(textChunk2.text) && !this.EndsWithSpace(textChunk1.text))
            stringBuilder.Append(' ');
          stringBuilder.Append(textChunk2.text);
        }
        else
        {
          stringBuilder.Append('\n');
          stringBuilder.Append(textChunk2.text);
        }
        textChunk1 = textChunk2;
      }
      return ((object) stringBuilder).ToString();
    }

    private void DumpState()
    {
      foreach (LocationTextExtractionStrategy2.TextChunk textChunk in this.locationalResult)
      {
        textChunk.PrintDiagnostics();
        Console.WriteLine();
      }
    }

    public virtual void RenderText(TextRenderInfo renderInfo)
    {
      LineSegment lineSegment = renderInfo.GetBaseline();
      if ((double) renderInfo.GetRise() != 0.0)
      {
        Matrix m = new Matrix(0.0f, -renderInfo.GetRise());
        lineSegment = lineSegment.TransformBy(m);
      }
      this.locationalResult.Add(new TextChunk(renderInfo.GetText(), lineSegment.GetStartPoint(), lineSegment.GetEndPoint(), renderInfo.GetSingleSpaceWidth()));
    }

    public void RenderImage(ImageRenderInfo renderInfo)
    {
    }

    private class TextChunk : IComparable<LocationTextExtractionStrategy2.TextChunk>
    {
      internal string text;
      internal iTextSharp.text.pdf.parser.Vector startLocation;
      internal iTextSharp.text.pdf.parser.Vector endLocation;
      internal iTextSharp.text.pdf.parser.Vector orientationVector;
      internal int orientationMagnitude;
      internal int distPerpendicular;
      internal float distParallelStart;
      internal float distParallelEnd;
      internal float charSpaceWidth;

      public TextChunk(string str, iTextSharp.text.pdf.parser.Vector startLocation, iTextSharp.text.pdf.parser.Vector endLocation, float charSpaceWidth)
      {
        this.text = str;
        this.startLocation = startLocation;
        this.endLocation = endLocation;
        this.charSpaceWidth = charSpaceWidth;
        var vector = endLocation.Subtract(startLocation);
        if ((double) vector.Length == 0.0)
          vector = new iTextSharp.text.pdf.parser.Vector(1f, 0.0f, 0.0f);
        this.orientationVector = vector.Normalize();
        this.orientationMagnitude = (int) (Math.Atan2((double) this.orientationVector[1], (double) this.orientationVector[0]) * 1000.0);
        iTextSharp.text.pdf.parser.Vector v = new iTextSharp.text.pdf.parser.Vector(0.0f, 0.0f, 1f);
        this.distPerpendicular = (int) startLocation.Subtract(v).Cross(this.orientationVector)[2];
        this.distParallelStart = this.orientationVector.Dot(startLocation);
        this.distParallelEnd = this.orientationVector.Dot(endLocation);
      }

      public void PrintDiagnostics()
      {
        Console.WriteLine("Text (@" + (object) this.startLocation + " -> " + (string) (object) this.endLocation + "): " + this.text);
        Console.WriteLine("orientationMagnitude: " + (object) this.orientationMagnitude);
        Console.WriteLine("distPerpendicular: " + (object) this.distPerpendicular);
        Console.WriteLine("distParallel: " + (object) this.distParallelStart);
      }

      public bool SameLine(LocationTextExtractionStrategy2.TextChunk a)
      {
        return (this.orientationMagnitude == a.orientationMagnitude && this.distPerpendicular == a.distPerpendicular)
			|| (this.orientationMagnitude == a.orientationMagnitude && this.distPerpendicular+1 == a.distPerpendicular)
			|| (this.orientationMagnitude == a.orientationMagnitude && this.distPerpendicular-1 == a.distPerpendicular)
			|| (this.orientationMagnitude == a.orientationMagnitude && this.distPerpendicular+2 == a.distPerpendicular)
			|| (this.orientationMagnitude == a.orientationMagnitude && this.distPerpendicular-2 == a.distPerpendicular)
			;
      }

      public float DistanceFromEndOf(LocationTextExtractionStrategy2.TextChunk other)
      {
        return this.distParallelStart - other.distParallelEnd;
      }

      public int CompareTo(LocationTextExtractionStrategy2.TextChunk rhs)
      {
        if (this == rhs)
          return 0;
        int num1 = LocationTextExtractionStrategy2.TextChunk.CompareInts(this.orientationMagnitude, rhs.orientationMagnitude);
        if (num1 != 0)
          return num1;
        int num2 = LocationTextExtractionStrategy2.TextChunk.CompareInts(this.distPerpendicular, rhs.distPerpendicular);
        if (num2 != 0)
          return num2;
        else
          return (double) this.distParallelStart < (double) rhs.distParallelStart ? -1 : 1;
      }

      private static int CompareInts(int int1, int int2)
      {
        if (int1 == int2)
          return 0;
        return int1 >= int2 ? 1 : -1;
      }
    }
  }