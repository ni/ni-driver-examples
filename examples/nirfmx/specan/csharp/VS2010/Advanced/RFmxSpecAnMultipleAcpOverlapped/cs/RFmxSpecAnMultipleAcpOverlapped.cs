//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3.A. Configure the basic signal properties - Center Frequency and External Attenuation
//3.B. Configure the Reference Level to be used in the first run of ACP Measurement
//4. Select ACP measurement and enable the traces
//5. Configure Averaging parameters for the ACP Measurement
//6. Configure Integration BW of the Carrier channel, Number of Offset Channels, Channel Spacing
//7. Initiate ACP Measurement with a Result name.
//   When Result name is wired to Initiate same result name should be used to retrieve results from session.
//8. Wait for ACP_1 acquisition to complete
//9. Configure the Reference level to be used in the second run of ACP Measurement
//10. Initiate another ACP Measurement with a Result name. Use this result name while retrieving the results from session
//11. Fetch ACP_1 Measurement Results using Result name for Selector String
//12. Fetch ACP_2 Measurement Results using Result name for Selector String
//13. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnMultipleAcpOverlapped
{
   public class RFmxSpecAnMultipleAcpOverlapped
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      string resourceName, result1Name, result2Name;
      string selectedPorts;
      double centerFrequency, referenceLevel1, referenceLevel2, externalAttenuation,
             integrationBandwidth, channelSpacing,
             timeout;
      int averagingCount;
      RFmxSpecAnMXAcpAveragingEnabled averagingEnabled;
      RFmxSpecAnMXAcpAveragingType averagingType;

      const int NumberOfOffsetChannels = 2;

      struct AcpMeasurement
      {
         public double carrierAbsolutePower;
         public double relativePower;
         public double carrierFrequency;
         public double resIntegrationBandwidth;
         public double[] lowerRelativePower;
         public double[] upperRelativePower;
         public double[] lowerAbsolutePower;
         public double[] upperAbsolutePower;
      }

      AcpMeasurement acpMeasurement1, acpMeasurement2;

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
         referenceLevel1 = 0.00;             /* dBm */
         referenceLevel2 = -10.00;           /* dBm */
         externalAttenuation = 0.00;         /* dB */
         timeout = 10;                       /* seconds */

         // ACP
         integrationBandwidth = 1.0e+6;      /* Hz */
         channelSpacing = 1.0e+6;            /* Hz */
         averagingEnabled = RFmxSpecAnMXAcpAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxSpecAnMXAcpAveragingType.Rms;

         acpMeasurement1.lowerRelativePower = new double[NumberOfOffsetChannels];
         acpMeasurement1.upperRelativePower = new double[NumberOfOffsetChannels];

         acpMeasurement2.lowerRelativePower = new double[NumberOfOffsetChannels];
         acpMeasurement2.upperRelativePower = new double[NumberOfOffsetChannels];
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
         specAn.SetSelectedPorts("", selectedPorts);
         specAn.ConfigureFrequency("", centerFrequency);
         specAn.ConfigureExternalAttenuation("", externalAttenuation);
         specAn.ConfigureReferenceLevel("", referenceLevel1);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Acp, false);
         specAn.Acp.Configuration.ConfigureAveraging("", averagingEnabled,
                                                     averagingCount, averagingType);
         specAn.Acp.Configuration.ConfigureCarrierAndOffsets("", integrationBandwidth,
                                                             NumberOfOffsetChannels,
                                                             channelSpacing);

         result1Name = RFmxSpecAnMX.BuildResultString("ACP_Results_1");
         specAn.Initiate("", result1Name);
         instrSession.WaitForAcquisitionComplete(timeout);

         result2Name = RFmxSpecAnMX.BuildResultString("ACP_Results_2");
         specAn.ConfigureReferenceLevel("", referenceLevel2);
         specAn.Initiate("", result2Name);
      }

      private void RetrieveResults()
      {
         specAn.Acp.Results.FetchCarrierMeasurement(result1Name, timeout, out acpMeasurement1.carrierAbsolutePower,
                                                    out acpMeasurement1.relativePower, out acpMeasurement1.carrierFrequency,
                                                    out acpMeasurement1.resIntegrationBandwidth);

         specAn.Acp.Results.FetchOffsetMeasurementArray(result1Name, timeout, ref acpMeasurement1.lowerRelativePower,
                                                   ref acpMeasurement1.upperRelativePower, ref acpMeasurement1.lowerAbsolutePower,
                                                   ref acpMeasurement1.upperAbsolutePower);

         specAn.Acp.Results.FetchCarrierMeasurement(result2Name, timeout, out acpMeasurement2.carrierAbsolutePower,
                                                    out acpMeasurement2.relativePower, out acpMeasurement2.carrierFrequency,
                                                    out acpMeasurement2.resIntegrationBandwidth);

         specAn.Acp.Results.FetchOffsetMeasurementArray(result2Name, timeout, ref acpMeasurement2.lowerRelativePower,
                                                   ref acpMeasurement2.upperRelativePower, ref acpMeasurement2.lowerAbsolutePower,
                                                   ref acpMeasurement2.upperAbsolutePower);
      }

      private void PrintResults()
      {
         Console.WriteLine("------------------------ACP Measurement1-----------------------------\n");
         Console.WriteLine("Carrier Abs Power (dBm or dBm/Hz)  {0}", acpMeasurement1.carrierAbsolutePower);
         for (int i = 0; i < NumberOfOffsetChannels; i++)
         {
            Console.WriteLine("\nOffset Channel : {0}", i);
            Console.WriteLine("Lower Relative Power (dB)          {0}", acpMeasurement1.lowerRelativePower[i]);
            Console.WriteLine("Upper Relative Power (dB)          {0}", acpMeasurement1.upperRelativePower[i]);
         }

         Console.WriteLine("\n------------------------ACP Measurement2-----------------------------\n");
         Console.WriteLine("Carrier Abs Power (dBm or dBm/Hz)  {0}", acpMeasurement2.carrierAbsolutePower);
         for (int i = 0; i < NumberOfOffsetChannels; i++)
         {
            Console.WriteLine("\nOffset Channel : {0}", i);
            Console.WriteLine("Lower Relative Power (dB)          {0}", acpMeasurement2.lowerRelativePower[i]);
            Console.WriteLine("Upper Relative Power (dB)          {0}", acpMeasurement2.upperRelativePower[i]);
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
