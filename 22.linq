<Query Kind="Program" />

void Main()
{   
	NetworkDrives.MapDrive("y", @"\\db\i$", @"MOLHSA\otakhi105", "ot@khi105");
	System.IO.File.WriteAllText(@"C:\temp\temp.txt", System.IO.File.ReadAllLines(@"\\db\i$\New Text Document.txt")[0]);
}


