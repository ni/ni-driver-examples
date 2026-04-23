namespace NationalInstruments.Examples.NIDigital.LoadSheetsAndBurst
{
    using ModularInstruments.NIDigital;
    using System;

    /// <summary>
    /// Demonstrates the use of the NI-Digital Pattern Driver API to create a session, configure by
    /// loading sheets, and burst a pattern from a digital pattern instrument.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource name of your digital pattern instrument. The default
    ///    value for the resource name is "PXI1Slot2,PXI1Slot3".
    /// 3. Specify your own sheets to load or use the provided defaults.
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

        // Files to load.
        private const string PinMapFilePath = "PinMap.pinmap";

        private const string SpecificationsFilePath = "Specifications.specs";
        private const string PinLevelsFilePath = "PinLevels.digilevels";
        private const string TimingSheetFilePath = "Timing.digitiming";
        private const string PatternFilePath = "Pattern.digipat";

        // Pattern label for the pattern to burst.
        private const string PatternStartLabel = "new_pattern";

        // Specifying an empty string as the sites to burst causes the pattern to burst on all
        // defined sites.
        private static readonly string SitesToBurst = string.Empty;

        #endregion Settings and Configuration

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1. Initializing the digital pattern instrument session.");
                using (NIDigital session = new NIDigital(ResourceName, true, true, OptionString))
                {
                    // Load the pin map for your instrument. Perform the pin map load at the
                    // beginning of your program to reference pin names determined by the pin map.
                    Console.WriteLine("2. Loading sheets.");
                    session.LoadPinMap(PinMapFilePath);

                    // Load specifications, levels, and timing sheets created using the Digital
                    // Pattern Editor. These settings are not applied until you call
                    // ApplyLevelsAndTiming on the NIDigital session object.
                    session.LoadSpecifications(SpecificationsFilePath);
                    session.LoadLevels(PinLevelsFilePath);
                    session.LoadTiming(TimingSheetFilePath);

                    // Apply the loaded sheets to your digital pattern instrument.
                    Console.WriteLine("3. Applying the levels and timing sheets to the instrument.");
                    session.ApplyLevelsAndTiming(string.Empty, PinLevelsFilePath, TimingSheetFilePath);

                    // Printing Pin Levels to demonstrate that the Levels file was successfully
                    // loaded. You can edit your Levels file in the Digital Pattern Editor. The
                    // value, "PinGroup1," which is passed for the pinSetString parameter of
                    // PrintPinLevelsForPinSet, is defined in PinMap.pinmap. You can pass any valid
                    // pin set string value here. For example, if you only want to print values for
                    // the first DUT pin, you could pass "DUTPin1" here. This helper method uses the
                    // GetPinSet method on the PinAndChannelMap property of the NIDigital session to
                    // get and print the pin set information for the pinSetString passed.
                    Console.WriteLine("4. Checking loaded pin levels.");
                    PrintPinLevelsForPinSet(session, "PinGroup1");

                    // Load a pattern file onto your instrument.
                    Console.WriteLine("5. Loading the pattern file.");
                    session.LoadPattern(PatternFilePath);

                    // Burst the pattern at the specified start label and with the specified timeout.
                    Console.WriteLine("6. Bursting the pattern.");
                    session.PatternControl.BurstPattern(SitesToBurst, PatternStartLabel, true, true, TimeSpan.FromSeconds(10.0));

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

        private static void PrintPinLevelsForPinSet(NIDigital session, string pinSetString)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session", "You must create a valid NIDigital session before calling PrintPinLevelsForPinSet.");
            }

            DigitalLevels pinSetLevels = session.PinAndChannelMap.GetPinSet(pinSetString).DigitalLevels;

            Console.WriteLine();
            Console.WriteLine(string.Format("{0} levels:", pinSetString));
            Console.WriteLine(string.Format("\tTermination mode: {0}", pinSetLevels.TerminationMode));
            Console.WriteLine(string.Format("\tVil: {0:N} V", pinSetLevels.Vil));
            Console.WriteLine(string.Format("\tVih: {0:N} V", pinSetLevels.Vih));
            Console.WriteLine(string.Format("\tVol: {0:N} V", pinSetLevels.Vol));
            Console.WriteLine(string.Format("\tVoh: {0:N} V", pinSetLevels.Voh));
            Console.WriteLine();
        }
    }
}