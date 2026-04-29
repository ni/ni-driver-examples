//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties Clock Source, Clock Frequency
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Reference Level and External Attenuation)
//5. Select Spur measurement and enable the traces
//6. Configure Spur Averaging
//7. Configure Spur FFT Window
//8. Configure Spur Trace Range Index
//9. Configure Spur Number of Ranges
//10. Configure Spur Range List properties:
//Start and Stop Frequency, Relative Attenuation, RBW Filter, Absolute Limit and Number of Spurs to Report using Selector String
//11. Initiate Measurement
//12. Fetch Range Status for all Ranges
//13. Use Number of Detected Spurs and Fetch Spur Measurement Results
//14. Fetch Spur Range Traces for all Ranges
//15. Fetch Measurement Status
//16. Close the RFmx Session

using System;
using NationalInstruments;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnSpurAdvanced
{
    class RFmxSpecAnSpurAdvanced
    {
        RFmxInstrMX instrSession;
        RFmxSpecAnMX specAn;
        String resourceName, frequencyReferenceSource;
      string selectedPorts;
        double referenceLevel, externalAttenuation, centerFrequency, frequencyReferenceFrequency;        
        double timeout;
        int traceRangeIndex;
        int averagingCount;
        Int32 totalSpur = 0;
        RFmxSpecAnMXSpurAveragingEnabled averagingEnabled;
        RFmxSpecAnMXSpurAveragingType averagingType;
        RFmxSpecAnMXSpurFftWindow fftWindow;
        RFmxSpecAnMXSpurMeasurementStatus measurementStatus;
        
        const int NumberOfRangeList = 1;
        const int NumberOfSpursToReport = 10;

        struct RangeList
        {

            public RFmxSpecAnMXSpurRangeEnabled[] enabled;
            public double[] startFrequency;
            public double[] stopFrequency;
            public double[] relativeAttenuation;
            public RFmxSpecAnMXSpurRbwFilterType[] rbwFilterType;
            public RFmxSpecAnMXSpurRbwAutoBandwidth[] rbwFilterAutoBandwidth;
            public RFmxSpecAnMXSpurRangeVbwFilterAutoBandwidth[] vbwAuto;
            public RFmxSpecAnMXSpurRangeDetectorType[] detectorType;
            public double[] rbwFilterBandwidth;
            public RFmxSpecAnMXSpurAbsoluteLimitMode[] absoluteLimitMode;
            public double[] absoluteStartLimit;
            public double[] absoluteStopLimit;
            public double[] peakThreshold;
            public double[] peakExcursion;
            public int[] numberOfSpursToReport;
            public double[] vbw;
            public double[] vbwToRbwRatio;
            public int[] detectorPoints;

            public RangeList(Int32 numOfRangeList)
            {
                enabled = new RFmxSpecAnMXSpurRangeEnabled[numOfRangeList];
                startFrequency = new double[numOfRangeList];
                stopFrequency = new double[numOfRangeList];
                relativeAttenuation = new double[numOfRangeList];
                rbwFilterType = new RFmxSpecAnMXSpurRbwFilterType[numOfRangeList];
                rbwFilterAutoBandwidth = new RFmxSpecAnMXSpurRbwAutoBandwidth[numOfRangeList];
                rbwFilterBandwidth = new double[numOfRangeList];
                absoluteLimitMode = new RFmxSpecAnMXSpurAbsoluteLimitMode[numOfRangeList];
                vbwAuto = new RFmxSpecAnMXSpurRangeVbwFilterAutoBandwidth[numOfRangeList];
                detectorType = new RFmxSpecAnMXSpurRangeDetectorType[numOfRangeList];
                absoluteStartLimit = new double[numOfRangeList];
                absoluteStopLimit = new double[numOfRangeList];
                peakThreshold = new double[numOfRangeList];
                peakExcursion = new double[numOfRangeList];
                numberOfSpursToReport = new int[numOfRangeList];
                vbw = new double[numOfRangeList];
                vbwToRbwRatio = new double[numOfRangeList];
                detectorPoints = new int[numOfRangeList];
            }
        };

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

        struct RangeMeasurement
        {

            public RFmxSpecAnMXSpurRangeStatus[] measurementStatus;
            public int[] detectedSpurs;
            public RangeMeasurement(Int32 numOfRangeList)
            {
                measurementStatus = new RFmxSpecAnMXSpurRangeStatus[numOfRangeList];
                detectedSpurs = new int[numOfRangeList];
            }
        };

        RangeList rangeInput = new RangeList(NumberOfRangeList);
        RangeMeasurement rangeMeasurement = new RangeMeasurement(NumberOfRangeList);
        SpurList spurMeasurement;


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
            centerFrequency = 1e+9;                   /* Hz */
            referenceLevel = 0.00;                    /* dBm */
            externalAttenuation = 0.00;               /* dB */
            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e+6;    /* Hz */    

            //Averaging
            averagingCount = 10;
            averagingEnabled = RFmxSpecAnMXSpurAveragingEnabled.False;
            averagingType = RFmxSpecAnMXSpurAveragingType.Rms;

            //FFT window
            fftWindow = RFmxSpecAnMXSpurFftWindow.FlatTop;

            timeout = 10.0;                           /* seconds */
            
            traceRangeIndex = 0;

            //Range input
            for (int i = 0; i < NumberOfRangeList; i++)
            {
                rangeInput.enabled[i] = RFmxSpecAnMXSpurRangeEnabled.True;
                rangeInput.startFrequency[i] = 1e+9;
                rangeInput.stopFrequency[i] = 1.5e+9;
                rangeInput.relativeAttenuation[i] = 0.00;
                rangeInput.rbwFilterType[i] = RFmxSpecAnMXSpurRbwFilterType.Gaussian;
                rangeInput.rbwFilterAutoBandwidth[i] = RFmxSpecAnMXSpurRbwAutoBandwidth.True;
                rangeInput.rbwFilterBandwidth[i] = 30e+3;
                rangeInput.absoluteLimitMode[i] = RFmxSpecAnMXSpurAbsoluteLimitMode.Couple;
                rangeInput.absoluteStartLimit[i] = -10.00;
                rangeInput.absoluteStopLimit[i] = -10.00;
                rangeInput.peakThreshold[i] = -200.00;
                rangeInput.peakExcursion[i] = 0.0;
                rangeInput.numberOfSpursToReport[i] = NumberOfSpursToReport;
                rangeInput.vbwAuto[i] = RFmxSpecAnMXSpurRangeVbwFilterAutoBandwidth.True;;
                rangeInput.vbw[i] = 30.0e3;           /* Hz */
                rangeInput.vbwToRbwRatio[i] = 3;
                rangeInput.detectorType[i] = RFmxSpecAnMXSpurRangeDetectorType.None;
                rangeInput.detectorPoints[i] = 1001;
            }
        }

        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName,"");
        }
        
        private void ConfigureSpecAn()
        {
            /* Get SpecAn signal */
            specAn = instrSession.GetSpecAnSignalConfiguration();

            /* Configure measurement */            
            
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         specAn.SetSelectedPorts("", selectedPorts);
            specAn.ConfigureFrequency("", centerFrequency);
            specAn.ConfigureReferenceLevel("", referenceLevel);
            specAn.ConfigureExternalAttenuation("", externalAttenuation);
            specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Spur, true);
            specAn.Spur.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                         averagingType);
            specAn.Spur.Configuration.ConfigureFftWindowType("", fftWindow);
            specAn.Spur.Configuration.ConfigureTraceRangeIndex("", traceRangeIndex);
            specAn.Spur.Configuration.ConfigureNumberOfRanges("", NumberOfRangeList);

         specAn.Spur.Configuration.ConfigureRangeFrequencyArray("", rangeInput.startFrequency, rangeInput.stopFrequency,
            rangeInput.enabled);
            specAn.Spur.Configuration.ConfigureRangeRelativeAttenuationArray("", rangeInput.relativeAttenuation);
         specAn.Spur.Configuration.ConfigureRangeRbwArray("", rangeInput.rbwFilterAutoBandwidth, rangeInput.rbwFilterBandwidth,
            rangeInput.rbwFilterType);
         specAn.Spur.Configuration.ConfigureRangeAbsoluteLimitArray("", rangeInput.absoluteLimitMode,
            rangeInput.absoluteStartLimit, rangeInput.absoluteStopLimit);
            specAn.Spur.Configuration.ConfigureRangeNumberOfSpursToReportArray("", rangeInput.numberOfSpursToReport);
            specAn.Spur.Configuration.ConfigureRangePeakCriteriaArray("", rangeInput.peakThreshold, rangeInput.peakExcursion);
            specAn.Spur.Configuration.ConfigureRangeDetectorArray("", rangeInput.detectorType, rangeInput.detectorPoints);
            specAn.Spur.Configuration.ConfigureRangeVbwFilterArray("", rangeInput.vbwAuto, rangeInput.vbw, rangeInput.vbwToRbwRatio);
            specAn.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Retrieve results */
                     
            string rangeString = "";
            Spectrum<float> absoluteLimit = null;
            Spectrum<float> spectrum = null;

            specAn.Spur.Results.FetchRangeStatusArray(rangeString, timeout,
                                                     ref rangeMeasurement.measurementStatus,
                                                     ref rangeMeasurement.detectedSpurs);
            for (int i = 0; i < NumberOfRangeList; i++)
            {
                totalSpur += rangeMeasurement.detectedSpurs[i];
            }
            spurMeasurement = new SpurList(totalSpur);
            specAn.Spur.Results.FetchAllSpurs("", timeout, 
                                            ref spurMeasurement.frequency,
                                            ref spurMeasurement.amplitude, 
                                            ref spurMeasurement.margin, 
                                            ref spurMeasurement.absoluteLimit, 
                                            ref spurMeasurement.rangeIndex);
            
            if (traceRangeIndex == -1)
            {
                traceRangeIndex = 0;
            }
            rangeString = RFmxSpecAnMX.BuildRangeString2("", traceRangeIndex);

            specAn.Spur.Results.FetchRangeAbsoluteLimitTrace(rangeString, timeout, ref absoluteLimit);
                
            specAn.Spur.Results.FetchRangeSpectrumTrace(rangeString, timeout, ref spectrum);

            specAn.Spur.Results.FetchMeasurementStatus("", timeout, out measurementStatus);
        }

        private void PrintResults()
        {
            string status = "Fail";

            Console.WriteLine("----------------Measurement-------------------\n");
            if (measurementStatus == RFmxSpecAnMXSpurMeasurementStatus.Pass)
                status = "Pass";
            Console.WriteLine("Measurement Status: {0}\n", status);

            Console.WriteLine("----------- Spur List-------------------\n");

            for (int i = 0; i < totalSpur; i++)
            {
                status = "Fail";
                if (rangeMeasurement.measurementStatus[spurMeasurement.rangeIndex[i]] == RFmxSpecAnMXSpurRangeStatus.Pass)
                    status = "Pass";
                Console.WriteLine("Spur                      {0}", i + 1);
            Console.WriteLine("Range Measurement Status  {0}", status);
                Console.WriteLine("Range Index               {0}", spurMeasurement.rangeIndex[i]);
                Console.WriteLine("Frequency (Hz)            {0}", spurMeasurement.frequency[i]);
                Console.WriteLine("Amplitude (dBm)           {0}", spurMeasurement.amplitude[i]);
                Console.WriteLine("Absolute Limit (dBm)      {0}", spurMeasurement.absoluteLimit[i]);
                Console.WriteLine("Margin (dB)               {0}", spurMeasurement.margin[i]);
                Console.WriteLine("---------------------------------------");
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

        static private void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }
	
    }
}
