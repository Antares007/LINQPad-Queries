<Query Kind="Expression">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAAAD89x4SL38S/4r7NUU2iHNNTcmnVTi1xQPx1AC1vAtFAAAAAAOgAAAAAIAACAAAABgO+xVBio0IKzceIXWbWfgFv0jcxQpOA9YilhDtPA8XxAAAABJcM5+MLInsGd5jUUGfXtnQAAAALHy7sVof5cKLhfpSHxbLdPESALwKOWLElOgeYcZmaeqO1sDG9SHnPN5xOzSlBHlSD7agk+KC9dH/4XMZn4gYvM=</Password>
    <Database>INSURANCEW</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

		
		
		
		DEVNILEBI_201101s.Select (d => new {Base_Type=2,Periodi=201012, d.FID,d.PID,d.FIRST_NAME,d.LAST_NAME,d.BIRTH_DATE,PIROBA=(int?)null})
.Concat(DEVNILEBI_201102s.Select (d => new {Base_Type=2,Periodi=201101, d.FID,d.PID,d.FIRST_NAME,d.LAST_NAME,d.BIRTH_DATE,PIROBA=(int?)null}))
.Concat(DEVNILEBI_201103s.Select (d => new {Base_Type=2,Periodi=201102, d.FID,d.PID,d.FIRST_NAME,d.LAST_NAME,d.BIRTH_DATE,PIROBA=(int?)null}))
.Concat(DEVNILEBI_201104s.Select (d => new {Base_Type=2,Periodi=201103, d.FID,d.PID,d.FIRST_NAME,d.LAST_NAME,d.BIRTH_DATE,PIROBA=d.PIROBA}))
.Concat(DEVNILEBI_201105s.Select (d => new {Base_Type=2,Periodi=201104, d.FID,d.PID,d.FIRST_NAME,d.LAST_NAME,d.BIRTH_DATE,PIROBA=d.PIROBA}))
.Concat(DEVNILEBI_201106s.Select (d => new {Base_Type=2,Periodi=201105, d.FID,d.PID,d.FIRST_NAME,d.LAST_NAME,d.BIRTH_DATE,PIROBA=d.PIROBA}))
.Concat(DEVNILEBI_201107s.Select (d => new {Base_Type=2,Periodi=201106, d.FID,d.PID,d.FIRST_NAME,d.LAST_NAME,d.BIRTH_DATE,PIROBA=d.PIROBA}))
.Concat(DEVNILEBI_201108s.Select (d => new {Base_Type=2,Periodi=201107, d.FID,d.PID,d.FIRST_NAME,d.LAST_NAME,d.BIRTH_DATE,PIROBA=d.PIROBA}))
.Concat(DEVNILEBI_201109s.Select (d => new {Base_Type=2,Periodi=201108, d.FID,d.PID,d.FIRST_NAME,d.LAST_NAME,d.BIRTH_DATE,PIROBA=d.PIROBA}))
.Concat(DEVNILEBI_201110s.Select (d => new {Base_Type=2,Periodi=201109, d.FID,d.PID,d.FIRST_NAME,d.LAST_NAME,d.BIRTH_DATE,PIROBA=d.PIROBA}))
.Concat(DEVNILEBI_201111s.Select (d => new {Base_Type=2,Periodi=201110, d.FID,d.PID,d.FIRST_NAME,d.LAST_NAME,d.BIRTH_DATE,PIROBA=d.PIROBA}))
.Concat(DEVNILEBI_201112s.Select (d => new {Base_Type=2,Periodi=201111, d.FID,d.PID,d.FIRST_NAME,d.LAST_NAME,d.BIRTH_DATE,PIROBA=d.PIROBA}))

.Concat(DEVNILEBI_201201s.Select (d => new {Base_Type=2,Periodi=201112, d.FID,d.PID,d.FIRST_NAME,d.LAST_NAME,d.BIRTH_DATE,PIROBA=d.PIROBA}))
.Concat(DEVNILEBI_201202s.Select (d => new {Base_Type=2,Periodi=201201, d.FID,d.PID,d.FIRST_NAME,d.LAST_NAME,d.BIRTH_DATE,d.PIROBA}))


.Concat(PEDAGOGEBI_201101s.Select (p => new {Base_Type=8,Periodi=201012, FID=null, PID=p.PID,p.FIRST_NAME,p.LAST_NAME,p.BIRTH_DATE,PIROBA=p.PIROBA}))
.Concat(PEDAGOGEBI_201101s.Select (p => new {Base_Type=8,Periodi=201101, FID=null, PID=p.PID,p.FIRST_NAME,p.LAST_NAME,p.BIRTH_DATE,PIROBA=p.PIROBA}))
.Concat(PEDAGOGEBI_201103s.Select (p => new {Base_Type=8,Periodi=201102, FID=null, PID=p.PID,p.FIRST_NAME,p.LAST_NAME,p.BIRTH_DATE,PIROBA=p.PIROBA}))
.Concat(PEDAGOGEBI_201104s.Select (p => new {Base_Type=8,Periodi=201103, FID=null, PID=p.PID,p.FIRST_NAME,p.LAST_NAME,p.BIRTH_DATE,PIROBA=p.PIROBA}))
.Concat(PEDAGOGEBI_201105s.Select (p => new {Base_Type=8,Periodi=201104, FID=null, PID=p.PID,p.FIRST_NAME,p.LAST_NAME,p.BIRTH_DATE,PIROBA=p.PIROBA}))
.Concat(PEDAGOGEBI_201106s.Select (p => new {Base_Type=8,Periodi=201105, FID=null, PID=p.PID,p.FIRST_NAME,p.LAST_NAME,p.BIRTH_DATE,PIROBA=p.PIROBA}))
.Concat(PEDAGOGEBI_201107s.Select (p => new {Base_Type=8,Periodi=201106, FID=null, PID=p.PID,p.FIRST_NAME,p.LAST_NAME,p.BIRTH_DATE,PIROBA=p.PIROBA}))
.Concat(PEDAGOGEBI_201108s.Select (p => new {Base_Type=8,Periodi=201107, FID=null, PID=p.PID,p.FIRST_NAME,p.LAST_NAME,p.BIRTH_DATE,PIROBA=p.PIROBA}))
.Concat(PEDAGOGEBI_201109s.Select (p => new {Base_Type=8,Periodi=201108, FID=null, PID=p.PID,p.FIRST_NAME,p.LAST_NAME,p.BIRTH_DATE,PIROBA=p.PIROBA}))
.Concat(PEDAGOGEBI_201110s.Select (p => new {Base_Type=8,Periodi=201109, FID=null, PID=p.PID,p.FIRST_NAME,p.LAST_NAME,p.BIRTH_DATE,PIROBA=p.PIROBA}))
.Concat(PEDAGOGEBI_201111s.Select (p => new {Base_Type=8,Periodi=201110, FID=null, PID=p.PID,p.FIRST_NAME,p.LAST_NAME,p.BIRTH_DATE,PIROBA=p.PIROBA}))
.Concat(PEDAGOGEBI_201112s.Select (p => new {Base_Type=8,Periodi=201111, FID=null, PID=p.PID,p.FIRST_NAME,p.LAST_NAME,p.BIRTH_DATE,PIROBA=p.PIROBA}))

.Concat(PEDAGOGEBI_201201s.Select (p => new {Base_Type=8,Periodi=201112, FID=null, PID=p.PID,p.FIRST_NAME,p.LAST_NAME,p.BIRTH_DATE,PIROBA=p.PIROBA}))
.Concat(PEDAGOGEBI_201202s.Select (p => new {Base_Type=8,Periodi=201201, FID=null, PID=p.PID,p.FIRST_NAME,p.LAST_NAME,p.BIRTH_DATE,PIROBA=p.PIROBA}))

.Concat(Source_Data.Where (sd => sd.Base_Type.HasValue).Select (sd => new {Base_Type=sd.Base_Type.Value,Periodi=201202, FID=sd.FID, PID=sd.PID,FIRST_NAME=sd.First_Name,LAST_NAME=sd.Last_Name,BIRTH_DATE=sd.Birth_Date,PIROBA=sd.Piroba}))

.Where (d => d.Base_Type==8)
.Take(10)