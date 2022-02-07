using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using Rhino;
using Rhino.Commands;
using Rhino.PlugIns;

using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;

using DesktopUI2.ViewModels;
using DesktopUI2.Views;

namespace SpeckleRhino
{
  public class SpeckleCommand2 : Command
  {
    #region Avalonia parent window
    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr value);
    const int GWL_HWNDPARENT = -8;
    #endregion

    public static SpeckleCommand2 Instance { get; private set; }

    public override string EnglishName => "Speckle2Mac";

    public static Window MainWindow { get; private set; }

    public static RhinoViewModel Bindings { get; set; } = new RhinoViewModel();

    public SpeckleCommand2()
    {
      Instance = this;
    }

    public static AppBuilder BuildAvaloniaApp()
    {
      //string path = Path.GetDirectoryName(typeof(App).Assembly.Location);
      string nativeLib = "/Users/alan/.nuget/packages/avalonia.native/0.10.999-cibuild0017846-beta/runtimes/osx/native/libAvaloniaNative.dylib";
      //Path.Combine(path, "Native", "libAvalonia.Native.OSX.dylib");
      return AppBuilder.Configure<DesktopUI2.App>()
      .UsePlatformDetect()
      .With(new X11PlatformOptions { UseGpu = false })
      .With(new MacOSPlatformOptions { ShowInDock = false, DisableDefaultApplicationMenuItems = true, DisableNativeMenus = true })
      .With(new AvaloniaNativePlatformOptions
      {
        AvaloniaNativeLibraryPath = nativeLib
      })
      .With(new SkiaOptions { MaxGpuResourceSizeBytes = 8096000 })
      .With(new Win32PlatformOptions { AllowEglInitialization = true, EnableMultitouch = false })
      .LogToTrace()
      .UseReactiveUI();
    }

    protected override Result RunCommand(RhinoDoc doc, RunMode mode)
    {
      CreateOrFocusSpeckle();
      return Result.Success;
    }

    public static void CreateOrFocusSpeckle()
    {
      if (MainWindow == null)
      {
        try
        {
          BuildAvaloniaApp().Start(AppMain, null);
        }
        catch (Exception e)
        {
          Console.Write(e);
        }
      }

      MainWindow.Show();
      MainWindow.Activate();

      // TODO: NOT SURE HOW TO FIX THIS!!!
      //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      //{
      //  var parentHwnd = RhinoApp.MainWindowHandle();
      //  var hwnd = MainWindow.PlatformImpl.Handle.Handle;
      //  SetWindowLongPtr(hwnd, GWL_HWNDPARENT, parentHwnd);
      //}
    }

    private static void AppMain(Application app, string[] args)
    {
      MainWindow = new MainWindow
      {
        DataContext = Bindings,
      };
      Task.Run(() => app.Run(MainWindow));
    }
  }
}
