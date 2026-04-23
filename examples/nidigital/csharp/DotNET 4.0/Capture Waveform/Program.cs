namespace NationalInstruments.Examples.NIDigital.CaptureWaveform
{
    using ModularInstruments.NIDigital;
    using System;
    using System.Linq;

    /// <summary>
    /// Demonstrates the use of the NI-Digital Pattern Driver API to create and capture data using a
    /// capture waveform.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource name of your digital pattern instrument. The default
    ///    value for the resource name is "PXI1Slot2,PXI1Slot3".
    /// 3. In the capture waveform configuration section, set the Mode field to the capture waveform
    ///    mode you would like to use. If you choose to use a serial capture waveform, select either
    ///    most or least significant bit for the SerialCaptureWaveformBitOrder field. The
    ///    PinSetString field will adapt to the value of the Mode field by initializing to the
    ///    correct pin set string identifying what pins defined in the pin map to use.
    /// 4. Build and run the example.
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

        // Capture waveform configuration. Use the Mode field to set capture waveform mode using the
        // CaptureWaveformMode enum defined in this example file.
        private static CaptureWaveformMode Mode = CaptureWaveformMode.Serial;
        private static readonly BitOrder SerialCaptureWaveformBitOrder = BitOrder.MostSignificantBitFirst;
        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);
        private const string WaveformName = "new_waveform";
        private const int SamplesToRead = 4;
        private const int SerialCaptureWaveformSampleWidth = 32;

        // Pin set string configuration. The pin set string is selected based on the value of the
        // Mode field you have configured. CreateSerial, called when Mode is
        // CaptureWaveformMode.Serial, requires the use of only one pin whereas every other mode can
        // use multiple pins.
        private static readonly string PinSetString = Mode == CaptureWaveformMode.Serial ? "DUTPin1" : "PinGroup1";

        // Files to load.
        private const string PinMapFilePath = "PinMap.pinmap";
        private const string SpecificationsFilePath = "Specifications.specs";
        private const string PinLevelsFilePath = "PinLevels.digilevels";
        private const string TimingSheetFilePath = "Timing.digitiming";
        private const string PatternFilePath = "Pattern.digipat";

        // File loaded when Mode is set to CaptureWaveformMode.FromDigicaptureFile.
        private const string CaptureWaveformFilePath = "CaptureWaveform.digicapture";

        // Pattern label for the pattern to burst.
        private const string PatternStartLabel = "new_pattern";

        // Specifying an empty string as the sites to burst or fetch causes the pattern to burst or
        // fetch on all defined sites.
        private static readonly string SitesToBurst = string.Empty;
        private static readonly string SitesToFetch = string.Empty;

        #endregion // Settings and Configuration

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1. Initializing the digital pattern instrument session.");
                using (NIDigital session = new NIDigital(ResourceName, true, true, OptionString))
                {
                    Console.WriteLine(string.Format("2. Creating a capture waveform in {0} mode.", Mode));
                    ConfigureSession(session);

                    switch (Mode)
                    {
                        case CaptureWaveformMode.Serial:
                            session.CaptureWaveforms.CreateSerial(PinSetString, WaveformName,
                                SerialCaptureWaveformSampleWidth, SerialCaptureWaveformBitOrder);
                            break;

                        case CaptureWaveformMode.Parallel:
                            session.CaptureWaveforms.CreateParallel(PinSetString, WaveformName);
                            break;

                        case CaptureWaveformMode.FromDigicaptureFile:
                            session.CaptureWaveforms.CreateFromFile(WaveformName, CaptureWaveformFilePath);
                            break;
                    }

                    // Burst the pattern from your instrument from the configured start label. This
                    // can be a label in your .digipat file, or it can be the name of the pattern.
                    Console.WriteLine("3. Bursting the pattern.");
                    session.PatternControl.BurstPattern(SitesToBurst, PatternStartLabel, true, Timeout);

                    // Fetch the capture waveform data from memory and store it in the
                    // fetchedCaptureWaveforms variable. To avoid allocating memory to store the
                    // results on subsequence calls to Fetch, pass the same array reference as the
                    // reference data parameter on each call to Fetch. The fetchedCaptureWaveforms
                    // array will not be reallocated as long as its size stays the same on subsequent calls.
                    Console.WriteLine("4. Fetching data.");
                    uint[][] fetchedCaptureWaveforms = new uint[][] { };
                    fetchedCaptureWaveforms = session.CaptureWaveforms.Fetch(SitesToFetch, WaveformName, SamplesToRead, Timeout, ref fetchedCaptureWaveforms);

                    // Display the fetched data to console.
                    Console.WriteLine("5. Displaying captured waveforms.");
                    DisplayData(fetchedCaptureWaveforms);
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
        }

        private static void DisplayData(uint[][] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            Console.WriteLine("Data:");

            for (int i = 0; i < data.Length; ++i)
            {
                uint[] siteData = data[i];

                if (siteData == null)
                {
                    Console.WriteLine("Site ({0}): No data", i);
                }
                else
                {
                    Console.WriteLine("Site ({0}): {1}", i, string.Join(", ", siteData.Select(j => j.ToString("X"))));
                }
            }
        }
    }

    internal enum CaptureWaveformMode
    {
        Serial,
        Parallel,
        FromDigicaptureFile
    }
}
