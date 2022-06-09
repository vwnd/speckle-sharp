using System;
using System.Windows;
using System.Windows.Controls;
using DesktopUI2.ViewModels;
using DesktopUI2.Views;


// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SpeckleConnectorCSI
{
    public partial class Panel : UserControl
    {
        public Panel()
        {
            InitializeComponent();
              this.DataContext = cPlugin.ViewModel;
            AvaloniaHost.Content = new MainUserControl();
        }

    private void InitializeComponent()
    {
    }
  }
}
