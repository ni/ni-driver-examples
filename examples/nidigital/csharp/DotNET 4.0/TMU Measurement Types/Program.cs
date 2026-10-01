namespace NationalInstruments.Examples.NIDigital.TmuMeasurementTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ModularInstruments.NIDigital;

    /// <summary>
    /// Demonstrates how to perform common TMU measurement types using the NI-Digital Pattern
    /// Driver API. This example shows three measurement configurations:
    ///   - Period: time between two consecutive identical events on the same channel
    ///     (rising-edge to rising-edge)
    ///   - Duty Cycle High: time the signal spends above a threshold on a single channel
    ///     (rising-edge to falling-edge)
    ///   - Skew: time difference between the same event on two different channels
    ///     (inter-channel timing)
    ///
    /// The TMU is not limited to these measurement configurations.
    ///
    /// Each measurement uses Edge arm to guarantee deterministic results. To use Immediate arm
    /// instead, remove the edge arm configuration and change the arm type to TmuArmType.Immediate.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource name of your digital pattern instrument. The default
    ///    value for the resource name is "PXI1Slot5".
    /// 3. Build and run the example.
    ///
    /// Note: The DUT must be generating signals on the configured channels for TMU measurements
    /// to complete. In simulate mode, the driver returns simulated measurement values.
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
                    Console.WriteLine("   Available TMU contexts: {0}", string.Join(", ", disabledContexts));

                    string tmuContext = disabledContexts.First();
                    Console.WriteLine("   Using TMU context: {0}", tmuContext);

                    DigitalTmu tmu = session.Tmu.GetTmu(tmuContext);

                    // Enable the TMU.
                    tmu.Enabled = true;

                    // Configure sample count and timeout (shared by all measurements).
                    tmu.SamplesToAcquire = SamplesToAcquire;
                    tmu.SampleTimeout = SampleTimeout;

                    // =================================================================
                    // Measurement 1: Period
                    // =================================================================
                    // Measures the time between two consecutive identical events on the
                    // same channel. Both start and stop sources are configured to the same
                    // channel with the same event and polarity.
                    //
                    // Configuration:
                    //   Start: DUTPin1, VOL threshold, Rising Edge
                    //   Stop:  DUTPin1, VOL threshold, Rising Edge (same as start)
                    //   Arm:   matches start/stop (Rising Edge at VOL on DUTPin1)
                    //
                    // The TMU captures the first rising-edge VOL crossing as the start,
                    // then the next one as the stop. The difference is one full period.
                    Console.WriteLine();
                    Console.WriteLine("5. Performing measurement 1 (Period).");

                    // Configure Edge arm.
                    tmu.ArmType = TmuArmType.Edge;

                    // Start source: DUTPin1, VOL threshold, Rising Edge
                    tmu.Start.Source = DutPin1;
                    tmu.Start.SourceEvent = TmuSourceEvent.Vol;
                    tmu.Start.SourceEventPolarity = TmuPolarity.RisingEdge;

                    // Stop source: same channel, same event/polarity for Period.
                    tmu.Stop.Source = DutPin1;
                    tmu.Stop.SourceEvent = TmuSourceEvent.Vol;
                    tmu.Stop.SourceEventPolarity = TmuPolarity.RisingEdge;

                    // Edge arm matches start/stop.
                    tmu.EdgeArm.Source = DutPin1;
                    tmu.EdgeArm.SourceEvent = TmuSourceEvent.Vol;
                    tmu.EdgeArm.Polarity = TmuPolarity.RisingEdge;

                    // Initiate and fetch.
                    tmu.Initiate();

                    double periodMeasurement = tmu.FetchAveragedMeasurement(FetchTimeout);
                    Console.WriteLine("   Period: {0:E6} seconds", periodMeasurement);

                    tmu.Abort();

                    // =================================================================
                    // Measurement 2: Duty Cycle High
                    // =================================================================
                    // Measures the high portion of a duty cycle on a single channel. The
                    // start event is the signal rising above a threshold, and the stop
                    // event is the signal falling below the same threshold.
                    //
                    // Configuration:
                    //   Start: DUTPin1, VOH threshold, Rising Edge
                    //   Stop:  DUTPin1, VOH threshold, Falling Edge
                    //   Arm:   matches start (Rising Edge at VOH on DUTPin1)
                    //
                    // The arm event guarantees the rising edge is captured first, so
                    // the measurement is always the time the signal spends above VOH.
                    Console.WriteLine();
                    Console.WriteLine("6. Performing measurement 2 (Duty Cycle High).");

                    // Start source: DUTPin1, VOH threshold, Rising Edge
                    tmu.Start.Source = DutPin1;
                    tmu.Start.SourceEvent = TmuSourceEvent.Voh;
                    tmu.Start.SourceEventPolarity = TmuPolarity.RisingEdge;

                    // Stop source: same channel, VOH threshold, Falling Edge.
                    tmu.Stop.Source = DutPin1;
                    tmu.Stop.SourceEvent = TmuSourceEvent.Voh;
                    tmu.Stop.SourceEventPolarity = TmuPolarity.FallingEdge;

                    // Edge arm matches start source.
                    tmu.EdgeArm.Source = DutPin1;
                    tmu.EdgeArm.SourceEvent = TmuSourceEvent.Voh;
                    tmu.EdgeArm.Polarity = TmuPolarity.RisingEdge;

                    // Initiate and fetch.
                    tmu.Initiate();

                    double dutyCycleMeasurement = tmu.FetchAveragedMeasurement(FetchTimeout);
                    Console.WriteLine("   Duty Cycle High: {0:E6} seconds", dutyCycleMeasurement);

                    tmu.Abort();

                    // =================================================================
                    // Measurement 3: Skew
                    // =================================================================
                    // Measures the time difference between the same event on two different
                    // channels. The start source is on one channel and the stop source is
                    // on a different channel. The result represents the inter-channel
                    // timing skew.
                    //
                    // Configuration:
                    //   Start: DUTPin1, VOL threshold, Rising Edge
                    //   Stop:  DUTPin2, VOL threshold, Rising Edge
                    //   Arm:   matches start (Rising Edge at VOL on DUTPin1)
                    //
                    // The arm event on DUTPin1 guarantees the start event on DUTPin1 is
                    // captured first, then the TMU waits for the corresponding event on
                    // DUTPin2 as the stop.
                    Console.WriteLine();
                    Console.WriteLine("7. Performing measurement 3 (Skew).");

                    // Start source: DUTPin1, VOL threshold, Rising Edge
                    tmu.Start.Source = DutPin1;
                    tmu.Start.SourceEvent = TmuSourceEvent.Vol;
                    tmu.Start.SourceEventPolarity = TmuPolarity.RisingEdge;

                    // Stop source: DUTPin2 (different channel), VOL threshold, Rising Edge.
                    tmu.Stop.Source = DutPin2;
                    tmu.Stop.SourceEvent = TmuSourceEvent.Vol;
                    tmu.Stop.SourceEventPolarity = TmuPolarity.RisingEdge;

                    // Edge arm matches start source.
                    tmu.EdgeArm.Source = DutPin1;
                    tmu.EdgeArm.SourceEvent = TmuSourceEvent.Vol;
                    tmu.EdgeArm.Polarity = TmuPolarity.RisingEdge;

                    // Initiate and fetch.
                    tmu.Initiate();

                    double skewMeasurement = tmu.FetchAveragedMeasurement(FetchTimeout);
                    Console.WriteLine("   Skew: {0:E6} seconds", skewMeasurement);

                    tmu.Abort();

                    // Disconnect all channels using programmable, onboard switching.
                    Console.WriteLine();
                    Console.WriteLine("8. Cleaning up.");
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
