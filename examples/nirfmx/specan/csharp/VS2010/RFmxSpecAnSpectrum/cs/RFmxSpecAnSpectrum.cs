//Steps:
//1. Open a new RFmx session.
//2. Configure the basic instrument properties(Clock Source, Clock Frequency).
//3. Configure Selected Ports.
//4. Configure the Center Frequency, Span or Start, Stop Frequency based on the Tab Selection.
//5. Configure the basic signal properties(Reference Level, External Attenuation).
//6. Select Spectrum measurement and enable the traces.
//7. Configure Measurement Method
//8. Configure RBW Filter Parameters.
//9. Configure Power Units.
//10. Configure Sweep Time.
//11. Configure Spectrum Averaging.
//12. Configure FFT parameters.
//13. Configure Noise Compensation Enabled.
//14. Configure Detectors.
//15. Configure VBW Filter Parameters.
//16. If Measurement Method is Sequential FFT, Configure Sequential FFT Parameters. 
//17. Configure Cleaner Spectrum.
//18. Initiate Measurement.
//19. Fetch Spectrum Traces and Measurements.
//20. Close the RFmx Session.

using System;
using NationalInstruments;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnSpectrum
{
   public class RFmxSpecAnSpectrum
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      string resourceName, frequencySource;
      bool isStartStopFreq;
      double startFrequency, endFrequency;
      string selectedPorts;
      double centerFrequency, referenceLevel, externalAttenuation, frequency,
             rbw, sweepTimeInterval, span, fftPadding, fftOverlapPercent, timeout;
      double vbw, vbwToRbwRatio;
      RFmxInstrMXCleanerSpectrum cleanerSpectrum;
      RFmxSpecAnMXSpectrumMeasurementMethod measurementMethod;
      RFmxSpecAnMXSpectrumPowerUnits powerUnits;
      RFmxSpecAnMXSpectrumNoiseCompensationEnabled noiseCompensationEnabled;
      RFmxSpecAnMXSpectrumRbwFilterType rbwFilterType;
      RFmxSpecAnMXSpectrumRbwAutoBandwidth rbwAuto;
      RFmxSpecAnMXSpectrumSweepTimeAuto sweepTimeAuto;
      RFmxSpecAnMXSpectrumAveragingEnabled averagingEnabled;
      int averagingCount;
      int detectorPoints;
      int sequentialFftSize;
      RFmxSpecAnMXSpectrumAveragingType averagingType;
      RFmxSpecAnMXSpectrumFftWindow fftWindow;
      RFmxSpecAnMXSpectrumFftOverlapMode fftOverlapMode;
      RFmxSpecAnMXSpectrumFftOverlapType fftOverlapType;
      RFmxSpecAnMXSpectrumVbwFilterAutoBandwidth vbwAuto;
      RFmxSpecAnMXSpectrumDetectorType detectorType;

      double peakAmplitude, peakFrequency, frequencyResolution;

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

         isStartStopFreq = false;
         startFrequency = 9.95e+8;       /* Hz */
         endFrequency = 1.005e+9;        /* Hz */

         selectedPorts = "";
         centerFrequency = 1e+9;         /* Hz */
         referenceLevel = 0.00;          /* dBm */
         externalAttenuation = 0.00;     /* dB */

         timeout = 10.0;                 /* seconds */

         frequencySource = RFmxInstrMXConstants.OnboardClock;
         frequency = 10.0e+6;            /* Hz */

         span = 1.0e+6;                  /* Hz */

         measurementMethod = RFmxSpecAnMXSpectrumMeasurementMethod.Normal;
         powerUnits = RFmxSpecAnMXSpectrumPowerUnits.dBm;

         noiseCompensationEnabled = RFmxSpecAnMXSpectrumNoiseCompensationEnabled.False;
         cleanerSpectrum = RFmxInstrMXCleanerSpectrum.Disabled;

         //RBW Filter
         rbwFilterType = RFmxSpecAnMXSpectrumRbwFilterType.Gaussian;
         rbwAuto = RFmxSpecAnMXSpectrumRbwAutoBandwidth.True;
         rbw = 10.0e+3;                  /* Hz */

         vbwAuto = RFmxSpecAnMXSpectrumVbwFilterAutoBandwidth.True;
         vbw = 30.0e3;                   /* Hz */
         vbwToRbwRatio = 3;

         detectorType = RFmxSpecAnMXSpectrumDetectorType.None;
         detectorPoints = 1001;

         //Sweep Time
         sweepTimeAuto = RFmxSpecAnMXSpectrumSweepTimeAuto.True;
         sweepTimeInterval = 1.00e-3;    /* seconds */

         //Averaging
         averagingEnabled = RFmxSpecAnMXSpectrumAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxSpecAnMXSpectrumAveragingType.Rms;

         //FFT
         fftWindow = RFmxSpecAnMXSpectrumFftWindow.FlatTop;
         fftPadding = -1.0;
         fftOverlapMode = RFmxSpecAnMXSpectrumFftOverlapMode.Disabled;
         fftOverlapPercent = 0;
         fftOverlapType = RFmxSpecAnMXSpectrumFftOverlapType.Rms;
         sequentialFftSize = 512;
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
         if (isStartStopFreq)
         {
            specAn.Spectrum.Configuration.ConfigureFrequencyStartStop("", startFrequency, endFrequency);
         }
         else //Configure span
         {
            specAn.ConfigureFrequency("", centerFrequency);
            specAn.Spectrum.Configuration.ConfigureSpan("", span);
         }
         specAn.ConfigureReferenceLevel("", referenceLevel);
         specAn.ConfigureExternalAttenuation("", externalAttenuation);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Spectrum, true);
         specAn.Spectrum.Configuration.ConfigureMeasurementMethod("",measurementMethod);
         specAn.Spectrum.Configuration.ConfigureRbwFilter("", rbwAuto, rbw, rbwFilterType);
         specAn.Spectrum.Configuration.ConfigurePowerUnits("", powerUnits);
         specAn.Spectrum.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         specAn.Spectrum.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                          averagingType);
         specAn.Spectrum.Configuration.ConfigureFft("", fftWindow, fftPadding);
         specAn.Spectrum.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled);
         specAn.Spectrum.Configuration.ConfigureDetector("", detectorType, detectorPoints);
         specAn.Spectrum.Configuration.ConfigureVbwFilter("", vbwAuto, vbw, vbwToRbwRatio);
         specAn.Spectrum.Configuration.SetFftOverlapMode("", fftOverlapMode);
         specAn.Spectrum.Configuration.SetFftOverlap("", fftOverlapPercent);
         specAn.Spectrum.Configuration.SetFftOverlapType("", fftOverlapType);
         specAn.Spectrum.Configuration.SetSequentialFftSize("", sequentialFftSize);
         instrSession.SetCleanerSpectrum("", cleanerSpectrum);
         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */
         Spectrum<float> spectrum = null;
         specAn.Spectrum.Results.FetchSpectrum("", timeout, ref spectrum);

         specAn.Spectrum.Results.FetchMeasurement("", timeout, out peakAmplitude, out peakFrequency,
                                                  out frequencyResolution);
      }

      private void PrintResults()
      {
         Console.WriteLine("Peak Amplitude (dBm)             {0}", peakAmplitude);
         Console.WriteLine("Peak Frequency  (Hz)              {0}", peakFrequency);
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
