using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using NationalInstruments.SystemConfiguration;

namespace NationalInstruments.Examples.BoardTemperatureMonitor
{
    /// <summary>
    /// Uses the StartTemperatureMonitor method to check for all present, non-simulated NI devices in the system using the HardwareViewModel.
    /// Updates Temperature values for each device using a background worker.
    /// </summary>
    internal class BoardTemperatureMonitorWorker : INotifyPropertyChanged
    {
        private bool canStartMonitor;
        private bool canClickStop;
        private List<HardwareViewModel> allHardwareResources;
        private string devicesAboveLimit;

        public BoardTemperatureMonitorWorker()
        {
            CanStartMonitor = true;
            CanClickStop = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanStartMonitor
        {
            get { return canStartMonitor; }
            set
            {
                if (canStartMonitor != value)
                {
                    canStartMonitor = value;
                    NotifyPropertyChanged("CanStartMonitor");
                }
            }
        }

        public bool CanClickStop
        {
            get { return canClickStop; }
            set
            {
                if (canClickStop != value)
                {
                    canClickStop = value;
                    NotifyPropertyChanged("CanClickStop");
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

        public bool StopMonitor
        {
            get;
            set;
        }

        public double TemperatureLimit
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
                return AllHardwareResources.Where(x => x.NumberOfExperts > 1 || x.ExpertProgrammaticName != "network");
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

        public void StartTemperatureMonitor(string password)
        {
            BackgroundWorker worker = new BackgroundWorker();
            devicesAboveLimit = "";
            worker.DoWork += new DoWorkEventHandler(delegate(object o, DoWorkEventArgs args)
            {
                CanStartMonitor = false;
                StopMonitor = false;

                try
                {
                    // Because the view does not allow modifying resources, there isn't a need to keep
                    // the raw HardwareResourceBase objects after creating the view models.
                    AllHardwareResources = null;
                    var session = new SystemConfiguration.SystemConfiguration(Target, Username, password);

                    Filter filter = new Filter(session);
                    filter.IsDevice = true;
                    filter.SupportsCalibration = true;
                    filter.IsPresent = IsPresentType.Present;
                    filter.IsSimulated = false;

                    ResourceCollection rawResources = session.FindHardware(filter);

                    CanClickStop = true;

                    AllHardwareResources = rawResources
                        .OfType<ProductResource>()
                        .Select(x => new HardwareViewModel(x))
                        .ToList();

                    while (StopMonitor == false)
                    {
                        foreach (HardwareViewModel model in AllHardwareResources)
                        {
                            model.UpdateSensorData(TemperatureLimit);
                        }

                        NotifyPropertyChanged("FilteredHardwareResources"); // Generate PropertyChanged event to update temperatures on UI.

                        devicesAboveLimit = string.Join(", ", AllHardwareResources
                            .Where(r => r.LimitReached)
                            .Select(r => r.UserAlias));

                        if (!string.IsNullOrEmpty(devicesAboveLimit))
                        {
                            MessageBox.Show(string.Format("Warning! {0} is/are above the temperature limit. Stopping scan...", devicesAboveLimit));
                            StopMonitor = true;
                        }
                        System.Threading.Thread.Sleep(100);
                    }
                }
                catch (SystemConfigurationException ex)
                {
                    string errorMessage = string.Format("Find Hardware threw a System Configuration Exception.\n\nErrorCode: {0:X}\n{1}", ex.ErrorCode, ex.Message);
                    MessageBox.Show(errorMessage, "System Configuration Exception");
                }
                finally
                {
                    CanStartMonitor = true;
                    CanClickStop = false;
                }
            });
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