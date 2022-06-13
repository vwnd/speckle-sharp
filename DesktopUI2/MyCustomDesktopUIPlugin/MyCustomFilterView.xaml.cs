using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace MyCustomDesktopUIPlugin
{
  public class MyCustomFilterView : ReactiveUserControl<MyCustomFilterModel>
  {
    public MyCustomFilterView()
    {
      InitializeComponent();
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }
  }
}