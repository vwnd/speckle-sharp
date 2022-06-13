using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DesktopUI2.Extensions
{
  public interface IDesktopUIPlugin
  {
    public Guid Guid { get; }
    public string Name { get; }
    public string Description { get; }
    public Version Version { get; }
    public string[] SupportedApplications { get;  }
    public string Author { get; }
    public string ContactEmail { get; }
  }

  public interface IDesktopUIFilterPlugin
  {
    public IEnumerable<IExternalSelectionFilter<T,U>> AvailableFilters<T,U>();
  }

  public abstract class DesktopUIFilterPlugin : IDesktopUIPlugin, IDesktopUIFilterPlugin
  {
    public abstract Guid Guid { get; }
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract Version Version { get; }
    public abstract string[] SupportedApplications { get; }
    public abstract string Author { get; }
    public abstract string ContactEmail { get; }
    
    public IEnumerable<IExternalSelectionFilter<T,U>> AvailableFilters<T,U>()
    {
      var type = typeof(IExternalSelectionFilter<T,U>);
      var types = Assembly.GetAssembly(GetType()).GetTypes()
        .Where(p => type.IsAssignableFrom(p) && p.IsClass);
      var res = types.ToList().Select(t => Activator.CreateInstance(t) as IExternalSelectionFilter<T,U>);
      return res;
    }
  }
}
