namespace NationalInstruments.Examples.NIDigital.MultiInstrumentMatchFailCombination
{
    using ModularInstruments.NIDigital;
    using System;

    /// <summary>
    /// Demonstrates how to use the NI-Digital Pattern Driver API to burst a pattern on multiple
    /// synchronized instruments and detect a match fail combination.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Ensure that the DigitalResourceNames field and all instances of the resource names within
    ///    the PinMap.pinmap file match the resource names of your digital pattern instruments.
    /// 3. Ensure that the SyncResourceName field matches the resource name of your NI-Sync instrument.
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

        // IVI resource name of your NI-Sync instrument in MAX.
        private const string SyncResourceName = "PXI1Slot4";

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

        // The sequencer flag that the pattern writes to indicate whether the pattern fails across
        // all instruments.
        private const string SequencerFlag0 = "seqflag0";

        #endregion Settings and Configuration

        private static void Main(string[] args)
        {
            IntPtr syncSession = default(IntPtr);
            try
            {
                // Initialize the NI-Sync session used by the Match Fail Combination feature.
                Console.WriteLine("1. Initializing the NI-Sync instrument session.");
                syncSession = NISyncNativeMethods.Initialize(SyncResourceName, true, true);

                //Initialize the NI-Digital session.
                Console.WriteLine("2. Initializing the digital pattern instrument session.");
                using (NIDigital session = new NIDigital(ResourceName, true, true, OptionString))
                {
                    Console.WriteLine("3. Configuring instrument session.");
                    ConfigureSession(session);

                    Console.WriteLine("4. Enabling Match Fail Combination mode on the instruments.");
                    session.PatternControl.EnableMatchFailCombination(syncSession);

                    Console.WriteLine("5. Bursting the pattern from multiple instruments.");
                    session.PatternControl.BurstPattern(SitesToBurst, PatternStartLabel, true, true, TimeSpan.FromSeconds(10.0));

                    // Read the sequencer flag to determine whether the pattern matched or failed across
                    // all digital pattern instruments.
                    bool failed = session.PatternControl.ReadSequencerFlag(SequencerFlag0);
                    Console.WriteLine();
                    Console.WriteLine("The pattern {0} across all instruments.", failed ? "failed" : "matched");
                    Console.WriteLine();

                    Console.WriteLine("6. Cleaning up for the closure of all sessions.");

                    // Disconnect all channels using programmable, onboard switching.
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
                // If the NI-Sync instrument session has been initialized, close it.
                if (syncSession != IntPtr.Zero)
                {
                    NISyncNativeMethods.Close(syncSession);
                }

                Console.WriteLine("Program complete. Press any key to close.");
                Console.ReadKey();
            }
        }

        private static void ConfigureSession(NIDigital session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session), "You must create a valid NIDigital session before calling ConfigureSession.");
            }

            // Load the pin map for your instrument. Perform the pin map load at the beginning of
            // your program to reference pin names defined in the pin map.
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
    }
}