<Query Kind="Program">
  <NuGetReference>System.Net.FtpClient</NuGetReference>
  <Namespace>System.Net.FtpClient</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

public IEnumerable<string> GetFileList(string ftpServerIP, string ftpUserID, string ftpPassword)
{
	using (FtpClient conn = new FtpClient()) 
	{
		conn.Host = ftpServerIP;
		conn.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
		return conn.GetListing().Select (c => c.FullName).ToArray();
	}

}
public static void DownLoad(string ftpServerIP, string ftpUserID, string ftpPassword, string fileName, string toFile) 
{
	using(FtpClient conn = new FtpClient()) 
	{
		conn.Host = ftpServerIP;
		conn.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
		
		using(Stream istream = conn.OpenRead(fileName)) 
		using(Stream to = new FileStream(toFile,FileMode.Create))
		{
			try 
			{
				CopyStream(istream, to);
			}
			finally {
				istream.Close();
				to.Flush();
				to.Close();
			}
		}
	}
}
public static void CopyStream(Stream input, Stream output)
{
    byte[] buffer = new byte[8 * 1024];
    int len;
    while ( (len = input.Read(buffer, 0, buffer.Length)) > 0)
    {
        output.Write(buffer, 0, len);
    }    
}
void Main()
{
//var p = Process.Start(@"C:\Program Files\WinRAR\Rar.exe",@"t c:\temp\aa.rar");
//p.WaitForExit();
//p.ExitCode.Dump();
GetFileList("81.95.168.42","moh","(Jan-Dacva)").Dump();
	
}

// Define other methods and classes here