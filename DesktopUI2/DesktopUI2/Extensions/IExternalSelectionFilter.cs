using System.Collections;
using System.Collections.Generic;
using DesktopUI2.Models.Filters;
using Speckle.Core.Kits;

namespace DesktopUI2.Extensions
{
  public interface IExternalSelectionFilter<T, U> :
    ISelectionFilter
  {
    public IEnumerable<U> Filter(T document, ISpeckleConverter converter);
  }
}