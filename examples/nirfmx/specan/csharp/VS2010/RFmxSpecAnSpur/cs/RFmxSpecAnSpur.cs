//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties Clock Source, Clock Frequency
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Reference Level and External Attenuation)
//5. Select Spur measurement and enable the traces
//6. Configure Spur Averaging
//7. Configure Spur Number of Ranges
//8. Configure Spur Range Start and Stop frequency
//9. Configure Spur Range RBW filter
//10. Configure Spur Range Limit Mode, Absolute Start and Stop Limit
//11. Configure Spur Range Number of Spurs to Report
//12. Configure Spur Trace Range Index
//13. Initiate Measurement
//14. Fetch Spur Measurements, Traces and Status
//15. Close the RFmx Session

using System;
using NationalInstruments;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnSpur
{
   class RFmxSpecAnSpur
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      String resourceName, frequencySource;
      string selectedPorts;
      double referenceLevel, externalAttenuation, frequency;
      double timeout;
      int averagingCount;
      RFmxSpecAnMXSpurAveragingEnabled averagingEnabled;
      RFmxSpecAnMXSpurAveragingType averagingType;
      RFmxSpecAnMXSpurMeasurementStatus measurementStatus;
      int traceRangeIndex;

      const int NumberOfRanges = 1;
      const int NumberOfSpursToReport = 10;

      //Input array values.
      RFmxSpecAnMXSpurRangeEnabled[] rangeEnabled = new RFmxSpecAnMXSpurRangeEnabled[NumberOfRanges];
      double[] startFrequency = new double[NumberOfRanges];
      double[] stopFrequency = new double[NumberOfRanges];
      RFmxSpecAnMXSpurRbwFilterType[] rbwFilterType = new RFmxSpecAnMXSpurRbwFilterType[NumberOfRanges];
      RFmxSpecAnMXSpurRbwAutoBandwidth[] rbwFilterAutoBandwidth = new RFmxSpecAnMXSpurRbwAutoBandwidth[NumberOfRanges];
      double[] rbwFilterBandwidth = new double[NumberOfRanges];
      RFmxSpecAnMXSpurAbsoluteLimitMode[] absoluteLimitMode = new RFmxSpecAnMXSpurAbsoluteLimitMode[NumberOfRanges];
      RFmxSpecAnMXSpurRangeVbwFilterAutoBandwidth[] vbwAuto = new RFmxSpecAnMXSpurRangeVbwFilterAutoBandwidth[NumberOfRanges];
      RFmxSpecAnMXSpurRangeDetectorType[] detectorType = new RFmxSpecAnMXSpurRangeDetectorType[NumberOfRanges];
      double[] absoluteLimitStart = new double[NumberOfRanges];
      double[] absoluteLimitStop = new double[NumberOfRanges];
      double[] peakThreshold = new double[NumberOfRanges];
      double[] peakExcursion = new double[NumberOfRanges];
      int[] numberOfSpursToReport = new int[NumberOfRanges];
      double[] vbw = new double[NumberOfRanges];
      double[] vbwToRbwRatio = new double[NumberOfRanges];
      int[] detectorPoints = new int[NumberOfRanges];

      //Output values
      int[] numberOfDetectedSpurs;
      int totalSpur = 0;
      RFmxSpecAnMXSpurRangeStatus[] rangeStatus;

      struct SpurList
      {

         public double[] frequency;
         public double[] amplitude;
         public double[] absoluteLimit;
         public double[] margin;
         public Int32[] rangeIndex;

         public SpurList(Int32 numOfDetectedSpur)
         {
            frequency = new double[numOfDetectedSpur];
            amplitude = new double[numOfDetectedSpur];
            absoluteLimit = new double[numOfDetectedSpur];
            margin = new double[numOfDetectedSpur];
            rangeIndex = new Int32[numOfDetectedSpur];
         }
      };

      private SpurList spurMeasurement;

      public void Run()
      {
         try
         {
            InitializeVariables();
            InitializeInstr();
            ConfigureSpecAn();
            RetrieveResults();
            PrintResults();
         }

         catch (Exception ex)
         {
            DisplayError(ex);
         }
         finally
         {
            /* Close session */
            CloseSession();
            Console.WriteLine("Press any key to exit.....");
            Console.ReadKey();
         }
      }

      private void InitializeVariables()
      {
         resourceName = "RFSA";
         selectedPorts = "";
         referenceLevel = 0.00;                       /* dBm */
         externalAttenuation = 0.00;                  /* dB */
         frequencySource = RFmxInstrMXConstants.OnboardClock;
         frequency = 10.0e+6;                         /* Hz */

         averagingCount = 10;
         averagingEnabled = RFmxSpecAnMXSpurAveragingEnabled.False;
         averagingType = RFmxSpecAnMXSpurAveragingType.Rms;

         // Rangelist       
         for (int i = 0; i < NumberOfRanges; i++)
         {
            rangeEnabled[i] = RFmxSpecAnMXSpurRangeEnabled.True;
            startFrequency[i] = 1e+9;               /* Hz */
            stopFrequency[i] = 1.5e+9;              /* Hz */

            rbwFilterType[i] = RFmxSpecAnMXSpurRbwFilterType.Gaussian;
            rbwFilterAutoBandwidth[i] = RFmxSpecAnMXSpurRbwAutoBandwidth.True;
            rbwFilterBandwidth[i] = 30e+3;          /* Hz */

            vbwAuto[i] = RFmxSpecAnMXSpurRangeVbwFilterAutoBandwidth.True;
            vbw[i] = 30.0e3;                        /* Hz */
            vbwToRbwRatio[i] = 3;

            detectorType[i] = RFmxSpecAnMXSpurRangeDetectorType.None;
            detectorPoints[i] = 1001;

            absoluteLimitMode[i] = RFmxSpecAnMXSpurAbsoluteLimitMode.Couple;
            absoluteLimitStart[i] = -10.00;
            absoluteLimitStop[i] = -10.00;

            peakThreshold[i] = -200;
            peakExcursion[i] = 0.00;

            numberOfSpursToReport[i] = NumberOfSpursToReport;
         }
         traceRangeIndex = 0;

         timeout = 10.0;                         /* seconds */
      }

      private void InitializeInstr()
      {
         /* Create a new RFmx Session */
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      private void ConfigureSpecAn()
      {
         /* Get SpecAn signal */
         specAn = instrSession.GetSpecAnSignalConfiguration();

         /* Configure measurement */

         instrSession.ConfigureFrequencyReference("", frequencySource, frequency);
         specAn.SetSelectedPorts("", selectedPorts);
         specAn.ConfigureReferenceLevel("", referenceLevel);
         specAn.ConfigureExternalAttenuation("", externalAttenuation);

         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Spur, true);
         specAn.Spur.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                      averagingType);
         specAn.Spur.Configuration.ConfigureNumberOfRanges("", NumberOfRanges);
         specAn.Spur.Configuration.ConfigureRangeFrequencyArray("", startFrequency,
                                                                stopFrequency, rangeEnabled);
         specAn.Spur.Configuration.ConfigureRangeRbwArray("", rbwFilterAutoBandwidth,
                                                          rbwFilterBandwidth, rbwFilterType);
         specAn.Spur.Configuration.ConfigureRangeAbsoluteLimitArray("", absoluteLimitMode,
                                                                    absoluteLimitStart,
                                                                    absoluteLimitStop);
         specAn.Spur.Configuration.ConfigureRangeNumberOfSpursToReportArray("",
                                                                           numberOfSpursToReport);
         specAn.Spur.Configuration.ConfigureRangePeakCriteriaArray("", peakThreshold, peakExcursion);
         specAn.Spur.Configuration.ConfigureRangeDetectorArray("", detectorType, detectorPoints);
         specAn.Spur.Configuration.ConfigureRangeVbwFilterArray("", vbwAuto, vbw, vbwToRbwRatio);
         specAn.Spur.Configuration.ConfigureTraceRangeIndex("", traceRangeIndex);

         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */

         Spectrum<float> absoluteLimitTrace = null;
         Spectrum<float> spectrumTrace = null;
         string rangeString = "";

         specAn.Spur.Results.FetchRangeStatusArray("", timeout, ref rangeStatus,
                                                   ref numberOfDetectedSpurs);

         for (int i = 0; i < NumberOfRanges; i++)
         {
            totalSpur += numberOfDetectedSpurs[i];
         }
         spurMeasurement = new SpurList(totalSpur);
         specAn.Spur.Results.FetchAllSpurs("", timeout,
                                         ref spurMeasurement.frequency,
                                         ref spurMeasurement.amplitude,
                                         ref spurMeasurement.margin,
                                         ref spurMeasurement.absoluteLimit,
                                         ref spurMeasurement.rangeIndex);

         int rangeNumber = (traceRangeIndex == -1) ? 0 : traceRangeIndex;
         rangeString = RFmxSpecAnMX.BuildRangeString2("", rangeNumber);

         specAn.Spur.Results.FetchRangeAbsoluteLimitTrace(rangeString, timeout,
                                                          ref absoluteLimitTrace);

         specAn.Spur.Results.FetchRangeSpectrumTrace(rangeString, timeout,
                                                     ref spectrumTrace);

         specAn.Spur.Results.FetchMeasurementStatus("", timeout, out measurementStatus);

      }

      private void PrintResults()
      {
         string status = "Fail";

         Console.WriteLine("------------------Measurement------------------\n");
         if (measurementStatus == RFmxSpecAnMXSpurMeasurementStatus.Pass)
            status = "Pass";
         Console.WriteLine("Measurement Status   : {0}\n", status);

         Console.WriteLine("\nSpur List:\n");
         for (int i = 0; i < NumberOfSpursToReport; i++)
         {
            Console.WriteLine("Spur {0}", i);
            Console.WriteLine("Range Index          : {0}", spurMeasurement.rangeIndex[i]);
            Console.WriteLine("Frequency (Hz)       : {0}", spurMeasurement.frequency[i]);
            Console.WriteLine("Amplitude (dBm)      : {0}", spurMeasurement.amplitude[i]);
            Console.WriteLine("Abosulte Limit (dBm) : {0}", spurMeasurement.absoluteLimit[i]);
            Console.WriteLine("Margin (dB)          : {0}", spurMeasurement.margin[i]);
            Console.WriteLine("--------------------------------------------------------------\n");
         }
      }

      private void CloseSession()
      {
         try
         {
            if (specAn != null)
            {
               specAn.Dispose();
               specAn = null;
            }

            if (instrSession != null)
            {
               instrSession.Close();
               instrSession = null;
            }
         }
         catch (Exception ex)
         {
            DisplayError(ex);
         }
      }

      private void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }

   }
}
