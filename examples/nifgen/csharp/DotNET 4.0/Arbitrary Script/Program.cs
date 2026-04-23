namespace NationalInstruments.Examples.NIFgen.ArbitraryScript
{
    using System;
    using ModularInstruments.NIFgen;

    /// <summary>
    /// Demonstrates the use of the NI-FGEN .NET API to generate arbitrary waveforms
    /// using a script.
    /// Note: Refer to the specifications document for your NI signal generator to determine whether the features and values used in this example are supported.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Set the channel(s) you would like to generate your waveform(s) on.
    /// 3. Set the names and data for the waveforms you would like to generate.
    /// 4. Set the name of the script and update the script to
    ///    reference the waveforms you would to generate.
    /// 5. Build and run the example.
    /// </remarks>
    public sealed class Program
    {
        #region Settings and Configuration

        // IVI resource name of your signal generator in MAX.
        private const string ResourceName = "PXI1Slot2";

        // The name of the channel(s) to use.
        private const string ChannelName = "0";

        // Waveform data and names. These names can be referenced
        // in the script defined below.
        private const string WaveformName1 = "Waveform1";
        private const string WaveformName2 = "Waveform2";
        private static readonly short[] Waveform1 = { 0, 1, 2, 3 };
        private static readonly short[] Waveform2 = { 0, 10, 0, 10 };

        // Script and script name to run on the signal generator.
        private const string ScriptName = "myScript0";
        private const string Script =
            @"script myScript0
                Generate Waveform1
                Generate Waveform2
              end script";

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
                    Console.WriteLine("2. Setting the signal generator's output mode to Script.");
                    session.Output.OutputMode = OutputMode.Script;

                    Console.WriteLine("3. Downloading {0} to the Arbitrary Waveform Generator's memory.", WaveformName1);
                    session.Arbitrary.NamedWaveform.Write(ChannelName, WaveformName1, Waveform1);

                    Console.WriteLine("4. Downloading {0} to the Arbitrary Waveform Generator's memory.", WaveformName2);
                    session.Arbitrary.NamedWaveform.Write(ChannelName, WaveformName2, Waveform2);

                    Console.WriteLine("5. Setting {0} to be the script to generate and writing the script to the signal generator.", ScriptName);
                    session.Script.ScriptToGenerate = ScriptName;
                    session.Script.Write(ChannelName, Script);

                    Console.WriteLine("6. Initiating generation.");
                    session.InitiateGeneration();

                    // Wait until the generation is complete. If the trigger mode is set
                    // to Continuous, then WaitUntilDone will time out.
                    try
                    {
                        session.WaitUntilDone(TimeSpan.FromSeconds(30));
                        Console.WriteLine("7. Generation complete.");
                    }
                    catch (TimeoutException)
                    {
                        session.AbortGeneration();
                        Console.WriteLine("7. Generation was aborted.");
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
