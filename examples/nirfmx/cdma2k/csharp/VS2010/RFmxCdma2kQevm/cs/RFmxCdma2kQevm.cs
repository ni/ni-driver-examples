//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Radio Configuration Parameter.
//6. Configure Uplink Spreading Long Code Mask Parameter.
//7. Select QEVM measurement and enable traces.
//8. Configure Synchronization Mode.
//9. Configure  Measurement Mode & Measurement Length.
//10. Initiate the Measurement.
//11. Fetch QEVM Measurements and Traces.
//12. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.Cdma2kMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.Cdma2kQevm
{
    public class RFmxCdma2kQevm
    {
        RFmxInstrMX instrSession;
        RFmxCdma2kMX cdma2k;

        string resourceName = "RFSA";
        string frequencySource = RFmxInstrMXConstants.OnboardClock;
        string digitalEdgeTriggerSource = RFmxCdma2kMXConstants.Pfi0;
        double centerFrequency =  833.49e+6;                                            /* Hz */
        double referenceLevel  = 0.00;                                                  /* dBm */
        double externalAttenuation = 0.00;                                              /* dB */
            
        double  frequency = 10e+6;                                                      /* Hz */
        double timeout = 10;                                                            /*sec*/ 

        RFmxCdma2kMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge = RFmxCdma2kMXDigitalEdgeTriggerEdge.Rising;
        double triggerDelay =  0.00;                                                     /*sec*/
        bool enableTrigger  = false;

        RFmxCdma2kMXRadioConfiguration radioConfiguration = RFmxCdma2kMXRadioConfiguration.RC3;
        long uplinkSpreadingLongCodeMask = 0;

        RFmxCdma2kMXQevmAveragingEnabled averagingEnabled = RFmxCdma2kMXQevmAveragingEnabled.False;
        int averagingCount = 10;
        int measurementLength = 1536;                                             /*chips*/

        double meanRmsEvm, maximumPeakEvm, meanFrequencyError, meanMagnitudeError, meanPhaseError, meanChipRateError;
        double meanIQOriginOffset, meanIQGainImbalance, meanIQQuadratureError, maximumIQOriginOffset;
        double maximumIQGainImbalance, maximumIQQuadratureError;

        public void Run()
        {
            try
            {
                InitializeInstr();
                ConfigureCdma2k();
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

        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        private void ConfigureCdma2k()
        {
            /* Get CDMA2k signal */
            cdma2k = instrSession.GetCdma2kSignalConfiguration();

            /* Configure measurement */
            instrSession.ConfigureFrequencyReference("", frequencySource, frequency);
            cdma2k.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            cdma2k.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger);
            cdma2k.ConfigureRadioConfiguration("", radioConfiguration);
            cdma2k.ConfigureUplinkSpreading("", uplinkSpreadingLongCodeMask);
            cdma2k.SelectMeasurements("", RFmxCdma2kMXMeasurementTypes.Qevm, true);
            cdma2k.Qevm.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            cdma2k.Qevm.Configuration.ConfigureMeasurementLength("", measurementLength);
            cdma2k.Initiate("","");     
        }

        private void RetrieveResults()
        {
            AnalogWaveform<float> evm = null;
            ComplexSingle[] constellation = null;
            /* Retrieve results */
            cdma2k.Qevm.Results.FetchEvm("", timeout, out meanRmsEvm, out  maximumPeakEvm, out  meanFrequencyError,
                out  meanMagnitudeError, out meanPhaseError, out meanChipRateError);
            cdma2k.Qevm.Results.FetchIQImpairments("", timeout, out meanIQOriginOffset, out meanIQGainImbalance,
                out meanIQQuadratureError,out maximumIQOriginOffset, out maximumIQGainImbalance, out maximumIQQuadratureError);
            cdma2k.Qevm.Results.FetchEvmTrace("", timeout,ref evm);
            cdma2k.Qevm.Results.FetchConstellationTrace("", timeout, ref constellation);          
        }

        private void PrintResults()
        {
            Console.WriteLine("--------------------EVM Results--------------------");
            Console.WriteLine("Mean RMS EVM (%)                     : {0}", meanRmsEvm);
            Console.WriteLine("Maximum Peak EVM (%)                 : {0}", maximumPeakEvm);
            Console.WriteLine("Mean Frequency Error (Hz)            : {0}", meanFrequencyError);
            Console.WriteLine("Mean Chip Rate Error (ppm)           : {0}", meanChipRateError);
            Console.WriteLine("Mean Magnitude Error (%)             : {0}", meanMagnitudeError);
            Console.WriteLine("Mean Phase Error (deg)               : {0}", meanPhaseError);
           
            Console.WriteLine("---------------------I/Q Impairments------------------------");
            Console.WriteLine("Mean I/Q Origin Offset (dB)          : {0}", meanIQOriginOffset);
            Console.WriteLine("Mean I/Q Gain Imbalance (dB)         : {0}", meanIQGainImbalance);
            Console.WriteLine("Mean I/Q Quadrature Error (deg)      : {0}", meanIQQuadratureError);
            Console.WriteLine("Maximum I/Q Origin Offset (dB)       : {0}", maximumIQOriginOffset);
            Console.WriteLine("Maximum I/Q Gain Imbalance (dB)      : {0}", maximumIQGainImbalance);
            Console.WriteLine("Maximum I/Q Quadrature Error (deg)   : {0}", maximumIQQuadratureError);
        }

        private void CloseSession()
        {
            try
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
