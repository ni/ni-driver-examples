//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure sweep settings: Frequency List, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
//4. Configure Averaging.
//5. Select S-Parameter measurement.
//6. Configure S-Parameter and Format.
//7. Configure Magnitude Units & Phase Trace Type.
//8. Initiate the Measurement.
//9. Read X-Axis Values (aggregated frequency list).
//10. Fetch S-Parameter Y data.
//11. Configure Marker Type as Normal.
//12. Configure Marker Data Source as 'sparam0' to be used by the Marker.
//13. Configure Marker Peak Threshold.
//14. Configure Marker Peak Excursion. 
//15. Perform Peak Search on the configured data source.
//16. Fetch X value of the Marker.
//17. Fetch Y value of the Marker.
//18. Based on the user selection move the Marker to Next Peak, Next Left Peak and Next Right Peak  position and Fetch X and Y value of the Marker after moving the Marker to new position. Stop the Loop on Error or if the user has pressed the Stop button.
//19. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVnaMarker
{
    public class RFmxVnaMarker
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna;
        string resourceName;

        double startFrequency;
        double stopFrequency;
        int numberOfPoints;
        double port1PowerLevel;
        double port2PowerLevel;
        double port1TestReceiverAttenuation;
        double port2TestReceiverAttenuation;
        double IFBandwidth;

        string sParamsSParameters;
        RFmxVnaMXSParamsFormat sParamsFormats;

        RFmxVnaMXAveragingEnabled averagingEnabled;
        int averagingCount;

        string portSelectorString;

        double timeout;

        double[] frequencyListResult;
        float[] sParamsY1DataResult;
        float[] sParamsY2DataResult;

        RFmxVnaMXMarkerPeakSearchThresholdEnabled thresholdEnabled;
        RFmxVnaMXMarkerPeakSearchExcursionEnabled excursionEnabled;
        double threshold, excursion;
        double markerX, markerY1, markerY2;

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureVna();
                RetrieveResults();
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
            finally
            {
                /* Close session */
                CloseSession();
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        void InitializeVariables()
        {
            resourceName = "VNA";

            startFrequency = 1e9;                                                                /* (Hz) */
            stopFrequency = 26e9;                                                                /* (Hz) */
            numberOfPoints = 251;
            port1PowerLevel = -10.0;                                                             /* (dBm) */
            port2PowerLevel = -10.0;                                                             /* (dBm) */
            port1TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            port2TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            IFBandwidth = 100.0e3;                                                               /* (Hz) */

            sParamsSParameters = "S11";
            sParamsFormats = RFmxVnaMXSParamsFormat.Magnitude;

            averagingEnabled = RFmxVnaMXAveragingEnabled.False;
            averagingCount = 10;

            //Peak threshold
            thresholdEnabled = RFmxVnaMXMarkerPeakSearchThresholdEnabled.False;
            threshold = -100;

            //Peak Excursion
            excursionEnabled = RFmxVnaMXMarkerPeakSearchExcursionEnabled.False;
            excursion = 3;

            timeout = 10.0;                                                                      /*seconds */
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureVna()
        {
            vna = instrSession.GetVnaSignalConfiguration();                                      /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", RFmxInstrMXConstants.PxiClock, 100e6);
            vna.SetSweepType("", RFmxVnaMXSweepType.Linear);
            vna.SetStartFrequency("", startFrequency);
            vna.SetStopFrequency("", stopFrequency);
            vna.SetNumberOfPoints("", numberOfPoints);
            vna.SetIFBandwidth("", IFBandwidth);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1");
            vna.SetPowerLevel(portSelectorString, port1PowerLevel);
            vna.SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2");
            vna.SetPowerLevel(portSelectorString, port2PowerLevel);
            vna.SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation);
            vna.SetAveragingEnabled("", averagingEnabled);
            vna.SetAveragingCount("", averagingCount);
            vna.SelectMeasurements("", RFmxVnaMXMeasurementTypes.SParams, false);
            
            vna.SParams.Configuration.ConfigureSParameter("", sParamsSParameters);
            vna.SParams.Configuration.SetFormat("", sParamsFormats);
            
            vna.SParams.Configuration.SetMagnitudeUnits("", RFmxVnaMXSParamsMagnitudeUnits.dB);
            vna.SParams.Configuration.SetPhaseTraceType("", RFmxVnaMXSParamsPhaseTraceType.Wrapped);
            vna.Initiate("", "");
        }

        void RetrieveResults()
        {

            vna.SParams.Results.FetchXData("", timeout, ref frequencyListResult);
            vna.SParams.Results.FetchYData("", timeout, ref sParamsY1DataResult, ref sParamsY2DataResult);
            
            //Configure Marker settings    
            vna.Marker.Configuration.ConfigureType("", RFmxVnaMXMarkerType.Normal);
            vna.Marker.Configuration.ConfigureDataSource("", "sparam0");        
            vna.Marker.Configuration.ConfigurePeakSearchThreshold("", thresholdEnabled, threshold);
            vna.Marker.Configuration.ConfigurePeakSearchExcursion("", excursionEnabled, excursion);

            vna.Marker.Results.MarkerSearch("", RFmxVnaMXMarkerSearchMode.Peak);

            //Fetch Marker Results
            vna.Marker.Results.FetchX("", out markerX);
            vna.Marker.Results.FetchY("", out markerY1, out markerY2);
        }

        private void DisplayResults()
        {
            ConsoleKeyInfo ch;
            bool exitMenu = false;
            bool donotFetch;
            RFmxVnaMXMarkerSearchMode peakToFetch = RFmxVnaMXMarkerSearchMode.NextPeak;

            DisplayMarkerData(markerX, markerY1, markerY2);
            DisplayMarkerMenu();

            while (true)
            {
                ch = Console.ReadKey(true);
                donotFetch = false;
                switch (ch.KeyChar)
                {
                    case 'l':
                    case 'L':
                        peakToFetch = RFmxVnaMXMarkerSearchMode.NextLeftPeak;
                        break;

                    case 'r':
                    case 'R':
                        peakToFetch = RFmxVnaMXMarkerSearchMode.NextRightPeak;
                        break;

                    case 'h':
                    case 'H':
                        peakToFetch = RFmxVnaMXMarkerSearchMode.NextPeak;
                        break;

                    case 's':
                    case 'S':
                        exitMenu = true;
                        donotFetch = true;
                        break;
                    default:
                        donotFetch = true;
                        DisplayMarkerMenu();
                        break;
                }

                if (!donotFetch)//Fetch marker data
                {
                    vna.Marker.Results.MarkerSearch("", peakToFetch);
                    vna.Marker.Results.FetchX("", out markerX);
                    vna.Marker.Results.FetchY("", out markerY1, out markerY2);
                    DisplayMarkerData(markerX, markerY1, markerY2);
                }

                if (exitMenu)
                {
                    Console.WriteLine("Exiting menu.. ");
                    break;
                }
            }
        }
        private void DisplayMarkerMenu()
        {
            Console.WriteLine("To read different peaks, use the following keys\n");
            Console.WriteLine("l/L - Next Left Peak\n");
            Console.WriteLine("r/R - Next Right Peak\n");
            Console.WriteLine("h/H - Next Peak\n");
            Console.WriteLine("s/S - Stop and Exit menu\n");
        }


        private void DisplayMarkerData(double markerX, double markerY1, double markerY2)
        {
            Console.WriteLine("---------------------------------------------------\n");
            Console.WriteLine("Marker X", markerX);
            Console.WriteLine("Marker Y1", markerY1);
            Console.WriteLine("Marker Y2", markerY2);
            Console.WriteLine("----------------------------------------------------\n");
        }

        void CloseSession()
        {
            if (vna != null)
            {
                vna.Dispose();
                vna = null;
            }
            if (instrSession != null)
            {
                instrSession.Close();
                instrSession = null;
            }
        }

        static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }
    }
}
