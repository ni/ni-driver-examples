namespace NationalInstruments.Examples.NIDigital.CreateTimeSets
{
    using System;
    using Ivi.Driver;
    using ModularInstruments.NIDigital;

    /// <summary>
    /// Demonstrates the use of the NI-Digital Pattern Driver API to configure a time set for the pattern.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource name of your digital pattern instrument. The default
    ///    value for the resource name is "PXI1Slot2,PXI1Slot3".
    /// 3. Set PinSetString to identify the channel(s)/pin(s) you would like to configure.
    /// 4. Give the time set a name and configure the Period property for the time set.
    /// 5. Set the drive format with the DriveEdgesFormat property and set the various edge placement
    ///    locations for the time set.
    /// 6. Configure the strobe edge for capture operations.
    /// 7. Build and run the example.
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
        
        // Time set settings.
        private const string TimeSetName = "tset0";
        private const string PinSetString = "PinGroup1";

        // Time set period setting for all channels.
        private static readonly PrecisionTimeSpan TimeSetPeriod = PrecisionTimeSpan.FromNanoseconds(20.0);

        // Time set drive edges settings for drive edge channels.
        private const DriveFormat DriveEdgesFormat = DriveFormat.NonReturn;
        private static readonly PrecisionTimeSpan DriveOnEdge = PrecisionTimeSpan.FromNanoseconds(0.0);
        private static readonly PrecisionTimeSpan DriveDataEdge = PrecisionTimeSpan.FromNanoseconds(0.0);
        private static readonly PrecisionTimeSpan DriveReturnEdge = PrecisionTimeSpan.FromNanoseconds(15.0);
        private static readonly PrecisionTimeSpan DriveOffEdge = PrecisionTimeSpan.FromNanoseconds(20.0);

        // Time set compare edges setting for compare edge channels.
        private static readonly PrecisionTimeSpan StrobeEdge = PrecisionTimeSpan.FromNanoseconds(10.0);

        // Files to load.
        private const string PinMapFilePath = "PinMap.pinmap";
        private const string SpecificationsFilePath = "Specifications.specs";
        private const string PinLevelsFilePath = "PinLevels.digilevels";
        private const string PatternFilePath = "Pattern.digipat";

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
                    Console.WriteLine("2. Configuring driver session.");
                    ConfigureSession(session);

                    // Create a new time set with the specified time set name.
                    Console.WriteLine("3. Creating a new time set and configuring timing properties.");
                    DigitalTimeSet timeSet = session.Timing.CreateTimeSet(TimeSetName);

                    // Configure the period, drive edges for the specified channels, and the compare
                    // (strobe) edges for those channels.
                    timeSet.ConfigurePeriod(TimeSetPeriod);
                    timeSet.ConfigureDriveEdges(PinSetString, DriveEdgesFormat, DriveOnEdge, DriveDataEdge, DriveReturnEdge, DriveOffEdge);
                    timeSet.ConfigureCompareEdgesStrobe(PinSetString, StrobeEdge);

                    // Burst the pattern from your instrument based on the configured start label.
                    // This can be a label in your .digipat file, or it can be the name of the pattern.
                    Console.WriteLine("4. Bursting the pattern.");
                    session.PatternControl.BurstPattern(SitesToBurst, PatternStartLabel, true, true, TimeSpan.FromSeconds(10.0));

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

        private static void ConfigureSession(NIDigital session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session", "You must create a valid NIDigital session before calling ConfigureSession.");
            }

            // Load the pin map for your instrument. Perform the pin map load at the beginning of
            // your program to reference pin names determined by the pin map.
            session.LoadPinMap(PinMapFilePath);

            // Load specifications and timing sheets created using the Digital Pattern Editor. Timing
            // settings are not applied until you call ApplyLevelsAndTiming on the NIDigital session object.
            // Note: the timing sheet is purposefully not loaded in order to configure the timing using
            // the configuration methods on the DigitalTimeSet object.
            session.LoadSpecifications(SpecificationsFilePath);
            session.LoadLevels(PinLevelsFilePath);

            // Apply the loaded sheets to your digital pattern instrument.
            session.ApplyLevelsAndTiming(string.Empty, PinLevelsFilePath, string.Empty);

            // Load a pattern file onto your instrument.
            session.LoadPattern(PatternFilePath);
        }
    }
}
