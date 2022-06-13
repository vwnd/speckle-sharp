using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DesktopUI2.Extensions;
using Speckle.Core.Kits;

namespace MyCustomDesktopUIPlugin
{
  public class MyCustomSpecklePlugin : DesktopUIFilterPlugin
  {
    public override Guid Guid => new Guid("49B71C19-FE2C-46F3-9CF1-F5A32A8D56F8");

    public override string Name => "My DesktopUI Plugin";

    public override string Description => "A plugin to customize how desktopUI behaves!";

    public override Version Version => new Version(GetType().Assembly.ImageRuntimeVersion);

    public override string[] SupportedApplications => new[] { VersionedHostApplications.Rhino6, VersionedHostApplications.Rhino7 };

    public override string Author => "Alan Rynne";
    
    public override string ContactEmail => "alan@rynne.es";
  }
}