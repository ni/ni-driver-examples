namespace NationalInstruments.Examples.NIFgen.FunctionGenerator
{
    using System;
    using ModularInstruments.NIFgen;

    /// <summary>
    /// Demonstrates the use of the NI-FGEN .NET API to generate a standard waveform.
    /// Note: Refer to the specifications document for your NI signal generator to determine whether the features and values used in this example are supported.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Set the channel(s) you would like to generate your waveform on.
    /// 3. Set the standard waveform you would like to generate.
    /// 4. Set the various standard waveform settings.
    /// 5. Build and run the example.
    /// </remarks>
    public sealed class Program
    {
        #region Settings and Configuration

        // IVI resource name of your signal generator in MAX.
        private const string ResourceName = "PXI1Slot2";

        // The name of the channel(s) to use.
        private const string ChannelName = "0";

        // The standard waveform the generate.
        private const StandardWaveform Waveform = StandardWaveform.Sine;

        // Settings for the standard waveform.
        private const double Amplitude = 1.0;
        private const double DCOffset = 0.0;
        private const double StartPhase = 0.0;
        private const double Frequency = 1000000;

        #endregion // Settings and Configuration

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1. Initializing the signal generator session.");
                using (NIFgen session = new NIFgen(ResourceName, false, true))
                {
                    // Set the output mode of the signal generator. Set this at the beginning of your 
                    // program to determine what the signal generator will output.
                    Console.WriteLine("2. Setting the signal generator's output mode to Function.");
                    session.Output.OutputMode = OutputMode.Function;

                    Console.WriteLine("3. Configuring the standard function:");
                    Console.WriteLine("\t - Generating a {0} wave.", Waveform);
                    Console.WriteLine("\t - Amplitude: {0} (Vp-p)", Amplitude);
                    Console.WriteLine("\t - DC Offset: {0} (V)", DCOffset);
                    Console.WriteLine("\t - Frequency: {0} (Hz)", Frequency);
                    Console.WriteLine("\t - Start Phase: {0}", StartPhase);
                    session.StandardWaveform.Configure(ChannelName, Waveform, Amplitude, DCOffset, Frequency, StartPhase);

                    // Note: Amplitude, DCOffset, and Frequency can be changed while the signal generator is generating.
                    Console.WriteLine("4. Initiating generation.");
                    session.InitiateGeneration();

                    // When the signal generator is in the Function output mode, the trigger mode
                    // must be continuous. This means that the function will continue to be generated
                    // until AbortGeneration() is called.
                    Console.WriteLine("5. Press any key to abort generation.");
                    Console.ReadKey();
                    session.AbortGeneration();
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
