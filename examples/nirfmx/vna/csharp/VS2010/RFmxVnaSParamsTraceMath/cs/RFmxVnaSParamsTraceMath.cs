//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
//4. Configure Averaging.
//5. Select S-Parameter measurement.
//6. Configure S-Parameter and format for selected S-Parameters.
//7. Configure Magnitude Units & Phase Trace Type.
//8. Initiate the Measurement.
//9. Copy Measurement Data to Memory and Get Memory Data.
//10. Configure Math Function.
//11. Initiate the Measurement for Applying Math Function.
//12. Read X-Axis Values (aggregated frequency list).
//13. Fetch Math Applied S-Parameter Y data.
//14. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVnaSParamsTraceMath
{
    public class RFmxVnaSParamsTraceMath
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna;
        string resourceName;

        double startFrequency;
        double stopFrequency;
        int numberOfPoints;
        double port1PowerLevel;
        double port2PowerLevel;
        double port1TestReceiverAttenuation;
        double port2TestReceiverAttenuation;
        double IFBandwidth;

        string sParamsSParameters;
        RFmxVnaMXSParamsFormat sParamsFormats;

        string sParamsMemoryName;
        RFmxVnaMXSParamsMathFunction sParamsMathFunction;

        RFmxVnaMXAveragingEnabled averagingEnabled;
        int averagingCount;

        double timeout;

        string portSelectorString;
        string sParamSelectorString;
        string measurementMemorySelectorString;

        double[] sParamsXDataResult;
        float[] sParamsY1DataResult;
        float[] sParamsY2DataResult;
        double[] sParamsMemoryXDataResult;
        float[] sParamsMemoryY1DataResult;
        float[] sParamsMemoryY2DataResult;

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

            startFrequency = 1e9;                                                                /* (Hz) */
            stopFrequency = 26e9;                                                                /* (Hz) */
            numberOfPoints = 251;
            port1PowerLevel = -10.0;                                                             /* (dBm) */
            port2PowerLevel = -10.0;                                                             /* (dBm) */
            port1TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            port2TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            IFBandwidth = 100.0e3;                                                               /* (Hz) */

            sParamsSParameters = "S11";
            sParamsFormats = RFmxVnaMXSParamsFormat.Magnitude;

            sParamsMemoryName = "Memory0";
            sParamsMathFunction = RFmxVnaMXSParamsMathFunction.Divide;

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
            instrSession.ConfigureFrequencyReference("", RFmxInstrMXConstants.PxiClock, 100e6);
            vna.SetSweepType("", RFmxVnaMXSweepType.Linear);
            vna.SetStartFrequency("", startFrequency);
            vna.SetStopFrequency("", stopFrequency);
            vna.SetNumberOfPoints("", numberOfPoints);
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

            sParamSelectorString = RFmxVnaMX.BuildSParameterString("", 0);
            vna.SParams.Configuration.ConfigureSParameter(sParamSelectorString, sParamsSParameters);
            vna.SParams.Configuration.SetFormat(sParamSelectorString, sParamsFormats);
            
            vna.SParams.Configuration.SetMagnitudeUnits("", RFmxVnaMXSParamsMagnitudeUnits.dB);
            vna.SParams.Configuration.SetPhaseTraceType("", RFmxVnaMXSParamsPhaseTraceType.Wrapped);
            vna.Initiate("", "");
        }

        void RetrieveResults()
        {
            vna.CopyDataToMeasurementMemory(sParamSelectorString, sParamsMemoryName);
            measurementMemorySelectorString = RFmxVnaMX.BuildMeasurementMemoryString(sParamSelectorString, sParamsMemoryName);
            vna.GetMeasurementMemoryXData(measurementMemorySelectorString, ref sParamsMemoryXDataResult);
            vna.GetMeasurementMemoryYData(measurementMemorySelectorString, ref sParamsMemoryY1DataResult, ref sParamsMemoryY2DataResult);
            vna.SParams.Configuration.SetMathFunction(sParamSelectorString, sParamsMathFunction);

            vna.Initiate("", "");
            vna.SParams.Results.FetchXData("", timeout, ref sParamsXDataResult);
            vna.SParams.Results.FetchYData(sParamSelectorString, timeout, ref sParamsY1DataResult, ref sParamsY2DataResult);
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
