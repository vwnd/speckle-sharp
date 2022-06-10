using System;
using System.Windows;
using System.Windows.Controls;
using DesktopUI2.Views;

namespace Speckle.ConnectorCSI.UI
{ 
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      AvaloniaHost.Content = new MainUserControl();
    }
  }
}
