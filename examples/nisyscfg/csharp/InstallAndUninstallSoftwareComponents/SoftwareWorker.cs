using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using NationalInstruments.SystemConfiguration;

namespace NationalInstruments.Examples.InstallAndUninstallSoftwareComponents
{
    class SoftwareWorker : INotifyPropertyChanged
    {
        private bool canBeginShowSoftware;
        private SoftwareComponentCollection allInstalledSoftware;
        private SoftwareComponentCollection allAvailableSoftware;
        private SystemConfiguration.SystemConfiguration session;

        public SoftwareWorker()
        {
            CanBeginShowSoftware = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanBeginShowSoftware
        {
            get { return canBeginShowSoftware; }
            set
            {
                if (canBeginShowSoftware != value)
                {
                    canBeginShowSoftware = value;
                    NotifyPropertyChanged("CanBeginShowSoftware");
                }
            }
        }

        public string Target
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        public IEnumerable<SoftwareComponent> InstalledComponentInfo
        {
            get
            {
                if (AllInstalledSoftware == null)
                {
                    return Enumerable.Empty<SoftwareComponent>();
                }
                return AllInstalledSoftware;
            }
        }

        public IEnumerable<SoftwareComponent> AvailableComponentInfo
        {
            get
            {
                if (AllAvailableSoftware == null)
                {
                    return Enumerable.Empty<SoftwareComponent>();
                }
                return AllAvailableSoftware;
            }
        }

        private SoftwareComponentCollection AllInstalledSoftware
        {
            get { return allInstalledSoftware; }
            set
            {
                if (allInstalledSoftware != value)
                {
                    allInstalledSoftware = value;
                    NotifyPropertyChanged("InstalledComponentInfo");
                }
            }
        }

        private SoftwareComponentCollection AllAvailableSoftware
        {
            get { return allAvailableSoftware; }
            set
            {
                if (allAvailableSoftware != value)
                {
                    allAvailableSoftware = value;
                    NotifyPropertyChanged("AvailableComponentInfo");
                }
            }
        }

        public void StartShowSoftware(string password)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(
                delegate(object o, DoWorkEventArgs args)
                {
                    CanBeginShowSoftware = false;
                    try
                    {
                        AllInstalledSoftware = null;
                        AllAvailableSoftware = null;
                        session = new SystemConfiguration.SystemConfiguration(Target, Username, password);
                        AllInstalledSoftware = session.GetInstalledSoftwareComponents();
                        AllAvailableSoftware = session.GetAvailableSoftwareComponents(ComponentTypeFilter.AllVisible);
                    }
                    catch (SystemConfigurationException ex)
                    {
                        string errorMessage = string.Format("Get Software Components threw a System Configuration Exception.\n\nError Code: {0:X}\n{1}", ex.ErrorCode, ex.Message);
                        MessageBox.Show(errorMessage, "System Configuration Exception");
                    }
                    finally
                    {
                        CanBeginShowSoftware = true;
                    }
                }
            );
            worker.RunWorkerAsync();
        }

        public void StartInstalling(SoftwareComponentCollection softwareToBeInstalled)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(
                delegate(object o, DoWorkEventArgs args)
                {
                    CanBeginShowSoftware = false;
                    try
                    {
                        session.InstallUninstallComponents(softwareToBeInstalled, null, true, false);
                        AllInstalledSoftware = session.GetInstalledSoftwareComponents();
                    }
                    catch (BrokenDependenciesException brex)
                    {
                        string errorMessage = string.Format("The installation failed because one or more dependencies were missing.\n\nError Code: {0:X}\n{1}", brex.ErrorCode, brex.Message);
                        foreach (Dependency brokenDp in brex.BrokenDependencies)
                        {
                            errorMessage += string.Format("\n{0} depends on {1}", brokenDp.Depender.Title, brokenDp.Dependee.Title);
                        }
                        MessageBox.Show(errorMessage, "Installation Failed");
                    }
                    catch (SystemConfigurationException ex)
                    {
                        string errorMessage = string.Format("Install Components threw a System Configuration Exception.\n\nError Code: {0:X}\n{1}", ex.ErrorCode, ex.Message);
                        MessageBox.Show(errorMessage, "System Configuration Exception");
                    }
                    finally
                    {
                        CanBeginShowSoftware = true;
                    }
                }
            );
            worker.RunWorkerAsync();
        }

        public void StartUninstalling(string[] softwareToBeUninstalled)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(
                delegate(object o, DoWorkEventArgs args)
                {
                    CanBeginShowSoftware = false;
                    try
                    {
                        session.InstallUninstallComponents(null, softwareToBeUninstalled);
                        AllInstalledSoftware = session.GetInstalledSoftwareComponents();
                    }
                    catch (BrokenDependenciesException brex)
                    {
                        string errorMessage = string.Format("The uninstall failed because installed software depends on the software being uninstalled.\n\nError Code: {0:X}\n{1}", brex.ErrorCode, brex.Message);
                        foreach (Dependency brokenDp in brex.BrokenDependencies)
                        {
                            errorMessage += string.Format("\n{0} depends on {1}", brokenDp.Depender.Title, brokenDp.Dependee.Title);
                        }
                        MessageBox.Show(errorMessage, "Installation Failed");
                    }
                    catch (SystemConfigurationException ex)
                    {
                        string errorMessage = string.Format("Uninstall Components threw a System Configuration Exception.\n\nError Code: {0:X}\n{1}", ex.ErrorCode, ex.Message);
                        MessageBox.Show(errorMessage, "System Configuration Exception");
                    }
                    finally
                    {
                        CanBeginShowSoftware = true;
                    }
                }
            );
            worker.RunWorkerAsync();
        }

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}