using System.Windows;
using System.Windows.Controls;

namespace NationalInstruments.Examples.RenameAliases
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            worker = new RenameAliasWorker();
            mainGrid.DataContext = worker;
        }

        private RenameAliasWorker worker;

        private void OnGetDevicesClick(object sender, RoutedEventArgs e)
        {
            worker.GetDevices(passwordBox.Password);
        }

        private void OnSubmitChangesClick(object sender, RoutedEventArgs e)
        {
            worker.SubmitChanges();
            worker.GetDevices(passwordBox.Password);
        }
    }
}
