using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using NationalInstruments.SystemConfiguration;

namespace NationalInstruments.Examples.RenameAliases
{
    class RenameAliasWorker : INotifyPropertyChanged
    {
        private ResourceCollection allHardwareResources;
        private bool canBeginShowHardware;
        private List<DeviceViewModel> deviceList;

        public RenameAliasWorker()
        {
            CanBeginShowHardware = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        public List<DeviceViewModel> DeviceList
        {
            get
            {
                if (deviceList != null)
                {
                    return deviceList;
                }
                deviceList = new List<DeviceViewModel>();
                if (AllHardwareResources == null)
                {
                    return deviceList;
                }
                foreach (HardwareResourceBase resource in AllHardwareResources)
                {
                    deviceList.Add(new DeviceViewModel((ProductResource)resource));
                }
                return deviceList;
            }
        }

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

        private ResourceCollection AllHardwareResources
        {
            get { return allHardwareResources; }
            set
            {
                if (allHardwareResources != value)
                {
                    allHardwareResources = value;
                    InvalidateDeviceList();
                }
            }
        }

        public void GetDevices(string password)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(delegate(object o, DoWorkEventArgs args)
                {
                    CanBeginShowHardware = false;
                    try
                    {
                        AllHardwareResources = null;
                        SystemConfiguration.SystemConfiguration session = new SystemConfiguration.SystemConfiguration(Target, Username, password);
                        SystemConfiguration.Filter filter = new SystemConfiguration.Filter(session);
                        filter.IsDevice = true;
                        AllHardwareResources = session.FindHardware(filter, "daqmx,ni-visa");
                    }
                    catch (SystemConfigurationException ex)
                    {
                        string errorMessage = string.Format("Find Hardware threw a System Configuration Exception.\n\nError Code: {0:X}\n{1}", ex.ErrorCode, ex.Message);
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

        public void SubmitChanges()
        {
            try
            {
                for (int i = 0; i < DeviceList.Count(); i++)
                {
                    if (!string.IsNullOrWhiteSpace(DeviceList.ElementAt(i).NewAlias) && DeviceList.ElementAt(i).NewAlias != AllHardwareResources[i].UserAlias)
                    {
                        ((ProductResource)AllHardwareResources[i]).Rename(DeviceList.ElementAt(i).NewAlias, true);
                    }
                }
            }
            catch (SystemConfigurationException ex)
            {
                string errorMessage = string.Format("Rename threw a System Configuration Exception.\n\nError Code: {0:X}\n{1}", ex.ErrorCode, ex.Message);
                MessageBox.Show(errorMessage, "System Configuration Exception");
            }
            finally
            {
                CanBeginShowHardware = true;
            }
        }

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void InvalidateDeviceList()
        {
            deviceList = null;
            NotifyPropertyChanged("DeviceList");
        }
    }
}