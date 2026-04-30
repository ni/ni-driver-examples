using System.ComponentModel;
using NationalInstruments.SystemConfiguration;

namespace NationalInstruments.Examples.ResetAllDevices
{
    public class DeviceViewModel : INotifyPropertyChanged
    {
        private string resultString;

        public DeviceViewModel(ProductResource deviceInfo)
        {
            Device = deviceInfo;
            UserAlias = deviceInfo.UserAlias;
            ResourceName = deviceInfo.Experts[0].ResourceName;
            ProductName = deviceInfo.ProductName;
        }

        public string UserAlias
        {
            get;
            private set;
        }

        public string ResourceName
        {
            get;
            private set;
        }

        public string ProductName
        {
            get;
            private set;
        }

        public ProductResource Device
        {
            get;
            private set;
        }

        public string ResultString
        {
            get { return resultString; }
            set
            {
                if (value != resultString)
                {
                    resultString = value;
                    NotifyPropertyChanged("ResultString");
                }
            }
        }

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}