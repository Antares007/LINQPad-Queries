<Query Kind="Program">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAAAD89x4SL38S/4r7NUU2iHNNTcmnVTi1xQPx1AC1vAtFAAAAAAOgAAAAAIAACAAAABgO+xVBio0IKzceIXWbWfgFv0jcxQpOA9YilhDtPA8XxAAAABJcM5+MLInsGd5jUUGfXtnQAAAALHy7sVof5cKLhfpSHxbLdPESALwKOWLElOgeYcZmaeqO1sDG9SHnPN5xOzSlBHlSD7agk+KC9dH/4XMZn4gYvM=</Password>
    <Database>SocialuriDazgveva</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Namespace>System</Namespace>
  <Namespace>System.Collections</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
  <Namespace>System.Text</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

void Main()
{
	foreach(var w in new Windows().Cast<Window>())
	{
		ScreenCapture sc = new ScreenCapture();
		var b= sc.CaptureWindow(w.hWnd);
		b.Dump(w.Title);
	}
}


/// <summary>
/// Object used to control a Windows Form.
/// </summary>
public class Window
{
   /// <summary>
   /// Win32 API Imports
   /// </summary>
   [DllImport("user32.dll")]
   private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
   [DllImport("user32.dll")]
   private static extern bool SetForegroundWindow(IntPtr hWnd);
   [DllImport("user32.dll")]
   private static extern bool IsIconic(IntPtr hWnd);
   [DllImport("user32.dll")] 
   private static extern bool IsZoomed(IntPtr hWnd);
   [DllImport("user32.dll")] 
   private static extern IntPtr GetForegroundWindow();
   [DllImport("user32.dll")]
   private static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
   [DllImport("user32.dll")]
   private static extern IntPtr AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, int fAttach);

   /// <summary>
   /// Win32 API Constants for ShowWindowAsync()
   /// </summary>
   private const int SW_HIDE = 0;
   private const int SW_SHOWNORMAL = 1;
   private const int SW_SHOWMINIMIZED = 2;
   private const int SW_SHOWMAXIMIZED = 3;
   private const int SW_SHOWNOACTIVATE = 4;
   private const int SW_RESTORE = 9;
   private const int SW_SHOWDEFAULT = 10;

   /// <summary>
   /// Private Fields
   /// </summary>
   private IntPtr m_hWnd;
   private string m_Title;
   private bool m_Visible = true;
   private string m_Process;
   private bool m_WasMax = false;

   /// <summary>
   /// Window Object's Public Properties
   /// </summary>
   public IntPtr hWnd
   {
       get{ return m_hWnd;}
   }
   public string Title
   {
       get{ return m_Title;}
   }
   public string Process
   {
       get{ return m_Process;}
   }

   /// <summary>
   /// Sets this Window Object's visibility
   /// </summary>
   public bool Visible
   {
       get{ return m_Visible;}
       set
       {
           //show the window
           if(value == true)
           {
               if(m_WasMax)
               {
                   if(ShowWindowAsync(m_hWnd,SW_SHOWMAXIMIZED))
                       m_Visible = true;
               }
               else
               {
                   if(ShowWindowAsync(m_hWnd,SW_SHOWNORMAL))
                       m_Visible = true;
               }
           }
           //hide the window
           if(value == false)
           {
               m_WasMax = IsZoomed(m_hWnd);
               if(ShowWindowAsync(m_hWnd,SW_HIDE))
                   m_Visible = false;
           }
       }
   }

   /// <summary>
   /// Constructs a Window Object
   /// </summary>
   /// <param name="Title">Title Caption</param>
   /// <param name="hWnd">Handle</param>
   /// <param name="Process">Owning Process</param>
   public Window(string Title, IntPtr hWnd, string Process)
   {
       m_Title = Title;
       m_hWnd = hWnd;
       m_Process = Process;
   }

   //Override ToString() 
   public override string ToString()
   {
       //return the title if it has one, if not return the process name
       if (m_Title.Length > 0)
       {
           return m_Title;
       }
       else
       {
           return m_Process;
       }
   }

   /// <summary>
   /// Sets focus to this Window Object
   /// </summary>
   public void Activate()
   {
       if(m_hWnd == GetForegroundWindow())
           return;

       IntPtr ThreadID1 = GetWindowThreadProcessId(GetForegroundWindow(),
                                                   IntPtr.Zero);
       IntPtr ThreadID2 = GetWindowThreadProcessId(m_hWnd,IntPtr.Zero);
       
       if (ThreadID1 != ThreadID2)
       {
           AttachThreadInput(ThreadID1,ThreadID2,1);
           SetForegroundWindow(m_hWnd);
           AttachThreadInput(ThreadID1,ThreadID2,0);
       }
       else
       {
           SetForegroundWindow(m_hWnd);
       }

       if (IsIconic(m_hWnd))
       {
           ShowWindowAsync(m_hWnd,SW_RESTORE);
       }
       else
       {
           ShowWindowAsync(m_hWnd,SW_SHOWNORMAL);
       }
   }
}

/// <summary>
/// Collection used to enumerate Window Objects
/// </summary>
public class Windows : IEnumerable, IEnumerator
{
   /// <summary>
   /// Win32 API Imports
   /// </summary>
   [DllImport("user32.dll")] private static extern 
         int GetWindowText(int hWnd, StringBuilder title, int size);
   [DllImport("user32.dll")] private static extern 
         int GetWindowModuleFileName(int hWnd, StringBuilder title, int size);
   [DllImport("user32.dll")] private static extern 
         int EnumWindows(EnumWindowsProc ewp, int lParam); 
   [DllImport("user32.dll")] private static extern 
         bool IsWindowVisible(int hWnd);

   //delegate used for EnumWindows() callback function
   public delegate bool EnumWindowsProc(int hWnd, int lParam);

   private int m_Position = -1; // holds current index of wndArray, 
                                // necessary for IEnumerable
   
   ArrayList wndArray = new ArrayList(); //array of windows
   
   //Object's private fields
   private bool m_invisible = false;
   private bool m_notitle = false;

   /// <summary>
   /// Collection Constructor with additional options
   /// </summary>
   /// <param name="Invisible">Include invisible Windows</param>
   /// <param name="Untitled">Include untitled Windows</param>
   public Windows(bool Invisible, bool Untitled)
   {
       m_invisible = Invisible;
       m_notitle = Untitled;

       //Declare a callback delegate for EnumWindows() API call
       EnumWindowsProc ewp = new EnumWindowsProc(EvalWindow);
       //Enumerate all Windows
       EnumWindows(ewp, 0);
   }
   /// <summary>
   /// Collection Constructor
   /// </summary>
   public Windows()
   {
       //Declare a callback delegate for EnumWindows() API call
       EnumWindowsProc ewp = new EnumWindowsProc(EvalWindow);
       //Enumerate all Windows
       EnumWindows(ewp, 0);
   }
   //EnumWindows CALLBACK function
   private bool EvalWindow(int hWnd, int lParam)
   {
       if (m_invisible == false && !IsWindowVisible(hWnd))
           return(true);

       StringBuilder title = new StringBuilder(256);
       StringBuilder module = new StringBuilder(256);

       GetWindowModuleFileName(hWnd, module, 256);
       GetWindowText(hWnd, title, 256);

       if (m_notitle == false && title.Length == 0)
           return(true);

       wndArray.Add(new Window(title.ToString(), (IntPtr)hWnd, 
                               module.ToString()));

       return(true);
   }
   
   //implement IEnumerable
   public IEnumerator GetEnumerator()
   {
       return (IEnumerator)this;
   }
   //implement IEnumerator
   public bool MoveNext()
   {
       m_Position++;
       if (m_Position < wndArray.Count)
       {
           return true;
       }
       else
       {
           return false;
       }
   }
   public void Reset()
   {
       m_Position = -1;
   }
   public object Current
   {
       get
       {
           return wndArray[m_Position];
       }
   }
}
/// <summary>
/// Provides functions to capture the entire screen, or a particular window, and save it to a file.
/// </summary>
public class ScreenCapture
{
   /// <summary>
   /// Creates an Image object containing a screen shot of the entire desktop
   /// </summary>
   /// <returns></returns>
   public Image CaptureScreen() 
   {
       return CaptureWindow( User32.GetDesktopWindow() );
   }
   /// <summary>
   /// Creates an Image object containing a screen shot of a specific window
   /// </summary>
   /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
   /// <returns></returns>
   public Image CaptureWindow(IntPtr handle)
   {
       // get te hDC of the target window
       IntPtr hdcSrc = User32.GetWindowDC(handle);
       // get the size
       User32.RECT windowRect = new User32.RECT();
       User32.GetWindowRect(handle,ref windowRect);
       int width = windowRect.right - windowRect.left;
       int height = windowRect.bottom - windowRect.top;
       // create a device context we can copy to
       IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
       // create a bitmap we can copy it to,
       // using GetDeviceCaps to get the width/height
       IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc,width,height); 
       // select the bitmap object
       IntPtr hOld = GDI32.SelectObject(hdcDest,hBitmap);
       // bitblt over
       GDI32.BitBlt(hdcDest,0,0,width,height,hdcSrc,0,0,GDI32.SRCCOPY);
       // restore selection
       GDI32.SelectObject(hdcDest,hOld);
       // clean up 
       GDI32.DeleteDC(hdcDest);
       User32.ReleaseDC(handle,hdcSrc);
       // get a .NET image object for it
       Image img = Image.FromHbitmap(hBitmap);
       // free up the Bitmap object
       GDI32.DeleteObject(hBitmap);
       return img;
   }
   /// <summary>
   /// Captures a screen shot of a specific window, and saves it to a file
   /// </summary>
   /// <param name="handle"></param>
   /// <param name="filename"></param>
   /// <param name="format"></param>
   public void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format) 
   {
       Image img = CaptureWindow(handle);
       img.Save(filename,format);
   }
   /// <summary>
   /// Captures a screen shot of the entire desktop, and saves it to a file
   /// </summary>
   /// <param name="filename"></param>
   /// <param name="format"></param>
   public void CaptureScreenToFile(string filename, ImageFormat format) 
   {
       Image img = CaptureScreen();
       img.Save(filename,format);
   }
 
   /// <summary>
   /// Helper class containing Gdi32 API functions
   /// </summary>
   private class GDI32
   {
       
       public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
       [DllImport("gdi32.dll")]
       public static extern bool BitBlt(IntPtr hObject,int nXDest,int nYDest,
           int nWidth,int nHeight,IntPtr hObjectSource,
           int nXSrc,int nYSrc,int dwRop);
       [DllImport("gdi32.dll")]
       public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC,int nWidth, 
           int nHeight);
       [DllImport("gdi32.dll")]
       public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
       [DllImport("gdi32.dll")]
       public static extern bool DeleteDC(IntPtr hDC);
       [DllImport("gdi32.dll")]
       public static extern bool DeleteObject(IntPtr hObject);
       [DllImport("gdi32.dll")]
       public static extern IntPtr SelectObject(IntPtr hDC,IntPtr hObject);
   }

   /// <summary>
   /// Helper class containing User32 API functions
   /// </summary>
   private class User32
   {
       [StructLayout(LayoutKind.Sequential)]
       public struct RECT
       {
           public int left;
           public int top;
           public int right;
           public int bottom;
       }
       [DllImport("user32.dll")]
       public static extern IntPtr GetDesktopWindow();
       [DllImport("user32.dll")]
       public static extern IntPtr GetWindowDC(IntPtr hWnd);
       [DllImport("user32.dll")]
       public static extern IntPtr ReleaseDC(IntPtr hWnd,IntPtr hDC);
       [DllImport("user32.dll")]
       public static extern IntPtr GetWindowRect(IntPtr hWnd,ref RECT rect);
   }
}