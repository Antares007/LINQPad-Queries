<Query Kind="Statements">
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
</Query>

var sw= new System.Diagnostics.Stopwatch();
sw.Start();
var datas =  Pirvelckaro_01_UMCEOEBIs
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=1, dt=x.RecDate.Date})
     .Concat(Pirvelckaro_02_DEVNILEBIs
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=2, dt=x.RecDate.Date}))
     .Concat(Pirvelckaro_03_BAVSHVEBIs
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=3, dt=x.RecDate.Date}))
     .Concat(Pirvelckaro_04_REINTEGRACIAs
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=4, dt=x.RecDate.Date}))
     .Concat(Pirvelckaro_05_KULTURAs
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=5, dt=x.RecDate.Date}))
     .Concat(Pirvelckaro_06_XANDAZMULEBIs
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=6, dt=x.RecDate.Date}))
     .Concat(Pirvelckaro_07_SKOLA_PANSIONEBIs
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=7, dt=x.RecDate.Date}))
     .Concat(Pirvelckaro_08_TEACHERS
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=8, dt=x.RecDate.Date}))
     .Concat(Pirvelckaro_09_UFROSI_AGMZRDELEBIs
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=9, dt=x.RecDate.Date}))
     .Concat(Pirvelckaro_10_APKHAZETIS_OJAKHEBIs
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=10,dt=x.RecDate.Date}))
     .Concat(Pirvelckaro_11_SATEMOs
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=11,dt=x.RecDate.Date}))
     .Concat(Pirvelckaro_12_MCIRE_SAOJAXOs
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=12,dt=x.RecDate.Date}))
     .Concat(Pirvelckaro_13_TEACHERS_AFXes
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=13,dt=x.RecDate.Date}))
     .Concat(Pirvelckaro_14_RESURSCENTRIS_TANAMSHROMLEBIs
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=14,dt=x.RecDate.Date}))
     .Concat(Pirvelckaro_21_SAPENSIO_ASAKIS_MOSAXLEOBAs
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=21,dt=x.RecDate.Date}))
     .Concat(Pirvelckaro_22_STUDENTEBIs
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=22,dt=x.RecDate.Date}))
     .Concat(Pirvelckaro_23_BAVSHVEBI165s
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=23,dt=x.RecDate.Date}))
     .Concat(Pirvelckaro_24_INVALIDI_BAVSHVEBIs
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=24,dt=x.RecDate.Date}))
     .Concat(Pirvelckaro_25_MKVETRAD_GAMOXATULI_INVALIDI_BAVSHVEBIs
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=25,dt=x.RecDate.Date}))
     .Concat(Pirvelckaro_26_ARASAQARTVELOS_MOQALAQE_PENSIONREBIs
                .Where (x => x.Unnom.HasValue)
                .Select (x => new{u=x.Unnom,b=26,dt=x.RecDate.Date}))
     .ToArray();
sw.Stop();
sw.ElapsedMilliseconds.Dump("Loaded "+ datas.Length + " records");
sw.Restart();


var results=(
  from d in datas
  select new {d.u, d.b, Pers=new []{new {p=d.dt.Year*100+d.dt.Month, f=d.dt.Year*100+d.dt.Month, t=d.dt.AddMonths(1).Year*100+d.dt.AddMonths(1).Month }}}
).ToArray();
sw.Stop();
sw.ElapsedMilliseconds.Dump("Maped to " + results.Length + " records");
sw.Restart();

var reducedResults =(
  from r in results.AsParallel()
  group r by new{r.u,r.b} into g
  let prs= g.SelectMany (x => x.Pers).OrderBy (x => x.f).ToArray()
  select new {u=g.Key.u,b=g.Key.b,Pers =prs.Aggregate (new LinkedList<dynamic>(),  
    (l,p)=>
        {
            if (l.First == null)
            {   
                l.AddFirst(p);
                return l;
            }
            
            var cp = l.First.Value;
            if(cp.t==p.f)
            {
                l.RemoveFirst();
                l.AddFirst(new {cp.p,cp.f,p.t});
                return l;
            }
            l.AddFirst(p);
            return l;
        }
  )}
).ToArray();

sw.Stop();
sw.ElapsedMilliseconds.Dump("Reduced to " + reducedResults.SelectMany (r => r.Pers).Count() + " records");

(
  from r in reducedResults
  from p in r.Pers
  select new {r.u, r.b, p.f, p.t}
)
.Where (em => em.u == 5328585)
.Dump  ();