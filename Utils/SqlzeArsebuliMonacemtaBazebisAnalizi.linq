<Query Kind="Statements">
  <Connection>
    <ID>453dabbc-e8b3-4c04-8eec-536e8e4e7b58</ID>
    <Persist>true</Persist>
    <Server>triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <NoPluralization>true</NoPluralization>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA3XCANzX8A0+HtkxmZtXFGAAAAAACAAAAAAAQZgAAAAEAACAAAACp+sEJqJxOPz2BQ1lxRRtXH/XJirk9/kw8mJj0bPf5jQAAAAAOgAAAAAIAACAAAACZd/POK3nxMSCEaCgbVDIUSs/pSsra35l5MYJ8LGMjExAAAABDe6Hrhl+CP58Aq2DzWMGNQAAAAA6bC1RC4u17G4KQF35FDnGlInhYWmmeVWDNOqKPGjJdSaTe9PMIuov4DO+z/r3whP06rpngcdEwF0GcF80cZT0=</Password>
    <Database>master</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>C:\Temp\EPPlus.dll</Reference>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

var dbs = 	(
			from divfs in dm_io_virtual_file_stats(null,null)
			from mf in Master_files
			from db in Databases
			where mf.Database_id == divfs.Database_id && mf.File_id == divfs.File_id && db.Database_id == mf.Database_id
			select new { db.Name, mf.Physical_name, Disk=System.IO.Path.GetPathRoot(mf.Physical_name).ToUpper(), Path=System.IO.Path.GetDirectoryName(mf.Physical_name).ToUpper(), divfs.Size_on_disk_bytes }
			).ToList();
dbs.OrderBy (x => x.Name).Dump();


//dm_io_virtual_file_stats(null,null).SelectMany (divfs => Master_files.Where (mf =>mf.Database_id == divfs.Database_id && mf.File_id == divfs.File_id) )