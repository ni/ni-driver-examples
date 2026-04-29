//Steps:
//1.  Open NI - RFSG session.
//2. Configure RFSG Selected Ports.
//3. Configure RFSG generation mode to Script. This allows for writing a script that determines the behavior of the generation.
//4.  Configure RFSG frequency reference.
//5.  Configure RFSG configuration settled event to the device scriptTrigger0, to make sure generation starts only after
//    RFSG configuration settled.
//6.  Configure RFSG to advance upon receipt of RFSA Ready for Advance event through PXI trigger line
//7.  Configure frequency and external gain of RF output signal.
//8.  Get terminal name for marker0and assign to the RFSA Reference Trigger Digital Edge source
//9.  Create a Configuration List. Pass Power Level in the Configuration List Properties parameter to be able to configure
//    Power Level in each step that we create. The Set As Active List parameter in this VI defaults to true, this will set the
//    Active Configuration List property to the name of the created configuration list.Once the Active Configuration List
//    is set, using a property node to access Power Level will modify the property for this configuration list.
//10.  Create a Configuration List Step.The Set As Active Step parameter in this VI defaults to true, this will set the Active
//    Configuration List Step property to the created configuration list step index.Once the Active Configuration List
//    Step is set, using a property node to access Power Level will modify the property for this configuration list step in
//    the configuration list indicated by the Active Configuration List property.
//11. Configure the Power Level for the Active Configuration List Step in the Active Configuration List.
//12. Read waveform from file and download Waveform from file to RFSG.
//13. Write script to generate the waveform specified in the script.This script is programmed to generate waveform
//    continuously and generate a marker at the start of the waveform(sample 0).
//14. Open a new RFmx Session.
//15. Configure the Frequency Reference properties(Clock Source and Clock Frequency).
//16. Export RFSA Ready for Advance event to PXI trigger line
//17. Create RFmx SpecAn List.
//18. Create RFmx SpecAn List Step.
//19. Configure the Reference Level for the Specified List Step in the SpecAn List.
//20. Configure Center Frequency, Selected Ports, External Attenuation, Trigger Type and Trigger Parameters for all Configuration List Step.
//21. Configure Sweep Time, RBW Filter, FFT parameters for all List Steps.
//22. Configure Integration BW of the Carrier channel, Number of Offset Channelsand Channel Spacing for all List Steps.
//23. Configure Carrier and Offset RRC Filter for all List Steps.
//24. Select ACP measurement and disable traces for all List Steps.
//25. Initiate ACP measurement for List.
//26. Initiate signal generation.
//27. Wait for Acquisition to complete
//28. Stop signal generation.
//29. Delete the Configuration List to avoid committing the Configuration List to the hardware with subsequent
//    calls to Commit. Deleting the list will reset the Active Configuration List.
//30. Fetch ACP measurement Results for all Configuration List Steps one by one.
//31. Delete RFmx SpecAn List.
//32. Close the RFmx Session.
//33. Close the RFSG session. 
//    It is recommended to clear the waveform before closing RFSG session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsgPlayback;

namespace NationalInstruments.Examples.RFmxSpecAnListModeWithSGSAHandshaking
{
   public class RFmxSpecAnListModeWithSGSAHandshaking
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

      RFmxSpecAnMXDigitalEdgeTriggerEdge digitalEdge;
      double triggerDelay;

      int numberOfSteps;

      double startReferenceLevel;
      double stopReferenceLevel;

      double[] rampPattern;
      double timeout;
      string script;
      RFmxSpecAnMX[] step;
      double[] absolutePower;

      string configurationSettledEvenTerminalName;
      string markerEventTerminalName;

      RFmxSpecAnMXAcpSweepTimeAuto sweepTimeAuto;
      double sweepTimeInterval;
      RFmxSpecAnMXAcpRbwAutoBandwidth rbwAutoBandwidth;
      RFmxSpecAnMXAcpRbwFilterType rbwFilterType;
      double rbwBandwidth;
      double fftPading;
      RFmxSpecAnMXAcpFftWindow fftWindow;
      double integrationBandwidth;

      string offsetString;
      string carrierString;

      int numberOfOffsetChannels;
      double channelSpacing;        /* Hz */

      RFmxSpecAnMXAcpOffsetRrcFilterEnabled offsetRRCEnabled;
      RFmxSpecAnMXAcpCarrierRrcFilterEnabled carrierRRCEnabled;
      double offsetRRCAlpha;
      double carrierRRCAlpha;

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

         digitalEdge = RFmxSpecAnMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                                                  /* (s) */

         numberOfSteps = 10;

         startReferenceLevel = -20.0;                                         /* (dBm) */
         stopReferenceLevel = 0.0;                                            /* (dBm) */

         sweepTimeAuto = RFmxSpecAnMXAcpSweepTimeAuto.False;
         sweepTimeInterval = 666.67e-6;                                       /* (us) */
         rbwAutoBandwidth = RFmxSpecAnMXAcpRbwAutoBandwidth.True;
         rbwFilterType = RFmxSpecAnMXAcpRbwFilterType.FftBased;
         rbwBandwidth = 38.400e3;
         fftPading = 1.00;
         fftWindow = RFmxSpecAnMXAcpFftWindow.FlatTop;
         integrationBandwidth = 3.840e6;

         numberOfOffsetChannels = 2;

         channelSpacing = 5e6;
         offsetRRCEnabled = RFmxSpecAnMXAcpOffsetRrcFilterEnabled.True;
         carrierRRCEnabled = RFmxSpecAnMXAcpCarrierRrcFilterEnabled.True;
         offsetRRCAlpha = 0.220;
         carrierRRCAlpha = 0.220;

         rampPattern = new double[numberOfSteps];
         LinearRampPattern(startReferenceLevel, stopReferenceLevel, numberOfSteps, false, ref rampPattern);

         timeout = 10.0;                                                      /* (s) */

         script = string.Format("script GenerateWaveform\n  repeat forever\n    generate {0} marker0(0)\n" +
            "  wait until scripttrigger0\n   end repeat\n  end script", waveformName);

         step = new RFmxSpecAnMX[numberOfSteps];
         absolutePower = new double[numberOfSteps];                           /* (dBm or dBm/Hz) */

      }

      void ConfigureRfsg()
      {
         rfsgSession = new NIRfsg(rfsgResourceName, true, false);
         rfsgSession.SignalPath.SelectedPorts = rfsgSelectedPorts;
         rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
         rfsgSession.FrequencyReference.Configure(rfsgFrequencyReferenceSource, rfsgFrequency);
         configurationSettledEvenTerminalName = rfsgSession.DeviceEvents.ConfigurationSettledEvent.TerminalName;
         rfsgSession.Triggers.ScriptTriggers[0].DigitalEdge.Configure(configurationSettledEvenTerminalName,
            RfsgTriggerEdge.RisingEdge);
         rfsgSession.Triggers.ConfigurationListStepTrigger.DigitalEdge.Configure(
            RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine0, RfsgTriggerEdge.RisingEdge);
         rfsgSession.RF.ExternalGain = -1 * rfsgExternalAttenuation;
         rfsgSession.RF.Frequency = centerFrequency;
         markerEventTerminalName = rfsgSession.DeviceEvents.MarkerEvents[0].TerminalName;
         RfsgConfigurationListProperties[] properties = new RfsgConfigurationListProperties[1]
            { RfsgConfigurationListProperties.PowerLevel };
         rfsgSession.BasicConfigurationList.CreateConfigurationList("PowerLevelList", properties, true);
         for (int i = 0; i < numberOfSteps; i++)
         {
            rfsgSession.BasicConfigurationList.CreateStep(true);
            rfsgSession.RF.PowerLevel = rampPattern[i];
         }
         instrumentHandle = rfsgSession.GetInstrumentHandle().DangerousGetHandle();
         rfsgSession.Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, waveformFilePath, 0);
         rfsgSession.Arb.Scripting.WriteScript(script);
        }

      void ConfigureRFmx()
      {
         instrSession = new RFmxInstrMX(rfsaResourceName, "");
         instrSession.ConfigureFrequencyReference("", rfsaFrequencyReferenceSource, rfsaFrequency);
         instrSession.ExportSignal(RFmxInstrMXExportSignalSource.ReadyForAdvanceEvent, RFmxInstrMXConstants.PxiTriggerLine0);
         SpecAnList = instrSession.GetSpecAnList("ACP_List");
         for (int i = 0; i < numberOfSteps; i++)
         {
            step[i] = SpecAnList.CreateListStep();
            step[i].SetReferenceLevel("", rampPattern[i]);
         }
         RFmxSpecAnMX stepAll = SpecAnList.GetListStepAll();
         stepAll.ConfigureFrequency("", centerFrequency);
         stepAll.SetSelectedPorts("", rfsaSelectedPorts);
         stepAll.ConfigureDigitalEdgeTrigger("", markerEventTerminalName, digitalEdge, triggerDelay, true);
         stepAll.ConfigureExternalAttenuation("", rfsaExternalAttenuation);
         stepAll.Acp.Configuration.SetSweepTimeAuto("", sweepTimeAuto);
         stepAll.Acp.Configuration.SetSweepTimeInterval("", sweepTimeInterval);
         stepAll.Acp.Configuration.SetRbwFilterAutoBandwidth("", rbwAutoBandwidth);
         stepAll.Acp.Configuration.SetRbwFilterType("", rbwFilterType);
         stepAll.Acp.Configuration.SetRbwFilterBandwidth("", rbwBandwidth);
         stepAll.Acp.Configuration.SetFftPadding("", fftPading);
         stepAll.Acp.Configuration.SetFftWindow("", fftWindow);
         stepAll.Acp.Configuration.ConfigureCarrierAndOffsets("", integrationBandwidth, numberOfOffsetChannels, channelSpacing);
         offsetString = RFmxSpecAnMX.BuildOffsetString2("", -1);
         stepAll.Acp.Configuration.ConfigureOffsetRrcFilter(offsetString, offsetRRCEnabled, offsetRRCAlpha);
         carrierString = RFmxSpecAnMX.BuildCarrierString2("", -1);
         stepAll.Acp.Configuration.ConfigureCarrierRrcFilter(carrierString, carrierRRCEnabled, carrierRRCAlpha);
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