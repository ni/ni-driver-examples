namespace NationalInstruments.Examples.NIDigital.TmuGettingStarted
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ModularInstruments.NIDigital;

    /// <summary>
    /// Demonstrates the minimal workflow to configure and perform a Time Measurement Unit (TMU)
    /// measurement using the NI-Digital Pattern Driver API. This example walks through every
    /// required step:
    ///   1. Initialize the instrument session
    ///   2. Load a pin map
    ///   3. Configure voltage levels (VOL/VOH define TMU thresholds)
    ///   4. Discover available TMU contexts
    ///   5. Enable the TMU
    ///   6. Configure a period measurement with Immediate arm
    ///   7. Initiate the TMU acquisition
    ///   8. Fetch the averaged measurement
    ///   9. Abort the TMU and clean up
    ///
    /// This is the simplest possible TMU example. For arm type comparison, multi-measurement
    /// workflows, debounce filtering, or multi-instrument sessions, see the other TMU examples.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource name of your digital pattern instrument. The default
    ///    value for the resource name is "PXI1Slot5".
    /// 3. Build and run the example.
    ///
    /// Note: The DUT must be generating signals on the configured channel for the TMU measurement
    /// to complete. In simulate mode, the driver returns simulated measurement values.
    /// </remarks>
    public sealed class Program
    {
        #region Settings and Configuration

        // IVI resource name of your digital pattern instrument in MAX.
        private const string ResourceName = "PXI1Slot5";

        // Option String for initializing the session. Simulates the hardware.
        private const string OptionString = "Simulate=1,DriverSetup=Model:6571";

        // If using a real instrument connected to a DUT generating a signal on the channel
        // configured for the TMU measurement, pass an empty option string to disable simulation.
        //private const string OptionString = "";

        // Pin map file to load.
        private const string PinMapFilePath = "PinMap.pinmap";

        // Pin name used for TMU measurement (must match a pin in PinMap.pinmap).
        private const string DutPin1 = "DUTPin1";

        // Number of samples to acquire and average per measurement.
        private const long SamplesToAcquire = 100;

        // Per-sample timeout in seconds. If a single sample is not acquired within this
        // time, the measurement fails.
        private const double SampleTimeout = 10.0;

        // Fetch timeout in seconds.
        private const double FetchTimeout = 10.0;

        #endregion Settings and Configuration

        private static void Main(string[] args)
        {
            try
            {
                // Step 1: Initialize the digital pattern instrument session. The reset
                // parameter is true to ensure the instrument starts in a known state.
                Console.WriteLine("1. Initializing the digital pattern instrument session.");
                using (NIDigital session = new NIDigital(ResourceName, true, true, OptionString))
                {
                    // Step 2: Load the pin map. This allows referencing pin names (e.g.,
                    // "DUTPin1") instead of raw channel numbers.
                    Console.WriteLine("2. Loading the pin map file.");
                    session.LoadPinMap(PinMapFilePath);

                    // Step 3: Configure voltage levels for all channels. The VOL and VOH
                    // levels define the thresholds used by the TMU for SOURCE_EVENT_VOL
                    // and SOURCE_EVENT_VOH events respectively.
                    Console.WriteLine("3. Configuring voltage levels.");
                    DigitalPinSet allPins = session.PinAndChannelMap.GetPinSet(string.Empty);
                    allPins.DigitalLevels.Vil = 0.0;
                    allPins.DigitalLevels.Vih = 5.0;
                    allPins.DigitalLevels.Vol = 0.5;
                    allPins.DigitalLevels.Voh = 2.4;
                    allPins.DigitalLevels.Vterm = 0.0;

                    // Step 4: Discover available TMU contexts. After initialization, all
                    // TMUs are disabled. The context string identifies which TMU resource
                    // to use (e.g., "PXI1Slot5/tmu0").
                    Console.WriteLine("4. Discovering available TMU contexts.");
                    List<string> disabledContexts = session.Tmu.GetDisabledTmuContexts();
                    Console.WriteLine("   Available TMU contexts (initially disabled): {0}", string.Join(", ", disabledContexts));

                    string tmuContext = disabledContexts.First();
                    Console.WriteLine("   Using TMU context: {0}", tmuContext);

                    DigitalTmu tmu = session.Tmu.GetTmu(tmuContext);

                    // Step 5: Enable the TMU. TMUs are disabled by default and must be
                    // explicitly enabled before use.
                    Console.WriteLine("5. Enabling the TMU.");
                    tmu.Enabled = true;

                    // Step 6: Configure a period measurement with Immediate arm.
                    //
                    // Arm Type: Immediate means the TMU begins looking for start/stop
                    // events as soon as Initiate() is called. No explicit arm event is required.
                    //
                    // Period measurement: Both start and stop sources are set to the same
                    // channel with the same event and polarity. This measures the time between
                    // two consecutive rising-edge VOL threshold crossings, which is the signal
                    // period.
                    Console.WriteLine("6. Configuring period measurement with Immediate arm.");
                    tmu.ArmType = TmuArmType.Immediate;

                    // Start source: DUTPin1, VOL threshold, Rising Edge
                    tmu.Start.Source = DutPin1;
                    tmu.Start.SourceEvent = TmuSourceEvent.Vol;
                    tmu.Start.SourceEventPolarity = TmuPolarity.RisingEdge;

                    // Stop source: same channel, same event, same polarity (period)
                    tmu.Stop.Source = DutPin1;
                    tmu.Stop.SourceEvent = TmuSourceEvent.Vol;
                    tmu.Stop.SourceEventPolarity = TmuPolarity.RisingEdge;

                    // Configure the number of samples to acquire and average. A higher sample
                    // count produces a more stable averaged measurement but takes longer.
                    tmu.SamplesToAcquire = SamplesToAcquire;
                    tmu.SampleTimeout = SampleTimeout;

                    // Step 7: Initiate the TMU acquisition (non-blocking). The TMU begins
                    // looking for start/stop events immediately.
                    Console.WriteLine("7. Initiating TMU acquisition.");
                    tmu.Initiate();

                    // Step 8: Fetch the averaged measurement. This blocks until all configured
                    // samples have been acquired and averaged, or the timeout elapses. The
                    // result is in seconds.
                    Console.WriteLine("8. Fetching the averaged measurement.");
                    double periodMeasurement = tmu.FetchAveragedMeasurement(FetchTimeout);
                    Console.WriteLine("   Period measurement: {0:E6} seconds", periodMeasurement);

                    // Step 9: Abort the TMU acquisition and clean up.
                    Console.WriteLine("9. Aborting TMU and cleaning up.");
                    tmu.Abort();

                    // Disconnect all channels using programmable, onboard switching.
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
