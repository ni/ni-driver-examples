namespace NationalInstruments.Examples.NIDigital.TmuDebounce
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ModularInstruments.NIDigital;

    /// <summary>
    /// Demonstrates the use of the NI-Digital Pattern Driver API to configure TMU input debounce
    /// filtering. This example shows:
    ///   - Discovering available TMU resources using GetDisabledTmuContexts
    ///   - Configuring debounce times for start and stop inputs
    ///   - The constraint that when start and stop sources are the same channel, the start and stop
    ///     debounce times must be equal
    ///   - When start and stop sources are different channels, the debounce times can be configured
    ///     independently
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource name of your digital pattern instrument. The default
    ///    value for the resource name is "PXI1Slot5".
    /// 3. Build and run the example.
    /// </remarks>
    public sealed class Program
    {
        #region Settings and Configuration

        // IVI resource name of your digital pattern instrument in MAX.
        private const string ResourceName = "PXI1Slot5";

        // Option String for initializing the session. Simulates the hardware.
        private const string OptionString = "Simulate=1,DriverSetup=Model:6571";

        // If using a real instrument connected to a DUT generating signals on the channels
        // configured for the TMU measurements, pass an empty option string to disable simulation.
        //private const string OptionString = "";

        // Pin map file to load.
        private const string PinMapFilePath = "PinMap.pinmap";

        // Pin names used for TMU measurements (must match pins in PinMap.pinmap).
        private const string DutPin1 = "DUTPin1";
        private const string DutPin2 = "DUTPin2";

        // Number of samples to acquire and average per measurement.
        private const long SamplesToAcquire = 100;

        // Per-sample timeout in seconds.
        private const double SampleTimeout = 10.0;

        // Fetch timeout in seconds.
        private const double FetchTimeout = 10.0;

        // Debounce time for same-channel example (both start and stop must be equal).
        private const double SameChannelDebounceTime = 200.0e-9; // 200 ns

        // Debounce times for different-channel example (can be independent).
        private const double StartDebounceTime = 200.0e-9; // 200 ns
        private const double StopDebounceTime = 500.0e-9;  // 500 ns

        #endregion Settings and Configuration

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1. Initializing the digital pattern instrument session.");
                using (NIDigital session = new NIDigital(ResourceName, true, true, OptionString))
                {
                    Console.WriteLine("2. Loading the pin map file.");
                    session.LoadPinMap(PinMapFilePath);

                    Console.WriteLine("3. Configuring voltage levels.");
                    DigitalPinSet allPins = session.PinAndChannelMap.GetPinSet(string.Empty);
                    allPins.DigitalLevels.Vil = 0.0;
                    allPins.DigitalLevels.Vih = 5.0;
                    allPins.DigitalLevels.Vol = 0.5;
                    allPins.DigitalLevels.Voh = 2.4;
                    allPins.DigitalLevels.Vterm = 0.0;

                    // Discover available TMU contexts.
                    Console.WriteLine("4. Discovering available TMU contexts.");
                    List<string> disabledContexts = session.Tmu.GetDisabledTmuContexts();
                    Console.WriteLine("   Available TMU contexts (initially disabled): {0}", string.Join(", ", disabledContexts));

                    string tmuContext = disabledContexts.First();
                    Console.WriteLine("   Using TMU context: {0}", tmuContext);

                    DigitalTmu tmu = session.Tmu.GetTmu(tmuContext);

                    // === Example 1: Same Channel - Debounce Times Must Be Equal ===
                    // When the TMU start source and stop source are configured to the same channel,
                    // the start input debounce time and stop input debounce time MUST be set to the
                    // same value. This constraint exists because the same physical input path is used
                    // for both start and stop events when they share a channel.
                    Console.WriteLine();
                    Console.WriteLine("5. Example 1: Same channel debounce (equal times).");
                    tmu.Enabled = true;
                    tmu.ArmType = TmuArmType.Immediate;

                    // Configure start and stop to the SAME channel (period measurement).
                    tmu.Start.Source = DutPin1;
                    tmu.Start.SourceEvent = TmuSourceEvent.Vol;
                    tmu.Start.SourceEventPolarity = TmuPolarity.RisingEdge;

                    tmu.Stop.Source = DutPin1;
                    tmu.Stop.SourceEvent = TmuSourceEvent.Vol;
                    tmu.Stop.SourceEventPolarity = TmuPolarity.RisingEdge;

                    tmu.SamplesToAcquire = SamplesToAcquire;
                    tmu.SampleTimeout = SampleTimeout;

                    // Set debounce times for both start and stop to the SAME value.
                    // A debounce time of 200ns filters out glitches shorter than 200ns.
                    tmu.Start.InputDebounceTime = SameChannelDebounceTime;
                    tmu.Stop.InputDebounceTime = SameChannelDebounceTime;

                    Console.WriteLine("   Same channel ({0}): start debounce = {1} ns, stop debounce = {2} ns",
                        DutPin1, SameChannelDebounceTime * 1.0e9, SameChannelDebounceTime * 1.0e9);

                    Console.WriteLine("6. Initiating TMU period measurement with debounce.");
                    tmu.Initiate();

                    double measurement1 = tmu.FetchAveragedMeasurement(FetchTimeout);
                    Console.WriteLine("   Period measurement with debounce: {0:E6} seconds", measurement1);

                    tmu.Abort();

                    // === Example 2: Different Channels - Independent Debounce Times ===
                    // When the TMU start source and stop source are configured to different channels,
                    // the start and stop debounce times can be configured independently. Each channel
                    // has its own physical input path with its own debounce filter.
                    Console.WriteLine();
                    Console.WriteLine("7. Example 2: Different channels debounce (independent times).");
                    tmu.Enabled = true;
                    tmu.ArmType = TmuArmType.Immediate;

                    // Configure start and stop to DIFFERENT channels (skew measurement).
                    tmu.Start.Source = DutPin1;
                    tmu.Start.SourceEvent = TmuSourceEvent.Vol;
                    tmu.Start.SourceEventPolarity = TmuPolarity.RisingEdge;

                    tmu.Stop.Source = DutPin2;
                    tmu.Stop.SourceEvent = TmuSourceEvent.Vol;
                    tmu.Stop.SourceEventPolarity = TmuPolarity.RisingEdge;

                    tmu.SamplesToAcquire = SamplesToAcquire;
                    tmu.SampleTimeout = SampleTimeout;

                    // Set DIFFERENT debounce times for start and stop inputs. This is allowed
                    // because the start and stop sources are on different channels.
                    tmu.Start.InputDebounceTime = StartDebounceTime;
                    tmu.Stop.InputDebounceTime = StopDebounceTime;

                    Console.WriteLine("   Different channels: start ({0}) debounce = {1} ns, stop ({2}) debounce = {3} ns",
                        DutPin1, StartDebounceTime * 1.0e9, DutPin2, StopDebounceTime * 1.0e9);

                    Console.WriteLine("8. Initiating TMU skew measurement with independent debounce.");
                    tmu.Initiate();

                    double measurement2 = tmu.FetchAveragedMeasurement(FetchTimeout);
                    Console.WriteLine("   Skew measurement with independent debounce: {0:E6} seconds", measurement2);

                    tmu.Abort();

                    // Disconnect all channels using programmable, onboard switching.
                    Console.WriteLine();
                    Console.WriteLine("9. Cleaning up for session closure.");
                    session.PinAndChannelMap.GetPinSet(string.Empty).SelectedFunction = SelectedFunction.Disconnect;
                }
            }
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
    }
}
