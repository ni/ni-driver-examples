//Steps:
//1.Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure sweep settings: IF Bandwidth, Power Level and Test Rx Attenuation with different port names.
//4. Configure Sweep Type as Segment.Configure Number of Segments and Independent Settings enabled per segment. 
//5. Configure per segment settings like Segment Enabled, Start and Stop Frequencies, Number of Frequency points, Segment IF Bandwidth, Segment Dwell Time, Segment Power Level and Segment Test Receiver Attenuation.
//   For the properties where the Segment <property> Enabled was set to True in Step 4, values configured in Step 5 are used. If they were set to False, values configured in Step 3 are used.
//6. Configure Trigger settings.
//7. Select S-Parameter measurement.
//8. Configure number of S-Parameters.
//9. Configure each S-Parameter and format.
//10. Configure Magnitude Units & Phase Trace Type.
//11. Configure Calibration Ports and Calibration Method
//12. Configure Connector type & vCal Resource Name for each VNA port
//13. Initiate Calibration
//14. Acquire Calibration data after user confirmation 
//15. Save Calibration data
//16. Enable Correction
//17. Initiate the Measurement after user confirmation
//18. Read Number of SParams.
//19. Fetch S-Parameter Correction State.
//20. Fetch S-Parameter X data.
//21. Fetch S-Parameter Y data for each S-Parameter.
//22. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVnaSegmentSweep
{
    public class RFmxVnaSegmentSweep
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna;
        string resourceName;

        double port1PowerLevel;
        double port2PowerLevel;
        double port1TestReceiverAttenuation;
        double port2TestReceiverAttenuation;

        double IFBandwidth;

        RFmxVnaMXSegmentPowerLevelEnabled segmentPowerLevelEnabled;
        RFmxVnaMXSegmentIFBandwidthEnabled segmentIFBandwidthEnabled;
        RFmxVnaMXSegmentDwellTimeEnabled segmentDwellTimeEnabled;
        RFmxVnaMXSegmentTestReceiverAttenuationEnabled segmentTestReceiverAttenuationEnabled;

        int numberOfSegments;
        RFmxVnaMXSegmentEnabled[] segmentEnabled;
        double[] segmentStartFrequency;
        double[] segmentStopFrequency;
        int[] segmentNumberOfFrequencyPoints;
        double[] segmentIFBandwidth;
        double[] segmentDwellTime;
        double[] port1SegmentPowerLevel;
        double[] port2SegmentPowerLevel;
        double[] port1SegmentTestReceiverAttenuation;
        double[] port2SegmentTestReceiverAttenuation;

        RFmxVnaMXTriggerType triggerType;
        RFmxVnaMXTriggerMode triggerMode;
        double triggerDelay;

        int numberOfSParams;
        string[] sParamsSParameters;
        RFmxVnaMXSParamsFormat[] sParamsFormats;
        RFmxVnaMXSParamsMagnitudeUnits magnitudeUnits;
        RFmxVnaMXSParamsPhaseTraceType phaseTraceType;

        string vCalResourceName;
        string connectorType;
        double calibrationTimeout;

        string sParamSelectorString;
        string portSelectorString;
        string segmentSelectorString;
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
            port1PowerLevel = -10.0;                                                                                                /* (dBm) */
            port2PowerLevel = -10.0;                                                                                                /* (dBm) */
            port1TestReceiverAttenuation = 0.0;                                                                                     /* (dB) */
            port2TestReceiverAttenuation = 0.0;                                                                                     /* (dB) */
            IFBandwidth = 100.0e3;                                                                                                  /* (Hz) */
            segmentPowerLevelEnabled = RFmxVnaMXSegmentPowerLevelEnabled.True;
            segmentIFBandwidthEnabled = RFmxVnaMXSegmentIFBandwidthEnabled.True;
            segmentTestReceiverAttenuationEnabled = RFmxVnaMXSegmentTestReceiverAttenuationEnabled.True;
            segmentDwellTimeEnabled = RFmxVnaMXSegmentDwellTimeEnabled.True;

            numberOfSegments = 5;
            segmentEnabled = new RFmxVnaMXSegmentEnabled[] { RFmxVnaMXSegmentEnabled.True, RFmxVnaMXSegmentEnabled.True, RFmxVnaMXSegmentEnabled.True,
                RFmxVnaMXSegmentEnabled.True, RFmxVnaMXSegmentEnabled.True };
            segmentStartFrequency = new Double[] { 1000000000.0, 5100000000.0, 10100000000.0, 15100000000.0, 20100000000.0 };       /*(Hz)*/
            segmentStopFrequency = new Double[] {5000000000.0, 10000000000.0, 15000000000.0, 20000000000.0, 26500000000.0};         /*(Hz)*/
            segmentNumberOfFrequencyPoints = new int[] {41, 50, 50, 50, 65};
            segmentIFBandwidth = new double[] { 100000.0, 1000000.0, 100000.0, 10000.0, 100000.0 };                                 /*(Hz)*/
            segmentDwellTime = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0 };                                                            /*(s)*/
            port1SegmentPowerLevel = new double[] { -10.0, -10.0, -10.0, -10.0, -10.0 };                                            /*(dBm)*/
            port1SegmentTestReceiverAttenuation = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0 };                                         /*(dB)*/
            port2SegmentPowerLevel = new double[] { -10.0, -10.0, -10.0, -10.0, -10.0 };                                            /*(dBm)*/
            port2SegmentTestReceiverAttenuation = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0 };                                         /*(dB)*/

            triggerType = RFmxVnaMXTriggerType.None;
            triggerMode = RFmxVnaMXTriggerMode.Segment;
            triggerDelay = 0.0;                                                                 /*seconds*/

            numberOfSParams = 4;
            sParamsSParameters = new string[] { "S11", "S12", "S21", "S22" };
            sParamsFormats = new RFmxVnaMXSParamsFormat[] { RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude };

            magnitudeUnits = RFmxVnaMXSParamsMagnitudeUnits.dB;
            phaseTraceType = RFmxVnaMXSParamsPhaseTraceType.Wrapped;

            vCalResourceName = "vCal";
            connectorType = "3.5 mm female";
            calibrationTimeout = 100.0;                                                                                             /*seconds */

            timeout = 10.0;                                                                                                         /*seconds */
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureVna()
        {
            vna = instrSession.GetVnaSignalConfiguration();                                                                         /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", RFmxInstrMXConstants.PxiClock, 100e6);                                     /*(Hz)*/

            vna.SetIFBandwidth("", IFBandwidth);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1");
            vna.SetPowerLevel(portSelectorString, port1PowerLevel);
            vna.SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2");
            vna.SetPowerLevel(portSelectorString, port2PowerLevel);
            vna.SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation);

            vna.SetSweepType("", RFmxVnaMXSweepType.Segment);
            vna.SetNumberOfSegments("", numberOfSegments);
            vna.SetSegmentPowerLevelEnabled("", segmentPowerLevelEnabled);
            vna.SetSegmentIFBandwidthEnabled("", segmentIFBandwidthEnabled);
            vna.SetSegmentTestReceiverAttenuationEnabled("", segmentTestReceiverAttenuationEnabled);
            vna.SetSegmentDwellTimeEnabled("", segmentDwellTimeEnabled);
            for (int i = 0; i < numberOfSegments; i++)
            {
                segmentSelectorString = RFmxVnaMX.BuildSegmentString("", i);
                vna.SetSegmentEnabled(segmentSelectorString, segmentEnabled[i]);
                vna.SetSegmentStartFrequency(segmentSelectorString, segmentStartFrequency[i]);
                vna.SetSegmentStopFrequency(segmentSelectorString, segmentStopFrequency[i]);
                vna.SetSegmentNumberOfFrequencyPoints(segmentSelectorString, segmentNumberOfFrequencyPoints[i]);
                vna.SetSegmentIFBandwidth(segmentSelectorString, segmentIFBandwidth[i]);
                vna.SetSegmentDwellTime(segmentSelectorString, segmentDwellTime[i]);
                portSelectorString = RFmxVnaMX.BuildPortString(segmentSelectorString, "port1");
                vna.SetSegmentPowerLevel(portSelectorString, port1SegmentPowerLevel[i]);
                vna.SetSegmentTestReceiverAttenuation(portSelectorString, port1SegmentTestReceiverAttenuation[i]);
                portSelectorString = RFmxVnaMX.BuildPortString(segmentSelectorString, "port2");
                vna.SetSegmentPowerLevel(portSelectorString, port2SegmentPowerLevel[i]);
                vna.SetSegmentTestReceiverAttenuation(portSelectorString, port2SegmentTestReceiverAttenuation[i]);
            }

            vna.SetTriggerType("", triggerType);
            vna.SetTriggerMode("", triggerMode);
            vna.SetTriggerDelay("", triggerDelay);

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

            vna.SetCorrectionCalibrationPorts("", new string[] { "port1", "port2" });
            vna.SetCorrectionCalibrationMethod("", RFmxVnaMXCorrectionCalibrationMethod.Solt);

            vna.SetCorrectionCalibrationConnectorType("port::all", connectorType);
            vna.SetCorrectionCalibrationCalkitElectronicResourceName("port::all", vCalResourceName);

            vna.CalibrationInitiate("");
            Console.WriteLine("Connect Port A of NI CAL-5501 to Port 1 of NI PXIe-5633,  and Port B of NI CAL-5501 to Port 2 of NI PXIe-5633.");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            vna.CalibrationAcquire("", calibrationTimeout);
            vna.CalibrationSave("", "");
            Console.WriteLine("Connect DUT across port1 and port2 of NI PXIe-5633.");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();

            vna.SetCorrectionEnabled("", RFmxVnaMXCorrectionEnabled.True);
            vna.Initiate("", "");
        }

        void RetrieveResults()
        {
            vna.SParams.Configuration.GetNumberOfSParameters("", out numberOfSParamsResult);
            vna.SParams.Results.GetCorrectionState("", out correctionStateResult);
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
