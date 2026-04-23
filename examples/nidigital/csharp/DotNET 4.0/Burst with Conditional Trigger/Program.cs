namespace NationalInstruments.Examples.NIDigital.BurstWithConditionalTrigger
{
    using ModularInstruments.NIDigital;
    using System;

    /// <summary>
    /// Demonstrates the use of the NI-Digital Pattern Driver API to create and configure a session
    /// and burst a pattern from a digital pattern instrument using a Conditional trigger.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource name of your digital pattern instrument. The default
    ///    value for the resource name is "PXI1Slot2,PXI1Slot3".
    /// 3. In the trigger settings section, set the TriggerTypeSetting field to the trigger type you
    ///    would like to use. If you choose the 'DigitalEdge' trigger type, set the TriggerSource
    ///    field to an appropriate trigger source. The default value, "PXI_Trig0," indicates the
    ///    trigger will be received from PXI trigger line 0.
    /// 4. Build and run the example.
    /// </remarks>
    public sealed class Program
    {
        #region Settings and Configuration

        // Trigger settings.
        private static readonly TriggerType TriggerTypeSetting = TriggerType.Software;

        private const string TriggerSource = "/PXI1Slot2/PXI_Trig0";

        // IVI resource name of your digital pattern instrument in MAX. Multiple instruments may be added as a comma-separated list.
        private const string ResourceName = "PXI1Slot2,PXI1Slot3";

        // Option String for initializing the session. Simulates the hardware.
        private const string OptionString = "Simulate=1,DriverSetup=Model:6570";

        // Pass an empty OptionString to run on hardware (disable simulation)
        //private const string OptionString = "";

        // Timeout to use when waiting for the pattern burst to complete.
        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

        // Files to load.
        private const string PinMapFilePath = "PinMap.pinmap";

        private const string SpecificationsFilePath = "Specifications.specs";
        private const string PinLevelsFilePath = "PinLevels.digilevels";
        private const string TimingSheetFilePath = "Timing.digitiming";
        private const string PatternFilePath = "Pattern.digipat";

        // Pattern settings.
        private const string PatternStartLabel = "new_pattern";

        #endregion Settings and Configuration

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1. Initializing the digital pattern instrument session.");
                using (NIDigital session = new NIDigital(ResourceName, true, true, OptionString))
                {
                    Console.WriteLine("2. Configuring driver session.");
                    ConfigureSession(session);

                    Console.WriteLine($"3. Bursting in {TriggerTypeSetting} trigger mode.");
                    switch (TriggerTypeSetting)
                    {
                        // Select 'None' as the conditional jump trigger mode to burst the pattern from your
                        // instrument based on the configured start label. Pass 'true' for the
                        // waitUntilDone parameter of BurstPattern to ensure the pattern finishes
                        // bursting before execution of this program continues to cleanup.
                        case TriggerType.None:
                            session.PatternControl.BurstPattern(string.Empty, PatternStartLabel, true, true, Timeout);
                            break;

                        // Select 'DigitalEdge' as the conditional jump trigger mode to configure the conditional jump
                        // trigger for digital edge and select the source and edge on which to
                        // trigger. A valid digital edge must be received in order for BurstPattern
                        // to execute without a timeout error.
                        case TriggerType.DigitalEdge:
                            session.Trigger.ConditionalJumpTriggers[0].DigitalEdge.Configure(TriggerSource, DigitalEdge.Rising);
                            session.PatternControl.BurstPattern(string.Empty, PatternStartLabel, true, true, Timeout);
                            break;

                        // Select 'Software' as the conditional jump trigger mode to configure the conditional jump trigger
                        // for software edge and burst the pattern. Pass 'false' for the
                        // waitUntilDone parameter of BurstPattern to make it a non-blocking call.
                        // After BurstPattern is called, the pattern will loop on a single vector waiting for a software trigger
                        // before bursting. Once the software trigger is sent to the hardware, the
                        // pattern completes bursting.
                        case TriggerType.Software:
                            session.Trigger.ConditionalJumpTriggers[0].Software.Configure();

                            // This call to BurstPattern returns immediately because waitUntilDone is 'false.'
                            session.PatternControl.BurstPattern(string.Empty, PatternStartLabel, true, false, Timeout);

                            // Wait for user input to send the software trigger
                            Console.WriteLine("Waiting for software trigger. Press any key to send software trigger.");
                            Console.ReadKey();

                            // When you want to burst the pattern, call Send to send the software trigger.
                            session.Trigger.ConditionalJumpTriggers[0].Software.Send();
                            session.PatternControl.WaitUntilDone(Timeout);
                            break;
                    }

                    // Disconnect all channels using programmable, onboard switching.
                    Console.WriteLine("4. Cleaning up for session closure.");
                    session.PinAndChannelMap.GetPinSet(string.Empty).SelectedFunction = SelectedFunction.Disconnect;
                }
            }
            // Handle driver-specific exceptions before the general exception handler that follows.
            // An example of a driver-specific exception is Ivi.Driver.IviCDriverException.
            catch (Exception e)
            {
                Console.Write(e);
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