using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using NationalInstruments.SystemConfiguration;

namespace NationalInstruments.Examples.CalibrationAudit
{
    /// <summary>
    /// Creates a background worker in the StartRunAudit method to identify all devices in the system and display calibration data.
    /// </summary>
    internal class CalibrationAuditWorker : INotifyPropertyChanged
    {
        private bool canBeginRunAudit;
        private List<HardwareViewModel> allHardwareResources;

        public CalibrationAuditWorker()
        {
            CanBeginRunAudit = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanBeginRunAudit
        {
            get { return canBeginRunAudit; }
            set
            {
                if (canBeginRunAudit != value)
                {
                    canBeginRunAudit = value;
                    NotifyPropertyChanged("CanBeginRunAudit");
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

        public void StartRunAudit(string password)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(delegate(object o, DoWorkEventArgs args)
                {
                    CanBeginRunAudit = false;
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

                        AllHardwareResources = rawResources // Return all hardware in the system and the corresponding properties.
                            .OfType<ProductResource>()
                            .Select(x => new HardwareViewModel(x))
                            .ToList();
                    }
                    catch (SystemConfigurationException ex)
                    {
                        if (ex.ErrorCode != ErrorCode.PropertyDoesNotExist) // Do not report error if device does not support self calibration (-2147220623).
                        {
                            string errorMessage = string.Format("Find Hardware threw a System Configuration Exception.\n\nErrorCode: {0:X}\n{1}", ex.ErrorCode, ex.Message);
                            MessageBox.Show(errorMessage, "System Configuration Exception");
                        }
                    }
                    finally
                    {
                        CanBeginRunAudit = true;
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
