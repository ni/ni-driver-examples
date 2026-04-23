namespace NationalInstruments.Examples.NIDigital.HistoryRAMStreaming
{
    using ModularInstruments.NIDigital;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Demonstrates how to use the History Ram Streaming feature of the NI-Digital Pattern Driver API to
    /// capture more samples of history RAM than available memory on the onboard device buffer.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource name of your digital pattern instrument. The default
    ///    value for the resource name is "PXI1Slot2,PXI1Slot3".
    /// 3. Build and run the example.
    /// </remarks>

    public sealed class Program
    {
        #region Settings and Configuration

        // IVI resource name of your digital pattern instrument in MAX. Multiple instruments may be added as a comma-separated list.
        private const string ResourceName = "PXI1Slot2,PXI1Slot3";

        // Option String for initializing the session. Simulates the hardware.
        private const string OptionString = "Simulate=1,DriverSetup=Model:6570";

        // Pass an empty OptionString to run on hardware (disable simulation)
        //private const string OptionString = "";

        // Files to load.
        private const string PinMapFilePath = "PinMap.pinmap";

        private const string PinLevelsFilePath = "PinLevels.digilevels";
        private const string TimingSheetFilePath = "Timing.digitiming";
        private const string PatternFilePath = "Pattern.digipat";
        private const string SpecificationsFilePath = "Specifications.specs";

        // Pattern label for the pattern to burst.
        private const string PatternStartLabel = "hram_example";

        // Specifying an empty string as the sites to burst or fetch causes the pattern to burst or
        // fetch on all defined sites.
        private static readonly string SitesToBurst = string.Empty;

        private static readonly string SitesToFetch = "site0";

        #endregion Settings and Configuration

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1. Initializing the digital pattern instrument session.");
                using (NIDigital session = new NIDigital(ResourceName, true, true, OptionString))
                {
                    //Get input from the console to determine if History RAM Streaming should be enabled or not
                    bool streaming = false;
                    bool validAnswer = false;
                    Console.WriteLine("Enable History RAM Streaming? (y/n):");
                    do
                    {
                        string answer = Console.ReadLine();
                        if (answer == "y" || answer == "Y")
                        {
                            streaming = true;
                            validAnswer = true;
                        }
                        else if (answer == "n" || answer == "N")
                        {
                            validAnswer = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Value Entered. Enable History RAM Streaming ? (y / n) :");
                        }
                    } while (validAnswer == false);

                    //Configure the Digital Pattern Session
                    Console.WriteLine("2. Configuring instrument session.");
                    ConfigureSession(session, streaming);

                    // Create data structures for streaming. We will maintain a list of each chunk of samples and merge them at the end, as this is faster.
                    List<DigitalHistoryRamCycleInformation[]> historyRamCycleInformation = new List<DigitalHistoryRamCycleInformation[]>();
                    List<long[]> historyRamScanCycleResults = new List<long[]>();
                    long fetchedSamplesCount = 0;

                    // Burst the pattern from your instrument from the configured start label. This
                    // can be a label in your .digipat file, or it can be the name of the pattern.
                    Console.WriteLine("4. Bursting the pattern.");
                    session.PatternControl.BurstPattern(SitesToBurst, PatternStartLabel, true, !streaming, TimeSpan.FromSeconds(1000));

                    bool isDone = false;
                    long newSamples = 0;
                    while (!isDone || newSamples > 0)
                    {
                        isDone = session.PatternControl.IsDone;
                        long totalSamplesCount = session.HistoryRam.GetSampleCount(SitesToFetch);
                        newSamples = totalSamplesCount - fetchedSamplesCount;

                        // Fetch chunk of new samples and add it to the list
                        historyRamCycleInformation.Add(session.HistoryRam.FetchCycleInformation(SitesToFetch, "", fetchedSamplesCount, newSamples));
                        historyRamScanCycleResults.Add(session.HistoryRam.FetchScanCycleNumbers(SitesToFetch, fetchedSamplesCount, newSamples));

                        fetchedSamplesCount += newSamples;
                    }

                    Console.WriteLine("Streaming complete.");

                    // Collapse the blocks of History RAM samples into a single array
                    DigitalHistoryRamCycleInformation[] historyRamResults = new DigitalHistoryRamCycleInformation[fetchedSamplesCount];
                    long[] historyRamScanResults = new long[fetchedSamplesCount];
                    int currentPosition = 0;
                    for (int blockIndex = 0; blockIndex < historyRamCycleInformation.Count; blockIndex++)
                    {
                        historyRamCycleInformation[blockIndex].CopyTo(historyRamResults, currentPosition);
                        historyRamScanCycleResults[blockIndex].CopyTo(historyRamScanResults, currentPosition);
                        currentPosition += historyRamCycleInformation[blockIndex].Length;
                    }

                    Console.WriteLine($"Fetched {historyRamResults.Length} total History RAM samples");

                    // Disconnect all channels using programmable, onboard switching.
                    Console.WriteLine("5. Cleaning up for session closure.");
                    session.PinAndChannelMap.GetPinSet(string.Empty).SelectedFunction = SelectedFunction.Disconnect;
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

        private static void ConfigureSession(NIDigital session, bool streamingEnabled)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session), "You must create a valid NIDigital session before calling ConfigureSession.");
            }

            // Load the pin map for your instrument. Perform the pin map load at the beginning of
            // your program to reference pin names defined in the pin map.
            session.LoadPinMap(PinMapFilePath);

            // Load specifications, levels, and timing sheets created using the Digital Pattern
            // Editor. These settings are not applied until you call ApplyLevelsAndTiming on the
            // NIDigital session object.
            session.LoadSpecifications(SpecificationsFilePath);
            session.LoadLevels(PinLevelsFilePath);
            session.LoadTiming(TimingSheetFilePath);

            // Apply the loaded sheets to your digital pattern instrument.
            session.ApplyLevelsAndTiming(string.Empty, PinLevelsFilePath, TimingSheetFilePath);

            // Load a pattern file onto your instrument.
            session.LoadPattern(PatternFilePath);

            // Configure History RAM streaming Parameters
            session.HistoryRam.CyclesToAcquire = HistoryRamCycle.All;
            session.Trigger.HistoryRamTrigger.TriggerType = HistoryRamTriggerType.CycleNumber;
            session.Trigger.HistoryRamTrigger.CycleNumber.Number = 0;
            session.Trigger.HistoryRamTrigger.PretriggerSamples = 0;
            session.HistoryRam.MaximumSamplesToAcquirePerSite = -1;
            session.HistoryRam.NumberOfSamplesIsFinite = !streamingEnabled;
            session.HistoryRam.BufferSizePerSite = 100000;
        }
    }
}