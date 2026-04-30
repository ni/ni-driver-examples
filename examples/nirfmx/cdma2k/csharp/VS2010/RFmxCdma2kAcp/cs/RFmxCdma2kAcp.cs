//Steps:
//1. Open a new RFmx session
//2. Configure Reference Clock
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure RF Attenuation
//5. Configure Trigger
//6. Configure Band Class
//7. Set Measurement to ACP
//8. Configure Number of Offsets
//9. Set Noise Compensation
//10. Set Dynamic Range Mode
//11. Configure Sweep Time
//12. Configure Averaging Parameters
//13. Commit Settings and Initiate Measurement
//14. Fetch diverse ACP Measurement Results
//15. Close the RFmx Session


using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.Cdma2kMX;

namespace NationalInstruments.Examples.RFmxCdma2kAcp
{
    public class RFmxCdma2kAcp
    {
        RFmxInstrMX instrSession;
        RFmxCdma2kMX cdma2k;
        int i = 0;
        double centerFrequency;
        double externalAttenuation;
        bool autoLevel ;
        double referenceLevel ;
        double frequency ;
        double triggerDelay ;
        int numberOfOffsets;
        double sweepTimeInterval;
        int averagingCount ;
        double timeout;
        RFmxInstrMXRFAttenuationAuto attenuationAuto;
        double attenuationValue;
        string digitalEdgeTriggerSource;
        RFmxCdma2kMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge;
        bool enableTrigger;
        int bandclass;
        RFmxCdma2kMXAcpNoiseCompensationEnabled noiseCompensationEnabled;
        RFmxCdma2kMXAcpMeasurementMethod measurementMethod;
        RFmxCdma2kMXAcpSweepTimeAuto sweepTimeAuto;
        RFmxCdma2kMXAcpAveragingEnabled averagingEnabled;
        RFmxCdma2kMXAcpAveragingType averagingType;

        double carrierAbsolutePower ;

        double[] lowerRelativePower ;
        double[] upperRelativePower;
        double[] lowerAbsolutePower;
        double[] upperAbsolutePower;

        Spectrum<float> spectrum ;
        double measurementInterval;
        string resourceName;

        void CloseSession()
        {
            if (cdma2k != null)
            {
                cdma2k.Dispose();
                cdma2k = null;
            }
            if (instrSession != null)
            {
                instrSession.Close();
                instrSession = null;
            }
        }

        private void InitializeVariables()
        {
            /* Initialize input variables */
            centerFrequency = 833.490e+6;                           /*Hz*/
            externalAttenuation = 0.00;                             /*dB*/
            attenuationAuto = RFmxInstrMXRFAttenuationAuto.True;
            attenuationValue = 10.0;                                /*dB*/
            digitalEdgeTriggerSource = RFmxInstrMXConstants.Pfi0;
            digitalEdgeTriggerEdge = RFmxCdma2kMXDigitalEdgeTriggerEdge.Rising;
            enableTrigger = false;
            bandclass = 0;
            noiseCompensationEnabled = RFmxCdma2kMXAcpNoiseCompensationEnabled.False;
            measurementMethod = RFmxCdma2kMXAcpMeasurementMethod.Normal;
            sweepTimeAuto = RFmxCdma2kMXAcpSweepTimeAuto.True;
            averagingEnabled = RFmxCdma2kMXAcpAveragingEnabled.False;
            averagingType = RFmxCdma2kMXAcpAveragingType.Rms;
            autoLevel = true;
            referenceLevel = 0.00;                                  /*dBm*/
            frequency = 10.0e+6;                                    /*Hz*/
            triggerDelay = 0.00;                                    /*seconds*/
            numberOfOffsets = 2;
            sweepTimeInterval = 1.67e-3;                            /*seconds*/
            averagingCount = 10;
            timeout = 10.0;                                         /*seconds*/
            carrierAbsolutePower = 0.0;                             /*dBm*/
            lowerRelativePower = null;
            upperRelativePower = null;
            lowerAbsolutePower = null;
            upperAbsolutePower = null;

            spectrum = null;
            measurementInterval = 20.0e-3;                          /*seconds*/
            resourceName = "RFSA";
        }

        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        private void ConfigureCdma2k()
        {
			/* Get cdma2k signal */
            cdma2k = instrSession.GetCdma2kSignalConfiguration();
            /* Configure CDMA2k ACP measurement parameters */
            instrSession.ConfigureFrequencyReference("", RFmxInstrMXConstants.OnboardClock, frequency);
            cdma2k.ConfigureExternalAttenuation("", externalAttenuation);
            cdma2k.ConfigureFrequency("", centerFrequency);
            instrSession.ConfigureRFAttenuation("", attenuationAuto, attenuationValue);

            cdma2k.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, 
                enableTrigger);

            if (autoLevel)
            {
                cdma2k.AutoLevel("", measurementInterval, out referenceLevel);
                Console.WriteLine("Reference level (dBm)       : {0}\n", referenceLevel);

            }
            else
            {
                cdma2k.ConfigureReferenceLevel("", referenceLevel);
            }
            cdma2k.ConfigureBandClass("", bandclass);
            cdma2k.SelectMeasurements("", RFmxCdma2kMXMeasurementTypes.Acp, true);
            cdma2k.Acp.Configuration.ConfigureNumberOfOffsets("", numberOfOffsets);

            cdma2k.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled);
            cdma2k.Acp.Configuration.ConfigureMeasurementMethod("", measurementMethod);
            cdma2k.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            cdma2k.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            cdma2k.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Fetch ACP Measurement Results */
            cdma2k.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref lowerRelativePower, ref upperRelativePower, 
                ref lowerAbsolutePower, ref upperAbsolutePower);
            cdma2k.Acp.Results.FetchCarrierAbsolutePower("", timeout, out carrierAbsolutePower);
            cdma2k.Acp.Results.FetchSpectrum("", timeout, ref spectrum);
        }

        private void PrintResults()
        {
            /* Display ACP Measurement Results */
            Console.WriteLine("Carrier Absolute Power (dBm):  {0}\n", carrierAbsolutePower);
            Console.WriteLine("-------------- Offset Channel Measurements --------------");
            for (i = 0; i < lowerRelativePower.Length; i++)
            {
                Console.WriteLine("\nOFFSET                      : {0}", i );
                Console.WriteLine("Lower Relative Power (dB)   :  {0}", lowerRelativePower[i]);
                Console.WriteLine("Upper Relative Power (dB)   :  {0}", upperRelativePower[i]);
                Console.WriteLine("Lower Absolute Power (dBm)  :  {0}", lowerAbsolutePower[i]);
                Console.WriteLine("Upper Absolute Power (dBm)  :  {0}", upperAbsolutePower[i]);
            }
        }

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureCdma2k();
                RetrieveResults();
                PrintResults();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                CloseSession();
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }


    }
}
