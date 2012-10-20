<Query Kind="Statements">
  <Connection>
    <ID>453dabbc-e8b3-4c04-8eec-536e8e4e7b58</ID>
    <Persist>true</Persist>
    <Server>triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <NoPluralization>true</NoPluralization>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA3XCANzX8A0+HtkxmZtXFGAAAAAACAAAAAAAQZgAAAAEAACAAAACp+sEJqJxOPz2BQ1lxRRtXH/XJirk9/kw8mJj0bPf5jQAAAAAOgAAAAAIAACAAAACZd/POK3nxMSCEaCgbVDIUSs/pSsra35l5MYJ8LGMjExAAAABDe6Hrhl+CP58Aq2DzWMGNQAAAAA6bC1RC4u17G4KQF35FDnGlInhYWmmeVWDNOqKPGjJdSaTe9PMIuov4DO+z/r3whP06rpngcdEwF0GcF80cZT0=</Password>
    <IncludeSystemObjects>true</IncludeSystemObjects>
    <Database>SocialuriDazgveva</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

var maxTarigebi= PolisisStatusisCvlilebebi
					.GroupBy (psc => psc.PolisisNomeri)
					.Select (g => new {PolisisNomeri = g.Key, MaxTarigi = g.Max (x => x.Tarigi)});
					
var boloCvlileba = from c in PolisisStatusisCvlilebebi
				   join m in maxTarigebi on new {c.PolisisNomeri, c.Tarigi} equals new {m.PolisisNomeri,Tarigi=m.MaxTarigi }
				   select c;

(	from p in Polisebi
	join  c in boloCvlileba on p.PolisisNomeri equals c.PolisisNomeri 
	where p.PolisisStatusi != c.Statusi
	select new {p.PolisisNomeri, p.PolisisStatusi, c.Statusi}
	).Dump();

