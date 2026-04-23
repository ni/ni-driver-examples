namespace NationalInstruments.Examples.NIFgen.ArbitraryWaveform
{
    using System;
    using ModularInstruments.NIFgen;

    /// <summary>
    /// Demonstrates the use of the NI-FGEN .NET API to generate an arbitrary waveform.
    /// Note: Refer to the specifications document for your NI signal generator to determine whether the features and values used in this example are supported.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Set the channel(s) you would like to generate your waveform on.
    /// 3. Set the sample clock settings.
    /// 4. Set the data for the waveforms you would like to generate.
    /// 6. Set the gain for the waveform to generate.
    /// 7. Build and run the example.
    /// </remarks>
    public sealed class Program
    {
        #region Settings and Configuration

        // IVI resource name of your signal generator in MAX.
        private const string ResourceName = "PXI1Slot2";

        // The name of the channel(s) to use.
        private const string ChannelName = "0";

        // Sample clock settings.
        private const double SampleRate = 20000000;
        private const ClockMode SampleClockMode = ClockMode.DivideDown;

        // Waveform data.
        private static readonly short[] Waveform = { 0, 1, 2, 3 };

        // The gain used when generating the waveform.
        private const double Gain = 0.5;

        #endregion // Settings and Configuration

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1. Initializing the signal generator session.");
                using (NIFgen session = new NIFgen(ResourceName, ChannelName, true, ""))
                {
                    // Setting the trigger mode to Single will cause the signal generator
                    // to only generate once.
                    session.Trigger.SetTriggerMode(ChannelName, TriggerMode.Single);

                    // Set the output mode of the signal generator. Set this at the beginning of your 
                    // program to determine what the signal generator will output.
                    Console.WriteLine("2. Setting the signal generator's output mode to Arbitrary.");
                    session.Output.OutputMode = OutputMode.Arbitrary;

                    // Set the sample rate and sample clock mode. The actual sample rate will depend
                    // on the sample clock mode.
                    Console.WriteLine("3. Configuring the sample clock mode to \"{0}\" and the sample rate to {1} samples per second.", SampleClockMode, SampleRate);
                    session.Arbitrary.SampleRate = SampleRate;
                    session.Timing.SampleClock.ClockMode = SampleClockMode;

                    Console.WriteLine("4. Enabling output on channel(s) {0}.", ChannelName);
                    session.Output.SetEnabled(ChannelName, true);

                    Console.WriteLine("5. Allocating memory up front for the waveform to generate.");
                    int waveformHandle = session.Arbitrary.Waveform.Allocate(ChannelName, Waveform.Length);

                    Console.WriteLine("6. Downloading the waveform(s) to the Arbitrary Waveform Generator's memory for the channel(s) specified.");
                    session.Arbitrary.Waveform.Write(ChannelName, waveformHandle, Waveform);

                    // Gain can be set separately for each channel used.
                    // Additionally, if the trigger mode is set to Continuous, the gain can be
                    // changed on the fly after InitiateGeneration() has been called.
                    Console.WriteLine("7. Setting the gain for the channel(s).");
                    session.Arbitrary.SetGain(ChannelName, Gain);

                    Console.WriteLine("8. Initiating generation.");
                    session.InitiateGeneration();

                    // Wait until the generation is complete. If the trigger mode is set
                    // to Continuous, then WaitUntilDone will time out.
                    try
                    {
                        session.WaitUntilDone(TimeSpan.FromSeconds(30));
                        Console.WriteLine("9. Generation complete.");
                    }
                    catch (TimeoutException)
                    {
                        session.AbortGeneration();
                        Console.WriteLine("9. Generation was aborted.");
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
