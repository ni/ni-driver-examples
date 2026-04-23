namespace NationalInstruments.Examples.NIDigital.SelfCalibrationAndSelfTest
{
    using System;
    using Ivi.Driver;
    using ModularInstruments.NIDigital;

    /// <summary>
    /// Demonstrates the use of the NI-Digital Pattern Driver API to self-test and self-calibrate one
    /// or many digital pattern instruments.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Set the ResourceName field to the resource name(s) of your digital pattern instrument(s).
    /// 2. Build and run.
    /// </remarks>
    public sealed class Program
    {
        // IVI resource name of your digital pattern instrument in MAX. Multiple instruments may be added as a comma-separated list.
        private const string ResourceName = "PXI1Slot2,PXI1Slot3";

        // Option String for initializing the session. Simulates the hardware.
        private const string OptionString = "Simulate=1,DriverSetup=Model:6570";

        // Pass an empty OptionString to run on hardware (disable simulation)
        //private const string OptionString = "";

        static void Main(string[] args)
        {
            
                try
                {
                    Console.WriteLine(string.Format("1. Initializing the digital pattern instrument session for {0}.", ResourceName));
                    using (NIDigital session = new NIDigital(ResourceName, true, true, OptionString))
                    {
                        Console.WriteLine("2. Performing self-test.");
                        SelfTestResult results = session.Utility.SelfTest();
                        PrintSelfTestResults(results);

                        Console.WriteLine("3. Performing self-calibrate.");
                        session.SelfCalibrate();
                    }
                }
                // Handle driver-specific exceptions before the general exception handler that follows.
                // An example of a driver-specific exception is Ivi.Driver.IviCDriverException.
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine();
                }
            

            Console.WriteLine("Program complete. Press any key to close.");
            Console.ReadKey();
        }

        private static void PrintSelfTestResults(SelfTestResult results)
        {
            Console.WriteLine();
            Console.WriteLine(string.Format("\t{0}: {1}", results.Code, results.Message));
            Console.WriteLine();
        }
    }
}
