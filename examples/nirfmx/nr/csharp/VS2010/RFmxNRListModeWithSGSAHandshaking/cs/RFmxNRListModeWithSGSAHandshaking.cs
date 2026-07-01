//Steps:
//1. Open NI-RFSG session.
//2. Configure RFSG Selected Ports and GenerationMode to Script.
//3. Configure RFSG frequency reference.
//4. Configure RFSG configuration settled event to the device scriptTrigger0, to make sure generation starts only after
//   RFSG configuration settled.
//5. Configure RFSG to advance upon receipt of RFSA Ready for Advance event through PXI trigger line
//6. Configure frequency, external gain, Power Level Type and Pre-filter Gain of RF output signal. 
//7. Get terminal name for marker0 and assign to the RFSA Reference Trigger Digital Edge source 
//8. Create a Configuration List. Pass Power Level in the Configuration List Properties parameter to be able to configure
//   Power Level in each step that we create. The Set As Active List parameter in this VI defaults to true, this will set the
//   Active Configuration List property to the name of the created configuration list.Once the Active Configuration List
//   is set, using a property node to access Power Level will modify the property for this configuration list.
//9. Create a Configuration List Step.The Set As Active Step parameter in this VI defaults to true, this will set the Active
//   Configuration List Step property to the created configuration list step index.Once the Active Configuration List
//   Step is set, using a property node to access Power Level will modify the property for this configuration list step in
//   the configuration list indicated by the Active Configuration List property.
//10. Configure the Power Level for the Active Configuration List Step in the Active Configuration List.
//11. Configure RFSG LO Source to Automatic SG SA Shared.
//12. Read waveform from file and download Waveform from file to RFSG.
//13. Retrieve the waveform PAPR, Signal Bandwidth and IQ rate. Add the waveform PAPR to the RFSA reference level while configuring to
//    every list step.
//14. Configure RFSG Signal Bandwidth, IQ rate, and PAPR. With the signal bandwidth configured and the Upconverter Frequency Offset 
//       Mode set to Automatic by default, the RFSG LO is placed outside the signal if the signal bandwidth is less than half of the device 
//       instantaneous bandwidth; otherwise, the LO is placed at the center of the signal.
//15. Write script to generate the waveform specified in the script. This script is programmed to generate waveform
//    continuously and generate a marker at the start of the waveform (sample 0).
//16. Open a new RFmx Session.
//17. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
//18. Export RFSA Ready for Advance event to PXI trigger line.
//19. Create RFmx NR List.
//20. Create RFmx NR List Step.
//21. Configure the Reference Level for the Specified List Step in the NR List.
//22. Configure personality and measurement parameters for a List Step.
//23. Configure Center Frequency, Selected Ports, External Attenuation, Trigger Type and Trigger Parameters for all List Step.
//24. Configure Link Direction, Frequency Range, CC bandwidth, Cell ID, Band, and BWP Subcarrier Spacing for all List Step.
//25. Set LO Leakage Avoidance Enabled to True and Automatic SG SA Shared LO to Enabled. Enabling LO Leakage Avoidance causes RFmx
//    to place the SA LO outside the measurement bandwidth, if the measurement bandwidth is less than half of the device instantaneous
//    bandwidth; otherwise, the LO is placed at the center of the signal.
//26. Select ModAcc measurement and disable traces for all List Step.
//27. Initiate ModAcc measurement for List.
//28. Initiate signal generation.
//29. Wait for Acquisition to complete.
//30. Stop signal generation.
//31. Delete the Configuration List to avoid committing the Configuration List to the hardware with subsequent
//    calls to Commit. Deleting the list will reset the Active Configuration List.
//32. Fetch ModAcc measurement Results for all Configuration List Steps one by one.
//33. Delete RFmx NR List.
//34. Close the RFmx Session.
//35. Close the RFSG session. 
//    It is recommended to clear the waveform before closing RFSG session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;
using NationalInstruments.ModularInstruments.NIRfsg;

namespace NationalInstruments.Examples.RFmxNRListModeWithSGSAHandshaking
{
   public class RFmxNRListModeWithSGSAHandshaking
   {
      RFmxInstrMX instrSession;
      RFmxNRMXList NRList;
      NIRfsg rfsgSession;

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

      RFmxNRMXDigitalEdgeTriggerEdge digitalEdge;
      double triggerDelay;

      int numberOfSteps;

      double startReferenceLevel;
      double stopReferenceLevel;

      RFmxNRMXFrequencyRange frequencyRange;
      double carrierBandwidth;
      double subcarrierSpacing;
      int band;
      int cellID;

      double[] rampPattern;
      double timeout;
      string script;
      double papr;
      RFmxNRMX[] step;
      string configurationSettledEvenTerminalName;
      string markerEventTerminalName;

      double[] compositeRmsEvmMean;
      double[] compositePeakEvmMaximum;


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
         centerFrequency = 3.5e9;                                             /* (Hz) */

         rfsgResourceName = "RFSG";
         rfsgSelectedPorts = "";
         waveformFilePath = "NR_FR2_UL_SISO_CC-1_BW-50MHz_SCS-120kHz.tdms";
         waveformName = "Wfm";

         rfsgExternalAttenuation = 0.0;                                       /* (dB) */

         rfsgFrequencyReferenceSource = RfsgFrequencyReferenceSource.OnboardClock;
         rfsgFrequency = 10.0e6;                                              /* (Hz) */

         rfsaResourceName = "RFSA";
         rfsaSelectedPorts = "";
         rfsaExternalAttenuation = 0.0;                                       /* (dB) */

         rfsaFrequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         rfsaFrequency = 10.0e6;                                              /* (Hz) */

         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                                                  /* (s) */

         numberOfSteps = 10;

         startReferenceLevel = -20.0;                                         /* (dBm) */
         stopReferenceLevel = 0.0;                                            /* (dBm) */

         frequencyRange = RFmxNRMXFrequencyRange.Range2;
         carrierBandwidth = 50e6;                                             /* (Hz) */
         subcarrierSpacing = 120e3;                                           /* (Hz) */
         band = 257;
         cellID = 0;

         rampPattern = new double[numberOfSteps];
         LinearRampPattern(startReferenceLevel, stopReferenceLevel, numberOfSteps, false, ref rampPattern);

         timeout = 10.0;                                                      /* (s) */

         script = string.Format("script GenerateWaveform\n  repeat forever\n    generate {0} marker0(0)\n" +
            "  wait until scripttrigger0\n   end repeat\n  end script", waveformName);

         step = new RFmxNRMX[numberOfSteps];
         compositeRmsEvmMean = new double[numberOfSteps];
         compositePeakEvmMaximum = new double[numberOfSteps];
      }

      void ConfigureRfsg()
      {
         rfsgSession = new NIRfsg(rfsgResourceName, true, false);
         rfsgSession.SignalPath.SelectedPorts = rfsgSelectedPorts;
         rfsgSession.FrequencyReference.Configure(rfsgFrequencyReferenceSource, rfsgFrequency);
         configurationSettledEvenTerminalName = rfsgSession.DeviceEvents.ConfigurationSettledEvent.TerminalName;
         rfsgSession.Triggers.ScriptTriggers[0].DigitalEdge.Configure(configurationSettledEvenTerminalName,
            RfsgTriggerEdge.RisingEdge);
         rfsgSession.Triggers.ConfigurationListStepTrigger.DigitalEdge.Configure(
            RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine0, RfsgTriggerEdge.RisingEdge);
         rfsgSession.RF.ExternalGain = -1 * rfsgExternalAttenuation;
         rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
         rfsgSession.Arb.PreFilterGain = -1.5;
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
         rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
         rfsgSession.Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, waveformFilePath, 0);
         papr = rfsgSession.Arb.Waveforms[waveformName].Papr;
         double waveformIqRate = rfsgSession.Arb.Waveforms[waveformName].IQRate;
         double waveformSignalBandwidth = rfsgSession.Arb.Waveforms[waveformName].SignalBandwidth;
         rfsgSession.Arb.IQRate = waveformIqRate;
         rfsgSession.Arb.SignalBandwidth = waveformSignalBandwidth;
         rfsgSession.RF.PeakPowerAdjustment = papr;
         rfsgSession.RF.LocalOscillator.Source = RfsgLocalOscillatorSource.AutomaticSGSAShared;
         rfsgSession.Arb.Scripting.WriteScript(script);
      }

      void ConfigureRFmx()
      {
         instrSession = new RFmxInstrMX(rfsaResourceName, "");
         instrSession.ConfigureFrequencyReference("", rfsaFrequencyReferenceSource, rfsaFrequency);
         instrSession.ExportSignal(RFmxInstrMXExportSignalSource.ReadyForAdvanceEvent, RFmxInstrMXConstants.PxiTriggerLine0);
         NRList = instrSession.GetNRList("ModAcc_List");
         for (int i = 0; i < numberOfSteps; i++)
         {
            step[i] = NRList.CreateListStep();
            step[i].SetReferenceLevel("", papr + rampPattern[i]);
         }
         RFmxNRMX stepAll = NRList.GetListStepAll();
         stepAll.ConfigureFrequency("", centerFrequency);
         stepAll.SetSelectedPorts("", rfsaSelectedPorts);
         stepAll.ConfigureDigitalEdgeTrigger("", markerEventTerminalName, digitalEdge, triggerDelay, true);
         stepAll.ConfigureExternalAttenuation("", rfsaExternalAttenuation);
         stepAll.SetLinkDirection("", RFmxNRMXLinkDirection.Uplink);
         stepAll.SetFrequencyRange("", frequencyRange);
         stepAll.ComponentCarrier.SetBandwidth("", carrierBandwidth);
         stepAll.ComponentCarrier.SetCellID("", cellID);
         stepAll.SetBand("", band);
         stepAll.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing);

         instrSession.SetLOSource("", RFmxInstrMXConstants.LOSourceAutomaticSGSAShared);
         instrSession.SetLOLeakageAvoidanceEnabled("", RFmxInstrMXLOLeakageAvoidanceEnabled.True);

         stepAll.SelectMeasurements("", RFmxNRMXMeasurementTypes.ModAcc, false);
         NRList.Initiate("", "");
         rfsgSession.Initiate();
         instrSession.WaitForAcquisitionComplete(timeout);
      }

      void RetrieveResults()
      {
         for (int i = 0; i < numberOfSteps; i++)
         {
            step[i].ModAcc.Results.GetCompositeRmsEvmMean("", out compositeRmsEvmMean[i]);
            step[i].ModAcc.Results.GetCompositePeakEvmMaximum("", out compositePeakEvmMaximum[i]);
         }     
      }


      void PrintResults()
      {
         Console.WriteLine("------------------Measurements------------------\n");
         Console.WriteLine("Composite RMS EVM Mean (%)");
         for (int i = 0; i < numberOfSteps; i++)
         {
            Console.WriteLine("Step{0}    : {1}", i, compositeRmsEvmMean[i]);
         }
         Console.WriteLine("\nComposite Peak EVM Maximum (%)");
         for (int i = 0; i < numberOfSteps; i++)
         {
            Console.WriteLine("Step{0}    : {1}", i, compositePeakEvmMaximum[i]);
         }
      }

      void CloseSession()
      {
         if (NRList != null)
         {
            NRList.Dispose();
            NRList = null;
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
