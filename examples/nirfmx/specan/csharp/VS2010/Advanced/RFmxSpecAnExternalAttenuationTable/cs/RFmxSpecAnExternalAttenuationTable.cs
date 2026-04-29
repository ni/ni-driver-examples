//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties (Clock Source, Clock Frequency)
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Configure External Attenuation Table
//6. Select SEM measurement and enable the traces
//7. Configure SEM Power Units and Reference Type
//8. Configure Averaging parameters
//9. Configure Integration BW of the carrier
//10. Configure RBW Filter parameters
//11. Configure RRC Filter to be applied on the acquired carrier channel
//12. Configure Number of Offsets
//13. Configure Frequency Ranges of Offset channels
//Use Array API's to configure all offset parameters as an array
//14. Configure Absolute Limit mask for Offset Channels
//15. Configure Relative Limit mask for Offset Channels
//16. Configure RBW Filter parameters for the Offset channels
//17. Configure Offset Limit Fail Mask
//18. Configure Amplitude Correction Type

using System;
using NationalInstruments;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnExternalAttenuationTable
{
   public class RFmxSpecAnExternalAttenuationTable
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;

      string selectedPorts;
      String resourceName;
      string offsetString;
      string portString;
      double centerFrequency, referenceLevel, externalAttenuation;
      String frequencyReferenceSource;
      double frequencyReferenceFrequency;

      const int TableSize = 3;
      const int NumberOfOffsets = 4;

      double[] frequency = new double[TableSize] { 997.0e+6, 1.0e+9, 1.003e+9 };
      double[] attenuation = new double[TableSize] { 0.20, 0.50, 0.30 };

      double integrationBandwidth;

      RFmxSpecAnMXSemCarrierRbwAutoBandwidth rbwAuto;
      RFmxSpecAnMXSemCarrierRbwFilterType rbwFilterType;
      double rbw;

      RFmxSpecAnMXSemCarrierRrcFilterEnabled rrcFilterEnabled;
      double rrcAlpha;

      RFmxSpecAnMXSemAmplitudeCorrectionType amplitudeCorrectionType;

      RFmxSpecAnMXSemAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxSpecAnMXSemAveragingType averagingType;

      RFmxSpecAnMXSemReferenceType referenceType;
      RFmxSpecAnMXSemPowerUnits powerUnits;

      RFmxSpecAnMXSemOffsetEnabled[] offsetEnabled =
          new RFmxSpecAnMXSemOffsetEnabled[NumberOfOffsets]
          {
                RFmxSpecAnMXSemOffsetEnabled.True, RFmxSpecAnMXSemOffsetEnabled.True,
                RFmxSpecAnMXSemOffsetEnabled.True, RFmxSpecAnMXSemOffsetEnabled.True
          };

      RFmxSpecAnMXSemOffsetSideband[] offsetSideband =
          new RFmxSpecAnMXSemOffsetSideband[NumberOfOffsets]
          {
                RFmxSpecAnMXSemOffsetSideband.Both, RFmxSpecAnMXSemOffsetSideband.Both,
                RFmxSpecAnMXSemOffsetSideband.Both, RFmxSpecAnMXSemOffsetSideband.Both
          };

      double[] offsetStartFrequency = new double[NumberOfOffsets] { 2.515e6, 4.000e6, 7.500e6, 8.500e6 };   /* Hz */
      double[] offsetStopFrequency = new double[NumberOfOffsets] { 3.485e6, 7.500e6, 8.500e6, 12.000e6 };   /* Hz */

      RFmxSpecAnMXSemOffsetRbwFilterType[] offsetRbwFilterType =
          new RFmxSpecAnMXSemOffsetRbwFilterType[NumberOfOffsets]
          {
                RFmxSpecAnMXSemOffsetRbwFilterType.Gaussian, RFmxSpecAnMXSemOffsetRbwFilterType.Gaussian,
                RFmxSpecAnMXSemOffsetRbwFilterType.Gaussian, RFmxSpecAnMXSemOffsetRbwFilterType.Gaussian
          };
      RFmxSpecAnMXSemOffsetRbwAutoBandwidth[] offsetRbwAuto =
          new RFmxSpecAnMXSemOffsetRbwAutoBandwidth[NumberOfOffsets]
          {
                RFmxSpecAnMXSemOffsetRbwAutoBandwidth.False, RFmxSpecAnMXSemOffsetRbwAutoBandwidth.False,
                RFmxSpecAnMXSemOffsetRbwAutoBandwidth.False, RFmxSpecAnMXSemOffsetRbwAutoBandwidth.False
          };
      double[] offsetRbw = new double[NumberOfOffsets] { 30.0e3, 500.0e3, 1.000e6, 1.000e6 };              /* Hz */

      RFmxSpecAnMXSemOffsetAbsoluteLimitMode[] absoluteLimitMode =
          new RFmxSpecAnMXSemOffsetAbsoluteLimitMode[NumberOfOffsets]{
            RFmxSpecAnMXSemOffsetAbsoluteLimitMode.Couple, RFmxSpecAnMXSemOffsetAbsoluteLimitMode.Couple,
            RFmxSpecAnMXSemOffsetAbsoluteLimitMode.Couple, RFmxSpecAnMXSemOffsetAbsoluteLimitMode.Couple
      };
      double[] absoluteLimitStart = new double[NumberOfOffsets] { -69.60, -54.30, -54.30, -54.30 };       /* dBm */
      double[] absoluteLimitStop = new double[NumberOfOffsets] { -69.60, -54.30, -54.30, -54.30 };        /* dBm */

      RFmxSpecAnMXSemOffsetRelativeLimitMode[] relativeLimitMode = new RFmxSpecAnMXSemOffsetRelativeLimitMode[NumberOfOffsets]
      {
            RFmxSpecAnMXSemOffsetRelativeLimitMode.Manual, RFmxSpecAnMXSemOffsetRelativeLimitMode.Manual,
            RFmxSpecAnMXSemOffsetRelativeLimitMode.Manual, RFmxSpecAnMXSemOffsetRelativeLimitMode.Couple
      };
      double[] relativeLimitStart = new double[NumberOfOffsets] { -33.73, -34.00, -37.50, -47.50 };       /* dBm */
      double[] relativeLimitStop = new double[NumberOfOffsets] { -48.27, -37.50, -47.50, -47.50 };        /* dBm */

      RFmxSpecAnMXSemOffsetLimitFailMask offsetLimitFailMask;
      double timeout;

      /* Output Variables */

      RFmxSpecAnMXSemCompositeMeasurementStatus compositeMeasurementStatus;
      double carrierAbsolutePower, peakAbsolutePower, peakFrequency, totalRelativePower;

      public double[] lowerOffsetMargin;
      public double[] lowerOffsetMarginAbsolutePower;
      public double[] lowerOffsetMarginRelativePower;
      public double[] lowerOffsetMarginFrequency;
      public RFmxSpecAnMXSemLowerOffsetMeasurementStatus[] lowerOffsetMeasurementStatus;

      public double[] upperOffsetMargin;
      public double[] upperOffsetMarginAbsolutePower;
      public double[] upperOffsetMarginRelativePower;
      public double[] upperOffsetMarginFrequency;
      public RFmxSpecAnMXSemUpperOffsetMeasurementStatus[] upperOffsetMeasurementStatus;

      Spectrum<float> spectrum;
      Spectrum<float> absoluteMask;
      Spectrum<float> relativeMask;

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

         centerFrequency = 1e+9;                                             /* Hz */
         referenceLevel = 0.00;                                              /* dBm */
         externalAttenuation = 0.00;                                         /* dB */

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10.0e+6;                              /* Hz */

         integrationBandwidth = 3.840e+6;

         rbwAuto = RFmxSpecAnMXSemCarrierRbwAutoBandwidth.False;
         rbwFilterType = RFmxSpecAnMXSemCarrierRbwFilterType.Gaussian;
         rbw = 30.0e+3;                                                      /* Hz */

         rrcFilterEnabled = RFmxSpecAnMXSemCarrierRrcFilterEnabled.True;
         rrcAlpha = 0.220;

         amplitudeCorrectionType = RFmxSpecAnMXSemAmplitudeCorrectionType.RFCenterFrequency;

         averagingEnabled = RFmxSpecAnMXSemAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxSpecAnMXSemAveragingType.Rms;

         referenceType = RFmxSpecAnMXSemReferenceType.Integration;
         powerUnits = RFmxSpecAnMXSemPowerUnits.dBm;

         offsetLimitFailMask = RFmxSpecAnMXSemOffsetLimitFailMask.AbsoluteAndRelative;

         absoluteMask = null;
         relativeMask = null;
         spectrum = null;

         timeout = 10.0;                                                     /* seconds */
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
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         specAn.SetSelectedPorts("", selectedPorts);
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         portString = RFmxInstrMX.BuildPortString2("", selectedPorts, "", 0);
         instrSession.ConfigureExternalAttenuationTable(portString, "", frequency, attenuation);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Sem, true);
         specAn.Sem.Configuration.ConfigurePowerUnits("", powerUnits);
         specAn.Sem.Configuration.ConfigureReferenceType("", referenceType);
         specAn.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         specAn.Sem.Configuration.ConfigureCarrierIntegrationBandwidth("", integrationBandwidth);
         specAn.Sem.Configuration.ConfigureCarrierRbwFilter("", rbwAuto, rbw, rbwFilterType);
         specAn.Sem.Configuration.ConfigureCarrierRrcFilter("", rrcFilterEnabled, rrcAlpha);
         specAn.Sem.Configuration.ConfigureNumberOfOffsets("", NumberOfOffsets);
         specAn.Sem.Configuration.ConfigureOffsetFrequencyArray("", offsetStartFrequency,
                                                             offsetStopFrequency, offsetEnabled, offsetSideband);
         specAn.Sem.Configuration.ConfigureOffsetAbsoluteLimitArray("", absoluteLimitMode,
                                             absoluteLimitStart, absoluteLimitStop);
         specAn.Sem.Configuration.ConfigureOffsetRelativeLimitArray("", relativeLimitMode,
                                             relativeLimitStart, relativeLimitStop);
         specAn.Sem.Configuration.ConfigureOffsetRbwFilterArray("", offsetRbwAuto, offsetRbw, offsetRbwFilterType);
         offsetString = RFmxSpecAnMX.BuildOffsetString2("", -1);
         specAn.Sem.Configuration.ConfigureOffsetLimitFailMask(offsetString, offsetLimitFailMask);
         specAn.Sem.Configuration.SetAmplitudeCorrectionType("", amplitudeCorrectionType);
         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */

         specAn.Sem.Results.FetchLowerOffsetMarginArray("", timeout, ref lowerOffsetMeasurementStatus,
                                                                     ref lowerOffsetMargin, ref lowerOffsetMarginFrequency,
                                                                     ref lowerOffsetMarginAbsolutePower, ref lowerOffsetMarginRelativePower);
         specAn.Sem.Results.FetchUpperOffsetMarginArray("", timeout, ref upperOffsetMeasurementStatus,
                                                                     ref upperOffsetMargin, ref upperOffsetMarginFrequency,
                                                                     ref upperOffsetMarginAbsolutePower, ref upperOffsetMarginRelativePower);
         specAn.Sem.Results.FetchCarrierMeasurement("", timeout, out carrierAbsolutePower,
                                                                 out peakAbsolutePower, out peakFrequency, out totalRelativePower);
         specAn.Sem.Results.FetchAbsoluteMaskTrace("", timeout, ref absoluteMask);
         specAn.Sem.Results.FetchRelativeMaskTrace("", timeout, ref relativeMask);
         specAn.Sem.Results.FetchSpectrum("", timeout, ref spectrum);
         specAn.Sem.Results.FetchCompositeMeasurementStatus("", timeout, out compositeMeasurementStatus);
      }

      private void PrintResults()
      {
         Console.WriteLine("Measurement status                     :  {0}", compositeMeasurementStatus);
         Console.WriteLine("Carrier Absolute Power (dBm or dBm/Hz) :  {0}\n", carrierAbsolutePower);

         Console.WriteLine("---------------Lower Offset---------------\n");
         Console.WriteLine("Lower Offset Segment Measurements\n");
         for (int i = 0; i < NumberOfOffsets; i++)
         {
            Console.WriteLine("Offset : {0}\n", i);
            Console.WriteLine("Margin (dB)                            :  {0}", lowerOffsetMargin[i]);
            Console.WriteLine("Margin Absolute Power (dBm)            :  {0}", lowerOffsetMarginAbsolutePower[i]);
            Console.WriteLine("Margin Relative Power (dB)             :  {0}", lowerOffsetMarginRelativePower[i]);
            Console.WriteLine("Margin Frequency (Hz)                  :  {0}", lowerOffsetMarginFrequency[i]);
            Console.WriteLine("Measurement Status                     :  {0}", lowerOffsetMeasurementStatus[i]);
         }

         Console.WriteLine("---------------Upper Offset---------------\n");
         Console.WriteLine("Upper Offset Segment Measurements\n");
         for (int i = 0; i < NumberOfOffsets; i++)
         {
            Console.WriteLine("Offset : {0}\n", i);
            Console.WriteLine("Margin (dB)                            :  {0}", upperOffsetMargin[i]);
            Console.WriteLine("Margin Absolute Power (dBm)            :  {0}", upperOffsetMarginAbsolutePower[i]);
            Console.WriteLine("Margin Relative Power (dB)             :  {0}", upperOffsetMarginRelativePower[i]);
            Console.WriteLine("Margin Frequency (Hz)                  :  {0}", upperOffsetMarginFrequency[i]);
            Console.WriteLine("Measurement Status                     :  {0}", upperOffsetMeasurementStatus[i]);
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
