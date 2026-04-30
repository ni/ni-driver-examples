//Steps:
//1. Open a new RFmx session
//2. Configure the instrument properties Clock Source and Clock Frequency
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select Spectrum measurement
//6. Configure Spectrum RBW
//7. Configure Spectrum Span
//8. Configure Spectrum Averaging
//9.  Initiate Measurment
//10. Fetch Spectrum Trace
//11. Configure Marker Peak Threshold
//12. Configure Marker Type as Normal
//13. Configure "Spectrum" as the trace to be used by the Marker
//14. Use Marker Peak Search to detect the Number of Peaks in the Spectrum
//15. Fetch XY Location of the Marker
//16. Based on the user selection move the Marker to Next Highest, Next Left and Next Right position
//and Fetch XY Location of the Marker after moving the Marker to new position
//Stop the Loop on Error or if the user has pressed the Stop button
//17. Close the RFmx Session

using System;
using NationalInstruments;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnMarker
{
   class RFmxSpecAnMarker
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      string resourceName, frequencySource;
      string selectedPorts;
      double centerFrequency, referenceLevel, externalAttenuation, frequency,
       span, rbw, timeout;
      RFmxSpecAnMXSpectrumRbwFilterType rbwFilterType;
      RFmxSpecAnMXSpectrumRbwAutoBandwidth rbwAutoBandwidth;
      RFmxSpecAnMXSpectrumAveragingEnabled averagingEnabled;
      RFmxSpecAnMXSpectrumAveragingType averagingType;
      int averagingCount, threshold, numberOfPeaks;
      RFmxSpecAnMXMarkerThresholdEnabled thresholdEnabled;
      RFmxSpecAnMXMarkerPeakExcursionEnabled excursionEnabled;
      double excursion;
      RFmxSpecAnMXMarkerType markerType;
      RFmxSpecAnMXMarkerTrace markerTrace;
      const int NumberOfMarkers = 1;

      double markerXLocation, markerYLocation;
      bool nextPeakFound;
      Spectrum<float> spectrum;

      public void Run()
      {
         try
         {
            InitializeVariables();
            InitializeInstr();
            ConfigureSpecAn();
            RetrieveResults();
            DisplayResults();
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
         /* Intialize input variables */

         resourceName = "RFSA";
         selectedPorts = "";
         centerFrequency = 1e+9;              /* Hz */
         referenceLevel = 0.00;               /* dBm */
         externalAttenuation = 0.00;          /* dB */

         frequencySource = RFmxInstrMXConstants.OnboardClock;
         frequency = 10.0e+6;                 /* Hz */
         span = 1.0e+6;                       /* Hz */

         rbwFilterType = RFmxSpecAnMXSpectrumRbwFilterType.Gaussian;
         rbwAutoBandwidth = RFmxSpecAnMXSpectrumRbwAutoBandwidth.False;
         rbw = 10.0e+3;                       /* Hz */

         //Averaging 
         averagingEnabled = RFmxSpecAnMXSpectrumAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxSpecAnMXSpectrumAveragingType.Rms;

         markerType = RFmxSpecAnMXMarkerType.Normal;
         markerTrace = RFmxSpecAnMXMarkerTrace.Spectrum;

         //Peak threshold
         thresholdEnabled = RFmxSpecAnMXMarkerThresholdEnabled.False;
         threshold = -90;

         //Peak Excursion
         excursionEnabled = RFmxSpecAnMXMarkerPeakExcursionEnabled.False;
         excursion = 6;  /* Rel Units */

         timeout = 10;
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
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);

         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Spectrum, true);
         specAn.Spectrum.Configuration.ConfigureRbwFilter("", rbwAutoBandwidth, rbw,
                                                          rbwFilterType);
         specAn.Spectrum.Configuration.ConfigureSpan("", span);
         specAn.Spectrum.Configuration.ConfigureAveraging("", averagingEnabled,
                                                          averagingCount, averagingType);
         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve Results */

         //Fetch Spectrum data
         specAn.Spectrum.Results.FetchSpectrum("", timeout, ref spectrum);

         //Configure Marker settings            
         specAn.Marker.Configuration.ConfigureThreshold("", thresholdEnabled, threshold);
         specAn.Marker.Configuration.ConfigurePeakExcursion("", excursionEnabled, excursion);
         specAn.Marker.Configuration.ConfigureType("marker0", markerType);
         specAn.Marker.Configuration.ConfigureTrace("marker0", markerTrace);

         specAn.Marker.Results.PeakSearch("marker0", out numberOfPeaks);
         specAn.Marker.Results.FetchXY("marker0", out markerXLocation, out markerYLocation);
      }

      private void DisplayResults()
      {
         ConsoleKeyInfo ch;
         bool exitMenu = false;
         bool donotFetch;
         RFmxSpecAnMXMarkerNextPeak peakToFetch = RFmxSpecAnMXMarkerNextPeak.NextHighest;

         DisplayMarkerData(numberOfPeaks, nextPeakFound, markerXLocation, markerYLocation);
         DisplayMarkerMenu();

         while (true)
         {
            ch = Console.ReadKey(true);
            donotFetch = false;
            switch (ch.KeyChar)
            {
               case 'l':
               case 'L':
                  peakToFetch = RFmxSpecAnMXMarkerNextPeak.NextLeft;
                  break;

               case 'r':
               case 'R':
                  peakToFetch = RFmxSpecAnMXMarkerNextPeak.NextRight;
                  break;

               case 'h':
               case 'H':
                  peakToFetch = RFmxSpecAnMXMarkerNextPeak.NextHighest;
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
               specAn.Marker.Results.NextPeak("marker0", peakToFetch, out nextPeakFound);
               specAn.Marker.Results.FetchXY("marker0", out markerXLocation, out markerYLocation);
               DisplayMarkerData(numberOfPeaks, nextPeakFound, markerXLocation, markerYLocation);
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
         Console.WriteLine("l/L - Next Left\n");
         Console.WriteLine("r/R - Next Right\n");
         Console.WriteLine("h/H - Next Highest\n");
         Console.WriteLine("s/S - Stop and Exit menu\n");
      }

      private void DisplayMarkerData(int numberOfPeaks, bool peakFound, double markerXLocation, double markerYLocation)
      {
         Console.WriteLine("---------------------------------------------------\n");
         Console.WriteLine("Number of peaks          {0}", numberOfPeaks);
         Console.WriteLine("Next Peak found?         {0}", peakFound);
         Console.WriteLine("Marker X Location (Hz)   {0}", markerXLocation);
         Console.WriteLine("Marker Y Location (dBm)  {0}", markerYLocation);
         Console.WriteLine("----------------------------------------------------\n");
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
