//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the Center Frequency
//4. Configure the basic instrument properties (Clock Source, Clock Frequency)
//5. Configure the basic signal properties  (Reference Level, External Attenuation and RF Attenuation)
//6. Select ACP measurement and enable the traces
//7. Configure ACP Measurement Method, Power Units and Averaging Parameters
//8. Configure ACP FFT
//9. Configure ACP RBW Filter
//10. Configure ACP Sweep Time
//11. Configure ACP Noise Compensation
//12. Configure ACP Number of Carrier Channels
//13. Configure ACP Carrier Channel Settings (Integration BW, Carrier Mode, RRC Filter, Carrier Offset)
//14. Configure ACP Number of Offset Channels
//15. Configure ACP Offset Channel Settings (Integration BW, Offset Frequency, Offset Power Reference,
//    Relative Attenuation, RRC Filter)
//16. Initiate Measurement
//17. Fetch ACP Measurements and Traces
//18. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnAcpAdvanced
{
   struct CarrierChannel
   {
      public RFmxSpecAnMXAcpCarrierMode mode;
      public double carrierFrequency;
      public double integrationBandwidth;
      public RFmxSpecAnMXAcpCarrierRrcFilterEnabled rrcFilterEnabled;
      public double rrcFilterAlpha;
   };

   struct CarrierMeasurement
   {
      public double absolutePower;
      public double totalRelativePower;
      public double resCarrierFrequency;
      public double resIntegrationBandwidth;
   };

   struct OffsetChannel
   {
      public RFmxSpecAnMXAcpOffsetEnabled[] enabled;
      public RFmxSpecAnMXAcpOffsetSideband[] frequencySideband;
      public double[] frequencyOffset;
      public RFmxSpecAnMXAcpOffsetPowerReferenceCarrier[] referenceCarrier;
      public int[] referenceSpecific;
      public RFmxSpecAnMXAcpOffsetRrcFilterEnabled[] rrcFilterEnabled;
      public double[] integrationBandwidth;
      public double[] relativeAttenuation;
      public double[] rrcFilterAlpha;
      public RFmxSpecAnMXAcpOffsetFrequencyDefinition[] frequencyDefinition;

      public OffsetChannel(Int32 numOfOffsets)
      {
         enabled = new RFmxSpecAnMXAcpOffsetEnabled[numOfOffsets];
         frequencySideband = new RFmxSpecAnMXAcpOffsetSideband[numOfOffsets];
         frequencyOffset = new double[numOfOffsets];
         referenceCarrier = new RFmxSpecAnMXAcpOffsetPowerReferenceCarrier[numOfOffsets];
         referenceSpecific = new int[numOfOffsets];
         rrcFilterEnabled = new RFmxSpecAnMXAcpOffsetRrcFilterEnabled[numOfOffsets];
         integrationBandwidth = new double[numOfOffsets];
         relativeAttenuation = new double[numOfOffsets];
         rrcFilterAlpha = new double[numOfOffsets];
         frequencyDefinition = new RFmxSpecAnMXAcpOffsetFrequencyDefinition[numOfOffsets];
      }
   };

   struct OffsetChannelMeasurement
   {
      public double[] lowerRelativePower;
      public double[] upperRelativePower;
      public double[] lowerAbsolutePower;
      public double[] upperAbsolutePower;
      public OffsetChannelMeasurement(int numOfOffsets)
      {
         lowerRelativePower = new double[numOfOffsets];
         upperRelativePower = new double[numOfOffsets];
         lowerAbsolutePower = new double[numOfOffsets];
         upperAbsolutePower = new double[numOfOffsets];
      }
   };

   class RFmxSpecAnAcpAdvanced
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      string resourceName, frequencySource;
      string selectedPorts;
      double centerFrequency, referenceLevel, externalAttenuation, rfAttenuation, frequency;
      int averagingCount;
      RFmxSpecAnMXAcpNoiseCompensationEnabled noiseCompensationEnabled;
      RFmxSpecAnMXAcpFftWindow fftWindow;
      RFmxSpecAnMXAcpRbwAutoBandwidth rbwAuto;
      RFmxSpecAnMXAcpRbwFilterType rbwFilterType;
      RFmxSpecAnMXAcpSweepTimeAuto sweepTimeAuto;
      double rbw, sweepTimeInterval, fftPadding, totalCarrierPower, timeout;
      RFmxSpecAnMXAcpAveragingEnabled averagingEnabled;
      RFmxSpecAnMXAcpAveragingType averagingType;
      RFmxSpecAnMXAcpMeasurementMethod measurementMethod;
      private bool enableAllTraces = true;
      RFmxInstrMXRFAttenuationAuto rfAttenuationAuto;
      const int NumberOfCarriers = 1;
      const int NumberOfOffsets = 2;
      RFmxSpecAnMXAcpPowerUnits powerUnits;
      CarrierChannel[] carrierChannelInput = new CarrierChannel[NumberOfCarriers];
      CarrierMeasurement[] carrierChannelOutput = new CarrierMeasurement[NumberOfCarriers];
      OffsetChannel offsetChannelInput = new OffsetChannel(NumberOfOffsets);
      OffsetChannelMeasurement offsetChannelOutput = new OffsetChannelMeasurement(NumberOfOffsets);

      internal void Run()
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
         /* Initialize input variables */
         resourceName = "RFSA";
         selectedPorts = "";
         centerFrequency = 1e+9;                             /* Hz */
         referenceLevel = 0.00;                              /* dBm */
         externalAttenuation = 0.00;                         /* dB */
         timeout = 10.0;                                     /* seconds */

         rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.True;
         rfAttenuation = 10.00;                              /* dB */

         frequencySource = RFmxInstrMXConstants.OnboardClock;
         frequency = 10.0e+6;                                /* Hz */

         powerUnits = RFmxSpecAnMXAcpPowerUnits.dBm;
         measurementMethod = RFmxSpecAnMXAcpMeasurementMethod.Normal;
         noiseCompensationEnabled = RFmxSpecAnMXAcpNoiseCompensationEnabled.False;

         // Sweep Time
         sweepTimeAuto = RFmxSpecAnMXAcpSweepTimeAuto.True;
         sweepTimeInterval = 1.00e-3;                    /* seconds */

         // RBW Filter
         rbwFilterType = RFmxSpecAnMXAcpRbwFilterType.Gaussian;
         rbwAuto = RFmxSpecAnMXAcpRbwAutoBandwidth.True;
         rbw = 10.0e+3;                                  /* Hz */

         // FFT
         fftWindow = RFmxSpecAnMXAcpFftWindow.FlatTop;
         fftPadding = -1.0;

         //Averaging 
         averagingEnabled = RFmxSpecAnMXAcpAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxSpecAnMXAcpAveragingType.Rms;

         // Carrier Channels
         for (int i = 0; i < NumberOfCarriers; i++)
         {
            carrierChannelInput[i].mode = RFmxSpecAnMXAcpCarrierMode.Active;
            carrierChannelInput[i].carrierFrequency = 0.000;
            carrierChannelInput[i].integrationBandwidth = 1.0e+6;   /* Hz */
            carrierChannelInput[i].rrcFilterEnabled = RFmxSpecAnMXAcpCarrierRrcFilterEnabled.False;
            carrierChannelInput[i].rrcFilterAlpha = 0.220;
         }

         // Offset Channels
         for (int i = 0; i < NumberOfOffsets; i++)
         {
            offsetChannelInput.enabled[i] = RFmxSpecAnMXAcpOffsetEnabled.True;
            if (i == 0) /* For offset 0, set frequency offset = 1 MHz */
               offsetChannelInput.frequencyOffset[i] = 1.0e+6;
            else  /* For offset 1, set frequency offset = 2 MHz */
               offsetChannelInput.frequencyOffset[i] = 2.0e+6;
            offsetChannelInput.frequencySideband[i] = RFmxSpecAnMXAcpOffsetSideband.Both;
            offsetChannelInput.referenceCarrier[i] = RFmxSpecAnMXAcpOffsetPowerReferenceCarrier.Closest;
            offsetChannelInput.referenceSpecific[i] = 0;
            offsetChannelInput.integrationBandwidth[i] = 1.0e+6;
            offsetChannelInput.relativeAttenuation[i] = 0.00;
            offsetChannelInput.rrcFilterEnabled[i] = RFmxSpecAnMXAcpOffsetRrcFilterEnabled.False;
            offsetChannelInput.rrcFilterAlpha[i] = 0.220;
            offsetChannelInput.frequencyDefinition[i] = RFmxSpecAnMXAcpOffsetFrequencyDefinition.CarrierCenterToOffsetCenter;
         }
      }

      private void InitializeInstr()
      {
         /* Create a new RFmx Session */
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      private void ConfigureSpecAn()
      {
         string offsetString, carrierString;

         /* Get SpecAn signal */
         specAn = instrSession.GetSpecAnSignalConfiguration();

         /* Configure measurement */
         instrSession.ConfigureFrequencyReference("", frequencySource, frequency);
         specAn.SetSelectedPorts("", selectedPorts);
         specAn.ConfigureFrequency("", centerFrequency);
         specAn.ConfigureReferenceLevel("", referenceLevel);
         specAn.ConfigureExternalAttenuation("", externalAttenuation);
         instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Acp, enableAllTraces);
         specAn.Acp.Configuration.ConfigureMeasurementMethod("", measurementMethod);
         specAn.Acp.Configuration.ConfigurePowerUnits("", powerUnits);
         specAn.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         specAn.Acp.Configuration.ConfigureFft("", fftWindow, fftPadding);
         specAn.Acp.Configuration.ConfigureRbwFilter("", rbwAuto, rbw, rbwFilterType);
         specAn.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         specAn.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled);

         specAn.Acp.Configuration.ConfigureNumberOfCarriers("", NumberOfCarriers);
         for (int i = 0; i < NumberOfCarriers; i++)
         {
            carrierString = RFmxSpecAnMX.BuildCarrierString2("", i);
            specAn.Acp.Configuration.ConfigureCarrierIntegrationBandwidth(carrierString,
                                                                          carrierChannelInput[i].integrationBandwidth);
            specAn.Acp.Configuration.ConfigureCarrierMode(carrierString,
                                                          carrierChannelInput[i].mode);
            specAn.Acp.Configuration.ConfigureCarrierRrcFilter(carrierString,
                                                               carrierChannelInput[i].rrcFilterEnabled,
                                                               carrierChannelInput[i].rrcFilterAlpha);
            specAn.Acp.Configuration.ConfigureCarrierFrequency(carrierString, carrierChannelInput[i].carrierFrequency);
         }
         specAn.Acp.Configuration.ConfigureNumberOfOffsets("", NumberOfOffsets);

         for (int i = 0; i < NumberOfOffsets; i++)
         {
            offsetString = RFmxSpecAnMX.BuildOffsetString2("", i);
            specAn.Acp.Configuration.ConfigureOffsetFrequencyDefinition(offsetString, offsetChannelInput.frequencyDefinition[i]);
         }

         specAn.Acp.Configuration.ConfigureOffsetArray("", offsetChannelInput.frequencyOffset, offsetChannelInput.frequencySideband,
                                                       offsetChannelInput.enabled);
         specAn.Acp.Configuration.ConfigureOffsetIntegrationBandwidthArray("", offsetChannelInput.integrationBandwidth);
         specAn.Acp.Configuration.ConfigureOffsetPowerReferenceArray("", offsetChannelInput.referenceCarrier,
                                                                     offsetChannelInput.referenceSpecific);
         specAn.Acp.Configuration.ConfigureOffsetRelativeAttenuationArray("", offsetChannelInput.relativeAttenuation);
         specAn.Acp.Configuration.ConfigureOffsetRrcFilterArray("", offsetChannelInput.rrcFilterEnabled,
                                                                offsetChannelInput.rrcFilterAlpha);

         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */

         string carrierString;
         Spectrum<float> spectrum = null;


         specAn.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref offsetChannelOutput.lowerRelativePower,
                                                        ref offsetChannelOutput.upperRelativePower,
                                                        ref offsetChannelOutput.lowerAbsolutePower,
                                                        ref offsetChannelOutput.upperAbsolutePower);

         for (int i = 0; i < NumberOfCarriers; i++)
         {
            carrierString = RFmxSpecAnMX.BuildCarrierString2("", i);
            specAn.Acp.Results.FetchCarrierMeasurement(carrierString, timeout, out carrierChannelOutput[i].absolutePower,
                                                       out carrierChannelOutput[i].totalRelativePower,
                out carrierChannelOutput[i].resCarrierFrequency, out carrierChannelOutput[i].resIntegrationBandwidth);
         }
         specAn.Acp.Results.FetchTotalCarrierPower("", timeout, out totalCarrierPower);

         specAn.Acp.Results.FetchSpectrum("", timeout, ref spectrum);
      }

      private void PrintResults()
      {
         /* Display the results */

         Console.WriteLine("Total Carrier Power (dBm or dBm/Hz)  {0}\n", totalCarrierPower);
         Console.WriteLine("Carrier Measurements: \n");
         for (int i = 0; i < NumberOfCarriers; i++)
         {
            Console.WriteLine("Carrier {0}:", i);
            Console.WriteLine("Abosulte Power (dBm or dBm/Hz)       {0}", carrierChannelOutput[i].absolutePower);
            Console.WriteLine("Total Relative Power (dB)            {0}", carrierChannelOutput[i].totalRelativePower);
            Console.WriteLine("Carrier Offset (Hz)                  {0}", carrierChannelOutput[i].resCarrierFrequency);
            Console.WriteLine("Integration Bandwidth (Hz)           {0}", carrierChannelOutput[i].resIntegrationBandwidth);
            Console.WriteLine("---------------------------------------------------\n");
         }

         Console.WriteLine("Offset Channel Measurements: \n");
         for (int i = 0; i < NumberOfOffsets; i++)
         {
            Console.WriteLine("Offset {0}:", i);
            Console.WriteLine("Lower Relative Power (dB)            {0}", offsetChannelOutput.lowerRelativePower[i]);
            Console.WriteLine("Upper Relative Power (dB)            {0}", offsetChannelOutput.upperRelativePower[i]);
            Console.WriteLine("Lower Absolute Power (dBm or dBm/Hz) {0}", offsetChannelOutput.lowerAbsolutePower[i]);
            Console.WriteLine("Upper Absolute Power (dBm or dBm/Hz) {0}", offsetChannelOutput.upperAbsolutePower[i]);
            Console.WriteLine("-------------------------------------------------\n");
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
