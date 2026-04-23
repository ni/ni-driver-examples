namespace NationalInstruments.Examples.NIDigital.TdrMeasurement
{
    using Ivi.Driver;
    using ModularInstruments.NIDigital;
    using System;

    /// <summary>
    /// Demonstrates the use of the NI-Digital Pattern Driver API to create an instrument session and
    /// use Time-Domain Reflectometry (TDR) to find offsets on each physical channel defined in the
    /// pin group.s
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource name of your digital pattern instrument. The default
    ///    value for the resource name is "PXI1Slot2,PXI1Slot3".
    /// 3. Select whether to apply the TDR offsets to the instrument by setting the ApplyTdrOffsets field.
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

        // File to load.
        private const string PinMapFilePath = "PinMap.pinmap";

        // Pin group string used to create a DigitalPinSet for the TDR measurement.
        private const string PinGroupString = "PinGroup1";

        // Specifies whether to apply the offsets from the TDR measurement to the digital pattern instrument.
        private const bool ApplyTdrOffsets = false;

        // Configure the TDR Endpoint Termination. Valid options are TdrToOpenCircuit (default) and TdrToShortToGround
        private const TdrEndpointTermination TdrTermination = TdrEndpointTermination.TdrToOpenCircuit;

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

                    // Use the Tdr method to measure propagation delay in connectors and cabling for
                    // your system setup. Passing 'true' for the apply parameter automatically
                    // applies the measured offsets to the digital pattern instrument.
                    session.Timing.TdrEndpointTermination = TdrTermination;
                    Console.WriteLine(string.Format("3. Executing TDR{0}", ApplyTdrOffsets ? " and applying to instrument." : "."));
                    PrecisionTimeSpan[] offsets = session.PinAndChannelMap.GetPinSet(PinGroupString).Tdr(ApplyTdrOffsets);
                    PrintTdrOffsets(offsets);

                    // Disconnect all channels using programmable, onboard switching.
                    Console.WriteLine("4. Cleaning up for session closure.");
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

        static private void PrintTdrOffsets(PrecisionTimeSpan[] offsets)
        {
            if (offsets == null)
            {
                throw new ArgumentNullException("offsets");
            }

            Console.WriteLine();
            Console.WriteLine("TDR Offsets (s):");

            foreach (PrecisionTimeSpan offset in offsets)
            {
                Console.WriteLine(string.Format("\t{0}", offset.ToDecimal()));
            }

            Console.WriteLine();
        }
    }
}