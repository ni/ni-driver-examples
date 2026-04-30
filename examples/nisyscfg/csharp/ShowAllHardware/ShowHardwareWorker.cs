using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using NationalInstruments.SystemConfiguration;

namespace NationalInstruments.Examples.ShowAllHardware
{
    class ShowHardwareWorker : INotifyPropertyChanged
    {
        private bool canBeginShowHardware;
        private bool shouldShowNetworkDevices;
        private List<HardwareViewModel> allHardwareResources;

        public ShowHardwareWorker()
        {
            CanBeginShowHardware = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanBeginShowHardware
        {
            get { return canBeginShowHardware; }
            set
            {
                if (canBeginShowHardware != value)
                {
                    canBeginShowHardware = value;
                    NotifyPropertyChanged("CanBeginShowHardware");
                }
            }
        }

        public bool ShouldShowNetworkDevices
        {
            get { return shouldShowNetworkDevices; }
            set
            {
                if (shouldShowNetworkDevices != value)
                {
                    shouldShowNetworkDevices = value;
                    NotifyPropertyChanged("FilteredHardwareResources");
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

        public IEnumerable<HardwareViewModel> FilteredHardwareResources
        {
            get
            {
                if (AllHardwareResources == null)
                {
                    return Enumerable.Empty<HardwareViewModel>();
                }
                return from resource in AllHardwareResources
                       where ShouldShowNetworkDevices || !(resource.NumberOfExperts == 1 && resource.Expert0ProgrammaticName.Equals("network"))
                       select resource;
            }
        }

        private List<HardwareViewModel> AllHardwareResources
        {
            get { return allHardwareResources; }
            set
            {
                if (allHardwareResources != value)
                {
                    allHardwareResources = value;
                    NotifyPropertyChanged("FilteredHardwareResources");
                }
            }
        }

        public void StartShowHardware(string password)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(
                delegate(object o, DoWorkEventArgs args)
                {
                    CanBeginShowHardware = false;
                    try
                    {
                        // Because the view does not allow modifying resources, there isn't a need to keep
                        // the raw HardwareResourceBase objects after creating the view models.
                        AllHardwareResources = null;
                        var session = new SystemConfiguration.SystemConfiguration(Target, Username, password);
                        ResourceCollection rawResources = session.FindHardware();
                        AllHardwareResources =
                            (from resource in rawResources
                             select new HardwareViewModel(resource)).ToList();
                    }
                    catch (SystemConfigurationException ex)
                    {
                        string errorMessage = string.Format("Find Hardware threw a System Configuration Exception.\n\nErrorCode: {0:X}\n{1}", ex.ErrorCode, ex.Message);
                        MessageBox.Show(errorMessage, "System Configuration Exception");
                    }
                    finally
                    {
                        CanBeginShowHardware = true;
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
