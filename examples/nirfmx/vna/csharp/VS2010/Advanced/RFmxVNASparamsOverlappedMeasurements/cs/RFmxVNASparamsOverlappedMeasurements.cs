//Steps:
//1.Open a new RFmx session
//2.Create two named Signals called 'Signal1' and 'Signal2'. Signals here can be considered equivalent to the concept of Channels in third party software.
//For each Signal, configure sweep and measurement settings. Note that, Power Level varies for each Signal in this example.
//    2A. Create Signal configuration
//    2B. Configure the sweep properties - Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, Test Receiver Attn, Power Level
//    2C. Select S-Parameter Measurement
//    2D. Configure the number of S-Parameters
//    2E. Configure the S-Parameter and Format
//3. For each Signal, initiate the measurement and wait for the acquisition to complete.
//Here note that, between the two signals, only acquisition is sequential and measurements are overlapped.
//4. Fetch the measurement results of each Signal
//5. Close the RFmx Session

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVnaSparamsOverlappedMeasurements
{
    public class RFmxVnaSparamsOverlappedMeasurements
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vnaSignal1;
        RFmxVnaMX vnaSignal2;
        string resourceName;
        string[] namedSignals;

        double frequencyStart;
        double frequencyEnd;
        int numberOfFrequencyPoints;

        double powerLevel;
        double signalPowerLevel;
        double port1TestReceiverAttenuation;
        double port2TestReceiverAttenuation;
        double IFBandwidth;
        string frequencyReferenceSource;
        double frequencyReferenceFrequency;
        int numberOfSParams;
        string[] sParamsSParameters;
        RFmxVnaMXSParamsFormat[] sParamsFormats;
        string sParamSelectorString;
        string portSelectorString;
        double acquisitionTimeout;
        double fetchTimeout;
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

            frequencyStart = 1e9;                                                                /* (Hz) */
            frequencyEnd = 26e9;                                                                 /* (Hz) */
            numberOfFrequencyPoints = 251;
            powerLevel = -10.0;                                                                  /* (dBm) */
            port1TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            port2TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            IFBandwidth = 100.0e3;                                                               /* (Hz) */
            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock;
            frequencyReferenceFrequency = 100e6;                                                 /* (Hz) */
            numberOfSParams = 4;
            sParamsSParameters = new string[] { "S11", "S12", "S21", "S22" };
            namedSignals = new string[] { "Signal1", "Signal2" };
            sParamsFormats = new RFmxVnaMXSParamsFormat[] { RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude };
            acquisitionTimeout = 10.0;                                                           /*seconds */
            fetchTimeout = 10.0;                                                                 /*seconds */
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureVna()
        {
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            vnaSignal1 = instrSession.GetVnaSignalConfiguration("Signal1");                     /* Create a new RFmx Session for Signal1 */
            vnaSignal2 = instrSession.GetVnaSignalConfiguration("Signal2");                     /* Create a new RFmx Session fro Signal2 */
            for (int i = 0; i < namedSignals.Length; i++)
            {

                GetSignalName(i).SetSweepType("", RFmxVnaMXSweepType.Linear);
                GetSignalName(i).SetStartFrequency("", frequencyStart);
                GetSignalName(i).SetStopFrequency("", frequencyEnd);
                GetSignalName(i).SetNumberOfPoints("", numberOfFrequencyPoints);
                GetSignalName(i).SetIFBandwidth("", IFBandwidth);
                portSelectorString = RFmxVnaMX.BuildPortString("", "port1");
                GetSignalName(i).SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation);
                signalPowerLevel = powerLevel - (i * 10);
                GetSignalName(i).SetPowerLevel(portSelectorString, signalPowerLevel);
                portSelectorString = RFmxVnaMX.BuildPortString("", "port2");
                GetSignalName(i).SetPowerLevel(portSelectorString, signalPowerLevel);
                GetSignalName(i).SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation);
                GetSignalName(i).SelectMeasurements("", RFmxVnaMXMeasurementTypes.SParams, false);
                GetSignalName(i).SParams.Configuration.SetNumberOfSParameters("", numberOfSParams);
                for (int k = 0; k < numberOfSParams; k++)
                {
                    sParamSelectorString = RFmxVnaMX.BuildSParameterString("", k);
                    GetSignalName(i).SParams.Configuration.ConfigureSParameter(sParamSelectorString, sParamsSParameters[k]);
                    GetSignalName(i).SParams.Configuration.SetFormat(sParamSelectorString, sParamsFormats[k]);
                }
            }
            for (int i = 0; i < namedSignals.Length; i++)
            {
                GetSignalName(i).Initiate("", "");
                instrSession.WaitForAcquisitionComplete(acquisitionTimeout);
            }
        }

        void RetrieveResults()
        {
            for (int j = 0; j < namedSignals.Length; j++)
            {
                GetSignalName(j).SParams.Configuration.GetNumberOfSParameters("", out numberOfSParamsResult);
                GetSignalName(j).SParams.Results.FetchXData("", fetchTimeout, ref sParamsXDataResult);
                sParamsY1DataResult = new float[numberOfSParamsResult][];
                sParamsY2DataResult = new float[numberOfSParamsResult][];
                for (int i = 0; i < numberOfSParamsResult; i++)
                {
                    sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i);
                    GetSignalName(j).SParams.Results.FetchYData(sParamSelectorString, fetchTimeout, ref sParamsY1DataResult[i], ref sParamsY2DataResult[i]);
                }
                GetSignalName(j).SParams.Results.GetCorrectionState("", out correctionStateResult);
            }
        }

        private RFmxVnaMX GetSignalName(int index)
        {
            switch (index)
            {
                case 0:
                    return vnaSignal1;
                case 1:
                    return vnaSignal2;
                default:
                    throw new InvalidOperationException("Invalid index");
            }
        }

        void CloseSession()
        {
            if (vnaSignal1 != null)
            {
                vnaSignal1.Dispose();
                vnaSignal1 = null;
            }
            if (vnaSignal2 != null)
            {
                vnaSignal2.Dispose();
                vnaSignal2 = null;
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
