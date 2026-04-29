//Steps:
//1. Open a new RFmx session
//2. Create WCDMA Signal
//3. Configure Selected Ports for WCDMA Signal
//4. Configure the basic signal properties for WCDMA Signal (Center Frequency, Reference Level and External Attenuation)
//5. Configure ACP Averaging for WCDMA Signal
//6. Configure ACP Integration BW, Number of Offset Channels, Channel Spacing for WCDMA Signal
//7. Create LTE Signal
//8. Configure Selected Ports for LTE Signal
//9. Configure the basic signal properties for LTE Signal (Center Frequency, Reference Level and External Attenuation)
//10. Configure ACP Averaging for LTE Signal
//11. Configure ACP Number of Carriers = "1" for LTE Signal
//12. Configure ACP Carrier Integration BW for LTE Signal
//13. Configure ACP Number of Offsets for LTE Signal
//14. Configure ACP Offset Integration BW, Offset Frequency, Sidebands and RRC Filter for LTE Signal
//15. Read ACP Measurements for WCDMA Signal
//16. Read ACP Measurements for LTE Signal
//17. Delete WCDMA Signal
//18. Delete LTE Signal
//19. Close the RFmx session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnMultipleSignalsAcp
{
   public class RFmxSpecAnMultipleSignalsAcp
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX wcdmaSignal, lteSignal;
      string resourceName;
      string selectedPorts;
      double referenceLevel, externalAttenuation, timeout, wcdmaCenterFrequency,
             wcdmaIntegrationBandwidth, wcdmaChannelSpacing, lteCenterFrequency,
             lteIntegrationBandwidth;
      int wcdmaNumberOfOffsetChannels, wcdmaAveragingCount, lteAveragingCount;
      RFmxSpecAnMXAcpAveragingEnabled wcdmaAveragingEnabled, lteAveragingEnabled;
      RFmxSpecAnMXAcpAveragingType wcdmaAveragingType, lteAveragingType;

      const int NumberOfOffsetChannels = 2;
      const int NumberOfCarriers = 1;

      RFmxSpecAnMXAcpOffsetEnabled[] offsetChannelEnabled = new RFmxSpecAnMXAcpOffsetEnabled[NumberOfOffsetChannels];
      double[] offsetChannelOffset = new double[NumberOfOffsetChannels];
      RFmxSpecAnMXAcpOffsetSideband[] offsetChannelSideband = new RFmxSpecAnMXAcpOffsetSideband[NumberOfOffsetChannels];
      double[] offsetChannelIntegrationBandwidth = new double[NumberOfOffsetChannels];
      RFmxSpecAnMXAcpOffsetRrcFilterEnabled[] offsetChannelRrcFilterEnabled = new RFmxSpecAnMXAcpOffsetRrcFilterEnabled[NumberOfOffsetChannels];
      double[] offsetChannelRrcFilterAlpha = new double[NumberOfOffsetChannels];

      double wcdmaCarrierAbsolutePower, wcdmaOffCh0LowerRelativePower,
             wcdmaOffCh0UpperRelativePower, wcdmaOffCh1LowerRelativePower,
             wcdmaOffCh1UpperRelativePower;
      double lteCarrierAbsolutePower, lteOffCh0LowerRelativePower,
             lteOffCh0UpperRelativePower, lteOffCh1LowerRelativePower,
             lteOffCh1UpperRelativePower;

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
         /* Initialize input variables */

         resourceName = "RFSA";
         selectedPorts = "";
         referenceLevel = 0.00;                  /* dBm */
         externalAttenuation = 0.00;             /* dB */
         timeout = 10.0;                         /* seconds */

         /* WCDMA signal settings */
         wcdmaCenterFrequency = 468.0e+6;        /* Hz */
         wcdmaIntegrationBandwidth = 3.840e+6;   /* Hz */
         wcdmaNumberOfOffsetChannels = NumberOfOffsetChannels;
         wcdmaChannelSpacing = 5.0e+6;           /* Hz */
         wcdmaAveragingEnabled = RFmxSpecAnMXAcpAveragingEnabled.False;
         wcdmaAveragingCount = 10;
         wcdmaAveragingType = RFmxSpecAnMXAcpAveragingType.Rms;

         /* LTE signal settings */
         lteCenterFrequency = 2.1e+9;            /* Hz */
         lteIntegrationBandwidth = 9.0e+6;       /* Hz */
         lteAveragingEnabled = RFmxSpecAnMXAcpAveragingEnabled.False;
         lteAveragingCount = 10;
         lteAveragingType = RFmxSpecAnMXAcpAveragingType.Rms;

         for (int i = 0; i < NumberOfOffsetChannels; i++)
         {
            offsetChannelEnabled[i] = RFmxSpecAnMXAcpOffsetEnabled.True;
            offsetChannelSideband[i] = RFmxSpecAnMXAcpOffsetSideband.Both;
            offsetChannelRrcFilterAlpha[i] = 0.220;

            if (i == 0)
            {
               offsetChannelOffset[i] = 10.0e+6;
               offsetChannelIntegrationBandwidth[i] = 9.0e+6;
               offsetChannelRrcFilterEnabled[i] = RFmxSpecAnMXAcpOffsetRrcFilterEnabled.False;
            }
            else
            {
               offsetChannelOffset[i] = 7.500e+6;
               offsetChannelIntegrationBandwidth[i] = 3.840e+6;
               offsetChannelRrcFilterEnabled[i] = RFmxSpecAnMXAcpOffsetRrcFilterEnabled.True;
            }
         }
      }

      private void InitializeInstr()
      {
         /* Create a new RFmx Session */
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      private void ConfigureSpecAn()
      {
         /* Get a SpecAn signal for WCDMA analysis */
         wcdmaSignal = instrSession.GetSpecAnSignalConfiguration("WCDMA");

         /* Configure measurement for WCDMA */
         wcdmaSignal.SetSelectedPorts("", selectedPorts);
         wcdmaSignal.ConfigureRF("", wcdmaCenterFrequency, referenceLevel, externalAttenuation);
         wcdmaSignal.Acp.Configuration.ConfigureAveraging("", wcdmaAveragingEnabled,
                                                          wcdmaAveragingCount,
                                                          wcdmaAveragingType);
         wcdmaSignal.Acp.Configuration.ConfigureCarrierAndOffsets("", wcdmaIntegrationBandwidth,
                                                                  wcdmaNumberOfOffsetChannels,
                                                                  wcdmaChannelSpacing);
         /* Get a SpecAn signal for LTE analysis */
         lteSignal = instrSession.GetSpecAnSignalConfiguration("LTE");

         /* Configure measurement for LTE */
         lteSignal.SetSelectedPorts("", selectedPorts);
         lteSignal.ConfigureRF("", lteCenterFrequency, referenceLevel, externalAttenuation);
         lteSignal.Acp.Configuration.ConfigureAveraging("", lteAveragingEnabled,
                                                        lteAveragingCount,
                                                        lteAveragingType);
         lteSignal.Acp.Configuration.ConfigureNumberOfCarriers("", NumberOfCarriers);
         lteSignal.Acp.Configuration.ConfigureCarrierIntegrationBandwidth("",
                                                                          lteIntegrationBandwidth);
         lteSignal.Acp.Configuration.ConfigureNumberOfOffsets("", NumberOfOffsetChannels);

         lteSignal.Acp.Configuration.ConfigureOffsetIntegrationBandwidthArray("",
                                                                         offsetChannelIntegrationBandwidth);
         lteSignal.Acp.Configuration.ConfigureOffsetArray("", offsetChannelOffset,
                                                     offsetChannelSideband,
                                                     offsetChannelEnabled);
         lteSignal.Acp.Configuration.ConfigureOffsetRrcFilterArray("",
                                                              offsetChannelRrcFilterEnabled,
                                                              offsetChannelRrcFilterAlpha);
      }

      private void RetrieveResults()
      {
         /* Retrieve results */

         wcdmaSignal.Acp.Results.Read("", timeout, out wcdmaCarrierAbsolutePower,
                                      out wcdmaOffCh0LowerRelativePower, out wcdmaOffCh0UpperRelativePower,
                                      out wcdmaOffCh1LowerRelativePower, out wcdmaOffCh1UpperRelativePower);

         lteSignal.Acp.Results.Read("", timeout, out lteCarrierAbsolutePower,
                                    out lteOffCh0LowerRelativePower, out lteOffCh0UpperRelativePower,
                                    out lteOffCh1LowerRelativePower, out lteOffCh1UpperRelativePower);
      }

      private void PrintResults()
      {
         Console.WriteLine("---------------WCDMA Result--------------------\n");
         Console.WriteLine("Carrier Abs Power (dBm or dBm/Hz)    : {0}", wcdmaCarrierAbsolutePower);
         Console.WriteLine("Off ch0 Lower Relative Power (dB)    : {0}", wcdmaOffCh0LowerRelativePower);
         Console.WriteLine("Off ch0 Upper Relative Power(dB)     : {0}", wcdmaOffCh0UpperRelativePower);
         Console.WriteLine("Off ch1 Lower Relative Power(dB)     : {0}", wcdmaOffCh1LowerRelativePower);
         Console.WriteLine("Off ch1 Upper Relative Power(dB)     : {0}", wcdmaOffCh1UpperRelativePower);

         Console.WriteLine("\n---------------LTE Result----------------------\n");
         Console.WriteLine("Carrier Abs Power (dBm or dBm/Hz)    : {0}", lteCarrierAbsolutePower);
         Console.WriteLine("Off ch0 Lower Relative Power (dB)    : {0}", lteOffCh0LowerRelativePower);
         Console.WriteLine("Off ch0 Upper Relative Power(dB)     : {0}", lteOffCh0UpperRelativePower);
         Console.WriteLine("Off ch1 Lower Relative Power(dB)     : {0}", lteOffCh1LowerRelativePower);
         Console.WriteLine("Off ch1 Upper Relative Power(dB)     : {0}", lteOffCh1UpperRelativePower);
      }

      private void CloseSession()
      {
         try
         {
            if (wcdmaSignal != null)
            {
               wcdmaSignal.Dispose();
               wcdmaSignal = null;
            }
            if (lteSignal != null)
            {
               lteSignal.Dispose();
               lteSignal = null;
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
