namespace NationalInstruments.Examples.NIDigital.SelectPinFunction
{
    using ModularInstruments.NIDigital;
    using System;

    /// <summary>
    /// Demonstrates the use of the NI-Digital Pattern Driver API to switch between different pin
    /// selected functions on your digital pattern instrument.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource name of your digital pattern instrument. The default
    ///    value for the resource name is "PXI1Slot2,PXI1Slot3".
    /// 3. In the pin group settings section, configure the selected function and pin state. The
    ///    PinGroupState field is only used if the PinGroupSelectedFunction is set to SelectedFunction.Digital.
    /// 4. Build and run the example.
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

        // Pin group settings.
        private const string PinGroupName = "PinGroup1";
        private static readonly SelectedFunction PinGroupSelectedFunction = SelectedFunction.Ppmu;
        private const PinState PinGroupState = PinState._0;

        // Pattern label for the pattern to burst.
        private const string PatternStartLabel = "new_pattern";

        // Specifying an empty string as the sites to burst causes the pattern to burst on all
        // defined sites.
        private static readonly string SitesToBurst = string.Empty;

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
                    Console.WriteLine("2. Configuring instrument session.");
                    session.LoadPinMap(PinMapFilePath);

                    // Create a DigitalPinSet object you can use to set the selected function for a
                    // pin set. Based on the selected function you configure, you can use the
                    // DigitalPinSet object to perform corresponding functions, some of which are
                    // demonstrated for the Digital or PPMU selected function as follows.
                    Console.WriteLine("3. Creating a new DigitalPinSet and configuring the selected function.");
                    DigitalPinSet pinGroup1 = session.PinAndChannelMap.GetPinSet(PinGroupName);
                    DigitalPinInformation[] pinGroup1Information = session.Utility.GetPinResultsPinInformation(pinGroup1);
                    pinGroup1.SelectedFunction = PinGroupSelectedFunction;

                    switch (PinGroupSelectedFunction)
                    {
                        case SelectedFunction.Digital:
                            // If the selected function is set to Digital, exercise the static write
                            // and static read functionality.
                            Console.WriteLine("4. Writing and reading pin state statically using Digital-configured pins.");
                            pinGroup1.WriteStatic(PinGroupState);
                            PinState[] pinStateData = pinGroup1.ReadStatic();
                            PrintPinStateReadings(pinGroup1Information, pinStateData);
                            break;

                        case SelectedFunction.Ppmu:
                            // If the selected function is set to PPMU, measure voltage.
                            Console.WriteLine("4. Reading voltage using PPMU-configured pins.");
                            double[] voltageData = pinGroup1.Ppmu.Measure(PpmuMeasurementType.Voltage);
                            PrintVoltageMeasurements(pinGroup1Information, voltageData);
                            break;

                        case SelectedFunction.Off:
                        case SelectedFunction.Disconnect:
                        default:
                            // Add code in the Off or Disconnect cases to do something specific when
                            // the selected function is set to off or disconnect. For brevity, these
                            // cases have been left as default.
                            Console.WriteLine("4. Doing nothing for the default case.");
                            break;
                    }

                    // Disconnect all channels using programmable, onboard switching.
                    Console.WriteLine("5. Cleaning up for session closure.");
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

        private static void PrintVoltageMeasurements(DigitalPinInformation[] pinSetInformation, double[] measurements)
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
            Console.WriteLine("Voltage measurements:");

            for (int i = 0; i < pinSetInformation.Length; ++i)
            {
                Console.WriteLine(string.Format("\t{0}: {1} V", pinSetInformation[i].PinName, measurements[i]));
            }

            Console.WriteLine();
        }

        private static void PrintPinStateReadings(DigitalPinInformation[] pinSetInformation, PinState[] readings)
        {
            if (pinSetInformation == null)
            {
                throw new ArgumentNullException("pinSetInformation");
            }

            if (readings == null)
            {
                throw new ArgumentNullException("readings");
            }

            Console.WriteLine();
            Console.WriteLine("Pin state readings:");

            for (int i = 0; i < pinSetInformation.Length; ++i)
            {
                Console.WriteLine(string.Format("\t{0} pin state: {1}", pinSetInformation[i].PinName, readings[i]));
            }

            Console.WriteLine();
        }
    }
}
