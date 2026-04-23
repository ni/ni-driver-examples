namespace NationalInstruments.Examples.NIFgen.SwitchBetweenWaveforms
{
    using System;
    using ModularInstruments.NIFgen;

    /// <summary>
    /// Demonstrates the use of the NI-FGEN .NET API to use scripts to switch between generating
    /// different arbitrary waveforms.
    /// Note: Refer to the specifications document for your NI signal generator to determine whether the features and values used in this example are supported.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Set the channel(s) you would like to generate your waveform on.
    /// 3. [Optional] Set the data in Waveform1 and Waveform2 to data that represents the waveforms
    ///    you would like to generate.
    /// 4. Set the ScriptRepeatCount to determine how many times Waveform1 and Waveform2 will be
    ///    generated.
    /// 5. Build and run the example.
    /// </remarks>
    public sealed class Program
    {
        #region Settings and Configuration

        // IVI resource name of your signal generator in MAX.
        private const string ResourceName = "PXI1Slot2";

        // The name of the channel(s) to use.
        private const string ChannelName = "0";

        // Waveform names and data. Replace the waveform data with your own data.
        private const string WaveformName1 = "Waveform1";
        private const string WaveformName2 = "Waveform2";
        private static readonly short[] Waveform1 = new short[1000];
        private static readonly short[] Waveform2 = new short[1000];

        // The number of times to generate the two waveforms.
        private const int ScriptRepeatCount = 3;

        // Script to generate the waveforms.
        private const string TriggeredSwitchWaveformsScript = @"
            script SwitchWaveforms
              repeat forever
                repeat until scriptTrigger0
                  generate Waveform1
                end repeat
                repeat until scriptTrigger0
                  generate Waveform2
                end repeat
              end repeat
            end script";

        // Script trigger id to trigger the waveform generation from the script.
        private const string ScriptTriggerId = "scriptTrigger0";

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
                    Console.WriteLine("2. Setting the signal generator's output mode to Script.");
                    session.Output.OutputMode = OutputMode.Script;

                    Console.WriteLine("3. Allocating memory for {0} and {1}.", WaveformName1, WaveformName2);
                    session.Arbitrary.NamedWaveform.Allocate(ChannelName, WaveformName1, Waveform1.Length);
                    session.Arbitrary.NamedWaveform.Allocate(ChannelName, WaveformName2, Waveform2.Length);

                    Console.WriteLine("4. Configuring the script trigger.");
                    session.Trigger.Script.SoftwareEdge.Configure(ScriptTriggerId);

                    Console.WriteLine("5. Writing the script to the signal generator.");
                    session.Script.Write(ChannelName, TriggeredSwitchWaveformsScript);

                    Console.WriteLine("6. Initiating generation.");
                    session.InitiateGeneration();

                    for (int i = 0; i < ScriptRepeatCount; i++)
                    {
                        Console.WriteLine("Iteration {0} of {1}:", i + 1, ScriptRepeatCount);

                        // Reset the waveform position, write the new waveform data to the waveform specified, and
                        // send a software trigger to start generating the waveform.
                        SwitchWaveforms(session, WaveformName1, Waveform1);

                        // Reset the waveform position, write the new waveform data to the waveform specified, and
                        // send a software trigger to start generating the waveform.
                        SwitchWaveforms(session, WaveformName2, Waveform2);
                    }

                    Console.WriteLine("7. Aborting generation.");
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

        private static void SwitchWaveforms(NIFgen session, string waveformName, short[] waveformData)
        {
            Console.WriteLine("\t Setting the waveform write position for {0} to the beginning of the waveform.", waveformName);
            session.Arbitrary.NamedWaveform.SetNextWritePosition(ChannelName, waveformName, WaveformWritePosition.Start, 0);

            Console.WriteLine("\t Writing data into {0}.", waveformName);
            session.Arbitrary.NamedWaveform.Write(ChannelName, waveformName, waveformData);

            Console.WriteLine("\t Sending script trigger to generate {0}.", waveformName);
            session.Trigger.Script.SoftwareEdge.Send(ScriptTriggerId);
        }
    }
}
