namespace NationalInstruments.Examples.NIDigital.LoopbackAndCompare
{
    using ModularInstruments.NIDigital;
    using System;

    /// <summary>
    /// Demonstrates the use of the NI-Digital Pattern Driver API to perform a loopback test. The
    /// example creates and configures a session, and bursts a pattern from a digital pattern
    /// instrument, while checking for pass/fail conditions. The Pattern.digipat file contains
    /// vectors that drive values and read them back, comparing them against an expected result to
    /// perform the loopback.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Connect your digital pattern instrument in an external loopback configuration:
    ///     - Connect physical channel 0 to channel 6.
    ///     - Connect physical channel 1 to channel 7.
    /// 2. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 3. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource name of your digital pattern instrument. The default
    ///    value for the resource name is "PXI1Slot2,PXI1Slot3".
    /// 4. Build and run the example. The results will display in the console window. The example
    ///    files contain a failure which results in an output similar to the following: 'Site 0: Fail'
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

        // Pattern and capture waveform settings.
        private const string PatternStartLabel = "new_pattern";
        private const string PinSetString = "PinGroup1";
        private const string CaptureWaveformName = "new_waveform";

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
                    Console.WriteLine("2. Configuring instrument session.");
                    ConfigureSession(session);

                    // Burst the pattern from the specified start label and with the specified
                    // timeout. Store the site results to display them later.
                    Console.WriteLine("3. Bursting the pattern and retrieving site results.");
                    bool[] siteResults = session.PatternControl.BurstPattern(SitesToBurst, PatternStartLabel, true, TimeSpan.FromSeconds(10));

                    // For the example Pattern.digipat file provided, the output should be "Site 0: Fail"
                    Console.WriteLine("4. Displaying site results.");
                    DisplaySiteResults(siteResults);

                    // Disconnect all channels using programmable, onboard switching.
                    Console.WriteLine("5. Cleaning up for session closure.");
                    session.PinAndChannelMap.GetPinSet(PinSetString).SelectedFunction = SelectedFunction.Disconnect;
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

        private static void ConfigureSession(NIDigital session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session", "You must create a valid NIDigital session before calling ConfigureSession.");
            }

            // Load the pin map for your instrument. Perform the pin map load at the beginning of
            // your program to reference pin names determined by the pin map.
            session.LoadPinMap(PinMapFilePath);

            // Load specifications, levels, and timing sheets created using the Digital Pattern
            // Editor. These settings are not applied until you call ApplyLevelsAndTiming on the
            // NIDigital session object.
            session.LoadSpecifications(SpecificationsFilePath);
            session.LoadLevels(PinLevelsFilePath);
            session.LoadTiming(TimingSheetFilePath);

            // Apply the loaded sheets to your digital pattern instrument.
            session.ApplyLevelsAndTiming(string.Empty, PinLevelsFilePath, TimingSheetFilePath);

            // Load a pattern file onto your instrument.
            session.LoadPattern(PatternFilePath);
        }

        private static void DisplaySiteResults(bool[] siteResults)
        {
            if (siteResults == null)
            {
                throw new ArgumentNullException("siteResults");
            }

            Console.WriteLine();

            for (int i = 0; i < siteResults.Length; ++i)
            {
                Console.WriteLine(string.Format("\tSite {0}: {1}", i, siteResults[i] == true ? "Pass" : "Fail"));
            }

            Console.WriteLine();
        }
    }
}
