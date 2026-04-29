//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties: Clock Source and Clock Frequency
//3. Configure the basic signal properties: Center Frequency, Reference Level and External Attenuation
//4. Configure the trigger properties
//5. Select SEM measurement and enable the traces
//6. Configure Sweep Time for the SEM measurement
//7. Configure Averaging Parameters for the SEM measurement
//8. Initiate Measurement
//9. Fetch SEM Measurements and Traces
//10. Close the RFmx session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.TdscdmaMX;

namespace NationalInstruments.Examples.RFmxTdscdmaSem
{
    public class RFmxTdscdmaSem
    {                                                               
        RFmxInstrMX instrSession;
        RFmxTdscdmaMX tdscdma;
        String resourceName, frequencySource,iqPowerEdgeTriggerSource;
        double centerFrequency, referenceLevel, externalAttenuation, frequencyReferenceFrequency, sweepTimeInterval,
               triggerDelay, minimumQuietTimeDuration, iqPowerEdgeTriggerLevel;
      
        int averagingCount;
        RFmxTdscdmaMXSemAveragingEnabled averagingEnabled;
        RFmxTdscdmaMXSemAveragingType averagingType;
        RFmxTdscdmaMXSemSweepTimeAuto sweepTimeAuto;

        double timeout = 10.0;
                       
        double carrierAbsoluteIntegratedPower;
        double[] lowerOffsetMargin ;
        double[] lowerOffsetMarginAbsolutePower ;
        double[] lowerOffsetMarginRelativePower ;
        double[] lowerOffsetMarginFrequency ;
        RFmxTdscdmaMXSemLowerOffsetMeasurementStatus[] lowerOffsetMeasurementStatus;

        RFmxTdscdmaMXSemMeasurementStatus measurementStatus;

        double[] upperOffsetMargin;
        double[] upperOffsetMarginAbsolutePower;
        double[] upperOffsetMarginRelativePower;
        double[] upperOffsetMarginFrequency;
        Spectrum<float> spectrum, absoluteMask, relativeMask;

        RFmxTdscdmaMXSemUpperOffsetMeasurementStatus[] upperOffsetMeasurementStatus;

        RFmxTdscdmaMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;
        RFmxTdscdmaMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        bool enableTrigger;
        RFmxTdscdmaMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;

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
            resourceName = "RFSA";

            centerFrequency = 1.91e+9;                 /* Hz */
            referenceLevel = 0.00;                  /* dBm */
            externalAttenuation = 0.00;             /* dB */

            frequencySource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e+6;                    /* Hz */

            triggerDelay = 0.00;                    /* seconds */
            minimumQuietTimeDuration = 16E-6;     /* seconds */
            iqPowerEdgeTriggerLevel = -20.00;       /*dB*/
            iqPowerEdgeTriggerSource = "0";
            iqPowerEdgeTriggerSlope = RFmxTdscdmaMXIQPowerEdgeTriggerSlope.Rising;
            enableTrigger = true;
            iqPowerEdgeTriggerLevelType = RFmxTdscdmaMXIQPowerEdgeTriggerLevelType.Relative;
            minimumQuietTimeMode = RFmxTdscdmaMXTriggerMinimumQuietTimeMode.Auto;

            // Sweep Time
            sweepTimeAuto = RFmxTdscdmaMXSemSweepTimeAuto.True;
            sweepTimeInterval = 660e-6;	    /* seconds */

            averagingEnabled = RFmxTdscdmaMXSemAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxTdscdmaMXSemAveragingType.Rms;
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

            instrSession.ConfigureFrequencyReference("", frequencySource, frequencyReferenceFrequency);
            tdscdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            tdscdma.ConfigureIQPowerEdgeTrigger("",iqPowerEdgeTriggerSource,  iqPowerEdgeTriggerSlope,iqPowerEdgeTriggerLevel,
                triggerDelay, minimumQuietTimeMode,  minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType,enableTrigger);
            
            tdscdma.SelectMeasurements("", RFmxTdscdmaMXMeasurementTypes.Sem, true);
           
            tdscdma.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                        averagingType);
            tdscdma.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         
                      
            tdscdma.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Retrieve results */


            tdscdma.Sem.Results.FetchLowerOffsetMarginArray("", timeout, ref lowerOffsetMeasurementStatus, 
                                                           ref lowerOffsetMargin, 
                                                           ref lowerOffsetMarginFrequency, 
                                                           ref lowerOffsetMarginAbsolutePower, 
                                                           ref lowerOffsetMarginRelativePower);

            tdscdma.Sem.Results.FetchUpperOffsetMarginArray("", timeout, ref upperOffsetMeasurementStatus, 
                                                           ref upperOffsetMargin,
                                                           ref upperOffsetMarginFrequency, 
                                                           ref upperOffsetMarginAbsolutePower, 
                                                           ref upperOffsetMarginRelativePower);

            tdscdma.Sem.Results.FetchMeasurementStatus("", timeout, out measurementStatus);
            tdscdma.Sem.Results.FetchCarrierAbsoluteIntegratedPower("", timeout, out carrierAbsoluteIntegratedPower);
            tdscdma.Sem.Results.FetchSpectrum("", timeout, ref spectrum, ref absoluteMask,ref relativeMask);

        }

        private void PrintResults()
        {
            Console.WriteLine("Measurement Status  :                                  {0}\n", measurementStatus);
         
	        Console.WriteLine("\n--------------------------Carrier Measurements----------------------------\n");
            Console.WriteLine("Carrier Absolute Integrated Power (dBm):               {0}", carrierAbsoluteIntegratedPower);	
	        
	        Console.WriteLine("\n--------------Offset Segment Measurements ---------------------------\n");
            for (int i = 0; i < lowerOffsetMargin.Length; i++)
	        {
                Console.WriteLine("Offset {0}\n", i);
               
		        Console.WriteLine("Lower Offset : Margin (dB):                           {0}", lowerOffsetMargin[i]);
		        Console.WriteLine("Lower Offset : Margin Absolute Power (dBm):           {0}", lowerOffsetMarginAbsolutePower[i]);
		        Console.WriteLine("Lower Offset : Margin Relative Power (dB):            {0}", lowerOffsetMarginRelativePower[i]);
		        Console.WriteLine("Lower Offset : Margin Frequency (Hz):                 {0}", lowerOffsetMarginFrequency[i]);			

		        Console.WriteLine("Lower Offset : Measurement Status :                    {0}\n", lowerOffsetMeasurementStatus[i]);	       


		        Console.WriteLine("Upper Offset : Margin (dB):                            {0}",upperOffsetMargin[i]);
		        Console.WriteLine("Upper Offset : Margin Absolute Power (dBm):            {0}",upperOffsetMarginAbsolutePower[i]);
		        Console.WriteLine("Upper Offset : Margin Relative Power (dB):             {0}",upperOffsetMarginRelativePower[i]);
		        Console.WriteLine("Upper Offset : Margin Frequency (Hz):                  {0}",upperOffsetMarginFrequency[i]);
	
		        Console.WriteLine("Upper Offset : Measurement Status :                    {0}\n", upperOffsetMeasurementStatus[i]);
		    }
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

        private void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }
    }
}
