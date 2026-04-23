namespace NationalInstruments.Examples.NIDigital.ConfigureLevelsAndTermination
{
    using ModularInstruments.NIDigital;
    using System;

    /// <summary>
    /// Demonstrates the use of the NI-Digital Pattern Driver API to create a session, configure
    /// various voltage levels, and burst a pattern.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource name of your digital pattern instrument. The default
    ///    value for the resource name is "PXI1Slot2,PXI1Slot3".
    /// 3. Set the PinSetTerminationMode property to the termination mode you would like to use.
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

        private static readonly TerminationMode PinSetTerminationMode = TerminationMode.HighZ;
        private const string PinSetString = "PinGroup1";
        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

        // Files to load.
        private const string PinMapFilePath = "PinMap.pinmap";
        private const string SpecificationsFilePath = "Specifications.specs";
        private const string TimingSheetFilePath = "Timing.digitiming";
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
                    Console.WriteLine("2. Configuring instrument session.");
                    ConfigureSession(session);

                    // Configure various voltage levels for your instrument. To begin, obtain a
                    // DigitalPinSet for the particular channel(s), pin(s), or pin group(s) to which
                    // you will apply the settings.
                    // Note: The DigitalPinSet settings are not applied to your instrument until you
                    // burst a pattern or explicitly commit them to hardware using the Commit method
                    // on the PatternControl property of the session.
                    Console.WriteLine("3. Configuring voltage levels.");
                    DigitalPinSet pinGroup1 = session.PinAndChannelMap.GetPinSet(PinSetString);
                    pinGroup1.DigitalLevels.TerminationMode = PinSetTerminationMode;

                    // General voltage levels. Values are specified in volts.
                    pinGroup1.DigitalLevels.Vil = 0.0;
                    pinGroup1.DigitalLevels.Vih = 3.3;
                    pinGroup1.DigitalLevels.Vol = 1.6;
                    pinGroup1.DigitalLevels.Voh = 1.7;

                    // Active Load voltage levels. Values are specified in volts. These values are
                    // used if PinSetTerminationMode is set to TerminationMode.ActiveLoad.
                    pinGroup1.DigitalLevels.Vcom = 0.0;
                    pinGroup1.DigitalLevels.Iol = 2.0e-3;
                    pinGroup1.DigitalLevels.Ioh = -2.0e-3;

                    // Vterm voltage levels. Values are specified in volts. This value is used if
                    // PinSetTerminationMode is set to TerminationMode.Vterm.
                    pinGroup1.DigitalLevels.Vterm = 2.0;

                    Console.WriteLine(string.Format("4. Bursting in {0} termination mode.", PinSetTerminationMode));
                    session.PatternControl.BurstPattern(SitesToBurst, PatternStartLabel, true, true, Timeout);

                    // Disconnect all channels using programmable, onboard switching.
                    Console.WriteLine("5. Cleaning up for session closure.");
                    pinGroup1.SelectedFunction = SelectedFunction.Disconnect;
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
            // your program to reference pin names defined in the pin map.
            session.LoadPinMap(PinMapFilePath);

            // Load specifications and timing sheets created using the Digital Pattern Editor. Timing
            // settings are not applied until you call ApplyLevelsAndTiming on the NIDigital session object.
            // Note: the levels sheet is purposefully not loaded in order to configure levels using
            // the voltage levels properties on the session.
            session.LoadSpecifications(SpecificationsFilePath);
            session.LoadTiming(TimingSheetFilePath);

            // Apply the loaded sheets to your digital pattern instrument.
            session.ApplyLevelsAndTiming(string.Empty, null, TimingSheetFilePath);

            // Load a pattern file onto your instrument.
            session.LoadPattern(PatternFilePath);
        }
    }
}
