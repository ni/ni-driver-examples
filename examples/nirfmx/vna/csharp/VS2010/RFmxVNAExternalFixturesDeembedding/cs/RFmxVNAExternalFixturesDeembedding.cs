//Steps:
//1.Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure S-parameter External Attenuation Table (External Fixture's De-embedding Table) from S2P File.  
//4. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
//5. Configure Averaging.
//6. Select S-Parameter measurement.
//7. Configure number of S-Parameters.
//8. Configure each S-Parameter and format.
//9. Configure Magnitude Units & Phase Trace Type.
//11. Load Calset data from a file. 
//12. Enable Correction.
//13. Initiate the Measurement after user confirmation.
//14. Read Number of SParams.
//15. Fetch S-Parameter X data.
//16. Fetch S-Parameter Y data for each S-Parameter.
//17. Close RFmx Session.

using System;
using System.IO;
using System.Reflection;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVNAExternalFixturesDeembedding
{
    public class RFmxVNAExternalFixturesDeembedding
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna;
        string resourceName;

        double frequencyStart;
        double frequencyEnd;
        int numberOfFrequencyPoints;

        double port1PowerLevel;
        double port2PowerLevel;
        double port1TestReceiverAttenuation;
        double port2TestReceiverAttenuation;
        double IFBandwidth;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        int numberOfExternalFixtures;
        string[] portNames;
        string[] s2pFilePaths;
        RFmxInstrMXSParameterOrientation[] sParameterOrientations;
        string currentDirectoryPath;

        int numberOfSParams;
        string[] sParamsSParameters;
        RFmxVnaMXSParamsFormat[] sParamsFormats;

        RFmxVnaMXSParamsMagnitudeUnits magnitudeUnits;
        RFmxVnaMXSParamsPhaseTraceType phaseTraceType;

        RFmxVnaMXAveragingEnabled averagingEnabled;
        int averagingCount;

        string calsetFilePath;

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
            port1TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            port2TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            IFBandwidth = 100e3;                                                                 /* (Hz) */

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock;
            frequencyReferenceFrequency = 100e6;                                                 /* (Hz) */

            numberOfExternalFixtures = 2;
            portNames = new string[] { "port1", "port2" };
            s2pFilePaths = new string[] { "1dB_Attenuation.s2p", "1dB_Attenuation.s2p" };
            sParameterOrientations = new RFmxInstrMXSParameterOrientation[]
            {
               RFmxInstrMXSParameterOrientation.Port2TowardsDut,
               RFmxInstrMXSParameterOrientation.Port2TowardsDut
            };

            numberOfSParams = 4;
            sParamsSParameters = new string[] { "S11", "S12", "S21", "S22" };
            sParamsFormats = new RFmxVnaMXSParamsFormat[]
            {
                RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Magnitude
            };

            magnitudeUnits = RFmxVnaMXSParamsMagnitudeUnits.dB;
            phaseTraceType = RFmxVnaMXSParamsPhaseTraceType.Wrapped;

            averagingEnabled = RFmxVnaMXAveragingEnabled.False;
            averagingCount = 10;

            calsetFilePath = "";

            timeout = 10.0;                                                                      /*seconds */
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureVna()
        {
            vna = instrSession.GetVnaSignalConfiguration();                                      /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);

            currentDirectoryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\";
            for (int i = 0; i < numberOfExternalFixtures; i++)
            {
                portSelectorString = RFmxVnaMX.BuildPortString("", portNames[i]);
                instrSession.LoadSParameterExternalAttenuationTableFromS2pFile(
                    portSelectorString, "", currentDirectoryPath + s2pFilePaths[i], sParameterOrientations[i]);
            }

            vna.SetSweepType("", RFmxVnaMXSweepType.Linear);
            vna.SetStartFrequency("", frequencyStart);
            vna.SetStopFrequency("", frequencyEnd);
            vna.SetNumberOfPoints("", numberOfFrequencyPoints);
            vna.SetIFBandwidth("", IFBandwidth);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1");
            vna.SetPowerLevel(portSelectorString, port1PowerLevel);
            vna.SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2");
            vna.SetPowerLevel(portSelectorString, port2PowerLevel);
            vna.SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation);

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

            vna.SParams.Configuration.SetMagnitudeUnits("", magnitudeUnits);
            vna.SParams.Configuration.SetPhaseTraceType("", phaseTraceType);

            vna.CalsetLoadFromFile("", "", calsetFilePath);

            vna.SetCorrectionEnabled("", RFmxVnaMXCorrectionEnabled.True);
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
