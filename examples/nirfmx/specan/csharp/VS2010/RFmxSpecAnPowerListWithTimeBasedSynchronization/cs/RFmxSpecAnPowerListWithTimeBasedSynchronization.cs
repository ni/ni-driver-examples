//Steps:
//1.Open a session to the NI-RFSG.
//2. Configure RFSG generation mode to Script. This allows for writing a script that determines the behavior of the generation.
//3. Configure the Reference Clock Source and Frequency.
//4. Configure Frequency Setting Units and Frequency Settings.
//5. Configure following properties:
//  - External Gain
//  - Selected Ports
//6. Get the terminal name for marker0. Use this as the source to configure RFSG List to advance upon receipt of a marker event
//7. Read waveform from file and download waveform from file to RFSG.
//8. Retrieve the waveform sample rate and waveform PAPR. Add the retrieved PAPR to the RFSA reference level while configuring to every list step.
//9. Write script to generate a waveform. This script is programmed to continuously generate a waveform of length equal to the RFmx list step duration 
//   and generate marker0 at the end of list step acquisition.
//10. Create a Configuration List. Pass Frequency and Power Level in the Configuration List Properties parameter to be able to configure Frequency and 
//    Power Level in each step that is created. The Set As Active List parameter in this VI defaults to true, this will set the Active Configuration List   
//    property to the name of the created configuration list. Once the Active Configuration List is set, use a property node to access Power Level will    
//    modify the property for this configuration list.
//11. Create a Configuration List Step. The Set As Active Step parameter in this VI defaults to true, this will set the Active Configuration List Step property    
//    to the created configuration list step index. Once the Active Configuration List Step is set, using a property node to access Frequency or Power 
//    Level will modify the property for this configuration list step, in the configuration list indicated by the Active Configuration List property.
//12. Configure the Frequency and Power Level for the Active Configuration List Step in the Active Configuration List.
//13. Open a new RFmx Session.
//14. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
//15 - 17. Configure Selected Ports, External Attenuation and Trigger Parameters
//18. Configure Segment Parameters like Number of Segments, Frequency, Reference Level, Measurement Length, Trigger Type, Segment Length and RBW Filter.
//19. Select PowerList measurement.
//20. Initiate PowerList measurement.
//21. Initiate signal generation.
//22. Wait for Acquisition to complete.
//23. Stop signal generation.
//24. Delete the Configuration List to avoid committing the Configuration List to the hardware with subsequent calls to Commit. 
//    Deleting the list will reset the Active Configuration List.
//25. Fetch PowerList measurement results for all segments.
//26. Close the RFmx Session.
//27. Close the RFSG session. 
//    It is recommended to clear the waveform before closing RFSG session.
//32. Delete the Configuration List to avoid committing the Configuration List to the hardware with subsequent calls to Commit.
//    Deleting the list will reset the Active Configuration List. 
//33. Delete RFmx SpecAn List
//34. Close the RFmx Session.
//35. Close the RFSG session.
//    It is recommended to clear the waveform before closing RFSG session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsgPlayback;
using System.Threading;

namespace NationalInstruments.Examples.RFmxSpecAnPowerListWithTimeBasedSynchronization
{
    public class RFmxSpecAnPowerListWithTimeBasedSynchronization
    {
        RFmxInstrMX instrSession;
        NIRfsg rfsgSession;
        IntPtr instrumentHandle;

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

        double segment0IQPowerEdgeLevel;
        double segment0MinimumQuietTime;
        double triggerDelay;

        int numberOfSegments;

        double StartCenterFrequency;
        double StopCenterFrequency;

        double startReferenceLevel;
        double stopReferenceLevel;

        double[] measurementLengthArray;
        double[] referenceLevelArray;
        int[] segmentTriggerArray;
        double[] segmentLengthArray;
        double[] rbwArray;
        int[] rbwFilterTypeArray;
        double[] rbwRrcAlphaArray;



        double measurementLength;
        double segmentLength;
        double rbw, rrcAlpha;
        RFmxSpecAnMXTxpRbwFilterType rbwFilterType;
        RFmxSpecAnMX specAn;
        double[] referenceLevelRampPattern;
        double[] centerFrequencyRampPattern;
        double timeout;
        string script;
        string markerEventTerminalName;
        double sampleRate;
        int numberOfSamples;
        int markerLocation;
        double papr;


        enum segmentTriggerType
        {   None,
            DigitalEdge,
            IQPowerEdge,
        }


        double[] meanAbsolutePower;
        double[] maximumPower;
        double[] minimumPower;


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
            rfsgResourceName = "RFSG";
            rfsgSelectedPorts = "";
            waveformFilePath = "WCDMA_Uplink_DPCH_Waveform.tdms";
            waveformName = "Wfm";

            rfsgExternalAttenuation = 0.0;                                       /* (dB) */

            rfsgFrequencyReferenceSource = RfsgFrequencyReferenceSource.OnboardClock;
            rfsgFrequency = 10.0e6;                                              /* (Hz) */

            rfsaResourceName = "RFSA";
            rfsaSelectedPorts = "";
            rfsaExternalAttenuation = 0.0;                                       /* (dB) */

            rfsaFrequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            rfsaFrequency = 10.0e6;                                              /* (Hz) */

            segment0IQPowerEdgeLevel = -10;                                      /* (dB) */
            segment0MinimumQuietTime = 0.0;                                      /* (s) */
            triggerDelay = 0.0;                                                  /* (s) */

            numberOfSegments = 10;

            StartCenterFrequency = 1e9;                                         /* (s) */
            StopCenterFrequency = 2e9;                                          /* (s) */

            startReferenceLevel = -20.0;                                        /* (dBm) */
            stopReferenceLevel = 0.0;                                           /* (dBm) */

            measurementLength = 1e-3;                                           /* (Hz) */
            segmentLength = 1.5e-3;                                             /* (Hz) */
            rbw = 5e+6;                                                         /* (Hz) */
            rbwFilterType = RFmxSpecAnMXTxpRbwFilterType.Flat;
            rrcAlpha = 0.010;
            measurementLengthArray = new double[numberOfSegments];
            referenceLevelArray = new double[numberOfSegments];
            segmentTriggerArray = new int[numberOfSegments];
            segmentLengthArray = new double[numberOfSegments];
            rbwArray = new double[numberOfSegments];
            rbwFilterTypeArray = new int[numberOfSegments];
            rbwRrcAlphaArray = new double[numberOfSegments];

            centerFrequencyRampPattern = new double[numberOfSegments];
            referenceLevelRampPattern = new double[numberOfSegments];
            LinearRampPatternReverse(StartCenterFrequency, StopCenterFrequency, numberOfSegments, false, ref centerFrequencyRampPattern);
            LinearRampPattern(startReferenceLevel, stopReferenceLevel, numberOfSegments, false, ref referenceLevelRampPattern);

            timeout = 10.0;                                                     /* (s) */
        }

        void ConfigureRfsg()
        {
            rfsgSession = new NIRfsg(rfsgResourceName, true, false);
            rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
            rfsgSession.FrequencyReference.Configure(rfsgFrequencyReferenceSource, rfsgFrequency);
            rfsgSession.RF.Advanced.FrequencySettlingUnits = NationalInstruments.ModularInstruments.NIRfsg.RfsgRFFrequencySettlingUnits.TimeAfterIO;
            rfsgSession.RF.Advanced.FrequencySettlingTime = 0.001;
            rfsgSession.RF.ExternalGain = -1 * rfsgExternalAttenuation;
            rfsgSession.SignalPath.SelectedPorts = rfsgSelectedPorts;

            markerEventTerminalName = rfsgSession.DeviceEvents.MarkerEvents[0].TerminalName;
            rfsgSession.Triggers.ConfigurationListStepTrigger.DigitalEdge.Configure(markerEventTerminalName, RfsgTriggerEdge.RisingEdge);

            instrumentHandle = rfsgSession.GetInstrumentHandle().DangerousGetHandle();
            rfsgSession.Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, waveformFilePath, 0);
            sampleRate = rfsgSession.Arb.Waveforms[waveformName].IQRate;
            papr = rfsgSession.Arb.Waveforms[waveformName].Papr;
            numberOfSamples = (int)(sampleRate * segmentLength);
            markerLocation = (int)(sampleRate * measurementLength);
            script = string.Format("script GenerateWaveform\n  repeat forever\n    generate {0} subset(0, {1}) marker0({2})\n" +
                "  end repeat\nend script", waveformName, numberOfSamples, markerLocation);
            rfsgSession.Arb.Scripting.WriteScript(script);

            RfsgConfigurationListProperties[] properties = new RfsgConfigurationListProperties[2]
            {   RfsgConfigurationListProperties.Frequency,
             RfsgConfigurationListProperties.PowerLevel
            };
            rfsgSession.BasicConfigurationList.CreateConfigurationList("PowerList", properties, true);
            for (int i = 0; i < numberOfSegments; i++)
            {
                rfsgSession.BasicConfigurationList.CreateStep(true);
                rfsgSession.RF.Frequency = centerFrequencyRampPattern[i];
                rfsgSession.RF.PowerLevel = referenceLevelRampPattern[i];
            }
        }

        void ConfigureRFmx()
        {
            instrSession = new RFmxInstrMX(rfsaResourceName, "");
            instrSession.ConfigureFrequencyReference("", rfsaFrequencyReferenceSource, rfsaFrequency);
            specAn = instrSession.GetSpecAnSignalConfiguration();
            specAn.SetSelectedPorts("", rfsaSelectedPorts);
            specAn.ConfigureExternalAttenuation("", rfsaExternalAttenuation);
            specAn.SetDigitalEdgeTriggerSource("", RFmxSpecAnMXConstants.TimerEvent);
            specAn.SetIQPowerEdgeTriggerSource("", "0");
            specAn.SetIQPowerEdgeTriggerLevelType("", RFmxSpecAnMXIQPowerEdgeTriggerLevelType.Relative);
            specAn.SetIQPowerEdgeTriggerLevel("", segment0IQPowerEdgeLevel);
            specAn.SetTriggerMinimumQuietTimeDuration("", segment0MinimumQuietTime);
            specAn.SetTriggerDelay("", triggerDelay);


            specAn.PowerList.Configuration.SetNumberOfSegments("", numberOfSegments);
            specAn.PowerList.Configuration.SetSegmentFrequency("", centerFrequencyRampPattern);

            for (int i = 0; i < numberOfSegments; ++i)
            {
                referenceLevelArray[i] = referenceLevelRampPattern[i] + papr;
            }
            specAn.PowerList.Configuration.SetSegmentReferenceLevel("", referenceLevelArray);
            for (int i = 0; i < numberOfSegments; i++)
            {
                measurementLengthArray[i] = measurementLength;
            }
            specAn.PowerList.Configuration.SetSegmentMeasurementLength("", measurementLengthArray);
            segmentTriggerArray[0] = (int)segmentTriggerType.IQPowerEdge;
            for (int i = 1; i < numberOfSegments; i++)
            {
                segmentTriggerArray[i] = (int)segmentTriggerType.DigitalEdge;
            }
            specAn.PowerList.Configuration.SetSegmentTriggerType("", segmentTriggerArray);
            for (int i = 0; i < numberOfSegments; i++)
            {
                segmentLengthArray[i] = segmentLength;
            }
            specAn.PowerList.Configuration.SetSegmentLength("", segmentLengthArray);
            for (int i = 0; i < numberOfSegments; i++)
            {
                rbwArray[i] = rbw;
            }
            specAn.PowerList.Configuration.SetSegmentRbwFilterBandwidth("", rbwArray);
            for (int i = 0; i < numberOfSegments; i++)
            {
                rbwFilterTypeArray[i] = (int)rbwFilterType;
            }
            specAn.PowerList.Configuration.SetSegmentRbwFilterType("", rbwFilterTypeArray);
            for (int i = 0; i < numberOfSegments; i++)
            {
                rbwRrcAlphaArray[i] = rrcAlpha;
            }
            specAn.PowerList.Configuration.SetSegmentRbwFilterAlpha("", rbwRrcAlphaArray);

            specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.PowerList, false);

            specAn.Initiate("", "");
            rfsgSession.Initiate();
            instrSession.WaitForAcquisitionComplete(timeout);
        }

        void RetrieveResults()
        {
            specAn.PowerList.Results.FetchMinimumPowerArray("", timeout, ref minimumPower);
            specAn.PowerList.Results.FetchMaximumPowerArray("", timeout, ref maximumPower);
            specAn.PowerList.Results.FetchMeanAbsolutePowerArray("", timeout, ref meanAbsolutePower);
        }


        void PrintResults()
        {
            Console.WriteLine("\n-----------Measurements----------- \n");
            for (int i = 0; i < numberOfSegments; i++)
            {
                Console.WriteLine("Offset {0}: ", i);
                Console.WriteLine("Mean Absolute Power (dBm)     :  {0}", meanAbsolutePower[i]);
                Console.WriteLine("Maximum Power (dBm)           :  {0}", maximumPower[i]);
                Console.WriteLine("Minimum Power (dBm)           :  {0}", minimumPower[i]);
                Console.WriteLine("-------------------------------------------------\n");
            }
        }

        void CloseSession()
        {
            if (specAn != null)
            {
                specAn.Dispose();
                specAn = null;
            }
            if (instrSession != null)
            {
                instrSession.Close();
                instrSession = null;
            }
            if (rfsgSession != null)
            {
                rfsgSession.Abort();
                rfsgSession.BasicConfigurationList.DeleteConfigurationList("PowerList");
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
        void LinearRampPatternReverse(double start, double end, int samples, bool includeEnd, ref double[] rampPattern)
        {
            int m = includeEnd ? samples - 1 : (samples - 1);
            double delta = (start - end) / m;
            for (int i = 0; i < samples; i++)
            {
                rampPattern[i] = end + (i * delta);
            }
        }
    }
}
