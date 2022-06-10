using System;
using System.Windows;
using System.Windows.Controls;
using DesktopUI2.ViewModels;
using DesktopUI2.Views;

namespace Speckle.ConnectorTeklaStructures.UI
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : UserControl
  {
    public MainWindow()
    {
      InitializeComponent();

      AvaloniaHost.Content = new MainUserControl();
    }
  }
}
