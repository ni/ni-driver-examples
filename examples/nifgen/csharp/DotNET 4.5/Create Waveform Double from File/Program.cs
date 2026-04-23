namespace NationalInstruments.Examples.NIFgen.CreateWaveformDoubleFromFile
{
    using System;
    using System.IO;
    using NationalInstruments.ModularInstruments.NIFgen;

    /// <summary>
    /// Demonstrates the use of the NI-FGEN .NET API to 
    /// download waveform data from a file and generate a waveform signal.
    /// Note: Refer to the specifications document for your NI signal generator to determine whether the features and values used in this example are supported.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Familiarize yourself with the settings in the Settings and Configuration code region.
    /// 2. Set the waveform data for the waveform you would like to generate.
    /// 3. Set the file path to write and read your waveform from.
    /// 4. Build and run the example.
    /// </remarks>
    public sealed class Program
    {
        #region Settings and Configuration

        // IVI resource name of your signal generator in MAX.
        private const string ResourceName = "PXI1Slot2";

        // The name of the channel(s) to use.
        private const string ChannelName = "0";

        // The sample rate to use.
        private const double SampleRate = 4.0e7;

        // Waveform data for writing to and reading from the binary file.
        private static readonly double[] WaveformData = new double[] { -0.9, -0.5, -0.25, -0.1, 0.1, 0.25, 0.5, 0.9 };

        // Path to the binary file to which the waveform data is written and 
        // from where to read and create the waveform in the signal generator's memory.
        private const string WaveformDoubleDataFilePath = @"\WaveformDoubleData.bin";

        #endregion // Settings and Configuration

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1. Initializing the signal generator session.");
                using (NIFgen session = new NIFgen(ResourceName, false, true))
                {
                    Console.WriteLine("2. Setting the signal generator's output mode to Arbitrary.");
                    session.Output.OutputMode = OutputMode.Arbitrary;

                    Console.WriteLine("3. Setting the signal generator's sample rate to {0} samples per second.", SampleRate);
                    session.Arbitrary.SampleRate = SampleRate;

                    // Setting the trigger mode to Single will cause the signal generator
                    // to only generate once.
                    session.Trigger.SetTriggerMode(ChannelName, TriggerMode.Single);

                    Console.WriteLine("4. Creating a waveform data file of double samples at \"{0}\"", WaveformDoubleDataFilePath);
                    WriteWaveformDataToFile(WaveformDoubleDataFilePath, WaveformData);

                    Console.WriteLine("5. Downloading a waveform to the signal generator's memory from the file located at \"{0}\"", WaveformDoubleDataFilePath);
                    int waveformHandle = session.Arbitrary.Waveform.CreateChannelWaveformDoubleFromFile(ChannelName, WaveformDoubleDataFilePath, ByteOrder.LittleEndian);

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

        private static void WriteWaveformDataToFile(string filePath, double[] data)
        {
            if (data == null)
            {
                // Throw if data is null.
                throw new ArgumentNullException("data");
            }
            else if (filePath == null)
            {
                // Throw if filePath is null.
                throw new ArgumentNullException("filePath");
            }
            else
            {
                // Creates an array of bytes of same size as the data array.
                byte[] byteDataArray = new byte[data.Length * sizeof(double)];

                // Copy data to the byteDataArray.
                Buffer.BlockCopy(data, 0, byteDataArray, 0, byteDataArray.Length);

                // Write the data directly to file.
                using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    stream.Write(byteDataArray, 0, byteDataArray.Length);
                }
            }
        }
    }
}