using System;
using System.Collections.Generic;
using System.Linq;
using DesktopUI2.Extensions;
using Rhino;
using Speckle.Core.Kits;

namespace MyCustomDesktopUIPlugin
{
  public class MyCustomFilter : ExternalSelectionFilter<RhinoDoc, MyCustomFilterView, MyCustomFilterModel, string>
  {
    public MyCustomFilter() : base("My Custom Filter", null, "MCF", "Does some filtering...", "My custom filter description") { }

    public override IEnumerable<string> Filter(RhinoDoc document, ISpeckleConverter converter)
    {
      Console.WriteLine(document.ToString());
      
      return document.Objects.Select(o => o.Id.ToString());
    }
  }
}