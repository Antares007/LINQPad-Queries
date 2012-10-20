<Query Kind="Program">
  <Reference>D:\temp\PolGen\PolisisGeneracia.dll</Reference>
  <Namespace>PolisisGeneracia</Namespace>
  <Namespace>System.IO.Compression</Namespace>
  <Namespace>System.Runtime.Serialization.Formatters.Binary</Namespace>
</Query>

void Main()
{
  var pak = GetPaketi();
  pak.Dump();
  File.WriteAllBytes(@"D:\temp\PolGen\pak.new", Serialize(pak));

  Show(@"D:\temp\PolGen\pak.old");
  Show(@"D:\temp\PolGen\pak.new");
}

private static void Show(string file)
{
  var fileName = file + ".pdf";
  var newFromOld = Deserialize<Paketi>(File.ReadAllBytes(file)).Dump();
  File.WriteAllBytes(fileName, newFromOld.GadaikvanePdfShi());
  Process.Start(fileName);
}

private static byte[] Serialize(object paketi)
{
  using (var memoryStream = new MemoryStream())
  using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
  {
    new BinaryFormatter().Serialize(gZipStream, paketi);
    gZipStream.Flush();
    gZipStream.Close();
    return memoryStream.ToArray();
  }
}

private static T Deserialize<T>(byte[]  bites)
{
  using (var memoryStream = new MemoryStream(bites))
  using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
  {
      return (T)new BinaryFormatter().Deserialize(gZipStream);
  }
}

private static Paketi GetPaketi()
{
  var misamarti = "ქუთაისი; ქ. ქუთაისი ქუთაისი ქუთ. თაბუკაშვილის ქ.№ 181 ოთ.59 ქუთაისი; ქ. ქუთაისი ქუთაისი ქუთ. "
                + "თაბუკაშვილის ქ.№ 181 ოთ.59 ქუთაისი; ქ. ქუთაისი ქუთაისი ქუთ. თაბუკაშვილის ქ.№ 181 ოთ.59";

  Func<Mzgveveli, PolisisTipi, Polisi> makePol = (mzgveveli, tipi) => new Polisi
				{
					PolisisNomeri = "006488463",
					OjakhisNomeri = "010103600113",
					PiradiNomeri = "01019073987",
					Sakheli = "ნინო",
					Gvari = "მკრტიჩიანი",
					DabTarigi = DateTime.Parse("2000-10-03"),
					Mzgveveli = mzgveveli,
					PolisisTipi = tipi,
					SadazgvevoPeriodisDasackisi = DateTime.Parse("2012-01-01"),
					SruliMisamarti = misamarti,
					Unnom = 123,
				};

  var polisi = makePol(Mzgveveli.ALP, PolisisTipi.Debuleba218);

  var paketi = new Paketi()
  {
	PaketisNomeri = "9101231233",
	ProgramisDasakheleba = "რეინტეგრაციის და მინდობით აღზრდის ქვეპროგრამის ბენეფიციარები",
	Misamarti = misamarti,
	Polisebi = new List<Polisi>
					{
						polisi
					},
	GadacemisBoloVada = DateTime.Today.AddDays(60),

	Mimgebi = new Pirovneba { Pid = "12345678901", Sakheli = "ნინო", Gvari = "მკრტიჩიანი", DabTarigi = DateTime.Today.AddYears(-65) },
	Gamcemi = new Kontakti { Email = "ggogia@ssa.gov.ge", Sakheli = "გიორგი", Gvari = "გოგია" },
	ChabarebisTarigi = DateTime.Now
  };
  return paketi;
}