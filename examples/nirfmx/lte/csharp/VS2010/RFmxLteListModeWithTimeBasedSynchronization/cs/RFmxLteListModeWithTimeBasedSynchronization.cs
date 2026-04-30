// Steps:
// 1. Open NI - RFSG session. 
// 2. Configure RFSG generation mode to Script. This allows for writing a script that determines the behavior of the generation.
// 3. Configure RFSG Frequency Reference (Clock Source and Clock Frequency).
// 4. Configure RFSG Selected Ports, External Gain and Frequency of RF output signal.
// 5. Get the terminal name for marker0. Use this as the source to configure RFSG List to advance upon receipt of a marker event.
// 6. Read waveform from file and download waveform from file to RFSG.
// 7. Retrieve waveform sample rate from the waveform file.
// 8. Retrieve the value of PAPR from the waveform file.
// 9. Write script to generate a waveform. This script is programmed to continuously generate a waveform of length
//    equal to the RFmx list step duration and generate marker0 at the end of list step acquisition.
// 10. Create a Configuration List. Pass Power Level in the Configuration List Properties parameter to be able to configure
//    Power Level in each step that we create.The Set As Active List parameter in this method defaults to true, this will set the
//    Active Configuration List property to the name of the created configuration list. Once the Active Configuration List
//    is set, using a property node to access Power Level will modify the property for this configuration list.
// 11. Create a Configuration List Step.The Set As Active Step parameter in this method defaults to true, this will set the Active
//     Configuration List Step property to the created configuration list step index. Once the Active Configuration List
//     Step is set, using a property node to access Power Level will modify the property for this configuration list step in
//     the configuration list indicated by the Active Configuration List property.
// 12. Configure the Power Level for the Active Configuration List Step in the Active Configuration List.
// 13. Open a new RFmx Session.
// 14. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
// 15. Create RFmx Lte List.
// 16. Create RFmx Lte List Step.
// 17. Configure the Reference Level for the Specified List Step in the Lte List.
// 18. Configure Trigger Parameters for IQ Power Edge Trigger on List Step0.
// 19. Configure Trigger Parameters for Digital Edge Time Trigger on List Step1 to N - 1.
// 20. Configure List Step Timer Offset for List Step1 to N - 1
// 21. Configure List Step Timer Duration for all List steps.
// 22. Configure Center Frequency, Selected Portand External Attenuation for all List Step.
// 23. Configure List Step Timer Unit as Time for all List Step.
// 24. Configure Link Direction,  Carrier Bandwidthand ,Duplex Scheme for all List Step.
// 25. Configure Sweep Time Parameters for all List Step.
// 26. Select ACP measurement and enable Traces for all List Step.
// 27. Initiate ACP measurement for Lte List.
// 28. Initiate signal generation.
// 29. Wait for Acquisition to complete.
// 30. Fetch ACP measurement Results for all Configuration List Steps one by one.
// 31. Fetch ACP Traces for the desired List Step.
// 32. Stop signal generation.
// 33. Delete the Configuration List to avoid committing the Configuration List to the hardware with subsequent
//     calls to Commit. Deleting the list will reset the Active Configuration List.
// 34. Delete RFmx Lte List.
// 35. Close the RFmx Session.
// 36. Close the RFSG session. 
//     It is recommended to clear the waveform before closing RFSG session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsgPlayback;

namespace NationalInstruments.Examples.RFmxLteListModeWithTimeBasedSynchronization
{
    public class RFmxLteListModeWithTimeBasedSynchronization
    {
        RFmxInstrMX instrSession;
        RFmxLteMXList LteList;
        NIRfsg rfsgSession;
        IntPtr instrumentHandle;

        double centerFrequency;

        string rfsgResourceName;
        string rfsgSelectedPorts;
        string waveformFilePath;
        string waveformName;

        double rfsgExternalAttenuation;

        RfsgFrequencyReferenceSource rfsgFrequencyReferenceSource;
        double rfsgFrequency;

        string rfsaResourceName;
        string rfsaSelectedPorts;
        double rfsaExternalAttenuation;

        string rfsaFrequencyReferenceSource;
        double rfsaFrequency;

        double step0IQPowerEdgeLevel;
        double step0MinimumQuietTime;
        double triggerDelay;

        int numberOfSteps;

        RFmxLteMXListStepTimerUnit listStepTimerUnit;
        double listStepTimerDuration;
        double listStepTimerOffset;

        double startReferenceLevel;
        double stopReferenceLevel;


        RFmxLteMXDuplexScheme duplexScheme;
        RFmxLteMXLinkDirection linkDirection;

        double carrierBandwidth;


        double sweepTimeInterval;

        int traceStepNumber;

        double[] rampPattern;
        double timeout;
        string script;
        string markerEventTerminalName;
        double sampleRate;
        int numberOfSamples;
        int markerLocation;
        double papr;
        RFmxLteMX[] step;

        struct OffsetMeasurement
        {
            public double[] lowerRelativePower;                                  /* (dBm)*/
            public double[] upperRelativePower;                                  /* (dBm)*/
            public double[] lowerAbsolutePower;                                  /* (dBm or dBm/Hz)*/
            public double[] upperAbsolutePower;                                  /* (dBm or dBm/Hz)*/
        };
        OffsetMeasurement[] offsetMeasurementObject;
        double[] absolutePower;                                                  /* (dBm or dBm/Hz)*/
        double[] totalRelativePower;                                             /*(dBm)*/
        Spectrum<float> relativePowersTrace;
        Spectrum<float> spectrum;

        public void Run()
        {
            try
            {
                InitializeVariables();
                ConfigureRfsg();
                ConfigureRFmx();
                RetrieveResults();
                PrintResults();
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
            finally
            {
                CloseSession();
                Console.WriteLine("\nPress any key to exit");
                Console.ReadKey();
            }
        }

        void InitializeVariables()
        {
            centerFrequency = 1.95e9;                                            /* (Hz) */

            rfsgResourceName = "RFSG";
            rfsgSelectedPorts = "";
            waveformFilePath = "LTE_UL_FDD_CC-1_BW-10MHz_PUSCH-QPSK.tdms";
            waveformName = "Wfm";

            rfsgExternalAttenuation = 0.0;                                       /* (dB) */

            rfsgFrequencyReferenceSource = RfsgFrequencyReferenceSource.OnboardClock;
            rfsgFrequency = 10.0e6;                                              /* (Hz) */

            rfsaResourceName = "RFSA";
            rfsaSelectedPorts = "";
            rfsaExternalAttenuation = 0.0;                                       /* (dB) */

            rfsaFrequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            rfsaFrequency = 10.0e6;                                              /* (Hz) */

            step0IQPowerEdgeLevel = -10;                                         /* (dB) */
            step0MinimumQuietTime = 0.0;                                         /* (s) */
            triggerDelay = 0.0;                                                  /* (s) */

            numberOfSteps = 10;

            listStepTimerUnit = RFmxLteMXListStepTimerUnit.Time;
            listStepTimerDuration = 1.50e-3;                                     /* (s) */
            listStepTimerOffset = 0.0;                                           /* (s) */

            startReferenceLevel = -20.0;                                         /* (dBm) */
            stopReferenceLevel = 0.0;                                            /* (dBm) */

            duplexScheme = RFmxLteMXDuplexScheme.Fdd;
            linkDirection = RFmxLteMXLinkDirection.Uplink;

            carrierBandwidth = 10e6;                                             /* (Hz) */


            sweepTimeInterval = 1.0e-3;                                          /* (s) */

            traceStepNumber = 0;

            rampPattern = new double[numberOfSteps];
            LinearRampPattern(startReferenceLevel, stopReferenceLevel, numberOfSteps, false, ref rampPattern);

            timeout = 10.0;                                                      /* (s) */

            step = new RFmxLteMX[numberOfSteps];
            absolutePower = new double[numberOfSteps];
            totalRelativePower = new double[numberOfSteps];
        }

        void ConfigureRfsg()
        {
            rfsgSession = new NIRfsg(rfsgResourceName, true, false);
            rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
            rfsgSession.FrequencyReference.Configure(rfsgFrequencyReferenceSource, rfsgFrequency);
            
            rfsgSession.RF.ExternalGain = -1 * rfsgExternalAttenuation;
            rfsgSession.SignalPath.SelectedPorts = rfsgSelectedPorts;
            rfsgSession.RF.Frequency = centerFrequency;

            markerEventTerminalName = rfsgSession.DeviceEvents.MarkerEvents[0].TerminalName;
            rfsgSession.Triggers.ConfigurationListStepTrigger.DigitalEdge.Configure(markerEventTerminalName,
                RfsgTriggerEdge.RisingEdge);
            instrumentHandle = rfsgSession.GetInstrumentHandle().DangerousGetHandle();
            rfsgSession.Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, waveformFilePath, 0);
            sampleRate = rfsgSession.Arb.Waveforms[waveformName].IQRate;
            papr = rfsgSession.Arb.Waveforms[waveformName].Papr;
            numberOfSamples = (int)(sampleRate * listStepTimerDuration);
            markerLocation = (int)(sampleRate * sweepTimeInterval);
            script = string.Format("script GenerateWaveform\n  repeat forever\n" +
                "generate {0} subset(0, {1}) marker0({2})\n" +
                "  end repeat\nend script", waveformName, numberOfSamples, markerLocation);
            rfsgSession.Arb.Scripting.WriteScript(script);

            RfsgConfigurationListProperties[] properties = new RfsgConfigurationListProperties[1]
            { RfsgConfigurationListProperties.PowerLevel };
            rfsgSession.BasicConfigurationList.CreateConfigurationList("PowerLevelList", properties, true);
            for (int i = 0; i < numberOfSteps; i++)
            {
                rfsgSession.BasicConfigurationList.CreateStep(true);
                rfsgSession.RF.PowerLevel = rampPattern[i];
            }

        }

        void ConfigureRFmx()
        {
            instrSession = new RFmxInstrMX(rfsaResourceName, "");
            instrSession.ConfigureFrequencyReference("", rfsaFrequencyReferenceSource, rfsaFrequency);

            LteList = instrSession.GetLteList("ModAcc_List");
            for (int i = 0; i < numberOfSteps; i++)
            {
                step[i] = LteList.CreateListStep();
                step[i].SetReferenceLevel("", papr + rampPattern[i]);

                if (i == 0)
                {
                    step[i].ConfigureIQPowerEdgeTrigger("", "0", RFmxLteMXIQPowerEdgeTriggerSlope.Rising, step0IQPowerEdgeLevel,
                        triggerDelay, RFmxLteMXTriggerMinimumQuietTimeMode.Manual, step0MinimumQuietTime,
                        RFmxLteMXIQPowerEdgeTriggerLevelType.Relative, true);
                }
                else
                {
                    step[i].ConfigureDigitalEdgeTrigger("", "TimerEvent", RFmxLteMXDigitalEdgeTriggerEdge.Rising,
                        triggerDelay, true);
                    step[i].SetListStepTimerOffset("", listStepTimerOffset);
                }

                step[i].SetListStepTimerDuration("", listStepTimerDuration);
            }

            RFmxLteMX stepAll = LteList.GetListStepAll();
            stepAll.ConfigureFrequency("", centerFrequency);
            stepAll.SetSelectedPorts("", rfsaSelectedPorts);
            stepAll.ConfigureExternalAttenuation("", rfsaExternalAttenuation);
            stepAll.SetListStepTimerUnit("", listStepTimerUnit);
            stepAll.SetLinkDirection("", linkDirection);
            stepAll.ComponentCarrier.SetBandwidth("", carrierBandwidth);
            stepAll.SetDuplexScheme("", duplexScheme);

            stepAll.SelectMeasurements("", RFmxLteMXMeasurementTypes.Acp, true);
            stepAll.Acp.Configuration.ConfigureSweepTime("", RFmxLteMXAcpSweepTimeAuto.False, sweepTimeInterval);

            LteList.Initiate("", "");
            rfsgSession.Initiate();
            instrSession.WaitForAcquisitionComplete(timeout);
        }

        void RetrieveResults()
        {


            offsetMeasurementObject = new OffsetMeasurement[numberOfSteps];
            for (int i = 0; i < numberOfSteps; i++)
            {
                step[i].Acp.Results.FetchOffsetMeasurementArray("", timeout, ref offsetMeasurementObject[i].lowerRelativePower,
                    ref offsetMeasurementObject[i].upperRelativePower, ref offsetMeasurementObject[i].lowerAbsolutePower,
                    ref offsetMeasurementObject[i].upperAbsolutePower);

                step[i].Acp.Results.ComponentCarrier.FetchMeasurement("", timeout, out absolutePower[i],
                    out totalRelativePower[i]);
            }

            for (int i = 0; i < offsetMeasurementObject[traceStepNumber].lowerRelativePower.Length; i++)
            {
                step[traceStepNumber].Acp.Results.FetchRelativePowersTrace("", timeout, i, ref relativePowersTrace);
            }

            step[traceStepNumber].Acp.Results.FetchSpectrum("", timeout, ref spectrum);
        }

        void PrintResults()
        {
            Console.WriteLine("\n-----------Measurements----------- \n");
            for (int i = 0; i < numberOfSteps; i++)
            {
                Console.WriteLine("Step {0}: ", i);
                Console.WriteLine("\n-----------Carrier Measurements----------- \n");
                Console.WriteLine("\nAbsolute Power (dBm or dBm/Hz) : {0}", absolutePower[i]);
                Console.WriteLine("\n-----------Offset Channel Measurements----------- \n");
                for (int j = 0; j < offsetMeasurementObject[i].lowerRelativePower.Length; j++)
                {
                    Console.WriteLine("Offset {0}: ", j);
                    Console.WriteLine("Lower Relative Power (dB)              : {0}", offsetMeasurementObject[i].lowerRelativePower[j]);
                    Console.WriteLine("Upper Relative Power (dB)              : {0}", offsetMeasurementObject[i].upperRelativePower[j]);
                    Console.WriteLine("Lower Absolute Power (dBm or dBm/Hz)   : {0}", offsetMeasurementObject[i].lowerAbsolutePower[j]);
                    Console.WriteLine("Upper Absolute Power (dBm or dBm/Hz)   : {0}", offsetMeasurementObject[i].upperAbsolutePower[j]);
                    Console.WriteLine("-------------------------------------------------\n");
                }
                Console.WriteLine("-------------------------------------------------\n");
            }
        }

        void CloseSession()
        {
            if (LteList != null)
            {
                LteList.Dispose();
                LteList = null;
            }
            if (instrSession != null)
            {
                instrSession.Close();
                instrSession = null;
            }
            if (rfsgSession != null)
            {
                rfsgSession.Abort();
                rfsgSession.BasicConfigurationList.DeleteConfigurationList("PowerLevelList");
                rfsgSession.Arb.ClearWaveform(waveformName);
                rfsgSession.Close();
                rfsgSession = null;
            }
        }

        static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }

        void LinearRampPattern(double start, double end, int samples, bool includeEnd, ref double[] rampPattern)
        {
            int m = includeEnd ? samples : (samples - 1);
            double delta = (end - start) / m;
            for (int i = 0; i < samples; i++)
            {
                rampPattern[i] = start + (i * delta);
            }
        }

    }
}
