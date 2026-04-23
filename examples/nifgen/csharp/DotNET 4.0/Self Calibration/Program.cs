namespace NationalInstruments.Examples.NIFgen.CreateWaveformDoubleFromFile
{
    using System;
    using NationalInstruments.ModularInstruments.NIFgen;

    /// <summary>
    /// Demonstrates the use of the NI-FGEN .NET API to perform self-calibration on the signal generator.
    /// Note: Refer to the specifications document for your NI signal generator to determine whether the features and values used in this example are supported.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Familiarize yourself with the settings in the Settings and Configuration code region.
    /// 2. Build and run the example.
    /// </remarks>
    public sealed class Program
    {
        #region Settings and Configuration

        // IVI resource name of your signal generator in MAX.
        private const string ResourceName = "PXI1Slot2";

        #endregion // Settings and Configuration

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1. Initializing the signal generator session.");
                using (NIFgen session = new NIFgen(ResourceName, false, true))
                {
                    if (session.Utility.IsSelfCalibrationSupported)
                    {
                        Console.WriteLine("2. Last self calibration results:");
                        Console.WriteLine("\t Time: {0}", session.Utility.LastSelfCalibrationDateTime.ToString("MM/dd/yyyy hh:mm:ss"));
                        Console.WriteLine("\t Temperature: {0} Celsius.", session.Utility.LastSelfCalibrationTemperature);

                        Console.WriteLine("3. Starting self calibration.");
                        session.Utility.SelfCalibrate();
                        Console.WriteLine("4. Self calibration complete.");

                        Console.WriteLine("5. New self calibration results:");
                        Console.WriteLine("\t Time: {0}", session.Utility.LastSelfCalibrationDateTime.ToString("MM/dd/yyyy hh:mm:ss"));
                        Console.WriteLine("\t Temperature: {0} Celsius.", session.Utility.LastSelfCalibrationTemperature);
                    }
                    else
                    {
                        Console.WriteLine("2. Self calibration is not supported by the signal generator in use.");
                    }
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
    }
}