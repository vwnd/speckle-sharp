using System;
using System.Collections.Generic;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Speckle.Core.Kits;

namespace DesktopUI2.Extensions
{
  public abstract class ExternalSelectionFilter<T, U, V, W> :
    IExternalSelectionFilter<T,W>
    where U : ReactiveUserControl<V>
    where V : ReactiveObject
  {
    public ExternalSelectionFilter(string name, string icon, string slug, string summary, string description)
    {
      Name = name;
      Icon = icon;
      Slug = slug;
      Summary = summary;
      Description = description;
    }

    public string Name { get; set; }
    public string Type => GetType().ToString();
    public string Icon { get; set; }
    public string Slug { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public List<string> Selection { get; set; }
    public Type ViewType => typeof(U);

    public abstract IEnumerable<W> Filter(T document, ISpeckleConverter converter);
  }
}