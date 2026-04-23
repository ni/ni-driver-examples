namespace NationalInstruments.Examples.NIFgen.ArbitraryWaveformStreaming
{
    using System;
    using ModularInstruments.NIFgen;

    /// <summary>
    /// Demonstrates the use of the NI-FGEN .NET API to stream arbitrary waveform data.
    /// Note: Refer to the specifications document for your NI signal generator to determine whether the features and values used in this example are supported.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Set the channel(s) you would like to generate your waveform(s) on.
    /// 3. Set the waveform size to allocate.
    /// 4. Set how many blocks you would like the waveform to be written in.
    /// 5. Set the number of times to repeat the generation of the waveform.
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
        private const double SampleRate = 2000000;

        // The size of the waveform and how many blocks the waveform will be broken up into.
        private const int WaveformSize = 10485760;
        private const int BlockCount = 10;

        // The number of times to repeat the generation of the waveform.
        private const int RepeatCount = 5;

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
                    Console.WriteLine("2. Setting the signal generator's output mode to Arbitrary.");
                    session.Output.OutputMode = OutputMode.Arbitrary;

                    Console.WriteLine("3. Configuring the sample rate to {0} samples per second.", SampleRate);
                    session.Arbitrary.SampleRate = SampleRate;

                    Console.WriteLine("4. Allocating enough space for {0} samples on the Arbitrary Waveform Generator's onboard memory.", WaveformSize);
                    int handle = session.Arbitrary.Waveform.Allocate(ChannelName, WaveformSize);

                    Console.WriteLine("5. Setting the streaming handle to the previously allocated waveform.");
                    session.Arbitrary.Streaming.WaveformHandle = handle;

                    Console.WriteLine("6. Configuring the trigger mode to Single.");
                    session.Trigger.SetTriggerMode(ChannelName, TriggerMode.Single);

                    Console.WriteLine("7. Setting the repeat count to {0}.", RepeatCount);
                    session.Arbitrary.Waveform.RepeatCount = RepeatCount;

                    // Calculate the size of the waveform needed to write the number of samples in N blocks.
                    // Replace this code with the desired intial waveform to use.
                    int waveformBlockSize = (int)Math.Floor((double)(WaveformSize / BlockCount));
                    short[] waveform = new short[waveformBlockSize];

                    // To change where the waveform data will begin being written in the allocated memory,
                    // use session.Arbitrary.Waveform.SetNextWritePosition.
                    Console.WriteLine("8. Filling the streaming waveform with the initial data before streaming.");
                    for (int i = 0; i < BlockCount; i++)
                    {
                        session.Arbitrary.Waveform.Write(ChannelName, handle, waveform);
                    }

                    Console.WriteLine("9. Initiating generation.");
                    session.InitiateGeneration();

                    Console.WriteLine("10. Streaming new waveform data.");
                    for (int i = 0; i < BlockCount * (RepeatCount - 1); i++)
                    {
                        // Replace the waveform here with the desired waveform(s) to stream.
                        session.Arbitrary.Waveform.Write(ChannelName, handle, waveform);
                    }

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
