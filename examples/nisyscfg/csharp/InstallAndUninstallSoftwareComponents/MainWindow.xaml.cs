using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using NationalInstruments.SystemConfiguration;

namespace NationalInstruments.Examples.InstallAndUninstallSoftwareComponents
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            worker = new SoftwareWorker();
            mainGrid.DataContext = worker;
            installationGrid.Visibility = Visibility.Collapsed;
        }

        private SoftwareWorker worker;

        private void OnShowSoftwareClick(object sender, RoutedEventArgs e)
        {
            targetGrid.Visibility = Visibility.Collapsed;
            installationGrid.Visibility = Visibility.Visible;
            worker.StartShowSoftware(PasswordBox.Password);
        }

        private void OnUninstallClick(object sender, RoutedEventArgs e)
        {
            List<string> SoftwareToBeUninstalled = new List<string>();
            foreach (SoftwareComponent component in InstalledSoftwareGrid.SelectedItems)
            {
                SoftwareToBeUninstalled.Add(component.Id.ToString());
            }
            worker.StartUninstalling(SoftwareToBeUninstalled.ToArray());
        }

        private void OnInstallClick(object sender, RoutedEventArgs e)
        {
            SoftwareComponentCollection SoftwareToBeInstalled = new SoftwareComponentCollection();
            foreach (SoftwareComponent component in AvailableSoftwareGrid.SelectedItems)
            {
                SoftwareToBeInstalled.Add(component.Id.ToString());
            }
            worker.StartInstalling(SoftwareToBeInstalled);
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            targetGrid.Visibility = Visibility.Visible;
            installationGrid.Visibility = Visibility.Collapsed;
        }
    }
}