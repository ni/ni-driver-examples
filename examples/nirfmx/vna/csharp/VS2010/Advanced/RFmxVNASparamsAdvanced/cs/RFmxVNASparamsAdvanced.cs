//Steps:
//1.Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure S-parameter External Attenuation Table (External Fixture's De-embedding Table) from S2P File.
//4. Configure Port Extension.
//5 & 6. Configure sweep settings: Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
//7. Configure Averaging.
//8. Configure Trigger.
//9. Select S-Parameter measurement.
//10. Configure number of S-Parameters.
//11. Configure S-Parameter and format.
//12. Configure Magnitude Units, Phase Trace Type & Group Delay Aperture Settings.
//13. Load Calset data from a file. 
//14. Enable Correction, Configure Interpolation Enabled and Configure correction port subset settings.
//15. Initiate the Measurement after user confirmation.
//16. Read Number of SParams.
//17. Fetch S-Parameter X data.
//18. Fetch S-Parameter Y data for each S-Parameter.
//19. Fetch S-Parameter Correction Level.
//20. Fetch S-Parameter Correction State.
//21. Set SnP Export attributes (can be accessed and written before or after measurement initiate) and save S-Parameter data to file.
//22. Close RFmx Session.

using System;
using System.IO;
using System.Reflection;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;
using System.Collections.Generic;

namespace NationalInstruments.Examples.RFmxVnaSparamsAdvanced
{
    public class RFmxVnaSparamsAdvanced
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna;
        string resourceName;

        double frequencyStart;
        double frequencyStop;
        int numberOfFrequencyPoints;
        RFmxVnaMXSweepType sweepType;
        double[] frequencyList;
        int frequencyListSize;
        double frequencyStep;

        double port1PowerLevel;
        double port2PowerLevel;
        double port1TestReceiverAttenuation;
        double port2TestReceiverAttenuation;
        double IFBandwidth;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        RFmxVnaMXTriggerType triggerType;
        RFmxVnaMXTriggerMode triggerMode;
        double triggerDelay;

        RFmxVnaMXCorrectionPortExtensionEnabled portExtensionEnabled;
        RFmxVnaMXCorrectionPortExtensionDelayDomain portExtensionDelayDomain;
        double portExtensionDelay;
        double portExtensionDistance;
        RFmxVnaMXCorrectionPortExtensionDistanceUnit portExtensionDistanceUnit;
        double portExtensionVelocityFactor;
        RFmxVnaMXCorrectionPortExtensionDCLossEnabled portExtensionDCLossEnabled;
        double portExtensionDCLoss;
        RFmxVnaMXCorrectionPortExtensionLoss1Enabled portExtensionLoss1Enabled;
        RFmxVnaMXCorrectionPortExtensionLoss2Enabled portExtensionLoss2Enabled;
        double portExtensionLoss1Frequency;
        double portExtensionLoss2Frequency;
        double portExtensionLoss1;
        double portExtensionLoss2;

        int numberOfPortExtension;

        string[] portNames;
        int numberOfExternalFixtures;
        string[] s2pFilePaths;
        RFmxInstrMXSParameterOrientation[] sParameterOrientations;
        string currentDirectoryPath;

        int numberOfSParams;
        string[] sParamsSParameters;
        RFmxVnaMXSParamsFormat[] sParamsFormats;

        RFmxVnaMXSParamsMagnitudeUnits magnitudeUnits;
        RFmxVnaMXSParamsPhaseTraceType phaseTraceType;
        RFmxVnaMXSParamsGroupDelayApertureMode groupDelayApertureMode;
        double groupDelayAperturePoints;
        double groupDelayAperturePercentage;
        double groupDelayApertureFrequencySpan;

        string calsetFilePath;
        RFmxVnaMXCorrectionInterpolationEnabled interpolationEnabled;
        RFmxVnaMXCorrectionPortSubsetEnabled portSubsetEnabled;

        RFmxVnaMXAveragingEnabled averagingEnabled;
        int averagingCount;

        string snPFilePath;

        RFmxVnaMXMeasurementTypes measurement;
        bool enableAllTraces;

        string sParamSelectorString;
        string portSelectorString;
        string correctionLevelResult;

        double timeout;

        int numberOfSParamsResult;
        RFmxVnaMXSParamsCorrectionState correctionStateResult;
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

            frequencyListSize = 251;
            frequencyList = new double[frequencyListSize];
            frequencyStep = 100e6;                                                               /* (Hz) */
            sweepType = RFmxVnaMXSweepType.Linear;
            frequencyStart = 1e9;                                                                /* (Hz) */
            frequencyStop = 26e9;                                                                /* (Hz) */
            numberOfFrequencyPoints = 251;
            port1PowerLevel = -10.0;                                                             /* (dBm) */
            port2PowerLevel = -10.0;                                                             /* (dBm) */
            port1TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            port2TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            IFBandwidth = 100.0e3;                                                               /* (Hz) */

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock;
            frequencyReferenceFrequency = 100e6;                                                 /* (Hz) */

            numberOfSParams = 4;
            sParamsSParameters = new string[] { "S11", "S12", "S21", "S22" };
            sParamsFormats = new RFmxVnaMXSParamsFormat[] { RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude };

            magnitudeUnits = RFmxVnaMXSParamsMagnitudeUnits.dB;
            phaseTraceType = RFmxVnaMXSParamsPhaseTraceType.Wrapped;
            groupDelayApertureMode = RFmxVnaMXSParamsGroupDelayApertureMode.Points;
            groupDelayAperturePoints = 11.0;
            groupDelayAperturePercentage = 4.0;                                                    /* (%) */
            groupDelayApertureFrequencySpan = 1.0e9;                                               /* (Hz) */
            snPFilePath = "";

            portNames = new string[] { "port1", "port2" };
            numberOfExternalFixtures = 2;
            s2pFilePaths = new string[] { "1dB_Attenuation.s2p", "1dB_Attenuation.s2p" };
            sParameterOrientations = new RFmxInstrMXSParameterOrientation[] { RFmxInstrMXSParameterOrientation.Port2TowardsDut, RFmxInstrMXSParameterOrientation.Port2TowardsDut };

            portExtensionEnabled = RFmxVnaMXCorrectionPortExtensionEnabled.False;
            portExtensionDelayDomain = RFmxVnaMXCorrectionPortExtensionDelayDomain.Delay;
            portExtensionDelay = 100.0e-12;                                                      /* (seconds) */
            portExtensionDistance = 29.9792e-3;
            portExtensionDistanceUnit = RFmxVnaMXCorrectionPortExtensionDistanceUnit.Meters;
            portExtensionVelocityFactor = 1.0;
            portExtensionDCLossEnabled = RFmxVnaMXCorrectionPortExtensionDCLossEnabled.False;
            portExtensionDCLoss = 0.0;                                                           /* (dB) */
            numberOfPortExtension = 2;
            portExtensionLoss1Enabled = RFmxVnaMXCorrectionPortExtensionLoss1Enabled.False;
            portExtensionLoss2Enabled = RFmxVnaMXCorrectionPortExtensionLoss2Enabled.False;
            portExtensionLoss1Frequency = 0.0;                                                   /* (Hz) */
            portExtensionLoss2Frequency = 0.0;                                                   /* (Hz) */
            portExtensionLoss1 = 0.0;                                                            /* (dB) */
            portExtensionLoss2 = 0.0;                                                            /* (dB) */

            calsetFilePath = "";
            interpolationEnabled = RFmxVnaMXCorrectionInterpolationEnabled.True;
            portSubsetEnabled = RFmxVnaMXCorrectionPortSubsetEnabled.False;
            averagingEnabled = RFmxVnaMXAveragingEnabled.False;
            averagingCount = 10;

            triggerType = RFmxVnaMXTriggerType.None;
            triggerMode = RFmxVnaMXTriggerMode.Signal;
            triggerDelay = 0.0;                                                                 /*seconds*/

            measurement = RFmxVnaMXMeasurementTypes.SParams;
            enableAllTraces = false;

            timeout = 10.0;                                                                     /*seconds */
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureVna()
        {
            vna = instrSession.GetVnaSignalConfiguration();                                     /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);

            currentDirectoryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\";
            for (int i = 0; i < numberOfExternalFixtures; i++)
            {
                portSelectorString = RFmxVnaMX.BuildPortString("", portNames[i]);
                instrSession.LoadSParameterExternalAttenuationTableFromS2pFile(
                    portSelectorString, "", currentDirectoryPath + s2pFilePaths[i], sParameterOrientations[i]);
            }
            for (int i = 0; i < numberOfPortExtension ; i++)
            {
                portSelectorString = RFmxVnaMX.BuildPortString("", portNames[i]);
                vna.SetCorrectionPortExtensionEnabled(portSelectorString, portExtensionEnabled);
                vna.SetCorrectionPortExtensionDelayDomain(portSelectorString, portExtensionDelayDomain);
                vna.SetCorrectionPortExtensionDelay(portSelectorString, portExtensionDelay);
                vna.SetCorrectionPortExtensionDistance(portSelectorString, portExtensionDistance);
                vna.SetCorrectionPortExtensionDistanceUnit(portSelectorString, portExtensionDistanceUnit);
                vna.SetCorrectionPortExtensionVelocityFactor(portSelectorString, portExtensionVelocityFactor);
                vna.SetCorrectionPortExtensionDCLossEnabled(portSelectorString, portExtensionDCLossEnabled);
                vna.SetCorrectionPortExtensionLossDCLoss(portSelectorString, portExtensionDCLoss);
                vna.SetCorrectionPortExtensionLoss1Enabled(portSelectorString, portExtensionLoss1Enabled);
                vna.SetCorrectionPortExtensionLoss2Enabled(portSelectorString, portExtensionLoss2Enabled);
                vna.SetCorrectionPortExtensionLoss1Frequency(portSelectorString, portExtensionLoss1Frequency);
                vna.SetCorrectionPortExtensionLoss2Frequency(portSelectorString, portExtensionLoss2Frequency);
                vna.SetCorrectionPortExtensionLoss1(portSelectorString, portExtensionLoss1);
                vna.SetCorrectionPortExtensionLoss2(portSelectorString, portExtensionLoss2);
            }

            switch(sweepType)
            {
                case RFmxVnaMXSweepType.List:
                    vna.SetSweepType("", sweepType);
                    for (int i = 0; i < frequencyListSize; i++)
                    {
                        frequencyList[i] = frequencyStart + i * frequencyStep;
                    }
                    vna.SetFrequencyList("", frequencyList);
                    break;
                case RFmxVnaMXSweepType.Linear:
                    vna.SetSweepType("", sweepType);
                    vna.SetStartFrequency("", frequencyStart);
                    vna.SetStopFrequency("", frequencyStop);
                    vna.SetNumberOfPoints("", numberOfFrequencyPoints);
                    break;
                default:
                    break;
            }
            
            vna.SetIFBandwidth("", IFBandwidth);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1");
            vna.SetPowerLevel(portSelectorString, port1PowerLevel);
            vna.SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2");
            vna.SetPowerLevel(portSelectorString, port2PowerLevel);
            vna.SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation);
            vna.SetAveragingEnabled("", averagingEnabled);
            vna.SetAveragingCount("", averagingCount);
            vna.SetTriggerType("", triggerType);
            vna.SetTriggerMode("", triggerMode);
            vna.SetTriggerDelay("", triggerDelay);
            vna.SelectMeasurements("", measurement, enableAllTraces);
            vna.SParams.Configuration.SetNumberOfSParameters("", numberOfSParams);
            for (int i = 0; i < numberOfSParams; i++)
            {
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i);
                vna.SParams.Configuration.ConfigureSParameter(sParamSelectorString, sParamsSParameters[i]);
                vna.SParams.Configuration.SetFormat(sParamSelectorString, sParamsFormats[i]);
            }
            vna.SParams.Configuration.SetMagnitudeUnits("", magnitudeUnits);
            vna.SParams.Configuration.SetPhaseTraceType("", phaseTraceType);
            vna.SParams.Configuration.SetGroupDelayApertureMode("", groupDelayApertureMode);
            vna.SParams.Configuration.SetGroupDelayAperturePoints("", groupDelayAperturePoints);
            vna.SParams.Configuration.SetGroupDelayAperturePercentage("", groupDelayAperturePercentage);
            vna.SParams.Configuration.SetGroupDelayApertureFrequencySpan("", groupDelayApertureFrequencySpan);
            vna.CalsetLoadFromFile("", "", calsetFilePath);
            vna.SetCorrectionEnabled("", RFmxVnaMXCorrectionEnabled.True);
            vna.SetCorrectionInterpolationEnabled("", interpolationEnabled);
            vna.SetCorrectionPortSubsetEnabled("", portSubsetEnabled);
            vna.SetCorrectionPortSubsetFullPorts("", "port1,port2");
            vna.SetCorrectionPortSubsetResponsePorts("", "");
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
                vna.SParams.Results.GetCorrectionLevel(sParamSelectorString, out correctionLevelResult);
            }
            vna.SParams.Results.GetCorrectionState("", out correctionStateResult);
            vna.SParams.Configuration.SetSnPDataFormat("", RFmxVnaMXSParamsSnPDataFormat.Auto);
            vna.SParams.Configuration.SetSnPPorts("", "port1,port2");
            vna.SParams.Configuration.ExportToSnPFile("", snPFilePath);
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
