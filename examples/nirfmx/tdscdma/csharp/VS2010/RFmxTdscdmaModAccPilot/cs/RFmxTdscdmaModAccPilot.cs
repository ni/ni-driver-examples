//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties: Clock Source and Clock Frequency
//3. Configure the basic signal properties: Center Frequency, Reference Level and External Attenuation
//4. Configure the trigger properties
//5. Select ModAcc measurement and enable the traces
//6. Configure Uplink Scrambling Code
//7. Configure the basic Measurement Settings
//8. Configure the Midamble Settings 
//9. Initiate Measurement
//10. Fetch ModAcc Measurements and Traces
//11. Close the RFmx session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.TdscdmaMX;

namespace NationalInstruments.Examples.RFmxTdscdmaModAccPilot
{
    public class RFmxTdscdmaModAccPilot
    {
        RFmxInstrMX instrSession;
        RFmxTdscdmaMX tdscdma;
        string resourceName, frequencyReferenceSource,iqPowerEdgeTriggerSource;
        double centerFrequency, referenceLevel, externalAttenuation, frequencyReferenceFrequency,
               triggerDelay, minimumQuietTime, iqPowerEdgeTriggerLevel;    
        
        RFmxTdscdmaMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;
        RFmxTdscdmaMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        bool enableTrigger;
        RFmxTdscdmaMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;
         
		int pilotCode; 
        RFmxTdscdmaMXModAccSlotType slotType;		 
		RFmxTdscdmaMXModAccAveragingEnabled averagingEnabled;
		int averagingCount;
		
        double timeout;
        double rmsPilotEvm, peakPilotEvm, pilotRho, frequencyError,
            rmsPilotMagnitudeError, rmsPilotPhaseError;
		double rmsCompositeEvm, peakCompositeEvm, compositeRho, chipRateError,
            rmsCompositeMagnitudeError, rmsCompositePhaseError;
        double iqOriginOffset, iqGainImbalance, iqQuadratureError;
        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureTdscdma();
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

            centerFrequency = 1.91e+9;             /* Hz */
            referenceLevel = 0.00;                 /* dBm */
            externalAttenuation = 0.00;            /* dB */

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e+6;   /* Hz */

            triggerDelay = 0.00;                   /* seconds */
            minimumQuietTime = 50E-6;              /* seconds */
            iqPowerEdgeTriggerLevel = -20.00;      /*dB*/
            iqPowerEdgeTriggerSource = "0";
            iqPowerEdgeTriggerSlope = RFmxTdscdmaMXIQPowerEdgeTriggerSlope.Rising;
            enableTrigger = true;
            iqPowerEdgeTriggerLevelType = RFmxTdscdmaMXIQPowerEdgeTriggerLevelType.Relative;
            minimumQuietTimeMode = RFmxTdscdmaMXTriggerMinimumQuietTimeMode.Auto;
			
			pilotCode = 0;
			slotType = RFmxTdscdmaMXModAccSlotType.Pilot;
            averagingEnabled = RFmxTdscdmaMXModAccAveragingEnabled.False;
			averagingCount =10;

            timeout = 10;                          /* seconds */
        }

        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }
        
        private void ConfigureTdscdma()
        {
            /* Get SpecAn signal */
            tdscdma = instrSession.GetTdscdmaSignalConfiguration();

            /* Configure measurement */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            tdscdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            tdscdma.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, 
                iqPowerEdgeTriggerLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime, 
                iqPowerEdgeTriggerLevelType, enableTrigger);
			tdscdma.ConfigurePilot("", pilotCode);	
            tdscdma.SelectMeasurements("", RFmxTdscdmaMXMeasurementTypes.ModAcc, true);
			tdscdma.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            tdscdma.ModAcc.Configuration.ConfigureSlotType("", slotType);
            tdscdma.Initiate("", "");
        }

        private void RetrieveResults()
        {
            ComplexSingle[] constellation = null;
            AnalogWaveform<float> evm = null;
            /* Retrieve results */
            tdscdma.ModAcc.Results.FetchPilotEvm("", timeout, out rmsPilotEvm, out peakPilotEvm, out pilotRho, 
                out rmsPilotMagnitudeError, out rmsPilotPhaseError);
			tdscdma.ModAcc.Results.FetchCompositeEvm("", timeout, out rmsCompositeEvm, out peakCompositeEvm,  
                out compositeRho, out frequencyError, out chipRateError, 
				out rmsCompositeMagnitudeError, out rmsCompositePhaseError);
            tdscdma.ModAcc.Results.FetchConstellationTrace("", timeout, ref constellation);
            tdscdma.ModAcc.Results.FetchIQImpairments("", timeout, out iqOriginOffset, out iqGainImbalance, out iqQuadratureError);
            tdscdma.ModAcc.Results.FetchEvmTrace("",timeout,ref evm);
        }

        private void PrintResults()
        {
            Console.WriteLine("--------------------Pilot EVM Results--------------------");
            Console.WriteLine("RMS Pilot EVM (%)             {0}",rmsPilotEvm);
            Console.WriteLine("Peak Pilot EVM (%)            {0}",peakPilotEvm);
            Console.WriteLine("Pilot Rho                     {0}",pilotRho);
			Console.WriteLine("RMS Pilot Magnitude Error (%) {0}",rmsPilotMagnitudeError);
            Console.WriteLine("RMS Pilot Phase Error (deg)   {0}",rmsPilotPhaseError);
			Console.WriteLine("Frequency Error (Hz)          {0}",frequencyError);

            Console.WriteLine("\n---------------------IQ Impairments------------------------");
            Console.WriteLine("I/Q Origin Offset (dB)         {0}", iqOriginOffset);
            Console.WriteLine("I/Q Gain Imbalance (dB)        {0}", iqGainImbalance);
            Console.WriteLine("I/Q Quadrature Error (deg)     {0}", iqQuadratureError);
        }

        private void CloseSession()
        {
            try
            {
                if (tdscdma != null)
                {
                    tdscdma.Dispose();
                    tdscdma = null;
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
