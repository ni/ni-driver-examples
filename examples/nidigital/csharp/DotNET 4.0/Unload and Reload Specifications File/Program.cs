namespace NationalInstruments.Examples.NIDigital.UnloadAndReloadSpecificationsFile
{
    using ModularInstruments.NIDigital;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Demonstrates how to use the NI-Digital Pattern Driver API to create and configure an
    /// instrument session and to unload and reload specification files. Additionally, it
    /// demonstrates sweeping of specifications variables by unloading and reloading specifications
    /// file and generating a Shmoo plot based on Capture Waveform data.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Disconnect the cable from the front of the instrument
    /// 3. Ensure that the ResourceName field and all instances of the resource name within the
    ///    PinMap.pinmap file match the resource name of your digital pattern instrument. The default
    ///    value for the resource name is "PXI1Slot2,PXI1Slot3".
    /// 4. Build and run the example.
    /// </remarks>
    public struct SweepParameters
    {
        public string SpecificationToSweep;
        public double StartValue;
        public double StopValue;
        public int NumberOfSteps;
    }

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
        private const string PatternFilePath = "demo_pattern_no_hw.digipat";
        private const string SpecificationsFilePath = "Specifications.specs";

        //Specification Sweep Parameters
        private static readonly SweepParameters XAxisParameters = new SweepParameters
        {
            SpecificationToSweep = "AC.Strobe",
            StartValue = 0,
            StopValue = 0.00000001,
            NumberOfSteps = 50
        };

        private static readonly SweepParameters YAxisParameters = new SweepParameters
        {
            SpecificationToSweep = "DC.Voh",
            StartValue = 0,
            StopValue = 5,
            NumberOfSteps = 100
        };

        // Source Waveform configuration.
        private const string WaveformName = "demo_src_wfm";

        private const SourceDataMapping DataMapping = SourceDataMapping.Broadcast;
        private const string SerialPinSetString = "SDATA";
        private const int SerialSampleWidth = 32;
        private const BitOrder SerialBitOrder = BitOrder.MostSignificantBitFirst;

        // Capture waveform configuration.
        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

        private const string CaptureWaveformName = "demo_waveform";
        private const string CaptureWaveformPath = "demo_waveform.digicapture";

        // Broadcast waveform configuration
        private static IEnumerable<uint> _broadcastWaveformData;

        // Pattern label for the pattern to burst.
        private const string PatternStartLabel = "startdemo";

        // Specifying an empty string as the sites to burst or fetch causes the pattern to burst or
        // fetch on all defined sites.
        private static readonly string SitesToBurst = string.Empty;

        private static readonly string SitesToFetch = string.Empty;

        #endregion Settings and Configuration

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1. Initializing the digital pattern instrument session.");
                using (NIDigital session = new NIDigital(ResourceName, true, true, OptionString))
                {
                    Console.WriteLine("2. Configuring instrument session.");
                    string tempSpecificationsFilePath;
                    ConfigureSession(session, out tempSpecificationsFilePath);

                    Console.WriteLine("3. Creating source and capture waveforms using the configured settings.");

                    session.SourceWaveforms.CreateSerial(SerialPinSetString, WaveformName, DataMapping, SerialSampleWidth, SerialBitOrder);
                    WriteSourceWaveform(session);

                    session.CaptureWaveforms.CreateFromFile(CaptureWaveformName, CaptureWaveformPath);

                    // Burst the pattern from your instrument from the configured start label. This
                    // can be a label in your .digipat file, or it can be the name of the pattern.
                    Console.WriteLine("4. Bursting the pattern.");

                    //Create the sweep arrays for the two values.
                    IEnumerable<double> xAxisSweepValues = GenerateSweepValues(XAxisParameters);
                    IEnumerable<double> yAxisSweepValues = GenerateSweepValues(YAxisParameters);

                    //determine number of samples to capture.
                    int samplesToRead = _broadcastWaveformData.Count();

                    byte[] expectedData = _broadcastWaveformData.SelectMany(BitConverter.GetBytes).ToArray();
                    byte[] receivedData = new byte[expectedData.GetLength(0)];
                    BitArray expectedDataBits = new BitArray(expectedData);

                    double[,] bitErrorRate = new double[XAxisParameters.NumberOfSteps, YAxisParameters.NumberOfSteps];
                    //Sweep over the two arrays
                    int xAxisCounter = 0;

                    foreach (double xAxisSweepValue in xAxisSweepValues)
                    {
                        int yAxisCounter = 0;
                        foreach (double yAxisSweepValue in yAxisSweepValues)
                        {
                            session.UnloadSpecifications(tempSpecificationsFilePath); //Unload Specifications File
                            UpdateSpecificationValue(tempSpecificationsFilePath, XAxisParameters.SpecificationToSweep, $"{xAxisSweepValue:0.##########}"); //Modify Single Specification
                            UpdateSpecificationValue(tempSpecificationsFilePath, YAxisParameters.SpecificationToSweep, $"{yAxisSweepValue:0.##########}"); //Modify Single Specification
                            session.LoadSpecifications(tempSpecificationsFilePath); //Reload Specification File
                            session.ApplyLevelsAndTiming(string.Empty, PinLevelsFilePath, TimingSheetFilePath); //Re-apply Timing and Levels to use the newly loaded Specifications

                            session.PatternControl.BurstPattern(SitesToBurst, PatternStartLabel, true, true, TimeSpan.FromSeconds(10.0));
                            uint[][] fetchedCaptureWaveforms = new uint[][] { };
                            session.CaptureWaveforms.Fetch(SitesToFetch, CaptureWaveformName, samplesToRead, Timeout, ref fetchedCaptureWaveforms);
                            receivedData = fetchedCaptureWaveforms[0].SelectMany(BitConverter.GetBytes).ToArray();
                            BitArray receivedDataBits = new BitArray(receivedData);
                            bitErrorRate[xAxisCounter, yAxisCounter] = CalculateBitErrorRate(receivedDataBits, expectedDataBits);
                            yAxisCounter++;
                        }
                        xAxisCounter++;
                    }

                    // Write the results to the ShmooResults.csv file.
                    List<string> csv = new List<string>();
                    Console.WriteLine("6. Writing the results to the ShmooResults.csv file.");

                    //To properly format the data in our file we start at the end of our yAxis sweep and work backwards to the beginning
                    for (int yAxis = bitErrorRate.GetLength(1) - 1; yAxis >= 0; yAxis--)
                    {
                        string bitErrorString = "";
                        for (int xAxis = 0; xAxis < bitErrorRate.GetLength(0); xAxis++)
                        {
                            bitErrorString += $"{bitErrorRate[xAxis, yAxis]:0.00},";
                        }
                        csv.Add(bitErrorString);
                    }
                    File.WriteAllLines("ShmooResults.csv", csv);

                    // Disconnect all channels using programmable, onboard switching.
                    Console.WriteLine("5. Cleaning up for session closure.");
                    session.PinAndChannelMap.GetPinSet(string.Empty).SelectedFunction = SelectedFunction.Disconnect;

                    //Delete the temporary Specifications File.
                    if (tempSpecificationsFilePath != null)
                    {
                        File.Delete(tempSpecificationsFilePath);
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

        private static void ConfigureSession(NIDigital session, out string tempSpecificationsFilePath)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session), "You must create a valid NIDigital session before calling ConfigureSession.");
            }

            // Load the pin map for your instrument. Perform the pin map load at the beginning of
            // your program to reference pin names defined in the pin map.
            session.LoadPinMap(PinMapFilePath);

            //Create a temporary version of the Specifications sheet so that we don't over-write the original on disk
            tempSpecificationsFilePath = CreateTempSpecificationsFile(SpecificationsFilePath);

            // Load specifications, levels, and timing sheets created using the Digital Pattern
            // Editor. These settings are not applied until you call ApplyLevelsAndTiming on the
            // NIDigital session object.
            session.LoadSpecifications(tempSpecificationsFilePath);
            session.LoadLevels(PinLevelsFilePath);
            session.LoadTiming(TimingSheetFilePath);

            // Apply the loaded sheets to your digital pattern instrument.
            session.ApplyLevelsAndTiming(string.Empty, PinLevelsFilePath, TimingSheetFilePath);

            // Load a pattern file onto your instrument.
            session.LoadPattern(PatternFilePath);
        }

        private static string CreateTempSpecificationsFile(string originalSpecificationsFilePath)
        {
            string tempSpecificationsFilePath = Path.GetTempFileName();
            File.Copy(originalSpecificationsFilePath, tempSpecificationsFilePath, true);
            FileInfo fileInfo = new FileInfo(tempSpecificationsFilePath)
            {
                Attributes = FileAttributes.Temporary,
                IsReadOnly = false //ensure that we can overwrite the temp file, even in the event the original file is Read Only
            };
            return tempSpecificationsFilePath;
        }

        private static IEnumerable<uint> CreateWaveformData()
        {
            //creates a waveform comprised of 32 samples of pseudorandom uint
            Random randomNumber = new Random();
            IEnumerable<uint> waveformData = Enumerable
                .Repeat(0, 32)
                .Select(i => (uint)(randomNumber.NextDouble() * uint.MaxValue))
                .ToArray();
            return waveformData;
        }

        private static void WriteSourceWaveform(NIDigital session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session), "You must create a valid NIDigital session before calling WriteSourceWaveform.");
            }
            _broadcastWaveformData = CreateWaveformData();
            session.SourceWaveforms.WriteBroadcast(WaveformName, _broadcastWaveformData.ToArray());
        }

        private static IEnumerable<double> GenerateSweepValues(SweepParameters parameters)
        {
            IEnumerable<double> sweepValues = Enumerable.Range(0, parameters.NumberOfSteps).Select(i => parameters.StartValue + (parameters.StopValue - parameters.StartValue) * ((double)i / (parameters.NumberOfSteps - 1)));
            return sweepValues;
        }

        private static void UpdateSpecificationValue(string specificationsFilePath, string specification, string newValue) //use Split to get section and symbol
        {
            string[] splitSpecification = specification.Split('.');
            string section = splitSpecification[0];
            string symbol = splitSpecification[1];
            var doc = XDocument.Load(specificationsFilePath);
            var root = doc.Root;
            var defaultNamespace = root.GetDefaultNamespace();
            var formulaNamespace = root.GetNamespaceOfPrefix("f");

            var element = doc
                .Elements(defaultNamespace + "Specifications")
                .Descendants(defaultNamespace + "Section")
                .First(el => (string)el.Attribute("name") == section)
                .Descendants(formulaNamespace + "Formula")
                .First(el => (string)el.Attribute("symbol") == symbol)
                .Descendants().First();

            element.Value = newValue;

            doc.Save(specificationsFilePath);
        }

        private static double CalculateBitErrorRate(BitArray receivedDataBits, BitArray expectedDataBits)
        {
            receivedDataBits.Xor(expectedDataBits);
            int bitErrorCount = 0;
            foreach (bool bit in receivedDataBits)
            {
                if (bit)
                {
                    bitErrorCount = bitErrorCount + 1;
                }
            }
            double bitErrorRate = Convert.ToDouble(bitErrorCount) / Convert.ToDouble(expectedDataBits.Length);
            return bitErrorRate;
        }
    }
}