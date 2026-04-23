//===========================================================================================
//
// Title:
//        Stream To Disk Console
//
// Description:
//      The Application demonstrates the programmatic usage of .Net API for NI-Scope.
//      The application acquires data from a channel and saves it onto a file in the local disk.
//      The waveform stored in the file can be loaded by another shipping example titled "Stream
//      To Disk".
//         
//===========================================================================================

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using NationalInstruments.ModularInstruments.NIScope;

namespace NationalInstruments.Examples.StreamToDiskConsole
{
    class Program
    {
        static NIScope scopeSession;

        static void Main()
        {
            string deviceName, channelList;
            double range, sampleRateMin;
            long recordLengthMin;
            FileStream outputFileStream;
            AnalogWaveformCollection<double> waveforms = null;
            Exception thrownException = null;

            GetInputParameters(out deviceName, out channelList, out range, out sampleRateMin, out recordLengthMin, out outputFileStream);

            try
            {
                InitializeSession(deviceName);

                scopeSession.Channels[channelList].Enabled = true;
                scopeSession.Channels[channelList].Range = range;
                scopeSession.Acquisition.SampleRateMin = sampleRateMin;
                scopeSession.Acquisition.NumberOfPointsMin = recordLengthMin;

                scopeSession.Measurement.Initiate();
                waveforms = scopeSession.Channels[channelList].Measurement.FetchDouble(PrecisionTimeSpan.Zero, recordLengthMin, waveforms);

                new BinaryFormatter().Serialize(outputFileStream, waveforms);
            }
            catch (SecurityException ex)
            {
                Console.WriteLine("Unable to serialize the data: ");
                thrownException = ex;
            }
            catch (SerializationException ex)
            {
                Console.WriteLine("Unable to serialize the data: ");
                thrownException = ex;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured in NI-Scope: ");
                thrownException = ex;
            }
            finally
            {
                if (scopeSession != null)
                {
                    scopeSession.Close();
                    scopeSession = null;
                }
                if (outputFileStream != null)
                {
                    outputFileStream.Close();
                }
                if (thrownException != null)
                {
                    Console.WriteLine(thrownException.Message);
                }
                else
                {
                    Console.WriteLine("Successfully saved acquired data to \"" + outputFileStream.Name + "\"");
                }
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        static void InitializeSession(string deviceName)
        {
            scopeSession = new NIScope(deviceName, false, false);
            scopeSession.DriverOperation.Warning += new EventHandler<ScopeWarningEventArgs>(DriverOperation_Warning);
        }

        static void DriverOperation_Warning(object sender, ScopeWarningEventArgs e)
        {
            Console.WriteLine(e.Text);
        }

        static void GetInputParameters(out string deviceName, out string channelList, out double range, out double SampleRateMin, out long recordLengthMin, out FileStream outputFileStream)
        {
            outputFileStream = null;

            Console.WriteLine("Provide input parameter values for the acquisition:");
            Console.WriteLine("Press Enter to accept the default values for the parameters.");
            Console.WriteLine();

            deviceName = GetInputString("Device Name", "Dev1");
            channelList = GetInputString("Channel List", "0");

            while (!double.TryParse(GetInputString("Range", "10"), out range))
            {
                Console.WriteLine("The entered value is not in the correct format. Please try again:");
            }
            while (!double.TryParse(GetInputString("Minimum Sample Rate", "1e6"), out SampleRateMin))
            {
                Console.WriteLine("The entered value is not in the correct format. Please try again:");
            }
            while (!long.TryParse(GetInputString("Minimum Record Length", "1000"), out recordLengthMin))
            {
                Console.WriteLine("The entered value is not in the correct format. Please try again:");
            }
            while (outputFileStream == null)
            {
                try
                {
                    // Folder location.
                    string directoryPath = @"C:\waveform";
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    FileInfo fileName = new FileInfo(Path.Combine(directoryPath, @"waveform1.txt"));
                    if (!fileName.Exists)
                    {
                        fileName.Create().Close();
                    }
                    else
                    {
                        File.WriteAllText(fileName.ToString(), String.Empty);
                    }
                    outputFileStream = new FileStream(fileName.ToString(), FileMode.Create, FileAccess.ReadWrite);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unable to create a file stream with the given path:");
                    Console.WriteLine(e.Message);
                }
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        static string GetInputString(string prompt, string defaultValue)
        {
            Console.Write(prompt + " [" + defaultValue + "]:");
            string inputString = Console.ReadLine();

            if (string.IsNullOrEmpty(inputString))
                return defaultValue;
            else
                return inputString;
        }
    }
}
