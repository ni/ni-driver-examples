using NationalInstruments.SystemConfiguration;

namespace NationalInstruments.Examples.ShowAllHardware
{
    class HardwareViewModel
    {
        public HardwareViewModel(HardwareResourceBase resource)
        {
            UserAlias = resource.UserAlias;
            NumberOfExperts = resource.Experts.Count;
            Expert0ResourceName = resource.Experts[0].ResourceName;
            Expert0ProgrammaticName = resource.Experts[0].ExpertProgrammaticName;

            HardwareResource hardwareResource = resource as HardwareResource;
            if (hardwareResource != null)
            {
                ProductName = hardwareResource.ProductName;
                SerialNumber = hardwareResource.SerialNumber;
            }
        }

        public string UserAlias
        {
            get;
            private set;
        }

        public string Expert0ResourceName
        {
            get;
            private set;
        }

        public string Expert0ProgrammaticName
        {
            get;
            private set;
        }

        public string ProductName
        {
            get;
            private set;
        }

        public string SerialNumber
        {
            get;
            private set;
        }

        public int NumberOfExperts
        {
            get;
            private set;
        }
    }
}
