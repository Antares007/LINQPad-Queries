<Query Kind="Program">
  <Connection>
    <ID>393fc2a1-3d2f-4fc3-8b9d-c916c0bb1c52</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAN27c6lA2WkeiuPoKsF6zVAAAAAACAAAAAAAQZgAAAAEAACAAAACbLLAsNsDt7paFHc5L9mKrBtKrPuHB+O2Pe8qohNTpzwAAAAAOgAAAAAIAACAAAAARwiWVIvi4yqvM5LGOqZqOnRK7TKpCRjPXZ7PkGNEFvxAAAAA0DMltrt8SinSUpeRajEf6QAAAACzIm5Lx/1cDUpvEPcBjgOVVcG4y9njzOG5jRo3BFxCqffNXq8PX7vXvt6t2LlSsl7mTqha7tgg+9E46F4mXHMQ=</Password>
    <Database>SocialuriDazgveva</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

void Main()
{
    //new []{ 1, 1, 1, 2, 3, 4 }.Except(new []{ 1,2,2 }).Dump();
	var gadasakhdeliPaketebi = ChabarebuliPaketebi()
									.Gamoakeli(GadaxdiliPaketebi(),   Paketi_.ShigtavisShemdarebeli)
									;
	var gamosartmeviPaketebi = ChabarebuliPaketebi()
									.Gamoakeli(GamortmeuliPaketebi(), Paketi_.PaketisNomrisShemdarebeli)
									.DakhliceKompaniebisMikhedvit()
									;
}

public IEnumerable<Paketi_> ChabarebuliPaketebi()
{
	return new List<Paketi_>();
}

public IEnumerable<Paketi_> GadaxdiliPaketebi()
{
	return new List<Paketi_>();
}

public IEnumerable<Paketi_> GamortmeuliPaketebi()
{
	return new List<Paketi_>();
}

public static class PaketiEx
{
	public static IEnumerable<Paketi_> Gamoakeli(this IEnumerable<Paketi_> pirveli, IEnumerable<Paketi_>  meore, IEqualityComparer<Paketi_> shemdarebeli)
	{
		return pirveli.Except(meore, shemdarebeli);
	}
	public static IEnumerable<Paketi_> DakhliceKompaniebisMikhedvit(this IEnumerable<Paketi_> pirveli)
	{
		return pirveli.SelectMany (p => p.DakhlicheMzgveveliKompaniebisMikhedvit());
	}
}

public class Paketi_
{
	public string _key;
	
	public Paketi_(string paketisNomeri, string chambarebeli, string periodi, IEnumerable<Polisi_> polisebi, int vizitisPeriodi)
	{
		if ( polisebi.Select(p => p.Dadgenileba).Distinct().Count() != 1 )
			throw new InvalidOperationException ();
		PaketisNomeri = paketisNomeri;
		Chambarebeli = chambarebeli;
		Periodi = periodi;
		Polisebi = polisebi.ToList ();
		Chabarda = Polisebi.Any (x => x.Statusi == "Chabarda");
		KontraktiGaformda = Polisebi.Any (x => x.GaformdaKontrakti);
		MzgveveliKompaniebi = Polisebi.Select( po => po.MzgveveliKompaniisKodi).Distinct().ToList();
		VizitisPeriodi = vizitisPeriodi;
		Dadgenileba = Polisebi.First().Dadgenileba;
		_key = string.Join("", Polisebi
								  .Select   ( po => po.PolisisNomeri)
								  .Distinct ()
								  .OrderBy  ( x => x) );
	}
	
	public readonly string PaketisNomeri;
	public readonly string Chambarebeli;
	public readonly string Periodi;
	public readonly bool Chabarda;
	public readonly bool KontraktiGaformda;
	public readonly IEnumerable<Polisi_> Polisebi;
	public readonly IEnumerable<string> MzgveveliKompaniebi;
	public readonly int Dadgenileba;
	public readonly DateTime VizitisTarigi;
	public readonly int VizitisPeriodi;
	
	public IEnumerable<Paketi_> DakhlicheMzgveveliKompaniebisMikhedvit()
	{
		if (MzgveveliKompaniebi.Count() == 1) return new[] { this };
		return MzgveveliKompaniebi
					.Select(mk => new Paketi_(PaketisNomeri, Chambarebeli, Periodi, Polisebi.Where(p => p.MzgveveliKompaniisKodi == mk), VizitisPeriodi))
					.ToList();
	}
	
	
	private sealed class KeyEqualityComparer : IEqualityComparer<Paketi_>
	{
		public bool Equals(Paketi_ x, Paketi_ y)
		{
			if (ReferenceEquals(x, y)) return true;
			if (ReferenceEquals(x, null)) return false;
			if (ReferenceEquals(y, null)) return false;
			if (x.GetType() != y.GetType()) return false;
			return string.Equals(x._key, y._key);
		}
	
		public int GetHashCode(Paketi_ obj)
		{
			return (obj._key != null ? obj._key.GetHashCode() : 0);
		}
	}
	
	private static readonly IEqualityComparer<Paketi_> ShigtavisShemdarebeliInstance = new KeyEqualityComparer();
	
	public static IEqualityComparer<Paketi_> ShigtavisShemdarebeli
	{
		get { return ShigtavisShemdarebeliInstance; }
	}
	
	private sealed class PaketisNomeriEqualityComparer : IEqualityComparer<Paketi_>
	{
		public bool Equals(Paketi_ x, Paketi_ y)
		{
			if (ReferenceEquals(x, y)) return true;
			if (ReferenceEquals(x, null)) return false;
			if (ReferenceEquals(y, null)) return false;
			if (x.GetType() != y.GetType()) return false;
			return string.Equals(x.PaketisNomeri, y.PaketisNomeri);
		}
	
		public int GetHashCode(Paketi_ obj)
		{
			return (obj.PaketisNomeri != null ? obj.PaketisNomeri.GetHashCode() : 0);
		}
	}
	
	private static readonly IEqualityComparer<Paketi_> PaketisNomrisShemdarebeliInstance = new PaketisNomeriEqualityComparer();
	
	public static IEqualityComparer<Paketi_> PaketisNomrisShemdarebeli
	{
		get { return PaketisNomrisShemdarebeliInstance; }
	}
}

public class Polisi_
{
	public Polisi_(string polisisNomeri, string mzgveveliKompaniisKodi, string statusi, string polisisStatusi, bool gaformdaKontrakti, int dadgenileba)
	{
		PolisisNomeri = polisisNomeri;
		MzgveveliKompaniisKodi = mzgveveliKompaniisKodi;
		Statusi = statusi;
		PolisisStatusi = polisisStatusi;
		GaformdaKontrakti = gaformdaKontrakti;
		Dadgenileba = dadgenileba;
	}
	
	public readonly string PolisisNomeri;
	public readonly string MzgveveliKompaniisKodi;
	public readonly string Statusi;
	public readonly string PolisisStatusi;
	public readonly bool GaformdaKontrakti;
	public readonly int Dadgenileba;
	
	public override string ToString()
	{
		return string.Format("{0} - ({1}, {2})", PolisisNomeri, Statusi, PolisisStatusi);
	}
}