namespace NationalInstruments.Examples.NIFgen.SweepGenerator
{
    using System;
    using ModularInstruments.NIFgen;

    /// <summary>
    /// Demonstrates the use of the NI-FGEN .NET API to create and configure a frequency list.
    /// Note: Refer to the specifications document for your NI signal generator to determine whether the features and values used in this example are supported.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Set the channel(s) you would like to generate your waveform on.
    /// 3. Set the start and end frequencies as well as the step count and duration per step.
    /// 4. Set the standard waveform you would like to generate and configure its settings.
    /// 5. Build and run the example.
    /// </remarks>
    public sealed class Program
    {
        #region Settings and Configuration

        // IVI resource name of your signal generator in MAX.
        private const string ResourceName = "PXI1Slot2";

        // The name of the channel(s) to use.
        private const string ChannelName = "0";

        // Settings used to create and configure the frequency list.
        private const double StartFrequency = 1.0E3;
        private const double EndFrequency = 1.0E6;
        private const int StepCount = 512;

        // Duration per step in seconds.
        private const double DurationPerStep = 10E-3;

        // The standard waveform to generate.
        private const StandardWaveform Waveform = StandardWaveform.Sine;

        // Waveform settings.
        private const double Amplitude = 1.0;
        private const double DCOffset = 0.0;
        private const double StartPhase = 0.0;

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
                    Console.WriteLine("2. Setting the signal generator's output mode to FrequencyList.");
                    session.Output.OutputMode = OutputMode.FrequencyList;

                    double[] frequencies = new double[StepCount];
                    Ivi.Driver.PrecisionTimeSpan[] durations = new Ivi.Driver.PrecisionTimeSpan[StepCount];

                    for (int i = 0; i < StepCount; i++)
                    {
                        frequencies[i] = (((double)i / StepCount) * (EndFrequency - StartFrequency)) + StartFrequency;
                        durations[i] = Ivi.Driver.PrecisionTimeSpan.FromSeconds(DurationPerStep);
                    }

                    Console.WriteLine("3. Creating a frequncy list from the following settings:");
                    Console.WriteLine("\t Generating a {0} wave.", Waveform);
                    Console.WriteLine("\t Start frequency: {0} Hz", StartFrequency);
                    Console.WriteLine("\t End frequency: {0} Hz", EndFrequency);
                    Console.WriteLine("\t Number of steps: {0}", StepCount);
                    Console.WriteLine("\t Duration per step: {0} seconds per step", DurationPerStep);
                    int frequencyListHandle = session.FrequencyList.Create(Waveform, frequencies, durations);

                    Console.WriteLine("4. Configuring the frequency list with the following settings:");
                    Console.WriteLine("\t - Amplitude: {0} (Vp-p)", Amplitude);
                    Console.WriteLine("\t - DC Offset: {0} (V)", DCOffset);
                    Console.WriteLine("\t - Start Phase: {0}", StartPhase);
                    session.FrequencyList.Configure(ChannelName, frequencyListHandle, Amplitude, DCOffset, StartPhase);

                    Console.WriteLine("5. Initiating generation.");
                    session.InitiateGeneration();

                    // Wait until the generation is complete. If the trigger mode is set
                    // to Continuous, then WaitUntilDone will time out.
                    try
                    {
                        session.WaitUntilDone(TimeSpan.FromSeconds(30));
                        Console.WriteLine("6. Generation complete.");
                    }
                    catch (TimeoutException)
                    {
                        session.AbortGeneration();
                        Console.WriteLine("6. Generation was aborted.");
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