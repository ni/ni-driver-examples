//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties (Clock Source and Clock Frequency)
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Min Quiet Time)
//6. Configure IQ measurement
//7. Configure measurement mode and Acquisition parameters
//8. Initiate Measurement
//9. Fetch RawIQ Data
//10. Mean Power calculation - Power in dBm = 10* log (((I^2+Q^2)/(2*R))/1mW), where R=50 Ohms
//11. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnIQRawIQ
{
   public class RFmxSpecAnIQRawIQ
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

         string frequencySource = RFmxInstrMXConstants.OnboardClock;
         double frequency = 10e+6;               /* Hz */

         bool iqPowerEdgeEnabled = false;
         double iqPowerEdgeLevel = -20.0;        /* dBm */
         double triggerDelay = 0.0;              /* seconds */
         double minimumQuietTime = 0.0;          /* seconds */

         double sampleRate = 10e+6;              /* samples per second */
         double acquisitionTime = 0.001;         /* seconds */

         int recordToFetch = 0;
         long samplesToRead = -1;
         double meanPower, meanPowerInDbm;

         double timeout = 10;                   /* seconds */
         ComplexWaveform<ComplexSingle> data = null;
         double sum = 0.0;
         double[] realArray;
         double[] imaginaryArray;

         try
         {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");

            /* Get SpecAn signal */
            specAn = instrSession.GetSpecAnSignalConfiguration();

            /* Configure measurement */
            instrSession.ConfigureFrequencyReference("", frequencySource, frequency);
            specAn.SetSelectedPorts("", selectedPorts);
            specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            specAn.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeLevel, RFmxSpecAnMXIQPowerEdgeTriggerSlope.Rising,
                                              triggerDelay, RFmxSpecAnMXTriggerMinimumQuietTimeMode.Manual,
                                              minimumQuietTime, iqPowerEdgeEnabled);
            specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.IQ, false);
            specAn.IQ.Configuration.SetMeasurementMode("",RFmxSpecAnMXIQMeasurementMode.RawIQ);
            specAn.IQ.Configuration.ConfigureAcquisition("", sampleRate, 1, acquisitionTime, 0);
            specAn.Initiate("", "");

            /* Retrieve results */
            instrSession.FetchRawIQData("", timeout, recordToFetch, samplesToRead, ref data);

            realArray = data.GetRealDataArray(false);
            imaginaryArray = data.GetImaginaryDataArray(false);

            for (int i = 0; i < data.SampleCount; i++)
            {
               sum += (realArray[i] * realArray[i]) + (imaginaryArray[i] * imaginaryArray[i]);
            }

            meanPower = sum / (double)data.SampleCount;
            meanPowerInDbm = 10 * Math.Log10(meanPower / (2 * 50) / 0.001);

            Console.WriteLine("Mean Power (dBm) {0}", meanPowerInDbm);
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
