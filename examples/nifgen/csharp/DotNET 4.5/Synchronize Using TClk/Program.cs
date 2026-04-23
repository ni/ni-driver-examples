namespace NationalInstruments.Examples.NIFgen.SynchronizationUsingTClock
{
    using System;
    using System.Collections.Generic;
    using NationalInstruments.ModularInstruments.NIFgen;
    using NationalInstruments.ModularInstruments.SystemServices.TimingServices;

    /// <summary>
    /// Demonstrates the use of the NI-FGEN .NET API and NI-TClk .NET API to synchronize waveform generation on multiple signal generators.
    /// Note: Refer to the specifications document for your NI signal generator to determine whether the features and values used in this example are supported.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Familiarize yourself with the settings in the Settings and Configuration code region.
    /// 2. Set the resource names of the signal generators on which to synchronize waveform generation.
    /// 3. Set the channel(s) you would like to generate your waveform on for each signal generator.
    /// 4. Set the data for the waveforms you would like to generate for each signal generator.
    /// 5. Build and run the example.
    /// </remarks>
    public sealed class Program
    {
        #region Settings and Configuration

        // An array of IVI resource names of your signal generators in MAX.
        private static readonly string[] ResourceNames = new string[] { "PXI1Slot2" };

        // The name of the channel(s) to use.
        private const string ChannelName = "0";

        // Waveform data.
        private static readonly short[] Waveform = { 0, 1, 2, 3 };

        #endregion // Settings and Configuration

        static void Main(string[] args)
        {
            // This list is used to store all signal generator sessions that are created.
            List<NIFgen> sessionList = new List<NIFgen>();

            // Create an NI-TClk object that can be used to synchronize multiple instruments.
            TClock tclk = new TClock();

            try
            {
                Console.WriteLine("1. Initializing the signal generator sessions");
                foreach (string resourceName in ResourceNames)
                {
                    Console.WriteLine("\t Resource: {0}", resourceName);

                    // Create the signal generator session.
                    NIFgen session = new NIFgen(resourceName, false, true);

                    // Add the signal generator session to the list.
                    sessionList.Add(session);

                    // Setting the trigger mode to Single will cause the signal generator
                    // to only generate once.
                    session.Trigger.SetTriggerMode(ChannelName, TriggerMode.Single);

                    // Set the output mode of the signal generator. Set this at the beginning of your 
                    // program to determine what the signal generator will output.
                    Console.WriteLine("\t\t Setting the signal generator's output mode to Arbitrary.");
                    session.Output.OutputMode = OutputMode.Arbitrary;

                    Console.WriteLine("\t\t Allocating memory up front for the waveform to generate.");
                    int waveformHandle = session.Arbitrary.Waveform.Allocate(ChannelName, Waveform.Length);

                    Console.WriteLine("\t\t Downloading the waveform(s) to the Arbitrary Waveform Generator's memory for the channel(s) specified.");
                    session.Arbitrary.Waveform.Write(ChannelName, waveformHandle, Waveform);

                    // Add the signal generator session to the NI-TClk object.
                    tclk.DevicesToSynchronize.Add(session);
                }

                Console.WriteLine("2. Configuring homogeneous triggers for synchronization.");
                tclk.ConfigureForHomogeneousTriggers();

                Console.WriteLine("3. Synchronizing signal generators.");
                tclk.Synchronize();

                Console.WriteLine("4. Initiating generation on all signal generators.");
                tclk.Initiate();

                // Wait until the generation is complete on all signal generators. If the trigger mode is set
                // to Continuous, then WaitUntilDone will time out.
                try
                {
                    tclk.WaitUntilDone(PrecisionTimeSpan.FromSeconds(30));
                    Console.WriteLine("5. Generation complete.");
                }
                catch (TimeoutException)
                {
                    // Abort generation on all signal generators.
                    foreach (NIFgen session in sessionList)
                    {
                        session.AbortGeneration();
                    }

                    Console.WriteLine("5. Generation was aborted.");
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
                Console.WriteLine("6. Closing sessions.");

                // Close all sessions from the list.
                foreach (NIFgen session in sessionList)
                {
                    session.Dispose();
                }

                Console.WriteLine("Program complete. Press any key to close.");
                Console.ReadKey();
            }
        }
    }
}
