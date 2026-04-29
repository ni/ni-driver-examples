//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure ACP Averaging Parameters
//5. Configure ACP Integration BW, Number of Offset Channels and Channel Spacing
//This function configures a Carrier Channel with Offset Channels as specified by the Number of Offsets
//6. Read ACP Measurement Results
//This function returns Absolute Power for the Carrier Channel and Relative Powers for two Offset Channels
//7. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnAcpBasic
{
   public class RFmxSpecAnAcpBasic
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;

      internal void Run()
      {
         string resourceName = "RFSA";
         string selectedPorts = "";
         double centerFrequency = 1e+9;         /* Hz */
         double referenceLevel = 0.00;          /* dBm */
         double externalAttenuation = 0.00;     /* dB */

         double integrationBandwidth = 1.0e+6;  /* Hz */
         int numberOfOffsetChannels = 2;
         double channelSpacing = 1.0e+6;        /* Hz */

         RFmxSpecAnMXAcpAveragingEnabled averagingEnabled = RFmxSpecAnMXAcpAveragingEnabled.False;
         RFmxSpecAnMXAcpAveragingType averagingType = RFmxSpecAnMXAcpAveragingType.Rms;
         int averagingCount = 10;
         double timeout = 10;                   /* seconds */

         double carrierAbsolutePower, offCh0LowerRelativePower, offCh0UpperRelativePower, offCh1LowerRelativePower,
                offCh1UpperRelativePower;

         try
         {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");

            /* Get SpecAn signal */
            specAn = instrSession.GetSpecAnSignalConfiguration();

            /* Configure measurement */
            specAn.SetSelectedPorts("", selectedPorts);
            specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            specAn.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            specAn.Acp.Configuration.ConfigureCarrierAndOffsets("", integrationBandwidth, numberOfOffsetChannels,
                                                                channelSpacing);

            /* Retrieve results */
            specAn.Acp.Results.Read("", timeout, out carrierAbsolutePower, out offCh0LowerRelativePower,
                                    out offCh0UpperRelativePower, out offCh1LowerRelativePower,
                                    out offCh1UpperRelativePower);

            Console.WriteLine("Carrier Absolute Power (dBm or dBm/Hz) {0}", carrierAbsolutePower);
            Console.WriteLine("Offset ch0 Lower Relative Power (dB)   {0}", offCh0LowerRelativePower);
            Console.WriteLine("Offset ch0 Upper Relative Power(dB)    {0}", offCh0UpperRelativePower);
            Console.WriteLine("Offset ch1 Lower Relative Power(dB)    {0}", offCh1LowerRelativePower);
            Console.WriteLine("Offset ch1 Upper Relative Power(dB)    {0}", offCh1UpperRelativePower);
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
