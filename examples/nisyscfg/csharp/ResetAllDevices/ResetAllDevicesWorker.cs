using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using NationalInstruments.SystemConfiguration;

namespace NationalInstruments.Examples.ResetAllDevices
{
    class ResetAllDevicesWorker : INotifyPropertyChanged
    {
        private bool gridEnabled;
        private List<DeviceViewModel> deviceList;

        public ResetAllDevicesWorker()
        {
            GridEnabled = true;
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

        public List<DeviceViewModel> DeviceList
        {
            get { return deviceList; }
            set
            {
                if (deviceList != value)
                {
                    deviceList = value;
                    NotifyPropertyChanged("DeviceList");
                }
            }
        }

        public bool GridEnabled
        {
            get { return gridEnabled; }
            set
            {
                if (gridEnabled != value)
                {
                    gridEnabled = value;
                    NotifyPropertyChanged("GridEnabled");
                }
            }
        }

        public void GetAndResetDevices(string password)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(delegate(object o, DoWorkEventArgs args)
                {
                    GridEnabled = false;
                    try
                    {
                        DeviceList = null;
                        SystemConfiguration.SystemConfiguration session = new SystemConfiguration.SystemConfiguration(Target, Username, password);
                        Filter filter = new Filter(session)
                        {
                           IsDevice = true
                        };
                        var viewModels =
                            from resource in session.FindHardware(filter)
                            where resource is ProductResource
                            select new DeviceViewModel((ProductResource)resource);
                        DeviceList = new List<DeviceViewModel>(viewModels);
                        foreach (var device in DeviceList)
                        {
                            try
                            {
                                device.Device.Reset();
                                device.ResultString = "Pass";
                            }
                            catch (SystemConfigurationException ex)
                            {
                                device.ResultString = (ex.ErrorCode == ErrorCode.NotImplemented) ? "Not Supported" : string.Format("Error Code: {0:X}", ex.ErrorCode);
                            }
                        }
                    }
                    catch (SystemConfigurationException ex)
                    {
                        string errorMessage = string.Format("Find Hardware threw a System Configuration Exception.\n\nErrorCode: {0:X}\n{1}", ex.ErrorCode, ex.Message);
                        MessageBox.Show(errorMessage, "System Configuration Exception");
                    }
                    finally
                    {
                        GridEnabled = true;
                    }
                }
            );
            worker.RunWorkerAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

