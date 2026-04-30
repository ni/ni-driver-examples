//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Select ACP,TXP measurements in the measurements Array (to perform composite measurement)
//5. Configure ACP Averaging
//6. Configure ACP Integration BW, Number of Offset Channels and Channel Spacing. 
//This function configures a Carrier Channel with Offset Channels as specified by the Number of Offsets
//Refer to VI help for more information
//7. Configure ACP Measurement Interval and RBW
//8. Configure TXP Averaging
//9. Initiate Measurement
//10. Fetch ACP Carrier and Offset Measurements
//11. Fetch TXP Measurement
//12. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnAcpTxpComposite
{
   class RFmxSpecAnAcpTxpComposite
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      string resourceName;
      const int NumberOfOffsetChannels = 2;
      string selectedPorts;
      double centerFrequency, referenceLevel, externalAttenuation, rrcAlpha,
             integrationBandwidth, channelSpacing, timeout, measurementInterval, rbw;
      int acpAveragingCount, txpAveragingCount;
      RFmxSpecAnMXAcpAveragingEnabled acpAveragingEnabled;
      RFmxSpecAnMXTxpAveragingEnabled txpAveragingEnabled;
      RFmxSpecAnMXAcpAveragingType acpAveragingType;
      RFmxSpecAnMXTxpAveragingType txpAveragingType;
      RFmxSpecAnMXTxpRbwFilterType rbwFilterType;
      bool enableAllTraces;

      double carrierAbsolutePower, offCh0LowerRelativePower, offCh0UpperRelativePower,
             offCh1LowerRelativePower, offCh1UpperRelativePower,
             averageMeanPower, peakToAverageRatio, maxPower, minPower;
      double relativePower, carrierFrequency, resIntegrationBandwidth,
             offCh1UpperAbsolutePower, offCh1LowerAbsolutePower,
             offCh0UpperAbsolutePower, offCh0LowerAbsolutePower;


      internal void Run()
      {
         try
         {
            InitializeVariable();
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

      private void InitializeVariable()
      {
         resourceName = "RFSA";
         selectedPorts = "";
         centerFrequency = 1e+9;             /* Hz */
         referenceLevel = 0.00;              /* dBm */
         externalAttenuation = 0.00;         /* dB */
         enableAllTraces = true;
         timeout = 10;                       /* seconds */

         // ACP
         integrationBandwidth = 1.0e+6;      /* Hz */
         channelSpacing = 1.0e+6;            /* Hz */
         acpAveragingEnabled = RFmxSpecAnMXAcpAveragingEnabled.False;
         acpAveragingCount = 10;
         acpAveragingType = RFmxSpecAnMXAcpAveragingType.Rms;

         // Txp
         measurementInterval = 1.0e-3;       /* seconds */
         rbw = 100.00e+3;                    /* Hz */
         rbwFilterType = RFmxSpecAnMXTxpRbwFilterType.Gaussian;
         txpAveragingEnabled = RFmxSpecAnMXTxpAveragingEnabled.False;
         txpAveragingCount = 10;
         txpAveragingType = RFmxSpecAnMXTxpAveragingType.Rms;
         rrcAlpha = 0.1;
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

         /* Configure measurements */
         specAn.SetSelectedPorts("", selectedPorts);
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Acp | RFmxSpecAnMXMeasurementTypes.Txp,
                                   enableAllTraces);

         //ACP
         specAn.Acp.Configuration.ConfigureAveraging("", acpAveragingEnabled, acpAveragingCount,
                                                     acpAveragingType);
         specAn.Acp.Configuration.ConfigureCarrierAndOffsets("", integrationBandwidth,
                                                             NumberOfOffsetChannels,
                                                             channelSpacing);

         //TXP
         specAn.Txp.Configuration.ConfigureMeasurementInterval("", measurementInterval);
         specAn.Txp.Configuration.ConfigureRbwFilter("", rbw, rbwFilterType, rrcAlpha);
         specAn.Txp.Configuration.ConfigureAveraging("", txpAveragingEnabled,
                                                     txpAveragingCount, txpAveragingType);

         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */

         //ACP
         string selectorString = RFmxSpecAnMX.BuildCarrierString2("", 0);
         specAn.Acp.Results.FetchCarrierMeasurement(selectorString, timeout, out carrierAbsolutePower,
                                                    out relativePower, out carrierFrequency,
                                                    out resIntegrationBandwidth);

         selectorString = RFmxSpecAnMX.BuildOffsetString2("", 0);
         specAn.Acp.Results.FetchOffsetMeasurement(selectorString, timeout, out offCh0LowerRelativePower,
                                                   out offCh0UpperRelativePower, out offCh0LowerAbsolutePower,
                                                   out offCh0UpperAbsolutePower);

         selectorString = RFmxSpecAnMX.BuildOffsetString2("", 1);
         specAn.Acp.Results.FetchOffsetMeasurement(selectorString, timeout, out offCh1LowerRelativePower,
                                                   out offCh1UpperRelativePower, out offCh1LowerAbsolutePower,
                                                   out offCh1UpperAbsolutePower);

         //TXP
         specAn.Txp.Results.FetchMeasurement("", timeout, out averageMeanPower, out peakToAverageRatio,
                                             out maxPower, out minPower);
      }

      private void PrintResults()
      {
         Console.WriteLine("------------------------ACP-----------------------------\n");
         Console.WriteLine("Carrier Abs Power (dBm or dBm/Hz)        {0}", carrierAbsolutePower);
         Console.WriteLine("Offset ch0 Lower Relative Power (dB)     {0}", offCh0LowerRelativePower);
         Console.WriteLine("Offset ch0 Upper Relative Power (dB)     {0}", offCh0UpperRelativePower);
         Console.WriteLine("Offset ch1 Lower Relative Power (dB)     {0}", offCh1LowerRelativePower);
         Console.WriteLine("Offset ch1 Upper Relative Power (dB)     {0}", offCh1UpperRelativePower);

         Console.WriteLine("\n------------------------TXP-----------------------------\n");
         Console.WriteLine("Average Mean Power    (dBm)              {0}", averageMeanPower);
         Console.WriteLine("Peak to Average Ratio (dB)               {0}", peakToAverageRatio);
         Console.WriteLine("Maximum Power         (dBm)              {0}", maxPower);
         Console.WriteLine("Minimum Power         (dBm)              {0}", minPower);
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
