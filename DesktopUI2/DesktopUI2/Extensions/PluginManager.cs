using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Sentry;
using Speckle.Core.Logging;
using Speckle.Core.Models;

namespace DesktopUI2.Extensions
{
  public static class PluginManager
  {
    private static string _pluginsFolder;

    /// <summary>
    /// Local installations store plugins in C:\Users\USERNAME\AppData\Roaming\Speckle\Plugins
    /// Admin/System-wide installations in C:\ProgramData\Speckle\Plugins
    /// </summary>
    public static string PluginsFolder
    {
      get
      {
        if (_pluginsFolder != null)
          return _pluginsFolder;

        var appDir = Assembly.GetAssembly(typeof(PluginManager)).Location.Contains("ProgramData")
          ? Environment.SpecialFolder.CommonApplicationData
          : Environment.SpecialFolder.ApplicationData;

        return Path.Combine(Environment.GetFolderPath(appDir), "Speckle", "Plugins");
      }
      set => _pluginsFolder = value;
    }

    public static readonly AssemblyName SpeckleAssemblyName = typeof(Base).GetTypeInfo().Assembly.GetName();

    private static Dictionary<Guid, IDesktopUIPlugin> _SpecklePlugins = new Dictionary<Guid, IDesktopUIPlugin>();

    private static bool _initialized;

    /// <summary>
    /// Checks whether a specific plugin exists.
    /// </summary>
    /// <param name="pluginGuid"></param>
    /// <returns></returns>
    public static bool HasPlugin(Guid pluginGuid)
    {
      Initialize();
      return _SpecklePlugins.ContainsKey(pluginGuid);
    }

    /// <summary>
    /// Gets a specific plugin.
    /// </summary>
    /// <param name="pluginGuid"></param>
    /// <returns></returns>
    public static IDesktopUIPlugin GetPlugin(Guid pluginGuid)
    {
      Initialize();
      return _SpecklePlugins[pluginGuid];
    }

    /// <summary>
    /// Returns a list of all the plugins found on this user's device.
    /// </summary>
    public static IEnumerable<IDesktopUIPlugin> Plugins
    {
      get
      {
        Initialize();
        return _SpecklePlugins.Values.Where(v => v != null);
      }
    }

    /// <summary>
    /// Tells the plugin manager to initialise from a specific location.
    /// </summary>
    /// <param name="pluginFolderLocation"></param>
    public static void Initialize(string pluginFolderLocation)
    {
      if (_initialized)
        throw new SpeckleException(
          "The plugin manager has already been initialised. Make sure you call this method earlier in your code!",
          level: SentryLevel.Warning);

      PluginsFolder = pluginFolderLocation;
      Load();
      _initialized = true;
    }

    #region Private Methods

    private static void Initialize()
    {
      if (_initialized) return;
      Load();
      _initialized = true;
    }

    private static void Load()
    {
      Log.AddBreadcrumb("Initialize DUI Plugin Manager");
      LoadSpeckleReferencingAssemblies();
    }

    private static void LoadSpeckleReferencingAssemblies()
    {
      if (!Directory.Exists(PluginsFolder))
        return;

      foreach (var directory in Directory.GetDirectories(PluginsFolder))
      foreach (var assemblyPath in Directory.EnumerateFiles(directory, "*.spckl"))
      {
        var unloadedAssemblyName = SafeGetAssemblyName(assemblyPath);
        if (unloadedAssemblyName == null)
          continue;

        var assembly = Assembly.LoadFrom(assemblyPath);
        var pluginClass = GetPluginClass(assembly);

        if (!assembly.IsReferencing(SpeckleAssemblyName) || pluginClass == null)
          continue;

        if (Activator.CreateInstance(pluginClass) is IDesktopUIPlugin specklePlugin)
          _SpecklePlugins.Add(specklePlugin.Guid, specklePlugin);
      }
    }

    private static Type GetPluginClass(Assembly assembly)
    {
      try
      {
        return assembly.GetTypes().FirstOrDefault(type =>
        {
          return type
            .GetInterfaces()
            .FirstOrDefault(iFace => iFace.Name == nameof(IDesktopUIPlugin)) != null;
        });
      }
      catch
      {
        // this will be a ReflectionTypeLoadException and is expected. we don't need to care!
        return null;
      }
    }

    private static Assembly SafeLoadAssembly(AppDomain domain, AssemblyName assemblyName)
    {
      try
      {
        return domain.Load(assemblyName);
      }
      catch
      {
        return null;
      }
    }

    private static AssemblyName SafeGetAssemblyName(string assemblyPath)
    {
      try
      {
        return AssemblyName.GetAssemblyName(assemblyPath);
      }
      catch
      {
        return null;
      }
    }

    #endregion
  }

  public static class AssemblyExtensions
  {
    /// <summary>
    /// Indicates if a given assembly references another which is identified by its name.
    /// </summary>
    /// <param name="assembly">The assembly which will be probed.</param>
    /// <param name="referenceName">The reference assembly name.</param>
    /// <returns>A boolean value indicating if there is a reference.</returns>
    public static bool IsReferencing(this Assembly assembly, AssemblyName referenceName)
    {
      return AssemblyName.ReferenceMatchesDefinition(assembly.GetName(), referenceName)
             || assembly
               .GetReferencedAssemblies()
               .Any(referencedAssemblyName =>
                 AssemblyName.ReferenceMatchesDefinition(referencedAssemblyName, referenceName));
    }
  }
}