//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure UARFCN Band.
//6. Select SEM measurement and enable Traces.
//7. Configure Sweep Time Parameters.
//8. Configure Averaging Parameters for SEM measurement.
//9. Initiate the Measurement.
//10. Fetch SEM Measurements and Traces.
//11. Close RFmx Session. 
using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WcdmaMX;

namespace NationalInstruments.Examples.RFmxWcdmaSemSingleCarrier
{
   public class RFmxWcdmaSemSingleCarrier
   {
      RFmxInstrMX instrSession;
      RFmxWcdmaMX wcdma;

      string resourceName = "RFSA";

      RFmxWcdmaMXMeasurementTypes measurement = RFmxWcdmaMXMeasurementTypes.Sem;

      string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
      double frequencyReferenceFrequency = 10.0e+6;                           /* Hz */

      double centerFrequency = 1.95e+9;                                       /* Hz */
      double externalAttenuation = 0.0;                                       /* dB */
      double referenceLevel = 0.0;                                            /* dBm */

      string digitalEdgeSource = RFmxWcdmaMXConstants.Pfi0;
      RFmxWcdmaMXDigitalEdgeTriggerEdge digitalEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising;
      double triggerDelay = 0.0;                                              /* seconds */

      RFmxWcdmaMXSemAveragingEnabled averagingEnabled = RFmxWcdmaMXSemAveragingEnabled.False;
      int averagingCount = 10;
      RFmxWcdmaMXSemAveragingType averagingType = RFmxWcdmaMXSemAveragingType.Rms;

      RFmxWcdmaMXSemSweepTimeAuto sweepTimeAuto = RFmxWcdmaMXSemSweepTimeAuto.True;
      double sweepTimeInterval = 6.6667e-8;                                   /* seconds */
      double timeout = 10;                                                    /* seconds */

      bool enableAllTraces = true;
      bool enableTrigger = false;

      double absoluteIntegratedPower;
      Spectrum<float> spectrum;
      Spectrum<float> absoluteMask;
      Spectrum<float> relativeMask;
      int band = 1;
      RFmxWcdmaMXSemLowerOffsetMeasurementStatus[] lowerOffsetMeasurementStatus;
      double[] lowerOffsetMargin;
      double[] lowerOffsetMarginFrequency;
      double[] lowerOffsetMarginAbsolutePower;
      double[] lowerOffsetMarginRelativePower;

      RFmxWcdmaMXSemUpperOffsetMeasurementStatus[] upperOffsetMeasurementStatus;
      double[] upperOffsetMargin;
      double[] upperOffsetMarginFrequency;
      double[] upperOffsetMarginAbsolutePower;
      double[] upperOffsetMarginRelativePower;
      RFmxWcdmaMXSemMeasurementStatus measurementStatus;
      double relativeIntegratedPower;

      public void Run()
      {
         try
         {
            InitializeInstr();
            ConfigureWcdma();
            RetrieveResults();
            PrintResults();
         }
         catch (Exception ex)
         {
            DisplayError(ex);
         }
         finally
         {
            CloseSession();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
         }
      }

      void InitializeInstr()
      {
         /* Create a new RFmx Session */
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      void ConfigureWcdma()
      {
         wcdma = instrSession.GetWcdmaSignalConfiguration();
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         wcdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
         wcdma.ConfigureBand("", band);
         wcdma.SelectMeasurements("", measurement, enableAllTraces);
         wcdma.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         wcdma.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
      }

      void RetrieveResults()
      {
         wcdma.Initiate("", "");
         wcdma.Sem.Results.FetchLowerOffsetMarginArray("", timeout, ref lowerOffsetMeasurementStatus,
              ref lowerOffsetMargin, ref lowerOffsetMarginFrequency, ref lowerOffsetMarginAbsolutePower, ref lowerOffsetMarginRelativePower);
         wcdma.Sem.Results.FetchUpperOffsetMarginArray("", timeout, ref upperOffsetMeasurementStatus,
                          ref upperOffsetMargin, ref upperOffsetMarginFrequency, ref upperOffsetMarginAbsolutePower, ref upperOffsetMarginRelativePower);
         wcdma.Sem.Results.FetchMeasurementStatus("", timeout, out measurementStatus);
         wcdma.Sem.Results.FetchCarrierMeasurement("", timeout, out absoluteIntegratedPower, out relativeIntegratedPower);
         wcdma.Sem.Results.FetchSpectrum("", timeout, ref spectrum, ref relativeMask, ref absoluteMask);
      }

      void PrintResults()
      {
         Console.WriteLine("Measurement Status                          : {0}", measurementStatus);
         Console.WriteLine("Carrier Absolute Integrated Power (dBm)     : {0}", absoluteIntegratedPower);
         Console.WriteLine("\nLower Offset Segment Measurements\n");
         for (int i = 0; i < lowerOffsetMargin.Length; i++)
         {
            Console.WriteLine("\nMeasurement                         : {0}", i);
            Console.WriteLine("Margin (dB)                         : {0}", lowerOffsetMargin[i]);
            Console.WriteLine("Margin Absolute Power (dBm)         : {0}", lowerOffsetMarginAbsolutePower[i]);
            Console.WriteLine("Margin Relative Power (dB)          : {0}", lowerOffsetMarginRelativePower[i]);
            Console.WriteLine("Margin Frequency (Hz)               : {0}", lowerOffsetMarginFrequency[i]);
            Console.WriteLine("Measurement Status                  : {0}", lowerOffsetMeasurementStatus[i]);
         }

         Console.WriteLine("\nUpper Offset Segment Measurements\n");
         for (int i = 0; i < upperOffsetMargin.Length; i++)
         {
            Console.WriteLine("\nMeasurement                         : {0}", i);
            Console.WriteLine("Margin (dB)                         : {0}", upperOffsetMargin[i]);
            Console.WriteLine("Margin Absolute Power (dBm)         : {0}", upperOffsetMarginAbsolutePower[i]);
            Console.WriteLine("Margin Relative Power (dB)          : {0}", upperOffsetMarginRelativePower[i]);
            Console.WriteLine("Margin Frequency (Hz)               : {0}", upperOffsetMarginFrequency[i]);
            Console.WriteLine("Measurement Status                  : {0}", upperOffsetMeasurementStatus[i]);
         }

      }

      void CloseSession()
      {
         if (wcdma != null)
         {
            wcdma.Dispose();
            wcdma = null;
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
