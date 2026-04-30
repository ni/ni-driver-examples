using System.Windows;
using System.Windows.Controls;

namespace NationalInstruments.Examples.GetAndSetImage
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            worker = new GetAndSetImageWorker();
            mainGrid.DataContext = worker;
        }

        private GetAndSetImageWorker worker;

        private void OnGetSetImageClick(object sender, RoutedEventArgs e)
        {
            if (getImageButton.IsChecked == true)
            {
                worker.StartGetImage(passwordBox.Password);
            }
            else
            {
                worker.StartSetImage(passwordBox.Password);
            }
        }
    }
}
