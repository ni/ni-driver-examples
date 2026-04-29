using System;
using System.Linq;
using NationalInstruments.SystemConfiguration;

namespace NationalInstruments.Examples.CalibrationAudit
{
    /// <summary>
    /// The device view model is responsible for internal and external calibration data and temperature.
    /// </summary>
    internal class HardwareViewModel
    {
        private const string NotAvailable = "N/A";

        public HardwareViewModel(ProductResource resource)
        {
            UserAlias = resource.UserAlias;
            NumberOfExperts = resource.Experts.Count;
            ExpertResourceName = resource.Experts[0].ResourceName;
            ExpertProgrammaticName = resource.Experts[0].ExpertProgrammaticName;
            Model = resource.ProductName;
            SerialNumber = resource.SerialNumber;

            if (resource.SupportsInternalCalibration)
            {
                ShowInternalCalData(resource);
            }
            else
            {
                InternalLastCalDate = NotAvailable;
                InternalLastCalTemp = NotAvailable;
            }

            if (resource.SupportsExternalCalibration)
            {
                ShowExternalCalData(resource);
            }
            else
            {
                ExternalLastCalDate = NotAvailable;
                ExternalLastCalTemp = NotAvailable;
                RecommendedNextCal = NotAvailable;
            }

            ShowTemperatureData(resource);
        }

        // Helper functions use try catch blocks to detect System Configuration exceptions.
        // If a value is not supported by the device (returns an exception) then "N/A" is returned.

        public void ShowInternalCalData(ProductResource resource)
        {
            try
            {
                CalibrationDetail[] details = resource.QueryInternalCalibration();
                if (details.Length > 0)
                {
                    InternalLastCalDate = details[0].LastTime.ToLocalTime().ToShortDateString();
                    InternalLastCalTemp = details[0].LastTemperature.ToString("0.00");
                    return;
                }
            }
            catch (SystemConfigurationException ex)
            {
                Error = ex.ErrorCode.ToString();
            }
            InternalLastCalDate = NotAvailable;
            InternalLastCalTemp = NotAvailable;
        }

        public void ShowExternalCalData(ProductResource resource)
        {
            CalibrationOverdue = false;

            try
            {
                ExternalLastCalDate = resource.ExternalCalibrationDate.ToLocalTime().ToShortDateString();
            }
            catch (SystemConfigurationException ex)
            {
                ExternalLastCalDate = NotAvailable;
                Error = ex.ErrorCode.ToString();
            }

            try
            {
                ExternalLastCalTemp = resource.ExternalCalibrationTemperature.ToString("0.00");
            }
            catch (SystemConfigurationException ex)
            {
                ExternalLastCalTemp = NotAvailable;
                Error = ex.ErrorCode.ToString();
            }
            try
            {
                RecommendedNextCal = resource.ExternalCalibrationDueDate.ToLocalTime().ToString("MM/yyyy");
                CalibrationOverdue = resource.ExternalCalibrationDueDate < DateTime.Now;
            }
            catch (SystemConfigurationException ex)
            {
                RecommendedNextCal = NotAvailable;
                Error = ex.ErrorCode.ToString();
            }
        }

        public void ShowTemperatureData(ProductResource productResource)
        {
            try
            {
                TemperatureSensor[] sensors = productResource.QueryTemperatureSensors(SensorInfo.Reading);

                Temperature = string.Join(", ", sensors
                    .Select(s => s.Reading.ToString("0.00")));
            }
            catch (SystemConfigurationException ex) // If device does not have internal temperature sensor(s), display temperature as "N/A".
            {
                Temperature = NotAvailable;
                Error = ex.ErrorCode.ToString();
            }
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

        public string Model
        {
            get;
            private set;
        }

        public string SerialNumber
        {
            get;
            private set;
        }

        public string InternalLastCalDate
        {
            get;
            private set;
        }

        public string InternalLastCalTemp
        {
            get;
            private set;
        }

        public string ExternalLastCalDate
        {
            get;
            private set;
        }

        public string ExternalLastCalTemp
        {
            get;
            private set;
        }

        public string RecommendedNextCal
        {
            get;
            private set;
        }

        public string Temperature
        {
            get;
            private set;
        }

        public string Error
        {
            get;
            private set;
        }

        public bool CalibrationOverdue
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
