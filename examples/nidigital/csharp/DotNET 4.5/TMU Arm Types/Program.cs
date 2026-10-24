namespace NationalInstruments.Examples.NIDigital.TmuArmTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ModularInstruments.NIDigital;

    /// <summary>
    /// Demonstrates the difference between Immediate and Edge arm types for TMU measurements
    /// using the NI-Digital Pattern Driver API. This example shows:
    ///   - Immediate arm: the TMU begins looking for start/stop events as soon as Initiate()
    ///     is called. When start and stop events differ (e.g., duty cycle), the detection order
    ///     is not guaranteed on a free-running signal, which can produce negative measurements.
    ///   - Edge arm: the TMU waits for a specific arm event before looking for start/stop events,
    ///     guaranteeing deterministic event ordering and consistent (positive) measurements.
    ///   - The constraint that edge arm source/event/polarity must match either the start or
    ///     stop source configuration.
    ///
    /// Both parts measure the same quantity (duty cycle high) on the same channel so the
    /// behavioral difference between the two arm types is clearly visible.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource name of your digital pattern instrument. The default
    ///    value for the resource name is "PXI1Slot5".
    /// 3. Build and run the example.
    ///
    /// Note: The DUT must be generating signals on the configured channel for TMU measurements
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
        // configured for the TMU measurements, pass an empty option string to disable simulation.
        //private const string OptionString = "";

        // Pin map file to load.
        private const string PinMapFilePath = "PinMap.pinmap";

        // Pin name used for TMU measurements (must match a pin in PinMap.pinmap).
        private const string DutPin1 = "DUTPin1";

        // Number of samples to acquire and average per measurement.
        private const long SamplesToAcquire = 100;

        // Per-sample timeout in seconds.
        private const double SampleTimeout = 10.0;

        // Fetch timeout in seconds.
        private const double FetchTimeout = 10.0;

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

                    // Discover available TMU contexts. We need two TMUs: one for Immediate
                    // arm and one for Edge arm.
                    Console.WriteLine("4. Discovering available TMU contexts.");
                    List<string> disabledContexts = session.Tmu.GetDisabledTmuContexts();
                    Console.WriteLine("   Available TMU contexts: {0}", string.Join(", ", disabledContexts));

                    if (disabledContexts.Count < 2)
                    {
                        Console.WriteLine("   ERROR: This example requires at least 2 TMU contexts.");
                        return;
                    }

                    // TMU0 will be used for the Immediate arm measurement.
                    // TMU1 will be used for the Edge arm measurement.
                    string tmuContext0 = disabledContexts[0];
                    string tmuContext1 = disabledContexts[1];
                    Console.WriteLine("   TMU0 (Immediate arm): {0}", tmuContext0);
                    Console.WriteLine("   TMU1 (Edge arm): {0}", tmuContext1);

                    DigitalTmu tmu0 = session.Tmu.GetTmu(tmuContext0);
                    DigitalTmu tmu1 = session.Tmu.GetTmu(tmuContext1);

                    // Enable both TMUs.
                    tmu0.Enabled = true;
                    tmu1.Enabled = true;

                    // Configure sample count and timeout for both TMUs.
                    tmu0.SamplesToAcquire = SamplesToAcquire;
                    tmu0.SampleTimeout = SampleTimeout;
                    tmu1.SamplesToAcquire = SamplesToAcquire;
                    tmu1.SampleTimeout = SampleTimeout;

                    // =================================================================
                    // Part 1: Duty Cycle High with Immediate Arm (TMU0)
                    // =================================================================
                    // With Immediate arm, the TMU begins looking for start and stop events
                    // as soon as Initiate() is called. Because the start event (VOH Rising
                    // Edge) and stop event (VOH Falling Edge) are different, the first event
                    // detected after initiation could be either one.
                    //
                    // If the stop event (Falling Edge) happens to be detected first, the
                    // measurement result will be NEGATIVE, indicating the stop timestamp
                    // preceded the start timestamp. This is the fundamental ambiguity of
                    // Immediate arm with non-identical start/stop events on a free-running
                    // periodic signal.
                    Console.WriteLine();
                    Console.WriteLine("=== Part 1: Duty Cycle High with IMMEDIATE Arm (TMU0) ===");

                    // Configure Immediate arm type on TMU0.
                    tmu0.ArmType = TmuArmType.Immediate;

                    // Start source: DUTPin1, VOH threshold, Rising Edge
                    tmu0.Start.Source = DutPin1;
                    tmu0.Start.SourceEvent = TmuSourceEvent.Voh;
                    tmu0.Start.SourceEventPolarity = TmuPolarity.RisingEdge;

                    // Stop source: same channel, VOH threshold, Falling Edge
                    tmu0.Stop.Source = DutPin1;
                    tmu0.Stop.SourceEvent = TmuSourceEvent.Voh;
                    tmu0.Stop.SourceEventPolarity = TmuPolarity.FallingEdge;

                    // =================================================================
                    // Part 2: Duty Cycle High with Edge Arm (TMU1)
                    // =================================================================
                    // With Edge arm, the TMU waits for a specific arm event before it begins
                    // looking for start/stop events. This eliminates the ambiguity of
                    // Immediate arm by establishing a known starting point in the signal cycle.
                    //
                    // Here we configure the arm to match the start source (DUTPin1, VOH,
                    // Rising Edge). The sequence is:
                    //   1. TMU waits for arm event (Rising Edge at VOH on DUTPin1)
                    //   2. Arm event is captured as the start event
                    //   3. TMU then waits for the stop event (Falling Edge at VOH)
                    //   4. Measurement = stop timestamp - start timestamp (always positive)
                    //
                    // IMPORTANT CONSTRAINT: For the PXIe-6570 and PXIe-6571, the edge arm source,
                    // event, and polarity must match those of either the start or stop source
                    // configuration. If they do not match, the driver returns an error.
                    Console.WriteLine();
                    Console.WriteLine("=== Part 2: Duty Cycle High with EDGE Arm (TMU1) ===");

                    // Configure Edge arm type on TMU1.
                    tmu1.ArmType = TmuArmType.Edge;

                    // Start source: same duty cycle configuration as Part 1.
                    tmu1.Start.Source = DutPin1;
                    tmu1.Start.SourceEvent = TmuSourceEvent.Voh;
                    tmu1.Start.SourceEventPolarity = TmuPolarity.RisingEdge;

                    // Stop source: same duty cycle configuration as Part 1.
                    tmu1.Stop.Source = DutPin1;
                    tmu1.Stop.SourceEvent = TmuSourceEvent.Voh;
                    tmu1.Stop.SourceEventPolarity = TmuPolarity.FallingEdge;

                    // Configure the edge arm to match the start source. This guarantees
                    // the rising edge (start) is always captured before the falling edge
                    // (stop), producing a consistent positive measurement.
                    tmu1.EdgeArm.Source = DutPin1;
                    tmu1.EdgeArm.SourceEvent = TmuSourceEvent.Voh;
                    tmu1.EdgeArm.Polarity = TmuPolarity.RisingEdge;

                    // =================================================================
                    // Initiate both TMUs back-to-back
                    // =================================================================
                    // By initiating both TMUs consecutively, they begin acquiring
                    // measurements on the same signal nearly simultaneously.
                    Console.WriteLine();
                    Console.WriteLine("5. Initiating both TMUs.");
                    tmu0.Initiate();
                    tmu1.Initiate();

                    // Fetch results from both TMUs.
                    double immediateMeasurement = tmu0.FetchAveragedMeasurement(FetchTimeout);
                    double edgeMeasurement = tmu1.FetchAveragedMeasurement(FetchTimeout);

                    // The Immediate arm result may be positive or negative. A negative
                    // value means the falling edge (stop) was detected before the rising
                    // edge (start). This is expected with Immediate arm on a free-running
                    // signal when start and stop events differ.
                    Console.WriteLine();
                    Console.WriteLine("   Immediate Arm (TMU0) - Duty Cycle High: {0:E6} seconds", immediateMeasurement);

                    // With Edge arm, the result is always positive because the arm event
                    // guarantees the start event is captured before the stop event.
                    Console.WriteLine("   Edge Arm (TMU1) - Duty Cycle High: {0:E6} seconds", edgeMeasurement);

                    // Abort both TMUs.
                    Console.WriteLine();
                    Console.WriteLine("6. Aborting TMUs and cleaning up.");
                    tmu0.Abort();
                    tmu1.Abort();

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
