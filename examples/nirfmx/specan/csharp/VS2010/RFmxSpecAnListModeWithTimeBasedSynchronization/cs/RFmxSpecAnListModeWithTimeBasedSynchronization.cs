//Steps:
//1.  Open NI - RFSG session. 
//2.  Configure RFSG generation mode to Script. This allows for writing a script that determines the behavior of the generation.
//3.  Configure RFSG Frequency Reference (Clock Source and Clock Frequency).
//4.  Configure RFSG Selected Ports, External Gain and Frequency of RF output signal.
//5.  Get the terminal name for marker0. Use this as the source to configure RFSG List to advance upon receipt of a 
//    marker event.
//6.  Read waveform from file and download Waveform from file to RFSG. 
//7.  Retrieve waveform sample rate from the waveform file.
//8.  Retrieve the value of PAPR from the waveform file.
//9.  Write script to generate a waveform. This script is programmed to continuously generate a waveform of length
//    equal to the RFmx list step duration and generate marker0 at the end of list step acquisition. 
//10.  Create a Configuration List. Pass Power Level in the Configuration List Properties parameter to be able to configure
//    Power Level in each step that we create.The Set As Active List parameter in this method defaults to true, this will set the
//    Active Configuration List property to the name of the created configuration list.Once the Active Configuration List
//    is set, using a property node to access Power Level will modify the property for this configuration list.
//11. Create a Configuration List Step.The Set As Active Step parameter in this method defaults to true, this will set the Active
//    Configuration List Step property to the created configuration list step index. Once the Active Configuration List
//    Step is set, using a property node to access Power Level will modify the property for this configuration list step in
//    the configuration list indicated by the Active Configuration List property.
//12. Configure the Power Level for the Active Configuration List Step in the Active Configuration List.
//13. Open a new RFmx Session.
//14. Configure the Frequency Reference properties(Clock Source and Clock Frequency). 
//15. Create RFmx SpecAn List.
//16. Create RFmx SpecAn List Step.
//17. Configure the Reference Level for the Specified List Step in the SpecAn List.
//18. Configure Trigger Parameters for IQ Power Edge Trigger on List Step0.
//19. Configure Trigger Parameters for Digital Edge Time Trigger on List Step1 to N - 1.
//20. Configure List Step Timer Offset for List Step1 to N - 1
//21. Configure List Step Timer Duration for all List steps.
//22. Configure Center Frequency, Selected Ports, External Attenuation for all Configuration List Step.
//23. Configure Sweep Time, RBW Filter, FFT parameters for all List Steps.
//24. Configure Integration BW of the Carrier channel, Number of Offset Channelsand Channel Spacing for all List Steps.
//25. Configure Carrier and Offset RRC Filter for all List Steps.
//26. Select ACP measurement and disable traces for all List Steps.
//27. Initiate ACP measurement for List.
//28. Initiate signal generation.
//29. Wait for Acquisition to complete.
//30. Fetch ACP measurement Results for all List steps one by one.
//31. Stop signal generation.
//32. Delete the Configuration List to avoid committing the Configuration List to the hardware with subsequent
//    calls to Commit. Deleting the list will reset the Active Configuration List. 
//33. Delete RFmx SpecAn List
//34. Close the RFmx Session.
//35. Close the RFSG session.
//    It is recommended to clear the waveform before closing RFSG session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsgPlayback;

namespace NationalInstruments.Examples.RFmxSpecAnListModeWithTimeBasedSynchronization
{
   public class RFmxSpecAnListModeWithTimeBasedSynchronization
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMXList SpecAnList;
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

      double listStepTimerDuration;
      double listStepTimerOffset;

      double startReferenceLevel;
      double stopReferenceLevel;

      double integrationBandwidth;
      double channelSpacing;
      double sweepTimeInterval;

      double[] rampPattern;
      double timeout;
      string script;
      string markerEventTerminalName;
      double sampleRate;
      int numberOfSamples;
      int markerLocation;
      double papr;
      RFmxSpecAnMX[] step;
      double[] absolutePower;

      string offsetString;
      string carrierString;

      int numberOfOffsetChannels;

      struct OffsetMeasurement
      {
         public double[] lowerRelativePower;
         public double[] upperRelativePower;
         public double[] lowerAbsolutePower;
         public double[] upperAbsolutePower;
      };
      OffsetMeasurement[] offsetMeasurementObject;


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

         step0IQPowerEdgeLevel = -10;                                         /* (dB) */
         step0MinimumQuietTime = 0.0;                                         /* (s) */
         triggerDelay = 0.0;                                                  /* (s) */

         numberOfSteps = 10;

         listStepTimerDuration = 1.0e-3;                                      /* (s) */
         listStepTimerOffset = 0.0;                                           /* (s) */

         startReferenceLevel = -20.0;                                         /* (dBm) */
         stopReferenceLevel = 0.0;                                            /* (dBm) */

         integrationBandwidth = 3.840e6;                                      /* (Hz) */
         channelSpacing = 5.0e6;                                              /* (Hz) */
         sweepTimeInterval = 666.67e-6;                                       /* (s) */

         numberOfOffsetChannels = 2;

         rampPattern = new double[numberOfSteps];
         LinearRampPattern(startReferenceLevel, stopReferenceLevel, numberOfSteps, false, ref rampPattern);

         timeout = 10.0;                                                      /* (s) */

         step = new RFmxSpecAnMX[numberOfSteps];
         absolutePower = new double[numberOfSteps];                           /* (dBm or dBm/Hz) */

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
         rfsgSession.Triggers.ConfigurationListStepTrigger.DigitalEdge.Configure(markerEventTerminalName, RfsgTriggerEdge.RisingEdge);

         instrumentHandle = rfsgSession.GetInstrumentHandle().DangerousGetHandle();
         rfsgSession.Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, waveformFilePath, 0);
         sampleRate = rfsgSession.Arb.Waveforms[waveformName].IQRate;
         papr = rfsgSession.Arb.Waveforms[waveformName].Papr;
         numberOfSamples = (int)(sampleRate * listStepTimerDuration);
         markerLocation = (int)(sampleRate * sweepTimeInterval);
         script = string.Format("script GenerateWaveform\n  repeat forever\n    generate {0} subset(0, {1}) marker0({2})\n" +
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
         SpecAnList = instrSession.GetSpecAnList("ACP_List");
         for (int i = 0; i < numberOfSteps; i++)
         {
            step[i] = SpecAnList.CreateListStep();
            step[i].SetReferenceLevel("", papr + rampPattern[i]);

            if( i == 0)
            {
                step[i].ConfigureIQPowerEdgeTrigger("", "0", step0IQPowerEdgeLevel, RFmxSpecAnMXIQPowerEdgeTriggerSlope.Rising,
                    triggerDelay, RFmxSpecAnMXTriggerMinimumQuietTimeMode.Manual, step0MinimumQuietTime, true);
                step[i].SetIQPowerEdgeTriggerLevelType("", RFmxSpecAnMXIQPowerEdgeTriggerLevelType.Relative);
            }
            else
            {
                step[i].ConfigureDigitalEdgeTrigger("", "TimerEvent", RFmxSpecAnMXDigitalEdgeTriggerEdge.Rising, triggerDelay, true);
                step[i].SetListStepTimerOffset("", listStepTimerOffset);
            }
                step[i].SetListStepTimerDuration("", listStepTimerDuration);
         }
         RFmxSpecAnMX stepAll = SpecAnList.GetListStepAll();
         stepAll.ConfigureFrequency("", centerFrequency);
         stepAll.SetSelectedPorts("", rfsaSelectedPorts);
         stepAll.ConfigureExternalAttenuation("", rfsaExternalAttenuation);
         stepAll.Acp.Configuration.SetSweepTimeAuto("", RFmxSpecAnMXAcpSweepTimeAuto.False);
         stepAll.Acp.Configuration.SetSweepTimeInterval("", sweepTimeInterval);
         stepAll.Acp.Configuration.SetRbwFilterAutoBandwidth("", RFmxSpecAnMXAcpRbwAutoBandwidth.True);
         stepAll.Acp.Configuration.SetRbwFilterBandwidth("", 38.40e3);
         stepAll.Acp.Configuration.SetRbwFilterType("", RFmxSpecAnMXAcpRbwFilterType.FftBased);         
         stepAll.Acp.Configuration.SetFftPadding("", 1.00);
         stepAll.Acp.Configuration.SetFftWindow("", RFmxSpecAnMXAcpFftWindow.FlatTop);
         stepAll.Acp.Configuration.ConfigureCarrierAndOffsets("", integrationBandwidth, numberOfOffsetChannels, channelSpacing);
         offsetString = RFmxSpecAnMX.BuildOffsetString2("", -1);
         stepAll.Acp.Configuration.ConfigureOffsetRrcFilter(offsetString, RFmxSpecAnMXAcpOffsetRrcFilterEnabled.True, 0.220);
         carrierString = RFmxSpecAnMX.BuildCarrierString2("", -1);
         stepAll.Acp.Configuration.ConfigureCarrierRrcFilter(carrierString, RFmxSpecAnMXAcpCarrierRrcFilterEnabled.True, 0.220);
         stepAll.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Acp, false);
         SpecAnList.Initiate("", "");
         rfsgSession.Initiate();
         instrSession.WaitForAcquisitionComplete(timeout);
      }

      void RetrieveResults()
      {
         double[] totalRelativePower = new double[numberOfSteps];
         double[] carrierFrequency = new double[numberOfSteps];
         double[] integrationBandwidth = new double[numberOfSteps];

         offsetMeasurementObject = new OffsetMeasurement[numberOfSteps];
         for (int i = 0; i < numberOfSteps; i++)
         {
            step[i].Acp.Results.FetchOffsetMeasurementArray("", timeout, ref offsetMeasurementObject[i].lowerRelativePower,
               ref offsetMeasurementObject[i].upperRelativePower, ref offsetMeasurementObject[i].lowerAbsolutePower,
               ref offsetMeasurementObject[i].upperAbsolutePower);

            step[i].Acp.Results.FetchCarrierMeasurement("", timeout, out absolutePower[i],
                                                         out totalRelativePower[i],
                                                         out carrierFrequency[i],
                                                         out integrationBandwidth[i]);
         }
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
         if (SpecAnList != null)
         {
            SpecAnList.Dispose();
            SpecAnList = null;
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