namespace NationalInstruments.Examples.NIDigital.SourceWaveform
{
    using ModularInstruments.NIDigital;
    using System;
    using System.Linq;

    /// <summary>
    /// Demonstrates the use of the NI-Digital Pattern Driver API to configure and create a source
    /// waveform using source memory.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource name of your digital pattern instrument. The default
    ///    value for the resource name is "PXI1Slot2,PXI1Slot3".
    /// 3. Set the SourceWaveformType field to configure the waveform type. Also configure the
    ///    waveform type-specific fields that follow it.
    /// 4. Specify the data type to use by setting the DataMapping field. The DataMapping field
    ///    determines the behavior of the WriteSourceWaveform method defined in this example. The
    ///    DataMapping field does not affect the example if SourceWaveformType is set to FromFileTdms.
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

        // Files to load.
        private const string PinMapFilePath = "PinMap.pinmap";
        private const string SpecificationsFilePath = "Specifications.specs";
        private const string PinLevelsFilePath = "PinLevels.digilevels";
        private const string TimingSheetFilePath = "Timing.digitiming";
        private const string PatternFilePath = "Pattern.digipat";

        // Waveform configuration.
        private static readonly WaveformType SourceWaveformType = WaveformType.Parallel;
        private const string WaveformName = "new_waveform";
        private const SourceDataMapping DataMapping = SourceDataMapping.Broadcast;

        // Broadcast waveform configuration. The default value is an array of size 50 populated with
        // all '0' values.
        private static readonly uint[] BroadcastWaveformData = Enumerable.Repeat(0U, 50).ToArray();

        // Site unique waveform configuration. The default value is a jagged array of four arrays of
        // size 50 populated with all '0' values.
        private static readonly string SiteList = string.Empty;
        private static readonly uint[][] SiteUniqueWaveformData =
            Enumerable.Range(0, 3).Select(i => Enumerable.Repeat(0U, 50).ToArray()).ToArray();

        // Parallel waveform type configuration.
        private const string ParallelPinSetString = "PinGroup1";

        // Serial waveform type configuration.
        private const string SerialPinSetString = "DUTPin1";
        private const int SerialSampleWidth = 32;
        private const BitOrder SerialBitOrder = BitOrder.MostSignificantBitFirst;

        // From file (TDMS) waveform type configuration.
        private const string TdmsFilePath = "SourceWaveform.tdms";
        private const bool WriteWaveformData = true;

        // Pattern label for the pattern to burst.
        private const string PatternStartLabel = "new_pattern";

        // Specifying an empty string as the sites to burst causes the pattern to burst on all
        // defined sites.
        private static readonly string SitesToBurst = string.Empty;

        #endregion // Settings and Configuration

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1. Initializing the digital pattern instrument session.");
                using (NIDigital session = new NIDigital(ResourceName, true, true, OptionString))
                {
                    Console.WriteLine("2. Configuring instrument session.");
                    ConfigureSession(session);

                    Console.WriteLine("3. Creating and writing the source waveform using the configured settings.");
                    switch (SourceWaveformType)
                    {
                        case WaveformType.Parallel:
                            session.SourceWaveforms.CreateParallel(ParallelPinSetString, WaveformName, DataMapping);
                            WriteSourceWaveform(session, DataMapping);
                            break;

                        case WaveformType.Serial:
                            session.SourceWaveforms.CreateSerial(SerialPinSetString, WaveformName, DataMapping, SerialSampleWidth, SerialBitOrder);
                            WriteSourceWaveform(session, DataMapping);
                            break;

                        case WaveformType.FromFileTdms:
                            session.SourceWaveforms.CreateFromFile(WaveformName, TdmsFilePath, WriteWaveformData);
                            break;
                    }

                    // Burst the pattern from your instrument from the configured start label. This
                    // can be a label in your .digipat file, or it can be the name of the pattern.
                    Console.WriteLine("4. Bursting the pattern.");
                    session.PatternControl.BurstPattern(SitesToBurst, PatternStartLabel, true, true, TimeSpan.FromSeconds(10.0));

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

        private static void WriteSourceWaveform(NIDigital session, SourceDataMapping dataMapping)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            switch (dataMapping)
            {
                case SourceDataMapping.Broadcast:
                    session.SourceWaveforms.WriteBroadcast(WaveformName, BroadcastWaveformData);
                    break;

                case SourceDataMapping.SiteUnique:
                    session.SourceWaveforms.WriteSiteUnique(SiteList, WaveformName, SiteUniqueWaveformData);
                    break;
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
    }

    internal enum WaveformType
    {
        Parallel,
        Serial,
        FromFileTdms
    }
}
