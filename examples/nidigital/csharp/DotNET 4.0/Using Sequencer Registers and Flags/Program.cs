namespace NationalInstruments.Examples.NIDigital.UsingSequencerRegistersAndFlags
{
    using ModularInstruments.NIDigital;
    using System;
    using System.Threading;

    /// <summary>
    /// Demonstrates the use of the NI-Digital Pattern Driver API to read and write sequencer
    /// registers and flags.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource name of your digital pattern instrument. The default
    ///    value for the resource name is "PXI1Slot2,PXI1Slot3".
    /// 3. Notice the sequencer registers section of the Settings and Configuration region. The
    ///    constants defined in this section identify sequencer registers used in the pattern and
    ///    values written to those registers by this example program.
    /// 4. Notice the sequencer flags section of the Settings and Configuration region. The constants
    ///    defined in this section identify sequencer flags used in the pattern and a value to write
    ///    to one of them. You may change the SequencerFlag1State field to change which value is
    ///    written to seqflag1 during the execution of this example.
    /// 5. Build and run the example.
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

        // Sequencer registers: The register values determine the number of loop iterations for two
        // loops within the pattern.
        private const string SequencerRegister0 = "reg0";
        private const int Register0Value = 100;
        private const string SequencerRegister1 = "reg1";
        private const int Register1Value = 50;

        // Sequencer flags: The sequencer flags are used to perform handshaking between the bursting
        // pattern and this example program.
        private const string SequencerFlag0 = "seqflag0";
        private const string SequencerFlag1 = "seqflag1";
        private const bool SequencerFlag1State = true;

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

                    // Write SequencerRegister0 with a value representing the number of loop
                    // iterations for the loaded pattern. The value written into SequencerRegister0
                    // by the application is interpreted by the pattern at runtime.
                    Console.WriteLine(string.Format("3. Writing a value of {0} to sequencer register {1}.", Register0Value, SequencerRegister0));
                    session.PatternControl.WriteSequencerRegister(SequencerRegister0, Register0Value);

                    // Burst the pattern from your instrument based on the configured start label.
                    // This can be a label in your .digipat file, or it can be the name of the
                    // pattern. The waitUntilDone parameter is passed 'false,' which makes the burst
                    // pattern function non-blocking, and the execution of the execution of this
                    // example code continues while the pattern is bursting.
                    Console.WriteLine("4. Beginning to burst the pattern.");
                    session.PatternControl.BurstPattern(SitesToBurst, PatternStartLabel, true, false, default(TimeSpan));

                    // During execution of the pattern, the application can interact with the burst
                    // sequence by writing a new register value after bursting begins. Here, the
                    // second portion of the pattern is looped based on the value of SequencerRegister1.
                    Console.WriteLine(string.Format("5. Writing a value of {0} to sequencer register {1}.", Register1Value, SequencerRegister1));
                    session.PatternControl.WriteSequencerRegister(SequencerRegister1, Register1Value);

                    // Read the value of SequencerRegister1 to verify that the value has updated.
                    int secondaryLoopCount = session.PatternControl.ReadSequencerRegister(SequencerRegister1);
                    Console.WriteLine();
                    Console.WriteLine(string.Format("   Sequencer register {0} has been set to a value of {1}.", SequencerRegister1, secondaryLoopCount));
                    Console.WriteLine();

                    // The execution of the pattern can control application flow through sequencer
                    // flag handshaking. In this case, the value read from SequencerFlag0 influences
                    // whether this example program continues to execute.
                    Console.WriteLine("6. Performing simple sequencer flag handshake using {0}.", SequencerFlag0);
                    Console.WriteLine();

                    while (!session.PatternControl.ReadSequencerFlag(SequencerFlag0) && !Program.EnterKeyPressed)
                    {
                        Console.Write("\r   Waiting for the pattern to set sequencer flag to true. Press <Enter> to continue program execution without waiting.");
                        Thread.Sleep(100);
                    }

                    Console.WriteLine();
                    Console.WriteLine();

                    // During execution of the pattern, the application can set sequencer flags that
                    // influence how the pattern bursts. Here, the program sets the value of
                    // SequencerFlag1, telling the pattern sequencer whether to continue executing
                    // the second loop in the bursting pattern.
                    Console.WriteLine("7. Writing a value of {0} to sequencer flag {1}", SequencerFlag1State, SequencerFlag1);
                    session.PatternControl.WriteSequencerFlag(SequencerFlag1, SequencerFlag1State);

                    // Read the value of SequencerFlag1 to verify that the value has updated.
                    bool sequencerFlag1Value = session.PatternControl.ReadSequencerFlag(SequencerFlag1);
                    Console.WriteLine();
                    Console.WriteLine(string.Format("   Sequencer flag {0} has been set to a value of {1}.", SequencerFlag1, sequencerFlag1Value));
                    Console.WriteLine();

                    // Disconnect all channels using programmable, onboard switching.
                    Console.WriteLine("8. Cleaning up for session closure.");
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

        private static bool EnterKeyPressed
        {
            get
            {
                return Console.KeyAvailable
                    && Console.ReadKey(true).Key == ConsoleKey.Enter;
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
