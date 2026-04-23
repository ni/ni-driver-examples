namespace NationalInstruments.Examples.NIFgen.ArbitrarySequence
{
    using System;
    using ModularInstruments.NIFgen;
    using System.Collections.Generic;

    /// <summary>
    /// Demonstrates the use of the NI-FGEN .NET API to create and generate an arbitrary sequence.
    /// Note: Refer to the specifications document for your NI signal generator to determine whether the features and values used in this example are supported.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Create the waveforms you would like to generate in the sequence.
    /// 3. Set the LoopCounts and SampleCounts to determine how the sequence will generate each of
    ///    the waveforms.
    /// 4. Configure the sample clock settings, sequence gain and offset, and whether you would like
    ///    to apply any filters.
    /// 5. Build and run the example.
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
        private const string SampleClockSource = "OnboardClock";
        private const ClockMode SampleClockMode = ClockMode.DivideDown;

        // Define the waveform data to generate in the sequence.
        private static readonly short[] Waveform1 = { 0, 1, 2, 3, 4, 5, 6, 7 };
        private static readonly short[] Waveform2 = { 0, 10, 0, 10, 0, 10, 0, 10 };
        private static readonly short[] Waveform3 = { 0, 2, 4, 6, 6, 4, 2, 0 };
        private static readonly short[] Waveform4 = { 7, 6, 5, 4, 3, 2, 1, 0 };

        // Set the loop counts to determine how many times to generate each of
        // the four waveforms defined above.
        private static readonly int[] LoopCounts = { 1, 1, 3, 1 };

        // Set the sample counts to determine how many samples will be generated from
        // each of the four waveforms defined above. Each sample count must be a subset
        // of the number of samples defined in these waveforms.
        private static readonly int[] SampleCounts = { 4, 8, 8, 4 };

        // Set any markers to place in each of the four waveforms defined above.
        // Specify -1 for no marker.
        private static readonly int[] Markers = { -1, -1, -1, -1 };

        // Set the desired gain and offset to apply to the sequence.
        private const double Gain = 1.0;
        private const double Offset = 0.0;

        // Set whether to apply an anlog or digital filter
        // to the sequence.
        private const bool AnalogFilterEnabled = true;
        private const bool DigitalFilterEnabled = true;

        #endregion // Settings and Configuration

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1. Initializing the signal generator session.");
                using (NIFgen session = new NIFgen(ResourceName, false, true))
                {
                    // Setting the trigger mode to Single will cause the signal generator
                    // to only generate once.
                    session.Trigger.SetTriggerMode(ChannelName, TriggerMode.Single);

                    // Set the output mode of the signal generator. Set this at the beginning of your 
                    // program to determine what the signal generator will output.
                    Console.WriteLine("2. Setting the signal generator's output mode to Sequence.");
                    session.Output.OutputMode = OutputMode.Sequence;

                    Console.WriteLine("3. Configuring the sample clock to use:");
                    Console.WriteLine("\t - A sample rate of {0} samples per second.", SampleRate);
                    Console.WriteLine("\t - \"{0}\" as the sample clock source.", SampleClockSource);
                    Console.WriteLine("\t - A {0} sample clock mode.", SampleClockMode);

                    session.Arbitrary.SampleRate = SampleRate;
                    session.Timing.SampleClock.Source = SampleClockSource;
                    session.Timing.SampleClock.ClockMode = SampleClockMode;

                    // If you want to run this program in a loop and replace a particular waveform, you must call
                    // session.Arbitrary.Waveform.Clear(handle), specifying the handle of the waveform you would 
                    // like to clear before calling CreateChannelWaveform().
                    // Use -1 to clear all waveforms.
                    Console.WriteLine("4. Downloading waveforms to the Arbitrary Waveform Generator's memory.");
                    int[] waveformHandles = new int[4];
                    waveformHandles[0] = session.Arbitrary.Waveform.CreateChannelWaveform(ChannelName, Waveform1);
                    waveformHandles[1] = session.Arbitrary.Waveform.CreateChannelWaveform(ChannelName, Waveform2);
                    waveformHandles[2] = session.Arbitrary.Waveform.CreateChannelWaveform(ChannelName, Waveform3);
                    waveformHandles[3] = session.Arbitrary.Waveform.CreateChannelWaveform(ChannelName, Waveform4);

                    // If you want to run this program in a loop and replace a particular sequence, you must call
                    // session.Arbitrary.Sequence.Clear(handle) specifying the handle of the sequence you would 
                    // like to clear before calling Create().
                    // Use -1 to clear all sequences.
                    Console.WriteLine("5. Creating the arbitrary sequence.");
                    int[] coercedMarkers;
                    int sequenceHandle = session.Arbitrary.Sequence.Create(waveformHandles, LoopCounts, SampleCounts, Markers, out coercedMarkers);

                    Console.WriteLine("6. Configuring arbitrary sequence to have a gain of {0} and an offset of {1}.", Gain, Offset);
                    session.Arbitrary.Sequence.Configure(ChannelName, sequenceHandle, Gain, Offset);

                    Console.WriteLine("7. {0} the analog filter.", AnalogFilterEnabled ? "Enabling" : "Disabling");
                    session.Output.Filter.SetAnalogFilterEnabled(ChannelName, AnalogFilterEnabled);

                    Console.WriteLine("8. {0} the digital filter.", DigitalFilterEnabled ? "Enabling" : "Disabling");
                    session.Output.Filter.SetDigitalFilterEnabled(ChannelName, DigitalFilterEnabled);

                    Console.WriteLine("9. Enabling output on channel(s) {0}.", ChannelName);
                    session.Output.SetEnabled(ChannelName, true);

                    Console.WriteLine("10. Initiating generation.");
                    session.InitiateGeneration();

                    // Wait until the generation is complete. If the trigger mode is set
                    // to Continuous, then WaitUntilDone will time out.
                    try
                    {
                        session.WaitUntilDone(TimeSpan.FromSeconds(30));
                        Console.WriteLine("11. Generation complete.");
                    }
                    catch (TimeoutException)
                    {
                        session.AbortGeneration();
                        Console.WriteLine("11. Generation was aborted.");
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
