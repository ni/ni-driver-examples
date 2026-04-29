//Steps:
//1.Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level with different port names.
//4. Configure Pulse Settings.
//5. Configure Pulse Generator Settings.
//6. Configure Averaging,
//7. Select S-Parameter measurement.
//8. Configure number of S-Parameters.
//9. Configure each S-Parameter and format.
//10. Initiate the Measurement.
//11. Read Number of SParams.
//12. Fetch S-Parameter X data.
//13. Fetch S-Parameter Y data for each S-Parameter.
//14. Close RFmx Session.. 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVnaPulseGenerators
{
    public class RFmxVnaPulseGenerators
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna;
        string resourceName;
        double frequencyStart;
        double frequencyStop;
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
        int numberOfPulseGenerators;
        RFmxVnaMXPulseGeneratorEnabled[] pulseGeneratorEnabled;
        string[] pulseGeneratorExportOutputTerminal;
        double[] pulseGeneratorDelay;
        double[] pulseGeneratorWidth;

        RFmxVnaMXAveragingEnabled averagingEnabled;
        int averagingCount;
        string sParamSelectorString;
        string portSelectorString;
        string pulseGeneratorSelectorString;
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
            frequencyStop = 1e9;                                                                 /* (Hz) */
            numberOfFrequencyPoints = 1;
            port1PowerLevel = -10.0;                                                             /* (dBm) */
            port2PowerLevel = -10.0;                                                             /* (dBm) */
            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock;
            frequencyReferenceFrequency = 100e6;                                                 /* (Hz) */
            numberOfSParams = 1;
            numberOfPulseGenerators = 4;
            sParamsSParameters = new string[] { "S11" };
            sParamsFormats = new RFmxVnaMXSParamsFormat[] { RFmxVnaMXSParamsFormat.Magnitude };
            pulseModeEnabled = RFmxVnaMXPulseModeEnabled.True;
            pulsePeriod =1.0e-3;                                                                 /*seconds */
            pulseModulatorDelay = 0.00;                                                          /*seconds */
            pulseModulatorWidth =  100.0e-6;                                                     /*seconds */
            pulseAcquisitionAuto = RFmxVnaMXPulseAcquisitionAuto.True;
            pulseAcquisitionDelay = 46.65e-6;                                                    /*seconds */
            pulseAcquisitionWidth = 20.0e-6;                                                     /*seconds */
            pulseGeneratorEnabled = new RFmxVnaMXPulseGeneratorEnabled[] {
                RFmxVnaMXPulseGeneratorEnabled.True,
                RFmxVnaMXPulseGeneratorEnabled.False,
                RFmxVnaMXPulseGeneratorEnabled.False,
                RFmxVnaMXPulseGeneratorEnabled.False};
            pulseGeneratorExportOutputTerminal = new string[] {
                RFmxInstrMXConstants.Pfi0,
                RFmxInstrMXConstants.DoNotExportSignal,
                RFmxInstrMXConstants.DoNotExportSignal,
                RFmxInstrMXConstants.DoNotExportSignal };                                                                 
            pulseGeneratorDelay = new double[] { 0, 0, 0, 0 };                                   /*seconds */
            pulseGeneratorWidth = new double[] { 100.0e-6, 100.0e-6, 100.0e-6, 100.0e-6 };       /*seconds */
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
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource,
                frequencyReferenceFrequency);                                                    
            vna.SetSweepType("", RFmxVnaMXSweepType.Linear);
            vna.SetStartFrequency("", frequencyStart);
            vna.SetStopFrequency("", frequencyStop);
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
            for (int i = 0; i < numberOfPulseGenerators; i++)
            {
                pulseGeneratorSelectorString = RFmxVnaMX.BuildPulseGeneratorString("", i);
                vna.SetPulseGeneratorEnabled(pulseGeneratorSelectorString, pulseGeneratorEnabled[i]);
                vna.SetPulseGeneratorExportOutputTerminal(pulseGeneratorSelectorString, pulseGeneratorExportOutputTerminal[i]);
                vna.SetPulseGeneratorDelay(pulseGeneratorSelectorString, pulseGeneratorDelay[i]);
                vna.SetPulseGeneratorWidth(pulseGeneratorSelectorString, pulseGeneratorWidth[i]);
            }
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
