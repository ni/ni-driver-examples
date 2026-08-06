namespace NationalInstruments.Examples.NIDigital.TmuMultiInstrument
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ModularInstruments.NIDigital;

    /// <summary>
    /// Demonstrates the use of the NI-Digital Pattern Driver API to work with TMU resources in a
    /// multi-instrument session. This example shows:
    ///   - TMU naming conventions: each device has its own set of TMUs, and in multi-instrument
    ///     sessions, TMU contexts are device-qualified (e.g. "PXI1Slot5/tmu0")
    ///   - How GetDisabledTmuContexts returns device-qualified TMU context strings so you can
    ///     identify which device each TMU belongs to
    ///   - Using TMU resources across multiple devices in the same session
    ///   - Each device has independent TMU hardware that can be configured and operated separately
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource names of your digital pattern instruments. The default
    ///    value for the resource name is "PXI1Slot5,PXI1Slot6".
    /// 3. Build and run the example.
    /// </remarks>
    public sealed class Program
    {
        #region Settings and Configuration

        // IVI resource names of your digital pattern instruments in MAX (comma-separated for multi-instrument).
        private const string ResourceName = "PXI1Slot5,PXI1Slot6";

        // Option String for initializing the session. Simulates the hardware.
        private const string OptionString = "Simulate=1,DriverSetup=Model:6571";

        // If using real instruments connected to a DUT generating signals on the channels
        // configured for the TMU measurements, pass an empty option string to disable simulation.
        //private const string OptionString = "";

        // Pin map file to load.
        private const string PinMapFilePath = "PinMap.pinmap";

        // Pin names mapped to different devices (must match pins in PinMap.pinmap).
        private const string DutPin1 = "DUTPin1"; // Connected to PXI1Slot5
        private const string DutPin3 = "DUTPin3"; // Connected to PXI1Slot6

        // Number of samples to acquire and average per measurement.
        private const long SamplesToAcquireDevice1 = 100;
        private const long SamplesToAcquireDevice2 = 100;

        // Per-sample timeout in seconds.
        private const double SampleTimeout = 10.0;

        // Fetch timeout in seconds.
        private const double FetchTimeout = 10.0;

        #endregion Settings and Configuration

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1. Initializing the multi-instrument digital pattern session.");
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

                    // Discover TMU contexts. In a multi-instrument session, each device
                    // contributes its own TMU resources with device-qualified names.
                    Console.WriteLine("4. Discovering TMU contexts in multi-instrument session.");
                    List<string> disabledContexts = session.Tmu.GetDisabledTmuContexts();
                    Console.WriteLine("   All TMU contexts (initially disabled): {0}", string.Join(", ", disabledContexts));

                    // Parse contexts to find one TMU per device. TMU contexts include a device
                    // prefix (e.g., "PXI1Slot5/tmu0") so we select contexts from different devices.
                    string tmuContext1 = disabledContexts.First();
                    string device1Prefix = tmuContext1.Substring(0, tmuContext1.IndexOf("/tmu", StringComparison.Ordinal));
                    string tmuContext2 = disabledContexts.First(c => !c.StartsWith(device1Prefix, StringComparison.Ordinal));

                    Console.WriteLine("   TMU context 1 (Device 1): {0}", tmuContext1);
                    Console.WriteLine("   TMU context 2 (Device 2): {0}", tmuContext2);

                    DigitalTmu tmu1 = session.Tmu.GetTmu(tmuContext1);
                    DigitalTmu tmu2 = session.Tmu.GetTmu(tmuContext2);

                    // === Device 1: Period measurement on DUTPin1 ===
                    // DUTPin1 is connected to the first device per the pin map.
                    Console.WriteLine();
                    Console.WriteLine("5. Configuring Device 1 ({0}): Period on {1}.", tmuContext1, DutPin1);
                    tmu1.Enabled = true;
                    tmu1.ArmType = TmuArmType.Immediate;

                    tmu1.Start.Source = DutPin1;
                    tmu1.Start.SourceEvent = TmuSourceEvent.Vol;
                    tmu1.Start.SourceEventPolarity = TmuPolarity.RisingEdge;

                    tmu1.Stop.Source = DutPin1;
                    tmu1.Stop.SourceEvent = TmuSourceEvent.Vol;
                    tmu1.Stop.SourceEventPolarity = TmuPolarity.RisingEdge;

                    tmu1.SamplesToAcquire = SamplesToAcquireDevice1;
                    tmu1.SampleTimeout = SampleTimeout;

                    // === Device 2: Period measurement on DUTPin3 ===
                    // DUTPin3 is connected to the second device per the pin map.
                    Console.WriteLine("6. Configuring Device 2 ({0}): Period on {1}.", tmuContext2, DutPin3);
                    tmu2.Enabled = true;
                    tmu2.ArmType = TmuArmType.Immediate;

                    tmu2.Start.Source = DutPin3;
                    tmu2.Start.SourceEvent = TmuSourceEvent.Vol;
                    tmu2.Start.SourceEventPolarity = TmuPolarity.RisingEdge;

                    tmu2.Stop.Source = DutPin3;
                    tmu2.Stop.SourceEvent = TmuSourceEvent.Vol;
                    tmu2.Stop.SourceEventPolarity = TmuPolarity.RisingEdge;

                    tmu2.SamplesToAcquire = SamplesToAcquireDevice2;
                    tmu2.SampleTimeout = SampleTimeout;

                    // This code snippet demonstrates that GetDisabledTmuContexts returns
                    // only the context strings for TMUs that have not been enabled. Because
                    // two TMUs have been enabled, one on each instrument, GetDisabledTmuContexts
                    // returns two fewer context strings.
                    Console.WriteLine();
                    Console.WriteLine("7. Checking disabled TMUs after configuration.");
                    List<string> remainingDisabled = session.Tmu.GetDisabledTmuContexts();
                    if (remainingDisabled.Count > 0)
                    {
                        Console.WriteLine("   Still disabled TMU contexts: {0}", string.Join(", ", remainingDisabled));
                    }
                    else
                    {
                        Console.WriteLine("   All TMUs are enabled.");
                    }

                    // Initiate and fetch measurements on both devices.
                    // Each device's TMU operates independently in parallel.
                    Console.WriteLine();
                    Console.WriteLine("8. Initiating measurements on both devices.");
                    tmu1.Initiate();
                    tmu2.Initiate();

                    double measurement1 = tmu1.FetchAveragedMeasurement(FetchTimeout);
                    Console.WriteLine("   {0} Period: {1:E6} seconds", tmuContext1, measurement1);

                    double measurement2 = tmu2.FetchAveragedMeasurement(FetchTimeout);
                    Console.WriteLine("   {0} Period: {1:E6} seconds", tmuContext2, measurement2);

                    tmu1.Abort();
                    tmu2.Abort();

                    // Disconnect all channels using programmable, onboard switching.
                    Console.WriteLine();
                    Console.WriteLine("9. Cleaning up.");
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
