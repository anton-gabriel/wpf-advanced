namespace ArrangeElements
{
  using ArrangeElements.ViewModel;
  using System;
  using System.Runtime.InteropServices;
  using System.Windows;
  using System.Windows.Input;
  using System.Windows.Interop;

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private bool _RestoreIfMove = false;

    public MainWindow(MainViewModel viewModel)
    {
      DataContext = viewModel;
      InitializeComponent();
      Header.PreviewMouseLeftButtonDown += rctHeader_PreviewMouseLeftButtonDown;
      Header.PreviewMouseLeftButtonUp += rctHeader_PreviewMouseLeftButtonUp;
      Header.PreviewMouseMove += rctHeader_PreviewMouseMove;
    }
    protected override void OnSourceInitialized(EventArgs e)
    {
      base.OnSourceInitialized(e);
      IntPtr mWindowHandle = (new WindowInteropHelper(this)).Handle;
      HwndSource.FromHwnd(mWindowHandle).AddHook(new HwndSourceHook(WindowProc));
    }

    protected override void OnClosed(EventArgs e)
    {
      base.OnClosed(e);
      Header.PreviewMouseLeftButtonDown -= rctHeader_PreviewMouseLeftButtonDown;
      Header.PreviewMouseLeftButtonUp -= rctHeader_PreviewMouseLeftButtonUp;
      Header.PreviewMouseMove -= rctHeader_PreviewMouseMove;
    }

    private static System.IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
      switch (msg)
      {
        case 0x0024:
          WmGetMinMaxInfo(hwnd, lParam);
          break;
      }

      return IntPtr.Zero;
    }


    private static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
    {
      POINT lMousePosition;
      GetCursorPos(out lMousePosition);

      IntPtr lCurrentScreen = MonitorFromPoint(lMousePosition, MonitorOptions.MONITOR_DEFAULTTONEAREST);


      MINMAXINFO lMmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

      MONITORINFO lCurrentScreenInfo = new MONITORINFO();
      if (GetMonitorInfo(lCurrentScreen, lCurrentScreenInfo) == false)
      {
        return;
      }

      lMmi.ptMaxPosition.X = lCurrentScreenInfo.rcWork.Left - lCurrentScreenInfo.rcMonitor.Left;
      lMmi.ptMaxPosition.Y = lCurrentScreenInfo.rcWork.Top - lCurrentScreenInfo.rcMonitor.Top;
      lMmi.ptMaxSize.X = lCurrentScreenInfo.rcWork.Right - lCurrentScreenInfo.rcWork.Left;
      lMmi.ptMaxSize.Y = lCurrentScreenInfo.rcWork.Bottom - lCurrentScreenInfo.rcWork.Top;

      Marshal.StructureToPtr(lMmi, lParam, true);
    }


    private void SwitchWindowState()
    {
      switch (WindowState)
      {
        case WindowState.Normal:
          {
            WindowState = WindowState.Maximized;
            break;
          }
        case WindowState.Maximized:
          {
            WindowState = WindowState.Normal;
            break;
          }
      }
    }


    private void rctHeader_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (e.ClickCount == 2)
      {
        if ((ResizeMode == ResizeMode.CanResize) || (ResizeMode == ResizeMode.CanResizeWithGrip))
        {
          SwitchWindowState();
        }

        return;
      }

      else if (WindowState == WindowState.Maximized)
      {
        _RestoreIfMove = true;
        return;
      }

      DragMove();
    }


    private void rctHeader_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      _RestoreIfMove = false;
    }


    private void rctHeader_PreviewMouseMove(object sender, MouseEventArgs e)
    {
      if (_RestoreIfMove)
      {
        _RestoreIfMove = false;

        double percentHorizontal = e.GetPosition(this).X / ActualWidth;
        double targetHorizontal = RestoreBounds.Width * percentHorizontal;

        double percentVertical = e.GetPosition(this).Y / ActualHeight;
        double targetVertical = RestoreBounds.Height * percentVertical;

        WindowState = WindowState.Normal;

        POINT lMousePosition;
        GetCursorPos(out lMousePosition);

        Left = lMousePosition.X - targetHorizontal;
        Top = lMousePosition.Y - targetVertical;

        DragMove();
      }
    }

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetCursorPos(out POINT lpPoint);


    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

    enum MonitorOptions : uint
    {
      MONITOR_DEFAULTTONULL = 0x00000000,
      MONITOR_DEFAULTTOPRIMARY = 0x00000001,
      MONITOR_DEFAULTTONEAREST = 0x00000002
    }


    [DllImport("user32.dll")]
    static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);


    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
      public int X;
      public int Y;

      public POINT(int x, int y)
      {
        this.X = x;
        this.Y = y;
      }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct MINMAXINFO
    {
      public POINT ptReserved;
      public POINT ptMaxSize;
      public POINT ptMaxPosition;
      public POINT ptMinTrackSize;
      public POINT ptMaxTrackSize;
    };


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MONITORINFO
    {
      public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
      public RECT rcMonitor = new RECT();
      public RECT rcWork = new RECT();
      public int dwFlags = 0;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
      public int Left, Top, Right, Bottom;

      public RECT(int left, int top, int right, int bottom)
      {
        this.Left = left;
        this.Top = top;
        this.Right = right;
        this.Bottom = bottom;
      }
    }

    private void ShutDown(object sender, RoutedEventArgs e)
    {
      Close();
    }
  }
}