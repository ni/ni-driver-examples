//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure Frequency Reference
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Configure RF Attenuation
//6. Select ACP measurement and enable the traces
//7. Configure ACP Measurement Method, Power Units and Averaging Parameters
//8. Configure ACP RBW filter
//9. Configure ACP Sweep Time
//10. Configure ACP Noise Compensation
//11. Configure ACP Carrier Channel Settings (Integration BW, RRC Filter)
//12. Configure ACP Number of Offset Channels
//13. Configure ACP Offset Channel Settings (Offset Frequencies, Integration BW, RRC Filter)
//Use "offset:all" selector string to set a parameter for all the Offset Channels
//14. Initiate Measurement
//15. Fetch ACP Measurements and Traces
//16. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnAcp
{
   public class RFmxSpecAnAcpArrayExample
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      string resourceName;

      const int NumberOfOffsets = 2;

      string selectedPorts;
      double centerFrequency, referenceLevel, autoSetReferenceLevel, externalAttenuation,
             rbw, frequency, sweepTimeInterval, measurementInterval,
             timeout;
      string frequencySource;
      bool autoLevel;
      RFmxSpecAnMXAcpPowerUnits powerUnits;
      RFmxSpecAnMXAcpNoiseCompensationEnabled noiseCompensationEnabled;
      RFmxSpecAnMXAcpRbwFilterType rbwFilterType;
      RFmxSpecAnMXAcpRbwAutoBandwidth rbwAuto;
      RFmxSpecAnMXAcpSweepTimeAuto sweepTimeAuto;
      int averagingCount;
      RFmxSpecAnMXAcpAveragingEnabled averagingEnabled;
      RFmxSpecAnMXAcpAveragingType averagingType;

      //Input values
      struct CarrierChannel
      {
         public double integrationBandwidth;
         public RFmxSpecAnMXAcpCarrierRrcFilterEnabled rrcFilterEnabled;
         public double rrcFilterAlpha;
      };
      CarrierChannel carrierChannelInput;

      struct OffsetChannel
      {
         public double integrationBandwidth;
         public double[] frequencyOffset;
         public RFmxSpecAnMXAcpOffsetRrcFilterEnabled rrcFilterEnabled;
         public double rrcFilterAlpha;
      };
      OffsetChannel offsetChannelInput;

      //Output values
      double absolutePower;
      double[] lowerRelativePower;
      double[] upperRelativePower;
      double[] lowerAbsolutePower;
      double[] upperAbsolutePower;

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
         centerFrequency = 1e+9;             /* Hz */
         referenceLevel = 0.00;              /* dBm */
         externalAttenuation = 0.00;         /* dB */

         frequencySource = RFmxInstrMXConstants.OnboardClock;
         frequency = 10.0e+6;                /* Hz */

         measurementInterval = 10e-3;        /* seconds */
         autoLevel = true;

         // Carrier Channels
         carrierChannelInput.integrationBandwidth = 1.0e+6;
         carrierChannelInput.rrcFilterEnabled = RFmxSpecAnMXAcpCarrierRrcFilterEnabled.False;
         carrierChannelInput.rrcFilterAlpha = 0.220;

         powerUnits = RFmxSpecAnMXAcpPowerUnits.dBm;
         noiseCompensationEnabled = RFmxSpecAnMXAcpNoiseCompensationEnabled.False;

         // Sweep Time
         sweepTimeAuto = RFmxSpecAnMXAcpSweepTimeAuto.True;
         sweepTimeInterval = 1.00e-3;      /* seconds */

         // RBW Filter
         rbwFilterType = RFmxSpecAnMXAcpRbwFilterType.Gaussian;
         rbwAuto = RFmxSpecAnMXAcpRbwAutoBandwidth.True;
         rbw = 10.0e+3;                     /* Hz */

         // Offset Channels	
         offsetChannelInput.integrationBandwidth = 1.0e+6;  /* Hz */
         offsetChannelInput.rrcFilterEnabled = RFmxSpecAnMXAcpOffsetRrcFilterEnabled.False;
         offsetChannelInput.rrcFilterAlpha = 0.220;

         offsetChannelInput.frequencyOffset = new double[NumberOfOffsets];
         offsetChannelInput.frequencyOffset[0] = 1.0e+6;     /* Hz */
         offsetChannelInput.frequencyOffset[1] = 2.0e+6;     /* Hz */

         //Averaging 
         averagingEnabled = RFmxSpecAnMXAcpAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxSpecAnMXAcpAveragingType.Rms;

         timeout = 10.0;                     /* seconds */
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
         specAn.ConfigureFrequency("", centerFrequency);
         specAn.ConfigureExternalAttenuation("", externalAttenuation);

         if (autoLevel)
         {
            specAn.AutoLevel("", carrierChannelInput.integrationBandwidth, measurementInterval, out autoSetReferenceLevel);
            Console.WriteLine("Reference Level(dBm)                  {0}\n", autoSetReferenceLevel);
         }
         else
         {
            specAn.ConfigureReferenceLevel("", referenceLevel);
         }

         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Acp, true);

         specAn.Acp.Configuration.ConfigurePowerUnits("", powerUnits);
         specAn.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                     averagingType);
         specAn.Acp.Configuration.ConfigureRbwFilter("", rbwAuto, rbw, rbwFilterType);
         specAn.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         specAn.Acp.Configuration.ConfigureNoiseCompensationEnabled("",
                                                                    noiseCompensationEnabled);
         specAn.Acp.Configuration.ConfigureCarrierIntegrationBandwidth("",
                                                                       carrierChannelInput.integrationBandwidth);
         specAn.Acp.Configuration.ConfigureCarrierRrcFilter("",
                                                            carrierChannelInput.rrcFilterEnabled,
                                                            carrierChannelInput.rrcFilterAlpha);

         specAn.Acp.Configuration.ConfigureNumberOfOffsets("", NumberOfOffsets);

         specAn.Acp.Configuration.ConfigureOffsetArray("", offsetChannelInput.frequencyOffset,
                                                       null, null);

         specAn.Acp.Configuration.ConfigureOffsetIntegrationBandwidth("offset::all",
                                                                      offsetChannelInput.integrationBandwidth);

         specAn.Acp.Configuration.ConfigureOffsetRrcFilter("offset::all",
                                                           offsetChannelInput.rrcFilterEnabled,
                                                           offsetChannelInput.rrcFilterAlpha);

         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */

         Spectrum<float> spectrum = null;
         double totalRelativePower, carrierFrequency, integrationBandwidth;

         specAn.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref lowerRelativePower,
                                                        ref upperRelativePower,
                                                        ref lowerAbsolutePower,
                                                        ref upperAbsolutePower);

         specAn.Acp.Results.FetchCarrierMeasurement("", timeout, out absolutePower,
                                                    out totalRelativePower,
                                                    out carrierFrequency,
                                                    out integrationBandwidth);

         specAn.Acp.Results.FetchSpectrum("", timeout, ref spectrum);
      }

      private void PrintResults()
      {
         Console.WriteLine("-----------------Carrier Measurements-----------------\n");
         Console.WriteLine("Absolute Power (dBm or dBm/Hz)       {0}", absolutePower);

         Console.WriteLine("\n--------------Offset Channel Measurements-------------\n");
         for (int i = 0; i < NumberOfOffsets; i++)
         {
            Console.WriteLine("----Offset {0}\n", i);
            Console.WriteLine("Lower Relative Power (dB)            {0}", lowerRelativePower[i]);
            Console.WriteLine("Upper Relative Power (dB)            {0}", upperRelativePower[i]);
            Console.WriteLine("Lower Absolute Power (dBm or dBm/Hz) {0}", lowerAbsolutePower[i]);
            Console.WriteLine("Upper Absolute Power (dBm or dBm/Hz) {0}", upperAbsolutePower[i]);
         }
         Console.WriteLine("-------------------------------------------------\n");
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
