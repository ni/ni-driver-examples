namespace NationalInstruments.Examples.NIDigital.PpmuSourceAndMeasure
{
    using System;
    using System.Threading;
    using ModularInstruments.NIDigital;

    /// <summary>
    /// Demonstrates the use of the NI-Digital Pattern Driver API to configure and source or measure
    /// voltage and current using the PPMU on selected channels/pins of a digital pattern instrument.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource name of your digital pattern instrument. The default
    ///    value for the resource name is "PXI1Slot2,PXI1Slot3".
    /// 3. Configure the pin set used to source and measure by setting the PinSetString field and
    ///    configuring the timing.
    /// 4. Select the PPMU output function to use by setting the SourceType field. If you select
    ///    either DCVoltage or DCCurrent, configure the corresponding settings for DCVoltage or
    ///    DCCurrent that follow.
    /// 5. Select the PPMU measurement type by setting the MeasurementType field.
    /// 6. Build and run the example.
    /// </remarks>
    public sealed class Program
    {
        #region Settings and Configuration

        // IVI resource name of your digital pattern instrument in MAX. Multiple instruments may be added as a comma-separated list.
        private const string ResourceName = "PXI1Slot2,PXI1Slot3";
        
        // Option String for initializing the session. Simulates the hardware.
        private const string OptionString = "Simulate=1,DriverSetup=Model:6570";

        // Pass an empty OptionString to run on hardware (disable simulation)
        //private const string OptionString = "";

        // Pin map file to load.
        private const string PinMapFilePath = "PinMap.pinmap";

        // Pin set string identifying the pins defined in the pin map file used to measure and source
        // voltage and current.
        private const string PinSetString = "DUTPin1, SystemPin1";

        // Timing configuration.
        private const PpmuApertureTimeUnits ApertureTimeUnits = PpmuApertureTimeUnits.Seconds;
        private const double ApertureTime = 4.0e-6; // 4 uS
        private const int SettlingTimeDelay = 10; // 10 ms

        // Determines whether the following DC voltage or current sourcing configuration settings are
        // used. The SourceTypeMode enum is defined in this file.
        private static readonly SourceTypeMode SourceType = SourceTypeMode.DCCurrent;

        // DC voltage configuration.
        private const double VoltageLevel = 3.3; // 3.3 V
        private const double CurrentLimitRange = 2.0e-6; // 2 uA

        // DC current configuration.
        private const double CurrentLevel = 2.0e-6; // 2 uA
        private const double CurrentLevelRange = 2.0e-6; // 2 uA
        private const double VoltageLimitLow = 0.0; // 0.0 V
        private const double VoltageLimitHigh = 3.3; // 3.3 V

        // Determines whether to measure voltage or current.
        private static readonly PpmuMeasurementType MeasurementType = PpmuMeasurementType.Voltage;

        #endregion // Settings and Configuration

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1. Initializing the digital pattern instrument session.");
                using (NIDigital session = new NIDigital(ResourceName, true, true, OptionString))
                {
                    // Load the pin map for your instrument. Perform the pin map load at the
                    // beginning of your program to reference pin names defined in the pin map.
                    Console.WriteLine("2. Loading the pin map file.");
                    session.LoadPinMap(PinMapFilePath);

                    // Create a DigitalPinSet object and configure the aperture time for all PPMU
                    // operations for the pins it represents.
                    Console.WriteLine("3. Creating a pin set and configuring the PPMU aperture time.");
                    DigitalPinSet pinSet = session.PinAndChannelMap.GetPinSet(PinSetString);
                    pinSet.Ppmu.ConfigureApertureTime(ApertureTime, ApertureTimeUnits);

                    Console.WriteLine("4. Configuring and sourcing using settings for the {0} source type.", SourceType);
                    switch (SourceType)
                    {
                        // Select 'DCVoltage' as the source type to configure the output function to
                        // be DC Voltage. Note: Check your instrument specifications to determine
                        // valid ranges. Some instruments may not current-limit the source voltage
                        // within the configured range. If this is the case, the CurrentLimitRange
                        // value determines the accuracy of the voltage sourced.
                        case SourceTypeMode.DCVoltage:
                            pinSet.Ppmu.OutputFunction = PpmuOutputFunction.DCVoltage;
                            pinSet.Ppmu.DCVoltage.CurrentLimitRange = CurrentLimitRange;
                            pinSet.Ppmu.DCVoltage.VoltageLevel = VoltageLevel;

                            // Source the voltage as configured in the previous lines. The Source
                            // method sources based on the configured output function.
                            pinSet.Ppmu.Source();

                            // Insert a settling time between sourcing and measuring.
                            Thread.Sleep(SettlingTimeDelay);
                            break;

                        // Select 'DCCurrent' as the source type to configure the output function to
                        // be DC Current. Note: The range selected determines the accuracy of the
                        // current sourced. Use the smallest range that will still allow you to
                        // source the desired current level.
                        case SourceTypeMode.DCCurrent:
                            pinSet.Ppmu.OutputFunction = PpmuOutputFunction.DCCurrent;
                            pinSet.Ppmu.DCCurrent.CurrentLevelRange = CurrentLevelRange;
                            pinSet.Ppmu.DCCurrent.CurrentLevel = CurrentLevel;

                            // Configuring the voltage limits determines the levels at which the
                            // voltage clamps activate. See your instrument specifications for more information.
                            pinSet.Ppmu.DCCurrent.ConfigureVoltageLimits(VoltageLimitLow, VoltageLimitHigh);

                            // Source the current as configured in the previous lines. The Source
                            // method sources based on the configured output function.
                            pinSet.Ppmu.Source();

                            // Insert a settling time between sourcing and measuring.
                            Thread.Sleep(SettlingTimeDelay);
                            break;

                        // Select 'None' as the source type to bypass sourcing voltage or current.
                        case SourceTypeMode.None:
                            break;
                    }

                    // Measure a voltage or current based on the configured measurement type. Use the
                    // digital pin information and the measurement results to display results in the console.
                    Console.WriteLine("6. Measuring {0} using the PPMU.", MeasurementType);
                    DigitalPinInformation[] pinSetInformation = session.Utility.GetPinResultsPinInformation(pinSet);
                    double[] measurements = pinSet.Ppmu.Measure(MeasurementType);
                    PrintPpmuMeasurements(pinSetInformation, measurements, MeasurementType);

                    // Disconnect all channels using programmable, onboard switching.
                    Console.WriteLine("7. Cleaning up for session closure.");
                    session.PinAndChannelMap.GetPinSet(string.Empty).SelectedFunction = SelectedFunction.Disconnect;
                }
            }
            // Handle driver-specific exceptions before the general exception handler that follows.
            // An example of a driver-specific exception is Ivi.Driver.IviCDriverException.
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("Program complete. Press any key to close.");
                Console.ReadKey();
            }
        }

        private static void PrintPpmuMeasurements(DigitalPinInformation[] pinSetInformation, double[] measurements, PpmuMeasurementType measurementType)
        {
            if (pinSetInformation == null)
            {
                throw new ArgumentNullException("pinSetInformation");
            }

            if (measurements == null)
            {
                throw new ArgumentNullException("measurements");
            }

            Console.WriteLine();
            Console.WriteLine("{0} measurements:", measurementType);

            for (int i = 0; i < pinSetInformation.Length; ++i)
            {
                Console.WriteLine("\t{0}: {1} {2}",
                    pinSetInformation[i].PinName,
                    measurements[i],
                    measurementType == PpmuMeasurementType.Current ? "A" : "V");
            }

            Console.WriteLine();
        }

        internal enum SourceTypeMode
        {
            DCVoltage,
            DCCurrent,
            None
        }
    }
}
