namespace NationalInstruments.Examples.NIFgen.Trigger
{
    using System;
    using ModularInstruments.NIFgen;

    /// <summary>
    /// Demonstrates the use of the NI-FGEN .NET API to generate an arbitrary sequence using various
    /// start trigger set ups.
    /// Note: Refer to the specifications document for your NI signal generator to determine whether the features and values used in this example are supported.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Set the channel(s) you would like to generate your waveform on.
    /// 3. Set the trigger mode and the trigger type you would like to use.
    /// 4. Replace the waveform data with the waveforms you would like to generate.
    /// 5. Build and run the example.
    /// </remarks>
    public sealed class Program
    {
        #region Settings and Configuration

        // IVI resource name of your signal generator in MAX.
        private const string ResourceName = "PXI1Slot2";

        // The name of the channel(s) to use.
        private const string ChannelName = "0";

        // Signal generator sample rate.
        private const double SampleRate = 2.0e7;

        // Trigger settings.
        private readonly static TriggerMode StartTriggerMode = TriggerMode.Burst;
        private readonly static TriggerType StartTriggerType = TriggerType.SoftwareEdge;

        // Settings for a digital edge start trigger.
        private const string Source = "PFI0";
        private const DigitalEdge StartTriggerEdge = DigitalEdge.Rising;

        // Waveform data. Replace the waveform data with your own data.
        private static readonly double[] Waveform1 = { 0.0, 0.1, 0.2, 0.3, 0.4, 0.3, 0.2, 0.1 };
        private static readonly double[] Waveform2 = { -1.0, -0.5, 0.0, 0.5, 1.0, 0.5, 0.0, -0.5 };
        private static readonly double[] Waveform3 = { -0.5, 0.5, -0.5, 0.5, -0.5, 0.5, -0.5, 0.5 };

        // The number of times to repeat each waveform in the sequence.
        private static readonly int[] LoopCounts = { 3, 2, 1 };

        // The gain and offset to apply to the arbitrary sequence.
        public const double Gain = 1.0;
        public const double Offset = 0.0;

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
                    Console.WriteLine("2. Setting the signal generator's output mode to Sequence.");
                    session.Output.OutputMode = OutputMode.Sequence;

                    Console.WriteLine("3. Setting the sample rate to {0} Hz.", SampleRate);
                    session.Arbitrary.SampleRate = SampleRate;

                    Console.WriteLine("4. Setting the trigger mode to {0}.", StartTriggerMode);
                    session.Trigger.SetTriggerMode(ChannelName, StartTriggerMode);

                    switch (StartTriggerType)
                    {
                        case TriggerType.None:
                            Console.WriteLine("5. Disabling the start trigger.");
                            session.Trigger.Start.Disable();
                            break;
                        case TriggerType.SoftwareEdge:
                            Console.WriteLine("5. Configuring the start trigger to use a software edge.");
                            session.Trigger.Start.SoftwareEdge.Configure();
                            break;
                        case TriggerType.DigitalEdge:
                            Console.WriteLine("5. Configuring the start trigger to use a {0:F} digital edge from the source {1:F}.", StartTriggerEdge, Source);
                            session.Trigger.Start.DigitalEdge.Configure(Source, StartTriggerEdge);
                            break;
                        default:
                            Console.WriteLine("The TriggerType supplied is not supported by this example.");
                            return;
                    }

                    Console.WriteLine("6. Creating the arbitrary waveforms and downloading them to memory.");
                    int waveformHandle1 = session.Arbitrary.Waveform.CreateChannelWaveform(ChannelName, Waveform1);
                    int waveformHandle2 = session.Arbitrary.Waveform.CreateChannelWaveform(ChannelName, Waveform2);
                    int waveformHandle3 = session.Arbitrary.Waveform.CreateChannelWaveform(ChannelName, Waveform3);

                    int[] waveformHandles = new int[] { waveformHandle1, waveformHandle2, waveformHandle3 };

                    Console.WriteLine("7. Creating the arbitrary sequence.");
                    int sequenceHandle = session.Arbitrary.Sequence.Create(waveformHandles, LoopCounts);
                    session.Arbitrary.Sequence.SetHandle(ChannelName, sequenceHandle);

                    Console.WriteLine("8. Configuring the arbitrary sequence to have a gain of {0} and an offset of {1}.", Gain, Offset);
                    session.Arbitrary.Sequence.Configure(ChannelName, sequenceHandle, Gain, Offset);

                    Console.WriteLine("9. Initiating generation.");
                    session.InitiateGeneration();

                    if (StartTriggerType == TriggerType.SoftwareEdge)
                    {
                        // If the trigger mode is set to Burst or Stepped then the sequence will need a 
                        // software edge for each waveform in the sequence.
                        if (StartTriggerMode == TriggerMode.Burst || StartTriggerMode == TriggerMode.Stepped)
                        {
                            for (int i = 0; i < waveformHandles.Length; i++)
                            {
                                Console.WriteLine("\t - Sending software edge.");
                                session.Trigger.Start.SoftwareEdge.Send();
                            }
                        }
                        else
                        {
                            Console.WriteLine("\t - Sending software edge.");
                            session.Trigger.Start.SoftwareEdge.Send();
                        }
                    }

                    if (StartTriggerMode == TriggerMode.Single)
                    {
                        try
                        {
                            session.WaitUntilDone(TimeSpan.FromSeconds(30));
                            Console.WriteLine("10. Generation complete.");
                        }
                        catch (TimeoutException)
                        {
                            session.AbortGeneration();
                            Console.WriteLine("10. Generation was aborted.");
                        }
                    }
                    else
                    {
                        // If the trigger mode is set to anything other than Single, 
                        // then the signal generator will continue generating until AbortGeneration() is called.
                        session.AbortGeneration();
                        Console.WriteLine("10. Generation was aborted.");
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
