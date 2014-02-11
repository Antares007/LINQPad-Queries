<Query Kind="Program">
  <Reference>C:\pkg\Newtonsoft.Json.4.0.2\lib\net40\Newtonsoft.Json.dll</Reference>
  <Reference>C:\pkg\NLog.2.0.0.2000\lib\net40\NLog.dll</Reference>
  <Reference>C:\pkg\RavenDB.1.0.499\lib\net40\Raven.Abstractions.dll</Reference>
  <Reference>C:\pkg\RavenDB.1.0.499\lib\net40\Raven.Client.Lightweight.dll</Reference>
  <Namespace>Raven.Client.Document</Namespace>
  <Namespace>Raven.Client.Linq</Namespace>
</Query>

void Main()
{
	using (var store = new DocumentStore{Url = "http://172.17.7.40:8080",Conventions = { FindTypeTagName = type => type.Name }}.Initialize())
	using (var session = store.OpenSession())
	{
		RavenQueryStatistics stat;
    	session	.Query<Polisi>()
				.Statistics(out stat)
				.Count()
				.Dump();
		stat.Dump();
    }
}

public class Polisi
{
   private string _id;
   private string _polisisNomeri;

   public string Id
   {
       get { return _id; }
       set
       {
           _id = value;
           _polisisNomeri = value.Split('/').Last();
       }
   }

   public string PolisisNomeri
   {
       get { return _polisisNomeri; }
       set
       {
           _id = "polisi/" + value;
           _polisisNomeri = value;
       }
   }

   public int UnikaluriKodi { get; set; }
   public string Fid { get; set; }
   public string PiradiNomeri { get; set; }
   public string DocumentisNomeri { get; set; }
   public string Sakheli { get; set; }
   public string Gvari { get; set; }
   public DateTime? DabadebisTarigi { get; set; }
   public string RaionisKodi { get; set; }
   public string Raioni { get; set; }
   public string Kalaki { get; set; }
   public string Sopeli { get; set; }
   public string SapostoIndeksi { get; set; }
   public string SruliMisamarti { get; set; }
   public string MzgveveliKompaniisKodi { get; set; }
   public string MzgveveliKompania { get; set; }
   public Guid DamzgvevisId { get; set; }
   public string DacesebulebisDasaxeleba { get; set; }
   public string ProgramisDasakheleba { get; set; }
   public int ProgramisId { get; set; }
   public PolisisMdgomareoba Mdgomareoba { get; set; }

   public DateTime ShekmnisTarigi { get; set; }
   public DateTime ChabarebisBoloVada { get; set; }
   public DateTime ChabarebisTarigi { get; set; }
}

public enum PolisisMdgomareoba
{
   Dasabechdi,
   Chasabarebeli,
   Chabarebuli,
   Gaukmebuli,
   AdgilidanGasacemi,
}