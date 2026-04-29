using NationalInstruments.SystemConfiguration;

namespace NationalInstruments.Examples.RenameAliases
{
    public class DeviceViewModel
    {
        public DeviceViewModel(ProductResource deviceInfo)
        {
            ProductName = deviceInfo.ProductName;
            UserAlias = deviceInfo.UserAlias;
        }

        public string NewAlias
        {
            get;
            set;
        }

        public string ProductName
        {
            get;
            set;
        }

        public string UserAlias
        {
            get;
            set;
        }
    }
}
