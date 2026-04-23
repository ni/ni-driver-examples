using System.Collections.Generic;
using System.Linq;

namespace NationalInstruments.Examples.NIDigital.LogHistoryRAMResultsToFile
{
    using ModularInstruments.NIDigital;
    using System;

    /// <summary>
    /// Demonstrates how to use the NI-Digital Pattern Driver API to log History RAM results.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource name of your digital pattern instrument. The default
    ///    value for the resource name is "PXI1Slot2,PXI1Slot3".
    /// 3. Configure the number of pretrigger History RAM samples. The default value is 0.
    /// 4. Configure the type of History RAM cycles to acquire. The default value is failures only.
    /// 5. Build and run the example.
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

        // Number of pretrigger History RAM samples.
        private const int PretriggerSamples = 0;

        // Type of History RAM cycles to acquire.
        private const HistoryRamCycle CycleToAcquire = HistoryRamCycle.Failed;

        // Timeout to use when waiting for the pattern burst to complete.
        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

        // Files to load.
        private const string PinMapFilePath = "PinMap.pinmap";
        private const string SpecificationsFilePath = "Specifications.specs";
        private const string PinLevelsFilePath = "PinLevels.digilevels";
        private const string TimingSheetFilePath = "Timing.digitiming";
        private const string PatternFilePath = "Pattern.digipat";

        // Pattern settings.
        private const string PatternStartLabel = "new_pattern";

        #endregion // Settings and Configuration

        static void Main(string[] args)
        {
            try
            {
                // Initialize the digital pattern instrument session. The reset
                // input of the NIDigital Constructor is TRUE to ensure that
                // the instrument starts in a known state. All channels are in a
                // high-impedance state, and the I / O switches are open.
                Console.WriteLine("1. Initializing the digital pattern instrument session.");
                using (NIDigital session = new NIDigital(ResourceName, true, true, OptionString))
                {
                    Console.WriteLine("2. Configuring driver session.");
                    ConfigureSession(session);

                    // Burst the pattern.
                    Console.WriteLine("3. Bursting pattern.");
                    string site = "site0";
                    bool selectDigitalFunction = true;
                    bool waitUntilDone = true;
                    session.PatternControl.BurstPattern(site, PatternStartLabel, selectDigitalFunction, waitUntilDone, Timeout);

                    // Get the History RAM sample count for the specified site. This will be used to
                    // determine how many History RAM samples to fetch.
                    long samplesAvailable = session.HistoryRam.GetSampleCount(site);

                    // Fetch the timeset name, pattern name, vector number, cycle number,
                    // actual pin states, expected pin states, and per pin pass fail for all History RAM samples.
                    Console.WriteLine("4. Fetching History RAM samples.");
                    long position = 0;
                    DigitalPinSet pinSet = session.PatternControl.GetPatternPinSet(PatternStartLabel);
                    DigitalHistoryRamCycleInformation[] cycleInformationArray = session.HistoryRam.FetchCycleInformation(site, pinSet, position, samplesAvailable);

                    // Write the History RAM header to the HistoryRAMResults.csv file.
                    Console.WriteLine("5. Writing the header to the HistoryRAMResults.csv file.");

                    string[] patternPins = session.PatternControl.GetPatternPinSetString(PatternStartLabel).Split(',');
                    IEnumerable<string> actualHeaders = patternPins.Select(s => s.Trim() + " actual");
                    string actualHeadersString = string.Join(",", actualHeaders);
                    IEnumerable<string> expectedHeaders = patternPins.Select(s => s.Trim() + " expected");
                    string expectedHeadersString = string.Join(",", expectedHeaders);

                    string header = "TimeSet,Pattern,Vector,Cycle," + actualHeadersString + "," + expectedHeadersString;
                    System.Collections.Generic.List<string> csv = new System.Collections.Generic.List<string>();
                    csv.Add(header);

                    // Write the History RAM results to the HistoryRAMResults.csv file.
                    Console.WriteLine("6. Writing the History RAM results to the HistoryRAMResults.csv file.");
                    foreach (DigitalHistoryRamCycleInformation cycleInformation in cycleInformationArray)
                    {
                        string cycleInfoString = string.Format("{0},{1},{2},{3}", cycleInformation.TimeSetName, cycleInformation.PatternName,
                            cycleInformation.VectorNumber.ToString(), cycleInformation.CycleNumber.ToString());

                        foreach (PinState[] actualPinStates in cycleInformation.ActualPinStates)
                        {
                            foreach (PinState actualPinState in actualPinStates)
                            {
                                cycleInfoString = string.Format("{0},{1}", cycleInfoString, actualPinState);
                            }
                        }

                        foreach (PinState[] expectedPinStates in cycleInformation.ExpectedPinStates)
                        {
                            foreach (PinState expectedPinState in expectedPinStates)
                            {
                                cycleInfoString = string.Format("{0},{1}", cycleInfoString, expectedPinState);
                            }
                        }

                        csv.Add(cycleInfoString);
                    }
                    System.IO.File.WriteAllLines("HistoryRAMResults.csv", csv);

                    // Disconnect all channels using programmable, onboard switching. Then close the digital pattern instrument session.
                    Console.WriteLine("7. Cleaning up for session closure.");
                    session.PinAndChannelMap.GetPinSet(string.Empty).SelectedFunction = SelectedFunction.Disconnect;
                }
            }
            // Handle driver-specific exceptions before the general exception handler that follows.
            // An example of a driver-specific exception is Ivi.Driver.IviCDriverException.
            catch (Exception e)
            {
                Console.Write(e);
            }
            finally
            {
                Console.WriteLine("Program complete. Press any key to close.");
                Console.ReadKey();
            }
        }

        private static void ConfigureSession(NIDigital session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session", "You must create a valid NIDigital session before calling ConfigureSession.");
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

            // Configure the History RAM to trigger on first failure.
            session.Trigger.HistoryRamTrigger.FirstFailure.Configure(PretriggerSamples);

            // Configure History RAM to cycles to acquire.
            session.HistoryRam.CyclesToAcquire = CycleToAcquire;
        }
    }
}