//Steps:
//1.Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level with different port names.
//4. Configure Pulse Settings.
//5. Configure Averaging,
//6. Select S-Parameter measurement.
//7. Configure number of S-Parameters.
//8. Configure each S-Parameter and format.
//9. Initiate the Measurement.
//10. Read Number of SParams.
//11. Fetch S-Parameter X data.
//12. Fetch S-Parameter Y data for each S-Parameter.
//13. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVnaPulsedSParams
{
    public class RFmxVnaPulsedSParams
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna;
        string resourceName;

        double frequencyStart;
        double frequencyEnd;
        int numberOfFrequencyPoints;
        double port1PowerLevel;
        double port2PowerLevel;
        string frequencyReferenceSource;
        double frequencyReferenceFrequency;
        int numberOfSParams;
        string[] sParamsSParameters;
        RFmxVnaMXSParamsFormat[] sParamsFormats;
        RFmxVnaMXPulseModeEnabled pulseModeEnabled;
        double pulsePeriod;
        double pulseModulatorDelay;
        double pulseModulatorWidth;
        RFmxVnaMXPulseAcquisitionAuto pulseAcquisitionAuto;
        double pulseAcquisitionDelay;
        double pulseAcquisitionWidth;
        RFmxVnaMXAveragingEnabled averagingEnabled;
        int averagingCount;
        string sParamSelectorString;
        string portSelectorString;
        double timeout;
        int numberOfSParamsResult;
        double[] sParamsXDataResult;
        float[][] sParamsY1DataResult;
        float[][] sParamsY2DataResult;

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureVna();
                RetrieveResults();
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
            finally
            {
                /* Close session */
                CloseSession();
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        void InitializeVariables()
        {
            resourceName = "VNA";

            frequencyStart = 1e9;                                                                /* (Hz) */
            frequencyEnd = 26e9;                                                                 /* (Hz) */
            numberOfFrequencyPoints = 251;
            port1PowerLevel = -10.0;                                                             /* (dBm) */
            port2PowerLevel = -10.0;                                                             /* (dBm) */
            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock;
            frequencyReferenceFrequency = 100e6;                                                 /* (Hz) */
            numberOfSParams = 4;
            sParamsSParameters = new string[] { "S11", "S12", "S21", "S22" };
            sParamsFormats = new RFmxVnaMXSParamsFormat[] { RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude };

            pulseModeEnabled = RFmxVnaMXPulseModeEnabled.True;
            pulsePeriod =1.0e-3;                                                                 /*seconds */
            pulseModulatorDelay = 0.00;                                                          /*seconds */
            pulseModulatorWidth =  100.0e-6;                                                     /*seconds */
            pulseAcquisitionAuto = RFmxVnaMXPulseAcquisitionAuto.True;
            pulseAcquisitionDelay = 20.00e-6;                                                    /*seconds */
            pulseAcquisitionWidth = 46.65e-6;                                                    /*seconds */
            averagingEnabled = RFmxVnaMXAveragingEnabled.False;
            averagingCount = 10;
            timeout = 10.0;                                                                      /*seconds */
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureVna()
        {
            vna = instrSession.GetVnaSignalConfiguration();                                      /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);                                                 // (Hz)

            vna.SetSweepType("", RFmxVnaMXSweepType.Linear);
            vna.SetStartFrequency("", frequencyStart);
            vna.SetStopFrequency("", frequencyEnd);
            vna.SetNumberOfPoints("", numberOfFrequencyPoints);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1");
            vna.SetPowerLevel(portSelectorString, port1PowerLevel);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2");
            vna.SetPowerLevel(portSelectorString, port2PowerLevel);
            vna.SetPulseModeEnabled("", pulseModeEnabled);
            vna.SetPulsePeriod("", pulsePeriod);
            vna.SetPulseModulatorDelay("", pulseModulatorDelay);
            vna.SetPulseModulatorWidth("", pulseModulatorWidth);
            vna.SetPulseAcquisitionAuto("", pulseAcquisitionAuto);
            vna.SetPulseAcquisitionDelay("", pulseAcquisitionDelay);
            vna.SetPulseAcquisitionWidth("", pulseAcquisitionWidth);
            vna.SetAveragingEnabled("", averagingEnabled);
            vna.SetAveragingCount("", averagingCount);
            vna.SelectMeasurements("", RFmxVnaMXMeasurementTypes.SParams, false);
            vna.SParams.Configuration.SetNumberOfSParameters("", numberOfSParams);
            for (int i = 0; i < numberOfSParams; i++)
            {
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i);
                vna.SParams.Configuration.ConfigureSParameter(sParamSelectorString, sParamsSParameters[i]);
                vna.SParams.Configuration.SetFormat(sParamSelectorString, sParamsFormats[i]);
            }
            vna.Initiate("", "");
        }

        void RetrieveResults()
        {
            vna.SParams.Configuration.GetNumberOfSParameters("", out numberOfSParamsResult);
            vna.SParams.Results.FetchXData("", timeout, ref sParamsXDataResult);
            sParamsY1DataResult = new float[numberOfSParamsResult][];
            sParamsY2DataResult = new float[numberOfSParamsResult][];
            for (int i = 0; i < numberOfSParamsResult; i++)
            {
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i);
                vna.SParams.Results.FetchYData(sParamSelectorString, timeout, ref sParamsY1DataResult[i], ref sParamsY2DataResult[i]);
            }
        }

        void CloseSession()
        {
            if (vna != null)
            {
                vna.Dispose();
                vna = null;
            }
            if (instrSession != null)
            {
                instrSession.Close();
                instrSession = null;
            }
        }

        static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }
    }
}
