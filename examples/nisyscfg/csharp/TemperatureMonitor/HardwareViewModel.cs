using System.Linq;
using NationalInstruments.SystemConfiguration;

namespace NationalInstruments.Examples.BoardTemperatureMonitor
{
    /// <summary>
    /// Class for each device in the system that contains temperature data.
    /// </summary>
    internal class HardwareViewModel
    {
        public HardwareViewModel(ProductResource resource)
        {
            Resource = resource;
            UserAlias = resource.UserAlias;
            NumberOfExperts = resource.Experts.Count;
            ExpertResourceName = resource.Experts[0].ResourceName;
            ExpertProgrammaticName = resource.Experts[0].ExpertProgrammaticName;
            LimitReached = false;
        }

        public void UpdateSensorData(double temperatureLimit)
        {
            try
            {
                TemperatureSensor[] sensors = Resource.QueryTemperatureSensors(SensorInfo.Reading);

                Temperature = string.Join(", ", sensors
                    .Select(s => s.Reading.ToString("0.00")));

                LimitReached = sensors
                    .Any(s => s.Reading > temperatureLimit);
            }
            catch (SystemConfigurationException)
            {
                Temperature = "N/A"; // If device does not have internal temperature sensor(s), display temperature as "N/A".
            }
        }

        public ProductResource Resource
        {
            get;
            private set;
        }

        public string UserAlias
        {
            get;
            private set;
        }

        public string ExpertResourceName
        {
            get;
            private set;
        }

        public string ExpertProgrammaticName
        {
            get;
            private set;
        }

        public string Temperature
        {
            get;
            private set;
        }

        public int NumberOfExperts
        {
            get;
            private set;
        }

        public bool LimitReached
        {
            get;
            private set;
        }
    }
}